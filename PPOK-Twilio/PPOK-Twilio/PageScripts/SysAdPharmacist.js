window.SysAdPharmacist = (function ($) {
    function inactivatePharmacist(id, PharmacyId) {
        if (confirm("Are you sure you want to inactivate this Pharmacist? " + id)) {
            $.ajax({
                type: "POST",
                url: "/SystemAdmin/InactivatePharmacist",
                data: { id, PharmacyId },
                dataType: "json",
                success: function (r) {
                    window.history.go(0);
                }
            });
        }
    };
    function add() {
        $.ajax({
            success: function () {
                $('#AddModal').modal('toggle');
            }
        });
    };
    function edit(id, PharmacyId) {
        console.log("Got here taco : " + id);
        $.ajax({
            type: "POST",
            url: "/SystemAdmin/GetSinglePharmacist", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id, PharmacyId },
            dataType: "json",
            success: function (r) {
                console.log(r);
                $("#Code").val(r.Code);
                $("#PharmacyCode").val(r.PharmacyCode);
                $("#FirstName").val(r.FirstName);
                $("#LastName").val(r.LastName);
                $("#Email").val(r.Email);
                $("#Phone").val(r.Phone);
                $('#EditModal').modal('toggle');
            }
        });
    };
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
                            {
                                "data": "Code",
                                "render": function (data, type, row) {

                                    return "<button  type=\"button\" class=\"btn btn-primary\" onclick=\"window.SysAdPharmacist.edit(" + row.Code + "," + row.PharmacyCode + ")\">Edit</button>";
                                }

                            },
                             {
                                 "data": "Code",
                                 "render": function (data, type, row) {
                                     return "<button  type=\"button\" class=\"btn btn-danger\" onclick=\"window.SysAdPharmacist.inactivatePharmacist(" + row.Code + "," + row.PharmacyCode + ")\">X</button>";
                                 }

                             },
                        ],
                        "columnDefs": [
                                 { targets: [7, 8], searchable: false }
                        ],

                    });
                }
            });
            console.log("finished loading js");
        },
        inactivatePharmacist: function (id, PharmacyId) {
            inactivatePharmacist(id, PharmacyId);
        },
        edit: function (id, PharmacyId) {
            edit(id, PharmacyId);
        },
        add: function () {
            add();
        },
    }
})(jQuery);