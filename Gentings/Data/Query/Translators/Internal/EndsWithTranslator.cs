﻿using System.Linq.Expressions;
using System.Reflection;
using Gentings.Data.Query.Expressions;

namespace Gentings.Data.Query.Translators.Internal
{
    /// <summary>
    /// EndsWith表达式转换类。
    /// </summary>
    public class EndsWithTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo
            = typeof(string).GetRuntimeMethod(nameof(string.EndsWith), new[] {typeof(string)});

        private static readonly MethodInfo _concat
            = typeof(string).GetRuntimeMethod(nameof(string.Concat), new[] {typeof(string), typeof(string)});

        /// <summary>
        /// 转换表达式。
        /// </summary>
        /// <param name="methodCallExpression">方法调用表达式。</param>
        /// <returns>返回转换后的表达式。</returns>
        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            Check.NotNull(methodCallExpression, nameof(methodCallExpression));

            return ReferenceEquals(methodCallExpression.Method, _methodInfo)
                ? new LikeExpression(
                    methodCallExpression.Object,
                    Expression.Add(new LiteralExpression("%"), methodCallExpression.Arguments[0], _concat))
                : null;
        }
    }
}