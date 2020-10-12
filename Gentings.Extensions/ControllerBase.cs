using System.Threading.Tasks;
using Gentings.Extensions.EventLogging;
using Gentings.Extensions.Notifications;
using Gentings.Extensions.Properties;
using Gentings.Extensions.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    public abstract class ControllerBase : Gentings.AspNetCore.ControllerBase
    {
        /// <summary>
        /// 获取对象对比实例。
        /// </summary>
        /// <param name="instance">当前对象实例，在更改对象实例之前的实例。</param>
        /// <returns>返回当前实例。</returns>
        protected IObjectDiffer GetObjectDiffer(object instance)
        {
            var differ = GetRequiredService<IObjectDiffer>();
            differ.Stored(instance);
            return differ;
        }

        /// <summary>
        /// 从表单中读取扩展属性。
        /// </summary>
        /// <param name="extend">扩展实例。</param>
        /// <param name="form">表单集合。</param>
        protected void Merge(ExtendBase extend, IFormCollection form)
        {
            foreach (var key in form.Keys)
            {
                if (key.StartsWith("ex:"))
                {
                    extend[key] = form[key];
                }
            }
        }

        /// <summary>
        /// 获取网站配置。
        /// </summary>
        /// <returns>返回网站配置实例。</returns>
        protected async Task<IActionResult> GetSettingsAsync<TSettings>()
            where TSettings : class, new()
        {
            var settings = await GetRequiredService<ISettingsManager>().GetSettingsAsync<TSettings>();
            return OkResult(settings);
        }

        /// <summary>
        /// 保存网站配置。
        /// </summary>
        /// <param name="settings">配置实例。</param>
        /// <param name="name">配置名称，用于日志保存。</param>
        /// <returns>返回保存结果。</returns>
        protected async Task<IActionResult> SaveSettingsAsync<TSettings>(TSettings settings, string name)
            where TSettings : class, new()
        {
            var result = await GetRequiredService<ISettingsManager>().SaveSettingsAsync(settings);
            if (result)
            {
                Log(Resources.SaveSettings, name);
                return OkResult();
            }
            return BadResult(Resources.SaveSettingsFailured);
        }

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