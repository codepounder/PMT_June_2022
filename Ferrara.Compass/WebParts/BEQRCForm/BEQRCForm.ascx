<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BEQRCForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.BEQRCForm.BEQRCForm" %>



<div class="container">
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
    <div class="content">
        <div class="col-lg-11 col-centered">
            <h1>BE QRC Form – Marketing Confirmation</h1>
            <div class="row hidebutton">
                <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
                    <asp:ValidationSummary ID="ItemValidationSummary" ClientIDMode="Static" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
                    <div id="dverror_message" style="display: none;">
                        <ul id="error_message"></ul>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnTBDIndicator" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnParentProjectNumber" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDeleteApprovedGraphicsAssetUrl" ClientIDMode="Static" runat="server" />
            <asp:Button ID="hdnbtnDeleteApprovedGraphicsAsset" runat="server" ClientIDMode="Static" CssClass="hide" OnClick="hdnbtnDeleteApprovedGraphicsAsset_Click" />
            <asp:HiddenField ID="hdnDeleteVisualreferenceUrl" ClientIDMode="Static" runat="server" />
            <asp:Button ID="btnhdnDeleteVisualreferenceUrl" runat="server" ClientIDMode="Static" CssClass="hide" OnClick="btnhdnDeleteVisualreferenceUrl_Click" />
            <asp:HiddenField ID="hdnDeleteBEQRCEPSFileUrl" ClientIDMode="Static" runat="server" />
            <asp:Button ID="btnhdnDeleteBEQRCEPSFileUrl" runat="server" ClientIDMode="Static" CssClass="hide" OnClick="btnhdnDeleteBEQRCEPSFileUrl_Click" />
            <div id="loadingIcon" style="height: 400px; width: 500px;">
                <img src="/_layouts/15/Ferrara.Compass/images/loading.gif" alt="lOADING" />
            </div>
            <div class="IPFSUmmary">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <h2>IPF Summary</h2>
                    </div>
                </div>
                <div class="row RowBottomMargin">
                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Project Type:</label>
                            <asp:TextBox ID="txtProjectType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Line of Business:</label>
                            <asp:TextBox ID="txtLineOfBusiness" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Product Hierarchy 2:</label>
                            <asp:TextBox ID="txtProductHierarchy2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Brand:</label>
                            <asp:TextBox ID="txtBrand" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />
                            <label class="control-label">First Ship Date:</label>
                            <asp:TextBox ID="txtFirstShipDate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Weeks until Ship:</label>
                            <asp:TextBox ID="txtWeeksUntilShip" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <asp:HiddenField ID="HiddenField2" runat="server" ClientIDMode="Static" />
                            <label class="control-label">Annual $:</label>
                            <asp:TextBox ID="txtAnnualDollars" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Annual Units:</label>
                            <asp:TextBox ID="txtAnnualUnits" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Customer:</label>
                            <asp:TextBox ID="txtCustomer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label">Expected Gross Margin %:</label>
                            <asp:TextBox ID="txtExpectedGrossMarginPct" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="form-group">
                            <asp:Label ID="lblComments" CssClass="control-label" Text="Item Concept:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                            <asp:TextBox ID="txtItemConcept" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="RegulatoryForm">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <h2>Regulatory Form</h2>
                    </div>
                </div>
                <div class="row RowBottomMargin">
                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                </div>
                <div id="divCandySemi" runat="server" visible="false" class="repeater divCandySemi">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <table id="tabCandySemi" class="gridTable" style="width: 100%">
                                <asp:Repeater ID="rptCandySemi" runat="server" OnItemDataBound="rptCandySemi_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr>
                                            <th class="gridCellH">CandySemi #</th>
                                            <th class="gridCellH">CandySemi Description</th>
                                            <th class="gridCellH">Do any ingredients within this item need to claim Bio-Engineering?</th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                                            <td class="gridCell">
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                <asp:HiddenField ID="hdnCompassListItemId" runat="server" Value=' <%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                                <asp:Label runat="server" ID="txtMaterialNumber" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' />
                                            </td>
                                            <td class="gridCell">
                                                <asp:Label runat="server" ID="txtMaterialDescription" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' />
                                            </td>
                                            <td class="gridCell">
                                                <asp:Label runat="server" ID="txtIngredientsNeedToClaimBioEng" Text='<%# DataBinder.Eval(Container.DataItem, "IngredientsNeedToClaimBioEng") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>
                <br />
                <div id="divPurchasedCandy" runat="server" visible="false" class="repeater divPurchasedCandy">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <table id="tabPurchasedCandy" style="width: 100%">
                                <asp:Repeater ID="rptPurchasedCandy" runat="server" OnItemDataBound="rptPurchasedCandy_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr>
                                            <th class="gridCellH">Purchased CandySemi #</th>
                                            <th class="gridCellH">Purchased CandySemi Description</th>
                                            <th class="gridCellH">Do any ingredients within this item need to claim Bio-Engineering?</th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                                            <td class="gridCell">
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                <asp:HiddenField ID="hdnCompassListItemId" runat="server" Value=' <%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                                <asp:Label runat="server" ID="txtMaterialNumber" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' />
                                            </td>
                                            <td class="gridCell">
                                                <asp:Label runat="server" ID="txtMaterialDescription" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' />
                                            </td>
                                            <td class="gridCell">
                                                <asp:Label runat="server" ID="txtIngredientsNeedToClaimBioEng" Text='<%# DataBinder.Eval(Container.DataItem, "IngredientsNeedToClaimBioEng") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>
                <br />
                <div id="divFG" runat="server" visible="false" class="repeater divFG">
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <table id="tabFG" style="width: 100%">
                                <asp:Repeater ID="rptFG" runat="server" OnItemDataBound="rptFG_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr>
                                            <th class="gridCellH">Finished Good #</th>
                                            <th class="gridCellH">Finished Good Description</th>
                                            <th class="gridCellH">Do any ingredients within this item need to claim Bio-Engineering?</th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                                            <td class="gridCell">
                                                <asp:HiddenField ID="hdnId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                                <asp:HiddenField ID="hdnCompassListItemId" runat="server" Value=' <%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                                <asp:Label runat="server" ID="txtMaterialNumber" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' />
                                            </td>
                                            <td class="gridCell">
                                                <asp:Label runat="server" ID="txtMaterialDescription" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' />
                                            </td>
                                            <td class="gridCell">
                                                <asp:Label runat="server" ID="txtIngredientsNeedToClaimBioEng" Text='<%# DataBinder.Eval(Container.DataItem, "IngredientsNeedToClaimBioEng") %>' />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="divConsumerFacingProdDesc">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <h2>BE Landing Page Information</h2>
                    </div>
                </div>
                <div class="row RowBottomMargin">
                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-6">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Consumer Facing Product Description (BE Products Only):</label>
                            <asp:TextBox ID="txtConsumerFacingProdDesc" runat="server" CssClass="form-control required" ClientIDMode="Static"></asp:TextBox>
                            <p class="comment-block" id="pConsumerFacingProdDesc">
                                <asp:Label ID="lblSAPItemDescriptionMessage" CssClass="comment-block" runat="server" ClientIDMode="Static" Text="Ex: SAP description: TROLLI SBC VERY BERRY 12/5 OZ"></asp:Label>
                                <asp:Label ID="lblSAPItemDescription" CssClass="comment-block" runat="server" ClientIDMode="Static" Text="Description to be present on landing page: Trolli Sour Bright Crawlers Very Berry"></asp:Label><br />
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>FG BOM Details</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="PositionReferenceForButtons"></div>
            <div id="wizard" class="wizard clearfix">
                <%--<div class="steps clearfix">
                    <ul role="tablist" id="IPFLinkHeaders">
                        <li class=""><a href="#wizard-h-11">12. FG BOM Details</a></li>
                    </ul>
                </div>--%>
                <div class="content clearfix">
                    <div id="dverror_Proposed"></div>
                    <section id="wizard-h-11">
                        <div>
                            <asp:HiddenField ID="hdnIItemId" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnProjectNumber" runat="server" ClientIDMode="Static" />

                            <asp:HiddenField ID="updateHdnSteps" runat="server" ClientIDMode="Static" Value="0" />
                            <asp:HiddenField ID="hdnPLMFlag" runat="server" ClientIDMode="Static" Value="" />
                            <asp:UpdatePanel ID="UpdateTS" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="TSSections" ClientIDMode="Static" runat="server" class="">
                                        <asp:PlaceHolder ID="phTS" runat="server" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdatePanel ID="UpdateFG" runat="server">
                                <ContentTemplate>
                                    <asp:Panel ID="FGSection" ClientIDMode="Static" runat="server">
                                        <asp:HiddenField ID="activeTabHolder" runat="server" ClientIDMode="Static" Value="0" />
                                        <asp:HiddenField ID="hdnUCLoaded" runat="server" ClientIDMode="Static" Value="false" />
                                        <asp:PlaceHolder ID="phFG" runat="server" />
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </section>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-9 col-md-7">
                    <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
                </div>
                <div class="col-xs-12 col-sm-9 col-md-2">
                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="ButtonControl" CausesValidation="false" Style="float: right;" />
                </div>
                <div class="col-xs-12 col-sm-9 col-md-2">
                    <asp:Button ID="btnRequestQRCodes" runat="server" Text="Request QR Codes" OnClick="btnRequestQRCodes_Click" CssClass="ButtonControlAutoSize" OnClientClick="return ValidateDataONRequestQRCodes()" Style="float: right;" />
                </div>
                <div class="col-xs-12 col-sm-9 col-md-1">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="ButtonControl" OnClientClick="return ValidateIPFData();" Style="float: right;" />
                    <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="LoadAttachments HiddenButton" OnClick="btnReloadAttachment_Click"></asp:Button>
                </div>
            </div>
            <div class="row">
                <asp:HiddenField ID="hdnBrand" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnProductForm" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnPackType" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            </div>

        </div>
    </div>
    <%--Container--%>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        function RemoveRougeChar(convertString) {
            if (convertString.substring(0, 1) == ",") {
                return convertString.substring(1, convertString.length)
            }
            return convertString;
        }
        drpCompType_load();
        $("#FGSection .tsButtonLink").click();
    });
    function openAttachment(docType, title) {
        var itemId = $("#hiddenItemId").val();
        var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=0&DocType=' + docType + '&CompassItemId=' + itemId;
        openAttachmentsDialog(url, title);
        return false;
    }
    function drpNew_changed(drpNewId, copyValues) {
        BOMNewCondition(drpNewId, copyValues);
    }
    function drpCompType_changed(arg) {
        var anchor = $("#" + arg.id);
        var parentRow = anchor.closest(".bomrow");
        var newExisting = parentRow.find(".drpNewClass option:selected").text();
        var compType = anchor.find("option:selected").text().toLocaleLowerCase();
        if (compType == "candy semi" || compType == "transfer semi" || compType == "purchased candy semi") {
            parentRow.find(".hideableRow").addClass("hideItem");
            parentRow.find(".hideableRow .col-xs-12").addClass("hideItem");

            parentRow.find(".drpGraphics").val("-1");
            parentRow.find(".drpComponentContainsNLEA").val("-1");
            parentRow.find(".drpGraphicsVendor").val("-1");

            if (compType == "transfer semi" || compType == "purchased candy semi") {
                parentRow.find(".TSOnlyRow").removeClass("hideItem");
                parentRow.find(".TSOnlyRow .col-xs-12").removeClass("hideItem");
                parentRow.find(".TSOnlyRow .switch").html(anchor.find("option:selected").text());
                if (!parentRow.find(".drpNewClass option:eq(3)").length) {
                    parentRow.find(".drpNewClass").append('<option value="Network Move">Network Move</option>');
                }
                if (newExisting != "New") {
                    parentRow.find(".TSOnlyRow.new").addClass("hideItem");
                } else {
                    parentRow.find(".TSOnlyRow.new").removeClass("hideItem");
                }
            } else {
                parentRow.find(".TSOnlyRow").addClass("hideItem");
                parentRow.find(".TSOnlyRow .col-xs-12").addClass("hideItem");
                parentRow.find(".drpNewClass option:eq(3)").remove();
            }
        } else {
            parentRow.find(".hideableRow").removeClass("hideItem");
            parentRow.find(".hideableRow .col-xs-12").removeClass("hideItem");
            parentRow.find(".TSOnlyRow").addClass("hideItem");
            parentRow.find(".TSOnlyRow .col-xs-12").addClass("hideItem");
            parentRow.find(".drpNewClass option:eq(3)").remove();
        }

        if (compType == "candy semi" || compType == "transfer semi" || compType == "purchased candy semi" || compType.indexOf('finished good') != -1) {
            parentRow.find(".BEQRChideableRow").addClass("hideItem");
            parentRow.find(".BEQRChideableRow .col-xs-12").addClass("hideItem");
            parentRow.find(".ddlUPCAssociated").val("-1");
        } else {
            parentRow.find(".BEQRChideableRow").removeClass("hideItem");
            parentRow.find(".BEQRChideableRow .col-xs-12").removeClass("hideItem");
        }
    }
    function BOMNewCondition(drpNewId, copyValues) {
        var anchor = $("#" + drpNewId);
        var parentRow = anchor.closest(".bomrow");
        var idGraphics = parentRow.find(".drpGraphics:eq(0)").attr("id");
        var idFlowthrough = parentRow.find(".flowthroughClass:eq(0)").attr("id");
        var idCompType = parentRow.find(".drpComponentType:eq(0)").attr("id");

        var idComponent = parentRow.find(".Component:eq(0)").attr("id");
        var idComponentDesc = parentRow.find(".ComponentDesc:eq(0)").attr("id");

        var idLikeMaterial = parentRow.find(".LikeMaterial:eq(0)").attr("id");
        var idLikeDescription = parentRow.find(".LikeDescription:eq(0)").attr("id");

        var idOldMaterial = parentRow.find(".OldMaterial:eq(0)").attr("id");
        var idOldDescription = parentRow.find(".OldDescription:eq(0)").attr("id");

        var spanGraphicsNeeded = parentRow.find(".spanGraphicsNeeded:eq(0)");
        var spanGraphicsVendor = parentRow.find(".spanGraphicsVendor:eq(0)");
        var idwhyLikeComponent = parentRow.find(".whyLikeComponent:eq(0)").attr("id");
        var spanWhyComponent = parentRow.find(".spanWhyComponent:eq(0)");

        var spanComponent = parentRow.find(".spanComponent:eq(0)");
        var spanComponentDesc = parentRow.find(".spanComponentDesc:eq(0)");
        var spanLikeComponent = parentRow.find(".spanLikeComponent:eq(0)");
        var spanLikeComponentDesc = parentRow.find(".spanLikeComponentDesc:eq(0)");
        var spanFlowthrough = parentRow.find(".spanFlowthrough:eq(0)");
        var spanOldMaterial = parentRow.find(".spanOldMaterial:eq(0)");

        var componentVal = $("#" + idComponent).val().toLocaleLowerCase();
        var likeComponentVal = $("#" + idLikeMaterial).val().toLocaleLowerCase();
        var oldComponentVal = $("#" + idOldMaterial).val().toLocaleLowerCase();
        var componentDescVal = $("#" + idComponentDesc).val().toLocaleLowerCase();
        var likeComponentDescVal = $("#" + idLikeDescription).val().toLocaleLowerCase();
        var oldComponentDescVal = $("#" + idOldDescription).val().toLocaleLowerCase();

        //following checks are required if packaging item is new
        if ($("#" + drpNewId + " option:selected").text().toLowerCase() == 'new') {
            $("#" + idGraphics).addClass("PCBOMrequired");
            $("#" + idwhyLikeComponent).addClass("PCBOMrequired");
            $("#" + idComponent).prop("readonly", true);
            $("#" + idComponentDesc).prop("readonly", true);
            $("#" + idLikeMaterial).prop("readonly", false);
            $("#" + idLikeDescription).prop("readonly", false);
            $("#" + idOldMaterial).prop("readonly", false);
            $("#" + idOldDescription).prop("readonly", false);
            $("#" + idwhyLikeComponent).prop("readonly", false);

            spanLikeComponent.show();
            spanLikeComponentDesc.show();
            spanComponent.hide();
            spanComponentDesc.hide();
            spanGraphicsNeeded.show();
            spanWhyComponent.addClass('showItempc').removeClass('hideItem');

            if (copyValues) {
                if ((likeComponentVal == "" || likeComponentVal == "not applicable") && componentVal != "needs new") {
                    $("#" + idLikeMaterial).val(componentVal);
                }
                if ((likeComponentDescVal == "" || likeComponentDescVal == "not applicable") && componentDescVal != "needs new") {
                    $("#" + idLikeDescription).val(componentDescVal);
                }
                $("#" + idComponent).val("Needs New");
                $("#" + idComponentDesc).val("Needs New");
            }
            if ((likeComponentVal == "" || likeComponentVal == "not applicable") && componentVal != "needs new") {
                $("#" + idLikeMaterial).val(componentVal);
            }
            if (oldComponentVal == "not applicable") {
                $("#" + idOldMaterial).val("");
            }
            if (oldComponentDescVal == "not applicable") {
                $("#" + idOldDescription).val("");
            }
            $("#" + idFlowthrough).prop("disabled", false);
            if ($("#hdnTBDIndicator").val() == "No") {
                spanOldMaterial.addClass("hideItem");
                $("#" + idOldMaterial).removeClass("PCBOMrequired");
            } else if ($("#hdnTBDIndicator").val() == "Yes") {
                if ($("#" + idFlowthrough + " option:selected").text() == "Soft") {
                    spanOldMaterial.removeClass("hideItem");
                    $("#" + idOldMaterial).addClass("PCBOMrequired");
                } else {
                    spanOldMaterial.addClass("hideItem");
                    $("#" + idOldMaterial).removeClass("PCBOMrequired");
                }
            } else {
                spanOldMaterial.addClass("hideItem");
                $("#" + idOldMaterial).removeClass("PCBOMrequired");
            }
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").removeClass("hideItem");
            }
        }
        else if ($("#" + drpNewId + " option:selected").text().toLowerCase() == 'existing') {
            $("#" + idGraphics).addClass("PCBOMrequired");
            $("#" + idwhyLikeComponent).addClass("PCBOMrequired");
            $("#" + idLikeMaterial).prop("readonly", true);
            $("#" + idLikeDescription).prop("readonly", true);
            $("#" + idOldMaterial).prop("readonly", true);
            $("#" + idOldDescription).prop("readonly", true);
            $("#" + idwhyLikeComponent).prop("readonly", true);

            spanLikeComponent.hide();
            spanLikeComponentDesc.hide();
            $("#" + idComponent).prop("readonly", false);
            $("#" + idComponentDesc).prop("readonly", false);
            spanComponent.show();
            spanComponentDesc.show();
            spanGraphicsNeeded.show();
            spanWhyComponent.removeClass('showItempc').addClass('hideItem');
            $("#" + idOldMaterial).val("Not Applicable");
            $("#" + idOldDescription).val("Not Applicable");

            if (copyValues) {
                if ((componentVal == "" || componentVal == "needs new") && likeComponentVal != "not applicable") {
                    $("#" + idComponent).val(likeComponentVal);
                }
                if ((componentDescVal == "" || componentDescVal == "needs new") && likeComponentDescVal != "not applicable") {
                    $("#" + idComponentDesc).val(likeComponentDescVal);
                }
                $("#" + idLikeMaterial).val("Not Applicable");
                $("#" + idLikeDescription).val("Not Applicable");
                $("#" + idwhyLikeComponent).val("Not Applicable");
            }
            if ($("#" + idFlowthrough).val() == "-1") {
                //$("#" + idFlowthrough).prop("disabled", true);
                $("#" + idFlowthrough).val("3").change();
            }
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").addClass("hideItem");
            }
        } else if ($("#" + drpNewId + " option:selected").text().toLowerCase() == 'network move') {
            $("#" + idLikeMaterial).removeClass("PCBOMrequired");
            $("#" + idLikeDescription).removeClass("PCBOMrequired");
            spanGraphicsNeeded.hide();
            spanWhyComponent.addClass('hideItem').removeClass('showItempc');
            $("#" + idLikeMaterial).prop("readonly", false);
            $("#" + idLikeDescription).prop("readonly", false);
            $("#" + idOldMaterial).prop("readonly", false);
            $("#" + idOldDescription).prop("readonly", false);
            $("#" + idwhyLikeComponent).prop("readonly", false);
            if (oldComponentVal == "not applicable") {
                $("#" + idOldMaterial).val("");
            }
            if (oldComponentDescVal == "not applicable") {
                $("#" + idOldDescription).val("");
            }

            spanComponent.show();
            spanComponentDesc.show();
            spanLikeComponent.hide();
            spanLikeComponentDesc.hide();

            $("#" + idFlowthrough).val("3").change();
            $("#" + idFlowthrough).prop("disabled", false);
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").addClass("hideItem");
            }
        }
        else {
            $("#" + idGraphics).removeClass("PCBOMrequired");
            $("#" + idwhyLikeComponent).removeClass("PCBOMrequired");
            spanGraphicsNeeded.hide();
            spanWhyComponent.addClass('hideItem').removeClass('showItempc');
            $("#" + idLikeMaterial).prop("readonly", false);
            $("#" + idLikeDescription).prop("readonly", false);
            $("#" + idOldMaterial).prop("readonly", false);
            $("#" + idOldDescription).prop("readonly", false);
            $("#" + idwhyLikeComponent).prop("readonly", false);
            if (oldComponentVal == "not applicable") {
                $("#" + idOldMaterial).val("");
            }
            if (oldComponentDescVal == "not applicable") {
                $("#" + idOldDescription).val("");
            }

            spanComponent.show();
            spanComponentDesc.show();
            spanLikeComponent.show();
            spanLikeComponentDesc.show();
            $("#" + idFlowthrough).prop("disabled", false);
            if ($("#" + idCompType + " option:selected").text() == "Transfer Semi" || $("#" + idCompType + " option:selected").text() == "Purchased Candy Semi") {
                parentRow.find(".TSOnlyRow.new").addClass("hideItem");
            }
        }
    }
    function FlowthroughCheck(arg) {
        var anchor = $("#" + arg.id);
        var selectedFlowthrough = $("#" + arg.id + " option:selected").text().toLowerCase()
        var spanOldMaterial = anchor.closest(".bomrow").find(".spanOldMaterial:eq(0)");
        var idOldMaterial = anchor.closest(".bomrow").find(".OldMaterial:eq(0)").attr("id");
        if (selectedFlowthrough == 'soft' && anchor.closest(".bomrow").find(".drpNewClass:eq(0)").val() == "New" && $("#ddlTBDIndicator option:selected").text() == "Yes") {
            spanOldMaterial.removeClass("hideItem");
            $("#" + idOldMaterial).addClass("PCBOMrequired");
        } else {
            spanOldMaterial.addClass("hideItem");
            $("#" + idOldMaterial).removeClass("PCBOMrequired");
        }
    }
    function GraphicsCheck(arg) {
        var anchor = $("#" + arg.id);
        var idGraphicsBrief = anchor.closest(".bomrow").find(".GraphicsBrief");
        var spanGraphicsBrief = anchor.closest(".bomrow").find(".spanGraphicsBrief");
        var idGraphicsVendor = anchor.closest(".bomrow").find(".drpGraphicsVendor");
        var spanGraphicsVendor = anchor.closest(".bomrow").find(".spanGraphicsVendor");

        if ($("#" + arg.id + " option:selected").text() == 'Yes') {
            idGraphicsBrief.addClass("PCBOMrequired");
            spanGraphicsBrief.addClass('showItempc').removeClass('hideItem');
            idGraphicsVendor.addClass("PCBOMrequired");
            spanGraphicsVendor.addClass('showItempc').removeClass('hideItem');
            $('.showmarkGB').show();
        }
        else {
            idGraphicsBrief.removeClass("PCBOMrequired");
            spanGraphicsBrief.addClass('hideItem').removeClass('showItempc');
            idGraphicsVendor.removeClass("PCBOMrequired");
            spanGraphicsVendor.addClass('hideItem').removeClass('showItempc');
            $('.showmarkGB').hide();
        }
    }
</script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/BEQRC.js?v=18"></script>
<script>
    function updateJavascriptStyles() {
        $(".drpNewClass").each(function (index, drpNew) {
            if ($(this).hasClass("PCBOMrequired")) {
                BOMNewCondition($(drpNew).attr('id'), false);
            }
        });
        defineFormats();
    }
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(updateJavascriptStyles);
    updateJavascriptStyles();
    function callSAPNomenclature() {
        BindHierarchiesOnLoad();
    }
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callSAPNomenclature);
    callSAPNomenclature();
</script>
