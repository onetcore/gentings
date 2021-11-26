using Gentings.AspNetCore;
using Gentings.Extensions.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ControllerBase = Gentings.AspNetCore.ControllerBase;

namespace GS.Controllers
{
    /// <summary>
    /// 賬戶控制器。
    /// </summary>
    [Authorize]
    [Route("api/{controller}-{action}")]
    public class SkinController : ControllerBase
    {
        /// <summary>
        /// 保存皮肤。
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Save(SkinSettings settings)
        {
            var result = await GetRequiredService<ISettingsManager>().SaveSettingsAsync(settings);
            return Json(result, Gentings.Extensions.DataAction.Updated, "皮肤");
        }
    }
}
