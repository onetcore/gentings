using Gentings.AspNetCore.Properties;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// MarkDown编辑器。
    /// </summary>
    [HtmlTargetElement("gt:markdown")]
    public class MarkDownTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 源码模型属性。
        /// </summary>
        [HtmlAttributeName("md-for")]
        public ModelExpression SourceFor { get; set; }

        /// <summary>
        /// HTML模型属性。
        /// </summary>
        [HtmlAttributeName("html-for")]
        public ModelExpression HtmlFor { get; set; }

        /// <summary>
        /// 源码名称。
        /// </summary>
        [HtmlAttributeName("md-name")]
        public string SourceName { get; set; }

        /// <summary>
        /// HTML名称。
        /// </summary>
        [HtmlAttributeName("html-name")]
        public string HtmlName { get; set; }

        /// <summary>
        /// 值。
        /// </summary>
        [HtmlAttributeName("value")]
        public string Value { get; set; }

        /// <summary>
        /// 上传图片文件地址。
        /// </summary>
        [HtmlAttributeName("upload")]
        public string UploadUrl { get; set; }

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
            output.Render("div", builder =>
            {
                if (UploadUrl != null)
                    builder.MergeAttribute("action", UploadUrl);
                builder.AddCssClass("mozmd-editor");
                builder.AppendTag("div", toolbar =>
                {
                    toolbar.AddCssClass("mozmd-toolbar");
                    toolbar.AppendTag("div", left =>
                    {
                        left.AddCssClass("mozmd-left");
                        if (!actions.IsEmptyOrWhiteSpace)
                            left.InnerHtml.AppendHtml(actions.GetContent().Trim());
                        ProcessToolbar(left);
                        left.AppendTag("a", a =>
                        {
                            a.AppendTag("i", x => x.AddCssClass("bi-eye"));
                            a.MergeAttribute("title", Resources.Mozmd_ModePreview);
                            a.AddCssClass("mozmd-mode-preview");
                        });
                    });
                    toolbar.AppendTag("div", right =>
                    {
                        right.AddCssClass("mozmd-right");
                        ProcessRightToolbar(right);
                        right.AppendTag("a", a =>
                        {
                            a.AppendTag("i", x => x.AddCssClass("bi-window-fullscreen"));
                            a.MergeAttribute("title", Resources.Mozmd_FullScreen);
                            a.AddCssClass("mozmd-fullscreen");
                        });
                    });
                });
                builder.AppendTag("div", source =>
                {
                    source.AddCssClass("mozmd-source scrollBar");
                    source.MergeAttribute("contenteditable", "plaintext-only");
                    source.InnerHtml.Append(Value);
                });
                builder.AppendTag("div", source =>
                {
                    source.AddCssClass("mozmd-preview scrollBar txt");
                });
                builder.AppendTag("textarea", x =>
                {
                    x.MergeAttribute("name", SourceName);
                    x.AddCssClass("mozmd-source-value");
                    x.MergeAttribute("style", "display:none", true);
                });
                if (!string.IsNullOrEmpty(HtmlName))
                {
                    builder.AppendTag("textarea", x =>
                    {
                        x.MergeAttribute("name", HtmlName);
                        x.AddCssClass("mozmd-html-value");
                        x.MergeAttribute("style", "display:none", true);
                    });
                }
            });
        }

        /// <summary>
        /// 添加工具栏按钮。
        /// </summary>
        /// <param name="builder">Html内容构建实例。</param>
        protected virtual void ProcessToolbar(TagBuilder builder)
        {
            builder.AddSyntax("header", "bi-type-h1", Resources.Mozmd_Syntax_Header)
                   .AddSyntax("bold", "bi-type-bold", Resources.Mozmd_Syntax_Bold)
                   .AddSyntax("italic", "bi-type-italic", Resources.Mozmd_Syntax_Italic)
                   .AddSyntax("ul", "bi-list-ul", Resources.Mozmd_Syntax_Ul)
                   .AddSyntax("ol", "bi-list-ol", Resources.Mozmd_Syntax_Ol)
                   .AddSyntax("link", "bi-link", Resources.Mozmd_Syntax_Link)
                   .AddSyntax("image", "bi-image", Resources.Mozmd_Syntax_Image)
                   .AddSyntax("quote", "bi-blockquote-left", Resources.Mozmd_Syntax_Quote)
                   .AddSyntax("code", "bi-code", Resources.Mozmd_Syntax_Code);
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
