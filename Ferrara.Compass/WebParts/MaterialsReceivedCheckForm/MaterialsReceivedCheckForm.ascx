<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MaterialsReceivedCheckForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.MaterialsReceivedCheckForm.MaterialsReceivedCheckForm" %>

<div class="container" id="dvMain" runat="server">
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
            <h1>Materials Received Check</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_message" style="display:none;"> <ul id="error_message"></ul></div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Tracking Details</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <asp:Repeater ID="rpPackagingItems" runat="server" OnItemDataBound="rpPackagingItems_ItemDataBound">
            <HeaderTemplate>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-2"><label class="control-label">New/Existing</label></div>
                    <div class="col-xs-12 col-sm-6 col-md-1"><label class="control-label">Component#</label></div>
                    <div class="col-xs-12 col-sm-6 col-md-4"><label class="control-label">Component Description</label></div>
                    <div class="col-xs-12 col-sm-6 col-md-1"><label class="control-label">Plant</label></div>
                    <div class="col-xs-12 col-sm-6 col-md-1"><label class="control-label">Current Available Quantity</label></div>
                    <div class="col-xs-12 col-sm-6 col-md-2"><label class="control-label">Date of Order</label></div>
                    <div class="col-xs-12 col-sm-6 col-md-1"><label class="control-label">Quantity of Order</label></div>
                </div>
            </HeaderTemplate>
            <ItemTemplate>
                <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div class="col-xs-12 col-sm-6 col-md-2">
                        <asp:TextBox ID="txtNewExisting" BorderStyle="None" ReadOnly="true" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "NewExisting") %>'></asp:TextBox>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-1">
                        <asp:TextBox ID="txtMaterialNumber" BorderStyle="None" ReadOnly="true" runat="server" class="form-control" MaxLength="6" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>'></asp:TextBox>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <asp:TextBox ID="txtMaterialDesc" BorderStyle="None" ReadOnly="true" runat="server" class="form-control" MaxLength="40" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>'></asp:TextBox>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-1">
                        <asp:TextBox ID="txtPlant" BorderStyle="None" ReadOnly="true" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Plant") %>'></asp:TextBox>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-1">  
                        <asp:TextBox ID="txtCurrentAvaialableQuantity" BorderStyle="None" ReadOnly="true" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentAvailQuantity") %>'></asp:TextBox>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-2">
                        <asp:TextBox ID="txtDateofOrder" BorderStyle="None" ReadOnly="true" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "DateOfOrder") %>'></asp:TextBox>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-1">
                        <asp:TextBox ID="QuantityofOrder" BorderStyle="None" ReadOnly="true" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "QuantityOfOrder") %>'></asp:TextBox>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
             <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
             <asp:HiddenField ID="hdnProjectType" runat="server" />
        </div>
    </div>
</div>