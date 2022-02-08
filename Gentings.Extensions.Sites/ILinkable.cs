namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 链接地址接口。
    /// </summary>
    public interface ILinkable
    {
        /// <summary>
        /// 链接地址。
        /// </summary>
        public string LinkUrl { get; }

        /// <summary>
        /// 打开方式。
        /// </summary>
        public OpenTarget Target { get; }

        /// <summary>
        /// 框架名称。
        /// </summary>
        public string? FrameName { get; }

        /// <summary>
        /// a和link标签的rel属性。
        /// </summary>
        public LinkRel? Rel { get; }
    }
}