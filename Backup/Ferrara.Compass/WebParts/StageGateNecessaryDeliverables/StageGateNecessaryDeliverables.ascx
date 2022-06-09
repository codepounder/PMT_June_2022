<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StageGateNecessaryDeliverables.ascx.cs" Inherits="Ferrara.Compass.WebParts.StageGateNecessaryDeliverables.StageGateNecessaryDeliverables" %>
<div class="container">
    <div id="divAccessDenied" runat="server" class="AccessDenied">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                WARNING: You do not have access to update this page. Save functionality will be disabled!
                <br />
                If you require access, please email the
                <asp:LinkButton ID="lbHelpDeskEmail" runat="server" CausesValidation="False">HelpDesk</asp:LinkButton>.<br />
            </div>
        </div>
    </div>
    <div id="divAccessRequest" runat="server" class="AccessRequest">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                Your request for access has been sent!
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
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
        <!-- Project Completed Modal -->
        <div class="modal fade" id="DialogProjectCompletdMessage" role="dialog" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                        <h4 class="modal-title">Stage Gate Project Completion</h4>
                    </div>
                    <div class="modal-body" style="display: flex;">
                        <i class="fa fa-exclamation-circle" aria-hidden="true" style="margin-top: 5px;"></i>
                        <p style="margin-left: 10px; text-align: justify;">
                            This Submit/Complete button will identify the Parent Project as Completed. This assumes all IPF/Child Projects are also in the Completed Status. 
                            If a Post Launch phase is required as indicated by the drop down within this form, the project will now be in Post Launch.
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnSubmitModal" class="btn btn-default" data-dismiss="modal" onclick="ParentProjectSubmit()">Complete</button>
                        <button type="button" id="btnCacnelModal" class="btn btn-default" data-dismiss="modal" onclick="HideDialogProjectCompletdMessage()">Cancel</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="DialogStageGateStageResubmitMessage" role="dialog" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header" style="background-color: #BCD3F2; border-radius: 6px;">
                        <h4 class="modal-title">Stage Gate Stage Re-Submission</h4>
                    </div>
                    <div class="modal-body" style="display: flex;">
                        <i class="fa fa-exclamation-circle" aria-hidden="true"></i>
                        <p style="margin-left: 10px;">
                            This form has already been submitted on the dates shown below.
                            <br />
                            <label id="lblProjectSubmittedDate"></label>
                            <br />
                            <br />
                            Resubmitting this form will record today as the date in which the Project was approved to move onto the next phase.<br />
                            <br />
                            Are you sure you’d like to Resubmit?
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnResubmitClose" class="btn btn-default" data-dismiss="modal" onclick="DialogStageGateStageResubmitMessageCancel()">No</button>
                        <button type="button" id="btnResubmitOk" class="btn btn-default" data-dismiss="modal" onclick="DialogStageGateStageResubmitMessageOK()">Yes</button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="upGateProjectInfo">
        <ContentTemplate>
            <asp:Panel runat="server" ID="SGSGateInfo"></asp:Panel>
            <asp:Panel ID="phRAGuide" runat="server">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-6">
                        <h2>Risk Assessment Status Guide</h2>
                        <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-1 RAGuideGreen">
                                &nbsp;<br />
                                &nbsp;
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-11"><i>On track.</i> Projected to meet current first ship/production timing</div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-1 RAGuideYellow">
                                &nbsp;<br />
                                &nbsp;
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-11"><i>Slightly off track.</i>  Project is behind but potential for interventions to be made to get back on track</div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-1 RAGuideRed">
                                &nbsp;<br />
                                &nbsp;
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-11"><i>Off track.</i>  In serious jeopardy of missing first ship/production timing unless major interventions are made</div>
                        </div>
                    </div>

                    <div class="col-xs-12 col-sm-12 col-md-6">
                        <asp:PlaceHolder ID="phMsg" runat="server" />
                    </div>
                </div>
            </asp:Panel>
            <h2>Stage</h2>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>

            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Select Current Project Stage:</label>
                        <asp:DropDownList ID="ddlProjectStage" ClientIDMode="Static" runat="server" CssClass="form-control" AppendDataBoundItems="true" onchange="projectStageChange();">
                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-12 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Update First Ship Date: </label>
                        <asp:TextBox ID="txtRevisedFirstShipDate" runat="server" CssClass="datePicker form-control" ToolTip="Click to Choose Date" AppendDataBoundItems="true"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row" id="nextGateDets">
                <div class="col-xs-12 col-sm-12 col-md-4">
                    <div class="form-group">
                        <label class="control-label">Next Gate: </label>
                        <asp:TextBox ID="lblNextGate" runat="server" ClientIDMode="Static" BorderStyle="None" ReadOnly="True" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
            </div>
            <asp:Repeater ID="rptParentDeliverables" runat="server" OnItemDataBound="rptParentDeliverables_ItemDataBound">
                <ItemTemplate>
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-9">
                            <h3 runat="server" id="lblNextStage" class="deliverablesH2"></h3>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-3 gateReadiness" runat="server" id="gateReadiness" visible="true">
                            <label class="control-label">Gate <span id="pchGateNumber" runat="server"></span>Readiness: <span id="pchGatePct" clientidmode="Static" runat="server"></span>%</label>
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnTotal" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCompleted" runat="server" ClientIDMode="Static" />
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-12">
                            <table class="deliverablesTable">
                                <asp:Repeater ID="rptDeliverables" runat="server" OnItemDataBound="rptDeliverables_ItemDataBound">
                                    <HeaderTemplate>
                                        <tr>
                                            <th class="deliverablesRow">
                                                <div runat="server" id="lblPrevStage"></div>
                                            </th>
                                            <th class="ownerRow">Owner</th>
                                            <th class="applicableRow">Applicable to Project</th>
                                            <th class="statusRow">Status</th>
                                            <th class="commentsRow">Comments</th>
                                            <th class="modifiedBy">Modified By</th>
                                            <th class="modifiedDate">Modified Date</th>
                                            <th class="deleteRow"></th>
                                        </tr>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td runat="server" id="tdDeliverable">
                                                <asp:Label runat="server" ID="lblDeliverable"></asp:Label>
                                                <asp:TextBox ID="txtDeliverable" runat="server" CssClass="trackChange" Visible="false"></asp:TextBox>
                                            </td>
                                            <td runat="server" id="tdOwner">
                                                <asp:Label runat="server" ID="lblOwner"></asp:Label>
                                                <asp:TextBox ID="txtOwner" runat="server" Visible="false" CssClass="trackChange"></asp:TextBox>
                                            </td>
                                            <td class="">
                                                <asp:DropDownList ID="ddlApplicable" runat="server" AppendDataBoundItems="true" CssClass="form-control SGSApplicable trackChange">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="N"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="">
                                                <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" CssClass="form-control SGSStatus trackChange" onchange="updateTotals();">
                                                    <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="commentsRow">
                                                <asp:TextBox TextMode="MultiLine" CssClass="form-control trackChange" ID="txtComments" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="modifiedBy">
                                                <asp:Label runat="server" ID="lblModifiedBy"></asp:Label></td>
                                            <td class="modifiedDate">
                                                <asp:Label runat="server" ID="lblModifiedDate"></asp:Label></td>
                                            <td class="deleteRow">
                                                <asp:Image ID="btnDeleteRow" CssClass="deleteRow" onClick="deleteDelivRow(this);return false;" Visible="false" AlternateText="Delete Row" ImageUrl="/_layouts/15/Ferrara.Compass/images/cancel.png" runat="server" />
                                                <asp:HiddenField ID="hdnDeletedStatus" Value="false" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdnChangeMade" Value="false" runat="server" ClientIDMode="Static" />
                                                <asp:HiddenField ID="hdnDeliverableId" Value="0" runat="server" ClientIDMode="Static" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <tr>
                                            <th class="deliverablesRow"></th>
                                            <th class="TotalsRow">Total Applicable to Project, Total Completed: </th>
                                            <th class="applicableRow">
                                                <asp:Label ClientIDMode="Static" runat="server" ID="lblTotalApplicable"></asp:Label></th>
                                            <th class="statusRow">
                                                <asp:Label ClientIDMode="Static" runat="server" ID="lblTotalCompleted"></asp:Label></th>
                                            <th class="commentsRow"></th>
                                            <th class="modifiedBy"></th>
                                            <th class="modifiedDate"></th>
                                            <th class="deleteRow"></th>
                                        </tr>
                                    </FooterTemplate>
                                </asp:Repeater>

                            </table>
                            <div class="row RowBottomMargin">
                                <div class="col-xs-12 col-sm-12 col-md-9">
                                    <asp:HiddenField ID="hdnStageView" runat="server" />
                                    <asp:HiddenField ID="hdnStagePct" runat="server" />
                                    <asp:HiddenField ID="hdnLookupList" runat="server" />
                                    <asp:HiddenField ID="hdnNextStageText" runat="server" />
                                    <asp:HiddenField ID="hdnStage" runat="server" />
                                    <asp:HiddenField ID="hdnDeliverablesTotalApplicable" runat="server" ClientIDMode="Static" />
                                    <asp:HiddenField ID="hdnDeliverablesTotalCompleted" runat="server" ClientIDMode="Static" />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-3">
                                    <asp:Button ID="btnAddDeliverable" runat="server" Text="Add Deliverable" CausesValidation="false" CssClass="ButtonControlAutoSize" OnClick="btnAddDeliverable_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <h2 runat="server" id="sgsGateProjectInfoHeader">Stage Gate
        <asp:Label runat="server" ID="lblBriefStage"></asp:Label>
                Brief Generation</h2>

            <asp:Repeater ID="rptGateProjectInfo" runat="server" OnItemDataBound="rptGateProjectInfo_ItemDataBound" OnItemCommand="rptGateProjectInfo_ItemCommand">
                <ItemTemplate>
                    <asp:Panel ID="summaries" CssClass="SGSGateProjectInfo FinanceBriefs" runat="server">
                        <h3 class="accordion" onclick="showhideAccordion(this);">Brief G<asp:Label ID="lblGateText" runat="server" Text=""></asp:Label>#
                        <asp:Label ID="lblBriefNo" runat="server" Text=""></asp:Label>:
                        <asp:Label ID="lblBriefName" runat="server" Text=""></asp:Label>

                        </h3>
                        <div class="FinanceBrief">
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Brief Name:</label>
                                        <asp:TextBox ID="txtBriefName" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <h2>High Level Project Summary</h2>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Product/SKU/Pack Formats:</label>
                                        <textarea id="txtProductFormatsT" class="form-control RTBox" clientidmode="Static" runat="server" textmode="MultiLine" rows="6"></textarea>
                                        <asp:HiddenField ID="txtProductFormats" ClientIDMode="Static" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Retail Execution:</label>
                                        <div class="comment-block">(Example of attribute)</div>
                                        <asp:TextBox ID="txtRetailExecution" CssClass="form-control" ClientIDMode="Static" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Other Key Info:</label>
                                        <div class="comment-block">(Such as claims if new/different, etc.) from project scope</div>
                                        <textarea id="txtOtherKeyInfoT" class="form-control RTBox" clientidmode="Static" runat="server" textmode="MultiLine" rows="5"></textarea>
                                        <asp:HiddenField ID="txtOtherKeyInfo" ClientIDMode="Static" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <h2>Project Health/Current Situation</h2>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Overall Risk:</label>
                                        <div class="comment-block">Low / Medium / High</div>
                                        <asp:DropDownList ID="ddlOverallRisk" CssClass="form-control" runat="server">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-8">
                                    <div class="form-group">
                                        <label class="control-label">&nbsp;</label>
                                        <div class="comment-block">Reason (Distinguish between Technical, Commercial, Competitive)</div>
                                        <asp:TextBox ID="txtRiskReason" CssClass="form-control" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">
                                            Gate
                        <asp:Label runat="server" ID="lblPHGate"></asp:Label>
                                            Readiness:</label>
                                        <div class="comment-block">Any missing? Explain why?</div>
                                        <asp:TextBox ID="txtGateReadiness" CssClass="form-control" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-4">
                                    <div class="form-group">
                                        <label class="control-label">Overall Status (issues):</label>
                                        <div class="comment-block">Green / Yellow / Red</div>
                                        <asp:DropDownList ID="ddlOverallStatus" CssClass="form-control listColor" runat="server" onchange="setDropdownColor();">
                                            <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-8">
                                    <div class="form-group">
                                        <label class="control-label">&nbsp;</label>
                                        <div class="comment-block">Reason</div>
                                        <asp:TextBox ID="txtStatusReason" CssClass="form-control" ClientIDMode="Static" runat="server" TextMode="MultiLine" Rows="1"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Major Upcoming Milestones:</label>
                                        <div class="comment-block">List out 3-4 major upcoming activities</div>
                                        <textarea id="txtMilestonesT" class="form-control RTBox" clientidmode="Static" runat="server" textmode="MultiLine" rows="5"></textarea>
                                        <asp:HiddenField ID="txtMilestones" ClientIDMode="Static" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <h2>Impacts of Project Health/Current Situation</h2>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Impacts of Project Health/Current Situation:</label>
                                        <div class="comment-block">
                                            <ul>
                                                <li>What will the team need to compress/parallel path to execute given risks, issues, compression?</li>
                                                <li>What support needs will they have because of this?</li>
                                            </ul>
                                        </div>
                                        <textarea id="txtImpactProjectHealthT" class="form-control RTBox" clientidmode="Static" runat="server" textmode="MultiLine" rows="5"></textarea>
                                        <asp:HiddenField ID="txtImpactProjectHealth" ClientIDMode="Static" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <h2>Team's Recommendation</h2>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <div class="form-group">
                                        <label class="control-label">Team's Recommendation:</label>
                                        <ul class="comment-block">
                                            <li class="comment-block">Agree to launch with stated risks</li>
                                            <li class="comment-block">Approve to support X, Y, Z</li>
                                            <li class="comment-block">Approve gate request</li>
                                        </ul>
                                        <textarea id="txtTeamRecommendationT" class="form-control RTBox" runat="server" textmode="MultiLine" rows="5"></textarea>
                                        <asp:HiddenField ID="txtTeamRecommendation" ClientIDMode="Static" runat="server" Value="" />
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <h2>Brief Image</h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row" id="fileTable" runat="server" visible="false">
                                <div class="col-xs-12 col-sm-12 col-md-2">
                                    <label class="control-label">Action</label>
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-10">
                                    <label class="control-label">Document Name</label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="comment-block" id="div1" runat="server">*Only one image can be attached per brief.*</div><br />
                                <div class="col-xs-12 col-sm-12 col-md-2">
                                    <asp:Button ID="btnUploadImageBrief" runat="server" Text="Upload Brief Image" CssClass="ButtonControlAutoSize" OnClientClick="return updateWSYWIG('true');" CommandName="Upload" />
                                    <asp:Button runat="server" ID="lnkOtherAttachment" Text="Delete Image" CommandName="DeleteImage" CssClass="ButtonControlAutoSize" CausesValidation="false" OnClientClick='javascript:return updateWSYWIG("false");' />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-10">
                                    <a target='_blank' id="briefImage" runat="server" href='' visible="false"></a>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <h2>Brief Actions</h2>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-4">
                                    <asp:Button ID="btnGenerateBrief" runat="server" Text="Generate Brief" CssClass="ButtonControlAutoSize" OnClientClick="return updateWSYWIG('true');" CommandName="generate" />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-2">
                                    <asp:CheckBox ID="chkFinanceBriefInGateBrief" Text="Include Financial Brief" runat="server" Style="margin-left: -20px; margin-top: 12px; color: #084C61" />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-6">
                                    <a target="_blank" id="ancBriefPDF" runat="server" visible="false"></a>&nbsp;
                                    <asp:Label ID="lblGeneratedBrief" CssClass="SuccessMessage" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-10">
                                    <asp:Button ID="btnCopyBrief" runat="server" Text="Copy Brief" CssClass="ButtonControlAutoSize" OnClientClick="return updateWSYWIG('true');" CommandName="copy" />
                                </div>
                                <div class="col-xs-12 col-sm-12 col-md-2">
                                    <asp:Button ID="btnDeleteBrief" runat="server" Text="Delete Brief" CssClass="ButtonControlAutoSize" OnClientClick="return deleteBrief(this);" CommandName="delete" />
                                    <asp:HiddenField ID="hdnProjectBriefID" ClientIDMode="Static" runat="server" />
                                    <asp:HiddenField ID="hdnDeleted" ClientIDMode="Static" runat="server" Value="false" />
                                </div>
                            </div>

                        </div>
                    </asp:Panel>
                </ItemTemplate>
            </asp:Repeater>
            <div class="row" runat="server" id="sgsGateProjectInfoFooter">
                <div class="col-xs-12 col-sm-3 col-md-4">
                    <div class="form-group">
                        <asp:Button ID="btnAddBrief" runat="server" Text="Add Additional Brief" CssClass="ButtonControlAutoSize" OnClick="btnAddBrief_Click" OnClientClick="return updateWSYWIG('true');" />&nbsp;    
                    </div>
                </div>
                <div class="col-xs-12 col-sm-3 col-md-10">
                    <div class="form-group">
                        <asp:Button ID="btnLoadBrief" runat="server" Text="Load Brief from Previous Gate" CssClass="ButtonControlAutoSize" OnClick="btnLoadBrief_Click" OnClientClick="return updateWSYWIG('true');" />&nbsp;    
                    </div>
                </div>
            </div>
            <asp:Panel ID="PanelPostLaunch" runat="server" Visible="false">
                <h2>Post Launch</h2>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-3">
                        <div class="form-group">
                            <label class="control-label"><span class="markrequired">*</span>Post Launch?:</label>
                            <asp:DropDownList ID="ddlPostLaunch" runat="server" AppendDataBoundItems="true" ClientIDMode="Static" CssClass="form-control required" ToolTip="Post Launch" onchange="updatePostLaunch();">
                                <asp:ListItem Text="Select..." Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="Y"></asp:ListItem>
                                <asp:ListItem Text="No" Value="N"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <div class="form-group">
                            <label id="postLaunchMessage" class="redMessage hideItem">Post Launch Stage will be activated for this project</label>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="PanelAttachments" runat="server">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <h2>Attachments</h2>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12 CompassSeparator">&nbsp;</div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-3 col-md-4">
                        <label class="control-label">Gate Attachment:</label>
                    </div>
                    <div class="col-xs-12 col-sm-9 col-md-4">
                        <asp:Button ID="btnUploadDocuments" runat="server" Text="Upload Documents" CssClass="ButtonControlAutoSize" OnClick="btnUploadDocs_Click" OnClientClick="return updateWSYWIG('true');" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-md-12">
                        <table>
                            <asp:Repeater ID="rpGateAttachments" runat="server" OnItemCommand="rpGateAttachments_ItemCommand">
                                <HeaderTemplate>
                                    <tr>
                                        <th>Action</th>
                                        <th>Document Name</th>
                                    </tr>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:LinkButton runat="server" ID="lnkDeleteGateFile" Text="Delete" CommandName="Delete" CausesValidation="false" OnClientClick='javascript:return confirm("Are you sure you want to delete?")' CommandArgument='<%# DataBinder.Eval(Container.DataItem, "FileUrl") %>'></asp:LinkButton>
                                        </td>
                                        <td>
                                            <a target='_blank' href='<%#Eval("FileUrl") %>'><%#Eval("FileName")%></a>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>
                </div>
            </asp:Panel>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:HiddenField ID="hdnGateNumber" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnPageName" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnProjectNumber" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnCompassListItemId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnGlobalStage" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnStageGateProjectListItemId" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnGateBriefCount" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnPrevSubmittedDate" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnContinueSubmit" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnStage" runat="server" ClientIDMode="Static" />
                    <asp:HiddenField ID="hdnProjectType" runat="server" ClientIDMode="Static" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="row RowBottomMargin">
        <div class="col-xs-12 col-sm-3 col-md-8">
            <asp:Label ID="lblSaved" CssClass="SuccessMessage justifyRight" runat="server"></asp:Label>
        </div>
        <div class="col-xs-12 col-sm-3 col-md-2">
            <asp:Button ID="btnSave" runat="server" Text="Save" CausesValidation="false" CssClass="ButtonControl" OnClick="btnSave_Click" />&nbsp;&nbsp;&nbsp;
        </div>
        <div class="col-xs-12 col-sm-3 col-md-2">
            <asp:Button ID="btnSubmit" ClientIDMode="Static" runat="server" Text="Submit" CssClass="ButtonControlAutoSize" OnClientClick="return SGSValidate(this);" OnClick="btnSubmit_Click" />&nbsp;
            <asp:Button ID="btnReloadAttachment" runat="server" Text="Load Attachments" CssClass="LoadAttachments HiddenButton" OnClick="btnSave_Click"></asp:Button>
            <asp:Button ID="btnCompleteProject" runat="server" Text="Complete Project" CssClass="HiddenButton" OnClick="btnCompleteProject_Click" ClientIDMode="Static"></asp:Button>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            setDropdownColor();
            projectStageChange();
            $('.datePicker').datepicker({
                format: 'mm/dd/yyyy',
                autoclose: true
            });
            runTinyMC();
        });
    </script>
</div>
