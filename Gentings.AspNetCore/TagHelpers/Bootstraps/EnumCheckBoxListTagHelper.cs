using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 枚举复选框列表。
    /// </summary>
    [HtmlTargetElement("gt:enum-checkboxlist")]
    public class EnumCheckBoxListTagHelper : CheckBoxListTagHelper
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
            Type? type = null;
            if (For != null && For.Model is IEnumerable array)
            {
                type = For.ModelExplorer.ModelType.GetElementType();
                Value = array.OfType<Enum>().ToArray();
            }
            else if (IgnoreValue != null)
                type = IgnoreValue.GetType();
            else if (IgnoreValues != null)
                type = IgnoreValues.First().GetType();
            else if (Value is not null)
                type = Value.First().GetType();
            if (type != null)
                Init(items, type);
            else
                throw new Exception(Resources.EnumDropdownListTagHelper_TypeNotFound);
        }

        /// <summary>
        /// 判断选中的状态。
        /// </summary>
        /// <param name="current">当前项目值。</param>
        /// <returns>返回判断结果。</returns>
        protected override bool IsChecked(object? current)
        {
            return Value?.Contains(current) == true;
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
        public new Enum[]? Value { get; set; }

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
