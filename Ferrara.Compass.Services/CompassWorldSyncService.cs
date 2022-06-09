using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;

namespace Ferrara.Compass.Services
{
    public class CompassWorldSyncService : ICompassWorldSyncService
    {
        public CompassWorldSyncListItem GetCompassWorldSyncListItem(int compassItemId, int childId)
        {
            CompassWorldSyncListItem worldSync = new CompassWorldSyncListItem(); ;
            SPListItemCollection compassItemCol;
            SPQuery spQuery;
            SPList spList;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorldSyncList);
                    spQuery = new SPQuery();
                    if(childId == 0) {
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassItemId + "</Value></Eq><Eq><FieldRef Name=\"ParentGlobalId\" /><Value Type=\"Int\">0</Value></Eq></And></Where>";
                    } else {
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassItemId + "</Value></Eq><Eq><FieldRef Name=\"ID\" /><Value Type=\"Counter\">" + childId + "</Value></Eq></And></Where>";
                    }
                    compassItemCol = spList.GetItems(spQuery);
                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            worldSync.Id = item.ID;
                            worldSync.ParentId = Convert.ToInt32(item[CompassWorldSyncListFields.ParentGlobalId]);
                            worldSync.CompassListItemId = Convert.ToInt32(item[CompassWorldSyncListFields.CompassListItemId]);
                            worldSync.TargetMarket = Convert.ToString(item[CompassWorldSyncListFields.TargetMarket]);
                            worldSync.ProductType = Convert.ToString(item[CompassWorldSyncListFields.ProductType]);
                            worldSync.GPCClassification = Convert.ToString(item[CompassWorldSyncListFields.GPCClassification]);
                            worldSync.BrandOwnerGLN = Convert.ToString(item[CompassWorldSyncListFields.BrandOwnerGLN]);
                            worldSync.BaseUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.BaseUnitIndicator]);
                            worldSync.ConsumerUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.ConsumerUnitIndicator]);
                            worldSync.AlternateClassificationScheme = Convert.ToString(item[CompassWorldSyncListFields.AlternateClassificationScheme]);
                            worldSync.Code = Convert.ToString(item[CompassWorldSyncListFields.Code]);
                            worldSync.AlternateItemIdAgency = Convert.ToString(item[CompassWorldSyncListFields.AlternateItemIdAgency]);
                            worldSync.GS1TradeItemsIDKeyCode = Convert.ToString(item[CompassWorldSyncListFields.GS1TradeItemsIDKeyCode]);
                            worldSync.OrderingUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.OrderingUnitIndicator]);
                            worldSync.DispatchUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.DispatchUnitIndicator]);
                            worldSync.InvoiceUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.InvoiceUnitIndicator]);
                            worldSync.DataCarrierTypeCode = Convert.ToString(item[CompassWorldSyncListFields.DataCarrierTypeCode]);
                            worldSync.TradeChannel = Convert.ToString(item[CompassWorldSyncListFields.TradeChannel]);
                            worldSync.TemperatureQualitiferCode = Convert.ToString(item[CompassWorldSyncListFields.TemperatureQualitiferCode]);
                            worldSync.CustomerBrandName = Convert.ToString(item[CompassWorldSyncListFields.CustomerBrandName]);
                            worldSync.NetContent = Convert.ToString(item[CompassWorldSyncListFields.NetContent]);
                            worldSync.QtyOfNextLevelItems = Convert.ToString(item[CompassWorldSyncListFields.QtyOfNextLevelItems]);
                        }
                    }
                }
            }
            return worldSync;
        }
        public List<CompassWorldSyncListItem> GetCompassWorldSyncChildListItems(int compassItemId)
        {
            CompassWorldSyncListItem worldSync = null;
            List<CompassWorldSyncListItem> worldSyncList = new List<CompassWorldSyncListItem>();
            SPListItemCollection compassItemCol;
            SPQuery spQuery;
            SPList spList;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorldSyncList);
                    spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassItemId + "</Value></Eq><Neq><FieldRef Name=\"ParentGlobalId\" /><Value Type=\"Int\">0</Value></Neq></And></Where>";
                    compassItemCol = spList.GetItems(spQuery);
                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            worldSync = new CompassWorldSyncListItem
                            {
                                Id = item.ID,
                                ParentId = Convert.ToInt32(item[CompassWorldSyncListFields.ParentGlobalId]),
                                CompassListItemId = Convert.ToInt32(item[CompassWorldSyncListFields.CompassListItemId]),
                                TargetMarket = Convert.ToString(item[CompassWorldSyncListFields.TargetMarket]),
                                ProductType = Convert.ToString(item[CompassWorldSyncListFields.ProductType]),
                                GPCClassification = Convert.ToString(item[CompassWorldSyncListFields.GPCClassification]),
                                BrandOwnerGLN = Convert.ToString(item[CompassWorldSyncListFields.BrandOwnerGLN]),
                                BaseUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.BaseUnitIndicator]),
                                ConsumerUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.ConsumerUnitIndicator]),
                                AlternateClassificationScheme = Convert.ToString(item[CompassWorldSyncListFields.AlternateClassificationScheme]),
                                Code = Convert.ToString(item[CompassWorldSyncListFields.Code]),
                                AlternateItemIdAgency = Convert.ToString(item[CompassWorldSyncListFields.AlternateItemIdAgency]),
                                GS1TradeItemsIDKeyCode = Convert.ToString(item[CompassWorldSyncListFields.GS1TradeItemsIDKeyCode]),
                                OrderingUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.OrderingUnitIndicator]),
                                DispatchUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.DispatchUnitIndicator]),
                                InvoiceUnitIndicator = Convert.ToString(item[CompassWorldSyncListFields.InvoiceUnitIndicator]),
                                DataCarrierTypeCode = Convert.ToString(item[CompassWorldSyncListFields.DataCarrierTypeCode]),
                                TradeChannel = Convert.ToString(item[CompassWorldSyncListFields.TradeChannel]),
                                TemperatureQualitiferCode = Convert.ToString(item[CompassWorldSyncListFields.TemperatureQualitiferCode]),
                                CustomerBrandName = Convert.ToString(item[CompassWorldSyncListFields.CustomerBrandName]),
                                NetContent = Convert.ToString(item[CompassWorldSyncListFields.NetContent]),
                                QtyOfNextLevelItems = Convert.ToString(item[CompassWorldSyncListFields.QtyOfNextLevelItems]),
                            };
                            worldSyncList.Add(worldSync);
                        }
                    }
                }
            }
            return worldSyncList;
        }
        public int UpsertCompassWorldSyncListItem(CompassWorldSyncListItem worldSync)
        {
            SPListItem item;
            int id = 0;
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorldSyncList);
                        if (worldSync.Id <= 0) // insert
                        {
                            item = spList.AddItem();
                            item[CompassWorldSyncListFields.CompassListItemId] = worldSync.CompassListItemId;
                            item[CompassWorldSyncListFields.GPCClassification] = "10000047";
                        }
                        else
                            item = spList.GetItemById(worldSync.Id);
                        item[CompassWorldSyncListFields.ParentGlobalId] = worldSync.ParentId;
                        item[CompassWorldSyncListFields.TargetMarket] = worldSync.TargetMarket;
                        item[CompassWorldSyncListFields.ProductType] = worldSync.ProductType;
                        item[CompassWorldSyncListFields.BrandOwnerGLN] = worldSync.BrandOwnerGLN;
                        item[CompassWorldSyncListFields.BaseUnitIndicator] = worldSync.BaseUnitIndicator;
                        item[CompassWorldSyncListFields.ConsumerUnitIndicator] = worldSync.ConsumerUnitIndicator;
                        item[CompassWorldSyncListFields.AlternateClassificationScheme] = worldSync.AlternateClassificationScheme;
                        item[CompassWorldSyncListFields.Code] = worldSync.Code;
                        item[CompassWorldSyncListFields.AlternateItemIdAgency] = worldSync.AlternateItemIdAgency;
                        item[CompassWorldSyncListFields.GS1TradeItemsIDKeyCode] = worldSync.GS1TradeItemsIDKeyCode;
                        item[CompassWorldSyncListFields.OrderingUnitIndicator] = worldSync.OrderingUnitIndicator;
                        item[CompassWorldSyncListFields.DispatchUnitIndicator] = worldSync.DispatchUnitIndicator;
                        item[CompassWorldSyncListFields.InvoiceUnitIndicator] = worldSync.InvoiceUnitIndicator;
                        item[CompassWorldSyncListFields.DataCarrierTypeCode] = worldSync.DataCarrierTypeCode;
                        item[CompassWorldSyncListFields.TradeChannel] = worldSync.TradeChannel;
                        item[CompassWorldSyncListFields.TemperatureQualitiferCode] = worldSync.TemperatureQualitiferCode;
                        item[CompassWorldSyncListFields.CustomerBrandName] = worldSync.CustomerBrandName;
                        item[CompassWorldSyncListFields.QtyOfNextLevelItems] = worldSync.QtyOfNextLevelItems;
                        item[CompassWorldSyncListFields.NetContent] = worldSync.NetContent;
                        
                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;
                        item.Update();
                        id = item.ID;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return id;
        }
        public void DeleteWorldSyncDetailItem(int deletedId)
        {
            SPList spList;
            SPListItem itemD;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorldSyncList);
                        itemD = spList.GetItemById(deletedId);
                        itemD.Delete();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}
