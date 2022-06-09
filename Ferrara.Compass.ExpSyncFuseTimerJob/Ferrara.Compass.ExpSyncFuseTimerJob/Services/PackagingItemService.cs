using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;
using Ferrara.Compass.ExpSyncFuseTimerJob.Models;
using Ferrara.Compass.ExpSyncFuseTimerJob.Constants;

namespace Ferrara.Compass.ExpSyncFuseTimerJob.Services
{
    public class PackagingItemService
    {
        SPSite site;
        public PackagingItemService(SPSite site)
        {
            this.site = site;
        }
        public List<PackagingItem> GetCandySemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPWeb spWeb = site.OpenWeb())
            {
                SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                SPQuery spQuery = new SPQuery();
                spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_CandySemi + "</Value></Eq></And></Where>";

                SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                if (compassItemCol.Count > 0)
                {
                    foreach (SPListItem item in compassItemCol)
                    {
                        if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                            continue;

                        PackagingItem packagingItem = new PackagingItem();
                        packagingItem.Id = item.ID;
                        packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                        packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                        packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                        packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                        packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                        packagingItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                        packagingItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                        packagingItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                        packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                        packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                        packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                        packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                        packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                        packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                        packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);

                        packagingItem.Length = Convert.ToString(item[PackagingItemListFields.Length]);
                        packagingItem.Width = Convert.ToString(item[PackagingItemListFields.Width]);
                        packagingItem.Height = Convert.ToString(item[PackagingItemListFields.Height]);
                        packagingItem.CADDrawing = Convert.ToString(item[PackagingItemListFields.CADDrawing]);
                        packagingItem.Structure = Convert.ToString(item[PackagingItemListFields.Structure]);
                        packagingItem.StructureColor = Convert.ToString(item[PackagingItemListFields.StructureColor]);
                        packagingItem.BackSeam = Convert.ToString(item[PackagingItemListFields.BackSeam]);
                        packagingItem.WebWidth = Convert.ToString(item[PackagingItemListFields.WebWidth]);
                        packagingItem.ExactCutOff = Convert.ToString(item[PackagingItemListFields.ExactCutOff]);
                        packagingItem.BagFace = Convert.ToString(item[PackagingItemListFields.BagFace]);
                        packagingItem.Unwind = Convert.ToString(item[PackagingItemListFields.Unwind]);
                        packagingItem.Description = Convert.ToString(item[PackagingItemListFields.Description]);

                        packagingItem.FilmMaxRollOD = Convert.ToString(item[PackagingItemListFields.FilmMaxRollOD]);
                        packagingItem.FilmRollID = Convert.ToString(item[PackagingItemListFields.FilmRollID]);
                        packagingItem.FilmPrintStyle = Convert.ToString(item[PackagingItemListFields.FilmPrintStyle]);
                        packagingItem.FilmStyle = Convert.ToString(item[PackagingItemListFields.FilmStyle]);
                        packagingItem.CorrugatedPrintStyle = Convert.ToString(item[PackagingItemListFields.CorrugatedPrintStyle]);
                        packagingItem.GraphicsChangeRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                        packagingItem.ExternalGraphicsVendor = Convert.ToString(item[PackagingItemListFields.ExternalGraphicsVendor]);
                        packagingItem.GraphicsBrief = Convert.ToString(item[PackagingItemListFields.GraphicsBrief]);
                        packagingItem.BOMEffectiveDate = Convert.ToDateTime(item[PackagingItemListFields.BOMEffectiveDate]);
                        packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                        packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                        packagingItem.MRPC = Convert.ToString(item[PackagingItemListFields.MRPC]);
                        packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);

                        packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                        packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                        packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                        packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                        packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                        packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                        packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                        packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                        packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                        packagingItems.Add(packagingItem);
                    }
                }
            }
            return packagingItems;
        }
    }
}