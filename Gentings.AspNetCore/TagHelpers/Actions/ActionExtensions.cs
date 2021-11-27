namespace Gentings.AspNetCore.TagHelpers.Bootstraps.Actions
{
    /// <summary>
    /// 扩展方法类。
    /// </summary>
    public static class ActionExtensions
    {
        /// <summary>
        /// 获取状态图标。
        /// </summary>
        /// <param name="type">状态类型。</param>
        /// <returns>返回图标样式名称。</returns>
        public static string GetIconClassName(this ActionType type)
        {
            switch (type)
            {
                case ActionType.Edit:
                    return "bi-pencil";
                case ActionType.Delete:
                    return "bi-trash";
                case ActionType.MoveUp:
                    return "bi-arrow-up";
                case ActionType.MoveDown:
                    return "bi-arrow-down";
                case ActionType.Add:
                    return "bi-plus";
                case ActionType.Upload:
                    return "bi-upload";
            }
            return null;
        }
    }
}
