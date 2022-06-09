$(document).ready(function () {
    var url = window.location.href.toLocaleLowerCase();
    if (url.indexOf('stagegateprojectpanel.aspx') == -1) {
        updateTotals();
    }
    updateStatus();
    projectStageChange();
    $(".listColor").change(function () {
        var required = "";
        if ($(this).hasClass("required")) {
            required = " required";
        }
        var color = $(this).find("option:selected").text();
        color = color + " listColor" + required;
        $(this).attr("class", color);
    });
    $(".Summaryaccordion").not("#btnGenerateFinancePDF, #btnFinanceBrief").click(function () {
        showhideAccordion($(this));
    });

    if ($("#ddlPostLaunch option:selected").text() == "Yes") {
        $("#postLaunchMessage").removeClass("hideItem");
    } else {
        $("#postLaunchMessage").addClass("hideItem");
    }

    $("#hdnDeleted").each(function () {
        if ($(this).val() == "true") {
            $(this).closest(".FinanceBriefs").addClass("hide");
        }
    });
    UpdateAllSigns();
    ProjectTypeOnLoadChanges();
    CalculateFinancials();
    BusinessFunctionChanged();
});
function runTinyMC() {
    tinyMCE.remove();
    tinyMCE.init({
        plugins: "lists advlist",
        toolbar: '| bold italic underline | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link',
        selector: '.RTBox',
        menubar: false
    });
}
function runTinyMCDisabled() {
    tinyMCE.remove();
    tinyMCE.init({
        plugins: "lists advlist",
        toolbar: '| bold italic underline | forecolor backcolor | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link',
        selector: '.RTBox',
        menubar: false,
        readonly: 1
    });
}
/*,
        setup: function (ed) {
            ed.on("change", function () {
                updateHidden(ed);
            })
        }*/
