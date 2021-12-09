using Microsoft.AspNetCore.Authorization;

namespace Gentings.AspNetCore.OpenServices.Areas.OpenServices.Pages.Backend
{
    /// <summary>
    /// 模型基类。
    /// </summary>
    [Authorize]
    public abstract class ModelBase : AspNetCore.ModelBase
    {

    }
}
