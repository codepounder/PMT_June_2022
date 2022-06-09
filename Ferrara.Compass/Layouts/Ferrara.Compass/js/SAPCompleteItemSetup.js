var requiredDiv = $("<span class='markrequired'>*</span>");
function BOMNewCondition() {
    GraphicsCheckLoad("drpGraphicsNeeded");
    IPFFieldCheck();
    if ($("#drpNew option:selected").text() == 'New' && !$("#ddlPrinter").hasClass("requireduc")) {
        $("#ddlPrinter").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        $("#ddlPrinter").addClass("requireduc");
    }
    else if ($("#drpNew option:selected").text() == 'Existing') {
        $("#ddlPrinter").closest(".form-group").find(".markrequired").remove();
        $("#ddlPrinter").removeClass("requireduc");
    }
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

        $(".row.transferSemi, .row.candySemi, .row.purchasedCandy").addClass("hideItem");
        $(".printer, .dimensions, .attachment, .ipf").removeClass("hideItem");
        $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Structure:");
        $("#drpFilmBackSeam, #txtFilmBagFace, #ddlFilmSubstrate, #ddlStructureColor, #ddlFilmStyle, #ddlFilmPrintStyle").parent().parent().removeClass("hideItem");
        $("#txtNewSubstrate, #ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD, #ddlFilmPrintStyle").closest(".row").removeClass("hideItem");
        $("#drpNew").prop("disabled", false);
        $("#txtNotes").closest(".row").parent().show();

        $(".ipf :input:not([type=hidden]):not([type=submit]):not([type=button])").each(function () {
            if (!$(this).hasClass("requireduc")) {
                $(this).addClass("requireduc");
                $(this).closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $("#ddlFlowthrough").removeClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
            $("#ddlFlowthrough").prop("disabled", true);
        });

        $(".ipf.TSOnlyRow").addClass("hideItem");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();

        if (v.indexOf('Candy') != -1 && v.indexOf('Purchased Candy') == -1) {
            $(".candySemi").removeClass("hideItem");
            $(".candySemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".candySemi").find(".form-group").prepend($("<span class='markrequired'>*</span>"));

            $(".printer, .dimensions, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".printer, .dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
        } else if (v.indexOf('Corrugated') != -1) {
            $("#txtNewSubstrate, #ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD").closest(".row").addClass("hideItem");
            $("#drpFilmBackSeam, #ddlFilmStyle").parent().parent().addClass("hideItem");
        } else if (v.indexOf('Film') != -1) {
            $("#ddlStructureColor").parent().parent().addClass("hideItem");
            $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Film Structure:");
        } else if (v.indexOf('Finished') != -1) {
            $(".printer, .dimensions, .attachment").addClass("hideItem");
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
            $("#ddlFilmSubstrate").parent().parent().addClass("hideItem");
        } else if (v.indexOf('Transfer') != -1) {
            $(".row.transferSemi, .ipf.TSOnlyRow").removeClass("hideItem");
            $(".printer, .dimensions, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            enableDisableCountry();
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
            $(".printer, .dimensions, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            enableDisableCountry();
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

        if ($("#drpNew option:selected").text().trim().toLowerCase() == 'new' && !$("#ddlPrinter").hasClass("requireduc")) {
            $("#ddlPrinter").addClass("requireduc");
            $("#ddlPrinter").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        } else if ($("#drpNew option:selected").text() == 'Existing') {
            $("#ddlPrinter").removeClass("requireduc");
            $("#ddlPrinter").closest(".form-group").find(".markrequired").remove();
        }
        $("#ddlFlowthrough").removeClass("requireduc");
        $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
        //$("#ddlFlowthrough").prop("disabled", true);
        GraphicsCheckLoad("drpGraphicsNeeded");
        IPFFieldCheck();
    }
    if ($("#lblCaseUCC").val() == "") {
        $("#lblCaseUCC").parent().parent().hide();
    }
    if ($("#lblPalletUCC").val() == "") {
        $("#lblPalletUCC").parent().parent().hide();
    }
    $(".OBMSetup").addClass("hideItem");
    $('.hidebtn').addClass("hideItem");
    if ($("#txtMaterial").val() == '') {
        $("#txtMaterial").val("Needs New");
        $("#txtMaterialDescription").val("Needs New");
    }
    else if ($("#txtLikeItem").val() == '') {
        $("#txtLikeItem").val("Not Applicable");
        $("#txtLikeDescription").val("Not Applicable");
    }
    SetPrinterAndSupplierVisibility();
    TSBarcodeGenerationVisibility();
    LoadPrintStyleTypes();
}
function OnPackTrialChange() {
}
function validatePage() {
    var isChecked = true;
    var isValid = false;
    var idchk = 'dvTop';
    var ed = 'Please verify all steps have been completed';
    $('.bomTask input').each(function (idx, chk) {
        if (!chk.checked)
            isChecked = false;
    });
    isValid = ValidateData();
    if (!isChecked) {
        $("#dverror_message").show();
        $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + ed + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + idchk + '"' + ') >Update</a>   </li></br>');
    }

    if (isValid && isChecked) {
        return true;
    }
    if (!isValid || !isChecked) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
        setFocusError();
    }
    return false;
}
function NewPackagingItem() {
    var componentType = $("#drpPkgComponent option:selected").text();
    $("#hdnComponentType").val(componentType);
    $("#ddlFilmPrintStyle").val("-1");
    pageLoadCheck();
}