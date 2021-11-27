using Gentings.AspNetCore.Menus;

namespace Gentings.AspNetCore.Storages
{
    /// <summary>
    /// 管理菜单。
    /// </summary>
    public class AdminMenu : MenuProvider
    {
        /// <summary>
        /// 区域名称。
        /// </summary>
        public const string AreaName = "Storages";

        /// <summary>
        /// 初始化菜单实例。
        /// </summary>
        /// <param name="root">根目录菜单。</param>
        public override void Init(MenuItem root)
        {
            root.AddMenu("storages", menu => menu.Texted("文件存储管理", IconType.Files).Page("/Backend/Index", area: AreaName)
                .AddMenu("index", it => it.Texted("媒体文件管理").Page("/Backend/Index", area: AreaName))
            );
        }
    }
}
