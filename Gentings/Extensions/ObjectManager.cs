using Gentings.Data;
using Gentings.Extensions.Internal;

namespace Gentings.Extensions
{
    /// <summary>
    /// 对象管理基类。
    /// </summary>
    /// <typeparam name="TModel">当前模型实例。</typeparam>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public abstract class ObjectManager<TModel, TKey> : ObjectManagerBase<TModel, TKey>, IObjectManager<TModel, TKey>
        where TModel : IIdObject<TKey>
    {
        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回实例列表。</returns>
        public virtual IEnumerable<TModel> LoadAll<TQuery>(TQuery query) where TQuery : QueryBase<TModel>
        {
            return Context.Fetch(query);
        }

        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回实例列表。</returns>
        public virtual IEnumerable<TObject> LoadAll<TQuery, TObject>(TQuery query) where TQuery : QueryBase<TModel>
        {
            return Context.Fetch<TQuery, TObject>(query);
        }

        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回实例列表。</returns>
        public virtual Task<IEnumerable<TModel>> LoadAllAsync<TQuery>(TQuery query, CancellationToken cancellationToken = default) where TQuery : QueryBase<TModel>
        {
            return Context.FetchAsync(query, cancellationToken);
        }

        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回实例列表。</returns>
        public virtual Task<IEnumerable<TObject>> LoadAllAsync<TQuery, TObject>(TQuery query, CancellationToken cancellationToken = default) where TQuery : QueryBase<TModel>
        {
            return Context.FetchAsync<TQuery, TObject>(query, cancellationToken);
        }

        /// <summary>
        /// 初始化类<see cref="ObjectManager{TModel,TKey}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        protected ObjectManager(IDbContext<TModel> context) : base(context)
        {
        }
    }

    /// <summary>
    /// 对象管理实现基类。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public abstract class ObjectManager<TModel> : ObjectManager<TModel, int>, IObjectManager<TModel>
        where TModel : IIdObject
    {
        /// <summary>
        /// 初始化类<see cref="ObjectManager{TModel}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        protected ObjectManager(IDbContext<TModel> context) : base(context)
        {
        }
    }
}