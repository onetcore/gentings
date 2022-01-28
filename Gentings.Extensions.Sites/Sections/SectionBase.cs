using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.Sections
{
    /// <summary>
    /// HTML代码节点基类。
    /// </summary>
    public abstract class SectionBase : ISection
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        public virtual int Priority { get; }

        /// <summary>
        /// 类型名称。
        /// </summary>
        public virtual string Name => GetType().Name;

        /// <summary>
        /// 显示名称。
        /// </summary>
        public virtual string DisplayName => Name;

        /// <summary>
        /// 图标地址。
        /// </summary>
        public abstract string IconUrl { get; }

        /// <summary>
        /// 配置地址。
        /// </summary>
        public abstract string EditUrl { get; }

        /// <summary>
        /// 描述。
        /// </summary>
        public virtual string? Summary { get; }

        /// <summary>
        /// 样式。
        /// </summary>
        public virtual string? Style { get; }

        /// <summary>
        /// 脚本。
        /// </summary>
        public virtual string? Script { get; }

        /// <summary>
        /// 初始代码。
        /// </summary>
        public virtual string? Html { get; }

        /// <summary>
        /// 呈现节点实例。
        /// </summary>
        /// <param name="context">节点上下文。</param>
        /// <param name="output">输出实例对象。</param>
        /// <returns>当前节点呈现任务。</returns>
        public virtual async Task ProcessAsync(SectionContext context, TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(context.Section.Html))
            {
                var content = await output.GetChildContentAsync();
                context.AppendHtml(content);
                return;
            }
            context.AppendHtml(context.Section.Html);
        }
    }
}