function updateHidden(clicked) {
    var el = $(clicked);
    var Id = el.attr("id"); alert(Id);
    var copy = el.val(); alert(copy);
    var length = Id.length;
    var newId = Id.substr(0, length);
    $("#" + newId).val(copy);
}
function changeMade(id) {
    $("#" + id).closest("tr").find("#hdnChangeMade").val("true");
}
function deleteBrief(clicked) {
    var el = $(clicked);
    if (confirm("Are you sure you want to delete brief?")) {
        el.closest(".FinanceBriefs").addClass("hide");
        el.closest(".FinanceBriefs").find("#hdnDeleted").val("true");
        return false;
    } else {
        el.addClass("noIcon");
        return false;
    }
}
function setDropdownColor() {
    $(".listColor").each(function () {
        var required = "";
        if ($(this).hasClass("required")) {
            required = " required";
        }
        var color = $(this).find("option:selected").text();
        color = color + " listColor" + required;
        $(this).attr("class", color);
    });
}
function SGSValidate() {
    $(".required").each(function () {
        var el = $(this);
        if (el.hasClass("listColor")) {
            var comments = $(this).closest("tr").find("textarea");
            comments.removeClass("txtRequired");
            var selected = el.find("option:selected").text();
            if (selected != "Green" && selected != "N/A") {
                comments.addClass("txtRequired");
            }
        }
    });
    var isValid = true;
    $('#error_message').empty();
    $(".highlightElement").removeClass("highlightElement");
    $(".deliverablesTable .SGSApplicable").each(function () {
        var el = $(this);
        var id = $(this).attr("name");
        if (el.closest("tr").find("#hdnDeletedStatus").val() == "false") {
            if (el.find("option:selected").val() == -1) {
                $("#dverror_message").show();
                $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">Applicable to Project is required.  <a style="color:darkblue" onclick=setFocusElementRepeater(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                isValid = false;
            } else if (el.find("option:selected").val() == "Y") {
                var status = el.closest("tr").find(".SGSStatus").val();
                if (status == "4") {
                    $("#dverror_message").show();
                    $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">Applicable to Project cannot be \"N/A\".  <a style="color:darkblue" onclick=setFocusElementRepeater(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                    isValid = false;
                }
            }
        }
    });
    $(".deliverablesTable .SGSStatus").each(function () {
        var el = $(this);
        var id = $(this).attr("name");
        if (el.closest("tr").find("#hdnDeletedStatus").val() == "false") {
            if (el.find("option:selected").val() == -1) {
                $("#dverror_message").show();
                $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">Status is required.  <a style="color:darkblue" onclick=setFocusElementRepeater(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                isValid = false;
            }
        }
    });
    $(".required").each(function () {
        var el = $(this);
        var message = el.attr("title");
        var id = $(this).attr("id");
        if (el.find("option:selected").val() == -1) {
            $("#dverror_message").show();
            $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + message + ' is required.  <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
            isValid = false;
        }
    });
    $(".txtRequired").each(function () {
        var el = $(this);
        var id = $(this).attr("id");
        var message = el.attr("title")
        if (el.val() == "") {
            $("#dverror_message").show();
            $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + message + ' are required.  <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
            isValid = false;
        }
    });
    if (!isValid) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
        setFocusError();
    } else {
        var url = window.location.href.toLocaleLowerCase();
        if (url.indexOf('stagegatelaunchdeliverables.aspx') == -1) {
            if ($("#hdnPrevSubmittedDate").val() == "" || $("#hdnContinueSubmit").val() == "true") {
                return isValid;
            } else {
                isValid = false;
                DialogStageGateStageResubmitMessage();
                /*var _html = document.createElement('div');
                _html.innerHTML = "This form has already been submitted on the dates shown below.  Resubmitting this form will record today as the date in which the Project was approved to move onto the next phase. Are you sure you’d like to Resubmit?<br />" + $("#hdnPrevSubmittedDate").val() + "<input type='submit' value='Yes' id='btnYes' onclick='ConfirmDialog(this);' class='ButtonControl' />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type='submit' value='No' onclick='ConfirmDialog(this);' id='btnNo' class='ButtonControl' />";
                var options = { html: _html, title: "", dialogReturnValueCallback: onPopUpCloseCallBack }
                SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);*/
            }
        } else {
            if ($("#hdnContinueSubmit").val() == "true") {
                return isValid;
            } else {
                //isValid = false;
                //var _html = document.createElement('div');
                //_html.innerHTML = "This Submit/Complete button will identify the Parent Project as Completed. This assumes all IPF/Child Projects are also in the Completed Status. If a Post Launch phase is required as indicated by the drop down within this form, the project will now be in Post Launch.<br /><input type='submit' value='Submit' id='btnYes' onclick='ConfirmDialog(this);' class='ButtonControl' />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type='submit' value='Cancel' onclick='ConfirmDialog(this);' id='btnNo' class='ButtonControl' />";
                //var options = { html: _html, title: "", dialogReturnValueCallback: onPopUpCloseCallBack }
                //SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
            }
        }
    }
    return isValid;
}
function confirmCancel() {
    $('#DialogStageCancelMessage').modal('show');
    /*var _html = document.createElement('div');
    _html.innerHTML = "This Cancel Project button will cause the Parent Project and all IPF/Child Projects will be Canceled. Please confirm you'd like to Cancel all projects associated with this Parent Project.<br /><input type='submit' value='Confirm' id='btnYes' onclick='ConfirmCancelDialog(this);' class='ButtonControl' />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<input type='submit' value='Cancel' onclick='ConfirmCancelDialog(this);' id='btnNo' class='ButtonControl' />";
    var options = { html: _html, title: "", dialogReturnValueCallback: onPopUpCloseCallBack }
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);*/
    return false;
}
function ConfirmDialog(clicked) {
    var clicked = $(clicked);
    var top = clicked.position().top;
    var left = clicked.position().left;
    var width = clicked.outerWidth();
    var height = clicked.outerHeight();
    clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");


    var clickedID = $(clicked).attr("id");

    if (clickedID == "btnYes") {
        $("#hdnPrevSubmittedDate").val("");
        if ($("#hdnContinueSubmit").length) {
            $("#hdnContinueSubmit").val("true");
        }
        $("#btnSubmit").click();
    } else {
        $('.ms-dlgCloseBtnImg').trigger('click');
        $(".disablingLoadingIcon").remove();
    }
}
function ConfirmCancelDialog(clicked) {
    var clickedID = $(clicked).attr("id");
    if (clickedID == "btnYes") {
        $("#hdnPrevSubmittedDate").val("");
        $("#btnCancel").click();
    } else {
        $('.ms-dlgCloseBtnImg').trigger('click');
        $(".disablingLoadingIcon").remove();
    }
}
function updatePostLaunch() {
    if ($("#ddlPostLaunch option:selected").text() == "Yes") {
        $("#postLaunchMessage").removeClass("hideItem");
    } else {
        $("#postLaunchMessage").addClass("hideItem");
    }
}
function projectStageChange() {
    if ($("#ddlProjectStage option:selected").val() == "1") {
        $("#lblNextGate").val("1");
        $("#nextGateDets").removeClass("hideItem");
    } else if ($("#ddlProjectStage option:selected").val() == "2") {
        $("#lblNextGate").val("2");
        $("#nextGateDets").removeClass("hideItem");
    } else if ($("#ddlProjectStage option:selected").val() == "3") {
        $("#lblNextGate").val("2");
        $("#nextGateDets").removeClass("hideItem");
    } else if ($("#ddlProjectStage option:selected").val() == "4") {
        $("#lblNextGate").val("3");
        $("#nextGateDets").removeClass("hideItem");
    } else if ($("#ddlProjectStage option:selected").val() == "5") {
        $("#nextGateDets").addClass("hideItem");
    } else if ($("#ddlProjectStage option:selected").val() == "6") {
        $("#nextGateDets").addClass("hideItem");
    } else {
        $("#nextGateDets").addClass("hideItem");
    }
}
function SGSPostbackFuntions() {
    updateStatus();
    updateTotals();
    projectStageChange();
    runTinyMC();
    updateHeader();
    $('.datePicker').datepicker({
        format: 'mm/dd/yyyy',
        autoclose: true
    });
}
function updateStatus() {
    $(".SGSApplicable").each(function () {
        var el = $(this);
        if (el.find("option:selected").val() == "N") {
            el.closest("tr").find(".SGSStatus").val("4");
            el.closest("tr").find(".SGSStatus").prop('disabled', 'disabled');
        } else {
            el.closest("tr").find(".SGSStatus").prop('disabled', false);
        }
    });
}
function updateTotals() {
    var url = window.location.href.toLocaleLowerCase();
    if (url.indexOf('stagegateprojectpanel.aspx') == -1) {
        var totalApplicable = 0;
        var totalCompleted = 0;
        $(".deliverablesTable").each(function () {
            var totalLocalApplicable = 0;
            var totalLocalCompleted = 0;
            $(this).find(".SGSApplicable").each(function () {
                var el = $(this);
                if (el.find("option:selected").val() == "Y") {
                    totalApplicable++;
                    totalLocalApplicable++;
                }
            });
            $(this).find(".SGSStatus").each(function () {
                var el = $(this);
                if (el.find("option:selected").val() == 1) {
                    totalCompleted++;
                    totalLocalCompleted++;
                }
            });
            $(this).find("#lblTotalApplicable").html(totalLocalApplicable);
            $(this).find("#lblTotalCompleted").html(totalLocalCompleted);
            $(this).next().find("#hdnDeliverablesTotalApplicable").val(totalLocalApplicable);
            $(this).next().find("#hdnDeliverablesTotalCompleted").val(totalLocalCompleted);
            var totalLocalAppl = parseFloat((parseFloat(totalLocalApplicable)));
            var totalLocalCompl = parseFloat(100 * totalLocalCompleted)
            var LocalPct = parseFloat(totalLocalCompl / totalLocalAppl);
            if (!(LocalPct > 0) || isNaN(LocalPct) || LocalPct == "Infinity") {
                LocalPct = 0;
            }
            $(this).closest(".row").prevAll(".row:first").find("#pchGatePct").html(LocalPct.toFixed(0));
        });
        var totalAppl = parseFloat((parseFloat(totalApplicable)));
        var totalCompl = 100 * parseFloat(totalCompleted)
        var pct = Math.round((totalCompl / totalAppl));
        if (!(pct > 0) || isNaN(pct) || pct == "Infinity") {
            pct = 0;
        }
        $("#hdnTotalDeliverablesTotalApplicable").val(totalAppl);
        $("#hdnTotalDeliverablesTotalCompleted").val(totalCompl);
        $("#pchReadinessPct").html(pct.toFixed(0));
    }
}
function deleteDelivRow(clicked) {
    $('#error_message').empty();
    var button = $(clicked);
    button.closest("tr").addClass("hideItem");
    button.closest("td").find("#hdnDeletedStatus").val("true");
    button.closest("td").find("#hdnChangeMade").val("true");
    button.closest("tr").find(".SGSApplicable").val("N");
    button.closest("tr").find(".SGSStatus").val("4");
    updateTotals();
}
function deleteRow(clicked, hdnDeletedStatus) {
    $('#error_message').empty();
    var button = $(clicked);
    button.closest("tr").addClass("hideItem");
    button.closest("td").find("#" + hdnDeletedStatus).val("true");
}
function AddProjectTeamMembersRow(btnId) {
    $('#' + btnId).click();
}
// Stage Gate Create Form Methods
function DialogStageGateProjectCreatedMessage() {
    $("#lblProjectCreatedMessage").text($("#hdnProjectNumber").val());
    $('#DialogStageGateProjectCreatedMessage').modal('show');
}
function DialogStageGateProjectResubmitMessage() {
    $("#lblProjectSubmittedDate").text($("#hdnProjectAlreadySubmittedDate").val());
    $('#DialogStageGateProjectResubmitMessage').modal('show');
}
function DialogStageGateStageResubmitMessage() {
    $("#lblProjectSubmittedDate").html($("#hdnPrevSubmittedDate").val());
    $('#DialogStageGateStageResubmitMessage').modal('show');
}
function DialogStageGateStageResubmitMessageCancel() {
    $('#DialogStageGateStageResubmitMessage').modal('hide');
    $(".disablingLoadingIcon").remove();
}
function DialogStageGateStageResubmitMessageOK() {
    $('#DialogStageGateProjectResubmitMessage').modal('hide');
    $('#hdnContinueSubmit').val("true");
    $("#btnSubmit").click();
}
function DialogStageGateProjectResubmitMessageOK() {
    $('#DialogStageGateProjectResubmitMessage').modal('hide');
    $('#hdnProjectAlreadySubmittedOK').val("Yes");
    $("#btnSubmit").click();
}
function DialogStageGateProjectResubmitMessageCancel() {
    $('#DialogStageGateProjectResubmitMessage').modal('hide');
    $(".disablingLoadingIcon").remove();
}
function StageGateProjectCreatedRedirect() {
    $('#DialogStageGateProjectCreatedMessage').modal('hide');
    $("#btnRedirect").click();
}
function DialogStageCancelProjectMessageOK() {
    $('#DialogStageCancelMessage').modal('hide');
    $("#hdnPrevSubmittedDate").val("");
    $("#btnCancel").click();
}
function DialogStageCancelProjectMessageCancel() {
    $('#DialogStageCancelMessage').modal('hide');
    $(".disablingLoadingIcon").remove();
}
// Stage Gate Project Complete 
function ShowDialogProjectCompletdMessage() {
    $('#DialogProjectCompletdMessage').modal('show');
}
function HideDialogProjectCompletdMessage() {
    $('#DialogProjectCompletdMessage').modal('hide');
}
function ParentProjectSubmit() {
    $('#DialogProjectCompletdMessage').modal('hide');
    $("#btnCompleteProject").click();
}
function UpdateRequiredAttributeForPeopleEditors(SimpleNetworkMove, RenovationAndQualityImprovement) {
    if (SimpleNetworkMove) {
        $('.SNWChangeRequiredOptional').each(function (i, obj) {
            var dvMain = $(this).closest("div.form-group");
            var span = dvMain.find('.markrequired');
            span.addClass('hide');
            $(this).removeClass('required');
        });

        //Project Team controls
        $('.SNWChangeRequiredOptionalPeopleEditor').each(function (i, obj) {
            $(this).addClass('hide');
        });

        $("[id^=hdnRequired]").each(function (i, obj) {
            $(this).val('False');
        });

        $('.UploadProjectBrief').addClass('hide');
        $('#hdnAddProjectBriefRequired').val("False");
    }
    else if (RenovationAndQualityImprovement) {
        $('.SNWChangeRequiredOptional').each(function (i, obj) {
            var dvMain = $(this).closest("div.form-group");
            var span = dvMain.find('.markrequired');
            span.removeClass('hide');
            $(this).addClass('required');
        });

        //Project Team controls
        $('.SNWChangeRequiredOptionalPeopleEditor').each(function (i, obj) {
            $(this).removeClass('hide');
        });

        $("[id^=hdnRequired]").each(function (i, obj) {
            $(this).val('True');
        });

        $('.RenQualWChangeRequiredOptionalPeopleEditor').each(function (i, obj) {
            $(this).addClass('hide');
        });

        $('#hdnRequiredExternalMfgProcurementMembers').val('False');
        $('#hdnRequiredPackagingProcurementMembers').val('False');
        $('#hdnRequiredLifeCycleManagementMembers').val('False');
        $('#hdnRequiredLegalMembers').val('False');

        $('.UploadProjectBrief').removeClass('hide');
        $('#hdnAddProjectBriefRequired').val("True");
    }
    else {
        $('.SNWChangeRequiredOptional').each(function (i, obj) {
            var dvMain = $(this).closest("div.form-group");
            var span = dvMain.find('.markrequired');
            span.removeClass('hide');
            $(this).addClass('required');
        });

        //Project Team controls
        $('.SNWChangeRequiredOptionalPeopleEditor').each(function (i, obj) {
            $(this).removeClass('hide');
        });

        $("[id^=hdnRequired]").each(function (i, obj) {
            $(this).val('True');
        });

        $('.UploadProjectBrief').removeClass('hide');
        $('#hdnAddProjectBriefRequired').val("True");
    }
}
function AddProjectTeamMembersRow_New(clicked, btnId) {
    var button = $(clicked);
    var MemberDiv = button.closest(".MemberDiv");
    var ddlMember = MemberDiv.find('.ddlMember');
    if (ddlMember != "-1") {
        $('#' + btnId).click();
    }
}
function openAttachment(docType, title) {
    var itemId = $("#hdnCompassListItemId").val();
    var projectNo = $("#hdnProjectNumber").val();

    if (projectNo == "") {
        $("#hdnUploadingDocuments").val(docType);
        $(".Save").click();
        var itemId = $("#hdnCompassListItemId").val();
        var projectNo = $("#hdnProjectNumber").val();
    }
    var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?ProjectNo=' + projectNo + '&DocType=' + docType + '&CompassItemId=' + itemId;
    openAttachmentsDialog(url, title);
    return false;
}
function openBriefAttachment(docType, title, gate, briefNo) {
    //$(".LoadAttachments").click();
    var PMTId = $("#hdnCompassListItemId").val();
    var projectNo = $("#hdnProjectNumber").val();
    var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?ProjectNo=' + projectNo + '&DocType=' + docType + '&CompassItemId=' + PMTId + "&Gate=" + gate + "&BriefNo=" + briefNo;
    openAttachmentsDialog(url, title);
    return false;
}
function openLoadPreviousBrief(gate, PMTListItemId, BriefCount, URLStage) {
    var projectNo = $("#hdnProjectNumber").val();
    var url = '/_layouts/15/Ferrara.Compass/AppPages/LoadBrief.aspx?ProjectNo=' + projectNo + '&Gate=' + gate + '&BriefCount=' + BriefCount + '&PMTListItemId=' + PMTListItemId + "&URLStage=" + URLStage;
    openAttachmentsDialog(url, "Load Brief from Previous Gate");
    return false;
}
function openAttachmentsDialog(tUrl, tTitle) {
    var y = $(window).scrollTop();
    var boxBorder = $(window).height() - $(".ms-dlgContent").height();
    y = y + boxBorder / 2;
    var options = {
        url: tUrl,
        title: tTitle,
        dialogReturnValueCallback: onPopUpAttachmentCallBack,
        y: y
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}
function onPopUpAttachmentCallBack(result, returnValue) {
    // $(".Save").click();

    $(".LoadAttachments").click();
}
function StageGateProjectTypeChange() {
    var ProjectTypeSelected = $("#ddlProjectType option:selected").text();
    var SimpleNetworkMove = false;
    var Multiple = false;
    var RenovationAndQualityImprovement = false;
    $("#ddlNewFinishedGood").prop('disabled', false);
    $("#ddlNewFinishedGood").prop('disabled', false);
    $("#ddlNewBaseFormula").prop('disabled', false);
    $("#ddlNewShape").prop('disabled', false);
    $("#ddlNewPackType").prop('disabled', false);
    $("#ddlNewNetWeight").prop('disabled', false);
    $("#ddlNewGraphics").prop('disabled', false);
    $("#ddlNewFlavorColor").prop('disabled', false);


    $("#ddlNewFinishedGood").val("-1");
    $("#ddlNewBaseFormula").val("-1");
    $("#ddlNewShape").val("-1");
    $("#ddlNewPackType").val("-1");
    $("#ddlNewNetWeight").val("-1");
    $("#ddlNewGraphics").val("-1");
    $("#ddlNewFlavorColor").val("-1");

    if (ProjectTypeSelected == "New Product & Process Development" || ProjectTypeSelected == "New Product and Process Development") {
        $("#ddlNewFinishedGood").val("Yes");
        $("#hdnddlNewFinishedGood").val("Yes");
        $("#ddlNewFinishedGood").prop('disabled', false);
        $("#ddlNewBaseFormula").val("Yes");
        $("#ddlNewShape").val("Yes");
        $("#ddlNewPackType").val("Yes");
        $("#ddlNewNetWeight").val("Yes");
        $("#ddlNewGraphics").val("Yes");
        $("#ddlNewFlavorColor").val("Yes");
    }
    else if (ProjectTypeSelected == "Packaging Development" || ProjectTypeSelected == "New Pack Type") {
        $("#ddlNewFinishedGood").val("Yes");
        $("#hdnddlNewFinishedGood").val("Yes");
        $("#ddlNewFinishedGood").prop('disabled', false);
        $("#ddlNewBaseFormula").val("No");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("Yes");
        $("#ddlNewNetWeight").val("Yes");
        $("#ddlNewGraphics").val("Yes");
        $("#ddlNewFlavorColor").val("No");
    }
    else if (ProjectTypeSelected == 'Line Extension') {
        $("#ddlNewFinishedGood").val("Yes");
        $("#hdnddlNewFinishedGood").val("Yes");
        $("#ddlNewFinishedGood").prop('disabled', false);
        $("#ddlNewBaseFormula").val("No");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("Yes");
        $("#ddlNewNetWeight").val("Yes");
        $("#ddlNewGraphics").val("Yes");
        $("#ddlNewFlavorColor").val("Yes");
    }
    else if (ProjectTypeSelected == 'Downweight/Transitions' || ProjectTypeSelected == 'Downweights/Transitions' || ProjectTypeSelected == 'Down-weights/Transitions') {
        $("#ddlNewFinishedGood").val("Yes");
        $("#hdnddlNewFinishedGood").val("Yes");
        $("#ddlNewFinishedGood").prop('disabled', false);
        $("#ddlNewBaseFormula").val("No");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("No");
        $("#ddlNewNetWeight").val("Yes");
        $("#ddlNewGraphics").val("Yes");
        $("#ddlNewFlavorColor").val("No");
    }
    else if (ProjectTypeSelected == "Renovation and Quality Improvement" || ProjectTypeSelected == "Renovation & Quality Improvement") {
        $("#ddlNewFinishedGood").val("No");
        $("#hdnddlNewFinishedGood").val("Yes");
        $("#ddlNewFinishedGood").prop('disabled', false);
        $("#ddlNewBaseFormula").val("Yes");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("No");
        $("#ddlNewNetWeight").val("No");
        $("#ddlNewGraphics").val("Yes");
        $("#ddlNewFlavorColor").val("Yes");
        RenovationAndQualityImprovement = true;
    }
    else if (ProjectTypeSelected == 'Graphics Change Only') {
        $("#ddlNewFinishedGood").val("No");
        $("#ddlNewBaseFormula").val("No");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("No");
        $("#ddlNewNetWeight").val("No");
        $("#ddlNewGraphics").val("Yes");
        $("#ddlNewFlavorColor").val("No");

        $("#hdnddlNewFinishedGood").val("No");
        $("#hdnddlNewBaseFormula").val("No");
        $("#hdnddlNewShape").val("No");
        $("#hdnddlNewPackType").val("No");
        $("#hdnddlNewNetWeight").val("No");
        $("#hdnddlNewGraphics").val("Yes");
        $("#hdnddlNewFlavorColor").val("No");

        $("#ddlNewFinishedGood").prop('disabled', 'disabled');
        $("#ddlNewBaseFormula").prop('disabled', 'disabled');
        $("#ddlNewShape").prop('disabled', 'disabled');
        $("#ddlNewPackType").prop('disabled', 'disabled');
        $("#ddlNewNetWeight").prop('disabled', 'disabled');
        $("#ddlNewGraphics").prop('disabled', 'disabled');
        $("#ddlNewFlavorColor").prop('disabled', 'disabled');
    }
    else if (ProjectTypeSelected == 'Simple Network Move') {
        $("#ddlNewFinishedGood").val("No");
        $("#ddlNewBaseFormula").val("No");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("No");
        $("#ddlNewNetWeight").val("No");
        $("#ddlNewGraphics").val("No");
        $("#ddlNewFlavorColor").val("No");

        $("#hdnddlNewFinishedGood").val("No");
        $("#hdnddlNewBaseFormula").val("No");
        $("#hdnddlNewShape").val("No");
        $("#hdnddlNewPackType").val("No");
        $("#hdnddlNewNetWeight").val("No");
        $("#hdnddlNewGraphics").val("No");
        $("#hdnddlNewFlavorColor").val("No");

        $("#ddlNewFinishedGood").prop('disabled', 'disabled');
        $("#ddlNewBaseFormula").prop('disabled', 'disabled');
        $("#ddlNewShape").prop('disabled', 'disabled');
        $("#ddlNewPackType").prop('disabled', 'disabled');
        $("#ddlNewNetWeight").prop('disabled', 'disabled');
        $("#ddlNewGraphics").prop('disabled', 'disabled');
        $("#ddlNewFlavorColor").prop('disabled', 'disabled');

        SimpleNetworkMove = true;
    }
    else if (ProjectTypeSelected == 'Multiple') {
        $("#ddlNewFinishedGood").val("-1");
        $("#hdnddlNewFinishedGood").val("-1");
        $("#ddlNewFinishedGood").prop('disabled', false);
        $("#ddlNewBaseFormula").val("-1");
        $("#ddlNewShape").val("-1");
        $("#ddlNewPackType").val("-1");
        $("#ddlNewNetWeight").val("-1");
        $("#ddlNewGraphics").val("-1");
        $("#ddlNewFlavorColor").val("-1");

        Multiple = true;
    }

    getStageGateProjectDescriptionType(ProjectTypeSelected);
    UpdateRequiredAttributeForPeopleEditors(SimpleNetworkMove, RenovationAndQualityImprovement);
    SetProjectTypeSubCategory(Multiple);
}
function ProjectTypeOnLoadChanges() {
    var ProjectTypeSelected = $("#ddlProjectType option:selected").text();
    var SimpleNetworkMove = false;
    var Multiple = false;
    var RenovationAndQualityImprovement = false;
    $("#ddlNewFinishedGood").prop('disabled', false);

    if (ProjectTypeSelected == 'Simple Network Move') {
        $("#ddlNewFinishedGood").val("No");
        $("#ddlNewBaseFormula").val("No");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("No");
        $("#ddlNewNetWeight").val("No");
        $("#ddlNewGraphics").val("No");
        $("#ddlNewFlavorColor").val("No");

        $("#hdnddlNewFinishedGood").val("No");
        $("#hdnddlNewBaseFormula").val("No");
        $("#hdnddlNewShape").val("No");
        $("#hdnddlNewPackType").val("No");
        $("#hdnddlNewNetWeight").val("No");
        $("#hdnddlNewGraphics").val("No");
        $("#hdnddlNewFlavorColor").val("No");

        $("#ddlNewFinishedGood").prop('disabled', 'disabled');
        $("#ddlNewBaseFormula").prop('disabled', 'disabled');
        $("#ddlNewShape").prop('disabled', 'disabled');
        $("#ddlNewPackType").prop('disabled', 'disabled');
        $("#ddlNewNetWeight").prop('disabled', 'disabled');
        $("#ddlNewGraphics").prop('disabled', 'disabled');
        $("#ddlNewFlavorColor").prop('disabled', 'disabled');

        SimpleNetworkMove = true;
    }
    else if (ProjectTypeSelected == 'Graphics Change Only') {

        $("#ddlNewFinishedGood").val("No");
        $("#ddlNewBaseFormula").val("No");
        $("#ddlNewShape").val("No");
        $("#ddlNewPackType").val("No");
        $("#ddlNewNetWeight").val("No");
        $("#ddlNewGraphics").val("Yes");
        $("#ddlNewFlavorColor").val("No");

        $("#hdnddlNewFinishedGood").val("No");
        $("#hdnddlNewBaseFormula").val("No");
        $("#hdnddlNewShape").val("No");
        $("#hdnddlNewPackType").val("No");
        $("#hdnddlNewNetWeight").val("No");
        $("#hdnddlNewGraphics").val("Yes");
        $("#hdnddlNewFlavorColor").val("No");

        $("#ddlNewFinishedGood").prop('disabled', 'disabled');
        $("#ddlNewBaseFormula").prop('disabled', 'disabled');
        $("#ddlNewShape").prop('disabled', 'disabled');
        $("#ddlNewPackType").prop('disabled', 'disabled');
        $("#ddlNewNetWeight").prop('disabled', 'disabled');
        $("#ddlNewGraphics").prop('disabled', 'disabled');
        $("#ddlNewFlavorColor").prop('disabled', 'disabled');
    }
    else if (ProjectTypeSelected == 'Multiple') {
        Multiple = true;
    }
    else if (ProjectTypeSelected == "Renovation and Quality Improvement" || ProjectTypeSelected == "Renovation & Quality Improvement") {
        RenovationAndQualityImprovement = true;
    }
    UpdateRequiredAttributeForPeopleEditors(SimpleNetworkMove, RenovationAndQualityImprovement);
    SetProjectTypeSubCategory(Multiple);
}
function SetProjectTypeSubCategory(Multiple) {
    var ddlProjectTypeSubCategory = $("#ddlProjectTypeSubCategory");

    if (Multiple && $("#ddlProjectTypeSubCategory").val() == "-1") {
        ddlProjectTypeSubCategory.val("5");
    }
}
function getStageGateProjectDescriptionType(projectTypeName) {
    projectTypeName = encodeURIComponent(projectTypeName);
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Stage Gate Project Types Lookup')/items?$select=ProjectDescription&$filter=(Title eq '" + projectTypeName + "')&$top=1",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var html, finding = results.d.results[0];
            if (results.d.results.length) {
                html = finding.ProjectDescription.replace(/&lt;/g, '<');
                html = html.replace(/&gt;/g, '>');
                $("#divProjectDescription").html(html);
                $("#labProjectDescription").text($("#ddlProjectType option:selected").text());
            }
            else {
                $("#labProjectDescription").text('Not selected');
                $("#divProjectDescription").html('');
            }
        },
        error: function (xhr, status, error) {
            $("#divProjectDescription").html('Error occurred: ' + error);
        }
    });
}
function GotoPeoplePickerStepAndFocus(arg) {
    var scrollTo = $("div[title='People Picker']").eq(arg);
    scrollTo.addClass("highlightElement");
    scrollTo.focus();
    $('html, body').animate({
        scrollTop: scrollTo.offset().top - 100
    }, 1000);
}
function addNewIPF() {
    var clicked = $("#btnAddIPF");
    var top = clicked.position().top;
    var left = clicked.position().left;
    var width = clicked.outerWidth();
    var height = clicked.outerHeight();
    var PMTListItemID = $("#hdnPMTListItemId").val();
    clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");

    projectDetails.push({
        "PMTListItemId": PMTListItemID, "CompassListItemId": "", "ProjectNumber": "", "FinishedGood": "", "LikeNumber": "", "Description": "", "UCC": "", "UPC": "", "ProductHierarchy1": "Select...", "ProductHierarchy2": "Select...", "BrandMaterialGroup1": "Select...", "ProductMaterialGroup4": "Select...", "PackTypeMaterialGroup5": "Select...", "ProjectStatus": "", "CreateIPFBtn": true, "NeedsNewBtn": false, "DeleteBtn": true, "Generated": false
    });
    GenerateIPFs();
    loadingIconAdded = true;
    return false;
}
function SaveIPFs() {
    var clicked = $("#btnSaveIPF");
    var top = clicked.position().top;
    var left = clicked.position().left;
    var width = clicked.outerWidth();
    var height = clicked.outerHeight();
    clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");

    var currentData = generateIPFData();
    var notes = $(this).text();
    var projectID = $(this).attr("id");
    $("#hdnAction").val("save");
    /*$.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/Pages/SGSGenerateIPF.aspx?Action=save&SGSSaveData=" + JSON.stringify(currentData),
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        complete: function () {
            $(".disablingLoadingIcon").remove();
            var timestamp = $.date($.now());
            $("#lblSavedMessage").html("Changes Saved: " + timestamp);
            return false;
        }
    });*/
    loadingIconAdded = true;
    return false;
}
function GenerateIPFs() {
    $.fn.dataTable.ext.search.push(
           function (settings, data, dataIndex) {
               var hidFilterType = $("#hidFilterType").val();
               var hidFilterValue = $("#hidFilterValue").val();

               if (hidFilterType == 'expandProcess') {
                   if (data[12] == 'Yes') {
                       return true;
                   }
                   else {
                       var filterVauesArray = [];
                       filterVauesArray = hidFilterValue.split(',');
                       var found = false;
                       $.each(filterVauesArray, function (i, val) {
                           if (data[13] == val) {
                               found = true;
                           }
                       });
                       return found;
                   }
               }
               else if (hidFilterType == 'SearchFilter') {
                   if (hidFilterValue == "") {
                       if (data[12] == 'No' && data[13] != 0) {
                           return false;
                       }
                       return true;

                   } else {
                       return true;
                   }
               }
               else if (hidFilterType == 'FooterFilter') {
                   if (hidFilterValue == "") {
                       if (data[12] == 'No' && data[13] != 0) {
                           return false;
                       }
                       return true;

                   } else {
                       return true;
                   }
               }
               else {
                   if (data[12] == 'No' && data[13] != 0) {
                       return false;
                   }
                   return true;
               }
           }
       );
    if (!$(".panel").length) {
        $("#ChildIPFs").dataTable().fnDestroy();
        var ChildIPFs = $('#ChildIPFs').DataTable({
            bPaginate: false
        }/*{
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        $("#hidFilterType").val('FooterFilter');
                        $("#hidFilterValue").val('SomethingSelected');

                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();

                        if (val == "") {
                            var atleastOneFilterPresent = false;
                            $("#ChildIPFs tfoot th select").each(function (i) {
                                var filterval = $.fn.dataTable.util.escapeRegex($(this).val());
                                if (filterval != "") {
                                    atleastOneFilterPresent = true;
                                }
                            });

                            if (atleastOneFilterPresent == false) {
                                $('[class^="Child_"]').not(".Child_0").each(function (i, obj) {
                                    $(this).addClass('hide');
                                    $("#hidFilterValue").val('');
                                    $("#hidFilterType").val('');
                                    myOpenProjects.draw();
                                });
                            }
                        }
                        else {
                            $('[class^="Child_"]').each(function (i, obj) {
                                $(this).removeClass('hide');
                            });
                        }
                    });
                column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                    var val = $('<div/>').html(d).text();
                    select.append('<option value="' + val + '">' + val + '</option>');
                });
            });
        }
    }*/
        );
    }
    $("#ChildIPFs tr .IPFGenerated").each(function () {
        var $row = $(this).closest("tr");
        $row.find(".PrelimField").each(function () {
            $(this).addClass("SGSRequired");
        });
    });
    $('.createIPF').on('click', function () {
        var notes = $(this).text();
        var projectID = $(this).attr("id");
        $("#hdnAction").val("generate");
    });
    $('.NeedsNewBtn').on('click', function () {
        var notes = $(this).text();
        var projectCompassID = $(this).closest("tr").find(".CompassListItemId").val();
        $("#hdnAction").val("workflow");
    });
    $("#ChildIPFs .ddlTBDIndicator").each(function () {
        updateRowStatus($(this));
    });
    checkRows();
}
function ValidateGenerateIPF(clicked) {
    var row = $(clicked).closest("tr");
    $('#error_message').empty();
    var isValid = true;
    var tbd = row.find(".ddlTBDIndicator");
    var TBDid = tbd.attr("id");
    var requiredTBDMsg = tbd.attr('tooltip') + ' is required';
    if (tbd.find("option:selected").val() == "-1") {
        isValid = false;
        $("#dverror_message").show();
        $('#error_message').append('<li class="errorMessage" >' + requiredTBDMsg + ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + TBDid + '"' + ') >Update</a>   </li></br>');
    } else if (tbd.find("option:selected").val() == "Y") {
        var popupValidated = row.find("#hdnPopupValidated");
        row.find(".PrelimField").each(function (i, obj) {
            var requiredMsg = $(this).attr('tooltip') + ' is required';
            var id = $(this).attr('id');
            if ($(this).is('input') || ($(this).is('textarea'))) {
                var value = $(this).val().trim();
                if (value == "") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            } else {
                var value = $(this).val();
                if (value == '-1') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else if (value == null && $(this).find("option").length <= 0) {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
            if (popupValidated.val() == "false") {
                var boxID = $(this).closest("tr").find(".itemDetails").attr("id");
                isValid = false;
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >Please complete IPF details.  <a style="color:darkblue" onclick=openIPFDets(' + '"' + boxID + '"' + ') >Update</a>   </li></br>');
            }
        });
    }
    if (isValid) {
        showWaitPopup('Creating IPF...', 'Please be patient, this may take a few minutes...');
    }
    return isValid;
}
function openIPFDets(id) {
    $("#" + id).click();
}
function updateSGSSAPDesc(clicked, txtBoxToChange) {
    var clicked = $(clicked);
    if (clicked.text().trim() != "") {
        $("#" + txtBoxToChange).val(clicked.text());
    }
}
function ChangeBrandChecks() {
    $("#ddlBrand").val($("#ddlBrand_Material option:selected").val());
}
function generateIPFData() {
    var IPFs = [];
    var i = 0;
    $("#ChildIPFs tr").each(function () {
        var $row = $(this);
        var IPF = [];
        var obj = {
        };
        var ProjectNumber = $row.find(".hdnProjectNumber").val();
        obj["ProjectNumber"] = ProjectNumber;
        IPF.push(obj);
        var TempProject = $row.find(".hdnTempProject").val();
        obj["ProjectNotes"] = TempProject;
        IPF.push(obj);
        var PMTProjectListItemId = $("#hdnPMTListItemId").val();
        obj["PMTProjectListItemId"] = PMTProjectListItemId;
        IPF.push(obj);
        $row.find("textarea").each(function () {
            var $this, textBoxes, text;
            textBoxes = $(this);

            text = textBoxes.text();
            obj[textBoxes.attr("class")] = text;
            IPF.push(obj);
        });
        $row.find("select").each(function () {
            var $this, text, dropdowns;
            dropdowns = $(this);

            text = dropdowns.find("option:selected").text();
            obj[dropdowns.attr("class")] = text;
            IPF.push(obj);
        });
        IPFs[i] = obj;
        console.log(obj);
        i++;
    });
    return IPFs;
}
function deleteIPF(clicked) {
    var notes = $(clicked).text();
    var projectID = $(clicked).attr("id");
    var projectNotes = $(clicked).closest("tr").find(".projectName a").text();
    var currentData = generateIPFData();
    $("#hdnAction").val("delete");
    /*$.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/Pages/SGSGenerateIPF.aspx?Action=delete&ProjectNo=" + projectID + "&SGSSaveData=" + JSON.stringify(currentData),
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        complete: function () {
            $("#ChildIPFs").DataTable().row($(clicked).parents('tr')).remove().draw();

            return false;
        }
    });*/
}
$.date = function (dateObject) {
    var d = new Date(dateObject);
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();
    if (day < 10) {
        day = "0" + day;
    }
    if (month < 10) {
        month = "0" + month;
    }
    var time = d.getHours() + ":" + d.getMinutes() + ":" + d.getSeconds();
    var date = day + "/" + month + "/" + year + " " + time + " ";

    return date;
};
//Finance Brief Form Methods
function DialogLoadFinanceBrieffromPreviousGate() {
    $('#DialogLoadFinanceBrieffromPreviousGate').modal('show');
}
function DialogStageGateFinanciaBriefsCopiedMessage() {
    $('#DialogLoadFinanceBrieffromPreviousGate').modal('hide');
    $('#DialogStageGateFinanceCopiedMessage').modal('show');
}
function StageGateFinanceCopiedRedirect() {
    $('#DialogStageGateProjectCreatedMessage').modal('hide');
    $("#btnRedirect").click();
}
function GetFinanceBrieffromPreviousGateCancel() {
    $('#DialogLoadFinanceBrieffromPreviousGate').modal('hide');
    $("#btnRedirect").click();
}
function ValdateFinanceBrieffromPreviousGateCancel() {
    var atleastOneChecked = false;
    $('.selectFinanceBrief').each(function (i, obj) {
        var chk = $(this).find('#chkFinanceBrief');
        if (chk.prop('checked')) {
            atleastOneChecked = true;
        };
    });

    if (!atleastOneChecked) {
        $('#lblFinanceBrieffromPreviousGateValidationMsg').text("Please select atleast one Brief.");
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
    }

    return atleastOneChecked;
}
function deleteFinancialBriefAnalysisItem(clicked) {
    var button = $(clicked);
    var parentFinancialBriefrow = $(clicked).closest(".FinancialBriefrow");
    var hdnAnalysisListItemId = parentFinancialBriefrow.find("#hdnAnalysisListItemId").val();

    loadingIconAdded = true;
    parentFinancialBriefrow.find(".disablingLoadingIcon").remove();
    parentFinancialBriefrow.find('#hdnDeletedStatus').val("deleted");
    parentFinancialBriefrow.find("#ddlIncludePLInConsolidatedFinancials").val("No");
    CalculateFinancials();
    UpdateIncludePLInConsolidatedFinancials();
    parentFinancialBriefrow.addClass("hideItem");
    //return true;
}
function CalculateFinancials() {
    CalculateFinancialAnalysis();
    CalculateFinancialSummary();
    UpdateIncludePLInConsolidatedFinancials();
    UpdateAllSigns();
}
function hideSelectButton(arg) {
    $('.' + arg).each(function (i, obj) {
        $(this).addClass("hideItem");
    });
}
function CalculateFinancialSummary() {
    var Volume;
    var GrossSales;
    var COGS;
    var AvgTargetMarginPct;
    //Calc
    var NetSales;
    var GrossMargins;
    var GrossMarginPct;
    var NSPerLb;
    var COGSerLb;
    var count = 0;

    var TotalOrIncrementals = ["Total", "Incremental"]
    var Years = ["1", "2", "3"]
    var NumberOfAnalysis;

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            Volume = 0;
            GrossSales = 0;
            COGS = 0;
            AvgTargetMarginPct = 0;
            NetSales = 0;
            GrossMargins = 0;
            GrossMarginPct = 0;
            NSPerLb = 0;
            COGSerLb = 0;
            count = 0;

            $('.FinancialBriefrow').each(function (i, obj) {
                if ($(this).find('#ddlIncludePLInConsolidatedFinancials').children("option:selected").val() == "Yes") {
                    count++;
                    FinancialBriefrow = $(this);
                    var VolumeValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "Volume" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
                    var GrossSalesValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossSales" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
                    var NetSalesValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "NetSales" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
                    var COGSValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "COGS" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
                    var AvgTargetMarginPctValue = FinancialBriefrow.find('#txtTargetMarginPct').val().replace(/,/g, '').replace("$", "").replace("%", "");

                    Volume = Volume + ($.isNumeric(VolumeValue) ? parseFloat(VolumeValue) : 0);
                    GrossSales = GrossSales + ($.isNumeric(GrossSalesValue) ? parseFloat(GrossSalesValue) : 0);
                    NetSales = NetSales + ($.isNumeric(NetSalesValue) ? parseFloat(NetSalesValue) : 0);
                    COGS = COGS + ($.isNumeric(COGSValue) ? parseFloat(COGSValue) : 0);
                    AvgTargetMarginPct = AvgTargetMarginPct + ($.isNumeric(AvgTargetMarginPctValue) ? parseFloat(AvgTargetMarginPctValue) : 0);
                }
            });

            GrossMargins = CheckForNan(NetSales) - CheckForNan(COGS);

            if (CheckForNan(NetSales) == 0) {
                GrossMarginPct = 0;
            }
            else {
                GrossMarginPct = (CheckForNan(GrossMargins) / NetSales) * 100;
            }

            if (CheckForNan(Volume) == 0) {
                NSPerLb = 0;
                COGSerLb = 0;
            }
            else {
                NSPerLb = (CheckForNan(NetSales) / Volume);
                COGSerLb = (CheckForNan(COGS) / Volume);
            }

            $('.FinancialSummaryrow').find('#txt' + TotalOrIncremental + "Volume" + Year).val(numberWithCommas((CheckForNan(Volume)).toFixed(2)));
            $('.FinancialSummaryrow').find('#txt' + TotalOrIncremental + "GrossSales" + Year).val('$' + numberWithCommas((CheckForNan(GrossSales)).toFixed(2)));
            $('.FinancialSummaryrow').find('#txt' + TotalOrIncremental + "COGS" + Year).val('$' + numberWithCommas((CheckForNan(COGS)).toFixed(2)));
            $('.FinancialSummaryrow').find('#txt' + TotalOrIncremental + "NetSales" + Year).val('$' + numberWithCommas((CheckForNan(NetSales)).toFixed(2)));
            $('.FinancialSummaryrow').find('#txt' + TotalOrIncremental + "GrossMargin" + Year).val('$' + numberWithCommas((CheckForNan(GrossMargins)).toFixed(2)));
            $('.FinancialSummaryrow').find('#txt' + TotalOrIncremental + "GrossMarginPct" + Year).val(numberWithCommas((CheckForNan(GrossMarginPct)).toFixed(2)) + '%');
            if (TotalOrIncremental == "Total") {
                $('.FinancialSummaryrow').find('#txtNSPerLB' + Year).val('$' + CheckForNan(NSPerLb).toFixed(2));
                $('.FinancialSummaryrow').find('#txtCOGSPerLB' + Year).val('$' + CheckForNan(COGSerLb).toFixed(2));
            }
        });
    });
}
function CalculateFinancialAnalysis() {
    $('.FinancialBriefrow').each(function (i, obj) {
        CalculateNetSalesAnalysis($(this));
        CalculateGrossMarginsAnalysis($(this));
        CalculateGrossMarginsPCTAnalysis($(this));
        CalculateNSPerLbAnalysis($(this));
        CalculateCOGSerLbAnalysis($(this));
    });
}
function CalculateNetSalesAnalysis(FinancialBriefrow) {
    var TotalOrIncrementals = ["Total", "Incremental"]
    var Years = ["1", "2", "3"]
    var TotalGrossSales;
    var TotalTradeRate;
    var TotalOGTN;
    var TotalNetSales;

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            var TotalGrossSalesValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossSales" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            var TotalTradeRateValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "TradeRate" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            var TotalOGTNValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "OGTN" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");

            TotalGrossSales = $.isNumeric(TotalGrossSalesValue) ? parseFloat(TotalGrossSalesValue) : 0;
            TotalTradeRate = $.isNumeric(TotalTradeRateValue) ? parseFloat(TotalTradeRateValue) : 0;
            TotalOGTN = $.isNumeric(TotalOGTNValue) ? parseFloat(TotalOGTNValue) : 0;
            TotalNetSales = CheckForNan(TotalGrossSales) - (CheckForNan(TotalGrossSales) * CheckForNan(TotalTradeRate / 100)) - (CheckForNan(TotalGrossSales) * CheckForNan(TotalOGTN / 100));
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "NetSales" + Year).val('$' + numberWithCommas((CheckForNan(TotalNetSales)).toFixed(2)));
        });
    });
}
function CalculateGrossMarginsAnalysis(FinancialBriefrow) {
    var TotalOrIncrementals = ["Total", "Incremental"]
    var Years = ["1", "2", "3"]
    var TotalNetSales;
    var TotalCOGS;
    var GrossMargins;

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            var TotalNetSalesValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "NetSales" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            var TotalCOGSValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "COGS" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            TotalNetSales = $.isNumeric(TotalNetSalesValue) ? parseFloat(TotalNetSalesValue) : 0;
            TotalCOGS = $.isNumeric(TotalCOGSValue) ? parseFloat(TotalCOGSValue) : 0;
            GrossMargins = CheckForNan(TotalNetSales) - CheckForNan(TotalCOGS);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossMargin" + Year).val('$' + numberWithCommas((CheckForNan(GrossMargins)).toFixed(2)));
        });
    });
}
function CalculateGrossMarginsPCTAnalysis(FinancialBriefrow) {
    var TotalOrIncrementals = ["Total", "Incremental"]
    var Years = ["1", "2", "3"]
    var GrossMargins;
    var TotalNetSales;
    var GrossMarginPct;

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            var TotalNetSalesValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "NetSales" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            var GrossMarginsValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossMargin" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            TotalNetSales = $.isNumeric(TotalNetSalesValue) ? parseFloat(TotalNetSalesValue) : 0;
            GrossMargins = $.isNumeric(GrossMarginsValue) ? parseFloat(GrossMarginsValue) : 0;

            if (CheckForNan(TotalNetSales) == 0) {
                GrossMarginPct = 0;
            }
            else {
                GrossMarginPct = (CheckForNan(GrossMargins) / TotalNetSales) * 100;
            }

            FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossMarginPct" + Year).val((GrossMarginPct).toFixed(2) + '%');
        });
    });
}
function CalculateNSPerLbAnalysis(FinancialBriefrow) {
    var TotalOrIncrementals = ["Total"]
    var Years = ["1", "2", "3"]
    var TotalNetSales;
    var TotalVolume;
    var NSPerLb;

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            var TotalNetSalesValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "NetSales" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            var TotalVolumeValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "Volume" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            TotalNetSales = $.isNumeric(TotalNetSalesValue) ? parseFloat(TotalNetSalesValue) : 0;
            TotalVolume = $.isNumeric(TotalVolumeValue) ? parseFloat(TotalVolumeValue) : 0;

            if (CheckForNan(TotalVolume) > 0) {
                NSPerLb = (CheckForNan(TotalNetSales) / CheckForNan(TotalVolume));
                FinancialBriefrow.find('#txtNSPerLB' + Year).val('$' + CheckForNan((NSPerLb)).toFixed(2));
            }
        });
    });
}
function CalculateCOGSerLbAnalysis(FinancialBriefrow) {
    var TotalOrIncrementals = ["Total"]
    var Years = ["1", "2", "3"]
    var TotalCOGS;
    var TotalVolume;
    var COGSerLb;

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            var TotalCOGSValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "COGS" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            var TotalVolumeValue = FinancialBriefrow.find('#txt' + TotalOrIncremental + "Volume" + Year).val().replace(/,/g, '').replace("$", "").replace("%", "");
            TotalCOGS = $.isNumeric(TotalCOGSValue) ? parseFloat(TotalCOGSValue) : 0;
            TotalVolume = $.isNumeric(TotalVolumeValue) ? parseFloat(TotalVolumeValue) : 0;
            if (CheckForNan(TotalVolume) > 0) {
                COGSerLb = (CheckForNan(TotalCOGS) / CheckForNan(TotalVolume));
                FinancialBriefrow.find('#txtCOGSPerLBValue' + Year).val('$' + (CheckForNan(COGSerLb)).toFixed(2));
            }
        });
    });
}
function UpdateIncludePLInConsolidatedFinancials() {
    var ConsolidatedFinancials = "";
    $('.IncludePLInConsolidatedFinancials').each(function (i, obj) {
        if ($(this).children("option:selected").val() == "Yes") {
            dvMain = $(this).closest(".FinancialBriefrow");
            if (ConsolidatedFinancials == "" || ConsolidatedFinancials == undefined) {
                ConsolidatedFinancials = dvMain.find('#txtAnalysisName').val();
            } else {
                ConsolidatedFinancials = ConsolidatedFinancials + ", " + dvMain.find('#txtAnalysisName').val();
            }
        }
    });
    $('#lblAnalysesIncludedInSummary').text(ConsolidatedFinancials);
    $("#hdnAnalysesIncludedInSummary").val(ConsolidatedFinancials);
}
$(document.body).on("blur", ".CalculateFinancials", function () {
    CalculateFinancials();
});
$(document.body).on("change", ".IncludePLInConsolidatedFinancials", function () {
    CalculateFinancials();
});
$(document.body).on("blur", ".AnalysisName", function () {
    CalculateFinancials();
});
$(document.body).on("blur", ".NumberWithComma", function (event) {
    $(this).val(function (index, value) {
        value = value
                .replace(/,/g, '')
                .replace("$", "")
                .replace("%", "");
        value = CheckForNan(value);
        value = value * 1;

        if ($(this).hasClass('SatgeGaePct')) {
            return value + '%';
        }
        else if ($(this).hasClass('volume')) {
            return numberWithCommas(value);
        }
        else if ($(this).hasClass('NumberWithComma')) {
            value = parseFloat(value).toFixed(2);
            return '$' + numberWithCommas(value);
        }
    });
});
$(document.body).on("keypress", ".NumberWithComma", function (evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);

    if (key.length == 0) {
        return;
    }

    var regex = /^\-*\d*(\.\d*)?$/;

    if ($(this).hasClass('volume')) {
        regex = /^[0-9.,\b]+$/;
    }

    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }

    if (!$(this).hasClass('volume')) {
        var position = evt.target.selectionStart;
        var value = $(this).val();
        var Latestvalues = [value.slice(0, position), key, value.slice(position)].join('');

        Latestvalues = $.trim(Latestvalues)
                        .replace("$", '')
                        .replace("%", '')
                        .replace(/,/g, '');

        if (!regex.test(Latestvalues)) {
            theEvent.returnValue = false;
            if (theEvent.preventDefault) theEvent.preventDefault();
        }
    }
});
$(document.body).on("click", ".FinancialBriefAnalysisHeader", function (e) {
    if (e.target !== this)
        return;

    var FinancialBriefrow = $(this).closest('.FinancialBriefrow');
    var dvFinanceAnalysis = FinancialBriefrow.find(".FinancialBriefAnalysisBody");
    if (dvFinanceAnalysis.hasClass('hide')) {
        dvFinanceAnalysis.removeClass('hide');
    }
    else {
        dvFinanceAnalysis.addClass('hide');
    }
});
function numberWithCommas(x) {
    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}
