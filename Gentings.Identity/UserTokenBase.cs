﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gentings.Data.Extensions;
using Gentings.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Gentings.Identity
{
    /// <summary>
    /// 用户登录提供者的一些信息存储。
    /// </summary>
    [Table("sec_Users_Tokens")]
    public abstract class UserTokenBase : IdentityUserToken<int>
    {
        /// <summary>
        /// 用户ID。
        /// </summary>
        [Key]
        public override int UserId { get; set; }

        /// <summary>
        /// 登录提供者。
        /// </summary>
        [Key]
        [Size(256)]
        public override string LoginProvider { get; set; }

        /// <summary>
        /// 标识唯一键。
        /// </summary>
        [Key]
        [Size(256)]
        public override string Name { get; set; }

        /// <summary>
        /// 当前标识的值。
        /// </summary>
        [ProtectedPersonalData]
        public override string Value { get; set; }
    }
}