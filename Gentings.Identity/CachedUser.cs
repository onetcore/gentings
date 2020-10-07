using System.Text.Json.Serialization;

namespace Gentings.Identity
{
    /// <summary>
    /// 缓存用户实例。
    /// </summary>
    public class CachedUser
    {
        /// <summary>
        /// 用户Id。
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        [JsonIgnore]
        public string UserName { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 昵称。
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 角色名称。
        /// </summary>
        [JsonIgnore]
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名称。
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色颜色。
        /// </summary>
        public string RoleColor { get; set; }

        /// <summary>
        /// 用户角色URL地址。
        /// </summary>
        public string RoleIcon { get; set; }

        /// <summary>
        /// 角色等级。
        /// </summary>
        [JsonIgnore]
        public int RoleLevel { get; set; }

        /// <summary>
        /// 电子邮件。
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 电话号码。
        /// </summary>
        public string PhoneNumber { get; set; }
    }
}