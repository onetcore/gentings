using Gentings.AspNetCore.RazorPages.AdminMenus;
using Gentings.Extensions.Emails;

namespace Gentings.AspNetCore.Emails
{
    /// <summary>
    /// 管理菜单。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 邮件管理。
        /// </summary>
        public const string Index = "sys.emails";

        /// <summary>
        /// 邮件配置。
        /// </summary>
        public const string Settings = "sys.emails.settings";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("sys", menu =>
                menu.AddMenu("emails",
                        it => it.Texted("邮件管理").Page("/Admin/Email/Index", area: EmailSettings.ExtensionName).Allow(Permissions.Email))
                    .AddMenu("emails.settings",
                        it => it.Texted("邮件配置").Page("/Admin/Email/Settings", area: EmailSettings.ExtensionName).Allow(Permissions.Settings)));
        }
    }
}