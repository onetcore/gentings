using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Gentings.AspNetCore
{
    /// <summary>
    /// 状态消息，主要用于页面提示信息。
    /// </summary>
    public class StatusMessage
    {
        private readonly ITempDataDictionary _tempData;
        private const string StatusMessageKey = "Status.Message";
        private const string StatusCodeKey = "Status.Code";
        /// <summary>
        /// 初始化类<see cref="StatusMessage"/>。
        /// </summary>
        /// <param name="tempData">临时数据存储实例。</param>
        public StatusMessage(ITempDataDictionary tempData)
        {
            _tempData = tempData;
        }

        private int? _code;
        /// <summary>
        /// 消息类型。
        /// </summary>
        public int Code
        {
            get
            {
                if (_code == null)
                {
                    if (_tempData.TryGetValue(StatusCodeKey, out var code))
                        _code = (int)code;
                    else
                        _code = 0;
                }
                return _code.Value;
            }
            set
            {
                _tempData[StatusCodeKey] = value;
                _code = value;
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

        /// <summary>
        /// 重新初始化实例。
        /// </summary>
        /// <param name="code">错误代码。</param>
        /// <param name="message">消息实例。</param>
        public void Reinitialize(int code, string message)
        {
            _code = code;
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