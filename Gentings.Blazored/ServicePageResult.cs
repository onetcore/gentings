using System.Collections.Generic;

namespace Gentings.Blazored
{
    /// <summary>
    /// 分页数据结果。
    /// </summary>
    /// <typeparam name="TData">查询实例。</typeparam>
    public class ServicePageResult<TData> : ServiceDataResult<IEnumerable<TData>>
    {
        /// <summary>
        /// 页码。
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 每页显示记录数。
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数。
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数。
        /// </summary>
        public int Pages { get; set; }
    }
}