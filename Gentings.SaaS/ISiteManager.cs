using System.Threading.Tasks;
using Gentings.Extensions;

namespace Gentings.SaaS
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
    }
}