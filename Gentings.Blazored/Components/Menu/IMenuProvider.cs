namespace Gentings.Blazored.Components.Menu
{
    /// <summary>
    /// 菜单提供者。
    /// </summary>
    public interface IMenuProvider
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 提供者名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 初始化菜单。
        /// </summary>
        /// <param name="root">菜单根目录实例。</param>
        void Init(MenuItem root);
    }
}