using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 枚举单选框列表。
    /// </summary>
    [HtmlTargetElement("gt:enum-radiolist")]
    public class EnumRadioListTagHelper : RadioListTagHelper
    {
        /// <summary>
        /// 忽略值。
        /// </summary>
        [HtmlAttributeName("ignore")]
        public Enum? IgnoreValue { get; set; }

        /// <summary>
        /// 忽略值。
        /// </summary>
        [HtmlAttributeName("ignores")]
        public Enum[]? IgnoreValues { get; set; }

        /// <summary>
        /// 附加复选项目列表，文本/值。
        /// </summary>
        /// <param name="items">复选框项目列表实例。</param>
        protected override void Init(IDictionary<string, object?> items)
        {
            if (For != null)
            {
                Init(items, For.ModelExplorer.ModelType);
                Value = For.Model as Enum;
            }
            else if (IgnoreValue != null)
                Init(items, IgnoreValue.GetType());
            else if (Value is Enum value)
                Init(items, value.GetType());
            else
                throw new Exception(Resources.EnumDropdownListTagHelper_TypeNotFound);
        }

        private bool IsIgnore(Enum value)
        {
            if (IgnoreValue != null)
                return Equals(value, IgnoreValue);
            if (IgnoreValues?.Length > 0)
                return IgnoreValues.Contains(value);
            return false;
        }

        /// <summary>
        /// 当前值。
        /// </summary>
        [HtmlAttributeName("value")]
        public new Enum? Value { get; set; }

        private void Init(IDictionary<string, object?> items, Type type)
        {
            if (type.IsNullableType())
                type = Nullable.GetUnderlyingType(type)!;
            foreach (Enum value in Enum.GetValues(type))
            {
                if (IsIgnore(value)) continue;
                items.Add(Localizer.GetString(value), value);
            }
        }
    }
}
