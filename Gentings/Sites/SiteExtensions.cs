using System;
using System.Linq.Expressions;
using Gentings.Data;

namespace Gentings.Sites
{
    /// <summary>
    /// 网站扩展类。
    /// </summary>
    public static class SiteExtensions
    {
        /// <summary>
        /// 附加网站Id限制。
        /// </summary>
        /// <typeparam name="TModel">模型类型。</typeparam>
        /// <param name="expression">条件表达式。</param>
        /// <param name="siteId">网站Id。</param>
        /// <returns>返回附加网站Id条件判断的表达式实例。</returns>
        public static Expression<Predicate<TModel>> AndAlso<TModel>(this Expression<Predicate<TModel>> expression,
            int siteId) where TModel : ISite
        {
            Expression<Predicate<TModel>> predicate = x => x.SiteId == siteId;
            return expression == null ? predicate : expression.AndAlso(predicate);
        }
    }
}