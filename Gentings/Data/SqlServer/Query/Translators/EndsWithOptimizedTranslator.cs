using System.Linq.Expressions;
using System.Reflection;
using Gentings.Data.Query.Expressions;
using Gentings.Data.Query.Translators;

namespace Gentings.Data.SqlServer.Query.Translators
{
    /// <summary>
    /// EndsWith优化。
    /// </summary>
    public class EndsWithOptimizedTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo
            = typeof(string).GetRuntimeMethod(nameof(string.EndsWith), new[] { typeof(string) });

        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (ReferenceEquals(methodCallExpression.Method, _methodInfo))
            {
                Expression patternExpression = methodCallExpression.Arguments[0];
                ConstantExpression patternConstantExpression = patternExpression as ConstantExpression;

                BinaryExpression endsWithExpression = Expression.Equal(
                    new SqlFunctionExpression(
                        "RIGHT",
                        // ReSharper disable once PossibleNullReferenceException
                        methodCallExpression.Object.Type,
                        new[]
                        {
                            methodCallExpression.Object,
                            new SqlFunctionExpression("LEN", typeof(int), new[] { patternExpression })
                        }),
                    patternExpression);

                return new NotNullableExpression(
                    patternConstantExpression != null
                        ? (string)patternConstantExpression.Value == string.Empty
                            ? (Expression)Expression.Constant(true)
                            : endsWithExpression
                        : Expression.OrElse(
                            endsWithExpression,
                            Expression.Equal(patternExpression, Expression.Constant(string.Empty))));
            }

            return null;
        }
    }
}