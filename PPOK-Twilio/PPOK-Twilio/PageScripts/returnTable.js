function remove(eventScheduleID) {

    var esi = eventScheduleID;

    alert(esi);

    $.ajax({
        url: '/LandingPage/RemoveEventScheduleByID',
        type: "POST",
        dataType: 'text',
        data: { id: esi },
        async: false,
        success: function (data) {
            alert('Success!')
            $("#reload").load('/LandingPage/ReturnTable');
        },
        error: function (data) {
            alert('Error!');
            $("#reload").load('/LandingPage/ReturnTable');
        }
    });
}