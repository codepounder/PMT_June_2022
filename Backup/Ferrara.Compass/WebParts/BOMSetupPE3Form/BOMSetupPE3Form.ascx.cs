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

namespace Ferrara.Compass.WebParts.BOMSetupPE3Form
{
    [ToolboxItemAttribute(false)]
    public partial class BOMSetupPE3Form : WebPart
    {
        #region Member Variables
        private IBOMSetupService BOMSetupService;
        private IUserManagementService userManagementService;
        private IWorkflowService workflowService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private INotificationService notificationService;
        private string webUrl;
        private int iItemId = 0;
        private const string _ucBOMEditable_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable_New.ascx";
        private const string _ucBOMGridPackMeas_New = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGridPackMeas_New.ascx";
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
        public BOMSetupPE3Form()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<BOMSetupService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
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
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE3.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BOMSetupPE3.ToString(), "Page_Load");
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
            if (!userManagementService.HasReadAccess(CompassForm.BOMSetupPE3))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupPE3))
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

        private void LoadFormData()
        {
            hdnPageName.Value = GlobalConstants.PAGE_NAME_PE3;
            hdnCompassListItemId.Value = iItemId.ToString();

            BOMSetupProjectSummaryItem projectSummaryItem = BOMSetupService.GetProjectSummaryDetails(iItemId);
            hdnMaterialDesc.Value = projectSummaryItem.SAPDescription;
            hdnMaterialNumber.Value = projectSummaryItem.SAPItemNumber;
            hdnProjectType.Value = projectSummaryItem.ProjectType;

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
            ucBOMGridPackMeas_New ctrl = (ucBOMGridPackMeas_New)Page.LoadControl(_ucBOMGridPackMeas_New);
            int newItemCount = dtPackingItem.Select(r => r.NewExisting.ToLower() == "new").Count();
            BOMSetupItem FGItem = new BOMSetupItem()
            {
                PackagingComponent = "Finished Good",
                MaterialNumber = hdnMaterialNumber.Value,
                MaterialDescription = hdnMaterialDesc.Value,
                ParentID = 0
            };
            ctrl.ID = "grid" + iItemId.ToString();
            ctrl.iItemId = iItemId;
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.packagingItems = dtPackingItem;
            ctrl.ParentId = 0;
            ctrl.GridItem = FGItem;
            ctrl.openBtnSave = openBtnSave;
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
        private BillofMaterialsItem ConstructFormData(bool submitted)
        {
            BillofMaterialsItem bomItem = new BillofMaterialsItem();
            try
            {
                bomItem.CompassListItemId = iItemId;
                if (submitted)
                {
                    if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_PackagingEngineer))
                    {
                        bomItem.PackagingEngineerLead = SPContext.Current.Web.CurrentUser.ID.ToString() + ";#" + SPContext.Current.Web.CurrentUser.LoginName;
                    }
                    else
                    {
                        bomItem.PackagingEngineerLead = GlobalConstants.GROUP_PackagingEngineer;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE3.ToString() + ": ConstructFormData: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE3.ToString(), "ConstructFormData");
            }
            return bomItem;
        }
        private ApprovalItem ConstructApprovalData()
        {
            var item = new ApprovalItem();

            item.CompassListItemId = iItemId;
            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        public void openBtnSave()
        {
            GetuserControls();
        }
        #region Button Methods
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "BOM Setup PE3");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupPE3))
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }
                GetuserControls();

                BillofMaterialsItem item = ConstructFormData(false);
                BOMSetupService.UpdateBillOfMaterialsItem(item, GlobalConstants.PAGE_BOMSetupPE3);

                ApprovalItem approvalItem = ConstructApprovalData();
                BOMSetupService.UpdateBillofMaterialsApprovalItem(approvalItem, GlobalConstants.PAGE_BOMSetupPE3, false);

                lblSaved.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE3.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE3.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupPE3))
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

                BillofMaterialsItem item = ConstructFormData(true);
                BOMSetupService.UpdateBillOfMaterialsItem(item, GlobalConstants.PAGE_BOMSetupPE3);

                ApprovalItem approvalItem = ConstructApprovalData();
                BOMSetupService.UpdateBillofMaterialsApprovalItem(approvalItem, GlobalConstants.PAGE_BOMSetupPE3, true);
                // Complete the workflow task
                CompleteWorkflowTask();

                // Redirect to Home page after successfull Submit 
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE3.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE3.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
        private void CompleteWorkflowTask()
        {
            workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BOMSetupPE3);
        }
        private bool RequiredFieldCheckForPackagingComponent()
        {
            List<string> completedCompIds = new List<string>();
            List<BOMSetupItem> totalPackingItem = new List<BOMSetupItem>();
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();
            bool iserror = false;
            if (hdnComponentStatusChangeIds.Value != "")
            {
                completedCompIds = hdnComponentStatusChangeIds.Value.Split(',').ToList();
            }
            dtPackingItem = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);
            totalPackingItem.AddRange((from FGItem in dtPackingItem where FGItem.ParentID == 0 select FGItem).ToList());

            foreach (BOMSetupItem item in dtPackingItem)
            {
                if (string.Equals(item.PackagingComponent.ToLower(), GlobalConstants.COMPONENTTYPE_TransferSemi.ToLower()) || string.Equals(item.PackagingComponent.ToLower(), GlobalConstants.COMPONENTTYPE_PurchasedSemi.ToLower()))
                {
                    List<BOMSetupItem> dtSemiPackingItem = (from semiBomitems in dtPackingItem where semiBomitems.ParentID == item.Id select semiBomitems).ToList();
                    totalPackingItem.AddRange(dtSemiPackingItem);
                }
            }
            completedCompIds = completedCompIds.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
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
    }
}
