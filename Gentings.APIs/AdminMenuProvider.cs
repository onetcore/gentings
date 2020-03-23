using Gentings.AspNetCore.RazorPages.AdminMenus;

namespace Gentings.APIs
{
    public class AdminMenuProvider : MenuProvider
    {
        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("dashboard", item => item.Texted("控制面板", "home").Page("/Index"));
        }
    }
}