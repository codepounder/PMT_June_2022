using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using System;
using System.ComponentModel;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Drawing;

namespace Ferrara.Compass.WebParts.StageGateProjectPanelForm
{
    [ToolboxItemAttribute(false)]
    public partial class StageGateProjectPanelForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private IStageGateGeneralService stageGateGeneralService;
        private IStageGateFinancialServices sgsFinancialService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private IExceptionService exceptionService;
        private INotificationService notificationService;
        private IConfigurationManagementService configurationService;
        private IDashboardService dashboardservice;
        private const string _ucGateDets = @"~/_controltemplates/15/Ferrara.Compass/ucGateDets.ascx";
        private const string _ucSGSProjectInformation = @"~/_controltemplates/15/Ferrara.Compass/ucSGSProjectInformation.ascx";
        private int StageGateProjectItemId = 0;
        private string webUrl;
        private List<FileAttribute> files = new List<FileAttribute>();
        #endregion
        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        #endregion
        #region Constructor
        public StageGateProjectPanelForm()
        {

        }
        #endregion
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            stageGateCreateProjectService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            sgsFinancialService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateFinancialServices>();
            dashboardservice = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
        }
        #endregion
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;

                if (StageGateProjectItemId > 0)
                {
                    LoadFormData();
                }
            }
            else
            {
                StageGateProjectItemId = Convert.ToInt32(hdnStageGateProjectListItemId.Value);
            }

            hdnStageGateProjectListItemId.Value = StageGateProjectItemId.ToString();
            hdnCompassListItemId.Value = StageGateProjectItemId.ToString();
            hdnProjectNumber.Value = ProjectNumber;
            hdnPageName.Value = GlobalConstants.PAGE_ItemProposal;
            LoadProjectInformation();
            bool ShowButtons = CheckUserPrivileges(StageGateProjectItemId);
            divCancelProject.Visible = ShowButtons;
            divOnHold.Visible = ShowButtons;
        }
        #endregion
        #region Private Methods
        private bool CheckProjectNumber()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                StageGateProjectItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);
            }

            // Store Id in Hidden field
            this.hdnStageGateProjectListItemId.Value = StageGateProjectItemId.ToString();
            return true;
        }
        private bool CheckWriteAccess()
        {
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_StageGateProjectPanel.ToLower()))
            {
                if (userManagementService.HasWriteAccess(CompassForm.StageGateProjectPanel))
                {
                    return true;
                }
            }
            return false;
        }

        private Boolean ValidateForm()
        {
            Boolean bValid = true;


            return bValid;
        }
        #endregion
        #region Data Transfer Methods
        private void LoadFormData()
        {
            StageGateCreateProjectItem stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateProjectItemId);
            lblOnHoldButtonMessage.Visible = false;
            lblOnHoldButtonMessage.Text = "";
            if (stageGateItem.Stage == GlobalConstants.WORKFLOWPHASE_OnHold)
            {
                btnOnHoldProject.Visible = false;
                btnRemoveOnHold.Visible = true;
                // Check if all "On Hold" are complete.
                if (stageGateGeneralService.CheckPMTWorkflowUpdateStatuses(StageGateProjectItemId, "OnHold"))
                {
                    btnRemoveOnHold.Enabled = false;
                    lblOnHoldButtonMessage.Visible = true;
                    lblOnHoldButtonMessage.Text = "Child projects are still being put On-Hold.";
                }
                else
                {
                    btnOnHoldProject.Enabled = true;
                }
            }
            else
            {
                btnOnHoldProject.Visible = true;
                btnRemoveOnHold.Visible = false;
                btnOnHoldProject.Enabled = false;
                if (stageGateItem.Stage.ToLower() != "completed" && stageGateItem.Stage.ToLower() != "cancelled")
                {
                    // Check if all "Relase On Hold" are complete.
                    if (stageGateGeneralService.CheckPMTWorkflowUpdateStatuses(StageGateProjectItemId, "ReleaseOnHold"))
                    {
                        lblOnHoldButtonMessage.Visible = true;
                        lblOnHoldButtonMessage.Text = "Child projects are still being released from On-Hold.";
                    }
                    else
                    {
                        btnOnHoldProject.Enabled = true;
                    }
                }
            }

            try
            {
                if (!string.Equals(stageGateItem.ProjectType, GlobalConstants.PROJECTTYPE_SimpleNetworkMove))
                {
                    List<int> gates = new List<int>() { 1, 2, 3 };
                    rptGateDetails.DataSource = gates;
                    rptGateDetails.DataBind();
                }
                else
                {
                    phRASection.Visible = false;
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateProjectPanel.ToString() + ": Gates: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateProjectPanel.ToString(), "Gates");
            }
            try
            {

                txtRevisedFirstShipDate.Text = stageGateItem.RevisedShipDate == DateTime.MinValue ? stageGateItem.DesiredShipDate.ToString("MM/dd/yyyy") : stageGateItem.RevisedShipDate.ToString("MM/dd/yyyy");

                StageGateGateItem gate1 = stageGateGeneralService.GetStageGateGateItem(StageGateProjectItemId, 1);
                StageGateGateItem gate2 = stageGateGeneralService.GetStageGateGateItem(StageGateProjectItemId, 2);
                StageGateGateItem gate3 = stageGateGeneralService.GetStageGateGateItem(StageGateProjectItemId, 3);
                StageGateGateItem gate4 = stageGateGeneralService.GetStageGateGateItem(StageGateProjectItemId, 4);
                StageGateGateItem gate5 = stageGateGeneralService.GetStageGateGateItem(StageGateProjectItemId, 5);
                List<KeyValuePair<DateTime, string>> gate1Finance = sgsFinancialService.GetAllStageGateCreatedFinancialSummaryItems(StageGateProjectItemId, "1");
                List<KeyValuePair<DateTime, string>> gate2Finance = sgsFinancialService.GetAllStageGateCreatedFinancialSummaryItems(StageGateProjectItemId, "2");
                List<KeyValuePair<DateTime, string>> gate3Finance = sgsFinancialService.GetAllStageGateCreatedFinancialSummaryItems(StageGateProjectItemId, "3");
                gate1Finance = gate1Finance.OrderBy(x => x.Key).ToList();
                gate2Finance = gate2Finance.OrderBy(x => x.Key).ToList();
                gate3Finance = gate3Finance.OrderBy(x => x.Key).ToList();
                if (stageGateItem != null)
                {
                    hdnPageName.Value = Utilities.GetCurrentPageName();
                    hdnStageGateProjectListItemId.Value = StageGateProjectItemId.ToString();
                }
                List<StageGateProjectSummaryItem> projectSummaryItem = new List<StageGateProjectSummaryItem>();

                StageGateProjectSummaryItem gate0 = new StageGateProjectSummaryItem();
                gate0.EndDate = stageGateItem.Gate0ApprovedDate.ToString();
                gate0.EndDateList = gate0Versions(StageGateProjectItemId, 0, StageGateProjectListFields.Gate0ApprovedDate, GlobalConstants.LIST_StageGateProjectListName, StageGateProjectListFields.ProjectLeaderName);
                gate0.Gate = 0;
                gate0.PhaseLabel = "";
                gate0.Stage = "Gate 0 Approved";
                gate0.StartDate = gate0.EndDateList[gate0.EndDateList.Count - 1].Key;
                gate0.URL = "";
                gate0.URLText = "Gate 0 Approved";
                gate0.Submitter = StageGateProjectListFields.ProjectLeader;
                projectSummaryItem.Add(gate0);

                StageGateProjectSummaryItem projectInfo = new StageGateProjectSummaryItem();
                projectInfo.EndDate = stageGateItem.FormSubmittedDate.ToString();
                projectInfo.EndDateList = submittedVersions(StageGateProjectItemId, 0, StageGateProjectListFields.FormSubmittedDate, GlobalConstants.LIST_StageGateProjectListName, StageGateProjectListFields.FormSubmittedBy);
                projectInfo.Gate = 0;
                projectInfo.PhaseLabel = "";
                projectInfo.Stage = "Project Information";
                projectInfo.StartDate = stageGateItem.CreatedDate;
                projectInfo.URL = "/Pages/StageGateCreateProject.aspx?ProjectNo=";
                projectInfo.URLText = "Project Information";
                projectInfo.Submitter = StageGateProjectListFields.FormSubmittedBy;
                projectSummaryItem.Add(projectInfo);

                StageGateProjectSummaryItem Designitem = new StageGateProjectSummaryItem();
                Designitem.EndDate = gate1.FormSubmittedDate;
                Designitem.EndDateList = submittedVersions(StageGateProjectItemId, 1, PMTRiskAssessmentFIelds.SubmittedDate, GlobalConstants.LIST_PMTRAListName, PMTRiskAssessmentFIelds.SubmittedBy);
                Designitem.Gate = 1;
                Designitem.PhaseLabel = "Design";
                Designitem.Stage = "Design";
                Designitem.StartDate = projectInfo.EndDateList[projectInfo.EndDateList.Count - 1].Key;
                Designitem.URL = "/Pages/StageGateDesignDeliverables.aspx?ProjectNo=";
                Designitem.URLText = "Design Checklist - Gate 1 Readiness";
                projectSummaryItem.Add(Designitem);

                StageGateProjectSummaryItem DesignitemFG = new StageGateProjectSummaryItem();
                DesignitemFG.EndDate = gate1.FormSubmittedDate;
                DesignitemFG.EndDateList = Designitem.EndDateList;
                DesignitemFG.Gate = 1;
                DesignitemFG.PhaseLabel = "skip";
                DesignitemFG.Stage = "Design";
                DesignitemFG.StartDate = gate1Finance.Count <= 0 ? DateTime.MinValue : gate1Finance[gate1Finance.Count - 1].Key;
                DesignitemFG.URL = "/Pages/StageGateFinancialSummary.aspx?ProjectNo=";
                DesignitemFG.URLText = "Finance - Gate 1 Readiness";
                projectSummaryItem.Add(DesignitemFG);

                StageGateProjectSummaryItem DevelopItem = new StageGateProjectSummaryItem();
                DevelopItem.EndDate = gate2.FormSubmittedDate;
                DevelopItem.EndDateList = submittedVersions(StageGateProjectItemId, 2, PMTRiskAssessmentFIelds.SubmittedDate, GlobalConstants.LIST_PMTRAListName, PMTRiskAssessmentFIelds.SubmittedBy);
                DevelopItem.Gate = 2;
                DevelopItem.PhaseLabel = "Develop/Validate";
                DevelopItem.Stage = "Develop";
                DevelopItem.StartDate = Designitem.EndDateList[Designitem.EndDateList.Count - 1].Key;
                DevelopItem.URL = "/Pages/StageGateDevelopDeliverables.aspx?ProjectNo=";
                DevelopItem.URLText = "Develop/Validate Checklist - Gate 2 Readiness";
                projectSummaryItem.Add(DevelopItem);

                StageGateProjectSummaryItem DesignItemFG = new StageGateProjectSummaryItem();
                DesignItemFG.EndDate = gate2.FormSubmittedDate;
                DesignItemFG.EndDateList = DevelopItem.EndDateList;
                DesignItemFG.Gate = 2;
                DesignItemFG.PhaseLabel = "skip";
                DesignItemFG.Stage = "Design";
                DesignItemFG.StartDate = gate2Finance.Count <= 0 ? DateTime.MinValue : gate2Finance[gate2Finance.Count - 1].Key;
                DesignItemFG.URL = "/Pages/StageGateFinancialSummary.aspx?ProjectNo=";
                DesignItemFG.URLText = "Finance - Gate 2 Readiness";
                projectSummaryItem.Add(DesignItemFG);

                StageGateProjectSummaryItem IPFItem = new StageGateProjectSummaryItem();
                IPFItem.EndDate = DateTime.MinValue.ToString();
                IPFItem.EndDateList = new List<KeyValuePair<DateTime, string>>() { (new KeyValuePair<DateTime, string>(DateTime.MinValue, "")) };
                IPFItem.Gate = 3;
                IPFItem.PhaseLabel = "Item Proposal Form";
                IPFItem.Stage = "IPF Phase";
                if (stageGateItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    IPFItem.StartDate = projectInfo.EndDateList[projectInfo.EndDateList.Count - 1].Key; ;
                }
                else
                {
                    IPFItem.StartDate = string.IsNullOrEmpty(stageGateItem.IPFStartDate) ? DateTime.MinValue : Convert.ToDateTime(stageGateItem.IPFStartDate);
                }
                IPFItem.Submitter = stageGateItem.IPFSubmitter;
                IPFItem.URL = "/Pages/StageGateGenerateIPFs.aspx?ProjectNo=";
                IPFItem.URLText = "Generate IPFs";
                projectSummaryItem.Add(IPFItem);

                StageGateProjectSummaryItem IndustrializeItem = new StageGateProjectSummaryItem();
                IndustrializeItem.EndDate = gate3.FormSubmittedDate;
                IndustrializeItem.EndDateList = submittedVersions(StageGateProjectItemId, 3, PMTRiskAssessmentFIelds.SubmittedDate, GlobalConstants.LIST_PMTRAListName, PMTRiskAssessmentFIelds.SubmittedBy);
                IndustrializeItem.Gate = 3;
                IndustrializeItem.PhaseLabel = "Industrialize";
                IndustrializeItem.Stage = "Industrialize";
                IndustrializeItem.StartDate = DevelopItem.EndDateList[DevelopItem.EndDateList.Count - 1].Key;
                IndustrializeItem.URL = "/Pages/StageGateIndustrializeDeliverables.aspx?ProjectNo=";
                IndustrializeItem.URLText = "Industrialize Checklist - Gate 3 Readiness";
                projectSummaryItem.Add(IndustrializeItem);

                StageGateProjectSummaryItem IndustrializeItemFG = new StageGateProjectSummaryItem();
                IndustrializeItemFG.EndDate = gate3.FormSubmittedDate;
                IndustrializeItemFG.EndDateList = IndustrializeItem.EndDateList;
                IndustrializeItemFG.Gate = 3;
                IndustrializeItemFG.PhaseLabel = "skip";
                IndustrializeItemFG.Stage = "Industrialize";
                IndustrializeItemFG.StartDate = gate3Finance.Count <= 0 ? DateTime.MinValue : gate3Finance[gate3Finance.Count - 1].Key;
                IndustrializeItemFG.URL = "/Pages/StageGateFinancialSummary.aspx?ProjectNo=";
                IndustrializeItemFG.URLText = "Finance - Gate 3 Readiness";
                projectSummaryItem.Add(IndustrializeItemFG);

                StageGateProjectSummaryItem LaunchItem = new StageGateProjectSummaryItem();
                LaunchItem.EndDate = gate4.FormSubmittedDate;
                LaunchItem.EndDateList = submittedVersions(StageGateProjectItemId, 4, PMTRiskAssessmentFIelds.SubmittedDate, GlobalConstants.LIST_PMTRAListName, PMTRiskAssessmentFIelds.SubmittedBy);
                LaunchItem.Gate = 4;
                LaunchItem.PhaseLabel = "Launch";
                LaunchItem.Stage = "Launch";
                LaunchItem.StartDate = IndustrializeItem.EndDateList[IndustrializeItem.EndDateList.Count - 1].Key;
                LaunchItem.URL = "/Pages/StageGateLaunchDeliverables.aspx?ProjectNo=";
                LaunchItem.URLText = "Launch Checklist";
                projectSummaryItem.Add(LaunchItem);

                var postLaunchActive = stageGateItem.PostLaunchActive;
                DateTime PLStartDate = DateTime.MinValue;
                string PLEndDate = "";
                List<KeyValuePair<DateTime, string>> PLEndDateList = new List<KeyValuePair<DateTime, string>>();
                if (postLaunchActive == "Yes")
                {
                    PLStartDate = LaunchItem.EndDateList[LaunchItem.EndDateList.Count - 1].Key;
                    PLEndDate = string.IsNullOrEmpty(gate5.FormSubmittedDate) ? DateTime.MinValue.ToString() : gate5.FormSubmittedDate;
                    PLEndDateList = submittedVersions(StageGateProjectItemId, 5, PMTRiskAssessmentFIelds.SubmittedDate, GlobalConstants.LIST_PMTRAListName, PMTRiskAssessmentFIelds.SubmittedBy);
                }
                if (stageGateItem.PostLaunchActive == "Yes")
                {
                    StageGateProjectSummaryItem item7 = new StageGateProjectSummaryItem();
                    item7.EndDate = PLEndDate;
                    item7.EndDateList = PLEndDateList;
                    item7.Gate = 5;
                    item7.PhaseLabel = "Post Launch";
                    item7.Stage = "Post Launch";
                    item7.StartDate = PLStartDate;
                    item7.URL = "/Pages/StageGatePostLaunchDeliverables.aspx?ProjectNo=";
                    item7.URLText = "Post Launch Checklist";
                    projectSummaryItem.Add(item7);
                }
                test(projectSummaryItem, stageGateItem);

                LoadGateAttachments();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateProjectPanel.ToString() + ": Project Summary: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateProjectPanel.ToString(), "Project Summary");
            }
        }
        private void LoadProjectInformation()
        {
            //Project Information
            ucSGSProjectInformation ctrl2 = (ucSGSProjectInformation)Page.LoadControl(_ucSGSProjectInformation);
            ctrl2.StageGateItemId = StageGateProjectItemId;
            SGSProjectInformation.Controls.Add(ctrl2);
        }
        protected void rptGateDetails_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int gate = (int)e.Item.DataItem;
                PlaceHolder phMsg = (PlaceHolder)e.Item.FindControl("phGateInfo");
                ucGateDets ctrl2 = (ucGateDets)Page.LoadControl(_ucGateDets);
                ctrl2.StageGateItemId = StageGateProjectItemId;
                ctrl2.ProjectNumber = ProjectNumber;
                ctrl2.Gate = gate;
                ctrl2.readOnly = false;
                phMsg.Controls.Add(ctrl2);
            }
        }
        protected void rptGateDetails_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        #endregion
        public static double weekends(DateTime start, DateTime end, List<DateTime> holidays)
        {
            double days = 0;

            DateTime tempEnd = end;
            TimeSpan startEnd = end - start;
            double currentDays = startEnd.Days;
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                int dw = (int)date.DayOfWeek;
                if (dw == 0 || dw == 6)
                {
                    days++;
                }

            }
            int exclusionDays = 0;
            int exclWEOld = 0;
            int exclHDOld = 0;
            while (currentDays >= 0)
            {
                int excHolidayTest = exclHolidays(start, tempEnd, holidays);
                int excDayTest = exclDays(start, tempEnd);
                if (exclusionDays == (excDayTest + excHolidayTest))
                {
                    break;
                }
                else
                {
                    exclusionDays = excDayTest + excHolidayTest;
                    if (exclWEOld == 0 && exclHDOld == 0)
                    {
                        tempEnd = tempEnd.AddDays((excDayTest + excHolidayTest));
                    }
                    else
                    {
                        tempEnd = tempEnd.AddDays((excDayTest - exclWEOld));
                        tempEnd = tempEnd.AddDays((excHolidayTest - exclHDOld));
                    }
                    exclWEOld = excDayTest;
                    exclHDOld = excHolidayTest;

                }
                days = excDayTest + excHolidayTest;
                currentDays--;
            }

            return days;
        }
        public static int exclDays(DateTime start, DateTime end)
        {
            int exclusionDays = 0;
            for (DateTime date = start; date <= end; date = date.AddDays(1))
            {
                int dw = (int)date.DayOfWeek;
                if (dw == 0 || dw == 6)
                {
                    exclusionDays++;
                }

            }
            return exclusionDays;
        }
        public static int exclHolidays(DateTime start, DateTime end, List<DateTime> holidays)
        {
            int exclusionHolidays = 0;

            foreach (DateTime holiday in holidays.Where(r => r.Date >= start.Date && r.Date <= end.Date))
            {
                exclusionHolidays++;
            }

            return exclusionHolidays;
        }
        private void test(List<StageGateProjectSummaryItem> list, StageGateCreateProjectItem ProjectInfo)
        {
            foreach (StageGateProjectSummaryItem listItem in list)
            {
                string EndDate = "";
                string Submitter = "";
                string startDate = "";
                List<DateTime> holidaysList = new List<DateTime>();

                double weekendCount = 0;
                double duration = 0;
                string status = "";
                TimeSpan diff = new TimeSpan();
                DateTime finalDate = new DateTime();
                TableRow tr5 = new TableRow();
                TableCell tdSpace12 = new TableCell();
                TableCell tdSpace4 = new TableCell();
                TableCell tdSpace5 = new TableCell();
                tdSpace4.CssClass = "spacingCol";
                TableCell tdSpace13 = new TableCell();
                tdSpace5.CssClass = "spacingCol";
                TableCell ProcessCell5 = new TableCell();
                TableCell StatusCell5 = new TableCell();
                TableCell StartCell5 = new TableCell();
                TableCell EndCell5 = new TableCell();
                TableCell SubmittedByCell5 = new TableCell();
                TableCell DurationCell5 = new TableCell();
                if (!string.IsNullOrEmpty(listItem.URL))
                {
                    HyperLink taskForm3 = new HyperLink();
                    if (listItem.PhaseLabel == "skip")
                    {
                        taskForm3.NavigateUrl = listItem.URL + ProjectNumber + "&Gate=" + listItem.Gate;
                    }
                    else
                    {
                        taskForm3.NavigateUrl = listItem.URL + ProjectNumber;
                    }
                    taskForm3.Text = listItem.URLText;
                    ProcessCell5.Controls.Add(taskForm3);
                }
                else
                {
                    LiteralControl taskForm3 = new LiteralControl();
                    taskForm3.Text = listItem.URLText;
                    ProcessCell5.Controls.Add(taskForm3);
                }
                if (listItem.EndDateList[0].Key == DateTime.MinValue)
                {
                    if (listItem.StartDate != DateTime.MinValue)
                    {
                        StartCell5.Controls.Add(new LiteralControl(listItem.StartDate.ToString("MM/dd/yyyy")));
                        weekendCount = weekends(listItem.StartDate, DateTime.Now, holidaysList);
                        diff = DateTime.Now - listItem.StartDate;
                        duration = diff.Days - weekendCount;
                        status = "Active";
                        startDate = listItem.StartDate.ToString("MM/dd/yyyy");
                    }
                    else
                    {
                        status = "Waiting";
                        startDate = "";
                    }
                    EndDate = "";
                }
                else
                {
                    if (listItem.EndDateList[0].Key != DateTime.MinValue)
                    {
                        string endDays = "";
                        string submitters = "";
                        if (listItem.StartDate != DateTime.MinValue)
                        {
                            StartCell5.Controls.Add(new LiteralControl(listItem.StartDate.ToString("MM/dd/yyyy")));

                            int checkDates = DateTime.Compare(listItem.StartDate, listItem.EndDateList[0].Key);
                            if (checkDates > 0)
                            {
                                status = "Active";
                                startDate = listItem.StartDate.ToString("MM/dd/yyyy");
                                EndDate = "";
                            }
                            else
                            {
                                if (listItem.EndDateList.Count > 0 || string.IsNullOrEmpty(listItem.Submitter))
                                {
                                    foreach (KeyValuePair<DateTime, string> versionDets in listItem.EndDateList)
                                    {
                                        int checkSpecificDates = DateTime.Compare(listItem.StartDate, versionDets.Key);
                                        if (checkSpecificDates <= 0)
                                        {
                                            endDays += versionDets.Key + "<br />";
                                            submitters += utilityService.GetPersonFieldForDisplayUpdated(versionDets.Value) + "<br />";
                                            submitters = submitters.Replace("#", "");
                                            EndDate = versionDets.Key.ToString("MM/dd/yyyy");
                                            Submitter = utilityService.GetPersonFieldForDisplayUpdated(versionDets.Value);
                                            Submitter = Submitter.Replace("#", "");
                                            finalDate = versionDets.Key;
                                        }
                                    }
                                }
                                else if (!string.IsNullOrEmpty(listItem.Submitter))
                                {
                                    submitters = utilityService.GetPersonFieldForDisplay(listItem.Submitter);
                                }
                                weekendCount = weekends(listItem.StartDate, finalDate, holidaysList);
                                diff = finalDate - listItem.StartDate;
                                duration = diff.Days - weekendCount;
                                EndCell5.Controls.Add(new LiteralControl(endDays));
                                SubmittedByCell5.Controls.Add(new LiteralControl(submitters));
                                status = "Completed";
                                startDate = listItem.StartDate.ToString("MM/dd/yyyy");
                            }
                        }
                        else if (listItem.StartDate == DateTime.MinValue && listItem.PhaseLabel != "skip")
                        {
                            if (listItem.EndDateList.Count > 0 || string.IsNullOrEmpty(listItem.Submitter))
                            {
                                foreach (KeyValuePair<DateTime, string> versionDets in listItem.EndDateList)
                                {
                                    endDays += versionDets.Key + "<br />";
                                    submitters += utilityService.GetPersonFieldForDisplayUpdated(versionDets.Value) + "<br />";
                                    submitters = submitters.Replace("#", "");
                                    EndDate = versionDets.Key.ToString("MM/dd/yyyy");
                                    Submitter = utilityService.GetPersonFieldForDisplayUpdated(versionDets.Value);
                                    Submitter = Submitter.Replace("#", "");
                                    finalDate = versionDets.Key;
                                }
                            }
                            else if (!string.IsNullOrEmpty(listItem.Submitter))
                            {
                                submitters = utilityService.GetPersonFieldForDisplay(listItem.Submitter);
                            }
                            EndCell5.Controls.Add(new LiteralControl(endDays));
                            SubmittedByCell5.Controls.Add(new LiteralControl(submitters));
                            status = "Completed";
                            startDate = "";
                        }
                        else if (listItem.StartDate == DateTime.MinValue && listItem.PhaseLabel == "skip")
                        {
                            status = "Waiting";
                            startDate = "";
                            EndDate = "";
                        }
                    }
                    else
                    {
                        if (listItem.StartDate != DateTime.MinValue)
                        {
                            StartCell5.Controls.Add(new LiteralControl(listItem.StartDate.ToString("MM/dd/yyyy")));
                            weekendCount = weekends(listItem.StartDate, DateTime.Now, holidaysList);
                            diff = DateTime.Now - listItem.StartDate;
                            duration = diff.Days - weekendCount;
                            status = "Active";
                            startDate = listItem.StartDate.ToString("MM/dd/yyyy");
                            EndDate = "";
                        }
                        else
                        {
                            status = "Waiting";
                            startDate = "";
                            EndDate = "";
                        }
                    }
                }
                string developPhaseStatus = status;
                string developPhaseEnd = EndDate;
                StatusCell5.Controls.Add(new LiteralControl(status));
                DurationCell5.Controls.Add(new LiteralControl(""));
                tr5.Cells.Add(ProcessCell5);
                tr5.Cells.Add(tdSpace12);
                tr5.Cells.Add(StatusCell5);
                tr5.Cells.Add(tdSpace13);
                tr5.Cells.Add(StartCell5);
                tr5.Cells.Add(EndCell5);
                tr5.Cells.Add(SubmittedByCell5);
                tr5.Cells.Add(DurationCell5);

                if (ProjectInfo.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
                {
                    if (listItem.Stage == "Project Information" || listItem.Stage == "IPF Phase")
                    {
                        projectSummary.Rows.Add(tr5);
                    }
                }
                else if (ProjectInfo.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    if (listItem.Stage != "Design" && listItem.Stage != "Develop")
                    {
                        projectSummary.Rows.Add(tr5);
                    }
                }
                else
                {
                    projectSummary.Rows.Add(tr5);
                }
                //Develop Phase
                if (listItem.Gate != 0 && listItem.PhaseLabel != "skip")
                {
                    TableRow tr6 = new TableRow();
                    tr6.CssClass = "stageHeader";
                    TableCell tdSpace10 = new TableCell();
                    tdSpace10.CssClass = "spacingCol";
                    TableCell tdSpace11 = new TableCell();
                    tdSpace11.CssClass = "spacingCol";
                    TableCell ProcessCell6 = new TableCell();
                    TableCell StatusCell6 = new TableCell();
                    TableCell StartCell6 = new TableCell();
                    TableCell EndCell6 = new TableCell();
                    TableCell SubmittedByCell6 = new TableCell();
                    TableCell DurationCell6 = new TableCell();

                    ProcessCell6.Controls.Add(new LiteralControl(listItem.PhaseLabel));
                    StartCell6.Controls.Add(new LiteralControl(startDate));
                    EndCell6.Controls.Add(new LiteralControl(EndDate));
                    SubmittedByCell6.Controls.Add(new LiteralControl(Submitter));
                    StatusCell6.Controls.Add(new LiteralControl(status));
                    if (listItem.StartDate != DateTime.MinValue)
                    {
                        weekendCount = weekends(listItem.StartDate, finalDate, holidaysList);
                        diff = finalDate - listItem.StartDate;
                        duration = diff.Days - weekendCount;
                        if (duration < 0)
                        {
                            duration = 0;
                        }
                        DurationCell6.Controls.Add(new LiteralControl(duration.ToString()));
                    }
                    else
                    {
                        DurationCell6.Controls.Add(new LiteralControl(""));
                    }


                    tr6.Cells.Add(ProcessCell6);
                    tr6.Cells.Add(tdSpace10);
                    tr6.Cells.Add(StatusCell6);
                    tr6.Cells.Add(tdSpace11);
                    tr6.Cells.Add(StartCell6);
                    tr6.Cells.Add(EndCell6);
                    tr6.Cells.Add(SubmittedByCell6);
                    tr6.Cells.Add(DurationCell6);
                    if (ProjectInfo.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
                    {
                        if (listItem.Stage == "Project Information" || listItem.Stage == "IPF Phase")
                        {
                            projectSummary.Rows.AddAt(projectSummary.Rows.Count - 1, tr6);
                        }
                    }
                    else if (ProjectInfo.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                    {
                        if (listItem.Stage != "Design" && listItem.Stage != "Develop")
                        {
                            projectSummary.Rows.AddAt(projectSummary.Rows.Count - 1, tr6);
                        }
                    }
                    else
                    {
                        projectSummary.Rows.AddAt(projectSummary.Rows.Count - 1, tr6);
                    }
                }
            }
        }
        private List<KeyValuePair<DateTime, string>> gate0Versions(int StageGateItemId, int gateNo, string field, string table, string submitterField)
        {
            List<KeyValuePair<DateTime, string>> versions = new List<KeyValuePair<DateTime, string>>();
            versions = stageGateGeneralService.Gate0SubmittedVersions(StageGateItemId, gateNo, field, table, submitterField);
            if (versions.Count <= 0)
            {
                versions.Add(new KeyValuePair<DateTime, string>(DateTime.MinValue, ""));
            }
            return versions;
        }
        private List<KeyValuePair<DateTime, string>> submittedVersions(int StageGateItemId, int gateNo, string field, string table, string submitterField)
        {
            List<KeyValuePair<DateTime, string>> versions = new List<KeyValuePair<DateTime, string>>();
            versions = stageGateGeneralService.SubmittedVersions(StageGateItemId, gateNo, field, table, submitterField);
            if (versions.Count <= 0)
            {
                versions.Add(new KeyValuePair<DateTime, string>(DateTime.MinValue, ""));
            }
            return versions;
        }
        private void LoadGateAttachments()
        {
            files = stageGateGeneralService.GetStageGateFiles(ProjectNumber, webUrl);
            //files = files.OrderBy(r => r.FileContentLength).ToList();
            if (files.Count > 0)
            {
                var gates = files.Select(x => x.FileContentLength).Distinct().ToList();
                List<int> gatesList = new List<int>();
                bool hasProjectFiles = false;
                foreach (int gate in gates)
                {
                    int gateInt = Convert.ToInt32(gate);
                    if (gateInt != 0)
                    {
                        gatesList.Add(gateInt);
                    }
                    else
                    {
                        hasProjectFiles = true;
                    }
                }

                rpAttachmentsHeader.Visible = true;
                gatesList = gatesList.OrderBy(r => r).ToList();
                if (hasProjectFiles)
                {
                    gatesList.Add(0);
                }
                rpAttachmentsHeader.DataSource = gatesList;
                rpAttachmentsHeader.DataBind();
            }
            else
            {
                rpAttachmentsHeader.Visible = false;
                rpAttachmentsHeader.Visible = false;
            }
        }
        protected void rpAttachmentsHeader_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                int gate = (int)e.Item.DataItem;
                PlaceHolder lblStageHeader = (PlaceHolder)e.Item.FindControl("lblStageHeader");
                string stage = "";
                if (gate == 0)
                {
                    stage = "Project Files";
                }
                else if (gate == 1)
                {
                    stage = "Design Files";
                }
                else if (gate == 2)
                {
                    stage = "Develop/Validate Files";
                }
                else if (gate == 3)
                {
                    stage = "Industrialize Files";
                }
                else if (gate == 4)
                {
                    stage = "Launch Files";
                }
                else if (gate == 5)
                {
                    stage = "Post Launch Files";
                }

                lblStageHeader.Controls.Add(new LiteralControl(stage));

                List<FileAttribute> filteredFiles = files.Where(r => r.FileContentLength == gate).ToList();
                Repeater repeater = (Repeater)e.Item.FindControl("rpAttachments");
                repeater.DataSource = filteredFiles;
                repeater.DataBind();
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                /*if (!CheckWriteAccess())
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }*/
                if (StageGateProjectItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                StageGateCreateProjectItem stageItem = new StageGateCreateProjectItem();
                stageItem.RevisedShipDate = txtRevisedFirstShipDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtRevisedFirstShipDate.Text);

                stageItem.StageGateProjectListItemId = StageGateProjectItemId;
                stageGateGeneralService.updateRevisedFirstShip(stageItem);
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);

            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateProjectPanel.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateProjectPanel.ToString(), "btnSubmit_Click");
            }
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            LoadGateAttachments();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (StageGateProjectItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                stageGateGeneralService.cancelProject(StageGateProjectItemId);
                var stateGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateProjectItemId);
                //Send Parent Project Submitted Email
                if (stateGateItem.ProjectCacnelledSent != "Yes")
                {
                    if (notificationService.EmailParentWFStep("ParentProjectCancelled", stateGateItem))
                    {
                        stateGateItem.ProjectCacnelledSent = "Yes";
                        stageGateCreateProjectService.UpdateStageGateProjectCancelledEmailSent(stateGateItem);
                    }
                }
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateProjectPanel.ToString() + ": btnCancel_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateProjectPanel.ToString(), "btnCancel_Click");
            }
        }
        protected void btnOnHoldProject_Click(object sender, EventArgs e)
        {
            try
            {
                if (StageGateProjectItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                stageGateGeneralService.ProjectOnHold(StageGateProjectItemId);
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateProjectPanel.ToString() + ": btnOnHoldProject_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateProjectPanel.ToString(), "btnOnHoldProject_Click");
            }
        }
        protected void btnRemoveOnHold_Click(object sender, EventArgs e)
        {
            try
            {
                if (StageGateProjectItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                var stateGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateProjectItemId);
                DateTime OnHoldStartDate = stateGateItem.OnHoldStartDate == DateTime.MinValue ? DateTime.Now : stateGateItem.OnHoldStartDate;
                List<DateTime> holidaysList = new List<DateTime>();
                var weekendCount = weekends(OnHoldStartDate, DateTime.Now, holidaysList);
                var diff = DateTime.Now - OnHoldStartDate;
                var duration = diff.Days == 0 ? 0 : (diff.Days - weekendCount < 0 ? 0 : diff.Days - weekendCount);
                stageGateGeneralService.RemoveProjectOnHold(StageGateProjectItemId, stateGateItem.TotalOnHoldDays + Convert.ToUInt16(duration));
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateProjectPanel.ToString() + ": btnRemoveOnHold_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateProjectPanel.ToString(), "btnRemoveOnHold_Click", "StageGateProjectItemId = " + StageGateProjectItemId);
            }
        }
        private bool CheckUserPrivileges(int StageGateProjectListItemId)
        {
            string userId = SPContext.Current.Web.CurrentUser.ID.ToString();
            var userfilter = "";
            bool showButtons = false;

            userfilter = "<Where>" +
                           "<And>" +
                                "<Eq><FieldRef Name=\"ID\" LookupId=\"TRUE\" /><Value Type=\"Int\">" + StageGateProjectListItemId + "</Value></Eq>" +
                                "<Eq><FieldRef Name=\"ProjectLeader\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                           "</And>" +
                       "</Where>";
            var ProjectLeader = dashboardservice.getStageGateRequestedProjectDetails(userfilter);

            if (ProjectLeader.Count > 0 || Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_ProjectManagers + "," + GlobalConstants.GROUP_SeniorProjectManager))
            {
                showButtons = true;
            }

            return showButtons;
        }
    }
}
