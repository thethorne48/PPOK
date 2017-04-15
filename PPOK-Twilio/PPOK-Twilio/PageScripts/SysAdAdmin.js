window.SysAdAdmin = (function ($) {
    function add() {
        $("#AdminModal").modal('toggle');
    };

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
            $.get("/SystemAdmin/AdminModal", function (data) {
                console.log("Getting modal");
                console.log(data);
                $('.datatablesCustomBack').after(data);
            })
            console.log("finished loading js");
        },
        add: function () {
            add();
        }
    }
})(jQuery);