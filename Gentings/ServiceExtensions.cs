using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gentings.AspNetCore;

namespace Gentings
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加框架容器注册。
        /// </summary>
        /// <param name="services">服务容器集合。</param>
        /// <param name="configuration">配置接口。</param>
        /// <returns>返回服务集合实例对象。</returns>
        public static IServiceBuilder AddGentings(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton(typeof(IServiceAccessor<>), typeof(ServiceAccessor<>));
            IEnumerable<Type> exportedTypes = GetExportedTypes(configuration);
            ServiceBuilder builder = new ServiceBuilder(services, configuration);
            BuildServices(builder, exportedTypes);
            return builder;
        }

        private static void BuildServices(IServiceBuilder builder, IEnumerable<Type> exportedTypes)
        {
            builder.AddServices(services =>
            {
                foreach (Type source in exportedTypes)
                {
                    if (typeof(IServiceConfigurer).IsAssignableFrom(source))
                    {
                        IServiceConfigurer service = Activator.CreateInstance(source) as IServiceConfigurer;
                        service?.ConfigureServices(builder);
                    }
                    else if (typeof(IHostedService).IsAssignableFrom(source))
                    {
                        //后台任务
                        services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService), source));
                    }
                    else //注册类型
                    {
                        IEnumerable<Type> interfaceTypes = source.GetInterfaces()
                            .Where(itf => typeof(IService).IsAssignableFrom(itf));
                        foreach (Type interfaceType in interfaceTypes)
                        {
                            if (typeof(ISingletonService).IsAssignableFrom(interfaceType))
                            {
                                services.TryAddSingleton(interfaceType, source);
                            }
                            else if (typeof(IScopedService).IsAssignableFrom(interfaceType))
                            {
                                services.TryAddScoped(interfaceType, source);
                            }
                            else if (typeof(ISingletonServices).IsAssignableFrom(interfaceType))
                            {
                                services.TryAddEnumerable(ServiceDescriptor.Singleton(interfaceType, source));
                            }
                            else if (typeof(IScopedServices).IsAssignableFrom(interfaceType))
                            {
                                services.TryAddEnumerable(ServiceDescriptor.Scoped(interfaceType, source));
                            }
                            else if (typeof(IServices).IsAssignableFrom(interfaceType))
                            {
                                services.TryAddEnumerable(ServiceDescriptor.Transient(interfaceType, source));
                            }
                            else
                            {
                                services.TryAddTransient(interfaceType, source);
                            }
                        }
                    }
                }
            });
        }

        private static IEnumerable<Type> GetExportedTypes(IConfiguration configuration)
        {
            List<Type> types = GetServices(configuration).ToList();
            List<TypeInfo> susppendServices = types.Select(type => type.GetTypeInfo())
                .Where(type => type.IsDefined(typeof(SuppressAttribute)))
                .ToList();
            List<string> susppendTypes = new List<string>();
            foreach (TypeInfo susppendService in susppendServices)
            {
                SuppressAttribute suppendAttribute = susppendService.GetCustomAttribute<SuppressAttribute>();
                susppendTypes.Add(suppendAttribute.FullName);
            }
            susppendTypes = susppendTypes.Distinct().ToList();
            return types.Where(type => !susppendTypes.Contains(type.FullName))
                .ToList();
        }

        private static IEnumerable<Type> GetServices(IConfiguration configuration)
        {
            List<Type> types = GetAssemblies(configuration)
                .SelectMany(assembly => assembly.GetTypes())
                .ToList();
            foreach (Type type in types)
            {
                TypeInfo info = type.GetTypeInfo();
                if (info.IsPublic && info.IsClass && !info.IsAbstract && typeof(IService).IsAssignableFrom(type))
                    yield return type;
            }
        }

        private static IEnumerable<string> GetExcludeAssemblies(IConfiguration configuration)
        {
            return configuration.GetSection("Excludes").AsList() ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// 获取应用程序中的程序集。
        /// </summary>
        /// <param name="configuration">配置实例。</param>
        /// <returns>返回应用程序集列表。</returns>
        public static IEnumerable<Assembly> GetAssemblies(this IConfiguration configuration)
        {
            List<Assembly> assemblies = new List<Assembly>();
            IEnumerable<string> excludes = GetExcludeAssemblies(configuration);
            foreach (RuntimeLibrary library in DependencyContext.Default.RuntimeLibraries)
            {
                if (library.Serviceable || excludes.Contains(library.Name, StringComparer.OrdinalIgnoreCase))
                    continue;
                assemblies.Add(Assembly.Load(new AssemblyName(library.Name)));
            }
            return assemblies;
        }

        /// <summary>
        /// 使用配置。
        /// </summary>
        /// <param name="app">应用程序构建实例接口。</param>
        /// <param name="configuration">配置实例接口。</param>
        /// <returns>应用程序构建实例接口。</returns>
        public static IApplicationBuilder UseGentings(this IApplicationBuilder app, IConfiguration configuration)
        {
            IApplicationConfigurer[] services = app.ApplicationServices.GetService<IEnumerable<IApplicationConfigurer>>()
                .OrderByDescending(x => x.Priority)
                .ToArray();
            foreach (IApplicationConfigurer service in services) service.Configure(app, configuration);
            return app;
        }

        /// <summary>
        /// 获取配置节点的字符串列表。
        /// </summary>
        /// <param name="section">配置节点。</param>
        /// <returns>返回当前配置的字符串列表。</returns>
        public static List<string> AsList(this IConfigurationSection section)
        {
            return section?.AsEnumerable().Where(x => x.Value != null).Select(x => x.Value).ToList();
        }
    }
}