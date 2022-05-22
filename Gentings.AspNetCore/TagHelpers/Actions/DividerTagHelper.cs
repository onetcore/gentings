using Gentings.Localization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Actions
{
    /// <summary>
    /// 分隔符。
    /// </summary>
    [HtmlTargetElement("divider", ParentTag = "td", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("divider", ParentTag = "gt:action-dropdownmenu", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("gt:divider", ParentTag = "td", TagStructure = TagStructure.NormalOrSelfClosing)]
    [HtmlTargetElement("gt:divider", ParentTag = "gt:action-dropdownmenu", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class DividerTagHelper : ActionMenuItemTagHelper
    {
        /// <summary>
        /// 初始化类<see cref="DividerTagHelper"/>。
        /// </summary>
        /// <param name="generator">HTML代码生成器。</param>
        /// <param name="localizer">本地化资源实例接口。</param>
        public DividerTagHelper(IHtmlGenerator generator, ILocalizer localizer) : base(generator, localizer)
        {
            Type = ActionType.Divider;
        }
    }
}
