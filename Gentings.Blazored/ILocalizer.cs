using System;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace Gentings.Blazored
{
    /// <summary>
    /// 本地化接口。
    /// </summary>
    public interface ILocalizer
    {
        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <returns>返回本地资源<see cref="T:Microsoft.AspNetCore.Components.MarkupString" />实例。</returns>
        MarkupString this[string name] { get; }

        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <param name="arguments">参数列表。</param>
        /// <returns>返回本地资源<see cref="T:Microsoft.AspNetCore.Components.MarkupString" />实例。</returns>
        MarkupString this[string name, params object[] arguments] { get; }

        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <returns>返回本地资源字符串实例。</returns>
        string GetString(string name);

        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <param name="arguments">参数列表。</param>
        /// <returns>返回本地资源字符串实例。</returns>
        string GetString(string name, params object[] arguments);
    }

    /// <summary>
    /// 本地化资源实现类。
    /// </summary>
    public class Localizer : ILocalizer
    {
        /// <summary>
        /// 本地存储键。
        /// </summary>
        public const string StorageKey = "culture";

        private readonly IStringLocalizer _localizer;
        /// <summary>
        /// 初始化类<see cref="Localizer"/>。
        /// </summary>
        /// <param name="type">资源类型。</param>
        /// <param name="serviceProvider">服务提供者接口。</param>
        public Localizer(Type type, IServiceProvider serviceProvider)
        {
            _localizer = serviceProvider.GetRequiredService<IStringLocalizerFactory>().Create(type);
        }

        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <returns>返回本地资源<see cref="T:Microsoft.AspNetCore.Components.MarkupString" />实例。</returns>
        public virtual MarkupString this[string name] => new(GetString(name) ?? name);

        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <param name="arguments">参数列表。</param>
        /// <returns>返回本地资源<see cref="T:Microsoft.AspNetCore.Components.MarkupString" />实例。</returns>
        public virtual MarkupString this[string name, params object[] arguments] => new(GetString(name, arguments) ?? string.Format(name, arguments));

        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <returns>返回本地资源字符串实例。</returns>
        public virtual string GetString(string name)
        {
            return _localizer[name];
        }

        /// <summary>获取本帝资源字符串。</summary>
        /// <param name="name">资源描述名称。</param>
        /// <param name="arguments">参数列表。</param>
        /// <returns>返回本地资源字符串实例。</returns>
        public virtual string GetString(string name, params object[] arguments)
        {
            return _localizer[name, arguments];
        }
    }
}