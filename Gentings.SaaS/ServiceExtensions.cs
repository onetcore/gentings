using System.Threading.Tasks;
using Gentings.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.SaaS
{
    /// <summary>
    /// 扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加Saas服务。
        /// </summary>
        /// <typeparam name="TSite">网站实例类型。</typeparam>
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddSaaS<TSite>(this IServiceBuilder builder)
            where TSite : Site, new()
        {
            return builder.AddServices(services =>
            {
                services.AddSingleton(typeof(ISiteManager<>), typeof(SiteManager<>));
                services.AddScoped(service => service.GetRequiredService<ISiteDomainManager>().Current);
                services.AddScoped(service =>
                {
                    var current = service.GetRequiredService<SiteDomain>();
                    if (current == null)
                        return null;
                    var siteManager = service.GetRequiredService<ISiteManager<TSite>>();
                    return siteManager.Find(current.SiteId);
                });
            });
        }

        /// <summary>
        /// 使用Saas中间件。
        /// </summary>
        /// <typeparam name="TSite">网站实例类型。</typeparam>
        /// <param name="app">应用构建实例对象。</param>
        /// <returns>应用服务构建实例对象。</returns>
        public static IApplicationBuilder UseSaaS<TSite>(this IApplicationBuilder app)
            where TSite : Site, new()
        {
            return app.UseMiddleware<SiteMiddleware<TSite>>();
        }

        private class SiteMiddleware<TSite>
            where TSite : Site
        {
            private readonly RequestDelegate _next;

            public SiteMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                var site = context.RequestServices.GetService<TSite>();
                if (site?.Disabled == false)
                    await _next(context);
                else
                {
                    context.Response.Clear();
                    context.Response.StatusCode = 400;
                }
            }
        }
    }
}