using Gentings.Data.Migrations;

namespace Gentings.Extensions.AspNetCore.EventLogging
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加事件相关模块。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>服务构建实例。</returns>
        public static IServiceBuilder AddEventLoggers(this IServiceBuilder builder)
        {
            builder.AddTransients<IDataMigration, DefaultEventDataMigration>();
            builder.AddSingleton<IEventLogger, DefaultEventLogger>();
            builder.AddSingleton<IEventManager, DefaultEventManager>();
            builder.AddSingleton<IEventTypeManager, DefaultEventTypeManager>();
            return builder;
        }
    }
}