using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 可空下拉框列表。
    /// </summary>
    [HtmlTargetElement("gt:enabled-dropdownlist")]
    public class EnabledDropdownListTagHelper : DropdownListTagHelper
    {
        /// <summary>
        /// <code>true</code>显示的文本。
        /// </summary>
        [HtmlAttributeName("enabled")]
        public string? EnabledText { get; set; }

        /// <summary>
        /// <code>false</code>显示的文本。
        /// </summary>
        [HtmlAttributeName("disabled")]
        public string? DisabledText { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (DefaultText == null) DefaultText = Resources.EnabledDropdownListTagHelper_Default;
            if (EnabledText == null) EnabledText = Resources.EnabledDropdownListTagHelper_True;
            if (DisabledText == null) DisabledText = Resources.EnabledDropdownListTagHelper_False;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override IEnumerable<SelectListItem>? Init()
        {
            yield return new SelectListItem(DefaultText, "");
            yield return new SelectListItem(EnabledText, "true");
            yield return new SelectListItem(DisabledText, "false");
        }
    }
}