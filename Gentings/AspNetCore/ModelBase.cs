using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.AspNetCore.StatusMessages;
using Gentings.Extensions;
using Gentings.Extensions.Events;
using Gentings.Properties;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 页面模型基类。
    /// </summary>
    public abstract class ModelBase : PageModel
    {
        #region common
        private Version _version;
        /// <summary>
        /// 当前程序的版本。
        /// </summary>
        public Version Version => _version ??= Cores.Version;

        private ILocalizer _localizer;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        public ILocalizer Localizer => _localizer ??= GetRequiredService<ILocalizer>();

        private ILogger _logger;
        /// <summary>
        /// 日志接口。
        /// </summary>
        public virtual ILogger Logger => _logger ??= GetRequiredService<ILoggerFactory>().CreateLogger(GetType());

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

        private int? _userId;
        /// <summary>
        /// 当前登录用户Id。
        /// </summary>
        public int UserId => _userId ??= User.GetUserId();

        private string _userName;
        /// <summary>
        /// 当前登录用户名称。
        /// </summary>
        public string UserName => _userName ??= User.GetUserName();

        /// <summary>
        /// 是否已经登录。
        /// </summary>
        public virtual bool IsAuthenticated => User.Identity.IsAuthenticated;

        /// <summary>
        /// 获取对象对比实例。
        /// </summary>
        /// <param name="instance">当前对象实例，在更改对象实例之前的实例。</param>
        /// <returns>返回当前实例。</returns>
        protected IObjectDiffer GetObjectDiffer(object instance)
        {
            var differ = new ObjectDiffer(HttpContext);
            differ.Stored(instance);
            return differ;
        }

        /// <summary>
        /// 判断验证码。
        /// </summary>
        /// <param name="key">当前唯一键。</param>
        /// <param name="code">验证码。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsCodeValid(string key, string code)
        {
            if (string.IsNullOrEmpty(code) || !Request.Cookies.TryGetValue(key, out var value))
                return false;
            code = Cores.Hashed(code);
            return string.Equals(value, code, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region pages
        private StatusMessage _statusMessage;

        /// <summary>
        /// 设置状态消息。
        /// </summary>
        /// <param name="message">消息内容。</param>
        protected void StatusMessage(string message) => StatusMessage(StatusType.Success, message);

        /// <summary>
        /// 设置状态消息。
        /// </summary>
        /// <param name="type">状态类型。</param>
        /// <param name="message">消息内容。</param>
        protected void StatusMessage(StatusType type, string message)
        {
            if (_statusMessage == null)
                _statusMessage = new StatusMessage(TempData);
            _statusMessage.Message = message;
            _statusMessage.Type = type;
        }

        /// <summary>
        /// 消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult InfoPage(string message) => Page(StatusType.Info, message);

        /// <summary>
        /// 警告消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult WarningPage(string message) => Page(StatusType.Warning, message);

        /// <summary>
        /// 错误消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult ErrorPage(string message) => Page(StatusType.Danger, message);

        /// <summary>
        /// 成功消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult SuccessPage(string message) => Page(StatusType.Success, message);

        private IActionResult Page(StatusType statusType, string message)
        {
            StatusMessage(statusType, message);
            return Page();
        }

        /// <summary>
        /// 消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToInfoPage(string message, string pageName = null, string pageHandler = null, object routeValues = null)
            => RedirectToPage(StatusType.Info, message, pageName, pageHandler, routeValues);

        /// <summary>
        /// 警告消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToWarningPage(string message, string pageName = null, string pageHandler = null, object routeValues = null)
            => RedirectToPage(StatusType.Warning, message, pageName, pageHandler, routeValues);

        /// <summary>
        /// 错误消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToErrorPage(string message, string pageName = null, string pageHandler = null, object routeValues = null)
            => RedirectToPage(StatusType.Danger, message, pageName, pageHandler, routeValues);

        /// <summary>
        /// 成功消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToSuccessPage(string message, string pageName = null, string pageHandler = null, object routeValues = null)
            => RedirectToPage(StatusType.Success, message, pageName, pageHandler, routeValues);

        /// <summary>
        /// 返回带状态消息页面结果。
        /// </summary>
        /// <param name="statusType">状态类型。</param>
        /// <param name="message">消息。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        private IActionResult RedirectToPage(StatusType statusType, string message, string pageName = null, string pageHandler = null, object routeValues = null)
        {
            StatusMessage(statusType, message);
            if (pageName != null)
                return RedirectToPage(pageName, pageHandler, routeValues);
            return RedirectToPage();
        }

        /// <summary>
        /// 返回带状态消息页面结果。
        /// </summary>
        /// <param name="result">数据结果。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult Page(DataResult result, params object[] args)
        {
            if (result.Succeed())
                return SuccessPage(result.ToString(args));
            return ErrorPage(result.ToString(args));
        }

        /// <summary>
        /// 返回带状态消息页面结果。
        /// </summary>
        /// <param name="result">数据结果。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToResult(DataResult result, params object[] args)
        {
            if (result.Succeed())
                return RedirectToSuccessPage(result.ToString(args));
            return RedirectToErrorPage(result.ToString(args));
        }
        #endregion

        #region jsons
        /// <summary>
        /// 返回消息的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Info(string message = null)
        {
            return Json(StatusType.Info, message);
        }

        /// <summary>
        /// 返回消息的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="data">数据实例对象。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Info<TData>(string message, TData data)
        {
            return Json(StatusType.Info, message, data);
        }

        /// <summary>
        /// 返回警告的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Warning(string message = null)
        {
            return Json(StatusType.Warning, message);
        }

        /// <summary>
        /// 返回警告的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="data">数据实例对象。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Warning<TData>(string message, TData data)
        {
            return Json(StatusType.Warning, message, data);
        }

        /// <summary>
        /// 返回错误的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Error(string message = null)
        {
            return Json(StatusType.Danger, message);
        }

        /// <summary>
        /// 返回错误的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="data">数据实例对象。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Error<TData>(string message, TData data)
        {
            return Json(StatusType.Danger, message, data);
        }

        /// <summary>
        /// 返回成功的JSON对象。
        /// </summary>
        /// <param name="affected">是否有执行。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Success(bool affected = true)
        {
            return Success(new { affected });
        }

        /// <summary>
        /// 返回成功的JSON对象。
        /// </summary>
        /// <param name="data">客户数据对象。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Success<TData>(TData data)
        {
            return Json(StatusType.Success, null, data);
        }

        /// <summary>
        /// 返回成功的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Success(string message)
        {
            return Json(StatusType.Success, message);
        }

        /// <summary>
        /// 返回成功的JSON对象。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="data">数据实例对象。</param>
        /// <returns>返回JSON结果。</returns>
        protected IActionResult Success<TData>(string message, TData data)
        {
            return Json(StatusType.Success, message, data);
        }

        private IActionResult Json(StatusType type, string message)
        {
            if (type != StatusType.Success && !ModelState.IsValid)
            {
                var dic = new Dictionary<string, string>();
                foreach (var key in ModelState.Keys)
                {
                    var error = ModelState[key].Errors.FirstOrDefault()?.ErrorMessage;
                    if (string.IsNullOrEmpty(key))
                        message ??= error;
                    else
                        dic[key] = error;
                }
                return Json(type, message, dic);
            }
            return Json(new JsonStatusMesssage(type, message));
        }

        private IActionResult Json<TData>(StatusType type, string message, TData data)
        {
            return Json(new JsonStatusMesssage<TData>(type, message, data));
        }

        /// <summary>
        /// 返回JSON对象。
        /// </summary>
        /// <param name="data">对象实例。</param>
        /// <returns>返回JSON试图结果。</returns>
        protected IActionResult Json(object data)
        {
            return new JsonResult(data);
        }

        /// <summary>
        /// 返回JSON试图结果。
        /// </summary>
        /// <param name="result">数据结果。</param>
        /// <param name="name">操作对象名称。</param>
        /// <returns>返回JSON试图结果。</returns>
        protected IActionResult Json(DataResult result, string name)
        {
            if (result.Succeed())
                return Success(result.ToString(name));
            return Error(result.ToString(name));
        }

        /// <summary>
        /// 返回JSON试图结果。
        /// </summary>
        /// <param name="result">数据结果。</param>
        /// <param name="action">操作成功的方法。</param>
        /// <param name="name">操作对象名称。</param>
        /// <returns>返回JSON试图结果。</returns>
        protected IActionResult Json(bool result, DataAction action, string name)
        {
            if (result)
                return Success(Localizer.GetString(action, name));
            action = (DataAction)(-(int)action);
            return Error(Localizer.GetString(action, name));
        }
        #endregion

        #region events
        private IEventLogger _eventLogger;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        protected IEventLogger Events => _eventLogger ??= GetRequiredService<IEventLogger>();

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        protected void Log(string message, EventLevel level = EventLevel.Success, string source = null)
            => Events.Log(message, EventType, level, source);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        protected Task LogAsync(string message, EventLevel level = EventLevel.Success, string source = null)
            => Events.LogAsync(message, EventType, level, source);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected void Log(string message, params object[] args)
            => Events.Log(string.Format(message, args));

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected Task LogAsync(string message, params object[] args)
            => Events.LogAsync(string.Format(message, args));

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        protected void Log(Exception exception)
            => Events.Log(exception, EventType);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        protected Task LogAsync(Exception exception)
            => Events.LogAsync(exception, EventType);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="source">来源。</param>
        protected void LogResult(DataResult result, string name, string source = null)
            => Events.LogResult(result, name, EventType, source);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="source">来源。</param>
        protected Task LogResultAsync(DataResult result, string name, string source = null)
            => Events.LogResultAsync(result, name, EventType, source);

        /// <summary>
        /// 事件类型。
        /// </summary>
        protected virtual string EventType => Resources.EventType;
        #endregion
    }
}