function setFocusFinanceElement(sender, FinancialBriefrowId) {
    dvMain = $("#" + FinancialBriefrowId);
    dvMain.find("#" + sender).addClass('highlightElement');
    dvMain.find("#" + sender).focus();
    $('html, body').animate({
        scrollTop: dvMain.find("#" + sender).offset().top - 100
    }, 1000);
}
function UpdateAllSigns() {
    $('.stageGateReadOnly').each(function (i, obj) {
        $(this).prop("readonly", true);
    });

    $('.stageGateDropDownReadOnly').each(function (i, obj) {
        $(this).attr("readonly", true);
    });

    $('.NumberWithComma').each(function (i, obj) {
        var valueExisting = $(this).val();
        //valueExisting = CheckForNan(valueExisting);
        //valueExisting = parseFloat(valueExisting).toFixed(2);
        $(this).val(numberWithCommas(valueExisting));
    });

    $('.SatgeGaePct').each(function (i, obj) {
        var value = $.trim($(this).val());
        if (value != '') {
            $(this).val(value.replace('%', "") + '%');
        }
    });

    $('.NumberWithComma').not('.SatgeGaePct').not('.volume').each(function (i, obj) {
        var value = $.trim($(this).val());
        if (value != '') {
            value = value.replace('$', "");
            $(this).val('$' + value.replace('$', ""));
        }
    });
}

