using System.Text.Json.Serialization;
using Gentings.Extensions;

namespace Gentings.SaaS
{
    /// <summary>
    /// 配置基类。
    /// </summary>
    [Target(typeof(SiteAdapter))]
    public class Site
    {
        /// <summary>
        /// Id。
        /// </summary>
        [JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 唯一键。
        /// </summary>
        [JsonIgnore]
        public string SiteKey { get; set; }

        /// <summary>
        /// 网站名称。
        /// </summary>
        [JsonIgnore]
        public string SiteName { get; set; }

        /// <summary>
        /// 网站描述。
        /// </summary>
        [JsonIgnore]
        public string Description { get; set; }

        /// <summary>
        /// 禁用。
        /// </summary>
        [JsonIgnore]
        public bool Disabled { get; set; }
    }
}