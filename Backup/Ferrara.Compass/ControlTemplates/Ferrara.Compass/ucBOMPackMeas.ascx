<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBOMPackMeas.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucBOMPackMeas" %>
<div class="dvPackNext" style="background-color:#BCD3F2;">
    <div class="hideItem">
        <asp:TextBox ID="hdnNewExistingComp" ClientIDMode="Static" CssClass="hdnNewExistingComp" runat="server"  />
        <asp:TextBox ID="hdnNewComponentExists" ClientIDMode="Static" CssClass="hdnNewComponentExists" runat="server"  />
        <asp:TextBox ID="hdnIsTransferSemi" ClientIDMode="Static" CssClass="hdnIsTransferSemi" runat="server"  />
        <asp:TextBox ID="hdnMaterialGroup5PackType" ClientIDMode="Static" CssClass="hdnMaterialGroup5PackType" runat="server"  />
    </div>
    <div id="dvElements" runat="server" class="miscOpsClass" >
        <asp:HiddenField ID="hiddenItemId" runat="server" />
        <asp:HiddenField ID="hdnParentComponentId" runat="server" />
        <asp:HiddenField ID="hdnUCBOMComponentType" runat="server"  />
        <asp:HiddenField ID="hdnUCSEMiBOMComponentType" runat="server"  />
        
         <asp:HiddenField ID="hdnid" runat="server" />
            <asp:HiddenField ID="hdnPackTrial" ClientIDMode="Static" runat="server" />
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
        <div id="dvPack" runat="server" class="OBMSetup">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h2>Pack Trial</h2> 
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">Is a pack trial required?</label>
                        <asp:DropDownList ID="ddlPackTrial" runat="server" CssClass="form-control" ClientIDMode="Static" onchange="OnPackTrialChange();" >
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="No" Value="N"></asp:ListItem>
                            <asp:ListItem Text="Unknown" Value="U"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-group">
                        <label class="control-label">What was the result of the pack trial?</label>
                         <asp:DropDownList ID="ddlResultPackTrial" ClientIDMode="Static" runat="server" CssClass="form-control packTrial">
                            <asp:ListItem Value="-1">Select...</asp:ListItem>
                         </asp:DropDownList>
                    </div>
                </div>   
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <div class="form-group">
                        <label class="control-label">Comments on Pack Trial:</label>
                        <asp:TextBox ID="txtCommentPackTrial" runat="server" TextMode="MultiLine" MaxLength="255" Rows="6" CssClass="form-control packTrial"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-4 col-md-4">
                    <div class="form-group">
                        <label class="control-label packTrialAttachment">Pack Trial File Attachment:</label>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-8 col-md-8">  
                    <input id="btnUploadPackTrial" runat="server" type="button" class="ButtonControlAutoSize" value="Upload Pack Trial Documents" onclick="openBasicDialog2('Upload Pack Trial Documents', 'PackTrial', this.className); return false;" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <table width="50%" id="packTrialAttachments">
                    <asp:Repeater ID="rptUploadPackTrial" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th>Action</th>                            
                                <th>Document Name</th>                            
                            </tr>                            
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkPackTrialFileDelete" runat="server" Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteAttachment_Click"  CausesValidation="false"></asp:LinkButton>
                                </td>
                                <td>
                                    <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    </table>
                </div>
                 <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="ReloadAttachment HiddenButton" OnClick="btnReloadAttachment_Click" ></asp:Button>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2><asp:Label id="lblSAPSpecChangeHeader" runat="server"></asp:Label> Specifications</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="form-group">
                    <label class="control-label">Are any of the current <asp:Label id="lblSAPSpecChange" runat="server"></asp:Label> Material Master (SAP) specifications changing?</label>
                    <asp:DropDownList ID="drpSAPSpecsChange" runat="server" CssClass="form-control drpSAPSpecsChange" Width="125px">
                        <asp:ListItem Value="-1">Select...</asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Unit Measurements</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Net Unit Weight (oz):</label>
                    <asp:TextBox ID="txtNetUnitWeight" runat="server" CssClass="decimaltwoplaces form-control"></asp:TextBox>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-9">
                <div class="form-group">
                    <label class="control-label">Unit Dimensions:</label>
                    <div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">L</span><asp:TextBox ID="txtUnitMeasurementsL" Width="100" runat="server" CssClass="form-control currencyMask"></asp:TextBox><span class="input-group-addon">in.</span>
                           <span class="dimension">X</span>
                                  </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtUnitMeasurementsW" Width="100" runat="server" CssClass="form-control currencyMask"></asp:TextBox><span class="input-group-addon">in.</span>
                             <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtUnitMeasurementsH" Width="105" runat="server" CssClass="form-control currencyMask"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </div>

    <div id="divMixes" runat="server" visible="false">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Mixes</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table class="gridTable">
                    <asp:Repeater ID="rpMixesSummary" runat="server" OnItemDataBound="rpMixesSummary_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                            <th class="gridCellH">Item # in Mix</th>
                            <th class="gridCellH">Item Description</th>
                            <th class="gridCellH">Total Pieces per Selling Unit</th>
                            <th class="gridCellH">Ounces per Piece</th>
                            <th class="gridCellH">Ounces per Selling Unit</th>
                            <th class="gridCellH">Qty for Mix</th>
                            <th class="gridCellH">Lbs for FG BOM</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblMixItemNumber" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "ItemNumber") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblMixItemDescription" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "ItemDescription") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="Label1" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "NumberOfPieces") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="Label2" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "OuncesPerPiece") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblOzPerPiece" runat="server"></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblQtyMix" runat="server"></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblLbsForFGBOM" runat="server"></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div id="divShipper" runat="server" visible="false">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Shippers</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table style="border-spacing: 8px; border-collapse: separate;">
                    <asp:Repeater ID="rpShipperSummary" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th>FG Item # in Display</th>
                                <th>FG Item Description</th>
                                <th># of Units</th>
                                <th>Ounces per Unit</th>
                                <th>Ounces per FG Unit</th>
                                <th>Pack Unit</th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr <%# Container.ItemIndex % 2 == 0 ? "" : "class=\"gridRow2\""%>>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblFGItemDisplay" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumber") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblFGItemDescription" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "FGItemDescription") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblFGItemQuantity" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "FGItemNumberUnits") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblFGouncesPerUnit" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit") %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblFGouncesPerFGunit" runat="server" 
                                Text='<%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "FGItemNumberUnits")) *
                                    Convert.ToDouble(DataBinder.Eval(Container.DataItem, "FGItemOuncesPerUnit")) %>'></asp:Label></td>
                            <td class="gridCell"><asp:Label CssClass="summary" ID="lblFGPackUnit" runat="server" 
                                Text='<%# DataBinder.Eval(Container.DataItem, "FGPackUnit") %>'></asp:Label></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>

    <div class="row">
    <div class="col-xs-12 col-sm-12 col-md-12">
        <h2>Case Measurements</h2>
    </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>

     <div class="row caseDims">
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Case Pack:</label>
               <asp:TextBox ID="txtCasePack"  CssClass="form-control numericNoMask" runat="server"></asp:TextBox>
            </div>
        </div>
      <div class="col-xs-12 col-sm-6 col-md-9">
                <div class="form-group">
                    <label class="control-label">Case Dimensions:</label>
                    <div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">L</span><asp:TextBox ID="txtCaseMeasurementsL" Width="100" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                             <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtCaseMeasurementsW" Width="100"   runat ="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                             <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtCaseMeasurementsH" Width="105" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </div>
    <div class="row caseDims">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
               <label class="control-label">Case Cube (cubic ft):</label>
               <asp:TextBox ID="txtCaseCube" Enabled="false"  CssClass="form-control decimaltwoplaces" runat="server"></asp:TextBox>
            </div>
        </div>
      <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                   <label class="control-label">Case Net Weight (lbs):</label>
                     <asp:TextBox ID="txtCaseNetWeight" Enabled="false"  CssClass="form-control decimaltwoplaces" runat="server"></asp:TextBox>
                </div>
            </div>
    </div>
    <div class="row caseDims">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                <label class="control-label">Case Gross Weight (lbs):</label>
               <asp:TextBox ID="txtCaseGrossWeight"  CssClass="form-control decimaltwoplaces" runat="server"></asp:TextBox>
            </div>
        </div>
      <div class="col-xs-12 col-sm-6 col-md-6">
             
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Pallet Measurements</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
     <div class="row palletDims">
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
              <label class="control-label">Cases per Pallet:</label>
               <asp:TextBox ID="txtCasesperPallet"  CssClass="form-control numericNoMask" runat="server"></asp:TextBox>
            </div>
        </div>
      <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="form-group">
                   <label class="control-label">Double Stackable?:</label>
                    <asp:DropDownList ID="ddlDoubleStackable" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                        <asp:ListItem Text="No" Value="N"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
    </div>
    <div class="row palletDims">
        <div class="col-xs-12 col-sm-6 col-md-9">
            <div class="form-group">
                    <label class="control-label">Pallet Dimensions:</label>
                    <div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">L</span><asp:TextBox ID="txtPalletDimensionsL" Width="100" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                           <span class="dimension">X</span>
                                  </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtPalletDimensionsW" Width="100" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                             <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtPalletDimensionsH" Width="105" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
      <div class="col-xs-12 col-sm-6 col-md-3">
                <div class="form-group">
                    <label class="control-label">Pallet Cube (cubic ft):</label>
                    <asp:TextBox ID="txtPalletCube" Enabled="false" runat="server" CssClass="form-control decimaltwoplaces"></asp:TextBox>
                </div>
            </div>
    </div>
     <div class="row palletDims">
        <div class="col-xs-12 col-sm-6 col-md-6"> 
            <div class="form-group">
                   <label class="control-label">Pallet Weight (lbs):</label>
                    <asp:TextBox ID="txtPalletWeight" runat="server" CssClass="form-control decimaltwoplaces"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                   <label class="control-label">Pallet Gross Weight (lbs):</label>
                    <asp:TextBox ID="txtPalletGrossWeight" Enabled="false" runat="server" CssClass="form-control decimaltwoplaces"></asp:TextBox>
             </div>
        </div>
     </div>

      <div class="row palletDims">
        <div class="col-xs-12 col-sm-6 col-md-6"> 
            <div class="form-group">
                   <label class="control-label">Cases per Layer:</label>
                    <asp:TextBox ID="txtCasesperLayer" runat="server" CssClass="form-control numericNoMask"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-6">
            <div class="form-group">
                  <label class="control-label">Layers per Pallet:</label>
                    <asp:TextBox ID="txtLayersperPallet" runat="server" CssClass="form-control numericNoMask"></asp:TextBox>
             </div>
        </div>
     </div>
    <div class="row OBMSetup palletPattern">
        <div class="col-xs-12 col-sm-6 col-md-3"> 
            <div class="form-group">
                <label class="control-label">Is Pallet Pattern Changing?:</label>
                <asp:DropDownList ID="drpPalletPatternChange" runat="server" CssClass="drpPalletPatternChange form-control" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
     </div>
    <div class="row OBMSetup palletPattern">
        <div class="col-xs-12 col-sm-6 col-md-6"> 
            <div class="form-group">
                    <label class="control-label palletUploadLbl">Pallet Pattern Upload:</label>
                    <input id="btnUploadPalletPatern" runat="server" type="button" class="ButtonControlAutoSize" value="Pallet Pattern Upload" onclick="openBasicDialog2('Upload Pallet Pattern', 'PalletPattern', this.className); return false;" />  
                <asp:HiddenField ID="hdnPalletPatterCount" ClientIDMode="Static" runat="server" />
            </div>
        </div>
     </div>

     <div class="row OBMSetup">
         <div class="col-xs-12 col-sm-6 col-md-6">
          <table width="50%">
                <asp:Repeater ID="rptPalletPattern" runat="server" >
                    <HeaderTemplate>
                        <tr>
                            <th>Action</th>
                            <th>Document Name</th>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkPalletPatternDelete" runat="server"  Text="Delete" CommandName='<%#Eval("FileUrl") %>' OnClientClick='javascript:return confirm("Are you sure you want to delete?")' OnClick="lnkDeleteAttachment_Click" CausesValidation="false"></asp:LinkButton>
                            </td>
                            <td>
                                <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("DisplayFileName")%></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>

    <div id="dvSales" runat="server">
     <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h2>Sales Dimensions</h2>
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
     <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-9">
            <div class="form-group">
                    <label class="control-label">Unit Dimensions:</label>
                    <div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtSalesUnitDimensionsH" Width="100" ReadOnly="true" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>
                            </div>
                             
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtSalesUnitDimensionsw" Width="100" ReadOnly="true" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>
                            </div>
                        </div>
                         <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">D</span><asp:TextBox ID="txtSalesUnitDimensionsd" Width="105" ReadOnly="true" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>    
                        </div>
                    </div>
                </div>
        </div>
    </div>
      <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-9">
            <div class="form-group">
                    <label class="control-label">Case Dimensions:</label>
                    <div >
                         <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtSalesCaseDimensionsH" Width="100" ReadOnly="true" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>  
                            </div>
                        </div>                 
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtSalesCaseDimensionsW" Width="100" ReadOnly="true" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">D</span><asp:TextBox ID="txtSalesCaseDimensionsD" Width="105" ReadOnly="true" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </div>
        </div>
    <div id="dvDisplay" runat="server" class="dvDisplay">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <h2>Display Dimensions</h2>
            </div>
        </div>
        <div class="row RowBottomMargin">
            <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-9">
                <div class="form-group">
                    <label class="control-label">Display Dimensions:</label>
                    <div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">L</span><asp:TextBox ID="txtDisplayDimensionsL" Width="100" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtDisplayDimensionsW" Width="100" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtDisplayDimensionsH" Width="105" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-9">
                <div class="form-group">
                    <label class="control-label">Set Up Dimensions:</label>
                    <div >
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">L</span><asp:TextBox ID="txtSetUpDimensionL" Width="100" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">W</span><asp:TextBox ID="txtSetUpDimensionW" Width="100" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                                    <span class="dimension">X</span>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">H</span><asp:TextBox ID="txtSetUpDimensionH" Width="105" runat="server" CssClass="form-control decimalfourplaces"></asp:TextBox><span class="input-group-addon">in.</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
      
    </div>
</div>
<script type="text/javascript">
    $(document).ready(function () {
        dimensionsUpdate();
    });
</script>