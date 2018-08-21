using System;
using System.Reflection;
using XCommon.Utils;

namespace XCommon.Dynamic
{
    /// <summary>
    /// 反射方法工具类
    /// </summary>
    public static class Reflaction
    {
        /// <summary>
        /// 访问一个实例的公开或非公开属性
        /// </summary>
        /// <param name="obj">要访问的属性所属的实例</param>
        /// <param name="memberName">要访问的属性的名称</param>
        /// <param name="throwWhenNull">指示无法访问是否抛出异常</param>
        /// <returns>属性的值</returns>
        public static object GetMemberValue(object obj, string memberName, bool throwWhenNull = true)
        {
            if (throwWhenNull)
            {
                Check.NotNull(obj, nameof(obj));
                Check.NotEmpty(memberName, nameof(memberName));
            }
            else
            {
                if (obj == null || string.IsNullOrWhiteSpace(memberName))
                {
                    return null;
                }
            }

            var type = obj.GetType();
            var member = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (member != null)
            {
                return member.GetValue(obj);
            }
            var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (field != null)
            {
                return field.GetValue(obj);
            }

            if (throwWhenNull)
            {
                throw new System.Exception("在指定的对象类型中没有发现名称为" + memberName + "的属性或字段。");
            }
            return null;
        }
        /// <summary>
        /// 访问一个实例的公开或非公开属性
        /// </summary>
        /// <typeparam name="T">要访问属性的类型</typeparam>
        /// <param name="obj">要访问的属性所属的实例</param>
        /// <param name="memberName">要访问的属性的名称</param>
        /// <param name="throwWhenNull">指示无法访问是否抛出异常</param>
        /// <returns>属性的值</returns>
        public static T GetMemberValue<T>(object obj, string memberName, bool throwWhenNull = true)
        {
            var rst = GetMemberValue(obj, memberName, throwWhenNull);
            return (T)rst;
        }
        /// <summary>
        /// 调用一个实例的公开或非公开的方法
        /// </summary>
        /// <param name="obj">要调用的方法所属的实例</param>
        /// <param name="methodName">要调用的方法的名称</param>
        /// <param name="args">要调用的方法的参数</param>
        /// <returns>调用结果</returns>
        public static object Invoke(object obj, string methodName, params object[] args)
        {
            Check.NotNull(obj, nameof(obj));
            Check.NotEmpty(methodName, nameof(methodName));

            var type = obj.GetType();
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            if (method != null)
            {
                return method.Invoke(method.IsStatic ? null : obj, args);
            }

            return null;
        }
        /// <summary>
        /// 调用一个实例的公开或非公开的泛型方法
        /// </summary>
        /// <param name="obj">要调用的方法所属的实例</param>
        /// <param name="methodName">要调用的方法的名称</param>
        /// <param name="types">要调用的方法的泛型类型列表</param>
        /// <param name="args">要调用的方法的参数</param>
        /// <returns></returns>
        public static object InvokeGeneric(object obj, string methodName, Type[] types, params object[] args)
        {
            Check.NotNull(obj, nameof(obj));
            Check.NotEmpty(methodName, nameof(methodName));

            var type = obj.GetType();
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            method = method?.MakeGenericMethod(types);
            if (method != null)
            {
                return method.Invoke(method.IsStatic ? null : obj, args);
            }

            return null;
        }
    }
}