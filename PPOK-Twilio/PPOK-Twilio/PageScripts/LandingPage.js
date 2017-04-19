window.landingPage = (function ($) {
    function Send() {
        if (confirm("Are you sure you want to Send These Events?")) {
            $.ajax({
                type: "POST",
                url: "/LandingPage/Send",
                data: {  },
                dataType: "json",
                success: function (r) {
                    console.log("Successfully sent events")
                    window.history.go(0);
                }
            });
        }
    };
    return {
        init: function () {
            console.log("Loading JS");
            $('[data-toggle="tooltip"]').tooltip();
            console.log("finished loading js");
        },
        Send: function () {
            Send();
        },
    }

})(jQuery);