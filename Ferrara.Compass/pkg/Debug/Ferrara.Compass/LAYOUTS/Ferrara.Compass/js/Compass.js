$(document).ready(function () {
    var url = window.location.href.toLocaleLowerCase();
    if ($('input[type="submit"][value="Submit"]').length && url.indexOf('projectstatus.aspx') == -1 &&
            url.indexOf('itemproposal.aspx') == -1 && url.indexOf('worldsyncrequestupload.aspx') == -1 &&
            url.indexOf('worldsyncrequestreceipt.aspx') == -1 &&
         url.indexOf('stagegate') == -1 && url.indexOf('apppages') == -1) {
        disableButtonCheck();
    }

    if (url.indexOf('stagegatecreateproject.aspx') != -1 || url.indexOf('itemproposal.aspx') != -1) {

        getStageGateProjectDescriptionType($("#ddlProjectType option:selected").text());

        $('.ms-error').each(function (i, obj) {
            if ($(this).html() == "No exact match was found. Click the item(s) that did not resolve for more options." || $(this).html().toUpperCase().indexOf("NA") != -1 || $(this).html().toUpperCase().indexOf("NOT APPLICABLE") != -1 || $(this).html().toUpperCase().indexOf("N/A") != -1) {
                $(this).addClass('hideItem');
            }

            if ($(this).html() == "You must specify a value for this required field.") {
                $(this).addClass('hideItem');
            }

        });

        $('.ms-entity-unresolved').each(function (i, obj) {
            $(this).removeClass('ms-entity-unresolved');
        });

        $(".StageGateNumbers").keypress(function (e) {
            //if the letter is not digit then display error and don't type anything
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });

        $(".StageGateNumbersNA").focusout(function () {
            if ($(this).val().toUpperCase() == "N") {
                $(".StageGateNumbersNA").val("NA");
            }
        });

        $(".StageGateNumbersNA").keypress(function (e) {
            //if the letter is not digit then display error and don't type anything
            //65 A, 97 a, 78 N, 110 n
            if (e.which == 78 || e.which == 110) {
                if ($(".StageGateNumbersNA").val() == '') {
                    $(".StageGateNumbersNA").val("NA");
                }
                else {
                    return false;
                }
            }
            else if (e.which == 65 || e.which == 97) {
                if ($(".StageGateNumbersNA").val().toUpperCase() == 'N') {
                    $(".StageGateNumbersNA").val("NA");
                }
                else {
                    return false;
                }
            }
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
            else {
                if ($(".StageGateNumbersNA").val().toUpperCase() == 'N' || $(".StageGateNumbersNA").val().toUpperCase() == 'NA') {
                    return false;
                }
            }
        });

        $('.MembersTable').each(function (i, obj) {
            // $(this).find('.DeleteRow').first().remove();
            $(this).find('.DeleteRow').first().addClass('hide');
        });

        $('.ReadOnlyMembers').each(function (i, obj) {
            $(this).prop("readonly", true);
        });

        $('[id^="hdnDeletedStatusFor"]').each(function (i, obj) {
            if ($(this).val() == "true") {
                $(this).closest("tr").addClass("hideItem");
            };
        });
    }

    $(".ParnetProjectNotesDrillDown").addClass("collapseProcess20X20");

    $(".ParnetProjectNotesDrillDown").on("click", function () {
        var clicked = $(this);
        if (clicked.hasClass("collapseProcess20X20")) {
            $('.divProjectNotesParent').addClass('hide');
            clicked.removeClass("collapseProcess20X20");
            clicked.addClass("expandProcess20X20");
        }
        else if (clicked.hasClass("expandProcess20X20")) {
            $('.divProjectNotesParent').removeClass('hide');
            clicked.removeClass("expandProcess20X20");
            clicked.addClass("collapseProcess20X20");
        }
    });

    ClearDropdwonStyles();
});
function getStageGateProjectDescriptionType(projectTypeName) {
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
function updatePeopleEditors() {
    $('.MembersTable').each(function (i, obj) {
        //$(this).find('.DeleteRow').first().remove();
        $(this).find('.DeleteRow').first().addClass('hide');
    });

    $('.ReadOnlyMembers').each(function (i, obj) {
        $(this).prop("readonly", true);
    });

    $('[id^="hdnDeletedStatusFor"]').each(function (i, obj) {
        if ($(this).val() == "true") {
            $(this).closest("tr").addClass("hideItem");
        };
    });

    $('.ms-error').each(function (i, obj) {
        if ($(this).html() == "No exact match was found. Click the item(s) that did not resolve for more options." || $(this).html().toUpperCase().indexOf("NA") != -1 || $(this).html().toUpperCase().indexOf("NOT APPLICABLE") != -1 || $(this).html().toUpperCase().indexOf("N/A") != -1) {
            $(this).addClass('hideItem');
        }

        if ($(this).html() == "You must specify a value for this required field.") {
            $(this).addClass('hideItem');
        }

    });

    $('.ms-entity-unresolved').each(function (i, obj) {
        $(this).removeClass('ms-entity-unresolved');
    });
}
function PCSRequirements() {
    var PSSec = $("#OpsPS");
    PSSec.find(".markrequired").hide();
    PSSec.find(".BOMrequired").removeClass("BOMrequired");
    PSSec.find(".materialDescription").addClass("BOMrequired")
    //PSSec.find("#dvTSSPKChange").addClass("hideItem");

    PSSec.find(".descriptionmark").show();
    var TSSec = $("#OpsTS");
    //TSSec.find("#dvTSSPKChange").addClass("hideItem");
    TSSec.find(".NewExisting").each(function () {
        var anchor = $("#" + this.id);
        var newExisting = $("#" + this.id + " option:selected").text();
        if (newExisting == 'New') {
            anchor.closest(".semiRow").find(".markrequired").show();
            anchor.closest(".semiRow").find(".EXISTINGBOM").addClass("BOMrequired");
        }
        if (newExisting == "Network Move") {
            anchor.closest(".semiRow").find(".markrequired").show();
            anchor.closest(".semiRow").find(".EXISTINGBOM").addClass("BOMrequired");
            // anchor.closest(".semiRow").find("#dvTSSPKChange").removeClass("hideItem");
        }
        if (newExisting == 'Existing') {
            anchor.closest(".semiRow").find(".markrequired").hide();
            anchor.closest(".semiRow").find(".BOMrequired").addClass("EXISTINGBOM").removeClass("BOMrequired");
        }
        anchor.closest(".semiRow").find(".TSRequired").addClass("BOMrequired");
        anchor.closest(".semiRow").find(".TSRequiredMark").show();
    });
    PSSec.find(".NewExisting").each(function () {
        var anchor = $("#" + this.id);
        var newExisting = $("#" + this.id + " option:selected").text();
        if (newExisting == "Network Move") {
            //anchor.closest(".semiRow").find("#dvTSSPKChange").removeClass("hideItem");
        }
    });

    var ExtPCSSec = $("#ExtPS");
    ExtPCSSec.find("#dvTSSPKChange").addClass("hideItem");
    ExtPCSSec.find(".NewExisting").each(function () {
        var anchor = $("#" + this.id);
        var newExisting = $("#" + this.id + " option:selected").text();
        if (newExisting == 'Existing') {
            anchor.closest(".semiRow").find(".LikeMaterial").closest(".form-group").find(".markrequired").hide();
            anchor.closest(".semiRow").find(".LikeDescription").closest(".form-group").find(".markrequired").hide();
            anchor.closest(".semiRow").find(".LikeMaterial").removeClass("BOMrequired");
            anchor.closest(".semiRow").find(".LikeDescription").removeClass("BOMrequired");
        } else if (newExisting == 'Network Move') {
            anchor.closest(".semiRow").find(".LikeMaterial").closest(".form-group").find(".markrequired").show();
            anchor.closest(".semiRow").find(".LikeMaterial").addClass("BOMrequired");
            anchor.closest(".semiRow").find(".LikeDescription").closest(".form-group").find(".markrequired").show();
            anchor.closest(".semiRow").find(".LikeDescription").addClass("BOMrequired");
            anchor.closest(".semiRow").find("#dvTSSPKChange").removeClass("hideItem");
        } else {
            anchor.closest(".semiRow").find(".LikeMaterial").closest(".form-group").find(".markrequired").show();
            anchor.closest(".semiRow").find(".LikeMaterial").addClass("BOMrequired");
            anchor.closest(".semiRow").find(".LikeDescription").closest(".form-group").find(".markrequired").show();
            anchor.closest(".semiRow").find(".LikeDescription").addClass("BOMrequired");
        }
    });
}

function deletePackagingItem(clicked) {
    var button = $(clicked);
    var parentBomRow = $(clicked).closest(".bomrow");
    var MaterialTypeArray = ["Transfer Semi", "Purchased Candy Semi"]
    var hdnCompassListItemId = parentBomRow.find("#hdnCompassListItemId").val();
    var hdnItemID = parentBomRow.find("#hdnItemID").val();
    var compDescID = parentBomRow.find(".DescriptionClass").attr("id");
    var ComponentType = parentBomRow.find(".VerifySAPNumbersType").find("option:selected").text();
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
                    button.closest(".row").append("<div class='noResultsRow'>Cannot delete this components as it has child elements.</div>");
                    HasChildItems = true;
                }
                else {
                    HasChildItems = false;
                }
            },
            complete: function () {
                loadingIconAdded = true;
                parentBomRow.find(".disablingLoadingIcon").remove();

                if (HasChildItems) {
                    button.focus();
                    button.blur(function () {
                        $(".noResultsRow").remove();
                    });
                }
                else {
                    parentBomRow.find(".form-group").parent().addClass("hideItem");
                    parentBomRow.addClass("hideItem");
                    parentBomRow.find("#hdnDeletedStatus").val("deleted");
                }

                //return !HasChildItems;
            }
        });
    }
    else {
        loadingIconAdded = true;
        parentBomRow.find(".disablingLoadingIcon").remove();
        parentBomRow.find(".form-group").parent().addClass("hideItem");
        parentBomRow.addClass("hideItem");
        parentBomRow.find("#hdnDeletedStatus").val("deleted");
        //return true;
    }
}

function getRPTCompDescription(clicked) {
    var button = $(clicked);
    var enteredText = button.closest(".row").find(".NumberClass").val();
    var compDescID = button.closest(".row").find(".DescriptionClass").attr("id");
    var SAPNumberFound = false;

    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP BOM List')/items?$select=ID,MaterialDescription&$filter=(MaterialNumber eq '" + enteredText + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            if (finding === undefined) {
                SAPNumberFound = false;
            } else {
                button.closest(".row").find(".noResultsRow").remove();
                $("#" + compDescID).val(finding.MaterialDescription);
                SAPNumberFound = true;
            }
        },
        complete: function () {
            if (SAPNumberFound) {
                button.closest(".row").find(".disablingLoadingIcon").remove();
                $("#" + compDescID).focus();
                $("#" + compDescID).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
            else {
                $.ajax({
                    url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,SAPItemNumber,CandySemiNumber&$filter=(SAPItemNumber eq '" + enteredText + "')&$top=500",
                    method: 'GET',
                    dataType: 'json',
                    headers: {
                        Accept: "application/json;odata=verbose"
                    },
                    success: function (results) {
                        var finding = results.d.results[0];
                        if (finding === undefined) {
                            button.closest(".row").append("<div class='noResultsRow'>Lookup returned no results.</div>");
                        } else {
                            button.closest(".row").find(".noResultsRow").remove();
                            $("#" + compDescID).val(finding.SAPDescription);
                        }
                    },
                    complete: function () {
                        button.closest(".row").find(".disablingLoadingIcon").remove();
                        $("#" + compDescID).focus();
                        $("#" + compDescID).blur(function () {
                            $(".noResultsRow").remove();
                        });
                    }
                });
            }
        }
    });
}

