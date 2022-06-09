<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BOMSetupSAPForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.BOMSetupSAPForm.BOMSetupSAPForm" %>

<style>
    #cblVerifications label {
        margin-left: 10px;
    }

    .bomTask {
        margin-left: 8px !important;
        margin-bottom: 0px !important;
        margin-top: 0px !important;
    }
</style>
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
            <h1>SAP BOM Setup</h1>
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
            <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDeletedCompIds" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <asp:PlaceHolder ID="phTopBOMSETUP" runat="server">
        <div id="dvTop" runat="server" clientidmode="Static">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>SAP BOM Tasks</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <asp:Repeater ID="rptCheckBoxes" runat="server" OnItemDataBound="rptCheckBoxes_ItemDataBound">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <asp:CheckBox ID="CheckBox1" runat="server" Text="" CssClass="" />
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <asp:Repeater ID="rptSPKDets" runat="server" OnItemDataBound="rptSPKDets_ItemDataBound">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <asp:Label ID="lblTSSPKStatus" runat="server" Font-Bold></asp:Label>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:Label ID="lblImmediateSPKChange" runat="server" Font-Bold></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:Label ID="lblflowthroughStatus" runat="server" Font-Bold>See BOM Below for material flowthrough statuses</asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Project Details</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-6">
                    <label class="control-label">Project Type:</label>
                    <asp:TextBox ID="txtProjectType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-6">
                    <label class="control-label">Project Type SubCategory:</label>
                    <asp:TextBox ID="txtProjectTypeSubCategory" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-12">
                    <div class="form-group">
                        <asp:Label ID="lblItemConcept" CssClass="control-label" Text="Item Concept:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                        <asp:TextBox ID="txtItemConcept" ClientIDMode="Static" TextMode="MultiLine" Rows="3" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Logistic Details</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6" id="dvProcurementType" runat="server">
                    <div class="form-group">
                        <label class="control-label">Procurement Type:</label>
                        <asp:TextBox ID="lblCoManufacturingClassification" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6" id="divMakeLocation" runat="server">
                    <div class="form-group">
                        <label class="control-label">Make Location:</label>
                        <asp:TextBox ID="lblMakeLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6" id="divPrimaryPackLocation" runat="server">
                    <div class="form-group">
                        <label class="control-label">Finished Good Pack Location:</label>
                        <asp:TextBox ID="lblPrimaryPackLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6" id="dvCoManLocation" runat="server">
                    <div class="form-group">
                        <label class="control-label">External Manufacturer:</label>
                        <asp:TextBox ID="lblExternalManufacturer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6" id="dvPackLocation" runat="server">
                    <div class="form-group">
                        <label class="control-label">External Packer:</label>
                        <asp:TextBox ID="lblExternalPacker" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6" id="dvPurchasedIntoCenter" runat="server">
                    <div class="form-group">
                        <label class="control-label">Purchased into Center:</label>
                        <asp:TextBox ID="lblPurchasedIntoCenter" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">SAP Base UOM:</label>
                        <asp:TextBox ID="lblSAPBaseUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Deployment Information</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group" id="divDesignateHUBDC">
                        <label class="control-label">Designate HUB DC (aka Material: Delivery Plant):</label>
                        <asp:TextBox ID="txtDesignateHUBDC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group" id="divDeploymentModeofItem" runat="server">
                        <label class="control-label">What is the Deployment Mode of Item?:</label>
                        <asp:TextBox ID="txtDeploymentModeofItem" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div id="divSELLDCs" runat="server">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoSL07" runat="server">
                            <label class="control-label">Extend to SL07 (Dallas):</label>
                            <asp:TextBox ID="txtExtendtoSL07" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetSL07SPKto" runat="server">
                            <label class="control-label">Set SL07 (Dallas) SPK to:</label>
                            <asp:TextBox ID="txtSetSL07SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoSL13" runat="server">
                            <label class="control-label">Extend to SL13 (Bolingbrook):</label>
                            <asp:TextBox ID="txtExtendtoSL13" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetSL13SPKto" runat="server">
                            <label class="control-label">Set SL13 (Bolingbrook) SPK to:</label>
                            <asp:TextBox ID="txtSetSL13SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoSL18" runat="server">
                            <label class="control-label">Extend to SL18 (Jonestown):</label>
                            <asp:TextBox ID="txtExtendtoSL18" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetSL18SPKto" runat="server">
                            <label class="control-label">Set SL18 (Jonestown) SPK to:</label>
                            <asp:TextBox ID="txtSetSL18SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoSL19" runat="server">
                            <label class="control-label">Extend to SL19 (Phoenix):</label>
                            <asp:TextBox ID="txtExtendtoSL19" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetSL19SPKto" runat="server">
                            <label class="control-label">Set SL19 (Phoenix) SPK to:</label>
                            <asp:TextBox ID="txtSetSL19SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoSL30" runat="server">
                            <label class="control-label">Extend to SL30 (Atlanta):</label>
                            <asp:TextBox ID="txtExtendtoSL30" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetSL30SPKto" runat="server">
                            <label class="control-label">Set SL30 (Atlanta) SPK to:</label>
                            <asp:TextBox ID="txtSetSL30SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoSL14" runat="server">
                            <label class="control-label">Extend to SL41 (DeKalb):</label>
                            <asp:TextBox ID="txtExtendtoSL14" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetSL14SPKto" runat="server">
                            <label class="control-label">Set SL41 (DeKalb) SPK to:</label>
                            <asp:TextBox ID="txtSetSL14SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div id="DivFERQDCs" runat="server">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoFQ26" runat="server">
                            <label class="control-label">Extend to FQ26 (Louisville):</label>
                            <asp:TextBox ID="txtExtendtoFQ26" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetFQ26SPKto" runat="server">
                            <label class="control-label">Set FQ26 (Louisville) SPK to:</label>
                            <asp:TextBox ID="txtSetFQ26SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoFQ27" runat="server">
                            <label class="control-label">Extend to FQ27 (Louisville):</label>
                            <asp:TextBox ID="txtExtendtoFQ27" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetFQ27SPKto" runat="server">
                            <label class="control-label">Set FQ27 (Louisville) SPK to:</label>
                            <asp:TextBox ID="txtSetFQ27SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoFQ28" runat="server">
                            <label class="control-label">Extend to FQ28 (Triways):</label>
                            <asp:TextBox ID="txtExtendtoFQ28" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetFQ28SPKto" runat="server">
                            <label class="control-label">Set FQ28 (Triways) SPK to:</label>
                            <asp:TextBox ID="txtSetFQ28SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoFQ29" runat="server">
                            <label class="control-label">Extend to FQ29 (Digital Orders/Evans):</label>
                            <asp:TextBox ID="txtExtendtoFQ29" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetFQ29SPKto" runat="server">
                            <label class="control-label">Set FQ29 (Digital Orders/Evans) SPK to:</label>
                            <asp:TextBox ID="txtSetFQ29SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoFQ34" runat="server">
                            <label class="control-label">Extend to FQ34 (Port Logistics):</label>
                            <asp:TextBox ID="txtExtendtoFQ34" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetFQ34SPKto" runat="server">
                            <label class="control-label">Set FQ34 (Port Logistics) SPK to:</label>
                            <asp:TextBox ID="txtSetFQ34SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divExtendtoFQ35" runat="server">
                            <label class="control-label">Extend to FQ35 (Advance):</label>
                            <asp:TextBox ID="txtExtendtoFQ35" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group" id="divSetFQ35SPKto" runat="server">
                            <label class="control-label">Set FQ35 (Advance) SPK to:</label>
                            <asp:TextBox ID="txtSetFQ35SPKto" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Product Hierarchy</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Product Hierarchy Level 1:</label>
                        <asp:TextBox ID="lblProductHierarchy1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Product Hierarchy Level 2:</label>
                        <asp:TextBox ID="lblProductHierarchy2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Material Group 1 (Brand):</label>
                        <asp:TextBox ID="lblMaterialGroup1Brand" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Material Group 2 (Trade Promo Group):</label>
                        <asp:TextBox ID="lblMaterialGroup2Pricing" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Material Group 4 (Product Form):</label>
                        <asp:TextBox ID="lblMaterialGroup4ProductForm" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Material Group 5 (Pack Type):</label>
                        <asp:TextBox ID="lblMaterialGroup5PackType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Profit Center:</label>
                        <asp:TextBox ID="lblProfitCenter" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Item UPCs/UCCs</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Unit UPC:</label>
                        <asp:TextBox ID="lblUnitUPC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Case UCC:</label>
                        <asp:TextBox ID="lblCaseUCC" runat="server" BorderStyle="None" ReadOnly="True" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6" id="divDisplayUPC" runat="server">
                    <div class="form-group">
                        <label class="control-label">Display UPC:</label>
                        <asp:TextBox ID="lblDisplayUPC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Pallet UCC:</label>
                        <asp:TextBox ID="lblPalletUCC" runat="server" BorderStyle="None" ReadOnly="True" ClientIDMode="Static" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Candy Semis</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row" runat="server" id="divNoCandySemi" visible="false">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="form-group">
                        <div class="comment-block">No Candy Semi / Purchased Candy Semi.</div>
                    </div>
                </div>
            </div>
            <asp:Repeater ID="rptCandy" runat="server">
                <ItemTemplate>
                    <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                        <div class="col-xs-6 col-sm-3 col-md-2">
                            <div class="form-group">
                                <label class="control-label">Component Type:</label>
                                <asp:TextBox ID="TextBox1" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>' ToolTip="Component Type" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-sm-3 col-md-2">
                            <div class="form-group">
                                <label class="control-label">New or Existing?:</label>
                                <asp:TextBox ID="txtNewExisting" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "NewExisting") %>' ToolTip="Candy Semi #" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-sm-3 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %> #:</label>
                                <asp:TextBox ID="txtMaterialNumber" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' ToolTip="Candy Semi #" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-sm-3 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %> Description:</label>
                                <asp:TextBox ID="txtMaterialDescription" runat="server" MaxLength="40" value='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' ToolTip="Candy Semi #" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-6 col-sm-3 col-md-2">
                            <div class="form-group">
                                <label class="control-label">Shelf Life:</label>
                                <asp:TextBox ID="txtShelfLife" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "ShelfLife") %>' ToolTip="Shelf Life" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
    </asp:PlaceHolder>
    <div id="dvUserControl" class="dvUserControl" runat="server">
        <asp:PlaceHolder ID="phBOMEdits" runat="server" />
    </div>
    <asp:PlaceHolder ID="phPage" runat="server">
        <div id="BOMPages" runat="server" class="sapBOMSetup" clientidmode="Static">
            <asp:PlaceHolder ID="phBOMGrid" runat="server" />
        </div>
    </asp:PlaceHolder>
    <asp:HiddenField ID="hdnComponentStatusChangeIds" ClientIDMode="Static" runat="server" />
    <asp:PlaceHolder ID="phBottomBOMSETUP" runat="server">
        <div id="dvBottom" runat="server">
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-9 col-md-10">
                    <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-1">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="ButtonControl" CausesValidation="false" />&nbsp;&nbsp;&nbsp;
                </div>
                <div class="col-xs-12 col-sm-3 col-md-1">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="ButtonControl" OnClientClick="return validatePage();" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnProjectType" runat="server" />
                    <asp:HiddenField ID="hdnComponentype" runat="server" />
                    <asp:HiddenField ID="hdnParentID" runat="server" />
                    <asp:HiddenField ID="hdnPackagingID" runat="server" />
                    <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
                    <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
                    <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnPackagingNumbers" ClientIDMode="Static" runat="server" />
                </div>
            </div>
        </div>
    </asp:PlaceHolder>
</div>
<div class="container" id="dvMsg" visible="false" runat="server">
    <div class="row">
        <div id="div1" runat="server" class="col-xs-12 col-sm-12 col-md-12 PageAccessDenied">
            WARNING: This Project Type doesn't have access to this page.
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        pageLoadCheck();
    });
</script>
