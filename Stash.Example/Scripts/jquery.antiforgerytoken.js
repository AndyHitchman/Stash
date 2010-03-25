;(function($) {
    $.getAntiForgeryTokenQueryString = function() {
        return $(document.getElementsByName("__RequestVerificationToken")).fieldSerialize();
    };
    $.paramsWithAntiForgeryToken = function(parameters) {
        var afParams = $.extend({
            __RequestVerificationToken: $(document.getElementsByName("__RequestVerificationToken")).val()
        }, parameters || {});

        return afParams;
    };
})(jQuery);

