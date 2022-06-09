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
    public partial class ucBOMEditable_New : UserControl
    {
        #region Member Variables
        private IBOMSetupService BOMSetupService;
        private IItemProposalService IPFService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private IExternalManufacturingService extMgfService;
        private IPackagingItemService packagingItemService;

        private string webUrl;
        private int iItemId = 0;
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
        public int PackagingItemId { get; set; }
        public int CompassListItemId { get; set; }
        public string ParentID { get; set; }
        public string ParentMaterialNumber { get; set; }
        public string ParentType { get; set; }
        public bool IsNew { get; set; }
        public List<BOMSetupItem> AllPIs { get; set; }
        public string PackagingComponent { get; set; }
        public string firstLoad { get; set; }
        public string ProjectType { get; set; }
        public string ProjectTypeSubCategory { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            hdnPackagingItemId.Value = PackagingItemId.ToString();
            webUrl = SPContext.Current.Web.Url;
            if (firstLoad == "true")
            {
                try
                {
                    using (SPSite site = new SPSite(webUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            Utilities.BindDropDownItems(drpPkgComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, web.Url);
                            Utilities.BindDropDownItems(ddlPrinter, GlobalConstants.LIST_PrinterSupplierLookup, web.Url);
                            Utilities.BindDropDownItems(ddlPackUnit, GlobalConstants.LIST_PackUnitLookup, web.Url);
                            Utilities.BindDropDownItemsById(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);
                            Utilities.BindDropDownItemsById(ddlPurchasedIntoLocation, GlobalConstants.LIST_CompPurchasedIntoLocationsLookup, webUrl);

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
                if (AllPIs == null || AllPIs.Count == 0)
                {
                    AllPIs = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);
                }
                this.hiddenItemId.Value = iItemId.ToString();
                this.hdnIsNew.Value = IsNew ? "Yes" : "No";
                hdnPackagingComponent.Value = IsNew ? "" : PackagingComponent;
                hdnParentID.Value = ParentID.ToString();
                hdnParentType.Value = ParentType;
                hdnProjectType.Value = ProjectType;
                hdnProjectTypeSubCategory.Value = ProjectTypeSubCategory;
                LoadControlData();

            }
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

            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            IPFService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            extMgfService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        #region Data Transfer Methods
        private void LoadControlData()
        {
            try
            {
                if (PackagingItemId > 0)
                {
                    hdnPackagingItemId.Value = PackagingItemId.ToString();
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

                    dvMain.Visible = true;

                    var packagingItem = (from PI in AllPIs where PI.Id == PackagingItemId select PI).FirstOrDefault();

                    if (packagingItem.ParentID != 0)
                    {
                        var parentComoponent = BOMSetupService.GetBOMSetupItemByComponentId(packagingItem.ParentID);
                        hdnParentType.Value = parentComoponent.PackagingComponent;
                    }

                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, this.drpPkgComponent, this.Page);
                    Utilities.SetDropDownValue(packagingItem.PackUnit, this.ddlPackUnit, this.Page);
                    Utilities.SetDropDownValue(packagingItem.NewExisting, this.drpNew, this.Page);
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, this.ddlFlowthrough, this.Page);
                    Utilities.SetDropDownValue(packagingItem.PurchasedIntoLocation, this.ddlPurchasedIntoLocation, this.Page);

                    Utilities.SetDropDownValue(packagingItem.ReviewPrinterSupplier, this.ddlReviewPrinterSupplier, this.Page);
                    Utilities.BindDropDownItemsWithClass(ddlFilmPrintStyle, GlobalConstants.LIST_PrintStyleLookup, SPContext.Current.Web.Url);
                    if (packagingItem.PackagingComponent.ToLower().Contains("corrugated") || packagingItem.PackagingComponent.ToLower().Contains("paperboard"))
                    {
                        Utilities.SetDropDownValue(packagingItem.CorrugatedPrintStyle, this.ddlFilmPrintStyle, this.Page);
                    }
                    else
                    {
                        Utilities.SetDropDownValue(packagingItem.FilmPrintStyle, this.ddlFilmPrintStyle, this.Page);
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
                    hdnLOB.Value = ipfItem.ProductHierarchyLevel1;
                    txtMaterialDescription.Text = packagingItem.MaterialDescription; // ?? Is this mapping correct
                    txtLikeItem.Text = packagingItem.CurrentLikeItem; // ?? Is this mapping correct
                    txtLikeDescription.Text = packagingItem.CurrentLikeItemDescription; // ?? Is this mapping correct
                    txtGraphicsBrief.Text = packagingItem.GraphicsBrief;
                    txtLikeMaterial.Text = packagingItem.CurrentLikeItemReason;
                    txtOldMaterial.Text = packagingItem.CurrentOldItem;
                    txtOldMaterialDesc.Text = packagingItem.CurrentOldItemDescription;
                    #region Transfer Semi Barcode Generation
                    txt13DigitCode.Text = packagingItem.ThirteenDigitCode;
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()) || string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_OBMSecondReview.ToLower()))
                    {
                        dvSAPDescAbrev.Visible = true;
                        Utilities.BindDropDownPassColumnNames(ddlSAPDescAbrev, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl, "Title", "SAPDescription");
                        Utilities.SetDropDownValueById(packagingItem.SAPDescAbbrev, this.ddlSAPDescAbrev, this.Page);
                        bool TransferSemiBarcodeGenerationVisibile = false;
                        int parentCompId = string.IsNullOrEmpty(ParentID) ? 0 : Convert.ToInt32(ParentID);
                        if (parentCompId != 0)
                        {
                            var parentItem = packagingItemService.GetPackagingItemByPackagingId(parentCompId);
                            if (parentItem != null)
                            {
                                if (parentItem.PackagingComponent.ToLower().Contains("transfer") && (parentItem.PackLocation.Contains("FQ22") || parentItem.PackLocation.Contains("FQ25")))
                                {
                                    ParentMaterialNumber = parentItem.MaterialNumber;
                                    TransferSemiBarcodeGenerationVisibile = true;
                                    hdnTSBarcodeGenerationVisibility.Value = "Yes";
                                    hdn13DigitCode.Value = string.Concat("1000000", ParentMaterialNumber);
                                }
                            }
                        }

                        if (packagingItem.PackagingComponent.ToLower().Contains("corrugated") && packagingItem.NewExisting.ToLower() == "new" && TransferSemiBarcodeGenerationVisibile)
                        {
                            txt13DigitCode.Text = string.Concat("1000000", ParentMaterialNumber);
                        }
                    }

                    txt14DigitBarcode.Text = packagingItem.FourteenDigitBarcode;
                    #endregion
                    txtShelfLife.Text = packagingItem.ShelfLife;
                    Utilities.SetDropDownValue(packagingItem.TrialsCompleted, this.drpTrialsCompleted, this.Page);
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPCompleteItemSetup.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupSAP.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()))
                    {
                        divTrialsCompleted.Visible = false;
                    }
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

                    txtSpecificationNo.Text = packagingItem.SpecificationNo;
                    txtPackQty.Text = packagingItem.PackQuantity == "-9999" ? "" : packagingItem.PackQuantity;
                    txtLeadTimeMaterial.Text = packagingItem.LeadMaterialTime;

                    if (packagingItem.PackagingComponent.ToLower().Contains("transfer semi") || packagingItem.PackagingComponent.ToLower().Contains("purchased"))
                        txtSEMIComment.Text = packagingItem.Notes;

                    Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);
                    if (!string.IsNullOrEmpty(packagingItem.ExternalGraphicsVendor))
                    {
                        Utilities.SetDropDownValue(packagingItem.ExternalGraphicsVendor, ddlGraphicsVendor, this.Page);
                    }

                    if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                    {
                        Utilities.SetDropDownValue(packagingItem.GraphicsChangeRequired, drpGraphicsNeeded, this.Page);
                    }
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE2.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE3.ToLower()))
                    {
                        drpGraphicsNeeded.Attributes.Add("onchange", "GraphicsCheck(this);");
                    }
                    else
                    {
                        drpGraphicsNeeded.Attributes.Add("onchange", "GraphicsCheckLoadSpec(this);");
                    }
                    Utilities.SetDropDownValue(packagingItem.IsAllProcInfoCorrect, this.ddlIsAllProcInfoCorrect, this.Page);
                    txtWhatProcInfoHasChanged.Text = packagingItem.WhatProcInfoHasChanged;
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

                    Utilities.SetDropDownValue(packagingItem.NewPrinterSupplierForLocation, this.ddlNewPrinterSupplierForLocation, this.Page);

                    if (packagingItem.DielineURL != "")
                    {
                        generatedLinkEdit.HRef = packagingItem.DielineURL;
                        string title = string.IsNullOrEmpty(PackagingComponent) ? "Finished Good" : PackagingComponent;
                        title = title + ": " + packagingItem.MaterialNumber + ": Dieline";
                        generatedLinkEdit.InnerText = title;
                        txtDielineLinkEdit.Text = packagingItem.DielineURL;
                        generatedLinkEdit.Attributes.Remove("class");
                    }
                    try
                    {
                        Utilities.BindDropDownItems(ddlPHL1, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                        // Load the level 1 list
                        Utilities.SetDropDownValue(packagingItem.PHL1, ddlPHL1, this.Page);
                        ddlProductHierarchyLevel1_SelectedIndexChanged(ddlPHL1, null);

                        Utilities.SetDropDownValue(packagingItem.PHL2, ddlPHL2, this.Page);
                        // Load the level 2 list
                        ddlProductHierarchyLevel2_SelectedIndexChanged(ddlPHL2, null);

                        // Load Brand List
                        Utilities.SetDropDownValue(packagingItem.Brand, ddlBrand, this.Page);
                        ddlBrand_SelectedIndexChanged(ddlBrand, null);
                    }
                    catch (Exception e)
                    {
                        LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Hierarchy Error: " + e.Message);
                        exceptionService.Handle(LogCategory.CriticalError, e, "Hierarchy Error", "Hierarchy Error");
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "LoadFormData");
            }
        }
        private BOMSetupItem ConstructFormData()
        {
            var bomSetupItem = new BOMSetupItem();

            try
            {
                // If this is an existing item, get the current values
                if (PackagingItemId > 0 && (AllPIs == null || AllPIs.Count == 0))
                    bomSetupItem = BOMSetupService.GetBOMSetupItemByComponentId(PackagingItemId);
                bomSetupItem.Id = PackagingItemId;
                bomSetupItem.CompassListItemId = CompassListItemId;
                string packcomp = this.drpPkgComponent.SelectedItem.Text;
                bomSetupItem.PackagingComponent = packcomp;
                bomSetupItem.PackUnit = this.ddlPackUnit.SelectedItem.Text;
                bomSetupItem.NewExisting = drpNew.SelectedItem.Text;
                bomSetupItem.MaterialNumber = txtMaterial.Text;
                bomSetupItem.MaterialDescription = txtMaterialDescription.Text;
                bomSetupItem.CurrentLikeItem = txtLikeItem.Text;
                bomSetupItem.CurrentLikeItemDescription = txtLikeDescription.Text;
                bomSetupItem.CurrentOldItem = txtOldMaterial.Text;
                bomSetupItem.CurrentOldItemDescription = txtOldMaterialDesc.Text;
                bomSetupItem.TrialsCompleted = this.drpTrialsCompleted.SelectedItem.Text;
                bomSetupItem.NewFormula = this.ddlNewFormula.SelectedItem.Text;
                bomSetupItem.ShelfLife = this.txtShelfLife.Text;
                #region Transfer Semi Barcode Generation
                bomSetupItem.ThirteenDigitCode = txt13DigitCode.Text;
                bomSetupItem.FourteenDigitBarcode = txt14DigitBarcode.Text;
                #endregion
                if (packcomp == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    bomSetupItem.CountryOfOrigin = this.drpPCSCountryofOrigin.SelectedItem.Text;
                    bomSetupItem.PackLocation = this.drpPCSPackLocation.SelectedItem.Text;
                    bomSetupItem.TransferSEMIMakePackLocations = this.drpPurchasedCandyLocation.SelectedItem.Text;
                }
                else
                {
                    bomSetupItem.CountryOfOrigin = this.drpTSCountryOfOrigin.SelectedItem.Text;
                    bomSetupItem.PackLocation = this.drpTSPackLocation.SelectedItem.Text;
                    bomSetupItem.TransferSEMIMakePackLocations = this.drpTransferLocation.SelectedItem.Text;
                }

                bomSetupItem.SpecificationNo = txtSpecificationNo.Text;
                bomSetupItem.PurchasedIntoLocation = this.ddlPurchasedIntoLocation.SelectedItem.Text;

                bomSetupItem.PackQuantity = txtPackQty.Text;
                bomSetupItem.LeadMaterialTime = txtLeadTimeMaterial.Text;
                if (bomSetupItem.PackagingComponent.ToLower().Contains("transfer semi") || bomSetupItem.PackagingComponent.ToLower().Contains("purchased candy"))
                    bomSetupItem.Notes = txtSEMIComment.Text;
                bomSetupItem.PrinterSupplier = ddlPrinter.SelectedItem.Text;

                bomSetupItem.NewPrinterSupplierForLocation = ddlNewPrinterSupplierForLocation.SelectedItem.Text;

                if (PackagingItemId <= 0)
                {
                    string componantType = Utilities.GetLookupValue(GlobalConstants.LIST_PackagingComponentTypesLookup, bomSetupItem.PackagingComponent, webUrl);
                    bomSetupItem.SAPMaterialGroup = componantType;
                }
                else
                {
                    bomSetupItem.SAPMaterialGroup = hdnSAPMatGroup.Value;
                }

                bomSetupItem.ParentID = string.IsNullOrEmpty(hdnParentID.Value) ? 0 : Convert.ToInt32(hdnParentID.Value);

                bomSetupItem.GraphicsBrief = txtGraphicsBrief.Text;
                bomSetupItem.GraphicsChangeRequired = drpGraphicsNeeded.SelectedItem.Text;
                bomSetupItem.ExternalGraphicsVendor = ddlGraphicsVendor.SelectedItem.Text;
                bomSetupItem.ComponentContainsNLEA = drpComponentContainsNLEA.SelectedItem.Text;
                bomSetupItem.CurrentLikeItemReason = txtLikeMaterial.Text;
                bomSetupItem.Flowthrough = ddlFlowthrough.SelectedItem.Text;
                bomSetupItem.ReviewPrinterSupplier = ddlReviewPrinterSupplier.SelectedItem.Text;
                bomSetupItem.IsAllProcInfoCorrect = ddlIsAllProcInfoCorrect.SelectedItem.Text;
                bomSetupItem.WhatProcInfoHasChanged = txtWhatProcInfoHasChanged.Text;
                if (bomSetupItem.PackagingComponent.ToLower().Contains("film"))
                {
                    bomSetupItem.FilmPrintStyle = ddlFilmPrintStyle.SelectedItem.Text;
                    bomSetupItem.CorrugatedPrintStyle = "";
                }
                else if (bomSetupItem.PackagingComponent.ToLower().Contains("corrugated") || bomSetupItem.PackagingComponent.ToLower().Contains("paperboard"))
                {
                    bomSetupItem.CorrugatedPrintStyle = ddlFilmPrintStyle.SelectedItem.Text;
                    bomSetupItem.FilmPrintStyle = "";
                }
                if (bomSetupItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || bomSetupItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                {
                    bomSetupItem.PHL1 = ddlPHL1.SelectedItem.Text;
                    bomSetupItem.PHL2 = ddlPHL2.SelectedItem.Text;
                    bomSetupItem.Brand = ddlBrand.SelectedItem.Text;
                    bomSetupItem.ProfitCenter = txtProfitCenterUC.Text;
                }
                string movedPackType = ddlMoveTS.SelectedItem.Text;
                int moveId = Convert.ToInt32(ddlMoveTS.SelectedItem.Value);
                if (moveId != -1)
                {
                    bomSetupItem.ParentID = moveId;
                }
                //New Fields
                bomSetupItem.DielineURL = txtDielineLinkEdit.Text;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "ConstructFormData");
            }

            return bomSetupItem;
        }
        #endregion
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
            var files = BOMSetupService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Dieline);

            if (files.Count > 0)
            {
                rptDieline.Visible = true;
                rptDieline.DataSource = files;
                rptDieline.DataBind();
            }
            else
            {
                rptDieline.Visible = false;
            }

            #region Approved Graphics Asset
            var ApprovedGraphicsAssets = BOMSetupService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_ApprovedGraphicsAsset);

            if (ApprovedGraphicsAssets.Count > 0)
            {
                btnApprovedGraphicsAsset.Visible = false;
                rptApprovedGraphicsAsset.Visible = true;
                rptApprovedGraphicsAsset.DataSource = ApprovedGraphicsAssets;
                rptApprovedGraphicsAsset.DataBind();
            }
            else
            {
                rptApprovedGraphicsAsset.Visible = false;
                btnApprovedGraphicsAsset.Visible = true;
            }
            #endregion
        }
        protected void lnkDeleteApprovedGraphicsAsset_Click(object sender, EventArgs e)
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
        private void BindAttachments(HtmlAnchor anc, FileUpload fld, HtmlAnchor anc1, FileUpload fld1, LinkButton lnk, LinkButton lnk1)
        {
            var files = BOMSetupService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Dieline);
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
        #region Button Methods
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
            if (string.Equals(hdnIsNew.Value, "Yes"))
            {
                BOMSetupService.DeleteBOMSetupItem(Convert.ToInt32(hdnPackagingItemId.Value));
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
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "btnSubmit_Click");
            }
        }
        #endregion
        #region Private Methods
        private void SaveData()
        {
            var item = ConstructFormData();
            if (PackagingItemId > 0)
            {
                BOMSetupService.UpdateBOMSetupItem(item);
            }
            else
            {
                item.Id = BOMSetupService.InsertBOMSetupItem(item);
            }
        }
        public List<int> filterParents(List<int> IDs, int movingId)
        {
            List<int> idsHolder = IDs;
            List<KeyValuePair<int, int>> AllItems = new List<KeyValuePair<int, int>>();

            foreach (BOMSetupItem item in AllPIs)
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
        #endregion
        protected void ddlProductHierarchyLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlProductHierarhcyLevel1 = (DropDownList)sender;
            string productHierarhcyLevel1 = ddlProductHierarhcyLevel1.SelectedItem.Text;
            Control repeaterItem = (Control)ddlProductHierarhcyLevel1.Parent;
            DropDownList ddlBrand_Material = ((DropDownList)repeaterItem.FindControl("ddlBrand"));
            DropDownList ddlProductHierarchyLevel2 = ((DropDownList)repeaterItem.FindControl("ddlPHL2"));
            TextBox txtProfitCenter = (TextBox)repeaterItem.FindControl("txtProfitCenterUC");
            string level2 = Utilities.GetLookupValue(GlobalConstants.LIST_ProductHierarchyLevel1Lookup, productHierarhcyLevel1, webUrl);

            if ((!string.IsNullOrEmpty(level2)) && (!string.Equals(level2, "Select...")))
            {
                Utilities.BindDropDownItemsByValue(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, level2, webUrl);
                ddlProductHierarchyLevel2.SelectedIndex = -1;
            }
            else
            {
                ddlProductHierarchyLevel2.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                ddlProductHierarchyLevel2.Items.Add(li);
            }
            ddlBrand_Material.Items.Clear();
            ListItem li2 = new ListItem();
            li2.Text = "Select...";
            li2.Value = "-1";
            ddlBrand_Material.Items.Add(li2);
            txtProfitCenter.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ReloadProductHierarchyLevel2", "BOMNewCondition();", true);
        }
        protected void ddlProductHierarchyLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlProductHierarchyLevel2 = (DropDownList)sender;
            String productHierarhcyLevel2 = ddlProductHierarchyLevel2.SelectedItem.Text;

            Control repeaterItem = (Control)ddlProductHierarchyLevel2.Parent;
            DropDownList productHierarhcyLevel1 = ((DropDownList)repeaterItem.FindControl("ddlPHL1"));
            DropDownList ddlBrand_Material = ((DropDownList)repeaterItem.FindControl("ddlBrand"));
            TextBox txtProfitCenter = (TextBox)repeaterItem.FindControl("txtProfitCenterUC");

            if ((!string.IsNullOrEmpty(productHierarhcyLevel2)) && (!string.Equals(productHierarhcyLevel2, "Select...")))
            {
                Utilities.BindDropDownItemsByValueAndColumn(ddlBrand_Material, GlobalConstants.LIST_MaterialGroup1Lookup, "ParentPHL2", productHierarhcyLevel2, webUrl);
                ddlBrand_Material.SelectedIndex = -1;
            }
            else
            {
                ddlBrand_Material.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                ddlBrand_Material.Items.Add(li);

            }
            txtProfitCenter.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ReloadProductHierarchyLevel2", "BOMNewCondition();", true);
        }
        protected void ddlBrand_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlBrand_Material = (DropDownList)sender;
            Control repeaterItem = (Control)ddlBrand_Material.Parent;
            DropDownList productHierarhcyLevel1 = ((DropDownList)repeaterItem.FindControl("ddlPHL1"));
            DropDownList ddlProductHierarchyLevel2 = ((DropDownList)repeaterItem.FindControl("ddlPHL2"));
            TextBox txtProfitCenter = (TextBox)repeaterItem.FindControl("txtProfitCenterUC");
            string profitCenter = Utilities.GetLookupDetailsByValueAndColumn("ProfitCenter", GlobalConstants.LIST_MaterialGroup1Lookup, "Title", ddlBrand_Material.SelectedItem.Text, "ParentPHL2", ddlProductHierarchyLevel2.SelectedItem.Text, webUrl);
            txtProfitCenter.Text = profitCenter;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ReloadProductHierarchyLevel2", "BOMNewCondition();", true);
        }
    }
}
