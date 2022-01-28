namespace Gentings.Extensions.Sites.Templates
{
    /// <summary>
    /// 页面模板。
    /// </summary>
    public interface ITemplate : IServices
    {
        /// <summary>
        /// 唯一名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 缩略图地址。
        /// </summary>
        string? IconUrl { get; }

        /// <summary>
        /// 描述。
        /// </summary>
        string? Description { get; }

        /// <summary>
        /// 布局页面路径。
        /// </summary>
        string Path { get; }
    }
}
