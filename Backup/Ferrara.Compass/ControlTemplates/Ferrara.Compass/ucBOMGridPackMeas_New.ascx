<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBOMGridPackMeas_New.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucBOMGridPackMeas_New" %>

<h3 class="accordion">
    <asp:Label ID="lblTitle" ClientIDMode="Static" CssClass="titlelbl" runat="server"></asp:Label>
    <asp:Label ID="lblDesc" ClientIDMode="Static" runat="server"></asp:Label>
</h3>
<div class="panel" style="background-color: #BCD3F2;">
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div id="dvUCBomPE" class="bompe" runat="server">
        <asp:HiddenField ID="hdnParentComponentType" ClientIDMode="Static" runat="server" Value='' />
        <asp:HiddenField ID="hdnParentPackagingType" ClientIDMode="Static" runat="server" Value='' />
        <asp:PlaceHolder ID="noResultsHolder" runat="server" />
        <table class="container-fluid ucBOMTable">
            <asp:Repeater ID="rptPackingItem" runat="server" OnItemCommand="rptPackingItem_ItemCommand" OnItemDataBound="rptPackingItem_ItemDataBound">
                <HeaderTemplate>
                    <tr>
                        <th class="BOMEdit">Select</th>
                        <asp:PlaceHolder ID="statusHeader" runat="server">
                            <th class="BOMStatus">Status</th>
                        </asp:PlaceHolder>
                        <th class="BOMNewExisting">New/Existing</th>
                        <th class="BOMMat">Component #</th>
                        <th class="BOMMatDesc">Component Description</th>
                        <th class="BOMLikeMat">Like Comp. #</th>
                        <th class="BOMOldMat">Old Comp. #</th>
                        <asp:PlaceHolder ID="phComponentTypeHeader" runat="server" Visible="true">
                            <th class="BOMComp">Component Type</th>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phMaterialGroupHeader" runat="server" Visible="false">
                            <th class="BOMMatGroup">SAP Material Group</th>
                        </asp:PlaceHolder>
                        <th class="BOMUofM">Pack Unit (UOM)</th>
                        <asp:PlaceHolder ID="phPrinterSupplierHeader" runat="server" Visible="false">
                            <th class="BOMPrinterSupplier">Printer / Supplier</th>
                        </asp:PlaceHolder>
                        <th class="BOMPackQty">Pack Qty.</th>
                        <asp:PlaceHolder ID="phCnsmrLblHeader" runat="server" Visible="false">
                            <th class="BOMMove">Cnsmr Lbl</th>
                        </asp:PlaceHolder>
                        <th class="BOMFlowthrough">Flowthrough</th>
                        <asp:PlaceHolder ID="phSupplierChgHeader" runat="server" Visible="false">
                            <th class="BOMMove">Supplier Chg</th>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phBOMMoveHeader" runat="server" Visible="false">
                            <th class="BOMMove">Move</th>
                        </asp:PlaceHolder>
                        <th class="BOMDelete">Delete</th>
                    </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr id="dvPackingItemRow" class="bomrow" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                        <td class="BOMEdit">
                            <asp:ImageButton ID="btnSelect" CausesValidation="false" Height="16" Width="16" CssClass="select" AlternateText="Select" OnClientClick="hideSelectButton('Select');" CommandName="LoadControl" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' ImageUrl="/_layouts/15/Ferrara.Compass/images/Edit.gif" runat="server" />
                            <asp:HiddenField ID="hdnPackagingComponent" ClientIDMode="Static" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>' />
                            <asp:HiddenField ID="hdnNewExisting" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "NewExisting") %>' />
                            <asp:HiddenField ID="hdnGraphicsRequired" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "GraphicsChangeRequired") %>' />
                            <asp:HiddenField ID="hdnParentID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "ParentID") %>' />
                            <asp:HiddenField ID="hdnItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            <asp:HiddenField ID="hdnId" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnCompassListItemId" Value='<%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnComponentType" Value='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>' runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnDeletedStatus" Value="false" runat="server" ClientIDMode="Static" />
                            <asp:HiddenField ID="hdnHasChildren" Value="false" runat="server" ClientIDMode="Static" />
                        </td>
                        <asp:PlaceHolder ID="statusImage" runat="server">
                            <td class="BOMStatus">
                                <asp:Image ID="imgStatus" runat="server" CssClass="red" BorderWidth="0" Height="25px" Width="25px" ImageUrl="/_layouts/15/Ferrara.Compass/img/redCircle.png" />
                            </td>
                        </asp:PlaceHolder>
                        <td class="BOMNewExisting">
                            <asp:TextBox ID="txtNewExisting" Enabled="false" ReadOnly="false" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "NewExisting") %>'></asp:TextBox>
                        </td>
                        <td class="BOMMat">
                            <asp:TextBox ID="txtMaterial" Enabled="false" ReadOnly="false" runat="server" class="minimumlength numericNoMask form-control Component numericDecimal3" MaxLength="6" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>'></asp:TextBox>
                        </td>
                        <td class="BOMMatDesc">
                            <asp:Label ID="lbMaterialDesc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>'></asp:Label>
                        </td>
                        <td class="BOMLikeMat">
                            <asp:TextBox ID="txtLikeMaterial" Enabled="false" ReadOnly="true" runat="server" class="readOnly form-control Component" MaxLength="6" Text='<%# DataBinder.Eval(Container.DataItem, "currentLikeItem") %>'></asp:TextBox>
                        </td>
                        <td class="BOMOldMat">
                            <asp:TextBox ID="txtOldMaterial" Enabled="false" ReadOnly="true" runat="server" class="readOnly form-control Component" MaxLength="6" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentOldItem") %>'></asp:TextBox>
                        </td>
                        <asp:PlaceHolder ID="phMaterialGroup" runat="server" Visible="false">
                            <td class="BOMMatGroup">
                                <asp:TextBox ID="txtSAPMatGroup" Enabled="false" ReadOnly="true" runat="server" class="readOnly form-control" Text=''></asp:TextBox>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phComponentType" runat="server" Visible="true">
                            <td class="BOMComp">
                                <asp:Label ID="lblPackagingComponent" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>'></asp:Label>
                            </td>
                        </asp:PlaceHolder>
                        <td class="BOMUofM">
                            <asp:DropDownList ID="drpUnitOfMeasure" runat="server" Enabled="false" CssClass="readOnly form-control">
                                <asp:ListItem Value="-1">Select...</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <asp:PlaceHolder ID="phPrinterSupplier" runat="server" Visible="false">
                            <td class="BOMPrinterSupplier">
                                <asp:Label ID="lblPrinterSupplier" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "PrinterSupplier") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem, "PrinterSupplier") %>'></asp:Label>
                            </td>
                        </asp:PlaceHolder>
                        <td class="BOMPackQty">
                            <asp:TextBox ID="txtPackQty" Enabled="false" ReadOnly="false" runat="server" class="readOnly numeric form-control numericDecimal3" Text='<%# DataBinder.Eval(Container.DataItem, "PackQuantity") %>'></asp:TextBox>
                        </td>
                        <asp:PlaceHolder ID="phCnsmrLbl" runat="server" Visible="false">
                            <td class="BOMCnsmrLbl">
                                <asp:TextBox ID="txtCnsmrLbl" Enabled="false" ReadOnly="false" runat="server" class="readOnly form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ComponentContainsNLEA") %>'></asp:TextBox>
                            </td>
                        </asp:PlaceHolder>
                        <td class="BOMFlowthrough">
                            <asp:TextBox ID="txtFlowthrough" Enabled="false" ReadOnly="false" runat="server" class="readOnly form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Flowthrough") %>'></asp:TextBox>
                        </td>
                        <asp:PlaceHolder ID="phSupplierChg" runat="server" Visible="false">
                            <td>
                                <asp:TextBox ID="txtSupplierChg" Enabled="false" ReadOnly="false" runat="server" class="readOnly form-control" Text='<%# DataBinder.Eval(Container.DataItem, "NewPrinterSupplierForLocation") %>'></asp:TextBox>
                            </td>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder ID="phBOMMove" runat="server" Visible="false">
                            <td class="BOMMove">
                                <asp:ImageButton ID="move" runat="server" CausesValidation="false" Height="16" Width="16" CssClass="select" AlternateText="Move" ImageUrl="/_layouts/15/Ferrara.Compass/img/moveArrow.png" />
                            </td>
                        </asp:PlaceHolder>
                        <td class="BOMDelete">
                            <asp:Image ID="btnDeleteAttachment" CssClass="deleteComponent" onClick="deleteComponent(this);return false;" AlternateText="Delete Attachment" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>


        <script type="text/javascript">
            $("#lnkMain").click();
        </script>

        <a id="lnkMain" href="#dvUCBomPE"></a>
    </div>
    <div class="OBMSetup miscOpsClass">
        <div class="row" id="dvAddFinishGoodItem" runat="server" visible="false">
            <div class="col-xs-12 col-sm-12 col-md-3">
                <asp:Button ID="btnAddNewPackagingItem" CssClass="ButtonControlAutoSize" Text="Add New Packaging Component" CausesValidation="false" runat="server" OnClick="btnAddNewPackagingItem_Click" OnClientClick="javascript:showWaitPopup('Adding Packaging Component...', 'Please be patient, this may take a few seconds...');" />
                <asp:Button ID="btnreloadPackagingItem" CssClass="finish ReloadBOM HiddenButton" runat="server" />
                <asp:HiddenField ID="hdnPackagingItemCount" ClientIDMode="Static" runat="server" />
            </div>
        </div>
        <div class="row" id="dvAddTransferSemi" runat="server" visible="false">
            <div class="col-xs-12 col-sm-12 col-md-3">
                <asp:Button ID="btnAddTransferSemi" CssClass="ButtonControlAutoSize" Text="Add Transfer Semi Component" CausesValidation="false" runat="server" OnClick="btnAddTransferSemi_Click" />
            </div>
        </div>
    </div>
