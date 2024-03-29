﻿using Gentings.Documents.TableOfContent;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Documents
{
    /// <summary>
    /// 后台管理导航。
    /// </summary>
    [HtmlTargetElement("gt:toc-navigator", Attributes = AttributeName)]
    public class TocNavigatorTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = "data";

        /// <summary>
        /// 当前数据对象。
        /// </summary>
        public Toc? Data { get; set; }

        /// <summary>
        /// 呈现标记。
        /// </summary>
        /// <param name="context">标记辅助上下文。</param>
        /// <param name="output">输出实例。</param>
        /// <returns>返回执行任务。</returns>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Data == null) return;
            output.TagName = "ol";
            output.AddCssClass("breadcrumb");
            var current = Data.GetByHref(ViewContext.HttpContext.Request.GetUri().AbsolutePath);
            var navigators = LoadNavigators(current).ToList();
            if (navigators.Count == 0)
                return;
            navigators.Reverse();
            var links = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
            foreach (var navigator in navigators)
            {
                links[navigator.Name!] = navigator.Href;
            }
            if (!string.IsNullOrEmpty(Home))
            {
                links.Remove(Home);
                output.Content.AppendHtml($"<li><a href=\"{Href}\">{Home}</a></li>");
            }
            foreach (var link in links)
            {
                output.Content.AppendHtml(CreateLink(link.Value!, link.Key));
            }
        }

        private TagBuilder CreateLink(string linkUrl, string text)
        {
            var builder = new TagBuilder("li");
            builder.AddCssClass("breadcrumb-item");
            if (linkUrl != null)
            {
                var anchor = new TagBuilder("a");
                anchor.MergeAttribute("href", linkUrl);
                anchor.InnerHtml.AppendHtml(text);
                builder.InnerHtml.AppendHtml(anchor);
            }
            else
            {
                builder.AddCssClass("active");
                builder.InnerHtml.AppendHtml(text);
            }
            return builder;
        }

        private IEnumerable<TocItem> LoadNavigators(TocItem? current)
        {
            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        /// <summary>
        /// 首页链接地址。
        /// </summary>
        public string? Href { get; set; }

        /// <summary>
        /// 首页名称。
        /// </summary>
        public string? Home { get; set; }
    }
}