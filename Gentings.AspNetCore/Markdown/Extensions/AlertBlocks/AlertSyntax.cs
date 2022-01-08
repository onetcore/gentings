﻿using Gentings.AspNetCore.Properties;
using System.Collections.Concurrent;

namespace Gentings.AspNetCore.Markdown.Extensions.AlertBlocks
{
    /// <summary>
    /// Alert标签语法类。
    /// </summary>
    public class AlertSyntax
    {
        static AlertSyntax()
        {
            var syntaxs = new List<SyntaxEntry>
            {
                 new ("warning", "warning", "bi-brightness-low-fill"),
                new ( "tip","info" , "bi-info-circle-fill"),
                new ( "note", "info", "bi-info-circle-fill"),
                new ( "important", "danger", "bi-exclamation-triangle-fill"),
                new ( "caution", "danger", "bi-exclamation-triangle-fill"),
            };
            foreach (var syntax in syntaxs)
            {
                _syntaxs.TryAdd(syntax.Syntax, syntax);
            }
        }

        /// <summary>
        /// 警告标签定义的语法键值。
        /// </summary>
        private static readonly ConcurrentDictionary<string, SyntaxEntry> _syntaxs = new ConcurrentDictionary<string, SyntaxEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 语法实体。
        /// </summary>
        public class SyntaxEntry
        {
            internal SyntaxEntry(string key, string alertClass, string icon)
            {
                Key = key;
                Text = Resources.ResourceManager.GetString("AlertBlock_" + key) ?? key.ToUpperInvariant();
                Syntax = $"> [!{key}]";
                AlertClass = alertClass;
                Icon = icon;
            }

            /// <summary>
            /// 唯一键。
            /// </summary>
            public string Key { get; }

            /// <summary>
            /// 显示字符串。
            /// </summary>
            public string Text { get; }

            /// <summary>
            /// 语法。
            /// </summary>
            public string Syntax { get; }

            /// <summary>
            /// 样式名称。
            /// </summary>
            public string AlertClass { get; }

            /// <summary>
            /// 图标样式。
            /// </summary>
            public string Icon { get; }
        }

        /// <summary>
        /// 所有语法列表。
        /// </summary>
        public static string[] Syntaxs => _syntaxs.Keys.ToArray();

        /// <summary>
        /// 获取语法实例对象。
        /// </summary>
        /// <param name="syntax">语法字符串。</param>
        /// <returns>返回语法实体对象。</returns>
        public static SyntaxEntry? GetSyntax(string syntax)
        {
            _syntaxs.TryGetValue(syntax, out var entry);
            return entry;
        }
    }
}