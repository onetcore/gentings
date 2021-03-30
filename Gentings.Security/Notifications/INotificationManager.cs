using System.Threading.Tasks;
using Gentings.Extensions;

namespace Gentings.Security.Notifications
{
    /// <summary>
    /// 通知管理接口。
    /// </summary>
    public interface INotificationManager : IObjectManager<Notification>
    {
        /// <summary>
        /// 获取当前用户的最新通知。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">获取记录数。</param>
        /// <returns>返回当前用户的最新通知。</returns>
        NotificationCollection Load(int userId, int size);

        /// <summary>
        /// 获取当前用户的最新通知。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">获取记录数。</param>
        /// <returns>返回当前用户的最新通知。</returns>
        Task<NotificationCollection> LoadAsync(int userId, int size);

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <param name="notification">通知实例。</param>
        /// <param name="ids">通知Id列表。</param>
        /// <returns>返回添加结果。</returns>
        bool Create(Notification notification, int[] ids);

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <param name="notification">通知实例。</param>
        /// <param name="ids">通知Id列表。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateAsync(Notification notification, int[] ids);

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <typeparam name="TUserRole">用户角色关联类型。</typeparam>
        /// <param name="notification">通知实例。</param>
        /// <param name="roleId">角色Id。</param>
        /// <returns>返回添加结果。</returns>
        bool Create<TUserRole>(Notification notification, int roleId)
            where TUserRole : UserRoleBase;

        /// <summary>
        /// 添加通知。
        /// </summary>
        /// <typeparam name="TUserRole">用户角色关联类型。</typeparam>
        /// <param name="notification">通知实例。</param>
        /// <param name="roleId">角色Id。</param>
        /// <returns>返回添加结果。</returns>
        Task<bool> CreateAsync<TUserRole>(Notification notification, int roleId)
            where TUserRole : UserRoleBase;
    }
}