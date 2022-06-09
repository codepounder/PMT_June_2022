<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMTProjectHeader2.ascx.cs" Inherits="Ferrara.Compass.WebParts.PMTProjectHeader2.PMTProjectHeader2" %>

<link rel="stylesheet" href="/_layouts/15/Ferrara.Compass/css/datepicker.min.css" />
<link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css?v=<%=versionNumber %>" rel="Stylesheet" />
<link href="../../_layouts/15/Ferrara.Compass/font-awesome/css/font-awesome.min.css" rel="stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/jquery.dataTables.css" type="text/css" rel="Stylesheet" />

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Bootstrap/bootstrap.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js?v=<%=versionNumber %>"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=<%=versionNumber %>"></script>

<asp:PlaceHolder ID="phJS" runat="server" />

<div class="container" id="divProjectHeader" runat="server">
    <div class="row">
        <div id="divFormLinks" style="text-align: right">
            <asp:LinkButton ID="lbTaskDashboard" runat="server" CausesValidation="False" OnClick="lbTaskDashboard_Click">Task Dashboard</asp:LinkButton><br />
            <asp:LinkButton ID="lbDisplayCommForm" runat="server" CausesValidation="False">Commercialization Form</asp:LinkButton><br />
            <asp:LinkButton ID="lbProjectStatusForm" runat="server" CausesValidation="False">Project Status Form</asp:LinkButton>
            <asp:HiddenField ID="hddOBMAdmin" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hddPackingEngineer" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hddBrandManager" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hddDeveloper" ClientIDMode="Static" runat="server" />
        </div>
    </div>
    <asp:Panel runat="server" ID="UATMessage" CssClass="redMessage" ClientIDMode="Static" Visible="false">UAT Testing Environment</asp:Panel>
    <asp:Panel runat="server" ID="DebugMessage" CssClass="redMessage" ClientIDMode="Static" Visible="false">PROJECT IS IN DEBUG MODE</asp:Panel>
    <asp:Button ID="btnDebugMode" ClientIDMode="Static" CssClass="ButtonControlAutoSize" Text="" CausesValidation="false" runat="server" OnClick="btnDebugMode_Click" Visible="false" />
    <div class="row PageSubHeader">
        <asp:Label CssClass="control-label" ID="lblProjectTitle" runat="server" ClientIDMode="Static"></asp:Label>
    </div>
    <div class="row PageSubHeader">
        <label class="control-label">Project Type:&nbsp;&nbsp;&nbsp;</label>
        <asp:Label CssClass="control-label" ClientIDMode="Static" ID="lblProjectType" runat="server"></asp:Label>
    </div>
    <div class="row PageSubHeader" id="divProjectTypeSubCategory" clientidmode="Static" runat="server">
        <label class="control-label">Project Type SubCategory:&nbsp;&nbsp;&nbsp;</label>
        <asp:Label CssClass="control-label" ClientIDMode="Static" ID="lblProjectTypeSubCategory" runat="server"></asp:Label>
    </div>
    <div class="row PageSubHeader">
        <label class="control-label">Current Workflow Phase:&nbsp;&nbsp;&nbsp;</label>
        <asp:Label CssClass="control-label" ClientIDMode="Static" ID="lblCurrentWorkflowPhase" runat="server"></asp:Label>
    </div>
    <div class="row PageSubHeader">
        <div id="divCompassMessage" runat="server">
            <h4>Special Message:&nbsp;&nbsp;&nbsp;<asp:Label ID="lblCompassMessage" runat="server" Text="" CssClass="control-label CompassMessage"></asp:Label></h4>
        </div>
    </div>
</div>
<!-- Modal -->
<div class="modal fade" id="DialogDeleteSemisMessage" role="dialog">
    <div class="modal-dialog">
        <!-- Modal content-->
        <div class="modal-content">
            <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                <h4 class="modal-title">Delete Component</h4>
            </div>
            <div class="modal-body" style="display: flex;">
                <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                <p style="margin-left: 10px;">Cannot delete this components as it has child elements.</p>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

<asp:Literal ID="litScript" runat="server"></asp:Literal>
<script type="text/javascript">
    $(document).ready(function () {
        /******* Following line is to disable Enter **************/
        document.onkeypress = stopRKey;

        $('.datePicker').datepicker({
            format: 'mm/dd/yyyy',
            autoclose: true
        });
        function isNumber(evt, element) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (
                (charCode != 45 || $(element).val().indexOf('-') != -1) &&
                (charCode != 46 || $(element).val().indexOf('.') != -1) &&
                (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
        showMessage("submittedMessage");
    })
</script>
