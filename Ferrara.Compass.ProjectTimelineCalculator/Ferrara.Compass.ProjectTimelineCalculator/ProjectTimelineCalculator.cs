using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint;
using Ferrara.Compass.ProjectTimelineCalculator.Classes;
using Ferrara.Compass.ProjectTimelineCalculator.Models;
using Ferrara.Compass.ProjectTimelineCalculator.Constants;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Globalization;

namespace Ferrara.Compass.ProjectTimelineCalculator
{

    public class ProjectTimelineCalculator : SPJobDefinition
    {

        #region Constants
        SPWeb spWeb = null;
        //FROM GLOBAL CONTANTS
        public const string LIST_CompassListName = "Compass List";
        public const string LIST_ApprovalListName = "Compass Approval List";
        public const string LIST_LogsListName = "Logs List";
        public const string LIST_WorkflowTaskListName3 = "Compass Workflow Tasks 3";
        public const string LIST_PackagingItemListName = "Compass Packaging Item List";
        public const string LIST_SAPStatusListName = "SAP Status List";
        public const string LIST_ProjectTimelineDetailsList = "Compass Timeline Details List";
        public const string LIST_ProjectTimelineUpdateName = "Update Project Timeline";
        public const string LIST_ProjectTimelineTypeListName = "Project Timeline Type Days List";
        public const string LIST_HolidayLookup = "Holiday Lookup";
        public const string LIST_TimelineTypesLookup = "Timeline Types Lookup";
        public const string LIST_ApprovalList2Name = "Compass Approval List 2";
        public const string LIST_SAPApprovalListName = "Compass SAP Approval List";
        public const string LIST_ProjectDecisionsListName = "Compass Project Decisions List";
        public const string LIST_CompassTaskAssignmentListName = "Compass Task Assignment";
        public const string LIST_DragonflyStatusListName = "Dragonfly Status List";
        public const string LIST_ComponentCostingListName = "Component Costing List";
        public const string LIST_MarketingClaimsListName = "Marketing Claims List";

