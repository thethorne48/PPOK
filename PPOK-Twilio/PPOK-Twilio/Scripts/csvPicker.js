function previewFile() {
    
    //$("#Load").css("display", "block");
    //$("#Event").css("display", "block");
    
    $("#Load").show();
    $("#Event").show();
    window.setTimeout(function (){

        var file2 = document.querySelector('input[type=file]').files[0];
        var reader = new FileReader();

        reader.onloadend = function () {

            var result = reader.result;
            $.ajax({
                url: '/LandingPage/UpdateDatabase',
                type: "POST",
                dataType: 'text',
                data: { file1: result },
                async: false,
                success: function (data) {
                    $("#reload").load('/LandingPage/ReturnTable');
                    //$("#Load").css("display", "none");
                    //$("#Event").css("display", "none");
                    $('#file').val('');
                    
                },
                error: function (data) {
                    alert('Error!');
                    $("#reload").load('/LandingPage/ReturnTable');
                    //$("#Load").css("display", "none");
                    //$("#Event").css("display", "none");
                    $("#Load").hide();
                    $("#Event").hide();
                    $('#file').val('');
                }
            });
        }

        if (file2) {
            reader.readAsText(file2);
            //document.querySelector('input[type=file]').attr("value", "");
        }
        else {
            $("#Load").hide();
            $("#Event").hide();
        }
    }, 10);
}

 $('#uploadbutton').click(function(){
     $('input[type=file]').click();
 });

 $('#send').click(function(){
     window.landingPage.Send();
     $("#reload").load('/LandingPage/ReturnTable');
 });