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
    public class SAPCompleteItemSetupService : ISAPCompleteItemSetupService
    {
        public SAPCompleteSetupItem GetSAPCompleteItemSetupItem(int itemId)
        {
            SAPCompleteSetupItem sgItem = new SAPCompleteSetupItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);

                        sgItem.ImmediateSPKChange = Convert.ToString(item[CompassListFields.ImmediateSPKChange]);
                        sgItem.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.MaterialGroup2Pricing = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                        sgItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        sgItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                        sgItem.ProfitCenter = Convert.ToString(item[CompassListFields.ProfitCenter]);
                        sgItem.ProcurementType = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        sgItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        sgItem.CaseUCC = Convert.ToString(item[CompassListFields.CaseUCC]);
                        sgItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        sgItem.PalletUCC = Convert.ToString(item[CompassListFields.PalletUCC]);
                        sgItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        sgItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                    }
                    #region Compass List 2
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spComapssList2Query = new SPQuery();
                    spComapssList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spComapssList2Query.RowLimit = 1;

                    SPListItemCollection compassList2ItemCol = spList.GetItems(spComapssList2Query);
                    if (compassList2ItemCol.Count > 0)
                    {
                        SPListItem compassList2Item = compassList2ItemCol[0];
                        if (compassList2Item != null)
                        {
                            sgItem.DesignateHUBDC = Convert.ToString(compassList2Item[CompassList2Fields.DesignateHUBDC]);
                        }
                    }
                    #endregion
                    #region ProjectDecisionsList
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
                            sgItem.CmpltFGMtrlMaster = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltFGMtrlMstr]);
                            sgItem.TurnkeyFGMMCrtd = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPCmpltItmStup_TurnkeyFGMMCrtd]);
                            sgItem.FGSAPSpChCmpltd = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPCmpltItmStup_FGSAPSpChCmpltd]);
                            sgItem.CompleteTSBOM = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltTSBOM]);
                        }
                    }
                    #endregion
                    #region CompassPackMeasurementsList
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">0</Value></Eq></And></Where>";
                    SPListItemCollection compassItemCol2 = spList.GetItems(spQuery2);
                    sgItem.FGSAPSpecsChangePackMeas = "";
                    if (compassItemCol2.Count > 0)
                    {
                        SPListItem decisionItem2 = compassItemCol2[0];
                        if (decisionItem2 != null)
                        {
                            sgItem.DoubleStackable = Convert.ToString(decisionItem2[CompassPackMeasurementsFields.DoubleStackable]);
                            sgItem.FGSAPSpecsChangePackMeas = Convert.ToString(decisionItem2[CompassPackMeasurementsFields.SAPSpecsChange]);
                        }
                    }
                    #endregion
                }
            }
            return sgItem;
        }
        public List<string> getTSSPKDetails(int itemId)
        {
            List<string> sgItems = new List<string>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery3 = new SPQuery();
                    spQuery3.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Neq><FieldRef Name=\"" + PackagingItemListFields.Deleted + "\" /><Value Type=\"Text\">Yes</Value></Neq></And></Where>";
                    SPListItemCollection compassItemCol3 = spList.GetItems(spQuery3);
                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem decisionItem3 in compassItemCol3)
                        {
                            if (decisionItem3 != null)
                            {
                                string materialType = Convert.ToString(decisionItem3[PackagingItemListFields.PackagingComponent]);
                                if (materialType.ToLower() == "transfer semi" || materialType.ToLower() == "purchased candy semi")
                                {
                                    string newExisting = Convert.ToString(decisionItem3[PackagingItemListFields.NewExisting]);
                                    if (newExisting == "Network Move")
                                    {
                                        string MatType = "";
                                        string MatNumber = Convert.ToString(decisionItem3[PackagingItemListFields.MaterialNumber]);
                                        string SPKDec = Convert.ToString(decisionItem3[PackagingItemListFields.ImmediateSPKChange]);
                                        if (materialType.ToLower() == "purchased candy semi")
                                        {
                                            MatType = "PSC";
                                        }
                                        else
                                        {
                                            MatType = "TS";
                                        }
                                        sgItems.Add("Immediate SPK Change for " + MatType + " " + MatNumber + ": " + SPKDec);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return sgItems;
        }
        public void UpdateSAPCompleteItemSetupItem(SAPCompleteSetupItem sapCompleteItemSetupItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        #region LIST_ProjectDecisionsListName
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + sapCompleteItemSetupItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            item[CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltFGMtrlMstr] = sapCompleteItemSetupItem.CmpltFGMtrlMaster;
                            item[CompassProjectDecisionsListFields.SAPCmpltItmStup_TurnkeyFGMMCrtd] = sapCompleteItemSetupItem.TurnkeyFGMMCrtd;
                            item[CompassProjectDecisionsListFields.SAPCmpltItmStup_FGSAPSpChCmpltd] = sapCompleteItemSetupItem.FGSAPSpChCmpltd;
                            item[CompassProjectDecisionsListFields.SAPCmpltItmStup_CmpltTSBOM] = sapCompleteItemSetupItem.CompleteTSBOM;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        } 
                        #endregion


                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });

            UpdateLastUpdatedFormInCompassList(sapCompleteItemSetupItem);
        }
        public void UpdateSAPCompleteItemSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.SAPCompleteItemSetup_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.SAPCompleteItemSetup_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SAPCompleteItemSetup_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.SAPCompleteItemSetup_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SAPCompleteItemSetup_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        private void UpdateLastUpdatedFormInCompassList(SAPCompleteSetupItem sapCompleteItemSetupItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(sapCompleteItemSetupItem.CompassListItemId);

                        if (item != null)
                        {
                            item[CompassListFields.LastUpdatedFormName] = CompassForm.SAPCompleteItemSetup.ToString();

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}
