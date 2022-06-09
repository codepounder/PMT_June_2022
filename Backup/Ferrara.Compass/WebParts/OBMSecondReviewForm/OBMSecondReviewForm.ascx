<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OBMSecondReviewForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.OBMSecondReviewForm.OBMSecondReviewForm" %>

<div class="container" id="dvcontainer">
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled!
            <br />
            If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False" OnClick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>PM Second Review</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
            <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDeletedCompIds" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <h3 class="accordion">
        <asp:Label ID="lblTitle" Text="PM 1st Review Summary" ClientIDMode="Static" CssClass="titlelbl" runat="server"></asp:Label></h3>
    <div class="panel">
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
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Product Hierarchy Level 1/Line of Business:</label>
                    <asp:TextBox ID="lblLineOfBusiness" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Product Hierarchy Level 2:</label>
                    <asp:TextBox ID="lblProductHierarchyLevel2" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Material Group 1 (Brand):</label>
                    <asp:TextBox ID="lblBrand" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">First Ship Date:</label>
                    <asp:TextBox ID="lblFirstShipDate" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Weeks Until Ship:</label>
                    <asp:TextBox ID="lblWeeksUntilShip" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">12 Month Projected $:</label>
                    <div class="input-group"><span class="input-group-addon">$</span><asp:TextBox ID="lblAnnualProjectedDollars" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox></div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Annual Projected Retail Selling Units:</label>
                    <asp:TextBox ID="lblAnnualProjectedUnits" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4" id="dvCustomer">
                <div class="form-group">
                    <label class="control-label">Customer:</label>
                    <asp:TextBox ID="lblCustomer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Expected Gross Margin %:</label>
                    <asp:TextBox ID="lblExpectedGrossMargin" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Project Notes:</label>
                    <asp:TextBox ID="lblProjectNotes" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" TextMode="MultiLine" Rows="6"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Make/Pack for Finished Good</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Make Location:</label>
                    <asp:TextBox ID="lblManufacturingLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Finished Good Pack Location:</label>
                    <asp:TextBox ID="lblPrimaryPackingLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalHeading2" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2 class="fleft">Timeline</h2>
                <asp:Panel ID="TimelineInfo" CssClass="sectionCompletionInfo" runat="server"></asp:Panel>
            </div>
        </div>
        <div class="row RowBottomMargin" id="dvExternalSeperator2" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row" id="dvExternalTimeline" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Is Current Timeline Acceptable?</label>
                    <asp:TextBox ID="txtCurrentTimelineAcceptable" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalLeadTime" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Lead Time From Supplier:</label>
                    <asp:TextBox ID="txtLeadTimeFromSupplier" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalArtwork" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Final Artwork Due to Supplier:</label>
                    <asp:TextBox ID="txtFinalArtworkDueToSupplier" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Distribution</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Designate HUB DC (aka Material: Delivery Plant):</label>
                    <asp:TextBox ID="lblDesignateHUBDC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row" id="dvExternalHeading" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>External Manufacturing</h2>
            </div>
        </div>
        <div class="row RowBottomMargin" id="dvExternalSeparator" runat="server">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row" id="dvExternalFields" runat="server">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Co-Manufacturing Classification:</label>
                    <asp:TextBox ID="lblCoManufacturingClassification" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">External Manufacturer:</label>
                    <asp:TextBox ID="lblExternalManufacturer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">External Packer:</label>
                    <asp:TextBox ID="lblExternalPacker" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Initial Item Setup Form</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Unit UPC:</label>
                    <asp:TextBox ID="lblUnitUPC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Jar/Display UPC:</label>
                    <asp:TextBox ID="lblJarDisplayUPC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Case UCC:</label>
                    <asp:TextBox ID="lblCaseUCC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Pallet UCC:</label>
                    <asp:TextBox ID="lblPalletUCC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Finished Goods</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvUserControl" class="dvUserControl" runat="server">
        <asp:PlaceHolder ID="phMsg" runat="server" />
    </div>
    <asp:PlaceHolder ID="phPage" runat="server" />
    <div id="BOMPages" clientidmode="Static" runat="server">
        <asp:PlaceHolder ID="phBOM" runat="server" />
    </div>
    <asp:HiddenField ID="hdnPackagingComponent" runat="server" />
    <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnComponentStatusChangeIds" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hiddenItemId" runat="server" />
    <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
    <asp:HiddenField ID="hdnComponentype" runat="server" />
    <asp:HiddenField ID="hdnParentID" runat="server" />
    <asp:HiddenField ID="hdnPackagingID" runat="server" />
    <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
    <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
    <asp:HiddenField ID="hdnPackagingNumbers" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="hdnPLMProject" ClientIDMode="Static" runat="server" />
    <%--<div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Initial Reviews Approval</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Based on the input above, how would you like to continue?</label>
                <asp:TextBox ID="lblHowContinue" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>--%>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>2nd Review Check</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">Does all of the information above look correct?</label>
                <asp:DropDownList ID="ddlcorrect" CssClass="required form-control" ClientIDMode="Static" ToolTip="Does all of the information above look correct" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Key Information to Confirm:</label>
                <ul style="margin-left: -22px;">
                    <li>Pack Trial Results </li>
                    <li>BOM Correctness comparing SAP with PMT </li>
                    <li>All Necessary Pack Dimensions Present </li>
                    <li>Pallet Pattern Uploaded if Corrugate is being updated </li>
                    <li>Any New Material Components have Yes stated for New Graphics if they are required </li>
                    <li>All flow through drop downs are chosen when relevant </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <span class="markrequired">*</span><label class="control-label">SGS Expedited Workflow Approved?</label>
                <asp:DropDownList ID="ddlSGSExpeditedWorkflowApproved" CssClass="required form-control" ClientIDMode="Static" runat="server" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
                <p class="comment-block">Requires approval from SGS Team and Andrew Madaychik.</p>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Revised First Ship Date:</label>
                <asp:TextBox ID="txtFirstShipDate" runat="server" BorderStyle="None" CssClass="datePicker required form-control"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">Revised First Production Date:</label>
                <asp:TextBox ID="txtProductionDate" runat="server" BorderStyle="None" CssClass="datePicker form-control required"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-9 col-md-10">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="false" CssClass="ButtonControl" OnClick="btnSave_Click" />
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="return validatePage()" CssClass="ButtonControl" OnClick="btnSubmit_Click" />
        </div>
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('title').text("Pages - PM Second Review Form");

        var customer = $("#lblCustomer").val().toLocaleLowerCase().trim();
        if (customer.indexOf("select") == -1 && customer != "") {
            $("#dvCustomer").removeClass("hideItem");
        } else {
            $("#dvCustomer").addClass("hideItem");
        }
        if ($("#ddlcorrect option:selected").text().toLocaleLowerCase() == 'yes') {
            $('#cblSectionConcerns').parent().hide();
        } else {
            $('#cblSectionConcerns').parent().show();
        }
        pageLoadCheck();
        $("#ddlcorrect").change(function () {
            if ($("#ddlcorrect option:selected").text().toLocaleLowerCase() == 'yes') {
                $('#cblSectionConcerns').parent().hide();
            } else {
                $('#cblSectionConcerns').parent().show();
            }
        });
    });
</script>
