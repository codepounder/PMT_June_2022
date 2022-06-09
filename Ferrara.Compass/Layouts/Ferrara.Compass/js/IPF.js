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
    $('.MembersTable').each(function (i, obj) {
        $(this).find('.DeleteRow').first().remove();
    });
    $('.ReadOnlyMembers').each(function (i, obj) {
        $(this).prop("readonly", true);
    });
    setActiveTab(gotoStep);
    $('.next-tab, .prev-tab').click(function () {
        setActiveTab($(this).attr("rel"));
        if ("13" == $(this).attr("rel")) {
            btnLoadingIcon($('.actions .submit-tab'));
            $("#btnLoadSummary").click();
            return false;
        }
        return false;
    });
    $("#wizard .steps ul li a").click(function () {
        if (!($(this).parent().hasClass("ui-state-disabled"))) {
            var href = $(this).attr("href");
            var clicked = href.replace('#wizard-h-', '');
            setActiveTab(clicked);
            if ("12" == clicked) {
                $("#btnLoadSummary").click();
                return false;
            }
        }
    });
    $('#loadingIcon').hide();

    $('a[href="#finish"]').click(function () {
        btnLoadingIcon($(this));
        $('.finish').click();
        return false;
    });

    fixMonth();
    ProjectTypeChange();

    $("#ddlCustomerSpecific").each(function () {
        var theValue = $("#ddlCustomerSpecific").val();
        $("#ddlCustomerSpecific").data("ddlvalue", theValue);
    });

    //conditionalChecks();
    marketingClaims();
    $(".vitamin").change(function () {
        if ($(this).find("option:selected").text().toLocaleLowerCase() != "yes") {
            $(this).parent().parent().next().find(".form-group").addClass("hideItem");
        } else {
            $(this).parent().parent().next().find(".form-group").removeClass("hideItem");
        }
    });
    $("#drpMadeInUSA").change(function () {
        if ($(this).find("option:selected").text().toLocaleLowerCase() == "yes") {
            $("#drpMadeInUSAPct").addClass("required");
            $("#drpMadeInUSAPct").parent().parent().removeClass("hideItem");
            $("#drpMadeInUSAPct").parent().prepend("<span class='markrequired'>*</span>");
        } else {
            $("#drpMadeInUSAPct").val("-1");
            $("#drpMadeInUSAPct").removeClass("required");
            $("#drpMadeInUSAPct").parent().parent().addClass("hideItem");
            $("#drpMadeInUSAPct").parent().find(".markrequired").remove();
        }
    });
    $(".add-row").click(function () {
        var desc = $("#fgItemDesc").val().trim();
        var quan = $("#fgItemQuan").val().trim();
        if (desc != '' && quan != '') {
            var markup = "<tr><td><input type='checkbox' name='record'></td><td>" + desc + "</td><td>" + quan + "</td></tr>";
            $("table tbody").append(markup);
        }
    });

    // Find and remove selected table rows
    $(".delete-row").click(function () {
        $("table tbody").find('input[name="record"]').each(function () {
            if ($(this).is(":checked")) {
                $(this).parents("tr").remove();
            }
        });
    });
    if ($("#hdnSteps").val() == "12") {
        LoadSummaryData();
    }

    //SAP Nomenclature
    //SAPNomenclature();

    $('#txtProductFormDescription').keyup(function () {
        SAPNomenclature();
    });

    RetailUnitWeightRequirement();
    AutoPopulateBaseUofMNetWeight();
    BindHierarchiesOnLoad();
    ShowHideApprovedGraphicsAsset();
});
function deleteRow(clicked, hdnDeletedStatus) {
    $('#error_message').empty();
    var button = $(clicked);
    button.closest("tr").addClass("hideItem");
    button.closest("td").find("#" + hdnDeletedStatus).val("true");
}
function AddProjectTeamMembersRow(btnId) {
    $('#' + btnId).click();
}
function AddProjectTeamMembersRow_New(clicked, btnId) {
    var button = $(clicked);
    var MemberDiv = button.closest(".MemberDiv");
    var ddlMember = MemberDiv.find('.ddlMember');
    if (ddlMember != "-1") {
        $('#' + btnId).click();
    }
}
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
function btnLoadingIcon(clicked) {
    var top = clicked.position().top + 7;
    var left = clicked.position().left + 4;
    var width = clicked.outerWidth();
    var height = clicked.outerHeight();
    clicked.parent().append("<div class='disablingLoadingIcon' style='top:" + top + "px;width:" + width + "px;height:" + height + "px;left:" + left + "px;'>&nbsp;</div>");
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
// ProjectTypeChange
function SingleProjectTypeChange() {
    var projectType = $("#ddlProjectType option:selected").text();
    if (projectType == "Simple Network Move" || projectType == "Graphics Change Only") {
        $("#ddlTBDIndicator").val("N");
        $("#ddlTBDIndicator option[value='N']").prop("selected", true);
        $("#ddlTBDIndicator").prop('disabled', true);
        $("#ddlTBDIndicator").prop('readonly', true);
    } else if (projectType == 'Renovation & Quality Improvement') {
        $("#ddlTBDIndicator").val("N");
        $("#ddlTBDIndicator option[value='N']").prop("selected", true);
        $("#ddlTBDIndicator").prop('disabled', false);
        $("#ddlTBDIndicator").prop('readonly', false);
    }
    else {
        if ($("#ddlTBDIndicator option:selected").text() == "Select...") {
            $("#ddlTBDIndicator").val("Y");
            $("#ddlTBDIndicator option[value='Y']").prop("selected", true);
        }
        $("#ddlTBDIndicator").prop('disabled', false);
        $("#ddlTBDIndicator").prop('readonly', false);
    }

}
function ProjectTypeChange() {
    var projectType = $("#ddlProjectType option:selected").text();

    $("#lblChangeNotes").html("Item Concept:");
    $("#lblItemNote").html("Please include the business case for this project as part of your Item Concept.");
    getProjectDescriptionType(projectType);
    var url = window.location.href;
    var pieces = url.split("?");
    var checkIfnew = "";
    if (pieces.length > 1) {
        var q = pieces[1].split("=");
        checkIfnew = q[0];
    }
    if (projectType == "Simple Network Move") {
        if (checkIfnew.toLocaleLowerCase() == 'projectno') {
            var tabsToDisable = [3, 4, 5, 6, 8, 9, 10];
            $("#wizard").tabs("option", "disabled", tabsToDisable);
            $.each(tabsToDisable, function (i, e) {
                $('section').eq(e).find(".required").addClass("requiredHolder").removeClass("required");
            });
            $(".HideForSNM").addClass("hideItem");
            ClearErrorsIfTabIsDisabled();
        }
        $("#ddlProjectTypeSubCategory option").each(function () {
            if ($(this).text() == "Complex Network Move") {
                $(this).remove();
            }
        });
        $("#ddlTBDIndicator").val("N");
        $("#ddlTBDIndicator option[value='N']").prop("selected", true);
        $("#ddlTBDIndicator").prop('disabled', true);
        $("#ddlTBDIndicator").prop('readonly', true);

    } else {
        if (checkIfnew.toLocaleLowerCase() == 'projectno') {
            $("#wizard").tabs("enable");

            $('section').find(".requiredHolder").addClass("required").removeClass("requiredHolder");
        }
        var hasComplex = false;
        $("#ddlProjectTypeSubCategory option").each(function () {
            if ($(this).text() == "Complex Network Move") {
                hasComplex = true;
            }
        });
        if (hasComplex == false && projectType != "Graphics Change Only") {
            $("#ddlProjectTypeSubCategory").append('<option value="2">Complex Network Move</option>');
        }
        $("#ddlTBDIndicator").prop('disabled', false);
        $("#ddlTBDIndicator").prop('readonly', false);
        $(".HideForSNM").removeClass("hideItem");

        if (projectType == "Graphics Change Only") {
            $("#ddlProjectTypeSubCategory option:contains('Complex Network Move')").remove();

            if (checkIfnew.toLocaleLowerCase() == 'projectno') {
                var tabsToDisable = [4];
                $("#wizard").tabs("option", "disabled", tabsToDisable);
                $.each(tabsToDisable, function (i, e) {
                    $('section').eq(e).find(".required").addClass("requiredHolder").removeClass("required");
                });
                $(".HideGraphics").addClass("hideItem");
            }
        } else {
            if (checkIfnew.toLocaleLowerCase() == 'projectno') {
                $("#wizard").tabs("enable");
                $('section').find(".requiredHolder").addClass("required").removeClass("requiredHolder");
            }
            $(".HideGraphics").removeClass("hideItem");
        }
    }

    if (projectType.indexOf("Process Development") != -1 || projectType == 'Line Extension') {

        hideItem('lblSummaryServingSize');
        hideItem('lblSummaryServingSizeLabel');
        $("#ddlServiceSizeChange option[value='-1']").prop("selected", true);
        $("#ddlServiceSizeChange").val("-1");

        //hideItem('dvlast12');
        //$("#dvlast12").find(".input-group").addClass("hideItem");
        //hideItem('lblSummaryLast12MonthSales');
        //hideItem('lblSummaryLast12MonthSalesLabel');
        //$("#txtLast12MonthSales").val("");

        hideItem('dvSwitchDate');
        $("#dvSwitchDate .markrequired").addClass("hideItem");
        $("#dvSwitchDate  #txtExpectedFilmSwitch").removeClass("required");
        hideItem('lblSummaryExpectedPackagingSwitchDate');
        hideItem('lblSummaryExpectedPackagingSwitchDateLabel');
        $("#txtExpectedFilmSwitch").val("");

        //hideItem('dvReplacementItemNumber');

        showItem('dvNewFormula');
        showItem('lblSummaryNewFormula');
        showItem('lblSummaryNewFormulaLabel');

        $("#txtExpectedGrossMarginPercent").addClass('required');
        showItem('lblSummaryExpectedGrossMarginPercent');
        showItem('lblSummaryExpectedGrossMarginPercentLabel');

        if ($("#ddlNeedNewUPCUCC").val() == "-1") {
            $("#ddlNeedNewUPCUCC option[value='Y']").prop("selected", true);
            $("#ddlNeedNewUPCUCC").val("Y");
        }

        //$("#ddlNeedNewUPCUCC").prop('disabled', true);
        //$("#ddlNeedNewUPCUCC").prop('readonly', true);

        $("#txtAnnualProjectedDollars").addClass('required');
    }
    if (projectType == 'Downweight/Transitions') {
        //showItem('dvlast12');
        //$("#dvlast12").find(".input-group").removeClass("hideItem");
        //showItem('lblSummaryLast12MonthSales');
        //showItem('lblSummaryLast12MonthSalesLabel');

        showItem('dvNewFormula');
        showItem('lblSummaryNewFormula');
        showItem('lblSummaryNewFormulaLabel');

        //showItem('dvReplacementItemNumber');
        showItem('dvSwitchDate');
        $("#dvSwitchDate .markrequired").removeClass("hideItem");
        $("#dvSwitchDate  #txtExpectedFilmSwitch").addClass("required");
        $("#txtAnnualProjectedDollars").removeClass('required');
    }

    if (projectType == 'Graphics Changes/Internal Adjustments' || projectType == "Graphics Change Only") {
        $("#lblChangeNotes").html("Change Notes:");
        $("#lblItemNote").html("Please include the business case for this project as part of your Change Notes.");
        $("#ddlNewFormula option[value='N']").prop("selected", true);
        $("#ddlNewShape option[value='N']").prop("selected", true);
        $("#ddlNewFlavorColor option[value='N']").prop("selected", true);
        $("#ddlNewNetWeight option[value='N']").prop("selected", true);
        $("#ddlNewFormula").val("N");
        $("#ddlNewShape").val("N");
        $("#ddlNewFlavorColor").val("N");
        $("#ddlNewNetWeight").val("N");
        $("#ddlNewFormula").prop('disabled', true);
        $("#ddlNewFormula").prop('readonly', true);
        $("#ddlNewShape").prop('disabled', true);
        $("#ddlNewShape").prop('readonly', true);
        $("#ddlNewFlavorColor").prop('disabled', true);
        $("#ddlNewFlavorColor").prop('readonly', true);
        $("#ddlNewNetWeight").prop('disabled', true);
        $("#ddlNewNetWeight").prop('readonly', true);
        hideItem('lblSummaryNewFormula');
        hideItem('lblSummaryNewFormulaLabel');
        hideItem('dvFCC');
        $("#txtExpectedGrossMarginPercent").removeClass('required');
        $("#dvFCC .markrequired").addClass("hideItem");
        $("#txtExpectedGrossMarginPercent").val("");
        hideItem('lblSummaryExpectedGrossMarginPercent');
        hideItem('lblSummaryExpectedGrossMarginPercentLabel');

        showItem('lblSummaryServingSize');
        showItem('lblSummaryServingSizeLabel');

        //showItem('dvlast12');
        //$("#dvlast12").find(".input-group").removeClass("hideItem");
        //showItem('lblSummaryLast12MonthSales');
        //showItem('lblSummaryLast12MonthSalesLabel');

        showItem('dvSwitchDate');
        $("#dvSwitchDate .markrequired").removeClass("hideItem");
        $("#dvSwitchDate  #txtExpectedFilmSwitch").addClass("required");
        showItem('lblSummaryExpectedPackagingSwitchDate');
        showItem('lblSummaryExpectedPackagingSwitchDateLabel');

        //showItem('dvReplacementItemNumber');

        // $("#txtAnnualProjectedDollars").addClass('required');
        $("#ddlTBDIndicator option[value='N']").prop("selected", true);
        $("#ddlTBDIndicator").val("N");
        $("#ddlTBDIndicator").prop('disabled', true);
        $("#ddlTBDIndicator").prop('readonly', true);
        $("#ddlNeedNewUPCUCC").prop('disabled', true);
        $("#ddlNeedNewUPCUCC").prop('readonly', true);
        hideItem('dvFormulationDoc');

    } else {
        showItem('dvFormulationDoc');
        $("#ddlNewFormula").prop('disabled', false);
        $("#ddlNewFormula").prop('readonly', false);
        $("#ddlNewShape").prop('disabled', false);
        $("#ddlNewShape").prop('readonly', false);
        $("#ddlNewFlavorColor").prop('disabled', false);
        $("#ddlNewFlavorColor").prop('readonly', false);
        $("#ddlNewNetWeight").prop('disabled', false);
        $("#ddlNewNetWeight").prop('readonly', false);
        showItem('dvFCC');
        $("#txtExpectedGrossMarginPercent").addClass('required');
        $("#dvFCC .markrequired").removeClass("hideItem");
        showItem('lblSummaryExpectedGrossMarginPercent');
        showItem('lblSummaryExpectedGrossMarginPercentLabel');
        $("#ddlNeedNewUPCUCC").prop('disabled', false);
        $("#ddlNeedNewUPCUCC").prop('readonly', false);
    }
    if (projectType == 'Renovation & Quality Improvement' || projectType == 'Downweight/Transitions') {
        showItem('dvServingSize');
        showItem('lblSummaryServingSize');
        showItem('lblSummaryServingSizeLabel');

    } else {
        hideItem("dvServingSize");
        hideItem('lblSummaryServingSize');
        hideItem('lblSummaryServingSizeLabel');
    }
    if (projectType == 'Simple Network Move' || projectType == "Graphics Change Only") {
        if ($("#ddlNeedNewUPCUCC").val() == "-1") {
            $("#ddlNeedNewUPCUCC option[value='N']").prop("selected", true);
            $("#ddlNeedNewUPCUCC").val("N");
        }
        if ($("#ddlNeedNewUnitUPC").val() == "-1") {
            $("#ddlNeedNewUnitUPC option[value='N']").prop("selected", true);
            $("#ddlNeedNewUnitUPC").val("N");
        }
        if ($("#ddlNeedNewDisplayBoxUPC").val() == "-1") {
            $("#ddlNeedNewDisplayBoxUPC option[value='N']").prop("selected", true);
            $("#ddlNeedNewDisplayBoxUPC").val("N");
        }
        if ($("#ddlNeedNewCaseUCC").val() == "-1") {
            $("#ddlNeedNewCaseUCC option[value='N']").prop("selected", true);
            $("#ddlNeedNewCaseUCC").val("N");
        }
        if ($("#ddlNeedNewPalletUCC").val() == "-1") {
            $("#ddlNeedNewPalletUCC option[value='N']").prop("selected", true);
            $("#ddlNeedNewPalletUCC").val("N");
        }
    }
    if ($("#ddlTBDIndicator option:selected").text() == "Select...") {
        if (projectType != 'Renovation & Quality Improvement' && projectType != 'Simple Network Move' && projectType != "Graphics Change Only") {
            $("#ddlTBDIndicator option[value='Y']").prop("selected", true);
            $("#ddlTBDIndicator").val("Y");
        }
    }
    if ($("#ddlNewFormula option:selected").text() == "Select...") {
        if (projectType == 'Renovation & Quality Improvement' || projectType.indexOf("Process Development") != -1) {
            $("#ddlNewFormula option[value='Y']").prop("selected", true);
            $("#ddlNewFormula").val("Y");
        } else if (projectType == 'New Pack Type' || projectType == 'Line Extension' || projectType == 'Downweight/Transitions') {
            $("#ddlNewFormula option[value='N']").prop("selected", true);
            $("#ddlNewFormula").val("N");
        }
    }
    if ($("#ddlNewShape option:selected").text() == "Select...") {
        if (projectType.indexOf("Process Development") != -1) {
            $("#ddlNewShape option[value='Y']").prop("selected", true);
            $("#ddlNewShape").val("Y");
        } else if (projectType == 'New Pack Type' || projectType == 'Line Extension' || projectType == 'Downweight/Transitions' || projectType == 'Renovation & Quality Improvement') {
            $("#ddlNewShape option[value='N']").prop("selected", true);
            $("#ddlNewShape").val("N");
        }
    }
    if ($("#ddlNewFlavorColor option:selected").text() == "Select...") {
        if (projectType.indexOf("Process Development") != -1 || projectType == 'Line Extension' || projectType == 'Renovation & Quality Improvement') {
            $("#ddlNewFlavorColor option[value='Y']").prop("selected", true);
            $("#ddlNewFlavorColor").val("Y");
        } else if (projectType == 'New Pack Type' || projectType == 'Downweight/Transitions') {
            $("#ddlNewFlavorColor option[value='N']").prop("selected", true);
            $("#ddlNewFlavorColor").val("N");
        }
    }
    if ($("#ddlNewNetWeight option:selected").text() == "Select...") {
        if (projectType == 'Renovation & Quality Improvement') {
            $("#ddlNewNetWeight option[value='N']").prop("selected", true);
            $("#ddlNewNetWeight").val("N");
        } else if (projectType == 'New Pack Type' || projectType == 'Line Extension' || projectType == 'Downweight/Transitions' || projectType.indexOf("Process Development") != -1) {
            $("#ddlNewNetWeight option[value='Y']").prop("selected", true);
            $("#ddlNewNetWeight").val("Y");
        }
    }
    conditionalChecks();
    UpdateRequiredAttributeForPeopleEditors();
    ShowHideApprovedGraphicsAsset();
}
function UpdateRequiredAttributeForPeopleEditors() {
    if ($("#ddlProjectType option:selected").text() == 'Simple Network Move') {
        $('.SNWChangeRequiredOptionalPeopleEditor').each(function (i, obj) {
            $(this).addClass('hide');
        });

        $("[id^=hdnRequired]").each(function (i, obj) {
            $(this).val('False');
        });
    }
    else {
        $('.SNWChangeRequiredOptionalPeopleEditor').each(function (i, obj) {
            $(this).removeClass('hide');
        });

        $("[id^=hdnRequired]").each(function (i, obj) {
            $(this).val('True');
        });
    }
}
function ShowHideApprovedGraphicsAsset() {
    var projectType = $("#ddlProjectType option:selected").text();

    if (projectType == "Graphics Change Only") {
        if (!$("#ddlCopyFormsForGraphicsProject").hasClass("required")) {
            $("#ddlCopyFormsForGraphicsProject").addClass("required");
            $("#ddlCopyFormsForGraphicsProject").parent().parent().removeClass("hideItem");
            $("#ddlCopyFormsForGraphicsProject").parent().prepend("<span class='markrequired'>*</span>");
            $('.ddlCopyFormsForGraphicsProject').removeClass('hide');
        }

        if (!$("#ddlExternalSemisItem").hasClass("required")) {
            $("#ddlExternalSemisItem").addClass("required");
            $("#ddlExternalSemisItem").parent().parent().removeClass("hideItem");
            $("#ddlExternalSemisItem").parent().prepend("<span class='markrequired'>*</span>");
            $('.ddlExternalSemisItem').removeClass('hide');
        }

        $('.ApprovedGraphicsAsset').each(function (i, obj) {
            var drpNewId = $(this).closest('.bomrow').find('.drpNewClass').attr('id');
            var drpGraphicsId = $(this).closest('.bomrow').find('.drpGraphics').attr('id');
            var drpComponentTypeId = $(this).closest('.bomrow').find('.drpComponentType').attr('id');
            var NewExisting = $("#" + drpNewId + " option:selected").text();
            var GraphicsRequired = $("#" + drpGraphicsId + " option:selected").text();
            var CompType = $("#" + drpComponentTypeId + " option:selected").text();

            if (NewExisting == "New" && GraphicsRequired == "Yes" && CompType.indexOf('Transfer') == -1 && CompType.indexOf('Candy') == -1) {
                $(this).removeClass('hide');
            } else {
                $(this).addClass('hide');
            }
        });
    } else {
        $("#ddlCopyFormsForGraphicsProject").removeClass("required");
        $("#ddlCopyFormsForGraphicsProject").parent().parent().addClass("hideItem");
        $("#ddlCopyFormsForGraphicsProject").parent().find(".markrequired").remove();
        $('.ddlCopyFormsForGraphicsProject').addClass('hide');
        $('#ddlCopyFormsForGraphicsProject').val("-1");
        $("#ddlExternalSemisItem").removeClass("required");
        $("#ddlExternalSemisItem").parent().parent().addClass("hideItem");
        $("#ddlExternalSemisItem").parent().find(".markrequired").remove();
        $('.ddlExternalSemisItem').addClass('hide');
        $('#ddlExternalSemisItem').val("-1");

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
function RemoveRequired() {
    var arrText = new Array();
    $('input[type=text]').each(function () {
        if (!$(this).hasClass('required')) {
            var span = $(this).closest('span');
            span.hide();
        }
    })
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
function GotoPeoplePickerStepAndFocus(arg) {
    /// 0: Initiator, 1: Brand Manager, 2:PM
    setActiveTab(1)
    $("div[title='People Picker']").eq(arg).addClass("highlightElement");
    $("div[title='People Picker']").eq(arg).focus();
}
function LoadSummaryData() {
    // Propoased Project
    $("#lblSummaryProjectTypeSubCategory").html($("#ddlProjectTypeSubCategory option:selected").text());
    $("#lblSummaryProjectType").html($("#ddlProjectType option:selected").text());
    $("#lblSummaryFirstShipDate").html($("#txtFirstShipDate").val());

    $("#lblCopyFormsForGraphicsProject").html($("#ddlCopyFormsForGraphicsProject").val());
    if ($("#hdnIPFSubmitted").val() == "Yes") {
        $("#lblExternalSemisItem").html($("#hdnExternalSemisItem").val());
    } else {
        $("#lblExternalSemisItem").html($("#ddlExternalSemisItem").val());
    }

    if ($("#ddlProjectType option:selected").text() == "Graphics Change Only") {
        $('.divExternalSemisItem').removeClass('hide');
        $('.divCopyFormsForGraphicsProjectText').removeClass('hide');
    } else {
        $('.divExternalSemisItem').addClass('hide');
        $('.divCopyFormsForGraphicsProjectText').addClass('hide');
    }
    $("#lblSummaryChangeNotes").html($("#txtChangeNotes").text());
    $("#lblSummaryChangeNotesLabel").html($("#lblChangeNotes").text());

    //Project Team

    // SAP Item #
    $("#lblSummaryIndicator").html($("#ddlTBDIndicator option:selected").text());
    $("#lblSummaryFGItemNumber").html($("#txtSAPItemNumber").val());
    $("#lblSummaryFGItemDesc").html($("#txtSAPItemDescription").val());
    $("#lblSummaryLikeFGItemNumber").html($("#txtLikeFGItemNumber").val());
    $("#lblSummaryLikeFGItemDesc").html($("#txtLikeItemDescription").val());
    $("#lblSummaryOldFGItemNumber").html($("#txtOldFGItemNumber").val());
    $("#lblSummaryOldFGItemDesc").html($("#txtOldItemDescription").val());

    //Project Specifications
    $("#lblSummaryOrganic").html($("#ddlOrganic option:selected").text());

    $("#lblSummaryNewFormula").html($("#ddlNewFormula option:selected").text());
    $("#lblSummaryNewShape").html($("#ddlNewShape option:selected").text());
    $("#lblSummaryNewFlavorColor").html($("#ddlNewFlavorColor option:selected").text());
    $("#lblSummaryNewNetWeight").html($("#ddlNewNetWeight option:selected").text());

    if (!$("#dvServingSize").hasClass('.hideItem'))
        $("#lblSummaryServingSize").html($("#ddlServiceSizeChange option:selected").text());

    //Item Financial Details
    if (!$("#dvlast12").hasClass('.hideItem'))
        $("#lblSummaryLast12MonthSales").html("$ " + $("#txtLast12MonthSales").val());

    if (!$("#dvFCC").hasClass('.hideItem'))
        $("#lblSummaryExpectedGrossMarginPercent").html($("#txtExpectedGrossMarginPercent").val());

    $("#lblSummaryTruckLoadSellingPrice").html("$ " + $("#txtTruckLoadSellingPrice").val());
    $("#lblSummaryAnnualProjectedUnits").html($("#txtAnnualProjectedUnits").val());
    $("#lblSummaryAnnualProjectedDollars").html("$ " + $("#txtAnnualProjectedDollars").val());
    $("#lblSummary1stAnnualProjectedUnits").html($("#txtMonth1ProjectedUnits").val());
    $("#lblSummary1stAnnualProjectedDollars").html("$ " + $("#txtMonth1ProjectedDollars").val());
    $("#lblSummary2ndAnnualProjectedUnits").html($("#txtMonth2ProjectedUnits").val());
    $("#lblSummary2ndAnnualProjectedDollars").html("$ " + $("#txtMonth2ProjectedDollars").val());
    $("#lblSummary3rdAnnualProjectedUnits").html($("#txtMonth3ProjectedUnits").val());
    $("#lblSummary3rdAnnualProjectedDollars").html("$ " + $("#txtMonth3ProjectedDollars").val());

    ///Customer Specifications
    $("#lblSummaryCustomerChannelSpecific").html($("#ddlCustomerSpecific option:selected").text());

    if (!$("#dvCustomer").hasClass('.hideItem'))
        $("#lblSummaryCustomer").html($("#ddlCustomer option:selected").text());

    if (!$("#dvCustomerSpecific").hasClass('.hideItem'))
        $("#lblSummaryCustomerSpecificLotCode").html($("#txtCustomerSpecificLotCode").val());

    if (!$("#dvChannel").hasClass('.hideItem'))
        $("#lblSummaryChannel").html($("#ddlChannel option:selected").text());

    $("#lblSummaryOutsideUSA").html($("#ddlOutsideUSA option:selected").text());

    if (!$("#dvCountryofSale").hasClass('.hideItem'))
        $("#lblSummaryCountryOfSale").html($("#ddlCountryOfSale option:selected").text());

    // Item Hierarchy
    var SAPNomnclature = false;
    var CoMan = false;

    if ($('#hdnNewIPF').val() == "Yes" && $("#ddlTBDIndicator option:selected").text() == "Yes") {
        SAPNomnclature = true;
        if ($("#ddlProductHierarchyLevel1 option:selected").text() == "Co-Manufacturing (000000027)") {
            CoMan = true;
            if ($("#ddlManuallyCreateSAPDescription option:selected").text() != "No") {
                SAPNomnclature = false;
            }
        }
    }


    $("#lblSummaryProductHierarchyLevel1").html($(".HierarchyLevel1 option:selected").text());
    hideItem('divSummaryManuallyCreateSAPDescription');
    if (CoMan) {
        showItem('divSummaryManuallyCreateSAPDescription');
        $("#lblSummaryManuallyCreateSAPDescription").html($("#ddlManuallyCreateSAPDescription option:selected").text());
    }

    $("#lblSummaryProductHierarchyLevel2").html($("#ddlProductHierarchyLevel2 option:selected").text());
    $("#lblSummaryMaterialGroup1Brand").html($("#ddlBrand_Material option:selected").text());
    $("#lblSummaryMaterialGroup4ProductForm").html($("#ddlMaterialGroup4 option:selected").text());


    hideItem('divSummaryProductFormDescription');
    if (SAPNomnclature) {
        showItem('divSummaryProductFormDescription');
        $("#lblSummaryProductFormDescription").html($("#txtProductFormDescription").val());
    }

    $("#lblSummaryMaterialGroup5PackType").html($("#ddlMaterialGroup5 option:selected").text());
    $("#lblSummaryNoveltyProject").html($("#drpNovelyProject option:selected").text());

    if ($("#ddlMaterialGroup5 option:selected").text().toLowerCase() == 'shipper (shp)' || $("#ddlMaterialGroup5 option:selected").text().toLowerCase() == 'shippers (shp)') {
        $("#lblSummary3QuantityofUnitsInDisplay").html($("#drpFGTotalQuantityUnitsInDisplay option:selected").text());
    }

    //Item UPCs
    $("#lblSummaryNeedNewUPCUCC").html($("#ddlNeedNewUPCUCC option:selected").text());

    if (!$("#dvddlNeedNewUPC").hasClass('.hideItem'))
        $("#lblSummaryNeedNewUnitUPC").html($("#ddlNeedNewUnitUPC option:selected").text());

    if (!$("#dvUnitUPC").hasClass('.hideItem'))
        $("#lblSummaryUnitUPC").html($("#txtUnitUPC").val());

    if (!$("#dvNeedNewDisplayBoxUPC").hasClass('.hideItem'))
        $("#lblSummaryNeedNewDisplayBoxUPC").html($("#ddlNeedNewDisplayBoxUPC option:selected").text());

    if (!$("#dvDisplayBoxUPC").hasClass('.hideItem'))
        $("#lblSummaryDisplayUPCBox").html($("#txtDisplayUPCBox").val());

    if (!$("#dvSAPBaseUOM").hasClass('.hideItem'))
        $("#lblSummarySAPBaseUOM").html($("#ddlSAPBUOM  option:selected").text());

    if (!$("#dvNeedNewCaseUCC").hasClass('.hideItem'))
        $("#lblSummaryNeedNewCaseUCC").html($("#ddlNeedNewCaseUCC  option:selected").text());

    if (!$("#dvCaseUCC").hasClass('.hideItem'))
        $("#lblSummaryCaseUCC").html($("#txtCaseUCC").val());

    if (!$("#dvNeedNewPalletUCC").hasClass('.hideItem'))
        $("#lblSummaryNeedNewPalletUCC").html($("#ddlNeedNewPalletUCC option:selected").text());

    if (!$("#dvPalletUCC").hasClass('.hideItem'))
        $("#lblSummaryPalletUCC").html($("#txtPalletUCC").val());

    //if (!$("#dvFlowthrough").hasClass('.hideItem'))
    $("#lblSummaryPalletUCC").html($("#txtPalletUCC").val());


    // Additional Item Details
    $("#lblSummaryFilmSubstrate").html($("#ddlFilmSubstrate option:selected").text());

    $("#lblSummaryCaseType").html($("#ddlCaseType option:selected").text());
    $("#lblSummaryPegHoleNeeded").html($("#ddlPegHoleNeeded option:selected").text());

    hideItem('divlblforSummaryInvolvesCarton');
    hideItem('divlblSummaryInvolvesCarton');
    hideItem('divSummaryUnitsInsideCarton');
    hideItem('divSummaryNumberofTraysPerBaseUOM1');
    hideItem('divSummaryNumberofTraysPerBaseUOM2');
    if (SAPNomnclature) {
        showItem('divlblforSummaryInvolvesCarton');
        showItem('divlblSummaryInvolvesCarton');

        $("#lblSummaryInvolvesCarton").html($("#ddlInvolvesCarton option:selected").text());

        if ($("#ddlInvolvesCarton option:selected").text() == "Yes") {
            showItem('divSummaryUnitsInsideCarton');
            showItem('divSummaryNumberofTraysPerBaseUOM1');
            showItem('divSummaryNumberofTraysPerBaseUOM2');
            $("#lblSummaryUnitsInsideCarton").html($("#txtUnitsInsideCarton").val());
            $("#lblSummaryIndividualPouchWeight").html($("#txtIndividualPouchWeight").val());
            $("#lblSummaryNumberofTraysPerBaseUOM").html($("#txtNumberofTraysPerBaseUOM").val());
        }
    }

    $("#lblSummaryClaimsLabelingRequirements").html($("#txtClaimsLabelingRequirements").val());
    $("#lblSummaryRetailSellingUnitsPerBaseUOM").html($("#txtRetailSellingUnitsPerBaseUOM").val());
    $("#lblSummaryRetailUnitWeight").html($("#txtRetailUnitWeight").val());
    $("#lblSummaryBaseUOMNetWeight").html($("#txtBaseUofMNetWeight").val());
    $("#lblSummaryClaimsDesired").html($("#drpDesiredClaims option:selected").text());
    $("#lblSummarySellableUnit").html($("#drpSellableUnit option:selected").text());
    $("#lblSummaryNewNLEAFormat").html($("#drpNewNLEAFormat option:selected").text());
    $("#lblSummaryBioEngineeringLabelingAcceptable").html($("#drpBioEngineeringLabelingAcceptable option:selected").text());
    $("#lblSummaryMadeInUSA").html($("#drpMadeInUSA option:selected").text());
    if ($("#drpMadeInUSA option:selected").text().toLowerCase() == "yes") {
        $("#lblSummaryMadeInUSAPct").html($("#drpMadeInUSAPct option:selected").text());
        showItem('lblSummaryMadeInUSAPct');
        showItem('lblSummaryMadeInUSAPctLabel');
    }
    else {
        hideItem('lblSummaryMadeInUSAPct');
        hideItem('lblSummaryMadeInUSAPctLabel');
    }
    $("#lblSummaryMarketingOrganic").html($("#drpOrganic option:selected").text());
    $("#lblSummaryGMOClaim").html($("#drpGMOClaim option:selected").text());
    $("#lblSummaryGlutenFree").html($("#drpGlutenFree option:selected").text());
    $("#lblSummaryFatFree").html($("#drpFatFree option:selected").text());
    $("#lblSummaryKosher").html($("#drpKosher option:selected").text());
    $("#lblSummaryNaturalColors").html($("#drpNaturalColors option:selected").text());
    $("#lblSummaryNaturalFlavors").html($("#drpNaturalFlavors option:selected").text());
    $("#lblSummaryPreservativeFree").html($("#drpPreservativeFree option:selected").text());
    $("#lblSummaryLactoseFree").html($("#drpLactoseFree option:selected").text());
    $("#lblSummaryJuiceConcentrate").html($("#txtJuiceConcentrate").text());
    $("#lblSummaryLowSodium").html($("#drpLowSodium option:selected").text());
    $("#lblSummaryGoodSource").html($("#hdnSelectedGoodSource").val());
    $("#lblSummaryPotassiumPct").html($("#drpPotassiumPct option:selected").text());
    $("#lblSummaryIronPct").html($("#drpIronPct option:selected").text());
    $("#lblSummaryCalciumPct").html($("#drpCalciumPct option:selected").text());
    $("#lblSummaryAllergenAlmonds").html($("#drpAllergenAlmonds option:selected").text());
    $("#lblSummaryAllergenCoconut").html($("#drpAllergenCoconut option:selected").text());
    $("#lblSummaryAllergenEggs").html($("#drpAllergenEggs option:selected").text());
    $("#lblSummaryAllergenMilk").html($("#drpAllergenMilk option:selected").text());
    $("#lblSummaryAllergenPeanuts").html($("#drpAllergenPeanuts option:selected").text());
    $("#lblSummaryAllergenSoy").html($("#drpAllergenSoy option:selected").text());
    $("#lblSummaryAllergenWheat").html($("#drpAllergenWheat option:selected").text());

    if (!$(".existingClaims").hasClass('.hideItem'))
        $("#lblSummaryFinishedGoodComponent").html($("#txtMaterialClaimsCompNumber").val());

    if (!$(".existingClaims").hasClass('.hideItem'))
        $("#lblSummaryFinishedGoodComponentDescription").html($("#txtMaterialClaimsCompDesc").val());

    if (!$("#dvSwitchDate").hasClass('.hideItem'))
        $("#lblSummaryExpectedPackagingSwitchDate").html($("#txtExpectedFilmSwitch").val());
    $("#lblSummaryProfitCenter").html($("#txtProfitCenter").val());
    //Marketing Claims Details

    HideBlankSummary();
    //copyFGBOMItemstoSummary();
}

function HideBlankSummary() {
    $('.summarytest').each(function () {
        if ($(this).html() == '' || $(this).html().toLowerCase().indexOf('select') != -1) {
            $($(this).closest("div").prev()).addClass('hideItem');
            $($(this).parent()[0]).addClass('hideItem');
        }

    });
}

function elementFocus(arg) {
    $('#' + arg).focus();
}

function hideItem(arg) {
    $("#" + arg).removeClass('showItem').addClass('hideItem');
}

function showItem(arg) {
    $("#" + arg).removeClass('hideItem').addClass('showItem');
}

function conditionalChecks() {
    var previousValue = $("#ddlCustomerSpecific").data("ddlvalue");

    // setting the new previous value
    var theValue = $("#ddlCustomerSpecific").val();
    $("#ddlCustomerSpecific").data("ddlvalue", theValue);
    if ($("#ddlCustomerSpecific").val() == 'CU') {
        if (previousValue == 'N') {
            $("#ddlCustomer option[value='-1']").prop("selected", true);
            $("#ddlCustomer").val("-1");
        }
        showItem('dvCustomer');
        showItem('lblSummaryCustomer');
        showItem('lblSummaryCustomerLabel');
        showItem('dvCustomerSpecific');
        showItem('lblSummaryCustomerSpecificLotCode');
        showItem('lblSummaryCustomerSpecificLotCodeLabel');
        hideItem('dvChannel');
        $("#ddlChannel option[value='-1']").prop("selected", true);
        $("#ddlChannel").val("-1");
        hideItem('lblSummaryChannel');
        hideItem('lblSummaryChannelLabel');
    }
    else if ($("#ddlCustomerSpecific").val() == 'CH') {
        showItem('dvChannel');
        showItem('lblSummaryChannel');
        showItem('lblSummaryChannelLabel');
        hideItem('dvCustomer');
        $("#ddlCustomer option[value='-1']").prop("selected", true);
        $("#ddlCustomer").val("-1");
        hideItem('lblSummaryCustomer');
        hideItem('lblSummaryCustomerLabel');
        hideItem('dvCustomerSpecific');
        $("#txtCustomerSpecificLotCode").val("");
        hideItem('lblSummaryCustomerSpecificLotCode');
        hideItem('lblSummaryCustomerSpecificLotCodeLabel');
    }
    else if ($("#ddlCustomerSpecific").val() == 'N') {
        showItem('dvCustomer');
        if ($("#ddlCustomer").val() == '-1' || (previousValue == 'CU'))
            $('#ddlCustomer option:contains("PRICELIST")').prop('selected', true);
        showItem('lblSummaryCustomer');
        showItem('lblSummaryCustomerLabel');
        hideItem('dvCustomerSpecific');
        hideItem('lblSummaryCustomerSpecificLotCode');
        hideItem('lblSummaryCustomerSpecificLotCodeLabel');
        hideItem('dvChannel');
        $("#ddlChannel option[value='-1']").prop("selected", true);
        $("#ddlChannel").val("-1");
        hideItem('lblSummaryChannel');
        hideItem('lblSummaryChannelLabel');
    }
    else {
        hideItem('dvCustomer');
        $("#ddlCustomer option[value='-1']").prop("selected", true);
        $("#ddlCustomer").val("-1");
        hideItem('lblSummaryCustomer');
        hideItem('lblSummaryCustomerLabel');
        hideItem('dvCustomerSpecific');
        $("#txtCustomerSpecificLotCode").val("");
        hideItem('lblSummaryCustomerSpecificLotCode');
        hideItem('lblSummaryCustomerSpecificLotCodeLabel');
        hideItem('dvChannel');
        $("#ddlChannel option[value='-1']").prop("selected", true);
        $("#ddlChannel").val("-1");
        hideItem('lblSummaryChannel');
        hideItem('lblSummaryChannelLabel');
    }

    $("#labelSAPBaseUOM").html($("#ddlSAPBUOM").val());

    if ($("#ddlMaterialGroup5 option:selected").text().toLowerCase() == 'shipper (shp)' || $("#ddlMaterialGroup5 option:selected").text().toLowerCase() == 'shippers (shp)') {
        showItem('dvShipper');
        showItem('dvFGTotalQuantityUnitsInDisplay');
        showItem('dvSummaryShipper');
        updateFGtotals();
    }
    else {
        hideItem('dvShipper');
        hideItem('dvFGTotalQuantityUnitsInDisplay');
        hideItem('dvSummaryShipper');
    }

    if ($("#ddlMaterialGroup4 option:selected").text() == 'MIXES (MIX)') {
        showItem('dvMixes');
        showItem('dvSummaryMixes');
    }
    else {
        hideItem('dvMixes');
        hideItem('dvSummaryMixes');
    }

    if ($("#ddlNeedNewUPCUCC option:selected").text() == 'Yes') {
        showItem('dvddlNeedNewUPC');
        showItem('lblSummaryNeedNewUnitUPC');
        showItem('lblSummaryNeedNewUnitUPCLabel');

        showItem('dvNeedNewDisplayBoxUPC');
        showItem('lblSummaryNeedNewDisplayBoxUPC');
        showItem('lblSummaryNeedNewDisplayBoxUPCLabel');

        showItem('dvSAPBaseUOM');
        showItem('lblSummarySAPBaseUOM');
        showItem('lblSummarySAPBaseUOMLabel');

        if ($("#dvddlNeedNewUPC option:selected").text() == 'No') {
            showItem('dvUnitUPC');
            showItem('lblSummaryUnitUPC');
            showItem('lblSummaryUnitUPCLabel');
        }
        else {
            hideItem('dvUnitUPC');
            //$("#txtUnitUPC").val("");
            hideItem('lblSummaryUnitUPC');
            hideItem('lblSummaryUnitUPCLabel');
        }

        if ($("#ddlNeedNewDisplayBoxUPC option:selected").text() == 'No') {
            showItem('dvDisplayBoxUPC');
            showItem('lblSummaryDisplayUPCBox');
            showItem('lblSummaryDisplayUPCBoxLabel');
        }
        else {
            hideItem('dvDisplayBoxUPC');
            //$("#txtDisplayUPCBox").val("");
            hideItem('lblSummaryDisplayUPCBox');
            hideItem('lblSummaryDisplayUPCBoxLabel');
        }

        if ($("#ddlSAPBUOM option:selected").text() == "PAL") {
            showItem('dvNeedNewPalletUCC');
            showItem('lblSummaryNeedNewPalletUCC');
            showItem('lblSummaryNeedNewPalletUCCLabel');

            hideItem('dvNeedNewCaseUCC');
            $("#ddlNeedNewCaseUCC option[value='-1']").prop("selected", true);
            $("#ddlNeedNewCaseUCC").val("-1");
            hideItem('lblSummaryNeedNewCaseUCC');
            hideItem('lblSummaryNeedNewCaseUCCLabel');

            hideItem('dvCaseUCC');
            //$("#txtCaseUCC").val("");
            hideItem('lblSummaryCaseUCC');
            hideItem('lblSummaryCaseUCCLabel');

            if ($("#ddlNeedNewPalletUCC option:selected").text() == "Yes") {

                hideItem('dvPalletUCC');
                //$("#txtPalletUCC").val("");
                hideItem('lblSummaryPalletUCC');
                hideItem('lblSummaryPalletUCCLabel');
            }
            else if ($("#ddlNeedNewPalletUCC option:selected").text() == "No") {

                showItem('dvPalletUCC');
                showItem('lblSummaryPalletUCC');
                showItem('lblSummaryPalletUCCLabel');
            }
            else {

                hideItem('dvPalletUCC');
                //$("#txtPalletUCC").val("");
                hideItem('lblSummaryPalletUCC');
                hideItem('lblSummaryPalletUCCLabel');
            }
        }
        else if ($("#ddlSAPBUOM option:selected").text() == "CS") {
            showItem('dvNeedNewCaseUCC');
            showItem('lblSummaryNeedNewCaseUCC');
            showItem('lblSummaryNeedNewCaseUCCLabel');

            hideItem('dvNeedNewPalletUCC');
            $("#ddlNeedNewPalletUCC option[value='-1']").prop("selected", true);
            $("#ddlNeedNewPalletUCC").val("-1");
            hideItem('lblSummaryNeedNewPalletUCC');
            hideItem('lblSummaryNeedNewPalletUCCLabel');

            hideItem('dvPalletUCC');
            //$("#txtPalletUCC").val("");
            hideItem('lblSummaryPalletUCC');
            hideItem('lblSummaryPalletUCCLabel');

            if ($("#ddlNeedNewCaseUCC option:selected").text() == "Yes") {
                hideItem('dvCaseUCC');
                //$("#txtCaseUCC").val("");
                hideItem('lblSummaryCaseUCC');
                hideItem('lblSummaryCaseUCCLabel');
            }
            else if ($("#ddlNeedNewCaseUCC option:selected").text() == "No") {
                showItem('dvCaseUCC');
                showItem('lblSummaryCaseUCC');
                showItem('lblSummaryCaseUCCLabel');
            }
            else {

                hideItem('dvCaseUCC');
                //$("#txtCaseUCC").val("");
                hideItem('lblSummaryCaseUCC');
                hideItem('lblSummaryCaseUCCLabel');
            }
        }
        else {
            hideItem('dvPalletUCC');
            //$("#txtPalletUCC").val("");
            hideItem('lblSummaryPalletUCC');
            hideItem('lblSummaryPalletUCCLabel');

            hideItem('dvNeedNewPalletUCC');
            $("#ddlNeedNewPalletUCC option[value='-1']").prop("selected", true);
            $("#ddlNeedNewPalletUCC").val("-1");
            hideItem('lblSummaryNeedNewPalletUCC');
            hideItem('lblSummaryNeedNewPalletUCCLabel');

            hideItem('dvNeedNewCaseUCC');
            $("#ddlNeedNewCaseUCC option[value='-1']").prop("selected", true);
            $("#ddlNeedNewCaseUCC").val("-1");
            hideItem('lblSummaryNeedNewCaseUCC');
            hideItem('lblSummaryNeedNewCaseUCCLabel');

            hideItem('dvCaseUCC');
            //$("#txtCaseUCC").val("");
            hideItem('lblSummaryCaseUCC');
            hideItem('lblSummaryCaseUCCLabel');
        }
    }
    else if ($("#ddlNeedNewUPCUCC option:selected").text() == 'No') {
        showItem('dvUnitUPC');
        showItem('lblSummaryUnitUPC');
        showItem('lblSummaryUnitUPCLabel');

        showItem('dvDisplayBoxUPC');
        showItem('lblSummaryDisplayUPCBox');
        showItem('lblSummaryDisplayUPCBoxLabel');

        showItem('dvSAPBaseUOM');
        showItem('lblSummarySAPBaseUOM');
        showItem('lblSummarySAPBaseUOMLabel');

        hideItem('dvddlNeedNewUPC');
        $("#ddlNeedNewUnitUPC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewUnitUPC").val("-1");
        hideItem('lblSummaryNeedNewUnitUPC');
        hideItem('lblSummaryNeedNewUnitUPCLabel');

        hideItem('dvNeedNewDisplayBoxUPC');
        $("#ddlNeedNewDisplayBoxUPC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewDisplayBoxUPC").val("-1");
        hideItem('lblSummaryNeedNewDisplayBoxUPC');
        hideItem('lblSummaryNeedNewDisplayBoxUPCLabel');

        hideItem('dvNeedNewCaseUCC');
        $("#ddlNeedNewCaseUCC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewCaseUCC").val("-1");
        hideItem('lblSummaryNeedNewCaseUCC');
        hideItem('lblSummaryNeedNewCaseUCCLabel');

        hideItem('dvNeedNewPalletUCC');
        $("#ddlNeedNewPalletUCC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewPalletUCC").val("-1");
        hideItem('lblSummaryNeedNewPalletUCC');
        hideItem('lblSummaryNeedNewPalletUCCLabel');

        if ($("#ddlSAPBUOM option:selected").text() == "PAL") {
            showItem('dvPalletUCC');
            showItem('lblSummaryPalletUCC');
            showItem('lblSummaryPalletUCCLabel');

            hideItem('dvCaseUCC');
            //$("#txtCaseUCC").val("");
            hideItem('lblSummaryCaseUCC');
            hideItem('lblSummaryCaseUCCLabel');
        }
        else if ($("#ddlSAPBUOM option:selected").text() == "CS") {
            showItem('dvCaseUCC');
            showItem('lblSummaryCaseUCC');
            showItem('lblSummaryCaseUCCLabel');

            hideItem('dvPalletUCC');
            //$("#txtPalletUCC").val("");
            hideItem('lblSummaryPalletUCC');
            hideItem('lblSummaryPalletUCCLabel');
        }
        else {
            hideItem('dvPalletUCC');
            //$("#txtPalletUCC").val("");
            hideItem('lblSummaryPalletUCC');
            hideItem('lblSummaryPalletUCCLabel');

            hideItem('dvCaseUCC');
            //$("#txtCaseUCC").val("");
            hideItem('lblSummaryCaseUCC');
            hideItem('lblSummaryCaseUCCLabel');
        }
    }
    else {
        hideItem('dvddlNeedNewUPC');
        $("#ddlNeedNewUnitUPC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewUnitUPC").val("-1");
        hideItem('lblSummaryNeedNewUnitUPC');
        hideItem('lblSummaryNeedNewUnitUPCLabel');

        hideItem('dvUnitUPC');
        //$("#txtUnitUPC").val("");
        hideItem('lblSummaryUnitUPC');
        hideItem('lblSummaryUnitUPCLabel');

        hideItem('dvNeedNewDisplayBoxUPC');
        $("#ddlNeedNewDisplayBoxUPC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewDisplayBoxUPC").val("-1");
        hideItem('lblSummaryNeedNewDisplayBoxUPC');
        hideItem('lblSummaryNeedNewDisplayBoxUPCLabel');

        hideItem('dvDisplayBoxUPC');
        //$("#txtDisplayUPCBox").val("");
        hideItem('lblSummaryDisplayUPCBox');
        hideItem('lblSummaryDisplayUPCBoxLabel');

        hideItem('dvSAPBaseUOM');
        $("#ddlSAPBUOM option[value='-1']").prop("selected", true);
        $("#ddlSAPBUOM").val("-1");
        hideItem('lblSummarySAPBaseUOM');
        hideItem('lblSummarySAPBaseUOMLabel');

        hideItem('dvNeedNewCaseUCC');
        $("#ddlNeedNewCaseUCC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewCaseUCC").val("-1");
        hideItem('lblSummaryNeedNewCaseUCC');
        hideItem('lblSummaryNeedNewCaseUCCLabel');

        hideItem('dvCaseUCC');
        //$("#txtCaseUCC").val("");
        hideItem('lblSummaryCaseUCC');
        hideItem('lblSummaryCaseUCCLabel');

        hideItem('dvNeedNewPalletUCC');
        $("#ddlNeedNewPalletUCC option[value='-1']").prop("selected", true);
        $("#ddlNeedNewPalletUCC").val("-1");
        hideItem('lblSummaryNeedNewPalletUCC');
        hideItem('lblSummaryNeedNewPalletUCCLabel');

        hideItem('dvPalletUCC');
        //$("#txtPalletUCC").val("");
        hideItem('lblSummaryPalletUCC');
        hideItem('lblSummaryPalletUCCLabel');

    }

    if ($("#ddlTBDIndicator option:selected").text() == 'Yes') {
        $("#reqLikeFGItemNumber").removeClass('hideItem').addClass('showRequired');
        $(".LookupLikeSAPItemNumber").removeClass('hideItem').addClass('showRequired');
        $("#txtLikeFGItemNumber").addClass("required");
        $("#reqLikeItemDescription").removeClass('hideItem').addClass('showRequired');
        $("#txtLikeItemDescription").addClass("required");
        $("#reqSAPItemNumber").removeClass('showRequired').addClass('hideItem');
        $(".LookupSAPItemNumberIcon").removeClass('showRequired').addClass('hideItem');
        $("#txtSAPItemNumber").removeClass("required");
        $("#txtSAPItemDescription").removeClass("required");
        $("#reqSAPItemDescription").addClass('hideItem').removeClass('showRequired');

        if ($("#txtSAPItemNumber").val() == "") {
            $("#txtSAPItemNumber").val("NEEDS NEW");
        }


        $("#txtLast12MonthSales").val('');
        hideItem('dvlast12');
        $("#dvlast12").find(".input-group").addClass("hideItem");
        hideItem('lblSummaryLast12MonthSales');
        hideItem('lblSummaryLast12MonthSalesLabel')

    } else {
        $("#reqLikeFGItemNumber").removeClass('showRequired').addClass('hideItem');
        $(".LookupLikeSAPItemNumber").removeClass('showRequired').addClass('hideItem');
        $("#txtLikeFGItemNumber").removeClass("required");
        $("#reqLikeItemDescription").addClass('hideItem').removeClass('showRequired');
        $("#txtLikeItemDescription").removeClass("required");
        $("#reqSAPItemNumber").removeClass('hideItem').addClass('showRequired');
        $(".LookupSAPItemNumberIcon").removeClass('hideItem').addClass('showRequired');
        $("#txtSAPItemNumber").addClass("required");
        $("#txtSAPItemDescription").addClass("required");
        $("#reqSAPItemDescription").removeClass('hideItem').addClass('showRequired');

        if ($("#txtSAPItemNumber").val().toLocaleLowerCase() == "needs new") {
            $("#txtSAPItemNumber").val("");
        }

        showItem('dvlast12');
        $("#dvlast12").find(".input-group").removeClass("hideItem");
        showItem('lblSummaryLast12MonthSales');
        showItem('lblSummaryLast12MonthSalesLabel');

    }

    if ($("#ddlOutsideUSA option:selected").text() == 'Yes') {
        showItem('dvCountryofSale');
        showItem('lblSummaryCountryOfSale');
        showItem('lblSummaryCountryOfSaleLabel');
    }
    else {
        hideItem('dvCountryofSale');
        $("#ddlCountryOfSale option[value='-1']").prop("selected", true);
        $("#ddlCountryOfSale").val("-1");
        hideItem('lblSummaryCountryOfSale');
        hideItem('lblSummaryCountryOfSaleLabel');
    }

    //$(".drpNewClass").each(function () {
    //    BOMNewCondition(this);
    //});
    //ProjectTypeChange();
    RemoveRequired();
    if ($("#hdnIsChangeRequest").val() == "CHANGE") {
        $("#txtChangeReason").addClass("required");
        $("#rowChangeReason, #rowChangeReason .form-group, #reqChangeReason").removeClass('hideItem');
    } else {
        $("#txtChangeReason").removeClass("required");
        $("#rowChangeReason, #rowChangeReason .form-group, #reqChangeReason").addClass('hideItem');
    }
    checkRequiredFields();
    drpCompType_load();
    ShowHideApprovedGraphicsAsset();
}

function checkRequiredFields() {
    if (isShipperGridRequired()) {
        $("#txtFGTotalQuantityUnitsInDisplay").addClass('required');
        $("#txtFGTotalTotalOuncesPerShipper").addClass('required');
    }
    else {
        $("#txtFGTotalQuantityUnitsInDisplay").removeClass('required');
        $("#txtFGTotalTotalOuncesPerShipper").removeClass('required');
    }

    var projectType = $("#ddlProjectType option:selected").text();
    if (projectType == "Simple Network Move") {
        $("#reqLikeFGItemNumber").removeClass('showRequired').addClass('hideItem');
        $("#txtLikeFGItemNumber").removeClass("required");
        $("#reqLikeItemDescription").addClass('hideItem').removeClass('showRequired');
        $("#txtLikeItemDescription").removeClass("required");
    }
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

    if ($("#ddlProjectType option:selected").text() == "Graphics Change Only") {
        $('.ApprovedGraphicsAsset').each(function (i, obj) {
            var sd = 'Approved Graphics Asset is required';
            var id = $(this).closest('.bomrow').find('.ddlMoveTS').attr('id');
            var drpNewId = $(this).closest('.bomrow').find('.drpNewClass').attr('id');
            var drpGraphicsId = $(this).closest('.bomrow').find('.drpGraphics').attr('id');
            var drpComponentTypeId = $(this).closest('.bomrow').find('.drpComponentType').attr('id');
            var NewExisting = $("#" + drpNewId + " option:selected").text();
            var GraphicsRequired = $("#" + drpGraphicsId + " option:selected").text();
            var CompType = $("#" + drpComponentTypeId + " option:selected").text();

            if (NewExisting == "New" && GraphicsRequired == "Yes" && CompType.indexOf('Transfer') == -1 && CompType.indexOf('Candy') == -1) {
                if ($(this).find('.ancApprovedGraphicsAsset').attr('href') === undefined) {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=activateTab(' + '"' + id + '"' + ');GotoStepAndFocusNoRed(' + '"' + id + '"' + '); >Update</a>   </li></br>');
                }
            }
        });
    }

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

function isShipperGridRequired() {
    var ddlMaterialGroup5sel = $("#ddlMaterialGroup5 option:selected");
    return ddlMaterialGroup5sel.text().toLowerCase() == "shipper (shp)" || ddlMaterialGroup5sel.text().toLowerCase() == "shippers (shp)";
}
function checkForNetworkMove() {
    var projectType = $("#ddlProjectType option:selected").text();
    if (projectType.toLocaleLowerCase() == "simple network move") {
        var tabsToDisable = [3, 4, 5, 6, 8, 9, 10];
        $.each(tabsToDisable, function (i, e) {
            $('section').eq(e).find("select").val("-1");
            $('section').eq(e).find(":input:not([type=hidden]):not([type=submit]):not([type=button])").each(function () {
                if (!$(this).hasClass("NMNumber") && !$(this).hasClass("datePicker")) {
                    $(this).val("N/A for Simple Network Move");
                } else if ($(this).hasClass("datePicker")) {
                    $(this).val("");
                } else {
                    $(this).val("0");
                }
            });

        });
    }
}
function preSaveUpdates() {
    $("#lblProjectType").html($("#ddlProjectType option:selected").text());
    if ($("#ddlProjectTypeSubCategory option:selected").text() != "NA") {
        $("#divProjectTypeSubCategory").show();
        $("#lblProjectTypeSubCategory").html($("#ddlProjectTypeSubCategory option:selected").text());
    } else {
        $("#divProjectTypeSubCategory").hide();
    }

    var unitUPCRequired = $("#ddlNeedNewUnitUPC option:selected").text();
    var DBUPCRequired = $("#ddlNeedNewDisplayBoxUPC option:selected").text();
    var PalletUPCRequired = $("#ddlNeedNewPalletUCC option:selected").text();
    var CaseUPCRequired = $("#ddlNeedNewCaseUCC option:selected").text();
    if (unitUPCRequired == "Yes" && $("#txtUnitUPC").val() == "") {
        $("#txtUnitUPC").val("NEEDS NEW");
    }
    if (DBUPCRequired == "Yes" && $("#txtDisplayUPCBox").val() == "") {
        $("#txtDisplayUPCBox").val("NEEDS NEW");
    }
    if (PalletUPCRequired == "Yes" && $("#txtPalletUCC").val() == "") {
        $("#txtPalletUCC").val("NEEDS NEW");
    }
    if (CaseUPCRequired == "Yes" && $("#txtCaseUCC").val() == "") {
        $("#txtCaseUCC").val("NEEDS NEW");
    }

    $('#hdnUCLoaded').val("");
    return true;
}
function ValidateIPFData() {
    $('#error_message').empty();
    var isValid = true;
    checkRequiredFields();
    checkForNetworkMove();
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
    //SAP Nomenclature
    var isSAPDescriptionValid = true;
    var CheckForSAPNomenclature = false;
    if ($('#hdnNewIPF').val() == "Yes" && $("#ddlTBDIndicator option:selected").text() == "Yes") {
        CheckForSAPNomenclature = true;
        if ($("#ddlProductHierarchyLevel1 option:selected").text() == "Co-Manufacturing (000000027)") {
            if ($("#ddlManuallyCreateSAPDescription option:selected").text() != "No") {
                CheckForSAPNomenclature = false;
            }
        }
    }

    $('.MemberDiv').each(function (i, obj) {
        var hdnRequired = $(this).find("[id^=hdnRequired]").val();
        if (hdnRequired != 'False') {
            var ddlMember = $(this).find(".ddlMember").val();
            //var txtMemberCount = $(this).find('.txtMember').length;
            if ((ddlMember == "-1" || ddlMember == "")) {
                var id = $(this).find(".ddlMember").attr('id');
                isValid = false;
                var label = $(this).find('label');
                var fieldName = label.length ? label.text().replace(":", "") : $(this).attr('title');
                var requiredMsg = fieldName + ' is required';
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >' + requiredMsg +
                    ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
            }
        }
    });

    //Marketing cannot be just NA
    if ($('#ddlMarketingMembers').val() == "NA") {
        var AllNa = true;
        $('.txtMarketingMembers').each(function (i, obj) {
            if ($(this).val() != "NA") {
                AllNa = false;
            }
        });
        if (AllNa == true) {
            var id = 'ddlMarketingMembers';
            isValid = false;
            var requiredMsg = "NA is not a valid entry for Marketing. Please add a valid member";
            $("#dverror_message").show();
            $('#error_message').append('<li class="errorMessage" >' + requiredMsg +
                ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + id + '"' + ') >Update</a>   </li></br>');
        }
    }

    if (CheckForSAPNomenclature) {
        SAPDescriptionLenght = $("#txtSAPItemDescription").val().length;
        if (SAPDescriptionLenght > 40) {
            var checkDescriptionLength = true;
            var ErrMsg = ""
            //Parent - 2
            if ($("#ddlInvolvesCarton option:selected").text() == 'Yes') {
                var Parent = $("#txtUnitsInsideCarton").val();
                if (Parent.length > 2) {
                    checkDescriptionLength = false;
                    isSAPDescriptionValid = false;
                    ErrMsg = "Maximum allowed Units Inside Carton is 999.";
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + ErrMsg +
                        ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + 'txtUnitsInsideCarton' + '"' + ') >Update</a>   </li></br>');
                }
            }
            //Count - 4
            var NumberofTraysPerBaseUOM = $("#txtNumberofTraysPerBaseUOM").val();
            if (NumberofTraysPerBaseUOM != '') {
                if (NumberofTraysPerBaseUOM.length > 4) {
                    checkDescriptionLength = false;
                    isSAPDescriptionValid = false;
                    ErrMsg = "Maximum allowed Number of Trays Per Base UOM is 9999.";
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + ErrMsg +
                        ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + 'txtNumberofTraysPerBaseUOM' + '"' + ') >Update</a>   </li></br>');
                }
            }
            else {
                var RetailSellingUnitsPerBaseUOM = $("#txtRetailSellingUnitsPerBaseUOM").val();
                if (RetailSellingUnitsPerBaseUOM.length > 4) {
                    checkDescriptionLength = false;
                    isSAPDescriptionValid = false;
                    ErrMsg = "Maximum allowed Retail Selling Units Per Base UOM is 9999.";
                    $("#dverror_message").show();
                    $('#error_message').append('<li class="errorMessage" >' + ErrMsg +
                        ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + 'txtRetailSellingUnitsPerBaseUOM' + '"' + ') >Update</a>   </li></br>');
                }
            }
            //Oz Weight -5
            var OzWeight = '';
            var Error = '';
            var OZWeightAccounted = false;
            if ($("#ddlInvolvesCarton option:selected").text() == 'Yes') {
                var dOzWeight = ($.isNumeric($("#txtIndividualPouchWeight").val().replace(',', '')) ? parseFloat($("#txtIndividualPouchWeight").val().replace(',', '')) : 0);
                OzWeight = dOzWeight.toString();
                if (OzWeight != "0") {
                    OZWeightAccounted = true;
                }

                if (OzWeight.length > 5) {
                    Error = '<li class="errorMessage" >' + "Maximum allowed Individual Pouch weight is 99.99." +
                        ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + 'txtIndividualPouchWeight' + '"' + ') >Update</a>   </li></br>'
                }
            }
            if (!OZWeightAccounted) {
                var dOzWeight = ($.isNumeric($("#txtRetailUnitWeight").val().replace(',', '')) ? parseFloat($("#txtRetailUnitWeight").val().replace(',', '')) : 0);
                OzWeight = dOzWeight.toString();
                if (OzWeight.length > 5) {
                    Error = '<li class="errorMessage" >' + "Maximum allowed Retail Unit weight is 99.99." +
                        ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + 'txtRetailUnitWeight' + '"' + ') >Update</a>   </li></br>'
                }
            }

            if (OzWeight.length > 5) {
                checkDescriptionLength = false;
                isSAPDescriptionValid = false;
                $("#dverror_message").show();
                $('#error_message').append(Error);
            }
            //Product Form Description
            if (checkDescriptionLength) {
                var maxProductFormDescription = SAPNomenclature();
                isSAPDescriptionValid = false;
                ErrMsg = "Product form description exceeds maximum allowed characters: " + maxProductFormDescription + ".";
                $("#dverror_message").show();
                $('#error_message').append('<li class="errorMessage" >' + ErrMsg +
                    ' <a style="color:darkblue" onclick=GotoStepAndFocus(' + '"' + 'txtProductFormDescription' + '"' + ') >Update</a>   </li></br>');
            }
        }
    }

    if (isValidBom && isValidMixes && isValidShipper && isValid && isSAPDescriptionValid)
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

function CalculateAnnualProjectedUnits() {
    var annualProjectDollars = $("#txtAnnualProjectedDollars").val();
    var month1ProjectDollars = $("#txtMonth1ProjectedDollars").val();
    var month2ProjectDollars = $("#txtMonth2ProjectedDollars").val();
    var month3ProjectDollars = $("#txtMonth3ProjectedDollars").val();
    var truckloadSellingPrice = $("#txtTruckLoadSellingPrice").val();

    annualProjectDollars = annualProjectDollars.replace(/,/g, '');
    month1ProjectDollars = month1ProjectDollars.replace(/,/g, '');
    month2ProjectDollars = month2ProjectDollars.replace(/,/g, '');
    month3ProjectDollars = month3ProjectDollars.replace(/,/g, '');
    truckloadSellingPrice = truckloadSellingPrice.replace(/,/g, '');

    if (isNaN(annualProjectDollars) || annualProjectDollars == 0) {
        annualProjectDollars = 0;
    }

    if (isNaN(month1ProjectDollars) || month1ProjectDollars == 0) {
        month1ProjectDollars = 0;
    }

    if (isNaN(month2ProjectDollars) || month2ProjectDollars == 0) {
        month2ProjectDollars = 0;
    }

    if (isNaN(month3ProjectDollars) || month3ProjectDollars == 0) {
        month3ProjectDollars = 0;
    }

    if (isNaN(truckloadSellingPrice) || truckloadSellingPrice == 0) {
        truckloadSellingPrice = 0;
    }

    if ((annualProjectDollars != 0) && (truckloadSellingPrice != 0)) {
        var num = annualProjectDollars / truckloadSellingPrice;
        $("#txtAnnualProjectedUnits").val(maskNumber(num.toFixed(0)));
    }
    else {
        $("#txtAnnualProjectedUnits").val(0);
    }

    if ((month1ProjectDollars != 0) && (truckloadSellingPrice != 0)) {
        var num = month1ProjectDollars / truckloadSellingPrice;
        $("#txtMonth1ProjectedUnits").val(maskNumber(num.toFixed(0)));
    }
    else {
        $("#txtMonth1ProjectedUnits").val(0);
    }

    if ((month2ProjectDollars != 0) && (truckloadSellingPrice != 0)) {
        var num = month2ProjectDollars / truckloadSellingPrice;
        $("#txtMonth2ProjectedUnits").val(maskNumber(num.toFixed(0)));
    }
    else {
        $("#txtMonth2ProjectedUnits").val(0);
    }

    if ((month3ProjectDollars != 0) && (truckloadSellingPrice != 0)) {
        var num = month3ProjectDollars / truckloadSellingPrice;
        $("#txtMonth3ProjectedUnits").val(maskNumber(num.toFixed(0)));
    }
    else {
        $("#txtMonth3ProjectedUnits").val(0);
    }
}

function CalculateBaseUnitOfMeasureNetWeightLbs() {
    if (AutoPopulateBaseUofMNetWeight()) {
        var retailSellingUnitsPerBaseUOM = $("#txtRetailSellingUnitsPerBaseUOM").val();
        var retailUnitWeight = $("#txtRetailUnitWeight").val();

        retailSellingUnitsPerBaseUOM = retailSellingUnitsPerBaseUOM.replace(",", "");
        retailUnitWeight = retailUnitWeight.replace(",", "");

        if (isNaN(retailSellingUnitsPerBaseUOM) || retailSellingUnitsPerBaseUOM.length == 0) {
            retailSellingUnitsPerBaseUOM = 0;
        }

        if (isNaN(retailUnitWeight)) {
            retailUnitWeight = 0;
        }

        if ((retailSellingUnitsPerBaseUOM != 0) && (retailUnitWeight != 0)) {
            var num = retailSellingUnitsPerBaseUOM * (retailUnitWeight / 16);
            $("#txtBaseUofMNetWeight").val(maskNumber(num.toFixed(2)));
        }
        else {
            $("#txtBaseUofMNetWeight").val(0);
        }
    } else {
        $("#txtBaseUofMNetWeight").val(0);
    }
}
function AutoPopulateBaseUofMNetWeight() {
    //"Auto Generated Locked if ""Retail Unit Weight"" field is populated. If ""Retail Unit Weight"" field not populated -> User Generated"

    var AutoPopulateBaseUofMNetWeight = true;

    var retailUnitWeight = $("#txtRetailUnitWeight").val();

    if (!retailUnitWeight) {
        AutoPopulateBaseUofMNetWeight = false;
    }

    retailUnitWeight = retailUnitWeight.replace(",", "");

    if (AutoPopulateBaseUofMNetWeight && isNaN(retailUnitWeight)) {
        AutoPopulateBaseUofMNetWeight = false;
    }

    if (AutoPopulateBaseUofMNetWeight && retailUnitWeight == 0) {
        AutoPopulateBaseUofMNetWeight = false;
    }

    if (AutoPopulateBaseUofMNetWeight) {
        $("#txtBaseUofMNetWeight").prop('readonly', true);
    } else {
        $("#txtBaseUofMNetWeight").prop('readonly', false);
    }

    return AutoPopulateBaseUofMNetWeight;
}
//textbox masking

/// Packaging Item Attachments
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

function updateOPSU(evt, element) {
    var currentRow = $(element).parents("tr");
    var numberOfPieces = currentRow.find("#txtMixNumberOfPieces").val();
    var oucesPerPiece = currentRow.find("#txtOzPerPiece").val();
    var retailSellingUnitsPerBaseUOM = $("#txtRetailSellingUnitsPerBaseUOM").val();
    retailSellingUnitsPerBaseUOM = retailSellingUnitsPerBaseUOM.replace(",", "")
    numberOfPieces = isNaN(numberOfPieces) || numberOfPieces == '' ? 0.0 : parseFloat(numberOfPieces);
    oucesPerPiece = isNaN(oucesPerPiece) || oucesPerPiece == '' ? 0 : parseFloat(oucesPerPiece);
    retailSellingUnitsPerBaseUOM = isNaN(retailSellingUnitsPerBaseUOM) || retailSellingUnitsPerBaseUOM == '' ? 0.0 : parseFloat(retailSellingUnitsPerBaseUOM);
    var oucesPerSellingUnit, lbsFGBOM, GramsPerSellingUnit;
    oucesPerSellingUnit = numberOfPieces * oucesPerPiece;
    GramsPerSellingUnit = oucesPerSellingUnit * 28.3495;
    currentRow.find("#txtOzPerSellingUnit").val(oucesPerSellingUnit);
    currentRow.find("#txtGramsPerSellingUnit").val(GramsPerSellingUnit);
    lbsFGBOM = retailSellingUnitsPerBaseUOM * numberOfPieces;
    currentRow.find("#txtQtyMix").val(lbsFGBOM);
    currentRow.find("#txtLbsFGBOM").val(((lbsFGBOM * oucesPerPiece) / 16.0).toFixed(3));
}

function updateOuncesPerFGunit(evt, element) {
    var currentRow = $(element).parents("tr");
    var numberOfPieces = currentRow.find("#txtFGnumberUnits").val();
    var oucesPerPiece = currentRow.find("#txtFGouncesPerUnit").val();
    var oucesPerSellingUnit = 0;
    if (numberOfPieces > 0 && oucesPerPiece > 0) {
        oucesPerSellingUnit = numberOfPieces * oucesPerPiece;
        txtFGouncesPerFGunit = currentRow.find("#txtFGouncesPerFGunit");
        txtFGouncesPerFGunit.val(oucesPerSellingUnit.toFixed(2));
    }
    updateFGtotals();
}

function updateFGtotals() {
    var txtFGTotalQuantityUnitsInDisplay = $("#txtFGTotalQuantityUnitsInDisplay");
    var txtFGTotalTotalOuncesPerShipper = $("#txtFGTotalTotalOuncesPerShipper");
    var w, numberOfPieces, ouncesPerFGunit;
    numberOfPieces = 0;
    ouncesPerFGunit = 0;
    $("#tabShipper").find("tr").each(function (w, row) {
        txtFGnumberUnits = $(row).find("#txtFGnumberUnits");
        txtFGouncesPerFGunit = $(row).find("#txtFGouncesPerFGunit");
        if (txtFGnumberUnits.val() !== undefined && txtFGouncesPerFGunit.val() !== undefined) {
            if (txtFGnumberUnits.val() != '' && !isNaN(txtFGnumberUnits.val()))
                numberOfPieces += parseInt(txtFGnumberUnits.val());
            if (txtFGouncesPerFGunit.val() != '' && !isNaN(txtFGouncesPerFGunit.val()))
                ouncesPerFGunit += parseFloat(txtFGouncesPerFGunit.val());
        }
    });
    txtFGTotalQuantityUnitsInDisplay.val(numberOfPieces);
    txtFGTotalTotalOuncesPerShipper.val(ouncesPerFGunit.toFixed(2));
}

function updateSAPDesc(clicked, txtBoxToChange) {
    if ($('#hdnNewIPF').val() != "Yes") {
        var clicked = $(clicked);
        if (clicked.val().trim() != "") {
            $("#" + txtBoxToChange).val(clicked.val());
        }
    }
    SAPNomenclature();
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
    $(".tsButtonLink").each(function () {
        if (!$(this).parents("#FGSection").length) {
            var leftMath = (index * 225) + 25;
            var topMath = 0;
            if (topindex == 1) {
                topMath = 25;
            } else {
                topMath = (topindex * 40);
            }
            $(this).attr("style", "left:" + leftMath + "px;top:" + topMath + "px;");
            index++;
            if (index > 3) {
                index = 0;
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
    ShowHideApprovedGraphicsAsset();
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
        $("#drpMadeInUSAPct").val("-1");
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
function ParentProjectTypeChange() {
    $("#ProjectTypeChangeNote").removeClass("hideItem");
}

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
            if (controlId != null && (controlId.id == "ddlTBDIndicator" || controlId.id == "ddlProjectType")) {
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