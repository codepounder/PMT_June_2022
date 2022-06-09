using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Ferrara.Compass.ProjectTimelineWorkflowTask.Classes;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.ProjectTimelineWorkflowTask
{
    public class ProjectTimelineTask : SPJobDefinition
    {

        #region Member Variables
        private IInitialApprovalReviewService initialApprovalService;// = DependencyResolution.DependencyMapper.Container.Resolve<IInitialApprovalReviewService>();
        private IProjectTimelineUpdateService projectTimelineUpdateService;// = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineUpdateService>();
        private IExceptionService exceptionService;// = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
        private IProjectTimelineTypeService timelineService;// = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
        #endregion
        #region Constructors
        public ProjectTimelineTask() : base() { }
        public ProjectTimelineTask(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public ProjectTimelineTask(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        #endregion

        public override void Execute(Guid targetInstanceId)
        {

          timelineService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineTypeService>();
         exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
       projectTimelineUpdateService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectTimelineUpdateService>();
        initialApprovalService = DependencyResolution.DependencyMapper.Container.Resolve<IInitialApprovalReviewService>();
               UpdateTimelineReportFields();
            
        }

        #region SAP Status List Methods
        private void UpdateTimelineReportFields()
        {
            List<DashboardDetailsItem> activeProjectDets = new List<DashboardDetailsItem>();
            SPWebApplication webApp = this.Parent as SPWebApplication;
            SPWeb spWeb = webApp.Sites[0].RootWeb;
            /*using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {*/

                    SPList projects = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Completed</Value></Neq><Neq><FieldRef Name=\"" + CompassListFields.WorkflowPhase + "\" /><Value Type=\"Text\">Cancelled</Value></Neq></And></Where>";
                    SPListItemCollection activeProjects = projects.GetItems(spQuery);

                    // Loop thru all In Progress Workflow Tasks
                    
                    foreach (SPListItem item in activeProjects)
                    {
                        try
                        {
                            DashboardDetailsItem detailsItem = new DashboardDetailsItem();
                            detailsItem.FirstProductionDate = Convert.ToString(item[CompassListFields.FirstProductionDate]);
                            detailsItem.FirstShipDate = Convert.ToString(item[CompassListFields.RevisedFirstShipDate]);
                            string timelineType = Convert.ToString(item[CompassListFields.TimelineType]);
                            if (timelineType == "")
                            {
                                detailsItem.TimelineType = "Standard";
                            }
                            else
                            {
                                detailsItem.TimelineType = Convert.ToString(item[CompassListFields.TimelineType]);
                            }

                            detailsItem.WorkflowPhase = Convert.ToString(item[CompassListFields.WorkflowPhase]);
                            detailsItem.ProjectType = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                            detailsItem.CompassProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                            detailsItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                            var itemDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                            detailsItem.ProjectName = Convert.ToString(item[CompassListFields.ProjectNumber]) + ": " + Convert.ToString(item[CompassListFields.SAPItemNumber]) + " " + itemDescription;
                            detailsItem.ParentProjectNumber = Convert.ToString(item[CompassListFields.ParentProjectNumber]);
                            detailsItem.StageGateProjectListItemId = Convert.ToString(item[CompassListFields.StageGateProjectListItemId]);
                            detailsItem.CompassListItemId = item.ID;
                            detailsItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);

                            activeProjectDets.Add(detailsItem);
                        }
                        catch (Exception ex)
                        {
                            InsertLog("Project Timeline Job-Error", "UpdateTimelineReportFields", string.Concat("Test0): ", ex.Message));
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Project Timeline Job-Error-UpdateTimelineReportFields: ", ex.Message));
                        }
                    //}
                //}
            }
            InsertLog("Project Timeline Job-Error", "UpdateTimelineReportFields", string.Concat("activeProjectDets Count", activeProjectDets.Count));

            foreach (DashboardDetailsItem item in activeProjectDets) {
                try
                {
                    int isExisting = projectTimelineUpdateService.GetProjectTimelineItem(item.CompassListItemId);
                    if (isExisting > 0)
                    {
                        item.ExistingItem = true;
                    }
                    else
                    {
                        item.ExistingItem = false;
                    }
                }
                catch (Exception ex)
                {
                    InsertLog("Project Timeline Job-Error", "UpdateTimelineReportFields", string.Concat("Test1", item.CompassListItemId, ex.Message));
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Project Timeline Job-Error-UpdateTimelineReportFields Test1: ", ex.Message));
                }
            }
                foreach (DashboardDetailsItem item in activeProjectDets)
                {

                try
                {
                    InsertLog("1", "UpdateTimelineReportFields", string.Concat("1", item.CompassListItemId));
                    if (timelineService != null)
                    {
                        var projectStatusReportDetails = timelineService.actualTimeLine(item, false);
                        InsertLog("2", "UpdateTimelineReportFields", string.Concat("2", item.CompassListItemId));
                        List<ProjectStatusReportItem> ActualReportItem = projectStatusReportDetails.Item1;
                        InsertLog("3", "UpdateTimelineReportFields", string.Concat("3", item.CompassListItemId));
                        List<ProjectStatusReportItem> OriginalReportItem = projectStatusReportDetails.Item2;
                        InsertLog("4", "UpdateTimelineReportFields", string.Concat("4", item.CompassListItemId));
                        initialApprovalService.insertOriginalTimeline(item.CompassListItemId, item.ProjectNumber, OriginalReportItem);
                        InsertLog("5", "UpdateTimelineReportFields", string.Concat("5", item.CompassListItemId));
                        insertActualTimeline(item, ActualReportItem);
                        InsertLog("6", "UpdateTimelineReportFields", string.Concat("6", item.CompassListItemId));
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    InsertLog("Project Timeline Job-Error", "UpdateTimelineReportFields", string.Concat("Test2", item.CompassListItemId, ex.Message));
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Project Timeline Job-Error-UpdateTimelineReportFields Test2: ", ex.Message));
                }
            }
            
        }
        private void InsertLog(string message, string method, string additionalInfo)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            SPWeb spWeb = webApp.Sites[0].RootWeb;
            /*using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {*/
                    var list = spWeb.Lists.TryGetList(GlobalConstants.LIST_LogsListName);
                    if (list != null)
                    {
                spWeb.AllowUnsafeUpdates = true;
                var item = list.Items.Add();
                        item["Category"] = "CriticalError";
                        item["Message"] = message;
                        item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                        item["Form"] = "SAP Workflow Task Timer Job";
                        item["Method"] = method;
                        item["AdditionalInfo"] = additionalInfo;
                        item["CreatedDate"] = DateTime.Now;
                        item.SystemUpdate(false);
                spWeb.AllowUnsafeUpdates = false;
            }
                //}
            //}
        }
        public void insertActualTimeline(DashboardDetailsItem dbItem, List<ProjectStatusReportItem> actualTimeline)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            SPWeb spWeb = webApp.Sites[0].RootWeb;
            /*using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate ()
            {*/
            spWeb.AllowUnsafeUpdates = true;
                SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineDetailsList);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + dbItem.CompassListItemId + "</Value></Eq><Eq><FieldRef Name=\"" + ProjectStatusDatesFields.RowType + "\" /><Value Type=\"Text\">Actuak</Value></Eq></And></Where>";
                spQuery.RowLimit = 1;

                SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                SPListItem appItem;
                if (compassItemCol.Count > 0)
                {
                    appItem = compassItemCol[0];
                }
                else
                {
                    appItem = spList.AddItem();
                    appItem[ProjectStatusDatesFields.compassListItemId] = dbItem.CompassListItemId;
                    appItem[ProjectStatusDatesFields.RowType] = "Actual";
                    appItem["Title"] = dbItem.ProjectNumber;
                }
                if (appItem != null)
                {
                    foreach (ProjectStatusReportItem item in actualTimeline)
                    {
                        if (string.IsNullOrEmpty(item.WorflowQuickStep) || item.WorflowQuickStep == "Float" || item.Checks == "DF" || item.WorflowQuickStep == "IPF")
                        {
                            continue;
                        }
                        string columnStart = item.WorflowQuickStep + "_Start";
                        string columnEnd = item.WorflowQuickStep + "_End";
                        string columnDuration = item.WorflowQuickStep + "_Duration";
                        try
                        {

                            appItem[columnStart] = item.ActualStartDay.ToString();
                            appItem[columnEnd] = item.ActualEndDay.ToString();
                            appItem[columnDuration] = item.ActualDuration.ToString();
                        }
                        catch (Exception e)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, e, "InitialApprovalService", "insertOriginalTimeline", columnStart);
                        }
                    }
                    appItem.Update();
                    
                }
            spWeb.AllowUnsafeUpdates = false;
            /*});
                }
            }*/
        }
    #endregion
    }
}

