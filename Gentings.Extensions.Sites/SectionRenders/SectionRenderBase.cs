using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gentings.Extensions.Sites.SectionRenders
{
    /// <summary>
    /// HTML代码节点基类。
    /// </summary>
    public abstract class SectionRenderBase : ISectionRender
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        public virtual int Priority { get; }

        /// <summary>
        /// 类型名称。
        /// </summary>
        public virtual string Name => GetType().Name.Replace("SectionRender", string.Empty);

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
        public virtual string EditUrl => $"./{Name}/Index";

        /// <summary>
        /// 描述。
        /// </summary>
        public virtual string? Summary { get; }

        /// <summary>
        /// 样式。
        /// </summary>
        public virtual string? Style => "/*没有填写选择器或者&，表示当前顶级元素*/";

        /// <summary>
        /// 脚本。
        /// </summary>
        public virtual string? Script => "/*脚本格式为：\r\nfunction($this){\r\n  //$this：表示当前顶级元素的jQuery实例\r\n}*/";

        /// <summary>
        /// 呈现节点实例。
        /// </summary>
        /// <param name="context">节点上下文。</param>
        /// <param name="output">输出实例对象。</param>
        /// <returns>当前节点呈现任务。</returns>
        public abstract Task ProcessAsync(SectionContext context, TagBuilder output);
    }
}