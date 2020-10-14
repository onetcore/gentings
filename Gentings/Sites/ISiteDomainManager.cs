using System.Collections.Generic;
using System.Threading.Tasks;
using Gentings.Extensions;

namespace Gentings.Sites
{
    /// <summary>
    /// 网站管理接口。
    /// </summary>
    public interface ISiteDomainManager : ISingletonService
    {
        /// <summary>
        /// 获取当前请求的域名实例。
        /// </summary>
        SiteDomain Current { get; }

        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        SiteDomain GetDomain(string domain);

        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        Task<SiteDomain> GetDomainAsync(string domain);

        /// <summary>
        /// 添加网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <returns>返回添加结果。</returns>
        DataResult CreateDomain(SiteDomain domain);

        /// <summary>
        /// 添加网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <returns>返回添加结果。</returns>
        Task<DataResult> CreateDomainAsync(SiteDomain domain);

        /// <summary>
        /// 修改网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <param name="newDomain">新域名地址。</param>
        /// <returns>返回修改结果。</returns>
        DataResult RenameDomain(string domain, string newDomain);

        /// <summary>
        /// 修改网站域名。
        /// </summary>
        /// <param name="domain">域名地址。</param>
        /// <param name="newDomain">新域名地址。</param>
        /// <returns>返回修改结果。</returns>
        Task<DataResult> RenameDomainAsync(string domain, string newDomain);

        /// <summary>
        /// 获取网站域名列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站域名列表。</returns>
        List<SiteDomain> LoaDomains(int siteId);

        /// <summary>
        /// 获取网站域名列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回网站域名列表。</returns>
        Task<List<SiteDomain>> LoaDomainsAsync(int siteId);

        /// <summary>
        /// 判断域名是否已经存在。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        Task<bool> AnyAsync();

        /// <summary>
        /// 添加默认网站。
        /// </summary>
        /// <param name="siteKey">唯一键。</param>
        /// <param name="siteName">网站名称。</param>
        /// <param name="domains">域名列表。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateDefaultSiteAsync(string siteKey, string siteName, IEnumerable<string> domains);

        /// <summary>
        /// 添加默认网站。
        /// </summary>
        /// <param name="siteKey">唯一键。</param>
        /// <param name="siteName">网站名称。</param>
        /// <param name="shortName">网站简称。</param>
        /// <param name="description">描述。</param>
        /// <param name="domains">域名列表。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateDefaultSiteAsync(string siteKey, string siteName, string shortName, string description,
            IEnumerable<string> domains);
    }
}