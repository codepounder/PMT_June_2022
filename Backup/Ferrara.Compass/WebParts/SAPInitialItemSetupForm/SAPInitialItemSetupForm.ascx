<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SAPInitialItemSetupForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.SAPInitialItemSetupForm.SAPInitialItemSetupForm" %>

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
            <h1>SAP Initial Item Setup Form</h1>
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
            <asp:HiddenField ID="hdnPLMProject" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnDeletedCompIds" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>SAP Initial Item Setup Tasks</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <asp:Repeater ID="rptCheckBoxes" runat="server" OnItemDataBound="rptCheckBoxes_ItemDataBound">
        <ItemTemplate>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="" CssClass="" /></td>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
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
            <label class="control-label">Immediate SPK Change:</label>
            <asp:TextBox ID="txtImmediateSPKChange" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <asp:Repeater ID="rptImmediateSPKChangeForSPSC" runat="server">
                <ItemTemplate>
                    <asp:Label ID="lblImmediateSPKChangeForTSORPSC" CssClass="control-label" Text='<%# "Immediate SPK Change for " + DataBinder.Eval(Container.DataItem, "MaterialNumber") + " :" %>' runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                    <asp:TextBox ID="txtImmediateSPKChangeForTSORPSC" Text='<%# DataBinder.Eval(Container.DataItem, "ImmediateSPKChange") %>' runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </ItemTemplate>
            </asp:Repeater>
        </div>
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
        <div class="col-xs-12 col-sm-12 col-md-6">
            <label class="control-label">Old Finished Good Item #:</label>
            <asp:TextBox ID="txtOldFGNumber" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-6">
            <label class="control-label">Old Finished Good Item Description:</label>
            <asp:TextBox ID="txtOldFGDescription" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Item Details</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Retail Selling Units per Base UOM:</label>
                <asp:TextBox ID="lblRetailSellingUnitsUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
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
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <label class="control-label">Retail Unit Weight (oz):</label>
                <asp:TextBox ID="lblRetailUnitWeight" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Logistics Information</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Finished Good Make Location:</label>
                <asp:TextBox ID="txtMakeLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Finished Good Pack Location:</label>
                <asp:TextBox ID="txtPackLocation1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="divProcurementType" runat="server">
            <div class="form-group">
                <label class="control-label">Procurement Type:</label>
                <asp:TextBox ID="txtProcurementType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4" id="divExternalManufacturer" runat="server">
            <div class="form-group">
                <label class="control-label">External Manufacturer:</label>
                <asp:TextBox ID="txtExternalManufacturer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="dvPackLocation" runat="server">
            <div class="form-group">
                <label class="control-label">External Packer:</label>
                <asp:TextBox ID="txtExternalPacker" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="divPurchaseIntoLocation" runat="server">
            <div class="form-group">
                <label class="control-label">Purchase Into Location:</label>
                <asp:TextBox ID="txtPurchaseIntoLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4" id="divSAPBaseUOM" runat="server">
            <div class="form-group">
                <label class="control-label">SAP Base UOM:</label>
                <asp:TextBox ID="txtSAPBaseUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
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
            <div class="form-group" id="divDeploymentModeofItem">
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
                <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                <asp:TextBox ID="lblProductHierarchyLevel1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 2:</label>
                <asp:TextBox ID="lblProductHierarchyLevel2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Material Group 1 (Brand):</label>
                <asp:TextBox ID="lblMaterialGroup" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Material Group 2 (Trade Promo Group):</label>
                <asp:TextBox ID="lblTradePromo" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Material Group 4 (Product Form):</label>
                <asp:TextBox ID="lblMaterialGroup4" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Material Group 5 (Pack Type):</label>
                <asp:TextBox ID="lblMaterialGroup5" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <label class="control-label">Profit Center:</label>
                <asp:TextBox ID="lblProfitCenter" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div id="divMixes" runat="server" visible="false">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Mixes</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table class="gridTable">
                    <asp:Repeater ID="rpMixesSummary" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th class="gridCellH">Item # in Mix</th>
                                <th class="gridCellH">Item Description</th>
                                <th class="gridCellH">Total Pieces per Selling Unit</th>
                                <th class="gridCellH">Ounces per Piece</th>
                                <th class="gridCellH">Ounces per Selling Unit</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblMixItemNumber" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ItemNumber") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblMixItemDescription" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ItemDescription") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="Label1" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "NumberOfPieces") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="Label2" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "OuncesPerPiece") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblOzPerPiece" runat="server"
                                        Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NumberOfPieces")) *
                                        Convert.ToDouble(DataBinder.Eval(Container.DataItem, "OuncesPerPiece")) %>'></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div id="divShipper" runat="server" visible="false">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Shippers</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table class="gridTable">
                    <asp:Repeater ID="rpShipperSummary" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th class="gridCellH">FG Item # in Display</th>
                                <th class="gridCellH">FG Item Description</th>
                                <th class="gridCellH"># of Units</th>
                                <th class="gridCellH">Ounces per Unit</th>
                                <th class="gridCellH">Ounces per FG Unit</th>
                                <th class="gridCellH">Pack Unit</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGItemDisplay" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumber") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGItemDescription" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemDescription") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGItemQuantity" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumberUnits") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGouncesPerUnit" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit") %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGouncesPerFGunit" runat="server"
                                        Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "FGItemNumberUnits")) *
                                        Convert.ToDouble(DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit")) %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGPackUnit" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGPackUnit") %>'></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Packaging Components</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvUserControl" class="dvUserControl" runat="server">
        <asp:PlaceHolder ID="phMsg" runat="server" />
    </div>
    <asp:PlaceHolder ID="phPage" runat="server" />
    <div id="BOMPages" clientidmode="Static" runat="server">
        <asp:PlaceHolder ID="phBOM" runat="server" />
    </div>

    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Initial Item Setup</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">SAP Item #:</label>
                <asp:TextBox ID="txtSAPItem" runat="server" MaxLength="20" ClientIDMode="Static" CssClass="form-control alphanumericToUpper1 required minimumlength"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-6">
            <div class="form-group">
                <label class="control-label">SAP Description:</label>
                <asp:TextBox ID="txtSAPDescription" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Unit UPC:</label>
                <asp:TextBox ID="txtUnitUPC" runat="server" CssClass="form-control numericNoMask requiredNeedsNew" MaxLength="12"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span id="spanDisplayUPC" class="markrequired">*</span><label class="control-label">Jar/Display UPC:</label>
                <asp:TextBox ID="txtDisplayUPC" runat="server" ClientIDMode="Static" CssClass="form-control numericNoMask requiredNeedsNew" MaxLength="12"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6"></div>
    </div>
    <div class="row" id="dvCaseUcc" runat="server">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Case UCC:</label>
                <asp:TextBox ID="txtCaseUCC" runat="server" CssClass="form-control numericNoMask requiredNeedsNew" MaxLength="14"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6"></div>
    </div>
    <div class="row" id="dvPalletUcc" runat="server">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Pallet UCC:</label>
                <asp:TextBox ID="txtPalletUCC" runat="server" CssClass="form-control numericNoMask requiredNeedsNew "></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6"></div>
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
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="ButtonControl" OnClientClick="return validatePage()" />
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
            <asp:HiddenField ID="hdnFGCount" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingNumbers" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingComponent" runat="server" />
            <asp:HiddenField ID="hddIsTransferSemiIncuded" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnComponentStatusChangeIds" ClientIDMode="Static" runat="server" />
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
<script type="text/javascript">
    $(document).ready(function () {
        conditionalChecks();
        pageLoadCheck();
    });
    function validatePage() {
        var isChecked = true;
        var isValid = true;
        var idchk = 'cblVerifications';
        var ed = 'Please verify all steps have been completed';
        if ($("#txtSAPItem").val().toLowerCase().trim() == "needs new") {
            var sd = "Please enter SAP Item#";
            var id = "txtSAPItem";
            $("#dverror_message").show();
            $('#error_message').append('<li class="errorMessage" >' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');

            isValid = false;
        }

        $('.bomTask input').each(function (idx, chk) {
            if (!chk.checked)
                isChecked = false;
        });
        if (!isValid) {
            isValid = ValidateData();
        }
        if (!isChecked) {
            $("#dverror_message").show();
            $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + ed + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + idchk + '"' + ') >Update</a>   </li></br>');
        }

        var isSAPNumbersValid = VerifySAPNumbers();

        if (isValid && isChecked && isSAPNumbersValid) {
            return true;
        }
        if (!isValid || !isChecked || !isSAPNumbersValid) {
            loadingIconAdded = true;
            $(".disablingLoadingIcon").remove();
            setFocusError();
        }
        return false;
    }
</script>
