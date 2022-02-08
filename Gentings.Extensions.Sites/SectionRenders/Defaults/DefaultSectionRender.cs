using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gentings.Extensions.Sites.SectionRenders.Defaults
{
    /// <summary>
    /// 默认节点。
    /// </summary>
    public class DefaultSectionRender : SectionRenderBase
    {
        /// <summary>
        /// 默认名称。
        /// </summary>
        public const string Default = "Default";

        /// <summary>
        /// 类型名称。
        /// </summary>
        public override string Name => Default;

        /// <summary>
        /// 显示名称。
        /// </summary>
        public override string DisplayName => "默认节点";

        /// <summary>
        /// 图标地址。
        /// </summary>
        public override string IconUrl => "bi-card-text";

        /// <summary>
        /// 优先级。
        /// </summary>
        public override int Priority => int.MaxValue;

        /// <summary>
        /// 描述。
        /// </summary>
        public override string Summary => "默认节点实例，提供节点默认设置。";

        /// <summary>
        /// 呈现节点实例。
        /// </summary>
        /// <param name="context">节点上下文。</param>
        /// <param name="output">输出实例对象。</param>
        /// <returns>当前节点呈现任务。</returns>
        public override Task ProcessAsync(SectionContext context, TagBuilder output)
        {
            if (context.Section.RenderName == Name)
            {
                var source = context.Section.As<DefaultSection>();
                if (source?.Html != null)
                    output.InnerHtml.AppendHtml(source.Html);
            }
            return Task.CompletedTask;
        }
    }
}