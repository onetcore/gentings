using System;
using System.Threading.Tasks;
using Gentings.AspNetCore;
using Gentings.Properties;
using Microsoft.AspNetCore.Http;

namespace Gentings.Extensions.Events
{
    /// <summary>
    /// 事件日志实现类。
    /// </summary>
    public class EventLogger : IEventLogger
    {
        private readonly IEventManager _eventManager;
        private readonly IHttpContextAccessor _contextAccessor;
        /// <summary>
        /// 初始化类<see cref="EventLogger"/>。
        /// </summary>
        /// <param name="eventManager">事件管理接口。</param>
        /// <param name="contextAccessor">HTTP上下文访问接口。</param>
        public EventLogger(IEventManager eventManager, IHttpContextAccessor contextAccessor)
        {
            _eventManager = eventManager;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="init">实例化代理方法。</param>
        /// <param name="eventType">事件类型名称。</param>
        public void Log(Action<Event> init, string eventType = null)
        {
            // 事件类型
            var type = _eventManager.GetEventType(eventType ?? Resources.EventType);
            if (type == null)
            {
                type = new EventType();
                type.Name = eventType;
                if (!_eventManager.Create(type))
                    return;
            }
            // 事件实例
            var @event = new Event();
            @event.EventId = type.Id;
            var context = _contextAccessor.HttpContext;
            if (context != null)
            {
                @event.IPAdress = context.GetUserAddress();
                @event.UserId = context.User.GetUserId();
            }
            // 实例化事件
            init(@event);
            if (!string.IsNullOrEmpty(@event.Message))
                _eventManager.Create(@event);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="init">实例化代理方法。</param>
        /// <param name="eventType">事件类型名称。</param>
        public async Task LogAsync(Action<Event> init, string eventType = null)
        {
            // 事件类型
            var type = await _eventManager.GetEventTypeAsync(eventType ?? Resources.EventType);
            if (type == null)
            {
                type = new EventType();
                type.Name = eventType;
                if (!await _eventManager.CreateAsync(type))
                    return;
            }
            // 事件实例
            var @event = new Event();
            @event.EventId = type.Id;
            var context = _contextAccessor.HttpContext;
            if (context != null)
            {
                @event.IPAdress = context.GetUserAddress();
                @event.UserId = context.User.GetUserId();
            }
            // 实例化事件
            init(@event);
            if (!string.IsNullOrEmpty(@event.Message))
                await _eventManager.CreateAsync(@event);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        /// <param name="eventType">事件类型名称。</param>
        public virtual void Log(Exception exception, string eventType = null)
        {
            Log(@event =>
            {
                @event.Message = exception.Message;
                @event.Data = exception.StackTrace;
                @event.Source = exception.Source;
                @event.Level = EventLevel.Error;
            }, eventType);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        /// <param name="eventType">事件类型名称。</param>
        public virtual Task LogAsync(Exception exception, string eventType = null)
        {
            return LogAsync(@event =>
            {
                @event.Message = exception.Message;
                @event.Data = exception.StackTrace;
                @event.Source = exception.Source;
                @event.Level = EventLevel.Error;
            }, eventType);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        public virtual void Log(string message, string eventType = null, EventLevel level = EventLevel.Success, string source = null)
        {
            Log(@event =>
            {
                @event.Message = message;
                @event.Source = source;
                @event.Level = level;
            }, eventType);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        public virtual Task LogAsync(string message, string eventType = null, EventLevel level = EventLevel.Success, string source = null)
        {
            return LogAsync(@event =>
            {
                @event.Message = message;
                @event.Source = source;
                @event.Level = level;
            }, eventType);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="source">来源。</param>
        public virtual void LogResult(DataResult result, string name, string eventType = null, string source = null)
        {
            if (!result) return;
            Log(@event =>
            {
                @event.Message = result.ToString(name);
                @event.Source = source;
                @event.Level = EventLevel.Success;
            }, eventType);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="eventType">事件类型名称。</param>
        /// <param name="source">来源。</param>
        public virtual async Task LogResultAsync(DataResult result, string name, string eventType = null, string source = null)
        {
            if (!result) return;
            await LogAsync(@event =>
            {
                @event.Message = result.ToString(name);
                @event.Source = source;
                @event.Level = EventLevel.Success;
            }, eventType);
        }
    }
}