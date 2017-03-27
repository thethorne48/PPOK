window.Search = (function ($) {
    function deleteEvent(id) {
        alert("tacos - you sure you want to delete event #" + id);
        //call somehting to pop up an edit modal
    };

    
    function showDetails(id) {
        console.log("Got here taco : " + id);
        $.ajax({
            type: "POST",
            url: "/Search/GetSingleEvent", //cause every programmer Hurrttssss ::FeelsBadMan:: 
            data: { id },
            dataType: "json",
            success: function (r) {
                console.log(r);
                document.getElementById("Code").value = r.Code;
                document.getElementById("Name").value = r.Name;
                document.getElementById("Phone").value = r.Phone;
                document.getElementById("Email").value = r.Email;
                document.getElementById("CurrPref").value = r.CurrPref;
                document.getElementById("SentType").value = r.SentType;
                document.getElementById("PrescriptionName").value = r.PrescriptionName;
                document.getElementById("PrescriptionNumber").value = r.PrescriptionNumber;
                document.getElementById("Status").value = r.Status;
                document.getElementById("SendDate").value = r.SendDate;
                document.getElementById("FillDate").value = r.FillDate;
                document.getElementById("FillPharmacist").value = r.FillPharmacist;
                document.getElementById("RejectedBy").value = r.RejectedBy;
                document.getElementById("RejectedDate").value = r.RejectedDate;
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