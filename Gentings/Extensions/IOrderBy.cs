using System;
using System.Collections.Generic;
using System.Linq;

namespace Gentings.Extensions
{
    /// <summary>
    /// 排序。
    /// </summary>
    public interface IOrderBy
    {
        /// <summary>
        /// 是否降序。
        /// </summary>
        bool Desc { get; }

        /// <summary>
        /// 排序列枚举。
        /// </summary>
        Enum? Order { get; }
    }

    /// <summary>
    /// 排序规则。
    /// </summary>
    /// <typeparam name="TEnum">排序枚举类型。</typeparam>
    public abstract class OrderBy<TEnum> : IOrderBy
        where TEnum : Enum
    {
        /// <summary>
        /// 是否降序。
        /// </summary>
        public bool Desc { get; set; }

        /// <summary>
        /// 排序列枚举。
        /// </summary>
        public TEnum? Order { get; set; }
        Enum IOrderBy.Order => Order;

        /// <summary>
        /// 排序。
        /// </summary>
        /// <typeparam name="TSource">原类型。</typeparam>
        /// <typeparam name="TKey">排序属性。</typeparam>
        /// <param name="items">当前集合。</param>
        /// <param name="keySelector">属性选择器。</param>
        /// <returns>返回排序结果。</returns>
        public IEnumerable<TSource> Sorted<TSource, TKey>(IEnumerable<TSource> items, Func<TSource, TKey> keySelector)
        {
            if (Desc)
                return items.OrderByDescending(keySelector);
            return items.OrderBy(keySelector);
        }
    }
}