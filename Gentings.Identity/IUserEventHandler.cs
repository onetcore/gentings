﻿using System.Threading;
using System.Threading.Tasks;
using Gentings.Data.Internal;

namespace Gentings.Identity
{
    /// <summary>
    /// 用户扩展接口，主要是在添加用户时候进行得添加操作。
    /// </summary>
    public interface IUserEventHandler<TUser> : IServices
        where TUser : UserBase
    {
        /// <summary>
        /// 优先级。
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// 当用户添加后触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        bool OnCreated(IDbTransactionContext<TUser> context, TUser user);

        /// <summary>
        /// 当用户添加后触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        Task<bool> OnCreatedAsync(IDbTransactionContext<TUser> context, TUser user, CancellationToken cancellationToken = default);

        /// <summary>
        /// 当用户删除前触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        bool OnDelete(IDbTransactionContext<TUser> context, TUser user);

        /// <summary>
        /// 当用户删除前触发得方法。
        /// </summary>
        /// <param name="context">数据库事务操作实例。</param>
        /// <param name="user">用户实例。</param>
        /// <param name="cancellationToken">取消标志。</param>
        /// <returns>返回操作结果，返回<c>true</c>表示操作成功，将自动提交事务，如果<c>false</c>或发生错误，则回滚事务。</returns>
        Task<bool> OnDeleteAsync(IDbTransactionContext<TUser> context, TUser user, CancellationToken cancellationToken = default);
    }
}