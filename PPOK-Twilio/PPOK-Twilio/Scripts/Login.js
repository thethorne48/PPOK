$(document).ready(function () {
    $('#pass').keyup(confirmPass);
    $('#confirmPass').keyup(confirmPass);

    function confirmPass(e) {
        var pass = $('#pass').val();
        var confirm = $('#confirmPass').val();
        if (pass.localeCompare(confirm) === 0 && pass.length > 6 && checkComplexity(pass)) {
            $('#warning').hide();
            $('#submit-btn').removeAttr('disabled');
        } else {
            $('#warning').show();
            $('#submit-btn').attr('disabled', 'disabled');
        }
    }

    function checkComplexity(password) {
        //$.ajax({
        //    type: "GET",
        //    url: "/Account/CheckPassword?password=" + password,
        //    success: function (data) {
        //        console.log(data);
        //        return data;
        //    }
        //});
        return true;
    }
});