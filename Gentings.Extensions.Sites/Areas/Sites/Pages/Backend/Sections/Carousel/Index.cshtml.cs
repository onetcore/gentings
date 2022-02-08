using Gentings.Extensions.Sites.SectionRenders.Carousels;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend.Sections.Carousel
{
    /// <summary>
    /// 页面滚动项目列表。
    /// </summary>
    public class IndexModel : RenderModelBase<CarouselSection>
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
        /// 页面列表。
        /// </summary>
        public IEnumerable<SectionRenders.Carousels.Carousel>? Items { get; private set; }

        /// <summary>
        /// 查询其他实例。
        /// </summary>
        /// <returns>返回成功返回当前页面实例，否则返回NotFound。</returns>
        protected override bool OnFound()
        {
            Items = _carouselManager.Fetch(x => x.SectionId == Section!.Id).OrderBy(x => x.Order).ToList();
            return true;
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
