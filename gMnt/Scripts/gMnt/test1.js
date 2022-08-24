$(function () {
    //alert('lll' + location.pathname);

    $('#file').change(function (evt) {
        var f = evt.target.files[0];
        var fileReader = new FileReader();

        //check if the selected file is an image file
        if (!f.type.match('image.*')) {
            alert("Selected file is not an image file, please try again.");
            return;
        }

        //change img tag attribute
        fileReader.onload = function (e) {
            $('img#image').attr('src', e.target.result);
        }

        fileReader.readAsDataURL(f);
    });


    var $did = location.pathname.split('/').pop();
    //alert($did);

    $.ajax({
        url: '/Dish/DishImage',
        type: 'GET',
        data: { id: $did },
        dataType: 'html'
    }).success(function (result) {
        $('#dish-image').html(result);
    }).error(function (xhr, status) {
        //alert('Status: ' + status);
    });
})