function getCompDescriptionBySAPBOMList(lookupValue, descriptionID) {
    var enteredText = $("#" + lookupValue).val();
    var SAPNumberFound = false;

    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP BOM List')/items?$select=ID,MaterialDescription&$filter=(MaterialNumber eq '" + enteredText + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            if (finding === undefined) {
                SAPNumberFound = false;
            } else {
                $("#" + lookupValue).closest(".row").find(".noResultsRow").remove();
                $("#" + descriptionID).val(finding.MaterialDescription);
                SAPNumberFound = true;
            }
        },
        complete: function () {
            if (SAPNumberFound) {
                $("#" + lookupValue).closest(".row").find(".disablingLoadingIcon").remove();
                $("#" + descriptionID).focus();
                $("#" + descriptionID).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
            else {
                $.ajax({
                    url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,SAPItemNumber,CandySemiNumber&$filter=(SAPItemNumber eq '" + enteredText + "')&$top=500",
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
                        }
                    },
                    complete: function () {
                        $("#" + lookupValue).closest(".row").find(".disablingLoadingIcon").remove();
                        $("#" + descriptionID).focus();
                        $("#" + descriptionID).blur(function () {
                            $(".noResultsRow").remove();
                        });
                    }
                });
            }
        }
    });
}
function getPackagingComponentSpecificationNumberFromPLM(MaterialNumberId, SpecNoId) {
    var MaterialNumber = $("#" + MaterialNumberId).val();

    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('PLM Specifications List')/items?$select=ID,Specification,Status&$filter=(Title eq '" + MaterialNumber + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            if (finding === undefined) {
                $("#" + SpecNoId).val("No Specification Found");
            } else if (results.d.results.length > 1) {
                $("#" + SpecNoId).val("Multiple Specificiations Found");
            } else {
                if (finding.Status === "600") {
                    $("#" + SpecNoId).val(finding.Specification);
                } else {
                    $("#" + SpecNoId).val("Specification is not in Released Status");
                }
            }
            $("#" + SpecNoId).closest(".row").find(".disablingLoadingIcon").remove();
        }
    });
}
function getCompDescriptionByMaterialMasterList(numberId, descriptionID) {
    var enteredText = $("#" + numberId).val();
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,SAPItemNumber,CandySemiNumber&$filter=(SAPItemNumber eq '" + enteredText + "' or CandySemiNumber eq '" + enteredText + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            if (finding === undefined) {
                $("#" + numberId).closest(".row").append("<div class='noResultsRow'>Lookup returned no results.</div>");
            } else {
                $("#" + numberId).closest(".row").find(".noResultsRow").remove();
                $("#" + descriptionID).val(finding.SAPDescription);
            }
            $("#" + numberId).closest(".row").find(".disablingLoadingIcon").remove();
            $("#" + descriptionID).focus();
        },
        complete: function () {
            $("#" + descriptionID).blur(function () {
                $(".noResultsRow").remove();
            });
        }
    });
}
function getCompDescriptionReturn(lookupValue, descriptionID) {
    if (($("#ddlTBDIndicator option:selected").val() == "N" && lookupValue == "txtSAPItemNumber") ||
        ($("#ddlTBDIndicator option:selected").val() == "Y" && lookupValue == "txtLikeFGItemNumber")) {
        return true;
    } else {
        var enteredText = $("#" + lookupValue).val();
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
                    $("#" + lookupValue).closest(".row").append("<div class='noResultsRow' style=\"margin-left: 460px;\">Lookup returned no results.</div>");
                } else {
                    $("#" + lookupValue).closest(".row").find(".noResultsRow").remove();
                    if ($('#hdnNewIPF').val() == "Yes" && $("#ddlTBDIndicator option:selected").val() == "Y" && lookupValue == "txtSAPItemNumber") {
                        $("#lblSAPItemDescription").text(finding.SAPDescription);
                    }
                    else {
                        $("#" + descriptionID).val(finding.SAPDescription);
                    }
                }
                $("#" + lookupValue).closest(".row").find(".disablingLoadingIcon").remove();
                $("#" + descriptionID).focus();
                $("#" + descriptionID).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
        });
        return false;
    }
}

function getCompDescription(lookupValue, descriptionID) {
    var enteredText = $("#" + lookupValue).val();
    var SAPNumberFound = false;

    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP BOM List')/items?$select=ID,MaterialDescription&$filter=(MaterialNumber eq '" + enteredText + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            if (finding === undefined) {
                SAPNumberFound = false;
            } else {
                $("#" + lookupValue).closest(".row").find(".noResultsRow").remove();
                $("#" + descriptionID).val(finding.MaterialDescription);
                SAPNumberFound = true;
            }
        },
        complete: function () {
            if (SAPNumberFound) {
                $("#" + lookupValue).closest(".row").find(".disablingLoadingIcon").remove();
                $("#" + descriptionID).focus();
                $("#" + descriptionID).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
            else {
                $.ajax({
                    url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,SAPItemNumber,CandySemiNumber&$filter=(SAPItemNumber eq '" + enteredText + "')&$top=500",
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
                        }
                    },
                    complete: function () {
                        $("#" + lookupValue).closest(".row").find(".disablingLoadingIcon").remove();
                        $("#" + descriptionID).focus();
                        $("#" + descriptionID).blur(function () {
                            $(".noResultsRow").remove();
                        });
                    }
                });
            }
        }
    });
}

function getTableCompDescription(clicked, numberClass, descriptionClass) {
    var button = $(clicked);
    var enteredID = button.closest("tr").find("." + numberClass).attr("id");
    var enteredText = button.closest("tr").find("." + numberClass).val();
    var compDescID = button.closest("tr").find("." + descriptionClass).attr("id");
    var SAPNumberFound = false;
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP BOM List')/items?$select=ID,MaterialDescription&$filter=(MaterialNumber eq '" + enteredText + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            if (finding === undefined) {
                SAPNumberFound = false;
            } else {
                button.closest("table").find(".noResultsRow").remove();
                $("#" + compDescID).val(finding.MaterialDescription);
                SAPNumberFound = false;
            }
        },
        complete: function () {
            if (SAPNumberFound) {
                button.closest("tr").find(".disablingLoadingIcon").remove();
                $("#" + compDescID).focus();
                $("#" + compDescID).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
            else {
                $.ajax({
                    url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,SAPItemNumber,CandySemiNumber&$filter=(SAPItemNumber eq '" + enteredText + "')&$top=500",
                    method: 'GET',
                    dataType: 'json',
                    headers: {
                        Accept: "application/json;odata=verbose"
                    },
                    success: function (results) {
                        var finding = results.d.results[0];
                        if (finding === undefined) {
                            $("<tr><td colspan='3'><div class='noResultsRow'>Lookup returned no results.</div></td></tr>").insertAfter($("#" + compDescID).closest("tr"));
                        } else {
                            button.closest("table").find(".noResultsRow").remove();
                            $("#" + compDescID).val(finding.SAPDescription);
                        }
                    },
                    complete: function () {
                        button.closest("tr").find(".disablingLoadingIcon").remove();
                        $("#" + compDescID).focus();
                        $("#" + compDescID).blur(function () {
                            $(".noResultsRow").remove();
                        });
                    }
                });
            }
        }
    });
}

function getCandySemiDescription(clicked, numberClass, descriptionClass) {
    var button = $(clicked);

    var enteredText = button.closest(".row").find("." + numberClass).val();
    var bgcolor = button.closest(".row").css("background-color");
    var SAPNumberFound = false;

    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP BOM List')/items?$select=ID,MaterialDescription&$filter=(MaterialNumber eq '" + enteredText + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            if (finding === undefined) {
                SAPNumberFound = false;
            } else {
                button.closest(".row").find(".noResultsRow").remove();
                button.closest(".row").find("." + descriptionClass).val(finding.MaterialDescription);
                SAPNumberFound = true;
            }
        },
        complete: function () {
            if (SAPNumberFound) {
                button.closest(".row").find(".disablingLoadingIcon").remove();
                button.closest(".row").find("." + descriptionClass).focus();
                button.closest(".row").find("." + descriptionClass).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
            else {
                $.ajax({
                    url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,SAPItemNumber,CandySemiNumber&$filter=(SAPItemNumber eq '" + enteredText + "')&$top=500",
                    method: 'GET',
                    dataType: 'json',
                    headers: {
                        Accept: "application/json;odata=verbose"
                    },
                    success: function (results) {
                        var finding = results.d.results[0];
                        if (finding === undefined) {
                            $("<div class='noResultsRow row' style='background-color:" + bgcolor + "'\;>Lookup returned no results.</div>").insertAfter(button.closest(".row"));
                        } else {
                            button.closest(".row").find(".noResultsRow").remove();
                            button.closest(".row").find("." + descriptionClass).val(finding.SAPDescription);
                        }
                    },
                    complete: function () {
                        button.closest(".row").find(".disablingLoadingIcon").remove();
                        button.closest(".row").find("." + descriptionClass).focus();
                        button.closest(".row").find("." + descriptionClass).blur(function () {
                            $(".noResultsRow").remove();
                        });
                    }
                });
            }
        }
    });
}

var headerHTML = $("h1:eq(1)").closest(".row").html();

var loadingIconAdded = false;

if ($("h1:eq(1)").closest(".row").length > 0 && $('.PageSubHeader').length > 0) {
    var rowIndex = $("h1:eq(1)").closest(".row").index();
    rowIndex++;
    $("h1:eq(1)").closest(".row").parent().find(".row:eq(" + rowIndex + ")").addClass("leftPaddingFifteen");
    $("h1:eq(1)").closest(".row").remove();
    $('.PageSubHeader .control-label').addClass("leftPaddingFifteen");
    $('.PageSubHeader:first').prepend(headerHTML);
}

function checkNumberLength(ctrlId, lengthReq) {
    $('#error_message').empty();
    var isvalid = true;
    if ($("#" + ctrlId).length) {
        if ($("#" + ctrlId).val().length < lengthReq) {
            $("#dverror_message").show();
            $('#error_message').append('<li class="errorMessage" >' + 'Please enter at least ' + lengthReq + ' digits' + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + ctrlId + '"' + ') >Update</a>   </li></br>');
            loadingIconAdded = true;
            $(".disablingLoadingIcon").remove();
            isvalid = false;
        }
    }
    return isvalid;
}

function getPageName(url) {

    var index = url.lastIndexOf("/") + 1;
    var filenameWithExtension = url.substr(index);
    var filename = filenameWithExtension.split(".")[0];
    filename = filename.split("?")[0];
    filename = filename.toLocaleLowerCase();
    return filename;
}

function getProjectNo(url) {
    var projectNo = url.split('ProjectNo=');
    var finalProjectNo = projectNo[1];
    if (projectNo[1].indexOf("&") != -1) {
        finalProjectNo = projectNo[1].split("&")[0]
    }
    return finalProjectNo;
}

function disableButtonCheck() {
    var url = window.location.href;
    var wfStepPages = {
        itemproposal: "IPF",
        initialapprovalreview: "SrOBMApproval",
        tradepromogroup: "TradePromo",
        estpricing: "EstPricing",
        estbracketpricing: "EstBracketPricing",
        distribution: "Distribution",
        ops: "Operations",
        externalmfg: "ExternalMfg",
        initialcapacityreview: "InitialCapacity",
        initialcostingreview: "InitialCosting",
        sapinitialitemsetup: "SAPInitialSetup",
        prelimsapinitialitemsetup: "PrelimSAPInitialSetup",
        qa: "QA",
        obmfirstreview: "OBMReview1",
        bomsetuppe: "BOMSetupPE",
        bomsetupproc: "BOMSetupProc",
        bomsetupmaterialwarehouse: "BOMSetupMaterialWarehouse",
        bomsetuppe2: "BOMSetupPE2",
        bomsetuppe3: "BOMSetupPE3",
        sapbomsetup: "SAPBOMSetup",
        bomsetupsap: "SAPBOMSetup",
        secondaryapprovalreview: "SrOBMApproval2",
        obmsecondreview: "OBMReview2",
        finishedgoodpackspec: "FGPackSpec",
        sapcompleteitemsetup: "SAPCompleteItemSetup",
        graphicsrequest: "GRAPHICS",
        graphicsrequest_new: "GRAPHICS",
        materialsreceivedcheck: "MaterialsRcvdChk",
        firstproductioncheck: "FirstProductionChk",
        distributioncentercheck: "DistributionChk",
        componentcosting: "ComponentCostingCorrugatedPaperboard",
        pe1: "BOMSetupPE",
        pe2: "BOMSetupPE2",
        pe3: "BOMSetupPE3",
        proc: "BOMSetupProc",
        beqrc: "BEQRC"
    };
    var pageName = getPageName(url);
    var projectNo = getProjectNo(url);
    var currentWFStep = wfStepPages[pageName];
    var userId = _spPageContextInfo.userId;
    var projectNoHeader = $("#hdnProjectNumberHeader").val();
    var message = "You are not able to submit this form because there is no current workflow task assigned.";
    if ($("#hddOBMAdmin").val() == "true")
        disableButtonCheckAdmin(message, projectNo, currentWFStep);
    else
        if ($("#hddPackingEngineer").val() == "true" && (pageName == "bomsetuppe2" || pageName == "finishedgoodpackspec"))
            disableButtonCheckSpecificWorkflowPhase(message, projectNo, currentWFStep, 2);
        else
            if ($("#hddBrandManager").val() == "true" && pageName == "itemproposal")
                disableButtonCheckSpecificWorkflowPhase(message, projectNo, currentWFStep, 1);
            else
                disableButtonCheckOthers(message, userId);
}

