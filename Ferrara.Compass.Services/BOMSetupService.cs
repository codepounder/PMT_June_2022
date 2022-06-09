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
using System.Collections;
using System.IO;

namespace Ferrara.Compass.Services
{
    public class BOMSetupService : IBOMSetupService
    {
        private readonly IUtilityService utilityService;
        private readonly IExceptionService exceptionService;
        private readonly IPackagingItemService packagingItemService;

        public BOMSetupService(IUtilityService utilityService, IExceptionService exceptionService, IPackagingItemService packagingItemService)
        {
            this.utilityService = utilityService;
            this.exceptionService = exceptionService;
            this.packagingItemService = packagingItemService;
        }
        public string GetPLMFlag(int itemId)
        {
            string PLMFlag = "";
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        PLMFlag = Convert.ToString(item[CompassListFields.PLMProject]);
                    }
                }
            }
            return PLMFlag;
        }
        public BOMSetupProjectSummaryItem GetProjectSummaryDetails(int itemId)
        {
            BOMSetupProjectSummaryItem summary = new BOMSetupProjectSummaryItem();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    SPListItem item = spList.GetItemById(itemId);
                    if (item != null)
                    {
                        //Project Information
                        summary.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        summary.ProjectSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        summary.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        summary.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        summary.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        summary.WorkCenterAddInfo = Convert.ToString(item[CompassListFields.WorkCenterAdditionalInfo]);
                        summary.PegHoleNeeded = Convert.ToString(item[CompassListFields.PegHoleNeeded]);
                        summary.FGLikeItem = Convert.ToString(item[CompassListFields.LikeFGItemNumber]);
                        summary.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        //Project Team
                        summary.InitiatorName = Convert.ToString(item[CompassListFields.InitiatorName]);
                        summary.PMName = Convert.ToString(item[CompassListFields.PMName]);
                        summary.PackagingEngineerName = Convert.ToString(item[CompassListFields.PackagingEngineerLead]);
                        //Logistic Information
                        summary.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        summary.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        summary.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);
                        summary.ProcurementType = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);
                        summary.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        summary.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        summary.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);
                    }
                    #region Compass Team
                    SPList spList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassTeamListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    SPListItemCollection spcol = spList2.GetItems(spQuery);
                    if (spcol.Count > 0)
                    {
                        SPListItem item2 = spcol[0];
                        if (item2 != null)
                        {
                            summary.MarketingName = Convert.ToString(item2[CompassTeamListFields.MarketingName]);
                            summary.InTechManagerName = Convert.ToString(item2[CompassTeamListFields.InTechName]);
                            summary.PackagingEngineerName = Convert.ToString(item2[CompassTeamListFields.PackagingEngineeringName]);
                        }
                    }
                    #endregion

                    #region Compass List 2
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
                            summary.DesignateHUBDC = Convert.ToString(compassList2tem[CompassList2Fields.DesignateHUBDC]);
                            summary.AllAspectsApprovedFromPEPersp = Convert.ToString(compassList2tem[CompassList2Fields.AllAspectsApprovedFromPEPersp]);
                            summary.WhatIsIncorrectPE = Convert.ToString(compassList2tem[CompassList2Fields.WhatIsIncorrectPE]);
                        }
                    }
                    #endregion
                }
            }
            return summary;
        }
        public bool DeleteBOMSetupItem(int packagingItemId)
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
        public bool DeleteBOMSetupItems(List<int> packagingItemIds)
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

                        foreach (var packagingItemId in packagingItemIds)
                        {
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
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
            return isDeleted;
        }
        public int InsertBOMSetupItem(BOMSetupItem bomSetupItem)
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
                        item["Title"] = bomSetupItem.ProjectNumber;
                        item[PackagingItemListFields.CompassListItemId] = bomSetupItem.CompassListItemId;
                        item[PackagingItemListFields.MaterialNumber] = bomSetupItem.MaterialNumber;
                        item[PackagingItemListFields.MaterialDescription] = bomSetupItem.MaterialDescription;
                        item[PackagingItemListFields.PackagingComponent] = bomSetupItem.PackagingComponent;
                        item[PackagingItemListFields.NewExisting] = bomSetupItem.NewExisting;
                        item[PackagingItemListFields.CurrentLikeItem] = bomSetupItem.CurrentLikeItem;
                        item[PackagingItemListFields.CurrentLikeItemDescription] = bomSetupItem.CurrentLikeItemDescription;
                        item[PackagingItemListFields.CurrentLikeItemReason] = bomSetupItem.CurrentLikeItemReason;
                        item[PackagingItemListFields.CurrentOldItem] = bomSetupItem.CurrentOldItem;
                        item[PackagingItemListFields.CurrentOldItemDescription] = bomSetupItem.CurrentOldItemDescription;
                        item[PackagingItemListFields.PackQuantity] = bomSetupItem.PackQuantity;
                        item[PackagingItemListFields.SpecificationNo] = bomSetupItem.SpecificationNo;
                        item[PackagingItemListFields.PurchasedIntoLocation] = bomSetupItem.PurchasedIntoLocation;
                        item[PackagingItemListFields.LeadPlateTime] = bomSetupItem.LeadPlateTime;
                        item[PackagingItemListFields.LeadMaterialTime] = bomSetupItem.LeadMaterialTime;
                        item[PackagingItemListFields.PrinterSupplier] = bomSetupItem.PrinterSupplier;
                        item[PackagingItemListFields.Notes] = bomSetupItem.Notes;
                        item[PackagingItemListFields.GraphicsChangeRequired] = bomSetupItem.GraphicsChangeRequired;
                        item[PackagingItemListFields.ExternalGraphicsVendor] = bomSetupItem.ExternalGraphicsVendor;
                        item[PackagingItemListFields.GraphicsBrief] = bomSetupItem.GraphicsBrief;
                        item[PackagingItemListFields.PackUnit] = bomSetupItem.PackUnit;
                        item[PackagingItemListFields.ParentID] = bomSetupItem.ParentID;

                        item[PackagingItemListFields.MakeLocation] = bomSetupItem.MakeLocation;
                        item[PackagingItemListFields.PackLocation] = bomSetupItem.PackLocation;
                        item[PackagingItemListFields.CountryOfOrigin] = bomSetupItem.CountryOfOrigin;
                        item[PackagingItemListFields.NewFormula] = bomSetupItem.NewFormula;
                        item[PackagingItemListFields.TrialsCompleted] = bomSetupItem.TrialsCompleted;
                        item[PackagingItemListFields.ShelfLife] = bomSetupItem.ShelfLife;
                        item[PackagingItemListFields.Kosher] = bomSetupItem.Kosher;
                        item[PackagingItemListFields.Allergens] = bomSetupItem.Allergens;
                        item[PackagingItemListFields.SAPMaterialGroup] = bomSetupItem.SAPMaterialGroup;
                        item[PackagingItemListFields.TransferSEMIMakePackLocations] = bomSetupItem.TransferSEMIMakePackLocations;
                        item[PackagingItemListFields.Deleted] = bomSetupItem.Deleted;
                        item[PackagingItemListFields.ComponentContainsNLEA] = bomSetupItem.ComponentContainsNLEA;
                        item[PackagingItemListFields.Flowthrough] = bomSetupItem.Flowthrough;
                        //item[PackagingItemListFields.ImmediateSPKChange] = bomSetupItem.ImmediateSPKChange;
                        item[PackagingItemListFields.ReviewPrinterSupplier] = bomSetupItem.ReviewPrinterSupplier;
                        item[PackagingItemListFields.SAPDescAbbrev] = bomSetupItem.SAPDescAbbrev;
                        item[PackagingItemListFields.DielineLink] = bomSetupItem.DielineURL;
                        item[PackagingItemListFields.PHL1] = bomSetupItem.PHL1;
                        item[PackagingItemListFields.PHL2] = bomSetupItem.PHL2;
                        item[PackagingItemListFields.Brand] = bomSetupItem.Brand;
                        item[PackagingItemListFields.ProfitCenter] = bomSetupItem.ProfitCenter;
                        item[PackagingItemListFields.IsAllProcInfoCorrect] = bomSetupItem.IsAllProcInfoCorrect;
                        item[PackagingItemListFields.WhatProcInfoHasChanged] = bomSetupItem.WhatProcInfoHasChanged;
                        item[PackagingItemListFields.NewPrinterSupplierForLocation] = bomSetupItem.NewPrinterSupplierForLocation;

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
        public List<BOMSetupItem> GetAllBOMSetupItemsForProject(int compassListItemId)
        {
            List<BOMSetupItem> bomSetupItems = new List<BOMSetupItem>();
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

                            BOMSetupItem bomSetupItem = new BOMSetupItem();
                            bomSetupItem.Id = item.ID;
                            bomSetupItem.CompassListItemId = Convert.ToInt32(item[PackagingItemListFields.CompassListItemId]);
                            bomSetupItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            bomSetupItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            bomSetupItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            bomSetupItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                            bomSetupItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                            bomSetupItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                            bomSetupItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                            bomSetupItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                            bomSetupItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                            bomSetupItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                            bomSetupItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                            bomSetupItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                            bomSetupItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                            bomSetupItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
                            bomSetupItem.GraphicsChangeRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                            bomSetupItem.ExternalGraphicsVendor = Convert.ToString(item[PackagingItemListFields.ExternalGraphicsVendor]);
                            bomSetupItem.GraphicsBrief = Convert.ToString(item[PackagingItemListFields.GraphicsBrief]);
                            bomSetupItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);

                            bomSetupItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                            bomSetupItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                            bomSetupItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                            bomSetupItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                            bomSetupItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                            bomSetupItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                            bomSetupItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                            bomSetupItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                            bomSetupItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                            bomSetupItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                            bomSetupItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                            bomSetupItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            bomSetupItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                            bomSetupItem.SpecificationNo = Convert.ToString(item[PackagingItemListFields.SpecificationNo]);
                            bomSetupItem.PurchasedIntoLocation = Convert.ToString(item[PackagingItemListFields.PurchasedIntoLocation]);
                            bomSetupItem.FilmPrintStyle = Convert.ToString(item[PackagingItemListFields.FilmPrintStyle]);
                            bomSetupItem.CorrugatedPrintStyle = Convert.ToString(item[PackagingItemListFields.CorrugatedPrintStyle]);
                            bomSetupItem.DielineURL = Convert.ToString(item[PackagingItemListFields.DielineLink]);
                            bomSetupItem.ReviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]);
                            bomSetupItem.ThirteenDigitCode = Convert.ToString(item[PackagingItemListFields.ThirteenDigitCode]);
                            bomSetupItem.FourteenDigitBarcode = Convert.ToString(item[PackagingItemListFields.FourteenDigitBarcode]);
                            bomSetupItem.SAPDescAbbrev = Convert.ToString(item[PackagingItemListFields.SAPDescAbbrev]);

                            bomSetupItem.PHL1 = Convert.ToString(item[PackagingItemListFields.PHL1]);
                            bomSetupItem.PHL2 = Convert.ToString(item[PackagingItemListFields.PHL2]);
                            bomSetupItem.Brand = Convert.ToString(item[PackagingItemListFields.Brand]);
                            bomSetupItem.ProfitCenter = Convert.ToString(item[PackagingItemListFields.ProfitCenter]);
                            bomSetupItem.IsAllProcInfoCorrect = Convert.ToString(item[PackagingItemListFields.IsAllProcInfoCorrect]);
                            bomSetupItem.WhatProcInfoHasChanged = Convert.ToString(item[PackagingItemListFields.WhatProcInfoHasChanged]);
                            bomSetupItem.NewPrinterSupplierForLocation = Convert.ToString(item[PackagingItemListFields.NewPrinterSupplierForLocation]);

                            bomSetupItems.Add(bomSetupItem);
                        }
                    }
                }
            }
            return bomSetupItems;
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
        public BOMSetupItem GetBOMSetupItemByComponentId(int packagingId)
        {
            BOMSetupItem bomSetupItem = new BOMSetupItem();
            int iItemId = 0;
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPListItem item = spList.GetItemById(packagingId);
                    if (item != null)
                    {
                        bomSetupItem.Id = item.ID;
                        iItemId = Convert.ToInt32(item[PackagingItemListFields.CompassListItemId]);
                        bomSetupItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                        bomSetupItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                        bomSetupItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                        bomSetupItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                        bomSetupItem.NewExisting = Convert.ToString(item[PackagingItemListFields.NewExisting]);
                        bomSetupItem.CurrentLikeItem = Convert.ToString(item[PackagingItemListFields.CurrentLikeItem]);
                        bomSetupItem.CurrentLikeItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemDescription]);
                        bomSetupItem.CurrentLikeItemReason = Convert.ToString(item[PackagingItemListFields.CurrentLikeItemReason]);
                        bomSetupItem.CurrentOldItem = Convert.ToString(item[PackagingItemListFields.CurrentOldItem]);
                        bomSetupItem.CurrentOldItemDescription = Convert.ToString(item[PackagingItemListFields.CurrentOldItemDescription]);
                        bomSetupItem.PackQuantity = Convert.ToString(item[PackagingItemListFields.PackQuantity]);
                        bomSetupItem.SpecificationNo = Convert.ToString(item[PackagingItemListFields.SpecificationNo]);
                        bomSetupItem.LeadPlateTime = Convert.ToString(item[PackagingItemListFields.LeadPlateTime]);
                        bomSetupItem.LeadMaterialTime = Convert.ToString(item[PackagingItemListFields.LeadMaterialTime]);
                        bomSetupItem.PrinterSupplier = Convert.ToString(item[PackagingItemListFields.PrinterSupplier]);
                        bomSetupItem.Notes = Convert.ToString(item[PackagingItemListFields.Notes]);
                        bomSetupItem.ComponentContainsNLEA = Convert.ToString(item[PackagingItemListFields.ComponentContainsNLEA]);
                        bomSetupItem.GraphicsChangeRequired = Convert.ToString(item[PackagingItemListFields.GraphicsChangeRequired]);
                        bomSetupItem.ExternalGraphicsVendor = Convert.ToString(item[PackagingItemListFields.ExternalGraphicsVendor]);
                        bomSetupItem.GraphicsBrief = Convert.ToString(item[PackagingItemListFields.GraphicsBrief]);
                        bomSetupItem.PackUnit = Convert.ToString(item[PackagingItemListFields.PackUnit]);

                        bomSetupItem.MakeLocation = Convert.ToString(item[PackagingItemListFields.MakeLocation]);
                        bomSetupItem.CountryOfOrigin = Convert.ToString(item[PackagingItemListFields.CountryOfOrigin]);
                        bomSetupItem.PackLocation = Convert.ToString(item[PackagingItemListFields.PackLocation]);
                        bomSetupItem.NewFormula = Convert.ToString(item[PackagingItemListFields.NewFormula]);
                        bomSetupItem.TrialsCompleted = Convert.ToString(item[PackagingItemListFields.TrialsCompleted]);
                        bomSetupItem.ShelfLife = Convert.ToString(item[PackagingItemListFields.ShelfLife]);
                        bomSetupItem.Kosher = Convert.ToString(item[PackagingItemListFields.Kosher]);
                        bomSetupItem.Allergens = Convert.ToString(item[PackagingItemListFields.Allergens]);
                        bomSetupItem.SAPMaterialGroup = Convert.ToString(item[PackagingItemListFields.SAPMaterialGroup]);
                        bomSetupItem.TransferSEMIMakePackLocations = Convert.ToString(item[PackagingItemListFields.TransferSEMIMakePackLocations]);
                        bomSetupItem.Flowthrough = Convert.ToString(item[PackagingItemListFields.Flowthrough]);
                        bomSetupItem.ReviewPrinterSupplier = Convert.ToString(item[PackagingItemListFields.ReviewPrinterSupplier]);
                        bomSetupItem.DielineURL = Convert.ToString(item[PackagingItemListFields.DielineLink]);

                        bomSetupItem.FilmPrintStyle = Convert.ToString(item[PackagingItemListFields.FilmPrintStyle]);
                        bomSetupItem.CorrugatedPrintStyle = Convert.ToString(item[PackagingItemListFields.CorrugatedPrintStyle]);
                        bomSetupItem.SAPDescAbbrev = Convert.ToString(item[PackagingItemListFields.SAPDescAbbrev]);
                        bomSetupItem.NewPrinterSupplierForLocation = Convert.ToString(item[PackagingItemListFields.NewPrinterSupplierForLocation]);
                    }
                }
            }
            return bomSetupItem;
        }
        public void UpdateBOMSetupItem(BOMSetupItem bomSetupItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        SPListItem item = spList.GetItemById(bomSetupItem.Id);
                        if (item != null)
                        {
                            item[PackagingItemListFields.MaterialNumber] = bomSetupItem.MaterialNumber;
                            item[PackagingItemListFields.MaterialDescription] = bomSetupItem.MaterialDescription;
                            item[PackagingItemListFields.PackagingComponent] = bomSetupItem.PackagingComponent;
                            item[PackagingItemListFields.NewExisting] = bomSetupItem.NewExisting;
                            item[PackagingItemListFields.CurrentLikeItem] = bomSetupItem.CurrentLikeItem;
                            item[PackagingItemListFields.CurrentLikeItemDescription] = bomSetupItem.CurrentLikeItemDescription;
                            item[PackagingItemListFields.CurrentLikeItemReason] = bomSetupItem.CurrentLikeItemReason;
                            item[PackagingItemListFields.CurrentOldItem] = bomSetupItem.CurrentOldItem;
                            item[PackagingItemListFields.CurrentOldItemDescription] = bomSetupItem.CurrentOldItemDescription;
                            item[PackagingItemListFields.PackQuantity] = bomSetupItem.PackQuantity;
                            item[PackagingItemListFields.SpecificationNo] = bomSetupItem.SpecificationNo;
                            item[PackagingItemListFields.PurchasedIntoLocation] = bomSetupItem.PurchasedIntoLocation;
                            item[PackagingItemListFields.LeadPlateTime] = bomSetupItem.LeadPlateTime;
                            item[PackagingItemListFields.LeadMaterialTime] = bomSetupItem.LeadMaterialTime;
                            item[PackagingItemListFields.PrinterSupplier] = bomSetupItem.PrinterSupplier;
                            item[PackagingItemListFields.Notes] = bomSetupItem.Notes;
                            item[PackagingItemListFields.GraphicsChangeRequired] = bomSetupItem.GraphicsChangeRequired;
                            item[PackagingItemListFields.ExternalGraphicsVendor] = bomSetupItem.ExternalGraphicsVendor;
                            item[PackagingItemListFields.GraphicsBrief] = bomSetupItem.GraphicsBrief;
                            item[PackagingItemListFields.PackUnit] = bomSetupItem.PackUnit;
                            item[PackagingItemListFields.ParentID] = bomSetupItem.ParentID;
                            item[PackagingItemListFields.TransferSEMIMakePackLocations] = bomSetupItem.TransferSEMIMakePackLocations;

                            item[PackagingItemListFields.MakeLocation] = bomSetupItem.MakeLocation;
                            item[PackagingItemListFields.PackLocation] = bomSetupItem.PackLocation;
                            item[PackagingItemListFields.CountryOfOrigin] = bomSetupItem.CountryOfOrigin;
                            item[PackagingItemListFields.NewFormula] = bomSetupItem.NewFormula;
                            item[PackagingItemListFields.TrialsCompleted] = bomSetupItem.TrialsCompleted;
                            item[PackagingItemListFields.ShelfLife] = bomSetupItem.ShelfLife;
                            item[PackagingItemListFields.SAPMaterialGroup] = bomSetupItem.SAPMaterialGroup;
                            item[PackagingItemListFields.Flowthrough] = bomSetupItem.Flowthrough;
                            item[PackagingItemListFields.ComponentContainsNLEA] = bomSetupItem.ComponentContainsNLEA;
                            item[PackagingItemListFields.ReviewPrinterSupplier] = bomSetupItem.ReviewPrinterSupplier;
                            item[PackagingItemListFields.LastFormUpdated] = SPContext.Current.Item["Title"];
                            item[PackagingItemListFields.FilmPrintStyle] = bomSetupItem.FilmPrintStyle;
                            item[PackagingItemListFields.CorrugatedPrintStyle] = bomSetupItem.CorrugatedPrintStyle;
                            item[PackagingItemListFields.DielineLink] = bomSetupItem.DielineURL;
                            item[PackagingItemListFields.ThirteenDigitCode] = bomSetupItem.ThirteenDigitCode;
                            item[PackagingItemListFields.FourteenDigitBarcode] = bomSetupItem.FourteenDigitBarcode;
                            item[PackagingItemListFields.SAPDescAbbrev] = bomSetupItem.SAPDescAbbrev;
                            item[PackagingItemListFields.PHL1] = bomSetupItem.PHL1;
                            item[PackagingItemListFields.PHL2] = bomSetupItem.PHL2;
                            item[PackagingItemListFields.Brand] = bomSetupItem.Brand;
                            item[PackagingItemListFields.ProfitCenter] = bomSetupItem.ProfitCenter;
                            item[PackagingItemListFields.IsAllProcInfoCorrect] = bomSetupItem.IsAllProcInfoCorrect;
                            item[PackagingItemListFields.WhatProcInfoHasChanged] = bomSetupItem.WhatProcInfoHasChanged;
                            item[PackagingItemListFields.NewPrinterSupplierForLocation] = bomSetupItem.NewPrinterSupplierForLocation;

                            //item[PackagingItemListFields.Deleted] = bomSetupItem.Deleted;
                            SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                            if (user != null)
                            {
                                // Set Modified By to current user NOT System Account
                                item["Modified By"] = user.ID;
                            }

                            item.Update();
                            // Update our Packaging Components
                            UpdatePackagingComponents(spWeb, Convert.ToInt32(bomSetupItem.CompassListItemId));
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpsertPackMeasurementsItem(BOMSetupItem pmItem, string projectNumber)
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
                        spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + pmItem.CompassListItemId + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">" + pmItem.ParentID + "</Value></Eq></And></Where>";

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
                            item[CompassPackMeasurementsFields.ParentComponentId] = pmItem.ParentID;
                            item[CompassPackMeasurementsFields.SAPSpecsChange] = pmItem.SAPSpecsChange;
                            item[CompassPackMeasurementsFields.NotesSpec] = pmItem.NotesSpec;
                            item[CompassPackMeasurementsFields.PackSpecNumber] = pmItem.PackSpecNumber;
                            item[CompassPackMeasurementsFields.PalletSpecNumber] = pmItem.PalletSpecNumber;
                            item[CompassPackMeasurementsFields.PalletSpecLink] = pmItem.PalletSpecLink;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        public void UpsertPackMeasurementsItem(List<BOMSetupItem> pmItems, string projectNumber)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);

                        foreach (BOMSetupItem pmItem in pmItems)
                        {
                            SPQuery spQuery = new SPQuery();
                            spQuery.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + pmItem.CompassListItemId + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">" + pmItem.ParentID + "</Value></Eq></And></Where>";

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
                                item[CompassPackMeasurementsFields.ParentComponentId] = pmItem.ParentID;
                                item[CompassPackMeasurementsFields.SAPSpecsChange] = pmItem.SAPSpecsChange;
                                item[CompassPackMeasurementsFields.NotesSpec] = pmItem.NotesSpec;
                                item[CompassPackMeasurementsFields.PackSpecNumber] = pmItem.PackSpecNumber;
                                item[CompassPackMeasurementsFields.PalletSpecNumber] = pmItem.PalletSpecNumber;
                                item[CompassPackMeasurementsFields.PalletSpecLink] = pmItem.PalletSpecLink;

                                // Set Modified By to current user NOT System Account
                                item["Editor"] = SPContext.Current.Web.CurrentUser;

                                item.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
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
        public List<FileAttribute> GetUploadedFiles(string projectNo, string docType)
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
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => x.Item[CompassListFields.DOCLIBRARY_CompassDocType].Equals(docType)).ToList();
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
        public void UpdateTransferSemiMakePackLocations(List<BOMSetupItem> TSMakePackLocationsitems)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);

                        foreach (BOMSetupItem TSMakePackLocationsitem in TSMakePackLocationsitems)
                        {
                            SPListItem item = spList.GetItemById(TSMakePackLocationsitem.Id);
                            if (item != null)
                            {

                                item[PackagingItemListFields.TransferSEMIMakePackLocations] = TSMakePackLocationsitem.TransferSEMIMakePackLocations;
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
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public BOMSetupItem GetPackMeasurementsItem(int itemId, int parentId)
        {
            BOMSetupItem pmItem = new BOMSetupItem();
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
                            pmItem.Id = item.ID;
                            pmItem.CompassListItemId = Convert.ToInt32(item[CompassPackMeasurementsFields.CompassListItemId]);

                            pmItem.SAPSpecsChange = Convert.ToString(item[CompassPackMeasurementsFields.SAPSpecsChange]);
                            pmItem.NotesSpec = Convert.ToString(item[CompassPackMeasurementsFields.NotesSpec]);
                            pmItem.PackSpecNumber = Convert.ToString(item[CompassPackMeasurementsFields.PackSpecNumber]);
                            pmItem.PalletSpecNumber = Convert.ToString(item[CompassPackMeasurementsFields.PalletSpecNumber]);

                            pmItem.PalletSpecLink = Convert.ToString(item[CompassPackMeasurementsFields.PalletSpecLink]);
                            pmItem.ParentID = Convert.ToInt32(item[CompassPackMeasurementsFields.ParentID]);

                        }
                    }
                }
            }
            return pmItem;
        }
        public List<BOMSetupItem> GetPackMeasurementsItems(int itemId)
        {
            List<BOMSetupItem> pmItems = new List<BOMSetupItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";

                    // spQuery.RowLimit = 1;
                    SPListItemCollection compassItemCol;

                    compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (item != null)
                            {
                                BOMSetupItem pmItem = new BOMSetupItem();
                                pmItem.Id = item.ID;
                                pmItem.CompassListItemId = Convert.ToInt32(item[CompassPackMeasurementsFields.CompassListItemId]);

                                pmItem.SAPSpecsChange = Convert.ToString(item[CompassPackMeasurementsFields.SAPSpecsChange]);
                                pmItem.NotesSpec = Convert.ToString(item[CompassPackMeasurementsFields.NotesSpec]);
                                pmItem.PackSpecNumber = Convert.ToString(item[CompassPackMeasurementsFields.PackSpecNumber]);
                                pmItem.PalletSpecNumber = Convert.ToString(item[CompassPackMeasurementsFields.PalletSpecNumber]);

                                pmItem.PalletSpecLink = Convert.ToString(item[CompassPackMeasurementsFields.PalletSpecLink]);
                                pmItem.ParentID = Convert.ToInt32(item[CompassPackMeasurementsFields.ParentComponentId]);
                                pmItems.Add(pmItem);
                            }
                        }
                    }

                    foreach (var pmItem in pmItems)
                    {
                        if (pmItem.ParentID != 0)
                        {
                            SPList spPackagingList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                            SPListItem compassPackagingItem = spPackagingList.GetItemById(pmItem.ParentID);

                            if (compassPackagingItem != null)
                            {
                                pmItem.PackagingComponent = Convert.ToString(compassPackagingItem[PackagingItemListFields.PackagingComponent]);
                                pmItem.MaterialDescription = Convert.ToString(compassPackagingItem[PackagingItemListFields.MaterialDescription]);
                                pmItem.MaterialNumber = Convert.ToString(compassPackagingItem[PackagingItemListFields.MaterialNumber]);
                            }
                        }
                    }
                }
            }
            return pmItems;
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
                        #region Compass List
                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);

                        SPListItem item = spList.GetItemById(materialItem.CompassListItemId);
                        if (item != null)
                        {
                            var PELead = Convert.ToString(item[CompassListFields.PackagingEngineerLead]);
                            if (pageName == "BOMSetupPE" && !string.IsNullOrEmpty(materialItem.PackagingEngineerLead))
                            {
                                if (materialItem.PackagingEngineerLead != GlobalConstants.GROUP_PackagingEngineer)
                                {
                                    item[CompassListFields.PackagingEngineerLead] = materialItem.PackagingEngineerLead;
                                }
                                else
                                {
                                    SPGroup PEGroup = spWeb.Groups[GlobalConstants.GROUP_PackagingEngineer];
                                    item[CompassListFields.PackagingEngineerLead] = PEGroup;
                                }
                            }
                            else if (pageName == GlobalConstants.PAGE_BOMSetupPE2 || pageName == GlobalConstants.PAGE_BOMSetupPE3)
                            {
                                if (string.IsNullOrEmpty(PELead))
                                {
                                    item[CompassListFields.PackagingEngineerLead] = materialItem.PackagingEngineerLead;
                                }
                            }
                            // Set Modified By to current user NOT System Account
                            item[CompassListFields.LastUpdatedFormName] = pageName;
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }
                        #endregion
                        #region Compass List 2
                        SPList spCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spCompassList2Query = new SPQuery();
                        spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + materialItem.CompassListItemId + "</Value></Eq></Where>";

                        SPListItemCollection compassIList2temCol = spCompassList2.GetItems(spCompassList2Query);
                        if (compassIList2temCol.Count > 0)
                        {
                            SPListItem item2 = compassIList2temCol[0];
                            if (item2 != null)
                            {
                                if (pageName == GlobalConstants.PAGE_BOMSetupPE2)
                                {
                                    item2[CompassList2Fields.AllAspectsApprovedFromPEPersp] = materialItem.AllAspectsApprovedFromPEPersp;
                                    item2[CompassList2Fields.WhatIsIncorrectPE] = materialItem.WhatIsIncorrectPE;
                                }

                                // Set Modified By to current user NOT System Account
                                item2["Editor"] = SPContext.Current.Web.CurrentUser;
                                item2.Update();
                            }
                        }
                        #endregion
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateBillofMaterialsApprovalItem(ApprovalItem approvalItem, string pageName, bool bSubmitted)
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
                                if (string.Equals(pageName.ToLower(), GlobalConstants.PAGE_BOMSetupPE.ToLower()))
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
                                else if (string.Equals(pageName.ToLower(), GlobalConstants.PAGE_BOMSetupPE2.ToLower()))
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
                                else if (string.Equals(pageName.ToLower(), GlobalConstants.PAGE_BOMSetupProc.ToLower()))
                                {
                                    appItem[ApprovalListFields.BOMSetupProc_ModifiedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.BOMSetupProc_ModifiedBy] = approvalItem.ModifiedBy;
                                }
                                else if (string.Equals(pageName.ToLower(), GlobalConstants.PAGE_BOMSetupPE3.ToLower()))
                                {
                                    if ((bSubmitted) && (appItem[ApprovalListFields.BOMSetupPE3_SubmittedDate] == null))
                                    {
                                        appItem[ApprovalListFields.BOMSetupPE3_SubmittedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.BOMSetupPE3_SubmittedBy] = approvalItem.ModifiedBy;
                                    }
                                    else
                                    {
                                        appItem[ApprovalListFields.BOMSetupPE3_ModifiedDate] = approvalItem.ModifiedDate;
                                        appItem[ApprovalListFields.BOMSetupPE3_ModifiedBy] = approvalItem.ModifiedBy;
                                    }
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public SAPBOMSetupItem GetSAPBOMSetupItem(int itemId)
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
                        sgItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        sgItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);
                        sgItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        sgItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        sgItem.ProjectTypeSubCategory = Convert.ToString(item[CompassListFields.ProjectTypeSubCategory]);
                        sgItem.MfgLocationChange = Convert.ToString(item[CompassListFields.MfgLocationChange]);
                        sgItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);

                        sgItem.ImmediateSPKChange = Convert.ToString(item[CompassListFields.ImmediateSPKChange]);
                        sgItem.MakeLocation = Convert.ToString(item[CompassListFields.ManufacturingLocation]);
                        sgItem.PackingLocation = Convert.ToString(item[CompassListFields.PackingLocation]);
                        sgItem.PurchasedIntoLocation = Convert.ToString(item[CompassListFields.PurchasedIntoLocation]);

                        sgItem.SAPBaseUOM = Convert.ToString(item[CompassListFields.SAPBaseUOM]);

                        sgItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        sgItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        sgItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        sgItem.MaterialGroup2Pricing = Convert.ToString(item[CompassListFields.MaterialGroup2Pricing]);
                        sgItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        sgItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                        sgItem.ProfitCenter = Convert.ToString(item[CompassListFields.ProfitCenter]);
                        sgItem.ProcurementType = Convert.ToString(item[CompassListFields.CoManufacturingClassification]);

                        sgItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        sgItem.CaseUCC = Convert.ToString(item[CompassListFields.CaseUCC]);
                        sgItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        sgItem.PalletUCC = Convert.ToString(item[CompassListFields.PalletUCC]);
                        sgItem.ExternalManufacturer = Convert.ToString(item[CompassListFields.ExternalManufacturer]);
                        sgItem.ExternalPacker = Convert.ToString(item[CompassListFields.ExternalPacker]);
                        sgItem.PLMProject = Convert.ToString(item[CompassListFields.PLMProject]);
                    }
                    #region LIST_CompassList2Name
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spCompassList2Query = new SPQuery();
                    spCompassList2Query.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq></Where>";
                    spCompassList2Query.RowLimit = 1;

                    SPListItemCollection compassList2ItemCol = spList.GetItems(spCompassList2Query);
                    if (compassList2ItemCol.Count > 0)
                    {
                        SPListItem compassList2Item = compassList2ItemCol[0];
                        if (compassList2Item != null)
                        {
                            sgItem.DesignateHUBDC = Convert.ToString(compassList2Item[CompassList2Fields.DesignateHUBDC]);
                        }
                    }
                    #endregion
                    #region LIST_ProjectDecisionsListName
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
                            sgItem.FinishedGoodBOMSetup = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FinishedGoodBOMSetup]);
                            sgItem.NewMaterialNumbersCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewMaterialNumbersCreated]);
                            sgItem.ContBuildFGBOM = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ContBuildFGBOM]);
                            sgItem.TransferSemiBOMSetup = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TransferSemiBOMSetup]);
                            sgItem.TransferMatNumCreatd = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TransferMatNumCreatd]);
                            sgItem.HardSoftTransition = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_HardSoftTransition]);
                            sgItem.TransferSAPSpecsChangeCompleted = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSSAPSpecsChangeComp]);
                            sgItem.FGSAPSpecsChangeCompleted = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGSAPSpecsChangeComp]);
                            sgItem.TurnkeyFGMMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TurnkeyFGMaterialMasterCreated]);
                            sgItem.CompleteFGBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_CompleteFGBOMCreated]);
                            sgItem.CompleteTSBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_CompleteTSBOMCreated]);
                            sgItem.PackMatsCreatedInPackLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_PackMatsCreatedInPackLoc]);
                            sgItem.FGBOMCreatedInNewPackLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGBOMCreatedInNewPackLoc]);
                            sgItem.SPKUpdatedInDCsPack = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsPack]);
                            sgItem.TSCompsCreatedInNewMPLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsCreatedInNewMPLoc]);
                            sgItem.TSFGBOMCreatedInNewMakeLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSFGBOMCreatedInNewMakeLoc]);
                            sgItem.SPKUpdatedInDCsMake = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_SPKUpdatedInDCsMake]);
                            sgItem.ProdVersionCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ProdVersionCreated]);
                            sgItem.CreateNewPURCNDYSAPMatNum = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_CreateNewPURCNDYSAPMatNum]);
                            sgItem.NewFGBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewFGBOMCreated]);
                            sgItem.NewTSMaterialNumbersCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewTSMaterialNumbersCreated]);
                            sgItem.NewTSCompPackNumsCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_NewTSComponentPackNumsCreated]);
                            sgItem.InitialFGBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_InitialFGBOMCreated]);
                            sgItem.InitialTSBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_InitialTSBOMCreated]);
                            sgItem.FGSubConBOMCreated = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGSubConBOMCreated]);
                            sgItem.ExtendFGToDCs = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGToDCs]);
                            sgItem.VerifyFGBOMInDCs = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGBOMInDCs]);
                            sgItem.GS1Calculator = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_GS1Calculator]);
                            sgItem.FGPrivateLable = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGPrivateLable]);
                            sgItem.FGDCFP07 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP07]);
                            sgItem.FGDCFP13 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGDCFP13]);
                            sgItem.FGSPKOthers = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_FGSPKOthers]);
                            sgItem.VerifyPrivateLabel = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyPrivateLabel]);
                            sgItem.VerifyFGDCFP07 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP07]);
                            sgItem.VerifyFGDCFP13 = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_VerifyFGDCFP13]);
                            sgItem.AddZSTOMatEntry = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_AddZSTOMatEntry]);
                            sgItem.TSCompsExtendedInNewMPLoc = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsExtendedInNewMPLoc]);
                            sgItem.SPKsUpdatedPerDeployment = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_SPKsUpdatedPerDeployment]);
                            sgItem.ExtProfitCenterToDC = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ExtProfitCenterToDC]);
                            sgItem.ClckNewTSPCPrftCntr = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ClckNewTSPCPrftCntr]);
                            sgItem.ExtendFGHL12Brand = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGHL12Brand]);
                            sgItem.ApplySemiHL12Brand = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_ApplySemiHL12Brand]);
                            sgItem.AddOldComp = Convert.ToString(decisionItem[CompassProjectDecisionsListFields.SAPBOMSetup_AddOldComp]);
                        }
                    }
                    #endregion
                    spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassPackMeasurementsListName);
                    SPQuery spQuery2 = new SPQuery();
                    spQuery2.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Eq><FieldRef Name=\"ParentComponentId\" /><Value Type=\"Int\">0</Value></Eq></And></Where>";
                    SPListItemCollection compassItemCol2 = spList.GetItems(spQuery2);
                    sgItem.FGSAPSpecsChangePackMeas = "";
                    if (compassItemCol2.Count > 0)
                    {
                        SPListItem decisionItem2 = compassItemCol2[0];
                        if (decisionItem2 != null)
                        {
                            sgItem.DoubleStackable = Convert.ToString(decisionItem2[CompassPackMeasurementsFields.DoubleStackable]);
                            sgItem.FGSAPSpecsChangePackMeas = Convert.ToString(decisionItem2[CompassPackMeasurementsFields.SAPSpecsChange]);
                        }
                    }
                    SPQuery spQuery3 = new SPQuery();
                    spQuery3.Query = "<Where><And><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + itemId + "</Value></Eq><Neq><FieldRef Name=\"" + CompassPackMeasurementsFields.ParentComponentId + "\" /><Value Type=\"Int\">0</Value></Neq></And></Where>";
                    SPListItemCollection compassItemCol3 = spList.GetItems(spQuery3);
                    sgItem.TransferSAPSpecsChangePackMeas = "no";
                    if (compassItemCol3.Count > 0)
                    {
                        foreach (SPListItem decisionItem3 in compassItemCol3)
                        {
                            if (decisionItem3 != null)
                            {
                                string SAPSpecsChange = Convert.ToString(decisionItem3[CompassPackMeasurementsFields.SAPSpecsChange]);
                                if (SAPSpecsChange.ToLower() == "yes")
                                {
                                    sgItem.TransferSAPSpecsChangePackMeas = SAPSpecsChange;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return sgItem;
        }
        public void UpdateSAPBOMSetupItem(SAPBOMSetupItem sapBOMSetupItem)
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
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + sapBOMSetupItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        SPListItem item = compassItemCol[0];
                        if (item != null)
                        {
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ContBuildFGBOM] = sapBOMSetupItem.ContBuildFGBOM;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_NewMaterialNumbersCreated] = sapBOMSetupItem.NewMaterialNumbersCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_GS1Calculator] = sapBOMSetupItem.GS1Calculator;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_NewTSComponentPackNumsCreated] = sapBOMSetupItem.NewTSCompPackNumsCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_PackMatsCreatedInPackLoc] = sapBOMSetupItem.PackMatsCreatedInPackLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_FGBOMCreatedInNewPackLoc] = sapBOMSetupItem.FGBOMCreatedInNewPackLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ExtProfitCenterToDC] = sapBOMSetupItem.ExtProfitCenterToDC;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_InitialTSBOMCreated] = sapBOMSetupItem.InitialTSBOMCreated;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsExtendedInNewMPLoc] = sapBOMSetupItem.TSCompsExtendedInNewMPLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_TSCompsCreatedInNewMPLoc] = sapBOMSetupItem.TSCompsCreatedInNewMPLoc;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ExtendFGHL12Brand] = sapBOMSetupItem.ExtendFGHL12Brand;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_ApplySemiHL12Brand] = sapBOMSetupItem.ApplySemiHL12Brand;
                            item[CompassProjectDecisionsListFields.SAPBOMSetup_AddOldComp] = sapBOMSetupItem.AddOldComp;

                            // Set Modified By to current user NOT System Account
                            item["Editor"] = SPContext.Current.Web.CurrentUser;

                            item.Update();
                        }

                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        #region Private Methods
        private void UpdatePackagingComponents(SPWeb spWeb, int compassListItemId)
        {
            SPListItemCollection compassItemCol;
            SPListItem decisionItem;
            SPList spList;
            SPQuery spQuery;
            List<PackagingItem> packagingItems = packagingItemService.GetAllPackagingItemsForProject(compassListItemId);
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
            StringBuilder packagingComponentsNewPURCANDYSemisPackLOcationTable = new StringBuilder();
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
            packagingComponentsNMPURCANDYSemisPackLocationTable.Append(GenerateNewPURCANDYSemisPackLocationTables(packagingItems));

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
                decisionItem[CompassProjectDecisionsListFields.NMTSsForSAPIntItemSetup] = NMTSsForSAPIntItemSetupTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NewPurCandySemis] = packagingComponentsNewPURCANDYSemisTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NewPurCandySemisPackLocation] = packagingComponentsNewPURCANDYSemisPackLOcationTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NMPurCandySemisPackLocation] = packagingComponentsNMPURCANDYSemisPackLocationTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NetworkMoveTSPackLocations] = packagingComponentsNetworkTransferSemisTable.ToString();
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
        }
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
                    value.Append("<td><b>" + (isTransferSemi ? "Make" : "Pack") + " Location</b></td>");
                    value.Append("<td><b>14 Digit Code</b></td></tr>");
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
        private string GenerateNewTransferSemiWithPackLocationsTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();
            List<PackagingItem> dtPackingItemsTS =
                (
                    from PIs in AllPackagingItems
                    where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && PIs.NewExisting == "New"
                    select PIs
                ).ToList<PackagingItem>();

            var EligibleTransferSemis =
                   (
                       from packgingItem in AllPackagingItems
                       where packgingItem.PackagingComponent.ToLower().Contains("transfer") && (packgingItem.PackLocation.Contains("FQ22") || packgingItem.PackLocation.Contains("FQ25"))
                       select packgingItem
                   ).ToList();

            if (dtPackingItemsTS.Count > 0)
            {
                TSSummary.Append("<B>New Transfer Semis :</B>");
                TSSummary.Append(GenerateNewTransferSemiWithPackLocations(true, null, ""));
                foreach (PackagingItem item in dtPackingItemsTS)
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
            List<PackagingItem> AllTransferSemis = (from PIs in AllPackagingItems where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && PIs.NewExisting == "Network Move" select PIs).ToList<PackagingItem>();

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
                value.Append("<tr><td><b>PUR CANDY Semi #</b></td>");
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
                value.Append("<tr><td><b>Purchased Semi #</b></td>");
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
        #endregion
    }
}