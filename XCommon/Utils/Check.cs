using System;
using System.Collections;
using XCommon.Extenstions;
using XCommon.Utils;

namespace XCommon.Utils
{
    /// <summary>
    /// 检查类
    /// </summary>
    public static class Check
    {
        /// <summary>
        /// 如果一个值满足指定条件,则抛出异常
        /// </summary>
        /// <param name="precondition"></param>
        /// <param name="message"></param>
        /// <param name="argName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public static void NotMatch(Func<bool> precondition, string argName = null, string message = null)
        {
            if (precondition())
            {
                if (argName == null)
                    throw Errors.Application(message);
                else
                    throw Errors.ArgumentError(argName, message);
            }
        }
        /// <summary>
        /// 如果一个值是该类型的默认值,则抛出异常
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="argName"></param>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public static void NotDefault<T>(T value, string argName = null, string message = null)
        {
            var dft = default(T);
            if (value.Equals(dft))
            {
                message = message ?? "值不能为类型的默认值。";
                if (argName == null)
                    throw Errors.Application(message);
                else
                    throw Errors.ArgumentError(argName, message);
            }
        }

        /// <summary>
        /// 检查输入的参数 <paramref name="value"/> 是否为 Null。
        /// 如果 <paramref name="value"/> 值为 Null，则抛出 <see cref="ArgumentNullException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <typeparam name="T"><paramref name="value"/> 参数的类型。</typeparam>
        /// <param name="value">被检查的参数值。</param>
        /// <param name="message"></param>
        /// <param name="argName">被检查的参数名称。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="value"/> 值为 Null，则抛出该异常。</exception>
        /// <exception cref="ApplicationException"></exception>
        public static void NotNull<T>(T value, string argName = null, string message = null) where T : class
        {
            if (value == null)
            {
                message = message ?? "值不能为 null。";
                if (argName == null)
                    throw Errors.Application(message);
                else
                    throw Errors.ArgumentError(argName, message);
            }
        }

        /// <summary>
        /// 检查输入的参数是否为 Null、空或者空白字符串组成。
        /// 如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出 <see cref="ArgumentException"/> 异常；否则返回 <paramref name="value"/> 值本身。
        /// </summary>
        /// <param name="value">被检查的参数值。</param>
        /// <param name="message"></param>
        /// <param name="argName">被检查的参数名称。</param>
        /// <returns>如果 <paramref name="value"/> 值不为 Null、空或者空白字符串组成 ，则返回 <paramref name="value"/> 值本身。</returns>
        /// <exception cref="ArgumentNullException">如果 <paramref name="value"/> 值为 Null、空或者空白字符串组成，则抛出该异常。</exception>
        /// <exception cref="ApplicationException"></exception>
        public static void NotEmpty(string value, string argName = null, string message = null)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                message = message ?? "值不能为 null、空或者空白字符串。";
                if (argName == null)
                    throw Errors.Application(message);
                else
                    throw Errors.ArgumentError(argName, message);
            }
        }

        /// <summary>
        /// 如果一个集合是空或空序列，则抛出一个异常
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="argName"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public static void NotNullOrEmpty(IEnumerable value, string argName = null, string message = null)
        {
            if (value.IsNullOrEmpty())
            {
                message = message ?? "值不能为 null 或空序列。";
                if (argName == null)
                    throw Errors.Application(message);
                else
                    throw Errors.ArgumentError(argName, message);
            }
        }

        /// <summary>
        /// 检查无效的操作
        /// </summary>
        /// <param name="precondition"></param>
        /// <param name="message"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void NotInvalid(Func<bool> precondition, string message)
        {
            if (precondition())
            {
                Errors.InvalidOperation(message);
            }
        }

        /// <summary>
        /// 检查错误
        /// </summary>
        /// <param name="precondition"></param>
        /// <param name="message"></param>
        /// <exception cref="ApplicationException"></exception>
        public static void Not(Func<bool> precondition, string message)
        {
            if (precondition())
            {
                throw Errors.Application(message);
            }
        }
    }
}
