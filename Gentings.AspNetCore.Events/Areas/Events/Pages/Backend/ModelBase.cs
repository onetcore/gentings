using Gentings.Security;

namespace Gentings.AspNetCore.Events.Areas.Events.Pages.Backend
{
    /// <summary>
    /// 模型基类。
    /// </summary>
    [PermissionAuthorize]
    public abstract class ModelBase : AspNetCore.ModelBase
    {

    }
}
