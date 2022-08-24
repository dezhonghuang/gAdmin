$(function () {
    //load 1st restaurant
    $('table tr:nth-child(2)').addClass('selected');

    loadRestaurant($('.selected td:first-child').html());
});

//click event of class restaurant
$('.restaurants').on('click', function () {
    $('tr').removeClass('selected');
    $(this).toggleClass('selected');
    loadRestaurant($('.selected td').html());
});


function loadRestaurant(rid) {
    var $url = '/Restaurant/Details/' + rid;

    $.get($url, function (data) {
        $('#restaurant-details').html(data);
    });
}