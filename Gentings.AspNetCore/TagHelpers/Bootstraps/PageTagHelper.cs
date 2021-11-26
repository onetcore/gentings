using Gentings.AspNetCore.Properties;
using Gentings.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
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
    public class PageTagHelper : ViewContextableTagHelperBase
    {
        private const string ActionAttributeName = "asp-action";
        private const string ControllerAttributeName = "asp-controller";
        private const string AreaAttributeName = "asp-area";
        private const string FragmentAttributeName = "asp-fragment";
        private const string HostAttributeName = "asp-host";
        private const string ProtocolAttributeName = "asp-protocol";
        private const string RouteAttributeName = "asp-route";
        private const string RouteValuesDictionaryName = "all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        private const string HrefAttributeName = "href";
        private const string DataAttributeName = "data";
        private const string FactorAttributeName = "factor";
        private IDictionary<string, string>? _routeValues;

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
        /// 试图名称。
        /// </summary>
        [HtmlAttributeName(ActionAttributeName)]
        public string? Action { get; set; }

        /// <summary>
        /// 控制器名称。
        /// </summary>
        [HtmlAttributeName(ControllerAttributeName)]
        public string? Controller { get; set; }

        /// <summary>
        /// 区域名称。
        /// </summary>
        [HtmlAttributeName(AreaAttributeName)]
        public string? Area { get; set; }

        /// <summary>
        /// 协议如：http:或者https:等。
        /// </summary>
        [HtmlAttributeName(ProtocolAttributeName)]
        public string? Protocol { get; set; }

        /// <summary>
        /// 主机名称。
        /// </summary>
        [HtmlAttributeName(HostAttributeName)]
        public string? Host { get; set; }

        /// <summary>
        /// URL片段。
        /// </summary>
        [HtmlAttributeName(FragmentAttributeName)]
        public string? Fragment { get; set; }

        /// <summary>
        /// 路由配置名称。
        /// </summary>
        [HtmlAttributeName(RouteAttributeName)]
        public string? Route { get; set; }

        /// <summary>
        /// 路由对象列表。
        /// </summary>
        [HtmlAttributeName(RouteValuesDictionaryName, DictionaryAttributePrefix = RouteValuesPrefix)]
        public IDictionary<string, string> RouteValues
        {
            get => _routeValues ??= new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            set => _routeValues = value;
        }

        /// <summary>
        /// 分页数据对象。
        /// </summary>
        [HtmlAttributeName(DataAttributeName)]
        public IPageEnumerable? Data { get; set; }

        /// <summary>
        /// 链接地址。
        /// </summary>
        [HtmlAttributeName(HrefAttributeName)]
        public string? Href { get; set; }

        /// <summary>
        /// 显示页码数量。
        /// </summary>
        [HtmlAttributeName(FactorAttributeName)]
        public int Factor { get; set; } = 9;

        /// <summary>
        /// 网页。
        /// </summary>
        [HtmlAttributeName("asp-page")]
        public string? Page { get; set; }

        /// <summary>
        /// 网页。
        /// </summary>
        [HtmlAttributeName("asp-page-handler")]
        public string? PageHandler { get; set; }

        /// <summary>
        /// 排序。
        /// </summary>
        [HtmlAttributeName("orderby")]
        public IOrderBy OrderBy { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            base.Init(context);
            if (Area == null && ViewContext.RouteData.Values.TryGetValue("area", out var area))
                Area = area.ToString();
            if ((Controller == null || Action == null) && ViewContext.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                Controller ??= descriptor.ControllerName;
                Action ??= descriptor.ActionName;
            }
        }

        private Func<int, TagBuilder>? _createAnchor;
        private Func<int, TagBuilder> GenerateActionLink()
        {
            var routeValues = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            if (_routeValues != null && _routeValues.Count > 0)
            {
                foreach (var routeValue in _routeValues)
                {
                    routeValues.Add(routeValue.Key, routeValue.Value);
                }
            }

            if (OrderBy != null)
            {
                routeValues["order"] = OrderBy.Order.ToString();
                routeValues["desc"] = OrderBy.Desc.ToString();
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
            var startIndex = Cores.GetRange(Data.PageIndex, Data.Pages, Factor, out var endIndex);
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
            title = title ?? string.Format(Resources.PageTagHelper_NumberPage, pageIndex);

            if (Data!.PageIndex == pageIndex)
            {
                li.AddCssClass("active");
                li.AppendTag("span", span =>
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
    }
}