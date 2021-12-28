using Gentings.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gentings.AspNetCore.TagHelpers
{
    /// <summary>
    /// 可访问<see cref="Microsoft.AspNetCore.Mvc.Rendering.ViewContext"/>实例的标记。
    /// </summary>
    public abstract class ViewContextableTagHelperBase : TagHelperBase
    {
        /// <summary>
        /// 试图上下文。
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public ViewContext ViewContext { get; set; }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

        /// <summary>
        /// HTTP上下文实例。
        /// </summary>
        protected HttpContext HttpContext => ViewContext.HttpContext;

        /// <summary>
        /// 获取当前文档的计数器。
        /// </summary>
        /// <returns>返回计数器值。</returns>
        protected int GetCounter()
        {
            var current = (HttpContext.Items["taghelper:counter"] as int?) ?? 0;
            current++;
            HttpContext.Items["taghelper:counter"] = current;
            return current;
        }

        private ILogger? _logger;
        /// <summary>
        /// 日志接口。
        /// </summary>
        protected virtual ILogger Logger =>
            _logger ??= GetRequiredService<ILoggerFactory>()
                .CreateLogger(GetType());

        /// <summary>
        /// 获取注册的服务对象。
        /// </summary>
        /// <typeparam name="TService">服务类型或者接口。</typeparam>
        /// <returns>返回当前服务的实例对象。</returns>
        protected TService? GetService<TService>()
        {
            return HttpContext.RequestServices.GetService<TService>();
        }

        /// <summary>
        /// 获取已经注册的服务对象。
        /// </summary>
        /// <typeparam name="TService">服务类型或者接口。</typeparam>
        /// <returns>返回当前服务的实例对象。</returns>
        protected TService GetRequiredService<TService>() where TService : notnull
        {
            return HttpContext.RequestServices.GetRequiredService<TService>();
        }

        private ILocalizer? _localizer;
        /// <summary>
        /// 本地化接口。
        /// </summary>
        public ILocalizer Localizer
        {
            get
            {
                if (_localizer == null)
                {
                    var factory = GetRequiredService<ILocalizerFactory>();
                    if (ViewContext.ActionDescriptor is CompiledPageActionDescriptor cpage)
                        _localizer = factory.CreateLocalizer(cpage.ModelTypeInfo);
                    else if (ViewContext.ActionDescriptor is ControllerActionDescriptor controller)
                        _localizer = factory.CreateLocalizer(controller.ControllerTypeInfo);
                    else
                        _localizer = factory.CreateLocalizer(GetType());
                }
                return _localizer;
            }
        }
    }
}