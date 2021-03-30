using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions;

namespace Gentings.Security.Notifications
{
    /// <summary>
    /// 通知管理实现类。
    /// </summary>
    public class NotificationManager : ObjectManager<Notification>, INotificationManager
    {
        /// <summary>
        /// 初始化类<see cref="NotificationManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        public NotificationManager(IDbContext<Notification> context) : base(context)
        {
        }

        /// <summary>
        /// 获取当前用户的最新通知。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">获取记录数。</param>
        /// <returns>返回当前用户的最新通知。</returns>
        public virtual NotificationCollection Load(int userId, int size)
        {
            var notifications = Context.AsQueryable().WithNolock()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .AsEnumerable(size);
            return new NotificationCollection(notifications);
        }

        /// <summary>
        /// 获取当前用户的最新通知。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">获取记录数。</param>
        /// <returns>返回当前用户的最新通知。</returns>
        public virtual async Task<NotificationCollection> LoadAsync(int userId, int size)
        {
            var notifications = await Context.AsQueryable().WithNolock()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Id)
                .AsEnumerableAsync(size);
            return new NotificationCollection(notifications);
        }

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <param name="notification">通知实例。</param>
        /// <param name="ids">通知Id列表。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool Create(Notification notification, int[] ids)
        {
            return Context.BeginTransaction(db =>
            {
                foreach (var id in ids)
                {
                    notification.UserId = id;
                    db.Create(notification);
                }

                return true;
            }, 300);
        }

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <param name="notification">通知实例。</param>
        /// <param name="ids">通知Id列表。</param>
        /// <returns>返回添加结果。</returns>
        public virtual Task<bool> CreateAsync(Notification notification, int[] ids)
        {
            return Context.BeginTransactionAsync(async db =>
            {
                foreach (var id in ids)
                {
                    notification.UserId = id;
                    await db.CreateAsync(notification);
                }

                return true;
            }, 300);
        }

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <typeparam name="TUserRole">用户角色关联类型。</typeparam>
        /// <param name="notification">通知实例。</param>
        /// <param name="roleId">角色Id。</param>
        /// <returns>返回添加结果。</returns>
        public virtual bool Create<TUserRole>(Notification notification, int roleId) where TUserRole : UserRoleBase
        {
            return Context.BeginTransaction(db =>
            {
                var ids = db.As<TUserRole>().AsQueryable().WithNolock()
                    .Select(x => x.UserId)
                    .Where(x => x.RoleId == roleId)
                    .AsEnumerable(x => x.GetInt32(0));
                foreach (var id in ids)
                {
                    notification.UserId = id;
                    db.Create(notification);
                }

                return true;
            }, 300);
        }

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <typeparam name="TUserRole">用户角色关联类型。</typeparam>
        /// <param name="notification">通知实例。</param>
        /// <param name="roleId">角色Id。</param>
        /// <returns>返回添加结果。</returns>
        public virtual Task<bool> CreateAsync<TUserRole>(Notification notification, int roleId) where TUserRole : UserRoleBase
        {
            return Context.BeginTransactionAsync(async db =>
            {
                var ids = await db.As<TUserRole>().AsQueryable().WithNolock()
                    .Select(x => x.UserId)
                    .Where(x => x.RoleId == roleId)
                    .AsEnumerableAsync(x => x.GetInt32(0));
                foreach (var id in ids)
                {
                    notification.UserId = id;
                    await db.CreateAsync(notification);
                }

                return true;
            }, 300);
        }
    }
}