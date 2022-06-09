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
using System.Text;

namespace Ferrara.Compass.WebParts.BOMSetupPEForm
{
    [ToolboxItemAttribute(false)]
    public partial class BOMSetupPEForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

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
        public BOMSetupPEForm()
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
            //this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;

            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;

                LoadFormData();
                InitializeScreen();
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
            hdnComponentStatusChangeIds.Value = "";
            LoadBOMGrid();
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                this.hiddenItemId.Value = iItemId.ToString();
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userManagementService.HasReadAccess(CompassForm.BOMSetupPE))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupPE))
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
            hdnPageName.Value = Utilities.GetCurrentPageName();
            hdnCompassListItemId.Value = iItemId.ToString();

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
        #endregion
        private void LoadBOMGrid()
        {
            List<BOMSetupItem> dtPackingItem = new List<BOMSetupItem>();

            phBOMGrid.Controls.Clear();
            dtPackingItem = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);

            foreach (BOMSetupItem item in dtPackingItem)
            {
                if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    ucBOMGrid_New ctrl2 = (ucBOMGrid_New)Page.LoadControl(_ucBOMGrid_New);
                    ctrl2.ID = "grid" + item.Id.ToString();
                    ctrl2.iItemId = iItemId;
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.ProjectType = hdnProjectType.Value;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.GridItem = item;
                    ctrl2.openBtnSave = openBtnSave;
                    phBOMGrid.Controls.Add(ctrl2);
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
            ctrl.ProjectNumber = ProjectNumber;
            ctrl.packagingItems = dtPackingItem;
            ctrl.ParentId = 0;
            ctrl.GridItem = FGItem;
            ctrl.openBtnSave = openBtnSave;
            phBOMGrid.Controls.Add(ctrl);


            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable_New)Page.LoadControl(_ucBOMEditable_New);
                ctrlPE.PackagingComponent = hdnPackagingComponent.Value;
                ctrlPE.PackagingItemId = Convert.ToInt32(hdnPackagingID.Value);
                ctrlPE.ParentID = hdnParentID.Value;
                ctrlPE.CompassListItemId = iItemId;
                ctrlPE.AllPIs = dtPackingItem;
                // Add messages to page
                phBOMEdits.Controls.Add(ctrlPE);
            }
        }
        private BillofMaterialsItem ConstructFormData(bool submitted)
        {
            BillofMaterialsItem bomItem = new BillofMaterialsItem();
            try
            {
                bomItem.CompassListItemId = iItemId;
                if (submitted)
                {
                    if (string.IsNullOrEmpty(hdnPELead.Value))
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
                    else
                    {
                        bomItem.PackagingEngineerLead = hdnPELead.Value;
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE.ToString() + ": ConstructFormData: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE.ToString(), "ConstructFormData");
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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "BOM Setup PE");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupPE))
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
                BOMSetupService.UpdateBillOfMaterialsItem(item, "BOMSetupPE");

                ApprovalItem approvalItem = ConstructApprovalData();
                BOMSetupService.UpdateBillofMaterialsApprovalItem(approvalItem, Utilities.GetCurrentPageName(), false);

                lblSaved.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userManagementService.HasWriteAccess(CompassForm.BOMSetupPE))
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
                BOMSetupService.UpdateBillOfMaterialsItem(item, "BOMSetupPE");

                ApprovalItem approvalItem = ConstructApprovalData();
                if (!string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BillofMaterialSetUpProc.ToLower()))
                {
                    BOMSetupService.UpdateBillofMaterialsApprovalItem(approvalItem, Utilities.GetCurrentPageName(), true);
                }
                // Complete the workflow task
                CompleteWorkflowTask();

                // Redirect to Home page after successfull Submit 
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

        public void GetuserControls()
        {
            List<int> deletedIds = Utilities.GetIntegerArrayFromDelimittedString(hdnDeletedCompIds.Value, ';');
            if (deletedIds.Count > 0)
            {
                BOMSetupService.DeleteBOMSetupItems(deletedIds);
                hdnDeletedCompIds.Value = "";
            }
        }
        private void CompleteWorkflowTask()
        {
            workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.BOMSetupPE);
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
