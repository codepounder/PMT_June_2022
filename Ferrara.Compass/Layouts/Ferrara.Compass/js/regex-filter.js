// vvv Allows the use of regex to search with vvv
jQuery.expr[':'].regex = function(elem, index, match) {
    var matchParams = match[3].split(','),
        validLabels = /^(data|css):/,
        attr = {
            method: matchParams[0].match(validLabels) ? 
                        matchParams[0].split(':')[0] : 'attr',
            property: matchParams.shift().replace(validLabels,'')
        },
        regexFlags = 'ig',
        regex = new RegExp(matchParams.join('').replace(/^\s+|\s+$/g,''), regexFlags);
    return regex.test(jQuery(elem)[attr.method](attr.property));
}
// ^^^ Allows the use of regex to search with ^^^

//main JS 
$(document).ready(function() {
    fieldsInitial = $("input:regex(class, field[1-3])")
    fieldsPart1 = $("input:regex(class,field4)")


    checkAndUnHide(fieldsInitial,"#part_1")
    checkAndUnHide(fieldsPart1,"#part_2")

})

// vvv my js code, place this in separate file to use the functions vvv
//Helper Function
function checkAndUnHide ( checkThis,unHideThis) {
    checkThis.keyup(unHideThis, function(event){ 
        unHide(checkThis,event.data)
    }) 
}

//checks if the .val() of each element is blank or not
function allElementsFilled(arrayOfDomElements) {
return !(arrayOfDomElements.map(function () {
         return $(this).val()})
         .get()
         .some(function (element) { return element == ""})
        )
}

//will unhide element if check's .val() is not blank
function unHide(check, element) {
    if(typeof check !== 'string'){
        if (allElementsFilled(check)) {
            $(element).css("visibility", "visible")            
        }
    }
    else{
        if(check.val != ""){
            $(element).css("visibility", "visible")
        }
    }
}