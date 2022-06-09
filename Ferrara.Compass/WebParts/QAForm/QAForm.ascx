<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QAForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.QAForm.QAForm" %>

<script type="text/javascript">
    $(document).ready(function () {
        var cust = $('#lblSecondaryPackLocation').val();
        if (cust == "Select...") {
            $('#dvPackLocation').addClass('hidden');
        }
        else {
            $('#dvPackLocation').removeClass('hidden');
        }
        if ($(".repeater.divShipper").length) {
            $("#btnUploadNLEA").closest(".row").find(".markrequired").hide();
        } else {
            $("#btnUploadNLEA").closest(".row").find(".markrequired").show();
        }
        if ($('#txtMadeInUSA').val().toLocaleLowerCase() == 'yes') {
            $('#dvMadeInUSAClaim').removeClass('hidden');
        }
        else {
            $('#dvMadeInUSAClaim').addClass('hidden');
        }
    });
</script>
<style>
    #dvCandy .ButtonControl, #dvPurchased .ButtonControl {
        margin-left: -5px;
    }

    .BOMDelete {
        display: none;
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
            <h1>InTech Regulatory Form</h1>
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
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
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
                <asp:TextBox ID="txtComments" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">PM:</label>
                <asp:TextBox ID="lblPM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Brand Manager:</label>
                <asp:TextBox ID="lblBrandManager" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <asp:HiddenField ID="hiddenManufacturingLocation" runat="server" ClientIDMode="Static" />
                <label class="control-label">Make Location:</label>
                <asp:TextBox ID="lblManufacturingLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Make Country of Origin:</label>
                <asp:TextBox ID="lblManufacturingCountryOfOrigin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Finished Good Pack Location:</label>
                <asp:TextBox ID="lblPrimaryPackLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6" id="dvPackLocation">
            <%--<div class="form-group">
                <asp:label runat="server" ID="lblSecondaryPackLocationLabel" Font-Bold="true" CssClass="control-label">2nd Pack Location:</asp:label>
                <asp:TextBox ID="lblSecondaryPackLocation" ClientIDMode="Static" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>--%>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Procurement Type:</label>
                <asp:TextBox ID="txtCoManufacturingClassification" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Case Type:</label>
                <asp:TextBox ID="txtCaseType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row marketingClaims">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Marketing Claims/Labeling:</label>
                <asp:TextBox ID="lblMarketingClaims" ClientIDMode="Static" TextMode="MultiLine" Rows="6" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                <asp:HiddenField ID="hdnDesiredClaims" ClientIDMode="Static" runat="server"></asp:HiddenField>
            </div>
        </div>
    </div>
    <div class="row marketingClaims">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Sellable Unit:</label>
                <asp:TextBox ID="txtSellableUnit" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">New NLEA Format:</label>
                <asp:TextBox ID="txtNewNLEAFormat" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row marketingClaims">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Made In USA Claim:</label>
                <asp:TextBox ID="txtMadeInUSA" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6" id="dvMadeInUSAClaim">
            <div class="form-group">
                <label class="control-label">Made In USA Claim Details:</label>
                <asp:TextBox ID="txtMadeInUSAPct" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">GMO Claim:</label>
                <asp:TextBox ID="txtGMOClaim" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row marketingClaims">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Gluten Free:</label>
                <asp:TextBox ID="txtGlutenFree" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Organic:</label>
                <asp:TextBox ID="txtOrganic" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Natural Colors:</label>
                <asp:TextBox ID="txtNaturalColors" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Natural / Real Flavors Claims:</label>
                <asp:TextBox ID="txtNaturalFlavors" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row marketingClaims">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Fat Free:</label>
                <asp:TextBox ID="txtFatFree" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Preservative Free:</label>
                <asp:TextBox ID="txtPreservativeFree" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Lactose Free:</label>
                <asp:TextBox ID="txtLactoseFree" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Low Sodium:</label>
                <asp:TextBox ID="txtLowSodium" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Kosher:</label>
                <asp:TextBox ID="txtKosher" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row marketingClaims">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Juice Concentrate:</label>
                <asp:TextBox ID="txtJuiceConcentrate" ClientIDMode="Static" TextMode="MultiLine" Rows="6" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row marketingClaims">
        <span class="headSummary">Nutrients</span>
    </div>
    <div class="row marketingClaims">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Good / Excellent Source:</label>
                <asp:TextBox ID="txtGoodSource" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div id="divMixes" runat="server" visible="false">
        <asp:HiddenField ID="hddRetailSellingUnitsPerBaseUOM" runat="server" />
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
                                    <asp:Label CssClass="summary" ID="Label3" runat="server"
                                        Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NumberOfPieces")) *
                                        Convert.ToDouble(hddRetailSellingUnitsPerBaseUOM.Value) %>'></asp:Label></td>
                                <td class="gridCell">
                                    <asp:Label CssClass="summary" ID="lblLbsForFGBOM" runat="server"
                                        Text='<%# (Convert.ToInt32(DataBinder.Eval(Container.DataItem, "NumberOfPieces")) *
                                        Convert.ToDouble(DataBinder.Eval(Container.DataItem, "OuncesPerPiece")) *
                                        Convert.ToDouble(hddRetailSellingUnitsPerBaseUOM.Value) / 16.0).ToString("0.00") %>'></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div id="divShipper" runat="server" visible="false" class="repeater divShipper">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Shipper Details</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div id="FGError_Message"></div>
                <asp:HiddenField runat="server" ID="hdnMaterialGroup5" Value="" />
                <table id="tabShipper">
                    <asp:Repeater ID="rptShipper" runat="server" OnItemDataBound="rptShipper_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <th>FG Item # in Display</th>
                                <th>FG Item Description</th>
                                <th># of Units</th>
                                <th>Ounces per Unit</th>
                                <th>Ounces per FG Unit</th>
                                <th>Pack Unit</th>
                                <th><span class="markrequired">*</span>Shelf Life</th>
                                <th><span class="markrequired">*</span>Bio-Eng.?</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidShipperId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ItemId") %>' />
                                    <asp:HiddenField ID="hdnCompassListItemId" runat="server" Value=' <%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                    <asp:TextBox runat="server" ID="txtFGItemDisplay" ReadOnly="true" class="form-control minimumlength txtFGItemDisplay alphanumericNumeric" title="FG Item # in Display" MaxLength="6" value='<%# DataBinder.Eval(Container.DataItem, "FGItemNumber") %>' />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGItemDescription" ReadOnly="true" class="form-control txtFGItemDescription" value='<%# DataBinder.Eval(Container.DataItem, "FGItemDescription") %>' title="FG Item Description" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGnumberUnits" ReadOnly="true" class="form-control numericDecimal0" title="# of Units" oninput="updateOuncesPerFGunit(event, this);" value='<%# DataBinder.Eval(Container.DataItem, "FGItemNumberUnits") %>' ClientIDMode="Static" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGouncesPerUnit" ReadOnly="true" class="form-control numericDecimal2" title="Ounces per Unit" value='<%# DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit") %>' oninput="updateOuncesPerFGunit(event, this);" ClientIDMode="Static" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGouncesPerFGunit" ReadOnly="true" class="form-control" title="Ounces per FG Unit" value='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "FGItemNumberUnits")) * Convert.ToDouble(DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit")) %>' ClientIDMode="Static" />
                                </td>
                                <td style="width: 100px">
                                    <asp:DropDownList ID="ddlFGPackUnit" runat="server" readonly="true" class="form-control" ClientIDMode="Static" ToolTip="Pack Unit">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGShelfLife" class="form-control required numericDecimal0" title="Shelf Life" value='<%# DataBinder.Eval(Container.DataItem, "FGShelfLife") %>' ClientIDMode="Static" />
                                </td>
                                <td style="width: 100px">
                                    <asp:DropDownList ID="ddlIngredientsNeedToClaimBioEng" class="form-control required" runat="server" ClientIDMode="Static" ToolTip="Do any ingredients within this item need to claim Bio-Engineering?">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                        <asp:ListItem Value="No">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>

                        <td></td>
                        <td>Totals</td>
                        <td>
                            <asp:TextBox ID="txtFGTotalQuantityUnitsInDisplay" ClientIDMode="Static" runat="server"
                                CssClass="required numericNoMask form-control" ReadOnly="true" ToolTip="FG Total Quantity Units In Display"></asp:TextBox></td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtFGTotalTotalOuncesPerShipper" ClientIDMode="Static" runat="server"
                                CssClass="required numericNoMask form-control" ReadOnly="true" ToolTip="FG Total Ounces Per Shipper"></asp:TextBox></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div id="divFG" runat="server" visible="false" class="repeater divFG">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Finished Goods</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table id="tabFG">
                    <asp:Repeater ID="rptFG" runat="server" OnItemDataBound="rptFG_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <th>FG Item # in Display</th>
                                <th>FG Item Description</th>
                                <th># of Units</th>
                                <th>Ounces per Unit</th>
                                <th>Ounces per FG Unit</th>
                                <th>Pack Unit</th>
                                <th><span class="markrequired">*</span>Shelf Life</th>
                                <th><span class="markrequired">*</span>Bio-Eng.?</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidFGId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                    <asp:HiddenField ID="hdnCompassListItemId" runat="server" Value=' <%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                    <asp:TextBox runat="server" ID="txtFGItemDisplay" ReadOnly="true" class="form-control minimumlength txtFGItemDisplay alphanumericNumeric" title="FG Item # in Display" MaxLength="6" value='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGItemDescription" ReadOnly="true" class="form-control txtFGItemDescription" value='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' title="FG Item Description" /></td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGnumberUnits" ReadOnly="true" class="form-control numericDecimal0" title="# of Units" oninput="updateOuncesPerFGunit(event, this);" ClientIDMode="Static" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGouncesPerUnit" ReadOnly="true" class="form-control numericDecimal2" title="Ounces per Unit" oninput="updateOuncesPerFGunit(event, this);" ClientIDMode="Static" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGouncesPerFGunit" ReadOnly="true" class="form-control" title="Ounces per FG Unit" ClientIDMode="Static" />
                                </td>
                                <td style="width: 100px">
                                    <asp:DropDownList ID="ddlFGPackUnit" runat="server" readonly="true" class="form-control" ClientIDMode="Static" ToolTip="Pack Unit">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtFGShelfLife" class="form-control required numericDecimal0" title="Shelf Life" value='<%# DataBinder.Eval(Container.DataItem, "ShelfLife") %>' ClientIDMode="Static" />
                                </td>
                                <td style="width: 100px">
                                    <asp:DropDownList ID="ddlIngredientsNeedToClaimBioEng" runat="server" class="form-control" ClientIDMode="Static" ToolTip="Do any ingredients within this item need to claim Bio-Engineering?">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        <asp:ListItem Value="Yes">Yes</asp:ListItem>
                                        <asp:ListItem Value="No">No</asp:ListItem>
                                    </asp:DropDownList>
                                </td>

                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td></td>
                        <td>Totals</td>
                        <td>
                            <asp:TextBox ID="txtFGTotalQuantityUnitsInDisplayFG" ClientIDMode="Static" runat="server"
                                CssClass="required numericNoMask form-control" ReadOnly="true" ToolTip="FG Total Quantity Units In Display"></asp:TextBox></td>
                        <td></td>
                        <td>
                            <asp:TextBox ID="txtFGTotalTotalOuncesPerFG" ClientIDMode="Static" runat="server"
                                CssClass="required numericNoMask form-control" ReadOnly="true" ToolTip="FG Total Ounces Per Shipper"></asp:TextBox></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <div class="row TransferSemiDetailsHeader">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Transfer Semi Details</h2>
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
        <div class="col-xs-12 col-sm-6 col-md-3">
            <h2>Allergens</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Milk:</label>
                <asp:DropDownList ID="drpAllergenMilk" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Eggs:</label>
                <asp:DropDownList ID="drpAllergenEggs" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Peanuts:</label>
                <asp:DropDownList ID="drpAllergenPeanuts" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Coconut:</label>
                <asp:DropDownList ID="drpAllergenCoconut" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Almonds:</label>
                <asp:DropDownList ID="drpAllergenAlmonds" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Soy:</label>
                <asp:DropDownList ID="drpAllergenSoy" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Wheat:</label>
                <asp:DropDownList ID="drpAllergenWheat" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Hazelnuts:</label>
                <asp:DropDownList ID="drpAllergenHazelnuts" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Other Allergen(s):</label>
                <asp:TextBox ID="drpAllergenOther" ClientIDMode="Static" runat="server" CssClass="form-control" AppendDataBoundItems="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>InTech Regulatory Form - Candy Semis</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvCandy" class="repeater">
        <div class="form-group">
            <div id="CandySemiError_Message" runat="server"></div>
            <asp:Repeater ID="rptCandy" runat="server" OnItemCommand="rptCandy_ItemCommand" OnItemDataBound="rptCandy_ItemDataBound">
                <HeaderTemplate></HeaderTemplate>
                <ItemTemplate>
                    <div class="SAPVerifyItem">
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-12 col-md-2">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New or Existing?</label>
                                    <asp:DropDownList ID="drpNewExisting" CssClass="form-control QARequired VerifySAPNewExisting" onchange="IsInternalTransferSemiRequired(this);" title="New or Existing?" runat="server">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                        <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-3">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Candy Semi #</label>
                                    <asp:TextBox ID="txtMaterialNumber" runat="server" ToolTip="Candy Semi #" MaxLength="20" CssClass="QARequired transferSemi form-control alphanumericToUpper1 minimumlength txtMaterialNumber VerifySAPNumbers" value='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>'></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-12 col-md-1">
                                <div class="form-group">
                                    <label class="control-label">&nbsp;</label>
                                    <asp:Button runat="server" ID="btnFind" ClientIDMode="Static" OnClientClick="getCandySemiDescription(this,'txtMaterialNumber','txtMaterialDescription');return false;" CssClass="ButtonControl" Text="Find" OnClick="btnFind_Click" />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-5">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Candy Semi Description</label>
                                    <asp:TextBox ID="txtMaterialDescription" runat="server" ToolTip="Candy Semi Description" CssClass="txtMaterialDescription QARequired form-control" MaxLength="40" value='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-1">
                                <div class="form-group">
                                    <label class="control-label">Action</label>
                                    <asp:Button ID="btndelete" CausesValidation="false" CssClass="button ButtonControl" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" Text="Delete" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="lblCompDeleteErrorCandy" runat="server" Visible="false">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <label class="comment-block noResultsRow">Cannot delete this components as it has child elements.</label>
                            </div>
                        </asp:Panel>
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Transfer Parent</label>
                                    <asp:DropDownList ID="ddlTransfParent" CssClass="form-control" ToolTip="Transfer Semi Parent" runat="server"
                                        DataTextField="MaterialNumber" DataValueField="Id" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0">Finished Good</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Trials completed and product set aside for pack trials?:</label>
                                    <asp:DropDownList ID="ddlTrialsCompleted" CssClass="form-control" title="Trials completed and product set aside for pack trials?" runat="server">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New Formula?:</label>
                                    <asp:DropDownList ID="ddlNewFormula" CssClass="QARequired form-control" title="New Formula?" runat="server">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Shelf Life:</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtShelfLife" title="Shelf Life" runat="server" CssClass="QARequired form-control numericDecimal0" value='<%# DataBinder.Eval(Container.DataItem, "ShelfLife") %>'></asp:TextBox><span class="input-group-addon"> Days</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Do any ingredients within this item need to claim Bio-Engineering?:</label>
                                    <asp:DropDownList ID="ddlIngredientsNeedToClaimBioEng" runat="server" CssClass="QARequired form-control">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Flowthrough:</label>
                                    <asp:DropDownList ID="ddlFlowthrough" onchange="QAFlowthroughChange();" runat="server" CssClass="CandyFlowthrough form-control">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnFlowthrough" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <br />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">&nbsp;</label>
                <asp:Button runat="server" ID="btnAddItem" ClientIDMode="Static" OnClientClick="return addItemValidation();" CssClass="ButtonControlAutoSize" Text="Add Candy Semi" OnClick="btnAddItem_Click" />
                <asp:HiddenField ID="hdnTBDIndicator" ClientIDMode="Static" runat="server" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>InTech Regulatory Form - Purchased Candy Semis</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvPurchased" class="repeater">
        <div class="form-group">
            <div id="PurchasedSemiError_Message" runat="server"></div>
            <asp:Repeater ID="rptPurchased" runat="server" OnItemCommand="rptPurchased_ItemCommand" OnItemDataBound="rptPurchased_ItemDataBound">
                <HeaderTemplate></HeaderTemplate>
                <ItemTemplate>
                    <div class="SAPVerifyItem">
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-12 col-md-2">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New or Existing?</label>
                                    <asp:DropDownList ID="drpNewExisting" CssClass="form-control QARequired VerifySAPNewExisting" onchange="IsInternalTransferSemiRequired(this);" title="New or Existing?" runat="server">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                        <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                                        <asp:ListItem Text="Network Move" Value="Network Move"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-3">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Purchased Candy Semi #</label>
                                    <asp:TextBox ID="txtMaterialNumber" runat="server" ToolTip="Purchased Candy Semi #" MaxLength="20" CssClass="QARequired transferSemi form-control alphanumericToUpper1 minimumlength txtMaterialNumber VerifySAPNumbers" value='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>'></asp:TextBox>
                                </div>
                            </div>

                            <div class="col-xs-12 col-sm-12 col-md-1">
                                <div class="form-group">
                                    <label class="control-label">&nbsp;</label>
                                    <asp:Button runat="server" ID="btnFind" ClientIDMode="Static" OnClientClick="getCandySemiDescription(this,'txtMaterialNumber','txtMaterialDescription');return false;" CssClass="ButtonControl" Text="Find" OnClick="btnFind_Click" />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-5">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Purchased Candy Semi Description</label>
                                    <asp:TextBox ID="txtMaterialDescription" runat="server" ToolTip="Purchased Candy Semi Description" CssClass="txtMaterialDescription QARequired form-control" MaxLength="40" value='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-1">
                                <div class="form-group">
                                    <label class="control-label">Action</label>
                                    <asp:Button ID="btndelete" CausesValidation="false" CssClass="button ButtonControl" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" Text="Delete" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="lblCompDeleteErrorPURCandy" runat="server" Visible="false">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <label class="comment-block noResultsRow">Cannot delete this components as it has child elements.</label>
                            </div>
                        </asp:Panel>
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Purchased Parent</label>
                                    <asp:DropDownList ID="ddlTransfParent" CssClass="form-control" ToolTip="Purchased Candy Semi Parent" runat="server"
                                        DataTextField="MaterialNumber" DataValueField="Id" AppendDataBoundItems="true">
                                        <asp:ListItem Value="0">Finished Good</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Trials completed and product set aside for pack trials?:</label>
                                    <asp:DropDownList ID="ddlTrialsCompleted" CssClass="form-control" title="Trials completed and product set aside for pack trials?" runat="server">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>New Formula?:</label>
                                    <asp:DropDownList ID="ddlNewFormula" CssClass="QARequired form-control" title="New Formula?" runat="server">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Shelf Life:</label>
                                    <div class="input-group">
                                        <asp:TextBox ID="txtShelfLife" title="Shelf Life" runat="server" CssClass="QARequired form-control numericDecimal0" value='<%# DataBinder.Eval(Container.DataItem, "ShelfLife") %>'></asp:TextBox><span class="input-group-addon"> Days</span>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Immediate SPK Change:</label>
                                    <asp:TextBox ID="lblImmediateSPKChange" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" value='<%# DataBinder.Eval(Container.DataItem, "ImmediateSPKChange") %>'></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Flowthrough:</label>
                                    <asp:DropDownList ID="ddlFlowthrough" runat="server" CssClass="form-control" ClientIDMode="Static">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnFlowthrough" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                        <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Do any ingredients within this item need to claim Bio-Engineering?:</label>
                                    <asp:DropDownList ID="ddlIngredientsNeedToClaimBioEng" runat="server" CssClass="QARequired form-control">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        <asp:ListItem Text="Unknown" Value="Unknown"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <br />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Attachments</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-3 col-md-4">
            <label class="control-label"><span class="markrequired">*</span>Upload Documents:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">
            <input id="btnUploadNLEA" type="button" class="ButtonControlAutoSize" value="Upload NLEA Statement" onclick="openBasicDialog('Upload NLEA Statement', 'NLEA'); return false;" />
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
                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="ApprovedGraphicsAsset GraphicsOnly">
        <div class="row">
            <div class="col-xs-12 col-sm-3 col-md-4">
                <label class="control-label">Approved Graphic Asset for New Components:</label>
            </div>
            <%-- <div class="col-xs-12 col-sm-9 col-md-4">
                <input id="btnApprovedGraphicsAsset" type="button" class="ButtonControlAutoSize" value="Upload Approved Graphic Asset" onclick="openBasicDialog('Upload Approved Graphic Asset for New Components', 'ApprovedGraphicsAsset'); return false;" />
            </div>--%>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <table style="width: 100%;">
                    <asp:Repeater ID="rptApprovedGraphicsAsset" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>
                                <th>Document Name</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkDeleteApprovedGraphicsAsset" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteApprovedGraphicsAsset_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                                <td>
                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="row GraphicsOnly">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Is Regulatory information correct? :</label>
                <asp:DropDownList ID="ddlIsRegulatoryinformationCorrect" onchange="IsRegulatoryinformationCorrectChanged(this);" CssClass="form-control GraphicsRequired" ClientIDMode="Static" runat="server" ViewStateMode="Enabled">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 WhatRegulatoryInfoIsIncorrect">
            <div class="form-group">
                <label class="control-label">What Regulatory information is incorrect?:</label>
                <asp:TextBox ID="txtWhatRegulatoryInfoIsIncorrect" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control GraphicsBrief" Text='' ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row GraphicsOnly">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Do you approve this project to proceed?:</label>
                <asp:DropDownList ID="ddlDoYouApproveThisProjectToProceed" CssClass="form-control GraphicsRequired" ClientIDMode="Static" runat="server" ViewStateMode="Enabled">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
                <label class="comment-block" style="color: #777777;">
                    You would not approve this project if the changes required are more than just graphics changes OR if a mateiral needs to be updated that the IPF initiator did not intend to change.
                </label>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="ReloadAttachment HiddenButton hidebtn" OnClick="btnReloadAttachment_Click"></asp:Button>

            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="ButtonControl" CausesValidation="false" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="ButtonControl" OnClientClick="return ValidateData()" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hdnFinishedGoodCount" Value="0" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnProjectType" ClientIDMode="Static" runat="server" />
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
            <asp:HiddenField ID="hdnPLMProject" runat="server" ClientIDMode="Static" />
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
        onloadcheck();
        updateFGtotals();
        pageLoadCheck();
        HideTransferSemiHeader();

        $(".GraphicsOnly").each(function () {
            $(this).addClass('hideItem');
        });

        var projectType = $("#hdnProjectType").val();
        if (projectType == "Graphics Change Only") {
            $(".GraphicsOnly").each(function () {
                $(this).removeClass('hideItem');
                $(this).find(".GraphicsRequired").addClass("required");
                $(this).find(".GraphicsRequired").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            });
        }

        IsRegulatoryinformationCorrectChanged();
    });

    function IsRegulatoryinformationCorrectChanged() {
        var projectType = $("#hdnProjectType").val();
        $(".WhatRegulatoryInfoIsIncorrect").addClass('hideItem');
        $("#txtWhatRegulatoryInfoIsIncorrect").removeClass("required");
        $(".WhatRegulatoryInfoIsIncorrect").find(".markrequired").remove()
        $("#txtWhatRegulatoryInfoIsIncorrect").parent().addClass('hideItem');

        if (projectType == "Graphics Change Only") {
            if ($("#ddlIsRegulatoryinformationCorrect").val() == "No") {
                $(".WhatRegulatoryInfoIsIncorrect").removeClass('hideItem');
                $("#txtWhatRegulatoryInfoIsIncorrect").addClass("required");
                $("#txtWhatRegulatoryInfoIsIncorrect").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                $("#txtWhatRegulatoryInfoIsIncorrect").parent().removeClass('hideItem');
            } else {
                $("#txtWhatRegulatoryInfoIsIncorrect").val("");
            }
        }
    }

    function IsInternalTransferSemiRequired(arg) {
        var anchor = $("#" + arg.id);
        materialNumber = anchor.closest(".row").find(".txtMaterialNumber").val();
        if ($("#" + arg.id + " option:selected").text() == 'New') {
            if (materialNumber == "") {
                anchor.closest(".row").find(".txtMaterialNumber").val("NEEDS NEW");
            }
        }
        if ($("#" + arg.id + " option:selected").text() == 'Existing') {
            if (materialNumber == "NEEDS NEW" || materialNumber == "Needs New") {
                anchor.closest(".row").find(".txtMaterialNumber").val('');
            }
        }
    }

    function updateFGtotals() {
        var txtFGTotalQuantityUnitsInDisplay = $("#txtFGTotalQuantityUnitsInDisplay");
        var txtFGTotalTotalOuncesPerShipper = $("#txtFGTotalTotalOuncesPerShipper");
        var w, numberOfPieces, ouncesPerFGunit;
        numberOfPieces = 0;
        ouncesPerFGunit = 0;
        $("#tabShipper").find("tr").each(function (w, row) {
            txtFGnumberUnits = $(row).find("#txtFGnumberUnits");
            txtFGouncesPerFGunit = $(row).find("#txtFGouncesPerFGunit");
            if (txtFGnumberUnits.val() !== undefined && txtFGouncesPerFGunit.val() !== undefined) {
                if (txtFGnumberUnits.val() != '' && !isNaN(txtFGnumberUnits.val()))
                    numberOfPieces += parseInt(txtFGnumberUnits.val());
                if (txtFGouncesPerFGunit.val() != '' && !isNaN(txtFGouncesPerFGunit.val()))
                    ouncesPerFGunit += parseFloat(txtFGouncesPerFGunit.val());
            }
        });
        txtFGTotalQuantityUnitsInDisplay.val(numberOfPieces);
        txtFGTotalTotalOuncesPerShipper.val(ouncesPerFGunit.toFixed(2));

        var txtFGTotalQuantityUnitsInDisplayFG = $("#txtFGTotalQuantityUnitsInDisplayFG");
        var txtFGTotalTotalOuncesPerFG = $("#txtFGTotalTotalOuncesPerFG");
        var w, numberOfPieces, ouncesPerFGunit;
        numberOfPieces = 0;
        ouncesPerFGunit = 0;
        $("#tabFG").find("tr").each(function (w, row) {
            txtFGnumberUnits = $(row).find("#txtFGnumberUnits");
            txtFGouncesPerFGunit = $(row).find("#txtFGouncesPerFGunit");
            if (txtFGnumberUnits.val() !== undefined && txtFGouncesPerFGunit.val() !== undefined) {
                if (txtFGnumberUnits.val() != '' && !isNaN(txtFGnumberUnits.val()))
                    numberOfPieces += parseInt(txtFGnumberUnits.val());
                if (txtFGouncesPerFGunit.val() != '' && !isNaN(txtFGouncesPerFGunit.val()))
                    ouncesPerFGunit += parseFloat(txtFGouncesPerFGunit.val());
            }
        });
        txtFGTotalQuantityUnitsInDisplayFG.val(numberOfPieces);
        txtFGTotalTotalOuncesPerFG.val(ouncesPerFGunit.toFixed(2));
    }

    function addItemValidation() {
        var isValid = true;
        $(".QARequired").each(function (i, obj) {
            var id = $(this).attr('id');
            if ($(this).is('input')) {
                var sd = $(this).attr('title');
                var value = $(this).val();

                if (value == "") {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
            }
            else {
                var sd = $(this).attr('title');
                var value = $(this).val();
                if (value == '-1') {
                    isValid = false;
                    $("#dverror_message").show();
                    $('#error_message').append('<li style="padding-left:20px;float:left; display: inline;color:red">' + sd + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                }
            }
        });
        if (!isValid) {
            loadingIconAdded = true;
            $(".disablingLoadingIcon").remove();
            setFocusError();

            return false;
        }
        return BOMValidator(true);
    }

    function onloadcheck() {
        if ($("#hdnDesiredClaims").val() != "Yes") {
            $(".marketingClaims").addClass("hideItem");
        }
        $('.readOnly').each(function (i, obj) {
            //$(this).attr('disabled', 'disabled');
            $(this).attr('readonly', 'readonly');
            $(this).attr('disabled', 'disabled');
        });
        if ($("#hiddenManufacturingLocation").val().trim().indexOf('Externally Manufactured') != -1) {
            $("#ddlCoManufacturingClassification").parent().parent().show();
        } else {
            $("#ddlCoManufacturingClassification").parent().parent().hide();
        }
        if ($("#hiddenManufacturingLocation").val().trim().indexOf('Externally Manufactured') != -1) {
            if ($("#ddlCoManufacturingClassification option:selected").text().trim().indexOf('External Turnkey Semi') != -1) {
                $("#dvExistingBulkSemiNumber").show();
                $("#dvBulkSemiDescription").show();
            }
            else if ($("#ddlCoManufacturingClassification option:selected").text().trim().indexOf('External Turnkey FG') != -1) {
                $("#dvExistingBulkSemiNumber").show();
                $("#dvBulkSemiDescription").show();
                $("#txtBulkSemiDescription").val("Not Applicable");
            }
        }
        else {
            $("#dvExistingBulkSemiNumber").hide();
            $("#dvBulkSemiDescription").hide();
        }
    }

    defineFormats();

    function HideTransferSemiHeader() {
        var found = false;
        $(".titlelbl").each(function (i, obj) {
            var value = $(this).text();
            if (value.indexOf("Transfer Semi") >= 0) {
                found = true;
            }
        });

        if (found) {
            $(".TransferSemiDetailsHeader").removeClass('hideItem').addClass('showItem');
        }
        else {
            $(".TransferSemiDetailsHeader").removeClass('showItem').addClass('hideItem');
        }
    }
</script>
