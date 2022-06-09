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
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.IO;


namespace Ferrara.Compass.WebParts.SAPInitialItemSetupForm
{
    [ToolboxItemAttribute(false)]
    public partial class SAPInitialItemSetupForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public SAPInitialItemSetupForm()
        {
        }

        #region Member Variables

        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private INotificationService notificationService;
        private ISAPMaterialMasterService sapMMService;
        private ISAPInitialItemSetUpService sapInitialISetUpService;
        private IOPSService opsService;
        private IPackagingItemService packagingService;
        private IShipperFinishedGoodService shipperFinishedGoodService;
        private IMixesService mixesService;
        private IItemProposalService proposalService;
        private IExternalManufacturingService externalMfgService;
        private ISAPBOMSetupService sapBOMSetupService;

        private int iItemId = 0;
        private List<PackagingItem> TransferSEMI = new List<PackagingItem>();
        private string webUrl;
        private const string _ucBOMEditable = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx";
        private const string _ucBOMGrid = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx";
        private IBOMSetupService BOMSetupService;
        private string PLMProject = "";
        private const string _ucBOMGridPackMeas_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGridPackMeas_New.ascx";
        private const string _ucBOMEditable_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx";
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            sapInitialISetUpService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPInitialItemSetUpService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            sapMMService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPMaterialMasterService>();
            opsService = DependencyResolution.DependencyMapper.Container.Resolve<IOPSService>();
            packagingService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            shipperFinishedGoodService = DependencyResolution.DependencyMapper.Container.Resolve<IShipperFinishedGoodService>();
            mixesService = DependencyResolution.DependencyMapper.Container.Resolve<IMixesService>();
            proposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            externalMfgService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();
            sapBOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMSetupService>();
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

                    InitializeScreen();

