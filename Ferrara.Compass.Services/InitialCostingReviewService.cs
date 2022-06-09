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
    public class InitialCostingReviewService : IInitialCostingReviewService
    {
        public InitialCostingReviewItem GetInitialCostingReviewItem(int itemId)
        {
            InitialCostingReviewItem sgItem = new InitialCostingReviewItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        try { sgItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]); }
                        catch { sgItem.ExpectedGrossMarginPercent = 0.0; }

                        try { sgItem.RevisedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.RevisedGrossMarginPercent]); }
                        catch { sgItem.RevisedGrossMarginPercent = 0.0; }
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
                            // Initial Costing Fields
                            sgItem.InitialCosting_Decision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCosting_Decision]);
                            sgItem.InitialCosting_GrossMarginAccurate = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCosting_GrossMarginAccurate]);
                            sgItem.InitialCosting_Comments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCosting_Comments]);
                            sgItem.SrOBMApproval_CostingReviewComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingReviewComments]);
                        }
                    }
                }
            }
            return sgItem;
        }
        public void UpdateInitialCostingReviewItem(InitialCostingReviewItem costingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(costingItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.RevisedGrossMarginPercent] = costingItem.RevisedGrossMarginPercent;
                            //item[CompassListFields.LastUpdatedFormName] = CompassForm.InitialCosting.ToString();

                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }

                        // Update Project Decisions List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + costingItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                // Initial Costing Fields
                                appItem[CompassProjectDecisionsListFields.InitialCosting_Decision] = costingItem.InitialCosting_Decision;
                                appItem[CompassProjectDecisionsListFields.InitialCosting_GrossMarginAccurate] = costingItem.InitialCosting_GrossMarginAccurate;
                                appItem[CompassProjectDecisionsListFields.InitialCosting_Comments] = costingItem.InitialCosting_Comments;
                                appItem.Update();
                            }
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Approval Methods
        public void UpdateInitialCostingReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.InitialCosting_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.InitialCosting_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.InitialCosting_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.InitialCosting_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.InitialCosting_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetInitialCostingStartDate(int compassListItemId, DateTime startDate)
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
                                // Initial Costing Fields
                                if (item[ApprovalListFields.InitialCosting_StartDate] == null)
                                {
                                    item[ApprovalListFields.InitialCosting_StartDate] = startDate.ToString();
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
