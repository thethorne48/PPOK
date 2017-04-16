window.singleEvent = (function ($) {
    function refill(id) {
        console.log("Got here taco : " + id);
        $.ajax({
            type: "POST",
            url: "/ResendEvents/GetSingleEvent", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id },
            dataType: "json",
            success: function (r) {
                console.log(r);
                $("#Code").val(r.Code);
                $('#ResendModal').modal('toggle');
            }
        });
    };
    return {
        init: function () {
            console.log("Loading JS");
            $('#myTable').DataTable(
                {
                    "scrollY": "200px",
                    "scrollCollapse": true,
                    "bLengthChange": false,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "columnDefs": [
                            { targets: 3, searchable: false }
                    ]
                }
           );
            console.log("finished loading js");
        },
        refill: function (id) {
            refill(id);
        },
    }

})(jQuery);