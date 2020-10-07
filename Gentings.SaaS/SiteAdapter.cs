using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站配置数据库操作适配器。
    /// </summary>
    [Table("saas_Sites")]
    public class SiteAdapter
    {
        /// <summary>
        /// Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 唯一键。
        /// </summary>
        [Size(20)]
        public string SiteKey { get; set; }

        /// <summary>
        /// 网站名称。
        /// </summary>
        [Size(64)]
        public string SiteName { get; set; }

        /// <summary>
        /// 网站描述。
        /// </summary>
        [Size(256)]
        public string Description { get; set; }

        /// <summary>
        /// 禁用。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 配置的字符串或JSON格式化的字符串。
        /// </summary>
        public string SettingValue { get; set; }

        /// <summary>
        /// 将当前对象转换为<typeparamref name="TSite"/>类型实例。
        /// </summary>
        /// <typeparam name="TSite">网站类型实例。</typeparam>
        /// <returns>返回当前网站实例对象。</returns>
        public TSite AsSite<TSite>() where TSite : Site, new()
        {
            var site = Cores.FromJsonString<TSite>(SettingValue) ?? new TSite();
            site.Description = Description;
            site.Disabled = Disabled;
            site.Id = Id;
            site.SiteKey = SiteKey;
            site.SiteName = SiteName;
            return site;
        }

        /// <summary>
        /// 将网站实例转换为适配对象。
        /// </summary>
        /// <param name="site">网站配置。</param>
        /// <returns>返回网站适配对象实例。</returns>
        public static SiteAdapter FromSite(Site site)
        {
            return new SiteAdapter
            {
                Description = site.Description,
                Disabled = site.Disabled,
                Id = site.Id,
                SettingValue = site.ToJsonString(),
                SiteName = site.SiteName,
                SiteKey = site.SiteKey,
            };
        }
    }
}