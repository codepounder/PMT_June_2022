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
using System.Web.UI;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Linq;

namespace Ferrara.Compass.WebParts.OBMFirstReviewForm
{
    [ToolboxItemAttribute(false)]
    public partial class OBMFirstReviewForm : WebPart
    {
        #region Member Variables
        private IOBMFirstReviewService obmFirstReviewService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private IOPSService opsService;
        private IExternalManufacturingService externalMfgService;
        private IBOMSetupService BOMSetupService;
        private int iItemId = 0;
        private string PLMProject = "";
        private bool bContructDataErrors = false;
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
        public OBMFirstReviewForm()
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
            opsService = DependencyResolution.DependencyMapper.Container.Resolve<IOPSService>();
            externalMfgService = DependencyResolution.DependencyMapper.Container.Resolve<IExternalManufacturingService>();
            BOMSetupService = DependencyResolution.DependencyMapper.Container.Resolve<IBOMSetupService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.OBMReview1.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.OBMReview1.ToString(), "Page_Load");
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
            if (!userMgmtService.HasReadAccess(CompassForm.OBMReview1))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.OBMReview1))
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
        #endregion

        #region Data Transfer Methods
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
                    ctrl2.ProjectNumber = ProjectNumber;
                    ctrl2.packagingItems = dtPackingItem;
                    ctrl2.ParentId = item.Id;
                    ctrl2.GridItem = item;
                    //ctrl2.openBtnSave = openBtnSave;
                    phBOM.Controls.Add(ctrl2);
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
            //ctrl.openBtnSave = openBtnSave;
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
            foreach (var ctrl in phBOM.Controls)
            {
                if (ctrl is ucBOMGrid_New)
                {
                    var type2 = (ucBOMGrid_New)ctrl;

                    type2.saveData();

                }
                if (ctrl is ucBOMGrid)
                {
                    var type2 = (ucBOMGrid)ctrl;

                    type2.saveData();

                }
            }
        }
        private void LoadFormData()
        {
            hdnPageName.Value = GlobalConstants.PAGE_OBMFirstReview;
            hdnCompassListItemId.Value = iItemId.ToString();
            OBMFirstReviewItem item = obmFirstReviewService.GetPMFirstReviewItem(iItemId);
            ApprovalListItem approvedInfo = obmFirstReviewService.GetCompletedApprovalInfo(iItemId);
            OPSItem opsItem = opsService.GetOPSItem(iItemId);
            ExternalManufacturingItem extMfg = externalMfgService.GetExternalManufacturingItem(iItemId);
            hdnMaterialNumber.Value = item.MaterialNumber;
            hdnMaterialDesc.Value = item.MaterialDescriptiom;
            hdnPLMProject.Value = extMfg.PLMFlag;
            PLMProject = extMfg.PLMFlag;
            txtFirstProductionDate.Text = Utilities.GetDateForDisplay(item.FirstProductionDate);
            txtFirstShipRevisionComments.Text = item.RevisedFirstShipDateComments;
            txtOBMFirstReviewComments.Text = item.OBMFirstReviewComments;
            txtRevisedFirstShipDate.Text = Utilities.GetDateForDisplay(item.RevisedFirstShipDate);

            Utilities.SetDropDownValue(item.OBMFirstReviewCheck, this.ddlOBMFirstReviewConfirmation, this.Page);
            Utilities.SetDropDownValue(item.DoesFirstShipNeedRevision, this.ddlRevisedFirstShipDate, this.Page);

            lblAnnualProjectedDollars.Text = item.AnnualProjectedDollars.ToString();// Utilities.FormatCurrency(item.AnnualProjectedDollars);
            lblAnnualProjectedUnits.Text = Utilities.FormatDecimal(item.AnnualProjectedUnits, 0);
            lblBrand.Text = item.MaterialGroup1Brand;
            
            List<PackagingItem> dtPackingItem = packagingItemService.GetCandyAndPurchasedSemisForProject(iItemId);
            if (dtPackingItem.Count == 0)
            {
                rptCandiSemi.Visible = false;
                lblNoCandySemi.Visible = true;
            }
            else
            {
                rptCandiSemi.DataSource = dtPackingItem;
                rptCandiSemi.DataBind();
            }


            if (item.CaseUCC == "")
            {
                caseUCCDiv.Visible = false;
            }
            lblCaseUCC.Text = item.CaseUCC;

            lblCustomer.Text = item.Customer;
            //if (item.InitialCosting_GrossMarginAccurate == null || string.Equals(item.InitialCosting_GrossMarginAccurate.ToLower(), "no"))
            //    lblExpectedGrossMargin.Text = Utilities.FormatPercentage(item.RevisedGrossMarginPercent, 2);
            //else
            lblExpectedGrossMargin.Text = Utilities.FormatPercentage(item.ExpectedGrossMarginPercent, 2);

            lblFirstShipDate.Text = Utilities.GetDateForDisplay(item.RevisedFirstShipDate);
            lblJarDisplayUPC.Text = item.DisplayBoxUPC;
            lblLineOfBusiness.Text = item.ProductHierarchyLevel1;
            txtMakeLocation.Text = Utilities.GetDropDownDisplayValue(opsItem.MakeLocation);
            txtCountryOrigin.Text = Utilities.GetDropDownDisplayValue(opsItem.CountryOfOrigin);
            txtPackingLocation1.Text = Utilities.GetDropDownDisplayValue(opsItem.PackingLocation);
            if (item.PalletUCC == "")
            {
                palletUCCDiv.Visible = false;
            }
            lblPalletUCC.Text = item.PalletUCC;
            lblDesignateHUBDC.Text = item.DesignateHUBDC;
            lblProductHierarchyLevel2.Text = item.ProductHierarchyLevel2;
            lblProjectNotes.Text = item.ItemConcept;
            lblProjectType.Text = item.ProjectType;
            lblUnitUPC.Text = item.UnitUPC;
            lblWeeksUntilShip.Text = Utilities.DetermineWeeksToShip(item.RevisedFirstShipDate);

            string[] concerns = item.SectionsOfConcern.Split(',');
            foreach (string concern in concerns)
            {
                if (!string.IsNullOrEmpty(concern))
                    cblSectionConcerns.Items.FindByValue(concern).Selected = true;
            }

            // Check for External Manufacturing
            if ((string.Equals(item.PackingLocation, GlobalConstants.EXTERNAL_PACKER)) ||
                (string.Equals(item.MakeLocation, GlobalConstants.EXTERNAL_MANUFACTURER)))
            {
                lblCoManufacturingClassification.Text = item.CoManufacturingClassification;
            }
            else
            {
                dvExternalFields.Visible = false;
            }
            txtCapacityComments.Text = opsItem.InitialCapacity_CapacityRiskComments;
            txtProjectAcceptance.Text = opsItem.InitialCapacity_AcceptanceComments;
            int TSCount = packagingItemService.GetTransferPurchasedSemiItemsForProject(iItemId, GlobalConstants.COMPONENTTYPE_TransferSemi).Count();
            // Check for Internal Transfer Semi's
            if (TSCount <= 0)
            {
                dvTransferSemiHeader.Visible = false;
                dvTransferSemiRepeater.Visible = false;
                dvTransferSemiSeparator.Visible = false;
            }
            else
            {

                var packagingItems = GetTransferSemiPackagingItems();
                //    List<PackagingItem> dtPackingItem = new List<PackagingItem>();
                //var obj = new PackagingItem { NewExisting = "New", Id = 1, MaterialNumber = "100",
                //    MaterialDescription = "test",
                //    TransferSEMIMakePackLocations = "select", Notes = "test" };

                // dtPackingItem.Add(obj);

                if (packagingItems.Count == 0)
                {
                    rptTransferSemi.Visible = false;
                    lblNoTransferSemi.Visible = true;
                }
                else
                {
                    rptTransferSemi.DataSource = packagingItems;
                    rptTransferSemi.DataBind();
                }
            }
            if (!string.IsNullOrEmpty(approvedInfo.IPF_SubmittedDate))
            {
                ipfCompletionInfo.Controls.Add(new LiteralControl("&nbsp;- " + approvedInfo.IPF_SubmittedBy + ": " + approvedInfo.IPF_SubmittedDate));
            }
            if (!string.IsNullOrEmpty(approvedInfo.Operations_SubmittedDate))
            {
                makePackCompletionInfo.Controls.Add(new LiteralControl("&nbsp;- " + approvedInfo.Operations_SubmittedBy + ": " + approvedInfo.Operations_SubmittedDate));
            }
            if (!string.IsNullOrEmpty(approvedInfo.Distribution_SubmittedDate))
            {
                distributionCompletionInfo.Controls.Add(new LiteralControl("&nbsp;- " + approvedInfo.Distribution_SubmittedBy + ": " + approvedInfo.Distribution_SubmittedDate));
            }
            if (!string.IsNullOrEmpty(approvedInfo.QA_SubmittedDate))
            {
                QACompletionInfo.Controls.Add(new LiteralControl("&nbsp;- " + approvedInfo.QA_SubmittedBy + ": " + approvedInfo.QA_SubmittedDate));
            }
            if (!string.IsNullOrEmpty(approvedInfo.SAPInitialSetup_SubmittedDate))
            {
                InitialSetupCompletionInfo.Controls.Add(new LiteralControl("&nbsp;- " + approvedInfo.SAPInitialSetup_SubmittedBy + ": " + approvedInfo.SAPInitialSetup_SubmittedDate));
            }

            // Check for External Manufacturing
            if ((string.Equals(item.PackingLocation, GlobalConstants.EXTERNAL_PACKER)) ||
                (string.Equals(item.MakeLocation, GlobalConstants.EXTERNAL_MANUFACTURER)))
            {
                txtCurrentTimelineAcceptable.Text = extMfg.CurrentTimelineAcceptable;
                txtLeadTimeFromSupplier.Text = extMfg.LeadTimeFromSupplier;
                txtFinalArtworkDueToSupplier.Text = Utilities.GetDateForDisplay(extMfg.FinalArtworkDueToSupplier);
            }
            else
            {
                dvExternalManufacturing.Visible = false;
            }

            LoadAttachments();
        }
        private OBMFirstReviewItem ConstructFormData()
        {
            try { GetuserControls(); } catch (Exception error) { }
            var item = new OBMFirstReviewItem();
            string strErrors = string.Empty;
            bContructDataErrors = false;

            item.CompassListItemId = iItemId;
            item.RevisedFirstShipDateComments = txtFirstShipRevisionComments.Text.Trim();
            item.OBMFirstReviewComments = txtOBMFirstReviewComments.Text.Trim();
            item.OBMFirstReviewCheck = this.ddlOBMFirstReviewConfirmation.SelectedItem.Text;
            //item.ProjectStatus = this.ddlProjectStatus.SelectedItem.Text;
            item.DoesFirstShipNeedRevision = this.ddlRevisedFirstShipDate.SelectedItem.Text;

            item.RevisedFirstShipDate = Utilities.GetDateFromField(txtRevisedFirstShipDate.Text.Trim());
            if ((string.Equals(item.RevisedFirstShipDate, Utilities.GetMinDate())) && (!string.IsNullOrEmpty(txtRevisedFirstShipDate.Text.Trim())))
            {
                // Invalid Date was entered
                strErrors = "Invalid Revised First Ship Date was entered. Please re-enter.<a href='javascript:setFocus(&quot;txtRevisedFirstShipDate&quot;)'>  [Update]</a>";
                ErrorSummary.AddError(strErrors, this.Page);
                bContructDataErrors = true;
            }
            item.FirstProductionDate = Utilities.GetDateFromField(txtFirstProductionDate.Text.Trim());
            if ((string.Equals(item.FirstProductionDate, Utilities.GetMinDate())) && (!string.IsNullOrEmpty(txtFirstProductionDate.Text.Trim())))
            {
                // Invalid Date was entered
                strErrors = "Invalid First Production Date was entered. Please re-enter.<a href='javascript:setFocus(&quot;txtFirstProductionDate&quot;)'>  [Update]</a>";
                ErrorSummary.AddError(strErrors, this.Page);
                bContructDataErrors = true;
            }

            item.SectionsOfConcern = string.Empty;
            foreach (ListItem li in cblSectionConcerns.Items)
            {
                if (li.Selected == true)
                {
                    item.SectionsOfConcern = item.SectionsOfConcern + li.Value + ",";
                }
            }

            item.LastUpdatedFormName = CompassForm.OBMReview1.ToString();

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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "PM First Review");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        public void openBtnSave()
        {
            if (!userMgmtService.HasWriteAccess(CompassForm.OBMReview1))
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
            OBMFirstReviewItem item = ConstructFormData();
            obmFirstReviewService.UpdatePMFirstReviewItem(item);
            //workflowService.UpdateWorkflowTaskFirstShipDate(iItemId, item.RevisedFirstShipDate.ToString());

            ApprovalItem approvalItem = ConstructApprovalData();
            obmFirstReviewService.UpdatePMFirstReviewApprovalItem(approvalItem, false);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                openBtnSave();

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.OBMReview1.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.OBMReview1.ToString(), "btnSave_Click");
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
                if (!userMgmtService.HasWriteAccess(CompassForm.OBMReview1))
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }


                // Retrieve the data from the form
                OBMFirstReviewItem item = ConstructFormData();
                var iserror = RequiredFieldCheckForPackagingComponent();

                if (iserror)
                {
                    ItemValidationSummary.HeaderText = "<strong><p style='font-size:medium'>Submit Failed:</p></strong><br/>";
                    return;
                }
                if (bContructDataErrors)
                    return;
                obmFirstReviewService.UpdatePMFirstReviewItem(item);
                //workflowService.UpdateWorkflowTaskFirstShipDate(iItemId, item.RevisedFirstShipDate.ToString());

                ApprovalItem approvalItem = ConstructApprovalData();
                obmFirstReviewService.UpdatePMFirstReviewApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.OBMReview1);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.OBMReview1.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.OBMReview1.ToString(), "btnSubmit_Click");
            }
        }
        #endregion
        private void LoadAttachments()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_NLEA);
            if (files.Count > 0)
            {
                rpAttachments.Visible = true;
                rpAttachments.DataSource = files;
                rpAttachments.DataBind();
            }
            else
            {
                rpAttachments.Visible = false;
                noNLEAMessage.Visible = true;
            }
        }
    }
}
