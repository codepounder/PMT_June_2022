<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SecondaryApprovalReviewForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.SecondaryApprovalReviewForm.SecondaryApprovalReviewForm" %>

<div class="container">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled! <br />If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False" onclick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Sr. PM Secondary Review</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
            <div id="dverror_message" style="display:none;"> <ul id="error_message"></ul> </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>IPF Summary</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Type:</label>
                 <asp:TextBox ID="lblProjectType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label class="control-label">1st Ship Date:</label>
                 <asp:TextBox ID="lblRevisedFirstShipDate" runat="server" ReadOnly="True" ClientIDMode="Static"  CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label class="control-label">Weeks Until First Ship:</label>
                 <asp:TextBox ID="lblWeeksToShip" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
           
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
             <div class="form-group">
                <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                 <asp:TextBox ID="lblProductHierarchy1" runat="server" ReadOnly="True" ClientIDMode="Static"  CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 2:</label>
                 <asp:TextBox ID="lblProductHierarchy2" runat="server"  ReadOnly="True" ClientIDMode="Static"  CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label class="control-label">Material Group 1 (Brand):</label>
                 <asp:TextBox ID="lblBrand" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
           
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
             <div class="form-group">
                <label class="control-label">12 Month Projected $:</label>
                 <asp:TextBox ID="lblAnnualDollar" runat="server"  ReadOnly="True" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
             <div class="form-group">
                <label class="control-label">Annual Projected Retail Selling Units:</label>
                 <asp:TextBox ID="lblAnnualUnits" runat="server" ClientIDMode="Static" ReadOnly="True" CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div id="dvCustomer"  class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Customer:</label>
                 <asp:TextBox ID="lblCustomer" runat="server"  ReadOnly="True" ClientIDMode="Static"  CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>      
    </div>
    <div class="row"> 
          <div class="col-xs-12 col-sm-6 col-md-4" id="dvChannel">
            <div class="form-group">
                <label class="control-label">Channel:</label>
                <asp:TextBox ID="txtChannel" runat="server" BorderStyle="None" ClientIDMode="Static" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Project Notes:</label>
                <asp:TextBox ID="lblProjectNotes" runat="server" BorderStyle="None" TextMode="MultiLine" Rows="6" ReadOnly="True" CssClass="form-control"></asp:TextBox>  
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Initial Capacity Review</h1>
        </div>
    </div>
     <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
   <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Critical Data Points</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Annual Projected Retail Selling Units:</label>
                <asp:TextBox ID="lblAnnualProjectedUnits" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label class="control-label">Annual Projected Cases:</label>
                <asp:TextBox ID="lblAnnualProjectedCases" runat="server" ReadOnly="True" ClientIDMode="Static"  CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label class="control-label">Annual Projected lbs:</label>
                <asp:TextBox ID="lblAnnualProjectLbs" runat="server" ReadOnly="True" ClientIDMode="Static"  CssClass="form-control percent"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Make Location:</label>
                <asp:TextBox ID="lblMakeLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label class="control-label">Finished Good Pack Location:</label>
                <asp:TextBox ID="lblPrimaryPackLocation" runat="server"  ReadOnly="True" ClientIDMode="Static"  CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" >
            <%--<div class="form-group">
                <label class="control-label">2nd Pack Location:</label>
                 <asp:TextBox ID="lblSecondaryPackLocation" runat="server" ClientIDMode="Static" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>--%>
        </div>
    </div>
    <div class="row">
         <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments from Sr PM:</label>
                <asp:TextBox ID="lblCapacitySrOBMComments" runat="server" BorderStyle="None" TextMode="MultiLine" Rows="6" ReadOnly="True" CssClass="form-control"></asp:TextBox>  
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Capacity Review Details</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6" >
            <div class="form-group">
                <label class="control-label">Will there be any capacity issues at the Make Location?:</label>
                <asp:TextBox ID="lblMakeLocationIssues" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>  
            </div>
        </div>
        <div id="dvCustomer" class="col-xs-12 col-sm-6 col-md-6" >
            <div class="form-group">
                <label class="control-label">Will there be any capacity issues at the Pack Location?:</label>
                <asp:TextBox ID="lblPackLocationIssues" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments on Capacity/Risks:</label>
                <asp:TextBox ID="lblCapacityComments" runat="server" BorderStyle="None" TextMode="MultiLine" Rows="6" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Recommendation on Project Acceptance:</label>
                 <asp:TextBox ID="lblProjectDecisionCapacity" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox> 
            </div>
        </div>
    </div>
    <div class="row">
         <div class="col-xs-12 col-sm-6 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments on Project Acceptance:</label>
                <asp:TextBox ID="lblProjectAcceptance" runat="server" BorderStyle="None" TextMode="MultiLine" Rows="6" ReadOnly="True" CssClass="form-control"></asp:TextBox>  
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Initial Costing Review</h1>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Critical Data Points</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <label class="control-label">Expected Gross Margin %:</label><br />
                <asp:TextBox ID="lblExpectedGrossMargin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments from Sr PM:</label>
                <asp:TextBox ID="lblCostingSrOBMComments" runat="server" ReadOnly="true" Rows="6" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Costing Review Details</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <label id="lblMarginAccurate" class="control-label">Is Expected Gross Margin Accurate?:</label>
                <asp:TextBox ID="lblGrossMarginAccurate" runat="server" ReadOnly="true"  CssClass="form-control" TextMode="MultiLine" ></asp:TextBox>
            </div>
        </div>
        <div id="dvRevisedExpectedGrossMargin" class="col-xs-12 col-sm-12 col-md-6" runat="server">
            <div class="form-group">
                <label class="control-label">Revised Gross Margin %:</label>
                <asp:TextBox ID="lblRevisedGrossMargin" ClientIDMode="Static" runat="server" ReadOnly="true" CssClass="form-control" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments:</label>
                <asp:TextBox ID="lblInitialCostingComments" ClientIDMode="Static" runat="server" Rows="6" ReadOnly="true" CssClass="form-control" TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <label class="control-label">Recommendation on Project Acceptance:</label>
                <asp:TextBox ID="lblProjectDecisionCosting" runat="server" ReadOnly="true"  CssClass="form-control" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Initial Reviews Approval</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Based on the input above, how would you like to continue?:</label>
                <asp:DropDownList ID="drpCountinue" ClientIDMode="Static" onchange="conditionalCheck();" runat="server" CssClass="form-control required" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" ClientIDMode="Static" onclick="btnSubmit_Click" OnClientClick="return ValidateData()" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        conditionalCheck(); conditionalCheck();
    })
    function conditionalCheck() {
        if ($("#drpCountinue option:selected").text().toLowerCase() == 'put project on hold') {
            $("#btnSubmit").addClass('disabled');
            $("#btnSubmit").attr('disabled', true);
        }
        else {
            $("#btnSubmit").removeClass('disabled');
            $("#btnSubmit").attr('disabled', false);
        }
    }
</script>