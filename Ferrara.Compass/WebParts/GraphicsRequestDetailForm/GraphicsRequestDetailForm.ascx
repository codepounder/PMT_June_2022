<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GraphicsRequestDetailForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.GraphicsRequestDetailForm.GraphicsRequestDetailForm" %>

<div class="container">
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Graphics Request Details</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">1. Submitted By:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblSubmittedBy" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">2. Date Graphic Request Form Submitted:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblDateSubmitted" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">3. First Production Date:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblFirstProductionDate" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">4. First Ship Date:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblFirstShipDate" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">5. Project Number:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblProjectNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">6. Product Hierarchy Level 1/Line of Business:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLineOfBusiness" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">7. Season:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblSeason" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">8. Customer:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblCustomer" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">9. Material Group 1 (Brand):</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblBrand" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">10. Packaging Engineer:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPackagingEngineer" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">11. Is Item externally manufactured or packaged?:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblExternallyManufacturedPackaged" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">12. Make Location:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblMakeLocation" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">13. Pack Location:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPackLocation" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">14. Item #:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblItemNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">15. Item Description:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblItemDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">16. Material #:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblMaterialNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">17. Material Description:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblMaterialDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">18. Printer:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPrinter" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">19. Component Type:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblComponentType" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>

    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">20. Old Material Number:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldMaterialNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">21. Old Material Description:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldMaterialDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">22. Substrate:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblSubstrate" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">23. Substrate Color:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblSubstrateColor" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">24. Print Style:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPrintStyle" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">25. Direct Print, Label or Offset:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="Label1" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">26. Unwind #:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblUnwindNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">27. Unit UPC:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblUnitUPC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">28. Jar/Display UPC:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblJarDispalyUPC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">29. Case UCC:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblCaseUCC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">30. Pallet UCC:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPalletUCC" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">30. Old Finished Good Item Number:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldFGItemNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">31. Old Finished Good Item Description:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblOldFGItemDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">32. Like Material Number:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLikeMaterialNumber" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">33. Like Material Description:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLikeMaterialDescription" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">34. How is it a Like Component Number:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblLikeMaterialReason" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">35. Reg Sheet:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rpNLEA" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hyperFile" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank"
                                    Text='<%#Eval("FileName")%>'></asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">36. Dieline(s):</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rpDielines" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hyperFile" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank"
                                    Text='<%#Eval("FileName")%>'></asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">37. Seal Info:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblBackSeam" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">38. Is Pallet Pattern Changing?:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblPalletPatternChange" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">39. Pallet Pattern:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rpPalletPattern" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hyperFile" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank"
                                    Text='<%#Eval("FileName")%>'></asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">40. Marketing Claims Labeling Requirements:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblMarketClaimsLabelingRequirements" runat="server" Text="Label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">41. Component Graphics Brief (Marketing):</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblGraphicsBrief" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">42. Project Direction/Notes (Marketing):</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblProjectNotesMarketing" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">43. Visual Reference/Rendering:</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <table>
                <asp:Repeater ID="rpRenderings" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="hyperFile" runat="server" NavigateUrl='<%#Eval("FileUrl")%>' Target="_blank"
                                    Text='<%#Eval("FileName")%>'></asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">44. Component Notes (PE):</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblComponentNotesPE" runat="server" CssClass="control-label"></asp:Label>
        </div>
    </div>
    <div class="row" style="background-color: #BCD3F2;">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">45. Graphics Vendor :</label>
        </div>
        <div class="col-xs-12 col-sm-8 col-md-8">
            <asp:Label ID="lblGraphicsVendor" runat="server" CssClass="control-label"></asp:Label>
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
            <asp:HiddenField ID="hiddenPackagingItemId" ClientIDMode="Static" runat="server" />
        </div>
    </div>
</div>
