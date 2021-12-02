using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.Security
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加权限验证服务。
        /// </summary>
        /// <param name="builder">验证服务构建实例。</param>
        /// <returns>返回验证服务构建实例。</returns>
        public static AuthenticationBuilder AddPermission(this AuthenticationBuilder builder)
        {
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return builder;
        }
    }
}
