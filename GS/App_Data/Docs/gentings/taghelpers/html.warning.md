---
title: 设置空对象属性标签
---

# 设置空对象属性标签

这个标签主要用于在分页数据显示时候，如果数据为空显示的警告信息。

## 使用方法

```html
<tbody class="align-middle" .warning="Model.Items" .warning-text="未找到相关用户信息！">
```

参数说明：

* .warning：数据列表对象，一般为分页实例对象`IPagableEnumerable`；
* .warning-text：如果为空，显示的字符串

## 例子

```html
<table class="table table-hover">
    <thead>
        <tr>
            <th class="checkbox"><gt:checkall /></th>
            <th .sort="@UserOrderBy.UserName">名称</th>
            <th>邮件地址</th>
            <th .sort="@UserOrderBy.CreatedDate">添加时间</th>
            <th .sort="@UserOrderBy.UpdatedDate">更新时间</th>
            <th .sort="@UserOrderBy.LastLoginDate">最后登录</th>
        </tr>
    </thead>
    <tbody class="align-middle" .warning="Model.Items" .warning-text="未找到相关用户信息！">
        @foreach (var item in Model.Items)
        {
            <tr class="data-item">
                <td class="checkbox"><gt:checkbox value="@item.Id" /></td>
                <td>
                    @(item.NickName)
                </td>
                <td>@item.Email</td>
                <td>@item.CreatedDate.ToString("f")</td>
                <td>@item.UpdatedDate.ToString("f")</td>
                <td>@item.LastLoginDate?.ToString("f")</td>
            </tr>
        }
    </tbody>
</table>
```

当`Model.Items`为空时候，就会显示“**未找到相关用户信息！**”。