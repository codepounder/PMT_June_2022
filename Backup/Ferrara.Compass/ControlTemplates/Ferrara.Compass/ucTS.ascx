<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucTS.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucTS" %>

<!-- Repeated data -->
<asp:Panel ID="TSButton" CssClass="tsButton" runat="server"></asp:Panel>
<asp:Panel ID="TSSection" CssClass="TSSection" runat="server">
    <asp:Label runat="server" ID="lblTSNote" class="fgBOMNotes"></asp:Label>
    <table class="container-fluid ucTSTable">
        <asp:Repeater ID="rptTSItem" runat="server" OnItemDataBound="rptTSItem_ItemDataBound">
            <ItemTemplate>
                <asp:Panel runat="server" CssClass="bomrow SAPVerifyItem">
                    <asp:HiddenField ID="hdnParentId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnDeletedStatus" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnComponentType" runat="server" ClientIDMode="Static" />
                    <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>Component Type:</label>
                                <asp:DropDownList ID="drpComponent" runat="server" CssClass="PCBOMrequired form-control drpComponentType VerifySAPNumbersType" onchange="drpCompType_changed(this);">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>' />
                                <asp:HiddenField ID="hdnItemID" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>New or Existing?:</label>
                                <asp:DropDownList ID="drpNew" onchange="drpNew_changed(this.id, true);" runat="server" CssClass="PCBOMrequired drpNewClass form-control VerifySAPNewExisting">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                    <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                                    <asp:ListItem Text="Network Move" Value="Network Move"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>Base UOM Qty:</label>
                                <asp:TextBox ID="txtPackQty" runat="server" CssClass="PCBOMrequired numericDecimal3 form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span class="markrequired">*</span>UOM:</label>
                                <asp:DropDownList ID="drpUnitOfMeasure" runat="server" CssClass="PCBOMrequired form-control">
                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="lblCompNote" runat="server" Visible="false">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <label class="comment-block">This component has been disabled because it has children, please delete or move all child components <strong>and save</strong> before changing the component type.</label>
                        </div>
                    </asp:Panel>
                    <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span id="spanFlowthrough" class="markrequired spanFlowthrough">*</span>Flowthrough:</label>
                                <asp:DropDownList ID="ddlFlowthrough" runat="server" onchange="FlowthroughCheck(this);" CssClass="PCBOMrequired flowthroughClass form-control">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row hideableRow" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-4 col-md-4">
                            <div class="form-group">
                                <label class="control-label"><span id="spanGraphicsNeeded" runat="server" class="markrequired spanGraphicsNeeded">*</span>New Graphics Required?:</label>
                                <asp:DropDownList ID="drpGraphicsNeeded" runat="server" onchange="GraphicsCheck(this);" CssClass="PCBOMrequired form-control drpGraphics">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4">
                            <div class="form-group">
                                <label class="control-label"><span id="spanComponentContainsNLEA" runat="server" class="markrequired spanGraphicsNLEA">*</span>Component requires consumer facing labeling?:</label>
                                <asp:DropDownList ID="drpComponentContainsNLEA" runat="server" CssClass="PCBOMrequired form-control drpGraphicsNLEA">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:DropDownList>
                                <label class="comment-block" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"color:#777777;\"" %>>Yes would mean this packaging material would contain nutritionals, a UPC, BE QR code, etc to enable sale to a consumer.</label>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4">
                            <div class="form-group">
                                <label class="control-label"><span id="spanGraphicsVendor" runat="server" class="markrequired spanGraphicsVendor">*</span>Graphics Vendor:</label>
                                <asp:DropDownList ID="ddlGraphicsVendor" runat="server" CssClass="PCBOMrequired form-control drpGraphicsVendor">
                                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span id="spanComponent" class="markrequired spanComponent">*</span>Component #:</label>
                                <asp:TextBox ID="txtMaterial" runat="server" CssClass="PCBOMrequired alphanumericToUpper1 minimumlength form-control Component NumberClass VerifySAPNumbers" MaxLength="20" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>'></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-2">
                            <div id="dvCompFindButton" class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnLookupCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getRPTCompDescription(this);return false;" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' />
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-7">
                            <div class="form-group">
                                <label class="control-label"><span id="spanComponentDesc" class="markrequired spanComponentDesc">*</span>Component Description:</label>
                                <asp:TextBox ID="txtMaterialDesc" runat="server" Style="text-transform: uppercase" CssClass="PCBOMrequired form-control ComponentDesc DescriptionClass" MaxLength="40" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>'></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span id="spanLikeComponent" class="markrequired spanLikeComponent">*</span>Like Component #:</label>
                                <asp:TextBox ID="txtLikeMaterial" runat="server" CssClass="PCBOMrequired alphanumericToUpper1 minimumlength form-control LikeMaterial NumberClass" MaxLength="20" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItem") %>'></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-2">
                            <div id="dvLikeCompFindButton" class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnLookupLikeCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getRPTCompDescription(this);return false;" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItem") %>' />
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-7">
                            <div class="form-group">
                                <label class="control-label"><span id="spanLikeComponentDesc" class="markrequired spanLikeComponentDesc">*</span>Like Component Description:</label>
                                <asp:TextBox ID="txtLikeMaterialDesc" runat="server" CssClass="PCBOMrequired form-control LikeDescription DescriptionClass" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItemDescription") %>'></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="form-group">
                                <label class="control-label"><span id="spanOldMaterial" class="markrequired spanOldMaterial hideItem">*</span>Old Component #:</label>
                                <asp:TextBox ID="txtOldMaterial" runat="server" CssClass="alphanumericToUpper1 minimumlength NumberClass OldMaterial form-control" MaxLength="20" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentOldItem") %>'></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-2">
                            <div id="dvOldCompFindButton" class="form-group">
                                <label class="control-label">&nbsp;</label>
                                <asp:Button ID="btnLookupOldCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getRPTCompDescription(this);return false;" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "CurrentOldItem") %>' />
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-7">
                            <div class="form-group">
                                <label class="control-label">Old Component Description:</label>
                                <asp:TextBox ID="txtOldMaterialDesc" runat="server" CssClass="form-control OldDescription DescriptionClass" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentOldItemDescription") %>'></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %> class="hierarchyPanel2">
                        <%--<asp:UpdatePanel ID="hierarchyPanel2" ClientIDMode="Static" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlPHL1" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlPHL2" EventName="SelectedIndexChanged" />
                                <asp:AsyncPostBackTrigger ControlID="ddlBrand" EventName="SelectedIndexChanged" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:UpdateProgress ID="UpdateProgress2" runat="server">
                                    <ProgressTemplate>
                                        <div id="reloadingCover" clientidmode="Static" runat="server" style="height: 165px; position: absolute; width: 98%; background-color: #AAAAAA; z-index: 100; opacity: 20%;">
                                            <div class="disablingLoadingIcon" id="disablingLoadingHierarchy" style="top: 45%; width: 84px; height: 35px; left: 45%; background-color: #AAAAAA !important;">&nbsp;</div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                        <div class="row TSOnlyRow hideItem new">
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 1:</label>
                                    <%--<asp:DropDownList ID="ddlPHL1" CssClass="PCBOMrequired PHL1 form-control" OnSelectedIndexChanged="ddlProductHierarchyLevel1_SelectedIndexChanged" runat="server" AppendDataBoundItems="true" AutoPostBack="True">--%>
                                    <asp:DropDownList ID="ddlPHL1" CssClass="PCBOMrequired PHL1 form-control" onchange="BindPHL2DropDownItemsByPHL1(this);" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <p class="comment-block">
                                        <asp:Label ID="lblPHL1" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Product Hierarchy Level 2:</label>
                                    <%--<asp:DropDownList ID="ddlPHL2" OnSelectedIndexChanged="ddlProductHierarchyLevel2_SelectedIndexChanged" AutoPostBack="True" CssClass="PCBOMrequired form-control" runat="server" AppendDataBoundItems="true">--%>
                                    <asp:DropDownList ID="ddlPHL2" runat="server" CssClass="PCBOMrequired form-control PHL2" onchange="BindBrandDropDownItemsByPHL2(this);" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <p class="comment-block">
                                        <asp:Label ID="lblPHL2" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-4">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Material Group 1 (Brand):</label>
                                    <%--<asp:DropDownList ID="ddlBrand" OnSelectedIndexChanged="ddlBrand_SelectedIndexChanged" AutoPostBack="True" CssClass="PCBOMrequired form-control" runat="server" AppendDataBoundItems="true">--%>
                                    <asp:DropDownList ID="ddlBrand" CssClass="PCBOMrequired form-control Brand" onchange="GetProfitCenter(this);" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                    <p class="comment-block">
                                        <asp:Label ID="lblBrand" CssClass="comment-block" runat="server"></asp:Label>
                                    </p>
                                </div>
                            </div>
                        </div>
                        <div class="row TSOnlyRow hideItem new">
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Profit Center:</label>
                                    <asp:TextBox ID="txtProfitCenterUC" runat="server" ReadOnly="true" class="PCBOMrequired form-control" ClientIDMode="Static"></asp:TextBox>
                                    <asp:HiddenField ID="hdnProfitCenterUC" runat="server" ClientIDMode="Static" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row hideableRow" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-6 col-md-6">
                            <div class="form-group">
                                <label class="control-label"><span id="spanWhyComponent" runat="server" class="markrequired spanWhyComponent">*</span>How is it a Like Component #:</label>
                                <asp:TextBox ID="txtLikeReason" TextMode="MultiLine" ToolTip="Does like component # have same dieline, graphics, etc." Rows="3" runat="server" CssClass="form-control whyLikeComponent" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItemReason") %>'></asp:TextBox>
                                <label id="lblItemNote" class="comment-block" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"color:#777777;\"" %>>Does like component # have same dieline, graphics, etc.</label>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-6">
                            <div class="form-group">
                                <label class="control-label"><span id="spanGraphicsBrief" runat="server" class="markrequired spanGraphicsBrief">*</span>Graphics Brief:</label>
                                <asp:TextBox ID="txtGraphicsBrief" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control GraphicsBrief" Text='<%# DataBinder.Eval(Container.DataItem, "GraphicsBrief") %>'></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row TSOnlyRow hideItem" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <div class="form-group">
                                <label class="control-label">Transfer/Purchase Candy Semi Comments:</label>
                                <asp:TextBox ID="txtTSComments" TextMode="MultiLine" ToolTip="Transfer/Purchase Candy Semi Comments" Rows="3" runat="server" CssClass="form-control TSComments" Text='<%# DataBinder.Eval(Container.DataItem, "Notes") %>'></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="<%# Container.ItemIndex % 2 == 0 ? "": "background-color:#BCD3F2;" %>">
                        <div class="col-xs-12 col-sm-6 col-md-6">
                            <div class="form-group">
                                <label class="control-label">Move this component:</label>
                                <asp:DropDownList ID="ddlMoveTS" runat="server" CssClass="ddlMoveTS form-control">
                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="row hideableRow" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-12 col-md-4">
                            <label class="control-label">Visual Reference / Rendering:</label>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-5">
                            <form role="form" class="form-inline">
                                <div id="divAttachFile" runat="server">
                                    <a id="ancAttachFile" href="#" title="edit" onclick="openBasicDialogIPF('/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=<%# DataBinder.Eval(Container.DataItem, "Id") %>&CompassItemId=<%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>&DocType=Rendering', 'Upload Rendering');return false;">
                                        <img src="/_layouts/15/Ferrara.Compass/images/Attachtb.gif" id="btnAttachment" runat="server" alt="Attach File" /></a>

                                </div>
                                <asp:ImageButton ID="btnDeleteAttachment" CausesValidation="false" AlternateText="Delete Attachment" Visible="false" CommandName="DeleteAtt" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" OnClientClick="DeleteVisualreference(this);return false;" runat="server" />
                                <a target="_blank" id="ancRendering" runat="server"></a>
                                <asp:HiddenField ID="DeletedVisualreferenceUrl" ClientIDMode="Static" runat="server" />
                            </form>
                        </div>
                    </div>
                    <div class="row ApprovedGraphicsAsset" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-12 col-md-4">
                            <label class="control-label lblApprovedGraphicsAsset" id="lblApprovedGraphicsAsset" runat="server"><span class="markrequired spanGraphicsBrief">*</span>Approved Graphics Asset:</label>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-5">
                            <form role="form" class="form-inline">
                                <div id="divAttachApprovedGraphicsAsset" runat="server">
                                    <a id="ancAttachApprovedGraphicsAsset" href="#" title="edit" onclick="openBasicDialogIPF('/_layouts/15/Ferrara.Compass/AppPages/UploadForm.aspx?PackagingItemId=<%# DataBinder.Eval(Container.DataItem, "Id") %>&CompassItemId=<%# DataBinder.Eval(Container.DataItem, "CompassListItemId") %>&DocType=ApprovedGraphicsAsset', 'Upload Approved Graphics Asset');return false;">
                                        <img src="/_layouts/15/Ferrara.Compass/images/Attachtb.gif" id="btnApprovedGraphicsAsset" runat="server" alt="Attach File" /></a>

                                </div>
                                <asp:ImageButton ID="btnDeleteApprovedGraphicsAsset" CausesValidation="false" AlternateText="Delete Attachment" Visible="false" CommandName="DeleteApprovedGraphicsAsset" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" OnClientClick="DeleteApprovedGraphicsAsset(this);return false;" runat="server" />
                                <a target="_blank" id="ancApprovedGraphicsAsset" class="ancApprovedGraphicsAsset" runat="server"></a>
                                <asp:HiddenField ID="DeletedApprovedGraphicsAssetUrl" ClientIDMode="Static" runat="server" />
                            </form>
                        </div>
                    </div>
                    <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                        <div class="col-xs-12 col-sm-12 col-md-9">&nbsp;</div>
                        <div class="col-xs-12 col-sm-12 col-md-3">
                            <asp:Button ID="btndelete" CausesValidation="false" CssClass="ButtonControlAutoSize" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" Text="Delete Component" OnClientClick="deletePackagingItem(this);return false;" />
                        </div>
                    </div>
                </asp:Panel>
            </ItemTemplate>
        </asp:Repeater>
    </table>

    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-3">
            <asp:Button ID="btnAddNewPackagingItem" CssClass="ButtonControlAutoSize" Text="Add New Component #" OnClick="btnAddNewPackagingItem_Click" OnClientClick="$('#hdnUCLoaded').val('false');" runat="server" />
        </div>
    </div>
</asp:Panel>
