namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 连接接口。
    /// </summary>
    public interface IConnection : global::RabbitMQ.Client.IConnection
    {
        /// <summary>
        /// 配置名称。
        /// </summary>
        string Name { get; }
    }
}