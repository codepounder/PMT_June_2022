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
    public class SAPMaterialMasterService : ISAPMaterialMasterService
    {
        public SAPMaterialMasterListItem GetSAPMaterialMaster(string sapItemNumber)
        {
            var newItem = new SAPMaterialMasterListItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_SAPMaterialMasterListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">" + sapItemNumber + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        var item = compassItemCol[0];
                        if (item != null)
                        {
                            newItem.SAPItemNumber = Convert.ToString(item["Title"]);
                            newItem.SAPDescription = Convert.ToString(item[SAPMaterialMasterListFields.SAPDescription]);
                            newItem.CaseType = Convert.ToString(item[SAPMaterialMasterListFields.CaseType]);

                            newItem.CandySemiNumber = Convert.ToString(item[SAPMaterialMasterListFields.CandySemiNumber]);
                            newItem.TruckLoadPricePerSellingUnit = Convert.ToString(item[SAPMaterialMasterListFields.TruckLoadPricePerSellingUnit]);
                            newItem.Last12MonthSales = Convert.ToString(item[SAPMaterialMasterListFields.Last12MonthSales]);

                            newItem.ProductHierarchyLevel1 = string.Concat(Convert.ToString(item[SAPMaterialMasterListFields.ProductHierarchyLevel1]), " (",
                                        Convert.ToString(item[SAPMaterialMasterListFields.ProductHierarchyLevel1Code]),")");
                            newItem.ProductHierarchyLevel2 = string.Concat(Convert.ToString(item[SAPMaterialMasterListFields.ProductHierarchyLevel2]).ToUpper(), " (",
                                        Convert.ToString(item[SAPMaterialMasterListFields.ProductHierarchyLevel2Code]),")");
                            newItem.MaterialGroup1Brand = string.Concat(Convert.ToString(item[SAPMaterialMasterListFields.MaterialGroup1Brand]), " (",
                                        Convert.ToString(item[SAPMaterialMasterListFields.MaterialGroup1BrandCode]), ")");
                            newItem.MaterialGroup4ProductForm = string.Concat(Convert.ToString(item[SAPMaterialMasterListFields.MaterialGroup4ProductForm]).ToUpper(), " (",
                                        Convert.ToString(item[SAPMaterialMasterListFields.MaterialGroup4ProductFormCode]),")");
                            newItem.MaterialGroup5PackType = string.Concat(Convert.ToString(item[SAPMaterialMasterListFields.MaterialGroup5PackType]).ToUpper(), " (",
                                        Convert.ToString(item[SAPMaterialMasterListFields.MaterialGroup5PackTypeCode]),")");

                            newItem.RetailSellingUnitsBaseUOM = Convert.ToString(item[SAPMaterialMasterListFields.RetailSellingUnitsBaseUOM]);
                            newItem.RetailUnitWieghtOz = Convert.ToString(item[SAPMaterialMasterListFields.RetailUnitWieghtOz]);

                            newItem.CaseUCC = Convert.ToString(item[SAPMaterialMasterListFields.CaseUCC]);
                            newItem.DisplayBoxUPC = Convert.ToString(item[SAPMaterialMasterListFields.DisplayBoxUPC]);
                            newItem.PalletUCC = Convert.ToString(item[SAPMaterialMasterListFields.PalletUCC]);
                            newItem.UnitUPC = Convert.ToString(item[SAPMaterialMasterListFields.UnitUPC]);
                            newItem.MaterialType = Convert.ToString(item[SAPMaterialMasterListFields.MaterialType]);
                            newItem.MaterialType2 = Convert.ToString(item[SAPMaterialMasterListFields.MaterialType2]);
                        }
                    }
                }
            }
            return newItem;
        }
    }
}
