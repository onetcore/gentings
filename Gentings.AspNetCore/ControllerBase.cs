using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.AspNetCore.EventLogging;
using Gentings.AspNetCore.Properties;
using Gentings.Extensions;
using Gentings.Extensions.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        private Version _version;
        /// <summary>
        /// 当前程序的版本。
        /// </summary>
        protected Version Version => _version ??= Cores.Version;

        private ILocalizer _localizer;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        protected ILocalizer Localizer => _localizer ??= GetRequiredService<ILocalizer>();

        private ILogger _logger;
        /// <summary>
        /// 日志接口。
        /// </summary>
        protected virtual ILogger Logger => _logger ??= GetRequiredService<ILoggerFactory>().CreateLogger(GetType());

        /// <summary>
        /// 获取注册的服务对象。
        /// </summary>
        /// <typeparam name="TService">服务类型或者接口。</typeparam>
        /// <returns>返回当前服务的实例对象。</returns>
        protected TService GetService<TService>() => HttpContext.RequestServices.GetService<TService>();

        /// <summary>
        /// 获取已经注册的服务对象。
        /// </summary>
        /// <typeparam name="TService">服务类型或者接口。</typeparam>
        /// <returns>返回当前服务的实例对象。</returns>
        protected TService GetRequiredService<TService>() => HttpContext.RequestServices.GetRequiredService<TService>();

        /// <summary>
        /// 参数错误。
        /// </summary>
        /// <param name="parameterName">参数名称。</param>
        /// <returns>返回错误结果。</returns>
        protected virtual IActionResult BadParameter(string parameterName)
        {
            return BadResult(ErrorCode.InvalidParameters, args: parameterName);
        }

        /// <summary>
        /// 参数错误。
        /// </summary>
        /// <param name="parameterNames">参数名称列表。</param>
        /// <returns>返回错误结果。</returns>
        protected virtual IActionResult BadParameters(params string[] parameterNames)
        {
            return BadParameter(string.Join(", ", parameterNames));
        }

        /// <summary>
        /// 返回验证失败结果。
        /// </summary>
        /// <returns>验证失败结果。</returns>
        protected virtual IActionResult BadResult()
        {
            var dic = new Dictionary<string, string>();
            var result = new ApiDataResult<Dictionary<string, string>>(dic) { Code = ErrorCode.ValidError };
            foreach (var key in ModelState.Keys)
            {
                var error = ModelState[key].Errors.FirstOrDefault()?.ErrorMessage;
                if (string.IsNullOrEmpty(key))
                {
                    result.Message = error;
                }
                else
                {
                    dic[key] = error;
                }
            }
            return OkResult(result);
        }

        /// <summary>
        /// 返回错误结果。
        /// </summary>
        /// <param name="message">错误消息。</param>
        /// <param name="args">错误消息参数。</param>
        /// <returns>返回JSON结果。</returns>
        protected virtual IActionResult BadResult(string message, params object[] args) =>
            BadResult(ErrorCode.UnknownError, message, args);

        /// <summary>
        /// 返回错误结果。
        /// </summary>
        /// <param name="code">错误编码。</param>
        /// <param name="message">错误消息。</param>
        /// <param name="args">错误消息参数。</param>
        /// <returns>返回JSON结果。</returns>
        protected virtual IActionResult BadResult(Enum code, string message = null, params object[] args)
        {
            if (message == null)
            {
                message = Localizer.GetString(code);
            }

            if (args != null)
            {
                message = string.Format(message, args);
            }

            return OkResult(new ApiResult { Code = code, Message = message });
        }

        /// <summary>
        /// 返回数据结果。
        /// </summary>
        /// <param name="result">API执行结果。</param>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult OkResult(ApiResult result = null)
        {
            return Ok(result ?? ApiResult.Success);
        }

        /// <summary>
        /// 返回数据结果。
        /// </summary>
        /// <param name="data">数据列表。</param>
        /// <param name="message">消息。</param>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult OkResult(object data, string message = null)
        {
            var instance = Activator.CreateInstance(typeof(ApiDataResult<>).MakeGenericType(data.GetType()), data) as ApiResult;
            instance.Message = message;
            return OkResult(instance);
        }

        /// <summary>
        /// 返回分页数据结果。
        /// </summary>
        /// <typeparam name="TPageData">查询类型。</typeparam>
        /// <param name="query">数据列表。</param>
        /// <param name="message">消息。</param>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult OkResult<TPageData>(TPageData query, string message = null)
            where TPageData : IPageEnumerable
        {
            return OkResult(new ApiPageResult<TPageData>(query) { Message = message });
        }

        /// <summary>
        /// 获取枚举名称列表。
        /// </summary>
        /// <typeparam name="T">枚举类型。</typeparam>
        /// <returns>返回枚举名称列表。</returns>
        protected IDictionary<int, string> GetNames<T>() where T : Enum
        {
            var dic = new Dictionary<int, string>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                dic[(int)(object)value] = value.ToString();
            }

            return dic;
        }

        /// <summary>
        /// 获取枚举名称资源列表。
        /// </summary>
        /// <typeparam name="T">枚举类型。</typeparam>
        /// <returns>返回枚举名称资源列表。</returns>
        protected IDictionary<int, string> GetDisplayNames<T>() where T : Enum
        {
            var dic = new Dictionary<int, string>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                dic[(int)(object)value] = Localizer.GetString(value);
            }

            return dic;
        }

        private int? _userId;
        /// <summary>
        /// 当前登录用户Id。
        /// </summary>
        protected int UserId => _userId ??= User.GetUserId();

        private string _userName;
        /// <summary>
        /// 当前登录用户名称。
        /// </summary>
        protected string UserName => _userName ??= User.GetUserName();

        /// <summary>
        /// 是否已经登录。
        /// </summary>
        protected bool IsAuthenticated => User.Identity.IsAuthenticated;

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