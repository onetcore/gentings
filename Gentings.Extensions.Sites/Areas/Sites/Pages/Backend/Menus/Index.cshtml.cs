using Gentings.Extensions.Sites.Menus;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Menus
{
    /// <summary>
    /// 菜单分类。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IMenuCategoryManager _categoryManager;

        public IndexModel(IMenuCategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        /// <summary>
        /// 分类列表。
        /// </summary>
        public IEnumerable<MenuCategory> Items { get; private set; }

        public async Task OnGetAsync()
        {
            Items = await _categoryManager.FetchAsync();
        }

        public async Task<IActionResult> OnPostAsync(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请先选择分类后再进行删除操作！");
            var result = await _categoryManager.DeleteAsync(id);
            return Json(result, "分类");
        }
    }
}
