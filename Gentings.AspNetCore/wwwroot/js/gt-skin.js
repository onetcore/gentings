"use strict";

var resources = {
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
        "mustFormElement": "当前请求的元素必须为form表单元素！"
    },
    "elementError": "元素不存在或者操作一个元素实例，不能获取data-*相关数据！"
};

(function ($) {
    /**
     * 将对象附加到URL字符串之后。
     * @param {Object} query 查询对象实例。
     */
    String.prototype.appendQuery = function (query) {
        var url = this;
        for (var key in query) {
            if (url.indexOf('?') == -1) url += '?';else url += '&';
            url += key + '=' + query[key];
        }
        return url;
    };
    /**
     * 附加URL字符串之后的随机数。
     */
    String.prototype.appendRandom = function () {
        var url = this;
        if (url.indexOf('?') == -1) url += '?';else url += '&';
        url += 't=' + new Date().valueOf();
    };
    // jQuery方法扩展。
    $.fn.extend({
        /**
         * 获取或设置当前“data-”开头的属性值。
         * @param {string} attributeName 属性名称，不包含“data-”。
         * @param {object} value 属性值。
         * @returns 返回当前属性值或者当前对象。
         */
        dataAttr: function dataAttr(attributeName, value) {
            return this.attr('data-' + attributeName, value);
        },
        /**
         * 获取当前元素所有以“data-”开头的所有属性值对象。
         * @returns 返回当前元素所有以“data-”开头的所有属性值对象。
         */
        dataAttrs: function dataAttrs() {
            if (this.length != 1) throw new Error(resources.elementError);
            var attrs = {};
            for (var i = 0; i < this[0].attributes.length; i++) {
                var attr = this[0].attributes[i];
                if (attr.name.startsWith('data-')) attrs[attr.name.substr(5)] = attr.value.trim();
            }
            return attrs;
        },
        /**
         * 事件指向的对象。
         * @returns 返回事件指向的对象。
         */
        targetElement: function targetElement() {
            var target = this.attr('_target');
            if (target) {
                if (target.startsWith('>')) return target.find(target.substr(1).trim());
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
        dataSet: function dataSet(key, func) {
            var data = this.data(key);
            if (!data) {
                data = func();
                this.data(key, data);
            }
            return data;
        },
        /**
         * 禁用元素。
         * */
        disabled: function disabled() {
            return this.attr('disabled', 'disabled').addClass('disabled');
        },
        /**
         * 激活元素。
         * */
        enabled: function enabled() {
            return this.removeAttr('disabled').removeClass('disabled');
        },
        /**
         * Ajax提交表单元素。
         * @param {Function|undefined} success 成功后执行的方法。
         * @param {Function|undefined} error 错误后执行的方法。
         */
        submit: function submit(_success, _error) {
            if (!this.is('form')) throw Error(resources.ajax.mustFormElement);
            var form = this;
            var data = new FormData(this[0]);
            var submit = form.find('[type=submit]').disabled();
            $.ajax({
                type: "POST",
                url: form.attr('action') || location.href,
                contentType: false,
                processData: false,
                data: data,
                headers: headers(),
                success: function success(d) {
                    submit.removeAttr('disabled');
                    if (_success && _success(d, form)) {
                        return;
                    }
                    if (d.message) {
                        showMsg(d.message, d.type, function () {
                            if (d.data && d.data.url) location.href = d.data.url;else if (d.code === 0) location.href = location.href;
                        });
                    }
                },
                error: function error(e) {
                    submit.enabled();
                    errorHandler(e, _error);
                }
            });
            return false;
        },
        /**
         * 加载当前元素指定的模态框。
         * */
        loadModal: function loadModal() {
            var _this = this;

            var current = this.dataSet('app-modal', function () {
                return $('<div class="modal fade" data-backdrop="static"><div>').appendTo(document.body).data('target', _this.targetElement());
            });
            var url = this.attr('href') || this.attr('action');
            url = url.appendQuery(this.dataAttrs());
            current.load(url, function (response, status, xhr) {
                switch (status) {
                    case 'error':
                        var errorMsg = resources.status[xhr.status];
                        if (!errorMsg) errorMsg = resources.unknownError;
                        showMsg(errorMsg);
                        return;
                    case 'timeout':
                        showMsg(resources.modal.timeout);
                        return;
                }
                var form = current.find('form');
                if (form.length > 0) {
                    if (!form.attr('action')) form.attr('action', url);
                    if (form.find('input[type=file]').length > 0) form.attr('enctype', 'multipart/form-data');
                    current.find('[type=submit]').click(function () {
                        form.submit(function (d, form) {
                            form.find('.field-validation-valid').hide();
                            if (d.data) {
                                //表单验证
                                for (var key in d.data) {
                                    var element = form.find("[data-valmsg-for=\"" + key + "\"]");
                                    if (element.length == 0) {
                                        var char = key.charAt(0);
                                        if (char >= 'a' && char <= 'z') {
                                            var id = char.toUpperCase() + key.substr(1);
                                            element = form.find("[data-valmsg-for=\"" + id + "\"]");
                                        }
                                    }
                                    if (element.length) element.html(d.data[key]).show();
                                }
                            }
                            if (d.message) {
                                if (d.code == 0) //成功刷新
                                    showMsg(d.message, function () {
                                        current.data('app-modal').modal('hide');
                                        location.href = location.href;
                                    });else //失败显示错误消息
                                    showMsg(d.message);
                            }
                        });
                    });
                }
                render(current);
                current.modal('show');
            });
        }
    });
    /**
     * 呈现后执行的方法。
     * */
    function render(context) {
        // 点击事件
        $('[_click]', context).each(function () {
            var current = $(this);
            var eventType = current.attr('_click');
            var target = current.targetElement();
            // 展示对象元素，点击元素外的对象隐藏对象元素
            if (eventType == 'show') {
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
            // 点击拷贝对象内容
            else if (eventType == 'copy') {
                    current.on('click', function (event) {
                        event.preventDefault();
                        $(document).on('copy', function (e) {
                            // 设置信息，实现复制
                            e.clipboardData.setData('text/plain', target.text().trim());
                            e.preventDefault();
                        });
                        document.execCommand('copy');
                        return false;
                    });
                }
                // 点击上传文件
                else if (eventType == 'upload') {
                        current.off('click').on('click', function () {
                            $('<input type="file" class="hide"/>').appendTo(document.body).on('change', function () {
                                var file = $(this);
                                if (this.files.length == 0) {
                                    file.remove();
                                    return false;
                                }
                                current.disabled();
                                var data = new FormData();
                                data.append("file", this.files[0]);
                                var elements = current.dataAttrs();
                                for (var key in elements) {
                                    data.append(key, elements[key]);
                                }
                                var url = current.attr('href') || current.attr('action');
                                $.ajax({
                                    type: "POST",
                                    url: url,
                                    contentType: false,
                                    processData: false,
                                    data: data,
                                    headers: headers(),
                                    success: function success(d) {
                                        if (d.message) showMsg(d.message, d.code);
                                        if (d.url) {
                                            current.trigger('uploaded', d.url);
                                            current.parent().find('.uploaded').each(function () {
                                                var that = $(this);
                                                if (that.is('input')) that.val(d.url);else if (that.is('img')) that.attr('src', d.url);
                                            });
                                        }
                                        file.remove();
                                        current.enabled();
                                    },
                                    error: function error(e) {
                                        errorHandler(e);
                                        file.remove();
                                        current.enabled();
                                    }
                                });
                            }).click();
                            return false;
                        });
                    } else if (eventType == '') {
                        current.off('click').on('click', function (event) {
                            var offcanvas = new bootstrap.Offcanvas(target[0]);
                            offcanvas.show();
                            event.preventDefault();
                            event.stopPropagation();
                            return false;
                        });
                    } else {
                        current.on('click', function (event) {
                            var index = eventType.indexOf(':stop');
                            if (index != -1) {
                                event.stopPropagation();
                                eventType = eventType.replace(':stop', '');
                            }
                            index = eventType.indexOf(':prevent');
                            if (index != -1) {
                                event.preventDefault();
                                eventType = eventType.replace(':prevent', '');
                            }
                            switch (eventType) {
                                case 'modal':
                                    current.loadModal();
                                    return false;
                                case 'action':
                                    {
                                        var data = current.dataAttrs();
                                        var url = current.attr('href') || current.attr('action');
                                        $ajax(url, data);
                                    }
                                    return false;
                                case 'checked':
                                    {
                                        (function () {
                                            var data = current.dataAttrs();
                                            var items = [];
                                            $('.data-item', context).find('input[type=checkbox]').each(function () {
                                                if (this.checked) items.push(this.value);
                                            });
                                            if (items.length > 0) data.id = items;
                                            var url = current.attr('href') || current.attr('action');
                                            $ajax(url, data);
                                        })();
                                    }
                                    return false;
                            }
                        });
                    }
        });
        // 高亮显示
        if (window.hljs) $('pre code', context).each(function () {
            hljs.highlightBlock(this);
        });
        // 表格排序
        $('table thead .sorting', context).on('click', function () {
            var current = 'sorting-asc';
            var desc = 'false';
            if ($(this).hasClass('sorting-asc')) {
                desc = 'true';
                current = 'sorting-desc';
            }
            $(this).parent().find('.sorting').removeAttr('data-desc').removeClass('sorting-asc').removeClass('sorting-desc');
            $(this).addClass(current).dataAttr('desc', desc);
        });
    };
    /**
     * Ajax请求。
     * @param {string} url 请求的URL地址。
     * @param {Object} data 请求的数据对象。
     * @param {Function|undefined} success 成功后执行的方法。
     * @param {Function|undefined} error 发生错误时候执行的方法。
     */
    window.$ajax = function (url, data, _success2, _error2) {
        $.ajax({
            url: url,
            data: data,
            dataType: 'JSON',
            type: 'POST',
            headers: headers(),
            success: function success(d) {
                if (d.message) {
                    if (d.code == 0) {
                        showMsg(d.message, d.code, function () {
                            location.href = d.data && d.data.url ? d.data.url : location.href;
                        });
                    } else {
                        showMsg(d.message);
                    }
                }
                if (_success2) _success2(d.data);
            },
            error: function error(e) {
                if (_error2) _error2(e);else errorHandler(e);
            }
        });
    };
    /**
     * 显示消息。
     * @param {string|Object} message 消息字符串。
     * @param {Number|Function} code 错误代码或者回调函数。
     * @param {Function|undefined} func 回调函数。
     */
    window.showMsg = function (message, code, func) {
        if (typeof code === 'function') {
            func = code;
            code = -1;
        }
        if (typeof message === 'object') {
            code = message.code;
            message = message.message;
        }
        var type = code == 0 ? 'success' : 'danger';
        var container = $("<div style=\"z-index:100000;\" class=\"toast bg-" + type + " text-white position-fixed top-0 start-50 translate-middle-x\">\n\t<div class=\"d-flex\">\n\t\t<div class=\"toast-body\">\n\t\t\t" + message + "\n\t\t</div>\n\t\t<button type=\"button\" class=\"btn-close btn-close-white me-2 m-auto\" data-bs-dismiss=\"toast\" aria-label=\"Close\"></button>\n\t</div>\n</div>").appendTo(document.body);
        var toast = new bootstrap.Toast(container, { timeout: 3000 });
        if (func) container.on('hide.bs.toast', func);
        toast.show();
    };
    /**
     * 请求标题头。
     * */
    function headers() {
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
        if (error && error(e)) return;
        var status = resources.status[e.status];
        if (status) {
            showMsg(status, false);
            return;
        } else {
            console.error(e.responseText);
            showMsg(resources.unknownError, false);
        }
    };
})(jQuery);

