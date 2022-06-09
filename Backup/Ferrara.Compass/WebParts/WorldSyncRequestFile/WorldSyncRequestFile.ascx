<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorldSyncRequestFile.ascx.cs" Inherits="Ferrara.Compass.WebParts.WorldSyncRequestFile.WorldSyncRequestFile" %>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Bootstrap/bootstrap.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=21"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js"></script>

<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" rel="Stylesheet" />

<div class="container" id="dvMain">
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>PMT Requests</h1>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 ValidationSummary">
            <asp:ValidationSummary ID="ItemValidationSummary" runat="server" Enabled="True" HeaderText="<strong><p style='font-size:medium'>Submission Failed!</p></strong><br/>Please correct the errors below:<br/>" />
            <div id="dverror_message" style="display: none;">
                <ul id="error_message"></ul>
            </div>
        </div>
    </div>
    <div class="row">
        <div id="divAccessDenied" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessDenied">
            WARNING: You do not have access to update this page. Save functionality will be disabled!
            <br />
            If you require access, please email the
            <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False" OnClick="lbHelpDeskEmail_Click">HelpDesk</asp:LinkButton>.<br />
        </div>
    </div>
    <div class="row">
        <div id="divAccessRequest" runat="server" class="col-xs-12 col-sm-12 col-md-12 AccessRequest">
            Your request for access has been sent!
        </div>
    </div>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">SAP number:</label>
                <asp:TextBox ID="txtSAPnumber" runat="server" CssClass="form-control required alphanumericToUpper2" ToolTip="SAP number" ClientIDMode="Static" MaxLength="30"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-4">
            <div class="form-group">
                <label class="control-label">SAP description:</label>
                <asp:TextBox ID="txtSAPdescription" runat="server" CssClass="form-control required" ToolTip="SAP description" ClientIDMode="Static" MaxLength="100"></asp:TextBox>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-6">
            <table style="width: 100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="ButtonControlAutoSize" OnClientClick="return validateSearch()" OnClick="btnSearch_Click" Enabled="false" /></td>
                    <td></td>
                </tr>
            </table>
        </div>
    </div>
    <div id="dvSearchResult" class="searchResult" runat="server" visible="false">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <label class="control-label">SAP number:</label>
                <asp:Label ID="lblSAPNumber" runat="server" CssClass="control-label" ClientIDMode="Static"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <label class="control-label">SAP description:</label>
                <asp:Label ID="lblSAPDescription" runat="server" CssClass="control-label" ClientIDMode="Static"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <label class="control-label">Project No: </label>
                <asp:Label ID="lblProjectNumber" runat="server" CssClass="control-label" ClientIDMode="Static"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table width="50%" id="tabAttachmentsImages">
                    <asp:Repeater ID="rptAttachmentSpecImage" ClientIDMode="Static" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th><span class="control-label">Images: </span></th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td><a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a></td>
                                </tr>
                            </tbody>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>

        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table width="50%" id="tabAttachmentsNLEA">
                    <asp:Repeater ID="rptAttachmentSpecNLEA" ClientIDMode="Static" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th><span class="control-label">Nutritionals: </span></th>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tbody>
                                <tr>
                                    <td><a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a></td>
                                </tr>
                            </tbody>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
        </div>
    </div>
    <div class="row col-xs-12 col-sm-12 col-md-12">
        <asp:Label ID="lblmsg" CssClass="SuccessMessage" runat="server"></asp:Label>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <table style="width: 100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnRequestImage" runat="server" Text="Request Image" CssClass="ButtonControlAutoSize" OnClientClick="return ValidateData()" OnClick="btnRequestImage_Click" Enabled="false" /></td>
                    <td align="left">
                        <asp:Button ID="btnRequestNutritionals" runat="server" Text="Request Nutritionals" CssClass="ButtonControlAutoSize" OnClientClick="return ValidateData()" OnClick="btnRequestNutritionals_Click" Enabled="false" /></td>
                </tr>
            </table>
        </div>
    </div>
</div>

<script>
    function validateSearch() {       
        var  label, fieldName, requiredMsg;
        var isValid = true;
        $('#error_message').empty();
        $('.required').each(function (i, obj) {
            dvMain = $(this).closest("div.form-group");
            label = dvMain.find('label');
            if (label.length)
                fieldName = label.text().replace(":", "");
            else
                fieldName = $(this).attr('title');
            requiredMsg = fieldName + ' is required';

            var id = $(this).attr('id');
            if ($(this).is('input') || ($(this).is('textarea'))) {
                var value = $(this).val().trim();
                if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                    if (value == "") {
                        isValid = false;
                        $(".searchResult").hide();
                        $("#dverror_message").show();
                        $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                    }
                    else {
                        $(this).removeClass('highlightElement');                     

                    }
            }
            else {
                var value = $(this).val();
                if (!$(this).parent().hasClass('hideItem') && !$(this).prop("disabled") && !$(this).prop("readonly"))
                    if (value == '-1') {
                        isValid = false;
                        $(".searchResult").hide();
                        $("#dverror_message").show();
                        $('#error_message').append('<li class="errorMessage" >' + requiredMsg + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
                    } else {
                        $(this).removeClass('highlightElement');                       
                    }
            }
        });

        if (isValid)
            return true;
        setFocusError();
        loadingIconAdded = true;
        return false;            
    }
    function setFocusError() {
        $('html, body').animate({
            scrollTop: $("h1").offset().top - 100
        }, 1000);
    }
    </script>

