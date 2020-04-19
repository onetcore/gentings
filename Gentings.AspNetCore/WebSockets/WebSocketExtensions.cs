using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.AspNetCore.WebSockets
{
    /// <summary>
    /// 扩展方法。
    /// </summary>
    public static class WebSocketExtensions
    {
        /// <summary>
        /// 接收字符串。
        /// </summary>
        /// <param name="socket">当前WebSocket实例。</param>
        /// <param name="ct">取消标志。</param>
        /// <returns>返回当前请求的字符串。</returns>
        public static async Task<string> ReceiveStringAsync(this WebSocket socket, CancellationToken ct = default)
        {
            var buffer = new ArraySegment<byte>(new byte[WebSocketWrapper.BufferSize]);
            await using var ms = new MemoryStream();
            WebSocketReceiveResult result;
            do
            {
                ct.ThrowIfCancellationRequested();
                result = await socket.ReceiveAsync(buffer, ct);
                ms.Write(buffer.Array, buffer.Offset, result.Count);
            }
            while (!result.EndOfMessage);
            if (result.CloseStatus.HasValue)
                await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            ms.Seek(0, SeekOrigin.Begin);
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        /// <summary>
        /// 使用WebSocket处理实例。
        /// </summary>
        /// <param name="app">应用程序构建实例。</param>
        /// <returns>应用程序构建实例。</returns>
        public static IApplicationBuilder UseWebSocketHandler(this IApplicationBuilder app) =>
            app.Use(async (context, next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();
                    await context.RequestServices.GetRequiredService<IWebSocketManager>().InvokeAsync(webSocket, context);
                    return;
                }

                await next();
            });
    }
}