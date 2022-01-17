---
title: 设置按钮样式标签
---

# 设置按钮样式标签

在Gentings开发的UI中，集成了Bootstrap样式，设置按钮标签就是设置Bootstrap的按钮样式。

## 使用方法

```html
<a .type={ButtonType} .outline={ButtonType}>...</a>
```

或者按钮标签为：

```html
<button .type={ButtonType} .outline={ButtonType}>...</button>
```

参数说明：

* .type：按钮类型
* .outline：按钮类型

> [!warning]
> `.type`和`.outline`属性只能使用一个，如果同时使用只能使用`.type`属性的样式。

## 例子

```html
<button .type="Primary">...</button>
```

其中`ButtonType`枚举如下：

```csharp
/// <summary>
/// 主色调。
/// </summary>
Primary,
/// <summary>
/// 副色调。
/// </summary>
Secondary,
/// <summary>
/// 成功。
/// </summary>
Success,
/// <summary>
/// 错误。
/// </summary>
Danger,
/// <summary>
/// 警告。
/// </summary>
Warning,
/// <summary>
/// 消息。
/// </summary>
Info,
/// <summary>
/// 亮色。
/// </summary>
Light,
/// <summary>
/// 暗色。
/// </summary>
Dark,
/// <summary>
/// 白色。
/// </summary>
White,
```
