using Gentings.Extensions;
using System;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 包含数据的结果。
    /// </summary>
    public class ApiDataResult : ApiResult
    {
        /// <summary>
        /// 初始化类<see cref="ApiDataResult"/>。
        /// </summary>
        /// <param name="data">数据实例。</param>
        public ApiDataResult(object data)
        {
            if (data is IPageEnumerable pd)//分页数据
                Data = new { Data = data, pd.Pages, pd.PageSize, pd.PageIndex, pd.Size };
            else
                Data = data;
        }

        /// <summary>
        /// 数据实例。
        /// </summary>
        public object Data { get; }
    }

    ///// <summary>
    ///// 包含数据的结果。
    ///// </summary>
    ///// <typeparam name="TData">数据类型。</typeparam>
    //public class ApiDataResult<TData> : ApiDataResult
    //{
    //    /// <summary>
    //    /// 初始化类<see cref="ApiDataResult{TData}"/>。
    //    /// </summary>
    //    /// <param name="data">数据实例。</param>
    //    public ApiDataResult(object data) : base(data)
    //    {
    //    }

    //    /// <summary>
    //    /// 初始化类<see cref="ApiDataResult{TData}"/>。
    //    /// </summary>
    //    public ApiDataResult() : this(Activator.CreateInstance<TData>()) { }

    //    /// <summary>
    //    /// 数据实例。
    //    /// </summary>
    //    public new TData Data => (TData)base.Data;
    //}
}