var gotoStep;
$(document).ready(function () {
    drpCompType_load();
    var $tabs = $('#wizard').tabs();
    gotoStep = $("#hdnSteps").val();
    if ($("#updateHdnSteps").val() == "10") {
        gotoStep = 10;
        $("#hdnSteps").val("10");

    }
    var url = window.location.href;
    var pieces = url.split("?");
    var checkIfnew;
    if (pieces.length > 1) {
        var q = pieces[1].split("=");
        checkIfnew = q[0];
        var status = pieces[1].split("&");
        if (status[1] == "Status=Saved") {
            if ($("#hdnStatus").val() == '') {
                $("#hdnStatus").val("saved");
                $("#hdnSteps").val("1");
                gotoStep = 1;
            }
        }
    }
    if (gotoStep == null || gotoStep == "") {
        gotoStep = 0;
    }
    if (checkIfnew != 'ProjectNo') {
        $("#wizard").tabs("disable");
        $("#wizard").tabs("enable", 0);
    } else {
        var next = parseInt(gotoStep) + 1;
        var prev = parseInt(gotoStep) - 1;
        var appendCode = "<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'>";
        appendCode = appendCode + "<a href='#' class='next-tab mover justifyRight' rel='" + next + "'><div>Next</div></a>";
        appendCode = appendCode + "<a href='#finish' class='submit-tab mover justifyRight'><div>Submit</div></a>";
        appendCode = appendCode + "<a href='#' class='prev-tab mover justifyRight' rel='" + prev + "'><div>Previous</div></a>";
        appendCode = appendCode + "</div></div>";
        $(".actions").html(appendCode);
    }
    setActiveTab(gotoStep);
    $('#loadingIcon').hide();
    BindHierarchiesOnLoad();
    ShowHideApprovedGraphicsAsset()
    UPCAssociatedChanged();
    BioEngLabelingRequiredChanged();
    showConsumerFacingProdDesc();
});
function activateTS(clicked) {
    $(clicked).closest("section").find(".tsButton").each(function () {
        $(this).next().addClass("hideItem");
    });
    $(clicked).parent().next().removeClass("hideItem");
    $(clicked).closest("section").find(".tsButtonLink").each(function () {
        $(this).removeClass("activeTSButton");
    });
    $(clicked).addClass("activeTSButton");
}
function activateTab(id) {
    $(".TSSection").addClass("hideItem");
    $("#" + id).parents(".TSSection").removeClass("hideItem");
    $(".tsButtonLink").removeClass("activeTSButton");
    $("#" + id).parents(".TSSection").prev().find(".tsButtonLink").addClass("activeTSButton");
}
function isDisabled(index) {

    var disabled = $("#wizard").tabs("option", "disabled");
    for (var i = 0; i < disabled.length; i++) {
        disabled[i] = parseInt(disabled[i]);
    }
    return $.inArray(index, disabled) > -1;
}
function setActiveTab(activeTabNumber) {
    var tempActiveTabNumber = parseInt(activeTabNumber);
    var currentlySelected = parseInt($("#wizard").tabs('option', 'active'));
    var clicked = parseInt(tempActiveTabNumber);

    while (true) {
        if (isDisabled(tempActiveTabNumber)) {
            if (currentlySelected < clicked) {
                tempActiveTabNumber++;
            } else {
                tempActiveTabNumber--;
            }
        } else {
            activeTabNumber = tempActiveTabNumber;
            //alert("3: "+tempActiveTabNumber);
            break;
        }
    }
    $("#wizard").tabs({ "active": activeTabNumber });
    var updateNext = parseInt(activeTabNumber) + 1;
    var updatePrev = parseInt(activeTabNumber) - 1;
    $('.prev-tab').attr("rel", updatePrev);
    $('.next-tab').attr("rel", updateNext);
    if ("12" == activeTabNumber) {
        $('.actions .next-tab').hide();
        $('.actions .submit-tab').show();
        $(".actions .prev-tab").show();
    } else if (activeTabNumber == "0") {
        $(".actions .prev-tab").hide();
        $('.actions .next-tab').show();
        $('.actions .submit-tab').hide()
    } else {
        $(".actions .prev-tab").show();
        $('.actions .next-tab').show();
        $('.actions .submit-tab').hide()
    }
}

function ShowHideApprovedGraphicsAsset() {
    var projectType = $('#txtProjectType').val();
    if (projectType == "Graphics Change Only") {
        $('.ApprovedGraphicsAsset').each(function (i, obj) {
            $(this).removeClass('hide');
        });
    } else {
        $('.ApprovedGraphicsAsset').each(function (i, obj) {
            $(this).addClass('hide');
        });
    }
}

function DeleteApprovedGraphicsAsset(clicked) {
    var button = $(clicked);
    var parentBomRow = $(clicked).closest(".bomrow");
    var DeletedApprovedGraphicsAssetUrl = parentBomRow.find("#DeletedApprovedGraphicsAssetUrl").val();
    $("#hdnDeleteApprovedGraphicsAssetUrl").val(DeletedApprovedGraphicsAssetUrl);
    $("#hdnbtnDeleteApprovedGraphicsAsset").click();
}

function DeleteVisualreference(clicked) {
    var button = $(clicked);
    var parentBomRow = $(clicked).closest(".bomrow");
    var DeletedVisualreferenceUrl = parentBomRow.find("#DeletedVisualreferenceUrl").val();
    $("#hdnDeleteApprovedGraphicsAssetUrl").val(DeletedVisualreferenceUrl);
    $("#hdnbtnDeleteApprovedGraphicsAsset").click();
}

function DeleteBEQRCEPSFile(clicked) {
    var button = $(clicked);
    var parentBomRow = $(clicked).closest(".bomrow");
    var DeletedBEQRCEPSFileUrl = parentBomRow.find("#DeletedBEQRCEPSFileUrl").val();
    $("#hdnDeleteApprovedGraphicsAssetUrl").val(DeletedBEQRCEPSFileUrl);
    $("#hdnbtnDeleteApprovedGraphicsAsset").click();
}

function GotoStepAndFocus(arg) {
    var anchor = $("#" + arg);
    var wizard = anchor.closest("section")[0].id;
    var gotoIndex = wizard.replace('wizard-h-', '');

    setActiveTab(gotoIndex)
    $("#" + arg).addClass('highlightElement');
    $('#' + arg).focus();
}
function GotoStepAndFocusNoRed(arg) {
    var anchor = $("#" + arg);
    var wizard = anchor.closest("section")[0].id;
    var gotoIndex = wizard.replace('wizard-h-', '');

    setActiveTab(gotoIndex);
    $('#' + arg).focus();
}
function hideItem(arg) {
    $("#" + arg).removeClass('showItem').addClass('hideItem');
}
function showItem(arg) {
    $("#" + arg).removeClass('hideItem').addClass('showItem');
}

