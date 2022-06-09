<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StageGateProjectPanelForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.StageGateProjectPanelForm.StageGateProjectPanelForm" %>

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
    <%-- <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Project Summary Form</h1>
        </div>
    </div>--%>
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
        <div class="modal fade" id="DialogStageCancelMessage" role="dialog" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                        <h4 class="modal-title">Stage Gate Parent Project Cancellation</h4>
                    </div>
                    <div class="modal-body" style="display: flex;">
                        <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                        <p style="margin-left: 10px;">
                            This Cancel Project button will cause the Parent Project and all IPF/Child Projects will be Canceled. Please confirm you'd like to Cancel all projects associated with this Parent Project.
                            <br />
                            <br />
                            Are you sure you’d like to Cancel?
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnResubmitClose" class="btn btn-default" data-dismiss="modal" onclick="DialogStageCancelProjectMessageCancel()">No</button>
                        <button type="button" id="btnResubmitOk" class="btn btn-default" data-dismiss="modal" onclick="DialogStageCancelProjectMessageOK()">Yes</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="SGSProjectInformation"></asp:PlaceHolder>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <asp:PlaceHolder ID="phMsg" runat="server" />
        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="phRASection">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Risk Assessment</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <asp:Repeater ID="rptGateDetails" runat="server" OnItemDataBound="rptGateDetails_ItemDataBound">
                <ItemTemplate>
                    <div class="col-xs-12 col-sm-12 col-md-4">
                        <asp:PlaceHolder ID="phGateInfo" runat="server" />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12"></div>
    </div>
    <asp:Table ID="projectSummary" runat="server" ClientIDMode="Static">
        <asp:TableHeaderRow TableSection="TableHeader" runat="server" ID="headerRow" ClientIDMode="Static">
            <asp:TableHeaderCell CssClass="processCol">Process</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="statusCol">Status</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="startDayCol">Start Date</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="endDayCol">End Date</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="submittedCol">Submitted By</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="totalDaysCol">Duration</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>
    <br />
    <br />
    <br />
    <br />
    <asp:UpdatePanel ID="updateFirstShipDate" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Update First Ship Date: </label>
                        <asp:TextBox ID="txtRevisedFirstShipDate" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date" AppendDataBoundItems="true"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-8">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                        <asp:Button ID="btnSubmit" ClientIDMode="Static" runat="server" Text="Submit" CssClass="ButtonControl" OnClick="btnSubmit_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatOnHoldProject" runat="server">
        <ContentTemplate>
            <div class="row" runat="server" id="divOnHold">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="form-group">
                        <label class="control-label">&nbsp;</label>
                        <asp:Button ID="btnOnHoldProject" ClientIDMode="Static" runat="server" Text="On Hold" CssClass="ButtonControl" OnClick="btnOnHoldProject_Click" />
                        <asp:Button ID="btnRemoveOnHold" ClientIDMode="Static" runat="server" Text="Remove On Hold" CssClass="ButtonControlAutoSize" OnClick="btnRemoveOnHold_Click" />
                        <asp:Label ID="lblOnHoldButtonMessage" ClientIDMode="Static" Visible="false" runat="server" CssClass="SuccessMessage"></asp:Label>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div runat="server" id="divCancelProject">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <br />
                    <asp:Button ID="btnCancelProject" ClientIDMode="Static" runat="server" Text="Cancel Project" CssClass="ButtonControlAutoSize" OnClientClick="return confirmCancel();" />
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Button ID="btnCancel" ClientIDMode="Static" runat="server" Text="Cancel Project" CssClass="ButtonControlAutoSize HiddenButton" OnClick="btnCancel_Click" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
            <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnStageGateProjectListItemId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnProjectNumber" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPrevSubmittedDate" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />

        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <asp:UpdatePanel ID="attachments" runat="server">
        <ContentTemplate>
            <asp:Panel ID="PanelAttachments" runat="server">
                <asp:Repeater ID="rpAttachmentsHeader" runat="server" OnItemDataBound="rpAttachmentsHeader_ItemDataBound">
                    <ItemTemplate>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <label class="control-label">
                                    <asp:PlaceHolder ID="lblStageHeader" runat="server"></asp:PlaceHolder>
                                </label>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <label class="control-label">File Type</label>
                            </div>
                        </div>
                        <asp:Repeater ID="rpAttachments" runat="server">
                            <ItemTemplate>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-6">
                                        <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                    </div>
                                    <div class="col-xs-12 col-sm-12 col-md-6">
                                        <label class="control-label"><%#Eval("DocType")%></label>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="row">
                    <div class="col-xs-12 col-sm-8 col-md-12">
                        <asp:Button ID="btnUploadOtherAttachments" runat="server" class="ButtonControlAutoSize" Text="Add Project File" OnClientClick="openAttachment('Stage Gate Others', 'Upload Stage Gate Other Documents'); return false;" />
                    </div>
                </div>
            </asp:Panel>
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="LoadAttachments HiddenButton" OnClick="btnReloadAttachment_Click"></asp:Button>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
