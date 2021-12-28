using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers
{
    /// <summary>
    /// 枚举下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:enum-dropdownlist")]
    public class EnumDropdownListTagHelper : DropdownListTagHelper
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
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override IEnumerable<SelectListItem> Init()
        {
            if (For != null)
                return GetEnumItems(For.ModelExplorer.ModelType);
            if (Value is Enum value)
                return GetEnumItems(value.GetType());
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

        private IEnumerable<SelectListItem> GetEnumItems(Type type)
        {
            if (type.IsNullableType())
            {
                if (DefaultText == null)
                    DefaultText = Resources.DropdownListTagHelper_DefaultText;
                type = Nullable.GetUnderlyingType(type)!;
            }
            foreach (Enum value in Enum.GetValues(type!))
            {
                if (IsIgnore(value)) continue;
                yield return new SelectListItem
                {
                    Text = Localizer.GetString(value),
                    Value = value.ToString()
                };
            }
        }
    }
}