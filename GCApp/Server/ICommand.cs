using System.Threading.Tasks;
using System.Threading;
using Gentings;

namespace GCApp.Server
{
    /// <summary>
    /// 命令接口。
    /// </summary>
    public interface ICommand : IServices
    {
        /// <summary>
        /// 操作码。
        /// </summary>
        CMPPCommand CMPPCommand { get; }

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="session">当前用户会话实例。</param>
        /// <param name="header">包头。</param>
        /// <param name="body">包体。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回执行任务。</returns>
        Task ExecuteAsync(Session session, PackageHeader header, byte[] body, CancellationToken cancellationToken);
    }
}
