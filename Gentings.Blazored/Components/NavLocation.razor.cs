using System.Collections.Generic;
using Gentings.Blazored.Components.Menu;
using Microsoft.AspNetCore.Components;

namespace Gentings.Blazored.Components
{
    /// <summary>
    /// NavLocation组件。
    /// </summary>
    public partial class NavLocation
    {
        /// <summary>
        /// 当前菜单。
        /// </summary>
        [CascadingParameter]
        public MenuView MenuView { get; set; }

        private List<MenuItem> _items;
        protected override void OnParametersSet()
        {
            _items = new List<MenuItem>();
            var current = MenuView.Current;
            while (!current.IsRoot)
            {
                _items.Add(current);
                current = current.Parent;
            }
            _items.Reverse();
        }
    }
}

