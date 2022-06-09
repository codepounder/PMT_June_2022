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
    public class BOMSetupMaterialWarehouseService : IBOMSetupMaterialWarehouseService
    {
        public BOMSetupMaterialWarehouseItem GetBOMSetupMaterialWarehouseItem(int itemId)
        {
            BOMSetupMaterialWarehouseItem sgItem = new BOMSetupMaterialWarehouseItem();
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
                        sgItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        sgItem.LineWorkcenterAdditionalInfo = Convert.ToString(item[CompassListFields.WorkCenterAdditionalInfo]);
                        sgItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        sgItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.FinishedGoodPackLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.IsPegHoleNeeded = Convert.ToString(item[CompassListFields.PegHoleNeeded]);
                        sgItem.FGLikeItem = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                        sgItem.ProjectInitiator = Convert.ToString(item[CompassListFields.Initiator]);
                        sgItem.ProjectInitiatorName = Convert.ToString(item[CompassListFields.InitiatorName]);
                        sgItem.PM = Convert.ToString(item[CompassListFields.PM]);
                        sgItem.PMName = Convert.ToString(item[CompassListFields.PMName]);
                        sgItem.PackagingEngineer = Convert.ToString(item[CompassListFields.PackagingEngineerLead]);
                        sgItem.PackagingEngineerName = Convert.ToString(item[CompassListFields.PackagingEngineerLead]);
                        sgItem.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);
                        sgItem.ProcurementType = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        sgItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        sgItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                    }

                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem teamItem = compassItemCol[0];
                        if (teamItem != null)
                        {
                            sgItem.InTechManager = Convert.ToString(teamItem[CompassTeamListFields.InTech]);
                            sgItem.InTechManagerName = Convert.ToString(teamItem[CompassTeamListFields.InTechName]);
                            sgItem.BrandManager = Convert.ToString(teamItem[CompassTeamListFields.Marketing]);
                            sgItem.BrandManagerName = Convert.ToString(teamItem[CompassTeamListFields.MarketingName]);
                        }
                    }

                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spCompassList2Query = new SPQuery();
                    spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spCompassList2Query.RowLimit = 1;

                    SPListItemCollection compass2ItemCol = spList.GetItems(spCompassList2Query);
                    if (compass2ItemCol.Count > 0)
                    {
                        SPListItem compassList2tem = compass2ItemCol[0];
                        if (compassList2tem != null)
                        {
                            sgItem.DesignateHUBDC = Convert.ToString(compassList2tem[CompassList2Fields.DesignateHUBDC]);
                        }
                    }
                }
            }
            return sgItem;
        }
        public void UpdateBOMSetupMaterialWarehouseItem(BOMSetupMaterialWarehouseItem bomSetupMaterialWarehouseItem)
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
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + bomSetupMaterialWarehouseItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateBOMSetupMaterialWarehouseApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
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
                                if ((bSubmitted) && (appItem[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.BOMSetupMaterialWarehouse_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.BOMSetupMaterialWarehouse_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
    }
}
