using Gentings.Extensions;
using Gentings.Extensions.Events;
using Gentings.Security;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Events.Areas.Events.Pages.Backend
{
    /// <summary>
    /// 日志。
    /// </summary>
    [PermissionAuthorize(Permissions.Events)]
    public class IndexModel : ModelBase
    {
        private readonly IEventManager _eventManager;
        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="eventManager">事件管理接口。</param>
        public IndexModel(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// 查询实例。
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public EventQuery Query { get; set; }

        /// <summary>
        /// 事件列表。
        /// </summary>
        public IPageEnumerable<Event> Items { get; set; }

        /// <summary>
        /// 分页获取事件列表。
        /// </summary>
        public void OnGet()
        {
            if (Query.End != null)
                Query.End = Query.End.Value.Add(new TimeSpan(23, 59, 59));
            Items = _eventManager.Load(Query);
        }
    }
}