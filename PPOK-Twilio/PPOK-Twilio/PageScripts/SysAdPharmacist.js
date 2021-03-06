﻿window.SysAdPharmacist = (function ($) {
    return {
        init: function () {
            console.log("Loading JS");
            var dataTaco;
            $.ajax({
                type: "POST",
                url: "/SystemAdmin/GetAllPharmacists",
                dataType: "json",
                success: function (r) {
                    console.log(r);
                    var dt = $('#example').DataTable({
                        "data": r,
                        "scrollY": "200px",
                        "scrollCollapse": true,
                        "bLengthChange": false,
                        "bFilter": true,
                        "bInfo": false,
                        "bAutoWidth": false,
                        "columns": [
                            { "data": "FirstName" },
                            { "data": "LastName" },
                            { "data": "Email" },
                            { "data": "Phone" },
                            { "data": "PharmacyName" },
                            { "data": "PharmacyPhone" },
                            { "data": "PharmacyAddress" },
                        ],
                    });
                }
            });
            console.log("finished loading js");
        },
    }
})(jQuery);