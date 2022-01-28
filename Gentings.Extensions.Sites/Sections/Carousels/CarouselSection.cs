using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.Extensions.Sites.Sections.Carousels
{
    /// <summary>
    /// Carousel节点。
    /// </summary>
    public class CarouselSection : SectionRenderBase
    {
        private readonly ICarouselManager _carouselManager;
        /// <summary>
        /// 初始化类<see cref="CarouselSection"/>。
        /// </summary>
        /// <param name="carouselManager">滚动管理接口。</param>
        public CarouselSection(ICarouselManager carouselManager)
        {
            _carouselManager = carouselManager;
        }

        /// <summary>
        /// 图标地址。
        /// </summary>
        public override string IconUrl => "bi-sliders";

        /// <summary>
        /// 显示名称。
        /// </summary>
        public override string DisplayName => "滚动节点";

        /// <summary>
        /// 配置地址。
        /// </summary>
        public override string EditUrl => "./Carousel/Index";

        /// <summary>
        /// 样式。
        /// </summary>
        public override string Style => @"&.carousel{z-index:-1;}";

        /// <summary>
        /// 呈现节点实例。
        /// </summary>
        /// <param name="context">节点上下文。</param>
        /// <param name="output">输出实例对象。</param>
        /// <returns>当前节点呈现任务。</returns>
        public override async Task ProcessAsync(SectionContext context, TagBuilder output)
        {
            output.AddCssClass("slide");
            output.AddCssClass("carousel");
            output.MergeAttribute("data-bs-ride", "carousel");
            var carousels = await _carouselManager.FetchAsync(x => x.SectionId == context.Section.Id && !x.Disabled);
            if (!carousels.Any())
                return;
            // 滚动条
            carousels = carousels.OrderBy(x => x.Order);
            var indicators = new TagBuilder("ol");
            indicators.AddCssClass("carousel-indicators");
            output.InnerHtml.AppendHtml(indicators);
            var index = 0;
            foreach (var carousel in carousels)
            {
                var li = new TagBuilder("li");
                li.MergeAttribute("data-bs-target", $"#{context.Section.UniqueId}");
                li.MergeAttribute("data-bs-slide-to", index.ToString());
                if (index == 0)
                    li.AddCssClass("active");
                indicators.InnerHtml.AppendHtml(li);
                index++;
            }
            // 图片以及描述
            var inner = new TagBuilder("div");
            inner.AddCssClass("carousel-inner");
            output.InnerHtml.AppendHtml(inner);
            index = 0;
            foreach (var carousel in carousels)
            {
                var item = new TagBuilder("div");
                inner.InnerHtml.AppendHtml(item);
                item.AddCssClass("carousel-item");
                if (index == 0)
                    item.AddCssClass("active");
                var styles = new Dictionary<string, string>();
                if (!string.IsNullOrEmpty(carousel.ImageUrl))
                    styles.Add("background-image", $"url({carousel.ImageUrl})");
                if (!string.IsNullOrEmpty(carousel.BgColor))
                    styles.Add("background-color", carousel.BgColor);
                var image = new TagBuilder("div");
                if (styles.Count > 0)
                    image.MergeAttribute("style", styles.Select(x => $"{x.Key}:{x.Value}").Join(";"));
                image.AddCssClass("carousel-image");
                image.AddCssClass("carousel-index" + index);
                item.InnerHtml.AppendHtml(image);
                // 内容块
                var caption = new TagBuilder("div");
                item.InnerHtml.AppendHtml(caption);
                caption.AddCssClass("carousel-caption d-none d-md-block");
                if (!string.IsNullOrWhiteSpace(carousel.HTML))
                {
                    caption.InnerHtml.AppendHtml(carousel.HTML);
                }
                else if (!string.IsNullOrEmpty(carousel.Title))
                {
                    var h5 = new TagBuilder("h5");
                    caption.InnerHtml.AppendHtml(h5);
                    h5.InnerHtml.AppendHtml(carousel.Title);
                    if (!string.IsNullOrEmpty(carousel.Description))
                    {
                        var p = new TagBuilder("p");
                        caption.InnerHtml.AppendHtml(p);
                        p.InnerHtml.AppendHtml(carousel.Description);
                    }
                }
                index++;
            }
        }
    }
}