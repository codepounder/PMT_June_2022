using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using System.Web;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.Services
{
    public class MaterialsReceivedService : IMaterialsReceivedService
    {
        private readonly IUtilityService utilityService;
        private readonly IExceptionService exceptionService;

        public MaterialsReceivedService(IUtilityService utilityService, IExceptionService exceptionService)
        {
            this.utilityService = utilityService;
            this.exceptionService = exceptionService;
        }
        public List<MaterialsReceivedItem> getMaterialsReceivedItem(int compassID)
        {
            List<MaterialsReceivedItem> materialsReceivedList = new List<MaterialsReceivedItem>();
            List<string> materialNumbers = new List<string>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassID + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            MaterialsReceivedItem materialsReceivedItem = new MaterialsReceivedItem();
                            materialsReceivedItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            materialsReceivedItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            materialsReceivedItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            materialNumbers.Add(Convert.ToString(item[PackagingItemListFields.MaterialNumber]));
                            materialsReceivedList.Add(materialsReceivedItem);

                        }
                    }
                    SPList spSAPList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPHanaStatusListName);
                    SPQuery spSAPQuery = new SPQuery();
                    if (materialNumbers.Count > 0)
                    {
                        List<MaterialsReceivedItem> newMaterialsReceivedList = new List<MaterialsReceivedItem>();
                        string createQuery = "<Where><In><FieldRef Name=\"Title\" /><Values>";
                        foreach(string matNumber in materialNumbers)
                        {
                            createQuery = createQuery + "<Value Type=\"Text\">" + matNumber + "</Value>";
                        }
                        createQuery = createQuery + "</Values></In></Where>";
                        spSAPQuery.Query = createQuery;

                        SPListItemCollection materialItemCol = spSAPList.GetItems(spSAPQuery);
                        //if (materialItemCol.Count > 0)
                        //{
                        foreach (MaterialsReceivedItem materialsReceivedItem in materialsReceivedList)
                        {
                            SPListItem item = null;
                            int itemCount = (from SPListItem HanaItem in materialItemCol where Convert.ToString(HanaItem[SAPHanaStatusListFields.Material]) == materialsReceivedItem.MaterialNumber select HanaItem).Count();
                            if (itemCount > 1)
                            {
                                SPList spCompassList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                                SPListItem compassItem = spCompassList.GetItemById(compassID);
                                string compassPlant = "";
                                if (compassItem != null)
                                {
                                    string fullPlant = Convert.ToString(compassItem[CompassListFields.ManufacturingLocation]);
                                    compassPlant = Regex.Match(fullPlant, @"\(([^)]*)\)").Groups[1].Value;
                                    item = (from SPListItem HanaItem in materialItemCol where Convert.ToString(HanaItem[SAPHanaStatusListFields.Material]) == materialsReceivedItem.MaterialNumber && Convert.ToString(HanaItem[SAPHanaStatusListFields.Plant]) == compassPlant select HanaItem).FirstOrDefault();
                                }
                            }
                            if (itemCount == 1 || item == null)
                            {
                                item = (from SPListItem HanaItem in materialItemCol where Convert.ToString(HanaItem[SAPHanaStatusListFields.Material]) == materialsReceivedItem.MaterialNumber select HanaItem).FirstOrDefault();
                            }
                            if (item != null)
                            {
                                materialsReceivedItem.Plant = Convert.ToString(item[SAPHanaStatusListFields.Plant]);
                                materialsReceivedItem.QuantityOfOrder = Convert.ToString(item[SAPHanaStatusListFields.QuantityofOrder]);
                                materialsReceivedItem.CurrentAvailQuantity = Convert.ToString(item[SAPHanaStatusListFields.CurrentAvailableQuantity]);
                                materialsReceivedItem.DateOfOrder = Convert.ToString(item[SAPHanaStatusListFields.DateofOrder]);
                                newMaterialsReceivedList.Add(materialsReceivedItem);
                            }
                        }
                        //}
                        materialsReceivedList = newMaterialsReceivedList;
                    }
                }
            }
            return materialsReceivedList;
        }
    }
}