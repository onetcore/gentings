using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 排序标签。
    /// </summary>
    [HtmlTargetElement("gt:orderby")]
    public class OrderByTagHelper : ViewContextableTagHelperBase
    {
        private readonly HtmlEncoder _encoder;
        private const string RouteValuesDictionaryName = "all-route-data";
        private const string RouteValuesPrefix = "asp-route-";
        /// <summary>
        /// 排序。
        /// </summary>
        [HtmlAttributeName("order")]
        public Enum OrderBy { get; set; }

        private IDictionary<string, string> _routeValues;
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
        /// 初始化类<see cref="OrderByTagHelper"/>。
        /// </summary>
        /// <param name="encoder">Html编码实例。</param>
        public OrderByTagHelper(HtmlEncoder encoder)
        {
            _encoder = encoder;
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            RouteValues["order"] = OrderBy.ToString();
            var box = new TagBuilder("a");
            box.InnerHtml.AppendHtml(await output.GetChildContentAsync());
            output.TagName = box.TagName;
            if (HttpContext.Request.Query.TryGetValue("order", out var order) &&
                order == OrderBy.ToString())
            {
                var icon = new TagBuilder("i");
                box.InnerHtml.AppendHtml(icon);
                icon.AddCssClass("fa ml-1");
                if (HttpContext.Request.Query.TryGetValue("desc", out var value) &&
                    bool.TryParse(value, out var desc) &&
                    desc)
                {
                    RouteValues.Remove("desc");
                    icon.AddCssClass("fa-angle-down");
                }
                else
                {
                    RouteValues["desc"] = "true";
                    icon.AddCssClass("fa-angle-up");
                }
            }
            box.AddCssClass("orderby");
            var href = RouteValues.Select(x => $"{x.Key}={_encoder.Encode(x.Value)}").Join("&");
            box.MergeAttribute("href", $"?{href}", true);
            output.MergeAttributes(box);
            output.Content.AppendHtml(box);
        }
    }
}