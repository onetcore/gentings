using Gentings.Messages.CMPP.Packaging;
using Gentings.Messages.CMPP.Packaging;
using Gentings.Messages.CMPP.ServiceProviders;
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gentings.Messages.CMPP.Services
{
    /// <summary>
    /// 流服务。
    /// </summary>
    public class ServiceContext
    {
        private readonly NetworkStream _networkStream;
        private readonly ICMPPServiceProvider _configuration;
        private DateTime _lastWrited = DateTime.Now;

        internal ServiceContext(NetworkStream networkStream, ICMPPServiceProvider configuration)
        {
            _networkStream = networkStream;
            _configuration = configuration;
        }

        /// <summary>
        /// 发送检测请求。
        /// </summary>
        public async Task SendPingAsync()
        {
            if (DateTime.Now.AddMinutes(-3) >= _lastWrited)
                await SendAsync(CMPPCommand.CMPP_ACTIVE_TEST, CMPPCommand.CMPP_ACTIVE_TEST_RESP);
        }

        /// <summary>
        /// 发送短信。
        /// </summary>
        /// <param name="msg">短信信息，长度小于140。</param>
        /// <param name="phoneNumbers">电话号码，最多100个。</param>
        /// <returns>返回提交结果。</returns>
        public async Task<SubmitMessage> SubmitAsync(string msg, params string[] phoneNumbers)
        {
            var package = new SubmitPackage(MsgFormat.BG2312, msg, phoneNumbers);
            package.PkTotal = 1;
            package.RegisteredDelivery = RegisteredDelivery.Yes;
            package.MsgLevel = 1;
            package.ServiceId = "abcd123456";
            package.FeeUserType = FeeUserType.SP;
            package.FeeTerminalType = TerminalType.PseudoCode;
            package.MsgSrc = _configuration.Spid;
            return await SendMessageAsync<SubmitMessage>(package);
        }

        /// <summary>
        /// 发送取消短信包。
        /// </summary>
        /// <param name="msgId">消息Id。</param>
        /// <returns>返回发送结果。</returns>
        public async Task<bool> SendCancelAsync(uint msgId)
        {
            var package = new CancelPackage();
            package.MsgId = msgId;
            var message = await SendMessageAsync<CancelMessage>(package);
            return message?.SuccessId == 0;
        }

        /// <summary>
        /// 发送数据包，并返回消息实例。
        /// </summary>
        /// <typeparam name="TMessage">返回的消息类型。</typeparam>
        /// <param name="package">包实例。</param>
        /// <returns>返回消息实例。</returns>
        public async Task<TMessage> SendMessageAsync<TMessage>(IPackage package)
            where TMessage : class, IMessage
        {
            await _networkStream.WritePackageAsync(package);
            _lastWrited = DateTime.Now;
            return await _networkStream.ReadMessageAsync<TMessage>();
        }

        /// <summary>
        /// 通知服务端终止连接。
        /// </summary>
        /// <returns>返回发送结果。</returns>
        public Task<bool> SendTerminalAsync() => SendAsync(CMPPCommand.CMPP_TERMINATE, CMPPCommand.CMPP_TERMINATE_RESP);

        /// <summary>
        /// 发送没有消息体的包。
        /// </summary>
        /// <param name="requestCommand">发送包操作码。</param>
        /// <param name="receiveCommand">返回包操作码。</param>
        /// <returns>判断是否发送成功。</returns>
        public async Task<bool> SendAsync(CMPPCommand requestCommand, CMPPCommand receiveCommand)
        {
            var package = new PackageHeader(PackageHeader.Size, requestCommand, 1);
            await _networkStream.WriteBytesAsync(package.ToBytes());
            _lastWrited = DateTime.Now;
            var buffer = await _networkStream.ReadBytesAsync();
            if (buffer == null) return false;
            var message = new PackageHeader(buffer);
            return message.CommandId == receiveCommand && message.SequenceId == package.SequenceId;
        }

        /// <summary>
        /// 判断数据是否可以读取。
        /// </summary>
        public bool DataAvailable => _networkStream.DataAvailable;

        /// <summary>
        /// 读取当前可读数据。
        /// </summary>
        /// <returns>返回当前可读数据。</returns>
        public Task<byte[]> ReadBytesAsync() => _networkStream.ReadBytesAsync();
    }
}
