using Gentings.Data;
using Gentings.Extensions.Sites.SectionRenders;
using Gentings.Extensions.Sites.SectionRenders.Defaults;
using System.Collections.Concurrent;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 节点类管理接口。
    /// </summary>
    public interface ISectionManager : IObjectManager<Section>, ISingletonService
    {
        /// <summary>
        /// 节点呈现模板列表。
        /// </summary>
        IEnumerable<ISectionRender> SectionRenderes { get; }

        /// <summary>
        /// 通过节点类型名称获取节点呈现模板实例。
        /// </summary>
        /// <param name="sectionType">节点类型名称。</param>
        /// <returns>节点呈现模板实例。</returns>
        ISectionRender GetSectionRender(string? sectionType);

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
    /// 节点类管理实现类。
    /// </summary>
    public class SectionManager : ObjectManager<Section>, ISectionManager
    {
        private readonly ConcurrentDictionary<string, ISectionRender> _sections;

        /// <summary>
        /// 初始化类<see cref="Section"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="sections">节点模板呈现列表。</param>
        public SectionManager(IDbContext<Section> context, IEnumerable<ISectionRender> sections) : base(context)
        {
            _sections = new ConcurrentDictionary<string, ISectionRender>(sections.OrderByDescending(x => x.Priority).ToDictionary(x => x.Name), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 节点呈现模板列表。
        /// </summary>
        public IEnumerable<ISectionRender> SectionRenderes => _sections.Values;

        /// <summary>
        /// 通过节点类型名称获取节点呈现模板实例。
        /// </summary>
        /// <param name="sectionType">节点类型名称。</param>
        /// <returns>节点呈现模板实例。</returns>
        public ISectionRender GetSectionRender(string? sectionType)
        {
            if (!_sections.TryGetValue(sectionType ?? DefaultSectionRender.Default, out var section))
            {
                section = _sections[DefaultSectionRender.Default];
            }
            return section;
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        public override bool Create(Section model)
        {
            if (model.Id == 0)
                model.Order = 1 + Context.Max(x => x.Order, x => x.PageId == model.PageId);
            return base.Create(model);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        public override async Task<bool> CreateAsync(Section model, CancellationToken cancellationToken = default)
        {
            if (model.Id == 0)
                model.Order = 1 + await Context.MaxAsync(x => x.Order, x => x.PageId == model.PageId, cancellationToken);
            return await base.CreateAsync(model, cancellationToken);
        }

        /// <summary>
        /// 上移一个位置。
        /// </summary>
        /// <param name="id">当前页面Id。</param>
        /// <returns>返回移动结果。</returns>
        public virtual async Task<bool> MoveUpAsync(int id)
        {
            var section = await Context.FindAsync(x => x.Id == id);
            if (section == null)
                return false;
            if (await Context.MoveUpAsync(id, x => x.Order, x => x.PageId == section.PageId, false))
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
            var section = await Context.FindAsync(x => x.Id == id);
            if (section == null)
                return false;
            if (await Context.MoveDownAsync(id, x => x.Order, x => x.PageId == section.PageId, false))
                return true;

            return false;
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        public override bool IsDuplicated(Section model)
        {
            return Context.Any(x => x.Name == model.Name && x.PageId == model.PageId && x.Id != model.Id);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回判断结果。</returns>
        public override Task<bool> IsDuplicatedAsync(Section model, CancellationToken cancellationToken = default)
        {
            return Context.AnyAsync(x => x.Name == model.Name && x.PageId == model.PageId && x.Id != model.Id, cancellationToken);
        }
    }
}

