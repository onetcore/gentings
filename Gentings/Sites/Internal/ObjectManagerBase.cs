﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions;

namespace Gentings.Sites.Internal
{
    /// <summary>
    /// 对象管理基类。
    /// </summary>
    /// <typeparam name="TModel">当前模型实例。</typeparam>
    /// <typeparam name="TKey">唯一键类型。</typeparam>
    public abstract class ObjectManagerBase<TModel, TKey> : IObjectManagerBase<TModel, TKey>
        where TModel : ISiteIdObject<TKey>
    {
        /// <summary>
        /// 数据库操作实例。
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected IDbContext<TModel> Context { get; }

        /// <summary>
        /// 初始化类<see cref="ObjectManagerBase{TModel,TKey}"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        protected ObjectManagerBase(IDbContext<TModel> context)
        {
            Context = context;
        }

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        public virtual DataResult Save(TModel model)
        {
            if (IsDuplicated(model))
            {
                return DataAction.Duplicate;
            }

            if (Context.Any(x => x.SiteId == model.SiteId && x.Id.Equals(model.Id)))
            {
                return DataResult.FromResult(Update(model), DataAction.Updated);
            }

            return DataResult.FromResult(Create(model), DataAction.Created);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        public virtual bool IsDuplicated(TModel model)
        {
            return false;
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public virtual DataResult Update(int siteId, TKey id, object fields)
        {
            return DataResult.FromResult(Context.Update(x => x.SiteId == siteId && x.Id.Equals(id), fields), DataAction.Updated);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public virtual DataResult Update(int siteId, Expression<Predicate<TModel>> expression, object fields)
        {
            return DataResult.FromResult(Context.Update(expression.AndAlso(siteId), fields), DataAction.Updated);
        }

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int siteId, Expression<Predicate<TModel>> expression)
        {
            return DataResult.FromResult(Context.Delete(expression.AndAlso(siteId)), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int siteId, TKey id)
        {
            return DataResult.FromResult(Context.Delete(x => x.SiteId == siteId && x.Id.Equals(id)), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual TModel Find(int siteId, TKey id)
        {
            return Context.Find(x => x.SiteId == siteId && x.Id.Equals(id));
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual TModel Find(int siteId, Expression<Predicate<TModel>> expression)
        {
            return Context.Find(expression.AndAlso(siteId));
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <returns>返回模型实例列表。</returns>
        public virtual IEnumerable<TModel> Fetch(int siteId, Expression<Predicate<TModel>> expression = null)
        {
            return Context.Fetch(expression.AndAlso(siteId));
        }

        /// <summary>
        /// 保存对象实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回保存结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public virtual async Task<DataResult> SaveAsync(TModel model, CancellationToken cancellationToken = default)
        {
            if (await IsDuplicatedAsync(model, cancellationToken))
            {
                return DataAction.Duplicate;
            }

            if (Context.Any(x => x.SiteId == model.SiteId && x.Id.Equals(model.Id)))
            {
                return DataResult.FromResult(await UpdateAsync(model, cancellationToken), DataAction.Updated);
            }

            return DataResult.FromResult(await CreateAsync(model, cancellationToken), DataAction.Created);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public virtual Task<bool> IsDuplicatedAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(false);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public virtual async Task<DataResult> UpdateAsync(int siteId, TKey id, object fields,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.UpdateAsync(x => x.SiteId == siteId && x.Id.Equals(id), fields, cancellationToken),
                DataAction.Updated);
        }

        /// <summary>
        /// 根据条件更新特定的实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="fields">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public virtual async Task<DataResult> UpdateAsync(int siteId, Expression<Predicate<TModel>> expression, object fields,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.UpdateAsync(expression.AndAlso(siteId), fields, cancellationToken),
                DataAction.Updated);
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public virtual void Clear()
        {
            Context.Delete();
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        public virtual Task ClearAsync(CancellationToken cancellationToken = default)
        {
            return Context.DeleteAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        public virtual void Clear(int siteId)
        {
            Context.Delete(x => x.SiteId == siteId);
        }

        /// <summary>
        /// 清空所有数据。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        public virtual Task ClearAsync(int siteId, CancellationToken cancellationToken = default)
        {
            return Context.DeleteAsync(x => x.SiteId == siteId, cancellationToken);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="ids">唯一Id集合。</param>
        /// <returns>返回删除结果。</returns>
        public virtual DataResult Delete(int siteId, IEnumerable<TKey> ids)
        {
            return DataResult.FromResult(Context.Delete(x => x.SiteId == siteId && x.Id.Included(ids)), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="ids">唯一Id集合。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(int siteId, IEnumerable<TKey> ids,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.DeleteAsync(x => x.SiteId == siteId && x.Id.Included(ids), cancellationToken),
                DataAction.Deleted);
        }

        /// <summary>
        /// 根据条件删除实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(int siteId, Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.DeleteAsync(expression.AndAlso(siteId), cancellationToken), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一Id删除对象实例。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回删除结果。</returns>
        public virtual async Task<DataResult> DeleteAsync(int siteId, TKey id, CancellationToken cancellationToken = default)
        {
            return DataResult.FromResult(await Context.DeleteAsync(x => x.SiteId == siteId && x.Id.Equals(id), cancellationToken), DataAction.Deleted);
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="id">唯一Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual Task<TModel> FindAsync(int siteId, TKey id, CancellationToken cancellationToken = default)
        {
            return Context.FindAsync(x => x.SiteId == siteId && x.Id.Equals(id), cancellationToken);
        }

        /// <summary>
        /// 通过唯一键获取当前值。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前模型实例。</returns>
        public virtual Task<TModel> FindAsync(int siteId, Expression<Predicate<TModel>> expression,
            CancellationToken cancellationToken = default)
        {
            return Context.FindAsync(expression.AndAlso(siteId), cancellationToken);
        }

        /// <summary>
        /// 根据条件获取列表。
        /// </summary>
        /// <param name="siteId">网站Id。</param>
        /// <param name="expression">条件表达式。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回模型实例列表。</returns>
        public virtual Task<IEnumerable<TModel>> FetchAsync(int siteId, Expression<Predicate<TModel>> expression = null,
            CancellationToken cancellationToken = default)
        {
            return Context.FetchAsync(expression.AndAlso(siteId), cancellationToken);
        }

        /// <summary>
        /// 实例化一个查询实例，这个实例相当于实例化一个查询类，不能当作属性直接调用。
        /// </summary>
        /// <returns>返回模型的一个查询实例。</returns>
        public virtual IQueryable<TModel> AsQueryable()
        {
            return Context.AsQueryable();
        }

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <returns>返回更新结果。</returns>
        public virtual bool Update(TModel model)
        {
            return Context.Update(model);
        }

        /// <summary>
        /// 更新特定的实例。
        /// </summary>
        /// <param name="model">更新对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回更新结果。</returns>
        public virtual Task<bool> UpdateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Context.UpdateAsync(model, cancellationToken);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool Create(TModel model)
        {
            return Context.Create(model);
        }

        /// <summary>
        /// 添加实例。
        /// </summary>
        /// <param name="model">添加对象。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回添加结果。</returns>
        public virtual Task<bool> CreateAsync(TModel model, CancellationToken cancellationToken = default)
        {
            return Context.CreateAsync(model, cancellationToken);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual IPageEnumerable<TModel> Load<TQuery>(TQuery query,
            Expression<Func<TModel, object>> countExpression = null) where TQuery : QueryBase<TModel>
        {
            return Context.Load(query, countExpression);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual IPageEnumerable<TObject> Load<TQuery, TObject>(TQuery query,
            Expression<Func<TModel, object>> countExpression = null) where TQuery : QueryBase<TModel>
        {
            return Context.Load<TQuery, TObject>(query, countExpression);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual Task<IPageEnumerable<TModel>> LoadAsync<TQuery>(TQuery query,
            Expression<Func<TModel, object>> countExpression = null, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>
        {
            return Context.LoadAsync(query, countExpression, cancellationToken);
        }

        /// <summary>
        /// 分页获取实例列表。
        /// </summary>
        /// <typeparam name="TObject">返回的对象模型类型。</typeparam>
        /// <typeparam name="TQuery">查询实例类型。</typeparam>
        /// <param name="query">查询实例。</param>
        /// <param name="countExpression">返回总记录数的表达式,用于多表拼接过滤重复记录数。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回分页实例列表。</returns>
        public virtual Task<IPageEnumerable<TObject>> LoadAsync<TQuery, TObject>(TQuery query,
            Expression<Func<TModel, object>> countExpression = null, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TModel>
        {
            return Context.LoadAsync<TQuery, TObject>(query, countExpression, cancellationToken);
        }
    }
}