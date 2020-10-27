using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Gentings.RabbitMQ
{
    internal class Channel : IChannel
    {
        private readonly IModel _model;
        private static readonly ConcurrentDictionary<string, IChannel> _channels = new ConcurrentDictionary<string, IChannel>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 获取或添加当前频道实例。
        /// </summary>
        /// <param name="queue">频道名称。</param>
        /// <param name="name">配置名称。</param>
        /// <param name="func">获取连接方法。</param>
        /// <returns>返回当前频道实例。</returns>
        public static IChannel GetOrCreate(string queue, string name, Func<IConnection> func)
        {
            return _channels.GetOrAdd($"{name}[{queue}]", _ =>
            {
                var connection = func();
                var model = connection.CreateModel();
                var channel = new Channel(model, queue);
                channel.Connection = connection;
                return channel;
            });
        }

        /// <summary>
        /// 移除频道。
        /// </summary>
        protected void Remove() => _channels.TryRemove($"[{Connection.Name}[{Queue}]", out _);
        /// <summary>
        /// 初始化类<see cref="Channel"/>。
        /// </summary>
        /// <param name="model">模型实例。</param>
        /// <param name="queue">队列名称。</param>
        public Channel(IModel model, string queue)
        {
            _model = model;
            _model.BasicAcks += Model_BasicAcks;
            _model.BasicNacks += Model_BasicNacks;
            _model.BasicRecoverOk += Model_BasicRecoverOk;
            _model.BasicReturn += Model_BasicReturn;
            _model.CallbackException += Model_CallbackException;
            _model.FlowControl += Model_FlowControl;
            _model.ModelShutdown += Model_ModelShutdown;
            Queue = queue;
        }

        private void Model_ModelShutdown(object sender, ShutdownEventArgs e)
        {
            ModelShutdown?.Invoke(sender, e);
        }

        private void Model_FlowControl(object sender, FlowControlEventArgs e)
        {
            FlowControl?.Invoke(sender, e);
        }

        private void Model_CallbackException(object sender, CallbackExceptionEventArgs e)
        {
            CallbackException?.Invoke(sender, e);
        }

        private void Model_BasicReturn(object sender, BasicReturnEventArgs e)
        {
            BasicReturn?.Invoke(sender, e);
        }

        private void Model_BasicRecoverOk(object sender, EventArgs e)
        {
            BasicRecoverOk?.Invoke(sender, e);
        }

        private void Model_BasicNacks(object sender, BasicNackEventArgs e)
        {
            BasicNacks?.Invoke(sender, e);
        }

        private void Model_BasicAcks(object sender, BasicAckEventArgs e)
        {
            BasicAcks?.Invoke(sender, e);
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public virtual void Dispose()
        {
            _model.Dispose();
            Remove();
        }

        /// <summary>Abort this session.</summary>
        /// <remarks>
        /// If the session is already closed (or closing), then this
        /// method does nothing but wait for the in-progress close
        /// operation to complete. This method will not return to the
        /// caller until the shutdown is complete.
        /// In comparison to normal <see cref="M:RabbitMQ.Client.IModel.Close" /> method, <see cref="M:RabbitMQ.Client.IModel.Abort" /> will not throw
        /// <see cref="T:RabbitMQ.Client.Exceptions.AlreadyClosedException" /> or <see cref="T:System.IO.IOException" /> or any other <see cref="T:System.Exception" /> during closing model.
        /// </remarks>
        public virtual void Abort()
        {
            _model.Abort();
        }

        /// <summary>Abort this session.</summary>
        /// <remarks>
        /// The method behaves in the same way as <see cref="M:RabbitMQ.Client.IModel.Abort" />, with the only
        /// difference that the model is closed with the given model close code and message.
        /// <para>
        /// The close code (See under "Reply Codes" in the AMQP specification)
        /// </para>
        /// <para>
        /// A message indicating the reason for closing the model
        /// </para>
        /// </remarks>
        public virtual void Abort(ushort replyCode, string replyText)
        {
            _model.Abort(replyCode, replyText);
        }

        /// <summary>Acknowledge one or more delivered message(s).</summary>
        public virtual void BasicAck(ulong deliveryTag, bool multiple)
        {
            _model.BasicAck(deliveryTag, multiple);
        }

        /// <summary>Delete a Basic content-class consumer.</summary>
        public virtual void BasicCancel(string consumerTag)
        {
            _model.BasicCancel(consumerTag);
        }

        /// <summary>
        /// Same as BasicCancel but sets nowait to true and returns void (as there
        /// will be no response from the server).
        /// </summary>
        public virtual void BasicCancelNoWait(string consumerTag)
        {
            _model.BasicCancelNoWait(consumerTag);
        }

        /// <summary>Start a Basic content-class consumer.</summary>
        public virtual string BasicConsume(string queue, bool autoAck, string consumerTag, bool noLocal, bool exclusive, IDictionary<string, object> arguments,
            IBasicConsumer consumer)
        {
            return _model.BasicConsume(queue, autoAck, consumerTag, noLocal, exclusive, arguments, consumer);
        }

        /// <summary>
        /// Retrieve an individual message, if
        /// one is available; returns null if the server answers that
        /// no messages are currently available. See also <see cref="M:RabbitMQ.Client.IModel.BasicAck(System.UInt64,System.Boolean)" />.
        /// </summary>
        public virtual BasicGetResult BasicGet(string queue, bool autoAck)
        {
            return _model.BasicGet(queue, autoAck);
        }

        /// <summary>Reject one or more delivered message(s).</summary>
        public virtual void BasicNack(ulong deliveryTag, bool multiple, bool requeue)
        {
            _model.BasicNack(deliveryTag, multiple, requeue);
        }

        /// <summary>Publishes a message.</summary>
        /// <remarks>
        ///   <para>
        ///     Routing key must be shorter than 255 bytes.
        ///   </para>
        /// </remarks>
        public virtual void BasicPublish(string exchange, string routingKey, bool mandatory, IBasicProperties basicProperties,
            ReadOnlyMemory<byte> body)
        {
            _model.BasicPublish(exchange, routingKey, mandatory, basicProperties, body);
        }

        /// <summary>Configures QoS parameters of the Basic content-class.</summary>
        public virtual void BasicQos(uint prefetchSize, ushort prefetchCount, bool global)
        {
            _model.BasicQos(prefetchSize, prefetchCount, global);
        }

        /// <summary>
        /// Indicates that a consumer has recovered.
        /// Deprecated. Should not be used.
        /// </summary>
        public virtual void BasicRecover(bool requeue)
        {
            _model.BasicRecover(requeue);
        }

        /// <summary>
        /// Indicates that a consumer has recovered.
        /// Deprecated. Should not be used.
        /// </summary>
        public virtual void BasicRecoverAsync(bool requeue)
        {
            _model.BasicRecoverAsync(requeue);
        }

        /// <summary> Reject a delivered message.</summary>
        public virtual void BasicReject(ulong deliveryTag, bool requeue)
        {
            _model.BasicReject(deliveryTag, requeue);
        }

        /// <summary>Close this session.</summary>
        /// <remarks>
        /// If the session is already closed (or closing), then this
        /// method does nothing but wait for the in-progress close
        /// operation to complete. This method will not return to the
        /// caller until the shutdown is complete.
        /// </remarks>
        public virtual void Close()
        {
            _model.Close();
            Remove();
        }

        /// <summary>Close this session.</summary>
        /// <remarks>
        /// The method behaves in the same way as Close(), with the only
        /// difference that the model is closed with the given model
        /// close code and message.
        /// <para>
        /// The close code (See under "Reply Codes" in the AMQP specification)
        /// </para>
        /// <para>
        /// A message indicating the reason for closing the model
        /// </para>
        /// </remarks>
        public virtual void Close(ushort replyCode, string replyText)
        {
            _model.Close(replyCode, replyText);
            Remove();
        }

        /// <summary>Enable publisher acknowledgements.</summary>
        public virtual void ConfirmSelect()
        {
            _model.ConfirmSelect();
        }

        /// <summary>Creates a BasicPublishBatch instance</summary>
        public virtual IBasicPublishBatch CreateBasicPublishBatch()
        {
            return _model.CreateBasicPublishBatch();
        }

        /// <summary>
        /// Construct a completely empty content header for use with the Basic content class.
        /// </summary>
        public virtual IBasicProperties CreateBasicProperties()
        {
            var properties = _model.CreateBasicProperties();
            properties.MessageId = Guid.NewGuid().ToString("N");
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
            return properties;
        }

        /// <summary>Bind an exchange to an exchange.</summary>
        /// <remarks>
        ///   <para>
        ///     Routing key must be shorter than 255 bytes.
        ///   </para>
        /// </remarks>
        public virtual void ExchangeBind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.ExchangeBind(destination, source, routingKey, arguments);
        }

        /// <summary>Like ExchangeBind but sets nowait to true.</summary>
        /// <remarks>
        ///   <para>
        ///     Routing key must be shorter than 255 bytes.
        ///   </para>
        /// </remarks>
        public virtual void ExchangeBindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.ExchangeBindNoWait(destination, source, routingKey, arguments);
        }

        /// <summary>Declare an exchange.</summary>
        /// <remarks>
        /// The exchange is declared non-passive and non-internal.
        /// The "nowait" option is not exercised.
        /// </remarks>
        public virtual void ExchangeDeclare(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            _model.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);
        }

        /// <summary>
        /// Same as ExchangeDeclare but sets nowait to true and returns void (as there
        /// will be no response from the server).
        /// </summary>
        public virtual void ExchangeDeclareNoWait(string exchange, string type, bool durable, bool autoDelete, IDictionary<string, object> arguments)
        {
            _model.ExchangeDeclareNoWait(exchange, type, durable, autoDelete, arguments);
        }

        /// <summary>Do a passive exchange declaration.</summary>
        /// <remarks>
        /// This method performs a "passive declare" on an exchange,
        /// which verifies whether .
        /// It will do nothing if the exchange already exists and result
        /// in a channel-level protocol exception (channel closure) if not.
        /// </remarks>
        public virtual void ExchangeDeclarePassive(string exchange)
        {
            _model.ExchangeDeclarePassive(exchange);
        }

        /// <summary>Delete an exchange.</summary>
        public virtual void ExchangeDelete(string exchange, bool ifUnused)
        {
            _model.ExchangeDelete(exchange, ifUnused);
        }

        /// <summary>Like ExchangeDelete but sets nowait to true.</summary>
        public virtual void ExchangeDeleteNoWait(string exchange, bool ifUnused)
        {
            _model.ExchangeDeleteNoWait(exchange, ifUnused);
        }

        /// <summary>Unbind an exchange from an exchange.</summary>
        /// <remarks>Routing key must be shorter than 255 bytes.</remarks>
        public virtual void ExchangeUnbind(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.ExchangeUnbind(destination, source, routingKey, arguments);
        }

        /// <summary>Like ExchangeUnbind but sets nowait to true.</summary>
        /// <remarks>
        ///   <para>
        ///     Routing key must be shorter than 255 bytes.
        ///   </para>
        /// </remarks>
        public virtual void ExchangeUnbindNoWait(string destination, string source, string routingKey, IDictionary<string, object> arguments)
        {
            _model.ExchangeUnbindNoWait(destination, source, routingKey, arguments);
        }

        /// <summary>Bind a queue to an exchange.</summary>
        /// <remarks>
        ///   <para>
        ///     Routing key must be shorter than 255 bytes.
        ///   </para>
        /// </remarks>
        public virtual void QueueBind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            _model.QueueBind(queue, exchange, routingKey, arguments);
        }

        /// <summary>Same as QueueBind but sets nowait parameter to true.</summary>
        /// <remarks>
        ///   <para>
        ///     Routing key must be shorter than 255 bytes.
        ///   </para>
        /// </remarks>
        public virtual void QueueBindNoWait(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            _model.QueueBindNoWait(queue, exchange, routingKey, arguments);
        }

        /// <summary> Declare a queue.</summary>
        public virtual QueueDeclareOk QueueDeclare(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            return _model.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }

        /// <summary>
        /// Same as QueueDeclare but sets nowait to true and returns void (as there
        /// will be no response from the server).
        /// </summary>
        public virtual void QueueDeclareNoWait(string queue, bool durable, bool exclusive, bool autoDelete, IDictionary<string, object> arguments)
        {
            _model.QueueDeclareNoWait(queue, durable, exclusive, autoDelete, arguments);
        }

        /// <summary>Declare a queue passively.</summary>
        /// <remarks>
        /// The queue is declared passive, non-durable,
        /// non-exclusive, and non-autodelete, with no arguments.
        /// The queue is declared passively; i.e. only check if it exists.
        ///  </remarks>
        public virtual QueueDeclareOk QueueDeclarePassive(string queue)
        {
            return _model.QueueDeclarePassive(queue);
        }

        /// <summary>
        /// Returns the number of messages in a queue ready to be delivered
        /// to consumers. This method assumes the queue exists. If it doesn't,
        /// an exception will be closed with an exception.
        /// </summary>
        /// <param name="queue">The name of the queue</param>
        public virtual uint MessageCount(string queue)
        {
            return _model.MessageCount(queue);
        }

        /// <summary>
        /// Returns the number of consumers on a queue.
        /// This method assumes the queue exists. If it doesn't,
        /// an exception will be closed with an exception.
        /// </summary>
        /// <param name="queue">The name of the queue</param>
        public virtual uint ConsumerCount(string queue)
        {
            return _model.ConsumerCount(queue);
        }

        /// <summary>Delete a queue.</summary>
        /// <remarks>
        /// Returns the number of messages purged during queue deletion.
        ///  <code>uint.MaxValue</code>.
        ///  </remarks>
        public virtual uint QueueDelete(string queue, bool ifUnused, bool ifEmpty)
        {
            return _model.QueueDelete(queue, ifUnused, ifEmpty);
        }

        /// <summary>
        /// Same as QueueDelete but sets nowait parameter to true
        /// and returns void (as there will be no response from the server)
        ///  </summary>
        public virtual void QueueDeleteNoWait(string queue, bool ifUnused, bool ifEmpty)
        {
            _model.QueueDeleteNoWait(queue, ifUnused, ifEmpty);
        }

        /// <summary>Purge a queue of messages.</summary>
        /// <remarks>Returns the number of messages purged.</remarks>
        public virtual uint QueuePurge(string queue)
        {
            return _model.QueuePurge(queue);
        }

        /// <summary>Unbind a queue from an exchange.</summary>
        /// <remarks>
        ///   <para>
        ///     Routing key must be shorter than 255 bytes.
        ///   </para>
        /// </remarks>
        public virtual void QueueUnbind(string queue, string exchange, string routingKey, IDictionary<string, object> arguments)
        {
            _model.QueueUnbind(queue, exchange, routingKey, arguments);
        }

        /// <summary>Commit this session's active TX transaction.</summary>
        public virtual void TxCommit()
        {
            _model.TxCommit();
        }

        /// <summary>Roll back this session's active TX transaction.</summary>
        public virtual void TxRollback()
        {
            _model.TxRollback();
        }

        /// <summary>Enable TX mode for this session.</summary>
        public virtual void TxSelect()
        {
            _model.TxSelect();
        }

        /// <summary>Wait until all published messages have been confirmed.</summary>
        /// <remarks>
        /// Waits until all messages published since the last call have
        /// been either ack'd or nack'd by the broker.  Returns whether
        /// all the messages were ack'd (and none were nack'd). Note,
        /// throws an exception when called on a non-Confirm channel.
        /// </remarks>
        public virtual bool WaitForConfirms()
        {
            return _model.WaitForConfirms();
        }

        /// <summary>
        /// Wait until all published messages have been confirmed.
        /// </summary>
        /// <returns>True if no nacks were received within the timeout, otherwise false.</returns>
        /// <param name="timeout">How long to wait (at most) before returning
        /// whether or not any nacks were returned.
        ///  </param>
        /// <remarks>
        /// Waits until all messages published since the last call have
        /// been either ack'd or nack'd by the broker.  Returns whether
        /// all the messages were ack'd (and none were nack'd). Note,
        /// throws an exception when called on a non-Confirm channel.
        /// </remarks>
        public virtual bool WaitForConfirms(TimeSpan timeout)
        {
            return _model.WaitForConfirms(timeout);
        }

        /// <summary>Wait until all published messages have been confirmed.</summary>
        /// <returns>True if no nacks were received within the timeout, otherwise false.</returns>
        /// <param name="timeout">How long to wait (at most) before returning
        /// whether or not any nacks were returned.
        /// </param>
        /// <param name="timedOut">True if the method returned because
        /// the timeout elapsed, not because all messages were ack'd or at least one nack'd.
        /// </param>
        /// <remarks>
        /// Waits until all messages published since the last call have
        /// been either ack'd or nack'd by the broker.  Returns whether
        /// all the messages were ack'd (and none were nack'd). Note,
        /// throws an exception when called on a non-Confirm channel.
        /// </remarks>
        public virtual bool WaitForConfirms(TimeSpan timeout, out bool timedOut)
        {
            return _model.WaitForConfirms(timeout, out timedOut);
        }

        /// <summary>Wait until all published messages have been confirmed.</summary>
        /// <remarks>
        /// Waits until all messages published since the last call have
        /// been ack'd by the broker.  If a nack is received, throws an
        /// OperationInterrupedException exception immediately.
        /// </remarks>
        public virtual void WaitForConfirmsOrDie()
        {
            _model.WaitForConfirmsOrDie();
        }

        /// <summary>Wait until all published messages have been confirmed.</summary>
        /// <remarks>
        /// Waits until all messages published since the last call have
        /// been ack'd by the broker.  If a nack is received or the timeout
        /// elapses, throws an OperationInterrupedException exception immediately.
        /// </remarks>
        public virtual void WaitForConfirmsOrDie(TimeSpan timeout)
        {
            _model.WaitForConfirmsOrDie(timeout);
        }

        /// <summary>Channel number, unique per connections.</summary>
        public virtual int ChannelNumber => _model.ChannelNumber;

        /// <summary>
        /// Returns null if the session is still in a state where it can be used,
        /// or the cause of its closure otherwise.
        /// </summary>
        public virtual ShutdownEventArgs CloseReason => _model.CloseReason;

        /// <summary>Signalled when an unexpected message is delivered
        /// Under certain circumstances it is possible for a channel to receive a
        /// message delivery which does not match any consumer which is currently
        /// set up via basicConsume(). This will occur after the following sequence
        /// of events:
        /// ctag = basicConsume(queue, consumer); // i.e. with explicit acks
        /// // some deliveries take place but are not acked
        /// basicCancel(ctag);
        /// basicRecover(false);
        /// Since requeue is specified to be false in the basicRecover, the spec
        /// states that the message must be redelivered to "the original recipient"
        /// - i.e. the same channel / consumer-tag. But the consumer is no longer
        /// active.
        /// In these circumstances, you can register a default consumer to handle
        /// such deliveries. If no default consumer is registered an
        /// InvalidOperationException will be thrown when such a delivery arrives.
        /// Most people will not need to use this.</summary>
        public virtual IBasicConsumer DefaultConsumer { get => _model.DefaultConsumer; set => _model.DefaultConsumer = value; }

        /// <summary>
        /// Returns true if the model is no longer in a state where it can be used.
        /// </summary>
        public virtual bool IsClosed => _model.IsClosed;

        /// <summary>
        /// Returns true if the model is still in a state where it can be used.
        /// Identical to checking if <see cref="P:RabbitMQ.Client.IModel.CloseReason" /> equals null.</summary>
        public virtual bool IsOpen => _model.IsOpen;

        /// <summary>
        /// When in confirm mode, return the sequence number of the next message to be published.
        /// </summary>
        public virtual ulong NextPublishSeqNo => _model.NextPublishSeqNo;

        /// <summary>
        /// Amount of time protocol  operations (e.g. <code>queue.declare</code>) are allowed to take before
        /// timing out.
        /// </summary>
        public virtual TimeSpan ContinuationTimeout { get => _model.ContinuationTimeout; set => _model.ContinuationTimeout = value; }

        /// <summary>
        /// Signalled when a Basic.Ack command arrives from the broker.
        /// </summary>
        public virtual event EventHandler<BasicAckEventArgs> BasicAcks;

        /// <summary>
        /// Signalled when a Basic.Nack command arrives from the broker.
        /// </summary>
        public virtual event EventHandler<BasicNackEventArgs> BasicNacks;

        /// <summary>
        /// All messages received before this fires that haven't been ack'ed will be redelivered.
        /// All messages received afterwards won't be.
        /// </summary>
        /// <remarks>
        /// Handlers for this event are invoked by the connection thread.
        /// It is sometimes useful to allow that thread to know that a recover-ok
        /// has been received, rather than the thread that invoked <see cref="M:RabbitMQ.Client.IModel.BasicRecover(System.Boolean)" />.
        /// </remarks>
        public virtual event EventHandler<EventArgs> BasicRecoverOk;

        /// <summary>
        /// Signalled when a Basic.Return command arrives from the broker.
        /// </summary>
        public virtual event EventHandler<BasicReturnEventArgs> BasicReturn;

        /// <summary>
        /// Signalled when an exception occurs in a callback invoked by the model.
        /// Examples of cases where this event will be signalled
        /// include exceptions thrown in <see cref="T:RabbitMQ.Client.IBasicConsumer" /> methods, or
        /// exceptions thrown in <see cref="E:RabbitMQ.Client.IModel.ModelShutdown" /> delegates etc.
        /// </summary>
        public virtual event EventHandler<CallbackExceptionEventArgs> CallbackException;
        public virtual event EventHandler<FlowControlEventArgs> FlowControl;

        /// <summary>Notifies the destruction of the model.</summary>
        /// <remarks>
        /// If the model is already destroyed at the time an event
        /// handler is added to this event, the event handler will be fired immediately.
        /// </remarks>
        public virtual event EventHandler<ShutdownEventArgs> ModelShutdown;

        /// <summary>
        /// 队列名称。
        /// </summary>
        public string Queue { get; }

        /// <summary>
        /// 连接实例。
        /// </summary>
        public IConnection Connection { get; set; }
    }
}