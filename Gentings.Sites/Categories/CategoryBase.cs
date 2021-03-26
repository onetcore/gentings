using Gentings.Data.Extensions;

namespace Gentings.Sites.Categories
{
    /// <summary>
    /// 分类基类。
    /// </summary>
    public abstract class CategoryBase : ISiteIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public virtual int Id { get; set; }

        /// <summary>
        /// 网站Id。
        /// </summary>
        [NotUpdated]
        public int SiteId { get; set; }

        /// <summary>
        /// 分类名称。
        /// </summary>
        [Size(64)]
        public virtual string Name { get; set; }
    }
}