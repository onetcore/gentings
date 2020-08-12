using System.Threading.Tasks;
using Gentings.Extensions;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站管理接口。
    /// </summary>
    public interface ISiteManager : ICachableObjectManager<Site>, ISingletonService
    {
        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        Site GetSite(string domain);

        /// <summary>
        /// 通过域名获取网站。
        /// </summary>
        /// <param name="domain">域名和端口地址。</param>
        /// <returns>返回当前域名的网站。</returns>
        Task<Site> GetSiteAsync(string domain);
    }
}