/**
 * 绑定元素事件，自定义事件以'@'开头。
 * @param {string} eventName 事件名称。
 * @param {function} listener 绑定事件方法。
 * @param {Object} options 绑定事件选项。
 * @returns {Element} 当前元素实例。
 */
EventTarget.prototype.on = function (eventName, listener, options) {
    //不是一次性调用的系统事件都缓存起来，可以防止重复绑定事件
    if (!(options && options.once)) {
        if (typeof this.__events__ === 'undefined') this.__events__ = [];
        for (let i = 0; i < this.__events__.length; i++) {
            let event = this.__events__[i];
            //已经存在事件
            if (event.name === eventName && event.data.listener === listener)
                return this;
        }
        // 缓存事件
        this.__events__.push({ name: eventName, data: { listener: listener, options: options } });
    }
    //以@开头的为自定义事件，无需添加事件监听，需要用户触发
    if (!eventName.startsWith('@'))
        this.addEventListener(eventName, listener, options);
    return this;
};

/**
 * 绑定元素列表事件，自定义事件以'@'开头。
 * @param {string} eventName 事件名称。
 * @param {function} listener 绑定事件方法。
 * @param {Object} options 绑定事件选项。
 * @returns {Element} 当前元素列表实例。
 */
NodeList.prototype.on = function (eventName, listener, options) {
    this.forEach(item => item.on(eventName, listener, options));
    return this;
};

/**
 * 绑定元素事件，执行一次后会自动销毁，不对其提供监听是否只添加一次。
 * @param {string} eventName 事件名称。
 * @param {function} listener 绑定事件方法。
 * @returns {Element} 当前元素实例。
 */
EventTarget.prototype.once = function (eventName, listener) {
    return this.on(eventName, listener, { once: true });
};

/**
 * 绑定元素列表事件，执行一次后会自动销毁，不对其提供监听是否只添加一次。
 * @param {string} eventName 事件名称。
 * @param {function} listener 绑定事件方法。
 * @returns {NodeList} 当前元素列表实例。
 */
NodeList.prototype.once = function (eventName, listener) {
    return this.on(eventName, listener, { once: true });
};

/**
 * 移除元素事件。
 * @param {string} eventName 事件名称。
 * @param {function|undefined} listener 绑定事件方法，如果没有设置，将移除所有指定的事件。
 * @returns {Element} 当前元素实例。
 */
EventTarget.prototype.off = function (eventName, listener) {
    if (typeof this.__events__ === 'undefined' || this.__events__.length == 0) return this;
    const isAll = typeof listener === 'undefined';
    for (let i = 0; i < this.__events__.length; i++) {
        const event = this.__events__[i];
        //已经存在事件
        if (event.name === eventName && (isAll || event.data.listener === listener)) {
            this.__events__.splice(i, 1);//移除数组中的缓存事件实例
            if (!event.name.startsWith('@'))
                this.removeEventListener(event.name, event.listener);
            if (isAll)//如果是全部，需要遍历事件
                continue;
            return this;
        }
    }
    return this;
};

/**
 * 移除元素列表事件。
 * @param {string} eventName 事件名称。
 * @param {function|undefined} listener 绑定事件方法，如果没有设置，将移除所有指定的事件。
 * @returns {NodeList} 当前元素列表实例。
 */
NodeList.prototype.off = function (eventName, listener) {
    this.forEach(item => item.off(eventName, listener));
    return this;
};

/**
 * 触发元素自定义事件。
 * @param {string} eventName 事件名称，如果没有'@'开头，将会自动添加。
 * @returns {Element} 当前元素实例。
 */
EventTarget.prototype.trigger = function (eventName) {
    if (typeof this.__events__ === 'undefined' || this.__events__.length == 0) return this;
    if (!eventName.startsWith('@')) eventName = '@' + eventName;
    for (let i = 0; i < this.__events__.length; i++) {
        const current = this.__events__[i];
        //查找事件函数
        if (current.name === eventName) {
            var args = Array.prototype.slice.call(arguments, 1);
            current.data.listener.apply(this, args);
            if (current.data.options && current.data.options.once)
                this.__events__.splice(i, 1);//移除数组中的缓存事件实例
            return this;
        }
    }
    return this;
};

