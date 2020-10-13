using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions.SMS.Properties;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 短信管理类型。
    /// </summary>
    public class SmsManager : ObjectManager<SmsMessage>, ISmsManager
    {
        private readonly ConcurrentDictionary<string, ISmsClient> _clients;

        /// <summary>
        /// 初始化类<see cref="SmsManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="clients">客户端列表。</param>
        public SmsManager(IDbContext<SmsMessage> context, IEnumerable<ISmsClient> clients) : base(context)
        {
            _clients = new ConcurrentDictionary<string, ISmsClient>(clients.ToDictionary(x => x.Name), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 保存短信。
        /// </summary>
        /// <param name="client">客户端发送器名称<see cref="ISmsClient.Name"/>。</param>
        /// <param name="phoneNumbers">电话号码，多个电话号码使用“,”分开。</param>
        /// <param name="message">电子邮件字符串。</param>
        /// <returns>返回保存结果。</returns>
        public virtual DataResult Save(string client, string phoneNumbers, string message)
        {
            if (!_clients.TryGetValue(client, out var smsClient))
            {
                return Resources.SMSClientNotFound;
            }

            var msg = new SmsMessage();
            msg.Client = client;
            msg.Message = message;
            msg.Count = message.GetSmsCount();
            var numbers = phoneNumbers
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct()
                .ToList();
            return DataResult.FromResult(Context.BeginTransaction(db =>
            {
                foreach (var number in numbers)
                {
                    msg.Id = 0;
                    msg.PhoneNumber = number;
                    msg.ServiceType = number.GetServiceType();
                    var prev = db.AsQueryable()
                        .Where(x => x.HashKey == msg.HashKey)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefault();
                    if (prev != null && smsClient.IsDuplicated(msg, prev))
                    {
                        continue;
                    }

                    db.Create(msg);
                }

                return true;
            }), DataAction.Created);
        }

        /// <summary>
        /// 保存短信。
        /// </summary>
        /// <param name="client">客户端发送器名称<see cref="ISmsClient.Name"/>。</param>
        /// <param name="phoneNumbers">电话号码，多个电话号码使用“,”分开。</param>
        /// <param name="message">电子邮件字符串。</param>
        /// <returns>返回保存结果。</returns>
        public virtual async Task<DataResult> SaveAsync(string client, string phoneNumbers, string message)
        {
            if (!_clients.TryGetValue(client, out var smsClient))
            {
                return Resources.SMSClientNotFound;
            }

            var msg = new SmsMessage();
            msg.Client = client;
            msg.Message = message;
            msg.Count = message.GetSmsCount();
            var numbers = phoneNumbers
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Distinct()
                .ToList();
            return DataResult.FromResult(await Context.BeginTransactionAsync(async db =>
            {
                foreach (var number in numbers)
                {
                    msg.Id = 0;
                    msg.PhoneNumber = number;
                    msg.ServiceType = number.GetServiceType();
                    var prev = await db.AsQueryable()
                        .Where(x => x.HashKey == msg.HashKey)
                        .OrderByDescending(x => x.CreatedDate)
                        .FirstOrDefaultAsync();
                    if (prev != null && smsClient.IsDuplicated(msg, prev))
                    {
                        continue;
                    }

                    await db.CreateAsync(msg);
                }

                return true;
            }), DataAction.Created);
        }

        /// <summary>
        /// 发送并保存短信。
        /// </summary>
        /// <param name="message">短信实例对象。</param>
        /// <returns>返回发送结果。</returns>
        public virtual async Task<SmsResult> SendAsync(SmsMessage message)
        {
            if (!_clients.TryGetValue(message.Client, out var client))
            {
                return false;
            }

            var result = await client.SendAsync(message);
            if (result.Status == SmsStatus.Failured)
            {
                message.TryTimes++;
                if (message.TryTimes >= SmsSettings.MaxTimes)
                {
                    message.Status = SmsStatus.Failured;
                }

                message.MsgId = result.MsgId;
                await SaveAsync(message);
                return false;
            }

            message.Status = SmsStatus.Completed;
            message.SentDate = DateTimeOffset.Now;
            message.TryTimes = 0;
            await SaveAsync(message);
            return true;
        }

        /// <summary>
        /// 发送并保存短信。
        /// </summary>
        /// <param name="client">客户端发送器名称<see cref="ISmsClient.Name"/>。</param>
        /// <param name="phoneNumber">电话号码。</param>
        /// <param name="message">电子邮件字符串。</param>
        /// <returns>返回发送结果。</returns>
        public Task<SmsResult> SendAsync(string client, string phoneNumber, string message)
        {
            var msg = new SmsMessage();
            msg.Client = client;
            msg.PhoneNumber = phoneNumber;
            msg.ServiceType = phoneNumber.GetServiceType();
            msg.Message = message;
            msg.Count = message.GetSmsCount();
            return SendAsync(msg);
        }

        /// <summary>
        /// 加载未发送的短信列表。
        /// </summary>
        /// <param name="size">加载数量。</param>
        /// <returns>未发送的短信列表。</returns>
        public virtual Task<IEnumerable<SmsMessage>> LoadAsync(int size)
        {
            return Context.AsQueryable()
                .WithNolock()
                .OrderBy(x => x.Id)
                .Where(x => x.Status == SmsStatus.Pending)
                .AsEnumerableAsync(size);
        }
    }
}