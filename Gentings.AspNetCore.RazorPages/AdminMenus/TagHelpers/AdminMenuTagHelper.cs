using System;
using System.Collections.Generic;
using System.Linq;
using Gentings.AspNetCore.RazorPages.TagHelpers;
using Gentings.Identity.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.AspNetCore.RazorPages.AdminMenus.TagHelpers
{
    /// <summary>
    /// 管理员菜单标签。
    /// </summary>
    [HtmlTargetElement("gt:menu", Attributes = AttributeName)]
    public class AdminMenuTagHelper : ViewContextableTagHelperBase
    {
        private readonly IMenuProviderFactory _menuProviderFactory;
        private readonly IUrlHelperFactory _factory;
        private readonly IPermissionManager _permissionManager;
        private IUrlHelper _urlHelper;
        private const string AttributeName = "provider";

        /// <summary>
        /// 菜单提供者名称。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string Provider { get; set; }

        /// <summary>
        /// 初始化类<see cref="AdminMenuTagHelper"/>。
        /// </summary>
        /// <param name="menuProviderFactory">菜单提供者工厂接口。</param>
        /// <param name="factory">URL辅助类工厂接口。</param>
        /// <param name="serviceProvider">服务提供者。</param>
        public AdminMenuTagHelper(IMenuProviderFactory menuProviderFactory, IUrlHelperFactory factory, IServiceProvider serviceProvider)
        {
            _menuProviderFactory = menuProviderFactory;
            _factory = factory;
            _permissionManager = serviceProvider.GetService<IPermissionManager>();
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            _urlHelper = _factory.GetUrlHelper(ViewContext);
            output.TagName = "ul";
            output.AddCssClass("nav flex-column");
            var items = _menuProviderFactory.GetRoots(Provider)
                .Where(IsAuthorized)//当前项
                .ToList();
            if (items.Count == 0)
                return;
            var current = ViewContext.GetCurrent(_menuProviderFactory, Provider, _urlHelper);
            foreach (var item in items)
            {
                var children = item.Where(IsAuthorized).ToList();
                var li = CreateMenuItem(item, current, children, item.Any());
                if (li == null)
                    continue;

                output.Content.AppendHtml(li);
            }
        }

        private TagBuilder CreateMenuItem(MenuItem item, MenuItem current, List<MenuItem> items, bool hasSub)
        {
            var li = new TagBuilder("li");
            li.AddCssClass("nav-item");
            if (items?.Count > 0)
            {
                CreateTriggerItem(li, item, current.IsCurrent(item));
                CreateChildren(li, items, current, item.Level);
                return li;
            }

            if (!hasSub)
            {
                var anchor = new TagBuilder("a");
                var isCurrent = current.IsCurrent(item);
                if (isCurrent)
                    anchor.AddCssClass("active");
                var url = item.LinkUrl(_urlHelper);
                anchor.MergeAttribute("href", url);
                anchor.MergeAttribute("title", item.Text);
                anchor.AddCssClass("nav-link");

                //文本
                var span = new TagBuilder("span");
                if (!string.IsNullOrWhiteSpace(item.IconName))
                    anchor.InnerHtml.AppendHtml($"<span data-feather=\"{item.IconName}\"></span>");
                else
                    span.AddCssClass("nav-text-only");
                span.AddCssClass("nav-text");
                span.InnerHtml.Append(item.Text);
                anchor.InnerHtml.AppendHtml(span);
                if (!string.IsNullOrWhiteSpace(item.BadgeText))
                //badge
                {
                    var badge = new TagBuilder("span");
                    badge.AddCssClass("badge");
                    badge.AddCssClass(item.BadgeClassName);
                    badge.InnerHtml.Append(item.BadgeText);
                    anchor.InnerHtml.AppendHtml(badge);
                }

                li.InnerHtml.AppendHtml(anchor);
                return li;
            }

            return null;
        }

        private void CreateTriggerItem(TagBuilder li, MenuItem item, bool isCurrent)
        {
            li.AppendTag("div", x =>
            {
                if (isCurrent)
                {
                    li.AddCssClass("opened");
                    x.AddCssClass("active");
                }
                x.AddCssClass("nav-link nav-trigger");
                if (item.IconName != null)
                {
                    x.AppendTag("span", s => { s.MergeAttribute("data-feather", item.IconName); });
                    x.InnerHtml.AppendHtml(item.Text);
                }
                else
                {
                    x.AppendTag("span", s =>
                    {
                        s.InnerHtml.AppendHtml(item.Text);
                        s.AddCssClass("nav-text-only");
                    });
                }
                x.AppendTag("span", s =>
                {
                    if (isCurrent)
                        s.MergeAttribute("data-feather", "chevron-down");
                    else
                        s.MergeAttribute("data-feather", "chevron-right");
                    s.AddCssClass("text-muted nav-chevron");
                });
            });
        }

        private void CreateChildren(TagBuilder li, List<MenuItem> items, MenuItem current, int level)
        {
            var ihasSub = false;
            var iul = new TagBuilder("ul");
            iul.AddCssClass($"nav flex-column mb-2 nav-indent{level}");
            foreach (var it in items.OrderByDescending(x => x.Priority))
            {
                var children = it.Where(IsAuthorized).ToList();
                var ili = CreateMenuItem(it, current, children, it.Any());
                if (ili != null)
                {
                    ihasSub = true;
                    iul.InnerHtml.AppendHtml(ili);
                }
            }
            if (ihasSub)
                li.InnerHtml.AppendHtml(iul);
        }

        /// <summary>
        /// 判断是否具有权限。
        /// </summary>
        /// <param name="item">菜单项。</param>
        /// <returns>返回验证结果。</returns>
        public bool IsAuthorized(MenuItem item)
        {
            if (item.PermissionName == null || _permissionManager == null)
                return true;
            return _permissionManager.IsAuthorized($"{item.PermissionName}");
        }
    }
}