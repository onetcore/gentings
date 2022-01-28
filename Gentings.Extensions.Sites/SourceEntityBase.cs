namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 源码实体基类。
    /// </summary>
    public abstract class SourceEntityBase : SourceBase, IIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 唯一名称。
        /// </summary>
        [Size(64)]
        [NotUpdated]
        public string? Name { get; set; }

        /// <summary>
        /// 是否禁用。
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 显示模式。
        /// </summary>
        public DisplayMode DisplayMode { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 更新时间。
        /// </summary>
        public DateTimeOffset? UpdatedDate { get; set; }
    }
}
