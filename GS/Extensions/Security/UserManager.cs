﻿using Gentings;
using Gentings.Data;
using Gentings.Extensions;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 管理员管理接口。
    /// </summary>
    public interface IUserManager : IObjectManager<User>, ISingletonService
    {
        /// <summary>
        /// 获取当前登录用户。
        /// </summary>
        /// <returns>返回当前登录用户。</returns>
        User GetUser();
    }

    /// <summary>
    /// 管理员管理实现类。
    /// </summary>
    public class UserManager : ObjectManager<User>, IUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// 初始化类<see cref="User"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="contextAccessor">HTTP上下文访问实例。</param>
        public UserManager(IDbContext<User> context, IHttpContextAccessor contextAccessor) : base(context)
        {
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// 获取当前登录用户。
        /// </summary>
        /// <returns>返回当前登录用户。</returns>
        public User GetUser()
        {
            if (_contextAccessor.HttpContext == null)
                return new User();
            return Find(_contextAccessor.HttpContext.User.GetUserId());
        }
    }

}

