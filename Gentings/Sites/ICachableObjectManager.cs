using Gentings.Sites.Internal;

namespace Gentings.Sites
{
    /// <summary>
    /// 缓存对象管理接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    /// <typeparam name="TKey">模型主键类型。</typeparam>
    public interface ICachableObjectManager<TModel, TKey> : IObjectManagerBase<TModel, TKey>
        where TModel : ISiteIdObject<TKey>
    {
        /// <summary>
        /// 刷新缓存。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        void Refresh(int siteId);
    }

    /// <summary>
    /// 缓存对象管理接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public interface ICachableObjectManager<TModel> : ICachableObjectManager<TModel, int> where TModel : ISiteIdObject
    {
    }
}