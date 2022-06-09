<%@ Assembly Name="Ferrara.Compass, Version=1.0.0.0, Culture=neutral, PublicKeyToken=04ae2c9e0ea4efe6" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadForm.aspx.cs" Inherits="Ferrara.Compass.Layouts.Ferrara.Compass.AppPages.UploadForm" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
     <style type="text/css">
        .tableheader
        {
            font-family: Tahoma,Verdana;
            font-size: 14px;
            background-color: #003399;
            color: #CCCCFF;
            font-weight: bold;
            padding: 2px,2px,2px,2px;
        }
        .rowstyle
        {
            font-family: Tahoma,Verdana;
            font-size: 12px;
            padding: 2px,2px,2px,2px;
        }
        .lbl
        {
            font-family: Tahoma,Verdana;
            font-size: 12px;
            font-weight: bold;
        }
        .fieldsetlbl
        {
            font-family: Tahoma,Verdana;
            font-size: 12px;
        }
        .txt
        {
            font-family: Tahoma,Verdana;
            font-size: 12px;
            border: 1px solid #000;
        }
        .pnlDisplayNone
        {
            display: none;
        }
        .ButtonControl
        {
            background-color:#4863A0;
            color:White;
            width:75px;
            font-weight:bold;
        }
       .AccessRequest
        {
            color:Green;
            font-weight:bold;
            padding:5px;
            font-size:14px;
        }    
       .Error
        {
            color:Red;
            font-weight:bold;
            padding:5px;
            font-size:14px;
        }    
    </style>

<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" rel="Stylesheet" />

<script type="text/javascript">
     //******** Basic Dialog Starts Here ***********/
    function closeBasicDialog() {
      //  alert("closing");
        SP.SOD.executeFunc('sp.js', 'SP.ClientContext', 
                function (){
                    SP.UI.ModalDialog.commonModalDialogClose(1, 1);
                }
        );
    }

    function cancelBasicDialog() {
        SP.SOD.executeFunc('sp.js', 'SP.ClientContext',
                function () {
                    SP.UI.ModalDialog.commonModalDialogClose(0, 1);
                }
        );
    }

    //******** Basic Dialog Ends Here ***********/

 </script>

</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    
    <table id="TableMain" width="500px">
        <tr>
            <td class="rowstyle">
                <asp:FileUpload ID="docUpload" runat="server" />
           </td>
        </tr>
        <tr>
            <td style="float: left">
                <label id="lblError" style="color:Red" runat="server" />
                <asp:Label ID="lblUploadError" runat="server" Text="" ForeColor="Red"></asp:Label>
            </td>
            <td style="float: right">
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ButtonControl" OnClick="btnClose_Click" />
                <asp:Button ID="btnSubmit" runat="server" Text="Upload"  CssClass="ButtonControl" OnClick="btnSubmit_Click" />
            </td>
        </tr>
  </table>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Upload Document Form
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >

</asp:Content>
