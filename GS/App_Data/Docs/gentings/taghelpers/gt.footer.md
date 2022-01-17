---
title: Footer标签
---

# Footer标签

Footer标签为HTML中`body`最后中使用，主要用于引入集成的脚本库，使用方法如下：

## 使用方法

```html
<gt:footer status="" antiforgery=""></gt:footer>
```

参数说明：

* status：是否自动挂接状态消息，如果自动挂接状态消息在服务端返回状态时候，会自动显示错误或者成功消息；
* antiforgery：是否生成Antiforgery验证标记表单实例，这个主要用于Antiforgery验证，AJAX请求使用。

## 例子

一般会在布局页面中使用，参考如下：

```html
<gt:footer antiforgery="true" status="false">
    <script src="~/js/site.js" asp-append-version="true"></script>
    @await RenderSectionAsync("scripts", required: false)
</gt:footer>
```