using Microsoft.AspNetCore.Components;

namespace Gentings.Blazored.Components
{
    /// <summary>
    /// NavMenu组件。
    /// </summary>
    public partial class NavMenu
    {
        /// <summary>
        /// 当前菜单实例。
        /// </summary>
        [CascadingParameter]
        public MenuView MenuView { get; set; }
    }
}

