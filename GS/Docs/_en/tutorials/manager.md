---
title: 定义管理接口和实现类
---

# 定义管理接口和实现类

我们已经定了实体类，并且创建了数据库，现在我们需要在代码中使用这个类，在.NET6+中，我们需要创建一个接口`IUserManager`，以及接口实现类`UserManager`，然后注册到服务容器中，在进行调用，这些是.NET6+的基础操作方式，对于不熟悉这种开发方式的人，可以详细了解[微软的文档](https://docs.microsoft.com/dotnet)。

## 定义接口IUserManager

因为`User`继承了`IIdObject`接口，在Gentings中封装好了很多带有主键`Id`的数据库操作方法，所以接口我们就可以继承而来，这样对于所有具有单主键`Id`的对象快速实现管理类。

```csharp
    /// <summary>
    /// 管理员管理接口。
    /// </summary>
    public interface IUserManager : IObjectManager<User>, IUserService
    {
        /// <summary>
        /// 获取当前登录用户。
        /// </summary>
        /// <returns>返回当前登录用户。</returns>
        User GetUser();

        /// <summary>
        /// 刷新用户缓存。
        /// </summary>
        /// <param name="id">用户Id。</param>
        void Refresh(int id);
    }
```

## 实现类UserManager

```csharp
    /// <summary>
    /// 管理员管理实现类。
    /// </summary>
    public class UserManager : ObjectManager<User>, IUserManager
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// 初始化类<see cref="User"/>。
        /// </summary>
        /// <param name="context">数据库操作接口实例。</param>
        /// <param name="contextAccessor">HTTP上下文访问实例。</param>
        /// <param name="cache">缓存接口实例。</param>
        public UserManager(IDbContext<User> context, IHttpContextAccessor contextAccessor, IMemoryCache cache) : base(context)
        {
            _contextAccessor = contextAccessor;
            _cache = cache;
        }

        private string GetCacheKey(int id)
        {
            return $"users:{id}";
        }

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        public IUser GetCachedUser(int id)
        {
            return _cache.GetOrCreate(GetCacheKey(id), ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return Find(id);
            });
        }

        /// <summary>
        /// 获取缓存用户实例。
        /// </summary>
        /// <param name="id">用户Id。</param>
        /// <returns>返回缓存用户实例对象。</returns>
        public async Task<IUser> GetCachedUserAsync(int id)
        {
            return await _cache.GetOrCreateAsync(GetCacheKey(id), async ctx =>
            {
                ctx.SetDefaultAbsoluteExpiration();
                return await FindAsync(id);
            });
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

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        public override bool IsDuplicated(User model)
        {
            return Context.Any(x => (x.UserName == model.UserName || x.NickName == model.NickName) && x.Id != model.Id);
        }

        /// <summary>
        /// 判断是否重复。
        /// </summary>
        /// <param name="model">模型实例对象。</param>
        /// <returns>返回判断结果。</returns>
        /// <param name="cancellationToken">取消标识。</param>
        public override Task<bool> IsDuplicatedAsync(User model, CancellationToken cancellationToken = default)
        {
            return Context.AnyAsync(x => (x.UserName == model.UserName || x.NickName == model.NickName) && x.Id != model.Id, cancellationToken);
        }

        /// <summary>
        /// 刷新用户缓存。
        /// </summary>
        /// <param name="id">用户Id。</param>
        public void Refresh(int id)
        {
            _cache.Remove(GetCacheKey(id));
        }
    }
```

这样我们就定义好了接口和实现类型，在其他代码中就可以在构造函数中调用`IUserManager`接口了，详细扩展请参考：[Gentings单Id主键对象扩展查询](../gentings/data/idobject.md)