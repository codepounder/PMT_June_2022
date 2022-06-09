<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TradePromoGroupForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.TradePromoGroupForm.TradePromoGroupForm" %>

<div class="container" id="dvMain" runat="server">
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
            <h1>Trade Promo Group</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Critical Data Points</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <asp:Label ID="lblComments" CssClass="control-label" Text="Item Concept:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                <asp:TextBox ID="txtItemConcept" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                <asp:TextBox ID="lblProductHierarchyLevel1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 2:</label>
                <asp:TextBox ID="lblProductHierarchyLevel2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 1 (Brand):</label>
                <asp:TextBox ID="lblMaterialGroup1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 4 (Product Form):</label>
                <asp:TextBox ID="lblMaterialGroup4" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 5 (Pack Type):</label>
                <asp:TextBox ID="lblMaterialGroup5" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Case Type:</label>
                <asp:TextBox ID="lblCaseType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divNumberOfUnitsInsideCarton">
            <div class="form-group">
                <label class="control-label">Number of Units Inside of Carton/Mixed Bag/Tray/Etc.:</label>
                <asp:TextBox ID="txtNumberOfUnitsInsideCarton" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divIndividualPouchWeight">
            <div class="form-group">
                <label class="control-label">Individual Pouch Weight (oz):</label>
                <asp:TextBox ID="txtIndividualPouchWeight" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divNumberOfTraysPerBaseUOM">
            <div class="form-group">
                <label class="control-label">Number of Trays per Base UOM:</label>
                <asp:TextBox ID="txtNumberOfTraysPerBaseUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divRetailSellingUOM">
            <div class="form-group">
                <label class="control-label">Retail Selling Units per Base UOM:</label>
                <asp:TextBox ID="txtRetailSellingUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divRetailUnitWeight">
            <div class="form-group">
                <label class="control-label">Retail Unit Weight (oz):</label>
                <asp:TextBox ID="txtRetailUnitWeight" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" runat="server" id="divBaseUOMNetWeight">
            <div class="form-group">
                <label class="control-label">Base UOM Net Weight (lbs):</label>
                <asp:TextBox ID="txtBaseUOMNetWeight" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">List Price B1 per Selling Unit:</label>
                <asp:TextBox ID="txtTruckloadPricePerUnit" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Truckload Price per Base UOM:</label>
                <asp:TextBox ID="txtTruckloadPriceUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Base UOM:</label>
                <asp:TextBox ID="txtBaseUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Customer/Channel Specific:</label>
                <asp:TextBox ID="txtChannelSpecific" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="dvCustomer" runat="server">
            <div class="form-group">
                <label class="control-label">Customer:</label>
                <asp:TextBox ID="txtCustomer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="dvChannel" runat="server">
            <div class="form-group">
                <label class="control-label">Channel:</label>
                <asp:TextBox ID="txtChannel" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Trade Promo Group</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Material Group 2 (Trade Promo Group):</label>
                <asp:DropDownList ID="ddlMaterialGroup2" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
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
        <div class="col-xs-12 col-sm-6 col-md-4">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
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
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return ValidateData()" OnClick="btnSubmit_Click" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnProjectType" runat="server" />
        </div>
    </div>
</div>
<div class="container" id="dvMsg" visible="false" runat="server">
    <div class="row">
        <div id="div1" runat="server" class="col-xs-12 col-sm-12 col-md-12 PageAccessDenied">
            WARNING: This Project Type doesn't have access to this page.
        </div>
    </div>
</div>
