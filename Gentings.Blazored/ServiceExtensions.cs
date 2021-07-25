using System;
using System.Net.Http;
using Gentings.Blazored.Authentication;
using Gentings.Blazored.Components.Menu;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using AuthenticationStateProvider = Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider;

namespace Gentings.Blazored
{
    /// <summary>
    /// 服务扩展类。
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 添加菜单提供者。
        /// </summary>
        /// <typeparam name="TMenuProvider">菜单提供者类型。</typeparam>
        /// <param name="services">服务列表。</param>
        /// <returns>服务集合列表实例。</returns>
        public static IServiceCollection AddMenu<TMenuProvider>(this IServiceCollection services)
            where TMenuProvider : MenuProvider
        {
            services.TryAddEnumerable(ServiceDescriptor.Transient<IMenuProvider, TMenuProvider>());
            return services;
        }

        /// <summary>
        /// 添加当前程序集里的服务。
        /// </summary>
        /// <param name="services">服务集合。</param>
        /// <param name="baseAddress">请求的基地址。</param>
        /// <param name="localization">是否注册本地化资源实例。</param>
        /// <returns>返回服务集合。</returns>
        public static IServiceCollection AddBlazorComponent(this IServiceCollection services, string baseAddress = null,
            bool localization = true)
        {
            services.AddScoped<JSRuntime>();
            services.AddScoped<IMenuFactory, MenuFactory>();
            if (!string.IsNullOrEmpty(baseAddress))
            {
                services.AddHttpClient(ServiceBase.ServiceName, client => client.BaseAddress = new Uri(baseAddress));
                services.AddScoped(sp =>
                    sp.GetRequiredService<IHttpClientFactory>().CreateClient(ServiceBase.ServiceName));
                services.AddOptions()
                    .AddAuthorizationCore()
                    .AddScoped<Authentication.AuthenticationStateProvider>()
                    .AddScoped<AuthenticationStateProvider>(service => service.GetRequiredService<Authentication.AuthenticationStateProvider>())
                    .AddScoped<SignOutSessionStateManager>()
                    .AddScoped<IAuthorizedService, AuthorizedService>();
            }

            if (localization) services.AddLocalization();

            return services;
        }
    }
}