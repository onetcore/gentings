namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加事件组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddEvent(this IServiceBuilder builder)
        {
            builder.AddSingleton<IEventManager, EventManager>();
            builder.AddSingleton<IEventLogger, EventLogger>();
            return builder;
        }
    }
}