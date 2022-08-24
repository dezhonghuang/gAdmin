$(function () {
    $('#btnEdit').click(function () {
        var $editUrl = '/Restaurant/Edit/' + $('.selected td').html()

        $.get($editUrl, function (data) {
            $('#restaurant-details').empty().append(data);
        }).error(function () {
            alert('error');
        });

    });
})