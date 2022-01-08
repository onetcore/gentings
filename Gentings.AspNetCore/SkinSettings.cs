namespace Gentings.AspNetCore
{
    /// <summary>
    /// 后台皮肤配置。
    /// </summary>
    public class SkinSettings
    {
        /// <summary>
        /// 皮肤模式，如果为暗黑，则菜单必须为暗黑。
        /// </summary>
        public ColorMode SkinMode { get; set; }

        /// <summary>
        /// 菜单布局，横向为顶部，纵向为左侧菜单。
        /// </summary>
        public AlignMode MenuAlign { get; set; } = AlignMode.Vertical;

        /// <summary>
        /// 菜单模式。
        /// </summary>
        public ColorMode MenuMode { get; set; } = ColorMode.Dark;

        /// <summary>
        /// 菜单是否缩成图标模式，只有菜单为左侧布局时候才可以使用。
        /// </summary>
        public bool IsMenuCollapsed { get; set; }

        /// <summary>
        /// 菜单选中模式是否为高亮背景，菜单必须在左侧才会生效。
        /// </summary>
        public bool IsMenuPills { get; set; }

        /// <summary>
        /// 是否居中模式。
        /// </summary>
        public bool IsBoxed { get; set; }

        /// <summary>
        /// 菜单和导航是否漂浮。
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// 获取菜单边栏样式名称。
        /// </summary>
        /// <returns>返回菜单边栏样式名称。</returns>
        public string NavbarClass
        {
            get
            {
                var classNames = new List<string>();
                classNames.Add("navbar");
                if (MenuMode == ColorMode.Dark || SkinMode == ColorMode.Dark)
                    classNames.Add($"navbar-{MenuMode.ToLowerString()}");
                if (IsMenuPills)
                    classNames.Add("navbar-pills");
                if (IsFixed)
                    classNames.Add("navbar-fixed");
                return classNames.Join(" ");
            }
        }

        /// <summary>
        /// 获取main标签样式名称。
        /// </summary>
        /// <returns>返回main标签样式名称。</returns>
        public string MainClass
        {
            get
            {
                var classNames = new List<string>();
                if (MenuAlign == AlignMode.Vertical)
                    classNames.Add("navbar-vertical");
                if (IsMenuCollapsed)
                    classNames.Add("navbar-collapsed");
                if (IsBoxed)
                    classNames.Add("container");
                return classNames.Join(" ");
            }
        }
    }
}
