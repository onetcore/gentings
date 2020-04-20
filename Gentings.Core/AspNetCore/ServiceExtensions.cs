using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 使用管道配置。
        /// </summary>
        /// <param name="app">应用程序构建实例接口。</param>
        /// <param name="configuration">配置实例接口。</param>
        /// <returns>应用程序构建实例接口。</returns>
        public static IApplicationBuilder UseGentings(this IApplicationBuilder app, IConfiguration configuration)
        {
            var services = app.ApplicationServices.GetService<IEnumerable<IApplicationConfigurer>>()
                .OrderByDescending(x => x.Priority)
                .ToArray();
            foreach (var service in services)
            {
                service.Configure(app, configuration);
            }

            return app;
        }

        /// <summary>
        /// 添加数据加密密钥服务。
        /// </summary>
        /// <param name="builder">服务器集合。</param>
        /// <param name="directory">存储文件夹。</param>
        /// <returns>返回服务器接口集合。</returns>
        public static IServiceBuilder AddDataProtection(this IServiceBuilder builder, string directory = "../storages/keys")
        {
            DirectoryInfo info;
            try
            {
                info = new DirectoryInfo(directory);
            }
            catch
            {
                directory = Path.Combine(Directory.GetCurrentDirectory(), directory);
                info = new DirectoryInfo(directory);
            }

            return builder.AddServices(services => services.AddDataProtection()
                 .PersistKeysToFileSystem(info)
                 .ProtectKeysWithDpapi());
        }
    }
}