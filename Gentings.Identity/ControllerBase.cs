using System.Threading.Tasks;
using Gentings.Extensions;
using Gentings.Extensions.Settings;
using Gentings.Identity.Events;
using Gentings.Identity.Properties;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Identity
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    public abstract class ControllerBase : AspNetCore.ControllerBase
    {
        private int? _userId;
        /// <summary>
        /// 当前登录用户Id。
        /// </summary>
        protected int UserId => _userId ?? (_userId = HttpContext.User.GetUserId()).Value;

        private string _userName;
        /// <summary>
        /// 当前登录用户Id。
        /// </summary>
        protected string UserName => _userName ??= HttpContext.User.GetUserName();

        private IObjectDiffer _differ;
        /// <summary>
        /// 对象属性变更实例。
        /// </summary>
        protected IObjectDiffer Differ => _differ ??= GetRequiredService<IObjectDiffer>();

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
        protected virtual string EventType => Resources.EventType_User;

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
    }
}