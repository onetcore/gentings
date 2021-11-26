using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gentings.Extensions.OpenServices
{
    /// <summary>
    /// 应用管理接口。
    /// </summary>
    public interface IApplicationManager
    {
        /// <summary>
        /// 通过应用Id获取应用程序实例。
        /// </summary>
        /// <param name="appId">应用Id。</param>
        /// <returns>返回当前应用实例。</returns>
        Application Find(Guid appId);

        /// <summary>
        /// 通过应用Id获取应用程序实例。
        /// </summary>
        /// <param name="appId">应用Id。</param>
        /// <returns>返回当前应用实例。</returns>
        Task<Application> FindAsync(Guid appId);

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

        /// <summary>
        /// 分页获取应用。
        /// </summary>
        /// <param name="query">查询实例。</param>
        /// <returns>应用列表。</returns>
        IPageEnumerable<Application> Load(QueryBase<Application> query);

        /// <summary>
        /// 分页获取应用。
        /// </summary>
        /// <param name="query">查询实例。</param>
        /// <returns>应用列表。</returns>
        Task<IPageEnumerable<Application>> LoadAsync(QueryBase<Application> query);

        /// <summary>
        /// 添加应用。
        /// </summary>
        /// <param name="application">应用实例。</param>
        /// <returns>返回添加结果。</returns>
        DataResult Save(Application application);

        /// <summary>
        /// 添加应用。
        /// </summary>
        /// <param name="application">应用实例。</param>
        /// <returns>返回添加结果。</returns>
        Task<DataResult> SaveAsync(Application application);

        /// <summary>
        /// 删除应用。
        /// </summary>
        /// <param name="ids">应用ID列表。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(Guid[] ids);

        /// <summary>
        /// 删除应用。
        /// </summary>
        /// <param name="ids">应用ID列表。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(Guid[] ids);
    }
}