using Gentings.Extensions.Sites.Sections;
using Gentings.Extensions.Sites.Templates;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using static System.Collections.Specialized.BitVector32;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 页面模型上下文。
    /// </summary>
    public class PageContext
    {
        /// <summary>
        /// 当前页面模型。
        /// </summary>
        public Page Page { get; }

        /// <summary>
        /// 模板实例。
        /// </summary>
        public ITemplate Template { get; }

        private readonly IDictionary<string?, Section> _sections;
        private readonly IServiceProvider _services;

        /// <summary>
        /// 初始化类型<see cref="PageContext"/>。
        /// </summary>
        /// <param name="page">当前页面实例。</param>
        /// <param name="services">当前服务提供者接口。</param>
        /// <param name="sections">当前页面未禁用的节点列表。</param>
        /// <param name="settings">网站配置。</param>
        public PageContext(Page page, IServiceProvider services, IEnumerable<Section> sections, SiteSettings settings)
        {
            Page = page;
            _services = services;
            Template = services.GetRequiredService<ITemplateManager>().GetTemplate(page.TemplateName);
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

        /// <summary>
        /// 呈现节点。
        /// </summary>
        /// <returns>返回节点的HTML代码。</returns>
        public async Task<IHtmlContent> RenderSectionsAsync()
        {
            var content = new HtmlContentBuilder();
            var sectionManager = _services.GetRequiredService<ISectionManager>();
            foreach (var section in Sections)
            {
                var html = await RenderSectionAsync(sectionManager.GetSectionRender(section.RenderName), section);
                content.AppendHtml(html);
            }
            return content;
        }

        /// <summary>
        /// 获取当个节点呈现的HTML实例。
        /// </summary>
        /// <param name="render">当前节点呈现实例对象。</param>
        /// <param name="section">当前节点实例对象。</param>
        /// <returns>返回当前节点的HTML实例。</returns>
        public async Task<IHtmlContent> RenderSectionAsync(ISectionRender render, Section section)
        {
            var builder = new TagBuilder("section");
            var isFluid = section.IsFluid ?? Page.IsFluid ?? Settings.IsFluid;
            if (section.Name != null)
                builder.AddCssClass(section.Name);
            if (isFluid == true)
                builder.AddCssClass("container-fluid");
            else if (isFluid == false)
                builder.AddCssClass("container");
            builder.GenerateId(section.UniqueId, "_");
            var sctx = new SectionContext(this, section);
            await render.ProcessAsync(sctx, builder);
            return builder;
        }
    }
}
