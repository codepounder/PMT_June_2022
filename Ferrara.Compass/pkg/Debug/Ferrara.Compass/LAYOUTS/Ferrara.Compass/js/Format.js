"use strict";

(function ($) {
    if (!$.browser) {
        $.browser = {};
        $.browser.mozilla = /mozilla/.test(navigator.userAgent.toLowerCase()) && !/webkit/.test(navigator.userAgent.toLowerCase());
        $.browser.webkit = /webkit/.test(navigator.userAgent.toLowerCase());
        $.browser.opera = /opera/.test(navigator.userAgent.toLowerCase());
        $.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());
    }

    var methods = {
        destroy: function () {
            $(this).unbind(".numericValidation");

            if ($.browser.msie) {
                this.onpaste = null;
            }
            return this;
        },

        init: function (parameters) {
            parameters = $.extend({
                prefix: "",
                suffix: "",
                thousands: ",",
                decimal: ".",
                precision: 2,
                allowNegative: false
            }, parameters);

            return this.each(function () {
                var $input = $(this), settings,
                    onFocusValue;

                // data-* api
                settings = $.extend({}, parameters);
                settings = $.extend(settings, $input.data());

                function setSymbol(value) {
                    var operator = "";
                    if (value.indexOf("-") > -1) {
                        value = value.replace("-", "");
                        operator = "-";
                    }
                    return operator + settings.prefix + value + settings.suffix;
                }

                function maskValue(value) {
                    var validNumericChars = '[^0-9' + settings.decimal + ']';
                    var charsRegex = new RegExp(validNumericChars, 'g');
                    var negative = (value.indexOf("-") > -1 && settings.allowNegative) ? "-" : "";
                    var onlyNumbers = value.replace(charsRegex, "");
                    var pointPosition = onlyNumbers.indexOf(settings.decimal);
                    var newValue, integerPart, decimalPart, pointPosition2, precision, zeros = '';

                    if (pointPosition >= 0) {
                        pointPosition2 = onlyNumbers.indexOf(settings.decimal, pointPosition + 1);
                        if (pointPosition2 >= 0)
                            onlyNumbers = onlyNumbers.substr(0, pointPosition2);
                    }
                    else {
                        onlyNumbers += settings.decimal;
                        pointPosition = onlyNumbers.indexOf(settings.decimal);
                    }
                    integerPart = String(parseInt(onlyNumbers));
                    if (isNaN(integerPart)) {
                        integerPart = "0";
                    }
                    // put settings.thousands every 3 chars
                    if (settings.thousands != '')
                        integerPart = integerPart.replace(/\B(?=(\d{3})+(?!\d))/g, settings.thousands);
                    if (integerPart === "") {
                        integerPart = "0";
                    }
                    newValue = negative + integerPart;

                    precision = parseInt(settings.precision);
                    if (precision > 0) {
                        decimalPart = onlyNumbers.slice(pointPosition, pointPosition + precision + 1);
                        if (precision + 2 > decimalPart.length)
                            zeros = new Array(precision + 2 - decimalPart.length).join(0);
                        newValue += decimalPart + zeros;
                    }
                    return setSymbol(newValue);
                }

                function preventDefault(e) {
                    if (e.preventDefault) { //standard browsers
                        e.preventDefault();
                    } else { // old internet explorer
                        e.returnValue = false;
                    }
                }

                function keypressEvent(e) {
                    e = e || window.event;
                    var key = e.which || e.charCode || e.keyCode, decimalPart;
                    //added to handle an IE "special" event
                    if (key === undefined) {
                        return false;
                    }

                    // any key except the numbers 0-9
                    if (key < 48 || key > 57) {
                        // -(minus) key
                        if (key === 45) {
                            //$input.val(changeSign());
                            return settings.allowNegative && $input.val() == '';
                            // decimal key
                        } else if (settings.decimal.length > 0 && key === settings.decimal.charCodeAt(0)) {
                            //$input.val($input.val().replace("-", ""));
                            return settings.precision > 0 && $input.val().indexOf(settings.decimal) == -1;
                            // enter key or tab key
                        } else if (key === 13 || key === 9) {
                            return true;
                        } else if ($.browser.mozilla && (key === 37 || key === 39) && e.charCode === 0) {
                            // needed for left arrow key or right arrow key with firefox
                            // the charCode part is to avoid allowing "%"(e.charCode 0, e.keyCode 37)
                            return true;
                        } else { // any other key with keycode less than 48 and greater than 57
                            preventDefault(e);
                            return true;
                        }
                    }
                    //Limit the digits after the decimal character no more than settings.precision
                    if (settings.precision > 0 && $input.val().indexOf(settings.decimal) > -1) {
                        decimalPart = $input.val().substr($input.val().indexOf(settings.decimal) + 1);

                        var decimalIndex = $input.val().indexOf(settings.decimal);
                        var sel = document.selection.createRange();
                        var selLen = document.selection.createRange().text.length;
                        sel.moveStart('character', -$input.val().length);
                        var currentIndex = sel.text.length - selLen;
                        if (currentIndex <= decimalIndex) {
                            return true;
                        } else {
                            return decimalPart.length < settings.precision;
                        }
                    }
                }

                function blurEvent(e) {
                    $input.val(maskValue($input.val()));
                }

                $input.unbind(".numericValidation");
                $input.bind("keypress.numericValidation", keypressEvent);
                $input.bind("blur.numericValidation", blurEvent);
            });
        }
    };

    $.fn.numericValidation = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === "object" || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error("Method " + method + " does not exist on Format.numericValidation");
        }
    };
})(window.jQuery || window.Zepto);

