using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Gentings.AspNetCore.WebSockets
{
    /// <summary>
    /// WebSocket基类。
    /// </summary>
    public sealed class WebSocketWrapper : IWebSocket
    {
        /// <summary>
        /// HTTP请求上下文。
        /// </summary>
        public HttpContext HttpContext { get; }
        private readonly WebSocket _socket;
        private readonly CancellationToken _cancellationToken;
        private readonly ConcurrentDictionary<string, IWebSocketHandler> _handlers;

        /// <summary>
        /// 当前会话Id。
        /// </summary>
        public string Sid { get; }

        /// <summary>
        /// 缓存大小。
        /// </summary>
        public const int BufferSize = 1024 * 4;

        /// <summary>
        /// 操作方法分隔符。
        /// </summary>
        public const char Header = ':';

        /// <summary>
        /// 初始化类<see cref="WebSocketWrapper"/>。
        /// </summary>
        /// <param name="socket">WebSocket实例。</param>
        /// <param name="context">HTTP上下文。</param>
        public WebSocketWrapper(WebSocket socket, HttpContext context)
        {
            HttpContext = context;
            _socket = socket;
            _cancellationToken = context.RequestAborted;
            Sid = context.Request.Query["sid"];
            _handlers = new ConcurrentDictionary<string, IWebSocketHandler>(context.RequestServices.GetServices<IWebSocketHandler>().ToDictionary(x => x.Method), StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="data">数据实例。</param>
        /// <returns>返回发送任务实例。</returns>
        public Task SendAsync(string method, object? data = null)
        {
            if (data != null)
                method += Header + data.ToJsonString();
            var buffer = Encoding.UTF8.GetBytes(method);
            return _socket.SendAsync(buffer, WebSocketMessageType.Text, true, _cancellationToken);
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="errorCode">错误码。</param>
        /// <param name="message">错误消息。</param>
        /// <returns>返回发送任务实例。</returns>
        public Task SendAsync(string method, Enum errorCode, string message)
        {
            return SendAsync(method, (int)(object)errorCode, message);
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="errorCode">错误码。</param>
        /// <param name="message">错误消息。</param>
        /// <returns>返回发送任务实例。</returns>
        public Task SendAsync(string method, int errorCode, string message)
        {
            return SendAsync(method, new ApiResult { Code = errorCode, Message = message });
        }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="data">对象实例。</param>
        /// <returns>返回发送任务实例。</returns>
        public Task SendDataAsync<TData>(string method, TData data)
        {
            return SendAsync(method, new ApiDataResult(data));
        }

        /// <summary>
        /// 关闭连接。
        /// </summary>
        /// <returns>关闭连接任务。</returns>
        public Task CloseAsync()
        {
            return _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", _cancellationToken);
        }

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <returns>返回执行的任务。</returns>
        public async Task InvokeAsync()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var json = await _socket.ReceiveStringAsync(_cancellationToken);
                if (string.IsNullOrEmpty(json))
                {
                    await Task.Delay(TimeSpan.FromTicks(1), _cancellationToken);
                    continue;
                }

                var name = json;
                var index = json.IndexOf(Header);
                if (index != -1)
                {
                    name = name[..index];
                    json = json[(index + 1)..];
                }
                else
                    json = null;

                if (_handlers.TryGetValue(name, out var handler))
                {
                    try
                    {
                        await handler.ExecuteAsync(this, json);
                    }
                    catch (Exception exception)
                    {
                        await SendAsync(name, -1, exception.Message);
                    }
                }
                await Task.Delay(TimeSpan.FromTicks(1), _cancellationToken);
            }

            await CloseAsync();
        }

        /// <summary>
        /// 所有WebSocket实例。
        /// </summary>
        public ConcurrentDictionary<string, IWebSocket>? WebSockets { get; internal set; }
    }
}