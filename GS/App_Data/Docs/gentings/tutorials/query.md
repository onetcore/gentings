---
title: 分页查询实现
---

# 分页查询实现

在定义好了用户`User`实体后，我们在后台管理的时候需要对用户进行分页查询，这个是在很多地方都需要使用到的，在Gentings中对分页查询也实现了简单化操作。

## 分页查询代码

在分页查询中，我们需要定义一个参数类，用于前端参数的接收，同时根据参数实现相应的条件查询，具体代码如下：

```csharp
/// <summary>
/// 用户查询实例。
/// </summary>
public class UserQuery : OrderableQueryBase<User, UserOrderBy>
{
    /// <summary>
    /// 用户名称。
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 初始化查询上下文。
    /// </summary>
    /// <param name="context">查询上下文。</param>
    protected override void Init(IQueryContext<User> context)
    {
        base.Init(context);
        if (!string.IsNullOrEmpty(Name))
            context.Where(x => x.UserName.Contains(Name) || x.NickName.Contains(Name));
    }
}
```

## 排序

如果使用Gentings集成的后台管理UI，可以很好的实现排序功能，在用户查询中，我们也定义了排序功能，在Gentings中排序需要定义一个枚举，并且这个枚举的值名称必须和数据库中的字段一样，具体代码如下：

```csharp
/// <summary>
/// 排序。
/// </summary>
public enum UserOrderBy
{
    /// <summary>
    /// 用户名称。
    /// </summary>
    UserName,
    /// <summary>
    /// 添加时间。
    /// </summary>
    CreatedDate,
    /// <summary>
    /// 更新时间。
    /// </summary>
    UpdatedDate,
    /// <summary>
    /// 最后登录时间。
    /// </summary>
    LastLoginDate,
}
```

## 分页结果

在分页查询中，我们数据库操作提供了一个`Load`或者`LoadAsync`方法，返回的结果为`IPageEnumerable<TModel>`实例，里面包含了页数，页码，总记录数，每页显示记录数，以及当前页数据列表，具体代码如下：

```csharp
/// <summary>
/// 分页迭代接口。
/// </summary>
public interface IPageEnumerable : IEnumerable
{
    /// <summary>
    /// 页码。
    /// </summary>
    int PageIndex { get; }

    /// <summary>
    /// 每页显示记录数。
    /// </summary>
    int PageSize { get; }

    /// <summary>
    /// 总记录数。
    /// </summary>
    int Size { get; }

    /// <summary>
    /// 总页数。
    /// </summary>
    int Pages { get; }
}

/// <summary>
/// 分页迭代接口。
/// </summary>
/// <typeparam name="TModel">模型类型。</typeparam>
public interface IPageEnumerable<out TModel> : IPageEnumerable, IEnumerable<TModel>
{
}
```

这里只讲简单的实现方式，详细请参考：[Gentings数据库分页查询](../gentings/data/page.md)