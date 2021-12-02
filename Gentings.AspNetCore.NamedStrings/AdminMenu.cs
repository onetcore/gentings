using Gentings.AspNetCore.Menus;

namespace Gentings.AspNetCore.NamedStrings
{
    /// <summary>
    /// 后台菜单提供者。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 区域名称。
        /// </summary>
        public const string AreaName = "NamedStrings";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("sys", menu => menu
                .AddMenu("namedstrings", item => item.Texted("字典管理").Page("/backend/Index", area: AreaName))
            );
        }
    }
}
