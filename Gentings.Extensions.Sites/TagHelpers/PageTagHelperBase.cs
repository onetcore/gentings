using Gentings.AspNetCore.TagHelpers;
using Gentings.Extensions.Sites.Templates;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.TagHelpers
{
    /// <summary>
    /// 页面标签基类。
    /// </summary>
    public abstract class PageTagHelperBase : ViewContextableTagHelperBase
    {
        /// <summary>
        /// 当前页面模型上下文。
        /// </summary>
        protected PageContext? Context { get; private set; }

        /// <summary>
        /// 初始化当前标签上下文。
        /// </summary>
        /// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
        public override void Init(TagHelperContext context)
        {
            if (ViewContext.ViewData.Model is TemplateModelBase model)
                Context = model.Context;
        }
    }
}
