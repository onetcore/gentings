namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// 模型基类。
    /// </summary>
    public abstract class ModelBase : Backend.ModelBase
    {
        private ISectionManager? _sectionManager;
        /// <summary>
        /// 节点管理实例。
        /// </summary>
        protected ISectionManager SectionManager => _sectionManager ??= GetRequiredService<ISectionManager>();
    }
}
