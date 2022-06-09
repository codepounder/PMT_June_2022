<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompassWorldSyncMainForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.CompassWorldSyncMainForm.CompassWorldSyncMainForm" %>

<div class="container" id="dvcontainer">  
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1 id="pageHead" runat="server"></h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_message" style="display:none;"> <ul id="error_message"></ul></div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Global Information</h2>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Target Market:</label>
                <asp:TextBox ID="txtTargetMarket" MaxLength="50" CssClass="form-control required" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Product Type:</label>
                <asp:DropDownList ID="ddlProductType" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Customer Brand Name:</label>
                <asp:TextBox ID="txtCustomerBrandName" MaxLength="50" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Brand Owner GLN:</label>
                <asp:DropDownList ID="ddlBrandOwnerGLN" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Net Content</label>
                <asp:TextBox ID="txtNetContent" CssClass="form-control required" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Qty of Next Level items</label>
                <asp:TextBox ID="txtQtyNextLevelItems" CssClass="form-control required" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Base Unit Indicator:</label>
                <asp:DropDownList ID="ddlBaseUnitIndicator" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Consumer Unit Indicator:</label>
                <asp:DropDownList ID="ddlConsumerUnitIndicator" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row hideRow">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Alternate Classification Scheme:</label>
                <asp:DropDownList ID="ddlAlternateClassificationScheme" onchange="updateCode()" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Code:</label>
                <asp:TextBox ID="txtCode" Enabled="false" CssClass="form-control required" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>
    <div class="row hideRow">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Alternate Item Identification Agency:</label>
                <asp:TextBox ID="txtAlternateItemIdAgency" Text="Assigned By Manufacturer" MaxLength="50" CssClass="form-control required" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Temperature Qualitifer Code:</label>
                <asp:TextBox ID="txtTemperatureQualitiferCode" Text="Candy" MaxLength="50" CssClass="form-control required" ClientIDMode="Static" runat="server"></asp:TextBox>
            </div>
        </div>
    </div>        
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Ordering Unit Indicator:</label>
                <asp:DropDownList ID="ddlOrderingUnitIndicator" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Dispatch Unit Indicator:</label>
                <asp:DropDownList ID="ddlDispatchUnitIndicator" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Invoice Unit Indicator:</label>
                <asp:DropDownList ID="ddlInvoiceUnitIndicator" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Data Carrier Type Code:</label>
                <asp:DropDownList ID="ddlDataCarrierTypeCode" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>GS1 Trade Items ID Key Code:</label>
                <asp:DropDownList ID="ddlGS1TradeItemsIDkeyCode" runat="server" ClientIDMode="Static" CssClass="form-control required">
                    <asp:ListItem Value="-1">Select...</asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row hideRow">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="form-group">
                <label class="control-label"><span class="markrequired">*</span>Trade Channel:</label>
                <asp:Table ID="tabChannel" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <label class="control-label">Available: </label><br />
                            <asp:ListBox ID="lbChannelAvailable" Height="125px" SelectionMode="Multiple" ClientIDMode="Static" runat="server"></asp:ListBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <button type="button" onclick="addChannel()">&gt;</button>
                            <button type="button" onclick="removeChannel()">&lt;</button>
                        </asp:TableCell>
                        <asp:TableCell>
                            <label class="control-label">Selected: </label><br />
                            <asp:ListBox ID="lbChannelSelected" Height="125px" SelectionMode="Multiple" ClientIDMode="Static" runat="server"></asp:ListBox>
                            <asp:HiddenField ID="hdnSelectedTradeChannel" ClientIDMode="Static" runat="server"></asp:HiddenField>
                        </asp:TableCell>         
                    </asp:TableRow>
                </asp:Table>
            </div>
        </div>
    </div>
    <div class="row hideRow">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Sync Detail</h2>
        </div>
    </div>
    <div class="row RowBottomMargin hideRow">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <!--region sync detail grid-->
    <div class="row hideRow">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <asp:Button id="btnAddDetail" runat="server" onClick="addSyncDetail" Text="Add" CssClass="btn" />
            <div id="dvWSChildItems" class="" runat="server">
                <table class="" style="width:100%;">
                    <asp:Repeater ID="rptWSChildItem" runat="server" OnItemCommand="rptWSChildItem_ItemCommand" OnItemDataBound="rptWSChildItem_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <th>Select</th>
                                <th>Target Market</th>
                                <th>Product Type</th>
                                <th>Delete</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr id="dvWSChildItemItemRow" style="background-color:#BCD3F2;" >
                                <td>
                                    <asp:ImageButton ID="test" CausesValidation="false" Height="16" Width="16" CssClass="select srreadonly" AlternateText="Select" CommandName="LoadControl" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' ImageUrl="/_layouts/15/Ferrara.Compass/images/Edit.gif" runat="server" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChildTargetMarket" Enabled="false" ReadOnly="true" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "TargetMarket") %>'></asp:TextBox>
                                    <asp:HiddenField ID="hdnChildItemID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChildProductType" Enabled="false" ReadOnly="true" runat="server" class="form-control" Text='<%# DataBinder.Eval(Container.DataItem, "ProductType") %>'></asp:TextBox>
                                </td>
                                    
                                <td>
                                    <asp:ImageButton ID="btnDeleteChild" CssClass="readOnly" CausesValidation="false" AlternateText="Delete Child Item" CommandName="Delete" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <!--endregion sync detail grid-->
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <asp:Label ID="lblSavedMessage" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-1 col-md-1" id="dvBtnSave" runat="server">
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btn" />
        </div>
        <div class="col-xs-12 col-sm-1 col-md-1" id="dvBtnCancel" runat="server">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn" />
        </div>
        <div class="col-xs-12 col-sm-1 col-md-1" id="dvBtnSubmit" runat="server">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn" OnClientClick="return ValidateForm()" />
        </div>
        <asp:HiddenField ID="hddWorldSyncGlobalId" runat="server" Value="0" />
        <asp:HiddenField ID="hddWorldSyncChildId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnPageState" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
    </div>
    <script>
        $(document).ready(function () {
            if ($("#hdnPageState").val() == "child") {
                $(".hideRow").hide();
                $(".hideRow .required").parent().addClass("hideItem");
            } else {
                $(".hideRow").show();
                $(".hideRow .required").parent().removeClass("hideItem");
            }
            if ($("#hdnSelectedTradeChannel").length && $("#hdnSelectedTradeChannel").val() != "") {
                var tradeChannel = $("#hdnSelectedTradeChannel").val();
                $.each(tradeChannel.split(","), function (index, item) {
                    $("#lbChannelAvailable option").filter(function () {
                        return ($(this).text() == item);
                    }).prop('selected', true);
                });
                addChannel();
            }
        });
        //Control declarations
        var lstTradeChannelSelected = $("#lbChannelSelected");
        var lstTradeChannelAvailable = $("#lbChannelAvailable");
        var ddlAlternateClassificationScheme = $("#ddlAlternateClassificationScheme");
        var txtCode = $("#txtCode");

        //Trade Channel
        function addChannel() {
        
            lstTradeChannelAvailable.find("option:selected").each(function (idx, opt) {
                lstTradeChannelSelected.prepend("<option value=\"" + opt.value + "\">" + opt.text + "</option>");
            });
            lstTradeChannelAvailable.find("option:selected").remove();
            setHiddenselected();
        }
        function setHiddenselected() {
            var selected = "";
            var count = lstTradeChannelSelected.find("option").length - 1;
            lstTradeChannelSelected.find("option").each(function (idx, opt) {
                if (idx != 0) {
                    selected = opt.text + "," + selected;
                } else {
                    selected = opt.text;
                }
            });
            $("#hdnSelectedTradeChannel").val(selected);
        }
        function removeChannel() {
            var selected = "";
            lstTradeChannelSelected.find("option:selected").each(function (idx, opt) {
                lstTradeChannelAvailable.prepend("<option value=\"" + opt.value + "\">" + opt.text + "</option>");
            });
            lstTradeChannelSelected.find("option:selected").remove();
            setHiddenselected();
        }
        //Alternate Classification Scheme
        function updateCode() {
            var code;
            code = ddlAlternateClassificationScheme.val();
            if (code == "-1")
                code = "";
            txtCode.val(code);
        }
        //Main form validation
        function ValidateForm() {
            var valid;
            valid = ValidateData();
            if ($("#hdnPageState").val() != "child") {
                valid = valid && !isListBoxEmpty(lstTradeChannelSelected, dverror_message, error_message);
                if (valid)
                    selectAllOptions(lstTradeChannelSelected);
            }
        
            return valid;
        }
    </script>
</div>