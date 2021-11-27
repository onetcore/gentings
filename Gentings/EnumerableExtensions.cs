using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gentings
{
    /// <summary>
    /// 集合扩展类。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 将列表格式化为字符串。
        /// </summary>
        /// <param name="items">当前实例列表。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回格式化后的字符串。</returns>
        public static string Join(this IEnumerable items, string separator = ",")
        {
            var list = new List<object>();
            var enumerator = items.GetEnumerator();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            return string.Join(separator, list);
        }

        /// <summary>
        /// 将字符串分割为数值列表。
        /// </summary>
        /// <param name="items">当前数值字符串。</param>
        /// <param name="separator">分隔符。</param>
        /// <returns>返回数值列表。</returns>
        public static int[] ToInt32Array(this string items, string separator = ",")
        {
            if (string.IsNullOrEmpty(items))
                return null;
            return items.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert.ToInt32(x.Trim()))
                .Distinct()
                .ToArray();
        }

        /// <summary>
        /// 截取数组中的区间并且返回数组实例。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="array">当前数组实例。</param>
        /// <param name="index">开始索引。</param>
        /// <param name="length">长度。</param>
        /// <returns>返回截取的数组列表。</returns>
        public static IEnumerable<TModel> Slice<TModel>(this IEnumerable<TModel> array, int index, int length)
        {
            length = Math.Min(index + length, array.Count());
            for (var i = index; i < length; i++)
            {
                yield return array.ElementAt(i);
            }
        }
    }
}
