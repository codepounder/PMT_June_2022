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
    public class MixesService : IMixesService
    {
        public List<MixesItem> GetMixesItems(int itemId)
        {
            var newItem = new List<MixesItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol;
                    compassItemCol = spList.GetItems(spQuery);

                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            if (string.Equals(item[MixesListFields.MixDeleted], "Yes"))
                                continue;

                            MixesItem objMixesItem = new MixesItem();
                            objMixesItem.ItemId = item.ID;
                            objMixesItem.CompassListItemId = Convert.ToInt32(item[MixesListFields.CompassListItemId]);
                            objMixesItem.ItemNumber = Convert.ToString(item[MixesListFields.ItemNumber]);
                            objMixesItem.ItemDescription = Convert.ToString(item[MixesListFields.ItemDescription]);
                            try
                            {
                                objMixesItem.NumberOfPieces = Convert.ToDouble(item[MixesListFields.NumberOfPieces]);
                            }
                            catch
                            {
                                objMixesItem.NumberOfPieces = 0;
                            }
                            try
                            {
                                objMixesItem.OuncesPerPiece = Convert.ToDouble(item[MixesListFields.OuncesPerPiece]);
                            }
                            catch
                            {
                                objMixesItem.OuncesPerPiece = 0;
                            }

                            newItem.Add(objMixesItem);
                        }
                    }
                }
            }
            return newItem;
        }
        public int InsertMixesItem(int compassListItemId, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);

                        SPListItem appItem = spList.AddItem();

                        appItem["Title"] = title;
                        appItem[MixesListFields.CompassListItemId] = compassListItemId;

                        appItem.Update();
                        spWeb.AllowUnsafeUpdates = false;

                        id = appItem.ID;
                    }
                }
            });
            return id;
        }
        public void UpsertMixesItem(List<MixesItem> pmItems, string projectNumber)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);
                        foreach (MixesItem pmItem in pmItems)
                        {
                            if (pmItem.ItemId < 0)// insert
                            {
                                SPListItem item = spList.AddItem();
                                item[MixesListFields.ItemDescription] = pmItem.ItemDescription;
                                item[MixesListFields.ItemNumber] = pmItem.ItemNumber;
                                item[MixesListFields.OuncesPerPiece] = pmItem.OuncesPerPiece;
                                item[MixesListFields.NumberOfPieces] = pmItem.NumberOfPieces;

                                item[MixesListFields.MixDeleted] = pmItem.MixDeleted;
                                item[MixesListFields.CompassListItemId] = pmItem.CompassListItemId;
                                // Set Modified By to current user NOT System Account
                                item["Editor"] = SPContext.Current.Web.CurrentUser;
                                item.Update();
                            }
                            else if (pmItem.ItemId > 0)
                            {
                                SPListItem item = spList.GetItemById(pmItem.ItemId);
                                if (item != null)
                                {
                                    item[MixesListFields.ItemDescription] = pmItem.ItemDescription;
                                    item[MixesListFields.ItemNumber] = pmItem.ItemNumber;
                                    item[MixesListFields.OuncesPerPiece] = pmItem.OuncesPerPiece;
                                    item[MixesListFields.NumberOfPieces] = pmItem.NumberOfPieces;

                                    item[MixesListFields.MixDeleted] = pmItem.MixDeleted;
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
        public void CopyMixesItem(int copyId, int newItemId)
        {
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"" + MixesListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + copyId + "</Value></Eq></Where>";
                    var copyItems = spList.GetItems(spQuery);


                    if (copyItems != null)
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        foreach (SPListItem copyItem in copyItems)
                        {
                            if ((copyItem != null) && !(string.Equals(copyItem[MixesListFields.MixDeleted], "Yes")))
                            {
                                var newItem = spList.AddItem();

                                newItem[MixesListFields.CompassListItemId] = newItemId;
                                newItem[MixesListFields.ItemDescription] = Convert.ToString(copyItem[MixesListFields.ItemDescription]);
                                newItem[MixesListFields.ItemNumber] = Convert.ToString(copyItem[MixesListFields.ItemNumber]);
                                newItem[MixesListFields.NumberOfPieces] = Convert.ToString(copyItem[MixesListFields.NumberOfPieces]);
                                newItem[MixesListFields.OuncesPerPiece] = Convert.ToString(copyItem[MixesListFields.OuncesPerPiece]);
                                newItem[MixesListFields.MixDeleted] = Convert.ToString(copyItem[MixesListFields.MixDeleted]);
                                newItem["Editor"] = SPContext.Current.Web.CurrentUser;

                                newItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            }
        }

        public bool DeleteMixesItem(int ItemId, string webUrl)
        {
            bool isDeleted = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);

                        SPListItem item = spList.GetItemById(ItemId);
                        if (item != null)
                        {
                            item[MixesListFields.MixDeleted] = "Yes";

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

        public bool DeleteAllMixesItems(int ItemId, string webUrl)
        {
            bool isDeleted = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(webUrl))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassMixesListName);

                        SPListItem item = spList.GetItemById(ItemId);
                        if (item != null)
                        {
                            item[MixesListFields.MixDeleted] = "Yes";

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
