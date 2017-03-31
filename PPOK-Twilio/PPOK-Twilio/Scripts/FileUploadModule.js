window.FileUploadModule = (function ($) {

    var formId;
    var callback;

    function fileUpload() {
        var formData = new FormData();
        formData.append('file', $('input[type=file]')[0].files[0]);
        var formData = new FormData($('#' + formId).get(0));

        $.ajax({
            url: '/Recall/Upload',
            type: 'POST',
            dataType: 'html',
            data: formData,
            processData: false,
            contentType: false,
            success: function (data) {
                alert('data ' + data)
                if (typeof (callback) === 'function') {
                    callback(data);
                }
            },
            error: function () { }
        });
    }

    return {
        init: function (uploadFormId, onUploadCallback) {
            formId = uploadFormId;
            callback = onUploadCallback;
            
            //submit the form on selecting a file to upload
            $('#' + formId + ' input[type=file]').on('change', function () {
                $('#' + formId).submit();
            });

            //form onSubmit logic
            $('#' + formId).on('submit', function (e) {
                //prevent browser from changing pages
                e.preventDefault();
                //make ajax call to server
                fileUpload();
            });
        }
    }
})(jQuery);