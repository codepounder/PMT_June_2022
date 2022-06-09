using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using System.Threading.Tasks;
using Microsoft.SharePoint.Workflow;

namespace Ferrara.Compass.Services
{
    public class DashboardService : IDashboardService
    {
        public object ErrorSummary { get; private set; }
        private readonly IExceptionService exceptionService;
        public DashboardService(IExceptionService exceptionService)
        {
            this.exceptionService = exceptionService;
        }
        public async Task<List<KeyValuePair<int, Dictionary<string, string>>>> GetAllPackagingNumbersForProjectAsync(List<int> compassListItemIds)
        {
            List<KeyValuePair<int, Dictionary<string, string>>> packagingNumberList = new List<KeyValuePair<int, Dictionary<string, string>>>();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    string query = "<Where><In><FieldRef Name=\"CompassListItemId\" /><Values>";
                    foreach (int compassId in compassListItemIds)
                    {
                        query += "<Value Type=\"Int\">" + compassId + "</Value>";
                    }
                    query += "</Values></In></Where>";
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + PackagingItemListFields.MaterialNumber + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.Deleted + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.PackagingComponent + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.CompassListItemId + "' />");
                    spQuery.ViewFieldsOnly = true;
                    spQuery.Query = query;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            Dictionary<string, string> packagingNumbers = new Dictionary<string, string>();
                            packagingNumbers.Add(Convert.ToString(item[PackagingItemListFields.MaterialNumber]), Convert.ToString(item[PackagingItemListFields.PackagingComponent]));
                            packagingNumberList.Add(new KeyValuePair<int, Dictionary<string, string>>(Convert.ToInt32(item[PackagingItemListFields.CompassListItemId]), packagingNumbers));
                        }
                    }
                }
            }
            return await Task.FromResult(packagingNumberList);
        }
        public List<KeyValuePair<int, Dictionary<string, string>>> GetAllPackagingNumbersForProject(List<int> compassListItemIds)
        {
            List<KeyValuePair<int, Dictionary<string, string>>> packagingNumberList = new List<KeyValuePair<int, Dictionary<string, string>>>();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    string query = "<Where><In><FieldRef Name=\"CompassListItemId\" /><Values>";
                    foreach (int compassId in compassListItemIds)
                    {
                        query += "<Value Type=\"Int\">" + compassId + "</Value>";
                    }
                    query += "</Values></In></Where>";
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + PackagingItemListFields.MaterialNumber + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.Deleted + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.PackagingComponent + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.CompassListItemId + "' />");
                    spQuery.ViewFieldsOnly = true;
                    spQuery.Query = query;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            Dictionary<string, string> packagingNumbers = new Dictionary<string, string>();
                            packagingNumbers.Add(Convert.ToString(item[PackagingItemListFields.MaterialNumber]), Convert.ToString(item[PackagingItemListFields.PackagingComponent]));
                            packagingNumberList.Add(new KeyValuePair<int, Dictionary<string, string>>(Convert.ToInt32(item[PackagingItemListFields.CompassListItemId]), packagingNumbers));
                        }
                    }
                }
            }
            return packagingNumberList;
        }

        public List<ProjectDetailsItem> getProjectDetailsbyIDs(List<int> iItemIds)
        {
            List<ProjectDetailsItem> projectDetails = new List<ProjectDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    try
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery = new SPQuery();
                        string query = "<Where><In><FieldRef Name=\"ID\" /><Values>";
                        foreach (int compassId in iItemIds)
                        {
                            query += "<Value Type=\"Int\">" + compassId + "</Value>";
                        }
                        query += "</Values></In></Where>";
                        spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup1Brand + "' />",
                                   "<FieldRef Name='" + CompassListFields.Initiator + "' />",
                                   "<FieldRef Name='" + CompassListFields.InitiatorName + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.TimelineType + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />");
                        spQuery.ViewFieldsOnly = true;
                        spQuery.Query = query;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem newItem in compassItemCol)
                            {
                                ProjectDetailsItem projectDetail = new ProjectDetailsItem();

                                projectDetail.CompassItemId = newItem.ID;
                                string projectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                                projectDetail.ProjectNumber = projectNumber;
                                projectDetail.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                                projectDetail.ProjectTypeSubCategory = Convert.ToString(newItem[CompassListFields.ProjectTypeSubCategory]);
                                projectDetail.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                                projectDetail.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                                projectDetail.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                                projectDetail.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                                projectDetail.Initiator = Convert.ToString(newItem[CompassListFields.Initiator]);
                                projectDetail.InitiatorName = Convert.ToString(newItem[CompassListFields.InitiatorName]);
                                projectDetail.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);
                                projectDetail.TimelineType = Convert.ToString(newItem[CompassListFields.TimelineType]);

                                string FGNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                string FGDesc = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                /*if (string.IsNullOrEmpty(FGNumber))
                                {
                                    FGNumber = Convert.ToString(newItem[CompassListFields.LikeFGItemNumber]);
                                }*/
                                if (string.IsNullOrEmpty(FGNumber))
                                {
                                    FGNumber = "XXXXX";
                                }
                                if (string.IsNullOrEmpty(FGDesc))
                                {
                                    FGDesc = Convert.ToString(newItem[CompassListFields.LikeFGItemDescription]);
                                }
                                if (string.IsNullOrEmpty(FGDesc))
                                {
                                    FGDesc = "(Proposed)";
                                }
                                projectDetail.ProjectTitle = projectNumber + ": " + FGNumber + ": " + FGDesc;
                                projectDetails.Add(projectDetail);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getProjectDetailsbyIDs", "itemid :" + iItemIds);
                    }
                }
            }
            return projectDetails;
        }

        public ProjectDetailsItem getProjectDetails(int iItemId)
        {
            ProjectDetailsItem projectDetails = new ProjectDetailsItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    try
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        var newItem = spList.GetItemById(iItemId);
                        if (newItem != null)
                        {
                            projectDetails.CompassItemId = newItem.ID;
                            string projectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                            projectDetails.ProjectNumber = projectNumber;
                            projectDetails.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                            projectDetails.ProjectTypeSubCategory = Convert.ToString(newItem[CompassListFields.ProjectTypeSubCategory]);
                            projectDetails.SAPItemNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                            projectDetails.SAPDescription = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                            projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                            projectDetails.WorkflowPhase = Convert.ToString(newItem[CompassListFields.WorkflowPhase]);
                            projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                            projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                            projectDetails.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                            projectDetails.PackingLocation = Convert.ToString(newItem[CompassListFields.PackingLocation]);
                            projectDetails.PM = Convert.ToString(newItem[CompassListFields.PM]);
                            projectDetails.PackagingEngineer = Convert.ToString(newItem[CompassListFields.PackagingEngineerLead]);
                            projectDetails.Initiator = Convert.ToString(newItem[CompassListFields.Initiator]);
                            projectDetails.InitiatorName = Convert.ToString(newItem[CompassListFields.InitiatorName]);
                            projectDetails.Customer = Convert.ToString(newItem[CompassListFields.Customer]);
                            projectDetails.SubmittedDate = Convert.ToString(newItem[CompassListFields.SubmittedDate]);
                            projectDetails.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);
                            projectDetails.TimelineType = Convert.ToString(newItem[CompassListFields.TimelineType]);

                            string FGNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                            string FGDesc = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                            /*if (string.IsNullOrEmpty(FGNumber))
                            {
                                FGNumber = Convert.ToString(newItem[CompassListFields.LikeFGItemNumber]);
                            }*/
                            if (string.IsNullOrEmpty(FGNumber))
                            {
                                FGNumber = "XXXXX";
                            }
                            if (string.IsNullOrEmpty(FGDesc))
                            {
                                FGDesc = Convert.ToString(newItem[CompassListFields.LikeFGItemDescription]);
                            }
                            if (string.IsNullOrEmpty(FGDesc))
                            {
                                FGDesc = "(Proposed)";
                            }
                            projectDetails.ProjectTitle = projectNumber + ": " + FGNumber + ": " + FGDesc;

                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getProjectDetails", "itemid :" + iItemId);
                    }
                }
            }
            return projectDetails;
        }
        public List<ProjectDetailsItem> getAllProjectDetails(List<int> ids)
        {
            List<ProjectDetailsItem> projectDetailsList = new List<ProjectDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {

                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPView spview = spList.Views[GlobalConstants.VIEW_MyOpenTasksDash];
                    SPQuery spQuery = new SPQuery(spview);
                    string query = "<Where><In><FieldRef Name=\"ID\" LookupId=\"True\" /><Values>";
                    foreach (int id in ids)
                    {
                        query += "<Value Type=\"Integer\">" + id + "</Value>";
                    }
                    query += "</Values></In></Where>";

                    spQuery.Query = query;
                    spQuery.ViewFields = string.Concat(
                                    "<FieldRef Name='ID' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup1Brand + "' />",
                                   "<FieldRef Name='" + CompassListFields.ManufacturingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PM + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackagingEngineerLead + "' />",
                                   "<FieldRef Name='" + CompassListFields.Initiator + "' />",
                                   "<FieldRef Name='" + CompassListFields.InitiatorName + "' />",
                                   "<FieldRef Name='" + CompassListFields.TimelineType + "' />",
                                   "<FieldRef Name='" + CompassListFields.Customer + "' />",
                                   "<FieldRef Name='" + CompassListFields.SubmittedDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.LikeFGItemDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />");
                    spQuery.ViewFieldsOnly = true;

                    SPListItemCollection collection = spList.GetItems(spQuery);
                    try
                    {
                        foreach (SPListItem newItem in collection)
                        {
                            if (newItem != null)
                            {
                                ProjectDetailsItem projectDetails = new ProjectDetailsItem();
                                projectDetails.CompassItemId = newItem.ID;
                                string projectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                                projectDetails.ProjectNumber = projectNumber;
                                projectDetails.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                                projectDetails.ProjectTypeSubCategory = Convert.ToString(newItem[CompassListFields.ProjectTypeSubCategory]);
                                projectDetails.SAPItemNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                projectDetails.SAPDescription = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                                projectDetails.WorkflowPhase = Convert.ToString(newItem[CompassListFields.WorkflowPhase]);
                                projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                                projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                                projectDetails.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                                projectDetails.PackingLocation = Convert.ToString(newItem[CompassListFields.PackingLocation]);
                                projectDetails.PM = Convert.ToString(newItem[CompassListFields.PM]);
                                projectDetails.PackagingEngineer = Convert.ToString(newItem[CompassListFields.PackagingEngineerLead]);
                                projectDetails.Initiator = Convert.ToString(newItem[CompassListFields.Initiator]);
                                projectDetails.InitiatorName = Convert.ToString(newItem[CompassListFields.InitiatorName]);
                                projectDetails.Customer = Convert.ToString(newItem[CompassListFields.Customer]);
                                projectDetails.SubmittedDate = Convert.ToString(newItem[CompassListFields.SubmittedDate]);
                                projectDetails.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);
                                projectDetails.TimelineType = Convert.ToString(newItem[CompassListFields.TimelineType]);

                                string FGNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                string FGDesc = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                /*if (string.IsNullOrEmpty(FGNumber))
                                {
                                    FGNumber = Convert.ToString(newItem[CompassListFields.LikeFGItemNumber]);
                                }*/
                                if (string.IsNullOrEmpty(FGNumber))
                                {
                                    FGNumber = "XXXXX";
                                }
                                if (string.IsNullOrEmpty(FGDesc))
                                {
                                    FGDesc = Convert.ToString(newItem[CompassListFields.LikeFGItemDescription]);
                                }
                                if (string.IsNullOrEmpty(FGDesc))
                                {
                                    FGDesc = "(Proposed)";
                                }
                                projectDetails.ProjectTitle = projectNumber + ": " + FGNumber + ": " + FGDesc;
                                projectDetailsList.Add(projectDetails);

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getAllProjectDetails");
                    }
                }

            }
            return projectDetailsList;
        }
        public List<ProjectTimelineItem> GetProjectsItem(List<int> ids)
        {
            List<ProjectTimelineItem> updatedTimesList = new List<ProjectTimelineItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                    SPQuery spQuery = new SPQuery();
                    string query = "<Where><In><FieldRef Name=\"compassListItemId\" LookupId=\"True\" /><Values>";
                    foreach (int id in ids)
                    {
                        query += "<Value Type=\"Integer\">" + id + "</Value>";
                    }
                    query += "</Values></In></Where>";
                    spQuery.Query = query;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    foreach (SPListItem workflowItems in compassItemCol)
                    {
                        if (workflowItems != null)
                        {
                            ProjectTimelineItem updatedTimes = new ProjectTimelineItem();
                            try
                            {
                                updatedTimes.CompassListItemId = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.compassListItemId]);
                                updatedTimes.IPF = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.IPF_Planned]);
                                updatedTimes.BOMSetupPE3 = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.BOMSetupPE3_Planned]);
                                updatedTimes.BOMSetupPE2 = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.BOMSetupPE2_Planned]);
                                updatedTimes.BOMSetupPE = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.BOMSetupPE_Planned]);
                                updatedTimes.BOMSetupProc = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.BOMSetupProc_Planned]);
                                updatedTimes.CostingQuote = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.CostingQuote]);
                                updatedTimes.Distribution = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.Distribution_Planned]);
                                updatedTimes.ExternalMfg = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.ExternalMfg_Planned]);
                                updatedTimes.FGPackSpec = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.FGPackSpec]);
                                updatedTimes.GRAPHICS = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.GRAPHICS_Planned]);
                                updatedTimes.OBMReview1 = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.OBMReview1_Planned]);
                                updatedTimes.OBMReview2 = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.OBMReview2_Planned]);
                                updatedTimes.Operations = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.Operations_Planned]);
                                updatedTimes.QA = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.QA_Planned]);
                                updatedTimes.SAPBOMSetup = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SAPBOMSetup_Planned]);
                                updatedTimes.SAPInitialSetup = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SAPInitialSetup_Planned]);
                                updatedTimes.PrelimSAPInitialSetup = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.PrelimSAPInitialSetup_Planned]);
                                updatedTimes.SrOBMApproval2 = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SrOBMApproval2_Planned]);
                                updatedTimes.SrOBMApproval = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SrOBMApproval_Planned]);
                                updatedTimes.TradePromo = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.TradePromo_Planned]);
                                updatedTimes.EstPricing = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.EstPricing_Planned]);
                                updatedTimes.EstBracketPricing = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.EstBracketPricing_Planned]);
                                updatedTimes.SAPRoutingSetup = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SAPRoutingSetup_Planned]);
                                updatedTimes.BOMActiveDate = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.BOMActiveDate]);
                                updatedTimes.SAPCostingDetails = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SAPCostingDetails]);
                                updatedTimes.SAPWarehouseInfo = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SAPWarehouseInfo]);
                                updatedTimes.StandardCostEntry = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.StandardCostEntry]);
                                updatedTimes.CostFinishedGood = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.CostFinishedGood]);
                                updatedTimes.FinalCostingReview = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.FinalCostingReview]);
                                updatedTimes.PurchasedPO = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.PurchasedPO]);
                                updatedTimes.RemoveSAPBlocks = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.RemoveSAPBlocks]);
                                updatedTimes.CustomerPO = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.CustomerPO]);
                                updatedTimes.MaterialsRcvdChk = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.MaterialsRcvdChk]);
                                updatedTimes.FirstProductionChk = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.FirstProductionChk]);
                                updatedTimes.DistributionChk = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.DistributionChk]);
                                updatedTimes.FCST = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.FCST]);
                                updatedTimes.MaterialWarehouseSetUp = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.MaterialWarehouseSetUp_Planned]);
                                updatedTimes.SAPCompleteItemSetup = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.SAPCompleteItemSetup_Planned]);
                                updatedTimes.BEQRC = Convert.ToInt32(workflowItems[ProjectTimelineUpdateFields.BEQRC_Planned]);
                            }
                            catch (Exception e)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, e, "DashboardService", "GetProjectsItem", "itemid :" + workflowItems.ID);
                            }
                            updatedTimesList.Add(updatedTimes);
                        }
                    }
                }
            }
            return updatedTimesList;
        }
        public List<TimelineTypeItem> GetWorkflowStepItems()
        {
            var newItem = new List<TimelineTypeItem>();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
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
                                obTimelineTypeItem.Ludicrous = Convert.ToInt32(item[ProjectTimelineTypeDays.Ludicrous]);
                                obTimelineTypeItem.Expedited = Convert.ToInt32(item[ProjectTimelineTypeDays.Expedited]);
                                obTimelineTypeItem.Standard = Convert.ToInt32(item[ProjectTimelineTypeDays.Standard]);

                                newItem.Add(obTimelineTypeItem);

                            }
                        }
                    }
                }
            });
            return newItem;
        }
        public async Task<ProjectDetailsItem> getProjectDetailsAsync(int iItemId)
        {
            ProjectDetailsItem projectDetails = new ProjectDetailsItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    try
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        var newItem = spList.GetItemById(iItemId);
                        if (newItem != null)
                        {
                            projectDetails.CompassItemId = newItem.ID;
                            string projectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                            projectDetails.ProjectNumber = projectNumber;
                            projectDetails.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                            projectDetails.SAPItemNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                            projectDetails.SAPDescription = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                            projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                            projectDetails.WorkflowPhase = Convert.ToString(newItem[CompassListFields.WorkflowPhase]);
                            projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                            projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                            projectDetails.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                            projectDetails.PackingLocation = Convert.ToString(newItem[CompassListFields.PackingLocation]);
                            projectDetails.PM = Convert.ToString(newItem[CompassListFields.PM]);
                            projectDetails.PackagingEngineer = Convert.ToString(newItem[CompassListFields.PackagingEngineerLead]);
                            projectDetails.Initiator = Convert.ToString(newItem[CompassListFields.Initiator]);
                            projectDetails.InitiatorName = Convert.ToString(newItem[CompassListFields.InitiatorName]);
                            projectDetails.Customer = Convert.ToString(newItem[CompassListFields.Customer]);
                            projectDetails.SubmittedDate = Convert.ToString(newItem[CompassListFields.SubmittedDate]);
                            projectDetails.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);

                            string FGNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                            string FGDesc = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                            /*if (string.IsNullOrEmpty(FGNumber))
                            {
                                FGNumber = Convert.ToString(newItem[CompassListFields.LikeFGItemNumber]);
                            }*/
                            if (string.IsNullOrEmpty(FGNumber))
                            {
                                FGNumber = "XXXXX";
                            }
                            if (string.IsNullOrEmpty(FGDesc))
                            {
                                FGDesc = Convert.ToString(newItem[CompassListFields.LikeFGItemDescription]);
                            }
                            if (string.IsNullOrEmpty(FGDesc))
                            {
                                FGDesc = "(Proposed)";
                            }
                            projectDetails.ProjectTitle = projectNumber + ": " + FGNumber + ": " + FGDesc;

                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getProjectDetails", "itemid :" + iItemId);
                    }
                }
            }
            return await Task.FromResult(projectDetails);
        }
        public WorldSyncReqTask getWorldSyncRequestItem(int requestId)
        {
            WorldSyncReqTask request = new WorldSyncReqTask();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);
                    var item = spList.GetItemById(requestId);
                    if (item != null)
                    {
                        request.RequestId = item.ID;
                        request.RequestStatus = Convert.ToString(item[WorldSyncRequestFields.RequestStatus]);
                        request.RequestType = Convert.ToString(item[WorldSyncRequestFields.RequestType]);
                        request.SAPnumber = Convert.ToString(item[WorldSyncRequestFields.SAPnumber]);
                        request.SAPdescription = Convert.ToString(item[WorldSyncRequestFields.SAPdescription]);
                        request.WorkflowStep = Convert.ToString(item[WorldSyncRequestFields.WorkflowStep]);
                        request.SubmittedDate = Convert.ToDateTime(item["Created"]);
                    }
                }
            }
            return request;
        }
        public async Task<List<ProjectDetailsItem>> getRequestedProjectDetailsAsync(string queryDetails)
        {
            List<ProjectDetailsItem> projectDetailsList = new List<ProjectDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = queryDetails;
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup1Brand + "' />",
                                   "<FieldRef Name='" + CompassListFields.ManufacturingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PM + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackagingEngineerLead + "' />",
                                   "<FieldRef Name='" + CompassListFields.Initiator + "' />",
                                   "<FieldRef Name='" + CompassListFields.InitiatorName + "' />",
                                   "<FieldRef Name='" + CompassListFields.TimelineType + "' />",
                                   "<FieldRef Name='" + CompassListFields.Customer + "' />",
                                   "<FieldRef Name='" + CompassListFields.SubmittedDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.StageGateProjectListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.ParentProjectNumber + "' />");
                    spQuery.ViewFieldsOnly = true;
                    try
                    {
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem newItem in compassItemCol)
                            {
                                if (newItem != null)
                                {
                                    ProjectDetailsItem projectDetails = new ProjectDetailsItem();
                                    projectDetails.ProjectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                                    projectDetails.CompassItemId = newItem.ID;
                                    projectDetails.SAPItemNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                    projectDetails.SAPDescription = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                    projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                                    projectDetails.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);
                                    projectDetails.WorkflowPhase = Convert.ToString(newItem[CompassListFields.WorkflowPhase]);
                                    projectDetails.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                                    projectDetails.ProjectTypeSubCategory = Convert.ToString(newItem[CompassListFields.ProjectTypeSubCategory]);
                                    projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                                    projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                                    projectDetails.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                                    projectDetails.PackingLocation = Convert.ToString(newItem[CompassListFields.PackingLocation]);

                                    projectDetails.PM = cleanName(Convert.ToString(newItem[CompassListFields.PM]));
                                    projectDetails.PackagingEngineer = cleanName(Convert.ToString(newItem[CompassListFields.PackagingEngineerLead]));
                                    projectDetails.Initiator = cleanName(Convert.ToString(newItem[CompassListFields.Initiator]));
                                    projectDetails.InitiatorName = cleanName(Convert.ToString(newItem[CompassListFields.InitiatorName]));
                                    projectDetails.TimelineType = cleanName(Convert.ToString(newItem[CompassListFields.TimelineType]));
                                    projectDetails.Customer = Convert.ToString(newItem[CompassListFields.Customer]);
                                    projectDetails.SubmittedDate = Convert.ToString(newItem[CompassListFields.SubmittedDate]);
                                    projectDetails.Parent = GlobalConstants.CONST_No;
                                    projectDetails.StageGateProjectListItemId = Convert.ToInt32(newItem[CompassListFields.StageGateProjectListItemId]);
                                    projectDetails.ParentProjectNumber = Convert.ToString(newItem[CompassListFields.ParentProjectNumber]);

                                    projectDetailsList.Add(projectDetails);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getRequestedProjectDetailsAsync", ex.Message);
                    }
                }
            }
            return await Task.FromResult(projectDetailsList);
        }
        public List<ProjectDetailsItem> getRequestedProjectDetails(string queryDetails)
        {
            List<ProjectDetailsItem> projectDetailsList = new List<ProjectDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = queryDetails;
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup1Brand + "' />",
                                   "<FieldRef Name='" + CompassListFields.ManufacturingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PM + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackagingEngineerLead + "' />",
                                   "<FieldRef Name='" + CompassListFields.Initiator + "' />",
                                   "<FieldRef Name='" + CompassListFields.InitiatorName + "' />",
                                   "<FieldRef Name='" + CompassListFields.TimelineType + "' />",
                                   "<FieldRef Name='" + CompassListFields.Customer + "' />",
                                   "<FieldRef Name='" + CompassListFields.SubmittedDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.StageGateProjectListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.ParentProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.LikeFGItemDescription + "' />");

                    spQuery.ViewFieldsOnly = true;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem newItem in compassItemCol)
                        {
                            try
                            {
                                if (newItem != null)
                                {
                                    ProjectDetailsItem projectDetails = new ProjectDetailsItem();
                                    projectDetails.ProjectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                                    projectDetails.CompassItemId = newItem.ID;
                                    projectDetails.SAPItemNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                    projectDetails.SAPDescription = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                    projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                                    projectDetails.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);
                                    projectDetails.WorkflowPhase = Convert.ToString(newItem[CompassListFields.WorkflowPhase]);
                                    projectDetails.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                                    projectDetails.ProjectTypeSubCategory = Convert.ToString(newItem[CompassListFields.ProjectTypeSubCategory]);
                                    projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                                    projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                                    projectDetails.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                                    projectDetails.PackingLocation = Convert.ToString(newItem[CompassListFields.PackingLocation]);

                                    projectDetails.PM = cleanName(Convert.ToString(newItem[CompassListFields.PM]));
                                    projectDetails.PackagingEngineer = cleanName(Convert.ToString(newItem[CompassListFields.PackagingEngineerLead]));
                                    projectDetails.Initiator = cleanName(Convert.ToString(newItem[CompassListFields.Initiator]));
                                    projectDetails.InitiatorName = cleanName(Convert.ToString(newItem[CompassListFields.InitiatorName]));
                                    projectDetails.TimelineType = cleanName(Convert.ToString(newItem[CompassListFields.TimelineType]));
                                    projectDetails.Customer = Convert.ToString(newItem[CompassListFields.Customer]);
                                    projectDetails.SubmittedDate = Convert.ToString(newItem[CompassListFields.SubmittedDate]);
                                    projectDetails.Parent = GlobalConstants.CONST_No;
                                    projectDetails.StageGateProjectListItemId = Convert.ToInt32(newItem[CompassListFields.StageGateProjectListItemId]);
                                    projectDetails.ParentProjectNumber = Convert.ToString(newItem[CompassListFields.ParentProjectNumber]);
                                    string FGNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                    string FGDesc = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                    /*if (string.IsNullOrEmpty(FGNumber))
                                    {
                                        FGNumber = Convert.ToString(newItem[CompassListFields.LikeFGItemNumber]);
                                    }*/
                                    if (string.IsNullOrEmpty(FGNumber))
                                    {
                                        FGNumber = "XXXXX";
                                    }
                                    if (string.IsNullOrEmpty(FGDesc))
                                    {
                                        FGDesc = Convert.ToString(newItem[CompassListFields.LikeFGItemDescription]);
                                    }
                                    if (string.IsNullOrEmpty(FGDesc))
                                    {
                                        FGDesc = "(Proposed)";
                                    }
                                    projectDetails.ProjectTitle = Convert.ToString(newItem[CompassListFields.ProjectNumber]) + ": " + FGNumber + ": " + FGDesc;
                                    projectDetailsList.Add(projectDetails);
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getRequestedProjectDetails", ex.Message);
                            }
                        }
                    }
                }
            }
            return projectDetailsList;
        }
        public List<ProjectDetailsItem> getRequestedProjectDetails2(string queryDetails)
        {
            List<ProjectDetailsItem> projectDetailsList = new List<ProjectDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = queryDetails;
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup1Brand + "' />",
                                   "<FieldRef Name='" + CompassListFields.ManufacturingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PM + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackagingEngineerLead + "' />",
                                   "<FieldRef Name='" + CompassListFields.Initiator + "' />",
                                   "<FieldRef Name='" + CompassListFields.InitiatorName + "' />",
                                   "<FieldRef Name='" + CompassListFields.TimelineType + "' />",
                                   "<FieldRef Name='" + CompassListFields.Customer + "' />",
                                   "<FieldRef Name='" + CompassListFields.SubmittedDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.StageGateProjectListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.ParentProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.LikeFGItemDescription + "' />");

                    spQuery.ViewFieldsOnly = true;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem newItem in compassItemCol)
                        {
                            try
                            {
                                if (newItem != null)
                                {
                                    ProjectDetailsItem projectDetails = new ProjectDetailsItem();
                                    projectDetails.ProjectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                                    projectDetails.CompassItemId = newItem.ID;
                                    projectDetails.SAPItemNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                    projectDetails.SAPDescription = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                    projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                                    projectDetails.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);
                                    projectDetails.WorkflowPhase = Convert.ToString(newItem[CompassListFields.WorkflowPhase]);
                                    projectDetails.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                                    projectDetails.ProjectTypeSubCategory = Convert.ToString(newItem[CompassListFields.ProjectTypeSubCategory]);
                                    projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                                    projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                                    projectDetails.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                                    projectDetails.PackingLocation = Convert.ToString(newItem[CompassListFields.PackingLocation]);

                                    projectDetails.PM = cleanName(Convert.ToString(newItem[CompassListFields.PM]));
                                    projectDetails.PackagingEngineer = cleanName(Convert.ToString(newItem[CompassListFields.PackagingEngineerLead]));
                                    projectDetails.Initiator = cleanName(Convert.ToString(newItem[CompassListFields.Initiator]));
                                    projectDetails.InitiatorName = cleanName(Convert.ToString(newItem[CompassListFields.InitiatorName]));
                                    projectDetails.TimelineType = cleanName(Convert.ToString(newItem[CompassListFields.TimelineType]));
                                    projectDetails.Customer = Convert.ToString(newItem[CompassListFields.Customer]);
                                    projectDetails.SubmittedDate = Convert.ToString(newItem[CompassListFields.SubmittedDate]);
                                    projectDetails.Parent = GlobalConstants.CONST_No;
                                    projectDetails.StageGateProjectListItemId = Convert.ToInt32(newItem[CompassListFields.StageGateProjectListItemId]);
                                    projectDetails.ParentProjectNumber = Convert.ToString(newItem[CompassListFields.ParentProjectNumber]);
                                    string FGNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                    string FGDesc = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                    /*if (string.IsNullOrEmpty(FGNumber))
                                    {
                                        FGNumber = Convert.ToString(newItem[CompassListFields.LikeFGItemNumber]);
                                    }*/
                                    if (string.IsNullOrEmpty(FGNumber))
                                    {
                                        FGNumber = "XXXXX";
                                    }
                                    if (string.IsNullOrEmpty(FGDesc))
                                    {
                                        FGDesc = Convert.ToString(newItem[CompassListFields.LikeFGItemDescription]);
                                    }
                                    if (string.IsNullOrEmpty(FGDesc))
                                    {
                                        FGDesc = "(Proposed)";
                                    }
                                    projectDetails.ProjectTitle = Convert.ToString(newItem[CompassListFields.ProjectNumber]) + ": " + FGNumber + ": " + FGDesc;
                                    projectDetailsList.Add(projectDetails);

                                    #region Packaging items
                                    SPList spPackagingitemsList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                                    SPQuery spPackagingitemsQuery = new SPQuery();
                                    string query = "<Where><In><FieldRef Name=\"CompassListItemId\" /><Values>";
                                    query += "<Value Type=\"Int\">" + projectDetails.CompassItemId + "</Value>";
                                    query += "</Values></In></Where>";
                                    spPackagingitemsQuery.ViewFields = string.Concat(
                                                   "<FieldRef Name='" + PackagingItemListFields.MaterialNumber + "' />",
                                                   "<FieldRef Name='" + PackagingItemListFields.Deleted + "' />",
                                                   "<FieldRef Name='" + PackagingItemListFields.PackagingComponent + "' />",
                                                   "<FieldRef Name='" + PackagingItemListFields.CompassListItemId + "' />");
                                    spPackagingitemsQuery.ViewFieldsOnly = true;
                                    spPackagingitemsQuery.Query = query;

                                    SPListItemCollection PackagingitemsCol = spPackagingitemsList.GetItems(spPackagingitemsQuery);
                                    if (PackagingitemsCol.Count > 0)
                                    {
                                        string PackagingNumbers = "";
                                        foreach (SPListItem Packagingitem in PackagingitemsCol)
                                        {
                                            if (string.Equals(Packagingitem[PackagingItemListFields.Deleted], "Yes"))
                                                continue;

                                            PackagingNumbers += Convert.ToString(Packagingitem[PackagingItemListFields.MaterialNumber]) + ": " + Convert.ToString(Packagingitem[PackagingItemListFields.PackagingComponent]) + "; ";
                                        }
                                        projectDetails.PackagingNumbers = PackagingNumbers;
                                    }
                                    #endregion
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getRequestedProjectDetails", ex.Message);
                            }
                        }
                    }
                }
            }
            return projectDetailsList;
        }
        public List<ProjectDetailsItem> getLimitedRequestedProjectDetails(string queryDetails)
        {
            List<ProjectDetailsItem> projectDetailsList = new List<ProjectDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = queryDetails;
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup1Brand + "' />",
                                   "<FieldRef Name='" + CompassListFields.ManufacturingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackingLocation + "' />",
                                   "<FieldRef Name='" + CompassListFields.PM + "' />",
                                   "<FieldRef Name='" + CompassListFields.PackagingEngineerLead + "' />",
                                   "<FieldRef Name='" + CompassListFields.Initiator + "' />",
                                   "<FieldRef Name='" + CompassListFields.InitiatorName + "' />",
                                   "<FieldRef Name='" + CompassListFields.TimelineType + "' />",
                                   "<FieldRef Name='" + CompassListFields.Customer + "' />",
                                   "<FieldRef Name='" + CompassListFields.SubmittedDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.StageGateProjectListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.ParentProjectNumber + "' />");
                    spQuery.ViewFieldsOnly = true;
                    spQuery.RowLimit = 100;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem newItem in compassItemCol)
                        {
                            try
                            {
                                if (newItem != null)
                                {
                                    ProjectDetailsItem projectDetails = new ProjectDetailsItem();
                                    projectDetails.ProjectNumber = Convert.ToString(newItem[CompassListFields.ProjectNumber]);
                                    projectDetails.CompassItemId = newItem.ID;
                                    projectDetails.SAPItemNumber = Convert.ToString(newItem[CompassListFields.SAPItemNumber]);
                                    projectDetails.SAPDescription = Convert.ToString(newItem[CompassListFields.SAPDescription]);
                                    projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[CompassListFields.RevisedFirstShipDate]);
                                    projectDetails.FirstProduction = Convert.ToString(newItem[CompassListFields.FirstProductionDate]);
                                    projectDetails.WorkflowPhase = Convert.ToString(newItem[CompassListFields.WorkflowPhase]);
                                    projectDetails.ProjectType = Convert.ToString(newItem[CompassListFields.ProjectType]);
                                    projectDetails.ProjectTypeSubCategory = Convert.ToString(newItem[CompassListFields.ProjectTypeSubCategory]);
                                    projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[CompassListFields.ProductHierarchyLevel1]);
                                    projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[CompassListFields.MaterialGroup1Brand]);
                                    projectDetails.ManufacturingLocation = Convert.ToString(newItem[CompassListFields.ManufacturingLocation]);
                                    projectDetails.PackingLocation = Convert.ToString(newItem[CompassListFields.PackingLocation]);

                                    projectDetails.PM = cleanName(Convert.ToString(newItem[CompassListFields.PM]));
                                    projectDetails.PackagingEngineer = cleanName(Convert.ToString(newItem[CompassListFields.PackagingEngineerLead]));
                                    projectDetails.Initiator = cleanName(Convert.ToString(newItem[CompassListFields.Initiator]));
                                    projectDetails.InitiatorName = cleanName(Convert.ToString(newItem[CompassListFields.InitiatorName]));
                                    projectDetails.TimelineType = cleanName(Convert.ToString(newItem[CompassListFields.TimelineType]));
                                    projectDetails.Customer = Convert.ToString(newItem[CompassListFields.Customer]);
                                    projectDetails.SubmittedDate = Convert.ToString(newItem[CompassListFields.SubmittedDate]);
                                    projectDetails.Parent = GlobalConstants.CONST_No;
                                    projectDetails.StageGateProjectListItemId = Convert.ToInt32(newItem[CompassListFields.StageGateProjectListItemId]);
                                    projectDetails.ParentProjectNumber = Convert.ToString(newItem[CompassListFields.ParentProjectNumber]);

                                    projectDetailsList.Add(projectDetails);
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "getRequestedProjectDetails", ex.Message);
                            }
                        }
                    }
                }
            }
            return projectDetailsList;
        }

        public List<ItemProposalItem> getRequestedChildProjectDetails(int parentID)
        {
            List<ItemProposalItem> ChildProjectDetailsList = new List<ItemProposalItem>();
            string brand = "";
            string phl2 = "";
            string Stage = "";
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                    var item2 = spList2.GetItemById(parentID);
                    try
                    {
                        if (item2 != null)
                        {
                            brand = Convert.ToString(item2[StageGateProjectListFields.Brand]);
                            phl2 = Convert.ToString(item2[StageGateProjectListFields.ProductHierarchyL2]);
                            Stage = Convert.ToString(item2[StageGateProjectListFields.Stage]);

                        }
                    }
                    catch (Exception e)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, e, "getRequestedChildProjectDetails", "ParentID: " + parentID, e.Message);
                    }
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSChildProjectTempList);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"ParentID\" /><Value Type=\"Integer\">" + parentID + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (item != null)
                            {
                                try
                                {
                                    ItemProposalItem newItem = new ItemProposalItem();
                                    newItem.CompassListItemId = 0;
                                    newItem.StageGateProjectListItemId = Convert.ToInt32(item[SGSChildProjectTempListFields.ParentID]);
                                    newItem.TBDIndicator = Convert.ToString(item[SGSChildProjectTempListFields.TBDIndicator]);
                                    newItem.ProjectNumber = Convert.ToString(item.ID);
                                    newItem.SAPItemNumber = Convert.ToString(item[SGSChildProjectTempListFields.FinishedGood]);
                                    newItem.SAPDescription = Convert.ToString(item[SGSChildProjectTempListFields.Description]);
                                    newItem.ProductHierarchyLevel1 = Convert.ToString(item[SGSChildProjectTempListFields.ProductHierarchy1]);
                                    newItem.ManuallyCreateSAPDescription = Convert.ToString(item[SGSChildProjectTempListFields.ManuallyCreateSAPDescription]);

                                    newItem.MaterialGroup4ProductForm = Convert.ToString(item[SGSChildProjectTempListFields.ProductMaterialGroup4]);
                                    newItem.MaterialGroup5PackType = Convert.ToString(item[SGSChildProjectTempListFields.PackTypeMaterialGroup5]);
                                    newItem.RequireNewUPCUCC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewUPCUCC]);
                                    newItem.RequireNewUnitUPC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewUnitUPC]);
                                    newItem.UnitUPC = Convert.ToString(item[SGSChildProjectTempListFields.UnitUPC]);
                                    newItem.RequireNewDisplayBoxUPC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewDisplayBoxUPC]);
                                    newItem.DisplayBoxUPC = Convert.ToString(item[SGSChildProjectTempListFields.DisplayBoxUPC]);
                                    newItem.RequireNewCaseUCC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewCaseUCC]);
                                    newItem.CaseUCC = Convert.ToString(item[SGSChildProjectTempListFields.CaseUCC]);
                                    newItem.RequireNewPalletUCC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewPalletUCC]);
                                    newItem.PalletUCC = Convert.ToString(item[SGSChildProjectTempListFields.PalletUCC]);
                                    newItem.SAPBaseUOM = Convert.ToString(item[SGSChildProjectTempListFields.SAPBaseUOM]);

                                    newItem.CustomerSpecific = Convert.ToString(item[SGSChildProjectTempListFields.CustomerSpecific]);
                                    newItem.Customer = Convert.ToString(item[SGSChildProjectTempListFields.Customer]);
                                    newItem.Channel = Convert.ToString(item[SGSChildProjectTempListFields.Channel]);
                                    newItem.GenerateIPFSortOrder = Convert.ToInt32(item[SGSChildProjectTempListFields.GenerateIPFSortOrder]);
                                    string tempBrand = Convert.ToString(item[SGSChildProjectTempListFields.BrandMaterialGroup1]);
                                    string tempPHL2 = Convert.ToString(item[SGSChildProjectTempListFields.ProductHierarchy2]);
                                    if (string.IsNullOrEmpty(tempBrand))
                                    {
                                        newItem.MaterialGroup1Brand = brand;
                                    }
                                    else
                                    {
                                        newItem.MaterialGroup1Brand = tempBrand;
                                    }
                                    if (string.IsNullOrEmpty(tempPHL2))
                                    {
                                        newItem.ProductHierarchyLevel2 = phl2;
                                    }
                                    else
                                    {
                                        newItem.ProductHierarchyLevel2 = tempPHL2;
                                    }
                                    newItem.CreateIPFBtn = true;
                                    newItem.NeedsNewBtn = false;
                                    newItem.SubmittedDate = Convert.ToDateTime(item["Created"]);
                                    newItem.Initiator = Convert.ToString(item["Author"]);
                                    ChildProjectDetailsList.Add(newItem);
                                }
                                catch (Exception e)
                                {
                                    exceptionService.Handle(LogCategory.CriticalError, e, "getRequestedChildProjectDetails", "SGS Temp Child ID: " + item.ID, e.Message);
                                }
                            }

                        }
                    }
                    SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery3 = new SPQuery();
                    spQuery3.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Integer\">" + parentID + "</Value></Eq></Where>";
                    spQuery3.ViewFields = string.Concat(
                                   "<FieldRef Name='" + CompassListFields.TBDIndicator + "' />",
                                   "<FieldRef Name='" + CompassListFields.StageGateProjectListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPItemNumber + "' />",
                                   "<FieldRef Name='" + CompassListFields.CompassListItemId + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.RevisedFirstShipDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.FirstProductionDate + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel1 + "' />",
                                   "<FieldRef Name='" + CompassListFields.ManuallyCreateSAPDescription + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup4ProductForm + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup5PackType + "' />",
                                   "<FieldRef Name='" + CompassListFields.ProductHierarchyLevel2 + "' />",
                                   "<FieldRef Name='" + CompassListFields.MaterialGroup1Brand + "' />",
                                   "<FieldRef Name='" + CompassListFields.RequireNewUPCUCC + "' />",
                                   "<FieldRef Name='" + CompassListFields.RequireNewUnitUPC + "' />",
                                   "<FieldRef Name='" + CompassListFields.UnitUPC + "' />",
                                   "<FieldRef Name='" + CompassListFields.RequireNewDisplayBoxUPC + "' />",
                                   "<FieldRef Name='" + CompassListFields.DisplayBoxUPC + "' />",
                                   "<FieldRef Name='" + CompassListFields.RequireNewCaseUCC + "' />",
                                   "<FieldRef Name='" + CompassListFields.CaseUCC + "' />",
                                   "<FieldRef Name='" + CompassListFields.RequireNewPalletUCC + "' />",
                                   "<FieldRef Name='" + CompassListFields.PalletUCC + "' />",
                                   "<FieldRef Name='" + CompassListFields.SAPBaseUOM + "' />",
                                   "<FieldRef Name='" + CompassListFields.WorkflowPhase + "' />",
                                   "<FieldRef Name='" + CompassListFields.CustomerSpecific + "' />",
                                   "<FieldRef Name='" + CompassListFields.Customer + "' />",
                                   "<FieldRef Name='" + CompassListFields.Channel + "' />",
                                   "<FieldRef Name='" + CompassListFields.ParentProjectNumber + "' />",
                                    "<FieldRef Name='" + CompassListFields.GenerateIPFSortOrder + "' />",
                                    "<FieldRef Name='" + CompassListFields.SubmittedDate + "' />",
                                    "<FieldRef Name='Created' />",
                                    "<FieldRef Name='Author' />");
                    spQuery3.ViewFieldsOnly = true;
                    SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem oldItem in compassItemCol3)
                        {
                            if (oldItem != null)
                            {
                                try
                                {
                                    ItemProposalItem newItem = new ItemProposalItem();
                                    newItem.CompassListItemId = oldItem.ID;
                                    newItem.TBDIndicator = Convert.ToString(oldItem[CompassListFields.TBDIndicator]);
                                    newItem.StageGateProjectListItemId = Convert.ToInt32(oldItem[CompassListFields.StageGateProjectListItemId]);
                                    newItem.WorkflowPhase = Stage;
                                    newItem.ProjectNumber = Convert.ToString(oldItem[CompassListFields.ProjectNumber]);
                                    newItem.SAPItemNumber = Convert.ToString(oldItem[CompassListFields.SAPItemNumber]);
                                    newItem.SAPDescription = Convert.ToString(oldItem[CompassListFields.SAPDescription]);
                                    newItem.ProductHierarchyLevel1 = Convert.ToString(oldItem[CompassListFields.ProductHierarchyLevel1]);
                                    newItem.ManuallyCreateSAPDescription = Convert.ToString(oldItem[CompassListFields.ManuallyCreateSAPDescription]);
                                    newItem.ProductHierarchyLevel2 = Convert.ToString(oldItem[CompassListFields.ProductHierarchyLevel2]);
                                    newItem.MaterialGroup4ProductForm = Convert.ToString(oldItem[CompassListFields.MaterialGroup4ProductForm]);
                                    newItem.MaterialGroup5PackType = Convert.ToString(oldItem[CompassListFields.MaterialGroup5PackType]);
                                    newItem.RequireNewUPCUCC = Convert.ToString(oldItem[CompassListFields.RequireNewUPCUCC]);
                                    newItem.RequireNewUnitUPC = Convert.ToString(oldItem[CompassListFields.RequireNewUnitUPC]);
                                    newItem.UnitUPC = Convert.ToString(oldItem[CompassListFields.UnitUPC]);
                                    newItem.RequireNewDisplayBoxUPC = Convert.ToString(oldItem[CompassListFields.RequireNewDisplayBoxUPC]);
                                    newItem.DisplayBoxUPC = Convert.ToString(oldItem[CompassListFields.DisplayBoxUPC]);
                                    newItem.RequireNewCaseUCC = Convert.ToString(oldItem[CompassListFields.RequireNewCaseUCC]);
                                    newItem.CaseUCC = Convert.ToString(oldItem[CompassListFields.CaseUCC]);
                                    newItem.RequireNewPalletUCC = Convert.ToString(oldItem[CompassListFields.RequireNewPalletUCC]);
                                    newItem.PalletUCC = Convert.ToString(oldItem[CompassListFields.PalletUCC]);
                                    newItem.SAPBaseUOM = Convert.ToString(oldItem[CompassListFields.SAPBaseUOM]);
                                    newItem.ProjectStatus = Convert.ToString(oldItem[CompassListFields.WorkflowPhase]);

                                    newItem.CustomerSpecific = Convert.ToString(oldItem[CompassListFields.CustomerSpecific]);
                                    newItem.Customer = Convert.ToString(oldItem[CompassListFields.Customer]);
                                    newItem.Channel = Convert.ToString(oldItem[CompassListFields.Channel]);
                                    newItem.GenerateIPFSortOrder = Convert.ToInt32(oldItem[CompassListFields.GenerateIPFSortOrder]);
                                    newItem.MaterialGroup1Brand = Convert.ToString(oldItem[CompassListFields.MaterialGroup1Brand]);
                                    newItem.CreateIPFBtn = false;
                                    if (newItem.TBDIndicator == "Yes")
                                    {
                                        newItem.NeedsNewBtn = PrelimRun(spWeb, oldItem.ID);
                                    }
                                    else
                                    {
                                        newItem.NeedsNewBtn = false;
                                    }
                                    DateTime submitted = Convert.ToDateTime(oldItem[CompassListFields.SubmittedDate]);
                                    if (submitted == null || submitted == DateTime.MinValue)
                                    {
                                        newItem.SubmittedDate = Convert.ToDateTime(oldItem["Created"]);
                                        try
                                        {
                                            SPFieldUserValue userValue = new SPFieldUserValue(spWeb, oldItem["Author"].ToString());
                                            if (userValue != null)
                                            {
                                                SPUser user = userValue.User;
                                                newItem.Initiator = user.Name;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            exceptionService.Handle(LogCategory.CriticalError, ex, "getRequestedChildProjectDetails", "Error on Created by of ParentID: " + parentID, ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        try
                                        {
                                            SPList spList4 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                                            SPQuery spQuery4 = new SPQuery();
                                            spQuery4.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + oldItem.ID + "</Value></Eq></Where>";
                                            spQuery4.RowLimit = 1;
                                            spQuery4.ViewFields = string.Concat(
                                   "<FieldRef Name='" + ApprovalListFields.IPF_SubmittedDate + "' />",
                                   "<FieldRef Name='" + ApprovalListFields.IPF_SubmittedBy + "' />");
                                            spQuery4.ViewFieldsOnly = true;

                                            SPListItemCollection compassItemCol4 = spList4.GetItems(spQuery4);
                                            if (compassItemCol4.Count > 0)
                                            {
                                                SPListItem item4 = compassItemCol4[0];
                                                if (item4 != null)
                                                {
                                                    newItem.SubmittedDate = Convert.ToDateTime(item4[ApprovalListFields.IPF_SubmittedDate]);
                                                    newItem.Initiator = Convert.ToString(item4[ApprovalListFields.IPF_SubmittedBy]);
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            newItem.SubmittedDate = DateTime.MinValue;
                                            newItem.Initiator = "";
                                        }
                                    }

                                    ChildProjectDetailsList.Add(newItem);
                                }
                                catch (Exception e)
                                {
                                    exceptionService.Handle(LogCategory.CriticalError, e, "getRequestedChildProjectDetails", "Compass Child ID: " + oldItem.ID, e.Message);
                                }
                            }
                        }
                    }
                }
            }
            return ChildProjectDetailsList;
        }
        public bool PrelimRun(SPWeb spWeb, int CompassListItemId)
        {
            bool wasPrelimRun = false;

            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + CompassListItemId + "</Value></Eq></Where>";
            spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + ApprovalListFields.PrelimSAPInitialSetup_StartDate + "' />");
            spQuery.ViewFieldsOnly = true;
            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
            if (compassItemCol.Count > 0 && compassItemCol[0] != null)
            {
                SPListItem item = compassItemCol[0];
                string prelimStartDate = Convert.ToString(item[ApprovalListFields.PrelimSAPInitialSetup_StartDate]);
                if (!string.IsNullOrEmpty(prelimStartDate))
                {
                    wasPrelimRun = true;
                }
            }
            return wasPrelimRun;
        }
        public List<ProjectDetailsItem> getStageGateRequestedProjectDetails(string queryDetails)
        {
            List<ProjectDetailsItem> projectDetailsList = new List<ProjectDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = queryDetails;
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + StageGateProjectListFields.ProjectNumber + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.ProjectName + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.ProjectType + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.ProjectTypeSubCategory + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.RevisedShipDate + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.Stage + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.LineOfBusiness + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.DesiredShipDate + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.Brand + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.Gate0ApprovedDate + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.SKUs + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.ProjectManager + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.ProjectLeader + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.ProjectLeaderName + "' />",
                                   "<FieldRef Name='" + StageGateProjectListFields.FormSubmittedDate + "' />");
                    spQuery.ViewFieldsOnly = true;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem newItem in compassItemCol)
                        {
                            if (newItem != null)
                            {
                                ProjectDetailsItem projectDetails = new ProjectDetailsItem();
                                projectDetails.ProjectNumber = Convert.ToString(newItem[StageGateProjectListFields.ProjectNumber]);
                                projectDetails.CompassItemId = newItem.ID;
                                projectDetails.ProjectTitle = Convert.ToString(newItem[StageGateProjectListFields.ProjectName]);
                                projectDetails.ProjectType = Convert.ToString(newItem[StageGateProjectListFields.ProjectType]);
                                projectDetails.ProjectTypeSubCategory = Convert.ToString(newItem[StageGateProjectListFields.ProjectTypeSubCategory]);
                                projectDetails.SAPDescription = Convert.ToString(newItem[StageGateProjectListFields.ProjectName]);
                                projectDetails.RevisedFirstShipDate = Convert.ToString(newItem[StageGateProjectListFields.RevisedShipDate]);
                                projectDetails.WorkflowPhase = Convert.ToString(newItem[StageGateProjectListFields.Stage]);
                                projectDetails.ProductHierarchyLevel1 = Convert.ToString(newItem[StageGateProjectListFields.LineOfBusiness]);
                                projectDetails.MaterialGroup1Brand = Convert.ToString(newItem[StageGateProjectListFields.Brand]);
                                projectDetails.DesiredShipDate = Convert.ToString(newItem[StageGateProjectListFields.DesiredShipDate]);
                                projectDetails.Gate0ApprovedDate = Convert.ToString(newItem[StageGateProjectListFields.Gate0ApprovedDate]);
                                projectDetails.Brand = Convert.ToString(newItem[StageGateProjectListFields.Brand]);
                                projectDetails.SKUs = Convert.ToString(newItem[StageGateProjectListFields.SKUs]);
                                projectDetails.PM = cleanName(Convert.ToString(newItem[StageGateProjectListFields.ProjectManager]));
                                projectDetails.Stage = cleanName(Convert.ToString(newItem[StageGateProjectListFields.Stage]));
                                projectDetails.Initiator = cleanName(Convert.ToString(newItem[StageGateProjectListFields.ProjectLeader]));
                                projectDetails.InitiatorName = cleanName(Convert.ToString(newItem[StageGateProjectListFields.ProjectLeaderName]));
                                projectDetails.SubmittedDate = Convert.ToString(newItem[StageGateProjectListFields.FormSubmittedDate]);
                                projectDetails.Parent = GlobalConstants.CONST_Yes;

                                projectDetailsList.Add(projectDetails);
                            }
                        }
                    }
                }
            }
            return projectDetailsList;
        }
        public List<WorldSyncReqTask> getWorldSyncRequestItems(string queryDetails)
        {
            List<WorldSyncReqTask> requestList = new List<WorldSyncReqTask>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestList);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = queryDetails;
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + WorldSyncRequestFields.RequestStatus + "' />",
                                   "<FieldRef Name='" + WorldSyncRequestFields.RequestType + "' />",
                                   "<FieldRef Name='" + WorldSyncRequestFields.SAPnumber + "' />",
                                   "<FieldRef Name='" + WorldSyncRequestFields.WorkflowStep + "' />",
                                   "<FieldRef Name='" + WorldSyncRequestFields.SAPdescription + "' />",
                                   "<FieldRef Name='Created' />");
                    spQuery.ViewFieldsOnly = true;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (item != null)
                            {
                                WorldSyncReqTask request = new WorldSyncReqTask();
                                request.RequestId = item.ID;
                                request.RequestStatus = Convert.ToString(item[WorldSyncRequestFields.RequestStatus]);
                                request.RequestType = Convert.ToString(item[WorldSyncRequestFields.RequestType]);
                                request.SAPnumber = Convert.ToString(item[WorldSyncRequestFields.SAPnumber]);
                                request.SAPdescription = Convert.ToString(item[WorldSyncRequestFields.SAPdescription]);
                                request.WorkflowStep = Convert.ToString(item[WorldSyncRequestFields.WorkflowStep]);
                                request.SubmittedDate = Convert.ToDateTime(item["Created"]);
                                requestList.Add(request);
                            }
                        }
                    }
                }
            }
            return requestList;
        }
        public List<WorkflowTaskDetailsItem> getWorkflowTaskItems(string userId)
        {
            List<WorkflowTaskDetailsItem> assignedTasks = new List<WorkflowTaskDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName1);
                    SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName2);
                    SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName3);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"AssignedTo\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                    spQuery.RowLimit = 200;
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='AssignedTo' />",
                                   "<FieldRef Name='ProjectNumber' />",
                                   "<FieldRef Name='FormUrl' />",
                                   "<FieldRef Name='WorkflowStep' />",
                                   "<FieldRef Name='Title' />",
                                   "<FieldRef Name='CompassListItemId' />",
                                   "<FieldRef Name='Created' />");
                    spQuery.ViewFieldsOnly = true;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery);
                    SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery);
                    List<SPListItem> combinedLists = new List<SPListItem>();
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem newItem in compassItemCol)
                        {
                            if (newItem != null)
                            {
                                combinedLists.Add(newItem);
                            }
                        }
                    }
                    if (compassItemCol2.Count > 0)
                    {
                        foreach (SPListItem newItem in compassItemCol2)
                        {
                            if (newItem != null)
                            {
                                combinedLists.Add(newItem);
                            }
                        }
                    }
                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem newItem in compassItemCol3)
                        {
                            if (newItem != null)
                            {
                                combinedLists.Add(newItem);
                            }
                        }
                    }
                    if (combinedLists.Count > 0)
                    {
                        foreach (SPListItem newItem in combinedLists)
                        {

                            WorkflowTaskDetailsItem task = new WorkflowTaskDetailsItem();
                            task.AssignedToId = Convert.ToString(newItem["AssignedTo"]); ;
                            task.ProjectNumber = Convert.ToString(newItem["ProjectNumber"]);
                            task.FormUrl = Convert.ToString(newItem["FormUrl"]);
                            task.WorkflowStep = Convert.ToString(newItem["WorkflowStep"]);
                            task.TaskName = Convert.ToString(newItem["Title"]);
                            task.RequestedDate = Convert.ToString(newItem["Created"]);
                            task.CompassId = Convert.ToInt32(newItem["CompassListItemId"]);
                            assignedTasks.Add(task);

                        }
                    }
                }
            }
            return assignedTasks;
        }
        public List<WorkflowTaskDetailsItem> getWorldSyncWorkflowTaskItems(string userId)
        {
            List<WorkflowTaskDetailsItem> assignedTasks = new List<WorkflowTaskDetailsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    try
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncRequestTasks);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"AssignedTo\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq><Neq><FieldRef Name=\"Status\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                        spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='AssignedTo' />",
                                   "<FieldRef Name='ProjectNumber' />",
                                   "<FieldRef Name='FormUrl' />",
                                   "<FieldRef Name='WorkflowStep' />",
                                   "<FieldRef Name='Title' />",
                                   "<FieldRef Name='Created' />");
                        spQuery.ViewFieldsOnly = true;
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem newItem in compassItemCol)
                            {

                                if (newItem != null)
                                {
                                    WorkflowTaskDetailsItem task = new WorkflowTaskDetailsItem();
                                    task.AssignedToId = Convert.ToString(newItem["AssignedTo"]); ;
                                    task.CompassId = Convert.ToInt32(newItem["RequestId"]);
                                    task.FormUrl = Convert.ToString(newItem["FormUrl"]);
                                    task.WorkflowStep = Convert.ToString(newItem["WorkflowStep"]);
                                    task.TaskName = Convert.ToString(newItem["Title"]);
                                    task.RequestedDate = Convert.ToString(newItem["Created"]);
                                    assignedTasks.Add(task);
                                }

                            }
                        }
                    }
                    catch (Exception error)
                    {

                    }
                }
            }
            return assignedTasks;
        }
        public List<KeyValuePair<int, Dictionary<string, string>>> GetNewPackagingNumbersForProject(List<int> compassListItemIds)
        {
            List<KeyValuePair<int, Dictionary<string, string>>> packagingNumberList = new List<KeyValuePair<int, Dictionary<string, string>>>();

            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    try
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        SPQuery spQuery = new SPQuery();
                        string query = "<Where><And><In><FieldRef Name=\"CompassListItemId\" /><Values>";
                        foreach (int compassId in compassListItemIds)
                        {
                            query += "<Value Type=\"Int\">" + compassId + "</Value>";
                        }
                        query += "</Values></In><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq></And></Where>";
                        spQuery.Query = query;
                        spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + PackagingItemListFields.MaterialNumber + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.Deleted + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.PackagingComponent + "' />",
                                   "<FieldRef Name='" + PackagingItemListFields.CompassListItemId + "' />");
                        spQuery.ViewFieldsOnly = true;
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                    continue;

                                Dictionary<string, string> packagingNumbers = new Dictionary<string, string>();
                                packagingNumbers.Add(Convert.ToString(item[PackagingItemListFields.MaterialNumber]), Convert.ToString(item[PackagingItemListFields.PackagingComponent]));
                                packagingNumberList.Add(new KeyValuePair<int, Dictionary<string, string>>(Convert.ToInt32(item[PackagingItemListFields.CompassListItemId]), packagingNumbers));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionService.Handle(LogCategory.CriticalError, ex, "DashboardService", "GetNewPackagingNumbersForProject", ex.Message);
                    }
                }
            }
            return packagingNumberList;
        }
        public string cleanName(string person)
        {
            int index = person.IndexOf("#");
            string cleanName = (index < 0)
                ? person
                : person.Substring(index + 1);
            return cleanName;
        }
        public List<ItemProposalItem> getChildProjectsByProjectTeam(string queryDetails)
        {
            List<ItemProposalItem> ChildProjects = new List<ItemProposalItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = queryDetails;
                    spQuery.RowLimit = 200;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem ipItem in compassItemCol)
                        {
                            if (ipItem != null)
                            {
                                ItemProposalItem ChildProject = new ItemProposalItem();

                                ChildProject.CompassListItemId = Convert.ToInt32(ipItem[CompassListFields.CompassListItemId]);
                                ChildProject.ProjectLeader = Convert.ToString(ipItem[StageGateProjectListFields.ProjectLeader]);
                                ChildProject.ProjectLeaderName = Convert.ToString(ipItem[StageGateProjectListFields.ProjectLeaderName]);
                                ChildProject.SrProjectManager = Convert.ToString(ipItem[StageGateProjectListFields.SeniorProjectManager]);
                                ChildProject.SrProjectManagerName = Convert.ToString(ipItem[StageGateProjectListFields.SeniorProjectManagerName]);
                                ChildProject.QA = Convert.ToString(ipItem[StageGateProjectListFields.QAInnovation]);
                                ChildProject.QAName = Convert.ToString(ipItem[StageGateProjectListFields.QAInnovationName]);
                                ChildProject.InTech = Convert.ToString(ipItem[StageGateProjectListFields.InTech]);
                                ChildProject.InTechName = Convert.ToString(ipItem[StageGateProjectListFields.InTechName]);
                                ChildProject.InTechRegulatory = Convert.ToString(ipItem[StageGateProjectListFields.InTechRegulatory]);
                                ChildProject.InTechRegulatoryName = Convert.ToString(ipItem[StageGateProjectListFields.InTechRegulatoryName]);
                                ChildProject.RegulatoryQA = Convert.ToString(ipItem[StageGateProjectListFields.RegulatoryQA]);
                                ChildProject.RegulatoryQAName = Convert.ToString(ipItem[StageGateProjectListFields.RegulatoryQAName]);
                                ChildProject.PackagingEngineering = Convert.ToString(ipItem[StageGateProjectListFields.PackagingEngineering]);
                                ChildProject.PackagingEngineeringName = Convert.ToString(ipItem[StageGateProjectListFields.PackagingEngineeringName]);
                                ChildProject.SupplyChain = Convert.ToString(ipItem[StageGateProjectListFields.SupplyChain]);
                                ChildProject.SupplyChainName = Convert.ToString(ipItem[StageGateProjectListFields.SupplyChainName]);
                                ChildProject.Finance = Convert.ToString(ipItem[StageGateProjectListFields.Finance]);
                                ChildProject.FinanceName = Convert.ToString(ipItem[StageGateProjectListFields.FinanceName]);
                                ChildProject.Sales = Convert.ToString(ipItem[StageGateProjectListFields.Sales]);
                                ChildProject.SalesName = Convert.ToString(ipItem[StageGateProjectListFields.SalesName]);
                                ChildProject.Manufacturing = Convert.ToString(ipItem[StageGateProjectListFields.Manufacturing]);
                                ChildProject.ManufacturingName = Convert.ToString(ipItem[StageGateProjectListFields.ManufacturingName]);
                                ChildProject.OtherTeamMembers = Convert.ToString(ipItem[StageGateProjectListFields.OtherMember]);
                                ChildProject.OtherTeamMembersName = Convert.ToString(ipItem[StageGateProjectListFields.OtherMemberName]);
                                ChildProject.LifeCycleManagement = Convert.ToString(ipItem[StageGateProjectListFields.LifeCycleManagement]);
                                ChildProject.LifeCycleManagementName = Convert.ToString(ipItem[StageGateProjectListFields.LifeCycleManagementName]);
                                ChildProject.PackagingProcurement = Convert.ToString(ipItem[StageGateProjectListFields.PackagingProcurement]);
                                ChildProject.PackagingProcurementName = Convert.ToString(ipItem[StageGateProjectListFields.PackagingProcurementName]);
                                ChildProject.ExtManufacturingProc = Convert.ToString(ipItem[StageGateProjectListFields.ExtMfgProcurement]);
                                ChildProject.ExtManufacturingProcName = Convert.ToString(ipItem[StageGateProjectListFields.ExtMfgProcurementName]);

                                ChildProjects.Add(ChildProject);
                            }
                        }
                    }
                }
            }
            return ChildProjects;
        }
        public void setGenerateIPFStartDate(int StageGateListItemId)
        {
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                        SPListItem item = spList.GetItemById(StageGateListItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.IPFStartDate] = DateTime.Now.ToString();
                            item[StageGateProjectListFields.IPFSubmitter] = SPContext.Current.Web.CurrentUser;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void moveChildProject(ItemProposalItem IPFItem)
        {
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        //Compass List
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(IPFItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.ProjectNumber] = IPFItem.ProjectNumber;
                            item["Title"] = IPFItem.ProjectNumber;

                            // Set Links
                            SPFieldUrlValue value = new SPFieldUrlValue();
                            value.Description = IPFItem.ProjectNumber;
                            value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_CommercializationItemSummary, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", IPFItem.ProjectNumber);
                            item[CompassListFields.CommercializationLink] = value;

                            value = new SPFieldUrlValue();
                            value.Description = "Copy";
                            value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ItemProposal, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", IPFItem.ProjectNumber, "&IPFForm=Copy");
                            item[CompassListFields.CopyLink] = value;

                            value = new SPFieldUrlValue();
                            value.Description = "Change";
                            value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ItemProposal, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", IPFItem.ProjectNumber, "&IPFForm=Change");
                            item[CompassListFields.ChangeLink] = value;

                            // Workflow Status Link
                            value = new SPFieldUrlValue();
                            value.Description = IPFItem.ProjectNumber;
                            value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ProjectStatus, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", IPFItem.ProjectNumber);
                            item[CompassListFields.WorkflowStatusLink] = value;

                            item[CompassListFields.ParentProjectNumber] = IPFItem.ParentProjectNumber;
                            item[CompassListFields.StageGateProjectListItemId] = IPFItem.StageGateProjectListItemId;
                            item[CompassListFields.GenerateIPFSortOrder] = IPFItem.GenerateIPFSortOrder;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();

                            string strWFTemplateIdCancel = GetSPWFTemplateIdBasedOnName(spList, "PMT Terminate Project Workflows");
                            SPWorkflowAssociation spwaDocLibCancel = spList.WorkflowAssociations[new Guid(strWFTemplateIdCancel)];
                            spSite.WorkflowManager.StartWorkflow(item, spwaDocLibCancel, spwaDocLibCancel.AssociationData, true);
                        }
                        //Compass List 2
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        SPListItemCollection compass2ItemCol = spList.GetItems(spQuery);
                        if (compass2ItemCol.Count > 0)
                        {

                            foreach (SPListItem compass2Item in compass2ItemCol)
                            {
                                if (compass2Item != null)
                                {
                                    compass2Item["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    compass2Item["Editor"] = SPContext.Current.Web.CurrentUser;
                                    compass2Item.Update();
                                }
                            }
                        }

                        //ApprovalList
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ApprovalListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Documents
                        SPDocumentLibrary documentLib = spWeb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", IPFItem.ProjectNumber);
                        if (spWeb.GetFolder(folderUrl).Exists)
                        {
                            SPFolder ProjectFolder = spWeb.GetFolder(folderUrl);
                            ProjectFolder.Item["Name"] = IPFItem.ProjectNumber;
                            ProjectFolder.Update();
                        }
                        //Email Logging List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + EmailLoggingListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Mixes List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + MixesListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Pack Measurements List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassPackMeasurementsFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Packaging Item List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Project Decision List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassProjectDecisionsListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass SAP Approval List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPApprovalListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + SAPApprovalListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Shipper Finished Good List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ShipperFinishedGoodListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Team List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Workflow Status List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorkflowStatusListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassWorkflowStatusListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Workflow Tasks 1
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName1);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["ProjectNumber"] = IPFItem.ProjectNumber;

                                    //swap out formURL
                                    string formURL = Convert.ToString(appItem["FormUrl"]);
                                    int index = formURL.IndexOf("=");
                                    string projectNoSwitch = formURL.Substring(0, index + 1);
                                    projectNoSwitch = projectNoSwitch + IPFItem.ProjectNumber;
                                    appItem["FormUrl"] = projectNoSwitch;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Workflow Tasks 2
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName2);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["ProjectNumber"] = IPFItem.ProjectNumber;

                                    //swap out formURL
                                    string formURL = Convert.ToString(appItem["FormUrl"]);
                                    int index = formURL.IndexOf("=");
                                    string projectNoSwitch = formURL.Substring(0, index + 1);
                                    projectNoSwitch = projectNoSwitch + IPFItem.ProjectNumber;
                                    appItem["FormUrl"] = projectNoSwitch;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Compass Workflow Tasks 3
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorkflowTaskListName3);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["ProjectNumber"] = IPFItem.ProjectNumber;

                                    //swap out formURL
                                    string formURL = Convert.ToString(appItem["FormUrl"]);
                                    int index = formURL.IndexOf("=");
                                    string projectNoSwitch = formURL.Substring(0, index + 1);
                                    projectNoSwitch = projectNoSwitch + IPFItem.ProjectNumber;
                                    appItem["FormUrl"] = projectNoSwitch;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Marketing Claims List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + MarketingClaimsListFields.CompassListItemId + "\" /><Value Type=\"Integer\">" + IPFItem.CompassListItemId + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        //Dragonfly Status List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_DragonflyStatusListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + DragonflyStatusListFields.ProjectNumber + "\" /><Value Type=\"Text\">" + IPFItem.ProjectNumber + "</Value></Eq></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem appItem in compassItemCol)
                            {
                                if (appItem != null)
                                {
                                    appItem["Title"] = IPFItem.ProjectNumber;

                                    // Set Modified By to current user NOT System Account
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        private string GetSPWFTemplateIdBasedOnName(SPList spList, string wfName)
        {
            string strWFTemplateId = string.Empty;
            try
            {
                SPWorkflowAssociationCollection spwfAssociations = spList.WorkflowAssociations;
                foreach (SPWorkflowAssociation spwfAssociation in spwfAssociations)
                {
                    if (spwfAssociation.Name.ToLower() == wfName.ToLower())
                    {
                        strWFTemplateId = spwfAssociation.Id.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //Trace.Write(ex.Message);
            }
            return strWFTemplateId;
        }
    }
}