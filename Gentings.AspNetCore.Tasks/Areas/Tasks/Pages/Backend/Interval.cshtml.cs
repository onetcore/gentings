using Gentings.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.AspNetCore.Tasks.Areas.Tasks.Pages.Backend
{
    /// <summary>
    /// 设置后台服务时间间隔。
    /// </summary>
    public class IntervalModel : ModelBase
    {
        private readonly ITaskManager _taskManager;
        public IntervalModel(ITaskManager taskManager)
        {
            _taskManager = taskManager;
        }

        public TaskDescriptor Task { get; private set; }

        public string DefaultInterval { get; private set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public IActionResult OnGet(int id)
        {
            Task = _taskManager.GeTask(id);
            if (Task == null)
                return NotFound();
            TaskInterval interval = Task.Interval;
            DefaultInterval = interval.ToDisplayString();
            Input = new InputModel { Id = Task.Id, Interval = Task.TaskArgument.Interval ?? Task.Interval };
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            TaskInterval interval = null;
            if (!string.IsNullOrEmpty(Input.Interval))
            {
                try
                {
                    interval = Input.Interval;
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Input.Interval", "时间间隔格式错误，请输入正确的格式！");
                    return Error();
                }
            }

            Task = _taskManager.GeTask(Input.Id);
            if (await _taskManager.SaveArgumentIntervalAsync(Input.Id, interval?.ToString()))
            {
                interval = Task.Interval;
                await LogAsync("将后台服务 {2} 的时间间隔由 {0} 修改为 {1}。", Task.ToIntervalDisplayString(), interval.ToDisplayString(),
                    Task.Name);
                return Success("你已经成功更改了时间间隔！");
            }
            return Error();
        }

        public class InputModel
        {
            public int Id { get; set; }

            public string Interval { get; set; }
        }
    }
}