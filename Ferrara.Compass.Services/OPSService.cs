using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class OPSService : IOPSService
    {
        public OPSItem GetOPSItem(int itemId)
        {
            int RetailSellingUnitsBaseUOM = 0;
            OPSItem sgItem = new OPSItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.FGItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.FGItemDescription = Convert.ToString(item[CompassListFields.SAPDescription]);

                        // Manufacturing
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        sgItem.PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                        sgItem.SimilarUnitWeight = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                        sgItem.CommentsfromMarketing = Convert.ToString(item[CompassListFields.ItemConcept]);

                        sgItem.MfgLocationChange = Convert.ToString(item[CompassListFields.MfgLocationChange]);
                        sgItem.ImmediateSPKChange = Convert.ToString(item[CompassListFields.ImmediateSPKChange]);
                        sgItem.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        sgItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        sgItem.InternalTransferSemiNeeded = Convert.ToString(item[CompassListFields.InternalTransferSemiNeeded]);
                        sgItem.PurchasedSemiNeeded = Convert.ToString(item[CompassListFields.PurchasedCandySemiNeeded]);
                        sgItem.LikeItemDescription = Convert.ToString(item[CompassListFields.LikeFGItemDescription]);
                        sgItem.LikeItem = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                        sgItem.CountryOfOrigin = Convert.ToString(item[CompassListFields.ManufacturerCountryOfOrigin]);
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.WorkCenterAddInfo = Convert.ToString(item[CompassListFields.WorkCenterAdditionalInfo]);
                        sgItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        sgItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                        if (item[CompassListFields.RetailSellingUnitsBaseUOM] != null)
                            try { RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]); }
                            catch { }
                        sgItem.RetailSellingUnitsBaseUOM = RetailSellingUnitsBaseUOM;

                        //Intial Capacity
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.ManufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        sgItem.FirstProductionDate = Convert.ToDateTime(item[CompassListFields.FirstProductionDate]);
                        sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        sgItem.LineOfBusiness = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.CoManClassification = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        try { sgItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                        catch { sgItem.AnnualProjectedUnits = -9999; }

                        try { sgItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]); }
                        catch { sgItem.RetailSellingUnitsBaseUOM = -9999; }

                        try { sgItem.RetailUnitWieghtOz = Convert.ToDouble(item[CompassListFields.RetailUnitWieghtOz]); }
                        catch { sgItem.RetailUnitWieghtOz = -9999; }

                        try { sgItem.Month1ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month1ProjectedUnits]); }
                        catch { sgItem.Month1ProjectedUnits = -9999; }

                        try { sgItem.Month2ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month2ProjectedUnits]); }
                        catch { sgItem.Month2ProjectedUnits = -9999; }

                        try { sgItem.Month3ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month3ProjectedUnits]); }
                        catch { sgItem.Month3ProjectedUnits = -9999; }
                    }

                    //Intial Capacity
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

                            sgItem.InitialCapacity_Decision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_Decision]);
                            sgItem.InitialCapacity_CapacityRiskComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_CapacityRiskComments]);
                            sgItem.InitialCapacity_AcceptanceComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_AcceptanceComments]);
                            sgItem.InitialCapacity_MakeIssues = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_MakeIssues]);
                            sgItem.InitialCapacity_PackIssues = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_PackIssues]);
                            sgItem.SrOBMApproval_CapacityReviewComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments]);
                        }
                    }

                    GetDistributionItem2(sgItem, spWeb, itemId);
                }
            }
            return sgItem;
        }

        private OPSItem GetDistributionItem2(OPSItem sgItem, SPWeb spWeb, int itemId)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            var CompassList2Items = spList.GetItems(spQuery);

            if (CompassList2Items != null && CompassList2Items.Count > 0)
            {
                var ipItem = CompassList2Items[0];

                sgItem.WhatNetworkMoveIsRequired = Convert.ToString(ipItem[CompassList2Fields.WhatNetworkMoveIsRequired]);
                sgItem.ProjectApproved = Convert.ToString(ipItem[CompassList2Fields.ProjectApproved]);
                sgItem.ReasonForRejection = Convert.ToString(ipItem[CompassList2Fields.ReasonForRejection]);



            }
            return sgItem;
        }

        public void UpdateOPSItem(OPSItem compassListItem, int itemId)
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

                        //Update Project Decision List
                        SPList spListProjectDecision = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spListProjectDecision.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_Decision] = compassListItem.InitialCapacity_Decision;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_CapacityRiskComments] = compassListItem.InitialCapacity_CapacityRiskComments;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_AcceptanceComments] = compassListItem.InitialCapacity_AcceptanceComments;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_MakeIssues] = compassListItem.InitialCapacity_MakeIssues;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_PackIssues] = compassListItem.InitialCapacity_PackIssues;

                                appItem.Update();
                            }
                        }

                        //Update Compass Liust
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(itemId);
                        if (item != null)
                        {
                            // Manufacturing
                            item[CompassListFields.ManufacturingLocation] = compassListItem.MakeLocation;
                            item[CompassListFields.PackingLocation] = compassListItem.PackingLocation;
                            item[CompassListFields.ManufacturerCountryOfOrigin] = compassListItem.CountryOfOrigin;
                            item[CompassListFields.CoManufacturingClassification] = compassListItem.CoManClassification;
                            item[CompassListFields.InternalTransferSemiNeeded] = compassListItem.InternalTransferSemiNeeded;
                            item[CompassListFields.PurchasedCandySemiNeeded] = compassListItem.PurchasedSemiNeeded;
                            item[CompassListFields.LikeFGItemNumber] = compassListItem.LikeItem;
                            item[CompassListFields.LikeFGItemDescription] = compassListItem.LikeItemDescription;
                            item[CompassListFields.WorkCenterAdditionalInfo] = compassListItem.WorkCenterAddInfo;
                            item[CompassListFields.MfgLocationChange] = compassListItem.MfgLocationChange;
                            item[CompassListFields.ImmediateSPKChange] = compassListItem.ImmediateSPKChange;

                            //Initial Capacity
                            if ((compassListItem.FirstProductionDate != null) && (compassListItem.FirstProductionDate != DateTime.MinValue))
                                item[CompassListFields.FirstProductionDate] = compassListItem.FirstProductionDate;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item[CompassListFields.LastUpdatedFormName] = CompassForm.Operations.ToString();
                            item.Update();
                            UpdateCompassList2Item(compassListItem);
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateCompassList2Item(OPSItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        #region Update Compass List 2
                        var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + compassListItem.CompassListItemId + "</Value></Eq></Where>";
                        var CompassListItems = spList2.GetItems(spQuery);
                        SPListItem CompassListItem;
                        if (CompassListItems != null)
                        {
                            if (CompassListItems.Count > 0)
                            {
                                CompassListItem = CompassListItems[0];
                            }
                            else
                            {
                                CompassListItem = spList2.AddItem();
                                CompassListItem[CompassList2Fields.CompassListItemId] = compassListItem.CompassListItemId;
                            }

                            CompassListItem[CompassList2Fields.WhatNetworkMoveIsRequired] = compassListItem.WhatNetworkMoveIsRequired;
                            CompassListItem[CompassList2Fields.ProjectApproved] = compassListItem.ProjectApproved;
                            CompassListItem[CompassList2Fields.ReasonForRejection] = compassListItem.ReasonForRejection;

                            CompassListItem[CompassList2Fields.ModifiedBy] = SPContext.Current.Web.CurrentUser.ToString();
                            CompassListItem[CompassList2Fields.ModifiedDate] = DateTime.Now.ToString();
                            CompassListItem["Editor"] = SPContext.Current.Web.CurrentUser;
                            CompassListItem.Update();
                        }
                        #endregion
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public bool HasMakePackChanges(CompassListItem newItem, CompassListItem existingItem)
        {
            bool changes = false;

            if (!string.Equals(newItem.ManufacturingLocation, existingItem.ManufacturingLocation))
            {
                changes = true;
                if ((string.Equals(newItem.ManufacturingLocation, "Select...")) && (string.IsNullOrEmpty(existingItem.ManufacturingLocation)))
                    changes = false;
            }
            if (!string.Equals(newItem.PackingLocation, existingItem.PackingLocation))
            {
                changes = true;
                if ((string.Equals(newItem.PackingLocation, "Select...")) && (string.IsNullOrEmpty(existingItem.PackingLocation)))
                    changes = false;
            }
            if (!string.Equals(newItem.OPS_PlantLine, existingItem.OPS_PlantLine))
            {
                changes = true;
                if ((string.Equals(newItem.OPS_PlantLine, "Select...")) && (string.IsNullOrEmpty(existingItem.OPS_PlantLine)))
                    changes = false;
            }
            if (!string.Equals(newItem.OPS_ProcurementType, existingItem.OPS_ProcurementType))
            {
                changes = true;
                if ((string.Equals(newItem.OPS_ProcurementType, "Select...")) && (string.IsNullOrEmpty(existingItem.OPS_ProcurementType)))
                    changes = false;
            }
            if (!string.Equals(newItem.SecondaryManufacturingLocation, existingItem.SecondaryManufacturingLocation))
            {
                changes = true;
                if ((string.Equals(newItem.SecondaryManufacturingLocation, "Select...")) && (string.IsNullOrEmpty(existingItem.SecondaryManufacturingLocation)))
                    changes = false;
            }
            return changes;
        }
        public bool HasDistributionChanges(CompassListItem newItem, CompassListItem existingItem)
        {
            bool changes = false;

            if (!string.Equals(newItem.DistributionCenter, existingItem.DistributionCenter))
            {
                changes = true;
                if ((string.Equals(newItem.DistributionCenter, "Select...")) && (string.IsNullOrEmpty(existingItem.DistributionCenter)))
                    changes = false;
            }
            //if (!string.Equals(newItem.SecondaryDistributionCenter, existingItem.SecondaryDistributionCenter))
            //{
            //    changes = true;
            //    if ((string.Equals(newItem.SecondaryDistributionCenter, "Select...")) && (string.IsNullOrEmpty(existingItem.SecondaryDistributionCenter)))
            //        changes = false;
            //}
            if (!string.Equals(newItem.OPS_PurchasedIntoCenter, existingItem.OPS_PurchasedIntoCenter))
            {
                changes = true;
                if ((string.Equals(newItem.OPS_PurchasedIntoCenter, "Select...")) && (string.IsNullOrEmpty(existingItem.OPS_PurchasedIntoCenter)))
                    changes = false;
            }

            return changes;
        }
        public bool HasInternationalComplianceChanges(ApprovalListItem newItem, ApprovalListItem existingItem)
        {
            bool changes = false;

            //    if (!string.Equals(newItem.OPS_ICR_Decision, existingItem.OPS_ICR_Decision))
            //    {
            //        changes = true;
            //        if ((string.Equals(newItem.OPS_ICR_Decision, "Select...")) && (string.IsNullOrEmpty(existingItem.OPS_ICR_Decision)))
            //            changes = false;
            //    }
            //    if (!string.Equals(newItem.OPS_ICR_Comments, existingItem.OPS_ICR_Comments))
            //        changes = true;

            return changes;
        }

        #region OPS Approvals
        public ApprovalItem GetOPSApprovalItem(int itemId)
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
                            appItem.StartDate = Convert.ToString(item[ApprovalListFields.Operations_StartDate]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.Operations_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.Operations_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.Operations_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.Operations_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdateOPSApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.Operations_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.Operations_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.Operations_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.Operations_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.Operations_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetOPSStartDate(int compassListItemId, DateTime startDate)
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
                                // Distribution Fields
                                if (item[ApprovalListFields.Operations_StartDate] == null)
                                {
                                    item[ApprovalListFields.Operations_StartDate] = startDate.ToString();
                                    item.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public List<PackagingItem> GetTransferSemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq></And></Where>";

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
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                            packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                            packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }

        #endregion
    }
}
