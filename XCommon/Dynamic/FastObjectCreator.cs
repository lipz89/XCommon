using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using XCommon.Exception;

namespace XCommon.Dynamic
{
    /// <summary>
    /// 快速创建对象的工具类
    /// </summary>
    public static class FastObjectCreator
    {
        private delegate object CreateOjectHandler(object[] parameters);
        private static readonly Hashtable creatorCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// 根据参数快速创建一个对象
        /// </summary>
        /// <param name="type">对象的类型</param>
        /// <param name="parameters">对象的构造函数要求的参数</param>
        /// <returns>创建的对象</returns>
        public static object CreateObject(Type type, params object[] parameters)
        {
            int token = type.MetadataToken;
            Type[] parameterTypes = GetParameterTypes(ref token, parameters);

            var key = token ^ type.FullName.GetHashCode();

            lock (creatorCache.SyncRoot)
            {
                CreateOjectHandler ctor = creatorCache[key] as CreateOjectHandler;
                if (ctor == null)
                {
                    ctor = CreateHandler(type, parameterTypes);
                    creatorCache.Add(key, ctor);
                }
                return ctor.Invoke(parameters);
            }
        }

        /// <summary>
        /// 生产一个创建对象的委托
        /// </summary>
        /// <param name="type">创建对象的类型</param>
        /// <param name="paramsTypes">创建对象的构造函数的参数类型列表</param>
        /// <returns>创建对象的委托</returns>
        private static CreateOjectHandler CreateHandler(Type type, Type[] paramsTypes)
        {
            DynamicMethod method = new DynamicMethod("DynamicCreateObject", typeof(object),
                                                     new Type[] { typeof(object[]) }, typeof(CreateOjectHandler).Module);

            ConstructorInfo constructor = type.GetConstructor(paramsTypes);

            Check.NotNull(constructor, null, "没找到与指定参数匹配的构造函数");

            ILGenerator il = method.GetILGenerator();

            for (int i = 0; i < paramsTypes.Length; i++)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Ldelem_Ref);
                if (paramsTypes[i].IsValueType)
                {
                    il.Emit(OpCodes.Unbox_Any, paramsTypes[i]);
                }
                else
                {
                    il.Emit(OpCodes.Castclass, paramsTypes[i]);
                }
            }
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);

            return (CreateOjectHandler)method.CreateDelegate(typeof(CreateOjectHandler));
        }

        /// <summary>
        /// GetParameterTypes
        /// </summary>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private static Type[] GetParameterTypes(ref int token, params object[] parameters)
        {
            if (parameters == null) return new Type[0];
            Type[] values = new Type[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                values[i] = parameters[i].GetType();
                token = token * 13 + values[i].MetadataToken;
            }
            return values;
        }
    }
}
