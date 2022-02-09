using System.Collections.Concurrent;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;

namespace Gentings.AspNetCore.WebSockets
{
    /// <summary>
    /// WebSocket管理实现类。
    /// </summary>
    public class WebSocketManager : IWebSocketManager
    {
        private static readonly ConcurrentDictionary<string, IWebSocket> _sockets = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 添加<see cref="IWebSocket"/>实例。
        /// </summary>
        /// <param name="socket">WebSocket实例。</param>
        /// <param name="context">当前请求上下文。</param>
        /// <returns>返回当前<see cref="IWebSocket"/>实例。</returns>
        /// <returns>当前执行任务实例。</returns>
        public async Task InvokeAsync(WebSocket socket, HttpContext context)
        {
            var current = new WebSocketWrapper(socket, context);
            current.WebSockets = _sockets;
            _sockets.AddOrUpdate(current.Sid, _ => current, (k, _) => current);
            await current.InvokeAsync();
            if (socket.State == WebSocketState.Closed || socket.State == WebSocketState.CloseReceived)
                _sockets.TryRemove(current.Sid, out _);
        }
    }
}