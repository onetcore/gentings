using Gentings.Localization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action-group")]
    public class ActionGroupTagHelper : TagHelperBase
    {
        /// <summary>
        /// 对齐方式。
        /// </summary>
        public AlignMode Align { get; set; }

        /// <summary>
        /// 选中了才显示按钮。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            if (Align == AlignMode.Vertical)
                output.AddClass("btn-group-vertical");
            else
                output.AddClass("btn-group");
            if (Disabled) output.AddClass("checked-enabled disabled");
        }
    }

    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action", ParentTag = "gt:action-group")]
    public class ActionItemTagHelper : ActionMenuItemTagHelper
    {
        private readonly ILocalizer _localizer;
        /// <summary>
        /// 初始化类<see cref="ActionItemTagHelper"/>。
        /// </summary>
        /// <param name="generator">HTML代码生成器。</param>
        /// <param name="localizer">本地化资源实例接口。</param>
        public ActionItemTagHelper(IHtmlGenerator generator, ILocalizer localizer)
            : base(generator, localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 按钮类型。
        /// </summary>
        public ButtonType Mode { get; set; }

        /// <summary>
        /// 按钮类型。
        /// </summary>
        public bool Outline { get; set; }

        /// <summary>
        /// 选中了才显示按钮。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);
            await output.RenderAsync("a", async builder =>
            {
                if (Disabled) builder.AddCssClass("checked-enabled disabled");
                builder.AddCssClass("btn");
                if (Outline) builder.AddCssClass("btn-outline-" + Mode.ToLowerString());
                else builder.AddCssClass("btn-" + Mode.ToLowerString());

                if (IconName != null)
                    builder.AppendTag("i", i => i.AddCssClass(IconName));

                if (Confirm != null)
                    builder.MergeAttribute("_confirm", Confirm);

                if (Type == ActionType.Add || Type == ActionType.Modal)
                    builder.MergeAttribute("_click", "modal");
                else if (Type == ActionType.Edit)
                    builder.MergeAttribute("_click", "checked:modal");
                else if (Type != null)
                    builder.MergeAttribute("_click", "checked");

                var content = await output.GetChildContentAsync();
                builder.AppendTag("span", span =>
                {
                    if (content.IsEmptyOrWhiteSpace)
                        span.InnerHtml.AppendHtml(_localizer[Type]);
                    else
                        span.InnerHtml.AppendHtml(content);
                });
            });
        }
    }
}
