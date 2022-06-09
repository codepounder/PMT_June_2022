<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMSecondReview.ascx.cs" Inherits="Ferrara.Compass.WebParts.PMSecondReview.PMSecondReview" %>

<div class="container" id="dvcontainer">
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
            <h1>PM Second Review</h1>
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
    <%--Component Details pop-up--%>
    <div class="modal fade" id="dialog-form" role="dialog" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog ui-dialog ui-corner-all ui-widget ui-widget-content ui-front ui-dialog-buttons ui-draggable ui-resizable">
            <!-- Modal content-->
            <div class="modal-content" style="width: fit-content; margin-left: -300px;">
                <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                    <span class="modal-title">Component Details</span>
                </div>
                <div class="modal-body" style="display: flex;">
                    <div class="panel">
                        <div class="container ucmain" id="dvMain" style="border: 1px solid black;" clientidmode="Static" runat="server">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
                                    <asp:ValidationSummary ID="ValidationSummary1" ClientIDMode="Static" CssClass="error" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
                                    <div id="dverror_messageuc" class="error" style="display: none;">
                                        <ul id="error_messageuc">
                                            <li></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                            <div class="row ipf">
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Component Type:</label>
                                        <asp:DropDownList ID="drpPkgComponent" CssClass="form-control CompDetails" onchange="NewPackagingItem();" ClientIDMode="Static" runat="server">
                                            <asp:ListItem Text="--Select Packaging Component--" Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdnSAPMatGroup" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="HiddenField1" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hdnIsNew" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="HiddenField2" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hdn13DigitCode" ClientIDMode="Static" runat="server" />
                                        <asp:HiddenField ID="hdnTSBarcodeGenerationVisibility" ClientIDMode="Static" runat="server" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">New/Existing:</label>
                                        <asp:DropDownList ID="drpNew" onchange="BOMNewCondition(this);" ClientIDMode="Static" runat="server" CssClass="form-control CompDetails">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                            <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                                            <asp:ListItem Text="Network Move" Value="Network Move"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Pack Unit:</label>
                                        <asp:DropDownList ID="ddlPackUnit" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Pack Quantity:</label>
                                        <asp:TextBox ID="txtPackQty" CssClass="form-control minimumlength numericDecimal3 CompDetails" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <asp:Panel class="row ipf" ID="lblCompNote" runat="server" Visible="false">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <span class="comment-block">This component has been disabled because it has children, please delete or move all child components before changing the component type.</span>
                                </div>
                            </asp:Panel>
                            <div class="row hideableRow ipf">
                                <div class="col-xs-12 col-sm-4 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">New Graphics Required?:</label>
                                        <asp:DropDownList ID="drpGraphicsNeeded" runat="server" CssClass="form-control drpGraphics CompDetails" ClientIDMode="Static">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-4 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Component Contains NLEA?:</label>
                                        <asp:DropDownList ID="drpComponentContainsNLEA" runat="server" CssClass="form-control drpGraphics CompDetails" ClientIDMode="Static">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-4 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Graphics Vendor:</label>
                                        <asp:DropDownList ID="ddlGraphicsVendor" runat="server" CssClass="form-control drpGraphicsVendor CompDetails" ClientIDMode="Static">
                                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row ipf">
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Flowthrough:</label>
                                        <asp:DropDownList ID="ddlFlowthrough" runat="server" CssClass="form-control CompDetails" ClientIDMode="Static">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-6 col-md-4 PurchasedCandyLocation" runat="server" clientidmode="Static" id="dvPurchasedInto">
                                    <div class="form-group">
                                        <label class="control-label">Purchased Into Location:</label>
                                        <asp:DropDownList ID="ddlPurchasedIntoLocation" runat="server" CssClass="form-control CompDetails" ClientIDMode="Static">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4 SpecificationNo" runat="server" clientidmode="Static" id="dvSpecification">
                                    <div class="form-group">
                                        <label class="control-label">Packaging Specification #:</label>
                                        <asp:TextBox ID="txtSpecificationNo" CssClass="form-control minimumlength alphanumericToUpper1 CompDetails" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4" id="dvRevPrinterSupplier" runat="server" clientidmode="Static">
                                    <div class="form-group">
                                        <label class="control-label">Review Printer-Supplier (Proc)?:</label>
                                        <asp:DropDownList ID="ddlReviewPrinterSupplier" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="form-control CompDetails" Enabled="false">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">Print Style:</label>
                                        <asp:DropDownList ID="ddlFilmPrintStyle" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control CompDetails" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-6" runat="server" id="dvSAPDescAbrev" clientidmode="Static" visible="false">
                                    <div class="form-group">
                                        <label class="control-label">SAP Description Abbreviation:</label>
                                        <asp:DropDownList ID="ddlSAPDescAbrev" runat="server" CssClass="form-control CompDetails" ClientIDMode="Static " Enabled="false">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row ipf">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label">Component #:</label>
                                        <asp:TextBox ID="txtMaterial" CssClass="form-control minimumlength alphanumericToUpper1 Component CompDetails" ViewStateMode="Enabled" MaxLength="20" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label"></label>
                                        <asp:Button ID="btnLookupSAPItemNumber" CssClass="ButtonControl hidebtn" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtMaterial','txtMaterialDescription');return false;" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-8">
                                    <div class="form-group">
                                        <label class="control-label">Component Description:</label>
                                        <asp:TextBox ID="txtMaterialDescription" CssClass="form-control ComponentDesc CompDetails" ClientIDMode="Static" MaxLength="40" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row ipf">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label">Like Component #:</label>
                                        <asp:TextBox ID="txtLikeItem" CssClass="form-control minimumlength alphanumericToUpper1 OldComponent CompDetails" ViewStateMode="Enabled" MaxLength="20" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label"></label>
                                        <asp:Button ID="btnOldComponet" CssClass="ButtonControl hidebtn CompDetails" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtLikeItem','txtLikeDescription');return false;" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-8">
                                    <div class="form-group">
                                        <label class="control-label">Like Component Description:</label>
                                        <asp:TextBox ID="txtLikeDescription" CssClass="form-control OldComponentDesc CompDetails" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row ipf">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label">Old Component #:</label>
                                        <asp:TextBox ID="txtOldMaterial" ClientIDMode="Static" runat="server" CssClass="alphanumericToUpper1 minimumlength form-control CompDetails" MaxLength="20"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label">&nbsp;</label>
                                        <asp:Button ID="btnLookupOldCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtOldMaterial','txtOldMaterialDesc');return false;" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-8">
                                    <div class="form-group">
                                        <label class="control-label">Old Component Description:</label>
                                        <asp:TextBox ID="txtOldMaterialDesc" ClientIDMode="Static" runat="server" CssClass="form-control CompDetails minimumlength alphanumericToUpper1 OldComponent"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row ipf TSOnlyRow hideItem new">
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 1:</label>
                                        <asp:DropDownList ID="ddlPHL1" CssClass="required PHL1 form-control CompDetails" onchange="BindPHL2DropDownItemsByPHL1(this);" runat="server" AppendDataBoundItems="true" ClientIDMode="Static">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <p class="comment-block">
                                            <asp:Label ID="lblPHL1" CssClass="comment-block" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 2:</label>
                                        <asp:DropDownList ID="ddlPHL2" ClientIDMode="Static" CssClass="required form-control CompDetails PHL2" onchange="BindBrandDropDownItemsByPHL2(this);" runat="server" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <p class="comment-block">
                                            <asp:Label ID="lblPHL2" CssClass="comment-block" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Material Group 1 (Brand):</label>
                                        <asp:DropDownList ID="ddlBrand" ClientIDMode="Static" CssClass="required form-control CompDetails Brand" onchange="GetProfitCenter(this);" runat="server" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                        <p class="comment-block">
                                            <asp:Label ID="Label2" CssClass="comment-block" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="row ipf TSOnlyRow hideItem new">
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Profit Center:</label>
                                        <asp:TextBox ID="txtProfitCenterUC" ClientIDMode="Static" runat="server" ReadOnly="true" class="required form-control CompDetails"></asp:TextBox>
                                        <asp:HiddenField ID="hdnProfitCenterUC" runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                            <div class="row hideableRow ipf">
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">How is it a Like Component #:</label>
                                        <asp:TextBox ID="txtLikeMaterial" TextMode="MultiLine" ToolTip="Does like component # have same dieline, graphics, etc." Rows="3" runat="server" CssClass="form-control CompDetails whyLikeComponent" Text='' ClientIDMode="Static"></asp:TextBox>
                                        <label id="lblItemNote" class="comment-block" style="color: #777777">Does like component # have same dieline, graphics, etc.</label>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">Graphics Brief:</label>
                                        <asp:TextBox ID="txtGraphicsBrief" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control CompDetails GraphicsBrief" Text='' ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="ApprovedGraphicsAssetPanel GraphicsOnlyProc GraphicsOnlyPM2">
                                <div class="row attachment">
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <h2>Approved Graphics Asset - New Component</h2>
                                    </div>
                                </div>
                                <div class="row attachment">
                                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                                </div>
                                <asp:UpdatePanel ID="ApprovedGraphicsAssetPanel" ClientIDMode="Static" runat="server" ChildrenAsTriggers="true">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnReloadAttachment" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                                            <ProgressTemplate>
                                                <div id="reloadingCoverApprovedGraphicsAsset" clientidmode="Static" runat="server" style="height: 165px; position: absolute; width: 100%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                                                    <div class="disablingLoadingIcon" id="disablingLoadingApprovedGraphicsAsset" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                                                </div>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <div class="ApprovedGraphicsAsset">
                                            <div class="row attachment">
                                                <div class="col-xs-12 col-sm-3 col-md-4">
                                                    <label class="control-label">Approved Graphics Asset - New Component:</label>
                                                </div>
                                                <div class="col-xs-12 col-sm-9 col-md-4">
                                                    <input id="btnApprovedGraphicsAsset" type="button" runat="server" class="ButtonControlAutoSize hidebtn" value="Upload Approved Graphics Asset" onclick="OpenDialog('Upload Approved Graphics Asset', 'ApprovedGraphicsAsset'); return false;" />
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="col-xs-12 col-sm-12 col-md-6">
                                                    <table width="100%">
                                                        <asp:Repeater ID="rptApprovedGraphicsAsset" ClientIDMode="Static" runat="server">
                                                            <HeaderTemplate>
                                                                <tr>
                                                                    <th>Action</th>
                                                                    <th>Document Name</th>
                                                                </tr>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr class="CompDetailsAttachments">
                                                                    <td>
                                                                        <asp:LinkButton ID="lnkDeleteApprovedGraphicsAsset" ClientIDMode="Static" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteApprovedGraphicsAsset_Click" CausesValidation="false"></asp:LinkButton>
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
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div class="row GraphicsOnlyProc">
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">Is all procurement information correct?:</label>
                                        <asp:DropDownList ID="ddlIsAllProcInfoCorrect" onchange="IsAllProcInfoCorrectChanged(this);" ClientIDMode="Static" runat="server" ViewStateMode="Enabled" CssClass="form-control CompDetails">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-6 WhatProcInfoHasChanged">
                                    <div class="form-group">
                                        <label class="control-label">What procument information has changed?:</label>
                                        <asp:TextBox ID="txtWhatProcInfoHasChanged" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control CompDetails" Text='' ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div runat="server" id="divTransferSemiBarcodeGeneration" class="divTransferSemiBarcodeGeneration">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <h2>Transfer Semi Barcode Generation </h2>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-4">
                                        <div class="form-group">
                                            <label class="control-label">13 Digit Code:</label>
                                            <asp:TextBox ID="txt13DigitCode" ClientIDMode="Static" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control CompDetails"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-4">
                                        <div class="form-group">
                                            <label class="control-label">14 Digit Barcode:</label>
                                            <asp:TextBox ID="txt14DigitBarcode" ClientIDMode="Static" runat="server" CssClass="form-control CompDetails numericDecimal0" MaxLength="14"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row transferSemi purchasedCandy">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <h2>Operation</h2>
                                </div>
                            </div>
                            <div class="row transferSemi purchasedCandy">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row transferSemi Operations">
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Pack Location:</label>
                                        <asp:DropDownList ID="drpTSPackLocation" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server">
                                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Country of Origin:</label>
                                        <asp:DropDownList ID="drpTSCountryOfOrigin" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server">
                                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Make/Pack & Transfer Location</label>
                                        <asp:DropDownList ID="drpTransferLocation" ClientIDMode="Static" runat="server" CssClass="form-control CompDetails">
                                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row transferSemi purchasedCandy Operations">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Transfer/Purchased Candy SEMI Comments</label>
                                        <asp:TextBox ID="txtSEMIComment" runat="server" CssClass="form-control CompDetails" ClientIDMode="Static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row purchasedCandy Operations">
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Pack Location:</label>
                                        <asp:DropDownList ID="drpPCSPackLocation" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server">
                                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Country of Origin:</label>
                                        <asp:DropDownList ID="drpPCSCountryofOrigin" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server">
                                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Make/Pack & Transfer Location</label>
                                        <asp:DropDownList ID="drpPurchasedCandyLocation" ClientIDMode="Static" runat="server" CssClass="form-control CompDetails">
                                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row semiDetails purchasedCandy candySemi">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <h2>Semi Details</h2>
                                </div>
                            </div>
                            <div class="row semiDetails purchasedCandy candySemi">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row semiDetails purchasedCandy candySemi">
                                <div class="col-xs-12 col-sm-6 col-md-6" id="divTrialsCompleted" runat="server">
                                    <div class="form-group">
                                        <label class="control-label">Trials completed and product set aside for pack trials?:</label>
                                        <asp:DropDownList ID="drpTrialsCompleted" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group">
                                        <label class="control-label">New Formula:</label>
                                        <asp:DropDownList ID="ddlNewFormula" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row semiDetails purchasedCandy candySemi">
                                <div class="col-xs-12 col-sm-6 col-md-3">
                                    <div class="form-group">
                                        <label class="control-label">Shelf Life:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtShelfLife" ClientIDMode="Static" runat="server" CssClass="form-control CompDetails numericDecimal0"></asp:TextBox><span class="input-group-addon"> Days</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row printer">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <h2>Printer Details</h2>
                                </div>
                            </div>
                            <div class="row printer">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row printer">
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group ">
                                        <label class="control-label">Printer/Supplier:</label>
                                        <asp:DropDownList ID="ddlPrinter" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control CompDetails" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Request new Printer/Supplier:</label>
                                        <asp:TextBox ID="txtNewPrinter" CssClass="form-control CompDetails" ClientIDMode="Static" runat="server"></asp:TextBox>

                                        <br />
                                        <asp:Label ID="lblPrinterRequestSent" ClientIDMode="Static" Visible="false" runat="server" CssClass="AccessRequest">Your request has been sent!</asp:Label>
                                        <asp:Label ID="lblPrinterError" ClientIDMode="Static" runat="server" CssClass="Error"></asp:Label>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <label class="control-label"></label>
                                    <%--<asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ButtonControl" OnClientClick="return chkRequest('printer');" OnClick="btnAdd_Click" ClientIDMode="Static" />--%>
                                </div>
                            </div>
                            <div class="row printer">
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <div class="form-group ">
                                        <label class="control-label">Lead Time for Component:</label>
                                        <div class="input-group">
                                            <asp:TextBox ID="txtLeadTimeMaterial" CssClass="form-control CompDetails alphanumericToUpper1" ClientIDMode="Static" runat="server"></asp:TextBox><span class="input-group-addon">Days</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="dvMoveTS" runat="server">
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12">
                                        <h2>Move this component</h2>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                                </div>
                                <div class="row">
                                    <div class="col-xs-12 col-sm-6 col-md-6">
                                        <div class="form-group">
                                            <asp:DropDownList ID="ddlMoveTS" runat="server" CssClass="ddlMoveTS form-control CompDetails">
                                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row attachment">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <h2>Attachments</h2>
                                </div>
                            </div>
                            <div class="row attachment">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row attachment">
                                <div class="col-xs-12 col-sm-3 col-md-4">
                                    <label class="control-label">Upload Dieline File:</label>
                                </div>
                                <div class="col-xs-12 col-sm-9 col-md-4">
                                    <input id="btnUploadDieline" type="button" runat="server" class="ButtonControlAutoSize hidebtn" value="Upload Dieline PDF" onclick="OpenDialog('Upload Dieline', 'Dieline'); return false;" />
                                </div>
                            </div>
                            <div class="row attachment">
                                <div class="col-xs-12 col-sm-9 col-md-8">
                                    <div class="form-group">
                                        <label class="control-label">Hyperlink to Dieline in PLM:</label>
                                        <strong><a id="generatedLinkEdit" clientidmode="Static" runat="server" class="hideItem" target="_blank" href=""></a></strong>
                                        <asp:TextBox ID="txtDielineLinkEdit" ClientIDMode="Static" CssClass="form-control CompDetails" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-9 col-md-4">
                                    &nbsp;<br />
                                    <asp:Button ID="btnGenerateLink" ClientIDMode="Static" type="button" runat="server" CssClass="ButtonControlAutoSize MarginSevenTop" Text="Generate New Link" OnClientClick="return generateLink();" />
                                </div>
                            </div>
                            <asp:UpdatePanel ID="DielineAttachmentTPanel" ClientIDMode="Static" runat="server" ChildrenAsTriggers="true">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnReloadAttachment" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                                        <ProgressTemplate>
                                            <div id="reloadingCover" clientidmode="Static" runat="server" style="height: 165px; position: absolute; width: 100%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                                                <div class="disablingLoadingIcon" id="disablingLoadingDielines" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <div class="row attachment">
                                        <div class="col-xs-12 col-sm-12 col-md-6">
                                            <table width="50%">
                                                <asp:Repeater ID="rptDieline" ClientIDMode="Static" runat="server">
                                                    <HeaderTemplate>
                                                        <tr>
                                                            <th>Action</th>
                                                            <th>Document Name</th>
                                                        </tr>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <tr class="CompDetailsAttachments">
                                                            <td>
                                                                <asp:LinkButton ID="lnkDielineDelete" CssClass="ButtonControlAutoSize" ClientIDMode="AutoID" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteAttachment_Click" CausesValidation="false"></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                            <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="ReloadAttachment HiddenButton hidebtn" OnClick="btnReloadAttachment_Click"></asp:Button>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div class="row">
                                <asp:HiddenField ID="HiddenField3" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnPackagingItemId" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnRequiredCheck" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnTBDIndicator" ClientIDMode="Static" runat="server" />
                                <asp:HiddenField ID="hdnShowRevPrinterSupplierField" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="hdnParentType" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="hdnProductHierarchyLevel1" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="hdnNovelyProject" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="hdnExtMfgkickedoff" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="hdnCoManClassification" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="HiddenField4" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="hdnProjectTypeSubCategory" ClientIDMode="Static" runat="server" Value="" />
                                <asp:HiddenField ID="hdnLOB" ClientIDMode="Static" runat="server" Value="" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button runat="server" ID="btnCancel" class="ButtonControlAutoSize" data-dismiss="modal" OnClientClick="CloseCompDetailsForm(); return false;" Style="float: right; margin-left: 15px;" Text="Cancel"></asp:Button>
                    <asp:Button runat="server" ID="btnSaveValidate" OnClientClick="return checkrequiredValidate_New();" OnClick="btnSaveValidate_Click" class="ButtonControlAutoSize" Style="float: right; margin-left: 15px;" Text="Save and Validate"></asp:Button>
                    <asp:Button runat="server" ID="btnSaveComp" OnClick="btnSaveComp_Click" class="ButtonControlAutoSize" Style="float: right; margin-left: 15px;" Text="Save"></asp:Button>
                </div>
            </div>
        </div>
    </div>
    <h3 class="accordion">
        <asp:Label ID="lblTitle" Text="PM 1st Review Summary" ClientIDMode="Static" CssClass="titlelbl" runat="server"></asp:Label></h3>
    <div class="panel">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>IPF Summary</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Project Type:</label>
                    <asp:TextBox ID="lblProjectType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                    <asp:TextBox ID="lblLineOfBusiness" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Product Hierarchy Level 2:</label>
                    <asp:TextBox ID="lblProductHierarchyLevel2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Material Group 1 (Brand):</label>
                    <asp:TextBox ID="lblBrand" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">First Ship Date:</label>
                    <asp:TextBox ID="lblFirstShipDate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Weeks Until Ship:</label>
                    <asp:TextBox ID="lblWeeksUntilShip" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">12 Month Projected $:</label>
                    <div class="input-group"><span class="input-group-addon">$</span><asp:TextBox ID="lblAnnualProjectedDollars" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Annual Projected Retail Selling Units:</label>
                    <asp:TextBox ID="lblAnnualProjectedUnits" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4" id="dvCustomer">
                <div class="form-group">
                    <label class="control-label">Customer:</label>
                    <asp:TextBox ID="lblCustomer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Expected Gross Margin %:</label>
                    <asp:TextBox ID="lblExpectedGrossMargin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Project Notes:</label>
                    <asp:TextBox ID="lblProjectNotes" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Make/Pack for Finished Good</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Make Location:</label>
                    <asp:TextBox ID="lblManufacturingLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Finished Good Pack Location:</label>
                    <asp:TextBox ID="lblPrimaryPackingLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalHeading2" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2 class="fleft">Timeline</h2>
                <asp:Panel ID="TimelineInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
            </div>
        </div>
        <div class="row RowBottomMargin" id="dvExternalSeperator2" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row" id="dvExternalTimeline" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Is Current Timeline Acceptable?</label>
                    <asp:TextBox ID="txtCurrentTimelineAcceptable" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalLeadTime" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Lead Time From Supplier:</label>
                    <asp:TextBox ID="txtLeadTimeFromSupplier" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalArtwork" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Final Artwork Due to Supplier:</label>
                    <asp:TextBox ID="txtFinalArtworkDueToSupplier" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Distribution</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Designate HUB DC (aka Material: Delivery Plant):</label>
                    <asp:TextBox ID="lblDesignateHUBDC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalHeading" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>External Manufacturing</h2>
            </div>
        </div>
        <div class="row RowBottomMargin" id="dvExternalSeparator" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row" id="dvExternalFields" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Co-Manufacturing Classification:</label>
                    <asp:TextBox ID="lblCoManufacturingClassification" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">External Manufacturer:</label>
                    <asp:TextBox ID="lblExternalManufacturer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">External Packer:</label>
                    <asp:TextBox ID="lblExternalPacker" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Initial Item Setup Form</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Unit UPC:</label>
                    <asp:TextBox ID="lblUnitUPC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Jar/Display UPC:</label>
                    <asp:TextBox ID="lblJarDisplayUPC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Case UCC:</label>
                    <asp:TextBox ID="lblCaseUCC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Pallet UCC:</label>
                    <asp:TextBox ID="lblPalletUCC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <%--Start: BOM Grid--%>
    <div class="row">
        <div class="col-xs-12 col-sm-3 col-md-6">
            <asp:DropDownList ID="ddlComponentTab" runat="server" ClientIDMode="Static">
            </asp:DropDownList>
        </div>
    </div>
    <asp:Literal ID="litScriptBOM" runat="server"></asp:Literal>
    <div id="BOMGrid">
        <h3 class="accordion">
            <asp:Label ID="Label1" ClientIDMode="Static" CssClass="titlelbl" runat="server"></asp:Label>
            <asp:Label ID="lblDesc" ClientIDMode="Static" runat="server"></asp:Label>
        </h3>
        <div id="BOMPages" class="bompe" runat="server">
            <asp:HiddenField ID="hdnParentComponentType" ClientIDMode="Static" runat="server" Value='' />
            <asp:HiddenField ID="hdnParentPackagingType" ClientIDMode="Static" runat="server" Value='' />
            <asp:Button ID="btnPopulateComponent" runat="server" ClientIDMode="Static" CssClass="HiddenButton" OnClick="btnPopulateComponent_Click" />
            <asp:HiddenField ID="hdnPackagingItemIdClicked" ClientIDMode="Static" runat="server" />
            <asp:PlaceHolder ID="noResultsHolder" runat="server" />
            <asp:Literal ID="LiteralBOMGridTableData" runat="server"></asp:Literal>
            <table id="BOMGridTable" class="display" style="background-color: #BCD3F2">
                <thead>
                    <tr>
                        <th class="BOMEdit">Edit</th>
                        <th class="BOMStatus">Status</th>
                        <th class="BOMNewExisting">New/Existing</th>
                        <th class="BOMMat">Component #</th>
                        <th class="BOMMatDesc">Component Description</th>
                        <th class="BOMLikeMat">Like Comp. #</th>
                        <th class="BOMOldMat">Old Comp. #</th>
                        <th class="BOMComp">Component Type</th>
                        <th class="BOMUofM">Pack Unit (UOM)</th>
                        <th class="BOMPackQty">Pack Qty.</th>
                        <th class="BOMFlowthrough">Flowthrough</th>
                        <th class="BOMMove">Move</th>
                        <th class="BOMDelete">Delete</th>
                        <th class="BOMParent">BOMParent</th>
                    </tr>
                </thead>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-3">
            <asp:Button ID="btnAddNewPackagingItem" CssClass="ButtonControlAutoSize" Text="Add New Packaging Component" CausesValidation="false" runat="server" OnClientClick="AddNewPackagingItem(); return false;" />
        </div>
    </div>
    <%--End: BOM Grid--%>
    <div id="hiddenbuttons">
        <asp:HiddenField ID="hdnPackagingComponent" runat="server" />
        <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnComponentStatusChangeIds" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
        <asp:HiddenField ID="hdnComponentype" runat="server" />
        <asp:HiddenField ID="hdnParentID" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnPackagingID" runat="server" />
        <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
        <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
        <asp:HiddenField ID="hdnPackagingNumbers" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnPLMProject" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnDeletedCompIds" runat="server" ClientIDMode="Static" />
        <asp:Button ID="btnDeleteComponent" runat="server" Text="Delete" OnClick="btnDeleteComponent_Click" CssClass="hide" ClientIDMode="Static" />
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>2nd Review Check</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Does all of the information above look correct?</label>
                <asp:DropDownList ID="ddlcorrect" CssClass="required form-control" ClientIDMode="Static" ToolTip="Does all of the information above look correct" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Key Information to Confirm:</label>
                <ul style="margin-left: -22px;">
                    <li>Pack Trial Results </li>
                    <li>BOM Correctness comparing SAP with PMT </li>
                    <li>All Necessary Pack Dimensions Present </li>
                    <li>Pallet Pattern Uploaded if Corrugate is being updated </li>
                    <li>Any New Material Components have Yes stated for New Graphics if they are required </li>
                    <li>All flow through drop downs are chosen when relevant </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">SGS Expedited Workflow Approved?</label>
                <asp:DropDownList ID="ddlSGSExpeditedWorkflowApproved" CssClass="required form-control" ClientIDMode="Static" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
                <p class="comment-block">Requires approval from SGS Team and Andrew Madaychik.</p>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Revised First Ship Date:</label>
                <asp:TextBox ID="txtFirstShipDate" runat="server" BorderStyle="None" CssClass="datePicker required form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Revised First Production Date:</label>
                <asp:TextBox ID="txtProductionDate" runat="server" BorderStyle="None" CssClass="datePicker form-control required"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="false" CssClass="ButtonControl" OnClick="btnSave_Click" />
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return validatePage()" CssClass="ButtonControl" OnClick="btnSubmit_Click" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('title').text("Pages - PM Second Review Form");

        var customer = $("#lblCustomer").val().toLocaleLowerCase().trim();
        if (customer.indexOf("select") == -1 && customer != "") {
            $("#dvCustomer").removeClass("hideItem");
        } else {
            $("#dvCustomer").addClass("hideItem");
        }
        if ($("#ddlcorrect option:selected").text().toLocaleLowerCase() == 'yes') {
            $('#cblSectionConcerns').parent().hide();
        } else {
            $('#cblSectionConcerns').parent().show();
        }
        pageLoadCheck();
        $("#ddlcorrect").change(function () {
            if ($("#ddlcorrect option:selected").text().toLocaleLowerCase() == 'yes') {
                $('#cblSectionConcerns').parent().hide();
            } else {
                $('#cblSectionConcerns').parent().show();
            }
        });
    });
</script>
