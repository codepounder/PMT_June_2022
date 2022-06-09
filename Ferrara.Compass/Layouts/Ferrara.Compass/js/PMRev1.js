var requiredDiv = $("<span class='markrequired'>*</span>");
function BOMNewCondition() {
    pageLoadCheck();
}
function pageLoadCheck() {
    $(".panel").each(function () {
        var ischild = $(this).find("#hdnParentPackagingType").val();
        if (ischild != null) {
            if (ischild.toLocaleLowerCase() == "true") {
                var panel = $(this);
                panel.addClass("childItem");
                var accordion = panel.prev();
                if (accordion.hasClass("accordion")) {
                    accordion.addClass("childitem");
                }
                var packMeas = panel.next();
                if (packMeas.hasClass("dvPackNext")) {
                    packMeas.addClass("childitem");
                }
            }
        }
    });
    var v = $("#drpPkgComponent option:selected").text();

    if (v != null) {
        $("#lnkMain").click();
        var o = v.split('-');
        v = o[0];

        $(".requireduc").closest(".form-group").find(".markrequired").remove();
        $(".requireduc").removeClass("requireduc");

        $(".row.transferSemi, .row.candySemi, .row.printer, .row.purchasedCandy").addClass("hideItem");
        $(".dimensions, .attachment, .ipf").removeClass("hideItem");
        $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Structure:");
        $("#drpFilmBackSeam, #txtFilmBagFace, #ddlFilmSubstrate, #ddlStructureColor, #ddlFilmPrintStyle").parent().parent().removeClass("hideItem");
        $("#txtNewSubstrate, #ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD, #ddlFilmPrintStyle").closest(".row").removeClass("hideItem");
        $("#ddlFilmStyle").parent().parent().addClass("hideItem");
        $("#drpNew").prop("disabled", false);
        $("#txtNotes").closest(".row").parent().show();

        $(".ipf :input:not([type=hidden]):not([type=submit]):not([type=button])").each(function () {
            if (!$(this).hasClass("requireduc")) {
                $(this).addClass("requireduc");
                $(this).closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
        });
        $(".ipf.TSOnlyRow").addClass("hideItem");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();

        $(".SpecificationNo, .PurchasedCandyLocation").addClass("hideItem");

        if (v.indexOf('Candy') != -1 && v.indexOf('Purchased Candy') == -1) {
            $(".candySemi").removeClass("hideItem");
            $(".dimensions, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
        } else if (v.indexOf('Corrugated') != -1) {
            $("#txtNewSubstrate, #ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD").closest(".row").addClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
        } else if (v.indexOf('Film') != -1) {
            $("#ddlStructureColor").parent().parent().addClass("hideItem");
            $("#ddlFilmStyle").parent().parent().removeClass("hideItem");
            $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Film Structure:");
        } else if (v.indexOf('Finished') != -1) {
            $(".dimensions, .attachment").addClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
        } else if (v.indexOf('Label') != -1) {
            $("#ddlFilmPrintStyle, #txtFilmWebWidth").closest(".row").addClass("hideItem");
            $("#drpFilmBackSeam, #txtFilmBagFace, #ddlFilmSubstrate").parent().parent().addClass("hideItem");
            $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Label Structure:");
        } else if (v.indexOf('Other') != -1 || v.indexOf('Paperboard') != -1) {
            $(".dimensions").addClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
        } else if (v.indexOf('Rigid') != -1) {
            $("#ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD, #ddlFilmPrintStyle").closest(".row").addClass("hideItem");
            $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Rigid Plastic Structure:");
            $("#ddlStructureColor").closest(".form-group").find(".control-label").text("Rigid Plastic Structure Color:");
            $("#ddlFilmSubstrate").parent().parent().addClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
        } else if (v.indexOf('Transfer') != -1) {
            $(".row.transferSemi, .ipf.TSOnlyRow").removeClass("hideItem");
            $(".dimensions, .attachment, .hideableRow").addClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $(".ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $("#txtNotes").closest(".row").parent().hide();
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();
        } else if (v.indexOf('Purchased Candy') != -1) {
            $(".row.purchasedCandy, .ipf.TSOnlyRow").removeClass("hideItem");
            $(".dimensions, .attachment, .hideableRow").addClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

            $(".ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $("#txtNotes").closest(".row").parent().hide();
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();

        }
        /*if ($("#drpNew option:selected").text() == 'New' && $("#hdnTBDIndicator").val() == "No") {
            $("#ddlFlowthrough").addClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        } else if ($("#drpNew option:selected").text() == 'New' && $("#hdnTBDIndicator").val() == "Yes") {
            $("#ddlFlowthrough").addClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        } else if ($("#drpNew option:selected").text() == 'Existing' && $("#hdnTBDIndicator").val() == "No") {
            $("#ddlFlowthrough").removeClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
            $("#ddlFlowthrough").prop("disabled", true);
        } else if ($("#drpNew option:selected").text() == 'Existing' && $("#hdnTBDIndicator").val() == "Yes") {
            $("#ddlFlowthrough").removeClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
            $("#ddlFlowthrough").prop("disabled", true);
        }*/
        GraphicsCheckLoad("drpGraphicsNeeded");
        IPFFieldCheck();
        LoadPrintStyleTypes();
    }
}
function NewPackagingItem() {
    var componentType = $("#drpPkgComponent option:selected").text();
    $("#hdnComponentType").val(componentType);
    $("#ddlFilmPrintStyle").val("-1");
    pageLoadCheck();
}
function conditionalFirstRevChecks() {
    var customer = $("#lblCustomer").val();
    if ((customer.length == 0) || (customer == "Select...")) {
        $("#dvCustomer").hide();
    }

    if ($("#ddlOBMFirstReviewConfirmation option:selected").text().trim() == 'No') {
        $("#dvOBMFirstReviewComments").show();
        $("#dvSectionsOfConcern").show();
    }
    else {
        $("#dvOBMFirstReviewComments").hide();
        $("#dvSectionsOfConcern").hide();
    }

    if ($("#ddlRevisedFirstShipDate option:selected").text().trim() == 'Yes') {
        $("#dvRevisedFirstShipDate").show();
        $("#dvFirstShipRevisionComments").show();
    }
    else {
        $("#dvRevisedFirstShipDate").hide();
        $("#dvFirstShipRevisionComments").hide();
    }

    var lineBusiness = $("#lblLineOfBusiness").val();
    if (lineBusiness == "Seasonal (000000023)") {
        $("#dvFirstProductionDate").show();
    }
    else {
        $("#dvFirstProductionDate").hide();
    }
    if ($("#ddlOBMFirstReviewConfirmation option:selected").text().toLowerCase() != 'no') {
        var list = $('#cblSectionConcerns input');
        for (i = 0; i < list.length; i++) {
            list[i].checked = false;
        }
    }
    conditionalChecks();
}
function validatePage() {
    var isChecked = false;
    var isValid = false;
    var isBOMValid = true;
    var idchk = 'cblSectionConcerns';
    var ed = 'Please check sections do you have concerns on?';
    var list = $('#cblSectionConcerns input');
    for (i = 0; i < list.length; i++) {
        if (list[i].checked) {
            isChecked = true;
        }
    }
    isValid = ValidateData();

    $.each(BOMGridTableData, function (key, comp) {
        if (comp.CompStatus == "Red") {
            isBOMValid = false;

            $("#dverror_message").show();
            $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + "Please complete component information for - " + comp.MaterialNumber + ": " + comp.MaterialDescription + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + 'BOMGridTable' + '"' + ') >Update</a>   </li></br>');
        }
    });

    if ($("#ddlOBMFirstReviewConfirmation option:selected").text().toLowerCase() == 'no') {
        if (!isChecked) {
            $("#dverror_message").show();
            $('#error_message').append('<li style="padding-left:20px;flaot:left; display: inline;color:red">' + ed + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + idchk + '"' + ') >Update</a>   </li></br>');
        }
    }
    else { isChecked = true; }
    if (isValid && isChecked && isBOMValid) {
        return true;
    }
    return false;
}
$("form").submit(function () {
    $("#drpNew").prop("disabled", false);
    $("#ddlFlowthrough").prop("disabled", false);
    $("#ddlPurchasedIntoLocation").prop("disabled", false);
    $("#ddlReviewPrinterSupplier").prop("disabled", false);
    $("#ddlFilmPrintStyle").prop("disabled", false);
    $("#ddlSAPDescAbrev").prop("disabled", false);
    $("#drpTSPackLocation").prop("disabled", false);
    $("#drpPCSPackLocation").prop("disabled", false);
    $("#drpPCSCountryofOrigin").prop("disabled", false);
    $("#drpPurchasedCandyLocation").prop("disabled", false);
    $("#drpTrialsCompleted").prop("disabled", false);
    $("#ddlNewFormula").prop("disabled", false);
    $("#ddlPrinter").prop("disabled", false);
    $("#ddlPrinter").prop("disabled", false);

});