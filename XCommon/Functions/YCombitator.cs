using System;

namespace XCommon.Functions
{
    /// <summary>
    /// Y 组合子
    /// </summary>
    public static class YCombitator
    {
        /// <summary>
        /// 不动点算子
        /// </summary>
        /// <returns></returns>
        public static Func<T, TResult> Fix<T, TResult>(Func<Func<T, TResult>, Func<T, TResult>> f)
        {
            return x => f(Fix(f))(x);
        }

        /// <summary>
        /// 不动点算子
        /// </summary>
        /// <returns></returns>
        public static Func<T1, T2, TResult> Fix<T1, T2, TResult>(Func<Func<T1, T2, TResult>, Func<T1, T2, TResult>> f)
        {
            return (x, y) => f(Fix(f))(x, y);
        }

        /// <summary>
        /// 不动点算子
        /// </summary>
        /// <returns></returns>
        public static Func<T1, T2, T3, TResult> Fix<T1, T2, T3, TResult>(Func<Func<T1, T2, T3, TResult>, Func<T1, T2, T3, TResult>> f)
        {
            return (x, y, z) => f(Fix(f))(x, y, z);
        }

        /// <summary>
        /// 不动点算子
        /// </summary>
        /// <returns></returns>
        public static Func<T1, T2, T3, T4, TResult> Fix<T1, T2, T3, T4, TResult>(Func<Func<T1, T2, T3, T4, TResult>, Func<T1, T2, T3, T4, TResult>> f)
        {
            return (x, y, z, t) => f(Fix(f))(x, y, z, t);
        }
    }
}
