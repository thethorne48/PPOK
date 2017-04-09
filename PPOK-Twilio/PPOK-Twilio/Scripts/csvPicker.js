function previewFile() {

    var file2 = document.querySelector('input[type=file]').files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        var result = reader.result;

        $.ajax({
            url: '/LandingPage/UpdateDatabase',
            type: "POST",
            dataType:'text',
            data: { file1: result },
            async:false,
            success: function (data) {
                alert("Success");
                window.location.reload();
            },
            error: function (data) {
                alert('Error!');
            }
        });
    }

    if (file2) {
        reader.readAsText(file2);
    }
}

 $('#uploadbutton').click(function(){
     $('input[type=file]').click();
 });