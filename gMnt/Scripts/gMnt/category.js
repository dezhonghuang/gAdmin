/* $(function () {
     $('table tr:nth-child(2)').addClass('selected');
    load 1st restaurant
    $('.items').on('click', function () {
        $('tr').removeClass('selected');
        $(this).addClass('selected');
        loadItem($(this).index());
    });
})

function loadItem(rid) {
    var $url = 'Category/Details/' + rid;

    $.get($url, function (data) {
        $('#category').html(data);
    });
} */