function disableButtonCheckSpecificWorkflowPhase(message, projectNo, currentWFStep, workflowListId) {
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Workflow%20Tasks%20" + workflowListId + "')/items?$select=FormUrl,Status,ProjectNumber,WorkflowStep&$filter=(ProjectNumber eq '" + projectNo + "') and (WorkflowStep eq '" + currentWFStep + "')&$top=500",
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
            if (results.length <= 0) {
                var button = $('input[type="submit"][value="Submit"]');
                button.addClass("disabled");
                button.attr("disabled", "disabled");
                button.closest(".row").after("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><span class='SuccessMessage justifyRight'>" + message + "</span></div></div>");
            } else {
                var canSubmit = false;
                $.each(results, function (index, element) {
                    if (element.Status == "Completed" || element == null) {
                        return;
                    } else {
                        canSubmit = true;
                        return false;
                    }
                });
                if (!canSubmit) {
                    var button = $('input[type="submit"][value="Submit"]');
                    button.addClass("disabled");
                    button.attr("disabled", "disabled");
                    button.closest(".row").after("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><span class='SuccessMessage justifyRight'>" + message + "</span></div></div>");
                }
            }
        }
    });
}

function disableButtonCheckOthers(message, userId) {
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Workflow%20Tasks%201')/items?$select=FormUrl,Status,WorkflowStep,AssignedToId&$filter=(AssignedTo/Id eq '" + userId + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results1) {
            $.ajax({
                url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Workflow%20Tasks%202')/items?$select=FormUrl,Status,WorkflowStep,AssignedToId&$filter=(AssignedTo/Id eq '" + userId + "')&$top=500",
                method: 'GET',
                dataType: 'json',
                headers: {
                    Accept: "application/json;odata=verbose"
                },
                success: function (results2) {
                    $.ajax({
                        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Workflow%20Tasks%203')/items?$select=FormUrl,Status,WorkflowStep,AssignedToId&$filter=(AssignedTo/Id eq '" + userId + "')&$top=500",
                        method: 'GET',
                        dataType: 'json',
                        headers: {
                            Accept: "application/json;odata=verbose"
                        },
                        success: function (results3) {
                            var results = new Array();
                            $.each(results1.d.results, function (index, element) {
                                results.push(element);
                            });
                            $.each(results2.d.results, function (index, element) {
                                results.push(element);
                            });
                            $.each(results3.d.results, function (index, element) {
                                results.push(element);
                            });
                            if (results.length <= 0) {
                                var button = $('input[type="submit"][value="Submit"]');
                                button.addClass("disabled");
                                button.attr("disabled", "disabled");
                                button.closest(".row").after("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><span class='SuccessMessage justifyRight'>" + message + "</span></div></div>");
                            } else {
                                var url = window.location.href.toLocaleLowerCase();
                                var canSubmit = false;
                                var allTasks = "";
                                if (url.indexOf("/pe1.aspx") != -1) {
                                    url = url.replace("/pe1.aspx", "/bomsetuppe.aspx")
                                } else if (url.indexOf("/pe2.aspx") != -1) {
                                    url = url.replace("/pe2.aspx", "/bomsetuppe2.aspx")
                                } else if (url.indexOf("/proc.aspx") != -1) {
                                    url = url.replace("/proc.aspx", "/bomsetupproc.aspx")
                                }
                                else if (url.indexOf("/bomsetupsap.aspx") != -1) {
                                    url = url.replace("/bomsetupsap.aspx", "/sapbomsetup.aspx")
                                }
                                else if (url.indexOf("/graphicsrequest_new.aspx") != -1) {
                                    url = url.replace("/graphicsrequest_new.aspx", "/graphicsrequest.aspx")
                                }
                                $.each(results, function (index, element) {
                                    var activeTasksProc = element.WorkflowStep.toLocaleLowerCase();
                                    if (element.FormUrl != null) {
                                        var formURLOG = element.FormUrl.toLocaleLowerCase();
                                        var formURL = formURLOG.replace(/\\/g, "/");
                                        if (formURL == url) {
                                            if (activeTasksProc.indexOf("bomsetupproc") != -1 && allTasks.indexOf(element.WorkflowStep) == -1) {
                                                if (element.Status != "Completed" && element != null) {
                                                    allTasks = element.WorkflowStep + ";" + allTasks;
                                                }
                                            }
                                        }
                                    }
                                });
                                if ($("#hdnWorkflowSteps").length) {
                                    $("#hdnWorkflowSteps").val(allTasks);
                                }
                                $.each(results, function (index, element) {
                                    if (element.Status == "Completed" || element == null) {
                                        return;
                                    }
                                    if (element.FormUrl != null) {
                                        var formURLOG = element.FormUrl.toLocaleLowerCase();
                                        var formURL = formURLOG.replace(/\\/g, "/");
                                        if (formURL == url) {
                                            canSubmit = true;
                                        }
                                        if (!(url.indexOf('componentcosting.aspx') == -1) && !(formURL.indexOf("componentcostingsummary") == -1)) {
                                            canSubmit = true;
                                        }
                                    }
                                });
                                if (!canSubmit) {
                                    var button = $('input[type="submit"][value="Submit"]');
                                    button.addClass("disabled");
                                    button.attr("disabled", "disabled");
                                    button.closest(".row").after("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><span class='SuccessMessage justifyRight'>" + message + "</span></div></div>");
                                }
                            }
                        }
                    });
                }
            });
        }
    });
}

function disableButtonCheckAdmin(message, projectNo, currentWFStep) {
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Workflow%20Tasks%201')/items?$select=FormUrl,Status,ProjectNumber,WorkflowStep&$filter=(ProjectNumber eq '" + projectNo + "') and startswith(WorkflowStep,'" + currentWFStep + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results1) {
            $.ajax({
                url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Workflow%20Tasks%202')/items?$select=FormUrl,Status,ProjectNumber,WorkflowStep&$filter=(ProjectNumber eq '" + projectNo + "') and startswith(WorkflowStep,'" + currentWFStep + "')&$top=500",
                method: 'GET',
                dataType: 'json',
                headers: {
                    Accept: "application/json;odata=verbose"
                },
                success: function (results2) {
                    $.ajax({
                        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Workflow%20Tasks%203')/items?$select=FormUrl,Status,ProjectNumber,WorkflowStep&$filter=(ProjectNumber eq '" + projectNo + "') and startswith(WorkflowStep,'" + currentWFStep + "')&$top=500",
                        method: 'GET',
                        dataType: 'json',
                        headers: {
                            Accept: "application/json;odata=verbose"
                        },
                        success: function (results3) {
                            var results = new Array();
                            $.each(results1.d.results, function (index, element) {
                                results.push(element);
                            });
                            $.each(results2.d.results, function (index, element) {
                                results.push(element);
                            });
                            $.each(results3.d.results, function (index, element) {
                                results.push(element);
                            });
                            if (results.length <= 0) {
                                var button = $('input[type="submit"][value="Submit"]');
                                button.addClass("disabled");
                                button.attr("disabled", "disabled");
                                button.closest(".row").after("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><span class='SuccessMessage justifyRight'>" + message + "</span></div></div>");
                            } else {
                                var canSubmit = false;
                                var allTasks = "";
                                $.each(results, function (index, element) {
                                    var activeTasksProc = element.WorkflowStep.toLocaleLowerCase();
                                    if (activeTasksProc.indexOf("bomsetupproc") != -1) {
                                        if (element.Status != "Completed" && element != null) {
                                            allTasks = element.WorkflowStep + ";" + allTasks;
                                        }
                                    }
                                });
                                if ($("#hdnWorkflowSteps").length) {
                                    $("#hdnWorkflowSteps").val(allTasks);
                                }

                                $.each(results, function (index, element) {
                                    if (element.Status == "Completed" || element == null) {
                                        return;
                                    } else {
                                        canSubmit = true;
                                        return false;
                                    }
                                });
                                if (!canSubmit) {
                                    var button = $('input[type="submit"][value="Submit"]');
                                    button.addClass("disabled");
                                    button.attr("disabled", "disabled");
                                    button.closest(".row").after("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><span class='SuccessMessage justifyRight'>" + message + "</span></div></div>");
                                }
                            }
                        }
                    });
                }
            });
        }
    });
}

function getPackagingComponents(projectNo) {
    var packagingNumbers = "";
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Project%20Decisions%20List')/items?$select=NewPackagingComponents,Title&$filter=(Title eq '" + projectNo + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            packagingNumbers = finding.NewPackagingComponents;
        }
    });

    return packagingNumbers;
}

function GetAllPackagingItemsForProject() {
    var BOMItems = [];
    try {
        CompassListItemId = $('#hdnCompassListItemId').val();
        //alert(CompassListItemId);
        $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20Packaging%20Item%20List')/items?$select=MaterialNumber,PackagingComponent,NewExisting,ParentID,Deleted&$filter=(CompassListItemId eq '" + CompassListItemId + "')&$top=500",
            method: 'GET',
            async: false,
            dataType: 'json',
            headers: {
                Accept: "application/json;odata=verbose"
            },
            success: function (results) {
                $.each(results.d.results, function (index, item) {
                    if (item.Deleted != 'Yes') {
                        BOMItems.push([item.MaterialNumber, item.PackagingComponent, item.NewExisting, "", "Not Found"]);
                    }
                });
            }
        });
    }
    catch (err) {
        alert("Error in verifying Component type against SAP. Please try again.");
    }
    finally {
        return BOMItems;
    }
}

function getRevisedFirstShip(projectNo) {
    var RevisedFirstShipDate = "";
    $.ajax({
        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass%20List')/items?$select=RevisedFirstShipDate,Title&$filter=(Title eq '" + projectNo + "')&$top=500",
        method: 'GET',
        dataType: 'json',
        headers: {
            Accept: "application/json;odata=verbose"
        },
        success: function (results) {
            var finding = results.d.results[0];
            RevisedFirstShipDate = finding.RevisedFirstShipDate;
        }
    });

    return RevisedFirstShipDate;
}

function makeTable(PackagingNumbers) {
    var $dataWrapper = $("<div>");
    var code = "<table><tr><td>" + PackagingNumbers + "</td></tr></table>";

    var lastSemi = code.lastIndexOf('; ');
    code = code.substring(0, lastSemi);

    code = code.replace(/,([^;]*)$/, ' $1')
    code = code.replace(new RegExp("; ", 'g'), "</td></tr><tr><td>");
    code = code.replace(new RegExp(": ", 'g'), "</td><td>");
    $dataWrapper.html(code);   //wrap up the data to convert in a div.

    $dataWrapper.find('table').css("width", "300px");
    $dataWrapper.find('td').css("padding", "4px");
    return $dataWrapper.html();
}

function stripColumns(code) {
    var $dataWrapper = $("<div>");
    $dataWrapper.html(code);   //wrap up the data to convert in a div.

    //for each column in container element
    $dataWrapper.find('tr:eq(0)').each(function () {
        $(this).remove();
    });
    $dataWrapper.find('tr').each(function () {

        $(this).find('td:eq(3)').remove();
        $(this).find('td:eq(2)').remove();
    });
    $dataWrapper.find('table').css("width", "300px");
    $dataWrapper.find('td').css("padding", "4px");
    return $dataWrapper.html();
}

function setFocus(sender) {

    var id = jQuery("[id$=_" + sender + "]").attr("id");
    if (id == null)
        id = sender;
    setFocusElement(id);
}

function setFocusElement(sender) {
    var url = window.location.href;

    if (url.indexOf('ItemProposal.aspx') != -1) {
        var errorID = $("#" + sender).attr("id");
        GotoStepAndFocus(errorID);
    } else {
        if (document.getElementById(sender)) {
            // document.getElementById(sender).style.backgroundColor = 'Pink';
            $("#" + sender).addClass('highlightElement');
            document.getElementById(sender).focus();
            $('html, body').animate({
                scrollTop: $("#" + sender).offset().top - 100
            }, 1000);
        }
    }
}

function setFocusElementRepeater(sender) {
    var url = window.location.href;
    $("[name='" + sender + "']").addClass('highlightElement');
    $("[name='" + sender + "']").focus();
    $('html, body').animate({
        scrollTop: $("[name='" + sender + "']").offset().top - 100
    }, 1000);

}

function setFocusError() {
    $('html, body').animate({
        scrollTop: $("h1").offset().top - 100
    }, 1000);
}

