---
title: Header标签
---

# Header标签

Header标签为HTML中`head`内的标签，主要用于引入集成的样式表，以及设置标题等内容，使用方法如下：

## 使用方法

```html
<gt:header title="" keyword="" description="" import={ImportLibrary}></gt:header>
```

参数说明：

* title：标题
* keyword：关键词
* description：描述
* import：引入的样式或者脚本库，这边引入的样式库，在`footer`标签中，也会同样引入相应的脚本库。

> [!note]
> `import`只是默认引入的脚本库，在页面中可以使用`ViewContext.AddLibraries(ImportLibrary)`来增加脚本库的引入，当然如果没有集成的可以通过手动引入到页面中。

其中`ImportLibrary`默认的库有：

```csharp
/// <summary>
/// jQuery。
/// </summary>
JQuery = 1,
/// <summary>
/// Bootstrap。
/// </summary>
Bootstrap = 2,
/// <summary>
/// FontAwesome图标。
/// </summary>
FontAwesome = 4,
/// <summary>
/// gt-skin。
/// </summary>
GtSkin = 8,
/// <summary>
/// 脚本高亮渲染。
/// </summary>
Highlight = 0x10,
/// <summary>
/// 代码编辑器。
/// </summary>
CodeMirror = 0x20,
/// <summary>
/// MD简易编辑器。
/// </summary>
GtEditor = 0x40,
```

## 例子

一般会在布局页面中使用，参考如下：

```html
<gt:header title="@ViewData["Title"] - @ViewData["SiteName"]" import="Bootstrap">
    <link rel="stylesheet" href="~/GS.styles.css" asp-append-version="true" />
    @await RenderSectionAsync("header", required: false)
</gt:header>
```