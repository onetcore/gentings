﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using Gentings.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Gentings.Security.Roles
{
    /// <summary>
    /// 角色声明类。
    /// </summary>
    [Table("sec_Roles_Claims")]
    public abstract class RoleClaimBase : IdentityRoleClaim<int>
    {
        /// <summary>
        /// 标识ID。
        /// </summary>
        [Identity]
        public override int Id { get; set; }

        /// <summary>
        /// 角色ID。
        /// </summary>
        public override int RoleId { get; set; }

        /// <summary>
        /// 声明类型。
        /// </summary>
        [Size(256)]
        public override string ClaimType { get; set; }

        /// <summary>
        /// 声明值。
        /// </summary>
        public override string ClaimValue { get; set; }

        /// <summary>
        /// 将角色声明转换为<see cref="Claim"/>对象。
        /// </summary>
        /// <returns>返回<see cref="Claim"/>实例对象。</returns>
        public override Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        /// <summary>
        /// 实例化属性。
        /// </summary>
        /// <param name="other">当前<see cref="Claim"/>对象实例。</param>
        public override void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }
}