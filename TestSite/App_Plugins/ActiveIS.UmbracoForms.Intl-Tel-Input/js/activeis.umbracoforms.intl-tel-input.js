function activeisUmbracoFormsIntlTelInput(input, enableIPBasedCountry, initialIPCountry, autoPlaceholder, ipInfoKey) {

    var intlTelInputOptions = {};

    if (enableIPBasedCountry) {
        intlTelInputOptions.initialCountry = initialIPCountry;
        intlTelInputOptions.geoIpLookup = function(success, failure) {
            $.get("https://ipinfo.io/json?token=" + ipInfoKey, function() {}, "json").always(function(resp) {
                var countryCode = (resp && resp.country) ? resp.country : "";
                success(countryCode);
            });
        }
    }

    if (autoPlaceholder) {
        intlTelInputOptions.autoPlaceholder = "aggressive";
    }

    intlTelInput(input,
        intlTelInputOptions);
}