function calculateTotal(value1, value2, ctrl) {

    if (isNaN(value1) || value1.length == 0) {
        value1 = 0;
    }
    if (isNaN(value2) || value2.length == 0) {
        value2 = 0;
    }

    var num = value1 * value2;
    $(ctrl).text(num.toFixed(2));
}

function ValidateData() {
    var dvMain, label, fieldName, requiredMsg;
    var isValid = true;

    $('#error_message').empty();

    $('.required').each(function (i, obj) {
        dvMain = $(this).closest("div.form-group");
        label = dvMain.find('label');
        if (label.length)
            fieldName = label.text().replace(":", "");
        else
            fieldName = $(this).attr('title');
        requiredMsg = fieldName + ' is required';

        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == "") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        }
        else {
            var value = $(this).val();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == '-1') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
        }
    });

    $('.requiredNeedsNew').each(function (i, obj) {
        var dvMain = $(this).closest("div.form-group");
        var sd = dvMain.find('label').text().replace(":", "") + ' is required';
        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value.toLowerCase() == "needs new" || value == '') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        }
    });

    $('.minimumlength').each(function (i, obj) {
        var sd = $(this).parent().find('label').text().replace(":", "") + ' must be at least 5 digits';
        //var sd = 'Please enter at least 5 digits';
        var id = $(this).attr('id');
        var dvMain = $(this).closest("div.repeater");
        if (!dvMain.hasClass('hideItem')) {
            if ($(this).is('input')) {
                var value = $(this).val().trim().length;
                if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                    if (value < 5 && value > 0) {
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

    $('.QARequired').each(function (i, obj) {
        var dvMain = $(this).closest("td");
        var sd = $(this).attr('title') + ' is required';
        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if ($(this).hasClass("transferSemi") && value.length < 5) {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >Candy Semi must be at least 5 numbers<a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                if (value == '') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
        } else {
            var value = $(this).find("option:selected").text().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if (value.toLowerCase() == "select..." || value == '') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
        }
    });

    $('.WrongItemControl').each(function (i, obj) {
        if (!$(this).parent().hasClass('hideItem') && !$(this).parent().hasClass('hide') && !$(this).prop("disabled") && !$(this).prop("readonly")) {
            var id = $(this).attr('id');
            var selectedvalue = $(this).val();
            var selectedText = $("#" + id + " option:selected").text();
            if (selectedvalue == "-9999") {
                isValid = false;
                var requiredMsg = "'" + selectedText + "' is not a valid value! Please select a new one.";
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
            }
        }
    });

    $('.PricingCheck').each(function (i, obj) {
        dvMain = $(this).closest("div.form-group");
        label = dvMain.find('label');
        if (label.length)
            fieldName = label.text().replace(":", "");
        else
            fieldName = $(this).attr('title');
        requiredMsg = fieldName + ' is required';

        var id = $(this).children(":first").attr('id');

        if (!($("#" + id).prop("checked"))) {
            isValid = false;
            $("#dverror_message").show();
            $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
        }
    });

    var isSAPNumbersValid = VerifySAPNumbers();

    var url = window.location.href.toLocaleLowerCase();

    if (!(url.indexOf("qa.aspx") == -1)) {
        if ($("#hdnTBDIndicator").val().toLocaleLowerCase() == "yes" && !$(".repeater.divShipper").length) {
            if (!$("#dvCandy .row").length && !$("#dvPurchased .row").length && $("#hdnFinishedGoodCount").val() == "0") {
                isValid = false;
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >Candy Semi Item or Purchased Candy Semi Item is required <a style="color:darkblue" onclick=setFocusElement("btnAddItem") >Update</a>   </li></br>');
            }
        }
    }

    if (!(url.indexOf("componentcosting.aspx") == -1)) {
        if ($("#ddlBracketPricing option:selected").text().toLocaleLowerCase() == "yes") {
            if (!$("#bracketAttachmentTable tr").length) {
                isValid = false;
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >Attachment for bracket pricing is required <a style="color:darkblue" onclick=setFocusElement("btnUploadBracketPricing") >Update</a>   </li></br>');
            }
        }
    }

    if (!(url.indexOf("stagegatecreateproject.aspx") == -1)) {
        $('.MemberDiv').each(function (i, obj) {
            var hdnRequired = $(this).find("[id^=hdnRequired]").val();
            if (hdnRequired != 'False') {
                var ddlMember = $(this).find(".ddlMember").val();
                if ((ddlMember == "-1" || ddlMember == "")) {
                    var id = $(this).find(".ddlMember").attr('id');
                    isValid = false;
                    var label = $(this).find('label');
                    var fieldName = label.length ? label.text().replace(":", "") : $(this).attr('title');
                    var requiredMsg = fieldName + ' is required';
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + requiredMsg +
                        ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
            }
        });

        //Marketing cannot be just NA
        if ($('#ddlMarketingMembers').val() == "NA") {
            var AllNa = true;
            ('.txtMarketingMembers').each(function (i, obj) {
                if ($(this).val() != "NA") {
                    AllNa = false;
                }
            });
            if (AllNa == true) {
                var id = '#ddlMarketingMembers';
                isValid = false;
                var requiredMsg = "NA is not a valid entry for Marketing. Please add a valid member";
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >' + requiredMsg +
                    ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
            }
        }
    }

    var isBOMValid = BOMValidator(false);

    if (isBOMValid && isValid && isSAPNumbersValid)
        return true;

    loadingIconAdded = true;
    $(".disablingLoadingIcon").remove();
    setFocusError();
    return false;
}

function GetAllPackagingItems() {
    var PageArray = ["OBMFirstReview.aspx", "BOMSetupPE.aspx", "BOMSetupProc.aspx", "BOMSetupPE2.aspx", "SAPBOMSetup.aspx", "SAPInitialItemSetup.aspx", "OBMSecondReview.aspx", "PE1.aspx", "PE2.aspx", "PE3.aspx", "PROC.aspx"]

    if (PageArray.indexOf($("#hdnPageName").val()) >= 0) {
        return true;
    }
    else {
        return false;
    }
}

function BOMSetpPage() {
    var PageArray = ["BOMSetupPE.aspx", "BOMSetupProc.aspx", "BOMSetupPE2.aspx", "PE1.aspx", "PE2.aspx", "PE3.aspx", "PROC.aspx"]

    if (PageArray.indexOf($("#hdnPageName").val()) >= 0) {
        return '#dverror_message';
    }
    else {
        return '#error_message';
    }
}

function VerifySAPNumbers() {
    var AllMatched = true;
    return AllMatched;
    /*Commenting the following code for now as SAP list data is not accurate. 
    try {
        var BOMItems = [];
        var MaterialTypeArray = ["Transfer Semi", "Purchased Candy Semi", "Candy Semi"]
        var ClassMissMatchedBOMList = new Array();
        var matched = false;

        if (GetAllPackagingItems()) {
            BOMItems = GetAllPackagingItemsForProject();
        }
        else {
            $('.VerifySAPNumbers').each(function (i, obj) {
                var enteredMateralNumber = $(this).val();
                var id = $(this).attr('id');
                dvBOMRow = $(this).closest("div.SAPVerifyItem");
                if ($("#hdnPageName").val() == "QA.aspx") {
                    dvMain = $(this).closest("div.form-group");
                    label = dvMain.find('label');

                    if (label.length)
                        fieldName = label.text();
                    else
                        fieldName = $(this).attr('title');

                    if (fieldName.indexOf("Purchased Candy Semi") >= 0)
                        enteredMateraType = "Purchased Candy Semi";
                    else if (fieldName.indexOf("Candy Semi") >= 0)
                        enteredMateraType = "Candy Semi";
                }
                else {
                    enteredMateraType = dvBOMRow.find('.VerifySAPNumbersType').find("option:selected").text();
                }

                enteredNewExisting = dvBOMRow.find('.VerifySAPNewExisting').val();
                BOMItems.push([enteredMateralNumber, enteredMateraType, enteredNewExisting, id, "Not Found"]);
            });
        }

        $.each(BOMItems, function (index, BOMItem) {
            if (BOMItem[2] == "Existing") {
                if (MaterialTypeArray.indexOf(BOMItem[1]) >= 0) {
                    $.ajax({
                        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP BOM List')/items?$select=MaterialNumber,MaterialType&$filter=(MaterialNumber eq '" + BOMItem[0] + "')&$top=500",
                        method: 'GET',
                        dataType: 'json',
                        async: false,
                        headers: {
                            Accept: "application/json;odata=verbose"
                        },
                        success: function (SAPItems) {
                            var finding = SAPItems.d.results;
                            if (finding === undefined) {
                                matched = false;
                            } else {
                                var type;
                                matched = false;

                                if (BOMItem[1] == "Transfer Semi") {
                                    type = "TRANSFER";
                                } else if (BOMItem[1] == "Purchased Candy Semi") {
                                    type = "PURCHASED";
                                } else if (BOMItem[1] == "Candy Semi") {
                                    type = "CANDY";
                                }
                                //remove dupicates

                                $.each(SAPItems.d.results, function (ix, SAPItem) {
                                    BOMItem[4] = SAPItem.MaterialType;

                                    if (SAPItem.MaterialType == type) {
                                        matched = true;
                                    }
                                });
                            }
                        }
                    });

                    if (!matched) {
                        if (BOMItem[4] != null && BOMItem[4] != "Not Found") {
                            AllMatched = false;
                            ClassMissMatchedBOMList.push(BOMItem);
                        }
                    }
                }
            }
        });

        $.each(ClassMissMatchedBOMList, function (index, BOMItem) {
            var ErrorMessage;
            ErrDiv = BOMSetpPage()
            var UpdateLink = "";

            var type = BOMItem[4];
            if (BOMItem[4] == "TRANSFER") {
                type = "TRANSFER SEMI";
            } else if (BOMItem[4] == "PURCHASED") {
                type = "PUR CANDY SEMI";
            } else if (BOMItem[4] == "CANDY") {
                type = "CANDY SEMI";
            }

            if (BOMItem[4] != "Not Found") {
                $("#dverror_message").show();

                if (BOMItem[1] == "Candy Semi" && BOMItem[4] == "TRANSFER") {
                    ErrorMessage = "Component type of " + BOMItem[0] + " - " + BOMItem[1] + " does not match the classification in SAP. This component type of " + BOMItem[0] + " is categorized as a " + type + ".";
                } else if (BOMItem[1] == "Transfer Semi" && BOMItem[4] == "CANDY") {
                    ErrorMessage = "Component type of " + BOMItem[0] + " - " + BOMItem[1] + " does not match classification in SAP. Component type of " + BOMItem[0] + " is a " + type + ".";
                } else {
                    ErrorMessage = "Component type of " + BOMItem[0] + " - " + BOMItem[1] + " does not match classification in SAP. Component type of " + BOMItem[0] + " is a " + type + ".";
                }

                if ($("#hdnPageName").val() == "ItemProposal.aspx") {
                    UpdateLink = ' <a style="color:darkblue" onclick=activateTab(' + '"' + BOMItem[3] + '"' + ');setFocusElement(' + '"' + BOMItem[3] + '"' + ');>Update</a>';
                }
                else {
                    if ((BOMItem[3] != "") && (typeof BOMItem[3] != "undefined")) {
                        UpdateLink = ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + BOMItem[3] + '"' + ')>Update</a>';
                    }
                }

                $(ErrDiv).append('<li class="errorMessage" >' + ErrorMessage + UpdateLink + '</li></br>');
            }
        });
    }
    catch (err) {
        alert("Error in verifying Component types against SAP. Please try again.");
        AllMatched = false;
    }
    finally {
        return AllMatched
    }
    */
}

function setFocusErroruc(divMessage) {
    $('html, body').animate({
        scrollTop: $(divMessage).offset().top - 100
    }, 1000);
}

function validateuc(divMessage, lstMessage) {
    $(divMessage).empty();
    var isValid = true;
    //clearing out hidden transfer semi/candy semi fields
    if ($(".ipf.TSOnlyRow").hasClass("hideItem")) {
        $(".ipf.TSOnlyRow select").val("-1");
        $(".ipf.TSOnlyRow :input:not([type=hidden]):not([type=submit]):not([type=button])").val("");
    }
    if ($(".ipf.hideableRow").hasClass("hideItem")) {
        $(".ipf.hideableRow select").val("-1");
        $(".ipf.hideableRow :input:not([type=hidden]):not([type=submit]):not([type=button])").val("");
    }
    $('.requireduc').each(function (i, obj) {
        var dvMain = $(this).closest(".form-group");
        var sd = dvMain.find('label').text().replace(":", "") + ' is required';

        var id = $(this).attr('id');
        if (id == "ddlReviewPrinterSupplier") {
            sd = sd + ". Please contact your External Manufacturing Team Member.";
        }
        var value = $(this).val().trim();
        if (!$(this).parent().parent().hasClass('hideItem') && !$(this).closest(".row").hasClass("hideItem")) {
            if ($(this).is('input') || ($(this).is('textarea'))) {

                //if (!$(this).parent().parent().hasClass('hideItem')) {
                if (value == "") {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
                //}
            }
            else {
                ////if (!$(this).parent().parent().hasClass('hideItem')) {
                if (value == '-1') {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
                //}
            }
        }
    });

    $('.PCBOMrequired').each(function (i, obj) {
        var sd = $(this).parent().find('label').text().replace(":", "") + ' is required';
        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == "") {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        }
        else {
            var value = $(this).val();
            if (!$(this).parent().parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == '-1') {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
        }
    });

    $('.UCBOMrequired').each(function (i, obj) {

        var rowIndex = $(this).closest(".bomrow").children().index($(this).closest("td"));
        var sd = $(this).closest("table").find(" tr th:eq(" + rowIndex + ")").html() + ' is required';
        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == "") {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        }
        else {
            var value = $(this).val();
            if (!$(this).parent().parent().hasClass('hideItem')) {
                if (value == '-1') {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
        }
    });

    $('.requiredpm').each(function (i, obj) {

        var sd = $(this).closest(".form-group").find('label').text().replace(":", "") + ' is required';
        var id = $(this).attr('name');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == "") {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElementRepeater(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        }
        else {
            var value = $(this).val();
            if (!$(this).parent().parent().hasClass('hideItem'))
                if (value == '-1') {
                    isValid = false;
                    $(divMessage).show();
                    $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElementRepeater(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
        }
    });

    $('.drpPalletPatternChange').each(function (i, obj) {
        var dvMain = $(this).closest("div.miscOpsClass");
        var sd = 'Pallet Pattern Attachment is required';
        var id = $(this).attr('id');
        var value = $(this).find("option:selected").val();
        if (value.toLowerCase() == "y" && dvMain.find("#hdnPalletPatterCount").val() == '0') {
            if (!$(this).closest(".OBMSetup").hasClass('hideItem')) {
                isValid = false;
                $(divMessage).show();
                $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');

            }
        } else {
            $(this).removeClass('highlightElement');
        }
    });

    if ($("#ddlResultPackTrial").length && $("#ddlResultPackTrial").hasClass("requiredpm")) {
        var dvMain = $(this).closest("div.OBMSetup");
        var sd = 'Pack Trial Attachment is required';
        var value = $(this).find("option:selected").val();
        if ($("#packTrialAttachments").find("tr").length <= 0) {
            if (!$(this).closest(".OBMSetup").hasClass('hideItem')) {
                isValid = false;
                $(divMessage).show();
                $(divMessage).append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement("packTrialAttachments") >Update</a>   </li></br>');

            }
        } else {
            $(this).removeClass('highlightElement');
        }
    }

    var isSAPNumbersValid = true;
    if (!($("#dverror_messageuc").length)) {
        isSAPNumbersValid = VerifySAPNumbers();
    }

    if (!isValid || !isSAPNumbersValid) {
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
        setFocusErroruc(divMessage);
    }
    return isValid && isSAPNumbersValid;
}

function BOMValidator(clearList) {
    if (clearList)
        $('#error_message').empty();

    var isValid = true;

    var dvMain = $('.BOMrequired').closest("div.repeater");

    if (!dvMain.hasClass('hideItem')) {
        $('.BOMrequired').each(function (i, obj) {
            var id = $(this).attr('id');
            if ($(this).is('input')) {
                var sd = $(this).attr('title');
                var value = $(this).val();

                if (value == "") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li style="padding-left:20px;flaot:left; display: inline;color:red">' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
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

        $('.BOMrequiredNeedsNew').each(function (i, obj) {
            var id = $(this).attr('id');
            if ($(this).is('input')) {
                var sd = $(this).attr('title');
                var value = $(this).val();
                if (value == "" || value.toLowerCase() == "needs new") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li style="padding-left:20px;flaot:left; display: inline;color:red">' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
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

function CheckMinimumLength(arg) {
    $('#error_message').empty();
    var isValid = true;
    $('.minimumlength').each(function (i, obj) {
        var sd = $(this).parent().find('label').text().replace(":", "") + ' must be at least ' + arg + ' digits';
        //var sd = 'Please enter at least ' + arg + ' digits';
        var id = $(this).attr('id');
        if ($(this).is('input')) {
            var value = $(this).val().trim().length;
            if (!$(this).parent().parent().hasClass('hideItem'))
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

function hideItem(arg) {
    $("#" + arg).removeClass('showItem').addClass('hideItem');
}

function showItem(arg) {
    $("#" + arg).removeClass('hideItem').addClass('showItem');
}

function RemoveRougeChar(convertString) {
    if (convertString.substring(0, 1) == ",") {
        return convertString.substring(1, convertString.length)
    }
    return convertString;
}

function maskNumber(str) {
    return (str + "").replace(/\b(\d+)((\.\d+)*)\b/g, function (a, b, c) {
        return (b.charAt(0) > 0 && !(c || ".").lastIndexOf(".") ? b.replace(/(\d)(?=(\d{3})+$)/g, "$1,") : b) + c;
    });
}

//******** Basic Dialog Starts Here ***********/

function openBasicDialog(tTitle, docType, packagingItemId) {
    if (packagingItemId == undefined) {
        packagingItemId = 0;
    }
    var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=' + packagingItemId + '&DocType=' + docType + '&CompassItemId=' + $("#hiddenItemId").val();
    var options = {
        url: url,
        title: tTitle,
        dialogReturnValueCallback: onPopUpCloseCallBack,
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function openGenericDialog(tTitle, urlParameters) {
    var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx' + urlParameters;
    var options = {
        url: url,
        title: tTitle,
        dialogReturnValueCallback: onPopUpCloseCallBack,
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function onPopUpCloseCallBack(result, returnValue) {
    $(".ReloadAttachment").click();
    $(".disablingLoadingIcon").remove();
}

function showWaitPopup(tTitle, tMessage) {
    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
        var waitingDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose(tTitle, tMessage, 200, 600);
    });
}

//******** Basic Dialog Ends Here ***********/

$(".itemNumber").keypress(function (e) {
    //if the letter is not digit then display error and don't type anything
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        //display error message
        return false;
    }
});
//****************************************//
$('.decimal').keypress(function (event) {
    var $this = $(this);
    if ((event.which != 46 || $this.val().indexOf('.') != -1) &&
       ((event.which < 48 || event.which > 57) &&
       (event.which != 0 && event.which != 8))) {
        event.preventDefault();
    }

    var text = $(this).val();
    if ((event.which == 46) && (text.indexOf('.') == -1)) {
        setTimeout(function () {
            if ($this.val().substring($this.val().indexOf('.')).length > 3) {
                $this.val($this.val().substring(0, $this.val().indexOf('.') + 3));
            }
        }, 1);
    }
    if ((text.indexOf('.') != -1) &&
        (text.substring(text.indexOf('.')).length > 2) &&
        (event.which != 0 && event.which != 8) &&
        ($(this)[0].selectionStart >= text.length - 2)) {
        event.preventDefault();
    }
});

//******Disable Enter key***********//

function stopRKey(evt) {
    var evt = (evt) ? evt : ((event) ? event : null);
    var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
    if ((evt.keyCode == 13) && (node.type != "textarea")) {
        return false;
    }
}

//*****************//

function DTDisplayParentProject() {
    var myOpenProjects = $('#MyOpenParentProjects').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                        loadingIconAdded = true;
                    });
                column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                    var val = $('<div/>').html(d).text();
                    select.append('<option value="' + val + '">' + val + '</option>');
                });
            });
        },
        data: ParentProjectDetails,
        fixedColumns: true,
        "order":
            [0, 'desc'],
        "lengthMenu":
            [
                10,
                25,
                50,
                75,
                100
            ],
        columns:
            [
                {
                    'data': 'ProjectNumber'
                },
                {
                    'data': 'ProjectTitle'
                },
                {
                    'data': 'Gate0ApprovedDate',
                    'render': function (data, type, row, meta) {
                        if (data == null || data == "") {
                            var ShipDate = "";
                        } else {
                            var revisedfirstShip = new Date(data);
                            var ShipDay = revisedfirstShip.getDate();
                            var Shipmonth = revisedfirstShip.getMonth() + 1;
                            var ShipYear = revisedfirstShip.getFullYear();
                            var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                        }
                        return ShipDate;
                    }
                },
                {
                    'data': 'DesiredShipDate',
                    'render': function (data, type, row, meta) {
                        if (data == null || data == "") {
                            var ShipDate = "";
                        } else {
                            var revisedfirstShip = new Date(data);
                            var ShipDay = revisedfirstShip.getDate();
                            var Shipmonth = revisedfirstShip.getMonth() + 1;
                            var ShipYear = revisedfirstShip.getFullYear();
                            var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                        }
                        return ShipDate;
                    }
                },
                {
                    'data': 'RevisedFirstShipDate',
                    'render': function (data, type, row, meta) {
                        if (data == null || data == "") {
                            var ShipDate = "";
                        } else {
                            var revisedfirstShip = new Date(data);
                            var ShipDay = revisedfirstShip.getDate();
                            var Shipmonth = revisedfirstShip.getMonth() + 1;
                            var ShipYear = revisedfirstShip.getFullYear();
                            var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                        }
                        return ShipDate;
                    }
                },
                {
                    'data': 'Stage'
                },
                {
                    'data': 'Brand'
                },
                {
                    'data': 'SKUs'
                },
                {
                    'data': 'ProjectType'
                },
                {
                    'data': 'ProjectTypeSubCategory'
                },
                {
                    'data': 'PM'
                },
                {
                    'data': 'Initiator'
                }

            ]
    });

    $('#MyOpenParentProjects').css('width', '');

    //if ($('#MyOpenParentProjects tbody tr').length > 0 && !($('#MyOpenParentProjects tbody tr').length == 1 && $('#MyOpenParentProjects tbody tr td').hasClass("dataTables_empty"))) {
    //    $('#openParentProjectMainDiv').css({ "display": "inline-block" });
    //} else if ($('#MyOpenParentProjectsProcurement tbody tr').length > 0 && !($('#MyOpenParentProjectsProcurement tbody tr').length == 1 && $('#MyOpenParentProjectsProcurement tbody tr td').hasClass("dataTables_empty"))) {
    //    $('#openParentProjectMainDiv').css({ "display": "inline-block" });
    //}
}

function DTDisplayChildProject() {
    if ($("#hidProcurement").val() != "true") {
        var myOpenProjects = $('#MyOpenChildProjects').DataTable({
            initComplete: function () {
                this.api().columns().every(function () {
                    var column = this;
                    var select = $('<select><option value=""></option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                        });
                    column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                        var val = $('<div/>').html(d).text();
                        select.append('<option value="' + val + '">' + val + '</option>');
                    });
                });
            },
            data: ChildProjectDetails,
            "createdRow": function (row, data, dataIndex) {
                var startDate = new Date(data.SubmittedDate);
                var checkEndDate = "";
                if (!Date.parse(data.RevisedFirstShipDate)) {
                    checkEndDate = new Date(data.SubmittedDate);
                    checkEndDate.setDate(checkEndDate.getDate() + 45);

                } else {
                    checkEndDate = new Date(data.RevisedFirstShipDate);
                }

                var endDate = new Date(checkEndDate);
                if (Date.parse(endDate) && Date.parse(startDate)) {
                    var now = Date.now();
                    if (now > endDate) {
                        $(row).addClass("StatusRed");
                    } else {
                        var taskDiff = Math.abs(endDate.getTime() - startDate.getTime());
                        var taskSpan = Math.ceil(taskDiff / (1000 * 3600 * 24));


                        var currentDiff = Math.abs(now - startDate.getTime());
                        var currentSpan = Math.ceil(currentDiff / (1000 * 3600 * 24));
                        var percent = (currentSpan / taskSpan);
                        if (percent < .95) {
                            $(row).addClass("StatusGreen");
                        }
                        else if (percent >= .95 && percent < 1) {
                            $(row).addClass("StatusYellow");
                        }
                        else if (percent >= 1) {
                            $(row).addClass("StatusRed");
                        } else {
                            $(row).addClass("StatusRed");
                        }
                    }
                } else {
                    $(row).addClass("StatusNone");
                }
            },
            "columnDefs": [
                {
                    type: 'title-string', targets: 0, targets: 6
                },
                {
                    className: "projectName", targets: 1
                },
                {
                    className: "hide", targets: 12
                },
                {
                    className: "hide", targets: 13
                },
                {
                    className: "hide", targets: 14
                }
            ],
            "order": [
                [0, 'asc'],
                [9, 'asc'],
                [4, 'asc']
            ],
            fixedColumns: true,
            columns: [
                {
                    'data': 'OBM_ProjectStatus',
                    'render': function (data, type, row, meta) {
                        var colors = "";
                        var startDate = new Date(row.SubmittedDate);
                        var checkEndDate = ""
                        if (row.RevisedFirstShipDate == null) {
                            checkEndDate = new Date(startDate);
                            checkEndDate.setDate(checkEndDate.getDate() + 45);
                        } else {
                            checkEndDate = new Date(row.RevisedFirstShipDate);
                        }

                        var endDate = checkEndDate;
                        if (Date.parse(endDate) && Date.parse(startDate)) {
                            var now = Date.now();
                            if (now > endDate) {
                                colors = "A";
                            } else {
                                var taskDiff = Math.abs(endDate.getTime() - startDate.getTime());
                                var taskSpan = Math.ceil(taskDiff / (1000 * 3600 * 24));


                                var currentDiff = Math.abs(now - startDate.getTime());
                                var currentSpan = Math.ceil(currentDiff / (1000 * 3600 * 24));
                                var percent = (currentSpan / taskSpan);
                                if (percent < .95) {
                                    colors = "C";
                                }
                                else if (percent >= .95 && percent < 1) {
                                    colors = "B";
                                }
                                else if (percent >= 1) {
                                    colors = "A";
                                }

                            }
                        } else {
                            colors = "D";
                        }
                        return '<div title="' + colors + '">' + colors + '</span>';
                    }
                },
                {
                    'data': 'ProjectNumber',
                    'render': function (data, type, row, meta) {
                        var title = row.ProjectNumber + " : ";

                        if (row.Parent != "Yes") {
                            if (row.SAPItemNumber == null || row.SAPItemNumber == "") {
                                title = title + "XXXXX : ";
                            } else {
                                title = title + row.SAPItemNumber + " : ";
                            }
                        }

                        if (row.SAPDescription == null || row.SAPDescription == "") {
                            title = title + row.SAPDescription + "(Proposed)";
                        } else {
                            title = title + row.SAPDescription;
                        }

                        if (row.Parent != "Yes") {
                            return '<a href="/Pages/ProjectStatus.aspx?ProjectNo=' + row.ProjectNumber + '">' + title + '</a>';
                        }
                        else {
                            return '<a href="/Pages/StageGateProjectPanel.aspx?ProjectNo=' + row.ProjectNumber + '">' + title + '</a>';
                        }
                    }
                },
                {
                    'data': 'FirstProduction',
                    'render': function (data) {
                        if (data == null || data == "") {
                            var prodDate = "";
                        } else {
                            var newfirstProd = new Date(data);
                            var ProdDay = newfirstProd.getDate();
                            var Prodmonth = newfirstProd.getMonth() + 1;
                            var ProdYear = newfirstProd.getFullYear();
                            var prodDate = Prodmonth + "/" + ProdDay + "/" + ProdYear;
                        }
                        return prodDate;
                    }
                },
                {
                    'data': 'RevisedFirstShipDate',
                    'render': function (data) {
                        if (data == null || data == "") {
                            var shipDate = "";
                        } else {
                            var newfirstShip = new Date(data);
                            var ShipDay = newfirstShip.getDate();
                            var Shipmonth = newfirstShip.getMonth() + 1;
                            var ShipYear = newfirstShip.getFullYear();
                            var shipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                        }
                        return shipDate;
                    }
                },
                {
                    'data': 'ProjectType'
                },
                {
                    'data': 'ProjectTypeSubCategory'
                },
                {
                    'data': 'MaterialGroup1Brand'
                },
                {
                    'data': 'Customer'
                },
                {
                    'data': 'InitiatorName'
                },
                {
                    'data': 'TimelineType',
                    'render': function (data, type, row, meta) {
                        var sort = "D";
                        var timeline = "Standard"
                        if (row.TimelineType != null && row.TimelineType != "") {
                            timeline = row.TimelineType;
                        }
                        if (row.TimelineType == "Ludicrous") {
                            sort = "A";
                        } else if (row.TimelineType == "Expedited") {
                            sort = "B";
                        } else if (row.TimelineType == "Standard") {
                            sort = "C";
                        }
                        return '<span title="' + sort + '">' + timeline + '</span>';
                    }
                },
                {
                    'data': 'WorkflowPhase',
                    'render': function (data, type, row, meta) {
                        var phaseText = "";
                        if (row.WorkflowStep == null && row.WorkflowPhase != null) {
                            phaseText = row.WorkflowPhase;
                        } else if (row.WorkflowStep != null && row.WorkflowPhase != null) {
                            phaseText = row.WorkflowPhasee + ": " + row.WorkflowStep;

                        }
                        return phaseText.replace("OBM", "PM");
                    }
                },
                {
                    'data': 'ProductHierarchyLevel1'
                },
                {
                    'data': 'CompassItemId'
                },
                {
                    'data': 'Parent'
                },
                {
                    'data': 'StageGateProjectListItemId'
                }
            ]
        });
        myOpenProjects.draw();
    }
    else {
        $("#MyOpenChildProjectsProcurement").show();
        $("#MyOpenChildProjects").hide();
        var myOpenProjects = $('#MyOpenChildProjectsProcurement').DataTable({
            initComplete: function () {
                this.api().columns().every(function () {
                    var column = this;
                    var select = $('<select><option value=""></option></select>')
                            .appendTo($(column.footer()).empty())
                            .on('change', function () {
                                var val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );

                                column
                                    .search(val ? '^' + val + '$' : '', true, false)
                                    .draw();
                            });
                    column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                        var val = $('<div/>').html(d).text();
                        select.append('<option value="' + val + '">' + val + '</option>')
                    });
                });
            },
            data: ChildProjectDetails,
            "createdRow": function (row, data, dataIndex) {
                var startDate = new Date(data.SubmittedDate);
                var checkEndDate = "";
                if (!Date.parse(data.RevisedFirstShipDate)) {
                    checkEndDate = new Date(data.SubmittedDate);
                    checkEndDate.setDate(checkEndDate.getDate() + 45);

                } else {
                    checkEndDate = new Date(data.RevisedFirstShipDate);
                }

                var endDate = new Date(checkEndDate);
                if (Date.parse(endDate) && Date.parse(startDate)) {
                    var now = Date.now();
                    if (now > endDate) {
                        $(row).addClass("StatusRed");
                    } else {
                        var taskDiff = Math.abs(endDate.getTime() - startDate.getTime());
                        var taskSpan = Math.ceil(taskDiff / (1000 * 3600 * 24));


                        var currentDiff = Math.abs(now - startDate.getTime());
                        var currentSpan = Math.ceil(currentDiff / (1000 * 3600 * 24));
                        var percent = (currentSpan / taskSpan);
                        if (percent < .95) {
                            $(row).addClass("StatusGreen");
                        }
                        else if (percent >= .95 && percent < 1) {
                            $(row).addClass("StatusYellow");
                        }
                        else if (percent >= 1) {
                            $(row).addClass("StatusRed");
                        } else {
                            $(row).addClass("StatusRed");
                        }
                    }
                } else {
                    $(row).addClass("StatusNone");
                }
            },
            "columnDefs":
                [
                    {
                        type: 'title-string', targets: 0
                    },
                    {
                        className: "projectName", targets: 1
                    },
                    {
                        className: "hide", targets: 11
                    },
                    {
                        className: "hide", targets: 12
                    },
                    {
                        className: "hide", targets: 13
                    }
                ],
            "order":
            [11, 'desc'],
            fixedColumns: true,
            columns:
                [
                    {
                        'data': 'OBM_ProjectStatus',
                        'render': function (data, type, row, meta) {
                            var colors = "";
                            var startDate = new Date(row.SubmittedDate);
                            var checkEndDate = ""
                            if (row.RevisedFirstShipDate == null) {
                                checkEndDate = new Date(startDate);
                                checkEndDate.setDate(checkEndDate.getDate() + 45);
                            } else {
                                checkEndDate = new Date(row.RevisedFirstShipDate);
                            }

                            var endDate = checkEndDate;
                            if (Date.parse(endDate) && Date.parse(startDate)) {
                                var now = Date.now();
                                if (now > endDate) {
                                    colors = "A";
                                } else {
                                    var taskDiff = Math.abs(endDate.getTime() - startDate.getTime());
                                    var taskSpan = Math.ceil(taskDiff / (1000 * 3600 * 24));


                                    var currentDiff = Math.abs(now - startDate.getTime());
                                    var currentSpan = Math.ceil(currentDiff / (1000 * 3600 * 24));
                                    var percent = (currentSpan / taskSpan);
                                    if (percent < .95) {
                                        colors = "C";
                                    }
                                    else if (percent >= .95 && percent < 1) {
                                        colors = "B";
                                    }
                                    else if (percent >= 1) {
                                        colors = "A";
                                    }

                                }
                            } else {
                                colors = "D";
                            }
                            return '<div title="' + colors + '">' + colors + '</span>';
                        }
                    },
                    {
                        'data': 'ProjectNumber',
                        'render': function (data, type, row, meta) {
                            var title = row.ProjectNumber + " : ";

                            if (row.Parent != "Yes") {
                                if (row.SAPItemNumber == null || row.SAPItemNumber == "") {
                                    title = title + "XXXXX : ";
                                } else {
                                    title = title + row.SAPItemNumber + " : ";
                                }
                            }

                            if (row.SAPDescription == null || row.SAPDescription == "") {
                                title = title + row.SAPDescription + "(Proposed)";
                            } else {
                                title = title + row.SAPDescription;
                            }

                            if (row.Parent != "Yes") {
                                return '<a href="/Pages/ProjectStatus.aspx?ProjectNo=' + row.ProjectNumber + '">' + title + '</a>';
                            }
                            else {
                                return '<a href="/Pages/StageGateProjectPanel.aspx?ProjectNo=' + row.ProjectNumber + '">' + title + '</a>';
                            }
                        }
                    },
                    {
                        'data': 'RevisedFirstShipDate',
                        'render': function (data) {
                            if (data == null || data == "") {
                                var prodDate = "";
                            } else {
                                var newfirstProd = new Date(data);
                                var ProdDay = newfirstProd.getDate();
                                var Prodmonth = newfirstProd.getMonth() + 1;
                                var ProdYear = newfirstProd.getFullYear();
                                var prodDate = Prodmonth + "/" + ProdDay + "/" + ProdYear;
                            }

                            return prodDate;
                        }
                    },
                    {
                        'data': 'ProjectType'
                    },
                    {
                        'data': 'ProjectTypeSubCategory'
                    },
                    {
                        'data': 'MaterialGroup1Brand'
                    },
                    {
                        'data': 'Customer'
                    },
                    {
                        'data': 'InitiatorName'
                    },
                    {
                        'data': 'PackagingNumbers',
                        'render': function (data, type, row, meta) {
                            var pcCode = "";
                            if (row.PackagingNumbers != null) {
                                pcCode = "<table style='border: 1px solid black; border-image: none; width: 300px;'>" + row.PackagingNumbers + "</table>";
                            }
                            return pcCode;
                        }
                    },
                    {
                        'data': 'ProjectNumber',
                        'render': function (data, type, row, meta) {
                            var pageURL = "/Pages/CommercializationItem.aspx?ProjectNo=" + row.ProjectNumber;
                            return '<a href=' + pageURL + '>' + row.ProjectNumber + '</a>';
                        }
                    },
                    {
                        'data': 'ProductHierarchyLevel1'
                    },
                    {
                        'data': 'CompassItemId'
                    },
                    {
                        'data': 'Parent'
                    },
                    {
                        'data': 'StageGateProjectListItemId'
                    }
                ]
        });
        myOpenProjects.draw();
    }

    if ($('#MyOpenChildProjects tbody tr td').hasClass("dataTables_empty")) {
        $('#MyOpenChildProjects').css('width', '');
    }

    //if ($('#MyOpenChildProjects tbody tr').length > 0 && !($('#MyOpenChildProjects tbody tr').length == 1 && $('#MyOpenChildProjects tbody tr td').hasClass("dataTables_empty"))) {
    //    $('#openChildProjectMainDiv').css({ "display": "inline-block" });
    //} else if ($('#MyOpenChildProjectsProcurement tbody tr').length > 0 && !($('#MyOpenChildProjectsProcurement tbody tr').length == 1 && $('#MyOpenChildProjectsProcurement tbody tr td').hasClass("dataTables_empty"))) {
    //    $('#openChildProjectMainDiv').css({ "display": "inline-block" });
    //}
}

function UpdateTableHeaders() {

    var el = $("#projectDashboard"),
offset = el.offset(),
scrollTop = $(window).scrollTop(),
floatingHeader = $(".floatingHeader");
    //alert(scrollTop+">"+offset.top+scrollTop+"<"+offset.top + el.height());
    if ((scrollTop > offset.top) && (scrollTop < offset.top + el.height())) {
        floatingHeader.css({
            "visibility": "visible"
        });
    } else {
        floatingHeader.css({
            "visibility": "hidden"
        });
    }
    //});
}

function runFloatHead() {
    $("#projectDashboard tbody").on("click", ".collapseProcess", function () {
        var clicked = $(this);
        if (clicked.parent().hasClass("phaseRow")) {
            var currentIndex = clicked.closest(".phaseRow").index() + 1;
            var nextIndex = $("tbody .phaseRow").index(clicked.closest(".phaseRow")) + 1;
            var rowsBetween = $("tbody tr").index($("tbody .phaseRow:eq(" + nextIndex + ")"));
            $("#projectDashboard tbody tr").slice(currentIndex, rowsBetween).hide();
        } else {
            var currentIndex = clicked.closest("tr").index() + 1;
            var nextIndex = currentIndex + 7;
            $("#projectDashboard tbody tr").slice(currentIndex, nextIndex).hide();
        }
        clicked.addClass('expandProcess');
        clicked.removeClass('collapseProcess');
    });
    $("#projectDashboard tbody").on("click", ".expandProcess", function () {
        var clicked = $(this);
        if (clicked.parent().hasClass("phaseRow")) {
            var currentIndex = clicked.closest(".phaseRow").index() + 1;
            var nextIndex = $("tbody .phaseRow").index(clicked.closest(".phaseRow")) + 1;
            var rowsBetween = $("tbody tr").index($("tbody .phaseRow:eq(" + nextIndex + ")"));
            $("#projectDashboard tbody tr").slice(currentIndex, rowsBetween).show();
            $("#projectDashboard tbody tr.packagingRow td:first-child").addClass('collapseProcess');
            $("#projectDashboard tbody tr.packagingRow td:first-child").removeClass('expandProcess');
        } else {
            var currentIndex = clicked.closest("tr").index() + 1;
            var nextIndex = currentIndex + 7;
            $("#projectDashboard tbody tr").slice(currentIndex, nextIndex).show();
        }
        clicked.addClass("collapseProcess");
        clicked.removeClass("expandProcess");
    });
    var clonedHeaderRow;

    clonedHeaderRow = $("#projectDashboard thead tr");
    clonedHeaderRow
    .before(clonedHeaderRow.clone())
    .css("width", clonedHeaderRow.width())
    .addClass("floatingHeader");
    $(".floatingHeader").attr("id", "floadingheaderRow");
    $("#floadingheaderRow").find(".weekLine").remove();

}

$(document).on("click", ".ButtonControlAutoSize, .ButtonControl, .button", function () {
    if (!loadingIconAdded && !$(this).hasClass("noIcon")) {
        var clicked = $(this);
        var top = clicked.position().top;
        var left = clicked.position().left;
        var width = clicked.outerWidth();
        var height = clicked.outerHeight();
        clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");
    } else {
        loadingIconAdded = false;
        $(".disablingLoadingIcon").remove();

    }
});

function updateSalesDimensions(clicked, id) {
    var clicked = $(clicked);
    if (clicked.hasClass("salesDims")) {
        $("#" + id).val(clicked.val());
    }
}

function AllProjectDisplay() {
    visibility = true;
    //if ($("#hidProjectGroup").val() != "procurement") {
    //    visibility = false;
    //}

    var myOpenProjects = $('#AllOpenProjects').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                        loadingIconAdded = true;
                    });
                column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                    var val = $('<div/>').html(d).text();
                    select.append('<option value="' + val + '">' + val + '</option>');
                });
            });
        },
        data: projectDetails,
        "columnDefs": [
        ],
        fixedColumns: true,
        "order": [6, 'asc'],
        "lengthMenu": [25, 50, 75, 100],
        columns: [
            {
                'data': 'ParentProjectNumberLink'
            },
            {
                'data': 'ProjectWorkflowLink'
            },
            {
                'data': 'CommercializationLink'
            },
            {
                'data': 'SAPItemNumber'
            },
            {
                'data': 'SAPDescription'
            },
            {
                'data': 'RevisedFirstShipDate',
                'render': function (data, type, row, meta) {
                    if (data == null) {
                        var ShipDate = "";
                    } else {
                        var revisedfirstShip = new Date(data);
                        var ShipDay = revisedfirstShip.getDate();
                        var Shipmonth = revisedfirstShip.getMonth() + 1;
                        var ShipYear = revisedfirstShip.getFullYear();
                        var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                    }
                    return ShipDate;
                }
            },
            {
                'data': 'WorkflowPhase',
                'render': function (data, type, row, meta) {
                    var phaseText = "";
                    if (row.WorkflowPhase != null) {
                        phaseText = row.WorkflowPhase;
                    }
                    return phaseText.replace("OBM", "PM");
                }
            },
            {
                'data': 'ProductHierarchyLevel1'
            },
            {
                'data': 'MaterialGroup1Brand'
            },
            {
                'data': 'ManufacturingLocation'
            },
            {
                'data': 'PackingLocation'
            },
            {
                'data': 'ProjectType'
            },
            {
                'data': 'ProjectTypeSubCategory'
            },
            {
                'data': 'PM'
            },
            {
                'data': 'PackagingEngineer'
            },
            {
                'data': 'Initiator'
            },
            {
                'data': 'Customer'
            },
            {
                'data': 'SubmittedDate',
                'render': function (data) {
                    if (data == null) {
                        var prodDate = "";
                    } else {
                        var newfirstProd = new Date(data);
                        var ProdDay = newfirstProd.getDate();
                        var Prodmonth = newfirstProd.getMonth() + 1;
                        var ProdYear = newfirstProd.getFullYear();
                        var prodDate = Prodmonth + "/" + ProdDay + "/" + ProdYear;
                    }
                    return prodDate;
                }
            }
        ]
    });

    /*if ($('#AllOpenProjects tbody tr').length > 0 && !($('#AllOpenProjects tbody tr').length == 1 && $('#AllOpenProjects tbody tr td').hasClass("dataTables_empty"))) {
        $('#allProjectMainDiv').css({ "display": "inline-block" });
    }*/
    var buttons = new $.fn.dataTable.Buttons(myOpenProjects, {
        buttons: [{
            extend: "excelHtml5",
            text: "Export to Excel",
            className: "ButtonControlAutoSize"
        }]
    }).container().appendTo($('#AllOpenProjects_wrapper'));
    loadingIconAdded = true;
}

function AllProjectDetailsDisplay() {
    visibility = true;
    if ($("#hidProjectGroup").val() != "procurement") {
        visibility = false;
    }

    var myOpenProjects = $('#AllOpenProjects').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                        loadingIconAdded = true;
                    });
                column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                    var val = $('<div/>').html(d).text();
                    select.append('<option value="' + val + '">' + val + '</option>');
                });
            });
        },
        data: projectDetails,
        "columnDefs": [
                {
                    className: "packagingNumbersCol", targets: 5, visible: visibility, searchable: visibility
                },
        ],
        fixedColumns: true,
        "order": [7, 'asc'],
        "lengthMenu": [25, 50, 75, 100],
        columns: [
            {
                'data': 'ParentProjectNumberLink'
            },
            {
                'data': 'CommercializationLink'
            },
            {
                'data': 'ProjectWorkflowLink'
            },
            {
                'data': 'SAPItemNumber'
            },
            {
                'data': 'SAPDescription'
            },
            {
                'data': 'RevisedFirstShipDate',
                'render': function (data, type, row, meta) {
                    if (data == null) {
                        var ShipDate = "";
                    } else {
                        var revisedfirstShip = new Date(data);
                        var ShipDay = revisedfirstShip.getDate();
                        var Shipmonth = revisedfirstShip.getMonth() + 1;
                        var ShipYear = revisedfirstShip.getFullYear();
                        var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                    }
                    return ShipDate;
                }
            },
            {
                'data': 'PackagingNumbers',
                'render': function (data, type, row, meta) {
                    if (row.PackagingNumbers != null) {
                        var pcCode = makeTable(row.PackagingNumbers);
                        return pcCode;
                    } else {
                        return "";
                    }
                }
            },
            {
                'data': 'WorkflowPhase',
                'render': function (data, type, row, meta) {
                    var phaseText = "";
                    if (row.WorkflowPhase != null) {
                        phaseText = row.WorkflowPhase;
                    }
                    return phaseText.replace("OBM", "PM");
                }
            },
            {
                'data': 'ProductHierarchyLevel1'
            },
            {
                'data': 'MaterialGroup1Brand'
            },
            {
                'data': 'ManufacturingLocation'
            },
            {
                'data': 'PackingLocation'
            },
            {
                'data': 'ProjectType'
            },
            {
                'data': 'ProjectTypeSubCategory'
            },
            {
                'data': 'PM'
            },
            {
                'data': 'PackagingEngineer'
            },
            {
                'data': 'Initiator'
            },
            {
                'data': 'Customer'
            },
            {
                'data': 'SubmittedDate',
                'render': function (data) {
                    if (data == null) {
                        var prodDate = "";
                    } else {
                        var newfirstProd = new Date(data);
                        var ProdDay = newfirstProd.getDate();
                        var Prodmonth = newfirstProd.getMonth() + 1;
                        var ProdYear = newfirstProd.getFullYear();
                        var prodDate = Prodmonth + "/" + ProdDay + "/" + ProdYear;
                    }
                    return prodDate;
                }
            }
        ]
    });

    /*if ($('#AllOpenProjects tbody tr').length > 0 && !($('#AllOpenProjects tbody tr').length == 1 && $('#AllOpenProjects tbody tr td').hasClass("dataTables_empty"))) {
        $('#allProjectMainDiv').css({ "display": "inline-block" });
    }*/
    var buttons = new $.fn.dataTable.Buttons(myOpenProjects, {
        buttons: [{
            extend: "excelHtml5",
            text: "Export to Excel",
            className: "ButtonControlAutoSize"
        }]
    }).container().appendTo($('#AllOpenProjects_wrapper'));
    loadingIconAdded = true;
}
function AllProjectDetailsDisplay2() {
    visibility = true;
    if ($("#hidProjectGroup").val() != "procurement") {
        visibility = false;
    }

    var myOpenProjects = $('#AllOpenProjects').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                        loadingIconAdded = true;
                    });
                column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                    var val = $('<div/>').html(d).text();
                    select.append('<option value="' + val + '">' + val + '</option>');
                });
            });
        },
        data: projectDetails,
        "columnDefs": [
                {
                    className: "packagingNumbersCol", targets: 5, visible: visibility, searchable: visibility
                },
        ],
        fixedColumns: true,
        "order": [7, 'asc'],
        "lengthMenu": [25, 50, 75, 100],
        columns: [
            {
                'data': 'ParentProjectNumberLink',
                'render': function (data, type, row, meta) {
                    return "<a href='/Pages/StageGateProjectPanel.aspx?ProjectNo=" + row.ParentProjectNumber + ">" + row.ParentProjectNumber + "</a>";
                }
            },
            {
                'data': 'CommercializationLink',
                'render': function (data, type, row, meta) {
                    return "<a href='/Pages/CommercializationItem.aspx?ProjectNo=" + row.ProjectNumber + ">" + row.ProjectNumber + "</a>";
                },
            },
            {
                'data': 'ProjectWorkflowLink',
                'render': function (data, type, row, meta) {
                    return "<a href='/Pages/ProjectStatus.aspx?ProjectNo=" + row.ProjectNumber + ">" + row.ProjectNumber + "</a>";
                },
            },
            {
                'data': 'SAPItemNumber'
            },
            {
                'data': 'SAPDescription'
            },
            {
                'data': 'RevisedFirstShipDate',
                'render': function (data, type, row, meta) {
                    if (data == null) {
                        var ShipDate = "";
                    } else {
                        var revisedfirstShip = new Date(data);
                        var ShipDay = revisedfirstShip.getDate();
                        var Shipmonth = revisedfirstShip.getMonth() + 1;
                        var ShipYear = revisedfirstShip.getFullYear();
                        var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                    }
                    return ShipDate;
                }
            },
            {
                'data': 'PackagingNumbers',
                'render': function (data, type, row, meta) {
                    if (row.PackagingNumbers != null) {
                        var pcCode = makeTable(row.PackagingNumbers);
                        return pcCode;
                    } else {
                        return "";
                    }
                }
                //'render': function (data, type, row, meta) {
                //    var PackagingNumbers = "";
                //    var Yes = "Yes";
                //    $.ajax({
                //        url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('Compass Packaging Item List')/items?$select=MaterialNumber,PackagingComponent&$filter=(Deleted ne '" + Yes + "' and CompassListItemId eq '" + row.CompassItemId + "')&$top=500",
                //        method: 'GET',
                //        dataType: 'json',
                //        headers: {
                //            Accept: "application/json;odata=verbose"
                //        },
                //        success: function (results1) {
                //            $.each(results1.d.results, function (index, element) {
                //                PackagingNumbers = PackagingNumbers + element.MaterialNumber + ": " + element.PackagingComponent + ";";
                //            });

                //            if (PackagingNumbers != null) {
                //                var pcCode = makeTable(PackagingNumbers);
                //                return pcCode;
                //            } else {
                //                return "";
                //            }
                //        },
                //        complete: function () {

                //        }
                //    });

                //}
            },
            {
                'data': 'WorkflowPhase',
                'render': function (data, type, row, meta) {
                    var phaseText = "";
                    if (row.WorkflowPhase != null) {
                        phaseText = row.WorkflowPhase;
                    }
                    return phaseText.replace("OBM", "PM");
                }
            },
            {
                'data': 'ProductHierarchyLevel1'
            },
            {
                'data': 'MaterialGroup1Brand'
            },
            {
                'data': 'ManufacturingLocation'
            },
            {
                'data': 'PackingLocation'
            },
            {
                'data': 'ProjectType'
            },
            {
                'data': 'ProjectTypeSubCategory'
            },
            {
                'data': 'PM'
            },
            {
                'data': 'PackagingEngineer'
            },
            {
                'data': 'Initiator'
            },
            {
                'data': 'Customer'
            },
            {
                'data': 'SubmittedDate',
                'render': function (data) {
                    if (data == null) {
                        var prodDate = "";
                    } else {
                        var newfirstProd = new Date(data);
                        var ProdDay = newfirstProd.getDate();
                        var Prodmonth = newfirstProd.getMonth() + 1;
                        var ProdYear = newfirstProd.getFullYear();
                        var prodDate = Prodmonth + "/" + ProdDay + "/" + ProdYear;
                    }
                    return prodDate;
                }
            }
        ]
    });

    /*if ($('#AllOpenProjects tbody tr').length > 0 && !($('#AllOpenProjects tbody tr').length == 1 && $('#AllOpenProjects tbody tr td').hasClass("dataTables_empty"))) {
        $('#allProjectMainDiv').css({ "display": "inline-block" });
    }*/
    var buttons = new $.fn.dataTable.Buttons(myOpenProjects, {
        buttons: [{
            extend: "excelHtml5",
            text: "Export to Excel",
            className: "ButtonControlAutoSize"
        }]
    }).container().appendTo($('#AllOpenProjects_wrapper'));
    loadingIconAdded = true;
}

