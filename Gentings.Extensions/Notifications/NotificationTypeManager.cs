using System;
using System.Threading.Tasks;
using Gentings.Data;
using Gentings.Extensions.Categories;
using Microsoft.Extensions.Caching.Memory;

namespace Gentings.Extensions.Notifications
{
    /// <summary>
    /// 系统通知类型管理类型。
    /// </summary>
    public class NotificationTypeManager : CachableCategoryManager<NotificationType>, INotificationTypeManager
    {
        /// <summary>
        /// 初始化类<see cref="NotificationTypeManager"/>。
        /// </summary>
        /// <param name="context">数据库操作实例。</param>
        /// <param name="cache">缓存接口。</param>
        public NotificationTypeManager(IDbContext<NotificationType> context, IMemoryCache cache) : base(context, cache)
        {
        }

        /// <summary>
        /// 获取或添加通知类型。
        /// </summary>
        /// <param name="name">类型名称。</param>
        /// <param name="func">获取类型实例。</param>
        /// <returns>返回当前通知类型。</returns>
        public virtual NotificationType GetOrCreate(string name, Func<NotificationType> func = null)
        {
            var type = Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (type == null)
            {
                type = func?.Invoke() ?? new NotificationType();
                type.Name ??= name;
                if (Create(type))
                    return type;
            }

            return null;
        }

        /// <summary>
        /// 获取或添加通知类型。
        /// </summary>
        /// <param name="name">类型名称。</param>
        /// <param name="func">获取类型实例。</param>
        /// <returns>返回当前通知类型。</returns>
        public virtual async Task<NotificationType> GetOrCreateAsync(string name, Func<NotificationType> func = null)
        {
            var type = await FindAsync(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (type == null)
            {
                type = func?.Invoke() ?? new NotificationType();
                type.Name ??= name;
                if (await CreateAsync(type))
                    return type;
            }

            return null;
        }
    }
}