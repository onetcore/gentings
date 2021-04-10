using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Data.Initializers;
using Gentings.Data.Internal;
using Gentings.Localization;
using Gentings.Security.Roles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Gentings.Security.Data
{
    /// <summary>
    /// 用户数据初始化基类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    /// <typeparam name="TUserRole">用户角色类型。</typeparam>
    public abstract class DataInitializer<TUser, TRole, TUserRole> : IInitializer
        where TUser : UserBase, new()
        where TRole : RoleBase, new()
        where TUserRole : IUserRole, new()
    {
        private readonly List<IUserEventHandler<TUser>> _handlers;

        /// <summary>
        /// 服务提供者接口。
        /// </summary>
        protected IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// 用户数据库接口实例。
        /// </summary>
        protected IDbContext<TUser> Context { get; }

        /// <summary>
        /// 用户管理接口。
        /// </summary>
        protected IUserManager<TUser> UserManager { get; }

        /// <summary>
        /// 日志接口。
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// 本地化接口。
        /// </summary>
        protected ILocalizer Localizer { get; }

        /// <summary>
        /// 初始化类<see cref="DataInitializer{TUser, TRole, TUserRole}"/>。
        /// </summary>
        /// <param name="serviceProvider">服务提供者接口。</param>
        /// <param name="userManager">用户管理接口。</param>
        protected DataInitializer(IServiceProvider serviceProvider, IUserManager<TUser> userManager)
        {
            ServiceProvider = serviceProvider;
            Context = serviceProvider.GetRequiredService<IDbContext<TUser>>();
            UserManager = userManager;
            _handlers = serviceProvider.GetServices<IUserEventHandler<TUser>>()
                .OrderByDescending(x => x.Priority)
                .ToList();
            Logger = serviceProvider.GetRequiredService<ILogger<TUser>>();
            Localizer = ServiceProvider.GetRequiredService<ILocalizer>();
        }

        /// <summary>
        /// 优先级，越大越靠前。
        /// </summary>
        public virtual int Priority { get; } = 0;

        /// <summary>
        /// 判断是否禁用。
        /// </summary>
        /// <returns>返回判断结果。</returns>
        public virtual Task<bool> IsDisabledAsync()
        {
            return Context.AnyAsync();
        }

        /// <summary>
        /// 默认角色类型。
        /// </summary>
        protected abstract Type DefaultRolesType { get; }

        /// <summary>
        /// 判断哪些默认角色为默认添加到用户的角色，如果返回<c>true</c>，则添加用户时候会自动添加到用户中。
        /// </summary>
        /// <param name="defaultRole">默认角色枚举实例。</param>
        /// <returns>返回判断结果。</returns>
        protected abstract bool IsDefault(Enum defaultRole);

        /// <summary>
        /// 管理员账户和密码（密码会加两遍）。
        /// </summary>
        protected virtual string Administrator { get; } = "admin";

        /// <summary>
        /// 安装时候预先执行的接口。
        /// </summary>
        /// <returns>返回执行结果。</returns>
        public virtual Task<bool> ExecuteAsync()
        {
            return Context.BeginTransactionAsync(async db =>
            {
                var roles = new List<TRole>();
                var rdb = db.As<TRole>();
                foreach (Enum value in Enum.GetValues(DefaultRolesType))
                {
                    var role = new TRole();
                    role.Name = Localizer.GetString(value);
                    role.NormalizedName = role.Name.ToUpper();
                    role.RoleLevel = (int)(object)value;
                    role.IsSystem = true;//系统角色不能删除
                    role.IsDefault = IsDefault(value);//默认添加的角色
                    if (!role.IsDefault)//因为默认角色添加用户时候会自动添加
                        roles.Add(role);
                    if (!await rdb.CreateAsync(role))
                    {
                        Logger.LogCritical("添加默认角色失败：{0}", role.Name);
                        return false;
                    }
                }

                var id = await CreateAsync(db, Administrator, roles, 0);
                if (id == 0)
                {
                    Logger.LogCritical("添加用户账户失败：{0}", Administrator);
                    return false;
                }

                return await ExecuteAsync(db, roles, id);
            }, 3000);
        }

        /// <summary>
        /// 添加用户。
        /// </summary>
        /// <param name="db">当前事务实例。</param>
        /// <param name="roles">角色列表，不包含默认角色。</param>
        /// <param name="administratorId">顶级管理员用户Id。</param>
        /// <returns>返回当前用户Id。</returns>
        protected virtual Task<bool> ExecuteAsync(IDbTransactionContext<TUser> db, List<TRole> roles,
            int administratorId)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// 添加用户。
        /// </summary>
        /// <param name="db">当前事务实例。</param>
        /// <param name="userName">用户名称。</param>
        /// <param name="roles">角色列表，不包含默认角色。</param>
        /// <param name="parentId">父级Id。</param>
        /// <returns>返回当前用户Id。</returns>
        protected Task<int> CreateAsync(IDbTransactionContext<TUser> db, string userName,
            List<TRole> roles, int parentId) => CreateAsync(db, userName, userName + userName, roles, parentId);

        /// <summary>
        /// 添加用户。
        /// </summary>
        /// <param name="db">当前事务实例。</param>
        /// <param name="userName">用户名称。</param>
        /// <param name="password">密码。</param>
        /// <param name="roles">角色列表，不包含默认角色。</param>
        /// <param name="parentId">父级Id。</param>
        /// <returns>返回当前用户Id。</returns>
        protected async Task<int> CreateAsync(IDbTransactionContext<TUser> db, string userName, string password, List<TRole> roles, int parentId)
        {
            var user = new TUser();
            user.UserName = userName;
            user.PasswordHash = password;
            user.NickName = user.UserName;
            user.NormalizedUserName = UserManager.NormalizeName(user.UserName);
            user.PasswordHash = UserManager.HashPassword(user);
            user.ParentId = parentId;

            if (!await db.CreateAsync(user))
            {
                Logger.LogCritical("添加用户实例失败：{0}", userName);
                return 0;
            }

            foreach (var handler in _handlers)
            {
                if (!await handler.OnCreatedAsync(db, user))
                {
                    Logger.LogCritical("触发用户事件失败：{0}，类型：{1}", userName, handler.GetType().FullName);
                    return 0;
                }
            }

            var urdb = db.As<TUserRole>();
            foreach (var role in roles)
            {
                if (!await urdb.CreateAsync(new TUserRole { RoleId = role.Id, UserId = user.Id }))
                {
                    Logger.LogCritical("添加用户角色失败：{0}（{2}），角色：{1}（3）", userName, role.Name, user.Id, role.Id);
                    return 0;
                }
            }

            var maxRole = roles.OrderByDescending(x => x.RoleLevel).First();
            user.RoleId = maxRole.Id;
            if (await db.UpdateAsync(user.Id, new { user.RoleId }))
                return user.Id;

            Logger.LogCritical("添加用户最高角色失败：{0}，角色：{1}", userName, maxRole.Name);
            return 0;
        }
    }
}