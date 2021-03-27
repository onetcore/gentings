using System;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 引入的库。
    /// </summary>
    [Flags]
    public enum ImportLibrary
    {
        /// <summary>
        /// 无。
        /// </summary>
        None,
        /// <summary>
        /// jQuery。
        /// </summary>
        JQuery = 1,
        /// <summary>
        /// Bootstrap。
        /// </summary>
        Bootstrap = 2,
        /// <summary>
        /// FontAwesome图标。
        /// </summary>
        FontAwesome = 4,
        /// <summary>
        /// gt-skin。
        /// </summary>
        GtCore = 8,
        /// <summary>
        /// Feather图标。
        /// </summary>
        Feather = 0x10,
        /// <summary>
        /// 脚本高亮渲染。
        /// </summary>
        Highlight = 0x20,
    }
}