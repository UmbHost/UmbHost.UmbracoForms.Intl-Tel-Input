function activeisUmbracoFormsIntlTelInput(input, enableIPBasedCountry, initialIPCountry) {

    var intlTelInputOptions = {};

    if (enableIPBasedCountry) {
        intlTelInputOptions.initialCountry = initialIPCountry;
        intlTelInputOptions.geoIpLookup = function(success, failure) {
            $.get("https://ipinfo.io", function() {}, "jsonp").always(function(resp) {
                var countryCode = (resp && resp.country) ? resp.country : "";
                success(countryCode);
            });
        }
    }

    intlTelInput(input,
        intlTelInputOptions);
}