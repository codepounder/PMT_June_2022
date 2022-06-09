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
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Linq;

namespace Ferrara.Compass.WebParts.OBMSecondReviewForm
{
    [ToolboxItemAttribute(false)]
    public partial class OBMSecondReviewForm : WebPart
    {
        #region Member Variables
        private IOBMFirstReviewService obmFirstReviewService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private IOBMSecondReviewService obmSecondReviewService;
        private IExternalManufacturingService externalMfgService;

        private int iItemId = 0;
        private WorkflowStep wfStep;
        private string webUrl;
        private IBOMSetupService BOMSetupService;
        private string PLMProject = "";
        private const string _ucBOMEditable = @"~/_controltemplates/15/Ferrara.Compass/ucBOMEditable.ascx";
        private const string _ucBOMGrid = @"~/_controltemplates/15/Ferrara.Compass/ucBOMGrid.ascx";
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
        public OBMSecondReviewForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            obmFirstReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IOBMFirstReviewService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            obmSecondReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IOBMSecondReviewService>();
            externalMfgService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();
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
                    InitializeScreen();

                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.OBMReview2.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.OBMReview2.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
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
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            if (!userMgmtService.HasReadAccess(CompassForm.OBMReview2))
            {
                this.divAccessDenied.Visible = true;
            }
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            hdnPageName.Value = GlobalConstants.PAGE_OBMSecondReview;
            hdnCompassListItemId.Value = iItemId.ToString();

            OBMFirstReviewItem item = obmFirstReviewService.GetPMFirstReviewItem(iItemId);
            OBMSecondReviewItem secItem = obmSecondReviewService.GetPMSecondReviewItem(iItemId);
            ExternalManufacturingItem extMfg = externalMfgService.GetExternalManufacturingItem(iItemId);

            hdnProjectType.Value = item.ProjectType;
            hdnMaterialNumber.Value = secItem.SAPItemNumber;
            hdnMaterialDesc.Value = secItem.SAPDescription;
            hdnPLMProject.Value = extMfg.PLMFlag;
            PLMProject = extMfg.PLMFlag;
            lblAnnualProjectedDollars.Text = item.AnnualProjectedDollars.ToString("C0");
            lblAnnualProjectedUnits.Text = Utilities.FormatDecimal(item.AnnualProjectedUnits, 0);
            lblBrand.Text = item.MaterialGroup1Brand;
            lblCaseUCC.Text = item.CaseUCC;
            lblCustomer.Text = item.Customer;
            if (item.InitialCosting_GrossMarginAccurate != null && string.Equals(item.InitialCosting_GrossMarginAccurate.ToLower(), "no"))
                lblExpectedGrossMargin.Text = Utilities.FormatPercentage(item.RevisedGrossMarginPercent, 2);
            else
                lblExpectedGrossMargin.Text = Utilities.FormatPercentage(item.ExpectedGrossMarginPercent, 2);

            lblFirstShipDate.Text = Utilities.GetDateForDisplay(item.RevisedFirstShipDate);
            lblJarDisplayUPC.Text = item.DisplayBoxUPC;
            lblLineOfBusiness.Text = item.ProductHierarchyLevel1;
            lblManufacturingLocation.Text = item.MakeLocation;
            lblPalletUCC.Text = item.PalletUCC;
            lblDesignateHUBDC.Text = item.DesignateHUBDC;
            lblPrimaryPackingLocation.Text = item.PackingLocation;
            lblProductHierarchyLevel2.Text = item.ProductHierarchyLevel2;
            lblProjectNotes.Text = item.ItemConcept;
            lblProjectType.Text = item.ProjectType;
            lblUnitUPC.Text = item.UnitUPC;
            lblWeeksUntilShip.Text = Utilities.DetermineWeeksToShip(item.RevisedFirstShipDate);

