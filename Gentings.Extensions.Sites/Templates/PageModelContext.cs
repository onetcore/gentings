namespace Gentings.Extensions.Sites.Templates
{
    /// <summary>
    /// 页面模型上下文。
    /// </summary>
    public class PageModelContext
    {
        /// <summary>
        /// 当前页面模型。
        /// </summary>
        public Page Page { get; }

        /// <summary>
        /// 模板实例。
        /// </summary>
        public IPageTemplate Template { get; }

        private readonly IDictionary<string?, Section> _sections;
        /// <summary>
        /// 初始化类型<see cref="PageModelContext"/>。
        /// </summary>
        /// <param name="page">当前页面实例。</param>
        /// <param name="sections">节点列表。</param>
        /// <param name="template">模板实例。</param>
        /// <param name="settings">网站配置。</param>
        public PageModelContext(Page page, IEnumerable<Section> sections, IPageTemplate template, SiteSettings settings)
        {
            Page = page;
            Template = template;
            _sections = sections.ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
            Sections = _sections.Values.Where(x => !x.IsPaged).OrderBy(x => x.Order).ToList();
            Settings = settings;
        }

        /// <summary>
        /// 节点列表。
        /// </summary>
        public IEnumerable<Section> Sections { get; }

        /// <summary>
        /// 网站配置。
        /// </summary>
        public SiteSettings Settings { get; }
    }
}
