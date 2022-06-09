using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.DependencyResolution;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Classes;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint.WebControls;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace Ferrara.Compass.WebParts.InitialApprovalReviewForm
{
    [ToolboxItemAttribute(false)]
    public partial class InitialApprovalReviewForm : WebPart
    {
        #region Member Variables
        private IInitialApprovalReviewService initialApprovalService;
        private IApprovalService approvalService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private int iItemId = 0;
        private INotificationService notificationService;
        private IProjectTimelineTypeService projectTimelineTypeService;
        private static IProjectTimelineTypeService timelineNumbers;
        private IBillOfMaterialsService bomService;
        private bool InvalidPeopleEditor = false;
        private IBOMSetupService BOMSetupService;
        public static CultureInfo ci = new CultureInfo("en-US");
        #endregion

        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }

        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public InitialApprovalReviewForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            initialApprovalService = DependencyResolution.DependencyMapper.Container.Resolve<IInitialApprovalReviewService>();
            approvalService = DependencyResolution.DependencyMapper.Container.Resolve<IApprovalService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            projectTimelineTypeService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();

            timelineNumbers = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();

            bomService = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    this.divAccessDenied.Visible = false;
                    this.divAccessRequest.Visible = false;

                    Utilities.BindDropDownItemsById(drpInitialTimeTable, GlobalConstants.LIST_TimelineTypesLookup, SPContext.Current.Web.Url);

                    // Check for a valid project number
                    if (!CheckProjectNumber())
                        return;

                    LoadFormData();
                    InitializeScreen();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SrOBMApproval.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.SrOBMApproval.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
                //peOBM.Validate();
            }
            CallUpdatePeopleEditorScriptFunction();
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.SrOBMApproval))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.SrOBMApproval))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            //if (Utilities.LockScreen(wfStep.ToString()))
            //{
            //    if (!Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            //        this.btnSave.Enabled = false;
            //    this.btnSubmit.Enabled = false;
            //}

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }
        }
        private Boolean ValidateForm()
        {
            Boolean bValid = true;

            int RepeaterCount = 0;

            if (!ValidateMembers(ref RepeaterCount, "Project", divProjectManagers, "rptProjectManagers", "peProjectManagerMembers", "hdnDeletedStatusForProjectManager")) bValid = false;

            return bValid;
        }
        private void CheckforDuplicateProjects(string projectNumber, string sapNumber)
        {
            List<string> duplicates = utilityService.CheckForDuplicateFinishedGoodProjects(projectNumber, sapNumber);

            if ((duplicates != null) && (duplicates.Count > 0))
            {
                foreach (string proj in duplicates)
                {
                    this.ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Duplicate Projects Detected:</p></strong><br/>";
                    ErrorSummary.AddError("Project " + proj + " already in progress contains the same finished good number!", this.Page);
                }
            }
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            InitialApprovalReviewItem initialApprovalItem = initialApprovalService.GetInitialApprovalReviewItem(iItemId);
            CompassPackMeasurementsItem objCompassPackMeasurementsItem = bomService.GetPackMeasurementsItem(iItemId, 0);
            hdnProjectType.Value = initialApprovalItem.ProjectType;
            // Check for existing projects for this SAP Item Number
            if (!initialApprovalItem.SAPItemNumber.ToLower().Equals(GlobalConstants.CONST_NeedsNew.ToLower()))
                CheckforDuplicateProjects(ProjectNumber, initialApprovalItem.SAPItemNumber);

            if (initialApprovalItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                drpInitialTimeTable.Items.RemoveAt(3);
                drpInitialTimeTable.Items.RemoveAt(2);
            }

            // Set the Initial Approval Information
            try
            {
                this.lblAnnualDollar.Text = initialApprovalItem.AnnualProjectedDollars.ToString();
            }
            catch (Exception ex)
            {
                this.lblAnnualDollar.Text = "0";
            }
            try
            {
                this.lblAnnualUnits.Text = Utilities.FormatNumber(initialApprovalItem.AnnualProjectedUnits);
            }
            catch (Exception ex)
            {
                this.lblAnnualUnits.Text = "$0";
            }

            this.lblBrand.Text = initialApprovalItem.MaterialGroup1Brand;
            this.lblCustomer.Text = initialApprovalItem.Customer;

            this.lblExpectedGrossMargin.Text = Utilities.FormatPercentage(initialApprovalItem.ExpectedGrossMarginPercent, 2);

            this.lblProjectNotes.Text = initialApprovalItem.ItemConcept;
            this.lblProjectType.Text = initialApprovalItem.ProjectType;
            this.lblRevisedFirstShipDate.Text = Utilities.GetDateForDisplay(initialApprovalItem.RevisedFirstShipDate);
            this.lblWeeksToShip.Text = Utilities.DetermineWeeksToShip(initialApprovalItem.RevisedFirstShipDate);
            this.lblProductHierarchy1.Text = initialApprovalItem.ProductHierarchyLevel1;
            this.lblProductHierarchy2.Text = initialApprovalItem.ProductHierarchyLevel2;
            txtChannel.Text = initialApprovalItem.Channel;
            txtFirstShipdate.Text = Utilities.GetDateForDisplay(initialApprovalItem.RevisedFirstShipDate);
            txtProjectStartDate.Text = Utilities.GetDateForDisplay(initialApprovalItem.ProjectStartDate);

            //ProjectManager
            var ProjectManager = Utilities.SetPeoplePickerValue(initialApprovalItem.PM, SPContext.Current.Web);
            LoadProjectTeamMembers(ProjectManager, initialApprovalItem.PMName, "rptProjectManagers", divProjectManagers);

            // Approval info
            Utilities.SetDropDownValue(initialApprovalItem.SrOBMApproval_Decision, this.ddlInitialApprovalReview, this.Page);
            Utilities.SetDropDownValue(initialApprovalItem.NeedSExpeditedWorkflowWithSGS, this.ddlNeedSExpeditedWorkflowWithSGS, this.Page);

            Utilities.SetDropDownValue(initialApprovalItem.InitialTimeTable, this.drpInitialTimeTable, this.Page);
            this.txtRequestedInformation.Text = initialApprovalItem.SrOBMApproval_Comments;

            ///Timeline
            SetTimeTableValues(initialApprovalItem.InitialTimeTable);
            GetAttachments();
        }

        private void SetTimeTableValues(string timelineType)
        {
            if (timelineType != "Select...")
            {
                DashboardDetailsItem dashboardDetails = timelineNumbers.dashboardDetails(iItemId);
                var projectStatusReportDetails = timelineNumbers.actualTimeLine(dashboardDetails, false);
                txtEstimatedProjectLength.Text = projectStatusReportDetails.Item1[projectStatusReportDetails.Item1.Count - 1].Color.Split(';').ToList<string>()[1].ToString();
                txtFloatDays.Text = projectStatusReportDetails.Item1[projectStatusReportDetails.Item1.Count - 1].ActualDuration.ToString();
            }
        }
        private InitialApprovalReviewItem ConstructFormData(bool Submitting)
        {
            InitialApprovalReviewItem item = new InitialApprovalReviewItem();
            item.CompassListItemId = iItemId;

            #region ProjectManager
            int RepeaterCount = 0;

            Dictionary<string, string> ProjectManagers = ConstructMembers(ref RepeaterCount, "Project Manager", divProjectManagers, "rptProjectManagers", "peProjectManagerMembers", "hdnDeletedStatusForProjectManager", true, Submitting);
            item.PM = ProjectManagers["Member"];
            item.PMName = ProjectManagers["MemberName"];
            #endregion


            item.SrOBMApproval_Decision = this.ddlInitialApprovalReview.SelectedItem.Text;
            item.SrOBMApproval_Comments = this.txtRequestedInformation.Text;
            item.InitialTimeTable = this.drpInitialTimeTable.SelectedItem.Text;
            item.NeedSExpeditedWorkflowWithSGS = this.ddlNeedSExpeditedWorkflowWithSGS.SelectedItem.Text;

            //  item.
            return item;
        }
        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            item.CompassListItemId = iItemId;
            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        #endregion

        #region Dropdown Methods
        protected void drpInitialTimeTable_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
            SetTimeTableValues(drpInitialTimeTable.SelectedItem.Text);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "hideTimeLineDetails();", true);
        }
        #endregion

        #region Button Methods
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Initial Approver Review");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }

        protected void btnAddProjectManager_Click(object sender, EventArgs e)
        {
            AddMembers(divProjectManagers, "rptProjectManagers", "peProjectManagerMembers", "hdnDeletedStatusForProjectManager");
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SrOBMApproval))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                InvalidPeopleEditor = false;

                InitialApprovalReviewItem item = ConstructFormData(false);
                if (InvalidPeopleEditor)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "setFocusError();", true);
                    return;
                }


                initialApprovalService.UpdateInitialApprovalReviewItem(item);

                //bomService.UpsertPackMeasurementsPackTrial(iItemId, drpPackTrial.SelectedItem.Text, ProjectNumber);

                ApprovalItem approvalItem = ConstructApprovalData();

                initialApprovalService.UpdateInitialApprovalReviewApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SrOBMApproval.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.SrOBMApproval.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SrOBMApproval))
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                if (!ValidateForm())
                    return;


                // Retrieve the data from the form
                InvalidPeopleEditor = false;

                InitialApprovalReviewItem item = ConstructFormData(true);

                if (InvalidPeopleEditor)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "setFocusError();", true);
                    return;
                }

                initialApprovalService.UpdateInitialApprovalReviewItem(item);

                // bomService.UpsertPackMeasurementsPackTrial(iItemId, drpPackTrial.SelectedItem.Text, ProjectNumber);

                ApprovalItem approvalItem = ConstructApprovalData();
                if (item.SrOBMApproval_Decision.Equals(GlobalConstants.APPROVER_DECISION_Approved) || item.SrOBMApproval_Decision.Equals(GlobalConstants.APPROVER_DECISION_Rejected))
                {
                    initialApprovalService.UpdateInitialApprovalReviewApprovalItem(approvalItem, true);
                }
                else if (item.SrOBMApproval_Decision.Equals(GlobalConstants.APPROVER_DECISION_RequestIPFupdate))
                {
                    UpdateInitialApprovalDays(approvalItem.CompassListItemId);
                    initialApprovalService.ClearIPFSubmitDateOnRequestForIPFUpdate(approvalItem.CompassListItemId);
                }

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.SrOBMApproval);

                //InsertTimelineItem
                /*DashboardDetailsItem dashboardDetails = timelineNumbers.dashboardDetails(iItemId);
                var projectStatusReportDetails = timelineNumbers.actualTimeLine(dashboardDetails, false);
                List<ProjectStatusReportItem> ActualReportItem = projectStatusReportDetails.Item1;
                List<ProjectStatusReportItem> OriginalReportItem = projectStatusReportDetails.Item2;
                initialApprovalService.insertOriginalTimeline(iItemId, ProjectNumber, OriginalReportItem);*/


                // Redirect to Project Status page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoProjectStatusPage(ProjectNumber), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SrOBMApproval.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.SrOBMApproval.ToString(), "btnSubmit_Click");
            }

        }

        private void UpdateInitialApprovalDays(int CompassListItemId)
        {
            try
            {
                var approvalListItem = initialApprovalService.GetApprovalItem(CompassListItemId);
                int IPF_NumberApproverDays = approvalListItem.IPF_NumberApproverDays;

                DateTime SrOBMApprovalActualStart = GetUpdatedDate(approvalListItem.SrOBMApproval_StartDate);
                DateTime SrOBMApprovalActualEnd = GetUpdatedDate("");
                List<DateTime> holidayList = timelineNumbers.GetHolidays();
                double weekendCount = timelineNumbers.weekends(SrOBMApprovalActualStart, SrOBMApprovalActualEnd, holidayList, false);
                TimeSpan diff = SrOBMApprovalActualEnd - SrOBMApprovalActualStart;
                double duration = diff.Days - weekendCount;
                IPF_NumberApproverDays = IPF_NumberApproverDays + Convert.ToInt32(duration);
                initialApprovalService.UpdateInitialApprovalDays(CompassListItemId, IPF_NumberApproverDays);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SrOBMApproval.ToString() + ": UpdateInitialApprovalDays: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.SrOBMApproval.ToString(), "UpdateInitialApprovalDays"); ;
            }
        }

        private DateTime GetUpdatedDate(string InputDate)
        {
            DateTime UpdatedDate = new DateTime();

            if (String.IsNullOrEmpty(InputDate))
            {
                UpdatedDate = DateTime.Now;
            }
            else
            {
                UpdatedDate = DateTime.Parse(InputDate, ci);
            }
            TimeSpan SrOBMApprovalTime = UpdatedDate.TimeOfDay;

            if (SrOBMApprovalTime <= TimeSpan.Parse("07:59:00"))
            {
                TimeSpan ts = new TimeSpan(8, 0, 0);
                UpdatedDate = UpdatedDate.Date + ts;
            }
            else if (SrOBMApprovalTime >= TimeSpan.Parse("15:59:00"))
            {
                //dayHolder = 1;
                UpdatedDate = UpdatedDate.AddDays(1);
                TimeSpan ts = new TimeSpan(8, 0, 0);
                UpdatedDate = UpdatedDate.Date + ts;
            }
            if ((int)UpdatedDate.DayOfWeek == 0)//If Sunday Add 1 day
            {
                UpdatedDate = UpdatedDate.AddDays(1);
            }
            else if ((int)UpdatedDate.DayOfWeek == 6)//If Saturday Add 2 days
            {
                UpdatedDate = UpdatedDate.AddDays(2);
            }

            return UpdatedDate;
        }

        protected void lnkDeleteApprovedGraphicsAsset_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }
        #endregion
        private Dictionary<string, string> ConstructMembers(ref int repeaterCount, string MemberName, HtmlGenericControl div, string RepeaterName, string PeopleEditorName, string HiddenStatusFieldName, bool Required, bool Submitting)
        {
            Repeater rptMembers = ((Repeater)div.FindControl(RepeaterName));
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            bool NA;
            string NAText;

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMember = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        try
                        {
                            if (peMember.Entities.Count <= 0 && Required && Submitting)
                            {
                                InvalidPeopleEditor = true;
                                string strErrors = "Please enter " + MemberName + " member.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                                ErrorSummary.AddError(strErrors, this.Page);
                            }

                            if (peMember.Entities.Count > 0)
                            {
                                CheckForNA(out NA, out NAText, peMember);

                                if (NA)
                                {
                                    MembersNames += NAText + ";";
                                }
                                else
                                {
                                    Members.AddRange(Utilities.GetPeopleFromPickerControl(peMember, SPContext.Current.Web));
                                    MembersNames += Utilities.GetNamesFromPickerControl(peMember, SPContext.Current.Web) + ";";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            InvalidPeopleEditor = true;
                            string strErrors = "Invalid " + MemberName + " member was entered. Please re-enter. <a href ='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": " + strErrors);
                            exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "ConstructFormData", strErrors);
                        }
                        repeaterCount++;
                    }
                }
            }

            return new Dictionary<string, string>()
            {
                { "Member", Members.ToString() },
                { "MemberName",MembersNames }
            };
        }
        private string LoadProjectTeamMembers(string Members, string MemberNames, string RepeaterName, HtmlGenericControl div)
        {
            List<string> listTeamMemberNames = new List<string>();
            List<int> NAList = new List<int>();

            if (!string.IsNullOrEmpty(MemberNames))
            {
                MemberNames = MemberNames.Remove(MemberNames.LastIndexOf(";"), 1);
                listTeamMemberNames = MemberNames.Split(';').ToList();

                for (int i = 0; i < listTeamMemberNames.Count; i++)
                {
                    if (CheckForNA(listTeamMemberNames[i])) NAList.Add(i);
                }
            }

            List<string> listTeamMembers = new List<string>();
            if (!string.IsNullOrEmpty(Members))
            {
                Members = Members.Remove(Members.LastIndexOf(","), 1);
                listTeamMembers = Members.Split(',').ToList();
            }

            for (int i = 0; i < NAList.Count; i++)
            {
                listTeamMembers.Insert(NAList[i], "NA");
            }

            if (listTeamMembers.Count == 0)
            {
                listTeamMembers.Add(string.Empty);
            }

            Repeater Repeater = (Repeater)div.FindControl(RepeaterName);
            Repeater.DataSource = listTeamMembers;
            Repeater.DataBind();
            return Members;
        }
        private void CheckForNA(out bool NA, out string NAText, PeopleEditor peopleEditor)
        {
            NA = false;
            NAText = string.Empty;
            foreach (PickerEntity entity in peopleEditor.Entities)
            {
                if (entity.Key.ToUpper() == "NA" || entity.Key.ToUpper() == "NOT APPLICABLE" || entity.Key.ToUpper() == "N/A")
                {
                    NAText = entity.Key.ToUpper();
                    NA = true;
                }
                if (entity.DisplayText.ToUpper().Trim() == "NA" || entity.DisplayText.ToUpper().Trim() == "NOT APPLICABLE" || entity.DisplayText.ToUpper().Trim() == "N/A")
                {
                    NAText = entity.DisplayText.ToUpper();
                    NA = true;
                }
            }
        }
        private static bool CheckForNA(string stateGateItemName)
        {
            return (stateGateItemName.ToUpper() == "NA" || stateGateItemName.ToUpper() == "NOT APPLICABLE" || stateGateItemName.ToUpper() == "N/A");
        }
        private void LoadBlankProjectTeamber()
        {
            //ProjectManager
            LoadProjectTeamMembers(string.Empty, string.Empty, "rptProjectManagers", divProjectManagers);
        }
        private bool ValidateMembers(ref int Count, string MemberName, HtmlGenericControl div, string RepeaterName, string PeopleEditorName, string HiddenStatusFieldName, bool Required = true)
        {
            bool bValid = true;
            Repeater rptMembers = ((Repeater)div.FindControl(RepeaterName));
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Memebers = new SPFieldUserValueCollection();

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMember = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        if (peMember.Entities.Count <= 0 && Required)
                        {
                            string strErrors = MemberName + " member must be set.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + Count.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            bValid = false;
                        }
                    }
                }
                Count++;
            }
            return bValid;
        }

        private void AddMembers(HtmlGenericControl div, string repeaterName, string PeopleEditorName, string hiddenStatusFieldName)
        {
            Repeater rptMembers = ((Repeater)div.FindControl(repeaterName));
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            bool NA;
            string NAText;
            List<int> NAList = new List<int>();
            Dictionary<int, string> BadNamesList = new Dictionary<int, string>();
            int Counter = 0;

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMembers = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField hiddenStatusField = (HiddenField)item.FindControl(hiddenStatusFieldName);

                    if (hiddenStatusField.Value != "true")
                    {
                        try
                        {
                            if (peMembers.Entities.Count > 0)
                            {
                                CheckForNA(out NA, out NAText, peMembers);

                                if (NA)
                                {
                                    NAList.Add(Counter);
                                }
                                else
                                {
                                    Members.AddRange(Utilities.GetPeopleFromPickerControl(peMembers, SPContext.Current.Web));
                                }
                            }
                            else
                            {
                                Members.Add(new SPFieldUserValue());
                            }
                        }
                        catch (Exception ex)
                        {
                            string BadText;
                            foreach (PickerEntity entity in peMembers.Entities)
                            {
                                BadText = entity.Key;
                                BadText = string.IsNullOrEmpty(BadText) ? entity.DisplayText : BadText;
                                BadNamesList.Add(Counter, BadText);
                            }
                        }
                    }
                }
                Counter++;
            }

            string users = "";
            List<string> listpeMembers = new List<string>();

            foreach (SPFieldUserValue Member in Members)
            {
                if (Member.User == null)
                {
                    users += ",";
                }
                else
                {
                    users += Member.User.ToString() + ",";
                }
            }

            if (string.IsNullOrEmpty(users))
            {
                listpeMembers.Add(string.Empty);
            }
            else
            {
                listpeMembers = users.Split(',').ToList();
            }

            foreach (var index in NAList)
            {
                listpeMembers.Insert(index, "NA");
            }

            foreach (var BadValue in BadNamesList)
            {
                listpeMembers.Insert(BadValue.Key, BadValue.Value);
            }
            rptMembers.DataSource = listpeMembers;
            rptMembers.DataBind();
        }

        protected void rptProjectManagers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peProjectManagerMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }

        private void CallUpdatePeopleEditorScriptFunction()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "updatePeopleEditors();", true);
        }
        private void GetAttachments()
        {
            #region Approved Graphics Asset
            var ApprovedGraphicsAssets = BOMSetupService.GetUploadedFiles(ProjectNumber, GlobalConstants.DOCTYPE_ApprovedGraphicsAsset);

            if (ApprovedGraphicsAssets.Count > 0)
            {
                //btnApprovedGraphicsAsset.Visible = false;
                rptApprovedGraphicsAsset.Visible = true;
                rptApprovedGraphicsAsset.DataSource = ApprovedGraphicsAssets;
                rptApprovedGraphicsAsset.DataBind();
            }
            else
            {
                rptApprovedGraphicsAsset.Visible = false;
                //btnApprovedGraphicsAsset.Visible = true;
            }
            #endregion
        }
    }
}
