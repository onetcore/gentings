using Gentings.Data.Migrations;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        private class DefaultEventDataMigration : EventDataMigration
        {

        }
        private class DefaultDifferDataMigration : DifferDataMigration
        {

        }

        /// <summary>
        /// 添加事件组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddEvent(this IServiceBuilder builder)
        {
            builder.AddTransients<IDataMigration, DefaultEventDataMigration>();
            builder.AddSingleton<IEventManager, EventManager>();
            builder.AddSingleton<IEventLogger, EventLogger>();
            return builder;
        }

        /// <summary>
        /// 添加对象更改实例组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddDiffer(this IServiceBuilder builder)
        {
            builder.AddTransients<IDataMigration, DefaultDifferDataMigration>();
            builder.AddSingleton<IDifferManager, DifferManager>();
            return builder;
        }
    }
}