---
title: URL辅助操作
---

# URL辅助操作

URL辅助方法，所有方法都是对URL进行操作。

## URL.appendQuery

将查询对象附加到URL地址中，具体代码如下：

```javascript
/**
 * 将查询字符串附加到URL地址后面。
 * @param {string} url URL地址。
 * @param {object} query 查询对象。
 */
URL.appendQuery = function (url, query);
```

## URL.toSearch

将对象组合后格式化为搜索字符串，以“?”开头，具体代码如下：

```javascript
 /**
  * 将对象组合后格式化为搜索字符串，以“?”开头。
  * @param {Object|string} search 搜索字符串或者对象。
  * @param {Object} query 修改的搜索字符串对象。
  * @returns {string} 以"?"开头的字符串。
  */
URL.toSearch = function (search, query);
```

## URL.parseQuery

匹配查询字符串，并且返回查询对象实例。

```javascript
/**
 * 匹配查询字符串。
 * @param {string} search 查询字符串。
 * @returns {object} 返回查询对象实例。
 */
URL.parseQuery = function (search);
```

## URL.parse

将字符串解析，并且返回URL对象实例。

```javascript
/**
 * 匹配URL地址。
 * @param {string} url URL地址。
 * @returns {object} 返回地址对象实例。
 */
URL.parse = function (url);
```

返回的URL对象实例如下：

* source: 原URL地址；
* protocol: 协议；
* host: 主机域名；
* port: 端口；
* query: 查询字符串；
* params: 查询对象键值对列表；
* file: 文件名称；
* hash: 哈希值；
* path: 路径；
* relative: 相对路径；
* segments: 每个路径片段；

以上是为了辅助更好的对网址进行操作而扩展的方法。