using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions.Settings;

namespace Gentings.SaaS
{
    /// <summary>
    /// 网站配置数据库操作适配器。
    /// </summary>
    [Table("saas_Settings")]
    public class SiteSettingsAdapter : SettingsAdapter
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [Key]
        public int SiteId { get; set; }
    }
}