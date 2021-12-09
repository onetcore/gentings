using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Gentings
{
    internal class ServiceBuilder : IServiceBuilder
    {
        private readonly IServiceCollection _services;

        public ServiceBuilder(IServiceCollection services, IConfiguration configuration)
        {
            _services = services;
            Configuration = configuration;
        }

        /// <summary>
        /// 配置接口。
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 添加Singleton服务。
        /// </summary>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddSingleton(Type serviceType, Type implementationType)
        {
            _services.AddSingleton(serviceType, implementationType);
            return this;
        }

        /// <summary>
        /// 添加Singleton服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddSingleton<TService>() where TService : class
        {
            _services.TryAddSingleton<TService>();
            return this;
        }

        /// <summary>
        /// 添加Scoped服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddScoped<TService>() where TService : class
        {
            _services.TryAddScoped<TService>();
            return this;
        }

        /// <summary>
        /// 添加Transient服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddTransient<TService>() where TService : class
        {
            _services.TryAddTransient<TService>();
            return this;
        }

        /// <summary>
        /// 添加Singleton服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddSingleton<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            _services.TryAddSingleton<TService, TImplementation>();
            return this;
        }

        /// <summary>
        /// 添加Scoped服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddScoped<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            _services.TryAddScoped<TService, TImplementation>();
            return this;
        }

        /// <summary>
        /// 添加Scoped服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddScoped<TService>(Func<IServiceProvider, TService> func)
            where TService : class
        {
            _services.TryAddScoped(func);
            return this;
        }

        /// <summary>
        /// 添加Transient服务。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddTransient<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            _services.TryAddTransient<TService, TImplementation>();
            return this;
        }

        /// <summary>
        /// 添加Singleton服务集合。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddSingletons<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            _services.TryAddEnumerable(ServiceDescriptor.Singleton<TService, TImplementation>());
            return this;
        }

        /// <summary>
        /// 添加Scoped服务集合。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddScopeds<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            _services.TryAddEnumerable(ServiceDescriptor.Scoped<TService, TImplementation>());
            return this;
        }

        /// <summary>
        /// 添加Transient服务集合。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <typeparam name="TImplementation">实现类。</typeparam>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddTransients<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            _services.TryAddEnumerable(ServiceDescriptor.Transient<TService, TImplementation>());
            return this;
        }


        /// <summary>
        /// 添加服务。
        /// </summary>
        /// <param name="action">配置服务代理类。</param>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder AddServices(Action<IServiceCollection> action)
        {
            action(_services);
            return this;
        }

        /// <summary>
        /// 配置选项。
        /// </summary>
        /// <param name="instance">选项实例对象。</param>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder ConfigureOptions(object instance)
        {
            _services.ConfigureOptions(instance);
            return this;
        }

        /// <summary>
        /// 配置选项。
        /// </summary>
        /// <typeparam name="TOptions">配置选项类型。</typeparam>
        /// <param name="action">选项配置实例。</param>
        /// <returns>返回构建实例。</returns>
        public IServiceBuilder ConfigureOptions<TOptions>(Action<TOptions> action)
        {
            _services.ConfigureOptions(action);
            return this;
        }
    }
}