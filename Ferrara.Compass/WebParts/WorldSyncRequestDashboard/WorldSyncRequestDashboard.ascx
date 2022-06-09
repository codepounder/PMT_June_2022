<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorldSyncRequestDashboard.ascx.cs" Inherits="Ferrara.Compass.WebParts.WorldSyncRequestDashboard.WorldSyncRequestDashboard" %>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jszip.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/dataTables.buttons.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/buttons.html5.min.js"></script>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js"></script>

<link href="/_layouts/15/Ferrara.Compass/css/jquery.dataTables.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" type="text/css" rel="Stylesheet" />

<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-grid.min.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-table.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />

<div style="min-width: 1000px; padding: 3px;" id="myRequestMainDiv">
    <asp:Panel ID="dashboardError" runat="server" CssClass="dashboardError">
    </asp:Panel>
    <br />
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled!
            <br />
            If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False" OnClick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row">
        <div id="div1" runat="server" class="col-xs-12 col-sm-12 col-md-12">
            <asp:HyperLink ID="hlkNewRequest" runat="server">World Sync Request File</asp:HyperLink>
        </div>
    </div>
    <div class="row">
        <div id="divWorldSyncFuseFile" runat="server" class="col-xs-12 col-sm-12 col-md-12">
            <asp:HyperLink ID="hlkWorldSyncFuseFile" runat="server">Create World Sync FUSE File</asp:HyperLink>
        </div>
    </div>
    <h2>My Current Requests</h2>
    <br />
    <table id='tabMyCurrentRequests' class='display'>
        <thead>
            <tr>
                <th>RequestId</th>
                <th>SAP Item #</th>
                <th>SAP Description</th>
                <th>Status</th>
                <th>Task</th>
                <th>Submitted Date</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>RequestId</th>
                <th>SAP Item #</th>
                <th>SAP Description</th>
                <th>Status</th>
                <th>Task</th>
                <th>Submitted Date</th>
            </tr>
        </tfoot>
    </table>
    <h2>Open Requests</h2>
    <br />
    <table id='tabOpenRequests' class='display'>
        <thead>
            <tr>
                <th>RequestId</th>
                <th>SAP Item #</th>
                <th>SAP Description</th>
                <th>Status</th>
                <th>Step</th>
                <th>Request Type</th>
                <th>Submitted Date</th>
            </tr>
        </thead>
        <tfoot>
            <tr>
                <th>RequestId</th>
                <th>SAP Item #</th>
                <th>SAP Description</th>
                <th>Status</th>
                <th>Step</th>
                <th>Request Type</th>
                <th>Submitted Date</th>
            </tr>
        </tfoot>
    </table>
</div>
<asp:Literal ID="litScript" runat="server"></asp:Literal>
<script>
    function MyRequestsDisplay() {
        var myOpenProjects = $('#tabMyCurrentRequests').DataTable({
            initComplete: function () {
                this.api().columns().every(function () {
                    var column = this;
                    var select = $('<select><option value=""></option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                            loadingIconAdded = true;
                        });

                    column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                        var val = $('<div/>').html(d).text();
                        select.append('<option value="' + val + '">' + val + '</option>');
                    });
                });
            },
            data: MyWorldSyncReqTasks,
            fixedColumns: true,
            "SubmittedDate": [6, 'desc'],
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            columns: [
            { 'data': 'RequestId' },
            { 'data': 'SAPnumber' },
            { 'data': 'SAPdescription' },
            { 'data': 'RequestStatus' },
            { 'data': 'Task' },
                {
                    'data': 'SubmittedDate',
                    'render': function (data) {
                        return dateJavaScriptSerializer(data);
                    }
                }
            ],
            columnDefs: [
            {
                targets: 0,
                className: "text-center"
            }]
        });

        loadingIconAdded = true;
    }
    function OpenRequestsDisplay() {
        var myOpenProjects = $('#tabOpenRequests').DataTable({
            initComplete: function () {
                this.api().columns().every(function () {
                    var column = this;
                    var select = $('<select><option value=""></option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );

                            column
                                .search(val ? '^' + val + '$' : '', true, false)
                                .draw();
                            loadingIconAdded = true;
                        });

                    column.cells('', column[0]).render('display').sort().unique().each(function (d, j) {
                        var val = $('<div/>').html(d).text();
                        select.append('<option value="' + val + '">' + val + '</option>');
                    });
                });
            },
            data: OpenWorldSyncReqTasks,
            fixedColumns: true,
            "SubmittedDate": [6, 'desc'],
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
            columns: [
            { 'data': 'RequestId' },
            { 'data': 'SAPnumber' },
            { 'data': 'SAPdescription' },
            { 'data': 'RequestStatus' },
            { 'data': 'WorkflowStep' },
            { 'data': 'RequestType' },
                {
                    'data': 'SubmittedDate',
                    'render': function (data) {
                        return dateJavaScriptSerializer(data);
                    }
                }
            ],
            columnDefs: [
            {
                targets: 0,
                className: "text-center"
            }]
        });

        loadingIconAdded = true;
    }
    $(document).ready(function () {
        MyRequestsDisplay();
        OpenRequestsDisplay();
    });
</script>
