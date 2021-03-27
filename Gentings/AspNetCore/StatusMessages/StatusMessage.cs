﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gentings.AspNetCore.StatusMessages
{
    /// <summary>
    /// 消息类型。
    /// </summary>
    public class StatusMessage
    {
        private readonly ITempDataDictionary _tempData;
        private const string StatusMessageKey = "StatusMessage";
        private const string StatusTypeKey = "StatusType";
        private const string StatusUrlKey = "StatusUrl";
        /// <summary>
        /// 初始化类<see cref="StatusMessage"/>。
        /// </summary>
        /// <param name="tempData">临时数据存储实例。</param>
        public StatusMessage(ITempDataDictionary tempData)
        {
            _tempData = tempData;
        }

        private StatusType? _statusType;
        /// <summary>
        /// 消息类型。
        /// </summary>
        public StatusType Type
        {
            get
            {
                if (_statusType == null)
                {
                    if (_tempData.TryGetValue(StatusTypeKey, out var status))
                        _statusType = (StatusType)status;
                    else
                        _statusType = StatusType.Danger;
                }
                return _statusType.Value;
            }
            set
            {
                _tempData[StatusTypeKey] = value;
                _statusType = value;
            }
        }

        private string _message;
        /// <summary>
        /// 消息。
        /// </summary>
        public string Message
        {
            get => _message ??= _tempData[StatusMessageKey] as string;
            set
            {
                _tempData[StatusMessageKey] = value;
                _message = value;
            }
        }

        private string _url;
        /// <summary>
        /// 转向地址。
        /// </summary>
        public string Url
        {
            get => _url ??= _tempData[StatusUrlKey] as string;
            set
            {
                _tempData[StatusUrlKey] = value;
                _url = value;
            }
        }

        /// <summary>
        /// 重新初始化实例。
        /// </summary>
        /// <param name="type">类型状态。</param>
        /// <param name="message">消息实例。</param>
        public void Reinitialize(StatusType type, string message)
        {
            _statusType = type;
            _message = message;
        }

        /// <summary>
        /// 状态消息。
        /// </summary>
        /// <returns>返回状态消息。</returns>
        public override string ToString()
        {
            return _tempData.Peek(StatusMessageKey) as string;
        }
    }
}