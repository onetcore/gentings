using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Gentings.AspNetCore.WebSockets
{
    /// <summary>
    /// WebSocket管理接口。
    /// </summary>
    public interface IWebSocketManager : ISingletonService
    {
        /// <summary>
        /// 添加<see cref="IWebSocket"/>实例。
        /// </summary>
        /// <param name="socket">WebSocket实例。</param>
        /// <param name="context">当前请求上下文。</param>
        /// <returns>返回当前<see cref="IWebSocket"/>实例。</returns>
        /// <returns>当前执行任务实例。</returns>
        Task InvokeAsync(WebSocket socket, HttpContext context);
    }
}