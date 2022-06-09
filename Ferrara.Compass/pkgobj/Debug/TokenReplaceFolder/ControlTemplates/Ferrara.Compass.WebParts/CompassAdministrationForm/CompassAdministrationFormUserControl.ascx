<%@ Assembly Name="Ferrara.Compass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04ae2c9e0ea4efe6" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompassAdministrationFormUserControl.ascx.cs" Inherits="Ferrara.Compass.WebParts.CompassAdministrationForm.CompassAdministrationFormUserControl" %>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Bootstrap/bootstrap.min.js"></script>

<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" rel="Stylesheet" />

<div class="container" id="dvMain" runat="server">
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>PMT Administration</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <asp:TextBox ID="txtCurrentStatus" runat="server" Text="" ForeColor="Green" BorderStyle="None" ReadOnly="True"></asp:TextBox>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Logs</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnDeleteLogs" runat="server" Text="Delete Logs" CssClass="ButtonControlAutoSize" OnClick="btnDeleteLogs_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnDeleteAllLogs" runat="server" Text="Delete 1000 Logs" CssClass="ButtonControlAutoSize" OnClick="btnDeleteAllLogs_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:TextBox ID="txtRowLimit" runat="server" CssClass="form-control">1000</asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <asp:Button ID="btnDeleteLogsList" runat="server" Text="Delete Logs List" CssClass="ButtonControlAutoSize" OnClick="btnDeleteLogsList_Click" />
                <-- FOR DEV ONLY!!!!!!
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="Button4" runat="server" Text="Delete Logs other method" CssClass="ButtonControlAutoSize" OnClick="btnDeleteLogsPart2_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Delete/Deploy User Fields to new field</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnMoveChildTeams" runat="server" Text="Copy child team to new field" CssClass="ButtonControlAutoSize" OnClick="btnCopyChildTeam_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="Button1" runat="server" Text="Delete All Users Field" CssClass="ButtonControlAutoSize" OnClick="btnDeleteChildTeam_Click" />
            </div>
        </div>
    </div>
    <hr />
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnMoveParentTeams" runat="server" Text="Copy parent team to new field" CssClass="ButtonControlAutoSize" OnClick="btnCopyParentTeam_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="Button2" runat="server" Text="Delete All Users Field" CssClass="ButtonControlAutoSize" OnClick="btnDeleteParentTeam_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Insert Rows to Approval List 2 to existing projects</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="Button3" runat="server" Text="Insert Approval List2 Items" CssClass="ButtonControlAutoSize" OnClick="btnInsertApprovalList2Item_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="Button6" runat="server" Text="Insert Approval List2 for Completed Items" CssClass="ButtonControlAutoSize" OnClick="btnInsertCancelledCompletedApprovalList2Item_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="Button7" runat="server" Text="Populated Approval List2 for Proc Items" CssClass="ButtonControlAutoSize" OnClick="btnPopulateApprovalList2AllFieldsItem_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:TextBox ID="projectNumbertoRest" runat="server" /><asp:Button ID="Button5" runat="server" Text="Reset Procurememnt Workflow Data" CssClass="ButtonControlAutoSize" OnClick="btnResetProcurementWorkflowData_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Compass Groups Cleanup - OBM to PM</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnOBMToPM" runat="server" Text="Compy OBM to PM" CssClass="ButtonControlAutoSize" OnClick="btnOBMToPM_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <asp:Label ID="lblOBMToPMComplete" runat="server" Text="" Visible="false" ForeColor="ForestGreen" Font-Bold="true" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Compass Groups Cleanup - Stage Gate project List - Rnd to InTech & Regulatory RnD to InTech Regulatory</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnIntechStageGateMove" runat="server" Text="Copy RnD to InTech" CssClass="ButtonControlAutoSize" OnClick="btnIntechStageGateMove_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <asp:Label ID="lblIntechStageGateMoveMove" runat="server" Text="" Visible="false" ForeColor="ForestGreen" Font-Bold="true" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnInTechSRegtageGateMove" runat="server" Text="Copy RnD to InTech" CssClass="ButtonControlAutoSize" OnClick="btnInTechRegStageGateMove_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <asp:Label ID="lblInTechRegStageGateMove" runat="server" Text="" Visible="false" ForeColor="ForestGreen" Font-Bold="true" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Compass Groups Cleanup - Compass Team List - Regulatory RnD to InTech Regulatory</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnIntechCompassteameMove" runat="server" Text="Copy RnD to InTech" CssClass="ButtonControlAutoSize" OnClick="btnIntechCompassteameMove_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <asp:Label ID="lblbtnIntechCompassteameMove" runat="server" Text="" Visible="false" ForeColor="ForestGreen" Font-Bold="true" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Senior PM Project Team Transfer - In Progress and On Hold Seasonal Projects</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnHolidayEasterTrasnfer" runat="server" Text="Holiday/Easter To Gina Zilleox" CssClass="ButtonControlAutoSize" OnClick="btnHolidayEasterTrasnfer_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <asp:Label ID="lblHolidayEasterTrasnfer" runat="server" Text="" Visible="false" ForeColor="ForestGreen" Font-Bold="true" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="btnHalloweenVday" runat="server" Text="Halloween/Vday To Phil Kielma" CssClass="ButtonControlAutoSize" OnClick="btnHalloweenVday_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <asp:Label ID="lblHalloweenVday" runat="server" Text="" Visible="false" ForeColor="ForestGreen" Font-Bold="true" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:FileUpload ID="docUpload" runat="server" />
                <asp:Button ID="btnMoveFieldsFromXML" runat="server" Text="Move Fields from XML - Active" CssClass="ButtonControlAutoSize" OnClick="btnMoveFieldsFromXMLActive_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="btnMoveFieldsFromXMLOld" runat="server" Text="Move Fields from XML - Cancelled" CssClass="ButtonControlAutoSize" OnClick="btnMoveFieldsFromXMLCancelled_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:FileUpload ID="TradePromoUpload" runat="server" />
                <asp:Button ID="btnMoveTradePromoActive" runat="server" Text="Move Trade Promo Fields from XML - Active" CssClass="ButtonControlAutoSize" OnClick="btnMoveTradePromoFieldsFromXMLActive_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:FileUpload ID="TradePromoUploadCancelled" runat="server" />
                <asp:Button ID="Button8" runat="server" Text="Move Trade Promo Fields from XML - Cancelled" CssClass="ButtonControlAutoSize" OnClick="btnMoveTradePromoFieldsFromXMLCancelled_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <asp:Button ID="Button9" runat="server" Text="Check for mismatched updates" CssClass="ButtonControlAutoSize" OnClick="btnBadUpdatesReport_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <asp:FileUpload ID="docUploadSingleColumn" runat="server" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="Button11" runat="server" Text="Move PHL1" CssClass="ButtonControlAutoSize" OnClick="btnMovePHL1_click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="Button12" runat="server" Text="Move PHL2" CssClass="ButtonControlAutoSize" OnClick="btnMovePHL2_click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="Button13" runat="server" Text="Move Brand" CssClass="ButtonControlAutoSize" OnClick="btnMoveBrand_click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="Button14" runat="server" Text="Move Pack Type" CssClass="ButtonControlAutoSize" OnClick="btnMovePackType_click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="Button15" runat="server" Text="Move Trade Promo" CssClass="ButtonControlAutoSize" OnClick="btnMoveTradePromo_click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="Button10" runat="server" Text="Fix Profit Centers" CssClass="ButtonControlAutoSize" OnClick="btnCheckProfitCenter_Click" />
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="btnUpdateCancelledDate" runat="server" Text="Update Cancelled Date" CssClass="ButtonControlAutoSize" OnClick="btnUpdateCancelledDate_click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-12">
            <div class="form-group">
                <asp:Label ID="lblUpdateCancelledDate" runat="server" Visible="false" Style="color: red; font-weight: bold;" />
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="btnUpdatePreProductionDate" runat="server" Text="Update Pre-Production Date" CssClass="ButtonControlAutoSize" OnClick="btnUpdatePreProductionDate_click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-12">
            <div class="form-group">
                <asp:Label ID="lblUpdatePreProductionDate" runat="server" Visible="false" Style="color: red; font-weight: bold;" />
            </div>
        </div>
    </div>
     <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-2">
            <div class="form-group">
                <asp:Button ID="btnPopulateList" runat="server" Text="Populate List" CssClass="ButtonControlAutoSize" OnClick="btnPopulateList_Click" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-12">
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Visible="false" Style="color: red; font-weight: bold;" />
            </div>
        </div>
    </div>
</div>


