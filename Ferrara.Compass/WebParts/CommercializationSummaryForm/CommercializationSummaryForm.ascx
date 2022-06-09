<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommercializationSummaryForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.CommercializationSummaryForm.CommercializationSummaryForm" %>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js"></script>
<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" type="text/css" rel="Stylesheet" />

<div class="container">
    <h1>Commercialization Summary Form</h1>
    <div class="PageSubHeader">
        <asp:Label CssClass="control-label" ID="lblTitle" runat="server"></asp:Label>
    </div>
    <div class="PageSubHeader">
        <asp:Label CssClass="control-label" ID="lblProjectType" runat="server"></asp:Label>
    </div>
    <div class="PageSubHeader" id="divProjectTypeSubCategory" runat="server">
        <asp:Label CssClass="control-label" ID="lblProjectTypeSubCategory" runat="server"></asp:Label>
    </div>
    <asp:Panel ID="projectStatusLink" runat="server" ClientIDMode="Static"></asp:Panel>
    <asp:Panel ID="BOMPE2Link" runat="server" ClientIDMode="Static"></asp:Panel>
    <asp:Panel ID="commercializationPanel" runat="server" ClientIDMode="Static"></asp:Panel>
    <asp:Panel ID="MixesPanel" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <h2>Mixes</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <div class='CompassSeparator'>&nbsp;</div>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:HiddenField ID="hddRetailSellingUnitsPerBaseUOM" runat="server" />
                    <table class="gridTable">
                        <asp:Repeater ID="rpMixesSummary" runat="server">
                            <HeaderTemplate>
                                <tr>
                                    <th class="gridCellH">Item # in Mix</th>
                                    <th class="gridCellH">Item Description</th>
                                    <th class="gridCellH">Total Pieces per Selling Unit</th>
                                    <th class="gridCellH">Ounces per Piece</th>
                                    <th class="gridCellH">Ounces per Selling Unit</th>
                                    <th class="gridCellH">Grams per Selling Unit</th>
                                    <th class="gridCellH">Qty for Mix</th>
                                    <th class="gridCellH">Lbs for FG BOM</th>
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
                                    <td class="gridCell">
                                        <asp:Label CssClass="summary" ID="Label5" runat="server"
                                            Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NumberOfPieces")) *
                                            Convert.ToDouble(DataBinder.Eval(Container.DataItem, "OuncesPerPiece")) * 28.3495 %>'></asp:Label></td>
                                    <td class="gridCell">
                                        <asp:Label CssClass="summary" ID="Label4" runat="server"
                                            Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NumberOfPieces")) *
                                            Convert.ToDouble(hddRetailSellingUnitsPerBaseUOM.Value) %>'></asp:Label></td>
                                    <td class="gridCell">
                                        <asp:Label CssClass="summary" ID="Label3" runat="server"
                                            Text='<%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NumberOfPieces")) *
                                            Convert.ToDouble(DataBinder.Eval(Container.DataItem, "OuncesPerPiece")) *
                                            Convert.ToDouble(hddRetailSellingUnitsPerBaseUOM.Value) / 16.0).ToString("0.00") %>'></asp:Label></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="ShippersPanel" runat="server" Visible="false">
        <table>
            <tr>
                <td>
                    <h2>Shippers</h2>
                </td>
            </tr>
            <tr>
                <td>
                    <div class='CompassSeparator'>&nbsp;</div>
                </td>
            </tr>
            <tr>
                <td>
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
                                    <th class="gridCellH">Claim Bio-Eng?</th>
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
                                    <td class="gridCell">
                                        <asp:Label CssClass="summary" ID="lblIngredientsNeedToClaimBioEng" runat="server"
                                            Text='<%# DataBinder.Eval(Container.DataItem, "IngredientsNeedToClaimBioEng") %>'></asp:Label></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
            </tr>
        </table>
    </asp:Panel>
</div>
