window.FileUploadModule = (function ($) {

    var formId;
    var filePathFieldId;
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
                if (typeof (callback) === 'function') {
                    callback(data);
                }
            },
            error: function () { }
        });
    }

    function setFilePathFieldValue(filePath) {
        if (filePathFieldId) {
            $('#' + filePathFieldId).val(filePath);
        }
    }

    return {
        //outputPathFieldId is optional, can be used for the suggested setFilePathFieldValue() upload callback
        init: function (uploadFormId, onUploadCallback, outputPathFieldId) {
            formId = uploadFormId;
            callback = onUploadCallback;
            filePathFieldId = outputPathFieldId;
            
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
        },

        setFilePathFieldValue: function(value) {
            setFilePathFieldValue(value);
        }
    }
})(jQuery);