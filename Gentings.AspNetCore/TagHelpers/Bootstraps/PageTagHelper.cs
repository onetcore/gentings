using Gentings.AspNetCore.Properties;
using Gentings.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 分页标签。
    /// </summary> 
    [HtmlTargetElement("gt:page", Attributes = DataAttributeName)]
    public class PageTagHelper : LinkableTagHelperBase
    {
        private const string DataAttributeName = "data";

        /// <summary>
        /// 初始化类<see cref="PageTagHelper"/>。
        /// </summary>
        /// <param name="generator"><see cref="IHtmlGenerator"/>接口。</param>
        public PageTagHelper(IHtmlGenerator generator)
        {
            Generator = generator;
        }

        /// <summary>
        /// 排序。
        /// </summary>
        public override int Order => -1000;

        /// <summary>
        /// HTML辅助接口。
        /// </summary>
        protected IHtmlGenerator Generator { get; }

        /// <summary>
        /// 分页数据对象。
        /// </summary>
        [HtmlAttributeName(DataAttributeName)]
        public IPageEnumerable? Data { get; set; }

        /// <summary>
        /// 显示页码数量。
        /// </summary>
        public int Factor { get; set; } = 9;

        /// <summary>
        /// 排序。
        /// </summary>
        [HtmlAttributeName("orderby")]
        public IOrderBy? OrderBy { get; set; }

        private Func<int, TagBuilder>? _createAnchor;
        private Func<int, TagBuilder> GenerateActionLink()
        {
            var routeValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (RouteValues.Count > 0)
            {
                foreach (var routeValue in RouteValues)
                {
                    routeValues.Add(routeValue.Key, routeValue.Value);
                }
            }

            if (OrderBy != null)
            {
                routeValues["order"] = OrderBy.Order!.ToString();
                routeValues["desc"] = OrderBy.Desc.ToString() ?? "false";
            }

            if (Area != null)
                routeValues["area"] = Area;

            if (Page != null)
                return page =>
                {
                    routeValues["pageindex"] = page;
                    return Generator.GeneratePageLink(
                        ViewContext,
                        string.Empty,
                        Page,
                        PageHandler,
                        Protocol,
                        Host,
                        Fragment, routeValues, null
                    );
                };

            if (Route == null)
                return page =>
                {
                    routeValues["pageindex"] = page;
                    return Generator.GenerateActionLink(
                        ViewContext,
                        string.Empty,
                        Action,
                        Controller,
                        Protocol,
                        Host,
                        Fragment,
                        routeValues,
                        null);
                };
            return page =>
            {
                routeValues["pageindex"] = page;
                return Generator.GenerateRouteLink(
                    ViewContext,
                    string.Empty,
                    Route,
                    Protocol,
                    Host,
                    Fragment,
                    routeValues,
                    null);
            };
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Data == null)
            {
                if (ViewContext.ViewData.Model is IPageEnumerable data)
                    Data = data;
            }
            if (Data == null || Data.Pages == 0)
            {
                output.SuppressOutput();
                return;
            }
            if (Href == null && Action == null && Page == null && Route == null)
            {
                var query = new Dictionary<string, string>();
                foreach (var current in HttpContext.Request.Query)
                {
                    query[current.Key] = current.Value;
                }
                query["pageindex"] = "$page;";
                Href = $"?{string.Join("&", query.Select(x => $"{x.Key}={x.Value}"))}";
            }
            if (Href != null)
            {
                _createAnchor = page =>
                {
                    var tagBuilder = new TagBuilder("a");
                    tagBuilder.MergeAttribute("href", Href.Replace("$page;", page.ToString()));
                    return tagBuilder;
                };
            }
            else
            {
                _createAnchor = GenerateActionLink();
            }

            var builder = new TagBuilder("ul");
            builder.AddCssClass("pagination");
            var startIndex = GetRange(Data.PageIndex, Data.Pages, Factor, out var endIndex);
            if (Data.PageIndex > 1)
                builder.InnerHtml.AppendHtml(CreateLink(Data.PageIndex - 1, Resources.PageTagHelper_LastPage, Resources.PageTagHelper_LastPage));
            if (startIndex > 1)
                builder.InnerHtml.AppendHtml(CreateLink(1, "1"));
            if (startIndex > 2)
                builder.InnerHtml.AppendHtml("<li class=\"page-item\"><span>…</span></li>");
            for (var i = startIndex; i < endIndex; i++)
            {
                builder.InnerHtml.AppendHtml(CreateLink(i, i.ToString()));
            }
            if (endIndex < Data.Pages)
                builder.InnerHtml.AppendHtml("<li class=\"page-item\"><span>…</span></li>");
            if (endIndex <= Data.Pages)
                builder.InnerHtml.AppendHtml(CreateLink(Data.Pages, Data.Pages.ToString()));
            if (Data.PageIndex < Data.Pages)
                builder.InnerHtml.AppendHtml(CreateLink(Data.PageIndex + 1, Resources.PageTagHelper_NextPage, Resources.PageTagHelper_NextPage));
            output.TagName = "ul";
            output.MergeAttributes(builder);
            output.Content.AppendHtml(builder.InnerHtml);
        }

        private TagBuilder CreateLink(int pageIndex, string innerHtml, string? title = null)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("page-item");
            title ??= string.Format(Resources.PageTagHelper_NumberPage, pageIndex);

            if (Data!.PageIndex == pageIndex)
            {
                li.AddCssClass("active");
                li.AppendHtml("span", span =>
                {
                    span.AddCssClass("page-link");
                    span.MergeAttribute("title", title);
                    span.InnerHtml.AppendHtml(innerHtml);
                });
            }
            else
            {
                var anchor = _createAnchor!(pageIndex);
                anchor.AddCssClass("page-link");
                anchor.MergeAttribute("title", title);
                anchor.InnerHtml.AppendHtml(innerHtml);
                li.InnerHtml.AppendHtml(anchor);
            }
            return li;
        }

        /// <summary>
        /// 获取页面区间。
        /// </summary>
        /// <param name="pageIndex">页码。</param>
        /// <param name="pages">总页数。</param>
        /// <param name="factor">显示项数。</param>
        /// <param name="end">返回结束索引。</param>
        /// <returns>返回开始索引。</returns>
        protected static int GetRange(int pageIndex, int pages, int factor, out int end)
        {
            var item = factor / 2;
            var start = pageIndex - item;
            end = pageIndex + item;
            if (start < 1)
            {
                end += 1 - start;
                start = 1;
            }

            if (end > pages)
            {
                start -= (end - pages);
                end = pages;
            }

            if (end < 1)
            {
                end = 1;
            }

            if (start < 1)
            {
                return 1;
            }

            return start;
        }
    }
}