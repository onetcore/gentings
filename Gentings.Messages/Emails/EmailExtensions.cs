using System;
using System.Collections.Generic;

namespace Gentings.Messages.Emails
{
    /// <summary>
    /// 扩展类。
    /// </summary>
    public static class EmailExtensions
    {
        /// <summary>
        /// 替换关键词。
        /// </summary>
        /// <param name="message">当前电子邮件。</param>
        /// <param name="action">添加关键词。</param>
        /// <returns>返回替换后的字符串。</returns>
        public static string ReplaceBy(this string message, Action<IDictionary<string, string>> action)
        {
            var dic = new Dictionary<string, string>();
            action(dic);
            foreach (var kw in dic)
            {
                message = message.Replace($"[${kw.Key};]", kw.Value);
            }
            return message;
        }
    }
}