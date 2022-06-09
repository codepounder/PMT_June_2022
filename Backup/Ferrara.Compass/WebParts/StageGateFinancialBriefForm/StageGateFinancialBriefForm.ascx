<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StageGateFinancialBriefForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.StageGateFinancialBriefListForm.StageGateFinancialBriefForm" %>

<div class="container">
    <div class="row PageSubHeader" runat="server" id="divPageTitle" style="margin-top: -49px; margin-left: -5px;">
        <h2>
            <asp:Label CssClass="control-label" ID="lblFinanceBriefPageSubTitle" runat="server"></asp:Label>
        </h2>
    </div>
    <div id="divAccessDenied" runat="server" class="AccessDenied">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                WARNING: You do not have access to update this page. Save functionality will be disabled!
                <br />
                If you require access, please email the
                <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False">HelpDesk</asp:LinkButton>.<br />
            </div>
        </div>
    </div>
    <div id="divAccessRequest" runat="server" class="AccessRequest">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                Your request for access has been sent!
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdateSummary" runat="server" style="margin-left: 50px; margin-right: 35px;">
        <ContentTemplate>
            <asp:Panel ID="SummarySection" ClientIDMode="Static" runat="server">
                <asp:PlaceHolder ID="phFinancialSummary" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdateAnalysis" runat="server" style="margin-left: 50px; margin-right: 35px;">
        <ContentTemplate>
            <asp:Panel ID="AnalysisSection" ClientIDMode="Static" runat="server">
                <asp:PlaceHolder ID="phFinancialBriefAnalysis" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row" style="margin-left: 50px; margin-right: 35px;">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <div class="col-xs-12 col-sm-6 col-md-11">
                    <asp:Label ID="lblSaved" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-1">
                    <asp:Button ID="btnSaveFinacialAnalysis" OnClick="btnSaveFinacialAnalysis_Click" CssClass="ButtonControlAutoSize" Text="Save" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<asp:HiddenField ID="hdnStageGateProjectListItemId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnGate" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnBriefNumber" runat="server" ClientIDMode="Static" />
