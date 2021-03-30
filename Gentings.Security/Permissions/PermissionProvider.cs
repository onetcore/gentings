using System.Collections.Generic;

namespace Gentings.Security.Permissions
{
    /// <summary>
    /// 权限提供者基类。
    /// </summary>
    public abstract class PermissionProvider : IPermissionProvider
    {
        /// <summary>
        /// 权限名称。
        /// </summary>
        protected class Names
        {
            /// <summary>
            /// 访问。
            /// </summary>
            public const string View = "view";

            /// <summary>
            /// 添加。
            /// </summary>
            public const string Create = "create";

            /// <summary>
            /// 更新。
            /// </summary>
            public const string Update = "update";

            /// <summary>
            /// 删除。
            /// </summary>
            public const string Delete = "delete";

            /// <summary>
            /// 编辑。
            /// </summary>
            public const string Edit = "edit";

            /// <summary>
            /// 配置。
            /// </summary>
            public const string Setting = "setting";
        }

        /// <summary>
        /// 权限名称。
        /// </summary>
        protected class DotNames
        {
            /// <summary>
            /// 访问。
            /// </summary>
            public const string View = ".view";

            /// <summary>
            /// 添加。
            /// </summary>
            public const string Create = ".create";

            /// <summary>
            /// 更新。
            /// </summary>
            public const string Update = ".update";

            /// <summary>
            /// 删除。
            /// </summary>
            public const string Delete = ".delete";

            /// <summary>
            /// 编辑。
            /// </summary>
            public const string Edit = ".edit";

            /// <summary>
            /// 配置。
            /// </summary>
            public const string Setting = ".setting";
        }

        internal const string Core = "core";

        /// <summary>
        /// 分类。
        /// </summary>
        public virtual string Category => Core;

        /// <summary>
        /// 排序。
        /// </summary>
        public virtual int Order => 0;

        private readonly List<Permission> _permissions = new List<Permission>();

        /// <summary>
        /// 权限列表。
        /// </summary>
        /// <returns>返回权限列表。</returns>
        public IEnumerable<Permission> LoadPermissions()
        {
            Init();
            return _permissions;
        }

        /// <summary>
        /// 初始化权限实例。
        /// </summary>
        protected abstract void Init();

        /// <summary>
        /// 实例化一个权限。
        /// </summary>
        /// <param name="name">名称。</param>
        /// <param name="text">显示字符串。</param>
        /// <param name="description">描述。</param>
        /// <returns>返回权限实例。</returns>
        protected void Add(string name, string text, string description)
        {
            _permissions.Add(new Permission
            {
                Name = name,
                Text = text,
                Description = description
            });
        }
    }
}