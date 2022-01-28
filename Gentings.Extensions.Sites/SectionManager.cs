using Gentings.Data;
using Gentings.Extensions.Sites.Sections;
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
        IEnumerable<ISection> SectionTypes { get; }

        /// <summary>
        /// 通过节点类型名称获取节点呈现模板实例。
        /// </summary>
        /// <param name="sectionType">节点类型名称。</param>
        /// <returns>节点呈现模板实例。</returns>
        ISection GetSection(string? sectionType);

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
        private readonly ConcurrentDictionary<string, ISection> _sections;

        /// <summary>
        /// 初始化类<see cref="Section"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="sections">节点模板呈现列表。</param>
        public SectionManager(IDbContext<Section> context, IEnumerable<ISection> sections) : base(context)
        {
            _sections = new ConcurrentDictionary<string, ISection>(sections.OrderByDescending(x => x.Priority).ToDictionary(x => x.Name), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 节点呈现模板列表。
        /// </summary>
        public IEnumerable<ISection> SectionTypes => _sections.Values;

        /// <summary>
        /// 通过节点类型名称获取节点呈现模板实例。
        /// </summary>
        /// <param name="sectionType">节点类型名称。</param>
        /// <returns>节点呈现模板实例。</returns>
        public ISection GetSection(string? sectionType)
        {
            if(!_sections.TryGetValue(sectionType?? DefaultSection.Default, out var section))
            {
                section = _sections[DefaultSection.Default];
            }
            return section;
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
    }
}

