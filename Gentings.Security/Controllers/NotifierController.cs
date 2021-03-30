using System.Threading.Tasks;
using Gentings.Security.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.Security.Controllers
{
    /// <summary>
    /// 通知控制器。
    /// </summary>
    [Authorize]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class NotifierController : AspNetCore.ControllerBase
    {
        private readonly INotificationManager _notificationManager;
        /// <summary>
        /// 初始化类<see cref="NotifierController"/>。
        /// </summary>
        /// <param name="notificationManager">通知管理接口。</param>
        public NotifierController(INotificationManager notificationManager)
        {
            _notificationManager = notificationManager;
        }

        /// <summary>
        /// 获取通知。
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var notifications = await _notificationManager.LoadAsync(UserId, 10);
            return OkResult(notifications);
        }

        /// <summary>
        /// 确认。
        /// </summary>
        /// <param name="id">通知Id。</param>
        [HttpPost]
        public async Task<IActionResult> Confirmed(int id)
        {
            await _notificationManager.UpdateAsync(id, new { Status = NotificationStatus.Confirmed });
            return OkResult();
        }

        /// <summary>
        /// 清空当前用户的通知。
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            await _notificationManager.DeleteAsync(x => x.UserId == UserId);
            return OkResult();
        }
    }
}