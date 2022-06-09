using System;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Microsoft.SharePoint;
using System.Text;
using System.Linq;
using Ferrara.Compass.Abstractions.Enum;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web;

namespace Ferrara.Compass.WebParts.MyWorkflowTasksForm2
{
    [ToolboxItemAttribute(false)]
    public partial class MyWorkflowTasksForm2 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public MyWorkflowTasksForm2()
        {
        }
        #region Member Variables

        private IDashboardService dashboardService;
        private IProjectTimelineTypeService timelineService;
        private IProjectTimelineUpdateService timelineUpdateService;
        private IExceptionService exceptionService;
        private string userId;
        private string ImpersonationId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ImpersonationId] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ImpersonationId];
                return string.Empty;
            }
        }
        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            timelineService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
            timelineUpdateService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineUpdateService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            divImpoersonation.Visible = false;
            if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_Developers))
            {
                divImpoersonation.Visible = true;
            }
            this.hidTableDisplayType.Value = "false";
            bool isProcurementMember = false;
            if (string.IsNullOrEmpty(ImpersonationId))
            {
                userId = SPContext.Current.Web.CurrentUser.ID.ToString();

                if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_ProcurementPackaging))
                {
                    this.hdnProcurement.Value = "true";
                    isProcurementMember = true;
                }
            }
            else
            {
                userId = ImpersonationId;
                string userName = "";

                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPUser spUser = new SPFieldUserValue(spWeb, Convert.ToInt32(ImpersonationId), null).User;
                        lblImpersonation.Visible = true;
                        lblImpersonation.Text = "Currently the Dashboard is showing tasks and projects of " + spUser.Name + ". Click Remove to go back to the normal view.";
                        if (spUser != null)
                        {
                            userName = spUser.LoginName;
                            peImpersonate.CommaSeparatedAccounts = userName;
                            if (Utilities.CheckIfUserInGroup(GlobalConstants.GROUP_ProcurementPackaging, userName))
                            {
                                this.hdnProcurement.Value = "true";
                                isProcurementMember = true;
                            }
                        }
                    }
                }
            }


            List<WorkflowTaskDetailsItem> lst = new List<WorkflowTaskDetailsItem>();
            List<ProjectDetailsItem> lstDets = new List<ProjectDetailsItem>();
            List<WorkflowTaskDetailsItem> lstWSTasks = new List<WorkflowTaskDetailsItem>();
            List<WorldSyncRequestItem> lstWSDets = new List<WorldSyncRequestItem>();
            List<TimelineTypeItem> timelineNumbers = new List<TimelineTypeItem>();
            List<int> projectNumbers = new List<int>();

            lst = dashboardService.getWorkflowTaskItems(userId);
            foreach (WorkflowTaskDetailsItem task in lst)
            {
                projectNumbers.Add(Utilities.GetItemIdFromProjectNumber(task.ProjectNumber));
            }

            try
            {
                lstWSTasks = dashboardService.getWorldSyncWorkflowTaskItems(userId);
            }
            catch (Exception error)
            {
                ErrorSummary.AddError(error.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Workflow Tasks" + ": " + error.Message);
                exceptionService.Handle(LogCategory.CriticalError, error, "Workflow Tasks", "Page_Load");
            }

            foreach (WorkflowTaskDetailsItem task in lstWSTasks)
            {
                try
                {
                    int uniqueProjectCount = 0;
                    if (lstWSDets.Count > 0)
                    {
                        uniqueProjectCount = (from dets in lstWSDets where dets.RequestId == task.CompassId select dets).Count();
                    }
                    if (uniqueProjectCount <= 0)
                    {
                        lstWSDets.Add(dashboardService.getWorldSyncRequestItem(task.CompassId));
                    }
                }
                catch (Exception ex)
                {

                    ErrorSummary.AddError(ex.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Woorldsync Tasks" + ": task.CompassId : " + task.CompassId + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, "Woorldsync Tasks", "Page_Load");
                }
            }

            List<int> newProjectNumbers = projectNumbers.Distinct().ToList();
            foreach (int compassId in newProjectNumbers)
            {
                lstDets.Add(dashboardService.getProjectDetails(compassId));
            }

            foreach (WorkflowTaskDetailsItem task in lstWSTasks)
            {
                try
                {
                    WorldSyncRequestItem projectInfo = (from dets in lstWSDets where dets.RequestId == task.CompassId select dets).FirstOrDefault();
                    if (projectInfo.WorkflowStep == "RequestFile")
                    {
                        switch (projectInfo.RequestType)
                        {
                            case "RequestNutritional": task.Title = "Upload Nutritional"; task.FormUrl = "WorldSyncRequestUpload.aspx?RequestId=" + task.CompassId; break;
                            case "RequestImage": task.Title = "Upload Image"; task.FormUrl = "WorldSyncRequestUpload.aspx?RequestId=" + task.CompassId; break;
                            default: task.Title = ""; break;
                        }
                    }
                    else if (projectInfo.WorkflowStep == "ReceiveFile")
                    {
                        task.Title = "Confirm Receipt";
                        task.FormUrl = "WorldSyncRequestReceipt.aspx?RequestId=" + task.CompassId;
                    }
                    else
                    {
                        task.Title = "";
                    }
                    task.CompassId = -1;
                }
                catch (Exception ex)
                {
                    ErrorSummary.AddError(ex.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Woorldsync Tasks" + ": task.CompassId : " + task.CompassId + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, "Woorldsync Tasks", "Page_Load");
                }
            }

            foreach (WorkflowTaskDetailsItem task in lst)
            {
                try
                {
                    ProjectDetailsItem projectInfo = (from dets in lstDets where dets.ProjectNumber == task.ProjectNumber select dets).FirstOrDefault();
                    task.FirstProductionDate = projectInfo.FirstProduction;
                    task.Title = projectInfo.ProjectTitle;
                    task.FirstShipDate = projectInfo.RevisedFirstShipDate;
                    task.CompassId = projectInfo.CompassItemId;
                    task.ProjectType = projectInfo.ProjectType;
                    task.ProjectTypeSubcategory = projectInfo.ProjectTypeSubCategory;
                    task.MaterialGroup1Brand = projectInfo.MaterialGroup1Brand;
                    task.Initiator = projectInfo.Initiator;
                    task.InitiatorName = projectInfo.InitiatorName;
                    task.ProductHierarchyLevel1 = projectInfo.ProductHierarchyLevel1;
                    int updatedDay = timelineUpdateService.GetSingleProjectItem(projectInfo.CompassItemId, task.WorkflowStep);
                    string timelineType = string.IsNullOrEmpty(projectInfo.TimelineType) ? "Standard" : projectInfo.TimelineType;
                    if (updatedDay <= 0)
                    {
                        task.DueDateCalc = timelineService.GetSingleWorkflowStepItem(timelineType, task.WorkflowStep);
                    }
                    else
                    {
                        task.DueDateCalc = updatedDay;
                    }
                }
                catch (Exception ex)
                {
                    ErrorSummary.AddError(ex.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Workflow  details Tasks" + ": task.ProjectNumber : " + task.ProjectNumber + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, "Workflow details tasks", "Page_Load");
                }
            }


            if (isProcurementMember && newProjectNumbers.Count > 0)
            {
                try
                {
                    List<KeyValuePair<int, Dictionary<string, string>>> packagingNumberList = dashboardService.GetNewPackagingNumbersForProject(newProjectNumbers);

                    foreach (WorkflowTaskDetailsItem task in lst)
                    {
                        try
                        {
                            List<KeyValuePair<string, string>> packagingNumbers = packagingNumberList.Where(r => r.Key == task.CompassId).SelectMany(r => r.Value).ToList();
                            string condensed = "";
                            foreach (KeyValuePair<string, string> dets in packagingNumbers)
                            {
                                condensed += dets.Key + ": " + dets.Value + "; ";
                            }
                            task.NewPackagingComponents = condensed;
                        }
                        catch (Exception ex)
                        {
                            ErrorSummary.AddError(ex.Message, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Workflow  Tasks GetNewPackagingNumbersForProject : task.CompassId : " + task.CompassId + ex.Message);
                            exceptionService.Handle(LogCategory.CriticalError, ex, "Workflow details tasks", "Line 234");
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorSummary.AddError(ex.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Workflow  Tasks GetNewPackagingNumbersForProject :" + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, "Workflow details tasks", "Line 229");
                }
            }

            lst.AddRange(lstWSTasks);

            rptAllCurrentTasks.DataSource = lst;
            rptAllCurrentTasks.DataBind();
        }
        protected void rptAllCurrentTasks_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                WorkflowTaskDetailsItem item = (WorkflowTaskDetailsItem)e.Item.DataItem;

                HtmlGenericControl lblTaskStatus = (HtmlGenericControl)e.Item.FindControl("lblTaskStatus");
                HtmlTableRow taskRow = (HtmlTableRow)e.Item.FindControl("taskRow");
                HtmlTableCell lblProjectName = (HtmlTableCell)e.Item.FindControl("lblProjectName");
                HtmlTableCell lblTask = (HtmlTableCell)e.Item.FindControl("lblTask");
                HtmlTableCell lblRequestedDate = (HtmlTableCell)e.Item.FindControl("lblRequestedDate");
                HtmlTableCell lblDueDate = (HtmlTableCell)e.Item.FindControl("lblDueDate");
                HtmlTableCell lblFirstProdDate = (HtmlTableCell)e.Item.FindControl("lblFirstProdDate");
                HtmlTableCell lblFirstShipDate = (HtmlTableCell)e.Item.FindControl("lblFirstShipDate");
                HtmlTableCell lblProjectType = (HtmlTableCell)e.Item.FindControl("lblProjectType");
                HtmlTableCell lblProjectTypeSubCat = (HtmlTableCell)e.Item.FindControl("lblProjectTypeSubCat");
                HtmlTableCell lblMaterialGroup1Brand = (HtmlTableCell)e.Item.FindControl("lblMaterialGroup1Brand");
                HtmlTableCell lblInitiator = (HtmlTableCell)e.Item.FindControl("lblInitiator");
                HtmlTableCell lblPackagingNumbers = (HtmlTableCell)e.Item.FindControl("lblPackagingNumbers");
                HtmlTableCell lblCommLink = (HtmlTableCell)e.Item.FindControl("lblCommLink");
                HtmlTableCell lblProdHierarchyLvl1 = (HtmlTableCell)e.Item.FindControl("lblProdHierarchyLvl1");

                DateTime requestedDate = Convert.ToDateTime(item.RequestedDate);
                if(requestedDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    requestedDate = requestedDate.AddDays(2);
                }
                else if (requestedDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    requestedDate = requestedDate.AddDays(1);
                }
                TimeSpan startTime = requestedDate.TimeOfDay;
                if (startTime > TimeSpan.Parse("16:00:00"))
                {
                    TimeSpan ts = new TimeSpan(8, 0, 0);
                    requestedDate = requestedDate.Date + ts;
                    requestedDate = requestedDate.AddDays(1);
                }
                DateTime dueDate = requestedDate.AddDays(item.DueDateCalc);
                double weekendNum = weekends(requestedDate, dueDate);
                dueDate = dueDate.AddDays(weekendNum);
                TimeSpan taskSpan = dueDate - requestedDate;
                double taskMinutes = taskSpan.TotalMinutes;
                TimeSpan currentSpan = DateTime.Now - requestedDate;
                double currentMinutes = currentSpan.TotalMinutes;
                double currentPercent = currentMinutes / taskMinutes;
                if (currentPercent <= .75)
                {
                    lblTaskStatus.Attributes.Add("Title", "C");
                    taskRow.Attributes.Add("class", "StatusGreen");
                }
                else if (currentPercent > .75 && currentPercent <= 1)
                {
                    lblTaskStatus.Attributes.Add("Title", "B");
                    taskRow.Attributes.Add("class", "StatusYellow");
                }
                else if (currentPercent > 1)
                {
                    lblTaskStatus.Attributes.Add("Title", "A");
                    taskRow.Attributes.Add("class", "StatusRed");
                }
                lblProjectName.InnerText = item.Title;

                HyperLink task = new HyperLink();
                task.NavigateUrl = item.FormUrl;
                task.Text = item.TaskName;

                lblTask.Controls.Add(task);

                lblRequestedDate.InnerText = requestedDate.ToString("MM/dd/yyyy");
                lblDueDate.InnerText = requestedDate.AddDays(item.DueDateCalc + weekendNum).ToString("MM/dd/yyyy");
                lblFirstProdDate.InnerText = string.IsNullOrEmpty(item.FirstProductionDate) ? "" : Convert.ToDateTime(item.FirstProductionDate).ToString("MM/dd/yyyy");
                lblFirstShipDate.InnerText = string.IsNullOrEmpty(item.FirstShipDate) ? "" : Convert.ToDateTime(item.FirstShipDate).ToString("MM/dd/yyyy");
                lblProjectType.InnerText = item.ProjectType;
                lblProjectTypeSubCat.InnerText = item.ProjectTypeSubcategory;
                lblMaterialGroup1Brand.InnerText = item.MaterialGroup1Brand;
                lblInitiator.InnerText = Utilities.GetPersonFieldForDisplay(item.InitiatorName);
                lblPackagingNumbers.InnerText = item.NewPackagingComponents;

                HyperLink commLink = new HyperLink();
                commLink.NavigateUrl = "/Pages/CommercializationItem.aspx?ProjectNo=" + item.ProjectNumber;
                commLink.Text = item.ProjectNumber;
                lblCommLink.Controls.Add(commLink);

                lblProdHierarchyLvl1.InnerText = item.ProductHierarchyLevel1;
            }
        }
        public static double weekends(DateTime start, DateTime end)
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
                int excDayTest = exclDays(start, tempEnd);
                if (exclusionDays == (excDayTest))
                {
                    break;
                }
                else
                {
                    exclusionDays = excDayTest;
                    if (exclWEOld == 0 && exclHDOld == 0)
                    {
                        tempEnd = tempEnd.AddDays((excDayTest));
                    }
                    else
                    {
                        tempEnd = tempEnd.AddDays((excDayTest - exclWEOld));
                    }
                    exclWEOld = excDayTest;

                }
                days = excDayTest;
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

        protected void btnImpersonate_Click(object sender, EventArgs e)
        {
            if (peImpersonate.Entities.Count > 0)
            {
                SPFieldUserValueCollection Users = Utilities.GetPeopleFromPickerControl(peImpersonate, SPContext.Current.Web);
                int userId = Users[0].LookupId;

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_ImpersonationId, userId.ToString())
                };
                lblImpersonation.Visible = true;
                lblImpersonation.Text = "Impersonating " + Utilities.GetPersonFieldForDisplay(Users[0].User.LoginName);
                Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_TaskDashboard, parameters), false);
            }
        }
        protected void btnRemove_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_TaskDashboard), true);
        }

    }
}
