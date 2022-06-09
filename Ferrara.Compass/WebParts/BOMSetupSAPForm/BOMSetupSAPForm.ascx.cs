using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using Ferrara.Compass.Services;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.BOMSetupSAPForm
{
    [ToolboxItemAttribute(false)]
    public partial class BOMSetupSAPForm : WebPart
    {
        #region Member Variables
        private IBOMSetupService BOMSetupService;
        private ISAPBOMSetupService sapBOMSetupService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private IItemProposalService itemProposalService;
        private IExternalManufacturingService externalMfgService;
        private IQAService qaService;
        private int iItemId = 0;
        private string webUrl;
        private const string _ucBOMEditable_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx";
        private const string _ucBOMGridPackMeas_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGridPackMeas_New.ascx";
        private IPackagingItemService packagingItemService;
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
        public BOMSetupSAPForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<BOMSetupService>();
            sapBOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPBOMSetupService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            itemProposalService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
            externalMfgService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();
            qaService = DependencyResolution.DependencyMapper.Container.Resolve<IQAService>();
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

                    LoadFormData();
                    bindCandySemiData(false);
                    InitializeScreen();
                    List<CheckBox> checkBoxBinding = checkboxSetup();
                    rptCheckBoxes.DataSource = checkBoxBinding;
                    rptCheckBoxes.DataBind();

                    rptSPKDets.DataSource = sapBOMSetupService.getTSSPKDetails(iItemId);
                    rptSPKDets.DataBind();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPBOMSetup.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.SAPBOMSetup.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
            LoadUserControls();
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
            if (!userMgmtService.HasReadAccess(CompassForm.SAPBOMSetup))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.SAPBOMSetup))
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
            SAPBOMSetupItem item;

            hdnPageName.Value = GlobalConstants.PAGE_SAPBOMSetup;
            hdnCompassListItemId.Value = iItemId.ToString();
            item = sapBOMSetupService.GetSAPBOMSetupItem(iItemId);
            lblProductHierarchy1.Text = item.ProductHierarchyLevel1;
            lblProductHierarchy2.Text = item.ProductHierarchyLevel2;
            lblMaterialGroup1Brand.Text = item.MaterialGroup1Brand;
            lblMaterialGroup2Pricing.Text = item.MaterialGroup2Pricing;
            lblMaterialGroup4ProductForm.Text = item.MaterialGroup4ProductForm;
            lblMaterialGroup5PackType.Text = item.MaterialGroup5PackType;
            lblProfitCenter.Text = item.ProfitCenter;

            // Check for External Manufacturing
            if ((string.Equals(item.PackingLocation, GlobalConstants.EXTERNAL_PACKER)) ||
                (string.Equals(item.MakeLocation, GlobalConstants.EXTERNAL_MANUFACTURER)))
            {
                lblCoManufacturingClassification.Text = item.ProcurementType;
            }
            else
            {
                dvProcurementType.Visible = false;
            }

            if (string.IsNullOrEmpty(item.MakeLocation) || string.Equals(item.MakeLocation, "Select..."))
            {
                divMakeLocation.Visible = false;
            }
            else
            {
                divMakeLocation.Visible = true;
                lblMakeLocation.Text = item.MakeLocation;
            }

            if (string.IsNullOrEmpty(item.PackingLocation) || string.Equals(item.PackingLocation, "Select..."))
            {
                divPrimaryPackLocation.Visible = false;
            }
            else
            {
                divPrimaryPackLocation.Visible = true;
                lblPrimaryPackLocation.Text = item.PackingLocation;
            }

            if (!string.IsNullOrEmpty(item.ExternalManufacturer) && !string.Equals(item.ExternalManufacturer, "Select..."))
            {
                dvCoManLocation.Visible = true;
                lblExternalManufacturer.Text = item.ExternalManufacturer;
            }
            else
            {
                dvCoManLocation.Visible = false;
            }

            if (!string.IsNullOrEmpty(item.ExternalPacker) && !string.Equals(item.ExternalManufacturer, "Select..."))
            {
                dvPackLocation.Visible = true;
                lblExternalPacker.Text = item.ExternalPacker;
            }
            else
            {
                dvPackLocation.Visible = false;
            }

            #region Deployment Information
            txtDesignateHUBDC.Text = item.DesignateHUBDC;
            txtDeploymentModeofItem.Text = item.DeploymentModeofItem;
            LoadControlsByCompanyCode(item);
            #endregion
            if (string.IsNullOrEmpty(item.PurchasedIntoLocation) || string.Equals(item.PurchasedIntoLocation, "Select..."))
            {
                dvPurchasedIntoCenter.Visible = false;
            }
            else
            {
                dvPurchasedIntoCenter.Visible = true;
                lblPurchasedIntoCenter.Text = item.PurchasedIntoLocation;
            }

            lblUnitUPC.Text = item.UnitUPC;
            if (string.IsNullOrEmpty(item.DisplayBoxUPC))
            {
                divDisplayUPC.Visible = false;
            }
            else
            {
                divDisplayUPC.Visible = true;
                lblDisplayUPC.Text = item.DisplayBoxUPC;
            }

            lblCaseUCC.Text = item.CaseUCC;
            lblPalletUCC.Text = item.PalletUCC;
            lblSAPBaseUOM.Text = item.SAPBaseUOM;
            hdnMaterialDesc.Value = item.SAPDescription;
            hdnMaterialNumber.Value = item.SAPItemNumber;
            hdnProjectType.Value = item.ProjectType;
        }
        private void LoadControlsByCompanyCode(SAPBOMSetupItem SAPBOMSetupItem)
        {
            //divFPCODCs.Visible = false;
            divSELLDCs.Visible = false;
            DivFERQDCs.Visible = false;

            if (SAPBOMSetupItem.ProjectType != GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
            {
                divDeploymentModeofItem.Visible = false;
                return;
            }

            #region SellDCs
            if (SAPBOMSetupItem.ProductHierarchyLevel1 != GlobalConstants.PRODUCT_HIERARCHY1_LBB)
            {
                divSELLDCs.Visible = true;
                DivFERQDCs.Visible = false;

                txtExtendtoSL07.Text = SAPBOMSetupItem.ExtendtoSL07;
                if (SAPBOMSetupItem.ExtendtoSL07 == "Yes")
                {
                    divSetSL07SPKto.Visible = true;
                    txtSetSL07SPKto.Text = SAPBOMSetupItem.SetSL07SPKto;
                }
                else
                {
                    divSetSL07SPKto.Visible = false;
                }

                txtExtendtoSL13.Text = SAPBOMSetupItem.ExtendtoSL13;
                if (SAPBOMSetupItem.ExtendtoSL13 == "Yes")
                {
                    divSetSL13SPKto.Visible = true;
                    txtSetSL13SPKto.Text = SAPBOMSetupItem.SetSL13SPKto;
                }
                else
                {
                    divSetSL13SPKto.Visible = false;
                }

                txtExtendtoSL18.Text = SAPBOMSetupItem.ExtendtoSL18;
                if (SAPBOMSetupItem.ExtendtoSL18 == "Yes")
                {
                    divSetSL18SPKto.Visible = true;
                    txtSetSL18SPKto.Text = SAPBOMSetupItem.SetSL18SPKto;
                }
                else
                {
                    divSetSL18SPKto.Visible = false;
                }

                txtExtendtoSL19.Text = SAPBOMSetupItem.ExtendtoSL19;
                if (SAPBOMSetupItem.ExtendtoSL19 == "Yes")
                {
                    divSetSL19SPKto.Visible = true;
                    txtSetSL19SPKto.Text = SAPBOMSetupItem.SetSL19SPKto;
                }
                else
                {
                    divSetSL19SPKto.Visible = false;
                }

                txtExtendtoSL30.Text = SAPBOMSetupItem.ExtendtoSL30;
                if (SAPBOMSetupItem.ExtendtoSL30 == "Yes")
                {
                    divSetSL30SPKto.Visible = true;
                    txtSetSL30SPKto.Text = SAPBOMSetupItem.SetSL30SPKto;
                }
                else
                {
                    divSetSL30SPKto.Visible = false;
                }

                txtExtendtoSL14.Text = SAPBOMSetupItem.ExtendtoSL14;
                if (SAPBOMSetupItem.ExtendtoSL14 == "Yes")
                {
                    divSetSL14SPKto.Visible = true;
                    txtSetSL14SPKto.Text = SAPBOMSetupItem.SetSL14SPKto;
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

                txtExtendtoFQ26.Text = SAPBOMSetupItem.ExtendtoFQ26;
                if (SAPBOMSetupItem.ExtendtoFQ26 == "Yes")
                {
                    divSetFQ26SPKto.Visible = true;
                    txtSetFQ26SPKto.Text = SAPBOMSetupItem.SetFQ26SPKto;
                }
                else
                {
                    divSetFQ26SPKto.Visible = false;
                }

                txtExtendtoFQ27.Text = SAPBOMSetupItem.ExtendtoFQ27;
                if (SAPBOMSetupItem.ExtendtoFQ27 == "Yes")
                {
                    divSetFQ27SPKto.Visible = true;
                    txtSetFQ27SPKto.Text = SAPBOMSetupItem.SetFQ27SPKto;
                }
                else
                {
                    divSetFQ27SPKto.Visible = false;
                }

                txtExtendtoFQ28.Text = SAPBOMSetupItem.ExtendtoFQ28;
                if (SAPBOMSetupItem.ExtendtoFQ28 == "Yes")
                {
                    divSetFQ28SPKto.Visible = true;
                    txtSetFQ28SPKto.Text = SAPBOMSetupItem.SetFQ28SPKto;
                }
                else
                {
                    divSetFQ28SPKto.Visible = false;
                }

                txtExtendtoFQ29.Text = SAPBOMSetupItem.ExtendtoFQ29;
                if (SAPBOMSetupItem.ExtendtoFQ29 == "Yes")
                {
                    divSetFQ29SPKto.Visible = true;
                    txtSetFQ29SPKto.Text = SAPBOMSetupItem.SetFQ29SPKto;
                }
                else
                {
                    divSetFQ29SPKto.Visible = false;
                }

                txtExtendtoFQ34.Text = SAPBOMSetupItem.ExtendtoFQ34;
                if (SAPBOMSetupItem.ExtendtoFQ34 == "Yes")
                {
                    divSetFQ34SPKto.Visible = true;
                    txtSetFQ34SPKto.Text = SAPBOMSetupItem.SetFQ34SPKto;
                }
                else
                {
                    divSetFQ34SPKto.Visible = false;
                }

                txtExtendtoFQ35.Text = SAPBOMSetupItem.ExtendtoFQ35;
                if (SAPBOMSetupItem.ExtendtoFQ35 == "Yes")
                {
                    divSetFQ35SPKto.Visible = true;
                    txtSetFQ35SPKto.Text = SAPBOMSetupItem.SetFQ35SPKto;
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
        protected void rptSPKDets_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                string labelText = (string)e.Item.DataItem;
                Label lblTSSPKStatus = ((Label)e.Item.FindControl("lblTSSPKStatus"));
                lblTSSPKStatus.Text = labelText;
            }
        }
        private List<CheckBox> checkboxSetup()
        {

            SAPBOMSetupItem sapBomSetupItem = BOMSetupService.GetSAPBOMSetupItem(iItemId);
            List<PackagingItem> packagingComponents = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            #region Set Flags
            var NewFG = false;
            bool NetworkMoveTransferSemi = false;
            bool newTransferSemi = false;
            bool NetworkMoveProjectType = false;
            bool NetworkMoveProjectTypeSubCategory = false;
            bool NetworkMoveProject = false;
            bool newPCSemi = false;
            List<PackagingItem> NewTransferSemis = new List<PackagingItem>();
            List<PackagingItem> NetworkMoveTransferSemis = new List<PackagingItem>();
            #region NewFG
            if (sapBomSetupItem.TBDIndicator.ToLower() == "yes")
            {
                NewFG = true;
            }
            #endregion
            #region ProjectTypeNetworkMove
            if (sapBomSetupItem.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
            {
                NetworkMoveProjectType = true;
            }
            #endregion
            #region NetworkMoveProjectTypeSubCategory
            if (sapBomSetupItem.ProjectTypeSubCategory == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove)
            {
                NetworkMoveProjectTypeSubCategory = true;
            }
            #endregion
            #region ProjectTypeSubCategoryNetworkMove
            if (sapBomSetupItem.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove ||
                sapBomSetupItem.ProjectTypeSubCategory == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove ||
                sapBomSetupItem.MfgLocationChange == "Yes")
            {
                NetworkMoveProject = true;
            }
            #endregion
            #region Transfer Semi Flags
            NewTransferSemis =
                (
                    from packagingComponent in packagingComponents
                    where packagingComponent.PackagingComponent.ToLower().Contains("transfer") && packagingComponent.NewExisting.ToLower() == "new"
                    select packagingComponent
                ).ToList();

            NetworkMoveTransferSemis =
                (
                    from packagingComponent in packagingComponents
                    where packagingComponent.PackagingComponent.ToLower().Contains("transfer") && packagingComponent.NewExisting.ToLower() == "network move"
                    select packagingComponent
                ).ToList();

            if (NewTransferSemis.Count > 0)
            {
                newTransferSemi = true;
            }

            if (NetworkMoveTransferSemis.Count > 0)
            {
                NetworkMoveTransferSemi = true;
            }
            #endregion
            #endregion
            #region Set Form Fields
            txtProjectType.Text = sapBomSetupItem.ProjectType;
            txtProjectTypeSubCategory.Text = sapBomSetupItem.ProjectTypeSubCategory;
            txtItemConcept.Text = sapBomSetupItem.ItemConcept;
            if (sapBomSetupItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblItemConcept.Text = "Change Notes:";
            }
            else
            {
                lblItemConcept.Text = "Item Concept:";
            }

            lblImmediateSPKChange.Text = "Immediate SPK Change: " + sapBomSetupItem.ImmediateSPKChange;
            #endregion
            List<CheckBox> cbs = new List<CheckBox>();
            try
            {
                CheckBox checkbox = new CheckBox();
                #region Continue building FG BOM. Add new packaging components to the BOM. 
                if (NewFG)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkContBuildFGBOM";
                    checkbox.Text = "Continue building FG BOM. Add new packaging components to the BOM";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ContBuildFGBOM) ? false : sapBomSetupItem.ContBuildFGBOM.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
                #region New FG Packaging #s Created
                int NewFgComponent = (from PIs in packagingComponents where PIs.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && PIs.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi && PIs.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && PIs.NewExisting == "New" && PIs.ParentID == 0 select PIs).Count();
                if (NewFgComponent > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkNewMaterialNumbersCreated";
                    checkbox.Text = "New FG Packaging #s created";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.NewMaterialNumbersCreated) ? false : sapBomSetupItem.NewMaterialNumbersCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
                #region Use GS1 Calculator to Generate 14 Digit Code for New Corrugate Material in CCC Transfer Semis
                //Should be visible if there is a new corrugate material within a Transfer Semi that has a pack location of FQ21, FQ22, FQ23, FQ24, or FQ25
                var GS1CalculatorCheckBoxVisible = false;
                var TransferSemis =
                    (
                    from packgingItem in packagingComponents
                    where packgingItem.PackagingComponent.ToLower().Contains("transfer") && (packgingItem.PackLocation.Contains("FQ22") || packgingItem.PackLocation.Contains("FQ25"))
                    select packgingItem).ToList();

                foreach (var TransferSemi in TransferSemis)
                {
                    var Corrugates =
                        (
                            from packgingItem in packagingComponents
                            where packgingItem.PackagingComponent.ToLower().Contains("corrugated") && packgingItem.ParentID == TransferSemi.Id && packgingItem.NewExisting.ToLower() == "new"
                            select packgingItem
                        ).ToList();

                    if (Corrugates.Count > 0)
                    {
                        GS1CalculatorCheckBoxVisible = true;
                        break;
                    }
                }

                if (GS1CalculatorCheckBoxVisible)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkGS1Calculator";
                    checkbox.Text = "Use GS1 Calculator to Generate 14 Digit Code for New Corrugate Material in CCC Transfer Semis";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.GS1Calculator) ? false : sapBomSetupItem.GS1Calculator.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
                #region New Transfer Semi Component/Packaging Material Numbers created (if there are new components within the transfer semi)

                var AllTransferSemis =
                 (
                     from packagingComponent in packagingComponents
                     where packagingComponent.PackagingComponent.ToLower().Contains("transfer")
                     select packagingComponent
                 ).ToList();

                foreach (var TransferSemi in AllTransferSemis)
                {
                    var NewChildOfTransferSemi =
                        (
                            from packagingComponent in packagingComponents
                            where packagingComponent.ParentID == TransferSemi.Id && packagingComponent.NewExisting.ToLower() == "new"
                            select packagingComponent
                        ).ToList();

                    if (NewChildOfTransferSemi.Count > 0)
                    {
                        CheckBox checkbox3 = new CheckBox();
                        checkbox3.CssClass = "bomTask";
                        checkbox3.ID = "chkNewTSCompPackNumsCreated";
                        checkbox3.Text = "New Transfer Semi Component/Packaging Material Numbers created";
                        checkbox3.Checked = string.IsNullOrEmpty(sapBomSetupItem.NewTSCompPackNumsCreated) ? false : sapBomSetupItem.NewTSCompPackNumsCreated.ToLower().Equals("yes");
                        cbs.Add(checkbox3);
                        break;
                    }
                }
                #endregion
                #region Existing pack materials extended in new pack location for FG 
                if (NetworkMoveProjectType || NetworkMoveProjectTypeSubCategory)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkPackMatsCreatedInPackLoc";
                    checkbox.Text = "Existing pack materials extended in new pack location for FG";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.PackMatsCreatedInPackLoc) ? false : sapBomSetupItem.PackMatsCreatedInPackLoc.ToLower().Equals("yes");
                    cbs.Add(checkbox);
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
                #region Extend Profit Center to DC's
                //Should be visible if there is a New FG# or there is a new Transfer Semi or Purchased Semi in the BOM.
                //Should be visible if the Project Type is a Simple or Complex Network Move"

                if (NetworkMoveProjectType || NetworkMoveProjectTypeSubCategory)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtProfitCenterToDC";
                    checkbox.Text = "Extend Profit Center to DC's";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtProfitCenterToDC) ? false : sapBomSetupItem.ExtProfitCenterToDC.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }

                #endregion

                if (NetworkMoveTransferSemi)
                {
                    #region Initial Transfer Semi BOM/BOMs created
                    CheckBox checkbox6 = new CheckBox();
                    checkbox6.CssClass = "bomTask";
                    checkbox6.ID = "chkInitialTSBOMCreated";
                    checkbox6.Text = "Initial Transfer Semi BOM/BOMs created";
                    checkbox6.Checked = string.IsNullOrEmpty(sapBomSetupItem.InitialTSBOMCreated) ? false : sapBomSetupItem.InitialTSBOMCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox6);
                    #endregion
                    #region Transfer semi components created extended in new make/pack location (TS Network Move)
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkTSCompsExtendedInNewMPLoc";
                    checkbox.Text = "Transfer semi components extended in new make/pack location (TS Network Move)";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.TSCompsExtendedInNewMPLoc) ? false : sapBomSetupItem.TSCompsExtendedInNewMPLoc.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                    #endregion
                    #region Transfer Semi FG BOM created in new make location (TS Network Move)
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkTSCompsCreatedInNewMPLoc";
                    checkbox.Text = "Transfer semi components created in new make location (TS Network Move)";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.TSCompsCreatedInNewMPLoc) ? false : sapBomSetupItem.TSCompsCreatedInNewMPLoc.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                    #endregion
                }
                #region Extend the Finished Good Hierarchy 1,2, and Brand to the new packaging components within the FG BOM
                //Should be visible if there are new packaging components in the FG BOM
                int NewFGPackComponents = (
                        from PIs in packagingComponents
                        where
                            PIs.NewExisting == "New" &&
                            PIs.ParentID == 0
                        select PIs
                    ).Count();
                if (NewFgComponent > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkExtendFGHL12Brand";
                    checkbox.Text = "Extend the Finished Good Hierarchy 1,2, and Brand to the new packaging components within the FG BOM";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ExtendFGHL12Brand) ? false : sapBomSetupItem.ExtendFGHL12Brand.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
                #region  Apply the Transfer Semi and/or Purchased Semi hierarchy 1,2, brand and profit center to new components within Semi BOM
                //Should be visible if there are any new materials in a Transfer semi or Purchased Semi BOM
                int NewSemiComponents = (
                            from PIs in packagingComponents
                            where
                                PIs.NewExisting == "New" &&
                                PIs.ParentID != 0
                            select PIs
                            ).Count();

                if (NewSemiComponents > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkApplySemiHL12Brand";
                    checkbox.Text = "Apply the Transfer Semi and/or Purchased Semi hierarchy 1,2, brand and profit center to new components within Semi BOM";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.ApplySemiHL12Brand) ? false : sapBomSetupItem.ApplySemiHL12Brand.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
                #region  Add “Old Component” number to BOM for materials where “Soft” has been indicated for flowthrough status”
                //Should be visible if there is a new FG # or Transfer semi # 
                //AND there is a new Material Component in the BOM AND the new material flowthrough for that component is marked as “soft”
                int NewSoftFlowThroughComponents = (
                            from PIs in packagingComponents
                            where
                                PIs.NewExisting == "New" &&
                                PIs.Flowthrough.ToLower() == "soft"
                            select PIs
                            ).Count();

                if ((NewFG || NewTransferSemis.Count > 0) && NewSoftFlowThroughComponents > 0)
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkAddOldComp";
                    checkbox.Text = "Add “Old Component” number to BOM for materials where “Soft” has been indicated for flowthrough status”";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.AddOldComp) ? false : sapBomSetupItem.AddOldComp.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
            }
            catch (Exception exception)
            {
                string ids = "";
                foreach (CheckBox box in cbs)
                {
                    ids = ids + box.ID + ";";
                }
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupSAP.ToString() + ": " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BOMSetupSAP.ToString(), "checkboxSetup" + ids);
            }
            return cbs;
        }
        private void LoadUserControls()
        {
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();

            phBOMGrid.Controls.Clear();
            dtPackingItem = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);
            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();

            ucBOMGridPackMeas_New ctrl = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
            BOMSetupItem FGItem = new BOMSetupItem()
            {
                PackagingComponent = "Finished Good",
                MaterialNumber = hdnMaterialNumber.Value,
                MaterialDescription = hdnMaterialDesc.Value,
                ParentID = 0
            };
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.packagingItems = dtPackingItem;
            ctrl.ParentId = 0;
            ctrl.GridItem = FGItem;
            ctrl.iItemId = iItemId;
            ctrl.openBtnSave = openBtnSave;
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.NewComponentCount = newItemCount;
            phBOMGrid.Controls.Add(ctrl);
            foreach (BOMSetupItem item in dtPackingItem)
            {
                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBOMGridPackMeas_New ctrl2 = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
                    ctrl2.GridItem = item;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.iItemId = iItemId;
                    ctrl2.openBtnSave = openBtnSave;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.NewExisting = item.NewExisting;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOMGrid.Controls.Add(ctrl2);
                }
            }

            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable_New)Page.LoadControl(_ucBOMEditable_New);
                ctrlPE.PackagingComponent = hdnComponentype.Value;
                ctrlPE.PackagingItemId = Convert.ToInt32(hdnPackagingID.Value);
                ctrlPE.ParentID = hdnParentID.Value;
                ctrlPE.AllPIs = dtPackingItem;
                ctrlPE.CompassListItemId = iItemId;
                // Add messages to page
                phBOMEdits.Controls.Add(ctrlPE);
            }

        }
        public void GetuserControls()
        {
            List<BOMSetupItem> packMeasitems = new List<BOMSetupItem>();
            List<BOMSetupItem> TSMakePackLocationsitems = new List<BOMSetupItem>();

            foreach (var ctrl in phBOMGrid.Controls)
            {
                if (ctrl is ucBOMGridPackMeas_New)
                {
                    var type = (ucBOMGridPackMeas_New)ctrl;
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
        public void bindCandySemiData(bool extraRow)
        {
            var candylst = packagingItemService.GetCandyAndPurchasedSemisForProject(iItemId);
            if (extraRow)
            {
                PackagingItem blankItem = new PackagingItem();
                blankItem.Allergens = "";
                blankItem.NewExisting = "";
                blankItem.PackagingComponent = "";
                blankItem.NewFormula = "";
                blankItem.MaterialNumber = "";
                blankItem.MaterialDescription = "";
                blankItem.TransferSEMIMakePackLocations = "";
                blankItem.TrialsCompleted = "";
                blankItem.ShelfLife = "";
                blankItem.Kosher = "";

                int newId = packagingItemService.InsertPackagingItem(blankItem, iItemId);
                blankItem.Id = newId;
                candylst.Add(blankItem);
            }
            divNoCandySemi.Visible = candylst.Count > 0 ? false : true;
            rptCandy.DataSource = candylst;
            rptCandy.DataBind();
        }
        private SAPBOMSetupItem ConstructFormData()
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
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "SAP BOM Setup");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        public void openBtnSave()
        {
            GetuserControls();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPBOMSetup))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                SAPBOMSetupItem item = ConstructFormData();
                BOMSetupService.UpdateSAPBOMSetupItem(item);

                GetuserControls();

                ApprovalItem approvalItem = ConstructApprovalData();
                sapBOMSetupService.UpdateSAPBOMSetupApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupSAP.ToString() + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BOMSetupSAP.ToString(), "btnSave_Click");
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
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPBOMSetup))
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                var iserror = RequiredFieldCheckForPackagingComponent();

                if (iserror)
                {
                    ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Submit Failed:</p></strong><br/>";
                    return;
                }
                // Retrieve the data from the form
                SAPBOMSetupItem item = ConstructFormData();
                sapBOMSetupService.UpdateSAPBOMSetupItem(item);

                GetuserControls();

                ApprovalItem approvalItem = ConstructApprovalData();
                sapBOMSetupService.UpdateSAPBOMSetupApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.SAPBOMSetup);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupSAP.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupSAP.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
    }
}