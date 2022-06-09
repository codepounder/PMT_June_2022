var requiredDiv = $("<span class='markrequired'>*</span>");
$(document).ready(function () {
    $(".accordion").click(function () {
        var dvNext = $(this).next();
        if (dvNext.hasClass("panel")) {
            dvNext.toggleClass('hideItem');
        }
        var dvPackNext = $(this).next().next();
        if (dvPackNext.hasClass("dvPackNext")) {
            dvPackNext.toggleClass('hideItem');
        }
    });
    $(".radiofix").each(function () {
        var objRadiosContainer = $(this);
        objRadiosContainer.find("input[type=radio]").each(function () {
            var objRadio = $(this);
            objRadio.change(function () {
                if (objRadio.get(0).checked) {
                    objRadiosContainer.find("input[type=radio]").each(function () {
                        if ($(this).get(0).id != objRadio.get(0).id) {
                            $(this).get(0).checked = false;
                        }
                    });
                }
            });
        });
    });
    $(".deleteComponent").each(function () {
        var deleteComp = $(this);
        var deleteCompRow = deleteComp.closest("tr");

        var deleted = deleteCompRow.find("#hdnDeletedStatus").val();
        if (deleted == "true") {
            deleteCompRow.hide();
        }
    });
    $(".GraphicsOnlyPE2").each(function () {
        $(this).addClass('hideItem');
    });
    GraphicsOnlyProc();
    GraphicsOnlyPM2();
    var url = window.location.href.toLocaleLowerCase();
    var projectType = $("#hdnProjectType").val();
    if (projectType == "Graphics Change Only") {
        if (url.indexOf("pe2.aspx") != -1) {
            $(".GraphicsOnlyPE2").each(function () {
                if ($("#hdnPageState").val() != "PE") {
                    $(this).removeClass('hideItem');
                    if (!$(this).find("#ddlAllAspectsApprovedFromPEPersp").hasClass("requiredpm")) {
                        $(this).find("#ddlAllAspectsApprovedFromPEPersp").addClass("required");
                        $(this).find("#ddlAllAspectsApprovedFromPEPersp").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                    }
                }
            });
        }
    }
    AllAspectsApprovedFromPEPerspChanged();
    IsAllProcInfoCorrectChanged();
});
function GraphicsOnlyProc() {
    $(".GraphicsOnlyProc").each(function () {
        $(this).addClass('hideItem');
    });

    var projectType = $("#hdnProjectType").val();
    var url = window.location.href.toLocaleLowerCase();

    if (projectType == "Graphics Change Only") {
        if (url.indexOf("proc.aspx") != -1) {
            $(".GraphicsOnlyProc").each(function () {
                if ($("#drpNew").val() == "New") {
                    $(this).removeClass('hideItem');
                    if (!($(this).find("#ddlIsAllProcInfoCorrect").hasClass("requiredpm"))) {
                        $(this).find("#ddlIsAllProcInfoCorrect").addClass("requiredpm");
                        $(this).find("#ddlIsAllProcInfoCorrect").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                    }
                }
            });
        }
    }
}
function AllAspectsApprovedFromPEPerspChanged() {
    var projectType = $("#hdnProjectType").val();
    var GraphicsOnly = $("#ddlAllAspectsApprovedFromPEPersp").closest('.GraphicsOnly');
    GraphicsOnly.find(".WhatIsIncorrectPE").removeClass("required");
    GraphicsOnly.find(".WhatIsIncorrectPE").find(".markrequired").remove()
    GraphicsOnly.find(".WhatIsIncorrectPE").addClass('hideItem');
    GraphicsOnly.find(".WhatIsIncorrectPE").parent().addClass('hideItem');

    if (projectType == "Graphics Change Only") {
        if ($("#ddlAllAspectsApprovedFromPEPersp").val() == "No") {
            GraphicsOnly.find(".WhatIsIncorrectPE").removeClass('hideItem');
            GraphicsOnly.find("#txtWhatIsIncorrectPE").addClass("required");
            GraphicsOnly.find("#txtWhatIsIncorrectPE").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            GraphicsOnly.find(".WhatIsIncorrectPE").parent().removeClass('hideItem');
        } else {
            GraphicsOnly.find("#txtWhatIsIncorrectPE").val("");
        }
    }
}
function IsAllProcInfoCorrectChanged() {
    var projectType = $("#hdnProjectType").val();
    var GraphicsOnly = $("#ddlIsAllProcInfoCorrect").closest('.GraphicsOnlyProc');
    GraphicsOnly.find(".WhatProcInfoHasChanged").removeClass("required");
    GraphicsOnly.find(".WhatProcInfoHasChanged").find(".markrequired").remove()
    GraphicsOnly.find(".WhatProcInfoHasChanged").addClass('hideItem');
    $(this).parent().addClass('hideItem');
    var url = window.location.href.toLocaleLowerCase();

    if (url.indexOf("proc.aspx") != -1 && projectType == "Graphics Change Only") {
        if ($("#ddlIsAllProcInfoCorrect").val() == "No") {
            GraphicsOnly.find(".WhatProcInfoHasChanged").removeClass('hideItem');
            GraphicsOnly.find("#txtWhatProcInfoHasChanged").addClass("required");
            GraphicsOnly.find("#txtWhatProcInfoHasChanged").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        } else {
            GraphicsOnly.find("#txtWhatProcInfoHasChanged").val("");
        }
    }
}
function GraphicsOnlyPM2() {
    $(".GraphicsOnlyPM2").each(function () {
        $(this).addClass('hideItem');
    });

    var projectType = $("#hdnProjectType").val();
    var url = window.location.href.toLocaleLowerCase();

    var PkgComponent = $("#drpPkgComponent option:selected").text();
    var VisibleForPkgComponent = true;
    if (PkgComponent != null) {
        if (PkgComponent.indexOf('Candy') != -1 || PkgComponent.indexOf('Purchased Candy') != -1 || PkgComponent.indexOf('Transfer') != -1 || PkgComponent.indexOf('Finished') != -1) {
            VisibleForPkgComponent = false;
        }
    }

    if (projectType == "Graphics Change Only") {
        if (url.indexOf("obmsecondreview.aspx") != -1) {
            $(".GraphicsOnlyPM2").each(function () {
                if (VisibleForPkgComponent && $("#drpNew").val() == "New") {
                    $(this).removeClass('hideItem');
                } else {
                    $(this).addClass('hideItem');
                }
            });
        }
    }
}
function deleteComponent(clicked) {
    var button = $(clicked);
    var component = $(clicked);
    var MaterialTypeArray = ["Transfer Semi", "Purchased Candy Semi"]

    var hdnCompassListItemId = component.closest("tr").find("#hdnCompassListItemId").val();
    var hdnItemID = component.closest("tr").find("#hdnId").val();
    var DeletedIds = $("#hdnDeletedCompIds").val() + ";" + hdnItemID;
    var ComponentType = component.closest("tr").find("#hdnComponentType").val();
    var No = "No";
    var Yes = "Yes";
    var HasChildItems = false;

    if (MaterialTypeArray.indexOf(ComponentType) >= 0) {
        $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass Packaging Item List')/items?$select=ID,CompassListItemId,ParentID&$filter=(ParentID eq '" + hdnItemID + "' and Deleted ne '" + Yes + "' and CompassListItemId eq '" + hdnCompassListItemId + "')&$top=500",
            method: 'GET',
            dataType: 'json',
            headers: {
                Accept: "application/json;odata=verbose"
            },
            success: function (results1) {
                var results = new Array();
                $.each(results1.d.results, function (index, element) {
                    results.push(element);
                });
                if (results.length > 0) {
                    HasChildItems = true;
                }
                else {
                    HasChildItems = false;
                }
            },
            complete: function () {
                if (HasChildItems) {
                    $('#DialogDeleteSemisMessage').modal('show')
                }
                else {
                    var component = $(clicked);
                    component.closest("tr").find("#hdnDeletedStatus").val("true");
                    $("#hdnDeletedCompIds").val(DeletedIds);
                    component.closest("tr").hide();
                }
            }
        });
    }
    else {
        var component = $(clicked);
        component.closest("tr").find("#hdnDeletedStatus").val("true");
        $("#hdnDeletedCompIds").val(DeletedIds);
        component.closest("tr").hide();
    }
}
function DeletePackagingItem(PackagingItemId) {
    //Fetch the values from the input elements
    var Deleted = "Yes";
    var listName = 'Compass Packaging Item List';
    var itemType = GetItemTypeForListName(listName);

    var item = {
        "__metadata": { "type": itemType },
        "Deleted": Deleted
    };

    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/web/lists/GetByTitle('Compass Packaging Item List')/items(" + PackagingItemId + ")",
        method: "POST",
        data: JSON.stringify(item),
        headers: {
            "accept": "application/json;odata=verbose",
            "content-type": "application/json;odata=verbose",
            "X-RequestDigest": $("#__REQUESTDIGEST").val(),
            "IF-MATCH": "*",             //Overrite the changes in the sharepoint list item
            "X-HTTP-Method": "MERGE"      // Specifies the update operation in sharepoint list
        },
        success: function (data) {
            console.log(PackagingItemId + " deleted successfully!");
        },
        error: function (error) {
            console.log(JSON.stringify(error));

        }
    })
}

