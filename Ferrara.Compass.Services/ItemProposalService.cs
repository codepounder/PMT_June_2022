using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Microsoft.SharePoint.Workflow;

namespace Ferrara.Compass.Services
{
    public class ItemProposalService : IItemProposalService
    {
        #region Member Variables
        private readonly IExceptionService exceptionService;
        private readonly IWorkflowService workflowServices;
        private readonly IProjectNotesService notesService;
        private readonly ISAPMaterialMasterService sapMMService;
        #endregion
        #region Constructors
        public ItemProposalService()
        {

        }
        public ItemProposalService(IExceptionService exceptionService, IWorkflowService workflowServices, IProjectNotesService notesService, ISAPMaterialMasterService sapMMService)
        {
            this.exceptionService = exceptionService;
            this.workflowServices = workflowServices;
            this.notesService = notesService;
            this.sapMMService = sapMMService;
        }
        #endregion
        #region Compass List
        public Boolean IsExistingProjectNo(string projectNo)
        {
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"ProjectNumber\" /><Value Type=\"Text\">" + projectNo + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem found = compassItemCol[0];
                        if (found != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public ItemProposalItem GetItemProposalItem(int itemId)
        {
            var newItem = new ItemProposalItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        // Proposed Project Fields
                        newItem.CompassListItemId = item.ID;
                        newItem.NewIPF = Convert.ToString(item[CompassListFields.NewIPF]);
                        newItem.WorkflowPhase = Convert.ToString(item[CompassListFields.WorkflowPhase]);
                        newItem.ParentProjectNumber = Convert.ToString(item[CompassListFields.ParentProjectNumber]);
                        newItem.StageGateProjectListItemId = Convert.ToInt32(item[CompassListFields.StageGateProjectListItemId]);
                        newItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                        newItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        newItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        newItem.SubmittedDate = Convert.ToDateTime(item[CompassListFields.SubmittedDate]);
                        newItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        newItem.FirstShipDate = Convert.ToDateTime(item[CompassListFields.FirstShipDate]);
                        newItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        newItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);

                        // Project Team Fields
                        newItem.Initiator = Convert.ToString(item[CompassListFields.Initiator]);
                        newItem.PM = Convert.ToString(item[CompassListFields.PM]);
                        newItem.PMName = Convert.ToString(item[CompassListFields.PMName]);
                        newItem.PackagingEngineering =
                        // SAP Item # Fields
                        newItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        newItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        newItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        newItem.LikeFGItemNumber = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                        newItem.LikeFGItemDescription = Convert.ToString(item[CompassListFields.LikeFGItemDescription]);
                        newItem.OldFGItemNumber = Convert.ToString(item[CompassListFields.OldFGItemNumber]);
                        newItem.OldFGItemDescription = Convert.ToString(item[CompassListFields.OldFGItemDescription]);
                        // Project Specifications Fields
                        newItem.NewFormula = Convert.ToString(item[CompassListFields.NewFormula]);
                        newItem.NewFlavorColor = Convert.ToString(item[CompassListFields.NewFlavorColor]);
                        newItem.NewShape = Convert.ToString(item[CompassListFields.NewShape]);
                        newItem.NewNetWeight = Convert.ToString(item[CompassListFields.NewNetWeight]);
                        newItem.Organic = Convert.ToString(item[CompassListFields.Organic]);
                        newItem.ServingSizeWeightChange = Convert.ToString(item[CompassListFields.ServingSizeWeightChange]);

                        // Item Financial Details Fields
                        if (item[CompassListFields.AnnualProjectedDollars] != null)
                        {
                            try { newItem.AnnualProjectedDollars = Convert.ToDouble(item[CompassListFields.AnnualProjectedDollars]); }
                            catch { newItem.AnnualProjectedDollars = -9999; }
                        }
                        else
                        {
                            newItem.AnnualProjectedDollars = -9999;
                        }

                        if (item[CompassListFields.Month1ProjectedDollars] != null)
                        {
                            try { newItem.Month1ProjectedDollars = Convert.ToDouble(item[CompassListFields.Month1ProjectedDollars]); }
                            catch { newItem.Month1ProjectedDollars = -9999; }
                        }
                        else
                        {
                            newItem.Month1ProjectedDollars = -9999;
                        }

                        if (item[CompassListFields.Month2ProjectedDollars] != null)
                        {
                            try { newItem.Month2ProjectedDollars = Convert.ToDouble(item[CompassListFields.Month2ProjectedDollars]); }
                            catch { newItem.Month2ProjectedDollars = -9999; }
                        }
                        else
                        {
                            newItem.Month2ProjectedDollars = -9999;
                        }

                        if (item[CompassListFields.Month3ProjectedDollars] != null)
                        {
                            try { newItem.Month3ProjectedDollars = Convert.ToDouble(item[CompassListFields.Month3ProjectedDollars]); }
                            catch { newItem.Month3ProjectedDollars = -9999; }
                        }
                        else
                        {
                            newItem.Month3ProjectedDollars = -9999;
                        }

                        if (item[CompassListFields.ExpectedGrossMarginPercent] != null)
                        {
                            try { newItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]); }
                            catch { newItem.ExpectedGrossMarginPercent = -9999; }
                        }
                        else
                        {
                            newItem.ExpectedGrossMarginPercent = -9999;
                        }

                        if (item[CompassListFields.AnnualProjectedUnits] != null)
                        {
                            try { newItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                            catch { newItem.AnnualProjectedUnits = -9999; }
                        }
                        else
                        {
                            newItem.AnnualProjectedUnits = -9999;
                        }

                        if (item[CompassListFields.Month1ProjectedUnits] != null)
                        {
                            try { newItem.Month1ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month1ProjectedUnits]); }
                            catch { newItem.Month1ProjectedUnits = -9999; }
                        }
                        else
                        {
                            newItem.Month1ProjectedUnits = -9999;
                        }

