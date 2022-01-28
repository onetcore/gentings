using System.Collections.Concurrent;

namespace Gentings.Extensions.Sites.Templates
{
    /// <summary>
    /// 模板管理实现类。
    /// </summary>
    public class TemplateManager : ITemplateManager
    {
        private readonly ConcurrentDictionary<string, IPageTemplate> _templates;
        /// <summary>
        /// 初始化类<see cref="TemplateManager"/>。
        /// </summary>
        /// <param name="templates">模板列表。</param>
        public TemplateManager(IEnumerable<IPageTemplate> templates)
        {
            _templates = new(templates.ToDictionary(x => x.Name), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 模板列表。
        /// </summary>
        public IEnumerable<IPageTemplate> Templates => _templates.Values;

        /// <summary>
        /// 通过名称获取模板实例。
        /// </summary>
        /// <param name="name">唯一名称。</param>
        /// <returns>返回页面模板。</returns>
        public IPageTemplate GetTemplate(string? name)
        {
            if (!_templates.TryGetValue(name ?? PageTemplate.Default, out var template))
                template = _templates[PageTemplate.Default];
            return template;
        }
    }
}
