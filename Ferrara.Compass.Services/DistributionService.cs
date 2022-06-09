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
    public class DistributionService : IDistributionService
    {
        public DistributionItem GetDistributionItem(int itemId)
        {
            DistributionItem sgItem = new DistributionItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);

                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        sgItem.SoldOutsideUSA = Convert.ToString(item[CompassListFields.SoldOutsideUSA]);
                        sgItem.CountryOfSale = Convert.ToString(item[CompassListFields.CountryOfSale]);
                        sgItem.LikeItemNumber = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                        sgItem.LikeItemDescription = Convert.ToString(item[CompassListFields.LikeFGItemDescription]);
                        sgItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);


                        sgItem.ManufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.ProcurementType = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        sgItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        sgItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        sgItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);

                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.MaterialGroup2Pricing = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                        sgItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        sgItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);

                        if (item[CompassListFields.RetailSellingUnitsBaseUOM] != null)
                        {
                            try { sgItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]); }
                            catch { sgItem.RetailSellingUnitsBaseUOM = -9999; }
                        }
                        else
                        {
                            sgItem.RetailSellingUnitsBaseUOM = -9999;
                        }
                        sgItem.ItemDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.FGItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.FGItemDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem = GetDistributionItem2(sgItem, spWeb, itemId);
                    }
                }
            }
            return sgItem;
        }

        private DistributionItem GetDistributionItem2(DistributionItem distributionItem, SPWeb spWeb, int itemId)
        {
            var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
            SPQuery spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
            var CompassList2Items = spList.GetItems(spQuery);

            if (CompassList2Items != null && CompassList2Items.Count > 0)
            {
                var ipItem = CompassList2Items[0];

                distributionItem.DesignateHUBDC = Convert.ToString(ipItem[CompassList2Fields.DesignateHUBDC]);
                distributionItem.DeploymentModeofItem = Convert.ToString(ipItem[CompassList2Fields.DeploymentModeofItem]);
                #region SELL DCs
                distributionItem.ExtendtoSL07 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL07]);
                distributionItem.SetSL07SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL07SPKto]);
                distributionItem.ExtendtoSL13 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL13]);
                distributionItem.SetSL13SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL13SPKto]);
                distributionItem.ExtendtoSL18 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL18]);
                distributionItem.SetSL18SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL18SPKto]);
                distributionItem.ExtendtoSL19 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL19]);
                distributionItem.SetSL19SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL19SPKto]);
                distributionItem.ExtendtoSL30 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL30]);
                distributionItem.SetSL30SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL30SPKto]);
                distributionItem.ExtendtoSL14 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoSL14]);
                distributionItem.SetSL14SPKto = Convert.ToString(ipItem[CompassList2Fields.SetSL14SPKto]);
                #endregion
                #region FERQ DCs
                distributionItem.ExtendtoFQ26 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ26]);
                distributionItem.SetFQ26SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ26SPKto]);
                distributionItem.ExtendtoFQ27 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ27]);
                distributionItem.SetFQ27SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ27SPKto]);
                distributionItem.ExtendtoFQ28 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ28]);
                distributionItem.SetFQ28SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ28SPKto]);
                distributionItem.ExtendtoFQ29 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ29]);
                distributionItem.SetFQ29SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ29SPKto]);
                distributionItem.ExtendtoFQ34 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ34]);
                distributionItem.SetFQ34SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ34SPKto]);
                distributionItem.ExtendtoFQ35 = Convert.ToString(ipItem[CompassList2Fields.ExtendtoFQ35]);
                distributionItem.SetFQ35SPKto = Convert.ToString(ipItem[CompassList2Fields.SetFQ35SPKto]);
                #endregion
            }
            return distributionItem;
        }
        public void UpdateDistributionItem(DistributionItem distItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(distItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.LastUpdatedFormName] = CompassForm.Distribution.ToString();

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                            #region Update Compass List 2
                            var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassList2Fields.CompassListItemId + "\" /><Value Type=\"Int\">" + distItem.CompassListItemId + "</Value></Eq></Where>";
                            var CompassListItems = spList2.GetItems(spQuery);
                            SPListItem CompassListItem;
                            if (CompassListItems != null)
                            {
                                if (CompassListItems.Count > 0)
                                {
                                    CompassListItem = CompassListItems[0];
                                }
                                else
                                {
                                    CompassListItem = spList2.AddItem();
                                    CompassListItem["Title"] = distItem.ProjectNumber;
                                    CompassListItem[CompassList2Fields.CompassListItemId] = distItem.CompassListItemId;
                                }
                                CompassListItem[CompassList2Fields.DesignateHUBDC] = distItem.DesignateHUBDC;
                                CompassListItem[CompassList2Fields.DeploymentModeofItem] = distItem.DeploymentModeofItem;
                                #region SELL DCs
                                CompassListItem[CompassList2Fields.ExtendtoSL07] = distItem.ExtendtoSL07;
                                CompassListItem[CompassList2Fields.SetSL07SPKto] = distItem.SetSL07SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoSL13] = distItem.ExtendtoSL13;
                                CompassListItem[CompassList2Fields.SetSL13SPKto] = distItem.SetSL13SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoSL18] = distItem.ExtendtoSL18;
                                CompassListItem[CompassList2Fields.SetSL18SPKto] = distItem.SetSL18SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoSL19] = distItem.ExtendtoSL19;
                                CompassListItem[CompassList2Fields.SetSL19SPKto] = distItem.SetSL19SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoSL30] = distItem.ExtendtoSL30;
                                CompassListItem[CompassList2Fields.SetSL30SPKto] = distItem.SetSL30SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoSL14] = distItem.ExtendtoSL14;
                                CompassListItem[CompassList2Fields.SetSL14SPKto] = distItem.SetSL14SPKto;
                                #endregion
                                #region FERQ DCs
                                CompassListItem[CompassList2Fields.ExtendtoFQ26] = distItem.ExtendtoFQ26;
                                CompassListItem[CompassList2Fields.SetFQ26SPKto] = distItem.SetFQ26SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoFQ27] = distItem.ExtendtoFQ27;
                                CompassListItem[CompassList2Fields.SetFQ27SPKto] = distItem.SetFQ27SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoFQ28] = distItem.ExtendtoFQ28;
                                CompassListItem[CompassList2Fields.SetFQ28SPKto] = distItem.SetFQ28SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoFQ29] = distItem.ExtendtoFQ29;
                                CompassListItem[CompassList2Fields.SetFQ29SPKto] = distItem.SetFQ29SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoFQ34] = distItem.ExtendtoFQ34;
                                CompassListItem[CompassList2Fields.SetFQ34SPKto] = distItem.SetFQ34SPKto;
                                CompassListItem[CompassList2Fields.ExtendtoFQ35] = distItem.ExtendtoFQ35;
                                CompassListItem[CompassList2Fields.SetFQ35SPKto] = distItem.SetFQ35SPKto;
                                #endregion

                                CompassListItem[CompassList2Fields.ModifiedBy] = SPContext.Current.Web.CurrentUser.ToString();
                                CompassListItem[CompassList2Fields.ModifiedDate] = DateTime.Now.ToString();
                                CompassListItem.Update();
                            }
                            #endregion
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #region Distribution Approvals
        public ApprovalItem GetDistributionApprovalItem(int itemId)
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
                            appItem.StartDate = Convert.ToString(item[ApprovalListFields.Distribution_StartDate]);
                            appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.Distribution_ModifiedDate]);
                            appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.Distribution_ModifiedBy]);
                            appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedDate]);
                            appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.Distribution_SubmittedBy]);
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdateDistributionApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.Distribution_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.Distribution_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.Distribution_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.Distribution_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.Distribution_ModifiedDate] = approvalItem.ModifiedDate;
                                }
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetDistributionStartDate(int compassListItemId, DateTime startDate)
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
                                if (item[ApprovalListFields.Distribution_StartDate] == null)
                                {
                                    item[ApprovalListFields.Distribution_StartDate] = startDate.ToString();
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