        public const string PRODUCT_HIERARCHY1_Bulk = "Bulk (000000024)";
        public const string PRODUCT_HIERARCHY1_CoMan = "Co-Manufacturing (000000027)";
        public const string PRODUCT_HIERARCHY1_Everyday = "Everyday (000000025)";
        public const string PRODUCT_HIERARCHY1_PrivateLabel = "Private Label (000000026)";
        public const string PRODUCT_HIERARCHY1_Seasonal = "Seasonal (000000023)";
        public const string PROJECTTYPE_Innovation = "Innovation";
        public const string PROJECTTYPE_LineExtension = "Line Extenstion";
        public const string PROJECTTYPE_DownweightTransition = "Downweight/Transitions";
        public const string PROJECTTYPE_GraphicsChangesInternalAdjustments = "Graphics Changes/Internal Adjustments";
        public const string PROJECTTYPE_GraphicsChangeOnly = "Graphics Change Only";
        public const string PROJECTTYPE_SimpleNetworkMove = "Simple Network Move";
        public const string QUERYSTRING_SAPTask = "SAPTask";
        public const string PROJECTTYPESUBCATEGORY_ComplexNetworkMove = "Complex Network Move";
        public const string COMPONENTTYPE_PurchasedSemi = "Purchased Candy Semi";
        public const string COMPONENTTYPE_TransferSemi = "Transfer Semi";
        public const string COMPONENTTYPE_CandySemi = "Candy Semi";
        public const string WORKFLOWPHASE_Completed = "Completed";
        public const string WORKFLOWPHASE_Cancelled = "Cancelled";
        public const string SrOBMApproval_Decision = "SrOBMApproval_Decision";
        public const string SrOBMApproval2_Decision = "SrOBMApproval2_Decision";
        #endregion
        #region Constructors
        public ProjectTimelineCalculator() : base() { }
        public ProjectTimelineCalculator(string jobName, SPService service)
            : base(jobName, service, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        public ProjectTimelineCalculator(string jobName, SPWebApplication webapp)
            : base(jobName, webapp, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }
        #endregion
        public static CultureInfo ci = new CultureInfo("en-US");
        public override void Execute(Guid targetInstanceId)
        {
            SPWebApplication webApp = this.Parent as SPWebApplication;
            if (webApp.Name == "Compass")
            {
                spWeb = webApp.Sites[0].RootWeb;


                spWeb.AllowUnsafeUpdates = true;
                InsertLog("Project Timeline Job", "UpdateTimelineReportFields", "Project Timeline Job has Started - Ver 2.0");
                UpdateTimelineReportFields();
                InsertLog("Project Timeline Job", "UpdateTimelineReportFields", "Project Timeline Job has Completed - Ver 2.0");
                spWeb.AllowUnsafeUpdates = false;
            }

        }

        #region SAP Status List Methods
        public void UpdateTimelineReportFields()
        {
            List<DashboardDetailsItem> activeProjectDets = new List<DashboardDetailsItem>();
            //InsertLog(spWeb, "3", "UpdateTimelineReportFields", string.Concat("3: "));
            /* using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
              {
                  using (SPWeb web = spSite.OpenWeb())
                  {
              */
            SPList projects = spWeb.Lists.TryGetList(LIST_CompassListName);

            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Neq><FieldRef Name=\"" + CompassListFields.FinalUpdate + "\" /><Value Type=\"Text\">Yes</Value></Neq></Where>";
            SPListItemCollection activeProjects = projects.GetItems();
            //InsertLog(spWeb, "4", "UpdateTimelineReportFields", string.Concat("4: "));
            // Loop thru all In Progress Workflow Tasks
          
            
            foreach (SPListItem item in activeProjects)
            {
                try
                {
                    if (item != null)
                    {
                        //filter to test -- 2022-28-9  2018-133 2021-279-6
                        //string projectNubmer = Convert.ToString(item[CompassListFields.ProjectNumber]).ToString();
                        //if (projectNubmer.Trim() != "2021-99-1")
                        //{
                        //    continue;
                        //}
                        // if(item.ID != 6789)
                        //{
                        //    continue;
                        //}
                        DashboardDetailsItem detailsItem = new DashboardDetailsItem();
                        detailsItem.FirstProductionDate = Convert.ToString(item[CompassListFields.FirstProductionDate]);
                        detailsItem.FirstShipDate = Convert.ToString(item[CompassListFields.FirstShipDate]);
                        string timelineType = Convert.ToString(item[CompassListFields.TimelineType]);
                        if (timelineType == "")
                        {
                            detailsItem.TimelineType = "Standard";
                        }
                        else
                        {
                            detailsItem.TimelineType = Convert.ToString(item[CompassListFields.TimelineType]);
                        }
                        string workflowPhase = Convert.ToString(item[CompassListFields.WorkflowPhase]);
                        detailsItem.WorkflowPhase = workflowPhase;
                        detailsItem.ProjectType = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        detailsItem.CompassProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        detailsItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        var itemDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        detailsItem.ProjectName = Convert.ToString(item[CompassListFields.ProjectNumber]) + ": " + Convert.ToString(item[CompassListFields.SAPItemNumber]) + " " + itemDescription;
                        detailsItem.ParentProjectNumber = Convert.ToString(item[CompassListFields.ParentProjectNumber]);
                        detailsItem.StageGateProjectListItemId = Convert.ToString(item[CompassListFields.StageGateProjectListItemId]);
                        detailsItem.CompassListItemId = item.ID;
                        detailsItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                        detailsItem.RevisedFirstShipDate = Convert.ToString(item[CompassListFields.RevisedFirstShipDate]);
                        detailsItem.IPFSubmitted = Convert.ToString(item[CompassListFields.SubmittedDate]);

                        activeProjectDets.Add(detailsItem);
                        if (workflowPhase == WORKFLOWPHASE_Cancelled || workflowPhase == WORKFLOWPHASE_Completed)
                        {
                            item[CompassListFields.FinalUpdate] = "Yes";
                            item.Update();
                        }
                    }
                }
                catch (Exception ex)
                {
                    // added on 20-Jul-2022 by Mahipal Reddy
                    InsertLog("Project Timeline Job-Error Ver 2.0", "UpdateTimelineReportFields", string.Concat("Test0: ", Convert.ToString(item[CompassListFields.ProjectNumber]).ToString() + " - " + ex.Message));

                   // InsertLog("Project Timeline Job-Error", "UpdateTimelineReportFields", string.Concat("Test0): ", ex.Message));
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Project Timeline Job-Error-UpdateTimelineReportFields1: ", ex.Message));
                }
                //}
                //InsertLog(spWeb, "5", "UpdateTimelineReportFields", string.Concat("5: "));
                //}
            }
            InsertLog("Project Timeline Job", "UpdateTimelineReportFields", string.Concat("activeProjectDets Count: ", activeProjectDets.Count));

            foreach (DashboardDetailsItem item in activeProjectDets)
            {
                try
                {
                    DashboardDetailsItem itemnew = new DashboardDetailsItem();
                    itemnew.CompassListItemId = item.CompassListItemId;
                    int isExisting = GetProjectTimelineItem(itemnew.CompassListItemId);
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
                    InsertLog("Project Timeline Job-Error", "UpdateTimelineReportFields", string.Concat("Is Existing Error", item.CompassListItemId, ex.Message));
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Is Existing Error Test4: ", ex.Message));
                }
            }
            // InsertLog(spWeb, "8", "UpdateTimelineReportFields", string.Concat("8: "));
            foreach (DashboardDetailsItem item in activeProjectDets)
            {

                try
                {
                    //if (timelineService != null)
                    //{
                    DashboardDetailsItem newitem = new DashboardDetailsItem();
                    newitem = item;
                    var projectStatusReportDetails = actualTimeLine(newitem, false);
                    List<ProjectStatusReportItem> ActualReportItem = projectStatusReportDetails.Item1;
                    List<ProjectStatusReportItem> OriginalReportItem = projectStatusReportDetails.Item2;
                    insertOriginalTimeline(item, OriginalReportItem);
                    insertActualTimeline(item, ActualReportItem);
                    /*}
                    else
                    {
                        InsertLog("1", "UpdateTimelineReportFields", string.Concat("Timelineservice break", item.CompassListItemId));
                        break;
                    }*/
                }
                catch (Exception ex)
                {
                    InsertLog("Project Timeline Job-Error V 2.0", "UpdateTimelineReportFields", string.Concat("Test2 :  ", item.CompassListItemId, " - " , ex.Message));
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, string.Concat("Project Timeline Job-Error-UpdateTimelineReportFields Test2: ", ex.Message));
                    break;
                }
            }

        }
        public void insertOriginalTimeline(DashboardDetailsItem compassItem, List<ProjectStatusReportItem> originalTimeline)
        {
            /* SPSecurity.RunWithElevatedPrivileges(delegate ()
             {
                 using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                 {
                     using (SPWeb spWeb = spSite.OpenWeb())
                     {
                         spWeb.AllowUnsafeUpdates = true;*/
            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineDetailsList);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + ProjectStatusDatesFields.compassListItemId + "\" /><Value Type=\"Int\">" + compassItem.CompassListItemId + "</Value></Eq><Eq><FieldRef Name=\"" + ProjectStatusDatesFields.RowType + "\" /><Value Type=\"Text\">Original</Value></Eq></And></Where>";
            spQuery.RowLimit = 1;

            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
            if (compassItemCol.Count > 0)
            {
                return;
            }
            else
            {
                SPListItem appItem = spList.AddItem();
                appItem[ProjectStatusDatesFields.compassListItemId] = compassItem.CompassListItemId;
                appItem[ProjectStatusDatesFields.RowType] = "Original";
                appItem["Editor"] = spWeb.CurrentUser;
                appItem["Title"] = compassItem.ProjectNumber;

                foreach (ProjectStatusReportItem item in originalTimeline)
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

                        appItem[columnStart] = item.OGStartDay.ToString();
                        appItem[columnEnd] = item.OGEndDay.ToString();
                        appItem[columnDuration] = item.OGDuration.ToString();
                        appItem.Update();
                    }
                    catch (Exception e)
                    {
                        InsertLog("insertOriginalTimeline", "UpdateTimelineReportFields", "columnStart: " + columnStart);
                    }
                    try
                    {
                        appItem[ProjectStatusDatesFields.FirstProductionDate] = compassItem.FirstProductionDate;
                        appItem[ProjectStatusDatesFields.IPFSubmitted] = compassItem.IPFSubmitted;
                        appItem[ProjectStatusDatesFields.FirstShipDate] = compassItem.FirstShipDate;
                        appItem[ProjectStatusDatesFields.RevisedFirstShipDate] = compassItem.RevisedFirstShipDate;
                        appItem.Update();
                    }
                    catch (Exception ex)
                    {
                        InsertLog("insertOriginalTimeline Ver 2.0", "UpdateTimelineReportFields", "Test Child 1: " + ex.Message + " : Column " + columnStart);
                    }
                   

                }

            }
            /*spWeb.AllowUnsafeUpdates = false;
        }
    }
});*/
        }

        private void InsertLog(string message, string method, string additionalInfo)
        {
            /*SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;*/
            var list = spWeb.Lists.TryGetList(LIST_LogsListName);
            if (list != null)
            {

                var item = list.Items.Add();
                item["Category"] = "CriticalError";
                item["Message"] = message;
                item["Title"] = message.Length > 250 ? message.Substring(0, 250) : message;
                item["Form"] = "Project Timeline Calulator";
                item["Method"] = method;
                item["AdditionalInfo"] = additionalInfo;
                item["CreatedDate"] = DateTime.Now;
                item.SystemUpdate(false);

            }
            /* spWeb.AllowUnsafeUpdates = false;
             }
         }

     });*/
        }
        public void insertActualTimeline(DashboardDetailsItem dbItem, List<ProjectStatusReportItem> actualTimeline)
        {
            /*SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb web = spSite.OpenWeb())
                    {

                        web.AllowUnsafeUpdates = true;*/
            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineDetailsList);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + ProjectStatusDatesFields.compassListItemId + "\" /><Value Type=\"Int\">" + dbItem.CompassListItemId + "</Value></Eq><Eq><FieldRef Name=\"" + ProjectStatusDatesFields.RowType + "\" /><Value Type=\"Text\">Actual</Value></Eq></And></Where>";
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
                    string columnStatus = item.WorflowQuickStep + "_Status";
                    try
                    {

                        appItem[columnStart] = item.ActualStartDay.ToString();
                        appItem[columnEnd] = item.ActualEndDay.ToString();
                        appItem[columnDuration] = item.ActualDuration.ToString();
                        appItem[columnStatus] = item.Status.ToString();
                        appItem.Update();

                    }
                    catch (Exception e)
                    {
                        InsertLog("insertActualTimeline", "UpdateTimelineReportFields", "columnStart: " + columnStart);
                    }
                }
                try
                {
                    appItem[ProjectStatusDatesFields.FirstProductionDate] = dbItem.FirstProductionDate;
                    appItem[ProjectStatusDatesFields.IPFSubmitted] = dbItem.IPFSubmitted;
                    appItem[ProjectStatusDatesFields.FirstShipDate] = dbItem.FirstShipDate;
                    appItem[ProjectStatusDatesFields.RevisedFirstShipDate] = dbItem.RevisedFirstShipDate;
                    appItem.Update();
                }
                catch ( Exception ex)
                {
                    InsertLog("insertActualTimeline Ver 2.0", "UpdateTimelineReportFields", "Test Child 2: " + ex.Message);
                }
               

            }
            /*web.AllowUnsafeUpdates = false;

            }
             }
        });*/
        }
        public int GetProjectTimelineItem(int compassListId)
        {
            int projectCount = 0;
            /*using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {*/
            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineUpdateName);

            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
            spQuery.ViewFields = string.Concat(
                           "<FieldRef Name='" + ProjectTimelineUpdateFields.compassListItemId + "' />");
            projectCount = spList.GetItems(spQuery).Count;
            //}
            //}
            return projectCount;
        }
        public List<TimelineTypeItem> GetPhases()
        {
            var newItem = new List<TimelineTypeItem>();
            /*using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {*/
            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineTypeListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"WorkflowOrder\"></FieldRef><Value Type=\"Int\"></Value></Eq></Where><OrderBy><FieldRef Name=\"PhaseNumber\" Type='Int' /><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
            SPListItemCollection compassItemCol;
            compassItemCol = spList.GetItems(spQuery);

            foreach (SPListItem item in compassItemCol)
            {
                if (item != null)
                {

                    TimelineTypeItem obTimelineTypeItem = new TimelineTypeItem();
                    obTimelineTypeItem.WorkflowStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowStep]);
                    obTimelineTypeItem.PhaseNumber = Convert.ToInt32(item[ProjectTimelineTypeDays.PhaseNumber]);
                    obTimelineTypeItem.WorkflowQuickStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowQuickStep]);
                    newItem.Add(obTimelineTypeItem);
                }
            }
            // }
            //}
            return newItem;
        }
        public List<TimelineTypeItem> GetWorkflowStepItems(string timelineType, int CompassListItemId)
        {
            var newItem = new List<TimelineTypeItem>();
            /*SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {*/
            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineTypeListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Neq><FieldRef Name=\"WorkflowOrder\" /><Value Type=\"Int\">0</Value></Neq><And><And><Neq><FieldRef Name=\"WorkflowExceptions\" /><Value Type=\"Text\">DF</Value></Neq><Neq><FieldRef Name=\"WorkflowExceptions\" /><Value Type=\"Text\">NE</Value></Neq></And><Neq><FieldRef Name=\"WorkflowQuickStep\" /><Value Type=\"Text\">SAPRoutingSetup</Value></Neq></And></And></Where><OrderBy><FieldRef Name=\"PhaseNumber\" Type='Int' /><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
            //spQuery.Query = "<Where><And><Neq><FieldRef Name=\"WorkflowOrder\" /><Value Type=\"Int\">0</Value></Neq><Neq><FieldRef Name=\"WorkflowExceptions\" /><Value Type=\"Text\">DF</Value></Neq></And></Where><OrderBy><FieldRef Name=\"PhaseNumber\" Type='Int' /><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
            SPListItemCollection compassItemCol;
            compassItemCol = spList.GetItems(spQuery);

            foreach (SPListItem item in compassItemCol)
            {
                if (item != null)
                {

                    TimelineTypeItem obTimelineTypeItem = new TimelineTypeItem();
                    string WorkflowExceptions = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowExceptions]);
                    string WorkflowQuickStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowQuickStep]);
                    string WorkflowStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowStep]);
                    obTimelineTypeItem.WorkflowStep = WorkflowStep;
                    obTimelineTypeItem.WorkflowQuickStep = WorkflowQuickStep;
                    obTimelineTypeItem.PhaseNumber = Convert.ToInt32(item[ProjectTimelineTypeDays.PhaseNumber]);
                    obTimelineTypeItem.WorkflowOrder = Convert.ToInt32(item[ProjectTimelineTypeDays.WorkflowOrder]);
                    obTimelineTypeItem.WorkflowExceptions = WorkflowExceptions;
                    obTimelineTypeItem.WorkflowStacks = Convert.ToInt32(item[ProjectTimelineTypeDays.WorkflowStacks]);

                    if (timelineType.ToLower() == "standard")
                    {
                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Standard]);
                    }
                    else if (timelineType.ToLower() == "expedited")
                    {
                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Expedited]);
                    }
                    else if (timelineType.ToLower() == "ludicrous")
                    {
                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Ludicrous]);
                    }

                    newItem.Add(obTimelineTypeItem);

                }
            }
            List<TimelineTypeItem> DFItems = GetDFWorkflowStepItems(timelineType.ToLower(), CompassListItemId);
            if (DFItems.Count > 0)
            {
                int graphicsLoc = newItem.FindIndex(i => i.WorkflowQuickStep == "GRAPHICS");
                foreach (TimelineTypeItem DFItem in DFItems)
                {
                    newItem.Insert(graphicsLoc + 1, DFItem);
                    graphicsLoc++;
                }
            }

            List<TimelineTypeItem> newExistingItems = GetNewWorkflowStepItems(timelineType.ToLower(), CompassListItemId);

            if (newExistingItems.Count > 0)
            {
                int newLoc = newItem.FindLastIndex(i => i.WorkflowQuickStep == "CostingQuote");

                foreach (TimelineTypeItem newEItem in newExistingItems)
                {
                    newItem.Insert(newLoc + 1, newEItem);
                    newLoc++;
                }
            }
            /*   }
           }
       });*/
            return newItem;
        }
        public List<List<string>> GetWorkflowTasksStart(int compassId)
        {
            string BOMSetupProcStartDate = "";


            SPList spList = spWeb.Lists.TryGetList(LIST_ApprovalListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
            SPListItemCollection compassItemCol = spList.GetItems(spQuery);

            string projectNo = "";
            string compCostSeasonal = "";
            string compCostCorrPaper = "";
            string compCostFLRP = "";
            List<List<string>> projectWorkflow = new List<List<string>>();
            if (compassItemCol.Count > 0)
            {
                foreach (SPListItem workflowItems in compassItemCol)
                {
                    if (workflowItems != null)
                    {
                        projectWorkflow.Add(new List<string>() { "BOMSetupPE2", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupPE2_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "BOMSetupPE", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupPE_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "BOMSetupProc", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupProc_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "BOMSetupPE3", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupPE3_StartDate]) });
                        BOMSetupProcStartDate = Convert.ToString(workflowItems[ApprovalListFields.BOMSetupProc_StartDate]);
                        projectWorkflow.Add(new List<string>() { "Distribution", Convert.ToString(workflowItems[ApprovalListFields.Distribution_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "ExternalMfg", Convert.ToString(workflowItems[ApprovalListFields.ExternalMfg_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "FGPackSpec", Convert.ToString(workflowItems[ApprovalListFields.FGPackSpec_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "GRAPHICS", Convert.ToString(workflowItems[ApprovalListFields.GRAPHICS_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "OBMReview1", Convert.ToString(workflowItems[ApprovalListFields.OBMReview1_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "OBMReview2", Convert.ToString(workflowItems[ApprovalListFields.OBMReview2_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "Operations", Convert.ToString(workflowItems[ApprovalListFields.Operations_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "QA", Convert.ToString(workflowItems[ApprovalListFields.QA_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPBOMSetup", Convert.ToString(workflowItems[ApprovalListFields.SAPBOMSetup_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPInitialSetup", Convert.ToString(workflowItems[ApprovalListFields.SAPInitialSetup_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "PrelimSAPInitialSetup", Convert.ToString(workflowItems[ApprovalListFields.PrelimSAPInitialSetup_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "SrOBMApproval", Convert.ToString(workflowItems[ApprovalListFields.SrOBMApproval_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "TradePromo", Convert.ToString(workflowItems[ApprovalListFields.TradePromo_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "CostFinishedGood", Convert.ToString(workflowItems[ApprovalListFields.CostFinishedGood_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "FinalCostingReview", Convert.ToString(workflowItems[ApprovalListFields.FinalCostingReview_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "PurchasedPO", Convert.ToString(workflowItems[ApprovalListFields.PurchasedPO_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "RemoveSAPBlocks", Convert.ToString(workflowItems[ApprovalListFields.RemoveSAPBlocks_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "CustomerPO", Convert.ToString(workflowItems[ApprovalListFields.CustomerPO_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "MaterialsRcvdChk", Convert.ToString(workflowItems[ApprovalListFields.MaterialsReceivedChk_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "FirstProductionChk", Convert.ToString(workflowItems[ApprovalListFields.FirstProductionChk_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "DistributionChk", Convert.ToString(workflowItems[ApprovalListFields.DistributionCenterChk_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "MatrlWHSetUp", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupMaterialWarehouse_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPCompleteItem", Convert.ToString(workflowItems[ApprovalListFields.SAPCompleteItemSetup_StartDate]) });

                        projectNo = workflowItems.Title;
                        compCostSeasonal = Convert.ToString(workflowItems[ApprovalListFields.CompCostSeasonal_StartDate]);
                        compCostCorrPaper = Convert.ToString(workflowItems[ApprovalListFields.CompCostCorrPaper_StartDate]);
                        compCostFLRP = Convert.ToString(workflowItems[ApprovalListFields.CompCostFLRP_StartDate]);
                    }
                }
            }
            SPList spList3 = spWeb.Lists.TryGetList(LIST_ApprovalList2Name);
            SPQuery spQuery3 = new SPQuery();
            spQuery3.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
            SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);

            if (compassItemCol3.Count > 0)
            {
                foreach (SPListItem workflowItems in compassItemCol3)
                {
                    if (workflowItems != null)
                    {
                        projectWorkflow.Add(new List<string>() { "ProcAncillary", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcCorrugated", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcPurchased", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcFilm", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcLabel", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcMetal", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcOther", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcPaperboard", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcRigidPlastic", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtAncillary", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtCorrugated", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtPurchased", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtFilm", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtLabel", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtMetal", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtOther", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtPaperboard", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExtRigidPlastic", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcExternal", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcSeasonal", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcCoMan", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "ProcNovelty", BOMSetupProcStartDate });
                        projectWorkflow.Add(new List<string>() { "BEQRC", Convert.ToString(workflowItems[ApprovalListFields.BEQRC_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "EstPricing", Convert.ToString(workflowItems[ApprovalListFields.EstPricing_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "EstBracketPricing", Convert.ToString(workflowItems[ApprovalListFields.EstBracketPricing_StartDate]) });
                    }
                }
            }
            SPList spList2 = spWeb.Lists.TryGetList(LIST_SAPApprovalListName);
            SPQuery spQuery2 = new SPQuery();
            spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
            SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
            if (compassItemCol2.Count > 0)
            {
                foreach (SPListItem workflowItems in compassItemCol2)
                {
                    if (workflowItems != null)
                    {
                        projectWorkflow.Add(new List<string>() { "SAPRoutingSetup", Convert.ToString(workflowItems[SAPApprovalListFields.SAPRoutingSetup_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPCostingDetails", Convert.ToString(workflowItems[SAPApprovalListFields.SAPCostingDetails_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPWarehouseInfo", Convert.ToString(workflowItems[SAPApprovalListFields.SAPWarehouseInfo_StartDate]) });
                        projectWorkflow.Add(new List<string>() { "StandardCostEntry", Convert.ToString(workflowItems[SAPApprovalListFields.StandardCostEntry_StartDate]) });
                    }
                }
            }
            return projectWorkflow;

        }
        public List<List<string>> GetWorkflowTasksEnd(int compassId)
        {


            SPList spList = spWeb.Lists.TryGetList(LIST_ApprovalListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
            string compCostSeasonal = "";
            string compCostCorrPaper = "";
            string compCostFLRP = "";
            string projectNo = "";

            List<List<string>> projectWorkflow = new List<List<string>>();
            if (compassItemCol.Count > 0)
            {
                foreach (SPListItem workflowItems in compassItemCol)
                {
                    if (workflowItems != null)
                    {
                        projectWorkflow.Add(new List<string>() { "IPF", Convert.ToString(workflowItems[ApprovalListFields.IPF_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "BOMSetupPE2", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupPE2_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "BOMSetupPE", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupPE_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "BOMSetupProc", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupProc_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "BOMSetupPE3", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupPE3_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "Distribution", Convert.ToString(workflowItems[ApprovalListFields.Distribution_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ExternalMfg", Convert.ToString(workflowItems[ApprovalListFields.ExternalMfg_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "FGPackSpec", Convert.ToString(workflowItems[ApprovalListFields.FGPackSpec_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "GRAPHICS", Convert.ToString(workflowItems[ApprovalListFields.GRAPHICS_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "OBMReview1", Convert.ToString(workflowItems[ApprovalListFields.OBMReview1_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "OBMReview2", Convert.ToString(workflowItems[ApprovalListFields.OBMReview2_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "Operations", Convert.ToString(workflowItems[ApprovalListFields.Operations_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "QA", Convert.ToString(workflowItems[ApprovalListFields.QA_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPBOMSetup", Convert.ToString(workflowItems[ApprovalListFields.SAPBOMSetup_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPInitialSetup", Convert.ToString(workflowItems[ApprovalListFields.SAPInitialSetup_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "PrelimSAPInitialSetup", Convert.ToString(workflowItems[ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "SrOBMApproval", Convert.ToString(workflowItems[ApprovalListFields.SrOBMApproval_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "TradePromo", Convert.ToString(workflowItems[ApprovalListFields.TradePromo_SubmittedDate]) });
                       // projectWorkflow.Add(new List<string>() { "EstPricing", Convert.ToString(workflowItems[ApprovalListFields.EstPricing_SubmittedDate]) });
                      //  projectWorkflow.Add(new List<string>() { "EstBracketPricing", Convert.ToString(workflowItems[ApprovalListFields.EstBracketPricing_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "CostFinishedGood", Convert.ToString(workflowItems[ApprovalListFields.CostFinishedGood_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "FinalCostingReview", Convert.ToString(workflowItems[ApprovalListFields.FinalCostingReview_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "PurchasedPO", Convert.ToString(workflowItems[ApprovalListFields.PurchasedPO_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "RemoveSAPBlocks", Convert.ToString(workflowItems[ApprovalListFields.RemoveSAPBlocks_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "CustomerPO", Convert.ToString(workflowItems[ApprovalListFields.CustomerPO_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "MaterialsRcvdChk", Convert.ToString(workflowItems[ApprovalListFields.MaterialsReceivedChk_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "FirstProductionChk", Convert.ToString(workflowItems[ApprovalListFields.FirstProductionChk_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "DistributionChk", Convert.ToString(workflowItems[ApprovalListFields.DistributionCenterChk_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "MatrlWHSetUp", Convert.ToString(workflowItems[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPCompleteItem", Convert.ToString(workflowItems[ApprovalListFields.SAPCompleteItemSetup_SubmittedDate]) });

                        projectNo = workflowItems.Title;
                        compCostSeasonal = Convert.ToString(workflowItems[ApprovalListFields.CompCostSeasonal_StartDate]);
                        compCostCorrPaper = Convert.ToString(workflowItems[ApprovalListFields.CompCostCorrPaper_StartDate]);
                        compCostFLRP = Convert.ToString(workflowItems[ApprovalListFields.CompCostFLRP_StartDate]);
                    }
                }
            }
            SPList spList3 = spWeb.Lists.TryGetList(LIST_ApprovalList2Name);
            SPQuery spQuery3 = new SPQuery();
            spQuery3.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
            SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
            if (compassItemCol3.Count > 0)
            {
                foreach (SPListItem workflowItems in compassItemCol3)
                {
                    if (workflowItems != null)
                    {
                        projectWorkflow.Add(new List<string>() { "BEQRC", Convert.ToString(workflowItems[ApprovalListFields.BEQRC_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcAncillary", Convert.ToString(workflowItems[ApprovalListFields.ProcAncillary_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcCorrugated", Convert.ToString(workflowItems[ApprovalListFields.ProcCorrugated_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcPurchased", Convert.ToString(workflowItems[ApprovalListFields.ProcPurchased_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcFilm", Convert.ToString(workflowItems[ApprovalListFields.ProcFilm_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcLabel", Convert.ToString(workflowItems[ApprovalListFields.ProcLabel_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcMetal", Convert.ToString(workflowItems[ApprovalListFields.ProcMetal_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcOther", Convert.ToString(workflowItems[ApprovalListFields.ProcOther_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcPaperboard", Convert.ToString(workflowItems[ApprovalListFields.ProcPaperboard_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcRigidPlastic", Convert.ToString(workflowItems[ApprovalListFields.ProcRigidPlastic_SubmittedDate]) });

                        string procSubmittedDate = "";
                        if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalAncillary_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalAncillary_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalCorrugated_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalCorrugated_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalPurchased_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalPurchased_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalFilm_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalFilm_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalLabel_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalLabel_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalMetal_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalMetal_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalOther_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalOther_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalPaperboard_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalPaperboard_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternalRigidPlastic_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternalRigidPlastic_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcExternal_SubmittedDate])))
                        {
                            procSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcExternal_SubmittedDate]);
                        }
                        if (!string.IsNullOrEmpty(procSubmittedDate) && procSubmittedDate != DateTime.MinValue.ToString())
                        {
                            projectWorkflow.Add(new List<string>() { "ProcExternal", procSubmittedDate });
                        }

                        string procSeasonalSubmittedDate = "";
                        if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcAncillary_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcAncillary_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcCorrugated_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcCorrugated_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcPurchased_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcPurchased_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcFilm_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcFilm_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcLabel_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcLabel_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcMetal_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcMetal_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcOther_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcOther_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcPaperboard_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcPaperboard_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcRigidPlastic_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcRigidPlastic_SubmittedDate]);
                        }
                        else if (!string.IsNullOrEmpty(Convert.ToString(workflowItems[ApprovalListFields.ProcSeasonal_SubmittedDate])))
                        {
                            procSeasonalSubmittedDate = Convert.ToString(workflowItems[ApprovalListFields.ProcSeasonal_SubmittedDate]);
                        }
                        if (!string.IsNullOrEmpty(procSeasonalSubmittedDate) && procSeasonalSubmittedDate != DateTime.MinValue.ToString())
                        {
                            projectWorkflow.Add(new List<string>() { "ProcSeasonal", procSeasonalSubmittedDate });
                        }
                        projectWorkflow.Add(new List<string>() { "ProcCoMan", Convert.ToString(workflowItems[ApprovalListFields.ProcCoMan_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "ProcNovelty", Convert.ToString(workflowItems[ApprovalListFields.ProcNovelty_SubmittedDate]) });
                    }
                }
            }
            SPList spCompassList = spWeb.Lists.TryGetList(LIST_CompassListName);
            SPQuery spCompassQuery = new SPQuery();
            spCompassQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + projectNo + "</Value></Eq></Where>";
            SPListItemCollection CompassItemCol = spCompassList.GetItems(spCompassQuery);

            List<string> SAPItemNos = new List<string>();
            if (CompassItemCol.Count > 0)
            {
                foreach (SPListItem compassItems in CompassItemCol)
                {
                    if (compassItems != null)
                    {
                        SAPItemNos.Add(Convert.ToString(compassItems[CompassListFields.SAPItemNumber]));
                        projectNo = compassItems.Title;
                    }
                }
            }

            SPList spDFList = spWeb.Lists.TryGetList(LIST_DragonflyStatusListName);
            SPQuery spDFQuery = new SPQuery();
            spDFQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + projectNo + "</Value></Eq></Where>";
            SPListItemCollection DFItemCol = spDFList.GetItems(spDFQuery);
            if (DFItemCol.Count > 0)
            {
                foreach (SPListItem DFItem in DFItemCol)
                {
                    if (DFItem != null)
                    {
                        projectWorkflow.Add(new List<string>() { "ProductionArtStarted", Convert.ToString(DFItem[DragonflyStatusListFields.ProductionArtStartedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                        projectWorkflow.Add(new List<string>() { "ProductionArtUploaded", Convert.ToString(DFItem[DragonflyStatusListFields.ProductionArtUploadedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                        projectWorkflow.Add(new List<string>() { "RoutingComplete", Convert.ToString(DFItem[DragonflyStatusListFields.RoutingCompleteActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                        projectWorkflow.Add(new List<string>() { "ProofingStarted", Convert.ToString(DFItem[DragonflyStatusListFields.ProofingStartedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                        projectWorkflow.Add(new List<string>() { "ProofApproved", Convert.ToString(DFItem[DragonflyStatusListFields.ProofApprovedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                        projectWorkflow.Add(new List<string>() { "FinalFilesPlatesShipped", Convert.ToString(DFItem[DragonflyStatusListFields.FinalFilesPlatesShippedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                        projectWorkflow.Add(new List<string>() { "CostingQuote", "", Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    }
                }
            }
            SPList spList2 = spWeb.Lists.TryGetList(LIST_SAPApprovalListName);
            SPQuery spQuery2 = new SPQuery();
            spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
            SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
            if (compassItemCol2.Count > 0)
            {
                foreach (SPListItem workflowItems in compassItemCol2)
                {
                    if (workflowItems != null)
                    {
                        projectWorkflow.Add(new List<string>() { "SAPRoutingSetup", Convert.ToString(workflowItems[SAPApprovalListFields.SAPRoutingSetup_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPCostingDetails", Convert.ToString(workflowItems[SAPApprovalListFields.SAPCostingDetails_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "SAPWarehouseInfo", Convert.ToString(workflowItems[SAPApprovalListFields.SAPWarehouseInfo_SubmittedDate]) });
                        projectWorkflow.Add(new List<string>() { "StandardCostEntry", Convert.ToString(workflowItems[SAPApprovalListFields.StandardCostEntry_SubmittedDate]) });
                    }
                }
            }
            return projectWorkflow;

        }
        public List<DateTime> GetHolidays()
        {
            var newItem = new List<DateTime>();
            /* using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
             {
                 using (SPWeb spWeb = spSite.OpenWeb())
                 {*/
            SPList spList = spWeb.Lists.TryGetList(LIST_HolidayLookup);
            SPListItemCollection compassItemCol = spList.GetItems();

            foreach (SPListItem item in compassItemCol)
            {
                if (item != null)
                {

                    newItem.Add(Convert.ToDateTime(item["HolidayDate"]));
                }
            }
            //}
            // }
            return newItem;
        }
        private Dictionary<string, string> hideRow(int compassID)
        {
            Dictionary<string, string> showHideParams = new Dictionary<string, string>();
            showHideParams.Add("GRAPHICS", "N");
            showHideParams.Add("ExternalMfg", "N");
            showHideParams.Add("ProcAncillary", "Y");
            showHideParams.Add("ProcCorrugated", "Y");
            showHideParams.Add("ProcPurchased", "Y");
            showHideParams.Add("ProcFilm", "Y");
            showHideParams.Add("ProcLabel", "Y");
            showHideParams.Add("ProcMetal", "Y");
            showHideParams.Add("ProcOther", "Y");
            showHideParams.Add("ProcPaperboard", "Y");
            showHideParams.Add("ProcRigidPlastic", "Y");
            showHideParams.Add("ProcExternal", "Y");
            showHideParams.Add("ProcNovelty", "Y");
            showHideParams.Add("ProcCoMan", "Y");
            showHideParams.Add("ProcSeasonal", "Y");
            showHideParams.Add("QA", "N");
            showHideParams.Add("BOMSetupPE", "N");
            showHideParams.Add("BOMSetupProc", "N");
            showHideParams.Add("BOMSetupPE3", "N");
            showHideParams.Add("OBMReview2", "N");
            showHideParams.Add("Distribution", "N");
            showHideParams.Add("SAPInitialSetup", "N");
            showHideParams.Add("PrelimSAPInitialSetup", "N");
            showHideParams.Add("TradePromo", "N");
            showHideParams.Add("EstPricing", "N");
            showHideParams.Add("EstBracketPricing", "N");
            showHideParams.Add("MatrlWHSetUp", "Y");
            showHideParams.Add("SAPCompleteItem", "N");
            showHideParams.Add("FGPackSpec", "N");
            showHideParams.Add("SrOBMApproval", "N");
            showHideParams.Add("SrOBMApproval2", "N");
            showHideParams.Add("BEQRC", "Y");

            string projectType = "";
            string PHL1 = "";
            string comanType = "";
            string novelty = "";
            string projectSubcat = "";
            string PLMFlag = "";
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {

                    SPList spCompassList = spWeb.Lists.TryGetList(LIST_CompassListName);
                    SPListItem itemCompass = spCompassList.GetItemById(compassID);
                    if (itemCompass != null)
                    {
                        PHL1 = Convert.ToString(itemCompass[CompassListFields.ProductHierarchyLevel1]);
                        comanType = Convert.ToString(itemCompass[CompassListFields.CoManufacturingClassification]);
                        novelty = Convert.ToString(itemCompass[CompassListFields.NoveltyProject]);
                        projectType = Convert.ToString(itemCompass[CompassListFields.ProjectType]);
                        projectSubcat = Convert.ToString(itemCompass[CompassListFields.ProjectTypeSubCategory]);
                        PLMFlag = Convert.ToString(itemCompass[CompassListFields.PLMProject]);
                        if (PHL1 == PRODUCT_HIERARCHY1_CoMan)
                        {
                            showHideParams["ProcCoMan"] = "N";
                        }
                        else if (PHL1 == PRODUCT_HIERARCHY1_Seasonal)
                        {
                            if (novelty == "Yes")
                            {
                                showHideParams["ProcNovelty"] = "N";
                            }
                            else if (comanType == "External Turnkey FG")
                            {
                                showHideParams["ProcExternal"] = "N";
                            }
                            else
                            {
                                showHideParams["ProcSeasonal"] = "N";
                            }

                        }
                        else
                        {
                            if (novelty == "Yes")
                            {
                                showHideParams["ProcNovelty"] = "N";
                            }
                            else if (comanType == "External Turnkey FG")
                            {
                                showHideParams["ProcExternal"] = "N";
                            }
                        }
                        if (PLMFlag != "Yes")
                        {
                            showHideParams["BOMSetupPE3"] = "Y";
                            showHideParams["SAPCompleteItem"] = "Y";
                        }
                        else
                        {
                            showHideParams["FGPackSpec"] = "Y";
                        }
                        string tbdindicator = Convert.ToString(itemCompass[CompassListFields.TBDIndicator]);
                        if (tbdindicator != "Yes")
                        {
                            showHideParams["Distribution"] = "Y";
                            showHideParams["SAPInitialSetup"] = "Y";
                            showHideParams["PrelimSAPInitialSetup"] = "Y";
                            showHideParams["TradePromo"] = "Y";
                            showHideParams["EstPricing"] = "Y";
                            showHideParams["EstBracketPricing"] = "Y";
                        }
                        //For Hiding By Project Type
                        projectType = Convert.ToString(itemCompass[CompassListFields.ProjectType]);
                        if (projectType == PROJECTTYPE_GraphicsChangesInternalAdjustments)
                        {
                            /*if (tbdindicator != "Yes")
                            {
                                showHideParams["TradePromo"] = "Y");
                                showHideParams["Distribution"] = "Y");
                                showHideParams["SAPInitialSetup"] = "Y");
                                showHideParams["PrelimSAPInitialSetup"] = "Y");
                            }*/
                            showHideParams["QA"] = "Y";
                        }
                        else if (projectType == PROJECTTYPE_SimpleNetworkMove)
                        {
                            /*if (tbdindicator != "Yes")
                            {
                                showHideParams["PrelimSAPInitialSetup", "Y");
                                showHideParams["TradePromo", "Y");

                            }*/
                            showHideParams["ExternalMfg"] = "Y";
                            showHideParams["QA"] = "Y";
                            showHideParams["BOMSetupPE"] = "Y";
                            showHideParams["BOMSetupProc"] = "Y";
                            showHideParams["OBMReview2"] = "Y";
                            showHideParams["ProcCoMan"] = "Y";
                            showHideParams["ProcExternal"] = "Y";
                            showHideParams["ProcNovelty"] = "Y";
                            showHideParams["ProcSeasonal"] = "Y";
                        }
                        string manLocChange = Convert.ToString(itemCompass[CompassListFields.MfgLocationChange]);
                        if (PLMFlag == "Yes" && (projectSubcat == PROJECTTYPESUBCATEGORY_ComplexNetworkMove || projectType == PROJECTTYPE_SimpleNetworkMove || tbdindicator == "Yes" || manLocChange == "Yes"))
                        {
                            showHideParams["Distribution"] = "N";
                        }
                    }
                    string manufacturingLocation = Convert.ToString(itemCompass[CompassListFields.ManufacturingLocation]);
                    string packingLocation = Convert.ToString(itemCompass[CompassListFields.PackingLocation]);
                    SPList sDecisionItemsList = spWeb.Lists.TryGetList(LIST_ProjectDecisionsListName);
                    SPQuery spDecisionsQuery = new SPQuery();
                    spDecisionsQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassID.ToString() + "</Value></Eq></Where>";
                    SPListItemCollection sDecisionItems = sDecisionItemsList.GetItems(spDecisionsQuery);
                    if (sDecisionItems.Count > 0)
                    {
                        SPItem item = sDecisionItems[0];

                        if (item != null)
                        {
                            string approvalDecision = Convert.ToString(item[SrOBMApproval_Decision]);
                            string approval2Decision = Convert.ToString(item[SrOBMApproval2_Decision]);
                            if (approvalDecision.ToLower() == "request ipf update")
                            {
                                showHideParams["SrOBMApproval"] = "Y";
                            }

                            if (approval2Decision.ToLower() == "request ipf update")
                            {
                                showHideParams["SrOBMApproval2"] = "Y";
                            }
                        }
                    }



                    if ((!manufacturingLocation.ToLower().Contains("external") && !packingLocation.ToLower().Contains("external")) && projectType != PROJECTTYPE_SimpleNetworkMove)
                    {
                        showHideParams["ExternalMfg"] = "Y";
                    }
                    SPList spPackItemsList = spWeb.Lists.TryGetList(LIST_PackagingItemListName);
                    SPQuery spPIQuery = new SPQuery();
                    spPIQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassID.ToString() + "</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></Where>";
                    SPListItemCollection spPackItems = spPackItemsList.GetItems(spPIQuery);
                    bool hasGraphics = false;
                    bool hasPCS = false;
                    bool hasReviewPrinterSupplier = false;
                    bool BEQRC = false;
                    int newComponentCount = 0;
                    if (spPackItems.Count > 0)
                    {
                        foreach (SPItem item in spPackItems)
                        {
                            if (item != null)
                            {
                                string newGraphicsRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                                string packagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                                string newExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                                string IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);

                                if (newGraphicsRequired == "Yes")
                                {
                                    hasGraphics = true;
                                }
                                if (packagingComponent == COMPONENTTYPE_PurchasedSemi)
                                {
                                    hasPCS = true;
                                }
                                if (packagingComponent == COMPONENTTYPE_TransferSemi && newExisting == "New")
                                {
                                    showHideParams["SAPInitialSetup"] = "N";
                                }
                                if (string.Equals(newExisting, "New"))
                                {
                                    newComponentCount++;
                                }

                                string reviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]).ToLower();
                                if (showHideParams["ProcSeasonal"] != "N" && showHideParams["ProcCoMan"] != "N" && showHideParams["ProcNovelty"] != "N" && comanType != "External Turnkey FG" && showHideParams["BOMSetupProc"] == "N")
                                {
                                    if (((newExisting == "New" || newExisting == "Network Move") || projectType == PROJECTTYPE_SimpleNetworkMove || projectSubcat == PROJECTTYPESUBCATEGORY_ComplexNetworkMove) && reviewPrinterSupplier != "yes")
                                    {
                                        if (packagingComponent.ToLower().Contains("ancillary"))
                                        {
                                            showHideParams["ProcAncillary"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("corrugated"))
                                        {
                                            showHideParams["ProcCorrugated"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("(purchased)"))
                                        {
                                            showHideParams["ProcPurchased"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("film"))
                                        {
                                            showHideParams["ProcFilm"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("label"))
                                        {
                                            showHideParams["ProcLabel"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("metal"))
                                        {
                                            showHideParams["ProcMetal"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("other"))
                                        {
                                            showHideParams["ProcOther"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("paperboard"))
                                        {
                                            showHideParams["ProcPaperboard"] = "N";
                                        }
                                        else if (packagingComponent.ToLower().Contains("rigid"))
                                        {
                                            showHideParams["ProcRigidPlastic"] = "N";
                                        }
                                    }
                                }

                                if (reviewPrinterSupplier == "yes")
                                {
                                    hasReviewPrinterSupplier = true;
                                }


                                if (packagingComponent.ToLower().Contains("finished good") && IngredientsNeedToClaimBioEng == "Yes")
                                {
                                    BEQRC = true;
                                }
                            }
                        }
                    }

                    #region Marketing claims
                    SPList spList = spWeb.Lists.TryGetList(LIST_MarketingClaimsListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + compassID.ToString() + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol != null)
                    {
                        if (compassItemCol.Count > 0)
                        {
                            var item = compassItemCol[0];

                            var ClaimBioEngineering = Convert.ToString(item["BioEngLabelingAcceptable"]);
                            if (ClaimBioEngineering == "Yes")
                            {
                                BEQRC = true;
                            }
                        }
                    }

                    if (BEQRC)
                    {
                        showHideParams["BEQRC"] = "N";
                    }
                    #endregion

                    if (!hasGraphics)
                    {
                        showHideParams["GRAPHICS"] = "Y";
                    }
                    if (hasPCS)
                    {
                        showHideParams["ExternalMfg"] = "N";
                    }
                    if (hasReviewPrinterSupplier && showHideParams["ProcCoMan"] != "N" && showHideParams["ProcNovelty"] != "N" && showHideParams["BOMSetupProc"] == "N" && showHideParams["ExternalMfg"] == "N")
                    {
                        hasReviewPrinterSupplier = true;
                        showHideParams["ProcExternal"] = "N";
                    }
                    if (PLMFlag == "Yes")
                    {
                        if (string.Equals(projectSubcat, PROJECTTYPESUBCATEGORY_ComplexNetworkMove) || string.Equals(projectType, PROJECTTYPE_SimpleNetworkMove))
                        {
                            showHideParams["MatrlWHSetUp"] = "N";
                        }
                        else
                        {
                            if (newComponentCount > 0)
                            {
                                if (comanType != "External Turnkey FG")
                                {
                                    showHideParams["MatrlWHSetUp"] = "N";
                                }
                            }
                        }
                    }
                    if (newComponentCount == 0)
                    {

                        if (PHL1 == PRODUCT_HIERARCHY1_Seasonal)
                        {
                            showHideParams["ProcSeasonal"] = "Y";
                        }
                    }
                }
            }

            return showHideParams;
        }

        public List<TimelineTypeItem> GetDFWorkflowStepItems(string timelineType, int CompassListItemId)
        {
            List<TimelineTypeItem> FinalDFItems = new List<TimelineTypeItem>();

            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineTypeListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Neq><FieldRef Name=\"WorkflowOrder\"></FieldRef><Value Type=\"Int\">0</Value></Neq><Eq><FieldRef Name=\"WorkflowExceptions\"></FieldRef><Value Type=\"Text\">DF</Value></Eq></And></Where><OrderBy><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
            SPListItemCollection compassItemCol;
            compassItemCol = spList.GetItems(spQuery);
            List<TimelineTypeItem> DFItems = new List<TimelineTypeItem>();

            SPList spGraphicsList = spWeb.Lists.TryGetList(LIST_PackagingItemListName);
            SPQuery spGraphicsQuery = new SPQuery();
            spGraphicsQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId.ToString() + "</Value></Eq><And><Eq><FieldRef Name=\"GraphicsChangeRequired\" /><Value Type=\"Text\">Yes</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></And></Where>";
            SPListItemCollection itemUpdate = spGraphicsList.GetItems(spGraphicsQuery);

            if (itemUpdate.Count > 0)
            {
                List<KeyValuePair<string, string>> PIHeaders = new List<KeyValuePair<string, string>>();
                foreach (SPListItem packaging in itemUpdate)
                {
                    if (packaging != null)
                    {
                        string matId = Convert.ToString(packaging.ID);
                        string matNumber = Convert.ToString(packaging[PackagingItemListFields.MaterialNumber]);
                        string matDesc = Convert.ToString(packaging[PackagingItemListFields.MaterialDescription]);
                        string piHeaderText = matNumber + ": " + matDesc;
                        PIHeaders.Add(new KeyValuePair<string, string>(matId, piHeaderText));
                    }
                }
                if (itemUpdate.Count > 1 && PIHeaders.Count > 0)
                {


                    List<List<TimelineTypeItem>> graphicsList = new List<List<TimelineTypeItem>>();
                    foreach (KeyValuePair<string, string> header in PIHeaders)
                    {
                        List<TimelineTypeItem> graphicsItems = new List<TimelineTypeItem>(copyItem(compassItemCol, timelineType));

                        TimelineTypeItem headerItem = new TimelineTypeItem();
                        headerItem.WorkflowOrder = 0;
                        headerItem.WorkflowExceptions = "DF";
                        headerItem.WorkflowStep = header.Value;
                        headerItem.PhaseNumber = 4;
                        headerItem.Holder = graphicsItems[0].Holder;
                        graphicsItems.Insert(0, headerItem);
                        graphicsList.Add(graphicsItems);
                    }
                    int headerCount = 0;
                    foreach (List<TimelineTypeItem> DFItemsToAdd in graphicsList)
                    {
                        foreach (TimelineTypeItem DFItemToAdd in DFItemsToAdd)
                        {
                            string headerValue = PIHeaders[headerCount].Value;
                            int charLoc = headerValue.IndexOf(":");
                            String MatNumber = headerValue.Substring(0, charLoc);
                            DFItemToAdd.WorkflowMisc = MatNumber;
                            DFItemToAdd.Expedited = Convert.ToInt32(PIHeaders[headerCount].Key);
                            FinalDFItems.Add(DFItemToAdd);
                        }
                        headerCount++;
                    }
                }
                else if (itemUpdate.Count == 1 && PIHeaders.Count > 0)
                {
                    List<TimelineTypeItem> graphicsItems = new List<TimelineTypeItem>(copyItem(compassItemCol, timelineType));
                    foreach (TimelineTypeItem DFItemToAdd in graphicsItems)
                    {
                        string headerValue = PIHeaders[0].Value;
                        int charLoc = headerValue.IndexOf(":");
                        String MatNumber = headerValue.Substring(0, charLoc);
                        DFItemToAdd.WorkflowMisc = MatNumber;
                        DFItemToAdd.Expedited = Convert.ToInt32(PIHeaders[0].Key);
                        FinalDFItems.Add(DFItemToAdd);
                    }
                    //FinalDFItems = copyItem(compassItemCol, timelineType);
                }

            }

            return FinalDFItems;
        }
        public List<TimelineTypeItem> GetNewWorkflowStepItems(string timelineType, int CompassListItemId)
        {
            List<TimelineTypeItem> FinalNewItems = new List<TimelineTypeItem>();

            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineTypeListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Neq><FieldRef Name=\"WorkflowOrder\"></FieldRef><Value Type=\"Int\">0</Value></Neq><Or><Eq><FieldRef Name=\"WorkflowExceptions\"></FieldRef><Value Type=\"Text\">NE</Value></Eq><Eq><FieldRef Name=\"WorkflowQuickStep\"></FieldRef><Value Type=\"Text\">SAPRoutingSetup</Value></Eq></Or></And></Where><OrderBy><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
            SPListItemCollection compassItemCol;
            compassItemCol = spList.GetItems(spQuery);
            List<TimelineTypeItem> DFItems = new List<TimelineTypeItem>();

            SPList spGraphicsList = spWeb.Lists.TryGetList(LIST_PackagingItemListName);
            SPQuery spGraphicsQuery = new SPQuery();
            spGraphicsQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId.ToString() + "</Value></Eq><And><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></And></Where>";
            SPListItemCollection itemUpdate = spGraphicsList.GetItems(spGraphicsQuery);
            TimelineTypeItem graphicsHeaderItem = new TimelineTypeItem();
            if (itemUpdate.Count > 0)
            {
                foreach (SPListItem item in compassItemCol)
                {
                    if (item != null)
                    {
                        TimelineTypeItem obTimelineTypeItem = new TimelineTypeItem();

                        string WorkflowQuickStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowQuickStep]);
                        string WorkflowStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowStep]);
                        obTimelineTypeItem.WorkflowStep = WorkflowStep;
                        obTimelineTypeItem.WorkflowQuickStep = WorkflowQuickStep;
                        obTimelineTypeItem.PhaseNumber = Convert.ToInt32(item[ProjectTimelineTypeDays.PhaseNumber]);
                        obTimelineTypeItem.WorkflowOrder = Convert.ToInt32(item[ProjectTimelineTypeDays.WorkflowOrder]);
                        obTimelineTypeItem.WorkflowExceptions = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowExceptions]);
                        obTimelineTypeItem.WorkflowStacks = Convert.ToInt32(item[ProjectTimelineTypeDays.WorkflowStacks]);
                        obTimelineTypeItem.WorkflowMisc = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowMisc]);

                        if (timelineType.ToLower() == "standard")
                        {
                            obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Standard]);
                        }
                        else if (timelineType.ToLower() == "expedited")
                        {
                            obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Expedited]);
                        }
                        else if (timelineType.ToLower() == "ludicrous")
                        {
                            obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Ludicrous]);
                        }
                        graphicsHeaderItem = obTimelineTypeItem;
                        DFItems.Add(obTimelineTypeItem);
                    }
                }
                /*List<string> PIHeaders = new List<string>();
                if (itemUpdate.Count > 1)
                {
                    foreach (SPListItem packaging in itemUpdate)
                    {
                        string matNumber = Convert.ToString(packaging[PackagingItemListFields.MaterialNumber]);
                        string matDesc = Convert.ToString(packaging[PackagingItemListFields.MaterialDescription]);
                        string piHeaderText = matNumber + ": " + matDesc;
                        PIHeaders.Add(piHeaderText);
                    }
                    List<List<TimelineTypeItem>> graphicsList = new List<List<TimelineTypeItem>>();
                    foreach (string header in PIHeaders)
                    {
                        List<TimelineTypeItem> graphicsItems = new List<TimelineTypeItem>(DFItems);
                        TimelineTypeItem headerItem = new TimelineTypeItem();
                        headerItem.WorkflowOrder = 0;
                        headerItem.WorkflowExceptions = "NE";
                        headerItem.WorkflowStep = header;
                        headerItem.PhaseNumber = 4;
                        headerItem.Holder = 4;
                        graphicsItems.Insert(0, headerItem);
                        graphicsList.Add(graphicsItems);
                    }
                    foreach (List<TimelineTypeItem> DFItemsToAdd in graphicsList)
                    {
                        foreach (TimelineTypeItem DFItemToAdd in DFItemsToAdd)
                        {
                            FinalNewItems.Add(DFItemToAdd);
                        }
                    }
                }
                else if (itemUpdate.Count == 1)
                {
                    FinalNewItems = DFItems;
                }*/
            }
            return DFItems;
        }
        public ApprovalListItem GetApprovalItem(int itemId)
        {
            ApprovalListItem appItem = new ApprovalListItem();
            /*using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {*/
            SPList spList = spWeb.Lists.TryGetList(LIST_ApprovalListName);

            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            spQuery.RowLimit = 1;

            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
            if (compassItemCol.Count > 0)
            {
                SPListItem item = compassItemCol[0];

                if (item != null)
                {
                    try
                    {
                        appItem.ApprovalListItemId = item.ID;
                        appItem.CompassListItemId = Convert.ToInt32(item[ApprovalListFields.CompassListItemId]);
                        // IPF Fields
                        appItem.IPF_SubmittedBy = Convert.ToString(item[ApprovalListFields.IPF_SubmittedBy]);
                        appItem.IPF_SubmittedDate = Convert.ToString(item[ApprovalListFields.IPF_SubmittedDate]);
                        appItem.IPF_ModifiedBy = Convert.ToString(item[ApprovalListFields.IPF_ModifiedBy]);
                        appItem.IPF_ModifiedDate = Convert.ToString(item[ApprovalListFields.IPF_ModifiedDate]);
                        // SrOBMApproval, SrOBMApproval2 Fields
                        appItem.SrOBMApproval_StartDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_StartDate]);
                        appItem.SrOBMApproval_ModifiedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_ModifiedDate]);
                        appItem.SrOBMApproval_ModifiedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval_ModifiedBy]);
                        appItem.SrOBMApproval_SubmittedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_SubmittedDate]);
                        appItem.SrOBMApproval_SubmittedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval_SubmittedBy]);

                        appItem.SrOBMApproval2_StartDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_StartDate]);
                        appItem.SrOBMApproval2_ModifiedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_ModifiedDate]);
                        appItem.SrOBMApproval2_ModifiedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_ModifiedBy]);
                        appItem.SrOBMApproval2_SubmittedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_SubmittedDate]);
                        appItem.SrOBMApproval2_SubmittedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval2_SubmittedBy]);
                        // Initial Costing Fields
                        appItem.InitialCosting_StartDate = Convert.ToString(item[ApprovalListFields.InitialCosting_StartDate]);
                        appItem.InitialCosting_ModifiedBy = Convert.ToString(item[ApprovalListFields.InitialCosting_ModifiedBy]);
                        appItem.InitialCosting_ModifiedDate = Convert.ToString(item[ApprovalListFields.InitialCosting_ModifiedDate]);
                        appItem.InitialCosting_SubmittedBy = Convert.ToString(item[ApprovalListFields.InitialCosting_SubmittedBy]);
                        appItem.InitialCosting_SubmittedDate = Convert.ToString(item[ApprovalListFields.InitialCosting_SubmittedDate]);
                        // Distribution Fields
                        appItem.Distribution_StartDate = Convert.ToString(item[ApprovalListFields.Distribution_StartDate]);
                        appItem.Distribution_ModifiedDate = Convert.ToString(item[ApprovalListFields.Distribution_ModifiedDate]);
                        appItem.Distribution_ModifiedBy = Convert.ToString(item[ApprovalListFields.Distribution_ModifiedBy]);
                        appItem.Distribution_SubmittedDate = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedDate]);
                        appItem.Distribution_SubmittedBy = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedBy]);
                        // Operations Make Pack Fields
                        appItem.Operations_StartDate = Convert.ToString(item[ApprovalListFields.Operations_StartDate]);
                        appItem.Operations_ModifiedDate = Convert.ToString(item[ApprovalListFields.Operations_ModifiedDate]);
                        appItem.Operations_ModifiedBy = Convert.ToString(item[ApprovalListFields.Operations_ModifiedBy]);
                        appItem.Operations_SubmittedDate = Convert.ToString(item[ApprovalListFields.Operations_SubmittedDate]);
                        appItem.Operations_SubmittedBy = Convert.ToString(item[ApprovalListFields.Operations_SubmittedBy]);
                        // SAP Item Request Fields
                        appItem.SAPInitialSetup_StartDate = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_StartDate]);
                        appItem.SAPInitialSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_ModifiedDate]);
                        appItem.SAPInitialSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_ModifiedBy]);
                        appItem.SAPInitialSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_SubmittedDate]);
                        appItem.SAPInitialSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_SubmittedBy]);
                        // Preliminary SAP Item Request Fields
                        appItem.PrelimSAPInitialSetup_StartDate = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_StartDate]);
                        appItem.PrelimSAPInitialSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_ModifiedDate]);
                        appItem.PrelimSAPInitialSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_ModifiedBy]);
                        appItem.PrelimSAPInitialSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate]);
                        appItem.PrelimSAPInitialSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_SubmittedBy]);
                        // QA Fields
                        appItem.QA_StartDate = Convert.ToString(item[ApprovalListFields.QA_StartDate]);
                        appItem.QA_ModifiedDate = Convert.ToString(item[ApprovalListFields.QA_ModifiedDate]);
                        appItem.QA_ModifiedBy = Convert.ToString(item[ApprovalListFields.QA_ModifiedBy]);
                        appItem.QA_SubmittedDate = Convert.ToString(item[ApprovalListFields.QA_SubmittedDate]);
                        appItem.QA_SubmittedBy = Convert.ToString(item[ApprovalListFields.QA_SubmittedBy]);
                        // OBM First Review Fields
                        appItem.OBMReview1_StartDate = Convert.ToString(item[ApprovalListFields.OBMReview1_StartDate]);
                        appItem.OBMReview1_ModifiedDate = Convert.ToString(item[ApprovalListFields.OBMReview1_ModifiedDate]);
                        appItem.OBMReview1_ModifiedBy = Convert.ToString(item[ApprovalListFields.OBMReview1_ModifiedBy]);
                        appItem.OBMReview1_SubmittedDate = Convert.ToString(item[ApprovalListFields.OBMReview1_SubmittedDate]);
                        appItem.OBMReview1_SubmittedBy = Convert.ToString(item[ApprovalListFields.OBMReview1_SubmittedBy]);
                        // Material Numbers Fields
                        appItem.BOMSetupPE_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_StartDate]);
                        appItem.BOMSetupPE_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_ModifiedDate]);
                        appItem.BOMSetupPE_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE_ModifiedBy]);
                        appItem.BOMSetupPE_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_SubmittedDate]);
                        appItem.BOMSetupPE_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE_SubmittedBy]);
                        appItem.BOMSetupProc_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_StartDate]);
                        appItem.BOMSetupProc_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_ModifiedDate]);
                        appItem.BOMSetupProc_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupProc_ModifiedBy]);
                        appItem.BOMSetupProc_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_SubmittedDate]);
                        appItem.BOMSetupProc_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupProc_SubmittedBy]);
                        appItem.BOMSetupPE2_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_StartDate]);
                        appItem.BOMSetupPE2_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_ModifiedDate]);
                        appItem.BOMSetupPE2_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_ModifiedBy]);
                        appItem.BOMSetupPE2_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_SubmittedDate]);
                        appItem.BOMSetupPE2_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_SubmittedBy]);
                        appItem.BOMSetupPE3_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_StartDate]);
                        appItem.BOMSetupPE3_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_ModifiedDate]);
                        appItem.BOMSetupPE3_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_ModifiedBy]);
                        appItem.BOMSetupPE3_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_SubmittedDate]);
                        appItem.BOMSetupPE3_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE3_SubmittedBy]);

                        appItem.MatrlWHSetUp_StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_StartDate]);
                        appItem.MatrlWHSetUp_ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedDate]);
                        appItem.MatrlWHSetUp_ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedBy]);
                        appItem.MatrlWHSetUp_SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate]);
                        appItem.MatrlWHSetUp_SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedBy]);

                        appItem.SAPCompleteItem_StartDate = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_StartDate]);
                        appItem.SAPCompleteItem_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_ModifiedDate]);
                        appItem.SAPCompleteItem_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_ModifiedBy]);
                        appItem.SAPCompleteItem_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_SubmittedDate]);
                        appItem.SAPCompleteItem_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPCompleteItemSetup_SubmittedBy]);
                        // SAP Item Setup Fields
                        // OBM Second Review Fields
                        appItem.OBMReview2_StartDate = Convert.ToString(item[ApprovalListFields.OBMReview2_StartDate]);
                        appItem.OBMReview2_ModifiedDate = Convert.ToString(item[ApprovalListFields.OBMReview2_ModifiedDate]);
                        appItem.OBMReview2_ModifiedBy = Convert.ToString(item[ApprovalListFields.OBMReview2_ModifiedBy]);
                        appItem.OBMReview2_SubmittedDate = Convert.ToString(item[ApprovalListFields.OBMReview2_SubmittedDate]);
                        appItem.OBMReview2_SubmittedBy = Convert.ToString(item[ApprovalListFields.OBMReview2_SubmittedBy]);
                        // Request For Graphics Fields
                        appItem.GRAPHICS_StartDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_StartDate]);
                        appItem.GRAPHICS_ModifiedDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedDate]);
                        appItem.GRAPHICS_ModifiedBy = Convert.ToString(item[ApprovalListFields.GRAPHICS_ModifiedBy]);
                        appItem.GRAPHICS_SubmittedDate = Convert.ToString(item[ApprovalListFields.GRAPHICS_SubmittedDate]);
                        appItem.GRAPHICS_SubmittedBy = Convert.ToString(item[ApprovalListFields.GRAPHICS_SubmittedBy]);
                        // SAPBOMSetup Fields
                        appItem.SAPBOMSetup_StartDate = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_StartDate]);
                        appItem.SAPBOMSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_ModifiedDate]);
                        appItem.SAPBOMSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_ModifiedBy]);
                        appItem.SAPBOMSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_SubmittedDate]);
                        appItem.SAPBOMSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPBOMSetup_SubmittedBy]);
                        // FGPackSpec Fields
                        appItem.FGPackSpec_StartDate = Convert.ToString(item[ApprovalListFields.FGPackSpec_StartDate]);
                        appItem.FGPackSpec_ModifiedDate = Convert.ToString(item[ApprovalListFields.FGPackSpec_ModifiedDate]);
                        appItem.FGPackSpec_ModifiedBy = Convert.ToString(item[ApprovalListFields.FGPackSpec_ModifiedBy]);
                        appItem.FGPackSpec_SubmittedDate = Convert.ToString(item[ApprovalListFields.FGPackSpec_SubmittedDate]);
                        appItem.FGPackSpec_SubmittedBy = Convert.ToString(item[ApprovalListFields.FGPackSpec_SubmittedBy]);
                        // CostingQuote Fields
                        appItem.CompCostFLRP_StartDate = Convert.ToString(item[ApprovalListFields.CompCostFLRP_StartDate]);
                        appItem.CompCostFLRP_ModifiedDate = Convert.ToString(item[ApprovalListFields.CompCostFLRP_ModifiedDate]);
                        appItem.CompCostFLRP_ModifiedBy = Convert.ToString(item[ApprovalListFields.CompCostFLRP_ModifiedBy]);
                        appItem.CompCostFLRP_SubmittedDate = Convert.ToString(item[ApprovalListFields.CompCostFLRP_SubmittedDate]);
                        appItem.CompCostFLRP_SubmittedBy = Convert.ToString(item[ApprovalListFields.CompCostFLRP_SubmittedBy]);

                        appItem.CompCostSeasonal_StartDate = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_StartDate]);
                        appItem.CompCostSeasonal_ModifiedDate = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_ModifiedDate]);
                        appItem.CompCostSeasonal_ModifiedBy = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_ModifiedBy]);
                        appItem.CompCostSeasonal_SubmittedDate = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_SubmittedDate]);
                        appItem.CompCostSeasonal_SubmittedBy = Convert.ToString(item[ApprovalListFields.CompCostSeasonal_SubmittedBy]);

                        appItem.CompCostCorrPaper_StartDate = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_StartDate]);
                        appItem.CompCostCorrPaper_ModifiedDate = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_ModifiedDate]);
                        appItem.CompCostCorrPaper_ModifiedBy = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_ModifiedBy]);
                        appItem.CompCostCorrPaper_SubmittedDate = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_SubmittedDate]);
                        appItem.CompCostCorrPaper_SubmittedBy = Convert.ToString(item[ApprovalListFields.CompCostCorrPaper_SubmittedBy]);
                        //ExternalMfg
                        appItem.ExternalMfg_StartDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_StartDate]);
                        appItem.ExternalMfg_ModifiedDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_ModifiedDate]);
                        appItem.ExternalMfg_ModifiedBy = Convert.ToString(item[ApprovalListFields.ExternalMfg_ModifiedBy]);
                        appItem.ExternalMfg_SubmittedDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_SubmittedDate]);
                        appItem.ExternalMfg_SubmittedBy = Convert.ToString(item[ApprovalListFields.ExternalMfg_SubmittedBy]);
                        //SAP Routing Setup
                        appItem.SAPRoutingSetup_StartDate = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_StartDate]);
                        appItem.SAPRoutingSetup_ModifiedDate = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_ModifiedDate]);
                        appItem.SAPRoutingSetup_ModifiedBy = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_ModifiedBy]);
                        appItem.SAPRoutingSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_SubmittedDate]);
                        appItem.SAPRoutingSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPRoutingSetup_SubmittedBy]);
                        //Trade Promo
                        appItem.TradePromo_StartDate = Convert.ToString(item[ApprovalListFields.TradePromo_StartDate]);
                        appItem.TradePromo_ModifiedDate = Convert.ToString(item[ApprovalListFields.TradePromo_ModifiedDate]);
                        appItem.TradePromo_ModifiedBy = Convert.ToString(item[ApprovalListFields.TradePromo_ModifiedBy]);
                        appItem.TradePromo_SubmittedDate = Convert.ToString(item[ApprovalListFields.TradePromo_SubmittedDate]);
                        appItem.TradePromo_SubmittedBy = Convert.ToString(item[ApprovalListFields.TradePromo_SubmittedBy]);
                        // Other Fields
                        appItem.OnHold_ModifiedDate = Convert.ToString(item[ApprovalListFields.OnHold_ModifiedDate]);
                        appItem.OnHold_ModifiedBy = Convert.ToString(item[ApprovalListFields.OnHold_ModifiedBy]);
                        appItem.PreProduction_ModifiedDate = Convert.ToString(item[ApprovalListFields.PreProduction_ModifiedDate]);
                        appItem.PreProduction_ModifiedBy = Convert.ToString(item[ApprovalListFields.PreProduction_ModifiedBy]);
                        appItem.Completed_ModifiedDate = Convert.ToString(item[ApprovalListFields.Completed_ModifiedDate]);
                        appItem.Completed_ModifiedBy = Convert.ToString(item[ApprovalListFields.Completed_ModifiedBy]);
                        appItem.Cancelled_ModifiedDate = Convert.ToString(item[ApprovalListFields.Cancelled_ModifiedDate]);
                        appItem.Cancelled_ModifiedBy = Convert.ToString(item[ApprovalListFields.Cancelled_ModifiedBy]);
                        appItem.ProductionCompleted_ModifiedDate = Convert.ToString(item[ApprovalListFields.ProductionCompleted_ModifiedDate]);
                        appItem.ProductionCompleted_ModifiedBy = Convert.ToString(item[ApprovalListFields.ProductionCompleted_ModifiedBy]);
                    }
                    catch (Exception e)
                    {
                        InsertLog("Project Timeline Job-Error", "UpdateTimelineReportFields", "actualTimeLine 1455: " + e.Message);
                    }
                }
            }
            SPList spList2 = spWeb.Lists.TryGetList(LIST_ApprovalList2Name);

            SPQuery spQuery2 = new SPQuery();
            spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            spQuery2.RowLimit = 1;

            SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
            if (compassItemCol2.Count > 0)
            {
                SPListItem item2 = compassItemCol2[0];

                if (item2 != null)
                {
                    appItem.ProcAncillary_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedDate]);
                    appItem.ProcCorrugated_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedDate]);
                    appItem.ProcPurchased_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedDate]);
                    appItem.ProcFilm_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedDate]);
                    appItem.ProcLabel_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedDate]);
                    appItem.ProcMetal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedDate]);
                    appItem.ProcOther_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedDate]);
                    appItem.ProcPaperboard_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedDate]);
                    appItem.ProcRigidPlastic_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedDate]);
                    appItem.ProcExternal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcExternal_SubmittedDate]);
                    appItem.ProcSeasonal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcSeasonal_SubmittedDate]);
                    appItem.ProcCoMan_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcCoMan_SubmittedDate]);
                    appItem.ProcNovelty_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcNovelty_SubmittedDate]);
                    appItem.ProcAncillary_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedBy]);
                    appItem.ProcCorrugated_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedBy]);
                    appItem.ProcPurchased_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedBy]);
                    appItem.ProcFilm_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedBy]);
                    appItem.ProcLabel_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedBy]);
                    appItem.ProcMetal_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedBy]);
                    appItem.ProcOther_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedBy]);
                    appItem.ProcPaperboard_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedBy]);
                    appItem.ProcRigidPlastic_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedBy]);
                    appItem.ProcExternal_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcExternal_SubmittedBy]);
                    appItem.ProcExternalAncillary_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedDate]);
                    appItem.ProcExternalCorrugated_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedDate]);
                    appItem.ProcExternalPurchased_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedDate]);
                    appItem.ProcExternalFilm_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedDate]);
                    appItem.ProcExternalLabel_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedDate]);
                    appItem.ProcExternalMetal_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedDate]);
                    appItem.ProcExternalOther_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedDate]);
                    appItem.ProcExternalPaperboard_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedDate]);
                    appItem.ProcExternalRigidPlastic_SubmittedDate = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedDate]);
                    appItem.ProcExternalAncillary_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcAncillary_SubmittedBy]);
                    appItem.ProcExternalCorrugated_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcCorrugated_SubmittedBy]);
                    appItem.ProcExternalPurchased_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPurchased_SubmittedBy]);
                    appItem.ProcExternalFilm_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcFilm_SubmittedBy]);
                    appItem.ProcExternalLabel_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcLabel_SubmittedBy]);
                    appItem.ProcExternalMetal_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcMetal_SubmittedBy]);
                    appItem.ProcExternalOther_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcOther_SubmittedBy]);
                    appItem.ProcExternalPaperboard_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcPaperboard_SubmittedBy]);
                    appItem.ProcExternalRigidPlastic_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcRigidPlastic_SubmittedBy]);
                    appItem.ProcSeasonal_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcSeasonal_SubmittedBy]);
                    appItem.ProcCoMan_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcCoMan_SubmittedBy]);
                    appItem.ProcNovelty_SubmittedBy = Convert.ToString(item2[ApprovalListFields.ProcNovelty_SubmittedBy]);
                    //BEQRC
                    appItem.BEQRC_StartDate = Convert.ToString(item2[ApprovalListFields.BEQRC_StartDate]);
                    appItem.BEQRC_ModifiedDate = Convert.ToString(item2[ApprovalListFields.BEQRC_ModifiedDate]);
                    appItem.BEQRC_ModifiedBy = Convert.ToString(item2[ApprovalListFields.BEQRC_ModifiedBy]);
                    appItem.BEQRC_SubmittedDate = Convert.ToString(item2[ApprovalListFields.BEQRC_SubmittedDate]);
                    appItem.BEQRC_SubmittedBy = Convert.ToString(item2[ApprovalListFields.BEQRC_SubmittedBy]);
                    //EstPricing
                    appItem.EstPricing_StartDate = Convert.ToString(item2[ApprovalListFields.EstPricing_StartDate]);
                    appItem.EstPricing_ModifiedDate = Convert.ToString(item2[ApprovalListFields.EstPricing_ModifiedDate]);
                    appItem.EstPricing_ModifiedBy = Convert.ToString(item2[ApprovalListFields.EstPricing_ModifiedBy]);
                   // appItem.EstPricing_SubmittedDate = Convert.ToString(item2[ApprovalListFields.EstPricing_SubmittedDate]);
                    appItem.EstPricing_SubmittedBy = Convert.ToString(item2[ApprovalListFields.EstPricing_SubmittedBy]);
                    //Estimated Bracket Pricing
                    appItem.EstBracketPricing_StartDate = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_StartDate]);
                    appItem.EstBracketPricing_ModifiedDate = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_ModifiedDate]);
                    appItem.EstBracketPricing_ModifiedBy = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_ModifiedBy]);
                  //  appItem.EstBracketPricing_SubmittedDate = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_SubmittedDate]);
                    appItem.EstBracketPricing_SubmittedBy = Convert.ToString(item2[ApprovalListFields.EstBracketPricing_SubmittedBy]);
                }
            }
            // }
            // }
            return appItem;
        }
        public Tuple<List<ProjectStatusReportItem>, List<ProjectStatusReportItem>, int, int, DateTime, List<DateTime>, bool> actualTimeLine(DashboardDetailsItem dashboardDetails, bool pageName)
        {
            string timelineType = dashboardDetails.TimelineType;
            ApprovalListItem approvalItem = GetApprovalItem(dashboardDetails.CompassListItemId);

            List<DateTime> holidaysList = GetHolidays();
            List<TimelineTypeItem> phases = GetPhases();

            List<TimelineTypeItem> tasks = GetWorkflowStepItems(timelineType, dashboardDetails.CompassListItemId);
            List<List<string>> taskCallStart = GetWorkflowTasksStart(dashboardDetails.CompassListItemId);
            List<List<string>> taskCallEnd = GetWorkflowTasksEnd(dashboardDetails.CompassListItemId);
            List<List<string>> updatedTimes = GetProjectItem(dashboardDetails.CompassListItemId);
            Dictionary<string, string> hideRows = hideRow(dashboardDetails.CompassListItemId);

            List<KeyValuePair<int, DateTime>> listOfExceptionList = new List<KeyValuePair<int, DateTime>>();
            List<DateTime> ExceptionEndDays = new List<DateTime>();
            List<List<string>> existingProject = new List<List<string>>();
            List<KeyValuePair<string, string>> completedTasks = new List<KeyValuePair<string, string>>();
            if (pageName)
            {
                existingProject = GetProjectItem(dashboardDetails.CompassListItemId);
                completedTasks = GetCompletedItems(dashboardDetails.CompassListItemId);
            }
            DateTime previousTaskStart = new DateTime();
            int exceptionHolder = 0;
            double dayHolder = 0;
            double leftSpace = 0;
            DateTime projectStart = new DateTime();
            DateTime ipfSubmit = new DateTime();
            bool IPFSubmitted = false;
            if (String.IsNullOrEmpty(approvalItem.IPF_SubmittedDate))
            {
                ipfSubmit = DateTime.Now;
            }
            else
            {
                ipfSubmit = DateTime.Parse(approvalItem.IPF_SubmittedDate, ci);
                IPFSubmitted = true;
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
            DateTime tempFirstShip;
            DateTime firstProdDate;
            DateTime tempFirstProd;
            if (DateTime.TryParse(dashboardDetails.FirstShipDate, out tempFirstShip))
            {
                firstShipDate = DateTime.Parse(dashboardDetails.FirstShipDate);
            }
            if (dashboardDetails.ProjectType.ToLower().Contains("seasonal") && DateTime.TryParse(dashboardDetails.FirstProductionDate, out tempFirstProd))
            {
                firstProdDate = DateTime.Parse(dashboardDetails.FirstProductionDate);
                if (firstProdDate != DateTime.MinValue)
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
            List<ProjectStatusReportItem> dashboardRows = new List<ProjectStatusReportItem>();
            foreach (TimelineTypeItem data in phases)
            {
                List<ProjectStatusReportItem> dashboardPhaseRows = new List<ProjectStatusReportItem>();
                ProjectStatusReportItem dashboardPhaseRowCells = new ProjectStatusReportItem();
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
                    ProjectStatusReportItem dashboardRowCells = new ProjectStatusReportItem();
                    Boolean skipRow = false;
                    dashboardRowCells.Checks = WFException;
                    dashboardRowCells.WorflowQuickStep = taskName;
                    skipRow = hideCurrentRow(WFQuickStep, hideRows);
                    if (WFException == "PROC")
                    {
                        dashboardRowCells.Checks = "PROC";
                    }
                    if (skipRow)
                    {
                        hiddenTaskCounter = hiddenTaskCounter + 1;
                        continue;
                    }

                    dashboardRowCells.WorflowQuickStep = WFQuickStep;

                    if ((/*WFException == "DF" && */timelineInfo.WorkflowOrder == 0) || (WFException == "NE" && timelineInfo.WorkflowOrder == 0))
                    {

                        dashboardRowCells.Process = taskName;
                        dashboardRowCells.ActualStartDay = DateTime.MinValue;
                        dashboardRowCells.ActualEndDay = DateTime.MinValue;
                        dashboardRowCells.Checks = "PackagingItem";
                        dashboardRows.Add(dashboardRowCells);
                        allTaskCounter++;
                        headerRowCounter++;
                        continue;

                    }

                    foreach (KeyValuePair<string, string> task in urlPath.Where(r => r.Key == WFQuickStep))
                    {
                        HyperLink taskForm = new HyperLink();
                        string extraParams = "";
                        if (WFQuickStep == "CostingQuote")
                        {
                            extraParams = "&PackagingItemId=" + timelineInfo.Expedited;
                        }
                        else if (WFQuickStep == "SAPRoutingSetup")
                        {
                            extraParams = "&" + QUERYSTRING_SAPTask + "=SAPROUTINGSETUP";
                        }
                        else if (WFQuickStep == "SAPCostingDetails")
                        { extraParams = "&" + QUERYSTRING_SAPTask + "=SAPCOSTINGDETAILS"; }
                        else if (WFQuickStep == "SAPWarehouseInfo")
                        { extraParams = "&" + QUERYSTRING_SAPTask + "=SAPWAREHOUSEINFO"; }
                        else if (WFQuickStep == "StandardCostEntry")
                        { extraParams = "&" + QUERYSTRING_SAPTask + "=STANDARDCOSTENTRY"; }
                        else if (WFQuickStep == "CostFinishedGood")
                        { extraParams = "&" + QUERYSTRING_SAPTask + "=COSTFINISHEDGOOD"; }
                        else if (WFQuickStep == "FinalCostingReview")
                        { extraParams = "&" + QUERYSTRING_SAPTask + "=FINALCOSTINGREVIEW"; }
                        else if (WFQuickStep == "RemoveSAPBlocks")
                        { extraParams = "&" + QUERYSTRING_SAPTask + "=REMOVESAPBLOCKS"; }

                        taskForm.NavigateUrl = "/Pages/" + task.Value + ".aspx?ProjectNo=" + dashboardDetails.ProjectNumber + extraParams;

                        //365749: PMT Enhancement: Update OBM to PM
                        if (!string.IsNullOrWhiteSpace(taskName))
                        {
                            taskName = taskName.Replace("OBM", "PM");
                            taskName = taskName == "Operations Form" ? "Operations & Capacity Review Form" : taskName;
                            taskName = taskName == "Sr. PM Initial Review Form" ? "PM Initial Review Form" : taskName;
                            taskName = taskName == "PM 2nd Review Point" ? "PM 2nd Review Form" : taskName;
                        }

                        taskForm.Text = taskName;
                        StringBuilder myStringBuilder = new StringBuilder();
                        TextWriter myTextWriter = new StringWriter(myStringBuilder);
                        HtmlTextWriter myWriter = new HtmlTextWriter(myTextWriter);
                        taskForm.RenderControl(myWriter);
                        taskName = myTextWriter.ToString();
                    }
                    dashboardRowCells.Process = taskName;
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
                    if (WFQuickStep == "SrOBMApproval" || WFQuickStep == "SrOBMApproval2")
                    {

                    }
                    if (data.PhaseNumber == 1)//For IPF Phase
                    {
                        taskStart = newIPFStart;
                        List<string> approvalDecisions = (from IPFStatus in hideRows where IPFStatus.Key == "SrOBMApproval" || IPFStatus.Key == "SrOBMApproval2" select IPFStatus.Value).ToList();
                        if (approvalDecisions.Contains("request ipf update") && dashboardDetails.WorkflowPhase.ToLower() == "ipf phase")
                        {
                            status = "Active";
                            phaseStatus = "Active";
                            phaseColor = "phaseGreen";
                            color = "green";
                        }
                        else if (String.IsNullOrEmpty(approvalItem.IPF_SubmittedDate))
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

                                    updatedStart = true;
                                }
                                catch (Exception exception)
                                {
                                    dayHolder = 0;
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = " + dayHolder + "; BP = 1; CompassListItemId = " + dashboardDetails.CompassListItemId;
                                    //exceptionService.Handle(LogCategory.CriticalError, exception, PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
                                }
                            }
                        }
                        if (timelineInfo.WorkflowMisc != null && timelineInfo.WorkflowMisc.Split('.').Length == 2 && !updatedStart)
                        {
                            try
                            {
                                int phase = Convert.ToInt32(timelineInfo.WorkflowMisc.Split('.')[0]);
                                int order = Convert.ToInt32(timelineInfo.WorkflowMisc.Split('.')[1]);
                                TimelineTypeItem rowDetails = (from timelineRows in tasks where timelineRows.PhaseNumber == phase && timelineRows.WorkflowOrder == order select timelineRows).FirstOrDefault();
                                ProjectStatusReportItem precedingItem = (from rows in dashboardRows where rows.WorflowQuickStep == rowDetails.WorkflowQuickStep select rows).FirstOrDefault();
                                taskStart = precedingItem.ActualEndDay;
                                dayHolder = dayHolder + timelineInfo.Holder;
                            }
                            catch (Exception e)
                            {
                                //exceptionService.Handle(LogCategory.CriticalError, e, GlobalConstants.PAGE_NAME_ProjectStatusDashboard, "Page_Load", "error in reordering start");
                            }
                        }
                        else if (!updatedStart)
                        {
                            taskStart = previousTaskComplete;
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

                    }
                    if (taskStart.DayOfWeek == 0)
                    {
                        taskStart = taskStart.AddDays(1);
                        dayHolder = dayHolder + 1;
                        addedDay = addedDay + 1;
                    }
                    else if ((int)taskStart.DayOfWeek == 6)
                    {
                        taskStart = taskStart.AddDays(2);
                        dayHolder = dayHolder + 2;
                        addedDay = addedDay + 2;
                    }
                    if (timelineInfo.WorkflowMisc == null || timelineInfo.WorkflowMisc.Split('.').Length < 2)
                    {
                        previousTaskStart = taskStart;
                    }
                    dashboardRowCells.ActualStartDay = taskStart;

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
                                    }
                                }
                                catch (Exception exception)
                                {
                                    dayHolder = 0;
                                    status = "Calculation Error";
                                    updatedStart = true;
                                    color = "blue";
                                    phaseStatus = "Completed";
                                    phaseColor = "phaseBlue";
                                    taskEndConfirm = true;
                                    statusCheck = true;
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = " + dayHolder + "; BP = 2; CompassListItemId = " + dashboardDetails.CompassListItemId;
                                    //exceptionService.Handle(LogCategory.CriticalError, exception, PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
                                }
                            }
                        }
                        else
                        {
                            foreach (List<string> newTimes in taskCallEnd.Where(s => s[0] == WFQuickStep && s[2] == timelineInfo.WorkflowMisc)) //Get Compass Approval Submitted Dates
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
                                    }
                                }
                                catch (Exception exception)
                                {

                                    dayHolder = 0;
                                    status = "Calculation Error";
                                    updatedStart = true;
                                    color = "blue";
                                    phaseStatus = "Completed";
                                    phaseColor = "phaseBlue";
                                    taskEndConfirm = true;
                                    statusCheck = true;
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = " + dayHolder + "; BP = 3; CompassListItemId = " + dashboardDetails.CompassListItemId;
                                    //exceptionService.Handle(LogCategory.CriticalError, exception, PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
                                }
                            }
                        }

                    }
                    Boolean lateTask = false;
                    if (!taskEndConfirm)
                    {
                        foreach (List<string> updatedDays in updatedTimes.Where(t => t[0] == WFQuickStep))
                        {

                            if (!string.IsNullOrEmpty(updatedDays[1]))
                            {
                                try
                                {
                                    double addWeekendsToUpdate = addWeekends(taskStart, Convert.ToInt32(updatedDays[1]));
                                    taskEnd = taskStart.AddDays(addWeekendsToUpdate);
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
                                    }
                                    else if (compareEnd >= 0 && !updatedStart)
                                    {
                                        taskEndConfirm = true;
                                        dayHolder = dayHolder + diff.Days - timelineInfo.Holder - addedDay;
                                    }
                                    else
                                    {
                                        taskEndConfirm = true;
                                        dayHolder = dayHolder + diff.Days - timelineInfo.Holder - addedDay;
                                    }
                                }
                                catch (Exception exception)
                                {
                                    updatedStart = true;
                                    color = "blue";
                                    phaseStatus = "Completed";
                                    phaseColor = "phaseBlue";
                                    taskEndConfirm = true;
                                    statusCheck = true;
                                    dayHolder = 0;
                                    status = "Calculation Error";
                                    string errorDets = "WFName = " + timelineInfo.WorkflowStep + "; DayHolder = " + dayHolder + "; BP = 4; CompassListItemId = " + dashboardDetails.CompassListItemId;
                                    //exceptionService.Handle(LogCategory.CriticalError, exception, PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
                                }
                            }
                        }
                    }
                    if (!taskEndConfirm && !updatedStart && WFQuickStep != "Notification")
                    {
                        taskEnd = taskStart.AddDays(timelineInfo.Holder);
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
                            status = "Active - Late";
                            color = "red";
                            phaseColor = "phaseRed";
                            phaseStatus = "Active - Late";
                            statusCheck = true;
                            lateTask = true;
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

                    }
                    if (taskEnd.DayOfWeek == 0)
                    {
                        taskEnd = taskEnd.AddDays(1);
                    }
                    else if ((int)taskEnd.DayOfWeek == 6)
                    {
                        taskEnd = taskEnd.AddDays(2);
                    }

                    if (WFQuickStep != "Notification")// && WFQuickStep != "DF")
                    {
                        previousTaskStart = taskEnd;
                    }
                    if (hasException)
                    {
                        stackedEndList.Add(new KeyValuePair<int, DateTime>(timelineInfo.WorkflowStacks, taskEnd));
                        int comparePrevious = DateTime.Compare(taskEnd, previousTaskComplete);
                        if (comparePrevious > 0)// || WFException == "DF")//|| timelineInfo.WorkflowExceptions == "NE" || WFQuickStep == "SAPRoutingSetup")
                        {
                            previousTaskComplete = (from ends in stackedEndList where ends.Key == timelineInfo.WorkflowStacks select ends.Value).OrderByDescending(r => r).FirstOrDefault();
                        }
                        else if (WFException == "DF")
                        {
                            previousTaskComplete = taskEnd;
                        }
                    }
                    else
                    {
                        previousTaskComplete = taskEnd;
                    }
                    int compareStartEnd = DateTime.Compare(taskStart, taskEnd);
                    if (compareStartEnd > 0)
                    {
                        dashboardRowCells.ActualStartDay = taskEnd;
                        taskStart = taskEnd;
                    }
                    if (!statusCheck && phaseStatus == "Completed")
                    {
                        phaseStatus = "Waiting";
                        phaseColor = "phaseGrey";
                    }
                    phaseDayHolder.Add(taskEnd);
                    dashboardRowCells.ActualEndDay = taskEnd;
                    if (currentStack != 0)
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

                    dashboardRowCells.ActualDuration = taskDuration;
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

                    if (WFQuickStep == "SrOBMApproval")// || WFQuickStep == "SrOBMApproval2")
                    {
                        List<string> approvalDecisions = (from IPFStatus in hideRows where IPFStatus.Key == WFQuickStep select IPFStatus.Value.ToLower()).ToList();
                        if (approvalDecisions.Contains("request ipf update") && dashboardDetails.WorkflowPhase != "Sr. OBM Review Phase")
                        {
                            status = "Request IPF Update";
                        }
                    }
                    dashboardRowCells.Status = status;
                    //End Status Section
                    //Begin calendar Display
                    leftSpace = getWidth(todayFields, taskStart, holidaysList, false, true);
                    double taskWidth = getWidth(taskStart, taskEnd, holidaysList, true, true);
                    //double recalcWidth2 = (Math.Floor(taskWidth / 4) * 20) + ((taskWidth % 4) * 5);
                    //taskWidth = recalcWidth2;
                    if (pageName)
                    {
                        string passingArg = "";
                        if (WFQuickStep != "" && WFQuickStep != "Notification" && WFException != "DF")
                        {
                            if (!dashboardDetails.ExistingItem)
                            {
                                dashboardRowCells.ActualDuration = tempTaskDuration;

                                if (status == "Completed")
                                {
                                    dashboardRowCells.Checks = "true";
                                }
                                else
                                {
                                    dashboardRowCells.Checks = "false";
                                }
                            }
                            else
                            {
                                Boolean wasAdded = false;
                                foreach (List<string> task in existingProject.Where(r => r[0] == WFQuickStep))
                                {
                                    if (status == "Completed")
                                    {
                                        dashboardRowCells.Checks = "true";
                                        wasAdded = true;
                                    }
                                    else
                                    {
                                        dashboardRowCells.Checks = "false";
                                        wasAdded = true;
                                    }
                                }
                                if (!wasAdded)
                                {
                                    dashboardRowCells.Checks = "false";
                                }
                            }
                        }
                    }
                    else
                    {

                        dashboardRowCells.PixelsFromLeft = leftSpace;
                        dashboardRowCells.Width = taskWidth;
                    }
                    leftSpace = leftSpace + (taskWidth);
                    //End calendar Display
                    dashboardRowCells.Color = color;
                    //Start email section
                    if (status.ToLower() != "waiting" && WFQuickStep != "IPF")
                    {
                        HyperLink emailLink = new HyperLink();
                        string pageNameForEmail = (from pageURL in urlPath where pageURL.Key == WFQuickStep select pageURL.Value).FirstOrDefault();
                        string tempWFQuickStep = WFQuickStep;
                        if (tempWFQuickStep == "GRAPHICS")
                        {
                            tempWFQuickStep = "Graphics";
                        }
                        else if (tempWFQuickStep == "SrOBMApproval")
                        {
                            if (dashboardDetails.ProjectType.ToLower().Contains("seasonal"))
                            {
                                tempWFQuickStep = "SrOBMApprovalSeasonal";
                            }
                            else if (dashboardDetails.ProjectType.ToLower().Contains("bulk") || dashboardDetails.ProjectType.ToLower().Contains("co-man"))
                            {
                                tempWFQuickStep = "SrOBMApprovalBulkCoMan";
                            }
                            else if (dashboardDetails.ProjectType.ToLower().Contains("everyday") || dashboardDetails.ProjectType.ToLower().Contains("private"))
                            {
                                tempWFQuickStep = "SrOBMApprovalEveryday";
                            }
                        }
                        emailLink.Attributes.Add("onclick", "sendWFEmail(this,'" + tempWFQuickStep + "','" + pageNameForEmail + "','" + dashboardDetails.CompassListItemId + "','" + dashboardDetails.ProjectNumber + "')");
                        emailLink.NavigateUrl = "javascript:void(0)";
                        emailLink.Text = "Re-Send Email";
                        StringBuilder myStringBuilder = new StringBuilder();
                        TextWriter myTextWriter = new StringWriter(myStringBuilder);
                        HtmlTextWriter myWriter = new HtmlTextWriter(myTextWriter);
                        emailLink.RenderControl(myWriter);
                        string emailHTML = myTextWriter.ToString();
                        dashboardRowCells.Email = emailHTML;

                    }
                    //SubmittedBy Section
                    if (approvalItem.GetType().GetProperty(WFQuickStep + "_SubmittedBy") != null)
                    {
                        string approvedBy = Convert.ToString(approvalItem.GetType().GetProperty(WFQuickStep + "_SubmittedBy").GetValue(approvalItem, null));
                        dashboardRowCells.SubmittedBy = approvedBy;
                    }
                    else
                    {
                        dashboardRowCells.SubmittedBy = "";
                    }
                    //End Email section
                    dashboardRows.Add(dashboardRowCells);

                    if (counter == 0)
                    {
                        phaseLeft = leftSpace - (taskWidth);
                        phaseStart = taskStart;
                    }
                    counter++;
                    allTaskCounter++;

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
                    dashboardPhaseRowCells.Process = data.WorkflowStep;
                    dashboardPhaseRowCells.Status = phaseStatus;
                    dashboardPhaseRowCells.ActualStartDay = phaseStart;
                    dashboardPhaseRowCells.ActualEndDay = phaseEnd;
                    dashboardPhaseRowCells.ActualDuration = phaseDuration;
                    dashboardPhaseRowCells.PixelsFromLeft = phaseLeft;
                    dashboardPhaseRowCells.Width = phaseWidth;
                    dashboardPhaseRowCells.Color = phaseColor;
                    dashboardPhaseRowCells.Checks = "Phase";
                    dashboardRows.Insert(phasePosition, dashboardPhaseRowCells);
                }
                else if (data.PhaseNumber == 6)
                {
                    TimeSpan ProjectDaysTotal = projectEnd - projectStart;
                    int ProjectDaysWeekends = Convert.ToInt32(totalDaysCalc(projectStart, projectEnd, holidaysList));
                    int totalProjectDays = ProjectDaysTotal.Days - ProjectDaysWeekends;
                    projectDuration = Math.Round(projectDuration, 0);

                    TimeSpan expectedDaysTotal = firstShipDate - ipfSubmit;
                    int expectedDaysWeekends = Convert.ToInt32(totalDaysCalc(ipfSubmit, firstShipDate, holidaysList));
                    int totalExpectedDays = expectedDaysTotal.Days - expectedDaysWeekends;

                    int floatDays = totalExpectedDays - totalProjectDays;
                    //firstMath = firstMath - removeDaysFloat;
                    dashboardPhaseRowCells.Process = data.WorkflowStep;

                    if (firstShipDate != DateTime.MinValue && ipfSubmit != DateTime.MinValue)
                    {
                        dashboardPhaseRowCells.Color = totalExpectedDays.ToString() + ";" + totalProjectDays.ToString();
                        dashboardPhaseRowCells.ActualDuration = floatDays;
                    }
                    else
                    {
                        dashboardPhaseRowCells.Color = "-1" + ";" + totalProjectDays.ToString();
                    }
                    dashboardPhaseRowCells.WorflowQuickStep = data.WorkflowQuickStep;
                    dashboardPhaseRowCells.Checks = "Phase";
                    dashboardRows.Add(dashboardPhaseRowCells);

                }
            }
            List<ProjectStatusReportItem> OGTimeline = originalTimeLine(firstShipDate, ipfSubmit, newIPFStart, phases, tasks, hideRows, holidaysList);
            var returnItem = Tuple.Create(dashboardRows, OGTimeline, allTaskCounter, phaseCounter, todayFields, holidaysList, IPFSubmitted);
            return returnItem;
        }
        public static List<KeyValuePair<string, string>> urlList()
        {
            List<KeyValuePair<string, string>> urlList = new List<KeyValuePair<string, string>>();
            urlList.Add(new KeyValuePair<string, string>("IPF", "ItemProposal"));
            urlList.Add(new KeyValuePair<string, string>("SrOBMApproval", "InitialApprovalReview"));
            urlList.Add(new KeyValuePair<string, string>("TradePromo", "TradePromoGroup"));
            urlList.Add(new KeyValuePair<string, string>("EstPricing", "EstPricing"));
            urlList.Add(new KeyValuePair<string, string>("EstBracketPricing", "EstBracketPricing"));
            urlList.Add(new KeyValuePair<string, string>("Distribution", "Distribution"));
            urlList.Add(new KeyValuePair<string, string>("Operations", "OPS"));
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
            urlList.Add(new KeyValuePair<string, string>("BOMSetupPE3", "BOMSetupPE3"));
            urlList.Add(new KeyValuePair<string, string>("SrOBMApproval2", "SecondaryApprovalReview"));
            urlList.Add(new KeyValuePair<string, string>("OBMReview2", "OBMSecondReview"));
            urlList.Add(new KeyValuePair<string, string>("FGPackSpec", "FinishedGoodPackSpec"));
            urlList.Add(new KeyValuePair<string, string>("GRAPHICS", "GraphicsRequest"));
            urlList.Add(new KeyValuePair<string, string>("CostingQuote", "ComponentCosting"));
            urlList.Add(new KeyValuePair<string, string>("SAPRoutingSetup", "FinalRoutingsSummary"));
            urlList.Add(new KeyValuePair<string, string>("SAPCostingDetails", "FinalRoutingsSummary"));
            urlList.Add(new KeyValuePair<string, string>("SAPWarehouseInfo", "FinalRoutingsSummary"));
            urlList.Add(new KeyValuePair<string, string>("StandardCostEntry", "FinalRoutingsSummary"));
            urlList.Add(new KeyValuePair<string, string>("CostFinishedGood", "FinalRoutingsSummary"));
            urlList.Add(new KeyValuePair<string, string>("MatrlWHSetUp", "BOMSetupMaterialWarehouse"));
            urlList.Add(new KeyValuePair<string, string>("SAPCompleteItem", "SAPCompleteItemSetup"));
            urlList.Add(new KeyValuePair<string, string>("BEQRC", "BEQRC"));
            return urlList;
        }
        public List<ProjectStatusReportItem> originalTimeLine(DateTime firstShipDate, DateTime ipfSubmit, DateTime newIPFStart, List<TimelineTypeItem> phases, List<TimelineTypeItem> tasks, Dictionary<string, string> hideRows, List<DateTime> holidaysList)
        {
            Double dayHolder = 0;
            DateTime previousTaskStart = new DateTime();
            int exceptionHolder = 0;
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
            DateTime projectStart = new DateTime();
            List<KeyValuePair<int, DateTime>> listOfExceptionList = new List<KeyValuePair<int, DateTime>>();
            List<DateTime> ExceptionEndDays = new List<DateTime>();
            List<ProjectStatusReportItem> dashboardRows = new List<ProjectStatusReportItem>();
            foreach (TimelineTypeItem data in phases)
            {

                List<ProjectStatusReportItem> dashboardPhaseRows = new List<ProjectStatusReportItem>();
                ProjectStatusReportItem dashboardPhaseRowCells = new ProjectStatusReportItem();
                List<KeyValuePair<int, double>> stackedList = new List<KeyValuePair<int, double>>();
                List<KeyValuePair<int, DateTime>> stackedEndList = new List<KeyValuePair<int, DateTime>>();
                string phaseName = data.WorkflowStep;
                DateTime phaseStart = new DateTime();
                DateTime phaseEnd = new DateTime();

                double phaseDuration = 0;
                List<DateTime> phaseDayHolder = new List<DateTime>();
                int counter = 0;
                int taskCount = 0;
                int exceptionDateHolder = 0;
                double dayHolderHolder = 0;
                Boolean addDuration = false;
                int hiddenTaskCounter = 0;
                foreach (TimelineTypeItem timelineInfo in tasks.Where(r => r.PhaseNumber == data.PhaseNumber))
                {
                    double dayHolderDisplay = dayHolder;
                    string WFQuickStep = timelineInfo.WorkflowQuickStep;
                    string WFException = timelineInfo.WorkflowExceptions;
                    string taskName = "";
                    ProjectStatusReportItem dashboardRowCells = new ProjectStatusReportItem();
                    dashboardRowCells.WorflowQuickStep = WFQuickStep;
                    dashboardRowCells.Checks = WFException;
                    taskName = timelineInfo.WorkflowStep;
                    bool skipRow = false;
                    skipRow = hideCurrentRow(WFQuickStep, hideRows);
                    if (WFException == "PROC")
                    {
                        dashboardRowCells.Checks = "PROC";
                    }

                    if (skipRow)
                    {
                        hiddenTaskCounter = hiddenTaskCounter + 1;
                        continue;
                    }


                    Boolean statusCheck = false;
                    Boolean updatedStart = false;
                    TimeSpan time = new TimeSpan();
                    DateTime taskEnd = new DateTime();

                    Boolean hasException = false;
                    //Begin Start Task Day Section
                    DateTime taskStart = new DateTime();
                    int addedDay = 0;
                    double maxHolder = 0;
                    int currentStack = timelineInfo.WorkflowStacks;
                    if (data.PhaseNumber == 1)//For IPF Phase
                    {
                        taskStart = newIPFStart;
                        statusCheck = true;
                        taskEnd = newIPFStart;
                    }
                    else
                    {
                        if (timelineInfo.WorkflowMisc != null && timelineInfo.WorkflowMisc.Split('.').Length == 2)
                        {
                            try
                            {
                                int phase = Convert.ToInt32(timelineInfo.WorkflowMisc.Split('.')[0]);
                                int order = Convert.ToInt32(timelineInfo.WorkflowMisc.Split('.')[1]);
                                TimelineTypeItem rowDetails = (from timelineRows in tasks where timelineRows.PhaseNumber == phase && timelineRows.WorkflowOrder == order select timelineRows).FirstOrDefault();
                                ProjectStatusReportItem precedingItem = (from rows in dashboardRows where rows.WorflowQuickStep == rowDetails.WorkflowQuickStep select rows).FirstOrDefault();
                                taskStart = precedingItem.OGEndDay;
                                dayHolder = dayHolder + timelineInfo.Holder;
                            }
                            catch (Exception e)
                            {
                                //exceptionService.Handle(LogCategory.CriticalError, e, GlobalConstants.PAGE_NAME_ProjectStatusDashboard, "Page_Load", "error in reordering start");
                            }
                        }
                        else
                        {
                            taskStart = previousTaskComplete;
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

                    }
                    if (taskStart.DayOfWeek == 0)
                    {
                        taskStart = taskStart.AddDays(1);
                        dayHolder = dayHolder + 1;
                        addedDay = addedDay + 1;
                    }
                    else if ((int)taskStart.DayOfWeek == 6)
                    {
                        taskStart = taskStart.AddDays(2);
                        dayHolder = dayHolder + 2;
                        addedDay = addedDay + 2;
                    }
                    if (timelineInfo.WorkflowMisc == null || timelineInfo.WorkflowMisc.Split('.').Length < 2)
                    {
                        previousTaskStart = taskStart;
                    }
                    dashboardRowCells.OGStartDay = taskStart;

                    //End Start Task Day Section

                    //Begin End Task Day Section

                    taskEnd = taskStart.AddDays(timelineInfo.Holder);

                    weekendCount = weekends(taskStart, taskEnd, holidaysList, false);
                    dayHolder = dayHolder + weekendCount;
                    taskEnd = taskEnd.AddDays(weekendCount);
                    int result = DateTime.Compare(taskEnd, rightNow);
                    if (result < 0 && !statusCheck && updatedStart)
                    {
                        TimeSpan endSpan = rightNow - taskEnd;
                        double quickdays = weekends(taskEnd, rightNow, holidaysList, false);
                        double endDays = endSpan.Days - quickdays;
                        dayHolder = dayHolder + endDays;
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

                    }
                    if (taskEnd.DayOfWeek == 0)
                    {
                        taskEnd = taskEnd.AddDays(1);
                    }
                    else if ((int)taskEnd.DayOfWeek == 6)
                    {
                        taskEnd = taskEnd.AddDays(2);
                    }

                    if (WFQuickStep != "Notification")// && WFQuickStep != "DF")
                    {
                        previousTaskStart = taskEnd;
                    }
                    if (hasException)
                    {
                        stackedEndList.Add(new KeyValuePair<int, DateTime>(timelineInfo.WorkflowStacks, taskEnd));
                        int comparePrevious = DateTime.Compare(taskEnd, previousTaskComplete);
                        if (comparePrevious > 0)// || WFException == "DF")//|| timelineInfo.WorkflowExceptions == "NE" || WFQuickStep == "SAPRoutingSetup")
                        {
                            previousTaskComplete = (from ends in stackedEndList where ends.Key == timelineInfo.WorkflowStacks select ends.Value).OrderByDescending(r => r).FirstOrDefault();
                        }
                        else if (WFException == "DF")
                        {
                            previousTaskComplete = taskEnd;
                        }
                    }
                    else
                    {
                        previousTaskComplete = taskEnd;
                    }
                    int compareStartEnd = DateTime.Compare(taskStart, taskEnd);
                    if (compareStartEnd > 0)
                    {
                        dashboardRowCells.OGStartDay = taskEnd;
                        taskStart = taskEnd;
                    }
                    phaseDayHolder.Add(taskEnd);
                    dashboardRowCells.OGEndDay = taskEnd;
                    if (currentStack != 0)
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

                    dashboardRowCells.OGDuration = taskDuration;
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
                            taskDuration = 0;

                        }
                        else
                        {
                            previousTaskStart = ExceptionEndDays.OrderByDescending(x => x).FirstOrDefault();
                            addDuration = false;
                            taskDuration = maxHolder;
                        }
                    }

                    //End Total Days Section
                    dashboardRows.Add(dashboardRowCells);

                    if (counter == 0)
                    {
                        phaseStart = taskStart;
                    }
                    counter++;
                    allTaskCounter++;

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
                dashboardPhaseRowCells.WorflowQuickStep = data.WorkflowQuickStep;
                if (data.PhaseNumber != 6)
                {

                    dashboardPhaseRowCells.OGStartDay = phaseStart;
                    dashboardPhaseRowCells.OGEndDay = phaseEnd;
                    dashboardPhaseRowCells.OGDuration = phaseDuration;
                    dashboardRows.Insert(phasePosition, dashboardPhaseRowCells);
                }
                else if (data.PhaseNumber == 6)
                {
                    TimeSpan ProjectDaysTotal = projectEnd - projectStart;
                    int ProjectDaysWeekends = Convert.ToInt32(totalDaysCalc(projectStart, projectEnd, holidaysList));
                    int totalProjectDays = ProjectDaysTotal.Days - ProjectDaysWeekends;
                    projectDuration = Math.Round(projectDuration, 0);

                    TimeSpan expectedDaysTotal = firstShipDate - ipfSubmit;
                    int expectedDaysWeekends = Convert.ToInt32(totalDaysCalc(ipfSubmit, firstShipDate, holidaysList));
                    int totalExpectedDays = expectedDaysTotal.Days - expectedDaysWeekends;

                    double floatDays = totalExpectedDays - totalProjectDays;
                    if (firstShipDate != DateTime.MinValue && ipfSubmit != DateTime.MinValue)
                    {
                        //dashboardPhaseRowCells.OGStartDay = new DateTime(totalExpectedDays);
                        //dashboardPhaseRowCells.OGEndDay = new DateTime(totalProjectDays);
                        dashboardPhaseRowCells.Color = totalExpectedDays.ToString() + ";" + totalProjectDays.ToString();
                        dashboardPhaseRowCells.OGDuration = floatDays;
                    }
                    else
                    {
                        dashboardPhaseRowCells.Color = "-1" + ";" + totalProjectDays.ToString();
                    }
                    dashboardPhaseRowCells.WorflowQuickStep = data.WorkflowQuickStep;
                    dashboardPhaseRowCells.Checks = "Phase";
                    dashboardRows.Add(dashboardPhaseRowCells);
                    /*
                    if (firstShipDate != DateTime.MinValue && ipfSubmit != DateTime.MinValue)
                    {
                        dashboardPhaseRowCells.Add(totalExpectedDays.ToString());
                        dashboardPhaseRowCells.Add(totalProjectDays.ToString());
                        dashboardPhaseRowCells.Add(floatDays.ToString());
                    }
                    else
                    {
                        dashboardPhaseRowCells.Add("");
                        dashboardPhaseRowCells.Add(totalProjectDays.ToString());
                        dashboardPhaseRowCells.Add("");
                    }*/



                }
            }
            return dashboardRows;
        }
        public List<TimelineTypeItem> copyItem(SPListItemCollection compassItemCol, string timelineType)
        {
            List<TimelineTypeItem> DFItems = new List<TimelineTypeItem>();
            foreach (SPListItem item in compassItemCol)
            {
                if (item != null)
                {
                    TimelineTypeItem obTimelineTypeItem = new TimelineTypeItem();

                    string WorkflowQuickStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowQuickStep]);
                    string WorkflowStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowStep]);
                    obTimelineTypeItem.WorkflowStep = WorkflowStep;
                    obTimelineTypeItem.WorkflowQuickStep = WorkflowQuickStep;
                    obTimelineTypeItem.PhaseNumber = Convert.ToInt32(item[ProjectTimelineTypeDays.PhaseNumber]);
                    obTimelineTypeItem.WorkflowOrder = Convert.ToInt32(item[ProjectTimelineTypeDays.WorkflowOrder]);
                    obTimelineTypeItem.WorkflowExceptions = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowExceptions]);
                    obTimelineTypeItem.WorkflowStacks = Convert.ToInt32(item[ProjectTimelineTypeDays.WorkflowStacks]);

                    if (timelineType.ToLower() == "standard")
                    {
                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Standard]);
                    }
                    else if (timelineType.ToLower() == "expedited")
                    {
                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Expedited]);
                    }
                    else if (timelineType.ToLower() == "ludicrous")
                    {
                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Ludicrous]);
                    }
                    DFItems.Add(obTimelineTypeItem);
                }
            }
            return DFItems;
        }
        public Boolean hideCurrentRow(string taskQuickName, Dictionary<string, string> hideRows)
        {
            if (string.IsNullOrEmpty(taskQuickName))
            {
                return false;
            }
            Boolean hideThisRow = false;
            List<string> qualifiers = new List<string>(new string[]
               {
                    "Distribution",
                    "ExternalMfg",
                    "SAPInitialSetup",
                    "PrelimSAPInitialSetup",
                    "TradePromo",
                    "EstPricing",
                    "EstBracketPricing",
                    "OBMReview2",
                    "BOMSetupProc",
                    "BOMSetupPE",
                    "BOMSetupPE3",
                    "GRAPHICS",
                    "QA",
                    "MatrlWHSetUp",
                    "SAPCompleteItem",
                    "FGPackSpec",
                    "BEQRC"
               }
            );

            if (qualifiers.Contains(taskQuickName) || taskQuickName.Contains("Proc"))
            {
                foreach (KeyValuePair<string, string> taskCheck in hideRows.Where(r => r.Key == taskQuickName))
                {
                    if (taskCheck.Value == "Y")
                    {
                        hideThisRow = true;
                        break;
                    }
                    else
                    {
                        hideThisRow = false;
                        break;
                    }
                }
            }
            return hideThisRow;
        }
        public double totalDaysCalc(DateTime start, DateTime end, List<DateTime> holidays)
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
        public double weekends(DateTime start, DateTime end, List<DateTime> holidays, Boolean leftSpace)
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

            }

            return days;
        }
        public int exclDays(DateTime start, DateTime end)
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
        public int exclHolidays(DateTime start, DateTime end, List<DateTime> holidays)
        {
            int exclusionHolidays = 0;

            foreach (DateTime holiday in holidays.Where(r => r.Date >= start.Date && r.Date <= end.Date))
            {
                exclusionHolidays++;
            }

            return exclusionHolidays;
        }
        public double getWidth(DateTime start, DateTime end, List<DateTime> weekendList, Boolean change, Boolean leftSpace)
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
                    if (leftSpace)
                    {
                        width = width + 0;
                    }
                    else
                    {
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
                if (startTime < TimeSpan.Parse("08:00:00"))
                {
                    TimeSpan ts = new TimeSpan(8, 0, 0);
                    start = start.Date + ts;
                }
                if (endTime > TimeSpan.Parse("16:00:00"))
                {
                    TimeSpan ets = new TimeSpan(15, 59, 0);
                    end = end.Date + ets;
                }
                TimeSpan newTime = end - start;
                double hourDuration = Math.Round(newTime.TotalHours / 2, 0);
                if (width <= 0 && change)
                {
                    width = 1;
                }
                else if (width < 0 && !change)
                {
                    width = 0;
                }
            }
            double tempwidth = (Math.Floor(width / 4) * 21) + ((width % 4) * 5);
            return tempwidth;
        }
        public static double addWeekends(DateTime startDate, int Days)
        {
            double includingWeekends = 0;
            DateTime tmpDate = startDate;
            while (Days > 0)
            {
                includingWeekends++;
                tmpDate = tmpDate.AddDays(1);
                if (tmpDate.DayOfWeek < DayOfWeek.Saturday && tmpDate.DayOfWeek > DayOfWeek.Sunday)
                    Days--;
            }
            return includingWeekends;
        }
        public List<KeyValuePair<string, string>> GetCompletedItems(int compassListId)
        {
            List<KeyValuePair<string, string>> updatedItems = new List<KeyValuePair<string, string>>();
            /*using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {*/
            SPList spList = spWeb.Lists.TryGetList(LIST_ApprovalListName);

            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
            spQuery.RowLimit = 1;


            string compCostSeasonal = "";
            string compCostCorrPaper = "";
            string compCostFLRP = "";
            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
            if (compassItemCol.Count > 0)
            {
                SPListItem appItem = compassItemCol[0];
                if (appItem != null)
                {
                    updatedItems.Add(new KeyValuePair<string, string>("IPF", Convert.ToString(appItem[ApprovalListFields.IPF_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("BOMSetupPE3", Convert.ToString(appItem[ApprovalListFields.BOMSetupPE3_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("BOMSetupPE2", Convert.ToString(appItem[ApprovalListFields.BOMSetupPE2_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("BOMSetupPE", Convert.ToString(appItem[ApprovalListFields.BOMSetupPE_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("BOMSetupProc", Convert.ToString(appItem[ApprovalListFields.BOMSetupProc_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("Distribution", Convert.ToString(appItem[ApprovalListFields.Distribution_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("ExternalMfg", Convert.ToString(appItem[ApprovalListFields.ExternalMfg_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("FGPackSpec", Convert.ToString(appItem[ApprovalListFields.FGPackSpec_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("GRAPHICS", Convert.ToString(appItem[ApprovalListFields.GRAPHICS_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("InitialCosting", Convert.ToString(appItem[ApprovalListFields.InitialCosting_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("OBMReview1", Convert.ToString(appItem[ApprovalListFields.OBMReview1_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("OBMReview2", Convert.ToString(appItem[ApprovalListFields.OBMReview2_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("Operations", Convert.ToString(appItem[ApprovalListFields.Operations_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("QA", Convert.ToString(appItem[ApprovalListFields.QA_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SAPBOMSetup", Convert.ToString(appItem[ApprovalListFields.SAPBOMSetup_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SAPInitialSetup", Convert.ToString(appItem[ApprovalListFields.SAPInitialSetup_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("PrelimSAPInitialSetup", Convert.ToString(appItem[ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SrOBMApproval2", Convert.ToString(appItem[ApprovalListFields.SrOBMApproval2_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SrOBMApproval", Convert.ToString(appItem[ApprovalListFields.SrOBMApproval_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("TradePromo", Convert.ToString(appItem[ApprovalListFields.TradePromo_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SAPRoutingSetup", Convert.ToString(appItem[ApprovalListFields.SAPRoutingSetup_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SAPCostingDetails", Convert.ToString(appItem[ApprovalListFields.SAPCostingDetails_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SAPWarehouseInfo", Convert.ToString(appItem[ApprovalListFields.SAPWarehouseInfo_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("StandardCostEntry", Convert.ToString(appItem[ApprovalListFields.StandardCostEntry_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("CostFinishedGood", Convert.ToString(appItem[ApprovalListFields.CostFinishedGood_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("FinalCostingReview", Convert.ToString(appItem[ApprovalListFields.FinalCostingReview_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("PurchasedPO", Convert.ToString(appItem[ApprovalListFields.PurchasedPO_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("RemoveSAPBlocks", Convert.ToString(appItem[ApprovalListFields.RemoveSAPBlocks_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("CustomerPO", Convert.ToString(appItem[ApprovalListFields.CustomerPO_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("MaterialsRcvdChk", Convert.ToString(appItem[ApprovalListFields.MaterialsReceivedChk_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("FirstProductionChk", Convert.ToString(appItem[ApprovalListFields.FirstProductionChk_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("DistributionChk", Convert.ToString(appItem[ApprovalListFields.DistributionCenterChk_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("MatrlWHSetUp", Convert.ToString(appItem[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate])));
                    updatedItems.Add(new KeyValuePair<string, string>("SAPCompleteItem", Convert.ToString(appItem[ApprovalListFields.SAPCompleteItemSetup_SubmittedDate])));

                    compCostSeasonal = Convert.ToString(appItem[ApprovalListFields.CompCostSeasonal_StartDate]);
                    compCostCorrPaper = Convert.ToString(appItem[ApprovalListFields.CompCostCorrPaper_StartDate]);
                    compCostFLRP = Convert.ToString(appItem[ApprovalListFields.CompCostFLRP_StartDate]);
                }

                SPList spList2 = spWeb.Lists.TryGetList(LIST_ApprovalList2Name);

                SPQuery spQuery2 = new SPQuery();
                spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
                spQuery2.RowLimit = 1;

                SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery);
                if (compassItemCol2.Count > 0)
                {
                    SPListItem appItem2 = compassItemCol2[0];
                    if (appItem2 != null)
                    {
                        updatedItems.Add(new KeyValuePair<string, string>("BEQRC", Convert.ToString(appItem2[ApprovalListFields.BEQRC_SubmittedDate])));
                      //  updatedItems.Add(new KeyValuePair<string, string>("EstPricing", Convert.ToString(appItem2[ApprovalListFields.EstPricing_SubmittedDate])));
                      //  updatedItems.Add(new KeyValuePair<string, string>("EstBracketPricing", Convert.ToString(appItem2[ApprovalListFields.EstBracketPricing_SubmittedDate])));
                    }

                }

                /*}
            }*/
            }
            return updatedItems;
        }
        public List<List<string>> GetProjectItem(int compassListId)
        {
            List<List<string>> updatedTimes = new List<List<string>>();

            SPList spList = spWeb.Lists.TryGetList(LIST_ProjectTimelineUpdateName);

            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
            spQuery.RowLimit = 1;

            SPListItemCollection compassItemCol = spList.GetItems(spQuery);

            if (compassItemCol.Count > 0)
            {
                foreach (SPListItem workflowItems in compassItemCol)
                {
                    updatedTimes.Add(new List<string>() { "IPF", Convert.ToString(workflowItems["IPF"]) });
                    updatedTimes.Add(new List<string>() { "BOMSetupPE3", Convert.ToString(workflowItems["BOMSetupPE3"]) });
                    updatedTimes.Add(new List<string>() { "BOMSetupPE2", Convert.ToString(workflowItems["BOMSetupPE2"]) });
                    updatedTimes.Add(new List<string>() { "BOMSetupPE", Convert.ToString(workflowItems["BOMSetupPE"]) });
                    updatedTimes.Add(new List<string>() { "BOMSetupProc", Convert.ToString(workflowItems["BOMSetupProc"]) });
                    updatedTimes.Add(new List<string>() { "CostingQuote", Convert.ToString(workflowItems["CostingQuote"]) });
                    updatedTimes.Add(new List<string>() { "Distribution", Convert.ToString(workflowItems["Distribution"]) });
                    updatedTimes.Add(new List<string>() { "ExternalMfg", Convert.ToString(workflowItems["ExternalMfg"]) });
                    updatedTimes.Add(new List<string>() { "FGPackSpec", Convert.ToString(workflowItems["FGPackSpec"]) });
                    try
                    {
                        updatedTimes.Add(new List<string>() { "GRAPHICS", Convert.ToString(workflowItems["Graphics"]) });
                    }
                    catch (Exception e)
                    {
                        updatedTimes.Add(new List<string>() { "GRAPHICS", Convert.ToString(workflowItems["GRAPHICS"]) });
                    }
                    updatedTimes.Add(new List<string>() { "InitialCapacity", Convert.ToString(workflowItems["InitialCapacity"]) });
                    updatedTimes.Add(new List<string>() { "InitialCosting", Convert.ToString(workflowItems["InitialCosting"]) });
                    updatedTimes.Add(new List<string>() { "OBMReview1", Convert.ToString(workflowItems["OBMReview1"]) });
                    updatedTimes.Add(new List<string>() { "OBMReview2", Convert.ToString(workflowItems["OBMReview2"]) });
                    updatedTimes.Add(new List<string>() { "Operations", Convert.ToString(workflowItems["Operations"]) });
                    updatedTimes.Add(new List<string>() { "QA", Convert.ToString(workflowItems["QA"]) });
                    updatedTimes.Add(new List<string>() { "SAPBOMSetup", Convert.ToString(workflowItems["SAPBOMSetup"]) });
                    updatedTimes.Add(new List<string>() { "SAPInitialSetup", Convert.ToString(workflowItems["SAPInitialSetup"]) });
                    updatedTimes.Add(new List<string>() { "PrelimSAPInitialSetup", Convert.ToString(workflowItems["PrelimSAPInitialSetup"]) });
                    updatedTimes.Add(new List<string>() { "SrOBMApproval2", Convert.ToString(workflowItems["SrOBMApproval2"]) });
                    updatedTimes.Add(new List<string>() { "SrOBMApproval", Convert.ToString(workflowItems["SrOBMApproval"]) });
                    updatedTimes.Add(new List<string>() { "TradePromo", Convert.ToString(workflowItems["TradePromo"]) });
                    updatedTimes.Add(new List<string>() { "EstPricing", Convert.ToString(workflowItems["EstPricing"]) });
                    updatedTimes.Add(new List<string>() { "EstBracketPricing", Convert.ToString(workflowItems["EstBracketPricing"]) });
                    updatedTimes.Add(new List<string>() { "SAPRoutingSetup", Convert.ToString(workflowItems["SAPRoutingSetup"]) });
                    updatedTimes.Add(new List<string>() { "BOMActiveDate", Convert.ToString(workflowItems["BOMActiveDate"]) });
                    updatedTimes.Add(new List<string>() { "SAPCostingDetails", Convert.ToString(workflowItems["SAPCostingDetails"]) });
                    updatedTimes.Add(new List<string>() { "SAPWarehouseInfo", Convert.ToString(workflowItems["SAPWarehouseInfo"]) });
                    updatedTimes.Add(new List<string>() { "StandardCostEntry", Convert.ToString(workflowItems["StandardCostEntry"]) });
                    updatedTimes.Add(new List<string>() { "CostFinishedGood", Convert.ToString(workflowItems["CostFinishedGood"]) });
                    updatedTimes.Add(new List<string>() { "FinalCostingReview", Convert.ToString(workflowItems["FinalCostingReview"]) });
                    updatedTimes.Add(new List<string>() { "PurchasedPO", Convert.ToString(workflowItems["PurchasedPO"]) });
                    updatedTimes.Add(new List<string>() { "RemoveSAPBlocks", Convert.ToString(workflowItems["RemoveSAPBlocks"]) });
                    updatedTimes.Add(new List<string>() { "CustomerPO", Convert.ToString(workflowItems["CustomerPO"]) });
                    updatedTimes.Add(new List<string>() { "MaterialsRcvdChk", Convert.ToString(workflowItems["MaterialsRcvdChk"]) });
                    updatedTimes.Add(new List<string>() { "FirstProductionChk", Convert.ToString(workflowItems["FirstProductionChk"]) });
                    updatedTimes.Add(new List<string>() { "DistributionChk", Convert.ToString(workflowItems["DistributionChk"]) });
                    updatedTimes.Add(new List<string>() { "FCST", Convert.ToString(workflowItems["FCST"]) });
                    updatedTimes.Add(new List<string>() { "MatrlWHSetUp", Convert.ToString(workflowItems["MatrlWHSetUp"]) });
                    updatedTimes.Add(new List<string>() { "SAPCompleteItem", Convert.ToString(workflowItems["SAPCompleteItem"]) });
                    updatedTimes.Add(new List<string>() { "BEQRC", Convert.ToString(workflowItems["BEQRC"]) });


                }
            }

            return updatedTimes;
        }
    }
    #endregion
}