(function ($) {
    if (!$.browser) {
        $.browser = {};
        $.browser.mozilla = /mozilla/.test(navigator.userAgent.toLowerCase()) && !/webkit/.test(navigator.userAgent.toLowerCase());
        $.browser.webkit = /webkit/.test(navigator.userAgent.toLowerCase());
        $.browser.opera = /opera/.test(navigator.userAgent.toLowerCase());
        $.browser.msie = /msie/.test(navigator.userAgent.toLowerCase());
    }

    var methods = {
        destroy: function () {
            $(this).unbind(".alphanumericValidation");

            if ($.browser.msie) {
                this.onpaste = null;
            }
            return this;
        },

        init: function (parameters) {
            parameters = $.extend({   // Validation of valid and invalid characters is made using regular expressions.
                validCharacters: '[0-9a-zA-Z]', // If you want to accept a limited chatacter list. If \ character is used, it must be double, e.g. \\w  stands for: \w
                invalidCharacters: '', // If you want accept any character except the ones here.
                switchToLower: false,
                switchToUpper: false,
                trim: true
            }, parameters);

            return this.each(function () {
                var $input = $(this), settings,
                    onFocusValue;

                // data-* api
                settings = $.extend({}, parameters);
                settings = $.extend(settings, $input.data());

                function negativeRegex(charRange){
                    var indexBracket = charRange.indexOf('[');
                    var indexCaret = charRange.indexOf('^');
                    if (indexCaret >= 0)
                        return charRange.slice(0, indexCaret) + charRange.slice(indexCaret + 1);
                    if (indexBracket >= 0)
                        return charRange.slice(0, indexBracket + 1) + '^' + charRange.slice(indexBracket + 1);
                    return charRange;
                }

                function maskValue(value) {
                    var validCharsRegex, invalidCharsRegex, onlyValidOnes = value;
                    if (settings.validCharacters != ''){
                        validCharsRegex = new RegExp(negativeRegex(settings.validCharacters), 'g');
                        onlyValidOnes = onlyValidOnes.replace(validCharsRegex, '');
                    }
                    if (settings.invalidCharacters != ''){
                        invalidCharsRegex = new RegExp(settings.invalidCharacters, 'g');
                        onlyValidOnes = onlyValidOnes.replace(invalidCharsRegex, '');
                    }
                    if (settings.trim)
                        onlyValidOnes = onlyValidOnes.trim();
                    if (settings.switchToUpper)
                        return onlyValidOnes.toUpperCase();
                    if (settings.switchToLower)
                        return onlyValidOnes.toLowerCase();
                    return onlyValidOnes;
                }

                function preventDefault(e) {
                    if (e.preventDefault) { //standard browsers
                        e.preventDefault();
                    } else { // old internet explorer
                        e.returnValue = false;
                    }
                }

                function keypressEvent(e) {
                    e = e || window.event;
                    var key = e.which || e.charCode || e.keyCode, keyChar, validCharsRegex, invalidCharsRegex, valid = true;
                    //added to handle an IE "special" event
                    if (key === undefined) {
                        return false;
                    }
                    keyChar = String.fromCharCode(key);
                    if (settings.validCharacters != ''){
                        validCharsRegex = new RegExp(settings.validCharacters, 'g');
                        valid = valid && validCharsRegex.test(keyChar);
                    }
                    if (settings.invalidCharacters != ''){
                        invalidCharsRegex = new RegExp(settings.invalidCharacters, 'g');
                        valid = valid && !invalidCharsRegex.test(keyChar);
                    }
                    return valid;
                }

                function blurEvent(e) {
                    $input.val(maskValue($input.val()));
                }

                $input.unbind(".alphanumericValidation");
                $input.bind("keypress.alphanumericValidation", keypressEvent);
                $input.bind("blur.alphanumericValidation", blurEvent);
            });
        }
    };

    $.fn.alphanumericValidation = function (method) {
        if (methods[method]) {
            return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
        } else if (typeof method === "object" || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error("Method " + method + " does not exist on Format.alphanumericValidation");
        }
    };
})(window.jQuery || window.Zepto);

