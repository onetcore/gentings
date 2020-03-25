using Gentings.Storages;

namespace Gentings.AspNetCore.RazorPages
{
    /// <summary>
    /// 服务注册类。
    /// </summary>
    public class ServiceConfigurer : IServiceConfigurer
    {
        /// <summary>
        /// 配置服务方法。
        /// </summary>
        /// <param name="builder">容器构建实例。</param>
        public void ConfigureServices(IServiceBuilder builder)
        {
            builder.AddResources<ServiceConfigurer>();
        }
    }
}