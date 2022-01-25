---
title: jQuery扩展
---

# jQuery扩展

在UI的脚本集成开发中，主要对jQuery的几个方法进行了扩展。

## json方法

主要是获取或设置当前元素以"_ajax."开头的属性值，这些属性值主要是为了AJAX操作，用于发送到服务端的对象，具体方法和参数如下：

```javascript
/**
 * 获取或设置当前元素所有以“_ajax.”开头的所有属性值对象。
 * @param {string} name 属性名称，不包含“_ajax.”。
 * @param {object} value 属性值。
 * @returns 返回当前元素所有以“_ajax.”开头的所有属性值对象。
 */
json: function (name, value);
```

其中：如果`value`不设置则表示返回。

## target方法

在js开发中，需要很多元素指向目标对象的选择器，我们使用`_target`作为元素属性。

```javascript
/**
 * 事件指向的对象。
 * @returns 返回事件指向的对象。
 */
target: function ();
```

> 如果目标对象只是当前对象的子元素中，可以使用`>`来表示，否则对象将对全页面元素进行查找。

## dset方法

这个方法只是对jQuery的`data`方法进行封装，第二个参数需要是一个方法，如果元素中不存在对象，则就需要查找，即获取或者添加一个数据对象到元素中。

```javascript
/**
 * 获取或者设置数据对象。
 * @param {string} key 数据唯一键。
 * @param {Function} func 如果不存在获取对象的方法。
 * @returns 返回对象实例。
 */
dset: function (key, func);
```

## exec方法

这个方法是对jQuery的`each`进行了封装，如果需要过滤当前数组的的元素，可以在元素上添加`no-js`样式。而回调函数的参数，就是当前元素的jQuery对象。

```javascript
/**
 * 回调不包含no-js得所有元素。
 * @param {Function} callback 回调函数。
 */
exec: function (callback);
```

这个方法一般会配上Lambda表达式进行使用，代码如下：

```javascript
$('.item').exec(current=>{
	// 操作代码
});
```

## disabled和enabled方法

这两个方法主要是对元素的`disabled`属性进行操作，几个添加一个移除，同时也会对元素的样式`disabled`进行相应的添加和移除操作。

## ajaxSubmit方法

使用Ajax方法提交Form表单元素，当前调用的元素必须是Form，否则将会抛出错误。

```javascript
/**
 * Ajax提交表单元素。
 * @param {Function|undefined} success 成功后执行的方法。
 * @param {Function|undefined} error 错误后执行的方法。
 */
ajaxSubmit: function (success, error);
```

## loadModal方法

加载当前元素指定的模态框，当前元素需要包含`href`或者`action`属性，用来执行要加载的URL地址，当然在请求时候会自动将参数`query`以及元素中以`_ajax.`开头的属性值附加到URL查询字符串中。

```javascript
/**
 * 加载当前元素指定的模态框。
 * @param {object} query 查询字符串对象。
 * */
loadModal: function (query);
```

> jQuery元素属性的扩展，主要是为了更好的为集成其他的功能进行扩展，当然也可以在前端页面的`@section scripts`中进行调用。