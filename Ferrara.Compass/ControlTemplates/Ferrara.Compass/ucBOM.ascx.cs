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
    public partial class ucBOM : UserControl
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
        private const string _ascxPath = @"~/_controltemplates/15/Ferrara.Compass/ucPackagingEngineering.ascx";
        PlaceHolder phUC;
        #endregion

        #region Properties
        public int PackagingItemId { get; set; }
        public int ParentId { get; set; }
        public int CompassItemId { get; set; }
        public string ComponentType { get; set; }
        public string PackagingType { get; set; }
        public string CompassListId { get; set; }
        public string FilmSubstrate { get; set; }
        public bool IsNew { get; set; }
        public string MaterialNumber { get; set; }
        public string MaterialDesc { get; set; }
        public string SemiXferMakeLocation { get; set; }
        
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
                Utilities.BindDropDownItems(drpTransferLocation, GlobalConstants.LIST_MakePackTransfersLookup, webUrl);
                phUC = (PlaceHolder)this.Parent.FindControl("phMsg");
                hdnUCBOMComponentType.Value = PackagingType;

                LoadBOMItems();
                LoadFormData();
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
        private void CalculateFields(ucBOM ctrl)
        {
            double width = Utilities.GetDecimal(ctrl.txtCaseMeasurementsW.Text);
            double height = Utilities.GetDecimal(ctrl.txtCaseMeasurementsH.Text);
            double length = Utilities.GetDecimal(ctrl.txtCaseMeasurementsL.Text);
            var casecube = (width / 12) * (height / 12) * (length / 12);
            ctrl.txtCaseCube.Text = Utilities.FormatDecimal(casecube, 2);

            width = Utilities.GetDecimal(ctrl.txtPalletDimensionsW.Text);
            height = Utilities.GetDecimal(ctrl.txtPalletDimensionsH.Text);
            length = Utilities.GetDecimal(ctrl.txtPalletDimensionsL.Text);
            var pallet = (length / 12) * (height / 12) * (width / 12);
            ctrl.txtPalletCube.Text = Utilities.FormatDecimal(pallet, 2);

            double netUnitWeight = Utilities.GetDecimal(ctrl.txtNetUnitWeight.Text);
            double casePack = Utilities.GetDecimal(ctrl.txtCasePack.Text);
            var casenetweight = (netUnitWeight * casePack) / 16;
            double caseGrossWeight = Utilities.GetDecimal(ctrl.txtCaseGrossWeight.Text);
            ctrl.txtCaseNetWeight.Text = Utilities.FormatDecimal(casenetweight, 2);

            double palletWeight = Utilities.GetDecimal(ctrl.txtPalletWeight.Text);
            double casesPerPallet = Utilities.GetDecimal(ctrl.txtCasesperPallet.Text);
            var palletGrossWeight = (caseGrossWeight * casesPerPallet) + palletWeight;
            ctrl.txtPalletGrossWeight.Text = Utilities.FormatDecimal(palletGrossWeight, 2);
        }

        #endregion

        #region Repeater Events
        protected void rptPackingItem_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
                TextBox txtMaterialDesc = (TextBox)e.Item.FindControl("txtMaterialDesc");
                TextBox txtOldMaterial = (TextBox)e.Item.FindControl("txtOldMaterial");
                TextBox txtOldMaterialDesc = (TextBox)e.Item.FindControl("txtOldMaterialDesc");
                TextBox txtSAPMatGroup = (TextBox)e.Item.FindControl("txtSAPMatGroup");
                Image imgStatus = (Image)e.Item.FindControl("imgStatus");            
                HiddenField hdnComponentStatusChangeIds = (HiddenField)this.Parent.FindControl("hdnComponentStatusChangeIds");

                DropDownList ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpComponent"));
                if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BillofMaterialSetUpPE.ToLower()))
                {
                    if (packagingItem.PECompleted == GlobalConstants.DETAILFORMSTATUS_Completed)
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                    }
                    else
                    {
                        if (requirementsCheckPE(packagingItem))
                        {
                            imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                            if (hdnComponentStatusChangeIds != null)
                                hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                        }
                    }
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BillofMaterialSetUpPE2.ToLower()))
                {
                    if (packagingItem.PE2Completed == GlobalConstants.DETAILFORMSTATUS_Completed)
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                    }
                    else
                    {
                        if (requirementsCheckPE2(packagingItem))
                        {
                            imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                            if (hdnComponentStatusChangeIds != null)
                                hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id + ",";
                        }
                    }
                }
                else if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BillofMaterialSetUpProc.ToLower()))
                {
                    if (packagingItem.ProcCompleted == GlobalConstants.DETAILFORMSTATUS_Completed)
                    {
                        imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                    }
                    else
                    {
                        if (requirementsCheckPROC(packagingItem))
                        {
                            imgStatus.ImageUrl = "/_layouts/15/Ferrara.Compass/img/greenCircle.png";
                            if (hdnComponentStatusChangeIds != null)
                                hdnComponentStatusChangeIds.Value = hdnComponentStatusChangeIds.Value + packagingItem.Id +",";
                        }
                    }
                }
                else
                {
                    imgStatus.CssClass = "hideItem";
                    Panel statusColumn = (Panel)rptPackingItem.Controls[0].Controls[0].FindControl("statusColumn");
                    statusColumn.CssClass = "hideItem";
                }

                Utilities.BindDropDownItems(ddlPackagingComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl);
                if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty && !packagingItem.PackagingComponent.Contains("Select Packaging Component"))
                {
                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, ddlPackagingComponent, this.Page);
                }
                if (packagingItem.PackagingComponent.ToLower() == "transfer semi")
                {
                }

                ddlPackagingComponent.Enabled = false;

                DropDownList drpUnitOfMeasure = ((DropDownList)e.Item.FindControl("drpUnitOfMeasure"));
                Utilities.BindDropDownItems(drpUnitOfMeasure, GlobalConstants.LIST_PackUnitLookup, webUrl);
                if (packagingItem.PackUnit != null && packagingItem.PackUnit != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackUnit, drpUnitOfMeasure, this.Page);
                }

                DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNew"));
                if (packagingItem.NewExisting != null && packagingItem.NewExisting != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.NewExisting, drpNew, this.Page);

                    if (packagingItem.NewExisting.ToLower() == "new")
                    {
                        txtMaterial.Text = string.IsNullOrEmpty(packagingItem.MaterialNumber) ? "Needs New" : packagingItem.MaterialNumber;
                        txtMaterialDesc.Text = string.IsNullOrEmpty(packagingItem.MaterialDescription) ? "Needs New" : packagingItem.MaterialDescription;

                        txtMaterial.CssClass = "numericNoMask form-control Component PCBOMrequired";
                        txtMaterialDesc.CssClass = "form-control ComponentDesc PCBOMrequired";
                    }
                }
                txtOldMaterial.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItem) ? "" : packagingItem.CurrentLikeItem;
                txtOldMaterialDesc.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItemDescription) ? "" : packagingItem.CurrentLikeItemDescription;
                
                DropDownList drpPrinter = ((DropDownList)e.Item.FindControl("drpPrinter"));
                Utilities.BindDropDownItems(drpPrinter, GlobalConstants.LIST_PrinterSupplierLookup, webUrl);
                if (packagingItem.PrinterSupplier != null && packagingItem.PrinterSupplier != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PrinterSupplier, drpPrinter, this.Page);
                }

                if (string.IsNullOrEmpty(packagingItem.PackagingComponent))
                {
                    txtSAPMatGroup.Text = "";
                }
                else
                {
                    string componantType = Utilities.GetLookupValue(GlobalConstants.LIST_PackagingComponentTypesLookup, packagingItem.PackagingComponent, webUrl);
                    txtSAPMatGroup.Text = componantType;
                }
                HiddenField hdnPackagingNumbers = (HiddenField)this.Parent.FindControl("hdnPackagingNumbers");
                hdnPackagingNumbers.Value = hdnPackagingNumbers.Value + "<tr><td>"+ ddlPackagingComponent.SelectedItem.Text + "</td><td>"+ txtMaterial.Text + "</td></tr>";

            }
        }
        protected void rptPackingItem_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            HiddenField hdnType = (HiddenField)e.Item.FindControl("hdnComponentType");
            HiddenField hdnPackagingType = (HiddenField)e.Item.FindControl("hdnPackagingType");
            HiddenField hdnParID = (HiddenField)e.Item.FindControl("hdnParentID");

            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                if (id > 0)
                    packagingItemService.DeletePackagingItem(id);

                ParentId = Convert.ToInt32(hdnParID.Value);
                PackagingType = (GlobalConstants.PACKAGINGTYPE_FGBOM == hdnPackagingType.Value) ? "" : "transfer";
                LoadBOMItems();
            }
            if (e.CommandName.ToLower() == "loadcontrol")
            {
                saveData();
                phUC.Controls.Clear();
                var ctrl =  (ucPackagingEngineering)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucPackagingEngineering.ascx");
                ctrl.ComponentType = hdnType.Value;
                ctrl.PackagingItemId = Convert.ToInt32(e.CommandArgument.ToString());
                ctrl.ParentID = hdnParID.Value.ToString();
                ctrl.PackagingType = hdnPackagingType.Value;
                ctrl.IsNew = false;
                var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
                var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
                var hdnComponentype = (HiddenField)this.Parent.FindControl("hdnComponentype");
                var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

                hdnPageState.Value = "PE";
                hdnParentID.Value = hdnParID.Value.ToString(); 
                hdnPackagingID.Value = e.CommandArgument.ToString();
                hdnComponentype.Value = hdnType.Value;

                var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
                phPage.Visible = false;
                // Add messages to page
                phUC.Controls.Add(ctrl);
            }
        }
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
                string bomType = PackagingType.ToLower().Contains("transfer") ? GlobalConstants.PACKAGINGTYPE_SEMIBOM : GlobalConstants.PACKAGINGTYPE_FGBOM;
                if (bomType == GlobalConstants.PACKAGINGTYPE_FGBOM)
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
                exceptionService.Handle(LogCategory.CriticalError, exception, "ucBOM control", "GetAttachments");
            }
        }
        private void LoadBOMItems()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            MaterialNumber = string.IsNullOrEmpty(MaterialNumber) ? "Needs New" : MaterialNumber;
            MaterialDesc = string.IsNullOrEmpty(MaterialDesc) ? "Needs New" : MaterialDesc;
            if (PackagingType.ToLower().Contains("transfer"))
            {
                dtPackingItem = packagingItemService.GetSemiBOMItems(iItemId, ParentId);
                lblTitle.Text = "Transfer Semi Summary";
                dvAddTransferSemi.Visible = true;
                btnAddTransferSemi.CommandArgument = ParentId.ToString();
                btnSave.Text = "Save Transfer Semi Summary";
                hdnUCSEMiBOMComponentType.Value = "Transfer";
                dvPack.Visible = false;
                dvDisplay.Visible = false;
                dvSales.Visible = false;
                btnSave.CssClass = "Semi NonBOMPage";
                lblDesc.Text = ": " + MaterialNumber + " : " + MaterialDesc;
                Utilities.SetDropDownValue(SemiXferMakeLocation, this.drpTransferLocation, this.Page);
                dvTransferLocation.Visible = true;
                hdnid.Value = ParentId.ToString();
                btnUploadPalletPatern.Attributes.Add("class", " ButtonControlAutoSize " + ParentId.ToString());
                btnUploadPackTrial.Attributes.Add("class", " ButtonControlAutoSize " + ParentId.ToString());
                if (dtPackingItem.Count == 0)
                {
                    var item = new PackagingItem { ParentID = ParentId, CompassListItemId = iItemId.ToString(), PackagingType = GlobalConstants.PACKAGINGTYPE_SEMIBOM };
                    item.Id = packagingItemService.InsertPackagingItem(item, iItemId);
                    dtPackingItem = packagingItemService.GetSemiBOMItems(iItemId, ParentId);
                }

            }
            else
            {
                dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);
                hdnUCBOMComponentType.Value = "BOM";
                lblTitle.Text = "Finished Good Summary";
                dvAddFinishGoodItem.Visible = true;
                btnSave.Text = "Save Finished Item Summary";
                btnAddNewPackagingItem.CommandArgument = iItemId.ToString();
                btnSave.CssClass = "FGItem NonBOMPage";
                lblDesc.Text = ": " + MaterialNumber + " : " + MaterialDesc;
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
            }
            hdnPackagingItemCount.Value = Convert.ToString(dtPackingItem.Count);
            if (dtPackingItem.Count > 0)
            {
                dvUCBomPE.Visible = true;
                dvElements.Visible = true;
                LoadFormData();
            }
            else
            {
                dvUCBomPE.Visible = false;
                dvElements.Visible = false;
            }
            HiddenField hdnPackagingNumbers = (HiddenField)this.Parent.FindControl("hdnPackagingNumbers");
            hdnPackagingNumbers.Value = "";
            HiddenField hdnComponentStatusChangeIds = (HiddenField)this.Parent.FindControl("hdnComponentStatusChangeIds");
            if (hdnComponentStatusChangeIds != null)
                hdnComponentStatusChangeIds.Value = "";
            rptPackingItem.DataSource = dtPackingItem;
            rptPackingItem.DataBind();

        }
        private void LoadFormData()
        {
            string bomType = PackagingType.ToLower().Contains("transfer") ? GlobalConstants.PACKAGINGTYPE_SEMIBOM : GlobalConstants.PACKAGINGTYPE_FGBOM;
            List<ShipperFinishedGoodItem> shipperData;
            List<MixesItem> mixData;
            String MaterialGroup4ProductForm = null, MaterialGroup5PackType = null;

            BillofMaterialsItem billofMaterialsItem = billOfMaterialsService.GetBillOfMaterialsItem(iItemId);
            CompassPackMeasurementsItem compassPackMeasurementsItem = billOfMaterialsService.GetPackMeasurementsItem(iItemId, ParentId, bomType);
            if (bomType == GlobalConstants.PACKAGINGTYPE_SEMIBOM) {
                string location = packagingItemService.GetTransferSemiMakePackLocations(ParentId);
                Utilities.SetDropDownValue(location, this.drpTransferLocation, this.Page);
            }
            Utilities.SetDropDownValue(compassPackMeasurementsItem.PalletPatternChange, this.drpPalletPatternChange, this.Page);
            Utilities.SetDropDownValue(compassPackMeasurementsItem.PackTrialNeeded, this.ddlPackTrial, this.Page);
            Utilities.SetDropDownValue(compassPackMeasurementsItem.PackTrialResult, this.ddlResultPackTrial, this.Page);
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
            if (MaterialGroup5PackType.ToLower() == "shipper (shp)" || MaterialGroup5PackType.ToLower() == "shippers (shp)")
            {
                shipperData = GetShipperFGItem(iItemId);
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
        private CompassPackMeasurementsItem ConstructFormData(ucBOM ctrl)
        {
            var compassPackMeasurementsItem = new CompassPackMeasurementsItem();
            string bomType = ctrl.PackagingType.ToLower().Contains("transfer") ? GlobalConstants.PACKAGINGTYPE_SEMIBOM : GlobalConstants.PACKAGINGTYPE_FGBOM;
            try
            {
                
                compassPackMeasurementsItem.CompassListItemId = iItemId;
                compassPackMeasurementsItem.PackTrialNeeded = ctrl.ddlPackTrial.SelectedItem.Text.Trim();
                compassPackMeasurementsItem.PalletPatternChange = ctrl.drpPalletPatternChange.SelectedItem.Text.Trim();
                compassPackMeasurementsItem.PackTrialResult = ctrl.ddlResultPackTrial.SelectedItem.Text.Trim();
                compassPackMeasurementsItem.PackTrialComments = ctrl.txtCommentPackTrial.Text;
                compassPackMeasurementsItem.BOMType = bomType;
                compassPackMeasurementsItem.ParentComponentId = ctrl.ParentId;

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

                compassPackMeasurementsItem.SetUpDimensionsLength = Utilities.GetDecimal(ctrl.txtSetUpDimensionW.Text);
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
        protected void btnAddNewPackagingItem_Click(object sender, EventArgs e)
        {
            saveData();
            phUC.Controls.Clear();
            var obj = new PackagingItem();
            obj.ParentID = 0;
            int id= packagingItemService.InsertPackagingItem(obj, iItemId);
            var ctrl = (ucPackagingEngineering)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucPackagingEngineering.ascx");

            ctrl.ComponentType = "FGItem";
            ctrl.PackagingItemId = id;
            ctrl.ParentID = "0";
            ctrl.PackagingType = GlobalConstants.PACKAGINGTYPE_FGBOM;
            ctrl.IsNew = true;
            ctrl.CompassItemId = CompassItemId;
            var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
            var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
            var hdnComponentype = (HiddenField)this.Parent.FindControl("hdnComponentype");
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

            hdnPageState.Value = "PE";
            hdnParentID.Value = "0";
            hdnPackagingID.Value = id.ToString();
            hdnComponentype.Value = "FGItem";
            
            var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
            phPage.Visible = false;
            var projectInfoDiv = (Panel)this.Parent.FindControl("projectInfo");
            if (projectInfoDiv != null)
            {
                projectInfoDiv.CssClass = "hideItem";
            }
            // Add messages to page
            phUC.Controls.Add(ctrl);
        }
        protected void btnAddTransferSemi_Click(object sender, EventArgs e)
        {
            saveData();
            phUC.Controls.Clear();
            var obj = new PackagingItem();
            obj.ParentID = Convert.ToInt32(btnAddTransferSemi.CommandArgument.ToString());
            int id = packagingItemService.InsertPackagingItem(obj, iItemId);
            var ctrl = (ucPackagingEngineering)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucPackagingEngineering.ascx");
            ctrl.ComponentType = "Transfer";
            ctrl.PackagingItemId = id;
            ctrl.ParentID = btnAddTransferSemi.CommandArgument.ToString();
            ctrl.PackagingType = GlobalConstants.PACKAGINGTYPE_SEMIBOM;
            ctrl.IsNew = true;
            var hdnPackagingID = (HiddenField)this.Parent.FindControl("hdnPackagingID");
            var hdnParentID = (HiddenField)this.Parent.FindControl("hdnParentID");
            var hdnComponentype = (HiddenField)this.Parent.FindControl("hdnComponentype");
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");

            hdnPageState.Value = "PE";
            hdnParentID.Value = btnAddTransferSemi.CommandArgument.ToString();
            hdnPackagingID.Value = id.ToString();
            hdnComponentype.Value = "Transfer";

            var phPage = (PlaceHolder)this.Parent.FindControl("phPage");
            phPage.Visible = false;
            // Add messages to page
            phUC.Controls.Add(ctrl);

        }
        public void saveData()
        {
            var phBOM = (PlaceHolder)this.Parent.FindControl("phBOM");
            foreach (UserControl ctrl in phBOM.Controls)
            {
                var type = (ucBOM)ctrl;
                var item = ConstructFormData(type);
                if (type.iItemId > 0)
                {
                    if (type.ParentId > 0)
                    {
                       packagingItemService.UpdateTransferSemiMakePackLocations(type.ParentId, type.drpTransferLocation.SelectedItem.Text);
                    }
                    else
                    {
                        packagingItemService.UpdateFGMakePackTransferLocation(type.drpTransferLocation.SelectedItem.Text, type.iItemId);
                    }
                    billOfMaterialsService.UpsertPackMeasurementsItem(item, ProjectNumber);
                    CalculateFields(type);
                }
            }
            HiddenField hdnComponentStatusChangeIds = (HiddenField)this.Parent.FindControl("hdnComponentStatusChangeIds");
            if (hdnComponentStatusChangeIds != null)
                if(hdnComponentStatusChangeIds.Value != "")
                {
                    packagingItemService.updateCompletedItems(hdnComponentStatusChangeIds.Value, Utilities.GetCurrentPageName().ToLower());
                    hdnComponentStatusChangeIds.Value = "";
                }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var item = ConstructFormData(this);
                if (iItemId > 0)
                {
                    if (ParentId > 0)
                    {
                        packagingItemService.UpdateTransferSemiMakePackLocations(ParentId, drpTransferLocation.SelectedItem.Text);
                    }
                    else
                    {
                        packagingItemService.UpdateFGMakePackTransferLocation(drpTransferLocation.SelectedItem.Text, iItemId);
                    }
                    billOfMaterialsService.UpsertPackMeasurementsItem(item,ProjectNumber);
                    CalculateFields(this);
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "btnSubmit_Click");
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
        private bool requirementsCheckPE2(PackagingItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.NewExisting == "Select..." || item.NewExisting == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent == "Select..." || item.PackagingComponent == "")
            {
                completed = false;
            }
            else if (item.PackUnit == "Select..." || item.PackUnit == "")
            {
                completed = false;
            }
            else if (item.PackQuantity == "Select..." || item.PackQuantity == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItem == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialNumber == "")
            {
                completed = false;
            }
            else if (!string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi))
            {
                if (item.NewExisting == "New" && (item.Width == "" || item.Height == "" || item.Length == ""))
                {
                    completed = false;
                }
            }else if (string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi))
            {
                if (item.TransferSEMIMakePackLocations == "Select..." || item.TransferSEMIMakePackLocations == "")
                {
                    completed = false;
                }
                else if (item.PackLocation == "Select..." || item.PackLocation == "")
                {
                    completed = false;
                }
                else if (item.TransferSEMIMakePackLocations == "Externally Manufactured" && (item.CountryOfOrigin == "" || item.CountryOfOrigin == "Select..."))
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent.StartsWith("Rigid Plastic"))
            {
                if (item.NewExisting == "New")
                {
                    if (item.PrinterSupplier == "Select..." || item.PrinterSupplier == "")
                    {
                        completed = false;
                    }
                    else if (item.Structure == "Select..." || item.Structure == "")
                    {
                        completed = false;
                    }
                    else if (item.StructureColor == "Select..." || item.StructureColor == "")
                    {
                        completed = false;
                    }
                }
            }
            else if (item.PackagingComponent.StartsWith("Paperboard"))
            {
                if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent.StartsWith("Other"))
            {
                if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent.StartsWith("Label"))
            {
                if (item.NewExisting == "New")
                {
                    if (item.PrinterSupplier == "Select..." || item.PrinterSupplier == "")
                    {
                        completed = false;
                    }
                    else if (item.Structure == "Select..." || item.Structure == "")
                    {
                        completed = false;
                    }
                    else if (item.StructureColor == "Select..." || item.StructureColor == "")
                    {
                        completed = false;
                    }
                    else if (item.Unwind == "")
                    {
                        completed = false;
                    }
                }
            }
            else if (item.PackagingComponent.StartsWith("Film"))
            {
                if (item.FilmPrintStyle == "Select..." || item.FilmPrintStyle == "")
                {
                    completed = false;
                }
                if (item.NewExisting == "New")
                {
                    if (item.PrinterSupplier == "Select..." || item.PrinterSupplier == "")
                    {
                        completed = false;
                    }
                    else if (item.Structure == "Select..." || item.Structure == "")
                    {
                        completed = false;
                    }
                    else if (item.BackSeam == "Select..." || item.BackSeam == "")
                    {
                        completed = false;
                    }
                    else if (item.WebWidth == "" || item.ExactCutOff == "" || item.Unwind == "" || item.BagFace == "" || item.FilmMaxRollOD == "" || item.FilmRollID == "")
                    {
                        completed = false;
                    }
                    else if (item.FilmSubstrate == "Select..." || item.FilmSubstrate == "")
                    {
                        completed = false;
                    }
                    else if (item.FilmStyle == "Select..." || item.FilmStyle == "")
                    {
                        completed = false;
                    }
                }
            }
            else if (item.PackagingComponent.StartsWith("Corrugated"))
            {
                if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                {
                    completed = false;
                }
                else if (item.FilmPrintStyle == "Select..." || item.FilmPrintStyle == "")
                {
                    completed = false;
                }
            }
            else if (string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi))
            {
                if (item.TrialsCompleted == "Select..." || item.TrialsCompleted == "")
                {
                    completed = false;
                }
                else if (item.NewFormula == "Select..." || item.NewFormula == "")
                {
                    completed = false;
                }
                else if (item.ShelfLife == "" || item.Allergens == "")
                {
                    completed = false;
                }
                else if (item.Kosher == "Select..." || item.Kosher == "")
                {
                    completed = false;
                }
            }
            return completed;
        }
        private bool requirementsCheckPE(PackagingItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.NewExisting == "Select..." || item.NewExisting == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent == "Select..." || item.PackagingComponent == "")
            {
                completed = false;
            }
            else if (item.PackUnit == "Select..." || item.PackUnit == "")
            {
                completed = false;
            }
            else if (item.PackQuantity == "Select..." || item.PackQuantity == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItem == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialNumber == "")
            {
                completed = false;
            }
            else if (string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi))
            {
                if (item.TransferSEMIMakePackLocations == "Select..." || item.TransferSEMIMakePackLocations == "")
                {
                    completed = false;
                }
                else if (item.PackLocation == "Select..." || item.PackLocation == "")
                {
                    completed = false;
                }
                else if (item.TransferSEMIMakePackLocations == "Externally Manufactured" && (item.CountryOfOrigin == "" || item.CountryOfOrigin == "Select..."))
                {
                    completed = false;
                }
            }else if (string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi))
            {
                if (item.TrialsCompleted == "Select..." || item.TrialsCompleted == "")
                {
                    completed = false;
                }
                else if (item.NewFormula == "Select..." || item.NewFormula == "")
                {
                    completed = false;
                }
                else if (item.ShelfLife == "" || item.Allergens == "")
                {
                    completed = false;
                }
                else if (item.Kosher == "Select..." || item.Kosher == "")
                {
                    completed = false;
                }
            }
            return completed;
        }
        private bool requirementsCheckPROC(PackagingItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.NewExisting == "Select..." || item.NewExisting == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent == "Select..." || item.PackagingComponent == "")
            {
                completed = false;
            }
            else if (item.PackUnit == "Select..." || item.PackUnit == "")
            {
                completed = false;
            }
            else if (item.PackQuantity == "Select..." || item.PackQuantity == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItem == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialNumber == "")
            {
                completed = false;
            }
            else if (string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_TransferSemi))
            {
                if (item.TransferSEMIMakePackLocations == "Select..." || item.TransferSEMIMakePackLocations == "")
                {
                    completed = false;
                }
                else if (item.PackLocation == "Select..." || item.PackLocation == "")
                {
                    completed = false;
                }
                else if (item.TransferSEMIMakePackLocations == "Externally Manufactured" && (item.CountryOfOrigin == "" || item.CountryOfOrigin == "Select..."))
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent.StartsWith("Rigid Plastic"))
            {
                if (item.NewExisting == "New")
                {
                    if (item.PrinterSupplier == "Select..." || item.PrinterSupplier == "")
                    {
                        completed = false;
                    }
                }
            }
            else if (item.PackagingComponent.StartsWith("Paperboard"))
            {
                if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent.StartsWith("Other"))
            {
                if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent.StartsWith("Label"))
            {
                if (item.NewExisting == "New")
                {
                    if (item.PrinterSupplier == "Select..." || item.PrinterSupplier == "")
                    {
                        completed = false;
                    }
                }
            }
            else if (item.PackagingComponent.StartsWith("Film"))
            {
                if (item.NewExisting == "New")
                {
                    if (item.PrinterSupplier == "Select..." || item.PrinterSupplier == "")
                    {
                        completed = false;
                    }
                }
            }
            else if (item.PackagingComponent.StartsWith("Corrugated"))
            {
                if (item.NewExisting == "New" && (item.PrinterSupplier == "Select..." || item.PrinterSupplier == ""))
                {
                    completed = false;
                }
            }
            else if (string.Equals(item.PackagingComponent, GlobalConstants.COMPONENTTYPE_CandySemi))
            {
                if (item.TrialsCompleted == "Select..." || item.TrialsCompleted == "")
                {
                    completed = false;
                }
                else if (item.NewFormula == "Select..." || item.NewFormula == "")
                {
                    completed = false;
                }
                else if (item.ShelfLife == "" || item.Allergens == "")
                {
                    completed = false;
                }
                else if (item.Kosher == "Select..." || item.Kosher == "")
                {
                    completed = false;
                }
            }
            return completed;
        }
    }
}
