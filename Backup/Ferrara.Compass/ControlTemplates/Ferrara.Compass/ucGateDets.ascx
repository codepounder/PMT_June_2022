<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucGateDets.ascx.cs" Inherits="Ferrara.Compass.ControlTemplates.Ferrara.Compass.ucGateDets" %>

<table id="gateTable" class="gateTable" runat="server" visible="true" ClientIDMode="Static">
    <tr>
        <td colspan="3">
            <div class="form-group">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-10">
                        <label class="control-label">Gate <asp:Panel runat="server" ID="lblGateNumber2" ClientIDMode="Static"></asp:Panel> Readiness: <asp:Panel runat="server" ClientIDMode="Static" ID="pchReadinessPct"></asp:Panel>%</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-10">
                        <label class="control-label">Gate <asp:Panel runat="server" ID="lblGateNumber" ClientIDMode="Static"></asp:Panel> Status:</label>
                        <asp:TextBox ID="ddlSGMeetingStatus" ClientIDMode="Static"  ReadOnly="true" runat="server" CssClass="form-control" ></asp:TextBox>
                    </div>
                </div>
                            
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-10">
                        <label class="control-label">Target SG Meeting:</label> 
                        <asp:TextBox ID="txtSGMeetingDate" AutoCompleteType="Disabled" ClientIDMode="Static" Enabled="false" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-10">
                        <label class="control-label">Actual SG Meeting:</label> 
                        <asp:TextBox ID="txtActualSGMeetingDate" AutoCompleteType="Disabled" ClientIDMode="Static" Enabled="false" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date"></asp:TextBox>
                    </div>
                </div>
            </div>
        </td>
    </tr>
    <tr>
        <td><asp:Label  ID="lblMarketing" runat="server"><span class="markrequired">*</span>Marketing</asp:Label></td>
        <td><asp:DropDownList ID="ddlMarketingColor" CssClass="listColor required" ToolTip="Marketing Color" Enabled="false" runat="server" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtMarketingComments" ToolTip="Marketing Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblSales" runat="server"><span class="markrequired">*</span>Sales</asp:Label></td>
        <td><asp:DropDownList ID="ddlSalesColor" CssClass="listColor required" ToolTip="Sales Color" runat="server" Enabled="false" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtSalesComments" ToolTip="Sales Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblFinance" runat="server"><span class="markrequired">*</span>Finance</asp:Label></td>
        <td><asp:DropDownList ID="ddlFinanceColor" CssClass="listColor required" ToolTip="Finance Color" runat="server" Enabled="false" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtFinanceComments" ToolTip="Finance Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblRD" runat="server"><span class="markrequired">*</span>InTech</asp:Label></td>
        <td><asp:DropDownList ID="ddlRDColor" CssClass="listColor required" ToolTip="InTech Color" runat="server" Enabled="false" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtRDComments" ToolTip="InTech Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblQA" runat="server"><span class="markrequired">*</span>QA</asp:Label></td>
        <td><asp:DropDownList ID="ddlQAColor" CssClass="listColor required" ToolTip="QA Color" runat="server" Enabled="false" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtQAComments" ToolTip="QA Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblPE" runat="server"><span class="markrequired">*</span>PE</asp:Label></td>
        <td><asp:DropDownList ID="ddlPEColor" CssClass="listColor required" ToolTip="PE Color" runat="server" Enabled="false" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtPEComments" ToolTip="PE Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblManu" runat="server"><span class="markrequired">*</span>Mfg</asp:Label></td>
        <td><asp:DropDownList ID="ddlManuColor" CssClass="listColor required" ToolTip="Mfg Color" runat="server" Enabled="false" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList></td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtManuComments" ToolTip="Mfg Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
        <td><asp:Label ID="lblSupplyChain" runat="server"><span class="markrequired">*</span>Supply Chain</asp:Label></td>
        <td><asp:DropDownList ID="ddlSupplyChainColor" CssClass="listColor required" ToolTip="Supply Chain Color" runat="server" Enabled="false" onchange="setDropdownColor();">
                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td><asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtSupplyChainComments" ToolTip="Supply Chain Comments" Enabled="false" runat="server"></asp:TextBox></td>
    </tr>
</table>