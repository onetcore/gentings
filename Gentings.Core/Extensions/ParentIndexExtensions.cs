using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Internal;
using Gentings.Data.Migrations;

namespace Gentings.Extensions
{
    /// <summary>
    /// 父级索引扩展类。
    /// </summary>
    public static class ParentIndexExtensions
    {
        /// <summary>
        /// 添加实例事务时候新建索引。
        /// </summary>
        /// <typeparam name="TParentIndex">索引类型。</typeparam>
        /// <param name="db">数据库事务操作接口实例。</param>
        /// <param name="id">当前实例Id。</param>
        /// <param name="parentId">父级Id。</param>
        /// <returns>返回添加结果。</returns>
        public static bool CreateIndex<TParentIndex>(this IDbTransactionContext<TParentIndex> db, int id, int parentId)
            where TParentIndex : IParentIndex, new()
        {
            //最顶级无需更新
            if (parentId <= 0) return true;
            db.ExecuteNonQuery($@"INSERT INTO {db.EntityType.Table}(ParentId, Id) SELECT ParentId, {id} FROM {db.EntityType.Table} WITH(NOLOCK) WHERE Id = {parentId};");
            return db.Create(new TParentIndex { Id = id, ParentId = parentId });
        }

        /// <summary>
        /// 添加实例事务时候新建索引。
        /// </summary>
        /// <typeparam name="TParentIndex">索引类型。</typeparam>
        /// <param name="db">数据库事务操作接口实例。</param>
        /// <param name="id">当前实例Id。</param>
        /// <param name="parentId">父级Id。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回添加结果。</returns>
        public static async Task<bool> CreateIndexAsync<TParentIndex>(this IDbTransactionContext<TParentIndex> db, int id, int parentId, CancellationToken cancellationToken = default)
            where TParentIndex : IParentIndex, new()
        {
            //最顶级无需更新
            if (parentId <= 0) return true;
            await db.ExecuteNonQueryAsync($@"INSERT INTO {db.EntityType.Table}(ParentId, Id) SELECT ParentId, {id} FROM {db.EntityType.Table} WITH(NOLOCK) WHERE Id = {parentId};", cancellationToken: cancellationToken);
            return await db.CreateAsync(new TParentIndex { Id = id, ParentId = parentId }, cancellationToken);
        }

        /// <summary>
        /// 更新父级Id实例事务时候新建索引。
        /// </summary>
        /// <typeparam name="TParentIndex">索引类型。</typeparam>
        /// <param name="db">数据库事务操作接口实例。</param>
        /// <param name="id">当前实例Id。</param>
        /// <param name="parentId">父级Id。</param>
        /// <returns>返回添加结果。</returns>
        public static bool UpdateIndex<TParentIndex>(this IDbTransactionContext<TParentIndex> db, int id, int parentId)
            where TParentIndex : IParentIndex, new()
        {
            db.Delete(x => x.Id == id);//删除原有的父级ID
            return db.CreateIndex(id, parentId);
        }

        /// <summary>
        /// 更新父级Id实例事务时候新建索引。
        /// </summary>
        /// <typeparam name="TParentIndex">索引类型。</typeparam>
        /// <param name="db">数据库事务操作接口实例。</param>
        /// <param name="id">当前实例Id。</param>
        /// <param name="parentId">父级Id。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回添加结果。</returns>
        public static async Task<bool> UpdateIndexAsync<TParentIndex>(this IDbTransactionContext<TParentIndex> db, int id, int parentId, CancellationToken cancellationToken = default)
            where TParentIndex : IParentIndex, new()
        {
            await db.DeleteAsync(x => x.Id == id, cancellationToken);//删除原有的父级ID
            return await db.CreateIndexAsync(id, parentId, cancellationToken);
        }

        /// <summary>
        /// 通过索引表格查询所有子项目列表。
        /// </summary>
        /// <typeparam name="TParentModel">分组模型类型。</typeparam>
        /// <typeparam name="TParentIndex">索引表格类型。</typeparam>
        /// <param name="queryable">当前分组模型数据库查询接口实例。</param>
        /// <param name="id">父级Id。</param>
        /// <param name="topOnly">是否只查询最顶层项目。</param>
        /// <returns>返回<paramref name="id"/>的顶层项目列表，并且子项目已经附加到实例中。</returns>
        public static async Task<IEnumerable<TParentModel>> LoadChildrenAsync<TParentModel, TParentIndex>(this Data.IQueryable<TParentModel> queryable, int id, bool topOnly = true)
            where TParentModel : IParentable<TParentModel>
            where TParentIndex : IParentIndex
        {
            if (topOnly)
                return await queryable.Where(x => x.ParentId == id).AsEnumerableAsync<TParentModel>();
            //关联索引表
            if (id > 0)
                queryable = queryable.InnerJoin<TParentIndex>((u, s) => u.Id == s.Id)
                    .Where<TParentModel>(x => x.Id == id);
            var models = await queryable.AsEnumerableAsync<TParentModel>();
            if (models.MakeDictionary(id).TryGetValue(id, out var current))
                return current.Children;
            return Enumerable.Empty<TParentModel>();
        }

        /// <summary>
        /// 通过索引表格查询所有子项目列表。
        /// </summary>
        /// <typeparam name="TParentModel">分组模型类型。</typeparam>
        /// <typeparam name="TParentIndex">索引表格类型。</typeparam>
        /// <param name="queryable">当前分组模型数据库查询接口实例。</param>
        /// <param name="id">父级Id。</param>
        /// <param name="topOnly">是否只查询最顶层项目。</param>
        /// <returns>返回<paramref name="id"/>的顶层项目列表，并且子项目已经附加到实例中。</returns>
        public static IEnumerable<TParentModel> LoadChildren<TParentModel, TParentIndex>(this Data.IQueryable<TParentModel> queryable, int id, bool topOnly = true)
            where TParentModel : IParentable<TParentModel>
            where TParentIndex : IParentIndex
        {
            if (topOnly)
                return queryable.Where(x => x.ParentId == id).AsEnumerable<TParentModel>();
            //关联索引表
            if (id > 0)
                queryable = queryable.InnerJoin<TParentIndex>((u, s) => u.Id == s.Id)
                    .Where<TParentModel>(x => x.Id == id);
            var models = queryable.AsEnumerable<TParentModel>();
            if (models.MakeDictionary(id).TryGetValue(id, out var current))
                return current.Children;
            return Enumerable.Empty<TParentModel>();
        }

        /// <summary>
        /// 添加父级索引表格。
        /// </summary>
        /// <typeparam name="TParentModel">分组模型类型。</typeparam>
        /// <typeparam name="TParentIndex">索引表格类型。</typeparam>
        /// <param name="builder">数据库迁移构建实例。</param>
        public static void CreateIndexTable<TParentModel, TParentIndex>(this MigrationBuilder builder)
            where TParentModel : IParentable<TParentModel>
            where TParentIndex : IParentIndex
        {
            builder.CreateTable<TParentIndex>(table => table
                .Column(x => x.ParentId)
                .Column(x => x.Id)
                .ForeignKey<TParentModel>(x => x.ParentId, x => x.Id, onDelete: ReferentialAction.Cascade));
        }
    }
}