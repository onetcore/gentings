namespace Gentings.Extensions.Sites.Sections
{
    /// <summary>
    /// 默认节点。
    /// </summary>
    public class DefaultSection : SectionBase
    {
        /// <summary>
        /// 默认名称。
        /// </summary>
        public const string Default = "Default";

        /// <summary>
        /// 类型名称。
        /// </summary>
        public override string Name => Default;

        /// <summary>
        /// 显示名称。
        /// </summary>
        public override string DisplayName => "默认节点";

        /// <summary>
        /// 图标地址。
        /// </summary>
        public override string IconUrl => "bi-card-text";

        /// <summary>
        /// 优先级。
        /// </summary>
        public override int Priority => int.MaxValue;

        /// <summary>
        /// 描述。
        /// </summary>
        public override string Summary => "默认节点实例，提供节点默认设置。";

        /// <summary>
        /// 配置地址。
        /// </summary>
        public override string EditUrl => "/sites/backend/sections/edit";
    }
}