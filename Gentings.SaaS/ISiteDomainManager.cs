using System.Collections.Generic;
using System.Threading.Tasks;
using Gentings.Extensions;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站管理接口。
    /// </summary>
    public interface ISiteDomainManager : ISingletonService
    {
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
    }
}