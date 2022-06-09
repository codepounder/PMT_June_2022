<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WorldSyncFuseFile.ascx.cs" Inherits="Ferrara.Compass.WebParts.WorldSyncFuseFile.WorldSyncFuseFile" %>

<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/jquery.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Bootstrap/bootstrap.min.js"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Compass.js?v=21"></script>
<script type="text/javascript" src="/_layouts/15/Ferrara.Compass/js/Format.js"></script>

<link href="/_layouts/15/Ferrara.Compass/css/Bootstrap/bootstrap.min.css" rel="Stylesheet" />
<link href="/_layouts/15/Ferrara.Compass/css/Compass-Blue.css" rel="Stylesheet" />

<div class="container" id="dvMain">
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h1>PMT FUSE File Generation</h1>
        </div>
    </div>
    <asp:HiddenField ID="hddProjectId" runat="server" ClientIDMode="Static" />
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
        <div class="col-xs-12 col-sm-12 col-md-12">
            <h3 id="headerFailedSAPNumbers" runat="server" class="errorMessage">Failed SAP Numbers:</h3>
        </div>
        <div class="col-xs-12 col-sm-12 col-md-12">
            <ul id="ulFailedSAPNumbers" runat="server" visible="false"></ul>
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
        <div class="col-xs-12 col-sm-6 col-md-3" style="width: auto">
            <asp:Label ID="lblSuccess" runat="server" Text="" ForeColor="Green"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3" style="display: contents">
            <asp:Label ID="lblUploadError" runat="server" Text="" ForeColor="Red"></asp:Label>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <asp:FileUpload ID="docUpload" runat="server" Style="margin-top: 19px; margin-left: inherit" />
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <asp:Button ID="btnFUSEFileMassExport" OnClick="btnFUSEFileMassExport_Click" runat="server" CausesValidation="false" Text="Generate Mass FUSE File" CssClass="button noIcon" Style="margin-top: 15px; margin-left: inherit; width: auto;" />
            <%--<asp:Button ID="btnFUSEFileMassExport" OnClientClick="openBulkFuseFileUploadDialog('Bulk Fuse File Upload');return false;" runat="server" Text="Generate Mass FUSE File" CssClass="ButtonControlAutoSize" Style="margin-top: 15px; margin-left: inherit;" OnClick="btnFUSEFileMassExport_Click" />--%>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-4 col-md-4">
        </div>
        <div class="col-xs-12 col-sm-4 col-md-4">
            <h1 style="margin-left: 245px;">OR</h1>
        </div>
        <div class="col-xs-12 col-sm-4 col-md-4">
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-6 col-md-3">
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">SAP number:</label>
                <asp:TextBox ID="txtSAPNumber" runat="server" ClientIDMode="Static"></asp:TextBox>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3" style="margin-top: 23px; width: auto;">
            <div class="form-group">
                <button type="button" id="btnAddSAPNumber" style="width: 103px; margin: inherit;" onclick="addSAPNumber()" class="button noIcon">Add >></button>
                <button type="button" id="btnRemoveSAPNumber" style="width: 103px; margin: inherit;" onclick="removeSAPNumber()" class="button noIcon"><< Remove </button>
            </div>
        </div>
        <div class="col-xs-12 col-sm-6 col-md-3">
            <div class="form-group">
                <label class="control-label">Selected SAP numbers:</label>
                <asp:ListBox ID="lstSAPNumbersAdded" Height="125px" Width="150px" SelectionMode="Multiple" CssClass="required" ClientIDMode="Static" runat="server"></asp:ListBox>
                <asp:HiddenField ID="hdnSAPNumbers" ClientIDMode="Static" runat="server"></asp:HiddenField>
                <asp:Button ID="btnGenerateFuseFile" runat="server" Text="Generate FUSE File" CssClass="button noIcon" Style="margin-top: 15px; margin-left: inherit; width: auto;" OnClientClick="return ValidateRequiredData()" OnClick="btnGenerateFuseFile_Click" />
            </div>
        </div>
    </div>
    <div id="dvLatestFuseFiles" class="searchResult" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <table width="50%" id="tabLatestFuseFiles">
                    <asp:Repeater ID="rptLatestFuseFiles" ClientIDMode="Static" runat="server">
                        <HeaderTemplate>
                            <tr>
                                <th><span class="control-label">Current FUSE Files: </span></th>
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
</div>
<script>
    $(document).ready(function () {
        //called when key is pressed in textbox
        $("#txtSAPNumber").keypress(function (e) {
            //if the letter is not digit then display error and don't type anything
            if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                return false;
            }
        });
    });

    function ValidateRequiredData() {
        var isValid = true;
        $('#error_message').empty();
        var id = 'lstSAPNumbersAdded';

        if ($("#hdnSAPNumbers").val() == "") {
            isValid = false;
            $("#dverror_message").show();
            $('#error_message').append('<li class="errorMessage" >' + 'Please enter SAP Numbers' + ' <a style="color:darkblue" onclick=setFocusElement(' + '"' + id + '"' + ') >Update</a>   </li></br>');
        }
        else {
            $(this).removeClass('highlightElement');
            $("#dverror_message").hide();
        }

        if (isValid)
            return true;
        setFocusError();
        // loadingIconAdded = true;
        return false;
    }

    function setFocusError() {
        $('html, body').animate({
            scrollTop: $("h1").offset().top - 100
        }, 1000);
    }

    function addSAPNumber() {
        var txtSAPNumber = $("#txtSAPNumber");
        var lstSAPNumbersAdded = $("#lstSAPNumbersAdded");

        if ($("#lstSAPNumbersAdded option[value='" + txtSAPNumber.val() + "']").val() === undefined) {
            lstSAPNumbersAdded.prepend("<option value=\"" + txtSAPNumber.val() + "\">" + txtSAPNumber.val() + "</option>");
            sortSelect("#lstSAPNumbersAdded");
            setHiddenselected();
            txtSAPNumber.val("");
        }
    }

    function removeSAPNumber() {
        var lstSAPNumbersAdded = $("#lstSAPNumbersAdded");
        var selected = "";
        lstSAPNumbersAdded.find("option:selected").remove();
        setHiddenselected();
    }

    function setHiddenselected() {
        var drpGoodSourceAvailable = $("#drpGoodSourceAvailable");
        var lstSAPNumbersAdded = $("#lstSAPNumbersAdded");
        var selected = "";
        var count = lstSAPNumbersAdded.find("option").length - 1;
        lstSAPNumbersAdded.find("option").each(function (idx, opt) {
            var noSpaces = opt.text.replace(" ", "");
            if (idx != 0) {
                selected = opt.text + "," + selected;
            } else {
                selected = opt.text;
            }
        });
        $("#hdnSAPNumbers").val(selected);
    }

    var sortSelect = function (select) {
        $(select).html($(select).children('option').sort(function (x, y) {
            return $(x).text().toUpperCase() < $(y).text().toUpperCase() ? -1 : 1;
        }));
    };

    function openBulkFuseFileUploadDialog(tTitle) {
        var url = '/_layouts/15/Ferrara.Compass/AppPages/UploadBulkFuseFiles.aspx?ProjectNo=' + $("#hddProjectId").val();
        var options = {
            url: url,
            title: tTitle,
            dialogReturnValueCallback: function (result) {
                window.location.reload();
            }
        }
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);
    }
</script>