//Define new format in a function in order to call it again, when new textboxes are created using javascript
function defineFormats() {
    $('.numericDecimal0').numericValidation({ allowNegative: false, thousands: '', precision: 0 });
    $('.numericDecimal2').numericValidation({ allowNegative: false, thousands: '', precision: 2 });
    $('.numericDecimal3').numericValidation({ allowNegative: false, thousands: '', precision: 3 });
    $('.numericCommaDecimal0').numericValidation({ allowNegative: false, thousands: ',', decimal: '.', precision: 0 });
    $('.numericCommaDecimal2').numericValidation({ allowNegative: false, thousands: ',', decimal: '.', precision: 2 });
    $('.alphanumericToUpper1').alphanumericValidation({ validCharacters: '[0-9a-zA-Z ]', switchToUpper: true });
    $('.alphanumericToUpper2').alphanumericValidation({ validCharacters: '[0-9a-zA-Z]', switchToUpper: true });
    $('.alphanumericNumeric').alphanumericValidation({ validCharacters: '[0-9]', switchToUpper: true });
    $(".decimaloneplace").numericValidation({ allowNegative: false, thousands: '', precision: 1 });
    $(".decimaltwoplaces").numericValidation({ allowNegative: false, thousands: '', precision: 2 });
    $(".decimalfourplaces").numericValidation({ allowNegative: false, thousands: '', precision: 4 });
    $(".numericNoMask").numericValidation({ allowNegative: false, thousands: '', precision: 0 });
    $('.currencyMask').numericValidation({ allowNegative: false, thousands: '', precision: 2 });
    $('.currencyNoDecimal').numericValidation({ allowNegative: false, thousands: ',', precision: 2 });
    $('.currencyNoDecimalAllowZero').numericValidation({ allowNegative: false, thousands: ',', precision: 2 });
    $('.decimal').numericValidation({ allowNegative: false, thousands: '', precision: 2 });
}
defineFormats();