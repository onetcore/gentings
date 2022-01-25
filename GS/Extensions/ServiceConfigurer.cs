using Gentings;
using Gentings.AspNetCore;
using Gentings.Data.SqlServer;
using Gentings.Extensions.OpenServices;
using Gentings.Extensions.Settings;
using Gentings.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using GS.Extensions.Security;
using Gentings.AspNetCore.Options;

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
                .AddScoped(services => services.GetRequiredService<ISettingsManager>().GetSettings<SkinSettings>())
                .AddScoped(services => services.GetRequiredService<ISettingsManager>().GetSettings<SiteSettings>());
            builder.AddSqlServer()
                .AddModelUI<User>()//添加用户模型应用
                .AddOpenServices<User>()
                .AddServices(services =>
                {
                    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddPermissionAuthorization()//添加权限验证
                    .AddCookie(options =>
                    {
                        options.LoginPath = new PathString("/login");
                    });
                    services.AddControllers();
                    services.AddRazorPages(options =>
                    {
                        options.AddCultureLocalizationOptions();
                        options.Conventions.AuthorizeFolder("/admin");
                        options.Conventions.AuthorizeFolder("/account");
                    });
                });
        }
    }
}
