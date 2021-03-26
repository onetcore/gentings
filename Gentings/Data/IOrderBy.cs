using System;

namespace Gentings.Data
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
        Enum Order { get; }
    }
}