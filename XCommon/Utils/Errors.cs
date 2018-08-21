using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace XCommon.Utils
{
    /// <summary>
    /// 返回各类异常错误
    /// </summary>
    public static class Errors
    {
        #region Exception

        private static string FormatOrNot(string message, params object[] args)
        {
            return args.Any() ? string.Format(message, args) : message;
        }

        public static System.Exception Application(string message, params object[] args)
        {
            return new ApplicationException(FormatOrNot(message, args));
        }

        public static System.Exception Db(string message, params object[] args)
        {
            return new DataException(FormatOrNot(message, args));
        }

        public static System.Exception FileNotFound(string message, params object[] args)
        {
            return new FileNotFoundException(FormatOrNot(message, args));
        }

        public static System.Exception Io(string message, params object[] args)
        {
            return new IOException(FormatOrNot(message, args));
        }

        public static System.Exception Serialization(string message, params object[] args)
        {
            return new SerializationException(FormatOrNot(message, args));
        }

        public static System.Exception ArgumentNull(string argName)
        {
            return new ArgumentNullException(argName, "参数不能为空");
        }

        public static System.Exception ArgumentError(string argName, string message)
        {
            return new ArgumentException(message, argName);
        }

        public static System.Exception InvalidOperation(string message, System.Exception innerException = null, params object[] args)
        {
            if (innerException == null)
                return new InvalidOperationException(FormatOrNot(message, args));
            else
                return new InvalidOperationException(FormatOrNot(message, args), innerException);
        }

        public static System.Exception InvalidCast(Type fromType, Type toType, System.Exception innerException)
        {
            return new InvalidCastException(string.Format("不能转换类型 '{0}' 到 '{1}'.", fromType.FullName, toType.FullName), innerException);
        }

        #endregion
    }
}