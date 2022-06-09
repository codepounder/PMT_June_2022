using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.Services
{
    public class BillOfMaterialsService : IBillOfMaterialsService
    {
        public BillofMaterialsItem GetBillOfMaterialsItem(int itemId)
        {
            BillofMaterialsItem materialItem = new BillofMaterialsItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        materialItem.CompassListItemId = item.ID;

                        // Read-only fields
                        materialItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        materialItem.ProjectSubcat = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        materialItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        materialItem.Initiator = Convert.ToString(item[CompassListFields.Initiator]);
                        materialItem.PM = Convert.ToString(item[CompassListFields.PM]);
                        materialItem.PMName = Convert.ToString(item[CompassListFields.PMName]);
                        materialItem.PackagingEngineerLead = Convert.ToString(item[CompassListFields.PackagingEngineerLead]);
                        materialItem.PackagingNumbers = Convert.ToString(item[CompassListFields.PackagingNumbers]);
                        materialItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        materialItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        materialItem.WorkCenterAddInfo = Convert.ToString(item[CompassListFields.WorkCenterAdditionalInfo]);
                        materialItem.PegHoleNeeded = Convert.ToString(item[CompassListFields.PegHoleNeeded]);
                        materialItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        materialItem.FGLikeItem = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                        materialItem.NewExistingItem = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        materialItem.PLMProject = Convert.ToString(item[CompassListFields.PLMProject]);
                    }
                    var spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassTeamListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    var ipItems = spList2.GetItems(spQuery);

                    if (ipItems != null && ipItems.Count > 0)
                    {
                        var ipItem = ipItems[0];
                        materialItem.Marketing = Convert.ToString(ipItem[CompassTeamListFields.Marketing]);
                        materialItem.MarketingName = Convert.ToString(ipItem[CompassTeamListFields.MarketingName]);
                        materialItem.PackagingEngineering = Convert.ToString(ipItem[CompassTeamListFields.PackagingEngineering]);
                        materialItem.PackagingEngineeringName = Convert.ToString(ipItem[CompassTeamListFields.PackagingEngineeringName]);
                        materialItem.InTech = Convert.ToString(ipItem[CompassTeamListFields.InTech]);
                        materialItem.InTechName = Convert.ToString(ipItem[CompassTeamListFields.InTechName]);
                    }
                }
            }
            return materialItem;
        }
        public void UpdateBillOfMaterialsItem(BillofMaterialsItem materialItem, string pageName)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(materialItem.CompassListItemId);
                        if (item != null)
                        {
                            item[CompassListFields.PackagingNumbers] = materialItem.PackagingNumbers;
                            if (pageName.ToLower() == GlobalConstants.PAGE_PE.ToLower() && !string.IsNullOrEmpty(materialItem.PackagingEngineerLead))
                            {
                                if(materialItem.PackagingEngineerLead != GlobalConstants.GROUP_PackagingEngineer)
                                {
                                    item[CompassListFields.PackagingEngineerLead] = materialItem.PackagingEngineerLead;
                                }
                                else 
                                {
                                    SPGroup PEGroup = spWeb.Groups[GlobalConstants.GROUP_PackagingEngineer];
                                    item[CompassListFields.PackagingEngineerLead] = PEGroup;
                                }
                            }
                            // Set Modified By to current user NOT System Account
                            item[CompassListFields.LastUpdatedFormName] = pageName;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdatePackagingNumbers(string packagingNumbers, int iItemId)
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
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(iItemId);
                        if (item != null)
                        {
                            item[CompassListFields.PackagingNumbers] = packagingNumbers;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public string getProjectNewExisting(int iItemId)
        {
            string newExisting = "existing";
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(iItemId);
                    if (item != null)
                    {
                        string TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        if (TBDIndicator.ToLower() == "yes")
                        {
                            newExisting = "new";
                        }
                    }
                }
            }
            return newExisting;
        }
        #region Pack Measurements Methods
        public CompassPackMeasurementsItem GetPackMeasurementsItem(int itemId, int parentId)
        {
            CompassPackMeasurementsItem pmItem = new CompassPackMeasurementsItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">" + parentId + "</Value></Eq></And></Where>";

                    spQuery.RowLimit = 1;
                    SPListItemCollection compassItemCol;

                    compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            pmItem.ItemId = item.ID;
                            pmItem.CompassListItemId = Convert.ToInt32(item[CompassPackMeasurementsFields.CompassListItemId]);

                            pmItem.DoubleStackable = Convert.ToString(item[CompassPackMeasurementsFields.DoubleStackable]);
                            pmItem.PackTrialComments = Convert.ToString(item[CompassPackMeasurementsFields.PackTrialComments]);
                            pmItem.PackTrialNeeded = Convert.ToString(item[CompassPackMeasurementsFields.PackTrialNeeded]);
                            pmItem.PackTrialResult = Convert.ToString(item[CompassPackMeasurementsFields.PackTrialResult]);

                            if (item[CompassPackMeasurementsFields.CaseCube] != null)
                            {
                                try { pmItem.CaseCube = Convert.ToDouble(item[CompassPackMeasurementsFields.CaseCube]); }
                                catch { pmItem.CaseCube = -9999; }
                            }
                            else
                            {
                                pmItem.CaseCube = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CaseDimensionHeight] != null)
                            {
                                try { pmItem.CaseDimensionHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.CaseDimensionHeight]); }
                                catch { pmItem.CaseDimensionHeight = -9999; }
                            }
                            else
                            {
                                pmItem.CaseDimensionHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CaseDimensionLength] != null)
                            {
                                try { pmItem.CaseDimensionLength = Convert.ToDouble(item[CompassPackMeasurementsFields.CaseDimensionLength]); }
                                catch { pmItem.CaseDimensionLength = -9999; }
                            }
                            else
                            {
                                pmItem.CaseDimensionLength = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CaseDimensionWidth] != null)
                            {
                                try { pmItem.CaseDimensionWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.CaseDimensionWidth]); }
                                catch { pmItem.CaseDimensionWidth = -9999; }
                            }
                            else
                            {
                                pmItem.CaseDimensionWidth = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CaseGrossWeight] != null)
                            {
                                try { pmItem.CaseGrossWeight = Convert.ToDouble(item[CompassPackMeasurementsFields.CaseGrossWeight]); }
                                catch { pmItem.CaseGrossWeight = -9999; }
                            }
                            else
                            {
                                pmItem.CaseGrossWeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CaseNetWeight] != null)
                            {
                                try { pmItem.CaseNetWeight = Convert.ToDouble(item[CompassPackMeasurementsFields.CaseNetWeight]); }
                                catch { pmItem.CaseNetWeight = -9999; }
                            }
                            else
                            {
                                pmItem.CaseNetWeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CasePack] != null)
                            {
                                try { pmItem.CasePack = Convert.ToDouble(item[CompassPackMeasurementsFields.CasePack]); }
                                catch { pmItem.CasePack = -9999; }
                            }
                            else
                            {
                                pmItem.CasePack = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CasesPerLayer] != null)
                            {
                                try { pmItem.CasesPerLayer = Convert.ToDouble(item[CompassPackMeasurementsFields.CasesPerLayer]); }
                                catch { pmItem.CasesPerLayer = -9999; }
                            }
                            else
                            {
                                pmItem.CasesPerLayer = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.CasesPerPallet] != null)
                            {
                                try { pmItem.CasesPerPallet = Convert.ToDouble(item[CompassPackMeasurementsFields.CasesPerPallet]); }
                                catch { pmItem.CasesPerPallet = -9999; }
                            }
                            else
                            {
                                pmItem.CasesPerPallet = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.DisplayDimensionsHeight] != null)
                            {
                                try { pmItem.DisplayDimensionsHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.DisplayDimensionsHeight]); }
                                catch { pmItem.DisplayDimensionsHeight = -9999; }
                            }
                            else
                            {
                                pmItem.DisplayDimensionsHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.DisplayDimensionsLength] != null)
                            {
                                try { pmItem.DisplayDimensionsLength = Convert.ToDouble(item[CompassPackMeasurementsFields.DisplayDimensionsLength]); }
                                catch { pmItem.DisplayDimensionsLength = -9999; }
                            }
                            else
                            {
                                pmItem.DisplayDimensionsLength = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.DisplayDimensionsWidth] != null)
                            {
                                try { pmItem.DisplayDimensionsWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.DisplayDimensionsWidth]); }
                                catch { pmItem.DisplayDimensionsWidth = -9999; }
                            }
                            else
                            {
                                pmItem.DisplayDimensionsWidth = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.LayersPerPallet] != null)
                            {
                                try { pmItem.LayersPerPallet = Convert.ToDouble(item[CompassPackMeasurementsFields.LayersPerPallet]); }
                                catch { pmItem.LayersPerPallet = -9999; }
                            }
                            else
                            {
                                pmItem.LayersPerPallet = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.NetUnitWeight] != null)
                            {
                                try { pmItem.NetUnitWeight = Convert.ToDouble(item[CompassPackMeasurementsFields.NetUnitWeight]); }
                                catch { pmItem.NetUnitWeight = -9999; }
                            }
                            else
                            {
                                pmItem.NetUnitWeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.PalletCube] != null)
                            {
                                try { pmItem.PalletCube = Convert.ToDouble(item[CompassPackMeasurementsFields.PalletCube]); }
                                catch { pmItem.PalletCube = -9999; }
                            }
                            else
                            {
                                pmItem.PalletCube = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.PalletDimensionsHeight] != null)
                            {
                                try { pmItem.PalletDimensionsHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.PalletDimensionsHeight]); }
                                catch { pmItem.PalletDimensionsHeight = -9999; }
                            }
                            else
                            {
                                pmItem.PalletDimensionsHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.PalletDimensionsHeight] != null)
                            {
                                try { pmItem.PalletDimensionsHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.PalletDimensionsHeight]); }
                                catch { pmItem.PalletDimensionsHeight = -9999; }
                            }
                            else
                            {
                                pmItem.PalletDimensionsHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.PalletDimensionsLength] != null)
                            {
                                try { pmItem.PalletDimensionsLength = Convert.ToDouble(item[CompassPackMeasurementsFields.PalletDimensionsLength]); }
                                catch { pmItem.PalletDimensionsLength = -9999; }
                            }
                            else
                            {
                                pmItem.PalletDimensionsLength = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.PalletDimensionsWidth] != null)
                            {
                                try { pmItem.PalletDimensionsWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.PalletDimensionsWidth]); }
                                catch { pmItem.PalletDimensionsWidth = -9999; }
                            }
                            else
                            {
                                pmItem.PalletDimensionsWidth = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.PalletGrossWeight] != null)
                            {
                                try { pmItem.PalletGrossWeight = Convert.ToDouble(item[CompassPackMeasurementsFields.PalletGrossWeight]); }
                                catch { pmItem.PalletGrossWeight = -9999; }
                            }
                            else
                            {
                                pmItem.PalletGrossWeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.PalletWeight] != null)
                            {
                                try { pmItem.PalletWeight = Convert.ToDouble(item[CompassPackMeasurementsFields.PalletWeight]); }
                                catch { pmItem.PalletWeight = -9999; }
                            }
                            else
                            {
                                pmItem.PalletWeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SalesCaseDimensionsHeight] != null)
                            {
                                try { pmItem.SalesCaseDimensionsHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesCaseDimensionsHeight]); }
                                catch { pmItem.SalesCaseDimensionsHeight = -9999; }
                            }
                            else
                            {
                                pmItem.SalesCaseDimensionsHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SalesCaseDimensionsLength] != null)
                            {
                                try { pmItem.SalesCaseDimensionsLength = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesCaseDimensionsLength]); }
                                catch { pmItem.SalesCaseDimensionsLength = -9999; }
                            }
                            else
                            {
                                pmItem.SalesCaseDimensionsLength = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SalesCaseDimensionsWidth] != null)
                            {
                                try { pmItem.SalesCaseDimensionsWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesCaseDimensionsWidth]); }
                                catch { pmItem.SalesCaseDimensionsWidth = -9999; }
                            }
                            else
                            {
                                pmItem.SalesCaseDimensionsWidth = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SalesUnitDimensionsHeight] != null)
                            {
                                try { pmItem.SalesUnitDimensionsHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesUnitDimensionsHeight]); }
                                catch { pmItem.SalesUnitDimensionsHeight = -9999; }
                            }
                            else
                            {
                                pmItem.SalesUnitDimensionsHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SalesUnitDimensionsLength] != null)
                            {
                                try { pmItem.SalesUnitDimensionsLength = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesUnitDimensionsLength]); }
                                catch { pmItem.SalesUnitDimensionsLength = -9999; }
                            }
                            else
                            {
                                pmItem.SalesUnitDimensionsLength = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SalesUnitDimensionsWidth] != null)
                            {
                                try { pmItem.SalesUnitDimensionsWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesUnitDimensionsWidth]); }
                                catch { pmItem.SalesUnitDimensionsWidth = -9999; }
                            }
                            else
                            {
                                pmItem.SalesUnitDimensionsWidth = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SetUpDimensionsHeight] != null)
                            {
                                try { pmItem.SetUpDimensionsHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.SetUpDimensionsHeight]); }
                                catch { pmItem.SetUpDimensionsHeight = -9999; }
                            }
                            else
                            {
                                pmItem.SetUpDimensionsHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SetUpDimensionsLength] != null)
                            {
                                try { pmItem.SetUpDimensionsLength = Convert.ToDouble(item[CompassPackMeasurementsFields.SetUpDimensionsLength]); }
                                catch { pmItem.SetUpDimensionsLength = -9999; }
                            }
                            else
                            {
                                pmItem.SetUpDimensionsLength = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.SetUpDimensionsWidth] != null)
                            {
                                try { pmItem.SetUpDimensionsWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.SetUpDimensionsWidth]); }
                                catch { pmItem.SetUpDimensionsWidth = -9999; }
                            }
                            else
                            {
                                pmItem.SetUpDimensionsWidth = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.UnitDimensionHeight] != null)
                            {
                                try { pmItem.UnitDimensionHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.UnitDimensionHeight]); }
                                catch { pmItem.UnitDimensionHeight = -9999; }
                            }
                            else
                            {
                                pmItem.UnitDimensionHeight = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.UnitDimensionLength] != null)
                            {
                                try { pmItem.UnitDimensionLength = Convert.ToDouble(item[CompassPackMeasurementsFields.UnitDimensionLength]); }
                                catch { pmItem.UnitDimensionLength = -9999; }
                            }
                            else
                            {
                                pmItem.UnitDimensionLength = -9999;
                            }

                            if (item[CompassPackMeasurementsFields.UnitDimensionWidth] != null)
                            {
                                try { pmItem.UnitDimensionWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.UnitDimensionWidth]); }
                                catch { pmItem.UnitDimensionWidth = -9999; }
                            }
                            else
                            {
                                pmItem.UnitDimensionWidth = -9999;
                            }
                            pmItem.PalletPatternChange = Convert.ToString(item[CompassPackMeasurementsFields.PalletPatternChange]);
                            pmItem.SAPSpecsChange = Convert.ToString(item[CompassPackMeasurementsFields.SAPSpecsChange]);
                            pmItem.ParentComponentId = Convert.ToInt32(item[CompassPackMeasurementsFields.ParentComponentId]);

                        }
                    }
                }
            }
            return pmItem;
        }
        public int InsertPackMeasurementItem(int compassListItemId, string title)
        {
            int id = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);

                        SPListItem appItem = spList.AddItem();

                        appItem["Title"] = title;
                        appItem[CompassPackMeasurementsFields.CompassListItemId] = compassListItemId;

                        appItem.Update();
                        spWeb.AllowUnsafeUpdates = false;

                        id = appItem.ID;
                    }
                }
            });
            return id;
        }
        public void UpsertPackMeasurementsItem(CompassPackMeasurementsItem pmItem, string projectNumber)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + pmItem.CompassListItemId + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">" + pmItem.ParentComponentId + "</Value></Eq></And></Where>";

                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item;
                        if (compassItemCol.Count < 1)
                        {
                            // If we didn't find it, Insert record
                            int id = InsertPackMeasurementItem(pmItem.CompassListItemId, projectNumber);
                            item = spList.GetItemById(id);
                        }
                        else
                        {
                            item = compassItemCol[0];
                        }

                        if (item != null)
                        {
                            item[CompassPackMeasurementsFields.ParentComponentId] = pmItem.ParentComponentId;

                            item[CompassPackMeasurementsFields.CaseCube] = pmItem.CaseCube;
                            item[CompassPackMeasurementsFields.CaseDimensionHeight] = pmItem.CaseDimensionHeight;
                            item[CompassPackMeasurementsFields.CaseDimensionLength] = pmItem.CaseDimensionLength;
                            item[CompassPackMeasurementsFields.CaseDimensionWidth] = pmItem.CaseDimensionWidth;
                            item[CompassPackMeasurementsFields.CaseGrossWeight] = pmItem.CaseGrossWeight;
                            item[CompassPackMeasurementsFields.CaseNetWeight] = pmItem.CaseNetWeight;
                            item[CompassPackMeasurementsFields.CasePack] = pmItem.CasePack;
                            item[CompassPackMeasurementsFields.CasesPerLayer] = pmItem.CasesPerLayer;
                            item[CompassPackMeasurementsFields.CasesPerPallet] = pmItem.CasesPerPallet;

                            item[CompassPackMeasurementsFields.DisplayDimensionsHeight] = pmItem.DisplayDimensionsHeight;
                            item[CompassPackMeasurementsFields.DisplayDimensionsLength] = pmItem.DisplayDimensionsLength;
                            item[CompassPackMeasurementsFields.DisplayDimensionsWidth] = pmItem.DisplayDimensionsWidth;
                            item[CompassPackMeasurementsFields.DoubleStackable] = pmItem.DoubleStackable;
                            item[CompassPackMeasurementsFields.LayersPerPallet] = pmItem.LayersPerPallet;
                            item[CompassPackMeasurementsFields.NetUnitWeight] = pmItem.NetUnitWeight;
                            item[CompassPackMeasurementsFields.PackTrialComments] = pmItem.PackTrialComments;
                            item[CompassPackMeasurementsFields.PackTrialNeeded] = pmItem.PackTrialNeeded;
                            item[CompassPackMeasurementsFields.PackTrialResult] = pmItem.PackTrialResult;
                            item[CompassPackMeasurementsFields.PalletCube] = pmItem.PalletCube;
                            item[CompassPackMeasurementsFields.PalletDimensionsHeight] = pmItem.PalletDimensionsHeight;
                            item[CompassPackMeasurementsFields.PalletDimensionsLength] = pmItem.PalletDimensionsLength;
                            item[CompassPackMeasurementsFields.PalletDimensionsWidth] = pmItem.PalletDimensionsWidth;
                            item[CompassPackMeasurementsFields.PalletGrossWeight] = pmItem.PalletGrossWeight;
                            item[CompassPackMeasurementsFields.PalletWeight] = pmItem.PalletWeight;
                            item[CompassPackMeasurementsFields.SalesCaseDimensionsHeight] = pmItem.SalesCaseDimensionsHeight;
                            item[CompassPackMeasurementsFields.SalesCaseDimensionsLength] = pmItem.SalesCaseDimensionsLength;
                            item[CompassPackMeasurementsFields.SalesCaseDimensionsWidth] = pmItem.SalesCaseDimensionsWidth;
                            item[CompassPackMeasurementsFields.SalesUnitDimensionsHeight] = pmItem.SalesUnitDimensionsHeight;
                            item[CompassPackMeasurementsFields.SalesUnitDimensionsLength] = pmItem.SalesUnitDimensionsLength;
                            item[CompassPackMeasurementsFields.SalesUnitDimensionsWidth] = pmItem.SalesUnitDimensionsWidth;
                            item[CompassPackMeasurementsFields.SetUpDimensionsHeight] = pmItem.SetUpDimensionsHeight;
                            item[CompassPackMeasurementsFields.SetUpDimensionsLength] = pmItem.SetUpDimensionsLength;
                            item[CompassPackMeasurementsFields.SetUpDimensionsWidth] = pmItem.SetUpDimensionsWidth;
                            item[CompassPackMeasurementsFields.UnitDimensionHeight] = pmItem.UnitDimensionHeight;
                            item[CompassPackMeasurementsFields.UnitDimensionLength] = pmItem.UnitDimensionLength;
                            item[CompassPackMeasurementsFields.UnitDimensionWidth] = pmItem.UnitDimensionWidth;
                            item[CompassPackMeasurementsFields.PalletPatternChange] = pmItem.PalletPatternChange;
                            item[CompassPackMeasurementsFields.SAPSpecsChange] = pmItem.SAPSpecsChange;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpsertPackMeasurementsPackTrial(int compassListItemId, string packTrial, string projectNumber)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId.ToString() + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">0</Value></Eq></And></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item;
                        if (compassItemCol.Count < 1)
                        {
                            // If we didn't find it, Insert record
                            int id = InsertPackMeasurementItem(compassListItemId, projectNumber);
                            item = spList.GetItemById(id);
                        }
                        else
                        {
                            item = compassItemCol[0];
                        }

                        if (item != null)
                        {
                            item[CompassPackMeasurementsFields.PackTrialNeeded] = packTrial;
                            item[CompassPackMeasurementsFields.ParentComponentId] = 0;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion

        #region Bill of Materials Approvals
        public ApprovalItem GetBillofMaterialsApprovalItem(int itemId, string BOMType)
        {
            ApprovalItem appItem = new ApprovalItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spQuery.RowLimit = 1;

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];

                        if (item != null)
                        {
                            if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_PE.ToLower()))
                            {
                                appItem.StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_StartDate]);
                                appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_ModifiedDate]);
                                appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE_ModifiedBy]);
                                appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE_SubmittedDate]);
                                appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE_SubmittedBy]);
                            }
                            else if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
                            {
                                appItem.StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_StartDate]);
                                appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_ModifiedDate]);
                                appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_ModifiedBy]);
                                appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_SubmittedDate]);
                                appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupPE2_SubmittedBy]);
                            }
                            else if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
                            {
                                appItem.StartDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_StartDate]);
                                appItem.ModifiedDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_ModifiedDate]);
                                appItem.ModifiedBy = Convert.ToString(item[ApprovalListFields.BOMSetupProc_ModifiedBy]);
                                appItem.SubmittedDate = Convert.ToString(item[ApprovalListFields.BOMSetupProc_SubmittedDate]);
                                appItem.SubmittedBy = Convert.ToString(item[ApprovalListFields.BOMSetupProc_SubmittedBy]);
                            }
                        }
                    }
                }
            }
            return appItem;
        }
        public void UpdateBillofMaterialsApprovalItem(ApprovalItem approvalItem, string BOMType, bool bSubmitted)
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
                                if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_PE.ToLower()))
                                {
                                    if ((bSubmitted) && (appItem[ApprovalListFields.BOMSetupPE_SubmittedDate] == null))
                                    {
                                        appItem[ApprovalListFields.BOMSetupPE_SubmittedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.BOMSetupPE_SubmittedBy] = approvalItem.ModifiedBy;
                                    }
                                    else
                                    {
                                        appItem[ApprovalListFields.BOMSetupPE_ModifiedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.BOMSetupPE_ModifiedBy] = approvalItem.ModifiedBy;
                                    }
                                }
                                else if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
                                {
                                    if ((bSubmitted) && (appItem[ApprovalListFields.BOMSetupPE2_SubmittedDate] == null))
                                    {
                                        appItem[ApprovalListFields.BOMSetupPE2_SubmittedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.BOMSetupPE2_SubmittedBy] = approvalItem.ModifiedBy;
                                    }
                                    else
                                    {
                                        appItem[ApprovalListFields.BOMSetupPE2_ModifiedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.BOMSetupPE2_ModifiedBy] = approvalItem.ModifiedBy;
                                    }
                                }
                                else if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
                                {
                                    appItem[ApprovalListFields.BOMSetupProc_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.BOMSetupProc_ModifiedBy] = approvalItem.ModifiedBy;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void SetBillofMaterialsStartDate(int compassListItemId, DateTime startDate, string BOMType)
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
                                if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_PE.ToLower()))
                                {
                                    if (item[ApprovalListFields.BOMSetupPE_StartDate] == null)
                                    {
                                        item[ApprovalListFields.BOMSetupPE_StartDate] = startDate.ToString();
                                        item.Update();
                                    }
                                }
                                else if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_PE2.ToLower()))
                                {
                                    if (item[ApprovalListFields.BOMSetupPE2_StartDate] == null)
                                    {
                                        item[ApprovalListFields.BOMSetupPE2_StartDate] = startDate.ToString();
                                        item.Update();
                                    }
                                }
                                else if (string.Equals(BOMType.ToLower(), GlobalConstants.PAGE_Proc.ToLower()))
                                {
                                    if (item[ApprovalListFields.BOMSetupProc_StartDate] == null)
                                    {
                                        item[ApprovalListFields.BOMSetupProc_StartDate] = startDate.ToString();
                                        item.Update();
                                    }
                                }

                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #endregion

        public ItemProposalItem getIPFItem(int itemId)
        {
            ItemProposalItem team = new ItemProposalItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"" + CompassListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    var ipItems = spList.GetItems(spQuery);

                    if (ipItems != null && ipItems.Count > 0)
                    {
                        var ipItem = ipItems[0];

                        team.InTech = Convert.ToString(ipItem[CompassTeamListFields.InTech]);
                        team.InTechName = Convert.ToString(ipItem[CompassTeamListFields.InTechName]);
                        team.PackagingEngineering = Convert.ToString(ipItem[CompassTeamListFields.PackagingEngineering]);
                        team.PackagingEngineeringName = Convert.ToString(ipItem[CompassTeamListFields.PackagingEngineeringName]);
                    }
                }
            }
            return team;
        }
    }
}
