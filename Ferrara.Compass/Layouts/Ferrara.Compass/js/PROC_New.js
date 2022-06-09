var requiredDiv = $("<span class='markrequired'>*</span>");
function BOMNewCondition() {
    pageLoadCheck();
    if ($("#drpNew option:selected").text() == 'New') {
        $("#ddlPrinter").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        $("#ddlPrinter").addClass("requireduc");
    } else {
        $("#ddlPrinter").closest(".form-group").find(".markrequired").remove();
        $("#ddlPrinter").removeClass("requireduc");
    }
    GraphicsOnlyProc();
}
function NewPackagingItem() {
    var componentType = $("#drpPkgComponent option:selected").text();
    $("#hdnComponentType").val(componentType);
    $("#ddlFilmPrintStyle").val("-1");
    pageLoadCheck();
    GraphicsOnlyProc();
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

    $("#ddlNewPrinterSupplierForLocation").removeClass("requireduc");
    $("#ddlNewPrinterSupplierForLocation").closest(".form-group").find(".markrequired").remove();
    $(".NewPrinterSupplierForLocation").addClass("hideItem");

    var v = $("#drpPkgComponent option:selected").text();

    if (v != null) {
        $("#lnkMain").click();
        var o = v.split('-');
        v = o[0];
        $(".requireduc").closest(".form-group").find(".markrequired").remove();
        $(".requireduc").removeClass("requireduc");

        $(".row.transferSemi, .row.candySemi, .row.purchasedCandy, .row.semiDetails").addClass("hideItem");
        $(".attachment, .printer, .ipf").removeClass("hideItem");
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
        $("#ddlPurchasedIntoLocation").prop("disabled", true);
        if (v.indexOf('Candy') != -1 && v.indexOf('Purchased Candy') == -1) {
            $(".candySemi").removeClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();

            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
        } else if (v.indexOf('Finished') != -1) {
            $(".printer, .attachment").addClass("hideItem");
            $(".printer, .attachment").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .attachment").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
        } else if (v.indexOf('Transfer') != -1) {
            $(".row.transferSemi, .ipf.TSOnlyRow").removeClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").addClass("hideItem");
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $("#txtNotes").closest(".row").parent().hide();
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();
            enableDisableCountry();
        } else if (v.indexOf('Purchased Candy') != -1) {
            $(".row.purchasedCandy, .ipf.TSOnlyRow").removeClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").addClass("hideItem");
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $("#txtNotes").closest(".row").parent().hide();
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();
            enableDisableCountry();
        } else if (v.indexOf('Corrugated') != -1) {
            if ($("#drpNew option:selected").text() == 'Existing' && $('#hdnProjectTypeSubCategory').val() == "Complex Network Move") {
                $("#ddlNewPrinterSupplierForLocation").addClass("requireduc");
                $("#ddlNewPrinterSupplierForLocation").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                $(".NewPrinterSupplierForLocation").removeClass("hideItem");
            }
        }
        //BOMNewExistingDrpChange(false, $("#drpNew option:selected").text().trim());
        var lowerVal = $("#drpNew option:selected").text().trim().toLocaleLowerCase();
        if (lowerVal == "new") {
            $("#ddlPrinter").addClass("requireduc");
            $("#ddlPrinter").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        } else {
            $("#ddlPrinter").removeClass("requireduc");
            $("#ddlPrinter").closest(".form-group").find(".markrequired").remove();
        }
        //$("#ddlFlowthrough").removeClass("requireduc");
        //$("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
        //$("#ddlFlowthrough").prop("disabled", true);
        GraphicsCheckLoad("drpGraphicsNeeded");
        IPFFieldCheck();
        SetPrinterAndSupplierVisibility();
    }
    TSBarcodeGenerationVisibility();
    LoadPrintStyleTypes();
}