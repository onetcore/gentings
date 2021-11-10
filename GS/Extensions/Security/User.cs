using Gentings;
using Gentings.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 管理员。
    /// </summary>
    [Table("core_Administrators")]
    public class User : ExtendBase, IUser
    {
        /// <summary>
        /// 用户ID。
        /// </summary>
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 用户名称。
        /// </summary>
        [Size(64)]
        public string? UserName { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [Size(128)]
        public string? Password { get; set; }

        /// <summary>
        /// 昵称。
        /// </summary>
        [Size(64)]
        public string? NickName { get; set; }

        /// <summary>
        /// 电子邮件。
        /// </summary>
        [Size(256)]
        public string? Email { get; set; }

        /// <summary>
        /// 添加时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 最后更新时间。
        /// </summary>
        public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;

        /// <summary>
        /// 最后登录时间。
        /// </summary>
        [NotUpdated]
        public DateTimeOffset? LastLoginDate { get; set; }

        /// <summary>
        /// 头像。
        /// </summary>
        [Size(256)]
        public string Avatar { get; set; } = "/images/avatar.png";

        /// <summary>
        /// 登录IP。
        /// </summary>
        [NotUpdated]
        [Size(20)]
        public string? LoginIP { get; set; }

        /// <summary>
        /// 加密密码。
        /// </summary>
        /// <param name="userName">用户名称。</param>
        /// <param name="password">未加密的密码。</param>
        /// <returns>返回加密后的密码。</returns>
        public static string Hashed(string userName, string password)
        {
            userName = userName.Trim().ToUpper();
            password = password.Trim();
            return Cores.Hashed($"{userName}:{password}");
        }

        /// <summary>
        /// 验证密码。
        /// </summary>
        /// <param name="password">未加密的密码。</param>
        /// <returns>返回判断结果。</returns>
        public bool IsValid(string password)
        {
            password = Hashed(UserName!, password);
            return password.Equals(Password, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 返回当前用户的用户名。
        /// </summary>
        public override string? ToString()
        {
            return UserName;
        }
    }
}
