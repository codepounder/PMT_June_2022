using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Linq;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Services;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucBOMEditable : UserControl
    {
        #region Member Variables
        private IPackagingItemService packagingItemService;
        private IItemProposalService IPFService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private ISAPBOMService sapBOMService;
        private ISAPMaterialMasterService sapMMService;
        private IExternalManufacturingService extMgfService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<ShipperFinishedGoodItem> shipperFGItems = new List<ShipperFinishedGoodItem>();
        #endregion

        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }
        private List<KeyValuePair<int, string>> TSIds { get; set; }
        private List<KeyValuePair<int, string>> PSIds { get; set; }
        public int PackagingItemId { get; set; }
        public int CompassItemId { get; set; }
        public string PackagingComponent { get; set; }
        public string CompassListId { get; set; }
        public string FilmSubstrate { get; set; }
        public string ParentID { get; set; }
        public bool IsNew { get; set; }
        public List<PackagingItem> AllPIs;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnPackagingItemId.Value = PackagingItemId.ToString();
            webUrl = SPContext.Current.Web.Url;

            try
            {
                using (SPSite site = new SPSite(webUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        Utilities.BindDropDownItems(drpPkgComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, web.Url);
                        Utilities.BindDropDownItems(ddlFilmStyle, GlobalConstants.LIST_FilmStyleLookup, web.Url);
                        Utilities.BindDropDownItems(ddlPrinter, GlobalConstants.LIST_PrinterSupplierLookup, web.Url);
                        Utilities.BindDropDownItems(ddlPackUnit, GlobalConstants.LIST_PackUnitLookup, web.Url);
                        Utilities.BindDropDownItems(ddlFilmStructure, GlobalConstants.LIST_SubstrateLookup, web.Url);
                        Utilities.BindDropDownItems(ddlFilmSubstrate, GlobalConstants.LIST_FilmSubstrate, webUrl);
                        Utilities.BindDropDownItems(ddlStructureColor, GlobalConstants.LIST_SubstrateColorLookup, webUrl);
                        Utilities.BindDropDownItems(drpFilmBackSeam, GlobalConstants.LIST_BackSeamsLookup, webUrl);
                        Utilities.BindDropDownItemsById(ddlFilmUnWind, GlobalConstants.LIST_FilmUnWindLookup, webUrl);
                        Utilities.BindDropDownItemsById(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);

                        Utilities.BindDropDownItems(drpTSCountryOfOrigin, GlobalConstants.LIST_ManufacturerCountryOfOrigin, web.Url);
                        Utilities.BindDropDownWithTitleFilter(drpTransferLocation, GlobalConstants.LIST_MakePackTransfersLookup, webUrl, false, "PURCHASED");
                        Utilities.BindDropDownWithTitleFilter(drpTSPackLocation, GlobalConstants.LIST_PackingLocationsLookup, webUrl, false, "External");
                        Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);

                        Utilities.BindDropDownItems(drpPCSCountryofOrigin, GlobalConstants.LIST_ManufacturerCountryOfOrigin, web.Url);
                        Utilities.BindDropDownWithTitleFilter(drpPurchasedCandyLocation, GlobalConstants.LIST_MakePackTransfersLookup, webUrl, true, "PURCHASED");
                        Utilities.BindDropDownItems(drpPCSPackLocation, GlobalConstants.LIST_CoPackers, webUrl);

                        InitializeScreen();
                    }
                }
                if (!Page.IsPostBack)
                {
                    if (iItemId > 0)
                    {
                        //  LoadBOMItems();
                        dvMain.Visible = false;
                    }
                }

                hideItemsforSAPBOMPage();

                hideSpecificationFieldForNonPEPages();
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "PackagingComponentEntryForm", "Page_Load");
            }

            if (PackagingComponent == "Transfer")
            {
                ListItem removeItem = drpPkgComponent.Items.FindByValue("52");
                drpPkgComponent.Items.Remove(removeItem);
            }

            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            AllPIs = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            this.hiddenItemId.Value = iItemId.ToString();
            hdnPackagingComponent.Value = IsNew ? "" : PackagingComponent;
            hdnParentID.Value = ParentID.ToString();
            LoadControlData();
        }
        private void InitializeScreen()
        {
        }
        private void EnsureScriptManager()
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                scriptManager.EnablePartialRendering = true;

                if (Page.Form != null)
                {
                    Page.Form.Controls.AddAt(0, scriptManager);
                }
            }
        }
        private void EnsureUpdatePanelFixups()
        {
            if (this.Page.Form != null)
            {
                String formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if (formOnSubmitAtt == "return _spFormOnSubmitWrapper ();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper = true;", true);
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();
            // Page.EnableEventValidation = false;

            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            sapBOMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMService>();
            IPFService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            extMgfService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();

            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        private void LoadControlData()
        {
            try
            {

                Utilities.BindDropDownItems(ddlFilmSubstrate, GlobalConstants.LIST_FilmSubstrate, webUrl);
                if (PackagingItemId > 0)
                {
                    hdnPackagingItemId.Value = PackagingItemId.ToString();
                    //hdnShowRevPrinterSupplierField.Value = packagingItemService.GetReviewPrinterSupplierProcDets(iItemId).ToLower();
                    dvMain.Visible = true;

                    var packagingItem = packagingItemService.GetPackagingItemByPackagingId(PackagingItemId);
                    var ipfItem = IPFService.GetItemProposalItem(iItemId);
                    List<KeyValuePair<int, string>> PCSitems = packagingItemService.GetPurchasedSemiIDsForProject(iItemId);
                    hdnProductHierarchyLevel1.Value = ipfItem.ProductHierarchyLevel1;
                    hdnNovelyProject.Value = ipfItem.NovelyProject;
                    hdnExtMfgkickedoff.Value = "";
                    hdnCoManClassification.Value = ipfItem.CoManClassification;
                    if (ipfItem.ManufacturingLocation == "Externally Manufactured" || ipfItem.PackingLocation == "Externally Packed" || PCSitems.Count > 0)
                    {
                        hdnExtMfgkickedoff.Value = "Yes";
                    }

                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, this.drpPkgComponent, this.Page);
                    Utilities.SetDropDownValue(packagingItem.PackUnit, this.ddlPackUnit, this.Page);
                    Utilities.SetDropDownValue(packagingItem.NewExisting, this.drpNew, this.Page);
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, this.ddlFlowthrough, this.Page);
                    Utilities.SetDropDownValue(packagingItem.ReviewPrinterSupplier, this.ddlReviewPrinterSupplier, this.Page);
                    dvRevPrinterSupplier.Visible = true;
                    if (!PrinterAndSupplierVisible(packagingItem, ipfItem))
                    {
                        dvRevPrinterSupplier.Visible = false;
                    }

                    if (string.IsNullOrEmpty(packagingItem.SAPMaterialGroup))
                    {
                        string componantType = Utilities.GetLookupValue(GlobalConstants.LIST_PackagingComponentTypesLookup, packagingItem.PackagingComponent, webUrl);
                        hdnSAPMatGroup.Value = componantType;
                    }
                    else
                    {
                        hdnSAPMatGroup.Value = packagingItem.SAPMaterialGroup;
                    }
                    if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        int childCount = (from PI in AllPIs where PI.ParentID == packagingItem.Id select PI).Count();
                        if (childCount > 0)
                        {
                            this.drpPkgComponent.Enabled = false;
                            this.lblCompNote.Visible = true;
                        }
                    }
                    hdnParentID.Value = ParentID.ToString();
                    hdnTBDIndicator.Value = ipfItem.TBDIndicator;
                    txtMaterial.Text = packagingItem.MaterialNumber;
                    txtMaterialDescription.Text = packagingItem.MaterialDescription; // ?? Is this mapping correct
                    txtLikeItem.Text = packagingItem.CurrentLikeItem; // ?? Is this mapping correct
                    txtLikeDescription.Text = packagingItem.CurrentLikeItemDescription; // ?? Is this mapping correct
                    txtGraphicsBrief.Text = packagingItem.GraphicsBrief;
                    txtLikeMaterial.Text = packagingItem.CurrentLikeItemReason;
                    txtOldMaterial.Text = packagingItem.CurrentOldItem;
                    txtOldMaterialDesc.Text = packagingItem.CurrentOldItemDescription;

                    txtShelfLife.Text = packagingItem.ShelfLife;
                    Utilities.SetDropDownValue(packagingItem.TrialsCompleted, this.drpTrialsCompleted, this.Page);
                    Utilities.SetDropDownValue(packagingItem.NewFormula, this.ddlNewFormula, this.Page);

                    if (this.drpTSCountryOfOrigin.Items.FindByText(packagingItem.CountryOfOrigin) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.CountryOfOrigin, this.drpTSCountryOfOrigin, this.Page);
                    }
                    if (this.drpTSPackLocation.Items.FindByText(packagingItem.PackLocation) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.PackLocation, this.drpTSPackLocation, this.Page);
                    }
                    if (this.drpTransferLocation.Items.FindByText(packagingItem.TransferSEMIMakePackLocations) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.TransferSEMIMakePackLocations, this.drpTransferLocation, this.Page);
                    }
                    if (this.drpPCSCountryofOrigin.Items.FindByText(packagingItem.CountryOfOrigin) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.CountryOfOrigin, this.drpPCSCountryofOrigin, this.Page);
                    }
                    if (this.drpPCSPackLocation.Items.FindByText(packagingItem.PackLocation) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.PackLocation, this.drpPCSPackLocation, this.Page);
                    }
                    if (this.drpPurchasedCandyLocation.Items.FindByText(packagingItem.TransferSEMIMakePackLocations) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.TransferSEMIMakePackLocations, this.drpPurchasedCandyLocation, this.Page);
                    }

                    Utilities.SetDropDownValue(packagingItem.ComponentContainsNLEA, this.drpComponentContainsNLEA, this.Page);

                    txtTareWt.Text = packagingItem.TareWeight;
                    txtSpecificationNo.Text = packagingItem.SpecificationNo;
                    txtPackQty.Text = packagingItem.PackQuantity == "-9999" ? "" : packagingItem.PackQuantity;
                    txtLeadTimeMaterial.Text = packagingItem.LeadMaterialTime;

                    if (packagingItem.PackagingComponent.ToLower().Contains("transfer semi") || packagingItem.PackagingComponent.ToLower().Contains("purchased"))
                        txtSEMIComment.Text = packagingItem.Notes;

                    //txtPrinter.Text = packagingItem.PrinterSupplier;

                    txtFilmLength.Text = packagingItem.Length;
                    txtFilmWidth.Text = packagingItem.Width;
                    txtFilmHeight.Text = packagingItem.Height;
                    txtFilmBagFace.Text = packagingItem.BagFace;
                    txtFilmWebWidth.Text = packagingItem.WebWidth;
                    txtFilmExactCutoff.Text = packagingItem.ExactCutOff;

                    Utilities.SetDropDownValue(packagingItem.Unwind, ddlFilmUnWind, Page);
                    Utilities.SetDropDownValue(packagingItem.BackSeam, this.drpFilmBackSeam, this.Page);
                    Utilities.SetDropDownValue(packagingItem.FilmSubstrate, this.ddlFilmSubstrate, this.Page); // Pass from master page if component type is Film
                    Utilities.SetDropDownValue(packagingItem.StructureColor, this.ddlStructureColor, this.Page);
                    Utilities.SetDropDownValue(packagingItem.Structure, this.ddlFilmStructure, this.Page);
                    Utilities.SetDropDownValue(packagingItem.FilmStyle, this.ddlFilmStyle, this.Page);

                    Utilities.BindDropDownItemsWithClass(ddlFilmPrintStyle, GlobalConstants.LIST_PrintStyleLookup, SPContext.Current.Web.Url);

                    if (packagingItem.PackagingComponent.ToLower().Contains("corrugated") || packagingItem.PackagingComponent.ToLower().Contains("paperboard"))
                    {
                        Utilities.SetDropDownValue(packagingItem.CorrugatedPrintStyle, this.ddlFilmPrintStyle, this.Page);
                    }
                    else
                    {
                        Utilities.SetDropDownValue(packagingItem.FilmPrintStyle, this.ddlFilmPrintStyle, this.Page);
                    }

                    txtFilmMaxRollOD.Text = packagingItem.FilmMaxRollOD;
                    txtFilmRollID.Text = packagingItem.FilmRollID;

                    txtCadDrawingName.Text = packagingItem.CADDrawing;
                    Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);
                    if (!string.IsNullOrEmpty(packagingItem.ExternalGraphicsVendor))
                    {
                        Utilities.SetDropDownValue(packagingItem.ExternalGraphicsVendor, ddlGraphicsVendor, this.Page);
                    }

                    if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                    {
                        Utilities.SetDropDownValue(packagingItem.GraphicsChangeRequired, drpGraphicsNeeded, this.Page);
                    }
                    GetAttachments();
                    /*var files = packagingItemService.GetUploadedFiles(ProjectNumber, packagingItem.Id, GlobalConstants.DOCTYPE_Rendering);
                    if (files.Count > 0)
                    {
                        btnDeleteAttachment.Visible = true;
                        if (ancRendering != null)
                        {
                            string fileName = files[0].FileName;
                            fileName = fileName.Replace("_", " ");
                            ancRendering.Controls.Add(new LiteralControl(fileName));
                            ancRendering.HRef = files[0].FileUrl;
                            btnDeleteAttachment.CommandArgument = files[0].FileUrl;
                        }

                        btnAttachment.Visible = false;
                    }*/

                    dvMoveTS.Visible = false;
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_OBMFirstReview.ToLower()) ||
                            string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_OBMSecondReview.ToLower()))
                    {
                        dvMoveTS.Visible = true;
                        List<KeyValuePair<int, string>> allItems = new List<KeyValuePair<int, string>>();
                        TSIds = packagingItemService.GetTransferSemiIDsForProject(iItemId);
                        PSIds = packagingItemService.GetPurchasedSemiIDsForProject(iItemId);
                        if (TSIds != null)
                        {
                            allItems.AddRange(TSIds);
                        }
                        if (PSIds != null)
                        {
                            allItems.AddRange(PSIds);
                        }
                        List<int> idsOnly = new List<int>();
                        foreach (KeyValuePair<int, string> items in allItems)
                        {
                            idsOnly.Add(items.Key);
                        }

                        if (allItems != null && allItems.Count > 0)
                        {
                            List<int> filteredParents = filterParents(idsOnly, CompassItemId, packagingItem.Id);
                            foreach (KeyValuePair<int, string> IDs in allItems)
                            {
                                int approvedId = filteredParents.IndexOf(IDs.Key);
                                if (IDs.Key != packagingItem.Id && approvedId > -1)
                                {
                                    ddlMoveTS.Items.Add(new ListItem(IDs.Value, IDs.Key.ToString()));
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(packagingItem.PrinterSupplier))
                    {
                        try
                        {
                            ddlPrinter.ClearSelection();
                            ddlPrinter.Items.FindByText(packagingItem.PrinterSupplier).Selected = true;
                        }
                        catch (Exception ex)
                        {
                            ddlPrinter.Items.Add(packagingItem.PrinterSupplier);
                            ddlPrinter.Items.FindByText(packagingItem.PrinterSupplier).Selected = true;
                            ddlPrinter.BackColor = System.Drawing.Color.Pink;
                            this.lblPrinterError.Text = "'" + packagingItem.PrinterSupplier + "' is not a valid Printer/Supplier! Please select or request a new one.";
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "LoadFormData");
            }
        }

        private bool PrinterAndSupplierVisible(PackagingItem packagingItem, ItemProposalItem ipfItem)
        {
            if (Utilities.GetCurrentPageName() != GlobalConstants.PAGE_SAPInitialItemSetup)
            {
                return true;
            }

            bool visible = true;
            /*
            "PrinterAndSupplierVisible Only if:
            i). Project is Not Coman
            ii.) and project is Not Novelty
            iii). and External Manufacturing Form is kicked off
            iv). and the Procurement Type on the External Manufacturing Form is not ""External Turnkey FG""
            v.) and component type is not ""Transfer Semi""
            vi.) and component type is not ""Purchased Candy Semi""
            vii.) and component type is not ""Candy Semi""

            If Displayed, pull in corresponding Yes/ No value selected for the component from the External Manufacturing form ""Review Printer / Supplier"" field and lock the value"
            */
            if (ipfItem.ProductHierarchyLevel1 == GlobalConstants.PRODUCT_HIERARCHY1_CoMan ||
                visible && ipfItem.NovelyProject == GlobalConstants.CONST_Yes ||
                visible && ipfItem.CoManClassification == "External Turnkey FG" ||
                visible && (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi ||
                packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_CandySemi ||
                packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi))
            {
                visible = false;
            }

            if (visible)
            {
                var extMfgItem = extMgfService.GetExternalManufacturingApprovalItem(iItemId);
                DateTime dateValue;
                if (!(DateTime.TryParse(extMfgItem.StartDate, out dateValue)))
                {
                    visible = false;
                }
            }
            return visible;
        }
        private PackagingItem ConstructFormData()
        {
            var packagingItem = new PackagingItem();

            try
            {
                // If this is an existing item, get the current values
                if (PackagingItemId > 0)
                    packagingItem = packagingItemService.GetPackagingItemByPackagingId(PackagingItemId);
                CompassItemId = iItemId;
                packagingItem.Id = PackagingItemId;
                packagingItem.CompassListItemId = iItemId.ToString();
                string packcomp = this.drpPkgComponent.SelectedItem.Text;
                packagingItem.PackagingComponent = packcomp;
                packagingItem.PackUnit = this.ddlPackUnit.SelectedItem.Text;
                packagingItem.NewExisting = drpNew.SelectedItem.Text;
                packagingItem.MaterialNumber = txtMaterial.Text;
                packagingItem.MaterialDescription = txtMaterialDescription.Text;
                packagingItem.CurrentLikeItem = txtLikeItem.Text;
                packagingItem.CurrentLikeItemDescription = txtLikeDescription.Text;
                packagingItem.CurrentOldItem = txtOldMaterial.Text;
                packagingItem.CurrentOldItemDescription = txtOldMaterialDesc.Text;
                packagingItem.TrialsCompleted = this.drpTrialsCompleted.SelectedItem.Text;
                packagingItem.NewFormula = this.ddlNewFormula.SelectedItem.Text;
                packagingItem.ShelfLife = this.txtShelfLife.Text;
                if (packcomp == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    packagingItem.CountryOfOrigin = this.drpPCSCountryofOrigin.SelectedItem.Text;
                    packagingItem.PackLocation = this.drpPCSPackLocation.SelectedItem.Text;
                    packagingItem.TransferSEMIMakePackLocations = this.drpPurchasedCandyLocation.SelectedItem.Text;
                }
                else
                {
                    packagingItem.CountryOfOrigin = this.drpTSCountryOfOrigin.SelectedItem.Text;
                    packagingItem.PackLocation = this.drpTSPackLocation.SelectedItem.Text;
                    packagingItem.TransferSEMIMakePackLocations = this.drpTransferLocation.SelectedItem.Text;
                }
                packagingItem.TareWeight = txtTareWt.Text;

                if (this.dvSpecification.Visible) packagingItem.SpecificationNo = txtSpecificationNo.Text;

                packagingItem.PackQuantity = txtPackQty.Text;
                packagingItem.LeadMaterialTime = txtLeadTimeMaterial.Text;
                if (packagingItem.PackagingComponent.ToLower().Contains("transfer semi") || packagingItem.PackagingComponent.ToLower().Contains("purchased candy"))
                    packagingItem.Notes = txtSEMIComment.Text;

                packagingItem.Length = txtFilmLength.Text;
                packagingItem.Width = txtFilmWidth.Text;
                packagingItem.Height = txtFilmHeight.Text;
                packagingItem.BackSeam = drpFilmBackSeam.SelectedItem.Text;
                packagingItem.WebWidth = txtFilmWebWidth.Text;
                packagingItem.ExactCutOff = txtFilmExactCutoff.Text;
                packagingItem.Unwind = ddlFilmUnWind.SelectedItem.Text;
                packagingItem.BagFace = txtFilmBagFace.Text;
                packagingItem.FilmSubstrate = ddlFilmSubstrate.SelectedItem.Text;
                packagingItem.StructureColor = this.ddlStructureColor.SelectedItem.Text;
                packagingItem.Structure = this.ddlFilmStructure.SelectedItem.Text;

                if (packagingItem.PackagingComponent.ToLower().Contains("film"))
                {
                    packagingItem.FilmPrintStyle = this.ddlFilmPrintStyle.SelectedItem.Text;
                    packagingItem.CorrugatedPrintStyle = "";
                }
                else if (packagingItem.PackagingComponent.ToLower().Contains("corrugated") || packagingItem.PackagingComponent.ToLower().Contains("paperboard"))
                {
                    packagingItem.CorrugatedPrintStyle = this.ddlFilmPrintStyle.SelectedItem.Text;
                    packagingItem.FilmPrintStyle = "";
                }

                packagingItem.FilmMaxRollOD = txtFilmMaxRollOD.Text;
                packagingItem.FilmRollID = txtFilmRollID.Text;
                packagingItem.PrinterSupplier = ddlPrinter.SelectedItem.Text;
                packagingItem.CADDrawing = txtCadDrawingName.Text;
                packagingItem.FilmStyle = ddlFilmStyle.SelectedItem.Text;
                if (PackagingItemId <= 0)
                {
                    string componantType = Utilities.GetLookupValue(GlobalConstants.LIST_PackagingComponentTypesLookup, packagingItem.PackagingComponent, webUrl);
                    packagingItem.SAPMaterialGroup = componantType;
                }
                else
                {
                    packagingItem.SAPMaterialGroup = hdnSAPMatGroup.Value;
                }

                packagingItem.ParentID = string.IsNullOrEmpty(hdnParentID.Value) ? 0 : Convert.ToInt32(hdnParentID.Value);

                packagingItem.GraphicsBrief = txtGraphicsBrief.Text;
                packagingItem.GraphicsChangeRequired = drpGraphicsNeeded.SelectedItem.Text;
                packagingItem.ExternalGraphicsVendor = ddlGraphicsVendor.SelectedItem.Text;
                packagingItem.ComponentContainsNLEA = drpComponentContainsNLEA.SelectedItem.Text;
                packagingItem.CurrentLikeItemReason = txtLikeMaterial.Text;
                packagingItem.Flowthrough = ddlFlowthrough.SelectedItem.Text;
                packagingItem.ReviewPrinterSupplier = ddlReviewPrinterSupplier.SelectedItem.Text;
                string movedPackType = ddlMoveTS.SelectedItem.Text;
                int moveId = Convert.ToInt32(ddlMoveTS.SelectedItem.Value);
                if (moveId != -1)
                {
                    packagingItem.ParentID = moveId;
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "ConstructFormData");
            }

            return packagingItem;
        }

        #region Attachment Methods
        protected void lnkDeleteAttachment_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");
            hdnPageState.Value = "PE";
        }
        private void GetAttachments()
        {
            //var projectNo = utilityService.GetProjectNumberFromItemId(CompassItemId, webUrl);
            var files = packagingItemService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_CADDrawing);

            if (files.Count > 0)
            {
                rptCADDrawing.Visible = true;
                rptCADDrawing.DataSource = files;
                rptCADDrawing.DataBind();
            }
            else
            {
                rptCADDrawing.Visible = false;
            }
            if (files.Count <= 3)
            {
                btnUploadCAD.Visible = true;
            }
            else { btnUploadCAD.Visible = false; }

            files.Clear();
            files = packagingItemService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Rendering);
            if (files.Count > 0)
            {
                btnReference.Visible = false;
                rptVisualReference.Visible = true;
                rptVisualReference.DataSource = files;
                rptVisualReference.DataBind();
            }
            else
            {
                rptVisualReference.Visible = false;
                btnReference.Visible = true;
            }
        }
        private void BindAttachments(HtmlAnchor anc, FileUpload fld, HtmlAnchor anc1, FileUpload fld1, LinkButton lnk, LinkButton lnk1)
        {
            var projectNo = utilityService.GetProjectNumberFromItemId(CompassItemId, webUrl);
            var files = packagingItemService.GetUploadedFiles(projectNo, PackagingItemId, GlobalConstants.DOCTYPE_CADDrawing);
            lnk.Visible = lnk1.Visible = false;
            if (files.Count > 0)
            {
                anc.InnerText = files[0].FileName;
                anc.HRef = files[0].FileUrl;
                fld.Visible = false;
                lnk.Visible = true;
                lnk.CommandName = files[0].FileUrl;

                if (files.Count > 1)
                {
                    anc1.InnerText = files[1].FileName;
                    anc1.HRef = files[1].FileUrl;
                    fld1.Visible = false;
                    lnk1.Visible = true;
                    lnk1.CommandName = files[1].FileUrl;
                }
            }
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }
        #endregion

        protected void lbRequestNewSubstrate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtNewSubstrate.Text))
                {
                    this.lblSubstrateError.Text = "";
                    notificationService.SendHelpDeskLookupRequest(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Substrate", txtNewSubstrate.Text);
                    this.lblSubstrateRequestSent.Visible = true;
                    this.txtNewSubstrate.Visible = false;
                    btnRequestNewSubstrate.Visible = false;
                }
                else
                {
                    this.lblSubstrateError.Text = "Please enter a Substrate!";
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Request New Substrate: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "lbRequestNewSubstrate_Click", "RequestNewSubstrate");
            }
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string val = hdnRequiredCheck.Value;
                PackagingItemId = string.IsNullOrEmpty(hdnPackagingItemId.Value) ? 0 : Convert.ToInt32(hdnPackagingItemId.Value);
                SaveData();
                dvMain.Visible = false;
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
                //   ScriptManager.RegisterStartupScript(this.Page, UpdatePanelBOMPE.GetType(), "", "showSelectButton();", true);
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "btnSubmit_Click");
            }
        }
        private void SaveData()
        {
            var item = ConstructFormData();
            if (PackagingItemId > 0)
            {
                packagingItemService.UpdatePackagingItem(item);
            }
            else
            {
                item.Id = packagingItemService.InsertPackagingItem(item, CompassItemId);
            }
        }


        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");
                hdnPageState.Value = "PE";
                if (!string.IsNullOrEmpty(txtNewPrinter.Text))
                {
                    this.lblPrinterError.Text = "";
                    notificationService.SendHelpDeskLookupRequest(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Printer/Supplier", txtNewPrinter.Text);
                    this.lblPrinterRequestSent.Visible = true;
                    this.txtNewPrinter.Visible = false;
                    btnAdd.Visible = false;
                }
                else
                {
                    this.lblPrinterError.Text = "Please enter a Printer/Supplier!";
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Request Printer/Supplier: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "btnAdd_Click", "Request Printer/Supplier");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string newlyAdded = hdnPackagingComponent.Value;
            if (string.IsNullOrEmpty(newlyAdded))
            {
                packagingItemService.DeletePackagingItem(Convert.ToInt32(hdnPackagingItemId.Value));
            }
            Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
        }

        protected void btnSaveValidate_Click(object sender, EventArgs e)
        {
            try
            {
                PackagingItemId = string.IsNullOrEmpty(hdnPackagingItemId.Value) ? 0 : Convert.ToInt32(hdnPackagingItemId.Value);
                hdnRequiredCheck.Value = GlobalConstants.DETAILFORMSTATUS_Completed;
                SaveData();
                dvMain.Visible = false;
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), ProjectNumber), false);
                //   ScriptManager.RegisterStartupScript(this.Page, UpdatePanelBOMPE.GetType(), "", "showSelectButton();", true);
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "btnSubmit_Click");
            }
        }

        private void hideItemsforSAPBOMPage()
        {
            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()))
            {
                var phTop = (PlaceHolder)this.Parent.FindControl("phTopBOMSETUP");
                phTop.Visible = false;

                var phBottom = (PlaceHolder)this.Parent.FindControl("phBottomBOMSETUP");
                phBottom.Visible = false;
            }
        }
        private void hideSpecificationFieldForNonPEPages()
        {
            if (!(string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE.ToLower()) ||
                                string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_PE2.ToLower())))
            {
                this.dvSpecification.Visible = false;
            }
        }
        public List<int> filterParents(List<int> IDs, int compassItemID, int movingId)
        {
            List<int> idsHolder = IDs;
            List<KeyValuePair<int, int>> AllItems = new List<KeyValuePair<int, int>>();

            foreach (PackagingItem item in AllPIs)
            {
                AllItems.Add(new KeyValuePair<int, int>(item.Id, item.ParentID));
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
    }
}
