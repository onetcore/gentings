//支持highlight.js，需要加载脚本：https://highlightjs.org/
'use strict';

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ('value' in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError('Cannot call a class as a function'); } }

marked.setOptions({
    renderer: new marked.Renderer(),
    highlight: function highlight(code) {
        if (!window.hljs) return;
        return hljs.highlightAuto(code).value;
    },
    pedantic: false,
    gfm: true,
    tables: true,
    breaks: false,
    sanitize: true, //移除HTML,HTML将作为字符串
    smartLists: true,
    smartypants: false,
    xhtml: false
});

var MarkDownEditor = (function () {
    function MarkDownEditor(selector) {
        _classCallCheck(this, MarkDownEditor);

        this.selector = selector;
        this._height = selector.css('height'); //用于全屏
        this._uploadUrl = selector.attr('action');
        this.preview = $('.mozmd-preview', selector);
        this.source = $('.mozmd-source', selector);
        this.actions = $('.mozmd-toolbar a', selector);
        this._value = $('.mozmd-source-value', selector);
        this._html = $('.mozmd-html-value', selector);
        this.init();
    }

    _createClass(MarkDownEditor, [{
        key: 'update',
        value: function update() {
            var source = this.source.text();
            this._value.val(source);
            var html = marked.parse(source);
            this.preview.html(html);
            this._html.val(html);
            this.selector.trigger('mozmd.updated');
        }

        /**
         * 粘贴图片或者内容。
         * @param {Event} e 事件实例。
         */
    }, {
        key: 'paste',
        value: function paste(e) {
            var _this = this;

            if (!this._uploadUrl) return true;
            this.saveSelection();
            var clipboardData = e.clipboardData || e.originalEvent.clipboardData;
            if (clipboardData && clipboardData.items) {
                for (var i = 0; i < clipboardData.items.length; i++) {
                    var item = clipboardData.items[i];
                    if (item.kind == 'file' && item.type.indexOf('image') != -1) {
                        var file = item.getAsFile();
                        this.upload(file, function (d) {
                            _this.replaceSelection(function (text) {
                                var title = '';
                                if (!text) text = '';else title = ' "' + text + '"';
                                return '![' + title + '](' + d.url + ')';
                            });
                        });
                        return false;
                    }
                }
            }
            return true;
        }

        /**
         * 存储当前光标选中位置。
         * */
    }, {
        key: 'saveSelection',
        value: function saveSelection() {
            if (window.getSelection && document.createRange) {
                var sel = window.getSelection();
                if (!sel.rangeCount) {
                    this._selection = {
                        start: 0,
                        end: 0
                    };
                    return;
                }
                var range = sel.getRangeAt(0);
                var preSelectionRange = range.cloneRange();
                preSelectionRange.selectNodeContents(this.source[0]);
                preSelectionRange.setEnd(range.startContainer, range.startOffset);
                var start = preSelectionRange.toString().length;

                this._selection = {
                    start: start,
                    end: start + range.toString().length
                };
            } else if (document.selection && document.body.createTextRange) {
                var selectedTextRange = document.selection.createRange();
                var preSelectionTextRange = document.body.createTextRange();
                preSelectionTextRange.moveToElementText(this.source[0]);
                preSelectionTextRange.setEndPoint("EndToStart", selectedTextRange);
                var start = preSelectionTextRange.text.length;

                this._selection = {
                    start: start,
                    end: start + selectedTextRange.text.length
                };
            }
        }

        /**
         * 获取光标选中位置。
         * */
    }, {
        key: 'restoreSelection',
        value: function restoreSelection() {
            var savedSel = this._selection;
            if (window.getSelection && document.createRange) {
                var charIndex = 0,
                    range = document.createRange();
                range.setStart(this.source[0], 0);
                range.collapse(true);
                var nodeStack = [this.source[0]],
                    node,
                    foundStart = false,
                    stop = false;

                while (!stop && (node = nodeStack.pop())) {
                    if (node.nodeType == 3) {
                        var nextCharIndex = charIndex + node.length;
                        if (!foundStart && savedSel.start >= charIndex && savedSel.start <= nextCharIndex) {
                            range.setStart(node, savedSel.start - charIndex);
                            foundStart = true;
                        }
                        if (foundStart && savedSel.end >= charIndex && savedSel.end <= nextCharIndex) {
                            range.setEnd(node, savedSel.end - charIndex);
                            stop = true;
                        }
                        charIndex = nextCharIndex;
                    } else {
                        var i = node.childNodes.length;
                        while (i--) {
                            nodeStack.push(node.childNodes[i]);
                        }
                    }
                }

                var sel = window.getSelection();
                sel.removeAllRanges();
                sel.addRange(range);
            } else if (document.selection && document.body.createTextRange) {
                var textRange = document.body.createTextRange();
                textRange.moveToElementText(this.source[0]);
                textRange.collapse(true);
                textRange.moveEnd("character", savedSel.end);
                textRange.moveStart("character", savedSel.start);
                textRange.select();
            }
        }

        /**
         * 在光标处替换文本。
         * @param {string} text 文本字符串。
         * @param {number} offset 光标偏移量。
         */
    }, {
        key: 'replaceSelection',
        value: function replaceSelection(text, offset) {
            this.restoreSelection();
            var sel, range, length;
            if (window.getSelection) {
                sel = window.getSelection();
                if (typeof text === 'function') {
                    var selText = sel.toString();
                    text = text(selText);
                    length = selText.length;
                } else {
                    length = text.length;
                }
                if (sel.getRangeAt && sel.rangeCount) {
                    range = sel.getRangeAt(0);
                    range.deleteContents();
                    var textNode = document.createTextNode(text);
                    range.insertNode(textNode);
                    sel.removeAllRanges();
                    range = range.cloneRange();
                    range.selectNode(textNode);
                    range.collapse(false);
                    sel.addRange(range);
                }
            } else if (document.selection && document.selection.createRange) {
                range = document.selection.createRange();
                if (typeof text === 'function') {
                    text = text(range.text);
                    length = range.text;
                } else {
                    length = text.length;
                }
                range.pasteHTML(text);
                range.select();
            }
            if (typeof offset != 'undefined') {
                if (!this._selection) this._selection = {
                    start: 0,
                    end: length
                };else this._selection = {
                    start: this._selection.start + offset,
                    end: this._selection.start + offset + length
                };
                this.restoreSelection();
            }
            this.update();
        }
    }, {
        key: 'toggleActions',
        value: function toggleActions() {
            if (this.source.is(':visible')) {
                this.actions.filter('.disabled').removeClass('disabled');
            } else {
                this.actions.each(function () {
                    if (this.className.startsWith('mozmd-syntax-')) $(this).addClass('disabled');
                });
            }
        }
    }, {
        key: 'init',
        value: function init() {
            var _this2 = this;

            this.source.on('input', function (e) {
                return _this2.update();
            }).on('paste', function (e) {
                return _this2.paste(e);
            }).on('keydown', function (e) {
                //tab和shift+tab缩进
                if (e.keyCode == 9) {
                    _this2.saveSelection();
                    if (e.shiftKey) _this2.replaceSelection(function (text) {
                        if (!text) return '';
                        var lines = [];
                        text.split('\n').forEach(function (t) {
                            if (t.startsWith('\t')) lines.push(t.substr(1));else lines.push(t);
                        });
                        return lines.join('\n');
                    });else _this2.replaceSelection(function (text) {
                        if (!text) return '\t';
                        return '\t' + text.split('\n').join('\n\t');
                    });
                    return false;
                }
            });
            this.actions.exec(function (current) {
                return current.off('mousedown').on('mousedown', function (e) {
                    _this2.saveSelection();
                    var name = current.attr('class');
                    switch (name) {
                        case 'mozmd-fullscreen':
                            $(document.body).addClass('fullscreen');
                            _this2.selector.addClass('fullscreen-container').css('height', '100%');
                            current.attr('class', 'mozmd-exitfull').attr('title', resources.fullscreen.quit).find('i').attr('class', 'bi-window-stack');
                            break;
                        case 'mozmd-exitfull':
                            $(document.body).removeClass('fullscreen');
                            _this2.selector.removeClass('fullscreen-container').css('height', _this2._height);
                            current.attr('class', 'mozmd-fullscreen').attr('title', resources.fullscreen.show).find('i').attr('class', 'bi-window-fullscreen');
                            break;
                        case 'mozmd-mode-preview':
                            _this2.source.hide();
                            _this2.preview.show();
                            current.attr('class', 'mozmd-mode-source').attr('title', resources.markdown.source).find('i').attr('class', 'bi-keyboard');
                            _this2.toggleActions();
                            break;
                        case 'mozmd-mode-source':
                            _this2.source.show();
                            _this2.preview.hide();
                            current.attr('class', 'mozmd-mode-preview').attr('title', resources.markdown.preview).find('i').attr('class', 'bi-eye');
                            _this2.toggleActions();
                            break;
                        case 'mozmd-syntax-bold':
                            _this2.replaceSelection(function (text) {
                                return "**" + text.trim() + "**";
                            }, 2);
                            break;
                        case 'mozmd-syntax-italic':
                            _this2.replaceSelection(function (text) {
                                return "_" + text.trim() + "_";
                            }, 1);
                            break;
                        case 'mozmd-syntax-header':
                            _this2.replaceSelection(function (text) {
                                text = text.trim();
                                if (text[0] == '#') return '#' + text;
                                return "# " + text;
                            }, 1);
                            break;
                        case 'mozmd-syntax-code':
                            _this2.replaceSelection(function (text) {
                                if (!text) return '```\n\n```\n';
                                if (text.indexOf('\n') == -1) {
                                    if (text.startsWith('`')) return text;
                                    return '`' + text + '`';
                                }
                                if (text.startsWith('```')) return text;
                                return '```\n' + text.trim() + '\n```\n';
                            });
                            break;
                        case 'mozmd-syntax-ul':
                            _this2.replaceSelection(function (text) {
                                if (!text) return '* ';
                                var lines = [];
                                text.split('\n').forEach(function (t) {
                                    if (t.startsWith('1. ')) lines.push('*' + t.substr(2));else if (t.startsWith('* ')) lines.push(t);else lines.push('* ' + t);
                                });
                                return lines.join('\n');
                            });
                            break;
                        case 'mozmd-syntax-ol':
                            _this2.replaceSelection(function (text) {
                                if (!text) return '1. ';
                                var lines = [];
                                text.split('\n').forEach(function (t) {
                                    if (t.startsWith('* ')) lines.push('1. ' + t.substr(1));else if (t.startsWith('1. ')) lines.push(t);else lines.push('1. ' + t);
                                });
                                return lines.join('\n');
                            });
                            break;
                        case 'mozmd-syntax-link':
                            var link = window.prompt(resources.markdown.link, 'http://');
                            if (link) _this2.replaceSelection(function (text) {
                                if (!text) text = link;
                                return '[' + text + '](' + link + ')';
                            }, 1);
                            break;
                        case 'mozmd-syntax-quote':
                            _this2.replaceSelection(function (text) {
                                return '> ' + text;
                            }, 1);
                            break;
                        case 'mozmd-syntax-image':
                            if (!_this2._uploadUrl) {
                                var imageUrl = window.prompt(resources.markdown.image, 'http://');
                                if (imageUrl) _this2.replaceSelection(function (text) {
                                    var title = '';
                                    if (text) title = ' "' + text + '"';
                                    return '![' + title + '](' + imageUrl + ')';
                                }, 2);
                                break;
                            }
                            _this2.showDialog(name, '<div class="mb-2">\n            <label>' + resources.markdown.image + '：</label>\n            <div class="input-group-append">\n                <input class="form-control form-control-sm" type="text" name="image-url"/>\n                <input type="file" class="hide"/>\n                <button type="button" onclick="$(this).parent().find(\'input[type=file]\').click();" title="' + resources.markdown.upload + '"><i class="bi-upload"></i></button>\n            </div>\n            </div><div class="mb-1">\n            <label>' + resources.markdown.title + '：</label>\n            <input class="form-control form-control-sm" type="text" name="image-title"/>\n        </div>', function (m) {
                                var url = m.find('input[name=image-url]').val().trim();
                                if (!url) return;
                                var title = m.find('input[name=image-title]').val().trim();
                                _this2.replaceSelection(function (text) {
                                    if (title) {
                                        text = text || title;
                                        title = ' "' + title + '"';
                                    }
                                    return '![' + title + '](' + url + ')';
                                });
                            }).find('input[type=file]').on('change', function (e) {
                                var file = $(e.target);
                                if (e.target.files.length == 0) return;
                                _this2.upload(e.target.files[0], function (d) {
                                    file.parent().find('input[name=image-url]').val(d.url);
                                });
                            });
                            break;
                    }
                    _this2.selector.trigger(name.replace(/-+/ig, '.'));
                    return false;
                });
            });
            this.update();
        }

        /**
         * 上传文件。
         * @param {File} file 上传的文件对象实例。
         * @param {Function} success 上传成功后的回调函数。
         */
    }, {
        key: 'upload',
        value: function upload(file, success) {
            var data = new FormData();
            data.append("file", file);
            var ajaxData = this.selector.json();
            if (ajaxData) {
                for (var key in ajaxData) {
                    data.append(key, ajaxData[key]);
                }
            }
            $ajax(this._uploadUrl, data, success);
        }

        /**
         * 显示模态框。
         * @param {string} id 唯一id，此处为操作方法。
         * @param {string} body 模态内容代码。
         * @param {string} footer 底部代码。
         * @param {Function} func 点击确定后的回调函数。
         */
    }, {
        key: 'showDialog',
        value: function showDialog(id, body, footer, func) {
            var current = $(document.body).dset('gt-' + id, function () {
                return $('<div class="modal fade" data-bs-backdrop="static"><div class="modal-dialog modal-dialog-centered"><div class="modal-content"><div class="modal-body"></div><div class="modal-footer"><button type="button" class="btn btn-primary"> ' + resources.modal.confirm + ' </button></div></div></div></div>').appendTo(document.body);
            });
            current.find('.modal-body').html(body);
            if (typeof footer === 'function') {
                func = footer;
                footer = undefined;
            }
            if (footer) {
                current.find('.modal-footer').html(footer);
            }
            var button = current.find('button.btn-primary').off('click');
            if (func) {
                button.removeAttr('data-bs-dismiss').on('click', function () {
                    if (typeof func === 'function') {
                        current.modal('hide'); //先隐藏再进行操作
                        func(current);
                    } else if (typeof func === 'object') {
                        location.href = func.url || location.href;
                    } else {
                        location.href = location.href;
                    }
                });
            } else button.attr('data-bs-dismiss', 'modal').off('click');
            onrender(current);
            current.modal('show');
            return current;
        }
    }, {
        key: 'value',
        get: function get() {
            return this._value.val();
        },
        set: function set(source) {
            this.source.text(source);
            this.update();
        }
    }, {
        key: 'html',
        get: function get() {
            return this._html.val();
        }
    }]);

    return MarkDownEditor;
})();

onrender(function (context) {
    $('.mozmd-editor', context).each(function () {
        var current = $(this);
        current.data('mozmd-editor', new MarkDownEditor(current));
    });
});

