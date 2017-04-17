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
            url: "/ManagePharmacist/GetSinglePharmacist", //cause every programmer Hurrttssss ::FeelsBadMan:: 
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
                $("#IsActive").prop("checked", r.isActive);
                $("#IsAdmin").prop("checked", r.isAdmin);
                $('#EditModal').modal('toggle');
            }
        });
    };

    function editPharmacy(PharmacyId) {
        console.log("Got here taco : " + PharmacyId);
        $.ajax({
            type: "POST",
            url: "/ManagePharmacist/GetSinglePharmacy", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { PharmacyId },
            dataType: "json",
            success: function (r) {
                console.log(r);
                $("#PharmacyCode").val(r.PharmacyCode);
                $("#Name").val(r.Name);
                $("#Address").val(r.Address);
                $("#Phone").val(r.Phone);
                $('#EditPharmacyModal').modal('toggle');
            }
        });
    };

    function add() {
        $('#AddModal').modal('toggle');
    };

    return {
        init: function () {
            console.log("Loading JS");
            $('#myTable').DataTable();
            console.log("finished loading js");
        },
        edit: function (id, PharmacyId) {
            edit(id, PharmacyId);
        },
        editPharmacy: function (PharmacyId) {
            editPharmacy(PharmacyId);
        },
        add: function () {
            add();
        },
        inactivatePharmacist: function (id) {
            inactivatePharmacist(id);
        },
    }

})(jQuery);