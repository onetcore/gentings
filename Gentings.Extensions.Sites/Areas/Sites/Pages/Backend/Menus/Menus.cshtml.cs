using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Menus
{
    /// <summary>
    /// �˵��б�
    /// </summary>
    public class MenusModel : ModelBase
    {
        private readonly IPageMenuManager _menuManager;

        public MenusModel(IPageMenuManager menuManager)
        {
            _menuManager = menuManager;
        }

        /// <summary>
        /// �˵��б�
        /// </summary>
        public IEnumerable<PageMenu> Items { get; private set; }

        /// <summary>
        /// ����Id��
        /// </summary>
        public int CategoryId { get; private set; }

        public async Task<IActionResult> OnGet(int cid)
        {
            if (cid <= 0)
                return NotFound();
            CategoryId = cid;
            var items = await _menuManager.FetchAsync(x => x.CategoryId == cid);
            Items = items.Where(x => x.ParentId == 0)
                .OrderBy(x => x.Order)
                .ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostMoveUpAsync(int id)
        {
            if (await _menuManager.MoveUpAsync(id))
            {
                return Success();
            }

            return Error("����ʧ�ܣ������ԣ�");
        }

        public async Task<IActionResult> OnPostMoveDownAsync(int id)
        {
            if (await _menuManager.MoveDownAsync(id))
            {
                return Success();
            }

            return Error("����ʧ�ܣ������ԣ�");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int[] id)
        {
            if (id == null || id.Length == 0)
            {
                return Error("����ѡ��˵����ٽ���ɾ��������");
            }

            var result = await _menuManager.DeleteAsync(id);
            return Json(result, "�˵�");
        }
    }
}
