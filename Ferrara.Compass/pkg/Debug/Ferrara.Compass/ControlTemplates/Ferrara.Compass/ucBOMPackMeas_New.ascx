<%@ Assembly Name="Ferrara.Compass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04ae2c9e0ea4efe6" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucBOMPackMeas_New.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucBOMPackMeas_New" %>
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
