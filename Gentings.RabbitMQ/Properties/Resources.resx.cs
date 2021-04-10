// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace Gentings.RabbitMQ.Properties
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
        /// 发布失败
        /// </summary>
        internal static string ErrorCode_Failure => GetString("ErrorCode_Failure");

        /// <summary>
        /// 发送实体不能为空
        /// </summary>
        internal static string ErrorCode_NullBody => GetString("ErrorCode_NullBody");

        /// <summary>
        /// 成功
        /// </summary>
        internal static string ErrorCode_Success => GetString("ErrorCode_Success");
    }
}

