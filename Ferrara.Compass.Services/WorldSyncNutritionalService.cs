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
    public class WorldSyncNutritionalService : IWorldSyncNutritionalService
    {
        public List<WorldSyncNutritionalsListDetailItem> GetNutritionalDetailItems(int itemId)
        {
            WorldSyncNutritionalsListDetailItem nutrient;
            SPListItemCollection compassItemCol;
            SPList spList;
            SPQuery spQuery;
            var pmItem = new List<WorldSyncNutritionalsListDetailItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncNutritionalsDetail);
                    spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    compassItemCol = spList.GetItems(spQuery);
                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            nutrient = new WorldSyncNutritionalsListDetailItem();
                            nutrient.Id = item.ID;
                            nutrient.CompassListItemId = Convert.ToInt32(item[WorldSyncNutritionalsDetailFields.CompassListItemId]);
                            nutrient.NutrientQtyContained = Convert.ToInt32(item[WorldSyncNutritionalsDetailFields.NutrientQtyContained]);
                            nutrient.NutrientQtyContainedUOM = Convert.ToString(item[WorldSyncNutritionalsDetailFields.NutrientQtyContainedUOM]);
                            nutrient.NutrientType = Convert.ToString(item[WorldSyncNutritionalsDetailFields.NutrientType]);
                            nutrient.PctDailyValue = Convert.ToInt32(item[WorldSyncNutritionalsDetailFields.PctDailyValue]);
                            nutrient.NutrientQtyContainedMeasPerc = Convert.ToString(item[WorldSyncNutritionalsDetailFields.NutrientQtyContainedMeasPerc]);
                            nutrient.DailyValueIntakePct = Convert.ToString(item[WorldSyncNutritionalsDetailFields.DailyValueIntakePct]);

                            pmItem.Add(nutrient);
                        }
                    }
                }
            }
            return pmItem;
        }

        public int UpsertWorldSyncNutritionalsListDetailItem(int CompassListItemId, WorldSyncNutritionalsListDetailItem pmItem)
        {
            int nutrientId = 0;
            SPList spList;
            SPListItem item;
            // Get the current user
            SPUser currentUser = SPContext.Current.Web.CurrentUser;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncNutritionalsDetail);
                        if (pmItem.Id <= 0)// insert
                        {
                            item = spList.AddItem();
                            item[WorldSyncNutritionalsDetailFields.CompassListItemId] = pmItem.CompassListItemId;
                        }
                        else
                            item = spList.GetItemById(pmItem.Id);
                        item[WorldSyncNutritionalsDetailFields.NutrientType] = pmItem.NutrientType;
                        item[WorldSyncNutritionalsDetailFields.NutrientQtyContained] = pmItem.NutrientQtyContained;
                        item[WorldSyncNutritionalsDetailFields.NutrientQtyContainedUOM] = pmItem.NutrientQtyContainedUOM;
                        item[WorldSyncNutritionalsDetailFields.PctDailyValue] = pmItem.PctDailyValue;
                        item[WorldSyncNutritionalsDetailFields.NutrientQtyContainedMeasPerc] = pmItem.NutrientQtyContainedMeasPerc;
                        item[WorldSyncNutritionalsDetailFields.DailyValueIntakePct] = pmItem.DailyValueIntakePct;
                        // Set Modified By to current user NOT System Account
                        item["Editor"] = SPContext.Current.Web.CurrentUser;
                        item.Update();
                        nutrientId = item.ID;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return nutrientId;
        }

        public void DeleteNutritionalDetailItem(int deletedId)
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
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncNutritionalsDetail);
                        itemD = spList.GetItemById(deletedId);
                        itemD.Delete();
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        private string getNutritionalDetailItemsDeletedQuery(int CompassListItemId, List<int> updatedIds)
        {
            StringBuilder sb;
            int w;
            sb = new StringBuilder();
            sb.Append("<Where>");
            for(w = 0; w < updatedIds.Count; w++)
                sb.Append("<And>");
            sb.Append("<Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Integer\">" + CompassListItemId + "</Value></Eq>");
            foreach (int updatedId in updatedIds)
                sb.Append("<Neq><FieldRef Name=\"ID\" /><Value Type=\"Integer\">" + updatedId + "</Value></Neq></And>");
            sb.Append("</Where>");
            return sb.ToString();
        }

        public WorldSyncNutritionalsListItem GetNutritionalItem(int itemId)
        {
            SPListItemCollection compassItemCol;
            SPQuery spQuery;
            SPList spList;
            WorldSyncNutritionalsListItem nutrient = null;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncNutritionals);
                    spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    compassItemCol = spList.GetItems(spQuery);
                    foreach (SPListItem item in compassItemCol)
                    {
                        if (item != null)
                        {
                            nutrient = new WorldSyncNutritionalsListItem
                            {
                                Id = item.ID,
                                CompassListItemId = Convert.ToInt32(item[WorldSyncNutritionalsFields.CompassListItemId]),
                                NutrientBasisQty = Convert.ToString(item[WorldSyncNutritionalsFields.NutrientBasisQty]),
                                NutrientBasisQtyType = Convert.ToString(item[WorldSyncNutritionalsFields.NutrientBasisQtyType]),
                                NutrientBasisQtyUOM = Convert.ToString(item[WorldSyncNutritionalsFields.NutrientBasisQtyUOM]),
                                PreparationState = Convert.ToString(item[WorldSyncNutritionalsFields.PreparationState]),
                                ServingsPerPackage = Convert.ToString(item[WorldSyncNutritionalsFields.ServingsPerPackage]),
                                ServingSizeDescription = Convert.ToString(item[WorldSyncNutritionalsFields.ServingSizeDescription]),
                                ServingSize = Convert.ToString(item[WorldSyncNutritionalsFields.ServingSize]),
                                ServingSizeUOM = Convert.ToString(item[WorldSyncNutritionalsFields.ServingSizeUOM]),
                                AllergenSpecificationAgency = Convert.ToString(item[WorldSyncNutritionalsFields.AllergenSpecificationAgency]),
                                AllergenSpecificationName = Convert.ToString(item[WorldSyncNutritionalsFields.AllergenSpecificationName]),
                                AllergenStatement = Convert.ToString(item[WorldSyncNutritionalsFields.AllergenStatement]),
                                IngredientStatement = Convert.ToString(item[WorldSyncNutritionalsFields.IngredientStatement])
                            };
                        }
                    }
                }
            }
            return nutrient;
        }
        public int UpsertWorldSyncNutritionalsListItem(WorldSyncNutritionalsListItem pmItem)
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
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_WorldSyncNutritionals);
                        if (pmItem.Id <= 0)// insert
                        {
                            item = spList.AddItem();
                            item[WorldSyncNutritionalsFields.CompassListItemId] = pmItem.CompassListItemId;
                        }
                        else
                            item = spList.GetItemById(pmItem.Id);
                        item[WorldSyncNutritionalsFields.NutrientBasisQty] = pmItem.NutrientBasisQty;
                        item[WorldSyncNutritionalsFields.NutrientBasisQtyType] = pmItem.NutrientBasisQtyType;
                        item[WorldSyncNutritionalsFields.NutrientBasisQtyUOM] = pmItem.NutrientBasisQtyUOM;
                        item[WorldSyncNutritionalsFields.PreparationState] = pmItem.PreparationState;
                        item[WorldSyncNutritionalsFields.ServingsPerPackage] = pmItem.ServingsPerPackage;
                        item[WorldSyncNutritionalsFields.ServingSizeDescription] = pmItem.ServingSizeDescription;
                        item[WorldSyncNutritionalsFields.ServingSize] = pmItem.ServingSize;
                        item[WorldSyncNutritionalsFields.ServingSizeUOM] = pmItem.ServingSizeUOM;
                        item[WorldSyncNutritionalsFields.AllergenSpecificationAgency] = pmItem.AllergenSpecificationAgency;
                        item[WorldSyncNutritionalsFields.AllergenSpecificationName] = pmItem.AllergenSpecificationName;
                        item[WorldSyncNutritionalsFields.AllergenStatement] = pmItem.AllergenStatement;
                        item[WorldSyncNutritionalsFields.IngredientStatement] = pmItem.IngredientStatement;
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
    }
}

