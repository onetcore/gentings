using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Internal;

namespace Gentings.Security.Scores
{
    /// <summary>
    /// 用户添加时候增加积分实例。
    /// </summary>
    /// <typeparam name="TUser">用户类型。</typeparam>
    public abstract class UserEventHandler<TUser> : IUserEventHandler<TUser>
        where TUser : UserBase
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
            //积分
            var usdb = context.As<UserScore>();
            return usdb.Create(new UserScore { UserId = user.Id });
        }

        /// <summary>
        /// 当用户添加后触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        public virtual Task<bool> OnCreatedAsync(IDbTransactionContext<TUser> context, TUser user, CancellationToken cancellationToken = default)
        {
            //积分
            var usdb = context.As<UserScore>();
            return usdb.CreateAsync(new UserScore { UserId = user.Id }, cancellationToken);
        }

        /// <summary>
        /// 当用户删除前触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        public virtual bool OnDelete(IDbTransactionContext<TUser> context, TUser user)
        {
            var sdb = context.As<UserScore>();
            return sdb.Delete(x => x.UserId == user.Id);
        }

        /// <summary>
        /// 当用户删除前触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        public virtual Task<bool> OnDeleteAsync(IDbTransactionContext<TUser> context, TUser user, CancellationToken cancellationToken = default)
        {
            var sdb = context.As<UserScore>();
            return sdb.DeleteAsync(x => x.UserId == user.Id, cancellationToken);
        }
    }
}