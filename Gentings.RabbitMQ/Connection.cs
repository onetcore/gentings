using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Gentings.RabbitMQ
{
    internal class Connection : IConnection
    {
        private readonly global::RabbitMQ.Client.IConnection _connection;

        public Connection(global::RabbitMQ.Client.IConnection connection, string name)
        {
            _connection = connection;
            connection.CallbackException += Connection_CallbackException;
            connection.ConnectionBlocked += Connection_ConnectionBlocked;
            connection.ConnectionShutdown += Connection_ConnectionShutdown;
            connection.ConnectionUnblocked += Connection_ConnectionUnblocked;
            Name = name;
        }

        private void Connection_ConnectionUnblocked(object sender, EventArgs e)
        {
            ConnectionUnblocked?.Invoke(sender, e);
        }

        private void Connection_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            ConnectionShutdown?.Invoke(sender, e);
        }

        private void Connection_ConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            ConnectionBlocked?.Invoke(sender, e);
        }

        private void Connection_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            CallbackException?.Invoke(sender, e);
        }

        /// <summary>Local port.</summary>
        public virtual int LocalPort => _connection.LocalPort;

        /// <summary>Remote port.</summary>
        public virtual int RemotePort => _connection.RemotePort;

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public virtual void Dispose()
        {
            _connection.Dispose();
            _connections.TryRemove(Name, out _);
        }

        /// <summary>
        /// This method updates the secret used to authenticate this connection.
        /// It is used when secrets have an expiration date and need to be renewed,
        /// like OAuth 2 tokens.
        /// </summary>
        /// <param name="newSecret">The new secret.</param>
        /// <param name="reason">The reason for the secret update.</param>
        public virtual void UpdateSecret(string newSecret, string reason)
        {
            _connection.UpdateSecret(newSecret, reason);
        }

        /// <summary>Abort this connection and all its channels.</summary>
        /// <remarks>
        ///  Note that all active channels, sessions, and models will be closed if this method is called.
        ///  In comparison to normal <see cref="M:RabbitMQ.Client.IConnection.Close" /> method, <see cref="M:RabbitMQ.Client.IConnection.Abort" /> will not throw
        ///  <see cref="T:System.IO.IOException" /> during closing connection.
        /// This method waits infinitely for the in-progress close operation to complete.
        ///  </remarks>
        public virtual void Abort()
        {
            _connection.Abort();
        }

        /// <summary>Abort this connection and all its channels.</summary>
        /// <remarks>
        /// The method behaves in the same way as <see cref="M:RabbitMQ.Client.IConnection.Abort" />, with the only
        /// difference that the connection is closed with the given connection close code and message.
        /// <para>
        /// The close code (See under "Reply Codes" in the AMQP 0-9-1 specification)
        /// </para>
        /// <para>
        /// A message indicating the reason for closing the connection
        /// </para>
        /// </remarks>
        public virtual void Abort(ushort reasonCode, string reasonText)
        {
            _connection.Abort(reasonCode, reasonText);
        }

        /// <summary>
        /// Abort this connection and all its channels and wait with a
        /// timeout for all the in-progress close operations to complete.
        /// </summary>
        /// <remarks>
        /// This method, behaves in a similar way as method <see cref="M:RabbitMQ.Client.IConnection.Abort" /> with the
        /// only difference that it explictly specifies a timeout given
        /// for all the in-progress close operations to complete.
        /// If timeout is reached and the close operations haven't finished, then socket is forced to close.
        /// <para>
        /// To wait infinitely for the close operations to complete use <see cref="F:System.Threading.Timeout.Infinite" />.
        /// </para>
        /// </remarks>
        public virtual void Abort(TimeSpan timeout)
        {
            _connection.Abort(timeout);
        }

        /// <summary>
        /// Abort this connection and all its channels and wait with a
        /// timeout for all the in-progress close operations to complete.
        /// </summary>
        /// <remarks>
        /// The method behaves in the same way as <see cref="M:RabbitMQ.Client.IConnection.Abort(System.TimeSpan)" />, with the only
        /// difference that the connection is closed with the given connection close code and message.
        /// <para>
        /// The close code (See under "Reply Codes" in the AMQP 0-9-1 specification).
        /// </para>
        /// <para>
        /// A message indicating the reason for closing the connection.
        /// </para>
        /// </remarks>
        public virtual void Abort(ushort reasonCode, string reasonText, TimeSpan timeout)
        {
            _connection.Abort(reasonCode, reasonText, timeout);
        }

        /// <summary>Close this connection and all its channels.</summary>
        /// <remarks>
        /// Note that all active channels, sessions, and models will be
        /// closed if this method is called. It will wait for the in-progress
        /// close operation to complete. This method will not return to the caller
        /// until the shutdown is complete. If the connection is already closed
        /// (or closing), then this method will do nothing.
        /// It can also throw <see cref="T:System.IO.IOException" /> when socket was closed unexpectedly.
        /// </remarks>
        public virtual void Close()
        {
            _connection.Close();
            _connections.TryRemove(Name, out _);
        }

        /// <summary>Close this connection and all its channels.</summary>
        /// <remarks>
        /// The method behaves in the same way as <see cref="M:RabbitMQ.Client.IConnection.Close" />, with the only
        /// difference that the connection is closed with the given connection close code and message.
        /// <para>
        /// The close code (See under "Reply Codes" in the AMQP specification).
        /// </para>
        /// <para>
        /// A message indicating the reason for closing the connection.
        /// </para>
        /// </remarks>
        public virtual void Close(ushort reasonCode, string reasonText)
        {
            _connection.Close(reasonCode, reasonText);
            _connections.TryRemove(Name, out _);
        }

        /// <summary>
        /// Close this connection and all its channels
        /// and wait with a timeout for all the in-progress close operations to complete.
        /// </summary>
        /// <remarks>
        /// Note that all active channels, sessions, and models will be
        /// closed if this method is called. It will wait for the in-progress
        /// close operation to complete with a timeout. If the connection is
        /// already closed (or closing), then this method will do nothing.
        /// It can also throw <see cref="T:System.IO.IOException" /> when socket was closed unexpectedly.
        /// If timeout is reached and the close operations haven't finished, then socket is forced to close.
        /// <para>
        /// To wait infinitely for the close operations to complete use <see cref="F:System.Threading.Timeout.InfiniteTimeSpan" />.
        /// </para>
        /// </remarks>
        public virtual void Close(TimeSpan timeout)
        {
            _connection.Close(timeout);
            _connections.TryRemove(Name, out _);
        }

        /// <summary>
        /// Close this connection and all its channels
        /// and wait with a timeout for all the in-progress close operations to complete.
        /// </summary>
        /// <remarks>
        /// The method behaves in the same way as <see cref="M:RabbitMQ.Client.IConnection.Close(System.TimeSpan)" />, with the only
        /// difference that the connection is closed with the given connection close code and message.
        /// <para>
        /// The close code (See under "Reply Codes" in the AMQP 0-9-1 specification).
        /// </para>
        /// <para>
        /// A message indicating the reason for closing the connection.
        /// </para>
        /// <para>
        /// Operation timeout.
        /// </para>
        /// </remarks>
        public virtual void Close(ushort reasonCode, string reasonText, TimeSpan timeout)
        {
            _connection.Close(reasonCode, reasonText, timeout);
            _connections.TryRemove(Name, out _);
        }

        /// <summary>Create and return a fresh channel, session, and model.</summary>
        public virtual IModel CreateModel()
        {
            return _connection.CreateModel();
        }

        /// <summary>Handle incoming Connection.Blocked methods.</summary>
        public virtual void HandleConnectionBlocked(string reason)
        {
            _connection.HandleConnectionBlocked(reason);
        }

        /// <summary>Handle incoming Connection.Unblocked methods.</summary>
        public virtual void HandleConnectionUnblocked()
        {
            _connection.HandleConnectionUnblocked();
        }

        /// <summary>
        /// The maximum channel number this connection supports (0 if unlimited).
        /// Usable channel numbers range from 1 to this number, inclusive.
        /// </summary>
        public virtual ushort ChannelMax => _connection.ChannelMax;

        /// <summary>
        /// A copy of the client properties that has been sent to the server.
        /// </summary>
        public virtual IDictionary<string, object> ClientProperties => _connection.ClientProperties;

        /// <summary>
        /// Returns null if the connection is still in a state
        /// where it can be used, or the cause of its closure otherwise.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Applications should use the ConnectionShutdown event to
        /// avoid race conditions. The scenario to avoid is checking
        /// <see cref="P:RabbitMQ.Client.IConnection.CloseReason" />, seeing it is null (meaning the <see cref="T:RabbitMQ.Client.IConnection" />
        /// was available for use at the time of the check), and
        /// interpreting this mistakenly as a guarantee that the
        /// <see cref="T:RabbitMQ.Client.IConnection" /> will remain usable for a time. Instead, the
        /// operation of interest should simply be attempted: if the
        /// <see cref="T:RabbitMQ.Client.IConnection" /> is not in a usable state, an exception will be
        /// thrown (most likely <see cref="T:RabbitMQ.Client.Exceptions.OperationInterruptedException" />, but may
        /// vary depending on the particular operation being attempted).
        /// </para>
        /// </remarks>
        public virtual ShutdownEventArgs CloseReason => _connection.CloseReason;

        /// <summary>Retrieve the endpoint this connection is connected to.</summary>
        public virtual AmqpTcpEndpoint Endpoint => _connection.Endpoint;

        /// <summary>
        /// The maximum frame size this connection supports (0 if unlimited).
        /// </summary>
        public virtual uint FrameMax => _connection.FrameMax;

        /// <summary>
        /// The current heartbeat setting for this connection (System.TimeSpan.Zero for disabled).
        /// </summary>
        public virtual TimeSpan Heartbeat => _connection.Heartbeat;

        /// <summary>
        /// Returns true if the connection is still in a state where it can be used.
        /// Identical to checking if <see cref="P:RabbitMQ.Client.IConnection.CloseReason" /> equal null.
        /// </summary>
        public virtual bool IsOpen => _connection.IsOpen;

        /// <summary>
        /// Returns the known hosts that came back from the
        /// broker in the connection.open-ok method at connection
        /// startup time. Null until the connection is completely open and ready for use.
        /// </summary>
        public virtual AmqpTcpEndpoint[] KnownHosts => _connection.KnownHosts;

        /// <summary>
        /// The <see cref="T:RabbitMQ.Client.IProtocol" /> this connection is using to communicate with its peer.
        /// </summary>
        public virtual IProtocol Protocol => _connection.Protocol;

        /// <summary>
        /// A dictionary of the server properties sent by the server while establishing the connection.
        /// This typically includes the product name and version of the server.
        /// </summary>
        public virtual IDictionary<string, object> ServerProperties => _connection.ServerProperties;

        /// <summary>
        /// Returns the list of <see cref="T:RabbitMQ.Client.ShutdownReportEntry" /> objects that contain information
        /// about any errors reported while closing the connection in the order they appeared
        /// </summary>
        public virtual IList<ShutdownReportEntry> ShutdownReport => _connection.ShutdownReport;

        /// <summary>
        /// Application-specific connection name, will be displayed in the management UI
        /// if RabbitMQ server supports it. This value doesn't have to be unique and cannot
        /// be used as a connection identifier, e.g. in HTTP API requests.
        /// This value is supposed to be human-readable.
        /// </summary>
        public virtual string ClientProvidedName => _connection.ClientProvidedName;

        /// <summary>
        /// Signalled when an exception occurs in a callback invoked by the connection.
        /// </summary>
        /// <remarks>
        /// This event is signalled when a ConnectionShutdown handler
        /// throws an exception. If, in future, more events appear on
        /// <see cref="T:RabbitMQ.Client.IConnection" />, then this event will be signalled whenever one
        /// of those event handlers throws an exception, as well.
        /// </remarks>
        public virtual event EventHandler<CallbackExceptionEventArgs> CallbackException;
        public virtual event EventHandler<ConnectionBlockedEventArgs> ConnectionBlocked;

        /// <summary>Raised when the connection is destroyed.</summary>
        /// <remarks>
        /// If the connection is already destroyed at the time an
        /// event handler is added to this event, the event handler
        /// will be fired immediately.
        /// </remarks>
        public virtual event EventHandler<ShutdownEventArgs> ConnectionShutdown;
        public virtual event EventHandler<EventArgs> ConnectionUnblocked;

        private static readonly ConcurrentDictionary<string, IConnection> _connections = new ConcurrentDictionary<string, IConnection>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 获取或设置连接实例。
        /// </summary>
        /// <param name="name">配置名称。</param>
        /// <param name="func">实例化方法。</param>
        /// <returns>返回连接实例对象。</returns>
        public static IConnection GetOrCreate(string name, Func<ConnectionFactory> func)
        {
            if (!_connections.TryGetValue(name, out var connection))
            {
                var factory = func();
                factory.AutomaticRecoveryEnabled = true;
                connection = new Connection(factory.CreateConnection(), name);
                _connections.AddOrUpdate(name, connection, (k, v) => connection);
            }

            return connection;
        }

        /// <summary>
        /// 配置名称。
        /// </summary>
        public string Name { get; }
    }
}