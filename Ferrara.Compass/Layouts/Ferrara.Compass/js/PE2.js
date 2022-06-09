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
    if (v != null && v != "") {
        $("#lnkMain").click();
        var o = v.split('-');
        v = o[0];

        $("#dvMain").find(".markrequired").remove();

        $("#dvMain :input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
        $("#dvMain :input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

        $(".row.transferSemi, .row.candySemi, .row.purchasedCandy").addClass("hideItem");
        $(".transferSemi, .candySemi, .purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        $(".transferSemi, .candySemi, .purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();

        $(".printer, .dimensions, .attachment, .ipf").removeClass("hideItem");
        $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Structure:");
        $("#ddlStructureColor").closest(".form-group").find(".control-label").text("Structure Color:");

        $("#drpFilmBackSeam, #txtFilmBagFace, #ddlFilmSubstrate, #ddlStructureColor, #ddlFilmPrintStyle").parent().parent().removeClass("hideItem");
        $("#txtNewSubstrate, #ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD, #ddlFilmPrintStyle").closest(".row").removeClass("hideItem");
        $("#ddlFilmStyle").parent().parent().addClass("hideItem");
        $("#ddlFilmStyle").parent().parent().find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        $("#ddlFilmStyle").parent().parent().find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
        $("#txtOldMaterial, #txtNotes, #txtNewSubstrate, #txtTareWt").removeClass("requireduc");
        $("#txtOldMaterial, #txtNotes, #txtNewSubstrate, #txtTareWt").closest(".form-group").find(".markrequired").remove();
        $("#drpNew").prop("disabled", false);
        $("#txtNotes").closest(".row").parent().show();

        $(".ipf.TSOnlyRow").addClass("hideItem");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        $(".ipf.TSOnlyRow, .printer").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();

        enableDisableCountry();
        if (v.indexOf('Candy') != -1 && v.indexOf('Purchased Candy') == -1) {
            $(".candySemi").removeClass("hideItem");
            $(".printer, .dimensions, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove()
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
        }
        else if (v.indexOf('Corrugated') != -1) {
            $("#txtNewSubstrate, #ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD").closest(".row").addClass("hideItem");
            $("#ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD").closest(".row").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $("#ddlFilmSubstrate, #ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD").closest(".row").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            $("#drpFilmBackSeam").removeClass("requireduc");
            $("#drpFilmBackSeam").closest(".form-group").find(".markrequired").remove();
        }
        else if (v.indexOf('Film') != -1) {
            $("#ddlFilmStyle").parent().parent().removeClass("hideItem");
            $("#ddlFilmStyle").addClass("requireduc");
            $("#ddlFilmStyle").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Film Structure:");
            $("#ddlStructureColor").parent().parent().addClass("hideItem");
            $("#ddlStructureColor").removeClass("requireduc");
            $("#ddlStructureColor").closest(".form-group").find(".markrequired").remove();
        }
        else if (v.indexOf('Finished') != -1) {
            $(".printer, .dimensions, .attachment").addClass("hideItem");
            $(".dimensions, .attachment").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".dimensions, .attachment").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            $("#drpFilmBackSeam").removeClass("requireduc");
            $("#drpFilmBackSeam").closest(".form-group").find(".markrequired").remove();
        }
        else if (v.indexOf('Label') != -1) {
            $("#ddlFilmPrintStyle, #txtFilmWebWidth").closest(".row").addClass("hideItem");
            $("#ddlFilmPrintStyle, #txtFilmWebWidth").closest(".row").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $("#ddlFilmPrintStyle, #txtFilmWebWidth").closest(".row").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove()
            $("#drpFilmBackSeam, #txtFilmBagFace, #ddlFilmSubstrate").parent().parent().addClass("hideItem");
            $("#drpFilmBackSeam, #txtFilmBagFace, #ddlFilmSubstrate").removeClass("requireduc");
            $("#drpFilmBackSeam, #txtFilmBagFace, #ddlFilmSubstrate").closest(".form-group").find(".markrequired").remove();
            $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Label Structure:");
            $("#ddlStructureColor").closest(".form-group").find(".control-label").text("Label Structure Color:");
        }
        else if (v.indexOf('Other') != -1 || v.indexOf('Paperboard') != -1) {
            $(".dimensions").addClass("hideItem");
            $(".dimensions").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".dimensions").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            $("#drpFilmBackSeam").removeClass("requireduc");
            $("#drpFilmBackSeam").closest(".form-group").find(".markrequired").remove();
        }
        else if (v.indexOf('Rigid') != -1) {
            $("#ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD, #ddlFilmPrintStyle").closest(".row").addClass("hideItem");
            $("#ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD, #ddlFilmPrintStyle").closest(".row").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $("#ddlFilmUnWind, #txtFilmWebWidth, #txtFilmMaxRollOD, #ddlFilmPrintStyle").closest(".row").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove()
            $("#ddlFilmStructure").closest(".form-group").find(".control-label").text("Rigid Plastic Structure:");
            $("#ddlStructureColor").closest(".form-group").find(".control-label").text("Rigid Plastic Structure Color:");
            $("#ddlFilmSubstrate, #drpFilmBackSeam").parent().parent().addClass("hideItem");
            $("#ddlFilmSubstrate, #drpFilmBackSeam").removeClass("requireduc");
            $("#ddlFilmSubstrate, #drpFilmBackSeam").closest(".form-group").find(".markrequired").remove();
        }
        else if (v.indexOf('Transfer') != -1) {
            $(".row.transferSemi, .ipf.TSOnlyRow").removeClass("hideItem");
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.transferSemi").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $(".dimensions, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            //$(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            //$(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            $("#drpFilmBackSeam").removeClass("requireduc");
            $("#drpFilmBackSeam").closest(".form-group").find(".markrequired").remove();
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            $("#txtNotes").closest(".row").parent().hide();
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();
            enableDisableCountry();
        }
        else if (v.indexOf('Purchased Candy') != -1) {
            $(".row.purchasedCandy, .ipf.TSOnlyRow").removeClass("hideItem");
            if ($("#drpNew option:selected").text() == 'New') {
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
                $(".row.purchasedCandy").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".ipf.TSOnlyRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

            $(".dimensions, .attachment, .ipf.hideableRow").addClass("hideItem");
            $(".dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".dimensions, .attachment, .ipf.hideableRow").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
            $(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").removeClass("hideItem");
            //$(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            //$(".dimensions:eq(0), .dimensions:eq(1), .dimensions:eq(2)").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            $("#drpFilmBackSeam").parent().parent().addClass("hideItem");
            $("#drpFilmBackSeam").removeClass("requireduc");
            $("#drpFilmBackSeam").closest(".form-group").find(".markrequired").remove();
            if ($("#drpNew option:selected").text() != 'Select...') {
                $("#drpNew").prop("disabled", true);
            }
            $("#txtNotes").closest(".row").parent().hide();
            $("#txtSEMIComment").removeClass("requireduc");
            $("#txtSEMIComment").closest(".form-group").find(".markrequired").remove();
            enableDisableCountry();
        }

        var lowerVal = $("#drpNew option:selected").text().trim().toLocaleLowerCase();
        if (lowerVal == "existing") {
            $(".requireduc").removeClass("requireduc");
            $(".markrequired").remove();
        }
        $(".ipf :input:not([type=hidden]):not([type=submit]):not([type=button])").each(function () {
            if (!$(this).hasClass("requireduc") && !$(this).closest(".row").hasClass("hideItem")) {
                $(this).addClass("requireduc");
                $(this).closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }
            $("#ddlFlowthrough").removeClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
        });
        /*if ($("#drpNew option:selected").text() == 'New' && $("#hdnTBDIndicator").val() == "No") {
            $("#ddlFlowthrough").removeClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
        } else if ($("#drpNew option:selected").text() == 'New' && $("#hdnTBDIndicator").val() == "Yes") {
            $("#ddlFlowthrough").removeClass("requireduc");
            $("#ddlFlowthrough").closest(".form-group").find(".markrequired").remove();
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
        SetPrinterAndSupplierVisibility();
    } else {
        $(".hdnNewExistingComp").each(function () {
            var section = $(this).closest(".dvPackNext").find(".miscOpsClass");
            section.find(".markrequired").remove();
            section.find(".requiredpm").removeClass("requiredpm");
            if ($(this).val().trim().toLocaleLowerCase() == "new") {
                section.find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requiredpm");
                section.find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                if ($(this).parent().find(".hdnMaterialGroup5PackType").val() != "true") {
                    section.find(".dvDisplay .requiredpm").removeClass("requiredpm");
                    section.find(".dvDisplay .markrequired").remove();
                }
                section.find(".drpSAPSpecsChange").removeClass("requiredpm");
                section.find(".drpSAPSpecsChange").closest(".form-group").find(".markrequired").remove();
            } else {
                if ($(this).parent().find(".hdnIsTransferSemi").val() != "ts") {
                    section.find(".unitDims :input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requiredpm");
                    section.find(".unitDims :input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                }
                section.find(".drpSAPSpecsChange").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                section.find(".drpSAPSpecsChange").addClass("requiredpm");

            }
            if ($(this).parent().find(".hdnNewComponentExists").val() == "true") {
                if (!section.find(".drpPalletPatternChange").hasClass("requiredpm")) {
                    section.find(".drpPalletPatternChange").addClass("requiredpm");
                    section.find(".drpPalletPatternChange").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                    section.find(".drpPalletPatternChange").closest(".row").next().find(".palletUploadLbl").parent().prepend($("<span class='markrequired'>*</span>"));
                }
            }
        });
        OnPackTrialChange();
        $(".makePackLocation").removeClass("requiredpm");
        $(".makePackLocation").closest(".form-group").find(".markrequired").remove();
    }

    $("#txtSpecificationNo").removeClass("requireduc");
    $("#txtSpecificationNo").closest(".form-group").find(".markrequired").remove();
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