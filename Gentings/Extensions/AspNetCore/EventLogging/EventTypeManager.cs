using Gentings.Data;
using Gentings.Extensions.Categories;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.AspNetCore.EventLogging
{
    /// <summary>
    /// 事件类型管理。
    /// </summary>
    public abstract class EventTypeManager : CachableCategoryManager<EventType>, IEventTypeManager
    {
        /// <summary>
        /// 初始化类<see cref="EventTypeManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        protected EventTypeManager(IDbContext<EventType> context, IMemoryCache cache)
            : base(context, cache)
        {
        }
    }
}