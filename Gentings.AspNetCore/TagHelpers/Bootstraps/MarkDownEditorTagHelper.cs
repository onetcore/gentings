using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// MarkDown编辑器。
    /// </summary>
    [HtmlTargetElement("gt:markdown-editor")]
    public class MarkDownEditorTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 源码模型属性。
        /// </summary>
        [HtmlAttributeName("md-for")]
        public ModelExpression? SourceFor { get; set; }

        /// <summary>
        /// HTML模型属性。
        /// </summary>
        [HtmlAttributeName("html-for")]
        public ModelExpression? HtmlFor { get; set; }

        /// <summary>
        /// 源码名称。
        /// </summary>
        [HtmlAttributeName("md-name")]
        public string? SourceName { get; set; }

        /// <summary>
        /// HTML名称。
        /// </summary>
        [HtmlAttributeName("html-name")]
        public string? HtmlName { get; set; }

        /// <summary>
        /// 值。
        /// </summary>
        [HtmlAttributeName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// 上传图片文件地址。
        /// </summary>
        [HtmlAttributeName("upload")]
        public string? UploadUrl { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            ViewContext.AddLibraries(ImportLibrary.GtEditor);
            if (SourceName == null)
                SourceName = SourceFor?.Name;
            if (HtmlName == null)
                HtmlName = HtmlFor?.Name;
            if (!string.IsNullOrEmpty(Value) || SourceFor == null)
                return;
            Value = SourceFor.Model?.ToString();
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (SourceFor == null && SourceName == null)
            {
                output.SuppressOutput();
                return;
            }
            var actions = await output.GetChildContentAsync();
            output.Process("div", builder =>
            {
                if (UploadUrl != null)
                    builder.MergeAttribute("action", UploadUrl);
                builder.AddCssClass("mozmd-editor");
                builder.AppendHtml("div", toolbar =>
                {
                    toolbar.AddCssClass("mozmd-toolbar");
                    toolbar.AppendHtml("div", left =>
                    {
                        left.AddCssClass("mozmd-left");
                        if (!actions.IsEmptyOrWhiteSpace)
                            left.InnerHtml.AppendHtml(actions.GetContent().Trim());
                        ProcessToolbar(left);
                        left.AppendHtml("a", a =>
                        {
                            a.AppendHtml("i", x => x.AddCssClass("bi-eye"));
                            a.MergeAttribute("title", Resources.Mozmd_ModePreview);
                            a.AddCssClass("mozmd-mode-preview");
                        });
                    });
                    toolbar.AppendHtml("div", right =>
                    {
                        right.AddCssClass("mozmd-right");
                        ProcessRightToolbar(right);
                        right.AppendHtml("a", a =>
                        {
                            a.AppendHtml("i", x => x.AddCssClass("bi-window-fullscreen"));
                            a.MergeAttribute("title", Resources.Mozmd_FullScreen);
                            a.AddCssClass("mozmd-fullscreen");
                        });
                    });
                });
                builder.AppendHtml("div", source =>
                {
                    source.AddCssClass("mozmd-source scrollBar");
                    source.MergeAttribute("contenteditable", "plaintext-only");
                    source.InnerHtml.Append(Value);
                });
                builder.AppendHtml("div", source =>
                {
                    source.AddCssClass("mozmd-preview scrollBar txt");
                });
                builder.AppendHtml("textarea", x =>
                {
                    x.MergeAttribute("name", SourceName);
                    x.AddCssClass("mozmd-source-value");
                    x.MergeAttribute("style", "display:none", true);
                });
                if (!string.IsNullOrEmpty(HtmlName))
                {
                    builder.AppendHtml("textarea", x =>
                    {
                        x.MergeAttribute("name", HtmlName);
                        x.AddCssClass("mozmd-html-value");
                        x.MergeAttribute("style", "display:none", true);
                    });
                }
            });
        }

        /// <summary>
        /// 添加MarkDown编辑器按钮。
        /// </summary>
        /// <param name="builder">当前工具栏标签实例。</param>
        /// <param name="key">功能键。</param>
        /// <param name="icon">图标。</param>
        /// <param name="title">标题。</param>
        protected void AddButton(TagBuilder builder, string key, string icon, string title)
        {
            builder.AppendHtml("a", a =>
            {
                a.AppendHtml("i", x => x.AddCssClass(icon));
                a.MergeAttribute("title", title);
                a.AddCssClass($"mozmd-syntax-{key}");
            });
        }

        /// <summary>
        /// 添加工具栏按钮。
        /// </summary>
        /// <param name="builder">Html内容构建实例。</param>
        protected virtual void ProcessToolbar(TagBuilder builder)
        {
            AddButton(builder, "header", "bi-type-h1", Resources.Mozmd_Syntax_Header);
            AddButton(builder, "bold", "bi-type-bold", Resources.Mozmd_Syntax_Bold);
            AddButton(builder, "italic", "bi-type-italic", Resources.Mozmd_Syntax_Italic);
            AddButton(builder, "ul", "bi-list-ul", Resources.Mozmd_Syntax_Ul);
            AddButton(builder, "ol", "bi-list-ol", Resources.Mozmd_Syntax_Ol);
            AddButton(builder, "link", "bi-link", Resources.Mozmd_Syntax_Link);
            AddButton(builder, "image", "bi-image", Resources.Mozmd_Syntax_Image);
            AddButton(builder, "quote", "bi-blockquote-left", Resources.Mozmd_Syntax_Quote);
            AddButton(builder, "code", "bi-code", Resources.Mozmd_Syntax_Code);
        }

        /// <summary>
        /// 添加工具栏右边按钮。
        /// </summary>
        /// <param name="builder">Html内容构建实例。</param>
        protected virtual void ProcessRightToolbar(TagBuilder builder)
        {

        }
    }
}
