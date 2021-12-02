using Gentings.Data.Migrations;

namespace Gentings.Extensions.SensitiveWords
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        internal class DefaultSensitiveWordDataMigration : SensitiveWordDataMigration { }

        /// <summary>
        /// 添加敏感词汇组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddSensitiveWords(this IServiceBuilder builder)
        {
            return builder.AddTransients<IDataMigration, DefaultSensitiveWordDataMigration>()
                .AddSingleton<ISensitiveWordManager, SensitiveWordManager>();
        }

    }
}