namespace Gentings.RabbitMQ
{
    /// <summary>
    /// 交换机匹配类型。
    /// </summary>
    public enum ExchangeType
    {
        /// <summary>
        /// 要求routingKey和bindingKey必须相同。
        /// </summary>
        Direct,
        /// <summary>
        /// 只要routingKey符合bindingKey的规则：*表示一个单词，#表示0或多个单词(注意是单词，而不是字符)。
        /// </summary>
        Topic,
        /// <summary>
        /// 不需要指定routingKey。
        /// </summary>
        Fanout,
        /// <summary>
        /// 匹配通过Headers而不是通过routingKey和bindingKey。
        /// </summary>
        Headers,
    }
}