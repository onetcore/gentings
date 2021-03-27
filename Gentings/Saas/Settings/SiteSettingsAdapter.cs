using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Extensions;

namespace Gentings.Saas.Settings
{
    /// <summary>
    /// 网站配置数据库操作适配器。
    /// </summary>
    [Table("core_Sites_Settings")]
    public class SiteSettingsAdapter 
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [Key]
        public int SiteId { get; set; }

        /// <summary>
        /// 网站配置实例键。
        /// </summary>
        [Key]
        [Size(256)]
        public string SettingKey { get; set; }

        /// <summary>
        /// 配置的字符串或JSON格式化的字符串。
        /// </summary>
        public string SettingValue { get; set; }
    }
}