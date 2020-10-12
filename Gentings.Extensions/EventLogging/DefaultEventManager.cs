using Gentings.Data;

namespace Gentings.Extensions.EventLogging
{
    internal class DefaultEventManager : EventManager
    {
        /// <summary>
        /// 初始化类<see cref="DefaultEventManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public DefaultEventManager(IDbContext<EventMessage> context) : base(context)
        {
        }
    }
}