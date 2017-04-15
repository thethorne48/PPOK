window.SysAdAdmin = (function ($) {
    return {
        init: function () {
            console.log("Loading JS");
            var dataTaco;
            $.ajax({
                type: "POST",
                url: "/SystemAdmin/GetAllAdmins",
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
                        ],
                    });
                }
            });
            console.log("finished loading js");
        },
    }
})(jQuery);