using Gentings.AspNetCore.Properties;
using Gentings.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers
{
    /// <summary>
    /// 下拉列表标签基类。
    /// </summary>
    public abstract class DropdownListTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 模型属性。
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression? For { get; set; }

        /// <summary>
        /// 默认显示字符串：“请选择”。
        /// </summary>
        [HtmlAttributeName("default-text")]
        public string? DefaultText { get; set; }

        /// <summary>
        /// 默认值。
        /// </summary>
        [HtmlAttributeName("default-value")]
        public object? DefaultValue { get; set; }

        /// <summary>
        /// 值。
        /// </summary>
        [HtmlAttributeName("value")]
        public object? Value { get; set; }

        /// <summary>
        /// 当前值类型。
        /// </summary>
        protected virtual Type? ValueType
        {
            get
            {
                if (For != null) return For.ModelExplorer.ModelType;
                return (Value ?? DefaultValue)?.GetType();
            }
        }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (DefaultText == null && ValueType?.IsNullableType() != false)
                DefaultText = Resources.DropdownListTagHelper_DefaultText;
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "select";
            output.AddClass("form-select");
            var items = Init() ?? await InitAsync() ?? Enumerable.Empty<SelectListItem>();
            items = items.ToList();
            if (!string.IsNullOrEmpty(DefaultText))//添加默认选项
                items = new[] { new SelectListItem { Text = DefaultText, Value = DefaultValue?.ToString() ?? "" } }.Concat(items).ToList();
            if (For != null)
            {
                var htmlGenerator = GetRequiredService<IHtmlGenerator>();
                var tagHelper = new SelectTagHelper(htmlGenerator);
                tagHelper.ViewContext = ViewContext;
                tagHelper.For = For;
                tagHelper.Items = items;
                tagHelper.Init(context);
                await tagHelper.ProcessAsync(context, output);
            }
            else
            {
                var value = Value?.ToString();
                if (value != null) output.SetAttribute("value", value);
                foreach (var item in items)
                {
                    output.AppendHtml("option", option =>
                    {
                        if (string.Equals(item.Value, value, StringComparison.OrdinalIgnoreCase))
                            option.MergeAttribute("selected", "selected", true);
                        option.MergeAttribute("value", item.Value);
                        option.InnerHtml.AppendHtml(item.Text);
                    });
                }
            }
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected virtual IEnumerable<SelectListItem>? Init()
        {
            return null;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected virtual Task<IEnumerable<SelectListItem>> InitAsync()
        {
            return Task.FromResult(Enumerable.Empty<SelectListItem>());
        }

        /// <summary>
        /// 迭代循环列表。
        /// </summary>
        /// <typeparam name="TGroup">分组列表。</typeparam>
        /// <param name="items">选项列表。</param>
        /// <param name="groups">当前分组。</param>
        /// <param name="filter">过滤器，筛选，排序等等操作。</param>
        /// <param name="getValue">获取值代理方法，默认为Id。</param>
        /// <param name="getText">获取字符串的代理方法，默认为Name。</param>
        protected void InitChildren<TGroup>(List<SelectListItem> items, IEnumerable<TGroup> groups,
            Func<IEnumerable<TGroup>, IEnumerable<TGroup>>? filter = null,
            Func<TGroup, string>? getValue = null,
            Func<TGroup, string>? getText = null)
            where TGroup : IGroupable<TGroup>
        {
            if (filter != null)
                groups = filter(groups);
            foreach (var group in groups)
            {
                items.Add(new SelectListItem { Text = getText?.Invoke(group) ?? group.Name, Value = getValue?.Invoke(group) ?? group.Id.ToString() });
                InitChildren(items, group.Children, filter, getValue, getText, null);
            }
        }

        private void InitChildren<TGroup>(List<SelectListItem> items, IEnumerable<TGroup> groups, Func<IEnumerable<TGroup>, IEnumerable<TGroup>>? filter, Func<TGroup, string>? getValue, Func<TGroup, string>? getText, string? header)
            where TGroup : IGroupable<TGroup>
        {
            var index = 0;
            if (filter != null)
                groups = filter(groups);
            var count = groups.Count();
            if (count == 0)
                return;
            foreach (var group in groups)
            {
                index++;
                var current = header;
                if (index < count)
                    current += "  ├─";
                else
                    current += "  └─";
                items.Add(new SelectListItem { Text = $"{current} {getText?.Invoke(group) ?? group.Name}", Value = getValue?.Invoke(group) ?? group.Id.ToString() });
                current = current.Replace("└─", " ").Replace("├─", index < count ? "│ " : "  ");
                InitChildren(items, group.Children, filter, getValue, getText, current);
            }
        }
    }
}