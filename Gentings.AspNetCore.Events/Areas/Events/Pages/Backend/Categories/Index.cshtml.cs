using Gentings.Extensions.Events;
using Gentings.Security;

namespace Gentings.AspNetCore.Events.Areas.Events.Pages.Backend.Categories
{
    /// <summary>
    /// 日志分类。
    /// </summary>
    [PermissionAuthorize(Permissions.EventTypes)]
    public class IndexModel : ModelBase
    {
        private readonly IEventManager _eventManager;

        public IndexModel(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        public IEnumerable<EventType> Types { get; private set; }

        public void OnGet()
        {
            Types = _eventManager.GetEventTypes();
        }
    }
}