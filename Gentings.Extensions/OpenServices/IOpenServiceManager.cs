using System;
using System.Threading.Tasks;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 开发服务管理接口。
    /// </summary>
    public interface IOpenServiceManager
    {
        /// <summary>
        /// 获取或者添加开放服务实例。
        /// </summary>
        /// <param name="method">请求方法。</param>
        /// <param name="route">路由模板。</param>
        /// <param name="getService">获取服务实例。</param>
        /// <returns>返回当前获取的服务实例。</returns>
        OpenService GetOrCreate(string method, string route, Func<OpenService> getService);

        /// <summary>
        /// 获取或者添加开放服务实例。
        /// </summary>
        /// <param name="method">请求方法。</param>
        /// <param name="route">路由模板。</param>
        /// <param name="getService">获取服务实例。</param>
        /// <returns>返回当前获取的服务实例。</returns>
        Task<OpenService> GetOrCreateAsync(string method, string route, Func<OpenService> getService);
    }
}