using Gentings.Security.Data;
using Gentings.Security.Notifications;
using Gentings.Security.Permissions;
using Gentings.Security.Roles;
using Microsoft.AspNetCore.Identity;
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
    public abstract class ServiceConfigurer<TUser, TRole, TUserRole, TUserStore, TRoleStore, TUserManager, TRoleManager>
        : IServiceConfigurer
        where TUser : UserBase
        where TRole : RoleBase
        where TUserRole : UserRoleBase
        where TUserStore : class, IUserStore<TUser>
        where TRoleStore : class, IRoleStore<TRole>
        where TUserManager : UserManager<TUser>
        where TRoleManager : RoleManager<TRole>
    {
        /// <summary>
        /// 配置服务。
        /// </summary>
        /// <param name="builder">容器构建实例。</param>
        protected abstract void ConfigureIdentityServices(IServiceBuilder builder);

        /// <summary>
        /// 配置<see cref="IdentityBuilder"/>实例。
        /// </summary>
        /// <param name="builder"><see cref="IdentityBuilder"/>实例。</param>
        protected virtual void ConfigureIdentityServices(IdentityBuilder builder)
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
        public void ConfigureServices(IServiceBuilder builder)
        {
            builder.AddServices(services =>
            {
                services.AddIdentity<TUser, TRole>();
                ConfigureIdentityServices(new IdentityBuilder(typeof(TUser), typeof(TRole), services));
            });
            if ((IdentityMode & EnabledModule.Notification) == EnabledModule.Notification)
                builder.AddNotification();
            if ((IdentityMode & EnabledModule.PermissionAuthorization) == EnabledModule.PermissionAuthorization)
                builder.AddPermissions<TRole, TUserRole>();
            ConfigureIdentityServices(builder);
        }

        /// <summary>
        /// 开启的功能模型。
        /// </summary>
        protected virtual EnabledModule IdentityMode => EnabledModule.None;
    }
}