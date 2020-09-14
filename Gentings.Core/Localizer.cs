﻿using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;

namespace Gentings
{
    /// <summary>
    /// 本地化资源实现类。
    /// </summary>
    public class Localizer : ILocalizer
    {
        private readonly ConcurrentDictionary<Type, ResourceManager> _localizers =
            new ConcurrentDictionary<Type, ResourceManager>();

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(Enum key)
        {
            var type = key.GetType();
            var name = $"{type.Name}_{key}";
            var resource = GetString(type, name);
            if (resource == name)
                resource = key.ToStr();
            return resource;
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(Enum key, params object[] args)
        {
            var type = key.GetType();
            var name = $"{type.Name}_{key}";
            var resource = GetString(type, name);
            if (resource == name)
                resource = key.ToStr();
            return string.Format(resource, args);
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <typeparam name="TResource">包含当前资源包的所在程序集的类型。</typeparam>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString<TResource>(string key)
        {
            return GetString(typeof(TResource), key);
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <typeparam name="TResource">包含当前资源包的所在程序集的类型。</typeparam>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString<TResource>(string key, params object[] args)
        {
            var resource = GetString(typeof(TResource), key);
            if (resource == null)
            {
                return key;
            }

            return string.Format(resource, args);
        }

        /// <summary>
        /// 获取当前表达式类型属性得资源字符串。
        /// </summary>
        /// <typeparam name="TResource">当前属性所在得类型实例。</typeparam>
        /// <param name="expression">表达式。</param>
        /// <returns>返回当前属性本地化字符串。</returns>
        public virtual string GetString<TResource>(Expression<Func<TResource, object>> expression)
        {
            var member = expression.GetPropertyAccess();
            if (member == null)
            {
                return null;
            }

            return GetString(member.DeclaringType, member.Name);
        }

        /// <summary>
        /// 获取当前表达式类型属性得资源字符串。
        /// </summary>
        /// <typeparam name="TResource">当前属性所在得类型实例。</typeparam>
        /// <param name="expression">表达式。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前属性本地化字符串。</returns>
        public virtual string GetString<TResource>(Expression<Func<TResource, object>> expression, params object[] args)
        {
            var resource = GetString(expression);
            if (resource == null)
            {
                return null;
            }

            return string.Format(resource, args);
        }

        private const string Resources = ".Resources.resources";

        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="type">资源所在程序集的类型。</param>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(Type type, string key)
        {
            var resourceManager = _localizers.GetOrAdd(type, t =>
            {
                Assembly assembly;
                if (t == NullLocalizer.InstanceType)
                {
                    assembly = Assembly.GetEntryAssembly();
                }
                else
                {
                    assembly = t.Assembly;
                }

                var baseName = assembly.GetManifestResourceNames()
                    .SingleOrDefault(x => x.EndsWith(Resources));
                if (baseName == null)
                {
                    return null;
                }

                baseName = baseName[0..^10];
                return new ResourceManager(baseName, assembly);
            });
            return resourceManager?.GetString(key) ?? key;
        }

        private class NullLocalizer
        {
            public static readonly Type InstanceType = typeof(NullLocalizer);
        }

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(string key) => GetString(NullLocalizer.InstanceType, key);

        /// <summary>
        /// 获取当前键的本地化字符串实例（网站程序集）。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <param name="args">格式化参数。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public virtual string GetString(string key, params object[] args)
        {
            var resource = GetString(key);
            if (resource == null)
            {
                return key;
            }

            return string.Format(resource, args);
        }
    }
}