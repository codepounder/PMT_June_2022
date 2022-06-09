<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucConsolidatedFinancialSummary.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucConsolidatedFinancialSummary" %>
<%-- Modal For Financial Brief selection --%>
<div class="row">
    <!-- Modal -->
    <div class="modal fade" id="DialogLoadFinanceBrieffromPreviousGate" role="dialog" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                    <h4 class="modal-title">Copy Finance Briefs</h4>
                </div>
                <div class="modal-body" style="display: flex;">
                    <div class="row">
                        <asp:Label ID="lblFinanceBrieffromPreviousGateNoData" runat="server" Style="color: red;" Font-Bold="true" Text="No Finance Briefs are available to copy."></asp:Label>
                    </div>
                    <asp:Panel ID="LoadFinancialBriefSection" CssClass="FinancialBriefSection" runat="server" Style="width: -webkit-fill-available;">
                        <table class="table">
                            <tbody>
                                <asp:Repeater ID="rptPrevoiusFinancialBriefItem" runat="server" OnItemCommand="rptPrevoiusFinancialBriefItem_ItemCommand" OnItemDataBound="rptPrevoiusFinancialBriefItem_ItemDataBound">
                                    <ItemTemplate>
                                        <asp:Panel runat="server" CssClass="FinancialBriefCopyRow">
                                            <tr>
                                                <th scope="row">
                                                    <asp:CheckBox ID="chkFinanceBrief" CssClass="selectFinanceBrief" CommandName="CheckBoxClick" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Gate") +","+  DataBinder.Eval(Container.DataItem,"BriefNumber") %>' ClientIDMode="Static" runat="server" Style="vertical-align: -7px;" />
                                                </th>
                                                <td>
                                                    <asp:TextBox ID="txtBriefName" CssClass="form-control" runat="server" ClientIDMode="Static" ReadOnly="True"> </asp:TextBox>
                                                </td>
                                                <asp:HiddenField ID="hdnGate" Value='<%# DataBinder.Eval(Container.DataItem, "Gate") %>' runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdnBriefNumber" Value='<%# DataBinder.Eval(Container.DataItem, "BriefNumber") %>' runat="server" ClientIDMode="Static" />
                                            </tr>
                                        </asp:Panel>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </asp:Panel>
                </div>
                <div>
                    <div class="col-xs-12 col-sm-6 col-md-1">
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-11">
                        <asp:Label ID="lblFinanceBrieffromPreviousGateValidationMsg" runat="server" Style="color: red;" Font-Bold="true" ClientIDMode="Static"></asp:Label>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-2">
                        <asp:Button ID="btnGetFinanceBrieffromPreviousGateCancel" OnClientClick="return GetFinanceBrieffromPreviousGateCancel();" class="ButtonControlAutoSize" runat="server" Text="Cancel" />
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <asp:Button ID="btnGetFinanceBrieffromPreviousGate" OnClientClick="return ValdateFinanceBrieffromPreviousGateCancel();" class="ButtonControlAutoSize" OnClick="btnGetFinanceBrieffromPreviousGate_Click" runat="server" Text="Copy Finance Brief" />
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>
<%-- Modal For Financial Brief Copy success messge --%>
<div class="row">
    <!-- Modal -->
    <div class="modal fade" id="DialogStageGateFinanceCopiedMessage" role="dialog" data-keyboard="false" data-backdrop="static">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                    <h4 class="modal-title">Stage Gate Project</h4>
                </div>
                <div class="modal-body" style="display: flex;">
                    <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                    <p style="margin-left: 10px;">
                        <h2>Stage Gate Financial Briefs copied succefully.</h2>
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal" onclick="StageGateFinanceCopiedRedirect()">Ok</button>
                </div>
            </div>
        </div>
    </div>
</div>
<div class="row RowBottomMargin">
    <div class="col-xs-12 col-sm-3 col-md-1">
        <asp:Button ID="btnRedirect" ClientIDMode="Static" runat="server" Text="Redirect" Style="visibility: hidden; display: none;" CssClass="ButtonControl" OnClick="btnRedirect_Click" />
    </div>
</div>
<%--Financial Brief Copy Button--%>
<div class="row RowBottomMargin">
    <div class="col-xs-12 col-sm-3 col-md-10">
    </div>
    <div class="col-xs-12 col-sm-3 col-md-2">
        <asp:Button ID="btnCopyFinanceBrief" OnClick="btnCopyFinanceBrief_Click" runat="server" Text="Copy Finance Brief" CssClass="ButtonControlAutoSize noIcon" />
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12">
        <div class="form-group">
            <label class="control-label"><span class="markrequired">*</span>Brief Summary:</label>
            <asp:TextBox ID="txtBriefSummary" ClientIDMode="Static" runat="server" TextMode="MultiLine" MaxLength="1024" Rows="4" CssClass="required form-control"></asp:TextBox>
        </div>
    </div>
</div>
<div class="comment-block" style="float: left; margin-left: 12px; margin-bottom: -20px; font-weight: bold; padding-top: 8px">
    &nbsp;(in thousands)
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12">
        <div class="form-group">
            <h3 class="accordion" style="text-align: center;">
                <asp:Label runat="server" ID="lblTitle" ClientIDMode="Static" CssClass="titlelbl" Text="Total Project Financials"></asp:Label>
            </h3>
        </div>
    </div>
</div>
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12">
        Please reference <a href='/Shared%20Documents/Finance%20Term%20Definition.xlsx'>Finance Term Definition</a> for more details.
    </div>
