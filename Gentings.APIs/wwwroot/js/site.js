///<reference path="../lib/jquery/jquery.min.js"/>
///<reference path="../lib/bootstrap/js/bootstrap.bundle.min.js"/>
///<reference path="../lib/feather-icons/feather.js"/>
(function () {
    'use strict';

    feather.replace();

    $('.nav-trigger').on('click',
        function (e) {
            e.preventDefault();
            var nav = $(this).next('ul.nav');
            if (nav.is(':visible')) {
                $(this).find('.nav-chevron').html(feather.icons['chevron-right'].toSvg());
                nav.hide();
            } else {
                $(this).find('.nav-chevron').html(feather.icons['chevron-down'].toSvg());
                nav.show();
            }
        });
    $('.dropdown-panel').on('click',
        function(e) {
            e.preventDefault();
            e.stopPropagation();
        });
}());