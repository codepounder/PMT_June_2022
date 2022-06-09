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

namespace Ferrara.Compass.Services
{
    public class PackagingItemService : IPackagingItemService
    {
        private readonly IUtilityService utilityService;
        private readonly IExceptionService exceptionService;
        private readonly ISAPBOMService SAPBOMService;

        public PackagingItemService(IUtilityService utilityService, IExceptionService exceptionService, ISAPBOMService SAPBOMService)
        {
            this.utilityService = utilityService;
            this.exceptionService = exceptionService;
            this.SAPBOMService = SAPBOMService;
        }

        public PackagingItem GetPackagingItemByPackagingId(int packagingId)
        {
            PackagingItem packagingItem = new PackagingItem();
            int iItemId = 0;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPListItem item = spList.GetItemById(packagingId);
                    if (item != null)
                    {
                        packagingItem.Id = item.ID;
                        iItemId = Convert.ToInt32(item[PackagingItemListFields.CompassListItemId]);
                        packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                        packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                        packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                        packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                        packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                        packagingItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                        packagingItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                        packagingItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                        packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                        packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                        packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                        packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                        packagingItem.SpecificationNo = Convert.ToString(item[PackagingItemListFields.SpecificationNo]);
                        packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                        packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                        packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                        packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                        packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                        packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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

                        packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                        packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                        packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                        packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                        packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                        packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                        packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                        packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                        packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                        packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                        packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                        packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                        packagingItem.ReviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]);
                        packagingItem.DielineURL = Convert.ToString(item[PackagingItemListFields.DielineLink]);
                        packagingItem.UPCAssociated = Convert.ToString(item[PackagingItemListFields.UPCAssociated]);
                        packagingItem.UPCAssociatedManualEntry = Convert.ToString(item[PackagingItemListFields.UPCAssociatedManualEntry]);
                        packagingItem.BioEngLabelingRequired = Convert.ToString(item[PackagingItemListFields.BioEngLabelingRequired]);
                        packagingItem.FlowthroughMaterialsSpecs = Convert.ToString(item[PackagingItemListFields.FlowthroughMaterialsSpecs]);
                        if (packagingItem.ParentID == 0)
                        {
                            packagingItem.ParentType = "Finished Good";
                        }
                        else
                        {
                            var parentitem = GetPackagingItemByPackagingId(packagingItem.ParentID);
                            if (parentitem != null)
                            {
                                packagingItem.ParentType = parentitem.PackagingComponent;

                            }
                        }
                        packagingItem.FourteenDigitBarCode = Convert.ToString(item[PackagingItemListFields.FourteenDigitBarcode]);
                    }
                }
            }
            return packagingItem;
        }
        public string GetReviewPrinterSupplierProcDets(int itemId)
        {
            bool showField = false;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spPackList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_PurchasedSemi + "</Value></Eq></And><Neq><FieldRef Name=\"Deleted\" /><Value Type=\"Text\">Yes</Value></Neq></And></Where>";

                    int PCSCount = spPackList.GetItems(spQuery).Count;

                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        string PHL1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        string novelty = Convert.ToString(item[CompassListFields.NoveltyProject]);
                        string packingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        string manufacturingLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        string COMANClassification = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        try
                        {
                            if (PHL1 != GlobalConstants.PRODUCT_HIERARCHY1_CoMan && novelty != "Yes" && (packingLocation.Contains("External") || manufacturingLocation.Contains("External") || PCSCount > 0) && COMANClassification != "External Turnkey FG")
                            {
                                showField = true;
                            }
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            return showField.ToString();
        }
        public PackagingItem GetGraphicsPackagingItemByPackagingId(int packagingId)
        {
            PackagingItem packagingItem = new PackagingItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPListItem item = spList.GetItemById(packagingId);
                    if (item != null)
                    {
                        packagingItem.Id = item.ID;
                        packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);

                        packagingItem.FinalGraphicsDescription = Convert.ToString(item[PackagingItemListFields.FinalGraphicsDescription]);
                        packagingItem.ConfirmedNLEA = Convert.ToString(item[PackagingItemListFields.ConfirmedNLEA]);
                        packagingItem.KosherLabelRequired = Convert.ToString(item[PackagingItemListFields.KosherLabelRequired]);
                        packagingItem.EstimatedNumberOfColors = Convert.ToString(item[PackagingItemListFields.EstimatedNumberOfColors]);
                        packagingItem.BlockForDateCode = Convert.ToString(item[PackagingItemListFields.BlockForDateCode]);
                        packagingItem.ConfirmedDielineRestrictions = Convert.ToString(item[PackagingItemListFields.ConfirmedDielineRestrictions]);
                        packagingItem.RenderingProvided = Convert.ToString(item[PackagingItemListFields.RenderingProvided]);
                        packagingItem.GraphicsBrief = Convert.ToString(item[PackagingItemListFields.GraphicsBrief]);
                        packagingItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                        packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                        packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                    }
                }
            }
            return packagingItem;
        }
        public List<PackagingItem> GetAllPackagingItemsForProject(int compassListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";

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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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

                            packagingItem.ReceivingPlant = Convert.ToString(item[PackagingItemListFields.ReceivingPlant]);
                            packagingItem.CostingUnit = Convert.ToString(item[PackagingItemListFields.CostingUnit]);
                            packagingItem.EachesPerCostingUnit = Convert.ToString(item[PackagingItemListFields.EachesPerCostingUnit]);
                            packagingItem.LBPerCostingUnit = Convert.ToString(item[PackagingItemListFields.LBPerCostingUnit]);
                            packagingItem.CostingUnitPerPallet = Convert.ToString(item[PackagingItemListFields.CostingUnitPerPallet]);
                            packagingItem.QuantityQuote = Convert.ToString(item[PackagingItemListFields.QuantityQuote]);
                            packagingItem.StandardCost = Convert.ToString(item[PackagingItemListFields.StandardCost]);
                            packagingItem.VendorNumber = Convert.ToString(item[PackagingItemListFields.VendorNumber]);
                            packagingItem.StandardOrderingQuantity = Convert.ToString(item[PackagingItemListFields.StandardOrderingQuantity]);
                            packagingItem.OrderUOM = Convert.ToString(item[PackagingItemListFields.OrderUOM]);
                            packagingItem.Incoterms = Convert.ToString(item[PackagingItemListFields.Incoterms]);
                            packagingItem.XferOfOwnership = Convert.ToString(item[PackagingItemListFields.XferOfOwnership]);
                            packagingItem.PRDateCategory = Convert.ToString(item[PackagingItemListFields.PRDateCategory]);
                            packagingItem.VendorMaterialNumber = Convert.ToString(item[PackagingItemListFields.VendorMaterialNumber]);
                            packagingItem.CostingCondition = Convert.ToString(item[PackagingItemListFields.CostingCondition]);

                            packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                            packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                            packagingItem.PurchasedIntoLocation = Convert.ToString(item[PackagingItemListFields.PurchasedIntoLocation]);
                            packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            packagingItem.ReviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            packagingItem.FourteenDigitBarCode = Convert.ToString(item[PackagingItemListFields.FourteenDigitBarcode]);

                            packagingItem.PHL1 = Convert.ToString(item[PackagingItemListFields.PHL1]);
                            packagingItem.PHL2 = Convert.ToString(item[PackagingItemListFields.PHL2]);
                            packagingItem.Brand = Convert.ToString(item[PackagingItemListFields.Brand]);
                            packagingItem.ProfitCenter = Convert.ToString(item[PackagingItemListFields.ProfitCenter]);
                            packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            packagingItem.UPCAssociated = Convert.ToString(item[PackagingItemListFields.UPCAssociated]);
                            packagingItem.UPCAssociatedManualEntry = Convert.ToString(item[PackagingItemListFields.UPCAssociatedManualEntry]);
                            packagingItem.BioEngLabelingRequired = Convert.ToString(item[PackagingItemListFields.BioEngLabelingRequired]);
                            packagingItem.FlowthroughMaterialsSpecs = Convert.ToString(item[PackagingItemListFields.FlowthroughMaterialsSpecs]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetFinishedGoodItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId +
                        "</Value></Eq><Eq><FieldRef Name=\"ParentID\" /><Value Type=\"Int\">0</Value></Eq></And></Where><OrderBy><FieldRef Name='PackagingComponent' Ascending='FALSE' /></OrderBy>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    //int TSCount = (from SPListItem ts in compassItemCol where (!string.Equals(ts[PackagingItemListFields.Deleted], "Yes") && string.Equals(ts[PackagingItemListFields.PackagingComponent], GlobalConstants.COMPONENTTYPE_TransferSemi)) select ts).Count();
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            //if(TSCount > 0 && string.Equals(item[PackagingItemListFields.PackagingComponent],GlobalConstants.COMPONENTTYPE_CandySemi))
                            //{
                            //  continue;
                            //}
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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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
                            packagingItem.PECompleted = Convert.ToString(item[PackagingItemListFields.PECompleted]);
                            packagingItem.PE2Completed = Convert.ToString(item[PackagingItemListFields.PE2Completed]);
                            packagingItem.ProcCompleted = Convert.ToString(item[PackagingItemListFields.ProcCompleted]);

                            packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                            packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                            packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            packagingItem.ReviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetTransferPurchasedSemiItemsForProject(int stageListItemId, string semiType)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + semiType + "</Value></Eq></And></Where>";

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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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

                            packagingItem.PECompleted = Convert.ToString(item[PackagingItemListFields.PECompleted]);
                            packagingItem.PE2Completed = Convert.ToString(item[PackagingItemListFields.PE2Completed]);
                            packagingItem.ProcCompleted = Convert.ToString(item[PackagingItemListFields.ProcCompleted]);

                            packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                            packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                            packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItem.ImmediateSPKChange = Convert.ToString(item[PackagingItemListFields.ImmediateSPKChange]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetSemiBOMItems(int stageListItemId, int parentID)
        {
            PackagingItem packagingItem;
            List<PackagingItem> packagingItems;
            packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"ParentID\" /><Value Type=\"Int\">" + parentID + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            packagingItem = ListToObject(item);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetSemiChildTSBOMItems(int stageListItemId, int parentID, string componentType)
        {
            PackagingItem packagingItem;
            List<PackagingItem> packagingItems;
            packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"ParentID\" /><Value Type=\"Int\">" + parentID + "</Value></Eq></And><Or><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_PurchasedSemi + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq></Or></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            packagingItem = ListToObject(item);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetSemiChildTSBOMItemsForBOMSetup(int stageListItemId, int parentID, string componentType)
        {
            PackagingItem packagingItem;
            List<PackagingItem> packagingItems;
            packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"ParentID\" /><Value Type=\"Int\">" + parentID + "</Value></Eq></And><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + componentType + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            packagingItem = ListToObject(item);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        private PackagingItem ListToObject(SPItem item)
        {
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
            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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
            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);

            packagingItem.PECompleted = Convert.ToString(item[PackagingItemListFields.PECompleted]);
            packagingItem.PE2Completed = Convert.ToString(item[PackagingItemListFields.PE2Completed]);
            packagingItem.ProcCompleted = Convert.ToString(item[PackagingItemListFields.ProcCompleted]);

            packagingItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
            packagingItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
            packagingItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
            packagingItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
            packagingItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
            packagingItem.ReviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]);
            return packagingItem;
        }
        public void CopyPackagingItems(int sourceCompassId, string sourceProjectNumber, ItemProposalItem targetProposalItem)
        {
            SPListItemCollection compassItemCol;
            SPQuery spQuery;
            Dictionary<int, int> parentIds;
            SPListItem newItem;
            SPList spList;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + sourceCompassId + "</Value></Eq>" +
                            "<Eq><FieldRef Name=\"" + PackagingItemListFields.ParentID + "\" /><Value Type=\"Int\">0</Value></Eq></And></Where>";
                        parentIds = new Dictionary<int, int>();
                        compassItemCol = spList.GetItems(spQuery);
                        Dictionary<int, int> TSIDs = new Dictionary<int, int>();
                        foreach (SPListItem sourceItem in compassItemCol)
                        {
                            if (string.Equals(sourceItem[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            newItem = spList.AddItem();
                            foreach (SPField field in sourceItem.Fields)
                            {
                                if (!field.ReadOnlyField && sourceItem[field.InternalName] != null && !field.InternalName.Equals("Attachments"))
                                {
                                    switch (field.InternalName)
                                    {
                                        case PackagingItemListFields.CompassListItemId:
                                            newItem[field.InternalName] = targetProposalItem.CompassListItemId;
                                            break;
                                        case PackagingItemListFields.Title:
                                            newItem[field.InternalName] = targetProposalItem.ProjectNumber;
                                            break;
                                        default:
                                            newItem[field.InternalName] = sourceItem[field.InternalName];
                                            break;
                                    }
                                }
                            }
                            if (SPContext.Current.Item != null)
                            {
                                newItem[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            }
                            newItem[PackagingItemListFields.IsAllProcInfoCorrect] = string.Empty;
                            newItem[PackagingItemListFields.WhatProcInfoHasChanged] = string.Empty;
                            newItem.Update();
                            parentIds[sourceItem.ID] = newItem.ID;
                            if (string.Equals(sourceItem[PackagingItemListFields.PackagingComponent].ToString(), GlobalConstants.COMPONENTTYPE_TransferSemi))
                            {
                                TSIDs.Add(sourceItem.ID, newItem.ID);
                            }
                            else if (string.Equals(sourceItem[PackagingItemListFields.PackagingComponent].ToString(), GlobalConstants.COMPONENTTYPE_PurchasedSemi))
                            {
                                TSIDs.Add(sourceItem.ID, newItem.ID);
                            }
                            CopyPackagingFiles(sourceProjectNumber, sourceItem.ID, targetProposalItem.ProjectNumber, newItem.ID);
                        }
                        CopyPackMeasItem(sourceCompassId, 0, targetProposalItem, 0);
                        if (parentIds.Count == 0)
                        {
                            spWeb.AllowUnsafeUpdates = false;
                            return;
                        }
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + sourceCompassId + "</Value></Eq>" +
                            "<Neq><FieldRef Name=\"" + PackagingItemListFields.ParentID + "\" /><Value Type=\"Int\">0</Value></Neq></And></Where>";
                        compassItemCol = spList.GetItems(spQuery);

                        foreach (SPListItem sourceItem in compassItemCol)
                        {
                            if (string.Equals(sourceItem[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            if (!parentIds.ContainsKey(Convert.ToInt32(sourceItem[PackagingItemListFields.ParentID])))
                                continue;
                            newItem = spList.AddItem();
                            foreach (SPField field in sourceItem.Fields)

                                if (!field.ReadOnlyField && sourceItem[field.InternalName] != null && !field.InternalName.Equals("Attachments"))
                                    switch (field.InternalName)
                                    {
                                        case PackagingItemListFields.CompassListItemId:
                                            newItem[field.InternalName] = targetProposalItem.CompassListItemId; break;
                                        case PackagingItemListFields.Title:
                                            newItem[field.InternalName] = targetProposalItem.ProjectNumber; break;
                                        case PackagingItemListFields.ParentID:
                                            newItem[field.InternalName] = parentIds[Convert.ToInt32(sourceItem[field.InternalName])]; break;
                                        default: newItem[field.InternalName] = sourceItem[field.InternalName]; break;
                                    }
                            if (SPContext.Current.Item != null)
                            {
                                newItem[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            }
                            newItem.Update();
                            CopyPackagingFiles(sourceProjectNumber, sourceItem.ID, targetProposalItem.ProjectNumber, newItem.ID);
                        }
                        foreach (KeyValuePair<int, int> copyItems in TSIDs)
                        {
                            CopyPackMeasItem(sourceCompassId, copyItems.Key, targetProposalItem, copyItems.Value);
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void CopyIPFPackagingItems(int sourceCompassId, string sourceProjectNumber, ItemProposalItem targetProposalItem)
        {
            SPListItemCollection compassItemCol;
            SPQuery spQuery;
            Dictionary<int, int> parentIds;
            SPListItem newItem;
            SPList spList;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where>" +
                                            "<And>" +
                                                "<Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + sourceCompassId + "</Value></Eq>" +
                                                "<Eq><FieldRef Name=\"" + PackagingItemListFields.ParentID + "\" /><Value Type=\"Int\">0</Value></Eq>" +
                                            "</And>" +
                                        "</Where>";
                        parentIds = new Dictionary<int, int>();
                        compassItemCol = spList.GetItems(spQuery);
                        Dictionary<int, int> TSIDs = new Dictionary<int, int>();
                        foreach (SPListItem sourceItem in compassItemCol)
                        {
                            if (string.Equals(sourceItem[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            newItem = spList.AddItem();
                            newItem[PackagingItemListFields.Title] = targetProposalItem.ProjectNumber;
                            newItem[PackagingItemListFields.CompassListItemId] = targetProposalItem.CompassListItemId;
                            newItem[PackagingItemListFields.PackagingComponent] = sourceItem[PackagingItemListFields.PackagingComponent];
                            newItem[PackagingItemListFields.NewExisting] = sourceItem[PackagingItemListFields.NewExisting];
                            newItem[PackagingItemListFields.MaterialNumber] = sourceItem[PackagingItemListFields.MaterialNumber];
                            newItem[PackagingItemListFields.MaterialDescription] = sourceItem[PackagingItemListFields.MaterialDescription];
                            newItem[PackagingItemListFields.CurrentLikeItem] = sourceItem[PackagingItemListFields.CurrentLikeItem];
                            newItem[PackagingItemListFields.CurrentLikeItemDescription] = sourceItem[PackagingItemListFields.CurrentLikeItemDescription];
                            newItem[PackagingItemListFields.CurrentLikeItemReason] = sourceItem[PackagingItemListFields.CurrentLikeItemReason];
                            newItem[PackagingItemListFields.CurrentOldItem] = sourceItem[PackagingItemListFields.CurrentOldItem];
                            newItem[PackagingItemListFields.CurrentOldItemDescription] = sourceItem[PackagingItemListFields.CurrentOldItemDescription];
                            newItem[PackagingItemListFields.PackQuantity] = sourceItem[PackagingItemListFields.PackQuantity];
                            newItem[PackagingItemListFields.PackUnit] = sourceItem[PackagingItemListFields.PackUnit];
                            newItem[PackagingItemListFields.GraphicsBrief] = sourceItem[PackagingItemListFields.GraphicsBrief];
                            newItem[PackagingItemListFields.GraphicsChangeRequired] = sourceItem[PackagingItemListFields.GraphicsChangeRequired];
                            newItem[PackagingItemListFields.ExternalGraphicsVendor] = sourceItem[PackagingItemListFields.ExternalGraphicsVendor];
                            newItem[PackagingItemListFields.ComponentContainsNLEA] = sourceItem[PackagingItemListFields.ComponentContainsNLEA];
                            newItem[PackagingItemListFields.Flowthrough] = sourceItem[PackagingItemListFields.Flowthrough];
                            newItem[PackagingItemListFields.Notes] = sourceItem[PackagingItemListFields.Notes];
                            newItem[PackagingItemListFields.ParentID] = sourceItem[PackagingItemListFields.ParentID];

                            newItem[PackagingItemListFields.PHL1] = sourceItem[PackagingItemListFields.PHL1];
                            newItem[PackagingItemListFields.PHL2] = sourceItem[PackagingItemListFields.PHL2];
                            newItem[PackagingItemListFields.Brand] = sourceItem[PackagingItemListFields.Brand];
                            newItem[PackagingItemListFields.ProfitCenter] = sourceItem[PackagingItemListFields.ProfitCenter];
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                newItem["Modified By"] = user.ID;
                            }
                            if (SPContext.Current.Item != null)
                            {
                                newItem[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            }
                            newItem.Update();
                            parentIds[sourceItem.ID] = newItem.ID;
                            if (string.Equals(sourceItem[PackagingItemListFields.PackagingComponent].ToString(), GlobalConstants.COMPONENTTYPE_TransferSemi))
                            {
                                TSIDs.Add(sourceItem.ID, newItem.ID);
                            }
                            else if (string.Equals(sourceItem[PackagingItemListFields.PackagingComponent].ToString(), GlobalConstants.COMPONENTTYPE_PurchasedSemi))
                            {
                                TSIDs.Add(sourceItem.ID, newItem.ID);
                            }
                            CopyPackagingFiles(sourceProjectNumber, sourceItem.ID, targetProposalItem.ProjectNumber, newItem.ID);
                        }
                        if (parentIds.Count == 0)
                        {
                            spWeb.AllowUnsafeUpdates = false;
                            return;
                        }

                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + sourceCompassId + "</Value></Eq>" +
                            "<Neq><FieldRef Name=\"" + PackagingItemListFields.ParentID + "\" /><Value Type=\"Int\">0</Value></Neq></And></Where>";
                        compassItemCol = spList.GetItems(spQuery);

                        foreach (SPListItem sourceItem in compassItemCol)
                        {
                            if (string.Equals(sourceItem[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            if (!parentIds.ContainsKey(Convert.ToInt32(sourceItem[PackagingItemListFields.ParentID])))
                                continue;
                            newItem = spList.AddItem();
                            newItem[PackagingItemListFields.Title] = targetProposalItem.ProjectNumber;
                            newItem[PackagingItemListFields.CompassListItemId] = targetProposalItem.CompassListItemId;
                            newItem[PackagingItemListFields.PackagingComponent] = sourceItem[PackagingItemListFields.PackagingComponent];
                            newItem[PackagingItemListFields.NewExisting] = sourceItem[PackagingItemListFields.NewExisting];
                            newItem[PackagingItemListFields.MaterialNumber] = sourceItem[PackagingItemListFields.MaterialNumber];
                            newItem[PackagingItemListFields.MaterialDescription] = sourceItem[PackagingItemListFields.MaterialDescription];
                            newItem[PackagingItemListFields.CurrentLikeItem] = sourceItem[PackagingItemListFields.CurrentLikeItem];
                            newItem[PackagingItemListFields.CurrentLikeItemDescription] = sourceItem[PackagingItemListFields.CurrentLikeItemDescription];
                            newItem[PackagingItemListFields.CurrentLikeItemReason] = sourceItem[PackagingItemListFields.CurrentLikeItemReason];
                            newItem[PackagingItemListFields.CurrentOldItem] = sourceItem[PackagingItemListFields.CurrentOldItem];
                            newItem[PackagingItemListFields.CurrentOldItemDescription] = sourceItem[PackagingItemListFields.CurrentOldItemDescription];
                            newItem[PackagingItemListFields.PackQuantity] = sourceItem[PackagingItemListFields.PackQuantity];
                            newItem[PackagingItemListFields.PackUnit] = sourceItem[PackagingItemListFields.PackUnit];
                            newItem[PackagingItemListFields.GraphicsBrief] = sourceItem[PackagingItemListFields.GraphicsBrief];
                            newItem[PackagingItemListFields.GraphicsChangeRequired] = sourceItem[PackagingItemListFields.GraphicsChangeRequired];
                            newItem[PackagingItemListFields.ExternalGraphicsVendor] = sourceItem[PackagingItemListFields.ExternalGraphicsVendor];
                            newItem[PackagingItemListFields.ComponentContainsNLEA] = sourceItem[PackagingItemListFields.ComponentContainsNLEA];
                            newItem[PackagingItemListFields.Flowthrough] = sourceItem[PackagingItemListFields.Flowthrough];
                            newItem[PackagingItemListFields.Notes] = sourceItem[PackagingItemListFields.Notes];
                            newItem[PackagingItemListFields.ParentID] = parentIds[Convert.ToInt32(sourceItem[PackagingItemListFields.ParentID])];

                            newItem[PackagingItemListFields.PHL1] = sourceItem[PackagingItemListFields.PHL1];
                            newItem[PackagingItemListFields.PHL2] = sourceItem[PackagingItemListFields.PHL2];
                            newItem[PackagingItemListFields.Brand] = sourceItem[PackagingItemListFields.Brand];
                            newItem[PackagingItemListFields.ProfitCenter] = sourceItem[PackagingItemListFields.ProfitCenter];
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                newItem["Modified By"] = user.ID;
                            }

                            if (SPContext.Current.Item != null)
                            {
                                newItem[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            }
                            newItem.Update();

                            CopyPackagingFiles(sourceProjectNumber, sourceItem.ID, targetProposalItem.ProjectNumber, newItem.ID);
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        private void CopyPackagingFiles(string sourceProjectNumber, int sourcePackingItemId, string targetProjectNumber, int targetPackagingItemId)
        {
            SPDocumentLibrary documentLib;
            SPListItem targetFolderI, newItem;
            List<SPFile> spfiles;
            SPFolder sourceFolder, targetFolder;
            string newName, urlNewname;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spweb = spsite.OpenWeb())
                    {
                        spweb.AllowUnsafeUpdates = true;
                        documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                        string targetFolderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", targetProjectNumber);
                        string sourceFolderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", sourceProjectNumber);
                        if (!spweb.GetFolder(sourceFolderUrl).Exists)
                        {
                            spweb.AllowUnsafeUpdates = false;
                            return;
                        }
                        sourceFolder = spweb.GetFolder(sourceFolderUrl);
                        if (!spweb.GetFolder(targetFolderUrl).Exists)
                        {
                            targetFolderI = documentLib.Items.Add("", SPFileSystemObjectType.Folder, targetProjectNumber);
                            targetFolderI.Update();
                        }
                        if (!spweb.GetFolder(targetFolderUrl).Exists)
                        {
                            spweb.AllowUnsafeUpdates = false;
                            return;
                        }
                        targetFolder = spweb.GetFolder(targetFolderUrl);
                        spfiles = sourceFolder.Files.OfType<SPFile>().Where(x => Convert.ToInt32(x.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]).Equals(sourcePackingItemId)).ToList();
                        foreach (SPFile spfile in spfiles)
                        {
                            newName = spfile.Name.Replace(sourcePackingItemId.ToString(), targetPackagingItemId.ToString());
                            urlNewname = string.Concat(targetFolderUrl, "/", newName);
                            spfile.CopyTo(urlNewname);
                            newItem = spweb.GetListItem(urlNewname);
                            if (newItem != null)
                            {
                                newItem[CompassListFields.DOCLIBRARY_PackagingComponentItemId] = targetPackagingItemId;
                                newItem.Update();
                            }
                        }
                        spweb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void CopyPackMeasItem(int OldCompassId, int oldParentCompId, ItemProposalItem newProjectInfo, int newParentComponentId)
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
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + OldCompassId + "</Value></Eq>" +
                            "<Eq><FieldRef Name=\"" + CompassPackMeasurementsFields.ParentComponentId + "\" /><Value Type=\"Int\">" + oldParentCompId + "</Value></Eq></And></Where>";
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        foreach (SPListItem sourceItem in compassItemCol)
                        {
                            var newItem = spList.AddItem();
                            foreach (SPField field in sourceItem.Fields)
                                if (!field.ReadOnlyField && sourceItem[field.InternalName] != null && !field.InternalName.Equals("Attachments"))
                                    switch (field.InternalName)
                                    {
                                        case CompassPackMeasurementsFields.CompassListItemId:
                                            newItem[field.InternalName] = newProjectInfo.CompassListItemId; break;
                                        case CompassPackMeasurementsFields.Title:
                                            newItem[field.InternalName] = newProjectInfo.ProjectNumber; break;
                                        case CompassPackMeasurementsFields.ParentComponentId:
                                            newItem[field.InternalName] = newParentComponentId; break;
                                        default: newItem[field.InternalName] = sourceItem[field.InternalName]; break;
                                    }
                            newItem.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public List<PackagingItem> GetPackagingParents(int compassId)
        {
            SPListItemCollection compassItemCol;
            SPQuery spQuery;
            PackagingItem packagingItem;
            List<PackagingItem> packagingItems;
            Dictionary<int, int> dids;
            StringBuilder ids;
            packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + compassId + "</Value></Eq>" +
                        "<Neq><FieldRef Name=\"ParentID\" /><Value Type=\"Int\">0</Value></Neq></And></Where>";
                    dids = new Dictionary<int, int>();
                    compassItemCol = spList.GetItems(spQuery);
                    foreach (SPListItem item in compassItemCol)
                        if (string.Equals(item[PackagingItemListFields.Deleted], "No"))
                            dids[Convert.ToInt32(item[PackagingItemListFields.ParentID])] = 0;
                    if (dids.Count > 0)
                    {
                        ids = new StringBuilder();
                        foreach (int key in dids.Keys)
                            ids.Append("<Value Type=\"Int\">" + key + "</Value>");
                        spQuery = new SPQuery();
                        spQuery.Query = "<Where><In><FieldRef Name=\"ID\" /><Values>" + ids + "</Values></In></Where>";
                        compassItemCol = spList.GetItems(spQuery);
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            packagingItem = ListToObject(item);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetPackagingChildren(int parentID)
        {
            PackagingItem packagingItem;
            List<PackagingItem> packagingItems;
            packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"ParentID\" /><Value Type=\"Int\">" + parentID + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "No"))
                            {
                                packagingItem = ListToObject(item);
                                packagingItems.Add(packagingItem);
                            }
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetTransferSemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq></And></Where>";

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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<KeyValuePair<int, string>> GetTransferSemiIDsForProject(int stageListItemId)
        {
            List<KeyValuePair<int, string>> packagingItemIDs = new List<KeyValuePair<int, string>>();
            packagingItemIDs.Add(new KeyValuePair<int, string>(0, "Finished Good"));
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            string moveText = Convert.ToString(item[PackagingItemListFields.MaterialNumber]) + ": " + Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            string packComp = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            if (packComp == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                            {
                                moveText = "PCS: " + moveText;
                            }
                            else if (packComp == GlobalConstants.COMPONENTTYPE_TransferSemi)
                            {
                                moveText = "TS: " + moveText;
                            }
                            packagingItemIDs.Add(new KeyValuePair<int, string>(item.ID, moveText));
                        }
                    }
                }
            }
            return packagingItemIDs;
        }
        public List<KeyValuePair<int, string>> GetPurchasedSemiIDsForProject(int stageListItemId)
        {
            List<KeyValuePair<int, string>> packagingItemIDs = new List<KeyValuePair<int, string>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_PurchasedSemi + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;
                            string moveText = Convert.ToString(item[PackagingItemListFields.MaterialNumber]) + ": " + Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItemIDs.Add(new KeyValuePair<int, string>(item.ID, moveText));
                        }
                    }
                }
            }
            return packagingItemIDs;
        }
        public List<PackagingItem> GetNewTransferSemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq></And><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq></And></Where>";

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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetNetworkMoveTransferSemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq></And><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">Network Move</Value></Eq></And></Where>";

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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetNewPURCANDYSemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_PurchasedSemi + "</Value></Eq></And><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq></And></Where>";

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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetGraphicsPackagingItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    //var items = spList.Items.OfType<SPListItem>().Where(x => Convert.ToInt32(x[PackagingItemListFields.CompassListItemId]) == stageListItemId && x[PackagingItemListFields.GraphicsChangeRequired] != null && (x[PackagingItemListFields.GraphicsChangeRequired].ToString().ToLower()) == "yes").ToList();

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"GraphicsChangeRequired\" /><Value Type=\"Text\">Yes</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol != null && compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            PackagingItem packagingItem = new PackagingItem();
                            if (packagingItem.ParentID == 0)
                            {
                                packagingItem.ParentType = "Finished Good";
                            }
                            else
                            {
                                var parentitem = GetPackagingItemByPackagingId(packagingItem.ParentID);
                                if (parentitem != null)
                                {
                                    if (string.Equals(packagingItem.Deleted, "Yes"))
                                        continue;

                                    packagingItem.ParentType = parentitem.PackagingComponent;
                                    packagingItem.ParentPackLocation = parentitem.PackLocation;
                                }


                            }

                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                            packagingItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                            packagingItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
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

                            packagingItem.FinalGraphicsDescription = Convert.ToString(item[PackagingItemListFields.FinalGraphicsDescription]);
                            packagingItem.ConfirmedNLEA = Convert.ToString(item[PackagingItemListFields.ConfirmedNLEA]);
                            packagingItem.KosherLabelRequired = Convert.ToString(item[PackagingItemListFields.KosherLabelRequired]);
                            packagingItem.EstimatedNumberOfColors = Convert.ToString(item[PackagingItemListFields.EstimatedNumberOfColors]);
                            packagingItem.BlockForDateCode = Convert.ToString(item[PackagingItemListFields.BlockForDateCode]);
                            packagingItem.ConfirmedDielineRestrictions = Convert.ToString(item[PackagingItemListFields.ConfirmedDielineRestrictions]);
                            packagingItem.RenderingProvided = Convert.ToString(item[PackagingItemListFields.RenderingProvided]);
                            packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                            packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                            packagingItem.MRPC = Convert.ToString(item[PackagingItemListFields.MRPC]);

                            packagingItem.GraphicsNotes = Convert.ToString(item[PackagingItemListFields.Graphics_Notes]);
                            packagingItem.GraphicsPDFApprovedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate]);
                            packagingItem.GraphicsPlatesShippedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate]);
                            packagingItem.GraphicsRoutingDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_Routing_ModifiedDate]);
                            packagingItem.GraphicsRoutingReleasedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate]);
                            packagingItem.VendorMaterialNumber = Convert.ToString(item[PackagingItemListFields.VendorMaterialNumber]);
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItem.SpecificationNo = Convert.ToString(item[PackagingItemListFields.SpecificationNo]);
                            packagingItem.DielineURL = Convert.ToString(item[PackagingItemListFields.DielineLink]);
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            packagingItem.ParentType = "";
                            packagingItem.ParentPackLocation = "";


                            packagingItem.FourteenDigitBarCode = Convert.ToString(item[PackagingItemListFields.FourteenDigitBarcode]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }

        #region Get Component Costing Packaging Items For Project
        public List<PackagingItem> GetAllComponentCostingPackagingItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol != null && compassItemCol.Count > 0)
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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
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

                            packagingItem.FinalGraphicsDescription = Convert.ToString(item[PackagingItemListFields.FinalGraphicsDescription]);
                            packagingItem.ConfirmedNLEA = Convert.ToString(item[PackagingItemListFields.ConfirmedNLEA]);
                            packagingItem.KosherLabelRequired = Convert.ToString(item[PackagingItemListFields.KosherLabelRequired]);
                            packagingItem.EstimatedNumberOfColors = Convert.ToString(item[PackagingItemListFields.EstimatedNumberOfColors]);
                            packagingItem.BlockForDateCode = Convert.ToString(item[PackagingItemListFields.BlockForDateCode]);
                            packagingItem.ConfirmedDielineRestrictions = Convert.ToString(item[PackagingItemListFields.ConfirmedDielineRestrictions]);
                            packagingItem.RenderingProvided = Convert.ToString(item[PackagingItemListFields.RenderingProvided]);
                            packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                            packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                            packagingItem.MRPC = Convert.ToString(item[PackagingItemListFields.MRPC]);

                            packagingItem.GraphicsNotes = Convert.ToString(item[PackagingItemListFields.Graphics_Notes]);
                            packagingItem.GraphicsPDFApprovedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate]);
                            packagingItem.GraphicsPlatesShippedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate]);
                            packagingItem.GraphicsRoutingDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_Routing_ModifiedDate]);
                            packagingItem.GraphicsRoutingReleasedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate]);
                            packagingItem.CompCostSubmittedDate = Convert.ToString(item[PackagingItemListFields.CompCostSubmittedDate]);
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetFilmLabelRigidPlasticComponentCostingPackagingItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    string packagingComponent = string.Empty;
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol != null && compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            packagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);

                            if ((packagingComponent.Contains("Film")) || (packagingComponent.Contains("Label")) ||
                                (packagingComponent.Contains("Rigid")) || (packagingComponent.Contains("Other")))
                            {
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
                                packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                                packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
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

                                packagingItem.FinalGraphicsDescription = Convert.ToString(item[PackagingItemListFields.FinalGraphicsDescription]);
                                packagingItem.ConfirmedNLEA = Convert.ToString(item[PackagingItemListFields.ConfirmedNLEA]);
                                packagingItem.KosherLabelRequired = Convert.ToString(item[PackagingItemListFields.KosherLabelRequired]);
                                packagingItem.EstimatedNumberOfColors = Convert.ToString(item[PackagingItemListFields.EstimatedNumberOfColors]);
                                packagingItem.BlockForDateCode = Convert.ToString(item[PackagingItemListFields.BlockForDateCode]);
                                packagingItem.ConfirmedDielineRestrictions = Convert.ToString(item[PackagingItemListFields.ConfirmedDielineRestrictions]);
                                packagingItem.RenderingProvided = Convert.ToString(item[PackagingItemListFields.RenderingProvided]);
                                packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                                packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                                packagingItem.MRPC = Convert.ToString(item[PackagingItemListFields.MRPC]);

                                packagingItem.GraphicsNotes = Convert.ToString(item[PackagingItemListFields.Graphics_Notes]);
                                packagingItem.GraphicsPDFApprovedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate]);
                                packagingItem.GraphicsPlatesShippedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate]);
                                packagingItem.GraphicsRoutingDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_Routing_ModifiedDate]);
                                packagingItem.GraphicsRoutingReleasedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate]);
                                packagingItem.CompCostSubmittedDate = Convert.ToString(item[PackagingItemListFields.CompCostSubmittedDate]);
                                packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);

                                packagingItems.Add(packagingItem);
                            }
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetCorrugatedPaperboardComponentCostingPackagingItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    string packagingComponent = string.Empty;
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"NewExisting\" /><Value Type=\"Text\">New</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol != null && compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            packagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);

                            if ((packagingComponent.Contains("Corrugated")) || (packagingComponent.Contains("Paperboard")) ||
                                (packagingComponent.Contains("Other")))
                            {
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
                                packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                                packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
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

                                packagingItem.FinalGraphicsDescription = Convert.ToString(item[PackagingItemListFields.FinalGraphicsDescription]);
                                packagingItem.ConfirmedNLEA = Convert.ToString(item[PackagingItemListFields.ConfirmedNLEA]);
                                packagingItem.KosherLabelRequired = Convert.ToString(item[PackagingItemListFields.KosherLabelRequired]);
                                packagingItem.EstimatedNumberOfColors = Convert.ToString(item[PackagingItemListFields.EstimatedNumberOfColors]);
                                packagingItem.BlockForDateCode = Convert.ToString(item[PackagingItemListFields.BlockForDateCode]);
                                packagingItem.ConfirmedDielineRestrictions = Convert.ToString(item[PackagingItemListFields.ConfirmedDielineRestrictions]);
                                packagingItem.RenderingProvided = Convert.ToString(item[PackagingItemListFields.RenderingProvided]);
                                packagingItem.PlatesShipped = Convert.ToString(item[PackagingItemListFields.PlatesShipped]);
                                packagingItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);
                                packagingItem.MRPC = Convert.ToString(item[PackagingItemListFields.MRPC]);

                                packagingItem.GraphicsNotes = Convert.ToString(item[PackagingItemListFields.Graphics_Notes]);
                                packagingItem.GraphicsPDFApprovedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate]);
                                packagingItem.GraphicsPlatesShippedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate]);
                                packagingItem.GraphicsRoutingDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_Routing_ModifiedDate]);
                                packagingItem.GraphicsRoutingReleasedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate]);

                                packagingItem.CompCostSubmittedDate = Convert.ToString(item[PackagingItemListFields.CompCostSubmittedDate]);
                                packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);

                                packagingItems.Add(packagingItem);
                            }
                        }
                    }
                }
            }
            return packagingItems;
        }
        #endregion
        public List<PackagingItem> GetGraphicsProgressPackagingItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"GraphicsChangeRequired\" /><Value Type=\"Text\">Yes</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol != null && compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);

                            packagingItem.GraphicsNotes = Convert.ToString(item[PackagingItemListFields.Graphics_Notes]);
                            packagingItem.GraphicsPDFApprovedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate]);
                            packagingItem.GraphicsPlatesShippedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate]);
                            packagingItem.GraphicsRoutingDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_Routing_ModifiedDate]);
                            packagingItem.GraphicsRoutingReleasedDate = Convert.ToDateTime(item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }

        public SAPBOMSetupItem GetCompassListForProject(int itemId)
        {
            SAPBOMSetupItem sgItem = new SAPBOMSetupItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        sgItem.CompassListItemId = item.ID;
                        sgItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);
                    }
                }
            }
            return sgItem;
        }
        public List<PackagingItem> GetCandySemiItemsForProject(int stageListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
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
                            packagingItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            packagingItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            packagingItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            packagingItem.NetWeight = Convert.ToString(item[PackagingItemListFields.NetWeight]);
                            packagingItem.TareWeight = Convert.ToString(item[PackagingItemListFields.TareWeight]);
                            packagingItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            packagingItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            packagingItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            packagingItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            packagingItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
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
                            packagingItem.FilmSubstrate = Convert.ToString(item[PackagingItemListFields.Substrate]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public List<PackagingItem> GetCandyAndPurchasedSemisForProject(int CompassListItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where>" +
                                        "<And>" +
                                            "<Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + CompassListItemId + "</Value></Eq>" +
                                            "<Or>" +
                                                "<Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_CandySemi + "</Value></Eq>" +
                                                "<Eq><FieldRef Name=\"PackagingComponent\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_PurchasedSemi + "</Value></Eq>" +
                                            "</Or>" +
                                        "</And>" +
                                    "</Where>";

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
                            packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            packagingItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            packagingItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            packagingItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            packagingItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            packagingItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            packagingItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public int InsertPackagingItem(PackagingItem packagingItem, int compassListItemId)
        {
            int itemId = 0;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.AddItem();
                        item["Title"] = utilityService.GetProjectNumberFromItemId(compassListItemId, SPContext.Current.Web.Url);
                        item[PackagingItemListFields.CompassListItemId] = compassListItemId;
                        item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                        item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                        item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                        item[PackagingItemListFields.NewExisting] = packagingItem.NewExisting;
                        item[PackagingItemListFields.CurrentLikeItem] = packagingItem.CurrentLikeItem;
                        item[PackagingItemListFields.CurrentLikeItemDescription] = packagingItem.CurrentLikeItemDescription;
                        item[PackagingItemListFields.CurrentLikeItemReason] = packagingItem.CurrentLikeItemReason;
                        item[PackagingItemListFields.CurrentOldItem] = packagingItem.CurrentOldItem;
                        item[PackagingItemListFields.CurrentOldItemDescription] = packagingItem.CurrentOldItemDescription;
                        item[PackagingItemListFields.PackQuantity] = packagingItem.PackQuantity;
                        item[PackagingItemListFields.NetWeight] = packagingItem.NetWeight;
                        item[PackagingItemListFields.SpecificationNo] = packagingItem.SpecificationNo;
                        item[PackagingItemListFields.TareWeight] = packagingItem.TareWeight;
                        item[PackagingItemListFields.LeadPlateTime] = packagingItem.LeadPlateTime;
                        item[PackagingItemListFields.LeadMaterialTime] = packagingItem.LeadMaterialTime;
                        item[PackagingItemListFields.PrinterSupplier] = packagingItem.PrinterSupplier;
                        item[PackagingItemListFields.Notes] = packagingItem.Notes;

                        item[PackagingItemListFields.Length] = packagingItem.Length;
                        item[PackagingItemListFields.Width] = packagingItem.Width;
                        item[PackagingItemListFields.Height] = packagingItem.Height;
                        item[PackagingItemListFields.CADDrawing] = packagingItem.CADDrawing;
                        item[PackagingItemListFields.Structure] = packagingItem.Structure;
                        item[PackagingItemListFields.StructureColor] = packagingItem.StructureColor;
                        item[PackagingItemListFields.BackSeam] = packagingItem.BackSeam;
                        item[PackagingItemListFields.WebWidth] = packagingItem.WebWidth;
                        item[PackagingItemListFields.ExactCutOff] = packagingItem.ExactCutOff;
                        item[PackagingItemListFields.BagFace] = packagingItem.BagFace;
                        item[PackagingItemListFields.Unwind] = packagingItem.Unwind;
                        item[PackagingItemListFields.Description] = packagingItem.Description;

                        item[PackagingItemListFields.FilmMaxRollOD] = packagingItem.FilmMaxRollOD;
                        item[PackagingItemListFields.FilmRollID] = packagingItem.FilmRollID;
                        item[PackagingItemListFields.FilmPrintStyle] = packagingItem.FilmPrintStyle;
                        item[PackagingItemListFields.FilmStyle] = packagingItem.FilmStyle;
                        item[PackagingItemListFields.CorrugatedPrintStyle] = packagingItem.CorrugatedPrintStyle;
                        item[PackagingItemListFields.GraphicsChangeRequired] = packagingItem.GraphicsChangeRequired;
                        item[PackagingItemListFields.ExternalGraphicsVendor] = packagingItem.ExternalGraphicsVendor;
                        item[PackagingItemListFields.GraphicsBrief] = packagingItem.GraphicsBrief;
                        if ((packagingItem.BOMEffectiveDate != null) && (packagingItem.BOMEffectiveDate != DateTime.MinValue))
                            item[PackagingItemListFields.BOMEffectiveDate] = packagingItem.BOMEffectiveDate;
                        item[PackagingItemListFields.PlatesShipped] = packagingItem.PlatesShipped;
                        item[PackagingItemListFields.PackUnit] = packagingItem.PackUnit;
                        item[PackagingItemListFields.MRPC] = packagingItem.MRPC;
                        item[PackagingItemListFields.ParentID] = packagingItem.ParentID;

                        item[PackagingItemListFields.MakeLocation] = packagingItem.MakeLocation;
                        item[PackagingItemListFields.PackLocation] = packagingItem.PackLocation;
                        item[PackagingItemListFields.CountryOfOrigin] = packagingItem.CountryOfOrigin;
                        item[PackagingItemListFields.NewFormula] = packagingItem.NewFormula;
                        item[PackagingItemListFields.TrialsCompleted] = packagingItem.TrialsCompleted;
                        item[PackagingItemListFields.ShelfLife] = packagingItem.ShelfLife;
                        item[PackagingItemListFields.Kosher] = packagingItem.Kosher;
                        item[PackagingItemListFields.Allergens] = packagingItem.Allergens;
                        item[PackagingItemListFields.SAPMaterialGroup] = packagingItem.SAPMaterialGroup;
                        item[PackagingItemListFields.TransferSEMIMakePackLocations] = packagingItem.TransferSEMIMakePackLocations;
                        item[PackagingItemListFields.Deleted] = packagingItem.Deleted;
                        item[PackagingItemListFields.ComponentContainsNLEA] = packagingItem.ComponentContainsNLEA;
                        item[PackagingItemListFields.Substrate] = packagingItem.FilmSubstrate;
                        item[PackagingItemListFields.Flowthrough] = packagingItem.Flowthrough;
                        item[PackagingItemListFields.ImmediateSPKChange] = packagingItem.ImmediateSPKChange;
                        item[PackagingItemListFields.ReviewPrinterSupplier] = packagingItem.ReviewPrinterSupplier;

                        item[PackagingItemListFields.PHL1] = packagingItem.PHL1;
                        item[PackagingItemListFields.PHL2] = packagingItem.PHL2;
                        item[PackagingItemListFields.Brand] = packagingItem.Brand;
                        item[PackagingItemListFields.ProfitCenter] = packagingItem.ProfitCenter;
                        // Set Modified By to current user NOT System Account
                        //item["Editor"] = SPContext.Current.Web.CurrentUser;
                        // Set Created By to current user NOT System Account
                        //item["Author"] = SPContext.Current.Web.CurrentUser;
                        //item.Update();

                        SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                        if (user != null)
                        {
                            // Set Modified By to current user NOT System Account
                            item["Created By"] = user.ID;
                            item["Modified By"] = user.ID;
                        }
                        item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                        item.Update();

                        itemId = item.ID;
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return itemId;
        }
        public List<PackagingItem> InsertPackagingItems(List<PackagingItem> packagingItems, int compassListItemId, string ProjectNumber)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        foreach (var packagingItem in packagingItems)
                        {
                            SPListItem item = spList.AddItem();
                            item["Title"] = ProjectNumber;
                            item[PackagingItemListFields.CompassListItemId] = compassListItemId;
                            item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                            item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                            item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                            item[PackagingItemListFields.NewExisting] = packagingItem.NewExisting;
                            item[PackagingItemListFields.CurrentLikeItem] = packagingItem.CurrentLikeItem;
                            item[PackagingItemListFields.CurrentLikeItemDescription] = packagingItem.CurrentLikeItemDescription;
                            item[PackagingItemListFields.CurrentLikeItemReason] = packagingItem.CurrentLikeItemReason;
                            item[PackagingItemListFields.CurrentOldItem] = packagingItem.CurrentOldItem;
                            item[PackagingItemListFields.CurrentOldItemDescription] = packagingItem.CurrentOldItemDescription;
                            item[PackagingItemListFields.PackQuantity] = packagingItem.PackQuantity;
                            item[PackagingItemListFields.NetWeight] = packagingItem.NetWeight;
                            item[PackagingItemListFields.SpecificationNo] = packagingItem.SpecificationNo;
                            item[PackagingItemListFields.TareWeight] = packagingItem.TareWeight;
                            item[PackagingItemListFields.LeadPlateTime] = packagingItem.LeadPlateTime;
                            item[PackagingItemListFields.LeadMaterialTime] = packagingItem.LeadMaterialTime;
                            item[PackagingItemListFields.PrinterSupplier] = packagingItem.PrinterSupplier;
                            item[PackagingItemListFields.Notes] = packagingItem.Notes;

                            item[PackagingItemListFields.Length] = packagingItem.Length;
                            item[PackagingItemListFields.Width] = packagingItem.Width;
                            item[PackagingItemListFields.Height] = packagingItem.Height;
                            item[PackagingItemListFields.CADDrawing] = packagingItem.CADDrawing;
                            item[PackagingItemListFields.Structure] = packagingItem.Structure;
                            item[PackagingItemListFields.StructureColor] = packagingItem.StructureColor;
                            item[PackagingItemListFields.BackSeam] = packagingItem.BackSeam;
                            item[PackagingItemListFields.WebWidth] = packagingItem.WebWidth;
                            item[PackagingItemListFields.ExactCutOff] = packagingItem.ExactCutOff;
                            item[PackagingItemListFields.BagFace] = packagingItem.BagFace;
                            item[PackagingItemListFields.Unwind] = packagingItem.Unwind;
                            item[PackagingItemListFields.Description] = packagingItem.Description;

                            item[PackagingItemListFields.FilmMaxRollOD] = packagingItem.FilmMaxRollOD;
                            item[PackagingItemListFields.FilmRollID] = packagingItem.FilmRollID;
                            item[PackagingItemListFields.FilmPrintStyle] = packagingItem.FilmPrintStyle;
                            item[PackagingItemListFields.FilmStyle] = packagingItem.FilmStyle;
                            item[PackagingItemListFields.CorrugatedPrintStyle] = packagingItem.CorrugatedPrintStyle;
                            item[PackagingItemListFields.GraphicsChangeRequired] = packagingItem.GraphicsChangeRequired;
                            item[PackagingItemListFields.ExternalGraphicsVendor] = packagingItem.ExternalGraphicsVendor;
                            item[PackagingItemListFields.GraphicsBrief] = packagingItem.GraphicsBrief;
                            if ((packagingItem.BOMEffectiveDate != null) && (packagingItem.BOMEffectiveDate != DateTime.MinValue))
                                item[PackagingItemListFields.BOMEffectiveDate] = packagingItem.BOMEffectiveDate;
                            item[PackagingItemListFields.PlatesShipped] = packagingItem.PlatesShipped;
                            item[PackagingItemListFields.PackUnit] = packagingItem.PackUnit;
                            item[PackagingItemListFields.MRPC] = packagingItem.MRPC;
                            int ActualParentId = 0;
                            if (packagingItem.ParentID != 0)
                            {
                                ActualParentId =
                                  (
                                   from
                                       SAPPI in packagingItems
                                   where
                                       SAPPI.IdTemp == packagingItem.ParentID
                                   select
                                       SAPPI.Id
                                   ).FirstOrDefault();
                            }
                            packagingItem.ParentID = ActualParentId;
                            item[PackagingItemListFields.ParentID] = ActualParentId;
                            item[PackagingItemListFields.MakeLocation] = packagingItem.MakeLocation;
                            item[PackagingItemListFields.PackLocation] = packagingItem.PackLocation;
                            item[PackagingItemListFields.CountryOfOrigin] = packagingItem.CountryOfOrigin;
                            item[PackagingItemListFields.NewFormula] = packagingItem.NewFormula;
                            item[PackagingItemListFields.TrialsCompleted] = packagingItem.TrialsCompleted;
                            item[PackagingItemListFields.ShelfLife] = packagingItem.ShelfLife;
                            item[PackagingItemListFields.Kosher] = packagingItem.Kosher;
                            item[PackagingItemListFields.Allergens] = packagingItem.Allergens;
                            item[PackagingItemListFields.SAPMaterialGroup] = packagingItem.SAPMaterialGroup;
                            item[PackagingItemListFields.TransferSEMIMakePackLocations] = packagingItem.TransferSEMIMakePackLocations;
                            item[PackagingItemListFields.Deleted] = packagingItem.Deleted;
                            item[PackagingItemListFields.ComponentContainsNLEA] = packagingItem.ComponentContainsNLEA;
                            item[PackagingItemListFields.Substrate] = packagingItem.FilmSubstrate;
                            item[PackagingItemListFields.Flowthrough] = packagingItem.Flowthrough;
                            item[PackagingItemListFields.ImmediateSPKChange] = packagingItem.ImmediateSPKChange;
                            item[PackagingItemListFields.ReviewPrinterSupplier] = packagingItem.ReviewPrinterSupplier;

                            item[PackagingItemListFields.PHL1] = packagingItem.PHL1;
                            item[PackagingItemListFields.PHL2] = packagingItem.PHL2;
                            item[PackagingItemListFields.Brand] = packagingItem.Brand;
                            item[PackagingItemListFields.ProfitCenter] = packagingItem.ProfitCenter;
                            // Set Modified By to current user NOT System Account
                            //item["Editor"] = SPContext.Current.Web.CurrentUser;
                            // Set Created By to current user NOT System Account
                            //item["Author"] = SPContext.Current.Web.CurrentUser;
                            //item.Update();

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Created By"] = user.ID;
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();

                            packagingItem.Id = item.ID;
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return packagingItems;
        }

        public void UpdatePackagingItem(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            //item["Title"] = packagingItem.PackagingComponent;
                            item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                            item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                            item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                            item[PackagingItemListFields.NewExisting] = packagingItem.NewExisting;
                            item[PackagingItemListFields.CurrentLikeItem] = packagingItem.CurrentLikeItem;
                            item[PackagingItemListFields.CurrentLikeItemDescription] = packagingItem.CurrentLikeItemDescription;
                            item[PackagingItemListFields.CurrentLikeItemReason] = packagingItem.CurrentLikeItemReason;
                            item[PackagingItemListFields.CurrentOldItem] = packagingItem.CurrentOldItem;
                            item[PackagingItemListFields.CurrentOldItemDescription] = packagingItem.CurrentOldItemDescription;
                            item[PackagingItemListFields.PackQuantity] = packagingItem.PackQuantity;
                            item[PackagingItemListFields.NetWeight] = packagingItem.NetWeight;
                            item[PackagingItemListFields.SpecificationNo] = packagingItem.SpecificationNo;
                            item[PackagingItemListFields.TareWeight] = packagingItem.TareWeight;
                            item[PackagingItemListFields.LeadPlateTime] = packagingItem.LeadPlateTime;
                            item[PackagingItemListFields.LeadMaterialTime] = packagingItem.LeadMaterialTime;
                            item[PackagingItemListFields.PrinterSupplier] = packagingItem.PrinterSupplier;
                            item[PackagingItemListFields.Notes] = packagingItem.Notes;

                            item[PackagingItemListFields.Length] = packagingItem.Length;
                            item[PackagingItemListFields.Width] = packagingItem.Width;
                            item[PackagingItemListFields.Height] = packagingItem.Height;
                            item[PackagingItemListFields.CADDrawing] = packagingItem.CADDrawing;
                            item[PackagingItemListFields.Substrate] = packagingItem.FilmSubstrate;
                            item[PackagingItemListFields.Structure] = packagingItem.Structure;
                            item[PackagingItemListFields.StructureColor] = packagingItem.StructureColor;
                            item[PackagingItemListFields.BackSeam] = packagingItem.BackSeam;
                            item[PackagingItemListFields.WebWidth] = packagingItem.WebWidth;
                            item[PackagingItemListFields.ExactCutOff] = packagingItem.ExactCutOff;
                            item[PackagingItemListFields.BagFace] = packagingItem.BagFace;
                            item[PackagingItemListFields.Unwind] = packagingItem.Unwind;
                            item[PackagingItemListFields.Description] = packagingItem.Description;

                            item[PackagingItemListFields.FilmMaxRollOD] = packagingItem.FilmMaxRollOD;
                            item[PackagingItemListFields.FilmRollID] = packagingItem.FilmRollID;
                            item[PackagingItemListFields.FilmPrintStyle] = packagingItem.FilmPrintStyle;
                            item[PackagingItemListFields.FilmStyle] = packagingItem.FilmStyle;
                            item[PackagingItemListFields.CorrugatedPrintStyle] = packagingItem.CorrugatedPrintStyle;
                            item[PackagingItemListFields.GraphicsChangeRequired] = packagingItem.GraphicsChangeRequired;
                            item[PackagingItemListFields.ExternalGraphicsVendor] = packagingItem.ExternalGraphicsVendor;
                            item[PackagingItemListFields.GraphicsBrief] = packagingItem.GraphicsBrief;
                            if ((packagingItem.BOMEffectiveDate != null) && (packagingItem.BOMEffectiveDate != DateTime.MinValue))
                                item[PackagingItemListFields.BOMEffectiveDate] = packagingItem.BOMEffectiveDate;
                            item[PackagingItemListFields.PlatesShipped] = packagingItem.PlatesShipped;
                            item[PackagingItemListFields.PackUnit] = packagingItem.PackUnit;
                            item[PackagingItemListFields.MRPC] = packagingItem.MRPC;
                            item[PackagingItemListFields.ParentID] = packagingItem.ParentID;
                            item[PackagingItemListFields.TransferSEMIMakePackLocations] = packagingItem.TransferSEMIMakePackLocations;

                            item[PackagingItemListFields.MakeLocation] = packagingItem.MakeLocation;
                            item[PackagingItemListFields.PackLocation] = packagingItem.PackLocation;
                            item[PackagingItemListFields.CountryOfOrigin] = packagingItem.CountryOfOrigin;
                            item[PackagingItemListFields.NewFormula] = packagingItem.NewFormula;
                            item[PackagingItemListFields.TrialsCompleted] = packagingItem.TrialsCompleted;
                            item[PackagingItemListFields.ShelfLife] = packagingItem.ShelfLife;
                            item[PackagingItemListFields.SAPMaterialGroup] = packagingItem.SAPMaterialGroup;
                            item[PackagingItemListFields.CompCostSubmittedDate] = packagingItem.CompCostSubmittedDate;
                            item[PackagingItemListFields.Flowthrough] = packagingItem.Flowthrough;
                            item[PackagingItemListFields.ComponentContainsNLEA] = packagingItem.ComponentContainsNLEA;
                            item[PackagingItemListFields.ReviewPrinterSupplier] = packagingItem.ReviewPrinterSupplier;
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];

                            //item[PackagingItemListFields.Deleted] = packagingItem.Deleted;
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }

                            item.Update();

                            // Update our Packaging Components
                            UpdatePackagingComponents(spWeb, Convert.ToInt32(packagingItem.CompassListItemId));
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateOPSPackagingItem(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            //item["Title"] = packagingItem.PackagingComponent;
                            item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                            item[PackagingItemListFields.NewExisting] = packagingItem.NewExisting;
                            item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                            item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                            item[PackagingItemListFields.TransferSEMIMakePackLocations] = packagingItem.TransferSEMIMakePackLocations;
                            item[PackagingItemListFields.PackLocation] = packagingItem.PackLocation;
                            item[PackagingItemListFields.CountryOfOrigin] = packagingItem.CountryOfOrigin;
                            item[PackagingItemListFields.Notes] = packagingItem.Notes;
                            item[PackagingItemListFields.ImmediateSPKChange] = packagingItem.ImmediateSPKChange;

                            item[PackagingItemListFields.CurrentLikeItem] = packagingItem.CurrentLikeItem;
                            item[PackagingItemListFields.CurrentLikeItemDescription] = packagingItem.CurrentLikeItemDescription;

                            //item[PackagingItemListFields.Deleted] = packagingItem.Deleted;
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();

                            // Update our Packaging Components
                            UpdatePackagingComponents(spWeb, Convert.ToInt32(packagingItem.CompassListItemId));
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdatePackagingItemParentID(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.ParentID] = packagingItem.ParentID;

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                item["Modified By"] = user.ID;
                            }
                            //item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();

                            // Update our Packaging Components
                            UpdatePackagingComponents(spWeb, Convert.ToInt32(packagingItem.CompassListItemId));
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpdateIPFPackagingItem(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                            item[PackagingItemListFields.NewExisting] = packagingItem.NewExisting;
                            item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                            item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                            item[PackagingItemListFields.CurrentLikeItem] = packagingItem.CurrentLikeItem;
                            item[PackagingItemListFields.CurrentLikeItemDescription] = packagingItem.CurrentLikeItemDescription;
                            item[PackagingItemListFields.CurrentLikeItemReason] = packagingItem.CurrentLikeItemReason;
                            item[PackagingItemListFields.CurrentOldItem] = packagingItem.CurrentOldItem;
                            item[PackagingItemListFields.CurrentOldItemDescription] = packagingItem.CurrentOldItemDescription;
                            item[PackagingItemListFields.PackQuantity] = packagingItem.PackQuantity;
                            item[PackagingItemListFields.PackUnit] = packagingItem.PackUnit;
                            item[PackagingItemListFields.GraphicsBrief] = packagingItem.GraphicsBrief;
                            item[PackagingItemListFields.GraphicsChangeRequired] = packagingItem.GraphicsChangeRequired;
                            item[PackagingItemListFields.ExternalGraphicsVendor] = packagingItem.ExternalGraphicsVendor;
                            item[PackagingItemListFields.ComponentContainsNLEA] = packagingItem.ComponentContainsNLEA;
                            item[PackagingItemListFields.Flowthrough] = packagingItem.Flowthrough;
                            item[PackagingItemListFields.Notes] = packagingItem.Notes;
                            item[PackagingItemListFields.ParentID] = packagingItem.ParentID;
                            item[PackagingItemListFields.PHL1] = packagingItem.PHL1;
                            item[PackagingItemListFields.PHL2] = packagingItem.PHL2;
                            item[PackagingItemListFields.Brand] = packagingItem.Brand;
                            item[PackagingItemListFields.ProfitCenter] = packagingItem.ProfitCenter;
                            //item[PackagingItemListFields.Deleted] = packagingItem.Deleted;
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();

                            // Update our Packaging Components
                            UpdatePackagingComponents(spWeb, Convert.ToInt32(packagingItem.CompassListItemId));
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdatePackagingGraphicsBrief(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.GraphicsBrief] = packagingItem.GraphicsBrief;

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public string GetTransferSemiMakePackLocations(int id)
        {
            string TransferSemiMakePackLocations = string.Empty;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPListItem item = spList.GetItemById(id);
                    if (item != null)
                    {
                        TransferSemiMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                    }
                }
            }
            return TransferSemiMakePackLocations;

        }
        public void UpdateTransferSemiMakePackLocations(int id, string TransferSEMIMakePackLocations)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(id);
                        if (item != null)
                        {

                            item[PackagingItemListFields.TransferSEMIMakePackLocations] = TransferSEMIMakePackLocations;
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateGraphicsDevelopmentPackagingItem(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.GraphicsChangeRequired] = packagingItem.GraphicsChangeRequired;
                            item[PackagingItemListFields.ExternalGraphicsVendor] = packagingItem.ExternalGraphicsVendor;
                            item[PackagingItemListFields.GraphicsBrief] = packagingItem.GraphicsBrief;
                            if ((packagingItem.BOMEffectiveDate != null) && (packagingItem.BOMEffectiveDate != DateTime.MinValue))
                                item[PackagingItemListFields.BOMEffectiveDate] = packagingItem.BOMEffectiveDate;

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateGraphicsPackagingItem(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.FinalGraphicsDescription] = packagingItem.FinalGraphicsDescription;
                            item[PackagingItemListFields.ConfirmedNLEA] = packagingItem.ConfirmedNLEA;
                            item[PackagingItemListFields.KosherLabelRequired] = packagingItem.KosherLabelRequired;
                            item[PackagingItemListFields.EstimatedNumberOfColors] = packagingItem.EstimatedNumberOfColors;
                            item[PackagingItemListFields.BlockForDateCode] = packagingItem.BlockForDateCode;
                            item[PackagingItemListFields.ConfirmedDielineRestrictions] = packagingItem.ConfirmedDielineRestrictions;
                            item[PackagingItemListFields.RenderingProvided] = packagingItem.RenderingProvided;
                            item[PackagingItemListFields.PlatesShipped] = packagingItem.PlatesShipped;
                            item[PackagingItemListFields.PackUnit] = packagingItem.PackUnit;

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateGraphicsProgressReportItem(int compassId, string routingDate, string routingReleasedDate, string PDFApprovedDate, string platesShippedDate, string notes)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(compassId);
                        if (item != null)
                        {
                            if (!string.IsNullOrEmpty(routingDate))
                                item[PackagingItemListFields.Graphics_Routing_ModifiedDate] = routingDate;
                            if (!string.IsNullOrEmpty(routingReleasedDate))
                                item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate] = routingReleasedDate;
                            if (!string.IsNullOrEmpty(PDFApprovedDate))
                                item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate] = PDFApprovedDate;
                            if (!string.IsNullOrEmpty(platesShippedDate))
                                item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate] = platesShippedDate;
                            item[PackagingItemListFields.Graphics_Notes] = notes;

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateGraphicsProgressReportItem(string form, int materialNumber, string routingDate, string routingReleasedDate, string PDFApprovedDate, string platesShippedDate)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        bool bUpdated = false;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"MaterialNumber\" /><Value Type=\"Text\">" + materialNumber.ToString() + "</Value></Eq></Where>";
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                        //SPListItem item = spList.GetItemById(materialNumber);
                        if (compassItemCol != null && compassItemCol.Count > 0)
                        {
                            SPListItem item = compassItemCol[0];
                            if (item != null)
                            {
                                // Routing Date
                                if (!string.IsNullOrEmpty(routingDate))
                                {
                                    if (item[PackagingItemListFields.Graphics_Routing_ModifiedDate] == null)
                                    {
                                        item[PackagingItemListFields.Graphics_Routing_ModifiedDate] = routingDate;
                                        bUpdated = true;
                                    }
                                    else if (string.IsNullOrEmpty(item[PackagingItemListFields.Graphics_Routing_ModifiedDate].ToString()))
                                    {
                                        item[PackagingItemListFields.Graphics_Routing_ModifiedDate] = routingDate;
                                        bUpdated = true;
                                    }
                                    else if (!string.Equals(item[PackagingItemListFields.Graphics_Routing_ModifiedDate].ToString(), routingDate))
                                    {
                                        item[PackagingItemListFields.Graphics_Routing_ModifiedDate] = routingDate;
                                        bUpdated = true;
                                    }
                                }

                                // Routing Released Date
                                if (!string.IsNullOrEmpty(routingReleasedDate))
                                {
                                    if (item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate] == null)
                                    {
                                        item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate] = routingReleasedDate;
                                        bUpdated = true;
                                    }
                                    else if (string.IsNullOrEmpty(item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate].ToString()))
                                    {
                                        item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate] = routingReleasedDate;
                                        bUpdated = true;
                                    }
                                    else if (!string.Equals(item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate].ToString(), routingReleasedDate))
                                    {
                                        item[PackagingItemListFields.Graphics_RoutingReleased_ModifiedDate] = routingReleasedDate;
                                        bUpdated = true;
                                    }
                                }

                                // PDF Approved
                                if (!string.IsNullOrEmpty(PDFApprovedDate))
                                {
                                    if (item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate] == null)
                                    {
                                        item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate] = PDFApprovedDate;
                                        bUpdated = true;
                                    }
                                    else if (string.IsNullOrEmpty(item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate].ToString()))
                                    {
                                        item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate] = PDFApprovedDate;
                                        bUpdated = true;
                                    }
                                    else if (!string.Equals(item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate].ToString(), PDFApprovedDate))
                                    {
                                        item[PackagingItemListFields.Graphics_PDFApproved_ModifiedDate] = PDFApprovedDate;
                                        bUpdated = true;
                                    }
                                }

                                // Plates Shipped
                                if (!string.IsNullOrEmpty(platesShippedDate))
                                {
                                    if (item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate] == null)
                                    {
                                        item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate] = platesShippedDate;
                                        bUpdated = true;
                                    }
                                    else if (string.IsNullOrEmpty(item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate].ToString()))
                                    {
                                        item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate] = platesShippedDate;
                                        bUpdated = true;
                                    }
                                    else if (!string.Equals(item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate].ToString(), platesShippedDate))
                                    {
                                        item[PackagingItemListFields.Graphics_PlatesShipped_ModifiedDate] = platesShippedDate;
                                        bUpdated = true;
                                    }
                                }

                                SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                                if (user != null)
                                {
                                    // Set Modified By to current user NOT System Account
                                    item["Modified By"] = user.ID;
                                }
                                item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                                if (bUpdated)
                                    item.Update();
                            }
                        }
                        else
                            exceptionService.HandleGraphicsImport(GraphicsLogCategory.MissingMaterialNumber, "Cannot find Material #: " + materialNumber.ToString(), form, materialNumber.ToString());

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdatePackagingItemPlatesShipped(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.PlatesShipped] = packagingItem.PlatesShipped;

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdatePackagingItemMaterialDescription(PackagingItem packagingItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public bool DeletePackagingItem(int packagingItemId)
        {
            bool isDeleted = false;
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(packagingItemId);
                        if (item != null)
                        {
                            item[PackagingItemListFields.Deleted] = "Yes";

                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item.Update();
                            isDeleted = true;
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isDeleted;
        }
        public int CheckForExistingPackagingItem(int stageListItemId, string bulkSemiNumber, string materialDescription)
        {
            int doesExist = 0;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                    SPQuery spQuery = new SPQuery();
                    if (!string.IsNullOrEmpty(bulkSemiNumber))
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"MaterialNumber\" /><Value Type=\"Text\">" + bulkSemiNumber.ToString() + "</Value></Eq></And></Where>";
                    else
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq><Eq><FieldRef Name=\"MaterialDescription\" /><Value Type=\"Text\">" + materialDescription.ToString() + "</Value></Eq></And></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);

                    if (compassItemCol.Count > 0)
                    {
                        SPListItem item = compassItemCol[0];
                        doesExist = item.ID;
                    }
                }
            }
            return doesExist;
        }
        public bool CheckIfPackagingItemsExistForProject(int stageListItemId)
        {
            bool doesExist = false;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    //var items = spList.Items.OfType<SPListItem>().Where(x => Convert.ToInt32(x[PackagingItemListFields.CompassListItemId]) == stageListItemId).ToList();

                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + stageListItemId + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        doesExist = true;
                    }
                }
            }
            return doesExist;
        }
        public List<FileAttribute> GetUploadedFiles(string projectNo, int packagingItemId, string docType)
        {
            return GetUploadedFiles(projectNo, packagingItemId, docType, SPContext.Current.Web.Url);
        }
        public List<FileAttribute> GetUploadedFiles(string projectNo, int packagingItemId, string docType, string webUrl)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(webUrl))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToInt32(x.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]).Equals(packagingItemId) && x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(docType)).ToList();
                        if (spfiles.Count > 0)
                        {
                            foreach (SPFile spfile in spfiles)
                            {
                                FileAttribute file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType]);
                                file.DisplayFileName = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_DisplayFileName]);

                                files.Add(file);
                            }
                        }
                    }
                }
            }
            return files;
        }
        public List<FileAttribute> GetRenderingUploadedFiles(string projectNo)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToString(x.Item[CompassListFields.DOCLIBRARY_CompassDocType]).Equals(GlobalConstants.DOCTYPE_Rendering));
                        foreach (SPFile spfile in spfiles)
                        {
                            try
                            {
                                FileAttribute file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType]);
                                file.DisplayFileName = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_DisplayFileName]);
                                file.PackagingComponentItemId = Convert.ToInt32(spfile.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]);

                                files.Add(file);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
            }
            return files;
        }

        public List<FileAttribute> GetApprovedGraphicsAssetUploadedFiles(string projectNo)
        {
            List<FileAttribute> files = new List<FileAttribute>();
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToString(x.Item[CompassListFields.DOCLIBRARY_CompassDocType]).Equals(GlobalConstants.DOCTYPE_ApprovedGraphicsAsset));
                        foreach (SPFile spfile in spfiles)
                        {
                            try
                            {
                                FileAttribute file = new FileAttribute();
                                if (string.IsNullOrEmpty(spfile.Title))
                                {
                                    file.FileName = spfile.Name;
                                }
                                else
                                {
                                    file.FileName = spfile.Title;
                                }
                                string fileurl = spfile.Url;
                                fileurl = fileurl.Replace("’", "%27");
                                fileurl = fileurl.Replace("'", "%27");
                                file.FileUrl = string.Concat(spweb.Url, "/", fileurl);
                                file.DocType = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_CompassDocType]);
                                file.DisplayFileName = Convert.ToString(spfile.Item[CompassListFields.DOCLIBRARY_DisplayFileName]);
                                file.PackagingComponentItemId = Convert.ToInt32(spfile.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]);

                                files.Add(file);
                            }
                            catch (Exception e)
                            {

                            }
                        }
                    }
                }
            }
            return files;
        }
        public List<CompassPackMeasurementsItem> GetPackMeasurements(int itemId)
        {
            SPList spList;
            SPQuery spQuery;
            SPListItemCollection compassItemCol;
            CompassPackMeasurementsItem measure;
            List<CompassPackMeasurementsItem> measurements = new List<CompassPackMeasurementsItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    compassItemCol = spList.GetItems(spQuery);
                    foreach (SPItem item in compassItemCol)
                    {
                        measure = new CompassPackMeasurementsItem();
                        measure.ItemId = item.ID;
                        measure.CompassListItemId = Convert.ToInt32(item[CompassPackMeasurementsFields.CompassListItemId]);
                        measure.SalesCaseDimensionsHeight = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesCaseDimensionsHeight]);
                        measure.SalesCaseDimensionsWidth = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesCaseDimensionsWidth]);
                        measure.SalesCaseDimensionsLength = Convert.ToDouble(item[CompassPackMeasurementsFields.SalesCaseDimensionsLength]);
                        measure.CaseGrossWeight = Convert.ToDouble(item[CompassPackMeasurementsFields.CaseGrossWeight]);
                        measure.NetUnitWeight = Convert.ToDouble(item[CompassPackMeasurementsFields.NetUnitWeight]);
                        measurements.Add(measure);
                    }
                }
            }
            return measurements;
        }
        #region Private Methods
        private string GeneratePackagingComponents(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>Material #</b></td>");
                value.Append("<td><b>Material Description</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td></tr>");
            }
            return value.ToString();
        }
        private string GeneratePackagingComponentsSAPsetupBOM(bool bHeader, PackagingItem item, bool isTransferSemi, bool FourteenDigitBarCode = false)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>Material #</b></td>");
                value.Append("<td><b>New or Existing</b></td>");
                value.Append("<td><b>Material Description</b></td>");
                value.Append("<td><b>Flowthrough</b></td>");
                if (FourteenDigitBarCode)
                {
                    value.Append("<td><b>14 Digit Code</b></td>");
                    value.Append("<td><b>" + (isTransferSemi ? "Make" : "Pack") + " Location</b></td></tr>");
                }
                else
                {
                    value.Append("<td><b>" + (isTransferSemi ? "Make" : "Pack") + " Location</b></td></tr>");
                }
            }
            else
            {
                value.Append("<tr><td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.NewExisting + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + item.Flowthrough + "</td>");
                if (FourteenDigitBarCode)
                {
                    value.Append("<td>" + (string.IsNullOrWhiteSpace((isTransferSemi ? item.MakeLocation : item.PackLocation)) ? "   -   " : (isTransferSemi ? item.MakeLocation : item.PackLocation)) + "</td>");
                    value.Append("<td>" + item.FourteenDigitBarCode + "</td></tr>");
                }
                else
                {
                    value.Append("<td>" + (string.IsNullOrWhiteSpace((isTransferSemi ? item.MakeLocation : item.PackLocation)) ? "   -   " : (isTransferSemi ? item.MakeLocation : item.PackLocation)) + "</td></tr>");
                }
            }
            return value.ToString();
        }
        private string GeneratePackagingComponentsSAPsetupBOMTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder FGTSSummary = new StringBuilder();
            StringBuilder FGSummary = new StringBuilder();
            StringBuilder TSSummary = new StringBuilder();

            List<PackagingItem> dtPackingItemsFG = new List<PackagingItem>();
            dtPackingItemsFG =
                (
                    from PIs in AllPackagingItems
                    where PIs.ParentID == 0
                    select PIs
                ).ToList<PackagingItem>();

            var EligibleTransferSemis =
                  (
                      from packgingItem in AllPackagingItems
                      where packgingItem.PackagingComponent.ToLower().Contains("transfer") && (packgingItem.PackLocation.Contains("FQ22") || packgingItem.PackLocation.Contains("FQ25"))
                      select packgingItem
                  ).ToList();

            FGSummary.Append("<B>Finished Good Summary :</B>");
            FGSummary.Append(GeneratePackagingComponentsSAPsetupBOM(true, null, false));

            foreach (PackagingItem item in dtPackingItemsFG)
            {
                FGSummary.Append(GeneratePackagingComponentsSAPsetupBOM(false, item, false));

                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    #region Declare Variable
                    List<PackagingItem> dtSemiBOMItems = (from PIs in AllPackagingItems where PIs.ParentID == item.Id select PIs).ToList<PackagingItem>();
                    List<Tuple<int, string, string, string, string>> ChildSemiBomItems = new List<Tuple<int, string, string, string, string>>();
                    #endregion
                    #region Corrugates
                    var Corrugates =
                                    (
                                        from
                                            packgingItem in AllPackagingItems
                                        join
                                            EligibleTransferSemi in EligibleTransferSemis
                                            on packgingItem.ParentID equals EligibleTransferSemi.Id
                                        where
                                            packgingItem.PackagingComponent.ToLower().Contains("corrugated")
                                            &&
                                            packgingItem.ParentID == item.Id
                                            &&
                                            packgingItem.NewExisting.ToLower() == "new"
                                        select packgingItem
                                    ).ToList();
                    #endregion
                    #region Header 
                    TSSummary.Append("<B>" + (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi ? GlobalConstants.PACKAGINGTYPE_SEMIBOM_Display : GlobalConstants.PACKAGINGTYPE_PURCHASEDSEMIBOM_Display) + ": " + item.MaterialNumber + " : " + item.MaterialDescription + "</B>");
                    TSSummary.Append(GeneratePackagingComponentsSAPsetupBOM(true, null, true, (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && Corrugates.Count > 0) ? true : false));
                    #endregion

                    foreach (PackagingItem SemiBOMItem in dtSemiBOMItems)
                    {
                        bool FourteenDigitBarCode = false;

                        if (SemiBOMItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || SemiBOMItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                        {
                            ChildSemiBomItems.Add(new Tuple<int, string, string, string, string>(SemiBOMItem.Id, SemiBOMItem.MaterialNumber, SemiBOMItem.MaterialDescription, (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi ? GlobalConstants.PACKAGINGTYPE_SEMIBOM : GlobalConstants.PACKAGINGTYPE_PURCHASEDSEMIBOM), SemiBOMItem.PackagingComponent));
                        }

                        if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && Corrugates.Count > 0)
                        {
                            FourteenDigitBarCode = true;
                            SemiBOMItem.FourteenDigitBarCode = (SemiBOMItem.PackagingComponent.ToLower().Contains("corrugated") && SemiBOMItem.NewExisting.ToLower() == "new") ? SemiBOMItem.FourteenDigitBarCode : "NA";
                        }

                        TSSummary.Append(GeneratePackagingComponentsSAPsetupBOM(false, SemiBOMItem, true, FourteenDigitBarCode));
                    }
                    TSSummary.Append("</table><br><br>");

                    foreach (Tuple<int, string, string, string, string> SemiBOMItem in ChildSemiBomItems)
                    {
                        #region Corrugates
                        var ChildCorrugates =
                                        (
                                            from
                                                packgingItem in AllPackagingItems
                                            join
                                                EligibleTransferSemi in EligibleTransferSemis
                                                on packgingItem.ParentID equals EligibleTransferSemi.Id
                                            where
                                                packgingItem.PackagingComponent.ToLower().Contains("corrugated")
                                                &&
                                                packgingItem.ParentID == SemiBOMItem.Item1
                                                &&
                                                packgingItem.NewExisting.ToLower() == "new"
                                            select packgingItem
                                        ).ToList();
                        #endregion

                        List<PackagingItem> dtChildSemiBOMItems = (from PIs in AllPackagingItems where PIs.ParentID == SemiBOMItem.Item1 select PIs).ToList<PackagingItem>();

                        #region Header
                        TSSummary.Append("<B>" + (SemiBOMItem.Item5 == GlobalConstants.COMPONENTTYPE_TransferSemi ? GlobalConstants.PACKAGINGTYPE_SEMIBOM_Display : GlobalConstants.PACKAGINGTYPE_PURCHASEDSEMIBOM_Display) + ": " + SemiBOMItem.Item2 + " : " + SemiBOMItem.Item3 + "</B>");
                        TSSummary.Append(GeneratePackagingComponentsSAPsetupBOM(true, null, true, (SemiBOMItem.Item5 == GlobalConstants.COMPONENTTYPE_TransferSemi && ChildCorrugates.Count > 0) ? true : false));
                        #endregion

                        foreach (PackagingItem ChildSemiBOMItem in dtChildSemiBOMItems)
                        {
                            bool FourteenDigitBarCode = false;
                            if ((SemiBOMItem.Item5 == GlobalConstants.COMPONENTTYPE_TransferSemi && ChildCorrugates.Count > 0))
                            {
                                FourteenDigitBarCode = true;
                                ChildSemiBOMItem.FourteenDigitBarCode = (ChildSemiBOMItem.PackagingComponent.ToLower().Contains("corrugated") && ChildSemiBOMItem.NewExisting.ToLower() == "new") ? ChildSemiBOMItem.FourteenDigitBarCode : "NA";
                            }

                            TSSummary.Append(GeneratePackagingComponentsSAPsetupBOM(false, ChildSemiBOMItem, true, FourteenDigitBarCode));
                        }
                        TSSummary.Append("</table><br><br>");
                    }
                }
            }
            FGSummary.Append("</table><br><br>");
            FGTSSummary.Append(FGSummary);
            FGTSSummary.Append(TSSummary);
            return FGTSSummary.ToString();
        }
        private string GenerateNewTransferSemiWithPackLocationsTables(List<PackagingItem> packagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();

            var AllTransferSemis =
                   (
                       from packgingItem in packagingItems
                       where packgingItem.PackagingComponent.ToLower().Contains("transfer") && (packgingItem.NewExisting.Equals("New"))
                       select packgingItem
                   ).ToList();

            var EligibleTransferSemis =
                   (
                       from packgingItem in packagingItems
                       where packgingItem.PackagingComponent.ToLower().Contains("transfer") && (packgingItem.PackLocation.Contains("FQ22") || packgingItem.PackLocation.Contains("FQ25"))
                       select packgingItem
                   ).ToList();

            if (AllTransferSemis.Count > 0)
            {
                TSSummary.Append("<B>New Transfer Semis :</B>");
                TSSummary.Append(GenerateNewTransferSemiWithPackLocations(true, null, ""));
                foreach (PackagingItem item in AllTransferSemis)
                {
                    var Corrugates =
                        (
                            from
                                packgingItem in packagingItems
                            join
                                EligibleTransferSemi in EligibleTransferSemis
                                on packgingItem.ParentID equals EligibleTransferSemi.Id
                            where
                                packgingItem.PackagingComponent.ToLower().Contains("corrugated")
                                &&
                                packgingItem.ParentID == item.Id
                                &&
                                packgingItem.NewExisting.ToLower() == "new"
                            select packgingItem
                        ).ToList();
                    string FourteenDigitBarCodes = Corrugates.Count > 0 ? string.Join(",", Corrugates.Select(c => c.FourteenDigitBarCode)) : "NA";

                    TSSummary.Append(GenerateNewTransferSemiWithPackLocations(false, item, FourteenDigitBarCodes));
                }
                TSSummary.Append("</table><br><br>");
            }

            return TSSummary.ToString();
        }
        private string GenerateNewTransferSemiWithPackLocations(bool bHeader, PackagingItem item, string FourteenDigitBarCode)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>Transfer Semi #</b></td>");
                value.Append("<td><b>Description</b></td>");
                value.Append("<td><b>Pack Location</b></td>");
                value.Append("<td><b>14 Digit CCC Code</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + "<span style=\"color:blue;font-weight:bold\"><U><big>" + item.PackLocation + "</big></U></span>" + "</td>");
                value.Append("<td>" + FourteenDigitBarCode + "</td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNetworkMoveTransferSemiWithPackLocationsTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();
            List<PackagingItem> AllTransferSemis =
                (
                    from
                        PIs in AllPackagingItems
                    where
                        PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi
                        &&
                        PIs.NewExisting == "Network Move"
                    select
                        PIs
                ).ToList<PackagingItem>();

            var EligibleTransferSemis =
                  (
                      from packgingItem in AllPackagingItems
                      where packgingItem.PackagingComponent.ToLower().Contains("transfer") && (packgingItem.PackLocation.Contains("FQ22") || packgingItem.PackLocation.Contains("FQ25"))
                      select packgingItem
                  ).ToList();


            if (AllTransferSemis.Count > 0)
            {
                TSSummary.Append("<B>Network Transfer Semis :</B>");
                TSSummary.Append(GenerateNetworkMoveTransferSemiWithPackLocations(true, null, ""));
                foreach (PackagingItem item in AllTransferSemis)
                {
                    var Corrugates =
                       (
                           from
                               packgingItem in AllPackagingItems
                           join
                               EligibleTransferSemi in EligibleTransferSemis
                               on packgingItem.ParentID equals EligibleTransferSemi.Id
                           where
                               packgingItem.PackagingComponent.ToLower().Contains("corrugated")
                               &&
                               packgingItem.ParentID == item.Id
                               &&
                               packgingItem.NewExisting.ToLower() == "new"
                           select packgingItem
                       ).ToList();
                    string FourteenDigitBarCodes = Corrugates.Count > 0 ? string.Join(",", Corrugates.Select(c => c.FourteenDigitBarCode)) : "NA";

                    TSSummary.Append(GenerateNetworkMoveTransferSemiWithPackLocations(false, item, FourteenDigitBarCodes));
                }
                TSSummary.Append("</table><br><br>");
            }

            return TSSummary.ToString();
        }
        private string GenerateNetworkMoveTransferSemiWithPackLocations(bool bHeader, PackagingItem item, string FourteenDigitBarCode)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>Transfer Semi #</b></td>");
                value.Append("<td><b>Description</b></td>");
                value.Append("<td><b>Pack Location</b></td>");
                value.Append("<td><b>14 Digit CCC Code</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + "<span style=\"color:blue;font-weight:bold\"><U><big>" + item.PackLocation + "</big></U></span>" + "</td>");
                value.Append("<td>" + FourteenDigitBarCode + "</td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNMTSsForSAPIntItemSetupTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();
            List<PackagingItem> NMTSs =
                (
                    from
                        PIs in AllPackagingItems
                    where
                        PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi
                        &&
                        PIs.NewExisting == "Network Move"
                    select
                        PIs
                ).ToList<PackagingItem>();

            if (NMTSs.Count > 0)
            {
                TSSummary.Append("<B>Network Move Transfer Semis:</B>");

                TSSummary.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                TSSummary.Append("<tr><td><b>Transfer Semi #</b></td>");
                TSSummary.Append("<td><b>Description</b></td>");
                TSSummary.Append("<td><b>Pack Location</b></td></tr>");

                foreach (PackagingItem NMTS in NMTSs)
                {
                    TSSummary.Append("<tr><td>" + NMTS.MaterialNumber + "</td>");
                    TSSummary.Append("<td>" + NMTS.MaterialDescription + "</td>");
                    TSSummary.Append("<td>" + "<span style=\"color:blue;font-weight:bold\"><U><big>" + NMTS.PackLocation + "</big></U></span>" + "</td></tr>");
                }
                TSSummary.Append("</table><br><br>");
            }

            return TSSummary.ToString();
        }
        private string GenerateNewPURCANDYSemisTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();

            List<PackagingItem> dtPackingItemsTS = (from PIs in AllPackagingItems where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi && PIs.NewExisting == "New" select PIs).ToList<PackagingItem>();

            if (dtPackingItemsTS.Count > 0)
            {

                TSSummary.Append("<B>New PUR CANDY Semis :</B>");
                TSSummary.Append(GenerateNewPURCANDYSemis(true, null));
                foreach (PackagingItem item in dtPackingItemsTS)
                {
                    TSSummary.Append(GenerateNewPURCANDYSemis(false, item));
                }
                TSSummary.Append("</table><br><br>");
            }

            return TSSummary.ToString();
        }
        private string GenerateNewPURCANDYSemis(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>Purchased Semi #</b></td>");
                value.Append("<td><b>Description</b></td>");
                value.Append("<td><b>Purchased Into Location</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + item.PurchasedIntoLocation + "</td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNewPURCANDYSemisPackLocationTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();

            List<PackagingItem> dtPackingItemsTS = (from PIs in AllPackagingItems where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi && PIs.NewExisting == "New" select PIs).ToList<PackagingItem>();

            if (dtPackingItemsTS.Count > 0)
            {
                TSSummary.Append("<B>New Purchased Semis :</B>");
                TSSummary.Append(GenerateNewPURCANDYSemisPackLocation(true, null));
                foreach (PackagingItem item in dtPackingItemsTS)
                {
                    TSSummary.Append(GenerateNewPURCANDYSemisPackLocation(false, item));
                }
                TSSummary.Append("</table><br><br>");
            }

            return TSSummary.ToString();
        }
        private string GenerateNewPURCANDYSemisPackLocation(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>PUR CANDY Semi #</b></td>");
                value.Append("<td><b>Description</b></td>");
                value.Append("<td><b>Pack Location</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td><span style=\"color:blue;font-weight:bold\"><U><big>" + item.PackLocation + "</big></U></span></td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNMPURCANDYSemisPackLocationTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();

            List<PackagingItem> dtPackingItemsTS = (from PIs in AllPackagingItems where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi && PIs.NewExisting == "Network Move" select PIs).ToList<PackagingItem>();

            if (dtPackingItemsTS.Count > 0)
            {
                TSSummary.Append("<B>Network Move Purchased Semis :</B>");
                TSSummary.Append(GenerateNMPURCANDYSemisPackLocation(true, null));
                foreach (PackagingItem item in dtPackingItemsTS)
                {
                    TSSummary.Append(GenerateNMPURCANDYSemisPackLocation(false, item));
                }
                TSSummary.Append("</table><br><br>");
            }

            return TSSummary.ToString();
        }
        private string GenerateNMPURCANDYSemisPackLocation(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>PUR CANDY Semi #</b></td>");
                value.Append("<td><b>Description</b></td>");
                value.Append("<td><b>Pack Location</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td><span style=\"color:blue;font-weight:bold\"><U><big>" + item.PackLocation + "</big></U></span></td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNewPackagingComponents(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>Component Type</b></td>");
                value.Append("<td><b>Material #</b></td>");
                value.Append("<td><b>Material Description</b></td>");
                value.Append("<td><b>Receiving Plant</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.PackagingComponent + "</td>");
                value.Append("<td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + item.ReceivingPlant + "</td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNewWarehouseDetails(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr><td><b>Component Type</b></td>");
                value.Append("<td><b>Material #</b></td>");
                value.Append("<td><b>Material Description</b></td>");
                value.Append("<td><b>Ferrara Plant</b></td>");
                value.Append("<td><b>Costing Unit</b></td>");
                value.Append("<td><b>Eaches per Costing Unit</b></td>");
                value.Append("<td><b>LBs per Costing Unit</b></td>");
                value.Append("<td><b>Costing Unit per Pallet</b></td></tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.PackagingComponent + "</td>");
                value.Append("<td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + item.ReceivingPlant + "</td>");
                value.Append("<td>" + item.CostingUnit + "</td>");
                value.Append("<td>" + item.EachesPerCostingUnit + "</td>");
                value.Append("<td>" + item.LBPerCostingUnit + "</td>");
                value.Append("<td>" + item.CostingUnitPerPallet + "</td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNewStdCostEntryDetails(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();

            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr>");
                value.Append("<td><b>Component Type</b></td>");
                value.Append("<td><b>Material #</b></td>");
                value.Append("<td><b>Material Description</b></td>");
                value.Append("<td><b>Ferrara Plant</b></td>");
                value.Append("<td><b>Quoted Quantities</b></td>");
                value.Append("<td><b>Standard Cost</b></td>");
                value.Append("</tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.PackagingComponent + "</td>");
                value.Append("<td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + item.ReceivingPlant + "</td>");
                value.Append("<td>" + item.QuantityQuote + "</td>");
                value.Append("<td>" + item.StandardCost + "</td></tr>");
            }
            return value.ToString();
        }
        private string GenerateNewSAPCostDetails(bool bHeader, PackagingItem item)
        {
            StringBuilder value = new StringBuilder();

            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr>");
                value.Append("<td><b>Component Type</b></td>");
                value.Append("<td><b>Material #</b></td>");
                value.Append("<td><b>Material Description</b></td>");
                value.Append("<td><b>Vendor #</b></td>");
                value.Append("<td><b>Vendor Name</b></td>");
                value.Append("<td><b>Receiving Plant</b></td>");
                value.Append("<td><b>Standard Ordering Quantity</b></td>");
                value.Append("<td><b>Order UOM</b></td>");
                value.Append("<td><b>Incoterms</b></td>");
                value.Append("<td><b>PR Date Category</b></td>");
                value.Append("<td><b>Vendor Material #</b></td>");
                value.Append("<td><b>Costing Conditions/All Costing Information</b></td>");
                value.Append("</tr>");
            }
            else
            {
                value.Append("<tr><td>" + item.PackagingComponent + "</td>");
                value.Append("<td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + item.VendorNumber + "</td>");
                value.Append("<td>" + item.PrinterSupplier + "</td>");
                value.Append("<td>" + item.ReceivingPlant + "</td>");
                value.Append("<td>" + item.StandardOrderingQuantity + "</td>");
                value.Append("<td>" + item.OrderUOM + "</td>");
                value.Append("<td>" + item.Incoterms + "</td>");
                value.Append("<td>" + item.PRDateCategory + "</td>");
                value.Append("<td>" + item.VendorMaterialNumber + "</td>");
                value.Append("<td>" + item.CostingCondition + "</td></tr>");
            }
            return value.ToString();
        }

        private void UpdatePackagingComponents(SPWeb spWeb, int compassListItemId)
        {
            SPListItemCollection compassItemCol;
            SPListItem decisionItem;
            SPList spList;
            SPQuery spQuery;
            List<PackagingItem> packagingItems = GetAllPackagingItemsForProject(compassListItemId);
            StringBuilder packagingComponentsTable = new StringBuilder();
            StringBuilder packagingComponentsSAPsetupBOMTable = new StringBuilder();
            StringBuilder newPackagingComponentsTable = new StringBuilder();
            StringBuilder newWarehouseDetailsTable = new StringBuilder();
            StringBuilder newStdCostEntryDetailsTable = new StringBuilder();
            StringBuilder newSAPCostDetailsTable = new StringBuilder();
            StringBuilder packagingComponents = new StringBuilder();
            StringBuilder packagingComponentsNewTransferSemisTable = new StringBuilder();
            StringBuilder packagingComponentsNetworkTransferSemisTable = new StringBuilder();
            StringBuilder NMTSsForSAPIntItemSetupTable = new StringBuilder();
            StringBuilder packagingComponentsNewPURCANDYSemisTable = new StringBuilder();
            StringBuilder packagingComponentsNewPURCANDYSemisPackLocationTable = new StringBuilder();
            StringBuilder packagingComponentsNMPURCANDYSemisPackLocationTable = new StringBuilder();

            bool bNewComponents = false;
            string NewMaterialsinBOM = GlobalConstants.CONST_No;
            string NewPURCANDYSemiInBOM = GlobalConstants.CONST_No;
            string NewTransferSemiInBOM = GlobalConstants.CONST_No;
            string NetworkTransferSemiInBOM = GlobalConstants.CONST_No;

            packagingComponentsTable.Append(GeneratePackagingComponents(true, null));
            packagingComponentsSAPsetupBOMTable.Append(GeneratePackagingComponentsSAPsetupBOMTables(packagingItems));
            newPackagingComponentsTable.Append(GenerateNewPackagingComponents(true, null));
            newWarehouseDetailsTable.Append(GenerateNewWarehouseDetails(true, null));
            newStdCostEntryDetailsTable.Append(GenerateNewStdCostEntryDetails(true, null));
            newSAPCostDetailsTable.Append(GenerateNewSAPCostDetails(true, null));

            foreach (PackagingItem item in packagingItems)
            {
                packagingComponentsTable.Append(GeneratePackagingComponents(false, item));
                if (item.NewExisting.ToLower() == "new")
                {
                    NewMaterialsinBOM = GlobalConstants.CONST_Yes;
                    if ((!string.IsNullOrEmpty(item.MaterialNumber)))
                    {
                        bNewComponents = true;
                        newPackagingComponentsTable.Append(GenerateNewPackagingComponents(false, item));
                        newWarehouseDetailsTable.Append(GenerateNewWarehouseDetails(false, item));
                        newStdCostEntryDetailsTable.Append(GenerateNewStdCostEntryDetails(false, item));
                        newSAPCostDetailsTable.Append(GenerateNewSAPCostDetails(false, item));
                    }

                    if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                    {
                        NewPURCANDYSemiInBOM = GlobalConstants.CONST_Yes;
                    }

                    if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        NewTransferSemiInBOM = GlobalConstants.CONST_Yes;
                    }
                }
                else if (item.NewExisting.ToLower() == "network move")
                {
                    if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        NetworkTransferSemiInBOM = GlobalConstants.CONST_Yes;
                    }
                }
            }
            packagingComponentsTable.Append("</table>");
            newPackagingComponentsTable.Append("</table>");
            newWarehouseDetailsTable.Append("</table>");
            newStdCostEntryDetailsTable.Append("</table>");
            newSAPCostDetailsTable.Append("</table>");

            packagingComponentsNewTransferSemisTable.Append(GenerateNewTransferSemiWithPackLocationsTables(packagingItems));
            packagingComponentsNetworkTransferSemisTable.Append(GenerateNetworkMoveTransferSemiWithPackLocationsTables(packagingItems));
            NMTSsForSAPIntItemSetupTable.Append(GenerateNMTSsForSAPIntItemSetupTables(packagingItems));
            packagingComponentsNewPURCANDYSemisTable.Append(GenerateNewPURCANDYSemisTables(packagingItems));
            packagingComponentsNewPURCANDYSemisPackLocationTable.Append(GenerateNewPURCANDYSemisPackLocationTables(packagingItems));
            packagingComponentsNMPURCANDYSemisPackLocationTable.Append(GenerateNMPURCANDYSemisPackLocationTables(packagingItems));

            spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ProjectDecisionsListName);
            spQuery = new SPQuery();
            spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";
            spQuery.RowLimit = 1;

            compassItemCol = spList.GetItems(spQuery);
            if (compassItemCol.Count > 0)
            {
                decisionItem = compassItemCol[0];
                decisionItem[CompassProjectDecisionsListFields.PackagingCompSAPsetupBOM] = packagingComponentsSAPsetupBOMTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NewTSPackLocations] = packagingComponentsNewTransferSemisTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NewPurCandySemis] = packagingComponentsNewPURCANDYSemisTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NewPurCandySemisPackLocation] = packagingComponentsNewPURCANDYSemisPackLocationTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NMPurCandySemisPackLocation] = packagingComponentsNewPURCANDYSemisPackLocationTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NetworkMoveTSPackLocations] = packagingComponentsNMPURCANDYSemisPackLocationTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NMTSsForSAPIntItemSetup] = NMTSsForSAPIntItemSetupTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NewTransferSemi] = NewTransferSemiInBOM;
                decisionItem[CompassProjectDecisionsListFields.NetworkMoveTransferSemi] = NetworkTransferSemiInBOM;
                decisionItem[CompassProjectDecisionsListFields.NewPurCandySemi] = NewPURCANDYSemiInBOM;
                decisionItem[CompassProjectDecisionsListFields.NewMaterialsinBOM] = NewMaterialsinBOM;
                decisionItem[CompassProjectDecisionsListFields.PackagingCompInitialSetup] = packagingComponentsTable.ToString();

                if (bNewComponents && decisionItem != null)
                {
                    decisionItem[CompassProjectDecisionsListFields.NewPackagingComponents] = newPackagingComponentsTable.ToString();
                    decisionItem[CompassProjectDecisionsListFields.NewStdCostEntryDetails] = newStdCostEntryDetailsTable.ToString();
                    decisionItem[CompassProjectDecisionsListFields.NewWarehouseDetails] = newWarehouseDetailsTable.ToString();
                    decisionItem[CompassProjectDecisionsListFields.NewSAPCostDetails] = newSAPCostDetailsTable.ToString();
                }
                decisionItem.Update();
            }

            //UpdatePackMeasurementsListNewTransferSemiPackLocations(spWeb, compassListItemId);
        }

        //private void UpdatePackMeasurementsListNewTransferSemiPackLocations(SPWeb spWeb, int compassListItemId)
        //{
        //    SPListItemCollection compassItemCol;
        //    SPListItem packMeasurementsListItem;
        //    SPList spList;
        //    SPQuery spQuery;
        //    StringBuilder packagingComponentsNewTransferSemisTable = new StringBuilder();
        //    packagingComponentsNewTransferSemisTable.Append(GenerateNewTransferSemiWithPackLocationsTables(compassListItemId));


        //    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
        //    spQuery = new SPQuery();
        //    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItemId + "</Value></Eq></Where>";
        //    spQuery.RowLimit = 1;

        //    compassItemCol = spList.GetItems(spQuery);
        //    if (compassItemCol.Count > 0)
        //    {
        //        packMeasurementsListItem = compassItemCol[0];
        //        //packMeasurementsListItem[CompassPackMeasurementsFields.NewTransferSemiPackLocations] = packagingComponentsNewTransferSemisTable.ToString();

        //        // Set Modified By to current user NOT System Account
        //        packMeasurementsListItem["Editor"] = SPContext.Current.Web.CurrentUser;

        //        packMeasurementsListItem.Update();
        //    }
        //}
        public void updateCompletedItems(string ComponentIds, string pageName)
        {
            List<string> IdsToBeUpdated = ComponentIds.Split(',').Select(s => s).ToList();
            string updatedField = "";
            if (pageName == GlobalConstants.PAGE_BillofMaterialSetUpPE.ToLower())
            {
                updatedField = PackagingItemListFields.PECompleted;
            }
            else if (pageName == GlobalConstants.PAGE_BillofMaterialSetUpPE2.ToLower())
            {
                updatedField = PackagingItemListFields.PE2Completed;
            }
            else if (pageName == GlobalConstants.PAGE_BillofMaterialSetUpProc.ToLower())
            {
                updatedField = PackagingItemListFields.ProcCompleted;
            }
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><In><FieldRef Name=\"ID\" /><Values>";
                        foreach (string i in IdsToBeUpdated)
                        {
                            if (i != "")
                            {
                                spQuery.Query += "<Value Type=\"Number\">" + i + "</Value>";
                            }
                        }
                        spQuery.Query += "</Values></In></Where>";
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol != null)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                item[updatedField] = GlobalConstants.DETAILFORMSTATUS_Completed;
                                item.Update();
                            }
                        }
                    }
                }
            });
        }

        public List<PackagingItem> GetClassMissMatchedBOMList(int compassListItemId)
        {
            var ClassMissMatchedBOMList = new List<PackagingItem>();
            string SAPBOMListMaterialType;

            List<PackagingItem> packagingItems = GetAllPackagingItemsForProject(compassListItemId);

            foreach (PackagingItem item in packagingItems)
            {
                if (item.NewExisting == GlobalConstants.PACKAGINGNEWEXISTINGE_EXISTING)
                {
                    SAPBOMListMaterialType = (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi) ? GlobalConstants.SAPBOMLIST_TRANSFERSEMI :
                                             (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi) ? GlobalConstants.SAPBOMLIST_PURCHASEDSEMI :
                                             (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_CandySemi) ? GlobalConstants.SAPBOMLIST_CANDYSEMI : string.Empty;

                    if (!CheckItemPresentInSAPBOMList(item.MaterialNumber, SAPBOMListMaterialType))
                    {
                        ClassMissMatchedBOMList.Add(item);
                    }
                }
            }

            return ClassMissMatchedBOMList;
        }
        private bool CheckItemPresentInSAPBOMList(string MaterialNumber, string SAPBOMMaterialType)
        {
            bool itemFound = false;
            List<SAPBOMListItem> SAPBOMListItems = SAPBOMService.GetSAPBOMItemsIPF(MaterialNumber);

            if (SAPBOMListItems == null)
            {
                itemFound = false;
            }
            else
            {
                foreach (SAPBOMListItem SAPBOMItem in SAPBOMListItems)
                {
                    if (SAPBOMItem.MaterialType == SAPBOMMaterialType)
                    {
                        itemFound = true;
                    }
                }
            }

            return itemFound;
        }
        #endregion
        public string getParentComponentType(int packagingItemId)
        {
            string parentCT = "";
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    spWeb.AllowUnsafeUpdates = true;
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPListItem packagingItem = spList.GetItemById(packagingItemId);
                    if (packagingItem != null)
                    {
                        parentCT = Convert.ToString(packagingItem[PackagingItemListFields.PackagingComponent]);
                    }
                }
            }
            return parentCT;
        }
        public bool BOMAttachmentsExist(string projectNo, int packagingItemId)
        {
            bool count = false;
            using (SPSite spsite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spweb = spsite.OpenWeb())
                {
                    SPDocumentLibrary documentLib = spweb.Lists.TryGetList(GlobalConstants.DOCLIBRARY_CompassLibraryName) as SPDocumentLibrary;
                    string folderUrl = string.Concat(documentLib.RootFolder.ServerRelativeUrl, "/", projectNo);
                    if (spweb.GetFolder(folderUrl).Exists)
                    {
                        SPFolder stageGateProjectFolder = spweb.GetFolder(folderUrl);
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToInt32(x.Item[CompassListFields.DOCLIBRARY_PackagingComponentItemId]).Equals(packagingItemId) && (x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_CADDrawing) || x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(GlobalConstants.DOCTYPE_Rendering))).ToList();
                        if (spfiles.Count > 0)
                        {
                            count = true;
                        }
                    }
                }
            }
            return count;
        }
        public List<int> filterParents(List<int> IDs, int compassItemID, int movingId)
        {
            List<int> idsHolder = IDs;
            List<KeyValuePair<int, int>> AllItems = new List<KeyValuePair<int, int>>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><And><Eq><FieldRef Name=\"" + PackagingItemListFields.CompassListItemId + "\" /><Value Type=\"Int\">" + compassItemID + "</Value></Eq><Or><Eq><FieldRef Name=\"" + PackagingItemListFields.PackagingComponent + "\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_TransferSemi + "</Value></Eq><Eq><FieldRef Name=\"" + PackagingItemListFields.PackagingComponent + "\" /><Value Type=\"Text\">" + GlobalConstants.COMPONENTTYPE_PurchasedSemi + "</Value></Eq></Or></And></Where>";
                    SPListItemCollection compassItemCol2 = spList.GetItems(spQuery2);
                    if (compassItemCol2.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol2)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            AllItems.Add(new KeyValuePair<int, int>(item.ID, Convert.ToInt32(item[PackagingItemListFields.ParentID])));
                        }
                    }
                    foreach (KeyValuePair<int, int> KVP in AllItems)
                    {
                        bool includeItems = includeItem(KVP.Key, movingId, AllItems);
                        if (!includeItems)
                        {
                            int IDIndex = IDs.IndexOf(KVP.Key);
                            idsHolder.RemoveAt(IDIndex);
                        }
                    }
                }
            }

            return idsHolder;
        }
        private bool includeItem(int lookupID, int compareID, List<KeyValuePair<int, int>> compareList)
        {
            bool includeID = true;
            int parentID = (from KVP in compareList where KVP.Key == lookupID select KVP.Value).FirstOrDefault();
            while (parentID >= 0)
            {
                if (parentID == 0)
                {
                    break;
                }
                if (parentID == compareID)
                {
                    includeID = false;
                    break;
                }
                int newParentID = (from KVP in compareList where KVP.Key == parentID select KVP.Value).FirstOrDefault();
                parentID = newParentID;
            }

            return includeID;
        }
        public void UpdateIPFPackagingItems(List<PackagingItem> packagingItems, int compassId)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><In><FieldRef Name=\"ID\" /><Values>";
                        foreach (PackagingItem i in packagingItems)
                        {
                            if (i.Id != 0)
                            {
                                spQuery.Query += "<Value Type=\"Number\">" + i.Id + "</Value>";
                            }
                        }
                        spQuery.Query += "</Values></In></Where>";
                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        //SPListItem item = spList.GetItemById(packagingItem.Id);
                        if (compassItemCol.Count > 0)
                        {
                            foreach (SPListItem item in compassItemCol)
                            {
                                if (item != null)
                                {
                                    PackagingItem packagingItem = (from PI in packagingItems where PI.Id == item.ID select PI).FirstOrDefault();
                                    item[PackagingItemListFields.PackagingComponent] = packagingItem.PackagingComponent;
                                    item[PackagingItemListFields.NewExisting] = packagingItem.NewExisting;
                                    item[PackagingItemListFields.MaterialNumber] = packagingItem.MaterialNumber;
                                    item[PackagingItemListFields.MaterialDescription] = packagingItem.MaterialDescription;
                                    item[PackagingItemListFields.CurrentLikeItem] = packagingItem.CurrentLikeItem;
                                    item[PackagingItemListFields.CurrentLikeItemDescription] = packagingItem.CurrentLikeItemDescription;
                                    item[PackagingItemListFields.CurrentLikeItemReason] = packagingItem.CurrentLikeItemReason;
                                    item[PackagingItemListFields.CurrentOldItem] = packagingItem.CurrentOldItem;
                                    item[PackagingItemListFields.CurrentOldItemDescription] = packagingItem.CurrentOldItemDescription;
                                    item[PackagingItemListFields.PackQuantity] = packagingItem.PackQuantity;
                                    item[PackagingItemListFields.PackUnit] = packagingItem.PackUnit;
                                    item[PackagingItemListFields.GraphicsBrief] = packagingItem.GraphicsBrief;
                                    item[PackagingItemListFields.GraphicsChangeRequired] = packagingItem.GraphicsChangeRequired;
                                    item[PackagingItemListFields.ExternalGraphicsVendor] = packagingItem.ExternalGraphicsVendor;
                                    item[PackagingItemListFields.ComponentContainsNLEA] = packagingItem.ComponentContainsNLEA;
                                    item[PackagingItemListFields.Flowthrough] = packagingItem.Flowthrough;
                                    item[PackagingItemListFields.Notes] = packagingItem.Notes;
                                    item[PackagingItemListFields.ParentID] = packagingItem.ParentID;

                                    item[PackagingItemListFields.PHL1] = packagingItem.PHL1;
                                    item[PackagingItemListFields.PHL2] = packagingItem.PHL2;
                                    item[PackagingItemListFields.Brand] = packagingItem.Brand;
                                    item[PackagingItemListFields.ProfitCenter] = packagingItem.ProfitCenter;
                                    //item[PackagingItemListFields.Deleted] = packagingItem.Deleted;
                                    SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                                    if (user != null)
                                    {
                                        // Set Modified By to current user NOT System Account
                                        item["Modified By"] = user.ID;
                                    }
                                    item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                                    item.Update();


                                }
                            }
                        }
                        // Update our Packaging Components
                        UpdatePackagingComponents(spWeb, compassId);
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

    }
}