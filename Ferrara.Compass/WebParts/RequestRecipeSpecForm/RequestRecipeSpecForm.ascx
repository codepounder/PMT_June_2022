<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
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
            <h1>Request <%=requestTypeForForm%> Recipe & Spec Form</h1>
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
                <asp:TextBox ID="lblLineofBusiness" runat="server" BorderStyle="None" ReadOnly="True" Text="Everyday (000000025)" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Brand::</label>
                <asp:TextBox ID="lblBrand" runat="server" BorderStyle="None" ReadOnly="True" Text="Multiple" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Desired 1st Ship Date::</label>
                <asp:TextBox ID="lblDesiredShipDate" runat="server" BorderStyle="None" ReadOnly="True" Text="22-Sep-2022" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Leader:</label>
                <asp:TextBox ID="lblProjectLeader" runat="server" BorderStyle="None" Text="Leah" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Project Manager:</label>
                <asp:TextBox ID="lblProjectManager" runat="server" BorderStyle="None" Text="Tim" ReadOnly="True" CssClass="form-control"></asp:TextBox>
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
    <asp:Panel ID="divFGSection" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <span class="markrequired">*</span><label class="control-label">Is this Request for FG Recipe & Spec for a New or Existing FG number?:</label>
                    <asp:DropDownList ID="ddlRequestNewExistingFG" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="New" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Existing" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divSpecDescription">
                    <label class="control-label"><span class="markrequired">*</span> Description :</label>
                    <asp:TextBox ID="txtDescription" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    <label class="comment-block">Example: TBD [Brand Abbrv] [Product] [General Pack Size]</label>
                </div>
            </div>
        
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divLikeFg">
                    <label class="control-label">Like FG # (Please enter a "Like" FG Number from which to copy)</label>
                    <asp:TextBox ID="TextBox3" runat="server" Text="0" CssClass="form-control"></asp:TextBox>
                    <label class="comment-block">Like FG # should represent the same pack structure as the new FG being requested.</label>

                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group" id="divExistingFg">
                    <label class="control-label"><span class="markrequired">*</span>Existing FG #</label>
                    <asp:TextBox ID="TextBox4" runat="server" Text="0" CssClass="form-control"></asp:TextBox>

                </div>
            </div>
        </div>
        <div class="row"> 
 
            <div class="col-xs-12 col-sm-6 col-md-4 MemberDiv" runat="server" id="divMakePackFG">
                <div runat="server" id="div3">
                    <div class="form-group">
                         <label class="control-label ProjectManager">FG Pack Location:</label>
                        <asp:DropDownList ID="drpMakeLocation" ClientIDMode="Static" CssClass="form-control required ddlMember" runat="server">
                            <asp:ListItem Value="-1">Select...</asp:ListItem>
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
                                            <asp:TextBox ID="txtMakePackNameValue" Value='<%# DataBinder.Eval(Container.DataItem, "MakepackNameValue") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                            <asp:TextBox ID="txtMakePackName" Value='<%# DataBinder.Eval(Container.DataItem, "MakePackName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>

                                        </div>
                                    </td>
                                    <td class="DeleteRow">
                                        <div class="form-group">
                                            <asp:Image ID="btnDeleteRow" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForMakePackName');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                            <asp:HiddenField ID="hdnDeletedStatusForMakePackName" Value="false" runat="server" ClientIDMode="Static" />
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    <asp:Button ID="btnAddMakePackPlantFG" OnClick="btnAddMakePackPlantFG_Click" ClientIDMode="Static" runat="server" Text="Add Make Pack" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
                </div>
            </div>






        </div>
        <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <span class="markrequired">*</span><label class="control-label">Send form to Ops Team for review of FG Pack Location(s)?:</label>
                        <asp:DropDownList ID="DropDownList2" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
     </asp:Panel>    
    <!-- Transfer Semi  Section -->
        <asp:Panel ID="divSemiSection" runat="server">
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <span class="markrequired">*</span><label class="control-label">Is this Request for Transfer Semi Recipe & Spec for a New or Existing number?:</label>
                        <asp:DropDownList ID="ddlRequestNewExistingSemi" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="New" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Existing" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group" id="divSpecDescriptionSemi">
                        <label class="control-label"><span class="markrequired">*</span> Description :</label>
                        <asp:TextBox ID="txtSemiDescription" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        <label class="comment-block">Example: TBD [Brand Abbrv] [Product] [General Pack Size]</label>
                    </div>
                </div>
           
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group" id="divLikeSemi">
                        <label class="control-label">Like Transfer Semi  # (Please enter a "Like" Transfer Semi  Number from which to copy)</label>
                        <asp:TextBox ID="txtLikeSemiNo" runat="server" Text="0" CssClass="form-control"></asp:TextBox>
                        <label class="comment-block">Like Transfer Semi  # should represent the same pack structure as the new Transfer Semi  being requested.</label>

                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group" id="divExistingSemi">
                        <label class="control-label"><span class="markrequired">*</span>Existing Transfer Semi  #</label>
                        <asp:TextBox ID="txtLikeSemiExistingNo" runat="server" Text="0" CssClass="form-control"></asp:TextBox>

                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label ProjectManager">Transfer Semi Pack Location</label>
                        <asp:DropDownList ID="ddlMakePackSemi" ClientIDMode="Static" CssClass="form-control required ddlMember" runat="server">
                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <asp:Image ID="Image1" class="AddMember" onClick="AddMakePackPlantFGRow_New(this, 'btnAddMakePackPlantSemi')" Style="cursor: pointer; height: 16px; float: right; margin-top: -43px" runat="server" AlternateText="Add Row" ImageUrl="../../_layouts/15/Ferrara.Compass/images/plus.png" ToolTip="Add " />

                    </div>
                    <table class="MakePackSEMITableNew" style="width: 100%">
                    <asp:Repeater ID="rptMakePackSemi" runat="server" ClientIDMode="Static">

                        <ItemTemplate>
                            <tr>
                                <td runat="server" id="tdDeliverableSemi">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtMakePackNameSemiValue" Value='<%# DataBinder.Eval(Container.DataItem, "MakepackNameValue") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtMakePackNameSemi" Value='<%# DataBinder.Eval(Container.DataItem, "MakePackName") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>

                                    </div>
                                </td>
                                <td class="DeleteRow">
                                    <div class="form-group">
                                        <asp:Image ID="btnDeleteRowSemi" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForMakePackNameSemi');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="hdnDeletedStatusForMakePackNameSemi" Value="false" runat="server" ClientIDMode="Static" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    
                </div> 
                <asp:Button ID="btnAddMakePackPlantSemi" OnClick="btnAddMakePackPlantSemi_Click" ClientIDMode="Static" runat="server" Text="Add Make Pack" Style="visibility: hidden; display: none; height: 5px;" CssClass="ButtonControl" />
            </div>


            
            <div class="row">
                <div class="col-xs-12 col-sm-col-6 col-md-6">
                    <asp:Button ID="btnAddSemi" OnClick="btnAddSemi_Click" ClientIDMode ="Static" runat="server" Text="Add new Transfer Semi" Style="width:200px; background-color:darkcyan;color:white;"/>
                    <%--<a href="javascript:void(0);" onclick="AddSemi()" style="width:220px !important;" class="btn btn-success">Add new Transfer Semi</a>--%>
                    <%--<asp:button  value="Add new Transfer Semi" id="btnaddSemi" onClick="AddSemi()" name="Add new Transfer Semi"  class="ButtonControlAutoSize">Add new Transfer Semi</asp:button>--%>
                </div>

            </div>
            <div class="col-xs-12 col-sm-col-12 col-md-12">
                <table class="MakePackSEMITableNew" style="width: 100%">
                       <asp:Repeater ID="rptNewSemiComponent" runat="server" ClientIDMode="Static">
                        <HeaderTemplate>
                            <tr>
                               <td>Request For</td> 
                                <td>Description</td> 
                                <td>Like Semi #</td>  
                                <td>Existing Semi #</td> 
                                <td>Location</td> 
                                <td></td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtRequestForSemiValue" Value='<%# DataBinder.Eval(Container.DataItem, "RequestForValue") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtRequestForSemi" Value='<%# DataBinder.Eval(Container.DataItem, "RequestFor") %>' runat="server" CssClass="required form-control ReadOnlyMembers"  Style="width: 95%"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDescriptionForSemiValue" Value='<%# DataBinder.Eval(Container.DataItem, "DescriptionValue") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtDescriptionForSemi" Value='<%# DataBinder.Eval(Container.DataItem, "Description") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtLikeSemiNoValue" Value='<%# DataBinder.Eval(Container.DataItem, "LikeSemiNoValue") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtLikeSemiNo" Value='<%# DataBinder.Eval(Container.DataItem, "LikeSemiNo") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtExistingNoValue" Value='<%# DataBinder.Eval(Container.DataItem, "ExistingNoValue") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtExistingNo" Value='<%# DataBinder.Eval(Container.DataItem, "ExistingNo") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                </td>
                                  <td>
                                    <asp:TextBox ID="txtLocationValue" Value='<%# DataBinder.Eval(Container.DataItem, "LocationValue") %>' runat="server" CssClass="required form-control" Style="visibility: hidden; display: none;"></asp:TextBox>
                                        <asp:TextBox ID="txtLocation" Value='<%# DataBinder.Eval(Container.DataItem, "Location") %>' runat="server" CssClass="required form-control ReadOnlyMembers" Style="width: 95%"></asp:TextBox>
                                </td>
                                <td class="DeleteRow">
                                    <div class="form-group">
                                        <asp:Image ID="btnDeleteRowSemiAdd" class="DeleteRow" Style="cursor: pointer; margin-top: -10px; margin-right: -4px;" onClick="deleteRow(this,'hdnDeletedStatusForAddNewSemi');return false;" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" ClientIDMode="Static" />
                                        <asp:HiddenField ID="hdnDeletedStatusForAddNewSemi" Value="false" runat="server" ClientIDMode="Static" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    </table>
              
                <%--<table class="table" id="NewSemiComponent" style="width: 100%;">
                    <thead>
                        <tr>
                            <th>Request For</th>
                            <th>Description</th>
                            <th>Like Semi #</th>
                            <th>Existing #</th>
                            <th>Location</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody id="addedSemiComponents">
                        
                    </tbody>
                   
                     
                </table>--%>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <span class="markrequired">*</span><label class="control-label">Send form to Ops Team for review of Transfer Semi  Pack Location(s)?:</label>
                        <asp:DropDownList ID="DropDownList1" runat="server" AppendDataBoundItems="true" CssClass="form-control required">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                            <asp:ListItem Text="No" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </asp:Panel>

    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <asp:Label ID="Label2" CssClass="control-label" Text="Comments:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottoOnClientCont-weight: 700;"></asp:Label>
                <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
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
    var selctedItems ='';
    $(document).ready(function () {
        $('#NewSemiComponent').addClass('hide');
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
    
    function AddMakeItem() {
       
    }
    function AddSemi() {
        <%--$('#NewSemiComponent').removeClass('hide');
        var table = document.getElementById('NewSemiComponent');
        var rowCount = table.rows.length;
        var row = table.insertRow(rowCount);

        //Column 1  
        var cell1 = row.insertCell(0);
        var s = $("#<%=ddlRequestNewExistingSemi.ClientID %> option:selected").text();
        cell1.innerHTML = s;

        //Column 2 
        var cell2 = row.insertCell(1);
        var value = document.getElementById('<%=TextBox2.ClientID%>').value;
        cell2.innerHTML = value;
      //  cell2.innerHTML = $('#TextBox2').val();

        //Column 3
        var cell3 = row.insertCell(2);
        var value3 = document.getElementById('<%=TextBox5.ClientID%>').value;
        cell3.innerHTML = value3;
        //Column 4
        var cell4 = row.insertCell(3);
        var value4 = document.getElementById('<%=TextBox6.ClientID%>').value;
        cell4.innerHTML = value4;


        // locaiton
        var cell6 = row.insertCell(4);
        cell6.innerHTML = selctedItems;
        //Column 1  
        var cell5 = row.insertCell(5);
        var element1 = document.createElement("input");
        element1.type = "button";
        var btnName = "button" + (rowCount + 1);
        element1.name = btnName;
        element1.setAttribute('value', 'Remove'); // or element1.value = "button";  
        element1.onclick = function () { removeRow(btnName); }
        cell5.appendChild(element1); --%>
    }
    function removeRow(btnName) {
        try {
          
            var table = document.getElementById('NewSemiComponent');
            var rowCount = table.rows.length;
            for (var i = 0; i < rowCount; i++) {
                var row = table.rows[i];
                var rowObj = row.cells[5].childNodes[0];
                if (rowObj.name == btnName) {
                    table.deleteRow(i);
                    rowCount--;
                }
            }
        }
        catch (e) {
            alert(e);
        }
    }
    $(document).ready(function () {
        
        //var table = document.getElementById('NewSemiComponent');
        //var rowCount = table.rows.length;
        //if (rowCount == 0) {
        //    $('#NewSemiComponent').addClass('hide');
        //}
      

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


    function deleteRow(clicked, hdnDeletedStatus) {
        $('#error_message').empty();
        var button = $(clicked);
        button.closest("tr").addClass("hideItem");
        button.closest("td").find("#" + hdnDeletedStatus).val("true");
    }


    function AddMakePackPlantFGRow_New(clicked, btnId) {

        var s = $("#<%=ddlRequestNewExistingFG.ClientID %> option:selected").text();
        var button = $(clicked);
        var MemberDiv = button.closest(".MemberDiv");
        var ddlMember = MemberDiv.find('.ddlMember');
        if (ddlMember != "-1") {
            $('#' + btnId).click();
        }
    }

</script>
