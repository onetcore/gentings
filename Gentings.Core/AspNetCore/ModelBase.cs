using System;
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
    }
}
