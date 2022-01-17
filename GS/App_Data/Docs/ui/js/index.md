---
title: JS库全局方法介绍
---

# JS库全局方法介绍

在Gentings集成的UI库中，会很经常在前端写脚本时候使用到全局的一些方法。

## onrender方法

如果参数`render`为函数，则该方法的功能只是将函数附加到队列中，队列中的所有方法都会在在页面加载完成或者模态框加载完成时候进行调用。

```javascript
/**
 * 当呈现后执行的方法。
 * @param {Function|JQuery|undefined} render 执行的方法或者当前对象，如果为当前对象则执行所有需要执行的方法。
 */
window.onrender = function (render);
```

具体的例子如，页面执行完成后的`onclick`事件等，详细的请参考：[点击功能事件](./js/click.md)。

## $ajax方法

在没有前后端分离中，很经常使用的方法就是AJAX请求，这个方法就是对jQuery中的`POST`进行封装。

```javascript
/**
 * Ajax请求。
 * @param {string} url 请求的URL地址。
 * @param {Object} data 请求的数据对象。
 * @param {Function|undefined} success 成功后执行的方法。
 * @param {Function|undefined} error 发生错误时候执行的方法。
 * @param {string|undefined} dataType 发送数据类型：'JSON'，不设置表示表单提交。
 */
window.$ajax = function (url, data, success, error, dataType);
```

> [!warning]
> 一般的只会对上传文件，提交Form表单才会使用表单方式(使用`FormData`对象)，其他都是使用JSON格式进行提交。

## showMsg和showAlert方法

showMsg方法主要是在页面顶部中间显示状态消息，在几秒后会自动小时，一般只是一行文字，回调函数在消息“**消失**”后就会触发。

```javascript
/**
 * 显示消息。
 * @param {string|Object} msg 消息字符串。
 * @param {Number|Function} code 错误代码或者回调函数。
 * @param {Function|undefined} func 回调函数。
 */
window.showMsg = function (msg, code, func);
```

showAlert和showMsg参数两则一样，不同的是，showAlert显示的是模态框，回调函数需要在点击“**确定**”之后才触发。

```javascript
/**
 * 显示模态对话框。
 * @param {string|Object} msg 消息字符串。
 * @param {Number|Function} code 错误代码或者回调函数。
 * @param {Function|undefined} func 回调函数。
 */
window.showAlert = function (msg, code, func); 
```

参数说明：

1. msg：可以是一个消息字符串，也可以是一个消息对象，即服务端传回的`ApiResult`实例。
2. code：可以是一个数值，`0`表示成功，其他表示失败，如果为函数就是表示回调函数。
3. func：回调函数，showAlert的回调函数参数将是当前模态框实例。

> [!warning]
> 为了统一操作方式，在这里说明在AJAX请求中，凡是涉及到URL地址的，我们优先获取`href`属性值，如果不存在则是`action`属性值，一般的为`a`和`form`，如果为`button`等其他元素，就需要添加这两项中的一项，指示发送请求的URL地址。
