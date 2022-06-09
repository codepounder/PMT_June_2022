using Ferrara.Compass.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class StageGateGeneralService : IStageGateGeneralService
    {
        #region Member Variables
        private readonly IExceptionService exceptionService;
        private IProjectTimelineTypeService timelineNumbers;
        private IWorkflowService workflowService;
        #endregion
        #region Constructor
        public StageGateGeneralService(IExceptionService exceptionService, IProjectTimelineTypeService timelineNumbers, IWorkflowService workflowService)
        {
            this.exceptionService = exceptionService;
            this.timelineNumbers = timelineNumbers;
            this.workflowService = workflowService;
        }
        #endregion
        public List<StageGateNecessaryDeliverablesItem> GetStageGateDeliverables(string listName)
        {
            List<StageGateNecessaryDeliverablesItem> listItems = new List<StageGateNecessaryDeliverablesItem>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(listName);
                    if (spList != null)
                    {
                        SPListItemCollection collection = spList.GetItems();

                        foreach (SPListItem item in collection)
                        {
                            StageGateNecessaryDeliverablesItem deliverableItem = new StageGateNecessaryDeliverablesItem();
                            deliverableItem.NecessaryDeliverables = item.Title;
                            deliverableItem.Subtask = Convert.ToString(item["Subtask"]);
                            deliverableItem.Owner = Convert.ToString(item["Owner"]);
                            listItems.Add(deliverableItem);
                        }
                    }
                }
            }
            return listItems;
        }
        public StageGateGateItem GetStageGateGateItem(int StageGateListItemId, int gateNo)
        {
            StageGateGateItem gateItem = new StageGateGateItem();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_PMTRAListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateNo + "</Value></Eq></And></Where>";

                    SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);

                    if (StageGateItemCol.Count > 0)
                    {

                        foreach (SPListItem item in StageGateItemCol)
                        {
                            if (item != null)
                            {
                                gateItem.MarketingColor = Convert.ToString(item[PMTRiskAssessmentFIelds.MarketingColor]);
                                gateItem.SalesColor = Convert.ToString(item[PMTRiskAssessmentFIelds.SalesColor]);
                                gateItem.FinanceColor = Convert.ToString(item[PMTRiskAssessmentFIelds.FinanceColor]);
                                gateItem.RDColor = Convert.ToString(item[PMTRiskAssessmentFIelds.RDColor]);
                                gateItem.QAColor = Convert.ToString(item[PMTRiskAssessmentFIelds.QAColor]);
                                gateItem.PEColor = Convert.ToString(item[PMTRiskAssessmentFIelds.PEColor]);
                                gateItem.ManuColor = Convert.ToString(item[PMTRiskAssessmentFIelds.ManuColor]);
                                gateItem.SupplyChainColor = Convert.ToString(item[PMTRiskAssessmentFIelds.SupplyChainColor]);

                                gateItem.MarketingComments = Convert.ToString(item[PMTRiskAssessmentFIelds.MarketingComments]);
                                gateItem.SalesComments = Convert.ToString(item[PMTRiskAssessmentFIelds.SalesComments]);
                                gateItem.FinanceComments = Convert.ToString(item[PMTRiskAssessmentFIelds.FinanceComments]);
                                gateItem.RDComments = Convert.ToString(item[PMTRiskAssessmentFIelds.RDComments]);
                                gateItem.QAComments = Convert.ToString(item[PMTRiskAssessmentFIelds.QAComments]);
                                gateItem.PEComments = Convert.ToString(item[PMTRiskAssessmentFIelds.PEComments]);
                                gateItem.ManuComments = Convert.ToString(item[PMTRiskAssessmentFIelds.ManuComments]);
                                gateItem.SupplyChainComments = Convert.ToString(item[PMTRiskAssessmentFIelds.SupplyChainComments]);

                                gateItem.SGMeetingDate = Convert.ToDateTime(item[PMTRiskAssessmentFIelds.SGMeetingDate]);
                                gateItem.ActualSGMeetingDate = Convert.ToDateTime(item[PMTRiskAssessmentFIelds.ActualSGMeetingDate]);
                                gateItem.SGMeetingStatus = Convert.ToString(item[PMTRiskAssessmentFIelds.SGMeetingStatus]);
                                gateItem.FormSubmittedBy = Convert.ToString(item[PMTRiskAssessmentFIelds.SubmittedBy]);
                                gateItem.FormSubmittedDate = Convert.ToString(item[PMTRiskAssessmentFIelds.SubmittedDate]);
                                gateItem.ReadinessPct = Convert.ToString(item[PMTRiskAssessmentFIelds.GateReadinessPct]);
                                gateItem.DeliverablesApplicable = Convert.ToInt32(item[PMTRiskAssessmentFIelds.TotalApplicable]);
                                gateItem.DeliverablesCompleted = Convert.ToInt32(item[PMTRiskAssessmentFIelds.TotalApplicableCompleted]);
                            }
                        }
                    }
                }
            }
            return gateItem;
        }
        public List<StageGateGateItem> GetStageGateBriefItem(int StageGateListItemId, int gateNo)
        {
            List<StageGateGateItem> ProjectGateItems = new List<StageGateGateItem>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_SGSGateBriefList);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<OrderBy><FieldRef Name=\"BriefNo\" /></OrderBy><Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateNo + "</Value></Eq></And></Where>";

                    SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);

                    if (StageGateItemCol.Count > 0)
                    {
                        foreach (SPListItem item in StageGateItemCol)
                        {
                            if (item != null)
                            {
                                //if (string.Equals(item[SGSGateBriefFields.Deleted], "Yes"))
                                //    continue;

                                StageGateGateItem gateItem = new StageGateGateItem();
                                gateItem.ID = item.ID;
                                gateItem.BriefName = Convert.ToString(item[SGSGateBriefFields.BriefName]);
                                gateItem.BriefNo = Convert.ToInt32(item[SGSGateBriefFields.BriefNo]);
                                gateItem.ProductFormats = Convert.ToString(item[SGSGateBriefFields.ProductFormats]);
                                gateItem.RetailExecution = Convert.ToString(item[SGSGateBriefFields.RetailExecution]);
                                gateItem.OtherKeyInfo = Convert.ToString(item[SGSGateBriefFields.OtherKeyInfo]);
                                gateItem.OverallRisk = Convert.ToString(item[SGSGateBriefFields.OverallRisk]);
                                gateItem.OverallStatus = Convert.ToString(item[SGSGateBriefFields.OverallStatus]);
                                gateItem.Milestones = Convert.ToString(item[SGSGateBriefFields.Milestones]);
                                gateItem.ImpactProjectHealth = Convert.ToString(item[SGSGateBriefFields.ImpactProjectHealth]);
                                gateItem.TeamRecommendation = Convert.ToString(item[SGSGateBriefFields.TeamRecommendation]);

                                gateItem.OverallRiskReason = Convert.ToString(item[SGSGateBriefFields.OverallRiskReason]);
                                gateItem.OverallStatusReason = Convert.ToString(item[SGSGateBriefFields.OverallStatusReason]);
                                gateItem.GateReadiness = Convert.ToString(item[SGSGateBriefFields.GateReadiness]);
                                gateItem.FinanceBriefInGateBrief = Convert.ToString(item[SGSGateBriefFields.FinanceBriefInGateBrief]);
                                gateItem.Deleted = Convert.ToString(item[SGSGateBriefFields.Deleted]);

                                ProjectGateItems.Add(gateItem);
                            }
                        }
                    }
                }
            }
            return ProjectGateItems;
        }
        public StageGateGateItem GetSingleStageGateBriefItem(int briefID)
        {
            StageGateGateItem gateItem = new StageGateGateItem();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_SGSGateBriefList);
                    SPListItem item = spList.GetItemById(briefID);

                    if (item != null)
                    {
                        gateItem.ID = item.ID;
                        gateItem.ProjectNumber = item.Title;
                        gateItem.StageGateListItemId = Convert.ToInt32(item[SGSGateBriefFields.StageGateProjectItemId]);
                        gateItem.Gate = Convert.ToString(item[SGSGateBriefFields.Gate]);
                        gateItem.BriefName = Convert.ToString(item[SGSGateBriefFields.BriefName]);
                        gateItem.BriefNo = Convert.ToInt32(item[SGSGateBriefFields.BriefNo]);
                        gateItem.ProductFormats = Convert.ToString(item[SGSGateBriefFields.ProductFormats]);
                        gateItem.RetailExecution = Convert.ToString(item[SGSGateBriefFields.RetailExecution]);
                        gateItem.OtherKeyInfo = Convert.ToString(item[SGSGateBriefFields.OtherKeyInfo]);
                        gateItem.OverallRisk = Convert.ToString(item[SGSGateBriefFields.OverallRisk]);
                        gateItem.OverallStatus = Convert.ToString(item[SGSGateBriefFields.OverallStatus]);
                        gateItem.Milestones = Convert.ToString(item[SGSGateBriefFields.Milestones]);
                        gateItem.ImpactProjectHealth = Convert.ToString(item[SGSGateBriefFields.ImpactProjectHealth]);
                        gateItem.TeamRecommendation = Convert.ToString(item[SGSGateBriefFields.TeamRecommendation]);
                        gateItem.OverallRiskReason = Convert.ToString(item[SGSGateBriefFields.OverallRiskReason]);
                        gateItem.OverallStatusReason = Convert.ToString(item[SGSGateBriefFields.OverallStatusReason]);
                        gateItem.GateReadiness = Convert.ToString(item[SGSGateBriefFields.GateReadiness]);
                        gateItem.FinanceBriefInGateBrief = Convert.ToString(item[SGSGateBriefFields.FinanceBriefInGateBrief]);
                        gateItem.Deleted = Convert.ToString(item[SGSGateBriefFields.Deleted]);
                    }
                }
            }
            return gateItem;
        }
        public int UpsertDeliverables(int StageGateListItemId, string stage, List<StageGateNecessaryDeliverablesItem> necessaryDeliverables, string projectNumber)
        {
            int addedId = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PMTNecessaryDeliverablesListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=" + StageGateDeliverablesFields.StageGateListItemId + " /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq><Eq><FieldRef Name=" + StageGateDeliverablesFields.Stage + " /><Value Type=\"Text\">" + stage + "</Value></Eq></And></Where>";

                        SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);
                        if (StageGateItemCol.Count > 0)
                        {
                            foreach (SPListItem deliv in StageGateItemCol)
                            {
                                StageGateNecessaryDeliverablesItem deliverable = (from filter in necessaryDeliverables where filter.DelivId == deliv.ID select filter).FirstOrDefault();
                                int index = necessaryDeliverables.IndexOf(deliverable);
                                if (index > -1)
                                {
                                    deliv[StageGateDeliverablesFields.DeliverableDetails] = deliverable.NecessaryDeliverables;
                                    deliv[StageGateDeliverablesFields.Applicable] = deliverable.Applicable;
                                    deliv[StageGateDeliverablesFields.Status] = deliverable.Status;
                                    deliv[StageGateDeliverablesFields.Comments] = deliverable.Comments;
                                    deliv["Editor"] = SPContext.Current.Web.CurrentUser;
                                    deliv["Modified"] = DateTime.Now;
                                    deliv[StageGateDeliverablesFields.Owner] = deliverable.Owner;
                                    deliv.Update();
                                    necessaryDeliverables.RemoveAt(index);
                                }
                            }
                        }
                        if (necessaryDeliverables.Count > 0)
                        {
                            foreach (StageGateNecessaryDeliverablesItem deliv in necessaryDeliverables)
                            {
                                SPListItem item = spList.AddItem();

                                item["Title"] = projectNumber;
                                item[StageGateDeliverablesFields.StageGateListItemId] = StageGateListItemId;
                                item[StageGateDeliverablesFields.Stage] = stage;
                                item[StageGateDeliverablesFields.DeliverableDetails] = deliv.NecessaryDeliverables;
                                item[StageGateDeliverablesFields.Applicable] = deliv.Applicable;
                                item[StageGateDeliverablesFields.Status] = deliv.Status;
                                item[StageGateDeliverablesFields.Comments] = deliv.Comments;
                                item[StageGateDeliverablesFields.Owner] = deliv.Owner;
                                item["Editor"] = SPContext.Current.Web.CurrentUser;
                                item["Modified"] = DateTime.Now;
                                item.Update();
                                addedId = item.ID;
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return addedId;
        }
        public List<StageGateNecessaryDeliverablesItem> GetSavedStageGateDeliverables(int StageGateListItemId, string Stage)
        {
            List<StageGateNecessaryDeliverablesItem> savedNecessaryDeliverables = new List<StageGateNecessaryDeliverablesItem>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_PMTNecessaryDeliverablesListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=" + StageGateDeliverablesFields.StageGateListItemId + " /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq><Eq><FieldRef Name=" + StageGateDeliverablesFields.Stage + " /><Value Type=\"Text\">" + Stage + "</Value></Eq></And></Where>";

                    SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);

                    if (StageGateItemCol.Count > 0)
                    {
                        foreach (SPListItem item in StageGateItemCol)
                        {
                            if (item != null)
                            {
                                StageGateNecessaryDeliverablesItem deliverable = new StageGateNecessaryDeliverablesItem();
                                deliverable.Applicable = Convert.ToString(item[StageGateDeliverablesFields.Applicable]);
                                deliverable.Status = Convert.ToString(item[StageGateDeliverablesFields.Status]);
                                deliverable.Comments = Convert.ToString(item[StageGateDeliverablesFields.Comments]);
                                deliverable.Owner = Convert.ToString(item[StageGateDeliverablesFields.Owner]);
                                deliverable.NecessaryDeliverables = Convert.ToString(item[StageGateDeliverablesFields.DeliverableDetails]);
                                deliverable.ModifiedBy = Convert.ToString(item["Editor"]);
                                deliverable.ModifiedDate = Convert.ToDateTime(item["Modified"]);
                                deliverable.DelivId = item.ID;
                                savedNecessaryDeliverables.Add(deliverable);
                            }
                        }
                    }
                }
            }

            return savedNecessaryDeliverables;
        }
        public int UpsertGateBriefItem(StageGateGateItem gateItem)
        {
            int briefId = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSGateBriefList);
                        SPListItem item;
                        if (gateItem.ID > 0)
                        {
                            item = spList.GetItemById(gateItem.ID);
                        }
                        else
                        {
                            item = spList.AddItem();
                            item["Title"] = gateItem.ProjectNumber;
                            item[SGSGateBriefFields.StageGateProjectItemId] = gateItem.StageGateListItemId;
                            item[SGSGateBriefFields.Gate] = gateItem.Gate;
                        }
                        if (item != null)
                        {
                            item[SGSGateBriefFields.BriefName] = gateItem.BriefName;
                            item[SGSGateBriefFields.ProductFormats] = gateItem.ProductFormats;
                            item[SGSGateBriefFields.RetailExecution] = gateItem.RetailExecution;
                            item[SGSGateBriefFields.OtherKeyInfo] = gateItem.OtherKeyInfo;
                            item[SGSGateBriefFields.OverallRisk] = gateItem.OverallRisk;
                            item[SGSGateBriefFields.OverallStatus] = gateItem.OverallStatus;
                            item[SGSGateBriefFields.Milestones] = gateItem.Milestones;
                            item[SGSGateBriefFields.ImpactProjectHealth] = gateItem.ImpactProjectHealth;
                            item[SGSGateBriefFields.TeamRecommendation] = gateItem.TeamRecommendation;

                            item[SGSGateBriefFields.OverallRiskReason] = gateItem.OverallRiskReason;
                            item[SGSGateBriefFields.OverallStatusReason] = gateItem.OverallStatusReason;
                            item[SGSGateBriefFields.GateReadiness] = gateItem.GateReadiness;
                            item[SGSGateBriefFields.BriefNo] = gateItem.BriefNo;
                            item[SGSGateBriefFields.FinanceBriefInGateBrief] = gateItem.FinanceBriefInGateBrief;
                            item[SGSGateBriefFields.Deleted] = gateItem.Deleted;
                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                            briefId = item.ID;
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return briefId;
        }
        public void updateReadinessDetails(StageGateGateItem gateItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PMTRAListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + gateItem.StageGateListItemId + "</Value></Eq><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateItem.Gate + "</Value></Eq></And></Where>";

                        spQuery.RowLimit = 1;

                        SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);
                        SPListItem item;
                        if (StageGateItemCol.Count < 1)
                        {
                            // If we didn't find it, Insert record
                            item = spList.AddItem();
                            item["Title"] = gateItem.ProjectNumber;
                            item[PMTRiskAssessmentFIelds.StageGateProjectItemId] = gateItem.StageGateListItemId;
                            item[PMTRiskAssessmentFIelds.Gate] = gateItem.Gate;
                        }
                        else
                        {
                            item = StageGateItemCol[0];
                        }

                        if (item != null)
                        {

                            item[PMTRiskAssessmentFIelds.GateReadinessPct] = gateItem.ReadinessPct;
                            item[PMTRiskAssessmentFIelds.TotalApplicable] = gateItem.DeliverablesApplicable;
                            item[PMTRiskAssessmentFIelds.TotalApplicableCompleted] = gateItem.DeliverablesCompleted;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpsertGateDetsItem(StageGateGateItem gateItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PMTRAListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + gateItem.StageGateListItemId + "</Value></Eq><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateItem.Gate + "</Value></Eq></And></Where>";

                        spQuery.RowLimit = 1;

                        SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);
                        SPListItem item;
                        if (StageGateItemCol.Count < 1)
                        {
                            // If we didn't find it, Insert record
                            item = spList.AddItem();
                            item["Title"] = gateItem.ProjectNumber;
                            item[PMTRiskAssessmentFIelds.StageGateProjectItemId] = gateItem.StageGateListItemId;
                            item[PMTRiskAssessmentFIelds.Gate] = gateItem.Gate;
                        }
                        else
                        {
                            item = StageGateItemCol[0];
                        }

                        if (item != null)
                        {
                            //Gate Information
                            item[PMTRiskAssessmentFIelds.MarketingColor] = gateItem.MarketingColor;
                            item[PMTRiskAssessmentFIelds.SalesColor] = gateItem.SalesColor;
                            item[PMTRiskAssessmentFIelds.FinanceColor] = gateItem.FinanceColor;
                            item[PMTRiskAssessmentFIelds.RDColor] = gateItem.RDColor;
                            item[PMTRiskAssessmentFIelds.QAColor] = gateItem.QAColor;
                            item[PMTRiskAssessmentFIelds.PEColor] = gateItem.PEColor;
                            item[PMTRiskAssessmentFIelds.ManuColor] = gateItem.ManuColor;
                            item[PMTRiskAssessmentFIelds.SupplyChainColor] = gateItem.SupplyChainColor;
                            item[PMTRiskAssessmentFIelds.MarketingComments] = gateItem.MarketingComments;
                            item[PMTRiskAssessmentFIelds.SalesComments] = gateItem.SalesComments;
                            item[PMTRiskAssessmentFIelds.FinanceComments] = gateItem.FinanceComments;
                            item[PMTRiskAssessmentFIelds.RDComments] = gateItem.RDComments;
                            item[PMTRiskAssessmentFIelds.QAComments] = gateItem.QAComments;
                            item[PMTRiskAssessmentFIelds.PEComments] = gateItem.PEComments;
                            item[PMTRiskAssessmentFIelds.ManuComments] = gateItem.ManuComments;
                            item[PMTRiskAssessmentFIelds.SupplyChainComments] = gateItem.SupplyChainComments;
                            item[PMTRiskAssessmentFIelds.SGMeetingStatus] = gateItem.SGMeetingStatus;
                            if (gateItem.SGMeetingDate != null && gateItem.SGMeetingDate != DateTime.MinValue)
                            {
                                item[PMTRiskAssessmentFIelds.SGMeetingDate] = gateItem.SGMeetingDate;
                            }
                            if (gateItem.ActualSGMeetingDate != null && gateItem.ActualSGMeetingDate != DateTime.MinValue)
                            {
                                item[PMTRiskAssessmentFIelds.ActualSGMeetingDate] = gateItem.ActualSGMeetingDate;

                            }
                            //item[PMTRiskAssessmentFIelds.GateReadinessPct] = gateItem.ReadinessPct;
                            //item[PMTRiskAssessmentFIelds.TotalApplicable] = gateItem.DeliverablesApplicable;
                            //item[PMTRiskAssessmentFIelds.TotalApplicableCompleted] = gateItem.DeliverablesCompleted;
                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            if (!string.IsNullOrEmpty(gateItem.FormSubmittedDate))
                            {
                                item[PMTRiskAssessmentFIelds.SubmittedDate] = DateTime.Now.ToString();
                                item[PMTRiskAssessmentFIelds.SubmittedBy] = SPContext.Current.Web.CurrentUser.Name;
                            }
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateStageGateGateListSubmitDetails(StageGateGateItem gateItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PMTRAListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + gateItem.StageGateListItemId + "</Value></Eq><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateItem.Gate + "</Value></Eq></And></Where>";

                        spQuery.RowLimit = 1;

                        SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);
                        SPListItem item;
                        if (StageGateItemCol.Count < 1)
                        {
                            // If we didn't find it, Insert record
                            item = spList.AddItem();
                            item["Title"] = gateItem.ProjectNumber;
                            item[PMTRiskAssessmentFIelds.StageGateProjectItemId] = gateItem.StageGateListItemId;
                            item[PMTRiskAssessmentFIelds.Gate] = gateItem.Gate;
                        }
                        else
                        {
                            item = StageGateItemCol[0];
                        }

                        if (item != null)
                        {
                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            if (!string.IsNullOrEmpty(gateItem.FormSubmittedDate))
                            {
                                item[PMTRiskAssessmentFIelds.SubmittedDate] = DateTime.Now.ToString();
                                item[PMTRiskAssessmentFIelds.SubmittedBy] = SPContext.Current.Web.CurrentUser.Name;
                            }
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateProjectItem(StageGateCreateProjectItem stageItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(stageItem.StageGateProjectListItemId);

                        if (item != null)
                        {
                            item[StageGateProjectListFields.Stage] = stageItem.Stage;
                            if (stageItem.RevisedShipDate != null && stageItem.RevisedShipDate != DateTime.MinValue)
                            {
                                item[StageGateProjectListFields.RevisedShipDate] = stageItem.RevisedShipDate;
                            }
                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });
        }
        public List<KeyValuePair<DateTime, string>> GateSubmittedVersions(int StageGateItemId, int gateNo)
        {
            List<KeyValuePair<DateTime, string>> versionList = new List<KeyValuePair<DateTime, string>>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_PMTRAListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + StageGateItemId + "</Value></Eq><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateNo + "</Value></Eq></And></Where>";

                    SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);

                    if (StageGateItemCol.Count > 0)
                    {
                        SPListItem item = StageGateItemCol[0];
                        if (item != null)
                        {
                            SPListItemVersionCollection versionCol = item.Versions;
                            KeyValuePair<DateTime, string> lastVersion = new KeyValuePair<DateTime, string>();
                            string currentVersion = string.Empty;
                            bool bFirstTime = true;
                            SPListItemVersion previousVersion = item.Versions[0];

                            foreach (SPListItemVersion version in versionCol)
                            {

                                if (bFirstTime)
                                {
                                    previousVersion = version;
                                    lastVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[PMTRiskAssessmentFIelds.SubmittedDate]), Convert.ToString(previousVersion[PMTRiskAssessmentFIelds.SubmittedBy]));
                                    bFirstTime = false;

                                    continue;
                                }

                                if (!string.IsNullOrEmpty(Convert.ToString(version[PMTRiskAssessmentFIelds.SubmittedDate])) || (!string.IsNullOrEmpty(lastVersion.Value)))
                                {
                                    currentVersion = Convert.ToDateTime(version[PMTRiskAssessmentFIelds.SubmittedDate]).ToString();

                                    if (!string.Equals(Convert.ToString(lastVersion.Key), currentVersion))
                                    {
                                        versionList.Add(new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[PMTRiskAssessmentFIelds.SubmittedDate]), Convert.ToString(previousVersion[PMTRiskAssessmentFIelds.SubmittedBy])));
                                        //versionList.Add(lastVersion);
                                        previousVersion = version;
                                        lastVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[PMTRiskAssessmentFIelds.SubmittedDate]), Convert.ToString(previousVersion[PMTRiskAssessmentFIelds.SubmittedBy]));
                                    }
                                }
                                previousVersion = version;
                            }
                        }
                    }
                }
            }
            return versionList;
        }
        public List<KeyValuePair<DateTime, string>> SubmittedVersions(int StageGateItemId, int gateNo, string field, string table, string submitterField)
        {
            List<KeyValuePair<DateTime, string>> versionList = new List<KeyValuePair<DateTime, string>>();
            KeyValuePair<DateTime, string> firstVersion = new KeyValuePair<DateTime, string>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(table);
                    SPListItem item = null;
                    SPQuery spQuery = new SPQuery();
                    if (gateNo == 0)
                    {
                        item = spList.GetItemById(StageGateItemId);
                    }
                    else
                    {
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + StageGateItemId + "</Value></Eq><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateNo + "</Value></Eq></And></Where>";
                        SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);
                        if (StageGateItemCol.Count > 0)
                        {
                            item = StageGateItemCol[0];
                        }
                    }
                    if (item != null)
                    {
                        SPListItemVersionCollection versionCol = item.Versions;
                        KeyValuePair<DateTime, string> lastVersion = new KeyValuePair<DateTime, string>();

                        string currentVersion = string.Empty;
                        bool bFirstTime = true;
                        SPListItemVersion previousVersion = item.Versions[0];

                        foreach (SPListItemVersion version in versionCol)
                        {

                            if (bFirstTime)
                            {
                                previousVersion = version;
                                lastVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[field]), Convert.ToString(previousVersion[submitterField]));
                                firstVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[field]), Convert.ToString(previousVersion[submitterField]));
                                bFirstTime = false;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(Convert.ToDateTime(version[field]).ToString()) || (!string.IsNullOrEmpty(Convert.ToDateTime(lastVersion.Key).ToString())))
                            {
                                currentVersion = Convert.ToDateTime(version[field]).ToString();

                                if (!string.Equals(Convert.ToDateTime(lastVersion.Key).ToString(), currentVersion))
                                {
                                    versionList.Add(new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[field]), Convert.ToString(previousVersion[submitterField])));
                                    //versionList.Add(lastVersion);
                                    previousVersion = version;
                                    lastVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[field]), Convert.ToString(previousVersion[submitterField]));
                                }
                            }
                            previousVersion = version;
                        }
                    }
                }
            }
            if (versionList.Count <= 0 && firstVersion.Key != DateTime.MinValue)
            {
                versionList.Add(firstVersion);
            }
            return versionList;
        }
        public List<KeyValuePair<DateTime, string>> Gate0SubmittedVersions(int StageGateItemId, int gateNo, string field, string table, string submitterField)
        {
            List<KeyValuePair<DateTime, string>> versionList = new List<KeyValuePair<DateTime, string>>();
            KeyValuePair<DateTime, string> firstVersion = new KeyValuePair<DateTime, string>();
            List<KeyValuePair<DateTime, string>> gate0List = new List<KeyValuePair<DateTime, string>>();
            KeyValuePair<DateTime, string> firstGate0 = new KeyValuePair<DateTime, string>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                    SPListItem item = spList.GetItemById(StageGateItemId);
                    if (item != null)
                    {
                        SPListItemVersionCollection versionCol = item.Versions;
                        KeyValuePair<DateTime, string> lastVersion = new KeyValuePair<DateTime, string>();
                        KeyValuePair<DateTime, string> lastGate0 = new KeyValuePair<DateTime, string>();

                        string currentVersion = string.Empty;
                        string currentGate0 = string.Empty;
                        bool bFirstTime = true;
                        SPListItemVersion previousVersion = item.Versions[0];

                        foreach (SPListItemVersion version in versionCol)
                        {
                            if (bFirstTime)
                            {
                                previousVersion = version;
                                lastVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.FormSubmittedDate]), Convert.ToString(previousVersion[submitterField]));
                                firstVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.FormSubmittedDate]), Convert.ToString(previousVersion[submitterField]));
                                lastGate0 = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.Gate0ApprovedDate]), Convert.ToString(previousVersion[submitterField]));
                                firstGate0 = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.Gate0ApprovedDate]), Convert.ToString(previousVersion[submitterField]));
                                bFirstTime = false;

                                continue;
                            }

                            if (!string.IsNullOrEmpty(Convert.ToDateTime(version[StageGateProjectListFields.FormSubmittedDate]).ToString()) || (!string.IsNullOrEmpty(Convert.ToDateTime(lastVersion.Key).ToString())))
                            {
                                currentVersion = Convert.ToDateTime(version[StageGateProjectListFields.FormSubmittedDate]).ToString();
                                currentGate0 = Convert.ToDateTime(version[StageGateProjectListFields.Gate0ApprovedDate]).ToString();
                                if (!string.Equals(Convert.ToDateTime(lastVersion.Key).ToString(), currentVersion))
                                {
                                    //if(!string.Equals(Convert.ToDateTime(lastGate0.Key).ToString(), currentGate0))
                                    //{
                                    gate0List.Add(new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.Gate0ApprovedDate]), Convert.ToString(previousVersion[submitterField])));
                                    //}
                                    versionList.Add(new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.FormSubmittedDate]), Convert.ToString(previousVersion[submitterField])));
                                    //versionList.Add(lastVersion);
                                    previousVersion = version;
                                    lastVersion = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.FormSubmittedDate]), Convert.ToString(previousVersion[submitterField]));
                                    lastGate0 = new KeyValuePair<DateTime, string>(Convert.ToDateTime(previousVersion[StageGateProjectListFields.Gate0ApprovedDate]), Convert.ToString(previousVersion[submitterField]));
                                }
                            }
                            previousVersion = version;
                        }
                    }
                }
            }
            if (gate0List.Count <= 0 && firstVersion.Key != DateTime.MinValue)
            {
                gate0List.Add(firstGate0);
            }
            else
            {
                string lastGate = DateTime.MinValue.ToString();
                List<int> indexes = new List<int>();
                List<KeyValuePair<DateTime, string>> gateHolder = gate0List;
                foreach (KeyValuePair<DateTime, string> pair in gate0List)
                {
                    string currentGate = pair.Key.ToString();
                    int index = gate0List.IndexOf(pair);
                    if (currentGate == lastGate)
                    {
                        indexes.Add(index);
                    }
                    lastGate = pair.Key.ToString();
                }
                foreach (int index in indexes)
                {
                    gate0List.RemoveAt(index);
                }
            }
            return gate0List;
        }
        public void deleteChildProjectDetails(int TempProjectId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSChildProjectTempList);
                        SPListItem item = spList.GetItemById(TempProjectId);
                        if (item != null)
                        {
                            item.Delete();
                        }
                        //item.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void deleteDeliverable(int deliverableId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PMTNecessaryDeliverablesListName);
                        SPListItem item = spList.GetItemById(deliverableId);
                        if (item != null)
                        {
                            item.Delete();
                        }
                        //item.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void updateChildItemDetails(ItemProposalItem ipfItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSChildProjectTempList);
                        SPListItem item = spList.GetItemById(ipfItem.CompassListItemId);
                        if (item != null)
                        {
                            item[SGSChildProjectTempListFields.TBDIndicator] = ipfItem.TBDIndicator;
                            item[SGSChildProjectTempListFields.FinishedGood] = ipfItem.SAPItemNumber;
                            item[SGSChildProjectTempListFields.Description] = ipfItem.SAPDescription;
                            item[SGSChildProjectTempListFields.ProductHierarchy1] = ipfItem.ProductHierarchyLevel1;
                            item[SGSChildProjectTempListFields.ManuallyCreateSAPDescription] = ipfItem.ManuallyCreateSAPDescription;
                            item[SGSChildProjectTempListFields.ProductHierarchy2] = ipfItem.ProductHierarchyLevel2;
                            item[SGSChildProjectTempListFields.BrandMaterialGroup1] = ipfItem.MaterialGroup1Brand;
                            item[SGSChildProjectTempListFields.ProductMaterialGroup4] = ipfItem.MaterialGroup4ProductForm;
                            item[SGSChildProjectTempListFields.PackTypeMaterialGroup5] = ipfItem.MaterialGroup5PackType;
                            item[SGSChildProjectTempListFields.RequireNewUPCUCC] = ipfItem.RequireNewUPCUCC;
                            item[SGSChildProjectTempListFields.RequireNewUnitUPC] = ipfItem.RequireNewUnitUPC;
                            item[SGSChildProjectTempListFields.UnitUPC] = ipfItem.UnitUPC;
                            item[SGSChildProjectTempListFields.RequireNewDisplayBoxUPC] = ipfItem.RequireNewDisplayBoxUPC;
                            item[SGSChildProjectTempListFields.DisplayBoxUPC] = ipfItem.DisplayBoxUPC;
                            item[SGSChildProjectTempListFields.SAPBaseUOM] = ipfItem.SAPBaseUOM;
                            item[SGSChildProjectTempListFields.RequireNewCaseUCC] = ipfItem.RequireNewCaseUCC;
                            item[SGSChildProjectTempListFields.CaseUCC] = ipfItem.CaseUCC;
                            item[SGSChildProjectTempListFields.RequireNewPalletUCC] = ipfItem.RequireNewPalletUCC;
                            item[SGSChildProjectTempListFields.PalletUCC] = ipfItem.PalletUCC;
                            item[SGSChildProjectTempListFields.CustomerSpecific] = ipfItem.CustomerSpecific;
                            item[SGSChildProjectTempListFields.Customer] = ipfItem.Customer;
                            item[SGSChildProjectTempListFields.Channel] = ipfItem.Channel;
                            item[SGSChildProjectTempListFields.FGReplacingAnExistingFG] = ipfItem.FGReplacingAnExistingFG;
                            item[SGSChildProjectTempListFields.IsThisAnLTOItem] = ipfItem.IsThisAnLTOItem;
                            item[SGSChildProjectTempListFields.RequestChangeToFGNumForSameUCC] = ipfItem.RequestChangeToFGNumForSameUCC;
                            if ((ipfItem.LTOTransitionStartWindowRDD != null) && (ipfItem.LTOTransitionStartWindowRDD != DateTime.MinValue))
                                item[SGSChildProjectTempListFields.LTOTransitionStartWindowRDD] = ipfItem.LTOTransitionStartWindowRDD;
                            if ((ipfItem.LTOTransitionEndWindowRDD != null) && (ipfItem.LTOTransitionEndWindowRDD != DateTime.MinValue))
                                item[SGSChildProjectTempListFields.LTOTransitionEndWindowRDD] = ipfItem.LTOTransitionEndWindowRDD;
                            item[SGSChildProjectTempListFields.LTOEndDateFlexibility] = ipfItem.LTOEndDateFlexibility;
                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public int upsertTempIPFList(ItemProposalItem newItem)
        {
            int tempId = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSChildProjectTempList);
                        SPListItem item;
                        if (newItem.ProjectNumber == "0")
                        {
                            // If we didn't find it, Insert record
                            item = spList.AddItem();

                            //item["Title"] = newItem.ProjectNumber;
                            item[SGSChildProjectTempListFields.ParentID] = newItem.StageGateProjectListItemId;
                            item[SGSChildProjectTempListFields.GenerateIPFSortOrder] = newItem.GenerateIPFSortOrder;
                            item["Created By"] = SPContext.Current.Web.CurrentUser;
                            //item[SGSChildProjectTempListFields.ProjectNumber] = newItem.ProjectNumber;
                            item.Update();
                        }
                        else
                        {
                            item = spList.GetItemById(Convert.ToInt32(newItem.ProjectNumber));
                        }
                        item[SGSChildProjectTempListFields.TBDIndicator] = newItem.TBDIndicator;
                        item[SGSChildProjectTempListFields.FinishedGood] = newItem.SAPItemNumber;
                        item[SGSChildProjectTempListFields.Description] = newItem.SAPDescription;
                        item[SGSChildProjectTempListFields.BrandMaterialGroup1] = newItem.MaterialGroup1Brand;
                        item[SGSChildProjectTempListFields.Customer] = newItem.Customer;

                        tempId = item.ID;
                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;
                        item.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return tempId;
        }
        public int insertTempIPFList(ItemProposalItem newItem)
        {
            int tempId = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSChildProjectTempList);
                        SPListItem item;
                        if (newItem.ProjectNumber == "0")
                        {
                            // If we didn't find it, Insert record
                            item = spList.AddItem();

                            //item["Title"] = newItem.ProjectNumber;
                            item[SGSChildProjectTempListFields.ParentID] = newItem.StageGateProjectListItemId;
                            item[SGSChildProjectTempListFields.GenerateIPFSortOrder] = newItem.GenerateIPFSortOrder;
                            item["Created By"] = SPContext.Current.Web.CurrentUser;
                            //item[SGSChildProjectTempListFields.ProjectNumber] = newItem.ProjectNumber;
                            item.Update();
                        }
                        else
                        {
                            item = spList.GetItemById(Convert.ToInt32(newItem.ProjectNumber));
                        }
                        item[SGSChildProjectTempListFields.TBDIndicator] = newItem.TBDIndicator;
                        item[SGSChildProjectTempListFields.FinishedGood] = newItem.SAPItemNumber;
                        item[SGSChildProjectTempListFields.Description] = newItem.SAPDescription;
                        item[SGSChildProjectTempListFields.Customer] = newItem.Customer;
                        item[SGSChildProjectTempListFields.ProductHierarchy1] = newItem.ProductHierarchyLevel1;
                        item[SGSChildProjectTempListFields.ProductHierarchy2] = newItem.ProductHierarchyLevel2;
                        item[SGSChildProjectTempListFields.BrandMaterialGroup1] = newItem.MaterialGroup1Brand;
                        tempId = item.ID;
                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;
                        item.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return tempId;
        }
        public void updateStageforPostLaunch(StageGateCreateProjectItem stageItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(stageItem.StageGateProjectListItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.Stage] = stageItem.Stage;
                            item[StageGateProjectListFields.PostLaunchActive] = stageItem.PostLaunchActive;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }

                        List<int> CompassIds = new List<int>();
                        SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery3 = new SPQuery();
                        spQuery3.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Int\">" + stageItem.StageGateProjectListItemId + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
                        if (compassItemCol3.Count > 0)
                        {
                            foreach (SPListItem item2 in compassItemCol3)
                            {
                                if (item2 != null)
                                {
                                    string wfPhase = Convert.ToString(item2[CompassListFields.WorkflowPhase]);

                                    if (wfPhase.ToLower() != "completed" & wfPhase.ToLower() != "cancelled" & wfPhase.ToLower() != "on hold")
                                    {
                                        //timelineNumbers.workflowStatusUpdate(item2.ID, "Completed");
                                        workflowService.UpdateWorkflowPhase(item2.ID, "Completed");
                                        CompassIds.Add(item2.ID);
                                    }
                                }
                            }
                        }

                        foreach (var CompassId in CompassIds)
                        {
                            string updateDate = DateTime.Now.ToString();

                            SPList spList4 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                            SPQuery spQuery4 = new SPQuery();
                            spQuery4.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassId.ToString() + "</Value></Eq></Where>";
                            SPListItemCollection itemUpdate = spList4.GetItems(spQuery4);

                            if (itemUpdate.Count > 0)
                            {
                                foreach (SPListItem appItem in itemUpdate)
                                {
                                    if (appItem != null)
                                    {
                                        appItem[ApprovalListFields.Completed_ModifiedDate] = updateDate;
                                        appItem[ApprovalListFields.Completed_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                        appItem.Update();
                                    }
                                }
                            }

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void updateRevisedFirstShip(StageGateCreateProjectItem stageItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(stageItem.StageGateProjectListItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.RevisedShipDate] = stageItem.RevisedShipDate;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void updateCurrentStage(StageGateCreateProjectItem stageItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(stageItem.StageGateProjectListItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.Stage] = stageItem.Stage;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void cancelProject(int StageGateListItemId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(StageGateListItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.WorkflowPhase] = "Cancelled";
                            item[StageGateProjectListFields.Stage] = "Cancelled";
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        List<int> CompassIds = new List<int>();
                        SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery3 = new SPQuery();
                        spQuery3.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
                        string CancellationReason = "Parent project was cancelled.";
                        if (compassItemCol3.Count > 0)
                        {
                            foreach (SPListItem item2 in compassItemCol3)
                            {
                                if (item != null)
                                {
                                    item2[CompassListFields.WorkflowPhase] = "Cancelled";
                                    item2[CompassListFields.LastUpdatedFormName] = GlobalConstants.PAGE_StageGateProjectPanel;
                                    item2[CompassListFields.ProjectNotes] = string.Concat("Reason for cancellation: ", CancellationReason);
                                    item2["Editor"] = SPContext.Current.Web.CurrentUser;
                                    item2.Update();
                                    CompassIds.Add(item2.ID);
                                    UpdateProjectCancellationReasons(item2.ID, CancellationReason);
                                    //timelineNumbers.workflowStatusUpdate(item2.ID, "cancelled");
                                    workflowService.StartCancelWorkflow(item2.ID);
                                }
                            }
                        }

                        foreach (var CompassId in CompassIds)
                        {
                            string updateDate = DateTime.Now.ToString();

                            SPList spList4 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                            SPQuery spQuery4 = new SPQuery();
                            spQuery4.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassId.ToString() + "</Value></Eq></Where>";
                            SPListItemCollection itemUpdate = spList4.GetItems(spQuery4);

                            if (itemUpdate.Count > 0)
                            {
                                foreach (SPListItem appItem in itemUpdate)
                                {
                                    if (appItem != null)
                                    {
                                        appItem[ApprovalListFields.Cancelled_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                        appItem[ApprovalListFields.Cancelled_ModifiedDate] = updateDate;
                                        appItem.Update();
                                    }
                                }
                            }

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void ProjectOnHold(int StageGateListItemId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(StageGateListItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.WorkflowPhase] = GlobalConstants.WORKFLOWPHASE_OnHold;
                            item[StageGateProjectListFields.StageOnHold] = item[StageGateProjectListFields.Stage];
                            item[StageGateProjectListFields.Stage] = GlobalConstants.WORKFLOWPHASE_OnHold;
                            item[StageGateProjectListFields.OnHoldStartDate] = DateTime.Now;
                            item[StageGateProjectListFields.OnHoldEndDate] = string.Empty;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }

                        List<int> CompassIds = new List<int>();
                        SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery3 = new SPQuery();
                        spQuery3.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
                        if (compassItemCol3.Count > 0)
                        {
                            foreach (SPListItem item2 in compassItemCol3)
                            {
                                if (item2 != null)
                                {
                                    string wfPhase = Convert.ToString(item2[CompassListFields.WorkflowPhase]);

                                    if (wfPhase.ToLower() != "completed" & wfPhase.ToLower() != "cancelled" & wfPhase.ToLower() != "on hold")
                                    {
                                        item2[CompassListFields.PMTWorkflowUpdateStatus] = "OnHold";
                                        item2[CompassListFields.OnHoldWorkflowPhase] = wfPhase;
                                        item2[CompassListFields.LastUpdatedFormName] = GlobalConstants.PAGE_StageGateProjectPanel;
                                        item2["Editor"] = SPContext.Current.Web.CurrentUser;
                                        item2.Update();
                                        CompassIds.Add(item2.ID);
                                    }
                                }
                            }
                        }

                        foreach (var CompassId in CompassIds)
                        {
                            string updateDate = DateTime.Now.ToString();

                            SPList spList4 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassId.ToString() + "</Value></Eq></Where>";
                            SPListItemCollection itemUpdate = spList4.GetItems(spQuery);

                            if (itemUpdate.Count > 0)
                            {
                                foreach (SPListItem appItem in itemUpdate)
                                {
                                    if (appItem != null)
                                    {
                                        appItem[ApprovalListFields.OnHold_ModifiedDate] = updateDate;
                                        appItem[ApprovalListFields.OnHold_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                        appItem.Update();
                                    }
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void RemoveProjectOnHold(int StageGateListItemId, int TotalOnHoldDays)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);
                        SPListItem item = spList.GetItemById(StageGateListItemId);
                        if (item != null)
                        {
                            item[StageGateProjectListFields.WorkflowPhase] = "";
                            item[StageGateProjectListFields.Stage] = item[StageGateProjectListFields.StageOnHold];
                            item[StageGateProjectListFields.StageOnHold] = string.Empty;
                            item[StageGateProjectListFields.TotalOnHoldDays] = TotalOnHoldDays;
                            item[StageGateProjectListFields.OnHoldEndDate] = DateTime.Now;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }

                        List<int> CompassIds = new List<int>();
                        SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery3 = new SPQuery();
                        spQuery3.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
                        if (compassItemCol3.Count > 0)
                        {
                            foreach (SPListItem item2 in compassItemCol3)
                            {
                                if (item2 != null)
                                {
                                    string wfPhase = Convert.ToString(item2[CompassListFields.WorkflowPhase]);

                                    if (wfPhase.ToLower() == "on hold")
                                    {
                                        if (Convert.ToString(item2[CompassListFields.OnHoldWorkflowPhase]) == GlobalConstants.WORKFLOWPHASE_PreProduction)
                                        {
                                            item2[CompassListFields.WorkflowPhase] = GlobalConstants.WORKFLOWPHASE_PreProduction;
                                            item2[CompassListFields.OnHoldWorkflowPhase] = string.Empty;
                                        }
                                        else
                                        {
                                            item2[CompassListFields.PMTWorkflowUpdateStatus] = "ReleaseOnHold";
                                        }
                                        item2[CompassListFields.LastUpdatedFormName] = GlobalConstants.PAGE_StageGateProjectPanel;
                                        item2["Editor"] = SPContext.Current.Web.CurrentUser;
                                        item2.Update();
                                        CompassIds.Add(item2.ID);
                                    }
                                }
                            }
                        }

                        foreach (var CompassId in CompassIds)
                        {
                            string updateDate = DateTime.Now.ToString();

                            SPList spList4 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);
                            SPQuery spQuery4 = new SPQuery();
                            spQuery4.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassId.ToString() + "</Value></Eq></Where>";
                            SPListItemCollection itemUpdate = spList4.GetItems(spQuery4);

                            if (itemUpdate.Count > 0)
                            {
                                foreach (SPListItem appItem in itemUpdate)
                                {
                                    if (appItem != null)
                                    {
                                        appItem[ApprovalListFields.OnHold_ModifiedDate] = updateDate;
                                        appItem[ApprovalListFields.OnHold_ModifiedBy] = SPContext.Current.Web.CurrentUser.Name;
                                        appItem.Update();
                                    }
                                }
                            }

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public bool CheckPMTWorkflowUpdateStatuses(int StageGateListItemId, string status)
        {
            bool childWithTheStatusPresent = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList3 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPQuery spQuery3 = new SPQuery();
                        spQuery3.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\" /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol3 = spList3.GetItems(spQuery3);
                        if (compassItemCol3.Count > 0)
                        {
                            foreach (SPListItem item2 in compassItemCol3)
                            {
                                if (item2 != null)
                                {
                                    string PMTWorkflowUpdateStatus = Convert.ToString(item2[CompassListFields.PMTWorkflowUpdateStatus]);

                                    if (PMTWorkflowUpdateStatus == status)
                                    {
                                        childWithTheStatusPresent = true;
                                        continue;
                                    }
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return childWithTheStatusPresent;
        }
        public ItemProposalItem GetTempIPFItem(int TempItemId, int StageGateParentProjectId)
        {
            var newItem = new ItemProposalItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSChildProjectTempList);
                    var item = spList.GetItemById(TempItemId);
                    if (item != null)
                    {
                        newItem.StageGateProjectListItemId = Convert.ToInt32(item[SGSChildProjectTempListFields.ParentID]);
                        newItem.ProjectNumber = Convert.ToString(item.ID);
                        newItem.SAPItemNumber = Convert.ToString(item[SGSChildProjectTempListFields.FinishedGood]);
                        newItem.SAPDescription = Convert.ToString(item[SGSChildProjectTempListFields.Description]);
                        newItem.ProductHierarchyLevel1 = Convert.ToString(item[SGSChildProjectTempListFields.ProductHierarchy1]);
                        newItem.ManuallyCreateSAPDescription = Convert.ToString(item[SGSChildProjectTempListFields.ManuallyCreateSAPDescription]);
                        newItem.ProductHierarchyLevel2 = Convert.ToString(item[SGSChildProjectTempListFields.ProductHierarchy2]);
                        newItem.MaterialGroup1Brand = Convert.ToString(item[SGSChildProjectTempListFields.BrandMaterialGroup1]);
                        newItem.MaterialGroup4ProductForm = Convert.ToString(item[SGSChildProjectTempListFields.ProductMaterialGroup4]);
                        newItem.MaterialGroup5PackType = Convert.ToString(item[SGSChildProjectTempListFields.PackTypeMaterialGroup5]);
                        newItem.RequireNewUPCUCC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewUPCUCC]);
                        newItem.RequireNewUnitUPC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewUnitUPC]);
                        newItem.UnitUPC = Convert.ToString(item[SGSChildProjectTempListFields.UnitUPC]);
                        newItem.RequireNewDisplayBoxUPC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewDisplayBoxUPC]);
                        newItem.DisplayBoxUPC = Convert.ToString(item[SGSChildProjectTempListFields.DisplayBoxUPC]);
                        newItem.RequireNewCaseUCC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewCaseUCC]);
                        newItem.CaseUCC = Convert.ToString(item[SGSChildProjectTempListFields.CaseUCC]);
                        newItem.RequireNewPalletUCC = Convert.ToString(item[SGSChildProjectTempListFields.RequireNewPalletUCC]);
                        newItem.PalletUCC = Convert.ToString(item[SGSChildProjectTempListFields.PalletUCC]);
                        newItem.SAPBaseUOM = Convert.ToString(item[SGSChildProjectTempListFields.SAPBaseUOM]);
                        newItem.GenerateIPFSortOrder = Convert.ToInt32(item[SGSChildProjectTempListFields.GenerateIPFSortOrder]);
                        newItem.CustomerSpecific = Convert.ToString(item[SGSChildProjectTempListFields.CustomerSpecific]);
                        newItem.Customer = Convert.ToString(item[SGSChildProjectTempListFields.Customer]);
                        newItem.Channel = Convert.ToString(item[SGSChildProjectTempListFields.Channel]);

                        newItem.FGReplacingAnExistingFG = Convert.ToString(item[SGSChildProjectTempListFields.FGReplacingAnExistingFG]);
                        newItem.IsThisAnLTOItem = Convert.ToString(item[SGSChildProjectTempListFields.IsThisAnLTOItem]);
                        newItem.RequestChangeToFGNumForSameUCC = Convert.ToString(item[SGSChildProjectTempListFields.RequestChangeToFGNumForSameUCC]);
                        newItem.LTOTransitionStartWindowRDD = Convert.ToDateTime(item[SGSChildProjectTempListFields.LTOTransitionStartWindowRDD]);
                        newItem.LTOTransitionEndWindowRDD = Convert.ToDateTime(item[SGSChildProjectTempListFields.LTOTransitionEndWindowRDD]);
                        newItem.LTOEndDateFlexibility = Convert.ToString(item[SGSChildProjectTempListFields.LTOEndDateFlexibility]);

                        newItem.TBDIndicator = Convert.ToString(item[SGSChildProjectTempListFields.TBDIndicator]);
                    }
                }
            }
            return newItem;
        }
        public bool UploadStageGateDocument(List<FileAttribute> fileList, string projectNo, string docType, string Gate, string BriefNo)
        {
            bool isUploaded = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_StageGateLibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                        if (!spweb.GetFolder(folderUrl).Exists)
                        {
                            SPListItem projectFolder = documentLib.Items.Add("", SPFileSystemObjectType.Folder, projectNo);
                            projectFolder.Update();
                        }
                        SPFolder stagGateDocLibrary = spweb.GetFolder(folderUrl);
                        foreach (var file in fileList)
                        {
                            try
                            {
                                string fileName = file.FileName;
                                SPFile spfile = stagGateDocLibrary.Files.Add(fileName, file.FileContent, true);
                                spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateDocType] = file.DocType;
                                spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateGate] = Gate;
                                spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateBriefNo] = BriefNo;
                                spfile.Item[StageGateProjectListFields.DOCLIBRARY_DisplayFileName] = fileName;
                                spfile.Item[StageGateProjectListFields.Title] = fileName;

                                spfile.Item.Update();
                                isUploaded = true;
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "StageGateGeneralService", "UploadStageGateDocument)", " projectNo: " + projectNo + " docType: " + docType + " Gate: " + Gate + " BriefNo: " + BriefNo);
                            }
                        }
                        stagGateDocLibrary.Update();
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isUploaded;
        }
        public List<FileAttribute> GetUploadedStageGateFiles(string projectNo, string docType, string Gate, string BriefNo, string webUrl)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(webUrl))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_StageGateLibraryName) as SPDocumentLibrary;
                    string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>()
                            .Where(x =>
                                x.Item[StageGateProjectListFields.DOCLIBRARY_StageGateGate].Equals(Gate)
                                &&
                                 x.Item[StageGateProjectListFields.DOCLIBRARY_StageGateBriefNo].Equals(BriefNo)
                                &&
                                x.Item[StageGateProjectListFields.DOCLIBRARY_StageGateDocType].Equals(docType)).ToList();
                        if (spfiles.Count > 0)
                        {
                            foreach (SPFile spfile in spfiles)
                            {
                                FileAttribute file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = Convert.ToString(spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateDocType]);
                                file.DisplayFileName = Convert.ToString(spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateGate_DisplayName]);
                                file.FileContent = spfile.OpenBinary();
                                files.Add(file);
                            }
                        }
                    }
                }
            }
            return files;
        }
        public List<FileAttribute> GetStageGateFiles(string projectNo, string webUrl)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(webUrl))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_StageGateLibraryName) as SPDocumentLibrary;
                    string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        var spfiles = stageGateProjectFolder.Files;//.OfType<SPFile>().OrderByDescending();
                        if (spfiles.Count > 0)
                        {
                            foreach (SPFile spfile in spfiles)
                            {
                                FileAttribute file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = Convert.ToString(spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateDocType]);
                                file.DisplayFileName = Convert.ToString(spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateGate_DisplayName]);
                                file.FileContent = spfile.OpenBinary();
                                file.FileContentLength = Convert.ToInt32(spfile.Item[StageGateProjectListFields.DOCLIBRARY_StageGateGate]);
                                files.Add(file);
                            }
                        }
                    }
                    SPDocumentLibrary documentLib2 = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    var folderUrl2 = string.Concat(documentLib2.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl2).Exists)
                    {
                        SPFolder CompassProjectFolder = spweb.GetFolder(folderUrl2);
                        try
                        {
                            var spFilesNullFilter = CompassProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType] != null).ToList();
                            var spFiles = spFilesNullFilter.Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_StageGateOthers) || x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_StageGateProjectBrief)).ToList();
                            foreach (SPFile spfile in spFiles)
                            {
                                var file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType]);
                                file.FileContent = spfile.OpenBinary();
                                file.FileContentLength = 0;
                                files.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            exceptionService.Handle(LogCategory.CriticalError, ex, "UtilityService", "GetUploadedFilesByDocType(string docLibrary, string folder, string docType)", string.Concat(projectNo, "-", GlobalConstants.DOCTYPE_StageGateOthers));
                        }
                    }
                }
            }
            return files;
        }
        public void DeleteStageGateFiles(string folder, string docType, string Gate, string BriefNo)
        {
            SPDocumentLibrary documentLib;
            SPFolder stageGateProjectFolder;
            IEnumerable<SPFile> spFiles;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;

                        documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_StageGateLibraryName) as SPDocumentLibrary;
                        var folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", folder);
                        if (spweb.GetFolder(folderUrl).Exists)
                        {
                            stageGateProjectFolder = spweb.GetFolder(folderUrl);
                            try
                            {
                                spFiles = stageGateProjectFolder.Files.OfType<SPFile>()
                                            .Where(x =>
                                                        x.Item[StageGateProjectListFields.DOCLIBRARY_StageGateGate].Equals(Gate)
                                                        &&
                                                        x.Item[StageGateProjectListFields.DOCLIBRARY_StageGateBriefNo].Equals(BriefNo)
                                                        &&
                                                        x.Item[StageGateProjectListFields.DOCLIBRARY_StageGateDocType].Equals(docType)
                                                    ).ToList();
                                foreach (SPFile spfile in spFiles)
                                {
                                    spfile.Delete();
                                }
                            }
                            catch (Exception ex)
                            {
                                exceptionService.Handle(LogCategory.CriticalError, ex, "StageGateGeneralService", "DeleteStageGateFiles", " projectNo: " + folder + " docType: " + docType + " Gate: " + Gate + " BriefNo: " + BriefNo);
                            }
                        }
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void DeleteProjectGateInfo(int deleteID)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSGateBriefList);
                        SPListItem itemD = spList.GetItemById(deleteID);
                        itemD.Delete();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetPrelimStartDate(int compassListItemId, DateTime startDate)
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
                                if (item[ApprovalListFields.PrelimSAPInitialSetup_StartDate] == null)
                                {
                                    item[ApprovalListFields.PrelimSAPInitialSetup_StartDate] = startDate.ToString();
                                    item.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public List<ItemProposalItem> GetSearchProjectName(string ProjectNo)
        {
            List<ItemProposalItem> IPFItems = new List<ItemProposalItem>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Or><Or><Contains><FieldRef Name=" + CompassListFields.ProjectNumber + " /><Value Type=\"Text\">" + ProjectNo + "</Value></Contains><Contains><FieldRef Name=" + CompassListFields.SAPDescription + " /><Value Type=\"Text\">" + ProjectNo + "</Value></Contains></Or><Contains><FieldRef Name=" + CompassListFields.SAPItemNumber + " /><Value Type=\"Text\">" + ProjectNo + "</Value></Contains></Or></Where>";

                    SPListItemCollection PMTItemCol = spList.GetItems(spQuery);

                    if (PMTItemCol.Count > 0)
                    {
                        foreach (SPListItem item in PMTItemCol)
                        {
                            if (item != null)
                            {
                                ItemProposalItem ipfItem = new ItemProposalItem();
                                ipfItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                                ipfItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                                ipfItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                                ipfItem.CompassListItemId = item.ID;
                                IPFItems.Add(ipfItem);
                            }
                        }
                    }
                }
            }
            return IPFItems;
        }
        public List<ItemProposalItem> GetSearchParentProjectName(string ProjectNo)
        {
            List<ItemProposalItem> IPFItems = new List<ItemProposalItem>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_StageGateProjectListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Or><Contains><FieldRef Name=" + StageGateProjectListFields.ProjectNumber + " /><Value Type=\"Text\">" + ProjectNo + "</Value></Contains><Contains><FieldRef Name=" + StageGateProjectListFields.ProjectName + " /><Value Type=\"Text\">" + ProjectNo + "</Value></Contains></Or></Where>";

                    SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);

                    if (StageGateItemCol.Count > 0)
                    {
                        foreach (SPListItem item in StageGateItemCol)
                        {
                            if (item != null)
                            {
                                ItemProposalItem ipfItem = new ItemProposalItem();
                                ipfItem.ProjectNumber = Convert.ToString(item[StageGateProjectListFields.ProjectNumber]);
                                ipfItem.SAPItemNumber = Convert.ToString(item[StageGateProjectListFields.ProjectName]);
                                ipfItem.StageGateProjectListItemId = item.ID;
                                IPFItems.Add(ipfItem);
                            }
                        }
                    }
                }
            }
            return IPFItems;
        }
        public void updateSGSPHL2(ItemProposalItem stageItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SGSChildProjectTempList);
                        SPListItem item = spList.GetItemById(stageItem.CompassListItemId);
                        if (item != null)
                        {
                            item[SGSChildProjectTempListFields.ProductHierarchy1] = stageItem.ProductHierarchyLevel1;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public List<StageGateGateItem> GetOtherStageGateBriefItem(int StageGateListItemId, int gateNo)
        {
            List<StageGateGateItem> ProjectGateItems = new List<StageGateGateItem>();
            using (var site = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spweb = site.OpenWeb())
                {
                    var spList = spweb.Lists.TryGetList(GlobalConstants.LIST_SGSGateBriefList);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<OrderBy><FieldRef Name=\"Gate\" /></OrderBy><Where><And><Eq><FieldRef Name=" + PMTRiskAssessmentFIelds.StageGateProjectItemId + " /><Value Type=\"Int\">" + StageGateListItemId + "</Value></Eq><Neq><FieldRef Name=" + PMTRiskAssessmentFIelds.Gate + " /><Value Type=\"Text\">" + gateNo + "</Value></Neq></And></Where>";

                    SPListItemCollection StageGateItemCol = spList.GetItems(spQuery);

                    if (StageGateItemCol.Count > 0)
                    {
                        foreach (SPListItem item in StageGateItemCol)
                        {
                            if (item != null)
                            {
                                if (string.Equals(item[SGSGateBriefFields.Deleted], "Yes"))
                                    continue;

                                StageGateGateItem gateItem = new StageGateGateItem();
                                gateItem.ID = item.ID;
                                gateItem.Gate = Convert.ToString(item[SGSGateBriefFields.Gate]);
                                gateItem.BriefName = Convert.ToString(item[SGSGateBriefFields.BriefName]);
                                gateItem.BriefNo = Convert.ToInt32(item[SGSGateBriefFields.BriefNo]);
                                gateItem.ProductFormats = Convert.ToString(item[SGSGateBriefFields.ProductFormats]);
                                gateItem.RetailExecution = Convert.ToString(item[SGSGateBriefFields.RetailExecution]);
                                gateItem.OtherKeyInfo = Convert.ToString(item[SGSGateBriefFields.OtherKeyInfo]);
                                gateItem.OverallRisk = Convert.ToString(item[SGSGateBriefFields.OverallRisk]);
                                gateItem.OverallStatus = Convert.ToString(item[SGSGateBriefFields.OverallStatus]);
                                gateItem.Milestones = Convert.ToString(item[SGSGateBriefFields.Milestones]);
                                gateItem.ImpactProjectHealth = Convert.ToString(item[SGSGateBriefFields.ImpactProjectHealth]);
                                gateItem.TeamRecommendation = Convert.ToString(item[SGSGateBriefFields.TeamRecommendation]);

                                gateItem.OverallRiskReason = Convert.ToString(item[SGSGateBriefFields.OverallRiskReason]);
                                gateItem.OverallStatusReason = Convert.ToString(item[SGSGateBriefFields.OverallStatusReason]);
                                gateItem.GateReadiness = Convert.ToString(item[SGSGateBriefFields.GateReadiness]);
                                gateItem.FinanceBriefInGateBrief = Convert.ToString(item[SGSGateBriefFields.FinanceBriefInGateBrief]);
                                gateItem.Deleted = Convert.ToString(item[SGSGateBriefFields.Deleted]);

                                ProjectGateItems.Add(gateItem);
                            }
                        }
                    }
                }
            }
            return ProjectGateItems;
        }

        public void UpdateProjectCancellationReasons(int CompassListItemId, string CancellationReason)
        {
            // Get the current user
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
                                appItem[CompassProjectDecisionsListFields.CancellationReasons] = CancellationReason;
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}