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
    public class QAService : IQAService
    {
        public QAItem GetQAItem(int itemId)
        {
            QAItem sgItem = new QAItem();
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

                        // QA Fields
                        sgItem.NewFormula = Convert.ToString(item[CompassListFields.NewFormula]);
                        sgItem.CoManufacturingClassification = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        sgItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        sgItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);

                        sgItem.MarketingClaimsLabeling = Convert.ToString(item[CompassListFields.MarketClaimsLabelingRequirements]);
                        sgItem.PM = Convert.ToString(item[CompassListFields.PM]);
                        sgItem.PMName = Convert.ToString(item[CompassListFields.PMName]);
                        sgItem.CaseType = Convert.ToString(item[CompassListFields.CaseType]);

                        sgItem.ManufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.MakeCountryOfOrigin = Convert.ToString(item[CompassListFields.ManufacturerCountryOfOrigin]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                    }
                    var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    var ipItems = spList2.GetItems(spQuery);

                    if (ipItems != null && ipItems.Count > 0)
                    {
                        var ipItem = ipItems[0];
                        sgItem.Marketing = Convert.ToString(ipItem[CompassTeamListFields.Marketing]);
                        sgItem.MarketingName = Convert.ToString(ipItem[CompassTeamListFields.MarketingName]);
                    }
                    var spCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spCompassList2Query = new SPQuery();
                    spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    var CompassList2Items = spCompassList2.GetItems(spQuery);

                    if (CompassList2Items != null && CompassList2Items.Count > 0)
                    {
                        var CompassList2Item = CompassList2Items[0];
                        sgItem.IsRegulatoryinformationCorrect = Convert.ToString(CompassList2Item[CompassList2Fields.IsRegulatoryinformationCorrect]);
                        sgItem.WhatRegulatoryInfoIsIncorrect = Convert.ToString(CompassList2Item[CompassList2Fields.WhatRegulatoryInfoIsIncorrect]);
                        sgItem.DoYouApproveThisProjectToProceed = Convert.ToString(CompassList2Item[CompassList2Fields.DoYouApproveThisProjectToProceed]);
                    }
                }
            }
            return sgItem;
        }
        public void UpdateMarketingClaimsItem(MarketingClaimsItem ipItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_MarketingClaimsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + ipItem.CompassListItemId + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item;
                        if (compassItemCol.Count > 0)
                        {
                            item = compassItemCol[0];
                        }
                        else
                        {
                            item = spList.AddItem();
                            item["Title"] = ipItem.Title;
                            item[MarketingClaimsListFields.CompassListItemId] = ipItem.CompassListItemId;
                        }

                        item[MarketingClaimsListFields.AllergenMilk] = ipItem.AllergenMilk;
                        item[MarketingClaimsListFields.AllergenEggs] = ipItem.AllergenEggs;
                        item[MarketingClaimsListFields.AllergenPeanuts] = ipItem.AllergenPeanuts;
                        item[MarketingClaimsListFields.AllergenCoconut] = ipItem.AllergenCoconut;
                        item[MarketingClaimsListFields.AllergenAlmonds] = ipItem.AllergenAlmonds;
                        item[MarketingClaimsListFields.AllergenSoy] = ipItem.AllergenSoy;
                        item[MarketingClaimsListFields.AllergenWheat] = ipItem.AllergenWheat;
                        item[MarketingClaimsListFields.AllergenHazelNuts] = ipItem.AllergenHazelNuts;
                        item[MarketingClaimsListFields.AllergenOther] = ipItem.AllergenOther;
                        item[MarketingClaimsListFields.ClaimBioEngineering] = ipItem.ClaimBioEngineering;

                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;

                        item.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }

                }
            });
        }
        public void UpdateRegulatoryItem(QAItem qaItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (var spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (var spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Text\">" + qaItem.CompassListItemId + "</Value></Eq></Where>";

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item;
                        if (compassItemCol.Count > 0)
                        {
                            item = compassItemCol[0];
                        }
                        else
                        {
                            item = spList.AddItem();
                            item[CompassList2Fields.CompassListItemId] = qaItem.CompassListItemId;
                        }

                        item[CompassList2Fields.IsRegulatoryinformationCorrect] = qaItem.IsRegulatoryinformationCorrect;
                        item[CompassList2Fields.WhatRegulatoryInfoIsIncorrect] = qaItem.WhatRegulatoryInfoIsIncorrect;
                        item[CompassList2Fields.DoYouApproveThisProjectToProceed] = qaItem.DoYouApproveThisProjectToProceed;

                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;

                        item.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }

                }
            });
        }
        public List<PackagingItem> GetCandySemiForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_CandySemi + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            packagingItem.IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetFinishedGoodItems(int ItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + ItemId + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes") || (!Convert.ToString(item[PackagingItemListFields.PackagingComponent]).Contains("Finished Good")))
                                continue;
                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            //packagingItem.CostingUnit = Convert.ToString(item[PackagingItemListFields.CostingUnit]);
                            //packagingItem.LBPerCostingUnit = Convert.ToString(item[PackagingItemListFields.LBPerCostingUnit]);
                            packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public void UpdateFinishedGoodShelfLife(List<PackagingItem> FGItems, string projectNumber)
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
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        foreach (PackagingItem FGItem in FGItems)
                        {
                            if (FGItem.Id > 0)
                            {
                                SPListItem item = spList.GetItemById(FGItem.Id);
                                if (item != null)
                                {
                                    item[PackagingItemListFields.ShelfLife] = FGItem.ShelfLife;
                                    item[PackagingItemListFields.IngredientsNeedToClaimBioEng] = FGItem.IngredientsNeedToClaimBioEng;
                                    item[PackagingItemListFields.LastFormUpdated] = GlobalConstants.PAGE_QA;
                                    // Set Modified By to current user NOT System Account
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
        public List<PackagingItem> GetPurchasedCandySemiForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_PurchasedSemi + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            packagingItem.ImmediateSPKChange = Convert.ToString(item[PackagingItemListFields.ImmediateSPKChange]);
                            packagingItem.IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetTransferSemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public int InsertPackagingItem(PackagingItem packagingItem, int compassListItemId, string projectNumber)
        {
            int itemId = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.AddItem();
                        item["Title"] = projectNumber;// utilityService.GetProjectNumberFromItemId(compassListItemId, SPContext.Current.Web.Url);
                        item[PackagingItemListFields.CompassListItemId] = compassListItemId;
                        item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                        item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                        item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                        item[PackagingItemListFields.ParentID] = packagingItem.ParentID;
                        item[PackagingItemListFields.MakeLocation] = packagingItem.MakeLocation;
                        item[PackagingItemListFields.PackLocation] = packagingItem.PackLocation;
                        item[PackagingItemListFields.CountryOfOrigin] = packagingItem.CountryOfOrigin;
                        item[PackagingItemListFields.NewFormula] = packagingItem.NewFormula;
                        item[PackagingItemListFields.TrialsCompleted] = packagingItem.TrialsCompleted;
                        item[PackagingItemListFields.ShelfLife] = packagingItem.ShelfLife;

                        SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                        if (user != null)
                        {
                            // Set Modified By to current user NOT System Account
                            item["Created By"] = user.ID;
                            item["Modified By"] = user.ID;
                        }
                        item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                        item.Update();

                        itemId = item.ID;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return itemId;
        }
        public void UpdatePackagingItem(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                            item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                            item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                            item[PackagingItemListFields.NewExisting] = packagingItem.NewExisting;
                            //item[PackagingItemListFields.TransferSEMIMakePackLocations] = packagingItem.TransferSEMIMakePackLocations;
                            item[PackagingItemListFields.NewFormula] = packagingItem.NewFormula;
                            item[PackagingItemListFields.TrialsCompleted] = packagingItem.TrialsCompleted;
                            item[PackagingItemListFields.ShelfLife] = packagingItem.ShelfLife;
                            item[PackagingItemListFields.Flowthrough] = packagingItem.Flowthrough;
                            item[PackagingItemListFields.IngredientsNeedToClaimBioEng] = packagingItem.IngredientsNeedToClaimBioEng;
                            item[PackagingItemListFields.ParentID] = packagingItem.ParentID;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #region Approval Methods
        public void UpdateQAApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.QA_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.QA_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.QA_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.QA_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.QA_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetQAStartDate(int compassListItemId, DateTime startDate, string title)
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
                                if (item[ApprovalListFields.QA_StartDate] == null)
                                {
                                    item[ApprovalListFields.QA_StartDate] = startDate.ToString();
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
