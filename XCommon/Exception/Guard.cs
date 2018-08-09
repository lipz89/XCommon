using System;

namespace XCommon.Exception
{
    /// <summary>
    /// throw,报警
    /// </summary>
    public class Guard
    {
        #region Throw

        public static void ThrowApplicationException(string message, params object[] args)
        {
            throw Error.Application(message, args);
        }

        public static void ThrowDbException(string message, params object[] args)
        {
            throw Error.Db(message, args);
        }

        public static void ThrowFileNotFoundException(string message, params object[] args)
        {
            throw Error.FileNotFound(message, args);
        }

        public static void ThrowIoException(string message, params object[] args)
        {
            throw Error.Io(message, args);
        }

        public static void ThrowSerializationException(string message, params object[] args)
        {
            throw Error.Serialization(message, args);
        }

        public static void ThrowArgumentNullException(string argName)
        {
            throw Error.ArgumentNull(argName);
        }

        public static void ThrowArgumentException(string argName, string message)
        {
            throw Error.ArgumentError(argName, message);
        }

        public static void ThrowInvalidOperationException(string message, System.Exception innerException = null, params object[] args)
        {
            throw Error.InvalidOperation(message, innerException, args);
        }

        public static void ThrowInvalidCastException(Type fromType, Type toType, System.Exception innerException)
        {
            throw Error.InvalidCast(fromType, toType, innerException);
        }
        #endregion
    }
}