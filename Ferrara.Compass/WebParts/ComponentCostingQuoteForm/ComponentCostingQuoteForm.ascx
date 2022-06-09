<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ComponentCostingQuoteForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.ComponentCostingQuoteForm.ComponentCostingQuoteForm" %>

<div class="container" id="dvcontainer">
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
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Request Component Costing Quote Form</h1>
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
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>General Information: <asp:Label ID="lblQRHeader" runat="server" Text="Label"></asp:Label> - <asp:Label ID="lblComponentType" runat="server" Text="Label"></asp:Label> </h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div> 
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Procurement Manager:</label>
                <SharePoint:PeopleEditor ID="peProcurementManager" runat="server" AllowEmpty="true" MultiSelect="false" SelectionSet="User" SharePointGroup="Procurement Managers" />
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Date of Submission:</label>
                <asp:TextBox ID="txtCostingQuoteDate" runat="server"  CssClass="datePicker form-control"></asp:TextBox>
            </div>
        </div>
     </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">SAP Material #:</label>
                <asp:TextBox ID="txtMaterial" runat="server" CssClass="form-control required"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Material Description:</label>
                <asp:TextBox ID="txtMaterialDescription" runat="server" MaxLength="40" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
           <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Receiving Plant:</label>
                <asp:DropDownList ID="ddlReceivingPlant" runat="server" CssClass="form-control required" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList> 
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Base UOM:</label>
                <asp:TextBox ID="lblBaseUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control numericNoMask"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Annual Projected Retail Selling Units:</label>
                <asp:TextBox ID="lblAnnualVolume" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control numericNoMask"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 ">
           <div class="form-group">
               <label class="control-label">First 90 days volume (EA):</label>
                <asp:TextBox ID="txt90daysvol" runat="server"  CssClass="form-control minimumlength numericNoMask"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">SKU #:</label>
                <asp:TextBox ID="lblSKU" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control numericNoMask"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="dvPrintStyle" class="col-xs-12 col-sm-6 col-md-4 filmdv print hideItem otherdv" runat="server">
            <div class="form-group">
              <label class="control-label">Print Style:</label>
                  <asp:DropDownList ID="ddlFilmPrintStyle"  ClientIDMode="Static" runat="server"  Width="96%" CssClass="form-control otherctrl" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                  </asp:DropDownList>
            </div>
        </div>
        <div id="dvStyle" class="col-xs-12 col-sm-6 col-md-4 print filmdv hideItem otherdv" runat="server">
           <div class="form-group">
            <label class="control-label">Style:</label>
                  <asp:DropDownList ID="ddlFilmStyle" runat="server" Width="96%" CssClass="form-control filmctrl otherctrl" AppendDataBoundItems="true">
                     <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                 </asp:DropDownList>
            </div>
        </div>
        <div id="dvPackagingComponentStructure" class="col-xs-12 col-sm-6 col-md-4 flr hideItem otherdv" runat="server">
           <div class="form-group ">
               <asp:Label ID="lblStructure" CSSClass="control-label" Font-Bold="true" runat="server">(<i>Packaging Component</i>) Structure:</asp:Label>
                <asp:DropDownList ID="ddlFilmStructure" runat="server" CssClass="form-control flrctrl otherctrl" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>   
            </div>
        </div>
    </div>
    <div class="row">
        <div id="dvWebWidth" class="col-xs-12 col-sm-6 col-md-6 fldv otherdv hideItem" runat="server">
            <div class="form-group ">
               <label class="control-label"> Web Width:</label>
                <asp:TextBox ID="txtFilmWebWidth" CssClass="form-control flctrl otherctrl numericNoMask"  ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
        <div id="dvExactCutOff" class="col-xs-12 col-sm-6 col-md-6 fldv otherdv hideItem" runat="server">
           <div class="form-group ">
              <label class="control-label">Exact Cut Off:</label>
               <asp:TextBox ID="txtFilmExactCutoff" CssClass="form-control flctrl otherctrl numericNoMask"  ClientIDMode="Static" runat="server" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="dvUnwind" class="col-xs-12 col-sm-6 col-md-6 filmdv lodv hideItem" runat="server">
            <div class="form-group ">
              <label class="control-label">UnWind:</label>
                <asp:DropDownList ID="ddlFilmUnWind" runat="server" CssClass="form-control filmctrl loctrl" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>   
            </div>
        </div>
        <div id="dvCoreSize" class="col-xs-12 col-sm-6 col-md-6 filmdv lodv hideItem" runat="server">
           <div class="form-group ">
              <label class="control-label">Core Size (roll ID):</label>
              <asp:TextBox ID="txtCoreSize" runat="server"  CssClass="form-control numericNoMask filmctrl loctrl"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="dvMaxDiameter" class="col-xs-12 col-sm-6 col-md-6  filmdv lodv hideItem" runat="server">
            <div class="form-group ">
              <label class="control-label">Max Diameter:</label>
                 <asp:TextBox ID="txtMaxDiaMeter" runat="server"  CssClass="form-control numericNoMask filmctrl loctrl"></asp:TextBox>
            </div>
        </div>       
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
              <label class="control-label">Quantities to Quote:</label>
                 <asp:TextBox ID="txtQuantitiesQuote" runat="server" CssClass="form-control numericNoMask" ></asp:TextBox>
            </div>
        </div>    
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
                <label class="control-label">Comments on Forecast:</label>
                <asp:TextBox ID="txtCommentForecast" runat="server" TextMode="MultiLine"  CssClass="form-control" ></asp:TextBox>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-6 ">
            <div class="form-group ">
               <label class="control-label">Request Due Date:</label>
               <asp:TextBox ID="txtRequestDueDate" runat="server"  CssClass="datePicker form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="dvNumberofColors" class="col-xs-12 col-sm-6 col-md-6 rigiddv" runat="server">
            <div class="form-group ">
                <label class="control-label">Number of Colors:</label>
                <asp:TextBox ID="txtColors" runat="server" CssClass="form-control numericNoMask rigidctrl"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row attachment">
         <div class="col-xs-12 col-sm-3 col-md-4 ">
            <label class="control-label">Approved Dieline(s):</label>
        </div>
    </div>
    <div class="row attachment">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table width="50%">
                <asp:Repeater ID="rptCApprovedDieline"  ClientIDMode="Static" runat="server" >
                    <HeaderTemplate>
                        <tr>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
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
        <div class="col-xs-12 col-sm-9 col-md-4">  
            <asp:Button ID="btnCreatePDF" runat="server" CssClass="ButtonControlAutoSize" Text="Create PDF of Quote Request" OnClick="btnCreatePDF_Click" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Info for Procurement Capabilities</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">SAP Vendor #:</label>
                <asp:TextBox ID="txtVendorNumber" runat="server"  CssClass="form-control minimumlength numericNoMask required"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Vendor Name:</label>
                <asp:TextBox ID="txtPrinterSupplier" runat="server"  CssClass="form-control" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                 <span class="markrequired">*</span><label class="control-label">Validity Start Date:</label>
                <asp:TextBox ID="txtValidityStartDate" runat="server" BorderStyle="None" CssClass="datePicker required form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                 <span class="markrequired">*</span><label class="control-label">Validity End Date:</label>
                <asp:TextBox ID="txtValidityEndDate" runat="server" BorderStyle="None" CssClass="datePicker required form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Supplier Agreement #:</label>
                <asp:TextBox ID="txtSupplierAgreementNumber" runat="server" CssClass="form-control numericNoMask"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 labeluc ">
           <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Price Determination:</label>
                <asp:DropDownList ID="drpPriceDetermination" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Delivery Date" Value="Delivery Date"></asp:ListItem>
                    <asp:ListItem Text="PO Date" Value="PO Date"></asp:ListItem>
                </asp:DropDownList> 
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6 labeluc ">
           <div class="form-group">
                <label class="control-label">Subcontracted?:</label>
                <asp:DropDownList ID="drpSubcontracted" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList> 
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
           <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Bracket Pricing:</label>
                <asp:DropDownList ID="ddlBracketPricing" ClientIDMode="Static" runat="server" CssClass="form-control required" onChange="bracketPricingReq();" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList> 
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
           <div class="form-group ">
                <span class="markrequired">*</span><label class="control-label">Order Unit of Measure:</label>
                <asp:DropDownList ID="ddlOrderUnitofMeasure" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList> 
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Delivered or Origin Cost?:</label>
                <asp:DropDownList ID="ddlDeliveredOriginCost" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Delivered" Value="Delivered"></asp:ListItem>
                    <asp:ListItem Text="Origin Cost" Value="Origin Cost"></asp:ListItem>
                </asp:DropDownList> 
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Freight Amount:</label>
                <asp:TextBox ID="txtFreightAmount" runat="server"  CssClass="form-control numericNoMask"></asp:TextBox>
            </div>
        </div>       
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
              <span class="markrequired">*</span><label class="control-label">Incoterms:</label>
                <asp:DropDownList ID="ddlIncoterms" runat="server" CssClass="required form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList> 
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Transfer of ownership City, State:</label>
                <asp:TextBox ID="txtTransferOfOwnership" runat="server" CssClass="form-control required"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Planned Delivery Time (Lead time):</label>
                <div class="input-group">
                    <asp:TextBox ID="txtPlannedDeliveryTime" runat="server"  CssClass="form-control required numericNoMask"></asp:TextBox><span class="input-group-addon">Days</span>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Minimum Order Quantity:</label>
                <asp:TextBox ID="txtMinimumOrderQTY" runat="server"  CssClass="form-control numericNoMask required"></asp:TextBox>
            </div>
        </div>
   </div>
   <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Standard Quantity:</label>
                <asp:TextBox ID="txtStandardQuantity" runat="server"  CssClass="form-control numericNoMask required"></asp:TextBox>
            </div>
        </div>
       <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">PIR Cost per UOM (order unit):</label>
                <asp:TextBox ID="txtPIRCostperUOM" runat="server"  CssClass="form-control required"></asp:TextBox>
            </div>
        </div>
       <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Per Unit:</label>
                <asp:TextBox ID="txtPerUnit" runat="server"  CssClass="form-control required numericNoMask"></asp:TextBox>
            </div>
        </div>
       <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Base UOM:</label>
                <asp:TextBox ID="lblStandardBaseUOM" runat="server" ReadOnly="true"  CssClass="form-control readOnly"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Tol. Overdelivery:</label>
                <div class="input-group">
                    <asp:TextBox ID="txtTolOverDelivery" runat="server"  CssClass="form-control numericNoMask required"></asp:TextBox><span class="input-group-addon">%</span>
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
              <span class="markrequired">*</span><label class="control-label">Tol. Underdelivery:</label>
                <div class="input-group">
                    <asp:TextBox ID="txtTolUnderDelivery" runat="server"  CssClass="form-control numericNoMask required"></asp:TextBox><span class="input-group-addon">%</span>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Purchasing Group:</label>
                <asp:TextBox ID="txtPurchasingGroup" runat="server"  CssClass="form-control numericNoMask"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
           <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Supplier Material #:</label>
                <asp:TextBox ID="txtVendorMaterial" runat="server" MaxLength="20" CssClass="form-control minimumlength alphanumericToUpper1 required"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Conversion factors from Base UOM to alternate UOM's:</label>
                <table id="alternateUOMTable">
                    <asp:Repeater ID="rptUOMAlternate" runat="server" OnItemCommand="rptUOMAlternate_ItemCommand" OnItemDataBound="rptUOMAlternate_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <th>x</th>
                                <th>Alternate UOM</th>
                                <th>=</th>
                                <th>Y</th>
                                <th>Base UOM</th>
                                <th>Action</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="alternateUOMRow" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                                <td>
                                    <asp:TextBox ID="txtX" runat="server" class="required numericNoMask form-control"></asp:TextBox>
                                    <asp:HiddenField ID="hdnAlternateUOMID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpAlternateUOM" runat="server" CssClass="required form-control">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>=</td>
                                <td>
                                    <asp:TextBox ID="txtY" runat="server" class="form-control required numericNoMask"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBaseUOM" Enabled="false" ReadOnly="true" runat="server" class="readOnly form-control"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:ImageButton ID="btnDeleteAlternateUOM" CssClass="readOnly" CausesValidation="false" AlternateText="Delete AttaAlternate UOM" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:Button ID="btnAddNewAlternateUOM" CssClass="ButtonControlAutoSize" Text="Add New Alternate UOM" CausesValidation="false" runat="server" OnClick="btnAddNewAlternateUOM_Click" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
                <span class="requiredspan markrequired">* </span>  <label class="control-label">Costing Conditions/All Costing Information:</label>
                <asp:TextBox ID="txtCostingInfo" runat="server" CssClass="form-control required" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Information from  Vendor </h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-6 col-sm-3 col-md-3">
            <span class="requiredspan markrequired">* </span>  <label class="control-label">Costing Unit:</label>
            <asp:DropDownList ID="ddlCostingUnit" runat="server" CssClass="form-control" AppendDataBoundItems="True" onchange="updateVendorLabels()"></asp:DropDownList>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
                <span class="requiredspan markrequired">* </span>  <label id="lblEachesPerCostingUnit" class="control-label">Eaches per X:</label>
                <asp:TextBox ID="txtEacher" runat="server" CssClass="form-control numericNoMask required"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 labeluc ">
           <div class="form-group ">
                <span class="requiredspan markrequired">* </span>  <label id="lblLBPerCostingUnit" class="control-label">LB per X:</label>
                    <asp:TextBox ID="txtLBRoll" runat="server" CssClass="form-control numericNoMask required"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
              <span class="requiredspan markrequired">*</span>  <label id="lblCostingUnitPerPallet" class="control-label">X per Pallet:</label>
                <asp:TextBox ID="txtRollPallet" runat="server" CssClass="form-control numericNoMask required"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group ">
                <span class="requiredspan markrequired">*</span>  <label id="lblStandardCostPerCostingUnit" class="control-label">Standard Cost per 1000 Eaches:</label>
                <div class="input-group"><span class="input-group-addon">$</span>
                    <asp:TextBox ID="txtStandard" ClientIDMode="static" runat="server" CssClass="form-control currencyMask required"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Attachments: </h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-3 col-md-4">
            <label class="control-label">Costing Contract with Vendor:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">  
            <input id="btnUploadNLEA" type="button" class="ButtonControlAutoSize" value="Upload Costing Contract with Vendor"  onclick="openBasicDialogPI('Upload Costing Contract with Vendor','Costing'); return false;" />  
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table>
                <asp:Repeater ID="rpAttachments" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>Action</th>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkFileDelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkFileDelete_Click" CausesValidation="false"></asp:LinkButton>
                            </td>
                            <td>
                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row attachment bracketAttachment">
        <div class="col-xs-12 col-sm-3 col-md-4">
            <span class="markrequired">*</span><label class="control-label">Attachment(s) for Bracket Pricing:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">  
            <input id="btnUploadBracketPricing" type="button" class="ButtonControlAutoSize" value="Upload Bracket Pricing Attachment(s)"  onclick="openBasicDialogPI('Upload Bracket Pricing Attachment(s)', 'BracketPricing'); return false;" />  
        </div>
    </div>
    <div class="row attachment">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table width="50%" id="bracketAttachmentTable">
                <asp:Repeater ID="rptBracketPricing"  ClientIDMode="Static" runat="server" >
                <HeaderTemplate>
                    <tr>
                        <th>Action</th>
                        <th>Document Name</th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnkBracketDelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkBracketDelete_Click" CausesValidation="false"></asp:LinkButton>
                        </td>
                        <td>
                            <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                        </td>
                    </tr>
                </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>       
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingItem" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnComponentType" runat="server" />
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-1">
            <asp:Button ID="btnSave" runat="server" CssClass="clickSave button justifyRight" ClientIDMode="Static" Text="Save" OnClick="btnSave_Click"  />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" CssClass="clickSave button justifyRight" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return ValidateData();"  />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        updateVendorLabels();
        bracketPricingReq();
    }); 
    function plural(noun) {
        var last1, last2, previous;
        last1 = noun.substr(noun.length - 1, 1);
        last2 = noun.substr(noun.length - 2, 2);
        previous = noun.substr(noun.length - 2, 1);
        if ('ss,sh,ch'.indexOf(last2) >= 0 || 'x,z,o'.indexOf(last1) >= 0)
            return noun + 'es';
        else if (last2 == 'fe')
            return noun.substr(0, noun.length - 2) + 'ves';
        else if (last1 == 'f')
            return noun.substr(0, noun.length - 1) + 'ves';
        else if (last1 == 'y' && 'aeiou'.indexOf(previous) == -1)
            return noun.substr(0, noun.length - 1) + 'ies';
        else if (last2 == 'us')
            return noun.substr(0, noun.length - 2) + 'i';
        else if (last2 == 'is')
            return noun.substr(0, noun.length - 2) + 'es';
        else if (last2 == 'on')
            return noun.substr(0, noun.length - 2) + 'a';
        else
            return noun + 's';
    }
    function updateVendorLabels() {
        var ddlCostingUnit = $get('<%= ddlCostingUnit.ClientID %>');
        var lblEachesPerCostingUnit = $get('lblEachesPerCostingUnit');
        var lblLBPerCostingUnit = $get('lblLBPerCostingUnit');
        var lblCostingUnitPerPallet = $get('lblCostingUnitPerPallet');
        var lblStandardCostPerCostingUnit = $get('lblStandardCostPerCostingUnit');
        var costingUnit;
        costingUnit = ddlCostingUnit.options[ddlCostingUnit.selectedIndex].text;
        if (costingUnit == "Box" || costingUnit == "Roll") {
            lblEachesPerCostingUnit.innerText = 'Eaches per ' + costingUnit + " (Rounding Value): ";
        } else {
            lblEachesPerCostingUnit.innerText = 'Eaches per ' + costingUnit + ": ";
        }
        lblLBPerCostingUnit.innerText = 'LB per ' + costingUnit + ": ";
        if (costingUnit == "Bundle" || costingUnit == "Case") {
            lblCostingUnitPerPallet.innerText = plural(costingUnit) + ' per Pallet (Rounding Value):';
        } else {
            lblCostingUnitPerPallet.innerText = plural(costingUnit) + ' per Pallet:';
        }
        lblStandardCostPerCostingUnit.innerText = 'Standard Cost per ' + costingUnit;
    }
    function bracketPricingReq() {
        if ($("#ddlBracketPricing option:selected").text() == "Yes") {
            $(".bracketAttachment .markrequired").removeClass("hideItem");
        } else {
            $(".bracketAttachment .markrequired").addClass("hideItem");
        }
    }
</script>