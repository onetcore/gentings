using Gentings.Documents.TableOfContent;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Documents
{
    /// <summary>
    /// Toc菜单。
    /// </summary>
    [HtmlTargetElement("gt:toc-menu", Attributes = AttributeName)]
    public class TocMenuTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = "data";

        /// <summary>
        /// 当前数据对象。
        /// </summary>
        public Toc? Data { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if(Data == null)
            {
                output.SuppressOutput();
                return;
            }
            output.TagName = "ul";
            output.AddCssClass("navbar-nav");
            var current = Data.GetByHref(ViewContext.HttpContext.Request.GetUri().AbsolutePath);
            foreach (var item in Data)
            {
                var builder = CreateMenuItem(item, current, 0);
                if (builder != null)
                    output.AppendHtml(builder);
            }
        }

        private TagBuilder CreateMenuItem(TocItem item, TocItem? current, int menu)
        {
            var li = new TagBuilder("li");
            if (item.Items.Count > 0)
            {
                li.AddCssClass("nav-group");
            }
            li.AddCssClass("nav-item");
            var active = current?.IsCurrent(item) ?? false;
            var anchor = new TagBuilder("a");
            if (active) anchor.AddCssClass("active");
            anchor.MergeAttribute("title", item.Name);
            anchor.AddCssClass($"nav-link");
            var span = new TagBuilder("span");
            span.InnerHtml.Append(item.Name);
            anchor.InnerHtml.AppendHtml(span);
            li.InnerHtml.AppendHtml(anchor);
            if (item.Items.Count > 0)
            {
                anchor.AddCssClass("dropdown-indicator");
                if (active)
                    anchor.MergeAttribute("aria-expanded", "true");
                else
                    anchor.AddCssClass("collapsed");
                anchor.MergeAttribute("data-bs-toggle", "collapse");
                anchor.MergeAttribute("href", "#" + item.Id);
                CreateChildren(li, item.Items, current, menu + 1, item.Id, active);
            }
            else
            {
                var url = item.Href;
                anchor.MergeAttribute("href", url);
            }
            return li;
        }

        private void CreateChildren(TagBuilder li, List<TocItem> items, TocItem? current, int level, string id, bool active)
        {
            var ihasSub = false;
            var menu = new TagBuilder("div");
            menu.MergeAttribute("id", id);
            menu.AddCssClass($"menu-{level}");
            if (active) menu.AddCssClass("show");
            menu.AddCssClass("collapse");
            var ul = new TagBuilder("ul");
            menu.InnerHtml.AppendHtml(ul);
            foreach (var it in items)
            {
                var item = CreateMenuItem(it, current, level);
                if (item != null)
                {
                    ihasSub = true;
                    ul.InnerHtml.AppendHtml(item);
                }
            }
            if (ihasSub)
                li.InnerHtml.AppendHtml(menu);
        }
    }
}
