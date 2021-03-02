using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentings.AspNetCore;
using Gentings.Data;
using Gentings.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Sites
{
    /// <summary>
    /// 网站管理实现类。
    /// </summary>
    public class SiteDomainManager : ISiteDomainManager
    {
        private readonly IDbContext<SiteDomain> _context;
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Type _cacheKey = typeof(SiteDomain);

        /// <summary>
        /// 初始化类<see cref="SiteDomainManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        /// <param name="contextAccessor">HTTP请求上下文。</param>
        public SiteDomainManager(IDbContext<SiteDomain> context, IMemoryCache cache, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _cache = cache;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 获取当前请求的域名实例。
        /// </summary>
        public SiteDomain Current
        {
            get
            {
                var domain = _contextAccessor.HttpContext.Request.GetDomain();
                return GetDomain(domain);
            }
        }

        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        public virtual SiteDomain GetDomain(string domain)
        {
            var sites = GetCacheDomains();
            sites.TryGetValue(domain, out var site);
            return site;
        }

        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        public virtual async Task<SiteDomain> GetDomainAsync(string domain)
        {
            var sites = await GetCacheDomainsAsync();
            sites.TryGetValue(domain, out var site);
            return site;
        }

        /// <summary>
        /// 如果结果正确返回<paramref name="succeed"/>，否则返回失败项。
        /// </summary>
        /// <param name="result">执行结果。</param>
        /// <param name="succeed">执行成功返回的值。</param>
        /// <returns>返回执行结果实例对象。</returns>
        protected DataResult FromResult(bool result, DataAction succeed)
        {
            if (result)
            {
                Refresh();
                return succeed;
            }
            return (DataAction)(-(int)succeed);
        }

        /// <summary>
        /// 添加网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <returns>返回添加结果。</returns>
        public virtual DataResult CreateDomain(SiteDomain domain)
        {
            var sites = GetCacheDomains();
            if (sites.TryGetValue(domain.Domain, out _))
                return DataAction.Duplicate;
            return FromResult(_context.Create(domain), DataAction.Created);
        }

        /// <summary>
        /// 添加网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <returns>返回添加结果。</returns>
        public virtual async Task<DataResult> CreateDomainAsync(SiteDomain domain)
        {
            var sites = await GetCacheDomainsAsync();
            if (sites.TryGetValue(domain.Domain, out _))
                return DataAction.Duplicate;
            return FromResult(await _context.CreateAsync(domain), DataAction.Created);
        }

        /// <summary>
        /// 修改网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <param name="newDomain">新域名地址。</param>
        /// <returns>返回修改结果。</returns>
        public virtual DataResult RenameDomain(string domain, string newDomain)
        {
            var sites = GetCacheDomains();
            if (sites.TryGetValue(domain, out _))
                return DataAction.Duplicate;
            return FromResult(_context.Update(domain, new { Domain = newDomain }), DataAction.Updated);
        }

        /// <summary>
        /// 修改网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <param name="newDomain">新域名地址。</param>
        /// <returns>返回修改结果。</returns>
        public virtual async Task<DataResult> RenameDomainAsync(string domain, string newDomain)
        {
            var sites = await GetCacheDomainsAsync();
            if (sites.TryGetValue(domain, out _))
                return DataAction.Duplicate;
            return FromResult(await _context.UpdateAsync(domain, new { Domain = newDomain }), DataAction.Updated);
        }

        /// <summary>
        /// 获取网站域名列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站域名列表。</returns>
        public virtual List<SiteDomain> LoaDomains(int siteId)
        {
            var sites = GetCacheDomains();
            return sites.Values.Where(x => x.SiteId == siteId).ToList();
        }

        /// <summary>
        /// 获取网站域名列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站域名列表。</returns>
        public virtual async Task<List<SiteDomain>> LoaDomainsAsync(int siteId)
        {
            var sites = await GetCacheDomainsAsync();
            return sites.Values.Where(x => x.SiteId == siteId).ToList();
        }

        /// <summary>
        /// 判断域名是否已经存在。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public virtual Task<bool> AnyAsync() => _context.AnyAsync();

        /// <summary>
        /// 添加默认网站。
        /// </summary>
        /// <param name="siteKey">唯一键。</param>
        /// <param name="siteName">网站名称。</param>
        /// <param name="domains">域名列表。</param>
        /// <returns>返回添加结果。</returns>
        public Task<bool> CreateDefaultSiteAsync(string siteKey, string siteName, IEnumerable<string> domains)
        {
            return CreateDefaultSiteAsync(siteKey, siteName, siteName, null, domains);
        }

        /// <summary>
        /// 添加默认网站。
        /// </summary>
        /// <param name="siteKey">唯一键。</param>
        /// <param name="siteName">网站名称。</param>
        /// <param name="shortName">网站简称。</param>
        /// <param name="description">描述。</param>
        /// <param name="domains">域名列表。</param>
        /// <returns>返回添加结果。</returns>
        public virtual async Task<bool> CreateDefaultSiteAsync(string siteKey, string siteName, string shortName, string description, IEnumerable<string> domains)
        {
            if (await _context.BeginTransactionAsync(async db =>
            {
                var site = new SiteAdapter
                {
                    SiteKey = siteKey,
                    SiteName = siteName,
                    ShortName = shortName ?? siteName,
                    Description = description
                };
                if (await db.As<SiteAdapter>().CreateAsync(site))
                {
                    foreach (var domain in domains)
                    {
                        var siteDomain = new SiteDomain
                        {
                            Domain = domain,
                            SiteId = site.Id
                        };
                        await db.CreateAsync(siteDomain);
                    }

                    return true;
                }

                return false;
            }))
            {
                Refresh();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 刷新缓存。
        /// </summary>
        public virtual void Refresh()
        {
            _cache.Remove(_cacheKey);
        }

        /// <summary>
        /// 获取网站缓存实例。
        /// </summary>
        /// <returns>返回网站缓存实例列表。</returns>
        protected virtual ConcurrentDictionary<string, SiteDomain> GetCacheDomains()
        {
            return _cache.GetOrCreate(_cacheKey, ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var domains = _context.Fetch()
                    .OrderByDescending(x => x.Domain.Length)
                    .ToDictionary(x => x.Domain);
                return new ConcurrentDictionary<string, SiteDomain>(domains, StringComparer.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// 获取网站缓存实例。
        /// </summary>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回网站缓存实例列表。</returns>
        protected virtual Task<ConcurrentDictionary<string, SiteDomain>> GetCacheDomainsAsync(CancellationToken cancellationToken = default)
        {
            return _cache.GetOrCreateAsync(_cacheKey, async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                var domains = await _context.FetchAsync(cancellationToken: cancellationToken);
                return new ConcurrentDictionary<string, SiteDomain>(domains
                    .OrderByDescending(x => x.Domain.Length)
                    .ToDictionary(x => x.Domain), StringComparer.OrdinalIgnoreCase);
            });
        }
    }
}