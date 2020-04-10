using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Gentings.AspNetCore.EventLogging
{
    internal class DefaultEventLogger : EventLogger
    {
        /// <summary>
        /// 初始化类<see cref="DefaultEventLogger"/>。
        /// </summary>
        /// <param name="eventTypeManager">事件类型管理接口。</param>
        /// <param name="eventManager">事件管理接口。</param>
        /// <param name="httpContextAccessor">HTTP上下文访问器。</param>
        /// <param name="logger">日志接口。</param>
        public DefaultEventLogger(IEventTypeManager eventTypeManager, IEventManager eventManager, IHttpContextAccessor httpContextAccessor, ILogger<EventLogger> logger) : base(eventTypeManager, eventManager, httpContextAccessor, logger)
        {
        }
    }
}