using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Menus
{
    /// <summary>
    /// �˵����ࡣ
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IMenuCategoryManager _categoryManager;

        public IndexModel(IMenuCategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        /// <summary>
        /// �����б�
        /// </summary>
        public IEnumerable<MenuCategory> Items { get; private set; }

        public async Task OnGetAsync()
        {
            Items = await _categoryManager.FetchAsync();
        }

        public async Task<IActionResult> OnPostAsync(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("����ѡ�������ٽ���ɾ��������");
            var result = await _categoryManager.DeleteAsync(id);
            return Json(result, "����");
        }
    }
}
