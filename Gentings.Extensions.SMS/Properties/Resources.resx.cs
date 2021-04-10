// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.Extensions.SMS.Properties
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
        /// 三网通
        /// </summary>
        internal static string ServiceType_All => GetString("ServiceType_All");

        /// <summary>
        /// 移动
        /// </summary>
        internal static string ServiceType_Mobile => GetString("ServiceType_Mobile");

        /// <summary>
        /// 未知
        /// </summary>
        internal static string ServiceType_None => GetString("ServiceType_None");

        /// <summary>
        /// 电信
        /// </summary>
        internal static string ServiceType_Telecom => GetString("ServiceType_Telecom");

        /// <summary>
        /// 联通
        /// </summary>
        internal static string ServiceType_Unicom => GetString("ServiceType_Unicom");

        /// <summary>
        /// 短信发送客户端不存在！
        /// </summary>
        internal static string SMSClientNotFound => GetString("SMSClientNotFound");

        /// <summary>
        /// 成功发送
        /// </summary>
        internal static string SmsStatus_Completed => GetString("SmsStatus_Completed");

        /// <summary>
        /// 发送失败
        /// </summary>
        internal static string SmsStatus_Failured => GetString("SmsStatus_Failured");

        /// <summary>
        /// 等待发送
        /// </summary>
        internal static string SmsStatus_Pending => GetString("SmsStatus_Pending");

        /// <summary>
        /// 发送中
        /// </summary>
        internal static string SmsStatus_Sending => GetString("SmsStatus_Sending");

        /// <summary>
        /// 短信发送服务
        /// </summary>
        internal static string SMSTaskService => GetString("SMSTaskService");

        /// <summary>
        /// 发送短信相关服务
        /// </summary>
        internal static string SMSTaskService_Description => GetString("SMSTaskService_Description");
    }
}

