namespace Gentings.RabbitMQ
{
    /// <summary>
    /// RabbitMQ配置接口。
    /// </summary>
    public interface IRabbitMQConfiguration : ISingletonService
    {
        /// <summary>
        /// 获取频道实例。
        /// </summary>
        /// <param name="queue">队列名称。</param>
        /// <param name="name">主机配置名称。</param>
        /// <returns>返回频道实例。</returns>
        IChannel GetOrCreateChannel(string queue, string name = "Default");
    }
}