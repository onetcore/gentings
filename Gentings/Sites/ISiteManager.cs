using System.Collections.Generic;
using System.Threading.Tasks;
using Gentings.Extensions;

namespace Gentings.Sites
{
    /// <summary>
    /// 网站管理接口。
    /// </summary>
    /// <typeparam name="TSite">网站实例。</typeparam>
    public interface ISiteManager<TSite>
        where TSite : Site, new()
    {
        /// <summary>
        /// 获取网站实例。
        /// </summary>
        /// <param name="id">网站Id。</param>
        /// <returns>返回网站实例。</returns>
        TSite Find(int id);

        /// <summary>
        /// 获取网站实例。
        /// </summary>
        /// <param name="id">网站Id。</param>
        /// <returns>返回网站实例。</returns>
        Task<TSite> FindAsync(int id);

        /// <summary>
        /// 保存当前实例。
        /// </summary>
        /// <param name="site">网站实例对象。</param>
        /// <returns>返回保存结果。</returns>
        DataResult Save(TSite site);

        /// <summary>
        /// 保存当前实例。
        /// </summary>
        /// <param name="site">网站实例对象。</param>
        /// <returns>返回保存结果。</returns>
        Task<DataResult> SaveAsync(TSite site);

        /// <summary>
        /// 删除当前实例。
        /// </summary>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回删除结果。</returns>
        DataResult Delete(int[] ids);

        /// <summary>
        /// 删除当前实例。
        /// </summary>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回删除结果。</returns>
        Task<DataResult> DeleteAsync(int[] ids);

        /// <summary>
        /// 获取激活的网站列表。
        /// </summary>
        /// <returns>返回所有激活的网站列表。</returns>
        IEnumerable<Site> Load();

        /// <summary>
        /// 获取激活的网站列表。
        /// </summary>
        /// <returns>返回所有激活的网站列表。</returns>
        Task<IEnumerable<Site>> LoadAsync();

        /// <summary>
        /// 分页查询网站实例。
        /// </summary>
        /// <param name="query">网站查询实例。</param>
        /// <returns>返回当前网站列表。</returns>
        IPageEnumerable<Site> Load(SiteQuery query);

        /// <summary>
        /// 分页查询网站实例。
        /// </summary>
        /// <param name="query">网站查询实例。</param>
        /// <returns>返回当前网站列表。</returns>
        Task<IPageEnumerable<Site>> LoadAsync(SiteQuery query);

        /// <summary>
        /// 启用网站。
        /// </summary>
        /// <param name="ids">启用Id列表。</param>
        /// <returns>返回启用结果。</returns>
        bool Enabled(int[] ids);

        /// <summary>
        /// 启用网站。
        /// </summary>
        /// <param name="ids">启用Id列表。</param>
        /// <returns>返回启用结果。</returns>
        Task<bool> EnabledAsync(int[] ids);

        /// <summary>
        /// 禁用网站。
        /// </summary>
        /// <param name="ids">禁用Id列表。</param>
        /// <returns>返回禁用结果。</returns>
        bool Disabled(int[] ids);

        /// <summary>
        /// 禁用网站。
        /// </summary>
        /// <param name="ids">禁用Id列表。</param>
        /// <returns>返回禁用结果。</returns>
        Task<bool> DisabledAsync(int[] ids);

        /// <summary>
        /// 是否已经有网站实例。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        bool Any();

        /// <summary>
        /// 是否已经有网站实例。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        Task<bool> AnyAsync();
    }
}