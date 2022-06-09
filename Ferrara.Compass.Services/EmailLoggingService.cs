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
    public class EmailLoggingService : IEmailLoggingService
    {
        public EmailLoggingListItem GetEmailLoggingItem(int itemId)
        {
            EmailLoggingListItem emailLoggingItem = new EmailLoggingListItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            emailLoggingItem.EmailLoggingListItemId = item.ID;
                            emailLoggingItem.CompassListItemId = Convert.ToInt32(item[EmailLoggingListFields.CompassListItemId]);

                            //emailLoggingItem.CMF_CM_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.CMF_CM_EmailDate]);
                            //emailLoggingItem.CMF_CM_EmailTo = Convert.ToString(item[EmailLoggingListFields.CMF_CM_EmailTo]);

                            //emailLoggingItem.COMAN_COMAN_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.COMAN_COMAN_EmailDate]);
                            //emailLoggingItem.COMAN_COMAN_EmailTo = Convert.ToString(item[EmailLoggingListFields.COMAN_COMAN_EmailTo]);

                            ////emailLoggingItem.ICF_CC_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.ICF_CC_EmailDate]);
                            ////emailLoggingItem.ICF_CC_EmailTo = Convert.ToString(item[EmailLoggingListFields.ICF_CC_EmailTo]);

                            ////emailLoggingItem.ICF_CST_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.ICF_CST_EmailDate]);
                            ////emailLoggingItem.ICF_CST_EmailTo = Convert.ToString(item[EmailLoggingListFields.ICF_CST_EmailTo]);

                            ////emailLoggingItem.ICF_SLT_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.ICF_SLT_EmailDate]);
                            ////emailLoggingItem.ICF_SLT_EmailTo = Convert.ToString(item[EmailLoggingListFields.ICF_SLT_EmailTo]);

                            //emailLoggingItem.MN_PE_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.MN_PE_EmailDate]);
                            //emailLoggingItem.MN_PE_EmailTo = Convert.ToString(item[EmailLoggingListFields.MN_PE_EmailTo]);

                            //emailLoggingItem.MN_PE2_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.MN_PE2_EmailDate]);
                            //emailLoggingItem.MN_PE2_EmailTo = Convert.ToString(item[EmailLoggingListFields.MN_PE2_EmailTo]);

                            //emailLoggingItem.MN_PUR_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.MN_PUR_EmailDate]);
                            //emailLoggingItem.MN_PUR_EmailTo = Convert.ToString(item[EmailLoggingListFields.MN_PUR_EmailTo]);

                            //emailLoggingItem.MN_SAP_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.MN_SAP_EmailDate]);
                            //emailLoggingItem.MN_SAP_EmailTo = Convert.ToString(item[EmailLoggingListFields.MN_SAP_EmailTo]);

                            //emailLoggingItem.OPS_DIST_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.OPS_DIST_EmailDate]);
                            //emailLoggingItem.OPS_DIST_EmailTo = Convert.ToString(item[EmailLoggingListFields.OPS_DIST_EmailTo]);

                            //emailLoggingItem.OPS_ICR_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.OPS_ICR_EmailDate]);
                            //emailLoggingItem.OPS_ICR_EmailTo = Convert.ToString(item[EmailLoggingListFields.OPS_ICR_EmailTo]);

                            //emailLoggingItem.OPS_MFG_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.OPS_MFG_EmailDate]);
                            //emailLoggingItem.OPS_MFG_EmailTo = Convert.ToString(item[EmailLoggingListFields.OPS_MFG_EmailTo]);

                            //emailLoggingItem.OPS_RT_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.OPS_RT_EmailDate]);
                            //emailLoggingItem.OPS_RT_EmailTo = Convert.ToString(item[EmailLoggingListFields.OPS_RT_EmailTo]);

                            //emailLoggingItem.QA_QA_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.QA_QA_EmailDate]);
                            //emailLoggingItem.QA_QA_EmailTo = Convert.ToString(item[EmailLoggingListFields.QA_QA_EmailTo]);

                            //emailLoggingItem.RGF_GR_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.RGF_GR_EmailDate]);
                            //emailLoggingItem.RGF_GR_EmailTo = Convert.ToString(item[EmailLoggingListFields.RGF_GR_EmailTo]);

                            //emailLoggingItem.RGF_OBM_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.RGF_OBM_EmailDate]);
                            //emailLoggingItem.RGF_OBM_EmailTo = Convert.ToString(item[EmailLoggingListFields.RGF_OBM_EmailTo]);

                            //emailLoggingItem.SIR_SAP_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.SIR_SAP_EmailDate]);
                            //emailLoggingItem.SIR_SAP_EmailTo = Convert.ToString(item[EmailLoggingListFields.SIR_SAP_EmailTo]);

                            //emailLoggingItem.SIS_SAP_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.SIS_SAP_EmailDate]);
                            //emailLoggingItem.SIS_SAP_EmailTo = Convert.ToString(item[EmailLoggingListFields.SIS_SAP_EmailTo]);

                            //emailLoggingItem.TBD_SAP_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.TBD_SAP_EmailDate]);
                            //emailLoggingItem.TBD_SAP_EmailTo = Convert.ToString(item[EmailLoggingListFields.TBD_SAP_EmailTo]);

                            //emailLoggingItem.OBMREV1_OBM_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.OBMREV1_OBM_EmailDate]);
                            //emailLoggingItem.OBMREV1_OBM_EmailTo = Convert.ToString(item[EmailLoggingListFields.OBMREV1_OBM_EmailTo]);

                            //emailLoggingItem.GPP_GR_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.GPP_GR_EmailDate]);
                            //emailLoggingItem.GPP_GR_EmailTo = Convert.ToString(item[EmailLoggingListFields.GPP_GR_EmailTo]);

                            //emailLoggingItem.SSR_SAP_EmailDate = Convert.ToDateTime(item[EmailLoggingListFields.SSR_SAP_EmailDate]);
                            //emailLoggingItem.SSR_SAP_EmailTo = Convert.ToString(item[EmailLoggingListFields.SSR_SAP_EmailTo]);
                        }
                    }
                }
            }
            return emailLoggingItem;
        }

        public int InsertEmailLoggingItem(int compassListItemId, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);

                        SPListItem emailLoggingItem = spList.AddItem();

                        emailLoggingItem["Title"] = title;
                        emailLoggingItem[EmailLoggingListFields.CompassListItemId] = compassListItemId;

                        emailLoggingItem.Update();
                        spWeb.AllowUnsafeUpdates = false;

                        id = emailLoggingItem.ID;
                    }
                }
            });
            return id;
        }

        public EmailLoggingSentVersions GetEmailLoggingVersions(int itemId)
        {
            EmailLoggingSentVersions emailVersions = new EmailLoggingSentVersions();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];

                        if ((item != null) && (item.Versions != null) && (item.Versions.Count > 0))
                        {
                            SPListItemVersionCollection versions = item.Versions;
                            foreach (SPListItemVersion ver in versions)
                            {
                                //if (ver[EmailLoggingListFields.ICF_SLT_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.ICF_SLT_EmailDate].ToString()), WorkflowStep.IAPP);
                                //}
                                //if (ver[EmailLoggingListFields.CMF_CM_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.CMF_CM_EmailDate].ToString()), WorkflowStep.TRADE);
                                //}
                                //if (ver[EmailLoggingListFields.COMAN_COMAN_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.COMAN_COMAN_EmailDate].ToString()), WorkflowStep.EXTMFG);
                                //}
                                //if (ver[EmailLoggingListFields.ICF_CC_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.ICF_CC_EmailDate].ToString()), WorkflowStep.ICAP);
                                //}
                                //if (ver[EmailLoggingListFields.ICF_CST_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.ICF_CST_EmailDate].ToString()), WorkflowStep.ICST);
                                //}
                                //if (ver[EmailLoggingListFields.MN_PE_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.MN_PE_EmailDate].ToString()), WorkflowStep.BOMPE);
                                //}
                                //if (ver[EmailLoggingListFields.MN_PE2_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.MN_PE2_EmailDate].ToString()), WorkflowStep.BOMPE2);
                                //}
                                //if (ver[EmailLoggingListFields.MN_PUR_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.MN_PUR_EmailDate].ToString()), WorkflowStep.BOMPROC);
                                //}
                                //if (ver[EmailLoggingListFields.MN_SAP_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.MN_SAP_EmailDate].ToString()), WorkflowStep.BOMSAP);
                                //}
                                //if (ver[EmailLoggingListFields.OBMREV1_OBM_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.OBMREV1_OBM_EmailDate].ToString()), WorkflowStep.OBMREV1);
                                //}
                                //if (ver[EmailLoggingListFields.OPS_DIST_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.OPS_DIST_EmailDate].ToString()), WorkflowStep.DIST);
                                //}
                                //if (ver[EmailLoggingListFields.OPS_ICR_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.OPS_ICR_EmailDate].ToString()), WorkflowStep.OPSICR);
                                //}
                                //if (ver[EmailLoggingListFields.OPS_MFG_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.OPS_MFG_EmailDate].ToString()), WorkflowStep.MAKEPACK);
                                //}
                                //if (ver[EmailLoggingListFields.QA_QA_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.QA_QA_EmailDate].ToString()), WorkflowStep.QA);
                                //}
                                //if (ver[EmailLoggingListFields.RGF_GR_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.RGF_GR_EmailDate].ToString()), WorkflowStep.RGFGR);
                                //}
                                //if (ver[EmailLoggingListFields.RGF_OBM_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.RGF_OBM_EmailDate].ToString()), WorkflowStep.OBMREV2);
                                //}
                                //if (ver[EmailLoggingListFields.SIR_SAP_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.SIR_SAP_EmailDate].ToString()), WorkflowStep.SAPITEMSETUP);
                                //}
                                //if (ver[EmailLoggingListFields.SIS_SAP_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.SIS_SAP_EmailDate].ToString()), WorkflowStep.SISSAP);
                                //}
                                //if (ver[EmailLoggingListFields.TBD_SAP_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.TBD_SAP_EmailDate].ToString()), WorkflowStep.NEWSEMI);
                                //}
                                //if (ver[EmailLoggingListFields.GPP_GR_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.GPP_GR_EmailDate].ToString()), WorkflowStep.GPP);
                                //}
                                //if (ver[EmailLoggingListFields.OBMREV3_OBM_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.OBMREV3_OBM_EmailDate].ToString()), WorkflowStep.OBMREV3);
                                //}
                                //if (ver[EmailLoggingListFields.SSR_SAP_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.SSR_SAP_EmailDate].ToString()), WorkflowStep.SSRSAP);
                                //}
                                //if (ver[EmailLoggingListFields.Trade_Spending_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.Trade_Spending_EmailDate].ToString()), WorkflowStep.ZESTPRICING_NOTIFICATION);
                                //}
                                //if (ver[EmailLoggingListFields.Demand_Planning_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.Demand_Planning_EmailDate].ToString()), WorkflowStep.DEMANDPLANNING_NOTIFICATION);
                                //}
                                ////if (ver[EmailLoggingListFields.Sales_Planning_EmailDate] != null)
                                ////{
                                ////    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.Sales_Planning_EmailDate].ToString()), WorkflowStep.SALESPLAN_NOTIFICATION);
                                ////}
                                //if (ver[EmailLoggingListFields.TBDSAP_Notification_EmailDate] != null)
                                //{
                                //    emailVersions.AddVersion(GetLocalDateTime(ver[EmailLoggingListFields.TBDSAP_Notification_EmailDate].ToString()), WorkflowStep.TBDSAP_NOTIFICATION);
                                //}
                            }
                        }
                    }
                }
            }
            return emailVersions;
        }
        public List<string> GetVersionDisplay(List<KeyValuePair<string, WorkflowStep>> versionList, string workflowStep)
        {
            List<string> AllWorflowStepVersions = (from key in versionList where key.Value.ToString() == workflowStep select key.Key).ToList();
            List<string> versions = new List<string>();
            string lastVersion = string.Empty;
            foreach (string str in AllWorflowStepVersions)
            {
                if (!string.Equals(str, lastVersion))
                    versions.Add(str);
                lastVersion = str;
            }
            return versions;
        }
        public List<KeyValuePair<string, WorkflowStep>> GetEmailLoggingHistory(int itemId)
        {
            List<KeyValuePair<string, WorkflowStep>> emailVersions = new List<KeyValuePair<string, WorkflowStep>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];

                        if ((item != null) && (item.Versions != null) && (item.Versions.Count > 0))
                        {
                            SPListItemVersionCollection versions = item.Versions;
                            foreach (SPListItemVersion ver in versions)
                            {
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.SrOBMApproval_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.SrOBMApproval_EmailDate]), WorkflowStep.SrOBMApproval));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.TradePromo_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.TradePromo_EmailDate]), WorkflowStep.TradePromo));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.EstPricing_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.EstPricing_EmailDate]), WorkflowStep.EstPricing));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.EstBracketPricing_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.EstBracketPricing_EmailDate]), WorkflowStep.EstBracketPricing));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.ExternalMfg_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.ExternalMfg_EmailDate]), WorkflowStep.ExternalMfg));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.BOMSetupPE_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.BOMSetupPE_EmailDate]), WorkflowStep.BOMSetupPE));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.BOMSetupPE2_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.BOMSetupPE2_EmailDate]), WorkflowStep.BOMSetupPE2));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.BOMSetupProc_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.BOMSetupProc_EmailDate]), WorkflowStep.BOMSetupProc));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.SAPBOMSetup_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.SAPBOMSetup_EmailDate]), WorkflowStep.SAPBOMSetup));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.OBMReview1_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.OBMReview1_EmailDate]), WorkflowStep.OBMReview1));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.Distribution_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.Distribution_EmailDate]), WorkflowStep.Distribution));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.Operations_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.Operations_EmailDate]), WorkflowStep.Operations));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.QA_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.QA_EmailDate]), WorkflowStep.QA));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.OBMReview2_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.OBMReview2_EmailDate]), WorkflowStep.OBMReview2));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.SAPInitialSetup_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.SAPInitialSetup_EmailDate]), WorkflowStep.SAPInitialSetup));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.PrelimSAPInitialSetup_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.PrelimSAPInitialSetup_EmailDate]), WorkflowStep.PrelimSAPInitialSetup));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.FGPackSpec_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.FGPackSpec_EmailDate]), WorkflowStep.FGPackSpec));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.Graphics_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.Graphics_EmailDate]), WorkflowStep.Graphics));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.OBMReview3_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.OBMReview3_EmailDate]), WorkflowStep.OBMReview3));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.SAPRoutingSetupNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.SAPRoutingSetupNotification_EmailDate]), WorkflowStep.SAPROUTINGSETUP_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.ZestPricingNotification_EmailDate])))
                                {
                                    //emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.ZestPricingNotification_EmailDate]), WorkflowStep.ZESTPRICING_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.DemandPlanningNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.DemandPlanningNotification_EmailDate]), WorkflowStep.DEMANDPLANNING_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.SAPCostingDetailsNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.SAPCostingDetailsNotification_EmailDate]), WorkflowStep.SAPCOSTINGDETAILS_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.PurchasedPONotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.PurchasedPONotification_EmailDate]), WorkflowStep.PURCHASEPO_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.BOMActiveDateNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.BOMActiveDateNotification_EmailDate]), WorkflowStep.BOMACTIVEDATE_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.RemoveSAPBlocksNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.RemoveSAPBlocksNotification_EmailDate]), WorkflowStep.REMOVESAPBLOCKS_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.CustomerPONotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.CustomerPONotification_EmailDate]), WorkflowStep.CUSTOMERPOSCANBEENTERED_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.StandardCostEntryNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.StandardCostEntryNotification_EmailDate]), WorkflowStep.STANDARDCOSTENTRY_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.CostFinishedGoodNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.CostFinishedGoodNotification_EmailDate]), WorkflowStep.COSTFINISHEDGOOD_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.FinalCostingReviewNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.FinalCostingReviewNotification_EmailDate]), WorkflowStep.FINALCOSTINGREVIEW_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.SAPWarehouseInfoNotification_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.SAPWarehouseInfoNotification_EmailDate]), WorkflowStep.SAPWAREHOUSEINFO_NOTIFICATION));
                                }
                                if (!String.IsNullOrEmpty(Convert.ToString(ver[EmailLoggingListFields.SAPCompleteItemSetup_EmailDate])))
                                {
                                    emailVersions.Add(new KeyValuePair<string, WorkflowStep>(Convert.ToString(ver[EmailLoggingListFields.SAPCompleteItemSetup_EmailDate]), WorkflowStep.SAPCompleteItemSetup));
                                }
                            }
                        }
                    }
                }
            }
            return emailVersions;
        }
        #region Email Sent Date
        public void LogSentEmailUpdate(int CompassItemId, string workflowTask, string projectName)
        {
            if (workflowTask == "SrOBMApprovalSeasonal" || workflowTask == "SrOBMApprovalBulkCoMan" || workflowTask == "SrOBMApprovalEveryday")
            {
                workflowTask = "SrOBMApproval";
            }else if (workflowTask.Contains("BOMSetupProc"))
            {
                workflowTask = "BOMSetupProc";
            }else if(workflowTask == "BOMSetupMaterialWarehouse")
            {
                workflowTask = "BOMSetupMH";
            }

            string workflowTaskDateName = workflowTask + "_EmailDate";
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        SPListItem emailLoggingItem = null;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            emailLoggingItem = compassItemCol[0];
                            if (emailLoggingItem == null)
                            {
                                // Couldn't find a logging item, so insert a new record!
                                emailLoggingItem = spList.AddItem();
                                emailLoggingItem["Title"] = projectName;
                                emailLoggingItem[EmailLoggingListFields.CompassListItemId] = CompassItemId;
                                emailLoggingItem[workflowTaskDateName] = DateTime.Now;
                                emailLoggingItem.Update();
                            }
                            else
                            {
                                emailLoggingItem[workflowTaskDateName] = DateTime.Now;
                                emailLoggingItem.Update();
                            }
                        }
                        else
                        {
                            // Couldn't find a logging item, so insert a new record!
                            emailLoggingItem = spList.AddItem();
                            emailLoggingItem["Title"] = projectName;
                            emailLoggingItem[EmailLoggingListFields.CompassListItemId] = CompassItemId;
                            emailLoggingItem[workflowTaskDateName] = DateTime.Now;
                            emailLoggingItem.Update();

                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void LogSentEmail(EmailLoggingListItem emailLoggingListItem, string title)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_EmailLoggingListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + emailLoggingListItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;
                        SPListItem emailLoggingItem = null;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            emailLoggingItem = compassItemCol[0];
                            if (emailLoggingItem == null)
                            {
                                // Couldn't find a logging item, so insert a new record!
                                int id = InsertEmailLoggingItem(emailLoggingListItem.CompassListItemId, title);
                                emailLoggingItem = spList.GetItemById(id);
                            }
                        }
                        else
                        {
                            // Couldn't find a logging item, so insert a new record!
                            int id = InsertEmailLoggingItem(emailLoggingListItem.CompassListItemId, title);
                            emailLoggingItem = spList.GetItemById(id);
                        }

                        if (emailLoggingItem != null)
                        {
                            // Update any email dates that have been set
                            //if ((emailLoggingListItem.CMF_CM_EmailDate != null) && (emailLoggingListItem.CMF_CM_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.CMF_CM_EmailDate] = emailLoggingListItem.CMF_CM_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.CMF_CM_EmailTo] = emailLoggingListItem.CMF_CM_EmailTo;
                            //}

                            //if ((emailLoggingListItem.COMAN_COMAN_EmailDate != null) && (emailLoggingListItem.COMAN_COMAN_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.COMAN_COMAN_EmailDate] = emailLoggingListItem.COMAN_COMAN_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.COMAN_COMAN_EmailTo] = emailLoggingListItem.COMAN_COMAN_EmailTo;
                            //}

                            //if ((emailLoggingListItem.ICF_CC_EmailDate != null) && (emailLoggingListItem.ICF_CC_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.ICF_CC_EmailDate] = emailLoggingListItem.ICF_CC_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.ICF_CC_EmailTo] = emailLoggingListItem.ICF_CC_EmailTo;
                            //}

                            //if ((emailLoggingListItem.ICF_CST_EmailDate != null) && (emailLoggingListItem.ICF_CST_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.ICF_CST_EmailDate] = emailLoggingListItem.ICF_CST_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.ICF_CST_EmailTo] = emailLoggingListItem.ICF_CST_EmailTo;
                            //}

                            //if ((emailLoggingListItem.ICF_SLT_EmailDate != null) && (emailLoggingListItem.ICF_SLT_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.ICF_SLT_EmailDate] = emailLoggingListItem.ICF_SLT_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.ICF_SLT_EmailTo] = emailLoggingListItem.ICF_SLT_EmailTo;
                            //}

                            //if ((emailLoggingListItem.MN_PE_EmailDate != null) && (emailLoggingListItem.MN_PE_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.MN_PE_EmailDate] = emailLoggingListItem.MN_PE_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.MN_PE_EmailTo] = emailLoggingListItem.MN_PE_EmailTo;
                            //}

                            //if ((emailLoggingListItem.MN_PE2_EmailDate != null) && (emailLoggingListItem.MN_PE2_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.MN_PE2_EmailDate] = emailLoggingListItem.MN_PE2_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.MN_PE2_EmailTo] = emailLoggingListItem.MN_PE2_EmailTo;
                            //}

                            //if ((emailLoggingListItem.MN_PUR_EmailDate != null) && (emailLoggingListItem.MN_PUR_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.MN_PUR_EmailDate] = emailLoggingListItem.MN_PUR_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.MN_PUR_EmailTo] = emailLoggingListItem.MN_PUR_EmailTo;
                            //}

                            //if ((emailLoggingListItem.MN_SAP_EmailDate != null) && (emailLoggingListItem.MN_SAP_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.MN_SAP_EmailDate] = emailLoggingListItem.MN_SAP_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.MN_SAP_EmailTo] = emailLoggingListItem.MN_SAP_EmailTo;
                            //}

                            //if ((emailLoggingListItem.OPS_DIST_EmailDate != null) && (emailLoggingListItem.OPS_DIST_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.OPS_DIST_EmailDate] = emailLoggingListItem.OPS_DIST_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.OPS_DIST_EmailTo] = emailLoggingListItem.OPS_DIST_EmailTo;
                            //}

                            //if ((emailLoggingListItem.OPS_ICR_EmailDate != null) && (emailLoggingListItem.OPS_ICR_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.OPS_ICR_EmailDate] = emailLoggingListItem.OPS_ICR_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.OPS_ICR_EmailTo] = emailLoggingListItem.OPS_ICR_EmailTo;
                            //}

                            //if ((emailLoggingListItem.OPS_MFG_EmailDate != null) && (emailLoggingListItem.OPS_MFG_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.OPS_MFG_EmailDate] = emailLoggingListItem.OPS_MFG_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.OPS_MFG_EmailTo] = emailLoggingListItem.OPS_MFG_EmailTo;
                            //}

                            //if ((emailLoggingListItem.OPS_RT_EmailDate != null) && (emailLoggingListItem.OPS_RT_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.OPS_RT_EmailDate] = emailLoggingListItem.OPS_RT_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.OPS_RT_EmailTo] = emailLoggingListItem.OPS_RT_EmailTo;
                            //}

                            //if ((emailLoggingListItem.QA_QA_EmailDate != null) && (emailLoggingListItem.QA_QA_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.QA_QA_EmailDate] = emailLoggingListItem.QA_QA_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.QA_QA_EmailTo] = emailLoggingListItem.QA_QA_EmailTo;
                            //}

                            //if ((emailLoggingListItem.RGF_GR_EmailDate != null) && (emailLoggingListItem.RGF_GR_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.RGF_GR_EmailDate] = emailLoggingListItem.RGF_GR_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.RGF_GR_EmailTo] = emailLoggingListItem.RGF_GR_EmailTo;
                            //}

                            //if ((emailLoggingListItem.RGF_OBM_EmailDate != null) && (emailLoggingListItem.RGF_OBM_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.RGF_OBM_EmailDate] = emailLoggingListItem.RGF_OBM_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.RGF_OBM_EmailTo] = emailLoggingListItem.RGF_OBM_EmailTo;
                            //}

                            //if ((emailLoggingListItem.SIR_SAP_EmailDate != null) && (emailLoggingListItem.SIR_SAP_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.SIR_SAP_EmailDate] = emailLoggingListItem.SIR_SAP_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.SIR_SAP_EmailTo] = emailLoggingListItem.SIR_SAP_EmailTo;
                            //}

                            //if ((emailLoggingListItem.SIS_SAP_EmailDate != null) && (emailLoggingListItem.SIS_SAP_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.SIS_SAP_EmailDate] = emailLoggingListItem.SIS_SAP_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.SIS_SAP_EmailTo] = emailLoggingListItem.SIS_SAP_EmailTo;
                            //}

                            //if ((emailLoggingListItem.TBD_SAP_EmailDate != null) && (emailLoggingListItem.TBD_SAP_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.TBD_SAP_EmailDate] = emailLoggingListItem.TBD_SAP_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.TBD_SAP_EmailTo] = emailLoggingListItem.TBD_SAP_EmailTo;
                            //}

                            //if ((emailLoggingListItem.OBMREV1_OBM_EmailDate != null) && (emailLoggingListItem.OBMREV1_OBM_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.OBMREV1_OBM_EmailDate] = emailLoggingListItem.OBMREV1_OBM_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.OBMREV1_OBM_EmailTo] = emailLoggingListItem.OBMREV1_OBM_EmailTo;
                            //}

                            //if ((emailLoggingListItem.GPP_GR_EmailDate != null) && (emailLoggingListItem.GPP_GR_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.GPP_GR_EmailDate] = emailLoggingListItem.GPP_GR_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.GPP_GR_EmailTo] = emailLoggingListItem.GPP_GR_EmailTo;
                            //}

                            //if ((emailLoggingListItem.OBMREV3_OBM_EmailDate != null) && (emailLoggingListItem.OBMREV3_OBM_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.OBMREV3_OBM_EmailDate] = emailLoggingListItem.OBMREV3_OBM_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.OBMREV3_OBM_EmailTo] = emailLoggingListItem.OBMREV3_OBM_EmailTo;
                            //}

                            //if ((emailLoggingListItem.SSR_SAP_EmailDate != null) && (emailLoggingListItem.SSR_SAP_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.SSR_SAP_EmailDate] = emailLoggingListItem.SSR_SAP_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.SSR_SAP_EmailTo] = emailLoggingListItem.SSR_SAP_EmailTo;
                            //}

                            //if ((emailLoggingListItem.Trade_Spending_EmailDate != null) && (emailLoggingListItem.Trade_Spending_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.Trade_Spending_EmailDate] = emailLoggingListItem.Trade_Spending_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.Trade_Spending_EmailTo] = emailLoggingListItem.Trade_Spending_EmailTo;
                            //}

                            //if ((emailLoggingListItem.Demand_Planning_EmailDate != null) && (emailLoggingListItem.Demand_Planning_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.Demand_Planning_EmailDate] = emailLoggingListItem.Demand_Planning_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.Demand_Planning_EmailTo] = emailLoggingListItem.Demand_Planning_EmailTo;
                            //}

                            ////if ((emailLoggingListItem.Sales_Planning_EmailDate != null) && (emailLoggingListItem.Sales_Planning_EmailDate != DateTime.MinValue))
                            ////{
                            ////    emailLoggingItem[EmailLoggingListFields.Sales_Planning_EmailDate] = emailLoggingListItem.Sales_Planning_EmailDate;
                            ////    emailLoggingItem[EmailLoggingListFields.Sales_Planning_EmailTo] = emailLoggingListItem.Sales_Planning_EmailTo;
                            ////}

                            //if ((emailLoggingListItem.TBDSAP_Notification_EmailDate != null) && (emailLoggingListItem.TBDSAP_Notification_EmailDate != DateTime.MinValue))
                            //{
                            //    emailLoggingItem[EmailLoggingListFields.TBDSAP_Notification_EmailDate] = emailLoggingListItem.TBDSAP_Notification_EmailDate;
                            //    emailLoggingItem[EmailLoggingListFields.TBDSAP_Notification_EmailTo] = emailLoggingListItem.TBDSAP_Notification_EmailTo;
                            //}
                            emailLoggingItem.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion

        #region Private Methods
        private string GetLocalDateTime(string datetime)
        {
            DateTime result;
            if (DateTime.TryParse(datetime, out result))
            {
                return result.ToLocalTime().ToString();
            }

            return datetime;
        }
        #endregion
    }
}
