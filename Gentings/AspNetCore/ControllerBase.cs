using System;
using System.Collections.Generic;
using System.Linq;
using Gentings.Extensions;
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
                    result.Message = error;
                else
                    dic[key] = error;
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
                message = Localizer.GetString(code);
            if (args != null)
                message = string.Format(message, args);
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
    }
}