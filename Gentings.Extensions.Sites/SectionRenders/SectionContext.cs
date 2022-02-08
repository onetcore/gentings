namespace Gentings.Extensions.Sites.SectionRenders
{
    /// <summary>
    /// 节点上下文。
    /// </summary>
    public class SectionContext
    {
        /// <summary>
        /// 节点实例对象。
        /// </summary>
        public Section Section { get; }

        /// <summary>
        /// 当前页面上下文实例。
        /// </summary>
        public PageContext Context { get; }

        internal SectionContext(PageContext context, Section section)
        {
            Context = context;
            Section = section;
        }
    }
}