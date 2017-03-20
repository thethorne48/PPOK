$("#ok").click(function () {
    var Info = {
        preference: $("#choice-preference").val(),
        email: $("#email-address").val()
    };

    $.ajax({
        type: "POST",
        url: "PatientMCP/Save",
        data: "Info",
        dataType : 'json',
        success: function (data) {
            
        },
        dataType: function (data) {
            // handle success callback
        }
    });
});
