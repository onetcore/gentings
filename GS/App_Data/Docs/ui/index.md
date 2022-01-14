---
title: UI库简单介绍
---

# UI库简单介绍

在开发后台管理中，如果没有使用前后端分离，直接使用RazorPages进行项目管理开发，就会使用到Gentings中的AspNetCore集成的一些UI库，包括基于Bootstrap，jQuery等等。

## jQuery库以及扩展

在集成UI开发中，我们使用jQuery进行开发，所有脚本都是基于jQuery进行开发，在Gentings集成库中，也会对jQuery进行扩展，具体的可以参考：[jQuery扩展](./ui/js/jquery.md)。

## Bootstrap库

在集成开发UI中，所有样式都是基于Bootstrap进行重新编译，而Bootstrap的脚本库也会引用其中，更多的信息可以参考：[Bootstrap扩展](./ui/css/bootstrap.md)。

## 其他扩展的脚本

除了jQuery和Bootstrap的扩展外，还增加了Markdown的简易编辑器，文件上传等等功能，主要包含如下：

* Ajax相关功能操作
* URL基本操作
* 消息弹出
* 状态消息展示

> [!tip]
> 这些脚本库可以通过`gt:footer`标签或者`ViewConext.AddLibraries()`进行引用，当然引用了之后，配套的样式也会引入到页面中。

## 配套VS插件

在开发这些UI和样式中，我们主要使用了**JS**和**SASS**进行开发，在VS中我们使用了**Web Compiler**进行编译。

而引入脚本库直接使用VS的**libman**。