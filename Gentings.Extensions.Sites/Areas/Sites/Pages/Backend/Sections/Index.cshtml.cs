using Gentings.Extensions.Sites.SectionRenders;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// ҳ��ڵ��б�
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IPageManager _pageManager;

        public IndexModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// ��ǰҳ�档
        /// </summary>
        public Page? CurrentPage { get; private set; }

        /// <summary>
        /// ҳ���б�
        /// </summary>
        public IEnumerable<Section>? Items { get; private set; }

        /// <summary>
        /// ��ȡ�ڵ�����ʵ����
        /// </summary>
        /// <param name="name">�ڵ��������ơ�</param>
        /// <returns>���ؽڵ�����ʵ����</returns>
        public ISectionRender GetSectionRender(string name) => SectionManager.GetSectionRender(name);

        public IActionResult OnGet(int pid)
        {
            if (pid <= 0)
                return NotFound();
            CurrentPage = _pageManager.Find(pid);
            Items = SectionManager.Fetch(x => x.PageId == pid).OrderBy(x => x.Order);
            return Page();
        }

        public IActionResult OnPostDelete(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("����ѡ��ڵ���ٽ���ɾ��������");
            var result = SectionManager.Delete(id);
            return Json(result, "�ڵ�");
        }

        public async Task<IActionResult> OnPostMoveUp(int id)
        {
            var result = await SectionManager.MoveUpAsync(id);
            if (result)
                return Success();
            return Error("�ƶ�ʧ�ܣ�");
        }

        public async Task<IActionResult> OnPostMoveDown(int id)
        {
            var result = await SectionManager.MoveDownAsync(id);
            if (result)
                return Success();
            return Error("�ƶ�ʧ�ܣ�");
        }
    }
}
