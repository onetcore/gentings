﻿using System.Linq.Expressions;
using System.Reflection;
using Gentings.Data.Query.Expressions;

namespace Gentings.Data.Query.Translators.Internal
{
    /// <summary>
    /// 转换字符串对比string.Compare表达式。
    /// </summary>
    public class StringCompareTranslator : IExpressionFragmentTranslator
    {
        private static readonly Dictionary<ExpressionType, ExpressionType> _operatorMap =
            new()
            {
                {ExpressionType.LessThan, ExpressionType.GreaterThan},
                {ExpressionType.LessThanOrEqual, ExpressionType.GreaterThanOrEqual},
                {ExpressionType.GreaterThan, ExpressionType.LessThan},
                {ExpressionType.GreaterThanOrEqual, ExpressionType.LessThanOrEqual},
                {ExpressionType.Equal, ExpressionType.Equal},
                {ExpressionType.NotEqual, ExpressionType.NotEqual},
            };

        private static readonly MethodInfo _methodInfo = typeof(string).GetTypeInfo()
            .GetDeclaredMethods(nameof(string.Compare))
            .Single(m => m.GetParameters().Count() == 2);

        /// <summary>
        /// 转换表达式。
        /// </summary>
        /// <param name="expression">当前表达式。</param>
        /// <returns>返回转换后的表达式。</returns>
        public virtual Expression? Translate(Expression? expression)
        {
            var binaryExpression = expression as BinaryExpression;
            if (binaryExpression != null)
            {
                if (!_operatorMap.ContainsKey(expression!.NodeType))
                {
                    return null;
                }

                var leftMethodCall = binaryExpression.Left as MethodCallExpression;
                var rightConstant = binaryExpression.Right as ConstantExpression;
                var translated = TranslateInternal(t => t, expression.NodeType, leftMethodCall, rightConstant);
                if (translated != null)
                {
                    return translated;
                }

                var leftConstant = binaryExpression.Left as ConstantExpression;
                var rightMethodCall = binaryExpression.Right as MethodCallExpression;
                var translatedReverse = TranslateInternal(t => _operatorMap[t], expression.NodeType, rightMethodCall,
                    leftConstant);
                if (translatedReverse != null)
                {
                    return translatedReverse;
                }
            }

            return null;
        }

        private Expression? TranslateInternal(
            Func<ExpressionType, ExpressionType> opFunc,
            ExpressionType op,
            MethodCallExpression? methodCall,
            ConstantExpression? constant)
        {
            if (methodCall != null
                && methodCall.Method == _methodInfo
                && methodCall.Type == typeof(int)
                && constant != null
                && constant.Type == typeof(int))
            {
                var arguments = methodCall.Arguments.ToList();
                var leftString = arguments[0];
                var rightString = arguments[1];
                var constantValue = (int) constant.Value!;

                if (constantValue == 0)
                {
                    // Compare(strA, strB) > 0 => strA > strB
                    return new StringCompareExpression(opFunc(op), leftString, rightString);
                }

                if (constantValue == 1)
                {
                    if (op == ExpressionType.Equal)
                    {
                        // Compare(strA, strB) == 1 => strA > strB
                        return new StringCompareExpression(ExpressionType.GreaterThan, leftString, rightString);
                    }

                    if (op == opFunc(ExpressionType.LessThan))
                    {
                        // Compare(strA, strB) < 1 => strA <= strB
                        return new StringCompareExpression(ExpressionType.LessThanOrEqual, leftString, rightString);
                    }
                }

                if (constantValue == -1)
                {
                    if (op == ExpressionType.Equal)
                    {
                        // Compare(strA, strB) == -1 => strA < strB
                        return new StringCompareExpression(ExpressionType.LessThan, leftString, rightString);
                    }

                    if (op == opFunc(ExpressionType.GreaterThan))
                    {
                        // Compare(strA, strB) > -1 => strA >= strB
                        return new StringCompareExpression(ExpressionType.GreaterThanOrEqual, leftString, rightString);
                    }
                }
            }

            return null;
        }
    }
}