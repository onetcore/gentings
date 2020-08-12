using System.Text.Json.Serialization;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站基类。
    /// </summary>
    public abstract class SiteSettingsBase
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [JsonIgnore]
        public virtual int SiteId { get; set; }
    }
}