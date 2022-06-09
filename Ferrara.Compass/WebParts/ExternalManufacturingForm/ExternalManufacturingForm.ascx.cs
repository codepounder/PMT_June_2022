using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.DependencyResolution;
using Microsoft.SharePoint;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Classes;
using System.Web.UI.WebControls;
using System.Globalization;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Collections.Generic;
using System.Linq;

namespace Ferrara.Compass.WebParts.ExternalManufacturingForm
{
    [ToolboxItemAttribute(false)]
    public partial class ExternalManufacturingForm : WebPart
    {
        #region Member Variables
        private IExternalManufacturingService externalMfgService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private int iItemId = 0;
        private ISAPMaterialMasterService sapMMService;
        private IPackagingItemService packagingItemService;
        private string webUrl;
        private const string _ucBOMEditable = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx";
        private const string _ucBOMGrid = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx";
        private IBOMSetupService BOMSetupService;
        private string PLMProject = "";
        private const string _ucBOMPackMeas_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMPackMeas_New.ascx";
        private const string _ucBOMEditable_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx";
        private const string _ucBOMGrid_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid_New.ascx";
        #endregion

        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return Page.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                return string.Empty;
            }
        }

        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ExternalManufacturingForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            externalMfgService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            webUrl = SPContext.Current.Web.Url;
            if (!Page.IsPostBack)
            {
                try
                {
                    this.divAccessDenied.Visible = false;
                    this.divAccessRequest.Visible = false;

                    // Check for a valid project number
                    if (!CheckProjectNumber())
                        return;

                    Utilities.BindDropDownItems(ddlCoManufacturingClassification, GlobalConstants.LIST_CoManufacturingClassifications, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlExternalManufacturer, GlobalConstants.LIST_CoManufacturers, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlExternalPacker, GlobalConstants.LIST_CoPackers, SPContext.Current.Web.Url);

                    Utilities.BindDropDownItemsById(ddlLeadTimeFromSupplier, GlobalConstants.LIST_SupplierLeadTime, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlManufacturerCountryOfOrigin, GlobalConstants.LIST_ManufacturerCountryOfOrigin, SPContext.Current.Web.Url);
                    Utilities.BindDropDownItems(ddlPurchasedIntoLocation, GlobalConstants.LIST_PurchasedIntoCenterLookup, SPContext.Current.Web.Url);


                    LoadFormData();
                    InitializeScreen();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.ExternalMfg.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
                PLMProject = hdnPLMProject.Value;
            }
            if (!string.Equals(PLMProject, "Yes"))
            {
                LoadBOMGrid();
            }
            else
            {
                LoadBOMGrid_New();
            }
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Ferrara.Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.ExternalMfg))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.ExternalMfg))
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
        }
        private Boolean ValidateForm()
        {
            Boolean bValid = true;

            // People fields
            if (peProjectLead.Entities.Count <= 0)
            {
                string strErrors = "External Manufacturing Lead must be set.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;0&quot;)'>  [Update]</a>";
                ErrorSummary.AddError(strErrors, this.Page);
                bValid = false;
            }

            return bValid;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            ExternalManufacturingItem item = externalMfgService.GetExternalManufacturingItem(iItemId);
            if (item.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblComments.Text = "Change Notes:";
            }
            else
            {
                lblComments.Text = "Item Concept:";
            }

            txtComments.Text = item.ItemConcept;
            hdnProjectType.Value = item.ProjectType;
            lblRevisedFirstShipDate.Text = Utilities.GetDateForDisplay(item.RevisedFirstShipDate);
            lblAnnualProjectUnits.Text = item.AnnualProjectedUnits.ToString("N0");

            if (item.SAPBaseUOM.ToLower() == "cs")
            {
                if (item.RetailSellingUnitsBaseUOM != 0)
                    lblAnnualProjectedCases.Text = Utilities.FormatNumber(item.AnnualProjectedUnits / item.RetailSellingUnitsBaseUOM);
                else
                    lblAnnualProjectedCases.Text = "NA";
            }
            else
                lblAnnualProjectedCases.Text = "NA";

            double grossMargin = item.ExpectedGrossMarginPercent;

            if (item.InitialCosting_GrossMarginAccurate != null && string.Equals(item.InitialCosting_GrossMarginAccurate.ToLower(), "no"))
                grossMargin = item.RevisedGrossMarginPercent;
            else
                grossMargin = item.ExpectedGrossMarginPercent;


            if (grossMargin == -9999)
                grossMargin = 0;
            else
                grossMargin = grossMargin / 100;

            lblAnnualProjectedLbs.Text = Utilities.FormatNumber((item.AnnualProjectedUnits * item.RetailUnitWieghtOz) / 16);
            double dUnitCostTarget = item.TruckLoadPricePerSellingUnit - (item.TruckLoadPricePerSellingUnit * grossMargin);
            lblUnitCostTarget.Text = dUnitCostTarget.ToString("C", new CultureInfo("en-US"));

            var users = Utilities.SetPeoplePickerValue(item.ExternalMfgProjectLead, SPContext.Current.Web);
            if (!string.IsNullOrEmpty(users))
            {
                peProjectLead.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1);
            }
            txtExistingBulkSemiNumber.Text = item.ExistingBulkSemiNumber;
            txtBulkSemiDescription.Text = item.BulkSemiDescription;
            txtFinalArtworkDueToSupplier.Text = Utilities.GetDateForDisplay(item.FinalArtworkDueToSupplier);
            string coManClass = item.CoManufacturingClassification;
            if (coManClass == "External Turn Key")
            {
                coManClass = "External Turnkey FG";
            }
            else if (coManClass == "External Manufacture" || coManClass == "External Purchased Semi")
            {
                coManClass = "External Turnkey Semi";
            }
            else if (coManClass == "External Co-Pack")
            {
                coManClass = "External Subcon FG";
            }
            else if (coManClass == "External Sub-Contract")
            {
                coManClass = "External Subcon Semi";
            }
            Utilities.SetDropDownValue(coManClass, this.ddlCoManufacturingClassification, this.Page);
            Utilities.SetDropDownValue(item.DoesBulkSemiExistToBringInHouse, this.ddlDoesBulkSemiExistToBringInHouse, this.Page);
            Utilities.SetDropDownValue(item.ExternalManufacturer, this.ddlExternalManufacturer, this.Page);
            Utilities.SetDropDownValue(item.ManufacturerCountryOfOrigin, this.ddlManufacturerCountryOfOrigin, this.Page);
            Utilities.SetDropDownValue(item.ExternalPacker, this.ddlExternalPacker, this.Page);
            Utilities.SetDropDownValue(item.PurchasedIntoLocation, this.ddlPurchasedIntoLocation, this.Page);

            Utilities.SetDropDownValue(item.CurrentTimelineAcceptable, this.ddlCurrentTimelineAcceptable, this.Page);
            Utilities.SetDropDownValue(item.LeadTimeFromSupplier, this.ddlLeadTimeFromSupplier, this.Page);

            lblPrimaryPackingLocation.Text = item.PackingLocation;
            lblManufacturingLocation.Text = item.MakeLocation;

            if (item.SAPBaseUOM.ToLower() == "cs")
                if (item.RetailSellingUnitsBaseUOM != 0)
                {
                    txtCaeseCostTarget.Text = (dUnitCostTarget * item.RetailSellingUnitsBaseUOM).ToString("C", new CultureInfo("en-US"));
                }
                else
                    txtCaeseCostTarget.Text = "NA";
            if (item.RetailUnitWieghtOz != 0)
                txtlbsCostTarget.Text = Utilities.FormatDecimal((dUnitCostTarget / (item.RetailUnitWieghtOz / 16)), 2);
            txtProjectUnit1.Text = Utilities.FormatNumber(item.Month1ProjectedUnits);
            txtProjectUnit2.Text = Utilities.FormatNumber(item.Month2ProjectedUnits);
            txtProjectUnit3.Text = Utilities.FormatNumber(item.Month3ProjectedUnits);

            if (item.SAPBaseUOM.ToLower() == "cs")
            {
                if (item.RetailSellingUnitsBaseUOM != 0)
                {
                    txtProjectCase1.Text = Utilities.FormatNumber(item.Month1ProjectedUnits / item.RetailSellingUnitsBaseUOM);
                    txtProjectCase2.Text = Utilities.FormatNumber(item.Month2ProjectedUnits / item.RetailSellingUnitsBaseUOM);
                    txtProjectCase3.Text = Utilities.FormatNumber(item.Month3ProjectedUnits / item.RetailSellingUnitsBaseUOM);
                }
                else
                {
                    txtProjectCase1.Text = "NA";
                    txtProjectCase2.Text = "NA";
                    txtProjectCase3.Text = "NA";
                }
            }
            else
            {
                txtProjectCase1.Text = "NA";
                txtProjectCase2.Text = "NA";
                txtProjectCase3.Text = "NA";
            }

            txtProjectlbs1.Text = Utilities.FormatNumber((item.Month1ProjectedUnits * item.RetailUnitWieghtOz) / 16);
            txtProjectlbs2.Text = Utilities.FormatNumber((item.Month2ProjectedUnits * item.RetailUnitWieghtOz) / 16);
            txtProjectlbs3.Text = Utilities.FormatNumber((item.Month3ProjectedUnits * item.RetailUnitWieghtOz) / 16);

            Utilities.SetDropDownValue(item.PackSupplierAndDielineSame, ddlPackSupplierAndDielineSame, this.Page);
            txtWhatChangeIsRequiredExtMfg.Text = item.WhatChangeIsRequiredExtMfg;

            hdnMaterialNumber.Value = item.MaterialNumber;
            hdnMaterialDesc.Value = item.MaterialDescriptiom;
            hdnNovelty.Value = item.NoveltyProject;
            hdnComan.Value = item.PHL1;
            hdnPLMProject.Value = item.PLMFlag;
            PLMProject = item.PLMFlag;
            LoadAttachment();
            fixMonth();
            loadPrinterSupplierDets();
        }
        private void loadPrinterSupplierDets()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            dtPackingItem = packagingItemService.GetAllPackagingItemsForProject(iItemId);

            Dictionary<PackagingItem, List<PackagingItem>> TSRepeaterDets = new Dictionary<PackagingItem, List<PackagingItem>>();
            Dictionary<PackagingItem, List<PackagingItem>> PCSRepeaterDets = new Dictionary<PackagingItem, List<PackagingItem>>();
            foreach (PackagingItem item in dtPackingItem.Where(r => r.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || r.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi))
            {
                List<PackagingItem> dtChildPackingItem = new List<PackagingItem>();
                List<PackagingItem> TSPCSOGPackingItems = new List<PackagingItem>();
                List<PackagingItem> TSPCSPackingItems = new List<PackagingItem>();
                dtChildPackingItem =
                    (
                        from
                            childItem in dtPackingItem
                        where
                            childItem.ParentID == item.Id
                            &&
                            childItem.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi
                            &&
                            childItem.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi
                        select
                            childItem
                    ).ToList<PackagingItem>();

                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && dtChildPackingItem.Count > 0)
                {
                    TSRepeaterDets.Add(item, dtChildPackingItem);
                }
                else if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi && dtChildPackingItem.Count > 0)
                {
                    PCSRepeaterDets.Add(item, dtChildPackingItem);
                }
            }
            //For new repeater FG section
            List<PackagingItem> ExtManFGList = new List<PackagingItem>();
            foreach (PackagingItem item in dtPackingItem.Where(r => r.ParentID == 0))
            {
                if (item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi &&
                    item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi &&
                    item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ExtManFGList.Add(item);
                }
            }
            rptPrinterSupplierFG.DataSource = ExtManFGList;
            rptPrinterSupplierFG.DataBind();
            //For new repeater TS section
            try
            {
                rptPrinterSupplierTS.DataSource = TSRepeaterDets;
                rptPrinterSupplierTS.DataBind();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": rptPrinterSupplierTS Binding: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.ExternalMfg.ToString(), "rptPrinterSupplierTS Binding");
            }
            //For new repeater PCS section
            try
            {
                rptPrinterSupplierPCS.DataSource = PCSRepeaterDets;
                rptPrinterSupplierPCS.DataBind();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": rptPrinterSupplierPCS Binding: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.ExternalMfg.ToString(), "rptPrinterSupplierPCS Binding");
            }
        }
        private void LoadBOMGrid()
        {
            ucSemiDetails ctrl2 = (ucSemiDetails)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucSemiDetails.ascx");
            ctrl2.ComponentType = GlobalConstants.COMPONENTTYPE_PurchasedSemi;
            ctrl2.ProjectNumber = ProjectNumber;
            ctrl2.CompassItemId = iItemId;
            ctrl2.ID = "PurchasedCandy";
            phPurchasedSemiFields.Controls.Clear();
            phPurchasedSemiFields.Controls.Add(ctrl2);

            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            phBOM.Controls.Clear();
            dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);

            ucBOMGrid ctrl = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.openBtnSave = openBtnSave;
            phBOM.Controls.Add(ctrl);

            foreach (PackagingItem item in dtPackingItem)
            {
                if (item.PackagingComponent.ToLower().Contains("transfer") || item.PackagingComponent.ToLower().Contains("purchased candy"))
                {
                    ucBOMGrid ctrl3 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                    ctrl3.openBtnSave = openBtnSave;
                    ctrl3.PackagingComponent = item.PackagingComponent;
                    ctrl3.ParentId = item.Id;
                    ctrl3.MaterialNumber = item.MaterialNumber;
                    ctrl3.MaterialDesc = item.MaterialDescription;
                    ctrl3.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOM.Controls.Add(ctrl3);

                    List<PackagingItem> dtChildPackingItem = new List<PackagingItem>();
                    dtChildPackingItem = packagingItemService.GetSemiChildTSBOMItems(iItemId, item.Id, item.PackagingComponent);

                    foreach (PackagingItem childItem in dtChildPackingItem)
                    {
                        ucBOMGrid ctrl4 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                        ctrl4.openBtnSave = openBtnSave;
                        ctrl4.PackagingComponent = childItem.PackagingComponent;
                        ctrl4.ParentId = childItem.Id;
                        ctrl4.MaterialNumber = childItem.MaterialNumber;
                        ctrl4.MaterialDesc = childItem.MaterialDescription;
                        ctrl4.SemiXferMakeLocation = childItem.TransferSEMIMakePackLocations;
                        ctrl4.isChildItem = true;
                        phBOM.Controls.Add(ctrl4);
                    }
                }
            }
            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable)Page.LoadControl(_ucBOMEditable);
                ctrlPE.PackagingComponent = hdnPackagingComponent.Value;
                ctrlPE.PackagingItemId = Convert.ToInt32(hdnPackagingID.Value);
                ctrlPE.ParentID = hdnParentID.Value;
                // Add messages to page
                phMsg.Controls.Add(ctrlPE);
            }
        }
        private void LoadBOMGrid_New()
        {
            ucSemiDetails ctrl3 = (ucSemiDetails)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucSemiDetails.ascx");
            ctrl3.ComponentType = GlobalConstants.COMPONENTTYPE_PurchasedSemi;
            ctrl3.ProjectNumber = ProjectNumber;
            ctrl3.CompassItemId = iItemId;
            ctrl3.ID = "PurchasedCandy";
            phPurchasedSemiFields.Controls.Clear();
            phPurchasedSemiFields.Controls.Add(ctrl3);

            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();

            phBOM.Controls.Clear();
            dtPackingItem = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);

            foreach (BOMSetupItem item in dtPackingItem)
            {
                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBOMGrid_New ctrl2 = (ucBOMGrid_New)Page.LoadControl(_ucBOMGrid_New);
                    ctrl2.ID = "grid" + item.Id.ToString();
                    ctrl2.iItemId = iItemId;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.GridItem = item;
                    //ctrl2.openBtnSave = openBtnSave;
                    phBOM.Controls.Add(ctrl2);
                }
            }
            BOMSetupItem FGItem = new BOMSetupItem()
            {
                PackagingComponent = "Finished Good",
                MaterialNumber = hdnMaterialNumber.Value,
                MaterialDescription = hdnMaterialDesc.Value,
                ParentID = 0
            };
            ucBOMGrid_New ctrl = (ucBOMGrid_New)Page.LoadControl(_ucBOMGrid_New);
            ctrl.ID = "grid" + iItemId.ToString();
            ctrl.iItemId = iItemId;
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.packagingItems = dtPackingItem;
            ctrl.ParentId = 0;
            ctrl.GridItem = FGItem;
            //ctrl.openBtnSave = openBtnSave;
            phBOM.Controls.Add(ctrl);

            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable_New)Page.LoadControl(_ucBOMEditable_New);
                ctrlPE.PackagingComponent = hdnPackagingComponent.Value;
                ctrlPE.PackagingItemId = Convert.ToInt32(hdnPackagingID.Value);
                ctrlPE.ParentID = hdnParentID.Value;
                ctrlPE.AllPIs = dtPackingItem;
                ctrlPE.CompassListItemId = iItemId;
                // Add messages to page
                phMsg.Controls.Add(ctrlPE);
            }
        }
        protected void rptPrinterSupplierFG_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PackagingItem FGItem = (PackagingItem)e.Item.DataItem;
                DropDownList ddlReviewPrinterSupplier = ((DropDownList)e.Item.FindControl("ddlReviewPrinterSupplier"));
                Utilities.SetDropDownValue(FGItem.ReviewPrinterSupplier, ddlReviewPrinterSupplier, Page);
            }
        }
        protected void rptPrinterSupplierTS_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeyValuePair<PackagingItem, List<PackagingItem>> TSKVP = (KeyValuePair<PackagingItem, List<PackagingItem>>)e.Item.DataItem;
                Label lblMatNumber = ((Label)e.Item.FindControl("lblMatNumber"));
                Label lblMatDesc = ((Label)e.Item.FindControl("lblMatDesc"));
                Repeater rptTSChildren = ((Repeater)e.Item.FindControl("rptTSChildren"));
                lblMatNumber.Text = TSKVP.Key.MaterialNumber;
                lblMatDesc.Text = TSKVP.Key.MaterialDescription;
                rptTSChildren.DataSource = TSKVP.Value;
                rptTSChildren.DataBind();
            }
        }
        protected void rptPrinterSupplierPCS_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                KeyValuePair<PackagingItem, List<PackagingItem>> PCSKVP = (KeyValuePair<PackagingItem, List<PackagingItem>>)e.Item.DataItem;
                Label lblMatNumber = ((Label)e.Item.FindControl("lblMatNumber"));
                Label lblMatDesc = ((Label)e.Item.FindControl("lblMatDesc"));
                Repeater rptPCSChildren = ((Repeater)e.Item.FindControl("rptPCSChildren"));
                lblMatNumber.Text = PCSKVP.Key.MaterialNumber;
                lblMatDesc.Text = PCSKVP.Key.MaterialDescription;
                rptPCSChildren.DataSource = PCSKVP.Value;
                rptPCSChildren.DataBind();
            }
        }
        protected void rptTSChildren_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PackagingItem item = ((PackagingItem)e.Item.DataItem);
                DropDownList ddlReviewPrinterSupplier = ((DropDownList)e.Item.FindControl("ddlReviewPrinterSupplier"));
                HiddenField hdnPackagingItemId = ((HiddenField)e.Item.FindControl("hdnPackagingItemId"));
                Utilities.SetDropDownValue(item.ReviewPrinterSupplier, ddlReviewPrinterSupplier, Page);
                hdnPackagingItemId.Value = item.Id.ToString();
            }
        }
        protected void rptPCSChildren_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PackagingItem item = ((PackagingItem)e.Item.DataItem);
                DropDownList ddlReviewPrinterSupplier = ((DropDownList)e.Item.FindControl("ddlReviewPrinterSupplier"));
                HiddenField hdnPackagingItemId = ((HiddenField)e.Item.FindControl("hdnPackagingItemId"));
                Utilities.SetDropDownValue(item.ReviewPrinterSupplier, ddlReviewPrinterSupplier, Page);
                hdnPackagingItemId.Value = item.Id.ToString();
            }
        }
        private void fixMonth()
        {
            if (!string.IsNullOrEmpty(lblRevisedFirstShipDate.Text))
            {
                string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

                DateTime date = Convert.ToDateTime(lblRevisedFirstShipDate.Text);
                int selectedMonth = date.Month - 1;
                lbl1stmonthU.InnerText = months[selectedMonth] + " Projected Retail Units:";
                lbl1stmonthC.InnerText = months[selectedMonth] + " Projected Cases:";
                lbl1stmonthL.InnerText = months[selectedMonth] + " Projected lbs:";


                if (selectedMonth + 1 > 11)
                    selectedMonth = -1;

                lbl2ndmonthU.InnerText = months[selectedMonth + 1] + " Projected Retail Units:";
                lbl2ndmonthC.InnerText = months[selectedMonth + 1] + " Projected Cases:";
                lbl2ndmonthL.InnerText = months[selectedMonth + 1] + " Projected lbs:";

                if (selectedMonth + 2 > 11)
                    selectedMonth = -2;
                lbl3rdmonthU.InnerText = months[selectedMonth + 2] + " Projected Retail Units:";
                lbl3rdmonthC.InnerText = months[selectedMonth + 2] + " Projected Cases:";
                lbl3rdmonthL.InnerText = months[selectedMonth + 2] + " Projected lbs:";
            }


        }
        private void LoadAttachment()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_NLEA);
            if (files.Count > 0)
            {
                rpNLEAs.Visible = true;
                rpNLEAs.DataSource = files;
                rpNLEAs.DataBind();
            }
            else
            {
                rpNLEAs.Visible = false;
            }

            var filesDieline = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_Dieline);
            if (filesDieline.Count > 0)
            {
                rpDielines.Visible = true;
                rpDielines.DataSource = filesDieline;
                rpDielines.DataBind();
            }
            else
            {
                rpDielines.Visible = false;
            }
            var filesPackSpecs = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_PACKSPECS);
            if (filesPackSpecs.Count > 0)
            {
                rpPackSpecs.Visible = true;
                rpPackSpecs.DataSource = filesPackSpecs;
                rpPackSpecs.DataBind();
            }
            else
            {
                rpPackSpecs.Visible = false;
            }

            #region Approved Graphics Asset
            var ApprovedGraphicsAssets = BOMSetupService.GetUploadedFiles(ProjectNumber, GlobalConstants.DOCTYPE_ApprovedGraphicsAsset);

            if (ApprovedGraphicsAssets.Count > 0)
            {
                //btnApprovedGraphicsAsset.Visible = false;
                rptApprovedGraphicsAsset.Visible = true;
                rptApprovedGraphicsAsset.DataSource = ApprovedGraphicsAssets;
                rptApprovedGraphicsAsset.DataBind();
            }
            else
            {
                rptApprovedGraphicsAsset.Visible = false;
                //btnApprovedGraphicsAsset.Visible = true;
            }
            #endregion
        }
        public void SaveuserControls()
        {
            foreach (var ctrl in phPurchasedSemiFields.Controls)
            {
                if (ctrl is ucSemiDetails)
                {
                    var type2 = (ucSemiDetails)ctrl;

                    type2.saveData();

                }
            }
        }
        public void openBtnSave()
        {
            if (!userMgmtService.HasWriteAccess(CompassForm.ExternalMfg))
            {
                ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                return;
            }
            if (iItemId <= 0)
            {
                ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                return;
            }
            ExternalManufacturingItem item = ConstructFormData();
            externalMfgService.UpdateExternalManufacturingItem(item);

            ApprovalItem approvalItem = ConstructApprovalData();
            externalMfgService.UpdateExternalManufacturingApprovalItem(approvalItem, false);
        }
        private ExternalManufacturingItem ConstructFormData()
        {
            try { SaveuserControls(); }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.ExternalMfg.ToString(), "SaveuserControls");
            }
            try
            {
                List<PackagingItem> procItems = new List<PackagingItem>();
                foreach (RepeaterItem rptItem in rptPrinterSupplierFG.Items)
                {
                    if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                    {
                        PackagingItem packagingItem = new PackagingItem();

                        packagingItem.ReviewPrinterSupplier = ((DropDownList)rptItem.FindControl("ddlReviewPrinterSupplier")).SelectedItem.Text;
                        packagingItem.Id = Convert.ToInt32(((HiddenField)rptItem.FindControl("hdnPackagingItemId")).Value);
                        try
                        {
                            externalMfgService.updateProcPrinterSupplier(packagingItem);
                        }
                        catch (Exception exception)
                        {
                            ErrorSummary.AddError("ID: " + packagingItem.Id.ToString(), this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": " + exception.Message);
                            exceptionService.Handle(LogCategory.CriticalError, exception, "ID: " + packagingItem.Id.ToString(), "Saving Printer Supplier Details");
                        }
                    }
                }
                foreach (RepeaterItem rptItem in rptPrinterSupplierTS.Items)
                {
                    if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                    {
                        Repeater childRepeater = (Repeater)rptItem.FindControl("rptTSChildren");
                        foreach (RepeaterItem childItem in childRepeater.Items)
                        {
                            if (childItem.ItemType == ListItemType.Item || childItem.ItemType == ListItemType.AlternatingItem)
                            {
                                PackagingItem packagingItem = new PackagingItem();

                                packagingItem.ReviewPrinterSupplier = ((DropDownList)childItem.FindControl("ddlReviewPrinterSupplier")).SelectedItem.Text;
                                packagingItem.Id = Convert.ToInt32(((HiddenField)childItem.FindControl("hdnPackagingItemId")).Value);
                                try
                                {
                                    externalMfgService.updateProcPrinterSupplier(packagingItem);
                                }
                                catch (Exception exception)
                                {
                                    ErrorSummary.AddError("ID: " + packagingItem.Id.ToString(), this.Page);
                                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": " + exception.Message);
                                    exceptionService.Handle(LogCategory.CriticalError, exception, "ID: " + packagingItem.Id.ToString(), "Saving Printer Supplier Details");
                                }
                            }
                        }
                    }
                }
                foreach (RepeaterItem rptItem in rptPrinterSupplierPCS.Items)
                {
                    if (rptItem.ItemType == ListItemType.Item || rptItem.ItemType == ListItemType.AlternatingItem)
                    {
                        Repeater childRepeater = (Repeater)rptItem.FindControl("rptPCSChildren");
                        foreach (RepeaterItem childItem in childRepeater.Items)
                        {
                            if (childItem.ItemType == ListItemType.Item || childItem.ItemType == ListItemType.AlternatingItem)
                            {
                                PackagingItem packagingItem = new PackagingItem();

                                packagingItem.ReviewPrinterSupplier = ((DropDownList)childItem.FindControl("ddlReviewPrinterSupplier")).SelectedItem.Text;
                                packagingItem.Id = Convert.ToInt32(((HiddenField)childItem.FindControl("hdnPackagingItemId")).Value);
                                try
                                {
                                    externalMfgService.updateProcPrinterSupplier(packagingItem);
                                }
                                catch (Exception exception)
                                {
                                    ErrorSummary.AddError("ID: " + packagingItem.Id.ToString(), this.Page);
                                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": " + exception.Message);
                                    exceptionService.Handle(LogCategory.CriticalError, exception, "ID: " + packagingItem.Id.ToString(), "Saving Printer Supplier Details");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.ExternalMfg.ToString(), "Saving Printer Supplier Details");
            }
            var item = new ExternalManufacturingItem();
            item.CompassListItemId = iItemId;
            item.LastUpdatedFormName = CompassForm.ExternalMfg.ToString();
            if (peProjectLead.Entities.Count > 0)
                item.ExternalMfgProjectLead = Utilities.GetPeopleFromPickerControl(peProjectLead, SPContext.Current.Web).ToString();
            item.ExistingBulkSemiNumber = txtExistingBulkSemiNumber.Text;
            item.BulkSemiDescription = txtBulkSemiDescription.Text;
            item.CoManufacturingClassification = ddlCoManufacturingClassification.SelectedItem.Text;
            item.DoesBulkSemiExistToBringInHouse = ddlDoesBulkSemiExistToBringInHouse.SelectedItem.Text;
            item.ExternalManufacturer = ddlExternalManufacturer.SelectedItem.Text;
            item.ManufacturerCountryOfOrigin = ddlManufacturerCountryOfOrigin.SelectedItem.Text;
            item.ExternalPacker = ddlExternalPacker.SelectedItem.Text;
            item.PurchasedIntoLocation = ddlPurchasedIntoLocation.SelectedItem.Text;

            item.CurrentTimelineAcceptable = ddlCurrentTimelineAcceptable.SelectedItem.Text;
            item.LeadTimeFromSupplier = ddlLeadTimeFromSupplier.SelectedItem.Text;
            item.FinalArtworkDueToSupplier = Utilities.GetDateFromField(txtFinalArtworkDueToSupplier.Text);
            item.PackSupplierAndDielineSame = ddlPackSupplierAndDielineSame.SelectedItem.Text;
            item.WhatChangeIsRequiredExtMfg = txtWhatChangeIsRequiredExtMfg.Text;
            return item;
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
        protected void lnkDielineDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                LoadAttachment();
            }
        }
        protected void lnkNLEADelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                LoadAttachment();
            }
        }
        protected void lnkPACKSPECSDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                LoadAttachment();
            }
        }
        protected void lnkDeleteApprovedGraphicsAsset_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                LoadAttachment();
            }
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            LoadAttachment();
        }
        #endregion

        #region Button Methods
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "External Manufacturing");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                openBtnSave();
                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.ExternalMfg.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.ExternalMfg))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                if (!ValidateForm())
                    return;

                ExternalManufacturingItem item = ConstructFormData();
                externalMfgService.UpdateExternalManufacturingItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                externalMfgService.UpdateExternalManufacturingApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.ExternalMfg);

                if (hdnProjectType.Value == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly && ddlPackSupplierAndDielineSame.SelectedItem.Text == "No")
                {
                    var ProjectCancelReasson = txtWhatChangeIsRequiredExtMfg.Text;
                    Utilities.UpdateProjectRejectionReason(iItemId, ProjectCancelReasson, GlobalConstants.PAGE_ExternalManufacturing);
                    Page.Response.Redirect(
                        string.Concat(
                            SPContext.Current.Web.Url,
                            "/Pages/ItemProposal.aspx?ProjectNo=",
                            ProjectNumber,
                            "&IPFMode=",
                            GlobalConstants.QUERYSTRINGVALUE_IPFChange,
                            "&", GlobalConstants.QUERYSTRING_ProjectRejected, "=",
                            "Yes"
                            ), false);
                }
                else
                {
                    // Redirect to Home page after successfull Submit                        
                    Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
                }
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.ExternalMfg.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.ExternalMfg.ToString(), "btnSave_Click");
            }
        }
        #endregion
    }
}
