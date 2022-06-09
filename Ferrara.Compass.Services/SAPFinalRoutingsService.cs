using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Microsoft.SharePoint;

namespace Ferrara.Compass.Services
{
    public class SAPFinalRoutingsService : ISAPFinalRoutingsService
    {
        public List<FinalRoutingsItem> GetSAPFinalRoutingsItems(string sapItemNumber)
        {
            List<FinalRoutingsItem> bomItems = new List<FinalRoutingsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPHanaStatusListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            FinalRoutingsItem newItem = new FinalRoutingsItem();

                            newItem.Plant = Convert.ToString(item[SAPHanaStatusListFields.Plant]);
                            newItem.Material = Convert.ToString(item[SAPHanaStatusListFields.Material]);
                            newItem.BBlockOnItem = Convert.ToString(item[SAPHanaStatusListFields.BBlockOnItem]);
                            newItem.MRPType = Convert.ToString(item[SAPHanaStatusListFields.MRPType]);
                            newItem.POExists = Convert.ToString(item[SAPHanaStatusListFields.POExists]);
                            newItem.SourceListComplete = Convert.ToString(item[SAPHanaStatusListFields.SourceListComplete]);
                            newItem.StandardCostSet = Convert.ToString(item[SAPHanaStatusListFields.StandardCostSet]);
                            newItem.ZBlocksComplete = Convert.ToString(item[SAPHanaStatusListFields.ZBlocksComplete]);
                            newItem.SAPRoutings = Convert.ToString(item[SAPHanaStatusListFields.SAPRoutings]);
                            newItem.CurrentAvailableQuantity = Convert.ToString(item[SAPHanaStatusListFields.CurrentAvailableQuantity]);
                            newItem.DateofFirstProduction = Convert.ToString(item[SAPHanaStatusListFields.DateofFirstProduction]);
                            newItem.QuantityofFirstProduction = Convert.ToString(item[SAPHanaStatusListFields.QuantityofFirstProduction]);
                            newItem.DateofOrder = Convert.ToString(item[SAPHanaStatusListFields.DateofOrder]);
                            newItem.QuantityofOrder = Convert.ToString(item[SAPHanaStatusListFields.QuantityofOrder]);
                            newItem.HANAKey = Convert.ToString(item[SAPHanaStatusListFields.HanaKey]);

                            bomItems.Add(newItem);
                        }
                    }
                }
            }
            return bomItems;
        }
        public PackagingItem getCompassItem(int iItemId)
        {
            PackagingItem newItem = new PackagingItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(iItemId);
                    if(item != null)
                    {
                        newItem.MaterialNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        newItem.MaterialDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        newItem.PackLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        newItem.CompassListItemId = iItemId.ToString();
                    }
                }
            }
            return newItem;
        }
        public FinalRoutingsItem GetSingleSAPFinalRoutingsItem(string sapItemNumber, string packPlant)
        {
            FinalRoutingsItem newItem = new FinalRoutingsItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPHanaStatusListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq><Eq><FieldRef Name=\"Plant\" /><Value Type=\"Text\">" + packPlant + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];

                        newItem.Plant = Convert.ToString(item[SAPHanaStatusListFields.Plant]);
                        newItem.Material = Convert.ToString(item[SAPHanaStatusListFields.Material]);
                        newItem.BBlockOnItem = Convert.ToString(item[SAPHanaStatusListFields.BBlockOnItem]);
                        newItem.MRPType = Convert.ToString(item[SAPHanaStatusListFields.MRPType]);
                        newItem.POExists = Convert.ToString(item[SAPHanaStatusListFields.POExists]);
                        newItem.SourceListComplete = Convert.ToString(item[SAPHanaStatusListFields.SourceListComplete]);
                        newItem.StandardCostSet = Convert.ToString(item[SAPHanaStatusListFields.StandardCostSet]);
                        newItem.ZBlocksComplete = Convert.ToString(item[SAPHanaStatusListFields.ZBlocksComplete]);
                        newItem.SAPRoutings = Convert.ToString(item[SAPHanaStatusListFields.SAPRoutings]);
                        newItem.CurrentAvailableQuantity = Convert.ToString(item[SAPHanaStatusListFields.CurrentAvailableQuantity]);
                        newItem.DateofFirstProduction = Convert.ToString(item[SAPHanaStatusListFields.DateofFirstProduction]);
                        newItem.QuantityofFirstProduction = Convert.ToString(item[SAPHanaStatusListFields.QuantityofFirstProduction]);
                        newItem.DateofOrder = Convert.ToString(item[SAPHanaStatusListFields.DateofOrder]);
                        newItem.QuantityofOrder = Convert.ToString(item[SAPHanaStatusListFields.QuantityofOrder]);
                        newItem.HANAKey = Convert.ToString(item[SAPHanaStatusListFields.HanaKey]);
                        
                    }
                }
            }
            return newItem;
        }
        public string getProjectPackPlant(int iItemId)
        {
            string packPlant = "";
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem listItem = spList.GetItemById(iItemId);
                    if (listItem != null)
                    {
                        packPlant = Convert.ToString(listItem[CompassListFields.PackingLocation]);
                    }
                }
            }
            return packPlant;
        }
        public SAPApprovalListItem getSAPApprovalItem(int iItemId)
        {
            SAPApprovalListItem SAPApprovalItem = new SAPApprovalListItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPApprovalListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + iItemId + "</Value></Eq></Where>";
                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    SPListItem item;
                    if (compassItemCol != null && compassItemCol.Count > 0)
                    {
                        item = compassItemCol[0];
                        SAPApprovalItem.SAPRoutingSetup_StartDate = Convert.ToString(item[SAPApprovalListFields.SAPRoutingSetup_StartDate]);
                        SAPApprovalItem.SAPRoutingSetup_ModifiedDate = Convert.ToString(item[SAPApprovalListFields.SAPRoutingSetup_ModifiedDate]);
                        SAPApprovalItem.SAPRoutingSetup_SubmittedDate = Convert.ToString(item[SAPApprovalListFields.SAPRoutingSetup_SubmittedDate]);
                        SAPApprovalItem.SAPCostingDetails_StartDate = Convert.ToString(item[SAPApprovalListFields.SAPCostingDetails_StartDate]);
                        SAPApprovalItem.SAPCostingDetails_ModifiedDate = Convert.ToString(item[SAPApprovalListFields.SAPCostingDetails_ModifiedDate]);
                        SAPApprovalItem.SAPCostingDetails_SubmittedDate = Convert.ToString(item[SAPApprovalListFields.SAPCostingDetails_SubmittedDate]);
                        SAPApprovalItem.SAPWarehouseInfo_StartDate = Convert.ToString(item[SAPApprovalListFields.SAPWarehouseInfo_StartDate]);
                        SAPApprovalItem.SAPWarehouseInfo_ModifiedDate = Convert.ToString(item[SAPApprovalListFields.SAPWarehouseInfo_ModifiedDate]);
                        SAPApprovalItem.SAPWarehouseInfo_SubmittedDate = Convert.ToString(item[SAPApprovalListFields.SAPWarehouseInfo_SubmittedDate]);
                        SAPApprovalItem.StandardCostEntry_StartDate = Convert.ToString(item[SAPApprovalListFields.StandardCostEntry_StartDate]);
                        SAPApprovalItem.StandardCostEntry_ModifiedDate = Convert.ToString(item[SAPApprovalListFields.StandardCostEntry_ModifiedDate]);
                        SAPApprovalItem.StandardCostEntry_SubmittedDate = Convert.ToString(item[SAPApprovalListFields.StandardCostEntry_SubmittedDate]);

                    }
                }
            }
            return SAPApprovalItem;
        }

    }

}

    



