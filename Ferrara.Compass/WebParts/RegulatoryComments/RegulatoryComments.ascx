<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegulatoryComments.ascx.cs" Inherits="Ferrara.Compass.WebParts.RegulatoryComments.RegulatoryComments" %>

<link rel="stylesheet" href="/_layouts/15/Ferrara.Compass/css/datepicker.min.css" />
<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" rel="Stylesheet" />
<link href="../../_layouts/15/Ferrara.Compass/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<%--<link rel="stylesheet" href="/resources/demos/style.css">--%>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Bootstrap/bootstrap.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js"></script>

<div class="container" id="dvcontainer">
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Regulatory Comments</h1>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>

    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <span class="markrequired">*</span>
                <label clientidmode="Static" id="lblChangeNotes" cssclass="control-label" runat="server">Regulatory Comments:</label>
                <asp:TextBox ID="txtRegulatoryComments" runat="server" ClientIDMode="Static" TextMode="MultiLine" MaxLength="255" Rows="6" CssClass="required form-control"></asp:TextBox>
            </div>
        </div>
    </div>

    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
         <div class="col-xs-12 col-sm-9 col-md-12">
            <asp:Button ID="btnSave" CssClass="button justifyRight" runat="server" Text="Save" OnClick="btnSave_Click"></asp:Button>
        </div>
    </div>
</div>
