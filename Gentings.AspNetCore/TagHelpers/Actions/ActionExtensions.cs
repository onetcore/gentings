namespace Gentings.AspNetCore.TagHelpers.Actions
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
        public static string? GetIconClassName(this ActionType type)
        {
            return type switch
            {
                ActionType.Edit => "bi-pencil",
                ActionType.Delete => "bi-trash",
                ActionType.MoveUp => "bi-arrow-up",
                ActionType.MoveDown => "bi-arrow-down",
                ActionType.Add => "bi-plus",
                ActionType.Upload => "bi-upload",
                _ => null,
            };
        }
    }
}
