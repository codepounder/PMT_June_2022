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
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;

namespace Ferrara.Compass.WebParts.QAForm
{
    [ToolboxItemAttribute(false)]
    public partial class QAForm : WebPart
    {
        #region Member Variables
        private IExternalManufacturingService externalMfgService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private int iItemId = 0;
        private string materialGroup5 = "";
        private INotificationService notificationService;
        private IQAService qaService;
        private ISAPBOMService sapBOMService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IMixesService mixesService;
        private IItemProposalService itemProposalService;
        private string webUrl;
        private IBOMSetupService BOMSetupService;
        private IPackagingItemService PackagingItemService;
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
        public QAForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            externalMfgService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            qaService = DependencyResolution.DependencyMapper.Container.Resolve<IQAService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            sapBOMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            PackagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
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

                    InitializeScreen();
                    // Utilities.BindDropDownItems(drpClaimBioEngineering, GlobalConstants.LIST_BioEngineeringLabelingcceptableLookup, webUrl);

                    LoadFormData();
                    LoadAttachment();
                    bindCandySemiData(false);
                    bindPurchasedSemiData(false);
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.QA.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.QA.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
                materialGroup5 = hdnMaterialGroup5.Value;
                PLMProject = hdnPLMProject.Value;
            }
            if (iItemId > 0)
            {
                if (!string.Equals(PLMProject, "Yes"))
                {
                    LoadBOMGrid();
                }
                else
                {
                    LoadBOMGrid_New();
                }
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
            // If user does not belong to a valid group for the page, inform them that they do not have access rights
            if (!userMgmtService.HasReadAccess(CompassForm.QA))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.QA))
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
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            List<ShipperFinishedGoodItem> shipperData;
            List<MixesItem> mixData;
            String MaterialGroup4ProductForm = null, MaterialGroup5PackType = null;
            QAItem item = qaService.GetQAItem(iItemId);
            ExternalManufacturingItem itemExt = externalMfgService.GetExternalManufacturingItem(iItemId);
            MarketingClaimsItem marketingItem = itemProposalService.GetMarketingClaimsItem(iItemId);

            this.hdnPLMProject.Value = itemExt.PLMFlag;
            PLMProject = itemExt.PLMFlag;
            this.hdnDesiredClaims.Value = marketingItem.ClaimsDesired;
            if (item.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblComments.Text = "Change Notes:";
            }
            else
            {
                lblComments.Text = "Item Concept:";
            }
            this.txtComments.Text = item.ItemConcept;

            this.hdnTBDIndicator.Value = item.TBDIndicator;
            this.hdnMaterialNumber.Value = item.SAPItemNumber;
            this.hdnMaterialDesc.Value = item.SAPDescription;
            this.lblManufacturingCountryOfOrigin.Text = item.MakeCountryOfOrigin;
            this.lblManufacturingLocation.Text = item.ManufacturingLocation;
            this.hiddenManufacturingLocation.Value = item.ManufacturingLocation;
            this.lblPrimaryPackLocation.Text = item.PackingLocation;
            hdnProjectType.Value = item.ProjectType;
            hdnPageName.Value = GlobalConstants.PAGE_QA;
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

            if (lblManufacturingLocation.Text.ToLower().Contains("externally") && (!string.IsNullOrEmpty(itemExt.ExternalManufacturer) && !string.Equals(itemExt.ExternalManufacturer, "Select...")))
            {
                lblManufacturingLocation.Text = itemExt.ExternalManufacturer;
            }

            if ((lblPrimaryPackLocation.Text.ToLower().Contains("externally")) && (!string.IsNullOrEmpty(itemExt.ExternalPacker) && !string.Equals(itemExt.ExternalPacker, "Select...")))
            {
                lblPrimaryPackLocation.Text = itemExt.ExternalPacker;
            }

            getMaterialGroups(iItemId, ref MaterialGroup4ProductForm, ref MaterialGroup5PackType);
            if (MaterialGroup5PackType.ToLower() == "shipper (shp)" || MaterialGroup5PackType.ToLower() == "shippers (shp)")
            {
                shipperData = GetShipperFGItem(iItemId);
                if (shipperData.Count > 0)
                {
                    divShipper.Visible = true;
                    rptShipper.DataSource = shipperData;
                    rptShipper.DataBind();
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

            var FGItems = GetFGItems(iItemId);
            if (FGItems.Count > 0)
            {
                divFG.Visible = true;
                rptFG.DataSource = FGItems;
                rptFG.DataBind();
            }

            this.hdnMaterialGroup5.Value = MaterialGroup5PackType.ToLower();
            materialGroup5 = MaterialGroup5PackType.ToLower();
            this.txtCaseType.Text = item.CaseType;
            this.txtCoManufacturingClassification.Text = coManClass;
            this.lblPM.Text = item.PMName;
            this.lblBrandManager.Text = item.MarketingName; //Utilities.GetPersonFieldForDisplay(item.BrandManager);
            this.lblMarketingClaims.Text = item.MarketingClaimsLabeling;
            //Marketing Clams Details
            txtSellableUnit.Text = marketingItem.SellableUnit;
            txtNewNLEAFormat.Text = marketingItem.NewNLEAFormat;
            txtMadeInUSA.Text = marketingItem.MadeInUSAClaim;
            txtMadeInUSAPct.Text = marketingItem.MadeInUSAClaimDets;
            txtGMOClaim.Text = marketingItem.GMOClaim;
            txtGlutenFree.Text = marketingItem.GlutenFree;
            txtOrganic.Text = marketingItem.Organic;
            txtNaturalColors.Text = marketingItem.NaturalColors;
            txtNaturalFlavors.Text = marketingItem.NaturalFlavors;
            txtFatFree.Text = marketingItem.FatFree;
            txtPreservativeFree.Text = marketingItem.PreservativeFree;
            txtLactoseFree.Text = marketingItem.LactoseFree;
            txtLowSodium.Text = marketingItem.LowSodium;
            txtKosher.Text = marketingItem.Kosher;
            txtJuiceConcentrate.Text = marketingItem.JuiceConcentrate;
            txtGoodSource.Text = marketingItem.GoodSource;
            Utilities.SetDropDownValue(marketingItem.AllergenAlmonds, this.drpAllergenAlmonds, this.Page);
            Utilities.SetDropDownValue(marketingItem.AllergenCoconut, this.drpAllergenCoconut, this.Page);
            Utilities.SetDropDownValue(marketingItem.AllergenEggs, this.drpAllergenEggs, this.Page);
            Utilities.SetDropDownValue(marketingItem.AllergenMilk, this.drpAllergenMilk, this.Page);
            Utilities.SetDropDownValue(marketingItem.AllergenPeanuts, this.drpAllergenPeanuts, this.Page);
            Utilities.SetDropDownValue(marketingItem.AllergenSoy, this.drpAllergenSoy, this.Page);
            Utilities.SetDropDownValue(marketingItem.AllergenWheat, this.drpAllergenWheat, this.Page);
            Utilities.SetDropDownValue(marketingItem.AllergenHazelNuts, this.drpAllergenHazelnuts, this.Page);
            Utilities.SetDropDownValue(item.IsRegulatoryinformationCorrect, this.ddlIsRegulatoryinformationCorrect, this.Page);
            txtWhatRegulatoryInfoIsIncorrect.Text = item.WhatRegulatoryInfoIsIncorrect;
            Utilities.SetDropDownValue(item.DoYouApproveThisProjectToProceed, this.ddlDoYouApproveThisProjectToProceed, this.Page);
            //Utilities.SetDropDownValue(marketingItem.ClaimBioEngineering, this.drpClaimBioEngineering, this.Page);
            drpAllergenOther.Text = marketingItem.AllergenOther;
        }
        private void getMaterialGroups(int itemId, ref string MaterialGroup4ProductForm, ref string MaterialGroup5PackType)
        {
            ItemProposalItem item;
            if (ViewState["MaterialGroup4ProductForm"] == null || ViewState["MaterialGroup5PackType"] == null)
            {
                item = itemProposalService.GetItemProposalItem(iItemId);
                hddRetailSellingUnitsPerBaseUOM.Value = item.RetailSellingUnitsBaseUOM == -9999 ? "0" : item.RetailSellingUnitsBaseUOM.ToString();
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
        private List<MixesItem> GetMixesItem(int itemId)
        {
            List<MixesItem> dtMixesItem;
            if (ViewState["MixesItemTable"] == null)
            {
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
            if (ViewState["FGShipperItemTable"] == null)
            {
                dtFGItem = shipperFinishedGoodService.GetShipperFinishedGoodItems(itemId);
                ViewState["FGShipperItemTable"] = dtFGItem;
            }
            else
                dtFGItem = (List<ShipperFinishedGoodItem>)ViewState["FGShipperItemTable"];
            return dtFGItem;
        }
        protected void rptShipper_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ShipperFinishedGoodItem shipItem;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                shipItem = (ShipperFinishedGoodItem)e.Item.DataItem;
                DropDownList ddlFGPackUnit = ((DropDownList)e.Item.FindControl("ddlFGPackUnit"));
                Utilities.BindDropDownItems(ddlFGPackUnit, GlobalConstants.LIST_PackUnitLookup, webUrl);
                Utilities.SetDropDownValue(shipItem.FGPackUnit, ddlFGPackUnit, Page);

                TextBox txtFGShelfLife = ((TextBox)e.Item.FindControl("txtFGShelfLife"));
                txtFGShelfLife.Text = shipItem.FGShelfLife;
                DropDownList ddlIngredientsNeedToClaimBioEng = ((DropDownList)e.Item.FindControl("ddlIngredientsNeedToClaimBioEng"));
                Utilities.SetDropDownValue(shipItem.IngredientsNeedToClaimBioEng, ddlIngredientsNeedToClaimBioEng, Page);
            }
        }
        private List<PackagingItem> GetFGItems(int itemId)
        {
            List<PackagingItem> dtFGItem;
            if (ViewState["FGItemTable"] == null)
            {
                dtFGItem = qaService.GetFinishedGoodItems(itemId);
                ViewState["FGItemTable"] = dtFGItem;
            }
            else
                dtFGItem = (List<PackagingItem>)ViewState["FGItemTable"];
            return dtFGItem;
        }
        protected void rptFG_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            PackagingItem FGItem;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                FGItem = (PackagingItem)e.Item.DataItem;
                DropDownList ddlFGPackUnit = ((DropDownList)e.Item.FindControl("ddlFGPackUnit"));
                Utilities.BindDropDownItems(ddlFGPackUnit, GlobalConstants.LIST_PackUnitLookup, webUrl);
                Utilities.SetDropDownValue(FGItem.PackUnit, ddlFGPackUnit, Page);

                TextBox txtFGShelfLife = ((TextBox)e.Item.FindControl("txtFGShelfLife"));
                txtFGShelfLife.Text = FGItem.ShelfLife;

                DropDownList ddlIngredientsNeedToClaimBioEng = ((DropDownList)e.Item.FindControl("ddlIngredientsNeedToClaimBioEng"));
                Utilities.SetDropDownValue(FGItem.IngredientsNeedToClaimBioEng, ddlIngredientsNeedToClaimBioEng, Page);
            }
        }
        private bool RequiredFieldCheckForPackagingComponent()
        {
            List<string> completedCompIds = new List<string>();
            if (hdnComponentStatusChangeIds.Value != "")
            {
                completedCompIds = hdnComponentStatusChangeIds.Value.Split(',').ToList();
            }
            List<PackagingItem> totalPackingItem = new List<PackagingItem>();
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);
            bool iserror = false;
            totalPackingItem.AddRange(dtPackingItem);
            completedCompIds = completedCompIds.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
            foreach (PackagingItem item in dtPackingItem)
            {
                if (item.PackagingComponent.ToLower().Contains("transfer") || item.PackagingComponent.ToLower().Contains("purchased"))
                {
                    List<PackagingItem> dtSemiPackingItem = new List<PackagingItem>();
                    dtSemiPackingItem = packagingItemService.GetSemiBOMItems(iItemId, item.Id);
                    totalPackingItem.AddRange(dtSemiPackingItem);
                }
            }

            foreach (var item in totalPackingItem)
            {
                int isCompleted = (from id in completedCompIds where Convert.ToInt32(id) == item.Id select id).Count();
                if (isCompleted <= 0)
                {
                    ErrorSummary.AddError("Please complete component information: " + item.PackagingComponent, this.Page);
                    iserror = true;
                }
            }
            return iserror;
        }
        private MarketingClaimsItem ConstructMarketingData()
        {
            var item = new MarketingClaimsItem();

            try
            {
                item.CompassListItemId = iItemId;
                item.Title = ProjectNumber;
                item.AllergenAlmonds = drpAllergenAlmonds.SelectedItem.Text.Trim();
                item.AllergenCoconut = drpAllergenCoconut.SelectedItem.Text.Trim();
                item.AllergenEggs = drpAllergenEggs.SelectedItem.Text.Trim();
                item.AllergenMilk = drpAllergenMilk.SelectedItem.Text.Trim();
                item.AllergenPeanuts = drpAllergenPeanuts.SelectedItem.Text.Trim();
                item.AllergenSoy = drpAllergenSoy.SelectedItem.Text.Trim();
                item.AllergenWheat = drpAllergenWheat.SelectedItem.Text.Trim();
                item.AllergenHazelNuts = drpAllergenHazelnuts.SelectedItem.Text.Trim();
                item.AllergenOther = drpAllergenOther.Text.Trim();
                // item.ClaimBioEngineering = drpClaimBioEngineering.SelectedItem.Text;
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Unexpected Error Occurred: ConstructMarketingData", this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.QA.ToString() + ": " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.QA.ToString(), "ConstructMarketingData");
                return null;
            }

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
        private QAItem ConstructRegulatoryData()
        {
            var item = new QAItem();

            try
            {
                item.CompassListItemId = iItemId;
                item.IsRegulatoryinformationCorrect = ddlIsRegulatoryinformationCorrect.SelectedItem.Text.Trim();
                item.WhatRegulatoryInfoIsIncorrect = txtWhatRegulatoryInfoIsIncorrect.Text;
                item.DoYouApproveThisProjectToProceed = ddlDoYouApproveThisProjectToProceed.SelectedItem.Text.Trim();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Unexpected Error Occurred: ConstructRegulatoryData", this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.QA.ToString() + ": " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.QA.ToString(), "ConstructRegulatoryData");
                return null;
            }

            return item;
        }
        private Boolean SavePackagingItem(int deleteID)
        {
            DropDownList ddlTransfParent;

            foreach (RepeaterItem item in rptCandy.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PackagingItem packagingItem = new PackagingItem();
                    packagingItem.PackagingComponent = GlobalConstants.COMPONENTTYPE_CandySemi;
                    string newExisting = ((DropDownList)item.FindControl("drpNewExisting")).SelectedItem.Text;
                    packagingItem.NewExisting = newExisting;

                    packagingItem.MaterialNumber = ((TextBox)item.FindControl("txtMaterialNumber")).Text;
                    packagingItem.MaterialDescription = ((TextBox)item.FindControl("txtMaterialDescription")).Text;
                    if (newExisting.ToLower() != "new")
                    {
                        packagingItem.CurrentLikeItem = "Not Applicable";
                        packagingItem.CurrentLikeItemDescription = "Not Applicable";
                    }
                    packagingItem.CompassListItemId = iItemId.ToString();

                    packagingItem.NewFormula = ((DropDownList)item.FindControl("ddlNewFormula")).SelectedItem.Text;
                    packagingItem.TrialsCompleted = ((DropDownList)item.FindControl("ddlTrialsCompleted")).SelectedItem.Text;

                    HiddenField hdnFlowthrough = ((HiddenField)item.FindControl("hdnFlowthrough"));
                    if (hdnFlowthrough == null)
                    {
                        packagingItem.Flowthrough = ((DropDownList)item.FindControl("ddlFlowthrough")).SelectedItem.Text;
                    }
                    else
                    {
                        packagingItem.Flowthrough = hdnFlowthrough.Value;
                    }
                    packagingItem.IngredientsNeedToClaimBioEng = ((DropDownList)item.FindControl("ddlIngredientsNeedToClaimBioEng")).SelectedItem.Text;

                    packagingItem.ShelfLife = ((TextBox)item.FindControl("txtShelfLife")).Text;
                    packagingItem.Id = Convert.ToInt32(((HiddenField)item.FindControl("hdnItemID")).Value);
                    ddlTransfParent = (DropDownList)item.FindControl("ddlTransfParent");
                    packagingItem.ParentID = Convert.ToInt32((ddlTransfParent).SelectedValue);
                    if (deleteID != 0 && deleteID == packagingItem.Id)
                    {
                        packagingItemService.DeletePackagingItem(packagingItem.Id);
                    }
                    else if (packagingItem.Id < 0 && deleteID == 0)
                    {
                        int newId = packagingItemService.InsertPackagingItem(packagingItem, iItemId);
                        HiddenField hidId = ((HiddenField)item.FindControl("hidNewId"));
                        if (hidId != null)
                            hidId.Value = newId.ToString();
                    }
                    else
                    {
                        qaService.UpdatePackagingItem(packagingItem);
                    }
                }
            }
            foreach (RepeaterItem item in rptPurchased.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PackagingItem packagingItem = new PackagingItem();
                    packagingItem.PackagingComponent = GlobalConstants.COMPONENTTYPE_PurchasedSemi;
                    string newExisting = ((DropDownList)item.FindControl("drpNewExisting")).SelectedItem.Text;
                    packagingItem.NewExisting = newExisting;

                    packagingItem.MaterialNumber = ((TextBox)item.FindControl("txtMaterialNumber")).Text;
                    packagingItem.MaterialDescription = ((TextBox)item.FindControl("txtMaterialDescription")).Text;
                    if (newExisting.ToLower() != "new")
                    {
                        packagingItem.CurrentLikeItem = "Not Applicable";
                        packagingItem.CurrentLikeItemDescription = "Not Applicable";
                    }
                    packagingItem.CompassListItemId = iItemId.ToString();

                    packagingItem.NewFormula = ((DropDownList)item.FindControl("ddlNewFormula")).SelectedItem.Text;
                    packagingItem.TrialsCompleted = ((DropDownList)item.FindControl("ddlTrialsCompleted")).SelectedItem.Text;
                    packagingItem.Flowthrough = ((DropDownList)item.FindControl("ddlFlowthrough")).SelectedItem.Text;
                    packagingItem.IngredientsNeedToClaimBioEng = ((DropDownList)item.FindControl("ddlIngredientsNeedToClaimBioEng")).SelectedItem.Text;
                    packagingItem.ShelfLife = ((TextBox)item.FindControl("txtShelfLife")).Text;

                    packagingItem.Id = Convert.ToInt32(((HiddenField)item.FindControl("hdnItemID")).Value);
                    ddlTransfParent = (DropDownList)item.FindControl("ddlTransfParent");
                    packagingItem.ParentID = Convert.ToInt32((ddlTransfParent).SelectedValue);
                    if (deleteID != 0 && deleteID == packagingItem.Id)
                    {
                        packagingItemService.DeletePackagingItem(packagingItem.Id);
                    }
                    else if (packagingItem.Id < 0 && deleteID == 0)
                    {
                        int newId = packagingItemService.InsertPackagingItem(packagingItem, iItemId);
                        HiddenField hidId = ((HiddenField)item.FindControl("hidNewId"));
                        if (hidId != null)
                            hidId.Value = newId.ToString();
                    }
                    else
                    {
                        qaService.UpdatePackagingItem(packagingItem);
                    }
                }
            }
            if (materialGroup5.ToLower() == "shipper (shp)" || materialGroup5.ToLower() == "shippers (shp)")
            {
                List<ShipperFinishedGoodItem> dtFGItem = new List<ShipperFinishedGoodItem>();
                foreach (RepeaterItem item in rptShipper.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var objFGItem = new ShipperFinishedGoodItem
                        {
                            ItemId = Convert.ToInt32(((HiddenField)item.FindControl("hidShipperId")).Value),
                            FGShelfLife = ((TextBox)item.FindControl("txtFGShelfLife")).Text,
                            IngredientsNeedToClaimBioEng = ((DropDownList)item.FindControl("ddlIngredientsNeedToClaimBioEng")).SelectedItem.Text,
                            CompassListItemId = iItemId
                        };
                        dtFGItem.Add(objFGItem);
                    }

                }
                shipperFinishedGoodService.UpdateShipperFinishedGoodShelfLife(dtFGItem, ProjectNumber);
            }

            #region FG 
            List<PackagingItem> dtFGItems = new List<PackagingItem>();
            foreach (RepeaterItem item in rptFG.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var objFGItem = new PackagingItem
                    {
                        Id = Convert.ToInt32(((HiddenField)item.FindControl("hidFGId")).Value),
                        ShelfLife = ((TextBox)item.FindControl("txtFGShelfLife")).Text,
                        IngredientsNeedToClaimBioEng = ((DropDownList)item.FindControl("ddlIngredientsNeedToClaimBioEng")).SelectedItem.Text,
                        CompassListItemId = iItemId.ToString()
                    };
                    dtFGItems.Add(objFGItem);
                }

            }
            qaService.UpdateFinishedGoodShelfLife(dtFGItems, ProjectNumber);
            #endregion
            bindCandySemiData(false);
            bindPurchasedSemiData(false);
            return true;
        }
        #endregion

        #region Attachment Methods
        protected void lnkFileDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                LoadAttachment();
            }
        }
        private void LoadAttachment()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_NLEA);
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
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            LoadAttachment();
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
        #endregion

        #region Lookup Methods
        protected void btnFind_Click(object sender, EventArgs e)
        {
            string sapNumber = string.Empty;
            RepeaterItem clickedItem = null;
            foreach (RepeaterItem item in rptCandy.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    sapNumber = ((TextBox)item.FindControl("txtMaterialNumber")).Text;
                    clickedItem = item;
                    if (string.IsNullOrEmpty(sapNumber))
                    {
                        // Set Error   
                        CandySemiError_Message.Controls.Add(new LiteralControl("<p style='color:#FF0000;'>Please enter a valid Candy Semi #</p>"));
                        return;
                    }
                }
            }
            SetLookupValues(sapNumber, clickedItem);
        }
        private void SetLookupValues(string sapNumber, RepeaterItem clickedItem)
        {
            List<SAPBOMListItem> lstSemis = sapBOMService.GetCandySemis(sapNumber);
            if (lstSemis == null || lstSemis.Count < 1)
            {
                ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Search Failed:</p></strong><br/>";
                ErrorSummary.AddError("Candy Semi # could not be found!  Please try again.", this.Page);
                return;
            }

            ((TextBox)clickedItem.FindControl("txtMaterialDescription")).Text = lstSemis[0].MaterialDescription;
        }
        #endregion

        #region Button Methods
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "InTech Regulatory");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        public void openBtnSave()
        {
            if (!userMgmtService.HasWriteAccess(CompassForm.QA))
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
            MarketingClaimsItem mcItem = ConstructMarketingData();
            qaService.UpdateMarketingClaimsItem(mcItem);

            QAItem qaItem = ConstructRegulatoryData();
            qaService.UpdateRegulatoryItem(qaItem);

            SavePackagingItem(0);
            ApprovalItem approvalItem = ConstructApprovalData();
            qaService.UpdateQAApprovalItem(approvalItem, false);

            lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                openBtnSave();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.QA.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.QA.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.QA))
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                // CheckCandySemi();
                // Retrieve the data from the form

                /*var iserror = RequiredFieldCheckForPackagingComponent();

                if (iserror)
                {
                    ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Submit Failed:</p></strong><br/>";
                    return;
                }*/
                if (ddlDoYouApproveThisProjectToProceed.SelectedItem.Text != "No")
                {
                    var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_NLEA);
                    if (files.Count <= 0 && (materialGroup5 != "shipper (shp)" && materialGroup5 != "shippers (shp)"))
                    {
                        ErrorSummary.AddError("NLEA Document is Required!", this.Page);
                        return;
                    }
                }
                MarketingClaimsItem mcItem = ConstructMarketingData();
                qaService.UpdateMarketingClaimsItem(mcItem);

                QAItem qaItem = ConstructRegulatoryData();
                qaService.UpdateRegulatoryItem(qaItem);

                SavePackagingItem(0);

                var packagingItems = PackagingItemService.GetAllPackagingItemsForProject(iItemId);

                int CandySemisCount =
                    (
                        from
                            packagingItem in packagingItems
                        where
                            packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_CandySemi
                        select
                            packagingItem
                    ).Count();

                int PurchasedCandySemisCount =
                    (
                        from
                            packagingItem in packagingItems
                        where
                            packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi
                        select
                            packagingItem
                    ).Count();

                int FinishedGoodsCount =
                    (
                        from
                            packagingItem in packagingItems
                        where
                            packagingItem.PackagingComponent.ToLower().Contains("finished good")
                        select
                            packagingItem
                    ).Count();

                if (CandySemisCount < 1 && PurchasedCandySemisCount < 1 && FinishedGoodsCount < 1)
                {
                    ErrorSummary.AddError("Please add a Candy Semi, a Purchased Candy Semi or a Finished Good!", this.Page);
                    return;
                }

                ApprovalItem approvalItem = ConstructApprovalData();
                qaService.UpdateQAApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.QA);

                if (hdnProjectType.Value == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly && ddlDoYouApproveThisProjectToProceed.SelectedItem.Text == "No")
                {
                    var ProjectCancelReasson = txtWhatRegulatoryInfoIsIncorrect.Text;
                    Utilities.UpdateProjectRejectionReason(iItemId, ProjectCancelReasson, GlobalConstants.PAGE_QA);
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
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.QA.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.QA.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

        protected void btnAddItem_Click(object sender, EventArgs e)
        {
            SavePackagingItem(0);
            bindCandySemiData(true);

        }
        protected void btnAddPurchasedSemi_Click(object sender, EventArgs e)
        {
            SavePackagingItem(0);
            bindPurchasedSemiData(true);
        }

        public void bindCandySemiData(bool extraRow)
        {
            var candylst = qaService.GetCandySemiForProject(iItemId);
            if (extraRow)
            {
                PackagingItem blankItem = new PackagingItem();
                blankItem.NewExisting = "";
                blankItem.NewFormula = "";
                blankItem.MaterialNumber = "";
                blankItem.MaterialDescription = "";
                blankItem.TrialsCompleted = "";
                blankItem.ShelfLife = "";

                int newId = packagingItemService.InsertPackagingItem(blankItem, iItemId);
                blankItem.Id = newId;
                candylst.Add(blankItem);
            }

            rptCandy.DataSource = candylst;
            rptCandy.DataBind();
        }

        protected void rptCandy_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                if (id > 0)
                {
                    SavePackagingItem(id);
                }
            }
        }

        protected void rptCandy_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList ddlPackagingComponent, ddlNewFormula, ddlTrialsCompleted, ddlTransfParent, ddlFlowthrough, ddlIngredientsNeedToClaimBioEng;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (e.Item.Visible)
                {
                    PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                    ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpNewExisting"));
                    Utilities.SetDropDownValue(packagingItem.NewExisting, ddlPackagingComponent, this.Page);

                    ddlNewFormula = ((DropDownList)e.Item.FindControl("ddlNewFormula"));
                    Utilities.SetDropDownValue(packagingItem.NewFormula, ddlNewFormula, this.Page);

                    ddlTrialsCompleted = ((DropDownList)e.Item.FindControl("ddlTrialsCompleted"));
                    Utilities.SetDropDownValue(packagingItem.TrialsCompleted, ddlTrialsCompleted, this.Page);

                    ddlFlowthrough = ((DropDownList)e.Item.FindControl("ddlFlowthrough"));
                    Utilities.BindDropDownItemsById(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, ddlFlowthrough, this.Page);

                    ddlIngredientsNeedToClaimBioEng = ((DropDownList)e.Item.FindControl("ddlIngredientsNeedToClaimBioEng"));
                    Utilities.SetDropDownValue(packagingItem.IngredientsNeedToClaimBioEng, ddlIngredientsNeedToClaimBioEng, this.Page);

                    HiddenField hdnFlowthrough = ((HiddenField)e.Item.FindControl("hdnFlowthrough"));
                    hdnFlowthrough.Value = packagingItem.Flowthrough;

                    ddlTransfParent = ((DropDownList)e.Item.FindControl("ddlTransfParent"));
                    ddlTransfParent.DataSource = GetTransferSemi();
                    ddlTransfParent.DataBind();
                    Utilities.SetDropDownValueById(packagingItem.ParentID.ToString(), ddlTransfParent, this.Page);
                }
            }
        }
        public void bindPurchasedSemiData(bool extraRow)
        {
            var purchasedlst = qaService.GetPurchasedCandySemiForProject(iItemId);
            if (extraRow)
            {
                PackagingItem blankItem = new PackagingItem();
                blankItem.NewExisting = "";
                blankItem.NewFormula = "";
                blankItem.MaterialNumber = "";
                blankItem.MaterialDescription = "";
                blankItem.TrialsCompleted = "";
                blankItem.ShelfLife = "";

                int newId = packagingItemService.InsertPackagingItem(blankItem, iItemId);
                blankItem.Id = newId;
                purchasedlst.Add(blankItem);
            }

            rptPurchased.DataSource = purchasedlst;
            rptPurchased.DataBind();
        }

        protected void rptPurchased_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                var Childlst = new List<PackagingItem>();
                Childlst = packagingItemService.GetPackagingChildren(id);

                if (Childlst.Count > 0)
                {
                    Panel lblCompDeleteError = ((Panel)e.Item.FindControl("lblCompDeleteErrorPURCandy"));
                    lblCompDeleteError.Visible = true;
                    if (e.Item.ItemIndex % 2 == 0)
                    {
                        lblCompDeleteError.CssClass = "row";
                    }
                    else
                    {
                        lblCompDeleteError.CssClass = "row blueRow";
                    }
                }
                else
                {
                    if (id > 0)
                    {
                        SavePackagingItem(id);
                    }
                }
            }
        }

        protected void rptPurchased_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DropDownList ddlPackagingComponent, ddlNewFormula, ddlTrialsCompleted, ddlTransfParent, ddlFlowthrough, ddlIngredientsNeedToClaimBioEng;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                if (e.Item.Visible)
                {
                    PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                    ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpNewExisting"));
                    Utilities.SetDropDownValue(packagingItem.NewExisting, ddlPackagingComponent, this.Page);

                    ddlNewFormula = ((DropDownList)e.Item.FindControl("ddlNewFormula"));
                    Utilities.SetDropDownValue(packagingItem.NewFormula, ddlNewFormula, this.Page);

                    ddlTrialsCompleted = ((DropDownList)e.Item.FindControl("ddlTrialsCompleted"));
                    Utilities.SetDropDownValue(packagingItem.TrialsCompleted, ddlTrialsCompleted, this.Page);

                    ddlFlowthrough = ((DropDownList)e.Item.FindControl("ddlFlowthrough"));
                    Utilities.BindDropDownItemsById(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, ddlFlowthrough, this.Page);

                    ddlIngredientsNeedToClaimBioEng = ((DropDownList)e.Item.FindControl("ddlIngredientsNeedToClaimBioEng"));
                    Utilities.SetDropDownValue(packagingItem.IngredientsNeedToClaimBioEng, ddlIngredientsNeedToClaimBioEng, this.Page);

                    ddlTransfParent = ((DropDownList)e.Item.FindControl("ddlTransfParent"));
                    ddlTransfParent.DataSource = GetTransferSemi();
                    ddlTransfParent.DataBind();
                    Utilities.SetDropDownValueById(packagingItem.ParentID.ToString(), ddlTransfParent, this.Page);
                }
            }
        }
        private void LoadBOMGrid()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            phBOM.Controls.Clear();
            dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);

            foreach (PackagingItem item in dtPackingItem)
            {

                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBOMGrid ctrl2 = (ucBOMGrid)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx");
                    ctrl2.openBtnSave = openBtnSave;
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOM.Controls.Add(ctrl2);

                }
            }
            ucBOMGrid ctrl = (ucBOMGrid)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx");
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.openBtnSave = openBtnSave;

            phBOM.Controls.Add(ctrl);
            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable)Page.LoadControl(@"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx");
                ctrlPE.PackagingComponent = hdnPackagingComponent.Value;
                ctrlPE.PackagingItemId = Convert.ToInt32(hdnPackagingID.Value);
                ctrlPE.ParentID = hdnParentID.Value;
                // Add messages to page
                phMsg.Controls.Add(ctrlPE);
            }
        }
        private void LoadBOMGrid_New()
        {
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();

            phBOM.Controls.Clear();
            dtPackingItem = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);
            int FFFGCount = (from PIs in dtPackingItem where PIs.PackagingComponent.Contains("Finished Good") select PIs).Count();
            hdnFinishedGoodCount.Value = FFFGCount.ToString();
            foreach (BOMSetupItem item in dtPackingItem)
            {
                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBOMGrid_New ctrl2 = (ucBOMGrid_New)Page.LoadControl(_ucBOMGrid_New);
                    ctrl2.ID = "grid" + item.Id.ToString();
                    ctrl2.iItemId = iItemId;
                    ctrl2.GridItem = item;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.openBtnSave = openBtnSave;
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
            ctrl.GridItem = FGItem;
            ctrl.ParentId = 0;
            ctrl.openBtnSave = openBtnSave;
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
        private List<PackagingItem> GetTransferSemi()
        {
            if (ViewState["TransferSemi"] == null)
                ViewState["TransferSemi"] = qaService.GetTransferSemiItemsForProject(iItemId);
            return (List<PackagingItem>)ViewState["TransferSemi"];
        }
    }
}
