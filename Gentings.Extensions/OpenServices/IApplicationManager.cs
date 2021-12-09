using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 应用管理接口。
    /// </summary>
    public interface IApplicationManager : IObjectManager<Application, Guid>
    {
        /// <summary>
        /// 获取用户应用，包含用户实例。
        /// </summary>
        /// <param name="appId">应用Id。</param>
        /// <returns>返回包含用户实例的应用类型实例。</returns>
        Task<Application> FindUserApplicationAsync(Guid appId);

        /// <summary>
        /// 获取应用程序所包含的服务Id列表。
        /// </summary>
        /// <param name="appid">应用程序Id。</param>
        /// <returns>返回服务Id列表。</returns>
        Task<List<int>> LoadApplicationServicesAsync(Guid appid);

        /// <summary>
        /// 将服务添加到应用程序中。
        /// </summary>
        /// <param name="appid">应用程序Id。</param>
        /// <param name="ids">服务Id列表。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> AddApplicationServicesAsync(Guid appid, int[] ids);
    }
}