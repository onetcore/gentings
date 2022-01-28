using Gentings.Extensions.Sites.Sections.Carousels;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Carousel
{
    /// <summary>
    /// 页面滚动项目列表。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly ICarouselManager _carouselManager;
        /// <summary>
        /// 初始化类<see cref="IndexModel"/>。
        /// </summary>
        /// <param name="carouselManager">滑动项目管理接口。</param>
        public IndexModel(ICarouselManager carouselManager)
        {
            _carouselManager = carouselManager;
        }

        /// <summary>
        /// 滚动项目Id。
        /// </summary>
        public int SectionId { get; private set; }

        /// <summary>
        /// 页面列表。
        /// </summary>
        public IEnumerable<Extensions.Sites.Sections.Carousels.Carousel> Items { get; private set; }

        public IActionResult OnGet(int id)
        {
            if (id <= 0)
                return NotFound();
            SectionId = id;
            Items = _carouselManager.Fetch(x => x.SectionId == id).OrderBy(x => x.Order);
            return Page();
        }

        public IActionResult OnPostDelete(int[] id)
        {
            if (id == null || id.Length == 0)
                return Error("请先选择项目后再进行删除操作！");
            var result = _carouselManager.Delete(id);
            return Json(result, "项目");
        }
    }
}
