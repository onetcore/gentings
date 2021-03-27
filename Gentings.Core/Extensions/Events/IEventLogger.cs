using System;
using System.Threading.Tasks;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件日志接口。
    /// </summary>
    public interface IEventLogger : ISingletonService
    {
        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="init">实例化代理方法。</param>
        /// <param name="eventType">事件类型名称。</param>
        void Log(Action<Event> init, string eventType = null);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="init">实例化代理方法。</param>
        /// <param name="eventType">事件类型名称。</param>
        Task LogAsync(Action<Event> init, string eventType = null);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        /// <param name="eventType">事件类型名称。</param>
        void Log(Exception exception, string eventType = null);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        /// <param name="eventType">事件类型名称。</param>
        Task LogAsync(Exception exception, string eventType = null);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        void Log(string message, string eventType = null, EventLevel level = EventLevel.Success, string source = null);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        Task LogAsync(string message, string eventType = null, EventLevel level = EventLevel.Success, string source = null);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="source">来源。</param>
        void LogResult(DataResult result, string name, string eventType = null, string source = null);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="source">来源。</param>
        Task LogResultAsync(DataResult result, string name, string eventType = null, string source = null);
    }
}