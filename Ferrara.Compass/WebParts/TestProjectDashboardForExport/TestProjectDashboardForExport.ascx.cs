using System;
using System.ComponentModel;
using System.Web.UI.WebControls;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Classes;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Globalization;
using Ferrara.Compass.Abstractions.Models;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text;
using System.IO;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.WebParts.TestProjectDashboardForExport
{
    [ToolboxItemAttribute(false)]
    public partial class TestProjectDashboardForExport : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        #region Member Variables
        private IApprovalService approvalService;
        private IProjectTimelineUpdateService timelineUpdater;
        private IExceptionService exceptionService;
        private static IProjectTimelineTypeService timelineNumbers;
        private IWorkflowService workflowService;
        private IExcelExportSyncService exportService;
        private IItemProposalService proposalService;
        private IBillOfMaterialsService BOMservice;
        private IPackagingItemService packagingService;
        private static int iItemId = 0;
        #endregion

        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        private Boolean pageName
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["updatepage"] != null && Convert.ToBoolean(HttpContext.Current.Request.QueryString["updatepage"]) == true)
                    return Convert.ToBoolean(HttpContext.Current.Request.QueryString["updatepage"]);
                return false;
            }
        }
        private static CultureInfo ci = new CultureInfo("en-US");
        public TestProjectDashboardForExport()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            approvalService = DependencyResolution.DependencyMapper.Container.Resolve<IApprovalService>();
            timelineNumbers = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
            timelineUpdater = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineUpdateService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            proposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            BOMservice = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            exportService = DependencyResolution.DependencyMapper.Container.Resolve<IExcelExportSyncService>();
        }
        public static List<KeyValuePair<string, string>> urlList()
        {
            List<KeyValuePair<string, string>> urlList = new List<KeyValuePair<string, string>>();
            urlList.Add(new KeyValuePair<string, string>("IPF", "ItemProposal"));
            urlList.Add(new KeyValuePair<string, string>("SrOBMApproval", "InitialApprovalReview"));
            urlList.Add(new KeyValuePair<string, string>("TradePromo", "TradePromoGroup"));
            urlList.Add(new KeyValuePair<string, string>("Distribution", "Distribution"));
            urlList.Add(new KeyValuePair<string, string>("Operations", "OPS"));
            urlList.Add(new KeyValuePair<string, string>("NewXferSemi", "NewTransferSemi"));
            urlList.Add(new KeyValuePair<string, string>("ExternalMfg", "ExternalMfg"));
            urlList.Add(new KeyValuePair<string, string>("InitialCapacity", "InitialCapacityReview"));
            urlList.Add(new KeyValuePair<string, string>("InitialCosting", "InitialCostingReview"));
            urlList.Add(new KeyValuePair<string, string>("PrelimSAPInitialSetup", "PrelimSAPInitialItemSetup"));
            urlList.Add(new KeyValuePair<string, string>("SAPInitialSetup", "SAPInitialItemSetup"));
            urlList.Add(new KeyValuePair<string, string>("QA", "QA"));
            urlList.Add(new KeyValuePair<string, string>("OBMReview1", "OBMFirstReview"));
            urlList.Add(new KeyValuePair<string, string>("BOMSetupPE", "BOMSetupPE"));
            urlList.Add(new KeyValuePair<string, string>("BOMSetupProc", "BOMSetupProc"));
            urlList.Add(new KeyValuePair<string, string>("BOMSetupPE2", "BOMSetupPE2"));
            urlList.Add(new KeyValuePair<string, string>("SAPBOMSetup", "SAPBOMSetup"));
            urlList.Add(new KeyValuePair<string, string>("RnDFinalCheck", "RnDFinalCheck"));
            urlList.Add(new KeyValuePair<string, string>("SrOBMApproval2", "SecondaryApprovalReview"));
            urlList.Add(new KeyValuePair<string, string>("OBMReview2", "OBMSecondReview"));
            urlList.Add(new KeyValuePair<string, string>("FGPackSpec", "FinishedGoodPackSpec"));
            urlList.Add(new KeyValuePair<string, string>("GRAPHICS", "GraphicsRequest"));
            urlList.Add(new KeyValuePair<string, string>("CostingQuote", "ComponentCosting"));

            return urlList;
        }
        public static List<List<string>> weeksList(DateTime projectStart, DateTime projectEndDate)
        {
            string format = "MMM dd";
            DateTime projectStartWeek;
            DateTime projectEndWeek;
            //DateTime projectEndDate = projectStart.AddDays(projectEnd);
            //int weekendCount = holidays(projectStart, projectEndDate, holidaysList);
            //projectEndDate = projectEndDate.AddDays(weekendCount);
            List<List<string>> projectWeeks = new List<List<string>>();
            String displayWeek;
            int mondayNum = (int)DayOfWeek.Monday;
            int firstMonday = (int)projectStart.DayOfWeek;
            int lastMonday = (int)projectEndDate.DayOfWeek;
            if (firstMonday == 1)
            {
                projectStartWeek = projectStart;
            }
            else if (firstMonday == 0)
            {
                projectStartWeek = projectStart.AddDays(1);
            }
            else
            {
                projectStartWeek = projectStart.AddDays(-(firstMonday) + mondayNum);
            }
            if (lastMonday == 1)
            {
                projectEndWeek = projectEndDate;
            }
            else if (lastMonday == 0)
            {
                projectEndWeek = projectEndDate.AddDays(1);
            }
            else
            {
                projectEndWeek = projectEndDate.AddDays(-(lastMonday) + mondayNum);
            }
            List<DateTime> holidayList = timelineNumbers.GetHolidays();
            
            for (DateTime i = projectStartWeek; i.CompareTo(projectEndWeek) < 0; i = i.AddDays(7))
            {
                string holidayDays = "";
                List<string> weekDisplay = new List<string>();
                displayWeek = "Week of <br />" + i.ToString(format, ci);
                weekDisplay.Add(displayWeek);
                foreach (DateTime holiday in holidayList.Where(r => r.Date >= i.Date && r.Date < i.AddDays(7).Date))
                {
                    if ((int)holiday.DayOfWeek != 0 && (int)holiday.DayOfWeek != 6)
                    {
                        holidayDays = holidayDays + ((int)holiday.DayOfWeek).ToString();
                    }
                }
                weekDisplay.Add(holidayDays);
                projectWeeks.Add(weekDisplay);
            }
            List<string> lastWeekDisplay = new List<string>();
            displayWeek = "Week of <br />" + projectEndWeek.ToString(format, ci);
            lastWeekDisplay.Add(displayWeek);
            string lastHolidayDays = "";
            foreach (DateTime holiday in holidayList.Where(r => r.Date >= projectEndWeek.Date && r.Date < projectEndWeek.AddDays(7).Date))
            {
                if ((int)holiday.DayOfWeek != 0 && (int)holiday.DayOfWeek != 6)
                {
                    lastHolidayDays = lastHolidayDays + ((int)holiday.DayOfWeek).ToString();
                }
            }
            lastWeekDisplay.Add(lastHolidayDays);
            projectWeeks.Add(lastWeekDisplay);
            return projectWeeks;
        }
        public void workflowRows(List<List<string>> columnValues)
        {
            int taskCount = 0;
            foreach (List<string> cells in columnValues)
            {

                TableRow workflowRow = new TableRow();
                //Process Column
                TableCell workflowName = new TableCell();

                workflowName.Attributes.Add("class", "processCol");
                string workflowEmailText = "";
                if (cells.Count > 10 && cells[8] == "Phase")
                {
                    workflowName.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + cells[0]));
                    workflowRow.CssClass = "phaseRow";
                    workflowName.Attributes.Add("class", "collapseProcess");
                }else if(cells.Count > 10 && cells[8] == "PackagingItem")
                {
                    workflowName.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + cells[0]));
                    workflowRow.CssClass = "packagingRow";
                    workflowName.Attributes.Add("class", "collapseProcess");
                }
                else
                {
                    workflowName.Controls.Add(new LiteralControl(cells[0]));
                    workflowEmailText = "";
                }
                workflowRow.Cells.Add(workflowName);
                //Spacing Column
                TableCell workflowCellSpacing4 = new TableCell();
                workflowCellSpacing4.Attributes.Add("class", "spacingCol");
                workflowRow.Cells.Add(workflowCellSpacing4);   
                //Status Column
                TableCell workflowStatus = new TableCell();
                workflowStatus.Controls.Add(new LiteralControl(cells[1]));
                workflowStatus.Attributes.Add("class", "statusCol");
                workflowRow.Cells.Add(workflowStatus);
                //SubmittedBy Column
                TableCell workflowSubmittedBy = new TableCell();
                workflowSubmittedBy.Controls.Add(new LiteralControl(cells[9]));
                workflowSubmittedBy.Attributes.Add("class", "startSourceCol");
                workflowRow.Cells.Add(workflowSubmittedBy);
                //Email Column
                TableCell workflowEmail = new TableCell();
                workflowEmail.Controls.Add(new LiteralControl(workflowEmailText));
                workflowEmail.Attributes.Add("class", "endSourceCol");
                workflowRow.Cells.Add(workflowEmail);
                //Spacing Column
                TableCell workflowCellSpacing1 = new TableCell();
                workflowCellSpacing1.Attributes.Add("class", "spacingCol");
                workflowRow.Cells.Add(workflowCellSpacing1);
                //Start Date Column
                TableCell workflowStart = new TableCell();
                workflowStart.Controls.Add(new LiteralControl(cells[2]));
                workflowStart.Attributes.Add("class", "startDayCol");
                workflowRow.Cells.Add(workflowStart);
                //End Date Column
                TableCell workflowEnd = new TableCell();
                workflowEnd.Controls.Add(new LiteralControl(cells[3]));
                workflowEnd.Attributes.Add("class", "endDayCol");
                workflowRow.Cells.Add(workflowEnd);
                //Total Days Column
                TableCell workflowTotal = new TableCell();
                if (cells.Count > 11 && cells[8] == "PackagingItem")
                {
                    workflowTotal.Controls.Add(new LiteralControl(""));
                }
                else
                {
                    double totalDaysDisplay = Math.Round(Convert.ToDouble(cells[4]), 0);

                    workflowTotal.Controls.Add(new LiteralControl(totalDaysDisplay.ToString()));
                }
                workflowTotal.Attributes.Add("class", "totalDaysCol");
                workflowRow.Cells.Add(workflowTotal);
                //Spacing Column
                TableCell workflowCellSpacing5 = new TableCell();
                workflowCellSpacing5.Attributes.Add("class", "spacingCol");
                workflowRow.Cells.Add(workflowCellSpacing5);
                //Calendar Columns
                //TableCell workflowCalendarLeft = new TableCell();
                TableCell workflowCalendar = new TableCell();
                //workflowCalendarLeft.Attributes.Add("class", "calendarCol");
                workflowCalendar.Attributes.Add("class", "calendarCol");
                int calendarCols = (headerRow.Cells.Count - 8) * 20;
                workflowCalendar.ColumnSpan = calendarCols;
                Panel calendarDisplay = new Panel();
                if (cells[7] != "" && Convert.ToDouble(cells[4]) != 0)
                {
                    if (pageName)
                    {
                        if(cells[5] != "" && cells.Count <= 10) {
                            TextBox tb = new TextBox();
                            tb.ID = cells[5];
                            tb.Text = cells[6].Split(',')[0];
                            tb.ReadOnly = Convert.ToBoolean(cells[6].Split(',')[1]);
                            tb.CssClass = "form-control";
                            calendarDisplay.Controls.Add(tb);
                        }
                        workflowCalendar.Controls.Add(calendarDisplay);

                    }
                    else
                    {
                        int colspan = Convert.ToInt32(cells[5]);
                        //workflowCalendarLeft.ColumnSpan = colspan;
                        calendarCols = calendarCols - colspan;
                        calendarDisplay.Width = Convert.ToInt32(cells[6]);
                        calendarDisplay.CssClass = cells[7];
                        calendarDisplay.Attributes.CssStyle.Add("margin-left", cells[5] + "px");
                        calendarDisplay.Controls.Add(new LiteralControl("|&nbsp;&nbsp;"));
                        Panel endSource = new Panel();
                        endSource.Controls.Add(new LiteralControl(cells[8]));
                        endSource.CssClass = "endSource";
                        workflowCalendar.Controls.Add(calendarDisplay);
                        workflowCalendar.Controls.Add(endSource);
                    }
                }
                //workflowRow.Controls.Add(workflowCalendarLeft);
                workflowRow.Controls.Add(workflowCalendar);


                projectDashboard.Rows.Add(workflowRow);

                taskCount++;
            }
        }
        public void displayCalendarDays(List<List<String>> displayWeek)
        {
            foreach (List<String> i in displayWeek)
            {
                TableCell calendarWeek = new TableCell();
                calendarWeek.ColumnSpan = 20;
                Panel week = new Panel();
                calendarWeek.Controls.Add(new LiteralControl(i[0]));
                calendarWeek.Attributes.Add("class", calendarWeek.Attributes["class"] + " " + "weekText");
                if (i[1] != "")
                {
                    if (i[1].ToLower().Contains('1'))
                    {
                        week.Attributes.Add("class", week.Attributes["class"] + " " + "monday");
                    }
                    if (i[1].ToLower().Contains('2'))
                    {
                        week.Attributes.Add("class", week.Attributes["class"] + " " + "tuesday");
                    }
                    if (i[1].ToLower().Contains('3'))
                    {
                        week.Attributes.Add("class", week.Attributes["class"] + " " + "wednesday");
                    }
                    if (i[1].ToLower().Contains('4'))
                    {
                        week.Attributes.Add("class", week.Attributes["class"] + " " + "thursday");
                    }
                    if (i[1].ToLower().Contains('5'))
                    {
                        week.Attributes.Add("class", week.Attributes["class"] + " " + "friday");
                    }
                }
                TableCell calendarDay = new TableCell();
                var dayClass = "dayWidth";
                
                Panel monday = new Panel();
                monday.CssClass = dayClass;
                monday.Controls.Add(new LiteralControl("M"));
                Panel tuesday = new Panel();
                tuesday.CssClass = dayClass;
                tuesday.Controls.Add(new LiteralControl("T"));
                Panel wednesday = new Panel();
                wednesday.CssClass = dayClass;
                wednesday.Controls.Add(new LiteralControl("W"));
                Panel thursday = new Panel();
                thursday.CssClass = dayClass;
                thursday.Controls.Add(new LiteralControl("Th"));
                Panel friday = new Panel();
                friday.CssClass = dayClass;
                friday.Controls.Add(new LiteralControl("F"));
                
                week.Controls.Add(monday);
                week.Controls.Add(tuesday);
                week.Controls.Add(wednesday);
                week.Controls.Add(thursday);
                week.Controls.Add(friday);
                week.Attributes.Add("class", week.Attributes["class"] + " " + "weekClass");

                calendarWeek.Controls.Add(week);
                headerRow.Cells.Add(calendarWeek);
                //dailyDisplay.Cells.Add(calendarDay);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(ProjectNumber == "")
            {
                Page.Response.Redirect("/Pages/TaskDashboard.aspx?error=true&errorText=Projent Number is Required to view the Project Dashboard", false);
            }
            bool addLink = true;
            if(!Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OperationsBusinessManager) && !Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_BrandManager) && !Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            {
                updateButtons.Visible = false;
                workflowChanges.Visible = false;
                updateDateButtons.Visible = false;
                addLink = false;

                //Page.Response.Redirect("/Pages/TaskDashboard.aspx?error=true&errorText=You do not have access to view the Project Dashboard", false);
            }
            else if(!Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OperationsBusinessManager) && !Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            {
                btnFirstShipDate.Visible = false;
                txtFirstProdDate.ReadOnly = true;
                txtFirstShipDate.ReadOnly = true;
                btnFirstProdDate.Visible = false;
            }
            if (!pageName && addLink)
            {
                HtmlAnchor updateLink = new HtmlAnchor();

                updateLink.Controls.Add(new LiteralControl("Update Total Days Fields"));
                updateLink.HRef = "/Pages/TesTDASHBOARDFOREXPORT.aspx?ProjectNo=" + ProjectNumber + "&updatepage=true";
                updateLinkPanel.Controls.Add(updateLink);
            }
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            hddProjectId.Value = iItemId.ToString();
            DashboardDetailsItem dashboardDetails = timelineNumbers.dashboardDetails(iItemId);
            testPanel.Controls.Add(new LiteralControl(dashboardDetails.ProjectName));

            string openWindow = @"javascript:window.open('CommercializationItem.aspx?ProjectNo=" + ProjectNumber + "'); return false;";
            lbDisplayCommForm.Attributes.Add("onclick", openWindow);

            ApprovalListItem approvalItem = approvalService.GetApprovalItem(iItemId);
            
            List<DateTime> holidaysList = timelineNumbers.GetHolidays();
            List<TimelineTypeItem> phases = timelineNumbers.GetPhases();
            String timelineType = dashboardDetails.TimelineType;
            
            List<TimelineTypeItem> tasks = timelineNumbers.GetWorkflowStepItems(timelineType, iItemId);
            List<List<string>> taskCallStart = timelineNumbers.GetWorkflowTasksStart(iItemId);
            List<List<string>> taskCallEnd = timelineNumbers.GetWorkflowTasksEnd(iItemId);
            List<List<string>> updatedTimes = timelineUpdater.GetProjectItem(iItemId);
            List<KeyValuePair<string,string>> hideRows = timelineNumbers.hideRow(iItemId);

            List<KeyValuePair<int, DateTime>> listOfExceptionList = new List<KeyValuePair<int, DateTime>>();
            List<DateTime> ExceptionEndDays = new List<DateTime>();
            List<List<string>> existingProject = new List<List<string>>();
            List<KeyValuePair<string, string>> completedTasks = new List<KeyValuePair<string, string>>();
            Boolean isExisting = false;
            string startSource = "";
            string endSource = "";
            if (pageName)
            {
                existingProject = timelineUpdater.GetProjectItem(iItemId);
                completedTasks = timelineUpdater.GetCompletedItems(iItemId);
                isExisting = existingEntry(iItemId);
            }
            DateTime previousTaskStart = new DateTime();
            int exceptionHolder = 0;
            double dayHolder = 0;
            double leftSpace = 0;
            DateTime projectStart = new DateTime();
            DateTime ipfSubmit = new DateTime();
            if (String.IsNullOrEmpty(approvalItem.IPF_SubmittedDate))
            {
                ipfSubmit = DateTime.Now;
                Panel submitDateMessage = new Panel();
                submitDateMessage.CssClass = "markrequired";
                submitDateMessage.ID = "submitDateMessage";
                submitDateMessage.ClientIDMode = ClientIDMode.Static;
                submitDateMessage.Controls.Add(new LiteralControl("This status report is an estimate of the current project based on today’s date. After submittal of the current project, dates are subject to change."));
                phMessaging.Controls.Add(submitDateMessage);
                startSource = "Right now";
                endSource = "Right now";
            }
            else
            {
                ipfSubmit = DateTime.Parse(approvalItem.IPF_SubmittedDate, ci);
                startSource = "IPF submitted";
                endSource = "IPF submitted";
            }
            
            DateTime newIPFStart = ipfSubmit;
            TimeSpan ipfTime = newIPFStart.TimeOfDay;
            if (ipfTime <= TimeSpan.Parse("07:59:00"))
            {
                TimeSpan ts = new TimeSpan(8, 0, 0);
                newIPFStart = newIPFStart.Date + ts;
            }
            else if (ipfTime >= TimeSpan.Parse("15:59:00"))
            {
                //dayHolder = 1;
                newIPFStart = newIPFStart.AddDays(1);
                TimeSpan ts = new TimeSpan(8, 0, 0);
                newIPFStart = newIPFStart.Date + ts;
            }
            if ((int)newIPFStart.DayOfWeek == 0)//If Sunday Add 1 day
            {
                newIPFStart = newIPFStart.AddDays(1);
                dayHolder = 1;
            }
            else if ((int)newIPFStart.DayOfWeek == 6)//If Saturday Add 2 days
            {
                newIPFStart = newIPFStart.AddDays(2);
                dayHolder = 2;
            }
            DateTime firstShipDate = DateTime.Now;
            DateTime firstProdDate;
            if (DateTime.TryParse(dashboardDetails.FirstShipDate, out firstShipDate))
            {
                firstShipDate = DateTime.Parse(dashboardDetails.FirstShipDate);
            }
            if (dashboardDetails.ProjectType.ToLower().Contains("seasonal") && DateTime.TryParse(dashboardDetails.FirstProductionDate, out firstProdDate))
            {
                firstProdDate = DateTime.Parse(dashboardDetails.FirstProductionDate);
                if (firstProdDate != Utilities.GetMinDate())
                {
                    firstShipDate = firstProdDate;
                }
            }
            DateTime todayFields = newIPFStart.AddDays(-(int)newIPFStart.DayOfWeek + (int)DayOfWeek.Monday);
            TimeSpan nf = new TimeSpan(8, 0, 0);
            todayFields = todayFields.Date + nf;
            DateTime projectEnd = new DateTime();
            DateTime rightNow = DateTime.Now;
            double projectDuration = 0;
            int phaseCounter = 0;
            int allTaskCounter = 0;
            double weekendCount = 0;
            DateTime previousTaskComplete = new DateTime();
            List<KeyValuePair<string, string>> urlPath = urlList();
            List<List<string>> dashboardRows = new List<List<string>>();
            foreach (TimelineTypeItem data in phases)
            {
                List<List<string>> dashboardPhaseRows = new List<List<string>>();
                List<string> dashboardPhaseRowCells = new List<string>();
                List<KeyValuePair<int, double>> stackedList = new List<KeyValuePair<int, double>>();
                List<KeyValuePair<int, DateTime>> stackedEndList = new List<KeyValuePair<int, DateTime>>();
                string phaseName = data.WorkflowStep;
                DateTime phaseStart = new DateTime();
                DateTime phaseEnd = new DateTime();
                string color = "grey";
                string phaseColor = "phaseGrey";
                double phaseDuration = 0;
                double phaseWidth = 0;
                double phaseLeft = 0;
                List<DateTime> phaseDayHolder = new List<DateTime>();
                string phaseStatus = "Waiting";
                int counter = 0;
                int taskCount = 0;
                int exceptionDateHolder = 0;
                double dayHolderHolder = 0;
                Boolean addDuration = false;
                int hiddenTaskCounter = 0;
                int headerRowCounter = 0;
                foreach (TimelineTypeItem timelineInfo in tasks.Where(r => r.PhaseNumber == data.PhaseNumber))
                {
                    double dayHolderDisplay = dayHolder;
                    string WFQuickStep = timelineInfo.WorkflowQuickStep;
                    string WFException = timelineInfo.WorkflowExceptions;
                    string taskName = "";
                    taskName = timelineInfo.WorkflowStep;                    
                    Boolean skipRow = hideCurrentRow(WFQuickStep, hideRows);
                    string skipRowReason = hideCurrentRowReason(WFQuickStep, hideRows);
                    List<string> dashboardRowCells = new List<string>();
                    if (skipRow)
                    {
                        dashboardRowCells.Add(taskName);
                        dashboardRowCells.Add("HIDDEN ROW");
                        dashboardRowCells.Add("N/A");
                        dashboardRowCells.Add("N/A");
                        dashboardRowCells.Add("0");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRows.Add(dashboardRowCells);
                        hiddenTaskCounter = hiddenTaskCounter + 1;
                        continue;
                    }
                    
                    if ((WFException == "DF" && timelineInfo.WorkflowOrder == 0) || (WFException == "NE" && timelineInfo.WorkflowOrder == 0))
                    {
                        dashboardRowCells.Add(taskName);
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("0");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("PackagingItem");
                        dashboardRowCells.Add("");
                        dashboardRowCells.Add("");
                        dashboardRows.Add(dashboardRowCells);
                        allTaskCounter++;
                        headerRowCounter++;
                        continue;
                    }
                    foreach (KeyValuePair<string, string> task in urlPath.Where(r => r.Key == WFQuickStep))
                    {
                        HyperLink taskForm = new HyperLink();
                        string extraParams = "";
                        if(WFQuickStep == "CostingQuote")
                        {
                            extraParams = "&PackagingItemId=" + timelineInfo.Expedited;
                        }
                        taskForm.NavigateUrl = "/Pages/" + task.Value + ".aspx?ProjectNo=" + ProjectNumber + extraParams;
                        taskForm.Text = taskName;
                        StringBuilder myStringBuilder = new StringBuilder();
                        TextWriter myTextWriter = new StringWriter(myStringBuilder);
                        HtmlTextWriter myWriter = new HtmlTextWriter(myTextWriter);
                        taskForm.RenderControl(myWriter);
                        taskName = myTextWriter.ToString();
                    }
                    dashboardRowCells.Add(taskName);
                    string status = "Waiting";
                    Boolean statusCheck = false;
                    Boolean updatedStart = false;
                    TimeSpan time = new TimeSpan();
                    DateTime taskEnd = new DateTime();
                    Boolean taskEndConfirm = false;

                    Boolean hasException = false;
                    //Begin Start Task Day Section
                    DateTime taskStart = new DateTime();
                    int addedDay = 0;
                    double maxHolder = 0;
                    int currentStack = timelineInfo.WorkflowStacks;
                    int diffDaysHolder = 0;
                    if (data.PhaseNumber == 1)//For IPF Phase
                    {
                        taskStart = newIPFStart;
                        if (String.IsNullOrEmpty(approvalItem.IPF_SubmittedDate))
                        {
                            status = "Waiting";
                            phaseStatus = "Waiting";
                            phaseColor = "phaseGrey";
                            color = "grey";

                        }
                        else
                        {
                            status = "Completed";
                            phaseStatus = "Completed";
                            color = "blue";
                            phaseColor = "phaseBlue";
                        }                       
                        statusCheck = true;
                        taskEnd = newIPFStart;
                        taskEndConfirm = true;
                    }
                    else
                    {
                        int taskListCount = taskCallStart.SelectMany(list => list).Distinct().Count();
                        foreach (List<string> newTimes in taskCallStart.Where(s => s[0] == WFQuickStep)) //Get Compass Approval Start Dates
                        {
                            if (newTimes[1] != "")
                            {
                                try
                                {
                                    taskStart = DateTime.Parse(newTimes[1], ci);
                                    DateTime holderDate = newIPFStart.AddDays(dayHolder);
                                    TimeSpan diff = new TimeSpan();
                                    int compareStart = DateTime.Compare(taskStart, holderDate);
                                    diff = taskStart - holderDate;
                                    diffDaysHolder = diff.Days;
                                    if (compareStart <= 0)
                                    {
                                        dayHolder = dayHolder + diff.Days;
                                    }
                                    else
                                    {
                                        dayHolder = dayHolder - diff.Days;
                                    }
                                    startSource = "from approval list";
                                    updatedStart = true;
                                }
                                catch (Exception exception)
                                {
                                    Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = "+ dayHolder + "; BP = 1";
                                    exceptionService.Handle(LogCategory.CriticalError, exception, errorDets, "Page_Load", errorDets);
                                }
                            }
                        }                       
                        if (!updatedStart)
                        {
                            taskStart = previousTaskComplete;
                            startSource = "based on previous task end day";
                            dayHolder = dayHolder + timelineInfo.Holder;
                        }
                        if (currentStack != 0)
                        {
                            int alreadyAdded = (from key in listOfExceptionList where key.Key == currentStack select key.Key).Count();
                            if (exceptionHolder != currentStack && alreadyAdded <= 0)
                            {
                                dayHolderHolder = dayHolder - weekendCount - addedDay;
                                exceptionDateHolder = (from task in tasks where task.WorkflowStacks == currentStack select task.WorkflowStacks).Count();
                                if (updatedStart)
                                {
                                    listOfExceptionList.Add(new KeyValuePair<int, DateTime>(currentStack, taskStart));
                                }
                                else
                                {
                                    listOfExceptionList.Add(new KeyValuePair<int, DateTime>(currentStack, previousTaskStart));
                                }

                            }
                            if (exceptionDateHolder > 1)
                            {
                                int currentExceptionCount = (from key in stackedList where key.Key == currentStack select key.Key).Count();
                                if (currentExceptionCount == exceptionDateHolder - 1)
                                {
                                    List<double> maxDays = new List<double>();
                                    foreach (KeyValuePair<int, double> kvp in stackedList.Where(r => r.Key == currentStack))
                                    {
                                        maxDays.Add(kvp.Value);
                                    }
                                    dayHolder = dayHolderHolder + maxDays.Max();
                                    maxHolder = maxDays.Max();
                                    addDuration = true;
                                }
                                DateTime exceptionStart = Convert.ToDateTime((from key in listOfExceptionList where key.Key == currentStack select key.Value).Last());
                                int keyValue = (from key in listOfExceptionList where key.Key == currentStack select key.Key).FirstOrDefault();
                                if (keyValue == currentStack && !updatedStart)
                                {
                                    taskStart = exceptionStart;
                                }
                                exceptionHolder = currentStack;
                                hasException = true;
                            }
                        }

                    }
                    time = taskStart.TimeOfDay;

                    if (time <= TimeSpan.Parse("07:59:00"))
                    {
                        TimeSpan ts = new TimeSpan(8, 0, 0);
                        taskStart = taskStart.Date + ts;
                    }
                    else if (time >= TimeSpan.Parse("15:59:00"))
                    {
                        taskStart = taskStart.AddDays(1);
                        dayHolder = dayHolder + 1;
                        //addedDay = addedDay + 1;
                        TimeSpan ts = new TimeSpan(8, 0, 0);
                        taskStart = taskStart.Date + ts;
                        startSource += "; +1 day since started after 4PM";

                    }
                    if (taskStart.DayOfWeek == 0)
                    {
                        taskStart = taskStart.AddDays(1);
                        dayHolder = dayHolder + 1;
                        addedDay = addedDay + 1;
                        startSource += "; +1 day since started on a Sunday";
                    }
                    else if ((int)taskStart.DayOfWeek == 6)
                    {
                        taskStart = taskStart.AddDays(2);
                        dayHolder = dayHolder + 2;
                        addedDay = addedDay + 2;
                        startSource += "; +2 days since started on a Saturday";
                    }
                    previousTaskStart = taskStart;
                    dashboardRowCells.Add(taskStart.ToString("MM/dd/yyyy"));
                    //End Start Task Day Section
                    //Begin End Task Day Section
                    if (!taskEndConfirm)
                    {              
                        if (WFException != "DF")
                        {
                            foreach (List<string> newTimes in taskCallEnd.Where(s => s[0] == WFQuickStep)) //Get Compass Approval Submitted Dates
                            {
                                try
                                {
                                    if (newTimes[1] != "")
                                    {
                                        taskEnd = DateTime.Parse(newTimes[1], ci);
                                        TimeSpan diff = new TimeSpan();
                                        int compareStart = DateTime.Compare(taskEnd, taskStart);
                                        if (compareStart <= 0)
                                        {
                                            diff = taskStart - taskEnd;
                                        }
                                        else
                                        {
                                            diff = taskEnd - taskStart;
                                        }
                                        double removeCompletedDays = weekends(taskStart, taskEnd, holidaysList, false);
                                        dayHolder = dayHolder + diff.Days - addedDay - removeCompletedDays;
                                        if (!updatedStart)
                                        {
                                            dayHolder = dayHolder - timelineInfo.Holder;
                                        }
                                        updatedStart = true;
                                        status = "Completed";
                                        color = "blue";
                                        phaseStatus = "Completed";
                                        phaseColor = "phaseBlue";
                                        taskEndConfirm = true;
                                        statusCheck = true;
                                        endSource = "from approval list";
                                    }
                                }
                                catch (Exception exception)
                                {
                                    Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = " + dayHolder + "; BP = 2";
                                    exceptionService.Handle(LogCategory.CriticalError, exception, errorDets, "Page_Load", errorDets);
                                }
                            }
                        }
                        else
                        {
                            foreach (List<string> newTimes in taskCallEnd.Where(s => s[0] == WFQuickStep && s[2] == timelineInfo.WorkflowMisc)) //Get Compass Approval Submitted Dates
                            {
                                try { 
                                    if (newTimes[1] != "")
                                    {
                                        taskEnd = DateTime.Parse(newTimes[1], ci);
                                        TimeSpan diff = new TimeSpan();
                                        int compareStart = DateTime.Compare(taskEnd, taskStart);
                                        if (compareStart <= 0)
                                        {
                                            diff = taskStart - taskEnd;
                                        }
                                        else
                                        {
                                            diff = taskEnd - taskStart;
                                        }
                                        double removeCompletedDays = weekends(taskStart, taskEnd, holidaysList, false);
                                        dayHolder = dayHolder + diff.Days - addedDay - removeCompletedDays;
                                        if (!updatedStart)
                                        {
                                            dayHolder = dayHolder - timelineInfo.Holder;
                                        }
                                        updatedStart = true;
                                        status = "Completed";
                                        color = "blue";
                                        phaseStatus = "Completed";
                                        phaseColor = "phaseBlue";
                                        taskEndConfirm = true;
                                        statusCheck = true;
                                        endSource = "from approval list";
                                    }
                                }
                                catch (Exception exception)
                                {
                                    Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = " + dayHolder + "; BP = 3";
                                    exceptionService.Handle(LogCategory.CriticalError, exception, errorDets, "Page_Load", errorDets);
                                }
                            }
                        }                    
                    }
                    Boolean lateTask = false;
                    if (!taskEndConfirm)
                    {                    
                        foreach (List<string> updatedDays in updatedTimes.Where(t => t[0] == WFQuickStep))
                        {
                            
                            if (updatedDays[1] != "" && updatedDays[1] != null)
                            {
                                try
                                {
                                    taskEnd = taskStart.AddDays(Convert.ToDouble(updatedDays[1]));

                                    double dayChange = weekends(taskStart, taskEnd, holidaysList, false);
                                    taskEnd = taskEnd.AddDays(dayChange);
                                    TimeSpan diff = new TimeSpan();
                                    int compareStart = DateTime.Compare(taskEnd, taskStart);
                                    if (compareStart <= 0)
                                    {
                                        diff = taskStart - taskEnd;
                                    }
                                    else
                                    {
                                        diff = taskEnd - taskStart;
                                    }
                                    dayHolder = dayHolder + diff.Days - timelineInfo.Holder - addedDay;
                                    int compareEnd = DateTime.Compare(taskEnd, rightNow);
                                    if (compareEnd < 0 && updatedStart)
                                    {
                                        status = "Active - Late";
                                        color = "red";
                                        phaseColor = "phaseRed";
                                        phaseStatus = "Active - Late";
                                        statusCheck = true;
                                        lateTask = true;
                                        taskEndConfirm = true;
                                    }else if (compareEnd >= 0 && !updatedStart)
                                    {
                                        taskEndConfirm = true;
                                    }
                                    endSource = "user updated timeline or task was initially late";
                                }
                                catch (Exception exception)
                                {
                                    Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = " + dayHolder + "; BP = 4";
                                    exceptionService.Handle(LogCategory.CriticalError, exception, errorDets, "Page_Load", errorDets);
                                }
                            }
                        }                       
                    }                    
                    if (!taskEndConfirm && !updatedStart && WFQuickStep != "Notification")
                    {
                        taskEnd = taskStart.AddDays(timelineInfo.Holder);
                        endSource = "added days based on timeline type";
                    }
                    else if (!taskEndConfirm && updatedStart && WFQuickStep != "Notification")
                    {
                        taskEnd = taskStart.AddDays(timelineInfo.Holder);
                        int compareEnd = DateTime.Compare(taskEnd, rightNow);
                        if (compareEnd < 0)
                        {

                            TimeSpan diffNow = rightNow - taskEnd;
                            TimeSpan addCalc = rightNow - taskStart;
                            taskEnd = rightNow;
                            time = taskEnd.TimeOfDay;
                            double removeweekends = totalDaysCalc(taskStart, taskEnd, holidaysList);
                            double addDays = addCalc.Days - removeweekends;
                            if (time <= TimeSpan.Parse("07:59:00"))
                            {
                                TimeSpan ts = new TimeSpan(8, 0, 0);
                                taskEnd = taskEnd.Date + ts;
                                //addDays = diffNow.Days;
                            }
                            else if (time >= TimeSpan.Parse("15:59:00"))
                            {
                                taskEnd = taskEnd.AddDays(1);
                                TimeSpan ts = new TimeSpan(8, 0, 0);
                                taskEnd = taskEnd.Date + ts;
                                dayHolder = dayHolder + 1;
                                addDays = addDays + 1;
                            }
                            dayHolder = dayHolder + diffNow.Days;
                            if (WFException != "DF" && WFException != "NE")
                            {
                                timelineUpdater.UpdateSingleProjectTimelineItem(WFQuickStep, addDays.ToString(), iItemId, timelineType, ProjectNumber);
                            }
                            status = "Active - Late";
                            color = "red";
                            phaseColor = "phaseRed";
                            phaseStatus = "Active - Late";
                            statusCheck = true;
                            lateTask = true;
                            endSource = "marked as right now as task is late";
                        }
                        else
                        {
                            endSource = "added days based on timeline type";
                        }

                    }
                    else if (WFQuickStep == "Notification")
                    {
                        taskEnd = taskStart;
                        //dayHolder = dayHolder - timelineInfo.Holder;
                        int compareEnd = DateTime.Compare(taskEnd, rightNow);
                        if (compareEnd <= 0)
                        {
                            statusCheck = true;
                            updatedStart = true;
                            status = "Completed";
                            color = "blue";
                        }
                    }

                    if (!updatedStart && allTaskCounter != 0)
                    {
                        color = "grey";
                        status = "Waiting";

                    }
                    weekendCount = weekends(taskStart, taskEnd, holidaysList, false);
                    dayHolder = dayHolder + weekendCount;
                    if (!lateTask && !taskEndConfirm)
                    {

                        taskEnd = taskEnd.AddDays(weekendCount);
                        int result = DateTime.Compare(taskEnd, rightNow);
                        if (result < 0 && !statusCheck && updatedStart)
                        {
                            TimeSpan endSpan = rightNow - taskEnd;
                        double quickdays = weekends(taskEnd, rightNow, holidaysList, false);
                        double endDays = endSpan.Days - quickdays;
                            dayHolder = dayHolder + endDays;
                        }
                    }
                    time = taskEnd.TimeOfDay;

                    if (time <= TimeSpan.Parse("07:59:00"))
                    {
                        TimeSpan ts = new TimeSpan(8, 0, 0);
                        taskEnd = taskEnd.Date + ts;
                    }
                    else if (time >= TimeSpan.Parse("15:59:00"))
                    {
                        taskEnd = taskEnd.AddDays(1);
                        TimeSpan ts = new TimeSpan(8, 0, 0);
                        taskEnd = taskEnd.Date + ts;
                        dayHolder = dayHolder + 1;
                        endSource += "; added 1 days as calculated time would be after 4PM";

                    }
                    if (taskEnd.DayOfWeek == 0)
                    {
                        taskEnd = taskEnd.AddDays(1);
                        endSource += "; added 1 days as calculated time would be Sunday";
                    }
                    else if ((int)taskEnd.DayOfWeek == 6)
                    {
                        taskEnd = taskEnd.AddDays(2);
                        endSource += "; added 1 days as calculated time would be Saturday";
                    }

                    if (WFQuickStep != "Notification")// && WFQuickStep != "DF")
                    {
                        previousTaskStart = taskEnd;
                    }
                    if (hasException) {
                        stackedEndList.Add(new KeyValuePair<int, DateTime>(timelineInfo.WorkflowStacks, taskEnd));
                        int comparePrevious = DateTime.Compare(taskEnd, previousTaskComplete);
                        if (comparePrevious > 0)// || WFException == "DF")//|| timelineInfo.WorkflowExceptions == "NE" || WFQuickStep == "SAPRoutingSetup")
                        {
                            previousTaskComplete = (from ends in stackedEndList where ends.Key == timelineInfo.WorkflowStacks select ends.Value).OrderByDescending(r => r).FirstOrDefault();
                        }
                        else if(WFException == "DF")
                        {
                            previousTaskComplete = taskEnd;
                        }
                    }
                    else
                    {
                        previousTaskComplete = taskEnd;
                    }
                int compareStartEnd = DateTime.Compare(taskStart, taskEnd);
                    if(compareStartEnd > 0)
                {
                    int numberOfCells = dashboardRowCells.Count;
                    dashboardRowCells[numberOfCells-1] = taskEnd.ToString("MM/dd/yyyy");
                    taskStart = taskEnd;
                }
                if (!statusCheck && phaseStatus == "Completed")
                {
                    phaseStatus = "Waiting";
                    phaseColor = "phaseGrey";
                }
                    phaseDayHolder.Add(taskEnd);
                    dashboardRowCells.Add(taskEnd.ToString("MM/dd/yyyy"));
                    if(currentStack != 0)
                    {
                        ExceptionEndDays.Add(taskEnd);
                    }
                    //End End Task Day Section

                    //Begin Total Days Section

                    TimeSpan taskSpan = taskEnd - taskStart;
                    double newWeekend = totalDaysCalc(taskStart, taskEnd, holidaysList);
                    double taskDuration = taskSpan.Days - newWeekend;
                    if (taskDuration.ToString() == null || taskDuration <= 0)
                    {
                        taskDuration = .25;
                    }
                    
                    dashboardRowCells.Add(taskDuration.ToString());
                double tempTaskDuration = taskDuration;
                if (tempTaskDuration == .25)
                {
                    tempTaskDuration = 0;
                }
                if (hasException)
                    {

                        int phaseHolderCount = phaseDayHolder.Count() - 1;
                        
                        stackedList.Add(new KeyValuePair<int, double>(timelineInfo.WorkflowStacks, tempTaskDuration));
                        
                        if (!addDuration)
                        {
                            //previousTaskComplete = taskDuration;
                            taskDuration = 0;

                        }
                        else
                        {
                            previousTaskStart = ExceptionEndDays.OrderByDescending(x => x).FirstOrDefault();
                            addDuration = false;
                            taskDuration = maxHolder;
                            
                            //if (WFQuickStep != "DF")
                            //{
                            
                            //}
                        }
                    }

                    //End Total Days Section
                    //Status Section
                    if (!statusCheck && updatedStart)
                    {

                        double taskMinutes = taskSpan.TotalMinutes;
                        TimeSpan currentSpan = rightNow - taskStart;
                        double currentMinutes = currentSpan.TotalMinutes;
                        double currentPercent = currentMinutes / taskMinutes;
                        if (currentPercent < .95)
                        {
                            status = "Active";
                            color = "green";
                            phaseColor = "phaseGreen";
                            phaseStatus = "Active";
                        }
                        else if (currentPercent > .95 && currentPercent <= 1)
                        {
                            status = "Active";
                            color = "green";
                            phaseColor = "phaseGreen";
                            phaseStatus = "Active";
                        }
                        else if (currentPercent > 1)
                        {
                            status = "Active - Late";
                            color = "red";
                            phaseColor = "phaseRed";
                            phaseStatus = "Active - Late";
                        }
                    }
                    dashboardRowCells.Insert(1, status);     
                    //End Status Section
                    //Begin calendar Display
                    leftSpace = getWidth(todayFields, taskStart, holidaysList, false, true);
                    double taskWidth = getWidth(taskStart, taskEnd, holidaysList, true, true);
                    //double recalcWidth2 = (Math.Floor(taskWidth / 4) * 20) + ((taskWidth % 4) * 5);
                    //taskWidth = recalcWidth2;
                    if (pageName)
                    {
                        string passingArg = "";
                        if (WFQuickStep != "" && WFQuickStep != "Notification" && WFException != "DF") {
                            dashboardRowCells.Add("txt" + WFQuickStep);
                            if (!isExisting) {
                                if(status == "Completed") { 
                                    passingArg = tempTaskDuration.ToString() + ",true";
                                    dashboardRowCells.Add(passingArg);
                                }else {
                                    dashboardRowCells.Add(tempTaskDuration + ",false");
                                }
                            } else {
                            Boolean wasAdded = false;
                                foreach (List<string> task in existingProject.Where(r => r[0] == WFQuickStep))
                                {
                                    if (status == "Completed")
                                    {
                                        passingArg = tempTaskDuration + ",true";
                                        dashboardRowCells.Add(passingArg);
                                        wasAdded = true;
                                    }
                                    else
                                    {
                                        passingArg = task[1] + ",false";
                                        dashboardRowCells.Add(passingArg);
                                    wasAdded = true;
                                    }
                                }
                            if (!wasAdded)
                            {
                                dashboardRowCells.Add(tempTaskDuration + ",false");
                            }    
                            }
                        }
                        else
                        {
                            dashboardRowCells.Add("");
                            dashboardRowCells.Add("");

                        }
                    }
                    else
                    {

                        dashboardRowCells.Add(leftSpace.ToString());
                        dashboardRowCells.Add(taskWidth.ToString());
                    }
                    leftSpace = leftSpace + (taskWidth);
                    //End calendar Display
                    dashboardRowCells.Add(color);
                    //Start End Day Source
                        dashboardRowCells.Add(endSource);

                    //SubmittedBy Section
                        dashboardRowCells.Add(startSource);
                    //End Email section
                    dashboardRows.Add(dashboardRowCells);
                    
                    if (counter == 0)
                    {
                        phaseLeft = leftSpace - (taskWidth);
                        phaseStart = taskStart;
                    }
                    counter++;
                    allTaskCounter++;
                    startSource = "";
                    endSource = "";
                }
                taskCount = tasks.Count(t => t.PhaseNumber == data.PhaseNumber) - hiddenTaskCounter;
                phaseEnd = phaseDayHolder.OrderByDescending(x => x).FirstOrDefault();
                int phaseCount = phases.Count - 2;
                if (phaseCounter == 0 && counter == 1)
                {
                    projectStart = phaseStart;
                }
                else if (phaseCounter == phaseCount)
                {
                    projectEnd = phaseEnd;
                }
                phaseCounter++;
                int phasePosition = dashboardRows.Count - taskCount;
                TimeSpan phaseTime = phaseEnd - phaseStart;
                double phaseRemoveDays = totalDaysCalc(phaseStart, phaseEnd, holidaysList);
                phaseDuration = phaseTime.Days - phaseRemoveDays;
                


                phaseWidth = getWidth(phaseStart, phaseEnd, holidaysList, true, true);
                    //double recalcWidth = (Math.Floor(phaseWidth / 4) * 20) + ((phaseWidth % 4) * 5);
                    //phaseWidth = recalcWidth;
                    if (data.PhaseNumber != 6)
                    {
                        dashboardPhaseRowCells.Add(data.WorkflowStep);
                        dashboardPhaseRowCells.Add(phaseStatus);
                        dashboardPhaseRowCells.Add(phaseStart.ToString("MM/dd/yyyy"));
                        dashboardPhaseRowCells.Add(phaseEnd.ToString("MM/dd/yyyy"));
                        dashboardPhaseRowCells.Add(phaseDuration.ToString());
                        dashboardPhaseRowCells.Add(phaseLeft.ToString());
                        dashboardPhaseRowCells.Add(phaseWidth.ToString());
                        dashboardPhaseRowCells.Add(phaseColor);
                        dashboardPhaseRowCells.Add("Phase");
                    dashboardPhaseRowCells.Add("");
                    dashboardPhaseRowCells.Add("");
                    dashboardRows.Insert(phasePosition, dashboardPhaseRowCells);
                    }
                    else if (data.PhaseNumber == 6)
                    {
                        TimeSpan ProjectDaysTotal = projectEnd - projectStart;
                        double ProjectDaysWeekends = totalDaysCalc(projectStart, projectEnd, holidaysList);
                        double totalProjectDays = ProjectDaysTotal.Days - ProjectDaysWeekends;
                        projectDuration = Math.Round(projectDuration, 0);

                        TimeSpan expectedDaysTotal = firstShipDate - ipfSubmit;
                        double expectedDaysWeekends = totalDaysCalc(ipfSubmit, firstShipDate, holidaysList);
                        double totalExpectedDays = expectedDaysTotal.Days - expectedDaysWeekends;

                        double floatDays = totalExpectedDays - totalProjectDays;
                        //firstMath = firstMath - removeDaysFloat;
                        dashboardPhaseRowCells.Add(data.WorkflowStep);
                        dashboardPhaseRowCells.Add("");
                        dashboardPhaseRowCells.Add(totalExpectedDays.ToString());
                        dashboardPhaseRowCells.Add(totalProjectDays.ToString());
                        dashboardPhaseRowCells.Add(floatDays.ToString());
                        dashboardPhaseRowCells.Add(floatDays.ToString());
                        dashboardPhaseRowCells.Add(0.ToString());
                        dashboardPhaseRowCells.Add("");
                        dashboardPhaseRowCells.Add("Phase");
                    dashboardPhaseRowCells.Add("");
                    dashboardPhaseRowCells.Add("");
                    dashboardRows.Add(dashboardPhaseRowCells);

                    }


                }
                if (!pageName)
                {
                    List<List<string>> displayWeek = weeksList(projectStart, projectEnd);
                    double position = getWidth(todayFields, rightNow, holidaysList, false, true);
                    position = position + 991;
                    todayPanel.Attributes.CssStyle.Add("left", position + "px");
                    int todayHeight = ((allTaskCounter + phaseCounter) * 20) + 10;
                    todayPanel.Attributes.CssStyle.Add("height", todayHeight + "px");
                    displayCalendarDays(displayWeek);
                    int numberOfWeeks = displayWeek.Count() - 1;
                    int updateLinkIndex = projectProgressDashboard.Controls.IndexOf(updateLinkPanel) + 1;
                    int weekPos = 991;
                    while (numberOfWeeks-- >= 0)
                    {
                        Panel weekLine = new Panel();
                        weekLine.Controls.Add(new LiteralControl("|"));
                        weekLine.CssClass = "weekLine";
                        weekLine.Attributes.CssStyle.Add("height", todayHeight + "px");
                        weekLine.Attributes.CssStyle.Add("left", weekPos + "px");
                        projectProgressDashboard.Controls.AddAt(updateLinkIndex, weekLine);
                        updateLinkIndex++;
                        weekPos = weekPos + 100;
                    }
                    if (!Page.IsPostBack){
                        DateTime formatDate;
                        if (DateTime.TryParse(dashboardDetails.FirstShipDate, out formatDate))
                        {
                            txtFirstShipDate.Text = DateTime.Parse(dashboardDetails.FirstShipDate).ToString("MM/dd/yyyy");
                        }
                        if (DateTime.TryParse(dashboardDetails.FirstProductionDate, out formatDate))
                        {
                            formatDate = DateTime.Parse(dashboardDetails.FirstProductionDate);
                            if (formatDate != Utilities.GetMinDate()) {
                                txtFirstProdDate.Text = DateTime.Parse(dashboardDetails.FirstProductionDate).ToString("MM/dd/yyyy");
                            }
                        }
                        
                    }
                    updatedWorkflowStatus.Controls.Add(new LiteralControl(dashboardDetails.WorkflowPhase));
                    updateButtons.Visible = false;
                }
                else
                {
                    updateButtons.Visible = true;
                    updateDateButtons.Visible = false;
                    todayPanel.Visible = false;
                    workflowStatus.Visible = false;
                    workflowChanges.Visible = false;
                }
                workflowRows(dashboardRows);
            
        }
        public static int days(DateTime start, DateTime end)
        {
            int days = 0;
            TimeSpan todayMarker = end - start;
            days = todayMarker.Days;
            return days;
        }
        public static double totalDaysCalc(DateTime start, DateTime end, List<DateTime> holidays)
        {
            int days = 0;

            DateTime tempstart = start;
            TimeSpan startEnd = end - start;
            int currentDays = startEnd.Days;
            while (currentDays-- > 0)
            {
                tempstart = tempstart.AddDays(1);

                if (tempstart.DayOfWeek == DayOfWeek.Saturday)
                {
                    //currentDays++;
                    days++;
                }
                else if (tempstart.DayOfWeek == DayOfWeek.Sunday)
                {
                    //currentDays++;
                    days++;
                }
            }
            //end = end.AddDays(days);
            foreach (DateTime holiday in holidays.Where(r => r.Date >= start.Date && r.Date <= end.Date))
            {
                if ((int)holiday.DayOfWeek != 0 && (int)holiday.DayOfWeek != 6)
                {
                    days++;
                }
            }
            return days;
        }
        public static double weekends(DateTime start, DateTime end, List<DateTime> holidays, Boolean leftSpace)
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

            if (!leftSpace)
            {
                int exclusionDays = 0;
                int exclWEOld = 0;
                int exclHDOld = 0;
                while (currentDays >= 0)
                {
                    int excHolidayTest = exclHolidays(start, tempEnd, holidays);
                    int excDayTest = exclDays(start, tempEnd);
                    if (exclusionDays == (excDayTest+ excHolidayTest))
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
        public static double getWidth(DateTime start, DateTime end, List<DateTime> weekendList, Boolean change, Boolean leftSpace)
        {
            double width = 0;
            TimeSpan days = end - start;
            TimeSpan startTime = start.TimeOfDay;
            TimeSpan endTime = end.TimeOfDay;
            if (days.Days > 0)
            {
                
                if (startTime <= TimeSpan.Parse("9:59:00"))
                {
                    width = 4;
                }
                else if (startTime >= TimeSpan.Parse("10:00:00") && startTime <= TimeSpan.Parse("11:59:00"))
                {
                    width = 3;
                }
                else if (startTime >= TimeSpan.Parse("12:00:00") && startTime <= TimeSpan.Parse("13:59:00"))
                {
                    width = 2;
                }
                else if (startTime >= TimeSpan.Parse("14:00:00"))
                {
                    width = 1;
                }
                start = start.AddDays(1);
                TimeSpan ts = new TimeSpan(0, 0, 0);
                start = start.Date + ts;

                if (endTime <= TimeSpan.Parse("9:59:00"))
                {
                    if(leftSpace) {
                        width = width + 0;
                    } else {
                        width = width + 1;
                    }
                    
                }
                else if (endTime >= TimeSpan.Parse("10:00:00") && endTime <= TimeSpan.Parse("11:59:00"))
                {
                    if (leftSpace)
                    {
                        width = width + 1;
                    }
                    else
                    {
                        width = width + 2;
                    }
                }
                else if (endTime >= TimeSpan.Parse("12:00:00") && endTime <= TimeSpan.Parse("13:59:00"))
                {
                    if (leftSpace)
                    {
                        width = width + 2;
                    }
                    else
                    {
                        width = width + 3;
                    }
                }
                else if (endTime >= TimeSpan.Parse("14:00:00"))
                {
                    if (leftSpace)
                    {
                        width = width + 3;
                    }
                    else
                    {
                        width = width + 4;
                    }
                }
                TimeSpan ets = new TimeSpan(00, 00, 00);
                end = end.Date + ets;

                
                
                TimeSpan daysSpan = end - start;
                double removeDays = weekends(start, end, weekendList, leftSpace);
                double totalDay = daysSpan.Days - removeDays;
                if (!leftSpace)
                {
                    end = end.AddDays(removeDays);
                    foreach (DateTime holiday in weekendList.Where(r => r.Date >= start.Date && r.Date <= end.Date))
                    {
                        totalDay++;
                    }
                }
                //double addBorders = (totalDay / 5) * 2;
                width = width + (totalDay * 4);// + addBorders;
            }
            else
            {
                if(startTime < TimeSpan.Parse("08:00:00")){
                    TimeSpan ts = new TimeSpan(8, 0, 0);
                    start = start.Date + ts;
                }
                if (endTime > TimeSpan.Parse("16:00:00")){
                    TimeSpan ets = new TimeSpan(15, 59, 0);
                    end = end.Date + ets;
                }
                TimeSpan newTime = end - start;
                double hourDuration = Math.Round(newTime.TotalHours/2,0);
                if(width <= 0  && change)
                {
                    width = 1;
                }
                else if(width < 0 && !change)
                {
                    width = 0;
                }
            }
            double tempwidth = (Math.Floor(width / 4) * 20) + ((width % 4) * 5);
            return tempwidth;
        }
        public static Boolean hideCurrentRow(string taskQuickName, List<KeyValuePair<string,string>> hideRows)
        {
            Boolean hideThisRow = false;
            List<string> qualifiers = new List<string>(new string[] { "InitialCosting", "InitialCapacity", "Distribution", "ExternalMfg", "SAPInitialSetup", "PrelimSAPInitialSetup", "NewXferSemi", "TradePromo","RnDFinalCheck", "SrOBMApproval2" });
            if (qualifiers.Contains(taskQuickName))
            {
                foreach (KeyValuePair<string,string> taskCheck in hideRows.Where(r => r.Key == taskQuickName))
                {                
                    if (taskQuickName == "InitialCosting" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }else if(taskQuickName == "InitialCapacity" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else if (taskQuickName == "ExternalMfg")
                    {

                        if (taskCheck.Value != "Externally Manufactured" && taskCheck.Value != "Externally Packed")
                        {
                            hideThisRow = true;
                            continue;
                        }
                        else if (taskCheck.Value == "Externally Manufactured" || taskCheck.Value == "Externally Packed")
                        {
                            hideThisRow = false;
                            break;
                        }
                    }
                    else if(taskQuickName == "NewXferSemi")
                    {
                        if (taskCheck.Value.Contains("NewTS") && taskCheck.Value.Contains("Yes"))
                        {
                            hideThisRow = false;
                            continue;
                        }
                        else if (taskCheck.Value.Contains("Bulk") && (taskCheck.Value.Contains("No") || taskCheck.Value == ""))
                        {
                            hideThisRow = false;
                            continue;
                        }
                        else
                        {
                            hideThisRow = true;
                            break;
                        }
                    }
                    else if (taskQuickName == "Distribution" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else if (taskQuickName == "SAPInitialSetup")
                    {
                        if (taskCheck.Value.ToLower().Contains("seasonal"))
                        {
                            hideThisRow = true;
                            break;
                        }
                        else
                        {
                            if (taskCheck.Value.ToLower().Contains("yes"))
                            {
                                hideThisRow = false;
                                continue;
                            }
                            else
                            {
                                hideThisRow = true;
                                continue;
                            }
                        }
                        
                    }
                    else if (taskQuickName == "PrelimSAPInitialSetup")
                    {
                        if (taskCheck.Value.ToLower().Contains("seasonal"))
                        {
                            if (taskCheck.Value.ToLower().Contains("yes"))
                            {
                                hideThisRow = false;
                                continue;
                            }
                            else
                            {
                                hideThisRow = true;
                                break;
                            }
                        }
                        else
                        {
                            hideThisRow = true;
                            break;
                        }

                    }
                    else if (taskQuickName == "TradePromo" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else if (taskQuickName == "SrOBMApproval2")
                    {
                        if (taskCheck.Value == "Yes")
                        {
                            hideThisRow = false;
                            break;
                        }
                        else if (taskCheck.Value != "Yes")
                        {
                            hideThisRow = true;
                            continue;
                        }
                    }
                    if (taskCheck.Value == "Graphics Changes/Internal Adjustments")
                    {
                        if (taskQuickName == "TradePromo" || taskQuickName == "Distribution" || taskQuickName == "SAPInitialSetup" || taskQuickName == "PrelimSAPInitialSetup" || taskQuickName == "RnDFinalCheck")
                        {
                            hideThisRow = true;
                            break;
                        }
                    }
                }
            }
            return hideThisRow;
        }
        public static string hideCurrentRowReason(string taskQuickName, List<KeyValuePair<string,string>> hideRows)
        {
            string reason = "";
            bool hideThisRow = false;
            List<string> qualifiers = new List<string>(new string[] { "InitialCosting", "InitialCapacity", "Distribution", "ExternalMfg", "SAPInitialSetup", "PrelimSAPInitialSetup", "NewXferSemi", "TradePromo", "RnDFinalCheck", "SrOBMApproval2" });
            if (qualifiers.Contains(taskQuickName))
            {
                foreach (KeyValuePair<string,string> taskCheck in hideRows.Where(r => r.Key == taskQuickName))
                {
                    if (taskQuickName == "InitialCosting" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else if (taskQuickName == "InitialCapacity" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else if (taskQuickName == "ExternalMfg")
                    {

                        if (taskCheck.Value != "Externally Manufactured" && taskCheck.Value != "Externally Packed")
                        {
                            hideThisRow = true;
                            continue;
                        }
                        else if (taskCheck.Value == "Externally Manufactured" || taskCheck.Value == "Externally Packed")
                        {
                            hideThisRow = false;
                            break;
                        }
                    }
                    else if (taskQuickName == "NewXferSemi")
                    {
                        if (taskCheck.Value.Contains("NewTS") && taskCheck.Value.Contains("Yes"))
                        {
                            hideThisRow = false;
                            continue;
                        }
                        else if (taskCheck.Value.Contains("Bulk") && (taskCheck.Value.Contains("No") || taskCheck.Value == ""))
                        {
                            hideThisRow = false;
                            continue;
                        }
                        else
                        {
                            hideThisRow = true;
                            break;
                        }
                    }
                    else if (taskQuickName == "Distribution" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else if (taskQuickName == "SAPInitialSetup")
                    {
                        if (taskCheck.Value.ToLower().Contains("seasonal"))
                        {
                            hideThisRow = true;
                            break;
                        }
                        else
                        {
                            if (taskCheck.Value.ToLower().Contains("yes"))
                            {
                                hideThisRow = false;
                                continue;
                            }
                            else
                            {
                                hideThisRow = true;
                                continue;
                            }
                        }

                    }
                    else if (taskQuickName == "PrelimSAPInitialSetup")
                    {
                        if (taskCheck.Value.ToLower().Contains("seasonal"))
                        {
                            if (taskCheck.Value.ToLower().Contains("yes"))
                            {
                                hideThisRow = false;
                                continue;
                            }
                            else
                            {
                                hideThisRow = true;
                                break;
                            }
                        }
                        else
                        {
                            hideThisRow = true;
                            break;
                        }

                    }
                    else if (taskQuickName == "TradePromo" && taskCheck.Value != "Yes")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else if (taskQuickName == "SrOBMApproval2")
                    {
                        if (taskCheck.Value == "Yes")
                        {
                            hideThisRow = false;
                            break;
                        }
                        else if (taskCheck.Value != "Yes")
                        {
                            hideThisRow = true;
                            continue;
                        }
                    }
                    if (taskCheck.Value == "Graphics Changes/Internal Adjustments")
                    {
                        if (taskQuickName == "TradePromo" || taskQuickName == "Distribution" || taskQuickName == "SAPInitialSetup" || taskQuickName == "PrelimSAPInitialSetup" || taskQuickName == "RnDFinalCheck")
                        {
                            hideThisRow = true;
                            break;
                        }
                    }
                }
            }
            return reason;
        }
        protected void updateDate(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            string clickedId = clicked.ID;
            int compassId = int.Parse(hddProjectId.Value);
            string whichUpdate = "";
            string date = "";
            if (clickedId == "btnFirstShipDate")
            {
                date = txtFirstShipDate.Text;
                whichUpdate = "ship";
            }
            if (clickedId == "btnFirstProdDate")
            {
                date = txtFirstProdDate.Text;
                whichUpdate = "prod";

            }
            timelineNumbers.dashboardUpdateDates(compassId, whichUpdate, date);
            Page.Response.Redirect(HttpContext.Current.Request.RawUrl);
            
        }
        protected void updateWorkflowStatus(object sender, EventArgs e)
        {
            Button clicked = (Button)sender;
            string clickedId = clicked.Text;
            int compassId = int.Parse(hddProjectId.Value);
            if (clickedId == "Change Request")
                Page.Response.Redirect("/Pages/ItemProposal.aspx?ProjectNo=" + ProjectNumber + "&IPFMode=" + GlobalConstants.QUERYSTRINGVALUE_IPFChange, false);
            else if (clickedId == "Copy Request")
                Page.Response.Redirect("/Pages/ItemProposal.aspx?ProjectNo=" + ProjectNumber + "&IPFMode=" + GlobalConstants.QUERYSTRINGVALUE_IPFCopy, false);
            else if (clickedId == "Export Request")
                Export(compassId);
            else
            {
                timelineNumbers.workflowStatusUpdate(compassId, clickedId);
                workflowService.UpdateWorkflowPhase(compassId, clickedId);
            }
        }
        #region Export
        private void Export(int compassId)
        {
            string fileName;
            byte[] fileContent;
            List<Dictionary<string, string>> itemRows = null;
            Dictionary<string, string> publishRow = null, linkRow = null;
            fileName = GlobalConstants.EXP_SYNC_FILENAME.Replace("{compassId}", compassId.ToString());
            GetExportData(compassId, ref itemRows, ref publishRow, ref linkRow);
            fileContent = exportService.WriteToFile(ProjectNumber, itemRows, publishRow, linkRow);
            Page.Response.Clear();
            Page.Response.ContentType = "application/force-download";
            Page.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Page.Response.BinaryWrite(fileContent);
            Page.Response.End();
        }
        private void GetExportData(int compassId, ref List<Dictionary<string, string>> itemRows, ref Dictionary<string, string> publishRow,
            ref Dictionary<string, string> linkRow)
        {
            ItemProposalItem proposalItem;
            CompassPackMeasurementsItem measure;
            Dictionary<string, string> values;
            string todayDate, childDescription, onz = "";
            todayDate = DateTime.Now.ToString("s");
            proposalItem = proposalService.GetItemProposalItem(compassId);
            measure = BOMservice.GetPackMeasurementsItem(compassId, 0);
            childDescription = GetChildDescription(proposalItem.SAPDescription, ref onz);
            itemRows = new List<Dictionary<string, string>>();
            values = new Dictionary<string, string>();
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, proposalItem.CaseUCC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemName, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_BrandName, proposalItem.MaterialGroup1Brand);
            values.Add(GlobalConstants.EXP_SYNC_ITM_Depth, Utilities.FormatDecimal(measure.CaseDimensionLength, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Height, Utilities.FormatDecimal(measure.CaseDimensionHeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Width, Utilities.FormatDecimal(measure.CaseDimensionWidth, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GrossWeight, Utilities.FormatDecimal(measure.CaseGrossWeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_NetWeight, Utilities.FormatDecimal(measure.CaseNetWeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GS1TradeItemIDKeyValue, proposalItem.CaseUCC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ShortDescription, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ProductDescription, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_FunctionalName, proposalItem.SAPDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_Volume, Utilities.FormatDecimal(measure.CaseCube, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_QtyofNextLevelItem, Utilities.FormatDecimal(proposalItem.RetailSellingUnitsBaseUOM, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_NumberofItemsinaCompleteLayerGTINPalletTi, Utilities.FormatDecimal(measure.CasesPerLayer, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_NumberofCompleteLayersContainedinItemGTINPalletHi, Utilities.FormatDecimal(measure.LayersPerPallet, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_AlternateItemIdentificationId, proposalItem.SAPItemNumber);
            values.Add(GlobalConstants.EXP_SYNC_ITM_MinProductLifespanfromProduction, GetShelfLife(compassId));
            values.Add(GlobalConstants.EXP_SYNC_ITM_StartAvailabilityDate, todayDate);
            values.Add(GlobalConstants.EXP_SYNC_ITM_EffectiveDate, todayDate);
            itemRows.Add(values);
            values = new Dictionary<string, string>(1);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, string.Empty);
            itemRows.Add(values);
            values = new Dictionary<string, string>();
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemID, proposalItem.UnitUPC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ItemName, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_BrandName, proposalItem.MaterialGroup1Brand);
            values.Add(GlobalConstants.EXP_SYNC_ITM_Depth, Utilities.FormatDecimal(measure.UnitDimensionLength, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Height, Utilities.FormatDecimal(measure.UnitDimensionHeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_Width, Utilities.FormatDecimal(measure.UnitDimensionWidth, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GrossWeight, onz);
            values.Add(GlobalConstants.EXP_SYNC_ITM_NetWeight, Utilities.FormatDecimal(measure.NetUnitWeight, 2));
            values.Add(GlobalConstants.EXP_SYNC_ITM_GS1TradeItemIDKeyValue, proposalItem.UnitUPC);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ShortDescription, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_ProductDescription, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_FunctionalName, childDescription);
            values.Add(GlobalConstants.EXP_SYNC_ITM_StartAvailabilityDate, todayDate);
            values.Add(GlobalConstants.EXP_SYNC_ITM_EffectiveDate, todayDate);
            itemRows.Add(values);
            publishRow = new Dictionary<string, string>();
            publishRow.Add(GlobalConstants.EXP_SYNC_PUB_ItemID, proposalItem.CaseUCC);
            publishRow.Add(GlobalConstants.EXP_SYNC_PUB_PublishDate, todayDate);
            linkRow = new Dictionary<string, string>();
            linkRow.Add(GlobalConstants.EXP_SYNC_LKN_ParentItemID, proposalItem.CaseUCC);
            linkRow.Add(GlobalConstants.EXP_SYNC_LKN_ChildItemID, proposalItem.UnitUPC);
            linkRow.Add(GlobalConstants.EXP_SYNC_LKN_QtyofChildItem, Utilities.FormatDecimal(proposalItem.RetailSellingUnitsBaseUOM, 2));
        }
        private string GetShelfLife(int compassId)
        {
            List<PackagingItem> packagingItems;
            int current, shelfLife = int.MaxValue;
            packagingItems = packagingService.GetCandySemiItemsForProject(compassId);
            foreach(PackagingItem packItem in packagingItems)
            {
                if (packItem.ShelfLife == "")
                    continue;
                current = int.Parse(packItem.ShelfLife);
                if (current < shelfLife)
                    shelfLife = current;
            }
            return shelfLife == int.MaxValue ? "" : shelfLife.ToString();
        }
        private string GetChildDescription(string description, ref string onz)
        {
            int slashIdx, ozIdx, w;
            char ch;
            bool otherCharsFound;
            otherCharsFound = false;
            slashIdx = description.IndexOf('/');
            if (slashIdx <= 0 || slashIdx == description.Length - 1)
                return description;
            for(w = slashIdx - 1; w > 0; w--)
            {
                ch = description[w];
                if (ch == ' ')
                {
                    if (otherCharsFound)
                        break;
                }
                else
                    otherCharsFound = true;
            }
            if (w > 0)
            {
                ozIdx = description.ToLower().IndexOf("oz", slashIdx);
                if (ozIdx > -1)
                    onz = description.Substring(slashIdx + 1, ozIdx - slashIdx - 1).Trim();
                return description.Substring(0, w).Trim() + " " + description.Substring(slashIdx + 1);
            }
            return description;
        }
        #endregion
        private string ValidateForm()
        {
            string bValid = "";
            TextBox tbox;
            foreach (TableRow tr in projectDashboard.Rows)
            {
                foreach (Control c in tr.Cells[7].Controls)
                {
                    if (c is TextBox)
                    {
                        tbox = c as TextBox;
                        if (tbox.Text == "")
                        {
                            bValid = tbox.ID;
                            break;
                        }
                    }
                }
            }

            return bValid;
        }

        private void ConstructFormData()
        {
            foreach (TableRow tr in projectDashboard.Rows)
            {
                foreach (TableCell tc in tr.Cells)
                {
                    foreach (Control c in tc.Controls)
                    {
                        if (c.GetType() == typeof(TextBox))
                        {
                            TextBox tb = (TextBox)c;
                            if (tb.Text != "" && tb.ReadOnly != true)
                            {
                                string WFQuickStep = tb.ID.Replace("txt", "");
                                timelineUpdater.UpdateSingleProjectTimelineItem(WFQuickStep, tb.Text, iItemId, "", ProjectNumber);
                            }
                        }
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == "")
            {
                ConstructFormData();
                Page.Response.Redirect("/Pages/TesTDASHBOARDFOREXPORT.aspx?ProjectNo=" + ProjectNumber+"&updatepage=false", true);
            }
            else
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Please enter a value for all fields.');", true);
            }

        }
        protected Boolean existingEntry(int compassId)
        {
            Boolean exists = false;
            try
            {
                int projectCount = timelineUpdater.GetProjectTimelineItem(compassId);
                if (projectCount > 0)
                {
                    exists = true;
                }
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
            }
            return exists;
        }
    }
}
