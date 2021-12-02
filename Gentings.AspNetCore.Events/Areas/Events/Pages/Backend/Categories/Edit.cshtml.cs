using Gentings.Extensions.Events;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Events.Areas.Events.Pages.Backend.Categories
{
    /// <summary>
    /// 编辑事件类型。
    /// </summary>
    public class EditModel : ModelBase
    {
        private readonly IEventManager _eventManager;

        /// <summary>
        /// 初始化类<see cref="EditModel"/>。
        /// </summary>
        /// <param name="eventManager">事件管理接口。</param>
        public EditModel(IEventManager eventManager)
        {
            _eventManager = eventManager;
        }

        /// <summary>
        /// 事件类型实例。
        /// </summary>
        [BindProperty]
        public EventType Input { get; set; }

        /// <summary>
        /// 获取电子邮件配置。
        /// </summary>
        /// <param name="id">配置Id。</param>
        public void OnGet(int id)
        {
            Input = _eventManager.GetEventType(id);
        }

        /// <summary>
        /// 保存电子邮件配置。
        /// </summary>
        public IActionResult OnPost()
        {
            var valid = true;
            if (string.IsNullOrEmpty(Input.Name))
            {
                valid = false;
                ModelState.AddModelError("Input.Name", "名称不能为空！");
            }
            if (valid)
            {
                if (_eventManager.Update(Input))
                {
                    Log("修改了事件类型：“{0}”！", Input.Name);
                    return Success("你已经成功事件类型！");
                }

                return Error("更改事件类型失败！");
            }

            return Error();
        }
    }
}