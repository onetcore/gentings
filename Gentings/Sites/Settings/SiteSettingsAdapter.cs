using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Settings;

namespace Gentings.Sites.Settings
{
    /// <summary>
    /// 网站配置数据库操作适配器。
    /// </summary>
    [Table("core_Sites_Settings")]
    public class SiteSettingsAdapter : SettingsAdapter
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [Key]
        public int SiteId { get; set; }
    }
}