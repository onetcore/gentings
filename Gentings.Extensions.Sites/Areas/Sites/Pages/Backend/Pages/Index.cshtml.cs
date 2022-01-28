using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Pages
{
    /// <summary>
    /// 页面列表。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IPageManager _pageManager;

        public IndexModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// 查询实例。
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public PageQuery Query { get; set; }

        /// <summary>
        /// 页面列表。
        /// </summary>
        public IPageEnumerable<Page> Items { get; private set; }

        public void OnGet()
        {
            Items = _pageManager.Load(Query);
        }

        public IActionResult OnPostDelete(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请先选择页面后再进行删除操作！");
            var result = _pageManager.Delete(id);
            return Json(result, "页面");
        }
    }
}
