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
using System.Collections.Generic;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Web.UI.WebControls;
using System.Linq;

namespace Ferrara.Compass.WebParts.SAPBOMSetupForm
{
    [ToolboxItemAttribute(false)]
    public partial class SAPBOMSetupForm : WebPart
    {
        #region Member Variables
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
        private const string _ucBOMPackMeas = @"~/_controltemplates/15/Ferrara.Compass/ucBOMPackMeas.ascx";
        private const string _ucBOMEditable = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx";
        private const string _ucBOMGrid = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx";
        private IPackagingItemService packagingItemService;
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
        public SAPBOMSetupForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
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
            PLMProject = item.PLMProject;
            hdnPLMProject.Value = item.PLMProject;
            lblProductHierarchy1.Text = item.ProductHierarchyLevel1;
            lblProductHierarchy2.Text = item.ProductHierarchyLevel2;
            lblMaterialGroup1Brand.Text = item.MaterialGroup1Brand;
            lblMaterialGroup2Pricing.Text = item.MaterialGroup2Pricing;
            lblMaterialGroup4ProductForm.Text = item.MaterialGroup4ProductForm;
            lblMaterialGroup5PackType.Text = item.MaterialGroup5PackType;
            lblMakeLocation.Text = item.MakeLocation;

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
            lblPrimaryPackLocation.Text = item.PackingLocation;
            lblDesignateHUBDC.Text = item.DesignateHUBDC;
            lblPurchasedIntoCenter.Text = item.PurchasedIntoLocation;
            lblUnitUPC.Text = item.UnitUPC;
            lblDisplayUPC.Text = item.DisplayBoxUPC;
            lblCaseUCC.Text = item.CaseUCC;
            lblPalletUCC.Text = item.PalletUCC;
            lblSAPBaseUOM.Text = item.SAPBaseUOM;
            lblDoubleStackable.Text = item.DoubleStackable;
            hdnMaterialDesc.Value = item.SAPDescription;
            hdnMaterialNumber.Value = item.SAPItemNumber;
            hdnProjectType.Value = item.ProjectType;

            if (string.IsNullOrEmpty(lblPurchasedIntoCenter.Text) || string.Equals(lblPurchasedIntoCenter.Text, "Select..."))
            {
                dvPurchasedIntoCenter.Visible = false;
            }
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

