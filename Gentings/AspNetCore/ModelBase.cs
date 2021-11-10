﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gentings.Extensions;
using Gentings.Extensions.Events;
using Gentings.Localization;
using Gentings.Properties;
using Microsoft.AspNetCore.Authentication;
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
        /// 写入用户登录。
        /// </summary>
        /// <param name="user">当前用户实例对象。</param>
        /// <param name="authenticationScheme">验证方式。</param>
        /// <param name="defaultRedirectUri">后台首页默认URL地址。</param>
        /// <param name="isRemembered">是否持久化登录状态。</param>
        /// <returns>登录任务。</returns>
        protected async Task SignInAsync(IUser user, string authenticationScheme, string defaultRedirectUri = "/backend", bool isRemembered = false)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, authenticationScheme);
            string returnUrl = Request.Query["ReturnUrl"];
            var properties = new AuthenticationProperties
            {
                IsPersistent = isRemembered,//是否持久化
                RedirectUri = string.IsNullOrWhiteSpace(returnUrl) ? defaultRedirectUri : returnUrl,//如果用户点“登录“进来，登录成功后跳转到首页，否则跳转到上一个页面
                ExpiresUtc = DateTime.UtcNow.AddDays(1) //设置 cookie 过期时间：一天后过期
            };
            
            //生成一个加密的 cookie 并输出到浏览器。
            await HttpContext.SignInAsync(authenticationScheme, new ClaimsPrincipal(identity), properties);
        }

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
        /// <param name="code">错误代码。</param>
        /// <param name="message">消息内容。</param>
        private void StatusMessage(int code, string message)
        {
            if (_statusMessage == null)
                _statusMessage = new StatusMessage(TempData);
            _statusMessage.Message = message;
            _statusMessage.Code = code;
        }

        /// <summary>
        /// 错误消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult ErrorPage(string message) => Page(-1, message);

        /// <summary>
        /// 成功消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult SuccessPage(string message) => Page(0, message);

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
        /// 返回当前页面结果。
        /// </summary>
        /// <param name="code">错误代码。</param>
        /// <param name="message">消息。</param>
        /// <returns>返回当前页面结果实例。</returns>
        private IActionResult Page(int code, string message)
        {
            StatusMessage(code, message);
            return Page();
        }

        /// <summary>
        /// 错误消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToErrorPage(string message, string pageName = null, string pageHandler = null, object routeValues = null)
            => RedirectToPage(-1, message, pageName, pageHandler, routeValues);

        /// <summary>
        /// 成功消息页面。
        /// </summary>
        /// <param name="message">消息字符串。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToSuccessPage(string message, string pageName = null, string pageHandler = null, object routeValues = null)
            => RedirectToPage(0, message, pageName, pageHandler, routeValues);

        /// <summary>
        /// 返回带状态消息页面结果。
        /// </summary>
        /// <param name="code">错误代码。</param>
        /// <param name="message">消息。</param>
        /// <param name="pageName">页面名称。</param>
        /// <param name="pageHandler">页面处理方法。</param>
        /// <param name="routeValues">参数匿名对象。</param>
        /// <returns>返回当前页面结果。</returns>
        private IActionResult RedirectToPage(int code, string message, string pageName = null, string pageHandler = null, object routeValues = null)
        {
            StatusMessage(code, message);
            return RedirectToPage(pageName, pageHandler, routeValues);
        }

        /// <summary>
        /// 返回带状态消息页面结果。
        /// </summary>
        /// <param name="result">数据结果。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回当前页面结果。</returns>
        protected IActionResult RedirectToPage(DataResult result, params object[] args)
        {
            if (result.Succeed())
                return RedirectToSuccessPage(result.ToString(args));
            return RedirectToErrorPage(result.ToString(args));
        }
        #endregion

        #region json
        /// <summary>
        /// 参数错误。
        /// </summary>
        /// <param name="parameterNames">参数名称列表。</param>
        /// <returns>返回错误结果。</returns>
        protected virtual IActionResult Invalid(params string[] parameterNames)
        {
            return Error(ErrorCode.InvalidParameters, args: parameterNames.Join());
        }

        /// <summary>
        /// 返回验证失败结果。
        /// </summary>
        /// <returns>验证失败结果。</returns>
        protected virtual IActionResult Error()
        {
            var dic = new Dictionary<string, string>();
            var result = new ApiDataResult<Dictionary<string, string>>(dic) { Code = (int)ErrorCode.ValidError, Message = Localizer[ErrorCode.ValidError] };
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
            return Success(result);
        }

        /// <summary>
        /// 返回错误结果。
        /// </summary>
        /// <param name="message">错误消息。</param>
        /// <param name="args">错误消息参数。</param>
        /// <returns>返回JSON结果。</returns>
        protected virtual IActionResult Error(string message, params object[] args)
        {
            if (args != null) message = string.Format(message, args);
            return Ok(new ApiResult { Code = (int)ErrorCode.UnknownError, Message = message });
        }

        /// <summary>
        /// 返回错误结果。
        /// </summary>
        /// <param name="code">错误编码。</param>
        /// <param name="args">错误消息参数。</param>
        /// <returns>返回JSON结果。</returns>
        protected virtual IActionResult Error(Enum code, params object[] args)
        {
            var resource = Localizer.GetString(code, args);
            return Ok(new ApiResult { Code = (int)(object)code, Message = resource });
        }

        /// <summary>
        /// 返回数据结果。
        /// </summary>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult Success()
        {
            return Ok(ApiResult.Success);
        }

        /// <summary>
        /// 返回数据结果。
        /// </summary>
        /// <param name="result">API执行结果。</param>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult Success(ApiResult result)
        {
            return Ok(result);
        }

        /// <summary>
        /// 返回数据结果。
        /// </summary>
        /// <param name="data">数据列表。</param>
        /// <param name="message">消息。</param>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult Success(object data, string message = null)
        {
            var instance = Activator.CreateInstance(typeof(ApiDataResult<>).MakeGenericType(data.GetType()), data) as ApiResult;
            instance.Message = message;
            return Success(instance);
        }

        /// <summary>
        /// 返回分页数据结果。
        /// </summary>
        /// <typeparam name="TPageData">查询类型。</typeparam>
        /// <param name="query">数据列表。</param>
        /// <param name="message">消息。</param>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult Success<TPageData>(TPageData query, string message = null)
            where TPageData : IPageEnumerable
        {
            return Success(new ApiPageResult<TPageData>(query) { Message = message });
        }

        /// <summary>
        /// 返回数据操作结果，如果成功则返回成功实例。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="action">操作方法。</param>
        /// <param name="name">名称。</param>
        /// <param name="logged">如果操作成功，是否保存到日志记录中。</param>
        /// <returns>返回数据操作结果。</returns>
        protected virtual IActionResult Success(bool result, DataAction action, string name, bool logged = false)
        {
            if (!result) action = (DataAction)(-(int)action);
            var resource = Localizer.GetString(action, name);
            var api = new ApiResult
            {
                Message = resource,
                Code = result ? 0 : (int)action
            };
            if (result && logged) Log(api.Message);
            return Ok(api);
        }

        /// <summary>
        /// 返回数据操作结果，如果成功则返回成功实例。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="logged">如果操作成功，是否保存到日志记录中。</param>
        /// <returns>返回数据操作结果。</returns>
        protected virtual IActionResult Success(DataResult result, string name, bool logged = false)
        {
            var api = new ApiResult { Message = result.ToString(name), Code = result.Succeed() ? 0 : result.Code };
            if (result.Succeed()) Log(api.Message);
            return Ok(api);
        }

        /// <summary>
        /// 返回数据操作结果，如果成功则返回成功实例，并且保存到日志中。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="action">操作方法。</param>
        /// <param name="name">名称。</param>
        /// <returns>返回数据操作结果。</returns>
        protected virtual async Task<IActionResult> SuccessAsync(bool result, DataAction action, string name)
        {
            if (!result) action = (DataAction)(-(int)action);
            var resource = Localizer.GetString(action, name);
            var api = new ApiResult
            {
                Message = resource,
                Code = result ? 0 : (int)action
            };
            if (result) await LogAsync(api.Message);
            return Ok(api);
        }

        /// <summary>
        /// 返回数据操作结果，如果成功则返回成功实例，并且保存到日志中。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <returns>返回数据操作结果。</returns>
        protected virtual async Task<IActionResult> SuccessAsync(DataResult result, string name)
        {
            var api = new ApiResult { Message = result.ToString(name), Code = result.Succeed() ? 0 : result.Code };
            if (result.Succeed()) await LogAsync(api.Message);
            return Ok(api);
        }

        /// <summary>
        /// 返回对象的JSON结果对象。
        /// </summary>
        /// <param name="data">返回的数据结果。</param>
        /// <returns>返回对象的JSON结果对象。</returns>
        protected IActionResult Ok(object data)
        {
            return new JsonResult(data);
        }
        #endregion

        #region events
        private IEventLogger _eventLogger;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        protected IEventLogger Events => _eventLogger ??= GetService<IEventLogger>();

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        protected void Log(string message, EventLevel level = EventLevel.Success, string source = null)
            => Events?.Log(message, EventType, level, source);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">消息。</param>
        /// <param name="level">事件等级。</param>
        /// <param name="source">来源。</param>
        protected async Task LogAsync(string message, EventLevel level = EventLevel.Success, string source = null)
        {
            if (Events != null)
                await Events.LogAsync(message, EventType, level, source);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected void Log(string message, params object[] args)
            => Events?.Log(string.Format(message, args));

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="message">事件消息。</param>
        /// <param name="args">格式化参数。</param>
        protected async Task LogAsync(string message, params object[] args)
        {
            if (Events != null)
                await Events.LogAsync(string.Format(message, args));
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        protected void Log(Exception exception)
            => Events?.Log(exception, EventType);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="exception">错误实例对象。</param>
        protected async Task LogAsync(Exception exception)
        {
            if (Events != null)
                await Events.LogAsync(exception, EventType);
        }

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="source">来源。</param>
        protected void LogResult(DataResult result, string name, string source = null)
            => Events?.LogResult(result, name, EventType, source);

        /// <summary>
        /// 添加事件日志。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="source">来源。</param>
        protected async Task LogResultAsync(DataResult result, string name, string source = null)
        {
            if (Events != null)
                await Events.LogResultAsync(result, name, EventType, source);
        }

        /// <summary>
        /// 事件类型。
        /// </summary>
        protected virtual string EventType => Resources.EventType;
        #endregion
    }
}
