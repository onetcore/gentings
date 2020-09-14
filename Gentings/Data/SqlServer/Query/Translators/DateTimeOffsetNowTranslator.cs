using System;
using System.Linq;
using System.Linq.Expressions;
using Gentings.Data.Query.Expressions;
using Gentings.Data.Query.Translators;

namespace Gentings.Data.SqlServer.Query.Translators
{
    /// <summary>
    /// DateTimeOffset.Now转换。
    /// </summary>
    public class DateTimeOffsetNowTranslator : IMemberTranslator
    {
        /// <summary>
        /// 转换字段或属性表达式。
        /// </summary>
        /// <param name="memberExpression">转换字段或属性表达式。</param>
        /// <returns>转换后的表达式。</returns>
        public virtual Expression Translate(MemberExpression memberExpression)
        {
            if (memberExpression.Expression == null
                && memberExpression.Member.DeclaringType == typeof(DateTimeOffset)
                && memberExpression.Member.Name == nameof(DateTimeOffset.Now))
            {
                return new SqlFunctionExpression("SYSDATETIMEOFFSET", memberExpression.Type, Enumerable.Empty<Expression>());
            }

            return null;
        }
    }
}