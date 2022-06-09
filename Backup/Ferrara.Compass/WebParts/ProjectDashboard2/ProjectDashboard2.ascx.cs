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
using Microsoft.SharePoint;

namespace Ferrara.Compass.WebParts.ProjectDashboard2
{
    [ToolboxItemAttribute(false)]
    public partial class ProjectDashboard2 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        #region Member Variables
        private IDashboardService dashboardService;
        private IApprovalService approvalService;
        private IProjectTimelineUpdateService timelineUpdater;
        private IExceptionService exceptionService;
        private static IProjectTimelineTypeService timelineNumbers;
        private IWorkflowService workflowService;
        private IExcelExportSyncService exportService;
        private IItemProposalService proposalService;
        private IBillOfMaterialsService BOMservice;
        private IPackagingItemService packagingService;
        private IStageGateCreateProjectService stageGateService;
        private int iItemId = 0;
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
        public ProjectDashboard2()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            approvalService = DependencyResolution.DependencyMapper.Container.Resolve<IApprovalService>();
            timelineNumbers = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
            timelineUpdater = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineUpdateService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            proposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            BOMservice = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            exportService = DependencyResolution.DependencyMapper.Container.Resolve<IExcelExportSyncService>();
            stageGateService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
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
        public void workflowRows(List<ProjectStatusReportItem> columnValues, List<ProjectStatusReportItem> OGcolumnValues)
        {
            int taskCount = 0;
            int taskIndex = 0;
            foreach (ProjectStatusReportItem cells in columnValues)
            {
                taskIndex = columnValues.IndexOf(cells);
                TableRow workflowRow = new TableRow();
                //Process Column
                TableCell workflowName = new TableCell();

                workflowName.Attributes.Add("class", "processCol");
                string workflowEmailText = "";
                if (cells.Checks == "Phase")
                {
                    workflowName.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + cells.Process));
                    workflowRow.CssClass = "phaseRow";
                    workflowName.Attributes.Add("class", "collapseProcess");
                }
                else if (cells.Checks == "PackagingItem")
                {
                    workflowName.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + cells.Process));
                    workflowRow.CssClass = "packagingRow";
                    workflowName.Attributes.Add("class", "collapseProcess");
                }
                else if (cells.Checks == "PROC")
                {
                    workflowName.Controls.Add(new LiteralControl("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + cells.Process));
                    workflowRow.CssClass = "PROCRow";
                    workflowEmailText = cells.Email;
                }
                else if (cells.WorflowQuickStep == "BOMSetupProc")
                {
                    workflowName.Controls.Add(new LiteralControl(cells.Process));
                }
                else
                {
                    workflowName.Controls.Add(new LiteralControl(cells.Process));
                    workflowEmailText = cells.Email;
                }
                workflowRow.Cells.Add(workflowName);
                //Spacing Column
                TableCell workflowCellSpacing4 = new TableCell();
                workflowCellSpacing4.Attributes.Add("class", "spacingCol");
                workflowRow.Cells.Add(workflowCellSpacing4);
                //Status Column
                TableCell workflowStatus = new TableCell();
                workflowStatus.Controls.Add(new LiteralControl(cells.Status));
                workflowStatus.Attributes.Add("class", "statusCol");
                workflowRow.Cells.Add(workflowStatus);
                //SubmittedBy Column
                TableCell workflowSubmittedBy = new TableCell();
                workflowSubmittedBy.Controls.Add(new LiteralControl(cells.SubmittedBy));
                workflowSubmittedBy.Attributes.Add("class", "submittedCol");
                workflowRow.Cells.Add(workflowSubmittedBy);
                //Email Column
                TableCell workflowEmail = new TableCell();
                workflowEmail.Controls.Add(new LiteralControl(workflowEmailText));
                workflowEmail.Attributes.Add("class", "emailCol");
                workflowRow.Cells.Add(workflowEmail);
                //Spacing Column
                TableCell workflowCellSpacing1 = new TableCell();
                workflowCellSpacing1.Attributes.Add("class", "spacingCol");
                workflowRow.Cells.Add(workflowCellSpacing1);
                //OG Start Date Column
                TableCell OGworkflowStart = new TableCell();
                if (cells.WorflowQuickStep != "Float")
                {
                    if (OGcolumnValues[taskIndex].OGStartDay != DateTime.MinValue)
                    {
                        OGworkflowStart.Controls.Add(new LiteralControl(OGcolumnValues[taskIndex].OGStartDay.ToString("MM/dd/yyyy")));
                    }
                }
                else
                {
                    OGworkflowStart.Controls.Add(new LiteralControl(OGcolumnValues[OGcolumnValues.Count - 1].Color.Split(';').ToList<string>()[0]));
                }
                OGworkflowStart.Attributes.Add("class", "startDayCol");
                workflowRow.Cells.Add(OGworkflowStart);
                //OG End Date Column
                TableCell OGworkflowEnd = new TableCell();
                if (cells.WorflowQuickStep != "Float")
                {
                    if (OGcolumnValues[taskIndex].OGEndDay != DateTime.MinValue)
                    {
                        OGworkflowEnd.Controls.Add(new LiteralControl(OGcolumnValues[taskIndex].OGEndDay.ToString("MM/dd/yyyy")));
                    }
                }
                else
                {
                    OGworkflowEnd.Controls.Add(new LiteralControl(OGcolumnValues[OGcolumnValues.Count - 1].Color.Split(';').ToList<string>()[1]));
                }
                OGworkflowEnd.Attributes.Add("class", "endDayCol");
                workflowRow.Cells.Add(OGworkflowEnd);
                //OG Total Days Column
                TableCell OGworkflowTotal = new TableCell();
                if (cells.Checks == "PackagingItem")
                {
                    OGworkflowTotal.Controls.Add(new LiteralControl(""));
                }
                else
                {
                    if (OGcolumnValues[taskIndex].OGDuration >= 0 && cells.WorflowQuickStep != "Float")
                    {
                        double totalDaysDisplay = Math.Round(OGcolumnValues[taskIndex].OGDuration, 0);
                        OGworkflowTotal.Controls.Add(new LiteralControl(totalDaysDisplay.ToString()));
                    }
                    else if (cells.WorflowQuickStep == "Float")
                    {
                        double totalDaysDisplay = Math.Round(OGcolumnValues[taskIndex].OGDuration, 0);
                        OGworkflowTotal.Controls.Add(new LiteralControl(totalDaysDisplay.ToString()));
                    }
                    else
                    {
                        OGworkflowTotal.Controls.Add(new LiteralControl(""));
                    }
                }
                OGworkflowTotal.Attributes.Add("class", "totalDaysCol");
                workflowRow.Cells.Add(OGworkflowTotal);
                //Spacing Column
                TableCell workflowCellSpacing10 = new TableCell();
                workflowCellSpacing10.Attributes.Add("class", "spacingCol");
                workflowRow.Cells.Add(workflowCellSpacing10);
                //Actual Start Date Column
                TableCell workflowStart = new TableCell();
                if (cells.WorflowQuickStep != "Float")
                {
                    if (cells.ActualStartDay != DateTime.MinValue)
                    {

                        workflowStart.Controls.Add(new LiteralControl(cells.ActualStartDay.ToString("MM/dd/yyyy")));
                    }
                }
                else
                {
                    workflowStart.Controls.Add(new LiteralControl(cells.Color.Split(';').ToList<string>()[0]));
                }
                workflowStart.Attributes.Add("class", "startDayCol");
                workflowRow.Cells.Add(workflowStart);
                //Actual End Date Column
                TableCell workflowEnd = new TableCell();
                if (cells.WorflowQuickStep != "Float")
                {
                    if (cells.ActualEndDay != DateTime.MinValue)
                    {

                        workflowEnd.Controls.Add(new LiteralControl(cells.ActualEndDay.ToString("MM/dd/yyyy")));
                    }
                }
                else
                {
                    workflowEnd.Controls.Add(new LiteralControl(cells.Color.Split(';').ToList<string>()[1]));
                }
                workflowEnd.Attributes.Add("class", "endDayCol");
                workflowRow.Cells.Add(workflowEnd);
                //Actual Total Days Column
                TableCell workflowTotal = new TableCell();
                if (cells.Checks == "PackagingItem")
                {
                    workflowTotal.Controls.Add(new LiteralControl(""));
                }
                else
                {
                    if (cells.ActualDuration >= 0 && cells.WorflowQuickStep != "Float")
                    {
                        double totalDaysDisplay = Math.Round(cells.ActualDuration, 0);
                        workflowTotal.Controls.Add(new LiteralControl(totalDaysDisplay.ToString()));
                    }
                    else if (cells.WorflowQuickStep == "Float")
                    {
                        double totalDaysDisplay = Math.Round(cells.ActualDuration, 0);
                        workflowTotal.Controls.Add(new LiteralControl(totalDaysDisplay.ToString()));
                    }
                    else
                    {
                        workflowTotal.Controls.Add(new LiteralControl(""));
                    }
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
                double calendarCols = (headerRow.Cells.Count - 8) * 20;
                workflowCalendar.ColumnSpan = Convert.ToInt32(calendarCols);
                Panel calendarDisplay = new Panel();
                if (cells.Color != "" && Convert.ToDouble(cells.ActualDuration) != 0)
                {
                    if (pageName)
                    {
                        if (!string.IsNullOrEmpty(cells.ReadOnly))
                        {
                            TextBox tb = new TextBox();
                            tb.ID = "txt" + cells.WorflowQuickStep;
                            tb.Text = cells.ActualDuration.ToString();
                            tb.ReadOnly = Convert.ToBoolean(cells.ReadOnly);
                            tb.CssClass = "form-control";
                            calendarDisplay.Controls.Add(tb);
                        }
                        workflowCalendar.Attributes.Add("class", "updatePage");
                        workflowCalendar.Controls.Add(calendarDisplay);

                    }
                    else
                    {
                        double colspan = cells.PixelsFromLeft;
                        calendarCols = calendarCols - colspan;
                        calendarDisplay.Width = Convert.ToInt32(cells.Width);
                        calendarDisplay.CssClass = cells.Color;
                        calendarDisplay.Attributes.CssStyle.Add("margin-left", cells.PixelsFromLeft + "px");
                        calendarDisplay.Controls.Add(new LiteralControl("|&nbsp;&nbsp;"));
                        workflowCalendar.Controls.Add(calendarDisplay);
                    }
                }
                //workflowRow.Controls.Add(workflowCalendarLeft);
                workflowRow.Controls.Add(workflowCalendar);


                projectDashboard.Rows.Add(workflowRow);

                taskCount++;
            }
        }
        public void displayCalendarDays(List<List<String>> displayWeek, string todayHeight)
        {
            foreach (List<String> i in displayWeek)
            {
                TableCell calendarWeek = new TableCell();
                //calendarWeek.ColumnSpan = 20;
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

                int numberOfWeeks = displayWeek.Count() - 1;
                int weekPos = 1303;
                Panel weekLine = new Panel();
                weekLine.Controls.Add(new LiteralControl("|"));
                weekLine.CssClass = "weekLine";
                weekLine.Attributes.CssStyle.Add("height", todayHeight + "px");
                calendarWeek.Controls.Add(weekLine);
                weekPos = weekPos + 105;
                headerRow.Cells.Add(calendarWeek);
                //dailyDisplay.Cells.Add(calendarDay);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            projectStatusReport();
        }
        private void projectStatusReport()
        {

            if (ProjectNumber == "")
            {
                Page.Response.Redirect("/Pages/TaskDashboard.aspx?error=true&errorText=Projent Number is Required to view the Project Dashboard", false);
            }
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            hddProjectId.Value = iItemId.ToString();
            hdnProjectNumber.Value = ProjectNumber;

            bool addLink = true;

            copyrequest.Visible = true;

            var ShowButtons = CheckUserPrivileges(iItemId);

            if (ShowButtons)
            {
                txtFirstShipDate.Enabled = true;
                btnFirstShipDate.Visible = true;

                txtFirstProdDate.Enabled = true;
                btnFirstProdDate.Visible = true;

                preproduction.Visible = true;
                onhold.Visible = true;
                completed.Visible = true;
                cancelled.Visible = true;
                changerequest.Visible = true;
                exportrequest.Visible = true;

                btnSave.Visible = true;
            }
            else
            {
                txtFirstShipDate.Enabled = false;
                btnFirstShipDate.Visible = false;

                txtFirstProdDate.Enabled = false;
                btnFirstProdDate.Visible = false;

                preproduction.Visible = false;
                onhold.Visible = false;
                completed.Visible = false;
                cancelled.Visible = false;
                changerequest.Visible = false;
                exportrequest.Visible = false;

                btnSave.Visible = false;

                addLink = false;
            }

            if (!pageName && addLink)
            {
                HtmlAnchor updateLink = new HtmlAnchor();

                updateLink.Controls.Add(new LiteralControl("Update Total Days Fields"));
                updateLink.HRef = "/Pages/ProjectStatus.aspx?ProjectNo=" + ProjectNumber + "&updatepage=true";
                updateLinkPanel.Controls.Add(updateLink);
            }

            DashboardDetailsItem dashboardDetails = timelineNumbers.dashboardDetails(iItemId);
            testPanel.Controls.Add(new LiteralControl(dashboardDetails.ProjectName));

            string openWindow = @"javascript:window.open('CommercializationItem.aspx?ProjectNo=" + ProjectNumber + "'); return false;";
            lbDisplayCommForm.Attributes.Add("onclick", openWindow);
            string parentProjectNumber = dashboardDetails.ParentProjectNumber;

            if (string.IsNullOrEmpty(parentProjectNumber) || parentProjectNumber == "0")
            {
                if (string.IsNullOrEmpty(dashboardDetails.StageGateProjectListItemId) || dashboardDetails.StageGateProjectListItemId == "0")
                {
                    lbDisplayParentSummaryForm.Visible = false;
                }
                else
                {
                    var stageGateListItem = stageGateService.GetStageGateProjectItem(Convert.ToInt32(dashboardDetails.StageGateProjectListItemId));
                    lbDisplayParentSummaryForm.Visible = string.IsNullOrEmpty(stageGateListItem.ProjectNumber) ? false : true;
                    parentProjectNumber = stageGateListItem.ProjectNumber;
                }
            }

            string openWindow2 = @"javascript:window.open('StageGateProjectPanel.aspx?ProjectNo=" + parentProjectNumber + "'); return false;";
            lbDisplayParentSummaryForm.Attributes.Add("onclick", openWindow2);

            dashboardDetails.ExistingItem = existingEntry(iItemId);
            var projectStatusReportDetails = timelineNumbers.actualTimeLine(dashboardDetails, pageName);
            List<ProjectStatusReportItem> dashboardRows = projectStatusReportDetails.Item1;
            if (!projectStatusReportDetails.Item7)
            {
                Panel submitDateMessage = new Panel();
                submitDateMessage.CssClass = "markrequired";
                submitDateMessage.ID = "submitDateMessage";
                submitDateMessage.ClientIDMode = ClientIDMode.Static;
                submitDateMessage.Controls.Add(new LiteralControl("This status report is an estimate of the current project based on today’s date. After submittal of the current project, dates are subject to change."));
                phMessaging.Controls.Add(submitDateMessage);
            }

            if (!pageName)
            {
                List<List<string>> displayWeek = weeksList(dashboardRows[0].ActualStartDay, dashboardRows[dashboardRows.Count - 2].ActualEndDay);
                double position = timelineNumbers.getWidth(projectStatusReportDetails.Item5, DateTime.Now, projectStatusReportDetails.Item6, false, true);
                position = position + 1300;
                todayPanel.Attributes.CssStyle.Add("left", position + "px");
                int todayHeight = ((projectStatusReportDetails.Item3 + projectStatusReportDetails.Item4) * 20) + 13;
                todayPanel.Attributes.CssStyle.Add("height", todayHeight + "px");
                displayCalendarDays(displayWeek, todayHeight.ToString());

                if (!Page.IsPostBack)
                {
                    DateTime formatDate;
                    if (DateTime.TryParse(dashboardDetails.FirstShipDate, out formatDate))
                    {
                        txtFirstShipDate.Text = DateTime.Parse(dashboardDetails.FirstShipDate).ToString("MM/dd/yyyy");
                    }
                    if (DateTime.TryParse(dashboardDetails.FirstProductionDate, out formatDate))
                    {
                        formatDate = DateTime.Parse(dashboardDetails.FirstProductionDate);
                        if (formatDate != Utilities.GetMinDate())
                        {
                            txtFirstProdDate.Text = DateTime.Parse(dashboardDetails.FirstProductionDate).ToString("MM/dd/yyyy");
                        }
                    }
                }
                string currentPhase = dashboardDetails.WorkflowPhase;
                string ProjectType = dashboardDetails.CompassProjectType;
                string ProjectTypeSubCategory = dashboardDetails.ProjectTypeSubCategory;
                if (currentPhase.ToLower() == "cancelled")
                {
                    updatedWorkflowStatus.CssClass = "redMessage";
                }

                if (currentPhase == GlobalConstants.WORKFLOWPHASE_SrOBMInitialReview)
                {
                    updatedWorkflowStatus.Controls.Add(new LiteralControl(GlobalConstants.WORKFLOWPHASE_PMInitialReview));
                }
                else
                {
                    updatedWorkflowStatus.Controls.Add(new LiteralControl(currentPhase.Replace("OBM", "PM")));
                }
                updatedProjectType.Controls.Add(new LiteralControl("Project Type: " + ProjectType));
                updatedProjectTypeSubCategory.Controls.Add(new LiteralControl("Project Type SubCategory: " + ProjectTypeSubCategory));
                if (ProjectTypeSubCategory == "NA")
                {
                    h2ProjectTypeSubCategory.Attributes.CssStyle.Add("visibility", "collapse");
                }
                if (string.Equals(currentPhase, GlobalConstants.WORKFLOWPHASE_OnHold))
                {
                    onhold.Visible = false;
                    removeOnHold.Visible = ShowButtons;
                }
                else
                {
                    onhold.Visible = ShowButtons;
                    removeOnHold.Visible = false;
                }
                updateButtons.Visible = false;
            }
            else
            {
                updateButtons.Visible = ShowButtons;
                updateDateButtons.Visible = false;
                todayPanel.Visible = false;
                workflowStatus.Visible = false;
                workflowChanges.Visible = false;
            }
            workflowRows(dashboardRows, projectStatusReportDetails.Item2);
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
            else if (clickedId == "On Hold")
            {
                timelineNumbers.workflowStatusUpdate(compassId, "onhold");
                workflowService.UpdateWorkflowPhase(compassId, "On Hold");
                Page.Response.Redirect("/Pages/ProjectStatus.aspx?ProjectNo=" + ProjectNumber + "&updatepage=false", true);
            }
            else
            {
                timelineNumbers.workflowStatusUpdate(compassId, clickedId);
                workflowService.UpdateWorkflowPhase(compassId, clickedId);
                Page.Response.Redirect("/Pages/ProjectStatus.aspx?ProjectNo=" + ProjectNumber + "&updatepage=false", true);
            }
            exceptionService.Handle(LogCategory.General, "updateWorkflowStatus", "Project Dashboard", "updateWorkflowStatus", "Project Number" + ProjectNumber + "; clicked: " + clickedId);
        }
        protected void reactivateProject(object sender, EventArgs e)
        {
            int compassId = int.Parse(hddProjectId.Value);
            timelineNumbers.workflowStatusUpdate(compassId, "OnHold");
            string OnHoldWorkFlowPhase = Utilities.GetOnHoldWorkFlowPhase(compassId);
            workflowService.UpdateWorkflowPhase(compassId, "Remove On Hold");

            if (GlobalConstants.WORKFLOWPHASE_PreProduction != OnHoldWorkFlowPhase)
            {
                workflowService.StartSpecificWorkflow(compassId, "1 - Start PMT Workflow");
            }
            onhold.Visible = true;
            removeOnHold.Visible = false;
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
            foreach (PackagingItem packItem in packagingItems)
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
            for (w = slashIdx - 1; w > 0; w--)
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
            string tboxID = "";
            try
            {
                TextBox tbox;
                foreach (TableRow tr in projectDashboard.Rows)
                {
                    foreach (Control c in tr.Cells[7].Controls)
                    {
                        if (c is TextBox)
                        {
                            tbox = c as TextBox;
                            tboxID = tbox.ID;
                            if (tbox.Text == "")
                            {
                                bValid = tbox.ID;
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Project Status Update Form: Project No" + ": " + ProjectNumber + "; TextboxID: " + tboxID + "; error: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "Project Status Update Form", "ValidateForm");
            }
            return bValid;
        }
        private ProjectTimelineItem ConstructFormData()
        {
            ProjectTimelineItem item = new ProjectTimelineItem();
            double convertedDouble;

            try
            {
                item.CompassListItemId = iItemId;
                item.IPF = 0;
                if (projectDashboard.FindControl("txtSrOBMApproval") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSrOBMApproval")).Text, out convertedDouble))
                {
                    item.SrOBMApproval = convertedDouble;
                }
                else
                {
                    item.SrOBMApproval = 0;
                }
                if (projectDashboard.FindControl("txtPrelimSAPInitialSetup") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtPrelimSAPInitialSetup")).Text, out convertedDouble))
                {
                    item.PrelimSAPInitialSetup = convertedDouble;
                }
                else
                {
                    item.SrOBMApproval = 0;
                }
                if (projectDashboard.FindControl("txtSrOBMApproval2") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSrOBMApproval2")).Text, out convertedDouble))
                {
                    item.SrOBMApproval2 = convertedDouble;
                }
                else
                {
                    item.SrOBMApproval2 = 0;
                }
                if (projectDashboard.FindControl("txtInitialCosting") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtInitialCosting")).Text, out convertedDouble))
                {
                    item.InitialCosting = convertedDouble;
                }
                else
                {
                    item.InitialCosting = 0;
                }
                if (projectDashboard.FindControl("txtInitialCapacity") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtInitialCapacity")).Text, out convertedDouble))
                {
                    item.InitialCapacity = convertedDouble;
                }
                else
                {
                    item.InitialCapacity = 0;
                }
                if (projectDashboard.FindControl("txtTradePromo") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtTradePromo")).Text, out convertedDouble))
                {
                    item.TradePromo = convertedDouble;
                }
                else
                {
                    item.TradePromo = 0;
                }
                if (projectDashboard.FindControl("txtDistribution") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtDistribution")).Text, out convertedDouble))
                {
                    item.Distribution = convertedDouble;
                }
                else
                {
                    item.Distribution = 0;
                }
                if (projectDashboard.FindControl("txtOperations") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtOperations")).Text, out convertedDouble))
                {
                    item.Operations = convertedDouble;
                }
                else
                {
                    item.Operations = 0;
                }
                if (projectDashboard.FindControl("txtSAPInitialSetup") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSAPInitialSetup")).Text, out convertedDouble))
                {
                    item.SAPInitialSetup = convertedDouble;
                }
                else
                {
                    item.SAPInitialSetup = 0;
                }
                if (projectDashboard.FindControl("txtQA") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtQA")).Text, out convertedDouble))
                {
                    item.QA = convertedDouble;
                }
                else
                {
                    item.QA = 0;
                }
                if (projectDashboard.FindControl("txtOBMReview1") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtOBMReview1")).Text, out convertedDouble))
                {
                    item.OBMReview1 = convertedDouble;
                }
                else
                {
                    item.OBMReview1 = 0;
                }
                if (projectDashboard.FindControl("txtBOMSetupPE") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtBOMSetupPE")).Text, out convertedDouble))
                {
                    item.BOMSetupPE = convertedDouble;
                }
                else
                {
                    item.BOMSetupPE = 0;
                }
                if (projectDashboard.FindControl("txtBOMSetupProc") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtBOMSetupProc")).Text, out convertedDouble))
                {
                    item.BOMSetupProc = convertedDouble;
                }
                else
                {
                    item.BOMSetupProc = 0;
                }
                if (projectDashboard.FindControl("txtBOMSetupPE2") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtBOMSetupPE2")).Text, out convertedDouble))
                {
                    item.BOMSetupPE2 = convertedDouble;
                }
                else
                {
                    item.BOMSetupPE2 = 0;
                }
                if (projectDashboard.FindControl("txtBOMSetupPE3") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtBOMSetupPE3")).Text, out convertedDouble))
                {
                    item.BOMSetupPE3 = convertedDouble;
                }
                else
                {
                    item.BOMSetupPE3 = 0;
                }
                if (projectDashboard.FindControl("txtOBMReview2") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtOBMReview2")).Text, out convertedDouble))
                {
                    item.OBMReview2 = convertedDouble;
                }
                else
                {
                    item.OBMReview2 = 0;
                }
                if (projectDashboard.FindControl("txtGRAPHICS") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtGRAPHICS")).Text, out convertedDouble))
                {
                    item.GRAPHICS = convertedDouble;
                }
                else
                {
                    item.GRAPHICS = 0;
                }
                if (projectDashboard.FindControl("txtCostingQuote") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtCostingQuote")).Text, out convertedDouble))
                {
                    item.CostingQuote = convertedDouble;
                }
                else
                {
                    item.CostingQuote = 0;
                }
                if (projectDashboard.FindControl("txtFGPackSpec") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtFGPackSpec")).Text, out convertedDouble))
                {
                    item.FGPackSpec = convertedDouble;
                }
                else
                {
                    item.FGPackSpec = 0;
                }
                if (projectDashboard.FindControl("txtSAPBOMSetup") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSAPBOMSetup")).Text, out convertedDouble))
                {
                    item.SAPBOMSetup = convertedDouble;
                }
                else
                {
                    item.SAPBOMSetup = 0;
                }
                if (projectDashboard.FindControl("txtExternalMfg") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtExternalMfg")).Text, out convertedDouble))
                {
                    item.ExternalMfg = convertedDouble;
                }
                else
                {
                    item.ExternalMfg = 0;
                }
                if (projectDashboard.FindControl("txtSAPRoutingSetup") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSAPRoutingSetup")).Text, out convertedDouble))
                {
                    item.SAPRoutingSetup = convertedDouble;
                }
                else
                {
                    item.SAPRoutingSetup = 0;
                }
                if (projectDashboard.FindControl("txtBOMActiveDate") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtBOMActiveDate")).Text, out convertedDouble))
                {
                    item.BOMActiveDate = convertedDouble;
                }
                else
                {
                    item.BOMActiveDate = 0;
                }
                if (projectDashboard.FindControl("txtSAPCostingDetails") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSAPCostingDetails")).Text, out convertedDouble))
                {
                    item.SAPCostingDetails = convertedDouble;
                }
                else
                {
                    item.SAPCostingDetails = 0;
                }
                if (projectDashboard.FindControl("txtSAPWarehouseInfo") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSAPWarehouseInfo")).Text, out convertedDouble))
                {
                    item.SAPWarehouseInfo = convertedDouble;
                }
                else
                {
                    item.SAPWarehouseInfo = 0;
                }
                if (projectDashboard.FindControl("txtStandardCostEntry") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtStandardCostEntry")).Text, out convertedDouble))
                {
                    item.StandardCostEntry = convertedDouble;
                }
                else
                {
                    item.StandardCostEntry = 0;
                }
                if (projectDashboard.FindControl("txtCostFinishedGood") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtCostFinishedGood")).Text, out convertedDouble))
                {
                    item.CostFinishedGood = convertedDouble;
                }
                else
                {
                    item.CostFinishedGood = 0;
                }
                if (projectDashboard.FindControl("txtFinalCostingReview") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtFinalCostingReview")).Text, out convertedDouble))
                {
                    item.FinalCostingReview = convertedDouble;
                }
                else
                {
                    item.FinalCostingReview = 0;
                }
                if (projectDashboard.FindControl("txtPurchasedPO") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtPurchasedPO")).Text, out convertedDouble))
                {
                    item.PurchasedPO = convertedDouble;
                }
                else
                {
                    item.PurchasedPO = 0;
                }
                if (projectDashboard.FindControl("txtRemoveSAPBlocks") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtRemoveSAPBlocks")).Text, out convertedDouble))
                {
                    item.RemoveSAPBlocks = convertedDouble;
                }
                else
                {
                    item.RemoveSAPBlocks = 0;
                }
                if (projectDashboard.FindControl("txtCustomerPO") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtCustomerPO")).Text, out convertedDouble))
                {
                    item.CustomerPO = convertedDouble;
                }
                else
                {
                    item.CustomerPO = 0;
                }
                if (projectDashboard.FindControl("txtMaterialsRcvdChk") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtMaterialsRcvdChk")).Text, out convertedDouble))
                {
                    item.MaterialsRcvdChk = convertedDouble;
                }
                else
                {
                    item.MaterialsRcvdChk = 0;
                }
                if (projectDashboard.FindControl("txtFirstProductionChk") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtFirstProductionChk")).Text, out convertedDouble))
                {
                    item.FirstProductionChk = convertedDouble;
                }
                else
                {
                    item.FirstProductionChk = 0;
                }
                if (projectDashboard.FindControl("txtDistributionChk") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtDistributionChk")).Text, out convertedDouble))
                {
                    item.DistributionChk = convertedDouble;
                }
                else
                {
                    item.DistributionChk = 0;
                }
                if (projectDashboard.FindControl("txtFCST") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtFCST")).Text, out convertedDouble))
                {
                    item.FCST = convertedDouble;
                }
                else
                {
                    item.FCST = 0;
                }
                if (projectDashboard.FindControl("txtMatrlWHSetUp") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtMatrlWHSetUp")).Text, out convertedDouble))
                {
                    item.MaterialWarehouseSetUp = convertedDouble;
                }
                else
                {
                    item.MaterialWarehouseSetUp = 0;
                }
                if (projectDashboard.FindControl("txtSAPCompleteItem") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtSAPCompleteItem")).Text, out convertedDouble))
                {
                    item.SAPCompleteItemSetup = convertedDouble;
                }
                else
                {
                    item.SAPCompleteItemSetup = 0;
                }
                if (projectDashboard.FindControl("txtBEQRC") != null && double.TryParse(((TextBox)projectDashboard.FindControl("txtBEQRC")).Text, out convertedDouble))
                {
                    item.BEQRC = convertedDouble;
                }
                else
                {
                    item.BEQRC = 0;
                }

            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Project Status Update Form: Project No" + ": " + ProjectNumber + "; error: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "Project Status Update Form", "Constructformdata");

                ErrorSummary.AddError("Unexpected Error Occurred: ConstructFormData", this.Page);
                return null;
            }


            return item;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm() == "")
            {
                ProjectTimelineItem item = ConstructFormData();
                try
                {
                    if (existingEntry(item.CompassListItemId))
                    {
                        timelineUpdater.UpdateProjectTimelineItem(item, ProjectNumber);
                    }
                    else
                    {
                        timelineUpdater.InsertProjectTimelineItem(item, ProjectNumber);
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Project Status Update Form: Project No" + ": " + ProjectNumber + "; error: " + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, "Project Status Update Form", "UdateService");
                }
                Page.Response.Redirect("/Pages/ProjectStatus.aspx?ProjectNo=" + ProjectNumber + "&updatepage=false", true);
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
        private bool CheckUserPrivileges(int CompassListItemId)
        {
            string userId = SPContext.Current.Web.CurrentUser.ID.ToString();
            var userfilter = "";
            bool showButtons = false;

            userfilter = "<Where>" +
                            "<And>" +
                                "<Eq><FieldRef Name=\"ID\" LookupId=\"TRUE\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq>" +
                                "<Eq><FieldRef Name=\"Initiator\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                            "</And>" +
                        "</Where>";
            var Initiator = dashboardService.getRequestedProjectDetails(userfilter);

            userfilter = "<Where>" +
                           "<And>" +
                               "<Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq>" +
                               "<Eq><FieldRef Name=\"ProjectLeader\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                           "</And>" +
                       "</Where>";
            var ProjectLeader = dashboardService.getChildProjectsByProjectTeam(userfilter);

            if (Initiator.Count > 0 || ProjectLeader.Count > 0 ||
                Utilities.CheckIfCurrentUserInGroup(
                    GlobalConstants.GROUP_ProjectManagers + "," +
                    GlobalConstants.GROUP_SeniorProjectManager + "," +
                    GlobalConstants.GROUP_OBMAdmins))
            {
                showButtons = true;
            }

            return showButtons;
        }
    }
}
