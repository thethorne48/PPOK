$("#PhoneSubmit").click(function () {

    var phone = $("#Phone").val();

    $.ajax({
        url: 'ChangePreference',
        data: { phoneNum: phone },
        type: 'POST',
        async: false,
        success: function (data) {
        }
    });
});
