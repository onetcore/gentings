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
        GtSkin = 8,
        /// <summary>
        /// 脚本高亮渲染。
        /// </summary>
        Highlight = 0x10,
        /// <summary>
        /// 脚本高亮渲染。
        /// </summary>
        Prettify = 0x20,
        /// <summary>
        /// 代码编辑器。
        /// </summary>
        CodeMirror = 0x40,
        /// <summary>
        /// MD简易编辑器。
        /// </summary>
        GtEditor = 0x80,
    }
}