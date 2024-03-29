﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data;
using Microsoft.AspNetCore.Identity;

namespace Gentings.Security.Roles
{
    /// <summary>
    /// 用户角色存储基类。
    /// </summary>
    /// <typeparam name="TRole">用户角色类型。</typeparam>
    /// <typeparam name="TUserRole">用户角色关联类。</typeparam>
    /// <typeparam name="TRoleClaim">角色声明类型。</typeparam>
    public abstract class RoleStoreBase<TRole, TUserRole, TRoleClaim> : RoleStoreBase<TRole, int, TUserRole, TRoleClaim>, IRoleStoreBase<TRole, TUserRole, TRoleClaim>
        where TRole : RoleBase
        where TUserRole : UserRoleBase, new()
        where TRoleClaim : RoleClaimBase, new()
    {
        /// <summary>
        /// 角色数据库操作接口。
        /// </summary>
        public IDbContext<TRole> RoleContext { get; }

        /// <summary>
        /// 用户角色数据库操作接口。
        /// </summary>
        public IDbContext<TUserRole> UserRoleContext { get; }

        /// <summary>
        /// 用户声明数据库操作接口。
        /// </summary>
        public IDbContext<TRoleClaim> RoleClaimContext { get; }

        /// <summary>
        /// 初始化类<see cref="RoleStoreBase{TRole,TUserRole,TRoleClaim}"/>。
        /// </summary>
        /// <param name="describer">错误描述<see cref="IdentityErrorDescriber"/>实例。</param>
        /// <param name="roleContext">角色数据库操作接口。</param>
        /// <param name="userRoleContext">用户角色数据库操作接口。</param>
        /// <param name="roleClaimContext">用户声明数据库操作接口。</param>
        protected RoleStoreBase(IdentityErrorDescriber describer,
            IDbContext<TRole> roleContext,
            IDbContext<TUserRole> userRoleContext,
            IDbContext<TRoleClaim> roleClaimContext) : base(describer)
        {
            RoleContext = roleContext;
            UserRoleContext = userRoleContext;
            RoleClaimContext = roleClaimContext;
        }

        /// <summary>
        /// 获取当前最大角色等级。
        /// </summary>
        /// <param name="role">当前角色实例。</param>
        /// <returns>返回最大角色等级。</returns>
        protected virtual Task<int> GetMaxRoleLevelAsync(TRole role)
        {
            return RoleContext.MaxAsync(x => x.RoleLevel, x => x.IsSystem == role.IsSystem && x.RoleLevel < int.MaxValue);
        }

        /// <summary>
        /// 获取当前最大角色等级。
        /// </summary>
        /// <param name="role">当前角色实例。</param>
        /// <returns>返回最大角色等级。</returns>
        protected virtual int GetMaxRoleLevel(TRole role)
        {
            return RoleContext.Max(x => x.RoleLevel, x => x.IsSystem == role.IsSystem && x.RoleLevel < int.MaxValue);
        }

        /// <summary>
        /// 添加用户角色。
        /// </summary>
        /// <param name="role">用户角色实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回角色添加结果。</returns>
        public override async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.RoleLevel = await GetMaxRoleLevelAsync(role) + 1;//获取当前角色等级
            if (await RoleContext.CreateAsync(role, cancellationToken))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 更新用户角色。
        /// </summary>
        /// <param name="role">用户角色实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回角色更新结果。</returns>
        public override async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            
            if (await RoleContext.UpdateAsync(role, cancellationToken))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 删除用户角色。
        /// </summary>
        /// <param name="role">用户角色实例。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回角色删除结果。</returns>
        public override async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            
            if (await RoleContext.DeleteAsync(role.Id, cancellationToken))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 更新用户角色。
        /// </summary>
        /// <param name="role">用户角色实例。</param>
        /// <returns>返回角色更新结果。</returns>
        public virtual IdentityResult Delete(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            
            if (RoleContext.Delete(role.Id))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 通过ID获取角色实例。
        /// </summary>
        /// <param name="id">角色Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public virtual Task<TRole> FindByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return RoleContext.FindAsync(id, cancellationToken);
        }

        /// <summary>
        /// 通过角色名称获取角色实例。
        /// </summary>
        /// <param name="normalizedName">角色名称。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public override Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            return RoleContext.FindAsync(x => x.NormalizedName == normalizedName, cancellationToken);
        }

        /// <summary>
        /// 获取角色声明列表。
        /// </summary>
        /// <param name="role">角色实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回当前角色的声明列表。</returns>
        public override async Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            var claims = await RoleClaimContext.FetchAsync(x => x.RoleId == role.Id, cancellationToken);
            return claims.Select(x => x.ToClaim()).ToList();
        }

        /// <summary>
        /// 添加角色声明。
        /// </summary>
        /// <param name="role">角色实例对象。</param>
        /// <param name="claim">声明实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        public override async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            await RoleClaimContext.CreateAsync(CreateRoleClaim(role, claim), cancellationToken);
        }

        /// <summary>
        /// 移除角色声明。
        /// </summary>
        /// <param name="role">角色实例对象。</param>
        /// <param name="claim">声明实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        public override async Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }
            await RoleClaimContext.DeleteAsync(x =>
                x.RoleId == role.Id && x.ClaimType == claim.Type && x.ClaimValue == claim.Value, cancellationToken);
        }

        /// <summary>
        /// 获取所有角色。
        /// </summary>
        /// <returns>返回角色列表。</returns>
        public virtual IEnumerable<TRole> LoadRoles()
        {
            return RoleContext.Fetch();
        }

        /// <summary>
        /// 获取所有角色。
        /// </summary>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回角色列表。</returns>
        public virtual Task<IEnumerable<TRole>> LoadRolesAsync(CancellationToken cancellationToken = default)
        {
            return RoleContext.FetchAsync(cancellationToken: cancellationToken);
        }

        /// <summary>
        /// 通过ID获取角色实例。
        /// </summary>
        /// <param name="id">角色Id。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public virtual TRole FindById(int id)
        {
            return RoleContext.Find(id);
        }

        /// <summary>
        /// 通过角色名称获取角色实例。
        /// </summary>
        /// <param name="normalizedName">角色名称。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public virtual TRole FindByName(string normalizedName)
        {
            return RoleContext.Find(x => x.NormalizedName == normalizedName);
        }

        /// <summary>
        /// 添加角色。
        /// </summary>
        /// <param name="role">角色实例。</param>
        /// <returns>返回添加结果。</returns>
        public virtual IdentityResult Create(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            role.RoleLevel = GetMaxRoleLevel(role) + 1;//获取当前角色等级
            
            if (RoleContext.Create(role))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 更新用户角色。
        /// </summary>
        /// <param name="role">用户角色实例。</param>
        /// <returns>返回角色更新结果。</returns>
        public virtual IdentityResult Update(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }
            
            if (RoleContext.Update(role))
            {
                return IdentityResult.Success;
            }

            return IdentityResult.Failed(ErrorDescriber.DefaultError());
        }

        /// <summary>
        /// 通过ID获取角色实例。
        /// </summary>
        /// <param name="id">角色Id。</param>
        /// <param name="cancellationToken">取消标识。</param>
        /// <returns>返回当前角色实例对象。</returns>
        public override async Task<TRole> FindByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            if (int.TryParse(id, out var roleId))
            {
                return await FindByIdAsync(roleId, cancellationToken);
            }

            return null;
        }

        /// <summary>
        /// 角色查询实例对象。
        /// </summary>
        public override System.Linq.IQueryable<TRole> Roles { get; } = null;
    }
}