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
using System.Text.RegularExpressions;

namespace Ferrara.Compass.Services
{
    public class ComponentCostingQuoteService : IComponentCostingQuoteService
    {
        public ComponentCostingQuoteItem GetComponentCostingQuoteItem(int packagingId, int itemId)
        {
            var newItem = new ComponentCostingQuoteItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    SPList spCCList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ComponentCostingListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"PackagingItemId\" /><Value Type=\"Text\">" + packagingId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;
                    SPListItemCollection CCCol = spCCList.GetItems(spQuery);
                    

                    if (CCCol != null)
                    {
                        if (CCCol.Count > 0)
                        {
                            SPListItem item = CCCol[0];
                            newItem.ValidityStartDate = Convert.ToDateTime(item[ComponentCostingListFields.ValidityStartDate]);
                            newItem.ValidityEndDate = Convert.ToDateTime(item[ComponentCostingListFields.ValidityEndDate]);
                            newItem.SupplierAgreementNumber = Convert.ToString(item[ComponentCostingListFields.SupplierAgreementNumber]);
                            newItem.Subcontracted = Convert.ToString(item[ComponentCostingListFields.Subcontracted]);
                            newItem.ProcurementManager = Convert.ToString(item[ComponentCostingListFields.ProcurementManager]);
                            newItem.BracketPricing = Convert.ToString(item[ComponentCostingListFields.BracketPricing]);
                            newItem.PIRCostPerUOM = Convert.ToString(item[ComponentCostingListFields.PIRCostPerUOM]);
                            newItem.PerUnit = Convert.ToString(item[ComponentCostingListFields.PerUnit]);
                            newItem.DeliveredOrOriginCost = Convert.ToString(item[ComponentCostingListFields.DeliveredOrOriginCost]);
                            newItem.FreightAmount = Convert.ToString(item[ComponentCostingListFields.FreightAmount]);
                            newItem.TransferOfOwnership = Convert.ToString(item[ComponentCostingListFields.TransferOfOwnership]);
                            newItem.PlannedDeliveryTime = Convert.ToString(item[ComponentCostingListFields.PlannedDeliveryTime]);
                            newItem.MinimumOrderQTY = Convert.ToString(item[ComponentCostingListFields.MinimumOrderQTY]);
                            newItem.StandardQuantity = Convert.ToString(item[ComponentCostingListFields.StandardQuantity]);
                            newItem.TolOverDelivery = Convert.ToString(item[ComponentCostingListFields.TolOverDelivery]);
                            newItem.TolUnderDelivery = Convert.ToString(item[ComponentCostingListFields.TolUnderDelivery]);
                            newItem.PurchasingGroup = Convert.ToString(item[ComponentCostingListFields.PurchasingGroup]);
                            newItem.ConversionFactors = Convert.ToString(item[ComponentCostingListFields.ConversionFactors]);
                            newItem.AnnualVolumeEA = Convert.ToString(item[ComponentCostingListFields.AnnualVolumeEA]);
                            newItem.NinetyDayVolume = Convert.ToString(item[ComponentCostingListFields.NinetyDayVolume]);
                            newItem.PriceDetermination = Convert.ToString(item[ComponentCostingListFields.PriceDetermination]);
                        }

                    }
                    var spPackagingList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    var packagingItem = spPackagingList.GetItemById(packagingId);
                    if (spPackagingList != null)
                    {
                        newItem.CostingQuoteDate = Convert.ToString(packagingItem[PackagingItemListFields.CostingQuoteDate]);
                        newItem.ForecastComments = Convert.ToString(packagingItem[PackagingItemListFields.ForecastComments]);
                        newItem.VendorNumber = Convert.ToString(packagingItem[PackagingItemListFields.VendorNumber]);
                        newItem.QuantityQuote = Convert.ToString(packagingItem[PackagingItemListFields.QuantityQuote]);
                        

                        newItem.InkCoveragePercentage = Convert.ToString(packagingItem[PackagingItemListFields.InkCoveragePercentage]);
                        newItem.PrinterSupplier = Convert.ToString(packagingItem[PackagingItemListFields.PrinterSupplier]);
                        newItem.MaterialNumber = Convert.ToString(packagingItem[PackagingItemListFields.MaterialNumber]);                                              
                        string PackagingComponent = Convert.ToString(packagingItem[PackagingItemListFields.PackagingComponent]).ToLower();
                        if (PackagingComponent.Contains("film"))
                        {
                            newItem.PrintStyle = Convert.ToString(packagingItem[PackagingItemListFields.FilmPrintStyle]);
                        }
                        else if (PackagingComponent.Contains("corrugated"))
                        {
                            newItem.PrintStyle = Convert.ToString(packagingItem[PackagingItemListFields.CorrugatedPrintStyle]);
                        }

                        newItem.Style = Convert.ToString(packagingItem[PackagingItemListFields.FilmStyle]);
                        newItem.ComponentType = Convert.ToString(packagingItem[PackagingItemListFields.PackagingComponent]);
                        newItem.Structure = Convert.ToString(packagingItem[PackagingItemListFields.Structure]);
                        newItem.WebWidth = Convert.ToString(packagingItem[PackagingItemListFields.WebWidth]);
                        newItem.ExactCutOff = Convert.ToString(packagingItem[PackagingItemListFields.ExactCutOff]);
                        newItem.Unwind = Convert.ToString(packagingItem[PackagingItemListFields.Unwind]);
                        newItem.CoreSize = Convert.ToString(packagingItem[PackagingItemListFields.FilmRollID]);
                        newItem.MaxDiameter = Convert.ToString(packagingItem[PackagingItemListFields.FilmMaxRollOD]);
                        newItem.ReceivingPlant = Convert.ToString(packagingItem[PackagingItemListFields.ReceivingPlant]);
                        newItem.RequestedDueDate = Convert.ToString(packagingItem[PackagingItemListFields.RequestedDueDate]);
                        newItem.NumberColors = Convert.ToString(packagingItem[PackagingItemListFields.EstimatedNumberOfColors]);
                        newItem.First90Days = Convert.ToString(packagingItem[PackagingItemListFields.First90Days]);
                        newItem.StandardOrderingQuantity = Convert.ToString(packagingItem[PackagingItemListFields.StandardOrderingQuantity]);
                        newItem.OrderUOM = Convert.ToString(packagingItem[PackagingItemListFields.OrderUOM]);
                        newItem.Incoterms = Convert.ToString(packagingItem[PackagingItemListFields.Incoterms]);
                        newItem.XferOfOwnership = Convert.ToString(packagingItem[PackagingItemListFields.XferOfOwnership]);
                        newItem.PRDateCategory = Convert.ToString(packagingItem[PackagingItemListFields.PRDateCategory]);
                        newItem.VendorMaterialNumber = Convert.ToString(packagingItem[PackagingItemListFields.VendorMaterialNumber]);
                        newItem.CostingCondition = Convert.ToString(packagingItem[PackagingItemListFields.CostingCondition]);
                        newItem.CostingUnit = Convert.ToString(packagingItem[PackagingItemListFields.CostingUnit]);
                        newItem.EachesPerCostingUnit = Convert.ToString(packagingItem[PackagingItemListFields.EachesPerCostingUnit]);
                        newItem.LBPerCostingUnit = Convert.ToString(packagingItem[PackagingItemListFields.LBPerCostingUnit]);
                        newItem.CostingUnitPerPallet = Convert.ToString(packagingItem[PackagingItemListFields.CostingUnitPerPallet]);
                        newItem.StandardCost = Convert.ToString(packagingItem[PackagingItemListFields.StandardCost]);
                        newItem.PackQty = Convert.ToString(packagingItem[PackagingItemListFields.PackQuantity]);
                        newItem.MaterialDescription = Convert.ToString(packagingItem[PackagingItemListFields.MaterialDescription]);
                        string receivingPlant = Convert.ToString(packagingItem[PackagingItemListFields.ReceivingPlant]);
                        string packPlant = Convert.ToString(packagingItem[PackagingItemListFields.PackLocation]);
                        string trimmedPlant = Regex.Match(packPlant, @"\(([^)]*)\)").Groups[1].Value;
                        if (trimmedPlant == "FM01")
                        {
                            receivingPlant = "FCC - VERNELL (FM01)";
                        }
                        else if (trimmedPlant == "FM02")
                        {
                            receivingPlant = "FCC - REYNOSA (FM02)";
                        }
                        else if (trimmedPlant == "FP01" || trimmedPlant == "FP05" || trimmedPlant == "FP03" || trimmedPlant == "FP14")
                        {
                            receivingPlant = "FCC - MAYWOOD, IL (FP12)";
                        }
                        newItem.ReceivingPlant = receivingPlant;

                    }
                    ///Items from Compass list
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var compassItem = spList.GetItemById(itemId);
                    if (compassItem != null)
                    {
                        // Proposed Project Fields
                        newItem.SKU = Convert.ToString(compassItem[CompassListFields.SAPItemNumber]);
                        newItem.ProductHierarchyLevel1 = Convert.ToString(compassItem[CompassListFields.ProductHierarchyLevel1]);
                        newItem.Month1ProjectedDollar = Convert.ToDecimal(compassItem[CompassListFields.Month1ProjectedDollars]);
                        newItem.Month2ProjectedDollar = Convert.ToDecimal(compassItem[CompassListFields.Month2ProjectedDollars]);
                        newItem.Month3ProjectedDollar = Convert.ToDecimal(compassItem[CompassListFields.Month3ProjectedDollars]);
                        newItem.Month1ProjectedUnits = Convert.ToInt32(compassItem[CompassListFields.Month1ProjectedUnits]);
                        newItem.Month2ProjectedUnits = Convert.ToInt32(compassItem[CompassListFields.Month2ProjectedUnits]);
                        newItem.Month3ProjectedUnits = Convert.ToInt32(compassItem[CompassListFields.Month3ProjectedUnits]);
                        newItem.AnnualVolumeCaseUOM = Convert.ToString(compassItem[CompassListFields.AnnualProjectedUnits]);
                        newItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(compassItem[CompassListFields.RetailSellingUnitsBaseUOM]);
                        newItem.BaseUOM = Convert.ToString(compassItem[CompassListFields.SAPBaseUOM]);

                    }
                }
            }
            return newItem;
        }
        public List<AlternateUOMItem> GetAlternateUOMConversions(string packagingId)
        {
            List<AlternateUOMItem> UOMList = new List<AlternateUOMItem>();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_AlternateUOMListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"PackagingItemId\" /><Value Type=\"Text\">" + packagingId + "</Value></Eq></Where>";
                    SPListItemCollection Col = spList.GetItems(spQuery);

                    if (Col != null)
                    {
                        foreach(SPListItem item in Col)
                        {
                            AlternateUOMItem newItem = new AlternateUOMItem();
                            newItem.XValue = Convert.ToString(item[AlternateUOMFields.XValue]);
                            newItem.AlternateUOM = Convert.ToString(item[AlternateUOMFields.AlternateUOM]);
                            newItem.YValue = Convert.ToString(item[AlternateUOMFields.YValue]);
                            newItem.Id = item.ID;
                            UOMList.Add(newItem);
                        }
                    }
                }
            }
            return UOMList;
        }
        public void UpdatePackagingItem(ComponentCostingQuoteItem packagingItem, string webUrl, string componentType)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.CostingQuoteDate] = packagingItem.CostingQuoteDate;
                            item[PackagingItemListFields.ForecastComments] = packagingItem.ForecastComments;
                            item[PackagingItemListFields.PrinterSupplier] = packagingItem.PrinterSupplier;
                            item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;

                            if (componentType != "" && componentType.ToLower().Contains("film"))
                            {
                                item[PackagingItemListFields.FilmPrintStyle] = packagingItem.PrintStyle;
                            }
                            if (componentType != "" && componentType.ToLower().Contains("corrugated"))
                            {
                                item[PackagingItemListFields.CorrugatedPrintStyle] = packagingItem.PrintStyle;
                            }

                            item[PackagingItemListFields.FilmStyle] = packagingItem.Style;
                            item[PackagingItemListFields.Structure] = packagingItem.Structure;
                            item[PackagingItemListFields.WebWidth] = packagingItem.WebWidth;
                            item[PackagingItemListFields.ExactCutOff] = packagingItem.ExactCutOff;
                            item[PackagingItemListFields.Unwind] = packagingItem.Unwind;
                            item[PackagingItemListFields.FilmRollID] = packagingItem.CoreSize;
                            item[PackagingItemListFields.FilmMaxRollOD] = packagingItem.MaxDiameter;
                            item[PackagingItemListFields.ReceivingPlant] = packagingItem.ReceivingPlant;

                            item[PackagingItemListFields.RequestedDueDate] = packagingItem.RequestedDueDate;
                            item[PackagingItemListFields.EstimatedNumberOfColors] = packagingItem.NumberColors;
                            item[PackagingItemListFields.StandardOrderingQuantity] = packagingItem.StandardOrderingQuantity;
                            item[PackagingItemListFields.InkCoveragePercentage] = packagingItem.InkCoveragePercentage;
                            item[PackagingItemListFields.PrinterSupplier] = packagingItem.PrinterSupplier;
                            item[PackagingItemListFields.VendorNumber] = packagingItem.VendorNumber;
                            item[PackagingItemListFields.QuantityQuote] = packagingItem.QuantityQuote;

                            item[PackagingItemListFields.OrderUOM] = packagingItem.OrderUOM;
                            item[PackagingItemListFields.Incoterms] = packagingItem.Incoterms;
                            item[PackagingItemListFields.XferOfOwnership] = packagingItem.XferOfOwnership;
                            item[PackagingItemListFields.PRDateCategory] = packagingItem.PRDateCategory;
                            item[PackagingItemListFields.VendorMaterialNumber] = packagingItem.VendorMaterialNumber;
                            item[PackagingItemListFields.CostingCondition] = packagingItem.CostingCondition;
                            item[PackagingItemListFields.CostingUnit] = packagingItem.CostingUnit;
                            item[PackagingItemListFields.EachesPerCostingUnit] = packagingItem.EachesPerCostingUnit;

                            item[PackagingItemListFields.LBPerCostingUnit] = packagingItem.LBPerCostingUnit;
                            item[PackagingItemListFields.CostingUnitPerPallet] = packagingItem.CostingUnitPerPallet;
                            item[PackagingItemListFields.StandardCost] = packagingItem.StandardCost;
                            item[PackagingItemListFields.First90Days] = packagingItem.First90Days;
                            if (packagingItem.CompCostSubmittedDate != DateTime.MinValue)
                            {
                                item[PackagingItemListFields.CompCostSubmittedDate] = packagingItem.CompCostSubmittedDate;
                            }
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                        }
                        item.Update();

                        SPList spCCList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ComponentCostingListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"PackagingItemId\" /><Value Type=\"Int\">" + packagingItem.PackagingItemId + "</Value></Eq><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + packagingItem.CompassListItemId + "</Value></Eq></And></Where>";
                        spQuery.RowLimit = 1;
                        SPListItemCollection CCCol = spCCList.GetItems(spQuery);
                        
                        SPListItem CCitem;
                        if (CCCol != null)
                        {
                            if (CCCol.Count > 0)
                            {
                                CCitem = CCCol[0];
                            }
                            else
                            {
                                CCitem = spCCList.AddItem();
                            }
                        }else{
                            CCitem = spCCList.AddItem();
                        }
                        CCitem[ComponentCostingListFields.CompassListItemId] = packagingItem.CompassListItemId;
                        CCitem[ComponentCostingListFields.PackagingItemId] = packagingItem.PackagingItemId;
                        CCitem[ComponentCostingListFields.ValidityStartDate] = packagingItem.ValidityStartDate;
                        CCitem[ComponentCostingListFields.ValidityEndDate] = packagingItem.ValidityEndDate;
                        CCitem[ComponentCostingListFields.SupplierAgreementNumber] = packagingItem.SupplierAgreementNumber;
                        CCitem[ComponentCostingListFields.Subcontracted] = packagingItem.Subcontracted;
                        CCitem[ComponentCostingListFields.ProcurementManager] = packagingItem.ProcurementManager;
                        CCitem[ComponentCostingListFields.BracketPricing] = packagingItem.BracketPricing;
                        CCitem[ComponentCostingListFields.PIRCostPerUOM] = packagingItem.PIRCostPerUOM;
                        CCitem[ComponentCostingListFields.PerUnit] = packagingItem.PerUnit;
                        CCitem[ComponentCostingListFields.DeliveredOrOriginCost] = packagingItem.DeliveredOrOriginCost;
                        CCitem[ComponentCostingListFields.FreightAmount] = packagingItem.FreightAmount;
                        CCitem[ComponentCostingListFields.TransferOfOwnership] = packagingItem.TransferOfOwnership;
                        CCitem[ComponentCostingListFields.PlannedDeliveryTime] = packagingItem.PlannedDeliveryTime;
                        CCitem[ComponentCostingListFields.MinimumOrderQTY] = packagingItem.MinimumOrderQTY;
                        CCitem[ComponentCostingListFields.StandardQuantity] = packagingItem.StandardQuantity;
                        CCitem[ComponentCostingListFields.TolOverDelivery] = packagingItem.TolOverDelivery;
                        CCitem[ComponentCostingListFields.TolUnderDelivery] = packagingItem.TolUnderDelivery;
                        CCitem[ComponentCostingListFields.PurchasingGroup] = packagingItem.PurchasingGroup;
                        CCitem[ComponentCostingListFields.ConversionFactors] = packagingItem.ConversionFactors;
                        CCitem[ComponentCostingListFields.AnnualVolumeEA] = packagingItem.AnnualVolumeEA;
                        CCitem[ComponentCostingListFields.NinetyDayVolume] = packagingItem.NinetyDayVolume;
                        CCitem[ComponentCostingListFields.AnnualVolumeCaseUOM] = packagingItem.AnnualVolumeCaseUOM;
                        CCitem[ComponentCostingListFields.PriceDetermination] = packagingItem.PriceDetermination;

                        CCitem.Update();

                        SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem Compassitem = spCompassList.GetItemById(packagingItem.CompassListItemId);
                        if (Compassitem != null)
                        {
                            Compassitem[CompassListFields.LastUpdatedFormName] = packagingItem.LastUpdatedFormName;
                        }
                        Compassitem.Update();

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateAlternateUOMs(AlternateUOMItem alternateItem, string webUrl)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_AlternateUOMListName);

                        SPListItem item;
                        if (alternateItem.Id > 0)
                        {
                            item = spList.GetItemById(alternateItem.Id);     
                        }
                        else
                        {
                            item = spList.AddItem();
                        }
                        if (item != null)
                        {
                            item[AlternateUOMFields.XValue] = alternateItem.XValue;
                            item[AlternateUOMFields.AlternateUOM] = alternateItem.AlternateUOM;
                            item[AlternateUOMFields.YValue] = alternateItem.YValue;
                            item[AlternateUOMFields.PackagingItemId] = alternateItem.PackagingItemId;
                        }
                        item.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Component Costing Approvals
        public ApprovalItem GetComponentCostingApprovalItem(int itemId)
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
                            /*appItem.StartDate = Convert.ToString(item[ApprovalListFields.CostingQuote_StartDate]);CostingQuoteUpdateFields
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.CostingQuote_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.CostingQuote_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.CostingQuote_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.CostingQuote_SubmittedBy]);*/
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdateComponentCostingApprovalItem(ApprovalItem approvalItem, bool bSubmitted, string productHierarchy)
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
                        bool update = false;
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                if(productHierarchy == "seasonal" && (appItem[ApprovalListFields.CompCostSeasonal_SubmittedDate] == null))
                                {
                                    update = true;
                                }
                                else if(productHierarchy == "film" && (appItem[ApprovalListFields.CompCostFLRP_SubmittedDate] == null))
                                {
                                    update = true;
                                }
                                else if(productHierarchy == "corrugated" && (appItem[ApprovalListFields.CompCostCorrPaper_SubmittedDate] == null))
                                {
                                    update = true;
                                }
                                if ((bSubmitted) && (update))
                                {
                                    if (productHierarchy == "seasonal")
                                    {
                                        appItem[ApprovalListFields.CompCostSeasonal_SubmittedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.CompCostSeasonal_SubmittedBy] = approvalItem.ModifiedBy;
                                    }
                                    else if (productHierarchy == "film")
                                    {
                                        appItem[ApprovalListFields.CompCostFLRP_SubmittedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.CompCostFLRP_SubmittedBy] = approvalItem.ModifiedBy;
                                    }
                                    else if (productHierarchy == "corrugated")
                                    {
                                        appItem[ApprovalListFields.CompCostCorrPaper_SubmittedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.CompCostCorrPaper_SubmittedBy] = approvalItem.ModifiedBy;

                                    }
                                }
                                else
                                {
                                    if (productHierarchy == "seasonal")
                                    {
                                        appItem[ApprovalListFields.CompCostSeasonal_ModifiedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.CompCostSeasonal_ModifiedBy] = approvalItem.ModifiedBy;
                                    }
                                    else if (productHierarchy == "film")
                                    {
                                        appItem[ApprovalListFields.CompCostFLRP_ModifiedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.CompCostFLRP_ModifiedBy] = approvalItem.ModifiedBy;
                                    }
                                    else if (productHierarchy == "corrugated")
                                    {
                                        appItem[ApprovalListFields.CompCostCorrPaper_ModifiedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.CompCostCorrPaper_ModifiedBy] = approvalItem.ModifiedBy;

                                    }
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetComponentCostingStartDate(int compassListItemId, DateTime startDate)
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
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];
                            if (item != null)
                            {
                                // OBM First Review Fields  CostingQuoteUpdateFields
                                /*if (item[ApprovalListFields.CostingQuote_StartDate] == null)
                                {
                                    item[ApprovalListFields.CostingQuote_StartDate] = startDate.ToString();
                                    item.Update();
                                }*/
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
        public void DeleteAlternateUOMItem(int deletedId)
        {
            SPList spList;
            SPListItem itemD;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_AlternateUOMListName);
                        itemD = spList.GetItemById(deletedId);
                        itemD.Delete();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public List<KeyValuePair<string, Boolean>> allCostingQuotesSubmitted(int itemId, string compType)
        {
            List<KeyValuePair<string, Boolean>> compeletedPackItems = new List<KeyValuePair<string, Boolean>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq></And></Where>";
                    bool seasonal = string.Equals(compType, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal);
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    int listCount = compassItemCol.Count;
                    int listCompletedCount = 0;
                    Boolean listCompleted = false;
                    string submittedDate = "";
                    string packagingComponent = "";
                    int corrugatedCount = 0;
                    int corrugatedCompletedCount = 0;
                    Boolean corrugatedCompleted = false;
                    int seasonalCount = 0;
                    int seasonalCompletedCount = 0;
                    Boolean seasonalCompleted = false;
                    int filmCount = 0;
                    int filmCompletedCount = 0;
                    Boolean filmCompleted = false;
                    

                    if (compassItemCol != null && listCount > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            packagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            submittedDate = Convert.ToString(item[PackagingItemListFields.CompCostSubmittedDate]);
                            if (submittedDate != "" && submittedDate != null)
                            {
                                listCompletedCount++;
                            }
                            if (((packagingComponent.Contains("Corrugated")) || (packagingComponent.Contains("Paperboard")) && !seasonal))
                            {
                                corrugatedCount++;
                                if (submittedDate != "" && submittedDate != null)
                                {
                                    corrugatedCompletedCount++;
                                }
                            }
                            else if (((packagingComponent.Contains("Film")) || (packagingComponent.Contains("Label")) || (packagingComponent.Contains("Rigid"))) && !seasonal)
                            {
                                filmCount++;
                                if (submittedDate != "" && submittedDate != null)
                                {
                                    filmCompletedCount++;
                                }

                            }
                            else if (packagingComponent.Contains("Other") && !seasonal)
                            {
                                corrugatedCount++;
                                filmCount++;
                                if (submittedDate != "" && submittedDate != null)
                                {
                                    corrugatedCompletedCount++;
                                    filmCompletedCount++;
                                }
                            }
                            else if(seasonal)
                            {
                                seasonalCount++;
                                if (submittedDate != "" && submittedDate != null)
                                {
                                    seasonalCompletedCount++;
                                }
                            }
                        }
                        if (seasonalCompletedCount == seasonalCount && seasonalCompletedCount > 0)
                        {
                            seasonalCompleted = true;
                        }
                        if (filmCompletedCount == filmCount && filmCompletedCount > 0)
                        {
                            filmCompleted = true;
                        }
                        if (corrugatedCompletedCount == corrugatedCount && corrugatedCompletedCount > 0)
                        {
                            corrugatedCompleted = true;
                        }
                        if (listCompletedCount == listCount)
                        {
                            listCompleted = true;
                        }
                        compeletedPackItems.Add(new KeyValuePair<string, bool>("seasonal", seasonalCompleted));
                        compeletedPackItems.Add(new KeyValuePair<string, bool>("film", filmCompleted));
                        compeletedPackItems.Add(new KeyValuePair<string, bool>("corrugated", corrugatedCompleted));
                        compeletedPackItems.Add(new KeyValuePair<string, bool>("all", listCompleted));
                    }
                }
            }
            return compeletedPackItems;
        }
    }
}
