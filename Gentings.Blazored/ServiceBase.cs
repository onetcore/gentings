using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.Blazored
{
    /// <summary>
    /// 服务基类。
    /// </summary>
    public abstract class ServiceBase
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 获取服务接口实例。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回当前服务实例对象。</returns>
        protected TService GetService<TService>() => _serviceProvider.GetService<TService>();

        /// <summary>
        /// 获取必须的服务接口实例。
        /// </summary>
        /// <typeparam name="TService">服务类型。</typeparam>
        /// <returns>返回当前服务实例对象。</returns>
        protected TService GetRequiredService<TService>() => _serviceProvider.GetRequiredService<TService>();

        /// <summary>
        /// 客户端请求实例。
        /// </summary>
        protected HttpClient Client { get; }

        /// <summary>
        /// 脚本运行时。
        /// </summary>
        protected JSRuntime JSRuntime { get; }

        /// <summary>
        /// 默认HttpClient注册的服务名称。
        /// </summary>
        public const string ServiceName = "ApiServer";

        /// <summary>
        /// 验证标志。
        /// </summary>
        public const string AuthToken = "auth-token";

        /// <summary>
        /// 初始化类<see cref="ServiceBase"/>。
        /// </summary>
        /// <param name="serviceProvider">服务提供者接口。</param>
        /// <param name="serviceName">服务名称。</param>
        protected ServiceBase(IServiceProvider serviceProvider, string serviceName = null)
        {
            _serviceProvider = serviceProvider;
            if (string.IsNullOrEmpty(serviceName))
                serviceName = ServiceName;
            JSRuntime = serviceProvider.GetRequiredService<JSRuntime>();
            Client = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient(serviceName);
        }

        /// <summary>
        /// 附加JWT验证。
        /// </summary>
        protected virtual async Task EnsuredAccessTokenAsync()
        {
            if (Client.DefaultRequestHeaders.Authorization != null)
                return;
            var token = await _serviceProvider.GetRequiredService<JSRuntime>().GetLocalStorageAsync(AuthToken);
            if (!string.IsNullOrWhiteSpace(token))
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <typeparam name="TResult">返回的结果类型。</typeparam>
        /// <param name="api">API地址。</param>
        /// <param name="query">查询参数。</param>
        /// <returns>返回发送结果。</returns>
        protected virtual Task<ServiceDataResult<TResult>> GetAsync<TResult>(string api, object query = null)
        {
            return CatchExecuteAsync(() =>
            {
                if (query == null)
                    return Client.GetFromJsonAsync<ServiceDataResult<TResult>>(api);
                api = api.AppendQuery(query);
                return Client.GetFromJsonAsync<ServiceDataResult<TResult>>(api);
            });
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <typeparam name="TResult">返回的结果类型。</typeparam>
        /// <param name="api">API地址。</param>
        /// <param name="query">查询参数。</param>
        /// <returns>返回发送结果。</returns>
        protected virtual async Task<TResult> GetDataAsync<TResult>(string api, object query = null)
        {
            var result = await GetAsync<TResult>(api, query);
            if (result.Status) return result.Data;
            return default;
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <typeparam name="TResult">返回的结果类型。</typeparam>
        /// <param name="api">API地址。</param>
        /// <param name="query">查询参数。</param>
        /// <returns>返回发送结果。</returns>
        protected virtual Task<ServicePageResult<TResult>> GetPageAsync<TResult>(string api, object query = null)
        {
            return CatchExecuteAsync(() =>
            {
                if (query == null)
                    return Client.GetFromJsonAsync<ServicePageResult<TResult>>(api);
                api = api.AppendQuery(query);
                return Client.GetFromJsonAsync<ServicePageResult<TResult>>(api);
            });
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="api">API地址。</param>
        /// <param name="data">数据实例。</param>
        /// <param name="options">JSON选项。</param>
        /// <param name="cancellation">取消标志。</param>
        /// <returns>返回发送结果。</returns>
        protected virtual Task<ServiceResult> PostAsync(string api, object data = null, JsonSerializerOptions options = null, CancellationToken cancellation = default)
        {
            return CatchExecuteAsync(async () =>
            {
                var response = await Client.PostAsJsonAsync(api, data, options, cancellation);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ServiceResult>(options, cancellation);
                return new ServiceResult { Code = (int)response.StatusCode, Message = response.ReasonPhrase };
            });
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="api">API地址。</param>
        /// <param name="data">数据实例。</param>
        /// <param name="options">JSON选项。</param>
        /// <param name="cancellation">取消标志。</param>
        /// <returns>返回发送结果。</returns>
        protected virtual Task<ServiceDataResult<TResult>> PostAsync<TResult>(string api, object data = null, JsonSerializerOptions options = null, CancellationToken cancellation = default)
        {
            return CatchExecuteAsync(async () =>
            {
                var response = await Client.PostAsJsonAsync(api, data, options, cancellation);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ServiceDataResult<TResult>>(options, cancellation);
                return new ServiceDataResult<TResult> { Code = (int)response.StatusCode, Message = response.ReasonPhrase };
            });
        }

        /// <summary>
        /// 执行<paramref name="func"/>，并且返回执行结果。
        /// </summary>
        /// <typeparam name="TResult">执行结果类型。</typeparam>
        /// <param name="func">当前请求函数。</param>
        /// <returns>返回当前请求的结果。</returns>
        protected async Task<TResult> CatchExecuteAsync<TResult>(Func<Task<TResult>> func)
            where TResult : ServiceResult, new()
        {
            try
            {
                await EnsuredAccessTokenAsync();
                return await func();
            }
            catch (Exception exception)
            {
                return new TResult { Code = (int)HttpStatusCode.BadRequest, Message = exception.Message };
            }
        }
    }
}