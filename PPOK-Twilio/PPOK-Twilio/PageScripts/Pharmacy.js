window.Pharmacy = (function ($) {
    function managePharmacy(id) {
        console.log("Got here bananasTaco : " + id);
        var url = '/SystemAdmin/SinglePharmacy?id=' + id;
        window.location.href = url;


    };
    return {
        init: function () {
            console.log("Loading JS");
            var dataTaco;
            $.ajax({
                type: "POST",
                url: "/SystemAdmin/GetAllPharmacies",
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
                            { "data": "Name" },
                            { "data": "Phone" },
                            { "data": "Address" },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {

                                    return "<button  type=\"button\" class=\"btn btn-primary\" onclick=\"window.Pharmacy.managePharmacy(" + data + ")\">Manage Pharmacy</button>";
                                }
                            },
                        ],
                        "columnDefs": [
                            { targets: 3, searchable: false }
                        ]

                    });
                }
            });
            console.log("finished loading js");
        },
        managePharmacy: function (id) {
            managePharmacy(id);
        },
    }

})(jQuery);