</div>
<asp:Panel ID="FinancialBriefSection" runat="server" CssClass="FinancialBriefSection FinancialSummaryrow">
    <div class="row" style="border-top-style: solid; border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-12">
                <div class="form-group">
                    <label class="control-label"><span class="markrequired">*</span>Name:</label>
                    <asp:TextBox ID="txtBriefSummaryName" Text="Consolidated Financial Summary" ClientIDMode="Static" CssClass="form-control stageGateReadOnly StageGateFinaceInput" runat="server" Font-Bold="true"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <label class="control-label">Target Margin:</label>
                <asp:TextBox ID="txtAvgTargetMarginPct" CssClass="form-control SatgeGaePct NumberWithComma" Style="text-align: right" runat="server" ClientIDMode="Static"> </asp:TextBox>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-5">
                <label class="control-label"><span class="markrequired">*</span>Display Consolidated Financials in Project Brief :</label>
                <asp:DropDownList ID="ddlDisplayConsolidatedFinancialsInProjectBrief" CssClass="required StageGateFinaceInput" runat="server" ClientIDMode="Static">
                    <asp:ListItem Selected="True" Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: solid; text-align: center; width: 30%; margin-left: 15px;">
                <asp:Label ID="Label13" runat="server" ClientIDMode="Static" Text="Year 1" Font-Bold="true" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: solid; text-align: center; width: 30%; margin-left: 30px;">
                <asp:Label ID="Label37" runat="server" ClientIDMode="Static" Text="Year 2" Font-Bold="true" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: solid; solid; text-align: center; width: 30%; margin-left: 30px;">
                <asp:Label ID="Label38" runat="server" ClientIDMode="Static" Text="Year 3" Font-Bold="true" />
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2" style="text-align: center">
                <asp:Label ID="lblTotal" runat="server" Text="Total" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2" style="text-align: center">
                <asp:Label ID="Label39" runat="server" Text="Incremental" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2" style="text-align: center">
                <asp:Label ID="Label43" runat="server" Text="Total" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2" style="text-align: center">
                <asp:Label ID="Label44" runat="server" Text="Incremental" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2" style="text-align: center">
                <asp:Label ID="Label45" runat="server" Text="Total" Font-Bold="true"></asp:Label>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2" style="text-align: center">
                <asp:Label ID="Label46" runat="server" Text="Incremental" Font-Bold="true"></asp:Label>
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label40" runat="server" ClientIDMode="Static" Text="Volume (lbs.)" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalVolume1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput  NumberWithComma volume" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalVolume1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma volume" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalVolume2" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma volume" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalVolume2" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma volume" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalVolume3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma volume" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalVolume3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma volume" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label35" runat="server" ClientIDMode="Static" Text="Gross Sales" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossSales1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossSales1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossSales2" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossSales2" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossSales3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossSales3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label42" runat="server" ClientIDMode="Static" Text="Net Sales" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalNetSales1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalNetSales1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalNetSales2" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalNetSales2" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" Style="text-align: right" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalNetSales3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalNetSales3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <div class="row" style="border-top: solid; border-top-width: thin; border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label47" runat="server" ClientIDMode="Static" Text="COGS" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalCOGS1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalCOGS1" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalCOGS2" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalCOGS2" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalCOGS3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalCOGS3" Style="text-align: right" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label48" runat="server" ClientIDMode="Static" Text="Gross Margin" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossMargin1" Style="text-align: right" CssClass="form-control  stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossMargin1" Style="text-align: right" CssClass="form-control  stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossMargin2" Style="text-align: right" CssClass="form-control  stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossMargin2" Style="text-align: right" CssClass="form-control  stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossMargin3" Style="text-align: right" CssClass="form-control  stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossMargin3" Style="text-align: right" CssClass="form-control  stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <div class="row" style="border-bottom: double; border-top: solid; border-top-width: thin; border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label49" runat="server" ClientIDMode="Static" Text="Gross Margin %" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossMarginPct1" Style="text-align: right" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossMarginPct1" Style="text-align: right" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossMarginPct2" Style="text-align: right" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossMarginPct2" Style="text-align: right" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtTotalGrossMarginPct3" Style="text-align: right" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtIncrementalGrossMarginPct3" Style="text-align: right" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label50" runat="server" ClientIDMode="Static" Text="NS$/LB" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtNSPerLB1" Style="text-align: center" runat="server" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtNSPerLB2" Style="text-align: center" runat="server" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtNSPerLB3" Style="text-align: center" runat="server" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
            </div>
        </div>
    </div>
    <div class="row" style="border-left-style: solid; border-right-style: solid; border-bottom-style: solid;">
        <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
            <asp:Label ID="Label51" runat="server" ClientIDMode="Static" Text="COGS/LB" Font-Bold="true" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-11">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtCOGSPerLB1" Style="text-align: center" runat="server" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtCOGSPerLB2" Style="text-align: center" runat="server" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <asp:TextBox ID="txtCOGSPerLB3" Style="text-align: center" runat="server" CssClass="form-control stageGateReadOnly StageGateFinaceInput NumberWithComma" ClientIDMode="Static" />
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
            </div>
        </div>
    </div>
</asp:Panel>
<div class="row">
    <div class="comment-block">
        <asp:Label ID="Label1" ClientIDMode="Static" runat="server" Text="Analyses included in Consolidated Financial Summary: " Font-Bold="true"></asp:Label>
        <asp:Label ID="lblAnalysesIncludedInSummary" ClientIDMode="Static" runat="server"></asp:Label>
        <asp:HiddenField ID="hdnAnalysesIncludedInSummary" runat="server" ClientIDMode="Static" />
    </div>
</div>
<asp:HiddenField ID="hdnSummaryListItemId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnStageGateProjectListItemId" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnGate" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hdnBriefNumber" runat="server" ClientIDMode="Static" />
<br />
<br />
<div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">
        &nbsp;
    </div>
</div>
