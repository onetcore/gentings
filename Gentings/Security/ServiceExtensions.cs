using Gentings.Data.Migrations;
using Gentings.Extensions.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.Security
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加权限验证服务。
        /// </summary>
        /// <param name="builder">验证服务构建实例。</param>
        /// <returns>返回验证服务构建实例。</returns>
        public static AuthenticationBuilder AddPermissionAuthorization(this AuthenticationBuilder builder)
        {
            builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            return builder;
        }

        private class DefaultSettingsDataMigration : Settings.SettingsDataMigration
        {

        }

        /// <summary>
        /// 添加配置组件。
        /// </summary>
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddUserSettings(this IServiceBuilder builder)
        {
            builder.AddSettings()
                .AddScoped<Settings.ISettingsManager, Settings.SettingsManager>()
                .AddTransients<IDataMigration, DefaultSettingsDataMigration>();
            return builder;
        }
    }
}
