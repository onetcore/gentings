using System;

namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 特性类型。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class RabbitMQAttribute : Attribute
    {
        /// <summary>
        /// 主机配置名称。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 队列名称。
        /// </summary>
        public string Queue { get; }

        /// <summary>
        /// 初始化类<see cref="RabbitMQAttribute"/>。
        /// </summary>
        /// <param name="name">主机配置名称。</param>
        /// <param name="queue">队列名称。</param>
        public RabbitMQAttribute(string name, string queue)
        {
            Name = name;
            Queue = queue;
        }

        /// <summary>
        /// 初始化类<see cref="RabbitMQAttribute"/>。
        /// </summary>
        /// <param name="queue">队列名称。</param>
        public RabbitMQAttribute(string queue) : this("Default", queue)
        {
        }

        /// <summary>
        /// 交换机名称。
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 交换机类型。
        /// </summary>
        public ExchangeType ExchangeType { get; set; }

        /// <summary>
        /// 路由键。
        /// </summary>
        public string RoutingKey { get; set; }
    }
}