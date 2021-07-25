using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Gentings.Blazored.Components.Menu
{
    /// <summary>
    /// NavMenuGroup组件。
    /// </summary>
    public partial class NavMenuGroup
    {
        /// <summary>
        /// 当前菜单项目。
        /// </summary>
        [Parameter]
        public MenuItem Item { get; set; }

        /// <summary>
        /// 当前菜单实例。
        /// </summary>
        [CascadingParameter]
        public MenuView MenuView { get; set; }

        private IEnumerable<MenuItem> _children;
        /// <summary>
        /// 参数设置后排序。
        /// </summary>
        protected override void OnParametersSet()
        {
            _children = Item.OrderByDescending(x => x.Priority).ToList();
            _active = MenuView.IsActived(Item) ? " active" : null;
        }
        
        private string _active;
    }
}