</div>

<div class="dvPackNext" style="background-color: #BCD3F2;">
    <div class="hideItem">
        <asp:TextBox ID="hdnNewExistingComp" ClientIDMode="Static" CssClass="hdnNewExistingComp" runat="server" />
        <asp:TextBox ID="hdnNewComponentExists" ClientIDMode="Static" CssClass="hdnNewComponentExists" runat="server" />
        <asp:TextBox ID="hdnIsTransferSemi" ClientIDMode="Static" CssClass="hdnIsTransferSemi" runat="server" />
        <asp:TextBox ID="hdnMaterialGroup5PackType" ClientIDMode="Static" CssClass="hdnMaterialGroup5PackType" runat="server" />
    </div>
    <div id="dvElements" runat="server" class="miscOpsClass">
        <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnParentComponentId" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnUCBOMComponentType" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hdnUCSEMiBOMComponentType" ClientIDMode="Static" runat="server" />

        <asp:HiddenField ID="hdnid" runat="server" />
        <div id="dvTransferLocation" runat="server" visible="false">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Transfer Location</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Make/Pack & Transfer Location</label>
                        <asp:TextBox ID="lblTransferLocation" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control makePackLocation"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        <asp:PlaceHolder ID="specsSection" runat="server">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>
                        <asp:Label ID="lblSAPSpecChangeHeader" runat="server"></asp:Label>
                        Specifications</h2>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="form-group">
                        <label class="control-label">
                            Is this a new
                        <asp:Label ID="lblSAPSpecChange" runat="server"></asp:Label>
                            or is it an existing
                        <asp:Label ID="lblSAPSpecChange2" runat="server"></asp:Label>
                            with a change to its specification?</label>
                        <asp:DropDownList ID="drpSAPSpecsChange" onchange="SpecsChange(this);" runat="server" CssClass="form-control drpNewExistingSpec pe2" Width="125px">
                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="form-group ">
                        <label class="control-label">If this is an existing specification with changes that don't constitute a new material number, please describe those changes:</label>
                        <asp:TextBox ID="txtSpecNotes" ClientIDMode="Static" TextMode="MultiLine" Rows="6" runat="server" CssClass=" SpecNotes form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group ">
                        <label class="control-label">
                            <asp:Label ID="lblFGPackSpecNumber" runat="server"></asp:Label>
                            Finished Good (Pack only) Specification Number:</label>
                        <asp:TextBox ID="txtFGPackSpecNumber" CssClass="txtFGPackSpecNumber form-control pe3" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group ">
                        <label class="control-label">
                            <asp:Label ID="lblPalletSpecNumber" runat="server"></asp:Label>
                            Pallet Specification Number:</label>
                        <asp:TextBox ID="txtPalletSpecNumber" CssClass="form-control pe2" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-9 col-md-8">
                    <div class="form-group ">
                        <label class="control-label">
                            <asp:Label ID="lblPalletPatternLink" runat="server"></asp:Label>
                            Pallet Pattern Specification Link:
                    <asp:HyperLink ID="generatedLink" CssClass="hideItem" ClientIDMode="Static" Target="_blank" NavigateUrl="" Text="" runat="server"></asp:HyperLink></label>
                        <asp:TextBox ID="txPalletPatternLink" ClientIDMode="Static" CssClass="form-control pe2" runat="server"></asp:TextBox>
                        <asp:Label ID="lblURLError" ClientIDMode="Static" runat="server" CssClass="Error"></asp:Label>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-9 col-md-4" id="divGenerateLink" runat="server">
                    <br />
                    <asp:Button ID="btnGenerateLink" type="button" runat="server" CssClass="ButtonControlAutoSize MarginSevenTop" Text="Generate New Link" OnClientClick="generateLinkPackMeas(this); return false;" />
                </div>
            </div>
        </asp:PlaceHolder>
    </div>
</div>
