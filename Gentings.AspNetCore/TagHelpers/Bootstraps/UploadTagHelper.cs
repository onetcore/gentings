using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Bootstraps
{
    /// <summary>
    /// 上传文件。
    /// </summary>
    [HtmlTargetElement("gt:upload")]
    public class UploadTagHelper : LinkableTagHelperBase
    {
        /// <summary>
        /// 名称。
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 选定对象值。
        /// </summary>
        public object? Value { get; set; }

        /// <summary>
        /// 设置属性模型。
        /// </summary>
        [HtmlAttributeName("asp-for")]
        public ModelExpression? For { get; set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            base.Init(context);
            if (string.IsNullOrEmpty(Name) && For != null)
            {
                Name = ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(For.Name);
                Value = For.Model;
            }
        }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Process("div", builder =>
            {
                builder.AddCssClass("input-group-append");
                builder.AppendHtml("input", input =>
                {
                    input.MergeAttributes(output);
                    input.MergeAttribute("autocomplete", "off");
                    input.AddCssClass("form-control uploaded");
                    input.MergeAttribute("name", Name);
                    if (Value != null)
                        input.MergeAttribute("value", Value.ToString());
                    input.GenerateId(Name, "_");
                    input.MergeAttribute("type", "text");
                    input.TagRenderMode = TagRenderMode.SelfClosing;
                });
                var link = GenerateLink();
                link.MergeAttribute("_click", "upload");
                link.AppendHtml("i", i => i.AddCssClass("bi-upload"));
                builder.AppendHtml(link);
            });
        }
    }
}
