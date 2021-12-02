using Gentings.AspNetCore.Options;
using Gentings.Extensions.Settings;

namespace Gentings.AspNetCore.Storages
{
    /// <summary>
    /// 服务配置。
    /// </summary>
    public class ServiceConfigurer : IServiceConfigurer
    {
        /// <summary>
        /// 配置服务方法。
        /// </summary>
        /// <param name="builder">容器构建实例。</param>
        public void ConfigureServices(IServiceBuilder builder)
        {
            builder.AddNamedString();
            builder.AddResources<ServiceConfigurer>();
        }
    }
}