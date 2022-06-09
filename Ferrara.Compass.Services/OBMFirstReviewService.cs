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
    public class OBMFirstReviewService : IOBMFirstReviewService
    {
        public OBMFirstReviewItem GetPMFirstReviewItem(int itemId)
        {
            OBMFirstReviewItem reviewItem = new OBMFirstReviewItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        reviewItem.CompassListItemId = item.ID;

                        reviewItem.OBMFirstReviewCheck = Convert.ToString(item[CompassListFields.OBMFirstReviewCheck]);
                        reviewItem.SectionsOfConcern = Convert.ToString(item[CompassListFields.SectionsOfConcern]);
                        reviewItem.OBMFirstReviewComments = Convert.ToString(item[CompassListFields.OBMFirstReviewComments]);
                        reviewItem.ProjectStatus = Convert.ToString(item[CompassListFields.ProjectStatus]);
                        reviewItem.DoesFirstShipNeedRevision = Convert.ToString(item[CompassListFields.DoesFirstShipNeedRevision]);
                        reviewItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        reviewItem.RevisedFirstShipDateComments = Convert.ToString(item[CompassListFields.RevisedFirstShipDateComments]);
                        reviewItem.FirstProductionDate = Convert.ToDateTime(item[CompassListFields.FirstProductionDate]);

                        // Read-only fields
                        reviewItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        reviewItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        reviewItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        reviewItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        reviewItem.FirstShipDate = Convert.ToDateTime(item[CompassListFields.FirstShipDate]);
                        reviewItem.Customer = Convert.ToString(item[CompassListFields.Customer]);
                        reviewItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        reviewItem.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        reviewItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        reviewItem.InternalTransferSemiNeeded = Convert.ToString(item[CompassListFields.InternalTransferSemiNeeded]);
                        reviewItem.CoManufacturingClassification = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        reviewItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        reviewItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        reviewItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        reviewItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        reviewItem.CaseUCC = Convert.ToString(item[CompassListFields.CaseUCC]);
                        reviewItem.PalletUCC = Convert.ToString(item[CompassListFields.PalletUCC]);
                        reviewItem.Channel = Convert.ToString(item[CompassListFields.Channel]);
                        reviewItem.MaterialNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        reviewItem.MaterialDescriptiom = Convert.ToString(item[CompassListFields.SAPDescription]);

                        if (item[CompassListFields.AnnualProjectedDollars] != null)
                        {
                            try { reviewItem.AnnualProjectedDollars = Convert.ToDouble(item[CompassListFields.AnnualProjectedDollars]); }
                            catch { reviewItem.AnnualProjectedDollars = -9999; }
                        }
                        else
                        {
                            reviewItem.AnnualProjectedDollars = -9999;
                        }

                        if (item[CompassListFields.ExpectedGrossMarginPercent] != null)
                        {
                            try { reviewItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]); }
                            catch { reviewItem.ExpectedGrossMarginPercent = -9999; }
                        }
                        else
                        {
                            reviewItem.ExpectedGrossMarginPercent = -9999;
                        }

                        if (item[CompassListFields.AnnualProjectedUnits] != null)
                        {
                            try { reviewItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                            catch { reviewItem.AnnualProjectedUnits = -9999; }
                        }
                        else
                        {
                            reviewItem.AnnualProjectedUnits = -9999;
                        }
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
                            reviewItem.DesignateHUBDC = Convert.ToString(compassList2Item[CompassList2Fields.DesignateHUBDC]);
                        }
                    }
                    #endregion

                    #region Compass Project Decisions List
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
                            reviewItem.InitialCosting_GrossMarginAccurate = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCosting_GrossMarginAccurate]);
                        }
                    }
                    #endregion
                }
            }
            return reviewItem;
        }
        public void UpdatePMFirstReviewItem(OBMFirstReviewItem reviewItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(reviewItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.ProjectStatus] = reviewItem.ProjectStatus;
                            item[CompassListFields.OBMFirstReviewCheck] = reviewItem.OBMFirstReviewCheck;
                            item[CompassListFields.SectionsOfConcern] = reviewItem.SectionsOfConcern;
                            item[CompassListFields.OBMFirstReviewComments] = reviewItem.OBMFirstReviewComments;
                            //item[CompassListFields.ProjectStatus] = reviewItem.ProjectStatus;
                            item[CompassListFields.DoesFirstShipNeedRevision] = reviewItem.DoesFirstShipNeedRevision;
                            item[CompassListFields.RevisedFirstShipDateComments] = reviewItem.RevisedFirstShipDateComments;

                            if ((reviewItem.RevisedFirstShipDate != null) && (reviewItem.RevisedFirstShipDate != DateTime.MinValue) && reviewItem.DoesFirstShipNeedRevision.ToLower() == "yes")
                                item[CompassListFields.RevisedFirstShipDate] = reviewItem.RevisedFirstShipDate;
                            if ((reviewItem.FirstProductionDate != null) && (reviewItem.FirstProductionDate != DateTime.MinValue))
                                item[CompassListFields.FirstProductionDate] = reviewItem.FirstProductionDate;

                            item[CompassListFields.LastUpdatedFormName] = CompassForm.OBMReview1.ToString();
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region PM First Review Approvals
        public ApprovalItem GetPMFirstReviewApprovalItem(int itemId)
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
                            appItem.StartDate = Convert.ToString(item[ApprovalListFields.OBMReview1_StartDate]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.OBMReview1_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.OBMReview1_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.OBMReview1_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.OBMReview1_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdatePMFirstReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.OBMReview1_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.OBMReview1_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.OBMReview1_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.OBMReview1_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.OBMReview1_ModifiedDate] = approvalItem.ModifiedDate;
                                }
                                // Set Modified By to current user NOT System Account
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetPMFirstReviewStartDate(int compassListItemId, DateTime startDate)
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
                                // PM First Review Fields
                                if (item[ApprovalListFields.OBMReview1_StartDate] == null)
                                {
                                    item[ApprovalListFields.OBMReview1_StartDate] = startDate.ToString();
                                    item["Editor"] = SPContext.Current.Web.CurrentUser;
                                    item.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public ApprovalListItem GetCompletedApprovalInfo(int itemId)
        {
            ApprovalListItem appItem = new ApprovalListItem();
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
                            appItem.IPF_SubmittedDate = Convert.ToString(item[ApprovalListFields.IPF_SubmittedDate]);
                            appItem.IPF_SubmittedBy = Convert.ToString(item[ApprovalListFields.IPF_SubmittedBy]);
                            appItem.Operations_SubmittedDate = Convert.ToString(item[ApprovalListFields.Operations_SubmittedDate]);
                            appItem.Operations_SubmittedBy = Convert.ToString(item[ApprovalListFields.Operations_SubmittedBy]);
                            appItem.Distribution_SubmittedDate = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedDate]);
                            appItem.Distribution_SubmittedBy = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedBy]);
                            appItem.QA_SubmittedDate = Convert.ToString(item[ApprovalListFields.QA_SubmittedDate]);
                            appItem.QA_SubmittedBy = Convert.ToString(item[ApprovalListFields.QA_SubmittedBy]);
                            appItem.SAPInitialSetup_SubmittedDate = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_SubmittedDate]);
                            appItem.SAPInitialSetup_SubmittedBy = Convert.ToString(item[ApprovalListFields.SAPInitialSetup_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        #endregion
    }
}
