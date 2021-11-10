using Gentings.Data.Migrations;

namespace GS.Extensions.Security
{
    /// <summary>
    /// 数据库迁移类。
    /// </summary>
    public class UserDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<User>(table => table
               .Column(x => x.Id)
               .Column(x => x.UserName)
               .Column(x => x.Password)
               .Column(x => x.NickName)
               .Column(x => x.Email)
               .Column(x => x.CreatedDate)
               .Column(x => x.UpdatedDate)
               .Column(x => x.LastLoginDate)
               .Column(x => x.Avatar)
               .Column(x => x.LoginIP)
               .Column(x => x.ExtendProperties)
            );
        }
    }
}

