using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Internal;
using Gentings.Identity.Roles;
using Gentings.Identity.Scores;

namespace Gentings.Identity
{
    /// <summary>
    /// 用户事件处理类。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    /// <typeparam name="TRole">角色类型。</typeparam>
    /// <typeparam name="TUserRole">用户角色类型。</typeparam>
    public abstract class UserEventHandler<TUser, TRole, TUserRole> : IUserEventHandler<TUser>
        where TUser : UserBase
        where TRole : RoleBase
        where TUserRole : UserRoleBase, new()
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        public virtual int Priority { get; }

        /// <summary>
        /// 当用户添加后触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        public virtual bool OnCreated(IDbTransactionContext<TUser> context, TUser user)
        {
            //添加用户角色
            var roles = context.As<TRole>().Fetch(x => x.IsDefault).ToList();
            var urdb = context.As<TUserRole>();
            foreach (var role in roles)
            {
                var userRole = new TUserRole { RoleId = role.Id, UserId = user.Id };
                urdb.Create(userRole);
            }
            //更新用户最大角色，用于显示等使用
            var maxRole = roles.OrderByDescending(x => x.RoleLevel).First();
            if (maxRole != null)
            {
                user.RoleId = maxRole.Id;
                context.Update(user.Id, new { user.RoleId });
            }
            //子账号
            if (user.ParentId > 0)
            {
                var sdb = context.As<IndexedUser>();
                context.ExecuteNonQuery($@"INSERT INTO {sdb.EntityType.Table}(UserId,IndexedId)
SELECT UserId, {user.Id} FROM {sdb.EntityType.Table} WHERE IndexedId = {user.ParentId};");
                sdb.Create(new IndexedUser { IndexedId = user.Id, UserId = user.ParentId });
            }
            //积分
            var usdb = context.As<UserScore>();
            usdb.Create(new UserScore { UserId = user.Id });

            return true;
        }

        /// <summary>
        /// 当用户添加后触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        public virtual async Task<bool> OnCreatedAsync(IDbTransactionContext<TUser> context, TUser user, CancellationToken cancellationToken = default)
        {
            //添加用户角色
            var roles = await context.As<TRole>().FetchAsync(x => x.IsDefault, cancellationToken);
            var urdb = context.As<TUserRole>();
            foreach (var role in roles)
            {
                var userRole = new TUserRole { RoleId = role.Id, UserId = user.Id };
                await urdb.CreateAsync(userRole, cancellationToken);
            }
            //更新用户最大角色，用于显示等使用
            var maxRole = roles.OrderByDescending(x => x.RoleLevel).First();
            if (maxRole != null)
            {
                user.RoleId = maxRole.Id;
                await context.UpdateAsync(user.Id, new { user.RoleId }, cancellationToken);
            }
            //子账号
            if (user.ParentId > 0)
            {
                var sdb = context.As<IndexedUser>();
                await context.ExecuteNonQueryAsync($@"INSERT INTO {sdb.EntityType.Table}(UserId,IndexedId)
SELECT UserId, {user.Id} FROM {sdb.EntityType.Table} WHERE IndexedId = {user.ParentId};", cancellationToken: cancellationToken);
                await sdb.CreateAsync(new IndexedUser { IndexedId = user.Id, UserId = user.ParentId }, cancellationToken);
            }
            //积分
            var usdb = context.As<UserScore>();
            await usdb.CreateAsync(new UserScore { UserId = user.Id }, cancellationToken);

            return true;
        }

        /// <summary>
        /// 当用户删除前触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        public virtual bool OnDelete(IDbTransactionContext<TUser> context, TUser user)
        {
            var rdb = context.As<ScoreReporter>();
            rdb.Delete(x => x.UserId == user.Id);
            var logdb = context.As<ScoreLog>();
            logdb.Delete(x => x.UserId == user.Id);
            var sdb = context.As<UserScore>();
            sdb.Delete(x => x.UserId == user.Id);
            return true;
        }

        /// <summary>
        /// 当用户删除前触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        public virtual async Task<bool> OnDeleteAsync(IDbTransactionContext<TUser> context, TUser user, CancellationToken cancellationToken = default)
        {
            var rdb = context.As<ScoreReporter>();
            await rdb.DeleteAsync(x => x.UserId == user.Id, cancellationToken);
            var logdb = context.As<ScoreLog>();
            await logdb.DeleteAsync(x => x.UserId == user.Id, cancellationToken);
            var sdb = context.As<UserScore>();
            await sdb.DeleteAsync(x => x.UserId == user.Id, cancellationToken);
            return true;
        }
    }
}