function ReturnCSVRequest(FormData) {
    $.ajax({
        url: "UpdateDatabase",
        data: { 'file' : FormData },
        type: "POST",
        cache: false,
        success: function()
        {
            alert("Success!");
        },
        error: function ()
        {
            alert("Failure");
        }
        });
}