function AllParentProjectDisplay() {
    var myOpenProjects = $('#AllParentOpenProjects').DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex($(this).val());
                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                        loadingIconAdded = true;
                    });
                column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                    var val = $('<div/>').html(d).text();
                    select.append('<option value="' + val + '">' + val + '</option>');
                });
            });
        },
        data: parentProjectDetails,
        fixedColumns: true,
        "order": [1, 'desc'],
        "lengthMenu": [25, 50, 75, 100],
        columns: [
        {
            'data': 'ProjectNumber'
        },
        {
            'data': 'ProjectTitle'
        },
        {
            'data': 'Gate0ApprovedDate',
            'render': function (data, type, row, meta) {
                if (data == null || data == "") {
                    var ShipDate = "";
                } else {
                    var revisedfirstShip = new Date(data);
                    var ShipDay = revisedfirstShip.getDate();
                    var Shipmonth = revisedfirstShip.getMonth() + 1;
                    var ShipYear = revisedfirstShip.getFullYear();
                    var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                }
                return ShipDate;
            }
        },
        {
            'data': 'DesiredShipDate',
            'render': function (data, type, row, meta) {
                if (data == null || data == "") {
                    var ShipDate = "";
                } else {
                    var revisedfirstShip = new Date(data);
                    var ShipDay = revisedfirstShip.getDate();
                    var Shipmonth = revisedfirstShip.getMonth() + 1;
                    var ShipYear = revisedfirstShip.getFullYear();
                    var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                }
                return ShipDate;
            }
        },
        {
            'data': 'RevisedFirstShipDate',
            'render': function (data, type, row, meta) {
                if (data == null || data == "") {
                    var ShipDate = "";
                } else {
                    var revisedfirstShip = new Date(data);
                    var ShipDay = revisedfirstShip.getDate();
                    var Shipmonth = revisedfirstShip.getMonth() + 1;
                    var ShipYear = revisedfirstShip.getFullYear();
                    var ShipDate = Shipmonth + "/" + ShipDay + "/" + ShipYear;
                }
                return ShipDate;
            }
        },
        {
            'data': 'Stage'
        },
        {
            'data': 'Brand'
        },
        {
            'data': 'SKUs'
        },
        {
            'data': 'ProjectType'
        },
        {
            'data': 'ProjectTypeSubCategory'
        },
        {
            'data': 'PM'
        },
        {
            'data': 'Initiator'
        }

        ]
    });
    /*
    if ($('#AllParentOpenProjects tbody tr').length > 0 && !($('#AllParentOpenProjects tbody tr').length == 1 && $('#AllParentOpenProjects tbody tr td').hasClass("dataTables_empty"))) {
        $('#allParentProjectMainDiv').css({ "display": "inline-block" });
    }*/
    var buttons = new $.fn.dataTable.Buttons(AllParentOpenProjects, {
        buttons: [{
            extend: "excelHtml5",
            text: "Export to Excel",
            className: "ButtonControlAutoSize"
        }]
    }).container().appendTo($('#AllParentOpenProjects_wrapper'));
    loadingIconAdded = true;
}

