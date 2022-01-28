using Gentings.Extensions.Sites.Sections;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// ҳ��ڵ��б�
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly ISectionManager _sectionManager;
        private readonly IPageManager _pageManager;

        public IndexModel(ISectionManager sectionManager, IPageManager pageManager)
        {
            _sectionManager = sectionManager;
            _pageManager = pageManager;
        }

        /// <summary>
        /// ��ǰҳ�档
        /// </summary>
        public Page CurrentPage { get; private set; }

        /// <summary>
        /// ҳ���б�
        /// </summary>
        public IEnumerable<Section> Items { get; private set; }

        /// <summary>
        /// ��ȡ�ڵ�����ʵ����
        /// </summary>
        /// <param name="name">�ڵ��������ơ�</param>
        /// <returns>���ؽڵ�����ʵ����</returns>
        public ISection GetSection(string name) => _sectionManager.GetSection(name);

        public IActionResult OnGet(int pid)
        {
            if (pid <= 0)
                return NotFound();
            CurrentPage = _pageManager.Find(pid);
            Items = _sectionManager.Fetch(x => x.PageId == pid).OrderBy(x => x.Order);
            return Page();
        }

        public IActionResult OnPostDelete(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("����ѡ��ڵ���ٽ���ɾ��������");
            var result = _sectionManager.Delete(id);
            return Json(result, "�ڵ�");
        }

        public async Task<IActionResult> OnPostMoveUp(int id)
        {
            var result = await _sectionManager.MoveUpAsync(id);
            if (result)
                return Success();
            return Error("�ƶ�ʧ�ܣ�");
        }

        public async Task<IActionResult> OnPostMoveDown(int id)
        {
            var result = await _sectionManager.MoveDownAsync(id);
            if (result)
                return Success();
            return Error("�ƶ�ʧ�ܣ�");
        }
    }
}
