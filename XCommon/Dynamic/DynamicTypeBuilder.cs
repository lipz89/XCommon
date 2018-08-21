using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using XCommon.Extenstions;
using XCommon.Utils;

namespace XCommon.Dynamic
{
    /// <summary>
    /// 动态类型创建类
    /// </summary>
    public static class DynamicTypeBuilder
    {
        private static readonly AssemblyName assemblyName = new AssemblyName() { Name = "DynamicTypes" };
        private static readonly ModuleBuilder moduleBuilder;
        private static readonly Dictionary<string, Type> builtTypes = new Dictionary<string, Type>();

        static DynamicTypeBuilder()
        {
            moduleBuilder = Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run).DefineDynamicModule(assemblyName.Name);
        }

        private static string GetTypeKey(Dictionary<string, Type> fields)
        {
            string key = "T<";
            key += string.Join(",", fields.Select(x => x.Key + "_" + x.Value.GetFullName()));
            key += '>';
            return key;
        }
        /// <summary>
        /// 通过指定的字段和类型生成一个动态类型
        /// </summary>
        /// <param name="fields">指定的字段和类型字典</param>
        /// <returns>只包含与指定的字段和类型相匹配属性的类型</returns>
        public static Type GetDynamicType(Dictionary<string, Type> fields)
        {
            Check.NotNullOrEmpty(fields, nameof(fields));

            try
            {
                Monitor.Enter(builtTypes);
                string className = GetTypeKey(fields);

                if (!builtTypes.ContainsKey(className))
                {
                    TypeBuilder typeBuilder = moduleBuilder.DefineType(className, TypeAttributes.Public | TypeAttributes.Class | TypeAttributes.Serializable);

                    foreach (var field in fields)
                        typeBuilder.DefineField(field.Key, field.Value, FieldAttributes.Public);

                    builtTypes[className] = typeBuilder.CreateType();
                }

                return builtTypes[className];
            }
            finally
            {
                Monitor.Exit(builtTypes);
            }
        }


        private static string GetTypeKey(IEnumerable<PropertyInfo> fields)
        {
            return GetTypeKey(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }
        /// <summary>
        /// 根据指定的属性生成一个动态类型
        /// </summary>
        /// <param name="fields">指定的属性</param>
        /// <returns>包含与指定属性匹配属性的类型</returns>
        public static Type GetDynamicType(IEnumerable<PropertyInfo> fields)
        {
            return GetDynamicType(fields.ToDictionary(f => f.Name, f => f.PropertyType));
        }

        /// <summary>
        /// 创建一个动态表达式
        /// </summary>
        /// <typeparam name="T">动态表达式参数的类型</typeparam>
        /// <param name="property">创建的动态表达式要访问的属性</param>
        /// <returns>动态表达式</returns>
        public static Expression<Func<T, dynamic>> CreateFuncDynamic<T>(PropertyInfo property)
        {
            return CreateFuncDynamic<T>(new List<PropertyInfo> { property });
        }
        /// <summary>
        /// 创建一个动态表达式
        /// </summary>
        /// <typeparam name="T">动态表达式参数的类型</typeparam>
        /// <param name="properties">创建的动态表达式要访问的属性</param>
        /// <returns>动态表达式</returns>
        public static Expression<Func<T, dynamic>> CreateFuncDynamic<T>(IEnumerable<PropertyInfo> properties)
        {
            Type source = typeof(T);
            Dictionary<string, PropertyInfo> sourceProperties = properties.ToDictionary(x => x.Name);

            Type dynamicType = GetDynamicType(sourceProperties.Values);
            ParameterExpression sourceItem = Expression.Parameter(source, "t");
            IEnumerable<MemberBinding> bindings = dynamicType.GetFields()
                .Select(p => Expression.Bind(p, Expression.Property(sourceItem, sourceProperties[p.Name])));

            var @new = Expression.New(dynamicType);
            var init = Expression.MemberInit(@new, bindings);
            Expression<Func<T, dynamic>> selector = Expression.Lambda<Func<T, dynamic>>(init, sourceItem);
            return selector;
        }
    }
}