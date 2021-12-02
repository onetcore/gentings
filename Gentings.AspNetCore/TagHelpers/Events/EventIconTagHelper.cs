using Gentings.Extensions.Events;
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

        /// <summary>
        /// 事件Id。
        /// </summary>
        [HtmlAttributeName("id")]
        public int EventId { get; set; }

        /// <summary>
        /// 是否显示名称。
        /// </summary>
        public bool Text { get; set; }

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
            output.Render("span", builder =>
            {
                if (string.IsNullOrEmpty(type.IconName))
                {
                    builder.AddCssClass("alert");
                    builder.InnerHtml.AppendHtml(type.Name);
                }
                else if (Text)
                {
                    builder.AppendTag("i", i => i.AddCssClass(type.IconName));
                    builder.InnerHtml.AppendHtml(" ");
                    builder.InnerHtml.AppendHtml(type.Name);
                }
                else
                {

                    builder.MergeAttribute("title", type.Name);
                    builder.AddCssClass(type.IconName);
                }
                string? css = null;
                if (context.AllAttributes.TryGetAttribute("style", out var style))
                    css = style.Value?.ToString().Trim();
                if (css?.EndsWith(";") == false) css += ";";
                if (!string.IsNullOrEmpty(type.BgColor))
                    css += $"background-color:{type.BgColor};";
                if (!string.IsNullOrEmpty(type.Color))
                    css += $"color:{type.Color};";
                if (css != null)
                    builder.MergeAttribute("style", css, true);
            });
        }
    }
}