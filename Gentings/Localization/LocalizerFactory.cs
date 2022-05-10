using System.Linq.Expressions;

namespace Gentings.Localization
{
    /// <summary>
    /// 本地化资源工厂接口。
    /// </summary>
    public class LocalizerFactory : ILocalizerFactory
    {
        private readonly ILocalizer _localizer;
        /// <summary>
        /// 初始化类<see cref="LocalizerFactory"/>。
        /// </summary>
        /// <param name="localizer">本地化资源接口实例。</param>
        public LocalizerFactory(ILocalizer localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 新建一个本地化实例。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回本地化资源实例。</returns>
        public virtual ILocalizer CreateLocalizer(Type type)
        {
            return new TypedLocalizer(_localizer, type);
        }

        private class TypedLocalizer : ILocalizer
        {
            private readonly ILocalizer _localizer;
            private readonly Type _type;

            public TypedLocalizer(ILocalizer localizer, Type type)
            {
                _localizer = localizer;
                _type = type;
            }

            public string this[Enum key] => GetString(key);
            public string this[string key] => GetString(key);
            public string this[Enum key, params object[] args] => GetString(key, args);
            public string this[string key, params object[] args] => GetString(key, args);

            public string GetString(Enum key)
            {
                return _localizer.GetString(key);
            }

            public string GetString(Enum key, params object[] args)
            {
                return _localizer.GetString(key, args);
            }

            public string GetString<TResource>(string key)
            {
                return _localizer.GetString<TResource>(key);
            }

            public string GetString<TResource>(string key, params object[] args)
            {
                return _localizer.GetString<TResource>(key, args);
            }

            public string? GetString<TResource>(Expression<Func<TResource, object?>> expression)
            {
                return _localizer.GetString(expression);
            }

            public string? GetString<TResource>(Expression<Func<TResource, object?>> expression, params object[] args)
            {
                return _localizer.GetString(expression, args);
            }

            public string GetString(Type type, string key)
            {
                return _localizer.GetString(type, key);
            }

            public string GetString(Type type, string key, params object[] args)
            {
                return _localizer.GetString(type, key, args);
            }

            public string GetString(string key)
            {
                return GetString(_type, key);
            }

            public string GetString(string key, params object[] args)
            {
                return GetString(_type, key, args);
            }
        }
    }
}