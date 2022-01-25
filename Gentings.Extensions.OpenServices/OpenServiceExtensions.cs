using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 扩展类型。
    /// </summary>
    public static class OpenServiceExtensions
    {
        /// <summary>
        /// 是否匿名可访问。
        /// </summary>
        /// <param name="descriptor">操作描述实例。</param>
        /// <returns>返回判断结果。</returns>
        public static bool IsAnonymous(this ActionDescriptor descriptor)
        {
            if (descriptor.EndpointMetadata.Any(x => x is AllowAnonymousAttribute))
                return true;
            return !descriptor.EndpointMetadata.Any(x => x is AuthorizeAttribute);
        }
    }
}
