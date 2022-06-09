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

    $("#ddlNewPrinterSupplierForLocation").prop("disabled", true);
    $(".NewPrinterSupplierForLocation").addClass("hideItem");

    var v = $("#drpPkgComponent option:selected").text();
    var projectType = $("#hdnProjectType").val();

    if (v != null && v != "") {
        $("#lnkMain").click();
        var o = v.split('-');
        v = o[0];

        $("#dvMain").find(".markrequired").remove();

        $("#dvMain :input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
        $("#dvMain :input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

        $(".row.transferSemi, .row.candySemi, .row.purchasedCandy, .row.semiDetails").addClass("hideItem");
        $(".transferSemi, .candySemi, .purchasedCandy, .row.semiDetails").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        $(".transferSemi, .candySemi, .purchasedCandy, .row.semiDetails").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();

        $(".printer, .attachment, .ipf").removeClass("hideItem");
        $("#txtOldMaterial, #ddlFilmPrintStyle, #ddlPurchasedIntoLocation, #txtSpecificationNo").removeClass("requireduc");
        $("#txtOldMaterial, #ddlFilmPrintStyle, #ddlPurchasedIntoLocation, #txtSpecificationNo").closest(".form-group").find(".markrequired").remove();
        $("#ddlPurchasedIntoLocation").prop("disabled", true);
        $("#drpNew").prop("disabled", false);

        $(".ipf.TSOnlyRow").addClass("hideItem");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();

        enableDisableCountry();
        if (v.indexOf('Candy') != -1 && v.indexOf('Purchased Candy') == -1) {
            $(".candySemi").removeClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove()
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
        } else if (v.indexOf('Finished Good') != -1) {
            $(".printer, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove()
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
        } else if ((v.indexOf('Film') != -1 || v.indexOf('Corrugated') != -1)) {
            var lowerVal = $("#drpNew option:selected").text().trim().toLocaleLowerCase();

            if (lowerVal == "new") {
                $("#txtSpecificationNo").addClass("requireduc");
                $("#txtSpecificationNo").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            } else {
                $("#txtDielineLinkEdit").removeClass("requireduc");
                $("#txtDielineLinkEdit").closest(".form-group").find(".markrequired").remove();
            }

            if ($("#drpGraphicsNeeded option:selected").text() == "Yes") {
                $("#ddlFilmPrintStyle").addClass("requireduc");
                $("#ddlFilmPrintStyle").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            if (lowerVal == "new" && projectType != "Graphics Change Only") {
                $("#ddlPrinter").addClass("requireduc");
                $("#ddlPrinter").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            } else {
                $("#ddlPrinter").removeClass("requireduc");
                $("#ddlPrinter").closest(".form-group").find(".markrequired").remove();
            }

            if ((v.indexOf('Corrugated') != -1) && $("#drpNew option:selected").text() == 'Existing' && $('#hdnProjectTypeSubCategory').val() == "Complex Network Move") {
                $(".NewPrinterSupplierForLocation").removeClass("hideItem");
            }

        } else if (v.indexOf('Transfer') != -1) {
            $(".row.transferSemi, .ipf.TSOnlyRow").removeClass("hideItem");
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $(".printer, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".printer, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();
            enableDisableCountry();
        } else if (v.indexOf('Purchased Candy') != -1) {
            $(".row.purchasedCandy, .ipf.TSOnlyRow").removeClass("hideItem");
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

            $(".printer, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();
            enableDisableCountry();
        } else {
            var lowerVal = $("#drpNew option:selected").text().trim().toLocaleLowerCase();

            if (lowerVal == "new") {
                $("#txtSpecificationNo").addClass("requireduc");
                $("#txtSpecificationNo").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            } else {
                $("#txtDielineLinkEdit").removeClass("requireduc");
                $("#txtDielineLinkEdit").closest(".form-group").find(".markrequired").remove();
            }

            if (lowerVal == "new" && projectType != "Graphics Change Only") {
                $("#ddlPrinter").addClass("requireduc");
                $("#ddlPrinter").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            } else {
                $("#ddlPrinter").removeClass("requireduc");
                $("#ddlPrinter").closest(".form-group").find(".markrequired").remove();
            }
        }

        $(".ipf :input:not([type=hidden]):not([type=submit]):not([type=button])").each(function () {
            if (!$(this).hasClass("requireduc") && !$(this).closest(".row").hasClass("hideItem")) {
                $(this).addClass("requireduc");
                $(this).closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $("#ddlFlowthrough").removeClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
        });
        GraphicsCheckSpec("drpGraphicsNeeded");
        IPFFieldCheck();
    } else {

        $(".pe2").addClass("requiredpm");
        $(".pe2").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        OnPackTrialChange();
        $(".makePackLocation").removeClass("requiredpm");
        $(".makePackLocation").closest(".form-group").find(".markrequired").remove();
        $(".txtFGPackSpecNumber").removeClass(".requiredpm");
        $(".txtFGPackSpecNumber").closest(".form-group").find(".markrequired").remove();
        $(".drpNewExistingSpec").each(function () {
            var el = $(this);
            var section = el.closest(".dvPackNext");
            var specNotes = section.find("#txtSpecNotes");
            if (el.find("option:selected").text() == "Yes") {
                specNotes.addClass("requiredpm");
                specNotes.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            } else {
                specNotes.removeClass("requiredpm");
                specNotes.closest(".form-group").find(".markrequired").remove();
            }
        });
    }
    SetPrinterAndSupplierVisibility();
    TSBarcodeGenerationVisibility();
    LoadPrintStyleTypes();
}
function OnPackTrialChange() {
    $("#hdnPackTrial").val($("#ddlPackTrial option:selected").text());
    if ($("#ddlPackTrial option:selected").text() == 'Yes' && !$("#ddlResultPackTrial").hasClass("requiredpm")) {
        $(".packTrial").addClass("requiredpm");
        $(".packTrial").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
    } else {
        $(".packTrial").removeClass("requiredpm");
        $(".packTrial").closest(".form-group").find(".markrequired").remove();
    }
    if ($("#ddlPackTrial option:selected").text() == 'Yes') {
        $(".packTrialAttachment").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
    } else {
        $(".packTrialAttachment").closest(".form-group").find(".markrequired").remove();
    }
}
function NewPackagingItem() {
    var componentType = $("#drpPkgComponent option:selected").text();
    $("#hdnComponentType").val(componentType);
    $("#ddlFilmPrintStyle").val("-1");
    pageLoadCheck();
}
function SpecsChange(drp) {
    var el = $(drp);
    var section = el.closest(".dvPackNext");
    var specNotes = section.find("#txtSpecNotes");
    if (el.find("option:selected").text() == "Yes") {
        specNotes.addClass("requiredpm");
        specNotes.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
    } else {
        specNotes.removeClass("requiredpm");
        specNotes.closest(".form-group").find(".markrequired").remove();
    }
}