function IPFBOMValidator(clearList) {
    if (clearList)
        $('#error_message').empty();

    var isValid = true;
    $('.PCBOMrequired').each(function (i, obj) {
        var sd = $(this).closest(".form-group").find('label').text().replace(":", "") + ' is required';
        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if (value == "") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=activateTab(' + '"' + id + '"' + ');setFocusElement(' + '"' + id + '"' + '); >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
            }
        }
        else {
            var value = $(this).val();
            if ((!$(this).parent().parent().hasClass('hideItem') && !$(this).closest(".row").hasClass('hideItem')) && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if (value == '-1') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=activateTab(' + '"' + id + '"' + ');setFocusElement(' + '"' + id + '"' + '); >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
        }
    });

    $('.BEQRCodeEPSFile').each(function (i, obj) {
        var sd = 'BE QR Code EPS File is required';
        var id = $(this).closest('.bomrow').find('.ddlMoveTS').attr('id');
        var ddlBioEngLabelingRequired = $(this).closest('.bomrow').find('.ddlBioEngLabelingRequired').attr('id');
        var drpComponentTypeId = $(this).closest('.bomrow').find('.drpComponentType').attr('id');
        var CompType = $("#" + drpComponentTypeId + " option:selected").text().toLocaleLowerCase();
        var BioEngLabelingRequired = $("#" + ddlBioEngLabelingRequired + " option:selected").text();

        if (BioEngLabelingRequired.indexOf("Yes – Apply QR Code") != -1 || BioEngLabelingRequired.indexOf("Yes -  Apply QR Code") != -1) {
            if (CompType.indexOf('candy') == -1 && CompType.indexOf('transfer') == -1 && CompType.indexOf('finished good') == -1 && CompType.indexOf('purchased candy') == -1) {
                if ($(this).find('.ancRenderingBEQRCodeEPSFile').attr('href') === undefined) {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=activateTab(' + '"' + ddlBioEngLabelingRequired + '"' + ');GotoStepAndFocusNoRed(' + '"' + ddlBioEngLabelingRequired + '"' + '); >Update</a>   </li></br>');
                }
            }
        }
    });

    var isSAPNumbersValid = VerifySAPNumbers();

    if (!isValid || !isSAPNumbersValid) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();

    }
    return (isValid && isSAPNumbersValid);
}
function RepeaterValidator(arg, clearList) {
    if (clearList)
        $('#error_message').empty();

    var isValid = true;
    var dvMain = $('.' + arg).closest("div.repeater");
    if (!dvMain.hasClass('hideItem')) {
        $('.' + arg).each(function (i, obj) {
            var id = $(this).attr('id');
            if ($(this).is('input')) {
                var sd = $(this).attr('title');
                var value = $(this).val();
                if (value == "" || value == "0") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
            }
            else {
                var sd = $(this).attr('title');
                var value = $(this).val();
                if (value == '-1') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
            }
        });
    }
    if (!isValid) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
        setFocusError();
    }
    return isValid;
}
function ValidateIPFData() {
    $('#error_message').empty();
    var isValid = true;
    $('.required').each(function (i, obj) {
        var dvMain = $(this).closest("div.form-group");
        var label = dvMain.find('label');
        var fieldName = label.length ? label.text().replace(":", "") : $(this).attr('title');
        var requiredMsg = fieldName + ' is required';
        var id = $(this).attr('id');
        if (id == "txtProfitCenter") {
            requiredMsg = "A Profit Center has not been identified for this item, please contact your brand finance team member";
        }
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).parent().hasClass('hide'))
                if (value == "") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + requiredMsg +
                        ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        }
        else {
            var value = $(this).val();
            if (!$(this).parent().hasClass('hideItem'))
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
    });

    $('.WrongItemControl').each(function (i, obj) {
        if (!$(this).parent().hasClass('hideItem') && !$(this).parent().hasClass('hide')) {
            var id = $(this).attr('id');
            var selectedvalue = $(this).val();
            var selectedText = $("#" + id + " option:selected").text();
            if (selectedvalue == "-9999") {
                isValid = false;
                var requiredMsg = "'" + selectedText + "' is not a valid value! Please select a new one.";
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
            }
        }
    });

    $('.minimumlength').each(function (i, obj) {
        var sd = $(this).parent().find('label').text().replace(":", "") + ' must be at least 5 digits';
        //var sd = 'Please enter at least 5 digits';
        var id = $(this).attr('id');
        if ($(this).is('input')) {
            var value = $(this).val().trim().length;
            var dvMain = $(this).closest("div.repeater");
            if (!$(this).parent().hasClass('hideItem') && !dvMain.hasClass('hideItem')) {
                if (value > 0 && value < 5) {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
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
    var isValidBom = IPFBOMValidator(false);
    var isValidMixes = RepeaterValidator('Mixesrequired', false);
    var isValidShipper = RepeaterValidator('FGItemrequired', false);
    if ($("#ddlProjectType option:selected").text() == "Graphics Changes/Internal Adjustments" || $("#ddlProjectType option:selected").text() == "Graphics Change Only") {
        if ($("#ddlTBDIndicator option:selected").text() == 'Yes') {
            isValid = false;
            $("#dverror_message").show();
            $('#error_message').append('<li class="errorMessage" >Graphics Changes/Internal Adjustments projects cannot request a New FG#. Please select a different Project Type.' +
                ' <a style="color:darkblue" onclick=GotoStepAndFocus("ddlProjectType") >Update</a>   </li></br>');
        }
    }
    if ($("#ItemValidationSummary:visible").length) {
        isValid = false;
    }

    if (isValidBom && isValidMixes && isValidShipper && isValid)
        return true;
    loadingIconAdded = true;
    $(".disablingLoadingIcon").remove();
    setFocusError();
    return false;
}
function CheckMinimumLength(arg) {
    //  $('#error_message').empty();
    var isValid = true;
    $('.minimumlength').each(function (i, obj) {
        var sd = $(this).parent().find('label').text().replace(":", "") + ' must be at least ' + arg + ' digits';
        //var sd = 'Please enter at least ' + arg + ' digits';
        var id = $(this).attr('id');
        if ($(this).is('input')) {
            var value = $(this).val().trim().length;
            if (!$(this).parent().hasClass('hideItem'))
                if (value < arg) {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        }
    });
    if (!isValid) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
        setFocusError();
    }
    return isValid;
}
//******** Basic Dialog Starts Here ***********/
function openBasicDialogIPF(tUrl, tTitle) {
    var options = {
        url: tUrl,
        title: tTitle,
        dialogReturnValueCallback: onPopUpCloseCallBack
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}
function onPopUpCloseCallBack(result, returnValue) {
    $("#hdnSteps").val("11");
    // $(".Save").click();
    $(".LoadAttachments").click();
}
//******** Basic Dialog Ends Here ***********/

///  Attachments
//******** Basic Dialog Starts Here ***********/
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
    $("#hdnSteps").val("10");
    // $(".Save").click();
    $(".LoadAttachments").click();
}
//******Disable Enter key***********//
function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type != "textarea")) {
        return false;
    }
}
function isNumberofPieces(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (
        (charCode != 45 || $(element).val().indexOf('-') != -1) &&
        (charCode != 46 || $(element).val().indexOf('.') != -1) &&
        (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function drpCompType_load() {
    $(".drpComponentType").each(function () {
        var compType = $(this).find("option:selected").text().toLocaleLowerCase();
        var anchor = $(this).closest(".bomrow");
        var newexisting = anchor.find(".drpNewClass option:selected").text();
        if (compType == "candy semi" || compType == "transfer semi" || compType == "purchased candy semi") {
            anchor.find(".hideableRow").addClass("hideItem");
            anchor.find(".hideableRow .col-xs-12").addClass("hideItem");
            if (compType == "transfer semi" || compType == "purchased candy semi") {
                anchor.find(".TSOnlyRow").removeClass("hideItem");
                anchor.find(".TSOnlyRow .col-xs-12").removeClass("hideItem");
                anchor.find(".TSOnlyRow .switch").html($(this).find("option:selected").text());
                if (!anchor.find(".drpNewClass option:eq(3)").length) {
                    anchor.find(".drpNewClass").append('<option value="Network Move">Network Move</option>');
                }
                if (newexisting != "New") {
                    anchor.find(".TSOnlyRow.new").addClass("hideItem");
                }

            } else {
                anchor.find(".TSOnlyRow").addClass("hideItem");
                anchor.find(".TSOnlyRow .col-xs-12").addClass("hideItem");
                anchor.find(".drpNewClass option:eq(3)").remove();
            }
        } else {
            anchor.find(".hideableRow").removeClass("hideItem");
            anchor.find(".hideableRow .col-xs-12").removeClass("hideItem");
            anchor.find(".TSOnlyRow").addClass("hideItem");
            anchor.find(".TSOnlyRow .col-xs-12").addClass("hideItem");
            anchor.find(".drpNewClass option:eq(3)").remove();
        }

        if (compType == "candy semi" || compType == "transfer semi" || compType == "purchased candy semi" || compType.indexOf('finished good') != -1) {
            anchor.find(".BEQRChideableRow").addClass("hideItem");
            anchor.find(".BEQRChideableRow .col-xs-12").addClass("hideItem");
        } else {
            anchor.find(".BEQRChideableRow").removeClass("hideItem");
            anchor.find(".BEQRChideableRow .col-xs-12").removeClass("hideItem");
        }
    });
    $(".drpNewClass").each(function () {
        var NewExisting = $(this).find("option:selected").text();
        var anchor = $(this).closest(".bomrow");
        var flowThrough = anchor.find(".flowthroughClass");
        var TBD = $("#ddlTBDIndicator option:selected").text();
        var compType = $(this).closest(".bomrow").find(".drpComponentType option:selected").text();
        if (NewExisting == 'New' && (compType == "Transfer Semi" || compType == "Purchased Candy Semi")) {
            anchor.find(".TSOnlyRow.new").removeClass("hideItem");
        } else {
            anchor.find(".TSOnlyRow.new").addClass("hideItem");
        }


        flowThrough.prop("disabled", false);
        if (NewExisting == 'New' && TBD == "No") {
            flowThrough.addClass("PCBOMrequired");
            flowThrough.closest(".form-group").find(".spanFlowthrough").removeClass("hideItem");
            anchor.find(".OldMaterial").removeClass("PCBOMrequired");
            anchor.find(".spanOldMaterial").addClass("hideItem");
            if ($("#ddlLineOfBusiness option:selected").text() == "Everyday (000000025)") {
                if (flowThrough.val() == "-1") {
                    flowThrough.find("option:contains(Soft)").attr('selected', 'selected');
                    flowThrough.val("2");
                }
            }
        } else if (NewExisting == 'New' && TBD == "Yes") {
            flowThrough.addClass("PCBOMrequired");
            flowThrough.closest(".form-group").find(".spanFlowthrough").removeClass("hideItem");
            flowThrough.prop("disabled", false);
            if (flowThrough.find("option:selected").text() == "Soft") {
                anchor.find(".OldMaterial").addClass("PCBOMrequired");
                anchor.find(".spanOldMaterial").removeClass("hideItem");
            } else {
                anchor.find(".OldMaterial").removeClass("PCBOMrequired");
                anchor.find(".spanOldMaterial").addClass("hideItem");
            }

        } else if (NewExisting == 'Existing') {
            flowThrough.removeClass("PCBOMrequired");
            flowThrough.closest(".form-group").find(".spanFlowthrough").addClass("hideItem");
            if (flowThrough.val() == "-1") {
                flowThrough.find("option:contains(Not Applicable)").attr('selected', 'selected');
                flowThrough.val("3");
            }
            anchor.find(".OldMaterial").removeClass("PCBOMrequired");
            anchor.find(".spanOldMaterial").addClass("hideItem");
            //flowThrough.prop("disabled", true);
        } else if (NewExisting == 'Network Move') {
            flowThrough.removeClass("PCBOMrequired");
            flowThrough.closest(".form-group").find(".spanFlowthrough").addClass("hideItem");
            flowThrough.prop("disabled", false);
            flowThrough.find("option:contains(Not Applicable)").attr('selected', 'selected');
            flowThrough.val("3");
            anchor.find(".OldMaterial").removeClass("PCBOMrequired");
            anchor.find(".spanOldMaterial").addClass("hideItem");
        } else {
            anchor.find(".OldMaterial").removeClass("PCBOMrequired");
            anchor.find(".spanOldMaterial").addClass("hideItem");
        }
    });
    if ($("#TSSections .bomrow").length) {
        $("#TSSections").removeClass("hideItem");
    }
    $(".ddlMoveTS").each(function () {
        if ($(this).find("option").length <= 1) {
            $(this).parents(".row").hide();
        }
    });
    var buttonCount = $(".tsButtonLink").length - 1;
    var index = 1;
    var topindex = 1;

    var PositionReferenceForButtons = $('.wizard').position();
    //
    //console.log(position.top);
    //console.log(position.left);

    $(".tsButtonLink").each(function () {
        var offsetpx = 400;
        //PositionReferenceForButtons = 0;
        if ($(this).parents("#FGSection").length) {
            var topMath = PositionReferenceForButtons.top - offsetpx;
            $(this).attr("style", "top:" + topMath + "px;");
        } else {
            var leftMath = (index * 225) + 25;
            var topMath = 0;
            if (topindex == 1) {
                topMath = PositionReferenceForButtons.top - offsetpx;
            } else {
                topMath = PositionReferenceForButtons.top - offsetpx + (topindex * 20);
            }
            $(this).attr("style", "left:" + leftMath + "px;top:" + topMath + "px;");
            index++;
            if (index > 3) {
                index = 0;
                topindex++;
            }

            if (topindex >= 3) {
                topindex++;
            }

            buttonCount--;
        }
    });
    if (topindex > 1) {
        var marginMath = 50 * (topindex);
        $(".TSSection").css("margin", marginMath + "px 0px");
    }
    if ($("#activeTabHolder").val() != 0 && $("#activeTabHolder").val() != "") {
        $("#TSSections #hdnParentId").each(function () {
            if ($(this).val() == $("#activeTabHolder").val()) {
                $(this).parents(".TSSection").prev().find(".tsButtonLink").click();
            }
        });
    } else {
        $("#FGSection .tsButtonLink").click();
    }
}

function getProjectDescriptionType(projectTypeName) {
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Project Types Lookup')/items?$select=ProjectDescription&$filter=(Title eq '" + projectTypeName.replace("&", "%26") + "')&$top=1",
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

function copyFGBOMItemstoSummary() {

    var newSummary = $("#wizard-h-11").data("arr", [1]),
    clone = newSummary.clone(false)
      .data("arr", $.extend([], newSummary.data("arr")));
    clone.find(":input[type=hidden]").remove();
    clone.find("[id='TSSections']").attr("id", "TSSectionsSummary");
    clone.find("[id='FGSection']").attr("id", "FGSectionSummary");
    clone.find(":not([id='TSSectionsSummary'],[id='FGSectionSummary'])").removeAttr("id");
    clone.find("span").removeAttr("id");
    clone.find("span").removeAttr("name");
    clone.find(":input[type=submit]").remove();
    clone.find(".markrequired").remove();
    clone.find(".PCBOMrequired").removeClass("PCBOMrequired");

    clone.find(":input[type=text]").removeAttr("onchange");
    clone.find(":input[type=text]").removeAttr("id");
    clone.find(":input[type=text]").removeAttr("name");
    clone.find(":input[type=text]").attr("readonly", "readonly");
    clone.find("textarea").removeAttr("onchange");
    clone.find("textarea").removeAttr("id");
    clone.find("textarea").removeAttr("name");
    clone.find("textarea").attr("readonly", "readonly");
    clone.find("select").removeAttr("id");
    clone.find("select").removeAttr("name");
    clone.find("select").removeAttr("onchange");
    clone.find("select").attr("readonly", "readonly");
    var cloneCode = $(clone).html();
    $(cloneCode).appendTo("#packagingItemSummary");
    //$("#packagingItemSummary").append(clone.html());
}

function marketingClaims() {
    /*$(".vitamin").each(function () {
        if ($(this).find("option:selected").text().toLocaleLowerCase() != "yes") {
            $(this).parent().parent().next().find(".form-group").addClass("hideItem");
        }
    });*/
    if ($("#drpDesiredClaims option:selected").val().toLocaleLowerCase() == "yes") {
        $(".marketingClaims").removeClass("hideItem");
        $(".marketingClaims input, .marketingClaims select, .marketingClaims textarea").addClass("required");
        $(".existingClaims").addClass("hideItem");
        $(".existingClaims input, .existingClaims select, .existingClaims textarea").removeClass("required");
        $("#drpGoodSourceAvailable").addClass("required");
    } else if ($("#drpDesiredClaims option:selected").val().toLocaleLowerCase() == "existing") {
        $(".existingClaims").removeClass("hideItem");
        $(".existingClaims input, .existingClaims select, .existingClaims textarea").addClass("required");
        $(".marketingClaims").addClass("hideItem");
        $(".marketingClaims input, .marketingClaims select, .marketingClaims textarea").removeClass("required");
        $("#drpGoodSourceAvailable").removeClass("required");
    } else {
        $(".marketingClaims, .existingClaims").addClass("hideItem");
        $(".marketingClaims input, .marketingClaims select, .marketingClaims textarea, .existingClaims input, .existingClaims select, .existingClaims textarea").removeClass("required");
        $("#drpGoodSourceAvailable").removeClass("required");
    }
    if ($("#drpMadeInUSA option:selected").text().toLocaleLowerCase() == "yes") {
        $("#drpMadeInUSAPct").addClass("required");
        $("#drpMadeInUSAPct").parent().prepend("<span class='markrequired'>*</span>");
        $("#drpMadeInUSAPct").parent().parent().removeClass("hideItem");
    } else {
        $("#drpMadeInUSAPct").removeClass("required");
        $("#drpMadeInUSAPct").parent().find(".markrequired").remove();
        $("#drpMadeInUSAPct").parent().parent().addClass("hideItem");
    }
    $(".vitaminPct").closest(".row").addClass("hideItem");
    if ($("#hdnSelectedGoodSource").length && $("#hdnSelectedGoodSource").val() != "") {
        var goodSource = $("#hdnSelectedGoodSource").val();
        $.each(goodSource.split(","), function (index, item) {
            $("#drpGoodSourceAvailable option").filter(function () {
                return ($(this).text() == item);
            }).prop('selected', true);
        });
        addChannel();
    }
}

function addChannel() {
    var drpGoodSourceAvailable = $("#drpGoodSourceAvailable");
    var drpGoodSourceSelected = $("#drpGoodSourceSelected");

    drpGoodSourceAvailable.find("option:selected").each(function (idx, opt) {
        drpGoodSourceSelected.prepend("<option value=\"" + opt.value + "\">" + opt.text + "</option>");
        /*var noSpaces = opt.text.replace(" ", "");
        var pctID = $("#drp" + noSpaces + "Pct");
        pctID.parent().parent().removeClass("hideItem");
        pctID.addClass("required");
        pctID.parent().prepend("<span class='markrequired'>*</span>");*/
    });
    drpGoodSourceAvailable.find("option:selected").remove();
    sortSelect("#drpGoodSourceSelected");
    setHiddenselected();
}

function setHiddenselected() {
    var drpGoodSourceAvailable = $("#drpGoodSourceAvailable");
    var drpGoodSourceSelected = $("#drpGoodSourceSelected");
    var selected = "";
    var count = drpGoodSourceSelected.find("option").length - 1;
    drpGoodSourceSelected.find("option").each(function (idx, opt) {
        var noSpaces = opt.text.replace(" ", "");
        var pctID = $("#drp" + noSpaces + "Pct");
        if (idx != 0) {
            selected = opt.text + "," + selected;
        } else {
            selected = opt.text;
        }
    });
    $("#hdnSelectedGoodSource").val(selected);
}

function removeChannel() {
    var drpGoodSourceAvailable = $("#drpGoodSourceAvailable");
    var drpGoodSourceSelected = $("#drpGoodSourceSelected");
    var selected = "";
    drpGoodSourceSelected.find("option:selected").each(function (idx, opt) {
        drpGoodSourceAvailable.prepend("<option value=\"" + opt.value + "\">" + opt.text + "</option>");
        /*var noSpaces = opt.text.replace(" ", "");
        var pctID = $("#drp" + noSpaces + "Pct");
        pctID.parent().parent().addClass("hideItem");
        pctID.removeClass("required");
        pctID.parent().parent().find(".markrequired").remove();*/
    });
    drpGoodSourceSelected.find("option:selected").remove();
    sortSelect("#drpGoodSourceAvailable");
    setHiddenselected();
}

var sortSelect = function (select) {
    $(select).html($(select).children('option').sort(function (x, y) {
        return $(x).text().toUpperCase() < $(y).text().toUpperCase() ? -1 : 1;
    }));
};

$("#drpDesiredClaims").change(function () {
    if ($(this).find("option:selected").val().toLocaleLowerCase() == "yes") {
        $(".marketingClaims").removeClass("hideItem");
        $(".marketingClaims input, .marketingClaims select, .marketingClaims textarea").addClass("required");
        $(".existingClaims").addClass("hideItem");
        $(".existingClaims input, .existingClaims select, .existingClaims textarea").removeClass("required");
        $("#drpGoodSourceAvailable").addClass("required");
    } else if ($(this).find("option:selected").val().toLocaleLowerCase() == "existing") {
        $(".existingClaims").removeClass("hideItem");
        $(".existingClaims input, .existingClaims select, .existingClaims textarea").addClass("required");
        $(".marketingClaims").addClass("hideItem");
        $(".marketingClaims input, .marketingClaims select, .marketingClaims textarea").removeClass("required");
        $("#drpGoodSourceAvailable").removeClass("required");
    } else {
        $(".marketingClaims, .existingClaims").addClass("hideItem");
        $(".marketingClaims input, .marketingClaims select, .marketingClaims textarea, .existingClaims input, .existingClaims select, .existingClaims textarea").removeClass("required");
        $("#drpGoodSourceAvailable").removeClass("required");
    }
});
function RetailUnitWeightRequirement() {
    //"IF Material Group 5 = Display, Shipper, Mod, Half Mod or Mulitple-> Optional ELSE ->Required"
    var txtRetailUnitWeight = $("#txtRetailUnitWeight");
    var MaterialGroup5 = $("#ddlMaterialGroup5 option:selected").text();
    if (MaterialGroup5 == null) {
        MaterialGroup5 = "";
    } else {
        MaterialGroup5 = MaterialGroup5.toLowerCase();
    }

    if (MaterialGroup5.toLowerCase() == "shipper (shp)" || MaterialGroup5 == "display (dis)" || MaterialGroup5 == "mod (mod)" || MaterialGroup5 == "half mod (hmd)" || MaterialGroup5 == "multiple (mlt)") {
        txtRetailUnitWeight.removeClass("required");
        txtRetailUnitWeight.closest(".form-group").find(".markrequired").remove();
    } else {
        if (!txtRetailUnitWeight.hasClass("required")) {
            txtRetailUnitWeight.addClass("required");
            txtRetailUnitWeight.closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
        }
    }
}
//SAP Nomenclature
function getCountryCode() {
    var CountryCode = "";
    if ($("#ddlOutsideUSA option:selected").text() == "Yes") {
        if ($("#ddlCountryOfSale option:selected").text() == "Argentina") {
            CountryCode = " AR";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Brazil") {
            CountryCode = " BR";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Canada") {
            CountryCode = " CA";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "China") {
            CountryCode = " CN";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "France") {
            CountryCode = " FR";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Germany") {
            CountryCode = " DE";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Italy") {
            CountryCode = " IT";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Mexico") {
            CountryCode = " MX";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Other") {
            CountryCode = "";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Poland") {
            CountryCode = " PL";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "Spain") {
            CountryCode = " ES";
        }
        else if ($("#ddlCountryOfSale option:selected").text() == "USA" || $("#ddlCountryOfSale option:selected").text() == "United States") {
            CountryCode = " US";
        }
    }
    return CountryCode;
}

$(document.body).on("keypress", "#txtUnitsInsideCarton", function (evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);

    if (key.length == 0) {
        return;
    }

    var regex = /^([1-9][0-9]?[0-9]?[A-z]?[A-z]?)$/;

    if (!regex.test($(this).val() + key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
});

function ClearDropdwonStyles() {
    $(".WrongItemControl").each(function (i, obj) {
        var id = $(this).attr('id');
        var selectedVal = $("#" + id + " option:selected").val();
        var WrongText = $("#" + id).children("option[value=" + "-9999" + "]").text();
        var hiddenControl = $('#hdn' + id);


        if (selectedVal == "-9999") {
            $(this).attr("style", "background-color:Pink;");
            if (hiddenControl != null) {
                hiddenControl.val(WrongText);
            }
        } else {
            $(this).removeAttr("style");
            $("#" + id).children("option[value=" + "-9999" + "]").hide();
            if (WrongText == "" && hiddenControl != null) {
                WrongText = hiddenControl.val();
            }
            if (WrongText != "") {
                $('#ItemValidationSummary li:contains(' + WrongText + ')').addClass('hide');
            }
        }
    });

    if ($('#ItemValidationSummary li[class!="hide"]').length == 0) {
        $('#ItemValidationSummary').text("");
        $('#ItemValidationSummary').text("");
    }
}

function ClearErrorsIfTabIsDisabled() {

    var ElementIds =
    [
        "ddlProductHierarchyLevel1",
        "ddlProductHierarchyLevel2",
        "ddlBrand_Material",
        "ddlMaterialGroup4",
        "ddlMaterialGroup5"
    ]

    $.each(ElementIds, function (index, ElementId) {
        if ($("#" + ElementId).hasClass('WrongItemControl')) {
            var selectedVal = $("#" + ElementId + " option:selected").val();

            if (selectedVal == "-9999") {
                var WrongText = $("#" + ElementId).children("option[value=" + "-9999" + "]").text();
                $("#" + ElementId).removeAttr("style");
                $("#" + ElementId).children("option[value=" + "-9999" + "]").hide();
                $("#" + ElementId).val("-1");
                $("#" + ElementId).removeAttr("style");
                $('#ItemValidationSummary li:contains(' + WrongText + ')').addClass('hide');
            }
        }
    });

    if ($('#ItemValidationSummary li[class!="hide"]').length == 0) {
        $('#ItemValidationSummary').text("");
        $('#ItemValidationSummary').text("");
    }
}

$(document.body).on("change", ".WrongItemControl", function () {
    var id = $(this).attr('id');
    var selectedVal = $("#" + id + " option:selected").val();
    var WrongText = $("#" + id).children("option[value=" + "-9999" + "]").text();
    var hiddenControl = $('#hdn' + id);

    if (selectedVal == "-9999") {
        $(this).attr("style", "background-color:Pink;");
        if (hiddenControl != null) {
            hiddenControl.val(WrongText);
        }
    } else {
        $(this).removeAttr("style");
        if ($(this).hasClass('highlightElement')) {
            $(this).removeClass('highlightElement')
        }
        $("#" + id).children("option[value=" + "-9999" + "]").hide();
        if (WrongText == "" && hiddenControl != null) {
            WrongText = hiddenControl.val();
        }
        if (WrongText != "") {
            $('#ItemValidationSummary li:contains(' + WrongText + ')').addClass('hide');
        }
    }

    if ($('#ItemValidationSummary li[class!="hide"]').length == 0) {
        $('#ItemValidationSummary').text("");
        $('#ItemValidationSummary').text("");
    }
});

function getSAPItemDescription(lookupValue, descriptionID) {
    var enteredText = $("#" + lookupValue).val();
    if (enteredText != "") {
        $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,Title&$filter=(Title eq '" + enteredText + "')&$top=500",
            method: 'GET',
            dataType: 'json',
            headers: {
                Accept: "application/json;odata=verbose"
            },
            success: function (results) {
                var finding = results.d.results[0];
                if (finding === undefined) {
                    $("#" + lookupValue).closest(".row").append("<div class='noResultsRow'>Lookup returned no results.</div>");
                } else {
                    $("#" + lookupValue).closest(".row").find(".noResultsRow").remove();
                    $("#" + descriptionID).val(finding.SAPDescription);

                    //Update Project Header title
                    //var ProjectTitle = $('#lblProjectTitle').text();
                    //var ProjectTitleArray = ProjectTitle.split(":");
                    //$('#lblProjectTitle').text(ProjectTitleArray[0] + " : " + ProjectTitleArray[1] + " : " + finding.SAPDescription);
                }
                $("#" + lookupValue).closest(".row").find(".disablingLoadingIcon").remove();
                $("#" + descriptionID).focus();
                $("#" + descriptionID).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
        });
    }
}

function SAPNomenclature(controlId) {
    ClearDropdwonStyles();
    var BuildSAPDescription = true;

    if ($('#hdnNewIPF').val() == "Yes") {
        $("#txtSAPItemDescription").prop('readonly', true);
        $("#pSAPItemDescriptionMessage").removeClass('hide');

        if ($("#ddlTBDIndicator option:selected").text() == "Yes") {
            if ($("#ddlProductHierarchyLevel1 option:selected").text() == "Co-Manufacturing (000000027)") {
                showItem('divManuallyCreateSAPDescription');
                if ($("#ddlManuallyCreateSAPDescription option:selected").text() == "Yes") {
                    BuildSAPDescription = false;
                    $("#txtSAPItemDescription").prop('readonly', false);

                    //Update Project Header title
                    var ProjectTitle = $('#lblProjectTitle').text();
                    var ProjectTitleArray = ProjectTitle.split(":");
                    $('#lblProjectTitle').text(ProjectTitleArray[0] + " : " + ProjectTitleArray[1] + " : " + $("#txtSAPItemDescription").val());
                }
                else {
                    BuildSAPDescription = true;
                    $("#txtSAPItemDescription").prop('readonly', true);
                }
            }
            else {
                $("#ddlManuallyCreateSAPDescription").val("No");
                hideItem('divManuallyCreateSAPDescription');
            }
        }
        else {
            $("#ddlManuallyCreateSAPDescription").val("No");
            hideItem('divManuallyCreateSAPDescription');
            BuildSAPDescription = false;
            if (controlId != null && controlId.id == "ddlTBDIndicator") {
                getSAPItemDescription("txtSAPItemNumber", "txtSAPItemDescription");
            }
        }
    }
    else {
        $("#ddlManuallyCreateSAPDescription").val("No");
        hideItem('divManuallyCreateSAPDescription');
        $("#pSAPItemDescriptionMessage").addClass('hide');
        $("#txtSAPItemDescription").prop('readonly', false);
        BuildSAPDescription = false;
    }

    if (BuildSAPDescription) {

        $("#txtSAPItemDescription").prop('readonly', true);

        $('#divProductFormDescription').removeClass('hide');
        $('#txtProductFormDescription').addClass('required');
        $('.spProductFormDescription').removeClass('hide');

        $('#dvInvolvesCarton').removeClass('hide');
        if ($("#ddlInvolvesCarton option:selected").text() == 'Yes') {
            $('#dvUnitsInsideCarton').removeClass('hide');
            $('#divNumberofTraysPerBaseUOM').removeClass('hide');
        } else {
            $('#dvUnitsInsideCarton').addClass('hide');
            $('#divNumberofTraysPerBaseUOM').addClass('hide');
        }

        var charAvailableforProdFormDesc = 40;
        var TBD = '';
        var Brand = '';
        var Season = '';
        var CustomerSpecific = '';
        var NumberOfUnitsInsideOfCarton = '';
        var Count = '';
        var OzWeight = '';
        var CountryCode = '';
        var PkgType = '';
        var Description = '';

        //TBD
        if ($("#ddlTBDIndicator option:selected").text() == 'Yes') {
            TBD = "TBD ";
        }
        //Brand
        var BrandSelection = $("#ddlBrand_Material option:selected").text();
        if (BrandSelection != 'Select...') {
            re = /.*\(([^)]+)\)/;
            Brand = BrandSelection.match(re)[1] + ' ';
        }
        //Season
        if ($("#ddlProductHierarchyLevel1 option:selected").text() == "Seasonal (000000023)") {
            var SeasonSelection = $("#ddlProductHierarchyLevel2 option:selected").text();

            if (SeasonSelection == 'VALENTINE\'S (000000008)' || SeasonSelection == 'VALENTINE\'S (000000008)' || SeasonSelection == 'VALENTINE\'S DAY (000000008)') {
                Season = 'VDY ';
            } else if (SeasonSelection == 'EASTER (000000003)' || SeasonSelection == 'EASTER BULK (000000004)') {
                Season = 'ESR ';
            } else if (SeasonSelection == 'HALLOWEEN (000000005)' || SeasonSelection == 'HALLOWEEN BULK (000000006)') {
                Season = 'HWN ';
            } else if (SeasonSelection == 'CHRISTMAS (000000001)' || SeasonSelection == 'CHRISTMAS BULK (000000002)') {
                Season = 'HLY ';
            } else if (SeasonSelection == 'HOLIDAY (000000001)') {
                Season = 'HLY ';
            } else if (SeasonSelection == 'SUMMER(000000007)') {
                Season = 'SMR ';
            }
        }

        //Customer specific
        if ($("#ddlCustomerSpecific option:selected").text() == "Customer Specific") {
            var CustomerspecificSelection = $("#ddlCustomer option:selected").text();

            if (CustomerspecificSelection != 'Select...') {
                re = /.*\(([^)]+)\)/;
                CustomerSpecific = CustomerspecificSelection.match(re)[1] + ' ';
            }
        }

        //NumberOfUnitsInsideOfCarton
        if ($("#ddlInvolvesCarton option:selected").text() == 'Yes') {
            NumberOfUnitsInsideOfCarton = $("#txtUnitsInsideCarton").val();
            if (NumberOfUnitsInsideOfCarton == "0" || NumberOfUnitsInsideOfCarton == "-9999" || NumberOfUnitsInsideOfCarton == "") {
                NumberOfUnitsInsideOfCarton = '';
            } else if (NumberOfUnitsInsideOfCarton != '') {
                NumberOfUnitsInsideOfCarton = NumberOfUnitsInsideOfCarton + '/';
            }
        } else {
            $("#txtUnitsInsideCarton").val('');
            $("#txtNumberofTraysPerBaseUOM").val('');
        }

        //Count
        var NumberofTraysPerBaseUOM = ($.isNumeric($("#txtNumberofTraysPerBaseUOM").val()) ? parseInt($("#txtNumberofTraysPerBaseUOM").val()) : 0);
        Count = NumberofTraysPerBaseUOM.toString();

        if (Count == "0") {
            var RetailSellingUnitsPerBaseUOM = ($.isNumeric($("#txtRetailSellingUnitsPerBaseUOM").val()) ? parseInt($("#txtRetailSellingUnitsPerBaseUOM").val()) : 0);
            Count = RetailSellingUnitsPerBaseUOM.toString();

            if (Count == "0") {
                Count = '';
            } else if (Count != '') {
                Count = Count + '/';
            }
        } else if (Count != '') {
            Count = Count + '/';
        }

        //Oz Weight
        var OZWeightAccounted = false;
        if ($("#ddlInvolvesCarton option:selected").text() == 'Yes') {
            var dOzWeight = ($.isNumeric($("#txtIndividualPouchWeight").val().replace(',', '')) ? parseFloat($("#txtIndividualPouchWeight").val().replace(',', '')) : 0);
            OzWeight = dOzWeight.toString();
            if (OzWeight == "0") {
                OzWeight = '';
            } else {
                OZWeightAccounted = true;
                var parts = OzWeight.toString().split(".");
                if (parts.length == 2) {
                    if (parts[1].length > 2) {
                        parts[1] = parts[1].substring(0, 2)
                        OzWeight = parts.join(".") + 'oz';
                    } else {
                        OzWeight = OzWeight + 'oz';
                    }
                } else {
                    OzWeight = OzWeight + 'oz';
                }
            }
        } else {
            $("#txtNumberofTraysPerBaseUOM").val('');
            $("#txtUnitsInsideCarton").val('');
            $("#txtIndividualPouchWeight").val('');
        }

        if (!OZWeightAccounted) {
            var dOzWeight = ($.isNumeric($("#txtRetailUnitWeight").val().replace(',', '')) ? parseFloat($("#txtRetailUnitWeight").val().replace(',', '')) : 0);
            OzWeight = dOzWeight.toString();

            if (OzWeight == "0") {
                OzWeight = '';
            } else {
                var parts = OzWeight.toString().split(".");
                if (parts.length > 1) {
                    if (parts[1].length > 2) {
                        parts[1] = parts[1].substring(0, 2)
                        OzWeight = parts.join(".") + 'oz';
                    } else {
                        OzWeight = OzWeight + 'oz';
                    }
                } else {
                    OzWeight = OzWeight + 'oz';
                }
            }
        }

        //Pkg Type
        if ($("#ddlCaseType option:selected").text() == "PALLET") {
            PkgType = ' PLT';
        }
        else {
            var ddlMaterialGroup5 = $("#ddlMaterialGroup5 option:selected").text();
            if (ddlMaterialGroup5.indexOf("(DOY)") != -1) {
                PkgType = ' DOY'
            }
            else if (ddlMaterialGroup5.indexOf("(SHP)") != -1) {
                PkgType = ' SHP'
            }
        }

        CountryCode = getCountryCode();

        if (OzWeight.length == 0) {
            if (NumberOfUnitsInsideOfCarton.length != 0) {
                NumberOfUnitsInsideOfCarton = NumberOfUnitsInsideOfCarton.replace('/', '');
            }
            else {
                if (Count.length != 0) {
                    Count = Count.replace('/', '');
                }
            }
        }

        charAvailableforProdFormDesc = charAvailableforProdFormDesc - TBD.length - Brand.length - Season.length - CustomerSpecific.length - NumberOfUnitsInsideOfCarton.length - Count.length - OzWeight.length - PkgType.length - CountryCode.length;

        //Description
        Description = $("#txtProductFormDescription").val();

        //Calculate text length available for the product form text box
        var mxLength = charAvailableforProdFormDesc - 1;
        if (mxLength < Description.trim().length) {
            $('#lblProductFormDescription').text(0);
            $("#txtProductFormDescription").attr('maxlength', mxLength);
            var trimmedDescription = $("#txtProductFormDescription").val().trim().substring(0, mxLength);
            $("#txtProductFormDescription").val(trimmedDescription);
            Description = trimmedDescription;
        } else {
            var textlen = (mxLength - Description.trim().length);
            $('#lblProductFormDescription').text(textlen);
            $("#txtProductFormDescription").attr('maxlength', mxLength);
        }

        //Update SAP Description
        if (Description != '') {
            Description = Description.toUpperCase() + ' ';
        }

        ProposedItem = TBD + Brand + Season + Description + CustomerSpecific + Count + NumberOfUnitsInsideOfCarton + OzWeight + PkgType + CountryCode;
        ProposedItem = ProposedItem.trim();

        $("#txtSAPItemDescription").val(ProposedItem);

        //Update Project Header title
        var ProjectTitle = $('#lblProjectTitle').text();
        var ProjectTitleArray = ProjectTitle.split(":");
        $('#lblProjectTitle').text(ProjectTitleArray[0] + " : " + ProjectTitleArray[1] + " : " + ProposedItem);

        return mxLength;
    }
    else {
        $('#divProductFormDescription').addClass('hide');
        $('#txtProductFormDescription').val('');
        $('#txtProductFormDescription').removeClass('required');
        $('.spProductFormDescription').addClass('hide');

        $("#lblSAPItemDescription").text('');

        $('#dvUnitsInsideCarton').addClass('hide');
        $('#divNumberofTraysPerBaseUOM').addClass('hide');
        $('#dvInvolvesCarton').addClass('hide');
        $("#ddlInvolvesCarton").val("-1");
        $('#txtUnitsInsideCarton').val('');
        $('#txtIndividualPouchWeight').val('');
        $('#txtNumberofTraysPerBaseUOM').val('');
    }
}

function BindHierarchiesOnLoad() {
    $('.VerifySAPNumbers').each(function (i, obj) {
        dvBOMRow = $(this).closest("div.SAPVerifyItem");
        var drpComponent = dvBOMRow.find('.VerifySAPNumbersType');
        var PHL1 = dvBOMRow.find('.PHL1');
        var PHL2 = dvBOMRow.find('.PHL2');
        var Brand = dvBOMRow.find('.Brand');
        var txtProfitCenterUC = dvBOMRow.find('#txtProfitCenterUC');

        var PHL2Id = dvBOMRow.find('.PHL2').attr('id');
        var BrandId = dvBOMRow.find('.Brand').attr('id');

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
    });
}

function BindPHL2DropDownItemsByPHL1(PHL1) {
    var PHL1Value = $(PHL1).val();
    var dvBOMRow = $(PHL1).closest("div.SAPVerifyItem");
    var PHL2 = dvBOMRow.find('.PHL2');
    var Brand = dvBOMRow.find('.Brand');
    var txtProfitCenterUC = dvBOMRow.find('#txtProfitCenterUC');
    var hdnProfitCenterUC = dvBOMRow.find('#hdnProfitCenterUC');

    PHL2.val("-1");
    Brand.val("-1");
    txtProfitCenterUC.val("");
    hdnProfitCenterUC.val("");

    if (PHL1Value != "" && PHL1Value != "-1") {
        var PHL2Id = dvBOMRow.find('.PHL2').attr('id');
        $("#" + PHL2Id).children("option[class^=" + "PHLOptions" + "]").hide();
        $("#" + PHL2Id).children("option[class$=" + PHL1Value + "]").show();
    }
}

function BindBrandDropDownItemsByPHL2(PHL2) {
    var PHL2Text = $(PHL2).find("option:selected").text();
    var PHL2Value = $(PHL2).val();
    var dvBOMRow = $(PHL2).closest("div.SAPVerifyItem");
    var Brand = dvBOMRow.find('.Brand');
    var txtProfitCenterUC = dvBOMRow.find('#txtProfitCenterUC');
    var hdnProfitCenterUC = dvBOMRow.find('#hdnProfitCenterUC');

    Brand.val("-1");
    txtProfitCenterUC.val("");
    hdnProfitCenterUC.val("");

    if (PHL2Value != "" && PHL2Value != "-1" && PHL2Text != "") {
        var BrandId = dvBOMRow.find('.Brand').attr('id');
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
    var PHL2 = dvBOMRow.find('.PHL2');
    var PHL2Text = $(PHL2).find("option:selected").text();
    var txtProfitCenterUC = dvBOMRow.find('#txtProfitCenterUC');
    var hdnProfitCenterUC = dvBOMRow.find('#hdnProfitCenterUC');

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
function UPCAssociatedChanged() {
    $('.ddlUPCAssociated').each(function (i, obj) {
        var UPCAssociated = $(this).find("option:selected").text();
        var dvBOMRow = $(this).closest("div.SAPVerifyItem");

        if (UPCAssociated.indexOf("Manual Entry") == -1) {
            dvBOMRow.find('.divUPCAssociated').addClass('hideItem');
            dvBOMRow.find('.UPCAssociated').addClass('hideItem');
            dvBOMRow.find('#txtUPCAssociated').val("");
        } else {
            dvBOMRow.find('.divUPCAssociated').removeClass('hideItem');
            dvBOMRow.find('.UPCAssociated').removeClass('hideItem');
        }
    });
}
function showConsumerFacingProdDesc() {
    var visible = false;

    $('.ddlBioEngLabelingRequired').each(function (i, obj) {
        var BioEngLabelingRequired = $(this).find("option:selected").text();
        if (BioEngLabelingRequired.indexOf("Yes – Apply QR Code") != -1 || BioEngLabelingRequired.indexOf("Yes – Apply BE") != -1) {
            visible = true;
        }
    });

    if ($('#hdnBrand').val().toLocaleLowerCase().indexOf("OTG") != -1) {
        visible = true;
    }

    if ($('#hdnProductForm').val().toLocaleLowerCase().indexOf("multiple") != -1 || $('#hdnProductForm').val().toLocaleLowerCase().indexOf("mixes") != -1) {
        visible = true;
    }

    if ($('#hdnPackType').val().toLocaleLowerCase().indexOf("vpc") != -1 || $('#hdnPackType').val().toLocaleLowerCase().indexOf("mxb") != -1) {
        visible = true;
    }

    $("#txtConsumerFacingProdDesc").removeClass("required");
    $("#txtConsumerFacingProdDesc").closest(".form-group").find(".markrequired").remove();

    if (visible) {
        $('divConsumerFacingProdDesc').removeClass('hideItem');
        $("#txtConsumerFacingProdDesc").addClass("required");
        $("#txtConsumerFacingProdDesc").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
    } else {
        $('.divConsumerFacingProdDesc').addClass('hideItem');
        $("#txtConsumerFacingProdDesc").val("");
    }
}

function BioEngLabelingRequiredChanged() {
    $('.ddlBioEngLabelingRequired').each(function (i, obj) {
        var dvBOMRow = $(this).closest("div.SAPVerifyItem");
        var BioEngLabelingRequired = $(this).find("option:selected").text();
        var drpComponentTypeId = $(this).closest('.bomrow').find('.drpComponentType').attr('id');
        var compType = $("#" + drpComponentTypeId + " option:selected").text().toLocaleLowerCase();

        dvBOMRow.find('.BEQRCodeEPSFile').removeClass('hideItem');

        if (compType == "candy semi" || compType == "transfer semi" || compType == "purchased candy semi" || compType.indexOf('finished good') != -1) {
            dvBOMRow.find('.BEQRCodeEPSFile').addClass('hideItem');
        }
        else {
            if (BioEngLabelingRequired.indexOf("Yes – Apply QR Code") == -1) {
                dvBOMRow.find('.BEQRCodeEPSFile').addClass('hideItem');
            }
        }
    });

    showConsumerFacingProdDesc();
}

function ValidateDataONRequestQRCodes() {
    $('#error_message').empty();

    var isValid = true;
    $('.BEQRCRequestEmialRequired').each(function (i, obj) {
        var sd = $(this).closest(".form-group").find('label').text().replace(":", "") + ' is required';
        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if (value == "") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=activateTab(' + '"' + id + '"' + ');setFocusElement(' + '"' + id + '"' + '); >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
            }
        }
        else {
            var value = $(this).val();
            if ((!$(this).parent().parent().hasClass('hideItem') && !$(this).closest(".row").hasClass('hideItem')) && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if (value == '-1') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=activateTab(' + '"' + id + '"' + ');setFocusElement(' + '"' + id + '"' + '); >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
        }
    });

    drpCompType_load();
    if (!isValid) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
        setFocusError();
    }
    return isValid;
}