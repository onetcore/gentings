namespace Gentings.Extensions.Sites.Menus
{
    /// <summary>
    /// 链接打开目标。
    /// </summary>
    public enum OpenTarget
    {
        /// <summary>
        /// 自身窗口。
        /// </summary>
        Self,
        /// <summary>
        /// 新窗口。
        /// </summary>
        Blank,
        /// <summary>
        /// 父级窗口。
        /// </summary>
        Parent,
        /// <summary>
        /// 顶级窗口。
        /// </summary>
        Top,
        /// <summary>
        /// 框架名称。
        /// </summary>
        Frame,
    }
}