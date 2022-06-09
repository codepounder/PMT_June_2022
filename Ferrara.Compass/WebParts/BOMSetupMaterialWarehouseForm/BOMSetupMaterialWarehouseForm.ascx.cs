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


namespace Ferrara.Compass.WebParts.BOMSetupMaterialWarehouseForm
{
    [ToolboxItemAttribute(false)]
    public partial class BOMSetupMaterialWarehouseForm : WebPart
    {
        #region Member Variables
        private IBOMSetupService BOMSetupService;
        private IBOMSetupMaterialWarehouseService bomSetupMaterialWarehouseService;
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
        public BOMSetupMaterialWarehouseForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
            bomSetupMaterialWarehouseService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupMaterialWarehouseService>();
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
                    InitializeScreen();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupMaterialWarehouse.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BOMSetupMaterialWarehouse.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
            LoadBOMGrid();
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
            if (!userMgmtService.HasReadAccess(CompassForm.BOMSetupMaterialWarehouse))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.BOMSetupMaterialWarehouse))
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
            hdnPageName.Value = GlobalConstants.PAGE_BOMSetupMaterialWarehouse;
            hdnCompassListItemId.Value = iItemId.ToString();
            BOMSetupMaterialWarehouseItem bomSetupMaterialWarehouseItem = bomSetupMaterialWarehouseService.GetBOMSetupMaterialWarehouseItem(iItemId);

            hdnMaterialDesc.Value = bomSetupMaterialWarehouseItem.SAPDescription;
            hdnMaterialNumber.Value = bomSetupMaterialWarehouseItem.SAPItemNumber;
            hdnProjType.Value = bomSetupMaterialWarehouseItem.ProjectType;
            hdnProjTypeSubCategory.Value = bomSetupMaterialWarehouseItem.ProjectTypeSubCategory;

            BOMSetupProjectSummaryItem projectSummaryItem = BOMSetupService.GetProjectSummaryDetails(iItemId);
            hdnMaterialDesc.Value = projectSummaryItem.SAPDescription;
            hdnMaterialNumber.Value = projectSummaryItem.SAPItemNumber;

            #region Project Information
            lblProjectType.Text = projectSummaryItem.ProjectType;
            lblProjectSubcategory.Text = projectSummaryItem.ProjectSubCategory;
            if (projectSummaryItem.PackingLocation == GlobalConstants.EXTERNAL_PACKER)
            {
                lblPackLocation.Text = projectSummaryItem.ExternalPacker;
            }
            else
            {
                lblPackLocation.Text = projectSummaryItem.PackingLocation;
            }

            txtWorkCenterAddInfo.Text = projectSummaryItem.WorkCenterAddInfo;
            lblPegHoleNeeded.Text = projectSummaryItem.PegHoleNeeded;
            lblItemConcept.Text = projectSummaryItem.ItemConcept;
            lblFGLikeItem.Text = projectSummaryItem.FGLikeItem;

            lblInitiatorName.Text = projectSummaryItem.InitiatorName;
            lblMarketingName.Text = projectSummaryItem.MarketingName;
            lblInTechManagerName.Text = projectSummaryItem.InTechManagerName;
            lblPMName.Text = projectSummaryItem.PMName;
            lblPackagingEngineerName.Text = Utilities.GetPersonFieldForDisplay(projectSummaryItem.PackagingEngineerName);


            if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupMaterialWarehouse.ToLower()))
            {
                divLogisticsInformation.Visible = true;
                #region Logistics Information
                txtMakeLocation.Text = projectSummaryItem.MakeLocation;
                txtPackLocation1.Text = projectSummaryItem.PackingLocation;
                //Procurement Type
                if (!string.IsNullOrEmpty(projectSummaryItem.ProcurementType) && !string.Equals(projectSummaryItem.ProcurementType, "Select..."))
                {
                    divProcurementType.Visible = true;
                    txtProcurementType.Text = projectSummaryItem.ProcurementType;
                }
                else
                {
                    divProcurementType.Visible = false;
                }
                //External Manufacturer
                if (!string.IsNullOrEmpty(projectSummaryItem.ExternalManufacturer) && !string.Equals(projectSummaryItem.ExternalManufacturer, "Select..."))
                {
                    divExternalManufacturer.Visible = true;
                    txtExternalManufacturer.Text = projectSummaryItem.ExternalManufacturer;
                }
                else
                {
                    divExternalManufacturer.Visible = false;
                }
                //External Packer
                if (!string.IsNullOrEmpty(projectSummaryItem.ExternalPacker) && !string.Equals(projectSummaryItem.ExternalPacker, "Select..."))
                {
                    dvPackLocation.Visible = true;
                    txtExternalPacker.Text = projectSummaryItem.ExternalPacker;
                }
                else
                {
                    dvPackLocation.Visible = false;
                }
                //Purchased Into
                if (!string.IsNullOrEmpty(projectSummaryItem.PurchasedIntoLocation) && !string.Equals(projectSummaryItem.PurchasedIntoLocation, "Select..."))
                {
                    divPurchaseIntoLocation.Visible = true;
                    txtPurchaseIntoLocation.Text = projectSummaryItem.PurchasedIntoLocation;
                }
                else
                {
                    divPurchaseIntoLocation.Visible = false;
                }
                //SAP Base UOM
                txtSAPBaseUOM.Text = projectSummaryItem.SAPBaseUOM;

                //Designate HUBDC
                txtDesignateHUBDC.Text = projectSummaryItem.DesignateHUBDC;
                #endregion
            }
            #endregion
        }
        private void LoadBOMGrid()
        {
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();

            phBOMGrid.Controls.Clear();
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
                    ctrl2.ProjectType = hdnProjType.Value;
                    ctrl2.ProjectTypeSubCategory = hdnProjTypeSubCategory.Value;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.NewExisting = item.NewExisting;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    ctrl2.openBtnSave = openBtnSave;
                    phBOMGrid.Controls.Add(ctrl2);
                }
            }
            BOMSetupItem FGItem = new BOMSetupItem()
            {
                PackagingComponent = "Finished Good",
                MaterialNumber = hdnMaterialNumber.Value,
                MaterialDescription = hdnMaterialDesc.Value,
                ParentID = 0
            };
            ucBOMGridPackMeas_New ctrl = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
            ctrl.ID = "grid" + iItemId.ToString();
            ctrl.iItemId = iItemId;
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.packagingItems = dtPackingItem;
            ctrl.ProjectType = hdnProjType.Value;
            ctrl.ProjectTypeSubCategory = hdnProjTypeSubCategory.Value;
            ctrl.ParentId = 0;
            ctrl.GridItem = FGItem;
            ctrl.openBtnSave = openBtnSave;
            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();
            ctrl.MaterialDesc = hdnMaterialDesc.Value;
            ctrl.MaterialNumber = hdnMaterialNumber.Value;
            ctrl.PackagingComponent = "";
            ctrl.ParentId = 0;
            ctrl.NewComponentCount = newItemCount;
            phBOMGrid.Controls.Add(ctrl);

            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable_New)Page.LoadControl(_ucBOMEditable_New);
                ctrlPE.PackagingComponent = hdnPackagingComponent.Value;
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
        private BOMSetupMaterialWarehouseItem ConstructFormData()
        {
            var item = new BOMSetupMaterialWarehouseItem();
            item.CompassListItemId = iItemId;
            item.LastUpdatedFormName = CompassForm.BOMSetupMaterialWarehouse.ToString();
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
                if (!userMgmtService.HasWriteAccess(CompassForm.BOMSetupMaterialWarehouse))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                GetuserControls();

                BOMSetupMaterialWarehouseItem item = ConstructFormData();
                bomSetupMaterialWarehouseService.UpdateBOMSetupMaterialWarehouseItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                bomSetupMaterialWarehouseService.UpdateBOMSetupMaterialWarehouseApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupMaterialWarehouse.ToString() + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BOMSetupMaterialWarehouse.ToString(), "btnSubmit_Click");
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
                if (!userMgmtService.HasWriteAccess(CompassForm.BOMSetupMaterialWarehouse))
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
                GetuserControls();
                // Retrieve the data from the form
                BOMSetupMaterialWarehouseItem item = ConstructFormData();
                bomSetupMaterialWarehouseService.UpdateBOMSetupMaterialWarehouseItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                bomSetupMaterialWarehouseService.UpdateBOMSetupMaterialWarehouseApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BOMSetupMaterialWarehouse);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupMaterialWarehouse.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupMaterialWarehouse.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
    }
}
