<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BillOfMaterialsSetUpForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.BillOfMaterialsSetUpForm.BillOfMaterialsSetUpForm" %>

<div class="container" id="dvcontainer">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled!
            <br />
            If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False">HelpDesk</asp:LinkButton>.<br />

        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1 id="pageHead" runat="server"></h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
            <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPELead" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPLMProject" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Project Information</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Project Type:</label>
                <asp:TextBox ID="lblProjectType" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Project Type SubCategory:</label>
                <asp:TextBox ID="lblProjectSubcat" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Line/Workcenter Additional Info:</label>
                <asp:TextBox ID="txtWorkCenterAddInfo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Finished Good Pack Location:</label>
                <asp:TextBox ID="lblPackLocation" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Is Peg Hole Needed:</label>
                <asp:TextBox ID="lblPegHoleNeeded" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">FG Like Item:</label>
                <asp:TextBox ID="lblFGLikeItem" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-12">
            <div class="form-group">
                <label class="control-label">Item Concept :</label>
                <asp:TextBox ID="lblItemConcept" TextMode="MultiLine" Rows="6" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Project Team</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Project Initiator:</label>
                <asp:TextBox ID="peInitiator" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Brand Manager:</label>
                <asp:TextBox ID="peBrandManager" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">PM:</label>
                <asp:TextBox ID="peOBM" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">InTech Manager:</label>
                <asp:TextBox ID="peResearch" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Packaging Engineer:</label>
                <asp:TextBox ID="lblPackagingEngineerValue" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
        </div>
    </div>
    <div id="dvUserControl" class="dvUserControl" runat="server">
        <asp:PlaceHolder ID="phMsg" runat="server" />
    </div>
    <asp:PlaceHolder ID="phPage" runat="server">
        <div id="BOMPages" clientidmode="Static" runat="server">
            <asp:PlaceHolder ID="phBOM" runat="server" />
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-6 col-md-10">
                <asp:Label ID="lblSaved" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
            </div>
            <div class="col-xs-12 col-sm-3 col-md-1">
                <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="false" CssClass="ButtonControl" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
            </div>
            <div class="col-xs-12 col-sm-3 col-md-1">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return validateBOMControl(this);" CssClass="ButtonControl" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
            <asp:HiddenField ID="hdnComponentype" runat="server" />
            <asp:HiddenField ID="hdnParentID" runat="server" />
            <asp:HiddenField ID="hdnPackagingID" runat="server" />
            <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
            <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
            <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingNumbers" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnComponentStatusChangeIds" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingComponent" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnWorkflowSteps" ClientIDMode="Static" runat="server" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        conditionalChecks();
        pageLoadCheck();
        $('html, body').animate({ scrollTop: 0 }, 'fast');
    });
</script>
