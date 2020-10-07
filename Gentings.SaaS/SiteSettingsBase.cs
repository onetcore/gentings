using System.Text.Json.Serialization;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站基类。
    /// </summary>
    public interface ISiteSettings
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [JsonIgnore]
        int SiteId { get; set; }
    }
}