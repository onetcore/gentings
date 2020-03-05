namespace Gentings.Messages.CMPP
{
    /// <summary>
    /// 操作码。
    /// </summary>
    public enum CMPPCommand : uint
    {
        /// <summary>
        /// 请求连接 
        /// </summary>
        CMPP_CONNECT = 0x00000001,
        /// <summary>
        ///请求连接应答
        /// </summary>
        CMPP_CONNECT_RESP = 0x80000001,
        /// <summary>
        /// 终止连接，无消息体。
        /// </summary>
        CMPP_TERMINATE = 0x00000002,
        /// <summary>
        /// 终止连接应答，无消息体。
        /// </summary>
        CMPP_TERMINATE_RESP = 0x80000002,
        /// <summary>
        /// 提交短信
        /// </summary>
        CMPP_SUBMIT = 0x00000004,
        /// <summary>
        /// 提交短信应答 
        /// </summary>
        CMPP_SUBMIT_RESP = 0x80000004,
        /// <summary>
        /// 短信下发
        /// </summary>
        CMPP_DELIVER = 0x00000005,
        /// <summary>
        /// 下发短信应答
        /// </summary>
        CMPP_DELIVER_RESP = 0x80000005,
        /// <summary>
        /// 发送短信状态查询
        /// </summary>
        CMPP_QUERY = 0x00000006,
        /// <summary>
        /// 发送短信状态查询应答
        /// </summary>
        CMPP_QUERY_RESP = 0x80000006,
        /// <summary>
        /// 删除短信
        /// </summary>
        CMPP_CANCEL = 0x00000007,
        /// <summary>
        /// 删除短信应答
        /// </summary>
        CMPP_CANCEL_RESP = 0x80000007,
        /// <summary>
        /// 激活测试
        /// </summary>
        CMPP_ACTIVE_TEST = 0x00000008,
        /// <summary>
        /// 激活测试应答
        /// </summary>
        CMPP_ACTIVE_TEST_RESP = 0x80000008,
        /// <summary>
        /// 消息前转
        /// </summary>
        CMPP_FWD = 0x00000009,
        /// <summary>
        /// 消息前转应答
        /// </summary>
        CMPP_FWD_RESP = 0x80000009,
        /// <summary>
        /// MT路由请求
        /// </summary>
        CMPP_MT_ROUTE = 0x00000010,
        /// <summary>
        /// MT路由请求应答
        /// </summary>
        CMPP_MT_ROUTE_RESP = 0x80000010,
        /// <summary>
        /// MO路由请求
        /// </summary>
        CMPP_MO_ROUTE = 0x00000011,
        /// <summary>
        /// MO路由请求应答
        /// </summary>
        CMPP_MO_ROUTE_RESP = 0x80000011,
        /// <summary>
        /// 获取MT路由请求
        /// </summary>
        CMPP_GET_MT_ROUTE = 0x00000012,
        /// <summary>
        /// 获取MT路由请求应答
        /// </summary>
        CMPP_GET_MT_ROUTE_RESP = 0x80000012,
        /// <summary>
        /// MT路由更新
        /// </summary>
        CMPP_MT_ROUTE_UPDATE = 0x00000013,
        /// <summary>
        /// MT路由更新应答
        /// </summary>
        CMPP_MT_ROUTE_UPDATE_RESP = 0x80000013,
        /// <summary>
        /// MO路由更新
        /// </summary>
        CMPP_MO_ROUTE_UPDATE = 0x00000014,
        /// <summary>
        /// MO路由更新应答
        /// </summary>
        CMPP_MO_ROUTE_UPDATE_RESP = 0x80000014,
        /// <summary>
        /// MT路由更新
        /// </summary>
        CMPP_PUSH_MT_ROUTE_UPDATE = 0x00000015,
        /// <summary>
        /// MT路由更新应答
        /// </summary>
        CMPP_PUSH_MT_ROUTE_UPDATE_RESP = 0x80000015,
        /// <summary>
        /// MO路由更新
        /// </summary>
        CMPP_PUSH_MO_ROUTE_UPDATE = 0x00000016,
        /// <summary>
        /// MO路由更新应答
        /// </summary>
        CMPP_PUSH_MO_ROUTE_UPDATE_RESP = 0x80000016,
        /// <summary>
        /// 获取MO路由请求
        /// </summary>
        CMPP_GET_MO_ROUTE = 0x00000017,
        /// <summary>
        /// 获取MO路由请求应答
        /// </summary>
        CMPP_GET_MO_ROUTE_RESP = 0x80000017,
    }
}
