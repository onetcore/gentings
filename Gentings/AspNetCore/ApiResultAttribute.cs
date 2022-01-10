using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 默认返回的<see cref="ApiResult"/>结果特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ApiResultAttribute : ProducesResponseTypeAttribute
    {
        /// <summary>
        /// 初始化类<see cref="ApiResultAttribute"/>。
        /// </summary>
        /// <param name="type">返回结果类型。</param>
        public ApiResultAttribute(Type type = null) : base(type ?? typeof(ApiResult), 200)
        {

        }
    }
}