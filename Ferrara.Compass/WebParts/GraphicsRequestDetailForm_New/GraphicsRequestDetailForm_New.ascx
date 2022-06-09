<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GraphicsRequestDetailForm_New.ascx.cs" Inherits="Ferrara.Compass.WebParts.GraphicsRequestDetailForm_New.GraphicsRequestDetailForm_New" %>
<style>
    .GraphicsAlternateBackground {
        background-color: #BCD3F2;
    }
</style>
<div class="container">
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Graphics Request Details</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblSubmittedByText" class="control-label" Text="Submitted By:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblSubmittedBy" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblDateSubmittedText" class="control-label" Text="Date Graphic Request Form Submitted:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblDateSubmitted" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblFirstProductionDateText" class="control-label" Text="Revised First Production Date:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblFirstProductionDate" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblFirstShipDateText" class="control-label" Text="Revised First Ship Date:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblFirstShipDate" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblProjectNumberText" class="control-label" Text="Project Number:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblProjectNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblLineOfBusinessText" class="control-label" Text="Product Hierarchy Level 1/Line of Business:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLineOfBusiness" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblSeasonText" class="control-label" Text="Season:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblSeason" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblCustomerText" class="control-label" Text="Customer:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblCustomer" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblBrandText" class="control-label" Text="Material Group 1 (Brand):"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblBrand" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPackagingEngineerText" class="control-label" Text="Packaging Engineer:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPackagingEngineer" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblExternallyManufacturedPackagedText" class="control-label" Text="Is Item externally manufactured or packaged?:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblExternallyManufacturedPackaged" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div id="divExternalManufacturer" runat="server" class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblExternalManufacturerText" class="control-label" Text="External Manufacturer:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblExternalManufacturer" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div id="divExternalPacker" runat="server" class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblExternalPackerText" class="control-label" Text="External Packer:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblExternalPacker" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblMakeLocationText" class="control-label" Text="FCC Make Location:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblMakeLocation" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPackLocationText" class="control-label" Text="FCC Pack Location:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPackLocation" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblItemNumberText" class="control-label" Text="Item #:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblItemNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblItemDescriptionText" class="control-label" Text="Item Description:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblItemDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblMaterialNumberText" class="control-label" Text="Material #:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblMaterialNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblMaterialDescriptionText" class="control-label" Text="Material Description:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblMaterialDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPrinterText" class="control-label" Text="Printer:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPrinter" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblComponentTypeText" class="control-label" Text="Component Type:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblComponentType" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblOldMaterialNumberText" class="control-label" Text="Old Material Number:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldMaterialNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblOldMaterialDescriptionText" class="control-label" Text="Old Material Description:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldMaterialDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblLikeMaterialNumberText" class="control-label" Text="Like Material Number:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLikeMaterialNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblLikeMaterialDescriptionText" class="control-label" Text="Like Material Description:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLikeMaterialDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblLikeMaterialReasonText" class="control-label" Text="How is it a Like Component Number:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLikeMaterialReason" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblOldFGItemNumberText" class="control-label" Text="Old Finished Good Item Number:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldFGItemNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblOldFGItemDescriptionText" class="control-label" Text="Old Finished Good Item Description:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldFGItemDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground" id="div14DigitBarcode" runat="server" visible="false">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lbl14DigitBarcodeText" class="control-label" Text="14 Digit Barcode:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lbl14DigitBarcode" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPrintStyleText" class="control-label" Text="Print Style:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPrintStyle" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblUnitUPCText" class="control-label" Text="Unit UPC:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblUnitUPC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblJarDispalyUPCText" class="control-label" Text="Jar/Display UPC:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblJarDispalyUPC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblUPCAssociatedWithThisPackagingComponentText" class="control-label" Text="UPC Associated with this Packaging Component:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblUPCAssociatedWithThisPackagingComponent" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblCaseUCCText" class="control-label" Text="Case UCC:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblCaseUCC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPalletUCCText" class="control-label" Text="Pallet UCC:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPalletUCC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblRegSheetText" class="control-label" Text="Reg Sheet:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rpNLEA" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hyperFile" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank" Text='<%#Eval("FileName")%>'></asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPackagingSpecificationNumberText" class="control-label" Text="Packaging Specification Number:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPackagingSpecificationNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblHyperlinkToDielineInPLMText" class="control-label" Text="Hyperlink to Dieline in PLM:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <strong><a id="anHyperlinktoDielineInPLM" clientidmode="Static" runat="server" target="_blank" href=""></a></strong>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblDieLineText" class="control-label" Text="Dieline Attachment:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rpDielines" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hyperFile" NavigateUrl='<%#Eval("FileUrl")%>' Text='<%#Eval("FileName")%>' runat="server" Target="_blank"></asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <%--<div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPalletPatternText" class="control-label" Text="Pallet Pattern:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table style="width: 100%; border: thin">
                <asp:Repeater ID="rpPalletPattern" OnItemDataBound="rpPalletPattern_ItemDataBound" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <td>
                                <label class="control-label">Pallet Specification Hyperlink</label>
                            </td>
                            <td>
                                <label class="control-label">Pallet Specification Number</label>
                            </td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hlPalletSpecHyperlink" runat="server" Target="_blank"></asp:HyperLink>
                            </td>
                            <td>
                                <asp:Label ID="lblPalletSpecNumber" Text='<%# DataBinder.Eval(Container.DataItem, "PalletSpecNumber") %>' CssClass="form-control" runat="server" ClientIDMode="Static"></asp:Label>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>--%>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPalletSpecificationHyperlinkText" class="control-label" Text="Pallet Specification Hyperlink:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <div class="col-xs-12 col-sm-8 col-md-8">
                <strong>
                    <asp:HyperLink ID="hlPalletSpecHyperlink" CssClass="hideItem" ClientIDMode="Static" Target="_blank" NavigateUrl="" Text="" runat="server"></asp:HyperLink>
                </strong>
            </div>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblPalletSpecificationNumberText" class="control-label" Text="Pallet Specification Number:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPalletSpecificationNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblGraphicsBriefText" class="control-label" Text="Component Graphics Brief (Marketing):"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblGraphicsBrief" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblRequireBELabelingText" class="control-label" Text="Does this material require BioEngineering (BE) Labeling?:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblRequireBELabeling" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <asp:Label runat="server" ID="lblBEQRCodeFileText" class="control-label" Text="BioEngineered (BE) QR Code File:"> </asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rptBEQRCodeFiles" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hlBEQRCodeFile" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank" Text='<%#Eval("FileName")%>'></asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4" style="font-weight: 700">
            <asp:Label runat="server" ID="lblProjectNotesMarketingText" class="control-label" Text="Project Direction/Notes (Marketing):"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblProjectNotesMarketing" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row GraphicsBackground GraphicsOnly">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <asp:Label runat="server" ID="lblRenderingsText" class="control-label" Text="Visual Reference/Rendering:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rpRenderings" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hyperFile" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank" Text='<%#Eval("FileName")%>'></asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row GraphicsBackground">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <asp:Label runat="server" ID="lblApprovedGraphicAssetText" class="control-label" Text="Approved Graphic Asset:"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rptApprovedGraphicAsset" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hlApprovedGraphicAsset" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank" Text='<%#Eval("FileName")%>'></asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-3 col-md-11">
            &nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnGraphicsRequest" runat="server" Text="Graphics Request" OnClick="btnGraphicsRequest_Click" CssClass="ButtonControlAutoSize" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnProjectType" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenPackagingItemId" ClientIDMode="Static" runat="server" />
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('.GraphicsBackground').each(function (i, obj) {
            if (i % 2 == 0) {
                $(this).addClass("GraphicsAlternateBackground");
            }
            else {
                $(this).removeClass("GraphicsAlternateBackground");
            }
        });

        if ($('#hdnProjectType').val() == "Graphics Change Only") {
            $('.GraphicsOnly').each(function (i, obj) {
                $(this).addClass("hideItem");
            });
        }
    });
</script>