$(document.body).on("click", ".CopyNumbers1", function (e) {
    if (e.target !== this)
        return;

    var TotalOrIncrementals = ["Total", "Incremental"]
    var Years = ["2"]
    var Year1 = "1";
    var FinancialBriefrow = $(this).closest('.FinancialBriefrow');

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            var Volume = FinancialBriefrow.find('#txt' + TotalOrIncremental + "Volume" + Year1).val();
            var GrossSales = FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossSales" + Year1).val();
            var TradeRate = FinancialBriefrow.find('#txt' + TotalOrIncremental + "TradeRate" + Year1).val();
            var OGTN = FinancialBriefrow.find('#txt' + TotalOrIncremental + "OGTN" + Year1).val();
            var COGS = FinancialBriefrow.find('#txt' + TotalOrIncremental + "COGS" + Year1).val();

            FinancialBriefrow.find('#txt' + TotalOrIncremental + "Volume" + Year).val(Volume);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossSales" + Year).val(GrossSales);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "TradeRate" + Year).val(TradeRate);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "OGTN" + Year).val(OGTN);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "COGS" + Year).val(COGS);

        });
    });
    CalculateFinancials();
});

$(document.body).on("click", ".CopyNumbers2", function (e) {
    if (e.target !== this)
        return;

    var TotalOrIncrementals = ["Total", "Incremental"]
    var Years = ["3"]
    var Year2 = "2";
    var FinancialBriefrow = $(this).closest('.FinancialBriefrow');

    $.each(Years, function (index, Year) {
        $.each(TotalOrIncrementals, function (index, TotalOrIncremental) {
            var Volume = FinancialBriefrow.find('#txt' + TotalOrIncremental + "Volume" + Year2).val();
            var GrossSales = FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossSales" + Year2).val();
            var TradeRate = FinancialBriefrow.find('#txt' + TotalOrIncremental + "TradeRate" + Year2).val();
            var OGTN = FinancialBriefrow.find('#txt' + TotalOrIncremental + "OGTN" + Year2).val();
            var COGS = FinancialBriefrow.find('#txt' + TotalOrIncremental + "COGS" + Year2).val();

            FinancialBriefrow.find('#txt' + TotalOrIncremental + "Volume" + Year).val(Volume);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "GrossSales" + Year).val(GrossSales);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "TradeRate" + Year).val(TradeRate);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "OGTN" + Year).val(OGTN);
            FinancialBriefrow.find('#txt' + TotalOrIncremental + "COGS" + Year).val(COGS);
        });
    });

    CalculateFinancials();
});

