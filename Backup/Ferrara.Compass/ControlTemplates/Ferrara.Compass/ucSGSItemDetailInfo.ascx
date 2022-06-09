<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSGSItemDetailInfo.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucSGSItemDetailInfo" %>

<h3 class="accordion">
    <asp:PlaceHolder runat="server" ID="pnProjectNo"></asp:PlaceHolder>
    Item Detail Information</h3>
<div class="panel">
    <div class="container ucmain" id="dvMain" style="border: 1px solid black;" clientidmode="Static" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
                <asp:ValidationSummary ID="ItemValidationSummary" ClientIDMode="Static" CssClass="error" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
                <div id="dverror_messageuc" class="error" style="display: none;">
                    <ul id="error_messageuc">
                        <li></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Is a New FG Item # Being Used?:</label>
                    <asp:DropDownList ID="ddlTBDIndicator" CssClass="form-control" ClientIDMode="Static" ToolTip="TBD Indicator" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">
                        <br />
                        <span class="markrequired">*</span>FG Item #:</label>
                    <asp:TextBox ID="txtSAPItemNumber" ClientIDMode="Static" runat="server" CssClass="alphanumericToUpper1 form-control minimumlength" MaxLength="20"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <br />
                    <label class="control-label">&nbsp;</label><br />
                    <asp:Button ID="btnLookupSAPItemNumber" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getSAPItemDescription('txtSAPItemNumber', 'txtSAPItemDescription'); return false;" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-5">
                <div class="form-group">
                    <br />
                    <label class="control-label"><span class="markrequired">*</span>FG Item Description:</label>
                    <asp:TextBox ID="txtSAPItemDescription" ClientIDMode="Static" MaxLength="60" CssClass="form-control" runat="server" onBlur="updateSGSSAPDesc(this,'txtSAPItemDescription')"></asp:TextBox>
                    <p class="comment-block">
                        <asp:Label ID="Label1" CssClass="comment-block" runat="server" ClientIDMode="Static" Text="If New FG #, description will automatically build based on information filled out in IPF."></asp:Label>
                    </p>
                    <p class="comment-block" id="pSAPItemDescription">
                        <asp:Label ID="lblSAPItemDescription" CssClass="comment-block" runat="server" ClientIDMode="Static"></asp:Label>
                    </p>
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
        <asp:UpdatePanel ID="hierarchyPanel" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlProductHierarchyLevel1" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlProductHierarchyLevel2" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlBrand_Material" EventName="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div id="reloadingCover" clientidmode="Static" runat="server" style="height: 350px; position: absolute; width: 100%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                            <div class="disablingLoadingIcon" id="disablingLoadingHierarchy" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 1:</label>
                            <asp:DropDownList ID="ddlProductHierarchyLevel1" onchange="SAPNomenclature()" CssClass="required HierarchyLevel1 form-control" runat="server"
                                AppendDataBoundItems="true" AutoPostBack="True" OnSelectedIndexChanged="ddlProductHierarchyLevel1_SelectedIndexChanged" ClientIDMode="Static">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                            <p class="comment-block">
                                <asp:Label ID="lblProductHierarchyLevel1" CssClass="comment-block" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6" id="divManuallyCreateSAPDescription">
                        <div class="form-group">
                            <label class="control-label">Manually Create SAP Description:</label>
                            <asp:DropDownList ID="ddlManuallyCreateSAPDescription" onchange="SAPNomenclature()" CssClass="form-control" runat="server" ClientIDMode="Static">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 2:</label>
                            <asp:DropDownList ID="ddlProductHierarchyLevel2" onchange="SAPNomenclature()" OnSelectedIndexChanged="ddlProductHierarchyLevel2_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                            <p class="comment-block">
                                <asp:Label ID="lblProductHierarchyLevel2" CssClass="comment-block" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Material Group 1 (Brand):</label>
                            <asp:DropDownList ID="ddlBrand_Material" onchange="SAPNomenclature()" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                            <p class="comment-block">
                                <asp:Label ID="lblBrand_Material" CssClass="comment-block" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Material Group 4 (Product Form):</label>
                    <asp:DropDownList ID="ddlMaterialGroup4" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                    <p class="comment-block">
                        <asp:Label ID="lblMaterialGroup4" CssClass="comment-block" runat="server"></asp:Label>
                    </p>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Material Group 5 (Pack Type):</label>
                    <asp:DropDownList ID="ddlMaterialGroup5" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true" TabIndex="12">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                    <p class="comment-block">
                        <asp:Label ID="lblMaterialGroup5" CssClass="comment-block" runat="server"></asp:Label>
                    </p>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Item UPCs</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Do we need any new UPCs (12 digit GTIN) or UCCs (14 digit GTIN)?:</label>
                    <asp:DropDownList ID="ddlNeedNewUPCUCC" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvddlNeedNewUPC" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Do we need a New Consumer Unit UPC (12 digit GTIN) – EACH?:</label>
                    <asp:DropDownList ID="ddlNeedNewUnitUPC" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvUnitUPC" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Existing Consumer Unit UPC (12 digit GTIN) – EACH:</label>
                    <asp:TextBox ID="txtUnitUPC" ClientIDMode="Static" runat="server" class="form-control" MaxLength="13"></asp:TextBox>
                    <label class="comment-block">1st Level UPC - This should be the smallest packaged item. This could also be a 13 digit EAN code.</label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvNeedNewDisplayBoxUPC" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Do we need a New Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray?:</label>
                    <asp:DropDownList ID="ddlNeedNewDisplayBoxUPC" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvDisplayBoxUPC" class="form-group">
                    <label class="control-label">Existing Consumer Unit UPC (12 digit GTIN) – Display/Carton/Display Tray:</label>
                    <asp:TextBox ID="txtDisplayUPCBox" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="13"></asp:TextBox>
                    <label class="comment-block">2nd Level UPC – This would be used if there is a UPC on a secondary level of packaging for an item; i.e. display, carton, display tray, etc</label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvSAPBaseUOM" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>SAP Base UOM:</label>
                    <asp:DropDownList ID="ddlSAPBUOM" CssClass="form-control" ClientIDMode="Static" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                    <label class="comment-block">Only use “PAL” if the full pallet can only be sold to the customer, i.e. club pallets. ½ and ¼ pallets/mods must use CS as the SAP Base UOM.</label>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvNeedNewCaseUCC" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Do we need a new Case UCC (14 digit GTIN)?:</label>
                    <asp:DropDownList ID="ddlNeedNewCaseUCC" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvCaseUCC" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Existing Case UCC (14 digit GTIN)::</label>
                    <asp:TextBox ID="txtCaseUCC" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="14"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvNeedNewPalletUCC" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Do we need a new Pallet UCC (14 digit GTIN)?:</label>
                    <asp:DropDownList ID="ddlNeedNewPalletUCC" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvPalletUCC" class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Existing Pallet UCC (14 digit GTIN):</label>
                    <asp:TextBox ID="txtPalletUCC" ClientIDMode="Static" runat="server" CssClass="form-control" MaxLength="14"></asp:TextBox>
                </div>
            </div>
        </div>
        <div id="dvLTO">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>LTO/Transition Information</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>Is this Finished Good replacing an existing Finished Good?:</label>
                        <asp:DropDownList ID="ddlFGReplacingAnExistingFG" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>Is this an LTO (Limited Time Offer) item?:</label>
                        <asp:DropDownList ID="ddlIsThisAnLTOItem" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4" id="divRequestChangeToFGNumForSameUCC">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>Are we requesting a change to the Finished Good number while maintaining the same UCC?:</label>
                        <asp:DropDownList ID="ddlRequestChangeToFGNumForSameUCC" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-4" id="divLTOTransitionStartWindowRDD">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>LTO/Transition Start Window (Requested Delivery Date - RDD):</label>
                        <asp:TextBox ID="txtLTOTransitionStartWindowRDD" ClientIDMode="Static" runat="server" CssClass="datePicker required form-control" onchange="fixMonth();" ToolTip="Click to Choose Date"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4" id="divLTOTransitionEndWindowRDD">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>LTO/Transition End Window (Requested Delivery Date - RDD):</label>
                        <asp:TextBox ID="txtLTOTransitionEndWindowRDD" ClientIDMode="Static" runat="server" CssClass="datePicker required form-control" onchange="fixMonth();" ToolTip="Click to Choose Date"></asp:TextBox>
                        <div class="comment-block">If the RDD is not known, it can be estimated by ship date plus 6 calendar days.</div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4" id="divLTOEndDateFlexibility">
                    <div class="form-group">
                        <label class="control-label"><span class="markrequired">*</span>LTO End Date Flexibility:</label>
                        <asp:DropDownList ID="ddlLTOEndDateFlexibility" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Hard Stop - Cannot Run Out" Value="HardStop"></asp:ListItem>
                            <asp:ListItem Text="Soft Stop - Can Sell Out Inventory" Value="SoftStop"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Customer Specifications</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Customer/Channel Specific:</label>
                    <asp:DropDownList ID="ddlCustomerSpecific" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Customer Specific" Value="CU"></asp:ListItem>
                        <asp:ListItem Text="Channel Specific" Value="CH"></asp:ListItem>
                        <asp:ListItem Text="Pricelist" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div id="dvCustomer" class="form-group">
                    <label class="control-label">Customer:</label>
                    <asp:DropDownList ID="ddlCustomer" ClientIDMode="Static" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div id="dvChannel" class="form-group">
                    <label class="control-label">Channel:</label>
                    <asp:DropDownList ID="ddlChannel" ClientIDMode="Static" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="UpdatePanel4" runat="server" ClientIDMode="Static">
            <ContentTemplate>
                <div class="row">

                    <div class="col-xs-12 col-sm-6 col-md-8">
                        <asp:HiddenField runat="server" ID="hdnChildProjectNo" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnParentProjectNo" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnGenerated" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnStageGateItemId" ClientIDMode="Static" />
                        <asp:HiddenField runat="server" ID="hdnStageGateChildItemId" ClientIDMode="Static" />
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-2">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button justifyRight" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-2">
                        <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" CssClass="clickSave button justifyRight" Text="Save" OnClick="btnSave_Click" />
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $(".container:not(.ucmain) > .row").hide();
        $(".dvUserControl").prev().hide();
        $("#ChildIPFs").hide();
        $(".ProjectNotesContainerParent").hide();

        SGSConditionalChecks();
        if ($("#hdnGenerated").val() != "false") {
            $(".form-control").attr("disabled", "disabled");
            $(".form-control").attr("readonly", "readonly");
            $("#btnSave").addClass("hideItem");
        }

        //SAP Nomenclature
        SAPNomenclature();
    });

    //SAP Nomenclature
    function SAPNomenclature(controlId) {
        var BuildSAPDescription = true;

        if ($('#hdnGenerated').val() != "true") {
            $("#txtSAPItemDescription").prop('readonly', true);

            if ($("#ddlTBDIndicator option:selected").text() == "Yes") {
                if ($("#ddlProductHierarchyLevel1 option:selected").text() == "Co-Manufacturing (000000027)") {
                    $("#divManuallyCreateSAPDescription").removeClass('hide');
                    if ($("#ddlManuallyCreateSAPDescription option:selected").text() == "Yes") {
                        BuildSAPDescription = false;
                        $("#txtSAPItemDescription").prop('readonly', false);
                    } else {
                        $("#txtSAPItemDescription").prop('readonly', true);
                    }
                }
                else {
                    $("#ddlManuallyCreateSAPDescription").val("No");
                    $("#divManuallyCreateSAPDescription").addClass('hide');
                }
            }
            else {
                $("#ddlManuallyCreateSAPDescription").val("No");
                $("#divManuallyCreateSAPDescription").addClass('hide');
                BuildSAPDescription = false;
                if (controlId != null && controlId.id == "ddlTBDIndicator") {
                    getSAPItemDescription("txtSAPItemNumber", "txtSAPItemDescription");
                }
            }
        } else {
            $("#divManuallyCreateSAPDescription").addClass('hide');
            $("#ddlManuallyCreateSAPDescription").val("No");
            BuildSAPDescription = false;
        }

        if (BuildSAPDescription) {
            $("#txtSAPItemDescription").prop('readonly', true);
            var TBD = '';
            var Brand = '';
            var Season = '';
            var CustomerSpecific = '';
            var PkgType = '';

            //TBD
            if ($("#ddlTBDIndicator option:selected").text() == 'Yes') {
                TBD = "TBD ";
            }
            //Brand
            var BrandSelection = $("#ddlBrand_Material option:selected").text();
            if (BrandSelection != 'Select...') {
                re = /.*\(([^)]+)\)/;
                Brand = BrandSelection.match(re)[1] + ' ';
            }
            //Season
            if ($("#ddlProductHierarchyLevel1 option:selected").text() == "Seasonal (000000023)") {
                var SeasonSelection = $("#ddlProductHierarchyLevel2 option:selected").text();

                if (SeasonSelection == 'VALENTINE\'S (000000008)' || SeasonSelection == 'VALENTINE\'S (000000008)' || SeasonSelection == 'VALENTINE\'S DAY (000000008)') {
                    Season = 'VDY ';
                } else if (SeasonSelection == 'EASTER (000000003)' || SeasonSelection == 'EASTER BULK (000000004)') {
                    Season = 'ESR ';
                } else if (SeasonSelection == 'HALLOWEEN (000000005)' || SeasonSelection == 'HALLOWEEN BULK (000000006)') {
                    Season = 'HWN ';
                } else if (SeasonSelection == 'CHRISTMAS (000000001)' || SeasonSelection == 'CHRISTMAS BULK (000000002)') {
                    Season = 'HLY ';
                } else if (SeasonSelection == 'HOLIDAY (000000001)') {
                    Season = 'HLY ';
                } else if (SeasonSelection == 'SUMMER(000000007)') {
                    Season = 'SMR ';
                }
            }
            //Customer specific
            if ($("#ddlCustomerSpecific option:selected").text() == "Customer Specific") {
                var CustomerspecificSelection = $("#ddlCustomer option:selected").text();

                if (CustomerspecificSelection != 'Select...') {
                    re = /\((.*)\)/i;
                    CustomerSpecific = CustomerspecificSelection.match(re)[1] + ' ';
                }
            }

            //Pkg Type
            var ddlMaterialGroup5 = $("#ddlMaterialGroup5 option:selected").text();
            if (ddlMaterialGroup5.indexOf("(DOY)") != -1) {
                PkgType = ' DOY'
            } else if (ddlMaterialGroup5.indexOf("(SHP)") != -1) {
                PkgType = ' SHP'
            }


            ProposedItem = TBD + Brand + Season + CustomerSpecific + PkgType;
            ProposedItem = ProposedItem.trim();

            $("#txtSAPItemDescription").val(ProposedItem);
        }
        else {
            $("#lblSAPItemDescription").text('');
        }
    }

    function getSAPItemDescription(numberId, descriptionID) {
        var enteredText = $("#" + numberId).val();
        $.ajax({
            url: _spPageContextInfo.webAbsoluteUrl + "/_api/Web/Lists/getbytitle('SAP Material Master List')/items?$select=ID,SAPDescription,SAPItemNumber,CandySemiNumber&$filter=(Title eq '" + enteredText + "')&$top=500",
            method: 'GET',
            dataType: 'json',
            headers: {
                Accept: "application/json;odata=verbose"
            },
            success: function (results) {
                var finding = results.d.results[0];
                if (finding === undefined) {
                    $("#" + numberId).closest(".row").append("<div class='noResultsRow' style=\"margin-left: 460px;\">Lookup returned no results.</div>");
                } else {
                    $("#" + numberId).closest(".row").find(".noResultsRow").remove();
                    if ($("#ddlTBDIndicator option:selected").text() == "Yes") {
                        $("#lblSAPItemDescription").text(finding.SAPDescription);
                    } else {
                        $("#" + descriptionID).val(finding.SAPDescription);
                        //Update Project Header title
                        var ProjectTitle = $('#lblProjectTitle').text();
                        var ProjectTitleArray = ProjectTitle.split(":");
                        $('#lblProjectTitle').text(ProjectTitleArray[0] + " : " + ProjectTitleArray[1] + " : " + finding.SAPDescription);
                    }
                }
                $("#" + numberId).closest(".row").find(".disablingLoadingIcon").remove();
                $("#" + descriptionID).focus();
            },
            complete: function () {
                $("#" + descriptionID).blur(function () {
                    $(".noResultsRow").remove();
                });
            }
        });
    }
</script>
