using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.SaaS.Security.Roles
{
    /// <summary>
    /// 角色声明类。
    /// </summary>
    [Table("saas_Roles_Claims")]
    public abstract class RoleClaimBase : Identity.Roles.RoleClaimBase
    {

    }
}