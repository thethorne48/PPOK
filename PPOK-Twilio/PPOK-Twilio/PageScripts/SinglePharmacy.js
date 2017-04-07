window.singlePharmacy = (function ($) {
    function inactivatePharmacist(id) {
        if (confirm("Are you sure you want to inactivate this Pharmacist? " + id)) {
            $.ajax({
                type: "POST",
                url: "/Search/Inactivate",
                data: { id },
                dataType: "json",
                success: function (r) {
                    window.history.go(0);
                }
            });
        }
    };

    function edit(id, PharmacyId) {
        console.log("Got here taco : " + id);
        $.ajax({
            type: "POST",
            url: "/SystemAdmin/GetSinglePharmacist", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id,PharmacyId },
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
        inactivatePharmacist: function (id) {
            inactivatePharmacist(id);
        },
    }

})(jQuery);