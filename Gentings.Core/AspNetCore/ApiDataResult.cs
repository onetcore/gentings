using System;
using Microsoft.AspNetCore.Mvc;

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
        /// 数据实例。
        /// </summary>
        public TData Data { get; }
    }

    /// <summary>
    /// 默认返回的<see cref="ApiDataResult{TData}"/>结果特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiDataResultAttribute : ProducesResponseTypeAttribute
    {
        /// <summary>
        /// 初始化类<see cref="ApiDataResultAttribute"/>。
        /// </summary>
        /// <param name="type">返回结果类型。</param>
        public ApiDataResultAttribute(Type type) : base(typeof(ApiDataResult<>).MakeGenericType(type), 200)
        {
        }
    }
}