function sendWFEmail(clicked, WFQuickStep, pageName, itemId, ProjectNumber) {
    var url = "/_layouts/15/Ferrara.Compass/AppPages/DashboardEmailForm.aspx?WFQuickStep=" + WFQuickStep + "&pageName=" + pageName + "&compassId=" + itemId + "&ProjectNo=" + ProjectNumber;
    var options = {
        url: url,
        title: "Re-Send " + WFQuickStep + " Email",
        dialogReturnValueCallback: onPopUpCloseCallBack,
    };
    SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
}

function selectAllOptions(listBox) {
    listBox.find("option").prop('selected', true);
}

function isListBoxEmpty(listBox, dverror_message, error_message) {
    var empty, id;
    empty = listBox.find("option").length == 0;
    if (empty) {
        dvMain = listBox.closest("div.form-group");
        label = dvMain.find('label');
        if (label.length)
            fieldName = label.text().replace(":", "");
        else
            fieldName = listBox.attr('title');
        requiredMsg = fieldName + ' is required';
        id = listBox.attr('id');
        if (!listBox.parent().hasClass('hideItem')) {
            dverror_message.show();
            error_message.append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
        }
    }
    return empty;
}

function ValidateDataGeneric(dverror_message, error_message, selectorToValidate) {
    var dvMain, label, fieldName, requiredMsg;
    var isValid = true;

    $(error_message).empty();
    $(selectorToValidate).each(function (i, obj) {
        dvMain = $(this).closest("div.form-group");
        label = dvMain.find('label');
        if (label.length)
            fieldName = label.text().replace(":", "");
        else
            fieldName = $(this).attr('title');
        requiredMsg = fieldName + ' is required';

        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == "") {
                    isValid = false;
                    $(dverror_message).show();
                    $(error_message).append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
                else {
                    $(this).removeClass('highlightElement');
                }
        } else if ($(this).is('select')) {
            var value = $(this).find("option:selected").val();
            if (!$(this).parent().parent().hasClass('hideItem')) {
                if (value == '-1') {
                    isValid = false;
                    $(dverror_message).show();
                    $(error_message).append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
            } else {
                $(this).removeClass('highlightElement');
            }
        } else {
            var value = $(this).val();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                if (value == '-1') {
                    isValid = false;
                    $(dverror_message).show();
                    $(error_message).append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
        }
    });
    return isValid;
}

function ValidateNutritionals() {
    var isValid = true;
    $('.NewNutRequired').each(function (i, obj) {
        var dvMain = $(this).closest("td");
        var sd = $(this).attr('title') + ' is required';
        var id = $(this).attr('id');
        if ($(this).is('input') || ($(this).is('textarea'))) {
            var value = $(this).val().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if (value == '') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
        } else {
            var value = $(this).find("option:selected").text().trim();
            if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly")) {
                if (value.toLowerCase() == "select..." || value == '') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                } else {
                    $(this).removeClass('highlightElement');
                }
            }
        }
    });
    return isValid;
}

