using System;
using System.Threading.Tasks;
using Gentings.Extensions.Categories;

namespace Gentings.Extensions.Notifications
{
    /// <summary>
    /// 系统通知类型管理接口。
    /// </summary>
    public interface INotificationTypeManager : ICachableCategoryManager<NotificationType>, ISingletonService
    {
        /// <summary>
        /// 获取或添加通知类型。
        /// </summary>
        /// <param name="name">类型名称。</param>
        /// <param name="func">获取类型实例。</param>
        /// <returns>返回当前通知类型。</returns>
        NotificationType GetOrCreate(string name, Func<NotificationType> func = null);

        /// <summary>
        /// 获取或添加通知类型。
        /// </summary>
        /// <param name="name">类型名称。</param>
        /// <param name="func">获取类型实例。</param>
        /// <returns>返回当前通知类型。</returns>
        Task<NotificationType> GetOrCreateAsync(string name, Func<NotificationType> func = null);
    }
}