---
title: 开始使用
---

# 欢迎使用Gentings!

欢迎使用Gentings框架进行基于.NET6+快速开发，本框架集成了很多业务快速开放模块，以及数据库等等操作底层模块。

所有程序集分为业务逻辑以及前端管理RazorUI库程序集，如果使用Razor Pages进行项目后台管理操作，还集成了基于Bootstrap，jQuery库的后台管理UI，以及各种标签类。

## 主要功能模块

1. Gentings：核心模块
2. Gentings.Data.SqlServer：SqlServer数据库操作模块
3. Gentings.Extensions：业务逻辑扩展模块
4. Gentings.Saas：支持Saas业务逻辑模块
5. Gentings.Storages：文档存储，Excel等等文件操作模块
6. Gentings.RabbitMQ：RabbitMQ队列集成模块
7. Gentings.Security：基于Identity验证用户模块
8. Gentings.AspNetCore：使用Razor Pages开发辅助模块，提供了前端库集成等等，主要包含如下后台管理库

    * Gentings.AspNetCore.Task：后台计划任务管理模块
    * Gentings.AspNetCore.Storages：文件存储管理模块
    * Gentings.AspNetCore.SensitiveWords：敏感词汇管理模块
    * Gentings.AspNetCore.NamedStrings：字典管理模块
    * Gentings.AspNetCore.Emails：邮件管理模块
    * Gentings.AspNetCore.Events：事件管理模块
    * Gentings.AspNetCore.OpenServices：开放平台服务管理模块

9. Gentings.Utilities：其他有用的集成模块

## 项目结构

基于Gentings开发的项目结构，建议使用如下的项目结构:

> [!div class="pmb-0"]
> VSource
> 
> ├─ Project(项目文件夹)
> 
> │ ├─ Project
> 
> │ ├─ Project.Extensions
> 
> │ └─ Project.Extensions.*
> 
> ├─ Gentings(框架文件夹)
> 
> │ ├─ Gentings
> 
> │ ├─ Gentings.Data.SqlServer
> 
> │ ├─ Gentings.AspNetCore
> 
> │ ├─ Gentings.AspNetCore.*
> 
> │ ├─ Gentings.Extensions
> 
> │ ├─ Gentings.Storages
> 
> │ └─ ...
> 
> ├─ Tests(测试文件夹)
> 
> │ ├─ Project.Tests
> 
> │ ├─ Project.Extensions.Tests
> 
> │ └─ Project.Extensions.*.Tests
> 
> └─ Others(其他文件夹)

## 物理文件夹结构

源代码根目录(Repos)

> [!div class="pmb-0"]
> 本目录
> 
> ├─ Project(项目文件夹)
> 
> ├─ Gentings(框架文件夹)
> 
> └─ Others(其他文件夹)

使用上面目录树结构进行开发的优势，可以通过git更新到最新的Gentings程序集，当然在Project项目中忽略掉Gentings目录即可，这样Gentings框架和项目源代码比较独立。如果选择特定版本的Gentings，可以通过git克隆相应的版本。

## 库地址

* [gitee](https://gitee.com/ydcl/gentings)
* [github](https://github.com/onetcore/gentings)

> 注意，所有基于.NET6+开发的都可以使用本框架，默认后台不建议前后端分离，直接使用集成的Razor Pages开发，也可以使用MVC进行开发，如果没有特别说明，本文档使用的为后端RazorPages开发。