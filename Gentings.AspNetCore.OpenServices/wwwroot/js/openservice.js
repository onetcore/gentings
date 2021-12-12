window.$Tester = {
    token: function (current) {
        if (typeof current === 'undefined') {
            return sessionStorage.getItem('jwt-token');
        } else {
            var token = $(current).json('token');
            if (token) sessionStorage.setItem('jwt-token', token);
            showMsg('你已经成功设置了Token!', 0, function () { location.href = location.href;});
            return false;
        }
    },
    submit: function (current) {
        var modal = $(current).parents('.modal');
        current = modal.find('form');
        var result = current.find('.test-result').html('');
        var method = current.attr('method');
        var url = current.attr('action') || location.href;
        var headers = {};
        var token = $Tester.token();
        if (token) headers = { 'Authorization': 'Bearer ' + token };
        var formData = new FormData(current[0]);
        var data = {};
        if (method === 'GET') {
            var query;
            formData.forEach((v, k) => {
                if (query) query += '&';
                query += k + '=' + v;
            });
            if (url.indexOf('?') === -1) url += '?' + query;
            else url += '&' + query;
        }
        else {
            formData.forEach((v, k) => data[k] = v);
        }
        $.ajax({
            type: method,
            url: url,
            dataType: 'json',
            contentType: "application/json;charset=utf-8",
            data: JSON.stringify(data),
            headers: headers,
            success: function (d) {
                result.parent().show();
                $(`<pre><code class="language-json">${JSON.format(d)}</code></pre>`).highlight().appendTo(result);
                if (!d.code && url.indexOf('open/token') !== -1) {
                    modal.find('.token-action').json('token', d.data).removeClass('hide');
                } else {
                    modal.find('.token-action').addClass('hide');
                }
            },
            error: function (e) {
                result.parent().show();
                $(`<pre><code class="language-json">${JSON.format(e.responseText)}</code></pre>`).highlight().appendTo(result);
                modal.find('.token-action').addClass('hide');
            }
        });
    },
    reset: function (current) {
        current = $(current).parents('.modal').find('form');
        current.reset();
    }
};
/**
 * 格式化JSON字符串。
 * @param {Object|string} code 代码片段或者对象。
 */
JSON.format = function (code) {
    if (typeof code === "object")
        code = JSON.stringify(code);
    var indent = '    ';
    var br = '\r\n';
    var index = 0;
    var getIndent = function () {
        var current = '';
        for (let i = 0; i < index; i++) {
            current += indent;
        }
        return current;
    }
    var getQuote = function (quote, start) {
        var name = quote;
        for (let i = start + 1; i < code.length; i++) {
            var current = code.charAt(i);
            if (current === '\\') {
                i++;
                name += current;
                name += code.charAt(i);
            }
            else if (current === quote) {
                name += current;
                break;
            }
            else {
                name += current;
            }
        }
        return name;
    }
    var result = '';
    var iskey = true;
    for (let i = 0; i < code.length; i++) {
        var current = code.charAt(i);
        if (current === '{') {
            iskey = true;
            result += current;
            result += br;
            index++;
        }
        else if (current === '}') {
            index--;
            result += br;
            result += getIndent();
            result += current;
        }
        else if (current === '\'' || current === '"' || current === '`') {
            var quote = getQuote(current, i);
            i += quote.length - 1;
            if (iskey) {
                result += getIndent();
                iskey = false;
            }
            result += quote;
        }
        else if (current === ',') {
            iskey = true;
            result += current;
            result += br;
        }
        else if (current === ':') {
            result += current;
            result += ' ';
        }
        else {
            result += current;
        }
    }
    return result;
}

$(function () {
    var token = $Tester.token();
    if (token) {
        $('.service-route i.bi-lock').addClass('hide');
        $('.service-route i.bi-unlock').removeClass('hide');
    } else {
        $('.service-route i.bi-lock').removeClass('hide');
        $('.service-route i.bi-unlock').addClass('hide');
    }
});