                    if (iItemId > 0)
                    {
                        LoadFormData();
                    }
                    if (hdnProjectType.Value.Contains("Renovations"))
                    {
                        dvMain.Visible = false;
                        dvMsg.Visible = true;
                    }
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPInitialSetup.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.SAPInitialSetup.ToString(), "Page_Load");
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
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.SAPInitialSetup))
            {
                this.divAccessDenied.Visible = true;
            }

            //If user does not have rights to save/ submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.SAPInitialSetup))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }

            //if (Utilities.LockScreen(wfStep.ToString()))
            //{
            //    if (!Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            //    {
            //        this.btnSave.Enabled = false;
            //        this.btnSubmit.Enabled = false;
            //    }
            //}

            string workflowPhase = utilityService.GetWorkflowPhase(iItemId);
            if ((string.Equals(workflowPhase, WorkflowStep.OnHold.ToString())) || (string.Equals(workflowPhase, WorkflowStep.Cancelled.ToString())) ||
                (string.Equals(workflowPhase, WorkflowStep.Completed.ToString())))
            {
                this.btnSave.Enabled = false;
                this.btnSubmit.Enabled = false;
            }
        }
        private void SetLookupValues(string sapNumber, bool sapLookup)
        {
            // Add BOM Items
            SAPMaterialMasterListItem mmItem = sapMMService.GetSAPMaterialMaster(sapNumber);
            if (sapLookup)
            {
                txtSAPDescription.Text = mmItem.SAPDescription;
            }

        }
        #endregion

        #region Data Transfer Methods
        private void LoadBOMGrid()
        {

            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            phBOM.Controls.Clear();
            dtPackingItem = packagingService.GetFinishedGoodItemsForProject(iItemId);

            foreach (PackagingItem item in dtPackingItem)
            {

                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBOMGrid ctrl2 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                    ctrl2.openBtnSave = openBtnSave;
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOM.Controls.Add(ctrl2);

                }
            }
            ucBOMGrid ctrl = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.openBtnSave = openBtnSave;

            phBOM.Controls.Add(ctrl);
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
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();

            phBOM.Controls.Clear();
            dtPackingItem = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);

            foreach (BOMSetupItem item in dtPackingItem)
            {
                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBOMGridPackMeas_New ctrl2 = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
                    ctrl2.ID = "grid" + item.Id.ToString();
                    ctrl2.iItemId = iItemId;
                    ctrl2.GridItem = item;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.openBtnSave = openBtnSave;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.NewExisting = item.NewExisting;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOM.Controls.Add(ctrl2);
                }
            }
            ucBOMGridPackMeas_New ctrl = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
            BOMSetupItem FGItem = new BOMSetupItem()
            {
                PackagingComponent = "Finished Good",
                MaterialNumber = hdnMaterialNumber.Value,
                MaterialDescription = hdnMaterialDesc.Value,
                ParentID = 0
            };
            ctrl.ID = "grid" + iItemId.ToString();
            ctrl.iItemId = iItemId;
            ctrl.GridItem = FGItem;
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.packagingItems = dtPackingItem;
            ctrl.ParentId = 0;
            ctrl.openBtnSave = openBtnSave;
            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.NewComponentCount = newItemCount;
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
        private void LoadFormData()
        {
            hdnPageName.Value = GlobalConstants.PAGE_SAPInitialItemSetup;
            hdnCompassListItemId.Value = iItemId.ToString();
            SAPInitialItemSetUp SAPInitialItemSetupItem = sapInitialISetUpService.GetSAPInitialSetupItem(iItemId);
            OPSItem opsItem = opsService.GetOPSItem(iItemId);
            ItemProposalItem ipf = proposalService.GetItemProposalItem(iItemId);
            #region SAP Initial Item Setup Tasks
            hdnMaterialNumber.Value = SAPInitialItemSetupItem.SAPItemNumber;
            hdnMaterialDesc.Value = SAPInitialItemSetupItem.SAPDescription;
            SAPBOMSetupItem sapbomsetupItem = sapBOMSetupService.GetSAPBOMSetupItem(iItemId);
            hdnPLMProject.Value = sapbomsetupItem.PLMProject;
            PLMProject = sapbomsetupItem.PLMProject;
            List<CheckBox> checkBoxBinding = checkboxSetup(ipf, sapbomsetupItem);
            rptCheckBoxes.DataSource = checkBoxBinding;
            rptCheckBoxes.DataBind();
            #endregion
            #region Project Details
            txtImmediateSPKChange.Text = SAPInitialItemSetupItem.ImmediateSPKChange;
            txtImmediateSPKChange.Visible = (string.IsNullOrEmpty(txtImmediateSPKChange.Text) || txtImmediateSPKChange.Text == "Select...") ? false : true;
            #region Immediate SPK Change For TSPC
            List<PackagingItem> TSPCs = packagingService.GetTransferPurchasedSemiItemsForProject(iItemId, GlobalConstants.COMPONENTTYPE_TransferSemi);
            TSPCs.AddRange(packagingService.GetTransferPurchasedSemiItemsForProject(iItemId, GlobalConstants.COMPONENTTYPE_PurchasedSemi));
            var TSPCsNonBlank = TSPCs.Where(x => (!string.IsNullOrEmpty(x.ImmediateSPKChange) && x.ImmediateSPKChange != "Select...")).ToList();
            rptImmediateSPKChangeForSPSC.DataSource = TSPCsNonBlank;
            rptImmediateSPKChangeForSPSC.DataBind();
            #endregion
            txtProjectType.Text = ipf.ProjectType;
            txtProjectTypeSubCategory.Text = ipf.ProjectTypeSubCategory;
            txtItemConcept.Text = ipf.ItemConcept;
            if (ipf.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblItemConcept.Text = "Change Notes:";
            }
            else
            {
                lblItemConcept.Text = "Item Concept:";
            }
            txtOldFGNumber.Text = ipf.OldFGItemNumber;
            txtOldFGDescription.Text = ipf.OldFGItemDescription;
            #endregion
            #region Item Details
            lblRetailSellingUnitsUOM.Text = Utilities.FormatNumber(SAPInitialItemSetupItem.RetailSellingUnitsBaseUOM);
            lblSAPBaseUOM.Text = SAPInitialItemSetupItem.SAPBaseUOM;
            lblRetailUnitWeight.Text = Utilities.FormatDecimal(SAPInitialItemSetupItem.RetailUnitWieghtOz, 2);
            lblProfitCenter.Text = SAPInitialItemSetupItem.ProfitCenter;
            #endregion
            #region Logistics Information
            txtMakeLocation.Text = opsItem.MakeLocation;
            txtPackLocation1.Text = opsItem.PackingLocation;
            //Procurement Type
            if (!string.IsNullOrEmpty(opsItem.CoManClassification) && !string.Equals(opsItem.CoManClassification, "Select..."))
            {
                divProcurementType.Visible = true;
                txtProcurementType.Text = opsItem.CoManClassification;
            }
            else
            {
                divProcurementType.Visible = false;
            }
            //External Manufacturer
            if (!string.IsNullOrEmpty(opsItem.ExternalManufacturer) && !string.Equals(opsItem.ExternalManufacturer, "Select..."))
            {
                divExternalManufacturer.Visible = true;
                txtExternalManufacturer.Text = opsItem.ExternalManufacturer;
            }
            else
            {
                divExternalManufacturer.Visible = false;
            }
            //External Packer
            if (!string.IsNullOrEmpty(opsItem.ExternalPacker) && !string.Equals(opsItem.ExternalPacker, "Select..."))
            {
                dvPackLocation.Visible = true;
                txtExternalPacker.Text = opsItem.ExternalPacker;
            }
            else
            {
                dvPackLocation.Visible = false;
            }
            //Purchased Into
            if (!string.IsNullOrEmpty(SAPInitialItemSetupItem.PurchasedIntoLocation) && !string.Equals(SAPInitialItemSetupItem.PurchasedIntoLocation, "Select..."))
            {
                divPurchaseIntoLocation.Visible = true;
                txtPurchaseIntoLocation.Text = SAPInitialItemSetupItem.PurchasedIntoLocation;
            }
            else
            {
                divPurchaseIntoLocation.Visible = false;
            }
            //SAP Base UOM
            txtSAPBaseUOM.Text = SAPInitialItemSetupItem.SAPBaseUOM;
            #endregion
            #region Deployment Information
            txtDesignateHUBDC.Text = SAPInitialItemSetupItem.DesignateHUBDC;
            txtDeploymentModeofItem.Text = SAPInitialItemSetupItem.DeploymentModeofItem;
            LoadControlsByCompanyCode(SAPInitialItemSetupItem);
            #endregion
            #region Product Hierarchy
            lblProductHierarchyLevel1.Text = SAPInitialItemSetupItem.ProductHierarchyLevel1;
            lblProductHierarchyLevel2.Text = SAPInitialItemSetupItem.ProductHierarchyLevel2;
            lblMaterialGroup.Text = SAPInitialItemSetupItem.MaterialGroup1Brand;
            lblTradePromo.Text = SAPInitialItemSetupItem.MaterialGroup2Pricing;
            lblMaterialGroup4.Text = SAPInitialItemSetupItem.MaterialGroup4ProductForm;
            lblMaterialGroup5.Text = SAPInitialItemSetupItem.MaterialGroup5PackType;
            #endregion
            #region Initial Item Setup
            hdnProjectType.Value = SAPInitialItemSetupItem.ProjectType;
            txtSAPItem.Text = string.IsNullOrEmpty(SAPInitialItemSetupItem.SAPItemNumber) ? "NEEDS NEW" : SAPInitialItemSetupItem.SAPItemNumber;
            txtSAPDescription.Text = string.IsNullOrEmpty(SAPInitialItemSetupItem.SAPDescription) ? "" : SAPInitialItemSetupItem.SAPDescription;
            //txtSAPDescription.ReadOnly = (SAPInitialItemSetupItem.NewIPF == "Yes") ? true : false;
            txtUnitUPC.Text = string.IsNullOrEmpty(SAPInitialItemSetupItem.UnitUPC) ? "NEEDS NEW" : SAPInitialItemSetupItem.UnitUPC;

            if (!string.IsNullOrEmpty(SAPInitialItemSetupItem.DisplayBoxUPC))
                txtDisplayUPC.Text = SAPInitialItemSetupItem.DisplayBoxUPC;
            else if (SAPInitialItemSetupItem.RequireNewDisplayBoxUPC.ToLower() == "yes")
                txtDisplayUPC.Text = "Needs New";
            else
                txtDisplayUPC.Text = "N/A";

            if (SAPInitialItemSetupItem.SAPBaseUOM.ToLower() == "pal")
            {
                txtCaseUCC.Text = "";
                dvCaseUcc.Visible = false;
            }
            else if (SAPInitialItemSetupItem.SAPBaseUOM.ToLower() == "cs")
                txtCaseUCC.Text = string.IsNullOrEmpty(SAPInitialItemSetupItem.CaseUCC) ? "Needs New" : SAPInitialItemSetupItem.CaseUCC;

            if (SAPInitialItemSetupItem.SAPBaseUOM.ToLower() == "cs")
            {
                txtPalletUCC.Text = "";
                dvPalletUcc.Visible = false;
            }
            else if (SAPInitialItemSetupItem.SAPBaseUOM.ToLower() == "pal")
                txtPalletUCC.Text = string.IsNullOrEmpty(SAPInitialItemSetupItem.PalletUCC) ? "Needs New" : SAPInitialItemSetupItem.PalletUCC;
            #endregion
            #region Shippers
            List<ShipperFinishedGoodItem> shipperData;
            if (SAPInitialItemSetupItem.MaterialGroup5PackType.ToLower() == "shipper (shp)" || SAPInitialItemSetupItem.MaterialGroup5PackType.ToLower() == "shippers (shp)")
            {
                shipperData = GetShipperFGItem(iItemId);
                if (shipperData.Count > 0)
                {
                    divShipper.Visible = true;
                    rpShipperSummary.DataSource = shipperData;
                    rpShipperSummary.DataBind();
                }
            }
            #endregion
            #region Mixes
            List<MixesItem> mixData;
            if (SAPInitialItemSetupItem.MaterialGroup4ProductForm == "MIXES (MIX)")
            {
                mixData = GetMixesItem(iItemId);
                if (mixData.Count > 0)
                {
                    divMixes.Visible = true;
                    rpMixesSummary.DataSource = mixData;
                    rpMixesSummary.DataBind();
                }
            }
            #endregion
        }
        private void LoadControlsByCompanyCode(SAPInitialItemSetUp SAPInitialItemSetupItem)
        {
            divSELLDCs.Visible = false;
            DivFERQDCs.Visible = false;
            #region SellDCs
            if (SAPInitialItemSetupItem.ProductHierarchyLevel1 != GlobalConstants.PRODUCT_HIERARCHY1_LBB)
            {
                divSELLDCs.Visible = true;
                DivFERQDCs.Visible = false;

                txtExtendtoSL07.Text = SAPInitialItemSetupItem.ExtendtoSL07;
                if (SAPInitialItemSetupItem.ExtendtoSL07 == "Yes")
                {
                    divSetSL07SPKto.Visible = true;
                    txtSetSL07SPKto.Text = SAPInitialItemSetupItem.SetSL07SPKto;
                }
                else
                {
                    divSetSL07SPKto.Visible = false;
                }

                txtExtendtoSL13.Text = SAPInitialItemSetupItem.ExtendtoSL13;
                if (SAPInitialItemSetupItem.ExtendtoSL13 == "Yes")
                {
                    divSetSL13SPKto.Visible = true;
                    txtSetSL13SPKto.Text = SAPInitialItemSetupItem.SetSL13SPKto;
                }
                else
                {
                    divSetSL13SPKto.Visible = false;
                }

                txtExtendtoSL18.Text = SAPInitialItemSetupItem.ExtendtoSL18;
                if (SAPInitialItemSetupItem.ExtendtoSL18 == "Yes")
                {
                    divSetSL18SPKto.Visible = true;
                    txtSetSL18SPKto.Text = SAPInitialItemSetupItem.SetSL18SPKto;
                }
                else
                {
                    divSetSL18SPKto.Visible = false;
                }

                txtExtendtoSL19.Text = SAPInitialItemSetupItem.ExtendtoSL19;
                if (SAPInitialItemSetupItem.ExtendtoSL19 == "Yes")
                {
                    divSetSL19SPKto.Visible = true;
                    txtSetSL19SPKto.Text = SAPInitialItemSetupItem.SetSL19SPKto;
                }
                else
                {
                    divSetSL19SPKto.Visible = false;
                }

                txtExtendtoSL30.Text = SAPInitialItemSetupItem.ExtendtoSL30;
                if (SAPInitialItemSetupItem.ExtendtoSL30 == "Yes")
                {
                    divSetSL30SPKto.Visible = true;
                    txtSetSL30SPKto.Text = SAPInitialItemSetupItem.SetSL30SPKto;
                }
                else
                {
                    divSetSL30SPKto.Visible = false;
                }

                txtExtendtoSL14.Text = SAPInitialItemSetupItem.ExtendtoSL14;
                if (SAPInitialItemSetupItem.ExtendtoSL14 == "Yes")
                {
                    divSetSL14SPKto.Visible = true;
                    txtSetSL14SPKto.Text = SAPInitialItemSetupItem.SetSL14SPKto;
                }
                else
                {
                    divSetSL14SPKto.Visible = false;
                }
            }
            #endregion
            #region FERQDCs
            else
            {
                divSELLDCs.Visible = false;
                DivFERQDCs.Visible = true;

                txtExtendtoFQ26.Text = SAPInitialItemSetupItem.ExtendtoFQ26;
                if (SAPInitialItemSetupItem.ExtendtoFQ26 == "Yes")
                {
                    divSetFQ26SPKto.Visible = true;
                    txtSetFQ26SPKto.Text = SAPInitialItemSetupItem.SetFQ26SPKto;
                }
                else
                {
                    divSetFQ26SPKto.Visible = false;
                }

                txtExtendtoFQ27.Text = SAPInitialItemSetupItem.ExtendtoFQ27;
                if (SAPInitialItemSetupItem.ExtendtoFQ27 == "Yes")
                {
                    divSetFQ27SPKto.Visible = true;
                    txtSetFQ27SPKto.Text = SAPInitialItemSetupItem.SetFQ27SPKto;
                }
                else
                {
                    divSetFQ27SPKto.Visible = false;
                }

                txtExtendtoFQ28.Text = SAPInitialItemSetupItem.ExtendtoFQ28;
                if (SAPInitialItemSetupItem.ExtendtoFQ28 == "Yes")
                {
                    divSetFQ28SPKto.Visible = true;
                    txtSetFQ28SPKto.Text = SAPInitialItemSetupItem.SetFQ28SPKto;
                }
                else
                {
                    divSetFQ28SPKto.Visible = false;
                }

                txtExtendtoFQ29.Text = SAPInitialItemSetupItem.ExtendtoFQ29;
                if (SAPInitialItemSetupItem.ExtendtoFQ29 == "Yes")
                {
                    divSetFQ29SPKto.Visible = true;
                    txtSetFQ29SPKto.Text = SAPInitialItemSetupItem.SetFQ29SPKto;
                }
                else
                {
                    divSetFQ29SPKto.Visible = false;
                }

                txtExtendtoFQ34.Text = SAPInitialItemSetupItem.ExtendtoFQ34;
                if (SAPInitialItemSetupItem.ExtendtoFQ34 == "Yes")
                {
                    divSetFQ34SPKto.Visible = true;
                    txtSetFQ34SPKto.Text = SAPInitialItemSetupItem.SetFQ34SPKto;
                }
                else
                {
                    divSetFQ34SPKto.Visible = false;
                }

                txtExtendtoFQ35.Text = SAPInitialItemSetupItem.ExtendtoFQ35;
                if (SAPInitialItemSetupItem.ExtendtoFQ35 == "Yes")
                {
                    divSetFQ35SPKto.Visible = true;
                    txtSetFQ35SPKto.Text = SAPInitialItemSetupItem.SetFQ35SPKto;
                }
                else
                {
                    divSetFQ35SPKto.Visible = false;
                }
            }
            #endregion
        }
        protected void rptCheckBoxes_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            CheckBox cb;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                cb = (CheckBox)e.Item.DataItem;
                CheckBox CheckBox1 = ((CheckBox)e.Item.FindControl("CheckBox1"));
                CheckBox1.Text = cb.Text;
                CheckBox1.CssClass = cb.ID + " bomTask";
                CheckBox1.Checked = cb.Checked;
            }
        }
        private List<CheckBox> checkboxSetup(ItemProposalItem itemProposalItem, SAPBOMSetupItem sapBomSetupItem)
        {
            var NewFG = false;
            bool NetworkMoveTransferSemi = false;
            bool newTransferSemi = false;
            bool NetworkMoveProjectType = false;
            bool NetworkMoveProjectTypeSubCategory = false;
            bool NetworkMoveProject = false;
            bool newPCSemi = false;
            string FGPackLocation = "";
            List<CheckBox> cbs = new List<CheckBox>();
            CheckBox checkbox = new CheckBox();

            FGPackLocation = itemProposalItem.PackingLocation;
            ExternalManufacturingItem externalMfgItem = externalMfgService.GetExternalManufacturingItem(iItemId);
            List<PackagingItem> packagingComponents = packagingService.GetAllPackagingItemsForProject(iItemId);

            List<PackagingItem> transferSemi =
               (
                   from packagingComponent in packagingComponents
                   where packagingComponent.PackagingComponent.ToLower().Contains("transfer")
                   select packagingComponent
               ).ToList();

            List<PackagingItem> purchasedSemi =
               (
                   from packagingComponent in packagingComponents
                   where packagingComponent.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi
                   select packagingComponent
               ).ToList();

            #region Set Flags
            #region NewFG
            if (itemProposalItem.TBDIndicator.ToLower() == "yes")
            {
                NewFG = true;
            }
            #endregion
            #region ProjectTypeNetworkMove
            if (itemProposalItem.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
            {
                NetworkMoveProjectType = true;
            }
            #endregion
            #region NetworkMoveProjectTypeSubCategory
            if (itemProposalItem.ProjectTypeSubCategory == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove)
            {
                NetworkMoveProjectTypeSubCategory = true;
            }
            #endregion
            #region ProjectTypeSubCategoryNetworkMove
            if (itemProposalItem.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove ||
                itemProposalItem.ProjectTypeSubCategory == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove ||
                itemProposalItem.MfgLocationChange == "Yes")
            {
                NetworkMoveProject = true;
            }
            #endregion
            #region Transfer Semi Flags
            if (transferSemi.Count > 0)
            {
                foreach (PackagingItem tsItem in transferSemi)
                {
                    if (tsItem.NewExisting.ToLower() == "network move")
                    {
                        NetworkMoveTransferSemi = true;
                    }
                    else
                    {
                        if (tsItem.NewExisting.ToLower() == "new")
                        {
                            newTransferSemi = true;
                        }
                    }
                }
            }
            #endregion
            #region purchasedSemi
            if (purchasedSemi.Count > 0)
            {
                foreach (PackagingItem psItem in purchasedSemi)
                {
                    if (psItem.NewExisting.ToLower() == "new")
                    {
                        newPCSemi = true;
                        break;
                    }
                }
            }
            #endregion

            #endregion

            #region Initial FG BOM Created
            if (NewFG)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkInitialFGBOMCreated";
                checkbox.Text = "Initial FG BOM Created";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.InitialFGBOMCreated) ? false : sapBomSetupItem.InitialFGBOMCreated.ToLower().Equals("yes");
                cbs.Add(checkbox);

            }
            #endregion
            #region Create empty Turnkey BOM at FC01
            //EmptyTurnkeyAtFC01
            try
            {
                string EmptyTurnkeyAtFC01PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "EmptyTurnkeyAtFC01", webUrl);

                List<string> EmptyTurnkeyAtFC01PackLocations = EmptyTurnkeyAtFC01PackLocationsString.Trim().ToLower().Split(',').ToList();

                if (EmptyTurnkeyAtFC01PackLocations.Contains(FGPackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower()))
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkEmptyTurnkeyAtFC01";
                    checkbox.Text = "Create empty Turnkey BOM at FC01";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.EmptyTurnkeyAtFC01) ? false : sapBomSetupItem.EmptyTurnkeyAtFC01.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend the Finished Good & associated  components to Sales Org 0001
            //ExtFGToSalesOrg0001
            try
            {
                var ExtFGToSalesOrg0001PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtFGToSalesOrg0001", webUrl);
                var ExtFGToSalesOrg0001DCLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter2", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtFGToSalesOrg0001", webUrl);

                var ExtFGToSalesOrg0001PackLocations = ExtFGToSalesOrg0001PackLocationsString.Trim().ToLower().Split(',').ToList();
                var ExtFGToSalesOrg0001DCLocations = ExtFGToSalesOrg0001DCLocationsString.Trim().ToLower().Split(',').ToList();

                //var ExtFGToSalesOrg0001FGs =
                //  (
                //      from packagingComponent in packagingComponents
                //      where packagingComponent.ParentID == 0 &&
                //      ExtFGToSalesOrg0001PackLocations.Contains(packagingComponent.PackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower())
                //      select packagingComponent
                //  ).ToList();

                if (ExtFGToSalesOrg0001PackLocations.Contains(FGPackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower()) || ExtFGToSalesOrg0001DCLocations.Contains(externalMfgItem.DesignateHUBDC.Split('-').ToList().FirstOrDefault().Trim().ToLower()))
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtFGToSalesOrg0001";
                    checkbox.Text = "Extend the Finished Good & associated  components to Sales Org 0001";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtFGToSalesOrg0001) ? false : sapBomSetupItem.ExtFGToSalesOrg0001.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend the Finished Good to Sales Org 1000
            //ExtFGToSalesOrg1000
            try
            {
                var ExtFGToSalesOrg1000DCLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtFGToSalesOrg1000", webUrl);

                var ExtFGToSalesOrg1000DCLocations = ExtFGToSalesOrg1000DCLocationsString.Trim().ToLower().Split(',').ToList();

                if (ExtFGToSalesOrg1000DCLocations.Contains(externalMfgItem.DesignateHUBDC.Split('-').ToList().FirstOrDefault().Trim().ToLower()))
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtFGToSalesOrg1000";
                    checkbox.Text = "Extend the Finished Good to Sales Org 1000";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtFGToSalesOrg1000) ? false : sapBomSetupItem.ExtFGToSalesOrg1000.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend the Finished Good to Sales Org 2000
            //ExtFGToSalesOrg2000
            try
            {
                var ExtFGToSalesOrg2000PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtFGToSalesOrg2000", webUrl);
                var ExtFGToSalesOrg2000DCLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter2", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtFGToSalesOrg2000", webUrl);

                var ExtFGToSalesOrg2000PackLocations = ExtFGToSalesOrg2000PackLocationsString.Trim().ToLower().Split(',').ToList();
                var ExtFGToSalesOrg2000DCLocations = ExtFGToSalesOrg2000DCLocationsString.Trim().ToLower().Split(',').ToList();

                if (ExtFGToSalesOrg2000PackLocations.Contains(FGPackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower()) ||
                    ExtFGToSalesOrg2000DCLocations.Contains(externalMfgItem.DesignateHUBDC.Split('-').ToList().FirstOrDefault().Trim().ToLower()))
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtFGToSalesOrg2000";
                    checkbox.Text = "Extend the Finished Good to Sales Org 2000";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtFGToSalesOrg2000) ? false : sapBomSetupItem.ExtFGToSalesOrg2000.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend the Finished Good associated components to Sales Org 2000
            //ExtFGToCompSale2000
            try
            {
                var ExtFGToCompSale2000PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtFGToCompSale2000", webUrl);

                var ExtFGToCompSale2000PackLocations = ExtFGToCompSale2000PackLocationsString.Trim().ToLower().Split(',').ToList();

                //var ExtFGToCompSale2000FGs =
                //  (
                //      from packagingComponent in packagingComponents
                //      where packagingComponent.ParentID == 0 &&
                //      ExtFGToCompSale2000PackLocations.Contains(packagingComponent.PackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower())
                //      select packagingComponent
                //  ).ToList();

                if (ExtFGToCompSale2000PackLocations.Contains(FGPackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower()))
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtFGToCompSale2000";
                    checkbox.Text = "Extend the Finished Good associated components to Sales Org 2000";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtFGToCompSale2000) ? false : sapBomSetupItem.ExtFGToCompSale2000.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region FG BOM created in new pack location (FG Network Move)
            if (NetworkMoveProject)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkFGBOMCreatedInNewPackLoc";
                checkbox.Text = "FG BOM created in new pack location (FG Network Move)";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.FGBOMCreatedInNewPackLoc) ? false : sapBomSetupItem.FGBOMCreatedInNewPackLoc.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Existing pack materials extended in new pack location for FG 
            if (NewFG || NetworkMoveProjectType || NetworkMoveProjectTypeSubCategory)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkPackMatsCreatedInPackLoc";
                checkbox.Text = "Existing pack materials extended in new pack location for FG";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.PackMatsCreatedInPackLoc) ? false : sapBomSetupItem.PackMatsCreatedInPackLoc.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Confirm SPKs updated per Deployment Information Section below
            checkbox = new CheckBox();
            checkbox.CssClass = "bomTask";
            checkbox.ID = "chkSPKsUpdatedPerDeployment";
            checkbox.Text = "Confirm SPKs updated per Deployment Information Section below";
            checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.SPKsUpdatedPerDeployment) ? false : sapBomSetupItem.SPKsUpdatedPerDeployment.ToLower().Equals("yes");
            cbs.Add(checkbox);
            #endregion
            #region Finished Good Subcon BOM Created
            if (NewFG && externalMfgItem.CoManufacturingClassification.ToLower() == "external subcon fg")
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkFGSubConBOMCreated";
                checkbox.Text = "Finished Good Subcon BOM Created";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.FGSubConBOMCreated) ? false : sapBomSetupItem.FGSubConBOMCreated.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Production Version Created
            if (NewFG && externalMfgItem.CoManufacturingClassification.ToLower() == "external subcon fg")
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkProdVersionCreated";
                checkbox.Text = "Production Version Created";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ProdVersionCreated) ? false : sapBomSetupItem.ProdVersionCreated.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Create new PUR CNDY SAP Material Number
            if (newPCSemi)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkCreateNewPURCNDYSAPMatNum";
                checkbox.Text = "Create new PUR CNDY SAP Material Number";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.CreateNewPURCNDYSAPMatNum) ? false : sapBomSetupItem.CreateNewPURCNDYSAPMatNum.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Create empty Turnkey PCS BOM at FC01
            //EmptyTurnkeyPCsAtFC01
            try
            {
                string EmptyTurnkeyPCsAtFC01PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "EmptyTurnkeyPCsAtFC01", webUrl);

                List<string> EmptyTurnkeyPCsAtFC01PackLocations = EmptyTurnkeyPCsAtFC01PackLocationsString.Trim().ToLower().Split(',').ToList();

                if (EmptyTurnkeyPCsAtFC01PackLocations.Contains(FGPackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower()))
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkEmptyTurnkeyPCsAtFC01";
                    checkbox.Text = "Create empty Turnkey BOM at FC01";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.EmptyTurnkeyPCsAtFC01) ? false : sapBomSetupItem.EmptyTurnkeyPCsAtFC01.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend Purchased Semi & all associated  components to Sales Org 0001
            //ExtPCToSalesOrg0001
            try
            {
                string ExtPCToSalesOrg0001PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtPCToSalesOrg0001", webUrl);

                List<string> ExtPCToSalesOrg0001PackLocations = ExtPCToSalesOrg0001PackLocationsString.Trim().ToLower().Split(',').ToList();


                List<PackagingItem> ExtPCToSalesOrg0001PurchasedSemis =
                  (
                      from packagingComponent in packagingComponents
                      where packagingComponent.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi
                      select packagingComponent
                  ).ToList();

                int DisplayExtPCToSalesOrg0001 = 0;
                foreach (var ExtPCToSalesOrg0001PurchasedSemi in ExtPCToSalesOrg0001PurchasedSemis)
                {
                    string ExtPCToSalesOrg0001PackLocation = ExtPCToSalesOrg0001PurchasedSemi.PackLocation;
                    int start = ExtPCToSalesOrg0001PackLocation.IndexOf("(") + 1;
                    int end = ExtPCToSalesOrg0001PackLocation.IndexOf(")", start);
                    string resultExtPCToSalesOrg0001PackLocation = ExtPCToSalesOrg0001PackLocation.Substring(start, end - start);
                    if (ExtPCToSalesOrg0001PackLocations.Contains(resultExtPCToSalesOrg0001PackLocation.ToLower()))
                    {
                        DisplayExtPCToSalesOrg0001++;
                    }
                }

                if (DisplayExtPCToSalesOrg0001 > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtPCToSalesOrg0001";
                    checkbox.Text = "Extend Purchased Semi & all associated  components to Sales Org 0001";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtPCToSalesOrg0001) ? false : sapBomSetupItem.ExtPCToSalesOrg0001.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend Purchased Semi & associated components to Sales Org 1000
            //ExtPCToSalesOrg1000
            try
            {
                var ExtPCToSalesOrg1000MakePackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtPCToSalesOrg1000", webUrl);

                var ExtPCToSalesOrg1000MakePackLocations = ExtPCToSalesOrg1000MakePackLocationsString.Trim().ToLower().Split(',').ToList();

                var ExtPCToSalesOrg1000PurchasedSemis =
                  (
                      from packagingComponent in packagingComponents
                      where packagingComponent.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi &&
                      ExtPCToSalesOrg1000MakePackLocations.Contains(packagingComponent.TransferSEMIMakePackLocations.ToLower())
                      select packagingComponent
                  ).ToList();

                if (ExtPCToSalesOrg1000PurchasedSemis.Count > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtPCToSalesOrg1000";
                    checkbox.Text = "Extend Purchased Semi & associated components to Sales Org 1000";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtPCToSalesOrg1000) ? false : sapBomSetupItem.ExtPCToSalesOrg1000.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Click into Component Details for New Transfer Semis and New Purchased Semis to obtain Hierarchy and Profit Center information
            //Should be visible if there is a new Transfer semi or Purchased Semi in the BOM
            if (newTransferSemi || newPCSemi)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkClckNewTSPCPrftCntr";
                checkbox.Text = "Click into Component Details for New Transfer Semis and New Purchased Semis to obtain Hierarchy and Profit Center information";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ClckNewTSPCPrftCntr) ? false : sapBomSetupItem.ClckNewTSPCPrftCntr.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Initial Transfer Semi BOM/BOMs created
            if (NetworkMoveTransferSemi || newTransferSemi)
            {
                CheckBox checkbox6 = new CheckBox();
                checkbox6.CssClass = "bomTask";
                checkbox6.ID = "chkInitialTSBOMCreated";
                checkbox6.Text = "Initial Transfer Semi BOM/BOMs created";
                checkbox6.Checked = string.IsNullOrEmpty(sapBomSetupItem.InitialTSBOMCreated) ? false : sapBomSetupItem.InitialTSBOMCreated.ToLower().Equals("yes");
                cbs.Add(checkbox6);
            }
            #endregion
            #region Transfer semi components created extended in new make/pack location (TS Network Move)
            if (NetworkMoveTransferSemi)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkTSCompsExtendedInNewMPLoc";
                checkbox.Text = "Transfer semi components extended in new make/pack location (TS Network Move)";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.TSCompsExtendedInNewMPLoc) ? false : sapBomSetupItem.TSCompsExtendedInNewMPLoc.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Transfer Semi FG BOM created in new make location (TS Network Move)
            if (NetworkMoveTransferSemi)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkTSCompsCreatedInNewMPLoc";
                checkbox.Text = "Transfer semi components created in new make location (TS Network Move)";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.TSCompsCreatedInNewMPLoc) ? false : sapBomSetupItem.TSCompsCreatedInNewMPLoc.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }
            #endregion
            #region Extend Transfer Semi & associated  components to Sales Org 0001
            //ExtTSToSalesOrg0001
            try
            {
                var ExtTSToSalesOrg0001PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtTSToSalesOrg0001", webUrl);

                var ExtTSToSalesOrg0001PackLocations = ExtTSToSalesOrg0001PackLocationsString.Trim().ToLower().Split(',').ToList();

                List<PackagingItem> ExtTSToSalesOrg0001TSs =
                  (
                      from packagingComponent in packagingComponents
                      where packagingComponent.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi &&
                      ExtTSToSalesOrg0001PackLocations.Contains(packagingComponent.PackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower())
                      select packagingComponent
                  ).ToList();

                if (ExtTSToSalesOrg0001TSs.Count > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtTSToSalesOrg0001";
                    checkbox.Text = "Extend Transfer Semi & associated  components to Sales Org 0001";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtTSToSalesOrg0001) ? false : sapBomSetupItem.ExtTSToSalesOrg0001.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend Transfer Semi & associated components to Sales Org 1000
            //ExtTSToSalesOrg1000
            try
            {
                string ExtTSToSalesOrg1000MakePackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtTSToSalesOrg1000", webUrl);

                var ExtTSToSalesOrg1000MakePackLocations = ExtTSToSalesOrg1000MakePackLocationsString.Trim().ToLower().Split(',').ToList();

                var ExtTSToSalesOrg1000TSs =
                  (
                      from packagingComponent in packagingComponents
                      where packagingComponent.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi &&
                      ExtTSToSalesOrg1000MakePackLocations.Contains(packagingComponent.TransferSEMIMakePackLocations.ToLower())
                      select packagingComponent
                  ).ToList();

                if (ExtTSToSalesOrg1000TSs.Count > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtTSToSalesOrg1000";
                    checkbox.Text = "Extend Transfer Semi & associated components to Sales Org 1000";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtTSToSalesOrg1000) ? false : sapBomSetupItem.ExtTSToSalesOrg1000.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend Transfer Semi & associated components to Sales Org 2000
            //ExtTSToSalesOrg2000
            try
            {
                var ExtTSToSalesOrg2000PackLocationsString = Utilities.GetLookupDetailsByValueAndColumn("Filter1", GlobalConstants.LIST_TaskCheckboxesLookup, "Title", "ExtTSToSalesOrg2000", webUrl);

                var ExtTSToSalesOrg2000PackLocations = ExtTSToSalesOrg2000PackLocationsString.Trim().ToLower().Split(',').ToList();

                var ExtTSToSalesOrg2000TSs =
                  (
                      from packagingComponent in packagingComponents
                      where packagingComponent.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi &&
                      ExtTSToSalesOrg2000PackLocations.Contains(packagingComponent.PackLocation.Split('-').ToList().FirstOrDefault().Trim().ToLower())
                      select packagingComponent
                  ).ToList();

                if (ExtTSToSalesOrg2000TSs.Count > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtTSToSalesOrg2000";
                    checkbox.Text = "Extend Transfer Semi & associated components to Sales Org 2000";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtTSToSalesOrg2000) ? false : sapBomSetupItem.ExtTSToSalesOrg2000.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
            }
            catch (Exception ex)
            {
            }
            #endregion
            #region Extend Profit Center to DC's
            //Should be visible if there is a New FG# or there is a new Transfer Semi or Purchased Semi in the BOM.
            //Should be visible if the Project Type is a Simple or Complex Network Move"

            if (NewFG || newTransferSemi || newPCSemi || NetworkMoveProjectType || NetworkMoveProjectTypeSubCategory)
            {
                checkbox = new CheckBox();
                checkbox.CssClass = "bomTask";
                checkbox.ID = "chkExtProfitCenterToDC";
                checkbox.Text = "Extend Profit Center to DC's";
                checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtProfitCenterToDC) ? false : sapBomSetupItem.ExtProfitCenterToDC.ToLower().Equals("yes");
                cbs.Add(checkbox);
            }

            #endregion
            #region Add ZSTO_MATERIALS entry
            checkbox = new CheckBox();
            checkbox.CssClass = "bomTask";
            checkbox.ID = "chkAddZSTOMatEntry";
            checkbox.Text = "Add ZSTO_MATERIALS entry";
            checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.AddZSTOMatEntry) ? false : sapBomSetupItem.AddZSTOMatEntry.ToLower().Equals("yes");
            cbs.Add(checkbox);
            #endregion
            return cbs;
        }
        private SAPInitialItemSetUp ConstructFormData()
        {
            SAPInitialItemSetUp sapInitialItemSetUp = new SAPInitialItemSetUp();

            sapInitialItemSetUp.SAPItemNumber = txtSAPItem.Text;
            sapInitialItemSetUp.SAPDescription = txtSAPDescription.Text;
            sapInitialItemSetUp.UnitUPC = txtUnitUPC.Text;
            sapInitialItemSetUp.DisplayBoxUPC = txtDisplayUPC.Text;

            sapInitialItemSetUp.CaseUCC = txtCaseUCC.Text;
            sapInitialItemSetUp.PalletUCC = txtPalletUCC.Text;
            sapInitialItemSetUp.CompassListItemId = iItemId;
            sapInitialItemSetUp.NewTransferSemi = GlobalConstants.CONST_No;

            // hdnProjectType.Value = opsItem.ProjectType;
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            if (iItemId > 0)
            {
                dtPackingItem = packagingService.GetNewTransferSemiItemsForProject(iItemId);
                if (dtPackingItem.Count > 0)
                {
                    sapInitialItemSetUp.NewTransferSemi = GlobalConstants.CONST_Yes;
                }
            }

            return sapInitialItemSetUp;
        }
        private SAPBOMSetupItem ConstructProjectDecisions()
        {
            var item = new SAPBOMSetupItem();
            item.CompassListItemId = iItemId;
            item.LastUpdatedFormName = CompassForm.SAPBOMSetup.ToString();
            foreach (RepeaterItem cbs in rptCheckBoxes.Items)
            {
                if (cbs.ItemType == ListItemType.Item || cbs.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox cb = ((CheckBox)cbs.FindControl("Checkbox1"));
                    string cbID = cb.CssClass.Replace(" bomTask", "");
                    cbID = cbID.Replace("chk", "");
                    string value = cb.Checked ? "Yes" : "No";
                    item.GetType().GetProperty(cbID).SetValue(item, value);
                }

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
        #endregion

        #region Button Methods  
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "SAP Initial Item Setup Form");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            string sapNumber = string.Empty;
            if (!string.IsNullOrEmpty(txtSAPItem.Text))
            {
                sapNumber = txtSAPItem.Text.Trim();
            }
            else
            {
                ErrorSummary.AddError("Please enter a valid SAP number!", this.Page);
                return;
            }

            SetLookupValues(sapNumber, true);
        }
        public void GetuserControls()
        {
            List<BOMSetupItem> packMeasitems = new List<BOMSetupItem>();
            List<BOMSetupItem> TSMakePackLocationsitems = new List<BOMSetupItem>();

            foreach (var ctrl in phBOM.Controls)
            {
                if (ctrl is ucBOMGridPackMeas_New)
                {
                    ucBOMGridPackMeas_New type = (ucBOMGridPackMeas_New)ctrl;
                    packMeasitems.Add(ConstructMakePackFormData(type));
                }
            }

            List<int> deletedIds = Utilities.GetIntegerArrayFromDelimittedString(hdnDeletedCompIds.Value, ';');

            if (deletedIds.Count > 0)
            {
                BOMSetupService.DeleteBOMSetupItems(deletedIds);
                hdnDeletedCompIds.Value = "";
            }

            BOMSetupService.UpsertPackMeasurementsItem(packMeasitems, ProjectNumber);
        }

        private BOMSetupItem ConstructMakePackFormData(ucBOMGridPackMeas_New ctrl)
        {
            BOMSetupItem bomsetupitem = new BOMSetupItem();
            try
            {

                bomsetupitem.CompassListItemId = iItemId;
                bomsetupitem.ParentID = Convert.ToInt32(((HiddenField)ctrl.FindControl("hdnParentComponentId")).Value);
                bomsetupitem.PalletSpecNumber = ((TextBox)ctrl.FindControl("txtPalletSpecNumber")).Text;
                bomsetupitem.PackSpecNumber = ((TextBox)ctrl.FindControl("txtFGPackSpecNumber")).Text;
                bomsetupitem.PalletSpecLink = ((TextBox)ctrl.FindControl("txPalletPatternLink")).Text;
                bomsetupitem.NotesSpec = ((TextBox)ctrl.FindControl("txtSpecNotes")).Text;
                bomsetupitem.SAPSpecsChange = ((DropDownList)ctrl.FindControl("drpSAPSpecsChange")).SelectedItem.Text;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "ConstructMakePackFormData");
            }
            return bomsetupitem;
        }
        public void openBtnSave()
        {
            try
            {
                GetuserControls();
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPInitialSetup))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                SAPInitialItemSetUp item = ConstructFormData();
                sapInitialISetUpService.UpdateSAPInitialSetupItem(item, CompassForm.SAPInitialSetup.ToString());

                SAPBOMSetupItem sapbomsetupItem = ConstructProjectDecisions();
                sapBOMSetupService.UpdateSAPBOMSetupItemFromInitial(sapbomsetupItem);

                ApprovalItem approvalItem = ConstructApprovalData();
                sapInitialISetUpService.UpdateSAPInitialSetupApprovalItem(approvalItem, false);
                //LoadUserControls();
                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "SAP Initial Item SetUp" + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "SAP Initial Item SetUp", "btnSave_Click");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            openBtnSave();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPInitialSetup))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                SAPInitialItemSetUp item = ConstructFormData();
                sapInitialISetUpService.UpdateSAPInitialSetupItem(item, CompassForm.SAPInitialSetup.ToString());

                ApprovalItem approvalItem = ConstructApprovalData();
                sapInitialISetUpService.UpdateSAPInitialSetupApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.SAPInitialSetup);

                // Redirect to Home page after successfull Submit                        
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "SAP Initial Item SetUp" + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "SAP Initial Item SetUp", "btnSubmit_Click");
            }
        }

        #endregion
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
    }
}
