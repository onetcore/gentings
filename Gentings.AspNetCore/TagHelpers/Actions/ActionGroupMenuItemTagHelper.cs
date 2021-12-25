using Gentings.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Actions
{
    /// <summary>
    /// 操作下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:action", ParentTag = "gt:action-groupmenu")]
    public class ActionGroupMenuItemTagHelper : ActionMenuItemTagHelper
    {
        /// <summary>
        /// 初始化类<see cref="ActionGroupMenuItemTagHelper"/>。
        /// </summary>
        /// <param name="generator">HTML代码生成器。</param>
        /// <param name="localizer">本地化资源实例接口。</param>
        public ActionGroupMenuItemTagHelper(IHtmlGenerator generator, ILocalizer localizer)
            : base(generator, localizer)
        {
        }

        /// <summary>
        /// 选中了才显示按钮。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 点击事件处理。
        /// </summary>
        /// <param name="builder">当前标签。</param>
        protected override void ClickHandler(TagBuilder builder)
        {
            if (Disabled) builder.AddCssClass("checked-enabled disabled");
            if (Type == ActionType.Add || Type == ActionType.Modal)
                builder.MergeAttribute("_click", "modal");
            else if (Type == ActionType.Upload)
                builder.MergeAttribute("_click", "upload");
            else if (Type == ActionType.Edit)
                builder.MergeAttribute("_click", "checked:modal");
            else if (Type != null)
                builder.MergeAttribute("_click", "checked");
        }
    }
}
