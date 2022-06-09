<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="InitialCostingReviewForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.InitialCostingReviewForm.InitialCostingReviewForm" %>

<script >
    $(document).ready(function () {
        conditionalChecks();
    });
    function conditionalChecks() {
        if ($("#ddlGrossMarginAccurate option:selected").text() == 'No') {
            showItem('dvGrossMargin');
            $("#txtRevisedGrossMargin").addClass('required');
        }
        else {
            $("#txtRevisedGrossMargin").val('');
            hideItem('dvGrossMargin');
            $("#txtRevisedGrossMargin").removeClass('required');
          
        }
    }
</script>
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
            <h1>Initial Costing Review</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" ShowSummary="true" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />  
            <div id="dverror_message" style="display:none;"> <ul id="error_message"></ul></div>
        </div>
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
                <label class="control-label">Expected Gross Margin %:</label>
                <asp:TextBox ID="lblExpectedGrossMargin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4"></div>
        <div class="col-xs-12 col-sm-6 col-md-4 form-group"></div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments from Sr. PM:</label>
                <asp:TextBox ID="lblComments" runat="server" BorderStyle="None" TextMode="MultiLine" Rows="6" ReadOnly="True" CssClass="form-control"></asp:TextBox>  
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Initial Costing Review</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label"><span style="color: red">* </span>Is Expected Gross Margin Accurate?:</label>
                 <asp:DropDownList ID="ddlGrossMarginAccurate" runat="server" ClientIDMode="Static" CssClass="form-control required">
                     <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                     <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                     <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div  class="col-xs-12 col-sm-6 col-md-4" id="dvGrossMargin">
            <div class="form-group">
                <label class="control-label"><span style="color: red">* </span>Revised Gross Margin %:</label>
                <div class="input-group"><asp:TextBox ID="txtRevisedGrossMargin" runat="server" ClientIDMode="Static" CssClass="form-control numericDecimal2"></asp:TextBox><span class="input-group-addon">%</span></div>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4 form-group"></div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments:</label>
               <asp:TextBox ID="txtCostingComments" runat="server" Height="77px" CssClass="form-control" TextMode="MultiLine" ></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
             <label class="control-label"><span style="color: red">* </span>Recommendations on Project Acceptance:</label>
              <asp:DropDownList ID="ddlProjectDecision" runat="server" CssClass="form-control required">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                <asp:ListItem Text="Accept" Value="A"></asp:ListItem>
                <asp:ListItem Text="Reject" Value="R"></asp:ListItem>
             </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4"></div>
        <div class="col-xs-12 col-sm-6 col-md-4 form-group"></div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Attachments</h2>
        </div>
    </div>
    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    <div  class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-3 col-md-4 CompassLabel">Upload Relevent Documents:</div>
        <div class="col-xs-12 col-sm-9 col-md-4 CompassLabel">  
            <asp:Button ID="btnUpload" Text="Upload Documents" OnClientClick="openBasicDialog('Upload Documents', 'Costing');return false;" CssClass="ButtonControlAutoSize" runat="server" />
        </div>
    </div>
    <div class="row" style="margin-top: 35px; margin-left: 10px;">
        <table>
            <asp:Repeater ID="rpAttachments" runat="server">
                <HeaderTemplate>
                    <tr>
                        <th>Action</th>
                        <th>Document Name</th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnkFileDelete" runat="server" CssClass="btn deleteMargin"  Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkFileDelete_Click" CausesValidation="false"></asp:LinkButton>
                        </td>
                        <td>
                            <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Links</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
               <ul>
                   <li>
                        <asp:LinkButton ID="lnlIPF" runat="server" CausesValidation="false" Text="Item Proposal Form" OnClick="lnlIPF_Click"  ></asp:LinkButton> 
                   </li>

               </ul>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
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
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CausesValidation="false" onclick="btnSubmit_Click" OnClientClick="return ValidateData()" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Reload Attachment" CssClass="ReloadAttachment" OnClick="btnReloadAttachment_Click" />
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
        </div>
    </div>
</div>