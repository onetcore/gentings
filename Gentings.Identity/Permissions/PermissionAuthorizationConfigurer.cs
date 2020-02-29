using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.Identity.Permissions
{
    /// <summary>
    /// 注册验证处理类。
    /// </summary>
    public class PermissionAuthorizationConfigurer : IServiceConfigurer
    {
        /// <summary>
        /// 配置服务方法。
        /// </summary>
        /// <param name="builder">容器构建实例。</param>
        public void ConfigureServices(IServiceBuilder builder)
        {
            builder.AddServices(services => services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>());
        }
    }
}