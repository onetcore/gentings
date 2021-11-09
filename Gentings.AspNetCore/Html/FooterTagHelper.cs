using System.Linq;
using System.Text.Encodings.Web;
using Gentings.AspNetCore.StatusMessages;
using System.Threading.Tasks;
using Gentings.AspNetCore.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.Html
{
    /// <summary>
    /// 底部元素。
    /// </summary>
    [HtmlTargetElement("gt:footer")]
    public class FooterTagHelper : ViewContextableTagHelperBase
    {
        private readonly IHtmlGenerator _generator;
        private readonly JavaScriptEncoder _encoder;
        /// <summary>
        /// 初始化类<see cref="FooterTagHelper"/>。
        /// </summary>
        /// <param name="generator">HTML代码生成器。</param>
        /// <param name="encoder">脚本编码实例。</param>
        public FooterTagHelper(IHtmlGenerator generator, JavaScriptEncoder encoder)
        {
            _generator = generator;
            _encoder = encoder;
        }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var content = await output.GetChildContentAsync();
            if (!content.IsEmptyOrWhiteSpace)
                output.AppendHtml(content.GetContent().Trim());
            var status = GetStatusMessage();
            if (status != null)
            {
                output.AppendHtml("script", x =>
                {
                    x.MergeAttribute("type", "text/javascript");
                    x.InnerHtml.AppendHtml("$(function(){");
                    var type = status.Type.ToString().ToLower();
                    var json = new { message = status.Message, type };
                    x.InnerHtml.AppendHtml($"showMsg({json.ToJsonString()});");
                    x.InnerHtml.AppendHtml("});");
                });
            }
            output.AppendHtml("form", builder =>
            {
                builder.GenerateId("ajax-protected-form", "-");
                builder.MergeAttribute("method", "post");
                builder.MergeAttribute("action", HttpContext.Request.Path);
                builder.InnerHtml.AppendHtml(_generator.GenerateAntiforgery(ViewContext));
            });
        }

        /// <summary>
        /// 获取状态消息实例。
        /// </summary>
        /// <returns>返回状态消息实例。</returns>
        private StatusMessage GetStatusMessage()
        {
            var message = new StatusMessage(ViewContext.TempData);
            if (message.Message == null)
            {
                var errorMessage = ViewContext.ModelState[""]?.Errors.FirstOrDefault()?.ErrorMessage;
                if (errorMessage == null)
                    return null;
                message.Reinitialize(StatusType.Danger, errorMessage);
            }

            return message;
        }
    }
}
