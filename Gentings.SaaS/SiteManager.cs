using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站管理实现类。
    /// </summary>
    public class SiteManager : CachableObjectManager<Site>, ISiteManager
    {
        /// <summary>
        /// 初始化类<see cref="SiteManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        public SiteManager(IDbContext<Site> context, IMemoryCache cache) : base(context, cache)
        {
        }

        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        public virtual Site GetSite(string domain)
        {
            var sites = GetCacheSites();
            sites.TryGetValue(domain, out var site);
            return site;
        }

        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        public virtual async Task<Site> GetSiteAsync(string domain)
        {
            var sites = await GetCacheSitesAsync();
            sites.TryGetValue(domain, out var site);
            return site;
        }

        /// <summary>
        /// 获取网站缓存实例。
        /// </summary>
        /// <returns>返回网站缓存实例列表。</returns>
        protected virtual ConcurrentDictionary<string, Site> GetCacheSites()
        {
            return Cache.GetOrCreate(CacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var sites = Context.Fetch()
                    .OrderByDescending(x => x.Domain.Length)
                    .ToDictionary(x => x.Domain);
                return new ConcurrentDictionary<string, Site>(sites, StringComparer.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// 获取网站缓存实例。
        /// </summary>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回网站缓存实例列表。</returns>
        protected virtual Task<ConcurrentDictionary<string, Site>> GetCacheSitesAsync(CancellationToken cancellationToken = default)
        {
            return Cache.GetOrCreateAsync(CacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var sites = await Context.FetchAsync(cancellationToken: cancellationToken);
                return new ConcurrentDictionary<string, Site>(sites
                    .OrderByDescending(x => x.Domain.Length)
                    .ToDictionary(x => x.Domain), StringComparer.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例列表。</returns>
        public override IEnumerable<Site> Fetch(Expression<Predicate<Site>> expression = null)
        {
            var sites = GetCacheSites();
            return sites.Values.Filter(expression);
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回模型实例列表。</returns>
        public override async Task<IEnumerable<Site>> FetchAsync(Expression<Predicate<Site>> expression = null, CancellationToken cancellationToken = default)
        {
            var sites = await GetCacheSitesAsync(cancellationToken);
            return sites.Values.Filter(expression);
        }
    }
}