using System;
using System.Threading.Tasks;
using Gentings.Data;

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

    /// <summary>
    /// 开发服务管理。
    /// </summary>
    public class OpenServiceManager : IOpenServiceManager
    {
        /// <summary>
        /// 数据库操作上下文。
        /// </summary>
        protected IDbContext<OpenService> Context { get; }

        /// <summary>
        /// 初始化类<see cref="OpenServiceManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public OpenServiceManager(IDbContext<OpenService> context)
        {
            Context = context;
        }

        /// <summary>
        /// 获取或者添加开放服务实例。
        /// </summary>
        /// <param name="method">请求方法。</param>
        /// <param name="route">路由模板。</param>
        /// <param name="getService">获取服务实例。</param>
        /// <returns>返回当前获取的服务实例。</returns>
        public virtual OpenService GetOrCreate(string method, string route, Func<OpenService> getService)
        {
            var service = Context.Find(x => x.HttpMethod == method && x.Route == route);
            if (service == null)
            {
                service = getService();
                if (service != null)
                    Context.Create(service);
            }

            return service;
        }

        /// <summary>
        /// 获取或者添加开放服务实例。
        /// </summary>
        /// <param name="method">请求方法。</param>
        /// <param name="route">路由模板。</param>
        /// <param name="getService">获取服务实例。</param>
        /// <returns>返回当前获取的服务实例。</returns>
        public virtual async Task<OpenService> GetOrCreateAsync(string method, string route, Func<OpenService> getService)
        {
            var service = await Context.FindAsync(x => x.HttpMethod == method && x.Route == route);
            if (service == null)
            {
                service = getService();
                if (service != null)
                    await Context.CreateAsync(service);
            }

            return service;
        }
    }
}