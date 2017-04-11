window.singlePharmacy = (function ($) {
    function add(id) {
        $('#PharmacyCode').val(id);
        $('#AddModal').modal('toggle');
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
                $("#PharmacistCode").val(r.Code);
                $('#IsActive').prop('checked', r.isActive);
                $('#IsAdmin').prop('checked', r.isAdmin);
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
        edit: function (id, PharmacyId) {
            edit(id, PharmacyId);
        },
        add: function (id) {
            add(id);
        },
    }

})(jQuery);