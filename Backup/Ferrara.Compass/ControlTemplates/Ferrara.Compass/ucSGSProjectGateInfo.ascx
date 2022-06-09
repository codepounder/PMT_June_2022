<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSGSProjectGateInfo.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucSGSProjectGateInfo" %>
<asp:Panel ID="summaries" CssClass="SGSGateProjectInfo FinanceBriefs" runat="server">
    <h3 class="accordion">Brief G<asp:Label ID="lblGateText" runat="server" Text=""></asp:Label>
        #
        <asp:Label ID="lblBriefNo" runat="server" Text=""></asp:Label>:
        <asp:Label ID="lblBriefName" runat="server" Text=""></asp:Label></h3>
    <div class="FinanceBrief">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Brief Name:</label>
                    <asp:TextBox ID="txtBriefName" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <h2>High Level Project Summary</h2>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Product/SKU/Pack Formats:</label>
                    <asp:TextBox ID="txtProductFormats" CssClass="form-control RTBox" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="6"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Retail Execution:</label>
                    <div class="comment-block">(Example of attribute)</div>
                    <asp:TextBox ID="txtRetailExecution" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Other Key Info:</label>
                    <div class="comment-block">(Such as claims if new/different, etc.) from project scope</div>
                    <asp:TextBox ID="txtOtherKeyInfo" CssClass="form-control RTBox" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                </div>
            </div>
        </div>
        <h2>Project Health/Current Situation</h2>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-4">
                <div class="form-group">
                    <label class="control-label">Overall Risk:</label>
                    <div class="comment-block">Low / Medium / High</div>
                    <asp:DropDownList ID="ddlOverallRisk" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-8">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                    <div class="comment-block">Reason (Distinguish between Technical, Commercial, Competitive)</div>
                    <asp:TextBox ID="txtRiskReason" CssClass="form-control" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="1"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">
                        Gate
                        <asp:Label runat="server" ID="lblPHGate"></asp:Label>
                        Readiness:</label>
                    <div class="comment-block">Any missing? Explain why?</div>
                    <asp:TextBox ID="txtGateReadiness" CssClass="form-control" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="1"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-4">
                <div class="form-group">
                    <label class="control-label">Overall Status (issues):</label>
                    <div class="comment-block">Green / Yellow / Red</div>
                    <asp:DropDownList ID="ddlOverallStatus" CssClass="form-control listColor" runat="server" onchange="setDropdownColor();">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-8">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                    <div class="comment-block">Reason</div>
                    <asp:TextBox ID="txtStatusReason" CssClass="form-control" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="1"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Major Upcoming Milestones:</label>
                    <div class="comment-block">List out 3-4 major upcoming activities</div>
                    <asp:TextBox ID="txtMilestones" CssClass="form-control RTBox" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                </div>
            </div>
        </div>
        <h2>Impacts of Project Health/Current Situation</h2>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Impacts of Project Health/Current Situation:</label>
                    <div class="comment-block">
                        <ul>
                            <li>What will the team need to compress/parallel path to execute given risks, issues, compression?</li>
                            <li>What support needs will they have because of this?</li>
                        </ul>
                    </div>
                    <asp:TextBox ID="txtImpactProjectHealth" CssClass="form-control RTBox" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                </div>
            </div>
        </div>
        <h2>Team's Recommendation</h2>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Team's Recommendation:</label>
                    <ul class="comment-block">
                        <li class="comment-block">Agree to launch with stated risks</li>
                        <li class="comment-block">Approve to support X, Y, Z</li>
                        <li class="comment-block">Approve gate request</li>
                    </ul>
                    <asp:TextBox ID="txtTeamRecommendation" CssClass="form-control RTBox" runat="server" TextMode="MultiLine" Rows="5"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-2">
                <div class="form-group">
                    <asp:Button ID="btnCopyBrief" runat="server" Text="Copy Brief" CssClass="ButtonControlAutoSize" OnClick="btnCopyBrief_Click" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 col-md-8">
                <asp:Button ID="btnGenerateBrief" runat="server" Text="Generate Brief" CssClass="ButtonControlAutoSize noIcon" OnClick="btnGenerateBrief_Click" />
            </div>
            <div class="col-xs-12 col-sm-12 col-md-2">
                <div class="form-group">
                    <asp:Button ID="btnDeleteBrief" runat="server" Text="Delete Brief" CssClass="ButtonControlAutoSize" OnClientClick="return deleteBrief(this);" />&nbsp;
                <asp:HiddenField ID="hdnProjectBriefID" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnDeleted" ClientIDMode="Static" runat="server" Value="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Panel>
<script type="text/javascript">
        runTinyMC();
    </script>
