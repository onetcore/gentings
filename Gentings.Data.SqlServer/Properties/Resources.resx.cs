// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.Data.SqlServer.Properties
{
    using System;
    using Gentings.Localization;

    /// <summary>
    /// 读取资源文件。
    /// </summary>
    internal class Resources
    {
        /// <summary>
        /// 获取当前键的本地化字符串实例。
        /// </summary>
        /// <param name="key">资源键。</param>
        /// <returns>返回当前本地化字符串。</returns>
        public static string GetString(string key)
        {
            return ResourceManager.GetString(typeof(Resources), key);
        }

        /// <summary>
        /// “TimestampAttribute”特性属性的数据类型必须为byte[]。
        /// </summary>
        internal static string TypeMustBeBytes => GetString("TypeMustBeBytes");

        /// <summary>
        /// 数据类型'{0}'暂时还不支持。
        /// </summary>
        internal static string UnsupportedType => GetString("UnsupportedType");
    }
}

