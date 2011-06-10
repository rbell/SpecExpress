(function ($) {
    /*http://devtrends.co.uk/blog/the-complete-guide-to-validation-in-asp.net-mvc-3-part-2*/

    $.validator.format = function (source, params) {
        if (arguments.length == 1)
            return function () {
                var args = $.makeArray(arguments);
                args.unshift(source);
                return $.validator.format.apply(this, args);
            };
        if (arguments.length > 2 && params.constructor != Array) {
            params = $.makeArray(arguments).slice(1);
        }
        if (params.constructor != Array) {
            params = [params];
        }
        $.each(params, function (i, n) {
            if (typeof n == "string" && n.substring(0, 1) == "#") {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), $(n).val());
            }
            else {
                source = source.replace(new RegExp("\\{" + i + "\\}", "g"), n);
            }
        });
        return source;
    };

    $.validator.addMethod("specminlength", function (value, element, params) {
        var len = typeof params == "string" ?
            parseInt($(params).val()) :
            params;

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
            setValidationValues(options, "specminlength", "#" + options.params.other);
        }
        else {
            setValidationValues(options, "specminlength", options.params.minlength);
        }
    });

    $.validator.unobtrusive.adapters.add("specmaxlength", ["length"], function (options) {
        setValidationValues(options, "maxlength", options.params.length);
    });
} (jQuery));


