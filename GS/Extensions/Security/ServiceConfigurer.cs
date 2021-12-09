using Gentings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 服务配置。
    /// </summary>
    public class ServiceConfigurer : IServiceConfigurer
    {
        /// <summary>
        /// 添加服务。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        public void ConfigureServices(IServiceBuilder builder)
        {
            builder.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.AddScoped(services => services.GetRequiredService<IUserManager>().GetUser());
        }
    }
}
