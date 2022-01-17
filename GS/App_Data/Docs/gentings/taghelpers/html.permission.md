---
title: 设置权限".permission"标签
---

# 设置权限".permission"标签

这个标签主要用于设置当前用户是否具有权限，如果有权限则显示当前元素，如果没有权限则隐藏此元素，主要使用在后台的操作按钮等等。

## 使用方法

```html
<a .permission="PermissionName">...</a>
```

参数说明：

* .permission：权限名称。

> 使用此标记需要配合`IPermissionAuthorizationService`接口使用，默认都是有权限的，如果使用`Gentings.Security`程序集，则就会判断当前登录用户的权限值。