---
title: 定义一个实体类
---

# 定义一个实体类

在.NET开发中，每一一个表格对应的一个实体类，这是经常使用到的，所以在Gentings中也对实体类进行定义，然后对数据库进行迁移。在Gentings中采用Code First方式来进行开发，如果对数据库不是很熟悉的人员可以不需要了解数据库。在Gentings的数据库模块中已经对数据库操作进行了很友好的封装，具体的可以参考[数据库模块](../data/index.md)。

## 实体类代码

首先我们定义一个用户实体类`User`，这个在程序中经常用到的，具体代码如下：

```csharp
/// <summary>
/// 管理员。
/// </summary>
[Table("core_Administrators")]
public class User : ExtendBase, IUser
{
    /// <summary>
    /// 用户ID。
    /// </summary>
    [Identity]
    public int Id { get; set; }

    /// <summary>
    /// 用户名称。
    /// </summary>
    [Size(64)]
    [NotUpdated]
    public string? UserName { get; set; }

    /// <summary>
    /// 密码。
    /// </summary>
    [Size(128)]
    [NotUpdated]
    public string? Password { get; set; }

    /// <summary>
    /// 昵称。
    /// </summary>
    [Size(64)]
    public string? NickName { get; set; }

    /// <summary>
    /// 电子邮件。
    /// </summary>
    [Size(256)]
    public string? Email { get; set; }

    /// <summary>
    /// 添加时间。
    /// </summary>
    [NotUpdated]
    public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// 最后更新时间。
    /// </summary>
    public DateTimeOffset UpdatedDate { get; set; } = DateTimeOffset.Now;

    /// <summary>
    /// 最后登录时间。
    /// </summary>
    [NotUpdated]
    public DateTimeOffset? LastLoginDate { get; set; }

    /// <summary>
    /// 头像。
    /// </summary>
    [Size(256)]
    public string Avatar { get; set; } = "/images/avatar.png";

    /// <summary>
    /// 登录IP。
    /// </summary>
    [NotUpdated]
    [Size(20)]
    public string? LoginIP { get; set; }
}
```

## 代码解析

首先要让数据库可操作，这个类的属性必须为公有`public`，并且具有`get`和`set`功能，否则将不被数据库操作识别。一般实体类就是定义和描述一个类型的属性，在代码中，使用到的特性如下：

* TableAttribute：指定数据库中对应的表格名称；
* IdentityAttribute：表示当前`Id`字段为自增长序列，因为没有特别的指定主键，`Id`自增长字段自动设置为主键；
* SizeAttribute：表示当前字段在数据库中长度，一般字符串和其他可变长度都需要设置，如果没有设置，在SqlServer数据库中使用`MAX`，如字符串就为`nvarchar(MAX)`；
* NotUpdatedAttribute：表示在更新整个实体时候，忽略此字段。

从上面的代码可以看到，一个实体类包含了属性名称和一些特性的定义，我们这里只介绍用到的一些特性，具体的可以参考：[Gentings实体类特性](../gentings/data/attributes.md)。

> 注：如果没有特殊的命名，我们约定添加时间属性为`CreatedDate`，更新时间为`UpdatedDate`。