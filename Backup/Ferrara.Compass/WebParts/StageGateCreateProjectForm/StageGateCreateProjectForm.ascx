<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StageGateCreateProjectForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.StageGateCreateProjectForm.StageGateCreateProjectForm" %>
<div class="container">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled!
            <br />
            If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row PageSubHeader" style="margin-top: -40px;">
        <h1>
            <asp:Label CssClass="control-label" ID="lblPageTitle" runat="server"></asp:Label>
        </h1>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Please correct the errors below:" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 comment-highlighted">
            <label class="control-label">Project Description: </label>
            <label id="labProjectDescription"></label>
            <div id="divProjectDescription"></div>
            Please reference <a href='/Shared%20Documents/Project%20Types%20and%20Definitions.xlsx'>Project Types and Definitions</a> for more details. 
        </div>
        <div class="col-xs-12 col-sm-12 col-md-12 comment-highlighted" style="padding-top: 15px;">
            <a href='/Shared%20Documents/Hierarchy%20and%20Brand%20PMT%20Materials.pptx'>Hierarchy/Brand Guidance</a>
        </div>
    </div>
    <div class="row">
        <!-- Project Created Modal -->
        <div class="modal fade" id="DialogStageGateProjectCreatedMessage" role="dialog" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                        <h4 class="modal-title">Stage Gate Project</h4>
                    </div>
                    <div class="modal-body" style="display: flex;">
                        <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                        <p style="margin-left: 10px;">
                            <h2>Stage Gate Project
                                <label id="lblProjectCreatedMessage"></label>
                                Submitted Successfully.</h2>
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal" onclick="StageGateProjectCreatedRedirect()">Ok</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- Project Re-Submitted Modal -->
        <div class="modal fade" id="DialogStageGateProjectResubmitMessage" role="dialog" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                        <h4 class="modal-title">Stage Gate Project Re-Submission</h4>
                    </div>
                    <div class="modal-body" style="display: flex;">
                        <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                        <p style="margin-left: 10px;">
                            This form has already been submitted on the dates shown below.<br />
                            <label id="lblProjectSubmittedDate"></label>
                            <br />
                            <br />
                            Resubmitting this form will record the Gate 0 Date to inidicate it was approved to move onto the next phase.
                            <br />
                            <br />
                            Are you sure you’d like to Resubmit?
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnResubmitClose" class="btn btn-default" data-dismiss="modal" onclick="DialogStageGateProjectResubmitMessageCancel()">No</button>
                        <button type="button" id="btnResubmitOk" class="btn btn-default" data-dismiss="modal" onclick="DialogStageGateProjectResubmitMessageOK()">Yes</button>
                    </div>
                </div>
            </div>
        </div>

    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Project Information</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvCaseType" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Project Name:</label>
                    <asp:TextBox ID="txtProjectName" ClientIDMode="Static" runat="server" CssClass="required form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div id="dvFilmSubstrate" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Gate 0 Approved Date:</label>
                    <asp:TextBox ID="txtGate0ApprovedDate" ClientIDMode="Static" runat="server" CssClass="SNWChangeRequiredOptional datePicker required form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="hierarchyPanel" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlLineOfBusiness" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlProductHierarchyLevel2" EventName="SelectedIndexChanged" />
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
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Line Of Business:</label>
                            <asp:DropDownList ID="ddlLineOfBusiness" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlProductHierarchyLevel1_SelectedIndexChanged">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Project Tier/priority (A,B,C):</label>
                            <asp:DropDownList ID="ddlProjectTier" ClientIDMode="Static" CssClass="SNWChangeRequiredOptional required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                <asp:ListItem Text="A" Value="A"></asp:ListItem>
                                <asp:ListItem Text="B" Value="B"></asp:ListItem>
                                <asp:ListItem Text="C" Value="C"></asp:ListItem>
                                <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Number of Novelty SKUs:</label>
                            <asp:TextBox ID="txtNumberofNoveltySKUs" ClientIDMode="Static" CssClass="SNWChangeRequiredOptional required form-control StageGateNumbersNA" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Desired 1st Ship Date:</label>
                            <asp:TextBox ID="txtDesiredShipDate" ClientIDMode="Static" runat="server" CssClass="datePicker required form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span># SKUs:</label>
                            <asp:TextBox ID="txtSKU" runat="server" ClientIDMode="Static" CssClass="required form-control StageGateNumbers"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 2:</label>
                            <asp:DropDownList ID="ddlProductHierarchyLevel2" OnSelectedIndexChanged="ddlProductHierarchyLevel2_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                            <p class="comment-block">
                                <asp:Label ID="lblProductHierarchyLevel2" CssClass="comment-block" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Brand:</label>
                            <asp:DropDownList ID="ddlBrand" runat="server" CssClass="Brand required form-control" ClientIDMode="Static" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" ClientIDMode="Static">
                <ContentTemplate>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Project Type:</label>
                            <asp:DropDownList ID="ddlProjectType" OnSelectedIndexChanged="ddlProjectType_SelectedIndexChanged" onchange="StageGateProjectTypeChange();" AutoPostBack="true" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Project Type SubCategory:</label>
                            <asp:DropDownList ID="ddlProjectTypeSubCategory" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Function Initiating Project:</label>
                            <asp:DropDownList ID="ddlBusinessFunction" onchange="BusinessFunctionChanged();" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group BusinessFunctionOther">
                            <label class="control-label"><span class="markrequired">*</span>Other:</label>
                            <asp:TextBox ID="txtBusinessFunctionOther" ClientIDMode="Static" runat="server" CssClass="required form-control"></asp:TextBox>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>New Finished Good:</label>
                    <asp:DropDownList ID="ddlNewFinishedGood" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Multiple" Value="Multiple"></asp:ListItem>
                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnddlNewFinishedGood" runat="server" ClientIDMode="Static" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>New Base Formula:</label>
                    <asp:DropDownList ID="ddlNewBaseFormula" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnddlNewBaseFormula" runat="server" ClientIDMode="Static" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>New Shape:</label>
                    <asp:DropDownList ID="ddlNewShape" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnddlNewShape" runat="server" ClientIDMode="Static" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>New Flavor/Color:</label>
                    <asp:DropDownList ID="ddlNewFlavorColor" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnddlNewFlavorColor" runat="server" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>New Pack Type:</label>
                    <asp:DropDownList ID="ddlNewPackType" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnddlNewPackType" runat="server" ClientIDMode="Static" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>New Net Weight:</label>
                    <asp:DropDownList ID="ddlNewNetWeight" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnddlNewNetWeight" runat="server" ClientIDMode="Static" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>New Graphics:</label>
                    <asp:DropDownList ID="ddlNewGraphics" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnddlNewGraphics" runat="server" ClientIDMode="Static" />
                </div>
            </div>
        </div>
        <div class="row">
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Project Concept Overview:</label>
                    <asp:TextBox ID="txtProjectConceptOverview" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="6" CssClass="required form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Stage</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Stage:</label>
                    <asp:DropDownList ID="ddlStage" Enabled="false" ClientIDMode="Static" CssClass="required form-control stageGateDropDownReadOnly" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Project Team</h2>
            <div class="comment-block" id="div1" runat="server">*If Team Not applicable type NA.*</div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div>
        <asp:UpdatePanel ID="TeamPanel" runat="server">
            <ContentTemplate>
                <div class="panel-body">
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
                                                <td class="deleteRow">
                                                    <div class="form-group">
                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -16px;" onClick="deleteRow(this,'hdnDeletedStatusForProjectLeader');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
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
                                <div runat="server" id="div2">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlMarketingMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div7">
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
                                                        <asp:TextBox ID="txtMarketingMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers txtMarketingMembers" Style="width: 95%"></asp:TextBox>
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
                                <div runat="server" id="div8">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlInTechMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div9">
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
                                <div runat="server" id="div10">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlQualityInnovationMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div11">
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
                                <div runat="server" id="div12">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlInTechRegulatoryMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div13">
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
                                <div runat="server" id="div14">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlRegulatoryQAMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div15">
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
                                <div runat="server" id="div16">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlPackagingEngineeringMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div17">
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
                                <div runat="server" id="div18">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlSupplyChainMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div21">
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
                                <div runat="server" id="div22">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlFinanceMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div31">
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
                                <div runat="server" id="div32">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlSalesMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div33">
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
                        <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divLegalMembers">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired SNWChangeRequiredOptionalPeopleEditor RenQualWChangeRequiredOptionalPeopleEditor">*</span>Legal:</label>
                            </div>
                            <div>
                                <div runat="server" id="div19">
                                    <div class="form-group">
                                        <asp:DropDownList ID="ddlLegalMembers" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div runat="server" id="div20">
                                    <div class="form-group">
                                        <asp:Image ID="ImgAddLegalMembers" class="AddMember" onClick="AddProjectTeamMembersRow_New(this, 'btnAddLegalMembers')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                                    </div>
                                </div>
                            </div>
                            <div>
                                <table class="MembersTableNew" style="width: 100%">
                                    <asp:Repeater ID="rptLegalMembers" runat="server" ClientIDMode="Static">
                                        <ItemTemplate>
                                            <tr>
                                                <td runat="server" id="tdDeliverable">
                                                    <div class="form-group">
                                                        <asp:TextBox ID="txtLegalMembersLoginName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                                        <asp:TextBox ID="txtLegalMembers" Value='<%# DataBinder.Eval(Container.DataItem, "MemberName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                                    </div>
                                                </td>
                                                <td class="DeleteRow">
                                                    <div class="form-group">
                                                        <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForLegalMembers');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                        <asp:HiddenField ID="hdnDeletedStatusForLegalMembers" Value="false" runat="server" ClientIDMode="Static" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </table>
                                <asp:Button ID="btnAddLegalMembers" OnClick="btnAddLegalMembers_Click" ClientIDMode="Static" runat="server" Text="Add Team Member" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                                <asp:HiddenField ID="hdnRequiredLegalMembers" Value="True" runat="server" ClientIDMode="Static" />
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
                                                <td class="deleteRow">
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

        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Attachments</h2>
                <div class="comment-block-red" id="divUploadDocumetsNote" runat="server">*You must first save the project in order to upload any attachments*</div>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-4 col-md-4">
                <label class="control-label" id="lblUploadProjectBrief"><span class="markrequired UploadProjectBrief">*</span>Add Project Brief:</label>
            </div>
            <div class="col-xs-12 col-sm-8 col-md-8">
                <asp:Button ID="btnUploadProjectBrief" runat="server" class="ButtonControlAutoSize" Text="Add Project Brief " OnClientClick="openAttachment('Stage Gate Project Brief', 'Upload Stage Gate Project Brief Documents'); return false;" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table>
                    <asp:Repeater ID="rptProjectBriefAttachments" runat="server" OnItemCommand="rpProjectBriefAttachments_ItemCommand">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>
                                <th>Document Name</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton runat="server" ID="lnkProjectBriefAttachment" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='javascript:return confirm("Are you sure you want to delete?")' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileUrl") %>'></asp:LinkButton>
                                </td>
                                <td>
                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:HiddenField ID="hdnAddProjectBriefRequired" runat="server" Value="True" ClientIDMode="Static" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12"><span class="head">&nbsp;</span></div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-4 col-md-4">
                <label class="control-label">Upload Relevant Documents:</label>
            </div>
            <div class="col-xs-12 col-sm-8 col-md-8">
                <asp:Button ID="btnUploadOtherAttachments" runat="server" class="ButtonControlAutoSize" Text="Upload Relevant Documents" OnClientClick="openAttachment('Stage Gate Others', 'Upload Stage Gate Other Documents'); return false;" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table>
                    <asp:Repeater ID="rptOtherAttachments" runat="server" OnItemCommand="rpOtherAttachments_ItemCommand">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>
                                <th>Document Name</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton runat="server" ID="lnkOtherAttachment" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='javascript:return confirm("Are you sure you want to delete?")' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileUrl") %>'></asp:LinkButton>
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
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-3 col-md-1">
                <asp:Button ID="btnRedirect" ClientIDMode="Static" runat="server" Text="Redirect" Style="visibility: hidden; display: none;" CssClass="ButtonControl" OnClick="btnRedirect_Click" />
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-6 col-md-9">
                <asp:Label ID="lblSaved" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
            </div>
            <div class="col-xs-12 col-sm-3 col-md-2">
                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="false" CssClass="ButtonControl justifyRight" OnClick="btnSave_Click" ClientIDMode="Static" />&nbsp;&nbsp;&nbsp;
            </div>
            <div class="col-xs-12 col-sm-3 col-md-1">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return ValidateData(this);" CssClass="ButtonControl" OnClick="btnSubmit_Click" ClientIDMode="Static" />
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-6 col-md-9">
                <asp:Label ID="lblSubmit" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnStage" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnUploadingDocuments" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnProjectNumber" runat="server" ClientIDMode="Static" />
                <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="LoadAttachments HiddenButton" OnClick="btnReloadAttachment_Click"></asp:Button>
                <asp:HiddenField ID="hdnProjectAlreadySubmitted" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnProjectAlreadySubmittedDate" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnProjectAlreadySubmittedOK" runat="server" ClientIDMode="Static" />
                <asp:HiddenField ID="hdnRevisedFirstShipDate" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </div>
</div>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/StageGateJS.js?v=100"></script>