/**
 * 触发元素列表自定义事件。
 * @param {string} eventName 事件名称，如果没有'@'开头，将会自动添加。
 * @returns {NodeList} 当前元素列表实例。
 */
NodeList.prototype.trigger = function (eventName) {
    var args = Array.prototype.slice.call(arguments, 1);
    this.forEach(item => item.trigger(eventName, args));
    return this;
};

/**
 * 查找元素或者元素列表。
 * @param {string} selector 选择器。
 * @returns {NodeList} 返回找到的元素列表。
 */
Node.prototype.find = function (selector) {
    return this.querySelectorAll(selector);
};

/**
 * 获取，设置或者移除元素中的属性。
 * @param {string} name 属性名称。
 * @param {string} value 属性值，如果为null, false, undefined, ''等表示删除当前元素属性，不设置表示获取当前元素值，其他为设置的值。
 * @returns {Element|string} 返回当前元素或者返回的属性值。
 */
Element.prototype.attr = function (name, value) {
    if (typeof value === 'undefined') {
        value = this.getAttribute(name);
        if (value)
            return value.trim();
        return null;
    }
    if (value) this.setAttribute(name, value === true ? '' : value);
    else this.removeAttribute(name);
    return this;
};

/**
 * 获取，设置或者移除元素列表中的属性。
 * @param {string} name 属性名称。
 * @param {string} value 属性值，如果为null则表示删除当前元素，undefined表示获取当前元素值。
 * @returns {NodeList|Array} 返回当前元素列表或者返回的属性值列表。
 */
NodeList.prototype.attr = function (name, value) {
    if (typeof value === 'undefined') {
        value = [];
        this.forEach(item => value.push(item.attr(name)));
        return value;
    }
    this.forEach(node => node.attr(name, value));
    return this;
};

/**
 * 获取所有prefix开头的属性。
 * @param {string} prefix 属性开头字符串。
 * @returns {Object} 返回属性键(不包含prefix指定的字符串)值对对象。
 */
Element.prototype.attrs = function (prefix = 'attr.') {
    let attrs = {};
    for (let i = 0; i < this.attributes.length; i++) {
        const attr = this.attributes[i];
        if (attr.name.startsWith(prefix) && attr.value)
            attrs[attr.name.substr(prefix.length)] = attr.value.trim();
    }
    return attrs;
};

/**
 * 获取或者设置元素缓存实例。
 * @param {string} key 缓存键，如果不设置则返回当前所有缓存。
 * @param {Object|function} func 缓存实例或者获取缓存实例的代理方法。
 * @returns {Element|Object} 返回当前元素或者缓存实例。
 */
Element.prototype.data = function (key, func) {
    if (typeof this.__data__ === 'undefined') this.__data__ = {};
    if (typeof func === 'undefined')
        return key ? this.__data__[key] : this.__data__;
    if (typeof func === 'function')
        func = func(this);
    this.__data__[key] = func;
    return this;
};

/**
 * 获取文档标题。
 */
export function getTitle() {
    return document.title;
}

/**
 * 设置文档标题。
 */
export function setTitle(value) {
    document.title = value;
}

/**
 * 设置元素属性。
 * @param {string} selector 元素选择器。
 * @param {string} name 属性名称。
 * @param {any} value 属性值。
 */
export function setAttribute(selector, name, value) {
    document.find(selector).attr(name, value);
}

/**
 * 获取元素属性。
 * @param {string} selector 元素选择器。
 * @param {string} name 属性名称。
 */
export function getAttribute(selector, name) {
    document.find(selector).attr(name);
}

