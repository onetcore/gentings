using System.Threading.Tasks;

namespace Gentings.Blazored.Components.Menu
{
    /// <summary>
    /// 菜单提供者基类。
    /// </summary>
    public abstract class MenuProvider : IMenuProvider
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        public virtual int Priority { get; }

        /// <summary>
        /// 提供者名称。
        /// </summary>
        public virtual string Name => DefaultName;

        /// <summary>
        /// 默认提供者名称。
        /// </summary>
        public const string DefaultName = "main";

        /// <summary>
        /// 初始化菜单。
        /// </summary>
        /// <param name="root">菜单根目录实例。</param>
        public virtual void Init(MenuItem root)
        {
            InitAsync(root).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 初始化菜单。
        /// </summary>
        /// <param name="root">菜单根目录实例。</param>
        protected virtual Task InitAsync(MenuItem root)
        {
            return Task.CompletedTask;
        }
    }
}