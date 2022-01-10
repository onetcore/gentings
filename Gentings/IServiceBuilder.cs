using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings
{
    /// <summary>
    /// Gentings容器构建实例。
    /// </summary>
    public interface IServiceBuilder
    {
        /// <summary>
        /// 添加服务。
        /// </summary>
        /// <param name="action">配置服务代理类。</param>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddServices(Action<IServiceCollection> action);

        /// <summary>
        /// 配置接口。
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// 添加Singleton服务。
        /// </summary>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddSingleton(Type serviceType, Type implementationType);

        /// <summary>
        /// 添加Singleton服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddSingleton<TService>()
            where TService : class;

        /// <summary>
        /// 添加Scoped服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddScoped<TService>()
            where TService : class;

        /// <summary>
        /// 添加Transient服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddTransient<TService>()
            where TService : class;

        /// <summary>
        /// 添加Singleton服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 添加Scoped服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddScoped<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 添加Scoped服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddScoped<TService>(Func<IServiceProvider, TService> func)
            where TService : class;

        /// <summary>
        /// 添加Transient服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 添加Singleton服务集合。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddSingletons<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 添加Scoped服务集合。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddScopeds<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 添加Transient服务集合。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder AddTransients<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        /// <summary>
        /// 配置选项。
        /// </summary>
        /// <param name="instance">选项实例对象。</param>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder ConfigureOptions(object instance);

        /// <summary>
        /// 配置选项。
        /// </summary>
        /// <typeparam name="TOptions">配置选项类型。</typeparam>
        /// <param name="action">选项配置实例。</param>
        /// <returns>返回构建实例。</returns>
        IServiceBuilder ConfigureOptions<TOptions>(Action<TOptions> action);
    }
}