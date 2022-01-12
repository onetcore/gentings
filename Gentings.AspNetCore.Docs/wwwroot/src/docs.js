function searchDocs(s) {
    var value = $.trim(s.value);
    if (value) {
        $('.docs-menu').addClass('searching').find('a.nav-link').exec(current => {
            if (current.parent().hasClass('nav-group')) return;
            var text = current.text().toLowerCase();
            if (text.indexOf(value) === -1) {
                current.addClass('hide');
            } else {
                current.removeClass('hide');
            }
        });
        $('.docs-menu').addClass('searching').find('.nav-group').exec(current => {
            if (current.find('ul a').not('.hide').length) {
                current.removeClass('hide');
            } else {
                current.addClass('hide');
            }
        });
    }
    else {
        $('.docs-menu').removeClass('searching').find('.hide').removeClass('hide');
    }
}
$(function () {
    var list = [];
    $('article').find('h1,h2,h3,h4,h5,h6').each(function () {
        if (!this.id) return;
        list.push({ id: this.id, text: this.innerText, type: this.tagName.toLowerCase() })
    });
    var html = [];
    html.push('<ul>');
    list.forEach(item => {
        html.push(`<li class="toc-${item.type}"><a href="#${item.id}">${item.text}</a></li>`)
    });
    html.push('</ul>')
    $('.docs-toc').html(html.join('\r\n'));
    //$(document.body).attr('data-bs-target', '.docs-toc').scrollspy();
});
