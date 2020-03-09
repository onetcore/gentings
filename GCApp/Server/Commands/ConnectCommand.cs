using System.Threading.Tasks;
using System.Threading;
using Gentings.ConsoleApp;
using GCApp.Packaging;
using Gentings;
using System.Runtime.InteropServices;

namespace GCApp.Server.Commands
{
    /// <summary>
    /// 连接命令。
    /// </summary>
    public class ConnectCommand : ICommand
    {
        /// <summary>
        /// 操作码。
        /// </summary>
        public CMPPCommand CMPPCommand => CMPPCommand.CMPP_CONNECT;

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="session">当前用户会话实例。</param>
        /// <param name="header">包头。</param>
        /// <param name="body">包体。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回执行任务。</returns>
        public async Task ExecuteAsync(Session session, PackageHeader header, byte[] body, CancellationToken cancellationToken)
        {
            var size = Marshal.SizeOf<ConnectPackage>();
            var bytes = new byte[body.Length + 2];
            body.CopyTo(bytes, 0);
            var message = bytes.ToStruct<ConnectPackage>();
            Consoles.Success(message.ToJsonString());
        }
    }
}
