// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.Extensions.Emails.Properties
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
        /// 成功发送
        /// </summary>
        internal static string EmailStatus_Completed => GetString("EmailStatus_Completed");

        /// <summary>
        /// 发送失败
        /// </summary>
        internal static string EmailStatus_Failured => GetString("EmailStatus_Failured");

        /// <summary>
        /// 等待发送
        /// </summary>
        internal static string EmailStatus_Pending => GetString("EmailStatus_Pending");

        /// <summary>
        /// 电子邮件发送服务
        /// </summary>
        internal static string EmailTaskService => GetString("EmailTaskService");

        /// <summary>
        /// 发送队列在数据库中的电子邮件
        /// </summary>
        internal static string EmailTaskService_Description => GetString("EmailTaskService_Description");
    }
}

