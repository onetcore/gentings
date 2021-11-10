using Gentings;
using Gentings.Data.SqlServer;
using Gentings.Extensions.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace GS.Extensions
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
            builder.AddSettings()
                .AddScoped(services => services.GetRequiredService<ISettingsManager>().GetSettings<SiteSettings>());
            builder.AddSqlServer()
                .AddServices(services =>
                {
                    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = new PathString("/login");
                    });
                    services.AddControllers();
                    services.AddRazorPages(options =>
                    {
                        options.Conventions.AuthorizeFolder("/admin");
                        options.Conventions.AuthorizeFolder("/account");
                    });
                });
        }
    }
}
