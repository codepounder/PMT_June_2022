<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StageGateGenerateIPFsForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.StageGateGenerateIPFsForm.StageGateGenerateIPFsForm" %>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/DataTables/jquery.dataTables.min.js"></script>

<link href="/_layouts/15/Ferrara.Compass/css/jquery.dataTables.css" type="text/css" rel="Stylesheet" />

<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-grid.min.css" type="text/css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap-table.css" type="text/css" rel="Stylesheet" />
<style>
    .container.ProjectNotesContainerParent {
        margin-top: 0;
    }
</style>
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
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="Submission Failed!<br/>Please correct the errors below:" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>

        </div>
    </div>
    <asp:PlaceHolder runat="server" ID="SGSProjectInformation"></asp:PlaceHolder>
    <asp:Panel ID="phRAGuide" runat="server" CssClass="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Key:</h2>
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-1 KeyGenerate">
                    &nbsp;<br />
                    &nbsp;
                </div>
                <div class="col-xs-12 col-sm-12 col-md-5"><strong>Generate IPF</strong>: this will only appear once the status of the line item is “green” and all required fields are filled out if you have a new FG # for your item, clicking this will generate the IPF for you to go into and fill out in more detail. Once the IPF has been generated all fields on the Generate IPF form will lock and all edits will need to be made in the IPF itself.</div>
                <div class="col-xs-12 col-sm-12 col-md-1 KeyRequest">
                    &nbsp;<br />
                    &nbsp;
                </div>
                <div class="col-xs-12 col-sm-12 col-md-5"><strong>Request new FG#, UPC, UCC</strong>: this will only appear if you select “Yes” for the dropdown for “New SAP FG#?” and you have generated the IPF. Clicking this will automatically send a ticket to master data requesting a new FG#, UPC# or UCC# as you have indicated in the required fields.</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-1 KeyIPF">
                    &nbsp;<br />
                    &nbsp;
                </div>
                <div class="col-xs-12 col-sm-12 col-md-5"><strong>IPF Button</strong>: this will only appear after you have clicked the “Generate IPF” button and if you click on it, it will bring you directly to the IPF you are drafting.</div>
                <div class="col-xs-12 col-sm-12 col-md-1 KeyDelete">
                    &nbsp;<br />
                    &nbsp;
                </div>
                <div class="col-xs-12 col-sm-12 col-md-5"><strong>Delete Line Item</strong>: this will only appear before the blue cross button is selected – once you generate the IPF you cannot delete it in this form</div>
            </div>
        </div>
    </asp:Panel>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12"></div>
    </div>
    <table id='ChildIPFs' class='display'>
        <thead>
            <tr>
                <th>Item Detail</th>
                <th>Status</th>
                <th>Child Project #</th>
                <th>New SAP FG #</th>
                <th>SAP FG #</th>
                <th>SAP Description</th>
                <th>Material Group 1 (Brand)</th>
                <th>Customer</th>
                <th>Project Status</th>
                <th>Actions</th>
                <th>Added</th>
                <!--<th>Create IPF</th>
                <th>Delete</th>-->
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="rptChildProjects" runat="server" OnItemCommand="rptChildProjects_ItemCommand" OnItemDataBound="rptChildProjects_ItemDataBound">
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:ImageButton ID="imgItemDetail" CausesValidation="false" Height="16" Width="16" CssClass="select itemDetails" AlternateText="Select" CommandName="LoadControl" ImageUrl="/_layouts/15/Ferrara.Compass/images/Edit.gif" runat="server" /></td>
                        <td>
                            <asp:Image ID="imgStatus" ClientIDMode="Static" runat="server" /></td>
                        <td>
                            <asp:HyperLink ID="lnkProjectNumber" runat="server" CssClass="ProjectNumber"></asp:HyperLink>
                            <asp:HiddenField ID="hdnProjectNumber" runat="server" />
                            <asp:HiddenField ID="hdnPMTProjectListItemId" runat="server" />
                            <asp:HiddenField ID="hdnCompassListItemId" runat="server" />
                            <asp:HiddenField ID="hdnIPFGenerated" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnPopupValidated" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnManuallyCreateSAPDescription" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnProductHierarchyLevel1" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnProductHierarchyLevel2" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnMaterialGroup5" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnCustomerSpecific" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnCustomer" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnSortOrder" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTBDIndicator" CssClass="form-control ddlTBDIndicator" onChange="updateRowStatus(this);" OnSelectedIndexChanged="UpDateSAPNomenclature" AutoPostBack="true"  ToolTip="Please select TBD Indicator" runat="server">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSAPItemNumber" ToolTip="SAP Item Number" onChange="updateRowStatus(this);" CssClass="form-control PrelimField txtSAPItemNumber" Style="cursor: pointer"></asp:TextBox></td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSAPDescription" ToolTip="SAP Description" MaxLength="60" onChange="updateRowStatus(this);" CssClass="form-control PrelimField" Style="cursor: pointer"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="ddlMaterialGroup1Brand" OnSelectedIndexChanged="UpDateSAPNomenclature" AutoPostBack="true" runat="server" ToolTip="Brand Material Group 1" onChange="updateRowStatus(this);" CssClass="form-control PrelimField" Style="cursor: pointer">
                                <asp:ListItem Value="-1">Select...</asp:ListItem>
                            </asp:DropDownList></td>
                        <td>
                            <asp:DropDownList ID="ddlCustomer" runat="server" OnSelectedIndexChanged="UpDateSAPNomenclature" ToolTip="Customer" AutoPostBack="true"  CssClass="form-control" Style="cursor: pointer">
                                <asp:ListItem Value="-1">Select...</asp:ListItem>
                            </asp:DropDownList></td>
                        <td>
                            <asp:Label ID="lblProjectStatus" CssClass="control-label" runat="server" Style="cursor: pointer"></asp:Label>
                        </td>
                        <td>
                            <asp:ImageButton CssClass="needsNew" ID="imgNeedsNew" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' CommandName="workflow" ToolTip="New FG#/UCC/UPC" runat="server" Visible="false" OnClientClick="alert('A new Finished Good Item number, UPC, and UCC request has been sent to Master Data. The estimated time for completion is 2 business days.');" />
                            <asp:ImageButton CssClass="createIPF" ID="imgCreateIPF" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ProjectNumber") %>' CommandName="generate" ToolTip="Create IPF" runat="server" Visible="false" ImageUrl="/_layouts/15/Ferrara.Compass/img/generated.png" OnClientClick="return ValidateGenerateIPF(this);" />
                            <asp:HyperLink Target="_blank" CssClass="IPFGenerated" ID="lnkIPFGenerated" runat="server" ToolTip="Item Proposal Form" Visible="false"><img src="/_layouts/15/Ferrara.Compass/img/generate.png" /></asp:HyperLink>
                            <asp:ImageButton CssClass="deleteIPF" ID="imgDeleteIPF" CausesValidation="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "ProjectNumber") %>' ToolTip="Delete" CommandName="delete" runat="server" Visible="false" ImageUrl="/_layouts/15/Ferrara.Compass/img/delete.png" /></td>
                        <td>
                            <asp:Label ID="lblSubmitter" CssClass="control-label" runat="server"></asp:Label></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
        <tfoot>
            <tr>
                <th>Item Detail</th>
                <th>Status</th>
                <th>Child Project #</th>
                <th>New SAP FG #</th>
                <th>SAP FG #</th>
                <th>SAP Description</th>
                <th>Material Group 1 (Brand)</th>
                <th>Customer</th>
                <th>Project Status</th>
                <th>Actions</th>
                <!--<th>Needs New FG#/UCC/UPC</th>
                <th>Create IPF</th>
                <th>Delete</th>-->
            </tr>
        </tfoot>
    </table>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div id="dvUserControl" class="dvUserControl" runat="server">
                <asp:PlaceHolder ID="phMsg" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnUCLoaded" runat="server" Value="false" />
    <asp:HiddenField ID="hdnUCLoadedIPFGenerated" runat="server" Value="" />
    <asp:HiddenField ID="hdnUCLoadedchildProjectNo" runat="server" Value="" />
    <asp:HiddenField ID="hdnUCLoadedPMTItemId" runat="server" Value="" />
    <asp:HiddenField ID="hdnUCLoadedId" runat="server" Value="" />
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <asp:Button ID="btnAddIPF" ClientIDMode="Static" runat="server" Text="Add New Item" CausesValidation="false" CssClass="ButtonControlAutoSize" OnClick="addTempIPF_Click" />
        </div>
        <div class="col-xs-12 col-sm-12 col-md-6">
            <asp:Button ID="btnSaveIPF" ClientIDMode="Static" runat="server" Text="Save Items" CssClass="ButtonControlAutoSize" OnClick="saveData_Click" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <asp:Button ID="btnCopyIPF" ClientIDMode="Static" runat="server" Text="Copy Existing IPF" CausesValidation="false" CssClass="ButtonControlAutoSize" OnClientClick="return GetProjectSearch();" />
        </div>
        <div class="col-xs-12 col-sm-12 col-md-6"></div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" ClientIDMode="Static" CssClass="SuccessMessage" runat="server"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
            <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnLOB" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPMTListItemId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnStage" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnParentProjectNumber" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <asp:Literal ID="ChildrenScript" runat="server"></asp:Literal>
    <asp:Literal ID="DDLProductHierarchy1" runat="server"></asp:Literal>
    <asp:Literal ID="DDLProductHierarchy2" runat="server"></asp:Literal>
    <asp:Literal ID="DDLBrand" runat="server"></asp:Literal>
    <asp:Literal ID="DDLProductPackTypeList" runat="server"></asp:Literal>
    <asp:Literal ID="DDLProductFormList" runat="server"></asp:Literal>

</div>
<script type="text/javascript">
    //Datatable sort
    jQuery.extend(jQuery.fn.dataTableExt.oSort, {
        "title-string-pre": function (a) {
            return a.match(/title="(.*?)"/)[1].toLowerCase();
        },

        "title-string-asc": function (a, b) {
            return ((a < b) ? -1 : ((a > b) ? 1 : 0));
        },

        "title-string-desc": function (a, b) {
            return ((a < b) ? 1 : ((a > b) ? -1 : 0));
        }
    });
    $(document).ready(function () {
        GenerateIPFs();
    });

</script>
