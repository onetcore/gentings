using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Html
{
    /// <summary>
    /// 头部标签。
    /// </summary>
    [HtmlTargetElement("gt:header", Attributes = AttributeName)]
    public class HeaderTagHelper : ViewContextableTagHelperBase
    {
        private const string AttributeName = "title";
        /// <summary>
        /// 标题。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string Title { get; set; }

        /// <summary>
        /// 关键词。
        /// </summary>
        [HtmlAttributeName("keyword")]
        public string Keyword { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [HtmlAttributeName("description")]
        public string Description { get; set; }

        /// <summary>
        /// 异步访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            output.Content.AppendHtml($"<title>{Title}</title>");
            if (!string.IsNullOrWhiteSpace(Keyword))
                output.Content.AppendHtml($"<meta name=\"keyword\" content=\"{Keyword}\" />");
            if (!string.IsNullOrWhiteSpace(Description))
                output.Content.AppendHtml($"<meta name=\"description\" content=\"{Description}\" />");
            AppendLibraries(output);
            output.AppendHtml(await output.GetChildContentAsync());
        }

        private void AppendLibraries(TagHelperOutput output)
        {
            var libraries = ViewContext.ViewData.GetLibraries();
            if ((libraries & ImportLibrary.FontAwesome) == ImportLibrary.FontAwesome)
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/font-awesome/css/font-awesome.min.css\" />");
            if ((libraries & ImportLibrary.Bootstrap) == ImportLibrary.Bootstrap ||
                (libraries & ImportLibrary.GtCore) == ImportLibrary.GtCore)
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/bootstrap/css/bootstrap.min.css\" />");
            if ((libraries & ImportLibrary.BootstrapIcons) == ImportLibrary.BootstrapIcons)
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/bootstrap-icons/font/bootstrap-icons.min.css\" />");
            if ((libraries & ImportLibrary.GtCore) == ImportLibrary.GtCore)
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/gtcore/dist/css/gtcore.min.css\" />");
            if ((libraries & ImportLibrary.Highlight) == ImportLibrary.Highlight)
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/highlight.js/styles/vs2015.min.css\" />");
            if ((libraries & ImportLibrary.Prettify) == ImportLibrary.Prettify)
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/prettify/prettify.min.css\" />");
            if ((libraries & ImportLibrary.CodeMirror) == ImportLibrary.CodeMirror)
            {
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/codemirror/codemirror.min.css\" />");
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/codemirror/theme/idea.min.css\" />");
                output.Content.AppendHtml("<link rel=\"stylesheet\" href=\"/lib/codemirror/addon/hint/show-hint.min.css\" />");
            }
        }
    }
}