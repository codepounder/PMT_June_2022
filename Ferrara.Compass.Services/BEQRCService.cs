using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ferrara.Compass.Services
{
    public class BEQRCService : IBEQRCService
    {
        private readonly IPackagingItemService PackagingItemService;
        public BEQRCService(IPackagingItemService PackagingItemService)
        {
            this.PackagingItemService = PackagingItemService;
        }
        #region Get
        public BEQRCItem GetBEQRCItem(int iItemId)
        {
            var newItem = new BEQRCItem();
            using (var spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (var spWeb = spSite.OpenWeb())
                {
                    var spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassListName);
                    var item = spList.GetItemById(iItemId);
                    if (item != null)
                    {
                        // Proposed Project Fields
                        newItem.TBDIndicator = Convert.ToString(item[CompassListFields.TBDIndicator]);
                        newItem.ProjectNumber = Convert.ToString(item[CompassListFields.ProjectNumber]);
                        newItem.ProjectType = Convert.ToString(item[CompassListFields.ProjectType]);
                        newItem.MaterialGroup1Brand = Convert.ToString(item[CompassListFields.MaterialGroup1Brand]);
                        newItem.ItemConcept = Convert.ToString(item[CompassListFields.ItemConcept]);
                        newItem.FirstShipDate = Convert.ToDateTime(item[CompassListFields.FirstShipDate]);
                        newItem.RevisedFirstShipDate = Convert.ToDateTime(item[CompassListFields.RevisedFirstShipDate]);
                        newItem.SAPItemNumber = Convert.ToString(item[CompassListFields.SAPItemNumber]);
                        newItem.SAPDescription = Convert.ToString(item[CompassListFields.SAPDescription]);

                        // Item Financial Details Fields
                        if (item[CompassListFields.AnnualProjectedDollars] != null)
                        {
                            try { newItem.AnnualProjectedDollars = Convert.ToDouble(item[CompassListFields.AnnualProjectedDollars]); }
                            catch { newItem.AnnualProjectedDollars = -9999; }
                        }
                        else
                        {
                            newItem.AnnualProjectedDollars = -9999;
                        }

                        if (item[CompassListFields.AnnualProjectedUnits] != null)
                        {
                            try { newItem.AnnualProjectedUnits = Convert.ToInt32(item[CompassListFields.AnnualProjectedUnits]); }
                            catch { newItem.AnnualProjectedUnits = -9999; }
                        }
                        else
                        {
                            newItem.AnnualProjectedUnits = -9999;
                        }

                        if (item[CompassListFields.ExpectedGrossMarginPercent] != null)
                        {
                            try { newItem.ExpectedGrossMarginPercent = Convert.ToDouble(item[CompassListFields.ExpectedGrossMarginPercent]); }
                            catch { newItem.ExpectedGrossMarginPercent = -9999; }
                        }
                        else
                        {
                            newItem.ExpectedGrossMarginPercent = -9999;
                        }
                        newItem.DisplayBoxUPC = Convert.ToString(item[CompassListFields.DisplayBoxUPC]);
                        newItem.UnitUPC = Convert.ToString(item[CompassListFields.UnitUPC]);
                        // Customer Specificaitons Fields
                        newItem.Customer = Convert.ToString(item[CompassListFields.Customer]);

                        // Item Hierarchy Fields
                        newItem.ProductHierarchyLevel1 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel1]);
                        newItem.ProductHierarchyLevel2 = Convert.ToString(item[CompassListFields.ProductHierarchyLevel2]);
                        newItem.MaterialGroup4ProductForm = Convert.ToString(item[CompassListFields.MaterialGroup4ProductForm]);
                        newItem.MaterialGroup5PackType = Convert.ToString(item[CompassListFields.MaterialGroup5PackType]);
                    }
                    #region Compass List 2
                    SPList spListCompassList2 = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + iItemId + "</Value></Eq></Where>";

                    SPListItemCollection compassItem2Col = spListCompassList2.GetItems(spQuery);
                    if (compassItem2Col != null && compassItem2Col.Count > 0)
                    {
                        var item2 = compassItem2Col[0];
                        if (item2 != null)
                        {
                            newItem.ConsumerFacingProdDesc = Convert.ToString(item2[CompassList2Fields.ConsumerFacingProdDesc]);
                        }

                    }
                    #endregion
                }
            }
            return newItem;
        }
        public List<PackagingItem> GetAllProjectItems(int ItemId)
        {
            List<PackagingItem> packagingItems = new List<PackagingItem>();
            using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
            {
                using (SPWeb spWeb = spSite.OpenWeb())
                {
                    SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_PackagingItemListName);
                    SPQuery spQuery = new SPQuery();
                    spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + ItemId + "</Value></Eq></Where>";

                    SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                    if (compassItemCol.Count > 0)
                    {
                        foreach (SPListItem item in compassItemCol)
                        {
                            if (string.Equals(item[PackagingItemListFields.Deleted], "Yes"))
                                continue;

                            PackagingItem packagingItem = new PackagingItem();
                            packagingItem.Id = item.ID;
                            packagingItem.ParentID = Convert.ToInt32(item[PackagingItemListFields.ParentID]);
                            packagingItem.CompassListItemId = Convert.ToString(item[PackagingItemListFields.CompassListItemId]);
                            packagingItem.PackagingComponent = Convert.ToString(item[PackagingItemListFields.PackagingComponent]);
                            packagingItem.MaterialNumber = Convert.ToString(item[PackagingItemListFields.MaterialNumber]);
                            packagingItem.MaterialDescription = Convert.ToString(item[PackagingItemListFields.MaterialDescription]);
                            packagingItem.IngredientsNeedToClaimBioEng = Convert.ToString(item[PackagingItemListFields.IngredientsNeedToClaimBioEng]);
                            packagingItem.UPCAssociated = Convert.ToString(item[PackagingItemListFields.UPCAssociated]);
                            packagingItem.UPCAssociatedManualEntry = Convert.ToString(item[PackagingItemListFields.UPCAssociatedManualEntry]);
                            packagingItem.BioEngLabelingRequired = Convert.ToString(item[PackagingItemListFields.BioEngLabelingRequired]);

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
        }
        public string GetPackagingComponentsWithQRCodeForEmail(int compassListItemId)
        {
            List<PackagingItem> packagingItems = GetAllProjectItems(compassListItemId);
            BEQRCItem compassItem = GetBEQRCItem(compassListItemId);
            StringBuilder packagingComponentsTable = new StringBuilder();
            packagingComponentsTable.Append(GeneratePackagingComponents(true, null, "", "", ""));

            foreach (PackagingItem item in packagingItems.Where(comp => comp.BioEngLabelingRequired.Contains("Yes")))
            {
                packagingComponentsTable.Append(GeneratePackagingComponents(false, item, compassItem.SAPDescription, compassItem.SAPItemNumber, compassItem.MaterialGroup1Brand));
            }
            packagingComponentsTable.Append("</table>");

            return packagingComponentsTable.ToString();
        }
        public List<FileAttribute> GetBEQRCodeEPSFileUploadedFiles(string projectNo)
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
                        var spfiles = stageGateProjectFolder.Files.OfType<SPFile>().Where(x => Convert.ToString(x.Item[CompassListFields.DOCLIBRARY_CompassDocType]).Equals(GlobalConstants.DOCTYPE_BEQRCodeEPSFile));
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

        #endregion
        #region Update
        public void UpdateBEQRCPackagingItems(List<PackagingItem> packagingItems, int compassId)
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

                                    item[PackagingItemListFields.UPCAssociated] = packagingItem.UPCAssociated;
                                    item[PackagingItemListFields.UPCAssociatedManualEntry] = packagingItem.UPCAssociatedManualEntry;
                                    item[PackagingItemListFields.BioEngLabelingRequired] = packagingItem.BioEngLabelingRequired;
                                    item[PackagingItemListFields.FlowthroughMaterialsSpecs] = packagingItem.FlowthroughMaterialsSpecs;

                                    SPUser user = spWeb.EnsureUser(SPContext.Current.Web.CurrentUser.LoginName);
                                    if (user != null)
                                    {
                                        // Set Modified By to current user NOT System Account
                                        item["Modified By"] = user.ID;
                                    }
                                    item[PackagingItemListFields.LastFormUpdated] = GlobalConstants.PAGE_BEQRC;
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
        public void UpdateBEQRCItem(BEQRCItem BEQRCItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + BEQRCItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem comp2tem = compassItemCol[0];
                            if (comp2tem != null)
                            {
                                comp2tem[CompassList2Fields.ConsumerFacingProdDesc] = BEQRCItem.ConsumerFacingProdDesc;
                                comp2tem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateBEQRCApprovalItem(ApprovalItem approvalItem, bool bSubmitted)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_ApprovalList2Name);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + approvalItem.CompassListItemId + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem appItem = compassItemCol[0];
                            if (appItem != null)
                            {
                                if ((bSubmitted) && (appItem[ApprovalListFields.BEQRC_SubmittedDate] == null))
                                {
                                    appItem[ApprovalListFields.BEQRC_SubmittedDate] = approvalItem.ModifiedDate;
                                    appItem[ApprovalListFields.BEQRC_SubmittedBy] = approvalItem.ModifiedBy;
                                }
                                else
                                {
                                    appItem[ApprovalListFields.BEQRC_ModifiedBy] = approvalItem.ModifiedBy;
                                    appItem[ApprovalListFields.BEQRC_ModifiedDate] = approvalItem.ModifiedDate;
                                }

                                appItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }
        public void UpdateBEQRCRequestEmailSent(int compassListItem)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        spWeb.AllowUnsafeUpdates = true;

                        SPList spList = spWeb.Lists.TryGetList(GlobalConstants.LIST_CompassWorkflowStatusListName);
                        SPQuery spQuery = new SPQuery();
                        spQuery.Query = "<Where><Eq><FieldRef Name=\"CompassListItemId\" /><Value Type=\"Int\">" + compassListItem + "</Value></Eq></Where>";
                        spQuery.RowLimit = 1;

                        SPListItemCollection compassItemCol = spList.GetItems(spQuery);
                        if (compassItemCol.Count > 0)
                        {
                            SPListItem wfStatusItem = compassItemCol[0];
                            if (wfStatusItem != null)
                            {
                                wfStatusItem[CompassWorkflowStatusListFields.BEQRCRequestSent_Completed] = "Yes";
                                wfStatusItem.Update();
                            }
                        }
                        spWeb.AllowUnsafeUpdates = false;
                    }
                }
            });
        }

        #endregion
        #region Insert
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
        #endregion
        #region Private
        private string GeneratePackagingComponents(bool bHeader, PackagingItem item, string SAPDescription, string FGNumber, string Brand)
        {
            StringBuilder value = new StringBuilder();
            if (bHeader)
            {
                value.Append("<table style='border: 1px solid black; width: 100%;' cellpadding='1' cellspacing='10'>");
                value.Append("<tr>");
                value.Append("<td><b>Brand</b></td>");
                value.Append("<td><b>SAP Description</b></td>");
                value.Append("<td><b>UPC</b></td>");
                value.Append("<td><b>Material Description</b></td>");
                value.Append("<td><b>FG #</b></td>");
                value.Append("<td><b>Packaging Component Number</b></td>");
                value.Append("<td><b>Component Type</b></td>");
                value.Append("</tr>");
            }
            else
            {
                var UPCAssociated = (item.UPCAssociated == "Manual Entry") ? item.UPCAssociatedManualEntry : item.UPCAssociated;
                value.Append("<tr>");
                value.Append("<td>" + Brand + "</td>");
                value.Append("<td>" + SAPDescription + "</td>");
                value.Append("<td>" + UPCAssociated + "</td>");
                value.Append("<td>" + item.MaterialDescription + "</td>");
                value.Append("<td>" + FGNumber + "</td>");
                value.Append("<td>" + item.MaterialNumber + "</td>");
                value.Append("<td>" + item.PackagingComponent + "</td>");
                value.Append("</tr>");
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
                decisionItem[CompassProjectDecisionsListFields.NMPurCandySemisPackLocation] = packagingComponentsNMPURCANDYSemisPackLocationTable.ToString();
                decisionItem[CompassProjectDecisionsListFields.NetworkMoveTSPackLocations] = packagingComponentsNetworkTransferSemisTable.ToString();
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
        private List<PackagingItem> GetAllPackagingItemsForProject(int compassListItemId)
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

                            packagingItems.Add(packagingItem);
                        }
                    }
                }
            }
            return packagingItems;
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
        private string GenerateNMPURCANDYSemisPackLocationTables(List<PackagingItem> AllPackagingItems)
        {
            StringBuilder TSSummary = new StringBuilder();
            TSSummary.Clear();

            List<PackagingItem> dtPackingItemsTS = (from PIs in AllPackagingItems where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi && PIs.NewExisting == "Network Move" select PIs).ToList<PackagingItem>();

            if (dtPackingItemsTS.Count > 0)
            {
                TSSummary.Append("<B>Network Move Purchased Semis :</B>");
                TSSummary.Append(GenerateNewPURCANDYSemisPackLocation(true, null));
                foreach (PackagingItem item in dtPackingItemsTS)
                {
                    TSSummary.Append(GenerateNewPURCANDYSemisPackLocation(false, item));
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
        private SAPBOMSetupItem GetCompassListForProject(int itemId)
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
        private List<PackagingItem> GetNewPURCANDYSemiItemsForProject(int stageListItemId)
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

        #endregion
    }
}
