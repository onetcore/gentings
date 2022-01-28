namespace Gentings.Extensions.Sites.Templates
{
    /// <summary>
    /// 页面模板。
    /// </summary>
    public class PageTemplate : IPageTemplate
    {
        /// <summary>
        /// 默认模板。
        /// </summary>
        public const string Default = "默认模板";

        /// <summary>
        /// 唯一名称。
        /// </summary>
        public virtual string Name => Default;

        /// <summary>
        /// 缩略图地址。
        /// </summary>
        public virtual string? IconUrl { get; }

        /// <summary>
        /// 描述。
        /// </summary>
        public virtual string? Description { get; }

        /// <summary>
        /// 布局页面路径。
        /// </summary>
        public virtual string Path => GetDefaultPath("Index");

        /// <summary>
        /// 获取默认路径。
        /// </summary>
        /// <param name="name">文件名称。</param>
        /// <returns>返回布局页面路径。</returns>
        protected string GetDefaultPath(string name) => $"/Areas/Sites/Templates/_{name}.cshtml";
    }
}
