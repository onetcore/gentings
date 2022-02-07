using Gentings.Extensions.Sites.Sections.Defaults;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Defaults
{
    /// <summary>
    /// 编辑默认节点。
    /// </summary>
    public class EditModel : EditModelBase<DefaultSection>
    {
        /// <summary>
        /// 保存之前更新节点实例。
        /// </summary>
        /// <param name="section">当前保存的节点实例。</param>
        /// <returns>返回成功返回当前页面实例，否则返回错误信息。</returns>
        protected override bool OnSaving(DefaultSection section)
        {
            section.Html = Input!.Html;
            return true;
        }
    }
}
