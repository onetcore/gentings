using Gentings.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Tasks.Areas.Tasks.Pages.Backend
{
    /// <summary>
    /// 后台服务列表。
    /// </summary>
    public class IndexModel : ModelBase
    {
        private readonly ITaskManager _taskManager;

        public IndexModel(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public IEnumerable<TaskDescriptor> Tasks { get; private set; }

        public async Task OnGetAsync()
        {
            Tasks = await _taskManager.LoadTasksAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var tasks = await _taskManager.LoadTasksAsync();
            return Success(tasks.Select(x => new
            {
                x.Id,
                LastExecuted = x.LastExecuted.ToNormalString(),
                NextExecuting = x.NextExecuting < DateTime.Now ? null : x.NextExecuting.ToNormalString()
            }));
        }
    }
}