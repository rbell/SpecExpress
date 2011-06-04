(function ($) {
    /*http://devtrends.co.uk/blog/the-complete-guide-to-validation-in-asp.net-mvc-3-part-2*/

    function setValidationValues(options, ruleName, value) {
        options.rules[ruleName] = value;
        if (options.message) {
            options.messages[ruleName] = options.message;
        }
    }

    $.validator.unobtrusive.adapters.add("specrequired", function (options) {
        // jQuery Validate equates "required" with "mandatory" for checkbox elements
        if (options.element.tagName.toUpperCase() !== "INPUT" || options.element.type.toUpperCase() !== "CHECKBOX") {
            setValidationValues(options, "required", true);
        }
    });

    //map to SpecExpress 
    $.validator.unobtrusive.adapters.add("specminlength", ["length"], function (options) {
        setValidationValues(options, "minlength", options.params.length);
    });

    $.validator.unobtrusive.adapters.add("specmaxlength", ["length"], function (options) {
        setValidationValues(options, "maxlength", options.params.length);
    });
    

} (jQuery));


