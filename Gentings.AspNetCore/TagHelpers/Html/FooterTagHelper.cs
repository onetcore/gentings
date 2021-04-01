using Gentings.AspNetCore.StatusMessages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 底部部标签。
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
        /// 是否显示消息弹窗。
        /// </summary>
        [HtmlAttributeName("alert")]
        public bool IsAlert { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var libraries = ViewContext.ViewData.GetLibraries();
            if (IsAlert)
                libraries |= ImportLibrary.GtCore | ImportLibrary.Bootstrap;
            output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/jquery/jquery.min.js"));
            if ((libraries & ImportLibrary.Bootstrap) == ImportLibrary.Bootstrap ||
                (libraries & ImportLibrary.GtCore) == ImportLibrary.GtCore)
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/bootstrap/js/bootstrap.bundle.min.js"));
            if ((libraries & ImportLibrary.GtCore) == ImportLibrary.GtCore)
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/gtcore/dist/js/gtcore.min.js"));
            if ((libraries & ImportLibrary.Highlight) == ImportLibrary.Highlight)
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/highlight.js/highlight.min.js"));
            if ((libraries & ImportLibrary.Prettify) == ImportLibrary.Prettify)
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/prettify/prettify.min.js"));
            if ((libraries & ImportLibrary.CodeMirror) == ImportLibrary.CodeMirror)
            {
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/codemirror.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/mode/htmlmixed/htmlmixed.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/show-hint.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/javascript-hint.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/sql-hint.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/html-hint.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/xml-hint.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/anyword-hint.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/css-hint.min.js"));
                output.AppendHtml("script", x => x.MergeAttribute("src", "/lib/codemirror/addon/hint/show-hint.min.js"));
            }
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
                    if (IsAlert)
                        x.InnerHtml.AppendHtml($"GtCore.alert({json.ToJsonString()});");
                    else
                    {
                        var message = _encoder.Encode(status.Message);
                        x.InnerHtml.AppendHtml(
                            $"$('<div class=\"toast status bg-{type}\"><div class=\"toast-body\">{message}</div></div>').appendTo('body').toast('show');");
                    }

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