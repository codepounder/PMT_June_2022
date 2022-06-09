<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemProposalForm2.ascx.cs" Inherits="Ferrara.Compass.WebParts.ItemProposalForm2.ItemProposalForm2" %>

<link rel="stylesheet" href="/_layouts/15/Ferrara.Compass/css/CompassStepper-Blue.css" />
<link href="/_layouts/15/Ferrara.Compass/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

<style type="text/css">
    .showTab {
        left: 0px;
        display: block !important;
    }

    .justifyRight {
        float: right;
    }

    .LoadAttachments {
        opacity: 0;
    }

    .showItempc {
        display: inline-block;
    }
</style>
<style id="printDiv" media="print" type="text/css">
    #divFormLinks, #IPFLinkHeaders, .ProjectNotesContainer, .actions, .hidebutton {
        display: none !important;
    }

    #divPrint {
        visibility: visible;
    }

    #divPrint {
        display: table;
        width: 100%;
    }

    div.row {
        display: table-row;
        width: 100%;
    }

    div.col-md-1 {
        display: table-cell;
        width: 8.33333333%;
    }

    div.col-md-2 {
        display: table-cell;
        width: 16.66666667%;
    }

    div.col-md-3 {
        display: table-cell;
        width: 25%;
    }

    div.col-md-4 {
        display: table-cell;
        width: 33.33333333%;
    }

    div.col-md-5 {
        display: table-cell;
        width: 41.66666667%;
    }

    div.col-md-6 {
        display: table-cell;
        width: 50%;
    }

    div.col-md-7 {
        display: table-cell;
        width: 58.33333333%;
    }

    div.col-md-8 {
        display: table-cell;
        width: 66.66666667%;
    }

    div.col-md-9 {
        display: table-cell;
        width: 75%;
    }

    div.col-md-10 {
        display: table-cell;
        width: 83.33333333%;
    }

    div.col-md-11 {
        display: table-cell;
        width: 91.66666667%;
    }

    div.col-md-12 {
        display: table-cell;
        width: 100%;
    }
