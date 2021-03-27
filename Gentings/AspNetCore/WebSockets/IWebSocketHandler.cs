using System;
using System.Threading.Tasks;

namespace Gentings.AspNetCore.WebSockets
{
    /// <summary>
    /// WebSocket处理接口。
    /// </summary>
    public interface IWebSocketHandler : IServices
    {
        /// <summary>
        /// 处理方法唯一键。
        /// </summary>
        string Method { get; }

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="socket">当前Socket实例。</param>
        /// <param name="data">获取数据。</param>
        /// <returns>返回执行任务实例。</returns>
        Task ExecuteAsync(IWebSocket socket, string data);
    }

#if DEBUG
    /// <summary>
    /// Hello。
    /// </summary>
    public class HelloWebSocketHandler : IWebSocketHandler
    {
        /// <summary>
        /// 处理方法唯一键。
        /// </summary>
        public string Method { get; } = "hello";

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="socket">当前Socket实例。</param>
        /// <param name="data">获取数据。</param>
        /// <returns>返回执行任务实例。</returns>
        public Task ExecuteAsync(IWebSocket socket, string data)
        {
            return socket.SendDataAsync(Method, $"[{DateTime.Now:HH:mm:ss}] Hi, I had received {data}");
        }
    }
#endif
}