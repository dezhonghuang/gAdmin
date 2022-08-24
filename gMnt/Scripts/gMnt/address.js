$(function () {
    $('#Address_FullAddress').autocomplete({
        //source: tags
        source: function (request, response) {
            $.get('/Restaurant/StreetList', { term: request.term }, function (data) {
                response(data);
            })
        },
        minlength:3,
        select: function (event, data) {
/*            alert('ttt ' + $('#Restaurant_AddressId').val());
            $.post('/Restaurant/AddressUpdate', { addressId: $('#Restaurant_AddressId').val() + 0,  streetId: data.item.id + 0, streetNumber:data.item.streetNo }, function (result) {
                $('#Restaurant_AddressId').val(result);
                       }); */
            $('#StreetNumber').val(data.item.streetNo);
            $('#StreetId').val(data.item.id);
        } 
    }).focusout(function () {
        if ($(this).val().length == 0) {
            $('#StreetNumber').val("");
            $('#StreetId').val(0);
        }
    });
});