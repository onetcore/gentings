---
title: 数据库迁移
---

# 数据库迁移

在.NET开发中，每一一个表格对应的一个实体类，在[上一节](./entity.md)中我们定义了一个`User`实体类，这一节主要将实体类映射到数据库字段中。

## 数据库迁移代码

在EntityFramework中会自动生成迁移代码，而在Gentings中需要手动编写数据库迁移代码，当然也提供了自动生成插件，这个后面在进行介绍，具体代码如下：

```csharp
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
```

## 代码解析

数据库迁移主要是继承`DataMigration`类，这样在Gentings中会自动注册到服务容器中，具体的可以参考：[Gentings服务注册](../gentings/service.md)

从代码中就可以看出，对应于每一字段，因为使用Lambda表达式，会自动识别标注的特性和类型，解析成SQL代码，在每次应用程序启动的时候都会执行迁移代码。

## 配置数据库

现在可以建立一个数据库，并且配置数据库连接，如果没有特殊说明，我们就在本地建立一个数据库：gentings。然后在网站项目中，复制一份配置文件，命名为`appsettings.development.json`，这个主要是为了在项目开发过程中，每个成员都可以自定义自己的配置，并且在项目中忽略git选项。

添加一个`Data`节点，因为是在本地服务器的SqlServer数据库，只要设置`Name`既可，Gentings会自动配置为管道连接方式，具体代码如下：

```json
{
  "Data": {
    "Name": "gentings"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

调试一下，就可以看到数据库中的表格就会自动创建，多出的`core_Migrations`和`core_Administrators`表格，详细迁移请参考：[Gentings数据库迁移](../gentings/data/migration.md)