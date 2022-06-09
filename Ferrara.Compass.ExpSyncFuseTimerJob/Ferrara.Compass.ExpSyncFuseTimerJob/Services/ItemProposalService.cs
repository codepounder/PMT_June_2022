using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.ExpSyncFuseTimerJob.Models;
using Ferrara.Compass.ExpSyncFuseTimerJob.Constants;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Services
{
    public class ItemProposalService
    {
        SPSite site;
        public ItemProposalService(SPSite site)
        {
            this.site = site;
        }
        public ItemProposalItem GetItemProposalItem(int itemId)
        {
            var newItem = new ItemProposalItem();
            using (var spWeb = site.OpenWeb())
            {
                var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                var item = spList.GetItemById(itemId);
                if (item != null)
                {
                    // Proposed Project Fields
                    newItem.ItemDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
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
                    newItem.BrandManager = Convert.ToString(item[CompassListFields.BrandManager]);
                    newItem.OBM = Convert.ToString(item[CompassListFields.OBM]);
                    newItem.ResearchDevelopmentLead = Convert.ToString(item[CompassListFields.ResearchDevelopmentLead]);

                    // SAP Item # Fields
                    newItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                    newItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                    newItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                    newItem.LikeFGItemNumber = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                    newItem.LikeFGItemDescription = Convert.ToString(item[CompassListFields.LikeFGItemDescription]);
                    newItem.NewTransferSemiRequired = Convert.ToString(item[CompassListFields.NewTransferSemiRequired]);

                    // Project Specifications Fields
                    newItem.NewFormula = Convert.ToString(item[CompassListFields.NewFormula]);
                    newItem.Organic = Convert.ToString(item[CompassListFields.Organic]);
                    newItem.DTVProject = Convert.ToString(item[CompassListFields.DTVProject]);
                    newItem.MfgLocationChange = Convert.ToString(item[CompassListFields.MfgLocationChange]);
                    newItem.ServingSizeWeightChange = Convert.ToString(item[CompassListFields.ServingSizeWeightChange]);
                    newItem.ReplacementForItemNumber = Convert.ToString(item[CompassListFields.ReplacementForItemNumber]);

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
                    newItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                    newItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                    newItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                    newItem.TotalQuantityUnitsInDisplay = Convert.ToString(item[CompassListFields.TotalQuantityUnitsInDisplay]);

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
                    newItem.Flowthrough = Convert.ToString(item[CompassListFields.Flowthrough]);

                    // Additional Item Details Fields
                    newItem.CandySemiNumber = Convert.ToString(item[CompassListFields.CandySemiNumber]);
                    newItem.CaseType = Convert.ToString(item[CompassListFields.CaseType]);
                    newItem.MarketClaimsLabelingRequirements = Convert.ToString(item[CompassListFields.MarketClaimsLabelingRequirements]);
                    newItem.FilmSubstrate = Convert.ToString(item[CompassListFields.FilmSubstrate]);
                    newItem.PegHoleNeeded = Convert.ToString(item[CompassListFields.PegHoleNeeded]);
                    newItem.ExpectedPackagingSwitchDate = Convert.ToDateTime(item[CompassListFields.ExpectedPackagingSwitchDate]);
                    newItem.ReasonForChange = Convert.ToString(item[CompassListFields.ReasonForChange]);
                    newItem.GraphicsRequired = Convert.ToString(item[CompassListFields.GraphicsRequired]);
                    newItem.NewComponentRequired = Convert.ToString(item[CompassListFields.NewComponentRequired]);
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
                }
            }
            return newItem;
        }
    }
}
