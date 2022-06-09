<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransferSemiBOMs.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.TransferSemiBOMs" %>

<asp:Repeater ID="rptTransferSemi" runat="server" OnItemDataBound="rptTransferSemi_ItemDataBound">
<ItemTemplate>
    <h3 class="accordion">Transfer Semi: <%# DataBinder.Eval(Container.DataItem, "MaterialNumber") + " " + DataBinder.Eval(Container.DataItem, "MaterialDescription") %></h3>
    <div class="panel" style="background-color:#BCD3F2;">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table class="gridTable">
                    <asp:Repeater ID="rptTransferSemiChildren" runat="server">
                        <HeaderTemplate>
                            <tr class="gridRow2">
                            <th class="gridCellH gridRow2">Material Number</th>
                            <th class="gridCellH gridRow2">Material Description</th>
                            <th class="gridCellH gridRow2">Make Location</th>
                            <th class="gridCellH gridRow2">Pack Location</th>
                            <th class="gridCellH gridRow2">Transfer SEMI Make Pack Locations</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblMaterialNumber" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblMaterialDescription" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblMakeLocation" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem, "MakeLocation") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblPackLocation" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem, "PackLocation") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblTransferLocations" runat="server"
                                Text='<%# DataBinder.Eval(Container.DataItem, "TransferSEMIMakePackLocations") %>'></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:Label ID="lblNoComponentsfound" runat="server" Text="No Components Found" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</ItemTemplate>
</asp:Repeater>