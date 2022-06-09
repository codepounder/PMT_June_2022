<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" ClientIDMode="Static" CodeBehind="InitialCapacityReviewForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.InitialCapacityReviewForm.InitialCapacityReviewForm" %>

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
            <h1>Initial Capacity Review</h1>
        </div>
    </div>
     <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
   <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
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
                <label class="control-label">Annual Projected Retail Selling Units:</label>
                <asp:TextBox ID="lblAnnualProjectedUnits" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Annual Projected Cases:</label>
                <asp:TextBox ID="lblAnnualProjectedCases" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Annual Projected lbs:</label>
                <asp:TextBox ID="lblAnnualProjectLbs" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl1stmonthU" runat="server" class="control-label"><span id="1stmonthU"></span></label>
                <asp:TextBox ID="txtProjectUnit1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
           <div class="form-group">
                <label id="lbl1stmonthC" runat="server" class="control-label"><span id="1stmonthC"></span></label>
                <asp:TextBox ID="txtProjectCase1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label id="lbl1stmonthL" runat="server" class="control-label"><span id="1stmonthL"></span></label>
                <asp:TextBox ID="txtProjectlbs1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
             <div class="form-group">
                <label id="lbl2ndmonthU" runat="server" class="control-label"><span id="2ndmonthU"></span></label>
                <asp:TextBox ID="txtProjectUnit2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>    
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl2ndmonthC" runat="server" class="control-label"><span id="2ndmonthC"></span></label>
                <asp:TextBox ID="txtProjectCase2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label id="lbl2ndmonthL" runat="server" class="control-label"><span id="2ndmonthL"></span></label>
                <asp:TextBox ID="txtProjectlbs2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl3rdmonthU" runat="server"  class="control-label"></label>
                <asp:TextBox ID="txtProjectUnit3" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>      
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label id="lbl3rdmonthC" runat="server" class="control-label"></label>
                <asp:TextBox ID="txtProjectCase3" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" >
            <div class="form-group">
                <label id="lbl3rdmonthL" runat="server"  class="control-label"></label>
                <asp:TextBox ID="txtProjectlbs3" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Make Location:</label>
                <asp:TextBox ID="lblPrimaryMakeLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Finished Good Pack Location:</label>
                <asp:TextBox ID="lblPrimaryPackLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control secondPackLoc"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4" id="dvSecondaryPackLocation">
            <%--<div class="form-group">
                <label runat="server" ID="lblSecondaryPackLocationLabel" class="control-label">2nd Pack Location:</label>
                <asp:TextBox ID="lblSecondaryPackLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>--%>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">First Ship Date:</label>
                <asp:TextBox ID="lblFirstShipDate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                <asp:TextBox ID="lblLineOfBusiness" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments from Sr. PM:</label>
                <asp:TextBox ID="lblComments" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Initial Capacity Review</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Recommended First Production Date:</label>
                 <asp:TextBox ID="txtProductionDate" ClientIDMode="Static" runat="server" CssClass="datePicker form-control required" ToolTip="Click to Choose Date"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Will there be any capacity issues at the Make Location?:</label>
                <asp:DropDownList ID="ddlMakeLocationIssues" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Will there be any capacity issues at the Pack Location?:</label>
                <asp:DropDownList ID="ddlPackLocationIssues" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div id="divCapacityComments" class="form-group">
                <label class="control-label">Comments on Capacity/Risk:</label>
                <asp:TextBox ID="txtCapacityComments" runat="server"  CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Recommendations on Project Acceptance:</label>
                <asp:DropDownList ID="ddlProjectDecision" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Accept" Value="A"></asp:ListItem>
                    <asp:ListItem Text="Reject" Value="R"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label">Comments on Project Acceptance:</label>
                <asp:TextBox ID="txtProjectAcceptance" runat="server"  CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Attachments</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-3 col-md-4 CompassLabel">
            Upload Documents:
        </div>
        <div class="col-xs-12 col-sm-9 col-md-4 CompassLabel">  
            <asp:Button ID="btnUpload" Text="Upload Documents" OnClientClick="openBasicDialog('Upload Documents','Capacity');return false;" CssClass="ButtonControlAutoSize" runat="server" />
        </div>
    </div>
    <div class="row">
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
                            <asp:LinkButton ID="lnkFileDelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkFileDelete_Click" CausesValidation="false"></asp:LinkButton>
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
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" OnClientClick="return ValidateData()" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Reload Attachment" CausesValidation="false" CssClass="ReloadAttachment" OnClick="btnReloadAttachment_Click"  />
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        var packLocation2 = $('#lblSecondaryPackLocation').val();
        if (packLocation2 == "Select...") {
            $('#dvSecondaryPackLocation').addClass('hidden');
        }
        else {
            $('#dvSecondaryPackLocation').removeClass('hidden');
        }
    });
</script>