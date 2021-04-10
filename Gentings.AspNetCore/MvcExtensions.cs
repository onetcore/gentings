using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// MVC扩展类。
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// 添加当前页面试图引入库实例。
        /// </summary>
        /// <param name="context">当前试图上下文。</param>
        /// <param name="libraries">引入库实例。</param>
        public static void AddLibraries(this ViewContext context, ImportLibrary libraries)
        {
            libraries |= context.GetLibraries();
            context.ViewData[nameof(ImportLibrary)] = libraries;
        }

        /// <summary>
        /// 添加当前页面试图引入库实例。
        /// </summary>
        /// <param name="context">当前试图上下文。</param>
        /// <param name="libraries">引入库实例。</param>
        public static void AddLibraries(this PageContext context, ImportLibrary libraries)
        {
            if (context.ViewData.TryGetValue(nameof(ImportLibrary), out var value) &&
                value is ImportLibrary data)
                libraries |= data;
            context.ViewData[nameof(ImportLibrary)] = libraries;
        }

        /// <summary>
        /// 获取当前页面试图引入库实例。
        /// </summary>
        /// <param name="context">当前试图上下文。</param>
        /// <returns>返回当前页面试图引入库实例。</returns>
        public static ImportLibrary GetLibraries(this ViewContext context)
        {
            if (context.ViewData.TryGetValue(nameof(ImportLibrary), out var value) &&
                value is ImportLibrary data)
                return data;
            return ImportLibrary.None;
        }
    }
}