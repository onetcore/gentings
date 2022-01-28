using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 页面菜单标签。
    /// </summary>
    [HtmlTargetElement("gt:page-menu", Attributes = AttributeName)]
    public class PageMenuTagHelper : PageTagHelperBase
    {
        private const string AttributeName = "name";
        /// <summary>
        /// 分类名称。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string? CategoryName { get; set; }

        /// <summary>
        /// 菜单ID。
        /// </summary>
        [HtmlAttributeName("menuid")]
        public int ParentId { get; set; }

        /// <summary>
        /// 分类链接。
        /// </summary>
        [HtmlAttributeName("link")]
        public string? Link { get; set; }

        /// <summary>
        /// 内部标签样式名称。
        /// </summary>
        [HtmlAttributeName("inner")]
        public string InnerClassName { get; set; } = "nav-link";

        /// <summary>
        /// 资源文件名称。
        /// </summary>
        public string? ResourceName { get; set; }

        /// <summary>
        /// 新建菜单项目。
        /// </summary>
        /// <param name="menu">菜单实例。</param>
        /// <param name="isActive">判断是否激活。</param>
        /// <returns>返回当前标签构建实例。</returns>
        protected virtual TagBuilder CreateMenu(PageMenu menu, bool isActive)
        {
            var builder = new TagBuilder("li");
            var link = new TagBuilder("a");
            builder.AddCssClass("nav-item");
            if (isActive)
            {
                builder.AddCssClass("active");
                link.AddCssClass("active");
            }
            var display = menu.DisplayName!;
            if (ResourceName != null)
                display = GetResource(ResourceName, display) ?? display;
            var text = new TagBuilder("span");
            text.InnerHtml.AppendHtml(display);
            link.AddCssClass(InnerClassName);
            link.InnerHtml.AppendHtml(text);
            builder.InnerHtml.AppendHtml(link);
            if (menu.Target == OpenTarget.Frame)
                link.MergeAttribute("target", menu.FrameName, true);
            else if (menu.Target != OpenTarget.Self)
                link.MergeAttribute("target", $"_{menu.Target.ToString().ToLower()}");
            if (menu.Rel != null)
                link.MergeAttribute("rel", menu.Rel.ToString()!.ToLower());
            link.MergeAttribute("href", menu.LinkUrl, true);
            return builder;
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var categoryManager = GetRequiredService<IMenuCategoryManager>();
            var category = await categoryManager.FindAsync(x => x.Name.Equals(CategoryName, StringComparison.OrdinalIgnoreCase));
            if (category == null)
            {
                category = new MenuCategory { DisplayName = CategoryName, Name = CategoryName };
                categoryManager.Create(category);
            }

            output.TagName = "ul";
            output.AddClass("nav");
            var content = await output.GetChildContentAsync();
            if (ContentPosition == Position.Header)
                output.AppendHtml(content);

            if (!string.IsNullOrEmpty(Link))
                output.AppendHtml($"<li class=\"nav-item title\"><a class=\"nav-link\" href=\"{Link}\">{category.DisplayName}</a></li>");
            var current = Context?.Page;
            var menus = await GetRequiredService<IPageMenuManager>().FetchAsync(x => x.CategoryId == category.Id && x.ParentId == ParentId);
            menus = menus.OrderBy(x => x.Order).ToList();
            foreach (var menu in menus)
            {
                if (menu.DisplayMode == DisplayMode.Anonymous && HttpContext.User.Identity?.IsAuthenticated == true ||
                    menu.DisplayMode == DisplayMode.Authorized && !HttpContext.User.Identity?.IsAuthenticated == false)
                    continue;
                var item = CreateMenu(menu, menu.Id == current?.MenuId || (ViewContext.ViewData[menu.Name] is bool active && active));
                output.Content.AppendHtml(item);
            }

            if (ContentPosition == Position.Tail)
                output.AppendHtml(content);
        }

        /// <summary>
        /// 子内容位置。
        /// </summary>
        [HtmlAttributeName("items")]
        public Position ContentPosition { get; set; }

        /// <summary>
        /// 子内容位置。
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// 尾部。
            /// </summary>
            Tail,
            /// <summary>
            /// 首部。
            /// </summary>
            Header,
        }
    }
}