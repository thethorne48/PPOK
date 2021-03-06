﻿window.Search = (function ($) {
    function showDetails(id) {
        console.log("Got here taco : " + id);
        $.ajax({
            type: "POST",
            url: "/Search/GetSingleEvent", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id },
            dataType: "json",
            success: function (r) {
                console.log(r);
                $("#Name").html(r.Name); //.val
                document.getElementById("Phone").innerHTML = r.Phone;
                document.getElementById("Email").innerHTML = r.Email;
                document.getElementById("CurrPref").innerHTML = r.CurrPref;
                document.getElementById("SentType").innerHTML = r.SentType;
                document.getElementById("PrescriptionName").innerHTML = r.PrescriptionName;
                document.getElementById("PrescriptionNumber").innerHTML = r.PrescriptionNumber;
                document.getElementById("Status").innerHTML = r.Status;
                document.getElementById("SendDate").innerHTML = r.sendDate || "null"
                document.getElementById("FillDate").innerHTML = r.FillDate;
                document.getElementById("FillPharmacist").innerHTML = r.FillPharmacist;
                document.getElementById("RejectedBy").innerHTML = r.RejectedBy;
                document.getElementById("RejectedDate").innerHTML = r.RejectedDate;
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
                url: "/Search/GetAllEvents",
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
                            { "data": "EventType" },
                            { "data": "Name" },
                            { "data": "PrescriptionName" },
                            { "data": "PrescriptionNumber" },
                            { "data": "Phone" },
                            { "data": "Status" },
                            { "data": "LastActivity" },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {

                                    return "<button  type=\"button\" class=\"btn btn-primary\" onclick=\"window.Search.showDetails(" + data + ")\">Details</button>";
                                }
                            },
                            {
                                "data": "Code",
                                "render": function (data, type, row) {
                                    var confirmation = "Are you sure you want to inactivate this event? " + data + " of type " + type;
                                    return '<button type="button" class="btn btn-danger cancel-button" onclick="window.Search.inactivateEvent(' + data + ',"' + type + '")" data-confirm="' + confirmation + '">X</button>';
                                }
                            }
                        ],
                        "columnDefs": [
                            { targets: [7,8], searchable: false }
                        ]
                    });
                    window.cancelButton.init();
                }
            });
            console.log("finished loading js");
        },
        showDetails: function (id) {
            showDetails(id);
        },
        inactivateEvent: function (id, eventType) {
            $.ajax({
                type: "POST",
                url: "/Search/Inactivate",
                data: { id, eventType },
                dataType: "json",
                success: function (r) {
                    window.history.go(0);
                }
            });
        },
    }

})(jQuery);