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
    public class SecondaryApprovalReviewService : ISecondaryApprovalReviewService
    {
        public SecondaryApprovalReviewItem GetSecondaryApprovalReviewItem(int itemId)
        {
            SecondaryApprovalReviewItem sgItem = new SecondaryApprovalReviewItem();
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
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.Customer = Convert.ToString(item[CompassListFields.Customer]);
                        sgItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        sgItem.PM = Convert.ToString(item[CompassListFields.PM]);
                        sgItem.Channel = Convert.ToString(item[CompassListFields.Channel]);

                        try { sgItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                        catch { sgItem.AnnualProjectedUnits = 0; }

                        try { sgItem.AnnualProjectedDollars = Convert.ToDouble(item[CompassListFields.AnnualProjectedDollars]); }
                        catch { sgItem.AnnualProjectedDollars = 0; }

                        try { sgItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]); }
                        catch { sgItem.ExpectedGrossMarginPercent = 0; }

                        sgItem.ProjectNotes = Convert.ToString(item[CompassListFields.ProjectNotes]);
                        sgItem.Channel = Convert.ToString(item[CompassListFields.Channel]);
                    }

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
                            // SrOBMApproval Approval fields
                            sgItem.SrOBMApproval_CapacityReviewComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments]);
                            sgItem.SrOBMApproval_CostingReviewComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingReviewComments]);

                            sgItem.SrOBMApproval2_Decision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval2_Decision]);
                        }
                    }
                }
            }
            return sgItem;
        }
        public void UpdateSecondaryApprovalReviewItem(SecondaryApprovalReviewItem approvalItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[CompassProjectDecisionsListFields.SrOBMApproval2_Decision] = approvalItem.SrOBMApproval2_Decision;
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Approval Methods
        public void UpdateSecondaryApprovalReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.SrOBMApproval2_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.SrOBMApproval2_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SrOBMApproval2_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.SrOBMApproval2_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SrOBMApproval2_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetSecondaryApprovalReviewStartDate(int compassListItemId, DateTime startDate)
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
                                // Initial Capacity Fields
                                if (item[ApprovalListFields.SrOBMApproval2_StartDate] == null)
                                {
                                    item[ApprovalListFields.SrOBMApproval2_StartDate] = startDate.ToString();
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
