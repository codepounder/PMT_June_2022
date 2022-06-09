<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectDashboard2.ascx.cs" Inherits="Ferrara.Compass.WebParts.ProjectDashboard2.ProjectDashboard2" %>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=115"></script>
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css?v=113" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js"></script>
<script type="text/javascript">
    $(function () {
        runFloatHead();
        $(window).on("scroll", function (evt) {
            var el = $("#projectDashboard"),
               offset = el.offset(),
               scrollTop = $(window).scrollTop(),
               floatingHeader = $(".floatingHeader");
            var scrollLeft = -1 * ($(window).scrollLeft() - 5);
            //alert(scrollTop+">"+offset.top+scrollTop+"<"+offset.top + el.height());
            if ((scrollTop > offset.top) && (scrollTop < offset.top + el.height())) {
                floatingHeader.css({
                    "visibility": "visible",
                    "left": scrollLeft + "px"
                });
            } else {
                floatingHeader.css({
                    "visibility": "hidden"
                });
            }

        });
        $('.datePicker').datepicker({
            format: 'mm/dd/yyyy',
            autoclose: true
        });
    });

    function OpenDialog(tTitle, page) {
        var url = '/_layouts/15/Ferrara.Compass/AppPages/' + page + '.aspx?CompassItemId=' + $("#hddProjectId").val();
        var options = {
            url: url,
            title: tTitle,
            dialogReturnValueCallback: function (result) {
                window.location.reload();
            }
        }
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    }
</script>

