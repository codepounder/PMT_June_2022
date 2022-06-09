using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using System.Linq;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Web.UI;
using Ferrara.Compass.Abstractions.Enum;
using System.Globalization;

namespace Ferrara.Compass.Services
{
    /*
         STOP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
         IF YOU MODIFY THIS PAGE YOU MUST MAKE THE SAME CHANGE TO PROJECT TIMELINE CALCULATOR!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!          
            */
    public class ProjectTimelineTypeService : IProjectTimelineTypeService
    {
        public readonly IExceptionService exceptionService;
        public readonly IApprovalService approvalService;
        public readonly IProjectTimelineUpdateService timelineUpdater;
        public static CultureInfo ci = new CultureInfo("en-US");
        public static object _lock;

        /*public static Tuple<List<ProjectStatusReportItem>, List<ProjectStatusReportItem>, int, int, DateTime, List<DateTime>, bool> actualTimeLineStatic(DashboardDetailsItem dashboardDetails, bool pageName)
        {
            return actualTimeLine(dashboardDetails, pageName);
        }*/

        public ProjectTimelineTypeService(IExceptionService exceptionService, IApprovalService approvalService, IProjectTimelineUpdateService timelineUpdater)
        {
            this.exceptionService = exceptionService;
            this.approvalService = approvalService;
            this.timelineUpdater = timelineUpdater;
            //_lock = new object();
        }
        public List<TimelineTypeItem> GetPhases()
        {
            var newItem = new List<TimelineTypeItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
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
                            obTimelineTypeItem.WorkflowMisc = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowMisc]);
                            newItem.Add(obTimelineTypeItem);
                        }
                    }
                }
            }
            return newItem;
        }
        public List<TimelineTypeItem> GetWorkflowStepItems(DashboardDetailsItem dashboardDetails)
        {
            var newItem = new List<TimelineTypeItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Neq><FieldRef Name=\"WorkflowOrder\" /><Value Type=\"Int\">0</Value></Neq><Neq><FieldRef Name=\"WorkflowExceptions\" /><Value Type=\"Text\">DF</Value></Neq></And></Where><OrderBy><FieldRef Name=\"PhaseNumber\" Type='Int' /><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
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
                            obTimelineTypeItem.WorkflowMisc = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowMisc]);


                            if (dashboardDetails.CompassProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                            {
                                var IntExt = "Internal";
                                if (dashboardDetails.ExternalItem == "Yes")
                                {
                                    IntExt = "External";
                                }

                                if (IntExt == "Internal")
                                {
                                    obTimelineTypeItem.WorkflowStacks = Convert.ToInt32(item[ProjectTimelineTypeDays.GraphicsInternalWorkflowStacks]);
                                    obTimelineTypeItem.WorkflowMisc = Convert.ToString(item[ProjectTimelineTypeDays.GraphicsInternalWorkflowMisc]);

                                    if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "standard")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalStandard]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "expedited")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalExpedited]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "ludicrous")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalLudicrous]);
                                    }
                                    else
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalStandard]);
                                    }
                                }
                                else
                                {
                                    obTimelineTypeItem.WorkflowStacks = Convert.ToInt32(item[ProjectTimelineTypeDays.GraphicsExternalWorkflowStacks]);
                                    obTimelineTypeItem.WorkflowMisc = Convert.ToString(item[ProjectTimelineTypeDays.GraphicsExternalWorkflowMisc]);

                                    if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "standard")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalStandard]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "expedited")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalExpedited]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "ludicrous")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalLudicrous]);
                                    }
                                    else
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalStandard]);
                                    }
                                }
                            }
                            else
                            {

                                if (dashboardDetails.TimelineType.ToLower() == "standard")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Standard]);
                                }
                                else if (dashboardDetails.TimelineType.ToLower() == "expedited")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Expedited]);
                                }
                                else if (dashboardDetails.TimelineType.ToLower() == "ludicrous")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Ludicrous]);
                                }
                                else
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Standard]);
                                }
                            }
                            newItem.Add(obTimelineTypeItem);

                        }
                    }
                    List<TimelineTypeItem> DFItems = GetDFWorkflowStepItems(dashboardDetails, spWeb);
                    if (DFItems.Count > 0)
                    {
                        int graphicsLoc = newItem.FindIndex(i => i.WorkflowQuickStep == "GRAPHICS");
                        foreach (TimelineTypeItem DFItem in DFItems)
                        {
                            newItem.Insert(graphicsLoc + 1, DFItem);
                            graphicsLoc++;
                        }
                    }

                    /*List<TimelineTypeItem> newExistingItems = GetNewWorkflowStepItems(timelineType.ToLower(), CompassListItemId, spWeb);

                    if (newExistingItems.Count > 0)
                    {
                        int newLoc = newItem.FindLastIndex(i => i.WorkflowQuickStep == "CostingQuote");

                        foreach (TimelineTypeItem newEItem in newExistingItems)
                        {
                            newItem.Insert(newLoc + 1, newEItem);
                            newLoc++;
                        }
                    }*/
                }
            }
            return newItem;
        }
        public int GetSingleWorkflowStepItem(string timelineType, string task)
        {
            int duration = 0;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"WorkflowQuickStep\" /><Value Type=\"Text\">" + task + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol;
                    compassItemCol = spList.GetItems(spQuery);

                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            duration = Convert.ToInt32(item[timelineType]);
                        }
                    }
                }
            }
            return duration;
        }

        public List<TimelineTypeItem> GetAllWorkflowStepItems()
        {
            var timelineTypeItems = new List<TimelineTypeItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
                    SPListItemCollection compassItemCol;
                    SPQuery spQuery = new SPQuery();
                    spQuery.ViewFields = string.Concat(
                               "<FieldRef Name='" + ProjectTimelineTypeDays.WorkflowQuickStep + "' />",
                               "<FieldRef Name='" + ProjectTimelineTypeDays.Standard + "' />",
                               "<FieldRef Name='" + ProjectTimelineTypeDays.Ludicrous + "' />",
                               "<FieldRef Name='" + ProjectTimelineTypeDays.Expedited + "' />");
                    spQuery.ViewFieldsOnly = true;
                    compassItemCol = spList.GetItems(spQuery);

                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            TimelineTypeItem timelineTypeItem = new TimelineTypeItem();
                            string WorkflowQuickStep = Convert.ToString(item[ProjectTimelineTypeDays.WorkflowQuickStep]);
                            timelineTypeItem.Standard = Convert.ToInt32(item[ProjectTimelineTypeDays.Standard]);
                            timelineTypeItem.Ludicrous = Convert.ToInt32(item[ProjectTimelineTypeDays.Ludicrous]);
                            timelineTypeItem.Expedited = Convert.ToInt32(item[ProjectTimelineTypeDays.Expedited]);
                            timelineTypeItems.Add(timelineTypeItem);
                        }
                    }
                }
            }
            return timelineTypeItems;
        }

        public string GetTimelineType(string compassID)
        {
            string timelineType = "Standard";

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"ID\"></FieldRef><Value Type=\"Int\">" + compassID + "</Value></Eq></Where>";
                    //spQuery.RowLimit = 1;
                    SPListItemCollection itemCol = spList.GetItems(spQuery);

                    if (itemCol.Count > 0)
                    {
                        SPListItem item = itemCol[0];
                        if (item["TimelineType"] != null)
                        {
                            timelineType = item["TimelineType"].ToString();
                        }
                    }
                }
            }
            return timelineType;
        }
        public DashboardDetailsItem dashboardDetails(int compassID)
        {
            DashboardDetailsItem detailsItem = new DashboardDetailsItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"ID\"></FieldRef><Value Type=\"Int\">" + compassID + "</Value></Eq></Where>";
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.TimelineType + "' />",
                                   "<FieldRef Name='" + CompassListFields.Customer + "' />",
                                   "<FieldRef Name='" + CompassListFields.SubmittedDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.StageGateProjectListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.PLMProject + "' />",
                                   "<FieldRef Name='" + CompassListFields.ParentProjectNumber + "' />");
                    spQuery.ViewFieldsOnly = true;
                    SPListItemCollection itemCol = spList.GetItems(spQuery);

                    if (itemCol.Count > 0)
                    {
                        SPListItem item = itemCol[0];
                        if (item != null)
                        {
                            detailsItem.CompassListItemId = item.ID;
                            detailsItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
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
                            detailsItem.PLMProject = Convert.ToString(item[CompassListFields.PLMProject]);
                        }

                        SPList spCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spCompassList2Query = new SPQuery();
                        spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\"></FieldRef><Value Type=\"Text\">" + compassID + "</Value></Eq></Where>";
                        spCompassList2Query.ViewFields = string.Concat(
                                       "<FieldRef Name='" + CompassList2Fields.CompassListItemId + "' />",
                                       "<FieldRef Name='" + CompassList2Fields.NeedSExpeditedWorkflowWithSGS + "' />",
                                       "<FieldRef Name='" + CompassList2Fields.SGSExpeditedWorkflowApproved + "' />",
                                       "<FieldRef Name='" + CompassList2Fields.ExternalSemisItem + "' />");
                        spCompassList2Query.ViewFieldsOnly = true;
                        SPListItemCollection CompassList2itemCol = spCompassList2.GetItems(spCompassList2Query);

                        if (CompassList2itemCol != null && CompassList2itemCol.Count > 0)
                        {
                            SPListItem CompassList2item = CompassList2itemCol[0];
                            if (CompassList2item != null)
                            {
                                detailsItem.ExternalItem = Convert.ToString(CompassList2item[CompassList2Fields.ExternalSemisItem]);
                                detailsItem.NeedSExpeditedWorkflowWithSGS = Convert.ToString(CompassList2item[CompassList2Fields.NeedSExpeditedWorkflowWithSGS]);
                                detailsItem.SGSExpeditedWorkflowApproved = Convert.ToString(CompassList2item[CompassList2Fields.SGSExpeditedWorkflowApproved]);
                            }
                        }
                        else
                        {
                            detailsItem.ExternalItem = "";
                            detailsItem.NeedSExpeditedWorkflowWithSGS = "";
                            detailsItem.SGSExpeditedWorkflowApproved = "";
                        }
                    }
                }
            }
            return detailsItem;
        }
        public List<List<string>> GetWorkflowTasksStart(int compassId)
        {
            string BOMSetupProcStartDate = "";
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {

                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
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
                    SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                    SPQuery spQuery3 = new SPQuery();
                    spQuery3.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);

                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem workflowItems in compassItemCol3)
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
                    SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPApprovalListName);
                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
                    if (compassItemCol2.Count > 0)
                    {
                        foreach (SPListItem workflowItems in compassItemCol2)
                        {
                            projectWorkflow.Add(new List<string>() { "SAPRoutingSetup", Convert.ToString(workflowItems[SAPApprovalListFields.SAPRoutingSetup_StartDate]) });
                            projectWorkflow.Add(new List<string>() { "SAPCostingDetails", Convert.ToString(workflowItems[SAPApprovalListFields.SAPCostingDetails_StartDate]) });
                            projectWorkflow.Add(new List<string>() { "SAPWarehouseInfo", Convert.ToString(workflowItems[SAPApprovalListFields.SAPWarehouseInfo_StartDate]) });
                            projectWorkflow.Add(new List<string>() { "StandardCostEntry", Convert.ToString(workflowItems[SAPApprovalListFields.StandardCostEntry_StartDate]) });
                        }
                    }

                    SPList spDFList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassDragonflyListName);
                    SPQuery spDFQuery = new SPQuery();
                    spDFQuery.Query = "<Where><Eq><FieldRef Name=\"CompassProjectNumber\" /><Value Type=\"Text\">" + projectNo + "</Value></Eq></Where>";
                    SPListItemCollection DFItemCol = spDFList.GetItems(spDFQuery);
                    if (DFItemCol.Count > 0)
                    {
                        foreach (SPListItem DFItem in DFItemCol)
                        {
                            if (DFItem != null)
                            {
                                projectWorkflow.Add(new List<string>() { "ProductionArtStarted", Convert.ToString(DFItem[DragonflyListFields.ProjUploadedtoDF_ActStart]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "ProductionArtUploaded", Convert.ToString(DFItem[DragonflyListFields.SGSOnsiteUploadsArt_ActStart]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "RoutingComplete", Convert.ToString(DFItem[DragonflyListFields.ArtworkApproved_ActStart]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "ProofingStarted", Convert.ToString(DFItem[DragonflyListFields.ProofCreatedUploaded_ActStart]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "ProofApproved", Convert.ToString(DFItem[DragonflyListFields.ProofApproved_ActStart]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "FinalFilesPlatesShipped", Convert.ToString(DFItem[DragonflyListFields.MakeAndShipPlates_ActStart]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "CostingQuote", "", Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                            }
                        }
                    }

                    return projectWorkflow;
                }
            }
        }
        public List<List<string>> GetWorkflowTasksEnd(int compassId)
        {
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {

                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
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
                    SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                    SPQuery spQuery3 = new SPQuery();
                    spQuery3.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem workflowItems in compassItemCol3)
                        {
                            projectWorkflow.Add(new List<string>() { "EstPricing", Convert.ToString(workflowItems[ApprovalListFields.EstPricing_SubmittedDate]) });
                            projectWorkflow.Add(new List<string>() { "EstBracketPricing", Convert.ToString(workflowItems[ApprovalListFields.EstBracketPricing_SubmittedDate]) });
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
                    SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
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

                    //SPList spDFList = spWeb.Lists.TryGetList(GlobalConstants.LIST_DragonflyStatusListName);
                    //SPQuery spDFQuery = new SPQuery();
                    //spDFQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + projectNo + "</Value></Eq></Where>";
                    //SPListItemCollection DFItemCol = spDFList.GetItems(spDFQuery);
                    //if (DFItemCol.Count > 0)
                    //{
                    //    foreach (SPListItem DFItem in DFItemCol)
                    //    {
                    //        if (DFItem != null)
                    //        {
                    //            projectWorkflow.Add(new List<string>() { "ProductionArtStarted", Convert.ToString(DFItem[DragonflyStatusListFields.ProductionArtStartedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    //            projectWorkflow.Add(new List<string>() { "ProductionArtUploaded", Convert.ToString(DFItem[DragonflyStatusListFields.ProductionArtUploadedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    //            projectWorkflow.Add(new List<string>() { "RoutingComplete", Convert.ToString(DFItem[DragonflyStatusListFields.RoutingCompleteActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    //            projectWorkflow.Add(new List<string>() { "ProofingStarted", Convert.ToString(DFItem[DragonflyStatusListFields.ProofingStartedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    //            projectWorkflow.Add(new List<string>() { "ProofApproved", Convert.ToString(DFItem[DragonflyStatusListFields.ProofApprovedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    //            projectWorkflow.Add(new List<string>() { "FinalFilesPlatesShipped", Convert.ToString(DFItem[DragonflyStatusListFields.FinalFilesPlatesShippedActual]), Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    //            projectWorkflow.Add(new List<string>() { "CostingQuote", "", Convert.ToString(DFItem[DragonflyStatusListFields.MaterialNumber]) });
                    //        }
                    //    }
                    //}

                    SPList spDFList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassDragonflyListName);
                    SPQuery spDFQuery = new SPQuery();
                    spDFQuery.Query = "<Where><Eq><FieldRef Name=\"CompassProjectNumber\" /><Value Type=\"Text\">" + projectNo + "</Value></Eq></Where>";
                    SPListItemCollection DFItemCol = spDFList.GetItems(spDFQuery);
                    if (DFItemCol.Count > 0)
                    {
                        foreach (SPListItem DFItem in DFItemCol)
                        {
                            if (DFItem != null)
                            {
                                projectWorkflow.Add(new List<string>() { "ProductionArtStarted", Convert.ToString(DFItem[DragonflyListFields.ProjUploadedtoDF_ActEnd]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "ProductionArtUploaded", Convert.ToString(DFItem[DragonflyListFields.SGSOnsiteUploadsArt_ActEnd]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "RoutingComplete", Convert.ToString(DFItem[DragonflyListFields.ArtworkApproved_ActEnd]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "ProofingStarted", Convert.ToString(DFItem[DragonflyListFields.ProofCreatedUploaded_ActEnd]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "ProofApproved", Convert.ToString(DFItem[DragonflyListFields.ProofApproved_ActEnd]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "FinalFilesPlatesShipped", Convert.ToString(DFItem[DragonflyListFields.MakeAndShipPlates_ActEnd]), Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                                projectWorkflow.Add(new List<string>() { "CostingQuote", "", Convert.ToString(DFItem[DragonflyListFields.MaterialNumber]) });
                            }
                        }
                    }

                    SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPApprovalListName);
                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
                    if (compassItemCol2.Count > 0)
                    {
                        foreach (SPListItem workflowItems in compassItemCol2)
                        {
                            projectWorkflow.Add(new List<string>() { "SAPRoutingSetup", Convert.ToString(workflowItems[SAPApprovalListFields.SAPRoutingSetup_SubmittedDate]) });
                            projectWorkflow.Add(new List<string>() { "SAPCostingDetails", Convert.ToString(workflowItems[SAPApprovalListFields.SAPCostingDetails_SubmittedDate]) });
                            projectWorkflow.Add(new List<string>() { "SAPWarehouseInfo", Convert.ToString(workflowItems[SAPApprovalListFields.SAPWarehouseInfo_SubmittedDate]) });
                            projectWorkflow.Add(new List<string>() { "StandardCostEntry", Convert.ToString(workflowItems[SAPApprovalListFields.StandardCostEntry_SubmittedDate]) });
                        }
                    }
                    return projectWorkflow;
                }
            }
        }
        public int GetStackedItems(int stacked, string timelineType)
        {
            int newItem = 0;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"WorkflowStacks\"></FieldRef><Value Type=\"Int\">" + stacked + "</Value></Eq></Where><OrderBy><FieldRef Name=\"WorkflowExceptions\" Type=\"Int\" /></OrderBy>";
                    SPListItemCollection compassItemCol;
                    compassItemCol = spList.GetItems(spQuery);

                    newItem = compassItemCol.Count;
                }
            }
            return newItem;
        }
        public List<DateTime> GetHolidays()
        {
            var newItem = new List<DateTime>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_HolidayLookup);
                    SPListItemCollection compassItemCol = spList.GetItems();

                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {

                            newItem.Add(Convert.ToDateTime(item[HolidayListFields.HolidayDate]));
                        }
                    }
                }
            }
            return newItem;
        }
        private Dictionary<string, string> hideRow(int compassID)
        {
            try
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

                string tbdindicator = "";
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

                        SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem itemCompass = spCompassList.GetItemById(compassID);
                        if (itemCompass != null)
                        {
                            PHL1 = Convert.ToString(itemCompass[CompassListFields.ProductHierarchyLevel1]);
                            comanType = Convert.ToString(itemCompass[CompassListFields.CoManufacturingClassification]);
                            novelty = Convert.ToString(itemCompass[CompassListFields.NoveltyProject]);
                            projectType = Convert.ToString(itemCompass[CompassListFields.ProjectType]);
                            projectSubcat = Convert.ToString(itemCompass[CompassListFields.ProjectTypeSubCategory]);
                            PLMFlag = Convert.ToString(itemCompass[CompassListFields.PLMProject]);
                            if (PHL1 == GlobalConstants.PRODUCT_HIERARCHY1_CoMan)
                            {
                                showHideParams["ProcCoMan"] = "N";
                            }
                            else if (PHL1 == GlobalConstants.PRODUCT_HIERARCHY1_Seasonal)
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
                            tbdindicator = Convert.ToString(itemCompass[CompassListFields.TBDIndicator]);
                            if (tbdindicator != "Yes")
                            {
                                showHideParams["Distribution"] = "Y";
                                showHideParams["PrelimSAPInitialSetup"] = "Y";
                                showHideParams["TradePromo"] = "Y";
                                showHideParams["EstPricing"] = "Y";
                                showHideParams["EstBracketPricing"] = "Y";
                            }

                            //For Hiding By Project Type
                            projectType = Convert.ToString(itemCompass[CompassListFields.ProjectType]);
                            if (projectType == GlobalConstants.PROJECTTYPE_GraphicsChangesInternalAdjustments)
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
                            else if (projectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
                            {
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
                            if (PLMFlag == "Yes" && (projectSubcat == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove || projectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove || tbdindicator == "Yes" || manLocChange == "Yes"))
                            {
                                showHideParams["Distribution"] = "N";
                            }
                        }
                        string manufacturingLocation = Convert.ToString(itemCompass[CompassListFields.ManufacturingLocation]);
                        string packingLocation = Convert.ToString(itemCompass[CompassListFields.PackingLocation]);
                        SPList sDecisionItemsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spDecisionsQuery = new SPQuery();
                        spDecisionsQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassID.ToString() + "</Value></Eq></Where>";
                        SPListItemCollection sDecisionItems = sDecisionItemsList.GetItems(spDecisionsQuery);
                        if (sDecisionItems.Count > 0)
                        {
                            SPItem item = sDecisionItems[0];

                            if (item != null)
                            {
                                string approvalDecision = Convert.ToString(item[CompassProjectDecisionsListFields.SrOBMApproval_Decision]);
                                string approval2Decision = Convert.ToString(item[CompassProjectDecisionsListFields.SrOBMApproval2_Decision]);
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



                        if ((!manufacturingLocation.ToLower().Contains("external") && !packingLocation.ToLower().Contains("external")) && projectType != GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
                        {
                            showHideParams["ExternalMfg"] = "Y";
                        }
                        SPList spPackItemsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        SPQuery spPIQuery = new SPQuery();
                        spPIQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassID.ToString() + "</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></Where>";
                        SPListItemCollection spPackItems = spPackItemsList.GetItems(spPIQuery);
                        bool hasGraphics = false;
                        bool hasNewTSs = false;
                        bool hasNMTSs = false;
                        bool hasPCS = false;
                        bool hasReviewPrinterSupplier = false;
                        bool BEQRC = false;
                        bool hasMWHComponent = false;
                        int newComponentCount = 0;
                        if (spPackItems.Count > 0)
                        {
                            foreach (SPItem item in spPackItems)
                            {
                                if (item != null)
                                {
                                    #region Set Flags
                                    string newGraphicsRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                                    string packagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                                    string newExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                                    string IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);
                                    int ParentId = Convert.ToInt32(item[PackagingItemListFields.ParentID]);

                                    if (newGraphicsRequired == "Yes")
                                    {
                                        hasGraphics = true;
                                    }
                                    if (packagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                                    {
                                        hasPCS = true;
                                    }
                                    if (packagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && (newExisting == "New"))
                                    {
                                        hasNewTSs = true;
                                    }
                                    if (packagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && (newExisting == "Network Move"))
                                    {
                                        hasNMTSs = true;
                                    }
                                    if (string.Equals(newExisting, "New"))
                                    {
                                        newComponentCount++;
                                    }
                                    if (
                                            Convert.ToString(item[PackagingItemListFields.NewExisting]) == "New"
                                            &&
                                            Convert.ToString(item[PackagingItemListFields.PackagingComponent]) != GlobalConstants.COMPONENTTYPE_PurchasedSemi
                                            &&
                                            Convert.ToString(item[PackagingItemListFields.PackagingComponent]) != GlobalConstants.COMPONENTTYPE_TransferSemi
                                            &&
                                            Convert.ToString(item[PackagingItemListFields.PackagingComponent]) != GlobalConstants.COMPONENTTYPE_CandySemi
                                            &&
                                            (!Convert.ToString(item[PackagingItemListFields.PackagingComponent]).Contains("Finished Good"))
                                        )
                                    {
                                        hasMWHComponent = true;
                                    }
                                    #endregion

                                    string reviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]).ToLower();
                                    if (showHideParams["ProcSeasonal"] != "N" && showHideParams["ProcCoMan"] != "N" && showHideParams["ProcNovelty"] != "N" && comanType != "External Turnkey FG" && showHideParams["BOMSetupProc"] == "N")
                                    {
                                        bool checkComp = false;

                                        if (newExisting == "New")
                                        {
                                            checkComp = true;
                                        }

                                        if (ParentId != 0)
                                        {
                                            var NMTSs =
                                                (
                                                    from
                                                        SPItem comp in spPackItems
                                                    where
                                                        Convert.ToInt32(comp.ID) == ParentId
                                                        &&
                                                        Convert.ToString(comp[PackagingItemListFields.PackagingComponent]) == GlobalConstants.COMPONENTTYPE_TransferSemi
                                                        &&
                                                        Convert.ToString(comp[PackagingItemListFields.NewExisting]) == "Network Move"
                                                    select
                                                        comp.ID
                                                 ).ToList();

                                            if (NMTSs.Count > 0)
                                            {
                                                checkComp = true;
                                            }
                                        }

                                        if (projectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove || projectSubcat == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove)
                                        {
                                            if (ParentId == 0)
                                            {
                                                checkComp = true;
                                            }
                                        }

                                        if (projectSubcat == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove && packagingComponent.ToLower().StartsWith("corrugated") && newExisting == "Existing")
                                        {
                                            checkComp = true;
                                        }

                                        if (checkComp && reviewPrinterSupplier != "yes")
                                        {
                                            if (packagingComponent.ToLower().StartsWith("ancillary"))
                                            {
                                                showHideParams["ProcAncillary"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("corrugated"))
                                            {
                                                showHideParams["ProcCorrugated"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("(purchased)"))
                                            {
                                                showHideParams["ProcPurchased"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("film"))
                                            {
                                                showHideParams["ProcFilm"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("label"))
                                            {
                                                showHideParams["ProcLabel"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("metal"))
                                            {
                                                showHideParams["ProcMetal"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("paperboard"))
                                            {
                                                showHideParams["ProcPaperboard"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("rigid"))
                                            {
                                                showHideParams["ProcRigidPlastic"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("other"))
                                            {
                                                showHideParams["ProcOther"] = "N";
                                            }
                                        }
                                    }

                                    if (reviewPrinterSupplier == "yes")
                                    {
                                        hasReviewPrinterSupplier = true;
                                    }

                                    if (IngredientsNeedToClaimBioEng == "Yes" && (packagingComponent.ToLower().Contains("finished good") || packagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi
                                        || packagingComponent == GlobalConstants.COMPONENTTYPE_CandySemi))
                                    {
                                        BEQRC = true;
                                    }
                                }
                            }
                        }
                        #region Marketing claims
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + compassID.ToString() + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol != null)
                        {
                            if (compassItemCol.Count > 0)
                            {
                                var item = compassItemCol[0];

                                var BioEngLabelingAcceptable = Convert.ToString(item[MarketingClaimsListFields.BioEngLabelingAcceptable]);
                                if (BioEngLabelingAcceptable == "Yes")
                                {
                                    BEQRC = true;
                                }
                            }
                        }
                        #endregion
                        if (BEQRC)
                        {
                            showHideParams["BEQRC"] = "N";
                        }

                        if (!(tbdindicator == "Yes" || string.Equals(projectSubcat, GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove) || hasNewTSs || hasNMTSs))
                        {
                            showHideParams["SAPInitialSetup"] = "Y";
                        }

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

                        if (PLMFlag == "Yes" && comanType != "External Turnkey FG")
                        {
                            if (hasNMTSs || string.Equals(projectSubcat, GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove) || string.Equals(projectType, GlobalConstants.PROJECTTYPE_SimpleNetworkMove))
                            {
                                showHideParams["MatrlWHSetUp"] = "N";
                            }
                            else
                            {
                                if (hasMWHComponent)
                                {
                                    showHideParams["MatrlWHSetUp"] = "N";
                                }
                            }
                        }

                        if (newComponentCount == 0 || showHideParams["ProcExternal"] == "N")
                        {

                            if (PHL1 == GlobalConstants.PRODUCT_HIERARCHY1_Seasonal)
                            {
                                showHideParams["ProcSeasonal"] = "Y";
                            }
                        }
                    }
                }

                return showHideParams;
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectTimelineTypeService", "hideRow", "Compass Id: " + compassID);
                throw;
            }
        }
        private Dictionary<string, string> hideRowGraphics(int compassID, string GraphicsIntExt)
        {
            try
            {
                Dictionary<string, string> showHideParams = new Dictionary<string, string>();
                showHideParams.Add("GRAPHICS", "N");
                showHideParams.Add("ExternalMfg", "Y");
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
                showHideParams.Add("BOMSetupPE", "Y");
                showHideParams.Add("BOMSetupProc", "N");
                showHideParams.Add("BOMSetupPE3", "N");
                showHideParams.Add("OBMReview2", "N");
                showHideParams.Add("Distribution", "Y");
                showHideParams.Add("SAPInitialSetup", "Y");
                showHideParams.Add("PrelimSAPInitialSetup", "Y");
                showHideParams.Add("TradePromo", "Y");
                showHideParams.Add("EstPricing", "Y");
                showHideParams.Add("EstBracketPricing", "Y");
                showHideParams.Add("MatrlWHSetUp", "N");
                showHideParams.Add("SAPCompleteItem", "N");
                showHideParams.Add("FGPackSpec", "N");
                showHideParams.Add("SrOBMApproval", "N");
                showHideParams.Add("SrOBMApproval2", "Y");
                showHideParams.Add("BEQRC", "Y");
                showHideParams.Add("OBMReview1", "Y");

                string tbdindicator = "";
                string projectType = "";
                string PHL1 = "";
                string comanType = "";
                string novelty = "";
                string projectSubcat = "";
                string PLMFlag = "";

                if (GraphicsIntExt == "External")
                {
                    showHideParams["ExternalMfg"] = "N";
                }

                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {

                        SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem itemCompass = spCompassList.GetItemById(compassID);
                        if (itemCompass != null)
                        {
                            PHL1 = Convert.ToString(itemCompass[CompassListFields.ProductHierarchyLevel1]);
                            comanType = Convert.ToString(itemCompass[CompassListFields.CoManufacturingClassification]);
                            novelty = Convert.ToString(itemCompass[CompassListFields.NoveltyProject]);
                            projectType = Convert.ToString(itemCompass[CompassListFields.ProjectType]);
                            projectSubcat = Convert.ToString(itemCompass[CompassListFields.ProjectTypeSubCategory]);
                            PLMFlag = Convert.ToString(itemCompass[CompassListFields.PLMProject]);
                            if (PHL1 == GlobalConstants.PRODUCT_HIERARCHY1_CoMan)
                            {
                                showHideParams["ProcCoMan"] = "N";
                            }
                            else if (PHL1 == GlobalConstants.PRODUCT_HIERARCHY1_Seasonal)
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
                            if (PLMFlag == "Yes")
                            {
                                showHideParams["FGPackSpec"] = "Y";
                            }
                            tbdindicator = Convert.ToString(itemCompass[CompassListFields.TBDIndicator]);

                            //For Hiding By Project Type
                            projectType = Convert.ToString(itemCompass[CompassListFields.ProjectType]);
                        }

                        SPList sDecisionItemsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spDecisionsQuery = new SPQuery();
                        spDecisionsQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassID.ToString() + "</Value></Eq></Where>";
                        SPListItemCollection sDecisionItems = sDecisionItemsList.GetItems(spDecisionsQuery);
                        if (sDecisionItems.Count > 0)
                        {
                            SPItem item = sDecisionItems[0];

                            if (item != null)
                            {
                                string approvalDecision = Convert.ToString(item[CompassProjectDecisionsListFields.SrOBMApproval_Decision]);
                                string approval2Decision = Convert.ToString(item[CompassProjectDecisionsListFields.SrOBMApproval2_Decision]);
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

                        SPList spPackItemsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        SPQuery spPIQuery = new SPQuery();
                        spPIQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassID.ToString() + "</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></Where>";
                        SPListItemCollection spPackItems = spPackItemsList.GetItems(spPIQuery);
                        bool hasGraphics = false;
                        bool hasReviewPrinterSupplier = false;
                        bool BEQRC = false;

                        int newComponentCount = 0;
                        if (spPackItems.Count > 0)
                        {
                            foreach (SPItem item in spPackItems)
                            {
                                if (item != null)
                                {
                                    #region Set Flags
                                    string newGraphicsRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                                    string packagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                                    string newExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                                    string IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);

                                    if (newGraphicsRequired == "Yes")
                                    {
                                        hasGraphics = true;
                                    }
                                    if (string.Equals(newExisting, "New"))
                                    {
                                        newComponentCount++;
                                    }

                                    #endregion

                                    string reviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]).ToLower();
                                    if (showHideParams["ProcSeasonal"] != "N" && showHideParams["ProcCoMan"] != "N" && showHideParams["ProcNovelty"] != "N" && comanType != "External Turnkey FG" && showHideParams["BOMSetupProc"] == "N")
                                    {
                                        bool checkComp = false;
                                        int ParentId = Convert.ToInt32(item[PackagingItemListFields.ParentID]);

                                        if (newExisting == "New")
                                        {
                                            checkComp = true;
                                        }

                                        if (ParentId != 0)
                                        {
                                            var NMTSs =
                                                (
                                                    from
                                                        SPItem comp in spPackItems
                                                    where
                                                        Convert.ToInt32(comp.ID) == ParentId
                                                        &&
                                                        Convert.ToString(comp[PackagingItemListFields.PackagingComponent]) == GlobalConstants.COMPONENTTYPE_TransferSemi
                                                        &&
                                                        Convert.ToString(comp[PackagingItemListFields.NewExisting]) == "Network Move"
                                                    select
                                                        comp.ID
                                                 ).ToList();

                                            if (NMTSs.Count > 0)
                                            {
                                                checkComp = true;
                                            }
                                        }

                                        if (projectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove || projectSubcat == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove)
                                        {
                                            if (ParentId == 0)
                                            {
                                                checkComp = true;
                                            }
                                        }

                                        if (checkComp && reviewPrinterSupplier != "yes")
                                        {
                                            if (packagingComponent.ToLower().StartsWith("ancillary"))
                                            {
                                                showHideParams["ProcAncillary"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("corrugated"))
                                            {
                                                showHideParams["ProcCorrugated"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("(purchased)"))
                                            {
                                                showHideParams["ProcPurchased"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("film"))
                                            {
                                                showHideParams["ProcFilm"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("label"))
                                            {
                                                showHideParams["ProcLabel"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("metal"))
                                            {
                                                showHideParams["ProcMetal"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("paperboard"))
                                            {
                                                showHideParams["ProcPaperboard"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("rigid"))
                                            {
                                                showHideParams["ProcRigidPlastic"] = "N";
                                            }
                                            else if (packagingComponent.ToLower().StartsWith("other"))
                                            {
                                                showHideParams["ProcOther"] = "N";
                                            }
                                        }
                                    }

                                    if (reviewPrinterSupplier == "yes")
                                    {
                                        hasReviewPrinterSupplier = true;
                                    }

                                    if (IngredientsNeedToClaimBioEng == "Yes" && (packagingComponent.ToLower().Contains("finished good") || packagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi
                                        || packagingComponent == GlobalConstants.COMPONENTTYPE_CandySemi))
                                    {
                                        BEQRC = true;
                                    }
                                }
                            }
                        }
                        #region Marketing claims
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + compassID.ToString() + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol != null)
                        {
                            if (compassItemCol.Count > 0)
                            {
                                var item = compassItemCol[0];

                                var BioEngLabelingAcceptable = Convert.ToString(item[MarketingClaimsListFields.BioEngLabelingAcceptable]);
                                if (BioEngLabelingAcceptable == "Yes")
                                {
                                    BEQRC = true;
                                }
                            }
                        }
                        #endregion
                        if (BEQRC)
                        {
                            showHideParams["BEQRC"] = "N";
                        }
                        if (!hasGraphics)
                        {
                            showHideParams["GRAPHICS"] = "Y";
                        }
                        if (hasReviewPrinterSupplier && showHideParams["ProcCoMan"] != "N" && showHideParams["ProcNovelty"] != "N" && showHideParams["BOMSetupProc"] == "N" && showHideParams["ExternalMfg"] == "N")
                        {
                            hasReviewPrinterSupplier = true;
                            showHideParams["ProcExternal"] = "N";
                        }
                        if (newComponentCount == 0)
                        {

                            if (PHL1 == GlobalConstants.PRODUCT_HIERARCHY1_Seasonal)
                            {
                                showHideParams["ProcSeasonal"] = "Y";
                            }
                        }
                    }
                }

                return showHideParams;
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectTimelineTypeService", "hideRowGraphics", "Compass Id: " + compassID);
                throw;
            }
        }
        public void dashboardUpdateDates(int CompassListItemId, string whichDate, string date)
        {
            //try {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        DateTime updateDate = DateTime.Parse(date);

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem appItem = spList.GetItemById(CompassListItemId);

                        if (appItem != null)
                        {
                            if (whichDate == "ship")
                            {
                                appItem[CompassListFields.RevisedFirstShipDate] = updateDate;
                            }
                            if (whichDate == "prod")
                            {
                                appItem[CompassListFields.FirstProductionDate] = updateDate;
                            }

                            appItem.Update();

                        }
                        /*if (whichDate == "ship")
                        {
                            SPList spShipList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId.ToString() + "</Value></Eq></Where>";
                            SPListItemCollection itemUpdate = spShipList.GetItems(spQuery);

                            if (itemUpdate.Count > 0)
                            {
                                foreach (SPListItem item in itemUpdate)
                                {
                                    if (item != null)
                                    {
                                        item["FirstShipDate"] = updateDate;
                                        item.Update();
                                    }
                                }

                            }
                        }*/
                        spWeb.AllowUnsafeUpdates = false;

                    }
                }
            });
            /*}catch(Exception e)
            {

            }*/
        }
        public void workflowStatusUpdate(int CompassListItemId, string status)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {//try {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        string updateDate = DateTime.Now.ToString();

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId.ToString() + "</Value></Eq></Where>";
                        SPListItemCollection itemUpdate = spList.GetItems(spQuery);

                        if (itemUpdate.Count > 0)
                        {
                            foreach (SPListItem appItem in itemUpdate)
                            {
                                if (appItem != null)
                                {
                                    if (status == "onhold")
                                    {
                                        appItem[ApprovalListFields.OnHold_ModifiedDate] = updateDate;
                                        appItem[ApprovalListFields.OnHold_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                    }
                                    else if (status == "preproduction" || status == "Pre-Production")
                                    {
                                        appItem[ApprovalListFields.PreProduction_ModifiedDate] = updateDate;
                                        appItem[ApprovalListFields.PreProduction_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                    }
                                    else if (status == "cancelled" || status == "Cancelled")
                                    {
                                        appItem[ApprovalListFields.Cancelled_ModifiedDate] = updateDate;
                                        appItem[ApprovalListFields.Cancelled_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                    }
                                    else if (status == "completed" || status == "Completed")
                                    {
                                        appItem[ApprovalListFields.Completed_ModifiedDate] = updateDate;
                                        appItem[ApprovalListFields.Completed_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                    }

                                    appItem.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;

                    }
                }
            });
            /*}catch(Exception e)
            {

            }*/
        }
        public List<TimelineTypeItem> GetDFWorkflowStepItems(DashboardDetailsItem dashboardDetails, SPWeb spWeb)
        {
            List<TimelineTypeItem> FinalDFItems = new List<TimelineTypeItem>();

            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><And><Neq><FieldRef Name=\"WorkflowOrder\"></FieldRef><Value Type=\"Int\">0</Value></Neq><Eq><FieldRef Name=\"WorkflowExceptions\"></FieldRef><Value Type=\"Text\">DF</Value></Eq></And></Where><OrderBy><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
            SPListItemCollection compassItemCol;
            compassItemCol = spList.GetItems(spQuery);
            List<TimelineTypeItem> DFItems = new List<TimelineTypeItem>();

            SPList spGraphicsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
            SPQuery spGraphicsQuery = new SPQuery();
            spGraphicsQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + dashboardDetails.CompassListItemId.ToString() + "</Value></Eq><And><Eq><FieldRef Name=\"GraphicsChangeRequired\" /><Value Type=\"Text\">Yes</Value></Eq><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></And></Where>";
            SPListItemCollection itemUpdate = spGraphicsList.GetItems(spGraphicsQuery);

            if (itemUpdate.Count > 0)
            {
                List<KeyValuePair<string, string>> PIHeaders = new List<KeyValuePair<string, string>>();
                foreach (SPListItem packaging in itemUpdate)
                {
                    string matId = Convert.ToString(packaging.ID);
                    string matNumber = Convert.ToString(packaging[PackagingItemListFields.MaterialNumber]);
                    string matDesc = Convert.ToString(packaging[PackagingItemListFields.MaterialDescription]);
                    string piHeaderText = matNumber + ": " + matDesc;
                    PIHeaders.Add(new KeyValuePair<string, string>(matId, piHeaderText));
                }
                if (itemUpdate.Count > 1 && PIHeaders.Count > 0)
                {


                    List<List<TimelineTypeItem>> graphicsList = new List<List<TimelineTypeItem>>();
                    foreach (KeyValuePair<string, string> header in PIHeaders)
                    {
                        List<TimelineTypeItem> graphicsItems = new List<TimelineTypeItem>(copyItem(compassItemCol, dashboardDetails));

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
                    List<TimelineTypeItem> graphicsItems = new List<TimelineTypeItem>(copyItem(compassItemCol, dashboardDetails));
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
        public List<TimelineTypeItem> GetNewWorkflowStepItems(string timelineType, int CompassListItemId, SPWeb spWeb)
        {
            List<TimelineTypeItem> FinalNewItems = new List<TimelineTypeItem>();

            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Neq><FieldRef Name=\"WorkflowOrder\"></FieldRef><Value Type=\"Int\">0</Value></Neq></Where><OrderBy><FieldRef Name=\"WorkflowOrder\" Type='Text' /></OrderBy>";
            SPListItemCollection compassItemCol;
            compassItemCol = spList.GetItems(spQuery);
            List<TimelineTypeItem> DFItems = new List<TimelineTypeItem>();

            SPList spGraphicsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
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
        public Tuple<List<ProjectStatusReportItem>, List<ProjectStatusReportItem>, int, int, DateTime, List<DateTime>, bool> actualTimeLine(DashboardDetailsItem dashboardDetails, bool pageName)
        {
            try
            {
                string timelineType = dashboardDetails.TimelineType;
                ApprovalListItem approvalItem = approvalService.GetApprovalItem(dashboardDetails.CompassListItemId);

                List<DateTime> holidaysList = GetHolidays();
                List<TimelineTypeItem> phases = GetPhases();
                var IntExt = "Internal";
                if (dashboardDetails.ExternalItem == "Yes")
                {
                    IntExt = "External";
                }
                dashboardDetails.PMReview2Submitted = false;
                if (!string.IsNullOrEmpty(approvalItem.OBMReview2_SubmittedDate))
                {
                    dashboardDetails.PMReview2Submitted = true;
                }
                List<TimelineTypeItem> tasks = GetWorkflowStepItems(dashboardDetails);
                List<List<string>> taskCallStart = GetWorkflowTasksStart(dashboardDetails.CompassListItemId);
                List<List<string>> taskCallEnd = GetWorkflowTasksEnd(dashboardDetails.CompassListItemId);
                List<List<string>> updatedTimes = timelineUpdater.GetProjectItem(dashboardDetails.CompassListItemId);
                Dictionary<string, string> hideRows = new Dictionary<string, string>();
                if (dashboardDetails.CompassProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    hideRows = hideRowGraphics(dashboardDetails.CompassListItemId, IntExt);
                }
                else
                {
                    hideRows = hideRow(dashboardDetails.CompassListItemId);
                }

                List<KeyValuePair<int, DateTime>> listOfExceptionList = new List<KeyValuePair<int, DateTime>>();
                List<DateTime> ExceptionEndDays = new List<DateTime>();
                List<List<string>> existingProject = new List<List<string>>();
                List<KeyValuePair<string, string>> completedTasks = new List<KeyValuePair<string, string>>();
                if (pageName)
                {
                    existingProject = timelineUpdater.GetProjectItem(dashboardDetails.CompassListItemId);
                    completedTasks = timelineUpdater.GetCompletedItems(dashboardDetails.CompassListItemId);
                }
                DateTime previousTaskStart = new DateTime();
                int exceptionHolder = 0;
                double dayHolder = 0;
                double leftSpace = 0;
                DateTime projectStart = new DateTime();
                DateTime ipfSubmit = new DateTime();
                DateTime ipfStart = new DateTime();
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
                if (String.IsNullOrEmpty(approvalItem.IPF_StartDate))
                {
                    ipfStart = DateTime.Now;
                }
                else
                {
                    ipfStart = DateTime.Parse(approvalItem.IPF_StartDate, ci);
                }
                #region IPF Actual End Date
                DateTime IPFActualEndDate = ipfSubmit;
                TimeSpan ipfTime = IPFActualEndDate.TimeOfDay;
                if (ipfTime <= TimeSpan.Parse("07:59:00"))
                {
                    TimeSpan ts = new TimeSpan(8, 0, 0);
                    IPFActualEndDate = IPFActualEndDate.Date + ts;
                }
                else if (ipfTime >= TimeSpan.Parse("15:59:00"))
                {
                    //dayHolder = 1;
                    IPFActualEndDate = IPFActualEndDate.AddDays(1);
                    TimeSpan ts = new TimeSpan(8, 0, 0);
                    IPFActualEndDate = IPFActualEndDate.Date + ts;
                }
                if ((int)IPFActualEndDate.DayOfWeek == 0)//If Sunday Add 1 day
                {
                    IPFActualEndDate = IPFActualEndDate.AddDays(1);
                    dayHolder = 1;
                }
                else if ((int)IPFActualEndDate.DayOfWeek == 6)//If Saturday Add 2 days
                {
                    IPFActualEndDate = IPFActualEndDate.AddDays(2);
                    dayHolder = 2;
                }
                #endregion
                #region IPF Actual Start Date
                DateTime IPFActualStartDate = ipfStart;
                ipfTime = IPFActualStartDate.TimeOfDay;
                if (ipfTime <= TimeSpan.Parse("07:59:00"))
                {
                    TimeSpan ts = new TimeSpan(8, 0, 0);
                    IPFActualStartDate = IPFActualEndDate.Date + ts;
                }
                else if (ipfTime >= TimeSpan.Parse("15:59:00"))
                {
                    //dayHolder = 1;
                    IPFActualStartDate = IPFActualStartDate.AddDays(1);
                    TimeSpan ts = new TimeSpan(8, 0, 0);
                    IPFActualStartDate = IPFActualStartDate.Date + ts;
                }
                if ((int)IPFActualStartDate.DayOfWeek == 0)//If Sunday Add 1 day
                {
                    IPFActualStartDate = IPFActualStartDate.AddDays(1);
                }
                else if ((int)IPFActualStartDate.DayOfWeek == 6)//If Saturday Add 2 days
                {
                    IPFActualStartDate = IPFActualStartDate.AddDays(2);
                }
                #endregion

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
                DateTime todayFields = IPFActualEndDate.AddDays(-(int)IPFActualEndDate.DayOfWeek + (int)DayOfWeek.Monday);
                TimeSpan nf = new TimeSpan(8, 0, 0);
                todayFields = todayFields.Date + nf;
                DateTime projectEnd = new DateTime();
                DateTime rightNow = DateTime.Now;
                double projectDuration = 0;
                int phaseCounter = 0;
                int allTaskCounter = 0;
                double weekendCount = 0;
                DateTime previousTaskComplete = new DateTime();
                DateTime GraphicsTaskComplete = new DateTime();
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
                                extraParams = "&" + GlobalConstants.QUERYSTRING_SAPTask + "=SAPROUTINGSETUP";
                            }
                            else if (WFQuickStep == "SAPCostingDetails")
                            { extraParams = "&" + GlobalConstants.QUERYSTRING_SAPTask + "=SAPCOSTINGDETAILS"; }
                            else if (WFQuickStep == "SAPWarehouseInfo")
                            { extraParams = "&" + GlobalConstants.QUERYSTRING_SAPTask + "=SAPWAREHOUSEINFO"; }
                            else if (WFQuickStep == "StandardCostEntry")
                            { extraParams = "&" + GlobalConstants.QUERYSTRING_SAPTask + "=STANDARDCOSTENTRY"; }
                            else if (WFQuickStep == "CostFinishedGood")
                            { extraParams = "&" + GlobalConstants.QUERYSTRING_SAPTask + "=COSTFINISHEDGOOD"; }
                            else if (WFQuickStep == "FinalCostingReview")
                            { extraParams = "&" + GlobalConstants.QUERYSTRING_SAPTask + "=FINALCOSTINGREVIEW"; }
                            else if (WFQuickStep == "RemoveSAPBlocks")
                            { extraParams = "&" + GlobalConstants.QUERYSTRING_SAPTask + "=REMOVESAPBLOCKS"; }

                            taskForm.NavigateUrl = "/Pages/" + task.Value + ".aspx?ProjectNo=" + dashboardDetails.ProjectNumber + extraParams;

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
                        #region Start Task Day Section
                        //Begin 
                        DateTime taskStart = new DateTime();
                        int addedDay = 0;
                        double maxHolder = 0;
                        int currentStack = timelineInfo.WorkflowStacks;
                        int diffDaysHolder = 0;
                        #region IPF
                        if (data.PhaseNumber == 1)//For IPF Phase
                        {
                            taskStart = IPFActualStartDate;
                            List<string> approvalDecisions = (
                                from
                                    IPFStatus in hideRows
                                where
                                    IPFStatus.Key == "SrOBMApproval" || IPFStatus.Key == "SrOBMApproval2"
                                select
                                    IPFStatus.Value
                            ).ToList();

                            try
                            {
                                if ((approvalDecisions[0] == "Y" || approvalDecisions[1] == "Y") && dashboardDetails.WorkflowPhase.ToLower() == "ipf phase")
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
                            }
                            catch (Exception e)
                            {
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
                            }
                            statusCheck = true;
                            taskEnd = IPFActualEndDate;
                            taskEndConfirm = true;
                        }
                        #endregion
                        #region Non-IPF
                        else //For all phases other than IPF
                        {
                            int taskListCount = taskCallStart.SelectMany(list => list).Distinct().Count();
                            if (WFException != "DF")
                            {
                                foreach (List<string> newTimes in taskCallStart.Where(s => s[0] == WFQuickStep)) //Get Compass Approval Start Dates
                                {
                                    if (newTimes[1] != "")
                                    {
                                        try
                                        {
                                            taskStart = DateTime.Parse(newTimes[1], ci);
                                            DateTime holderDate = IPFActualEndDate.AddDays(dayHolder);
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
                                            exceptionService.Handle(LogCategory.CriticalError, exception, GlobalConstants.PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
                                        }
                                    }
                                }
                                var UpdatedWorkflowMisc = false;
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
                                        UpdatedWorkflowMisc = true;
                                    }
                                    catch (Exception e)
                                    {
                                        taskStart = previousTaskComplete;
                                        dayHolder = dayHolder + timelineInfo.Holder;
                                    }
                                }
                                else if (!updatedStart)
                                {
                                    taskStart = previousTaskComplete;
                                    dayHolder = dayHolder + timelineInfo.Holder;

                                    if (WFException == "DF")
                                    {
                                        updatedStart = true;
                                    }
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
                                            if (UpdatedWorkflowMisc)
                                            {
                                                listOfExceptionList.Add(new KeyValuePair<int, DateTime>(currentStack, taskStart));
                                            }
                                            else
                                            {
                                                listOfExceptionList.Add(new KeyValuePair<int, DateTime>(currentStack, previousTaskStart));
                                            }
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
                            else
                            {
                                foreach (List<string> newTimes in taskCallStart.Where(s => s[0] == WFQuickStep && s[2] == timelineInfo.WorkflowMisc)) //Get Compass Approval Start Dates
                                {
                                    if (newTimes[1] != "")
                                    {
                                        try
                                        {
                                            if (WFQuickStep == "ProductionArtStarted")
                                            {
                                                taskStart = GraphicsTaskComplete;
                                            }
                                            else
                                            {
                                                taskStart = DateTime.Parse(newTimes[1], ci);
                                            }
                                            DateTime holderDate = IPFActualEndDate.AddDays(dayHolder);
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
                                            exceptionService.Handle(LogCategory.CriticalError, exception, GlobalConstants.PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
                                        }
                                    }
                                }
                                bool UpdateStarDate = true;
                                if (!updatedStart)
                                {
                                    if (WFQuickStep == "ProductionArtStarted")
                                    {
                                        taskStart = GraphicsTaskComplete;
                                        UpdateStarDate = false;
                                    }
                                    else
                                    {
                                        taskStart = previousTaskComplete;
                                    }
                                    dayHolder = dayHolder + timelineInfo.Holder;
                                    //updatedStart = true;
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
                                        if (keyValue == currentStack && !updatedStart && UpdateStarDate)
                                        {
                                            taskStart = exceptionStart;
                                        }
                                        exceptionHolder = currentStack;
                                        hasException = true;
                                    }
                                }
                            }
                        }
                        #endregion
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
                        #endregion
                        #region End Task Day Section
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
                                        exceptionService.Handle(LogCategory.CriticalError, exception, GlobalConstants.PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
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
                                        exceptionService.Handle(LogCategory.CriticalError, exception, GlobalConstants.PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
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
                                        taskEnd = addWeekends(taskStart, Convert.ToInt32(updatedDays[1]));
                                        double dayChange = weekends(taskStart, taskEnd, holidaysList, false);

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
                                            if (WFException != "DF" && WFException != "NE")
                                            {
                                                timelineUpdater.UpdateSingleProjectTimelineItem(WFQuickStep, addDays.ToString(), dashboardDetails.CompassListItemId, timelineType, dashboardDetails.ProjectNumber);
                                            }
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
                                        exceptionService.Handle(LogCategory.CriticalError, exception, GlobalConstants.PAGE_NAME_ProjectStatusDashboard, "Page_Load", errorDets);
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
                            DateTime taskEndTemp = addWeekends(taskStart, Convert.ToInt16(timelineInfo.Holder));
                            int compareEnd = DateTime.Compare(taskEndTemp, rightNow);
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
                                    timelineUpdater.UpdateSingleProjectTimelineItem(WFQuickStep, addDays.ToString(), dashboardDetails.CompassListItemId, timelineType, dashboardDetails.ProjectNumber);
                                }
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

                        if (WFQuickStep == "GRAPHICS")
                        {
                            GraphicsTaskComplete = taskEnd;
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
                        #endregion
                        #region Status Section
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
                            string approvalDecisions = (from IPFStatus in hideRows where IPFStatus.Key == WFQuickStep select IPFStatus.Value).FirstOrDefault();
                            if (approvalDecisions == "Y" && dashboardDetails.WorkflowPhase != GlobalConstants.WORKFLOWPHASE_SrOBMInitialReview)
                            {
                                status = "Request IPF Update";
                            }
                        }
                        dashboardRowCells.Status = status;
                        //End Status Section 
                        #endregion
                        #region calendar Display
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
                                        dashboardRowCells.ReadOnly = "true";
                                    }
                                    else
                                    {
                                        dashboardRowCells.ReadOnly = "false";
                                    }
                                }
                                else
                                {
                                    Boolean wasAdded = false;
                                    foreach (List<string> task in existingProject.Where(r => r[0] == WFQuickStep))
                                    {
                                        if (status == "Completed")
                                        {
                                            dashboardRowCells.ReadOnly = "true";
                                            wasAdded = true;
                                        }
                                        else
                                        {
                                            dashboardRowCells.ReadOnly = "false";
                                            wasAdded = true;
                                        }
                                    }
                                    if (!wasAdded)
                                    {
                                        dashboardRowCells.ReadOnly = "false";
                                    }
                                }
                            }
                            if (WFException == "PROC")
                            {
                                dashboardRowCells.ReadOnly = "true";
                            }
                        }
                        else
                        {

                            dashboardRowCells.PixelsFromLeft = leftSpace;
                            dashboardRowCells.Width = taskWidth;
                        }
                        leftSpace = leftSpace + (taskWidth);
                        //End calendar Display 
                        #endregion
                        dashboardRowCells.Color = color;
                        #region email section
                        //Start email section
                        if (status.ToLower() != "waiting" && WFQuickStep != "IPF" && WFQuickStep != "SAPRoutingSetup")
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
                            else if (WFException == "PROC")
                            {
                                tempWFQuickStep = "BOMSetup" + WFQuickStep;
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
                        #endregion
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
                List<ProjectStatusReportItem> OGTimeline = originalTimeLine(firstShipDate, ipfSubmit, IPFActualEndDate, phases, tasks, hideRows, holidaysList);
                var returnItem = Tuple.Create(dashboardRows, OGTimeline, allTaskCounter, phaseCounter, todayFields, holidaysList, IPFSubmitted);
                return returnItem;
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "ProjectTimelineTypeService", "actualTimeLine", "Project Number: " + dashboardDetails.ProjectNumber);
                throw;
            }
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
                        var UpdatedWorkflowMisc = false;
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
                                UpdatedWorkflowMisc = true;
                            }
                            catch (Exception e)
                            {
                                taskStart = previousTaskComplete;
                                dayHolder = dayHolder + timelineInfo.Holder;
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
                                    if (UpdatedWorkflowMisc)
                                    {
                                        listOfExceptionList.Add(new KeyValuePair<int, DateTime>(currentStack, taskStart));
                                    }
                                    else
                                    {
                                        listOfExceptionList.Add(new KeyValuePair<int, DateTime>(currentStack, previousTaskStart));
                                    }
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
        public List<TimelineTypeItem> copyItem(SPListItemCollection compassItemCol, DashboardDetailsItem dashboardDetails)
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

                    if (dashboardDetails.CompassProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                    {
                        var IntExt = "Internal";
                        if (dashboardDetails.ExternalItem == "Yes")
                        {
                            IntExt = "External";
                        }

                        if (IntExt == "Internal")
                        {
                            obTimelineTypeItem.WorkflowStacks = Convert.ToInt32(item[ProjectTimelineTypeDays.GraphicsInternalWorkflowStacks]);
                            obTimelineTypeItem.WorkflowMisc = Convert.ToString(item[ProjectTimelineTypeDays.GraphicsInternalWorkflowMisc]);

                            if (obTimelineTypeItem.WorkflowExceptions == "DF")
                            {
                                if (dashboardDetails.PMReview2Submitted)
                                {
                                    if (dashboardDetails.SGSExpeditedWorkflowApproved.ToLower() == "yes")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalExpedited]);
                                    }
                                    else
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalStandard]);
                                    }
                                }
                                else
                                {
                                    if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "standard")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalStandard]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "expedited")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalExpedited]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "ludicrous")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalLudicrous]);
                                    }
                                    else
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalStandard]);
                                    }
                                }
                            }
                            else
                            {
                                if (dashboardDetails.TimelineType.ToLower() == "standard")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalStandard]);
                                }
                                else if (dashboardDetails.TimelineType.ToLower() == "expedited")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalExpedited]);
                                }
                                else if (dashboardDetails.TimelineType.ToLower() == "ludicrous")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalLudicrous]);
                                }
                                else
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsInternalStandard]);
                                }

                            }
                        }
                        else
                        {
                            obTimelineTypeItem.WorkflowStacks = Convert.ToInt32(item[ProjectTimelineTypeDays.GraphicsExternalWorkflowStacks]);
                            obTimelineTypeItem.WorkflowMisc = Convert.ToString(item[ProjectTimelineTypeDays.GraphicsExternalWorkflowMisc]);
                            if (obTimelineTypeItem.WorkflowExceptions == "DF")
                            {
                                if (dashboardDetails.PMReview2Submitted)
                                {
                                    if (dashboardDetails.SGSExpeditedWorkflowApproved.ToLower() == "yes")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalExpedited]);
                                    }
                                    else
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalStandard]);
                                    }
                                }
                                else
                                {
                                    if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "standard")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalStandard]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "expedited")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalExpedited]);
                                    }
                                    else if (dashboardDetails.NeedSExpeditedWorkflowWithSGS.ToLower() == "ludicrous")
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalLudicrous]);
                                    }
                                    else
                                    {
                                        obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalStandard]);
                                    }
                                }
                            }
                            else
                            {
                                if (dashboardDetails.TimelineType.ToLower() == "standard")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalStandard]);
                                }
                                else if (dashboardDetails.TimelineType.ToLower() == "expedited")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalExpedited]);
                                }
                                else if (dashboardDetails.TimelineType.ToLower() == "ludicrous")
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalLudicrous]);
                                }
                                else
                                {
                                    obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.GraphicsExternalStandard]);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dashboardDetails.TimelineType) || dashboardDetails.TimelineType.ToLower() == "standard")
                        {
                            obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Standard]);
                        }
                        else if (dashboardDetails.TimelineType.ToLower() == "expedited")
                        {
                            obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Expedited]);
                        }
                        else if (dashboardDetails.TimelineType.ToLower() == "ludicrous")
                        {
                            obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Ludicrous]);
                        }
                        else
                        {
                            obTimelineTypeItem.Holder = Convert.ToDouble(item[ProjectTimelineTypeDays.Standard]);
                        }
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
                    "BEQRC",
                    "OBMReview1"
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
        public static DateTime addWeekends(DateTime startDate, int Days)
        {
            DateTime endDate = startDate;
            for (int i = 0; i < Days; i++)
            {
                endDate = endDate.AddDays(1);
                if (endDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    endDate = endDate.AddDays(2);
                }
                else if (endDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    endDate = endDate.AddDays(1);
                }
            }
            return endDate;
        }
    }
}
