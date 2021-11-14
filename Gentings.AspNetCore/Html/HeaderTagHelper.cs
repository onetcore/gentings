using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;

namespace Gentings.AspNetCore.Html
{
    /// <summary>
    /// 头部标签。
    /// </summary>
    [HtmlTargetElement("gt:header", Attributes = AttributeName)]
    public class HeaderTagHelper : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 初始化类<see cref="HeaderTagHelper"/>。
        /// </summary>
        /// <param name="environment">宿主环境变量实例。</param>
        public HeaderTagHelper(IHostEnvironment environment)
        {
            _environment = environment;
        }

        private const string AttributeName = "title";
        private readonly IHostEnvironment _environment;

        /// <summary>
        /// 标题。
        /// </summary>
        [HtmlAttributeName(AttributeName)]
        public string? Title { get; set; }

        /// <summary>
        /// 关键词。
        /// </summary>
        [HtmlAttributeName("keyword")]
        public string? Keyword { get; set; }

        /// <summary>
        /// 描述。
        /// </summary>
        [HtmlAttributeName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// 默认引入的脚本库。
        /// </summary>
        public ImportLibrary Import { get; set; }

        /// <summary>
        /// 初始化配置。
        /// </summary>
        /// <param name="context">标签上下文。</param>
        public override void Init(TagHelperContext context)
        {
            if (Import != ImportLibrary.None)
                ViewContext.AddLibraries(Import);
        }

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
            var isDevelopment = _environment.IsDevelopment();
            var libraries = ViewContext.GetLibraries();
            if ((libraries & ImportLibrary.FontAwesome) == ImportLibrary.FontAwesome)
                output.AppendStyle("/lib/font-awesome/css/font-awesome", isDevelopment);
            if ((libraries & ImportLibrary.Bootstrap) == ImportLibrary.Bootstrap ||
                (libraries & ImportLibrary.GtSkin) == ImportLibrary.GtSkin)
            {
                output.AppendStyle("/css/gt-skin", isDevelopment);
                output.AppendStyle("/lib/bootstrap-icons/font/bootstrap-icons", isDevelopment);
            }
            if ((libraries & ImportLibrary.Highlight) == ImportLibrary.Highlight)
                output.AppendStyle("/lib/highlight.js/styles/vs2015", isDevelopment);
            if ((libraries & ImportLibrary.Prettify) == ImportLibrary.Prettify)
                output.AppendStyle("/lib/prettify/prettify", isDevelopment);
            if ((libraries & ImportLibrary.CodeMirror) == ImportLibrary.CodeMirror)
            {
                output.AppendStyle("/lib/codemirror/codemirror", isDevelopment);
                output.AppendStyle("/lib/codemirror/theme/eclipse", isDevelopment);
                output.AppendStyle("/lib/codemirror/addon/hint/show-hint", isDevelopment);
                output.AppendStyle("/lib/codemirror/addon/fold/foldgutter", isDevelopment);
            }
        }
    }
}