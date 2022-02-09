using Gentings.Localization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 语言下拉列表框。
    /// </summary>
    [HtmlTargetElement("gt:languages")]
    [HtmlTargetElement("*", Attributes = "[dropdown=languages]")]
    public class LanguageDropdownMenuTagHelper : ViewContextableTagHelperBase
    {
        private readonly ILocalizationCulture _localizationCulture;
        /// <summary>
        /// 初始化类<see cref="LanguageDropdownMenuTagHelper"/>。
        /// </summary>
        /// <param name="localizationCulture">提供语言接口。</param>
        public LanguageDropdownMenuTagHelper(ILocalizationCulture localizationCulture)
        {
            _localizationCulture = localizationCulture;
        }

        /// <summary>
        /// 触发器A标签的样式。
        /// </summary>
        public string? ToggleClass { get; set; }

        /// <summary>
        /// 下拉菜单的样式。
        /// </summary>
        public string? MenuClass { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (output.TagName == "gt:languages")
                output.TagName = "li";
            output.AddCssClass("dropdown");
            var current = GetCurrentCulture();
            var uri = ViewContext.HttpContext.Request.GetUri();
            output.AppendHtml("a", builder =>
            {
                if (!string.IsNullOrEmpty(ToggleClass))
                    builder.AddCssClass(ToggleClass);
                builder.MergeAttribute("href", "#");
                builder.MergeAttribute("data-bs-toggle", "dropdown");
                builder.InnerHtml.AppendHtml(_localizationCulture.GetDisplayName(current));
            });
            output.AppendHtml("ul", builder =>
            {
                builder.AddCssClass("dropdown-menu");
                if (!string.IsNullOrEmpty(MenuClass))
                    builder.AddCssClass(MenuClass);
                foreach (var culture in _localizationCulture.SupportedLanguages)
                {
                    var active = culture.Key.IsCulture(current);
                    builder.AppendHtml("li", li =>
                    {
                        li.AppendHtml("a", a =>
                        {
                            a.MergeAttribute("href", GetUrl(uri, culture.Key));
                            a.AddCssClass("dropdown-item");
                            if (active)
                                a.MergeAttribute("style", "font-weight:600;");
                            a.AppendHtml("span", span =>
                            {
                                if (!active)
                                    span.MergeAttribute("style", "visibility:hidden");
                                span.AddCssClass("bi-check-lg");
                            });
                            a.InnerHtml.AppendHtml(culture.Value);
                        });
                    });
                }
            });
        }

        private string GetUrl(Uri uri, string culture)
        {
            var url = uri.AbsolutePath;
            var def = culture.Equals(_localizationCulture.DefaultCultureName, StringComparison.OrdinalIgnoreCase);
            foreach (var key in _localizationCulture.SupportedLanguages.Keys)
            {
                if (url.StartsWith($"/{key}/", StringComparison.OrdinalIgnoreCase))
                {
                    url = url.Substring(key.Length + 1);
                    break;
                }
            }
            if (!def) url = "/" + culture + url;
            return url + uri.Query;
        }

        private string GetCurrentCulture()
        {
            string culture;
            if (ViewContext.RouteData.Values.TryGetValue("culture", out var cultureInfo) && cultureInfo is not null)
                culture = cultureInfo.ToString()!;
            else
                culture = Thread.CurrentThread.CurrentUICulture.Name;
            return _localizationCulture.GetSupportedLanguage(culture);
        }
    }
}
