function previewFile() {
    var preview = document.querySelector('img');
    var file2 = document.querySelector('input[type=file]').files[0];
    var reader = new FileReader();

    reader.onloadend = function () {
        var result = reader.result;
        alert(result); //this is an ArrayBuffer
        console.log(result);
        console.log(result.toString());

        $.ajax({
            url: 'UpdateDatabase',
            type: "POST",
            data: { file1: result },
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                alert(data);
            },
            error: function () {
                alert();
            }
        });
    }

    if (file2) {
        reader.readAsText(file2);
    }
}