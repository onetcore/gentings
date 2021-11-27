namespace Gentings.AspNetCore
{
    /// <summary>
    /// 操作类型。
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// 发送请求。
        /// </summary>
        Action,
        /// <summary>
        /// 添加。
        /// </summary>
        Add,
        /// <summary>
        /// 编辑。
        /// </summary>
        Edit,
        /// <summary>
        /// 删除。
        /// </summary>
        Delete,
        /// <summary>
        /// 上移。
        /// </summary>
        MoveUp,
        /// <summary>
        /// 下移。
        /// </summary>
        MoveDown,
        /// <summary>
        /// 分隔符。
        /// </summary>
        Divider,
        /// <summary>
        /// 上传文件。
        /// </summary>
        Upload,
        /// <summary>
        /// 模态框显示。
        /// </summary>
        Modal,
    }
}
