using Gentings.Extensions.Sites.Sections.Defaults;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Defaults
{
    /// <summary>
    /// �༭Ĭ�Ͻڵ㡣
    /// </summary>
    public class EditModel : EditModelBase<DefaultSection>
    {
        /// <summary>
        /// ����֮ǰ���½ڵ�ʵ����
        /// </summary>
        /// <param name="section">��ǰ����Ľڵ�ʵ����</param>
        /// <returns>���سɹ����ص�ǰҳ��ʵ�������򷵻ش�����Ϣ��</returns>
        protected override bool OnSaving(DefaultSection section)
        {
            section.Html = Input!.Html;
            return true;
        }
    }
}
