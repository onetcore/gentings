---
title: TagHelper介绍
---

# TagHelper介绍

TagHelper是.NET6+中Razor Pages开发必不可少的标签，可以将它看成是Asp.Net以前WebForm开发的控件，但是他只是作为HTML标签呈现的封装。
在Razor Pages中可以像HTML标签一样使用，为了不和其他项目的标签重名，在Gentings中所有自定义的标签都以`gt:`开头，而在通用的标签中使用`.`开头的属性来表示标签。

## TagHelperBase基类

所有标签都继承自`TagHelperBase`类型，其中只包含了三个方法，具体代码如下：

```csharp
/// <summary>
/// 访问并呈现当前标签实例。
/// </summary>
/// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
/// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
public override void Process(TagHelperContext context, TagHelperOutput output);

/// <summary>
/// 初始化当前标签上下文。
/// </summary>
/// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
public override void Init(TagHelperContext context);

/// <summary>
/// 异步访问并呈现当前标签实例。
/// </summary>
/// <param name="context">当前HTML标签上下文，包含当前HTML相关信息。</param>
/// <param name="output">当前标签输出实例，用于呈现标签相关信息。</param>
public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output);
```

## ViewContextableTagHelperBase基类

可访问视图上下文的标签基类，同时也根据视图上下文扩展了一些有用的属性和方法，具体代码如下：

```csharp
/// <summary>
/// 试图上下文。
/// </summary>
public ViewContext ViewContext { get; set; }
        
/// <summary>
/// HTTP上下文实例。
/// </summary>
protected HttpContext HttpContext { get; }

/// <summary>
/// 获取当前文档的计数器。
/// </summary>
/// <returns>返回计数器值。</returns>
protected int GetCounter();

/// <summary>
/// 日志接口。
/// </summary>
protected virtual ILogger Logger { get; }
        
/// <summary>
/// 获取注册的服务对象。
/// </summary>
/// <typeparam name="TService">服务类型或者接口。</typeparam>
/// <returns>返回当前服务的实例对象。</returns>
protected TService? GetService<TService>();
        
/// <summary>
/// 获取已经注册的服务对象。
/// </summary>
/// <typeparam name="TService">服务类型或者接口。</typeparam>
/// <returns>返回当前服务的实例对象。</returns>
protected TService GetRequiredService<TService>() where TService : notnull;

/// <summary>
/// 本地化接口。
/// </summary>
public ILocalizer Localizer { get; }
```

## 其他扩展

为了更好的，更方便的扩展标签基类，我们还扩展了一些方法，具体的代码如下：

```csharp
/// <summary>
/// 添加子元素。
/// </summary>
/// <param name="builder">当前标签构建实例。</param>
/// <param name="tagName">子元素名称。</param>
/// <param name="action">子元素配置方法。</param>
/// <returns>返回当前标签实例。</returns>
public static TagBuilder AppendTag(this TagBuilder builder, string tagName, Action<TagBuilder> action);

/// <summary>
/// 讲当前输出设置为<paramref name="builder"/>元素实例。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="builder">构建实例对象。</param>
public static void Render(this TagHelperOutput output, TagBuilder builder);

/// <summary>
/// 讲当前输出设置为<paramref name="action"/>元素实例。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="tagName">标签名称。</param>
/// <param name="action">构建实例对象。</param>
public static void Render(this TagHelperOutput output, string tagName, Action<TagBuilder> action);

/// <summary>
/// 讲当前输出设置为<paramref name="action"/>元素实例。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="tagName">标签名称。</param>
/// <param name="action">构建实例对象。</param>
public static async Task RenderAsync(this TagHelperOutput output, string tagName, Func<TagBuilder, Task> action);

/// <summary>
/// 附加HTML内容。
/// </summary>
/// <param name="output">当前输出实例。</param>
/// <param name="htmlContent">HTML内容。</param>
public static void AppendHtml(this TagHelperOutput output, IHtmlContent htmlContent);

/// <summary>
/// 附加HTML内容。
/// </summary>
/// <param name="output">当前输出实例。</param>
/// <param name="encoded">HTML内容。</param>
public static void AppendHtml(this TagHelperOutput output, string encoded);

/// <summary>
/// 附加HTML标签。
/// </summary>
/// <param name="output">当前输出实例。</param>
/// <param name="tagName">标签名称。</param>
/// <param name="action">HTML标签实例化方法。</param>
public static void AppendHtml(this TagHelperOutput output, string tagName, Action<TagBuilder> action);

/// <summary>
/// 获取属性值。
/// </summary>
/// <param name="output">当前输出实例。</param>
/// <param name="attributeName">属性名称。</param>
/// <returns>返回当前属性值。</returns>
public static string? GetAttribute(this TagHelperOutput output, string attributeName);

/// <summary>
/// 添加样式。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="classNames">样式列表。</param>
public static void AddClass(this TagHelperOutput output, params string[] classNames);

/// <summary>
/// 移除样式。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="className">样式表。</param>
public static void RemoveClass(this TagHelperOutput output, string className);

/// <summary>
/// 添加样式。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="name">属性名称。</param>
/// <param name="value">属性值。</param>
public static void SetAttribute(this TagHelperOutput output, string name, string value);

/// <summary>
/// 添加MarkDown编辑器按钮。
/// </summary>
/// <returns>返回当前工具栏标签实例。</returns>
/// <param name="builder">当前工具栏标签实例。</param>
/// <param name="key">功能键。</param>
/// <param name="icon">图标。</param>
/// <param name="title">标题。</param>
public static TagBuilder AddSyntax(this TagBuilder builder, string key, string icon, string title);

/// <summary>
/// 附加样式文件引用。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="path">样式文件路径。</param>
public static void AppendStyle(this TagHelperOutput output, string path);

/// <summary>
/// 附加脚本文件引用。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="path">脚本文件路径。</param>
public static void AppendScript(this TagHelperOutput output, string path);

/// <summary>
/// 附加样式文件引用。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="path">样式文件路径。</param>
/// <param name="isDevelopment">是否为开发版本。</param>
public static void AppendStyle(this TagHelperOutput output, string path, bool isDevelopment);

/// <summary>
/// 附加脚本文件引用。
/// </summary>
/// <param name="output">输出实例对象。</param>
/// <param name="path">脚本文件路径。</param>
/// <param name="isDevelopment">是否为开发版本。</param>
public static void AppendScript(this TagHelperOutput output, string path, bool isDevelopment);
```

> [!warning]
> 为了统一使用，我们对于标签的使用，不管属性还是标签名称，都是要小写，如：`gt:header`, `gt:footer`, `.permission`等等。