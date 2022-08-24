$(function () {
      
    $('#btnEdit').on('click', function () {
        var $editUrl = '/Category/Edit/' + $('tr.selected').index();
        alert($editUrl);
        $.get($editUrl, function (data) {
            $('.category').html(data);
        });
    });

    $('#btnClick').on('click', function () {
        alert('34');
        $.get('/Category/Details/', +$('tr.selected').index(), function (data) {
            $('.category').html(data);
        });
    });
})