function checkRows() {
    $("#imgStatus").each(function () {
        var row = $(this).closest("tr").find(".createIPF");
        if ($(this).hasClass("red")) {
            row.addClass("hideItem");
        } else {
            row.removeClass("hideItem");
        }
    });
}
function updateRowStatus(changed) {
    var isValid = true;

    var row = $(changed).closest("tr");
    var hdnIPFGenerated = row.find("#hdnIPFGenerated");
    if (hdnIPFGenerated.val() == "true") {
        var tbd = row.find(".ddlTBDIndicator");
        var TBDid = tbd.attr("id");
        var requiredTBDMsg = tbd.tooltip() + ' is required';
        if (tbd.find("option:selected").val() == "-1") {
            isValid = false;
        } else if (tbd.find("option:selected").val() == "Y") {
            var popupValidated = row.find("#hdnPopupValidated");
            row.find(".PrelimField").each(function (i, obj) {
                if ($(this).is('input') || ($(this).is('textarea'))) {
                    var value = $(this).val().trim();
                    if (value == "") {
                        isValid = false;
                        $(this).addClass("highlightElement");
                    }
                    else {
                        $(this).removeClass("highlightElement");
                    }
                } else {
                    var value = $(this).val();
                    if (value == '-1') {
                        isValid = false;
                        $(this).addClass("highlightElement");
                    } else if (value == null && $(this).find("option").length <= 0) {
                        isValid = false;
                        $(this).addClass("highlightElement");
                    } else {
                        $(this).removeClass("highlightElement");
                    }
                }
            });
            if (popupValidated.val() == "false") {
                isValid = false;
            }
        } else {
            $(".highlightElement").removeClass("highlightElement");
        }
    }
    var imgStatus = row.find("#imgStatus");
    var generateIPF = row.find(".createIPF");
    if (isValid) {
        imgStatus.attr("src", "/_layouts/15/Ferrara.Compass/img/greenCircle.png");
        imgStatus.attr("class", "green");
        generateIPF.removeClass("hideItem");
    } else {
        imgStatus.attr("src", "/_layouts/15/Ferrara.Compass/img/redCircle.png");
        imgStatus.attr("class", "red");
        generateIPF.addClass("hideItem");
    }
}
function CheckForNan(Value) {
    if (isNaN(Value) || Value.length == 0) {
        return 0;
    }
    return Value;
}
function SGSConditionalChecks() {
    var previousValue = $("#ddlCustomerSpecific").data("ddlvalue");
    // setting the new previous value
    var theValue = $("#ddlCustomerSpecific").val();
    $("#ddlCustomerSpecific").data("ddlvalue", theValue);
    if ($("#ddlCustomerSpecific").val() == 'CU') {
        if (previousValue == 'N')
            $("#ddlCustomer").val("-1");
        showItem('dvCustomer');
        showItem('dvCustomerSpecific');
        hideItem('dvChannel');
        $("#ddlChannel").val("-1");;
    } else if ($("#ddlCustomerSpecific").val() == 'CH') {
        showItem('dvChannel');
        hideItem('dvCustomer');
        $("#ddlCustomer").val("-1");
        hideItem('dvCustomerSpecific');
        $("#txtCustomerSpecificLotCode").val("");
    } else if ($("#ddlCustomerSpecific").val() == 'N') {
        showItem('dvCustomer');
        if ($("#ddlCustomer").val() == '-1' || (previousValue == 'CU'))
            $('#ddlCustomer option:contains("PRICELIST")').prop('selected', true);
        hideItem('dvCustomerSpecific');
        hideItem('dvChannel');
        $("#ddlChannel").val("-1");
    } else {
        hideItem('dvCustomer');
        $("#ddlCustomer").val("-1");
        hideItem('dvCustomerSpecific');
        $("#txtCustomerSpecificLotCode").val("");
        hideItem('dvChannel');
        $("#ddlChannel").val("-1");;
    }
    if ($("#ddlNeedNewUPCUCC option:selected").text() == 'Yes') {
        showItem('dvddlNeedNewUPC');
        showItem('dvNeedNewDisplayBoxUPC');
        showItem('dvSAPBaseUOM');
        if ($("#dvddlNeedNewUPC option:selected").text() == 'No') {
            showItem('dvUnitUPC');
        } else {
            hideItem('dvUnitUPC');
        }
        if ($("#ddlNeedNewDisplayBoxUPC option:selected").text() == 'No') {
            showItem('dvDisplayBoxUPC');
        } else {
            hideItem('dvDisplayBoxUPC');
        }
        if ($("#ddlSAPBUOM option:selected").text() == "PAL") {
            showItem('dvNeedNewPalletUCC');
            hideItem('dvNeedNewCaseUCC');
            $("#ddlNeedNewCaseUCC").val("-1");
            hideItem('dvCaseUCC');
            if ($("#ddlNeedNewPalletUCC option:selected").text() == "Yes") {
                hideItem('dvPalletUCC');
            } else if ($("#ddlNeedNewPalletUCC option:selected").text() == "No") {
                showItem('dvPalletUCC');
            } else {
                hideItem('dvPalletUCC');
            }
        } else if ($("#ddlSAPBUOM option:selected").text() == "CS") {
            showItem('dvNeedNewCaseUCC');
            hideItem('dvNeedNewPalletUCC');
            $("#ddlNeedNewPalletUCC").val("-1");
            hideItem('dvPalletUCC');
            if ($("#ddlNeedNewCaseUCC option:selected").text() == "Yes") {
                hideItem('dvCaseUCC');
            } else if ($("#ddlNeedNewCaseUCC option:selected").text() == "No") {
                showItem('dvCaseUCC');
            } else {
                hideItem('dvCaseUCC');
            }
        } else {
            hideItem('dvPalletUCC');
            hideItem('dvNeedNewPalletUCC');
            $("#ddlNeedNewPalletUCC").val("-1");
            hideItem('dvNeedNewCaseUCC');
            $("#ddlNeedNewCaseUCC").val("-1");
            hideItem('dvCaseUCC');
        }
    } else if ($("#ddlNeedNewUPCUCC option:selected").text() == 'No') {
        showItem('dvUnitUPC');
        showItem('dvDisplayBoxUPC');
        showItem('dvSAPBaseUOM');
        hideItem('dvddlNeedNewUPC');
        $("#ddlNeedNewUnitUPC").val("-1");
        hideItem('dvNeedNewDisplayBoxUPC');
        $("#ddlNeedNewDisplayBoxUPC").val("-1");
        hideItem('dvNeedNewCaseUCC');
        $("#ddlNeedNewCaseUCC").val("-1");
        hideItem('dvNeedNewPalletUCC');
        $("#ddlNeedNewPalletUCC").val("-1");
        if ($("#ddlSAPBUOM option:selected").text() == "PAL") {
            showItem('dvPalletUCC');
            hideItem('dvCaseUCC');
        } else if ($("#ddlSAPBUOM option:selected").text() == "CS") {
            showItem('dvCaseUCC');
            hideItem('dvPalletUCC');
        } else {
            hideItem('dvPalletUCC');
            hideItem('dvCaseUCC');
        }
    } else {
        hideItem('dvddlNeedNewUPC');
        $("#ddlNeedNewUnitUPC").val("-1");
        hideItem('dvUnitUPC');
        hideItem('dvNeedNewDisplayBoxUPC');
        $("#ddlNeedNewDisplayBoxUPC").val("-1");
        hideItem('dvDisplayBoxUPC');
        hideItem('dvSAPBaseUOM');
        $("#ddlSAPBUOM").val("-1");
        hideItem('dvNeedNewCaseUCC');
        $("#ddlNeedNewCaseUCC").val("-1");
        hideItem('dvCaseUCC');
        hideItem('dvNeedNewPalletUCC');
        $("#ddlNeedNewPalletUCC").val("-1");
        hideItem('dvPalletUCC');
    }
    if ($("#ddlTBDIndicator").val() == "Y") {
        $(".markrequired").removeClass("hideItem");
        if ($("#txtSAPItemNumber").val() == "") {
            $("#txtSAPItemNumber").val("NEEDS NEW");
        }
    } else {
        $(".markrequired").addClass("hideItem")
        if ($("#txtSAPItemNumber").val().toLocaleLowerCase() == "needs new") {
            $("#txtSAPItemNumber").val("");
        }
    }
    $("#ddlTBDIndicator").closest(".form-group").find(".markrequired").removeClass("hideItem");
    //LTO

    //Remove required
    hideItem('divRequestChangeToFGNumForSameUCC');
    $("#ddlRequestChangeToFGNumForSameUCC").closest(".form-group").find(".markrequired").remove();
    $("#ddlRequestChangeToFGNumForSameUCC").removeClass('required');

    hideItem('dvLTO');
    $("#txtLTOTransitionStartWindowRDD").closest(".form-group").find(".markrequired").remove();
    $("#txtLTOTransitionStartWindowRDD").removeClass('required');

    hideItem('divLTOTransitionStartWindowRDD');
    $("#txtLTOTransitionEndWindowRDD").closest(".form-group").find(".markrequired").remove();
    $("#txtLTOTransitionEndWindowRDD").removeClass('required');

    hideItem('divLTOEndDateFlexibility');
    $("#ddlLTOEndDateFlexibility").closest(".form-group").find(".markrequired").remove();
    $("#ddlLTOEndDateFlexibility").removeClass('required');

    if ($("#hdnProjectType").val() != "Simple Network Move") {
        showItem('dvLTO');
        if ($("#ddlFGReplacingAnExistingFG").val() == "Yes") {
            showItem('divRequestChangeToFGNumForSameUCC');
            $("#ddlRequestChangeToFGNumForSameUCC").addClass('required');
            $("#ddlRequestChangeToFGNumForSameUCC").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }

        if ($("#ddlIsThisAnLTOItem").val() == "Yes" || $("#ddlFGReplacingAnExistingFG").val() == "Yes") {
            showItem('divLTOTransitionStartWindowRDD');
            $("#txtLTOTransitionStartWindowRDD").addClass('required');
            $("#txtLTOTransitionStartWindowRDD").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

            showItem('divLTOTransitionEndWindowRDD');
            $("#txtLTOTransitionEndWindowRDD").addClass('required');
            $("#txtLTOTransitionEndWindowRDD").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));

            if ($("#ddlIsThisAnLTOItem").val() != "Yes") {
                if ($("#txtLTOTransitionEndWindowRDD").val() == "") {
                    $("#txtLTOTransitionEndWindowRDD").val("12/31/9999");
                }
            } else {
                if ($("#txtLTOTransitionEndWindowRDD").val("12/31/9999")) {
                    $("#txtLTOTransitionEndWindowRDD").val("");
                }
            }
        }

        if ($("#ddlIsThisAnLTOItem").val() == "Yes") {
            showItem('divLTOEndDateFlexibility');
            $("#ddlLTOEndDateFlexibility").addClass('required');
            $("#ddlLTOEndDateFlexibility").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }
    } else {
        hideItem('dvLTO');
    }
}
function GetProjectSearch() {
    var url = "/_layouts/15/Ferrara.Compass/AppPages/CopyIPF.aspx?PMTListItemId=" + $("#hdnPMTListItemId").val() + "&ProjectNo=" + $("#hdnParentProjectNumber").val() + "&CopyMode=CopyExistingIPF";
    var options = {
        url: url,
        title: "Copy Existing IPF",
        dialogReturnValueCallback: onPopUpCloseCallBack,
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    return false;
}
function UpdateImage(imageId) {
    $("#" + imageId).attr("src", "/_layouts/15/Ferrara.Compass/img/Requested.png");
    $("#" + imageId).attr("onclick", "return false;");
    $("#" + imageId).attr("disabled", "disabled");
}
function updateHeader() {
    $("#lblProjectStage").text($("#ddlProjectStage option:selected").text());
}
function updateWSYWIG(bool) {
    $(".RTBox").each(function () {
        var iframe = $(this).closest(".row").find("iframe");
        var innerHTML = iframe.contents().find("#tinymce").html();
        $(this).html(innerHTML);
    });
    if (bool == "true") {
        return true;
    } else {
        return confirm("Are you sure you want to delete?");
    }
}
function showhideAccordion(clicked) {
    var el = $(clicked);
    var divFinanceBrief = el.closest("div.FinanceBriefs").find('.FinanceBrief');
    if (divFinanceBrief.hasClass('hide')) {
        divFinanceBrief.removeClass('hide');
    } else {
        divFinanceBrief.addClass('hide');
    }
}
function BusinessFunctionChanged() {
    if ($("#ddlBusinessFunction option:selected").text() == "Other") {
        $('.BusinessFunctionOther').removeClass('hideItem');
    } else {
        $('.BusinessFunctionOther').addClass('hideItem');
        $('#txtBusinessFunctionOther').val('');

    }
}
function fixMonth() {

    var months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
    if ($("#txtFirstShipDate").datepicker('getDate') != null) {
        var selectedMonth = $("#txtFirstShipDate").datepicker('getDate').getMonth();


        $("#1stmonth").html(months[selectedMonth]);
        $("#1stmonthU").html(months[selectedMonth]);
        $("#1stmonthsummary").html(months[selectedMonth]);
        $("#1stmonthUsummary").html(months[selectedMonth]);

        if (selectedMonth + 1 > 11)
            selectedMonth = -1;
        $("#2ndmonth").html(months[selectedMonth + 1]);
        $("#2ndmonthU").html(months[selectedMonth + 1]);
        $("#2ndmonthsummary").html(months[selectedMonth + 1]);
        $("#2ndmonthUsummary").html(months[selectedMonth + 1]);

        if (selectedMonth + 2 > 11)
            selectedMonth = -2;

        $("#3rdmonth").html(months[selectedMonth + 2]);
        $("#3rdmonthU").html(months[selectedMonth + 2]);
        $("#3rdmonthsummary").html(months[selectedMonth + 2]);
        $("#3rdmonthUsummary").html(months[selectedMonth + 2]);

    }
}
