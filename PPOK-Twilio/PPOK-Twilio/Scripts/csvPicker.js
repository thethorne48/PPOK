$(function () {

    $("#go").click(function () {

        var xhr = new XMLHttpRequest();
        var fd = new FormData();
        fd.append("file", document.getElementById('fileInput').files[0]);
        xhr.open("POST", "UploadContact", true);
        xhr.send(fd);
        xhr.addEventListener("load", function (event) {
            alert(event.target.response);
        }, false);

    });

});