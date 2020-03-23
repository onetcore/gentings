using System;
using System.Collections.Generic;
using Gentings.AspNetCore.RazorPages.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.RazorPages.TagHelpers
{
    /// <summary>
    /// 枚举下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:enum-dropdownlist")]
    public class EnumDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly ILocalizer _localizer;
        /// <summary>
        /// 初始化类<see cref="EnumDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="localizer">本地化接口。</param>
        public EnumDropdownListTagHelper(ILocalizer localizer)
        {
            _localizer = localizer;
        }

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

        private IEnumerable<SelectListItem> GetEnumItems(Type type)
        {
            if (type.IsNullableType())
            {
                if (DefaultText == null)
                    DefaultText = Resources.DropdownListTagHelper_SelectDefaultText;
                type = Nullable.GetUnderlyingType(type);
            }
            foreach (Enum value in Enum.GetValues(type))
            {
                yield return new SelectListItem
                {
                    Text = _localizer.GetString(value),
                    Value = value.ToString()
                };
            }
        }
    }
}