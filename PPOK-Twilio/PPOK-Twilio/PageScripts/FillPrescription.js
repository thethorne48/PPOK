window.fillPrescription = (function ($) {
    var warningID = null;
    function protect(func, delay) {
        //cancel the previous nav warning
        if (warningID != null)
            clearTimeout(warningID);

        //enable navigation warning
        window.onbeforeunload = function () { return true; };

        warningID = setTimeout(function () {
            window.onbeforeunload = null;
        }, delay);

        setTimeout(function () {
            func();
        }, delay);
    }

    function fill(button, id) {
        if (confirm("Are you sure you want to fill this Prescription?")) {

            var oldContent = button.innerHTML;
            button.innerHTML = 'Cancel<div class="progress" />';
            button.classList.remove('btn-primary');
            button.classList.add('btn-danger');

            protect(function () {
                button.classList.remove('btn-danger');
                button.classList.add('btn-primary');
                button.innerHTML = oldContent;

                $.ajax({
                    type: "post",
                    url: "/fillprescription/fill",
                    data: { id },
                    datatype: "json",
                    success: function (r) {
                        window.history.go(0);
                    }
                });
            }, 5000);
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
                                    return "<button type=\"button\" class=\"btn btn-primary cancel-button\" onclick=\"window.fillPrescription.fill(this, " + data + ")\">Fill</button>";
                                }
                            }
                        ]

                    });
                }
            });
            console.log("finished loading js");
        },
        fill: function (button, id) {
            fill(button, id);
        },
    }

})(jQuery);