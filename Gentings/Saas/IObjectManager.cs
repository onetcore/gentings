using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Saas.Internal;

namespace Gentings.Saas
{
    /// <summary>
    /// 对象管理接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public interface IObjectManager<TModel, TKey> : IObjectManagerBase<TModel, TKey>
        where TModel : ISiteIdObject<TKey>
    {
        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回实例列表。</returns>
        IEnumerable<TModel> LoadAll<TQuery>(TQuery query) where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <returns>返回实例列表。</returns>
        IEnumerable<TObject> LoadAll<TQuery, TObject>(TQuery query) where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回实例列表。</returns>
        Task<IEnumerable<TModel>> LoadAllAsync<TQuery>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>;

        /// <summary>
        /// 获取所有符合条件的实例列表，主要用于导出操作。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回实例列表。</returns>
        Task<IEnumerable<TObject>> LoadAllAsync<TQuery, TObject>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>;
    }

    /// <summary>
    /// 对象管理接口。
    /// </summary>
    /// <typeparam name="TModel">模型类型。</typeparam>
    public interface IObjectManager<TModel> : IObjectManager<TModel, int>
        where TModel : ISiteIdObject
    {
    }
}