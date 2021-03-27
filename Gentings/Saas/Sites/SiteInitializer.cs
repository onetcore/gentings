using System.Threading.Tasks;
using Gentings.Data.Initializers;

namespace Gentings.Saas.Sites
{
    /// <summary>
    /// 初始化网站数据实例。
    /// </summary>
    /// <typeparam name="TSite">网站类型。</typeparam>
    internal class SiteInitializer<TSite> : IInitializer
        where TSite : Site, new()
    {
        private readonly ISiteManager<TSite> _siteManager;
        private readonly ISiteDomainManager _domainManager;

        /// <summary>
        /// 初始化类<see cref="SiteInitializer{TSite}"/>。
        /// </summary>
        /// <param name="siteManager">网站管理接口实例。</param>
        /// <param name="domainManager">域名管理接口。</param>
        public SiteInitializer(ISiteManager<TSite> siteManager, ISiteDomainManager domainManager)
        {
            _siteManager = siteManager;
            _domainManager = domainManager;
        }

        /// <summary>
        /// 优先级，越大越靠前。
        /// </summary>
        public int Priority => int.MaxValue;

        /// <summary>
        /// 判断是否禁用。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public Task<bool> IsDisabledAsync()
        {
            return _siteManager.AnyAsync();
        }

        /// <summary>
        /// 安装时候预先执行的接口。
        /// </summary>
        /// <returns>返回执行结果。</returns>
        public async Task<bool> ExecuteAsync()
        {
            var site = new TSite();
            if (await _siteManager.SaveAsync(site))
            {
                _domainManager.Refresh();
                return true;
            }

            return false;
        }
    }
}