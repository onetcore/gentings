using Gentings.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.AspNetCore.EventLogging
{
    internal class DefaultEventTypeManager : EventTypeManager
    {
        /// <summary>
        /// 初始化类<see cref="DefaultEventTypeManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        public DefaultEventTypeManager(IDbContext<EventType> context, IMemoryCache cache) : base(context, cache)
        {
        }
    }
}