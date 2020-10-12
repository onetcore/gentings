using Gentings.Extensions.Categories;

namespace Gentings.Extensions.EventLogging
{
    /// <summary>
    /// 事件类型管理接口。
    /// </summary>
    public interface IEventTypeManager : ICachableCategoryManager<EventType>, ISingletonService
    {

    }
}