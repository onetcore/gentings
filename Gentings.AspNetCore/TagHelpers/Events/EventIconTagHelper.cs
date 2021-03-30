using Gentings.Extensions.Events;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Events
{
    /// <summary>
    /// 事件类型图标。
    /// </summary>
    [HtmlTargetElement("gt:event-icon")]
    public class EventIconTagHelper : TagHelperBase
    {
        private readonly IEventManager _eventManager;
        /// <summary>
        /// 初始化类<see cref="EventIconTagHelper"/>。
        /// </summary>
        /// <param name="eventManager">事件管理接口。</param>
        public EventIconTagHelper(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        [HtmlAttributeName("id")]
        public int EventId { get; set; }

        /// <summary>
        /// 访问并呈现当前标签实例。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        /// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var type = _eventManager.GetEventType(EventId);
            if (type == null)
            {
                output.SuppressOutput();
                return;
            }
            var builder = new TagBuilder("div");
            if (!string.IsNullOrEmpty(type.IconUrl))
            {
                if (type.IconUrl.IndexOf('.') == -1)
                {
                    output.TagName = "i";
                    builder.AddCssClass(type.IconUrl);
                }
                else
                {
                    output.TagName = "img";
                    builder.MergeAttribute("src", type.IconUrl);
                }
            }
            else
            {
                builder.InnerHtml.AppendHtml(type.Name);
            }

            string style = null;
            if (!string.IsNullOrEmpty(type.BgColor))
                style += $"background-color:{type.BgColor};";
            if (!string.IsNullOrEmpty(type.Color))
                style += $"color:{type.Color};";
            if (style != null)
                builder.MergeAttribute("style", style);
            output.MergeAttributes(builder);
        }
    }
}