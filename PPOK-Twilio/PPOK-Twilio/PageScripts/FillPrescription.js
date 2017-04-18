window.fillPrescription = (function ($) {
    return {
        init: function () {
            console.log("Loading JS");
            var dataTaco;
            $.ajax({
                type: "POST",
                url: "/FillPrescription/GetAllFilledPrescriptions",
                dataType: "json",
                success: function (r) {
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
                                    return '<button type="button" class="btn btn-primary cancel-button" onclick="window.fillPrescription.click(' + data + ')" data-confirm="Are you sure you want to fill this Prescription?">Fill</button>';
                                }
                            }
                        ]

                    });
                    window.cancelButton.init();
                }
            });
            console.log("finished loading js");
        },
        click: function (id) {
            $.ajax({
                type: "post",
                url: "/fillprescription/fill",
                data: { id },
                datatype: "json",
                success: function (r) {
                    window.history.go(0);
                }
            });
        },
    }

})(jQuery);