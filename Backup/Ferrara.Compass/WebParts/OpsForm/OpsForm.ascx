<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OpsForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.OpsForm.OpsForm" %>

<style>
    .repeater table td select, .repeater table td input[type='text'] {
        width: 90%;
    }

    .miscOpsClass {
        display: none;
    }
</style>
<div class="container" id="dvMain">
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
            <h1>Operations Form & Initial Capacity Review</h1>
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
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
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
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">SAP Base Unit of Measure:</label>
                <asp:TextBox ID="lblSAPBUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Pack Type:</label>
                <asp:TextBox ID="lblPackType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">First Ship Date:</label>
                <asp:TextBox ID="lblRevisedFirstShipDate" runat="server" ReadOnly="True" ClientIDMode="Static" CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Like FG Item # - same pack type/similar unit weight:</label>
                <asp:TextBox ID="lblLikeFGItem" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <asp:Label ID="lblComments" CssClass="control-label" Text="Item Concept:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                <asp:TextBox ID="txtComments" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row ipfSummary">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>IPF Summary</h2>
        </div>
    </div>
    <div class="row RowBottomMargin ipfSummary">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvUserControl" class="ipfSummary" runat="server">
        <asp:PlaceHolder ID="phMsg" runat="server" />
    </div>
    <asp:PlaceHolder ID="phTransferSemi" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="phPage" runat="server">
        <div id="OpsBOMItems" class="ipfSummary" runat="server" clientidmode="Static">
            <asp:PlaceHolder ID="phBOM" runat="server" />
        </div>
    </asp:PlaceHolder>
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
            <asp:HiddenField ID="hdnComponentype" runat="server" />
            <asp:HiddenField ID="hdnParentID" runat="server" />
            <asp:HiddenField ID="hdnPackagingID" runat="server" />
            <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
            <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
            <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnFGCount" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingNumbers" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnComanClassification" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hddIsTransferSemiIncuded" ClientIDMode="Static" runat="server" />
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
                <span class="markrequired">*</span><label class="control-label">Make Location:</label>
                <asp:DropDownList ID="drpMakeLocation" onchange="makeLocationChange();" ClientIDMode="Static" CssClass="form-control required" runat="server">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired" id="dvCountry">*</span><label class="control-label">Make Country of Origin:</label>
                <asp:DropDownList ID="drpCountryOrigin" ClientIDMode="Static" CssClass="form-control" runat="server">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Finished Good Pack Location:</label>
                <asp:DropDownList ID="drpPackLocation" CssClass="form-control required" runat="server">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div id="dvManufacturing" class="form-group">
                <label class="control-label">Manufacturing Location Change (Network Move):</label>
                <asp:DropDownList ID="ddlNetworkMove" onchange="NetworkMoveChanged()" ClientIDMode="Static" CssClass="form-control" ToolTip="Please select Network Move" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
                <div class="comment-block NetworkMoveComments">If "Yes" is selected for "Network Move Required," IPF will be rejected and returned to Initiator to re-classify & re-submit.</div>
            </div>
        </div>
    </div>
    <div class="row WhatNetworkMoveIsRequired">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <span class="markrequired">*</span>
                <label clientidmode="Static" id="lblChangeNotes" cssclass="control-label" runat="server">What Network Move is Required?:</label>
                <asp:TextBox ID="txtWhatNetworkMoveIsRequired" runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="255" Rows="3" CssClass="required form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div id="dvImmediateSPKChange" class="form-group HideForGraphics">
                <label class="control-label">Immediate SPK Change:</label>
                <asp:DropDownList ID="ddlImmediateSPKChange" ClientIDMode="Static" CssClass="form-control" ToolTip="Please select Immediate SPK Change" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-8">
            <div class="form-group">
                <label class="control-label">Line/Workcenter Additional Info:</label>
                <asp:TextBox ID="txtWorkCenterAddInfo" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row ProjectApproved">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired">*</span>
                <label class="control-label">Project Approved?:</label>
                <asp:DropDownList ID="ddlProjectApproved" onchange="ProjectApprovedChanged()" ClientIDMode="Static" CssClass="form-control required" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
                <div class="comment-block ProjectApprovedComments">If "Yes" is selected for "Network Move Required," IPF will be rejected and returned to Initiator to re-classify & re-submit.</div>
            </div>
        </div>
    </div>
    <div class="row ReasonForRejection">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <span class="markrequired">*</span>
                <label clientidmode="Static" id="Label1" cssclass="control-label" runat="server">Reason for rejection?:</label>
                <asp:TextBox ID="txtReasonForRejection" runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="255" Rows="3" CssClass="required form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <asp:UpdatePanel ID="UpdateTS" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Transfer SEMIs</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-12">
                    <div class="form-group">
                        <span id="lblItemNote">Transfer Semis being externally manufactured should not be included.</span>
                    </div>
                </div>
            </div>
            <div id="OpsTS">
                <asp:PlaceHolder ID="phTransferSemiFields" runat="server"></asp:PlaceHolder>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Purchased Candy SEMIs</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div id="OpsPS">
                <asp:PlaceHolder ID="phPurchasedSemiFields" runat="server"></asp:PlaceHolder>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%-- BEGIN: Fields from Initial Capacity Review --%>
    <div class="HideForGraphics">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Critical Data Points</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Annual Projected Retail Selling Units:</label>
                    <asp:TextBox ID="lblAnnualProjectedUnits" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
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
                    <asp:TextBox ID="lblAnnualProjectLbs" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
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
        <div class="row">
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
        <div class="row">
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
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">First Ship Date:</label>
                    <asp:TextBox ID="lblFirstShipDate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                    <asp:TextBox ID="lblLineOfBusiness" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Initial Capacity Review</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Recommended First Production Date:</label>
                <asp:TextBox ID="txtProductionDate" ClientIDMode="Static" runat="server" CssClass="datePicker form-control required" ToolTip="Click to Choose Date"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="HideForGraphics">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <div class="form-group">
                    <span class="markrequired">*</span><label class="control-label">Will there be any capacity issues at the Make Location?:</label>
                    <asp:DropDownList ID="ddlMakeLocationIssues" runat="server" AppendDataBoundItems="true" CssClass="form-control required NotRequiredforGraphics">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-6">
                <div class="form-group">
                    <span class="markrequired">*</span><label class="control-label">Will there be any capacity issues at the Pack Location?:</label>
                    <asp:DropDownList ID="ddlPackLocationIssues" runat="server" AppendDataBoundItems="true" CssClass="form-control required NotRequiredforGraphics">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div id="divCapacityComments" class="form-group">
                    <label class="control-label">Comments on Capacity/Risk:</label>
                    <asp:TextBox ID="txtCapacityComments" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <div class="form-group">
                    <span class="markrequired">*</span><label class="control-label">Recommendations on Project Acceptance:</label>
                    <asp:DropDownList ID="ddlProjectDecision" runat="server" AppendDataBoundItems="true" CssClass="form-control required NotRequiredforGraphics">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Accept" Value="A"></asp:ListItem>
                        <asp:ListItem Text="Reject" Value="R"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Comments on Project Acceptance:</label>
                    <asp:TextBox ID="txtProjectAcceptance" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
                </div>
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
        <div class="col-xs-12 col-sm-3 col-md-4 CompassLabel">
            Upload Documents:
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4 CompassLabel">
            <asp:Button ID="btnUpload" Text="Upload Documents" OnClientClick="openBasicDialog('Upload Documents','Capacity');return false;" CssClass="ButtonControlAutoSize" runat="server" />
        </div>
    </div>
    <div class="row">
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
    <%-- END: Fields from Initial Capacity Review --%>
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
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" OnClientClick="return ValidateData()" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Reload Attachment" CausesValidation="false" CssClass="ReloadAttachment" OnClick="btnReloadAttachment_Click" />
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('title').text("Pages - Operations & Initial Capacity Review Form");
        var projectType = $("#hdnProjectType").val();
        if ($("#OpsBOMItems").html() == "") {
            $(".ipfSummary").hide();
        } else {
            $(".ipfSummary").show();
        }

        if (projectType == "Graphics Change Only") {
            $("#ddlProjectApproved").parent().removeClass('hideItem');
            $(".ProjectApproved").removeClass('hide');
            $(".NetworkMoveComments").removeClass('hide');
            $(".ProjectApprovedComments").removeClass('hide');

            $(".HideForGraphics").each(function (i, obj) {
                $(this).addClass('hideItem');
            });

            $(".NotRequiredforGraphics").each(function (i, obj) {
                $(this).removeClass('required');
            });

        } else {
            $("#ddlProjectApproved").parent().addClass('hideItem');
            $(".ProjectApproved").addClass('hide');
            $(".NetworkMoveComments").addClass('hide');
            $(".ProjectApprovedComments").addClass('hide');

            $(".HideForGraphics").each(function (i, obj) {
                $(this).removeClass('hideItem');
            });

            $(".NotRequiredforGraphics").each(function (i, obj) {
                $(this).addClass('required');
            });
        }

        conditionalChecks();

        makeLocationChange();

        $(".makeLocation").each(function (index, drpTSMakeLocation) {
            TSMakeLocationChange(drpTSMakeLocation.id);
        });

        if ($(".bompe").length) {
            $(".bompe th:nth-child(1),.bompe th:nth-child(10),.bompe th:nth-child(12)").addClass("hideItem");
            $(".bompe th:nth-child(6),.bompe th:nth-child(7)").removeClass("hideItem");
            $(".bompe td:nth-child(1),.bompe td:nth-child(10),.bompe td:nth-child(12)").addClass("hideItem");
            $(".bompe td:nth-child(6),.bompe td:nth-child(7)").removeClass("hideItem");
        }

        $(".accordion").click(function () {
            var dvNext = $(this).next('div.panel');
            dvNext.toggleClass('hideItem');
        });

        NetworkMoveChanged();
        ProjectApprovedChanged();
    });

    function conditionalChecks() {
        if ($("#drpInternalTransferNeeded option:selected").text() != 'Yes') {
            $("#dvLikeItem").hide();
            $("#dvLikeItemDesc").hide();
            $("#btnFind").hide();
            $("#dvShipper").addClass('hideItem');
        } else {
            $("#dvLikeItem").show();
            $("#dvLikeItemDesc").show();
            $("#btnFind").show();
            $("#dvShipper").removeClass('hideItem');
        }

    }

    function TSMakeLocationChange(callerId) {
        var anchor = $("#" + callerId);
        var makeLocation = anchor.parents("table").find('.makeLocation:eq(0) option:selected').text();
        var packLocation = anchor.parents("table").find('.packLocation:eq(0) option:selected').text();
        var spanOrigin = anchor.parents("table").find('.spanOrigin:eq(0)');
        var selectOrigin = anchor.parents("table").find('.selectOrigin:eq(0)');
        if (makeLocation.indexOf('Externally') == -1 && packLocation.indexOf('Externally') == -1) {
            spanOrigin.show();
            selectOrigin.addClass('BOMrequired');
        }
        else {
            spanOrigin.hide();
            selectOrigin.removeClass('BOMrequired');
        }
    }

    function makeLocationChange() {
        var hddIsTransferSemiIncuded = $("#hddIsTransferSemiIncuded");
        var drpInternalTransferNeeded = $("#drpInternalTransferNeeded");
        if ($("#drpMakeLocation option:selected").text() != "Externally Manufactured") {
            $("#drpCountryOrigin").addClass('required');
            $("#dvCountry").show();
        }
        else {
            $("#drpCountryOrigin").removeClass('required');
            $("#dvCountry").hide();
        }
        if ($("#drpMakeLocation option:selected").text() != "Externally Manufactured" && hddIsTransferSemiIncuded.val() == "1") {

            conditionalChecks();
        }
    }

    function NetworkMoveChanged() {
        var projectType = $("#hdnProjectType").val();
        $(".WhatNetworkMoveIsRequired").addClass('hide');
        $("#txtWhatNetworkMoveIsRequired").parent().addClass('hideItem');
        if (projectType == "Graphics Change Only") {
            if ($("#ddlNetworkMove").val() == "Y") {
                $("#txtWhatNetworkMoveIsRequired").parent().removeClass('hideItem');
                $(".WhatNetworkMoveIsRequired").removeClass('hide');
            }
        }
    }

    function ProjectApprovedChanged() {
        var projectType = $("#hdnProjectType").val();
        $(".ReasonForRejection").addClass('hide');
        $("#txtReasonForRejection").parent().addClass('hideItem');
        if (projectType == "Graphics Change Only") {
            if ($("#ddlProjectApproved").val() == "N") {
                $("#txtReasonForRejection").parent().removeClass('hideItem');
                $(".ReasonForRejection").removeClass('hide');
            }
        }
    }

</script>
