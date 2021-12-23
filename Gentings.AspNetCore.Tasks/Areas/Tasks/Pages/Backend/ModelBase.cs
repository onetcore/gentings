using Gentings.Security;

namespace Gentings.AspNetCore.Tasks.Areas.Tasks.Pages.Backend
{
    /// <summary>
    /// 模型基类，开发人员才可以看到。
    /// </summary>
    [PermissionAuthorize(CorePermissions.Developer)]
    public abstract class ModelBase : AspNetCore.ModelBase
    {
    }
}
