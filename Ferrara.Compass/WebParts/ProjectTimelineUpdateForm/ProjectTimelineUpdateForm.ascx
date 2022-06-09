<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectTimelineUpdateForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.ProjectTimelineUpdateForm.ProjectTimelineUpdateForm" %>

<link rel="stylesheet" href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap.min.css" />
<link rel="stylesheet" href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" />

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Bootstrap/bootstrap.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        $("input").not("#txtProjectNo").keydown(function (e) {
            if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110, 190]) !== -1 ||
                // Allow: Ctrl+A, Command+A
            (e.keyCode === 65 && (e.ctrlKey === true || e.metaKey === true)) ||
                // Allow: home, end, left, right, down, up
            (e.keyCode >= 35 && e.keyCode <= 40)) {
                // let it happen, don't do anything
                return;
            }
            // Ensure that it is a number and stop the keypress
            if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
                e.preventDefault();
            }
        });
    });

</script>
<div class="container">
    <h1>Project Timeline Update Form</h1>

    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblIPF" runat="server" class="control-label">New IPF # of Days:</label>
                <asp:TextBox class="form-control" ID="txtIPF" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblPrelimSAPInitialSetup" runat="server" class="control-label">New Preliminary SAPInitialSetup # of Days:</label>
                <asp:TextBox class="form-control" ID="txtPrelimSAPInitialSetup" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblSrOBMApproval" runat="server" class="control-label">New SrPMApproval # of Days:</label>
                <asp:TextBox class="form-control" ID="txtSrOBMApproval" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblSrOBMApproval2" runat="server" class="control-label">New SrPMApproval2 # of Days:</label>
                <asp:TextBox class="form-control" ID="txtSrOBMApproval2" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblInitialCosting" runat="server" class="control-label">New InitialCosting # of Days:</label>
                <asp:TextBox class="form-control" ID="txtInitialCosting" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblInitialCapacity" runat="server" class="control-label">New InitialCapacity # of Days:</label>
                <asp:TextBox class="form-control" ID="txtInitialCapacity" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblTradePromo" runat="server" class="control-label">New TradePromo # of Days:</label>
                <asp:TextBox class="form-control" ID="txtTradePromo" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblDistribution" runat="server" class="control-label">New Distribution # of Days:</label>
                <asp:TextBox class="form-control" ID="txtDistribution" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblOperations" runat="server" class="control-label">New Operations # of Days:</label>
                <asp:TextBox class="form-control" ID="txtOperations" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblSAPInitialSetup" runat="server" class="control-label">New SAPInitialSetup # of Days:</label>
                <asp:TextBox class="form-control" ID="txtSAPInitialSetup" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblQA" runat="server" class="control-label">New QA # of Days:</label>
                <asp:TextBox class="form-control" ID="txtQA" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblOBMReview1" runat="server" class="control-label">New PMReview1 # of Days:</label>
                <asp:TextBox class="form-control" ID="txtOBMReview1" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblBOMSetupPE" runat="server" class="control-label">New BOMSetupPE # of Days:</label>
                <asp:TextBox class="form-control" ID="txtBOMSetupPE" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblBOMSetupProc" runat="server" class="control-label">New BOMSetupProc # of Days:</label>
                <asp:TextBox class="form-control" ID="txtBOMSetupProc" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblBOMSetupPE2" runat="server" class="control-label">New BOMSetupPE2 # of Days:</label>
                <asp:TextBox class="form-control" ID="txtBOMSetupPE2" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblOBMReview2" runat="server" class="control-label">New PMReview2 # of Days:</label>
                <asp:TextBox class="form-control" ID="txtOBMReview2" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblGRAPHICS" runat="server" class="control-label">New GRAPHICS # of Days:</label>
                <asp:TextBox class="form-control" ID="txtGRAPHICS" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblCostingQuote" runat="server" class="control-label">New CostingQuote # of Days:</label>
                <asp:TextBox class="form-control" ID="txtCostingQuote" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblFGPackSpec" runat="server" class="control-label">New FGPackSpec # of Days:</label>
                <asp:TextBox class="form-control" ID="txtFGPackSpec" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblSAPBOMSetup" runat="server" class="control-label">New SAPBOMSetup # of Days:</label>
                <asp:TextBox class="form-control" ID="txtSAPBOMSetup" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3">
            <div class="form-group">
                <label id="lblExternalMfg" runat="server" class="control-label">New ExternalMfg # of Days:</label>
                <asp:TextBox class="form-control" ID="txtExternalMfg" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <asp:Label ID="lblSaved" CssClass="" runat="server"></asp:Label>
    <asp:Button ID="btnSave" runat="server" CssClass="clickSave button justifyRight" Text="Submit" OnClick="btnSave_Click" />

</div>
