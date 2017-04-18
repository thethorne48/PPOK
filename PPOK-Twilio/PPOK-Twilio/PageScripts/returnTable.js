function remove(eventScheduleID, row) {
    
    var esi = eventScheduleID;

    $.ajax({
        url: '/LandingPage/RemoveEventScheduleByID',
        type: "POST",
        dataType: 'text',
        data: { id: esi },
        async: false,
        success: function (data) {
            var r = confirm("Are you sure you want to remove this from the send list?");
            if (r == true) {
                var table = $('#myTable').DataTable();
                table.row("#" + row).remove().draw();
            }            
            //$("#reload").load('/LandingPage/ReturnTable');
        },
        error: function (data) {
            alert('Error!');
        }
    });
}