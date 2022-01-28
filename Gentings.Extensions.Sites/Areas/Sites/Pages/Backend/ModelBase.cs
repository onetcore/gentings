using Gentings.Security;

namespace Gentings.Extensions.Sites.Areas.Sites.Pages.Backend
{
    /// <summary>
    /// 模型基类。
    /// </summary>
    [PermissionAuthorize(SitePermissions.ViewPages)]
    public abstract class ModelBase : Sites.Pages.ModelBase
    {

    }
}
