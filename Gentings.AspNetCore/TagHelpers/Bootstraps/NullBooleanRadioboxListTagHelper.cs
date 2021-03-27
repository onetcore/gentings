using System.Collections.Generic;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 可空单选框列表。
    /// </summary>
    [HtmlTargetElement("gt:null-boolean-radioboxlist", Attributes = "true,false")]
    public class NullBooleanRadioboxListTagHelper : RadioboxListTagHelper
    {
        /// <summary>
        /// <code>true</code>显示的文本。
        /// </summary>
        [HtmlAttributeName("true")]
        public string TrueText { get; set; }

        /// <summary>
        /// <code>false</code>显示的文本。
        /// </summary>
        [HtmlAttributeName("false")]
        public string FalseText { get; set; }

        /// <summary>
        /// <code>null</code>显示的文本。
        /// </summary>
        [HtmlAttributeName("null")]
        public string NullText { get; set; }

        /// <summary>
        /// 附加复选项目列表，文本/值。
        /// </summary>
        /// <param name="items">复选框项目列表实例。</param>
        protected override void Init(IDictionary<string, object> items)
        {
            items.Add(TrueText, true);
            items.Add(NullText, null);
            items.Add(FalseText, false);
        }
    }
}