using Gentings.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action-dropdownmenu")]
    public class ActionDropDownMenuTagHelper : TagHelperBase
    {
        /// <summary>
        /// 图标类型。
        /// </summary>
        [HtmlAttributeName("type")]
        public AlignMode IconType { get; set; } = AlignMode.Horizontal;

        /// <summary>
        /// 显示文本字符串。
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            return output.RenderAsync("div", async builder =>
            {
                builder.AddCssClass("dropdown");
                builder.AppendTag("a", anchor =>
                {
                    anchor.MergeAttribute("href", "#");
                    anchor.AddCssClass("dropdown-toggle");
                    anchor.AddCssClass("action-dropdown");
                    anchor.MergeAttribute("data-bs-toggle", "dropdown");
                    anchor.MergeAttribute("aria-expanded", "false");
                    anchor.AppendTag("span", span =>
                    {
                        if (IconType == AlignMode.Horizontal)
                            span.AddCssClass("bi-three-dots");
                        else
                            span.AddCssClass("bi-three-dots-vertical");
                    });
                    anchor.InnerHtml.AppendHtml(Text);
                });
                var content = await output.GetChildContentAsync();
                builder.AppendTag("div", menu =>
                {
                    menu.AddCssClass("dropdown-menu dropdown-menu-end");
                    menu.InnerHtml.AppendHtml(content);
                });
            });
        }
    }

    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action", ParentTag = "td")]
    [HtmlTargetElement("gt:action", ParentTag = "gt:action-dropdownmenu")]
    public class ActionMenuItemTagHelper : AnchorTagHelper
    {
        private readonly ILocalizer _localizer;
        /// <summary>
        /// 初始化类<see cref="ActionMenuItemTagHelper"/>。
        /// </summary>
        /// <param name="generator">HTML代码生成器。</param>
        /// <param name="localizer">本地化资源实例接口。</param>
        public ActionMenuItemTagHelper(IHtmlGenerator generator, ILocalizer localizer)
            : base(generator)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 确认属性。
        /// </summary>
        public string Confirm { get; set; }

        /// <summary>
        /// 操作类型。
        /// </summary>
        public ActionType? Type { get; set; }

        /// <summary>
        /// 图标。
        /// </summary>
        public IconType? Icon { get; set; }

        /// <summary>
        /// 图标样式名称。
        /// </summary>
        public string? IconName { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            base.Init(context);
            IconName = IconName ?? Icon?.ToDescriptionString() ?? Type?.GetIconClassName();
        }

        /// <summary>
        /// 判断是否只是普通按钮。
        /// </summary>
        public bool ActionOnly { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Type == ActionType.Divider)
            {
                output.Render("hr", hr => hr.AddCssClass("dropdown-divider"));
                return;
            }

            base.Process(context, output);
            await output.RenderAsync("a", async builder =>
            {
                if (ActionOnly)
                    builder.AddCssClass("action-only");
                else
                    builder.AddCssClass("dropdown-item");
                if (IconName != null)
                    builder.AppendTag("i", i => i.AddCssClass(IconName));

                var content = await output.GetChildContentAsync();
                if (!content.IsEmptyOrWhiteSpace)
                    builder.InnerHtml.AppendHtml(content);

                if (Confirm != null)
                    builder.MergeAttribute("_confirm", Confirm);

                ClickHandler(builder);

                if (content.IsEmptyOrWhiteSpace)
                    builder.InnerHtml.AppendHtml(_localizer[Type]);
            });
        }

        /// <summary>
        /// 点击事件处理。
        /// </summary>
        /// <param name="builder">当前标签。</param>
        protected virtual void ClickHandler(TagBuilder builder)
        {
            if (Type == ActionType.Add || Type == ActionType.Edit || Type == ActionType.Modal)
                builder.MergeAttribute("_click", "modal");
            else if (Type != null)
                builder.MergeAttribute("_click", "action");
        }
    }
}
