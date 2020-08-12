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
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddSaaS(this IServiceBuilder builder)
        {
            return builder.AddServices(services => services.AddScoped(service =>
              {
                  var siteManager = service.GetRequiredService<ISiteManager>();
                  var request = service.GetRequiredService<IHttpContextAccessor>().HttpContext.Request;
                  var current = request.GetDomain();
                  return siteManager.GetSite(current);
              }));
        }

        /// <summary>
        /// 使用Saas中间件。
        /// </summary>
        /// <param name="app">应用构建实例对象。</param>
        /// <returns>应用服务构建实例对象。</returns>
        public static IApplicationBuilder UseSaaS(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SiteMiddleware>();
        }

        internal class SiteMiddleware
        {
            private readonly RequestDelegate _next;

            public SiteMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                var site = context.RequestServices.GetRequiredService<Site>();
                if (site != null)
                    await _next(context);
            }
        }
    }
}