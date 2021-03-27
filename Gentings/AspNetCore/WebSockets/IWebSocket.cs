using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Gentings.Extensions;
using Microsoft.AspNetCore.Http;

namespace Gentings.AspNetCore.WebSockets
{
    /// <summary>
    /// WebSocket接口。
    /// </summary>
    public interface IWebSocket
    {
        /// <summary>
        /// 当前会话Id。
        /// </summary>
        string Sid { get; }

        /// <summary>
        /// HTTP请求上下文。
        /// </summary>
        HttpContext HttpContext { get; }

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="data">数据实例。</param>
        /// <returns>返回发送任务实例。</returns>
        Task SendAsync(string method, object data = null);

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="errorCode">错误码。</param>
        /// <param name="message">错误消息。</param>
        /// <returns>返回发送任务实例。</returns>
        Task SendAsync(string method, Enum errorCode, string message);

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="errorCode">错误码。</param>
        /// <param name="message">错误消息。</param>
        /// <returns>返回发送任务实例。</returns>
        Task SendAsync(string method, int errorCode, string message);

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="data">对象实例。</param>
        /// <returns>返回发送任务实例。</returns>
        Task SendDataAsync<TData>(string method, TData data);

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="method">操作方法。</param>
        /// <param name="data">对象实例。</param>
        /// <returns>返回发送任务实例。</returns>
        Task SendPageAsync<TData>(string method, TData data)
            where TData : IPageEnumerable<TData>;

        /// <summary>
        /// 关闭连接。
        /// </summary>
        /// <returns>关闭连接任务。</returns>
        Task CloseAsync();

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <returns>返回执行的任务。</returns>
        Task InvokeAsync();

        /// <summary>
        /// 所有WebSocket实例。
        /// </summary>
        ConcurrentDictionary<string, IWebSocket> WebSockets { get; }
    }
}