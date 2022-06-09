<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMTHeaderForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.PMTHeaderForm.PMTHeaderForm" %>

<link rel="stylesheet" href="/_layouts/15/Ferrara.Compass/css/datepicker.min.css" />
<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css?v=<%=versionNumber %>" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/StageGateStyles.css?v=<%=versionNumber %>" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/font-awesome/css/font-awesome.min.css" rel="stylesheet" />

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Bootstrap/bootstrap.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/stageGateJS.js?v=<%=versionNumber %>"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js?v=<%=versionNumber %>"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=<%=versionNumber %>"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/tinymce/jquery.tinymce.min.js?v=<%=versionNumber %>"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/tinymce/tinymce.min.js?v=<%=versionNumber %>"></script>

<asp:PlaceHolder ID="phJS" runat="server" />

<div class="container" id="divPMTProjectHeader" runat="server">
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-10">&nbsp;<br>
            &nbsp;</div>
        <div id="divFormLinks col-xs-12 col-sm-12 col-md-2" style="float: right">
            <asp:LinkButton ID="lbTaskDashboard" runat="server" CausesValidation="False" OnClick="lbTaskDashboard_Click">Task Dashboard</asp:LinkButton><br />
            <asp:LinkButton ID="lbProjectStatusForm" runat="server" CausesValidation="False">Project Summary Form</asp:LinkButton><br />
            <asp:LinkButton ID="lbExtraLink" OnClick="lbExtraLink_Click" runat="server" CausesValidation="False" Visible="false"></asp:LinkButton>
            <asp:HiddenField ID="hddDeveloper" ClientIDMode="Static" runat="server" />
        </div>
    </div>
    <asp:Panel runat="server" ID="UATMessage" CssClass="redMessage" ClientIDMode="Static" Visible="false">UAT Testing Environment</asp:Panel>
    <asp:Panel runat="server" ID="DebugMessage" CssClass="redMessage" ClientIDMode="Static" Visible="false">PROJECT IS IN DEBUG MODE</asp:Panel>
    <asp:Button ID="btnDebugMode" ClientIDMode="Static" CssClass="ButtonControlAutoSize" Text="" CausesValidation="false" runat="server" OnClick="btnDebugMode_Click" Visible="false" />
    <div class="row PageSubHeader" runat="server" id="divProjectTitle">
        <asp:Label CssClass="control-label" ID="lblProjectTitle" runat="server"></asp:Label>
    </div>
    <div class="row PageSubHeader" runat="server" id="divProjectType">
        <label class="control-label">Project Type:&nbsp;&nbsp;&nbsp;</label>
        <asp:Label CssClass="control-label" ID="lblProjectType" runat="server"></asp:Label>
    </div>
    <div class="row PageSubHeader" id="divProjectTypeSubCategory" runat="server">
        <label class="control-label">Project Type SubCategory:&nbsp;&nbsp;&nbsp;</label>
        <asp:Label CssClass="control-label" ID="lblProjectTypeSubCategory" runat="server"></asp:Label>
    </div>
    <div class="row PageSubHeader" id="divProjectStage" runat="server">
        <label class="control-label">Current Project Stage:&nbsp;&nbsp;&nbsp;</label>
        <asp:Label CssClass="control-label" ID="lblProjectStage" ClientIDMode="Static" runat="server"></asp:Label>
    </div>
    <div class="row PageSubHeader" runat="server" id="divPageTitle">
        <h1>
            <asp:Label CssClass="control-label" ID="lblPageTitle" runat="server"></asp:Label>
        </h1>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        document.onkeypress = stopRKey;
        $('.datePicker').datepicker({
            format: 'mm/dd/yyyy',
            autoclose: true
        });
    })

    function SetTarget() {
        document.forms[0].target = "_blank";
    }
</script>
