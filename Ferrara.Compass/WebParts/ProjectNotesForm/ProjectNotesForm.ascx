<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=16.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProjectNotesForm.ascx.cs" Inherits="Ferrara.Compass.WebParts.ProjectNotesForm.ProjectNotesForm" %>

<div>
    <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
</div>
<asp:UpdatePanel ID="ParentProjectNotes" runat="server" OnUnload="UpdatePanel_Unload">
    <ContentTemplate>
        <div id="divProjectNotesParentContainer" runat="server" class="container ProjectNotesContainerParent">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12  ParnetProjectNotesDrillDown">
                    <h1 style="margin-left: 30px;">Parent Project Notes</h1>
                </div>
            </div>
            <div class="divProjectNotesParent">
                <div class="row RowBottomMargin">
                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator"></div>
                </div>
                <div class="row RowNotesBottomMargin" id="divTextBoxNotesParent" runat="server">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <asp:TextBox ID="txtNotesParent" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" ToolTip="Enter Parent Project Notes"
                            placeholder="Enter Notes"></asp:TextBox>
                    </div>
                </div>
                <div class="row RowBottomMargin" id="divButtonSaveParent" runat="server">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <asp:Button ID="btnSaveParent" runat="server" Text="Add Comment" CssClass="ButtonControlAutoSize" OnClick="btnSaveParent_Click" ForeColor="White"
                            CausesValidation="False" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12">
                        <h4><u>Parent Project Notes History</u></h4>
                    </div>
                </div>
                <div class="row RowBottomMargin">
                    <div class="col-xs-12">
                        <asp:Label ID="lblNotesHistoryParent" runat="server" Text=""></asp:Label>
                    </div>
                </div>
                <div class="row RowBottomMargin">
                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator"></div>
                </div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdatePanel ID="childProjectNotes" runat="server" OnUnload="UpdatePanel_Unload">
    <ContentTemplate>
        <div id="divProjectNotes" runat="server" class="container ProjectNotesContainer">
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <h1>Child Project Notes</h1>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator"></div>
            </div>
            <div class="row RowNotesBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:TextBox ID="txtNotes" runat="server" TextMode="MultiLine" Rows="4" CssClass="form-control" ToolTip="Enter Project Notes" placeholder="Enter Notes"></asp:TextBox>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:Button ID="btnSave" runat="server" Text="Add Comment" CssClass="ButtonControlAutoSize" OnClick="btnSave_Click" ForeColor="White" CausesValidation="False" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <h4><u>Child Notes History</u></h4>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12">
                    <asp:Label ID="lblNotesHistory" runat="server" Text=""></asp:Label>
                </div>
            </div>
            <div class="row RowBottomMargin">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator"></div>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
