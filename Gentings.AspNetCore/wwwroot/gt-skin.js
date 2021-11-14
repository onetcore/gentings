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
        "mustFormElement": "当前请求的元素必须为form表单元素！"
    },
    "alert": {
        "confirm": "确认"
    },
    "elementError": "元素不存在或者操作一个元素实例，不能获取data-*相关数据！"
};

(function ($) {
    /**
     * 将对象附加到URL字符串之后。
     * @param {Object} query 查询对象实例。
     */
    String.prototype.appendQuery = function (query) {
        let url = this;
        for (const key in query) {
            if (url.indexOf('?') == -1)
                url += '?';
            else
                url += '&';
            url += key + '=' + query[key];
        }
        return url;
    };
    /**
     * 附加URL字符串之后的随机数。
     */
    String.prototype.appendRandom = function () {
        let url = this;
        if (url.indexOf('?') == -1)
            url += '?';
        else
            url += '&';
        url += '_=' + new Date().valueOf();
    };
    // jQuery方法扩展。
    $.fn.extend({
        /**
         * 获取或设置当前“data-”开头的属性值。
         * @param {string} attributeName 属性名称，不包含“data-”。
         * @param {object} value 属性值。
         * @returns 返回当前属性值或者当前对象。
         */
        dataAttr: function (attributeName, value) {
            return this.attr('data-' + attributeName, value);
        },
        /**
         * 获取当前元素所有以“data-”开头的所有属性值对象。
         * @returns 返回当前元素所有以“data-”开头的所有属性值对象。
         */
        dataAttrs: function () {
            if (this.length != 1) throw new Error(resources.elementError);
            let attrs = {};
            for (let i = 0; i < this[0].attributes.length; i++) {
                const attr = this[0].attributes[i];
                if (attr.name.startsWith('data-'))
                    attrs[attr.name.substr(5)] = attr.value.trim();
            }
            return attrs;
        },
        /**
         * 事件指向的对象。
         * @returns 返回事件指向的对象。
         */
        targetElement: function () {
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
            const form = this;
            const data = new FormData(this[0]);
            const submit = form.find('[type=submit]').disabled();
            $.ajax({
                type: "POST",
                url: form.attr('action') || location.href,
                contentType: false,
                processData: false,
                data: data,
                headers: ajaxHeaders(),
                success: function (d) {
                    submit.removeAttr('disabled');
                    if (success && success(d, form)) {
                        return;//如果success返回true结束处理，否则将会展示错误消息
                    }
                    showMsg(d, function () {
                        if (d.data && d.data.url)
                            location.href = d.data.url;
                        else if (!d.code)
                            location.href = location.href;
                    });
                },
                error: function (e) {
                    submit.enabled();
                    errorHandler(e, error);
                }
            });
            return false;
        },
        /**
         * 加载当前元素指定的模态框。
         * */
        loadModal: function () {
            let current = this.dset('gt-modal', () => $('<div class="modal fade" data-bs-backdrop="static"><div>')
                .appendTo(document.body)
                .data('target', this.targetElement()));
            let url = this.attr('href') || this.attr('action');
            url = url.appendQuery(this.dataAttrs());
            current.load(url, function (_, status, xhr) {
                switch (status) {
                    case 'error':
                        let errorMsg = resources.status[xhr.status];
                        if (!errorMsg) errorMsg = resources.unknownError;
                        showMsg(errorMsg);
                        return;
                    case 'timeout':
                        showMsg(resources.modal.timeout);
                        return;
                }
                const form = current.find('form');
                if (form.length > 0) {
                    if (!form.attr('action'))
                        form.attr('action', url);
                    if (form.find('input[type=file]').length > 0)
                        form.attr('enctype', 'multipart/form-data');
                    current.find('[type=submit]').click(function () {
                        form.ajaxSubmit(function (d, form) {
                            if (d.code) {
                                form.find('.field-validation-valid').hide();
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
                                if (d.message) showMsg(d.message);
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
            const current = $(this);
            let eventType = current.attr('_click');
            const target = current.targetElement();
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
                        const elements = current.dataAttrs();
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
                                showMsg(d);
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
                                current.enabled();
                            },
                            error: function (e) {
                                errorHandler(e);
                                file.remove();
                                current.enabled();
                            }
                        });
                    }).click();
                    return false;
                });
            }
            else if (eventType == '') {
                current.off('click').on('click', function (event) {
                    var offcanvas = new bootstrap.Offcanvas(target[0]);
                    offcanvas.show();
                    event.preventDefault();
                    event.stopPropagation();
                    return false;
                });
            }
            else {
                current.on('click', function (event) {
                    let index = eventType.indexOf(':stop');
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
                                const data = current.dataAttrs();
                                const url = current.attr('href') || current.attr('action');
                                $ajax(url, data);
                            }
                            return false;
                        case 'checked':
                            {
                                const data = current.dataAttrs();
                                let items = [];
                                $('.data-item', context).find('input[type=checkbox]').each(function () {
                                    if (this.checked) items.push(this.value);
                                });
                                if (items.length > 0) data.id = items;
                                const url = current.attr('href') || current.attr('action');
                                $ajax(url, data);
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
        // 图片
        $('img[_error]', context).each(function () {
            const src = this._error;
            if (src) {
                $(this).on('error', function () {
                    if (this.src != src)
                        this.src = src;
                });
            }
        });
        // 表格排序
        $('table thead .sorting', context).on('click', function () {
            let current = 'sorting-asc';
            let desc = 'false';
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
    window.$ajax = function (url, data, success, error) {
        $.ajax({
            url: url,
            data: data,
            dataType: 'JSON',
            type: 'POST',
            headers: ajaxHeaders(),
            success: function (d) {
                showMsg(d, function () {
                    location.href = d.data && d.data.url ? d.data.url : location.href;
                });
                if (success) success(d.data);
            },
            error: function (e) {
                if (error) error(e);
                else errorHandler(e);
            }
        });
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
        const current = $(document.body).dset('gt-toast', () => $(`<div style="z-index:100000;" class="toast text-white position-fixed top-0 start-50 translate-middle-x"><div class="d-flex"><div class="toast-body"></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button></div></div>`).appendTo(document.body));
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
        current.toast('show');//.data('toast').show();
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
    $(function () { render(); });
})(jQuery);