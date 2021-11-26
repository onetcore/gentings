using System;

namespace Gentings.Localization
{
    /// <summary>
    /// 本地化资源工厂接口。
    /// </summary>
    public interface ILocalizerFactory : ISingletonService
    {
        /// <summary>
        /// 新建一个本地化实例。
        /// </summary>
        /// <param name="type">当前类型。</param>
        /// <returns>返回本地化资源实例。</returns>
        ILocalizer CreateLocalizer(Type type);
    }
}