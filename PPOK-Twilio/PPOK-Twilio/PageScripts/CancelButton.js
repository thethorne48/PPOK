window.cancelButton = (function ($) {
    var buttonNames = ['primary', 'secondary', 'success', 'info', 'warning', 'danger'].map(function (str) { return 'btn-' + str; });
    function transform(button){
        var oldContent = button.contents();
        button.html('Cancel<div class="progress" />');

        var prevClasses = [];
        for (let i = 0; i < buttonNames.length; i++) {
            var clazz = buttonNames[i];
            if (button.hasClass(clazz)) {
                button.removeClass(clazz);
                prevClasses.push(clazz);
            }
        }

        button.addClass('btn-danger');

        return function () {
            button.empty();
            button.append(oldContent);
            button.removeClass('btn-danger');
            prevClasses.forEach(function (clazz) { button.addClass(clazz); });
        };
    }
    var warningID;
    function warn(time) {
        if (warningID != null)
            clearTimeout(warningID);
        window.onbeforeunload = function () { return true; };
        warningID = setTimeout(function () {
            warningID = null;
            window.onbeforeunload = null;
        }, time);
    }
    return {
        init: function () {
            console.log('Loading Cancel Buttons.');
            $('.cancel-button').each(function () {
                var button = $(this);

                var listener = this.onclick;
                this.onclick = null;

                var confirmation = button.data('confirm') || null;
                var delay = button.data('delay') || 5000;
                var undo;
                var id;

                button.click(function () {
                    if (id) {
                        clearTimeout(id);
                        id = null;
                        undo();
                    }else
                    if(confirmation == null || confirm(confirmation)) {
                        undo = transform(button);
                        warn(delay);
                        id = setTimeout(function () {
                            id = null;
                            undo();

                            listener.apply(this, arguments);
                        }, delay);
                    }
                });
            });
        }
    }
})(jQuery);