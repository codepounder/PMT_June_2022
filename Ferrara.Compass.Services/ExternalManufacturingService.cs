using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class ExternalManufacturingService : IExternalManufacturingService
    {
        public ExternalManufacturingItem GetExternalManufacturingItem(int itemId)
        {
            ExternalManufacturingItem sgItem = new ExternalManufacturingItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;

                        // IPF Fields
                        sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);

                        try { sgItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                        catch { sgItem.AnnualProjectedUnits = -9999; }

                        try { sgItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]); }
                        catch { sgItem.RetailSellingUnitsBaseUOM = -9999; }

                        try { sgItem.RetailUnitWieghtOz = Convert.ToDouble(item[CompassListFields.RetailUnitWieghtOz]); }
                        catch { sgItem.RetailUnitWieghtOz = -9999; }

                        try { sgItem.TruckLoadPricePerSellingUnit = Convert.ToDouble(item[CompassListFields.TruckLoadPricePerSellingUnit]); }
                        catch { sgItem.TruckLoadPricePerSellingUnit = -9999; }

                        try { sgItem.Month1ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month1ProjectedUnits]); }
                        catch { sgItem.Month1ProjectedUnits = -9999; }

                        try { sgItem.Month2ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month2ProjectedUnits]); }
                        catch { sgItem.Month2ProjectedUnits = -9999; }

                        try { sgItem.Month3ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month3ProjectedUnits]); }
                        catch { sgItem.Month3ProjectedUnits = -9999; }

                        try { sgItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]); }
                        catch { sgItem.ExpectedGrossMarginPercent = -9999; }

                        if (item[CompassListFields.RevisedGrossMarginPercent] != null)
                        {
                            try { sgItem.RevisedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.RevisedGrossMarginPercent]); }
                            catch { sgItem.RevisedGrossMarginPercent = -9999; }
                        }
                        else
                        {
                            sgItem.ExpectedGrossMarginPercent = -9999;
                        }

                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        sgItem.ExternalMfgProjectLead = Convert.ToString(item[CompassListFields.ExternalMfgProjectLead]);
                        sgItem.CoManufacturingClassification = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        sgItem.DoesBulkSemiExistToBringInHouse = Convert.ToString(item[CompassListFields.DoesBulkSemiExistToBringInHouse]);
                        sgItem.ExistingBulkSemiNumber = Convert.ToString(item[CompassListFields.ExistingBulkSemiNumber]);
                        sgItem.BulkSemiDescription = Convert.ToString(item[CompassListFields.BulkSemiDescription]);
                        sgItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        sgItem.DoesSupplierHaveMakeCapacity = Convert.ToString(item[CompassListFields.DoesSupplierHaveMakeCapacity]);
                        sgItem.ManufacturerCountryOfOrigin = Convert.ToString(item[CompassListFields.ManufacturerCountryOfOrigin]);
                        sgItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        sgItem.DoesSupplierHavePackCapacity = Convert.ToString(item[CompassListFields.DoesSupplierHavePackCapacity]);
                        sgItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);
                        sgItem.CurrentTimelineAcceptable = Convert.ToString(item[CompassListFields.CurrentTimelineAcceptable]);
                        sgItem.LeadTimeFromSupplier = Convert.ToString(item[CompassListFields.LeadTimeFromSupplier]);
                        sgItem.FinalArtworkDueToSupplier = Convert.ToDateTime(item[CompassListFields.FinalArtworkDueToSupplier]);

                        sgItem.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.MaterialNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.MaterialDescriptiom = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.NoveltyProject = Convert.ToString(item[CompassListFields.NoveltyProject]);
                        sgItem.PHL1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.PLMFlag = Convert.ToString(item[CompassListFields.PLMProject]);
                        sgItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                    }
                    #region Compass List 2
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);

                    SPQuery spCompassList2Query = new SPQuery();
                    spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spCompassList2Query.RowLimit = 1;

                    SPListItemCollection compassList2ItemCol = spList.GetItems(spCompassList2Query);
                    if (compassList2ItemCol.Count > 0)
                    {
                        SPListItem compassList2Item = compassList2ItemCol[0];

                        if (compassList2Item != null)
                        {
                            sgItem.DesignateHUBDC = Convert.ToString(compassList2Item[CompassList2Fields.DesignateHUBDC]);
                            sgItem.PackSupplierAndDielineSame = Convert.ToString(compassList2Item[CompassList2Fields.PackSupplierAndDielineSame]);
                            sgItem.WhatChangeIsRequiredExtMfg = Convert.ToString(compassList2Item[CompassList2Fields.WhatChangeIsRequiredExtMfg]);
                        }
                    }
                    #endregion
                    #region LIST_ProjectDecisionsListName
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem decisionItem = compassItemCol[0];

                        if (decisionItem != null)
                        {
                            // Initial Costing Fields
                            sgItem.InitialCosting_GrossMarginAccurate = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCosting_GrossMarginAccurate]);
                        }
                    } 
                    #endregion
                }
            }
            return sgItem;
        }
        public void updateProcPrinterSupplier(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.ReviewPrinterSupplier] = packagingItem.ReviewPrinterSupplier;

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = CompassForm.ExternalMfg.ToString();
                            item.Update();
                            
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateExternalManufacturingItem(ExternalManufacturingItem extManufacturingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        #region Compass List
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(extManufacturingItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.ExternalMfgProjectLead] = extManufacturingItem.ExternalMfgProjectLead;
                            item[CompassListFields.CoManufacturingClassification] = extManufacturingItem.CoManufacturingClassification;
                            item[CompassListFields.DoesBulkSemiExistToBringInHouse] = extManufacturingItem.DoesBulkSemiExistToBringInHouse;
                            item[CompassListFields.ExistingBulkSemiNumber] = extManufacturingItem.ExistingBulkSemiNumber;
                            item[CompassListFields.BulkSemiDescription] = extManufacturingItem.BulkSemiDescription;
                            item[CompassListFields.ExternalManufacturer] = extManufacturingItem.ExternalManufacturer;
                            item[CompassListFields.DoesSupplierHaveMakeCapacity] = extManufacturingItem.DoesSupplierHaveMakeCapacity;
                            item[CompassListFields.ManufacturerCountryOfOrigin] = extManufacturingItem.ManufacturerCountryOfOrigin;
                            item[CompassListFields.ExternalPacker] = extManufacturingItem.ExternalPacker;
                            item[CompassListFields.DoesSupplierHavePackCapacity] = extManufacturingItem.DoesSupplierHavePackCapacity;
                            item[CompassListFields.PurchasedIntoLocation] = extManufacturingItem.PurchasedIntoLocation;
                            item[CompassListFields.CurrentTimelineAcceptable] = extManufacturingItem.CurrentTimelineAcceptable;
                            item[CompassListFields.LeadTimeFromSupplier] = extManufacturingItem.LeadTimeFromSupplier;

                            if ((extManufacturingItem.FinalArtworkDueToSupplier != null) && (extManufacturingItem.FinalArtworkDueToSupplier != DateTime.MinValue))
                                item[CompassListFields.FinalArtworkDueToSupplier] = extManufacturingItem.FinalArtworkDueToSupplier;

                            item[CompassListFields.LastUpdatedFormName] = CompassForm.ExternalMfg.ToString();

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        #endregion
                        #region Compass List 2
                        var spCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spCompassList2Query = new SPQuery();
                        spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + extManufacturingItem.CompassListItemId + "</Value></Eq></Where>";
                        spCompassList2Query.RowLimit = 1;

                        SPListItemCollection CompassList2ItemCol = spCompassList2.GetItems(spCompassList2Query);
                        if (CompassList2ItemCol.Count > 0)
                        {
                            SPListItem CompassList2Item = CompassList2ItemCol[0];
                            if (CompassList2Item != null)
                            {
                                CompassList2Item[CompassList2Fields.PackSupplierAndDielineSame] = extManufacturingItem.PackSupplierAndDielineSame;
                                CompassList2Item[CompassList2Fields.WhatChangeIsRequiredExtMfg] = extManufacturingItem.WhatChangeIsRequiredExtMfg;
                                CompassList2Item["Editor"] = SPContext.Current.Web.CurrentUser;
                                CompassList2Item.Update();
                            }
                        }
                        #endregion
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #region Approval Methods
        public ApprovalItem GetExternalManufacturingApprovalItem(int itemId)
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
                            appItem.StartDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_StartDate]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.ExternalMfg_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.ExternalMfg_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.ExternalMfg_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdateExternalManufacturingApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.ExternalMfg_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.ExternalMfg_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.ExternalMfg_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.ExternalMfg_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.ExternalMfg_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetExternalManufacturingStartDate(int compassListItemId, DateTime startDate)
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
                                // External Manufacturing Fields
                                if (item[ApprovalListFields.ExternalMfg_StartDate] == null)
                                {
                                    item[ApprovalListFields.ExternalMfg_StartDate] = startDate.ToString();
                                    item.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion
    }
}
