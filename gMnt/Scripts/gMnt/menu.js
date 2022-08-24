$(function () {
    var $fullAddress = $('#Address_FullAddress');
    var $addressDialog = $('#addressDialog');

    $addressDialog.dialog({
        autoOpen: false,
        width: 500,
        resizable: false,
        modal: true,
        buttons: {
            "Ok": function () {
                $("#addressForm").submmit();
            },

            "Cancel": function () {
                $(this).dialog('close');
            }
        }
    })

    $fullAddress.on('click', function () {
        $.get('/Restaurant/RestaurantAddress', function (data) {
            $addressDialog.html(data);
            var $addressForm = $('#addressForm');
            //unbind existing validation
            $addressForm.unbind();
            $addressForm.data("validator", null);
            //check document for changes
            $.validator.unobtrusive.parse(document);
            //re add validation with changes
            $addressForm.validate($addressForm.data("unobtrusiveValidation").options);

            //open dialog
            $addressDialog.dialog('open');
        });

        return false;
    });
});