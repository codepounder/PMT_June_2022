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
using System.Globalization;

namespace Ferrara.Compass.WebParts.ComponentCostingQuoteForm
{
    [ToolboxItemAttribute(false)]
    public partial class ComponentCostingQuoteForm : WebPart
    {
        #region Member Variables
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private IBillOfMaterialsService billOfMaterialsService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private IComponentCostingQuoteService componentCostingQuoteService;
        private IPDFService pdfService;
        private IItemProposalService ipfService;
        private string webUrl;
        private int iItemId = 0;
        private string baseUOM = "";

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
        private string PackagingItemId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_PackagingItemId];
                return string.Empty;
            }
        }
        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ComponentCostingQuoteForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            billOfMaterialsService = DependencyResolution.DependencyMapper.Container.Resolve<IBillOfMaterialsService>();
            componentCostingQuoteService = DependencyResolution.DependencyMapper.Container.Resolve<IComponentCostingQuoteService>();
            pdfService = DependencyResolution.DependencyMapper.Container.Resolve<IPDFService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            ipfService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                Utilities.BindDropDownItems(ddlCostingUnit, GlobalConstants.LIST_CostingUnitLookup, webUrl);
                Utilities.BindDropDownItems(ddlReceivingPlant, GlobalConstants.LIST_PackingLocationsLookup, webUrl);
                Utilities.BindDropDownItems(ddlFilmPrintStyle, GlobalConstants.LIST_FilmPrintStyleLookup, webUrl);
                Utilities.BindDropDownItems(ddlFilmStyle, GlobalConstants.LIST_FilmStyleLookup, webUrl);
                Utilities.BindDropDownItems(ddlIncoterms, GlobalConstants.LIST_IncotermsLookup, SPContext.Current.Web.Url);
                Utilities.BindDropDownItems(ddlOrderUnitofMeasure, GlobalConstants.LIST_OrderUnitofMeasureLookup, SPContext.Current.Web.Url);
                Utilities.BindDropDownItems(ddlFilmStructure, GlobalConstants.LIST_SubstrateLookup, SPContext.Current.Web.Url);
                Utilities.BindDropDownItemsById(ddlFilmUnWind, GlobalConstants.LIST_FilmUnWindLookup, SPContext.Current.Web.Url);

                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;

                LoadFormData();
                InitializeScreen();
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }

        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                this.hiddenItemId.Value = iItemId.ToString();
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            hdnPackagingItem.Value = PackagingItemId;
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.ComponentCosting))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.ComponentCosting))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            if (lblComponentType.Text.Contains("Film"))
            {
                lblStructure.Text = "Film Structure:";
                dvPrintStyle.Attributes["class"] = dvPrintStyle.Attributes["class"].Replace("hideItem", "");
                dvStyle.Attributes["class"] = dvStyle.Attributes["class"].Replace("hideItem", "");
                dvPackagingComponentStructure.Attributes["class"] = dvPackagingComponentStructure.Attributes["class"].Replace("hideItem", "");
                dvWebWidth.Attributes["class"] = dvWebWidth.Attributes["class"].Replace("hideItem", "");
                dvExactCutOff.Attributes["class"] = dvExactCutOff.Attributes["class"].Replace("hideItem", "");
                dvUnwind.Attributes["class"] = dvUnwind.Attributes["class"].Replace("hideItem", "");
                dvCoreSize.Attributes["class"] = dvCoreSize.Attributes["class"].Replace("hideItem", "");
                dvMaxDiameter.Attributes["class"] = dvMaxDiameter.Attributes["class"].Replace("hideItem", "");
            }
            else if (lblComponentType.Text.Contains("Corrugated"))
            {
                lblStructure.Text = "Corrugated Structure:";
                dvPrintStyle.Attributes["class"] = dvPrintStyle.Attributes["class"].Replace("hideItem", "");
            }
            else if (lblComponentType.Text.Contains("Paperboard"))
            {
                lblStructure.Text = "Paperboard Structure:";
            }
            else if (lblComponentType.Text.Contains("Label"))
            {
                lblStructure.Text = "Label Structure:";
                dvPackagingComponentStructure.Attributes["class"] = dvPackagingComponentStructure.Attributes["class"].Replace("hideItem", "");
                dvWebWidth.Attributes["class"] = dvWebWidth.Attributes["class"].Replace("hideItem", "");
                dvExactCutOff.Attributes["class"] = dvExactCutOff.Attributes["class"].Replace("hideItem", "");
                dvUnwind.Attributes["class"] = dvUnwind.Attributes["class"].Replace("hideItem", "");
                dvCoreSize.Attributes["class"] = dvCoreSize.Attributes["class"].Replace("hideItem", "");
                dvMaxDiameter.Attributes["class"] = dvMaxDiameter.Attributes["class"].Replace("hideItem", "");
            }
            else if (lblComponentType.Text.Contains("Rigid"))
            {
                lblStructure.Text = "Rigid Plactic Structure:";
                dvPackagingComponentStructure.Attributes["class"] = dvPackagingComponentStructure.Attributes["class"].Replace("hideItem", "");
                dvNumberofColors.Attributes["class"] = dvNumberofColors.Attributes["class"] + " hideItem";
            }
            else if (lblComponentType.Text.Contains("Other"))
            {
                lblStructure.Text = "Other Structure:";
                dvPrintStyle.Attributes["class"] = dvPrintStyle.Attributes["class"].Replace("hideItem", "");
                dvStyle.Attributes["class"] = dvStyle.Attributes["class"].Replace("hideItem", "");
                dvPackagingComponentStructure.Attributes["class"] = dvPackagingComponentStructure.Attributes["class"].Replace("hideItem", "");
                dvWebWidth.Attributes["class"] = dvWebWidth.Attributes["class"].Replace("hideItem", "");
                dvExactCutOff.Attributes["class"] = dvExactCutOff.Attributes["class"].Replace("hideItem", "");
                dvUnwind.Attributes["class"] = dvUnwind.Attributes["class"].Replace("hideItem", "");
                dvCoreSize.Attributes["class"] = dvCoreSize.Attributes["class"].Replace("hideItem", "");
                dvMaxDiameter.Attributes["class"] = dvMaxDiameter.Attributes["class"].Replace("hideItem", "");
            }
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            var obj = new ComponentCostingQuoteItem();
            int pkgid = string.IsNullOrEmpty(PackagingItemId) ? 0 : Convert.ToInt32(PackagingItemId);
            obj = componentCostingQuoteService.GetComponentCostingQuoteItem(pkgid, iItemId);
            hdnComponentType.Value = obj.ComponentType;
            lblComponentType.Text = obj.ComponentType;
            txtCostingQuoteDate.Text = string.IsNullOrEmpty(obj.CostingQuoteDate.ToString()) ? DateTime.Now.ToShortDateString() : Utilities.GetDateForDisplay(Convert.ToDateTime(obj.CostingQuoteDate));
            lblBaseUOM.Text = obj.BaseUOM;
            lblStandardBaseUOM.Text = obj.BaseUOM;
            txtPrinterSupplier.Text = obj.PrinterSupplier;
            //txtConversionFactors.Text = obj.ConversionFactors;
            if (!String.IsNullOrEmpty(obj.VendorNumber))
            {
                txtVendorNumber.Text = obj.VendorNumber;
            }
            else
            {
                txtVendorNumber.Text = Utilities.GetLookupValue(GlobalConstants.LIST_PrinterSupplierLookup, obj.PrinterSupplier, webUrl);
            }

            txtMaterial.Text = obj.MaterialNumber;
            txtCommentForecast.Text = obj.ForecastComments;
            lblSKU.Text = obj.SKU;
            Utilities.SetDropDownValue(obj.PrintStyle, this.ddlFilmPrintStyle, this.Page);
            Utilities.SetDropDownValue(obj.Style, this.ddlFilmStyle, this.Page);
            Utilities.SetDropDownValue(obj.Structure, this.ddlFilmStructure, this.Page);
            txtFilmWebWidth.Text = obj.WebWidth;
            txtFilmExactCutoff.Text = obj.ExactCutOff;
            Utilities.SetDropDownValue(obj.Unwind, ddlFilmUnWind, Page);
            txtCoreSize.Text = obj.CoreSize;
            txtMaxDiaMeter.Text = obj.MaxDiameter;

            Utilities.SetDropDownValue(obj.ReceivingPlant, this.ddlReceivingPlant, this.Page);
            txtQuantitiesQuote.Text = obj.QuantityQuote;

            txtRequestDueDate.Text = obj.RequestedDueDate;
            txtColors.Text = obj.NumberColors;
            txtMinimumOrderQTY.Text = obj.StandardOrderingQuantity;
            Utilities.SetDropDownValue(obj.OrderUOM, this.ddlOrderUnitofMeasure, this.Page);
            Utilities.SetDropDownValue(obj.Incoterms, this.ddlIncoterms, this.Page);
            Utilities.SetDropDownValue(obj.CostingUnit.ToString(), this.ddlCostingUnit, this.Page);
            txtVendorMaterial.Text = obj.VendorMaterialNumber;
            txtCostingInfo.Text = obj.CostingCondition;
            txtEacher.Text = obj.EachesPerCostingUnit;
            txtLBRoll.Text = obj.LBPerCostingUnit;
            txtRollPallet.Text = obj.CostingUnitPerPallet;
            if (obj.StandardCost != "")
            {
                string standardCost = Utilities.FormatCurrency(Convert.ToDouble(obj.StandardCost));
                txtStandard.Text = standardCost;
            }
            else
            {
                txtStandard.Text = obj.StandardCost;
            }

            if (obj.RetailSellingUnitsBaseUOM > 0)
            {
                decimal packQty = string.IsNullOrEmpty(obj.PackQty) ? 0 : Convert.ToDecimal(obj.PackQty);
                double calc = Convert.ToDouble(((obj.Month1ProjectedUnits + obj.Month2ProjectedUnits + obj.Month3ProjectedUnits) / obj.RetailSellingUnitsBaseUOM) * packQty);
                txt90daysvol.Text = Utilities.FormatDecimal(calc, 0);
            }
            else
            {
                txt90daysvol.Text = "0";
            }
            //New Fields

            if (!string.IsNullOrEmpty(obj.ProcurementManager))
            {

                var users = Utilities.SetPeoplePickerValue(obj.ProcurementManager, SPContext.Current.Web);
                peProcurementManager.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1);
            }
            else
            {
                peProcurementManager.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.LoginName.ToString();
            }
            txtValidityStartDate.Text = String.IsNullOrEmpty(obj.ValidityStartDate.ToString()) ? DateTime.Now.ToShortDateString() : Utilities.GetDateForDisplay(Convert.ToDateTime(obj.ValidityStartDate));
            txtValidityEndDate.Text = String.IsNullOrEmpty(obj.ValidityEndDate.ToString()) ? "12/31/9999" : Utilities.GetDateForDisplay(Convert.ToDateTime(obj.ValidityEndDate));
            txtSupplierAgreementNumber.Text = obj.SupplierAgreementNumber;
            if (String.IsNullOrEmpty(obj.PriceDetermination))
            {
                Utilities.SetDropDownValue("Delivery Date", this.drpPriceDetermination, this.Page);
            }
            else
            {
                Utilities.SetDropDownValue(obj.PriceDetermination, this.drpPriceDetermination, this.Page);
            }
            Utilities.SetDropDownValue(obj.Subcontracted, this.drpSubcontracted, this.Page);
            Utilities.SetDropDownValue(obj.BracketPricing, this.ddlBracketPricing, this.Page);
            txtPIRCostperUOM.Text = obj.PIRCostPerUOM;
            txtPerUnit.Text = obj.PerUnit;
            if (String.IsNullOrEmpty(obj.DeliveredOrOriginCost))
            {
                Utilities.SetDropDownValue("Delivered", this.ddlDeliveredOriginCost, this.Page);
            }
            else
            {
                Utilities.SetDropDownValue(obj.DeliveredOrOriginCost, this.ddlDeliveredOriginCost, this.Page);
            }
            txtFreightAmount.Text = obj.FreightAmount;
            txtTransferOfOwnership.Text = obj.TransferOfOwnership;
            txtPlannedDeliveryTime.Text = obj.PlannedDeliveryTime;
            txtTolOverDelivery.Text = String.IsNullOrEmpty(obj.TolOverDelivery) ? "5" : obj.TolOverDelivery;
            txtTolUnderDelivery.Text = String.IsNullOrEmpty(obj.TolUnderDelivery) ? "5" : obj.TolUnderDelivery;
            txtPurchasingGroup.Text = obj.PurchasingGroup;
            lblAnnualVolume.Text = obj.AnnualVolumeCaseUOM;

            GetAttachments();
            GetDielineAttachments();
            GetBracketAttachments();
            LoadAlternateItems();
            lblQRHeader.Text = obj.MaterialNumber + " - " + obj.MaterialDescription;
            txtMaterialDescription.Text = obj.MaterialDescription;
        }
        private void LoadAlternateItems()
        {
            List<AlternateUOMItem> UOMConversionItem = new List<AlternateUOMItem>();
            UOMConversionItem = componentCostingQuoteService.GetAlternateUOMConversions(PackagingItemId);
            if(UOMConversionItem.Count <= 0)
            {
                AlternateUOMItem newAlternate = new AlternateUOMItem();
                newAlternate.AlternateUOM = "";
                newAlternate.PackagingItemId = Convert.ToInt32(PackagingItemId);
                newAlternate.Id = 0;
                newAlternate.XValue = "";
                newAlternate.YValue = "";
                UOMConversionItem.Add(newAlternate);
            }
            rptUOMAlternate.DataSource = UOMConversionItem;
            rptUOMAlternate.DataBind();
        }
        private ComponentCostingQuoteItem ConstructFormData()
        {
            var obj = new ComponentCostingQuoteItem();

            obj.CompassListItemId = iItemId;
            obj.PackagingItemId = PackagingItemId;

            string inputString = txtCostingQuoteDate.Text;
            DateTime dDate;

            if (DateTime.TryParse(inputString, out dDate))
            {
                obj.CostingQuoteDate = dDate.ToString();
            }
            else
            {
                obj.CostingQuoteDate = DateTime.Now.ToString();
            }
            obj.ProcurementManager = Utilities.GetPeopleFromPickerControl(peProcurementManager, SPContext.Current.Web).ToString();
            obj.MaterialNumber = txtMaterial.Text;
            obj.MaterialDescription = txtMaterialDescription.Text;
            obj.ReceivingPlant = ddlReceivingPlant.SelectedItem.Text;
            obj.First90Days = txt90daysvol.Text;
            obj.PrintStyle = ddlFilmPrintStyle.SelectedItem.Text;
            obj.QuantityQuote = txtQuantitiesQuote.Text;
            obj.ForecastComments = txtCommentForecast.Text;
            obj.RequestedDueDate = txtRequestDueDate.Text;
            obj.NumberColors = txtColors.Text;

            obj.VendorNumber = txtVendorNumber.Text;
            obj.PrinterSupplier = txtPrinterSupplier.Text;
            if (!string.IsNullOrEmpty(txtValidityStartDate.Text))
            {
                obj.ValidityStartDate = Convert.ToDateTime(txtValidityStartDate.Text);
            }
            if (!string.IsNullOrEmpty(txtValidityEndDate.Text))
            {
                obj.ValidityEndDate = Convert.ToDateTime(txtValidityEndDate.Text);
            }
            obj.SupplierAgreementNumber = txtSupplierAgreementNumber.Text;
            obj.PriceDetermination = drpPriceDetermination.SelectedItem.Text;
            obj.Subcontracted = drpSubcontracted.SelectedItem.Text;
            obj.BracketPricing = ddlBracketPricing.SelectedItem.Text;
            obj.PIRCostPerUOM = txtPIRCostperUOM.Text;
            obj.PerUnit = txtPerUnit.Text;
            obj.OrderUOM = ddlOrderUnitofMeasure.SelectedItem.Text;
            obj.DeliveredOrOriginCost = ddlDeliveredOriginCost.SelectedItem.Text;
            obj.FreightAmount = txtFreightAmount.Text;
            obj.Incoterms = ddlIncoterms.SelectedItem.Text;
            obj.TransferOfOwnership = txtTransferOfOwnership.Text;
            obj.PlannedDeliveryTime = txtPlannedDeliveryTime.Text;
            obj.MinimumOrderQTY = txtMinimumOrderQTY.Text;
            obj.StandardQuantity = txtStandardQuantity.Text;
            obj.TolOverDelivery = txtTolOverDelivery.Text;
            obj.TolUnderDelivery = txtTolUnderDelivery.Text;
            obj.PurchasingGroup = txtPurchasingGroup.Text;
            obj.VendorMaterialNumber = txtVendorMaterial.Text;

            obj.CostingCondition = txtCostingInfo.Text;

            obj.CostingUnit = ddlCostingUnit.SelectedItem.Text;
            obj.EachesPerCostingUnit = txtEacher.Text;
            obj.LBPerCostingUnit = txtLBRoll.Text;
            obj.CostingUnitPerPallet = txtRollPallet.Text;
            obj.StandardCost = txtStandard.Text;

            obj.NumberColors = txtColors.Text;
            obj.StandardOrderingQuantity = txtMinimumOrderQTY.Text;
            obj.Style = ddlFilmStyle.SelectedItem.Text;
            obj.Structure = ddlFilmStructure.SelectedItem.Text;
            obj.WebWidth = txtFilmWebWidth.Text;
            obj.ExactCutOff = txtFilmExactCutoff.Text;
            obj.Unwind = ddlFilmUnWind.SelectedItem.Text;
            obj.CoreSize = txtCoreSize.Text;
            obj.MaxDiameter = txtMaxDiaMeter.Text;

            //obj.ConversionFactors = txtConversionFactors.Text;
            //obj.AnnualVolumeEA = txtAnnualVolumeEA.Text;
            //obj.AnnualVolumeCaseUOM = txtAnnualVolumeCaseUOM.Text;
            //obj.SKU = lblSKU.Text;

            obj.Id = string.IsNullOrEmpty(PackagingItemId) ? 0 : Convert.ToInt32(PackagingItemId);

            return obj;
        }
        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            item.CompassListItemId = iItemId;
            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        #endregion

        #region Attachment Methods
        protected void lnkDeleteAttachment_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetDielineAttachments();
            }
        }
        protected void lnkFileDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
        }
        protected void lnkBracketDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetBracketAttachments();
            }
        }
        private void GetDielineAttachments()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_Dieline);
            if (files.Count > 0)
            {
                rptCApprovedDieline.Visible = true;
                rptCApprovedDieline.DataSource = files;
                rptCApprovedDieline.DataBind();
            }
            else
            {
                rptCApprovedDieline.Visible = false;
            }
        }
        private void GetAttachments()
        {
            var files = packagingItemService.GetUploadedFiles(ProjectNumber, Convert.ToInt32(PackagingItemId), GlobalConstants.DOCTYPE_COSTING);
            if (files.Count > 0)
            {
                rpAttachments.Visible = true;
                rpAttachments.DataSource = files;
                rpAttachments.DataBind();
            }
            else
            {
                rpAttachments.Visible = false;
            }
        }
        private void GetBracketAttachments()
        {
            var files = packagingItemService.GetUploadedFiles(ProjectNumber, Convert.ToInt32(PackagingItemId), GlobalConstants.DOCTYPE_BRACKETPRICING);
            if (files.Count > 0)
            {
                rptBracketPricing.Visible = true;
                rptBracketPricing.DataSource = files;
                rptBracketPricing.DataBind();
            }
            else
            {
                rptBracketPricing.Visible = false;
            }
        }
        #endregion

        #region Button Methods
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Initial Approver Review");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        private void CompleteWorkflowTask()
        {
        }
        private void saveAlternateItems(){
            foreach (RepeaterItem item in rptUOMAlternate.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    int itemId = Convert.ToInt32(((HiddenField)item.FindControl("hdnAlternateUOMID")).Value);
                    string xValue = ((TextBox)item.FindControl("txtX")).Text;
                    string yValue = ((TextBox)item.FindControl("txtY")).Text;
                    string alternateUOM = ((DropDownList)item.FindControl("drpAlternateUOM")).SelectedItem.Text;
                    if (itemId == 0 && (xValue == "" && yValue == "" && alternateUOM == "Select..."))
                    {
                            continue;
                    }
                    AlternateUOMItem alternateItem = new AlternateUOMItem();
                    alternateItem.AlternateUOM = alternateUOM;
                    alternateItem.PackagingItemId = Convert.ToInt32(PackagingItemId);
                    alternateItem.Id = itemId;
                    alternateItem.XValue = xValue;
                    alternateItem.YValue = yValue;
                    componentCostingQuoteService.UpdateAlternateUOMs(alternateItem, webUrl);
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.ComponentCosting))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                ComponentCostingQuoteItem item = ConstructFormData();

                item.LastUpdatedFormName = CompassForm.ComponentCosting.ToString();
                componentCostingQuoteService.UpdatePackagingItem(item,webUrl,hdnComponentType.Value);
                saveAlternateItems();
                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ComponentCosting.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.ComponentCosting.ToString(), "btnSave_Click");
            }
        }
        protected void btnCreatePDF_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave_Click(sender, e);
            }
            finally
            {
                pdfService.CreateComponentCostingRequestPDF(iItemId, PackagingItemId, "");
            }
            try {
                List<FileAttribute> requestPDF = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, "Costing");
                int packingIdLength = PackagingItemId.Length;
                foreach (FileAttribute file in requestPDF.Where(r => r.FileName.Substring(0, packingIdLength) == PackagingItemId))
                {
                    Page.Response.Write("<script>window.open('"+ file.FileUrl+"','_blank');</script>");
                    break;
                }
            }catch(Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ComponentCosting.ToString() + ": btnCreatePDF_Click: " + ex.Message);
            }

            /// USE FinalCostingGroup to pull emails also check Notification service
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.ComponentCosting))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                ComponentCostingQuoteItem item = ConstructFormData();
                item.CompCostSubmittedDate = DateTime.Now;
                componentCostingQuoteService.UpdatePackagingItem(item, webUrl, hdnComponentType.Value);

                ApprovalItem approvalItem = ConstructApprovalData();
                billOfMaterialsService.UpdateBillofMaterialsApprovalItem(approvalItem, Utilities.GetCurrentPageName(), true);
                ItemProposalItem ipfItem = ipfService.GetItemProposalItem(iItemId);
                List<KeyValuePair<string,bool>> completions = componentCostingQuoteService.allCostingQuotesSubmitted(iItemId, ipfItem.ProductHierarchyLevel1);
                string approvalUpdated = "";
                if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingSeasonal))
                {
                    if (string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                    {
                        string seasonalComp = (from key in completions where key.Key == "seasonal" select key.Value).ToString();
                        if (seasonalComp == "true")
                        {
                            workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.ComponentCostingSeasonal);
                        }
                        approvalUpdated = "seasonal";
                    }
                }
                else if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingFilm))
                {
                    if (!string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                    {
                        string filmComp = (from key in completions where key.Key == "film" select key.Value).ToString();
                        if (filmComp == "true")
                        {
                            workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.ComponentCostingFilmLabelRigidPlastic);
                        }
                        approvalUpdated = "film";
                    }
                }
                else if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PurchasingCorrugated))
                {
                    if (!string.Equals(ipfItem.ProductHierarchyLevel1, GlobalConstants.PRODUCT_HIERARCHY1_Seasonal))
                    {
                        string corrugatedComp = (from key in completions where key.Key == "corrugated" select key.Value).ToString();
                        if (corrugatedComp == "true")
                        {
                            workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.ComponentCostingCorrugatedPaperboard);
                        }
                        approvalUpdated = "corrugated";
                    }
                }
                string allComp = Convert.ToString((from key in completions where key.Key == "all" select key.Value).First());
                if (allComp.ToLower() == "true")
                {
                    componentCostingQuoteService.UpdateComponentCostingApprovalItem(approvalItem, true, approvalUpdated);
                }
                // Complete the workflow task
                //CompleteWorkflowTask();
                saveAlternateItems();
                // Redirect to Home page after successfull Submit 
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);

            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ComponentCosting.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.ComponentCosting.ToString(), "btnSubmit_Click");
            }
        }
        protected void btnAddNewAlternateUOM_Click(object sender, EventArgs e)
        {
            saveAlternateItems();
            List<AlternateUOMItem> UOMConversionItem = new List<AlternateUOMItem>();
            UOMConversionItem = componentCostingQuoteService.GetAlternateUOMConversions(PackagingItemId);
            AlternateUOMItem newAlternate = new AlternateUOMItem();
            newAlternate.AlternateUOM = "";
            newAlternate.PackagingItemId = Convert.ToInt32(PackagingItemId);
            newAlternate.Id = 0;
            newAlternate.XValue = "";
            newAlternate.YValue = "";
            UOMConversionItem.Add(newAlternate);
            rptUOMAlternate.DataSource = UOMConversionItem;
            rptUOMAlternate.DataBind();
        }
        #endregion

        protected void rptUOMAlternate_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                AlternateUOMItem alternateItem = (AlternateUOMItem)e.Item.DataItem;

                HiddenField hdnAlternateUOMID = (HiddenField)e.Item.FindControl("hdnAlternateUOMID");
                hdnAlternateUOMID.Value = Convert.ToString(alternateItem.Id);

                TextBox txtX = (TextBox)e.Item.FindControl("txtX");
                txtX.Text = alternateItem.XValue;

                DropDownList drpAlternateUOM = (DropDownList)e.Item.FindControl("drpAlternateUOM");
                Utilities.BindDropDownItems(drpAlternateUOM, GlobalConstants.LIST_SAPBaseUOMLookup, webUrl);
                if (!string.IsNullOrEmpty(alternateItem.AlternateUOM))
                {
                    Utilities.SetDropDownValue(alternateItem.AlternateUOM, drpAlternateUOM, this.Page);
                }

                TextBox txtY = (TextBox)e.Item.FindControl("txtY");
                txtY.Text = alternateItem.YValue;

                TextBox txtBaseUOM = (TextBox)e.Item.FindControl("txtBaseUOM");
                txtBaseUOM.Text = lblBaseUOM.Text;
            }
        }
        protected void rptUOMAlternate_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                if (id > 0)
                    componentCostingQuoteService.DeleteAlternateUOMItem(id);

                LoadAlternateItems();
            }
        }
    }
}
