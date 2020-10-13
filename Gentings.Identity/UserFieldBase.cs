using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Identity
{
    /// <summary>
    /// 包含用户字段基类。
    /// </summary>
    public abstract class UserFieldBase
    {
        /// <summary>
        /// 用户名称。
        /// </summary>
        [NotMapped]
        public string UserName { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        [NotMapped]
        public string Avatar { get; set; }

        /// <summary>
        /// 用户姓名。
        /// </summary>
        [NotMapped]
        public string NickName { get; set; }

        /// <summary>
        /// 角色名称。
        /// </summary>
        [NotMapped]
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称。
        /// </summary>
        [NotMapped]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色颜色。
        /// </summary>
        [NotMapped]
        public string RoleColor { get; set; }

        /// <summary>
        /// 用户角色URL地址。
        /// </summary>
        [NotMapped]
        public string RoleIcon { get; set; }
    }
}