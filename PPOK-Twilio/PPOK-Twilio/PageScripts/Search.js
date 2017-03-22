window.Search = (function ($) {
    function alertstuff(id) {
        alert(id);
        console.log(id);
    };
    return {
        init: function () {
            console.log("Loading JS");
            var dataTaco;
            $.ajax({
                type: "POST",
                url: "/Search/GetAllEvents",
                dataType: "json",
                success: function (r) {
                    console.log(r);
                    var dt = $('#example').DataTable({
                        autoFill: true,
                        "data": r,
                        "columns": [
                            { data: "EventType" },
                            { data: "Name" },
                            { data: "PrescriptionName" },
                            { data: "PrescriptionNumber" },
                            { data: "Phone" },
                            { data: "Status" },
                            { data: "SendDate" },
                            {
                                "render": function (data, type, row) {
                                    console.log(row)
                                    return ' (' + row[1] + ')';
                                },
                                targets : 6
                            }

                        ]
                    });
                }
            });
            console.log("finished loading js");
        },
        alertstuff: function (id) {
            alertstuff(id);
        },
    }

})(jQuery);