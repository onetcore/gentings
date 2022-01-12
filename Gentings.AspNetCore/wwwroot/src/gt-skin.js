const resources = {
    "status": {
        404: "请求出错，网页地址不存在！", //not found
        401: "很抱歉，你没有权限，如果没有登入可<a class=\"text-danger\" href=\"/login?returnUrl=" + location.href + "\">点击登入...</a>" //没权限
    },
    "unknownError": "很抱歉，出现了未知错误，请检查是否正确操作后请重试，如果多次出现问题请联系技术支持人员进行排查！",
    "modal": {
        "timeout": "服务器请求超时！",
        "confirm": "确定"
    },
    "ajax": {
        "notFoundUrl": "操作地址没有配置，请检查a[href],form[action]值！",
        "selectedFirst": "请选择项目后再进行操作！",
        "noHeader": "没有配置RequestVerificationToken验证表单！",
        "mustFormElement": "当前请求的元素必须为form表单元素！",
        "uploading": "上传中..."
    },
    "alert": {
        "confirm": "确认"
    },
    "elementError": "元素不存在或者操作一个元素实例，不能获取data-*相关数据！",
    copy: "拷贝",
    copyCode: "点击拷贝代码",
    copied: "已拷贝，请使用“ctrl+v”或者“粘贴”操作！",
    markdown: {
        fullscreen: {
            show: '全屏显示',
            quit: '退出全屏'
        },
        preview: '预览',
        source: '源码',
        link: '请输入链接地址',
        image: '请输入图片地址',
        upload: '上传图片',
        title: '标题'
    }
};

