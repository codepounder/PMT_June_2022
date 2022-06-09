<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GraphicsRequestForm_New.ascx.cs" Inherits="Ferrara.Compass.WebParts.GraphicsRequestForm_New.GraphicsRequestForm_New" %>

<div class="container">
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
            <h1>Graphics Request</h1>
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
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Project Information</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-12">
            <div class="form-group">
                <asp:Label ID="lblItemConcept" CssClass="control-label" Text="Item Concept:" runat="server" Style="display: inline-block; max-width: 100%; margin-bottom: 5px; font-weight: 700;"></asp:Label>
                <asp:TextBox ID="txtItemConcept" ClientIDMode="Static" TextMode="MultiLine" Rows="3" runat="server" ReadOnly="true" CssClass="form-control"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">Material Group 1 (Brand):</label>
            <asp:TextBox ID="txtBrand" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">PM:</label>
            <asp:TextBox ID="txtOBM" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true"></asp:TextBox>
        </div>
        <div class="col-xs-12 col-sm-4 col-md-4">
            <label class="control-label">Season:</label>
            <asp:TextBox ID="txtSeason" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true"></asp:TextBox>
        </div>
    </div>
    <br />
    <br />
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <table class="gridTable" style="width: 100%">
                <asp:Repeater ID="rptSummary" runat="server" OnItemDataBound="rptSummary_ItemDataBound">
                    <HeaderTemplate>
                        <tr>
                            <th class="gridCellH">Material Number</th>
                            <th class="gridCellH">Packaging Component</th>
                            <th class="gridCellH">Graphics Vendor</th>
                            <th class="gridCellH">Approved Die Line(s)</th>
                            <th class="gridCellH">Printer Name</th>
                            <th class="gridCellH">Notes</th>
                            <th class="gridCellH">Link</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                            <td class="gridCell"><%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %></td>
                            <td class="gridCell"><%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %></td>
                            <td class="gridCell"><%# DataBinder.Eval(Container.DataItem, "ExternalGraphicsVendor") %></td>
                            <td class="gridCell">
                                <a id="anHyperlinktoDielineInPLMSummary" clientidmode="Static" runat="server" target="_blank" href=""></a>
                            </td>
                            <td class="gridCell"><%# DataBinder.Eval(Container.DataItem, "PrinterSupplier") %></td>
                            <td class="gridCell"><%# DataBinder.Eval(Container.DataItem, "Notes") %></td>
                            <td class="gridCell">
                                <asp:HyperLink ID="ancGR" runat="server">Graphics Request</asp:HyperLink>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Graphics Matrix</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvGraphicsMatrixRepeater" class="row repeater" runat="server">
        <asp:Label ID="lblNoGraphics" runat="server" Text="No Graphics Items Found!" Visible="false" CssClass="control-label"></asp:Label>
        <asp:Repeater ID="rptGraphics" runat="server" OnItemDataBound="rptGraphics_ItemDataBound" OnItemCommand="rptGraphics_ItemCommand">
            <ItemTemplate>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label">Packaging Component:</label>
                            <asp:TextBox ID="lblPackagingComponent" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>'></asp:TextBox>
                            <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label">New or Existing?:</label>
                            <asp:TextBox ID="lblNewExisting" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "NewExisting") %>'></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label">Printer Name:</label>
                            <asp:TextBox ID="lblPrinterName" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "PrinterSupplier") %>'></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-3">
                        <div class="form-group">
                            <label class="control-label">Graphics Vendor:</label>
                            <asp:TextBox ID="lblGraphicsVendor" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "ExternalGraphicsVendor") %>'></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div class="col-xs-12 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label class="control-label">Material #:</label>
                            <asp:TextBox ID="lblMaterialNumber" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>'></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-8 col-md-9">
                        <div class="form-group">
                            <label class="control-label">Material Description:</label>
                            <asp:TextBox ID="lblMaterialDescription" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>'></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div id="dvLikeMaterial" runat="server" class="col-xs-12 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label class="control-label">Like Material #:</label>
                            <asp:TextBox ID="lblLikeMaterialNumber" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItem") %>'></asp:TextBox>
                        </div>
                    </div>
                    <div id="dvLikeMaterialDesc" runat="server" class="col-xs-12 col-sm-8 col-md-9">
                        <div class="form-group">
                            <label class="control-label">Like Material Description:</label>
                            <asp:TextBox ID="lblLikeMaterialDescription" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItemDescription") %>'></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div id="dvOldMaterial" runat="server" class="col-xs-12 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label class="control-label">Old Material #:</label>
                            <asp:TextBox ID="lblOldMaterialNumber" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentOldItem") %>'></asp:TextBox>
                        </div>
                    </div>
                    <div id="dvOldMaterialDesc" runat="server" class="col-xs-12 col-sm-8 col-md-9">
                        <div class="form-group">
                            <label class="control-label">Old Material Description:</label>
                            <asp:TextBox ID="lblOldMaterialDescription" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentOldItemDescription") %>'></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div id="dvWhyLikeMaterial" runat="server" class="col-xs-12 col-sm-12 col-md-12">
                        <div class="form-group">
                            <label class="control-label">Why is it a Like Material#?:</label>
                            <asp:TextBox ID="lblWhyLikeMaterial" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItemReason") %>'></asp:TextBox>
                            <label id="lblItemNote" class="comment-block">Please specify if the artwork and the dieline will be the same or not.</label>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div class="col-xs-12 col-sm-6 col-md-12" runat="server" id="div14DigitBarcode" visible="false">
                        <div class="form-group">
                            <label class="control-label">14 Digit Barcode:</label>
                            <asp:TextBox ID="txt14DigitBarcode" runat="server" CssClass="form-control" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="form-group">
                            <label class="control-label">Graphics Brief:</label>
                            <asp:TextBox ID="txtGraphicsBrief" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "GraphicsBrief") %>'></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="form-group">
                            <label class="control-label">Graphics Request:</label>
                            <div>
                                <a target="_blank" id="ancGraphicsRequest" runat="server"></a>
                            </div>
                            <div>
                                <a target="_blank" id="ancGraphicsRequest2" runat="server"></a>
                            </div>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="form-group">
                            <label class="control-label">Visual Reference/Rendering:</label>
                            <div id="divUploadRendering" runat="server">
                                <a id="ancUploadRendering" href="#" title="Upload" onclick="openBasicDialog('Upload Rendering', 'Rendering', <%# DataBinder.Eval(Container.DataItem, "Id") %>);return false;">
                                    <img src="/_layouts/15/Ferrara.Compass/images/Attachtb.gif" id="btnAttachRendering" runat="server" alt="Attach Rendering" /></a>
                            </div>
                            <asp:ImageButton ID="btnDeleteRendering" CausesValidation="false" AlternateText="Delete Attachment" Visible="false" CommandName="DeleteRendering" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" />
                            <a target="_blank" id="ancRendering" runat="server"></a>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="form-group">
                            <label class="control-label">Packaging Specification Number:</label>
                            <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "SpecificationNo") %>' BorderStyle="None" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <div class="col-xs-12 col-sm-6 col-md-4">
                        <div class="form-group">
                            <label class="control-label">Hyperlink to Dieline in PLM :</label>
                            <div>
                                <strong><a id="anHyperlinktoDielineInPLM" clientidmode="Static" runat="server" target="_blank" href=""></a></strong>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row" style="margin-top: 0px; <%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                    <asp:Label ID="lblNoDielineAttachments" runat="server" Text="No Dieline Attachment Found!" Visible="false" CssClass="control-label"></asp:Label>
                    <asp:Repeater ID="rpDielineAttachments" OnItemDataBound="rpDielineAttachments_ItemDataBound" runat="server">
                        <HeaderTemplate>
                            <div class="row" style="margin-top: 0px;">
                                <div class="col-xs-12 col-sm-6 col-md-10">
                                    <div class="form-group" style="margin-bottom: 0px;">
                                        <label class="control-label">Dieline Attachment(s) :</label>
                                    </div>
                                </div>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="row" style="margin-top: 0px;">
                                <div class="col-xs-12 col-sm-6 col-md-10">
                                    <div class="form-group" style="margin-bottom: 0px;">
                                        <asp:HyperLink ID="ancDielineAttachment" ClientIDMode="Static" Target="_blank" NavigateUrl="" Text="" runat="server"></asp:HyperLink></label>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Project Attachments</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-6 col-md-6 CompassLabel">
            NLEA:
        </div>
        <%--<div class="col-xs-12 col-sm-6 col-md-6 CompassLabel">
            Pallet Pattern:
        </div>--%>
    </div>
    <div class="row" style="margin-top: 35px; margin-left: 10px;">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Label ID="lblNoNLEA" runat="server" Text="No NLEA Attachments Found!" Visible="false" CssClass="control-label"></asp:Label>
            <table>
                <asp:Repeater ID="rpNLEA" runat="server">
                    <HeaderTemplate>
                        <tr>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-6 col-md-6 CompassLabel">
            Pallet Specifications:
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Label ID="lblNoPalletSpecifications" runat="server" Text="No Pallet Specification Attachments Found!" Visible="false" CssClass="control-label"></asp:Label>
            <table class="gridTable" style="width: 50%">
                <asp:Repeater ID="rpPalletSpecifications" runat="server" OnItemDataBound="rpPalletSpecifications_ItemDataBound">
                    <HeaderTemplate>
                        <tr>
                            <th class="gridCellH">Pallet Specification Hyperlink</th>
                            <th class="gridCellH">Pallet Specification Number</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                            <td class="gridCell">
                                <asp:HyperLink ID="generatedLink" CssClass="hideItem" ClientIDMode="Static" Target="_blank" NavigateUrl="" Text="" runat="server"></asp:HyperLink></label>
                            </td>
                            <td class="gridCell"><%# DataBinder.Eval(Container.DataItem, "PalletSpecNumber") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnGeneratePDFs" runat="server" Text="Generate PDFs" OnClick="btnGeneratePDFs_Click" CssClass="ButtonControlAutoSize" />
        </div>
        <div class="col-xs-12 col-sm-4 col-md-4">
            <asp:Label ID="lblPdfGenerated" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-5 col-md-5">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-1">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CausesValidation="false" CssClass="ButtonControl" />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Reload Attachment" CssClass="ReloadAttachment" OnClick="btnReloadAttachment_Click" />
            <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
            <asp:HiddenField ID="hiddenWorkflowStatus" runat="server" />
            <asp:HiddenField ID="hdnSAPItemNumber" runat="server" />
        </div>
    </div>
</div>
