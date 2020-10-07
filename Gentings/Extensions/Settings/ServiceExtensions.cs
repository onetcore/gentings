﻿using Gentings.Data.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加字典组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <param name="includeDictionary">是否组成字典实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddSettings(this IServiceBuilder builder, bool includeDictionary = false)
        {
            builder.AddServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor
                    .Transient<IDataMigration, SettingsDataMigration>());
                services.TryAddSingleton<ISettingsManager, SettingsManager>();
            });
            if (includeDictionary)
                builder.AddSettings<SettingDictionaryManager>();
            return builder;
        }

        /// <summary>
        /// 添加字典组件。
        /// </summary>
        /// <typeparam name="TGlobalSettings">全局配置实例类型。</typeparam>
        /// <param name="builder">服务构建实例。</param>
        /// <param name="includeDictionary">是否组成字典实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddSettings<TGlobalSettings>(this IServiceBuilder builder, bool includeDictionary = false)
            where TGlobalSettings : class, new()
        {
            builder.AddServices(services => services.AddScoped(service => service.GetRequiredService<ISettingsManager>().GetSettings<TGlobalSettings>()));
            return builder.AddSettings(includeDictionary);
        }

        /// <summary>
        /// 添加字典组件。
        /// </summary>
        /// <typeparam name="TSettingDictionaryManager">字典实现类。</typeparam>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddSettings<TSettingDictionaryManager>(this IServiceBuilder builder)
            where TSettingDictionaryManager : class, ISettingDictionaryManager
        {
            return builder.AddServices(services =>
            {
                services.TryAddEnumerable(ServiceDescriptor
                    .Transient<IDataMigration, SettingsDataMigration>());
                services.TryAddSingleton<ISettingsManager, SettingsManager>();
                services.TryAddEnumerable(ServiceDescriptor
                    .Transient<IDataMigration, SettingDictionaryDataMigration>());
                services.TryAddSingleton<ISettingDictionaryManager, TSettingDictionaryManager>();
            });
        }
    }
}