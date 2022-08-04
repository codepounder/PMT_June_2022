<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestRecipeSpecForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.RequestRecipeSpecForm.RequestRecipeSpecForm" %>

<div class="container" id="dvMain" runat="server">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled!
            <br />
            If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False">HelpDesk</asp:LinkButton>.<br />
            <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" OnClick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />--%> 
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>Request Recipe & Spec Form</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
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
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Gate 0 Approved Date:</label>
                <asp:TextBox ID="lblGate0ApprovedDate" runat="server" BorderStyle="None" ReadOnly="True" Text="10-Jun-2022" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Line of Business:</label>
                <asp:TextBox ID="lblLineofBusiness" runat="server" BorderStyle="None" ReadOnly="True" Text="Line of Business" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Brand::</label>
                <asp:TextBox ID="lblBrand" runat="server" BorderStyle="None" ReadOnly="True" Text="Brand Name" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Desired 1st Ship Date::</label>
                <asp:TextBox ID="lblDesiredShipDate" runat="server" BorderStyle="None" ReadOnly="True" text="22-Sep-2022" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Leader:</label>
                <asp:TextBox ID="lblProjectLeader" runat="server" BorderStyle="None" Text="Tim" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Manager:</label>
                <asp:TextBox ID="lblProjectManager" runat="server" BorderStyle="None" Text="Project Manager" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <asp:Label ID="Label1" CssClass="control-label" Text="Project Concept Overview:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                <asp:TextBox ID="TextBox1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div> 
     
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Request</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <!-- FG Section -->
    <asp:Panel id="divFGSection" runat="server">
         <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Is this Request for FG Recipe & Spec for a New or Existing FG number?:</label>
                <asp:DropDownList ID="ddlRequestNewExistingFG" runat="server"   AppendDataBoundItems="true" CssClass="form-control required">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="New" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Existing" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
         <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="divSpecDescription">
                <label class="control-label"><span class="markrequired">*</span> Description :</label>
                <asp:TextBox ID="txtDescription" runat="server"   Text=""   CssClass="form-control"></asp:TextBox>
                <label class="comment-block">Example: TBD [Brand Abbrv] [Product] [General Pack Size]</label>
            </div>
        </div>
    </div>
   
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="divLikeFg">
                <label class="control-label">Like FG # (Please enter a "Like" FG Number from which to copy)</label>
                <asp:TextBox ID="TextBox3" runat="server" Text="0"   CssClass="form-control"></asp:TextBox>
                  <label class="comment-block">Like FG # should represent the same pack structure as the new FG being requested.</label>
                
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="divExistingFg">
                <label class="control-label"><span class="markrequired">*</span>Existing FG #</label>
                <asp:TextBox ID="TextBox4" runat="server" Text="0"   CssClass="form-control"></asp:TextBox>
              
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">

                <div class="form-group">
                    <label class="control-label ProjectManager"> Make/Pack Plant:</label>
                </div>
                <div>
                    <div runat="server" id="div3">
                        <div class="form-group">
                            <asp:DropDownList ID="ddlMakePackPlantFG" ClientIDMode="Static" runat="server" CssClass="ddlMember form-control" AppendDataBoundItems="true" Style="margin-top: -12px; width: 90%">
                               <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>

                    <asp:ListItem Value="1" Text="FM01 &gt; SL07"></asp:ListItem>
                    <asp:ListItem Value="13" Text="FM01 &gt; SL07 &gt; SL13"></asp:ListItem>
                    <asp:ListItem Value="46" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP01"></asp:ListItem>
                    <asp:ListItem Value="36" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP03"></asp:ListItem>
                    <asp:ListItem Value="2" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP14"></asp:ListItem>
                    <asp:ListItem Value="59" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP20"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div runat="server" id="div4">
                        <div class="form-group">
                            <asp:Image ID="ImgAddMakePackPlantFG" class="AddMember" onClick="AddMakePackPlantFGRow_New(this, 'btnAddMakePackPlantFG')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />
                        </div>
                    </div>
                    <div>
                        <table class="MakePackFGTableNew" style="width: 100%">
                            <asp:Repeater ID="rptMakePackFGRepeater" runat="server" ClientIDMode="Static">
                                <ItemTemplate>
                                    <tr>
                                        <td runat="server" id="tdDeliverable">
                                            <div class="form-group">
                                                <asp:TextBox ID="txtMakePackName" Value='<%# DataBinder.Eval(Container.DataItem, "MemberLoginName") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>

                                            </div>
                                        </td>
                                        <td class="DeleteRow">
                                            <div class="form-group">
                                                <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForMakePackFG');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdnDeletedStatusForMakePackFG" Value="false" runat="server" ClientIDMode="Static" />
                                            </div>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                        <asp:Button ID="btnAddMakePackPlantFG" OnClick="btnAddAddMakePackPlangFG_Click" ClientIDMode="Static" runat="server" Text="Add Make Pack" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                    </div>
                </div>




            </div>
        </div>
    </div>
    </asp:Panel>
   

     <!-- Semi Section -->
    <asp:Panel id="divSemiSection" runat="server">
         <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Is this Request for Semi Recipe & Spec for a New or Existing Semi number?:</label>
                <asp:DropDownList ID="ddlRequestNewExistingSemi" runat="server"   AppendDataBoundItems="true" CssClass="form-control required">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="New" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Existing" Value="2"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
         <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="divSpecDescriptionSemi">
                <label class="control-label"><span class="markrequired">*</span> Description :</label>
                <asp:TextBox ID="TextBox2" runat="server"   Text=""   CssClass="form-control"></asp:TextBox>
                <label class="comment-block">Example: TBD [Brand Abbrv] [Product] [General Pack Size]</label>
            </div>
        </div>
    </div>
   
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="divLikeSemi">
                <label class="control-label">Like Semi # (Please enter a "Like" Semi Number from which to copy)</label>
                <asp:TextBox ID="TextBox5" runat="server" Text="0"   CssClass="form-control"></asp:TextBox>
                  <label class="comment-block">Like Semi # should represent the same pack structure as the new Semi being requested.</label>
                
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group" id="divExistingSemi">
                <label class="control-label"><span class="markrequired">*</span>Existing Semi #</label>
                <asp:TextBox ID="TextBox6" runat="server" Text="0"   CssClass="form-control"></asp:TextBox>
              
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Make/Pack Plant</label>
                <asp:DropDownList ID="DropDownList3" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>

                    <asp:ListItem Value="1" Text="FM01 &gt; SL07"></asp:ListItem>
                    <asp:ListItem Value="13" Text="FM01 &gt; SL07 &gt; SL13"></asp:ListItem>
                    <asp:ListItem Value="46" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP01"></asp:ListItem>
                    <asp:ListItem Value="36" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP03"></asp:ListItem>
                    <asp:ListItem Value="2" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP14"></asp:ListItem>
                    <asp:ListItem Value="59" Text="FM01 &gt; SL07 &gt; SL13 &gt; FP20"></asp:ListItem>
                </asp:DropDownList>


            </div>
        </div>
    </div>
    </asp:Panel>
   
   <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <asp:Label ID="Label2" CssClass="control-label" Text="Comments:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                <asp:TextBox ID="txtRemarks" runat="server"   CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
            </div>
        </div>
    </div> 

    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="false" CssClass="ButtonControl" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnProjectType" runat="server" />
        </div>
    </div>
