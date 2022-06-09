<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DistributionForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.DistributionForm.DistributionForm" %>

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
            <h1>Distribution</h1>
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
    <%--Project Details--%>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Project Details</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Type:</label>
                <asp:TextBox ID="lblProjectType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Type Subcategory:</label>
                <asp:TextBox ID="lblProjectTypeSubCategory" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="divSoldOutsideUSA" runat="server">
            <div class="form-group">
                <label class="control-label">Sold Outside of USA?:</label>
                <asp:TextBox ID="lblSoldOutsideUSA" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="divCountryOfSale" runat="server" class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Country of Sale:</label>
                <asp:TextBox ID="lblCountryOfSale" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div id="divLikeItemNumber" runat="server" class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">"Like" Item #:</label>
                <asp:TextBox ID="lblLikeItemNumber" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div id="divLikeItemDescription" runat="server" class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">"Like" Item Description:</label>
                <asp:TextBox ID="lblLikeItemDescription" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-12">
            <div class="form-group">
                <label class="control-label">Item Concept:</label>
                <asp:TextBox ID="lblItemConcept" ClientIDMode="Static" TextMode="MultiLine" Rows="3" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <%--Logistics Information--%>
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
                <asp:TextBox ID="lblManufacturingLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Finished Good Pack Location:</label>
                <asp:TextBox ID="lblPackingLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="divProcurementType" runat="server">
            <div class="form-group">
                <label class="control-label">Procurement Type:</label>
                <asp:TextBox ID="lblProcurementType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4" id="divExternalManufacturer" runat="server">
            <div class="form-group">
                <label class="control-label">External Manufacturer:</label>
                <asp:TextBox ID="lblExternalManufacturer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="divExternalPacker" runat="server">
            <div class="form-group">
                <label class="control-label">External Packer:</label>
                <asp:TextBox ID="lblExternalPacker" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="divPurchasedIntoLocation" runat="server">
            <div class="form-group">
                <label class="control-label">Purchase Into Location:</label>
                <asp:TextBox ID="lblPurchasedIntoLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">SAP Base UoM:</label>
                <asp:TextBox ID="lblSAPBaseUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <%--Item Hierarchy--%>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Item Hierarchy</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                <asp:TextBox ID="lblProductHierarchyLevel1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 2:</label>
                <asp:TextBox ID="lblProductHierarchyLevel2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 1 (Brand):</label>
                <asp:TextBox ID="lblMaterialGroup1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 4 (Product Form):</label>
                <asp:TextBox ID="lblMaterialGroup4" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 5 (Pack Type):</label>
                <asp:TextBox ID="lblMaterialGroup5" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <%--Mixes--%>
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
                    <asp:Repeater ID="rpMixesSummary" runat="server" OnItemDataBound="rpMixesSummary_ItemDataBound">
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
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ItemNumber") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblMixItemDescription" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "ItemDescription") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="Label1" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "NumberOfPieces") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="Label2" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "OuncesPerPiece") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblOzPerPiece" runat="server">
                                    </asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <%--Shippers--%>
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
                <table>
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
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumber") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGItemDescription" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemDescription") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGItemQuantity" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumberUnits") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGouncesPerUnit" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit") %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGouncesPerFGunit" runat="server"
                                        Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "FGItemNumberUnits")) *
                                    Convert.ToDouble(DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit")) %>'>
                                    </asp:Label>
                                </td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblFGPackUnit" runat="server"
                                        Text='<%# DataBinder.Eval(Container.DataItem, "FGPackUnit") %>'>
                                    </asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <%--Deployment Information--%>
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
                <span class="markrequired">*</span><label class="control-label">Designate HUB DC (aka Material: Delivery Plant):</label>
                <asp:DropDownList ID="ddlDesignateHUBDC" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="divDeploymentModeofItem">
                <span class="markrequired">*</span><label class="control-label">What is the Deployment Mode of Item?:</label>
                <asp:DropDownList ID="ddlDeploymentModeofItem" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div id="divSELLDCs" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoSL07">
                    <span class="markrequired">*</span><label class="control-label">Extend to SL07 (Dallas):</label>
                    <asp:DropDownList ID="ddlExtendtoSL07" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetSL07SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set SL07 (Dallas) SPK to:</label>
                    <asp:DropDownList ID="ddlSetSL07SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoSL13">
                    <span class="markrequired">*</span><label class="control-label">Extend to SL13 (Bolingbrook):</label>
                    <asp:DropDownList ID="ddlExtendtoSL13" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetSL13SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set SL13 (Bolingbrook) SPK to:</label>
                    <asp:DropDownList ID="ddlSetSL13SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoSL18">
                    <span class="markrequired">*</span><label class="control-label">Extend to SL18 (Jonestown):</label>
                    <asp:DropDownList ID="ddlExtendtoSL18" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetSL18SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set SL18 (Jonestown) SPK to:</label>
                    <asp:DropDownList ID="ddlSetSL18SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoSL19">
                    <span class="markrequired">*</span><label class="control-label">Extend to SL19 (Phoenix):</label>
                    <asp:DropDownList ID="ddlExtendtoSL19" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetSL19SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set SL19 (Phoenix) SPK to:</label>
                    <asp:DropDownList ID="ddlSetSL19SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoSL30">
                    <span class="markrequired">*</span><label class="control-label">Extend to SL30 (Atlanta):</label>
                    <asp:DropDownList ID="ddlExtendtoSL30" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetSL30SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set SL30 (Atlanta) SPK to:</label>
                    <asp:DropDownList ID="ddlSetSL30SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
         <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoSL14">
                    <span class="markrequired">*</span><label class="control-label">Extend to SL41 (DeKalb):</label>
                    <asp:DropDownList ID="ddlExtendtoSL14" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetSL14SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set SL41 (DeKalb) SPK to:</label>
                    <asp:DropDownList ID="ddlSetSL14SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
    </div>
    <div id="DivFERQDCs" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoFQ26">
                    <span class="markrequired">*</span><label class="control-label">Extend to FQ26 (Louisville):</label>
                    <asp:DropDownList ID="ddlExtendtoFQ26" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetFQ26SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set FQ26 (Louisville) SPK to:</label>
                    <asp:DropDownList ID="ddlSetFQ26SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoFQ27">
                    <span class="markrequired">*</span><label class="control-label">Extend to FQ27 (Louisville):</label>
                    <asp:DropDownList ID="ddlExtendtoFQ27" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetFQ27SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set FQ27 (Louisville) SPK to:</label>
                    <asp:DropDownList ID="ddlSetFQ27SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoFQ28">
                    <span class="markrequired">*</span><label class="control-label">Extend to FQ28 (Triways):</label>
                    <asp:DropDownList ID="ddlExtendtoFQ28" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetFQ28SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set FQ28 (Triways) SPK to:</label>
                    <asp:DropDownList ID="ddlSetFQ28SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoFQ29">
                    <span class="markrequired">*</span><label class="control-label">Extend to FQ29 (Digital Orders/Evans):</label>
                    <asp:DropDownList ID="ddlExtendtoFQ29" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetFQ29SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set FQ29 (Digital Orders/Evans) SPK to:</label>
                    <asp:DropDownList ID="ddlSetFQ29SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoFQ34">
                    <span class="markrequired">*</span><label class="control-label">Extend to FQ34 (Port Logistics):</label>
                    <asp:DropDownList ID="ddlExtendtoFQ34" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetFQ34SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set FQ34 (Port Logistics) SPK to:</label>
                    <asp:DropDownList ID="ddlSetFQ34SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExtendtoFQ35">
                    <span class="markrequired">*</span><label class="control-label">Extend to FQ35 (Advance):</label>
                    <asp:DropDownList ID="ddlExtendtoFQ35" onchange="DistributionFormConditionalChecks()" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSetFQ35SPKto">
                    <span class="markrequired">*</span><label class="control-label">Set FQ35 (Advance) SPK to:</label>
                    <asp:DropDownList ID="ddlSetFQ35SPKto" runat="server" AppendDataBoundItems="true" CssClass="required form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
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
        <div class="col-xs-12 col-sm-6 col-md-4"></div>
        <div class="col-xs-12 col-sm-6 col-md-4"></div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn" CausesValidation="false" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="ButtonControl" OnClientClick="return ValidateData()" />
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
<script type="text/javascript">
    $(document).ready(function () {
        DistributionFormConditionalChecks();
    });

    //SAP Nomenclature
    function DistributionFormConditionalChecks() {
        if ($('#ddlExtendtoSL07 option:selected').text() == "Yes") {
            $('#divSetSL07SPKto').removeClass('hideItem');
        } else {
            $('#divSetSL07SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoSL13 option:selected').text() == "Yes") {
            $('#divSetSL13SPKto').removeClass('hideItem');
        } else {
            $('#divSetSL13SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoSL18 option:selected').text() == "Yes") {
            $('#divSetSL18SPKto').removeClass('hideItem');
        } else {
            $('#divSetSL18SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoSL19 option:selected').text() == "Yes") {
            $('#divSetSL19SPKto').removeClass('hideItem');
        } else {
            $('#divSetSL19SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoSL30 option:selected').text() == "Yes") {
            $('#divSetSL30SPKto').removeClass('hideItem');
        } else {
            $('#divSetSL30SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoSL14 option:selected').text() == "Yes") {
            $('#divSetSL14SPKto').removeClass('hideItem');
        } else {
            $('#divSetSL14SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoFQ26 option:selected').text() == "Yes") {
            $('#divSetFQ26SPKto').removeClass('hideItem');
        } else {
            $('#divSetFQ26SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoFQ27 option:selected').text() == "Yes") {
            $('#divSetFQ27SPKto').removeClass('hideItem');
        } else {
            $('#divSetFQ27SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoFQ28 option:selected').text() == "Yes") {
            $('#divSetFQ28SPKto').removeClass('hideItem');
        } else {
            $('#divSetFQ28SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoFQ29 option:selected').text() == "Yes") {
            $('#divSetFQ29SPKto').removeClass('hideItem');
        } else {
            $('#divSetFQ29SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoFQ34 option:selected').text() == "Yes") {
            $('#divSetFQ34SPKto').removeClass('hideItem');
        } else {
            $('#divSetFQ34SPKto').addClass('hideItem');
        }

        if ($('#ddlExtendtoFQ35 option:selected').text() == "Yes") {
            $('#divSetFQ35SPKto').removeClass('hideItem');
        } else {
            $('#divSetFQ35SPKto').addClass('hideItem');
        }
    }

</script>
