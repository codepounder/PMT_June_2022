using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;

namespace Ferrara.Compass.Services
{
    public class ShipperFinishedGoodService : IShipperFinishedGoodService
    {
        public List<ShipperFinishedGoodItem> GetShipperFinishedGoodItems(int itemId)
        {
            var pmItem = new List<ShipperFinishedGoodItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol;

                    compassItemCol = spList.GetItems(spQuery);

                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            if (string.Equals(item[ShipperFinishedGoodListFields.FGDeleted], "Yes"))
                                continue;

                            var objShipperFinishedGoodItem = new ShipperFinishedGoodItem
                            {
                                ItemId = item.ID,
                                CompassListItemId = Convert.ToInt32(item[ShipperFinishedGoodListFields.CompassListItemId]),
                                FGItemDescription = Convert.ToString(item[ShipperFinishedGoodListFields.FGItemDescription]),
                                FGItemNumber = Convert.ToString(item[ShipperFinishedGoodListFields.FGItemNumber]),
                                FGItemNumberUnits = Convert.ToInt32(item[ShipperFinishedGoodListFields.FGItemNumberUnits]),
                                FGItemOuncesPerUnit = Convert.ToDouble(item[ShipperFinishedGoodListFields.FGItemOuncesPerUnit]),
                                FGPackUnit = Convert.ToString(item[ShipperFinishedGoodListFields.FGPackUnit]),
                                FGShelfLife = Convert.ToString(item[ShipperFinishedGoodListFields.FGShelfLife]),
                                IngredientsNeedToClaimBioEng = Convert.ToString(item[ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng])
                            };
                            pmItem.Add(objShipperFinishedGoodItem);
                        }
                    }
                }
            }
            return pmItem;
        }
        public int InsertShipperFinishedGoodItem(int compassListItemId, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);

                        SPListItem appItem = spList.AddItem();

                        appItem["Title"] = title;
                        appItem[ShipperFinishedGoodListFields.CompassListItemId] = compassListItemId;

                        appItem.Update();
                        spWeb.AllowUnsafeUpdates = false;

                        id = appItem.ID;
                    }
                }
            });
            return id;
        }
        public void UpsertShipperFinishedGoodItem(List<ShipperFinishedGoodItem> pmItems, string projectNumber)
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
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);
                        foreach (ShipperFinishedGoodItem pmItem in pmItems)
                        {
                            if (pmItem.ItemId < 0)// insert
                            {
                                SPListItem item = spList.AddItem();
                                item[ShipperFinishedGoodListFields.CompassListItemId] = pmItem.CompassListItemId;
                                item[ShipperFinishedGoodListFields.FGItemDescription] = pmItem.FGItemDescription;
                                item[ShipperFinishedGoodListFields.FGItemNumber] = pmItem.FGItemNumber;
                                item[ShipperFinishedGoodListFields.FGItemNumberUnits] = pmItem.FGItemNumberUnits;
                                item[ShipperFinishedGoodListFields.FGItemOuncesPerUnit] = pmItem.FGItemOuncesPerUnit;
                                item[ShipperFinishedGoodListFields.FGPackUnit] = pmItem.FGPackUnit;
                                item[ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng] = pmItem.IngredientsNeedToClaimBioEng;
                                item[ShipperFinishedGoodListFields.FGDeleted] = pmItem.FGDeleted;
                                // Set Modified By to current user NOT System Account
                                item["Editor"] = SPContext.Current.Web.CurrentUser;
                                item.Update();
                            }
                            else if (pmItem.ItemId > 0)
                            {
                                SPListItem item = spList.GetItemById(pmItem.ItemId);
                                if (item != null)
                                {
                                    item[ShipperFinishedGoodListFields.FGItemDescription] = pmItem.FGItemDescription;
                                    item[ShipperFinishedGoodListFields.FGItemNumber] = pmItem.FGItemNumber;
                                    item[ShipperFinishedGoodListFields.FGItemNumberUnits] = pmItem.FGItemNumberUnits;
                                    item[ShipperFinishedGoodListFields.FGItemOuncesPerUnit] = pmItem.FGItemOuncesPerUnit;
                                    item[ShipperFinishedGoodListFields.FGPackUnit] = pmItem.FGPackUnit;
                                    item[ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng] = pmItem.IngredientsNeedToClaimBioEng;
                                    item[ShipperFinishedGoodListFields.FGDeleted] = pmItem.FGDeleted;
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
        public void UpdateShipperFinishedGoodShelfLife(List<ShipperFinishedGoodItem> pmItems, string projectNumber)
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
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);
                        foreach (ShipperFinishedGoodItem pmItem in pmItems)
                        {
                            if (pmItem.ItemId > 0)
                            {
                                SPListItem item = spList.GetItemById(pmItem.ItemId);
                                if (item != null)
                                {
                                    item[ShipperFinishedGoodListFields.FGShelfLife] = pmItem.FGShelfLife;
                                    item[ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng] = pmItem.IngredientsNeedToClaimBioEng;
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
        public void CopyShipperFinishedGoodItem(int copyId, int newItemId)
        {
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);
                    var copyItem = spList.GetItemById(copyId);
                    var newItem = spList.AddItem();
                    if ((newItem != null) && (copyItem != null))
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        //newItem.ItemId = item.ID;
                        newItem[ShipperFinishedGoodListFields.CompassListItemId] = Convert.ToInt32(copyItem[ShipperFinishedGoodListFields.CompassListItemId]);

                        newItem[ShipperFinishedGoodListFields.FGItemDescription] = Convert.ToString(copyItem[ShipperFinishedGoodListFields.FGItemDescription]);
                        newItem[ShipperFinishedGoodListFields.FGItemNumber] = Convert.ToString(copyItem[ShipperFinishedGoodListFields.FGItemNumber]);
                        newItem[ShipperFinishedGoodListFields.FGItemNumberUnits] = Convert.ToInt32(copyItem[ShipperFinishedGoodListFields.FGItemNumberUnits]);
                        newItem[ShipperFinishedGoodListFields.FGItemOuncesPerUnit] = Convert.ToDouble(ShipperFinishedGoodListFields.FGItemOuncesPerUnit);
                        newItem[ShipperFinishedGoodListFields.FGPackUnit] = Convert.ToString(ShipperFinishedGoodListFields.FGPackUnit);
                        newItem[ShipperFinishedGoodListFields.FGShelfLife] = Convert.ToString(ShipperFinishedGoodListFields.FGShelfLife);
                        newItem[ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng] = Convert.ToString(ShipperFinishedGoodListFields.IngredientsNeedToClaimBioEng);

                        newItem.Update();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
        }
        public bool DeleteShipperFinishedGoodItem(int ItemId, string webUrl)
        {
            bool isDeleted = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassShipperFinishedGoodListName);

                        SPListItem item = spList.GetItemById(ItemId);
                        if (item != null)
                        {
                            item[ShipperFinishedGoodListFields.FGDeleted] = "Yes";

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

    }
}
