<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoadBrief.aspx.cs" Inherits="Ferrara.Compass.Layouts.Ferrara.Compass.AppPages.LoadBrief" DynamicMasterPageFile="~masterurl/default.master" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery-ui.js"></script>
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
    function showWaitPopup(tTitle, tMessage) {
        SP.SOD.executeFunc('sp.js', 'SP.ClientContext', function () {
            var waitingDialog = SP.UI.ModalDialog.showWaitScreenWithNoClose(tTitle, tMessage, 200, 600);
        });
    }
    //$(".ms-dlgContent").height("40%");
    //$(".ms-dlgContent").width("40%");
    //$(".ms-dlgContent").css("left", Math.max(0, (($(window).width() - $(".ms-dlgContent").outerWidth()) / 2) +
                                                //$(window).scrollLeft()) + "px");
    //******** Basic Dialog Ends Here ***********/

 </script>

</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server"> 
    <asp:UpdatePanel ID="LoadBriefPopup" runat="server" ClientIDMode="Static">
        <ContentTemplate>
            <div class="row">               
                <div class="col-xs-12">
                    <asp:Label ID="lblBriefName" runat="server" CssClass="control-label" Text="Select Brief" Visible="false"></asp:Label>
                </div>
            </div>
            <asp:RadioButtonList ID="rblRadioButtonList" runat="server" >
            </asp:RadioButtonList>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row">
        <div class="col-xs-8">&nbsp;</div>
        <div class="col-xs-2">
            <asp:Button ID="btnClose" runat="server" Text="Cancel" CssClass="ButtonControl" OnClick="btnClose_Click" />
        </div>
        <div class="col-xs-2">
            <asp:Button ID="btnSubmit" runat="server" Text="Confirm" CssClass="ButtonControlAutoSize" OnClick="btnSubmit_Click" OnClientClick='$("#disablingLoadingIconCopy").show();' />
            <div class="disablingLoadingIcon" id="disablingLoadingIconCopy" clientidmode="Static" runat="server" style="display:none;top:0px;width: 84px;height:35px;left:15px;">&nbsp;</div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <label id="lblError" style="color:Red" runat="server" />
            <asp:Label ID="lblUploadError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>

<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">

</asp:Content>

<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea" runat="server" >

</asp:Content>
