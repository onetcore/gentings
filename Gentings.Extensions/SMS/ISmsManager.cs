using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 短信管理接口。
    /// </summary>
    public interface ISmsManager : IObjectManager<SmsMessage>
    {
        /// <summary>
        /// 保存短信。
        /// </summary>
        /// <param name="client">客户端发送器名称<see cref="ISmsClient.Name"/>。</param>
        /// <param name="phoneNumbers">电话号码，多个电话号码使用“,”分开。</param>
        /// <param name="message">电子邮件字符串。</param>
        /// <returns>返回保存结果。</returns>
        DataResult Save(string client, string phoneNumbers, string message);

        /// <summary>
        /// 保存短信。
        /// </summary>
        /// <param name="client">客户端发送器名称<see cref="ISmsClient.Name"/>。</param>
        /// <param name="phoneNumbers">电话号码，多个电话号码使用“,”分开。</param>
        /// <param name="message">电子邮件字符串。</param>
        /// <returns>返回保存结果。</returns>
        Task<DataResult> SaveAsync(string client, string phoneNumbers, string message);

        /// <summary>
        /// 发送并保存短信。
        /// </summary>
        /// <param name="message">短信实例对象。</param>
        /// <returns>返回发送结果。</returns>
        Task<SmsResult> SendAsync(SmsMessage message);

        /// <summary>
        /// 发送并保存短信。
        /// </summary>
        /// <param name="client">客户端发送器名称<see cref="ISmsClient.Name"/>。</param>
        /// <param name="phoneNumber">电话号码。</param>
        /// <param name="message">电子邮件字符串。</param>
        /// <returns>返回发送结果。</returns>
        Task<SmsResult> SendAsync(string client, string phoneNumber, string message);

        /// <summary>
        /// 加载未发送的短信列表。
        /// </summary>
        /// <param name="size">加载数量。</param>
        /// <returns>未发送的短信列表。</returns>
        Task<IEnumerable<SmsMessage>> LoadAsync(int size);
    }
}