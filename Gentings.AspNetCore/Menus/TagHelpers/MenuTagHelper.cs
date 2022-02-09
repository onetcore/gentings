using Gentings.AspNetCore.TagHelpers;
using Gentings.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.Menus.TagHelpers
{
    /// <summary>
    /// 管理员菜单标签。
    /// </summary>
    [HtmlTargetElement("gt:menu")]
    public class MenuTagHelper : ViewContextableTagHelperBase
    {
        private readonly IMenuProviderFactory _factory;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private IUrlHelper? _urlHelper;
        private readonly IPermissionAuthorizationService _authorizationService;

        /// <summary>
        /// 菜单提供者名称。
        /// </summary>
        public string Provider { get; set; } = "admin";

        /// <summary>
        /// 方向。
        /// </summary>
        public AlignMode Align { get; set; } = AlignMode.Horizontal;

        /// <summary>
        /// 初始化类<see cref="MenuTagHelper"/>。
        /// </summary>
        /// <param name="factory">菜单提供者工厂接口。</param>
        /// <param name="urlHelperFactory">URL辅助类工厂接口。</param>
        /// <param name="authorizationService">权限验证服务。</param>
        public MenuTagHelper(IMenuProviderFactory factory, IUrlHelperFactory urlHelperFactory, IPermissionAuthorizationService authorizationService)
        {
            _factory = factory;
            _urlHelperFactory = urlHelperFactory;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            _urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext)!;
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "ul";
            output.AddCssClass("navbar-nav");
            var items = _factory.GetRoots(Provider!)
                .Where(IsAuthorized)//当前项
                .ToList();
            if (items.Count == 0)
                return;
            var current = ViewContext.GetCurrent(_factory, Provider!, _urlHelper!);
            foreach (var item in items)
            {
                var children = item.Where(IsAuthorized).ToList();
                var li = CreateMenuItem(item, current, children, item.Any());
                if (li == null)
                    continue;

                output.Content.AppendHtml(li);
            }
        }

        private TagBuilder? CreateMenuItem(MenuItem item, MenuItem? current, List<MenuItem> items, bool hasSub)
        {
            var li = new TagBuilder("li");
            if (items?.Count > 0)
            {
                li.AddCssClass("nav-group");
            }
            else if (hasSub)
            {//包含子菜单，子菜单中没有一个有权限，则主菜单也没有权限
                return null;
            }
            li.AddCssClass("nav-item");
            if (item.IsTitle)
            {//分组标题
                li.AddCssClass("nav-heading");
                li.InnerHtml.AppendHtml(item.LocalizedText);
                return li;
            }
            var isCurrent = current.IsCurrent(item);
            var anchor = new TagBuilder("a");
            if (isCurrent)
                anchor.AddCssClass("active");
            anchor.MergeAttribute("title", item.LocalizedText);
            anchor.AddCssClass($"nav-link");
            //图标
            if (!string.IsNullOrWhiteSpace(item.IconName))
            {
                anchor.InnerHtml.AppendHtml($"<i class=\"{item.IconName}\"></i>");
            }
            //文本
            var span = new TagBuilder("span");
            span.InnerHtml.Append(item.LocalizedText);
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
            //子菜单
            if (items?.Count > 0)
            {
                var id = item.Name!.Replace('.', '_');
                anchor.AddCssClass("dropdown-indicator");
                if (Align == AlignMode.Vertical)
                {
                    if (isCurrent)
                        anchor.MergeAttribute("aria-expanded", "true");
                    else
                        anchor.AddCssClass("collapsed");
                    anchor.MergeAttribute("data-bs-toggle", "collapse");
                }
                anchor.MergeAttribute("href", "#" + id);
                CreateChildren(li, items, current, item.Level + 1, id, isCurrent);
            }
            else
            {
                var url = item.LinkUrl(_urlHelper!);
                anchor.MergeAttribute("href", url);
            }
            return li;
        }

        private void CreateChildren(TagBuilder li, List<MenuItem> items, MenuItem? current, int level, string id, bool isCurrent)
        {
            var ihasSub = false;
            var menu = new TagBuilder("div");
            menu.GenerateId(id, ".");
            menu.AddCssClass($"menu-{level}");
            if (Align == AlignMode.Vertical)
            {
                if (isCurrent)
                    menu.AddCssClass("show");
                menu.AddCssClass("collapse");
            }
            var ul = new TagBuilder("ul");
            menu.InnerHtml.AppendHtml(ul);
            foreach (var it in items.OrderByDescending(x => x.Priority))
            {
                var children = it.Where(IsAuthorized).ToList();
                var item = CreateMenuItem(it, current, children, it.Any());
                if (item != null)
                {
                    ihasSub = true;
                    ul.InnerHtml.AppendHtml(item);
                }
            }
            if (ihasSub)
                li.InnerHtml.AppendHtml(menu);
        }

        /// <summary>
        /// 判断是否具有权限。
        /// </summary>
        /// <param name="item">菜单项。</param>
        /// <returns>返回验证结果。</returns>
        public virtual bool IsAuthorized(MenuItem item)
        {
            if (item.PermissionName == null)
                return true;
            return _authorizationService.IsAuthorized(item.PermissionName);
        }
    }
}