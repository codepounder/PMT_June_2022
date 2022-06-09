<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucFinancialBrief.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucFinancialBrief" %>

<asp:Panel ID="FinancialBriefSection" CssClass="FinancialBriefSection" runat="server">
    <table class="container-fluid ucTSTable">
        <asp:Repeater ID="rptFinancialBriefItem" runat="server" OnItemDataBound="rptFinancialBriefItem_ItemDataBound" OnItemCommand="rptFinancialBriefItem_ItemCommand">
            <ItemTemplate>
                <asp:Panel runat="server" CssClass="FinancialBriefrow" ID="pnlFinancialBriefrow">
                    <asp:HiddenField ID="hdnAnalysisListItemId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnStageGateProjectListItemId" Value='<%# DataBinder.Eval(Container.DataItem, "StageGateProjectListItemId") %>' runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnGate" Value='<%# DataBinder.Eval(Container.DataItem, "Gate") %>' runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnBriefNumber" Value='<%# DataBinder.Eval(Container.DataItem, "BriefNumber") %>' runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnDeletedStatus" runat="server" ClientIDMode="Static" />
                    <div class="row">
                        <br />
                        <br />
                    </div>
                    <div class="row FinancialBriefAnalysisHeader" style="background-color: #eee; color: #084c61; cursor: pointer; border: none;">
                        <%-- <div class="comment-block col-xs-12 col-sm-6 col-md-12 FinancialBriefAnalysisHeader" style="font-weight: bold; margin-top: -30px;">
                            &nbsp;(in thousands)
                        </div>--%>
                        <div class="col-xs-12 col-sm-6 col-md-1 FinancialBriefAnalysisHeader">
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-11 FinancialBriefAnalysisHeader">
                            <div class="col-xs-12 col-sm-6 col-md-8 FinancialBriefAnalysisHeader">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Name</label>
                                    <asp:TextBox ID="txtAnalysisName" ClientIDMode="Static" CssClass="required titlelbl AnalysisName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "AnalysisName") %>'></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4 FinancialBriefAnalysisHeader">
                                <div class="col-xs-12 col-sm-6 col-md-4 FinancialBriefAnalysisHeader" style="margin-left: -20px;">
                                    <div class="form-group">
                                        <label class="control-label">FG#</label>
                                        <asp:TextBox ID="txtFGNumber" ClientIDMode="Static" Style="width: 80px;" CssClass="required alphanumericToUpper1 form-control minimumlength titlelbl AnalysisName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "FGNumber") %>'></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2 FinancialBriefAnalysisHeader" style="margin-left: -25px; margin-right: 45px;">
                                    <div class="form-group">
                                        <label class="control-label" style="visibility: hidden;"><span class="markrequired">*</span></label>
                                        <asp:Button ID="btnSearchByFGNumber" CommandName="SearchByFGNumber" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "StageGateProjectListItemId") %>' CssClass="ButtonControlAutoSize" Text="Search" runat="server" CausesValidation="false" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-6 FinancialBriefAnalysisHeader">
                                    <div class="form-group">
                                        <label class="control-label" style="visibility: hidden;"><span class="markrequired">*</span></label>
                                        <asp:Button ID="btnDeleteFinancialBriefAnalysis" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' OnClientClick="deleteFinancialBriefAnalysisItem(this);return false;" CssClass="ButtonControlAutoSize" Text="Delete Analysis" runat="server" CausesValidation="false" />
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="FinancialBriefAnalysisBody">
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1">
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Customer/Channel</label>
                                        <asp:DropDownList ID="ddlCustomerChannel" runat="server" ClientIDMode="Static" CssClass="required">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                            <%-- <asp:ListItem Text="Walmart" Value="Walmart"></asp:ListItem>
                                            <asp:ListItem Text="Grocery/Conv" Value="Grocery/Conv"></asp:ListItem>
                                            <asp:ListItem Text="Conv" Value="Conv"></asp:ListItem>
                                            <asp:ListItem Text="Drug & Mass" Value="Drug & Mass"></asp:ListItem>
                                            <asp:ListItem Text="Dollar & Vend" Value="Dollar & Vend"></asp:ListItem>
                                            <asp:ListItem Text="Club" Value="Club"></asp:ListItem>
                                            <asp:ListItem Text="E-Comm/ B2B" Value="E-Comm/ B2B"></asp:ListItem>
                                            <asp:ListItem Text="B to B" Value="B to B"></asp:ListItem>
                                            <asp:ListItem Text="National Price List" Value="National Price List"></asp:ListItem>--%>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Brand/Season</label>
                                        <asp:DropDownList ID="ddlBrandSeason" runat="server" ClientIDMode="Static" CssClass="required">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Product Form</label>
                                        <asp:DropDownList ID="ddlProductForm" runat="server" ClientIDMode="Static" CssClass="required">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1">
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Target Margin %</label>
                                        <asp:TextBox ID="txtTargetMarginPct" Style="text-align: right" CssClass="required NumberWithComma SatgeGaePct CalculateFinancials" Text='<%# DataBinder.Eval(Container.DataItem, "TargetMarginPct") %>' runat="server" ClientIDMode="Static"> </asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Display P&L in Project Brief</label>
                                        <asp:DropDownList ID="ddlDisplayPLInProjectBrief" runat="server" ClientIDMode="Static" CssClass="required">
                                            <asp:ListItem Selected="True" Text="Yes" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label"><span class="markrequired">*</span>Include P&L in Consolidated Financials</label>
                                        <asp:DropDownList ID="ddlIncludePLInConsolidatedFinancials" CssClass="required IncludePLInConsolidatedFinancials" runat="server" ClientIDMode="Static">
                                            <asp:ListItem Selected="True" Text="Yes" Value="Yes"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right; font-weight: bold; color: #084c61;">
                                <label class="control-label"><span class="markrequired">*</span>(in thousands)</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: solid; text-align: center; width: 30%; margin-left: 15px;">
                                    <asp:Label ID="Label13" runat="server" ClientIDMode="Static" Text="Year 1" Font-Bold="true" />
                                    <i class="fa fa-arrow-circle-right CopyNumbers1" style="float: right; cursor: pointer !important; margin-right: -10px; margin-top: 2px; color: #084C61"></i>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: solid; text-align: center; width: 30%; margin-left: 35px;">
                                    <asp:Label ID="Label37" runat="server" ClientIDMode="Static" Text="Year 2" Font-Bold="true" />
                                    <i class="fa fa-arrow-circle-right CopyNumbers2" style="float: right; cursor: pointer !important; margin-right: -10px; margin-top: 2px; color: #084C61"></i>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4" style="border-style: solid; text-align: center; width: 30%; margin-left: 35px;">
                                    <asp:Label ID="Label38" runat="server" ClientIDMode="Static" Text="Year 3" Font-Bold="true" />
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
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
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label"><span class="markrequired">*</span>Volume (lbs.)</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Volume for Year 1</label>
                                        <asp:TextBox ID="txtTotalVolume1" CssClass="required NumberWithComma CalculateFinancials volume" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "VolumeTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Volume for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalVolume1" CssClass="required NumberWithComma CalculateFinancials volume" runat="server" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "VolumeIncremental1") %>' ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Volume for Year 2</label>
                                        <asp:TextBox ID="txtTotalVolume2" CssClass="required NumberWithComma CalculateFinancials volume" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "VolumeTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Volume for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalVolume2" CssClass="required NumberWithComma CalculateFinancials volume" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "VolumeIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Volume for Year 3</label>
                                        <asp:TextBox ID="txtTotalVolume3" CssClass="required NumberWithComma CalculateFinancials volume" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "VolumeTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Volume for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalVolume3" CssClass="required NumberWithComma CalculateFinancials volume" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "VolumeIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label"><span class="markrequired">*</span>Gross Sales</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Sales for Year 1</label>
                                        <asp:TextBox ID="txtTotalGrossSales1" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossSalesTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Sales for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalGrossSales1" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossSalesIncremental1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Sales for Year 2</label>
                                        <asp:TextBox ID="txtTotalGrossSales2" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossSalesTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Sales for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalGrossSales2" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossSalesIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Sales for Year 3</label>
                                        <asp:TextBox ID="txtTotalGrossSales3" CssClass="required  NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossSalesTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Sales for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalGrossSales3" CssClass="required  NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossSalesIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label"><span class="markrequired">*</span>Trade Rate</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Trade Rate for Year 1</label>
                                        <asp:TextBox ID="txtTotalTradeRate1" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TradeRateTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Trade Rate for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalTradeRate1" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TradeRateIncremental1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Trade Rate for Year 2</label>
                                        <asp:TextBox ID="txtTotalTradeRate2" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TradeRateTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Trade Rate for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalTradeRate2" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TradeRateIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Trade Rate for Year 3</label>
                                        <asp:TextBox ID="txtTotalTradeRate3" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TradeRateTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Trade Rate for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalTradeRate3" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TradeRateIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label" style="margin-top: 10px"><span class="markrequired">*</span>OGTN</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total OGTN for Year 1</label>
                                        <asp:TextBox ID="txtTotalOGTN1" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "OGTNTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental OGTN for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalOGTN1" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "OGTNIncremental1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total OGTN for Year 2</label>
                                        <asp:TextBox ID="txtTotalOGTN2" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "OGTNTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental OGTN for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalOGTN2" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "OGTNIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total OGTN for Year 3</label>
                                        <asp:TextBox ID="txtTotalOGTN3" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "OGTNTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental OGTN for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalOGTN3" CssClass="required SatgeGaePct NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "OGTNIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label"><span class="markrequired"></span>Net Sales</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Net Sales for Year 1</label>
                                        <asp:TextBox ID="txtTotalNetSales1" CssClass="form-control required stageGateReadOnly  NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "NetSalesTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Net Sales for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalNetSales1" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "NetSalesIncremental1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Net Sales for Year 2</label>
                                        <asp:TextBox ID="txtTotalNetSales2" CssClass="form-control required stageGateReadOnly  NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "NetSalesTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Net Sales for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalNetSales2" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "NetSalesIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Net Sales for Year 3</label>
                                        <asp:TextBox ID="txtTotalNetSales3" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "NetSalesTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Net Sales for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalNetSales3" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "NetSalesIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "style=\"border-top: solid; border-top-width: thin;\"" : "style=\"background-color:#BCD3F2;border-top: solid; border-top-width: thin;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label" style="margin-top: 10px"><span class="markrequired">*</span>COGS</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total COGS for Year 1</label>
                                        <asp:TextBox ID="txtTotalCOGS1" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "COGSTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental COGS for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalCOGS1" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "COGSIncremental1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total COGS for Year 2</label>
                                        <asp:TextBox ID="txtTotalCOGS2" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "COGSTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental COGS for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalCOGS2" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "COGSIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total COGS for Year 3</label>
                                        <asp:TextBox ID="txtTotalCOGS3" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "COGSTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental COGS for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalCOGS3" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "COGSIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label"><span class="markrequired"></span>Gross Margin</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Margin for Year 1</label>
                                        <asp:TextBox ID="txtTotalGrossMargin1" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Margin for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalGrossMargin1" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginIncremental1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Margin for Year 2</label>
                                        <asp:TextBox ID="txtTotalGrossMargin2" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Margin for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalGrossMargin2" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Margin for Year 3</label>
                                        <asp:TextBox ID="txtTotalGrossMargin3" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Margin for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalGrossMargin3" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "style=\"border-bottom: double; border-top: solid; border-top-width: thin;\"" : "style=\"background-color:#BCD3F2; border-bottom: double; border-top: solid; border-top-width: thin;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label"><span class="markrequired">*</span>Gross Margin %</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Margin % for Year 1</label>
                                        <asp:TextBox ID="txtTotalGrossMarginPct1" CssClass="form-control SatgeGaePct required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginPctTotal1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Margin % for Year 1</label>
                                        <asp:TextBox ID="txtIncrementalGrossMarginPct1" CssClass="form-control SatgeGaePct required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginPctIncremental1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Margin % for Year 2</label>
                                        <asp:TextBox ID="txtTotalGrossMarginPct2" CssClass="form-control SatgeGaePct required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginPctTotal2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Margin % for Year 2</label>
                                        <asp:TextBox ID="txtIncrementalGrossMarginPct2" CssClass="form-control SatgeGaePct required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginPctIncremental2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Total Gross Margin % for Year 3</label>
                                        <asp:TextBox ID="txtTotalGrossMarginPct3" CssClass="form-control SatgeGaePct required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginPctTotal3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>Incremental Gross Margin for Year 3</label>
                                        <asp:TextBox ID="txtIncrementalGrossMarginPct3" CssClass="form-control SatgeGaePct required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "GrossMarginPctIncremental3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label" style="margin-top: 10px"><span class="markrequired"></span>NS$/LB</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtNSPerLB1" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: center" Text='<%# DataBinder.Eval(Container.DataItem, "NSDollerperLB1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtNSPerLB2" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: center" Text='<%# DataBinder.Eval(Container.DataItem, "NSDollerperLB2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtNSPerLB3" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: center" Text='<%# DataBinder.Eval(Container.DataItem, "NSDollerperLB3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label" style="margin-top: 10px"><span class="markrequired"></span>COGS/LB</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtCOGSPerLBValue1" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: center" Text='<%# DataBinder.Eval(Container.DataItem, "COGSperLB1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtCOGSPerLBValue2" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: center" Text='<%# DataBinder.Eval(Container.DataItem, "COGSperLB2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtCOGSPerLBValue3" CssClass="form-control required stageGateReadOnly NumberWithComma CalculateFinancials" Style="text-align: center" Text='<%# DataBinder.Eval(Container.DataItem, "COGSperLB3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-2">
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="text-align: right;">
                                <label class="control-label"><span class="markrequired">*</span>TruckLoad Price per Retail Selling Unit</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>TruckLoad Price per Retail Selling Unit for Year 1</label>
                                        <asp:TextBox ID="txtTruckLoadPrice1" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TruckldPricePrRtlSllngUt1") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>TruckLoad Price per Retail Selling Unit for Year 2</label>
                                        <asp:TextBox ID="txtTruckLoadPrice2" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TruckldPricePrRtlSllngUt2") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired">*</span>TruckLoad Price per Retail Selling Unit for Year 3</label>
                                        <asp:TextBox ID="txtTruckLoadPrice3" CssClass="required NumberWithComma CalculateFinancials" Style="text-align: right" Text='<%# DataBinder.Eval(Container.DataItem, "TruckldPricePrRtlSllngUt3") %>' runat="server" ClientIDMode="Static" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-1" style="">
                                <label class="control-label" style="margin-top: 30px">Assumptions</label>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-11">
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired"></span>"Assumptions for Year 1</label>
                                        <asp:TextBox ID="txtAssumptions1" CssClass="required form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Assumptions1") %>' runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="1024" Rows="3" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired"></span>"Assumptions for Year 2</label>
                                        <asp:TextBox ID="txtAssumptions2" CssClass="required form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Assumptions2") %>' runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="1024" Rows="3" />
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label" style="display: none"><span class="markrequired"></span>"Assumptions for Year 3</label>
                                        <asp:TextBox ID="txtAssumptions3" CssClass="required form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Assumptions3") %>' runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="1024" Rows="3" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-2">
            <asp:Button ID="btnAddNewFinancialBriefAnalysis" OnClick="btnAddNewFinancialBriefAnalysis_Click" CssClass="ButtonControlAutoSize" Text="Add New Analysis" runat="server" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-10">
        </div>
    </div>
</asp:Panel>
