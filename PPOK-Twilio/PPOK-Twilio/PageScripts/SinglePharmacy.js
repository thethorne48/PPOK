﻿window.singlePharmacy = (function ($) {
    function add(id) {
        $('#PharmacyCode').val(id);
        $('#AddModal').modal('toggle');
    };
    function editPharmacy() {
        console.log("Got here taco : ");
        $('#EditPharmacy').modal('toggle');
        //$.ajax({
        //    type: "POST",
        //    url: "/SystemAdmin/getSinglePharmacy", //cause every programmer Hurrttssss ::FeelsBadMan:: 
        //    data: { PharmacyId },
        //    dataType: "json",
        //    success: function (r) {
        //        console.log(r);
        //        $('#IsActive').prop('checked', r.isActive);
        //        $('#IsAdmin').prop('checked', r.isAdmin);
        //        $("#PharmacyCode").val(r.PharmacyCode);
        //        $("#FirstName").val(r.FirstName);
        //        $("#LastName").val(r.LastName);
        //        $("#Email").val(r.Email);
        //        $("#Phone").val(r.Phone);
        //        $('#EditModal').modal('toggle');
        //    }
    };
    //);
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
                $("#IsActive").prop('checked', r.isActive);
                $("#IsAdmin").prop('checked', r.isAdmin);
                $("#PharmacyCode").val(r.PharmacyCode);
                $("#FirstName").val(r.FirstName);
                $("#LastName").val(r.LastName);
                $("#Email").val(r.Email);
                $("#editPhone").val(r.Phone);
                $('#EditModal').modal('toggle');
            }
        });
    };

    function inactivate(PharmacyId) {
        if (confirm("Are you sure you want to Inactivate this Pharmacy? " + PharmacyId)) {
            $.ajax({
                type: "POST",
                url: "/SystemAdmin/Inactivate", //cause every programmer Hurrttssss ::FeelsBadMan:: 
                data: { PharmacyId },
                dataType: "json",
                success: function () {
                    console.log("successful delete")
                    window.history.go(0);
                }
            });
        }
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
        editPharmacy: function () {
            editPharmacy();
        },
        add: function (id) {
            add(id);
        },
        inactivate: function (PharmacyId) {
            inactivate(PharmacyId);
        },
    }

})(jQuery);