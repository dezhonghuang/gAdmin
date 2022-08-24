$(function () {
    var path = location.pathname;
    
    $('ul#menu a[href="' + path + '"]').addClass('current');
})
