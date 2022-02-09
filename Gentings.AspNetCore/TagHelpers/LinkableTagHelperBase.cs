using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;

namespace Gentings.AspNetCore.TagHelpers
{
    /// <summary>
    /// 具有链接网站属性的标签。
    /// </summary>
    public abstract class LinkableTagHelperBase : ViewContextableTagHelperBase
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
        private IDictionary<string, string>? _routeValues;
        private IHtmlGenerator? _generator;

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
        /// 链接地址。
        /// </summary>
        [HtmlAttributeName(HrefAttributeName)]
        public string? Href { get; set; }

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
            _generator = GetRequiredService<IHtmlGenerator>();
        }

        /// <summary>
        /// 获取链接标签。
        /// </summary>
        /// <returns>返回链接标签实例对象。</returns>
        protected TagBuilder GenerateLink()
        {
            if (Href != null)
            {
                var anchor = new TagBuilder("a");
                anchor.MergeAttribute("href", Href);
                return anchor;
            }

            RouteValueDictionary? routeValueDictionary = null;
            if (_routeValues != null && _routeValues.Count > 0)
            {
                routeValueDictionary = new RouteValueDictionary(_routeValues);
            }

            if (Area != null)
            {
                if (routeValueDictionary == null)
                {
                    routeValueDictionary = new RouteValueDictionary();
                }

                routeValueDictionary["area"] = Area;
            }
            if (Page != null || PageHandler != null)
                return _generator!.GeneratePageLink(ViewContext, string.Empty, Page, PageHandler, Protocol, Host, Fragment, routeValueDictionary, null);

            if (Controller != null || Action != null)
                return _generator!.GenerateActionLink(ViewContext, string.Empty, Action, Controller, Protocol, Host, Fragment, routeValueDictionary, null);

            return _generator!.GenerateRouteLink(ViewContext, string.Empty, Route, Protocol, Host, Fragment, routeValueDictionary, null);
        }
    }
}