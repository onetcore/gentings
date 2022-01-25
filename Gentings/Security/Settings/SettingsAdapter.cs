using Gentings.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Security.Settings
{
    /// <summary>
    /// 用户配置数据库操作适配器。
    /// </summary>
    [Table("core_Users_Settings")]
    public class SettingsAdapter 
    {
        /// <summary>
        /// 网站配置实例键。
        /// </summary>
        [Key]
        [Size(256)]
        public string SettingKey { get; set; }

        /// <summary>
        /// 用户Id。
        /// </summary>
        [Key]
        public int UserId { get; set; }

        /// <summary>
        /// 配置的字符串或JSON格式化的字符串。
        /// </summary>
        public string SettingValue { get; set; }
    }
}