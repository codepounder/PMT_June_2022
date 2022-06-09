<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StageGateFinancialSummaryForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.StageGateFinanceSummaryForm.StageGateFinancialSummaryForm" %>

<div class="container">
    <div id="divAccessDenied" runat="server" class="AccessDenied">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                WARNING: You do not have access to update this page. Save functionality will be disabled!
                <br />
                If you require access, please email the
                <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False">HelpDesk</asp:LinkButton>.<br />
            </div>
        </div>
    </div>
    <div id="divAccessRequest" runat="server" class="AccessRequest">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                Your request for access has been sent!
            </div>
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
    <asp:Panel runat="server" ID="SGSProjectInformation"></asp:Panel>
    <div class="row">

        <div class="col-xs-12 col-sm-12 col-md-6">
            <asp:PlaceHolder ID="phSGSProjectInformation" runat="server" />
        </div>
    </div>
    <div class="RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="form-group">
        <asp:Repeater ID="rptFinanceBriefs" runat="server" OnItemCommand="rptFinanceBriefs_ItemCommand" OnItemDataBound="rptFinanceBriefs_ItemDataBound">
            <ItemTemplate>
                <div class="FinanceBriefs">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="form-group">
                            <h3 class="accordion Summaryaccordion">
                                <asp:HiddenField ID="hdnGate" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "Gate") %>' />
                                <asp:HiddenField ID="hdnBriefNo" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "BriefNo") %>' />
                                <asp:Label ID="lblBriefName" ClientIDMode="Static" runat="server"></asp:Label>
                            </h3>
                        </div>
                    </div>
                    <div class="FinanceBrief">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <asp:Button ID="btnGenerateFinancePDF" Text="Generate Finance PDF" CommandName="GenerateFinancePDF" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BriefNo") %>' runat="server" CssClass="ButtonControlAutoSize " Style="margin-top: -7px; float: right;" CausesValidation="false" />
                                    <asp:HyperLink Target="_blank" ID="btnFinanceBrief" Text="Finance Brief" CommandName="GoToFinanceBriefPage" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "BriefNo") %>' runat="server" CssClass="ButtonControlAutoSize btn noIcon" Style="margin-top: -7px; width: 124px; height: 35px" CausesValidation="false" />
                                </div>
                            </div>
                            <div class="form-group col-xs-12 col-sm-12 col-md-8">
                                <div class="form-group">
                                    <a target="_blank" id="ancFinancePDF" runat="server" visible="false"></a>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <div class="form-group">
                                        <h2>High Level Project Summary</h2>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label">
                                        • Product/SKU/Pack Formats:
                                    </label>
                                    <textarea id="txtProductFormats" enabled="false" class="form-control RTBox" style="border-radius: 25px;" clientidmode="Static" runat="server" textmode="MultiLine" rows="6"></textarea>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label">
                                        • Retail Execution:
                                <div class="comment-block" style="float: right;">&nbsp;(Example of attribute)</div>
                                    </label>
                                    <asp:TextBox ID="txtRetailExecution" CssClass="form-control" Style="border-radius: 10px;" ClientIDMode="Static" runat="server" ReadOnly="True" Text='<%# DataBinder.Eval(Container.DataItem, "RetailExecution") %>'>></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div>
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label">
                                        • Other Key Info:
                                <div class="comment-block" style="float: right;">&nbsp;(Such as claims if new/different, etc.) from project scope</div>
                                    </label>
                                    <textarea id="txtOtherKeyInfo" class="form-control RTBox" style="border-radius: 25px;" clientidmode="Static" runat="server" textmode="MultiLine" rows="5"></textarea>
                                </div>
                            </div>
                        </div>
                        <div id="divGrossMargin" runat="server">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <div class="form-group">
                                    <label class="control-label">
                                        • Gross Margin %:
                                    </label>
                                </div>
                            </div>
                            <div>
                                <div class="col-xs-12 col-sm-6 col-md-12">
                                    <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: groove; text-align: center; width: 30%; margin-left: 20px;">
                                        <asp:Label ID="Label13" runat="server" ClientIDMode="Static" Text="Year 1" Font-Bold="true" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: groove; text-align: center; width: 30%; margin-left: 35px;">
                                        <asp:Label ID="Label37" runat="server" ClientIDMode="Static" Text="Year 2" Font-Bold="true" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: groove; solid; text-align: center; width: 30%; margin-left: 35px;">
                                        <asp:Label ID="Label38" runat="server" ClientIDMode="Static" Text="Year 3" Font-Bold="true" />
                                    </div>
                                </div>
                            </div>
                            <div>
                                <div class="col-xs-12 col-sm-6 col-md-12">
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
                            <div>
                                <div class="col-xs-12 col-sm-6 col-md-12">
                                    <div class="col-xs-12 col-sm-6 col-md-2">
                                        <asp:TextBox ID="txtTotalGrossMarginPct1" Style="text-align: center" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-2">
                                        <asp:TextBox ID="txtIncrementalGrossMarginPct1" Style="text-align: center" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-2">
                                        <asp:TextBox ID="txtTotalGrossMarginPct2" Style="text-align: center" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-2">
                                        <asp:TextBox ID="txtIncrementalGrossMarginPct2" Style="text-align: center" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-2">
                                        <asp:TextBox ID="txtTotalGrossMarginPct3" Style="text-align: center" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
                                    </div>
                                    <div class="col-xs-12 col-sm-6 col-md-2">
                                        <asp:TextBox ID="txtIncrementalGrossMarginPct3" Style="text-align: center" CssClass="form-control SatgeGaePct stageGateReadOnly StageGateFinaceInput NumberWithComma" runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hdnStageGateListItemId" runat="server" ClientIDMode="Static" />
        </div>
    </div>
</div>
<script type="text/javascript">
    runTinyMCDisabled();
</script>