                        if (item[CompassListFields.Month2ProjectedUnits] != null)
                        {
                            try { newItem.Month2ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month2ProjectedUnits]); }
                            catch { newItem.Month2ProjectedUnits = -9999; }
                        }
                        else
                        {
                            newItem.Month2ProjectedUnits = -9999;
                        }

                        if (item[CompassListFields.Month3ProjectedUnits] != null)
                        {
                            try { newItem.Month3ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month3ProjectedUnits]); }
                            catch { newItem.Month3ProjectedUnits = -9999; }
                        }
                        else
                        {
                            newItem.Month3ProjectedUnits = -9999;
                        }

                        if (item[CompassListFields.TruckLoadPricePerSellingUnit] != null)
                        {
                            try { newItem.TruckLoadPricePerSellingUnit = Convert.ToDouble(item[CompassListFields.TruckLoadPricePerSellingUnit]); }
                            catch { newItem.TruckLoadPricePerSellingUnit = -9999; }
                        }
                        else
                        {
                            newItem.TruckLoadPricePerSellingUnit = -9999;
                        }

                        if (item[CompassListFields.Last12MonthSales] != null)
                        {
                            try { newItem.Last12MonthSales = Convert.ToDouble(item[CompassListFields.Last12MonthSales]); }
                            catch { newItem.Last12MonthSales = -9999; }
                        }
                        else
                        {
                            newItem.Last12MonthSales = -9999;
                        }

                        // Customer Specificaitons Fields
                        newItem.Customer = Convert.ToString(item[CompassListFields.Customer]);
                        newItem.CustomerSpecific = Convert.ToString(item[CompassListFields.CustomerSpecific]);
                        newItem.CustomerSpecificLotCode = Convert.ToString(item[CompassListFields.CustomerSpecificLotCode]);
                        newItem.Channel = Convert.ToString(item[CompassListFields.Channel]);
                        newItem.SoldOutsideUSA = Convert.ToString(item[CompassListFields.SoldOutsideUSA]);
                        newItem.CountryOfSale = Convert.ToString(item[CompassListFields.CountryOfSale]);

                        // Item Hierarchy Fields
                        newItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        newItem.ManuallyCreateSAPDescription = Convert.ToString(item[CompassListFields.ManuallyCreateSAPDescription]);
                        newItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        newItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        newItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                        newItem.ProductFormDescription = Convert.ToString(item[CompassListFields.ProductFormDescription]);
                        newItem.TotalQuantityUnitsInDisplay = Convert.ToString(item[CompassListFields.TotalQuantityUnitsInDisplay]);
                        newItem.NovelyProject = Convert.ToString(item[CompassListFields.NoveltyProject]);
                        newItem.CoManClassification = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);

                        // Item UPCs Fields
                        newItem.RequireNewUPCUCC = Convert.ToString(item[CompassListFields.RequireNewUPCUCC]);
                        newItem.RequireNewUnitUPC = Convert.ToString(item[CompassListFields.RequireNewUnitUPC]);
                        newItem.RequireNewDisplayBoxUPC = Convert.ToString(item[CompassListFields.RequireNewDisplayBoxUPC]);
                        newItem.RequireNewCaseUCC = Convert.ToString(item[CompassListFields.RequireNewCaseUCC]);
                        newItem.RequireNewPalletUCC = Convert.ToString(item[CompassListFields.RequireNewPalletUCC]);
                        newItem.CaseUCC = Convert.ToString(item[CompassListFields.CaseUCC]);
                        newItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        newItem.PalletUCC = Convert.ToString(item[CompassListFields.PalletUCC]);
                        newItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        newItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);

                        // Additional Item Details Fields
                        newItem.CaseType = Convert.ToString(item[CompassListFields.CaseType]);
                        newItem.MarketClaimsLabelingRequirements = Convert.ToString(item[CompassListFields.MarketClaimsLabelingRequirements]);
                        newItem.FilmSubstrate = Convert.ToString(item[CompassListFields.FilmSubstrate]);
                        newItem.PegHoleNeeded = Convert.ToString(item[CompassListFields.PegHoleNeeded]);
                        newItem.MfgLocationChange = Convert.ToString(item[CompassListFields.MfgLocationChange]);
                        newItem.InvolvesCarton = Convert.ToString(item[CompassListFields.InvolvesCarton]);
                        if (item[CompassListFields.UnitsInsideCarton] != null)
                        {
                            try
                            {
                                newItem.UnitsInsideCarton = Convert.ToString(item[CompassListFields.UnitsInsideCarton]);
                            }
                            catch
                            {
                                newItem.UnitsInsideCarton = "";
                            }
                        }
                        else
                        {
                            newItem.UnitsInsideCarton = "";
                        }

                        if (item[CompassListFields.IndividualPouchWeight] != null)
                        {
                            try
                            {
                                newItem.IndividualPouchWeight = Convert.ToDouble(item[CompassListFields.IndividualPouchWeight]);
                            }
                            catch
                            {
                                newItem.IndividualPouchWeight = -9999;
                            }
                        }
                        else
                        {
                            newItem.IndividualPouchWeight = -9999;
                        }

                        newItem.ReasonForChange = Convert.ToString(item[CompassListFields.ReasonForChange]);
                        if (item[CompassListFields.NumberofTraysPerBaseUOM] != null)
                        {
                            try { newItem.NumberofTraysPerBaseUOM = Convert.ToInt32(item[CompassListFields.NumberofTraysPerBaseUOM]); }
                            catch { newItem.NumberofTraysPerBaseUOM = -9999; }
                        }
                        else
                        {
                            newItem.NumberofTraysPerBaseUOM = -9999;
                        }

                        if (item[CompassListFields.RetailSellingUnitsBaseUOM] != null)
                        {
                            try { newItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]); }
                            catch { newItem.RetailSellingUnitsBaseUOM = -9999; }
                        }
                        else
                        {
                            newItem.RetailSellingUnitsBaseUOM = -9999;
                        }

                        if (item[CompassListFields.RetailUnitWieghtOz] != null)
                        {
                            try { newItem.RetailUnitWieghtOz = Convert.ToDouble(item[CompassListFields.RetailUnitWieghtOz]); }
                            catch { newItem.RetailUnitWieghtOz = -9999; }
                        }
                        else
                        {
                            newItem.RetailUnitWieghtOz = -9999;
                        }

                        if (item[CompassListFields.BaseUOMNetWeightLbs] != null)
                        {
                            try { newItem.BaseUOMNetWeightLbs = Convert.ToDouble(item[CompassListFields.BaseUOMNetWeightLbs]); }
                            catch { newItem.BaseUOMNetWeightLbs = -9999; }
                        }
                        else
                        {
                            newItem.BaseUOMNetWeightLbs = -9999;
                        }
                        newItem.FlowthroughDets = Convert.ToString(item[CompassListFields.FlowthroughDets]);
                        newItem.ManufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        newItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        newItem.ProfitCenter = Convert.ToString(item[CompassListFields.ProfitCenter]);
                        newItem.PLMFlag = Convert.ToString(item[CompassListFields.PLMProject]);
                        newItem = getIPFItem(newItem, spWeb, itemId);
                        newItem = getCompassList2(newItem, spWeb, itemId);
                    }
                }
            }
            return newItem;
        }
        public ItemProposalItem getIPFItem(ItemProposalItem newItem, SPWeb spWeb, int itemId)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            var ipItems = spList.GetItems(spQuery);

            if (ipItems != null && ipItems.Count > 0)
            {
                var ipItem = ipItems[0];

                newItem.ProjectLeader = Convert.ToString(ipItem[CompassTeamListFields.ProjectLeader]);
                newItem.ProjectLeaderName = Convert.ToString(ipItem[CompassTeamListFields.ProjectLeaderName]);
                newItem.Marketing = Convert.ToString(ipItem[CompassTeamListFields.Marketing]);
                newItem.MarketingName = Convert.ToString(ipItem[CompassTeamListFields.MarketingName]);
                newItem.SrProjectManager = Convert.ToString(ipItem[CompassTeamListFields.SeniorProjectManager]);
                newItem.SrProjectManagerName = Convert.ToString(ipItem[CompassTeamListFields.SeniorProjectManagerName]);
                newItem.QA = Convert.ToString(ipItem[CompassTeamListFields.QAInnovation]);
                newItem.QAName = Convert.ToString(ipItem[CompassTeamListFields.QAInnovationName]);
                newItem.InTech = Convert.ToString(ipItem[CompassTeamListFields.InTech]);
                newItem.InTechName = Convert.ToString(ipItem[CompassTeamListFields.InTechName]);
                newItem.InTechRegulatory = Convert.ToString(ipItem[CompassTeamListFields.InTechRegulatory]);
                newItem.InTechRegulatoryName = Convert.ToString(ipItem[CompassTeamListFields.InTechRegulatoryName]);
                newItem.RegulatoryQA = Convert.ToString(ipItem[CompassTeamListFields.RegulatoryQA]);
                newItem.RegulatoryQAName = Convert.ToString(ipItem[CompassTeamListFields.RegulatoryQAName]);
                newItem.PackagingEngineering = Convert.ToString(ipItem[CompassTeamListFields.PackagingEngineering]);
                newItem.PackagingEngineeringName = Convert.ToString(ipItem[CompassTeamListFields.PackagingEngineeringName]);
                newItem.SupplyChain = Convert.ToString(ipItem[CompassTeamListFields.SupplyChain]);
                newItem.SupplyChainName = Convert.ToString(ipItem[CompassTeamListFields.SupplyChainName]);
                newItem.Finance = Convert.ToString(ipItem[CompassTeamListFields.Finance]);
                newItem.FinanceName = Convert.ToString(ipItem[CompassTeamListFields.FinanceName]);
                newItem.Sales = Convert.ToString(ipItem[CompassTeamListFields.Sales]);
                newItem.SalesName = Convert.ToString(ipItem[CompassTeamListFields.SalesName]);
                newItem.Manufacturing = Convert.ToString(ipItem[CompassTeamListFields.Manufacturing]);
                newItem.ManufacturingName = Convert.ToString(ipItem[CompassTeamListFields.ManufacturingName]);
                newItem.OtherTeamMembers = Convert.ToString(ipItem[CompassTeamListFields.OtherMember]);
                newItem.OtherTeamMembersName = Convert.ToString(ipItem[CompassTeamListFields.OtherMemberName]);
                newItem.LifeCycleManagement = Convert.ToString(ipItem[CompassTeamListFields.LifeCycleManagement]);
                newItem.LifeCycleManagementName = Convert.ToString(ipItem[CompassTeamListFields.LifeCycleManagementName]);
                newItem.PackagingProcurement = Convert.ToString(ipItem[CompassTeamListFields.PackagingProcurement]);
                newItem.PackagingProcurementName = Convert.ToString(ipItem[CompassTeamListFields.PackagingProcurementName]);
                newItem.ExtManufacturingProc = Convert.ToString(ipItem[CompassTeamListFields.ExtMfgProcurement]);
                newItem.ExtManufacturingProcName = Convert.ToString(ipItem[CompassTeamListFields.ExtMfgProcurementName]);

            }
            return newItem;
        }
        public ItemProposalItem getCompassList2(ItemProposalItem newItem, SPWeb spWeb, int itemId)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            var ipItems = spList.GetItems(spQuery);

            if (ipItems != null && ipItems.Count > 0)
            {
                var ipItem = ipItems[0];

                newItem.CopyFormsForGraphicsProject = Convert.ToString(ipItem[CompassList2Fields.CopyFormsForGraphicsProject]);
                newItem.ExternalSemisItem = Convert.ToString(ipItem[CompassList2Fields.ExternalSemisItem]);
                newItem.IPFCopiedFromCompassListItemId = Convert.ToInt32(ipItem[CompassList2Fields.IPFCopiedFromCompassListItemId]);

                newItem.FGReplacingAnExistingFG = Convert.ToString(ipItem[CompassList2Fields.FGReplacingAnExistingFG]);
                newItem.IsThisAnLTOItem = Convert.ToString(ipItem[CompassList2Fields.IsThisAnLTOItem]);
                newItem.RequestChangeToFGNumForSameUCC = Convert.ToString(ipItem[CompassList2Fields.RequestChangeToFGNumForSameUCC]);
                newItem.LTOTransitionStartWindowRDD = Convert.ToDateTime(ipItem[CompassList2Fields.LTOTransitionStartWindowRDD]);
                newItem.LTOTransitionEndWindowRDD = Convert.ToDateTime(ipItem[CompassList2Fields.LTOTransitionEndWindowRDD]);
                newItem.LTOEndDateFlexibility = Convert.ToString(ipItem[CompassList2Fields.LTOEndDateFlexibility]);
            }
            return newItem;
        }
        public List<ItemProposalItem> GetActiveItemProposals()
        {
            var ipfItems = new List<ItemProposalItem>();

            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<OrderBy><FieldRef Name=\"ProjectNumber\" /></OrderBy><Where><And><And><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">OnHold</Value></Neq><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Cancelled</Value></Neq></And><Neq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">Completed</Value></Neq></And></Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            var sgItem = new ItemProposalItem();
                            sgItem.CompassListItemId = item.ID;
                            sgItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                            sgItem.PM = Convert.ToString(item[CompassListFields.PM]);
                            sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                            sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                            sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);

                            sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                            sgItem.LikeFGItemNumber = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                            sgItem.LikeFGItemDescription = Convert.ToString(item[CompassListFields.LikeFGItemDescription]);
                            sgItem.OldFGItemNumber = Convert.ToString(item[CompassListFields.OldFGItemNumber]);
                            sgItem.OldFGItemDescription = Convert.ToString(item[CompassListFields.OldFGItemDescription]);
                            ipfItems.Add(sgItem);
                        }
                    }
                }
            }
            return ipfItems;
        }
        public List<ItemProposalItem> GetGraphicsReviewProposals()
        {
            var ipfItems = new List<ItemProposalItem>();

            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<OrderBy><FieldRef Name=\"OBM_RevisedFirstShipDate\" /></OrderBy><Where><Eq><FieldRef Name=\"WorkflowStep\" /><Value Type=\"Text\">OBMREV2</Value></Eq></Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            var sgItem = new ItemProposalItem();
                            sgItem.CompassListItemId = item.ID;
                            sgItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                            sgItem.PM = Convert.ToString(item[CompassListFields.PM]);
                            sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                            sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                            sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                            //sgItem.ProjectStatus = Convert.ToString(item[CompassListFields.ProjectStatus]);

                            sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                            sgItem.LikeFGItemNumber = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                            sgItem.LikeFGItemDescription = Convert.ToString(item[CompassListFields.LikeFGItemDescription]);
                            sgItem.OldFGItemNumber = Convert.ToString(item[CompassListFields.OldFGItemNumber]);
                            sgItem.OldFGItemDescription = Convert.ToString(item[CompassListFields.OldFGItemDescription]);
                            ipfItems.Add(sgItem);
                        }
                    }
                }
            }
            return ipfItems;
        }
        public void insertTeamItem(ItemProposalItem ipItem, SPWeb spWeb)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);

            var item = spList.AddItem();

            item["Title"] = ipItem.ProjectNumber;
            item[CompassListFields.CompassListItemId] = ipItem.CompassListItemId;
            item[CompassTeamListFields.ProjectLeader] = ipItem.ProjectLeader;
            item[CompassTeamListFields.ProjectLeaderName] = ipItem.ProjectLeaderName;
            item[CompassTeamListFields.SeniorProjectManager] = ipItem.SrProjectManager;
            item[CompassTeamListFields.SeniorProjectManagerName] = ipItem.SrProjectManagerName;
            item[CompassTeamListFields.Marketing] = ipItem.Marketing;
            item[CompassTeamListFields.MarketingName] = ipItem.MarketingName;
            item[CompassTeamListFields.QAInnovation] = ipItem.QA;
            item[CompassTeamListFields.QAInnovationName] = ipItem.QAName;
            item[CompassTeamListFields.InTech] = ipItem.InTech;
            item[CompassTeamListFields.InTechName] = ipItem.InTechName;
            item[CompassTeamListFields.InTechRegulatory] = ipItem.InTechRegulatory;
            item[CompassTeamListFields.InTechRegulatoryName] = ipItem.InTechRegulatoryName;
            item[CompassTeamListFields.RegulatoryQA] = ipItem.RegulatoryQA;
            item[CompassTeamListFields.RegulatoryQAName] = ipItem.RegulatoryQAName;
            item[CompassTeamListFields.PackagingEngineering] = ipItem.PackagingEngineering;
            item[CompassTeamListFields.PackagingEngineeringName] = ipItem.PackagingEngineeringName;
            item[CompassTeamListFields.SupplyChain] = ipItem.SupplyChain;
            item[CompassTeamListFields.SupplyChainName] = ipItem.SupplyChainName;
            item[CompassTeamListFields.Finance] = ipItem.Finance;
            item[CompassTeamListFields.FinanceName] = ipItem.FinanceName;
            item[CompassTeamListFields.Sales] = ipItem.Sales;
            item[CompassTeamListFields.SalesName] = ipItem.SalesName;
            item[CompassTeamListFields.Manufacturing] = ipItem.Manufacturing;
            item[CompassTeamListFields.ManufacturingName] = ipItem.ManufacturingName;
            item[CompassTeamListFields.OtherMember] = ipItem.OtherTeamMembers;
            item[CompassTeamListFields.OtherMemberName] = ipItem.OtherTeamMembersName;
            item[CompassTeamListFields.LifeCycleManagement] = ipItem.LifeCycleManagement;
            item[CompassTeamListFields.LifeCycleManagementName] = ipItem.LifeCycleManagementName;
            item[CompassTeamListFields.PackagingProcurement] = ipItem.PackagingProcurement;
            item[CompassTeamListFields.PackagingProcurementName] = ipItem.PackagingProcurementName;
            item[CompassTeamListFields.ExtMfgProcurement] = ipItem.ExtManufacturingProc;
            item[CompassTeamListFields.ExtMfgProcurementName] = ipItem.ExtManufacturingProcName;
            item[CompassTeamListFields.Marketing] = ipItem.Marketing;
            item[CompassTeamListFields.MarketingName] = ipItem.MarketingName;
            item["Editor"] = SPContext.Current.Web.CurrentUser;
            item.Update();
        }
        public int InsertItemProposalItem(ItemProposalItem ipItem)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.AddItem();

                        item["Title"] = ipItem.ProjectNumber;

                        // Proposed Project Fields
                        item[CompassListFields.NewIPF] = ipItem.NewIPF;
                        item[CompassListFields.PMTWorkflowVersion] = ipItem.PMTWorkflowVersion;
                        item[CompassListFields.ProjectNumber] = ipItem.ProjectNumber;
                        item[CompassListFields.ParentProjectNumber] = ipItem.ParentProjectNumber;
                        item[CompassListFields.ProjectType] = ipItem.ProjectType;
                        item[CompassListFields.MaterialGroup1Brand] = ipItem.MaterialGroup1Brand;
                        if ((ipItem.SubmittedDate != null) && (ipItem.SubmittedDate != DateTime.MinValue))
                        {
                            item[CompassListFields.SubmittedDate] = ipItem.SubmittedDate;
                            item[CompassListFields.PLMProject] = "Yes";
                        }
                        item[CompassListFields.ItemConcept] = ipItem.ItemConcept;
                        if ((ipItem.FirstShipDate != null) && (ipItem.FirstShipDate != DateTime.MinValue))
                            item[CompassListFields.FirstShipDate] = ipItem.FirstShipDate;
                        if ((ipItem.RevisedFirstShipDate != null) && (ipItem.RevisedFirstShipDate != DateTime.MinValue))
                            item[CompassListFields.RevisedFirstShipDate] = ipItem.RevisedFirstShipDate;
                        item[CompassListFields.ProjectTypeSubCategory] = ipItem.ProjectTypeSubCategory;

                        // Project Team Fields
                        item[CompassListFields.Initiator] = ipItem.Initiator;
                        item[CompassListFields.InitiatorName] = ipItem.InitiatorName;
                        item[CompassListFields.PM] = ipItem.PM;
                        item[CompassListFields.PMName] = ipItem.PMName;
                        item[CompassListFields.OBM] = ipItem.PM;
                        item[CompassListFields.OBMName] = ipItem.PMName;

                        // SAP Item # Fields
                        item[CompassListFields.TBDIndicator] = ipItem.TBDIndicator;
                        item[CompassListFields.SAPItemNumber] = ipItem.SAPItemNumber;
                        item[CompassListFields.SAPDescription] = ipItem.SAPDescription;
                        item[CompassListFields.LikeFGItemNumber] = ipItem.LikeFGItemNumber;
                        item[CompassListFields.LikeFGItemDescription] = ipItem.LikeFGItemDescription;
                        item[CompassListFields.OldFGItemNumber] = ipItem.OldFGItemNumber;
                        item[CompassListFields.OldFGItemDescription] = ipItem.OldFGItemDescription;

                        // Project Specifications Fields
                        item[CompassListFields.NewFormula] = ipItem.NewFormula;
                        item[CompassListFields.NewFlavorColor] = ipItem.NewFlavorColor;
                        item[CompassListFields.NewShape] = ipItem.NewShape;
                        item[CompassListFields.NewNetWeight] = ipItem.NewNetWeight;
                        item[CompassListFields.Organic] = ipItem.Organic;
                        item[CompassListFields.ServingSizeWeightChange] = ipItem.ServingSizeWeightChange;

                        // Item Financial Details Fields
                        item[CompassListFields.AnnualProjectedDollars] = ipItem.AnnualProjectedDollars;
                        item[CompassListFields.Month1ProjectedDollars] = ipItem.Month1ProjectedDollars;
                        item[CompassListFields.Month2ProjectedDollars] = ipItem.Month2ProjectedDollars;
                        item[CompassListFields.Month3ProjectedDollars] = ipItem.Month3ProjectedDollars;
                        item[CompassListFields.ExpectedGrossMarginPercent] = ipItem.ExpectedGrossMarginPercent;
                        item[CompassListFields.AnnualProjectedUnits] = ipItem.AnnualProjectedUnits;
                        item[CompassListFields.Month1ProjectedUnits] = ipItem.Month1ProjectedUnits;
                        item[CompassListFields.Month2ProjectedUnits] = ipItem.Month2ProjectedUnits;
                        item[CompassListFields.Month3ProjectedUnits] = ipItem.Month3ProjectedUnits;
                        item[CompassListFields.TruckLoadPricePerSellingUnit] = ipItem.TruckLoadPricePerSellingUnit;
                        item[CompassListFields.Last12MonthSales] = ipItem.Last12MonthSales;

                        // Customer Specificaitons Fields
                        item[CompassListFields.CustomerSpecific] = ipItem.CustomerSpecific;
                        item[CompassListFields.Customer] = ipItem.Customer;
                        item[CompassListFields.CustomerSpecificLotCode] = ipItem.CustomerSpecificLotCode;
                        item[CompassListFields.Channel] = ipItem.Channel;
                        item[CompassListFields.SoldOutsideUSA] = ipItem.SoldOutsideUSA;
                        item[CompassListFields.CountryOfSale] = ipItem.CountryOfSale;

                        // Item Hierarchy Fields
                        item[CompassListFields.ProductHierarchyLevel1] = ipItem.ProductHierarchyLevel1;
                        item[CompassListFields.ManuallyCreateSAPDescription] = ipItem.ManuallyCreateSAPDescription;
                        item[CompassListFields.ProductHierarchyLevel2] = ipItem.ProductHierarchyLevel2;
                        item[CompassListFields.MaterialGroup4ProductForm] = ipItem.MaterialGroup4ProductForm;
                        item[CompassListFields.MaterialGroup5PackType] = ipItem.MaterialGroup5PackType;
                        item[CompassListFields.NoveltyProject] = ipItem.NovelyProject;
                        item[CompassListFields.TotalQuantityUnitsInDisplay] = ipItem.TotalQuantityUnitsInDisplay;
                        item[CompassListFields.ProductFormDescription] = ipItem.ProductFormDescription;

                        // Item UPCs Fields
                        item[CompassListFields.RequireNewUPCUCC] = ipItem.RequireNewUPCUCC;
                        item[CompassListFields.RequireNewUnitUPC] = ipItem.RequireNewUnitUPC;
                        item[CompassListFields.RequireNewDisplayBoxUPC] = ipItem.RequireNewDisplayBoxUPC;
                        item[CompassListFields.RequireNewCaseUCC] = ipItem.RequireNewCaseUCC;
                        item[CompassListFields.RequireNewPalletUCC] = ipItem.RequireNewPalletUCC;
                        item[CompassListFields.CaseUCC] = ipItem.CaseUCC;
                        item[CompassListFields.DisplayBoxUPC] = ipItem.DisplayBoxUPC;
                        item[CompassListFields.PalletUCC] = ipItem.PalletUCC;
                        item[CompassListFields.UnitUPC] = ipItem.UnitUPC;
                        item[CompassListFields.SAPBaseUOM] = ipItem.SAPBaseUOM;

                        // Additional Item Details Fields
                        item[CompassListFields.CaseType] = ipItem.CaseType;
                        item[CompassListFields.MarketClaimsLabelingRequirements] = ipItem.MarketClaimsLabelingRequirements;
                        item[CompassListFields.NumberofTraysPerBaseUOM] = ipItem.NumberofTraysPerBaseUOM;
                        item[CompassListFields.RetailSellingUnitsBaseUOM] = ipItem.RetailSellingUnitsBaseUOM;
                        item[CompassListFields.RetailUnitWieghtOz] = ipItem.RetailUnitWieghtOz;
                        item[CompassListFields.BaseUOMNetWeightLbs] = ipItem.BaseUOMNetWeightLbs;
                        item[CompassListFields.FilmSubstrate] = ipItem.FilmSubstrate;
                        item[CompassListFields.PegHoleNeeded] = ipItem.PegHoleNeeded;
                        item[CompassListFields.ReasonForChange] = ipItem.ReasonForChange;
                        item[CompassListFields.InvolvesCarton] = ipItem.InvolvesCarton;
                        item[CompassListFields.UnitsInsideCarton] = ipItem.UnitsInsideCarton;
                        item[CompassListFields.IndividualPouchWeight] = ipItem.IndividualPouchWeight;
                        item[CompassListFields.FlowthroughDets] = ipItem.FlowthroughDets;
                        item[CompassListFields.ProfitCenter] = ipItem.ProfitCenter;
                        item[CompassListFields.StageGateProjectListItemId] = ipItem.StageGateProjectListItemId;
                        item[CompassListFields.GenerateIPFSortOrder] = ipItem.GenerateIPFSortOrder;
                        item[CompassListFields.AllProjectUsers] = ipItem.AllUsers;
                        item[CompassListFields.PLMProject] = "Yes";
                        // Set Links
                        SPFieldUrlValue value = new SPFieldUrlValue();
                        value.Description = ipItem.ProjectNumber;
                        value.Url = ipItem.CommercializationLink;
                        item[CompassListFields.CommercializationLink] = value;

                        value = new SPFieldUrlValue();
                        value.Description = "Copy";
                        value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ItemProposal, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", ipItem.ProjectNumber, "&IPFForm=Copy");
                        item[CompassListFields.CopyLink] = value;

                        value = new SPFieldUrlValue();
                        value.Description = "Change";
                        value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ItemProposal, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", ipItem.ProjectNumber, "&IPFForm=Change");
                        item[CompassListFields.ChangeLink] = value;

                        // Workflow Status Link
                        value = new SPFieldUrlValue();
                        value.Description = ipItem.ProjectNumber;
                        value.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ProjectStatus, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", ipItem.ProjectNumber);
                        item[CompassListFields.WorkflowStatusLink] = value;

                        item[CompassListFields.WorkflowPhase] = GlobalConstants.WORKFLOWPHASE_IPF;

                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;
                        // Set Created By to current user NOT System Account
                        item["Author"] = SPContext.Current.Web.CurrentUser;

                        if (!string.IsNullOrEmpty(ipItem.TestProject))
                        {
                            if (ipItem.TestProject == "Yes")
                            {
                                item[CompassListFields.TestProject] = ipItem.TestProject;
                            }
                        }

                        item.Update();

                        ipItem.CompassListItemId = item.ID;
                        insertTeamItem(ipItem, spWeb);
                        string strWFTemplateId = GetSPWFTemplateIdBasedOnName(spList, "1 - Start PMT Workflow");
                        SPWorkflowAssociation spwaDocLib = spList.WorkflowAssociations[new Guid(strWFTemplateId)];
                        spSite.WorkflowManager.StartWorkflow(item, spwaDocLib, spwaDocLib.AssociationData, true);
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return ipItem.CompassListItemId;
        }
        public int InsertCompassList2(ItemProposalItem ipItem)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + ipItem.CompassListItemId + "</Value></Eq></Where>";
                        SPListItemCollection CompassList2Items = spList.GetItems(spQuery);
                        SPListItem CompassList2Item;
                        if (CompassList2Items != null && CompassList2Items.Count > 0)
                        {
                            CompassList2Item = CompassList2Items[0];
                        }
                        else
                        {
                            CompassList2Item = spList.AddItem();

                            CompassList2Item["Title"] = ipItem.ProjectNumber;
                            CompassList2Item[CompassList2Fields.CompassListItemId] = ipItem.CompassListItemId;
                        }
                        CompassList2Item[CompassList2Fields.FGReplacingAnExistingFG] = ipItem.FGReplacingAnExistingFG;
                        CompassList2Item[CompassList2Fields.IsThisAnLTOItem] = ipItem.IsThisAnLTOItem;
                        CompassList2Item[CompassList2Fields.RequestChangeToFGNumForSameUCC] = ipItem.RequestChangeToFGNumForSameUCC;
                        if ((ipItem.LTOTransitionStartWindowRDD != null) && (ipItem.LTOTransitionStartWindowRDD != DateTime.MinValue))
                            CompassList2Item[CompassList2Fields.LTOTransitionStartWindowRDD] = ipItem.LTOTransitionStartWindowRDD;
                        if ((ipItem.LTOTransitionEndWindowRDD != null) && (ipItem.LTOTransitionEndWindowRDD != DateTime.MinValue))
                            CompassList2Item[CompassList2Fields.LTOTransitionEndWindowRDD] = ipItem.LTOTransitionEndWindowRDD;
                        CompassList2Item[CompassList2Fields.LTOEndDateFlexibility] = ipItem.LTOEndDateFlexibility;

                        CompassList2Item[CompassList2Fields.ModifiedBy] = SPContext.Current.Web.CurrentUser.ToString();
                        CompassList2Item[CompassList2Fields.ModifiedDate] = DateTime.Now.ToString();
                        CompassList2Item.Update();

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return ipItem.CompassListItemId;
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
        public void UpdateItemProposalItem(ItemProposalItem ipItem, bool Submitted)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.GetItemById(ipItem.CompassListItemId);
                        if (item != null)
                        {
                            // Proposed Project Fields
                            //item[CompassListFields.ProjectNumber] = ipItem.ProjectNumber;
                            item[CompassListFields.ProjectType] = ipItem.ProjectType;
                            item[CompassListFields.MaterialGroup1Brand] = ipItem.MaterialGroup1Brand;
                            if ((ipItem.SubmittedDate != null) && (ipItem.SubmittedDate != DateTime.MinValue))
                            {
                                item[CompassListFields.SubmittedDate] = ipItem.SubmittedDate;
                            }
                            item[CompassListFields.ItemConcept] = ipItem.ItemConcept;
                            if ((ipItem.FirstShipDate != null) && (ipItem.FirstShipDate != DateTime.MinValue))
                                item[CompassListFields.FirstShipDate] = ipItem.FirstShipDate;
                            if ((ipItem.RevisedFirstShipDate != null) && (ipItem.RevisedFirstShipDate != DateTime.MinValue))
                                item[CompassListFields.RevisedFirstShipDate] = ipItem.RevisedFirstShipDate;
                            item[CompassListFields.ProjectTypeSubCategory] = ipItem.ProjectTypeSubCategory;

                            // Project Team Fields
                            item[CompassListFields.Initiator] = ipItem.Initiator;
                            item[CompassListFields.InitiatorName] = ipItem.InitiatorName;
                            item[CompassListFields.PM] = ipItem.PM;
                            item[CompassListFields.PMName] = ipItem.PMName;
                            item[CompassListFields.OBM] = ipItem.PM;
                            item[CompassListFields.OBMName] = ipItem.PMName;

                            // SAP Item # Fields
                            item[CompassListFields.TBDIndicator] = ipItem.TBDIndicator;
                            item[CompassListFields.SAPItemNumber] = ipItem.SAPItemNumber;
                            item[CompassListFields.SAPDescription] = ipItem.SAPDescription;
                            item[CompassListFields.LikeFGItemNumber] = ipItem.LikeFGItemNumber;
                            item[CompassListFields.LikeFGItemDescription] = ipItem.LikeFGItemDescription;
                            item[CompassListFields.OldFGItemNumber] = ipItem.OldFGItemNumber;
                            item[CompassListFields.OldFGItemDescription] = ipItem.OldFGItemDescription;

                            // Project Specifications Fields
                            item[CompassListFields.NewFormula] = ipItem.NewFormula;
                            item[CompassListFields.NewFlavorColor] = ipItem.NewFlavorColor;
                            item[CompassListFields.NewShape] = ipItem.NewShape;
                            item[CompassListFields.NewNetWeight] = ipItem.NewNetWeight;
                            item[CompassListFields.Organic] = ipItem.Organic;
                            item[CompassListFields.ServingSizeWeightChange] = ipItem.ServingSizeWeightChange;

                            // Item Financial Details Fields
                            item[CompassListFields.AnnualProjectedDollars] = ipItem.AnnualProjectedDollars;
                            item[CompassListFields.Month1ProjectedDollars] = ipItem.Month1ProjectedDollars;
                            item[CompassListFields.Month2ProjectedDollars] = ipItem.Month2ProjectedDollars;
                            item[CompassListFields.Month3ProjectedDollars] = ipItem.Month3ProjectedDollars;
                            item[CompassListFields.ExpectedGrossMarginPercent] = ipItem.ExpectedGrossMarginPercent;
                            item[CompassListFields.AnnualProjectedUnits] = ipItem.AnnualProjectedUnits;
                            item[CompassListFields.Month1ProjectedUnits] = ipItem.Month1ProjectedUnits;
                            item[CompassListFields.Month2ProjectedUnits] = ipItem.Month2ProjectedUnits;
                            item[CompassListFields.Month3ProjectedUnits] = ipItem.Month3ProjectedUnits;
                            item[CompassListFields.TruckLoadPricePerSellingUnit] = ipItem.TruckLoadPricePerSellingUnit;
                            item[CompassListFields.Last12MonthSales] = ipItem.Last12MonthSales;

                            // Customer Specificaitons Fields
                            item[CompassListFields.CustomerSpecific] = ipItem.CustomerSpecific;
                            item[CompassListFields.Customer] = ipItem.Customer;
                            item[CompassListFields.CustomerSpecificLotCode] = ipItem.CustomerSpecificLotCode;
                            item[CompassListFields.Channel] = ipItem.Channel;
                            item[CompassListFields.SoldOutsideUSA] = ipItem.SoldOutsideUSA;
                            item[CompassListFields.CountryOfSale] = ipItem.CountryOfSale;

                            // Item Hierarchy Fields
                            item[CompassListFields.ProductHierarchyLevel1] = ipItem.ProductHierarchyLevel1;
                            item[CompassListFields.ManuallyCreateSAPDescription] = ipItem.ManuallyCreateSAPDescription;
                            item[CompassListFields.ProductHierarchyLevel2] = ipItem.ProductHierarchyLevel2;
                            item[CompassListFields.MaterialGroup4ProductForm] = ipItem.MaterialGroup4ProductForm;
                            item[CompassListFields.MaterialGroup5PackType] = ipItem.MaterialGroup5PackType;
                            item[CompassListFields.ProductFormDescription] = ipItem.ProductFormDescription;
                            item[CompassListFields.NoveltyProject] = ipItem.NovelyProject;
                            item[CompassListFields.TotalQuantityUnitsInDisplay] = ipItem.TotalQuantityUnitsInDisplay;

                            // Item UPCs Fields
                            item[CompassListFields.RequireNewUPCUCC] = ipItem.RequireNewUPCUCC;
                            item[CompassListFields.RequireNewUnitUPC] = ipItem.RequireNewUnitUPC;
                            item[CompassListFields.RequireNewDisplayBoxUPC] = ipItem.RequireNewDisplayBoxUPC;
                            item[CompassListFields.RequireNewCaseUCC] = ipItem.RequireNewCaseUCC;
                            item[CompassListFields.RequireNewPalletUCC] = ipItem.RequireNewPalletUCC;
                            item[CompassListFields.CaseUCC] = ipItem.CaseUCC;
                            item[CompassListFields.DisplayBoxUPC] = ipItem.DisplayBoxUPC;
                            item[CompassListFields.PalletUCC] = ipItem.PalletUCC;
                            item[CompassListFields.UnitUPC] = ipItem.UnitUPC;
                            item[CompassListFields.SAPBaseUOM] = ipItem.SAPBaseUOM;

                            // Additional Item Details Fields
                            item[CompassListFields.CaseType] = ipItem.CaseType;
                            item[CompassListFields.MarketClaimsLabelingRequirements] = ipItem.MarketClaimsLabelingRequirements;
                            item[CompassListFields.NumberofTraysPerBaseUOM] = ipItem.NumberofTraysPerBaseUOM;
                            item[CompassListFields.RetailSellingUnitsBaseUOM] = ipItem.RetailSellingUnitsBaseUOM;
                            item[CompassListFields.RetailUnitWieghtOz] = ipItem.RetailUnitWieghtOz;
                            item[CompassListFields.BaseUOMNetWeightLbs] = ipItem.BaseUOMNetWeightLbs;
                            item[CompassListFields.FilmSubstrate] = ipItem.FilmSubstrate;
                            item[CompassListFields.PegHoleNeeded] = ipItem.PegHoleNeeded;
                            item[CompassListFields.InvolvesCarton] = ipItem.InvolvesCarton;
                            item[CompassListFields.UnitsInsideCarton] = ipItem.UnitsInsideCarton;
                            item[CompassListFields.IndividualPouchWeight] = ipItem.IndividualPouchWeight;
                            item[CompassListFields.ReasonForChange] = ipItem.ReasonForChange;

                            item[CompassListFields.FlowthroughDets] = ipItem.FlowthroughDets;
                            item[CompassListFields.AllProjectUsers] = ipItem.AllUsers;
                            item[CompassListFields.ProfitCenter] = ipItem.ProfitCenter;
                            item[CompassListFields.LastUpdatedFormName] = CompassForm.IPF.ToString();

                            // Clear External Mfg form fields for Graphic Changes Only - Internal  - projects
                            if (Submitted && ipItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly && ipItem.ExternalSemisItem != "Yes")
                            {
                                item[CompassListFields.CoManufacturingClassification] = string.Empty;
                                item[CompassListFields.DoesBulkSemiExistToBringInHouse] = string.Empty;
                                item[CompassListFields.ExistingBulkSemiNumber] = string.Empty;
                                item[CompassListFields.BulkSemiDescription] = string.Empty;
                                item[CompassListFields.ExternalManufacturer] = string.Empty;
                                item[CompassListFields.ExternalPacker] = string.Empty;
                                item[CompassListFields.PurchasedIntoLocation] = string.Empty;
                                item[CompassListFields.CurrentTimelineAcceptable] = string.Empty;
                                item[CompassListFields.LeadTimeFromSupplier] = string.Empty;
                            }

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            if (!string.IsNullOrEmpty(ipItem.TestProject))
                            {
                                if (ipItem.TestProject == "Yes")
                                {
                                    item[CompassListFields.TestProject] = ipItem.TestProject;
                                }
                            }

                            item.Update();

                        }
                        #region Compass Team Update
                        var spListTeam = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + ipItem.CompassListItemId + "</Value></Eq></Where>";
                        var teamItems = spListTeam.GetItems(spQuery);
                        SPListItem teamItem;
                        if (teamItems != null)
                        {
                            if (teamItems.Count > 0)
                            {
                                teamItem = teamItems[0];
                            }
                            else
                            {
                                teamItem = spListTeam.AddItem();
                                teamItem["Title"] = ipItem.ProjectNumber;
                                teamItem[CompassListFields.CompassListItemId] = ipItem.CompassListItemId;
                            }
                            teamItem[CompassTeamListFields.ProjectLeader] = ipItem.ProjectLeader;
                            teamItem[CompassTeamListFields.ProjectLeaderName] = ipItem.ProjectLeaderName;
                            teamItem[CompassTeamListFields.SeniorProjectManager] = ipItem.SrProjectManager;
                            teamItem[CompassTeamListFields.SeniorProjectManagerName] = ipItem.SrProjectManagerName;
                            teamItem[CompassTeamListFields.QAInnovation] = ipItem.QA;
                            teamItem[CompassTeamListFields.QAInnovationName] = ipItem.QAName;
                            teamItem[CompassTeamListFields.InTech] = ipItem.InTech;
                            teamItem[CompassTeamListFields.InTechName] = ipItem.InTechName;
                            teamItem[CompassTeamListFields.InTechRegulatory] = ipItem.InTechRegulatory;
                            teamItem[CompassTeamListFields.InTechRegulatoryName] = ipItem.InTechRegulatoryName;
                            teamItem[CompassTeamListFields.RegulatoryQA] = ipItem.RegulatoryQA;
                            teamItem[CompassTeamListFields.RegulatoryQAName] = ipItem.RegulatoryQAName;
                            teamItem[CompassTeamListFields.PackagingEngineering] = ipItem.PackagingEngineering;
                            teamItem[CompassTeamListFields.PackagingEngineeringName] = ipItem.PackagingEngineeringName;
                            teamItem[CompassTeamListFields.SupplyChain] = ipItem.SupplyChain;
                            teamItem[CompassTeamListFields.SupplyChainName] = ipItem.SupplyChainName;
                            teamItem[CompassTeamListFields.Finance] = ipItem.Finance;
                            teamItem[CompassTeamListFields.FinanceName] = ipItem.FinanceName;
                            teamItem[CompassTeamListFields.Sales] = ipItem.Sales;
                            teamItem[CompassTeamListFields.SalesName] = ipItem.SalesName;
                            teamItem[CompassTeamListFields.Manufacturing] = ipItem.Manufacturing;
                            teamItem[CompassTeamListFields.ManufacturingName] = ipItem.ManufacturingName;
                            teamItem[CompassTeamListFields.OtherMember] = ipItem.OtherTeamMembers;
                            teamItem[CompassTeamListFields.OtherMemberName] = ipItem.OtherTeamMembersName;
                            teamItem[CompassTeamListFields.LifeCycleManagement] = ipItem.LifeCycleManagement;
                            teamItem[CompassTeamListFields.LifeCycleManagementName] = ipItem.LifeCycleManagementName;
                            teamItem[CompassTeamListFields.PackagingProcurement] = ipItem.PackagingProcurement;
                            teamItem[CompassTeamListFields.PackagingProcurementName] = ipItem.PackagingProcurementName;
                            teamItem[CompassTeamListFields.ExtMfgProcurement] = ipItem.ExtManufacturingProc;
                            teamItem[CompassTeamListFields.ExtMfgProcurementName] = ipItem.ExtManufacturingProcName;
                            teamItem[CompassTeamListFields.Marketing] = ipItem.Marketing;
                            teamItem[CompassTeamListFields.MarketingName] = ipItem.MarketingName;

                            teamItem.Update();
                        }
                        #endregion

                        #region Compass List 2 Update
                        var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spList2Query = new SPQuery();
                        spList2Query.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + ipItem.CompassListItemId + "</Value></Eq></Where>";
                        var List2Items = spList2.GetItems(spList2Query);
                        SPListItem List2Item;
                        if (List2Items != null)
                        {
                            if (List2Items.Count > 0)
                            {
                                List2Item = List2Items[0];
                            }
                            else
                            {
                                List2Item = spList2.AddItem();
                                List2Item["Title"] = ipItem.ProjectNumber;
                                List2Item[CompassList2Fields.CompassListItemId] = ipItem.CompassListItemId;
                            }
                            List2Item[CompassList2Fields.CopyFormsForGraphicsProject] = ipItem.CopyFormsForGraphicsProject;
                            List2Item[CompassList2Fields.ExternalSemisItem] = ipItem.ExternalSemisItem;
                            List2Item[CompassList2Fields.IsDisplayBoxConsumerUnit] = ipItem.IsDisplayBoxConsumerUnit;

                            List2Item[CompassList2Fields.FGReplacingAnExistingFG] = ipItem.FGReplacingAnExistingFG;
                            List2Item[CompassList2Fields.IsThisAnLTOItem] = ipItem.IsThisAnLTOItem;
                            List2Item[CompassList2Fields.RequestChangeToFGNumForSameUCC] = ipItem.RequestChangeToFGNumForSameUCC;
                            if ((ipItem.LTOTransitionStartWindowRDD != null) && (ipItem.LTOTransitionStartWindowRDD != DateTime.MinValue))
                                List2Item[CompassList2Fields.LTOTransitionStartWindowRDD] = ipItem.LTOTransitionStartWindowRDD;
                            if ((ipItem.LTOTransitionEndWindowRDD != null) && (ipItem.LTOTransitionEndWindowRDD != DateTime.MinValue))
                                List2Item[CompassList2Fields.LTOTransitionEndWindowRDD] = ipItem.LTOTransitionEndWindowRDD;
                            List2Item[CompassList2Fields.LTOEndDateFlexibility] = ipItem.LTOEndDateFlexibility;

                            List2Item.Update();
                        }
                        #endregion

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public SPUser GetItemProposalBrandManager(int itemId)
        {
            SPUser brandMgr = null;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        var ipItems = spList.GetItems(spQuery);
                        if (ipItems.Count > 0)
                        {
                            var item = ipItems[0];
                            if (item != null)
                            {
                                var userField = (SPFieldUser)item.Fields.GetField(CompassTeamListFields.Marketing);
                                var fieldValue = (SPFieldUserValue)userField.GetFieldValue((string)item[CompassTeamListFields.Marketing]);
                                brandMgr = fieldValue.User;
                            }
                        }
                    }
                }
            });
            return brandMgr;
        }
        private string SAPNomenclature(SPListItem IpfItem)
        {
            var SAPDescription = string.Empty;
            if (Convert.ToString(IpfItem[CompassListFields.ProductHierarchyLevel1]) == GlobalConstants.PRODUCT_HIERARCHY1_CoMan
                && Convert.ToString(IpfItem[CompassListFields.ManuallyCreateSAPDescription]) == GlobalConstants.CONST_Yes)
            {
                SAPDescription = Convert.ToString(IpfItem[CompassListFields.SAPDescription]);
            }
            else
            {
                var TBD = "";
                var Brand = "";
                var Season = "";
                var CustomerSpecific = "";
                var PkgType = "";
                var UnitsInsideCarton = "";
                var Count = "";
                var OzWeight = "";
                var CountryCode = "";
                var ProductFormDescription = "";
                #region TBD
                TBD = "TBD ";
                #endregion
                #region Brand
                var BrandSelection = Convert.ToString(IpfItem[CompassListFields.MaterialGroup1Brand]);

                if (BrandSelection != "Select...")
                {
                    Brand = BrandSelection.Substring(BrandSelection.LastIndexOf("(") + 1, (BrandSelection.LastIndexOf(")") - BrandSelection.LastIndexOf("(")) - 1);
                }

                Brand = string.IsNullOrEmpty(Brand) ? "" : Brand + " ";
                #endregion
                #region Season
                if (Convert.ToString(IpfItem[CompassListFields.ProductHierarchyLevel1]) == "Seasonal (000000023)")
                {
                    var SeasonSelection = Convert.ToString(IpfItem[CompassListFields.ProductHierarchyLevel2]);

                    if (SeasonSelection == "VALENTINE\'S (000000008)" || SeasonSelection == "VALENTINE\'S (000000008)")
                    {
                        Season = "VDY ";
                    }
                    else if (SeasonSelection == "EASTER (000000003)" || SeasonSelection == "EASTER BULK (000000004)")
                    {
                        Season = "ESR ";
                    }
                    else if (SeasonSelection == "HALLOWEEN (000000005)" || SeasonSelection == "HALLOWEEN BULK (000000006)")
                    {
                        Season = "HWN ";
                    }
                    else if (SeasonSelection == "CHRISTMAS (000000001)" || SeasonSelection == "CHRISTMAS BULK (000000002)")
                    {
                        Season = "HLY ";
                    }
                    else if (SeasonSelection == "HOLIDAY (000000001)")
                    {
                        Season = "HLY ";
                    }
                    else if (SeasonSelection == "SUMMER(000000007)")
                    {
                        Season = "SMR ";
                    }
                }
                #endregion
                #region Customer specific
                if (Convert.ToString(IpfItem[CompassListFields.CustomerSpecific]) == "Customer Specific")
                {
                    var CustomerspecificSelection = Convert.ToString(IpfItem[CompassListFields.Customer]);

                    if (CustomerspecificSelection != "Select...")
                    {
                        var listName = GlobalConstants.LIST_CustomersLookup;

                        using (var site = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (var spweb = site.OpenWeb())
                            {
                                var spList = spweb.Lists.TryGetList(listName);
                                if (spList != null)
                                {
                                    var fields = new string[] { GlobalLookupFieldConstants.Id, GlobalLookupFieldConstants.Title, GlobalLookupFieldConstants.Active, GlobalLookupFieldConstants.LookupValue };
                                    SPListItemCollection items = spList.GetItems(fields);
                                    if (items.Count > 0)
                                    {
                                        foreach (SPListItem item in items)
                                        {
                                            if (Convert.ToString(item[GlobalLookupFieldConstants.Title]).Contains(CustomerspecificSelection))
                                            {
                                                CustomerSpecific = Convert.ToString(item[GlobalLookupFieldConstants.LookupValue]);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                CustomerSpecific = string.IsNullOrEmpty(CustomerSpecific) ? "" : CustomerSpecific + " ";

                #endregion
                #region UnitsInsideCarton
                if (Convert.ToString(IpfItem[CompassListFields.InvolvesCarton]) == "Yes")
                {
                    var strUnitsInsideCarton = Convert.ToString(IpfItem[CompassListFields.UnitsInsideCarton]);

                    if (!string.IsNullOrEmpty(strUnitsInsideCarton) && strUnitsInsideCarton != "-9999")
                    {
                        try
                        {
                            if (Convert.ToInt16(strUnitsInsideCarton) != 0)
                            {
                                UnitsInsideCarton = strUnitsInsideCarton + "/";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                #endregion
                #region Count
                var NumberofTraysPerBaseUOM = Convert.ToString(IpfItem[CompassListFields.NumberofTraysPerBaseUOM]);
                if (!string.IsNullOrEmpty(NumberofTraysPerBaseUOM) && NumberofTraysPerBaseUOM != "-9999")
                {
                    try
                    {
                        if (Convert.ToInt16(NumberofTraysPerBaseUOM) != 0)
                        {
                            Count = NumberofTraysPerBaseUOM + "/";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                else
                {
                    var RetailSellingUnitsBaseUOM = Convert.ToString(IpfItem[CompassListFields.RetailSellingUnitsBaseUOM]);
                    if (!string.IsNullOrEmpty(RetailSellingUnitsBaseUOM) && RetailSellingUnitsBaseUOM != "-9999")
                    {
                        try
                        {
                            if (Convert.ToInt16(RetailSellingUnitsBaseUOM) != 0)
                            {
                                Count = RetailSellingUnitsBaseUOM + "/";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                #endregion
                #region Oz Weight
                if (Convert.ToString(IpfItem[CompassListFields.InvolvesCarton]) == "Yes")
                {
                    var IndividualPouchWeight = Convert.ToString(IpfItem[CompassListFields.IndividualPouchWeight]);

                    if (!string.IsNullOrEmpty(IndividualPouchWeight) && IndividualPouchWeight != "-9999")
                    {
                        try
                        {
                            var dIndividualPouchWeight = Convert.ToDouble(IndividualPouchWeight);
                            if (dIndividualPouchWeight == 0)
                            {
                                OzWeight = "";
                            }
                            else
                            {
                                OzWeight = dIndividualPouchWeight.ToString("0.#####") + "oz";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                else
                {
                    var RetailUnitWieghtOz = Convert.ToString(IpfItem[CompassListFields.RetailUnitWieghtOz]);
                    if (!string.IsNullOrEmpty(RetailUnitWieghtOz) && RetailUnitWieghtOz != "-9999")
                    {
                        try
                        {
                            var dRetailUnitWieghtOz = Convert.ToDouble(RetailUnitWieghtOz);
                            if (dRetailUnitWieghtOz == 0)
                            {
                                OzWeight = "";
                            }
                            else
                            {
                                OzWeight = dRetailUnitWieghtOz.ToString("0.#####") + "oz";
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                #endregion
                #region Pkg Type
                var MaterialGroup5PackType = Convert.ToString(IpfItem[CompassListFields.MaterialGroup5PackType]);
                if (MaterialGroup5PackType.Contains("(DOY)"))
                {
                    PkgType = " DOY";
                }
                else if (MaterialGroup5PackType.Contains("(SHP)"))
                {
                    PkgType = " SHP";
                }
                #endregion
                #region Country Code
                if (Convert.ToString(IpfItem[CompassListFields.SoldOutsideUSA]) == "Yes")
                {
                    var CountrySelected = Convert.ToString(IpfItem[CompassListFields.CountryOfSale]);
                    CountryCode = getCountryCode(CountrySelected);
                }
                #endregion
                #region Product From Description
                ProductFormDescription = Convert.ToString(IpfItem[CompassListFields.ProductFormDescription]);

                if (!string.IsNullOrEmpty(ProductFormDescription))
                {
                    ProductFormDescription = ProductFormDescription.ToUpper() + ' ';
                }
                #endregion

                SAPDescription = TBD + Brand + Season + ProductFormDescription + CustomerSpecific + Count + UnitsInsideCarton + OzWeight + PkgType + CountryCode;
            }

            return SAPDescription;
        }
        private string getCountryCode(string CountrySelected)
        {
            var CountryCode = "";
            if (CountrySelected == "Argentina")
            {
                CountryCode = " AR";
            }
            else if (CountrySelected == "Brazil")
            {
                CountryCode = " BR";
            }
            else if (CountrySelected == "Canada")
            {
                CountryCode = " CA";
            }
            else if (CountrySelected == "China")
            {
                CountryCode = " CN";
            }
            else if (CountrySelected == "France")
            {
                CountryCode = " FR";
            }
            else if (CountrySelected == "Germany")
            {
                CountryCode = " DE";
            }
            else if (CountrySelected == "Italy")
            {
                CountryCode = " IT";
            }
            else if (CountrySelected == "Mexico")
            {
                CountryCode = " MX";
            }
            else if (CountrySelected == "Other")
            {
                CountryCode = "";
            }
            else if (CountrySelected == "Poland")
            {
                CountryCode = " PL";
            }
            else if (CountrySelected == "Spain")
            {
                CountryCode = " ES";
            }
            else if (CountrySelected == "USA")
            {
                CountryCode = " US";
            }
            return CountryCode;
        }
        public void CopyCompassItem(int copyId, int newItemId, string CopyMode, bool CopyTeam = true)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        var ipItem = spList.GetItemById(copyId);
                        var item = spList.GetItemById(newItemId);
                        try
                        {
                            if (ipItem != null)
                            {
                                spWeb.AllowUnsafeUpdates = true;
                                if (item == null)
                                {
                                    item = spList.AddItem();
                                    item[CompassListFields.ProjectNumber] = ipItem[CompassListFields.ProjectNumber];
                                }

                                item[CompassListFields.NewIPF] = "Yes";
                                item[CompassListFields.SAPItemNumber] = ipItem[CompassListFields.SAPItemNumber];

                                if (Convert.ToString(ipItem[CompassListFields.TBDIndicator]) == "Yes")
                                {
                                    item[CompassListFields.SAPDescription] = SAPNomenclature(ipItem);
                                }
                                else
                                {
                                    item[CompassListFields.SAPDescription] = ipItem[CompassListFields.SAPDescription];
                                }

                                item[CompassListFields.ProjectType] = ipItem[CompassListFields.ProjectType];
                                item[CompassListFields.FirstShipDate] = ipItem[CompassListFields.FirstShipDate];
                                item[CompassListFields.ProductHierarchyLevel1] = ipItem[CompassListFields.ProductHierarchyLevel1];
                                item[CompassListFields.ManuallyCreateSAPDescription] = ipItem[CompassListFields.ManuallyCreateSAPDescription];
                                item[CompassListFields.ProductHierarchyLevel2] = ipItem[CompassListFields.ProductHierarchyLevel2];
                                item[CompassListFields.ItemConcept] = ipItem[CompassListFields.ItemConcept];
                                item[CompassListFields.TBDIndicator] = ipItem[CompassListFields.TBDIndicator];
                                item[CompassListFields.NewFormula] = ipItem[CompassListFields.NewFormula];
                                item[CompassListFields.NewFlavorColor] = ipItem[CompassListFields.NewFlavorColor];
                                item[CompassListFields.NewShape] = ipItem[CompassListFields.NewShape];
                                item[CompassListFields.NewNetWeight] = ipItem[CompassListFields.NewNetWeight];
                                item[CompassListFields.Organic] = ipItem[CompassListFields.Organic];
                                if (CopyMode == "CopyExistingIPF")
                                {
                                    item[CompassListFields.MfgLocationChange] = "";
                                }
                                else
                                {
                                    item[CompassListFields.MfgLocationChange] = ipItem[CompassListFields.MfgLocationChange];
                                }
                                item[CompassListFields.ServingSizeWeightChange] = ipItem[CompassListFields.ServingSizeWeightChange];
                                item[CompassListFields.Last12MonthSales] = ipItem[CompassListFields.Last12MonthSales];
                                item[CompassListFields.AnnualProjectedDollars] = ipItem[CompassListFields.AnnualProjectedDollars];
                                item[CompassListFields.Month1ProjectedDollars] = ipItem[CompassListFields.Month1ProjectedDollars];
                                item[CompassListFields.Month2ProjectedDollars] = ipItem[CompassListFields.Month2ProjectedDollars];
                                item[CompassListFields.Month3ProjectedDollars] = ipItem[CompassListFields.Month3ProjectedDollars];
                                item[CompassListFields.ExpectedGrossMarginPercent] = ipItem[CompassListFields.ExpectedGrossMarginPercent];
                                item[CompassListFields.RevisedGrossMarginPercent] = ipItem[CompassListFields.RevisedGrossMarginPercent];
                                item[CompassListFields.TruckLoadPricePerSellingUnit] = ipItem[CompassListFields.TruckLoadPricePerSellingUnit];
                                item[CompassListFields.AnnualProjectedUnits] = ipItem[CompassListFields.AnnualProjectedUnits];
                                item[CompassListFields.Month1ProjectedUnits] = ipItem[CompassListFields.Month1ProjectedUnits];
                                item[CompassListFields.Month2ProjectedUnits] = ipItem[CompassListFields.Month2ProjectedUnits];
                                item[CompassListFields.Month3ProjectedUnits] = ipItem[CompassListFields.Month3ProjectedUnits];
                                item[CompassListFields.CustomerSpecific] = ipItem[CompassListFields.CustomerSpecific];
                                item[CompassListFields.Customer] = ipItem[CompassListFields.Customer];
                                item[CompassListFields.CustomerSpecificLotCode] = ipItem[CompassListFields.CustomerSpecificLotCode];
                                item[CompassListFields.Channel] = ipItem[CompassListFields.Channel];
                                item[CompassListFields.SoldOutsideUSA] = ipItem[CompassListFields.SoldOutsideUSA];
                                item[CompassListFields.CountryOfSale] = ipItem[CompassListFields.CountryOfSale];
                                item[CompassListFields.MaterialGroup1Brand] = ipItem[CompassListFields.MaterialGroup1Brand];
                                item[CompassListFields.MaterialGroup4ProductForm] = ipItem[CompassListFields.MaterialGroup4ProductForm];
                                item[CompassListFields.MaterialGroup5PackType] = ipItem[CompassListFields.MaterialGroup5PackType];
                                item[CompassListFields.ProductFormDescription] = ipItem[CompassListFields.ProductFormDescription];
                                item[CompassListFields.NoveltyProject] = ipItem[CompassListFields.NoveltyProject];
                                item[CompassListFields.TotalQuantityUnitsInDisplay] = ipItem[CompassListFields.TotalQuantityUnitsInDisplay];
                                item[CompassListFields.RequireNewUPCUCC] = ipItem[CompassListFields.RequireNewUPCUCC];
                                item[CompassListFields.RequireNewUnitUPC] = ipItem[CompassListFields.RequireNewUnitUPC];
                                item[CompassListFields.UnitUPC] = ipItem[CompassListFields.UnitUPC];
                                item[CompassListFields.RequireNewDisplayBoxUPC] = ipItem[CompassListFields.RequireNewDisplayBoxUPC];
                                item[CompassListFields.DisplayBoxUPC] = ipItem[CompassListFields.DisplayBoxUPC];
                                item[CompassListFields.RequireNewCaseUCC] = ipItem[CompassListFields.RequireNewCaseUCC];
                                item[CompassListFields.CaseUCC] = ipItem[CompassListFields.CaseUCC];
                                item[CompassListFields.RequireNewPalletUCC] = ipItem[CompassListFields.RequireNewPalletUCC];
                                item[CompassListFields.PalletUCC] = ipItem[CompassListFields.PalletUCC];
                                item[CompassListFields.LikeFGItemNumber] = ipItem[CompassListFields.LikeFGItemNumber];
                                item[CompassListFields.LikeFGItemDescription] = ipItem[CompassListFields.LikeFGItemDescription];
                                item[CompassListFields.OldFGItemNumber] = ipItem[CompassListFields.OldFGItemNumber];
                                item[CompassListFields.OldFGItemDescription] = ipItem[CompassListFields.OldFGItemDescription];
                                item[CompassListFields.CaseType] = ipItem[CompassListFields.CaseType];
                                item[CompassListFields.MarketClaimsLabelingRequirements] = ipItem[CompassListFields.MarketClaimsLabelingRequirements];
                                item[CompassListFields.SAPBaseUOM] = ipItem[CompassListFields.SAPBaseUOM];
                                item[CompassListFields.NumberofTraysPerBaseUOM] = ipItem[CompassListFields.NumberofTraysPerBaseUOM];
                                item[CompassListFields.RetailSellingUnitsBaseUOM] = ipItem[CompassListFields.RetailSellingUnitsBaseUOM];
                                item[CompassListFields.RetailUnitWieghtOz] = ipItem[CompassListFields.RetailUnitWieghtOz];
                                item[CompassListFields.BaseUOMNetWeightLbs] = ipItem[CompassListFields.BaseUOMNetWeightLbs];
                                item[CompassListFields.FilmSubstrate] = ipItem[CompassListFields.FilmSubstrate];
                                item[CompassListFields.PegHoleNeeded] = ipItem[CompassListFields.PegHoleNeeded];
                                item[CompassListFields.InvolvesCarton] = ipItem[CompassListFields.InvolvesCarton];
                                item[CompassListFields.UnitsInsideCarton] = ipItem[CompassListFields.UnitsInsideCarton];
                                item[CompassListFields.IndividualPouchWeight] = ipItem[CompassListFields.IndividualPouchWeight];
                                item[CompassListFields.ProjectTypeSubCategory] = ipItem[CompassListFields.ProjectTypeSubCategory];
                                item[CompassListFields.FlowthroughDets] = ipItem[CompassListFields.FlowthroughDets];
                                item[CompassListFields.ProfitCenter] = ipItem[CompassListFields.ProfitCenter];

                                if (CopyTeam)
                                {
                                    item[CompassListFields.Initiator] = ipItem[CompassListFields.Initiator];
                                    item[CompassListFields.InitiatorName] = ipItem[CompassListFields.InitiatorName];
                                    item[CompassListFields.PM] = ipItem[CompassListFields.PM];
                                    item[CompassListFields.PMName] = ipItem[CompassListFields.PMName];
                                    item[CompassListFields.OBM] = ipItem[CompassListFields.PM];
                                    item[CompassListFields.OBMName] = ipItem[CompassListFields.PMName];
                                }
                                else
                                {
                                    item[CompassListFields.Initiator] = SPContext.Current.Web.CurrentUser;
                                    item[CompassListFields.InitiatorName] = SPContext.Current.Web.CurrentUser;

                                    item[CompassListFields.PM] = string.Empty;
                                    item[CompassListFields.PMName] = string.Empty;
                                    item[CompassListFields.OBM] = string.Empty;
                                    item[CompassListFields.OBMName] = string.Empty;
                                }
                                item.ID = newItemId;

                                if (CopyMode == "CopyExistingIPF" || CopyMode == "CopyToAlternativeParentProject")
                                {
                                    string SAPNumber = Convert.ToString(ipItem[CompassListFields.SAPItemNumber]);

                                    if (!string.IsNullOrEmpty(SAPNumber))
                                    {
                                        SAPMaterialMasterListItem mmItem = sapMMService.GetSAPMaterialMaster(SAPNumber);
                                        if ((mmItem != null) && (mmItem.SAPItemNumber != null))
                                        {
                                            if (CopyMode == "CopyExistingIPF" && Convert.ToString(item[CompassListFields.TBDIndicator]) == "No")
                                            {
                                                item[CompassListFields.SAPDescription] = string.IsNullOrEmpty(mmItem.SAPDescription) ? ipItem[CompassListFields.SAPDescription] : mmItem.SAPDescription;
                                            }

                                            item[CompassListFields.CaseUCC] = ipItem[CompassListFields.CaseUCC];
                                            item[CompassListFields.DisplayBoxUPC] = mmItem.DisplayBoxUPC;
                                            item[CompassListFields.PalletUCC] = mmItem.PalletUCC;
                                            item[CompassListFields.UnitUPC] = mmItem.UnitUPC;
                                            item[CompassListFields.TruckLoadPricePerSellingUnit] = FormatDecimal(GetDecimal(mmItem.TruckLoadPricePerSellingUnit), 2);
                                            item[CompassListFields.Last12MonthSales] = FormatDecimal(GetDecimal(mmItem.Last12MonthSales), 2);
                                            item[CompassListFields.ProductHierarchyLevel1] = mmItem.ProductHierarchyLevel1;
                                            item[CompassListFields.ProductHierarchyLevel2] = mmItem.ProductHierarchyLevel2;
                                            item[CompassListFields.MaterialGroup1Brand] = mmItem.MaterialGroup1Brand;

                                            string profitCenter = GetLookupDetailsByValueAndColumn("ProfitCenter", GlobalConstants.LIST_MaterialGroup1Lookup, "Title", mmItem.MaterialGroup1Brand, "ParentPHL2", mmItem.ProductHierarchyLevel2);
                                            item[CompassListFields.ProfitCenter] = profitCenter;

                                            item[CompassListFields.MaterialGroup4ProductForm] = mmItem.MaterialGroup4ProductForm;
                                            item[CompassListFields.MaterialGroup5PackType] = mmItem.MaterialGroup5PackType;
                                            item[CompassListFields.RetailSellingUnitsBaseUOM] = FormatDecimal(GetDecimal(mmItem.RetailSellingUnitsBaseUOM), 0);
                                            item[CompassListFields.RetailUnitWieghtOz] = FormatDecimal(GetDecimal(mmItem.RetailUnitWieghtOz), 2);

                                            double retailSellingUnitsPerBaseUOM = GetDecimal(mmItem.RetailSellingUnitsBaseUOM);
                                            double retailUnitWeight = GetDecimal(mmItem.RetailUnitWieghtOz);
                                            if ((retailSellingUnitsPerBaseUOM != 0) && (retailUnitWeight != 0))
                                            {
                                                double num = retailSellingUnitsPerBaseUOM * (retailUnitWeight / 16);
                                                item[CompassListFields.BaseUOMNetWeightLbs] = num.ToString("N2");
                                            }
                                            else
                                            {
                                                item[CompassListFields.BaseUOMNetWeightLbs] = "0.00";
                                            }
                                        }
                                    }
                                }

                                item.Update();
                                CopyCompassTeam(copyId, newItemId, spWeb, CopyTeam);
                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "IPF Service", "CopyCompassItem", string.Concat("Exception while copying compass item.", " - copyFromId:", copyId.ToString(), "- copyToId:", newItemId.ToString()));
                        }
                    }
                }
            });
        }
        public void CopyCompassList2Item(int odlCompassListItemId, int newCompassListItemId, string newProjectNumber, string CopyMode)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        try
                        {
                            SPList spCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + odlCompassListItemId + "</Value></Eq></Where>";
                            var CompassListItemsOld = spCompassList2.GetItems(spQuery);

                            if (CompassListItemsOld != null && CompassListItemsOld.Count > 0)
                            {
                                SPListItem CompassList2ItemOld = CompassListItemsOld[0];

                                #region Update Compass List 2
                                var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                                SPQuery spQueryCompassList2 = new SPQuery();
                                spQueryCompassList2.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + newCompassListItemId + "</Value></Eq></Where>";
                                var CompassList2ItemsNew = spList2.GetItems(spQueryCompassList2);
                                SPListItem CompassList2ItemNew;
                                if (CompassList2ItemsNew != null)
                                {
                                    if (CompassList2ItemsNew.Count > 0)
                                    {
                                        CompassList2ItemNew = CompassList2ItemsNew[0];
                                    }
                                    else
                                    {
                                        CompassList2ItemNew = spList2.AddItem();

                                    }

                                    CompassList2ItemNew["Title"] = newProjectNumber;
                                    CompassList2ItemNew[CompassList2Fields.CompassListItemId] = newCompassListItemId;
                                    if (CopyMode != "CopyExistingIPF")
                                    {
                                        CompassList2ItemNew[CompassList2Fields.DesignateHUBDC] = CompassList2ItemOld[CompassList2Fields.DesignateHUBDC];
                                        CompassList2ItemNew[CompassList2Fields.DeploymentModeofItem] = CompassList2ItemOld[CompassList2Fields.DeploymentModeofItem];
                                        #region SELL DCs
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoSL07] = CompassList2ItemOld[CompassList2Fields.ExtendtoSL07];
                                        CompassList2ItemNew[CompassList2Fields.SetSL07SPKto] = CompassList2ItemOld[CompassList2Fields.SetSL07SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoSL13] = CompassList2ItemOld[CompassList2Fields.ExtendtoSL13];
                                        CompassList2ItemNew[CompassList2Fields.SetSL13SPKto] = CompassList2ItemOld[CompassList2Fields.SetSL13SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoSL18] = CompassList2ItemOld[CompassList2Fields.ExtendtoSL18];
                                        CompassList2ItemNew[CompassList2Fields.SetSL18SPKto] = CompassList2ItemOld[CompassList2Fields.SetSL18SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoSL19] = CompassList2ItemOld[CompassList2Fields.ExtendtoSL19];
                                        CompassList2ItemNew[CompassList2Fields.SetSL19SPKto] = CompassList2ItemOld[CompassList2Fields.SetSL19SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoSL30] = CompassList2ItemOld[CompassList2Fields.ExtendtoSL30];
                                        CompassList2ItemNew[CompassList2Fields.SetSL30SPKto] = CompassList2ItemOld[CompassList2Fields.SetSL30SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoSL14] = CompassList2ItemOld[CompassList2Fields.ExtendtoSL14];
                                        CompassList2ItemNew[CompassList2Fields.SetSL14SPKto] = CompassList2ItemOld[CompassList2Fields.SetSL14SPKto];
                                        #endregion
                                        #region FERQ DCs
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoFQ26] = CompassList2ItemOld[CompassList2Fields.ExtendtoFQ26];
                                        CompassList2ItemNew[CompassList2Fields.SetFQ26SPKto] = CompassList2ItemOld[CompassList2Fields.SetFQ26SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoFQ27] = CompassList2ItemOld[CompassList2Fields.ExtendtoFQ27];
                                        CompassList2ItemNew[CompassList2Fields.SetFQ27SPKto] = CompassList2ItemOld[CompassList2Fields.SetFQ27SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoFQ28] = CompassList2ItemOld[CompassList2Fields.ExtendtoFQ28];
                                        CompassList2ItemNew[CompassList2Fields.SetFQ28SPKto] = CompassList2ItemOld[CompassList2Fields.SetFQ28SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoFQ29] = CompassList2ItemOld[CompassList2Fields.ExtendtoFQ29];
                                        CompassList2ItemNew[CompassList2Fields.SetFQ29SPKto] = CompassList2ItemOld[CompassList2Fields.SetFQ29SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoFQ34] = CompassList2ItemOld[CompassList2Fields.ExtendtoFQ34];
                                        CompassList2ItemNew[CompassList2Fields.SetFQ34SPKto] = CompassList2ItemOld[CompassList2Fields.SetFQ34SPKto];
                                        CompassList2ItemNew[CompassList2Fields.ExtendtoFQ35] = CompassList2ItemOld[CompassList2Fields.ExtendtoFQ35];
                                        CompassList2ItemNew[CompassList2Fields.SetFQ35SPKto] = CompassList2ItemOld[CompassList2Fields.SetFQ35SPKto];
                                        #endregion
                                    }
                                    CompassList2ItemNew[CompassList2Fields.IPFCopiedFromCompassListItemId] = odlCompassListItemId;
                                    CompassList2ItemNew[CompassList2Fields.ModifiedBy] = SPContext.Current.Web.CurrentUser.ToString();
                                    CompassList2ItemNew[CompassList2Fields.ModifiedDate] = DateTime.Now.ToString();
                                    CompassList2ItemNew.Update();
                                }
                                #endregion
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void CopyCompassTeam(int copyId, int newItemId, SPWeb spWeb, bool CopyTeam = true)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + copyId + "</Value></Eq></Where>";
            var ipItems = spList.GetItems(spQuery);
            SPQuery spQuery2 = new SPQuery();
            spQuery2.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + newItemId + "</Value></Eq></Where>";
            var items = spList.GetItems(spQuery2);
            try
            {
                if (ipItems != null)
                {
                    var ipItem = ipItems[0];
                    SPListItem item;
                    spWeb.AllowUnsafeUpdates = true;
                    if (items == null)
                    {
                        item = spList.AddItem();
                        item["Title"] = ipItem[CompassListFields.ProjectNumber];
                        item[CompassListFields.CompassListItemId] = newItemId;
                    }
                    else
                    {
                        item = items[0];
                    }

                    if (CopyTeam)
                    {
                        item[CompassTeamListFields.ProjectLeader] = ipItem[CompassTeamListFields.ProjectLeader];
                        item[CompassTeamListFields.ProjectLeaderName] = ipItem[CompassTeamListFields.ProjectLeaderName];
                        item[CompassTeamListFields.SeniorProjectManager] = ipItem[CompassTeamListFields.SeniorProjectManager];
                        item[CompassTeamListFields.SeniorProjectManagerName] = ipItem[CompassTeamListFields.SeniorProjectManagerName];
                        item[CompassTeamListFields.QAInnovation] = ipItem[CompassTeamListFields.QAInnovation];
                        item[CompassTeamListFields.QAInnovationName] = ipItem[CompassTeamListFields.QAInnovationName];
                        item[CompassTeamListFields.InTech] = ipItem[CompassTeamListFields.InTech];
                        item[CompassTeamListFields.InTechName] = ipItem[CompassTeamListFields.InTechName];
                        item[CompassTeamListFields.InTechRegulatory] = ipItem[CompassTeamListFields.InTechRegulatory];
                        item[CompassTeamListFields.InTechRegulatoryName] = ipItem[CompassTeamListFields.InTechRegulatoryName];
                        item[CompassTeamListFields.RegulatoryQA] = ipItem[CompassTeamListFields.RegulatoryQA];
                        item[CompassTeamListFields.RegulatoryQAName] = ipItem[CompassTeamListFields.RegulatoryQAName];
                        item[CompassTeamListFields.PackagingEngineering] = ipItem[CompassTeamListFields.PackagingEngineering];
                        item[CompassTeamListFields.PackagingEngineeringName] = ipItem[CompassTeamListFields.PackagingEngineeringName];
                        item[CompassTeamListFields.SupplyChain] = ipItem[CompassTeamListFields.SupplyChain];
                        item[CompassTeamListFields.SupplyChainName] = ipItem[CompassTeamListFields.SupplyChainName];
                        item[CompassTeamListFields.Finance] = ipItem[CompassTeamListFields.Finance];
                        item[CompassTeamListFields.FinanceName] = ipItem[CompassTeamListFields.FinanceName];
                        item[CompassTeamListFields.Sales] = ipItem[CompassTeamListFields.Sales];
                        item[CompassTeamListFields.SalesName] = ipItem[CompassTeamListFields.SalesName];
                        item[CompassTeamListFields.Manufacturing] = ipItem[CompassTeamListFields.Manufacturing];
                        item[CompassTeamListFields.ManufacturingName] = ipItem[CompassTeamListFields.ManufacturingName];
                        item[CompassTeamListFields.OtherMember] = ipItem[CompassTeamListFields.OtherMember];
                        item[CompassTeamListFields.OtherMemberName] = ipItem[CompassTeamListFields.OtherMemberName];
                        item[CompassTeamListFields.LifeCycleManagement] = ipItem[CompassTeamListFields.LifeCycleManagement];
                        item[CompassTeamListFields.LifeCycleManagementName] = ipItem[CompassTeamListFields.LifeCycleManagementName];
                        item[CompassTeamListFields.PackagingProcurement] = ipItem[CompassTeamListFields.PackagingProcurement];
                        item[CompassTeamListFields.PackagingProcurementName] = ipItem[CompassTeamListFields.PackagingProcurementName];
                        item[CompassTeamListFields.ExtMfgProcurement] = ipItem[CompassTeamListFields.ExtMfgProcurement];
                        item[CompassTeamListFields.ExtMfgProcurementName] = ipItem[CompassTeamListFields.ExtMfgProcurementName];
                        item[CompassTeamListFields.Marketing] = ipItem[CompassTeamListFields.Marketing];
                        item[CompassTeamListFields.MarketingName] = ipItem[CompassTeamListFields.MarketingName];
                    }
                    else
                    {
                        item[CompassTeamListFields.ProjectLeader] = string.Empty;
                        item[CompassTeamListFields.ProjectLeaderName] = string.Empty;
                        item[CompassTeamListFields.SeniorProjectManager] = string.Empty;
                        item[CompassTeamListFields.SeniorProjectManagerName] = string.Empty;
                        item[CompassTeamListFields.QAInnovation] = string.Empty;
                        item[CompassTeamListFields.QAInnovationName] = string.Empty;
                        item[CompassTeamListFields.InTech] = string.Empty;
                        item[CompassTeamListFields.InTechName] = string.Empty;
                        item[CompassTeamListFields.InTechRegulatory] = string.Empty;
                        item[CompassTeamListFields.InTechRegulatoryName] = string.Empty;
                        item[CompassTeamListFields.RegulatoryQA] = string.Empty;
                        item[CompassTeamListFields.RegulatoryQAName] = string.Empty;
                        item[CompassTeamListFields.PackagingEngineering] = string.Empty;
                        item[CompassTeamListFields.PackagingEngineeringName] = string.Empty;
                        item[CompassTeamListFields.SupplyChain] = string.Empty;
                        item[CompassTeamListFields.SupplyChainName] = string.Empty;
                        item[CompassTeamListFields.Finance] = string.Empty;
                        item[CompassTeamListFields.FinanceName] = string.Empty;
                        item[CompassTeamListFields.Sales] = string.Empty;
                        item[CompassTeamListFields.SalesName] = string.Empty;
                        item[CompassTeamListFields.Manufacturing] = string.Empty;
                        item[CompassTeamListFields.ManufacturingName] = string.Empty;
                        item[CompassTeamListFields.OtherMember] = string.Empty;
                        item[CompassTeamListFields.OtherMemberName] = string.Empty;
                        item[CompassTeamListFields.LifeCycleManagement] = string.Empty;
                        item[CompassTeamListFields.LifeCycleManagementName] = string.Empty;
                        item[CompassTeamListFields.PackagingProcurement] = string.Empty;
                        item[CompassTeamListFields.PackagingProcurementName] = string.Empty;
                        item[CompassTeamListFields.ExtMfgProcurement] = string.Empty;
                        item[CompassTeamListFields.ExtMfgProcurementName] = string.Empty;
                        item[CompassTeamListFields.Marketing] = string.Empty;
                        item[CompassTeamListFields.MarketingName] = string.Empty;
                    }

                    item.ID = newItemId;
                    item.Update();
                    spWeb.AllowUnsafeUpdates = false;
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "IPF Service", "CopyCompassTeamItem", string.Concat("Exception while copying compass team item.", " - copyFromId:", copyId.ToString(), "- copyToId:", newItemId.ToString()));
            }
        }
        public void CopyFormsFromPreviousprojects(int PreviousItemId, int CurrentItemId, string newProjectNumber)
        {
            var PreviousProjectNumber = "";
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        try
                        {
                            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                            var PreviousItem = spList.GetItemById(PreviousItemId);
                            var Currentitem = spList.GetItemById(CurrentItemId);

                            if (PreviousItem != null)
                            {
                                spWeb.AllowUnsafeUpdates = true;
                                if (Currentitem == null)
                                {
                                    Currentitem = spList.AddItem();
                                    Currentitem[CompassListFields.ProjectNumber] = newProjectNumber;
                                    Currentitem[CompassListFields.CompassListItemId] = CurrentItemId;
                                }
                                PreviousProjectNumber = Convert.ToString(PreviousItem[CompassListFields.ProjectNumber]);
                                //OPS
                                Currentitem[CompassListFields.ManufacturingLocation] = PreviousItem[CompassListFields.ManufacturingLocation];
                                Currentitem[CompassListFields.ManufacturerCountryOfOrigin] = PreviousItem[CompassListFields.ManufacturerCountryOfOrigin];
                                Currentitem[CompassListFields.PackingLocation] = PreviousItem[CompassListFields.PackingLocation];
                                Currentitem[CompassListFields.MfgLocationChange] = PreviousItem[CompassListFields.MfgLocationChange];

                                //External Manufacturing
                                Currentitem[CompassListFields.CoManufacturingClassification] = PreviousItem[CompassListFields.CoManufacturingClassification];
                                Currentitem[CompassListFields.DoesBulkSemiExistToBringInHouse] = PreviousItem[CompassListFields.DoesBulkSemiExistToBringInHouse];
                                Currentitem[CompassListFields.ExistingBulkSemiNumber] = PreviousItem[CompassListFields.ExistingBulkSemiNumber];
                                Currentitem[CompassListFields.BulkSemiDescription] = PreviousItem[CompassListFields.BulkSemiDescription];
                                Currentitem[CompassListFields.ExternalManufacturer] = PreviousItem[CompassListFields.ExternalManufacturer];
                                Currentitem[CompassListFields.ExternalPacker] = PreviousItem[CompassListFields.ExternalPacker];
                                Currentitem[CompassListFields.PurchasedIntoLocation] = PreviousItem[CompassListFields.PurchasedIntoLocation];
                                Currentitem[CompassListFields.CurrentTimelineAcceptable] = PreviousItem[CompassListFields.CurrentTimelineAcceptable];
                                Currentitem[CompassListFields.LeadTimeFromSupplier] = PreviousItem[CompassListFields.LeadTimeFromSupplier];

                                //InTech Reg

                                //Procuremtent
                                //Currentitem[CompassListFields.PackagingEngineerLead] = PreviousItem[CompassListFields.PackagingEngineerLead];

                                //PE2

                                Currentitem.Update();

                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "IPF Service", "CopyFormsFromPreviousprojects", string.Concat("Exception while copying .", " - PreviousItemId:", PreviousItemId.ToString(), "- CurrentItemId:", CurrentItemId.ToString()));
                        }
                    }
                }
            });
            CopyPackagingItemsFromPreviousprojects(PreviousItemId, CurrentItemId);
            CopyMarketingClaimsItemFromPreviousProject(PreviousItemId, CurrentItemId, newProjectNumber);
            CopyPackMeasurementsFromPreviousProject(PreviousItemId, CurrentItemId, newProjectNumber);
            if (!string.IsNullOrEmpty(PreviousProjectNumber))
            {
                CopyNLEAFilesFromPreviousProject(PreviousProjectNumber, newProjectNumber);
            }
        }
        private void CopyPackagingItemsFromPreviousprojects(int PreviousItemId, int CurrentItemId)
        {
            List<PackagingItem> SourceProjectItems = GetAllPackagingItemsForProject(PreviousItemId);

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (SPWeb spWeb = spSite.OpenWeb())
                            {
                                spWeb.AllowUnsafeUpdates = true;

                                foreach (var SourceProjectItem in SourceProjectItems)
                                {
                                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                                    SPQuery spQuery = new SPQuery();
                                    spQuery.Query =
                                            "<Where>" +
                                                "<And>" +
                                                    "<Eq><FieldRef Name=\"MaterialNumber\" /><Value Type=\"Text\">" + SourceProjectItem.MaterialNumber + "</Value></Eq>" +
                                                    "<Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + CurrentItemId + "</Value></Eq>" +
                                                "</And>" +
                                            "</Where>";
                                    spQuery.RowLimit = 1;

                                    SPListItemCollection packageItemCol = spList.GetItems(spQuery);
                                    if (packageItemCol.Count > 0)
                                    {
                                        SPListItem packageItem = packageItemCol[0];
                                        if (packageItem != null)
                                        {
                                            //OPS
                                            packageItem[PackagingItemListFields.TransferSEMIMakePackLocations] = SourceProjectItem.TransferSEMIMakePackLocations;
                                            packageItem[PackagingItemListFields.CountryOfOrigin] = SourceProjectItem.CountryOfOrigin;
                                            packageItem[PackagingItemListFields.PackLocation] = SourceProjectItem.PackLocation;
                                            packageItem[PackagingItemListFields.Flowthrough] = SourceProjectItem.Flowthrough;

                                            //Ext Mfg
                                            packageItem[PackagingItemListFields.PrinterSupplier] = SourceProjectItem.PrinterSupplier;
                                            packageItem[PackagingItemListFields.TransferSEMIMakePackLocations] = SourceProjectItem.TransferSEMIMakePackLocations;
                                            packageItem[PackagingItemListFields.CountryOfOrigin] = SourceProjectItem.CountryOfOrigin;
                                            packageItem[PackagingItemListFields.PackLocation] = SourceProjectItem.PackLocation;
                                            packageItem[PackagingItemListFields.Notes] = SourceProjectItem.Notes;

                                            //InTech
                                            packageItem[PackagingItemListFields.ShelfLife] = SourceProjectItem.ShelfLife;
                                            packageItem[PackagingItemListFields.IngredientsNeedToClaimBioEng] = SourceProjectItem.IngredientsNeedToClaimBioEng;

                                            //Proc
                                            packageItem[PackagingItemListFields.PackQuantity] = SourceProjectItem.PackQuantity;
                                            packageItem[PackagingItemListFields.PackUnit] = SourceProjectItem.PackUnit;
                                            packageItem[PackagingItemListFields.ExternalGraphicsVendor] = SourceProjectItem.ExternalGraphicsVendor;
                                            packageItem[PackagingItemListFields.SpecificationNo] = SourceProjectItem.SpecificationNo;
                                            packageItem[PackagingItemListFields.PurchasedIntoLocation] = SourceProjectItem.PurchasedIntoLocation;
                                            packageItem[PackagingItemListFields.GraphicsBrief] = SourceProjectItem.GraphicsBrief;
                                            packageItem[PackagingItemListFields.PrinterSupplier] = SourceProjectItem.PrinterSupplier;
                                            packageItem[PackagingItemListFields.LeadMaterialTime] = SourceProjectItem.LeadMaterialTime;
                                            packageItem[PackagingItemListFields.LeadPlateTime] = SourceProjectItem.LeadPlateTime;
                                            packageItem[PackagingItemListFields.MakeLocation] = SourceProjectItem.MakeLocation;
                                            packageItem[PackagingItemListFields.CountryOfOrigin] = SourceProjectItem.CountryOfOrigin;
                                            packageItem[PackagingItemListFields.PackLocation] = SourceProjectItem.PackLocation;
                                            packageItem[PackagingItemListFields.TransferSEMIMakePackLocations] = SourceProjectItem.TransferSEMIMakePackLocations;
                                            packageItem[PackagingItemListFields.FilmPrintStyle] = SourceProjectItem.FilmPrintStyle;
                                            packageItem[PackagingItemListFields.CorrugatedPrintStyle] = SourceProjectItem.CorrugatedPrintStyle;
                                            packageItem[PackagingItemListFields.TransferSEMIMakePackLocations] = SourceProjectItem.TransferSEMIMakePackLocations;
                                            packageItem[PackagingItemListFields.IsAllProcInfoCorrect] = string.Empty;
                                            packageItem[PackagingItemListFields.WhatProcInfoHasChanged] = string.Empty;

                                            //PE2

                                            packageItem[PackagingItemListFields.DielineLink] = SourceProjectItem.DielineURL;
                                            packageItem[PackagingItemListFields.LastFormUpdated] = GlobalConstants.PAGE_ItemProposal;
                                            packageItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                            packageItem.Update();
                                        }
                                    }
                                }
                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "IPF Service", "CopyPackagingItemsFromPreviousprojects", string.Concat("Exception while copying compass item.", " - PreviousItemId:", PreviousItemId.ToString(), "- CurrentItemId:", CurrentItemId.ToString()));
            }
        }
        private void CopyMarketingClaimsItemFromPreviousProject(int PreviousItemId, int CurrentItemId, string newProjectNumber)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (var spSite = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (var spWeb = spSite.OpenWeb())
                            {
                                SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                                SPQuery spQuery = new SPQuery();
                                spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + PreviousItemId + "</Value></Eq></Where>";

                                SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                                if (compassItemCol.Count > 0)
                                {
                                    SPQuery spQuery2 = new SPQuery();
                                    spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + CurrentItemId + "</Value></Eq></Where>";
                                    SPListItemCollection compassItemCol2 = spList.GetItems(spQuery2);
                                    SPListItem newItem = null;
                                    var copyItem = compassItemCol[0];
                                    if ((copyItem != null))
                                    {
                                        if (compassItemCol2.Count <= 0)
                                        {
                                            newItem = spList.AddItem();
                                        }
                                        else
                                        {
                                            newItem = compassItemCol2[0];
                                        }

                                        spWeb.AllowUnsafeUpdates = true;

                                        newItem[MarketingClaimsListFields.CompassListItemId] = CurrentItemId;
                                        newItem[MarketingClaimsListFields.Title] = newProjectNumber;
                                        newItem[MarketingClaimsListFields.AllergenMilk] = copyItem[MarketingClaimsListFields.AllergenMilk];
                                        newItem[MarketingClaimsListFields.AllergenEggs] = copyItem[MarketingClaimsListFields.AllergenEggs];
                                        newItem[MarketingClaimsListFields.AllergenPeanuts] = copyItem[MarketingClaimsListFields.AllergenPeanuts];
                                        newItem[MarketingClaimsListFields.AllergenCoconut] = copyItem[MarketingClaimsListFields.AllergenCoconut];
                                        newItem[MarketingClaimsListFields.AllergenAlmonds] = copyItem[MarketingClaimsListFields.AllergenAlmonds];
                                        newItem[MarketingClaimsListFields.AllergenSoy] = copyItem[MarketingClaimsListFields.AllergenSoy];
                                        newItem[MarketingClaimsListFields.AllergenWheat] = copyItem[MarketingClaimsListFields.AllergenWheat];
                                        newItem[MarketingClaimsListFields.AllergenHazelNuts] = copyItem[MarketingClaimsListFields.AllergenHazelNuts];
                                        newItem[MarketingClaimsListFields.AllergenOther] = copyItem[MarketingClaimsListFields.AllergenOther];

                                        newItem.Update();
                                    }
                                }
                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "IPF Service", "CopyMarketingClaimsItemFromPreviousProject", string.Concat("Exception while copying.", " - PreviousItemId:", PreviousItemId.ToString(), "- CurrentItemId:", CurrentItemId.ToString()));
            }
        }
        private void CopyPackMeasurementsFromPreviousProject(int PreviousItemId, int CurrentItemId, string newProjectNumber)
        {
            try
            {
                List<BOMSetupItem> SourcePackMeasurementsItems = GetPackMeasurementsItems(PreviousItemId);
                List<PackagingItem> SourceProjectItems = GetAllPackagingItemsForProject(PreviousItemId);
                List<PackagingItem> TargetProjectItems = GetAllPackagingItemsForProject(CurrentItemId);


                SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        using (var spSite = new SPSite(SPContext.Current.Web.Url))
                        {
                            using (var spWeb = spSite.OpenWeb())
                            {
                                foreach (BOMSetupItem PackMeasurementsItem in SourcePackMeasurementsItems)
                                {
                                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                                    PackagingItem TargetComp = new PackagingItem();
                                    Int32 parentId;
                                    var MaterialNumber = "";
                                    if (PackMeasurementsItem.ParentID == 0)
                                    {
                                        parentId = 0;
                                    }
                                    else
                                    {
                                        MaterialNumber =
                                        (
                                            from
                                                SourceProjectItem in SourceProjectItems
                                            where
                                                SourceProjectItem.Id == PackMeasurementsItem.ParentID
                                            select
                                                SourceProjectItem.MaterialNumber
                                          ).FirstOrDefault();

                                        TargetComp =
                                        (
                                            from
                                                TargetProjectItem in TargetProjectItems
                                            where
                                                TargetProjectItem.MaterialNumber == MaterialNumber
                                            select
                                                TargetProjectItem
                                          ).FirstOrDefault();

                                        parentId = TargetComp.Id;
                                    }

                                    SPQuery spQuery2 = new SPQuery();
                                    spQuery2.Query = "<Where>" +
                                                        "<And>" +
                                                            "<Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + CurrentItemId + "</Value></Eq>" +
                                                            "<Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Text\">" + parentId + "</Value></Eq>" +
                                                        "</And>" +
                                                      "</Where>";
                                    SPListItemCollection compassItemCol2 = spList.GetItems(spQuery2);
                                    SPListItem newItem = null;
                                    if ((compassItemCol2 != null))
                                    {
                                        if (compassItemCol2.Count <= 0)
                                        {
                                            newItem = spList.AddItem();
                                        }
                                        else
                                        {
                                            newItem = compassItemCol2[0];
                                        }

                                        spWeb.AllowUnsafeUpdates = true;

                                        newItem[CompassPackMeasurementsFields.CompassListItemId] = CurrentItemId;
                                        newItem[CompassPackMeasurementsFields.Title] = newProjectNumber;

                                        newItem[CompassPackMeasurementsFields.SAPSpecsChange] = PackMeasurementsItem.SAPSpecsChange;
                                        newItem[CompassPackMeasurementsFields.NotesSpec] = PackMeasurementsItem.NotesSpec;
                                        newItem[CompassPackMeasurementsFields.PackSpecNumber] = PackMeasurementsItem.PackSpecNumber;
                                        newItem[CompassPackMeasurementsFields.PalletSpecNumber] = PackMeasurementsItem.PalletSpecNumber;
                                        newItem[CompassPackMeasurementsFields.PalletSpecLink] = PackMeasurementsItem.PalletSpecLink;
                                        newItem[CompassPackMeasurementsFields.ParentComponentId] = parentId;

                                        newItem.Update();
                                    }
                                }
                                spWeb.AllowUnsafeUpdates = false;
                            }
                        }
                    });
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "IPF Service", "CopyPackMeasurementsFromPreviousProject", string.Concat("Exception while copying .", " - PreviousItemId:", PreviousItemId.ToString(), "- CurrentItemId:", CurrentItemId.ToString()));
            }
        }
        private void CopyNLEAFilesFromPreviousProject(string PreviousProjectNumber, string CurrentProjectNumber)
        {
            try
            {
                SPDocumentLibrary documentLib;
                SPListItem targetFolderI, newItem;
                List<SPFile> spfiles;
                SPFolder sourceFolder, targetFolder;
                string urlNewname, newName;
                SPSecurity.RunWithElevatedPrivileges(delegate ()
                {
                    using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                    {
                        using (SPWeb spweb = spsite.OpenWeb())
                        {
                            spweb.AllowUnsafeUpdates = true;
                            documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                            string targetFolderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", CurrentProjectNumber);
                            string sourceFolderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", PreviousProjectNumber);
                            if (!spweb.GetFolder(sourceFolderUrl).Exists)
                            {
                                spweb.AllowUnsafeUpdates = false;
                                return;
                            }
                            sourceFolder = spweb.GetFolder(sourceFolderUrl);
                            if (!spweb.GetFolder(targetFolderUrl).Exists)
                            {
                                targetFolderI = documentLib.Items.Add("", SPFileSystemObjectType.Folder, CurrentProjectNumber);
                                targetFolderI.Update();
                            }
                            if (!spweb.GetFolder(targetFolderUrl).Exists)
                            {
                                spweb.AllowUnsafeUpdates = false;
                                return;
                            }
                            targetFolder = spweb.GetFolder(targetFolderUrl);
                            spfiles = sourceFolder.Files.OfType<SPFile>().Where(x => Convert.ToString(x.Item[CompassListFields.DOCLIBRARY_CompassDocType]).Equals(GlobalConstants.DOCTYPE_NLEA)).ToList();
                            foreach (SPFile spfile in spfiles)
                            {
                                newName = spfile.Name;
                                urlNewname = string.Concat(targetFolderUrl, "/", newName);
                                spfile.CopyTo(urlNewname);
                                newItem = spweb.GetListItem(urlNewname);
                                if (newItem != null)
                                {
                                    newItem.Update();
                                }
                            }
                            spweb.AllowUnsafeUpdates = false;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "IPF Service", "CopyNLEAFilesFromPreviousProject", string.Concat("Exception while copying .", " - Previous Project Number:", PreviousProjectNumber.ToString(), "- Current Project Number:", CurrentProjectNumber.ToString()));

            }
        }
        private List<PackagingItem> GetAllPackagingItemsForProject(int compassListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                            packagingItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                            packagingItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ConfirmedNLEA = Convert.ToString(item[PackagingItemListFields.ConfirmedNLEA]);
                            packagingItem.Length = Convert.ToString(item[PackagingItemListFields.Length]);
                            packagingItem.Width = Convert.ToString(item[PackagingItemListFields.Width]);
                            packagingItem.Height = Convert.ToString(item[PackagingItemListFields.Height]);
                            packagingItem.CADDrawing = Convert.ToString(item[PackagingItemListFields.CADDrawing]);
                            packagingItem.Structure = Convert.ToString(item[PackagingItemListFields.Structure]);
                            packagingItem.StructureColor = Convert.ToString(item[PackagingItemListFields.StructureColor]);
                            packagingItem.BackSeam = Convert.ToString(item[PackagingItemListFields.BackSeam]);
                            packagingItem.WebWidth = Convert.ToString(item[PackagingItemListFields.WebWidth]);
                            packagingItem.ExactCutOff = Convert.ToString(item[PackagingItemListFields.ExactCutOff]);
                            packagingItem.BagFace = Convert.ToString(item[PackagingItemListFields.BagFace]);
                            packagingItem.Unwind = Convert.ToString(item[PackagingItemListFields.Unwind]);
                            packagingItem.Description = Convert.ToString(item[PackagingItemListFields.Description]);

                            packagingItem.FilmMaxRollOD = Convert.ToString(item[PackagingItemListFields.FilmMaxRollOD]);
                            packagingItem.FilmRollID = Convert.ToString(item[PackagingItemListFields.FilmRollID]);
                            packagingItem.FilmPrintStyle = Convert.ToString(item[PackagingItemListFields.FilmPrintStyle]);
                            packagingItem.FilmStyle = Convert.ToString(item[PackagingItemListFields.FilmStyle]);
                            packagingItem.CorrugatedPrintStyle = Convert.ToString(item[PackagingItemListFields.CorrugatedPrintStyle]);
                            packagingItem.GraphicsChangeRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                            packagingItem.ExternalGraphicsVendor = Convert.ToString(item[PackagingItemListFields.ExternalGraphicsVendor]);
                            packagingItem.GraphicsBrief = Convert.ToString(item[PackagingItemListFields.GraphicsBrief]);
                            packagingItem.BOMEffectiveDate = Convert.ToDateTime(item[PackagingItemListFields.BOMEffectiveDate]);
                            packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                            packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                            packagingItem.MRPC = Convert.ToString(item[PackagingItemListFields.MRPC]);

                            packagingItem.ReceivingPlant = Convert.ToString(item[PackagingItemListFields.ReceivingPlant]);
                            packagingItem.CostingUnit = Convert.ToString(item[PackagingItemListFields.CostingUnit]);
                            packagingItem.EachesPerCostingUnit = Convert.ToString(item[PackagingItemListFields.EachesPerCostingUnit]);
                            packagingItem.LBPerCostingUnit = Convert.ToString(item[PackagingItemListFields.LBPerCostingUnit]);
                            packagingItem.CostingUnitPerPallet = Convert.ToString(item[PackagingItemListFields.CostingUnitPerPallet]);
                            packagingItem.QuantityQuote = Convert.ToString(item[PackagingItemListFields.QuantityQuote]);
                            packagingItem.StandardCost = Convert.ToString(item[PackagingItemListFields.StandardCost]);
                            packagingItem.VendorNumber = Convert.ToString(item[PackagingItemListFields.VendorNumber]);
                            packagingItem.StandardOrderingQuantity = Convert.ToString(item[PackagingItemListFields.StandardOrderingQuantity]);
                            packagingItem.OrderUOM = Convert.ToString(item[PackagingItemListFields.OrderUOM]);
                            packagingItem.Incoterms = Convert.ToString(item[PackagingItemListFields.Incoterms]);
                            packagingItem.XferOfOwnership = Convert.ToString(item[PackagingItemListFields.XferOfOwnership]);
                            packagingItem.PRDateCategory = Convert.ToString(item[PackagingItemListFields.PRDateCategory]);
                            packagingItem.VendorMaterialNumber = Convert.ToString(item[PackagingItemListFields.VendorMaterialNumber]);
                            packagingItem.CostingCondition = Convert.ToString(item[PackagingItemListFields.CostingCondition]);

                            packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                            packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                            packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            packagingItem.ReviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            packagingItem.FourteenDigitBarCode = Convert.ToString(item[PackagingItemListFields.FourteenDigitBarcode]);

                            packagingItem.SpecificationNo = Convert.ToString(item[PackagingItemListFields.SpecificationNo]);
                            packagingItem.DielineURL = Convert.ToString(item[PackagingItemListFields.DielineLink]);

                            packagingItem.PHL1 = Convert.ToString(item[PackagingItemListFields.PHL1]);
                            packagingItem.PHL2 = Convert.ToString(item[PackagingItemListFields.PHL2]);
                            packagingItem.Brand = Convert.ToString(item[PackagingItemListFields.Brand]);
                            packagingItem.ProfitCenter = Convert.ToString(item[PackagingItemListFields.ProfitCenter]);
                            packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            packagingItem.PurchasedIntoLocation = Convert.ToString(item[PackagingItemListFields.PurchasedIntoLocation]);
                            packagingItem.IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        private List<BOMSetupItem> GetPackMeasurementsItems(int itemId)
        {
            List<BOMSetupItem> pmItems = new List<BOMSetupItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";

                    // spQuery.RowLimit = 1;
                    SPListItemCollection compassItemCol;

                    compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (item != null)
                            {
                                BOMSetupItem pmItem = new BOMSetupItem();
                                pmItem.Id = item.ID;
                                pmItem.CompassListItemId = Convert.ToInt32(item[CompassPackMeasurementsFields.CompassListItemId]);

                                pmItem.SAPSpecsChange = Convert.ToString(item[CompassPackMeasurementsFields.SAPSpecsChange]);
                                pmItem.NotesSpec = Convert.ToString(item[CompassPackMeasurementsFields.NotesSpec]);
                                pmItem.PackSpecNumber = Convert.ToString(item[CompassPackMeasurementsFields.PackSpecNumber]);
                                pmItem.PalletSpecNumber = Convert.ToString(item[CompassPackMeasurementsFields.PalletSpecNumber]);

                                pmItem.PalletSpecLink = Convert.ToString(item[CompassPackMeasurementsFields.PalletSpecLink]);
                                pmItem.ParentID = Convert.ToInt32(item[CompassPackMeasurementsFields.ParentComponentId]);
                                pmItems.Add(pmItem);
                            }
                        }
                    }

                    foreach (var pmItem in pmItems)
                    {
                        if (pmItem.ParentID != 0)
                        {
                            SPList spPackagingList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                            SPListItem compassPackagingItem = spPackagingList.GetItemById(pmItem.ParentID);

                            if (compassPackagingItem != null)
                            {
                                pmItem.PackagingComponent = Convert.ToString(compassPackagingItem[PackagingItemListFields.PackagingComponent]);
                                pmItem.MaterialDescription = Convert.ToString(compassPackagingItem[PackagingItemListFields.MaterialDescription]);
                                pmItem.MaterialNumber = Convert.ToString(compassPackagingItem[PackagingItemListFields.MaterialNumber]);
                            }
                        }
                    }
                }
            }
            return pmItems;
        }
        public void CancelItemProposals(string projectId, string ProjectRejected)
        {
            string newProjectNumber = projectId;
            string lastCharacter = string.Empty;
            Boolean bDone = false;
            int value;
            byte[] asciiValue;

            // Keep checking previous characters for Z or non-alphabetic
            while (!bDone)
            {
                lastCharacter = projectId.Substring(projectId.Length - 1);
                asciiValue = Encoding.ASCII.GetBytes(lastCharacter);
                value = asciiValue[0];
                if ((value >= 65) && (value <= 90))
                {
                    // Found another alphabetic... so remove
                    projectId = projectId.Substring(0, projectId.Length - 1);
                }
                else
                {
                    // No letter found
                    bDone = true;
                }
            }

            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Contains><FieldRef Name=\"ProjectNumber\" /><Value Type=\"Text\">" + projectId + "</Value></Contains></Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        bool bCancel = true;
                        foreach (SPListItem item in compassItemCol)
                        {
                            bCancel = true;
                            // Since we are getting all projects that contain the project number, we need to ensure we don't 
                            // cancel 2016-669 when cancelling project 2016-66 since 2016-669 contains 2016-66
                            string projectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);

                            // To ensure we are only cancelling 2016-66, 2016-66A, 2016-66AAA etc, we will check the next character after 2016-66 
                            // to ensure it is a letter. If it is a number, then we have a different project number e.g. 2016-669 or 2016-669A
                            if (projectId.Length < projectNumber.Length)
                            {
                                // Get the next character in the project number beyond the current length of the cancelling project number
                                lastCharacter = projectNumber.Substring(projectId.Length, 1);
                                asciiValue = Encoding.ASCII.GetBytes(lastCharacter);
                                value = asciiValue[0];
                                if ((value >= 65) && (value <= 90))
                                    bCancel = true;
                                else
                                    bCancel = false;
                            }

                            if (bCancel)
                                CancelItemProposal(item.ID, newProjectNumber, ProjectRejected);
                        }
                    }
                }
            }
        }
        public void CancelItemProposal(int oldItemId, string newProjectId, string ProjectRejected)
        {
            var ChangeRequestLink = new SPFieldUrlValue();
            ChangeRequestLink.Description = newProjectId;
            ChangeRequestLink.Url = string.Concat(SPContext.Current.Web.Url, "/pages/", GlobalConstants.PAGE_ItemProposal, "?", GlobalConstants.QUERYSTRING_ProjectNo, "=", newProjectId);

            if (ProjectRejected == "Yes")
            {
                notesService.UpdateProjectComments(oldItemId, string.Concat("Reason for cancellation:", " Project was not approved. A change request was performed - <a href=\"", ChangeRequestLink.Url, "\">", newProjectId, "</a>"));
            }
            else
            {
                UpdateProjectCancellationReasons(oldItemId);
                notesService.UpdateProjectComments(oldItemId, string.Concat("Reason for cancellation:", " Project Cancelled due to the change request - <a href=\"", ChangeRequestLink.Url, "\">", newProjectId, "</a>"));
            }
            workflowServices.UpdateWorkflowPhaseForChangeRequest(oldItemId, GlobalConstants.WORKFLOWPHASE_Cancelled, ChangeRequestLink);
        }
        public void UpdateProjectCancellationReasons(int CompassListItemId)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[CompassProjectDecisionsListFields.CancellationReasons] = "Project Cancelled due to the change request.";
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
        #region Approval Methods
        public ApprovalItem GetItemProposalApprovalItem(int itemId)
        {
            ApprovalItem appItem = new ApprovalItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            appItem.ApprovalListItemId = item.ID;
                            appItem.CompassListItemId = Convert.ToInt32(item[ApprovalListFields.CompassListItemId]);
                            // IPF Fields
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.IPF_SubmittedBy]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.IPF_SubmittedDate]);
                            //appItem.IPF_ResubmittedBy = Convert.ToString(item[ApprovalListFields.IPF_ResubmittedBy]);
                            //appItem.IPF_ResubmittedDate = Convert.ToString(item[ApprovalListFields.IPF_ResubmittedDate]);
                            //appItem.IPF_ResubmittedStartDate = Convert.ToString(item[ApprovalListFields.IPF_ResubmittedStartDate]);
                        }
                    }
                }
            }
            return appItem;
        }
        public int InsertApprovalItem(ApprovalItem approvalListItem, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);

                        if (approvalListItem.CompassListItemId > 0)
                        {
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalListItem.CompassListItemId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                            SPListItem appItem;

                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                appItem = compassItemCol[0];
                            }
                            else
                            {
                                appItem = spList.AddItem();

                                appItem["Title"] = title;
                                appItem[ApprovalListFields.CompassListItemId] = approvalListItem.CompassListItemId;

                            }
                            // IPF Fields
                            appItem[ApprovalListFields.IPF_ModifiedBy] = approvalListItem.ModifiedBy;
                            appItem[ApprovalListFields.IPF_ModifiedDate] = approvalListItem.ModifiedDate;
                            appItem.Update();
                            id = appItem.ID;
                        }

                        SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);

                        if (approvalListItem.CompassListItemId > 0)
                        {
                            SPQuery spQuery2 = new SPQuery();
                            spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalListItem.CompassListItemId + "</Value></Eq></Where>";
                            spQuery2.RowLimit = 1;
                            SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
                            SPListItem appItem2;
                            if (compassItemCol2 != null)
                            {
                                if (compassItemCol2.Count > 0)
                                {
                                    appItem2 = compassItemCol2[0];
                                }
                                else
                                {
                                    appItem2 = spList2.AddItem();
                                    appItem2["Title"] = title;
                                    appItem2[ApprovalListFields.CompassListItemId] = approvalListItem.CompassListItemId;
                                }
                                appItem2.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return id;
        }
        public void UpdateItemProposalApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                if (bSubmitted)
                                {
                                    try
                                    {
                                        int IPF_NumberResubmits = Convert.ToInt32(appItem[ApprovalListFields.IPF_NumberResubmits]);
                                        IPF_NumberResubmits++;
                                        appItem[ApprovalListFields.IPF_NumberResubmits] = IPF_NumberResubmits;
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if ((appItem[ApprovalListFields.IPF_SubmittedDate] == null))
                                    {
                                        appItem[ApprovalListFields.IPF_SubmittedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.IPF_SubmittedBy] = approvalItem.ModifiedBy;
                                    }
                                }
                                else
                                {
                                    appItem[ApprovalListFields.IPF_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.IPF_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public bool IPFSubmitted(int itemId)
        {
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            DateTime DATETIME_MIN = new DateTime(1900, 1, 1);
                            DateTime date = Convert.ToDateTime(item[ApprovalListFields.IPF_SubmittedDate]);
                            if (date == Convert.ToDateTime(null))
                                return false;
                            else if (date.Equals(DATETIME_MIN))
                                return false;
                            else if (date.Equals("1/1/0001"))
                                return false;
                            else
                                return true;
                        }
                    }
                }
            }
            return false;
        }
        #endregion
        public int InsertProjectDecisionItem(int iItemId, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                        if (iItemId > 0)
                        {
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + iItemId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                            SPListItem appItem;
                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                appItem = compassItemCol[0];
                            }
                            else
                            {
                                appItem = spList.AddItem();
                                appItem["Title"] = title;
                                appItem[ApprovalListFields.CompassListItemId] = iItemId;
                            }
                            appItem.Update();
                            id = appItem.ID;
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return id;
        }
        public int InsertEmailLoggingItem(int iItemId, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);

                        if (iItemId > 0)
                        {
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + iItemId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                            SPListItem appItem;
                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                appItem = compassItemCol[0];
                            }
                            else
                            {
                                appItem = spList.AddItem();
                                appItem["Title"] = title;
                                appItem[EmailLoggingListFields.CompassListItemId] = iItemId;
                            }
                            appItem.Update();
                            id = appItem.ID;
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return id;
        }
        public int InsertWorkflowStatusItem(int iItemId, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorkflowStatusListName);

                        if (iItemId > 0)
                        {
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + iItemId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;
                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                            SPListItem appItem;
                            if (compassItemCol != null && compassItemCol.Count > 0)
                            {
                                appItem = compassItemCol[0];
                            }
                            else
                            {
                                appItem = spList.AddItem();
                                appItem["Title"] = title;
                                appItem[CompassProjectDecisionsListFields.CompassListItemId] = iItemId;
                            }
                            appItem.Update();
                            id = appItem.ID;
                        }

                        spWeb.AllowUnsafeUpdates = false;

                    }
                }
            });
            return id;
        }
        public List<PackagingItem> GetFinishedPackingComponentsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq>" +
                        "<Eq><FieldRef Name=\"ParentID\" /><Value Type=\"Int\">0</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                            packagingItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                            packagingItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);

                            packagingItem.Length = Convert.ToString(item[PackagingItemListFields.Length]);
                            packagingItem.Width = Convert.ToString(item[PackagingItemListFields.Width]);
                            packagingItem.Height = Convert.ToString(item[PackagingItemListFields.Height]);
                            packagingItem.CADDrawing = Convert.ToString(item[PackagingItemListFields.CADDrawing]);
                            packagingItem.Structure = Convert.ToString(item[PackagingItemListFields.Structure]);
                            packagingItem.StructureColor = Convert.ToString(item[PackagingItemListFields.StructureColor]);
                            packagingItem.BackSeam = Convert.ToString(item[PackagingItemListFields.BackSeam]);
                            packagingItem.WebWidth = Convert.ToString(item[PackagingItemListFields.WebWidth]);
                            packagingItem.ExactCutOff = Convert.ToString(item[PackagingItemListFields.ExactCutOff]);
                            packagingItem.BagFace = Convert.ToString(item[PackagingItemListFields.BagFace]);
                            packagingItem.Unwind = Convert.ToString(item[PackagingItemListFields.Unwind]);
                            packagingItem.Description = Convert.ToString(item[PackagingItemListFields.Description]);

                            packagingItem.FilmMaxRollOD = Convert.ToString(item[PackagingItemListFields.FilmMaxRollOD]);
                            packagingItem.FilmRollID = Convert.ToString(item[PackagingItemListFields.FilmRollID]);
                            packagingItem.FilmPrintStyle = Convert.ToString(item[PackagingItemListFields.FilmPrintStyle]);
                            packagingItem.FilmStyle = Convert.ToString(item[PackagingItemListFields.FilmStyle]);
                            packagingItem.CorrugatedPrintStyle = Convert.ToString(item[PackagingItemListFields.CorrugatedPrintStyle]);
                            packagingItem.GraphicsChangeRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                            packagingItem.ExternalGraphicsVendor = Convert.ToString(item[PackagingItemListFields.ExternalGraphicsVendor]);
                            packagingItem.GraphicsBrief = Convert.ToString(item[PackagingItemListFields.GraphicsBrief]);
                            packagingItem.BOMEffectiveDate = Convert.ToDateTime(item[PackagingItemListFields.BOMEffectiveDate]);
                            packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                            packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                            packagingItem.MRPC = Convert.ToString(item[PackagingItemListFields.MRPC]);

                            packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            packagingItem.PECompleted = Convert.ToString(item[PackagingItemListFields.PECompleted]);
                            packagingItem.PE2Completed = Convert.ToString(item[PackagingItemListFields.PE2Completed]);
                            packagingItem.ProcCompleted = Convert.ToString(item[PackagingItemListFields.ProcCompleted]);

                            packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                            packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                            packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetAllPackagingComponentsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq>" +
                        "<Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {

                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                            packagingItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                            packagingItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.GraphicsChangeRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                            packagingItem.ExternalGraphicsVendor = Convert.ToString(item[PackagingItemListFields.ExternalGraphicsVendor]);
                            packagingItem.GraphicsBrief = Convert.ToString(item[PackagingItemListFields.GraphicsBrief]);
                            packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                            packagingItem.ConfirmedNLEA = Convert.ToString(item[PackagingItemListFields.ConfirmedNLEA]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);

                            packagingItem.PHL1 = Convert.ToString(item[PackagingItemListFields.PHL1]);
                            packagingItem.PHL2 = Convert.ToString(item[PackagingItemListFields.PHL2]);
                            packagingItem.Brand = Convert.ToString(item[PackagingItemListFields.Brand]);
                            packagingItem.ProfitCenter = Convert.ToString(item[PackagingItemListFields.ProfitCenter]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public void InsertMarketingClaimsItem(MarketingClaimsItem ipItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);

                        var item = spList.AddItem();

                        item["Title"] = ipItem.Title;
                        item[MarketingClaimsListFields.CompassListItemId] = ipItem.CompassListItemId;
                        item[MarketingClaimsListFields.SellableUnit] = ipItem.SellableUnit;
                        item[MarketingClaimsListFields.NewNLEAFormat] = ipItem.NewNLEAFormat;
                        item[MarketingClaimsListFields.MadeInUSAClaim] = ipItem.MadeInUSAClaim;
                        item[MarketingClaimsListFields.MadeInUSAClaimDets] = ipItem.MadeInUSAClaimDets;
                        item[MarketingClaimsListFields.Organic] = ipItem.Organic;
                        item[MarketingClaimsListFields.GMOClaim] = ipItem.GMOClaim;
                        item[MarketingClaimsListFields.GlutenFree] = ipItem.GlutenFree;
                        item[MarketingClaimsListFields.FatFree] = ipItem.FatFree;
                        item[MarketingClaimsListFields.Kosher] = ipItem.Kosher;
                        item[MarketingClaimsListFields.NaturalColors] = ipItem.NaturalColors;
                        item[MarketingClaimsListFields.NaturalFlavors] = ipItem.NaturalFlavors;
                        item[MarketingClaimsListFields.PreservativeFree] = ipItem.PreservativeFree;
                        item[MarketingClaimsListFields.LactoseFree] = ipItem.LactoseFree;
                        item[MarketingClaimsListFields.JuiceConcentrate] = ipItem.JuiceConcentrate;
                        item[MarketingClaimsListFields.LowSodium] = ipItem.LowSodium;
                        item[MarketingClaimsListFields.GoodSource] = ipItem.GoodSource;
                        item[MarketingClaimsListFields.VitaminAPct] = ipItem.VitaminAPct;
                        item[MarketingClaimsListFields.VitaminB1Pct] = ipItem.VitaminB1Pct;
                        item[MarketingClaimsListFields.VitaminB2Pct] = ipItem.VitaminB2Pct;
                        item[MarketingClaimsListFields.VitaminB3Pct] = ipItem.VitaminB3Pct;
                        item[MarketingClaimsListFields.VitaminB5Pct] = ipItem.VitaminB5Pct;
                        item[MarketingClaimsListFields.VitaminB6Pct] = ipItem.VitaminB6Pct;
                        item[MarketingClaimsListFields.VitaminB12Pct] = ipItem.VitaminB12Pct;
                        item[MarketingClaimsListFields.VitaminCPct] = ipItem.VitaminCPct;
                        item[MarketingClaimsListFields.VitaminDPct] = ipItem.VitaminDPct;
                        item[MarketingClaimsListFields.VitaminEPct] = ipItem.VitaminEPct;
                        item[MarketingClaimsListFields.PotassiumPct] = ipItem.PotassiumPct;
                        item[MarketingClaimsListFields.IronPct] = ipItem.IronPct;
                        item[MarketingClaimsListFields.CalciumPct] = ipItem.CalciumPct;
                        item[MarketingClaimsListFields.ClaimsDesired] = ipItem.ClaimsDesired;
                        item[MarketingClaimsListFields.MaterialClaimsCompNumber] = ipItem.MaterialClaimsCompNumber;
                        item[MarketingClaimsListFields.BioEngLabelingAcceptable] = ipItem.BioEngLabelingAcceptable;


                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;
                        // Set Created By to current user NOT System Account
                        item["Author"] = SPContext.Current.Web.CurrentUser;

                        item.Update();

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateMarketingClaimsItem(MarketingClaimsItem ipItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + ipItem.CompassListItemId + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item = null;
                        if (compassItemCol.Count > 0)
                        {
                            item = compassItemCol[0];
                        }
                        else
                        {
                            item = spList.AddItem();
                        }
                        if (item != null)
                        {
                            item["Title"] = ipItem.Title;
                            item[MarketingClaimsListFields.CompassListItemId] = ipItem.CompassListItemId;
                            item[MarketingClaimsListFields.SellableUnit] = ipItem.SellableUnit;
                            item[MarketingClaimsListFields.NewNLEAFormat] = ipItem.NewNLEAFormat;
                            item[MarketingClaimsListFields.MadeInUSAClaim] = ipItem.MadeInUSAClaim;
                            item[MarketingClaimsListFields.MadeInUSAClaimDets] = ipItem.MadeInUSAClaimDets;
                            item[MarketingClaimsListFields.Organic] = ipItem.Organic;
                            item[MarketingClaimsListFields.GMOClaim] = ipItem.GMOClaim;
                            item[MarketingClaimsListFields.GlutenFree] = ipItem.GlutenFree;
                            item[MarketingClaimsListFields.FatFree] = ipItem.FatFree;
                            item[MarketingClaimsListFields.Kosher] = ipItem.Kosher;
                            item[MarketingClaimsListFields.NaturalColors] = ipItem.NaturalColors;
                            item[MarketingClaimsListFields.NaturalFlavors] = ipItem.NaturalFlavors;
                            item[MarketingClaimsListFields.PreservativeFree] = ipItem.PreservativeFree;
                            item[MarketingClaimsListFields.LactoseFree] = ipItem.LactoseFree;
                            item[MarketingClaimsListFields.JuiceConcentrate] = ipItem.JuiceConcentrate;
                            item[MarketingClaimsListFields.GoodSource] = ipItem.GoodSource;
                            item[MarketingClaimsListFields.LowSodium] = ipItem.LowSodium;
                            item[MarketingClaimsListFields.VitaminAPct] = ipItem.VitaminAPct;
                            item[MarketingClaimsListFields.VitaminB1Pct] = ipItem.VitaminB1Pct;
                            item[MarketingClaimsListFields.VitaminB2Pct] = ipItem.VitaminB2Pct;
                            item[MarketingClaimsListFields.VitaminB3Pct] = ipItem.VitaminB3Pct;
                            item[MarketingClaimsListFields.VitaminB5Pct] = ipItem.VitaminB5Pct;
                            item[MarketingClaimsListFields.VitaminB6Pct] = ipItem.VitaminB6Pct;
                            item[MarketingClaimsListFields.VitaminB12Pct] = ipItem.VitaminB12Pct;
                            item[MarketingClaimsListFields.VitaminCPct] = ipItem.VitaminCPct;
                            item[MarketingClaimsListFields.VitaminDPct] = ipItem.VitaminDPct;
                            item[MarketingClaimsListFields.VitaminEPct] = ipItem.VitaminEPct;
                            item[MarketingClaimsListFields.PotassiumPct] = ipItem.PotassiumPct;
                            item[MarketingClaimsListFields.IronPct] = ipItem.IronPct;
                            item[MarketingClaimsListFields.CalciumPct] = ipItem.CalciumPct;

                            item[MarketingClaimsListFields.ClaimsDesired] = ipItem.ClaimsDesired;
                            item[MarketingClaimsListFields.MaterialClaimsCompNumber] = ipItem.MaterialClaimsCompNumber;
                            item[MarketingClaimsListFields.MaterialClaimsCompDesc] = ipItem.MaterialClaimsCompDesc;
                            item[MarketingClaimsListFields.BioEngLabelingAcceptable] = ipItem.BioEngLabelingAcceptable;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }

                }
            });
        }
        public void UpdateIPF(IPFUpdateItem IPFUpdateItem, bool bSubmitted)
        {
            UpdateItemProposalItem(IPFUpdateItem.ItemProposalItem, bSubmitted);
            UpdateItemProposalApprovalItem(IPFUpdateItem.ApprovalItem, bSubmitted);
            UpdateMarketingClaimsItem(IPFUpdateItem.MarketingClaimsItem);
        }

        public MarketingClaimsItem GetMarketingClaimsItem(int itemId)
        {
            var newItem = new MarketingClaimsItem();
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + itemId + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol != null)
                        {
                            if (compassItemCol.Count > 0)
                            {
                                var item = compassItemCol[0];

                                newItem.CompassListItemId = Convert.ToInt32(item[MarketingClaimsListFields.CompassListItemId]);
                                newItem.SellableUnit = Convert.ToString(item[MarketingClaimsListFields.SellableUnit]);
                                newItem.NewNLEAFormat = Convert.ToString(item[MarketingClaimsListFields.NewNLEAFormat]);
                                newItem.MadeInUSAClaim = Convert.ToString(item[MarketingClaimsListFields.MadeInUSAClaim]);
                                newItem.MadeInUSAClaimDets = Convert.ToString(item[MarketingClaimsListFields.MadeInUSAClaimDets]);
                                newItem.Organic = Convert.ToString(item[MarketingClaimsListFields.Organic]);
                                newItem.GMOClaim = Convert.ToString(item[MarketingClaimsListFields.GMOClaim]);
                                newItem.GlutenFree = Convert.ToString(item[MarketingClaimsListFields.GlutenFree]);
                                newItem.FatFree = Convert.ToString(item[MarketingClaimsListFields.FatFree]);
                                newItem.Kosher = Convert.ToString(item[MarketingClaimsListFields.Kosher]);
                                newItem.NaturalColors = Convert.ToString(item[MarketingClaimsListFields.NaturalColors]);
                                newItem.NaturalFlavors = Convert.ToString(item[MarketingClaimsListFields.NaturalFlavors]);
                                newItem.PreservativeFree = Convert.ToString(item[MarketingClaimsListFields.PreservativeFree]);
                                newItem.LactoseFree = Convert.ToString(item[MarketingClaimsListFields.LactoseFree]);
                                newItem.JuiceConcentrate = Convert.ToString(item[MarketingClaimsListFields.JuiceConcentrate]);
                                newItem.LowSodium = Convert.ToString(item[MarketingClaimsListFields.LowSodium]);
                                newItem.GoodSource = Convert.ToString(item[MarketingClaimsListFields.GoodSource]);
                                newItem.VitaminAPct = Convert.ToString(item[MarketingClaimsListFields.VitaminAPct]);
                                newItem.VitaminB1Pct = Convert.ToString(item[MarketingClaimsListFields.VitaminB1Pct]);
                                newItem.VitaminB2Pct = Convert.ToString(item[MarketingClaimsListFields.VitaminB2Pct]);
                                newItem.VitaminB3Pct = Convert.ToString(item[MarketingClaimsListFields.VitaminB3Pct]);
                                newItem.VitaminB5Pct = Convert.ToString(item[MarketingClaimsListFields.VitaminB5Pct]);
                                newItem.VitaminB6Pct = Convert.ToString(item[MarketingClaimsListFields.VitaminB6Pct]);
                                newItem.VitaminB12Pct = Convert.ToString(item[MarketingClaimsListFields.VitaminB12Pct]);
                                newItem.VitaminCPct = Convert.ToString(item[MarketingClaimsListFields.VitaminCPct]);
                                newItem.VitaminDPct = Convert.ToString(item[MarketingClaimsListFields.VitaminDPct]);
                                newItem.VitaminEPct = Convert.ToString(item[MarketingClaimsListFields.VitaminEPct]);
                                newItem.PotassiumPct = Convert.ToString(item[MarketingClaimsListFields.PotassiumPct]);
                                newItem.IronPct = Convert.ToString(item[MarketingClaimsListFields.IronPct]);
                                newItem.CalciumPct = Convert.ToString(item[MarketingClaimsListFields.CalciumPct]);
                                newItem.AllergenMilk = Convert.ToString(item[MarketingClaimsListFields.AllergenMilk]);
                                newItem.AllergenEggs = Convert.ToString(item[MarketingClaimsListFields.AllergenEggs]);
                                newItem.AllergenPeanuts = Convert.ToString(item[MarketingClaimsListFields.AllergenPeanuts]);
                                newItem.AllergenCoconut = Convert.ToString(item[MarketingClaimsListFields.AllergenCoconut]);
                                newItem.AllergenAlmonds = Convert.ToString(item[MarketingClaimsListFields.AllergenAlmonds]);
                                newItem.AllergenSoy = Convert.ToString(item[MarketingClaimsListFields.AllergenSoy]);
                                newItem.AllergenWheat = Convert.ToString(item[MarketingClaimsListFields.AllergenWheat]);
                                newItem.AllergenHazelNuts = Convert.ToString(item[MarketingClaimsListFields.AllergenHazelNuts]);
                                newItem.AllergenOther = Convert.ToString(item[MarketingClaimsListFields.AllergenOther]);
                                newItem.ClaimsDesired = Convert.ToString(item[MarketingClaimsListFields.ClaimsDesired]);
                                newItem.MaterialClaimsCompNumber = Convert.ToString(item[MarketingClaimsListFields.MaterialClaimsCompNumber]);
                                newItem.MaterialClaimsCompDesc = Convert.ToString(item[MarketingClaimsListFields.MaterialClaimsCompDesc]);
                                newItem.BioEngLabelingAcceptable = Convert.ToString(item[MarketingClaimsListFields.BioEngLabelingAcceptable]);
                                newItem.ClaimBioEngineering = Convert.ToString(item[MarketingClaimsListFields.ClaimBioEngineering]);
                            }
                        }
                    }
                }
            });
            return newItem;
        }
        public void CopyMarketingClaimsItem(int copyId, int newItemId, string newProjectNumber, string Mode)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + copyId + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPQuery spQuery2 = new SPQuery();
                            spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + newItemId + "</Value></Eq></Where>";
                            SPListItemCollection compassItemCol2 = spList.GetItems(spQuery2);
                            SPListItem newItem = null;
                            var copyItem = compassItemCol[0];
                            if ((copyItem != null))
                            {
                                if (compassItemCol2.Count <= 0)
                                {
                                    newItem = spList.AddItem();
                                }
                                else
                                {
                                    newItem = compassItemCol2[0];
                                }

                                spWeb.AllowUnsafeUpdates = true;

                                newItem[MarketingClaimsListFields.CompassListItemId] = newItemId;
                                newItem[MarketingClaimsListFields.Title] = newProjectNumber;

                                newItem[MarketingClaimsListFields.SellableUnit] = copyItem[MarketingClaimsListFields.SellableUnit];
                                newItem[MarketingClaimsListFields.NewNLEAFormat] = copyItem[MarketingClaimsListFields.NewNLEAFormat];
                                newItem[MarketingClaimsListFields.MadeInUSAClaim] = copyItem[MarketingClaimsListFields.MadeInUSAClaim];
                                newItem[MarketingClaimsListFields.MadeInUSAClaimDets] = copyItem[MarketingClaimsListFields.MadeInUSAClaimDets];
                                newItem[MarketingClaimsListFields.Organic] = copyItem[MarketingClaimsListFields.Organic];
                                newItem[MarketingClaimsListFields.GMOClaim] = copyItem[MarketingClaimsListFields.GMOClaim];
                                newItem[MarketingClaimsListFields.GlutenFree] = copyItem[MarketingClaimsListFields.GlutenFree];
                                newItem[MarketingClaimsListFields.FatFree] = copyItem[MarketingClaimsListFields.FatFree];
                                newItem[MarketingClaimsListFields.Kosher] = copyItem[MarketingClaimsListFields.Kosher];
                                newItem[MarketingClaimsListFields.NaturalColors] = copyItem[MarketingClaimsListFields.NaturalColors];
                                newItem[MarketingClaimsListFields.NaturalFlavors] = copyItem[MarketingClaimsListFields.NaturalFlavors];
                                newItem[MarketingClaimsListFields.PreservativeFree] = copyItem[MarketingClaimsListFields.PreservativeFree];
                                newItem[MarketingClaimsListFields.LactoseFree] = copyItem[MarketingClaimsListFields.LactoseFree];
                                newItem[MarketingClaimsListFields.JuiceConcentrate] = copyItem[MarketingClaimsListFields.JuiceConcentrate];
                                newItem[MarketingClaimsListFields.LowSodium] = copyItem[MarketingClaimsListFields.LowSodium];
                                newItem[MarketingClaimsListFields.GoodSource] = copyItem[MarketingClaimsListFields.GoodSource];
                                newItem[MarketingClaimsListFields.VitaminAPct] = copyItem[MarketingClaimsListFields.VitaminAPct];
                                newItem[MarketingClaimsListFields.VitaminB1Pct] = copyItem[MarketingClaimsListFields.VitaminB1Pct];
                                newItem[MarketingClaimsListFields.VitaminB2Pct] = copyItem[MarketingClaimsListFields.VitaminB2Pct];
                                newItem[MarketingClaimsListFields.VitaminB3Pct] = copyItem[MarketingClaimsListFields.VitaminB3Pct];
                                newItem[MarketingClaimsListFields.VitaminB5Pct] = copyItem[MarketingClaimsListFields.VitaminB5Pct];
                                newItem[MarketingClaimsListFields.VitaminB6Pct] = copyItem[MarketingClaimsListFields.VitaminB6Pct];
                                newItem[MarketingClaimsListFields.VitaminB12Pct] = copyItem[MarketingClaimsListFields.VitaminB12Pct];
                                newItem[MarketingClaimsListFields.VitaminCPct] = copyItem[MarketingClaimsListFields.VitaminCPct];
                                newItem[MarketingClaimsListFields.VitaminDPct] = copyItem[MarketingClaimsListFields.VitaminDPct];
                                newItem[MarketingClaimsListFields.VitaminEPct] = copyItem[MarketingClaimsListFields.VitaminEPct];
                                newItem[MarketingClaimsListFields.PotassiumPct] = copyItem[MarketingClaimsListFields.PotassiumPct];
                                newItem[MarketingClaimsListFields.IronPct] = copyItem[MarketingClaimsListFields.IronPct];
                                newItem[MarketingClaimsListFields.CalciumPct] = copyItem[MarketingClaimsListFields.CalciumPct];
                                if (Mode != "CopyExistingIPF")
                                {
                                    newItem[MarketingClaimsListFields.AllergenMilk] = copyItem[MarketingClaimsListFields.AllergenMilk];
                                    newItem[MarketingClaimsListFields.AllergenEggs] = copyItem[MarketingClaimsListFields.AllergenEggs];
                                    newItem[MarketingClaimsListFields.AllergenPeanuts] = copyItem[MarketingClaimsListFields.AllergenPeanuts];
                                    newItem[MarketingClaimsListFields.AllergenCoconut] = copyItem[MarketingClaimsListFields.AllergenCoconut];
                                    newItem[MarketingClaimsListFields.AllergenAlmonds] = copyItem[MarketingClaimsListFields.AllergenAlmonds];
                                    newItem[MarketingClaimsListFields.AllergenSoy] = copyItem[MarketingClaimsListFields.AllergenSoy];
                                    newItem[MarketingClaimsListFields.AllergenWheat] = copyItem[MarketingClaimsListFields.AllergenWheat];
                                    newItem[MarketingClaimsListFields.AllergenHazelNuts] = copyItem[MarketingClaimsListFields.AllergenHazelNuts];
                                    newItem[MarketingClaimsListFields.AllergenOther] = copyItem[MarketingClaimsListFields.AllergenOther];
                                }
                                newItem[MarketingClaimsListFields.ClaimsDesired] = copyItem[MarketingClaimsListFields.ClaimsDesired];
                                newItem[MarketingClaimsListFields.MaterialClaimsCompNumber] = copyItem[MarketingClaimsListFields.MaterialClaimsCompNumber];
                                newItem[MarketingClaimsListFields.MaterialClaimsCompDesc] = copyItem[MarketingClaimsListFields.MaterialClaimsCompDesc];
                                if (Mode == "ChangeRequest")
                                {
                                    newItem[MarketingClaimsListFields.BioEngLabelingAcceptable] = copyItem[MarketingClaimsListFields.BioEngLabelingAcceptable];
                                }
                                newItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public string GetTBDIndicator(int itemId)
        {
            string TBDIndicator = "";
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                    }
                }
            }
            return TBDIndicator;
        }
        #region Private Helper functions
        private double GetDecimal(string value)
        {
            double newValue = 0;
            value = RemoveFormatting(value);
            try
            {
                newValue = Convert.ToDouble(value);
            }
            catch
            {
                newValue = -9999;
            }

            return newValue;
        }
        private string RemoveFormatting(string value)
        {
            value = value.Replace("%", "");
            value = value.Replace("$", "");
            value = value.Replace(",", "");

            return value;
        }
        private string FormatDecimal(double value, int numPlaces)
        {
            if (value == -9999)
                return string.Empty;

            return value.ToString("N" + numPlaces.ToString());
        }
        public static string GetLookupDetailsByValueAndColumn(string receivingColumn, string listName, string passingColumn1, string value1, string passingColumn2, string value2)
        {
            string result = "";
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = site.OpenWeb())
                {
                    SPList spList = spweb.Lists.TryGetList(listName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<And>" +
                                                "<Eq><FieldRef Name=\"" + passingColumn1 + "\" /><Value Type=\"Text\">" + value1 + "</Value></Eq>" +
                                                "<Eq><FieldRef Name=\"" + passingColumn2 + "\" /><Value Type=\"Text\">" + value2 + "</Value></Eq>" +
                                            "</And>" +
                                            "<Eq><FieldRef Name=\"RoutingEnabled\" /><Value Type=\"Boolean\">1</Value></Eq>" +
                                        "</And>" +
                                    "</Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        result = Convert.ToString(item[receivingColumn]);
                    }
                }
            }
            return result;
        }
        #endregion
    }
}