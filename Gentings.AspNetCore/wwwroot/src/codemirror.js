$(function () {
    // 编辑器
    if (typeof window.CodeMirror !== 'undefined') {
        window.CodeMirror.modeURL = "/lib/codemirror/mode/%N/%N.min.js";
        var codeMirrors = document.querySelectorAll('.CodeMirror');
        codeMirrors.forEach(item => {
            var mode = item.getAttribute('_mode') || 'htmlmixed';
            var height = item.getAttribute('_height') || '100vh';
            var readOnly = item.getAttribute('_readonly') || 'false';
            readOnly = readOnly === 'readonly' || readOnly === 'true';
            var editor = window.CodeMirror.fromTextArea(item,
                {
                    styleActiveLine: true,
                    theme: 'eclipse',
                    lineNumbers: true, //显示行号
                    lineWrapping: true, //是否强制换行
                    matchBrackets: true,    //括号匹配
                    mode: mode,
                    extraKeys: { "Tab": "autocomplete" },
                    foldGutter: true,
                    readOnly: readOnly,
                    gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"]
                });
            editor.setSize('100%', height);
            var loadMode = item.getAttribute('_load');
            if (!loadMode) {
                if (mode.startsWith('text/x-'))
                    loadMode = 'clike';
                else
                    loadMode = mode;
            }
            window.CodeMirror.autoLoadMode(editor, loadMode);
            $(item).data('CodeMirror', editor);
        });
        //bootstrap tab标签
        $('a[data-bs-toggle="tab"]').on('shown.bs.tab', function () {
            $('textarea.CodeMirror', this.getAttribute('href')).each(function () {
                $(this).data('CodeMirror').refresh();
            });
        });
    }
});