/**
 * 设置元素样式。
 * @param {string} selector 元素选择器。
 * @param {string} className 样式名称。
 */
export function addClass(selector, className) {
    document.find(selector).forEach(item => item.classList.add(className));
}

/**
 * 删除元素样式。
 * @param {string} selector 元素选择器。
 * @param {string} className 样式名称。
 */
export function removeClass(selector, className) {
    document.find(selector).forEach(item => item.classList.remove(className));
}

/**
 * 替换元素样式。
 * @param {string} selector 元素选择器。
 * @param {string} className 样式名称。
 */
export function toggleClass(selector, className) {
    document.find(selector).forEach(item => item.classList.toggle(className));
}

/**
 * 判断元素是否包含样式。
 * @param {string} selector 元素选择器。
 * @param {string} className 样式名称。
 * @return {boolean} 是否包含。
 */
export function hasClass(selector, className) {
    return document.querySelector(selector).classList.contains(className);
}

/**
 * 获取本地存储。
 * @param {string} key 存储唯一键。
 */
export function getLocalStorage(key) {
    return window.localStorage.getItem(key);
}

/**
 * 设置本地存储。
 * @param {string} key 存储唯一键。
 * @param {string} value 存储字符串，如果为空则移除本地存储。
 */
export function setLocalStorage(key, value) {
    if (value)
        window.localStorage.setItem(key, value);
    else
        window.localStorage.removeItem(key);
}

/**
 * 获取本地Session。
 * @param {string} key 存储唯一键。
 */
export function getSessionStorage(key) {
    return window.sessionStorage.getItem(key);
}

/**
 * 设置本地Session。
 * @param {string} key 存储唯一键。
 * @param {string} value 存储字符串，如果为空则移除本地存储。
 */
export function setSessionStorage(key, value) {
    if (value)
        window.sessionStorage.setItem(key, value);
    else
        window.sessionStorage.removeItem(key);
}

/**
 * 加载完成执行的方法。
 * @param {boolean} layout 是否为布局页。
 */
export function onload(layout) {
    document.querySelectorAll('.nav-group>.nav-link').forEach(item => {
        item.on('click',
            function(event) {
                this.classList.toggle('active');
                event.stopPropagation();
                event.preventDefault();
            });
    });
    document.querySelectorAll('[_click]').forEach(element => {
        const type = element.getAttribute('_click');
        let target = element.getAttribute('_click.' + type + ':target') || element.getAttribute('_click:target');
        if (!target) target = element;
        else target = document.querySelector(target);
        if (!target) return;
        if (type === 'offcanvas') {
            element.addEventListener('click',
                function (event) {
                    var offcanvas = new bootstrap.Offcanvas(target);
                    offcanvas.show();
                    event.preventDefault();
                    event.stopPropagation();
                });
        } else {
            const preventDefault = element.hasAttribute('_click.' + type + ':preventDefault') || element.hasAttribute('_click:preventDefault');
            const stopPropagation = element.hasAttribute('_click.' + type + ':stopPropagation') || element.hasAttribute('_click:stopPropagation');
            const value = element.getAttribute('_click.' + type + '.value') || element.getAttribute('_click.value');
            element.addEventListener('click',
                function (event) {
                    switch (type) {
                        case 'attr':
                            if (target.hasAttribute(value))
                                target.removeAttribute(value);
                            else
                                target.setAttribute(value, '');
                            break;
                        case 'class':
                            target.classList.toggle(value);
                            break;
                    }
                    if (preventDefault)
                        event.preventDefault();
                    if (stopPropagation)
                        event.stopPropagation();
                });
        }
    });
    document.on('click', function (e) {
        const visible = getComputedStyle(document.body, ':before').getPropertyValue('display');
        if (visible === 'block') {
            const sidebar = getComputedStyle(document.querySelector('.sidebar-container'));
            if (sidebar.visibility === 'visible') {
                document.body.attr('collapsed', false);
            }
        }
    });
}