using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gentings.Extensions.Sites.Sections
{
    /// <summary>
    /// Html节点呈现接口。
    /// </summary>
    public interface ISectionRender : IServices
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 类型名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 显示名称。
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 图标地址。
        /// </summary>
        string IconUrl { get; }

        /// <summary>
        /// 更新地址。
        /// </summary>
        string EditUrl { get; }

        /// <summary>
        /// 描述。
        /// </summary>
        string? Summary { get; }

        /// <summary>
        /// 初始样式。
        /// </summary>
        string? Style { get; }

        /// <summary>
        /// 初始脚本。
        /// </summary>
        string? Script { get; }

        /// <summary>
        /// 初始代码。
        /// </summary>
        string? Html { get; }

        /// <summary>
        /// 呈现节点实例。
        /// </summary>
        /// <param name="context">节点上下文。</param>
        /// <param name="output">输出实例对象。</param>
        /// <returns>当前节点呈现任务。</returns>
        Task ProcessAsync(SectionContext context, TagBuilder output);
    }
}