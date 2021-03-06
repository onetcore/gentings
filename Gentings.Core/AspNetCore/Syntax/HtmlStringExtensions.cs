﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Gentings.AspNetCore.Syntax
{
    /// <summary>
    /// 字符串扩展类。
    /// </summary>
    public static class HtmlStringExtensions
    {
        private const string HtmlCaseRegexReplacement = "-$1$2";

        private static readonly Regex _htmlCaseRegex =
            new Regex(
                "(?<!^)((?<=[a-zA-Z0-9])[A-Z][a-z])|((?<=[a-z])[A-Z])",
                RegexOptions.None,
                TimeSpan.FromMilliseconds(500));

        /// <summary>
        /// 将pascal/camel格式的名称转换为小写并且以“-”分隔的字符串名称。
        /// </summary>
        /// <example>
        /// SomeThing => some-thing
        /// capsONInside => caps-on-inside
        /// CAPSOnOUTSIDE => caps-on-outside
        /// ALLCAPS => allcaps
        /// One1Two2Three3 => one1-two2-three3
        /// ONE1TWO2THREE3 => one1two2three3
        /// First_Second_ThirdHi => first_second_third-hi
        /// </example>
        public static string ToHtmlCase(this string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            return _htmlCaseRegex.Replace(name, HtmlCaseRegexReplacement).ToLowerInvariant();
        }

        private static readonly Regex _htmlRegex =
            new Regex("</*[a-z].*?>", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        /// <summary>
        /// 移除所有HTML标记。
        /// </summary>
        /// <param name="source">当前代码。</param>
        /// <param name="isBlank">是否移除空格。</param>
        /// <returns>返回移除后的结果。</returns>
        public static string RemoveHtml(this string source, bool isBlank = false)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            source = _htmlRegex.Replace(source, string.Empty).Trim();
            if (isBlank)
            {
                source = source
                    .Replace("&nbsp;", string.Empty)
                    .Replace(" ", string.Empty);
            }

            return source;
        }

        /// <summary>
        /// 分隔HTML标记。
        /// </summary>
        /// <param name="source">源代码。</param>
        /// <returns>返回分隔后的字符串。</returns>
        public static string[] SplitHtml(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                return null;
            }

            return _htmlRegex.Split(source)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .ToArray();
        }

        /// <summary>
        /// 解码HTML字符。
        /// </summary>
        /// <param name="source">字符串。</param>
        /// <returns>返回解码后的字符串。</returns>
        public static string DecodeHtml(this string source)
        {
            source = source.Replace("&quot;", "\"");
            source = source.Replace("&amp;", "&");
            source = source.Replace("&lt;", "<");
            source = source.Replace("&gt;", ">");
            return source;
        }

        /// <summary>
        /// 判断当前字符是否为引号：'"','`','\''。
        /// </summary>
        /// <param name="current">当前字符。</param>
        /// <returns>返回判断结果。</returns>
        public static bool IsQuote(this char current) => current == '"' || current == '`' || current == '\'';
    }
}