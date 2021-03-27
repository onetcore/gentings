using System.Threading.Tasks;
using Gentings.Extensions.EventLogging;
using Gentings.Extensions.Notifications;
using Gentings.Extensions.Properties;
using Gentings.Extensions.Settings;

namespace Gentings.Extensions
{
    /// <summary>
    /// 页面模型基类。
    /// </summary>
    public abstract class ModelBase : AspNetCore.ModelBase
    {
        /// <summary>
        /// 获取字典字符串。
        /// </summary>
        /// <param name="key">字典唯一键。</param>
        /// <returns>返回当前字典实例。</returns>
        public string GetSettingString(string key) =>
            GetRequiredService<ISettingDictionaryManager>().GetOrAddSettings(key);

        private INotifier _notifier;
        /// <summary>
        /// 通知信息。
        /// </summary>
        protected INotifier Notifier => _notifier ??= GetRequiredService<INotifier>();

        #region events
        private IEventLogger _eventLogger;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        protected IEventLogger EventLoggers => _eventLogger ??= GetRequiredService<IEventLogger>();

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        protected void Log(string message) => EventLoggers.Log(EventType, message);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected void Log(string message, params object[] args) => EventLoggers.Log(EventType, message, args);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="message">事件消息。</param>
        protected void Log(int userId, string message) => EventLoggers.Log(userId, EventType, message);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected void Log(int userId, string message, params object[] args) => EventLoggers.Log(userId, EventType, message, args);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        protected Task LogAsync(string message) => EventLoggers.LogAsync(EventType, message);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected Task LogAsync(string message, params object[] args) => EventLoggers.LogAsync(EventType, message, args);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="message">事件消息。</param>
        protected Task LogAsync(int userId, string message) => EventLoggers.LogAsync(userId, EventType, message);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected Task LogAsync(int userId, string message, params object[] args) => EventLoggers.LogAsync(userId, EventType, message, args);

        /// <summary>
        /// 添加用户事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="message">事件消息。</param>
        protected void LogResult(DataResult result, string message) => EventLoggers.LogResult(result, EventType, message);

        /// <summary>
        /// 添加用户事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected void LogResult(DataResult result, string message, params object[] args) => EventLoggers.LogResult(result, EventType, message, args);

        /// <summary>
        /// 添加用户事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="message">事件消息。</param>
        protected Task LogResultAsync(DataResult result, string message) => EventLoggers.LogResultAsync(result, EventType, message);

        /// <summary>
        /// 添加用户事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected Task LogResultAsync(DataResult result, string message, params object[] args) => EventLoggers.LogResultAsync(result, EventType, message, args);

        /// <summary>
        /// 事件类型。
        /// </summary>
        protected virtual string EventType => Resources.EventType_Core;
        #endregion
    }
}
