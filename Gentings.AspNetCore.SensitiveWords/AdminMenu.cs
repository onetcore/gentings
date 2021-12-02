using Gentings.AspNetCore.Menus;

namespace Gentings.AspNetCore.SensitiveWords
{
    /// <summary>
    /// 后台菜单提供者。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 区域名称。
        /// </summary>
        public const string AreaName = "SensitiveWords";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("sys", menu => menu
                .AddMenu("SensitiveWords", item => item.Texted("敏感词汇").Page("/backend/Index", area: AreaName))
            );
        }
    }
}
