using Gentings.Data.Initializers;

namespace Gentings.Data
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加数据库基础组件。
        /// </summary>
        /// <param name="builder">服务构建实例。</param>
        /// <returns>返回服务构建实例。</returns>
        public static IServiceBuilder AddDataInitializer(this IServiceBuilder builder)
        {
            builder.AddSingleton<IInitializerManager, InitializerManager>();
            return builder;
        }
    }
}