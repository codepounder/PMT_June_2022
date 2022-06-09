using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using System.Collections.Specialized;

namespace Ferrara.Compass.Services
{
    public class ProjectTimelineUpdateService : IProjectTimelineUpdateService
    {
        /*
         STOP!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
         IF YOU MODIFY THIS PAGE YOU MUST MAKE THE SAME CHANGE TO PROJECT TIMELINE CALCULATOR!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!          
            */
        public int GetProjectTimelineItem(int compassListId)
        {
            int projectCount = 0;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
                    spQuery.ViewFields = string.Concat(
                                   "<FieldRef Name='" + ProjectTimelineUpdateFields.compassListItemId + "' />");
                    projectCount = spList.GetItems(spQuery).Count;
                }
            }
            return projectCount;
        }
        public List<KeyValuePair<string, string>> GetCompletedItems(int compassListId)
        {
            List<KeyValuePair<string, string>> updatedItems = new List<KeyValuePair<string, string>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    string compCostSeasonal = "";
                    string compCostCorrPaper = "";
                    string compCostFLRP = "";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem appItem = compassItemCol[0];
                        if (appItem != null)
                        {
                            updatedItems.Add(new KeyValuePair<string, string>("IPF", Convert.ToString(appItem[ApprovalListFields.IPF_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("BOMSetupPE2", Convert.ToString(appItem[ApprovalListFields.BOMSetupPE2_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("BOMSetupPE", Convert.ToString(appItem[ApprovalListFields.BOMSetupPE_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("BOMSetupProc", Convert.ToString(appItem[ApprovalListFields.BOMSetupProc_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("BOMSetupPE3", Convert.ToString(appItem[ApprovalListFields.BOMSetupPE3_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("Distribution", Convert.ToString(appItem[ApprovalListFields.Distribution_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("ExternalMfg", Convert.ToString(appItem[ApprovalListFields.ExternalMfg_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("FGPackSpec", Convert.ToString(appItem[ApprovalListFields.FGPackSpec_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("GRAPHICS", Convert.ToString(appItem[ApprovalListFields.GRAPHICS_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("InitialCosting", Convert.ToString(appItem[ApprovalListFields.InitialCosting_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("OBMReview1", Convert.ToString(appItem[ApprovalListFields.OBMReview1_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("OBMReview2", Convert.ToString(appItem[ApprovalListFields.OBMReview2_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("Operations", Convert.ToString(appItem[ApprovalListFields.Operations_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("QA", Convert.ToString(appItem[ApprovalListFields.QA_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SAPBOMSetup", Convert.ToString(appItem[ApprovalListFields.SAPBOMSetup_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SAPInitialSetup", Convert.ToString(appItem[ApprovalListFields.SAPInitialSetup_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("PrelimSAPInitialSetup", Convert.ToString(appItem[ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SrOBMApproval2", Convert.ToString(appItem[ApprovalListFields.SrOBMApproval2_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SrOBMApproval", Convert.ToString(appItem[ApprovalListFields.SrOBMApproval_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("TradePromo", Convert.ToString(appItem[ApprovalListFields.TradePromo_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SAPRoutingSetup", Convert.ToString(appItem[ApprovalListFields.SAPRoutingSetup_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SAPCostingDetails", Convert.ToString(appItem[ApprovalListFields.SAPCostingDetails_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SAPWarehouseInfo", Convert.ToString(appItem[ApprovalListFields.SAPWarehouseInfo_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("StandardCostEntry", Convert.ToString(appItem[ApprovalListFields.StandardCostEntry_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("CostFinishedGood", Convert.ToString(appItem[ApprovalListFields.CostFinishedGood_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("FinalCostingReview", Convert.ToString(appItem[ApprovalListFields.FinalCostingReview_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("PurchasedPO", Convert.ToString(appItem[ApprovalListFields.PurchasedPO_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("RemoveSAPBlocks", Convert.ToString(appItem[ApprovalListFields.RemoveSAPBlocks_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("CustomerPO", Convert.ToString(appItem[ApprovalListFields.CustomerPO_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("MaterialsRcvdChk", Convert.ToString(appItem[ApprovalListFields.MaterialsReceivedChk_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("FirstProductionChk", Convert.ToString(appItem[ApprovalListFields.FirstProductionChk_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("DistributionChk", Convert.ToString(appItem[ApprovalListFields.DistributionCenterChk_SubmittedDate])));


                            updatedItems.Add(new KeyValuePair<string, string>("MatrlWHSetUp", Convert.ToString(appItem[ApprovalListFields.MaterialsReceivedChk_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("SAPCompleteItem", Convert.ToString(appItem[ApprovalListFields.SAPCompleteItemSetup_SubmittedDate])));

                            compCostSeasonal = Convert.ToString(appItem[ApprovalListFields.CompCostSeasonal_StartDate]);
                            compCostCorrPaper = Convert.ToString(appItem[ApprovalListFields.CompCostCorrPaper_StartDate]);
                            compCostFLRP = Convert.ToString(appItem[ApprovalListFields.CompCostFLRP_StartDate]);
                        }
                    }


                    SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);

                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
                    spQuery2.RowLimit = 1;

                    SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery);
                    if (compassItemCol2.Count > 0)
                    {
                        SPListItem appItem2 = compassItemCol2[0];
                        if (appItem2 != null)
                        {
                            updatedItems.Add(new KeyValuePair<string, string>("BEQRC", Convert.ToString(appItem2[ApprovalListFields.BEQRC_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("EstPricing", Convert.ToString(appItem2[ApprovalListFields.EstPricing_SubmittedDate])));
                            updatedItems.Add(new KeyValuePair<string, string>("EstBracketPricing", Convert.ToString(appItem2[ApprovalListFields.EstBracketPricing_SubmittedDate])));
                        }

                    }
                }
            }
            return updatedItems;
        }
        public void UpdateProjectTimelineItem(ProjectTimelineItem projectTimelineItem, string title)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + projectTimelineItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem appItem;
                        if (compassItemCol.Count > 0)
                        {
                            appItem = compassItemCol[0];
                        }
                        else
                        {
                            appItem = spList.AddItem();
                            appItem[ProjectTimelineUpdateFields.compassListItemId] = projectTimelineItem.CompassListItemId;
                            appItem[ProjectTimelineUpdateFields.Title] = "title";

                        }
                        if (appItem != null)
                        {
                            appItem[ProjectTimelineUpdateFields.IPF] = 0;
                            appItem[ProjectTimelineUpdateFields.SrOBMApproval] = projectTimelineItem.SrOBMApproval;
                            appItem[ProjectTimelineUpdateFields.SrOBMApproval2] = projectTimelineItem.SrOBMApproval2;
                            appItem[ProjectTimelineUpdateFields.TradePromo] = projectTimelineItem.TradePromo;
                            appItem[ProjectTimelineUpdateFields.EstPricing] = projectTimelineItem.EstPricing;
                            appItem[ProjectTimelineUpdateFields.EstBracketPricing] = projectTimelineItem.EstBracketPricing;
                            appItem[ProjectTimelineUpdateFields.Distribution] = projectTimelineItem.Distribution;
                            appItem[ProjectTimelineUpdateFields.Operations] = projectTimelineItem.Operations;
                            appItem[ProjectTimelineUpdateFields.SAPInitialSetup] = projectTimelineItem.SAPInitialSetup;
                            appItem[ProjectTimelineUpdateFields.PrelimSAPInitialSetup] = projectTimelineItem.PrelimSAPInitialSetup;
                            appItem[ProjectTimelineUpdateFields.QA] = projectTimelineItem.QA;
                            appItem[ProjectTimelineUpdateFields.OBMReview1] = projectTimelineItem.OBMReview1;
                            appItem[ProjectTimelineUpdateFields.BOMSetupPE] = projectTimelineItem.BOMSetupPE;
                            appItem[ProjectTimelineUpdateFields.BOMSetupProc] = projectTimelineItem.BOMSetupProc;
                            appItem[ProjectTimelineUpdateFields.BOMSetupPE2] = projectTimelineItem.BOMSetupPE2;
                            appItem[ProjectTimelineUpdateFields.BOMSetupPE3] = projectTimelineItem.BOMSetupPE3;
                            appItem[ProjectTimelineUpdateFields.OBMReview2] = projectTimelineItem.OBMReview2;
                            appItem[ProjectTimelineUpdateFields.GRAPHICS] = projectTimelineItem.GRAPHICS;
                            appItem[ProjectTimelineUpdateFields.CostingQuote] = projectTimelineItem.CostingQuote;
                            appItem[ProjectTimelineUpdateFields.FGPackSpec] = projectTimelineItem.FGPackSpec;
                            appItem[ProjectTimelineUpdateFields.SAPBOMSetup] = projectTimelineItem.SAPBOMSetup;
                            appItem[ProjectTimelineUpdateFields.ExternalMfg] = projectTimelineItem.ExternalMfg;
                            appItem[ProjectTimelineUpdateFields.SAPRoutingSetup] = projectTimelineItem.SAPRoutingSetup;
                            appItem[ProjectTimelineUpdateFields.BOMActiveDate] = projectTimelineItem.BOMActiveDate;
                            appItem[ProjectTimelineUpdateFields.SAPCostingDetails] = projectTimelineItem.SAPCostingDetails;
                            appItem[ProjectTimelineUpdateFields.SAPWarehouseInfo] = projectTimelineItem.SAPWarehouseInfo;
                            appItem[ProjectTimelineUpdateFields.StandardCostEntry] = projectTimelineItem.StandardCostEntry;
                            appItem[ProjectTimelineUpdateFields.CostFinishedGood] = projectTimelineItem.CostFinishedGood;
                            appItem[ProjectTimelineUpdateFields.FinalCostingReview] = projectTimelineItem.FinalCostingReview;
                            appItem[ProjectTimelineUpdateFields.PurchasedPO] = projectTimelineItem.PurchasedPO;
                            appItem[ProjectTimelineUpdateFields.RemoveSAPBlocks] = projectTimelineItem.RemoveSAPBlocks;
                            appItem[ProjectTimelineUpdateFields.CustomerPO] = projectTimelineItem.CustomerPO;
                            appItem[ProjectTimelineUpdateFields.MaterialsRcvdChk] = projectTimelineItem.MaterialsRcvdChk;
                            appItem[ProjectTimelineUpdateFields.FirstProductionChk] = projectTimelineItem.FirstProductionChk;
                            appItem[ProjectTimelineUpdateFields.DistributionChk] = projectTimelineItem.DistributionChk;
                            appItem[ProjectTimelineUpdateFields.FCST] = projectTimelineItem.FCST;
                            appItem[ProjectTimelineUpdateFields.MaterialWarehouseSetUp] = projectTimelineItem.MaterialWarehouseSetUp;
                            appItem[ProjectTimelineUpdateFields.SAPCompleteItemSetup] = projectTimelineItem.SAPCompleteItemSetup;
                            appItem[ProjectTimelineUpdateFields.BEQRC] = projectTimelineItem.BEQRC;

                            appItem[ProjectTimelineUpdateFields.IPF_Planned] = 0;
                            appItem[ProjectTimelineUpdateFields.SrOBMApproval_Planned] = projectTimelineItem.SrOBMApproval;
                            appItem[ProjectTimelineUpdateFields.SrOBMApproval2_Planned] = projectTimelineItem.SrOBMApproval2;
                            appItem[ProjectTimelineUpdateFields.TradePromo_Planned] = projectTimelineItem.TradePromo;
                            appItem[ProjectTimelineUpdateFields.EstPricing_Planned] = projectTimelineItem.EstPricing;
                            appItem[ProjectTimelineUpdateFields.EstBracketPricing_Planned] = projectTimelineItem.EstBracketPricing;
                            appItem[ProjectTimelineUpdateFields.Distribution_Planned] = projectTimelineItem.Distribution;
                            appItem[ProjectTimelineUpdateFields.Operations_Planned] = projectTimelineItem.Operations;
                            appItem[ProjectTimelineUpdateFields.SAPInitialSetup_Planned] = projectTimelineItem.SAPInitialSetup;
                            appItem[ProjectTimelineUpdateFields.PrelimSAPInitialSetup_Planned] = projectTimelineItem.PrelimSAPInitialSetup;
                            appItem[ProjectTimelineUpdateFields.QA_Planned] = projectTimelineItem.QA;
                            appItem[ProjectTimelineUpdateFields.OBMReview1_Planned] = projectTimelineItem.OBMReview1;
                            appItem[ProjectTimelineUpdateFields.BOMSetupPE_Planned] = projectTimelineItem.BOMSetupPE;
                            appItem[ProjectTimelineUpdateFields.BOMSetupProc_Planned] = projectTimelineItem.BOMSetupProc;
                            appItem[ProjectTimelineUpdateFields.BOMSetupPE2_Planned] = projectTimelineItem.BOMSetupPE2;
                            appItem[ProjectTimelineUpdateFields.BOMSetupPE3_Planned] = projectTimelineItem.BOMSetupPE3;
                            appItem[ProjectTimelineUpdateFields.OBMReview2_Planned] = projectTimelineItem.OBMReview2;
                            appItem[ProjectTimelineUpdateFields.GRAPHICS_Planned] = projectTimelineItem.GRAPHICS;
                            appItem[ProjectTimelineUpdateFields.SAPBOMSetup_Planned] = projectTimelineItem.SAPBOMSetup;
                            appItem[ProjectTimelineUpdateFields.ExternalMfg_Planned] = projectTimelineItem.ExternalMfg;
                            appItem[ProjectTimelineUpdateFields.SAPRoutingSetup_Planned] = projectTimelineItem.SAPRoutingSetup;
                            appItem[ProjectTimelineUpdateFields.MaterialWarehouseSetUp_Planned] = projectTimelineItem.MaterialWarehouseSetUp;
                            appItem[ProjectTimelineUpdateFields.SAPCompleteItemSetup_Planned] = projectTimelineItem.SAPCompleteItemSetup;
                            appItem[ProjectTimelineUpdateFields.BEQRC_Planned] = projectTimelineItem.BEQRC;

                            // Set Modified By to current user NOT System Account
                            appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                            appItem.Update();

                        }
                        spWeb.AllowUnsafeUpdates = false;

                    }
                }
            });
        }

        public int InsertProjectTimelineItem(ProjectTimelineItem projectTimelineItem, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + projectTimelineItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                            return;

                        SPListItem appItem = spList.AddItem();

                        appItem["Title"] = title;
                        appItem[ProjectTimelineUpdateFields.compassListItemId] = projectTimelineItem.CompassListItemId;
                        appItem[ProjectTimelineUpdateFields.IPF] = 0;
                        appItem[ProjectTimelineUpdateFields.SrOBMApproval] = projectTimelineItem.SrOBMApproval;
                        appItem[ProjectTimelineUpdateFields.SrOBMApproval2] = projectTimelineItem.SrOBMApproval2;
                        appItem[ProjectTimelineUpdateFields.TradePromo] = projectTimelineItem.TradePromo;
                        appItem[ProjectTimelineUpdateFields.EstPricing] = projectTimelineItem.EstPricing;
                        appItem[ProjectTimelineUpdateFields.EstBracketPricing] = projectTimelineItem.EstBracketPricing;
                        appItem[ProjectTimelineUpdateFields.Distribution] = projectTimelineItem.Distribution;
                        appItem[ProjectTimelineUpdateFields.Operations] = projectTimelineItem.Operations;
                        appItem[ProjectTimelineUpdateFields.SAPInitialSetup] = projectTimelineItem.SAPInitialSetup;
                        appItem[ProjectTimelineUpdateFields.PrelimSAPInitialSetup] = projectTimelineItem.PrelimSAPInitialSetup;
                        appItem[ProjectTimelineUpdateFields.QA] = projectTimelineItem.QA;
                        appItem[ProjectTimelineUpdateFields.OBMReview1] = projectTimelineItem.OBMReview1;
                        appItem[ProjectTimelineUpdateFields.BOMSetupPE] = projectTimelineItem.BOMSetupPE;
                        appItem[ProjectTimelineUpdateFields.BOMSetupProc] = projectTimelineItem.BOMSetupProc;
                        appItem[ProjectTimelineUpdateFields.BOMSetupPE2] = projectTimelineItem.BOMSetupPE2;
                        appItem[ProjectTimelineUpdateFields.BOMSetupPE3] = projectTimelineItem.BOMSetupPE3;
                        appItem[ProjectTimelineUpdateFields.OBMReview2] = projectTimelineItem.OBMReview2;
                        appItem[ProjectTimelineUpdateFields.GRAPHICS] = projectTimelineItem.GRAPHICS;
                        appItem[ProjectTimelineUpdateFields.CostingQuote] = projectTimelineItem.CostingQuote;
                        appItem[ProjectTimelineUpdateFields.FGPackSpec] = projectTimelineItem.FGPackSpec;
                        appItem[ProjectTimelineUpdateFields.SAPBOMSetup] = projectTimelineItem.SAPBOMSetup;
                        appItem[ProjectTimelineUpdateFields.FCST] = projectTimelineItem.FCST;
                        appItem[ProjectTimelineUpdateFields.ExternalMfg] = projectTimelineItem.ExternalMfg;
                        appItem[ProjectTimelineUpdateFields.SAPRoutingSetup] = projectTimelineItem.SAPRoutingSetup;
                        appItem[ProjectTimelineUpdateFields.BOMActiveDate] = projectTimelineItem.BOMActiveDate;
                        appItem[ProjectTimelineUpdateFields.SAPCostingDetails] = projectTimelineItem.SAPCostingDetails;
                        appItem[ProjectTimelineUpdateFields.SAPWarehouseInfo] = projectTimelineItem.SAPWarehouseInfo;
                        appItem[ProjectTimelineUpdateFields.StandardCostEntry] = projectTimelineItem.StandardCostEntry;
                        appItem[ProjectTimelineUpdateFields.CostFinishedGood] = projectTimelineItem.CostFinishedGood;
                        appItem[ProjectTimelineUpdateFields.FinalCostingReview] = projectTimelineItem.FinalCostingReview;
                        appItem[ProjectTimelineUpdateFields.PurchasedPO] = projectTimelineItem.PurchasedPO;
                        appItem[ProjectTimelineUpdateFields.RemoveSAPBlocks] = projectTimelineItem.RemoveSAPBlocks;
                        appItem[ProjectTimelineUpdateFields.CustomerPO] = projectTimelineItem.CustomerPO;
                        appItem[ProjectTimelineUpdateFields.MaterialsRcvdChk] = projectTimelineItem.MaterialsRcvdChk;
                        appItem[ProjectTimelineUpdateFields.FirstProductionChk] = projectTimelineItem.FirstProductionChk;
                        appItem[ProjectTimelineUpdateFields.DistributionChk] = projectTimelineItem.DistributionChk;
                        appItem[ProjectTimelineUpdateFields.MaterialWarehouseSetUp] = projectTimelineItem.MaterialWarehouseSetUp;
                        appItem[ProjectTimelineUpdateFields.SAPCompleteItemSetup] = projectTimelineItem.SAPCompleteItemSetup;
                        appItem[ProjectTimelineUpdateFields.BEQRC] = projectTimelineItem.BEQRC;

                        appItem[ProjectTimelineUpdateFields.IPF_Planned] = 0;
                        appItem[ProjectTimelineUpdateFields.SrOBMApproval_Planned] = projectTimelineItem.SrOBMApproval;
                        appItem[ProjectTimelineUpdateFields.SrOBMApproval2_Planned] = projectTimelineItem.SrOBMApproval2;
                        appItem[ProjectTimelineUpdateFields.TradePromo_Planned] = projectTimelineItem.TradePromo;
                        appItem[ProjectTimelineUpdateFields.EstPricing_Planned] = projectTimelineItem.EstPricing;
                        appItem[ProjectTimelineUpdateFields.EstBracketPricing_Planned] = projectTimelineItem.EstBracketPricing;
                        appItem[ProjectTimelineUpdateFields.Distribution_Planned] = projectTimelineItem.Distribution;
                        appItem[ProjectTimelineUpdateFields.Operations_Planned] = projectTimelineItem.Operations;
                        appItem[ProjectTimelineUpdateFields.SAPInitialSetup_Planned] = projectTimelineItem.SAPInitialSetup;
                        appItem[ProjectTimelineUpdateFields.PrelimSAPInitialSetup_Planned] = projectTimelineItem.PrelimSAPInitialSetup;
                        appItem[ProjectTimelineUpdateFields.QA_Planned] = projectTimelineItem.QA;
                        appItem[ProjectTimelineUpdateFields.OBMReview1_Planned] = projectTimelineItem.OBMReview1;
                        appItem[ProjectTimelineUpdateFields.BOMSetupPE_Planned] = projectTimelineItem.BOMSetupPE;
                        appItem[ProjectTimelineUpdateFields.BOMSetupProc_Planned] = projectTimelineItem.BOMSetupProc;
                        appItem[ProjectTimelineUpdateFields.BOMSetupPE2_Planned] = projectTimelineItem.BOMSetupPE2;
                        appItem[ProjectTimelineUpdateFields.BOMSetupPE3_Planned] = projectTimelineItem.BOMSetupPE3;
                        appItem[ProjectTimelineUpdateFields.OBMReview2_Planned] = projectTimelineItem.OBMReview2;
                        appItem[ProjectTimelineUpdateFields.GRAPHICS_Planned] = projectTimelineItem.GRAPHICS;
                        appItem[ProjectTimelineUpdateFields.SAPBOMSetup_Planned] = projectTimelineItem.SAPBOMSetup;
                        appItem[ProjectTimelineUpdateFields.ExternalMfg_Planned] = projectTimelineItem.ExternalMfg;
                        appItem[ProjectTimelineUpdateFields.SAPRoutingSetup_Planned] = projectTimelineItem.SAPRoutingSetup;
                        appItem[ProjectTimelineUpdateFields.MaterialWarehouseSetUp_Planned] = projectTimelineItem.MaterialWarehouseSetUp;
                        appItem[ProjectTimelineUpdateFields.SAPCompleteItemSetup_Planned] = projectTimelineItem.SAPCompleteItemSetup;
                        appItem[ProjectTimelineUpdateFields.BEQRC_Planned] = projectTimelineItem.BEQRC;

                        appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                        appItem.Update();
                        spWeb.AllowUnsafeUpdates = false;

                        id = appItem.ID;
                    }
                }
            });
            return id;
        }
        public List<List<string>> GetProjectItem(int compassListId)
        {
            List<List<string>> updatedTimes = new List<List<string>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem workflowItems in compassItemCol)
                        {
                            updatedTimes.Add(new List<string>() { "IPF", Convert.ToString(workflowItems["IPF"]) });
                            updatedTimes.Add(new List<string>() { "BOMSetupPE3", Convert.ToString(workflowItems["BOMSetupPE3"]) });
                            updatedTimes.Add(new List<string>() { "BOMSetupPE2", Convert.ToString(workflowItems["BOMSetupPE2"]) });
                            updatedTimes.Add(new List<string>() { "BOMSetupPE", Convert.ToString(workflowItems["BOMSetupPE"]) });
                            updatedTimes.Add(new List<string>() { "BOMSetupProc", Convert.ToString(workflowItems["BOMSetupProc"]) });
                            updatedTimes.Add(new List<string>() { "CostingQuote", Convert.ToString(workflowItems["CostingQuote"]) });
                            updatedTimes.Add(new List<string>() { "Distribution", Convert.ToString(workflowItems["Distribution"]) });
                            updatedTimes.Add(new List<string>() { "ExternalMfg", Convert.ToString(workflowItems["ExternalMfg"]) });
                            updatedTimes.Add(new List<string>() { "FGPackSpec", Convert.ToString(workflowItems["FGPackSpec"]) });
                            try
                            {
                                updatedTimes.Add(new List<string>() { "GRAPHICS", Convert.ToString(workflowItems["Graphics"]) });
                            }
                            catch (Exception e)
                            {
                                updatedTimes.Add(new List<string>() { "GRAPHICS", Convert.ToString(workflowItems["GRAPHICS"]) });
                            }
                            updatedTimes.Add(new List<string>() { "InitialCapacity", Convert.ToString(workflowItems["InitialCapacity"]) });
                            updatedTimes.Add(new List<string>() { "InitialCosting", Convert.ToString(workflowItems["InitialCosting"]) });
                            updatedTimes.Add(new List<string>() { "OBMReview1", Convert.ToString(workflowItems["OBMReview1"]) });
                            updatedTimes.Add(new List<string>() { "OBMReview2", Convert.ToString(workflowItems["OBMReview2"]) });
                            updatedTimes.Add(new List<string>() { "Operations", Convert.ToString(workflowItems["Operations"]) });
                            updatedTimes.Add(new List<string>() { "QA", Convert.ToString(workflowItems["QA"]) });
                            updatedTimes.Add(new List<string>() { "SAPBOMSetup", Convert.ToString(workflowItems["SAPBOMSetup"]) });
                            updatedTimes.Add(new List<string>() { "SAPInitialSetup", Convert.ToString(workflowItems["SAPInitialSetup"]) });
                            updatedTimes.Add(new List<string>() { "PrelimSAPInitialSetup", Convert.ToString(workflowItems["PrelimSAPInitialSetup"]) });
                            updatedTimes.Add(new List<string>() { "SrOBMApproval2", Convert.ToString(workflowItems["SrOBMApproval2"]) });
                            updatedTimes.Add(new List<string>() { "SrOBMApproval", Convert.ToString(workflowItems["SrOBMApproval"]) });
                            updatedTimes.Add(new List<string>() { "TradePromo", Convert.ToString(workflowItems["TradePromo"]) });
                            updatedTimes.Add(new List<string>() { "EstPricing", Convert.ToString(workflowItems["EstPricing"]) });
                            updatedTimes.Add(new List<string>() { "EstBracketPricing", Convert.ToString(workflowItems["EstBracketPricing"]) });
                            updatedTimes.Add(new List<string>() { "SAPRoutingSetup", Convert.ToString(workflowItems["SAPRoutingSetup"]) });
                            updatedTimes.Add(new List<string>() { "BOMActiveDate", Convert.ToString(workflowItems["BOMActiveDate"]) });
                            updatedTimes.Add(new List<string>() { "SAPCostingDetails", Convert.ToString(workflowItems["SAPCostingDetails"]) });
                            updatedTimes.Add(new List<string>() { "SAPWarehouseInfo", Convert.ToString(workflowItems["SAPWarehouseInfo"]) });
                            updatedTimes.Add(new List<string>() { "StandardCostEntry", Convert.ToString(workflowItems["StandardCostEntry"]) });
                            updatedTimes.Add(new List<string>() { "CostFinishedGood", Convert.ToString(workflowItems["CostFinishedGood"]) });
                            updatedTimes.Add(new List<string>() { "FinalCostingReview", Convert.ToString(workflowItems["FinalCostingReview"]) });
                            updatedTimes.Add(new List<string>() { "PurchasedPO", Convert.ToString(workflowItems["PurchasedPO"]) });
                            updatedTimes.Add(new List<string>() { "RemoveSAPBlocks", Convert.ToString(workflowItems["RemoveSAPBlocks"]) });
                            updatedTimes.Add(new List<string>() { "CustomerPO", Convert.ToString(workflowItems["CustomerPO"]) });
                            updatedTimes.Add(new List<string>() { "MaterialsRcvdChk", Convert.ToString(workflowItems["MaterialsRcvdChk"]) });
                            updatedTimes.Add(new List<string>() { "FirstProductionChk", Convert.ToString(workflowItems["FirstProductionChk"]) });
                            updatedTimes.Add(new List<string>() { "DistributionChk", Convert.ToString(workflowItems["DistributionChk"]) });
                            updatedTimes.Add(new List<string>() { "FCST", Convert.ToString(workflowItems["FCST"]) });

                            updatedTimes.Add(new List<string>() { "MatrlWHSetUp", Convert.ToString(workflowItems["MatrlWHSetUp"]) });
                            updatedTimes.Add(new List<string>() { "SAPCompleteItem", Convert.ToString(workflowItems["SAPCompleteItem"]) });
                            updatedTimes.Add(new List<string>() { "BEQRC", Convert.ToString(workflowItems["BEQRC"]) });

                        }
                    }
                }
            }
            return updatedTimes;
        }
        public int GetSingleProjectItem(int compassListId, string task)
        {
            int updatedTimes = 0;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassListId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem workflowItems in compassItemCol)
                        {
                            try
                            {
                                updatedTimes = Convert.ToInt32(workflowItems[task]);
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }
                }
            }
            return updatedTimes;
        }
        public List<Tuple<int, string, int>> GetProjectItems(List<WorkflowTaskDetailsItem> taskItems)
        {
            var updatedTimes = new List<Tuple<int, string, int>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);

                    SPQuery spQuery = new SPQuery();
                    string query = "<Where><In><FieldRef Name=\"compassListItemId\" LookupId=\"True\" /><Values>";
                    foreach (var taskItem in taskItems)
                    {
                        query += "<Value Type=\"Integer\">" + taskItem.CompassId + "</Value>";
                    }
                    query += "</Values></In></Where>";
                    spQuery.Query = query;
                    SPListItemCollection compassItemCol = spList.GetItems(query);
                    foreach (SPListItem workflowItems in compassItemCol)
                    {
                        try
                        {
                            var CompassListItemId = Convert.ToInt32(workflowItems["CompassListItemId"]);
                            var TaskName = (from taskItem in taskItems where taskItem.CompassId == CompassListItemId select taskItem.WorkflowStep).FirstOrDefault();
                            var updatedTime = Convert.ToInt32(workflowItems[TaskName]);
                            updatedTimes.Add(new Tuple<int, string, int>(CompassListItemId, TaskName, updatedTime));
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }
            return updatedTimes;
        }

        public void UpdateSingleProjectTimelineItem(String column, string value, int compassId, string timelineType, string ProjectNo)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem appItem;
                        if (compassItemCol.Count > 0)
                        {
                            appItem = compassItemCol[0];


                        }
                        else
                        {
                            appItem = spList.AddItem();
                            appItem["Title"] = ProjectNo;
                            appItem[ProjectTimelineUpdateFields.compassListItemId] = compassId;
                        }
                        if (appItem != null)
                        {
                            try
                            {
                                appItem[column] = value;
                                appItem["Editor"] = SPContext.Current.Web.CurrentUser;
                                appItem.Update();
                            }
                            catch (Exception e)
                            {

                            }

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public double getEstProjectTotalDays(int compassId, string timelineType)
        {
            double projecttotaldays = 0;
            if (timelineType == "" || timelineType == null)
            {
                timelineType = "Standard";
            }
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList updateList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineUpdateName);
                        SPQuery updateQuery = new SPQuery();
                        updateQuery.Query = "<Where><Eq><FieldRef Name=\"compassListItemId\" /><Value Type=\"Int\">" + compassId.ToString() + "</Value></Eq></Where>";
                        updateQuery.RowLimit = 1;
                        SPListItemCollection updateCol = updateList.GetItems(updateQuery);
                        int updateListCount = updateCol.Count;
                        List<int> loopCount = new List<int>();
                        SPView updateView = updateList.DefaultView;
                        // try {
                        StringCollection updateFields = updateView.ViewFields.ToStringCollection();
                        List<KeyValuePair<string, double>> updateListValues = new List<KeyValuePair<string, double>>();
                        if (updateListCount > 0)
                        {
                            SPListItem updateItem = updateCol[0];
                            if (updateItem != null)
                            {
                                foreach (string fieldName in updateFields)
                                {
                                    if (fieldName != "LinkTitle")
                                    {
                                        updateListValues.Add(new KeyValuePair<string, double>(fieldName, Convert.ToDouble(updateItem[fieldName])));
                                    }
                                }
                            }
                        }
                        SPList timelineList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectTimelineTypeListName);
                        SPQuery timelineQuery = new SPQuery();
                        timelineQuery.Query = "<Where><Neq><FieldRef Name=\"WorkflowOrder\"></FieldRef><Value Type=\"Int\">0</Value></Neq></Where>";
                        SPListItemCollection timelineCol = timelineList.GetItems(timelineQuery);
                        if (timelineCol.Count > 0)
                        {
                            foreach (SPListItem timelineItem in timelineCol)
                            {
                                if (timelineItem != null)
                                {
                                    string quickname = Convert.ToString(timelineItem[ProjectTimelineTypeDays.WorkflowQuickStep]);
                                    double taskTotalDays = Convert.ToDouble(timelineItem[timelineType]);
                                    if (updateListCount > 0)
                                    {
                                        taskTotalDays = (from workflow in updateListValues where workflow.Key == quickname select workflow.Value).FirstOrDefault();
                                    }

                                    string exception = "";
                                    if (timelineItem[ProjectTimelineTypeDays.WorkflowStacks] != null)
                                    {
                                        exception = timelineItem[ProjectTimelineTypeDays.WorkflowStacks].ToString();
                                    }
                                    //List<KeyValuePair<string, int>> exceptionList = new List<KeyValuePair<string, int>>();
                                    int numberException;
                                    Boolean isException = int.TryParse(exception, out numberException);
                                    List<double> exceptionList = new List<double>();
                                    if (!isException)
                                    {
                                        projecttotaldays = projecttotaldays + taskTotalDays;
                                    }
                                    else
                                    {

                                        int currentException = 0;

                                        foreach (SPListItem timelineItemException in timelineCol)
                                        {
                                            int defCurrException;
                                            string stringException = Convert.ToString(timelineItemException[ProjectTimelineTypeDays.WorkflowStacks]);
                                            Boolean isCurrException = int.TryParse(stringException, out defCurrException);
                                            if (isCurrException)
                                            {
                                                if (defCurrException == numberException)
                                                {
                                                    currentException = defCurrException;
                                                    if (updateListCount > 0)
                                                    {
                                                        string currentVal = Convert.ToString(timelineItemException[ProjectTimelineTypeDays.WorkflowQuickStep]);
                                                        var updatedDaysValue = (from workflow in updateListValues where workflow.Key == currentVal select workflow.Value).FirstOrDefault();
                                                        exceptionList.Add(Convert.ToDouble(updatedDaysValue));
                                                    }
                                                    else
                                                    {
                                                        exceptionList.Add(Convert.ToDouble(timelineItemException[timelineType]));
                                                    }
                                                }
                                            }
                                        }

                                        if (!loopCount.Contains(currentException))
                                        {
                                            loopCount.Add(currentException);
                                            projecttotaldays = projecttotaldays + exceptionList.Max();
                                        }

                                    }

                                }
                            }
                        }
                        //}
                        //catch(Exception e)
                        //{

                        //}
                    }
                }
            });
            return projecttotaldays;
        }
    }
}

