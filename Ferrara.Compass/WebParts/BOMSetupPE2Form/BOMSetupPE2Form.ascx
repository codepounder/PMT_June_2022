<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BOMSetupPE2Form.ascx.cs" Inherits="Ferrara.Compass.WebParts.BOMSetupPE2Form.BOMSetupPE2Form" %>

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
            <h1>Bill of Materials Creation Form - PE2</h1>
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
            <asp:HiddenField ID="hdnProjectTypeSubCategory" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hdnDeletedCompIds" runat="server" ClientIDMode="Static" />
        </div>
    </div>
    <!-- Project Information and Project Team -->
    <div id="ProjectInformation">
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
                    <asp:TextBox ID="lblProjectSubcategory" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Finished Good Pack Location:</label>
                    <asp:TextBox ID="lblPackLocation" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Line/Workcenter Additional Info:</label>
                    <asp:TextBox ID="txtWorkCenterAddInfo" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
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
                    <label class="control-label">Item Concept:</label>
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
                    <asp:TextBox ID="lblInitiatorName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Marketing:</label>
                    <asp:TextBox ID="lblMarketingName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">PM:</label>
                    <asp:TextBox ID="lblPMName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">InTech Manager:</label>
                    <asp:TextBox ID="lblInTechManagerName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Packaging Engineer:</label>
                    <asp:TextBox ID="lblPackagingEngineerName" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
            </div>
        </div>
        <div id="divLogisticsInformation" runat="server" visible="false">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Logistics Information</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Finished Good Make Location:</label>
                        <asp:TextBox ID="txtMakeLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Finished Good Pack Location:</label>
                        <asp:TextBox ID="txtPackLocation1" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4" id="divProcurementType" runat="server">
                    <div class="form-group">
                        <label class="control-label">Procurement Type:</label>
                        <asp:TextBox ID="txtProcurementType" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-4" id="divExternalManufacturer" runat="server">
                    <div class="form-group">
                        <label class="control-label">External Manufacturer:</label>
                        <asp:TextBox ID="txtExternalManufacturer" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4" id="dvPackLocation" runat="server">
                    <div class="form-group">
                        <label class="control-label">External Packer:</label>
                        <asp:TextBox ID="txtExternalPacker" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4" id="divPurchaseIntoLocation" runat="server">
                    <div class="form-group">
                        <label class="control-label">Purchase Into Location:</label>
                        <asp:TextBox ID="txtPurchaseIntoLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-4" id="divSAPBaseUOM" runat="server">
                    <div class="form-group">
                        <label class="control-label">SAP Base UOM:</label>
                        <asp:TextBox ID="txtSAPBaseUOM" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Designate HUB DC (aka Material: Delivery Plant):</label>
                        <asp:TextBox ID="txtDesignateHUBDC" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
    </div>
    <asp:PlaceHolder ID="phBOMGrid" runat="server" />
    <div id="dvUserControl" class="dvUserControl" runat="server">
        <asp:PlaceHolder ID="phBOMEdits" runat="server" />
    </div>
    <div class="ApprovedGraphicsAsset GraphicsOnlyPE2">
        <div class="row">
            <div class="col-xs-12 col-sm-3 col-md-4">
                <label class="control-label">Approved Graphic Asset for New Components:</label>
            </div>
            <%-- <div class="col-xs-12 col-sm-9 col-md-4">
                <input id="btnApprovedGraphicsAsset" type="button" class="ButtonControlAutoSize" value="Upload Approved Graphic Asset" onclick="openBasicDialog('Upload Approved Graphic Asset for New Components', 'ApprovedGraphicsAsset'); return false;" />
            </div>--%>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <table style="width: 100%;">
                    <asp:Repeater ID="rptApprovedGraphicsAsset" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>
                                <th>Document Name</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkDeleteApprovedGraphicsAsset" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteApprovedGraphicsAsset_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                                <td>
                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="ExternalComponentDieline GraphicsOnlyPE2">
        <div class="row">
            <div class="col-xs-12 col-sm-3 col-md-4">
                <label class="control-label">External Component Dieline - New Component:</label>
            </div>
            <%-- <div class="col-xs-12 col-sm-9 col-md-4">
                <input id="btnApprovedGraphicsAsset" type="button" class="ButtonControlAutoSize" value="Upload Approved Graphic Asset" onclick="openBasicDialog('Upload Approved Graphic Asset for New Components', 'ApprovedGraphicsAsset'); return false;" />
            </div>--%>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <table style="width: 100%;">
                    <asp:Repeater ID="rptExternalComponentDieline" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>
                                <th>Document Name</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkDeleteExternalComponentDieline" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteExternalComponentDieline_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                                <td>
                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="row GraphicsOnlyPE2">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">All Aspects approved from PE Perspective?:</label>
                <asp:DropDownList ID="ddlAllAspectsApprovedFromPEPersp" onchange="AllAspectsApprovedFromPEPerspChanged(this);" ClientIDMode="Static" runat="server" ViewStateMode="Enabled" CssClass="form-control">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6 WhatIsIncorrectPE">
            <div class="form-group">
                <label class="control-label">What is incorrect?:</label>
                <asp:TextBox ID="txtWhatIsIncorrectPE" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" Text='' ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
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
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnComponentype" runat="server" />
            <asp:HiddenField ID="hdnParentID" runat="server" />
            <asp:HiddenField ID="hdnPackagingID" runat="server" />
            <asp:HiddenField ID="hdnMaterialNumber" runat="server" />
            <asp:HiddenField ID="hdnMaterialDesc" runat="server" />
            <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnComponentStatusChangeIds" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hdnPackagingComponent" ClientIDMode="Static" runat="server" />
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
        pageLoadCheck();
    });
</script>
