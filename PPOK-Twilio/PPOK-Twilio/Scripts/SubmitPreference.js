$("#ok").click(function () {

    var perf = $('input[name="choice-preference"]:checked').attr('id');
    var mail = $("#email-address").val();

    alert(perf + "|" + mail);
    $.ajax({
        url: 'Save',
        data: { preference: perf, email: mail },
        type: 'POST',
        success: function (data) {
            //Show popup
            $("#popup").html(data);
        }
    });
});
