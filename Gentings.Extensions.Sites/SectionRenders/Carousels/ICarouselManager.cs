using Gentings.Data;

namespace Gentings.Extensions.Sites.SectionRenders.Carousels
{
    /// <summary>
    /// Carousel管理接口。
    /// </summary>
    public interface ICarouselManager : IObjectManager<Carousel>, ISingletonService
    {
        /// <summary>
        /// 上移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        Task<bool> MoveUpAsync(int id);

        /// <summary>
        /// 下移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        Task<bool> MoveDownAsync(int id);
    }

    /// <summary>
    /// Carousel管理实现类。
    /// </summary>
    public class CarouselManager : ObjectManager<Carousel>, ICarouselManager
    {
        /// <summary>
        /// 初始化类<see cref="CarouselManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public CarouselManager(IDbContext<Carousel> context) : base(context)
        {
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        public override bool Create(Carousel model)
        {
            if (model.Id == 0)
                model.Order = 1 + Context.Max(x => x.Order, x => x.SectionId == model.SectionId);
            return base.Create(model);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        public override async Task<bool> CreateAsync(Carousel model, CancellationToken cancellationToken = default)
        {
            if (model.Id == 0)
                model.Order = 1 + await Context.MaxAsync(x => x.Order, x => x.SectionId == model.SectionId, cancellationToken);
            return await base.CreateAsync(model, cancellationToken);
        }

        /// <summary>
        /// 上移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        public virtual async Task<bool> MoveUpAsync(int id)
        {
            var carousel = await Context.FindAsync(x => x.Id == id);
            if (carousel == null)
                return false;

            if (await Context.MoveUpAsync(id, x => x.Order, x => x.SectionId == carousel.SectionId, false))
                return true;

            return false;
        }

        /// <summary>
        /// 下移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        public virtual async Task<bool> MoveDownAsync(int id)
        {
            var carousel = await Context.FindAsync(x => x.Id == id);
            if (carousel == null)
                return false;

            if (await Context.MoveDownAsync(id, x => x.Order, x => x.SectionId == carousel.SectionId, false))
                return true;

            return false;
        }
    }
}