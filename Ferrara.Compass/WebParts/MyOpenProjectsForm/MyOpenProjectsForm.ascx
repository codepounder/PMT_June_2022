<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyOpenProjectsForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.MyOpenProjectsForm.MyOpenProjectsForm" %>


<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=7"></script>

<link href="/_layouts/15/Ferrara.Compass/css/jquery.dataTables.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css?v=2" type="text/css" rel="Stylesheet" />

<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-grid.min.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-table.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />

<div style="min-width: 1000px; border: 0px solid black; padding: 3px; margin: 0 auto;" id="openParentProjectMainDiv">
    <asp:Panel ID="dashboardError" runat="server" CssClass="dashboardError"></asp:Panel>
    <h2>My Open Parent Projects</h2>
    <asp:Literal ID="litTableParentProjects" runat="server"></asp:Literal>

    <asp:HiddenField ID="hidProjectGroup" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hidProcurement" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hidProjectInitiator" runat="server" Value="" ClientIDMode="Static" />
    <asp:HiddenField ID="hidTableDisplayType" runat="server" Value="false" />
    <asp:HiddenField ID="hidFilterType" runat="server" Value="false" ClientIDMode="Static" />
    <asp:HiddenField ID="hidFilterValue" runat="server" Value="false" ClientIDMode="Static" />
</div>
<div style="min-width: 1000px; border: 0px solid black; padding: 3px; margin: 0 auto;" id="openChildProjectMainDiv">
    <h2>My Open Child Projects</h2>
    <asp:Literal ID="litTableChildProjects" runat="server"></asp:Literal>
</div>
<asp:Literal ID="litScriptParent" runat="server"></asp:Literal>
<asp:Literal ID="litScriptChild" runat="server"></asp:Literal>
<script type="text/javascript">
    //Datatable sort
    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "title-string-pre": function (a) {
            return a.match(/title="(.*?)"/)[1].toLowerCase();
        },

        "title-string-asc": function (a, b) {
            return ((a < b) ? -1 : ((a > b) ? 1 : 0));
        },

        "title-string-desc": function (a, b) {
            return ((a < b) ? 1 : ((a > b) ? -1 : 0));
        }
    });
    $(document).ready(function () {
        DTDisplayParentProject();
        DTDisplayChildProject();
    });

</script>


