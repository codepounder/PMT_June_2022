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
    public class SAPInitialItemSetUpService : ISAPInitialItemSetUpService
    {
        public SAPInitialItemSetUp GetSAPInitialSetupItem(int itemId)
        {
            var newItem = new SAPInitialItemSetUp();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        newItem.NewIPF = Convert.ToString(item[CompassListFields.NewIPF]);
                        newItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        newItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        newItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        newItem.ItemDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        newItem.ProfitCenter = Convert.ToString(item[CompassListFields.ProfitCenter]);

                        newItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        newItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        newItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        newItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                        newItem.CaseUCC = Convert.ToString(item[CompassListFields.CaseUCC]);
                        newItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        newItem.PalletUCC = Convert.ToString(item[CompassListFields.PalletUCC]);
                        newItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        newItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        newItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        newItem.MfgLocationChange = Convert.ToString(item[CompassListFields.MfgLocationChange]);
                        newItem.ImmediateSPKChange = Convert.ToString(item[CompassListFields.ImmediateSPKChange]);
                        newItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);
                        newItem.NoveltyProject = Convert.ToString(item[CompassListFields.NoveltyProject]);
                        newItem.ProfitCenter = Convert.ToString(item[CompassListFields.ProfitCenter]);

                        newItem.RequireNewDisplayBoxUPC = Convert.ToString(item[CompassListFields.RequireNewDisplayBoxUPC]);

                        newItem.MaterialGroup2Pricing = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);

                        if (item[CompassListFields.RetailSellingUnitsBaseUOM] != null)
                        {
                            try { newItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]); }
                            catch { newItem.RetailSellingUnitsBaseUOM = -9999; }
                        }
                        else
                        {
                            newItem.RetailSellingUnitsBaseUOM = -9999;
                        }

                        if (item[CompassListFields.RetailUnitWieghtOz] != null)
                        {
                            try { newItem.RetailUnitWieghtOz = Convert.ToDouble(item[CompassListFields.RetailUnitWieghtOz]); }
                            catch { newItem.RetailUnitWieghtOz = -9999; }
                        }
                        else
                        {
                            newItem.RetailUnitWieghtOz = -9999;
                        }
                        newItem = GetDistributionItem2(newItem, spWeb, itemId);
                    }
                }
            }
            return newItem;
        }
        private SAPInitialItemSetUp GetDistributionItem2(SAPInitialItemSetUp SAPInitialItem, SPWeb spWeb, int itemId)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            var CompassList2Items = spList.GetItems(spQuery);

            if (CompassList2Items != null && CompassList2Items.Count > 0)
            {
                var ipItem = CompassList2Items[0];
                SAPInitialItem.DesignateHUBDC = Convert.ToString(ipItem[CompassList2Fields.DesignateHUBDC]);
                SAPInitialItem.DeploymentModeofItem = Convert.ToString(ipItem[CompassList2Fields.DeploymentModeofItem]);

                #region SELL DCs
                SAPInitialItem.ExtendtoSL07 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL07]);
                SAPInitialItem.SetSL07SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL07SPKto]);
                SAPInitialItem.ExtendtoSL13 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL13]);
                SAPInitialItem.SetSL13SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL13SPKto]);
                SAPInitialItem.ExtendtoSL18 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL18]);
                SAPInitialItem.SetSL18SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL18SPKto]);
                SAPInitialItem.ExtendtoSL19 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL19]);
                SAPInitialItem.SetSL19SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL19SPKto]);
                SAPInitialItem.ExtendtoSL30 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL30]);
                SAPInitialItem.SetSL30SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL30SPKto]);
                SAPInitialItem.ExtendtoSL14 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL14]);
                SAPInitialItem.SetSL14SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL14SPKto]);
                #endregion

                #region FERQ DCs
                SAPInitialItem.ExtendtoFQ26 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ26]);
                SAPInitialItem.SetFQ26SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ26SPKto]);
                SAPInitialItem.ExtendtoFQ27 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ27]);
                SAPInitialItem.SetFQ27SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ27SPKto]);
                SAPInitialItem.ExtendtoFQ28 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ28]);
                SAPInitialItem.SetFQ28SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ28SPKto]);
                SAPInitialItem.ExtendtoFQ29 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ29]);
                SAPInitialItem.SetFQ29SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ29SPKto]);
                SAPInitialItem.ExtendtoFQ34 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ34]);
                SAPInitialItem.SetFQ34SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ34SPKto]);
                SAPInitialItem.ExtendtoFQ35 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ35]);
                SAPInitialItem.SetFQ35SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ35SPKto]);
                #endregion
            }
            return SAPInitialItem;
        }
        public void UpdateSAPInitialSetupItem(SAPInitialItemSetUp ipItem, string formName)
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
                        var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        var item = spList.GetItemById(ipItem.CompassListItemId);
                        if (item != null)
                        {
                            // SAP Item # Fields
                            item[CompassListFields.SAPItemNumber] = ipItem.SAPItemNumber;
                            item[CompassListFields.SAPDescription] = ipItem.SAPDescription;

                            item[CompassListFields.CaseUCC] = ipItem.CaseUCC;
                            item[CompassListFields.DisplayBoxUPC] = ipItem.DisplayBoxUPC;
                            item[CompassListFields.PalletUCC] = ipItem.PalletUCC;
                            item[CompassListFields.UnitUPC] = ipItem.UnitUPC;

                            item[CompassListFields.LastUpdatedFormName] = formName;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                            spWeb.AllowUnsafeUpdates = false;
                        }
                    }
                }
            });

            UpdateProjectDecisionListForNewTransferSemi(ipItem);
        }

        public void UpdateProjectDecisionListForNewTransferSemi(SAPInitialItemSetUp ipItem)
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
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + ipItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];

                            if (item != null)
                            {
                                item[CompassProjectDecisionsListFields.NewTransferSemi] = ipItem.NewTransferSemi;

                                // Set Modified By to current user NOT System Account
                                item["Editor"] = SPContext.Current.Web.CurrentUser;

                                item.Update();
                            }
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Approval Methods
        public void UpdateSAPInitialSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.SAPInitialSetup_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.SAPInitialSetup_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.SAPInitialSetup_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.SAPInitialSetup_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.SAPInitialSetup_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetSAPInitialSetupStartDate(int compassListItemId, DateTime startDate, string title)
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
                                if (item[ApprovalListFields.SAPInitialSetup_StartDate] == null)
                                {
                                    item[ApprovalListFields.SAPInitialSetup_StartDate] = startDate.ToString();
                                    item.Update();
                                }
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdatePrelimSAPInitialSetupApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.PrelimSAPInitialSetup_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.PrelimSAPInitialSetup_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.PrelimSAPInitialSetup_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.PrelimSAPInitialSetup_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetPrelimSAPInitialSetupStartDate(int compassListItemId, DateTime startDate, string title)
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
        #endregion
    }
}
