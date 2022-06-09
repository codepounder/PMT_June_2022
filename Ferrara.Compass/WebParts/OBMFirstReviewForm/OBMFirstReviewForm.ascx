<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OBMFirstReviewForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.OBMFirstReviewForm.OBMFirstReviewForm" %>

<style>
    #cblSectionConcerns label {
        margin-left: 10px;
    }
</style>
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
            <h1>PM First Review</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
            <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2 class="fleft">IPF Summary</h2>
            <asp:Panel ID="ipfCompletionInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
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
                <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
                <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
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
                <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                <asp:TextBox ID="lblLineOfBusiness" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Material Group 1 (Brand):</label>
                <asp:TextBox ID="lblBrand" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
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
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Expected Gross Margin %:</label>
                <asp:TextBox ID="lblExpectedGrossMargin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4" id="dvCustomer">
            <div class="form-group">
                <label class="control-label">Customer:</label>
                <asp:TextBox ID="lblCustomer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
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
            <h2 class="fleft">Operations Form</h2>
            <asp:Panel ID="makePackCompletionInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Make Location:</label>
                <asp:TextBox ID="txtMakeLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Make Country of Origin:</label>
                <asp:TextBox ID="txtCountryOrigin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Finished Good Pack Location:</label>
                <asp:TextBox ID="txtPackingLocation1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row" id="dvExternalFields" runat="server">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Co-Manufacturing Classification:</label>
                <asp:TextBox ID="lblCoManufacturingClassification" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div id="divCapacityComments" class="form-group">
                <label class="control-label">Comments on Capacity/Risk:</label>
                <asp:TextBox ID="txtCapacityComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" ReadOnly="True"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments on Project Acceptance:</label>
                <asp:TextBox ID="txtProjectAcceptance" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6" ReadOnly="True"></asp:TextBox>
            </div>
        </div>
    </div>
    <div id="dvExternalManufacturing" runat="server" ClientIDMode="Static">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2 class="fleft">External Manufacturing: Timeline</h2>
                <asp:Panel ID="TimelineInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row" id="Div1" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Is Current Timeline Acceptable?</label>
                    <asp:TextBox ID="txtCurrentTimelineAcceptable" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Lead Time From Supplier:</label>
                    <asp:TextBox ID="txtLeadTimeFromSupplier" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Final Artwork Due to Supplier:</label>
                    <asp:TextBox ID="txtFinalArtworkDueToSupplier" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2 class="fleft">Distribution</h2>
            <asp:Panel ID="distributionCompletionInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
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
    <div id="dvTransferSemiHeader" class="row" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2 class="fleft">Transfer Semi</h2>
            <asp:Panel ID="transferSemiCompletionInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
        </div>
    </div>
    <div id="dvTransferSemiSeparator" class="row RowBottomMargin" runat="server">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvTransferSemiRepeater" class="row RowBottomMargin repeater" runat="server">
        <div class="form-group">
            <table style="border-spacing: 5px; border-collapse: separate;">
                <asp:Label ID="lblNoTransferSemi" runat="server" Text="No Transfer Semis Found!" Visible="false" CssClass="control-label"></asp:Label>
                <asp:Repeater ID="rptTransferSemi" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>New or Existing?</th>
                            <th>Transfer SEMI #</th>
                            <th>Description</th>
                            <th>Make/Pack & Transfer Locations</th>
                            <th>Comments</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:TextBox ID="lblNewExisting" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "NewExisting") %>' Width="120px"></asp:TextBox>
                                <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            </td>
                            <td>
                                <asp:TextBox ID="lblMaterialNumber" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="lblMaterialDescription" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' Width="300px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="lblXferLocation" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "TransferSEMIMakePackLocations") %>' Width="250px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:TextBox ID="lblSEMIComment" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "Notes") %>' Width="300px"></asp:TextBox>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2 class="fleft">InTech Regulatory Form</h2>
            <asp:Panel ID="QACompletionInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row RowBottomMargin repeater">
        <div class="form-group">
            <table style="border-spacing: 5px; border-collapse: separate; width: 100%;">
                <asp:Label ID="lblNoCandySemi" runat="server" Text="No Candy Semis Found!" Visible="false" CssClass="control-label"></asp:Label>
                <asp:Repeater ID="rptCandiSemi" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th style="width: 25%;">Candy Semi Type</th>
                            <th style="width: 25%;">Candy Semi #</th>
                            <th style="width: 50%;">Candy Semi Description</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="text" disabled="disabled" id="txtCandiSemiType" class="form-control"
                                    title="Candi Semi Type" readonly="readonly" value='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>' />
                            </td>
                            <td>
                                <input type="text" disabled="disabled" id="txtCandiSemiNumber" class="form-control"
                                    title="Candi Semi #" readonly="readonly" value='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' />
                            </td>
                            <td>
                                <input type="text" disabled="disabled" id="txtCandiSemiDescription" class="form-control"
                                    title="Candi Semi Description" maxlength="40" readonly="readonly" value='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2 class="fleft">Initial Item Setup Form</h2>
            <asp:Panel ID="InitialSetupCompletionInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
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
        <div class="col-xs-12 col-sm-6 col-md-4" id="caseUCCDiv" runat="server">
            <div class="form-group">
                <label class="control-label">Case UCC:</label>
                <asp:TextBox ID="lblCaseUCC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4" id="palletUCCDiv" runat="server">
            <div class="form-group">
                <label class="control-label">Pallet UCC:</label>
                <asp:TextBox ID="lblPalletUCC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Finished Goods</h2>
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
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>NLEA Attachments</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div id="noNLEAMessage" visible="false" runat="server">No NLEA Attachments Found</div>
            <table>
                <asp:Repeater ID="rpAttachments" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
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
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>1st Review Check</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Is all of the information above complete?:</label>
                <asp:DropDownList ID="ddlOBMFirstReviewConfirmation" runat="server" AppendDataBoundItems="true" CssClass="form-control required" ClientIDMode="Static" onchange="conditionalFirstRevChecks();">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group" id="dvSectionsOfConcern">
                <span class="markrequired">*</span><label class="control-label">Which sections are not complete?:</label>
                <asp:CheckBoxList ID="cblSectionConcerns" ClientIDMode="Static" runat="server" CssClass="required">
                    <asp:ListItem Text="IPF Summary" Value="IPF"></asp:ListItem>
                    <asp:ListItem Text="Make/Pack" Value="MAKE"></asp:ListItem>
                    <asp:ListItem Text="Distribution" Value="DIST"></asp:ListItem>
                    <asp:ListItem Text="External Manufacturing" Value="EXT"></asp:ListItem>
                    <asp:ListItem Text="Transfer Semi" Value="XFER"></asp:ListItem>
                    <asp:ListItem Text="Quality Assurance" Value="QA"></asp:ListItem>
                    <asp:ListItem Text="Initial Item Setup" Value="INIT"></asp:ListItem>
                </asp:CheckBoxList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div id="dvOBMFirstReviewComments" class="form-group">
                <label class="control-label">Comments:</label>
                <asp:TextBox ID="txtOBMFirstReviewComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>1st Review Project Status</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Does First Ship Date need to be revised?:</label>
                <asp:DropDownList ID="ddlRevisedFirstShipDate" runat="server" AppendDataBoundItems="true" CssClass="form-control required" ClientIDMode="Static" onchange="conditionalFirstRevChecks();">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-6" id="dvRevisedFirstShipDate">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Revised First Ship Date:</label>
                <asp:TextBox ID="txtRevisedFirstShipDate" ClientIDMode="Static" runat="server" CssClass="datePicker required form-control" ToolTip="Click to Choose Date"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12" id="dvFirstShipRevisionComments">
            <div class="form-group">
                <label class="control-label">Comments on First Ship Revision:</label>
                <asp:TextBox ID="txtFirstShipRevisionComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6" id="dvFirstProductionDate">
            <div class="form-group">
                <label class="control-label">First Production Date:</label>
                <asp:TextBox ID="txtFirstProductionDate" ClientIDMode="Static" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return validatePage()" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
            <asp:HiddenField ID="hdnPLMProject" runat="server" />
            
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('title').text("Pages - PM First Review Form");
        conditionalFirstRevChecks();
        pageLoadCheck();
    });
</script>
