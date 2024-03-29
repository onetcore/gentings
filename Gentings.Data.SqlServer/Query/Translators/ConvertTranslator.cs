﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Gentings.Data.Query.Expressions;
using Gentings.Data.Query.Translators;

namespace Gentings.Data.SqlServer.Query.Translators
{
    /// <summary>
    /// Convert.ToXXX转换表达式。
    /// </summary>
    public class ConvertTranslator : IMethodCallTranslator
    {
        private static readonly Dictionary<string, DbType> _typeMapping = new()
        {
            [nameof(Convert.ToByte)] = DbType.Byte,
            [nameof(Convert.ToDecimal)] = DbType.Decimal,
            [nameof(Convert.ToDouble)] = DbType.Double,
            [nameof(Convert.ToInt16)] = DbType.Int16,
            [nameof(Convert.ToInt32)] = DbType.Int32,
            [nameof(Convert.ToInt64)] = DbType.Int64,
            [nameof(Convert.ToString)] = DbType.String,
        };

        private static readonly List<Type> _supportedTypes = new()
        {
            typeof(bool),
            typeof(byte),
            typeof(decimal),
            typeof(double),
            typeof(float),
            typeof(int),
            typeof(long),
            typeof(short),
            typeof(string),
        };

        private static readonly IEnumerable<MethodInfo> _supportedMethods;

        static ConvertTranslator()
        {
            _supportedMethods = _typeMapping.Keys
                .SelectMany(t => typeof(Convert).GetTypeInfo().GetDeclaredMethods(t)
                    .Where(m => m.GetParameters().Count() == 1
                                && _supportedTypes.Contains(m.GetParameters().First().ParameterType)));
        }

        /// <summary>
        /// 转换表达式。
        /// </summary>
        /// <param name="methodCallExpression">方法调用表达式。</param>
        /// <returns>返回转换后的表达式。</returns>
        public virtual Expression? Translate(MethodCallExpression methodCallExpression)
        {
            if (_supportedMethods.Contains(methodCallExpression.Method))
            {
                var arguments = new[]
                {
                    Expression.Constant(_typeMapping[methodCallExpression.Method.Name]),
                    methodCallExpression.Arguments[0]
                };

                return new SqlFunctionExpression("CONVERT", methodCallExpression.Type, arguments);
            }

            return null;
        }
    }
}