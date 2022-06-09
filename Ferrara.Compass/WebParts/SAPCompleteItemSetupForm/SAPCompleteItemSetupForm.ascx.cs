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

namespace Ferrara.Compass.WebParts.SAPCompleteItemSetupForm
{
    [ToolboxItemAttribute(false)]
    public partial class SAPCompleteItemSetupForm : WebPart
    {
        #region Member Variables
        private IBOMSetupService BOMSetupService;
        private ISAPCompleteItemSetupService sapCompleteItemSetupService;
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
        public SAPCompleteItemSetupForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            sapCompleteItemSetupService = DependencyResolution.DependencyMapper.Container.Resolve<ISAPCompleteItemSetupService>();
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

                    rptSPKDets.DataSource = sapCompleteItemSetupService.getTSSPKDetails(iItemId);
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
            if (!userMgmtService.HasReadAccess(CompassForm.SAPCompleteItemSetup))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.SAPCompleteItemSetup))
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
            SAPCompleteSetupItem item;

            hdnPageName.Value = GlobalConstants.PAGE_SAPCompleteItemSetup;
            hdnCompassListItemId.Value = iItemId.ToString();
            item = sapCompleteItemSetupService.GetSAPCompleteItemSetupItem(iItemId);
            lblProductHierarchy1.Text = item.ProductHierarchyLevel1;
            lblProductHierarchy2.Text = item.ProductHierarchyLevel2;
            lblMaterialGroup1Brand.Text = item.MaterialGroup1Brand;
            lblMaterialGroup2Pricing.Text = item.MaterialGroup2Pricing;
            lblMaterialGroup4ProductForm.Text = item.MaterialGroup4ProductForm;
            lblMaterialGroup5PackType.Text = item.MaterialGroup5PackType;
            lblProfitCenter.Text = item.ProfitCenter;
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

            if (!string.IsNullOrEmpty(item.ExternalPacker) && !string.Equals(item.ExternalPacker, "Select..."))
            {
                dvPackLocation.Visible = true;
                lblExternalPacker.Text = item.ExternalPacker;
            }
            else
            {
                dvPackLocation.Visible = false;
            }

            if (string.IsNullOrEmpty(item.DesignateHUBDC) || string.Equals(item.DesignateHUBDC, "Select..."))
            {
                divDesignateHUBDC.Visible = false;
            }
            else
            {
                divDesignateHUBDC.Visible = true;
                lblDesignateHUBDC.Text = item.DesignateHUBDC;
            }

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
            lblDisplayUPC.Text = item.DisplayBoxUPC;
            lblCaseUCC.Text = item.CaseUCC;
            lblPalletUCC.Text = item.PalletUCC;
            lblSAPBaseUOM.Text = item.SAPBaseUOM;
            hdnMaterialDesc.Value = item.SAPDescription;
            hdnMaterialNumber.Value = item.SAPItemNumber;
            hdnProjectType.Value = item.ProjectType;
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

            SAPCompleteSetupItem sapCompleteSetupItem = sapCompleteItemSetupService.GetSAPCompleteItemSetupItem(iItemId);
            ItemProposalItem itemProposalItem = itemProposalService.GetItemProposalItem(iItemId);
            List<PackagingItem> packagingComponents = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            txtProjectType.Text = itemProposalItem.ProjectType;
            txtProjectTypeSubCategory.Text = itemProposalItem.ProjectTypeSubCategory;
            txtItemConcept.Text = itemProposalItem.ItemConcept;
            if (itemProposalItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                lblItemConcept.Text = "Change Notes:";
            }
            else
            {
                lblItemConcept.Text = "Item Concept:";
            }

            lblImmediateSPKChange.Text = "Immediate SPK Change: " + sapCompleteSetupItem.ImmediateSPKChange;
            List<CheckBox> cbs = new List<CheckBox>();
            ExternalManufacturingItem externalMfgItem = externalMfgService.GetExternalManufacturingItem(iItemId);
            try
            {
                CheckBox checkbox = new CheckBox();
                #region  Complete FG material master 
                if (itemProposalItem.TBDIndicator.ToLower() == "yes")
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkCmpltFGMtrlMaster";
                    checkbox.Text = "Complete FG material master ";
                    checkbox.Checked = string.IsNullOrEmpty(sapCompleteSetupItem.CmpltFGMtrlMaster) ? false : sapCompleteSetupItem.CmpltFGMtrlMaster.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
                #region Turnkey FG Material Master created
                if (itemProposalItem.TBDIndicator.ToLower() == "yes" && externalMfgItem.CoManufacturingClassification.ToLower() == "external turnkey fg")
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkTurnkeyFGMMCrtd";
                    checkbox.Text = "Turnkey FG Material Master Created.";
                    checkbox.Checked = string.IsNullOrEmpty(sapCompleteSetupItem.TurnkeyFGMMCrtd) ? false : sapCompleteSetupItem.TurnkeyFGMMCrtd.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion
                #region FG Material Master Specifications(SAP) specifications is new or changing.Please update the specifications in SAP.
                if (sapCompleteSetupItem.FGSAPSpecsChangePackMeas.ToLower() == "yes")
                {
                    checkbox = new CheckBox();
                    checkbox.CssClass = "bomTask";
                    checkbox.ID = "chkFGSAPSpChCmpltd";
                    checkbox.Text = "FG Material Master Specifications (SAP) specifications is new or changing. Please update the specifications in SAP.";
                    checkbox.Checked = string.IsNullOrEmpty(sapCompleteSetupItem.FGSAPSpChCmpltd) ? false : sapCompleteSetupItem.FGSAPSpChCmpltd.ToLower().Equals("yes");
                    cbs.Add(checkbox);
                }
                #endregion

                #region Complete Transfer Semi BOM/ BOMs(if there is a transfer semi, and there are components listed within the BOM, or if the TS is marked as Network Move)
                int NewTransferSemiRequired = (from PIs in packagingComponents where PIs.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi select PIs).Count();
                if (NewTransferSemiRequired > 0)
                {
                    CheckBox checkbox2 = new CheckBox();
                    checkbox2.CssClass = "bomTask";
                    checkbox2.ID = "chkCompleteTSBOM";
                    checkbox2.Text = "Complete Transfer Semi BOM/ BOMs(if there is a transfer semi, and there are components listed within the BOM, or if the TS is marked as Network Move)";
                    checkbox2.Checked = string.IsNullOrEmpty(sapCompleteSetupItem.CompleteTSBOM) ? false : sapCompleteSetupItem.CompleteTSBOM.ToLower().Equals("yes");
                    cbs.Add(checkbox2);
                }
                #endregion
                return cbs;

            }
            catch (Exception exception)
            {
                string ids = "";
                foreach (CheckBox box in cbs)
                {
                    ids = ids + box.ID + ";";
                }
                ErrorSummary.AddError(exception.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPCompleteItemSetup.ToString() + ": " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.SAPCompleteItemSetup.ToString(), "checkboxSetup" + ids);
            }
            return cbs;
        }
        private void LoadUserControls()
        {
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();

            phBOMGrid.Controls.Clear();
            dtPackingItem = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);
            BOMSetupItem FGItem = new BOMSetupItem()
            {
                PackagingComponent = "Finished Good",
                MaterialNumber = hdnMaterialNumber.Value,
                MaterialDescription = hdnMaterialDesc.Value,
                ParentID = 0
            };
            ucBOMGridPackMeas_New ctrl = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();
            ctrl.GridItem = FGItem;
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.packagingItems = dtPackingItem;
            ctrl.ParentId = 0;
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
                if (item.PackagingComponent.ToLower().Contains("transfer") || item.PackagingComponent.ToLower().Contains("purchased candy"))
                {
                    ucBOMGridPackMeas_New ctrl2 = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.GridItem = item;
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
        private SAPCompleteSetupItem ConstructFormData()
        {
            GetuserControls();
            var item = new SAPCompleteSetupItem();
            item.CompassListItemId = iItemId;
            item.LastUpdatedFormName = CompassForm.SAPCompleteItemSetup.ToString();
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
        private void openBtnSave()
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPCompleteItemSetup))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                SAPCompleteSetupItem item = ConstructFormData();
                sapCompleteItemSetupService.UpdateSAPCompleteItemSetupItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                sapCompleteItemSetupService.UpdateSAPCompleteItemSetupApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPCompleteItemSetup.ToString() + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.SAPCompleteItemSetup.ToString(), "btnSubmit_Click");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            openBtnSave();
            LoadUserControls();
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
                if (!userMgmtService.HasWriteAccess(CompassForm.SAPCompleteItemSetup))
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
                SAPCompleteSetupItem item = ConstructFormData();
                sapCompleteItemSetupService.UpdateSAPCompleteItemSetupItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                sapCompleteItemSetupService.UpdateSAPCompleteItemSetupApprovalItem(approvalItem, true);

                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.SAPCompleteItemSetup);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SAPCompleteItemSetup.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.SAPCompleteItemSetup.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
    }
}
