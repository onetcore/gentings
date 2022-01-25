'use strict';

function searchDocs(s) {
    var value = $.trim(s.value);
    if (value) {
        $('.docs-menu').addClass('searching').find('a.nav-link').exec(function (current) {
            if (current.parent().hasClass('nav-group')) return;
            var text = current.text().toLowerCase();
            if (text.indexOf(value) === -1) {
                current.addClass('hide');
            } else {
                current.removeClass('hide');
            }
        });
        $('.docs-menu').addClass('searching').find('.nav-group').exec(function (current) {
            if (current.find('ul a').not('.hide').length) {
                current.removeClass('hide');
            } else {
                current.addClass('hide');
            }
        });
    } else {
        $('.docs-menu').removeClass('searching').find('.hide').removeClass('hide');
    }
}
$(function () {
    // toc
    var list = [];
    $('article').find('h1[id],h2[id],h3[id],h4[id],h5[id],h6[id]').each(function () {
        list.push({ id: this.id, text: this.innerText, type: this.tagName.toLowerCase(), top: $(this).offset().top });
    });
    var html = [];
    html.push('<ul>');
    list.forEach(function (item) {
        html.push('<li class="toc-' + item.type + '"><a href="#' + item.id + '" offset="' + item.top + '">' + item.text + '</a></li>');
    });
    html.push('</ul>');
    // scrollspy
    var anchors = $('.docs-toc').html(html.join('\r\n')).find('a');
    function toggleAnchors() {
        var offset = $(document).scrollTop();
        anchors.removeClass('active');
        for (var i = 0; i < list.length; i++) {
            var item = list[i];
            if (offset <= item.top) {
                var id = i == 0 ? item.id : list[i - 1].id;
                $('[href="#' + id + '"]').addClass('active');
                return;
            } else if (i == list.length - 1) {
                $('[href="#' + item.id + '"]').addClass('active');
            }
        }
    }
    toggleAnchors();
    $(window).scroll(toggleAnchors);
});

