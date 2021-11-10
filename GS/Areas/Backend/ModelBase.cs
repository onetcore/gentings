using Microsoft.AspNetCore.Authorization;

namespace GS.Areas.Backend.Pages
{
    /// <summary>
    /// 模型基类。
    /// </summary>
    [Authorize]
    public abstract class ModelBase : GS.Pages.ModelBase
    {
    }
}
