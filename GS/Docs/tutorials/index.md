---
title: 新建项目添加服务
---

# 新建项目添加服务

在.NET开发中，要使用Gentings框架，需要按照前面介绍的项目结构，以及目录结构，将`Gentings`程序集引用到项目当中，并且在项目启动程序`Program.cs`文件中注册服务和管道。

## 注册服务

一般我们会在程序启动的时候将我们项目中使用到的接口实现组件注册到服务中，在Gentings中也一样，代码很简单：

```csharp
using Gentings;
using Gentings.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGentings(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseGentings(builder.Configuration);
app.MapRazorPages();
app.MapControllers();
app.Run();
```

## 代码解析

在代码中我们可以看到添加了代码：

```csharp
// Add services to the container.
builder.Services.AddGentings(builder.Configuration);
```

以及在授权验证后面添加了代码：

```csharp
app.UseGentings(builder.Configuration);
```

这样就可以实现Gentings自带的服务注册系统运行起来，其中包括了后台任务等等，通常我们还会在这个地方添加数据库。

> 注：如果没有特殊说明，在本文档中的例子和代码就是Gentings项目的的GS项目中的代码。