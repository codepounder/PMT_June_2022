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
using Microsoft.SharePoint.WebControls;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.WebParts.ItemProposalForm2
{
    [ToolboxItemAttribute(false)]
    public class BOM
    {
        public PackagingItem Item = new PackagingItem();
        public List<BOM> Children { get; set; }
    }
    public partial class ItemProposalForm2 : WebPart
    {
        #region Member Variables
        private IItemProposalService itemProposalService;
        private IStageGateCreateProjectService SGSService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IMixesService mixesService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private INotificationService notificationService;
        private IWorkflowService workflowService;
        private IConfigurationManagementService configurationService;
        private ISAPBOMService sapBOMService;
        private ISAPMaterialMasterService sapMMService;
        private IProjectNotesService projectNotesService;
        private string webUrl;
        private int iItemId = 0;
        private string currentProjectNumber;
        private List<PackagingItem> packagingItems = new List<PackagingItem>();
        private List<ShipperFinishedGoodItem> shipperFGItems = new List<ShipperFinishedGoodItem>();
        private const string _ucTSPath = @"~/_controltemplates/15/Ferrara.Compass/ucTS.ascx";
        private List<PackagingItem> SAPPIs;
        private string newProjectNumber;
        private int indent = -1;
        private bool InvalidPeopleEditor = false;
        List<PackagingItem> allPIs;
        List<FileAttribute> allFiles;
        #endregion

        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    currentProjectNumber = HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
                else
                    currentProjectNumber = string.Empty;
                return currentProjectNumber;
            }
        }
        private string IPFMode
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_IPFMode] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_IPFMode];
                return string.Empty;
            }
        }
        private string ProjectRejected
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectRejected] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectRejected];
                return string.Empty;
            }
        }
        private string TestProject
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRINGVALUE_TestProject] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRINGVALUE_TestProject];
                return string.Empty;
            }
        }
        private string ProjectSaved
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Save] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Save];
                return string.Empty;
            }
        }
        #endregion
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public ItemProposalForm2()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            // Page.EnableEventValidation = false;

            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            sapBOMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            projectNotesService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectNotesService>();
            SGSService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();

            ddlProjectType.Attributes.Add("onchange", "ProjectTypeChange();SingleProjectTypeChange();SAPNomenclature(ddlProjectType);");
            ddlOutsideUSA.Attributes.Add("onchange", "conditionalChecks();SAPNomenclature();");
            ddlNeedNewUnitUPC.Attributes.Add("onchange", "conditionalChecks();");
            //ddlBrand_Material.Attributes.Add("onchange", "SAPNomenclature();");
            ddlCustomerSpecific.Attributes.Add("onchange", "conditionalChecks();SAPNomenclature();");
            ddlNeedNewUPCUCC.Attributes.Add("onchange", "conditionalChecks();");
            ddlNeedNewUnitUPC.Attributes.Add("onchange", "conditionalChecks();");
            ddlNeedNewDisplayBoxUPC.Attributes.Add("onchange", "conditionalChecks();");
            ddlSAPBUOM.Attributes.Add("onchange", "conditionalChecks();");
            ddlNeedNewCaseUCC.Attributes.Add("onchange", "conditionalChecks();");
            ddlNeedNewPalletUCC.Attributes.Add("onchange", "conditionalChecks();");
            ddlTBDIndicator.Attributes.Add("onchange", "conditionalChecks();SAPNomenclature(ddlTBDIndicator);");
            ddlMaterialGroup4.Attributes.Add("onchange", "conditionalChecks();");
            ddlMaterialGroup5.Attributes.Add("onchange", "conditionalChecks();SAPNomenclature();RetailUnitWeightRequirement();");
            ddlNewFormula.Attributes.Add("onchange", "conditionalChecks();");
            ddlNewFlavorColor.Attributes.Add("onchange", "conditionalChecks();");
            ddlNewShape.Attributes.Add("onchange", "conditionalChecks();");
            ddlNewNetWeight.Attributes.Add("onchange", "conditionalChecks();");
            ddlServiceSizeChange.Attributes.Add("onchange", "conditionalChecks();");
            txtTruckLoadSellingPrice.Attributes.Add("onkeyup", "CalculateAnnualProjectedUnits();");
            txtRetailSellingUnitsPerBaseUOM.Attributes.Add("onkeyup", "CalculateBaseUnitOfMeasureNetWeightLbs();");
            txtRetailUnitWeight.Attributes.Add("onkeyup", "CalculateBaseUnitOfMeasureNetWeightLbs();");
            txtAnnualProjectedDollars.Attributes.Add("onkeyup", "CalculateAnnualProjectedUnits();");
            txtMonth1ProjectedDollars.Attributes.Add("onkeyup", "CalculateAnnualProjectedUnits();");
            txtMonth2ProjectedDollars.Attributes.Add("onkeyup", "CalculateAnnualProjectedUnits();");
            txtMonth3ProjectedDollars.Attributes.Add("onkeyup", "CalculateAnnualProjectedUnits();");

            EnsureScriptManager();
            EnsureUpdatePanelFixups();

            txtAnnualProjectedUnits.Attributes.Add("readonly", "readonly");
            txtMonth1ProjectedUnits.Attributes.Add("readonly", "readonly");
            txtMonth2ProjectedUnits.Attributes.Add("readonly", "readonly");
            txtMonth3ProjectedUnits.Attributes.Add("readonly", "readonly");
            txtBaseUofMNetWeight.Attributes.Add("readonly", "readonly");
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                Utilities.BindDropDownItems(ddlCaseType, GlobalConstants.LIST_CaseTypesLookup, webUrl);
                Utilities.BindDropDownItemsAddValues(ddlCustomer, GlobalConstants.LIST_CustomersLookup, webUrl);
                Utilities.BindDropDownItems(ddlProjectType, GlobalConstants.LIST_ProjectTypesLookup, webUrl);
                Utilities.BindDropDownItems(ddlSAPBUOM, GlobalConstants.LIST_SAPBaseUOMLookup, webUrl);
                Utilities.BindDropDownItems(ddlChannel, GlobalConstants.LIST_ChannelLookup, webUrl);
                Utilities.BindDropDownItems(ddlProductHierarchyLevel1, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                Utilities.BindDropDownItems(ddlMaterialGroup4, GlobalConstants.LIST_MaterialGroup4Lookup, webUrl);
                Utilities.BindDropDownItems(ddlMaterialGroup5, GlobalConstants.LIST_MaterialGroup5Lookup, webUrl);
                Utilities.BindDropDownItems(ddlCountryOfSale, GlobalConstants.LIST_CountryOfOriginLookup, webUrl);
                Utilities.BindDropDownItems(ddlFilmSubstrate, GlobalConstants.LIST_FilmSubstrate, webUrl);
                Utilities.BindDropDownItems(ddlProjectTypeSubCategory, GlobalConstants.LIST_ProjectTypesSubCategoryLookup, webUrl);
                Utilities.AddItemToDropDown(this.ddlProjectTypeSubCategory, "NA", "4", true);

                Utilities.BindDropDownItems(drpMadeInUSAPct, GlobalConstants.LIST_MadeInUSAClaimLookup, webUrl);
                Utilities.BindDropDownItems(drpGMOClaim, GlobalConstants.LIST_GMOClaimLookup, webUrl);
                Utilities.BindDropDownItems(drpGlutenFree, GlobalConstants.LIST_GlutenFreeLookup, webUrl);
                Utilities.BindDropDownItems(drpKosher, GlobalConstants.LIST_KosherTypesLookup, webUrl);
                Utilities.BindDropDownItems(drpNaturalColors, GlobalConstants.LIST_NaturalColorsLookup, webUrl);
                Utilities.BindDropDownItems(drpNaturalFlavors, GlobalConstants.LIST_NaturalFlavorsLookup, webUrl);
                Utilities.BindListBoxItems(drpGoodSourceAvailable, GlobalConstants.LIST_GoodSourceLookup, webUrl);
                Utilities.BindDropDownItems(drpBioEngineeringLabelingAcceptable, GlobalConstants.LIST_BioEngineeringLabelingcceptableLookup, webUrl);
                List<DropDownList> itemsToBind = new List<DropDownList>() { drpVitaminAPct, drpVitaminB12Pct, drpVitaminB1Pct, drpVitaminB2Pct, drpVitaminB3Pct, drpVitaminB5Pct, drpVitaminB6Pct, drpVitaminCPct, drpVitaminDPct, drpVitaminEPct, drpPotassiumPct, drpIronPct, drpCalciumPct };
                foreach (DropDownList list in itemsToBind)
                {
                    Utilities.BindDropDownItems(list, GlobalConstants.LIST_PercentagesLookup, webUrl);
                }
                // Check for a valid project number
                if (!CheckProjectNumber())
                {
                    hdnNewIPF.Value = "Yes";
                    btnSave.Style.Add("opacity", "0");
                    btnNext.Visible = true;
                    SetInitiator();
                    return;
                }
                else
                {
                    btnSave.Style.Add("opacity", "1");
                    btnNext.Visible = false;
                }

                if (iItemId > 0)
                {
                    LoadFormData();
                    LoadFormulationAttachments();
                    LoadGraphicsAttachments();
                    LoadShipperFinishedGoodItems();
                    LoadMixesItems();
                    LoadMarketingClaimsData();
                    if (!string.IsNullOrEmpty(this.IPFMode))
                        CheckForCopyChangeRequest();
                }
                else
                {
                    LoadBlankProjectTeamber();
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);

                if (ViewState["Changes Saved"] != null && !string.IsNullOrEmpty(ViewState["Changes Saved"].ToString()))
                {
                    lblSaved.Text = "Changes Saved: " + DateTime.Now.ToString();
                }
            }
            hdnIItemId.Value = iItemId.ToString();
            hdnProjectNumber.Value = ProjectNumber;
            hdnPageName.Value = GlobalConstants.PAGE_ItemProposal;
            if (iItemId > 0)
            {
                SAPPIs = null;
                LoadBOMItems();
            }

            CallUpdatePeopleEditorScriptFunction();
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if ((iItemId == 0) && (!string.IsNullOrEmpty(ProjectNumber)))
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }
            else if (iItemId == 0)
            {
                this.hiddenItemId.Value = iItemId.ToString();
                return false;
            }
            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void SetInitiator()
        {
            // Set the Initiator people picker
            peInitiator.CommaSeparatedAccounts = SPContext.Current.Web.CurrentUser.LoginName;
            peInitiator.Validate();
        }
        private void EnsureScriptManager()
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                scriptManager.EnablePartialRendering = true;
                scriptManager.AsyncPostBackTimeout = 900;

                if (Page.Form != null)
                {
                    Page.Form.Controls.AddAt(0, scriptManager);
                }
            }
            else
            {
                scriptManager.AsyncPostBackTimeout = 900;
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
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "EnsureUpdatePanelFixups", "SAPNomenclature()", false);
        }
        private void CallUpdatePeopleEditorScriptFunction()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "updatePeopleEditors();", true);
        }
        private void CheckForCopyChangeRequest()
        {
            if (string.Equals(this.IPFMode, GlobalConstants.QUERYSTRINGVALUE_IPFChange))
            {
                // Change Request, so just Determine the Next Project Change Request Number
                string projectNumber = Utilities.DetermineChangeRequestProjectNumber(ProjectNumber);
                // Make sure generated project number is not a duplicate. If it is, generate the next one
                while (itemProposalService.IsExistingProjectNo(projectNumber))
                {
                    projectNumber = Utilities.DetermineChangeRequestProjectNumber(projectNumber);
                }

                // If this is a copy or Change request then Save the form to generate the new project
                exceptionService.Handle(LogCategory.General, "CheckForCopyChangeRequest", CompassForm.IPF.ToString(), "CheckForCopyChangeRequest", "Next Project Number" + projectNumber);
                btnSave_Click(null, null);
                if (this.ProjectRejected == "Yes")
                {
                    string redirectUrl = string.Concat(SPContext.Current.Web.Url, "/Pages/TaskDashboard.aspx");
                    Page.Response.Redirect(redirectUrl, false);
                }
                else
                {
                    Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), newProjectNumber), false);
                }
            }
            else if (string.Equals(this.IPFMode, GlobalConstants.QUERYSTRINGVALUE_IPFCopy))
            {
                btnSave_Click(null, null);
                Page.Response.Redirect(Utilities.RedirectPageForm(Utilities.GetCurrentPageName(), newProjectNumber), false);
            }
        }
        private Boolean SaveAllRepeaterItems()
        {
            SaveBOM();
            /// Saving Shipper Grid
            if (ddlMaterialGroup5.SelectedItem.Text.ToLower() == "shipper (shp)" || ddlMaterialGroup5.SelectedItem.Text.ToLower() == "shippers (shp)")
            {
                List<ShipperFinishedGoodItem> dtFGItem = new List<ShipperFinishedGoodItem>();
                foreach (RepeaterItem item in rptShipper.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var objFGItem = new ShipperFinishedGoodItem
                        {
                            ItemId = Convert.ToInt32(((HiddenField)item.FindControl("hidShipperId")).Value),
                            FGItemDescription = ((HtmlInputControl)item.FindControl("txtFGItemDescription")).Value,
                            FGItemNumber = ((HtmlInputControl)item.FindControl("txtFGItemDisplay")).Value,
                            FGItemNumberUnits = Convert.ToInt32(((HtmlInputControl)item.FindControl("txtFGnumberUnits")).Value),
                            FGItemOuncesPerUnit = Convert.ToDouble(((HtmlInputControl)item.FindControl("txtFGouncesPerUnit")).Value),
                            FGPackUnit = ((DropDownList)item.FindControl("ddlFGPackUnit")).SelectedItem.Text,
                            CompassListItemId = iItemId
                        };
                        dtFGItem.Add(objFGItem);
                    }

                }
                shipperFinishedGoodService.UpsertShipperFinishedGoodItem(dtFGItem, ProjectNumber);
            }

            if (ddlMaterialGroup4.SelectedItem.Text == "MIXES (MIX)")
            {
                /// Saving Mixes Grid
                List<MixesItem> dtMixesItem = new List<MixesItem>();
                foreach (RepeaterItem item in rpMixes.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        MixesItem objMixesItem = new MixesItem();
                        objMixesItem.ItemId = Convert.ToInt32(((HiddenField)item.FindControl("hidMixesId")).Value);
                        objMixesItem.CompassListItemId = iItemId;
                        objMixesItem.ItemNumber = ((HtmlInputControl)item.FindControl("txtMixItemNumber")).Value;
                        objMixesItem.ItemDescription = ((HtmlInputControl)item.FindControl("txtMixItemDescription")).Value;
                        try
                        {
                            objMixesItem.NumberOfPieces = Convert.ToDouble(((HtmlInputControl)item.FindControl("txtMixNumberOfPieces")).Value);
                        }
                        catch
                        {
                            objMixesItem.NumberOfPieces = 0.0;
                        }

                        try
                        {
                            objMixesItem.OuncesPerPiece = Convert.ToDouble(((HtmlInputControl)item.FindControl("txtOzPerPiece")).Value);
                        }
                        catch
                        {
                            objMixesItem.OuncesPerPiece = 0.0;
                        }

                        dtMixesItem.Add(objMixesItem);
                    }

                }
                mixesService.UpsertMixesItem(dtMixesItem, ProjectNumber);
            }
            ViewState["FGShipperItemTable"] = null;
            ViewState["PackagingItemTable"] = null;
            ViewState["MixesItemTable"] = null;
            //hdnUCLoaded.Value = "false";
            LoadBOMItems();
            LoadShipperFinishedGoodItems();
            LoadMixesItems();

            return true;
        }
        private void LoadBOMItems()
        {
            // List<PackagingItem> allPIs = new List<PackagingItem>();
            // List<FileAttribute> allFiles = new List<FileAttribute>();
            allPIs = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            allFiles = packagingItemService.GetRenderingUploadedFiles(ProjectNumber);
            allFiles.AddRange(packagingItemService.GetApprovedGraphicsAssetUploadedFiles(ProjectNumber));
            //ucTS ctrl = (ucTS)Page.LoadControl(_ucTSPath);

            //ctrl.ParentId = 0;
            //ctrl.MaterialNumber = "";
            //ctrl.MaterialDesc = "";
            //ctrl.ParentComponentType = "";
            //ctrl.ProjectNumber = ProjectNumber;
            //ctrl.IPFMode = IPFMode;
            //ctrl.projectAttachments = allFiles;
            //ctrl.AllPIs = allPIs;
            //ctrl.SAPPIs = SAPPIs;
            //ctrl.CompassItemId = iItemId;
            //ctrl.firstLoad = "true";
            //ctrl.ID = "FinishedGood";
            //phFG.Controls.Clear();
            //phTS.Controls.Clear();
            //phFG.Controls.Add(ctrl);

            lstviewFGBOM.DataSource = allPIs;
            lstviewFGBOM.DataBind();
        }
        protected void lstviewFGBOM_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //List<PackagingItem> allPIs = new List<PackagingItem>();
                //List<FileAttribute> allFiles = new List<FileAttribute>();
                if (allPIs == null)
                {
                    allPIs = packagingItemService.GetAllPackagingItemsForProject(iItemId);
                }
                if (allFiles == null)
                {
                    allFiles = packagingItemService.GetRenderingUploadedFiles(ProjectNumber);
                    allFiles.AddRange(packagingItemService.GetApprovedGraphicsAssetUploadedFiles(ProjectNumber));
                }

                ListViewDataItem dataItem = (ListViewDataItem)e.Item;
                // you would use your actual data item type here, not "object"
                PackagingItem packagingItem = (PackagingItem)dataItem.DataItem;

                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
                TextBox txtMaterialDesc = (TextBox)e.Item.FindControl("txtMaterialDesc");
                TextBox txtLikeMaterialDesc = (TextBox)e.Item.FindControl("txtLikeMaterialDesc");
                TextBox txtLikeMaterial = (TextBox)e.Item.FindControl("txtLikeMaterial");
                TextBox txtOldMaterialDesc = (TextBox)e.Item.FindControl("txtOldMaterialDesc");
                TextBox txtOldMaterial = (TextBox)e.Item.FindControl("txtOldMaterial");
                TextBox txtTSComments = (TextBox)e.Item.FindControl("txtTSComments");
                DropDownList drpGraphicsNeeded = ((DropDownList)e.Item.FindControl("drpGraphicsNeeded"));
                DropDownList ddlGraphicsVendor = ((DropDownList)e.Item.FindControl("ddlGraphicsVendor"));
                DropDownList ddlFlowthrough = ((DropDownList)e.Item.FindControl("ddlFlowthrough"));
                DropDownList ddlProductHierarchyLevel1 = ((DropDownList)e.Item.FindControl("ddlPHL1"));
                DropDownList ddlBrand_Material = ((DropDownList)e.Item.FindControl("ddlBrand"));
                DropDownList ddlProductHierarchyLevel2 = ((DropDownList)e.Item.FindControl("ddlPHL2"));
                TextBox txtProfitCenter = (TextBox)e.Item.FindControl("txtProfitCenterUC");
                HiddenField hdnProfitCenterUC = (HiddenField)e.Item.FindControl("hdnProfitCenterUC");
                TextBox txtGraphicsBrief = (TextBox)e.Item.FindControl("txtGraphicsBrief");
                HtmlControl spanGraphicsBrief = (HtmlControl)e.Item.FindControl("spanGraphicsBrief");
                HtmlControl spanWhyComponent = (HtmlControl)e.Item.FindControl("spanWhyComponent");
                TextBox txtLikeReason = (TextBox)e.Item.FindControl("txtLikeReason");
                HtmlControl spanGraphicsVendor = (HtmlControl)e.Item.FindControl("spanGraphicsVendor");
                HiddenField hdnDeletedStatus = (HiddenField)e.Item.FindControl("hdnDeletedStatus");
                HiddenField hdnParentId = (HiddenField)e.Item.FindControl("hdnParentId");
                hdnParentId.Value = packagingItem.ParentID.ToString();
                HiddenField hdnComponentType = (HiddenField)e.Item.FindControl("hdnComponentType");

                //hdnComponentType.Value = ParentComponentType;
                TextBox txtPackQty = (TextBox)e.Item.FindControl("txtPackQty");
                txtTSComments.Text = packagingItem.Notes;
                List<KeyValuePair<int, string>> allItems = new List<KeyValuePair<int, string>>();
                List<int> idsOnly = new List<int>();
                DropDownList ddlMoveTS = ((DropDownList)e.Item.FindControl("ddlMoveTS"));

                allItems.Add(new KeyValuePair<int, string>(0, "Finished Good"));
                idsOnly.Add(0);

                foreach (PackagingItem item in allPIs)
                {
                    if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        allItems.Add(new KeyValuePair<int, string>(item.Id, item.MaterialNumber + ": " + item.MaterialDescription));
                        idsOnly.Add(item.Id);
                    }
                }

                if (allItems != null && allItems.Count > 0)
                {
                    List<int> filteredParents = filterParents(idsOnly, iItemId, packagingItem.Id);
                    foreach (KeyValuePair<int, string> IDs in allItems)
                    {
                        int approvedId = filteredParents.IndexOf(IDs.Key);
                        if (IDs.Key != packagingItem.Id && approvedId > -1)
                        {
                            ddlMoveTS.Items.Add(new ListItem(IDs.Value, IDs.Key.ToString()));
                        }
                    }
                }
                hdnDeletedStatus.Value = packagingItem.Deleted;
                if (!string.IsNullOrEmpty(packagingItem.PackQuantity))
                {
                    var pacqty = (packagingItem.PackQuantity == "-9999" || string.IsNullOrEmpty(packagingItem.PackQuantity)) ? "0" : packagingItem.PackQuantity;
                    txtPackQty.Text = Math.Round(Convert.ToDouble(pacqty), 4).ToString();
                }
                txtLikeReason.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItemReason) ? "" : packagingItem.CurrentLikeItemReason;
                txtGraphicsBrief.Text = string.IsNullOrEmpty(packagingItem.GraphicsBrief) ? "" : packagingItem.GraphicsBrief;
                DropDownList ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpComponent"));
                Utilities.BindDropDownItems(ddlPackagingComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl);
                Utilities.BindDropDownItems(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);
                Utilities.BindDropDownItemsPHL1(ddlProductHierarchyLevel1, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                Utilities.BindDropDownItemsPHL2(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, webUrl);
                Utilities.BindDropDownItemsBrand(ddlBrand_Material, GlobalConstants.LIST_MaterialGroup1Lookup, webUrl);
                // Load the level 1 list
                Utilities.SetDropDownValue(packagingItem.PHL1, ddlProductHierarchyLevel1, this.Page);
                //ddlProductHierarchyLevel1_SelectedIndexChanged(ddlProductHierarchyLevel1, null);

                Utilities.SetDropDownValue(packagingItem.PHL2, ddlProductHierarchyLevel2, this.Page);
                // Load the level 2 list
                //ddlProductHierarchyLevel2_SelectedIndexChanged(ddlProductHierarchyLevel2, null);

                // Load Brand List
                Utilities.SetDropDownValue(packagingItem.Brand, ddlBrand_Material, this.Page);
                //ddlBrand_SelectedIndexChanged(ddlBrand_Material, null);

                txtProfitCenter.Text = packagingItem.ProfitCenter;
                hdnProfitCenterUC.Value = packagingItem.ProfitCenter;

                if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, ddlPackagingComponent, this.Page);
                    if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        int childCount = (from PI in allPIs where PI.ParentID == packagingItem.Id select PI).Count();
                        if (childCount > 0)
                        {
                            ddlPackagingComponent.Enabled = false;
                            Panel lblCompNote = ((Panel)e.Item.FindControl("lblCompNote"));
                            lblCompNote.Visible = true;
                            if (e.Item.DataItemIndex % 2 == 0)
                            {
                                lblCompNote.CssClass = "row";
                            }
                            else
                            {
                                lblCompNote.CssClass = "row blueRow";
                            }
                        }
                    }
                }

                DropDownList drpUnitOfMeasure = ((DropDownList)e.Item.FindControl("drpUnitOfMeasure"));
                Utilities.BindDropDownItems(drpUnitOfMeasure, GlobalConstants.LIST_PackUnitLookup, webUrl);
                if (packagingItem.PackUnit != null && packagingItem.PackUnit != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackUnit, drpUnitOfMeasure, this.Page);
                }

                DropDownList drpComponentContainsNLEA = ((DropDownList)e.Item.FindControl("drpComponentContainsNLEA"));
                if (packagingItem.ComponentContainsNLEA != null && packagingItem.ComponentContainsNLEA != string.Empty)
                    Utilities.SetDropDownValue(packagingItem.ComponentContainsNLEA, drpComponentContainsNLEA, this.Page);
                if (packagingItem.Flowthrough != null && packagingItem.Flowthrough != string.Empty)
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, ddlFlowthrough, this.Page);
                string matLikeNumb, matLikeDesc;
                matLikeNumb = string.IsNullOrEmpty(packagingItem.CurrentLikeItem) ? "" : packagingItem.CurrentLikeItem;
                matLikeDesc = string.IsNullOrEmpty(packagingItem.CurrentLikeItemDescription) ? "" : packagingItem.CurrentLikeItemDescription;
                if (matLikeDesc == "N/A")
                {
                    matLikeDesc = "Not Applicable";
                }
                if (matLikeNumb == "N/A")
                {
                    matLikeNumb = "Not Applicable";
                }
                matLikeNumb = string.IsNullOrEmpty(packagingItem.CurrentOldItem) ? "" : packagingItem.CurrentOldItem;
                matLikeDesc = string.IsNullOrEmpty(packagingItem.CurrentOldItemDescription) ? "" : packagingItem.CurrentOldItemDescription;
                if (matLikeDesc == "N/A")
                {
                    matLikeDesc = "Not Applicable";
                }
                if (matLikeNumb == "N/A")
                {
                    matLikeNumb = "Not Applicable";
                }
                txtOldMaterial.Text = matLikeNumb;
                txtOldMaterialDesc.Text = matLikeDesc;
                DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNew"));
                if (packagingItem.NewExisting != null && packagingItem.NewExisting != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.NewExisting, drpNew, this.Page);
                }

                Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);
                if (!string.IsNullOrEmpty(packagingItem.ExternalGraphicsVendor))
                {
                    Utilities.SetDropDownValue(packagingItem.ExternalGraphicsVendor, ddlGraphicsVendor, this.Page);
                }

                if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                {
                    Utilities.SetDropDownValue(packagingItem.GraphicsChangeRequired, drpGraphicsNeeded, this.Page);

                    if (packagingItem.GraphicsChangeRequired.ToLower() == "yes")
                    {
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief PCBOMrequired";
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief showItempc");
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor PCBOMrequired";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor showItempc");
                    }
                    else
                    {
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief hideItem");
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor hideItem");
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor";
                    }

                }

                #region Visual Reference / Rendering:
                HtmlImage imgAtt = (HtmlImage)e.Item.FindControl("btnAttachment");

                var files =
                    (
                        from
                            attachment in allFiles
                        where
                            attachment.PackagingComponentItemId == packagingItem.Id
                            &&
                            attachment.DocType == GlobalConstants.DOCTYPE_Rendering
                        select
                            attachment
                      ).ToList();

                if (files.Count > 0)
                {
                    ImageButton btnDeleteAttachment = (ImageButton)e.Item.FindControl("btnDeleteAttachment");
                    HtmlAnchor anc = ((HtmlAnchor)e.Item.FindControl("ancRendering"));
                    HiddenField DeletedVisualreferenceUrl = ((HiddenField)e.Item.FindControl("DeletedVisualreferenceUrl"));

                    btnDeleteAttachment.Visible = true;
                    if (anc != null)
                    {
                        string fileName = files[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        anc.Controls.Add(new LiteralControl(fileName));
                        anc.HRef = files[0].FileUrl;
                        btnDeleteAttachment.CommandArgument = files[0].FileUrl;
                        DeletedVisualreferenceUrl.Value = files[0].FileUrl;
                    }

                    imgAtt.Visible = false;
                }
                #endregion

                #region Approved Graphics Asset
                HtmlImage btnApprovedGraphicsAsset = (HtmlImage)e.Item.FindControl("btnApprovedGraphicsAsset");

                var ApprovedGraphicsAssets =
                    (
                        from
                            attachment in allFiles
                        where
                            attachment.PackagingComponentItemId == packagingItem.Id
                            &&
                            attachment.DocType == GlobalConstants.DOCTYPE_ApprovedGraphicsAsset
                        select
                            attachment
                      ).ToList();

                if (ApprovedGraphicsAssets.Count > 0)
                {
                    ImageButton btnDeleteApprovedGraphicsAsset = (ImageButton)e.Item.FindControl("btnDeleteApprovedGraphicsAsset");
                    HtmlAnchor ancApprovedGraphicsAsset = ((HtmlAnchor)e.Item.FindControl("ancApprovedGraphicsAsset"));
                    HiddenField DeletedApprovedGraphicsAssetUrl = ((HiddenField)e.Item.FindControl("DeletedApprovedGraphicsAssetUrl"));

                    btnDeleteApprovedGraphicsAsset.Visible = true;
                    if (ancApprovedGraphicsAsset != null)
                    {
                        string fileName = ApprovedGraphicsAssets[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        ancApprovedGraphicsAsset.Controls.Add(new LiteralControl(fileName));
                        ancApprovedGraphicsAsset.HRef = ApprovedGraphicsAssets[0].FileUrl;
                        btnDeleteApprovedGraphicsAsset.CommandArgument = ApprovedGraphicsAssets[0].FileUrl;
                        DeletedApprovedGraphicsAssetUrl.Value = ApprovedGraphicsAssets[0].FileUrl;
                    }

                    btnApprovedGraphicsAsset.Visible = false;
                }
                #endregion

                //if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                //{
                //    ucTS ctrl2 = (ucTS)Page.LoadControl(_ucTSPath);
                //    ctrl2.ParentId = packagingItem.Id;
                //    ctrl2.MaterialNumber = packagingItem.MaterialNumber;
                //    ctrl2.MaterialDesc = packagingItem.MaterialDescription;
                //    ctrl2.ProjectNumber = ProjectNumber;
                //    ctrl2.CompassItemId = iItemId;
                //    ctrl2.AllPIs = AllPIs;
                //    ctrl2.ParentComponentType = packagingItem.PackagingComponent;
                //    ctrl2.projectAttachments = projectAttachments;
                //    // ctrl2.firstLoad = "true";
                //    string PCTS = "";
                //    if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                //    {
                //        PCTS = "TS";
                //    }
                //    else if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                //    {
                //        PCTS = "PCS";
                //    }
                //    ctrl2.ID = PCTS + packagingItem.Id;

                //    this.Parent.FindControl("phTS").Controls.Add(ctrl2);
                //}
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "ProjectTypeChange", "ProjectTypeChange();", true);
        }
        public List<int> filterParents(List<int> IDs, int compassItemID, int movingId)
        {
            List<int> idsHolder = IDs;
            List<KeyValuePair<int, int>> AllItems = new List<KeyValuePair<int, int>>();
            foreach (PackagingItem item in allPIs)
            {
                AllItems.Add(new KeyValuePair<int, int>(item.Id, item.ParentID));
            }
            foreach (KeyValuePair<int, int> KVP in AllItems)
            {
                bool includeItems = includeItem(KVP.Key, movingId, AllItems);
                int IDIndex = IDs.IndexOf(KVP.Key);
                if (!includeItems && IDIndex >= 0)
                {

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
        public void SaveBOM()
        {
            var PIsToSave = new List<PackagingItem>();
            foreach (ListViewItem item in lstviewFGBOM.Items)
            {
                PackagingItem packagingItem = new PackagingItem();

                packagingItem.PackagingComponent = ((DropDownList)item.FindControl("drpComponent")).SelectedItem.Text;
                packagingItem.NewExisting = ((DropDownList)item.FindControl("drpNew")).SelectedItem.Text;
                packagingItem.MaterialNumber = ((TextBox)item.FindControl("txtMaterial")).Text;
                packagingItem.CurrentLikeItem = ((TextBox)item.FindControl("txtLikeMaterial")).Text;
                packagingItem.MaterialDescription = ((TextBox)item.FindControl("txtMaterialDesc")).Text;
                packagingItem.CurrentLikeItemDescription = ((TextBox)item.FindControl("txtLikeMaterialDesc")).Text;
                packagingItem.CurrentLikeItemReason = ((TextBox)item.FindControl("txtLikeReason")).Text;
                packagingItem.CurrentOldItem = ((TextBox)item.FindControl("txtOldMaterial")).Text;
                packagingItem.CurrentOldItemDescription = ((TextBox)item.FindControl("txtOldMaterialDesc")).Text;
                packagingItem.PackQuantity = ((TextBox)item.FindControl("txtPackQty")).Text;
                packagingItem.PackUnit = ((DropDownList)item.FindControl("drpUnitOfMeasure")).SelectedItem.Text;
                packagingItem.GraphicsBrief = ((TextBox)item.FindControl("txtGraphicsBrief")).Text;
                packagingItem.GraphicsChangeRequired = ((DropDownList)item.FindControl("drpGraphicsNeeded")).SelectedItem.Text;
                var CompId = ((HiddenField)item.FindControl("hdnItemID")).Value.Replace(",", "");
                packagingItem.Id = string.IsNullOrEmpty(CompId) ? 0 : Convert.ToInt32(CompId);
                packagingItem.ExternalGraphicsVendor = ((DropDownList)item.FindControl("ddlGraphicsVendor")).SelectedItem.Text;
                packagingItem.ComponentContainsNLEA = ((DropDownList)item.FindControl("drpComponentContainsNLEA")).SelectedItem.Text;
                if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                {
                    packagingItem.PHL1 = ((DropDownList)item.FindControl("ddlPHL1")).SelectedItem.Text;
                    packagingItem.PHL2 = ((DropDownList)item.FindControl("ddlPHL2")).SelectedItem.Text;
                    packagingItem.Brand = ((DropDownList)item.FindControl("ddlBrand")).SelectedItem.Text;
                    packagingItem.ProfitCenter = ((TextBox)item.FindControl("txtProfitCenterUC")).Text;
                    packagingItem.ProfitCenter = ((HiddenField)item.FindControl("hdnProfitCenterUC")).Value;
                }
                packagingItem.Flowthrough = ((DropDownList)item.FindControl("ddlFlowthrough")).SelectedItem.Text;
                packagingItem.Notes = ((TextBox)item.FindControl("txtTSComments")).Text;
                packagingItem.CompassListItemId = iItemId.ToString();

                int moveId = Convert.ToInt32(((DropDownList)item.FindControl("ddlMoveTS")).SelectedItem.Value);
                string movedPackType = ((DropDownList)item.FindControl("ddlMoveTS")).SelectedItem.Text;
                if (moveId != -1)
                {
                    packagingItem.ParentID = moveId;
                }
                else
                {
                    var hdnParentId = ((HiddenField)item.FindControl("hdnParentId")).Value;
                    if (!string.IsNullOrEmpty(hdnParentId))
                    {
                        packagingItem.ParentID = Convert.ToInt32(hdnParentId);
                    }
                }
                string deletedStatus = ((HiddenField)item.FindControl("hdnDeletedStatus")).Value;
                if (deletedStatus == "deleted")
                {
                    packagingItem.Deleted = "Yes";
                }
                if (packagingItem.Id <= 0 && deletedStatus != "deleted")
                {
                    int newId = packagingItemService.InsertPackagingItem(packagingItem, iItemId);
                    HiddenField hidId = ((HiddenField)item.FindControl("hdnItemID"));
                    if (hidId != null)
                        hidId.Value = newId.ToString();
                }
                else if (packagingItem.Id <= 0 && deletedStatus == "deleted")
                {

                }
                else if (packagingItem.Id > 0 && deletedStatus == "deleted")
                {
                    packagingItemService.DeletePackagingItem(packagingItem.Id);
                }
                else
                {
                    PIsToSave.Add(packagingItem);
                }
            }
            if (PIsToSave.Count > 0)
            {
                packagingItemService.UpdateIPFPackagingItems(PIsToSave, iItemId);
            }
        }
        private void DeleteItems()
        {
            if (ddlMaterialGroup5.SelectedItem.Text.ToLower() != "shipper (shp)" || ddlMaterialGroup5.SelectedItem.Text.ToLower() != "shippers (shp)")
            {
                foreach (RepeaterItem item in rptShipper.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var Id = Convert.ToInt32(((HiddenField)item.FindControl("hidShipperId")).Value);
                        if (Id > 0)
                            shipperFinishedGoodService.DeleteShipperFinishedGoodItem(Id, webUrl);
                    }
                }
            }
            if (ddlMaterialGroup4.SelectedItem.Text != "MIXES (MIX)")
            {
                foreach (RepeaterItem item in rpMixes.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        var Id = Convert.ToInt32(((HiddenField)item.FindControl("hidMixesId")).Value);
                        if (Id > 0)
                            mixesService.DeleteMixesItem(Id, webUrl);
                    }
                }
            }
        }
        private Boolean ValidateForm()
        {
            Boolean bValid = true;
            // People fields

            if (peInitiator.Entities.Count <= 0)
            {
                string strErrors = "Initiator must be set.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;0&quot;)'>  [Update]</a>";
                ErrorSummary.AddError(strErrors, this.Page);
                bValid = false;
            }

            int RepeaterCount = 1;
            if (!ValidateMembers(ref RepeaterCount, "Project Leader", divProjectLeaders, "rptProjectLeaders", "peProjectLeaderMembers", "hdnDeletedStatusForProjectLeader")) bValid = false;

            if (!ValidateMembers(ref RepeaterCount, "Other Team", divOtherTeamMembers, "rptOtherTeamMembers", "peOtherTeamMembers", "hdnDeletedStatusForOtherTeamMembers", false)) bValid = false;

            DateTime FirstShipDate;
            if ((DateTime.TryParse(txtFirstShipDate.Text, out FirstShipDate)))
            {
                int result = DateTime.Compare(FirstShipDate.Date, DateTime.Today.Date);
                if (!(result > 0))
                {
                    string strErrors = "Ship Date should be a future date.<a style=\"color: darkblue\" onclick=GotoStepAndFocus(\"txtFirstShipDate\") >Update</a>";
                    ErrorSummary.AddError(strErrors, this.Page);
                    bValid = false;
                }
            }

            return bValid;
        }
        private bool ValidateMembers(ref int Count, string MemberName, HtmlGenericControl div, string RepeaterName, string PeopleEditorName, string HiddenStatusFieldName, bool Required = true, bool JustNAValid = true)
        {
            bool bValid = true;
            Repeater rptMembers = ((Repeater)div.FindControl(RepeaterName));
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Memebers = new SPFieldUserValueCollection();
            int NACount = 0;
            List<string> strNAErrors = new List<string>();

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMember = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        if (peMember.Entities.Count <= 0 && Required)
                        {
                            string strErrors = MemberName + " member must be set.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + Count.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            bValid = false;
                        }
                        else if (Required && peMember.Entities.Count == 1 && !JustNAValid)
                        {
                            bool NA;
                            string NAText;
                            CheckForNA(out NA, out NAText, peMember);
                            if (NA)
                            {
                                NACount++;
                                strNAErrors.Add("NA is not a valid entry for " + MemberName + ". Please add a valid member. <a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + Count.ToString() + "&quot;)'>  [Update]</a>");
                            }
                        }
                    }

                }
                Count++;
            }

            if (rptMembers.Items.Count > 0 && rptMembers.Items.Count == NACount)
            {
                foreach (var error in strNAErrors)
                {
                    ErrorSummary.AddError(error, this.Page);
                    bValid = false;
                }
            }
            return bValid;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadMarketingClaimsData()
        {
            MarketingClaimsItem marketingItem = itemProposalService.GetMarketingClaimsItem(iItemId);

            Utilities.SetDropDownValue(marketingItem.SellableUnit, this.drpSellableUnit, this.Page);
            Utilities.SetDropDownValue(marketingItem.NewNLEAFormat, this.drpNewNLEAFormat, this.Page);
            Utilities.SetDropDownValue(marketingItem.BioEngLabelingAcceptable, this.drpBioEngineeringLabelingAcceptable, this.Page);
            Utilities.SetDropDownValue(marketingItem.MadeInUSAClaim, this.drpMadeInUSA, this.Page);
            Utilities.SetDropDownValue(marketingItem.MadeInUSAClaimDets, this.drpMadeInUSAPct, this.Page);
            Utilities.SetDropDownValue(marketingItem.Organic, this.drpOrganic, this.Page);
            Utilities.SetDropDownValue(marketingItem.GMOClaim, this.drpGMOClaim, this.Page);
            Utilities.SetDropDownValue(marketingItem.GlutenFree, this.drpGlutenFree, this.Page);
            Utilities.SetDropDownValue(marketingItem.FatFree, this.drpFatFree, this.Page);
            Utilities.SetDropDownValue(marketingItem.Kosher, this.drpKosher, this.Page);
            hdnSelectedGoodSource.Value = marketingItem.GoodSource;
            Utilities.SetDropDownValue(marketingItem.NaturalColors, this.drpNaturalColors, this.Page);
            Utilities.SetDropDownValue(marketingItem.NaturalFlavors, this.drpNaturalFlavors, this.Page);
            Utilities.SetDropDownValue(marketingItem.PreservativeFree, this.drpPreservativeFree, this.Page);
            Utilities.SetDropDownValue(marketingItem.LactoseFree, this.drpLactoseFree, this.Page);
            txtJuiceConcentrate.Text = marketingItem.JuiceConcentrate;
            txtMaterialClaimsCompNumber.Text = marketingItem.MaterialClaimsCompNumber;
            txtMaterialClaimsCompDesc.Text = marketingItem.MaterialClaimsCompNumber;
            Utilities.SetDropDownValue(marketingItem.LowSodium, this.drpLowSodium, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminA, this.drpVitaminA, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminAPct, this.drpVitaminAPct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminB1, this.drpVitaminB1, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminB12, this.drpVitaminB12, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminB12Pct, this.drpVitaminB12Pct, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminB1Pct, this.drpVitaminB1Pct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminB2, this.drpVitaminB2, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminB2Pct, this.drpVitaminB2Pct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminB3, this.drpVitaminB3, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminB3Pct, this.drpVitaminB3Pct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminB5, this.drpVitaminB5, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminB5Pct, this.drpVitaminB5Pct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminB6, this.drpVitaminB6, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminB6Pct, this.drpVitaminB6Pct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminC, this.drpVitaminC, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminCPct, this.drpVitaminCPct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminD, this.drpVitaminD, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminDPct, this.drpVitaminDPct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.VitaminE, this.drpVitaminE, this.Page);
            Utilities.SetDropDownValue(marketingItem.VitaminEPct, this.drpVitaminEPct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.Potassium, this.drpPotassium, this.Page);
            Utilities.SetDropDownValue(marketingItem.PotassiumPct, this.drpPotassiumPct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.Iron, this.drpIron, this.Page);
            Utilities.SetDropDownValue(marketingItem.IronPct, this.drpIronPct, this.Page);
            //Utilities.SetDropDownValue(marketingItem.Calcium, this.drpCalcium, this.Page);
            Utilities.SetDropDownValue(marketingItem.CalciumPct, this.drpCalciumPct, this.Page);
            Utilities.SetDropDownValue(marketingItem.ClaimsDesired, this.drpDesiredClaims, this.Page);

        }
        private void LoadFormData()
        {
            var objItemProposalService = new ItemProposalService();
            // Load the Original Item Proposal
            ItemProposalItem item = objItemProposalService.GetItemProposalItem(iItemId);
            hdnStageGateListItemId.Value = Convert.ToString(item.StageGateProjectListItemId);
            hdnParentProjectNumber.Value = item.ParentProjectNumber;
            hdnTBDIndicator.Value = item.TBDIndicator;
            hdnNewIPF.Value = item.NewIPF;
            hdnIPFCopiedFromCompassListItemId.Value = Convert.ToString(item.IPFCopiedFromCompassListItemId);
            Utilities.SetDropDownValue(item.TBDIndicator, this.ddlTBDIndicator, this.Page);

            // Force a re-selection of these fields for a copy request
            if (string.Equals(this.IPFMode.ToUpper(), GlobalConstants.QUERYSTRINGVALUE_IPFCopy))
            {
                this.ddlTBDIndicator.SelectedIndex = 0;
            }
            //true if it doesn't contain letters
            bool isChangeRequest = ProjectNumber.Any(x => char.IsLetter(x));
            if (isChangeRequest)
            {
                hdnIsChangeRequest.Value = GlobalConstants.QUERYSTRINGVALUE_IPFChange;
                txtChangeReason.Text = item.ReasonForChange;
            }

            // Proposed project tab
            //txtProposedItem.Text = item.SAPDescription;           
            Utilities.SetDropDownValue(item.ProjectType, this.ddlProjectType, this.Page);
            ddlProjectType.Attributes.Add("onchange", "ProjectTypeChange();SingleProjectTypeChange();ParentProjectTypeChange();SAPNomenclature(ddlProjectType);");
            Utilities.SetDropDownValue(item.ProjectTypeSubCategory, ddlProjectTypeSubCategory, Page);

            // If project has not been submitted yet, set the First Ship Date
            // Otherwise form has been submitted, so use the Revised First Ship Date

            if (objItemProposalService.IPFSubmitted(iItemId))// || item.FirstShipDate == DateTime.MinValue)
            {
                lblFirstShipDate.InnerHtml = "<span class=\"markrequired\">*</span>Revised First Ship Date:";
                txtFirstShipDate.Text = Utilities.GetDateForDisplay(item.RevisedFirstShipDate);
                if (item.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    hdnIPFSubmitted.Value = "Yes";
                    ddlExternalSemisItem.Attributes.Add("disabled", "true");
                }
            }
            else if (item.FirstShipDate != DateTime.MinValue)
            {
                txtFirstShipDate.Text = Utilities.GetDateForDisplay(item.FirstShipDate);
            }

            txtChangeNotes.Text = item.ItemConcept;
            Utilities.SetDropDownValue(item.CopyFormsForGraphicsProject, this.ddlCopyFormsForGraphicsProject, this.Page);
            Utilities.SetDropDownValue(item.ExternalSemisItem, this.ddlExternalSemisItem, this.Page);
            hdnExternalSemisItem.Value = item.ExternalSemisItem;
            txtFlowthroughDets.Text = item.FlowthroughDets;
            // Project Team tab                                     
            LoadProjectTeamData(item);

            // SAP Item # tab
            txtSAPItemNumber.Text = item.SAPItemNumber;
            txtSAPItemDescription.Text = item.SAPDescription;
            txtLikeFGItemNumber.Text = item.LikeFGItemNumber;
            txtLikeItemDescription.Text = item.LikeFGItemDescription;
            txtOldFGItemNumber.Text = item.OldFGItemNumber;
            txtOldItemDescription.Text = item.OldFGItemDescription;

            // Project Specifications tab
            Utilities.SetDropDownValue(item.NewFormula, this.ddlNewFormula, this.Page);
            Utilities.SetDropDownValue(item.Organic, this.ddlOrganic, this.Page);
            Utilities.SetDropDownValue(item.ServingSizeWeightChange, this.ddlServiceSizeChange, this.Page);
            Utilities.SetDropDownValue(item.NewShape, this.ddlNewShape, this.Page);
            Utilities.SetDropDownValue(item.NewFlavorColor, this.ddlNewFlavorColor, this.Page);
            Utilities.SetDropDownValue(item.NewNetWeight, this.ddlNewNetWeight, this.Page);

            // Item Financial Details tab
            txtAnnualProjectedUnits.Text = Utilities.FormatNumber(item.AnnualProjectedUnits);
            txtAnnualProjectedDollars.Text = Utilities.FormatDecimal(item.AnnualProjectedDollars, 0);
            txtTruckLoadSellingPrice.Text = Utilities.FormatCurrency(item.TruckLoadPricePerSellingUnit);
            txtExpectedGrossMarginPercent.Text = Utilities.FormatDecimal(item.ExpectedGrossMarginPercent, 2);
            txtLast12MonthSales.Text = Utilities.FormatDecimal(item.Last12MonthSales, 0);
            txtMonth1ProjectedDollars.Text = Utilities.FormatDecimal(item.Month1ProjectedDollars, 0);
            txtMonth1ProjectedUnits.Text = Utilities.FormatNumber(item.Month1ProjectedUnits);
            txtMonth2ProjectedDollars.Text = Utilities.FormatDecimal(item.Month2ProjectedDollars, 0);
            txtMonth2ProjectedUnits.Text = Utilities.FormatNumber(item.Month2ProjectedUnits);
            txtMonth3ProjectedDollars.Text = Utilities.FormatDecimal(item.Month3ProjectedDollars, 0);
            txtMonth3ProjectedUnits.Text = Utilities.FormatNumber(item.Month3ProjectedUnits);

            // Customer Specifications tab
            Utilities.SetDropDownValue(item.CustomerSpecific, this.ddlCustomerSpecific, this.Page);
            Utilities.SetDropDownValueMatchWithoutCodes(item.Customer, this.ddlCustomer, this.Page);
            txtCustomerSpecificLotCode.Text = item.CustomerSpecificLotCode;
            Utilities.SetDropDownValue(item.Channel, this.ddlChannel, this.Page);
            Utilities.SetDropDownValue(item.CountryOfSale, this.ddlCountryOfSale, this.Page);
            Utilities.SetDropDownValue(item.SoldOutsideUSA, this.ddlOutsideUSA, this.Page);

            // Item Hierarchy tab

            Utilities.SetDropDownValue(item.ManuallyCreateSAPDescription, this.ddlManuallyCreateSAPDescription, this.Page);
            // Load the level 1 list
            Utilities.SetDropDownValue(item.ProductHierarchyLevel1, this.ddlProductHierarchyLevel1, this.Page);
            ddlProductHierarchyLevel1_SelectedIndexChanged(null, null);

            Utilities.SetDropDownValue(item.ProductHierarchyLevel2, this.ddlProductHierarchyLevel2, this.Page);
            // Load the level 2 list
            ddlProductHierarchyLevel2_SelectedIndexChanged(null, null);

            // Load Brand List
            Utilities.SetDropDownValue(item.MaterialGroup1Brand, this.ddlBrand_Material, this.Page);
            ddlBrand_Material_SelectedIndexChanged(null, null);

            //this.ddlProductHierarchyLevel2.ClearSelection();


            Utilities.SetDropDownValue(item.MaterialGroup4ProductForm, this.ddlMaterialGroup4, this.Page);
            txtProductFormDescription.Text = string.IsNullOrEmpty(item.ProductFormDescription) ? string.Empty : item.ProductFormDescription.ToUpper();
            Utilities.SetDropDownValue(item.MaterialGroup5PackType, this.ddlMaterialGroup5, this.Page);
            Utilities.SetDropDownValue(item.NovelyProject, this.drpNovelyProject, this.Page);

            txtFGTotalQuantityUnitsInDisplay.Text = item.TotalQuantityUnitsInDisplay;

            // Item UPCs tab
            Utilities.SetDropDownValue(item.RequireNewUPCUCC, this.ddlNeedNewUPCUCC, this.Page);
            Utilities.SetDropDownValue(item.RequireNewUnitUPC, this.ddlNeedNewUnitUPC, this.Page);
            txtUnitUPC.Text = item.UnitUPC;
            Utilities.SetDropDownValue(item.RequireNewDisplayBoxUPC, this.ddlNeedNewDisplayBoxUPC, this.Page);
            txtDisplayUPCBox.Text = item.DisplayBoxUPC;
            Utilities.SetDropDownValue(item.SAPBaseUOM, this.ddlSAPBUOM, this.Page);
            Utilities.SetDropDownValue(item.RequireNewCaseUCC, this.ddlNeedNewCaseUCC, this.Page);
            txtCaseUCC.Text = item.CaseUCC;
            Utilities.SetDropDownValue(item.RequireNewPalletUCC, this.ddlNeedNewPalletUCC, this.Page);
            txtPalletUCC.Text = item.PalletUCC;

            // Additonal Item Details tab
            Utilities.SetDropDownValue(item.CaseType, this.ddlCaseType, this.Page);
            txtClaimsLabelingRequirements.Text = item.MarketClaimsLabelingRequirements;
            Utilities.SetDropDownValue(item.FilmSubstrate, this.ddlFilmSubstrate, this.Page);
            Utilities.SetDropDownValue(item.PegHoleNeeded, this.ddlPegHoleNeeded, this.Page);
            Utilities.SetDropDownValue(item.InvolvesCarton, this.ddlInvolvesCarton, this.Page);
            txtUnitsInsideCarton.Text = item.UnitsInsideCarton;
            txtIndividualPouchWeight.Text = Utilities.FormatDecimal(item.IndividualPouchWeight, 2);
            txtRetailSellingUnitsPerBaseUOM.Text = Utilities.FormatDecimal(item.RetailSellingUnitsBaseUOM, 0);
            txtNumberofTraysPerBaseUOM.Text = Utilities.FormatDecimal(item.NumberofTraysPerBaseUOM, 0);
            txtRetailUnitWeight.Text = Utilities.FormatDecimal(item.RetailUnitWieghtOz, 2);
            txtBaseUofMNetWeight.Text = Utilities.FormatDecimal(item.BaseUOMNetWeightLbs, 2);
            hdnPLMFlag.Value = item.PLMFlag;
        }
        private void LoadProjectTeamData(ItemProposalItem item)
        {
            #region Project Team
            //ProjectLeader
            var ProjectLeaders = Utilities.SetPeoplePickerValue(item.ProjectLeader, SPContext.Current.Web);
            LoadProjectTeamMembers(ProjectLeaders, item.ProjectLeaderName, "rptProjectLeaders", divProjectLeaders);

            //ProjectManager
            var ProjectManager = Utilities.SetPeoplePickerValue(item.PM, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlProjectManagerMembers, GlobalConstants.GROUP_ProjectManagers, true);
            LoadProjectTeamMembers_New(this.ddlProjectManagerMembers, ProjectManager, item.PMName, rptProjectManagers);

            //SeniorProjectManager
            var SeniorProjectManager = Utilities.SetPeoplePickerValue(item.SrProjectManager, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlSrProjectManagerMembers, GlobalConstants.GROUP_SeniorProjectManager, true);
            LoadProjectTeamMembers_New(this.ddlSrProjectManagerMembers, SeniorProjectManager, item.SrProjectManagerName, rptSrProjectManagers);

            //Marketing
            var Marketing = Utilities.SetPeoplePickerValue(item.Marketing, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlMarketingMembers, GlobalConstants.GROUP_Marketing, true);
            LoadProjectTeamMembers_New(this.ddlMarketingMembers, Marketing, item.MarketingName, rptMarketingMembers);

            //InTech (formal RnD)
            var InTech = Utilities.SetPeoplePickerValue(item.InTech, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlInTechMembers, GlobalConstants.GROUP_InTech, true);
            LoadProjectTeamMembers_New(this.ddlInTechMembers, InTech, item.InTechName, rptInTechMembers);

            //QAInnovation
            var QAInnovation = Utilities.SetPeoplePickerValue(item.QA, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlQualityInnovationMembers, GlobalConstants.GROUP_QualityInnovation, true);
            LoadProjectTeamMembers_New(this.ddlQualityInnovationMembers, QAInnovation, item.QAName, rptQualityInnovationMembers);

            //InTechRegulatory - (Earlier it was RegulatoryRnD)
            var InTechRegulatory = Utilities.SetPeoplePickerValue(item.InTechRegulatory, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlInTechRegulatoryMembers, GlobalConstants.GROUP_InTechRegulatory, true);
            LoadProjectTeamMembers_New(this.ddlInTechRegulatoryMembers, InTechRegulatory, item.InTechRegulatoryName, rptInTechRegulatoryMembers);

            //RegulatoryQA
            var RegulatoryQA = Utilities.SetPeoplePickerValue(item.RegulatoryQA, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlRegulatoryQAMembers, GlobalConstants.GROUP_RegulatoryQA, true);
            LoadProjectTeamMembers_New(this.ddlRegulatoryQAMembers, RegulatoryQA, item.RegulatoryQAName, rptRegulatoryQAMembers);

            //PackagingEngineering
            var PackagingEngineering = Utilities.SetPeoplePickerValue(item.PackagingEngineering, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlPackagingEngineeringMembers, GlobalConstants.GROUP_PackagingEngineer, true);
            LoadProjectTeamMembers_New(this.ddlPackagingEngineeringMembers, PackagingEngineering, item.PackagingEngineeringName, rptPackagingEngineeringMembers);

            //SupplyChain
            var SupplyChain = Utilities.SetPeoplePickerValue(item.SupplyChain, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlSupplyChainMembers, GlobalConstants.GROUP_SupplyChain, true);
            LoadProjectTeamMembers_New(this.ddlSupplyChainMembers, SupplyChain, item.SupplyChainName, rptSupplyChainMembers);

            //Finance
            var Finance = Utilities.SetPeoplePickerValue(item.Finance, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlFinanceMembers, GlobalConstants.GROUP_Finance, true);
            LoadProjectTeamMembers_New(this.ddlFinanceMembers, Finance, item.FinanceName, rptFinanceMembers);

            //Sales
            var Sales = Utilities.SetPeoplePickerValue(item.Sales, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlSalesMembers, GlobalConstants.GROUP_Sales, true);
            LoadProjectTeamMembers_New(this.ddlSalesMembers, Sales, item.SalesName, rptSalesMembers);

            //Manufacturing
            var Manufacturing = Utilities.SetPeoplePickerValue(item.Manufacturing, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlManufacturingMembers, GlobalConstants.GROUP_Manufacturing, true);
            LoadProjectTeamMembers_New(this.ddlManufacturingMembers, Manufacturing, item.ManufacturingName, rptManufacturingMembers);

            //External Mfg - Procurement
            var ExtMfgProcurement = Utilities.SetPeoplePickerValue(item.ExtManufacturingProc, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlExternalMfgProcurementMembers, GlobalConstants.GROUP_ExternalManufacturing, true);
            LoadProjectTeamMembers_New(this.ddlExternalMfgProcurementMembers, ExtMfgProcurement, item.ExtManufacturingProcName, rptExternalMfgProcurementMembers);

            //Packaging Procurement
            var PackagingProcurement = Utilities.SetPeoplePickerValue(item.PackagingProcurement, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlPackagingProcurementMembers, GlobalConstants.GROUP_ProcurementPackaging, true);
            LoadProjectTeamMembers_New(this.ddlPackagingProcurementMembers, PackagingProcurement, item.PackagingProcurementName, rptPackagingProcurementMembers);

            //Life Cycle Management
            var LifeCycleManagement = Utilities.SetPeoplePickerValue(item.LifeCycleManagement, SPContext.Current.Web);
            Utilities.BindGroupMembersToDropDown(this.ddlLifeCycleManagementMembers, GlobalConstants.GROUP_LifeCycleMngmt, true);
            LoadProjectTeamMembers_New(this.ddlLifeCycleManagementMembers, LifeCycleManagement, item.LifeCycleManagementName, rptLifeCycleManagementMembers);

            //Other Members
            var OtherMember = Utilities.SetPeoplePickerValue(item.OtherTeamMembers, SPContext.Current.Web);
            LoadProjectTeamMembers(OtherMember, item.OtherTeamMembersName, "rptOtherTeamMembers", divOtherTeamMembers);
            #endregion
            // Set the Initiator people picker
            var users = Utilities.SetPeoplePickerValue(item.Initiator, SPContext.Current.Web);
            if (!string.IsNullOrEmpty(users))
            {
                peInitiator.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1);
            }
        }
        private void LoadSummaryProjectTeamData(ItemProposalItem item)
        {
            lblSummaryInitiator.Text = Utilities.GetPersonFieldForDisplay(item.Initiator);
            lblSummaryProjectLeader.Text = item.ProjectLeaderName;
            lblSummaryProjectManager.Text = item.PMName;
            lblSummarySrProjectManager.Text = item.SrProjectManagerName;
            lblSummaryMarketing.Text = item.MarketingName;
            lblSummaryInTech.Text = item.InTechName;
            lblSummaryQualityInnovation.Text = item.QAName;
            lblSummaryInTechRegulatory.Text = item.InTechRegulatoryName;
            lblSummaryRegulatoryQA.Text = item.RegulatoryQAName;
            lblSummaryPackagingEngineering.Text = item.PackagingEngineeringName;
            lblSummarySupplyChain.Text = item.SupplyChainName;
            lblSummaryFinance.Text = item.FinanceName;
            lblSummarySales.Text = item.SalesName;
            lblSummaryManufacturing.Text = item.ManufacturingName;
            lblSummaryExternalMfgProcurement.Text = item.ExtManufacturingProcName;
            lblSummaryPackagingProcurement.Text = item.PackagingProcurementName;
            lblSummaryLifeCycleManagement.Text = item.LifeCycleManagementName;
            lblSummaryOtherTeamMembers.Text = item.OtherTeamMembersName;
        }
        private string LoadProjectTeamMembers(string Members, string MemberNames, string RepeaterName, HtmlGenericControl div)
        {
            try
            {
                List<string> listTeamMemberNames = new List<string>();
                List<int> NAList = new List<int>();

                if (!string.IsNullOrEmpty(MemberNames))
                {
                    if (MemberNames.LastIndexOf(";") != -1)
                    {
                        MemberNames = MemberNames.Remove(MemberNames.LastIndexOf(";"), 1);
                    }

                    listTeamMemberNames = MemberNames.Split(';').ToList();

                    for (int i = 0; i < listTeamMemberNames.Count; i++)
                    {
                        if (CheckForNA(listTeamMemberNames[i])) NAList.Add(i);
                    }
                }

                List<string> listTeamMembers = new List<string>();
                if (!string.IsNullOrEmpty(Members))
                {
                    Members = Members.Remove(Members.LastIndexOf(","), 1);
                    listTeamMembers = Members.Split(',').ToList();
                }

                for (int i = 0; i < NAList.Count; i++)
                {
                    listTeamMembers.Insert(NAList[i], "NA");
                }

                if (listTeamMembers.Count == 0)
                {
                    listTeamMembers.Add(string.Empty);
                }

                Repeater Repeater = (Repeater)div.FindControl(RepeaterName);
                Repeater.DataSource = listTeamMembers;
                Repeater.DataBind();
            }
            catch (Exception ex)
            {
                string strErrors = string.Concat("Exception while loading project team member .", " - RepeaterName:", RepeaterName, "- MemberNames:", MemberNames);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": " + strErrors);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "LoadProjectTeamMembers", strErrors);
            }
            return Members;
        }
        public class TeamMember
        {
            private string mMemberName;
            private string mMemberLoginName;
            public string MemberName { get { return mMemberName; } set { mMemberName = value; } }
            public string MemberLoginName { get { return mMemberLoginName; } set { mMemberLoginName = value; } }
        }
        private string LoadProjectTeamMembers_New(DropDownList ddlMember, string Members, string MemberNames, Repeater Repeater)
        {
            try
            {
                if (!string.IsNullOrEmpty(MemberNames))
                {
                    List<TeamMember> TeamMembers = new List<TeamMember>();
                    List<int> NAList = new List<int>();
                    List<string> listTeamMembers = new List<string>();
                    List<string> listTeamMemberNames = new List<string>();

                    if (MemberNames.LastIndexOf(";") != -1)
                    {
                        MemberNames = MemberNames.Remove(MemberNames.LastIndexOf(";"), 1);
                    }

                    if (!string.IsNullOrEmpty(Members))
                    {
                        Members = Members.Remove(Members.LastIndexOf(","), 1);
                    }

                    listTeamMemberNames = MemberNames.Split(';').ToList();
                    listTeamMembers = Members.Split(',').ToList();

                    for (int i = 0; i < listTeamMemberNames.Count; i++)
                    {
                        if (CheckForNA(listTeamMemberNames[i])) NAList.Add(i);
                    }

                    for (int i = 0; i < NAList.Count; i++)
                    {
                        listTeamMembers.Insert(NAList[i], "NA");
                    }

                    for (int i = 0; i < listTeamMemberNames.Count; i++)
                    {
                        TeamMembers.Add(new TeamMember() { MemberName = listTeamMemberNames[i], MemberLoginName = listTeamMembers[i] });
                    }
                    Utilities.SetDropDownValue(TeamMembers.FirstOrDefault().MemberName, ddlMember, Page);
                    TeamMembers.RemoveAt(0);

                    Repeater.DataSource = TeamMembers;
                    Repeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrors = string.Concat("Exception while loading project team member .", " - RepeaterName:", Repeater.ID, "- MemberNames:", MemberNames);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": " + strErrors);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "LoadProjectTeamMembers_New", strErrors);
            }
            return MemberNames;
        }
        private void LoadBlankProjectTeamber()
        {
            //ProjectLeader
            LoadProjectTeamMembers(string.Empty, string.Empty, "rptProjectLeaders", divProjectLeaders);

            //Other Members
            LoadProjectTeamMembers(string.Empty, string.Empty, "rptOtherTeamMembers", divOtherTeamMembers);
        }
        private Dictionary<string, string> ConstructMembers(ref int repeaterCount, string MemberName, HtmlGenericControl div, string RepeaterName, string PeopleEditorName, string HiddenStatusFieldName, bool Required, bool Submitting)
        {
            Repeater rptMembers = ((Repeater)div.FindControl(RepeaterName));
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            bool NA;
            string NAText;

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMember = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        try
                        {
                            if (peMember.Entities.Count <= 0 && Required && Submitting)
                            {
                                InvalidPeopleEditor = true;
                                string strErrors = "Please enter " + MemberName + " member.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                                ErrorSummary.AddError(strErrors, this.Page);
                            }

                            if (peMember.Entities.Count > 0)
                            {
                                CheckForNA(out NA, out NAText, peMember);

                                if (NA)
                                {
                                    MembersNames += NAText + ";";
                                }
                                else
                                {
                                    Members.AddRange(Utilities.GetPeopleFromPickerControl(peMember, SPContext.Current.Web));
                                    MembersNames += Utilities.GetNamesFromPickerControl(peMember, SPContext.Current.Web) + ";";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            InvalidPeopleEditor = true;
                            string strErrors = "Invalid " + MemberName + " member was entered. Please re-enter. <a href ='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": " + strErrors);
                            exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "ConstructMembers", strErrors);
                        }
                        repeaterCount++;
                    }
                }
            }

            return new Dictionary<string, string>()
            {
                { "Member", Members.ToString() },
                { "MemberName",MembersNames }
            };
        }
        private Dictionary<string, string> ConstructMembers_New(ref int repeaterCount, DropDownList ddlMember, string MemberName, Repeater rptMembers, string txtBoxName, string HiddenStatusFieldName, bool Required, bool Submitting)
        {
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();

            if (ddlMember.SelectedItem.Value != "-1")
            {
                MembersNames += ddlMember.SelectedItem.Text + ";";
                if (!string.IsNullOrEmpty(ddlMember.SelectedItem.Value) && !CheckForNA(ddlMember.SelectedItem.Value))
                {
                    Members.AddRange(Utilities.GetPeopleFromPickerControl(ddlMember.SelectedItem.Value, SPContext.Current.Web));
                }
            }

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtMember = (TextBox)item.FindControl(txtBoxName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        try
                        {
                            MembersNames += txtMember.Text + ";";
                            TextBox txtBoxNameLoginName = (TextBox)item.FindControl(string.Concat(txtBoxName + "LoginName"));

                            if (!string.IsNullOrEmpty(txtBoxNameLoginName.Text) && !CheckForNA(txtBoxNameLoginName.Text))
                            {
                                Members.AddRange(Utilities.GetPeopleFromPickerControl(txtBoxNameLoginName.Text, SPContext.Current.Web));
                            }
                        }
                        catch (Exception ex)
                        {
                            InvalidPeopleEditor = true;
                            string strErrors = "Invalid " + MemberName + " member was selected. <a href ='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": " + strErrors);
                            exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "ConstructMembers_New", strErrors);
                        }
                        repeaterCount++;
                    }
                }
            }

            return new Dictionary<string, string>()
            {
                { "Member", Members.ToString() },
                { "MemberName",MembersNames }
            };
        }
        private static bool CheckForNA(string stateGateItemName)
        {
            return (stateGateItemName.ToUpper() == "NA" || stateGateItemName.ToUpper() == "NOT APPLICABLE" || stateGateItemName.ToUpper() == "N/A");
        }
        private void LoadFormulationAttachments()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_Formulation);

            if (files.Count > 0)
            {
                rpFormulationAttachments.Visible = true;
                rpFormulationAttachments.DataSource = files;
                rpFormulationAttachments.DataBind();

                rpSummaryFormulationAttachments.Visible = true;
                rpSummaryFormulationAttachments.DataSource = files;
                rpSummaryFormulationAttachments.DataBind();
            }
            else
            {
                rpFormulationAttachments.Visible = false;
                rpSummaryFormulationAttachments.Visible = false;
            }
        }
        private void LoadGraphicsAttachments()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_Graphics);

            if (files.Count > 0)
            {
                rpGraphicsAttachments.Visible = true;
                rpGraphicsAttachments.DataSource = files;
                rpGraphicsAttachments.DataBind();

                rpSummaryGraphicsAttachments.Visible = true;
                rpSummaryGraphicsAttachments.DataSource = files;
                rpSummaryGraphicsAttachments.DataBind();
            }
            else
            {
                rpGraphicsAttachments.Visible = false;
                rpSummaryGraphicsAttachments.Visible = false;
            }
        }
        private ItemProposalItem ConstructFormData(bool Submitting)
        {
            ItemProposalItem item = new ItemProposalItem();
            string strErrors = string.Empty;
            int RepeaterCount = 1;
            try
            {
                item.CompassListItemId = iItemId;

                // Check if this is a copy or change request
                if (string.IsNullOrEmpty(ProjectNumber))
                {
                    // New Project, so just Get the Next Project Number
                    item.ProjectNumber = Utilities.GetNextProjectNumber();
                }
                else if (!string.IsNullOrEmpty(this.IPFMode))
                {
                    if (string.Equals(this.IPFMode.ToUpper(), GlobalConstants.QUERYSTRINGVALUE_IPFCopy))
                    {
                        // Copy Request, so just Get the Next Project Number
                        item.ProjectNumber = Utilities.GetNextProjectNumber();
                        newProjectNumber = item.ProjectNumber;
                    }
                    else if (string.Equals(this.IPFMode.ToUpper(), GlobalConstants.QUERYSTRINGVALUE_IPFChange))
                    {
                        // Change Request, so just Determine the Next Project Change Request Number
                        newProjectNumber = Utilities.DetermineChangeRequestProjectNumber(ProjectNumber);
                        // Make sure generated project number is not a duplicate. If it is, generate the next one
                        while (itemProposalService.IsExistingProjectNo(newProjectNumber))
                            newProjectNumber = Utilities.DetermineChangeRequestProjectNumber(newProjectNumber);
                        item.ProjectNumber = newProjectNumber;
                    }
                }
                if (!string.IsNullOrEmpty(this.TestProject))
                {
                    if (this.TestProject.ToLower() == "true" || this.TestProject.ToLower() == "yes")
                    {
                        item.TestProject = "Yes";
                    }
                }
                // Proposed project tab
                //item.ItemDescription = txtProposedItem.Text.Trim();
                item.ProjectType = ddlProjectType.SelectedItem.Text.Trim();
                item.ProjectTypeSubCategory = ddlProjectTypeSubCategory.SelectedItem.Text;

                var objItemProposalService = new ItemProposalService();

                if (objItemProposalService.IPFSubmitted(iItemId))
                {
                    item.RevisedFirstShipDate = string.IsNullOrEmpty(txtFirstShipDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFirstShipDate.Text.Trim());
                }
                else
                {
                    item.FirstShipDate = string.IsNullOrEmpty(txtFirstShipDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFirstShipDate.Text.Trim());
                    item.RevisedFirstShipDate = string.IsNullOrEmpty(txtFirstShipDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFirstShipDate.Text.Trim());
                }

                item.CopyFormsForGraphicsProject = ddlCopyFormsForGraphicsProject.SelectedItem.Text.Trim();
                item.ExternalSemisItem = (hdnIPFSubmitted.Value == "Yes") ? hdnExternalSemisItem.Value : ddlExternalSemisItem.SelectedItem.Text.Trim();
                item.ProductHierarchyLevel1 = ddlProductHierarchyLevel1.SelectedItem.Text;
                item.ManuallyCreateSAPDescription = ddlManuallyCreateSAPDescription.SelectedItem.Text;
                item.MaterialGroup1Brand = ddlBrand_Material.SelectedItem.Text.Trim();
                item.ItemConcept = txtChangeNotes.Text.Trim();

                // Project Team tab
                //Project Team
                List<string> users = new List<string>();
                #region Project Team                
                #region Initiator
                if (!string.IsNullOrEmpty(this.IPFMode))
                {
                    if (string.Equals(this.IPFMode.ToUpper(), GlobalConstants.QUERYSTRINGVALUE_IPFCopy))
                    {
                        SetInitiator();
                    }
                    else if (string.Equals(this.IPFMode.ToUpper(), GlobalConstants.QUERYSTRINGVALUE_IPFChange) && (!string.Equals(this.ProjectRejected, "Yes")))
                    {
                        SetInitiator();
                    }
                }

                try
                {
                    if (peInitiator.Entities.Count > 0)
                    {
                        item.Initiator = Utilities.GetPeopleFromPickerControl(peInitiator, SPContext.Current.Web).ToString();
                        item.InitiatorName = Utilities.GetNamesFromPickerControl(peInitiator, SPContext.Current.Web);
                        users.Add(item.Initiator);
                    }
                }
                catch (Exception ex)
                {
                    strErrors = "Invalid Initiator was entered. Please re-enter.<a href='javascript:setFocus(&quot;peInitiator&quot;)'>  [Update]</a>";
                    ErrorSummary.AddError(strErrors, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": " + strErrors);
                    exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "ConstructFormData", strErrors);
                }
                #endregion

                #region ProjectLeader
                Dictionary<string, string> ProjectLeader = ConstructMembers(ref RepeaterCount, "Project Leader", divProjectLeaders, "rptProjectLeaders", "peProjectLeaderMembers", "hdnDeletedStatusForProjectLeader", true, Submitting);
                item.ProjectLeader = ProjectLeader["Member"];
                item.ProjectLeaderName = ProjectLeader["MemberName"];
                users.AddRange(item.ProjectLeader.Split(';').ToList());
                #endregion

                #region ProjectManager
                Dictionary<string, string> ProjectManagers = ConstructMembers_New(ref RepeaterCount, ddlProjectManagerMembers, "Project Manager", rptProjectManagers, "txtProjectManagerMembers", "hdnDeletedStatusForProjectManagerMembers", true, Submitting);
                item.PM = ProjectManagers["Member"];
                item.PMName = ProjectManagers["MemberName"];
                users.AddRange(item.PM.Split(';').ToList());
                #endregion

                #region SeniorProjectManager
                Dictionary<string, string> SrProjectManagers = ConstructMembers_New(ref RepeaterCount, ddlSrProjectManagerMembers, "Sr. Project Manager", rptSrProjectManagers, "txtSrProjectManagerMembers", "hdnDeletedStatusForSrProjectManagerMembers", true, Submitting);
                item.SrProjectManager = SrProjectManagers["Member"];
                item.SrProjectManagerName = SrProjectManagers["MemberName"];
                users.AddRange(item.SrProjectManager.Split(';').ToList());
                #endregion

                #region Marketing
                Dictionary<string, string> MarketingMembers = ConstructMembers_New(ref RepeaterCount, ddlMarketingMembers, "Marketing", rptMarketingMembers, "txtMarketingMembers", "hdnDeletedStatusForMarketingMembers", true, Submitting);
                item.Marketing = MarketingMembers["Member"];
                item.MarketingName = MarketingMembers["MemberName"];
                users.AddRange(item.Marketing.Split(';').ToList());
                #endregion

                #region ResearchAndDevelopment / InTech
                bool required = hdnRequiredInTechMembers.Value == "True" ? true : false;
                Dictionary<string, string> ResearchAndDevelopmentMembers = ConstructMembers_New(ref RepeaterCount, ddlInTechMembers, "InTech", rptInTechMembers, "txtInTechMembers", "hdnDeletedStatusForInTechMembers", required, Submitting);
                item.InTech = ResearchAndDevelopmentMembers["Member"];
                item.InTechName = ResearchAndDevelopmentMembers["MemberName"];
                users.AddRange(item.InTech.Split(';').ToList());
                #endregion

                #region QA Innovation 
                required = hdnRequiredQualityInnovationMembers.Value == "True" ? true : false;
                Dictionary<string, string> QualityInnovationMembers = ConstructMembers_New(ref RepeaterCount, ddlQualityInnovationMembers, "Quality Innovation", rptQualityInnovationMembers, "txtQualityInnovationMembers", "hdnDeletedStatusForQualityInnovationMembers", required, Submitting);
                item.QA = QualityInnovationMembers["Member"];
                item.QAName = QualityInnovationMembers["MemberName"];
                users.AddRange(item.QA.Split(';').ToList());
                #endregion

                #region InTech
                required = hdnRequiredInTechRegulatoryMembers.Value == "True" ? true : false;
                Dictionary<string, string> InTechRegulatoryMembers = ConstructMembers_New(ref RepeaterCount, ddlInTechRegulatoryMembers, "InTech Regulatory", rptInTechRegulatoryMembers, "txtInTechRegulatoryMembers", "hdnDeletedStatusForInTechRegulatoryMembers", required, Submitting);
                item.InTechRegulatory = InTechRegulatoryMembers["Member"];
                item.InTechRegulatoryName = InTechRegulatoryMembers["MemberName"];
                users.AddRange(item.InTechRegulatory.Split(';').ToList());
                #endregion

                #region RegulatoryQA
                required = hdnRequiredRegulatoryQAMembers.Value == "True" ? true : false;
                Dictionary<string, string> RegulatoryQAMembers = ConstructMembers_New(ref RepeaterCount, ddlRegulatoryQAMembers, "Regulatory QA", rptRegulatoryQAMembers, "txtRegulatoryQAMembers", "hdnDeletedStatusForRegulatoryQAMembers", required, Submitting);
                item.RegulatoryQA = RegulatoryQAMembers["Member"];
                item.RegulatoryQAName = RegulatoryQAMembers["MemberName"];
                users.AddRange(item.RegulatoryQA.Split(';').ToList());
                #endregion

                #region Packaging Engineering
                Dictionary<string, string> PackagingEngineeringMembers = ConstructMembers_New(ref RepeaterCount, ddlPackagingEngineeringMembers, "Packaging Engineering", rptPackagingEngineeringMembers, "txtPackagingEngineeringMembers", "hdnDeletedStatusForPackagingEngineeringMembers", true, Submitting);
                item.PackagingEngineering = PackagingEngineeringMembers["Member"];
                item.PackagingEngineeringName = PackagingEngineeringMembers["MemberName"];
                users.AddRange(item.PackagingEngineering.Split(';').ToList());
                #endregion

                #region Supply Chain
                Dictionary<string, string> SupplyChainMembers = ConstructMembers_New(ref RepeaterCount, ddlSupplyChainMembers, "Supply Chain", rptSupplyChainMembers, "txtSupplyChainMembers", "hdnDeletedStatusForSupplyChainMembers", true, Submitting);
                item.SupplyChain = SupplyChainMembers["Member"];
                item.SupplyChainName = SupplyChainMembers["MemberName"];
                users.AddRange(item.SupplyChain.Split(';').ToList());
                #endregion

                #region Finance
                Dictionary<string, string> FinanceMembers = ConstructMembers_New(ref RepeaterCount, ddlFinanceMembers, "Finance", rptFinanceMembers, "txtFinanceMembers", "hdnDeletedStatusForFinanceMembers", true, Submitting);
                item.Finance = FinanceMembers["Member"];
                item.FinanceName = FinanceMembers["MemberName"];
                users.AddRange(item.Finance.Split(';').ToList());
                #endregion

                #region Sales
                required = hdnRequiredSalesMembers.Value == "True" ? true : false;
                Dictionary<string, string> SalesMembers = ConstructMembers_New(ref RepeaterCount, ddlSalesMembers, "Sales", rptSalesMembers, "txtSalesMembers", "hdnDeletedStatusForSalesMembers", true, Submitting);
                item.Sales = SalesMembers["Member"];
                item.SalesName = SalesMembers["MemberName"];
                users.AddRange(item.Sales.Split(';').ToList());
                #endregion

                #region Manufacturing
                required = hdnRequiredManufacturingMembers.Value == "True" ? true : false;
                Dictionary<string, string> ManufacturingMembers = ConstructMembers_New(ref RepeaterCount, ddlManufacturingMembers, "Manufacturing", rptManufacturingMembers, "txtManufacturingMembers", "hdnDeletedStatusForManufacturingMembers", required, Submitting);
                item.Manufacturing = ManufacturingMembers["Member"];
                item.ManufacturingName = ManufacturingMembers["MemberName"];
                users.AddRange(item.Manufacturing.Split(';').ToList());
                #endregion

                #region External Mfg - Procurement
                required = hdnRequiredExternalMfgProcurementMembers.Value == "True" ? true : false;
                Dictionary<string, string> ExternalMfgProcurementMembers = ConstructMembers_New(ref RepeaterCount, ddlExternalMfgProcurementMembers, "External Mfg - Procurement", rptExternalMfgProcurementMembers, "txtExternalMfgProcurementMembers", "hdnDeletedStatusForExternalMfgProcurementMembers", required, Submitting);
                item.ExtManufacturingProc = ExternalMfgProcurementMembers["Member"];
                item.ExtManufacturingProcName = ExternalMfgProcurementMembers["MemberName"];
                users.AddRange(item.ExtManufacturingProc.Split(';').ToList());
                #endregion

                #region Packaging Procurement
                required = hdnRequiredPackagingProcurementMembers.Value == "True" ? true : false;
                Dictionary<string, string> PackagingProcurementMembers = ConstructMembers_New(ref RepeaterCount, ddlPackagingProcurementMembers, "Packaging Procurement", rptPackagingProcurementMembers, "txtPackagingProcurementMembers", "hdnDeletedStatusForPackagingProcurementMembers", required, Submitting);
                item.PackagingProcurement = PackagingProcurementMembers["Member"];
                item.PackagingProcurementName = PackagingProcurementMembers["MemberName"];
                users.AddRange(item.PackagingProcurement.Split(';').ToList());
                #endregion

                #region Life Cycle Management
                required = hdnRequiredLifeCycleManagementMembers.Value == "True" ? true : false;
                Dictionary<string, string> LifeCycleManagementMembers = ConstructMembers_New(ref RepeaterCount, ddlLifeCycleManagementMembers, "Life Cycle Management", rptLifeCycleManagementMembers, "txtLifeCycleManagementMembers", "hdnDeletedStatusForLifeCycleManagementMembers", required, Submitting);
                item.LifeCycleManagement = LifeCycleManagementMembers["Member"];
                item.LifeCycleManagementName = LifeCycleManagementMembers["MemberName"];
                users.AddRange(item.LifeCycleManagement.Split(';').ToList());
                #endregion

                #region Team Members
                Dictionary<string, string> OtherTeamMembers = ConstructMembers(ref RepeaterCount, "Other Team", divOtherTeamMembers, "rptOtherTeamMembers", "peOtherTeamMembers", "hdnDeletedStatusForOtherTeamMembers", false, Submitting);
                item.OtherTeamMembers = OtherTeamMembers["Member"];
                item.OtherTeamMembersName = OtherTeamMembers["MemberName"];
                users.AddRange(item.OtherTeamMembers.Split(';').ToList());
                #endregion

                List<string> finalUsers = new List<string>();
                foreach (string user in users)
                {
                    string userTrimmed = Regex.Replace(user, "[^0-9.]", "");
                    int id;
                    if (int.TryParse(userTrimmed, out id))
                    {
                        if (id > 0)
                        {
                            finalUsers.Add(id.ToString());
                        }
                    }
                }
                List<string> finalUsersDistinct = finalUsers.Select(x => x).Distinct().ToList<string>();
                item.AllUsers = String.Join(",", finalUsersDistinct);
                if (!item.AllUsers.StartsWith(","))
                {
                    item.AllUsers = "," + item.AllUsers;
                }
                if (!item.AllUsers.EndsWith(","))
                {
                    item.AllUsers = item.AllUsers + ",";
                }
                #endregion

                // SAP Item # tab
                if (string.Equals(item.ProjectType, GlobalConstants.PROJECTTYPE_GraphicsChangeOnly))
                {
                    item.TBDIndicator = "No";
                    item.NewFormula = "No";
                    item.NewShape = "No";
                    item.NewFlavorColor = "No";
                    item.NewNetWeight = "No";
                    item.RequireNewUPCUCC = "No";
                }
                else if (string.Equals(item.ProjectType, GlobalConstants.PROJECTTYPE_SimpleNetworkMove))
                {
                    item.TBDIndicator = "No";
                    item.NewFormula = "";
                    item.NewShape = "";
                    item.NewFlavorColor = "";
                    item.NewNetWeight = "";
                    item.RequireNewUPCUCC = "";
                }
                else
                {
                    item.TBDIndicator = ddlTBDIndicator.SelectedItem.Text.Trim();
                    item.NewFormula = ddlNewFormula.SelectedItem.Text.Trim();
                    item.NewShape = ddlNewShape.SelectedItem.Text.Trim();
                    item.NewFlavorColor = ddlNewFlavorColor.SelectedItem.Text.Trim();
                    item.NewNetWeight = ddlNewNetWeight.SelectedItem.Text.Trim();
                    item.RequireNewUPCUCC = ddlNeedNewUPCUCC.SelectedItem.Text.Trim();
                }
                item.ServingSizeWeightChange = ddlServiceSizeChange.SelectedItem.Text.Trim();
                item.SAPItemNumber = txtSAPItemNumber.Text.Trim();
                item.SAPDescription = txtSAPItemDescription.Text.Trim();
                item.LikeFGItemNumber = txtLikeFGItemNumber.Text.Trim();
                item.LikeFGItemDescription = txtLikeItemDescription.Text.Trim();
                item.OldFGItemNumber = txtOldFGItemNumber.Text.Trim();
                item.OldFGItemDescription = txtOldItemDescription.Text.Trim();

                // Project Specifications tab

                item.Organic = ddlOrganic.SelectedItem.Text.Trim();

                // Item Financial Details tab
                if (string.Equals(item.ProjectType, GlobalConstants.PROJECTTYPE_GraphicsChangeOnly))
                {
                    item.AnnualProjectedUnits = 0;
                    item.AnnualProjectedDollars = 0;
                    item.TruckLoadPricePerSellingUnit = 0;
                    item.ExpectedGrossMarginPercent = 0;
                    item.Last12MonthSales = 0;
                    item.Month1ProjectedDollars = 0;
                    item.Month1ProjectedUnits = 0;
                    item.Month2ProjectedDollars = 0;
                    item.Month2ProjectedUnits = 0;
                    item.Month3ProjectedDollars = 0;
                    item.Month3ProjectedUnits = 0;
                }
                else
                {
                    item.AnnualProjectedUnits = Utilities.GetNumber(txtAnnualProjectedUnits.Text.Trim());
                    item.AnnualProjectedDollars = Utilities.GetDecimal(txtAnnualProjectedDollars.Text.Trim());
                    item.TruckLoadPricePerSellingUnit = Utilities.GetCurrency(txtTruckLoadSellingPrice.Text.Trim());
                    item.ExpectedGrossMarginPercent = Utilities.GetDecimal(txtExpectedGrossMarginPercent.Text.Trim());
                    item.Last12MonthSales = Utilities.GetDecimal(txtLast12MonthSales.Text);
                    item.Month1ProjectedDollars = Utilities.GetDecimal(txtMonth1ProjectedDollars.Text.Trim());
                    item.Month1ProjectedUnits = Utilities.GetNumber(txtMonth1ProjectedUnits.Text.Trim());
                    item.Month2ProjectedDollars = Utilities.GetDecimal(txtMonth2ProjectedDollars.Text.Trim());
                    item.Month2ProjectedUnits = Utilities.GetNumber(txtMonth2ProjectedUnits.Text.Trim());
                    item.Month3ProjectedDollars = Utilities.GetDecimal(txtMonth3ProjectedDollars.Text.Trim());
                    item.Month3ProjectedUnits = Utilities.GetNumber(txtMonth3ProjectedUnits.Text.Trim());
                }


                // Customer Specifications tab
                item.CustomerSpecific = ddlCustomerSpecific.SelectedItem.Text.Trim();
                var customerList = ddlCustomer.SelectedItem.Text.Split('(');
                if (customerList != null)
                {
                    item.Customer = customerList[0].Trim();
                }
                item.CustomerSpecificLotCode = txtCustomerSpecificLotCode.Text.Trim();
                item.Channel = this.ddlChannel.SelectedItem.Text.Trim();
                item.CountryOfSale = ddlCountryOfSale.SelectedItem.Text.Trim();
                item.SoldOutsideUSA = ddlOutsideUSA.SelectedItem.Text.Trim();

                // Item Hierarchy tab
                item.ProductHierarchyLevel2 = this.ddlProductHierarchyLevel2.SelectedItem.Text;
                item.ProductFormDescription = this.txtProductFormDescription.Text.ToUpper();
                item.MaterialGroup4ProductForm = this.ddlMaterialGroup4.SelectedItem.Text;
                item.MaterialGroup5PackType = this.ddlMaterialGroup5.SelectedItem.Text;
                item.TotalQuantityUnitsInDisplay = txtFGTotalQuantityUnitsInDisplay.Text.Trim();
                item.NovelyProject = drpNovelyProject.SelectedItem.Text.Trim();
                item.ProfitCenter = txtProfitCenter.Text;
                // Item UPCs tab

                item.UnitUPC = txtUnitUPC.Text.Trim();
                item.RequireNewUnitUPC = ddlNeedNewUnitUPC.SelectedItem.Text.Trim();
                if (string.Equals(item.RequireNewUnitUPC, "Yes") && string.IsNullOrEmpty(item.UnitUPC))
                {
                    item.UnitUPC = GlobalConstants.CONST_NeedsNew;
                }
                item.DisplayBoxUPC = txtDisplayUPCBox.Text.Trim();
                item.RequireNewDisplayBoxUPC = ddlNeedNewDisplayBoxUPC.SelectedItem.Text.Trim();
                if (string.Equals(item.RequireNewDisplayBoxUPC, "Yes") && string.IsNullOrEmpty(item.DisplayBoxUPC))
                {
                    item.DisplayBoxUPC = GlobalConstants.CONST_NeedsNew;
                }
                item.SAPBaseUOM = ddlSAPBUOM.SelectedItem.Text.Trim();
                item.CaseUCC = txtCaseUCC.Text.Trim();
                item.RequireNewCaseUCC = ddlNeedNewCaseUCC.SelectedItem.Text.Trim();
                if (string.Equals(item.RequireNewCaseUCC, "Yes") && string.IsNullOrEmpty(item.CaseUCC))
                {
                    item.CaseUCC = GlobalConstants.CONST_NeedsNew;
                }
                item.PalletUCC = txtPalletUCC.Text;
                item.RequireNewPalletUCC = ddlNeedNewPalletUCC.SelectedItem.Text.Trim();
                if (string.Equals(item.RequireNewPalletUCC, "Yes") && string.IsNullOrEmpty(item.PalletUCC))
                {
                    item.PalletUCC = GlobalConstants.CONST_NeedsNew;
                }
                item.FlowthroughDets = txtFlowthroughDets.Text;
                // Additonal Item Details tab
                item.CaseType = ddlCaseType.SelectedItem.Text.Trim();
                item.MarketClaimsLabelingRequirements = txtClaimsLabelingRequirements.Text.Trim();

                item.RetailSellingUnitsBaseUOM = Utilities.GetNumber(txtRetailSellingUnitsPerBaseUOM.Text.Trim());
                item.NumberofTraysPerBaseUOM = Utilities.GetNumber(txtNumberofTraysPerBaseUOM.Text.Trim());
                item.RetailUnitWieghtOz = Utilities.GetDecimal(txtRetailUnitWeight.Text.Trim());
                item.BaseUOMNetWeightLbs = Utilities.GetDecimal(txtBaseUofMNetWeight.Text.Trim());
                item.FilmSubstrate = ddlFilmSubstrate.SelectedItem.Text.Trim();
                item.PegHoleNeeded = ddlPegHoleNeeded.SelectedItem.Text.Trim();
                item.InvolvesCarton = ddlInvolvesCarton.SelectedItem.Text.Trim();
                item.UnitsInsideCarton = txtUnitsInsideCarton.Text.Trim();
                item.IndividualPouchWeight = Utilities.GetDecimal(txtIndividualPouchWeight.Text.Trim());
                item.ReasonForChange = txtChangeReason.Text.Trim();

            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Unexpected Error Occurred: ConstructFormData", this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "ConstructFormData");
                return null;
            }

            return item;
        }
        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            try
            {
                item.CompassListItemId = iItemId;

                // Set submitting user information
                item.ModifiedDate = DateTime.Now.ToString();
                item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Unexpected Error Occurred: ConstructApprovalData", this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "ConstructApprovalData");
                return null;
            }

            return item;
        }
        private MarketingClaimsItem ConstructMarketingData()
        {
            var item = new MarketingClaimsItem();

            try
            {
                item.CompassListItemId = iItemId;
                item.Title = ProjectNumber;
                item.SellableUnit = drpSellableUnit.SelectedItem.Text.Trim();
                item.NewNLEAFormat = drpNewNLEAFormat.SelectedItem.Text.Trim();
                item.BioEngLabelingAcceptable = drpBioEngineeringLabelingAcceptable.SelectedItem.Text.Trim();
                item.MadeInUSAClaim = drpMadeInUSA.SelectedItem.Text.Trim();
                item.MadeInUSAClaimDets = drpMadeInUSAPct.SelectedItem.Text.Trim();
                item.Organic = drpOrganic.SelectedItem.Text.Trim();
                item.GMOClaim = drpGMOClaim.SelectedItem.Text.Trim();
                item.GlutenFree = drpGlutenFree.SelectedItem.Text.Trim();
                item.FatFree = drpFatFree.SelectedItem.Text.Trim();
                item.Kosher = drpKosher.SelectedItem.Text.Trim();
                item.NaturalColors = drpNaturalColors.SelectedItem.Text.Trim();
                item.NaturalFlavors = drpNaturalFlavors.SelectedItem.Text.Trim();
                item.PreservativeFree = drpPreservativeFree.SelectedItem.Text.Trim();
                item.LactoseFree = drpLactoseFree.SelectedItem.Text.Trim();
                item.JuiceConcentrate = txtJuiceConcentrate.Text;
                item.LowSodium = drpLowSodium.SelectedItem.Text.Trim();
                item.GoodSource = hdnSelectedGoodSource.Value;
                //item.VitaminA = drpVitaminA.SelectedItem.Text.Trim();
                item.VitaminAPct = drpVitaminAPct.SelectedItem.Text.Trim();
                //item.VitaminB1 = drpVitaminB1.SelectedItem.Text.Trim();
                //item.VitaminB12 = drpVitaminB12.SelectedItem.Text.Trim();
                item.VitaminB12Pct = drpVitaminB12Pct.SelectedItem.Text.Trim();
                item.VitaminB1Pct = drpVitaminB1Pct.SelectedItem.Text.Trim();
                //item.VitaminB2 = drpVitaminB2.SelectedItem.Text.Trim();
                item.VitaminB2Pct = drpVitaminB2Pct.SelectedItem.Text.Trim();
                //item.VitaminB3 = drpVitaminB3.SelectedItem.Text.Trim();
                item.VitaminB3Pct = drpVitaminB3Pct.SelectedItem.Text.Trim();
                //item.VitaminB5 = drpVitaminB5.SelectedItem.Text.Trim();
                item.VitaminB5Pct = drpVitaminB5Pct.SelectedItem.Text.Trim();
                //item.VitaminB6 = drpVitaminB6.SelectedItem.Text.Trim();
                item.VitaminB6Pct = drpVitaminB6Pct.SelectedItem.Text.Trim();
                //item.VitaminC = drpVitaminC.SelectedItem.Text.Trim();
                item.VitaminCPct = drpVitaminCPct.SelectedItem.Text.Trim();
                //item.VitaminD = drpVitaminD.SelectedItem.Text.Trim();
                item.VitaminDPct = drpVitaminDPct.SelectedItem.Text.Trim();
                //item.VitaminE = drpVitaminE.SelectedItem.Text.Trim();
                item.VitaminEPct = drpVitaminEPct.SelectedItem.Text.Trim();
                //item.Potassium = drpPotassium.SelectedItem.Text.Trim();
                item.PotassiumPct = drpPotassiumPct.SelectedItem.Text.Trim();
                //item.Iron = drpIron.SelectedItem.Text.Trim();
                item.IronPct = drpIronPct.SelectedItem.Text.Trim();
                //item.Calcium = drpCalcium.SelectedItem.Text.Trim();
                item.CalciumPct = drpCalciumPct.SelectedItem.Text.Trim();
                item.ClaimsDesired = drpDesiredClaims.SelectedItem.Text.Trim();
                item.MaterialClaimsCompNumber = txtMaterialClaimsCompNumber.Text;
                item.MaterialClaimsCompDesc = txtMaterialClaimsCompDesc.Text;
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Unexpected Error Occurred: ConstructMarketingData", this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "ConstructMarketingData");
                return null;
            }

            return item;
        }
        #endregion

        #region Attachment Methods
        protected void rpFormulationAttachments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string fileName = e.CommandArgument.ToString();

                utilityService.DeleteAttachment(fileName);
                LoadFormulationAttachments();
                hdnSteps.Value = "10";
            }
        }
        protected void lnkFormulationFileDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                LoadFormulationAttachments();

            }
        }
        protected void lnkGraphicsFileDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                LoadGraphicsAttachments();
            }
        }
        protected void btnUploadAllAttachments_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
        }
        #endregion

        #region Dropdown methods
        protected void ddlProductHierarchyLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadProductHierarchyLevel2(this.ddlProductHierarchyLevel1.SelectedItem.Text);

            if (sender != null)
                hdnSteps.Value = "6";

        }
        private void ReloadProductHierarchyLevel2(string productHierarhcyLevel1)
        {
            // Get the Product Hierarchy Level 1 Value
            string level2 = Utilities.GetLookupValue(GlobalConstants.LIST_ProductHierarchyLevel1Lookup, productHierarhcyLevel1, webUrl);

            if ((!string.IsNullOrEmpty(level2)) && (!string.Equals(level2, "Select...")))
            {
                Utilities.BindDropDownItemsByValue(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, level2, webUrl);
                this.ddlProductHierarchyLevel2.SelectedIndex = -1;
            }
            else
            {
                this.ddlProductHierarchyLevel2.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                this.ddlProductHierarchyLevel2.Items.Add(li);
            }
            this.ddlBrand_Material.Items.Clear();
            ListItem li2 = new ListItem();
            li2.Text = "Select...";
            li2.Value = "-1";
            this.ddlBrand_Material.Items.Add(li2);
            txtProfitCenter.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tmp", "<script type='text/javascript'>SAPNomenclature();ClearDropdwonStyles();</script>", false);
        }
        protected void ddlProductHierarchyLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadBrand(this.ddlProductHierarchyLevel2.SelectedItem.Text);

            if (sender != null)
                hdnSteps.Value = "6";

        }
        private void ReloadBrand(string productHierarhcyLevel2)
        {
            if ((!string.IsNullOrEmpty(productHierarhcyLevel2)) && (!string.Equals(productHierarhcyLevel2, "Select...")))
            {
                Utilities.BindDropDownItemsByValueAndColumn(ddlBrand_Material, GlobalConstants.LIST_MaterialGroup1Lookup, "ParentPHL2", productHierarhcyLevel2, webUrl);
                this.ddlBrand_Material.SelectedIndex = -1;
            }
            else
            {
                this.ddlBrand_Material.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                this.ddlBrand_Material.Items.Add(li);

            }
            txtProfitCenter.Text = "";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tmp", "<script type='text/javascript'>SAPNomenclature();ClearDropdwonStyles();</script>", false);
        }
        protected void ddlBrand_Material_SelectedIndexChanged(object sender, EventArgs e)
        {
            string profitCenter = Utilities.GetLookupDetailsByValueAndColumn("ProfitCenter", GlobalConstants.LIST_MaterialGroup1Lookup, "Title", ddlBrand_Material.SelectedItem.Text, "ParentPHL2", ddlProductHierarchyLevel2.SelectedItem.Text, webUrl);
            txtProfitCenter.Text = profitCenter;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "tmp", "<script type='text/javascript'>SAPNomenclature();ClearDropdwonStyles();</script>", false);
        }
        #endregion

        #region Shipper Repeater methods
        protected void rptShipper_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ShipperFinishedGoodItem shipItem;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                shipItem = (ShipperFinishedGoodItem)e.Item.DataItem;
                DropDownList ddlFGPackUnit = ((DropDownList)e.Item.FindControl("ddlFGPackUnit"));
                Utilities.BindDropDownItems(ddlFGPackUnit, GlobalConstants.LIST_PackUnitLookup, webUrl);
                Utilities.SetDropDownValue(shipItem.FGPackUnit, ddlFGPackUnit, Page);
            }
        }
        private void LoadShipperFinishedGoodItems()
        {
            var data = GetShipperFGItem(false);
            if (data.Count > 0)
            {
                if (!string.IsNullOrEmpty(this.IPFMode))
                {
                    // if this is a Copy or Change request, set all the Shipper Items Ids to a negative number.
                    // Since we are copying an existing item, we want to clear all the ids so we don't try to delete non-existing items.
                    foreach (ShipperFinishedGoodItem pi in data)
                    {
                        pi.ItemId = Utilities.GetUniqueId();
                    }
                }

                rptShipper.DataSource = data;
                rptShipper.DataBind();

                rpShipperSummary.DataSource = data;
                rpShipperSummary.DataBind();
            }
        }
        private List<ShipperFinishedGoodItem> GetShipperFGItem(bool isNew)
        {
            List<ShipperFinishedGoodItem> dtFGItem = new List<ShipperFinishedGoodItem>();

            if (ViewState["FGShipperItemTable"] == null)
            {
                dtFGItem = shipperFinishedGoodService.GetShipperFinishedGoodItems(iItemId);
            }
            else
            {
                dtFGItem = (List<ShipperFinishedGoodItem>)ViewState["FGShipperItemTable"];
            }

            if (isNew)
            {
                dtFGItem.Clear();
                foreach (RepeaterItem ri in rptShipper.Items)
                {
                    if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                    {
                        HtmlInputControl fgItemDisplay = ri.FindControl("txtFGItemDisplay") as HtmlInputControl;
                        HtmlInputControl fgItemDescription = ri.FindControl("txtFGItemDescription") as HtmlInputControl;
                        HtmlInputControl txtFGnumberUnits = ri.FindControl("txtFGnumberUnits") as HtmlInputControl;
                        HtmlInputControl txtFGouncesPerUnit = ri.FindControl("txtFGouncesPerUnit") as HtmlInputControl;
                        DropDownList ddlFGPackUnit = ri.FindControl("ddlFGPackUnit") as DropDownList;
                        HiddenField hdnItemId = ri.FindControl("hidShipperId") as HiddenField;

                        ShipperFinishedGoodItem row = new ShipperFinishedGoodItem();
                        row.CompassListItemId = iItemId;
                        row.FGItemNumber = fgItemDisplay.Value;
                        row.FGItemDescription = fgItemDescription.Value;
                        row.FGItemNumberUnits = Convert.ToInt32(txtFGnumberUnits.Value);
                        row.FGItemOuncesPerUnit = Convert.ToDouble(txtFGouncesPerUnit.Value);
                        row.FGPackUnit = ddlFGPackUnit.SelectedItem.Text;
                        row.ItemId = Convert.ToInt32(hdnItemId.Value);

                        dtFGItem.Add(row);
                    }
                }

                ShipperFinishedGoodItem dtRow = new ShipperFinishedGoodItem();
                dtRow.ItemId = Utilities.GetUniqueId();
                dtRow.CompassListItemId = iItemId;
                dtRow.FGItemNumber = "";
                dtRow.FGItemDescription = "";
                dtRow.FGItemNumberUnits = 0;
                dtRow.FGItemOuncesPerUnit = 0;
                dtRow.FGPackUnit = "";
                dtFGItem.Add(dtRow);
            }

            if (dtFGItem.Count == 0)
            {
                ShipperFinishedGoodItem dtRow = new ShipperFinishedGoodItem();
                dtRow.ItemId = Utilities.GetUniqueId();
                dtRow.CompassListItemId = iItemId;
                dtRow.FGItemNumber = "";
                dtRow.FGItemDescription = "";
                dtRow.FGItemNumberUnits = 0;
                dtRow.FGItemOuncesPerUnit = 0;
                dtRow.FGPackUnit = "";
                dtFGItem.Add(dtRow);
            }

            ViewState["FGShipperItemTable"] = dtFGItem;

            return dtFGItem;
        }
        protected void btnAddShipperFinishedGood_Click(object sender, EventArgs e)
        {
            List<ShipperFinishedGoodItem> dtFGItem = new List<ShipperFinishedGoodItem>();

            dtFGItem = GetShipperFGItem(true);
            if (ViewState["FGShipperItemTable"] != null)
            {
                dtFGItem = (List<ShipperFinishedGoodItem>)ViewState["FGShipperItemTable"];

                rptShipper.DataSource = dtFGItem;
                rptShipper.DataBind();
                rptShipper.Visible = true;
            }
            else
            {
                rptShipper.DataSource = null;
                rptShipper.DataBind();
                rptShipper.Visible = false;
            }
            hdnSteps.Value = "6";
        }
        protected void rptShipper_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());

                List<ShipperFinishedGoodItem> lst = GetShipperFGItem(false);
                var l = lst.FindIndex(r => r.ItemId == id);
                lst.RemoveAt(l);

                if (id > 0)
                    shipperFinishedGoodService.DeleteShipperFinishedGoodItem(id, webUrl);

                rptShipper.DataSource = lst;
                rptShipper.DataBind();
                ViewState["FGShipperItemTable"] = lst;

                if (lst.Count == 0)
                    LoadShipperFinishedGoodItems();
            }
            if (e.CommandName.ToLower() == "find")
            {
                string FGItemNuminDisplay = string.Empty;
                RepeaterItem clickedItem = null;
                foreach (RepeaterItem item in rptShipper.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        FGItemNuminDisplay = ((HtmlInputControl)item.FindControl("txtFGItemDisplay")).Value;
                        clickedItem = item;
                        if (string.IsNullOrEmpty(FGItemNuminDisplay))
                        {
                            // Set Error   
                            ErrorSummary.AddError("Please enter a valid 'Like/Old' Component #!", this.Page);
                            return;
                        }
                    }
                }
                SetFGLookupValues(FGItemNuminDisplay, clickedItem);
            }
        }
        private void SetFGLookupValues(string FGNumber, RepeaterItem item)
        {
            SAPMaterialMasterListItem mmItem = sapMMService.GetSAPMaterialMaster(FGNumber);
            if ((mmItem == null) || (mmItem.SAPItemNumber == null))
            {
                ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Search Failed:</p></strong><br/>";
                ErrorSummary.AddError("Finished Good Item # could not be found!  Please try again.", this.Page);
                return;
            }
            else
            {
                if (item != null)
                {
                    ((HtmlInputControl)item.FindControl("txtFGItemDescription")).Value = mmItem.SAPDescription;
                }
            }
        }
        #endregion

        #region Mixes Repeater methods
        private List<MixesItem> GetMixesItem(bool isNew)
        {
            List<MixesItem> dtMixesItem = new List<MixesItem>();

            if (ViewState["MixesItemTable"] == null)
            {
                dtMixesItem = mixesService.GetMixesItems(iItemId);
            }
            else
            {
                dtMixesItem = (List<MixesItem>)ViewState["MixesItemTable"];
            }

            if (isNew)
            {
                dtMixesItem.Clear();
                // Get all existing rows and gather any entered changes
                foreach (RepeaterItem ri in rpMixes.Items)
                {
                    if (ri.ItemType == ListItemType.Item || ri.ItemType == ListItemType.AlternatingItem)
                    {
                        HtmlInputControl mixItemNumber = ri.FindControl("txtMixItemNumber") as HtmlInputControl;
                        HtmlInputControl mixItemDescription = ri.FindControl("txtMixItemDescription") as HtmlInputControl;
                        HtmlInputControl mixNumberOfPieces = ri.FindControl("txtMixNumberOfPieces") as HtmlInputControl;

                        HtmlInputControl ozPerPiece = ri.FindControl("txtOzPerPiece") as HtmlInputControl;
                        HiddenField hdnItemId = ri.FindControl("hidMixesId") as HiddenField;

                        MixesItem row = new MixesItem();
                        row.CompassListItemId = iItemId;
                        row.ItemNumber = mixItemNumber.Value;
                        row.ItemDescription = mixItemDescription.Value;
                        row.NumberOfPieces = Convert.ToDouble(mixNumberOfPieces.Value);
                        row.OuncesPerPiece = Convert.ToDouble(ozPerPiece.Value);
                        row.ItemId = Convert.ToInt32(hdnItemId.Value);

                        dtMixesItem.Add(row);
                    }
                }

                // Add New Row
                MixesItem dtRow = new MixesItem();
                dtRow.ItemId = Utilities.GetUniqueId();
                dtRow.CompassListItemId = iItemId;
                dtRow.ItemNumber = string.Empty;
                dtRow.ItemDescription = string.Empty;
                dtRow.NumberOfPieces = 0.0;
                dtRow.OuncesPerPiece = 0.0;
                dtMixesItem.Add(dtRow);
            }

            if (dtMixesItem.Count == 0)
            {
                MixesItem dtRow = new MixesItem();
                dtRow.ItemId = Utilities.GetUniqueId();
                dtRow.CompassListItemId = iItemId;
                dtRow.ItemNumber = string.Empty;
                dtRow.ItemDescription = string.Empty;
                dtRow.NumberOfPieces = 0.0;
                dtRow.OuncesPerPiece = 0.0;
                dtMixesItem.Add(dtRow);
            }

            ViewState["MixesItemTable"] = dtMixesItem;

            return dtMixesItem;
        }
        private void LoadMixesItems()
        {
            var data = GetMixesItem(false);
            if (data.Count > 0)
            {
                if (!string.IsNullOrEmpty(this.IPFMode))
                {
                    // if this is a Copy or Change request, set all the Mixes Items Ids to a negative number.
                    // Since we are copying an existing item, we want to clear all the ids so we don't try to delete non-existing items.
                    foreach (MixesItem pi in data)
                    {
                        pi.ItemId = Utilities.GetUniqueId();
                    }
                }



                rpMixes.DataSource = data;
                rpMixes.DataBind();

                rpMixesSummary.DataSource = data;
                rpMixesSummary.DataBind();
            }
        }
        protected void rpMixes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "delete")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());

                List<MixesItem> lst = GetMixesItem(false);
                var l = lst.FindIndex(r => r.ItemId == id);
                lst.RemoveAt(l);

                if (id > 0)
                    mixesService.DeleteMixesItem(id, webUrl);

                rpMixes.DataSource = lst;
                rpMixes.DataBind();
                ViewState["MixesItemTable"] = lst;

                if (lst.Count == 0)
                    LoadMixesItems();
            }
        }
        protected void rpMixes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HtmlInputControl txtMixNumberOfPieces, txtOzPerPiece, txtOzPerSellingUnit, txtGramsPerSellingUnit, txtLbsFGBOM, txtQtyMix;
            double OzPerSellingUnit, QtyFGBOM;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                txtMixNumberOfPieces = (HtmlInputControl)e.Item.FindControl("txtMixNumberOfPieces");
                txtOzPerPiece = (HtmlInputControl)e.Item.FindControl("txtOzPerPiece");
                txtOzPerSellingUnit = (HtmlInputControl)e.Item.FindControl("txtOzPerSellingUnit");
                txtGramsPerSellingUnit = (HtmlInputControl)e.Item.FindControl("txtGramsPerSellingUnit");
                txtLbsFGBOM = (HtmlInputControl)e.Item.FindControl("txtLbsFGBOM");
                txtQtyMix = (HtmlInputControl)e.Item.FindControl("txtQtyMix");
                txtOzPerSellingUnit.Value = "0";
                txtGramsPerSellingUnit.Value = "0";
                txtLbsFGBOM.Value = "0.00";
                txtQtyMix.Value = "0";
                if (!String.IsNullOrEmpty(txtMixNumberOfPieces.Value) && !String.IsNullOrEmpty(txtOzPerPiece.Value))
                {
                    if (Convert.ToDecimal(txtMixNumberOfPieces.Value) > 0 && Convert.ToDecimal(txtOzPerPiece.Value) > 0)
                    {
                        OzPerSellingUnit = Convert.ToDouble(txtMixNumberOfPieces.Value) * Convert.ToDouble(txtOzPerPiece.Value);
                        txtOzPerSellingUnit.Value = OzPerSellingUnit.ToString();
                        txtGramsPerSellingUnit.Value = (OzPerSellingUnit * 28.3495).ToString("0.00");
                        if (!String.IsNullOrEmpty(txtRetailSellingUnitsPerBaseUOM.Text))
                            if (Convert.ToDecimal(txtRetailSellingUnitsPerBaseUOM.Text) > 0)
                            {
                                QtyFGBOM = Convert.ToDouble(txtMixNumberOfPieces.Value) * Convert.ToDouble(txtRetailSellingUnitsPerBaseUOM.Text);
                                txtLbsFGBOM.Value = (QtyFGBOM * Convert.ToDouble(txtOzPerPiece.Value) / 16.0).ToString("0.00");
                                txtQtyMix.Value = QtyFGBOM.ToString();
                            }
                    }
                }
            }
        }
        protected void btnAddMixItem_Click(object sender, EventArgs e)
        {
            List<MixesItem> dtMixesItem = new List<MixesItem>();

            dtMixesItem = GetMixesItem(true);
            rpMixes.DataSource = dtMixesItem;
            rpMixes.DataBind();
            rpMixes.Visible = true;

            hdnSteps.Value = "6";
        }
        #endregion

        #region SAP Lookup Button Methods
        protected void btnLookupSAPItemNumber_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
            string sapNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtSAPItemNumber.Text))
            {
                sapNumber = txtSAPItemNumber.Text.Trim();
            }
            else
            {
                // Set Error
                ErrorSummary.AddError("Please enter a valid Finished Good Item #!", this.Page);
                return;
            }
            bool CopyFeatureUsed = false;
            int IPFCopiedFromCompassListItemId = 0;
            try
            {
                IPFCopiedFromCompassListItemId = Convert.ToInt32(hdnIPFCopiedFromCompassListItemId.Value);
            }
            catch (Exception ex)
            {
            }

            if (IPFCopiedFromCompassListItemId != 0)
            {
                CopyFeatureUsed = true;
            }

            SetLookupValues(sapNumber, txtSAPItemNumber.ID, CopyFeatureUsed);
            hdnSteps.Value = "2";
        }
        protected void btnLookupLikeSAPItemNumber_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
            string sapNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtLikeFGItemNumber.Text))
            {
                sapNumber = txtLikeFGItemNumber.Text.Trim();
            }
            else
            {
                // Set Error   
                ErrorSummary.AddError("Please enter a valid 'Like' Finished Good Item #!", this.Page);
                return;
            }

            SetLookupValues(sapNumber, txtLikeFGItemNumber.ID, false);
            hdnSteps.Value = "2";
        }
        protected void btnLookupOldSAPItemNumber_Click(object sender, EventArgs e)
        {
            string sapNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtOldFGItemNumber.Text))
                sapNumber = txtOldFGItemNumber.Text.Trim();
            else
            {
                ErrorSummary.AddError("Please enter a valid Old Finished Good Item #!", this.Page);
                return;
            }

            SetLookupValues(sapNumber, txtOldFGItemNumber.ID, false);
            hdnSteps.Value = "2";
        }
        private void SetLookupValues(string sapNumber, string controlId, bool CopyFeatureUsed)
        {
            List<SAPBOMListItem> bomList = new List<SAPBOMListItem>();
            if (!CopyFeatureUsed)
            {
                bomList = sapBOMService.GetSAPBOMItemsIPF(sapNumber);
            }

            double number = 0;
            SAPMaterialMasterListItem mmItem = sapMMService.GetSAPMaterialMaster(sapNumber);

            if ((mmItem == null) || (mmItem.SAPItemNumber == null))
            {
                ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Search Failed:</p></strong><br/>";
                ErrorSummary.AddError("Finished Good Item # could not be found!  Please try again.", this.Page);
                return;
            }
            switch (controlId)
            {
                case "txtSAPItemNumber":
                    if (ddlTBDIndicator.SelectedItem.Value == "Y")
                    {
                        lblSAPItemDescription.Text = mmItem.SAPDescription;
                    }
                    else
                    {
                        txtSAPItemDescription.Text = mmItem.SAPDescription;
                    }
                    txtCaseUCC.Text = mmItem.CaseUCC;
                    txtDisplayUPCBox.Text = mmItem.DisplayBoxUPC;
                    txtPalletUCC.Text = mmItem.PalletUCC;
                    txtUnitUPC.Text = mmItem.UnitUPC;
                    break;
                case "txtLikeFGItemNumber":
                    txtLikeItemDescription.Text = mmItem.SAPDescription;
                    break;
                case "txtOldFGItemNumber":
                    txtOldItemDescription.Text = mmItem.SAPDescription;
                    break;
            }
            double.TryParse(txtTruckLoadSellingPrice.Text, out number);
            if (txtTruckLoadSellingPrice.Text.Equals("") || number == 0)
            {
                txtTruckLoadSellingPrice.Text = Utilities.FormatDecimal(Utilities.GetDecimal(mmItem.TruckLoadPricePerSellingUnit), 2);
                lblTruckLoadSellingPrice.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            if (txtLast12MonthSales.Text.Equals(""))
            {
                txtLast12MonthSales.Text = Utilities.FormatDecimal(Utilities.GetDecimal(mmItem.Last12MonthSales), 2);
                lblLast12MonthSales.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            if (ddlProductHierarchyLevel1.SelectedValue.Equals("-1"))
            {
                Utilities.SetDropDownValue(mmItem.ProductHierarchyLevel1, this.ddlProductHierarchyLevel1, this.Page);
                ReloadProductHierarchyLevel2(this.ddlProductHierarchyLevel1.SelectedItem.Text);
                lblProductHierarchyLevel1.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            //if (ddlLineOfBusiness.SelectedValue.Equals("-1"))
            //{
            //    Utilities.SetDropDownValue(mmItem.ProductHierarchyLevel1, this.ddlLineOfBusiness, this.Page);
            //    lblLineOfBusiness.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            //}
            if (ddlProductHierarchyLevel2.SelectedValue.Equals("-1"))
            {
                Utilities.SetDropDownValue(mmItem.ProductHierarchyLevel2.ToUpper(), this.ddlProductHierarchyLevel2, this.Page);
                lblProductHierarchyLevel2.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            //if (ddlBrand.SelectedValue.Equals("-1"))
            //{
            //    Utilities.SetDropDownValue(mmItem.MaterialGroup1Brand.ToUpper(), this.ddlBrand, this.Page);
            //    lblBrand.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            //}
            if (ddlBrand_Material.SelectedValue.Equals("-1"))
            {
                Utilities.SetDropDownValue(mmItem.MaterialGroup1Brand.ToUpper(), this.ddlBrand_Material, this.Page);
                lblBrand_Material.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            if (ddlMaterialGroup4.SelectedValue.Equals("-1"))
            {
                Utilities.SetDropDownValue(mmItem.MaterialGroup4ProductForm.ToUpper(), this.ddlMaterialGroup4, this.Page);
                lblMaterialGroup4.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            if (ddlMaterialGroup5.SelectedValue.Equals("-1"))
            {
                Utilities.SetDropDownValue(mmItem.MaterialGroup5PackType.ToUpper(), this.ddlMaterialGroup5, this.Page);
                lblMaterialGroup5.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            if (txtRetailSellingUnitsPerBaseUOM.Text.Equals(""))
            {
                txtRetailSellingUnitsPerBaseUOM.Text = Utilities.FormatDecimal(Utilities.GetDecimal(mmItem.RetailSellingUnitsBaseUOM), 0);
                lblRetailSellingUnitsPerBaseUOM.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            if (txtRetailUnitWeight.Text.Equals(""))
            {
                txtRetailUnitWeight.Text = Utilities.FormatDecimal(Utilities.GetDecimal(mmItem.RetailUnitWieghtOz), 2);
                lblRetailUnitWeight.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }
            if (txtBaseUofMNetWeight.Text.Equals(""))
            {
                double retailSellingUnitsPerBaseUOM = Utilities.GetDecimal(mmItem.RetailSellingUnitsBaseUOM);
                double retailUnitWeight = Utilities.GetDecimal(mmItem.RetailUnitWieghtOz);

                if ((retailSellingUnitsPerBaseUOM != 0) && (retailUnitWeight != 0))
                {
                    double num = retailSellingUnitsPerBaseUOM * (retailUnitWeight / 16);
                    txtBaseUofMNetWeight.Text = num.ToString("N2");
                }
                else
                {
                    txtBaseUofMNetWeight.Text = "0.00";
                }
                lblBaseUofMNetWeight.Text = "This field was autopopulated with a 'Like' item. Please check for accuracy.";
            }

            if (!CopyFeatureUsed)
            {
                SAPPIs = BindBOMFromSAPItemNumber(bomList, controlId);
            }
            //hdnUCLoaded.Value = "false";
            LoadBOMItems();

        }

        private List<PackagingItem> BindBOMFromSAPItemNumber(List<SAPBOMListItem> bomList, string controlId)
        {
            string matDesc, matNumb;
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();

            foreach (SAPBOMListItem sapbom in bomList)
            {
                PackagingItem obj = new PackagingItem();

                switch (controlId)
                {
                    case "txtSAPItemNumber":
                        matDesc = sapbom.MaterialDescription;
                        matNumb = sapbom.MaterialNumber;
                        obj.MaterialDescription = matDesc;
                        obj.MaterialNumber = matNumb;
                        break;
                    case "txtLikeFGItemNumber":
                        matDesc = sapbom.MaterialDescription;
                        matNumb = sapbom.MaterialNumber;
                        if (matDesc == "N/A")
                            matDesc = "Not Applicable";
                        if (matNumb == "N/A")
                            matNumb = "Not Applicable";
                        obj.CurrentLikeItemDescription = matDesc;
                        obj.CurrentLikeItem = matNumb;
                        break;
                    case "txtOldFGItemNumber":
                        matDesc = sapbom.MaterialDescription;
                        matNumb = sapbom.MaterialNumber;
                        if (matDesc == "N/A")
                            matDesc = "Not Applicable";
                        if (matNumb == "N/A")
                            matNumb = "Not Applicable";
                        obj.CurrentOldItemDescription = matDesc;
                        obj.CurrentOldItem = matNumb;
                        break;
                }
                obj.PackQuantity = sapbom.PackQuantity;
                obj.PackUnit = sapbom.PackUnit;
                obj.CompassListItemId = iItemId.ToString();
                obj.CurrentLikeItemReason = "";
                obj.Id = obj.IdTemp = Utilities.GetUniqueId();
                dtPackingItem.Add(obj);

                if (sapbom.MaterialType == GlobalConstants.SAPBOMLIST_TRANSFERSEMI || sapbom.MaterialType == GlobalConstants.SAPBOMLIST_PURCHASEDSEMI || sapbom.MaterialType == GlobalConstants.SAPBOMLIST_CANDYSEMI)
                {
                    obj.PackagingComponent =
                        (sapbom.MaterialType == GlobalConstants.SAPBOMLIST_TRANSFERSEMI) ? GlobalConstants.COMPONENTTYPE_TransferSemi :
                        (sapbom.MaterialType == GlobalConstants.SAPBOMLIST_PURCHASEDSEMI) ? GlobalConstants.COMPONENTTYPE_PurchasedSemi :
                        (sapbom.MaterialType == GlobalConstants.SAPBOMLIST_CANDYSEMI) ? GlobalConstants.COMPONENTTYPE_CandySemi : string.Empty;

                    dtPackingItem.AddRange(GetChildItemsForSemis(sapbom.MaterialNumber, obj.Id, controlId));
                }
            }

            return dtPackingItem;
        }

        private List<PackagingItem> GetChildItemsForSemis(string sapNumber, int parentId, string controlId)
        {
            var bomList = sapBOMService.GetSAPBOMItemsIPF(sapNumber);
            var packLists = new List<PackagingItem>();
            string matDesc, matNumb;

            foreach (SAPBOMListItem sapbom in bomList)
            {
                PackagingItem obj = new PackagingItem();
                obj.ParentID = parentId;

                switch (controlId)
                {
                    case "txtSAPItemNumber":
                        matDesc = sapbom.MaterialDescription;
                        matNumb = sapbom.MaterialNumber;
                        obj.MaterialDescription = matDesc;
                        obj.MaterialNumber = matNumb;
                        break;
                    case "txtLikeFGItemNumber":
                        matDesc = sapbom.MaterialDescription;
                        matNumb = sapbom.MaterialNumber;
                        if (matDesc == "N/A")
                            matDesc = "Not Applicable";
                        if (matNumb == "N/A")
                            matNumb = "Not Applicable";
                        obj.CurrentLikeItemDescription = matDesc;
                        obj.CurrentLikeItem = matNumb;
                        break;
                    case "txtOldFGItemNumber":
                        matDesc = sapbom.MaterialDescription;
                        matNumb = sapbom.MaterialNumber;
                        if (matDesc == "N/A")
                            matDesc = "Not Applicable";
                        if (matNumb == "N/A")
                            matNumb = "Not Applicable";
                        obj.CurrentOldItemDescription = matDesc;
                        obj.CurrentOldItem = matNumb;
                        break;
                }
                obj.PackQuantity = sapbom.PackQuantity;
                obj.PackUnit = sapbom.PackUnit;
                obj.CompassListItemId = iItemId.ToString();
                obj.CurrentLikeItemReason = "";
                obj.Id = obj.IdTemp = Utilities.GetUniqueId();
                packLists.Add(obj);
            }

            return packLists;
        }
        #endregion

        #region Button Methods
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e);
            LoadFormulationAttachments();
            LoadGraphicsAttachments();
        }
        protected void btnLoadSummary_Click(object sender, EventArgs e)
        {
            hdnSteps.Value = "12";
            btnSave_Click(sender, e);
            Summary();

            lblLoadSummaryCompleted.Text = "Load Summary Completed: " + DateTime.Now;
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            // Validate Information
            bool bValid = true;
            string value = string.Empty;

            value = ddlProjectType.SelectedItem.Text.Trim();
            if ((string.IsNullOrEmpty(value)) || (string.Equals(value, "Select...")))
            {
                ErrorSummary.AddError("Project Type is Required.<a href='javascript:setFocus(&quot;ddlProjectType&quot;)'>  [Update]</a>", this.Page);
                bValid = false;
            }
            if (string.IsNullOrEmpty(txtFirstShipDate.Text))
            {
                ErrorSummary.AddError("First Ship Date is Required.<a href='javascript:setFocus(&quot;txtFirstShipDate&quot;)'>  [Update]</a>", this.Page);
                bValid = false;
            }

            if (string.IsNullOrEmpty(txtChangeNotes.Text))
            {
                ErrorSummary.AddError(lblChangeNotes.InnerText + " is Required.<a href='javascript:setFocus(&quot;txtChangeNotes&quot;)'>  [Update]</a>", this.Page);
                bValid = false;
            }

            if (!bValid)
                return;

            btnNext.Enabled = false;


            ItemProposalItem item = ConstructFormData(false);
            if (!SaveItemProposal(item, false))
                return;

            // Redirect to IPF page after successful Save Insert                    
            Page.Response.Redirect(Utilities.RedirectPageValueFirstSave(GlobalConstants.PAGE_ItemProposal, item.ProjectNumber), false);
        }
        private Boolean SaveItemProposal(ItemProposalItem item, bool bSubmitted)
        {
            ApprovalItem approvalItem;
            MarketingClaimsItem marketingItem;
            Boolean bSuccess = true;
            int sourceId;

            try
            {
                // Set the approval data
                approvalItem = ConstructApprovalData();
                marketingItem = ConstructMarketingData();
                if ((item.CompassListItemId > 0) && (string.IsNullOrEmpty(this.IPFMode)))
                {
                    // Need to update the current Item Proposal Lists
                    IPFUpdateItem IPFUpdateItem = new IPFUpdateItem() { ItemProposalItem = item, ApprovalItem = approvalItem, MarketingClaimsItem = marketingItem };
                    itemProposalService.UpdateIPF(IPFUpdateItem, bSubmitted);

                    //itemProposalService.UpdateItemProposalItem(item, bSubmitted);
                    //itemProposalService.UpdateItemProposalApprovalItem(approvalItem, bSubmitted);
                    //itemProposalService.UpdateMarketingClaimsItem(marketingItem);
                }
                else
                {
                    sourceId = iItemId;
                    // Cancel project being copied from and other derivatives
                    if ((!string.IsNullOrEmpty(this.IPFMode)) && (string.Equals(this.IPFMode.ToUpper(), GlobalConstants.QUERYSTRINGVALUE_IPFChange)))
                    {
                        itemProposalService.CancelItemProposals(item.ProjectNumber, this.ProjectRejected);
                    }
                    // Insert the Item Proposal
                    item.StageGateProjectListItemId = (string.IsNullOrEmpty(hdnStageGateListItemId.Value)) ? 0 : Convert.ToInt32(hdnStageGateListItemId.Value);
                    try
                    {
                        item.ParentProjectNumber = hdnParentProjectNumber.Value;
                    }
                    catch (Exception e)
                    {
                        LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": ParentProjectNumber was not inserted in project: " + e.Message);
                    }

                    string PMTWorkflowVersion = configurationService.GetConfiguration(SystemConfiguration.PMTWorkflowVersion);
                    item.PMTWorkflowVersion = string.IsNullOrEmpty(PMTWorkflowVersion) ? 9999 : Convert.ToInt32(PMTWorkflowVersion);

                    item.CompassListItemId = iItemId = itemProposalService.InsertItemProposalItem(item);
                    //insert Compass List 2
                    itemProposalService.InsertCompassList2(item);

                    approvalItem.CompassListItemId = item.CompassListItemId;
                    // Insert Approval Info
                    itemProposalService.InsertApprovalItem(approvalItem, item.ProjectNumber);
                    // Insert Project Decision Info
                    itemProposalService.InsertProjectDecisionItem(item.CompassListItemId, item.ProjectNumber);
                    // Insert Email Logging Info
                    itemProposalService.InsertEmailLoggingItem(item.CompassListItemId, item.ProjectNumber);
                    // Insert Workflow Status Info
                    itemProposalService.InsertWorkflowStatusItem(item.CompassListItemId, item.ProjectNumber);
                    marketingItem.CompassListItemId = item.CompassListItemId;
                    itemProposalService.InsertMarketingClaimsItem(marketingItem);
                    string Mode = string.Empty;
                    if ((!string.IsNullOrEmpty(this.IPFMode)) && (string.Equals(this.IPFMode.ToUpper(), GlobalConstants.QUERYSTRINGVALUE_IPFChange)))
                    {
                        // Perform Deep Copy of Project
                        itemProposalService.CopyCompassItem(sourceId, item.CompassListItemId, "");
                        //Copy compass List 2
                        itemProposalService.CopyCompassList2Item(sourceId, item.CompassListItemId, item.ProjectNumber, "");
                        // Copy all attachments on a Change Request
                        this.utilityService.CopyFiles(ProjectNumber, item.ProjectNumber);
                        //copy project notes
                        projectNotesService.CopyProjectNotes(sourceId, item.CompassListItemId);

                        Mode = "ChangeRequest";
                    }
                    itemProposalService.CopyMarketingClaimsItem(sourceId, item.CompassListItemId, item.ProjectNumber, Mode);
                    // Copy packaging items
                    packagingItemService.CopyPackagingItems(sourceId, ProjectNumber, item);

                }
            }
            catch (Exception exception)
            {
                bSuccess = false;
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": SaveItemProposal(main): " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "SaveItemProposal(main)");
            }

            return bSuccess;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                ItemProposalItem item = ConstructFormData(false);
                if (!SaveItemProposal(item, false))
                    return;

                SaveAllRepeaterItems();
                LoadFormulationAttachments();
                LoadGraphicsAttachments();

                ViewState["Changes Saved"] = "Changes Saved: " + DateTime.Now.ToString();
                lblSaved.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError("Error occurred while saving: " + exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": btnSave_Click: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateForm())
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SetFocus", "setFocusError();", true);
                    return;
                }
                // Get the Form Data
                ItemProposalItem item = ConstructFormData(true);

                // Set the Submitted Date for the Compass list
                item.SubmittedDate = DateTime.Now;

                // If this is supposed to be a new IPF then set id to zero
                if (!string.IsNullOrEmpty(this.IPFMode))
                    item.CompassListItemId = 0;

                if (!SaveItemProposal(item, true))
                    return;

                DeleteItems();

                SaveAllRepeaterItems();

                //Copy Forms From Previous projects
                CopyFormsFromPreviousprojects();

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.IPF);

                // Disable Submit to only allow 1 submit
                this.btnSubmit.Enabled = false;

                // Redirect to Home page after successfull Submit                        
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception exception)
            {
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": btnSubmit_Click: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "btnSubmit_Click");
            }
        }
        protected void btnCopyInParentProjectTeam_Click(object sender, EventArgs e)
        {
            var StageGateListItemId = hdnStageGateListItemId.Value;
            if (!string.IsNullOrEmpty(StageGateListItemId))
            {
                if (Convert.ToInt32(StageGateListItemId) != 0)
                {
                    StageGateCreateProjectItem parentItem = SGSService.GetStageGateProjectItem(Convert.ToInt32(StageGateListItemId));
                    ItemProposalItem ipf = new ItemProposalItem();
                    ipf.ProjectLeader = parentItem.ProjectLeader;
                    ipf.ProjectLeaderName = parentItem.ProjectLeaderName;
                    ipf.PM = parentItem.ProjectManager;
                    ipf.PMName = parentItem.ProjectManagerName;
                    ipf.Marketing = parentItem.Marketing;
                    ipf.MarketingName = parentItem.MarketingName;
                    ipf.SrProjectManager = parentItem.SeniorProjectManager;
                    ipf.SrProjectManagerName = parentItem.SeniorProjectManagerName;
                    ipf.QA = parentItem.QAInnovation;
                    ipf.QAName = parentItem.QAInnovationName;
                    ipf.InTech = parentItem.InTech;
                    ipf.InTechName = parentItem.InTechName;
                    ipf.InTechRegulatory = parentItem.InTechRegulatory;
                    ipf.InTechRegulatoryName = parentItem.InTechRegulatoryName;
                    ipf.RegulatoryQA = parentItem.RegulatoryQA;
                    ipf.RegulatoryQAName = parentItem.RegulatoryQAName;
                    ipf.PackagingEngineering = parentItem.PackagingEngineering;
                    ipf.PackagingEngineeringName = parentItem.PackagingEngineeringName;
                    ipf.SupplyChain = parentItem.SupplyChain;
                    ipf.SupplyChainName = parentItem.SupplyChainName;
                    ipf.Finance = parentItem.Finance;
                    ipf.FinanceName = parentItem.FinanceName;
                    ipf.Sales = parentItem.Sales;
                    ipf.SalesName = parentItem.SalesName;
                    ipf.Manufacturing = parentItem.Manufacturing;
                    ipf.ManufacturingName = parentItem.ManufacturingName;
                    ipf.OtherTeamMembers = parentItem.OtherMember;
                    ipf.OtherTeamMembersName = parentItem.OtherMemberName;
                    ipf.LifeCycleManagement = parentItem.LifeCycleManagement;
                    ipf.LifeCycleManagementName = parentItem.LifeCycleManagementName;
                    ipf.PackagingProcurement = parentItem.PackagingProcurement;
                    ipf.PackagingProcurementName = parentItem.PackagingProcurementName;
                    ipf.ExtManufacturingProc = parentItem.ExtMfgProcurement;
                    ipf.ExtManufacturingProcName = parentItem.ExtMfgProcurementName;

                    LoadProjectTeamData(ipf);
                    //btnSave_Click(sender, e);
                }
            }
        }
        protected void hdnbtnDeleteApprovedGraphicsAsset_Click(object sender, EventArgs e)
        {
            var hdnDeleteApprovedGraphicsAssetUrl = (HiddenField)this.FindControl("hdnDeleteApprovedGraphicsAssetUrl");
            if (hdnDeleteApprovedGraphicsAssetUrl != null && !string.IsNullOrEmpty(hdnDeleteApprovedGraphicsAssetUrl.Value))
            {
                utilityService.DeleteAttachment(hdnDeleteApprovedGraphicsAssetUrl.Value);
                SaveAllRepeaterItems();
            }
        }
        protected void btnhdnDeleteVisualreferenceUrl_Click(object sender, EventArgs e)
        {
            var hdnDeleteVisualreferenceUrl = (HiddenField)this.FindControl("hdnDeleteVisualreferenceUrl");
            if (hdnDeleteVisualreferenceUrl != null && !string.IsNullOrEmpty(hdnDeleteVisualreferenceUrl.Value))
            {
                utilityService.DeleteAttachment(hdnDeleteVisualreferenceUrl.Value);
                SaveAllRepeaterItems();
            }
        }
        #endregion
        protected void rpMixesSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Label lblNumberPieces, lblOzPerPiece, lblOzPerSellingUnit, lblGramsPerSellingUnit, lblLbsFGBOM, lblQtyMix;
            double OzPerSellingUnit, QtyFGBOM;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                lblNumberPieces = (Label)e.Item.FindControl("lblNumberPieces");
                lblOzPerPiece = (Label)e.Item.FindControl("lblOzPerPiece");
                lblOzPerSellingUnit = (Label)e.Item.FindControl("lblOzPerSellingUnit");
                lblGramsPerSellingUnit = (Label)e.Item.FindControl("lblGramsPerSellingUnit");
                lblLbsFGBOM = (Label)e.Item.FindControl("lblLbsFGBOM");
                lblQtyMix = (Label)e.Item.FindControl("lblQtyMix");
                lblOzPerSellingUnit.Text = "0";
                lblGramsPerSellingUnit.Text = "0";
                lblLbsFGBOM.Text = "0.00";
                lblQtyMix.Text = "0";
                if (!String.IsNullOrEmpty(lblNumberPieces.Text) && !String.IsNullOrEmpty(lblOzPerPiece.Text))
                {
                    if (Convert.ToDecimal(lblNumberPieces.Text) > 0 && Convert.ToDecimal(lblOzPerPiece.Text) > 0)
                    {
                        OzPerSellingUnit = Convert.ToDouble(lblNumberPieces.Text) * Convert.ToDouble(lblOzPerPiece.Text);
                        lblOzPerSellingUnit.Text = OzPerSellingUnit.ToString();
                        lblGramsPerSellingUnit.Text = (OzPerSellingUnit * 28.3495).ToString("0.00");
                        if (!String.IsNullOrEmpty(txtRetailSellingUnitsPerBaseUOM.Text))
                            if (Convert.ToDecimal(txtRetailSellingUnitsPerBaseUOM.Text) > 0)
                            {
                                QtyFGBOM = Convert.ToDouble(lblNumberPieces.Text) * Convert.ToDouble(txtRetailSellingUnitsPerBaseUOM.Text);
                                lblLbsFGBOM.Text = (QtyFGBOM * Convert.ToDouble(lblOzPerPiece.Text) / 16.0).ToString("0.00");
                                lblQtyMix.Text = QtyFGBOM.ToString();
                            }
                    }
                }
            }
        }
        protected void rpPackagingItemSummary_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string matLikeNumb, matLikeDesc;
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                PackagingItem packagingItem = (PackagingItem)e.Item.DataItem;

                TextBox txtMaterial = (TextBox)e.Item.FindControl("txtMaterial");
                TextBox txtMaterialDesc = (TextBox)e.Item.FindControl("txtMaterialDesc");
                TextBox txtLikeMaterialDesc = (TextBox)e.Item.FindControl("txtLikeMaterialDesc");
                TextBox txtLikeMaterial = (TextBox)e.Item.FindControl("txtLikeMaterial");
                TextBox txtOldMaterialDesc = (TextBox)e.Item.FindControl("txtOldMaterialDesc");
                TextBox txtOldMaterial = (TextBox)e.Item.FindControl("txtOldMaterial");
                DropDownList drpGraphicsNeeded = ((DropDownList)e.Item.FindControl("drpGraphicsNeeded"));
                DropDownList ddlGraphicsVendor = ((DropDownList)e.Item.FindControl("ddlGraphicsVendor"));
                TextBox txtGraphicsBrief = (TextBox)e.Item.FindControl("txtGraphicsBrief");
                HtmlControl spanGraphicsBrief = (HtmlControl)e.Item.FindControl("spanGraphicsBrief");
                HtmlControl spanWhyComponent = (HtmlControl)e.Item.FindControl("spanWhyComponent");
                TextBox txtLikeReason = (TextBox)e.Item.FindControl("txtLikeReason");
                HtmlControl spanGraphicsVendor = (HtmlControl)e.Item.FindControl("spanGraphicsVendor");
                HiddenField hdnDeletedStatus = (HiddenField)e.Item.FindControl("hdnDeletedStatus");
                HiddenField hdnParentId = (HiddenField)e.Item.FindControl("hdnParentId");
                hdnParentId.Value = packagingItem.ParentID.ToString();
                HiddenField hdnComponentType = (HiddenField)e.Item.FindControl("hdnComponentType");
                hdnComponentType.Value = packagingItem.PackagingComponent;
                TextBox txtPackQty = (TextBox)e.Item.FindControl("txtPackQty");

                hdnDeletedStatus.Value = packagingItem.Deleted;
                if (!string.IsNullOrEmpty(packagingItem.PackQuantity))
                {
                    var pacqty = (packagingItem.PackQuantity == "-9999" || string.IsNullOrEmpty(packagingItem.PackQuantity)) ? "0" : packagingItem.PackQuantity;
                    txtPackQty.Text = Math.Round(Convert.ToDouble(pacqty), 4).ToString();
                }
                txtLikeReason.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItemReason) ? "" : packagingItem.CurrentLikeItemReason;
                txtGraphicsBrief.Text = string.IsNullOrEmpty(packagingItem.CurrentLikeItem) ? "" : packagingItem.GraphicsBrief;
                DropDownList ddlPackagingComponent = ((DropDownList)e.Item.FindControl("drpComponent"));
                Utilities.BindDropDownItems(ddlPackagingComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl);
                if (packagingItem.PackagingComponent != null && packagingItem.PackagingComponent != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, ddlPackagingComponent, this.Page);
                }

                DropDownList drpUnitOfMeasure = ((DropDownList)e.Item.FindControl("drpUnitOfMeasure"));
                Utilities.BindDropDownItems(drpUnitOfMeasure, GlobalConstants.LIST_PackUnitLookup, webUrl);
                if (packagingItem.PackUnit != null && packagingItem.PackUnit != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.PackUnit, drpUnitOfMeasure, this.Page);
                }

                DropDownList drpComponentContainsNLEA = ((DropDownList)e.Item.FindControl("drpComponentContainsNLEA"));
                if (packagingItem.ComponentContainsNLEA != null && packagingItem.ComponentContainsNLEA != string.Empty)
                    Utilities.SetDropDownValue(packagingItem.ComponentContainsNLEA, drpComponentContainsNLEA, this.Page);

                matLikeNumb = string.IsNullOrEmpty(packagingItem.CurrentLikeItem) ? "" : packagingItem.CurrentLikeItem;
                matLikeDesc = string.IsNullOrEmpty(packagingItem.CurrentLikeItemDescription) ? "" : packagingItem.CurrentLikeItemDescription;
                if (matLikeDesc == "N/A")
                {
                    matLikeDesc = "Not Applicable";
                }
                if (matLikeNumb == "N/A")
                {
                    matLikeNumb = "Not Applicable";
                }
                txtLikeMaterial.Text = matLikeNumb;
                txtLikeMaterialDesc.Text = matLikeDesc;
                matLikeNumb = string.IsNullOrEmpty(packagingItem.CurrentOldItem) ? "" : packagingItem.CurrentOldItem;
                matLikeDesc = string.IsNullOrEmpty(packagingItem.CurrentOldItemDescription) ? "" : packagingItem.CurrentOldItemDescription;
                if (matLikeDesc == "N/A")
                {
                    matLikeDesc = "Not Applicable";
                }
                if (matLikeNumb == "N/A")
                {
                    matLikeNumb = "Not Applicable";
                }
                txtOldMaterial.Text = matLikeNumb;
                txtOldMaterialDesc.Text = matLikeDesc;
                DropDownList drpNew = ((DropDownList)e.Item.FindControl("drpNew"));
                if (packagingItem.NewExisting != null && packagingItem.NewExisting != string.Empty)
                {
                    Utilities.SetDropDownValue(packagingItem.NewExisting, drpNew, this.Page);
                }

                Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);
                if (!string.IsNullOrEmpty(packagingItem.ExternalGraphicsVendor))
                {
                    Utilities.SetDropDownValue(packagingItem.ExternalGraphicsVendor, ddlGraphicsVendor, this.Page);
                }

                if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                {
                    Utilities.SetDropDownValue(packagingItem.GraphicsChangeRequired, drpGraphicsNeeded, this.Page);

                    if (packagingItem.GraphicsChangeRequired.ToLower() == "yes")
                    {
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief PCBOMrequired";
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief showItempc");
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor PCBOMrequired";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor showItempc");
                    }
                    else
                    {
                        spanGraphicsBrief.Attributes.Add("class", "markrequired spanGraphicsBrief hideItem");
                        txtGraphicsBrief.CssClass = "form-control GraphicsBrief";
                        spanGraphicsVendor.Attributes.Add("class", "markrequired spanGraphicsVendor hideItem");
                        ddlGraphicsVendor.CssClass = "form-control drpGraphicsVendor";
                    }

                }

                HtmlImage imgAtt = (HtmlImage)e.Item.FindControl("btnAttachment");
                var files = packagingItemService.GetUploadedFiles(ProjectNumber, packagingItem.Id, GlobalConstants.DOCTYPE_Rendering);
                if (files.Count > 0)
                {
                    ImageButton btnDeleteAttachment = (ImageButton)e.Item.FindControl("btnDeleteAttachment");
                    HtmlAnchor anc = ((HtmlAnchor)e.Item.FindControl("ancRendering"));
                    btnDeleteAttachment.Visible = true;
                    if (anc != null)
                    {
                        string fileName = files[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        anc.Controls.Add(new LiteralControl(fileName));
                        anc.HRef = files[0].FileUrl;
                        btnDeleteAttachment.CommandArgument = files[0].FileUrl;
                    }

                    imgAtt.Visible = false;
                }
            }
        }
        private void Summary()
        {
            ItemProposalItem ipfItem = itemProposalService.GetItemProposalItem(iItemId);
            LoadSummaryProjectTeamData(ipfItem);
            List<PackagingItem> dtPackingItem;
            List<KeyValuePair<string, string>> sgItem = new List<KeyValuePair<string, string>>();
            dtPackingItem = itemProposalService.GetAllPackagingComponentsForProject(iItemId);

            List<PackagingItem> idList = new List<PackagingItem>();
            List<PackagingItem> PCSidList = new List<PackagingItem>();


            if (dtPackingItem.Count > 0)
            {
                int count = 0;
                //Packaging Items
                sgItem.Add(new KeyValuePair<string, string>("Finished Good BOM Summary", "header"));
                foreach (PackagingItem item in dtPackingItem)
                {
                    string parentID = Convert.ToString(item.ParentID);
                    string packagingComponent = Convert.ToString(item.PackagingComponent);
                    string newExisting = Convert.ToString(item.NewExisting);
                    string id = Convert.ToString(item.Id);
                    if (packagingComponent == "Transfer Semi")
                    {
                        idList.Add(item);
                    }
                    if (packagingComponent == "Purchased Candy Semi")
                    {
                        PCSidList.Add(item);
                    }

                    if (parentID != "0" && parentID != "" && packagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        continue;
                    }
                    if (parentID != "0" && parentID != "" && packagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                    {
                        continue;
                    }
                    if (parentID == "0")
                    {
                        sgItem.Add(new KeyValuePair<string, string>("new", ""));
                        sgItem.Add(new KeyValuePair<string, string>("Packaging Type", Convert.ToString(item.PackagingComponent)));
                        sgItem.Add(new KeyValuePair<string, string>("New or Existing?", newExisting));
                        sgItem.Add(new KeyValuePair<string, string>("Pack Qty", Convert.ToString(item.PackQuantity)));
                        sgItem.Add(new KeyValuePair<string, string>("UOM", Convert.ToString(item.PackUnit)));
                        sgItem.Add(new KeyValuePair<string, string>("Graphics Required?", Convert.ToString(item.GraphicsChangeRequired)));
                        sgItem.Add(new KeyValuePair<string, string>("Graphics Vendor", Convert.ToString(item.ExternalGraphicsVendor)));
                        sgItem.Add(new KeyValuePair<string, string>("Component #", Convert.ToString(item.MaterialNumber)));
                        sgItem.Add(new KeyValuePair<string, string>("Component Description", Convert.ToString(item.MaterialDescription)));
                        sgItem.Add(new KeyValuePair<string, string>("Like/Old Component #", Convert.ToString(item.CurrentLikeItem)));
                        sgItem.Add(new KeyValuePair<string, string>("Like/Old Component Description", Convert.ToString(item.CurrentLikeItemDescription)));
                        sgItem.Add(new KeyValuePair<string, string>("How is it a Like Component #", Convert.ToString(item.CurrentLikeItemReason)));
                        sgItem.Add(new KeyValuePair<string, string>("Graphics Brief", Convert.ToString(item.GraphicsBrief)));
                        sgItem.Add(new KeyValuePair<string, string>("Flowthrough", Convert.ToString(item.Flowthrough)));
                        sgItem.Add(new KeyValuePair<string, string>("Component requires consumer facing labeling?", Convert.ToString(item.ComponentContainsNLEA)));
                        if ((packagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || packagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi) && newExisting == "New")
                        {
                            sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 1", Convert.ToString(item.PHL1)));
                            sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 2", Convert.ToString(item.PHL2)));
                            sgItem.Add(new KeyValuePair<string, string>("Material Group 1 (Brand)", Convert.ToString(item.Brand)));
                            sgItem.Add(new KeyValuePair<string, string>("Profit Center", Convert.ToString(item.ProfitCenter)));
                        }
                        sgItem.Add(new KeyValuePair<string, string>("Transfer/Purchase Candy Semi Comments", Convert.ToString(item.Notes)));
                        sgItem.Add(new KeyValuePair<string, string>("endcomponent", "endcomponent"));
                    }
                    count++;
                }
                sgItem.Add(new KeyValuePair<string, string>("endFG", "endFG"));

                if (idList.Count > 0 || PCSidList.Count > 0)
                {

                    List<BOM> SemiBOMDetails = new List<BOM>();
                    SemiBOMDetails = dtPackingItem
                                    .Where(c => c.ParentID == 0 && (c.PackagingComponent == "Transfer Semi" || c.PackagingComponent == "Purchased Candy Semi"))
                                    .Select(c => new BOM()
                                    {
                                        Item = c,
                                        Children = GetChildren(dtPackingItem, c.Id)
                                    })
                                    .ToList();

                    HieararchySummary(SemiBOMDetails, sgItem);
                }

                if (sgItem.Count > 0)
                {
                    int endFG = sgItem.IndexOf(new KeyValuePair<string, string>("endFG", "endFG"));
                    if (endFG > 0)
                    {
                        int FGindex = sgItem.IndexOf(new KeyValuePair<string, string>("new", ""));
                        int FGcount = endFG - FGindex + 1;
                        List<KeyValuePair<string, string>> finishedGood = sgItem.GetRange(FGindex - 1, FGcount);
                        Table packTable = packagingItemsTable(finishedGood, "");
                        packTable.CssClass = "packTable";
                        commercializationPanel.Controls.Add(packTable);
                    }

                    int TSAndPCSCount = (from key in sgItem where key.Key == "newTS" select key.Key).Count();


                    if (TSAndPCSCount > 0)
                    {
                        int endTS = sgItem.LastIndexOf(new KeyValuePair<string, string>("endTS", "endTS"));
                        if (endTS != -1)
                        {
                            int TSindex = sgItem.IndexOf(new KeyValuePair<string, string>("newTS", ""));
                            int endTSindex = sgItem.IndexOf(new KeyValuePair<string, string>("endTS", "endTS"));
                            int TScount = endTSindex - TSindex;
                            while (endTSindex != -1)
                            {
                                List<KeyValuePair<string, string>> transferSemi = sgItem.GetRange(TSindex, TScount);
                                Table packTable = packagingItemsTable(transferSemi, "TS");
                                packTable.CssClass = "packTable";
                                commercializationPanel.Controls.Add(packTable);
                                TSindex = endTSindex + 1;
                                endTSindex = sgItem.IndexOf(new KeyValuePair<string, string>("endTS", "endTS"), TSindex);
                                TScount = endTSindex - TSindex;
                            }
                        }
                    }
                }

            }
        }
        private List<BOM> GetChildren(List<PackagingItem> BOMDetails, int parentId)
        {
            return BOMDetails
                    .Where(c => c.ParentID == parentId)
                    .Select(c => new BOM
                    {
                        Item = c,
                        Children = GetChildren(BOMDetails, c.Id)

                    })
                    .ToList();
        }
        private void HieararchySummary(List<BOM> BOMDetails, List<KeyValuePair<string, string>> sgItem)
        {
            indent++;
            if (BOMDetails != null)
            {
                foreach (var subItem in BOMDetails)
                {
                    if (subItem.Item.PackagingComponent == "Transfer Semi" || subItem.Item.PackagingComponent == "Purchased Candy Semi")
                    {
                        sgItem.Add(new KeyValuePair<string, string>("newTS", ""));
                        sgItem.Add(new KeyValuePair<string, string>("Index", Convert.ToString(indent * 50)));
                        string TSHeader = "New " + subItem.Item.PackagingComponent + " Summary: " + Convert.ToString(subItem.Item.MaterialNumber) + " - " + Convert.ToString(subItem.Item.MaterialDescription);
                        sgItem.Add(new KeyValuePair<string, string>(TSHeader, "header"));

                        foreach (var childItem in subItem.Children)
                        {
                            string PackagingComponent = Convert.ToString(childItem.Item.PackagingComponent);
                            string NewExisting = Convert.ToString(childItem.Item.NewExisting);

                            sgItem.Add(new KeyValuePair<string, string>("Packaging Type", Convert.ToString(childItem.Item.PackagingComponent)));
                            sgItem.Add(new KeyValuePair<string, string>("New or Existing?", childItem.Item.NewExisting));
                            sgItem.Add(new KeyValuePair<string, string>("Pack Qty", Convert.ToString(childItem.Item.PackQuantity)));
                            sgItem.Add(new KeyValuePair<string, string>("UOM", Convert.ToString(childItem.Item.PackUnit)));
                            sgItem.Add(new KeyValuePair<string, string>("Graphics Required?", Convert.ToString(childItem.Item.GraphicsChangeRequired)));
                            sgItem.Add(new KeyValuePair<string, string>("Graphics Vendor", Convert.ToString(childItem.Item.ExternalGraphicsVendor)));
                            sgItem.Add(new KeyValuePair<string, string>("Component #", Convert.ToString(childItem.Item.MaterialNumber)));
                            sgItem.Add(new KeyValuePair<string, string>("Component Description", Convert.ToString(childItem.Item.MaterialDescription)));
                            sgItem.Add(new KeyValuePair<string, string>("Like/Old Component #", Convert.ToString(childItem.Item.CurrentLikeItem)));
                            sgItem.Add(new KeyValuePair<string, string>("Like/Old Component Description", Convert.ToString(childItem.Item.CurrentLikeItemDescription)));
                            sgItem.Add(new KeyValuePair<string, string>("How is it a Like Component #", Convert.ToString(childItem.Item.CurrentLikeItemReason)));
                            sgItem.Add(new KeyValuePair<string, string>("Graphics Brief", Convert.ToString(childItem.Item.GraphicsBrief)));
                            sgItem.Add(new KeyValuePair<string, string>("Component requires consumer facing labeling?", Convert.ToString(childItem.Item.ComponentContainsNLEA)));
                            sgItem.Add(new KeyValuePair<string, string>("Flowthrough", Convert.ToString(childItem.Item.Flowthrough)));
                            if ((PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi) || NewExisting == "New")
                            {
                                sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 1", Convert.ToString(childItem.Item.PHL1)));
                                sgItem.Add(new KeyValuePair<string, string>("Product Hierarchy Level 2", Convert.ToString(childItem.Item.PHL2)));
                                sgItem.Add(new KeyValuePair<string, string>("Material Group 1 (Brand)", Convert.ToString(childItem.Item.Brand)));
                                sgItem.Add(new KeyValuePair<string, string>("Profit Center", Convert.ToString(childItem.Item.ProfitCenter)));
                            }
                            sgItem.Add(new KeyValuePair<string, string>("endcomponent", "endcomponent"));
                        }
                        sgItem.Add(new KeyValuePair<string, string>("endTS", "endTS"));
                    }

                    HieararchySummary(subItem.Children, sgItem);
                    indent--;
                }
            }
        }
        private Table packagingItemsTable(List<KeyValuePair<string, string>> newTrial, string breaker)
        {

            Table packagingTable = new Table();
            string newBreaker = "new" + breaker;
            string indent = "0";

            Table subDetailsPackTable = new Table();
            int subHeadindex = 0;
            int packageCount = 0;

            foreach (KeyValuePair<string, string> pair in newTrial.Where(r => r.Key != newBreaker))
            {

                if (pair.Key == "Packaging Type")
                {
                    subHeadindex = 0;
                    packageCount++;

                }


                TableRow newRow = new TableRow();
                TableCell newCell = new TableCell();
                TableCell newCell2 = new TableCell();
                int insertLoc = subDetailsPackTable.Rows.Count - 1;
                if (packageCount % 2 != 0)
                {
                    newRow.CssClass = "oddRow";
                }
                if (pair.Key == "Index")
                {
                    indent = pair.Value;

                }

                if (pair.Value == "header")
                {
                    newCell.Text = "<br/> <span class=headSummary>" + pair.Key + "</span > <hr />";
                    newCell.ColumnSpan = 4;
                    newRow.Cells.Add(newCell);
                    newRow.CssClass = "";
                    subDetailsPackTable.Rows.Add(newRow);
                    subHeadindex = 0;
                }
                else if (pair.Key == "endTS")
                {
                    continue;
                }
                else if (pair.Key == "endcomponent")
                {
                    newCell.Text = "<div><hr /></div>";
                    newCell.ColumnSpan = 4;
                    newRow.Cells.Add(newCell);
                    newRow.CssClass = "";
                    subDetailsPackTable.Rows.Add(newRow);
                }
                else if (pair.Key == "Index")
                {
                    continue;
                }
                else
                {

                    newCell.Text = "<strong>" + pair.Key + ":</strong>";
                    newCell2.Text = pair.Value;


                    if (insertLoc < 0 || subHeadindex == 0 || subHeadindex % 2 == 0)
                    {
                        newRow.Cells.Add(newCell);
                        newRow.Cells.Add(newCell2);
                        subDetailsPackTable.Rows.Add(newRow);

                    }
                    else
                    {
                        subDetailsPackTable.Rows[insertLoc].Cells.Add(newCell);
                        subDetailsPackTable.Rows[insertLoc].Cells.Add(newCell2);
                    }
                    subHeadindex++;
                }


            }
            TableRow packDetailsLastLastRow = new TableRow();
            TableCell packDetailsLastLastCell = new TableCell();
            packDetailsLastLastCell.ColumnSpan = 4;
            packDetailsLastLastCell.Controls.Add(subDetailsPackTable);
            packDetailsLastLastRow.Cells.Add(packDetailsLastLastCell);
            packagingTable.Rows.Add(packDetailsLastLastRow);
            packagingTable.Style.Add("margin-left", indent + "px!important");



            return packagingTable;
        }
        private void CheckForNA(out bool NA, out string NAText, PeopleEditor peopleEditor)
        {
            NA = false;
            NAText = string.Empty;
            foreach (PickerEntity entity in peopleEditor.Entities)
            {
                if (entity.Key.ToUpper() == "NA" || entity.Key.ToUpper() == "NOT APPLICABLE" || entity.Key.ToUpper() == "N/A")
                {
                    NAText = entity.Key.ToUpper();
                    NA = true;
                }
                if (entity.DisplayText.ToUpper().Trim() == "NA" || entity.DisplayText.ToUpper().Trim() == "NOT APPLICABLE" || entity.DisplayText.ToUpper().Trim() == "N/A")
                {
                    NAText = entity.DisplayText.ToUpper();
                    NA = true;
                }
            }
        }
        #region Repeater Methods for Project Teams
        protected void rptProjectLeaders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peProjectLeaderMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }

            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptOtherTeamMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peOtherTeamMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        #endregion       
        #region Add Team members Button events
        protected void btnAddProjectLeader_Click(object sender, EventArgs e)
        {
            AddMembers(divProjectLeaders, "rptProjectLeaders", "peProjectLeaderMembers", "hdnDeletedStatusForProjectLeader");
        }
        protected void btnAddProjectManager_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptProjectManagers, "ProjectManagerMembers", ddlProjectManagerMembers);
        }
        protected void btnAddSrProjectManagers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptSrProjectManagers, "SrProjectManagerMembers", ddlSrProjectManagerMembers);
        }
        protected void btnAddMarketingMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptMarketingMembers, "MarketingMembers", ddlMarketingMembers);
        }
        protected void btnAddInTechMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptInTechMembers, "InTechMembers", ddlInTechMembers);
        }
        protected void btnAddQualityInnovationMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptQualityInnovationMembers, "QualityInnovationMembers", ddlQualityInnovationMembers);
        }
        protected void btnAddInTechRegulatoryMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptInTechRegulatoryMembers, "InTechRegulatoryMembers", ddlInTechRegulatoryMembers);
        }
        protected void btnAddRegulatoryQAMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptRegulatoryQAMembers, "RegulatoryQAMembers", ddlRegulatoryQAMembers);
        }
        protected void btnAddPackagingEngineeringMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptPackagingEngineeringMembers, "PackagingEngineeringMembers", ddlPackagingEngineeringMembers);
        }
        protected void btnAddSupplyChainMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptSupplyChainMembers, "SupplyChainMembers", ddlSupplyChainMembers);
        }
        protected void btnAddFinanceMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptFinanceMembers, "FinanceMembers", ddlFinanceMembers);
        }
        protected void btnAddSalesMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptSalesMembers, "SalesMembers", ddlSalesMembers);
        }
        protected void btnAddManufacturingMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptManufacturingMembers, "ManufacturingMembers", ddlManufacturingMembers);
        }
        protected void btnAddExternalMfgProcurementMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptExternalMfgProcurementMembers, "ExternalMfgProcurementMembers", ddlExternalMfgProcurementMembers);
        }
        protected void btnAddPackagingProcurementMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptPackagingProcurementMembers, "PackagingProcurementMembers", ddlPackagingProcurementMembers);
        }
        protected void btnAddLifeCycleManagementMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptLifeCycleManagementMembers, "LifeCycleManagementMembers", ddlLifeCycleManagementMembers);
        }
        protected void btnAddOtherTeamMembers_Click(object sender, EventArgs e)
        {
            AddMembers(divOtherTeamMembers, "rptOtherTeamMembers", "peOtherTeamMembers", "hdnDeletedStatusForOtherTeamMembers");
        }
        #endregion
        #region Common - Add Team members method
        private void AddTeamMemberButtonClick(Repeater repeaterName, string MemberName, DropDownList ddlName)
        {
            if (ddlName.SelectedItem.Value != "-1")
            {
                AddMembers_New(repeaterName, string.Concat("txt", MemberName), ddlName.SelectedItem.Text, ddlName.SelectedItem.Value, string.Concat("hdnDeletedStatusFor", MemberName));
                ddlName.SelectedIndex = -1;
                CallUpdatePeopleEditorScriptFunction();
            }
        }
        private void AddMembers_New(Repeater rptMembers, string txtBoxName, string NewMember, string NewMemberLoginName, string hiddenStatusFieldName)
        {
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            List<int> NAList = new List<int>();
            Dictionary<int, string> BadNamesList = new Dictionary<int, string>();
            int Counter = 0;
            List<TeamMember> listMembers = new List<TeamMember>();
            listMembers.Add(new TeamMember() { MemberName = NewMember, MemberLoginName = NewMemberLoginName });

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtMember = (TextBox)item.FindControl(txtBoxName);
                    HiddenField hiddenStatusField = (HiddenField)item.FindControl(hiddenStatusFieldName);
                    TextBox txtMembersLoginName = (TextBox)item.FindControl(string.Concat(txtBoxName, "LoginName"));

                    if (hiddenStatusField.Value != "true")
                    {
                        try
                        {
                            if (CheckForNA(txtMember.Text))
                            {
                                listMembers.Add(new TeamMember() { MemberName = "NA", MemberLoginName = "NA" });
                            }
                            else
                            {
                                listMembers.Add(new TeamMember() { MemberName = txtMember.Text, MemberLoginName = txtMembersLoginName.Text });
                            }
                        }
                        catch (Exception exception)
                        {

                            ErrorSummary.AddError("Error occurred while adding new member: " + exception.Message, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": AddMembers_New: " + exception.Message);
                            exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "AddMembers_New");
                        }
                    }
                }
                Counter++;
            }

            rptMembers.DataSource = listMembers;
            rptMembers.DataBind();
        }
        private void AddMembers(HtmlGenericControl div, string repeaterName, string PeopleEditorName, string hiddenStatusFieldName)
        {
            Repeater rptMembers = ((Repeater)div.FindControl(repeaterName));
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            bool NA;
            string NAText;
            List<int> NAList = new List<int>();
            Dictionary<int, string> BadNamesList = new Dictionary<int, string>();
            int Counter = 0;

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMembers = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField hiddenStatusField = (HiddenField)item.FindControl(hiddenStatusFieldName);

                    if (hiddenStatusField.Value != "true")
                    {
                        try
                        {
                            if (peMembers.Entities.Count > 0)
                            {
                                CheckForNA(out NA, out NAText, peMembers);

                                if (NA)
                                {
                                    NAList.Add(Counter);
                                }
                                else
                                {
                                    Members.AddRange(Utilities.GetPeopleFromPickerControl(peMembers, SPContext.Current.Web));
                                }
                            }
                            else
                            {
                                Members.Add(new SPFieldUserValue());
                            }
                        }
                        catch (Exception ex)
                        {
                            string BadText;
                            foreach (PickerEntity entity in peMembers.Entities)
                            {
                                BadText = entity.Key;
                                BadText = string.IsNullOrEmpty(BadText) ? entity.DisplayText : BadText;
                                BadNamesList.Add(Counter, BadText);
                            }
                        }
                    }
                }
                Counter++;
            }

            string users = "";
            List<string> listpeMembers = new List<string>();

            foreach (SPFieldUserValue Member in Members)
            {
                if (Member.User == null)
                {
                    users += ",";
                }
                else
                {
                    users += Member.User.ToString() + ",";
                }
            }

            if (string.IsNullOrEmpty(users))
            {
                listpeMembers.Add(string.Empty);
            }
            else
            {
                listpeMembers = users.Split(',').ToList();
            }

            foreach (var index in NAList)
            {
                listpeMembers.Insert(index, "NA");
            }

            foreach (var BadValue in BadNamesList)
            {
                listpeMembers.Insert(BadValue.Key, BadValue.Value);
            }
            rptMembers.DataSource = listpeMembers;
            rptMembers.DataBind();
        }
        #endregion
        private void CopyFormsFromPreviousprojects()
        {
            try
            {
                if (ddlCopyFormsForGraphicsProject.SelectedItem.Text == "Yes")
                {
                    ItemProposalItem item = itemProposalService.GetItemProposalItem(iItemId);
                    if (item.IPFCopiedFromCompassListItemId != 0 && item.CompassListItemId != 0)
                    {
                        itemProposalService.CopyFormsFromPreviousprojects(item.IPFCopiedFromCompassListItemId, item.CompassListItemId, ProjectNumber);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError("Copy Forms From Previous projects Failed.", this.Page);
                exceptionService.Handle(LogCategory.CriticalError, "Copy Forms From Previous projects Failed.", GlobalConstants.PAGE_ItemProposal, "CopyFormsFromPreviousprojects", string.Concat("ProjectNumber", ProjectNumber));
            }
        }
    }
}
