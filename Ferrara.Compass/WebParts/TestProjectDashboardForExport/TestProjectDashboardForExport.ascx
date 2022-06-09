<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestProjectDashboardForExport.ascx.cs" Inherits="Ferrara.Compass.WebParts.TestProjectDashboardForExport.TestProjectDashboardForExport" %>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js"></script>
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />


<script>
    $(function () {
        runFloatHead();
        $(window).on("scroll", function (evt) {
            var el = $("#projectDashboard"),
               offset = el.offset(),
               scrollTop = $(window).scrollTop(),
               floatingHeader = $(".floatingHeader");
            var scrollLeft = -1 * ($(window).scrollLeft()-5);
            //alert(scrollTop+">"+offset.top+scrollTop+"<"+offset.top + el.height());
            if ((scrollTop > offset.top) && (scrollTop < offset.top + el.height())) {
                floatingHeader.css({
                    "visibility": "visible",
                    "left": scrollLeft+"px"
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
</script>

<asp:Panel id="projectProgressDashboard" runat="server" CssClass ="projectProgressDashboard" ClientIDMode="Static">
    <h1>Project Status Report</h1>
    <h2><asp:Panel id="testPanel" runat="server" ClientIDMode="Static"></asp:Panel></h2>
    <asp:PlaceHolder ID="phMessaging" runat="server" />
    <asp:Panel runat="server" ClientIDMode="Static" id ="workflowStatus">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label id="lblworkflowStatus" runat="server" ClientIDMode="Static" class="control-label">Current Workflow State:</label><asp:Panel ID="updatedWorkflowStatus" ClientIDMode="Static" runat="server"></asp:Panel>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <asp:LinkButton ID="lbDisplayCommForm" runat="server" CausesValidation="False">Commercialization Form</asp:LinkButton><br />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:HiddenField ID="hddProjectId" runat="server" />
    <asp:Panel id="updateLinkPanel" runat="server" ClientIDMode="Static">&nbsp;</asp:Panel>
    <asp:Panel ID ="todayPanel" ClientIDMode="Static" runat ="server"> | </asp:Panel>
    <asp:Table id="projectDashboard" runat="server" ClientIDMode="Static">
        <asp:TableHeaderRow TableSection="TableHeader" runat="server" id="headerRow"  ClientIDMode="Static">

            <asp:TableHeaderCell CssClass="processCol">Process</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="statusCol">Status</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="startSourceCol">Start Source</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="endSourceCol">End Source</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="spacingCol"></asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="startDayCol">Start Day</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="endDayCol">End Day</asp:TableHeaderCell>
            <asp:TableHeaderCell CssClass="totalDaysCol">Duration</asp:TableHeaderCell>
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
    <asp:Panel ID ="dataTablesDiv" ClientIDMode="Static" runat="server"></asp:Panel>
   <asp:Panel runat="server" ClientIDMode="Static" id ="updateDateButtons">
        <div class="row">
            <div class="col-sm-1">
                <div class="form-group">
                    <label id="lblFirstShipDate" runat="server" class="control-label">Update First Ship Date:</label>
                    <asp:TextBox ID="txtFirstShipDate" ClientIDMode="Static" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-1">
                <div class="form-group">
                    <label id="hiddenLabel1" runat="server" class="control-label">&nbsp;</label><br />
                    <asp:Button ID="btnFirstShipDate"  CssClass="ButtonControl"  CausesValidation="false" runat="server" Text="Submit" OnClick="updateDate" />
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-1">
                <div class="form-group">
                    <label id="lblFirstProdDate" runat="server" class="control-label">Update First Production Date:</label>
                    <asp:TextBox ID="txtFirstProdDate" ClientIDMode="Static" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                </div>
            </div>
            <div class="col-sm-1">
                <div class="form-group">
                    <label id="hiddenLabel2" runat="server" class="control-label">&nbsp;</label><br />
                    <asp:Button ID="btnFirstProdDate" CssClass="ButtonControl"  CausesValidation="false" runat="server" Text="Submit" OnClick="updateDate" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ClientIDMode="Static" id ="workflowChanges">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-9">
                <div class="form-group">
                    <asp:Button ID="preproduction" CssClass="ButtonControlAutoSize"  CausesValidation="false" runat="server" Text="Pre-Production" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="onhold" CssClass="ButtonControlAutoSize"  CausesValidation="false" runat="server" Text="On Hold" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="completed" CssClass="ButtonControlAutoSize"  CausesValidation="false" runat="server" Text="Completed" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="cancelled" CssClass="ButtonControlAutoSize"  CausesValidation="false" runat="server" Text="Cancelled" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="changerequest" CssClass="ButtonControlAutoSize"  CausesValidation="false" runat="server" Text="Change Request" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="copyrequest" CssClass="ButtonControlAutoSize"  CausesValidation="false" runat="server" Text="Copy Request" OnClick="updateWorkflowStatus" />
                    <asp:Button ID="exportrequest" CssClass="ButtonControlAutoSize"  CausesValidation="false" runat="server" Text="Export Request" OnClick="updateWorkflowStatus" />
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="updateButtons" runat="server">
        <asp:Label ID="lblSaved" CssClass="" runat="server"></asp:Label>
            
                <asp:Button ID="btnSave" runat="server" CssClass="clickSave button justifyRight" Text="Submit" OnClick="btnSave_Click" CausesValidation="False" />
    </asp:Panel>
    
</asp:Panel>