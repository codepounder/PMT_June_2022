<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyWorkflowTasksForm_New.ascx.cs" Inherits="Ferrara.Compass.WebParts.MyWorkflowTasksForm_New.MyWorkflowTasksForm_New" %>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=4"></script>

<link href="/_layouts/15/Ferrara.Compass/css/jquery.dataTables.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-grid.min.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-table.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />

<div style="width: 1000px; border: 0px solid black; padding: 3px;">
    <h2>My Current Tasks</h2>
    <table id='MyWorkflowTasks' class='display'>
        <thead>
            <tr>
                <th>Task Status</th>
                <th>Project Name</th>
                <th>Task</th>
                <th class="nonProc">Requested Date</th>
                <th class="nonProc">Due Date</th>
                <th class="nonProc">1st Production Date</th>
                <th>1st Ship Date</th>
                <th>Project Type</th>
                <th>Project Type Subcategory</th>
                <th>Material Group 1 (Brand)</th>
                <th>Initiator</th>
                <th class="proc">Packaging Numbers</th>
                <th class="proc">Commercialization Link</th>
                <th>Product Hierarchy Level 1</th>
            </tr>
        </thead>

        <tbody>
            <asp:Repeater ID="rptAllCurrentTasks" runat="server" OnItemDataBound="rptAllCurrentTasks_ItemDataBound">
                <ItemTemplate>
                    <tr runat="server" id="taskRow">
                        <td>
                            <div runat="server" id="lblTaskStatus"></div>
                        </td>
                        <td runat="server" cssclass="control-label" id="lblProjectName"></td>
                        <td runat="server" cssclass="control-label" id="lblTask"></td>
                        <td runat="server" cssclass="control-label" id="lblRequestedDate" class="nonProc"></td>
                        <td runat="server" cssclass="control-label" id="lblDueDate" class="nonProc"></td>
                        <td runat="server" cssclass="control-label" id="lblFirstProdDate" class="nonProc"></td>
                        <td runat="server" cssclass="control-label" id="lblFirstShipDate"></td>
                        <td runat="server" cssclass="control-label" id="lblProjectType"></td>
                        <td runat="server" cssclass="control-label" id="lblProjectTypeSubCat"></td>
                        <td runat="server" cssclass="control-label" id="lblMaterialGroup1Brand"></td>
                        <td runat="server" cssclass="control-label" id="lblInitiator"></td>
                        <td runat="server" cssclass="control-label" id="lblPackagingNumbers" class="proc"></td>
                        <td runat="server" cssclass="control-label" id="lblCommLink" class="proc"></td>
                        <td runat="server" cssclass="control-label" id="lblProdHierarchyLvl1"></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
        <tfoot>
            <tr>
                <th>Task Status</th>
                <th>Project Name</th>
                <th>Task</th>
                <th class="nonProc">Requested Date</th>
                <th class="nonProc">Due Date</th>
                <th class="nonProc">1st Production Date</th>
                <th>1st Ship Date</th>
                <th>Project Type</th>
                <th>Project Type Subcategory</th>
                <th>Material Group 1 (Brand)</th>
                <th>Initiator</th>
                <th class="proc">Packaging Numbers</th>
                <th class="proc">Commercialization Link</th>
                <th>Product Hierarchy Level 1</th>

            </tr>
        </tfoot>
    </table>
    <asp:Literal ID="litTable" runat="server"></asp:Literal>
    <asp:HiddenField ID="hidTableDisplayType" runat="server" Value="false" />
    <asp:HiddenField ID="hdnProcurement" ClientIDMode="Static" runat="server" Value="false" />
</div>
<asp:Literal ID="litScript" runat="server"></asp:Literal>
<script type="text/javascript">
    var results = new Array();
    function lookupItem(requestArray, requestId) {
        var value = null;
        $.each(requestArray, function (index, item) { if (item.Id == requestId) value = item; });
        return value;
    }
    function flowTasks() {
        var ProcVisibility = true;
        var NonProcVisibility = false;
        var HeaderTitle = "Revised First Ship Date";
        var columns = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12, 13];
        if ($("#hdnProcurement").val() != "true") {
            ProcVisibility = false;
            NonProcVisibility = true;
            HeaderTitle = "First Ship Date";
            columns = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13];
        }
        var myWorkflowTasks = $('#MyWorkflowTasks').DataTable({
            initComplete: function () {
                this.api().columns(columns).every(function (index) {
                    var column = this;
                    var select = $('<select><option value=""></option></select>')
                        .appendTo($(column.footer()).empty())
                        .on('change', function () {

                            var val;
                            if (index == 2 || index == 12) {

                                //Check the value of the selected option and set val variable to the same as your hidden text string.  I am using "True" and "False"
                                val = $(this).val();

                                //Call the search method with regex and update/redraw the table
                                column.search(val ? val : '', true, false).draw();

                            } else {

                                //If we are not in the offending icon column, run a standard search
                                val = $.fn.dataTable.util.escapeRegex(
                                    $(this).val()
                                );
                                column.search(val ? '^' + val + '$' : '', true, false).draw();
                            }

                        });

                    column.data().unique().sort().each(function (d, j) {
                        if (column.index() == 2 || column.index() == 12) { d = $('<div/>').html(d).text(); }
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            },
            "columnDefs": [
                {
                    "render": function (data, type, row) {
                        if (ProcVisibility == true) {
                            return makeTable(data);
                        } else {
                            return data;
                        }
                    },
                    "targets": 11
                },
                {
                    title: HeaderTitle,
                    targets: 6
                },
                {
                    type: 'title-string',
                    targets: [0, 2]
                },
                {
                    targets: [3, 4, 5],
                    visible: NonProcVisibility,
                    searchable: NonProcVisibility
                },
                {
                    targets: [11, 12],
                    visible: ProcVisibility,
                    searchable: ProcVisibility
                },
            ],
            "aLengthMenu": [
                [10, 25, 50, 75, -1],
                [10, 25, 50, 75, "All"]
            ],
            "pageLength": 25,
            "order": [
                [0, 'asc'],
                [4, 'asc']
            ]
        });
    }

    $(document).ready(function () {
        flowTasks();
    });
</script>
