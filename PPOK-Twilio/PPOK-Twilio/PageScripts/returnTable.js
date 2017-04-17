function remove(eventScheduleID, row) {
    
    var esi = eventScheduleID;

    $.ajax({
        url: '/LandingPage/RemoveEventScheduleByID',
        type: "POST",
        dataType: 'text',
        data: { id: esi },
        async: false,
        success: function (data) {
            alert('Success!')
            var table = $('#myTable').DataTable();
            alert($(this).parents('tr').index);
            table.row($(this).parents('tr').index).remove().draw();
            
            //$("#reload").load('/LandingPage/ReturnTable');
        },
        error: function (data) {
            alert('Error!');
        }
    });
}