using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using XCommon.Utils;

namespace XCommon.Extenstions
{
    /// <summary>
    /// 枚举器扩展方法
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 判断一个枚举器是否为空或空序列
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this IEnumerable source)
        {
            if (source == null)
                return true;

            foreach (var item in source)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 判断一个枚举器是否为空或空序列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true;
            return !source.Any();
        }

        #region DistinctBy

        /// <summary>
        /// 根据一个比较器返回非重复函数
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector">比较器</param>
        /// <returns></returns>
        public static IEnumerable<TSource> DistinctBy<TSource>(this IEnumerable<TSource> source,
            Func<TSource, dynamic> keySelector)
        {
            return source.Distinct(XEqualityComparer<TSource>.Get(keySelector));
        }

        #endregion

        #region ToSafeDictionary

        /// <summary>
        /// 根据指定的键选择器和元素选择器函数，从 System.Collections.Generic.IEnumerable&lt;TSource&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey,TSource&gt;，在此过程中遇到重复的键将被忽略。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey,TSource&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey,TSource&gt;，包含从输入序列中选择的类型为 TElement 的值。</returns>
        /// <exception cref="ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。- 或 -keySelector 产生了一个 null 键。</exception>
        public static Dictionary<TKey, TSource> ToSafeDictionary<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            return source.GroupBy(keySelector).ToDictionary(x => x.Key, x => x.First());
        }

        /// <summary>
        /// 根据指定的键选择器和元素选择器函数，从 System.Collections.Generic.IEnumerable&lt;TSource&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey,TSource&gt;，在此过程中遇到重复的键将被忽略。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey,TSource&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <param name="comparer">一个用于对键进行比较的 System.Collections.Generic.IEqualityComparer&lt;TKey&gt;。</param>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey,TSource&gt;，包含从输入序列中选择的类型为 TElement 的值。</returns>
        /// <exception cref="ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。- 或 -keySelector 产生了一个 null 键。</exception>
        public static Dictionary<TKey, TSource> ToSafeDictionary<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return source.GroupBy(keySelector, comparer).ToDictionary(x => x.Key, x => x.First());
        }

        /// <summary>
        /// 根据指定的键选择器和元素选择器函数，从 System.Collections.Generic.IEnumerable&lt;TSource&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey,TElement&gt;，在此过程中遇到重复的键将被忽略。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <typeparam name="TElement">elementSelector 返回的值的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey,TElement&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <param name="elementSelector">用于从每个元素产生结果元素值的转换函数。</param>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey,TElement&gt;，包含从输入序列中选择的类型为 TElement 的值。</returns>
        /// <exception cref="ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。- 或 -keySelector 产生了一个 null 键。</exception>
        public static Dictionary<TKey, TElement> ToSafeDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return source.GroupBy(keySelector).ToDictionary(x => x.Key, x => elementSelector(x.First()));
        }

        /// <summary>
        /// 根据指定的键选择器和元素选择器函数，从 System.Collections.Generic.IEnumerable&lt;TSource&gt; 创建一个 System.Collections.Generic.Dictionary&lt;TKey,TElement&gt;，在此过程中遇到重复的键将被忽略。
        /// </summary>
        /// <typeparam name="TSource">source 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">keySelector 返回的键的类型。</typeparam>
        /// <typeparam name="TElement">elementSelector 返回的值的类型。</typeparam>
        /// <param name="source">一个 System.Collections.Generic.IEnumerable&lt;TSource&gt;，将从它创建一个 System.Collections.Generic.Dictionary&lt;TKey,TElement&gt;。</param>
        /// <param name="keySelector">用于从每个元素中提取键的函数。</param>
        /// <param name="elementSelector">用于从每个元素产生结果元素值的转换函数。</param>
        /// <param name="comparer">一个用于对键进行比较的 System.Collections.Generic.IEqualityComparer&lt;TKey&gt;。</param>
        /// <returns>一个 System.Collections.Generic.Dictionary&lt;TKey,TElement&gt;，包含从输入序列中选择的类型为 TElement 的值。</returns>
        /// <exception cref="ArgumentNullException">source 或 keySelector 或 elementSelector 为 null。- 或 -keySelector 产生了一个 null 键。</exception>
        public static Dictionary<TKey, TElement> ToSafeDictionary<TSource, TKey, TElement>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey> comparer)
        {
            return source.GroupBy(keySelector, comparer).ToDictionary(x => x.Key, x => elementSelector(x.First()));
        }

        #endregion
    }
}