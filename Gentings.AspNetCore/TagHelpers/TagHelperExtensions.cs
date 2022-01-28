using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Gentings.AspNetCore.TagHelpers
{
    /// <summary>
    /// 扩展类。
    /// </summary>
    public static class TagHelperExtensions
    {
        /// <summary>
        /// 添加子元素。
        /// </summary>
        /// <param name="builder">当前标签构建实例。</param>
        /// <param name="tagName">子元素名称。</param>
        /// <param name="action">子元素配置方法。</param>
        /// <returns>返回当前标签实例。</returns>
        public static TagBuilder AppendTag(this TagBuilder builder, string tagName, Action<TagBuilder> action)
        {
            var tag = new TagBuilder(tagName);
            action(tag);
            builder.InnerHtml.AppendHtml(tag);
            return builder;
        }

        /// <summary>
        /// 讲当前输出设置为<paramref name="builder"/>元素实例。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="builder">构建实例对象。</param>
        public static void Render(this TagHelperOutput output, TagBuilder builder)
        {
            output.TagName = builder.TagName;
            output.MergeAttributes(builder);
            output.Content.AppendHtml(builder.InnerHtml);
        }

        /// <summary>
        /// 讲当前输出设置为<paramref name="action"/>元素实例。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="tagName">标签名称。</param>
        /// <param name="action">构建实例对象。</param>
        public static void Render(this TagHelperOutput output, string tagName, Action<TagBuilder> action)
        {
            var builder = new TagBuilder(tagName);
            action(builder);
            output.Render(builder);
        }

        /// <summary>
        /// 讲当前输出设置为<paramref name="action"/>元素实例。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="tagName">标签名称。</param>
        /// <param name="action">构建实例对象。</param>
        public static async Task RenderAsync(this TagHelperOutput output, string tagName, Func<TagBuilder, Task> action)
        {
            var builder = new TagBuilder(tagName);
            await action(builder);
            output.Render(builder);
        }

        /// <summary>
        /// 附加HTML内容。
        /// </summary>
        /// <param name="output">当前输出实例。</param>
        /// <param name="htmlContent">HTML内容。</param>
        public static void AppendHtml(this TagHelperOutput output, IHtmlContent htmlContent)
        {
            output.Content.AppendHtml(htmlContent);
        }

        /// <summary>
        /// 附加HTML内容。
        /// </summary>
        /// <param name="output">当前输出实例。</param>
        /// <param name="encoded">HTML内容。</param>
        public static void AppendHtml(this TagHelperOutput output, string encoded)
        {
            output.Content.AppendHtml(encoded);
        }

        /// <summary>
        /// 附加HTML标签。
        /// </summary>
        /// <param name="output">当前输出实例。</param>
        /// <param name="tagName">标签名称。</param>
        /// <param name="action">HTML标签实例化方法。</param>
        public static void AppendHtml(this TagHelperOutput output, string tagName, Action<TagBuilder> action)
        {
            var builder = new TagBuilder(tagName);
            action(builder);
            output.Content.AppendHtml(builder);
        }

        /// <summary>
        /// 附加HTML标签。
        /// </summary>
        /// <param name="output">当前输出实例。</param>
        /// <param name="tagName">标签名称。</param>
        /// <param name="action">HTML标签实例化方法。</param>
        public static async Task AppendHtmlAsync(this TagHelperOutput output, string tagName, Func<TagBuilder, Task> action)
        {
            var builder = new TagBuilder(tagName);
            await action(builder);
            output.Content.AppendHtml(builder);
        }

        /// <summary>
        /// 获取属性值。
        /// </summary>
        /// <param name="output">当前输出实例。</param>
        /// <param name="attributeName">属性名称。</param>
        /// <returns>返回当前属性值。</returns>
        public static string? GetAttribute(this TagHelperOutput output, string attributeName)
        {
            if (output.Attributes.TryGetAttribute(attributeName, out var attribute))
                return attribute.Value?.ToString();
            return null;
        }

        /// <summary>
        /// 添加样式。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="classNames">样式列表。</param>
        public static void AddClass(this TagHelperOutput output, params string[] classNames)
        {
            foreach (var className in classNames)
            {
                output.AddClass(className, HtmlEncoder.Default);
            }
        }

        /// <summary>
        /// 移除样式。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="className">样式表。</param>
        public static void RemoveClass(this TagHelperOutput output, string className)
        {
            output.RemoveClass(className, HtmlEncoder.Default);
        }

        /// <summary>
        /// 添加样式。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="name">属性名称。</param>
        /// <param name="value">属性值。</param>
        public static void SetAttribute(this TagHelperOutput output, string name, string value)
        {
            output.Attributes.SetAttribute(new TagHelperAttribute(name, value));
        }

        /// <summary>
        /// 添加MarkDown编辑器按钮。
        /// </summary>
        /// <returns>返回当前工具栏标签实例。</returns>
        /// <param name="builder">当前工具栏标签实例。</param>
        /// <param name="key">功能键。</param>
        /// <param name="icon">图标。</param>
        /// <param name="title">标题。</param>
        public static TagBuilder AddSyntax(this TagBuilder builder, string key, string icon, string title)
        {
            return builder.AppendTag("a", a =>
            {
                a.AppendTag("i", x => x.AddCssClass(icon));
                a.MergeAttribute("title", title);
                a.AddCssClass($"mozmd-syntax-{key}");
            });
        }

        /// <summary>
        /// 附加样式文件引用。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="path">样式文件路径。</param>
        public static void AppendStyle(this TagHelperOutput output, string path)
        {
            if (!path.EndsWith(".css", StringComparison.OrdinalIgnoreCase))
                path += ".css";
            output.AppendHtml("link", x =>
            {
                x.MergeAttribute("rel", "stylesheet");
                x.MergeAttribute("href", path);
                x.TagRenderMode = TagRenderMode.SelfClosing;
            });
        }

        /// <summary>
        /// 附加脚本文件引用。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="path">脚本文件路径。</param>
        public static void AppendScript(this TagHelperOutput output, string path)
        {
            if (!path.EndsWith(".js", StringComparison.OrdinalIgnoreCase))
                path += ".js";
            output.AppendHtml("script", x => x.MergeAttribute("src", path));
        }

        /// <summary>
        /// 附加样式文件引用。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="path">样式文件路径。</param>
        /// <param name="isDevelopment">是否为开发版本。</param>
        public static void AppendStyle(this TagHelperOutput output, string path, bool isDevelopment)
        {
            if (isDevelopment)
                path += ".css";
            else
                path += ".min.css";
            output.AppendHtml("link", x =>
            {
                x.MergeAttribute("rel", "stylesheet");
                x.MergeAttribute("href", path);
                x.TagRenderMode = TagRenderMode.SelfClosing;
            });
        }

        /// <summary>
        /// 附加脚本文件引用。
        /// </summary>
        /// <param name="output">输出实例对象。</param>
        /// <param name="path">脚本文件路径。</param>
        /// <param name="isDevelopment">是否为开发版本。</param>
        public static void AppendScript(this TagHelperOutput output, string path, bool isDevelopment)
        {
            if (isDevelopment)
                path += ".js";
            else
                path += ".min.js";
            output.AppendHtml("script", x => x.MergeAttribute("src", path));
        }
    }
}