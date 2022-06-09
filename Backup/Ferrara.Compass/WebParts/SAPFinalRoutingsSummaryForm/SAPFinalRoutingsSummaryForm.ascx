<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SAPFinalRoutingsSummaryForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.SAPFinalRoutingsSummaryForm.SAPFinalRoutingsSummaryForm" %>

<div class="container">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled! <br />If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False" onclick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1><asp:Panel runat="server" ID="headerTitle"></asp:Panel></h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
            <div id="dverror_message" style="display:none;"> <ul id="error_message"></ul></div>
        </div>
    </div>
    <!--<div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>New Components</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>-->
    <div id="dvComponentRepeater" class="row repeater" runat="server">
        <asp:Label ID="lblNoComponents" runat="server" Text="No New Components Found!" Visible="false" CssClass="control-label"></asp:Label>
        <asp:Repeater ID="rptNewComponents" runat="server" OnItemDataBound="rptNewComponents_ItemDataBound" OnItemCommand="rptNewComponents_ItemCommand">
        <ItemTemplate>
            <div class="row" style="margin-top:0px; <%# Container.ItemIndex % 2 == 0 ? "" : "background-color:#BCD3F2;" %>">
                <div class="col-xs-12 col-sm-6 col-md-1">
                    <div class="form-group">
                        <label class="control-label"><asp:label id="lblStatus" runat="server" Visible="false"></asp:label>Status:</label>
                        <asp:panel id="divFinalRoutingsStatus" runat="server"></asp:panel>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-2">
                    <div class="form-group">
                        <label class="control-label">Finished Good #:</label>
                        <asp:Textbox ID="lblMaterialNumber" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' ></asp:Textbox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Finished Good Description:</label>
                        <asp:Textbox ID="lblMaterialDesc" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' ></asp:Textbox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <div class="form-group">
                        <label class="control-label">Packing Location:</label>
                        <asp:Textbox ID="lblPackingLocation" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "PackLocation") %>' ></asp:Textbox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-2">
                    <div class="form-group">
                        <label class="control-label"><asp:label id="lblDate" Visible="false" runat="server"></asp:label>Submitted Date:</label>
                        <asp:Textbox ID="lblSubmittedDate" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='' ></asp:Textbox>
                    </div>
                </div>
            </div>
        </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-3 col-md-11">
            &nbsp;
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
        </div>
    </div>
</div>