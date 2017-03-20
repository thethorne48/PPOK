function previewFile() {

    var file2 = document.querySelector('input[type=file]').files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        var result = reader.result;
        alert(result); //this is an ArrayBuffer

        $.ajax({
            url: 'UpdateDatabase',
            type: "POST",
            data: { file1: result },
            dataType: "json",
            async:false,
            success: function (data) {
                alert("Success");
                window.location.reload();
            },
            error: function () {
                alert("ERROR");
            }
        });
    }

    if (file2) {
        reader.readAsText(file2);
    }
}
