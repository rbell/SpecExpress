(function ($) {
    /*http://devtrends.co.uk/blog/the-complete-guide-to-validation-in-asp.net-mvc-3-part-2*/

    $.format = function (source, params) {
        // My Format
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

    function setValidationValues(options, ruleName, value) {
        options.rules[ruleName] = value;
        if (options.message) {
            options.messages[ruleName] = options.message;
        }
    }

    function parseParam(paramVal) {
        var jsonParam = eval('(' + paramVal + ')');
        if (jsonParam.isProperty) {
            return "#" + jsonParam.propertyName;
        }
        else {
            return paramVal;
        }
    }
    
    $.validator.unobtrusive.adapters.add("specrequired", function (options) {
        // jQuery Validate equates "required" with "mandatory" for checkbox elements
        if (options.element.tagName.toUpperCase() !== "INPUT" || options.element.type.toUpperCase() !== "CHECKBOX") {
            setValidationValues(options, "required", true);
        }
    });

    $.validator.addMethod("specminlength", function (value, element, param) {
        var len = param.charAt(0) == "#" ?
            parseInt($(param).val()) :
            parseInt(param);

        return this.optional(element) || this.getLength($.trim(value), element) >= len;
    });
    
    $.validator.unobtrusive.adapters.add("specminlength", ["minlength"], function (options) {
        setValidationValues(options, "specminlength", parseParam(options.params.minlength));
    });

    $.validator.addMethod("specmaxlength", function (value, element, param) {
        var len = param.charAt(0) == "#" ?
            parseInt($(param).val()) :
            parseInt(param);

        return this.optional(element) || this.getLength($.trim(value), element) <= len;
    });
    
    $.validator.unobtrusive.adapters.add("specmaxlength", ["maxlength"], function (options) {
        setValidationValues(options, "specmaxlength", parseParam(options.params.maxlength));
    });
    
} (jQuery));


