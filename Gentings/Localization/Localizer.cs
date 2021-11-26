using System;
using System.Linq.Expressions;

namespace Gentings.Localization
{
    /// <summary>
    /// 本地化资源实现类。
    /// </summary>
    public class Localizer : ILocalizer
    {
        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(Enum key) => ResourceManager.GetString(key);

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(Enum key, params object[] args) => ResourceManager.GetString(key, args);

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <typeparam name="TResource">包含当前资源包的所在程序集的类型。</typeparam>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString<TResource>(string key) => ResourceManager.GetString<TResource>(key);

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <typeparam name="TResource">包含当前资源包的所在程序集的类型。</typeparam>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString<TResource>(string key, params object[] args) => ResourceManager.GetString<TResource>(key, args);

        /// <summary>
        /// 获取当前表达式类型属性得资源字符串。
        /// </summary>
        /// <typeparam name="TResource">当前属性所在得类型实例。</typeparam>
        /// <param name="expression">表达式。</param>
        /// <returns>返回当前属性本地化字符串。</returns>
        public virtual string GetString<TResource>(Expression<Func<TResource, object>> expression) => ResourceManager.GetString(expression);

        /// <summary>
        /// 获取当前表达式类型属性得资源字符串。
        /// </summary>
        /// <typeparam name="TResource">当前属性所在得类型实例。</typeparam>
        /// <param name="expression">表达式。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前属性本地化字符串。</returns>
        public virtual string GetString<TResource>(Expression<Func<TResource, object>> expression, params object[] args) => ResourceManager.GetString(expression, args);

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="type">资源所在程序集的类型。</param>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(Type type, string key) => ResourceManager.GetString(type, key);

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="type">资源所在程序集的类型。</param>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(Type type, string key, params object[] args) => ResourceManager.GetString(type, key, args);

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string this[Enum key] => GetString(key);

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string this[Enum key, params object[] args] => GetString(key, args);

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(string key) => ResourceManager.GetString(key);

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(string key, params object[] args) => ResourceManager.GetString(key, args);

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string this[string key] => GetString(key);

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string this[string key, params object[] args] => GetString(key, args);
    }
}