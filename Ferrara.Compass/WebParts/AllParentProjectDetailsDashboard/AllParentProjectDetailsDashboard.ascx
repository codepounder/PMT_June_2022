<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllParentProjectDetailsDashboard.ascx.cs" Inherits="Ferrara.Compass.WebParts.AllParentProjectDetailsDashboard.AllParentProjectDetailsDashboard" %>


<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jszip.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/buttons.html5.min.js"></script>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=115"></script>

<link href="/_layouts/15/Ferrara.Compass/css/jquery.dataTables.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css?v=4" type="text/css" rel="Stylesheet" />

<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-grid.min.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-table.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />

<div style="min-width: 1000px; border: 0px solid black; padding: 3px; margin: 0 auto;" id="allParentProjectMainDiv">
    <asp:Panel ID="dashboardError" runat="server" CssClass="dashboardError"></asp:Panel>
    <h2>All Parent Project Details</h2>
    <asp:DropDownList ID="ProjectStatusFilter" AutoPostBack="true" runat="server">
        <asp:ListItem Text="All Open Parent Projects" Value="All Open"></asp:ListItem>
        <asp:ListItem Text="All Completed Parent Projects" Value="Completed"></asp:ListItem>
        <asp:ListItem Text="All Cancelled Parent Projects" Value="Cancelled"></asp:ListItem>
        <asp:ListItem Text="All On Hold Parent Projects" Value="OnHold"></asp:ListItem>
        <asp:ListItem Text="All Parent Projects" Value="All Projects"></asp:ListItem>
    </asp:DropDownList>
    <table id='AllParentOpenProjects' class='display'>
        <thead>
            <tr>
                <th>Project Number</th>
                <th>Project Name</th>
                <th>Gate 0 Approved Date</th>
                <th>Desired 1st Ship Date</th>
                <th>Revised 1st Ship Date</th>
                <th>Current Project Stage</th>
                <th>Brand</th>
                <th>#SKUs</th>
                <th>Project Type</th>
                <th>Project Type SubCategory</th>
                <th>Project Manager</th>
                <th>Project Leader</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Project Number</th>
                <th>Project Name</th>
                <th>Gate 0 Approved Date</th>
                <th>Desired 1st Ship Date</th>
                <th>Revised 1st Ship Date</th>
                <th>Current Project Stage</th>
                <th>Brand</th>
                <th>#SKUs</th>
                <th>Project Type</th>
                <th>Project Type SubCategory</th>
                <th>Project Manager</th>
                <th>Project Leader</th>
            </tr>
        </tfoot>
    </table>
    <asp:HiddenField ID="hidProjectGroup" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hidProjectInitiator" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hidTableDisplayType" runat="server" Value="false" />
</div>
<asp:Literal ID="litScript" runat="server"></asp:Literal>
<script type="text/javascript">
    $(document).ready(function () {
        AllParentProjectDisplay();
    });

</script>


