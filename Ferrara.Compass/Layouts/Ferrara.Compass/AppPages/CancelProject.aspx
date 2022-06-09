<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CancelProject.aspx.cs" Inherits="Ferrara.Compass.Layouts.Ferrara.Compass.AppPages.CancelProject" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js"></script>
    <style type="text/css">
        .tableheader {
            font-family: Tahoma,Verdana;
            font-size: 14px;
            background-color: #003399;
            color: #CCCCFF;
            font-weight: bold;
            padding: 2px,2px,2px,2px;
        }

        .rowstyle {
            font-family: Tahoma,Verdana;
            font-size: 12px;
            padding: 2px,2px,2px,2px;
        }

        .lbl {
            font-family: Tahoma,Verdana;
            font-size: 12px;
            font-weight: bold;
        }

        .fieldsetlbl {
            font-family: Tahoma,Verdana;
            font-size: 12px;
        }

        .txt {
            font-family: Tahoma,Verdana;
            font-size: 12px;
            border: 1px solid #000;
        }

        .pnlDisplayNone {
            display: none;
        }

        .ButtonControl {
            background-color: #4863A0;
            color: White;
            width: 75px;
            font-weight: bold;
        }

        .AccessRequest {
            color: Green;
            font-weight: bold;
            padding: 5px;
            font-size: 14px;
        }

        .Error {
            color: Red;
            font-weight: bold;
            padding: 5px;
            font-size: 14px;
        }
    </style>
    <link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
    <link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" rel="Stylesheet" />

    <script type="text/javascript">
        //******** Basic Dialog Starts Here ***********/
        function closeBasicDialog() {
            //  alert("closing");
            SP.SOD.executeFunc('sp.js', 'SP.ClientContext',
                    function () {
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

    <script type="text/javascript">
        function validatePage() {
            if ($('.ProjectCancelReasson').val() == '') {
                $('.ValidationErrors').text('Please enter the reason for cancellation.');
                loadingIconAdded = true;
                $(".disablingLoadingIcon").remove();
                return false;
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <div id="dverror_message" style="margin-left: -25px;" class="dverrormessage" style="">
        <asp:Label ID="lblValidationErrors" Class='ValidationErrors' runat="server" Text="" Style="float: left; width: 400px;" ForeColor="Red"></asp:Label>
    </div>
    <table id="tbCancelProject" style="height: 150px; width: 500px; margin-left: -25px;">
        <tr>
            <td colspan="2">
                <asp:Label ID="lblStatusUpdateError" runat="server" Text="" Style="float: left; width: 400px;" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <span class="markrequired">*</span>
                <asp:Label ID="lblProjectCancelReasson" CssClass="control-label" runat="server" Style="width: 400px; font-weight: 700;" Text="Please enter in the reason for cancellation:"></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:TextBox ID="txtProjectCancelReasson" class="ProjectCancelReasson" runat="server" TextMode="MultiLine" MaxLength="1024" Style="float: left; width: 500px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="float: right;">
                <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ButtonControl" OnClick="btnClose_Click" />
            </td>
            <td style="float: right;">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="ButtonControl" OnClick="btnSubmit_Click" OnClientClick="return validatePage();" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Cancel Project
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server">
</asp:Content>