            // Check for External Manufacturing
            if ((string.Equals(item.PackingLocation, GlobalConstants.EXTERNAL_PACKER)) ||
                (string.Equals(item.MakeLocation, GlobalConstants.EXTERNAL_MANUFACTURER)))
            {
                lblCoManufacturingClassification.Text = item.CoManufacturingClassification;
                lblExternalManufacturer.Text = item.ExternalManufacturer;
                lblExternalPacker.Text = item.ExternalPacker;
            }
            else
            {
                dvExternalFields.Visible = false;
                dvExternalHeading.Visible = false;
                dvExternalSeparator.Visible = false;
                dvExternalArtwork.Visible = false;
                dvExternalHeading2.Visible = false;
                dvExternalLeadTime.Visible = false;
                dvExternalSeperator2.Visible = false;
                dvExternalTimeline.Visible = false;
            }
            //if (secItem.OBMSecondReviewConcern != null)
            //{
            //    string[] concerns = secItem.OBMSecondReviewConcern.Split(',');
            //    foreach (string concern in concerns)
            //    {
            //        if (!string.IsNullOrEmpty(concern))
            //            cblSectionConcerns.Items.FindByValue(concern).Selected = true;
            //    }
            //}
            ddlcorrect.SelectedValue = secItem.OBMSecondReviewCheck;
            txtProductionDate.Text = Utilities.GetDateForDisplay(item.FirstProductionDate);

            txtFirstShipDate.Text = Utilities.GetDateForDisplay(item.RevisedFirstShipDate);
            Utilities.SetDropDownValue(secItem.SGSExpeditedWorkflowApproved, this.ddlSGSExpeditedWorkflowApproved, this.Page);
            //lblHowContinue.Text = secItem.SrOBMApproval2_Decision;

            txtCurrentTimelineAcceptable.Text = extMfg.CurrentTimelineAcceptable;
            txtLeadTimeFromSupplier.Text = extMfg.LeadTimeFromSupplier;
            txtFinalArtworkDueToSupplier.Text = Utilities.GetDateForDisplay(extMfg.FinalArtworkDueToSupplier);
            // Check for Internal Transfer Semi's
            //if (!string.Equals(item.InternalTransferSemiNeeded, "Yes"))
            //{
            //    dvTransferSemiHeader.Visible = false;
            //    dvTransferSemiRepeater.Visible = false;
            //    dvTransferSemiSeparator.Visible = false;
            //}
            //else
            //{
            //    var packagingItems = GetTransferSemiPackagingItems();

