<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InitialApprovalReviewForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.InitialApprovalReviewForm.InitialApprovalReviewForm" %>

<div class="container">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled!
            <br />
            If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False" OnClick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>PM Initial Review</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>IPF Summary</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Type:</label>
                <asp:TextBox ID="lblProjectType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">1st Ship Date:</label>
                <asp:TextBox ID="lblRevisedFirstShipDate" runat="server" ReadOnly="True" ClientIDMode="Static" CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Weeks Until First Ship:</label>
                <asp:TextBox ID="lblWeeksToShip" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                <asp:TextBox ID="lblProductHierarchy1" runat="server" ReadOnly="True" ClientIDMode="Static" CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 2:</label>
                <asp:TextBox ID="lblProductHierarchy2" runat="server" ReadOnly="True" ClientIDMode="Static" CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 1 (Brand):</label>
                <asp:TextBox ID="lblBrand" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">12 Month Projected $:</label>
                <asp:TextBox ID="lblAnnualDollar" runat="server" ReadOnly="True" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Annual Projected Retail Selling Units:</label>
                <asp:TextBox ID="lblAnnualUnits" runat="server" ClientIDMode="Static" ReadOnly="True" CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Expected Gross Margin %:</label>
                <asp:TextBox ID="lblExpectedGrossMargin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="dvCustomer" class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Customer:</label>
                <asp:TextBox ID="lblCustomer" runat="server" ReadOnly="True" ClientIDMode="Static" CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="dvChannel">
            <div class="form-group">
                <label class="control-label">Channel:</label>
                <asp:TextBox ID="txtChannel" runat="server" BorderStyle="None" ClientIDMode="Static" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Project Notes:</label>
                <asp:TextBox ID="lblProjectNotes" runat="server" BorderStyle="None" TextMode="MultiLine" Rows="6" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Approval Review</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <asp:UpdatePanel ID="TeamPanel" runat="server">
        <ContentTemplate>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-6" runat="server" id="divProjectManagers">
                        <div class="form-group">
                            <label class="control-label ProjectManager"><span class="markrequired">*</span>Project Manager (PM):</label>
                            <asp:Image ID="ImgAddProjectManager" class="AddMember" onClick="AddProjectTeamMembersRow('btnAddProjectManager')" Style="cursor: pointer; margin-top: 27px; height: 16px; float: right;" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add Project Manager" />
                            <table class="MembersTable">
                                <asp:Repeater ID="rptProjectManagers" runat="server" OnItemDataBound="rptProjectManagers_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td runat="server" id="tdDeliverable">
                                                <div class="form-group">
                                                    <SharePoint:PeopleEditor Width="96%" ID="peProjectManagerMembers" runat="server" AllowEmpty="false" MultiSelect="false" SelectionSet="User" SharePointGroup="Project Manager Members" />
                                                </div>
                                            </td>
                                            <td class="deleteRow">
                                                <div class="form-group">
                                                    <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -16px;" onClick="deleteRow(this,'hdnDeletedStatusForProjectManager');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                    <asp:HiddenField ID="hdnDeletedStatusForProjectManager" Value="false" runat="server" ClientIDMode="Static" />
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                            <asp:Button ID="btnAddProjectManager" OnClick="btnAddProjectManager_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Is the Project Approved?:</label>
                <asp:DropDownList ID="ddlInitialApprovalReview" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Approved" Value="A"></asp:ListItem>
                    <asp:ListItem Text="Request IPF Update" Value="I"></asp:ListItem>
                    <asp:ListItem Text="Rejected" Value="R"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row" id="dvRequestedUpdate">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label id="lblRequestedInformation" class="control-label"><span class="markrequired">*</span>Requested Update:</label>
                <asp:TextBox ID="txtRequestedInformation" ClientIDMode="Static" runat="server" Height="77px" TextMode="MultiLine" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="drpInitialTimeTable" EventName="SelectedIndexChanged" />
        </Triggers>
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div id="reloadingCover" clientidmode="Static" runat="server" style="height: 350px; position: absolute; width: 100%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                        <div class="disablingLoadingIcon" id="disablingLoadingHierarchy" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Project Timeline Setup</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-6">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>Initial Time Table:</label>
                        <asp:DropDownList ID="drpInitialTimeTable" runat="server" ClientIDMode="Static" CssClass="required form-control" AutoPostBack="true" AppendDataBoundItems="true" onchange="hideTimeLineDetails();" OnSelectedIndexChanged="drpInitialTimeTable_SelectedIndexChanged">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-6">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>Do you anticipate needing an Expedited Workflow with SGS?:</label>
                        <asp:DropDownList ID="ddlNeedSExpeditedWorkflowWithSGS" runat="server" ClientIDMode="Static" CssClass="required form-control" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Standard" Value="Standard"></asp:ListItem>
                            <asp:ListItem Text="Expedited" Value="Expedited"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row timeline">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Timeline Details</h2>
                </div>
            </div>
            <div class="row RowBottomMargin timeline">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row timeline">
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Project Start Date:</label>
                        <asp:TextBox ID="txtProjectStartDate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Estimated Project Length:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtEstimatedProjectLength" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            <span class="input-group-addon">Business Days</span>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Float Days:</label>
                        <div class="input-group">
                            <asp:TextBox ID="txtFloatDays" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox><span class="input-group-addon">Business Days</span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row timeline">
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">First Ship Date:</label>
                        <asp:TextBox ID="txtFirstShipdate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="ApprovedGraphicsAsset GraphicsOnly">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Approved Graphic Asset for New Components:</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <asp:UpdatePanel ID="ApprovedGraphicsAssetPanel" ClientIDMode="Static" runat="server" ChildrenAsTriggers="true">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnReloadAttachment" EventName="Click" />
            </Triggers>
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                    <ProgressTemplate>
                        <div id="reloadingCoverApprovedGraphicsAsset" clientidmode="Static" runat="server" style="height: 165px; position: absolute; width: 100%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                            <div class="disablingLoadingIcon" id="disablingLoadingApprovedGraphicsAsset" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div>
                    <%--<div class="row attachment">
                    <div class="col-xs-12 col-sm-3 col-md-4">
                        <label class="control-label">Approved Graphics Asset - New Component:</label>
                    </div>
                     <div class="col-xs-12 col-sm-9 col-md-4">
                        <input id="btnApprovedGraphicsAsset" type="button" runat="server" class="ButtonControlAutoSize hidebtn" value="Upload Approved Graphics Asset" onclick="OpenDialog('Upload Approved Graphics Asset', 'ApprovedGraphicsAsset'); return false;" />
                    </div>
                </div>--%>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-6">
                            <table width="100%">
                                <asp:Repeater ID="rptApprovedGraphicsAsset" ClientIDMode="Static" runat="server">
                                    <HeaderTemplate>
                                        <tr>
                                            <th>Action</th>
                                            <th>Document Name</th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="lnkDeleteApprovedGraphicsAsset" ClientIDMode="Static" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteApprovedGraphicsAsset_Click" CausesValidation="false"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                            <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="ReloadAttachment HiddenButton hidebtn" OnClick="btnReloadAttachment_Click"></asp:Button>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Links</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <ul>
                    <li>
                        <asp:LinkButton ID="lnlIPF" runat="server" CausesValidation="false" Text="Item Proposal Form" OnClick="lnlIPF_Click"></asp:LinkButton>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return ValidateData()" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
            <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        conditionalChecks();
        // Set Default State of Approval Dropdown
        if ($('#<%=ddlInitialApprovalReview.ClientID %> option:selected').text() == 'Rejected') {
            $('#lblRequestedInformation').html('<span class="markrequired">*</span>Reason for Rejection:');
            $('#dvRequestedUpdate').removeClass('hidden');
            $('#txtRequestedInformation').addClass('required');
        }
        else if ($('#<%=ddlInitialApprovalReview.ClientID %> option:selected').text() == 'Request IPF update') {
            $('#lblRequestedInformation').html('<span class="markrequired">*</span>Requested Update:');
            $('#dvRequestedUpdate').removeClass('hidden');
            $('#txtRequestedInformation').addClass('required');
        }
        else {
            $('#dvRequestedUpdate').addClass('hidden');
            $('#txtRequestedInformation').removeClass('required');
        }
        // Change of Approval Dropdown
        $('#<%=ddlInitialApprovalReview.ClientID %>').change(ddlInitialApprovalReview_Change);

        ddlInitialApprovalReview_Change();

        hideTimeLineDetails();

        var projectType = $("#hdnProjectType").val();
        if (projectType != "Graphics Change Only") {
            $(".GraphicsOnly").each(function () {
                $(this).addClass('hideItem');
            });
        }

        if (projectType == "Simple Network Move") {
            $('#ddlNeedSExpeditedWorkflowWithSGS').parent().addClass('hideItem');
        }
    });
    function hideTimeLineDetails() {
        if ($("#drpInitialTimeTable option:selected").text() == "Select...") {
            $('.timeline').each(function (i, obj) {
                $(this).hide();
            });
        }
    }
    function ddlInitialApprovalReview_Change() {
        if ($('#<%=ddlInitialApprovalReview.ClientID %> option:selected').text() == 'Approved') {
            $('#dvRequestedUpdate').addClass('hidden');
            $('#txtRequestedInformation').removeClass('required');
        } else {
            if ($('#<%=ddlInitialApprovalReview.ClientID %> option:selected').text() == 'Rejected') {
                // Set Label Text
                $('#lblRequestedInformation').html('<span class="markrequired">*</span>Reason for Rejection:');
                $('#dvRequestedUpdate').removeClass('hidden');
                $('#txtRequestedInformation').addClass('required');
            } else if ($('#<%=ddlInitialApprovalReview.ClientID %> option:selected').text() == 'Request IPF Update') {
                // Set Label Text
                $('#lblRequestedInformation').html('<span class="markrequired">*</span>Requested Update:');
                $('#dvRequestedUpdate').removeClass('hidden');
                $('#txtRequestedInformation').addClass('required');
            } else {
                $('#dvRequestedUpdate').addClass('hidden');
                $('#txtRequestedInformation').removeClass('required');
            }
        }
    }
    function conditionalChecks() {
        var cust = $('#lblCustomer').val().toLowerCase();
        if (cust.indexOf("select") != -1) {
            $('#dvCustomer').addClass('hidden');
        }
        else {
            $('#dvCustomer').removeClass('hidden');
        }
        var chn = $('#txtChannel').val().toLowerCase().trim();
        if (chn.indexOf("select") != -1 || chn == '') {
            $('#dvChannel').addClass('hidden');
        }
        else {
            $('#dvChannel').removeClass('hidden');
        }
        // Hide all fields
        $('#dvRequestedUpdate').addClass('hidden');
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

    function GotoPeoplePickerStepAndFocus(arg) {
        var scrollTo = $("div[title='People Picker']").eq(arg);
        scrollTo.addClass("highlightElement");
        scrollTo.focus();
        $('html, body').animate({
            scrollTop: scrollTo.offset().top - 100
        }, 1000);
    }

</script>
