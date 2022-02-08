using Gentings.Extensions.Sites.SectionRenders;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections
{
    /// <summary>
    /// 页面节点列表。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly IPageManager _pageManager;

        public IndexModel(IPageManager pageManager)
        {
            _pageManager = pageManager;
        }

        /// <summary>
        /// 当前页面。
        /// </summary>
        public Page? CurrentPage { get; private set; }

        /// <summary>
        /// 页面列表。
        /// </summary>
        public IEnumerable<Section>? Items { get; private set; }

        /// <summary>
        /// 获取节点类型实例。
        /// </summary>
        /// <param name="name">节点类型名称。</param>
        /// <returns>返回节点类型实例。</returns>
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
                return Error("请先选择节点后再进行删除操作！");
            var result = SectionManager.Delete(id);
            return Json(result, "节点");
        }

        public async Task<IActionResult> OnPostMoveUp(int id)
        {
            var result = await SectionManager.MoveUpAsync(id);
            if (result)
                return Success();
            return Error("移动失败！");
        }

        public async Task<IActionResult> OnPostMoveDown(int id)
        {
            var result = await SectionManager.MoveDownAsync(id);
            if (result)
                return Success();
            return Error("移动失败！");
        }
    }
}
