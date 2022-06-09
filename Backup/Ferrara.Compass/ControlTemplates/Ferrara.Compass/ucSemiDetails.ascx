<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSemiDetails.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucSemiDetails" %>

<!-- Repeated data -->
<asp:Panel ID="semiSection" CssClass="semiSection" runat="server">
    <div id="semiRepeater" class="RowBottomMargin repeater">
        <div class="form-group">
            <div id="FGError_Message"></div>
            <asp:Repeater ID="rptSemis" runat="server" OnItemCommand="rptSemis_ItemCommand" OnItemDataBound="rptSemis_ItemDataBound">
                <HeaderTemplate></HeaderTemplate>
                <ItemTemplate>
                    <div class="semiRow SAPVerifyItem">
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-5">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Component Type</label>
                                    <asp:DropDownList ID="drpComponentType" CssClass="form-control BOMrequired VerifySAPNumbersType" title="Component Type" ToolTip="Component Type" runat="server">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        <asp:ListItem Text="Transfer Semi" Value="Transfer Semi"></asp:ListItem>
                                        <asp:ListItem Text="Purchased Candy Semi" Value="Purchased Candy Semi"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnComponentType" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "PackagingComponent") %>' />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-5">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired TSRequiredMark">*</span>New or Existing?</label>
                                    <asp:DropDownList ID="drpNewExisting" CssClass="form-control BOMrequired TSRequired NewExisting VerifySAPNewExisting" onchange="PCSRequirements();" title="New/Existing" ToolTip="New or Existing?" runat="server">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                        <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                        <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                                        <asp:ListItem Text="Network Move" Value="Network Move"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hdnItemID" runat="server" ClientIDMode="Static" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-2">
                                <div class="form-group">
                                    <label class="control-label">Action</label>
                                    <asp:Button ID="btndelete" CausesValidation="false" CssClass="button" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' runat="server" Text="Delete" />
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="lblCompDeleteError" runat="server" Visible="false">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <label class="comment-block noResultsRow">Cannot delete this components as it has child elements.</label>
                            </div>
                        </asp:Panel>

                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label"><span id="spanComponent" class="markrequired spanComponent TSRequiredMark">*</span><%=ComponentType %> #:</label>
                                    <asp:TextBox ID="txtMaterialNumber" runat="server" CssClass="BOMrequired alphanumericToUpper1 minimumlength TSRequired form-control Component NumberClass VerifySAPNumbers" MaxLength="20" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialNumber") %>' ToolTip="Material Number" ClientIDMode="Static"></asp:TextBox>
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
                                    <label class="control-label"><span id="spanComponentDesc" class="markrequired spanComponentDesc descriptionmark TSRequiredMark">*</span><%=ComponentType %> Description:</label>
                                    <asp:TextBox ID="txtMaterialDescription" runat="server" Style="text-transform: uppercase" CssClass="BOMrequired TSRequired form-control ComponentDesc DescriptionClass" MaxLength="40" Text='<%# DataBinder.Eval(Container.DataItem, "MaterialDescription") %>' ToolTip="Material Description"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-3">
                                <div class="form-group">
                                    <label class="control-label"><span id="spanLikeComponent" class="markrequired spanLikeComponent">*</span>Like <%=ComponentType %> #:</label>
                                    <asp:TextBox ID="txtLikeMaterial" runat="server" CssClass="BOMrequired alphanumericToUpper1 minimumlength form-control LikeMaterial NumberClass" MaxLength="20" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItem") %>' ToolTip="Like Item"></asp:TextBox>
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
                                    <label class="control-label"><span id="spanLikeComponentDesc" class="markrequired spanLikeComponentDesc">*</span>Like <%=ComponentType %> Description:</label>
                                    <asp:TextBox ID="txtLikeMaterialDesc" runat="server" CssClass="BOMrequired form-control LikeDescription DescriptionClass" Text='<%# DataBinder.Eval(Container.DataItem, "CurrentLikeItemDescription") %>' ToolTip="Like Item Description"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired MPTLocation">*</span>Make/Pack & Transfer Locations:</label>
                                    <asp:DropDownList ID="drpXferLocation" CssClass="BOMrequired form-control MakeTransferLoc" title="Make/Pack & Transfer Locations" ToolTip="Make/Pack & Transfer Locations" runat="server">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired spanOrigin">*</span>Country of Origin:</label>
                                    <asp:DropDownList ID="drpTSCountryOfOrigin" CssClass="form-control BOMrequired selectOrigin" title="Country of Origin" runat="server">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label"><span class="markrequired">*</span>Pack Location:</label>
                                    <asp:DropDownList ID="drpTSPackLocation" CssClass="form-control BOMrequired packLocation" title="Pack Location" runat="server">
                                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div id="dvTSSPKChange" class="form-group">
                                    <label class="control-label">Immediate SPK Change:</label>
                                    <asp:DropDownList ID="ddlImmediateSPKChange" ClientIDMode="Static" CssClass="form-control" ToolTip="Please select Immediate SPK Change" runat="server" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row ddlFlowthrough" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-6">
                                <div class="form-group">
                                    <label class="control-label">Flowthrough:</label>
                                    <asp:TextBox ID="txtFlowthrough" runat="server" BorderStyle="None" ReadOnly="True" CssClass="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "Flowthrough") %>' ToolTip="Flowthrough"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row" <%# Container.ItemIndex % 2 == 0 ? "" : "style=\"background-color:#BCD3F2;\"" %>>
                            <div class="col-xs-12 col-sm-6 col-md-12">
                                <div class="form-group">
                                    <label class="control-label"><%=ComponentType %> Comments</label>
                                    <input type="text" runat="server" class="form-control" id="txtSEMIComment" value='<%# DataBinder.Eval(Container.DataItem, "Notes") %>' />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <br />
            <asp:Button ID="btnAddnew" CssClass="ButtonControlAutoSize" Text="" runat="server" OnClientClick="return true;" OnClick="btnAddnew_Click" />
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            PCSRequirements();
        });


        function IsInternalSemiRequired(arg) {
            var anchor = $("#" + arg.id);
            var newExisting = $("#" + arg.id + " option:selected").text();
            if (newExisting == 'New') {
                anchor.closest(".semiRow").find(".markrequired").show();
                anchor.closest(".semiRow").find(".EXISTINGBOM").addClass("BOMrequired");
                //anchor.closest(".semiRow").find("#dvTSSPKChange").addClass("hideItem");
                if (anchor.closest(".semiRow").find("#txtMaterialNumber").val() == "") {
                    anchor.closest(".semiRow").find("#txtMaterialNumber").val("NEEDS NEW");
                }
            }
            if (newExisting == "Network Move") {
                anchor.closest(".semiRow").find(".markrequired").show();
                anchor.closest(".semiRow").find(".EXISTINGBOM").addClass("BOMrequired");
                //anchor.closest(".semiRow").find("#dvTSSPKChange").removeClass("hideItem");
            }
            if (newExisting == 'Existing') {
                anchor.closest(".semiRow").find(".markrequired").hide();
                anchor.closest(".semiRow").find(".BOMrequired").addClass("EXISTINGBOM").removeClass("BOMrequired");
                //anchor.closest(".semiRow").find("#dvTSSPKChange").addClass("hideItem");
                if (anchor.closest(".semiRow").find("#txtMaterialNumber").val().toLocaleLowerCase() == "needs new") {
                    anchor.closest(".semiRow").find("#txtMaterialNumber").val("");
                }
            }
            anchor.closest(".semiRow").find(".TSRequired").addClass("BOMrequired");
            anchor.closest(".semiRow").find(".TSRequiredMark").show();
        }

        function OnChangeNewExistingForPCs(arg) {
            var anchor = $("#" + arg.id);
            var newExisting = $("#" + arg.id + " option:selected").text();
            if (newExisting == 'New') {
                if (anchor.closest(".semiRow").find("#txtMaterialNumber").val() == "") {
                    anchor.closest(".semiRow").find("#txtMaterialNumber").val("NEEDS NEW");
                }
            }
            if (newExisting == 'Existing') {
                if (anchor.closest(".semiRow").find("#txtMaterialNumber").val().toLocaleLowerCase() == "needs new") {
                    anchor.closest(".semiRow").find("#txtMaterialNumber").val("");
                }
            }
        }

    </script>
</asp:Panel>
