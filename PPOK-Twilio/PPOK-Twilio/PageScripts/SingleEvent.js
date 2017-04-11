﻿window.singlePharmacy = (function ($) {
    function refill(id) {
        $.ajax({
            type: "POST",
            url: "/ResendEvents/resendevent", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id },
            dataType: "json",
            success: function (r) {
                console.log(r);
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
                            { targets: 4, searchable: false }
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