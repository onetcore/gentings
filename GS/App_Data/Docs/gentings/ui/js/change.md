---
title: onchange功能事件
---

# onchange功能事件

onchange事件和onclick事件的功能是一样的，只是onchange事件相对较少。

## modal

元素属性值改变后，根据配置模态框的地址，弹出模态框。

## ajax

元素属性值改变后，根据`_ajax.`和URL地址配置，发送到服务端，如数据的删除操作等。

在元素中，如果需要“**确认**”功能，可以对元素配置`_confirm`属性，它的属性值就是弹出的内容。

> [!tip]
> 如果在点击后需要阻止事件冒泡或者防止默认功能触发，可以在值中加上：`:prevent`和`:stop`，两者可以同时存在。