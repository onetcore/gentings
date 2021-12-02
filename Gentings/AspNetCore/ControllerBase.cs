using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Gentings.Extensions;
using Gentings.Extensions.Events;
using Gentings.Extensions.Settings;
using Gentings.Localization;
using Gentings.Properties;
using Gentings.Security;
using Gentings.Storages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        #region common
        private Version _version;
        /// <summary>
        /// 当前程序的版本。
        /// </summary>
        protected Version Version => _version ??= Cores.Version;

        private ILocalizer _localizer;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        protected ILocalizer Localizer => _localizer ??= GetRequiredService<ILocalizerFactory>().CreateLocalizer(GetType());

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

        private INamedStringManager _namedStringManager;
        /// <summary>
        /// 获取或者添加名称字符串。
        /// </summary>
        /// <param name="key">唯一键。</param>
        /// <returns>返回当前唯一键的值。</returns>
        protected string GetOrAddNamedString(string key)
        {
            if (_namedStringManager == null)
                _namedStringManager = GetService<INamedStringManager>();
            return _namedStringManager?.GetOrAddString(key);
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
            var result = new ApiDataResult<Dictionary<string, string>>(dic) { Code = (int)ErrorCode.ValidError };
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
            return Json(new ApiResult { Code = (int)ErrorCode.UnknownError, Message = message });
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
            return Json(new ApiResult { Code = (int)(object)code, Message = resource });
        }

        /// <summary>
        /// 返回数据结果。
        /// </summary>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult Success(string message = null)
        {
            var result = message == null ? ApiResult.Success : new ApiResult { Message = message };
            return Json(result);
        }

        /// <summary>
        /// 返回数据结果。
        /// </summary>
        /// <param name="result">API执行结果。</param>
        /// <returns>返回包含数据的结果。</returns>
        protected virtual IActionResult Success(ApiResult result)
        {
            return Json(result);
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
        protected virtual IActionResult Json(bool result, DataAction action, string name, bool logged = false)
        {
            if (!result) action = (DataAction)(-(int)action);
            var resource = Localizer.GetString(action, name);
            var api = new ApiResult
            {
                Message = resource,
                Code = result ? 0 : (int)action
            };
            if (result && logged) Log(api.Message);
            return Json(api);
        }

        /// <summary>
        /// 返回数据操作结果，如果成功则返回成功实例。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <param name="logged">如果操作成功，是否保存到日志记录中。</param>
        /// <returns>返回数据操作结果。</returns>
        protected virtual IActionResult Json(DataResult result, string name, bool logged = false)
        {
            var api = new ApiResult { Message = result.ToString(name), Code = result ? 0 : result.Code };
            if (result) Log(api.Message);
            return Json(api);
        }

        /// <summary>
        /// 返回数据操作结果，如果成功则返回成功实例，并且保存到日志中。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="action">操作方法。</param>
        /// <param name="name">名称。</param>
        /// <returns>返回数据操作结果。</returns>
        protected virtual async Task<IActionResult> JsonAsync(bool result, DataAction action, string name)
        {
            if (!result) action = (DataAction)(-(int)action);
            var resource = Localizer.GetString(action, name);
            var api = new ApiResult
            {
                Message = resource,
                Code = result ? 0 : (int)action
            };
            if (result) await LogAsync(api.Message);
            return Json(api);
        }

        /// <summary>
        /// 返回数据操作结果，如果成功则返回成功实例，并且保存到日志中。
        /// </summary>
        /// <param name="result">数据操作结果。</param>
        /// <param name="name">名称。</param>
        /// <returns>返回数据操作结果。</returns>
        protected virtual async Task<IActionResult> JsonAsync(DataResult result, string name)
        {
            var api = new ApiResult { Message = result.ToString(name), Code = result ? 0 : result.Code };
            if (result) await LogAsync(api.Message);
            return Json(api);
        }

        /// <summary>
        /// 返回上传文件结果，如果成功返回URL地址。
        /// </summary>
        /// <param name="file">上传后的文件实例。</param>
        /// <returns>返回上传文件结果。</returns>
        protected virtual IActionResult Json(IStorageFile file)
        {
            if (file != null) return Json(new { file.Url });
            return Error(Resources.UploadFileFailured);
        }

        /// <summary>
        /// 返回JSON数据。
        /// </summary>
        /// <param name="data">返回的数据对象。</param>
        /// <returns>返回JSON数据结果。</returns>
        protected virtual IActionResult Json(object data)
        {
            return Ok(data);
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

        #region users
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
        protected virtual bool IsAuthenticated => User.Identity.IsAuthenticated;

        /// <summary>
        /// 创建JWT访问Token。
        /// </summary>
        /// <param name="claims">用户声明列表。</param>
        /// <returns>返回Token字符串。</returns>
        protected string CreateJwtSecurityToken(IEnumerable<Claim> claims)
        {
            var configuration = GetRequiredService<IConfiguration>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecurityKey"] ?? "This'sJWTSecurityKeyPleaseConfigInFile!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToInt32(configuration["Jwt:Expires"] ?? "1440"));

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"] ?? "https://localhost/",
                configuration["Jwt:Audience"] ?? "https://localhost/",
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 判断验证码。
        /// </summary>
        /// <param name="key">当前唯一键。</param>
        /// <param name="code">验证码。</param>
        /// <returns>返回判断结果。</returns>
        protected bool IsCodeValid(string key, string code)
        {
            if (string.IsNullOrEmpty(code) || !Request.Cookies.TryGetValue(key, out var value))
                return false;
            code = Cores.Hashed(code);
            return string.Equals(value, code, StringComparison.OrdinalIgnoreCase);
        }

        private IUserService _userManager;
        /// <summary>
        /// 用户管理接口实例。
        /// </summary>
        private IUserService UserManager => _userManager ??= GetService<IUserService>();

        /// <summary>
        /// 获取当前缓存用户实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例。</returns>
        protected IUser GetUser(int userId) => UserManager?.GetCachedUser(userId);

        /// <summary>
        /// 获取当前缓存用户实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <returns>返回用户实例。</returns>
        protected Task<IUser> GetUserAsync(int userId) => UserManager?.GetCachedUserAsync(userId);
        #endregion
    }
}