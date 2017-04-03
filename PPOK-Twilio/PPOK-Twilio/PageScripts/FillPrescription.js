window.fillPrescription = (function ($) {
    function fill(id) {
        if (confirm("Are you sure you want to fill this Prescription?")) {
            $.ajax({
                type: "POST",
                url: "/FillPrescription/Fill",
                data: { id },
                dataType: "json",
                success: function (r) {
                    window.history.go(0);
                }
            });
        }
    };
    return {
        init: function () {
            console.log("Loading JS");
            var dataTaco;
            $.ajax({
                type: "POST",
                url: "/FillPrescription/GetAllFilledPrescriptions",
                dataType: "json",
                success: function (r) {
                    console.log(r);
                    var dt = $('#example').DataTable({
                        "data": r,
                        "columns": [
                            { "data": "Name" },
                            { "data": "PrescriptionName" },
                            { "data": "PrescriptionNumber" },
                            { "data": "Phone" },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {
                                    return "<button type=\"button\" class=\"btn btn-primary\" onclick=\"window.fillPrescription.fill(" + data + ")\">   <span>Fill</span></button>";
                                }
                            }
                        ]

                    });
                }
            });
            console.log("finished loading js");
        },
        fill: function (id) {
            fill(id);
        },
    }

})(jQuery);