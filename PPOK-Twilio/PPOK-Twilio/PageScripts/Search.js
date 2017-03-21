window.Search = (function ($) {
    function alertstuff(id) {
        //if (confirm("Are you sure you want to delete this record? " + id)) {
        //    $.ajax({
        //        type: "POST",
        //        url: "/ManageBadge/Delete",
        //        data: { id },
        //        dataType: "json",
        //        success: function (r) {
        //            window.history.go(0);
        //        }
        //    });
        //}
    };
    function EditBadge(id) {
        //console.log("Got here taco : " + id);
        //$.ajax({
        //    type: "POST",
        //    url: "/ManageBadge/GetSingleBadge",
        //    data: { id },
        //    dataType: "json",
        //    success: function (r) {
        //        console.log(r);
        //        document.getElementById("editId").value = r.result.ID;
        //        document.getElementById("editName").value = r.result.Name;
        //        document.getElementById("editDescript").value = r.result.Descript || "";
        //        var element = document.getElementById('editSenderType');
        //        element.value = r.result2;
        //        var element2 = document.getElementById('editBadgeType');
        //        element2.value = r.result3;
        //        document.getElementById("editImgLink").value = r.result.ImgLink;
        //        document.getElementById("editActDate").value = new Date(parseInt(r.result.ActDate.substr(6))).toLocaleDateString("en-US") || "";
        //        console.log(new Date(parseInt(r.result.ActDate.substr(6))).toLocaleDateString("en-US"));
        //        if (r.result.DeactDate != null)
        //            document.getElementById("editDeactDate").value = new Date(parseInt(r.result.DeactDate.substr(6))).toLocaleDateString("en-US");
        //        else
        //            document.getElementById("editDeactDate").value = "";
        //        $('#EditModal').modal('toggle');
        //    }
        //});
    };
    return {
        init: function () {
            console.log("Loading JS");
            var dataTaco;
            $.ajax({
                type: "POST",
                url: "/Search/GetAllEvents",
                dataType: "json",
                success: function (r) { //recursion limit exceeded
                    console.log(r);
                    $('#example').DataTable({
                        "data": r,
                        "columns": [
                            { data: "EventType" },
                            { data: "Name" },
                            { data: "PrescriptionName" },
                            { data: "PrescriptionNumber" },
                            { data: "Phone" },
                            { data: "Status" },
                            { data: "SendDate" }
                        ]
                    });
                }
            });
            console.log("finished loading js");
        },
        alertstuff: function (id) {
            alertstuff(id);
        },
        EditBadge: function (item) {
            EditBadge(item);
        },
    }

})(jQuery);