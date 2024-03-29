﻿using Gentings.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

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

        /// <summary>
        /// 设置布局页面，一般在_ViewStart.cshtml页面中调用。
        /// </summary>
        /// <param name="page">当前视图页面实例。</param>
        /// <param name="defaultLayout">默认布局页。</param>
        public static void AddLayout(this IRazorPage page, string defaultLayout = "_Layout")
        {
            if (page.ViewContext.ActionDescriptor is PageActionDescriptor descriptor)
            {
                if (descriptor.ViewEnginePath.StartsWith("/Backend/", StringComparison.OrdinalIgnoreCase))
                {
                    page.Layout = "_Admin";
                    return;
                }
                if (descriptor.ViewEnginePath.StartsWith("/Account/", StringComparison.OrdinalIgnoreCase))
                {
                    page.Layout = "_Account";
                    return;
                }
            }
            page.Layout = defaultLayout;
        }

        private static TService GetRequiredService<TService>(this ModelBase model)
            => model.HttpContext.GetOrCreate(() => model.HttpContext.RequestServices.GetRequiredService<TService>());

        /// <summary>
        /// 获取UI资源本地字符串实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="resourceName">资源名称。</param>
        /// <returns>返回资源本地字符串。</returns>
        public static string? GetResource(this ModelBase model, string resourceName)
        {
            string? culture = null;
            if (model.RouteData.Values.TryGetValue("culture", out var vaule))
                culture = vaule?.ToString();
            return model.GetRequiredService<IResourceManager>().GetResource(model.GetType(), resourceName, culture);
        }

        /// <summary>
        /// 获取UI资源本地字符串实例。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <param name="resourceName">资源名称。</param>
        /// <param name="args">参数。</param>
        /// <returns>返回资源本地字符串。</returns>
        public static string? GetResource(this ModelBase model, string resourceName, params object[] args)
        {
            var resource = model.GetResource(resourceName);
            if (resource == null)
                return null;
            return string.Format(resource, args);
        }
    }
}