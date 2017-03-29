window.SysAdPharmacist = (function ($) {
    function inactivatePharmacist(id) {
        if (confirm("Are you sure you want to inactivate this Pharmacist? " + id)) {
            $.ajax({
                type: "POST",
                url: "/SystemAdmin/InactivatePharmacist",
                data: { id },
                dataType: "json",
                success: function (r) {
                    window.history.go(0);
                }
            });
        }
    };

    function edit(id) {
        console.log("Got here taco : " + id);
        $.ajax({
            type: "POST",
            url: "/SystemAdmin/GetSinglePharmacist", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id },
            dataType: "json",
            success: function (r) {
                console.log(r);
                document.getElementById("FirstName").value = r.FirstName;
                document.getElementById("LastName").value = r.LastName;
                document.getElementById("Email").value = r.Email;
                document.getElementById("Phone").value = r.Phone;
                //fill dropdown of other pharmacies
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
                        autoFill: true,
                        "data": r,
                        "columns": [
                            { "data": "FirstName" },
                            { "data": "LastName" },
                            { "data": "Email" },
                            { "data": "Phone" },
                           // { "data": "PharmacyName" },
                            //{ "data": "PharmacyPhone" },
                            //{ "data": "PharmacyAddress" },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {

                                    return "<button  type=\"button\" class=\"btn btn-primary\" onclick=\"window.SysAdPharmacist.edit(" + data + ")\">Edit</button>";
                                }
                            },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {
                                    //if status is !inactive do this
                                    return "<button type=\"button\" class=\"btn btn-danger\" onclick=\"window.SysAdPharmacist.inactivatePharmacist(" + data + ")\">   <span>X</span></button>";
                                    //else status is inactive return this stuffs
                                }
                            }
                        ]

                    });
                }
            });
            console.log("finished loading js");
        },
        inactivatePharmacist: function (id) {
            inactivatePharmacist(id);
        },
        edit: function (id) {
            edit(id);
        },
    }

})(jQuery);