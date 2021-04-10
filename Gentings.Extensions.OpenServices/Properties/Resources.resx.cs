// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.Extensions.OpenServices.Properties
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
        /// 禁用
        /// </summary>
        internal static string ApplicationStatus_Disabled => GetString("ApplicationStatus_Disabled");

        /// <summary>
        /// 过期
        /// </summary>
        internal static string ApplicationStatus_Expired => GetString("ApplicationStatus_Expired");

        /// <summary>
        /// 正常
        /// </summary>
        internal static string ApplicationStatus_Normal => GetString("ApplicationStatus_Normal");

        /// <summary>
        /// 等待验证
        /// </summary>
        internal static string ApplicationStatus_Pending => GetString("ApplicationStatus_Pending");

        /// <summary>
        /// 验证失败
        /// </summary>
        internal static string ApplicationStatus_Unapproved => GetString("ApplicationStatus_Unapproved");

        /// <summary>
        /// AppId不能为空！
        /// </summary>
        internal static string TokenModel_AppIdNull => GetString("TokenModel_AppIdNull");

        /// <summary>
        /// 密钥不能为空！
        /// </summary>
        internal static string TokenModel_AppSecretNull => GetString("TokenModel_AppSecretNull");
    }
}