            //    if (packagingItems.Count == 0)
            //    {
            //        rptTransferSemi.Visible = false;
            //        lblNoTransferSemi.Visible = true;
            //    }
            //    else
            //    {
            //        rptTransferSemi.DataSource = packagingItems;
            //        rptTransferSemi.DataBind();
            //    }
            //}
        }
        public void openBtnSave()
        {
            if (!userMgmtService.HasWriteAccess(CompassForm.OBMReview2))
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
            OBMSecondReviewItem item = ConstructFormData();
            obmSecondReviewService.UpdatePMSecondReviewItem(item);

            GetuserControls();
        }
        private List<PackagingItem> GetTransferSemiPackagingItems()
        {
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();

            if (ViewState["TransferSemi"] == null)
            {
                if (iItemId > 0)
                {
                    dtPackingItem = packagingItemService.GetTransferSemiItemsForProject(iItemId);
                    ViewState["TransferSemi"] = dtPackingItem;
                }
            }
            else
            {
                dtPackingItem = (List<PackagingItem>)ViewState["TransferSemi"];
            }

            return dtPackingItem;
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
            ctrl.openBtnSave = openBtnSave;
            phBOM.Controls.Add(ctrl);

            foreach (PackagingItem item in dtPackingItem)
            {

                if (item.PackagingComponent.ToLower().Contains("transfer") || item.PackagingComponent.ToLower().Contains("purchased candy"))
                {
                    ucBOMGrid ctrl2 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                    ctrl2.openBtnSave = openBtnSave;
                    ctrl2.PackagingComponent = item.PackagingComponent;
                    ctrl2.ParentId = item.Id;
                    ctrl2.MaterialNumber = item.MaterialNumber;
                    ctrl2.MaterialDesc = item.MaterialDescription;
                    ctrl2.SemiXferMakeLocation = item.TransferSEMIMakePackLocations;
                    phBOM.Controls.Add(ctrl2);

                    List<PackagingItem> dtChildPackingItem = new List<PackagingItem>();
                    dtChildPackingItem = packagingItemService.GetSemiChildTSBOMItems(iItemId, item.Id, item.PackagingComponent);

                    foreach (PackagingItem childItem in dtChildPackingItem)
                    {
                        ucBOMGrid ctrl3 = (ucBOMGrid)Page.LoadControl(_ucBOMGrid);
                        ctrl3.openBtnSave = openBtnSave;
                        ctrl3.PackagingComponent = childItem.PackagingComponent;
                        ctrl3.ParentId = childItem.Id;
                        ctrl3.MaterialNumber = childItem.MaterialNumber;
                        ctrl3.MaterialDesc = childItem.MaterialDescription;
                        ctrl3.SemiXferMakeLocation = childItem.TransferSEMIMakePackLocations;
                        ctrl3.isChildItem = true;
                        phBOM.Controls.Add(ctrl3);
                    }
                }
            }

            if (hdnPageState.Value == "PE")
            {
                var ctrlPE = (ucBOMEditable)Page.LoadControl(_ucBOMEditable);
                ctrlPE.PackagingComponent = hdnPackagingComponent.Value;
                ctrlPE.PackagingItemId = Convert.ToInt32(hdnPackagingID.Value);
                ctrlPE.ParentID = hdnParentID.Value;
                ctrlPE.AllPIs = dtPackingItem;
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
            ctrl.ParentId = 0;
            ctrl.GridItem = FGItem;
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
        public void GetuserControls()
        {
            List<int> deletedIds = Utilities.GetIntegerArrayFromDelimittedString(hdnDeletedCompIds.Value, ';');

            if (deletedIds.Count > 0)
            {
                BOMSetupService.DeleteBOMSetupItems(deletedIds);
                hdnDeletedCompIds.Value = "";
            }
        }
        private OBMSecondReviewItem ConstructFormData()
        {
            var objOBMSecondReviewItem = new OBMSecondReviewItem
            {
                FirstProductionDate = Utilities.GetDateFromField(txtProductionDate.Text),
                FirstShipDate = Utilities.GetDateFromField(txtFirstShipDate.Text),
            };
            objOBMSecondReviewItem.CompassListItemId = iItemId;

            objOBMSecondReviewItem.OBMSecondReviewCheck = ddlcorrect.SelectedValue;
            objOBMSecondReviewItem.CompassListItemId = iItemId;

            objOBMSecondReviewItem.NewMaterialsinBOM = GlobalConstants.CONST_No;
            objOBMSecondReviewItem.NewCorrugatedPaperboardinBOM = GlobalConstants.CONST_No;
            objOBMSecondReviewItem.NewFilmLabelRigidPlasticinBOM = GlobalConstants.CONST_No;
            List<PackagingItem> dtPackingItem = new List<PackagingItem>();

            dtPackingItem = packagingItemService.GetAllPackagingItemsForProject(iItemId);
            foreach (PackagingItem item in dtPackingItem)
            {
                if (item.NewExisting.ToLower().Contains("new"))
                {
                    objOBMSecondReviewItem.NewMaterialsinBOM = GlobalConstants.CONST_Yes;

                    if ((item.PackagingComponent.Contains("Film")) || (item.PackagingComponent.Contains("Label")) ||
                        (item.PackagingComponent.Contains("Rigid")) || (item.PackagingComponent.Contains("Other")))
                        objOBMSecondReviewItem.NewFilmLabelRigidPlasticinBOM = GlobalConstants.CONST_Yes;

                    if ((item.PackagingComponent.Contains("Corrugated")) || (item.PackagingComponent.Contains("Paperboard")) ||
                        (item.PackagingComponent.Contains("Other")))
                        objOBMSecondReviewItem.NewCorrugatedPaperboardinBOM = GlobalConstants.CONST_Yes;
                }
            }
            objOBMSecondReviewItem.SGSExpeditedWorkflowApproved = this.ddlSGSExpeditedWorkflowApproved.SelectedItem.Text;

            return objOBMSecondReviewItem;
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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "PM Second Review");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.OBMReview2))
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
                OBMSecondReviewItem item = ConstructFormData();
                obmSecondReviewService.UpdatePMSecondReviewItem(item);

                GetuserControls();

                ApprovalItem approvalItem = ConstructApprovalData();
                obmSecondReviewService.UpdatePMSecondReviewApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.OBMReview2.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.OBMReview2.ToString(), "btnSave_Click");
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
                if (!userMgmtService.HasWriteAccess(CompassForm.OBMReview2))
                {
                    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
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
                OBMSecondReviewItem item = ConstructFormData();
                obmSecondReviewService.UpdatePMSecondReviewItem(item);

                GetuserControls();

                ApprovalItem approvalItem = ConstructApprovalData();
                obmSecondReviewService.UpdatePMSecondReviewApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.OBMReview2);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.OBMReview2.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.OBMReview2.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

    }
}
