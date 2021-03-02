using System.Collections.Generic;
using System.Threading.Tasks;
using Gentings.Data.Internal;

namespace Gentings.Sites
{
    /// <summary>
    /// 网站相关事件。
    /// </summary>
    public abstract class SiteEventHandler<TSite> : ISiteEventHandler<TSite>
        where TSite : Site, new()
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        public virtual int Priority => int.MaxValue;

        /// <summary>
        /// 初始化默认域名。
        /// </summary>
        protected abstract IEnumerable<string> DefaultDomains { get; }

        /// <summary>
        /// 网站添加后调用的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="site">网站实例。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        public virtual bool OnCreated(IDbTransactionContext<SiteAdapter> context, TSite site)
        {
            var sd = context.As<SiteDomain>();
            foreach (var domain in DefaultDomains)
            {
                if (!sd.Create(new SiteDomain { Domain = domain, SiteId = site.Id }))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 网站添加后调用的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="site">网站实例。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        public virtual async Task<bool> OnCreatedAsync(IDbTransactionContext<SiteAdapter> context, TSite site)
        {
            var sd = context.As<SiteDomain>();
            foreach (var domain in DefaultDomains)
            {
                if (!await sd.CreateAsync(new SiteDomain { Domain = domain, SiteId = site.Id }))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 网站删除前执行的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        public virtual bool OnDelete(IDbTransactionContext<SiteAdapter> context, int[] ids)
        {
            return true;
        }

        /// <summary>
        /// 网站删除前执行的方法。
        /// </summary>
        /// <param name="context">当前事务管理器。</param>
        /// <param name="ids">网站Id列表。</param>
        /// <returns>返回触发方法后的结果，如果返回<code>false</code>将回滚事件。</returns>
        public virtual Task<bool> OnDeleteAsync(IDbTransactionContext<SiteAdapter> context, int[] ids)
        {
            return Task.FromResult(true);
        }
    }
}