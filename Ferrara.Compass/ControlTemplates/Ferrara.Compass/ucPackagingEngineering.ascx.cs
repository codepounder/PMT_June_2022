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
    public partial class ucPackagingEngineering : UserControl
    {
        #region Member Variables
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private ISAPBOMService sapBOMService;
        private ISAPMaterialMasterService sapMMService;
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

        public int PackagingItemId { get; set; }
        public int CompassItemId { get; set; }
        public string ComponentType { get; set; }
        public string CompassListId { get; set; }
        public string FilmSubstrate { get; set; }
        public string ParentID { get; set; }
        public string PackagingType { get; set; }
        public bool IsNew { get; set; }

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

                        Utilities.BindDropDownItems(drpTSMakeLocation, GlobalConstants.LIST_ManufacturingLocationsLookup, web.Url);
                        Utilities.BindDropDownItems(drpTSCountryOfOrigin, GlobalConstants.LIST_ManufacturerCountryOfOrigin, web.Url);
                        Utilities.BindDropDownItems(drpTSPackLocation, GlobalConstants.LIST_PackingLocationsLookup, webUrl);
                        Utilities.BindDropDownItems(ddlKosher, GlobalConstants.LIST_KosherTypesLookup, webUrl);

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
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "PackagingComponentEntryForm", "Page_Load");
            }
            
            if (ComponentType == "Transfer")
            {
                ListItem removeItem = drpPkgComponent.Items.FindByValue("52");
                drpPkgComponent.Items.Remove(removeItem);
            }

            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
            this.hiddenItemId.Value = iItemId.ToString();
            hdnPackagingType.Value = PackagingType;
            hdnComponentType.Value = IsNew ? "" : ComponentType;
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
                "UpdatePanelBOMPE", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper = true;", true);
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
                    dvMain.Visible = true;
                   
                    var packagingItem = packagingItemService.GetPackagingItemByPackagingId(PackagingItemId);

                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, this.drpPkgComponent, this.Page);
                    Utilities.SetDropDownValue(packagingItem.PackUnit, this.ddlPackUnit, this.Page);
                    Utilities.SetDropDownValue(packagingItem.NewExisting, this.drpNew, this.Page);
                    if (string.IsNullOrEmpty(packagingItem.SAPMaterialGroup))
                    {
                        string componantType = Utilities.GetLookupValue(GlobalConstants.LIST_PackagingComponentTypesLookup, packagingItem.PackagingComponent, webUrl);
                        hdnSAPMatGroup.Value = componantType;
                    }
                    else
                    {
                        hdnSAPMatGroup.Value = packagingItem.SAPMaterialGroup;
                    }
                    hdnParentID.Value = ParentID.ToString();
                    txtMaterial.Text = packagingItem.MaterialNumber;
                    txtMaterialDescription.Text = packagingItem.MaterialDescription; // ?? Is this mapping correct
                    txtLikeItem.Text = packagingItem.CurrentLikeItem; // ?? Is this mapping correct
                    txtLikeDescription.Text = packagingItem.CurrentLikeItemDescription; // ?? Is this mapping correct

                    txtAllergen.Text = packagingItem.Allergens;
                    txtShelfLife.Text = packagingItem.ShelfLife;
                    Utilities.SetDropDownValue(packagingItem.TrialsCompleted, this.drpTrialsCompleted, this.Page);
                    Utilities.SetDropDownValue(packagingItem.NewFormula, this.ddlNewFormula, this.Page);
                    Utilities.SetDropDownValue(packagingItem.Kosher, this.ddlKosher, this.Page);

                    Utilities.SetDropDownValue(packagingItem.MakeLocation, this.drpTSMakeLocation, this.Page);
                    Utilities.SetDropDownValue(packagingItem.CountryOfOrigin, this.drpTSCountryOfOrigin, this.Page);
                    Utilities.SetDropDownValue(packagingItem.PackLocation, this.drpTSPackLocation, this.Page);

                    //txtTareWt.Text = packagingItem.TareWeight;
                    txtPackQty.Text = packagingItem.PackQuantity == "-9999" ?"": packagingItem.PackQuantity;
                    txtLeadTimeMaterial.Text = packagingItem.LeadMaterialTime;
                    txtNotes.Text = packagingItem.Notes;
                    //txtPrinter.Text = packagingItem.PrinterSupplier;

                    txtFilmLength.Text = packagingItem.Length;
                    txtFilmWidth.Text = packagingItem.Width;
                    txtFilmHeight.Text = packagingItem.Height;
                    txtFilmBagFace.Text = packagingItem.BagFace;
                    txtFilmWebWidth.Text = packagingItem.WebWidth;
                    txtFilmExactCutoff.Text = packagingItem.ExactCutOff;
                    txtFilmUnWind.Text = packagingItem.Unwind;

                    Utilities.SetDropDownValue(packagingItem.BackSeam, this.drpFilmBackSeam, this.Page);
                    Utilities.SetDropDownValue(packagingItem.FilmSubstrate, this.ddlFilmSubstrate, this.Page); // Pass from master page if component type is Film
                    Utilities.SetDropDownValue(packagingItem.StructureColor, this.ddlStructureColor, this.Page);
                    Utilities.SetDropDownValue(packagingItem.Structure, this.ddlFilmStructure, this.Page);
                    Utilities.SetDropDownValue(packagingItem.FilmStyle, this.ddlFilmStyle, this.Page);

                    if (packagingItem.PackagingComponent.ToLower().Contains("corrugated"))
                    {
                        Utilities.BindDropDownItems(ddlFilmPrintStyle, GlobalConstants.LIST_CorrugatedPrintStyleLookup, SPContext.Current.Web.Url);
                        Utilities.SetDropDownValue(packagingItem.CorrugatedPrintStyle, this.ddlFilmPrintStyle, this.Page);
                    }
                    else
                    {
                        Utilities.BindDropDownItems(ddlFilmPrintStyle, GlobalConstants.LIST_FilmPrintStyleLookup, SPContext.Current.Web.Url);
                        Utilities.SetDropDownValue(packagingItem.FilmPrintStyle, this.ddlFilmPrintStyle, this.Page);
                    }

                    txtFilmMaxRollOD.Text = packagingItem.FilmMaxRollOD;
                    txtFilmRollID.Text = packagingItem.FilmRollID;

                    txtCadDrawingName.Text =packagingItem.CADDrawing ;

                    GetAttachments();

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
                packagingItem.PackagingComponent = this.drpPkgComponent.SelectedItem.Text;
                packagingItem.PackUnit = this.ddlPackUnit.SelectedItem.Text;
                packagingItem.NewExisting = drpNew.SelectedItem.Text;
                packagingItem.MaterialNumber = txtMaterial.Text;
                packagingItem.MaterialDescription = txtMaterialDescription.Text;
                packagingItem.CurrentLikeItem = txtLikeItem.Text ;
                packagingItem.CurrentLikeItemDescription = txtLikeDescription.Text;

                packagingItem.Allergens = this.txtAllergen.Text;
                packagingItem.Kosher = this.ddlKosher.SelectedItem.Text;
                packagingItem.TrialsCompleted = this.drpTrialsCompleted.SelectedItem.Text;
                packagingItem.NewFormula = this.ddlNewFormula.SelectedItem.Text;
                packagingItem.ShelfLife = this.txtShelfLife.Text;

                packagingItem.MakeLocation = this.drpTSMakeLocation.SelectedItem.Text;
                packagingItem.CountryOfOrigin = this.drpTSCountryOfOrigin.SelectedItem.Text;
                packagingItem.PackLocation = this.drpTSPackLocation.SelectedItem.Text;

                //packagingItem.TareWeight = txtTareWt.Text;
                packagingItem.PackQuantity = txtPackQty.Text;
                packagingItem.LeadMaterialTime = txtLeadTimeMaterial.Text;
                packagingItem.Notes = txtNotes.Text;
                packagingItem.Length = txtFilmLength.Text;
                packagingItem.Width = txtFilmWidth.Text;
                packagingItem.Height = txtFilmHeight.Text;
                packagingItem.BackSeam = drpFilmBackSeam.SelectedItem.Text; 
                packagingItem.WebWidth = txtFilmWebWidth.Text;
                packagingItem.ExactCutOff = txtFilmExactCutoff.Text;
                packagingItem.Unwind = txtFilmUnWind.Text;
                packagingItem.BagFace = txtFilmBagFace.Text;
                packagingItem.StructureColor = this.ddlStructureColor.SelectedItem.Text;
                packagingItem.Structure = this.ddlFilmStructure.SelectedItem.Text;

                if (packagingItem.PackagingComponent.ToLower().Contains("film"))
                {
                    packagingItem.FilmPrintStyle = this.ddlFilmPrintStyle.SelectedItem.Text;
                }
                else if (packagingItem.PackagingComponent.ToLower().Contains("corrugated"))
                {
                    packagingItem.CorrugatedPrintStyle = this.ddlFilmPrintStyle.SelectedItem.Text;
                }

                packagingItem.FilmMaxRollOD = txtFilmMaxRollOD.Text;
                packagingItem.FilmRollID = txtFilmRollID.Text;
                packagingItem.PrinterSupplier = ddlPrinter.SelectedItem.Text;
                packagingItem.PackagingType = hdnPackagingType.Value;
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
            if (files.Count <= 1)
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
            var files =  packagingItemService.GetUploadedFiles(projectNo, PackagingItemId, GlobalConstants.DOCTYPE_CADDrawing);
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
                UpdateStatus();
            }
            else
            {
                item.Id = packagingItemService.InsertPackagingItem(item, CompassItemId);
            }
        }
        protected void btnLookupSAPItemNumber_Click(object sender, EventArgs e)
        {
            string sapNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtMaterial.Text))
            {
                sapNumber = txtMaterial.Text.Trim();
                dvMain.Visible = true;
                drpPkgComponent.Enabled = false;
                lblM.Visible = false;
            }
            else
            {
                // Set Error
                lblM.Visible = true;
                lblM.Text = "Please enter a valid Finished Good Item #!";


                ErrorSummary.AddError("Please enter a valid Finished Good Item #!", this.Page);
                return;
            }

            dvMain.Visible = true;
            txtMaterialDescription.Text = SetLookupValues(sapNumber, true);
          
            SaveData();
           
            if (!string.IsNullOrEmpty(hdnComponentType.Value))
            {
                ComponentType = hdnComponentType.Value;
                PackagingItemId = Convert.ToInt32(hdnPackagingItemId.Value);
               
                LoadControlData();

                if (drpNew.SelectedItem.Text.ToLower() == "existing")
                {
                    txtLikeItem.Enabled = false;
                    txtLikeDescription.Enabled = false;
                }
                //ScriptManager.RegisterStartupScript(this.Page, UpdatePanelBOMPE.GetType(), "", "pageLoadCheck();", true);
            }
        }
        private string SetLookupValues(string sapNumber, bool sapLookup)
        {
            SAPMaterialMasterListItem mmItem = sapMMService.GetSAPMaterialMaster(sapNumber);

            List<SAPBOMListItem> bomList = new List<SAPBOMListItem>();
            if (ComponentType == "Transfer")
            {
                bomList = sapBOMService.GetSAPBOMItems(sapNumber, GlobalConstants.MaterialType_Pack);
            }
            else {
                bomList = sapBOMService.GetSAPBOMItems(sapNumber, GlobalConstants.MaterialType_TransferSemi);
            }
            if ((bomList == null) || (bomList.Count==0))
            {
                ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Search Failed:</p></strong><br/>";
                ErrorSummary.AddError("Finished Good Component # could not be found!  Please try again.", this.Page);
                //return "";
            }
            if (sapLookup)
            {
                foreach (var item in bomList)
                {
                    txtMaterialDescription.Text = item.MaterialDescription;
                    txtPackQty.Text = item.PackQuantity;
                    Utilities.SetDropDownValue(item.PackUnit, this.ddlPackUnit, this.Page);
                }
                //  txtPackQty.Text = mmItem
            }
            else {
                foreach (var item in bomList)
                {
                    txtLikeDescription.Text = item.MaterialDescription;
                    txtPackQty.Text = item.PackQuantity;
                    Utilities.SetDropDownValue(item.PackUnit, this.ddlPackUnit, this.Page);
                }
               
            }
            if (string.IsNullOrEmpty(mmItem.SAPDescription))
            {
                lblM.Visible = true;
                lblM.Text = "Finished Good Component # could not be found!  Please try again.";
                ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Search Failed:</p></strong><br/>";
                ErrorSummary.AddError("Finished Good Component # could not be found!  Please try again.", this.Page);
                return "";
            }
            lblM.Visible = false;
            return mmItem.SAPDescription;
        }
        protected void btnOldComponet_Click(object sender, EventArgs e)
        {
            string sapNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtLikeItem.Text))
            {
                sapNumber = txtLikeItem.Text.Trim();
                dvMain.Visible = true;
                drpPkgComponent.Enabled = false;
            }
            else
            {
                // Set Error
                ErrorSummary.AddError("Please enter a valid Like/Old Component #", this.Page);
                return;
            }
            SaveData();

            dvMain.Visible = true;
            ComponentType = hdnComponentType.Value;
            PackagingItemId = Convert.ToInt32(hdnPackagingItemId.Value);
            LoadControlData();
            txtLikeDescription.Text =  SetLookupValues(sapNumber, false);
            if (drpNew.SelectedItem.Text.ToLower() == "new")
            {
                txtMaterialDescription.Enabled = false;
            }
            // ScriptManager.RegisterStartupScript(this.Page, UpdatePanelBOMPE.GetType(), "", "pageLoadCheck();", true);
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try {
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

        protected void btnAddNewPackagingItem_Click(object sender, EventArgs e)
        {
            hdnComponentType.Value = "";
            hdnPackagingType.Value = GlobalConstants.PACKAGINGTYPE_SEMIBOM;
            dvMain.Visible = true;
       //     ScriptManager.RegisterStartupScript(this.Page, UpdatePanelBOMPE.GetType(), "", "pageLoadCheck();", true);
        }

//        protected void rptTransferSemi_ItemCommand(object source, RepeaterCommandEventArgs e)
//        {
//            if (e.CommandName.ToLower() == "delete")
//            {
//                int id = Convert.ToInt32(e.CommandArgument.ToString());
//                if (id > 0)
//                    packagingItemService.DeletePackagingItem(id, webUrl);
//                LoadBOMItems();
//            }
//            if (e.CommandName.ToLower() == "loadcontrol")
//            {
//                HiddenField hdnType = (HiddenField)e.Item.FindControl("hdnComponentType");
//                ComponentType = hdnType.Value;

//                hdnComponentType.Value = hdnType.Value;
//                PackagingItemId = Convert.ToInt32(e.CommandArgument.ToString());
//                dvMain.Visible = true;
//                LoadControlData();

////                ScriptManager.RegisterStartupScript(this.Page, UpdatePanelBOMPE.GetType(), "", "pageLoadCheck();", true);
//            }
//        }

        //protected void rptTransferSemi_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
        //    {
        //        PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

        //        TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
        //        TextBox txtMaterialDesc = (TextBox)e.Item.FindControl("txtMaterialDesc");


        //        DropDownList ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpComponent"));

        //        Utilities.BindDropDownItems(ddlPackagingComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl);
        //        if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty)
        //        {
        //            ddlPackagingComponent.Items.FindByText(packagingItem.PackagingComponent).Selected = true;
        //        }
        //        ddlPackagingComponent.Enabled = false;

        //        DropDownList drpUnitOfMeasure = ((DropDownList)e.Item.FindControl("drpUnitOfMeasure"));
        //        Utilities.BindDropDownItems(drpUnitOfMeasure, GlobalConstants.LIST_PackUnitLookup, webUrl);
        //        if (packagingItem.PackUnit != null && packagingItem.PackUnit != string.Empty)
        //        {
        //            drpUnitOfMeasure.Items.FindByText(packagingItem.PackUnit).Selected = true;
        //        }

        //        DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNew"));
        //        if (packagingItem.ComponentType != null && packagingItem.ComponentType != string.Empty)
        //        {
        //            drpNew.Items.FindByText(packagingItem.ComponentType).Selected = true;

        //            if (packagingItem.ComponentType.ToLower() == "new")
        //            {
        //                txtMaterial.Text = string.IsNullOrEmpty(packagingItem.MaterialNumber) ? "Needs New" : packagingItem.MaterialNumber;
        //                txtMaterialDesc.Text = string.IsNullOrEmpty(packagingItem.MaterialDescription) ? "Needs New" : packagingItem.MaterialDescription;

        //                txtMaterial.CssClass = "numericNoMask form-control Component PCBOMrequired";
        //                txtMaterialDesc.CssClass = "form-control ComponentDesc PCBOMrequired";

        //            }
        //        }
        //    }
        //}

        protected void btnTransferSemi_Click(object sender, EventArgs e)
        {
            hdnPackagingType.Value = GlobalConstants.PACKAGINGTYPE_SEMIBOM;
            dvMain.Visible = true;
        //    ScriptManager.RegisterStartupScript(this.Page, UpdatePanelBOMPE.GetType(), "", "pageLoadCheck();", true);
        }

        protected void btnreloadTransferSemi_Click(object sender, EventArgs e)
        {


        }

        private void BindtransferSemiData()
        {
          List<PackagingItem> lstBOMitems =  packagingItemService.GetSemiBOMItemsForProject(iItemId);

            //if (lstBOMitems.Count > 0)
            //{
            //    rptTransferSemi.Visible = true;
            //    rptTransferSemi.DataSource = lstBOMitems;
            //    rptTransferSemi.DataBind();
            //}
            //else
            //{
            //    rptTransferSemi.Visible = false;
            //}

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
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

        private void UpdateStatus()
        {
            
            if (PackagingItemId > 0)
            {
                string val = hdnRequiredCheck.Value;
                if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BillofMaterialSetUpPE.ToLower()))
                {
                    packagingItemService.UpdateCompletionStatus("pe", val, PackagingItemId);
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BillofMaterialSetUpPE2.ToLower()))
                {
                    packagingItemService.UpdateCompletionStatus("pe2", val, PackagingItemId);
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BillofMaterialSetUpProc.ToLower()))
                {
                    packagingItemService.UpdateCompletionStatus("proc", val, PackagingItemId);
                }
               
            }
            
        }
    }
}
