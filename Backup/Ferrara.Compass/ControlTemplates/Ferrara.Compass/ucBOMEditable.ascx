<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBOMEditable.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucBOMEditable" %>

<h3 class="accordion">Component Details</h3>
<div class="panel">
    <div class="container ucmain" id="dvMain" style="border: 1px solid black;" clientidmode="Static" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
                <asp:ValidationSummary ID="ItemValidationSummary" ClientIDMode="Static" CssClass="error" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
                <div id="dverror_messageuc" class="error" style="display: none;">
                    <ul id="error_messageuc">
                        <li></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Component Type:</label>
                    <asp:DropDownList ID="drpPkgComponent" CssClass="form-control" onchange="NewPackagingItem();" ClientIDMode="Static" runat="server">
                        <asp:ListItem Text="--Select Packaging Component--" Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:HiddenField ID="hdnSAPMatGroup" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnPackagingComponent" ClientIDMode="Static" runat="server" />
                    <asp:HiddenField ID="hdnParentID" ClientIDMode="Static" runat="server" />

                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">New/Existing:</label>
                    <asp:DropDownList ID="drpNew" onchange="BOMNewCondition(this);" ClientIDMode="Static" runat="server" ViewStateMode="Enabled" CssClass="form-control">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="New" Value="New"></asp:ListItem>
                        <asp:ListItem Text="Existing" Value="Existing"></asp:ListItem>
                        <asp:ListItem Text="Network Move" Value="Network Move"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Pack Unit:</label>
                    <asp:DropDownList ID="ddlPackUnit" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Pack Quantity:</label>
                    <asp:TextBox ID="txtPackQty" CssClass="form-control minimumlength numericDecimal3" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Flowthrough:</label>
                    <asp:DropDownList ID="ddlFlowthrough" runat="server" CssClass="form-control" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <asp:Panel class="row ipf" ID="lblCompNote" runat="server" Visible="false">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <span class="comment-block">This component has been disabled because it has children, please delete or move all child components before changing the component type.</span>
            </div>
        </asp:Panel>
        <div class="row hideableRow ipf">
            <div class="col-xs-12 col-sm-4 col-md-4">
                <div class="form-group">
                    <label class="control-label">New Graphics Required?:</label>
                    <asp:DropDownList ID="drpGraphicsNeeded" runat="server" onchange="GraphicsCheck(this);" CssClass="form-control drpGraphics" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4 col-md-4">
                <div class="form-group">
                    <label class="control-label">Component Contains NLEA?:</label>
                    <asp:DropDownList ID="drpComponentContainsNLEA" runat="server" CssClass="form-control drpGraphics" ClientIDMode="Static">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4 col-md-4">
                <div class="form-group">
                    <label class="control-label">Graphics Vendor:</label>
                    <asp:DropDownList ID="ddlGraphicsVendor" runat="server" CssClass="form-control drpGraphicsVendor" ClientIDMode="Static">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group" id="dvRevPrinterSupplier" runat="server" ClientIDMode="Static">
                    <label class="control-label">Review Printer-Supplier (Proc)?:</label>
                    <asp:DropDownList ID="ddlReviewPrinterSupplier" runat="server" ClientIDMode="Static" AppendDataBoundItems="true" CssClass="form-control" Enabled="false">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row hideableRow">
            <div class="col-xs-12 col-sm-6 col-md-3" runat="server" id="dvSpecification">
                <div class="form-group">
                    <label class="control-label">Specification #:</label>
                    <asp:TextBox ID="txtSpecificationNo" CssClass="form-control minimumlength alphanumericToUpper1" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Tare Wt/Imps per lb:</label>
                    <asp:TextBox ID="txtTareWt" CssClass="form-control minimumlength numericDecimal3" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label">Component #:</label>
                    <asp:TextBox ID="txtMaterial" CssClass="form-control minimumlength alphanumericToUpper1 Component" ViewStateMode="Enabled" MaxLength="20" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label"></label>
                    <asp:Button ID="btnLookupSAPItemNumber" CssClass="ButtonControl hidebtn" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtMaterial','txtMaterialDescription');return false;" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-8">
                <div class="form-group">
                    <label class="control-label">Component Description:</label>
                    <asp:TextBox ID="txtMaterialDescription" CssClass="form-control ComponentDesc" ClientIDMode="Static" MaxLength="40" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label">Like Component #:</label>
                    <asp:TextBox ID="txtLikeItem" CssClass="form-control minimumlength alphanumericToUpper1 OldComponent" ViewStateMode="Enabled" MaxLength="20" ClientIDMode="Static" runat="server"></asp:TextBox>

                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label"></label>
                    <asp:Button ID="btnOldComponet" CssClass="ButtonControl hidebtn" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtLikeItem','txtLikeDescription');return false;" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-8">
                <div class="form-group">
                    <label class="control-label">Like Component Description:</label>
                    <asp:TextBox ID="txtLikeDescription" CssClass="form-control OldComponentDesc" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row ipf">
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label">Old Component #:</label>
                    <asp:TextBox ID="txtOldMaterial" ClientIDMode="Static" runat="server" CssClass="alphanumericToUpper1 minimumlength form-control" MaxLength="20"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group">
                    <label class="control-label">&nbsp;</label>
                    <asp:Button ID="btnLookupOldCompDesc" CssClass="ButtonControl" runat="server" Text="Find" OnClientClick="getCompDescriptionBySAPBOMList('txtOldMaterial','txtOldMaterialDesc');return false;" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-8">
                <div class="form-group">
                    <label class="control-label">Old Component Description:</label>
                    <asp:TextBox ID="txtOldMaterialDesc" ClientIDMode="Static" runat="server" CssClass="form-control minimumlength alphanumericToUpper1 OldComponent"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row hideableRow ipf">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">How is it a Like Component #:</label>
                    <asp:TextBox ID="txtLikeMaterial" TextMode="MultiLine" ToolTip="Does like component # have same dieline, graphics, etc." Rows="3" runat="server" CssClass="form-control whyLikeComponent" Text='' ClientIDMode="Static"></asp:TextBox>
                    <label id="lblItemNote" class="comment-block" style="color: #777777">Does like component # have same dieline, graphics, etc.</label>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Graphics Brief:</label>
                    <asp:TextBox ID="txtGraphicsBrief" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control GraphicsBrief" Text='' ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row transferSemi purchasedCandy">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Operation</h2>
            </div>
        </div>
        <div class="row transferSemi purchasedCandy">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row transferSemi">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Pack Location:</label>
                    <asp:DropDownList ID="drpTSPackLocation" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Country of Origin:</label>
                    <asp:DropDownList ID="drpTSCountryOfOrigin" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Make/Pack & Transfer Location</label>
                    <asp:DropDownList ID="drpTransferLocation" ClientIDMode="Static" runat="server" CssClass="form-control">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row transferSemi purchasedCandy">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Transfer/Purchased Candy SEMI Comments</label>
                    <asp:TextBox ID="txtSEMIComment" runat="server" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row purchasedCandy">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Pack Location:</label>
                    <asp:DropDownList ID="drpPCSPackLocation" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Country of Origin:</label>
                    <asp:DropDownList ID="drpPCSCountryofOrigin" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Make/Pack & Transfer Location</label>
                    <asp:DropDownList ID="drpPurchasedCandyLocation" ClientIDMode="Static" runat="server" CssClass="form-control">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row candySemi purchasedCandy">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Semi Details</h2>
            </div>
        </div>
        <div class="row candySemi purchasedCandy">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row candySemi purchasedCandy">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Trials completed and product set aside for pack trials?:</label>
                    <asp:DropDownList ID="drpTrialsCompleted" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">New Formula:</label>
                    <asp:DropDownList ID="ddlNewFormula" ClientIDMode="Static" CssClass="form-control" runat="server">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row candySemi purchasedCandy">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Shelf Life:</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtShelfLife" ClientIDMode="Static" runat="server" CssClass="form-control numericDecimal0"></asp:TextBox><span class="input-group-addon"> Days</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row printer">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Printer Details</h2>
            </div>
        </div>
        <div class="row printer">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row printer">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group ">
                    <label class="control-label">Printer/Supplier:</label>
                    <asp:DropDownList ID="ddlPrinter" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>


                </div>
            </div>

            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Request new Printer/Supplier:</label>
                    <asp:TextBox ID="txtNewPrinter" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>

                    <br />
                    <asp:Label ID="lblPrinterRequestSent" ClientIDMode="Static" Visible="false" runat="server" CssClass="AccessRequest">Your request has been sent!</asp:Label>
                    <asp:Label ID="lblPrinterError" ClientIDMode="Static" runat="server" CssClass="Error"></asp:Label>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <label class="control-label"></label>
                <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" OnClientClick="return chkRequest('printer');" OnClick="btnAdd_Click" />
            </div>
        </div>
        <div class="row printer">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group ">
                    <label class="control-label">Lead Time for Component:</label>
                    <div class="input-group">
                        <asp:TextBox ID="txtLeadTimeMaterial" CssClass="form-control alphanumericToUpper1" ClientIDMode="Static" runat="server"></asp:TextBox><span class="input-group-addon">Days</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Component Dimensions</h2>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-6 col-md-9">
                <div class="form-group">
                    <label class="control-label">Dimensions in inches:</label>
                    <div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">L</span><asp:TextBox ID="txtFilmLength" Width="105" CssClass="decimalfourplaces" ClientIDMode="Static" runat="server"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtFilmWidth" Width="105" ClientIDMode="Static" runat="server" CssClass="decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtFilmHeight" Width="105" ClientIDMode="Static" runat="server" CssClass="decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Back Seam:</label>
                    <asp:DropDownList ID="drpFilmBackSeam" ClientIDMode="Static" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group ">
                    <label class="control-label">Web Width (in.):</label>
                    <asp:TextBox ID="txtFilmWebWidth" CssClass="decimaltwoplaces form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group ">
                    <label class="control-label">Exact Cut Off (in.):</label>
                    <asp:TextBox ID="txtFilmExactCutoff" CssClass="decimaltwoplaces form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">UnWind (#):</label>
                    <asp:DropDownList ID="ddlFilmUnWind" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Bag Face (in.):</label>
                    <asp:TextBox ID="txtFilmBagFace" CssClass="decimaltwoplaces form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Clear/White Substrate:</label>
                    <asp:DropDownList ID="ddlFilmSubstrate" ClientIDMode="Static" CssClass="form-control" runat="server" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <asp:Label CssClass="control-label" Font-Bold="true" runat="server" ClientIDMode="Static"></asp:Label>
                    <asp:DropDownList ID="ddlFilmStructure" ClientIDMode="Static" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label class="control-label">Request new Substrate:</label>
                    <asp:TextBox ID="txtNewSubstrate" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                    <asp:Label ID="lblSubstrateRequestSent" runat="server" Visible="false" CssClass="AccessRequest">Your request has been sent!</asp:Label>
                    <asp:Label ID="lblSubstrateError" ClientIDMode="Static" runat="server" CssClass="Error"></asp:Label>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-2">
                <div class="form-group ">
                    <label class="control-label"></label>
                    <asp:Button ID="btnRequestNewSubstrate" runat="server" Text="Add" CssClass="button" OnClientClick="return chkRequest('substrate');" OnClick="lbRequestNewSubstrate_Click" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6" id="dvStructureColor">
                <div class="form-group ">
                    <asp:Label ID="idComponentColor" CssClass="control-label" Font-Bold="true" runat="server" ClientIDMode="Static">(<i>Packaging Component</i>) Structure Color:</asp:Label>
                    <asp:DropDownList ID="ddlStructureColor" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-6 col-md-6 ">
                <div class="form-group">
                    <label class="control-label">Max Roll (OD) (in.):</label>
                    <asp:TextBox ID="txtFilmMaxRollOD" CssClass="decimaltwoplaces form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Roll (ID) (in.):</label>
                    <asp:TextBox ID="txtFilmRollID" CssClass="decimaltwoplaces form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div class="row dimensions">
            <div class="col-xs-12 col-sm-6 col-md-6 ">
                <div class="form-group">
                    <label class="control-label">Print Style:</label>
                    <asp:DropDownList ID="ddlFilmPrintStyle" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">Style:</label>
                    <asp:DropDownList ID="ddlFilmStyle" ClientIDMode="Static" runat="server" Width="96%" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Attachments</h2>
            </div>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                    <label class="control-label">CAD Drawing Name:</label>
                    <asp:TextBox ID="txtCadDrawingName" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                </div>
            </div>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-3 col-md-4">
                <label class="control-label">CAD Drawing:</label>
            </div>
            <div class="col-xs-12 col-sm-9 col-md-4">
                <input id="btnUploadCAD" runat="server" type="button" class="ButtonControlAutoSize hidebtn" value="Upload CAD Drawing" onclick="OpenDialog('Upload CAD Drawing', 'CADDrawing'); return false;" />
            </div>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <table width="50%">
                    <asp:Repeater ID="rptCADDrawing" ClientIDMode="Static" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>
                                <th>Document Name</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkNLEADelete" ClientIDMode="Static" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteAttachment_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                                <td>
                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <p class="comment-block">
                    <label id="lblItemNote2" class="comment-block">Maximum four documents.</label>
                </p>
            </div>

        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-3 col-md-4">
                <label class="control-label">Visual Reference/Rendering:</label>
            </div>
            <div class="col-xs-12 col-sm-9 col-md-4">
                <input id="btnReference" type="button" runat="server" class="ButtonControlAutoSize hidebtn" value="Upload Visual Reference/Rendering" onclick="OpenDialog('Upload Visual Reference/Rendering', 'Rendering'); return false;" />
            </div>
        </div>
        <div id="dvMoveTS" runat="server">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Move this component</h2>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlMoveTS" runat="server" CssClass="ddlMoveTS form-control">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
        <div class="row attachment">
            <div class="col-xs-12 col-sm-12 col-md-6">
                <table width="50%">
                    <asp:Repeater ID="rptVisualReference" ClientIDMode="Static" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>
                                <th>Document Name</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkNLEADelete" ClientIDMode="Static" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteAttachment_Click" CausesValidation="false"></asp:LinkButton>
                                </td>
                                <td>
                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="ReloadAttachment HiddenButton hidebtn" OnClick="btnReloadAttachment_Click"></asp:Button>
                <asp:HiddenField ID="hiddenItemId" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnPackagingItemId" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnRequiredCheck" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnTBDIndicator" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="hdnShowRevPrinterSupplierField" ClientIDMode="Static" runat="server" Value="" />
                <asp:HiddenField ID="hdnProductHierarchyLevel1" ClientIDMode="Static" runat="server" Value="" />
                <asp:HiddenField ID="hdnNovelyProject" ClientIDMode="Static" runat="server" Value="" />
                <asp:HiddenField ID="hdnExtMfgkickedoff" ClientIDMode="Static" runat="server" Value="" />
                <asp:HiddenField ID="hdnCoManClassification" ClientIDMode="Static" runat="server" Value="" />
                <asp:HiddenField ID="hdnLOB" ClientIDMode="Static" runat="server" Value="" />
            </div>
        </div>
        <div class="row">
            <asp:UpdatePanel ID="UpdatePanel4" runat="server" ClientIDMode="Static">
                <ContentTemplate>
                    <div class="col-xs-12 col-sm-6 col-md-8">
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button justifyRight" OnClick="btnCancel_Click" />
                    </div>
                    <div class="col-xs-12 col-sm-6 col-md-2">
                        <asp:Button ID="btnSaveValidate" ClientIDMode="Static" OnClientClick="return checkrequiredValidate();" runat="server" CssClass="ButtonControlAutoSize justifyRight hidebtn" Text="Save and Validate" OnClick="btnSaveValidate_Click" />
                    </div>
                    <div class="colId-xs-12 col-sm-6 col-md-2">
                        <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" OnClientClick="return checkrequired();" CssClass="clickSave button justifyRight hidebtn" Text="Save" OnClick="btnSave_Click" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $(".container:not(.ucmain) > .row").hide();
        if ($("#dvPurchased").length) {
            $("#dvPurchased .row").hide();
        }
        if ($("#dvPurchased").length) {
            $("#dvPurchased .row").hide();
        }
        if ($("#dvCandy").length) {
            $("#dvCandy .row").hide();
        }
        if ($(".repeater.divShipper").length) {
            $(".repeater.divShipper .row").hide();
        }
        if ($("#UpdateTS").length) {
            $("#UpdateTS").hide();
        }
        if ($("#printerSupplierProcSec").length) {
            $("#printerSupplierProcSec").hide();
        }
        //$("#dvMain > .row").show();
        $(".panel").each(function () {
            if (!$(this).closest("#BOMPages").length && !$(this).closest(".dvUserControl").length) {
                $(this).hide();
                $(this).prev().hide();
            }
        });
        if (!$(".panel").closest("#BOMPages").length && !$(".panel").closest(".dvUserControl").length) {
            $(".panel:eq(0)").hide();
            $("h3.accordian:eq(0)").hide();
        }
        $("#BOMPages").hide();
    });
</script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js"></script>
