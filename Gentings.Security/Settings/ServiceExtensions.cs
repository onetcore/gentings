﻿using Gentings.Data.Migrations;
using Gentings.Extensions.Settings;

namespace Gentings.Security.Settings
{
    /// <summary>
    /// 扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        private class DefaultSettingsDataMigration : SettingsDataMigration
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
                .AddScoped<ISettingsManager, SettingsManager>()
                .AddTransients<IDataMigration, DefaultSettingsDataMigration>();
            return builder;
        }
    }
}