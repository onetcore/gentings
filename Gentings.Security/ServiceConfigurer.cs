using Gentings.Extensions.Settings;
using Gentings.Security.Data;
using Gentings.Security.Notifications;
using Gentings.Security.Permissions;
using Gentings.Security.Properties;
using Gentings.Security.Roles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.Security
{
    /// <summary>
    /// 服务配置基类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    /// <typeparam name="TUserStore">用户存储。</typeparam>
    /// <typeparam name="TRoleStore">角色存储。</typeparam>
    /// <typeparam name="TUserManager">用户管理类。</typeparam>
    /// <typeparam name="TRoleManager">角色管理类。</typeparam>
    /// <typeparam name="TUserRole">用户角色。</typeparam>
    /// <typeparam name="TSettings">配置类型。</typeparam>
    public abstract class ServiceConfigurer<TUser, TRole, TUserRole, TUserStore, TRoleStore, TUserManager, TRoleManager, TSettings>
        : IServiceConfigurer
        where TUser : UserBase, new()
        where TRole : RoleBase
        where TUserRole : UserRoleBase, new()
        where TUserStore : class, IUserStore<TUser>
        where TRoleStore : class, IRoleStore<TRole>
        where TUserManager : UserManager<TUser>
        where TRoleManager : RoleManager<TRole>
        where TSettings : IdentitySettings, new()
    {
        private static readonly TUser _anonymous = new TUser { UserName = "Anonymous", NickName = Resources.Anonymous };

        /// <summary>
        /// 配置<see cref="IdentityBuilder"/>实例。
        /// </summary>
        /// <param name="builder"><see cref="IdentityBuilder"/>实例。</param>
        protected virtual void ConfigureServices(IdentityBuilder builder)
        {
            builder.AddUserStore<TUserStore>()
                .AddRoleStore<TRoleStore>()
                .AddUserManager<TUserManager>()
                .AddRoleManager<TRoleManager>()
                .AddErrorDescriber<SecurityErrorDescriptor>()
                .AddDefaultTokenProviders();
        }

        /// <summary>
        /// 配置服务方法。
        /// </summary>
        /// <param name="builder">容器构建实例。</param>
        public virtual void ConfigureServices(IServiceBuilder builder)
        {
            builder.AddServices(services =>
            {
                services.AddIdentity<TUser, TRole>();
                services.AddScoped(sp => sp.GetRequiredService<IHttpContextAccessor>().HttpContext.GetUser<TUser>() ?? _anonymous);
                services.AddScoped(service => service.GetRequiredService<ISettingsManager>().GetSettings<TSettings>());
                ConfigureServices(new IdentityBuilder(typeof(TUser), typeof(TRole), services));
                ConfigureCookieServices(services, builder.Configuration.GetSection("Cookies"));
            });
            if ((EnabledModule & EnabledModule.Notification) == EnabledModule.Notification)
                builder.AddNotification();
            if ((EnabledModule & EnabledModule.PermissionAuthorization) == EnabledModule.PermissionAuthorization)
                builder.AddPermissions<TRole, TUserRole>();
        }

        /// <summary>
        /// 配置Cookie服务。
        /// </summary>
        /// <param name="services">服务集合实例对象。</param>
        /// <param name="section">Cookies配置节点。</param>
        protected virtual void ConfigureCookieServices(IServiceCollection services, IConfigurationSection section)
        {
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString(section["Login"] ?? "/login");
                options.LogoutPath = new PathString(section["Logout"] ?? "/logout");
                options.AccessDeniedPath = new PathString(section["Denied"] ?? "/denied");
                options.ReturnUrlParameter = section["ReturnUrl"] ?? "returnUrl";
            })
            .Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => false; //是否开启GDPR
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        /// <summary>
        /// 开启的功能模型。
        /// </summary>
        protected virtual EnabledModule EnabledModule => EnabledModule.None;
    }
}