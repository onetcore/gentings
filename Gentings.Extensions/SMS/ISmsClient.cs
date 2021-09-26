using System.Threading.Tasks;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 客户端发送器接口。
    /// </summary>
    public interface ISmsClient : ISingletonServices
    {
        /// <summary>
        /// 发送名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 发送短信。
        /// </summary>
        /// <param name="message">短信实例。</param>
        /// <returns>发送结果。</returns>
        Task<SmsResult> SendAsync(SmsMessage message);

        /// <summary>
        /// 判断是否重复发送短信。
        /// </summary>
        /// <param name="current">当前短信实例。</param>
        /// <param name="prev">上一次短信实例。</param>
        /// <returns>返回判断结果。</returns>
        bool IsDuplicated(SmsMessage current, SmsMessage prev);
    }
}