<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSGSProjectInformation.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucSGSProjectInformation" %>
<div class="row">    
    <div class="col-xs-12 col-sm-12 col-md-12">
        <h2>Project Information</h2>
    </div>
</div>
<div class="row RowBottomMargin">
    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
</div>
<div class="row"> 
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Gate 0 Approved Date: </label><asp:Label runat="server" ID="lblSubmittedDate"></asp:Label></div>
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Desired 1st Ship Date: </label><asp:Label runat="server" ID="lblFirstShipDate"></asp:Label></div>
</div>
<div class="row"> 
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Line of Business: </label><asp:Label runat="server" ID="lblLOB"></asp:Label></div>
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Revised 1st Ship Date: </label><asp:Label runat="server" ID="lblRevisedFirstShipDate"></asp:Label></div>
</div>
<div class="row"> 
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Brand: </label><asp:Label runat="server" ID="lblBrand"></asp:Label></div>
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Project Leader: </label><asp:Label runat="server" ID="lblProjectLeader"></asp:Label></div>
</div>
<div class="row"> 
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Project Type: </label><asp:Label runat="server" ID="lblProjectType"></asp:Label></div>
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Project Manager: </label><asp:Label runat="server" ID="lblProjectManager"></asp:Label></div>
</div>
<div class="row"> 
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label"># SKUs: </label><asp:Label runat="server" ID="lblSkus"></asp:Label></div>
    <div class="col-xs-12 col-sm-12 col-md-6"><label class="control-label">Project Tier/Priority: </label><asp:Label runat="server" ID="lblProjectTier"></asp:Label></div>
</div>
<div class="row"> 
    <div class="col-xs-12 col-sm-12 col-md-12">
        <label class="control-label">Project Concept Overview:</label><br />
        <asp:TextBox TextMode="MultiLine" ReadOnly="true" CssClass="control-label form-control" runat="server" ID="lblConceptOverview" Rows="10"></asp:TextBox><br />
    </div>
</div>
