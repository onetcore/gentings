﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Gentings.Data.Query.Expressions;

namespace Gentings.Data.Query.Translators
{
    /// <summary>
    /// 单个重载静态方法调用转换器基类。
    /// </summary>
    public abstract class SingleOverloadStaticMethodCallTranslator : IMethodCallTranslator
    {
        private readonly Type _declaringType;
        private readonly string _clrMethodName;
        private readonly string _sqlFunctionName;

        /// <summary>
        /// 初始化类<see cref="SingleOverloadStaticMethodCallTranslator"/>。
        /// </summary>
        /// <param name="declaringType">声明类型。</param>
        /// <param name="clrMethodName">CLR方法名称。</param>
        /// <param name="sqlFunctionName">SQL函数名称。</param>
        public SingleOverloadStaticMethodCallTranslator(Type declaringType, string clrMethodName,
            string sqlFunctionName)
        {
            _declaringType = declaringType;
            _clrMethodName = clrMethodName;
            _sqlFunctionName = sqlFunctionName;
        }

        /// <summary>
        /// 转换表达式。
        /// </summary>
        /// <param name="methodCallExpression">方法调用表达式。</param>
        /// <returns>返回转换后的表达式。</returns>
        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            var methodInfo = _declaringType.GetTypeInfo().GetDeclaredMethods(_clrMethodName).SingleOrDefault();
            if (methodInfo == methodCallExpression.Method)
            {
                return new SqlFunctionExpression(_sqlFunctionName, methodCallExpression.Type,
                    methodCallExpression.Arguments);
            }

            return null;
        }
    }
}