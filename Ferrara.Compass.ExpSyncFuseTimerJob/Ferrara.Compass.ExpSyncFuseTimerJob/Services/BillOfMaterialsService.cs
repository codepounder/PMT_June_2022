using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Ferrara.Compass.ExpSyncFuseTimerJob.Models;
using Ferrara.Compass.ExpSyncFuseTimerJob.Constants;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Services
{
    public class BillOfMaterialsService
    {
        SPSite site;
        public BillOfMaterialsService(SPSite site)
        {
            this.site = site;
        }
        public CompassPackMeasurementsItem GetPackMeasurementsItem(int itemId, int parentId, string bomType)
        {
            CompassPackMeasurementsItem pmItem = new CompassPackMeasurementsItem();
            using (SPWeb spWeb = site.OpenWeb())
            {
                SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                SPQuery spQuery = new SPQuery();
                if (bomType == GlobalConstants.PACKAGINGTYPE_FGBOM)
                {
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"BOMType\" /><Value Type=\"Choice\">FGBOM</Value></Eq></And></Where>";
                }
                else {
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"BOMType\" /><Value Type=\"Choice\">SEMIBOM</Value></Eq></And><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">" + parentId + "</Value></Eq></And></Where>";
                }
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
                        pmItem.BOMType = Convert.ToString(item[CompassPackMeasurementsFields.BOMType]);

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
                    }
                }
            }
            return pmItem;
        }
    }
}
