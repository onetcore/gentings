using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gentings.ChatServers.Controllers
{
    /// <summary>
    /// 控制器基类。
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public abstract class ControllerBase : AspNetCore.ControllerBase
    {
    }
}