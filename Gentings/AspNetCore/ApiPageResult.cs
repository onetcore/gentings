using Gentings.Extensions;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 分页数据结果。
    /// </summary>
    /// <typeparam name="TData">查询实例。</typeparam>
    public class ApiPageResult<TData> : ApiResult
        where TData : IPageEnumerable
    {
        /// <summary>
        /// 初始化类<see cref="ApiPageResult{TData}"/>。
        /// </summary>
        /// <param name="data">数据实例。</param>
        public ApiPageResult(TData data)
        {
            Data = data;
        }

        /// <summary>
        /// 分页数据。
        /// </summary>
        public TData Data { get; }

        /// <summary>
        /// 页码。
        /// </summary>
        public int Current => Data.Page;

        /// <summary>
        /// 每页显示记录数。
        /// </summary>
        public int PageSize => Data.PageSize;

        /// <summary>
        /// 总记录数。
        /// </summary>
        public int Total => Data.Size;

        /// <summary>
        /// 总页数。
        /// </summary>
        public int Pages => Data.Pages;
    }
}