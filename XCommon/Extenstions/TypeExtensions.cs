using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using XCommon.Exception;

namespace XCommon.Extenstions
{
    /// <summary>
    /// 类型扩展方法
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 返回一个类型的不可空类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetNonNullableType(this Type type)
        {
            if (IsNullableType(type))
                return type.GetGenericArguments()[0];

            return type;
        }

        /// <summary>
        /// 判断一个类型是空或void
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullOrVoid(this Type type)
        {
            return type == null || type == typeof(void);
        }

        /// <summary>
        /// 判断一个类型是Nullable
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableType(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 判断一个类型具备一个接口
        /// </summary>
        /// <param name="type"></param>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            Check.NotNull(type, nameof(type));
            Check.NotNull(interfaceType, nameof(interfaceType));
            Check.NotMatch(() => !interfaceType.IsInterface, nameof(interfaceType), "参数必须为接口类型。");


            var its = type.GetInterfaces();
            foreach (var it in its)
            {
                if (it == interfaceType)
                {
                    return true;
                }
                if (interfaceType.IsGenericTypeDefinition && it.IsGenericType && it.GetGenericTypeDefinition() == interfaceType)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断一个类型具备一个接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool HasInterface<T>(this Type type)
        {
            return type.HasInterface(typeof(T));
        }

        #region GetFullName

        // Methods
        private static string ExtractGenericArguments(this IEnumerable<Type> names)
        {
            StringBuilder builder = new StringBuilder();
            foreach (Type type in names)
            {
                if (builder.Length > 1)
                {
                    builder.Append(", ");
                }

                builder.Append(type.GetFullName());
            }

            return builder.ToString();
        }

        private static string ExtractName(string name)
        {
            int length = name.IndexOf("`", StringComparison.Ordinal);
            if (length > 0)
            {
                name = name.Substring(0, length);
            }

            return name;
        }

        /// <summary>
        /// 返回一个方法包含参数的完整签名
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetSignName(this MethodInfo method)
        {
            Check.NotNull(method, nameof(method));
            var sign = method.ReturnType.GetFullName() + " ";
            sign += method.GetFullName();
            var ps = method.GetParameters();
            sign += "(";
            if (ps.Any())
            {
                foreach (var info in ps)
                {
                    if (info.ParameterType.IsByRef)
                    {
                        if (info.Attributes.HasFlag(ParameterAttributes.Out))
                        {
                            sign += "out ";
                        }
                        else
                        {
                            sign += "ref ";
                        }
                    }
                    else if (info.IsOptional)
                    {
                        sign += "[Optional]";
                    }

                    sign += info.ParameterType.GetFullName();
                    sign += ", ";
                }

                sign = sign.RemoveRight(1);
            }

            sign += ")";
            return sign;
        }

        /// <summary>
        /// 返回一个方法的完全名称
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public static string GetFullName(this MethodInfo method)
        {
            Check.NotNull(method, nameof(method));

            if (!method.IsGenericMethod)
            {
                return method.Name;
            }

            return ExtractName(method.Name) + "<" + ExtractGenericArguments(method.GetGenericArguments()) + ">";
        }

        private static string GetFullNameCore(this Type type)
        {
            Check.NotNull(type, nameof(type));

            if (type.IsArray)
            {
                var n = type.Name;
                var etype = type.GetElementType();
                return n.Replace(etype.Name, etype.GetFullName());
            }

            var name = type.FullName ?? type.Name;
            if (type.IsGenericType)
            {
                var gtp = ExtractGenericArguments(type.GetGenericArguments());
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return gtp + "?";
                }

                var gt = ExtractName(name);
                return gt + "<" + gtp + ">";
            }

            if (type.IsByRef)
            {
                return name.RemoveRight(1);
            }

            return name;
        }

        /// <summary>
        /// 返回一个类型的名称部分，不包含泛型类型参数部分
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetGenericName(this Type type)
        {
            Check.NotNull(type, nameof(type));

            if (type.IsGenericType)
            {
                return ExtractName(type.Name);
            }

            return string.Empty;
        }

        private static readonly Dictionary<Type, string> simpleName = new Dictionary<Type, string>
        {
            {typeof(object), "object"},
            {typeof(string), "string"},
            {typeof(bool), "bool"},
            {typeof(char), "char"},
            {typeof(int), "int"},
            {typeof(uint), "uint"},
            {typeof(byte), "byte"},
            {typeof(sbyte), "sbyte"},
            {typeof(short), "short"},
            {typeof(ushort), "ushort"},
            {typeof(long), "long"},
            {typeof(ulong), "ulong"},
            {typeof(float), "float"},
            {typeof(double), "double"},
            {typeof(decimal), "decimal"}
        };

        /// <summary>
        /// 返回一个类型的完整名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetFullName(this Type type)
        {
            Check.NotNull(type, nameof(type));

            if (simpleName.ContainsKey(type))
            {
                return simpleName[type];
            }

            return type.GetFullNameCore();
        }

        #endregion

        #region Anonymous

        /// <summary>
        /// 判断一个类型是匿名类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(this Type type)
        {
            Check.NotNull(type, nameof(type));
            Type d = type;
            if (type.IsGenericType)
            {
                d = type.GetGenericTypeDefinition();
            }

            if (d.IsClass && d.IsSealed && d.Attributes.HasFlag(TypeAttributes.NotPublic))
            {
                var attributes = d.GetCustomAttribute<CompilerGeneratedAttribute>(false);
                if (attributes != null)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 判断一个类型是匿名类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsAnonymousType<T>()
        {
            return IsAnonymousType(typeof(T));
        }

        #endregion

        /// <summary>
        /// 返回一个成员的类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Type GetMemberType(this MemberInfo value)
        {
            if (value is FieldInfo)
            {
                return ((FieldInfo)value).FieldType;
            }
            else if (value is PropertyInfo)
            {
                return ((PropertyInfo)value).PropertyType;
            }
            else if (value is MethodInfo)
            {
                return ((MethodInfo)value).ReturnType;
            }
            throw new NotSupportedException();
        }
    }
}
