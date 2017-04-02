$("#ok").click(function () {

    var perf = $('input[name="choice-preference"]:checked').attr('id');
    var mail = $("#email-address").val();

    $.ajax({
        url: '/PatientMCP/Save',
        data: { preference: perf, email: mail },
        type: 'POST',
        async:false,
        success: function (data) {
            alert(data);
        },
        failure: function () {
            alert("NOPE!");
        }
    });
});
