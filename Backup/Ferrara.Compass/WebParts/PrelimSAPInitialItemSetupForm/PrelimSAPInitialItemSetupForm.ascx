<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrelimSAPInitialItemSetupForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.PrelimSAPInitialItemSetupForm.PrelimSAPInitialItemSetupForm" %>

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
            <h1>Preliminary SAP Initial Item Setup Form</h1>
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
            <h2>Preliminary SAP Initial Item Setup Tasks</h2>
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
                <label class="comment-block">Only use “PAL” if the full pallet can only be sold to the customer, i.e. club pallets. ½ and ¼ pallets/mods must use CS as the SAP Base UOM.</label>
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
                <asp:TextBox ID="lblMaterialGroup" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 2 (Trade Promo Group):</label>
                <asp:TextBox ID="lblTradePromo" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
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
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
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
                <asp:TextBox ID="txtSAPDescription" runat="server" MaxLength="60" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Consumer Unit UPC (12 digit GTIN) – EACH:</label>
                <asp:TextBox ID="txtUnitUPC" runat="server" CssClass="form-control numericNoMask requiredNeedsNew" MaxLength="13"></asp:TextBox>
                <label class="comment-block">1st Level UPC - This should be the smallest packaged item. This could also be a 13 digit EAN code.</label>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray:</label>
                <asp:TextBox ID="txtDisplayUPC" runat="server" ClientIDMode="Static" CssClass="form-control numericNoMask" MaxLength="13"></asp:TextBox>
                <label class="comment-block">2nd Level UPC – This would be used if there is a UPC on a secondary level of packaging for an item; i.e. display, carton, display tray, etc.</label>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6"></div>
    </div>
    <div class="row" id="dvCaseUcc" runat="server">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Case UCC (14 digit GTIN):</label>
                <asp:TextBox ID="txtCaseUCC" runat="server" CssClass="form-control numericNoMask requiredNeedsNew" MaxLength="14"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6"></div>
    </div>
    <div class="row" id="dvPalletUcc" runat="server">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Pallet UCC (14 digit GTIN):</label>
                <asp:TextBox ID="txtPalletUCC" runat="server" CssClass="form-control numericNoMask requiredNeedsNew" MaxLength="14"></asp:TextBox>
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
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="ButtonControl" OnClientClick="return validatePage()" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
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
        $(".accordion").click(function () {
            var dvNext = $(this).next('div.panel');
            dvNext.toggleClass('hideItem');
        });
        //conditionalChecks();

    })
    function conditionalChecks() {
        /* if ($("#txtDisplayUPC").length == 0)
             return;
         if ($("#txtDisplayUPC").val().toLowerCase().trim() == "needs new") {
             $("#spanDisplayUPC").show();
             $("#txtDisplayUPC").addClass("required");
         }
         else {
             $("#spanDisplayUPC").hide();
             $("#txtDisplayUPC").removeClass("required");
         }*/
    }
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

        if (isValid && isChecked) {
            return true;
        }

        if (!isValid || !isChecked) {
            loadingIconAdded = true;
            $(".disablingLoadingIcon").remove();
            setFocusError();
        }
        return false;
    }
</script>