function changeRowColor(arg) {
    var anchor = $("#" + arg.id);
    anchor.addClass("hideItem");
    var dvMain = anchor.parent().parent();
    dvMain.addClass("bgcolor");
}
function dimensionsUpdate() {
    var unitDText = $("#txtSalesUnitDimensionsd.salesDims").closest(".miscOpsClass").find("#txtUnitMeasurementsL").val();
    $("#txtSalesUnitDimensionsd.salesDims").val(unitDText);
    var unitWText = $("#txtSalesUnitDimensionsw.salesDims").closest(".miscOpsClass").find("#txtUnitMeasurementsW").val();
    $("#txtSalesUnitDimensionsw.salesDims").val(unitWText);
    var unitHText = $("#txtSalesUnitDimensionsH.salesDims").closest(".miscOpsClass").find("#txtUnitMeasurementsH").val();
    $("#txtSalesUnitDimensionsH.salesDims").val(unitHText);
    var caseDText = $("#txtSalesCaseDimensionsD.salesDims").closest(".miscOpsClass").find("#txtCaseMeasurementsL").val();
    $("#txtSalesCaseDimensionsD.salesDims").val(caseDText);
    var caseHText = $("#txtSalesCaseDimensionsH.salesDims").closest(".miscOpsClass").find("#txtCaseMeasurementsH").val();
    $("#txtSalesCaseDimensionsH.salesDims").val(caseHText);
    var caseWText = $("#txtSalesCaseDimensionsW.salesDims").closest(".miscOpsClass").find("#txtCaseMeasurementsW").val();
    $("#txtSalesCaseDimensionsW.salesDims").val(caseWText);
}
function enableDisableCountry() {
    if ($('#drpTSMakeLocation option:selected').text().indexOf('Externally') == -1) {
        $("#drpTSCountryOfOrigin").removeClass('requireduc');
        $("#drpTSCountryOfOrigin").closest(".form-group").find(".markRequired").remove();
    }
}
function conditionalChecks() {
    if ($("#drpInternalTransferNeeded option:selected").text() != 'Yes') {
        $("#dvLikeItem").hide();
        $("#dvLikeItemDesc").hide();
        $("#btnFind").hide();
        $("#dvShipper").addClass('hideItem');
    }
    else {
        $("#dvLikeItem").show();
        $("#dvLikeItemDesc").show();
        $("#btnFind").show();
        $("#dvShipper").removeClass('hideItem');
    }
    if ($("#txtDisplayUPC").length > 0) {
        if ($("#txtDisplayUPC").val().toLowerCase().trim() == "needs new") {
            $("#spanDisplayUPC").show();
            $("#txtDisplayUPC").addClass("required");
        }
        else {
            $("#spanDisplayUPC").hide();
            $("#txtDisplayUPC").removeClass("required");
        }
    }
}
function chkLength(ctrlId, lengthReq) {
    return checkNumberLength(ctrlId, lengthReq);
}
function RequestPrinter(arg) {
    if (arg == 'request') {
        $("#btnRequestPrinter").hide();
        $("#btnCancelRequestPrinter").show();
        $("#dvAddPrinter").addClass("showButton").removeClass("hideButton");
    }
    if (arg == 'cancel') {
        $("#btnRequestPrinter").show();
        $("#btnCancelRequestPrinter").hide();
        $("#dvAddPrinter").addClass("hideButton").removeClass("showButton");
    }
}
function hideSelectButton(arg) {
    $('.' + arg).each(function (i, obj) {
        $(this).addClass("hideItem");
    });
}
function chkRequest(arg) {
    if (arg == 'printer') {
        var p = $("#txtNewPrinter").val().trim();
        if (p == "") {
            $("#lblPrinterError").text("Please enter printer");
            return false;
        }
        else {
            $("#lblPrinterError").val("");
        }
    }
    if (arg == 'substrate') {
        var p = $("#txtNewSubstrate").val().trim();
        if (p == "") {
            $("#lblSubstrateError").text("Please enter substrate");
            return false;
        }
        else {
            $("#lblSubstrateError").val("");
        }
    }
}
function validateBOMControl(btn) {
    return validateuc('#dverror_message', '#error_message');
}
function GraphicsCheck(arg) {
    var anchor = $("#" + arg.id);
    var idGraphicsBrief = anchor.closest(".ucmain").find(".GraphicsBrief");
    var idGraphicsVendor = anchor.closest(".ucmain").find(".drpGraphicsVendor");

    if ($("#" + arg.id + " option:selected").text() == 'Yes') {
        if (!idGraphicsBrief.hasClass("requireduc")) {
            idGraphicsBrief.addClass("requireduc");
            idGraphicsBrief.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            idGraphicsVendor.addClass("requireduc");
            idGraphicsVendor.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }
    }
    else {
        idGraphicsBrief.removeClass("requireduc");
        idGraphicsBrief.closest(".form-group").find(".markrequired").remove();
        idGraphicsVendor.removeClass("requireduc");
        idGraphicsVendor.closest(".form-group").find(".markrequired").remove();
    }
}
function GraphicsCheckLoad(arg) {
    var anchor = $("#" + arg);
    var idGraphicsBrief = anchor.closest(".ucmain").find(".GraphicsBrief");
    var idGraphicsVendor = anchor.closest(".ucmain").find(".drpGraphicsVendor");

    if ($("#" + arg + " option:selected").text() == 'Yes') {
        if (!idGraphicsBrief.hasClass("requireduc")) {
            idGraphicsBrief.addClass("requireduc");
            idGraphicsBrief.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            idGraphicsVendor.addClass("requireduc");
            idGraphicsVendor.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }
    }
    else {
        idGraphicsBrief.removeClass("requireduc");
        idGraphicsBrief.closest(".form-group").find(".markrequired").remove()
        idGraphicsVendor.removeClass("requireduc");
        idGraphicsVendor.closest(".form-group").find(".markrequired").remove()
    }
}
function validateucSave() {
    $('#error_message').empty();
    var isValid = true;
    var url = window.location.href.toLocaleLowerCase();

    $('.requireduc').each(function (i, obj) {
        if (!$(this).parent().parent().hasClass('hideItem') && !$(this).closest(".row").hasClass("hideItem")) {
            if ($(this).is('input') || ($(this).is('textarea'))) {
                var value = $(this).val().trim();
                if (!$(this).prop("disabled") && !$(this).prop("readonly")) {
                    if (value == "") {
                        isValid = false;
                    }
                }
            }
            else {
                var value = $(this).find("option:selected").val();
                if (value == '-1') {
                    isValid = false;
                }

            }
        }
    });
    $('.drpPalletPatternChange').each(function (i, obj) {
        var dvMain = $(this).closest("div.miscOpsClass");
        var value = $(this).find("option:selected").val();
        if (value.toLowerCase() == "y" && dvMain.find("#hdnPalletPatterCount").val() == '0') {
            if (!$(this).closest(".OBMSetup").hasClass('hideItem')) {
                isValid = false;
            }
        }
    });
    //clearing out hidden transfer semi/candy semi fields
    if ($(".ipf.TSOnlyRow").hasClass("hideItem")) {
        $(".ipf.TSOnlyRow select").val("-1");
        $(".ipf.TSOnlyRow :input:not([type=hidden]):not([type=submit]):not([type=button])").val("");
    }
    if ($(".ipf.hideableRow").hasClass("hideItem")) {
        $(".ipf.hideableRow select").val("-1");
        $(".ipf.hideableRow :input:not([type=hidden]):not([type=submit]):not([type=button])").val("");
    }
    if (!isValid) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
    }
    return isValid;
}
function checkrequired() {
    var clicked = $("#btnSave");
    var top = clicked.position().top;
    var left = clicked.position().left;
    var width = clicked.outerWidth();
    var height = clicked.outerHeight();
    clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");

    var chkRequired = validateucSave();
    if (chkRequired)
        $("#hdnRequiredCheck").val("Completed");
    else
        $("#hdnRequiredCheck").val("Waiting");
    return true;
}
function checkrequiredValidate() {
    var clicked = $("#btnSaveValidate");
    var top = clicked.position().top;
    var left = clicked.position().left;
    var width = clicked.outerWidth();
    var height = clicked.outerHeight();
    clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");

    var chkRequired = validateuc('#dverror_messageuc', '#error_messageuc');
    if (chkRequired) {
        $("#hdnRequiredCheck").val("Completed");
    }
    else {
        $("#hdnRequiredCheck").val("Waiting");
        $(".disablingLoadingIcon").remove();
        return false;
    }
}
function IPFFieldCheck() {
    var idComponent = $("#txtMaterial");
    var idComponentDesc = $("#txtMaterialDescription");
    var idLikeMaterial = $("#txtLikeItem");
    var idLikeDescription = $("#txtLikeDescription");
    var idwhyLikeComponent = $("#txtLikeMaterial");
    var NewExisting = $("#drpNew").find("option:selected").text();
    var idComponentType = $("#drpPkgComponent");
    var flowThrough = $("#ddlFlowthrough");
    var TBD = $("#hdnTBDIndicator").val();

    $("#txtOldMaterial, #txtOldMaterialDesc").removeClass("requireduc");
    $("#txtOldMaterial, #txtOldMaterialDesc").closest(".form-group").find(".markrequired").remove();

    if (NewExisting.toLowerCase() == 'new') {
        if (idComponent.val() == "") {
            idComponent.val("NEEDS NEW");
        }
        if (idComponentDesc.val() == "") {
            idComponentDesc.val("NEEDS NEW");
        }
        if (idComponentType.find("option:selected").text() == "Transfer Semi" || idComponentType.find("option:selected").text() == "Purchased Candy Semi") {
            $(".TSOnlyRow.new").removeClass("hideItem");
            $(".TSOnlyRow.new").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").addClass("requireduc");
            $(".TSOnlyRow.new").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }
    } else if (NewExisting.toLowerCase() == 'existing') {
        //$(".ipf").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
        //$(".ipf").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
        idwhyLikeComponent.removeClass("requireduc");
        idwhyLikeComponent.closest(".form-group").find(".markrequired").remove();
        idLikeMaterial.removeClass("requireduc");
        idLikeMaterial.closest(".form-group").find(".markrequired").remove();
        idLikeDescription.removeClass("requireduc");
        idLikeDescription.closest(".form-group").find(".markrequired").remove();
        if (idComponentType.find("option:selected").text() == "Transfer Semi" || idComponentType.find("option:selected").text() == "Purchased Candy Semi") {
            $(".TSOnlyRow.new").addClass("hideItem");
            $(".TSOnlyRow.new").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".TSOnlyRow.new").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
        }
    } else {
        idwhyLikeComponent.removeClass("requireduc");
        idwhyLikeComponent.closest(".form-group").find(".markrequired").remove();
        if (idComponentType.find("option:selected").text() == "Transfer Semi" || idComponentType.find("option:selected").text() == "Purchased Candy Semi") {
            $(".TSOnlyRow.new").addClass("hideItem");
            $(".TSOnlyRow.new").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").removeClass("requireduc");
            $(".TSOnlyRow.new").find(":input:not([type=hidden]):not([type=submit]):not([type=button])").closest(".form-group").find(".markrequired").remove();
        }
    }

    flowThrough.prop("disabled", false);
    if (NewExisting == 'New' && TBD == "No") {
        if (!flowThrough.hasClass("requireduc")) {
            flowThrough.addClass("requireduc");
            flowThrough.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }
        $("#txtOldMaterial").removeClass("requireduc");
        $("#txtOldMaterial").closest(".form-group").find(".markrequired").remove();
        if ($("#hdnLOB").val() == "Everyday (000000025)") {
            if (flowThrough.val() == "-1") {
                flowThrough.find("option:contains(Soft)").attr('selected', 'selected');
                flowThrough.val("2");
            }
        }
    } else if (NewExisting == 'New' && TBD == "Yes") {
        flowThrough.addClass("requireduc");
        flowThrough.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        flowThrough.prop("disabled", false);
        if (flowThrough.find("option:selected").text() == "Soft") {
            $("#txtOldMaterial").addClass("requireduc");
            $("#txtOldMaterial").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        } else {
            $("#txtOldMaterial").removeClass("requireduc");
            $("#txtOldMaterial").closest(".form-group").find(".markrequired").remove();
        }

    } else if (NewExisting == 'Existing') {
        flowThrough.removeClass("requireduc");
        flowThrough.closest(".form-group").find(".markrequired").remove();
        if (flowThrough.val() == "-1") {
            flowThrough.find("option:contains(Not Applicable)").attr('selected', 'selected');
            flowThrough.val("3");
        }
        $("#txtOldMaterial").removeClass("requireduc");
        $("#txtOldMaterial").closest(".form-group").find(".markrequired").remove();
        flowThrough.prop("disabled", true);
    } else if (NewExisting == 'Network Move') {
        flowThrough.removeClass("requireduc");
        flowThrough.closest(".form-group").find(".markrequired").remove();
        flowThrough.prop("disabled", false);
        flowThrough.find("option:contains(Not Applicable)").attr('selected', 'selected');
        flowThrough.val("3");
        $("#txtOldMaterial").removeClass("requireduc");
        $("#txtOldMaterial").closest(".form-group").find(".markrequired").remove();
    } else {
        $("#txtOldMaterial").removeClass("requireduc");
        $("#txtOldMaterial").closest(".form-group").find(".markrequired").remove();
    }
}
//////////// Dialog Boxes
function openBasicDialogWithpackagingItemId(tTitle, docType, packagingItemId) {
    var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=' + packagingItemId + '&DocType=' + docType + '&CompassItemId=' + $("#hiddenItemId").val();
    var options = {
        url: url,
        title: tTitle,
        dialogReturnValueCallback: onPopUpCloseCallBack
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}
function openBasicDialog2(tTitle, docType, cl) {

    var str = cl.split(" ");
    var id = str[2];

    var url = '';
    if (id == "0")
        url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=0&DocType=' + docType + '&CompassItemId=' + $("#hiddenItemId").val();
    else
        url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=' + id + '&DocType=' + docType + '&CompassItemId=' + $("#hiddenItemId").val();

    var options = {
        url: url,
        title: tTitle,
        dialogReturnValueCallback: onPopUpCloseCallBack
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}
function OpenDialog(arg1, arg2) {
    var packId = $("#hdnPackagingItemId").val();
    $("#hdnPageState").val("PE");
    openBasicDialogWithpackagingItemId(arg1, arg2, packId);
}
function moveBOM(idBeingMoved, compassListItemId) {
    hideSelectButton('Select');
    url = '/_layouts/15/Ferrara.Compass/AppPages/MoveBOMForm.aspx?PackagingItemId=' + idBeingMoved + '&CompassItemId=' + compassListItemId;

    var options = {
        url: url,
        title: "Move BOM",
        dialogReturnValueCallback: function (result) {
            if (result == SP.UI.DialogResult.OK) {
                window.location.reload();
            }
            if (result == SP.UI.DialogResult.cancel) {
                window.location.reload();
            }
        }
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function SetPrinterAndSupplierVisibility() {
    /*
    "PrinterAndSupplierVisible Only if:
    i). Project is Not Coman
    ii.) and project is Not Novelty
    iii). and External Manufacturing Form is kicked off
    iv). and the Procurement Type on the External Manufacturing Form is not ""External Turnkey FG""
    v.) and component type is not ""Transfer Semi""
    vi.) and component type is not ""Purchased Candy Semi""
    vii.) and component type is not ""Candy Semi""
    
    If Displayed, pull in corresponding Yes/ No value selected for the component from the External Manufacturing form ""Review Printer / Supplier"" field and lock the value"
    */
    var PkgComponent = $("#drpPkgComponent option:selected").text();
    if ($("#hdnProductHierarchyLevel1").val() == 'Co-Manufacturing (000000027)') {//If coman hide
        $('#dvRevPrinterSupplier').addClass('hideItem');
        $("#ddlReviewPrinterSupplier").removeClass("requireduc");
        $("#ddlReviewPrinterSupplier").closest(".form-group").find(".markrequired").remove();
    } else if ($("#hdnNovelyProject").val() == 'Yes') {//if novelty hide
        $('#dvRevPrinterSupplier').addClass('hideItem');
        $("#ddlReviewPrinterSupplier").removeClass("requireduc");
        $("#ddlReviewPrinterSupplier").closest(".form-group").find(".markrequired").remove();
    } else if ($("#hdnExtMfgkickedoff").val() == 'Yes') {
        if ($("#hdnCoManClassification").val() == 'External Turnkey FG') { //if ext man kicked off and External Turnkey FG hide
            $('#dvRevPrinterSupplier').addClass('hideItem');
            $("#ddlReviewPrinterSupplier").removeClass("requireduc");
            $("#ddlReviewPrinterSupplier").closest(".form-group").find(".markrequired").remove();
        } else {
            if (PkgComponent != 'Transfer Semi' && PkgComponent != 'Candy Semi' && PkgComponent != 'Purchased Candy Semi') {//if ext man kicked off and  NOT External Turnkey FG  and NOT semi show
                $('#dvRevPrinterSupplier').removeClass('hideItem');
            } else {//if ext man kicked off and  NOT External Turnkey FG  and semi hide
                $('#dvRevPrinterSupplier').addClass('hideItem');
                $("#ddlReviewPrinterSupplier").removeClass("requireduc");
                $("#ddlReviewPrinterSupplier").closest(".form-group").find(".markrequired").remove();
            }
        }
    } else {
        $('#dvRevPrinterSupplier').addClass('hideItem');
        $("#ddlReviewPrinterSupplier").removeClass("requireduc");
        $("#ddlReviewPrinterSupplier").closest(".form-group").find(".markrequired").remove();
    }
}
function TSBarcodeGenerationVisibility() {
    var url = window.location.href.toLocaleLowerCase();
    var txt13DigitCode = $("#txt13DigitCode");
    var txt14DigitBarcode = $("#txt14DigitBarcode");

    if (url.indexOf("/sapbomsetup.aspx") == -1 && url.indexOf("/obmsecondreview.aspx") == -1) {
        if (!$(".divTransferSemiBarcodeGeneration").hasClass("hideItem")) {
            $(".divTransferSemiBarcodeGeneration").addClass("hideItem");
        }

        if (!txt13DigitCode.hasClass("hideItem")) {
            txt13DigitCode.addClass("hideItem");
        }

        if (!txt14DigitBarcode.hasClass("hideItem")) {
            txt14DigitBarcode.addClass("hideItem");
        }

        txt13DigitCode.removeClass("requireduc");
        txt13DigitCode.closest(".form-group").find(".markrequired").remove();

        txt14DigitBarcode.removeClass("requireduc");
        txt14DigitBarcode.closest(".form-group").find(".markrequired").remove();
    } else {
        if ($("#drpPkgComponent option:selected").text().indexOf("Corrugated") != -1 && $("#drpNew option:selected").text() == 'New' && $("#hdnTSBarcodeGenerationVisibility").val() == "Yes") {
            $(".divTransferSemiBarcodeGeneration").removeClass("hideItem");
            txt13DigitCode.val($("#hdn13DigitCode").val());
            txt13DigitCode.removeClass("hideItem");
            txt14DigitBarcode.removeClass("hideItem");

            if (!txt13DigitCode.hasClass("requireduc")) {
                txt13DigitCode.addClass("requireduc");
                txt13DigitCode.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }

            if (!txt14DigitBarcode.hasClass("requireduc")) {
                txt14DigitBarcode.addClass("requireduc");
                txt14DigitBarcode.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            }

            if (url.indexOf("/sapbomsetup.aspx") == -1) {
                $('#txt14DigitBarcode').prop("readonly", true);
            }
        } else {
            if (!$(".divTransferSemiBarcodeGeneration").hasClass("hideItem")) {
                $(".divTransferSemiBarcodeGeneration").addClass("hideItem");
            }

            if (!txt13DigitCode.hasClass("hideItem")) {
                txt13DigitCode.addClass("hideItem");
            }

            if (!txt14DigitBarcode.hasClass("hideItem")) {
                txt14DigitBarcode.addClass("hideItem");
            }

            txt13DigitCode.removeClass("requireduc");
            txt13DigitCode.closest(".form-group").find(".markrequired").remove();

            txt14DigitBarcode.removeClass("requireduc");
            txt14DigitBarcode.closest(".form-group").find(".markrequired").remove();
        }
    }
}
function LoadPrintStyleTypes() {
    var Corrugated = "Corrugated";
    var film = "Film";

    $("#ddlFilmPrintStyle").children("option[class^=" + "PrintStyleType" + "]").hide();

    var drpComponentText = $("#drpPkgComponent").find("option:selected").text().toLocaleLowerCase();

    if (drpComponentText.indexOf('corrugated') != -1 || drpComponentText.indexOf('paperboard') != -1) {
        $("#ddlFilmPrintStyle").children("option[class$=" + Corrugated + "]").show();
    } else if (drpComponentText.indexOf('film') != -1) {
        $("#ddlFilmPrintStyle").children("option[class$=" + film + "]").show();
    }
}