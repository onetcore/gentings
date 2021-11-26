using Gentings.Extensions.Events;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers.Events
{
    /// <summary>
    /// 事件类型。
    /// </summary>
    [HtmlTargetElement("gt:event-dropdownlist")]
    public class EventDropdownListTagHelper : DropdownListTagHelper
    {
        private readonly IEventManager _eventManager;
        /// <summary>
        /// 初始化类<see cref="EventDropdownListTagHelper"/>。
        /// </summary>
        /// <param name="eventManager">事件管理接口。</param>
        public EventDropdownListTagHelper(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// 初始化选项列表。
        /// </summary>
        /// <returns>返回选项列表。</returns>
        protected override IEnumerable<SelectListItem> Init()
        {
            return _eventManager.GetEventTypes()
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()))
                .ToList();
        }
    }
}