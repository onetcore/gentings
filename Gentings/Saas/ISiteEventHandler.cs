using System.Threading.Tasks;
using Gentings.Data.Internal;
using Gentings.Saas.Sites;

namespace Gentings.Saas
{
    /// <summary>
    /// 网站处理接口。
    /// </summary>
    /// <typeparam name="TSite">网站类型。</typeparam>
    public interface ISiteEventHandler<TSite> : ISingletonServices
        where TSite : Site, new()
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 网站添加后调用的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="site">网站实例。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        bool OnCreated(IDbTransactionContext<SiteAdapter> context, TSite site);

        /// <summary>
        /// 网站添加后调用的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="site">网站实例。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        Task<bool> OnCreatedAsync(IDbTransactionContext<SiteAdapter> context, TSite site);

        /// <summary>
        /// 网站删除前执行的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        bool OnDelete(IDbTransactionContext<SiteAdapter> context, int[] ids);

        /// <summary>
        /// 网站删除前执行的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        Task<bool> OnDeleteAsync(IDbTransactionContext<SiteAdapter> context, int[] ids);
    }
}