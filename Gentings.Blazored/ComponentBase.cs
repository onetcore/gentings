using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Gentings.Blazored
{
    /// <summary>
    /// 组件基类。
    /// </summary>
    public abstract class ComponentBase : Microsoft.AspNetCore.Components.ComponentBase
    {
        /// <summary>
        /// JS运行实例。
        /// </summary>
        [Inject]
        protected JSRuntime JSRuntime { get; set; }

        /// <summary>
        /// 子代码。
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// 样式名称。
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// 其他自定义属性。
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public IDictionary<string, object> AdditionalAttributes { get; set; }

        [Inject]
        private IServiceProvider ServiceProvider { get; set; }

        private ILocalizer _localizer;
        /// <summary>
        /// 当前本地化实例对象。
        /// </summary>
        protected ILocalizer Localizer => _localizer ??= new Localizer(GetType(), ServiceProvider);
    }
}