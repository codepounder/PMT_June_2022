using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;

namespace Ferrara.Compass.Services
{
    public class StageGateFinancialServices : IStageGateFinancialServices
    {
        #region Member Variables
        private readonly IExceptionService exceptionService;
        private readonly IWorkflowService workflowServices;
        #endregion
        #region Constructor
        public StageGateFinancialServices(IExceptionService exceptionService, IWorkflowService workflowServices)
        {
            this.exceptionService = exceptionService;
            this.workflowServices = workflowServices;
        }
        #endregion
        #region Financial Summary
        public StageGateConsolidatedFinancialSummaryItem GetStageGateConsolidatedFinancialSummaryItem(int StageGateProjectListItemitemId, string Gate, string BriefNumber)
        {
            StageGateConsolidatedFinancialSummaryItem newItem = null;
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateConsolidatedFinancialSummaryListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"StageGateProjectListItemId\"/><Value Type=\"Text\">" + StageGateProjectListItemitemId + "</Value></Eq><Eq><FieldRef Name=" + StageGateConsolidatedFinancialSummaryListFields.Gate + "/><Value Type=\"Text\">" + Gate + "</Value></Eq></And><Eq><FieldRef Name=\"BriefNumber\" /><Value Type=\"Text\">" + BriefNumber + "</Value></Eq></And></Where>";

                    SPListItemCollection items = spList.GetItems(spQuery);

                    foreach (SPListItem item in items)
                    {
                        if (item != null)
                        {
                            newItem = new StageGateConsolidatedFinancialSummaryItem();
                            newItem.Id = item.ID;
                            newItem.StageGateProjectListItemId = Convert.ToInt32(item[StageGateConsolidatedFinancialSummaryListFields.StageGateProjectListItemId]);
                            newItem.Gate = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.Gate]);
                            newItem.BriefNumber = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.BriefNumber]);
                            newItem.BriefName = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.BriefName]);
                            newItem.BriefSummary = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.BriefSummary]);
                            newItem.Name = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.Name]);
                            newItem.AverageTargetMargin = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.AverageTargetMargin]);
                            newItem.DispConsFinInProjBrief = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.DispConsFinInProjBrief]);
                            newItem.VolumeTotal1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal1]);
                            newItem.VolumeIncremental1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental1]);
                            newItem.GrossSalesTotal1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal1]);
                            newItem.GrossSalesIncremental1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental1]);
                            newItem.NetSalesTotal1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal1]);
                            newItem.NetSalesIncremental1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental1]);
                            newItem.COGSTotal1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal1]);
                            newItem.COGSIncremental1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental1]);
                            newItem.GrossMarginTotal1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal1]);
                            newItem.GrossMarginIncremental1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental1]);
                            newItem.GrossMarginPctTotal1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal1]);
                            newItem.GrossMarginPctIncremental1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental1]);
                            newItem.VolumeTotal2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal2]);
                            newItem.VolumeIncremental2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental2]);
                            newItem.GrossSalesTotal2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal2]);
                            newItem.GrossSalesIncremental2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental2]);
                            newItem.NetSalesTotal2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal2]);
                            newItem.NetSalesIncremental2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental2]);
                            newItem.COGSTotal2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal2]);
                            newItem.COGSIncremental2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental2]);
                            newItem.GrossMarginTotal2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal2]);
                            newItem.GrossMarginIncremental2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental2]);
                            newItem.GrossMarginPctTotal2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal2]);
                            newItem.GrossMarginPctIncremental2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental2]);
                            newItem.VolumeTotal3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal3]);
                            newItem.VolumeIncremental3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental3]);
                            newItem.GrossSalesTotal3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal3]);
                            newItem.GrossSalesIncremental3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental3]);
                            newItem.NetSalesTotal3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal3]);
                            newItem.NetSalesIncremental3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental3]);
                            newItem.COGSTotal3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal3]);
                            newItem.COGSIncremental3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental3]);
                            newItem.GrossMarginTotal3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal3]);
                            newItem.GrossMarginIncremental3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental3]);
                            newItem.GrossMarginPctTotal3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal3]);
                            newItem.GrossMarginPctIncremental3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental3]);
                            newItem.NSDollerperLB1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB1]);
                            newItem.NSDollerperLB2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB2]);
                            newItem.NSDollerperLB3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB3]);
                            newItem.COGSperLB1 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB1]);
                            newItem.COGSperLB2 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB2]);
                            newItem.COGSperLB3 = Convert.ToDouble(item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB3]);
                            newItem.Analysesincluded = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.Analysesincluded]);

                            newItem.FormSubmittedDate = Convert.ToDateTime(item[StageGateConsolidatedFinancialSummaryListFields.FormSubmittedDate]);
                            newItem.ModifiedDate = Convert.ToDateTime(item[StageGateConsolidatedFinancialSummaryListFields.ModifiedDate]);
                            newItem.FormSubmittedBy = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.FormSubmittedBy]);
                            newItem.ModifiedBy = Convert.ToString(item[StageGateConsolidatedFinancialSummaryListFields.ModifiedBy]);
                        }
                    }
                }
                return newItem;
            }
        }
        public List<KeyValuePair<DateTime, string>> GetAllStageGateCreatedFinancialSummaryItems(int StageGateProjectListItemitemId, string Gate)
        {
            List<KeyValuePair<DateTime, string>> allItems = new List<KeyValuePair<DateTime, string>>();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateConsolidatedFinancialSummaryListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"StageGateProjectListItemId\"/><Value Type=\"Text\">" + StageGateProjectListItemitemId + "</Value></Eq><Eq><FieldRef Name=" + StageGateConsolidatedFinancialSummaryListFields.Gate + "/><Value Type=\"Text\">" + Gate + "</Value></Eq></And></Where>";

                    SPListItemCollection items = spList.GetItems(spQuery);

                    foreach (SPListItem item in items)
                    {
                        if (item != null)
                        {
                            KeyValuePair<DateTime, string> newItem = new KeyValuePair<DateTime, string>(Convert.ToDateTime(item["Created"]), Convert.ToString(item["Created By"]));
                            allItems.Add(newItem);
                        }
                    }
                }
                return allItems;
            }
        }
        public int InsertStageGateConsolidatedFinancialSummaryItem(StageGateConsolidatedFinancialSummaryItem sgitem, bool submitted)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateConsolidatedFinancialSummaryListName);

                        var item = spList.AddItem();

                        item["Title"] = sgitem.StageGateProjectListItemId;

                        item[StageGateConsolidatedFinancialSummaryListFields.StageGateProjectListItemId] = sgitem.StageGateProjectListItemId;
                        item[StageGateConsolidatedFinancialSummaryListFields.Gate] = sgitem.Gate;
                        item[StageGateConsolidatedFinancialSummaryListFields.BriefNumber] = sgitem.BriefNumber;
                        item[StageGateConsolidatedFinancialSummaryListFields.BriefName] = sgitem.BriefName;
                        item[StageGateConsolidatedFinancialSummaryListFields.BriefSummary] = sgitem.BriefSummary;
                        item[StageGateConsolidatedFinancialSummaryListFields.Name] = sgitem.Name;
                        item[StageGateConsolidatedFinancialSummaryListFields.AverageTargetMargin] = sgitem.AverageTargetMargin;
                        item[StageGateConsolidatedFinancialSummaryListFields.DispConsFinInProjBrief] = sgitem.DispConsFinInProjBrief;
                        item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal1] = sgitem.VolumeTotal1;
                        item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental1] = sgitem.VolumeIncremental1;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal1] = sgitem.GrossSalesTotal1;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental1] = sgitem.GrossSalesIncremental1;
                        item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal1] = sgitem.NetSalesTotal1;
                        item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental1] = sgitem.NetSalesIncremental1;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal1] = sgitem.COGSTotal1;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental1] = sgitem.COGSIncremental1;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal1] = sgitem.GrossMarginTotal1;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental1] = sgitem.GrossMarginIncremental1;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal1] = sgitem.GrossMarginPctTotal1;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental1] = sgitem.GrossMarginPctIncremental1;
                        item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal2] = sgitem.VolumeTotal2;
                        item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental2] = sgitem.VolumeIncremental2;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal2] = sgitem.GrossSalesTotal2;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental2] = sgitem.GrossSalesIncremental2;
                        item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal2] = sgitem.NetSalesTotal2;
                        item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental2] = sgitem.NetSalesIncremental2;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal2] = sgitem.COGSTotal2;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental2] = sgitem.COGSIncremental2;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal2] = sgitem.GrossMarginTotal2;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental2] = sgitem.GrossMarginIncremental2;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal2] = sgitem.GrossMarginPctTotal2;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental2] = sgitem.GrossMarginPctIncremental2;
                        item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal3] = sgitem.VolumeTotal3;
                        item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental3] = sgitem.VolumeIncremental3;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal3] = sgitem.GrossSalesTotal3;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental3] = sgitem.GrossSalesIncremental3;
                        item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal3] = sgitem.NetSalesTotal3;
                        item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental3] = sgitem.NetSalesIncremental3;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal3] = sgitem.COGSTotal3;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental3] = sgitem.COGSIncremental3;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal3] = sgitem.GrossMarginTotal3;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental3] = sgitem.GrossMarginIncremental3;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal3] = sgitem.GrossMarginPctTotal3;
                        item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental3] = sgitem.GrossMarginPctIncremental3;
                        item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB1] = sgitem.NSDollerperLB1;
                        item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB2] = sgitem.NSDollerperLB2;
                        item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB3] = sgitem.NSDollerperLB3;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB1] = sgitem.COGSperLB1;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB2] = sgitem.COGSperLB2;
                        item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB3] = sgitem.COGSperLB3;
                        item[StageGateConsolidatedFinancialSummaryListFields.Analysesincluded] = sgitem.Analysesincluded;


                        item["Modified By"] = SPContext.Current.Web.CurrentUser;

                        if (submitted)
                        {
                            item[StageGateConsolidatedFinancialSummaryListFields.FormSubmittedDate] = DateTime.Now.ToString();
                            item[StageGateConsolidatedFinancialSummaryListFields.FormSubmittedBy] = SPContext.Current.Web.CurrentUser;
                        }
                        else
                        {
                            item[StageGateConsolidatedFinancialSummaryListFields.ModifiedDate] = DateTime.Now.ToString();
                            item[StageGateConsolidatedFinancialSummaryListFields.ModifiedBy] = SPContext.Current.Web.CurrentUser;
                        }

                        item.Update();

                        sgitem.StageGateProjectListItemId = item.ID;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return sgitem.StageGateProjectListItemId;
        }
        public int UpdateStageGateConsolidatedFinancialSummaryItem(StageGateConsolidatedFinancialSummaryItem sgitem, bool submitted)
        {
            //Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateConsolidatedFinancialSummaryListName);
                        if (sgitem.Id != 0)
                        {
                            var item = spList.GetItemById(sgitem.Id);
                            item["Title"] = sgitem.StageGateProjectListItemId;

                            item[StageGateConsolidatedFinancialSummaryListFields.StageGateProjectListItemId] = sgitem.StageGateProjectListItemId;
                            item[StageGateConsolidatedFinancialSummaryListFields.Gate] = sgitem.Gate;
                            item[StageGateConsolidatedFinancialSummaryListFields.BriefNumber] = sgitem.BriefNumber;
                            item[StageGateConsolidatedFinancialSummaryListFields.BriefName] = sgitem.BriefName;
                            item[StageGateConsolidatedFinancialSummaryListFields.BriefSummary] = sgitem.BriefSummary;
                            item[StageGateConsolidatedFinancialSummaryListFields.Name] = sgitem.Name;
                            item[StageGateConsolidatedFinancialSummaryListFields.AverageTargetMargin] = sgitem.AverageTargetMargin;
                            item[StageGateConsolidatedFinancialSummaryListFields.DispConsFinInProjBrief] = sgitem.DispConsFinInProjBrief;
                            item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal1] = sgitem.VolumeTotal1;
                            item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental1] = sgitem.VolumeIncremental1;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal1] = sgitem.GrossSalesTotal1;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental1] = sgitem.GrossSalesIncremental1;
                            item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal1] = sgitem.NetSalesTotal1;
                            item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental1] = sgitem.NetSalesIncremental1;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal1] = sgitem.COGSTotal1;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental1] = sgitem.COGSIncremental1;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal1] = sgitem.GrossMarginTotal1;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental1] = sgitem.GrossMarginIncremental1;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal1] = sgitem.GrossMarginPctTotal1;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental1] = sgitem.GrossMarginPctIncremental1;
                            item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal2] = sgitem.VolumeTotal2;
                            item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental2] = sgitem.VolumeIncremental2;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal2] = sgitem.GrossSalesTotal2;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental2] = sgitem.GrossSalesIncremental2;
                            item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal2] = sgitem.NetSalesTotal2;
                            item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental2] = sgitem.NetSalesIncremental2;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal2] = sgitem.COGSTotal2;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental2] = sgitem.COGSIncremental2;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal2] = sgitem.GrossMarginTotal2;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental2] = sgitem.GrossMarginIncremental2;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal2] = sgitem.GrossMarginPctTotal2;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental2] = sgitem.GrossMarginPctIncremental2;
                            item[StageGateConsolidatedFinancialSummaryListFields.VolumeTotal3] = sgitem.VolumeTotal3;
                            item[StageGateConsolidatedFinancialSummaryListFields.VolumeIncremental3] = sgitem.VolumeIncremental3;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesTotal3] = sgitem.GrossSalesTotal3;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossSalesIncremental3] = sgitem.GrossSalesIncremental3;
                            item[StageGateConsolidatedFinancialSummaryListFields.NetSalesTotal3] = sgitem.NetSalesTotal3;
                            item[StageGateConsolidatedFinancialSummaryListFields.NetSalesIncremental3] = sgitem.NetSalesIncremental3;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSTotal3] = sgitem.COGSTotal3;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSIncremental3] = sgitem.COGSIncremental3;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginTotal3] = sgitem.GrossMarginTotal3;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginIncremental3] = sgitem.GrossMarginIncremental3;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctTotal3] = sgitem.GrossMarginPctTotal3;
                            item[StageGateConsolidatedFinancialSummaryListFields.GrossMarginPctIncremental3] = sgitem.GrossMarginPctIncremental3;
                            item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB1] = sgitem.NSDollerperLB1;
                            item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB2] = sgitem.NSDollerperLB2;
                            item[StageGateConsolidatedFinancialSummaryListFields.NSDollerperLB3] = sgitem.NSDollerperLB3;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB1] = sgitem.COGSperLB1;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB2] = sgitem.COGSperLB2;
                            item[StageGateConsolidatedFinancialSummaryListFields.COGSperLB3] = sgitem.COGSperLB3;
                            item[StageGateConsolidatedFinancialSummaryListFields.Analysesincluded] = sgitem.Analysesincluded;


                            item["Modified By"] = SPContext.Current.Web.CurrentUser;

                            if (submitted)
                            {
                                item[StageGateConsolidatedFinancialSummaryListFields.FormSubmittedDate] = DateTime.Now.ToString();
                                item[StageGateConsolidatedFinancialSummaryListFields.FormSubmittedBy] = SPContext.Current.Web.CurrentUser;
                            }
                            else
                            {
                                item[StageGateConsolidatedFinancialSummaryListFields.ModifiedDate] = DateTime.Now.ToString();
                                item[StageGateConsolidatedFinancialSummaryListFields.ModifiedBy] = SPContext.Current.Web.CurrentUser;
                            }

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                            id = sgitem.StageGateProjectListItemId;
                        }
                        else
                        {
                            id = InsertStageGateConsolidatedFinancialSummaryItem(sgitem, submitted);
                        }
                    }
                }
            });
            return id;
        }
        #endregion
        #region Financial Analysis
        public List<StageGateFinancialAnalysisItem> GetAllStageGateFinancialAnalysisItems(int StageGateProjectListItemitemId)
        {
            var newItems = new List<StageGateFinancialAnalysisItem>();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateFinancialAnalysisListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"StageGateProjectListItemId\"/><Value Type=\"Text\">" + StageGateProjectListItemitemId + "</Value></Eq></Where>";

                    SPListItemCollection items = spList.GetItems(spQuery);

                    foreach (SPListItem item in items)
                    {
                        if (item != null)
                        {
                            if (string.Equals(item[StageGateFinancialAnalysisListFields.Deleted], "Yes"))
                                continue;

                            var newItem = new StageGateFinancialAnalysisItem();
                            newItem.Id = item.ID;
                            newItem.StageGateProjectListItemId = Convert.ToInt32(item[StageGateFinancialAnalysisListFields.StageGateProjectListItemId]);
                            newItem.Gate = Convert.ToString(item[StageGateFinancialAnalysisListFields.Gate]);
                            newItem.BriefNumber = Convert.ToString(item[StageGateFinancialAnalysisListFields.BriefNumber]);
                            newItem.BriefName = Convert.ToString(item[StageGateFinancialAnalysisListFields.BriefName]);
                            newItem.AnalysisName = Convert.ToString(item[StageGateFinancialAnalysisListFields.AnalysisName]);
                            newItem.FGNumber = Convert.ToString(item[StageGateFinancialAnalysisListFields.FGNumber]);
                            newItem.CustomerChannel = Convert.ToString(item[StageGateFinancialAnalysisListFields.CustomerChannel]);
                            newItem.BrandSeason = Convert.ToString(item[StageGateFinancialAnalysisListFields.BrandSeason]);
                            newItem.ProductForm = Convert.ToString(item[StageGateFinancialAnalysisListFields.ProductForm]);
                            newItem.TargetMarginPct = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TargetMarginPct]);
                            newItem.PLsinProjectBrief = Convert.ToString(item[StageGateFinancialAnalysisListFields.PLsinProjectBrief]);
                            newItem.PLinConsolidatedFinancials = Convert.ToString(item[StageGateFinancialAnalysisListFields.PLinConsolidatedFinancials]);
                            newItem.VolumeTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal1]);
                            newItem.VolumeIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental1]);
                            newItem.GrossSalesTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal1]);
                            newItem.GrossSalesIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental1]);
                            newItem.TradeRateTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal1]);
                            newItem.TradeRateIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental1]);
                            newItem.OGTNTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal1]);
                            newItem.OGTNIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental1]);
                            newItem.NetSalesTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal1]);
                            newItem.NetSalesIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental1]);
                            newItem.COGSTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal1]);
                            newItem.COGSIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental1]);
                            newItem.GrossMarginTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal1]);
                            newItem.GrossMarginIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental1]);
                            newItem.GrossMarginPctTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal1]);
                            newItem.GrossMarginPctIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental1]);
                            newItem.VolumeTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal2]);
                            newItem.VolumeIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental2]);
                            newItem.GrossSalesTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal2]);
                            newItem.GrossSalesIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental2]);
                            newItem.TradeRateTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal2]);
                            newItem.TradeRateIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental2]);
                            newItem.OGTNTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal2]);
                            newItem.OGTNIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental2]);
                            newItem.NetSalesTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal2]);
                            newItem.NetSalesIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental2]);
                            newItem.COGSTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal2]);
                            newItem.COGSIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental2]);
                            newItem.GrossMarginTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal2]);
                            newItem.GrossMarginIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental2]);
                            newItem.GrossMarginPctTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal2]);
                            newItem.GrossMarginPctIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental2]);
                            newItem.VolumeTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal3]);
                            newItem.VolumeIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental3]);
                            newItem.GrossSalesTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal3]);
                            newItem.GrossSalesIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental3]);
                            newItem.TradeRateTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal3]);
                            newItem.TradeRateIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental3]);
                            newItem.OGTNTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal3]);
                            newItem.OGTNIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental3]);
                            newItem.NetSalesTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal3]);
                            newItem.NetSalesIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental3]);
                            newItem.COGSTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal3]);
                            newItem.COGSIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental3]);
                            newItem.GrossMarginTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal3]);
                            newItem.GrossMarginIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental3]);
                            newItem.GrossMarginPctTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal3]);
                            newItem.GrossMarginPctIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental3]);
                            newItem.NSDollerperLB1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB1]);
                            newItem.NSDollerperLB2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB2]);
                            newItem.NSDollerperLB3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB3]);
                            newItem.COGSperLB1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB1]);
                            newItem.COGSperLB2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB2]);
                            newItem.COGSperLB3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB3]);
                            newItem.TruckldPricePrRtlSllngUt1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt1]);
                            newItem.TruckldPricePrRtlSllngUt2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt2]);
                            newItem.TruckldPricePrRtlSllngUt3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt3]);
                            newItem.Assumptions1 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions1]);
                            newItem.Assumptions2 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions2]);
                            newItem.Assumptions3 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions3]);
                            newItem.Deleted = Convert.ToString(item[StageGateFinancialAnalysisListFields.Deleted]);


                            newItem.FormSubmittedDate = Convert.ToDateTime(item[StageGateFinancialAnalysisListFields.FormSubmittedDate]);
                            newItem.ModifiedDate = Convert.ToDateTime(item[StageGateFinancialAnalysisListFields.ModifiedDate]);
                            newItem.FormSubmittedBy = Convert.ToString(item[StageGateFinancialAnalysisListFields.FormSubmittedBy]);
                            newItem.ModifiedBy = Convert.ToString(item[StageGateFinancialAnalysisListFields.ModifiedBy]);

                            newItems.Add(newItem);
                        }
                    }
                }
                return newItems;
            }
        }
        public List<StageGateFinancialAnalysisItem> GetAllStageGateFinancialAnalysisItemsByGate(int StageGateProjectListItemitemId, string Gate)
        {
            var newItems = new List<StageGateFinancialAnalysisItem>();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateFinancialAnalysisListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"StageGateProjectListItemId\"/><Value Type=\"Text\">" + StageGateProjectListItemitemId + "</Value></Eq><Eq><FieldRef Name=" + StageGateFinancialAnalysisListFields.Gate + " /><Value Type=\"Text\">" + Gate + "</Value></Eq></And></Where>";


                    SPListItemCollection items = spList.GetItems(spQuery);

                    foreach (SPListItem item in items)
                    {
                        if (item != null)
                        {
                            if (string.Equals(item[StageGateFinancialAnalysisListFields.Deleted], "Yes"))
                                continue;

                            var newItem = new StageGateFinancialAnalysisItem();
                            newItem.Id = item.ID;
                            newItem.StageGateProjectListItemId = Convert.ToInt32(item[StageGateFinancialAnalysisListFields.StageGateProjectListItemId]);
                            newItem.Gate = Convert.ToString(item[StageGateFinancialAnalysisListFields.Gate]);
                            newItem.BriefNumber = Convert.ToString(item[StageGateFinancialAnalysisListFields.BriefNumber]);
                            newItem.BriefName = Convert.ToString(item[StageGateFinancialAnalysisListFields.BriefName]);
                            newItem.AnalysisName = Convert.ToString(item[StageGateFinancialAnalysisListFields.AnalysisName]);
                            newItem.FGNumber = Convert.ToString(item[StageGateFinancialAnalysisListFields.FGNumber]);
                            newItem.CustomerChannel = Convert.ToString(item[StageGateFinancialAnalysisListFields.CustomerChannel]);
                            newItem.BrandSeason = Convert.ToString(item[StageGateFinancialAnalysisListFields.BrandSeason]);
                            newItem.ProductForm = Convert.ToString(item[StageGateFinancialAnalysisListFields.ProductForm]);
                            newItem.TargetMarginPct = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TargetMarginPct]);
                            newItem.PLsinProjectBrief = Convert.ToString(item[StageGateFinancialAnalysisListFields.PLsinProjectBrief]);
                            newItem.PLinConsolidatedFinancials = Convert.ToString(item[StageGateFinancialAnalysisListFields.PLinConsolidatedFinancials]);
                            newItem.VolumeTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal1]);
                            newItem.VolumeIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental1]);
                            newItem.GrossSalesTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal1]);
                            newItem.GrossSalesIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental1]);
                            newItem.TradeRateTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal1]);
                            newItem.TradeRateIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental1]);
                            newItem.OGTNTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal1]);
                            newItem.OGTNIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental1]);
                            newItem.NetSalesTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal1]);
                            newItem.NetSalesIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental1]);
                            newItem.COGSTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal1]);
                            newItem.COGSIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental1]);
                            newItem.GrossMarginTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal1]);
                            newItem.GrossMarginIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental1]);
                            newItem.GrossMarginPctTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal1]);
                            newItem.GrossMarginPctIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental1]);
                            newItem.VolumeTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal2]);
                            newItem.VolumeIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental2]);
                            newItem.GrossSalesTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal2]);
                            newItem.GrossSalesIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental2]);
                            newItem.TradeRateTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal2]);
                            newItem.TradeRateIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental2]);
                            newItem.OGTNTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal2]);
                            newItem.OGTNIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental2]);
                            newItem.NetSalesTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal2]);
                            newItem.NetSalesIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental2]);
                            newItem.COGSTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal2]);
                            newItem.COGSIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental2]);
                            newItem.GrossMarginTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal2]);
                            newItem.GrossMarginIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental2]);
                            newItem.GrossMarginPctTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal2]);
                            newItem.GrossMarginPctIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental2]);
                            newItem.VolumeTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal3]);
                            newItem.VolumeIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental3]);
                            newItem.GrossSalesTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal3]);
                            newItem.GrossSalesIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental3]);
                            newItem.TradeRateTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal3]);
                            newItem.TradeRateIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental3]);
                            newItem.OGTNTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal3]);
                            newItem.OGTNIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental3]);
                            newItem.NetSalesTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal3]);
                            newItem.NetSalesIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental3]);
                            newItem.COGSTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal3]);
                            newItem.COGSIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental3]);
                            newItem.GrossMarginTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal3]);
                            newItem.GrossMarginIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental3]);
                            newItem.GrossMarginPctTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal3]);
                            newItem.GrossMarginPctIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental3]);
                            newItem.NSDollerperLB1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB1]);
                            newItem.NSDollerperLB2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB2]);
                            newItem.NSDollerperLB3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB3]);
                            newItem.COGSperLB1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB1]);
                            newItem.COGSperLB2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB2]);
                            newItem.COGSperLB3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB3]);
                            newItem.TruckldPricePrRtlSllngUt1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt1]);
                            newItem.TruckldPricePrRtlSllngUt2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt2]);
                            newItem.TruckldPricePrRtlSllngUt3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt3]);
                            newItem.Assumptions1 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions1]);
                            newItem.Assumptions2 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions2]);
                            newItem.Assumptions3 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions3]);
                            newItem.Deleted = Convert.ToString(item[StageGateFinancialAnalysisListFields.Deleted]);


                            newItem.FormSubmittedDate = Convert.ToDateTime(item[StageGateFinancialAnalysisListFields.FormSubmittedDate]);
                            newItem.ModifiedDate = Convert.ToDateTime(item[StageGateFinancialAnalysisListFields.ModifiedDate]);
                            newItem.FormSubmittedBy = Convert.ToString(item[StageGateFinancialAnalysisListFields.FormSubmittedBy]);
                            newItem.ModifiedBy = Convert.ToString(item[StageGateFinancialAnalysisListFields.ModifiedBy]);

                            newItems.Add(newItem);
                        }
                    }
                }
                return newItems;
            }
        }
        public List<StageGateFinancialAnalysisItem> GetAllStageGateFinancialAnalysisItemsByGateAndBriefNumber(int StageGateProjectListItemitemId, string Gate, string BriefNumber)
        {
            var newItems = new List<StageGateFinancialAnalysisItem>();

            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateFinancialAnalysisListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"StageGateProjectListItemId\"/><Value Type=\"Text\">" + StageGateProjectListItemitemId + "</Value></Eq><Eq><FieldRef Name=" + StageGateFinancialAnalysisListFields.Gate + "/><Value Type=\"Text\">" + Gate + "</Value></Eq></And><Eq><FieldRef Name =\"BriefNumber\" /><Value Type=\"Text\">" + BriefNumber + "</Value></Eq></And></Where>";

                    SPListItemCollection items = spList.GetItems(spQuery);

                    foreach (SPListItem item in items)
                    {
                        if (item != null)
                        {
                            if (string.Equals(item[StageGateFinancialAnalysisListFields.Deleted], "Yes"))
                                continue;

                            var newItem = new StageGateFinancialAnalysisItem();

                            newItem.Id = item.ID;
                            newItem.StageGateProjectListItemId = Convert.ToInt32(item[StageGateFinancialAnalysisListFields.StageGateProjectListItemId]);
                            newItem.Gate = Convert.ToString(item[StageGateFinancialAnalysisListFields.Gate]);
                            newItem.BriefNumber = Convert.ToString(item[StageGateFinancialAnalysisListFields.BriefNumber]);
                            newItem.BriefName = Convert.ToString(item[StageGateFinancialAnalysisListFields.BriefName]);
                            newItem.AnalysisName = Convert.ToString(item[StageGateFinancialAnalysisListFields.AnalysisName]);
                            newItem.FGNumber = Convert.ToString(item[StageGateFinancialAnalysisListFields.FGNumber]);
                            newItem.CustomerChannel = Convert.ToString(item[StageGateFinancialAnalysisListFields.CustomerChannel]);
                            newItem.BrandSeason = Convert.ToString(item[StageGateFinancialAnalysisListFields.BrandSeason]);
                            newItem.ProductForm = Convert.ToString(item[StageGateFinancialAnalysisListFields.ProductForm]);
                            newItem.TargetMarginPct = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TargetMarginPct]);
                            newItem.PLsinProjectBrief = Convert.ToString(item[StageGateFinancialAnalysisListFields.PLsinProjectBrief]);
                            newItem.PLinConsolidatedFinancials = Convert.ToString(item[StageGateFinancialAnalysisListFields.PLinConsolidatedFinancials]);
                            newItem.VolumeTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal1]);
                            newItem.VolumeIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental1]);
                            newItem.GrossSalesTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal1]);
                            newItem.GrossSalesIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental1]);
                            newItem.TradeRateTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal1]);
                            newItem.TradeRateIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental1]);
                            newItem.OGTNTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal1]);
                            newItem.OGTNIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental1]);
                            newItem.NetSalesTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal1]);
                            newItem.NetSalesIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental1]);
                            newItem.COGSTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal1]);
                            newItem.COGSIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental1]);
                            newItem.GrossMarginTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal1]);
                            newItem.GrossMarginIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental1]);
                            newItem.GrossMarginPctTotal1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal1]);
                            newItem.GrossMarginPctIncremental1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental1]);
                            newItem.VolumeTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal2]);
                            newItem.VolumeIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental2]);
                            newItem.GrossSalesTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal2]);
                            newItem.GrossSalesIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental2]);
                            newItem.TradeRateTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal2]);
                            newItem.TradeRateIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental2]);
                            newItem.OGTNTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal2]);
                            newItem.OGTNIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental2]);
                            newItem.NetSalesTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal2]);
                            newItem.NetSalesIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental2]);
                            newItem.COGSTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal2]);
                            newItem.COGSIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental2]);
                            newItem.GrossMarginTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal2]);
                            newItem.GrossMarginIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental2]);
                            newItem.GrossMarginPctTotal2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal2]);
                            newItem.GrossMarginPctIncremental2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental2]);
                            newItem.VolumeTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeTotal3]);
                            newItem.VolumeIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.VolumeIncremental3]);
                            newItem.GrossSalesTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesTotal3]);
                            newItem.GrossSalesIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossSalesIncremental3]);
                            newItem.TradeRateTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateTotal3]);
                            newItem.TradeRateIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TradeRateIncremental3]);
                            newItem.OGTNTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNTotal3]);
                            newItem.OGTNIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.OGTNIncremental3]);
                            newItem.NetSalesTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesTotal3]);
                            newItem.NetSalesIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NetSalesIncremental3]);
                            newItem.COGSTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSTotal3]);
                            newItem.COGSIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSIncremental3]);
                            newItem.GrossMarginTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginTotal3]);
                            newItem.GrossMarginIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginIncremental3]);
                            newItem.GrossMarginPctTotal3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal3]);
                            newItem.GrossMarginPctIncremental3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental3]);
                            newItem.NSDollerperLB1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB1]);
                            newItem.NSDollerperLB2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB2]);
                            newItem.NSDollerperLB3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.NSDollerperLB3]);
                            newItem.COGSperLB1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB1]);
                            newItem.COGSperLB2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB2]);
                            newItem.COGSperLB3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.COGSperLB3]);
                            newItem.TruckldPricePrRtlSllngUt1 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt1]);
                            newItem.TruckldPricePrRtlSllngUt2 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt2]);
                            newItem.TruckldPricePrRtlSllngUt3 = Convert.ToDouble(item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt3]);
                            newItem.Assumptions1 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions1]);
                            newItem.Assumptions2 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions2]);
                            newItem.Assumptions3 = Convert.ToString(item[StageGateFinancialAnalysisListFields.Assumptions3]);
                            newItem.Deleted = Convert.ToString(item[StageGateFinancialAnalysisListFields.Deleted]);


                            newItem.FormSubmittedDate = Convert.ToDateTime(item[StageGateFinancialAnalysisListFields.FormSubmittedDate]);
                            newItem.ModifiedDate = Convert.ToDateTime(item[StageGateFinancialAnalysisListFields.ModifiedDate]);
                            newItem.FormSubmittedBy = Convert.ToString(item[StageGateFinancialAnalysisListFields.FormSubmittedBy]);
                            newItem.ModifiedBy = Convert.ToString(item[StageGateFinancialAnalysisListFields.ModifiedBy]);
                            newItems.Add(newItem);
                        }
                    }
                }
                return newItems;
            }
        }
        public List<int> InsertStageGateFinancialAnalysisItems(List<StageGateFinancialAnalysisItem> items, bool submitted)
        {
            var Ids = new List<int>();
            foreach (var item in items)
            {
                Ids.Add(InsertStageGateFinancialAnalysisItem(item, submitted));
            }
            return Ids;
        }
        public int InsertStageGateFinancialAnalysisItem(StageGateFinancialAnalysisItem sgitem, bool submitted)
        {
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;

            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateFinancialAnalysisListName);

                        var item = spList.AddItem();

                        item["Title"] = sgitem.StageGateProjectListItemId;

                        item[StageGateFinancialAnalysisListFields.StageGateProjectListItemId] = sgitem.StageGateProjectListItemId;
                        item[StageGateFinancialAnalysisListFields.Gate] = sgitem.Gate;
                        item[StageGateFinancialAnalysisListFields.BriefNumber] = sgitem.BriefNumber;
                        item[StageGateFinancialAnalysisListFields.BriefName] = sgitem.BriefName;
                        item[StageGateFinancialAnalysisListFields.AnalysisName] = sgitem.AnalysisName;
                        item[StageGateFinancialAnalysisListFields.FGNumber] = sgitem.FGNumber;
                        item[StageGateFinancialAnalysisListFields.CustomerChannel] = sgitem.CustomerChannel;
                        item[StageGateFinancialAnalysisListFields.BrandSeason] = sgitem.BrandSeason;
                        item[StageGateFinancialAnalysisListFields.ProductForm] = sgitem.ProductForm;
                        item[StageGateFinancialAnalysisListFields.TargetMarginPct] = sgitem.TargetMarginPct;
                        item[StageGateFinancialAnalysisListFields.PLsinProjectBrief] = sgitem.PLsinProjectBrief;
                        item[StageGateFinancialAnalysisListFields.PLinConsolidatedFinancials] = sgitem.PLinConsolidatedFinancials;
                        item[StageGateFinancialAnalysisListFields.VolumeTotal1] = sgitem.VolumeTotal1;
                        item[StageGateFinancialAnalysisListFields.VolumeIncremental1] = sgitem.VolumeIncremental1;
                        item[StageGateFinancialAnalysisListFields.GrossSalesTotal1] = sgitem.GrossSalesTotal1;
                        item[StageGateFinancialAnalysisListFields.GrossSalesIncremental1] = sgitem.GrossSalesIncremental1;
                        item[StageGateFinancialAnalysisListFields.TradeRateTotal1] = sgitem.TradeRateTotal1;
                        item[StageGateFinancialAnalysisListFields.TradeRateIncremental1] = sgitem.TradeRateIncremental1;
                        item[StageGateFinancialAnalysisListFields.OGTNTotal1] = sgitem.OGTNTotal1;
                        item[StageGateFinancialAnalysisListFields.OGTNIncremental1] = sgitem.OGTNIncremental1;
                        item[StageGateFinancialAnalysisListFields.NetSalesTotal1] = sgitem.NetSalesTotal1;
                        item[StageGateFinancialAnalysisListFields.NetSalesIncremental1] = sgitem.NetSalesIncremental1;
                        item[StageGateFinancialAnalysisListFields.COGSTotal1] = sgitem.COGSTotal1;
                        item[StageGateFinancialAnalysisListFields.COGSIncremental1] = sgitem.COGSIncremental1;
                        item[StageGateFinancialAnalysisListFields.GrossMarginTotal1] = sgitem.GrossMarginTotal1;
                        item[StageGateFinancialAnalysisListFields.GrossMarginIncremental1] = sgitem.GrossMarginIncremental1;
                        item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal1] = sgitem.GrossMarginPctTotal1;
                        item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental1] = sgitem.GrossMarginPctIncremental1;
                        item[StageGateFinancialAnalysisListFields.VolumeTotal2] = sgitem.VolumeTotal2;
                        item[StageGateFinancialAnalysisListFields.VolumeIncremental2] = sgitem.VolumeIncremental2;
                        item[StageGateFinancialAnalysisListFields.GrossSalesTotal2] = sgitem.GrossSalesTotal2;
                        item[StageGateFinancialAnalysisListFields.GrossSalesIncremental2] = sgitem.GrossSalesIncremental2;
                        item[StageGateFinancialAnalysisListFields.TradeRateTotal2] = sgitem.TradeRateTotal2;
                        item[StageGateFinancialAnalysisListFields.TradeRateIncremental2] = sgitem.TradeRateIncremental2;
                        item[StageGateFinancialAnalysisListFields.OGTNTotal2] = sgitem.OGTNTotal2;
                        item[StageGateFinancialAnalysisListFields.OGTNIncremental2] = sgitem.OGTNIncremental2;
                        item[StageGateFinancialAnalysisListFields.NetSalesTotal2] = sgitem.NetSalesTotal2;
                        item[StageGateFinancialAnalysisListFields.NetSalesIncremental2] = sgitem.NetSalesIncremental2;
                        item[StageGateFinancialAnalysisListFields.COGSTotal2] = sgitem.COGSTotal2;
                        item[StageGateFinancialAnalysisListFields.COGSIncremental2] = sgitem.COGSIncremental2;
                        item[StageGateFinancialAnalysisListFields.GrossMarginTotal2] = sgitem.GrossMarginTotal2;
                        item[StageGateFinancialAnalysisListFields.GrossMarginIncremental2] = sgitem.GrossMarginIncremental2;
                        item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal2] = sgitem.GrossMarginPctTotal2;
                        item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental2] = sgitem.GrossMarginPctIncremental2;
                        item[StageGateFinancialAnalysisListFields.VolumeTotal3] = sgitem.VolumeTotal3;
                        item[StageGateFinancialAnalysisListFields.VolumeIncremental3] = sgitem.VolumeIncremental3;
                        item[StageGateFinancialAnalysisListFields.GrossSalesTotal3] = sgitem.GrossSalesTotal3;
                        item[StageGateFinancialAnalysisListFields.GrossSalesIncremental3] = sgitem.GrossSalesIncremental3;
                        item[StageGateFinancialAnalysisListFields.TradeRateTotal3] = sgitem.TradeRateTotal3;
                        item[StageGateFinancialAnalysisListFields.TradeRateIncremental3] = sgitem.TradeRateIncremental3;
                        item[StageGateFinancialAnalysisListFields.OGTNTotal3] = sgitem.OGTNTotal3;
                        item[StageGateFinancialAnalysisListFields.OGTNIncremental3] = sgitem.OGTNIncremental3;
                        item[StageGateFinancialAnalysisListFields.NetSalesTotal3] = sgitem.NetSalesTotal3;
                        item[StageGateFinancialAnalysisListFields.NetSalesIncremental3] = sgitem.NetSalesIncremental3;
                        item[StageGateFinancialAnalysisListFields.COGSTotal3] = sgitem.COGSTotal3;
                        item[StageGateFinancialAnalysisListFields.COGSIncremental3] = sgitem.COGSIncremental3;
                        item[StageGateFinancialAnalysisListFields.GrossMarginTotal3] = sgitem.GrossMarginTotal3;
                        item[StageGateFinancialAnalysisListFields.GrossMarginIncremental3] = sgitem.GrossMarginIncremental3;
                        item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal3] = sgitem.GrossMarginPctTotal3;
                        item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental3] = sgitem.GrossMarginPctIncremental3;
                        item[StageGateFinancialAnalysisListFields.NSDollerperLB1] = sgitem.NSDollerperLB1;
                        item[StageGateFinancialAnalysisListFields.NSDollerperLB2] = sgitem.NSDollerperLB2;
                        item[StageGateFinancialAnalysisListFields.NSDollerperLB3] = sgitem.NSDollerperLB3;
                        item[StageGateFinancialAnalysisListFields.COGSperLB1] = sgitem.COGSperLB1;
                        item[StageGateFinancialAnalysisListFields.COGSperLB2] = sgitem.COGSperLB2;
                        item[StageGateFinancialAnalysisListFields.COGSperLB3] = sgitem.COGSperLB3;
                        item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt1] = sgitem.TruckldPricePrRtlSllngUt1;
                        item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt2] = sgitem.TruckldPricePrRtlSllngUt2;
                        item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt3] = sgitem.TruckldPricePrRtlSllngUt3;
                        item[StageGateFinancialAnalysisListFields.Assumptions1] = sgitem.Assumptions1;
                        item[StageGateFinancialAnalysisListFields.Assumptions2] = sgitem.Assumptions2;
                        item[StageGateFinancialAnalysisListFields.Assumptions3] = sgitem.Assumptions3;
                        item[StageGateFinancialAnalysisListFields.Deleted] = sgitem.Deleted;

                        item["Modified By"] = SPContext.Current.Web.CurrentUser;

                        if (submitted)
                        {
                            item[StageGateFinancialAnalysisListFields.FormSubmittedDate] = DateTime.Now.ToString();
                            item[StageGateFinancialAnalysisListFields.FormSubmittedBy] = SPContext.Current.Web.CurrentUser;
                        }
                        else
                        {
                            item[StageGateFinancialAnalysisListFields.ModifiedDate] = DateTime.Now.ToString();
                            item[StageGateFinancialAnalysisListFields.ModifiedBy] = SPContext.Current.Web.CurrentUser;
                        }

                        item.Update();

                        sgitem.StageGateProjectListItemId = item.ID;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return sgitem.StageGateProjectListItemId;
        }
        public int UpdateStageGateFinancialAnalysisItem(StageGateFinancialAnalysisItem sgitem, bool submitted)
        {
            //Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateFinancialAnalysisListName);
                        if (sgitem.Id != 0)
                        {
                            var item = spList.GetItemById(sgitem.Id);

                            item["Title"] = sgitem.StageGateProjectListItemId;

                            item[StageGateFinancialAnalysisListFields.StageGateProjectListItemId] = sgitem.StageGateProjectListItemId;
                            item[StageGateFinancialAnalysisListFields.Gate] = sgitem.Gate;
                            item[StageGateFinancialAnalysisListFields.BriefNumber] = sgitem.BriefNumber;
                            item[StageGateFinancialAnalysisListFields.BriefName] = sgitem.BriefName;
                            item[StageGateFinancialAnalysisListFields.AnalysisName] = sgitem.AnalysisName;
                            item[StageGateFinancialAnalysisListFields.FGNumber] = sgitem.FGNumber;
                            item[StageGateFinancialAnalysisListFields.CustomerChannel] = sgitem.CustomerChannel;
                            item[StageGateFinancialAnalysisListFields.BrandSeason] = sgitem.BrandSeason;
                            item[StageGateFinancialAnalysisListFields.ProductForm] = sgitem.ProductForm;
                            item[StageGateFinancialAnalysisListFields.TargetMarginPct] = sgitem.TargetMarginPct;
                            item[StageGateFinancialAnalysisListFields.PLsinProjectBrief] = sgitem.PLsinProjectBrief;
                            item[StageGateFinancialAnalysisListFields.PLinConsolidatedFinancials] = sgitem.PLinConsolidatedFinancials;
                            item[StageGateFinancialAnalysisListFields.VolumeTotal1] = sgitem.VolumeTotal1;
                            item[StageGateFinancialAnalysisListFields.VolumeIncremental1] = sgitem.VolumeIncremental1;
                            item[StageGateFinancialAnalysisListFields.GrossSalesTotal1] = String.Format("{0:0.00}", sgitem.GrossSalesTotal1);
                            item[StageGateFinancialAnalysisListFields.GrossSalesIncremental1] = sgitem.GrossSalesIncremental1;
                            item[StageGateFinancialAnalysisListFields.TradeRateTotal1] = sgitem.TradeRateTotal1;
                            item[StageGateFinancialAnalysisListFields.TradeRateIncremental1] = sgitem.TradeRateIncremental1;
                            item[StageGateFinancialAnalysisListFields.OGTNTotal1] = sgitem.OGTNTotal1;
                            item[StageGateFinancialAnalysisListFields.OGTNIncremental1] = sgitem.OGTNIncremental1;
                            item[StageGateFinancialAnalysisListFields.NetSalesTotal1] = sgitem.NetSalesTotal1;
                            item[StageGateFinancialAnalysisListFields.NetSalesIncremental1] = sgitem.NetSalesIncremental1;
                            item[StageGateFinancialAnalysisListFields.COGSTotal1] = sgitem.COGSTotal1;
                            item[StageGateFinancialAnalysisListFields.COGSIncremental1] = sgitem.COGSIncremental1;
                            item[StageGateFinancialAnalysisListFields.GrossMarginTotal1] = sgitem.GrossMarginTotal1;
                            item[StageGateFinancialAnalysisListFields.GrossMarginIncremental1] = sgitem.GrossMarginIncremental1;
                            item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal1] = sgitem.GrossMarginPctTotal1;
                            item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental1] = sgitem.GrossMarginPctIncremental1;
                            item[StageGateFinancialAnalysisListFields.VolumeTotal2] = sgitem.VolumeTotal2;
                            item[StageGateFinancialAnalysisListFields.VolumeIncremental2] = sgitem.VolumeIncremental2;
                            item[StageGateFinancialAnalysisListFields.GrossSalesTotal2] = sgitem.GrossSalesTotal2;
                            item[StageGateFinancialAnalysisListFields.GrossSalesIncremental2] = sgitem.GrossSalesIncremental2;
                            item[StageGateFinancialAnalysisListFields.TradeRateTotal2] = sgitem.TradeRateTotal2;
                            item[StageGateFinancialAnalysisListFields.TradeRateIncremental2] = sgitem.TradeRateIncremental2;
                            item[StageGateFinancialAnalysisListFields.OGTNTotal2] = sgitem.OGTNTotal2;
                            item[StageGateFinancialAnalysisListFields.OGTNIncremental2] = sgitem.OGTNIncremental2;
                            item[StageGateFinancialAnalysisListFields.NetSalesTotal2] = sgitem.NetSalesTotal2;
                            item[StageGateFinancialAnalysisListFields.NetSalesIncremental2] = sgitem.NetSalesIncremental2;
                            item[StageGateFinancialAnalysisListFields.COGSTotal2] = sgitem.COGSTotal2;
                            item[StageGateFinancialAnalysisListFields.COGSIncremental2] = sgitem.COGSIncremental2;
                            item[StageGateFinancialAnalysisListFields.GrossMarginTotal2] = sgitem.GrossMarginTotal2;
                            item[StageGateFinancialAnalysisListFields.GrossMarginIncremental2] = sgitem.GrossMarginIncremental2;
                            item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal2] = sgitem.GrossMarginPctTotal2;
                            item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental2] = sgitem.GrossMarginPctIncremental2;
                            item[StageGateFinancialAnalysisListFields.VolumeTotal3] = sgitem.VolumeTotal3;
                            item[StageGateFinancialAnalysisListFields.VolumeIncremental3] = sgitem.VolumeIncremental3;
                            item[StageGateFinancialAnalysisListFields.GrossSalesTotal3] = sgitem.GrossSalesTotal3;
                            item[StageGateFinancialAnalysisListFields.GrossSalesIncremental3] = sgitem.GrossSalesIncremental3;
                            item[StageGateFinancialAnalysisListFields.TradeRateTotal3] = sgitem.TradeRateTotal3;
                            item[StageGateFinancialAnalysisListFields.TradeRateIncremental3] = sgitem.TradeRateIncremental3;
                            item[StageGateFinancialAnalysisListFields.OGTNTotal3] = sgitem.OGTNTotal3;
                            item[StageGateFinancialAnalysisListFields.OGTNIncremental3] = sgitem.OGTNIncremental3;
                            item[StageGateFinancialAnalysisListFields.NetSalesTotal3] = sgitem.NetSalesTotal3;
                            item[StageGateFinancialAnalysisListFields.NetSalesIncremental3] = sgitem.NetSalesIncremental3;
                            item[StageGateFinancialAnalysisListFields.COGSTotal3] = sgitem.COGSTotal3;
                            item[StageGateFinancialAnalysisListFields.COGSIncremental3] = sgitem.COGSIncremental3;
                            item[StageGateFinancialAnalysisListFields.GrossMarginTotal3] = sgitem.GrossMarginTotal3;
                            item[StageGateFinancialAnalysisListFields.GrossMarginIncremental3] = sgitem.GrossMarginIncremental3;
                            item[StageGateFinancialAnalysisListFields.GrossMarginPctTotal3] = sgitem.GrossMarginPctTotal3;
                            item[StageGateFinancialAnalysisListFields.GrossMarginPctIncremental3] = sgitem.GrossMarginPctIncremental3;
                            item[StageGateFinancialAnalysisListFields.NSDollerperLB1] = sgitem.NSDollerperLB1;
                            item[StageGateFinancialAnalysisListFields.NSDollerperLB2] = sgitem.NSDollerperLB2;
                            item[StageGateFinancialAnalysisListFields.NSDollerperLB3] = sgitem.NSDollerperLB3;
                            item[StageGateFinancialAnalysisListFields.COGSperLB1] = sgitem.COGSperLB1;
                            item[StageGateFinancialAnalysisListFields.COGSperLB2] = sgitem.COGSperLB2;
                            item[StageGateFinancialAnalysisListFields.COGSperLB3] = sgitem.COGSperLB3;
                            item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt1] = sgitem.TruckldPricePrRtlSllngUt1;
                            item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt2] = sgitem.TruckldPricePrRtlSllngUt2;
                            item[StageGateFinancialAnalysisListFields.TruckldPricePrRtlSllngUt3] = sgitem.TruckldPricePrRtlSllngUt3;
                            item[StageGateFinancialAnalysisListFields.Assumptions1] = sgitem.Assumptions1;
                            item[StageGateFinancialAnalysisListFields.Assumptions2] = sgitem.Assumptions2;
                            item[StageGateFinancialAnalysisListFields.Assumptions3] = sgitem.Assumptions3;
                            item[StageGateFinancialAnalysisListFields.Deleted] = sgitem.Deleted;

                            item["Modified By"] = SPContext.Current.Web.CurrentUser;

                            if (submitted)
                            {
                                item[StageGateFinancialAnalysisListFields.FormSubmittedDate] = DateTime.Now.ToString();
                                item[StageGateFinancialAnalysisListFields.FormSubmittedBy] = SPContext.Current.Web.CurrentUser;
                            }
                            else
                            {
                                item[StageGateFinancialAnalysisListFields.ModifiedDate] = DateTime.Now.ToString();
                                item[StageGateFinancialAnalysisListFields.ModifiedBy] = SPContext.Current.Web.CurrentUser;
                            }

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                            id = sgitem.StageGateProjectListItemId;
                        }
                        else
                        {
                            id = InsertStageGateFinancialAnalysisItem(sgitem, submitted);
                        }
                    }
                }
            });
            return id;
        }
        public int UpdateAllStageGateFinancialAnalysisItems(List<StageGateFinancialAnalysisItem> items, bool submitted)
        {
            foreach (var item in items)
            {
                UpdateStageGateFinancialAnalysisItem(item, submitted);
            }

            return 1;
        }
        public bool DeleteStageGateFinancialAnalysisItem(int StageGateFinancialAnalysisItemId)
        {
            bool isDeleted = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_StageGateFinancialAnalysisListName);

                        SPListItem item = spList.GetItemById(StageGateFinancialAnalysisItemId);
                        if (item != null)
                        {
                            item[StageGateFinancialAnalysisListFields.Deleted] = "Yes";

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item.Update();
                            isDeleted = true;
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isDeleted;
        }

        public ItemProposalItem GetItemProposalItemByFGNumber(int StageGateProjectListItemitemId, string FGNumber)
        {
            var ipfItems = new List<ItemProposalItem>();
            var ipfItem = new ItemProposalItem();

            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<OrderBy><FieldRef Name=\"ID\" /></OrderBy>" +
                                    "<Where>" +
                                        "<And>" +
                                            "<Eq><FieldRef Name=\"" + CompassListFields.SAPItemNumber + "\" /><Value Type=\"Text\">" + FGNumber + "</Value></Eq>" +
                                            "<Eq><FieldRef Name=\"" + CompassListFields.StageGateProjectListItemId + "\" /><Value Type=\"Int\">" + StageGateProjectListItemitemId.ToString() + "</Value></Eq>" +
                                        "</And>" +
                                    "</Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            var sgItem = new ItemProposalItem();
                            sgItem.CompassListItemId = item.ID;
                            sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                            sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                            sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                            sgItem.CustomerSpecific = Convert.ToString(item[CompassListFields.CustomerSpecific]);
                            sgItem.Customer = Convert.ToString(item[CompassListFields.Customer]);
                            sgItem.Channel = Convert.ToString(item[CompassListFields.Channel]);
                            sgItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                            sgItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]);
                            ipfItems.Add(sgItem);
                        }
                        ipfItem = ipfItems[compassItemCol.Count - 1];

                    }
                    else
                    {
                        ipfItem = null;
                    }
                }
            }
            return ipfItem;
        }
        #endregion
    }
}
