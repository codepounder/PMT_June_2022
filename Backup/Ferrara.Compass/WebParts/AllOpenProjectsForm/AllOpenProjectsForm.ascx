<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AllOpenProjectsForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.AllOpenProjectsForm.AllOpenProjectsForm" %>


<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jszip.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/buttons.html5.min.js"></script>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=115"></script>

<link href="/_layouts/15/Ferrara.Compass/css/jquery.dataTables.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" type="text/css" rel="Stylesheet" />

<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-grid.min.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-table.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />

<div style="min-width: 1000px; border: 0px solid black; padding: 3px; margin: 0 auto;" id="allProjectMainDiv">
    <asp:Panel ID="dashboardError" runat="server" CssClass="dashboardError"></asp:Panel>
    <h2>PMT Projects</h2>
    <asp:DropDownList ID="ProjectStatusFilter" AutoPostBack="true" runat="server">
        <asp:ListItem Text="All Open Projects" Value="All Open"></asp:ListItem>
        <asp:ListItem Text="All Completed Projects" Value="Completed"></asp:ListItem>
        <asp:ListItem Text="All Cancelled Projects" Value="Cancelled"></asp:ListItem>
        <asp:ListItem Text="All On Hold Projects" Value="On Hold"></asp:ListItem>
        <asp:ListItem Text="All Projects" Value="All Projects"></asp:ListItem>
    </asp:DropDownList>
    <table id='AllOpenProjects' class='display'>
        <thead>
            <tr>
                <th>Parent Project Number</th>
                <th>Project Workflow Link</th>
                <th>Commercialization Link</th>
                <th>SAP Item #</th>
                <th>SAP Description</th>
                <th>Revised First Ship Date</th>
                <%--<th class="packagingNumbersCol">Packaging Numbers</th>--%>
                <th>Workflow Phase</th>
                <th>Product Hierarchy Level 1</th>
                <th>Material Group 1 Brand</th>
                <th>Manufacturing Location</th>
                <th>Packing Location</th>
                <th>Project Type</th>
                <th>Project Type SubCategory</th>
                <th>PM</th>
                <th>Packaging Engineer</th>
                <th>Initiator</th>
                <th>Customer</th>
                <th>Submitted Date</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>Parent Project Number</th>
                <th>Project Workflow Link</th>
                <th>Commercialization Link</th>
                <th>SAP Item #</th>
                <th>SAP Description</th>
                <th>Revised First Ship Date</th>
                <%--<th class="packagingNumbersCol">Packaging Numbers</th>--%>
                <th>Workflow Phase</th>
                <th>Product Hierarchy Level 1</th>
                <th>Material Group 1 Brand</th>
                <th>Manufacturing Location</th>
                <th>Packing Location</th>
                <th>Project Type</th>
                <th>Project Type SubCategory</th>
                <th>PM</th>
                <th>Packaging Engineer</th>
                <th>Initiator</th>
                <th>Customer</th>
                <th>Submitted Date</th>
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
        AllProjectDisplay();
        $('#ProjectStatusFilter').on('change', function () {
            var filterVal = this.value;
            $('#AllOpenProjects').dataTable().fnDestroy();
            if (filterVal == "All Open") {
                AllProjectDisplay("&$filter=(WorkflowPhase ne 'Completed' and WorkflowPhase ne 'On Hold' and WorkflowPhase ne 'Cancelled')");
            } else if (filterVal == "Completed") {
                AllProjectDisplay("&$filter=(WorkflowPhase eq 'Completed')");

            } else if (filterVal == "Cancelled") {
                AllProjectDisplay("&$filter=(WorkflowPhase eq 'Cancelled')");

            } else if (filterVal == "On Hold") {
                AllProjectDisplay("&$filter=(WorkflowPhase eq 'On Hold')");

            } else if (filterVal == "All Projects") {
                AllProjectDisplay();

            }
        });
    });

</script>


