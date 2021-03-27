﻿namespace Gentings.AspNetCore.StatusMessages
{
    /// <summary>
    /// JSON消息。
    /// </summary>
    public class JsonStatusMesssage
    {
        /// <summary>
        /// 初始化类<see cref="JsonStatusMesssage"/>。
        /// </summary>
        /// <param name="type">显示类型。</param>
        /// <param name="message">消息。</param>
        public JsonStatusMesssage(StatusType type, string message)
        {
            Type = type.ToString().ToLower();
            Message = message;
        }

        /// <summary>
        /// 类型。
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// 消息。
        /// </summary>
        public string Message { get; }
    }

    /// <summary>
    /// JSON消息。
    /// </summary>
    /// <typeparam name="TData">数据类型。</typeparam>
    public class JsonStatusMesssage<TData> : JsonStatusMesssage
    {
        /// <summary>
        /// 返回的数据。
        /// </summary>
        public TData Data { get; }

        /// <summary>
        /// 初始化类<see cref="JsonStatusMesssage"/>。
        /// </summary>
        /// <param name="type">显示类型。</param>
        /// <param name="message">消息。</param>
        /// <param name="data">数据实例对象。</param>
        public JsonStatusMesssage(StatusType type, string message, TData data) : base(type, message)
        {
            Data = data;
        }
    }
}