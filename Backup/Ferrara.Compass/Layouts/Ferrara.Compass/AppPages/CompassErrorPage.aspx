<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompassErrorPage.aspx.cs" Inherits="Ferrara.Compass.Layouts.Ferrara.Compass.AppPages.CompassErrorPage" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table id="TableMain" width="1000px">
        <tr>
            <td><h1 style="text-align: center;"><span style="color:Red;">Compass Error</span></h1></td>
        </tr>
        <tr style="text-align: center;">
            <td><h2>The following Error has occurred while trying to service your request:</h2></td>
        </tr>
        <tr style="text-align: center;">
            <td><h2><span style="color:Red;"><asp:Label ID="lblErrorMessage" runat="server" Text=""></asp:Label></span></h2></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr style="text-align: center; font-size: 12px">
            <td><asp:Label ID="lblCommHeader" runat="server" Text="The Commercialization Item can be accessed below:"></asp:Label></td>
        </tr>
        <tr style="text-align: center; font-size: 12px">
            <td><asp:LinkButton ID="lbCommercializationItem" runat="server" OnClick="lbCommercializationItem_Click">Commercialization Item</asp:LinkButton></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
        </tr>
        <tr style="text-align: center; font-size: 12px">
            <td><asp:LinkButton ID="lbHome" runat="server" OnClick="lbHome_Click"><< Home >></asp:LinkButton></td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Compass Error
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
Compass Error
</asp:Content>
