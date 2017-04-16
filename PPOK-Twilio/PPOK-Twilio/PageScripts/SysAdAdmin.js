window.SysAdAdmin = (function ($) {
    function add() {
        $("#AdminModal").modal('toggle');
    };

    function edit(id) {
        console.log("Got here taco : " + id);
        $.ajax({
            type: "POST",
            url: "/SystemAdmin/GetSingleAdmin", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id },
            dataType: "json",
            success: function (r) {
                console.log(r);
                $("#Code").val(r.Code);
                $("#editIsActive").prop('checked', r.isActive);
                $("#editFirstName").val(r.FirstName);
                $("#editLastName").val(r.LastName);
                $("#editEmail").val(r.Email);
                $("#editPhone").val(r.Phone);
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
                            {
                                "data": "Code",
                                "render": function (data, type, row) {
                                    return "<button type=\"button\" class=\"btn btn-primary\" onclick=\"window.SysAdAdmin.edit(" + data + ")\"><span>edit</span></button>";
                                }
                            }
                        ],
                    });
                }
            });
            console.log("finished loading js");
        },
        add: function () {
            add();
        },
        edit: function (id) {
            edit(id);
        }
    }
})(jQuery);