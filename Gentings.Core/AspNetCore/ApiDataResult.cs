using System;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 包含数据的结果。
    /// </summary>
    /// <typeparam name="TData">数据类型。</typeparam>
    public class ApiDataResult<TData> : ApiResult
    {
        /// <summary>
        /// 初始化类<see cref="ApiDataResult{TData}"/>。
        /// </summary>
        /// <param name="data">数据实例。</param>
        public ApiDataResult(TData data)
        {
            Data = data;
        }

        /// <summary>
        /// 初始化类<see cref="ApiDataResult{TData}"/>，用于API文档返回特性中。
        /// </summary>
        public ApiDataResult() : this(Activator.CreateInstance<TData>()) { }

        /// <summary>
        /// 数据实例。
        /// </summary>
        public TData Data { get; }
    }
}