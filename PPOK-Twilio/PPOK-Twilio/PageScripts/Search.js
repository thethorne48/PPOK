window.Search = (function ($) {
    function showDetails(id) {
        alert("tacos - showing details for event #" + id);
        //call somehting to pop up an edit modal
    };
    function deleteEvent(id) {
        alert("tacos - you sure you want to delete event #" + id);
        //call somehting to pop up an edit modal
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
                            { "data": "EventType" },
                            { "data": "Name" },
                            { "data": "PrescriptionName" },
                            { "data": "PrescriptionNumber" },
                            { "data": "Phone" },
                            { "data": "Status" },
                            { "data": "SendDate" },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {

                                    return "<button  type=\"button\" class=\"btn btn-primary\" onclick=\"window.Search.showDetails(" + data + ")\">Details</button>";
                                }
                            },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {
                                    return "<button type=\"button\" class=\"btn btn-danger\" onclick=\"window.Search.deleteEvent(" + data + ")\">   <span class=\"glyphicon glyphicon-trash\" \"></span></button>";
                                }
                            }
                        ]

                    });
                }
            });
            console.log("finished loading js");
        },
        showDetails: function (id) {
            showDetails(id);
        },
        deleteEvent: function (id) {
            deleteEvent(id);
        },
    }

})(jQuery);