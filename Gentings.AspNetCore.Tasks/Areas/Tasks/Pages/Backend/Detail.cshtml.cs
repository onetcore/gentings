using Gentings.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Tasks.Areas.Tasks.Pages.Backend
{
    /// <summary>
    /// 后台服务详情。
    /// </summary>
    public class DetailModel : ModelBase
    {
        private readonly ITaskManager _taskManager;

        public DetailModel(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public TaskDescriptor Descriptor { get; private set; }

        public IActionResult OnGet(int id)
        {
            Descriptor = _taskManager.GeTask(id);
            if (Descriptor == null)
                return NotFound();
            return Page();
        }
    }
}