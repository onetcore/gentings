using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 可空单选框列表。
    /// </summary>
    [HtmlTargetElement("gt:enabled-radioboxlist")]
    public class EnabledRadioListTagHelper : RadioListTagHelper
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
        /// <code>null</code>显示的文本。
        /// </summary>
        [HtmlAttributeName("default")]
        public string? NullText { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (NullText == null) NullText = Resources.EnabledRadioListTagHelper_Default;
            if (EnabledText == null) EnabledText = Resources.EnabledRadioListTagHelper_True;
            if (DisabledText == null) DisabledText = Resources.EnabledRadioListTagHelper_False;
        }

        /// <summary>
        /// 附加复选项目列表，文本/值。
        /// </summary>
        /// <param name="items">复选框项目列表实例。</param>
        protected override void Init(IDictionary<string, object> items)
        {
            items.Add(EnabledText, true);
            items.Add(NullText, null);
            items.Add(DisabledText, false);
        }
    }
}