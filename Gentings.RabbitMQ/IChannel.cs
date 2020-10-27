using RabbitMQ.Client;

namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 频道接口。
    /// </summary>
    public interface IChannel : IModel
    {
        /// <summary>
        /// 队列名称。
        /// </summary>
        string Queue { get; }

        /// <summary>
        /// 连接实例。
        /// </summary>
        IConnection Connection { get; }
    }
}