            SAPBOMSetupItem sapBomSetupItem = sapBOMSetupService.GetSAPBOMSetupItem(iItemId);
            ItemProposalItem itemProposalItem = itemProposalService.GetItemProposalItem(iItemId);
            List<PackagingItem> packagingComponents = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            txtProjectType.Text = itemProposalItem.ProjectType;
            txtProjectTypeSubCategory.Text = itemProposalItem.ProjectTypeSubCategory;
            //lblHardSoftTransition.Visible = itemProposalItem.ProjectType.Equals(GlobalConstants.PROJECTTYPE_DownweightTransition) ||
            //    itemProposalItem.ProjectType.Equals(GlobalConstants.PROJECTTYPE_GraphicsChangesInternalAdjustments);
            //if (lblHardSoftTransition.Visible)
            //    lblHardSoftTransition.Text = "Hard or Soft Transition: " + itemProposalItem.Flowthrough;
            lblImmediateSPKChange.Text = "Immediate SPK Change: " + sapBomSetupItem.ImmediateSPKChange;
            List<CheckBox> cbs = new List<CheckBox>();
            ExternalManufacturingItem externalMfgItem = externalMfgService.GetExternalManufacturingItem(iItemId);
            try
            {
                CheckBox chkVerifyFGBOMInDCs = new CheckBox();
                chkVerifyFGBOMInDCs.CssClass = "bomTask";
                chkVerifyFGBOMInDCs.ID = "chkVerifyPrivateLabel";
                chkVerifyFGBOMInDCs.Text = "Verify Private Label Finished Goods HUB DC (Delivering Plant) is set as the Primary DC indicated";
                chkVerifyFGBOMInDCs.Checked = string.IsNullOrEmpty(sapBomSetupItem.VerifyPrivateLabel) ? false : sapBomSetupItem.VerifyPrivateLabel.ToLower().Equals("yes");
                cbs.Add(chkVerifyFGBOMInDCs);

                chkVerifyFGBOMInDCs = new CheckBox();
                chkVerifyFGBOMInDCs.CssClass = "bomTask";
                chkVerifyFGBOMInDCs.ID = "chkVerifyFGDCFP07";
                chkVerifyFGBOMInDCs.Text = "Verify the Finished Good HUB DC (Delivering Plant) is set to FP07 when the pack location are any of the following: FM01, FM02, Sunrise, Canels, Pissa, Kua, Spangler, FP07";
                chkVerifyFGBOMInDCs.Checked = string.IsNullOrEmpty(sapBomSetupItem.VerifyFGDCFP07) ? false : sapBomSetupItem.VerifyFGDCFP07.ToLower().Equals("yes");
                cbs.Add(chkVerifyFGBOMInDCs);

                chkVerifyFGBOMInDCs = new CheckBox();
                chkVerifyFGBOMInDCs.CssClass = "bomTask";
                chkVerifyFGBOMInDCs.ID = "chkVerifyFGDCFP13";
                chkVerifyFGBOMInDCs.Text = "Verify the Finished Good HUB DC (Delivering Plant) is set to FP13 when the pack location is any of the following: Sunrise (in addition to FP07) and any other pack locations not indicated for FP07";
                chkVerifyFGBOMInDCs.Checked = string.IsNullOrEmpty(sapBomSetupItem.VerifyFGDCFP13) ? false : sapBomSetupItem.VerifyFGDCFP13.ToLower().Equals("yes");
                cbs.Add(chkVerifyFGBOMInDCs);

                chkVerifyFGBOMInDCs = new CheckBox();
                chkVerifyFGBOMInDCs.CssClass = "bomTask";
                chkVerifyFGBOMInDCs.ID = "chkVerifyFGSPKOthers";
                chkVerifyFGBOMInDCs.Text = "Verify the SPKs for the other DCs should point to the Hub DC (Delivering Plant). In the event of Sunrise, extend FP18 to pull from FP13, and for FP19 to pull from FP07";
                chkVerifyFGBOMInDCs.Checked = string.IsNullOrEmpty(sapBomSetupItem.VerifyFGSPKOthers) ? false : sapBomSetupItem.VerifyFGSPKOthers.ToLower().Equals("yes");
                cbs.Add(chkVerifyFGBOMInDCs);

                chkVerifyFGBOMInDCs = new CheckBox();
                chkVerifyFGBOMInDCs.CssClass = "bomTask";
                chkVerifyFGBOMInDCs.ID = "chkVerifyFGBOMInDCs";
                chkVerifyFGBOMInDCs.Text = "Verify the Finished Good is set up in the following DC's: FP07, FP13, FP18, FP19";
                chkVerifyFGBOMInDCs.Checked = string.IsNullOrEmpty(sapBomSetupItem.VerifyFGBOMInDCs) ? false : sapBomSetupItem.VerifyFGBOMInDCs.ToLower().Equals("yes");
                cbs.Add(chkVerifyFGBOMInDCs);

                if (itemProposalItem.TBDIndicator.ToLower() == "yes" && externalMfgItem.CoManufacturingClassification.ToLower() == "external turnkey fg")
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkTurnkeyFGMMCreated";
                    checkbox.Text = "Turnkey FG Material Master Created.";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.TurnkeyFGMMCreated) ? false : sapBomSetupItem.TurnkeyFGMMCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                    return cbs;
                }
                if (sapBomSetupItem.TransferSAPSpecsChangePackMeas.ToLower() == "yes")
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkTransferSAPSpecsChangeCompleted";
                    checkbox.Text = "Transfer Semi/Purchased Candy Semi Material Master Specifications (SAP) specifications are changing. Please update the specifications in SAP.";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.TransferSAPSpecsChangeCompleted) ? false : sapBomSetupItem.TransferSAPSpecsChangeCompleted.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                    return cbs;
                }
                else if (sapBomSetupItem.TransferSAPSpecsChangePackMeas.ToLower() == "yes")
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkFGSAPSpecsChangeCompleted";
                    checkbox.Text = "FG Material Master Specifications (SAP) specifications are changing. Please update the specifications in SAP.";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.FGSAPSpecsChangeCompleted) ? false : sapBomSetupItem.FGSAPSpecsChangeCompleted.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                if (itemProposalItem.TBDIndicator.ToLower() == "yes")
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkNewFGBOMCreated";
                    checkbox.Text = "New FG BOM Created";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.NewFGBOMCreated) ? false : sapBomSetupItem.NewFGBOMCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                int newTSRequired = (from PIs in packagingComponents where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && PIs.NewExisting == "New" select PIs).Count();
                int newFGComponentRequired = (from PIs in packagingComponents where PIs.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && PIs.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi && PIs.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && PIs.NewExisting == "New" && PIs.ParentID == 0 select PIs).Count();
                int newTSComponentRequired = (from PIs in packagingComponents where PIs.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && PIs.NewExisting == "New" && PIs.ParentID != 0 select PIs).Count();
                if (newTSRequired > 0)
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkNewMaterialNumbersCreated";
                    checkbox.Text = "New SEMIs created";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.NewMaterialNumbersCreated) ? false : sapBomSetupItem.NewMaterialNumbersCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox);

                    CheckBox checkbox2 = new CheckBox();
                    checkbox2.CssClass = "bomTask";
                    checkbox2.ID = "chkCompleteTSBOMCreated";
                    checkbox2.Text = "Complete Transfer Semi BOM(s) created";
                    checkbox2.Checked = string.IsNullOrEmpty(sapBomSetupItem.CompleteTSBOMCreated) ? false : sapBomSetupItem.CompleteTSBOMCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox2);
                }
                if (newFGComponentRequired > 0)
                {
                    CheckBox checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkNewMaterialNumbersCreated";
                    checkbox.Text = "New FG Packaging #s created";
                    checkbox.Checked = string.IsNullOrEmpty(sapBomSetupItem.NewMaterialNumbersCreated) ? false : sapBomSetupItem.NewMaterialNumbersCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                if (newTSComponentRequired > 0)
                {
                    CheckBox checkbox3 = new CheckBox();
                    checkbox3.CssClass = "bomTask";
                    checkbox3.ID = "chkNewTSCompPackNumsCreated";
                    checkbox3.Text = "New Transfer Semi Component/Packaging Material Numbers created";
                    checkbox3.Checked = string.IsNullOrEmpty(sapBomSetupItem.NewTSCompPackNumsCreated) ? false : sapBomSetupItem.NewTSCompPackNumsCreated.ToLower().Equals("yes");
                    cbs.Add(checkbox3);
                }
                List<PackagingItem> transferSemi = packagingItemService.GetTransferPurchasedSemiItemsForProject(iItemId, GlobalConstants.COMPONENTTYPE_TransferSemi);
                if (transferSemi.Count > 0)
                {
                    bool tsNetworkMove = false;
                    foreach (PackagingItem tsItem in transferSemi)
                    {
                        if (tsItem.NewExisting.ToLower() == "network move" || tsItem.NewExisting.ToLower() == "new")
                        {
                            tsNetworkMove = true;
                            break;
                        }
                        else
                        {
                            List<PackagingItem> transferSemiChildred = packagingItemService.GetSemiChildTSBOMItemsForBOMSetup(iItemId, tsItem.Id, GlobalConstants.COMPONENTTYPE_TransferSemi);
                            if (transferSemiChildred.Count > 0)
                            {
                                tsNetworkMove = true;
                                break;
                            }
                        }
                    }
                    if (tsNetworkMove && !(newTSRequired > 0))
                    {
                        CheckBox checkbox3 = new CheckBox();
                        checkbox3.CssClass = "bomTask";
                        checkbox3.ID = "chkCompleteFGBOMCreated";
                        checkbox3.Text = "Complete Transfer Semi BOM(s) created";
                        checkbox3.Checked = string.IsNullOrEmpty(sapBomSetupItem.CompleteFGBOMCreated) ? false : sapBomSetupItem.CompleteFGBOMCreated.ToLower().Equals("yes");
                        cbs.Add(checkbox3);
                    }
                }
            }
            catch (Exception exception)
            {
                string ids = "";
                foreach (CheckBox box in cbs)
                {
                    ids = ids + box.ID + ";";
                }
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPBOMSetup.ToString() + ": " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.SAPBOMSetup.ToString(), "checkboxSetup" + ids);
            }
            return cbs;
        }

        private void LoadBOMGrid()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();
            phBOM.Controls.Clear();
            dtPackingItem = packagingItemService.GetFinishedGoodItemsForProject(iItemId);

            ucBOMGrid ctrl = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            phBOM.Controls.Add(ctrl);

            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();

            ucBOMPackMeas packMeas = (ucBOMPackMeas)Page.LoadControl(_ucBOMPackMeas);
            packMeas.MaterialDesc = hdnMaterialDesc.Value;
            packMeas.MaterialNumber = hdnMaterialNumber.Value;
            packMeas.PackagingComponent = "";
            packMeas.ParentId = 0;
            packMeas.NewComponentCount = newItemCount;
            phBOM.Controls.Add(packMeas);

            foreach (PackagingItem item in dtPackingItem)
            {
                if (item.PackagingComponent.ToLower().Contains("transfer") || item.PackagingComponent.ToLower().Contains("purchased candy"))
                {
                    ucBOMGrid ctrl2 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOM.Controls.Add(ctrl2);

                    List<PackagingItem> dtChildPackingItem = new List<PackagingItem>();
                    dtChildPackingItem = packagingItemService.GetSemiChildTSBOMItems(iItemId, item.Id, GlobalConstants.COMPONENTTYPE_TransferSemi);
                    int newItemCountChild = dtChildPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();

                    ucBOMPackMeas packMeas2 = (ucBOMPackMeas)Page.LoadControl(_ucBOMPackMeas);
                    packMeas2.MaterialDesc = item.MaterialDescription;
                    packMeas2.MaterialNumber = item.MaterialNumber;
                    packMeas2.PackagingComponent = item.PackagingComponent;
                    packMeas2.ParentId = item.Id;
                    packMeas2.NewExisting = item.NewExisting;
                    packMeas2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    packMeas2.NewComponentCount = newItemCountChild;
                    phBOM.Controls.Add(packMeas2);


                    foreach (PackagingItem childItem in dtChildPackingItem)
                    {
                        ucBOMGrid ctrl3 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                        ctrl3.PackagingComponent = childItem.PackagingComponent;
                        ctrl3.ParentId = childItem.Id;
                        ctrl3.MaterialNumber = childItem.MaterialNumber;
                        ctrl3.MaterialDesc = childItem.MaterialDescription;
                        ctrl3.SemiXferMakeLocation = childItem.TransferSEMIMakePackLocations;
                        ctrl3.isChildItem = true;
                        phBOM.Controls.Add(ctrl3);

                        ucBOMPackMeas packMeas3 = (ucBOMPackMeas)Page.LoadControl(_ucBOMPackMeas);
                        packMeas3.MaterialDesc = childItem.MaterialDescription;
                        packMeas3.MaterialNumber = childItem.MaterialNumber;
                        packMeas3.PackagingComponent = childItem.PackagingComponent;
                        packMeas3.ParentId = childItem.Id;
                        packMeas3.SemiXferMakeLocation = childItem.TransferSEMIMakePackLocations;
                        packMeas3.NewExisting = childItem.NewExisting;
                        phBOM.Controls.Add(packMeas3);
                    }
                }
            }
            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable)Page.LoadControl(_ucBOMEditable);
                ctrlPE.PackagingComponent = hdnComponentype.Value;
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
                    ucBOMGrid_New ctrl2 = (ucBOMGrid_New)Page.LoadControl(_ucBOMGrid_New);
                    ctrl2.ID = "grid" + item.Id.ToString();
                    ctrl2.iItemId = iItemId;
                    ctrl2.GridItem = item;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    //ctrl2.openBtnSave = openBtnSave;
                    phBOM.Controls.Add(ctrl2);

                    ucBOMPackMeas_New packMeas2 = (ucBOMPackMeas_New)Page.LoadControl(_ucBOMPackMeas_New);
                    packMeas2.MaterialDesc = item.MaterialDescription;
                    packMeas2.MaterialNumber = item.MaterialNumber;
                    packMeas2.PackagingComponent = item.PackagingComponent;
                    packMeas2.ParentId = item.Id;
                    packMeas2.NewExisting = item.NewExisting;
                    packMeas2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    packMeas2.ID = "PackMeas" + item.Id;
                    phBOM.Controls.Add(packMeas2);
                }
            }
            ucBOMGrid_New ctrl = (ucBOMGrid_New)Page.LoadControl(_ucBOMGrid_New);
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
            //ctrl.openBtnSave = openBtnSave;
            phBOM.Controls.Add(ctrl);

            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();

            ucBOMPackMeas_New packMeas = (ucBOMPackMeas_New)Page.LoadControl(_ucBOMPackMeas_New);
            packMeas.MaterialDesc = hdnMaterialDesc.Value;
            packMeas.MaterialNumber = hdnMaterialNumber.Value;
            packMeas.PackagingComponent = "";
            packMeas.ParentId = 0;
            packMeas.NewComponentCount = newItemCount;
            packMeas.ID = "PackMeasFG" + iItemId;
            phBOM.Controls.Add(packMeas);


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

        public void GetuserControls()
        {
            foreach (var ctrl in phBOM.Controls)
            {
                if (ctrl is ucBOMPackMeas)
                {
                    var type = (ucBOMPackMeas)ctrl;

                    type.saveData();

                }
                if (ctrl is ucBOMGrid)
                {
                    var type2 = (ucBOMGrid)ctrl;

                    type2.saveData();

                }
            }
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

            rptCandy.DataSource = candylst;
            rptCandy.DataBind();
        }
        private SAPBOMSetupItem ConstructFormData()
        {
            GetuserControls();
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
                sapBOMSetupService.UpdateSAPBOMSetupItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                sapBOMSetupService.UpdateSAPBOMSetupApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();

                if (!string.Equals(PLMProject, "Yes"))
                {
                    LoadBOMGrid();
                }
                else
                {
                    LoadBOMGrid_New();
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPBOMSetup.ToString() + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.SAPBOMSetup.ToString(), "btnSubmit_Click");
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
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPBOMSetup.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.SAPBOMSetup.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
    }
}
