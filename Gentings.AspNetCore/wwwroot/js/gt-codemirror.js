﻿'use strict';

if (typeof window.CodeMirror !== 'undefined') {
    window.CodeMirror.modeURL = "/lib/codemirror/mode/%N/%N.min.js";
    onrender(function (context) {
        // 编辑器
        $('textarea.CodeMirror', context).exec(function (item) {
            var mode = item.attr('_mode') || 'htmlmixed';
            var readOnly = item.attr('_readonly') || 'false';
            readOnly = readOnly === 'readonly' || readOnly === 'true';
            var editor = window.CodeMirror.fromTextArea(item[0], {
                styleActiveLine: true,
                theme: 'eclipse',
                lineNumbers: true, //显示行号
                lineWrapping: true, //是否强制换行
                matchBrackets: true, //括号匹配
                mode: mode,
                extraKeys: { "Tab": "autocomplete" },
                foldGutter: true,
                readOnly: readOnly,
                gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"]
            });
            var height = item.attr('_height');
            if (height) editor.setSize('100%', height);
            var loadMode = item.attr('_load');
            if (!loadMode) {
                if (mode.startsWith('text/x-')) loadMode = 'clike';else loadMode = mode;
            }
            window.CodeMirror.autoLoadMode(editor, loadMode);
            item.data('CodeMirror', editor);
            // upload
            if (mode == 'markdown') {
                var upload = item.attr('_upload');
                if (upload) {
                    (function () {
                        var json = item.json();
                        editor.on('paste', function (cm, e) {
                            var clipboardData = e.clipboardData || e.originalEvent.clipboardData;
                            if (clipboardData && clipboardData.items) {
                                for (var i = 0; i < clipboardData.items.length; i++) {
                                    var item = clipboardData.items[i];
                                    if (item.kind == 'file' && item.type.indexOf('image') != -1) {
                                        var file = item.getAsFile();
                                        var data = new FormData();
                                        data.append("file", file);
                                        if (json) {
                                            for (var key in json) {
                                                data.append(key, json[key]);
                                            }
                                        }
                                        $ajax(upload, data, function (d) {
                                            if (d.url) {
                                                var selection = cm.doc.getSelection();
                                                cm.replaceSelection('![' + selection + '](' + d.url + ')');
                                            } else {
                                                showMsg(d);
                                            }
                                            return true;
                                        }, false);
                                        return false;
                                    }
                                }
                            }
                        });
                    })();
                }
            }
        });
        //bootstrap tab标签
        $('a[data-bs-toggle="tab"]', context).on('shown.bs.tab', function () {
            $('textarea.CodeMirror', this.getAttribute('href')).each(function () {
                $(this).data('CodeMirror').refresh();
            });
        });
    });
}

