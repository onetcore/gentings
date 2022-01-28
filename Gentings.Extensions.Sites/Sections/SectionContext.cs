using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Sites.Templates;
using Gentings.Localization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gentings.Extensions.Sites.Sections
{
    /// <summary>
    /// 节点上下文。
    /// </summary>
    public class SectionContext
    {
        internal SectionContext(Section section, PageModelContext context, ViewContext viewContext, TagBuilder container)
        {
            Section = section;
            Context = context;
            ViewContext = viewContext;
            Container = container;
        }

        /// <summary>
        /// 节点实例对象。
        /// </summary>
        public Section Section { get; }

        /// <summary>
        /// 当前页面上下文实例。
        /// </summary>
        public PageModelContext Context { get; }

        /// <summary>
        /// 节点Id。
        /// </summary>
        public int SectionId => Section?.Id ?? 0;

        /// <summary>
        /// 试图上下文。
        /// </summary>
        public ViewContext ViewContext { get; }

        /// <summary>
        /// 标签唯一Id。
        /// </summary>
        public string Id => Section.UniqueId;

        /// <summary>
        /// 内容标签实例。
        /// </summary>
        public TagBuilder Container { get; }

        /// <summary>
        /// HTTP上下文实例。
        /// </summary>
        public HttpContext HttpContext => ViewContext.HttpContext;

        private ILocalizer _localizer;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        public ILocalizer Localizer => _localizer ??= GetRequiredService<ILocalizer>();

        private ILogger _logger;
        /// <summary>
        /// 日志接口。
        /// </summary>
        public ILogger Logger =>
            _logger ??= HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
                .CreateLogger(GetType());

        /// <summary>
        /// 获取注册的服务对象。
        /// </summary>
        /// <typeparam name="TService">服务类型或者接口。</typeparam>
        /// <returns>返回当前服务的实例对象。</returns>
        public TService? GetService<TService>()
        {
            return HttpContext.RequestServices.GetService<TService>();
        }

        /// <summary>
        /// 获取已经注册的服务对象。
        /// </summary>
        /// <typeparam name="TService">服务类型或者接口。</typeparam>
        /// <returns>返回当前服务的实例对象。</returns>
        public TService GetRequiredService<TService>()
        {
            return HttpContext.RequestServices.GetRequiredService<TService>();
        }

        /// <summary>
        /// 附加HTML内容到内容标签中。
        /// </summary>
        /// <param name="content">内容实例。</param>
        public void AppendHtml(IHtmlContent content) => Container.InnerHtml.AppendHtml(content);

        /// <summary>
        /// 附加HTML内容到内容标签中。
        /// </summary>
        /// <param name="content">内容实例。</param>
        public void AppendHtml(string content) => Container.InnerHtml.AppendHtml(content);

        /// <summary>
        /// 附加HTML内容到内容标签中。
        /// </summary>
        /// <param name="tagName">标签名称。</param>
        /// <param name="action">实例化标签。</param>
        public void AppendTag(string tagName, Action<TagBuilder> action) => Container.AppendTag(tagName, action);
    }
}