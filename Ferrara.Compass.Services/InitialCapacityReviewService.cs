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
   public class InitialCapacityReviewService : IInitialCapacityReviewService
    {
        public InitialCapacityReviewItem GetInitialCapacityReviewItem(int itemId)
        {
            InitialCapacityReviewItem sgItem = new InitialCapacityReviewItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.ManufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                        sgItem.FirstProductionDate = Convert.ToDateTime(item[CompassListFields.FirstProductionDate]);
                        sgItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        sgItem.LineOfBusiness = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        try { sgItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                        catch { sgItem.AnnualProjectedUnits = -9999; }

                        try { sgItem.RetailSellingUnitsBaseUOM = Convert.ToInt32(item[CompassListFields.RetailSellingUnitsBaseUOM]); }
                        catch { sgItem.RetailSellingUnitsBaseUOM = -9999; }

                        try { sgItem.RetailUnitWieghtOz = Convert.ToDouble(item[CompassListFields.RetailUnitWieghtOz]); }
                        catch { sgItem.RetailUnitWieghtOz = -9999; }

                        try { sgItem.Month1ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month1ProjectedUnits]); }
                        catch { sgItem.Month1ProjectedUnits = -9999; }

                        try { sgItem.Month2ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month2ProjectedUnits]); }
                        catch { sgItem.Month2ProjectedUnits = -9999; }

                        try { sgItem.Month3ProjectedUnits = Convert.ToInt32(item[CompassListFields.Month3ProjectedUnits]); }
                        catch { sgItem.Month3ProjectedUnits = -9999; }
                    }

                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem decisionItem = compassItemCol[0];

                        if (decisionItem != null)
                        {
                           
                            sgItem.InitialCapacity_Decision = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_Decision]);
                            sgItem.InitialCapacity_CapacityRiskComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_CapacityRiskComments]);
                            sgItem.InitialCapacity_AcceptanceComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_AcceptanceComments]);
                            sgItem.InitialCapacity_MakeIssues = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_MakeIssues]);
                            sgItem.InitialCapacity_PackIssues = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.InitialCapacity_PackIssues]);
                            sgItem.SrOBMApproval_CapacityReviewComments = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SrOBMApproval_CapacityReviewComments]);
                        }
                    }

                }
            }
            return sgItem;
        }
        public void UpdateInitialCapacityReviewItem(InitialCapacityReviewItem capacityItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + capacityItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_Decision] = capacityItem.InitialCapacity_Decision;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_CapacityRiskComments] = capacityItem.InitialCapacity_CapacityRiskComments;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_AcceptanceComments] = capacityItem.InitialCapacity_AcceptanceComments;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_MakeIssues] = capacityItem.InitialCapacity_MakeIssues;
                                appItem[CompassProjectDecisionsListFields.InitialCapacity_PackIssues] = capacityItem.InitialCapacity_PackIssues;
                                
                                appItem.Update();
                            }
                        }

                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                        SPListItem item = spList.GetItemById(capacityItem.CompassListItemId);
                        if (item != null)
                        {
                          if ((capacityItem.FirstProductionDate != null) && (capacityItem.FirstProductionDate != DateTime.MinValue))
                                item[CompassListFields.FirstProductionDate] = capacityItem.FirstProductionDate;

                            //item[CompassListFields.LastUpdatedFormName] = CompassForm.InitialCapacity.ToString();
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}