(function ($) {
    // jQuery方法扩展。
    $.fn.extend({
        /**
         * 获取或设置当前元素所有以“_ajax.”开头的所有属性值对象。
         * @param {string} name 属性名称，不包含“_ajax.”。
         * @param {object} value 属性值。
         * @returns 返回当前元素所有以“_ajax.”开头的所有属性值对象。
         */
        json: function (name, value) {
            if (typeof name === 'undefined') {
                if (this.length !== 1) throw new Error(resources.elementError);
                let json = {};
                for (let i = 0; i < this[0].attributes.length; i++) {
                    const attr = this[0].attributes[i];
                    if (attr.name.startsWith('_ajax.'))
                        json[attr.name.substr(6)] = attr.value.trim();
                }
                return json;
            }
            if (typeof value === 'undefined')
                return this.attr('_ajax.' + name);
            return this.attr('_ajax.' + name, value);
        },
        /**
         * 事件指向的对象。
         * @returns 返回事件指向的对象。
         */
        target: function () {
            const target = this.attr('_target');
            if (target) {
                if (target.startsWith('>'))
                    return target.find(target.substr(1).trim());
                return $(target);
            }
            return this;
        },
        /**
         * 获取或者设置数据对象。
         * @param {string} key 数据唯一键。
         * @param {Function} func 如果不存在获取对象的方法。
         * @returns 返回对象实例。
         */
        dset: function (key, func) {
            let data = this.data(key);
            if (!data) {
                data = func();
                this.data(key, data);
            }
            return data;
        },
        /**
         * 回调不包含no-js得所有元素。
         * @param {Function} callback 回调函数。
         */
        exec: function (callback) {
            return this.each(function () {
                var current = $(this);
                if (!current.hasClass('no-js')) {
                    callback(current);
                }
            });
        },
        /**
         * 禁用元素。
         * */
        disabled: function () {
            return this.attr('disabled', 'disabled').addClass('disabled');
        },
        /**
         * 激活元素。
         * */
        enabled: function () {
            return this.removeAttr('disabled').removeClass('disabled');
        },
        /**
         * Ajax提交表单元素。
         * @param {Function|undefined} success 成功后执行的方法。
         * @param {Function|undefined} error 错误后执行的方法。
         */
        ajaxSubmit: function (success, error) {
            if (!this.is('form')) throw Error(resources.ajax.mustFormElement);
            const data = new FormData(this[0]);
            const submit = this.find('[type=submit]').disabled();
            return $ajax(this.attr('action') || location.href, data, d => {
                submit.enabled();
                if (success) success.call(this, d);
            }, function (e) {
                submit.enabled();
                if (error) error(e);
            });
        },
        /**
         * 加载当前元素指定的模态框。
         * @param {object} query 查询字符串对象。
         * */
        loadModal: function (query) {
            let current = this.dset('gt-modal', () => $('<div class="modal fade" data-bs-backdrop="static"><div>')
                .appendTo(document.body))
                .data('target', this.target());
            let url = this.attr('href') || this.attr('action');
            let data = this.json();
            $.extend(data, query);
            url = URL.appendQuery(url, data);
            current.load(url, function (_, status, xhr) {
                switch (status) {
                    case 'error':
                        {
                            let errorMsg = resources.status[xhr.status];
                            if (!errorMsg) errorMsg = resources.unknownError;
                            showMsg(errorMsg);
                        }
                        return;
                    case 'timeout':
                        showMsg(resources.modal.timeout);
                        return;
                }
                const form = current.find('form');
                if (form.length) {
                    if (!form.attr('action'))
                        form.attr('action', url);
                    if (form.find('input[type=file]').length > 0)
                        form.attr('enctype', 'multipart/form-data');
                    current.find('[type=submit]').click(function () {
                        form.find('.field-validation-valid').hide();
                        form.find('.modal-validation-summary').remove();
                        form.ajaxSubmit(function (d) {
                            if (d.code) {
                                if (d.data) {
                                    //表单验证
                                    for (const key in d.data) {
                                        let element = form.find(`[data-valmsg-for="${key}"]`);
                                        if (element.length == 0) {
                                            const char = key.charAt(0);
                                            if (char >= 'a' && char <= 'z') {
                                                const id = char.toUpperCase() + key.substr(1);
                                                element = form.find(`[data-valmsg-for="${id}"]`);
                                            }
                                        }
                                        if (element.length)
                                            element.html(d.data[key]).show();
                                    }
                                }
                                if (d.message) {
                                    form.prepend(`<div class="alert alert-danger modal-validation-summary"><span class="bi-exclamation-circle"></span><span class="summary">${d.message}</span></div>`);
                                }
                            }
                            else {
                                showAlert(d, function () {
                                    location.href = location.href;
                                });
                                current.modal('hide');
                            }
                            return true;//结束这次处理，无需再显示状态信息
                        });
                    });
                }
                onrender(current);
                current.on('hidden.bs.modal', function () {
                    current.remove();
                }).on('shown.bs.modal', function () {
                    current.data('target').trigger('modal', current);
                }).modal('show');
            });
        }
    });
    if (!window.__renders) window.__renders = [];
    /**
     * 当呈现后执行的方法。
     * @param {Function|JQuery|undefined} render 执行的方法或者当前对象，如果为当前对象则执行所有需要执行的方法。
     */
    window.onrender = function (render) {
        if (typeof render === 'function') window.__renders.push(render);
        else window.__renders.forEach(callback => callback(render));
    };
    // 默认需要执行的方法
    onrender(function (context) {
        // 点击事件
        $('[_click]', context).exec(current => {
            let eventType = current.attr('_click').toLowerCase();
            const target = current.target();
            // 展示对象元素，点击元素外的对象隐藏对象元素
            if (eventType === 'show') {
                current.on('click', function (event) {
                    event.stopPropagation();
                    target.addClass('d-block');
                    current.trigger('@show', target, event);
                });
                // 外部点击隐藏
                $(document).on('click', function () {
                    target.removeClass('d-block');
                    current.trigger('@hide', target, event);
                });
                // 阻止事件冒泡
                target.on('click', function (event) {
                    event.stopPropagation();
                });
            }
            // 点击拷贝对象内容copy,copy:text,copy:html
            else if (eventType === 'copy' || eventType.startsWith('copy:')) {
                current.on('click', function (event) {
                    event.preventDefault();
                    $(document).on('copy', function (e) {
                        // 设置信息，实现复制
                        const value = eventType === 'copy:html' ? target.html() : target.text();
                        e.originalEvent.clipboardData.setData('text/plain', value.trim());
                        e.preventDefault();
                        showMsg(resources.copied, 0);
                    });
                    document.execCommand('copy');
                    return false;
                });
            }
            // 点击上传文件
            else if (eventType === 'upload') {
                current.off('click').on('click', function () {
                    $('<input type="file" class="hide"/>').appendTo(document.body).on('change', function () {
                        var file = $(this);
                        if (!this.files.length) {
                            file.remove();
                            return false;
                        }
                        const inner = current.html();
                        current.disabled().html('<span class="spinner-border spinner-border-sm"></span> ' + resources.ajax.uploading);
                        var data = new FormData();
                        data.append("file", this.files[0]);
                        const elements = current.json();
                        for (const key in elements) {
                            data.append(key, elements[key]);
                        }
                        const url = current.attr('href') || current.attr('action');
                        $.ajax({
                            type: "POST",
                            url: url,
                            contentType: false,
                            processData: false,
                            data: data,
                            headers: ajaxHeaders(),
                            success: function (d) {
                                showMsg(d, function () {//没有传回网址时候刷新页面
                                    if (!d.code && !d.url) location.href = location.href;
                                });
                                if (d.url) {
                                    current.trigger('uploaded', d.url);
                                    current.parent().find('.uploaded').each(function () {
                                        var that = $(this);
                                        if (that.is('input'))
                                            that.val(d.url);
                                        else if (that.is('img'))
                                            that.attr('src', d.url);
                                    });
                                }
                                file.remove();
                                current.enabled().html(inner);
                            },
                            error: function (e) {
                                errorHandler(e);
                                file.remove();
                                current.enabled().html(inner);
                            }
                        });
                    }).click();
                    return false;
                });
            }
            else {
                current.on('click', function (event) {
                    let index = eventType.indexOf(':stop');
                    if (index !== -1) {
                        event.stopPropagation();
                        eventType = eventType.replace(':stop', '');
                    }
                    index = eventType.indexOf(':prevent');
                    if (index !== -1) {
                        event.preventDefault();
                        eventType = eventType.replace(':prevent', '');
                    }
                    const confirm = current.attr('_confirm');
                    if (confirm && !window.confirm(confirm))
                        return false;
                    switch (eventType) {
                        case 'modal':
                            {
                                current.loadModal();
                            }
                            return false;
                        case 'ajax':
                            {
                                const data = current.json();
                                const url = current.attr('href') || current.attr('action');
                                $ajax(url, data, function (d) {
                                    if (!d.data && !d.message) { location.href = location.href; }//如果成功没有返回数据，则刷新页面
                                    current.trigger('success', d.data);
                                }, current.attr('_json') === 'false' ? undefined : 'JSON');
                            }
                            return false;
                        case 'checked':
                            {
                                const data = current.json();
                                let items = [];
                                current.parents('.data-list').find('.data-item input[type=checkbox]').each(function () {
                                    if (this.checked) items.push(this.value);
                                });
                                if (items.length > 0) data.id = items;
                                const url = current.attr('href') || current.attr('action');
                                $ajax(url, data, function (d) {
                                    if (!d.data && !d.message) { location.href = location.href; }//如果成功没有返回数据，则刷新页面
                                    else current.trigger('success', d.data);
                                }, current.attr('_json') === 'false' ? undefined : 'JSON');
                            }
                            return false;
                        case 'checked:modal':
                        case 'modal:checked':
                            {
                                const query = {};
                                let items = [];
                                current.parents('.data-list').find('.data-item input[type=checkbox]').each(function () {
                                    if (this.checked) items.push(this.value);
                                });
                                if (items.length > 0) query.id = items.join(',');
                                current.loadModal(query);
                            }
                            return false;
                    }
                });
            }
        });
        // onchange时间
        $('[_change]', context).exec(current => {
            let eventType = current.attr('_change').toLowerCase();
            return current.on('change', function (event) {
                let index = eventType.indexOf(':stop');
                if (index !== -1) {
                    event.stopPropagation();
                    eventType = eventType.replace(':stop', '');
                }
                index = eventType.indexOf(':prevent');
                if (index !== -1) {
                    event.preventDefault();
                    eventType = eventType.replace(':prevent', '');
                }
                const confirm = current.attr('_confirm');
                if (confirm && !window.confirm(confirm))
                    return false;
                switch (eventType) {
                    case 'modal':
                        {
                            current.loadModal();
                        }
                        return false;
                    case 'ajax':
                        {
                            const data = current.json();
                            const url = current.attr('href') || current.attr('action');
                            $ajax(url, data, function (d) {
                                if (!d.data && !d.message) { location.href = location.href; }//如果成功没有返回数据，则刷新页面
                                current.trigger('success', d.data);
                            }, current.attr('_json') === 'false' ? undefined : 'JSON');
                        }
                        return false;
                }
            });
        });
        // 表格排序
        $('table thead .sorting', context).on('click', function () {
            // sort
            const current = $(this);
            let order = 'sorting-asc';
            if (current.hasClass('sorting-asc')) {
                current.json('desc', 'true');
                order = 'sorting-desc';
            } else {
                current.json('desc', 'false');
            }
            current.parent().find('.sorting').removeClass('sorting-asc').removeClass('sorting-desc');
            const query = current.addClass(order).json();
            // form[method=get]
            let search = {};
            $('.toolbar form[method=get]', context).find('input,select,textarea').each(function () {
                if (this.value) search[this.name] = this.value.trim();
            });
            location.href = URL.toSearch(search, query);
        });
        // 表单
        $('form[method=get]', context).exec(current => {
            current.find('input,select,textarea').each(function () {
                var name = this.name.toLowerCase();
                var index = name.indexOf('.');
                if (index > 0)
                    name = name.substr(index + 1);
                this.name = name;
            });
        });
        // 多级菜单
        $('.navbar-nav', context).find('.menu-1,.menu-2,.menu-3').on('hidden.bs.collapse', function (event) {
            event.stopPropagation();
            $(this).find('a[data-bs-toggle="collapse"]').addClass('collapsed').removeAttr('aria-expanded');
            $(this).find('div').removeClass('show');
        });
        // 选中显示toolbar
        $('.data-list', context).exec(current => {
            const actions = current.find('.checked-enabled');
            if (!actions.length) return;
            const items = current.find('.data-item input[type=checkbox]');
            current.find('input[type=checkbox]').on('click', function () {
                for (let i = 0; i < items.length; i++) {
                    if (items[i].checked) {
                        actions.enabled();
                        return;
                    }
                }
                actions.disabled();
            });
            for (let i = 0; i < items.length; i++) {
                if (items[i].checked) {
                    actions.enabled();
                    return;
                }
            }
        });
        // 将当前值设为目标属性值。
        (function (names, context) {
            names.split(',').forEach(name => $('[_v' + name + ']', context).exec(current => {
                const target = $(current.attr('_v' + name + ''), context);
                if (target.length) {
                    target.attr(name, current.val());
                    current.on('change', function () {
                        target.attr(name, this.value);
                    });
                }
            }));
        })("min,max", context);
        // ajax
        $('form[_ajax=true] [type=submit]', context).exec(current => {
            current.on('click', function () {
                current.parents('form').ajaxSubmit();
                return false;
            });
        });
        // card-collapse
        $('.card-header.collapse', context).exec(current => {
            current.on('click', function () {
                if (current.hasClass('show')) {
                    current.toggleClass('show').parents('.card').find('.card-body').toggleClass('hide');
                    return false;
                }
                const group = current.parents('.card-root');
                if (group.length) {
                    group.find('.card-header.collapse').removeClass('show').next('.card-body').addClass('hide');
                }
                current.addClass('show').parents('.card').find('.card-body').removeClass('hide');
                return false;
            });
        });
    });
    /**
     * Ajax请求。
     * @param {string} url 请求的URL地址。
     * @param {Object} data 请求的数据对象。
     * @param {Function|undefined} success 成功后执行的方法。
     * @param {Function|undefined} error 发生错误时候执行的方法。
     * @param {string|undefined} dataType 发送数据类型：'JSON'，不设置表示表单提交。
     */
    window.$ajax = function (url, data, success, error, dataType) {
        if (typeof data === 'undefined') { data = {}; }
        if (typeof success === 'string') {
            dataType = success;
            success = undefined;
        }
        if (typeof error === 'string') {
            dataType = error;
            error = undefined;
        }
        let options = {
            url: url,
            data: data,
            type: 'POST',
            headers: ajaxHeaders(),
            success: function (d) {
                if (success && success(d)) return;
                showMsg(d, function () {
                    location.href = d.data && d.data.url ? d.data.url : location.href;
                });
            },
            error: function (e) {
                errorHandler(e, error);
            }
        };
        if (dataType) {
            options.dataType = dataType;
        } else {
            options.contentType = false;
            options.processData = false;
        }
        $.ajax(options);
        return false;
    };
    //URL辅助方法
    if (!window.URL) window.URL = {};
    /**
     * 将查询字符串附加到URL地址后面。
     * @param {string} url URL地址。
     * @param {object} query 查询对象。
     */
    URL.appendQuery = function (url, query) {
        if (typeof url === 'object') {
            query = url;
            url = '?';
        }
        const index = url.indexOf('?');
        if (index !== -1) {
            let search = url.substr(index + 1);
            url = url.substr(0, index);
            query = URL.toSearch(search, query)
        } else {
            query = URL.toSearch(query);
        }
        return url + query;
    };
    /**
     * 将对象组合后格式化为搜索字符串，以“?”开头。
     * @param {Object|string} search 搜索字符串或者对象。
     * @param {Object} query 修改的搜索字符串对象。
     * @returns {string} 以"?"开头的字符串。
     */
    URL.toSearch = function (search, query) {
        if (typeof search === 'string')
            search = URL.parseQuery(search);
        if (!search) search = {};
        if (query) $.extend(search, query);
        query = '';
        for (const key in search) {
            const value = search[key];
            if (value === '') continue;
            if (query.length > 0) query += '&';
            query += key;
            query += '=';
            query += value;
        }
        return '?' + query;
    };
    /**
     * 匹配查询字符串。
     * @param {string} search 查询字符串。
     * @returns {object} 返回查询对象实例。
     */
    URL.parseQuery = function (search) {
        var query = {},
            seg = search.replace(/^\?/, '').split('&'),
            len = seg.length, i = 0, s;
        for (; i < len; i++) {
            if (!seg[i]) { continue; }
            s = seg[i].split('=');
            query[s[0]] = s[1];
        }
        return query;
    }
    /**
     * 匹配URL地址。
     * @param {string} url URL地址。
     * @returns {object} 返回地址对象实例。
     */
    URL.parse = function (url) {
        var a = document.createElement('a');
        a.href = url;
        return {
            source: url,
            protocol: a.protocol.replace(':', ''),
            host: a.hostname,
            port: a.port,
            query: a.search,
            params: URL.parseQuery(a.search),
            file: (a.pathname.match(/\/([^\/?#]+)$/i) || [, ''])[1],
            hash: a.hash.replace('#', ''),
            path: a.pathname.replace(/^([^\/])/, '/$1'),
            relative: (a.href.match(/tps?:\/\/[^\/]+(.+)/) || [, ''])[1],
            segments: a.pathname.replace(/^\//, '').split('/')
        };
    };
    /**
     * 显示消息。
     * @param {string|Object} msg 消息字符串。
     * @param {Number|Function} code 错误代码或者回调函数。
     * @param {Function|undefined} func 回调函数。
     */
    window.showMsg = function (msg, code, func) {
        if (typeof code === 'function') {
            func = code;
            code = -1;
        }
        else if (typeof code === 'undefined') { code = -1; }
        if (typeof msg === 'object') {
            code = msg.code;
            msg = msg.message;
        }
        // 没有提示信息直接返回
        if (!msg) return;
        const current = $(document.body).dset('gt-toast', () => $(`<div style="z-index:100000;" data-bs-delay="1000" class="toast text-white position-fixed top-0 start-50 translate-middle-x"><div class="d-flex"><div class="toast-body"></div></div></div>`).appendTo(document.body));
        if (code) {
            msg = '<span class="bi-exclamation-circle"></span> ' + msg;
            current.removeClass('bg-success').addClass('bg-danger');
        }
        else {
            msg = '<span class="bi-check-circle"></span> ' + msg;
            current.removeClass('bg-danger').addClass('bg-success');
        }
        current.off('hide.bs.toast').find('.toast-body').html(msg);
        if (!code && func) current.on('hide.bs.toast', function () { func(); });
        current.toast('show');
    };
    /**
     * 显示模态对话框。
     * @param {string|Object} msg 消息字符串。
     * @param {Number|Function} code 错误代码或者回调函数。
     * @param {Function|undefined} func 回调函数。
     */
    window.showAlert = function (msg, code, func) {
        if (typeof code === "function") {
            func = code;
            code = -1;
        }
        else if (typeof code === 'undefined') { code = -1; }
        if (typeof msg === "object") {
            code = msg.code;
            msg = msg.message;
        }
        if (!msg) return;
        const current = $(document.body).dset('gt-alert', () => $('<div class="modal fade" data-bs-backdrop="static"><div class="modal-dialog alert-dialog modal-dialog-centered"><div class="modal-content"><div class="modal-body"><div class="icon"><i></i></div> <div class="msg"></div></div><div class="modal-footer"><button type="button" class="btn btn-primary"> ' + resources.alert.confirm + ' </button></div></div></div></div>').appendTo(document.body));
        var body = current.find('.modal-body');
        const type = !code ? 'success' : 'danger';
        var icon = !code ? "bi-check-circle" : "bi-exclamation-circle";
        body.attr('class', 'modal-body text-' + type).find('i').attr('class', icon);
        body.find('div.msg').html(msg);
        var button = current.find('button').attr('class', 'btn btn-' + type);
        if (!code && func) {
            button.removeAttr('data-bs-dismiss').on('click', () => {
                if (typeof func === 'function') {
                    func(current);
                    current.modal('hide');
                } else if (typeof func === 'object') {
                    location.href = func.url || location.href;
                } else {
                    location.href = location.href;
                }
            });
        } else
            button.attr('data-bs-dismiss', 'modal').off('click');
        current.modal('show');
    };
    /**
     * 请求标题头。
     * */
    function ajaxHeaders() {
        var token = $('#ajax-protected-form').find('[name="__RequestVerificationToken"]');
        if (token.length == 0) {
            throw new Error(resources.ajax.noHeader);
        }
        return {
            'RequestVerificationToken': token.val()
        };
    };
    /**
     * 发生错误请求后执行的方法。
     * @param {Object} e 事件对象。
     * @param {Function|undefined} error 错误发生后执行的方法。
     */
    function errorHandler(e, error) {
        if (error && error(e))
            return;
        var status = resources.status[e.status]
        if (status) {
            showMsg(status);
            return;
        } else {
            console.error(e.responseText);
            showMsg(resources.unknownError);
        }
    };
    $(function () { onrender(); });
})(jQuery);