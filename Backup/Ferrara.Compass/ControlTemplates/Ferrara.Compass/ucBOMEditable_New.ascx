<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBOMEditable_New.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucBOMEditable_New" %>

<h3 class="accordion">Component Details</h3>
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
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Component Type:</label>
                    <asp:DropDownList ID="drpPkgComponent" CssClass="form-control" onchange="NewPackagingItem();" ClientIDMode="Static" runat="server">
                        <asp:ListItem Text="--Select Packaging Component--" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnSAPMatGroup" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnPackagingComponent" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnIsNew" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnParentID" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdn13DigitCode" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnTSBarcodeGenerationVisibility" ClientIDMode="Static" runat="server" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">New/Existing:</label>
                    <asp:DropDownList ID="drpNew" onchange="BOMNewCondition(this);" ClientIDMode="Static" runat="server" ViewStateMode="Enabled" CssClass="form-control">
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
                    <asp:DropDownList ID="ddlPackUnit" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Pack Quantity:</label>
                    <asp:TextBox ID="txtPackQty" CssClass="form-control minimumlength numericDecimal3" ClientIDMode="Static" runat="server"></asp:TextBox>
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
                    <asp:DropDownList ID="drpGraphicsNeeded" runat="server" CssClass="form-control drpGraphics" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4 col-md-4">
                <div class="form-group">
                    <label class="control-label">Component Contains NLEA?:</label>
                    <asp:DropDownList ID="drpComponentContainsNLEA" runat="server" CssClass="form-control drpGraphics" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4 col-md-4">
                <div class="form-group">
                    <label class="control-label">Graphics Vendor:</label>
                    <asp:DropDownList ID="ddlGraphicsVendor" runat="server" CssClass="form-control drpGraphicsVendor" ClientIDMode="Static">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Flowthrough:</label>
                    <asp:DropDownList ID="ddlFlowthrough" runat="server" CssClass="form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4 PurchasedCandyLocation" runat="server" clientidmode="Static" id="dvPurchasedInto">
                <div class="form-group">
                    <label class="control-label">Purchased Into Location:</label>
                    <asp:DropDownList ID="ddlPurchasedIntoLocation" runat="server" CssClass="form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4 SpecificationNo" runat="server" clientidmode="Static" id="dvSpecification">
                <div class="col-xs-12 col-sm-6 col-md-9">
                    <div class="form-group">
                        <label class="control-label">Packaging Specification #:</label>
                        <asp:TextBox ID="txtSpecificationNo" CssClass="form-control minimumlength alphanumericToUpper1" ClientIDMode="Static" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <label class="control-label"></label>
                    <asp:Button ID="btnLookupSpecificationNo" CssClass="ButtonControlAutoSize hidebtn" runat="server" Text="Find" OnClientClick="getPackagingComponentSpecificationNumberFromPLM('txtMaterial','txtSpecificationNo');return false;" Style="margin-left: -22px; margin-top: 5px;" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4" id="dvRevPrinterSupplier" runat="server" clientidmode="Static">
                <div class="form-group">
                    <label class="control-label">Review Printer-Supplier (Proc)?:</label>
                    <asp:DropDownList ID="ddlReviewPrinterSupplier" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="form-control" Enabled="false">
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
                    <asp:DropDownList ID="ddlFilmPrintStyle" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6" runat="server" id="dvSAPDescAbrev" clientidmode="Static" visible="false">
                <div class="form-group">
                    <label class="control-label">SAP Description Abbreviation:</label>
                    <asp:DropDownList ID="ddlSAPDescAbrev" runat="server" CssClass="form-control" ClientIDMode="Static" Enabled="false">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label">Component #:</label>
                    <asp:TextBox ID="txtMaterial" CssClass="form-control minimumlength alphanumericToUpper1 Component" ViewStateMode="Enabled" MaxLength="20" ClientIDMode="Static" runat="server"></asp:TextBox>
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
                    <asp:TextBox ID="txtMaterialDescription" CssClass="form-control ComponentDesc" ClientIDMode="Static" MaxLength="40" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label">Like Component #:</label>
                    <asp:TextBox ID="txtLikeItem" CssClass="form-control minimumlength alphanumericToUpper1 OldComponent" ViewStateMode="Enabled" MaxLength="20" ClientIDMode="Static" runat="server"></asp:TextBox>

                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label"></label>
                    <asp:Button ID="btnOldComponet" CssClass="ButtonControl hidebtn" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtLikeItem','txtLikeDescription');return false;" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-8">
                <div class="form-group">
                    <label class="control-label">Like Component Description:</label>
                    <asp:TextBox ID="txtLikeDescription" CssClass="form-control OldComponentDesc" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label">Old Component #:</label>
                    <asp:TextBox ID="txtOldMaterial" ClientIDMode="Static" runat="server" CssClass="alphanumericToUpper1 minimumlength form-control" MaxLength="20"></asp:TextBox>
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
                    <asp:TextBox ID="txtOldMaterialDesc" ClientIDMode="Static" runat="server" CssClass="form-control minimumlength alphanumericToUpper1 OldComponent"></asp:TextBox>
                </div>
            </div>
        </div>
        <asp:UpdatePanel ID="hierarchyPanel2" ClientIDMode="Static" runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlPHL1" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlPHL2" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlBrand" EventName="SelectedIndexChanged" />
            </Triggers>
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                    <ProgressTemplate>
                        <div id="reloadingCover" clientidmode="Static" runat="server" style="height: 165px; position: absolute; width: 100%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                            <div class="disablingLoadingIcon" id="disablingLoadingHierarchy" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div class="row ipf TSOnlyRow hideItem new">
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 1:</label>
                            <asp:DropDownList ID="ddlPHL1" CssClass="required PHL1 form-control" OnSelectedIndexChanged="ddlProductHierarchyLevel1_SelectedIndexChanged" runat="server"
                                AppendDataBoundItems="true" AutoPostBack="True" ClientIDMode="Static">
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
                            <asp:DropDownList ID="ddlPHL2" OnSelectedIndexChanged="ddlProductHierarchyLevel2_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
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
                            <asp:DropDownList ID="ddlBrand" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" AutoPostBack="True" ClientIDMode="Static" CssClass="required form-control" runat="server" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            </asp:DropDownList>
                            <p class="comment-block">
                                <asp:Label ID="lblBrand" CssClass="comment-block" runat="server"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
                <div class="row ipf TSOnlyRow hideItem new">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Profit Center:</label>
                            <asp:TextBox ID="txtProfitCenterUC" ClientIDMode="Static" runat="server" ReadOnly="true" class="required form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="row hideableRow ipf">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">How is it a Like Component #:</label>
                    <asp:TextBox ID="txtLikeMaterial" TextMode="MultiLine" ToolTip="Does like component # have same dieline, graphics, etc." Rows="3" runat="server" CssClass="form-control whyLikeComponent" Text='' ClientIDMode="Static"></asp:TextBox>
                    <label id="lblItemNote" class="comment-block" style="color: #777777">Does like component # have same dieline, graphics, etc.</label>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Graphics Brief:</label>
                    <asp:TextBox ID="txtGraphicsBrief" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control GraphicsBrief" Text='' ClientIDMode="Static"></asp:TextBox>
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
                                            <tr>
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
                    <asp:DropDownList ID="ddlIsAllProcInfoCorrect" onchange="IsAllProcInfoCorrectChanged(this);" ClientIDMode="Static" runat="server" ViewStateMode="Enabled" CssClass="form-control">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6 WhatProcInfoHasChanged">
                <div class="form-group">
                    <label class="control-label">What procument information has changed?:</label>
                    <asp:TextBox ID="txtWhatProcInfoHasChanged" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" Text='' ClientIDMode="Static"></asp:TextBox>
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
                        <asp:TextBox ID="txt13DigitCode" ClientIDMode="Static" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">14 Digit Barcode:</label>
                        <asp:TextBox ID="txt14DigitBarcode" ClientIDMode="Static" runat="server" CssClass="form-control numericDecimal0" MaxLength="14"></asp:TextBox>
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
                    <asp:DropDownList ID="drpTSPackLocation" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Country of Origin:</label>
                    <asp:DropDownList ID="drpTSCountryOfOrigin" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Make/Pack & Transfer Location</label>
                    <asp:DropDownList ID="drpTransferLocation" ClientIDMode="Static" runat="server" CssClass="form-control">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row transferSemi purchasedCandy Operations">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Transfer/Purchased Candy SEMI Comments</label>
                    <asp:TextBox ID="txtSEMIComment" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row purchasedCandy Operations">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Pack Location:</label>
                    <asp:DropDownList ID="drpPCSPackLocation" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Country of Origin:</label>
                    <asp:DropDownList ID="drpPCSCountryofOrigin" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Make/Pack & Transfer Location</label>
                    <asp:DropDownList ID="drpPurchasedCandyLocation" ClientIDMode="Static" runat="server" CssClass="form-control">
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
                    <asp:DropDownList ID="drpTrialsCompleted" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">New Formula:</label>
                    <asp:DropDownList ID="ddlNewFormula" ClientIDMode="Static" CssClass="form-control" runat="server">
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
                        <asp:TextBox ID="txtShelfLife" ClientIDMode="Static" runat="server" CssClass="form-control numericDecimal0"></asp:TextBox><span class="input-group-addon"> Days</span>
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
                    <asp:DropDownList ID="ddlPrinter" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Request new Printer/Supplier:</label>
                    <asp:TextBox ID="txtNewPrinter" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>

                    <br />
                    <asp:Label ID="lblPrinterRequestSent" ClientIDMode="Static" Visible="false" runat="server" CssClass="AccessRequest">Your request has been sent!</asp:Label>
                    <asp:Label ID="lblPrinterError" ClientIDMode="Static" runat="server" CssClass="Error"></asp:Label>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <label class="control-label"></label>
                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="ButtonControl" OnClientClick="return chkRequest('printer');" OnClick="btnAdd_Click" ClientIDMode="Static" />
            </div>
        </div>
        <div class="row printer">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group ">
                    <label class="control-label">Lead Time for Component:</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtLeadTimeMaterial" CssClass="form-control alphanumericToUpper1" ClientIDMode="Static" runat="server"></asp:TextBox><span class="input-group-addon">Days</span>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6 NewPrinterSupplierForLocation hideItem">
                <div class="form-group ">
                    <label class="control-label">Is this a new Printer/Supplier location for this material?:</label>
                    <asp:DropDownList ID="ddlNewPrinterSupplierForLocation" ClientIDMode="Static" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
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
                        <asp:DropDownList ID="ddlMoveTS" runat="server" CssClass="ddlMoveTS form-control">
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
                    <asp:TextBox ID="txtDielineLinkEdit" ClientIDMode="Static" CssClass="form-control" runat="server"></asp:TextBox>
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
                                    <tr>
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
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnRequiredCheck" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnTBDIndicator" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnShowRevPrinterSupplierField" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnParentType" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnProductHierarchyLevel1" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnNovelyProject" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnExtMfgkickedoff" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnCoManClassification" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnProjectType" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnProjectTypeSubCategory" ClientIDMode="Static" runat="server" Value="" />
            <asp:HiddenField ID="hdnLOB" ClientIDMode="Static" runat="server" Value="" />
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" ClientIDMode="Static">
                <ContentTemplate>
                    <div class="col-xs-12 col-sm-6 col-md-8">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button justifyRight" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-2">
                        <asp:Button ID="btnSaveValidate" ClientIDMode="Static" OnClientClick="return checkrequiredValidate();" runat="server" CssClass="ButtonControlAutoSize justifyRight hidebtn" Text="Save and Validate" OnClick="btnSaveValidate_Click" />
                    </div>
                    <div class="colId-xs-12 col-sm-6 col-md-2">
                        <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" OnClientClick="return checkrequired();" CssClass="clickSave button justifyRight hidebtn" Text="Save" OnClick="btnSave_Click" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $(".container:not(.ucmain) > .row").hide();
        if ($("#dvPurchased").length) {
            $("#dvPurchased .row").hide();
        }
        if ($("#dvPurchased").length) {
            $("#dvPurchased .row").hide();
        }
        if ($("#dvCandy").length) {
            $("#dvCandy .row").hide();
        }
        if ($(".repeater.divShipper").length) {
            $(".repeater.divShipper .row").hide();
        }
        if ($("#UpdateTS").length) {
            $("#UpdateTS").hide();
        }
        if ($("#printerSupplierProcSec").length) {
            $("#printerSupplierProcSec").hide();
        }
        //$("#dvMain > .row").show();
        $(".panel").each(function () {
            if (!$(this).closest("#BOMPages").length && !$(this).closest(".dvUserControl").length) {
                $(this).hide();
                $(this).prev().hide();
            }
        });
        if (!$(".panel").closest("#BOMPages").length && !$(".panel").closest(".dvUserControl").length) {
            $(".panel:eq(0)").hide();
            $("h3.accordian:eq(0)").hide();
        }
        $(".dvPackNext").hide();
        $("#BOMPages").hide();
        $("#dvExternalManufacturing").hide();

        TSBarcodeGenerationVisibility();
    });
    function generateLink() {
        var parentName = $("#hdnParentType").val();
        if (parentName == "") {
            parentName = "Finished Good";
        }
        var matNumber = $("#txtMaterial").val();
        if (matNumber == "") {
            matNumber = "XXXXX";
        }
        var linkName = parentName + ": " + matNumber + ": Dieline Link";
        if ($("#txtDielineLink").val() == "") {
            $("#generatedLinkEdit").addClass("hideItem");
            $("#generatedLinkEdit").attr("href", "");
            $("#generatedLinkEdit").html("");
        } else {
            $("#generatedLinkEdit").html(linkName);
            $("#generatedLinkEdit").removeClass("hideItem");
            $("#generatedLinkEdit").attr("href", $("#txtDielineLinkEdit").val());
        }
        loadingIconAdded = true;
        $(".disablingLoadingIcon").remove();
        return false;
    }
</script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js"></script>
