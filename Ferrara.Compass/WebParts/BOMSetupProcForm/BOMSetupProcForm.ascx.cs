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
using System.Threading;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.BOMSetupProcForm
{
    [ToolboxItemAttribute(false)]
    public partial class BOMSetupProcForm : WebPart
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
        public BOMSetupProcForm()
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
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.BOMSetupProc.ToString(), "Page_Load");
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
            if (!userManagementService.HasReadAccess(CompassForm.BOMSetupProc))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupProc))
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
            hdnPageName.Value = GlobalConstants.PAGE_BOMSetupProc;
            hdnCompassListItemId.Value = iItemId.ToString();

            BOMSetupProjectSummaryItem projectSummaryItem = BOMSetupService.GetProjectSummaryDetails(iItemId);
            hdnProjectType.Value = projectSummaryItem.ProjectType;
            hdnMaterialDesc.Value = projectSummaryItem.SAPDescription;
            hdnProjectTypeSubCategory.Value = projectSummaryItem.ProjectSubCategory;
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
        protected void lnkDeleteApprovedGraphicsAsset_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
            }
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
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.ProjectType = hdnProjectType.Value;
                    ctrl2.ProjectTypeSubCategory = hdnProjectTypeSubCategory.Value;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.GridItem = item;
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
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.ProjectType = hdnProjectType.Value;
            ctrl.ProjectTypeSubCategory = hdnProjectTypeSubCategory.Value;
            ctrl.packagingItems = dtPackingItem;
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
                ctrlPE.CompassListItemId = iItemId;
                ctrlPE.ProjectTypeSubCategory = hdnProjectTypeSubCategory.Value;
                ctrlPE.ProjectType = hdnProjectType.Value;
                // Add messages to page
                phBOMEdits.Controls.Add(ctrlPE);
            }
        }
        public void GetuserControls()
        {
            foreach (var ctrl in phBOMGrid.Controls)
            {
                if (ctrl is ucBOMGridPackMeas_New)
                {
                    ucBOMGridPackMeas_New type = (ucBOMGridPackMeas_New)ctrl;
                }
            }
            List<int> deletedIds = Utilities.GetIntegerArrayFromDelimittedString(hdnDeletedCompIds.Value, ';');
            if (deletedIds.Count > 0)
            {
                BOMSetupService.DeleteBOMSetupItems(deletedIds);
                hdnDeletedCompIds.Value = "";
            }
        }
        private BillofMaterialsItem ConstructFormData(bool submitted)
        {
            BillofMaterialsItem bomItem = new BillofMaterialsItem();
            try
            {
                bomItem.CompassListItemId = iItemId;
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": ConstructFormData: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "ConstructFormData");
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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "BOM Setup Proc");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupProc))
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
                BOMSetupService.UpdateBillOfMaterialsItem(item, GlobalConstants.PAGE_BillofMaterialSetUpProc);

                ApprovalItem approvalItem = ConstructApprovalData();
                BOMSetupService.UpdateBillofMaterialsApprovalItem(approvalItem, GlobalConstants.PAGE_BillofMaterialSetUpProc, false);

                lblSaved.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupProc))
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
                BOMSetupService.UpdateBillOfMaterialsItem(item, GlobalConstants.PAGE_BillofMaterialSetUpProc);

                //APPROVAL DATA FOR PROCUREMENT IS SET IN THE WORKFLOW

                // Complete the workflow task
                CompleteWorkflowTask();
                var RejectedItems = new List<string>();

                if (hdnProjectType.Value == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    var dtPackingItems = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);
                    RejectedItems =
                      (
                       from
                           dtPackingItem in dtPackingItems
                       where
                           dtPackingItem.IsAllProcInfoCorrect == "No"
                       select
                           string.Concat(dtPackingItem.MaterialNumber, ": ", dtPackingItem.MaterialDescription, ": ", dtPackingItem.WhatProcInfoHasChanged)
                       ).ToList();
                }

                if (RejectedItems.Count > 0)
                {
                    var ProjectCancelReasson = string.Join(", ", RejectedItems);
                    Utilities.UpdateProjectRejectionReason(iItemId, ProjectCancelReasson, GlobalConstants.PAGE_BOMSetupProc);
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
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

        private void CompleteWorkflowTask()
        {
            try
            {
                List<string> wftasks = hdnWorkflowSteps.Value.Split(';').ToList();
                foreach (string task in wftasks)
                {
                    workflowService.CompleteWorkflowTask(iItemId, (WorkflowStep)Enum.Parse(typeof(WorkflowStep), task));
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BOMSetupProc);
                }
                catch (Exception e)
                {

                }
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": CompleteWorkflowTask: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "CompleteWorkflowTask");
            }
        }
        private bool RequiredFieldCheckForPackagingComponent()
        {
            List<string> completedCompIds = new List<string>();
            List<string> lstrequiredItesms = new List<string>();
            List<BOMSetupItem> totalPackingItem = new List<BOMSetupItem>();
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();
            string taskAssigned = "";
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
            try
            {
                taskAssigned = hdnWorkflowSteps.Value.Split(';').ToList()[0];
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": hdnWorkflowSteps.Value.Split(';').ToList()[0]: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "hdnWorkflowSteps.Value.Split(';').ToList()[0]");
            }
            if (taskAssigned == "BOMSetupProcCoMan" || taskAssigned == "BOMSetupProcNovelty" || taskAssigned == "BOMSetupProcSeasonal")// || taskAssigned == "BOMSetupProcExternal"))
            {
                completedCompIds = completedCompIds.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                List<int> newCompletedCompIds = new List<int>();
                List<string> newCompletedCompRevPrinter = new List<string>();
                foreach (string compString in completedCompIds)
                {
                    int colonIndex = compString.IndexOf(":");
                    int semiIndex = compString.IndexOf(";");
                    string compId = compString.Substring(0, colonIndex);
                    int result;
                    if (int.TryParse(compId, out result))
                    {
                        newCompletedCompIds.Add(result);
                    }
                    string revPrinterSupplier = compString.Substring(semiIndex + 1);
                    newCompletedCompRevPrinter.Add(revPrinterSupplier);
                }
                foreach (var item in totalPackingItem)
                {
                    int isCompleted = (from id in newCompletedCompIds where Convert.ToInt32(id) == item.Id select id).Count();
                    int currentID = newCompletedCompIds[0];
                    if (isCompleted <= 0)
                    {
                        ErrorSummary.AddError("Please complete component information: " + currentID, this.Page);
                        iserror = true;
                    }
                }
            }
            else
            {
                BOMSetupProjectSummaryItem projectSemmary = BOMSetupService.GetProjectSummaryDetails(iItemId);
                string projectType = projectSemmary.ProjectType;
                string subCat = projectSemmary.ProjectSubCategory;
                try
                {
                    List<string> currentTasks = hdnWorkflowSteps.Value.Split(';').ToList();
                    List<BOMSetupItem> applicableItems = new List<BOMSetupItem>();
                    foreach (string task in currentTasks)
                    {
                        if (!string.IsNullOrEmpty(task))
                        {
                            string taskType = "";
                            if (task.Contains("BOMSetupProcSeasonal"))
                            {
                                taskType = task.Substring(20);
                            }
                            else if (task.Contains("BOMSetupProcExternal"))
                            {
                                taskType = task.Substring(20);
                            }
                            else if (task.Contains("BOMSetupProcEBP"))
                            {
                                taskType = task.Substring(15);
                            }

                            List<BOMSetupItem> taskItems = new List<BOMSetupItem>();

                            taskItems.AddRange((from PI in totalPackingItem where PI.PackagingComponent.ToLower().Replace(" ", "").Contains(taskType.ToLower()) && (PI.NewExisting == "New") select PI).ToList());
                            var NMTSs = (from BOMSetupItem comp in totalPackingItem where comp.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi && comp.NewExisting == "Network Move" select comp).ToList();

                            foreach (var NMTS in NMTSs)
                            {
                                taskItems.AddRange((from PI in totalPackingItem where PI.PackagingComponent.ToLower().Replace(" ", "").Contains(taskType.ToLower()) && PI.NewExisting == "Existing" && PI.ParentID == NMTS.Id select PI).ToList());
                            }

                            if (projectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove || subCat == GlobalConstants.PROJECTTYPESUBCATEGORY_ComplexNetworkMove)
                            {
                                taskItems.AddRange((from PI in totalPackingItem where PI.PackagingComponent.ToLower().Replace(" ", "").Contains(taskType.ToLower()) && PI.ParentID == 0 select PI).ToList());
                            }

                            if (task.Contains("BOMSetupProcExternal"))
                            {
                                taskItems = (from TI in taskItems where TI.ReviewPrinterSupplier == "Yes" select TI).ToList();
                            }
                            else
                            {
                                taskItems = (from TI in taskItems where TI.ReviewPrinterSupplier != "Yes" select TI).ToList<BOMSetupItem>();
                            }

                            applicableItems.AddRange(taskItems);
                        }
                    }
                    completedCompIds = completedCompIds.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
                    List<KeyValuePair<int, string>> newCompletedCompIds = new List<KeyValuePair<int, string>>();
                    foreach (string compString in completedCompIds)
                    {
                        int colonIndex = compString.IndexOf(":");
                        int semiIndex = compString.IndexOf(";");
                        string compId = compString.Substring(0, colonIndex);
                        int result;
                        if (int.TryParse(compId, out result))
                        {
                            string revPrinterSupplier = compString.Substring(semiIndex + 1);
                            newCompletedCompIds.Add(new KeyValuePair<int, string>(result, revPrinterSupplier));
                        }

                    }
                    foreach (BOMSetupItem item in applicableItems)
                    {

                        int isCompleted = (from id in newCompletedCompIds where Convert.ToInt32(id.Key) == item.Id select id).Count();
                        if (isCompleted <= 0)
                        {
                            ErrorSummary.AddError("Please complete component information: " + item.PackagingComponent, this.Page);
                            iserror = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupProc.ToString() + ": RequiredFieldCheckForPackagingComponent: " + ex.Message);
                    exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupProc.ToString(), "RequiredFieldCheckForPackagingComponent");
                }
            }
            return iserror;
        }
    }
}
