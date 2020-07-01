function activeisUmbracoFormsIntlTelInput(fieldId, enableIPBasedCountry, initialCountry, autoPlaceholder, ipInfoKey, placeholderType) {

    var input = document.querySelector("#" + fieldId);

        var intlTelInputOptions = {};

    intlTelInputOptions.initialCountry = initialCountry;

    if (enableIPBasedCountry) {
        intlTelInputOptions.geoIpLookup = function(success, failure) {
            $.get("https://ipinfo.io/json?token=" + ipInfoKey, function() {}, "json").always(function(resp) {
                var countryCode = (resp && resp.country) ? resp.country : "gb";
                success(countryCode);
            });
        }
    }

    if (autoPlaceholder) {
        intlTelInputOptions.autoPlaceholder = "aggressive";
    }

    if (placeholderType) {
        intlTelInputOptions.placeholderNumberType = placeholderType;
    }

    var iti = intlTelInput(input,
        intlTelInputOptions);

    var output = document.querySelector("#phone_intl_" + fieldId);

    var handleChange = function () {
        var number = (iti.isValidNumber()) ? iti.getNumber() : "";
        var cc = iti.getSelectedCountryData().iso2;
        input.setAttribute('data-intl-input-cc', cc);
        output.value = number;
    };

    input.addEventListener('change', handleChange);
    input.addEventListener('keyup', handleChange);
}