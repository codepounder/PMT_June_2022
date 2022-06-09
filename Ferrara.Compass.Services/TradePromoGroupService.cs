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
    public class TradePromoGroupService : ITradePromoGroupService
    {
        public TradePromoGroupItem GetTradePromoGroupItem(int itemId)
        {
            TradePromoGroupItem sgItem = new TradePromoGroupItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    #region Compass List
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);

                        sgItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.MaterialGroup2Pricing = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                        sgItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        sgItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);

                        if (item[CompassListFields.RetailSellingUnitsBaseUOM] != null)
                        {
                            try
                            {
                                sgItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]);
                            }
                            catch
                            {
                                sgItem.RetailSellingUnitsBaseUOM = -9999;
                            }
                        }
                        else
                        {
                            sgItem.RetailSellingUnitsBaseUOM = -9999;
                        }

                        sgItem.TruckLoadPricePerSellingUnit = Convert.ToDouble(item[CompassListFields.TruckLoadPricePerSellingUnit]);
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        sgItem.CustomerSpecific = Convert.ToString(item[CompassListFields.CustomerSpecific]);
                        sgItem.Customer = Convert.ToString(item[CompassListFields.Customer]);
                        sgItem.Channel = Convert.ToString(item[CompassListFields.Channel]);
                        sgItem.CaseType = Convert.ToString(item[CompassListFields.CaseType]);

                        if (item[CompassListFields.UnitsInsideCarton] != null)
                        {
                            try
                            {
                                sgItem.UnitsInsideCarton = Convert.ToString(item[CompassListFields.UnitsInsideCarton]);
                            }
                            catch
                            {
                                sgItem.UnitsInsideCarton = "";
                            }
                        }
                        else
                        {
                            sgItem.UnitsInsideCarton = "";
                        }

                        if (item[CompassListFields.IndividualPouchWeight] != null)
                        {
                            try
                            {
                                sgItem.IndividualPouchWeight = Convert.ToDouble(item[CompassListFields.IndividualPouchWeight]);
                            }
                            catch
                            {
                                sgItem.IndividualPouchWeight = -9999;
                            }
                        }
                        else
                        {
                            sgItem.IndividualPouchWeight = -9999;
                        }

                        if (item[CompassListFields.NumberofTraysPerBaseUOM] != null)
                        {
                            try { sgItem.NumberofTraysPerBaseUOM = Convert.ToInt32(item[CompassListFields.NumberofTraysPerBaseUOM]); }
                            catch { sgItem.NumberofTraysPerBaseUOM = -9999; }
                        }
                        else
                        {
                            sgItem.NumberofTraysPerBaseUOM = -9999;
                        }

                        if (item[CompassListFields.RetailUnitWieghtOz] != null)
                        {
                            try { sgItem.RetailUnitWieghtOz = Convert.ToDouble(item[CompassListFields.RetailUnitWieghtOz]); }
                            catch { sgItem.RetailUnitWieghtOz = -9999; }
                        }
                        else
                        {
                            sgItem.RetailUnitWieghtOz = -9999;
                        }

                        if (item[CompassListFields.BaseUOMNetWeightLbs] != null)
                        {
                            try { sgItem.BaseUOMNetWeightLbs = Convert.ToDouble(item[CompassListFields.BaseUOMNetWeightLbs]); }
                            catch { sgItem.BaseUOMNetWeightLbs = -9999; }
                        }
                        else
                        {
                            sgItem.BaseUOMNetWeightLbs = -9999;
                        }
                    }

                    #endregion
                    #region Compass List 2
                    SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery2.RowLimit = 1;
                    SPListItemCollection compassItemCol2 = spList2.GetItems(spQuery2);
                    if (compassItemCol2.Count > 0)
                    {
                        SPListItem item2 = compassItemCol2[0];

                        if (item2 != null)
                        {
                            sgItem.InitialEstimatedPricing = Convert.ToString(item2[CompassList2Fields.InitialEstimatedPricing]);
                            sgItem.InitialEstimatedBracketPricing = Convert.ToString(item2[CompassList2Fields.InitialEstimatedBracketPricing]);
                        }
                    }
                    #endregion
                }
            }
            return sgItem;
        }
        public void UpdateTradePromoGroupItem(TradePromoGroupItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(compassListItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.MaterialGroup2Pricing] = compassListItem.MaterialGroup2Pricing;
                            item[CompassListFields.LastUpdatedFormName] = CompassForm.TradePromo.ToString();

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateEstimatedBracketPricingItem(TradePromoGroupItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];

                            if (item != null)
                            {
                                item[CompassList2Fields.InitialEstimatedBracketPricing] = compassListItem.InitialEstimatedBracketPricing;

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

        public void UpdateEstimatedPricingItem(TradePromoGroupItem compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];

                            if (item != null)
                            {
                                item[CompassList2Fields.InitialEstimatedPricing] = compassListItem.InitialEstimatedPricing;
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

        #region Trade Promo Group Approvals
        public ApprovalItem GetTradePromoGroupApprovalItem(int itemId)
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
                            appItem.ApprovalListItemId = item.ID;
                            appItem.StartDate = Convert.ToString(item[ApprovalListFields.TradePromo_StartDate]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.TradePromo_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.TradePromo_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.TradePromo_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.TradePromo_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdateTradePromoGroupApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.TradePromo_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.TradePromo_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.TradePromo_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.TradePromo_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.TradePromo_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateEstimatedBracketPricingApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                if ((bSubmitted) && (appItem[ApprovalListFields.EstBracketPricing_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.EstBracketPricing_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.EstBracketPricing_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.EstBracketPricing_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.EstBracketPricing_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateEstimatedPricingApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                if ((bSubmitted) && (appItem[ApprovalListFields.EstPricing_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.EstPricing_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.EstPricing_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.EstPricing_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.EstPricing_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetTradePromoGroupStartDate(int compassListItemId, DateTime startDate)
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
                                if (item[ApprovalListFields.TradePromo_StartDate] == null)
                                {
                                    item[ApprovalListFields.TradePromo_StartDate] = startDate.ToString();
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
