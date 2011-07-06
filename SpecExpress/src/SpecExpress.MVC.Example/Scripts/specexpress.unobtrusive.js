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

    $.validator.unobtrusive.adapters.add("specalpha", function (options) {
           setValidationValues(options, "specalpha", true);
    });

    $.validator.addMethod("specalpha", function (value, element, param) {
        if (typeof value == "string") {
            return value.match("^[a-zA-Z\s]+$");
        }
        else {
            return true;
        }
    });

    $.validator.unobtrusive.adapters.add("specnumeric", function (options) {
        setValidationValues(options, "specnumeric", true);
    });

    $.validator.addMethod("specnumeric", function (value, element, param) {
        if (typeof value == "string") {
            return value.match("^\d+$");
        }
        else {
            return true;
        }
    });

    $.validator.unobtrusive.adapters.add("specmatches", ["match"], function (options, element, param) {
        setValidationValues(options, "specmatches", options.params.match);
    });

    $.validator.addMethod("specmatches", function (value, element, param) {
        var match = param.charAt(0) == "#" ?
            parseInt($(param).val()) :
            parseInt(param);
        
        if (typeof value == "string") {
            return value.match(match);
        }
        else {
            return true;
        }
    });
    
    $.validator.unobtrusive.adapters.add("speclengthbetween", ["minlength", "maxlength"], function (options) {
        setValidationValues(options, "speclengthbetween", [parseParam(options.params.minlength),parseParam(options.params.maxlength)]);
    });

    $.validator.addMethod("speclengthbetween", function (value, element, params) {
        var minlen = params[0].charAt(0) == "#" ?
            parseInt($(params[0]).val()) :
            parseInt(params[0]);

        var maxlen = params[1].charAt(0) == "#" ?
            parseInt($(params[1]).val()) :
            parseInt(params[1]);

        return this.optional(element) || (this.getLength($.trim(value), element) >= minlen && this.getLength($.trim(value), element) <= maxlen);
    });

    $.validator.unobtrusive.adapters.add("speclengthequalto", ["length"], function (options) {
        setValidationValues(options, "speclengthequalto", parseParam(options.params.length));
    });

    $.validator.addMethod("speclengthequalto", function (value, element, param) {
        var len = param.charAt(0) == "#" ?
            parseInt($(param).val()) :
            parseInt(param);

        return this.optional(element) || this.getLength($.trim(value), element) == len;
    });

    $.validator.unobtrusive.adapters.add("specminlength", ["minlength"], function (options) {
        setValidationValues(options, "specminlength", parseParam(options.params.minlength));
    });

    $.validator.addMethod("specminlength", function (value, element, param) {
        var len = param.charAt(0) == "#" ?
            parseInt($(param).val()) :
            parseInt(param);

        return this.optional(element) || this.getLength($.trim(value), element) >= len;
    });

    $.validator.unobtrusive.adapters.add("specmaxlength", ["maxlength"], function (options) {
        setValidationValues(options, "specmaxlength", parseParam(options.params.maxlength));
    });

    $.validator.addMethod("specmaxlength", function (value, element, param) {
        var len = param.charAt(0) == "#" ?
            parseInt($(param).val()) :
            parseInt(param);

        return this.optional(element) || this.getLength($.trim(value), element) <= len;
    });
    
    $.validator.unobtrusive.adapters.add("specgreaterthan", ["greaterthan"], function (options) {
        setValidationValues(options, "specgreaterthan", parseParam(options.params.greaterthan));
    });

    $.validator.addMethod("specgreaterthan", function (value, element, param) {
        var greaterThan = param.charAt(0) == "#" ?
            $(param).val() :
            param;

        return this.optional(element) || value > greaterThan;
    });

    $.validator.unobtrusive.adapters.add("speclessthan", ["lessthan"], function (options) {
        setValidationValues(options, "speclessthan", parseParam(options.params.lessthan));
    });

    $.validator.addMethod("speclessthan", function (value, element, param) {
        var lessThan = param.charAt(0) == "#" ?
            $(param).val() :
            param;

        return this.optional(element) || value < lessThan;
    });
    
    $.validator.unobtrusive.adapters.add("specgreaterthanequalto", ["greaterthanequalto"], function (options) {
        setValidationValues(options, "specgreaterthanequalto", parseParam(options.params.greaterthan));
    });

    $.validator.addMethod("specgreaterthanequalto", function (value, element, param) {
        var greaterThan = param.charAt(0) == "#" ?
            $(param).val() :
            param;

        return this.optional(element) || value >= greaterThan;
    });

    $.validator.unobtrusive.adapters.add("speclessthanequalto", ["lessthanequalto"], function (options) {
        setValidationValues(options, "speclessthanequalto", parseParam(options.params.lessthanequalto));
    });

    $.validator.addMethod("speclessthanequalto", function (value, element, param) {
        var lessThan = param.charAt(0) == "#" ?
            $(param).val() :
            param;

        return this.optional(element) || value <= lessThan;
    });

    $.validator.unobtrusive.adapters.add("specbetween", ["floor","ceiling"], function (options) {
        setValidationValues(options, "specbetween", [parseParam(options.params.floor),parseParam(options.params.ceiling)]);
    });

    $.validator.addMethod("specbetween", function (value, element, params) {
        var floor = params[0].charAt(0) == "#" ?
            $(params[0]).val() :
            param[0];

        var ceiling = params[1].charAt(0) == "#" ?
            $(params[1]).val() :
            param[1];

        return this.optional(element) || (value >= floor && value <= ceiling);
    });

    $.validator.unobtrusive.adapters.add("specequalto", ["equalto"], function (options) {
        setValidationValues(options, "specequalto", parseParam(options.params.equalto));
    });

    $.validator.addMethod("specequalto", function (value, element, params) {
        var equalto = params[0].charAt(0) == "#" ?
            $(param).val() :
            param;

        return this.optional(element) || value == equalto;
    });
} (jQuery));


