using Gentings.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Gentings.Projects
{
    /// <summary>
    /// 应用程序配置。
    /// </summary>
    public class ApplicationConfigurer : IApplicationConfigurer
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        public int Priority => -1;

        /// <summary>
        /// 配置应用程序实例。
        /// </summary>
        /// <param name="app">应用程序构建实例。</param>
        /// <param name="configuration">配置接口。</param>
        public void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseEndpoints(builder => { builder.MapRazorPages(); });
        }
    }
}