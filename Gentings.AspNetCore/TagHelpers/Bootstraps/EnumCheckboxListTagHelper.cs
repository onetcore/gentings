using System;
using System.Collections.Generic;
using System.Linq;
using Gentings.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Resources = Gentings.AspNetCore.Properties.Resources;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 枚举复选框列表。
    /// </summary>
    [HtmlTargetElement("gt:enum-checkboxlist")]
    public class EnumCheckboxListTagHelper : CheckboxListTagHelper
    {
        private readonly ILocalizer _localizer;
        /// <summary>
        /// 初始化类<see cref="EnumCheckboxListTagHelper"/>。
        /// </summary>
        /// <param name="localizer">本地化接口。</param>
        public EnumCheckboxListTagHelper(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 忽略值。
        /// </summary>
        [HtmlAttributeName("ignore")]
        public Enum IgnoreValue { get; set; }

        /// <summary>
        /// 忽略值。
        /// </summary>
        [HtmlAttributeName("ignores")]
        public Enum[] IgnoreValues { get; set; }

        /// <summary>
        /// 附加复选项目列表，文本/值。
        /// </summary>
        /// <param name="items">复选框项目列表实例。</param>
        protected override void Init(IDictionary<string, object> items)
        {
            if (IgnoreValue != null)
                Init(items, IgnoreValue.GetType());
            else if (For != null)
                Init(items, For.ModelExplorer.ModelType);
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

        private object _value;
        /// <summary>
        /// 当前值。
        /// </summary>
        [HtmlAttributeName("value")]
        public new object Value
        {
            get => _value;
            set
            {
                _value = value;
                base.Value = _value?.ToString();
            }
        }

        private void Init(IDictionary<string, object> items, Type type)
        {
            if (type.IsNullableType())
                type = Nullable.GetUnderlyingType(type);
            foreach (Enum value in Enum.GetValues(type))
            {
                if (IsIgnore(value)) continue;
                items.Add(_localizer.GetString(value), value);
            }
        }
    }
}