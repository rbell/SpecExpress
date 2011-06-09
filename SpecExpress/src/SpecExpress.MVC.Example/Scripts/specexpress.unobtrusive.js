(function ($) {
    /*http://devtrends.co.uk/blog/the-complete-guide-to-validation-in-asp.net-mvc-3-part-2*/

    $.validator.addMethod("specminlength", function (value, element, params) {
        var len;
        if (params.minlengthExp) {
            var otherProp = $('#' + params.minlengthExp);
            len = otherProp.val();
            this.settings.rules["specminlength"] = len;
        }
        else {
            len = params;
        }

        return this.optional(element) || this.getLength($.trim(value), element) >= len;
    });

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
    $.validator.unobtrusive.adapters.add("specminlength", ["minlength", "other"], function (options) {
        if (!options.params.minlength) {
            setValidationValues(options, "specminlength", { minlengthExp: options.params.other });
        }
        else {
            setValidationValues(options, "specminlength", { minlength: options.params.minlength });
        }
    });

    $.validator.unobtrusive.adapters.add("specmaxlength", ["length"], function (options) {
        setValidationValues(options, "maxlength", options.params.length);
    });
} (jQuery));


