var requiredDiv = $("<span class='markrequired'>*</span>");
$(document).ready(function () {
    var url = window.location.href.toLocaleLowerCase();
    if (url.indexOf("pmsecondreview.aspx") != -1 || url.indexOf("pmfirstreview.aspx") != -1) {
        DTDisplayBOMGridTable();

        var tbl = $('#BOMGridTable');
        if (url.indexOf("pmsecondreview.aspx") != -1) {
            tbl.DataTable().column(11).visible(false);
        }
    }
    onLoadChecks();
    //BindHierarchiesOnLoad();
});
function onLoadChecks() {
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
    GraphicsOnlyPM2();
    GraphicsOnlyProc();
    var url = window.location.href.toLocaleLowerCase();
    var projectType = $("#hdnProjectType").val();
    if (projectType == "Graphics Change Only") {
        if (url.indexOf("pe2.aspx") != -1) {
            $(".GraphicsOnlyPE2").each(function () {
                if ($("#hdnPageState").val() != "PE") {
                    $(this).removeClass('hideItem');
                    if (!$(this).find("#ddlAllAspectsApprovedFromPEPersp").hasClass("requiredpm")) {
                        $(this).find("#ddlAllAspectsApprovedFromPEPersp").addClass("requiredpm");
                        $(this).find("#ddlAllAspectsApprovedFromPEPersp").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                    }
                }
            });
        }
    }
    AllAspectsApprovedFromPEPerspChanged();
    IsAllProcInfoCorrectChanged();
}
function GraphicsOnlyProc() {
    $(".GraphicsOnlyProc").each(function () {
        $(this).addClass('hideItem');
    });

    var projectType = $("#hdnProjectType").val();
    var url = window.location.href.toLocaleLowerCase();

    var PkgComponent = $("#drpPkgComponent option:selected").text();
    var VisibleForPkgComponent = true;
    if (PkgComponent != null) {
        if (PkgComponent.indexOf('Candy Semi') != -1 || PkgComponent.indexOf('Purchased Candy') != -1 || PkgComponent.indexOf('Transfer') != -1 || PkgComponent.indexOf('Finished') != -1) {
            VisibleForPkgComponent = false;
        }
    }

    if (url.indexOf("proc.aspx") != -1) {
        $(".GraphicsOnlyProc").each(function () {
            if (projectType == "Graphics Change Only" && VisibleForPkgComponent && $("#drpNew").val() == "New") {
                $(this).removeClass('hideItem');
                $(this).find("#ddlIsAllProcInfoCorrect").removeClass("requiredpm");
                $(this).find("#ddlIsAllProcInfoCorrect").closest(".form-group").find(".markrequired").remove();
                $(this).find("#ddlIsAllProcInfoCorrect").addClass("requiredpm");
                $(this).find("#ddlIsAllProcInfoCorrect").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            } else {
                $(this).addClass('hideItem');
                $(this).find("#ddlIsAllProcInfoCorrect").removeClass("requiredpm");
                $(this).find("#ddlIsAllProcInfoCorrect").find(".markrequired").remove();
                $(this).find("#ddlIsAllProcInfoCorrect").val("-1");
                $(this).find("#txtWhatProcInfoHasChanged").val("");
            }
        });
        if (projectType == "Graphics Change Only") {
            var btnAdd = $('#btnAdd');
            btnAdd.addClass("disabled");
            btnAdd.attr("disabled", "disabled");
            IsAllProcInfoCorrectChanged();
        }
    }
}
function AllAspectsApprovedFromPEPerspChanged() {
    var projectType = $("#hdnProjectType").val();
    var GraphicsOnly = $("#ddlAllAspectsApprovedFromPEPersp").closest('.GraphicsOnlyPE2');
    GraphicsOnly.find(".WhatIsIncorrectPE").removeClass("requiredpm");
    GraphicsOnly.find(".WhatIsIncorrectPE").find(".markrequired").remove()
    GraphicsOnly.find(".WhatIsIncorrectPE").addClass('hideItem');
    $(this).parent().addClass('hideItem');
    var url = window.location.href.toLocaleLowerCase();

    if (url.indexOf("pe2.aspx") != -1 && projectType == "Graphics Change Only") {
        if ($("#ddlAllAspectsApprovedFromPEPersp").val() == "No") {
            GraphicsOnly.find(".WhatIsIncorrectPE").removeClass('hideItem');
            GraphicsOnly.find("#txtWhatIsIncorrectPE").addClass("requiredpm");
            GraphicsOnly.find("#txtWhatIsIncorrectPE").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        } else {
            GraphicsOnly.find("#txtWhatIsIncorrectPE").val("");
        }
    }
}
function IsAllProcInfoCorrectChanged() {
    var projectType = $("#hdnProjectType").val();
    var PkgComponent = $("#drpPkgComponent option:selected").text();
    var VisibleForPkgComponent = true;
    if (PkgComponent != null) {
        if (PkgComponent.indexOf('Candy') != -1 || PkgComponent.indexOf('Purchased Candy') != -1 || PkgComponent.indexOf('Transfer') != -1 || PkgComponent.indexOf('Finished') != -1) {
            VisibleForPkgComponent = false;
        }
    }

    var GraphicsOnly = $("#ddlIsAllProcInfoCorrect").closest('.GraphicsOnlyProc');
    GraphicsOnly.find(".WhatProcInfoHasChanged").removeClass("requiredpm");
    GraphicsOnly.find(".WhatProcInfoHasChanged").find(".markrequired").remove();
    GraphicsOnly.find(".WhatProcInfoHasChanged").addClass('hideItem');
    $(this).parent().addClass('hideItem');
    var url = window.location.href.toLocaleLowerCase();

    if (url.indexOf("proc.aspx") != -1 && projectType == "Graphics Change Only") {
        if ($("#ddlIsAllProcInfoCorrect").val() == "No") {
            if (VisibleForPkgComponent && $("#drpNew").val() == "New") {
                GraphicsOnly.find(".WhatProcInfoHasChanged").removeClass('hideItem');
                GraphicsOnly.find("#txtWhatProcInfoHasChanged").addClass("requiredpm");
                GraphicsOnly.find("#txtWhatProcInfoHasChanged").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                GraphicsOnly.find("#txtWhatProcInfoHasChanged").parent().removeClass('hideItem');
            }
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
function deleteComponent2(iItemId, ComponentType, clicked) {
    var hdnCompassListItemId = $("#hdnCompassListItemId").val();
    var hdnItemID = iItemId;
    var Yes = "Yes";
    var HasChildItems = false;
    var DeletedIds = $("#hdnDeletedCompIds").val() + ";" + hdnItemID;
    var MaterialTypeArray = ["Transfer Semi", "Purchased Candy Semi"]

    ComponentType = ComponentType.replaceAll('%20', ' ')

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
                    $("#btnDeleteComponent").click();
                }
            }
        });
    }
    else {
        var component = $(clicked);
        component.closest("tr").find("#hdnDeletedStatus").val("true");
        $("#hdnDeletedCompIds").val(DeletedIds);
        component.closest("tr").hide();
        $("#btnDeleteComponent").click();
    }
}
function changeRowColor(arg) {
    var anchor = $("#" + arg.id);
    anchor.addClass("hideItem");
    var dvMain = anchor.parent().parent();
    dvMain.addClass("bgcolor");
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
function checkrequiredValidate_New() {
    var clicked = $("#btnSaveValidate");
    //var top = clicked.position().top;
    //var left = clicked.position().left;
    //var width = clicked.outerWidth();
    //var height = clicked.outerHeight();
    //clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");

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
    var idComponentType = $("#drpPkgComponent");
    var idComponentDesc = $("#txtMaterialDescription");
    var idLikeMaterial = $("#txtLikeItem");
    var idLikeDescription = $("#txtLikeDescription");
    var idwhyLikeComponent = $("#txtLikeMaterial");
    var flowThrough = $("#ddlFlowthrough");
    var NewExisting = $("#drpNew").find("option:selected").text();
    var TBD = $("#hdnTBDIndicator").val();

    $("#txtOldMaterial, #txtOldMaterialDesc, #flowThrough").removeClass("requireduc");
    $("#txtOldMaterial, #txtOldMaterialDesc, #flowThrough").closest(".form-group").find(".markrequired").remove();

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
        if ($("#hdnCoManClassification").val() == 'External Turnkey FG' || ($("#hdnCoManClassification").val() == 'Ferrara Finished Good' && $("#hdnParentType").val() == "Transfer Semi")) { //if ext man kicked off and External Turnkey FG hide
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
function generateLinkPackMeas(clicked) {

    var el = $(clicked).closest(".dvPackNext");
    var url = el.find("#txPalletPatternLink").val();

    $(clicked).closest(".row").find("#lblURLError").html("");
    var parentName = el.find("#hdnUCBOMComponentType").val();
    if (parentName == "" || parentName == "BOM") {
        parentName = "Finished Good";
    }
    var matNumber = el.prev().prev().find("#lblDesc").html();
    if (matNumber == "") {
        matNumber = "XXXXX";
    }
    var index1 = matNumber.indexOf(":");
    var res = matNumber.substring(index1 + 2);
    index1 = res.indexOf(":");
    res = res.substring(0, index1);
    var linkName = parentName + ": " + res + ": Pallet Pattern";
    if (url == "") {
        el.find("#generatedLink").addClass("hideItem");
        el.find("#generatedLink").attr("href", "");
        el.find("#generatedLink").html("");
    } else {
        el.find("#generatedLink").html(linkName);
        el.find("#generatedLink").removeClass("hideItem");
        el.find("#generatedLink").attr("href", url);
    }
    loadingIconAdded = true;
    $(".disablingLoadingIcon").remove();
}
function GraphicsCheckSpec(arg) {
    var anchor = $("#" + arg);
    var idGraphicsBrief = anchor.closest(".ucmain").find(".GraphicsBrief");
    var idGraphicsVendor = anchor.closest(".ucmain").find(".drpGraphicsVendor");
    var idComponentType = $("#drpPkgComponent option:selected").text();
    var idPrintStyle = $("#ddlFilmPrintStyle");

    if ($("#" + arg + " option:selected").text() == 'Yes') {
        if (!idGraphicsBrief.hasClass("requireduc")) {
            idGraphicsBrief.addClass("requireduc");
            idGraphicsBrief.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            idGraphicsVendor.addClass("requireduc");
            idGraphicsVendor.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }
        /*if (idComponentType.indexOf("Film") != -1 || idComponentType.indexOf("Corrugate") != -1) {
            idPrintStyle.addClass("requireduc");
            idPrintStyle.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }*/
    }
    else {
        idGraphicsBrief.removeClass("requireduc");
        idGraphicsBrief.closest(".form-group").find(".markrequired").remove();
        idGraphicsVendor.removeClass("requireduc");
        idGraphicsVendor.closest(".form-group").find(".markrequired").remove();
    }
}
function TSBarcodeGenerationVisibility() {
    var url = window.location.href.toLocaleLowerCase();
    var txt13DigitCode = $("#txt13DigitCode");
    var txt14DigitBarcode = $("#txt14DigitBarcode");

    if (url.indexOf("/sapbomsetup.aspx") == -1 && url.indexOf("/obmsecondreview.aspx") == -1 && url.indexOf("/pmsecondreview.aspx") == -1) {
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
$(document.body).on("change", "#ddlComponentTab", function () {
    $('#hdnParentID').val($("#ddlComponentTab option:selected").val());

    UpdateTitle();
    var BOMComponents = $('#BOMGridTable').DataTable();
    regex = '\\b' + this.value + '\\b';

    BOMComponents
        .columns(13)
        .search(regex, true, false)
        .draw();
});
function UpdateTitle() {
    var title = $("#ddlComponentTab option:selected").text().split("(")[0].trim();
    if (title.indexOf('Finished Good') != -1) {
        title = 'Finished Good';
    }
    $("#lblDesc").text(title);
}
function DTDisplayBOMGridTable() {
    var BOMComponents = $('#BOMGridTable').DataTable({
        "autoWidth": false,
        "info": false,
        "ordering": false,
        "paging": false,
        "scrollX": false,
        data: BOMGridTableData,
        "createdRow": function (row, data, dataIndex) {
            if (data.CompStatus == "Red") {
                $(row).addClass("StatusRed");
            } else {
                $(row).addClass("StatusGreen");
            }
        },
        "columnDefs": [
            { className: "hide", targets: [13] },
            {
                className: "PackagingComponent", targets: 7
            }
        ],
        fixedColumns: true,
        columns: [
        {
            'data': 'Edit',
            'render': function (data, type, row, meta) {
                ImageUrl = "/_layouts/15/Ferrara.Compass/images/Edit.gif"
                var colors = row.CompStatus;
                return '<i class="fa fa-edit fa-lg" aria-hidden="true" onclick=OpenCompEditForm("' + row.Id + '") style="cursor: pointer;"></i>';
            }
        },
        {
            'data': 'CompStatus',
            'render': function (data, type, row, meta) {
                var colors = "0";
                return '<div title="' + colors + '">' + colors + '</span>';
            }
        },
        {
            'data': 'NewExisting',
        },
        {
            'data': 'MaterialNumber',
        },
        {
            'data': 'MaterialDescription',
        },
        {
            'data': 'CurrentLikeItem'
        },
        {
            'data': 'CurrentOldItem'
        },
        {
            'data': 'PackagingComponent'
        },
        {
            'data': 'PackUnit'
        },
        {
            'data': 'PackQuantity'
        },
        {
            'data': 'Flowthrough',
        },
        {
            'data': 'Id',
            'render': function (data, type, row, meta) {
                return '<i class="fa fa-arrows fa-lg" aria-hidden="true" onclick=moveBOM("' + row.Id + '","' + row.CompassListItemId + '") style="cursor: pointer;"></i> ';
            }
        },
        {
            'data': 'Id',
            'render': function (data, type, row, meta) {
                return '<i class="fa fa-trash fa-lg" aria-hidden="true" onclick=deleteComponent2("' + row.Id + '","' + row.PackagingComponent.trim().replace(/ /g, '%20') + '",this) style="cursor: pointer;"></i> ';
            }
        },
        {
            'data': 'ParentID',
        }
        ]
    });

    var title = $("#ddlComponentTab option:selected").text().split("(")[0].trim();
    if (title.indexOf('Finished Good') != -1) {
        title = 'Finished Good';
    }
    $("#lblDesc").text(title);

    regex = '\\b' + $("#ddlComponentTab").val() + '\\b';
    BOMComponents
        .columns(13)
        .search(regex, true, false)
        .draw();

    if ($('#BOMGridTable tbody tr td').hasClass("dataTables_empty")) {
        $('#BOMGridTable').css('width', '');
    }
}

function OpenCompEditForm(iItemId) {
    $("#hdnPackagingItemIdClicked").val(iItemId);
    $("#btnPopulateComponent").click();
}
function AddNewPackagingItem() {
    loadingIconAdded = false;
    $(".disablingLoadingIcon").remove();
    $("#txtComponentTabSelected").val($("#ddlComponentTab option:selected").val());
    $("#hdnPackagingItemIdClicked").val('0');
    $("#hdnPackagingItemId").val('0');
    $('#hdnParentID').val($("#ddlComponentTab option:selected").val());
    $('#dialog-form').modal('show');
}
function ShowCompDetailsForm(iItemId) {
    $('#dialog-form').modal('show');
    OnLoadCompForEdit();
    onLoadChecks();
    BindHierarchiesOnLoad();
}
function CloseCompDetailsForm(iItemId) {
    clearAllSelectionsFromCompDetailsForm();
    $(".disablingLoadingIcon").remove();
    $('#dialog-form').modal('hide');

    $("#ddlComponentTab").val($('#hdnParentID').val());
    var BOMComponents = $('#BOMGridTable').DataTable();
    regex = '\\b' + $('#hdnParentID').val() + '\\b';

    BOMComponents
        .columns(13)
        .search(regex, true, false)
        .draw();

    UpdateTitle();
}
function clearAllSelectionsFromCompDetailsForm() {
    $('.CompDetails').each(function (i, obj) {
        $(this).prop("disabled", false);
        if ($(this).is('input') || ($(this).is('textarea'))) {
            $(this).val('');
        }
        else {
            if ($(this).is("select")) {
                $(this).val('-1')
            };
            if ($(this).is(':checkbox, :radio')) {
                $(this).prop('checked', false);

            }
        }
    });
    generateLink();
    $('.CompDetailsAttachments').remove();
}
function OnLoadCompForEdit() {
    TSBarcodeGenerationVisibility();
}
function generateLink() {
    var parentName = $("#hdnParentType").val();
    if (parentName == "") {
        parentName = "Finished Good";
    }
    var matNumber = $("#txtMaterial").val();
    if (matNumber == "") {
        matNumber = "XXXXX";
    }
    var linkName = parentName + ": " + matNumber + ": Dieline Link";
    if ($("#txtDielineLink").val() == "") {
        $("#generatedLinkEdit").addClass("hideItem");
        $("#generatedLinkEdit").attr("href", "");
        $("#generatedLinkEdit").html("");
    } else {
        $("#generatedLinkEdit").html(linkName);
        $("#generatedLinkEdit").removeClass("hideItem");
        $("#generatedLinkEdit").attr("href", $("#txtDielineLinkEdit").val());
    }
    loadingIconAdded = true;
    $(".disablingLoadingIcon").remove();
    return false;
}
function BindHierarchiesOnLoad() {
    var drpComponent = $('#drpPkgComponent');
    var PHL1 = $('.PHL1');
    var PHL2 = $('.PHL2');
    var Brand = $('.Brand');
    var txtProfitCenterUC = $('#txtProfitCenterUC');

    var PHL2Id = PHL2.attr('id');
    var BrandId = Brand.attr('id');

    $("#" + PHL2Id).children("option[class^=" + "PHLOptions" + "]").hide();
    $("#" + BrandId).children("option[class^=" + "PHLOptions" + "]").hide();

    var drpComponentText = $(drpComponent).find("option:selected").text().toLocaleLowerCase();

    if (drpComponentText == "transfer semi" || drpComponentText == "purchased candy semi") {
        var PHL1Value = $(PHL1).val();

        if (PHL1Value != "" && PHL1Value != "-1") {
            $("#" + PHL2Id).children("option[class$=" + PHL1Value + "]").show();

            var PHL2Text = $(PHL2).find("option:selected").text();
            var PHL2Value = $(PHL2).val();

            if (PHL2Value != "" && PHL2Value != "-1" && PHL2Text != "") {
                $("#" + BrandId).children("option[class$=" + PHL2Text
                    .replace(/ /g, '')
                    .replaceAll(')', '')
                    .replaceAll('(', '')
                    .replaceAll("'", "")
                    .replaceAll('-', '')
                    .replaceAll('&', '')
                    .replaceAll('/', '')
                    + "]"
                    ).show();
            }
        }
    }
    else {
        $(PHL1).val("-1");
        $(PHL2).val("-1");
        $(Brand).val("-1");
        $(txtProfitCenterUC).val("");
    }
}

function BindPHL2DropDownItemsByPHL1(PHL1) {
    var PHL1Value = $(PHL1).val();
    var PHL2 = $('.PHL2');
    var Brand = $('.Brand');
    var txtProfitCenterUC = $('#txtProfitCenterUC');
    var hdnProfitCenterUC = $('#hdnProfitCenterUC');

    PHL2.val("-1");
    Brand.val("-1");
    txtProfitCenterUC.val("");
    hdnProfitCenterUC.val("");

    if (PHL1Value != "" && PHL1Value != "-1") {
        var PHL2Id = $('.PHL2').attr('id');
        $("#" + PHL2Id).children("option[class^=" + "PHLOptions" + "]").hide();
        $("#" + PHL2Id).children("option[class$=" + PHL1Value + "]").show();
    }
}

function BindBrandDropDownItemsByPHL2(PHL2) {
    var PHL2Text = $(PHL2).find("option:selected").text();
    var PHL2Value = $(PHL2).val();
    var Brand = $('.Brand');
    var txtProfitCenterUC = $('#txtProfitCenterUC');
    var hdnProfitCenterUC = $('#hdnProfitCenterUC');

    Brand.val("-1");
    txtProfitCenterUC.val("");
    hdnProfitCenterUC.val("");

    if (PHL2Value != "" && PHL2Value != "-1" && PHL2Text != "") {
        var BrandId = $('.Brand').attr('id');
        $("#" + BrandId).children("option[class^=" + "PHLOptions" + "]").hide();
        $("#" + BrandId).children("option[class$=" + PHL2Text
             .replace(/ /g, '')
             .replaceAll(')', '')
             .replaceAll('(', '')
             .replaceAll("'", "")
             .replaceAll('-', '')
             .replaceAll('&', '')
             .replaceAll('/', '')
             + "]"
         ).show();
    }
}

function GetProfitCenter(Brand) {
    var BrandId = $(Brand).attr('id');
    var BrandText = $(Brand).find("option:selected").text();
    var BrandValue = $(Brand).val();

    dvBOMRow = $(Brand).closest("div.SAPVerifyItem");
    var PHL2 = $('.PHL2');
    var PHL2Text = $(PHL2).find("option:selected").text();
    var txtProfitCenterUC = $('#txtProfitCenterUC');
    var hdnProfitCenterUC = $('#hdnProfitCenterUC');

    var PHL2Value = $(PHL2).val();
    PHL2Text = PHL2Text
        .replace(/'/g, "%27%27")
        .replace(/&/g, "%26");
    BrandText = BrandText
        .replace(/'/g, "%27%27")
        .replace(/&/g, "%26");

    if (BrandValue != "" && BrandValue != "-1" && BrandText != "" && PHL2Value != "-1" && PHL2Text != "") {
        $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Material Group1 Lookup')/items?$select=Id,Title,Compass_Value,ParentPHL2,ProfitCenter&$filter=(Title eq '" + BrandText + "') and (ParentPHL2 eq '" + PHL2Text + "') and (RoutingEnabled eq '1')",
            method: 'GET',
            dataType: 'json',
            headers: {
                Accept: "application/json;odata=verbose"
            },
            success: function (results) {
                var BrandItem = results.d.results[0];
                if (results.d.results.length) {
                    var ProfitCenter = BrandItem.ProfitCenter;

                    txtProfitCenterUC.val(ProfitCenter);
                    hdnProfitCenterUC.val(ProfitCenter);
                }
            },
            error: function (xhr, status, error) {
            }
        });
    }
    else {
        txtProfitCenterUC.val("");
        hdnProfitCenterUC.val("");
    }
}
