using Gentings.Data.Migrations;

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
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddSettingDictionary(this IServiceBuilder builder)
            => builder.AddSettingDictionary<SettingDictionaryManager>();

        /// <summary>
        /// 添加字典组件。
        /// </summary>
        /// <typeparam name="TSettingDictionaryManager">字典实现类。</typeparam>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddSettingDictionary<TSettingDictionaryManager>(this IServiceBuilder builder)
            where TSettingDictionaryManager : class, ISettingDictionaryManager
        {
            return builder.AddTransients<IDataMigration, SettingDictionaryDataMigration>()
                .AddSingleton<ISettingDictionaryManager, TSettingDictionaryManager>();
        }
    }
}