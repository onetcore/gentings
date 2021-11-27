using Gentings.Data.Migrations;

namespace Gentings.Extensions.Settings
{
    /// <summary>
    /// 扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        private class DefaultSettingsDataMigration : SettingsDataMigration
        {

        }

        private class DefaultNamedStringDataMigration : NamedStringDataMigration
        {

        }

        /// <summary>
        /// 添加配置组件。
        /// </summary>
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddSettings(this IServiceBuilder builder)
        {
            builder.AddTransients<IDataMigration, DefaultSettingsDataMigration>()
                   .AddSingleton<ISettingsManager, SettingsManager>();
            return builder;
        }

        /// <summary>
        /// 添加名称值组件。
        /// </summary>
        /// <param name="builder">服务构建实例对象。</param>
        /// <returns>返回服务构建实例对象。</returns>
        public static IServiceBuilder AddNamedString(this IServiceBuilder builder)
        {
            builder.AddTransients<IDataMigration, DefaultNamedStringDataMigration>()
                   .AddSingleton<INamedStringManager, NamedStringManager>();
            return builder;
        }
    }
}