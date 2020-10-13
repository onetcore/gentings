using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.SMS
{
    /// <summary>
    /// 短信。
    /// </summary>
    [Table("core_SMS")]
    public class SmsMessage : IIdObject
    {
        /// <summary>
        /// 获取或设置唯一Id。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置短信内容。
        /// </summary>
        [Size(350)]
        public string Message { get; set; }

        /// <summary>
        /// 电话号码。
        /// </summary>
        [Size(20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 服务类型。
        /// </summary>
        public ServiceType ServiceType { get; set; }

        /// <summary>
        /// 短信计量。
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 尝试发送次数。
        /// </summary>
        public int TryTimes { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        public SmsStatus Status { get; set; }

        /// <summary>
        /// 发送返回的短信Id。
        /// </summary>
        [Size(64)]
        public string MsgId { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 发送时间。
        /// </summary>
        public DateTimeOffset? SentDate { get; set; }

        /// <summary>
        /// 发送器名称。
        /// </summary>
        [Size(36)]
        public string Client { get; set; }

        /// <summary>
        /// 下发短信时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset? DeliveredDate { get; set; }

        /// <summary>
        /// 下发短信状态。
        /// </summary>
        [NotUpdated]
        public string DeliveredStatus { get; set; }

        /// <summary>
        /// 模板Id。
        /// </summary>
        public int TemplateId { get; set; }

        /// <summary>
        /// 模板参数。
        /// </summary>
        [Size(512)]
        public string TemplateParameters { get; set; }

        /// <summary>
        /// 参数列表。
        /// </summary>
        [NotMapped]
        public IDictionary<string, string> Parameters
        {
            get => Cores.FromJsonString<Dictionary<string, string>>(TemplateParameters);
            set => TemplateParameters = value.ToJsonString();
        }

        private string _hashkey;
        /// <summary>
        /// 唯一键验证。
        /// </summary>
        [NotUpdated]
        [Size(32)]
        public string HashKey
        {
            get => _hashkey ??= Cores.Md5($"{PhoneNumber}::{Message}::{TemplateId}::{TemplateParameters}");
            set => _hashkey = value;
        }
    }
}