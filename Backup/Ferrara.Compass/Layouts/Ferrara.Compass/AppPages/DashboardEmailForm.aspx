<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardEmailForm.aspx.cs" Inherits="Ferrara.Compass.Layouts.Ferrara.Compass.AppPages.DashboardEmailForm" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
    <script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
    <script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js"></script>

    <link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" type="text/css" rel="Stylesheet" />
    <link href="/_layouts/15/Ferrara.Compass/css/BootStrap/bootstrap.min.css" type="text/css" rel="Stylesheet" />
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
    <table>
        <tr>
            <td id="DBEmailMessageRow">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="form-group">
                            <asp:Label id="lblExtraMessage" runat="server" CssClass="control-label">Additional Message (255 characters max.):</asp:Label>
                            <asp:TextBox runat="server" ID="txtExtraMessage" TextMode="MultiLine" MaxLength="255" Rows="6" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-10">
                        <asp:Label ID="lblSendEmailCompleted" CssClass="SuccessMessage" runat="server"></asp:Label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-6 col-sm-6 col-md-2">
                        <div class="form-group">
                            <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="ButtonControl" OnClick="btnClose_Click" />
                        </div>
                    </div>
                    <div class="col-xs-6 col-sm-6 col-md-2">
                        <div class="form-group">
                            <asp:Button runat="server" ID="btnSendEmail" CssClass="ButtonControlAutoSize" Text="Re-Send Email"  OnClick="sendEmail" />
                        </div>
                    </div>
                </div>
            </td>
            <td id="DBEmailHistoryRow">
                <asp:Panel ID="emailHistoryPanel" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
Dashboard Email Form
</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >
    Dashboard Email Form
</asp:Content>