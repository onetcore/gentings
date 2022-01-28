namespace Gentings.Extensions.Sites.Templates
{
    /// <summary>
    /// 模板管理接口。
    /// </summary>
    public interface ITemplateManager : ISingletonService
    {
        /// <summary>
        /// 模板列表。
        /// </summary>
        IEnumerable<ITemplate> Templates { get; }

        /// <summary>
        /// 通过名称获取模板实例。
        /// </summary>
        /// <param name="name">唯一名称。</param>
        /// <returns>返回页面模板。</returns>
        ITemplate GetTemplate(string? name);
    }
}
