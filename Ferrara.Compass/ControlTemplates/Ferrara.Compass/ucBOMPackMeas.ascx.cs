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
    public partial class ucBOMPackMeas : UserControl
    {
        #region Member Variables
        private IItemProposalService itemProposalService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private IBillOfMaterialsService billOfMaterialsService;
        private ISAPMaterialMasterService sapMMService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IMixesService mixesService;
        private string webUrl;
        private int iItemId = 0;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<ShipperFinishedGoodItem> shipperFGItems = new List<ShipperFinishedGoodItem>();
        private const string _ascxPath = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx";
        PlaceHolder phUC;
        #endregion

        #region Properties
        public int ParentId { get; set; }
        public int CompassItemId { get; set; }
        public string PackagingComponent { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDesc { get; set; }
        public string SemiXferMakeLocation { get; set; }
        public string NewExisting { get; set; }
        public int NewComponentCount { get; set; }

        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }

        #endregion
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
            billOfMaterialsService = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            
        webUrl = SPContext.Current.Web.Url;
                iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);
                this.hiddenItemId.Value = iItemId.ToString();
                Utilities.BindDropDownItems(ddlResultPackTrial, GlobalConstants.LIST_PackTrialResultLookUp, webUrl);
                phUC = (PlaceHolder)this.Parent.FindControl("phMsg");
                hdnUCBOMComponentType.Value = PackagingComponent;

                
                LoadFormData();
                LoadBOMItems();
        }

        #region Private Methods       
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
        private void InitializeScreen()
        {

        }
        private void CalculateFields(ucBOMPackMeas ctrl)
        {
            double width = Utilities.GetDecimal(ctrl.txtCaseMeasurementsW.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtCaseMeasurementsW.Text);
            double height = Utilities.GetDecimal(ctrl.txtCaseMeasurementsH.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtCaseMeasurementsH.Text);
            double length = Utilities.GetDecimal(ctrl.txtCaseMeasurementsL.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtCaseMeasurementsL.Text);
            var casecube = (width / 12) * (height / 12) * (length / 12);
            ctrl.txtCaseCube.Text = Utilities.FormatDecimal(casecube, 2);

            width = Utilities.GetDecimal(ctrl.txtPalletDimensionsW.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtPalletDimensionsW.Text);
            height = Utilities.GetDecimal(ctrl.txtPalletDimensionsH.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtPalletDimensionsH.Text);
            length = Utilities.GetDecimal(ctrl.txtPalletDimensionsL.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtPalletDimensionsL.Text);
            var pallet = (length / 12) * (height / 12) * (width / 12);
            ctrl.txtPalletCube.Text = Utilities.FormatDecimal(pallet, 2);

            double netUnitWeight = Utilities.GetDecimal(ctrl.txtNetUnitWeight.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtNetUnitWeight.Text);
            double casePack = Utilities.GetDecimal(ctrl.txtCasePack.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtCasePack.Text);
            var casenetweight = (netUnitWeight * casePack) / 16;
            double caseGrossWeight = Utilities.GetDecimal(ctrl.txtCaseGrossWeight.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtCaseGrossWeight.Text);
            ctrl.txtCaseNetWeight.Text = Utilities.FormatDecimal(casenetweight, 2);

            double palletWeight = Utilities.GetDecimal(ctrl.txtPalletWeight.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtPalletWeight.Text);
            double casesPerPallet = Utilities.GetDecimal(ctrl.txtCasesperPallet.Text) == -9999 ? 0 : Utilities.GetDecimal(ctrl.txtCasesperPallet.Text);
            var palletGrossWeight = (caseGrossWeight * casesPerPallet) + palletWeight;
            ctrl.txtPalletGrossWeight.Text = Utilities.FormatDecimal(palletGrossWeight, 2);
        }

        #endregion

        #region Repeater Events
        protected void lnkDeleteAttachment_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }

        #endregion

        #region Data Transfer Methods
        private void GetAttachments()
        {
            try
            {
                if (ParentId == 0)
                {
                
                    var files = packagingItemService.GetUploadedFiles(ProjectNumber, 99999, GlobalConstants.DOCTYPE_PalletPattern);

                    if (files.Count > 0)
                    {
                        rptPalletPattern.Visible = true;
                        rptPalletPattern.DataSource = files;
                        rptPalletPattern.DataBind();
                        hdnPalletPatterCount.Value = files.Count().ToString();

                    }
                    else
                    {
                        rptPalletPattern.Visible = false;
                        hdnPalletPatterCount.Value = files.Count().ToString();
                    }

                    files = packagingItemService.GetUploadedFiles(ProjectNumber, 99999, GlobalConstants.DOCTYPE_PackTrial);

                    if (files.Count > 0)
                    {
                        rptUploadPackTrial.Visible = true;
                        rptUploadPackTrial.DataSource = files;
                        rptUploadPackTrial.DataBind();
                    }
                    else
                    {
                        rptUploadPackTrial.Visible = false;
                    }
                }
                else {
                    var files = packagingItemService.GetUploadedFiles(ProjectNumber, ParentId, GlobalConstants.DOCTYPE_PalletPattern);
                    if (files.Count > 0)
                    {
                        rptPalletPattern.Visible = true;
                        rptPalletPattern.DataSource = files;
                        rptPalletPattern.DataBind();
                        hdnPalletPatterCount.Value = files.Count().ToString();
                    }
                    else
                    {
                        rptPalletPattern.Visible = false;
                        hdnPalletPatterCount.Value = files.Count().ToString();
                    }

                    files = packagingItemService.GetUploadedFiles(ProjectNumber, ParentId, GlobalConstants.DOCTYPE_PackTrial);

                    if (files.Count > 0)
                    {
                        rptUploadPackTrial.Visible = true;
                        rptUploadPackTrial.DataSource = files;
                        rptUploadPackTrial.DataBind();
                    }
                    else
                    {
                        rptUploadPackTrial.Visible = false;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component GetAttachments method: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "ucBOMPackMeas control", "GetAttachments");
            }
        }
        private void LoadBOMItems()
        {
            MaterialNumber = string.IsNullOrEmpty(MaterialNumber) ? "Needs New" : MaterialNumber;
            MaterialDesc = string.IsNullOrEmpty(MaterialDesc) ? "Needs New" : MaterialDesc;
            if (NewComponentCount > 0)
            {
                hdnNewComponentExists.Text = "true";
            }
            else
            {
                hdnNewComponentExists.Text = "false";
            }
            if (ParentId != 0)
            {
                if (PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                {
                    hdnUCSEMiBOMComponentType.Value = "Transfer";
                    hdnIsTransferSemi.Text = "ts";
                }
                else if(PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    hdnUCSEMiBOMComponentType.Value = "Purchased Candy";
                    hdnIsTransferSemi.Text = "pcs";
                }
                
                dvPack.Visible = false;
                dvDisplay.Visible = false;
                dvSales.Visible = false;
                lblTransferLocation.Text = SemiXferMakeLocation;
                dvTransferLocation.Visible = true;
                hdnid.Value = ParentId.ToString();
                btnUploadPalletPatern.Attributes.Add("class", " ButtonControlAutoSize " + ParentId.ToString());
                btnUploadPackTrial.Attributes.Add("class", " ButtonControlAutoSize " + ParentId.ToString());
                lblSAPSpecChangeHeader.Text = PackagingComponent;
                lblSAPSpecChange.Text = PackagingComponent;

            }else
            {
                hdnUCBOMComponentType.Value = "BOM";
                hdnid.Value = "0";
                btnUploadPalletPatern.Attributes.Add("class", " ButtonControlAutoSize 99999");
                btnUploadPackTrial.Attributes.Add("class", " ButtonControlAutoSize 99999");
                txtUnitMeasurementsH.CssClass = txtUnitMeasurementsH.CssClass + "  salesDims";
                txtUnitMeasurementsW.CssClass = txtUnitMeasurementsW.CssClass + "  salesDims";
                txtUnitMeasurementsL.CssClass = txtUnitMeasurementsL.CssClass + "  salesDims";
                txtCaseMeasurementsH.CssClass = txtCaseMeasurementsH.CssClass + "  salesDims";
                txtCaseMeasurementsW.CssClass = txtCaseMeasurementsW.CssClass + "  salesDims";
                txtCaseMeasurementsL.CssClass = txtCaseMeasurementsL.CssClass + "  salesDims";

                txtSalesUnitDimensionsw.CssClass = txtSalesUnitDimensionsw.CssClass + "  salesDims";
                txtSalesUnitDimensionsH.CssClass = txtSalesUnitDimensionsH.CssClass + "  salesDims";
                txtSalesUnitDimensionsd.CssClass = txtSalesUnitDimensionsd.CssClass + "  salesDims";
                txtSalesCaseDimensionsD.CssClass = txtSalesCaseDimensionsD.CssClass + "  salesDims";
                txtSalesCaseDimensionsH.CssClass = txtSalesCaseDimensionsH.CssClass + "  salesDims";
                txtSalesCaseDimensionsW.CssClass = txtSalesCaseDimensionsW.CssClass + "  salesDims";

                lblSAPSpecChangeHeader.Text = "Finished Good";
                lblSAPSpecChange.Text = "Finished Good";
            }
             dvElements.Visible = true;
            HiddenField hdnPackagingNumbers = (HiddenField)this.Parent.FindControl("hdnPackagingNumbers");
            hdnPackagingNumbers.Value = "";

        }
        private void LoadFormData()
        {
           List<ShipperFinishedGoodItem> shipperData;
            List<MixesItem> mixData;
            String MaterialGroup4ProductForm = null, MaterialGroup5PackType = null;

            CompassPackMeasurementsItem compassPackMeasurementsItem = billOfMaterialsService.GetPackMeasurementsItem(iItemId, ParentId);
            if (PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi) {
                string location = packagingItemService.GetTransferSemiMakePackLocations(ParentId);
                lblTransferLocation.Text = location;
            }
            Utilities.SetDropDownValue(compassPackMeasurementsItem.PalletPatternChange, this.drpPalletPatternChange, this.Page);
            Utilities.SetDropDownValue(compassPackMeasurementsItem.PackTrialNeeded, this.ddlPackTrial, this.Page);
            Utilities.SetDropDownValue(compassPackMeasurementsItem.PackTrialResult, this.ddlResultPackTrial, this.Page);
            Utilities.SetDropDownValue(compassPackMeasurementsItem.SAPSpecsChange, this.drpSAPSpecsChange, this.Page);
            hdnParentComponentId.Value = Convert.ToString(ParentId);
            hdnPackTrial.Value = compassPackMeasurementsItem.PackTrialNeeded;

            txtCommentPackTrial.Text = compassPackMeasurementsItem.PackTrialComments;

            txtNetUnitWeight.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.NetUnitWeight, 4);

            txtUnitMeasurementsL.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.UnitDimensionLength, 4);
            txtUnitMeasurementsH.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.UnitDimensionHeight, 4);
            txtUnitMeasurementsW.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.UnitDimensionWidth, 4);

            txtCasePack.Text = Utilities.FormatNumber(compassPackMeasurementsItem.CasePack);

            txtCaseMeasurementsL.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.CaseDimensionLength, 4);
            txtCaseMeasurementsH.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.CaseDimensionHeight, 4);
            txtCaseMeasurementsW.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.CaseDimensionWidth, 4);

            getMaterialGroups(iItemId, ref MaterialGroup4ProductForm, ref MaterialGroup5PackType);
            if (ParentId == 0)
            {
                if (MaterialGroup5PackType.ToLower() == "shipper (shp)" || MaterialGroup5PackType.ToLower() == "shippers (shp)")
                {
                    shipperData = GetShipperFGItem(iItemId);
                    hdnMaterialGroup5PackType.Text = "true";
                    if (shipperData.Count > 0)
                    {
                        divShipper.Visible = true;
                        rpShipperSummary.DataSource = shipperData;
                        rpShipperSummary.DataBind();
                    }
                }
                if (MaterialGroup4ProductForm == "MIXES (MIX)")
                {
                    mixData = GetMixesItem(iItemId);
                    if (mixData.Count > 0)
                    {
                        divMixes.Visible = true;
                        rpMixesSummary.DataSource = mixData;
                        rpMixesSummary.DataBind();
                    }
                }
            }
            double width = compassPackMeasurementsItem.CaseDimensionWidth == -9999 ? 0 : compassPackMeasurementsItem.CaseDimensionWidth;
            double height = compassPackMeasurementsItem.CaseDimensionHeight == -9999 ? 0 : compassPackMeasurementsItem.CaseDimensionHeight;
            double length = compassPackMeasurementsItem.CaseDimensionLength == -9999 ? 0 : compassPackMeasurementsItem.CaseDimensionLength;
            var casecube = (width / 12) * (height / 12) * (length / 12);
            txtCaseCube.Text = Utilities.FormatDecimal(casecube, 2);

            txtCaseNetWeight.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.CaseNetWeight, 4);
            txtCaseGrossWeight.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.CaseGrossWeight, 4);

            txtCasesperPallet.Text = Utilities.FormatNumber(compassPackMeasurementsItem.CasesPerPallet);
            Utilities.SetDropDownValue(compassPackMeasurementsItem.DoubleStackable, this.ddlDoubleStackable, this.Page);

            txtPalletDimensionsL.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.PalletDimensionsLength, 4);
            txtPalletDimensionsH.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.PalletDimensionsHeight, 4);
            txtPalletDimensionsW.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.PalletDimensionsWidth, 4);

            width = compassPackMeasurementsItem.PalletDimensionsWidth == -9999 ? 0 : compassPackMeasurementsItem.PalletDimensionsWidth;
            height = compassPackMeasurementsItem.PalletDimensionsHeight == -9999 ? 0 : compassPackMeasurementsItem.PalletDimensionsHeight;
            length = compassPackMeasurementsItem.PalletDimensionsLength == -9999 ? 0 : compassPackMeasurementsItem.PalletDimensionsLength;
            var pallet = (length / 12) * (height / 12) * (width / 12);
            txtPalletCube.Text = Utilities.FormatDecimal(pallet, 2);

            double netUnitWeight = compassPackMeasurementsItem.NetUnitWeight == -9999 ? 0 : compassPackMeasurementsItem.NetUnitWeight;
            double casePack = compassPackMeasurementsItem.CasePack == -9999 ? 0 : compassPackMeasurementsItem.CasePack;
            var casenetweight = (netUnitWeight * casePack) / 16;
            txtCaseNetWeight.Text = Utilities.FormatDecimal(casenetweight, 4);

            txtPalletWeight.Text = Utilities.FormatNumber(compassPackMeasurementsItem.PalletWeight);

            double palletWeight = compassPackMeasurementsItem.PalletWeight == -9999 ? 0 : compassPackMeasurementsItem.PalletWeight;
            double casesPerPallet = compassPackMeasurementsItem.CasesPerPallet == -9999 ? 0 : compassPackMeasurementsItem.CasesPerPallet;
            double caseGrossWeight = compassPackMeasurementsItem.CaseGrossWeight == -9999 ? 0 : compassPackMeasurementsItem.CaseGrossWeight;
            var palletGrossWeight = (caseGrossWeight * casesPerPallet) + palletWeight;
            txtPalletGrossWeight.Text = Utilities.FormatDecimal(palletGrossWeight, 4);

            txtCasesperLayer.Text = Utilities.FormatNumber(compassPackMeasurementsItem.CasesPerLayer);
            txtLayersperPallet.Text = Utilities.FormatNumber(compassPackMeasurementsItem.LayersPerPallet);

            txtSalesUnitDimensionsH.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SalesUnitDimensionsHeight, 4);
            txtSalesUnitDimensionsw.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SalesUnitDimensionsWidth, 4);
            txtSalesUnitDimensionsd.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SalesUnitDimensionsLength, 4);

            txtSalesCaseDimensionsH.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SalesCaseDimensionsHeight, 4);
            txtSalesCaseDimensionsW.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SalesCaseDimensionsWidth, 4);
            txtSalesCaseDimensionsD.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SalesCaseDimensionsLength, 4);

            txtDisplayDimensionsL.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.DisplayDimensionsLength, 4);
            txtDisplayDimensionsW.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.DisplayDimensionsWidth, 4);
            txtDisplayDimensionsH.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.DisplayDimensionsHeight, 4);

            txtSetUpDimensionL.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SetUpDimensionsLength, 4);
            txtSetUpDimensionW.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SetUpDimensionsWidth, 4);
            txtSetUpDimensionH.Text = Utilities.FormatDecimal(compassPackMeasurementsItem.SetUpDimensionsHeight, 4);
            hdnNewExistingComp.Text = NewExisting;
            GetAttachments();
        }
        private List<MixesItem> GetMixesItem(int itemId)
        {
            List<MixesItem> dtMixesItem;
            if (ViewState["MixesItemTable"] == null){
                dtMixesItem = mixesService.GetMixesItems(itemId);
                ViewState["MixesItemTable"] = dtMixesItem;
            }
            else
                dtMixesItem = (List<MixesItem>)ViewState["MixesItemTable"];
            return dtMixesItem;
        }
        private List<ShipperFinishedGoodItem> GetShipperFGItem(int itemId)
        {
            List<ShipperFinishedGoodItem> dtFGItem;
            if (ViewState["FGShipperItemTable"] == null){
                dtFGItem = shipperFinishedGoodService.GetShipperFinishedGoodItems(itemId);
                ViewState["FGShipperItemTable"] = dtFGItem;
            }
            else
                dtFGItem = (List<ShipperFinishedGoodItem>)ViewState["FGShipperItemTable"];
            return dtFGItem;
        }
        private void getMaterialGroups(int itemId, ref string MaterialGroup4ProductForm, ref string MaterialGroup5PackType)
        {
            ItemProposalItem item;
            if (ViewState["MaterialGroup4ProductForm"] == null || ViewState["MaterialGroup5PackType"] == null)
            {
                item = itemProposalService.GetItemProposalItem(iItemId);
                ViewState["RetailSellingUnitsPerBaseUOM"] = item.RetailSellingUnitsBaseUOM == -9999 ?  "0" : item.RetailSellingUnitsBaseUOM.ToString();
                MaterialGroup4ProductForm = item.MaterialGroup4ProductForm;
                MaterialGroup5PackType = item.MaterialGroup5PackType;
                ViewState["MaterialGroup4ProductForm"] = MaterialGroup4ProductForm;
                ViewState["MaterialGroup5PackType"] = MaterialGroup5PackType;
            }
            else
            {
                MaterialGroup4ProductForm = (string)ViewState["MaterialGroup4ProductForm"];
                MaterialGroup5PackType = (string)ViewState["MaterialGroup5PackType"];
            }
        }
        private CompassPackMeasurementsItem ConstructFormData(ucBOMPackMeas ctrl)
        {
            var compassPackMeasurementsItem = new CompassPackMeasurementsItem();
            try
            {
                
                compassPackMeasurementsItem.CompassListItemId = iItemId;
                compassPackMeasurementsItem.PackTrialNeeded = ctrl.ddlPackTrial.SelectedItem.Text.Trim();
                compassPackMeasurementsItem.PalletPatternChange = ctrl.drpPalletPatternChange.SelectedItem.Text.Trim();
                compassPackMeasurementsItem.PackTrialResult = ctrl.ddlResultPackTrial.SelectedItem.Text.Trim();
                compassPackMeasurementsItem.PackTrialComments = ctrl.txtCommentPackTrial.Text;
                compassPackMeasurementsItem.ParentComponentId = Convert.ToInt32(ctrl.hdnParentComponentId.Value);
                compassPackMeasurementsItem.SAPSpecsChange = ctrl.drpSAPSpecsChange.SelectedItem.Text;

                //Unit Measurements
                compassPackMeasurementsItem.NetUnitWeight = Utilities.GetDecimal(ctrl.txtNetUnitWeight.Text);

                compassPackMeasurementsItem.UnitDimensionLength = Utilities.GetDecimal(ctrl.txtUnitMeasurementsL.Text);
                compassPackMeasurementsItem.UnitDimensionHeight = Utilities.GetDecimal(ctrl.txtUnitMeasurementsH.Text);
                compassPackMeasurementsItem.UnitDimensionWidth = Utilities.GetDecimal(ctrl.txtUnitMeasurementsW.Text);

                //Case Measurements
                compassPackMeasurementsItem.CasePack = Utilities.GetDecimal(ctrl.txtCasePack.Text);

                compassPackMeasurementsItem.CaseDimensionLength = Utilities.GetDecimal(ctrl.txtCaseMeasurementsL.Text);
                compassPackMeasurementsItem.CaseDimensionHeight = Utilities.GetDecimal(ctrl.txtCaseMeasurementsH.Text);
                compassPackMeasurementsItem.CaseDimensionWidth = Utilities.GetDecimal(ctrl.txtCaseMeasurementsW.Text);

                compassPackMeasurementsItem.CaseCube = Utilities.GetDecimal(ctrl.txtCaseCube.Text);
                compassPackMeasurementsItem.CaseNetWeight = Utilities.GetDecimal(ctrl.txtCaseNetWeight.Text);
                compassPackMeasurementsItem.CaseGrossWeight = Utilities.GetDecimal(ctrl.txtCaseGrossWeight.Text);

                //Pallet Measurements
                compassPackMeasurementsItem.CasesPerPallet = Utilities.GetDecimal(ctrl.txtCasesperPallet.Text);
                compassPackMeasurementsItem.DoubleStackable = ctrl.ddlDoubleStackable.SelectedItem.Text.Trim();

                compassPackMeasurementsItem.PalletDimensionsLength = Utilities.GetDecimal(ctrl.txtPalletDimensionsL.Text);
                compassPackMeasurementsItem.PalletDimensionsHeight = Utilities.GetDecimal(ctrl.txtPalletDimensionsH.Text);
                compassPackMeasurementsItem.PalletDimensionsWidth = Utilities.GetDecimal(ctrl.txtPalletDimensionsW.Text);

                compassPackMeasurementsItem.PalletCube = Utilities.GetDecimal(ctrl.txtPalletCube.Text);
                compassPackMeasurementsItem.PalletWeight = Utilities.GetDecimal(ctrl.txtPalletWeight.Text);
                compassPackMeasurementsItem.PalletGrossWeight = Utilities.GetDecimal(ctrl.txtPalletGrossWeight.Text);

                compassPackMeasurementsItem.CasesPerLayer = Utilities.GetDecimal(ctrl.txtCasesperLayer.Text);
                compassPackMeasurementsItem.LayersPerPallet = Utilities.GetDecimal(ctrl.txtLayersperPallet.Text);

                //Sales Dimensions
                compassPackMeasurementsItem.SalesUnitDimensionsHeight = Utilities.GetDecimal(ctrl.txtUnitMeasurementsH.Text);
                compassPackMeasurementsItem.SalesUnitDimensionsWidth = Utilities.GetDecimal(ctrl.txtUnitMeasurementsW.Text);
                compassPackMeasurementsItem.SalesUnitDimensionsLength = Utilities.GetDecimal(ctrl.txtUnitMeasurementsL.Text);

                compassPackMeasurementsItem.SalesCaseDimensionsHeight = Utilities.GetDecimal(ctrl.txtCaseMeasurementsH.Text);
                compassPackMeasurementsItem.SalesCaseDimensionsWidth = Utilities.GetDecimal(ctrl.txtCaseMeasurementsW.Text);
                compassPackMeasurementsItem.SalesCaseDimensionsLength = Utilities.GetDecimal(ctrl.txtCaseMeasurementsL.Text);

                //Display Dimensions
                compassPackMeasurementsItem.DisplayDimensionsLength = Utilities.GetDecimal(ctrl.txtDisplayDimensionsL.Text);
                compassPackMeasurementsItem.DisplayDimensionsWidth = Utilities.GetDecimal(ctrl.txtDisplayDimensionsW.Text);
                compassPackMeasurementsItem.DisplayDimensionsHeight = Utilities.GetDecimal(ctrl.txtDisplayDimensionsH.Text);

                compassPackMeasurementsItem.SetUpDimensionsLength = Utilities.GetDecimal(ctrl.txtSetUpDimensionL.Text);
                compassPackMeasurementsItem.SetUpDimensionsWidth = Utilities.GetDecimal(ctrl.txtSetUpDimensionW.Text);
                compassPackMeasurementsItem.SetUpDimensionsHeight = Utilities.GetDecimal(ctrl.txtSetUpDimensionH.Text);

            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "ConstructFormData");
            }
            return compassPackMeasurementsItem;
        }
        #endregion

        #region Button Methods
        
        public void saveData()
        {
            var phBOM = (PlaceHolder)this.Parent.FindControl("phBOM");
            foreach (UserControl ctrl in phBOM.Controls)
            {
                if (ctrl is ucBOMPackMeas)
                {
                    var type = (ucBOMPackMeas)ctrl;
                    var item = ConstructFormData(type);
                    if (type.iItemId > 0)
                    {
                        int ctrlParentId = Convert.ToInt32(type.hdnParentComponentId.Value);
                        if (ctrlParentId > 0)
                        {
                            packagingItemService.UpdateTransferSemiMakePackLocations(ctrlParentId, type.lblTransferLocation.Text);
                        }
                        billOfMaterialsService.UpsertPackMeasurementsItem(item, ProjectNumber);
                        CalculateFields(type);
                    }
                }
            }
        }
        
        #endregion

        protected void rpMixesSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblLbsForFGBOM, lblOzPerPiece, lblQtyMix;
            MixesItem mix;
            double RetailSellingUnitsPerBaseUOM;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                mix = (MixesItem)e.Item.DataItem;
                lblLbsForFGBOM = (Label)e.Item.FindControl("lblLbsForFGBOM");
                lblOzPerPiece = (Label)e.Item.FindControl("lblOzPerPiece");
                lblQtyMix = (Label)e.Item.FindControl("lblQtyMix");
                RetailSellingUnitsPerBaseUOM = Convert.ToDouble(ViewState["RetailSellingUnitsPerBaseUOM"]);
                lblQtyMix.Text = (mix.NumberOfPieces * RetailSellingUnitsPerBaseUOM).ToString();
                lblLbsForFGBOM.Text = (mix.NumberOfPieces * mix.OuncesPerPiece * RetailSellingUnitsPerBaseUOM / 16.0).ToString("0.00");
                lblOzPerPiece.Text = (mix.NumberOfPieces * mix.OuncesPerPiece).ToString();
            }
        } 
     }
}
