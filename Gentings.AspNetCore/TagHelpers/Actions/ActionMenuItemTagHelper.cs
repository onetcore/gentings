﻿using Gentings.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Actions
{
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
        public string? Confirm { get; set; }

        /// <summary>
        /// 操作类型。
        /// </summary>
        [HtmlAttributeName("typeof")]
        public ActionType Type { get; set; }

        /// <summary>
        /// 图标。
        /// </summary>
        public IconType Icon { get; set; } = IconType.None;

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
            if (IconName == null)
            {
                if (Icon != IconType.None)
                    IconName = Icon.ToDescriptionString();
                else
                    IconName = Type.GetIconClassName();
            }
            if (!RouteValues.ContainsKey("handler"))
            {
                switch (Type)
                {
                    case ActionType.Delete:
                    case ActionType.MoveUp:
                    case ActionType.MoveDown:
                    case ActionType.Upload:
                        RouteValues.Add("handler", Type.ToString());
                        break;
                }
            }
        }

        /// <summary>
        /// 是否只为链接。
        /// </summary>
        [HtmlAttributeName("link")]
        public bool IsLink { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Type == ActionType.Divider)
            {
                output.Process("hr", hr => hr.AddCssClass("dropdown-divider"));
                return;
            }

            base.Process(context, output);
            await output.ProcessAsync("a", async builder =>
            {
                if (IsLink)
                    builder.AddCssClass("action-link");
                else
                    builder.AddCssClass("dropdown-item");
                if (IconName != null)
                    builder.AppendHtml("i", i => i.AddCssClass(IconName));

                var content = await output.GetChildContentAsync();
                if (content.IsEmptyOrWhiteSpace)
                {
                    if (Type != ActionType.Link)
                        builder.InnerHtml.AppendHtml(_localizer[Type]);
                }
                else
                    builder.InnerHtml.AppendHtml(content);

                if (Confirm != null)
                    builder.MergeAttribute("_confirm", Confirm);

                ClickHandler(builder);
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
            else if (Type == ActionType.Upload)
                builder.MergeAttribute("_click", "upload");
            else if (Type != ActionType.Link)
                builder.MergeAttribute("_click", "ajax");
            if (Type == ActionType.MoveDown)
                builder.AddCssClass("l-hide");
            else if (Type == ActionType.MoveUp)
                builder.AddCssClass("f-hide");
        }
    }
}