</style>
<div class="container">
    <div class="content">
        <div class="col-lg-11 col-centered">
            <h1>Item Proposal Form</h1>
            <div class="row hidebutton">
                <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
                    <asp:ValidationSummary ID="ItemValidationSummary" ClientIDMode="Static" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
                    <div id="dverror_message" style="display: none;">
                        <ul id="error_message"></ul>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnTBDIndicator" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnParentProjectNumber" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDeleteApprovedGraphicsAssetUrl" ClientIDMode="Static" runat="server" />
            <asp:Button ID="hdnbtnDeleteApprovedGraphicsAsset" runat="server" ClientIDMode="Static" CssClass="hide" OnClick="hdnbtnDeleteApprovedGraphicsAsset_Click" />
            <asp:HiddenField ID="hdnDeleteVisualreferenceUrl" ClientIDMode="Static" runat="server" />
            <asp:Button ID="btnhdnDeleteVisualreferenceUrl" runat="server" ClientIDMode="Static" CssClass="hide" OnClick="btnhdnDeleteVisualreferenceUrl_Click" />
            <div id="loadingIcon" style="height: 400px; width: 500px;">
                <img src="/_layouts/15/Ferrara.Compass/images/loading.gif" alt="lOADING" />
            </div>
            <div id="wizard" class="wizard clearfix">
                <div class="steps clearfix">
                    <ul role="tablist" id="IPFLinkHeaders">
                        <li><a href="#wizard-h-0">1. Proposed Project</a></li>
                        <li><a href="#wizard-h-1">2. Project Team</a></li>
                        <li><a href="#wizard-h-2">3. SAP Item #</a></li>
                        <li><a href="#wizard-h-3">4. Project Specifications</a></li>
                        <li><a href="#wizard-h-4">5. Item Financial Details</a></li>
                        <li class=""><a href="#wizard-h-5">6. Customer Specifications</a></li>
                        <li class=""><a href="#wizard-h-6">7. Item Hierarchy</a></li>
                        <li class=""><a href="#wizard-h-7">8. Item UPCs</a></li>
                        <li class=""><a href="#wizard-h-8">9. Item Details</a></li>
                        <li class=""><a href="#wizard-h-9">10. Marketing Claims</a></li>
                        <li class=""><a href="#wizard-h-10">11. Attachments</a></li>
                        <li class=""><a href="#wizard-h-11">12. FG BOM Details</a></li>
                        <li class=""><a href="#wizard-h-12">13. Summary</a></li>
                    </ul>
                </div>
                <div class="content clearfix">
                    <div id="dverror_Proposed"></div>
                    <h2>1. Proposed Project</h2>
                    <section id="wizard-h-0">
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12 comment-highlighted">
                                <label class="control-label">Project Description: </label>
                                <label id="labProjectDescription"></label>
                                <div id="divProjectDescription"></div>
                                Please reference <a href='/Shared%20Documents/Project%20Types%20and%20Definitions.xlsx'>Project Types and Definitions</a> for more details.
                            </div>
                        </div>
                        <div class="row hide">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Item Description:</label>
                                    <%--<asp:TextBox ID="txtProposedItem" CssClass="required form-control" ClientIDMode="Static" MaxLength="40" runat="server"  onBlur="updateSAPDesc(this,'txtSAPItemDescription')"></asp:TextBox>--%>
                                    <asp:HiddenField ID="hdnSteps" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnStatus" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnNewIPF" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Project Type:</label>
                                    <asp:DropDownList ID="ddlProjectType" ClientIDMode="Static" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div class="SuccessMessage hideItem" id="ProjectTypeChangeNote">
                                        Please review the following drop downs as they may have changed.
                                        <ul>
                                            <li>Tab 3 - Is a New FG Item # Being Used?</li>
                                            <li>Tab 4 - New Base Formula, New Shape, New Flavor/Color, New Net Weight.</li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label id="lblFirstShipDate" runat="server" cssclass="control-label"><span class="markrequired">*</span>First Ship Date:</label>
                                    <asp:TextBox ID="txtFirstShipDate" ClientIDMode="Static" runat="server" CssClass="datePicker required form-control" onchange="fixMonth();" ToolTip="Click to Choose Date"></asp:TextBox>
                                    <div class="comment-block">Date the Finished Good inventory is physically in the Distribution Center and released.</div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Project Type SubCategory:</label>
                                    <asp:DropDownList ID="ddlProjectTypeSubCategory" ClientIDMode="Static" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-6 ddlCopyFormsForGraphicsProject">
                                <div class="form-group">
                                    <label class="control-label">Do you want to copy forms from previous project?:</label>
                                    <asp:DropDownList ID="ddlCopyFormsForGraphicsProject" ClientIDMode="Static" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnIPFCopiedFromCompassListItemId" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6 ddlExternalSemisItem">
                                <div class="form-group">
                                    <label class="control-label">Is this item external/contain external semis?:</label>
                                    <asp:DropDownList ID="ddlExternalSemisItem" ClientIDMode="Static" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnIPFSubmitted" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnExternalSemisItem" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <span class="markrequired">*</span>
                                    <label clientidmode="Static" id="lblChangeNotes" cssclass="control-label" runat="server">Change Notes:</label>
                                    <p class="comment-block">
                                        <label id="lblItemNote" class="comment-block">Please include the business case for this project as part of your Change Notes.</label>
                                    </p>
                                    <asp:TextBox ID="txtChangeNotes" runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="255" Rows="6" CssClass="required form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </section>
                    <h2>2. Project Team</h2>
                    <section id="wizard-h-1">
                        <asp:UpdatePanel ID="TeamPanel" runat="server">
                            <ContentTemplate>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4">
                                            <div class="form-group">
                                                <asp:Button ID="btnCopyInParentProjectTeam" OnClick="btnCopyInParentProjectTeam_Click" Text="Copy In Parent Project Team" Style="width: auto;" CssClass="ButtonControl" CausesValidation="false" runat="server" />
                                                <asp:HiddenField ID="hdnStageGateListItemId" Value="" runat="server" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4">
                                            <div class="form-group">
                                                <span class="markrequired">*</span><label class="control-label">Initiator:</label>
                                                <SharePoint:PeopleEditor ID="peInitiator" runat="server" AllowEmpty="false" MultiSelect="false" SelectionSet="User" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divProjectLeaders">
                                            <div class="form-group">
                                                <label class="control-label ProjectLeader"><span class="markrequired">*</span>Project Leader (PL):</label>
                                                <asp:Image ID="ImgAddProjectLeader" class="AddMember" onClick="AddProjectTeamMembersRow('btnAddProjectLeader')" Style="cursor: pointer; margin-top: 27px; height: 16px; float: right;" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add Project Leader" />
                                                <table class="MembersTable">
                                                    <asp:Repeater ID="rptProjectLeaders" runat="server" OnItemDataBound="rptProjectLeaders_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <SharePoint:PeopleEditor Width="96%" ID="peProjectLeaderMembers" runat="server" AllowEmpty="false" MultiSelect="false" SelectionSet="User" />
                                                                    </div>
                                                                </td>
                                                                <td <%# Container.ItemIndex == 0 ? "class=\"DeleteRow hide\"" : "class=\"DeleteRow\""%>>
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="deleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -16px;" onClick="deleteRow(this,'hdnDeletedStatusForProjectLeader');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForProjectLeader" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddProjectLeader" OnClick="btnAddProjectLeader_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divProjectManagers">
                                            <div class="form-group">
                                                <label class="control-label ProjectManager"><span class="markrequired">*</span>Project Manager (PM):</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div3">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlProjectManagerMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div4">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddProjectManager" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddProjectManager')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptProjectManagers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtProjectManagerMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtProjectManagerMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers txtMember" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForProjectManagerMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForProjectManagerMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddProjectManager" OnClick="btnAddProjectManager_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divSrProjectManagers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>Sr. Project Manager (Sr. PM):</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div5">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlSrProjectManagerMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div6">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddSrProjectManagers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddSrProjectManagerMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptSrProjectManagers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtSrProjectManagerMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtSrProjectManagerMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForSrProjectManagerMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForSrProjectManagerMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddSrProjectManagerMembers" OnClick="btnAddSrProjectManagers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divMarketingMembers">
                                            <div>
                                                <div class="form-group">
                                                    <label class="control-label"><span class="markrequired">*</span>Marketing:</label>
                                                </div>
                                            </div>
                                            <div>
                                                <div runat="server" id="div1">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlMarketingMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div2">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddMarketingMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddMarketingMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptMarketingMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtMarketingMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtMarketingMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForMarketingMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForMarketingMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddMarketingMembers" OnClick="btnAddMarketingMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divInTechMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor">*</span>InTech:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div7">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlInTechMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div8">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddInTechMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddInTechMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptInTechMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtInTechMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtInTechMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForInTechMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForInTechMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddInTechMembers" OnClick="btnAddInTechMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredInTechMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divQualityInnovationMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor">*</span>Quality Innovation (QA):</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div9">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlQualityInnovationMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div10">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddQualityInnovationMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddQualityInnovationMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptQualityInnovationMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtQualityInnovationMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtQualityInnovationMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForQualityInnovationMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForQualityInnovationMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddQualityInnovationMembers" OnClick="btnAddQualityInnovationMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredQualityInnovationMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divInTechRegulatoryMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor">*</span>InTech Regulatory:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div11">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlInTechRegulatoryMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div12">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddInTechRegulatoryMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddInTechRegulatoryMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptInTechRegulatoryMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtInTechRegulatoryMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtInTechRegulatoryMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForInTechRegulatoryMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForInTechRegulatoryMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddInTechRegulatoryMembers" OnClick="btnAddInTechRegulatoryMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredInTechRegulatoryMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divRegulatoryQAMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor">*</span>Regulatory QA:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div13">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlRegulatoryQAMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div14">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddRegulatoryQAMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddRegulatoryQAMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptRegulatoryQAMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtRegulatoryQAMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtRegulatoryQAMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForRegulatoryQAMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForRegulatoryQAMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddRegulatoryQAMembers" OnClick="btnAddRegulatoryQAMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredRegulatoryQAMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divPackagingEngineeringMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>Packaging Engineering (PE):</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div15">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlPackagingEngineeringMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div16">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddPackagingEngineeringMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddPackagingEngineeringMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptPackagingEngineeringMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtPackagingEngineeringMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtPackagingEngineeringMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForPackagingEngineeringMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForPackagingEngineeringMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddPackagingEngineeringMembers" OnClick="btnAddPackagingEngineeringMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divSupplyChainMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>Supply Chain (SC):</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div17">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlSupplyChainMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div18">
                                                    <div class="form-group">
                                                        <asp:Image ID="Image1" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddSupplyChainMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptSupplyChainMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtSupplyChainMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtSupplyChainMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForSupplyChainMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForSupplyChainMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddSupplyChainMembers" OnClick="btnAddSupplyChainMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divFinanceMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>Finance:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div19">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlFinanceMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div20">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddFinanceMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddFinanceMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptFinanceMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtFinanceMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtFinanceMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForFinanceMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForFinanceMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddFinanceMembers" OnClick="btnAddFinanceMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divSalesMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor">*</span>Sales (SSCM):</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div21">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlSalesMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div22">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddSalesMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddSalesMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptSalesMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtSalesMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtSalesMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForSalesMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForSalesMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddSalesMembers" OnClick="btnAddSalesMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredSalesMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divManufacturingMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor">*</span>Manufacturing:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div23">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlManufacturingMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div24">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddManufacturingMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddManufacturingMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptManufacturingMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtManufacturingMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtManufacturingMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForManufacturingMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForManufacturingMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddManufacturingMembers" OnClick="btnAddManufacturingMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredManufacturingMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divExternalMfgProcurementMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor RenQualWChangeRequiredOptionalPeopleEditor">*</span>External Mfg - Procurement:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div25">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlExternalMfgProcurementMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div26">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddExternalMfgProcurementMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddExternalMfgProcurementMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptExternalMfgProcurementMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtExternalMfgProcurementMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtExternalMfgProcurementMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForExternalMfgProcurementMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForExternalMfgProcurementMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddExternalMfgProcurementMembers" OnClick="btnAddExternalMfgProcurementMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredExternalMfgProcurementMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divPackagingProcurementMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor RenQualWChangeRequiredOptionalPeopleEditor">*</span>Packaging Procurement:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div27">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlPackagingProcurementMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div28">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddPackagingProcurementMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddPackagingProcurementMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptPackagingProcurementMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtPackagingProcurementMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtPackagingProcurementMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForPackagingProcurementMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForPackagingProcurementMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddPackagingProcurementMembers" OnClick="btnAddPackagingProcurementMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredPackagingProcurementMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divLifeCycleManagementMembers">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor RenQualWChangeRequiredOptionalPeopleEditor">*</span>Life Cycle Management:</label>
                                            </div>
                                            <div>
                                                <div runat="server" id="div29">
                                                    <div class="form-group">
                                                        <asp:DropDownList ID="ddlLifeCycleManagementMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div runat="server" id="div30">
                                                    <div class="form-group">
                                                        <asp:Image ID="ImgAddLifeCycleManagementMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddLifeCycleManagementMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                                    </div>
                                                </div>
                                            </div>
                                            <div>
                                                <table class="MembersTableNew" style="width: 100%">
                                                    <asp:Repeater ID="rptLifeCycleManagementMembers" runat="server" ClientIDMode="Static">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <asp:TextBox ID="txtLifeCycleManagementMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                                        <asp:TextBox ID="txtLifeCycleManagementMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                                    </div>
                                                                </td>
                                                                <td class="DeleteRow">
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForLifeCycleManagementMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForLifeCycleManagementMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddLifeCycleManagementMembers" OnClick="btnAddLifeCycleManagementMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                                <asp:HiddenField ID="hdnRequiredLifeCycleManagementMembers" Value="True" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divOtherTeamMembers">
                                            <div class="form-group">
                                                <label class="control-label">Other Team Members:</label>
                                                <asp:Image ID="ImgAddOtherTeamMembers" class="AddMember" onClick="AddProjectTeamMembersRow('btnAddOtherTeamMembers')" Style="cursor: pointer; margin-top: 27px; height: 16px; float: right;" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add other team members" />
                                                <table class="MembersTable" style="width: 94%">
                                                    <asp:Repeater ID="rptOtherTeamMembers" runat="server" OnItemDataBound="rptOtherTeamMembers_ItemDataBound">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td runat="server" id="tdDeliverable">
                                                                    <div class="form-group">
                                                                        <SharePoint:PeopleEditor Width="96%" ID="peOtherTeamMembers" runat="server" AllowEmpty="false" MultiSelect="false" SelectionSet="User" />
                                                                    </div>
                                                                </td>
                                                                <td <%# Container.ItemIndex == 0 ? "class=\"DeleteRow hide\"" : "class=\"DeleteRow\""%>>
                                                                    <div class="form-group">
                                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -16px;" onClick="deleteRow(this,'hdnDeletedStatusForOtherTeamMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                                        <asp:HiddenField ID="hdnDeletedStatusForOtherTeamMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                                <asp:Button ID="btnAddOtherTeamMembers" OnClick="btnAddOtherTeamMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </section>
                    <h2>3. SAP Item #</h2>
                    <section id="wizard-h-2">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Is a New FG Item # Being Used?:</label>
                                    <asp:DropDownList ID="ddlTBDIndicator" onchange="SAPNomenclature(ddlTBDIndicator)" CssClass="required form-control" ClientIDMode="Static" ToolTip="Please select TBD Indicator" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-12">
                                <div class="form-group">
                                    <label class="control-label">
                                        <span>
                                            <i class="fa fa-exclamation-circle" style="font-size: 12px; color: red"></i>
                                        </span>
                                        Please Note: If you press this button more than once, the BOM details will pull in multiple times.
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired" id="reqSAPItemNumber">*</span>Finished Good Item #:</label>
                                    <asp:TextBox ID="txtSAPItemNumber" ClientIDMode="Static" runat="server" CssClass="required alphanumericToUpper1 form-control minimumlength" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2" style="margin-left: -32px;">
                                <div class="col-xs-12 col-sm-6 col-md-10">
                                    <div class="form-group">
                                        <label class="control-label">&nbsp;</label>
                                        <asp:Button ID="btnLookupSAPItemNumber" CssClass="ButtonControl" runat="server" Text="Find" OnClick="btnLookupSAPItemNumber_Click" OnClientClick="return getCompDescriptionReturn('txtSAPItemNumber', 'txtSAPItemDescription');" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2 LookupSAPItemNumberIcon" style="padding-right: 0px !important; padding-left: 0px !important;">
                                    <i class="fa fa-exclamation-circle" style="font-size: 12px; color: red; margin-top: 25px; margin-left: 6px;"></i>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired" id="reqSAPItemDescription">*</span>Finished Good Item Description:</label>
                                    <asp:TextBox ID="txtSAPItemDescription" ClientIDMode="Static" MaxLength="60" CssClass="required form-control" runat="server" onBlur="updateSAPDesc(this,'txtProposedItem')"></asp:TextBox>
                                    <p class="comment-block" id="pSAPItemDescriptionMessage">
                                        <asp:Label ID="lblSAPItemDescriptionMessage" CssClass="comment-block" runat="server" ClientIDMode="Static" Text="If New FG #, description will automatically build based on information filled out in IPF."></asp:Label>
                                    </p>
                                    <p class="comment-block" id="pSAPItemDescription">
                                        <asp:Label ID="lblSAPItemDescription" CssClass="comment-block" runat="server" ClientIDMode="Static"></asp:Label>
                                    </p>

                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div id="dvLikeFGItem" class="form-group">
                                    <label class="control-label"><span class="markrequired" id="reqLikeFGItemNumber">*</span>"Like" Finished Good Item #:</label>
                                    <asp:TextBox ID="txtLikeFGItemNumber" ClientIDMode="Static" runat="server" class="alphanumericToUpper1 required form-control minimumlength" MaxLength="20" data-tip="Same Pack Type/Similar Unit Weight"></asp:TextBox>
                                    <p class="comment-block">"Like" Finished Good item should be same pack type and similar unit weight</p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2" style="margin-left: -32px;">
                                <div class="col-xs-12 col-sm-6 col-md-10">
                                    <div id="dvLikeFGItemFindButton" class="form-group">
                                        <label class="control-label">&nbsp;</label>
                                        <asp:Button ID="btnLookupLikeSAPItemNumber" CssClass="ButtonControl" runat="server" Text="Find" OnClick="btnLookupLikeSAPItemNumber_Click" OnClientClick="return getCompDescriptionReturn('txtLikeFGItemNumber', 'txtLikeItemDescription');" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2 LookupLikeSAPItemNumber" style="padding-right: 0px !important; padding-left: 0px !important;">
                                    <i class="fa fa-exclamation-circle" style="font-size: 12px; color: red; margin-top: 25px; margin-left: 6px;"></i>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvLikeFGItemDesc" class="form-group">
                                    <label class="control-label"><span class="markrequired" id="reqLikeItemDescription">*</span>"Like" Finished Good Item Description:</label>
                                    <asp:TextBox ID="txtLikeItemDescription" ClientIDMode="Static" CssClass="required form-control" MaxLength="60" runat="server"></asp:TextBox>
                                    <p class="comment-block">Additional comments can be added to the "Project Notes" if needed</p>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div id="dvOldFGItem" class="form-group">
                                    <label class="control-label">Old Finished Good Item #:</label>
                                    <asp:TextBox ID="txtOldFGItemNumber" ClientIDMode="Static" runat="server" class="alphanumericToUpper1 form-control minimumlength" MaxLength="20" data-tip="Old Finished Good"></asp:TextBox>
                                    <p class="comment-block">Old Finished Good Item should be the Finished Good Number the new item is replacing either permanently or temporarily</p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2" style="margin-left: -32px;">
                                <div class="col-xs-12 col-sm-6 col-md-10">
                                    <div id="dvOldFGItemFindButton" class="form-group">
                                        <label class="control-label">&nbsp;</label>
                                        <asp:Button ID="btnLookupOldSAPItemNumber" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="return getCompDescriptionReturn('txtOldFGItemNumber', 'txtOldItemDescription');return false;" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2 hide" style="padding-right: 0px !important; padding-left: 0px !important;">
                                    <i class="fa fa-exclamation-circle" style="font-size: 7px; color: red; margin-top: 25px; margin-left: 27px;"></i>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvOldFGItemDesc" class="form-group">
                                    <label class="control-label">Old Finished Good Item Description:</label>
                                    <asp:TextBox ID="txtOldItemDescription" ClientIDMode="Static" CssClass="form-control" MaxLength="60" runat="server"></asp:TextBox>
                                    <p class="comment-block">Additional comments can be added to the "Project Notes" if needed</p>
                                </div>
                            </div>
                        </div>
                    </section>

                    <h2>4. Project Specifications</h2>
                    <section id="wizard-h-3">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvNewFormula" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New Base Formula?:</label>
                                    <asp:DropDownList ID="ddlNewFormula" ClientIDMode="Static" CssClass="required form-control" ToolTip="Please select New Formula?" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="Unknown at this time" Value="U"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvNewShape" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New Shape?:</label>
                                    <asp:DropDownList ID="ddlNewShape" ClientIDMode="Static" CssClass="required form-control" ToolTip="Please select New Shape?" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="Unknown at this time" Value="U"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvNewFlavor" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New Flavor/Color?:</label>
                                    <asp:DropDownList ID="ddlNewFlavorColor" ClientIDMode="Static" CssClass="required form-control" ToolTip="Please select New Flavor/Color?" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="Unknown at this time" Value="U"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvNewNetWeight" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New Net Weight?:</label>
                                    <asp:DropDownList ID="ddlNewNetWeight" ClientIDMode="Static" CssClass="required form-control" ToolTip="Please select New Net Weight?" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="Unknown at this time" Value="U"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Organic?:</label>
                                    <asp:DropDownList ID="ddlOrganic" ClientIDMode="Static" CssClass="required form-control" ToolTip="Please select Organic?" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvServingSize" class="form-group">
                                    <label class="control-label">Serving Size/Piece Weight Change:</label>
                                    <asp:DropDownList ID="ddlServiceSizeChange" ClientIDMode="Static" CssClass="form-control" ToolTip="Please select Serving Size/Piece Weight Change" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </section>

                    <h2>5. Item Financial Details</h2>
                    <section id="wizard-h-4">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvlast12" class="form-group">
                                    <label class="control-label"><span class="markrequired" id="lbllast12">*</span>Last 12 Months Sales $:</label>
                                    <div class="input-group"><span class="input-group-addon">$</span><asp:TextBox ID="txtLast12MonthSales" ClientIDMode="Static" runat="server" CssClass="NMNumber currencyNoDecimal required form-control" aria-describedby="aoLast12MonthSales"></asp:TextBox></div>
                                    <p class="comment-block">
                                        <asp:Label ID="lblLast12MonthSales" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Truckload Price per Retail Selling Unit:</label>
                                    <div class="input-group">
                                        <span class="input-group-addon">$</span>
                                        <asp:TextBox ID="txtTruckLoadSellingPrice" ClientIDMode="Static" runat="server" CssClass="NMNumber required numericCommaDecimal2 form-control"></asp:TextBox>
                                    </div>
                                    <p class="comment-block">
                                        <asp:Label ID="lblTruckLoadSellingPrice" CssClass="comment-block" runat="server">Retail Selling Unit = Consumer Unit Sold</asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvFCC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>FCC Expected Gross Margin %:</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtExpectedGrossMarginPercent" ClientIDMode="Static" runat="server" CssClass="NMNumber required numericCommaDecimal2 form-control" MaxLength="50"></asp:TextBox><span class="input-group-addon">%</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>12 Month Projected $:</label>
                                    <div class="input-group"><span class="input-group-addon">$</span><asp:TextBox ID="txtAnnualProjectedDollars" ClientIDMode="Static" runat="server" CssClass="NMNumber required currencyNoDecimalAllowZero form-control"></asp:TextBox></div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Annual Projected Retail Selling Units:</label>
                                    <asp:TextBox ID="txtAnnualProjectedUnits" ClientIDMode="Static" runat="server" CssClass="NMNumber required currencyNoDecimalAllowZero form-control"></asp:TextBox>
                                    <p class="comment-block">
                                        <asp:Label ID="lblAnnualProjectedUnits" CssClass="comment-block" runat="server">Retail Selling Unit = Consumer Unit Sold</asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span><span id="1stmonth"></span> Projected $:</label>
                                    <div class="input-group"><span class="input-group-addon">$</span><asp:TextBox ID="txtMonth1ProjectedDollars" ClientIDMode="Static" runat="server" CssClass="NMNumber required currencyNoDecimalAllowZero form-control"></asp:TextBox></div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span><span id="1stmonthU"></span> Projected Retail Selling Units:</label>
                                    <asp:TextBox ID="txtMonth1ProjectedUnits" ClientIDMode="Static" runat="server" CssClass="NMNumber required form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span><span id="2ndmonth"></span> Projected $:</label>
                                    <div class="input-group"><span class="input-group-addon">$</span><asp:TextBox ID="txtMonth2ProjectedDollars" ClientIDMode="Static" runat="server" CssClass="NMNumber required currencyNoDecimalAllowZero form-control"></asp:TextBox></div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span><span id="2ndmonthU"></span> Projected Retail Selling Units:</label>
                                    <asp:TextBox ID="txtMonth2ProjectedUnits" ClientIDMode="Static" runat="server" CssClass="NMNumber required form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span><span id="3rdmonth"></span> Projected $:</label>
                                    <div class="input-group"><span class="input-group-addon">$</span><asp:TextBox ID="txtMonth3ProjectedDollars" ClientIDMode="Static" runat="server" CssClass="NMNumber required currencyNoDecimalAllowZero form-control"></asp:TextBox></div>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span><span id="3rdmonthU"></span> Projected Retail Selling Units:</label>
                                    <asp:TextBox ID="txtMonth3ProjectedUnits" ClientIDMode="Static" runat="server" CssClass="NMNumber required form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </section>

                    <h2>6. Customer Specifications</h2>
                    <section id="wizard-h-5">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Customer/Channel Specific:</label>
                                    <asp:DropDownList ID="ddlCustomerSpecific" onchange="SAPNomenclature()" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Customer Specific" Value="CU"></asp:ListItem>
                                        <asp:ListItem Text="Channel Specific" Value="CH"></asp:ListItem>
                                        <asp:ListItem Text="Pricelist" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvCustomer" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Customer:</label>
                                    <asp:DropDownList ID="ddlCustomer" onchange="SAPNomenclature()" ClientIDMode="Static" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvCustomerSpecific" class="form-group">
                                    <label class="control-label">Customer/Specific Lot Code:</label>
                                    <asp:TextBox ID="txtCustomerSpecificLotCode" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvChannel" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Channel:</label>
                                    <asp:DropDownList ID="ddlChannel" ClientIDMode="Static" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Sold outside of USA?:</label>
                                    <asp:DropDownList ID="ddlOutsideUSA" onchange="SAPNomenclature()" ClientIDMode="Static" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvCountryofSale" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Country of Sale:</label>
                                    <asp:DropDownList ID="ddlCountryOfSale" onchange="SAPNomenclature()" ClientIDMode="Static" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </section>

                    <h2>7. Item Hierarchy</h2>
                    <section id="wizard-h-6">
                        <asp:UpdatePanel ID="hierarchyPanel" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlProductHierarchyLevel1" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlProductHierarchyLevel2" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlBrand_Material" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                    <ProgressTemplate>
                                        <div id="reloadingCover" clientidmode="Static" runat="server" style="height: 350px; position: absolute; width: 100%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                                            <div class="disablingLoadingIcon" id="disablingLoadingHierarchy" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                <div class="col-xs-12 col-sm-12 col-md-12 comment-highlighted" style="padding-top: 15px; padding-bottom: 15px;">
                                    <a href='/Shared%20Documents/Hierarchy%20and%20Brand%20PMT%20Materials.pptx'>Hierarchy/Brand Guidance</a>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 1:</label>
                                            <asp:DropDownList ID="ddlProductHierarchyLevel1" CssClass="required HierarchyLevel1 form-control" runat="server"
                                                AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlProductHierarchyLevel1_SelectedIndexChanged" ClientIDMode="Static">
                                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <p class="comment-block">
                                                <asp:Label ID="lblProductHierarchyLevel1" CssClass="comment-block" runat="server"></asp:Label>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-6" id="divManuallyCreateSAPDescription">
                                        <div class="form-group">
                                            <label class="control-label">Manually Create SAP Description:</label>
                                            <asp:DropDownList ID="ddlManuallyCreateSAPDescription" onchange="SAPNomenclature()" CssClass="form-control" runat="server" ClientIDMode="Static">
                                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                                <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 2:</label>
                                            <asp:DropDownList ID="ddlProductHierarchyLevel2" OnSelectedIndexChanged="ddlProductHierarchyLevel2_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <p class="comment-block">
                                                <asp:Label ID="lblProductHierarchyLevel2" CssClass="comment-block" runat="server"></asp:Label>
                                            </p>
                                            <asp:HiddenField ID="hdnddlProductHierarchyLevel2" runat="server" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <label class="control-label"><span class="markrequired">*</span>Material Group 1 (Brand):</label>
                                            <asp:DropDownList ID="ddlBrand_Material" OnSelectedIndexChanged="ddlBrand_Material_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <p class="comment-block">
                                                <asp:Label ID="lblBrand_Material" CssClass="comment-block" runat="server"></asp:Label>
                                            </p>
                                            <asp:HiddenField ID="hdnddlBrand_Material" runat="server" ClientIDMode="Static" />
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <label class="control-label"><span class="markrequired">*</span>Profit Center:</label>
                                            <asp:TextBox ID="txtProfitCenter" ClientIDMode="Static" runat="server" ReadOnly="true" class="required form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Material Group 4 (Product Form):</label>
                                    <asp:DropDownList ID="ddlMaterialGroup4" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div>
                                        <a href='/Shared%20Documents/Guide%20to%20Material%20Group%204.pptx'>Guide to Material Group 4</a>
                                    </div>
                                    <p class="comment-block">
                                        <asp:Label ID="lblMaterialGroup4" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group" id="divProductFormDescription">
                                    <label class="control-label"><span class="markrequired spProductFormDescription">*</span>Product Form Description:</label>
                                    <asp:TextBox runat="server" ID="txtProductFormDescription" onchange="SAPNomenclature()" Style="text-transform: uppercase;" class="required form-control" ClientIDMode="static" MaxLength="9" />
                                    <p class="comment-block" id="pProductFormDescription">
                                        Characters remaining:
                                        <asp:Label ID="lblProductFormDescription" CssClass="comment-block" runat="server" ClientIDMode="Static"></asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div id="dvMixes" class="repeater">
                            <fieldset>
                                <legend>Mix Details
                                </legend>
                                <p class="comment-block">Please note that the calculations for "Qty for Mix" and "Lbs for FG BOM" are based off of <a style="color: darkblue;" onclick='GotoStepAndFocusNoRed("txtRetailSellingUnitsPerBaseUOM")'>"Selling Units Per Base UOM"</a>.  If this is not filled out, the calculations will not occur until after you submit the IPF.</p>
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-12 col-md-12">
                                                <div id="MixesError_Message" runat="server"></div>
                                                <table>
                                                    <asp:Repeater ID="rpMixes" runat="server" OnItemCommand="rpMixes_ItemCommand" OnItemDataBound="rpMixes_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <tr>
                                                                <th><span class="markrequired">*</span>Item # in Mix</th>
                                                                <th>&nbsp;</th>
                                                                <th>Item Description</th>
                                                                <th>Total Pieces per Selling Unit</th>
                                                                <th>Ounces per Piece</th>
                                                                <th>Ounces per Selling Unit</th>
                                                                <th>Grams per Selling Unit</th>
                                                                <th>Qty for Mix</th>
                                                                <th>Lbs for FG BOM</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hidMixesId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemId") %>' />
                                                                    <asp:HiddenField ID="hdnCompassListItemId" runat="server" Value=' <%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                                                    <input type="text" runat="server" id="txtMixItemNumber" title="Item # in Mix" class="minimumlength Mixesrequired form-control txtMixItemNumber alphanumericNumeric" maxlength="9" value='<%# DataBinder.Eval(Container.DataItem, "ItemNumber") %>' />
                                                                </td>
                                                                <td>
                                                                    <label class="control-label" style="display: none;">&nbsp;</label>
                                                                    <asp:Button ID="btnLookupMixesItemNumber" CssClass="ButtonControl" runat="server" Text="Find" ClientIDMode="Static" OnClientClick="getTableCompDescription(this,'txtMixItemNumber','txtMixItemDescription');return false;" />
                                                                </td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtMixItemDescription" title="Item Description" class="form-control txtMixItemDescription" value='<%# DataBinder.Eval(Container.DataItem, "ItemDescription") %>' />
                                                                </td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtMixNumberOfPieces" title="Total Pieces per Selling Unit" class="form-control numericDecimal0" clientidmode="static" oninput="updateOPSU(event,this);" value='<%# DataBinder.Eval(Container.DataItem, "NumberOfPieces") %>' />
                                                                </td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtOzPerPiece" title="Ounces per Piece" class="form-control numericDecimal3" clientidmode="static" oninput="updateOPSU(event,this);" value='<%# DataBinder.Eval(Container.DataItem, "OuncesPerPiece") %>' />
                                                                </td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtOzPerSellingUnit" title="Ounces per Selling Unit" clientidmode="static" class="decimal form-control" readonly="readonly" />
                                                                </td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtGramsPerSellingUnit" title="Grams per Selling Unit" clientidmode="static" class="decimal form-control" readonly="readonly" />
                                                                </td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtQtyMix" title="Qty for Mix" class="decimal form-control" clientidmode="static" readonly="readonly" />
                                                                </td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtLbsFGBOM" title="Lbs for FG BOM" class="decimal form-control" clientidmode="static" readonly="readonly" />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnDelete" CssClass="ButtonControl" CausesValidation="false" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId") %>' runat="server" Text="Delete" />
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-6 col-md-6">
                                                <div class="form-group">
                                                    <asp:Button ID="btnAddMixItem" CssClass="ButtonControlAutoSize" CausesValidation="false" runat="server" OnClientClick="return RepeaterValidator('Mixesrequired', true);" Text="Add New Mix Item" OnClick="btnAddMixItem_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Material Group 5 (Pack Type):</label>
                                    <asp:DropDownList ID="ddlMaterialGroup5" onchange="SAPNomenclature()" ClientIDMode="Static" CssClass="required form-control" ToolTip="Material Group 5 is required" runat="server" AppendDataBoundItems="true" TabIndex="12">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <div>
                                        <a href='/Shared%20Documents/Guide%20to%20Material%20Group%205.pptx'>Guide to Material Group 5</a>
                                    </div>
                                    <p class="comment-block">
                                        <asp:Label ID="lblMaterialGroup5" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div id="dvShipper" class="repeater">
                            <fieldset>
                                <legend>Shipper Details</legend>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-12 col-md-12">
                                                <div id="FGError_Message"></div>
                                                <table id="tabShipper">
                                                    <asp:Repeater ID="rptShipper" runat="server" OnItemCommand="rptShipper_ItemCommand" OnItemDataBound="rptShipper_ItemDataBound">
                                                        <HeaderTemplate>
                                                            <tr>
                                                                <th><span class="markrequired">*</span>FG Item # in Display</th>
                                                                <th></th>
                                                                <th>FG Item Description</th>
                                                                <th># of Units</th>
                                                                <th>Ounces per Unit</th>
                                                                <th>Ounces per FG Unit</th>
                                                                <th>Pack Unit</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td>
                                                                    <asp:HiddenField ID="hidShipperId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemId") %>' />
                                                                    <asp:HiddenField ID="hdnCompassListItemId" runat="server" Value=' <%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                                                    <input type="text" runat="server" id="txtFGItemDisplay" class="form-control minimumlength FGItemrequired txtFGItemDisplay alphanumericNumeric"
                                                                        title="FG Item # in Display" maxlength="6" value='<%# DataBinder.Eval(Container.DataItem, "FGItemNumber") %>' />
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnLookupFGDesc" ClientIDMode="Static" CssClass="ButtonControl" runat="server" Text="Find" CommandName="Find"
                                                                        OnClientClick="getTableCompDescription(this,'txtFGItemDisplay','txtFGItemDescription');return false;"
                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FGItemNumber") %>' /></td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtFGItemDescription" class="form-control txtFGItemDescription"
                                                                        value='<%# DataBinder.Eval(Container.DataItem, "FGItemDescription") %>' title="FG Item Description" /></td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtFGnumberUnits" class="form-control numericDecimal0"
                                                                        title="# of Units" oninput="updateOuncesPerFGunit(event, this);"
                                                                        value='<%# DataBinder.Eval(Container.DataItem, "FGItemNumberUnits") %>' clientidmode="Static" /></td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtFGouncesPerUnit" class="form-control numericDecimal2" title="Ounces per Unit"
                                                                        value='<%# DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit") %>'
                                                                        oninput="updateOuncesPerFGunit(event, this);" clientidmode="Static" /></td>
                                                                <td>
                                                                    <input type="text" runat="server" id="txtFGouncesPerFGunit" class="form-control"
                                                                        title="Ounces per FG Unit" value='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "FGItemNumberUnits")) *
                                                    Convert.ToDouble(DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit")) %>'
                                                                        clientidmode="Static" disabled="disabled" /></td>
                                                                <td style="width: 100px">
                                                                    <asp:DropDownList ID="ddlFGPackUnit" runat="server" class="form-control" ClientIDMode="Static" ToolTip="Pack Unit">
                                                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                                                    </asp:DropDownList></td>
                                                                <td>
                                                                    <asp:Button ID="btndelete" CssClass="ButtonControl" CausesValidation="false" CommandName="Delete"
                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ItemId") %>' runat="server" Text="Delete" /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <tr>
                                                        <td></td>
                                                        <td></td>
                                                        <td>Totals</td>
                                                        <td>
                                                            <asp:TextBox ID="txtFGTotalQuantityUnitsInDisplay" ClientIDMode="Static" runat="server"
                                                                CssClass="required numericNoMask form-control" disabled="disabled" ToolTip="FG Total Quantity Units In Display"></asp:TextBox></td>
                                                        <td></td>
                                                        <td>
                                                            <asp:TextBox ID="txtFGTotalTotalOuncesPerShipper" ClientIDMode="Static" runat="server"
                                                                CssClass="required numericNoMask form-control" disabled="disabled" ToolTip="FG Total Ounces Per Shipper"></asp:TextBox></td>
                                                        <td></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-xs-12 col-sm-6 col-md-6">
                                                <div class="form-group">
                                                    <asp:Button ID="btnAddShipperFinishedGood" CssClass="ButtonControlAutoSize" CausesValidation="false" runat="server" OnClientClick="return RepeaterValidator('FGItemrequired', true);" Text="Add New Shipper Item" OnClick="btnAddShipperFinishedGood_Click" />
                                                </div>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </fieldset>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Is this project considered to be Novelty?:</label>
                                    <asp:DropDownList ID="drpNovelyProject" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </section>

                    <h2>8. Item UPCs</h2>
                    <section id="wizard-h-7">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Do we need any new UPCs (12 digit GTIN) or UCCs (14 digit GTIN)?:</label>
                                    <asp:DropDownList ID="ddlNeedNewUPCUCC" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvddlNeedNewUPC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Do we need a New Consumer Unit UPC (12 digit GTIN) – EACH?:</label>
                                    <asp:DropDownList ID="ddlNeedNewUnitUPC" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvUnitUPC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Existing Consumer Unit UPC (12 digit GTIN) – EACH:</label>
                                    <asp:TextBox ID="txtUnitUPC" ClientIDMode="Static" runat="server" class="required form-control" MaxLength="13"></asp:TextBox>
                                    <label class="comment-block">1st Level UPC - This should be the smallest packaged item. This could also be a 13 digit EAN code.</label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvNeedNewDisplayBoxUPC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Do we need a New Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray?:</label>
                                    <asp:DropDownList ID="ddlNeedNewDisplayBoxUPC" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvDisplayBoxUPC" class="form-group">
                                    <label class="control-label">Existing Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray:</label>
                                    <asp:TextBox ID="txtDisplayUPCBox" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="13"></asp:TextBox>
                                    <label class="comment-block">2nd Level UPC – This would be used if there is a UPC on a secondary level of packaging for an item; i.e. display, carton, display tray, etc</label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvSAPBaseUOM" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>SAP Base UOM:</label>
                                    <asp:DropDownList ID="ddlSAPBUOM" CssClass="required form-control" ClientIDMode="Static" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label class="comment-block">Only use “PAL” if the full pallet can only be sold to the customer, i.e. club pallets. ½ and ¼ pallets/mods must use CS as the SAP Base UOM.</label>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvNeedNewCaseUCC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Do we need a new Case UCC (14 digit GTIN)?:</label>
                                    <asp:DropDownList ID="ddlNeedNewCaseUCC" ClientIDMode="Static" CssClass="required form-control" runat="server">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvCaseUCC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Existing Case UCC (14 digit GTIN):</label>
                                    <asp:TextBox ID="txtCaseUCC" ClientIDMode="Static" runat="server" CssClass="required form-control" MaxLength="14"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvNeedNewPalletUCC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Do we need a new Pallet UCC (14 digit GTIN)?:</label>
                                    <asp:DropDownList ID="ddlNeedNewPalletUCC" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvPalletUCC" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Existing Pallet UCC (14 digit GTIN):</label>
                                    <asp:TextBox ID="txtPalletUCC" ClientIDMode="Static" runat="server" CssClass="required form-control" MaxLength="14"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </section>

                    <h2>9. Item Details</h2>
                    <section id="wizard-h-8">
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvCaseType" class="form-group">
                                    <label class="control-label">Case Type:</label>
                                    <asp:DropDownList ID="ddlCaseType" onchange="SAPNomenclature()" CssClass="form-control" ClientIDMode="Static" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvFilmSubstrate" class="form-group">
                                    <label class="control-label">Film Substrate:</label>
                                    <asp:DropDownList ID="ddlFilmSubstrate" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Peg Hole Needed?:</label>
                                    <asp:DropDownList ID="ddlPegHoleNeeded" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>

                        <div id="dvInvolvesCarton" class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Does this project involve a carton/mixed bag/tray or other product form that goes within a case but also contains individual units within it?:</label>
                                    <asp:DropDownList ID="ddlInvolvesCarton" onchange="SAPNomenclature()" ClientIDMode="Static" CssClass=" form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6" id="dvUnitsInsideCarton" style="padding-top: 40px;">
                                <div class="form-group">
                                    <label class="control-label">Number of Units Inside of Carton/Mixed Bag/Tray/Etc:</label>
                                    <asp:TextBox ID="txtUnitsInsideCarton" MaxLength="5" onchange="SAPNomenclature()" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                        <div id="divNumberofTraysPerBaseUOM" class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Number of Trays per Base UOM:</label>
                                    <asp:TextBox ID="txtNumberofTraysPerBaseUOM" MaxLength="3" onchange="SAPNomenclature()" ClientIDMode="Static" runat="server" CssClass="NMNumber numericCommaDecimal0 form-control"></asp:TextBox>
                                    <p class="comment-block">
                                        <asp:Label ID="Label2" CssClass="comment-block" runat="server">If field is filled out, this value will replace the "Retail Selling Unit per Base UOM" in the description</asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Individual Pouch Weight (oz):</label>
                                    <asp:TextBox ID="txtIndividualPouchWeight" MaxLength="5" onchange="SAPNomenclature()" runat="server" ClientIDMode="Static" CssClass="NMNumber decimal form-control"></asp:TextBox>
                                    <p class="comment-block">
                                        <asp:Label ID="Label3" CssClass="comment-block" runat="server">If field is filled out, this value will replace the "Retail Unit Weight" in the description</asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvRetailSellingUnits" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Retail Selling Units Per Base UOM:</label>
                                    <asp:TextBox ID="txtRetailSellingUnitsPerBaseUOM" MaxLength="4" onchange="SAPNomenclature()" ClientIDMode="Static" runat="server" CssClass="NMNumber required numericCommaDecimal0 form-control"></asp:TextBox>
                                    <p class="comment-block">
                                        <asp:Label ID="lblRetailSellingUnitsPerBaseUOM" CssClass="comment-block" runat="server">Retail Selling Unit = Consumer Unit Sold</asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvRetailunitWeight" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Retail Unit Weight (oz):</label>
                                    <asp:TextBox ID="txtRetailUnitWeight" MaxLength="5" onchange="SAPNomenclature()" runat="server" ClientIDMode="Static" CssClass="NMNumber required decimal form-control"></asp:TextBox>
                                    <p class="comment-block">
                                        <asp:Label ID="lblRetailUnitWeight" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvBaseUnitMeasure" class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Base UOM Net Weight (lbs):</label>
                                    <asp:TextBox ID="txtBaseUofMNetWeight" ClientIDMode="Static" runat="server" CssClass="NMNumber required form-control"></asp:TextBox>
                                    <p class="comment-block">
                                        <asp:Label ID="lblBaseUofMNetWeight" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                    </section>
                    <h2>10. Marketing Claims</h2>
                    <section id="wizard-h-9">
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div id="dvClaims" class="form-group">
                                    <label class="control-label">Marketing Claims/Labeling Requirements:</label>
                                    <asp:TextBox ID="txtClaimsLabelingRequirements" ClientIDMode="Static" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Are there any desired claims?:</label>
                                    <asp:DropDownList ID="drpDesiredClaims" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        <asp:ListItem Text="Use Existing Claims" Value="Existing"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New NLEA Format:</label>
                                    <asp:DropDownList ID="drpNewNLEAFormat" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        <asp:ListItem Text="Not Updating NLEA" Value="NA"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Is Bio-Engineering Labeling Acceptable for this item?:</label>
                                    <asp:DropDownList ID="drpBioEngineeringLabelingAcceptable" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label class="comment-block" style="color: #777777;">
                                        Marking Yes you are saying that you are ok with a QR code or regulatory language on your item that would direct the consumer to a webpage stating your item is or is not BE.<br />
                                        N/A can only be used if item only contains Finished Goods already labeled appropriately for BE.</label>
                                </div>
                            </div>
                        </div>
                        <div class="row existingClaims">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Finished Good / Component #:</label>
                                    <asp:TextBox ID="txtMaterialClaimsCompNumber" ClientIDMode="Static" runat="server" CssClass="alphanumericToUpper1 minimumlength form-control" MaxLength="20" Text=""></asp:TextBox>
                                    <label class="comment-block" style="color: #777777;">Please utilize claims from this Finished Good / Component #</label>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <div class="form-group">
                                    <label class="control-label">&nbsp;</label>
                                    <asp:Button ID="btnLookupCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getCompDescriptionByMaterialMasterList('txtMaterialClaimsCompNumber', 'txtMaterialClaimsCompDesc');return false;" />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Finished Good / Component Description:</label>
                                    <asp:TextBox ID="txtMaterialClaimsCompDesc" ClientIDMode="Static" runat="server" CssClass="form-control" Text=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row marketingClaims">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Sellable Unit:</label>
                                    <asp:DropDownList ID="drpSellableUnit" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row marketingClaims">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Organic:</label>
                                    <asp:DropDownList ID="drpOrganic" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Natural Colors:</label>
                                    <asp:DropDownList ID="drpNaturalColors" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Natural / Real Flavors Claims:</label>
                                    <asp:DropDownList ID="drpNaturalFlavors" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row marketingClaims">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Fat Free:</label>
                                    <asp:DropDownList ID="drpFatFree" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Preservative Free:</label>
                                    <asp:DropDownList ID="drpPreservativeFree" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Lactose Free:</label>
                                    <asp:DropDownList ID="drpLactoseFree" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row marketingClaims">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Gluten Free:</label>
                                    <asp:DropDownList ID="drpGlutenFree" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Low Sodium:</label>
                                    <asp:DropDownList ID="drpLowSodium" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Kosher:</label>
                                    <asp:DropDownList ID="drpKosher" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row marketingClaims">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Made In USA Claim:</label>
                                    <asp:DropDownList ID="drpMadeInUSA" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <div class="form-group">
                                    <label class="control-label">Made In USA Claim Details:</label>
                                    <asp:DropDownList ID="drpMadeInUSAPct" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row marketingClaims">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>GMO Claim:</label>
                                    <asp:DropDownList ID="drpGMOClaim" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-12 col-md-8">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Juice Concentrate:</label>
                                    <asp:TextBox ID="txtJuiceConcentrate" ClientIDMode="Static" TextMode="MultiLine" Rows="6" runat="server" CssClass="form-control required"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row marketingClaims">
                            <span class="headSummary">Nutrients</span>
                        </div>
                        <div class="row marketingClaims">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Good / Excellent Source:</label>
                                    <asp:Table ID="tabChannel" runat="server">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <label class="control-label">Available: </label>
                                                <br />
                                                <asp:ListBox ID="drpGoodSourceAvailable" Height="125px" Width="150px" CssClass="" SelectionMode="Multiple" ClientIDMode="Static" runat="server"></asp:ListBox>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                <button type="button" onclick="addChannel()" id="availableSources" class="button noIcon">Add</button>
                                <button type="button" onclick="removeChannel()" id="selectedSources" class="button noIcon">Remove</button>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <label class="control-label">Selected: </label>
                                                <br />
                                                <asp:ListBox ID="drpGoodSourceSelected" Height="125px" Width="150px" SelectionMode="Multiple" CssClass="required" ClientIDMode="Static" runat="server"></asp:ListBox>
                                                <asp:HiddenField ID="hdnSelectedGoodSource" ClientIDMode="Static" runat="server"></asp:HiddenField>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin A:</label>
                    <asp:DropDownList ID="drpVitaminA" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin A Percent:</label>
                                    <asp:DropDownList ID="drpVitaminAPct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin B1:</label>
                    <asp:DropDownList ID="drpVitaminB1" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin B1 Percent:</label>
                                    <asp:DropDownList ID="drpVitaminB1Pct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin B2:</label>
                    <asp:DropDownList ID="drpVitaminB2" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin B2 Percent:</label>
                                    <asp:DropDownList ID="drpVitaminB2Pct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin B3:</label>
                    <asp:DropDownList ID="drpVitaminB3" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin B3 Percent:</label>
                                    <asp:DropDownList ID="drpVitaminB3Pct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin B5:</label>
                    <asp:DropDownList ID="drpVitaminB5" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin B5 Percent:</label>
                                    <asp:DropDownList ID="drpVitaminB5Pct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin B6:</label>
                    <asp:DropDownList ID="drpVitaminB6" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin B6 Percent:</label>
                                    <asp:DropDownList ID="drpVitaminB6Pct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin B12:</label>
                    <asp:DropDownList ID="drpVitaminB12" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin B12 Percent:</label>
                                    <asp:DropDownList ID="drpVitaminB12Pct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin C:</label>
                    <asp:DropDownList ID="drpVitaminC" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin C Percent:</label>
                                    <asp:DropDownList ID="drpVitaminCPct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin D:</label>
                    <asp:DropDownList ID="drpVitaminD" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin D Percent:</label>
                                    <asp:DropDownList ID="drpVitaminDPct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Vitamin E:</label>
                    <asp:DropDownList ID="drpVitaminE" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Vitamin E Percent:</label>
                                    <asp:DropDownList ID="drpVitaminEPct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Potassium:</label>
                    <asp:DropDownList ID="drpPotassium" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Potassium Percent:</label>
                                    <asp:DropDownList ID="drpPotassiumPct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Iron:</label>
                    <asp:DropDownList ID="drpIron" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Iron Percent:</label>
                                    <asp:DropDownList ID="drpIronPct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <%--<div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Calcium:</label>
                    <asp:DropDownList ID="drpCalcium" ClientIDMode="Static" CssClass="form-control vitamin" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>--%>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label">Calcium Percent:</label>
                                    <asp:DropDownList ID="drpCalciumPct" ClientIDMode="Static" CssClass="form-control vitaminPct" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </section>
                    <h2>11. Attachments</h2>
                    <section id="wizard-h-10">
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12 CompassLabel"><span class="comment-block">Supported file formats are doc, docx, txt, xls, xlsx, ppt, pptx, pdf, jpg, jpeg, png, bmp, gif.</span></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="head">&nbsp;</span></div>
                        </div>
                        <div class="row" id="dvFormulationDoc">
                            <div class="col-xs-12 col-sm-4 col-md-4">
                                <label class="control-label">Upload Formulation Documents:</label>
                            </div>
                            <div class="col-xs-12 col-sm-8 col-md-8">
                                <input id="btnUploadFormulations" type="button" class="ButtonControlAutoSize" value="Upload Formulation Documents" onclick="openAttachment('Formulation', 'Upload Formulation Documents'); return false;" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <table>
                                    <asp:Repeater ID="rpFormulationAttachments" runat="server" OnItemCommand="rpFormulationAttachments_ItemCommand">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>Action</th>
                                                <th>Document Name</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton runat="server" ID="lnkDeleteFormulationFile" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='javascript:return confirm("Are you sure you want to delete?")' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileUrl") %>'></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="head">&nbsp;</span></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-4 col-md-4">
                                <label class="control-label">Upload Graphics Documents:</label>
                            </div>
                            <div class="col-xs-12 col-sm-8 col-md-8">
                                <input id="btnUploadGraphics" type="button" class="ButtonControlAutoSize" value="Upload Graphics Documents" onclick="openAttachment('Graphics', 'Upload Graphics Documents'); return false;" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <table>
                                    <asp:Repeater ID="rpGraphicsAttachments" runat="server">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>Action</th>
                                                <th>Document Name</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lnkGraphicsFileDelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkGraphicsFileDelete_Click" CausesValidation="false"></asp:LinkButton>
                                                </td>
                                                <td>
                                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                        <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="LoadAttachments HiddenButton" OnClick="btnReloadAttachment_Click"></asp:Button>
                    </section>

                    <h2>12. FG BOM Details</h2>
                    <section id="wizard-h-11">
                        <asp:HiddenField ID="hdnIItemId" runat="server" ClientIDMode="Static" />
                        <asp:HiddenField ID="hdnProjectNumber" runat="server" ClientIDMode="Static" />

                        <asp:HiddenField ID="updateHdnSteps" runat="server" ClientIDMode="Static" Value="0" />
                        <asp:HiddenField ID="hdnPLMFlag" runat="server" ClientIDMode="Static" Value="" />
                        <asp:ListView ID="lstviewFGBOM" runat="server" ClientIDMode="Static" OnItemDataBound="lstviewFGBOM_ItemDataBound">
                            <ItemTemplate>
                                <asp:Panel runat="server" CssClass="bomrow SAPVerifyItem">
                                    <asp:HiddenField ID="hdnParentId" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnDeletedStatus" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnComponentType" runat="server" ClientIDMode="Static" />
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>Component Type:</label>
                                                <asp:DropDownList ID="drpComponent" runat="server" CssClass="PCBOMrequired form-control drpComponentType VerifySAPNumbersType" onchange="drpCompType_changed(this);">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" Value='<%# Eval("CompassListItemId") %>' />
                                                <asp:HiddenField ID="hdnItemID" runat="server" ClientIDMode="Static" Value='<%# Eval( "Id") %>' />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>New or Existing?:</label>
                                                <asp:DropDownList ID="drpNew" onchange="drpNew_changed(this.id, true);" runat="server" CssClass="PCBOMrequired drpNewClass form-control VerifySAPNewExisting">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                                    <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                                                    <asp:ListItem Text="Network Move" Value="Network Move"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>Base UOM Qty:</label>
                                                <asp:TextBox ID="txtPackQty" runat="server" CssClass="PCBOMrequired numericDecimal3 form-control"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span class="markrequired">*</span>UOM:</label>
                                                <asp:DropDownList ID="drpUnitOfMeasure" runat="server" CssClass="PCBOMrequired form-control">
                                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:Panel ID="lblCompNote" runat="server" Visible="false">
                                        <div class="col-xs-12 col-sm-12 col-md-12">
                                            <label class="comment-block">This component has been disabled because it has children, please delete or move all child components <strong>and save</strong> before changing the component type.</label>
                                        </div>
                                    </asp:Panel>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanFlowthrough" class="markrequired spanFlowthrough">*</span>Flowthrough:</label>
                                                <asp:DropDownList ID="ddlFlowthrough" runat="server" onchange="FlowthroughCheck(this);" CssClass="PCBOMrequired flowthroughClass form-control">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row hideableRow">
                                        <div class="col-xs-12 col-sm-4 col-md-4">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanGraphicsNeeded" runat="server" class="markrequired spanGraphicsNeeded">*</span>New Graphics Required?:</label>
                                                <asp:DropDownList ID="drpGraphicsNeeded" runat="server" onchange="GraphicsCheck(this);" CssClass="PCBOMrequired form-control drpGraphics">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-4 col-md-4">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanComponentContainsNLEA" runat="server" class="markrequired spanGraphicsNLEA">*</span>Component requires consumer facing labeling?:</label>
                                                <asp:DropDownList ID="drpComponentContainsNLEA" runat="server" CssClass="PCBOMrequired form-control drpGraphicsNLEA">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                                </asp:DropDownList>
                                                <label class="comment-block">Yes would mean this packaging material would contain nutritionals, a UPC, BE QR code, etc to enable sale to a consumer.</label>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-4 col-md-4">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanGraphicsVendor" runat="server" class="markrequired spanGraphicsVendor">*</span>Graphics Vendor:</label>
                                                <asp:DropDownList ID="ddlGraphicsVendor" runat="server" CssClass="PCBOMrequired form-control drpGraphicsVendor">
                                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanComponent" class="markrequired spanComponent">*</span>Component #:</label>
                                                <asp:TextBox ID="txtMaterial" runat="server" CssClass="PCBOMrequired alphanumericToUpper1 minimumlength form-control Component NumberClass VerifySAPNumbers" MaxLength="20" Text='<%# Eval("MaterialNumber") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-2">
                                            <div id="dvCompFindButton" class="form-group">
                                                <label class="control-label">&nbsp;</label>
                                                <asp:Button ID="btnLookupCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getRPTCompDescription(this);return false;" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-7">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanComponentDesc" class="markrequired spanComponentDesc">*</span>Component Description:</label>
                                                <asp:TextBox ID="txtMaterialDesc" runat="server" Style="text-transform: uppercase" CssClass="PCBOMrequired form-control ComponentDesc DescriptionClass" MaxLength="40" Text='<%# Eval( "MaterialDescription") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanLikeComponent" class="markrequired spanLikeComponent">*</span>Like Component #:</label>
                                                <asp:TextBox ID="txtLikeMaterial" runat="server" CssClass="PCBOMrequired alphanumericToUpper1 minimumlength form-control LikeMaterial NumberClass" MaxLength="20" Text='<%# Eval("CurrentLikeItem") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-2">
                                            <div id="dvLikeCompFindButton" class="form-group">
                                                <label class="control-label">&nbsp;</label>
                                                <asp:Button ID="btnLookupLikeCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getRPTCompDescription(this);return false;" CommandArgument='<%# Eval( "CurrentLikeItem") %>' />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-7">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanLikeComponentDesc" class="markrequired spanLikeComponentDesc">*</span>Like Component Description:</label>
                                                <asp:TextBox ID="txtLikeMaterialDesc" runat="server" CssClass="PCBOMrequired form-control LikeDescription DescriptionClass" Text='<%# Eval("CurrentLikeItemDescription") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-3">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanOldMaterial" class="markrequired spanOldMaterial hideItem">*</span>Old Component #:</label>
                                                <asp:TextBox ID="txtOldMaterial" runat="server" CssClass="alphanumericToUpper1 minimumlength NumberClass OldMaterial form-control" MaxLength="20" Text='<%# Eval( "CurrentOldItem") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-2">
                                            <div id="dvOldCompFindButton" class="form-group">
                                                <label class="control-label">&nbsp;</label>
                                                <asp:Button ID="btnLookupOldCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getRPTCompDescription(this);return false;" CommandArgument='<%# Eval( "CurrentOldItem") %>' />
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-7">
                                            <div class="form-group">
                                                <label class="control-label">Old Component Description:</label>
                                                <asp:TextBox ID="txtOldMaterialDesc" runat="server" CssClass="form-control OldDescription DescriptionClass" Text='<%# Eval( "CurrentOldItemDescription") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="hierarchyPanel2">
                                        <div class="row TSOnlyRow hideItem new">
                                            <div class="col-xs-12 col-sm-6 col-md-4">
                                                <div class="form-group">
                                                    <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 1:</label>
                                                    <%--<asp:DropDownList ID="ddlPHL1" CssClass="PCBOMrequired PHL1 form-control" OnSelectedIndexChanged="ddlProductHierarchyLevel1_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" AutoPostBack="True">--%>
                                                    <asp:DropDownList ID="ddlPHL1" CssClass="PCBOMrequired PHL1 form-control" onchange="BindPHL2DropDownItemsByPHL1(this);" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <p class="comment-block">
                                                        <asp:Label ID="lblPHL1" CssClass="comment-block" runat="server"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-xs-12 col-sm-6 col-md-4">
                                                <div class="form-group">
                                                    <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 2:</label>
                                                    <%--<asp:DropDownList ID="ddlPHL2" OnSelectedIndexChanged="ddlProductHierarchyLevel2_SelectedIndexChanged" AutoPostBack="True" CssClass="PCBOMrequired form-control" runat="server" AppendDataBoundItems="true">--%>
                                                    <asp:DropDownList ID="ddlPHL2" runat="server" CssClass="PCBOMrequired form-control PHL2" onchange="BindBrandDropDownItemsByPHL2(this);" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <p class="comment-block">
                                                        <asp:Label ID="lblPHL2" CssClass="comment-block" runat="server"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                            <div class="col-xs-12 col-sm-6 col-md-4">
                                                <div class="form-group">
                                                    <label class="control-label"><span class="markrequired">*</span>Material Group 1 (Brand):</label>
                                                    <%--<asp:DropDownList ID="ddlBrand" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" AutoPostBack="True" CssClass="PCBOMrequired form-control" runat="server" AppendDataBoundItems="true">--%>
                                                    <asp:DropDownList ID="ddlBrand" CssClass="PCBOMrequired form-control Brand" onchange="GetProfitCenter(this);" runat="server" AppendDataBoundItems="true">
                                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <p class="comment-block">
                                                        <asp:Label ID="lblBrand" CssClass="comment-block" runat="server"></asp:Label>
                                                    </p>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row TSOnlyRow hideItem new">
                                            <div class="col-xs-12 col-sm-6 col-md-6">
                                                <div class="form-group">
                                                    <label class="control-label"><span class="markrequired">*</span>Profit Center:</label>
                                                    <asp:TextBox ID="txtProfitCenterUC" runat="server" ReadOnly="true" class="PCBOMrequired form-control" ClientIDMode="Static"></asp:TextBox>
                                                    <asp:HiddenField ID="hdnProfitCenterUC" runat="server" ClientIDMode="Static" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row hideableRow">
                                        <div class="col-xs-12 col-sm-6 col-md-6">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanWhyComponent" runat="server" class="markrequired spanWhyComponent">*</span>How is it a Like Component #:</label>
                                                <asp:TextBox ID="txtLikeReason" TextMode="MultiLine" ToolTip="Does like component # have same dieline, graphics, etc." Rows="3" runat="server" CssClass="form-control whyLikeComponent" Text='<%# Eval("CurrentLikeItemReason") %>'></asp:TextBox>
                                                <label id="lblItemNote" class="comment-block">Does like component # have same dieline, graphics, etc.</label>
                                            </div>
                                        </div>
                                        <div class="col-xs-12 col-sm-6 col-md-6">
                                            <div class="form-group">
                                                <label class="control-label"><span id="spanGraphicsBrief" runat="server" class="markrequired spanGraphicsBrief">*</span>Graphics Brief:</label>
                                                <asp:TextBox ID="txtGraphicsBrief" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control GraphicsBrief" Text='<%# Eval("GraphicsBrief") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row TSOnlyRow hideItem">
                                        <div class="col-xs-12 col-sm-12 col-md-12">
                                            <div class="form-group">
                                                <label class="control-label">Transfer/Purchase Candy Semi Comments:</label>
                                                <asp:TextBox ID="txtTSComments" TextMode="MultiLine" ToolTip="Transfer/Purchase Candy Semi Comments" Rows="3" runat="server" CssClass="form-control TSComments" Text='<%# Eval("Notes") %>'></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-6 col-md-6">
                                            <div class="form-group">
                                                <label class="control-label">Move this component:</label>
                                                <asp:DropDownList ID="ddlMoveTS" runat="server" CssClass="ddlMoveTS form-control">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row hideableRow">
                                        <div class="col-xs-12 col-sm-12 col-md-4">
                                            <label class="control-label">Visual Reference / Rendering:</label>
                                        </div>
                                        <div class="col-xs-12 col-sm-12 col-md-5">
                                            <form role="form" class="form-inline">
                                                <div id="divAttachFile" runat="server">
                                                    <a id="ancAttachFile" href="#" title="edit" onclick="openBasicDialogIPF('/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=<%# Eval( "Id") %>&CompassItemId=<%# Eval( "CompassListItemId") %>&DocType=Rendering', 'Upload Rendering');return false;">
                                                        <img src="/_layouts/15/Ferrara.Compass/images/Attachtb.gif" id="btnAttachment" runat="server" alt="Attach File" /></a>

                                                </div>
                                                <asp:ImageButton ID="btnDeleteAttachment" CausesValidation="false" AlternateText="Delete Attachment" Visible="false" CommandName="DeleteAtt" CommandArgument='<%# Eval("Id") %>' ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" OnClientClick="DeleteVisualreference(this);return false;" runat="server" />
                                                <a target="_blank" id="ancRendering" runat="server"></a>
                                                <asp:HiddenField ID="DeletedVisualreferenceUrl" ClientIDMode="Static" runat="server" />
                                            </form>
                                        </div>
                                    </div>
                                    <div class="row ApprovedGraphicsAsset">
                                        <div class="col-xs-12 col-sm-12 col-md-4">
                                            <label class="control-label lblApprovedGraphicsAsset" id="lblApprovedGraphicsAsset" runat="server"><span class="markrequired spanGraphicsBrief">*</span>Approved Graphics Asset:</label>
                                        </div>
                                        <div class="col-xs-12 col-sm-12 col-md-5">
                                            <form role="form" class="form-inline">
                                                <div id="divAttachApprovedGraphicsAsset" runat="server">
                                                    <a id="ancAttachApprovedGraphicsAsset" href="#" title="edit" onclick="openBasicDialogIPF('/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=<%# Eval( "Id") %>&CompassItemId=<%# Eval( "CompassListItemId") %>&DocType=ApprovedGraphicsAsset', 'Upload Approved Graphics Asset');return false;">
                                                        <img src="/_layouts/15/Ferrara.Compass/images/Attachtb.gif" id="btnApprovedGraphicsAsset" runat="server" alt="Attach File" /></a>

                                                </div>
                                                <asp:ImageButton ID="btnDeleteApprovedGraphicsAsset" CausesValidation="false" AlternateText="Delete Attachment" Visible="false" CommandName="DeleteApprovedGraphicsAsset" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" OnClientClick="DeleteApprovedGraphicsAsset(this);return false;" runat="server" />
                                                <a target="_blank" id="ancApprovedGraphicsAsset" class="ancApprovedGraphicsAsset" runat="server"></a>
                                                <asp:HiddenField ID="DeletedApprovedGraphicsAssetUrl" ClientIDMode="Static" runat="server" />
                                            </form>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-12 col-sm-12 col-md-9">&nbsp;</div>
                                        <div class="col-xs-12 col-sm-12 col-md-3">
                                            <asp:Button ID="btndelete" CausesValidation="false" CssClass="ButtonControlAutoSize" CommandName="Delete" CommandArgument='<%# Eval("Id") %>' runat="server" Text="Delete Component" OnClientClick="deletePackagingItem(this);return false;" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </ItemTemplate>
                        </asp:ListView>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label">Please specify what materials, if any, need to flowthrough at the same time:</label>
                                    <label id="lblFlowthroughNote" class="comment-block">Note: If a material number is not yet assigned, please still reference the item by describing it</label>
                                    <asp:TextBox ID="txtFlowthroughDets" ClientIDMode="Static" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </section>

                    <h2>13. Summary</h2>
                    <section id="wizard-h-12">
                        <div class="row hidebutton">
                            <div class="col-xs-12 col-sm-6 col-md-2 hideItem hidebutton">
                                <asp:Button ID="btnLoadSummary" CssClass="Summary" ClientIDMode="Static" runat="server" Text="Load Summary" OnClick="btnLoadSummary_Click" />
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-10 hidebutton">
                                <asp:Label ID="lblLoadSummaryCompleted" CssClass="SuccessMessage" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div id="divPrint">
                            <div class="row">
                                <hr />
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <span class="headSummary">Proposed Project</span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <span class="LabelControl">Project Type:</span>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProjectType" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <span class="LabelControl" id="lblSummaryProjectTypeSubCategoryText">Project Type SubCategory</span>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProjectTypeSubCategory" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <span class="LabelControl">First Ship Date:</span>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryFirstShipDate" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="divCopyFormsForGraphicsProjectText">
                                    <div class="col-xs-12 col-sm-6 col-md-3">
                                        <span class="LabelControl" id="lblCopyFormsForGraphicsProjectText">Do you want to copy forms from previous project? :</span>
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-3">
                                        <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblCopyFormsForGraphicsProject" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row divExternalSemisItem">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Is this item external/contain external semis? :</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblExternalSemisItem" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-3 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryChangeNotesLabel" runat="server"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-9 col-md-9">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryChangeNotes" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Project Team</span></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Initiator:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryInitiator" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Project Leader (PL):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProjectLeader" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Project Manager (PM):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProjectManager" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Sr. Project Manager (Sr. PM):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummarySrProjectManager" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Marketing:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryMarketing" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">InTech:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryInTech" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Quality Innovation (QA):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryQualityInnovation" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">InTech Regulatory:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryInTechRegulatory" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Regulatory QA:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryRegulatoryQA" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Packaging Engineering (PE):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryPackagingEngineering" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Supply Chain (SC):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummarySupplyChain" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Finance:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryFinance" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Sales (SSCM):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummarySales" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Manufacturing:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryManufacturing" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">External Mfg - Procurement:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryExternalMfgProcurement" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Packaging Procurement:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryPackagingProcurement" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Life Cycle Management:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryLifeCycleManagement" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Other Team Members:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryOtherTeamMembers" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">SAP Item #</span></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Is a New FG Item # Being Used?:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryIndicator" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Finished Good Item #:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryFGItemNumber" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Finished Good Item Description:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryFGItemDesc" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">"Like" Finished Good Item #:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryLikeFGItemNumber" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">"Like" Finished Good Item Description:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryLikeFGItemDesc" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Old Finished Good Item #:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryOldFGItemNumber" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Old Finished Good Item Description:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryOldFGItemDesc" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Project Specifications</span></div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Organic?:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryOrganic" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNewFormulaLabel" runat="server" Text="New Base Formula?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNewFormula" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNewShapeLabel" runat="server" Text="New Shape?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNewShape" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNewFlavorColorLabel" runat="server" Text="New Flavor/Color?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNewFlavorColor" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNewNetWeightLabel" runat="server" Text="New Net Weight?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNewNetWeight" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryServingSizeLabel" runat="server" Text="Serving Size/Piece Weight Change:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryServingSize" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM HideGraphics">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Item Financial Details</span></div>
                        </div>
                        <div class="row HideForSNM HideGraphics">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryLast12MonthSalesLabel" runat="server" Text="Last 12 Months Sales:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryLast12MonthSales" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM HideGraphics">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Truckload Price per Retail Selling Unit:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryTruckLoadSellingPrice" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryExpectedGrossMarginPercentLabel" runat="server" Text="FCC Expected Gross Margin %:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryExpectedGrossMarginPercent" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM HideGraphics">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">12 Month Projected $:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryAnnualProjectedDollars" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Annual Projected Retail Selling Units:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryAnnualProjectedUnits" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM HideGraphics">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl"><span id="1stmonthsummary"></span>Projected $:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummary1stAnnualProjectedDollars" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl"><span id="1stmonthUsummary"></span>Projected Retail Units:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummary1stAnnualProjectedUnits" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM HideGraphics">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl"><span id="2ndmonthsummary"></span>Projected $:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummary2ndAnnualProjectedDollars" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl"><span id="2ndmonthUsummary"></span>Projected Retail Units:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummary2ndAnnualProjectedUnits" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM HideGraphics">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl"><span id="3rdmonthsummary"></span>Projected $:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummary3rdAnnualProjectedDollars" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl"><span id="3rdmonthUsummary"></span>Projected Retail Units:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummary3rdAnnualProjectedUnits" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Customer Specifications</span></div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Customer/Channel Specific:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryCustomerChannelSpecific" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryCustomerLabel" runat="server" Text="Customer:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryCustomer" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryCustomerSpecificLotCodeLabel" runat="server" Text="Customer/Specific Lot Code:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryCustomerSpecificLotCode" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryChannelLabel" runat="server" Text="Channel:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryChannel" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Sold outside of USA?:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryOutsideUSA" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryCountryOfSaleLabel" runat="server" Text="Country of Sale:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryCountryOfSale" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Item Hierarchy</span></div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Product Hierarchy Level 1:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProductHierarchyLevel1" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM" id="divSummaryManuallyCreateSAPDescription">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Manually Create SAP Description:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryManuallyCreateSAPDescription" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Product Hierarchy Level 2:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProductHierarchyLevel2" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Material Group 1 (Brand):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryMaterialGroup1Brand" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <label class="control-label">Profit Center:</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProfitCenter" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Material Group 4 (Product Form):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryMaterialGroup4ProductForm" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM" id="divSummaryProductFormDescription">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Product Form Description:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryProductFormDescription" Style="text-transform: uppercase;" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="HideForSNM" id="dvSummaryMixes">
                            <fieldset>
                                <span class="headSummary">Mix Details</span>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <table>
                                            <asp:Repeater ID="rpMixesSummary" runat="server" OnItemDataBound="rpMixesSummary_ItemDataBound">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th>Item # in Mix</th>
                                                        <th>Item Description</th>
                                                        <th>Total Pieces per Selling Unit</th>
                                                        <th>Ounces per Piece</th>
                                                        <th>Ounces per Selling Unit</th>
                                                        <th>Grams per Selling Unit</th>
                                                        <th>Qty for Mix</th>
                                                        <th>Lbs for FG BOM</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblMixItemNumber" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ItemNumber") %>'></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblMixItemDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "ItemDescription") %>'></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblNumberPieces" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "NumberOfPieces") %>'></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblOzPerPiece" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "OuncesPerPiece") %>'></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblOzPerSellingUnit" runat="server"></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblGramsPerSellingUnit" runat="server"></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblQtyMix" runat="server"></asp:Label></td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblLbsFGBOM" runat="server"></asp:Label></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <%--dvSummaryMixes--%>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Material Group 5 (Pack Type):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryMaterialGroup5PackType" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="HideForSNM" id="dvSummaryShipper">
                            <fieldset>
                                <span class="headSummary">Shipper Details</span>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-4">
                                        <span class="LabelControl">Total Quantity of Units in Display:</span>
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-2">
                                        <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryTotalQuantityofUnitsInDisplay" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <table>
                                            <asp:Repeater ID="rpShipperSummary" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                        <th>FG Item # in Display</th>
                                                        <th>FG Item Description</th>
                                                        <th># of Units</th>
                                                        <th>Ounces per Unit</th>
                                                        <th>Ounces per FG Unit</th>
                                                        <th>Pack Unit</th>
                                                    </tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblFGItemDisplay" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumber") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblFGItemDescription" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FGItemDescription") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblFGItemQuantity" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumberUnits") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblFGouncesPerUnit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit") %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblFGouncesPerFGunit" runat="server" Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "FGItemNumberUnits")) * Convert.ToDouble(DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit")) %>'></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label CssClass="summary" ID="lblFGpackUnit" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FGPackUnit") %>'></asp:Label>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <%--dvSummaryShipper--%>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <label class="control-label">Is this project considered to be Novelty?:</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNoveltyProject" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Item UPCs</span></div>
                        </div>
                        <div class="row" style="height: 60px">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Do we need any new UPCs (12 digit GTIN) or UCCs (14 digit GTIN)?:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNeedNewUPCUCC" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="height: 60px">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNeedNewUnitUPCLabel" runat="server" Text="Do we need a New Consumer Unit UPC (12 digit GTIN) – EACH?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNeedNewUnitUPC" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryUnitUPCLabel" runat="server" Text="Existing Consumer Unit UPC (12 digit GTIN) – EACH:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryUnitUPC" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="height: 80px">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNeedNewDisplayBoxUPCLabel" runat="server" Text="Do we need a New Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNeedNewDisplayBoxUPC" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryDisplayUPCBoxLabel" runat="server" Text="Existing Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryDisplayUPCBox" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="height: 60px">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummarySAPBaseUOMLabel" runat="server" Text="SAP Base UOM:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummarySAPBaseUOM" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="height: 60px">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNeedNewCaseUCCLabel" runat="server" Text="Do we need a new Case UCC (14 digit GTIN)?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNeedNewCaseUCC" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryCaseUCCLabel" runat="server" Text="Existing Case UCC (14 digit GTIN):"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryCaseUCC" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row" style="height: 60px">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryNeedNewPalletUCCLabel" runat="server" Text="Do we need a new Pallet UCC (14 digit GTIN)?:"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNeedNewPalletUCC" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryPalletUCCLabel" runat="server" Text="Existing Pallet UCC (14 digit GTIN):"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryPalletUCC" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Item Details</span></div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Case Type:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryCaseType" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Peg Hole Needed:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryPegHoleNeeded" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Film Substrate:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryFilmSubstrate" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3" id="divlblforSummaryInvolvesCarton">
                                <span class="LabelControl">Does this project involve a carton/mixed bag/tray or other product form that goes within a case but also contains individual units within it?:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3" id="divlblSummaryInvolvesCarton">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryInvolvesCarton" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM" id="divSummaryUnitsInsideCarton">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Number of Units Inside of Carton/Mixed Bag/Tray/Etc:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryUnitsInsideCarton" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Individual Pouch Weight (oz)::</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryIndividualPouchWeight" runat="server" Text=""></asp:Label>
                            </div>
                        </div>

                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4" id="divSummaryNumberofTraysPerBaseUOM1">
                                <span class="LabelControl">Number of Trays Per Base UOM:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2" id="divSummaryNumberofTraysPerBaseUOM2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryNumberofTraysPerBaseUOM" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Retail Selling Units Per Base U of M:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryRetailSellingUnitsPerBaseUOM" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Retail Unit Weight (oz):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryRetailUnitWeight" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Base UOM Net Weight (lbs):</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryBaseUOMNetWeight" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Marketing Claims</span></div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Marketing Claims/Labeling Requirements:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryClaimsLabelingRequirements" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <span class="LabelControl">Are there any desired claims?:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-8">
                                <asp:Label CssClass="summary" ClientIDMode="Static" ID="lblSummaryClaimsDesired" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3 marketingClaims">
                                <span class="LabelControl">Sellable Unit:</span>
                                <asp:Label ID="lblSummarySellableUnit" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">New NLEA Format:</span>
                                <asp:Label ID="lblSummaryNewNLEAFormat" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>

                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-12">
                                <span class="LabelControl">Is Bio-Engineering Labeling Acceptable for this item?:</span>
                                <asp:Label ID="lblSummaryBioEngineeringLabelingAcceptable" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row existingClaims HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-4 ">
                                <span class="LabelControl">Finished Good / Component #:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2 ">
                                <asp:Label ID="lblSummaryFinishedGoodComponent" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Finished Good / Component Description:</span>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label ID="lblSummaryFinishedGoodComponentDescription" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row marketingClaims HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Made In USA Claim:</span>
                                <asp:Label ID="lblSummaryMadeInUSA" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <asp:Label CssClass="LabelControl" ClientIDMode="Static" ID="lblSummaryMadeInUSAPctLabel" runat="server" Text="Made In USA Claim Details:"></asp:Label>
                                <asp:Label ID="lblSummaryMadeInUSAPct" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">GMO Claim:</span>
                                <asp:Label ID="lblSummaryGMOClaim" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row marketingClaims HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Gluten Free:</span>
                                <asp:Label ID="lblSummaryGlutenFree" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Fat Free:</span>
                                <asp:Label ID="lblSummaryFatFree" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Kosher:</span>
                                <asp:Label ID="lblSummaryKosher" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row marketingClaims HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Organic:</span>
                                <asp:Label ID="lblSummaryMarketingOrganic" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Natural Colors:</span>
                                <asp:Label ID="lblSummaryNaturalColors" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Natural / Real Flavors Claims:</span>
                                <asp:Label ID="lblSummaryNaturalFlavors" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row marketingClaims HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Preservative Free:</span>
                                <asp:Label ID="lblSummaryPreservativeFree" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Lactose Free:</span>
                                <asp:Label ID="lblSummaryLactoseFree" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <span class="LabelControl">Low Sodium:</span>
                                <asp:Label ID="lblSummaryLowSodium" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row marketingClaims HideForSNM">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <span class="LabelControl">Juice Concentrate:</span>
                                <asp:Label ID="lblSummaryJuiceConcentrate" ClientIDMode="Static" runat="server" CssClass="summary" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row marketingClaims HideForSNM">
                            <div class="col-xs-12 col-sm-6 col-md-12">
                                <span class="LabelControl">Good / Excellent Source:</span>
                                <asp:Label ID="lblSummaryGoodSource" ClientIDMode="Static" CssClass="summary" runat="server" Text=""></asp:Label>
                            </div>
                        </div>
                        <div class="row HideForSNM">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Attachments</span></div>
                        </div>
                        <div class="row HideForSNM">
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <table>
                                    <asp:Repeater ID="rpSummaryFormulationAttachments" runat="server">
                                        <HeaderTemplate>
                                            <tr>
                                                <th><span class="LabelControl">Formulation Attachments</span></th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <table>
                                    <asp:Repeater ID="rpSummaryGraphicsAttachments" runat="server">
                                        <HeaderTemplate>
                                            <tr>
                                                <th><span class="LabelControl">Graphics Attachments</span></th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                            </div>
                        </div>
                        <div class="row" id="dvPackagingComponents" runat="server">
                            <hr />
                            <div class="col-xs-12 col-sm-12 col-md-12"><span class="headSummary">Packaging Components:</span></div>
                        </div>
                        <div class="row ">
                            <asp:Panel ID="commercializationPanel" runat="server" ClientIDMode="Static"></asp:Panel>
                        </div>
                        <div class="row" id="rowChangeReason">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired" id="reqChangeReason">*</span>Reason for change?:</label>
                                    <asp:TextBox ID="txtChangeReason" ClientIDMode="Static" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:HiddenField ID="hdnIsChangeRequest" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                        <div class="row hidebutton">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div>
                                    <asp:Button ID="btnSubmit" CssClass="finish ButtonControl HiddenButton" CausesValidation="false" runat="server" Text="Submit" OnClientClick="return ValidateIPFData();" OnClick="btnSubmit_Click" />
                                </div>
                            </div>
                        </div>
                    </section>
                </div>
                <div class="actions clearfix"></div>
                <div class="row hidebutton">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <div class="col-xs-12 col-sm-6 col-md-10">
                                <asp:Label ID="lblSaved" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <asp:Button ID="btnSave" runat="server" CssClass="clickSave button justifyRight" Text="Save" OnClientClick="return preSaveUpdates();" OnClick="btnSave_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="row hidebutton">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <asp:Button ID="btnNext" runat="server" CssClass="ButtonControl justifyRight" Text="Next" OnClick="btnNext_Click" OnClientClick="javascript:showWaitPopup('Creating Project...', 'Please be patient, this may take a few seconds...');" />
                    </div>
                </div>
                <div class="row hidebutton">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--Container--%>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        function RemoveRougeChar(convertString) {
            if (convertString.substring(0, 1) == ",") {
                return convertString.substring(1, convertString.length)
            }
            return convertString;
        }
        drpCompType_load();
        ShowHideApprovedGraphicsAsset();
        $("#FGSection .tsButtonLink").click();
    });
    function openAttachment(docType, title) {
        var itemId = $("#hiddenItemId").val();
        var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=0&DocType=' + docType + '&CompassItemId=' + itemId;
        openAttachmentsDialog(url, title);
        return false;
    }
    function drpNew_changed(drpNewId, copyValues) {
        BOMNewCondition(drpNewId, copyValues);
        ShowHideApprovedGraphicsAsset();
    }
    function drpCompType_changed(arg) {
        var anchor = $("#" + arg.id);
        var parentRow = anchor.closest(".bomrow");
        var newExisting = parentRow.find(".drpNewClass option:selected").text();
        var compType = anchor.find("option:selected").text().toLocaleLowerCase();
        if (compType == "candy semi" || compType == "transfer semi" || compType == "purchased candy semi") {
            parentRow.find(".hideableRow").addClass("hideItem");
            parentRow.find(".hideableRow .col-xs-12").addClass("hideItem");

            parentRow.find(".drpGraphics").val("-1");
            parentRow.find(".drpComponentContainsNLEA").val("-1");
            parentRow.find(".drpGraphicsVendor").val("-1");

            if (compType == "transfer semi" || compType == "purchased candy semi") {
                parentRow.find(".TSOnlyRow").removeClass("hideItem");
                parentRow.find(".TSOnlyRow .col-xs-12").removeClass("hideItem");
                parentRow.find(".TSOnlyRow .switch").html(anchor.find("option:selected").text());
                if (!parentRow.find(".drpNewClass option:eq(3)").length) {
                    parentRow.find(".drpNewClass").append('<option value="Network Move">Network Move</option>');
                }
                if (newExisting != "New") {
                    parentRow.find(".TSOnlyRow.new").addClass("hideItem");
                } else {
                    parentRow.find(".TSOnlyRow.new").removeClass("hideItem");
                }
            } else {
                parentRow.find(".TSOnlyRow").addClass("hideItem");
                parentRow.find(".TSOnlyRow .col-xs-12").addClass("hideItem");
                parentRow.find(".drpNewClass option:eq(3)").remove();
            }

        } else {
            parentRow.find(".hideableRow").removeClass("hideItem");
            parentRow.find(".hideableRow .col-xs-12").removeClass("hideItem");
            parentRow.find(".TSOnlyRow").addClass("hideItem");
            parentRow.find(".TSOnlyRow .col-xs-12").addClass("hideItem");
            parentRow.find(".drpNewClass option:eq(3)").remove();
        }

        ShowHideApprovedGraphicsAsset();
    }
    function BOMNewCondition(drpNewId, copyValues) {
        var anchor = $("#" + drpNewId);
        var parentRow = anchor.closest(".bomrow");
        var idGraphics = parentRow.find(".drpGraphics:eq(0)").attr("id");
        var idFlowthrough = parentRow.find(".flowthroughClass:eq(0)").attr("id");
        var idCompType = parentRow.find(".drpComponentType:eq(0)").attr("id");

        var idComponent = parentRow.find(".Component:eq(0)").attr("id");
        var idComponentDesc = parentRow.find(".ComponentDesc:eq(0)").attr("id");

        var idLikeMaterial = parentRow.find(".LikeMaterial:eq(0)").attr("id");
        var idLikeDescription = parentRow.find(".LikeDescription:eq(0)").attr("id");

        var idOldMaterial = parentRow.find(".OldMaterial:eq(0)").attr("id");
        var idOldDescription = parentRow.find(".OldDescription:eq(0)").attr("id");

        var spanGraphicsNeeded = parentRow.find(".spanGraphicsNeeded:eq(0)");
        var spanGraphicsVendor = parentRow.find(".spanGraphicsVendor:eq(0)");
        var idwhyLikeComponent = parentRow.find(".whyLikeComponent:eq(0)").attr("id");
        var spanWhyComponent = parentRow.find(".spanWhyComponent:eq(0)");

        var spanComponent = parentRow.find(".spanComponent:eq(0)");
        var spanComponentDesc = parentRow.find(".spanComponentDesc:eq(0)");
        var spanLikeComponent = parentRow.find(".spanLikeComponent:eq(0)");
        var spanLikeComponentDesc = parentRow.find(".spanLikeComponentDesc:eq(0)");
        var spanFlowthrough = parentRow.find(".spanFlowthrough:eq(0)");
        var spanOldMaterial = parentRow.find(".spanOldMaterial:eq(0)");

        var componentVal = $("#" + idComponent).val().toLocaleLowerCase();
        var likeComponentVal = $("#" + idLikeMaterial).val().toLocaleLowerCase();
        var oldComponentVal = $("#" + idOldMaterial).val().toLocaleLowerCase();
        var componentDescVal = $("#" + idComponentDesc).val().toLocaleLowerCase();
        var likeComponentDescVal = $("#" + idLikeDescription).val().toLocaleLowerCase();
        var oldComponentDescVal = $("#" + idOldDescription).val().toLocaleLowerCase();

        //following checks are required if packaging item is new
        if ($("#" + drpNewId + " option:selected").text().toLowerCase() == 'new') {
            $("#" + idGraphics).addClass("PCBOMrequired");
            $("#" + idwhyLikeComponent).addClass("PCBOMrequired");
            $("#" + idComponent).prop("readonly", true);
            $("#" + idComponentDesc).prop("readonly", true);
            $("#" + idLikeMaterial).prop("readonly", false);
            $("#" + idLikeDescription).prop("readonly", false);
            $("#" + idOldMaterial).prop("readonly", false);
            $("#" + idOldDescription).prop("readonly", false);
            $("#" + idwhyLikeComponent).prop("readonly", false);

            spanLikeComponent.show();
            spanLikeComponentDesc.show();
            spanComponent.hide();
            spanComponentDesc.hide();
            spanGraphicsNeeded.show();
            spanWhyComponent.addClass('showItempc').removeClass('hideItem');

            if (copyValues) {
                if ((likeComponentVal == "" || likeComponentVal == "not applicable") && componentVal != "needs new") {
                    $("#" + idLikeMaterial).val(componentVal);
                }
                if ((likeComponentDescVal == "" || likeComponentDescVal == "not applicable") && componentDescVal != "needs new") {
                    $("#" + idLikeDescription).val(componentDescVal);
                }
                $("#" + idComponent).val("Needs New");
                $("#" + idComponentDesc).val("Needs New");
            }
            if ((likeComponentVal == "" || likeComponentVal == "not applicable") && componentVal != "needs new") {
                $("#" + idLikeMaterial).val(componentVal);
            }
            if (oldComponentVal == "not applicable") {
                $("#" + idOldMaterial).val("");
            }
            if (oldComponentDescVal == "not applicable") {
                $("#" + idOldDescription).val("");
            }
            $("#" + idFlowthrough).prop("disabled", false);
            if ($("#hdnTBDIndicator").val() == "No") {
                spanOldMaterial.addClass("hideItem");
                $("#" + idOldMaterial).removeClass("PCBOMrequired");
            } else if ($("#hdnTBDIndicator").val() == "Yes") {
                if ($("#" + idFlowthrough + " option:selected").text() == "Soft") {
                    spanOldMaterial.removeClass("hideItem");
                    $("#" + idOldMaterial).addClass("PCBOMrequired");
                } else {
                    spanOldMaterial.addClass("hideItem");
                    $("#" + idOldMaterial).removeClass("PCBOMrequired");
                }
            } else {
                spanOldMaterial.addClass("hideItem");
                $("#" + idOldMaterial).removeClass("PCBOMrequired");
            }
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").removeClass("hideItem");
            }
        }
        else if ($("#" + drpNewId + " option:selected").text().toLowerCase() == 'existing') {
            $("#" + idGraphics).addClass("PCBOMrequired");
            $("#" + idwhyLikeComponent).addClass("PCBOMrequired");
            $("#" + idLikeMaterial).prop("readonly", true);
            $("#" + idLikeDescription).prop("readonly", true);
            $("#" + idOldMaterial).prop("readonly", true);
            $("#" + idOldDescription).prop("readonly", true);
            $("#" + idwhyLikeComponent).prop("readonly", true);

            spanLikeComponent.hide();
            spanLikeComponentDesc.hide();
            $("#" + idComponent).prop("readonly", false);
            $("#" + idComponentDesc).prop("readonly", false);
            spanComponent.show();
            spanComponentDesc.show();
            spanGraphicsNeeded.show();
            spanWhyComponent.removeClass('showItempc').addClass('hideItem');
            $("#" + idOldMaterial).val("Not Applicable");
            $("#" + idOldDescription).val("Not Applicable");

            if (copyValues) {
                if ((componentVal == "" || componentVal == "needs new") && likeComponentVal != "not applicable") {
                    $("#" + idComponent).val(likeComponentVal);
                }
                if ((componentDescVal == "" || componentDescVal == "needs new") && likeComponentDescVal != "not applicable") {
                    $("#" + idComponentDesc).val(likeComponentDescVal);
                }
                $("#" + idLikeMaterial).val("Not Applicable");
                $("#" + idLikeDescription).val("Not Applicable");
                $("#" + idwhyLikeComponent).val("Not Applicable");
            }
            //$("#" + idFlowthrough).prop("disabled", true);
            if ($("#" + idFlowthrough).val() == "-1") {
                $("#" + idFlowthrough).val("3").change();
            }
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").addClass("hideItem");
            }
        } else if ($("#" + drpNewId + " option:selected").text().toLowerCase() == 'network move') {
            $("#" + idLikeMaterial).removeClass("PCBOMrequired");
            $("#" + idLikeDescription).removeClass("PCBOMrequired");
            spanGraphicsNeeded.hide();
            spanWhyComponent.addClass('hideItem').removeClass('showItempc');
            $("#" + idLikeMaterial).prop("readonly", false);
            $("#" + idLikeDescription).prop("readonly", false);
            $("#" + idOldMaterial).prop("readonly", false);
            $("#" + idOldDescription).prop("readonly", false);
            $("#" + idwhyLikeComponent).prop("readonly", false);
            if (oldComponentVal == "not applicable") {
                $("#" + idOldMaterial).val("");
            }
            if (oldComponentDescVal == "not applicable") {
                $("#" + idOldDescription).val("");
            }

            spanComponent.show();
            spanComponentDesc.show();
            spanLikeComponent.hide();
            spanLikeComponentDesc.hide();

            $("#" + idFlowthrough).val("3").change();
            $("#" + idFlowthrough).prop("disabled", false);
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").addClass("hideItem");
            }
        }
        else {
            $("#" + idGraphics).removeClass("PCBOMrequired");
            $("#" + idwhyLikeComponent).removeClass("PCBOMrequired");
            spanGraphicsNeeded.hide();
            spanWhyComponent.addClass('hideItem').removeClass('showItempc');
            $("#" + idLikeMaterial).prop("readonly", false);
            $("#" + idLikeDescription).prop("readonly", false);
            $("#" + idOldMaterial).prop("readonly", false);
            $("#" + idOldDescription).prop("readonly", false);
            $("#" + idwhyLikeComponent).prop("readonly", false);
            if (oldComponentVal == "not applicable") {
                $("#" + idOldMaterial).val("");
            }
            if (oldComponentDescVal == "not applicable") {
                $("#" + idOldDescription).val("");
            }

            spanComponent.show();
            spanComponentDesc.show();
            spanLikeComponent.show();
            spanLikeComponentDesc.show();
            $("#" + idFlowthrough).prop("disabled", false);
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").addClass("hideItem");
            }
        }
    }
    function FlowthroughCheck(arg) {
        var anchor = $("#" + arg.id);
        var selectedFlowthrough = $("#" + arg.id + " option:selected").text().toLowerCase()
        var spanOldMaterial = anchor.closest(".bomrow").find(".spanOldMaterial:eq(0)");
        var idOldMaterial = anchor.closest(".bomrow").find(".OldMaterial:eq(0)").attr("id");
        if (selectedFlowthrough == 'soft' && anchor.closest(".bomrow").find(".drpNewClass:eq(0)").val() == "New" && $("#ddlTBDIndicator option:selected").text() == "Yes") {
            spanOldMaterial.removeClass("hideItem");
            $("#" + idOldMaterial).addClass("PCBOMrequired");
        } else {
            spanOldMaterial.addClass("hideItem");
            $("#" + idOldMaterial).removeClass("PCBOMrequired");
        }
    }
    function GraphicsCheck(arg) {
        var anchor = $("#" + arg.id);
        var idGraphicsBrief = anchor.closest(".bomrow").find(".GraphicsBrief");
        var spanGraphicsBrief = anchor.closest(".bomrow").find(".spanGraphicsBrief");
        var idGraphicsVendor = anchor.closest(".bomrow").find(".drpGraphicsVendor");
        var spanGraphicsVendor = anchor.closest(".bomrow").find(".spanGraphicsVendor");

        if ($("#" + arg.id + " option:selected").text() == 'Yes') {
            idGraphicsBrief.addClass("PCBOMrequired");
            spanGraphicsBrief.addClass('showItempc').removeClass('hideItem');
            idGraphicsVendor.addClass("PCBOMrequired");
            spanGraphicsVendor.addClass('showItempc').removeClass('hideItem');
            $('.showmarkGB').show();
        }
        else {
            idGraphicsBrief.removeClass("PCBOMrequired");
            spanGraphicsBrief.addClass('hideItem').removeClass('showItempc');
            idGraphicsVendor.removeClass("PCBOMrequired");
            spanGraphicsVendor.addClass('hideItem').removeClass('showItempc');
            $('.showmarkGB').hide();
        }

        ShowHideApprovedGraphicsAsset();
    }
    function SAPNomenclature() {
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
                        //var ProjectTitle = $('#lblProjectTitle').text();
                        //var ProjectTitleArray = ProjectTitle.split(":");
                        //$('#lblProjectTitle').text(ProjectTitleArray[0] + " : " + ProjectTitleArray[1] + " : " + $("#txtSAPItemDescription").val());
                    }
                    else {
                        BuildSAPDescription = true;
                        $("#txtSAPItemDescription").prop('readonly', true);
                    }
                }
                else {
                    $("#ddlManuallyCreateSAPDescription").val("No");
                    hideItem('divManuallyCreateSAPDescription');
                    BuildSAPDescription = false;
                }
            }
            else {
                $("#ddlManuallyCreateSAPDescription").val("No");
                hideItem('divManuallyCreateSAPDescription');
                BuildSAPDescription = false;
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

                if (SeasonSelection == 'VALENTINE\'S (000000008)' || SeasonSelection == 'VALENTINE\'S (000000008)') {
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
                if (NumberOfUnitsInsideOfCarton == 0) {
                    NumberOfUnitsInsideOfCarton = '';
                } else if (NumberOfUnitsInsideOfCarton != '') {
                    NumberOfUnitsInsideOfCarton = NumberOfUnitsInsideOfCarton + '/';
                }
            } else {
                $("#txtUnitsInsideCarton").val('');
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
                } else if (ddlMaterialGroup5.indexOf("(SHP)") != -1) {
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
            //var ProjectTitle = $('#lblProjectTitle').text();
            //var ProjectTitleArray = ProjectTitle.split(":");
            //$('#lblProjectTitle').text(ProjectTitleArray[0] + " : " + ProjectTitleArray[1] + " : " + ProposedItem);

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
</script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/IPF.js?v=138"></script>
<script>
    function updateJavascriptStyles() {
        $(".drpNewClass").each(function (index, drpNew) {
            if ($(this).hasClass("PCBOMrequired")) {
                BOMNewCondition($(drpNew).attr('id'), false);
            }
        });
        defineFormats();
    }
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(updateJavascriptStyles);
    updateJavascriptStyles();
    function callSAPNomenclature() {
        SAPNomenclature();
        BindHierarchiesOnLoad();
    }
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callSAPNomenclature);
    callSAPNomenclature();
</script>
