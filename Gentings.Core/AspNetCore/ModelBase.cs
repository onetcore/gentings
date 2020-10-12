using System;
using System.Collections.Generic;
using System.Linq;
using Gentings.AspNetCore.StatusMessages;
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
        public bool IsAuthenticated => User.Identity.IsAuthenticated;


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
        /// <param name="pageOrUrl">页面或者URL地址。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="area">区域。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToInfoPage(string message, string pageOrUrl = null, string pageHandler = null, string area = null)
            => RedirectToPage(StatusType.Info, message, pageOrUrl, pageHandler, area);

        /// <summary>
        /// 警告消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageOrUrl">页面或者URL地址。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="area">区域。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToWarningPage(string message, string pageOrUrl = null, string pageHandler = null, string area = null)
            => RedirectToPage(StatusType.Warning, message, pageOrUrl, pageHandler, area);

        /// <summary>
        /// 错误消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageOrUrl">页面或者URL地址。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="area">区域。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToErrorPage(string message, string pageOrUrl = null, string pageHandler = null, string area = null)
            => RedirectToPage(StatusType.Danger, message, pageOrUrl, pageHandler, area);

        /// <summary>
        /// 成功消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageOrUrl">页面或者URL地址。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="area">区域。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToSuccessPage(string message, string pageOrUrl = null, string pageHandler = null, string area = null)
            => RedirectToPage(StatusType.Success, message, pageOrUrl, pageHandler, area);

        /// <summary>
        /// 返回带状态消息页面结果。
        /// </summary>
        /// <param name="statusType">状态类型。</param>
        /// <param name="message">消息。</param>
        /// <param name="pageOrUrl">页面或者URL地址。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="area">区域。</param>
        /// <returns>返回当前页面结果。</returns>
        private IActionResult RedirectToPage(StatusType statusType, string message, string pageOrUrl = null, string pageHandler = null, string area = null)
        {
            StatusMessage(statusType, message);
            if (pageOrUrl != null)
                return RedirectToPage(pageOrUrl, pageHandler, new { area });
            return RedirectToPage();
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
        #endregion
    }
}
