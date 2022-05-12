jQuery.validator.addMethod("validatephonenumber",
    function (value, element, params) {
        if (value) {
            var iti = window.intlTelInputUtils;
            var cc = element.getAttribute('data-intl-input-cc');
            var isValid = iti.isValidNumber(value, cc);
            return isValid;
        }

        return true;
    });
jQuery.validator.unobtrusive.adapters.addBool("validatephonenumber");