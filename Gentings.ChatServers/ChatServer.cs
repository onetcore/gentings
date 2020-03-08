using Gentings.ChatServers.Properties;
using Gentings.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Gentings.ChatServers
{
    /// <summary>
    /// 聊天服务器。
    /// </summary>
    [Authorize]
    public class ChatServer : Hub
    {
        private readonly IUserManager _userManager;
        private readonly IMessageManager _messageManager;

        /// <summary>
        /// 初始化类<see cref="ChatServer"/>。
        /// </summary>
        /// <param name="userManager">用户管理接口实例。</param>
        public ChatServer(IUserManager userManager, IMessageManager messageManager)
        {
            _userManager = userManager;
            _messageManager = messageManager;
        }

        /// <summary>
        /// 在线列表。
        /// </summary>
        private static ConcurrentDictionary<string, User> Onlines { get; } = new ConcurrentDictionary<string, User>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 在线列表。
        /// </summary>
        private static ConcurrentDictionary<int, string> Connections { get; } = new ConcurrentDictionary<int, string>();

        /// <summary>
        /// 用户连接时候执行的方法。
        /// </summary>
        /// <returns>连接执行任务。</returns>
        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext().User.GetUserId();
            if (userId == 0)
            {
                var user = await _userManager.FindAsync(userId);
                if (user == null)
                {
                    throw new UnauthorizedAccessException();
                }

                user.IsOnline = true;
                user.ConnectedDate = DateTimeOffset.Now;
                Onlines.AddOrUpdate(Context.ConnectionId, _ => user, (_, _1) => user);
                Connections.AddOrUpdate(userId, _ => Context.ConnectionId, (_, _1) => Context.ConnectionId);
                await _userManager.UpdateAsync(user);
            }
            await base.OnConnectedAsync();
        }

        /// <summary>
        /// 发送消息给对方。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="msg">消息字符串。</param>
        /// <returns>返回发送任务。</returns>
        public async Task SendMessageAsync(int userId, string msg)
        {
            var user = GetUser();
            var message = new Message();
            message.Sender = user.Id;
            message.Receiver = userId;
            message.Content = msg;
            await _messageManager.CreateAsync(message);
            if (Connections.TryGetValue(userId, out var connectionId))
            {
                await Clients.Clients(connectionId).SendAsync("msg", message);
            }
        }

        /// <summary>
        /// 获取当前用户实例。
        /// </summary>
        /// <returns>当前用户实例。</returns>
        protected User GetUser()
        {
            if (Onlines.TryGetValue(Context.ConnectionId, out var user))
            {
                return user;
            }

            throw new HubException(Resources.ClientDisconnected);
        }

        /// <summary>
        /// 发送消息给所有连接客户。
        /// </summary>
        /// <param name="method">方法名称。</param>
        /// <param name="args">发送信息列表。</param>
        /// <returns>发送任务。</returns>
        protected Task SendAllAsync(string method, params object[] args)
        {
            return Clients.All.SendAsync(method, args, default);
        }

        /// <summary>
        /// 发送消息给其他所有连接客户。
        /// </summary>
        /// <param name="method">方法名称。</param>
        /// <param name="args">发送信息列表。</param>
        /// <returns>发送任务。</returns>
        protected Task SendOthersAsync(string method, params object[] args)
        {
            return Clients.AllExcept(Context.ConnectionId).SendAsync(method, args, default);
        }

        /// <summary>
        /// 发送消息给当前连接客户。
        /// </summary>
        /// <param name="method">方法名称。</param>
        /// <param name="args">发送信息列表。</param>
        /// <returns>发送任务。</returns>
        protected Task SendAsync(string method, params object[] args)
        {
            return Clients.Caller.SendAsync(method, args, default);
        }

        /// <summary>
        /// 断开连接执行的方法。
        /// </summary>
        /// <param name="exception">错误实例。</param>
        /// <returns>断开连接执行任务。</returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (Onlines.TryRemove(Context.ConnectionId, out var user))
            {
                Connections.TryRemove(user.Id, out _);
                user.IsOnline = false;
                user.DisconnectedDate = DateTimeOffset.Now;
                await _userManager.UpdateAsync(user.Id, new { user.IsOnline });
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
