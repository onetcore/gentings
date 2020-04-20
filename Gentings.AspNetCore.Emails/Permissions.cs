using Gentings.Extensions.Emails;
using Gentings.Identity.Permissions;

namespace Gentings.AspNetCore.Emails
{
    /// <summary>
    /// 权限。
    /// </summary>
    public class Permissions : PermissionProvider
    {
        /// <summary>
        /// 分类。
        /// </summary>
        public override string Category { get; } = EmailSettings.ExtensionName;

        /// <summary>
        /// 邮件管理。
        /// </summary>
        public const string Email = "email.index";

        /// <summary>
        /// 邮件配置管理。
        /// </summary>
        public const string Settings = "email.settings";

        /// <summary>
        /// 初始化权限实例。
        /// </summary>
        protected override void Init()
        {
            Add("index", "邮件管理", "允许管理邮件相关操作!");
            Add("settings", "邮件配置", "允许管理邮件配置相关操作!");
        }
    }
}