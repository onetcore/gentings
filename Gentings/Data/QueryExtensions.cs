using System.Linq.Expressions;
using System;
using Gentings.Extensions;

namespace Gentings.Data
{
    /// <summary>
    /// 数据查询扩展类。
    /// </summary>
    public static class QueryExtensions
    {
        /// <summary>
        /// 选择用户相关联字段。
        /// </summary>
        /// <typeparam name="TModel">当前实例模型。</typeparam>
        /// <typeparam name="JModel">级联实例类型。</typeparam>
        /// <param name="context">数据操作接口实例对象。</param>
        /// <param name="expression">关联表达式。</param>
        /// <param name="select">选择返回列对象实例。</param>
        /// <returns>返回当前查询实例。</returns>
        public static IQueryable<TModel> JoinSelect<TModel, JModel>(this IDbContext<TModel> context, Expression<Func<TModel, JModel, bool>> expression, Expression<Func<JModel, object>> select)
            where TModel : class
            where JModel : class
        {
            return context.AsQueryable().WithNolock().InnerJoin<JModel>(expression).Select().Select(select);
        }

        /// <summary>
        /// 选择用户相关联字段。
        /// </summary>
        /// <typeparam name="TModel">当前实例模型。</typeparam>
        /// <typeparam name="JModel">级联实例类型。</typeparam>
        /// <param name="context">数据操作接口实例对象。</param>
        /// <param name="expression">关联表达式。</param>
        /// <param name="select">选择返回列对象实例。</param>
        /// <returns>返回当前查询实例。</returns>
        public static IQueryContext<TModel> JoinSelect<TModel, JModel>(this IQueryContext<TModel> context, Expression<Func<TModel, JModel, bool>> expression, Expression<Func<JModel, object>> select)
            where TModel : class
            where JModel : class
        {
            return context.WithNolock().InnerJoin<JModel>(expression).Select().Select(select);
        }
    }
}
