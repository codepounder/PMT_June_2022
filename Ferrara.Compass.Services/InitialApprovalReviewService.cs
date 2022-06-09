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
    public class InitialApprovalReviewService : IInitialApprovalReviewService
    {
        private readonly IExceptionService exceptionService;
        public InitialApprovalReviewService(IExceptionService exceptionService)
        {
            this.exceptionService = exceptionService;
        }
        public InitialApprovalReviewService()
        {

        }
        public InitialApprovalReviewItem GetInitialApprovalReviewItem(int itemId)
        {
            InitialApprovalReviewItem sgItem = new InitialApprovalReviewItem();
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
                        sgItem.PMName = Convert.ToString(item[CompassListFields.PMName]);
                        sgItem.Channel = Convert.ToString(item[CompassListFields.Channel]);
                        sgItem.InitialTimeTable = Convert.ToString(item[CompassListFields.TimelineType]);
                        sgItem.ProjectStartDate = Convert.ToDateTime(item[CompassListFields.SubmittedDate]);

                        try { sgItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                        catch { sgItem.AnnualProjectedUnits = 0; }

                        try { sgItem.AnnualProjectedDollars = Convert.ToDouble(item[CompassListFields.AnnualProjectedDollars]); }
                        catch { sgItem.AnnualProjectedDollars = 0; }

                        try { sgItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]); }
                        catch { sgItem.ExpectedGrossMarginPercent = 0; }
                    }
                    #region Compass List 2
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);

                    SPQuery spCompassList2Query = new SPQuery();
                    spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spCompassList2Query.RowLimit = 1;

                    SPListItemCollection compassList2ItemCol = spList.GetItems(spCompassList2Query);
                    if (compassList2ItemCol.Count > 0)
                    {
                        SPListItem CompassList2Item = compassList2ItemCol[0];

                        if (CompassList2Item != null)
                        {
                            sgItem.NeedSExpeditedWorkflowWithSGS = Convert.ToString(CompassList2Item[CompassList2Fields.NeedSExpeditedWorkflowWithSGS]);
                        }
                    }
                    #endregion
                    #region Project Decisions List
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
                            sgItem.SrOBMApproval_Decision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_Decision]);
                            sgItem.SrOBMApproval_CostingDecision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingDecision]);
                            //sgItem.SrOBMApproval_CapacityDecision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityDecision]);
                            sgItem.SrOBMApproval_Comments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_Comments]);
                            sgItem.SrOBMApproval_CostingReviewComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingReviewComments]);
                            //sgItem.SrOBMApproval_CapacityReviewComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments]);
                        }
                    }
                    #endregion
                }
            }
            return sgItem;
        }
        public void UpdateInitialApprovalReviewItem(InitialApprovalReviewItem approvalItem)
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

                        #region Compass List
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(approvalItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.PM] = approvalItem.PM;
                            item[CompassListFields.PMName] = approvalItem.PMName;
                            item[CompassListFields.OBM] = approvalItem.PM;
                            item[CompassListFields.OBMName] = approvalItem.PMName;

                            item[CompassListFields.TimelineType] = approvalItem.InitialTimeTable;
                            item[CompassListFields.LastUpdatedFormName] = CompassForm.SrOBMApproval.ToString();

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        #endregion
                        #region Compass List 2
                        var spCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spCompassList2Query = new SPQuery();
                        spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spCompassList2Query.RowLimit = 1;

                        SPListItemCollection CompassList2ItemCol = spCompassList2.GetItems(spCompassList2Query);
                        if (CompassList2ItemCol.Count > 0)
                        {
                            SPListItem CompassList2Item = CompassList2ItemCol[0];
                            if (CompassList2Item != null)
                            {
                                CompassList2Item[CompassList2Fields.NeedSExpeditedWorkflowWithSGS] = approvalItem.NeedSExpeditedWorkflowWithSGS;
                                CompassList2Item["Editor"] = SPContext.Current.Web.CurrentUser;
                                CompassList2Item.Update();
                            }
                        }
                        #endregion
                        #region Project Decisions List
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[CompassProjectDecisionsListFields.SrOBMApproval_Comments] = approvalItem.SrOBMApproval_Comments;
                                appItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingReviewComments] = approvalItem.SrOBMApproval_CostingReviewComments;
                                //appItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments] = approvalItem.SrOBMApproval_CapacityReviewComments;
                                appItem[CompassProjectDecisionsListFields.SrOBMApproval_Decision] = approvalItem.SrOBMApproval_Decision;
                                appItem[CompassProjectDecisionsListFields.SrOBMApproval_CostingDecision] = approvalItem.SrOBMApproval_CostingDecision;
                                //appItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityDecision] = approvalItem.SrOBMApproval_CapacityDecision;
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }
                        #endregion

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Approval Methods
        public void UpdateInitialApprovalReviewApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if (bSubmitted && appItem[ApprovalListFields.SrOBMApproval_SubmittedDate] == null)
                                {
                                    appItem[ApprovalListFields.SrOBMApproval_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SrOBMApproval_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.SrOBMApproval_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SrOBMApproval_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });

        }
        public void UpdateInitialApprovalDays(int CompassListItemId, int IPF_NumberApproverDays)
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
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[ApprovalListFields.IPF_NumberApproverDays] = IPF_NumberApproverDays;
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void ClearIPFSubmitDateOnRequestForIPFUpdate(int CompassListItemId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        #region LIST_ApprovalListName
                        try
                        {
                            SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq></Where>";
                            spQuery.RowLimit = 1;

                            SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                            if (compassItemCol.Count > 0)
                            {
                                SPListItem appItem = compassItemCol[0];
                                if (appItem != null)
                                {
                                    appItem[ApprovalListFields.IPF_SubmittedBy] = string.Empty;
                                    appItem[ApprovalListFields.IPF_SubmittedDate] = string.Empty;
                                    appItem[ApprovalListFields.IPF_ModifiedBy] = SPContext.Current.Web.CurrentUser;
                                    appItem[ApprovalListFields.IPF_ModifiedDate] = DateTime.Now.ToString();
                                    appItem[ApprovalListFields.SrOBMApproval_StartDate] = string.Empty;
                                    appItem[ApprovalListFields.SrOBMApproval_ModifiedBy] = SPContext.Current.Web.CurrentUser;
                                    appItem[ApprovalListFields.SrOBMApproval_ModifiedDate] = DateTime.Now.ToString();
                                    appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appItem.Update();
                                }
                            }
                        }
                        catch (Exception) { }
                        #endregion
                        #region LIST_ProjectTimelineUpdateName
                        try
                        {
                            SPList spTimeLineUpdateList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);
                            SPQuery spTimeLineUpdateQuery = new SPQuery();
                            spTimeLineUpdateQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + CompassListItemId.ToString() + "</Value></Eq></Where>";
                            spTimeLineUpdateQuery.RowLimit = 1;
                            SPListItemCollection compassTimeLineUpdateItemCol = spTimeLineUpdateList.GetItems(spTimeLineUpdateQuery);
                            SPListItem appTimeLineUpdateItem;
                            if (compassTimeLineUpdateItemCol.Count > 0)
                            {
                                appTimeLineUpdateItem = compassTimeLineUpdateItemCol[0];
                                if (appTimeLineUpdateItem != null)
                                {

                                    appTimeLineUpdateItem["SrOBMApproval2"] = "";
                                    appTimeLineUpdateItem["SrOBMApproval2_Planned"] = "";
                                    appTimeLineUpdateItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                    appTimeLineUpdateItem.Update();
                                }
                            }
                        }
                        catch (Exception e) { }
                        #endregion
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetInitialApprovalReviewStartDate(int compassListItemId, DateTime startDate)
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
                                if (item[ApprovalListFields.SrOBMApproval_StartDate] == null)
                                {
                                    item[ApprovalListFields.SrOBMApproval_StartDate] = startDate.ToString();
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
        public void insertOriginalTimeline(int iItemId, string projectNumber, List<ProjectStatusReportItem> originalTimeline)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineDetailsList);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"" + ProjectStatusDatesFields.compassListItemId + "\" /><Value Type=\"Int\">" + iItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            return;
                        }
                        else
                        {
                            SPListItem appItem = spList.AddItem();
                            appItem[ProjectStatusDatesFields.compassListItemId] = iItemId;
                            appItem[ProjectStatusDatesFields.RowType] = "Original";
                            appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                            appItem["Title"] = projectNumber;

                            foreach (ProjectStatusReportItem item in originalTimeline)
                            {
                                if (string.IsNullOrEmpty(item.WorflowQuickStep) || item.WorflowQuickStep == "Float" || item.Checks == "DF" || item.WorflowQuickStep == "IPF")
                                {
                                    continue;
                                }
                                string columnStart = item.WorflowQuickStep + "_Start";
                                string columnEnd = item.WorflowQuickStep + "_End";
                                string columnDuration = item.WorflowQuickStep + "_Duration";
                                try
                                {

                                    appItem[columnStart] = item.OGStartDay.ToString();
                                    appItem[columnEnd] = item.OGEndDay.ToString();
                                    appItem[columnDuration] = item.OGDuration.ToString();
                                }
                                catch (Exception e)
                                {
                                    exceptionService.Handle(LogCategory.CriticalError, e, "InitialApprovalService", "insertOriginalTimeline", columnStart);
                                }
                            }
                            appItem.Update();

                            SPListItem appItem2 = spList.AddItem();
                            appItem2[ProjectStatusDatesFields.compassListItemId] = iItemId;
                            appItem2[ProjectStatusDatesFields.RowType] = "Actual";
                            appItem2["Editor"] = SPContext.Current.Web.CurrentUser;
                            appItem2["Title"] = projectNumber;
                            ProjectStatusReportItem srPMApprovalItem = (from og in originalTimeline where og.WorflowQuickStep == "SrOBMApproval" select og).FirstOrDefault();
                            appItem2[ProjectStatusDatesFields.SrOBMApprovalStart] = srPMApprovalItem.ActualStartDay;
                            appItem2[ProjectStatusDatesFields.SrOBMApprovalEnd] = DateTime.Now.ToString();
                            appItem2[ProjectStatusDatesFields.SrOBMApprovalDuration] = srPMApprovalItem.ActualDuration;
                            appItem2[ProjectStatusDatesFields.SrOBMApprovalStatus] = srPMApprovalItem.Status;
                            appItem2.Update();

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public ApprovalListItem GetApprovalItem(int itemId)
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
                            appItem.ApprovalListItemId = item.ID;
                            appItem.CompassListItemId = Convert.ToInt32(item[ApprovalListFields.CompassListItemId]);
                            // IPF Fields
                            appItem.IPF_StartDate = Convert.ToString(item[ApprovalListFields.IPF_StartDate]);
                            appItem.IPF_SubmittedBy = Convert.ToString(item[ApprovalListFields.IPF_SubmittedBy]);
                            appItem.IPF_SubmittedDate = Convert.ToString(item[ApprovalListFields.IPF_SubmittedDate]);
                            appItem.IPF_ModifiedBy = Convert.ToString(item[ApprovalListFields.IPF_ModifiedBy]);
                            appItem.IPF_ModifiedDate = Convert.ToString(item[ApprovalListFields.IPF_ModifiedDate]);
                            appItem.IPF_NumberResubmits = Convert.ToInt32(item[ApprovalListFields.IPF_NumberResubmits]);
                            // SrOBMApproval, SrOBMApproval2 Fields
                            appItem.IPF_NumberApproverDays = Convert.ToInt32(item[ApprovalListFields.IPF_NumberApproverDays]);
                            appItem.SrOBMApproval_StartDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_StartDate]);
                            appItem.SrOBMApproval_ModifiedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_ModifiedDate]);
                            appItem.SrOBMApproval_ModifiedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval_ModifiedBy]);
                            appItem.SrOBMApproval_SubmittedDate = Convert.ToString(item[ApprovalListFields.SrOBMApproval_SubmittedDate]);
                            appItem.SrOBMApproval_SubmittedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval_SubmittedBy]);
                            appItem.SrOBMApproval_SubmittedBy = Convert.ToString(item[ApprovalListFields.SrOBMApproval_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        #endregion
    }


}