<asp:Panel ID="projectProgressDashboard" runat="server" CssClass="projectProgressDashboard" ClientIDMode="Static">
    <h1>Project Status Report</h1>
    <h2>
        <asp:Panel ID="testPanel" runat="server" ClientIDMode="Static"></asp:Panel>
    </h2>
    <asp:PlaceHolder ID="phMessaging" runat="server" />
    <asp:Panel runat="server" ClientIDMode="Static" ID="workflowStatus">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-12">
                <div class="form-group">
                    <h2>
                        <label id="lblworkflowStatus" runat="server" clientidmode="Static" class="control-label">Current Workflow State:</label>
                        <asp:Panel ID="updatedWorkflowStatus" ClientIDMode="Static" runat="server"></asp:Panel>
                    </h2>
                    <br />
                    <h2>
                        <asp:Panel ID="updatedProjectType" ClientIDMode="Static" runat="server"></asp:Panel>
                    </h2>
                    <br />
                    <h2 id="h2ProjectTypeSubCategory" runat="server">
                        <asp:Panel ID="updatedProjectTypeSubCategory" ClientIDMode="Static" runat="server"></asp:Panel>
                    </h2>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <asp:LinkButton ID="lbDisplayCommForm" runat="server" CausesValidation="False">Commercialization Form</asp:LinkButton><br />
                    <asp:LinkButton ID="lbDisplayParentSummaryForm" runat="server" CausesValidation="False">Project Summary Form</asp:LinkButton><br />
                </div>
            </div>
        </div>

    </asp:Panel>
    <asp:HiddenField ID="hddProjectId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnProjectNumber" runat="server" ClientIDMode="Static" />
    <asp:Panel ID="updateLinkPanel" runat="server" ClientIDMode="Static">&nbsp;</asp:Panel>
    <asp:Panel ID="todayPanel" ClientIDMode="Static" runat="server">| </asp:Panel>
    <asp:Table ID="projectDashboard" runat="server" ClientIDMode="Static">
        <asp:TableHeaderRow TableSection="TableHeader" runat="server" ID="headerRow" ClientIDMode="Static">
            <asp:TableHeaderCell CssClass="processCol">Process</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="statusCol">Status</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="submittedCol">Submitted By</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="emailCol">Email</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="startDayCol">Original<br />Start Day</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="endDayCol">Original<br />End Day</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="totalDaysCol">Original<br />Duration</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="startDayCol">Actual<br />Start Day</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="endDayCol">Actual<br />End Day</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="totalDaysCol">Actual<br />Duration</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <%--<asp:TableHeaderCell CssClass="calendarCol" ID ="calendarHeader">
                <div class="positionDiv">
                    <table>
                        <asp:TableRow runat="server" ID="calendarDisplay" CssClass="dailyDisplayRow"></asp:TableRow>
                        <asp:TableRow  runat="server" ID="dailyDisplay" CssClass="dailyDisplayRow"></asp:TableRow>
                    </table>
                    
                </div>
            </asp:TableHeaderCell>--%>
        </asp:TableHeaderRow>
    </asp:Table>
    <asp:Panel ID="dataTablesDiv" ClientIDMode="Static" runat="server"></asp:Panel>
    <asp:Panel runat="server" ClientIDMode="Static" ID="pnExtGraphicsMessage">
        <br />
        <div class="row">
            <div class="form-group">
                <label id="Label1" runat="server" style="color: red; font-weight: bold; margin-left: 25px;">* PLEASE NOTE: External graphic workflows may vary from standard workflows. External projects may not include proof upload and approval step. Dates shown for graphics process above may be omitted or out of order. Please reach out to your External Procurement contact to understand particular External graphics workflow and task owners.</label>
            </div>
        </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ClientIDMode="Static" ID="updateDateButtons">
        <div class="row">
            <div style="width: 250px; float: left;">
                <div class="form-group">
                    <label id="lblFirstShipDate" runat="server" class="control-label">Update First Ship Date:</label>
                    <asp:TextBox ID="txtFirstShipDate" ClientIDMode="Static" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-1">
                <div class="form-group">
                    <label id="hiddenLabel1" runat="server" class="control-label">&nbsp;</label><br />
                    <asp:Button ID="btnFirstShipDate" CssClass="ButtonControl" CausesValidation="false" runat="server" Text="Submit" OnClick="updateDate" />
                </div>
            </div>
        </div>
        <div class="row">
            <div style="width: 250px; float: left;">
                <div class="form-group">
                    <label id="lblFirstProdDate" runat="server" class="control-label">Update First Production Date:</label>
                    <asp:TextBox ID="txtFirstProdDate" ClientIDMode="Static" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-1">
                <div class="form-group">
                    <label id="hiddenLabel2" runat="server" class="control-label">&nbsp;</label><br />
                    <asp:Button ID="btnFirstProdDate" CssClass="ButtonControl" CausesValidation="false" runat="server" Text="Submit" OnClick="updateDate" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ClientIDMode="Static" ID="workflowChanges">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-9">
                <div class="form-group">
                    <asp:Button ID="preproduction" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text="Pre-Production" OnClientClick="javascript:return DBConfirm('Are you sure you want to change this project to Pre-Production?');" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="onhold" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text="On Hold" OnClientClick="javascript:return DBConfirm('Are you sure you want to change this project to On Hold?');" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="removeOnHold" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text=" Remove On Hold" OnClientClick="javascript:return DBConfirm('Are you sure you want to reactivate this project?');" OnClick="reactivateProject" />
                    <asp:Button ID="completed" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text="Completed" OnClientClick="OpenDialog('Complete Child Project','CompleteChildProject'); return false;" />
                    <asp:Button ID="cancelled" CssClass="ButtonControlAutoSize" CausesValidation="false" runat="server" Text="Cancel" OnClientClick="OpenDialog('Cancel Project','CancelProject'); return false;" />
                    <asp:Button ID="changerequest" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text="Change Request" OnClientClick="javascript:return DBConfirm('Are you sure you want to submit this change request?');" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="copyrequest" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text="Copy Request" OnClientClick="return GetChildProjectSearch();" />
                    <asp:Button ID="exportrequest" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text="Export Request" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="btnMoveChild" CssClass="ButtonControlAutoSize noIcon" CausesValidation="false" runat="server" Text="Move to Alternative Project" OnClientClick="OpenDialog('Move Child','MoveIPF'); return false;" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="updateButtons" runat="server">
        <asp:Label ID="lblSaved" CssClass="" runat="server"></asp:Label>

        <asp:Button ID="btnSave" runat="server" CssClass="clickSave button justifyRight" Text="Submit" OnClick="btnSave_Click" CausesValidation="False" />
    </asp:Panel>
    </asp:ContentPanel></asp:UpdatePanel>
</asp:Panel>
<script>
    function GetChildProjectSearch() {
        var url = "/_layouts/15/Ferrara.Compass/AppPages/CopyIPF.aspx?ChildProjectNo=" + $("#hddProjectId").val() + "&CopyMode=CopyToAlternativeParentProject";
        var options = {
            url: url,
            title: "Copy to Alternative Parent Project"
        };
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
        return false;
    }
</script>
