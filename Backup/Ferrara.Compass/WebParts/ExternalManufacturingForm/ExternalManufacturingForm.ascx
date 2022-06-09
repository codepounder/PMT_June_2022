<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExternalManufacturingForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.ExternalManufacturingForm.ExternalManufacturingForm" %>

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
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>External Manufacturing</h1>
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
    <div class="row GraphicsHide">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Annual Projected Retail Selling Units:</label>
                <asp:TextBox ID="lblAnnualProjectUnits" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Annual Projected Cases:</label>
                <asp:TextBox ID="lblAnnualProjectedCases" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Annual Projected lbs:</label>
                <asp:TextBox ID="lblAnnualProjectedLbs" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row GraphicsHide">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl1stmonthU" runat="server" class="control-label"><span id="1stmonthU"></span></label>
                <asp:TextBox ID="txtProjectUnit1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl1stmonthC" runat="server" class="control-label"><span id="1stmonthC"></span></label>
                <asp:TextBox ID="txtProjectCase1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl1stmonthL" runat="server" class="control-label"><span id="1stmonthL"></span></label>
                <asp:TextBox ID="txtProjectlbs1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row GraphicsHide">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl2ndmonthU" runat="server" class="control-label"><span id="2ndmonthU"></span></label>
                <asp:TextBox ID="txtProjectUnit2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl2ndmonthC" runat="server" class="control-label"><span id="2ndmonthC"></span></label>
                <asp:TextBox ID="txtProjectCase2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl2ndmonthL" runat="server" class="control-label"><span id="2ndmonthL"></span></label>
                <asp:TextBox ID="txtProjectlbs2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row GraphicsHide">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl3rdmonthU" runat="server" class="control-label"></label>
                <asp:TextBox ID="txtProjectUnit3" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl3rdmonthC" runat="server" class="control-label"></label>
                <asp:TextBox ID="txtProjectCase3" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl3rdmonthL" runat="server" class="control-label"></label>
                <asp:TextBox ID="txtProjectlbs3" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row GraphicsHide">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Unit Cost Target:</label>
                <asp:TextBox ID="lblUnitCostTarget" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Base UOM Target:</label>
                <asp:TextBox ID="txtCaeseCostTarget" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Lbs Cost Target:</label>
                <asp:TextBox ID="txtlbsCostTarget" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-4">
            <div class="form-group">
                <label class="control-label">Revised First Ship Date:</label>
                <asp:TextBox ID="lblRevisedFirstShipDate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
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
    <asp:HiddenField ID="hdnPackagingComponent" runat="server" />
    <asp:HiddenField ID="hdnParentID" runat="server" />
    <asp:HiddenField ID="hdnPackagingID" runat="server" />
    <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnPackagingNumbers" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnComponentStatusChangeIds" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
    <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
    <asp:HiddenField ID="hdnNovelty" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnComan" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnPLMProject" runat="server" ClientIDMode="Static" />

    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Define Project</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>External Manufacturing Lead:</label>
                <SharePoint:PeopleEditor ID="peProjectLead" runat="server" AllowEmpty="false" MultiSelect="false" SelectionSet="User" SharePointGroup="External Manufacturing Members" />
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Procurement Type:</label>
                <asp:DropDownList ID="ddlCoManufacturingClassification" onchange="conditionalChecks();" ClientIDMode="Static" runat="server" AppendDataBoundItems="true" CssClass="required form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row GraphicsHide">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="dvDoesBulkSemiExistToBringInHouse">
                <label class="control-label"><span class="markrequired">*</span>Does Bulk Semi Exist to Bring In-House?:</label>
                <asp:DropDownList ID="ddlDoesBulkSemiExistToBringInHouse" runat="server" ClientIDMode="Static" onchange="conditionalChecks();" CssClass="form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row GraphicsHide">
        <div id="dvExistingBulkSemiNumber">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>What is the Existing Bulk Semi #?:</label>
                    <asp:TextBox ID="txtExistingBulkSemiNumber" ClientIDMode="Static" runat="server" MaxLength="6" onblur="checkNumberLength('txtExistingBulkSemiNumber',5)" CssClass="numericNoMask minimumlength form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                    <asp:Button runat="server" ID="btnFind" ClientIDMode="Static" OnClientClick="getCompDescription('txtExistingBulkSemiNumber','txtBulkSemiDescription');return false;" CssClass="ButtonControl" Text="Find" />
                </div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3" id="dvBulkSemiDescription">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Bulk Semi Description:</label>
                <asp:TextBox ID="txtBulkSemiDescription" ClientIDMode="Static" runat="server" MaxLength="60" CssClass="form-control"></asp:TextBox>

            </div>
        </div>
    </div>
    <div id="printerSupplierProcSec" class="printerSupplierProcSec GraphicsHide" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Review Printer/Supplier (Proc)?</h2>
            </div>
        </div>

        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <asp:Repeater ID="rptPrinterSupplierFG" runat="server" OnItemDataBound="rptPrinterSupplierFG_ItemDataBound">
            <HeaderTemplate>
                <h3 class="FinishedGoodSec">Finished Good:</h3>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="row FinishedGoodSec">
                    <div class="col-xs-12 col-sm-12 col-md-3">
                        <label class="control-label" id="lblComponentType"><span class="markrequired">*</span><%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %></label>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-md-2">
                        <asp:HiddenField ID="hdnPackagingItemId" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                        <asp:DropDownList ID="ddlReviewPrinterSupplier" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="required form-control">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptPrinterSupplierTS" runat="server" OnItemDataBound="rptPrinterSupplierTS_ItemDataBound">
            <ItemTemplate>
                <h3 class="TransferSemiSec">Transfer Semi:
                        <asp:Label ID="lblMatNumber" runat="server"></asp:Label>
                    :
                        <asp:Label ID="lblMatDesc" runat="server"></asp:Label></h3>
                <asp:Repeater ID="rptTSChildren" runat="server" OnItemDataBound="rptTSChildren_ItemDataBound">
                    <ItemTemplate>
                        <div class="row TransferSemiSec">
                            <div class="col-xs-12 col-sm-12 col-md-3">
                                <label class="control-label" id="lblComponentType"><span class="markrequired">*</span><%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %></label>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-2">
                                <asp:HiddenField ID="hdnPackagingItemId" runat="server" />
                                <asp:DropDownList ID="ddlReviewPrinterSupplier" runat="server" AppendDataBoundItems="true" CssClass="required form-control">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="rptPrinterSupplierPCS" runat="server" OnItemDataBound="rptPrinterSupplierPCS_ItemDataBound">
            <ItemTemplate>
                <h3 class="PCSSec">Purchased Candy Semi:
                        <asp:Label ID="lblMatNumber" runat="server"></asp:Label>
                    :
                        <asp:Label ID="lblMatDesc" runat="server"></asp:Label></h3>
                <asp:Repeater ID="rptPCSChildren" runat="server" OnItemDataBound="rptPCSChildren_ItemDataBound">
                    <ItemTemplate>
                        <div class="row PCSSec">
                            <div class="col-xs-12 col-sm-12 col-md-3">
                                <label class="control-label" id="lblComponentType"><span class="markrequired">*</span><%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %></label>
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-2">
                                <asp:HiddenField ID="hdnPackagingItemId" runat="server" />
                                <asp:DropDownList ID="ddlReviewPrinterSupplier" runat="server" AppendDataBoundItems="true" CssClass="required form-control">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Suppliers</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label" id="ddlExternalManufacturerLabel"><span class="markrequired">*</span>External Manufacturer:</label>
                <asp:DropDownList ID="ddlExternalManufacturer" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label" id="ddlManufacturerCountryOfOriginLabel"><span class="markrequired">*</span>Manufacturer Country of Origin:</label>
                <asp:DropDownList ID="ddlManufacturerCountryOfOrigin" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="required form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6" id="dvddlExternalPacker">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>External Finished Good Packer:</label>
                <asp:DropDownList ID="ddlExternalPacker" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="required form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Purchased Into Location:</label>
                <asp:DropDownList ID="ddlPurchasedIntoLocation" runat="server" AppendDataBoundItems="true" CssClass="required form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdateTS" runat="server" ClientIDMode="Static">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Purchased Candy SEMIs</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div id="ExtPS">
                <asp:PlaceHolder ID="phPurchasedSemiFields" runat="server"></asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Timeline</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Is Current Timeline Acceptable?:</label>
                <asp:DropDownList ID="ddlCurrentTimelineAcceptable" runat="server" ClientIDMode="Static" onchange="onCurrentTimeChange();" AppendDataBoundItems="true" CssClass="required form-control">
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
                <label class="control-label" id="ddlLeadTimeFromSupplierLabel"><span class="markrequired">*</span>Lead Time From Supplier:</label>
                <asp:DropDownList ID="ddlLeadTimeFromSupplier" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="required form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Final Artwork Due to Supplier:</label>
                <asp:TextBox ID="txtFinalArtworkDueToSupplier" ClientIDMode="Static" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
            </div>
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
            <label id="btnUploadDielineLabel" class="control-label">Upload Dieline:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">
            <input id="btnUploadDieline" type="button" class="ButtonControlAutoSize" value="Upload Dieline" onclick="openBasicDialog('Upload Dieline', 'Dieline'); return false;" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table style="width: 100%;">
                <asp:Repeater ID="rpDielines" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>Action</th>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkDielineDelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDielineDelete_Click" CausesValidation="false"></asp:LinkButton>
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
    <div class="row">
        <div class="col-xs-12 col-sm-3 col-md-4">
            <label class="control-label">Upload NLEA Statement:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">
            <input id="btnUploadNLEA" type="button" class="ButtonControlAutoSize" value="Upload NLEA Statement" onclick="openBasicDialog('Upload NLEA Statement', 'NLEA'); return false;" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table style="width: 100%;">
                <asp:Repeater ID="rpNLEAs" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>Action</th>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkNLEADelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkNLEADelete_Click" CausesValidation="false"></asp:LinkButton>
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
                <label class="control-label">Confirm packaging supplier & dieline has stayed the same:</label>
                <asp:DropDownList ID="ddlPackSupplierAndDielineSame" onchange="PackSupplierAndDielineSameChanged(this);" ClientIDMode="Static" runat="server" ViewStateMode="Enabled" CssClass="form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    <asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 WhatChangeIsRequiredExtMfg">
            <div class="form-group">
                <label class="control-label">What change is required?:</label>
                <asp:TextBox ID="txtWhatChangeIsRequiredExtMfg" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control GraphicsBrief" Text='' ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-3 col-md-4">
            <label class="control-label">Upload Packaging Specifications:</label>
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4">
            <input id="btnUploadPackSpecs" type="button" class="ButtonControlAutoSize" value="Upload Packaging Specifications" onclick="openBasicDialog('Upload Packaging Specifications', 'PackagingSpecifications'); return false;" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table style="width: 100%;">
                <asp:Repeater ID="rpPackSpecs" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>Action</th>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkPACKSPECSDelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkPACKSPECSDelete_Click" CausesValidation="false"></asp:LinkButton>
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
    <div class="row">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="ReloadAttachment HiddenButton hidebtn" OnClick="btnReloadAttachment_Click"></asp:Button>

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
            <asp:HiddenField ID="hdnProjectType" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        conditionalChecks();
        pageLoadCheck();

        var projectType = $("#hdnProjectType").val();
        $(".GraphicsHide").each(function () {
            if (projectType == "Graphics Change Only") {
                $(this).addClass('hideItem');
            } else {
                if (!$(this).hasClass('printerSupplierProcSec')) {
                    $(this).removeClass('hideItem');
                }
            }
        });

        $(".GraphicsOnly").each(function () {
            $(this).addClass('hideItem');
        });

        if (projectType == "Graphics Change Only") {
            $(".GraphicsOnly").each(function () {
                $(this).removeClass('hideItem');
                $(this).find("#ddlPackSupplierAndDielineSame").addClass("required");
                $(this).find("#ddlPackSupplierAndDielineSame").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
            });
        }

        PackSupplierAndDielineSameChanged();
    });

    function PackSupplierAndDielineSameChanged() {
        var projectType = $("#hdnProjectType").val();
        var GraphicsOnly = $("#ddlPackSupplierAndDielineSame").closest('.GraphicsOnly');
        GraphicsOnly.find(".WhatChangeIsRequiredExtMfg").removeClass("required");
        GraphicsOnly.find(".WhatChangeIsRequiredExtMfg").find(".markrequired").remove()
        GraphicsOnly.find(".WhatChangeIsRequiredExtMfg").addClass('hideItem');
        GraphicsOnly.find(".WhatRegulatoryInfoIsIncorrect").parent().addClass('hideItem');

        if (projectType == "Graphics Change Only") {
            if ($("#ddlPackSupplierAndDielineSame").val() == "No") {
                GraphicsOnly.find(".WhatChangeIsRequiredExtMfg").removeClass('hideItem');
                GraphicsOnly.find("#txtWhatChangeIsRequiredExtMfg").addClass("required");
                GraphicsOnly.find("#txtWhatChangeIsRequiredExtMfg").closest(".form-group").prepend($("<span class='markrequired'>*</span>"));
                GraphicsOnly.find(".WhatRegulatoryInfoIsIncorrect").parent().removeClass('hideItem');
            } else {
                GraphicsOnly.find("#txtWhatChangeIsRequiredExtMfg").val("");
            }
        }
    }
    function GotoPeoplePickerStepAndFocus(arg) {
        var scrollTo = $("div[title='People Picker']").eq(arg);
        scrollTo.addClass("highlightElement");
        scrollTo.focus();
        $('html, body').animate({
            scrollTop: scrollTo.offset().top - 100
        }, 1000);
    }
    function onCurrentTimeChange() {
        if ($('#ddlCurrentTimelineAcceptable option:selected').val() == 'N') {
            $('#ddlLeadTimeFromSupplier').addClass('required');
            $('#ddlLeadTimeFromSupplierLabel').html("<span class='markrequired'>*</span>Lead Time From Supplier:");
        } else {
            $('#ddlLeadTimeFromSupplier').removeClass('required');
            $('#ddlLeadTimeFromSupplierLabel').html("Lead Time From Supplier:");
        }
    }
    function conditionalChecks() {
        var projectType = $("#hdnProjectType").val();

        if ($("#ddlCoManufacturingClassification option:selected").text().trim().indexOf('External Turnkey Semi') != -1) {
            if (projectType != "Graphics Change Only") {
                $("#ddlDoesBulkSemiExistToBringInHouse").addClass("required");
            }
            $("#dvDoesBulkSemiExistToBringInHouse").show();
            $('#ddlExternalManufacturer').show();
            $('#ddlExternalManufacturerLabel').show();
            $("#ddlExternalManufacturer").addClass("required");
            $("#dvExistingBulkSemiNumber").hide();
            $("#dvBulkSemiDescription").hide();
            $("#dvddlExternalPacker").hide();
            $("#dvMakePackTransfer").hide();
            $("#ddlExternalPacker").removeClass("required");
            $('#ddlManufacturerCountryOfOrigin').show();
            $('#ddlManufacturerCountryOfOriginLabel').show();
        }
        else if ($("#ddlCoManufacturingClassification option:selected").text().trim() == 'External Turnkey FG') {
            $("#ddlDoesBulkSemiExistToBringInHouse").removeClass("required");
            $("#dvDoesBulkSemiExistToBringInHouse").hide();
            $('#ddlExternalManufacturer').show();
            $('#ddlExternalManufacturerLabel').show();
            $("#ddlExternalManufacturer").addClass("required");
            $("#dvExistingBulkSemiNumber").hide();
            $("#dvBulkSemiDescription").hide();
            $("#dvMakePackTransfer").hide();
            $('#ddlManufacturerCountryOfOrigin').show();
            $('#ddlManufacturerCountryOfOriginLabel').show();
            $("#dvddlExternalPacker").show();
            $("#ddlExternalPacker").addClass("required");
        }
        else if ($("#ddlCoManufacturingClassification option:selected").text().trim() == 'External Subcon Semi') {
            $("#ddlDoesBulkSemiExistToBringInHouse").removeClass("required");
            $("#dvDoesBulkSemiExistToBringInHouse").hide();
            $("#dvExistingBulkSemiNumber").hide();
            $("#dvBulkSemiDescription").hide();
            $("#dvMakePackTransfer").hide();
            $('#ddlExternalManufacturer').hide();
            $('#ddlExternalManufacturer').removeClass('required');
            $('#ddlExternalManufacturerLabel').hide();
            $("#ddlExternalManufacturerLabel").removeClass("required");
            $('#ddlManufacturerCountryOfOrigin').hide();
            $('#ddlManufacturerCountryOfOriginLabel').hide();
            $("#dvddlExternalPacker").show();
            $("#ddlExternalPacker").addClass("required");
        }
        else {
            $("#ddlDoesBulkSemiExistToBringInHouse").removeClass("required");
            $("#dvDoesBulkSemiExistToBringInHouse").hide();
            $("#dvExistingBulkSemiNumber").hide();
            $("#dvBulkSemiDescription").hide();
        }

        if ($("#ddlDoesBulkSemiExistToBringInHouse option:selected").text().trim() == 'Yes' && $('#dvDoesBulkSemiExistToBringInHouse').css('display') != 'none') {
            $("#txtExistingBulkSemiNumber").addClass("required");
            $("#dvExistingBulkSemiNumber").show();
            $("#dvBulkSemiDescription").show();
            $("#dvMakePackTransfer").show();
        }
        else if ($("#ddlDoesBulkSemiExistToBringInHouse option:selected").text().trim() == 'No') {
            $("#dvExistingBulkSemiNumber").hide();
            $("#dvBulkSemiDescription").show();
            $("#dvMakePackTransfer").show();
            $("#txtExistingBulkSemiNumber").removeClass("required");
            $("#txtBulkSemiDescription").addClass("required");
        }
        if ($("#ddlDoesBulkSemiExistToBringInHouse option:selected").val() == -1 || $('#dvDoesBulkSemiExistToBringInHouse').css('display') == 'none') {
            $("#txtBulkSemiDescription").removeClass("required");
            $("#dvBulkSemiDescription").hide();
            $("#dvExistingBulkSemiNumber").hide();
            $("#dvMakePackTransfer").hide();
        }
        if ($('#ddlCurrentTimelineAcceptable option:selected').val() == 'N') {
            $('#ddlLeadTimeFromSupplier').addClass('required');
            $('#ddlLeadTimeFromSupplierLabel').html("<span class='markrequired'>*</span>Lead Time From Supplier:");
        } else {
            $('#ddlLeadTimeFromSupplier').removeClass('required');
            $('#ddlLeadTimeFromSupplierLabel').html("Lead Time From Supplier:");
        }
        if ($("#hdnNovelty").val() == "Yes" || $("#ddlCoManufacturingClassification option:selected").text().trim() == 'External Turnkey FG' || $("#hdnComan").val().indexOf('000000027') != -1) {
            $(".printerSupplierProcSec").addClass("hideItem");
            $(".printerSupplierProcSec .required").each(function () {
                $(this).removeClass("required");
            });
        } else {
            if ($("#ddlCoManufacturingClassification option:selected").text().trim() == 'Ferrara Finished Good') {
                $(".printerSupplierProcSec").removeClass("hideItem");
                $(".printerSupplierProcSec .PCSSec select").each(function () {
                    $(this).addClass("required");
                });
                $(".printerSupplierProcSec .FinishedGoodSec select").each(function () {
                    $(this).addClass("required");
                });
                $(".printerSupplierProcSec .TransferSemiSec").addClass("hideItem");
                $(".printerSupplierProcSec .TransferSemiSec select").removeClass("required");
            } else {
                $(".printerSupplierProcSec").removeClass("hideItem");
                $(".printerSupplierProcSec select").each(function () {
                    $(this).addClass("required");
                });

                $(".printerSupplierProcSec .TransferSemiSec").removeClass("hideItem");
                $(".printerSupplierProcSec .TransferSemiSec select").addClass("required");
            }
        }
    }
</script>
