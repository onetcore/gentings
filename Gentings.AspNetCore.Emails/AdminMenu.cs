using Gentings.AspNetCore.Menus;
using Gentings.Security;

namespace Gentings.AspNetCore.Emails
{
    /// <summary>
    /// 后台菜单提供者。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 区域名称。
        /// </summary>
        public const string AreaName = "Emails";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("emails", menu => menu.Texted("EmailsManager", IconType.Mailbox).Page("/backend/Index", area: AreaName).Allow(CorePermissions.Administrator)
                .AddMenu("index", item => item.Texted("Email_Title").Page("/backend/Index", area: AreaName).Allow(CorePermissions.Administrator))
                .AddMenu("settings", item => item.Texted("Settings_Title").Page("/backend/Settings/Index", area: AreaName).Allow(CorePermissions.Owner))
            );
        }
    }
}