</div>
<div class="container" id="dvMsg" visible="false" runat="server">
    <div class="row">
        <div id="div1" runat="server" class="col-xs-12 col-sm-12 col-md-12 PageAccessDenied">
            WARNING: This Project Type doesn't have access to this page.
        </div>
    </div>
</div>
<script type="text/javascript">
          $(document).ready(function () {
              $("#<%=ddlRequestNewExistingSemi.ClientID %>").change(function (e) {
                
         var s = $("#<%=ddlRequestNewExistingSemi.ClientID %> option:selected").text();
        if (s == "New") {
            $('#divSpecDescriptionSemi').removeClass('hide');
            $('#divLikeSemi').removeClass('hide');
            $('#divExistingSemi').addClass('hide');
            //$('#txtProductFormDescription').addClass('required')
        }
        else {
            $('#divSpecDescriptionSemi').addClass('hide');
            $('#divLikeSemi').addClass('hide');
            $('#divExistingSemi').removeClass('hide');
        }
        });
    });
</script>

<script type="text/javascript">
    $(document).ready(function () {
        $("#<%=ddlRequestNewExistingFG.ClientID %>").change(function (e) {

                  var s = $("#<%=ddlRequestNewExistingFG.ClientID %> option:selected").text();
                  if (s == "New") {
                      $('#divSpecDescription').removeClass('hide');
                      $('#divLikeFg').removeClass('hide');
                      $('#divExistingFg').addClass('hide');
                      //$('#txtProductFormDescription').addClass('required')
                  }
                  else {
                      $('#divSpecDescription').addClass('hide');
                      $('#divLikeFg').addClass('hide');
                      $('#divExistingFg').removeClass('hide');
                  }
              });
          });
</script>
<script>


   

    function dropNewChanged() {
        //var t = $("#ddlRequestNewExisting option:selected").text();
        //var s = $("#ddlRequestNewExisting option:selected").text();
        //if (s == "New") {
        //    $('#divSpecDescription').removeClass('hide');
        //    $('#divLikeFg').removeClass('hide');
        //    $('#divExistingFg').addClass('hide');
        //    //$('#txtProductFormDescription').addClass('required')
        //}
        //else {
        //    $('#divSpecDescription').addClass('hide');
        //    $('#divLikeFg').addClass('hide');
        //    $('#divExistingFg').removeClass('hide');
        //}
    }

    
</script>