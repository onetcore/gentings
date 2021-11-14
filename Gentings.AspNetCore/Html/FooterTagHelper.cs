﻿using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;

namespace Gentings.AspNetCore.Html
{
    /// <summary>
    /// 底部元素。
    /// </summary>
    [HtmlTargetElement("gt:footer")]
    public class FooterTagHelper : ViewContextableTagHelperBase
    {
        private readonly IHtmlGenerator _generator;
        private readonly JavaScriptEncoder _scriptEncoder;
        private readonly IHostEnvironment _environment;

        /// <summary>
        /// 初始化类<see cref="FooterTagHelper"/>。
        /// </summary>
        /// <param name="generator">HTML代码生成器。</param>
        /// <param name="scriptEncoder">脚本格式化实例。</param>
        /// <param name="environment">宿主环境实例接口。</param>
        public FooterTagHelper(IHtmlGenerator generator, JavaScriptEncoder scriptEncoder, IHostEnvironment environment)
        {
            _generator = generator;
            _scriptEncoder = scriptEncoder;
            _environment = environment;
        }

        /// <summary>
        /// 是否自动挂接状态消息。
        /// </summary>
        [HtmlAttributeName("status")]
        public bool AttachStatusMessage { get; set; }

        /// <summary>
        /// 是否生成Antiforgery验证标记表单实例。
        /// </summary>
        [HtmlAttributeName("antiforgery")]
        public bool GenerateAntiforgery { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            AppendLibraries(output);
            var content = await output.GetChildContentAsync();
            if (!content.IsEmptyOrWhiteSpace)
                output.AppendHtml(content.GetContent().Trim());
            if (AttachStatusMessage)
            {
                var status = GetStatusMessage();
                if (status != null)
                {
                    output.AppendHtml("script", x =>
                    {
                        x.MergeAttribute("type", "text/javascript");
                        x.InnerHtml.AppendHtml("$(function(){");
                        x.InnerHtml.AppendHtml($"if(window.showMsg){{showMsg({status.ToJsonString()});}}else{{alert('{_scriptEncoder.Encode(status.Message)}');}}");
                        x.InnerHtml.AppendHtml("});");
                    });
                }
            }
            if (GenerateAntiforgery)
            {
                output.AppendHtml("form", builder =>
                {
                    builder.GenerateId("ajax-protected-form", "-");
                    builder.MergeAttribute("method", "post");
                    builder.MergeAttribute("action", HttpContext.Request.Path);
                    builder.InnerHtml.AppendHtml(_generator.GenerateAntiforgery(ViewContext));
                });
            }
        }

        /// <summary>
        /// 获取状态消息实例。
        /// </summary>
        /// <returns>返回状态消息实例。</returns>
        private StatusMessage? GetStatusMessage()
        {
            var message = new StatusMessage(ViewContext.TempData);
            if (message.Message == null)
            {
                var errorMessage = ViewContext.ModelState[""]?.Errors.FirstOrDefault()?.ErrorMessage;
                if (errorMessage == null)
                    return null;
                message.Reinitialize(-1, errorMessage);
            }

            return message;
        }

        private void AppendLibraries(TagHelperOutput output)
        {
            var isDevelopment = _environment.IsDevelopment();
            var libraries = ViewContext.GetLibraries();
            output.AppendScript("/lib/jquery/jquery", isDevelopment);
            if ((libraries & ImportLibrary.Bootstrap) == ImportLibrary.Bootstrap ||
                (libraries & ImportLibrary.GtSkin) == ImportLibrary.GtSkin)
                output.AppendScript("/lib/bootstrap/js/bootstrap.bundle", isDevelopment);
            if ((libraries & ImportLibrary.GtSkin) == ImportLibrary.GtSkin)
                output.AppendScript("/js/gt-skin", isDevelopment);
            if ((libraries & ImportLibrary.Highlight) == ImportLibrary.Highlight)
            {
                output.AppendScript("/lib/highlight.js/highlight", isDevelopment);
                output.AppendHtml("<script>$(function(){hljs.highlightAll();});</script>");
            }
            if ((libraries & ImportLibrary.Prettify) == ImportLibrary.Prettify)
                output.AppendScript("/lib/prettify/prettify", isDevelopment);
            if ((libraries & ImportLibrary.CodeMirror) == ImportLibrary.CodeMirror)
            {
                output.AppendScript("/lib/codemirror/codemirror", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/mode/loadmode", isDevelopment);
                //output.AppendScript("/lib/codemirror/mode/htmlmixed/htmlmixed", isDevelopment);
                // 代码提示
                output.AppendScript("/lib/codemirror/addon/hint/show-hint", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/hint/javascript-hint", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/hint/sql-hint", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/hint/html-hint", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/hint/xml-hint", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/hint/anyword-hint", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/hint/css-hint", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/hint/show-hint", isDevelopment);
                // 代码折叠
                output.AppendScript("/lib/codemirror/addon/selection/active-line", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/fold/foldcode", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/fold/foldgutter", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/fold/brace-fold", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/fold/comment-fold", isDevelopment);
                output.AppendScript("/lib/codemirror/addon/fold/xml-fold", isDevelopment);
                // 匹配代码
                output.AppendScript("/js/codemirror", isDevelopment);
            }
        }
    }
}