function showMessage(messageKey) {
    if (typeof allMessages != "undefined")
        if (typeof allMessages[messageKey] != "undefined")
            $(".tableCol-75 .row").last().after("<div class='row'><div class='col-xs-12 col-sm-12 col-md-12'><span class='SuccessMessage justifyRight'>" + allMessages[messageKey] + "</span></div></div>");
}

function dateJavaScriptSerializer(data) {
    var jsDate, day, month, year;
    if (data == null)
        return '';
    jsDate = eval('new ' + data.replace(/\//g, ''));
    day = jsDate.getDate();
    month = jsDate.getMonth() + 1;
    year = jsDate.getFullYear();
    return month + "/" + day + "/" + year;
}

function DBConfirm(text) {
    if (confirm(text)) {
        return true;
    } else {
        return false;
    }
}

function ClearDropdwonStyles() {
    $(".WrongItemControl").each(function (i, obj) {
        var id = $(this).attr('id');
        var selectedVal = $("#" + id + " option:selected").val();
        var WrongText = $("#" + id).children("option[value=" + "-9999" + "]").text();
        var hiddenControl = $('#hdn' + id);


        if (selectedVal == "-9999") {
            $(this).attr("style", "background-color:Pink;");
            if (hiddenControl.length) {
                hiddenControl.val(WrongText);
            }
        } else {
            $(this).removeAttr("style");
            $("#" + id).children("option[value=" + "-9999" + "]").hide();
            if (WrongText == "" && hiddenControl.length) {
                WrongText = hiddenControl.val();
            }
            if (WrongText != "") {
                $('[id$=ItemValidationSummary] li:contains(' + WrongText + ')').addClass('hide');
                $('[id^=ItemValidationSummary] li:contains(' + WrongText + ')').addClass('hide');
            }
        }

        if ($('[id$=ItemValidationSummary] li[class!="hide"]').length == 0 && $('[id^=ItemValidationSummary] li[class!="hide"]').length == 0) {
            $('[id$=ItemValidationSummary]').text("");
            $('[id^=ItemValidationSummary]').text("");
        }
    });
}

$(document.body).on("change", ".WrongItemControl", function () {
    var id = $(this).attr('id');
    var selectedVal = $("#" + id + " option:selected").val();
    var WrongText = $("#" + id).children("option[value=" + "-9999" + "]").text();
    var hiddenControl = $('#hdn' + id);

    if (selectedVal == "-9999") {
        $(this).attr("style", "background-color:Pink;");
        if (hiddenControl.length) {
            hiddenControl.val(WrongText);
        }
    } else {
        $(this).removeAttr("style");
        if ($(this).hasClass('highlightElement')) {
            $(this).removeClass('highlightElement')
        }
        $("#" + id).children("option[value=" + "-9999" + "]").hide();
        if (WrongText == "" && hiddenControl.length) {
            WrongText = hiddenControl.val();
        }
        if (WrongText != "") {
            $('[id$=ItemValidationSummary] li:contains(' + WrongText + ')').addClass('hide');
            $('[id^=ItemValidationSummary] li:contains(' + WrongText + ')').addClass('hide');
        }
    }

    if ($('[id$=ItemValidationSummary] li[class!="hide"]').length == 0 && $('[id^=ItemValidationSummary] li[class!="hide"]').length == 0) {
        $('[id$=ItemValidationSummary]').text("");
        $('[id^=ItemValidationSummary]').text("");
    }
});