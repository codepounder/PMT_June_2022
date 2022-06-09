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
    public class OBMSecondReviewService : IOBMSecondReviewService
    {
        public OBMSecondReviewItem GetPMSecondReviewItem(int itemId)
        {
            OBMSecondReviewItem reviewItem = new OBMSecondReviewItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    #region Project Decisions List
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            reviewItem.CompassListItemId = item.ID;

                            reviewItem.OBMSecondReviewComments = Convert.ToString(item[CompassProjectDecisionsListFields.OBMSecondReview_Comments]);
                            reviewItem.OBMSecondReviewCheck = Convert.ToString(item[CompassProjectDecisionsListFields.OBMSecondReview_Check]);
                            reviewItem.OBMSecondReviewConcern = Convert.ToString(item[CompassProjectDecisionsListFields.OBMSecondReview_Concern]);
                            reviewItem.NewMaterialsinBOM = Convert.ToString(item[CompassProjectDecisionsListFields.NewMaterialsinBOM]);
                            reviewItem.NewCorrugatedPaperboardinBOM = Convert.ToString(item[CompassProjectDecisionsListFields.NewCorrugatedPaperboardinBOM]);
                            reviewItem.NewFilmLabelRigidPlasticinBOM = Convert.ToString(item[CompassProjectDecisionsListFields.NewFilmLabelRigidPlasticinBOM]);
                            reviewItem.SrOBMApproval2_Decision = Convert.ToString(item[CompassProjectDecisionsListFields.SrOBMApproval2_Decision]);
                        }
                    }
                    #endregion
                    #region Compass List
                    SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem compassItem = compassList.GetItemById(itemId);
                    if (compassItem != null)
                    {
                        reviewItem.FirstShipDate = Convert.ToDateTime(compassItem[CompassListFields.RevisedFirstShipDate]);
                        reviewItem.FirstProductionDate = Convert.ToDateTime(compassItem[CompassListFields.FirstProductionDate]);
                        reviewItem.SAPDescription = Convert.ToString(compassItem[CompassListFields.SAPDescription]);
                        reviewItem.SAPItemNumber = Convert.ToString(compassItem[CompassListFields.SAPItemNumber]);
                    } 
                    #endregion
                    #region Compass List 2
                    SPList spcompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);

                    SPQuery spcompassList2Query = new SPQuery();
                    spcompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spcompassList2Query.RowLimit = 1;

                    SPListItemCollection spcompassList2QueryItemCol = spcompassList2.GetItems(spcompassList2Query);
                    if (spcompassList2QueryItemCol.Count > 0)
                    {
                        SPListItem item = spcompassList2QueryItemCol[0];
                        if (item != null)
                        {
                            reviewItem.CompassListItemId = item.ID;

                            reviewItem.SGSExpeditedWorkflowApproved = Convert.ToString(item[CompassList2Fields.SGSExpeditedWorkflowApproved]);
                        }
                    } 
                    #endregion
                }
            }
            return reviewItem;
        }
        public void UpdatePMSecondReviewItem(OBMSecondReviewItem reviewItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        #region Project Decisions List
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + reviewItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];

                            if (item != null)
                            {
                                item[CompassProjectDecisionsListFields.OBMSecondReview_Comments] = reviewItem.OBMSecondReviewComments;
                                item[CompassProjectDecisionsListFields.OBMSecondReview_Check] = reviewItem.OBMSecondReviewCheck;
                                item[CompassProjectDecisionsListFields.OBMSecondReview_Concern] = reviewItem.OBMSecondReviewConcern;

                                item[CompassProjectDecisionsListFields.NewMaterialsinBOM] = reviewItem.NewMaterialsinBOM;
                                item[CompassProjectDecisionsListFields.NewCorrugatedPaperboardinBOM] = reviewItem.NewCorrugatedPaperboardinBOM;
                                item[CompassProjectDecisionsListFields.NewFilmLabelRigidPlasticinBOM] = reviewItem.NewFilmLabelRigidPlasticinBOM;

                                // Set Modified By to current user NOT System Account
                                item["Editor"] = SPContext.Current.Web.CurrentUser;

                                item.Update();
                            }
                        } 
                        #endregion
                        #region Compass List
                        SPList compassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem compassItem = compassList.GetItemById(reviewItem.CompassListItemId);
                        if (compassItem != null)
                        {
                            if ((reviewItem.FirstShipDate != null) && (reviewItem.FirstShipDate != DateTime.MinValue))
                                compassItem[CompassListFields.RevisedFirstShipDate] = reviewItem.FirstShipDate;
                            if ((reviewItem.FirstProductionDate != null) && (reviewItem.FirstProductionDate != DateTime.MinValue))
                                compassItem[CompassListFields.FirstProductionDate] = reviewItem.FirstProductionDate;

                            compassItem[CompassListFields.LastUpdatedFormName] = CompassForm.OBMReview2.ToString();

                            // Set Modified By to current user NOT System Account
                            compassItem["Editor"] = SPContext.Current.Web.CurrentUser;

                            compassItem.Update();
                        } 
                        #endregion
                        #region Compass List 2
                        SPList spCompassList2List = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);

                        SPQuery spCompassList2Query = new SPQuery();
                        spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + reviewItem.CompassListItemId + "</Value></Eq></Where>";
                        spCompassList2Query.RowLimit = 1;

                        SPListItemCollection compassList2ItemCol = spCompassList2List.GetItems(spCompassList2Query);
                        if (compassList2ItemCol.Count > 0)
                        {
                            SPListItem item = compassList2ItemCol[0];

                            if (item != null)
                            {
                                item[CompassList2Fields.SGSExpeditedWorkflowApproved] = reviewItem.SGSExpeditedWorkflowApproved;

                                // Set Modified By to current user NOT System Account
                                item["Editor"] = SPContext.Current.Web.CurrentUser;

                                item.Update();
                            }
                        } 
                        #endregion
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region PM Second Review Approvals
        public ApprovalItem GetPMSecondReviewApprovalItem(int itemId)
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
                            appItem.StartDate = Convert.ToString(item[ApprovalListFields.OBMReview2_StartDate]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.OBMReview2_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.OBMReview2_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.OBMReview2_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.OBMReview2_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdatePMSecondReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.OBMReview2_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.OBMReview2_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.OBMReview2_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.OBMReview2_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.OBMReview2_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetPMSecondReviewStartDate(int compassListItemId, DateTime startDate)
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
                                if (item[ApprovalListFields.OBMReview2_StartDate] == null)
                                {
                                    item[ApprovalListFields.OBMReview2_StartDate] = startDate.ToString();
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
