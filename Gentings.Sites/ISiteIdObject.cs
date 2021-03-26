using System.ComponentModel.DataAnnotations;
using Gentings.Data.Extensions;
using Gentings.Extensions;

namespace Gentings.Sites
{
    /// <summary>
    /// 支持SaaS的对象接口。
    /// </summary>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public interface ISiteIdObject<TKey> : ISite, IIdObject<TKey>
    {

    }

    /// <summary>
    /// 支持SaaS的对象基类。
    /// </summary>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public abstract class SiteIdObject<TKey> : ISiteIdObject<TKey>
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [NotUpdated]
        public virtual int SiteId { get; set; }

        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Key]
        public virtual TKey Id { get; set; }
    }

    /// <summary>
    /// 支持SaaS的扩展对象基类。
    /// </summary>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public abstract class SiteExtendBase<TKey> : ExtendBase, ISiteIdObject<TKey>
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [NotUpdated]
        public virtual int SiteId { get; set; }

        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Key]
        public virtual TKey Id { get; set; }
    }

    /// <summary>
    /// 支持SaaS的对象接口。
    /// </summary>
    public interface ISiteIdObject : ISiteIdObject<int>, IIdObject
    {

    }

    /// <summary>
    /// 支持SaaS的对象基类。
    /// </summary>
    public abstract class SiteIdObject : SiteIdObject<int>, ISiteIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public override int Id { get; set; }
    }

    /// <summary>
    /// 支持SaaS的扩展对象基类。
    /// </summary>
    public abstract class SiteExtendBase : ExtendBase, ISiteIdObject
    {
        /// <summary>
        /// 网站Id。
        /// </summary>
        [NotUpdated]
        public virtual int SiteId { get; set; }

        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public virtual int Id { get; set; }
    }
}