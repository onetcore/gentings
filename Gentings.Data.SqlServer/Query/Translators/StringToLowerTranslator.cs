﻿using Gentings.Data.Query.Translators;

namespace Gentings.Data.SqlServer.Query.Translators
{
    /// <summary>
    /// 小写转换。
    /// </summary>
    public class StringToLowerTranslator : ParameterlessInstanceMethodCallTranslator
    {
        /// <summary>
        /// 初始化类<see cref="StringToLowerTranslator"/>。
        /// </summary>
        public StringToLowerTranslator()
            : base(typeof(string), nameof(string.ToLower), "LOWER")
        {
        }
    }
}