using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.PMFirstReviewForm
{
    [ToolboxItemAttribute(false)]
    public partial class PMFirstReviewForm : WebPart
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
        private IItemProposalService IPFService;
        private int iItemId = 0;
        int PackagingItemId = 0;
        private string PLMProject = "";
        private bool bContructDataErrors = false;
        private string webUrl;
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
        private string ComponentTab
        {
            get
            {
                if (Page.Request.QueryString[GlobalConstants.QUERYSTRING_ComponentTab] != null)
                    return Page.Request.QueryString[GlobalConstants.QUERYSTRING_ComponentTab];
                return string.Empty;
            }
        }
        #endregion
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public PMFirstReviewForm()
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
            IPFService = DependencyResolution.DependencyMapper.Container.Resolve<IItemProposalService>();
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

                    BindCompEditDropdowns();
                    LoadFormData();
                    InitializeScreen();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.PMReview1.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.PMReview1.ToString(), "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
                PLMProject = hdnPLMProject.Value;
            }
            LoadBOMGrid();
        }
        #region Data Transfer Methods
        private void LoadFormData()
        {
            hdnPageName.Value = GlobalConstants.PAGE_PMFirstReview;
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

            item.LastUpdatedFormName = CompassForm.PMReview1.ToString();

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
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.PMReview1.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.PMReview1.ToString(), "btnSave_Click");
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
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.PMReview1.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.PMReview1.ToString(), "btnSubmit_Click");
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
        #region BOM Grid
        private void LoadBOMGrid()
        {
            List<BOMSetupItem> dtPackingItems = new List<BOMSetupItem>();
            dtPackingItems = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;

            foreach (var PackingItem in dtPackingItems)
            {
                PackingItem.CompStatus = requirementsCheckIPF(PackingItem) ? "Green" : "Red";
            }
            BindddlComponentTab(dtPackingItems);
            if (!string.IsNullOrEmpty(ComponentTab))
            {
                ddlComponentTab.SelectedValue = ComponentTab;
            }
            LiteralBOMGridTableData.Text = "<script> var BOMGridTableData=" + js.Serialize(dtPackingItems) + "</script>";
        }
        private void BindddlComponentTab(List<BOMSetupItem> dtPackingItems)
        {
            int Level = 1;
            dtPackingItems
                .Where(t => t.ParentID == 0)
                .Select(S => { S.Level = 1; return S; }).ToList();

            while ((dtPackingItems.Count(t => t.Level == Level)) > 0)
            {
                foreach (var item in dtPackingItems.Where(t => t.Level == Level))
                {
                    dtPackingItems
                        .Where(t => t.ParentID == item.Id)
                        .Select(S => { S.Level = Level + 1; return S; }).ToList();
                }
                Level++;
            }

            Level = 1;
            var sortedList = new List<BOMSetupItem>();
            while ((dtPackingItems.Count(t => t.Level == Level)) > 0)
            {
                foreach (var item in dtPackingItems.Where(t => t.Level == Level))
                {
                    if (Level == 1) sortedList.Add(item);
                    int index = sortedList.FindIndex(t => t.Id == item.Id);
                    sortedList.InsertRange(index + 1, (from semi in dtPackingItems where semi.ParentID == item.Id select semi).ToList());
                }
                Level++;
            }

            var fgcount =
                  (
                       from
                           dtPackingItem in dtPackingItems
                       where
                           dtPackingItem.ParentID == 0
                       select
                           dtPackingItem.ParentID
                   ).Count();

            var redfgCount =
                   (
                        from
                            dtPackingItem in dtPackingItems
                        where
                            dtPackingItem.ParentID == 0
                            &&
                            dtPackingItem.CompStatus == "Red"
                        select
                            dtPackingItem.ParentID
                    ).Count();

            List<Tuple<string, string, string>> CompTabItems = new List<Tuple<string, string, string>>();
            CompTabItems.Add(new Tuple<string, string, string>("0", string.Concat("Finished Good - ", "(", fgcount.ToString(), ")"), (redfgCount == 0) ? "#A5C26A" : "#CA6A68"));

            foreach (var PackingItem in sortedList.Where(t => (t.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || t.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)))
            {
                var count =
                   (
                        from
                            dtPackingItem in dtPackingItems
                        where
                            dtPackingItem.ParentID == PackingItem.Id
                        select
                            dtPackingItem.ParentID
                    ).Count();

                var redCount =
                   (
                        from
                            dtPackingItem in dtPackingItems
                        where
                            dtPackingItem.ParentID == PackingItem.Id
                            &&
                            dtPackingItem.CompStatus == "Red"
                        select
                            dtPackingItem.ParentID
                    ).Count();

                CompTabItems.Add(new Tuple<string, string, string>(PackingItem.Id.ToString(), string.Concat(new string('-', PackingItem.Level * 5), PackingItem.PackagingComponent, " - ", PackingItem.MaterialNumber, " - ", PackingItem.MaterialDescription, " (", count.ToString(), ")"), (redCount == 0) ? "#A5C26A" : "#CA6A68"));
            }

            ddlComponentTab.Items.Clear();

            foreach (var CompTabItem in CompTabItems)
            {
                Utilities.AddItemToDropDownWithAttributes(ddlComponentTab, CompTabItem.Item2, CompTabItem.Item1, "style", string.Concat("background-color: ", CompTabItem.Item3));
            }
        }
        protected void btnDeleteComponent_Click(object sender, EventArgs e)
        {
            List<int> deletedIds = Utilities.GetIntegerArrayFromDelimittedString(hdnDeletedCompIds.Value, ';');

            if (deletedIds.Count > 0)
            {
                BOMSetupService.DeleteBOMSetupItems(deletedIds);
                hdnDeletedCompIds.Value = "";
                LoadBOMGrid();
            }
        }
        private bool requirementsCheckIPF(BOMSetupItem item)
        {
            bool completed = true;
            //Rules for all components
            if (item.NewExisting == "Select..." || item.NewExisting == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent == "Select..." || item.PackagingComponent == "")
            {
                completed = false;
            }
            else if (item.PackUnit == "Select..." || item.PackUnit == "")
            {
                completed = false;
            }
            else if (item.PackQuantity == "Select..." || item.PackQuantity == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItem == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialNumber == "")
            {
                completed = false;
            }
            else if (item.Flowthrough == "" || item.Flowthrough == "Select...")
            {
                completed = false;
            }
            else if (item.NewExisting == "New" && item.CurrentLikeItemDescription == "")
            {
                completed = false;
            }
            else if (item.NewExisting == "Existing" && item.MaterialDescription == "")
            {
                completed = false;
            }
            else if (item.PackagingComponent != GlobalConstants.COMPONENTTYPE_CandySemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_TransferSemi && item.PackagingComponent != GlobalConstants.COMPONENTTYPE_PurchasedSemi && !item.PackagingComponent.Contains("Finished Good"))
            {
                if (item.NewExisting == "New" && item.CurrentLikeItemReason == "")
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "Select..." || item.ComponentContainsNLEA == "Select...")
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "" || item.ComponentContainsNLEA == "")
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "Yes" && (item.ExternalGraphicsVendor == "Select..." || item.ExternalGraphicsVendor == ""))
                {
                    completed = false;
                }
                else if (item.GraphicsChangeRequired == "Yes" && item.GraphicsBrief == "")
                {
                    completed = false;
                }
            }
            else if (item.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi || item.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
            {
                if (item.NewExisting == "New")
                {
                    if (string.IsNullOrEmpty(item.PHL1) || item.PHL1 == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.PHL2) || item.PHL2 == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.Brand) || item.Brand == "Select...")
                    {
                        completed = false;
                    }
                    else if (string.IsNullOrEmpty(item.ProfitCenter))
                    {
                        completed = false;
                    }
                }
            }
            return completed;
        }
        #endregion
        #region Component Details - Popup
        private void BindCompEditDropdowns()
        {
            try
            {
                using (SPSite site = new SPSite(webUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        Utilities.BindDropDownItems(drpPkgComponent, GlobalConstants.LIST_PackagingComponentTypesLookup, web.Url);
                        Utilities.BindDropDownItems(ddlPrinter, GlobalConstants.LIST_PrinterSupplierLookup, web.Url);
                        Utilities.BindDropDownItems(ddlPackUnit, GlobalConstants.LIST_PackUnitLookup, web.Url);
                        Utilities.BindDropDownItems(ddlFlowthrough, GlobalConstants.LIST_FlowThroughTypeLookup, webUrl);
                        Utilities.BindDropDownItemsById(ddlPurchasedIntoLocation, GlobalConstants.LIST_CompPurchasedIntoLocationsLookup, webUrl);

                        Utilities.BindDropDownItems(drpTSCountryOfOrigin, GlobalConstants.LIST_ManufacturerCountryOfOrigin, web.Url);
                        Utilities.BindDropDownWithTitleFilter(drpTransferLocation, GlobalConstants.LIST_MakePackTransfersLookup, webUrl, false, "PURCHASED");
                        Utilities.BindDropDownWithTitleFilter(drpTSPackLocation, GlobalConstants.LIST_PackingLocationsLookup, webUrl, false, "External");
                        Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);

                        Utilities.BindDropDownItems(drpPCSCountryofOrigin, GlobalConstants.LIST_ManufacturerCountryOfOrigin, web.Url);
                        Utilities.BindDropDownWithTitleFilter(drpPurchasedCandyLocation, GlobalConstants.LIST_MakePackTransfersLookup, webUrl, true, "PURCHASED");
                        Utilities.BindDropDownItems(drpPCSPackLocation, GlobalConstants.LIST_CoPackers, webUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                exceptionService.Handle(LogCategory.CriticalError, ex, "PMSecondReview", "btnPopulateComponent_Click");
            }
        }
        private void LoadControlData()
        {
            try
            {
                PackagingItemId = Convert.ToInt32(hdnPackagingItemIdClicked.Value);

                var AllPIs = BOMSetupService.GetAllBOMSetupItemsForProject(iItemId);

                if (PackagingItemId > 0)
                {
                    hdnPackagingItemId.Value = PackagingItemId.ToString();
                    var ipfItem = IPFService.GetItemProposalItem(iItemId);
                    List<KeyValuePair<int, string>> PCSitems = packagingItemService.GetPurchasedSemiIDsForProject(iItemId);

                    hdnProductHierarchyLevel1.Value = ipfItem.ProductHierarchyLevel1;
                    hdnNovelyProject.Value = ipfItem.NovelyProject;
                    hdnExtMfgkickedoff.Value = "";
                    hdnCoManClassification.Value = ipfItem.CoManClassification;
                    if (ipfItem.ManufacturingLocation == "Externally Manufactured" || ipfItem.PackingLocation == "Externally Packed" || PCSitems.Count > 0)
                    {
                        hdnExtMfgkickedoff.Value = "Yes";
                    }

                    dvMain.Visible = true;

                    var packagingItem = (from PI in AllPIs where PI.Id == PackagingItemId select PI).FirstOrDefault();
                    PackagingItem parentComoponent = new PackagingItem();
                    if (packagingItem.ParentID != 0)
                    {
                        parentComoponent = packagingItemService.GetPackagingItemByPackagingId(packagingItem.ParentID);
                        hdnParentType.Value = parentComoponent.PackagingComponent;
                    }

                    Utilities.SetDropDownValue(packagingItem.PackagingComponent, this.drpPkgComponent, this.Page);
                    Utilities.SetDropDownValue(packagingItem.PackUnit, this.ddlPackUnit, this.Page);
                    Utilities.SetDropDownValue(packagingItem.NewExisting, this.drpNew, this.Page);
                    Utilities.SetDropDownValue(packagingItem.Flowthrough, this.ddlFlowthrough, this.Page);
                    Utilities.SetDropDownValue(packagingItem.PurchasedIntoLocation, this.ddlPurchasedIntoLocation, this.Page);

                    Utilities.SetDropDownValue(packagingItem.ReviewPrinterSupplier, this.ddlReviewPrinterSupplier, this.Page);
                    Utilities.BindDropDownItemsWithClass(ddlFilmPrintStyle, GlobalConstants.LIST_PrintStyleLookup, SPContext.Current.Web.Url);
                    if (packagingItem.PackagingComponent.ToLower().Contains("corrugated") || packagingItem.PackagingComponent.ToLower().Contains("paperboard"))
                    {
                        Utilities.SetDropDownValue(packagingItem.CorrugatedPrintStyle, this.ddlFilmPrintStyle, this.Page);
                    }
                    else
                    {
                        Utilities.SetDropDownValue(packagingItem.FilmPrintStyle, this.ddlFilmPrintStyle, this.Page);
                    }


                    if (string.IsNullOrEmpty(packagingItem.SAPMaterialGroup))
                    {
                        string componantType = Utilities.GetLookupValue(GlobalConstants.LIST_PackagingComponentTypesLookup, packagingItem.PackagingComponent, webUrl);
                        hdnSAPMatGroup.Value = componantType;
                    }
                    else
                    {
                        hdnSAPMatGroup.Value = packagingItem.SAPMaterialGroup;
                    }
                    if (packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || packagingItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                    {
                        int childCount = (from PI in AllPIs where PI.ParentID == packagingItem.Id select PI).Count();
                        if (childCount > 0)
                        {
                            this.drpPkgComponent.Enabled = false;
                            this.lblCompNote.Visible = true;
                        }
                    }
                    hdnParentID.Value = packagingItem.ParentID.ToString();
                    hdnTBDIndicator.Value = ipfItem.TBDIndicator;
                    txtMaterial.Text = packagingItem.MaterialNumber;
                    hdnLOB.Value = ipfItem.ProductHierarchyLevel1;
                    txtMaterialDescription.Text = packagingItem.MaterialDescription; // ?? Is this mapping correct
                    txtLikeItem.Text = packagingItem.CurrentLikeItem; // ?? Is this mapping correct
                    txtLikeDescription.Text = packagingItem.CurrentLikeItemDescription; // ?? Is this mapping correct
                    txtGraphicsBrief.Text = packagingItem.GraphicsBrief;
                    txtLikeMaterial.Text = packagingItem.CurrentLikeItemReason;
                    txtOldMaterial.Text = packagingItem.CurrentOldItem;
                    txtOldMaterialDesc.Text = packagingItem.CurrentOldItemDescription;
                    #region Transfer Semi Barcode Generation
                    txt13DigitCode.Text = packagingItem.ThirteenDigitCode;
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()) || string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_OBMSecondReview.ToLower()))
                    {
                        dvSAPDescAbrev.Visible = true;
                        Utilities.BindDropDownPassColumnNames(ddlSAPDescAbrev, GlobalConstants.LIST_PackagingComponentTypesLookup, webUrl, "Title", "SAPDescription");
                        Utilities.SetDropDownValueById(packagingItem.SAPDescAbbrev, this.ddlSAPDescAbrev, this.Page);
                        bool TransferSemiBarcodeGenerationVisibile = false;
                        if (packagingItem.ParentID != 0)
                        {
                            if (parentComoponent != null)
                            {
                                if (parentComoponent.PackagingComponent.ToLower().Contains("transfer") && (parentComoponent.PackLocation.Contains("FQ22") || parentComoponent.PackLocation.Contains("FQ25")))
                                {
                                    TransferSemiBarcodeGenerationVisibile = true;
                                    hdnTSBarcodeGenerationVisibility.Value = "Yes";
                                    hdn13DigitCode.Value = string.Concat("1000000", parentComoponent.MaterialNumber);
                                }
                            }
                        }

                        if (packagingItem.PackagingComponent.ToLower().Contains("corrugated") && packagingItem.NewExisting.ToLower() == "new" && TransferSemiBarcodeGenerationVisibile)
                        {
                            txt13DigitCode.Text = string.Concat("1000000", parentComoponent.MaterialNumber);
                        }
                    }

                    txt14DigitBarcode.Text = packagingItem.FourteenDigitBarcode;
                    #endregion
                    txtShelfLife.Text = packagingItem.ShelfLife;
                    Utilities.SetDropDownValue(packagingItem.TrialsCompleted, this.drpTrialsCompleted, this.Page);
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPCompleteItemSetup.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupSAP.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_SAPBOMSetup.ToLower()))
                    {
                        divTrialsCompleted.Visible = false;
                    }
                    Utilities.SetDropDownValue(packagingItem.NewFormula, this.ddlNewFormula, this.Page);
                    if (this.drpTSCountryOfOrigin.Items.FindByText(packagingItem.CountryOfOrigin) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.CountryOfOrigin, this.drpTSCountryOfOrigin, this.Page);
                    }
                    if (this.drpTSPackLocation.Items.FindByText(packagingItem.PackLocation) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.PackLocation, this.drpTSPackLocation, this.Page);
                    }
                    if (this.drpTransferLocation.Items.FindByText(packagingItem.TransferSEMIMakePackLocations) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.TransferSEMIMakePackLocations, this.drpTransferLocation, this.Page);
                    }
                    if (this.drpPCSCountryofOrigin.Items.FindByText(packagingItem.CountryOfOrigin) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.CountryOfOrigin, this.drpPCSCountryofOrigin, this.Page);
                    }
                    if (this.drpPCSPackLocation.Items.FindByText(packagingItem.PackLocation) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.PackLocation, this.drpPCSPackLocation, this.Page);
                    }
                    if (this.drpPurchasedCandyLocation.Items.FindByText(packagingItem.TransferSEMIMakePackLocations) != null)
                    {
                        Utilities.SetDropDownValue(packagingItem.TransferSEMIMakePackLocations, this.drpPurchasedCandyLocation, this.Page);
                    }

                    Utilities.SetDropDownValue(packagingItem.ComponentContainsNLEA, this.drpComponentContainsNLEA, this.Page);

                    txtSpecificationNo.Text = packagingItem.SpecificationNo;
                    txtPackQty.Text = packagingItem.PackQuantity == "-9999" ? "" : packagingItem.PackQuantity;
                    txtLeadTimeMaterial.Text = packagingItem.LeadMaterialTime;

                    if (packagingItem.PackagingComponent.ToLower().Contains("transfer semi") || packagingItem.PackagingComponent.ToLower().Contains("purchased"))
                        txtSEMIComment.Text = packagingItem.Notes;

                    Utilities.BindDropDownItems(ddlGraphicsVendor, GlobalConstants.LIST_ExternalGraphicsVendorLookup, webUrl);
                    if (!string.IsNullOrEmpty(packagingItem.ExternalGraphicsVendor))
                    {
                        Utilities.SetDropDownValue(packagingItem.ExternalGraphicsVendor, ddlGraphicsVendor, this.Page);
                    }

                    if (!string.IsNullOrEmpty(packagingItem.GraphicsChangeRequired))
                    {
                        Utilities.SetDropDownValue(packagingItem.GraphicsChangeRequired, drpGraphicsNeeded, this.Page);
                    }
                    if (string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE2.ToLower()) ||
                    string.Equals(Utilities.GetCurrentPageName().ToLower(), GlobalConstants.PAGE_BOMSetupPE3.ToLower()))
                    {
                        drpGraphicsNeeded.Attributes.Add("onchange", "GraphicsCheck(this);");
                    }
                    else
                    {
                        drpGraphicsNeeded.Attributes.Add("onchange", "GraphicsCheckLoadSpec(this);");
                    }
                    Utilities.SetDropDownValue(packagingItem.IsAllProcInfoCorrect, this.ddlIsAllProcInfoCorrect, this.Page);
                    txtWhatProcInfoHasChanged.Text = packagingItem.WhatProcInfoHasChanged;

                    GetAttachments();

                    dvMoveTS.Visible = false;

                    if (!string.IsNullOrEmpty(packagingItem.PrinterSupplier))
                    {
                        try
                        {
                            ddlPrinter.ClearSelection();
                            ddlPrinter.Items.FindByText(packagingItem.PrinterSupplier).Selected = true;
                        }
                        catch (Exception ex)
                        {
                            ddlPrinter.Items.Add(packagingItem.PrinterSupplier);
                            ddlPrinter.Items.FindByText(packagingItem.PrinterSupplier).Selected = true;
                            ddlPrinter.BackColor = System.Drawing.Color.Pink;
                            this.lblPrinterError.Text = "'" + packagingItem.PrinterSupplier + "' is not a valid Printer/Supplier! Please select or request a new one.";
                        }
                    }

                    if (packagingItem.DielineURL != "")
                    {
                        generatedLinkEdit.HRef = packagingItem.DielineURL;
                        string title = packagingItem.ParentID == 0 ? "Finished Good" : ddlComponentTab.SelectedItem.Text.Split('-')[0];
                        title = title + ": " + packagingItem.MaterialNumber + ": Dieline";
                        generatedLinkEdit.InnerText = title;
                        txtDielineLinkEdit.Text = packagingItem.DielineURL;
                        generatedLinkEdit.Attributes.Remove("class");
                    }
                    try
                    {
                        Utilities.BindDropDownItemsPHL1(ddlPHL1, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                        Utilities.BindDropDownItemsPHL2(ddlPHL2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, webUrl);
                        Utilities.BindDropDownItemsBrand(ddlBrand, GlobalConstants.LIST_MaterialGroup1Lookup, webUrl);
                        Utilities.SetDropDownValue(packagingItem.PHL1, ddlPHL1, this.Page);
                        Utilities.SetDropDownValue(packagingItem.PHL2, ddlPHL2, this.Page);
                        Utilities.SetDropDownValue(packagingItem.Brand, ddlBrand, this.Page);
                        txtProfitCenterUC.Text = packagingItem.ProfitCenter;
                        hdnProfitCenterUC.Value = packagingItem.ProfitCenter;
                    }
                    catch (Exception e)
                    {
                        LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Hierarchy Error: " + e.Message);
                        exceptionService.Handle(LogCategory.CriticalError, e, "Hierarchy Error", "Hierarchy Error");
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ShowCompDetailsForm", "ShowCompDetailsForm();", true);
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "LoadFormData");
            }
        }
        protected void btnSaveValidate_Click(object sender, EventArgs e)
        {
            SaveComponent();
        }
        protected void btnSaveComp_Click(object sender, EventArgs e)
        {
            SaveComponent();
        }
        private void SaveComponent()
        {
            try
            {
                var item = ConstructCompData();
                PackagingItemId = Convert.ToInt32(hdnPackagingItemIdClicked.Value);

                if (PackagingItemId > 0)
                {
                    BOMSetupService.UpdateBOMSetupItem(item);
                }
                else
                {
                    item.Id = BOMSetupService.InsertBOMSetupItem(item);
                }

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_ProjectNo, ProjectNumber),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_ComponentTab, hdnParentID.Value)
                };

                Page.Response.Redirect(Utilities.RedirectPageValue(Utilities.GetCurrentPageName(), parameters), false);
            }
            catch (Exception exception)
            {

                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "btnSubmit_Click");
            }

        }
        private BOMSetupItem ConstructCompData()
        {
            var bomSetupItem = new BOMSetupItem();

            try
            {
                // If this is an existing item, get the current values
                PackagingItemId = Convert.ToInt32(hdnPackagingItemIdClicked.Value);
                if (PackagingItemId > 0)
                {
                    bomSetupItem = BOMSetupService.GetBOMSetupItemByComponentId(PackagingItemId);
                }
                bomSetupItem.Id = PackagingItemId;
                bomSetupItem.CompassListItemId = iItemId;
                string packcomp = this.drpPkgComponent.SelectedItem.Text;
                bomSetupItem.PackagingComponent = packcomp;
                bomSetupItem.PackUnit = this.ddlPackUnit.SelectedItem.Text;
                bomSetupItem.NewExisting = drpNew.SelectedItem.Text;
                bomSetupItem.MaterialNumber = txtMaterial.Text;
                bomSetupItem.MaterialDescription = txtMaterialDescription.Text;
                bomSetupItem.CurrentLikeItem = txtLikeItem.Text;
                bomSetupItem.CurrentLikeItemDescription = txtLikeDescription.Text;
                bomSetupItem.CurrentOldItem = txtOldMaterial.Text;
                bomSetupItem.CurrentOldItemDescription = txtOldMaterialDesc.Text;
                bomSetupItem.TrialsCompleted = this.drpTrialsCompleted.SelectedItem.Text;
                bomSetupItem.NewFormula = this.ddlNewFormula.SelectedItem.Text;
                bomSetupItem.ShelfLife = this.txtShelfLife.Text;
                #region Transfer Semi Barcode Generation
                bomSetupItem.ThirteenDigitCode = txt13DigitCode.Text;
                bomSetupItem.FourteenDigitBarcode = txt14DigitBarcode.Text;
                #endregion
                if (packcomp == GlobalConstants.COMPONENTTYPE_PurchasedSemi)
                {
                    bomSetupItem.CountryOfOrigin = this.drpPCSCountryofOrigin.SelectedItem.Text;
                    bomSetupItem.PackLocation = this.drpPCSPackLocation.SelectedItem.Text;
                    bomSetupItem.TransferSEMIMakePackLocations = this.drpPurchasedCandyLocation.SelectedItem.Text;
                }
                else
                {
                    bomSetupItem.CountryOfOrigin = this.drpTSCountryOfOrigin.SelectedItem.Text;
                    bomSetupItem.PackLocation = this.drpTSPackLocation.SelectedItem.Text;
                    bomSetupItem.TransferSEMIMakePackLocations = this.drpTransferLocation.SelectedItem.Text;
                }

                bomSetupItem.SpecificationNo = txtSpecificationNo.Text;
                bomSetupItem.PurchasedIntoLocation = this.ddlPurchasedIntoLocation.SelectedItem.Text;

                bomSetupItem.PackQuantity = txtPackQty.Text;
                bomSetupItem.LeadMaterialTime = txtLeadTimeMaterial.Text;
                if (bomSetupItem.PackagingComponent.ToLower().Contains("transfer semi") || bomSetupItem.PackagingComponent.ToLower().Contains("purchased candy"))
                    bomSetupItem.Notes = txtSEMIComment.Text;
                bomSetupItem.PrinterSupplier = ddlPrinter.SelectedItem.Text;
                if (PackagingItemId <= 0)
                {
                    string componantType = Utilities.GetLookupValue(GlobalConstants.LIST_PackagingComponentTypesLookup, bomSetupItem.PackagingComponent, webUrl);
                    bomSetupItem.SAPMaterialGroup = componantType;
                }
                else
                {
                    bomSetupItem.SAPMaterialGroup = hdnSAPMatGroup.Value;
                }


                bomSetupItem.ParentID = Convert.ToInt32(hdnParentID.Value);

                bomSetupItem.GraphicsBrief = txtGraphicsBrief.Text;
                bomSetupItem.GraphicsChangeRequired = drpGraphicsNeeded.SelectedItem.Text;
                bomSetupItem.ExternalGraphicsVendor = ddlGraphicsVendor.SelectedItem.Text;
                bomSetupItem.ComponentContainsNLEA = drpComponentContainsNLEA.SelectedItem.Text;
                bomSetupItem.CurrentLikeItemReason = txtLikeMaterial.Text;
                bomSetupItem.Flowthrough = ddlFlowthrough.SelectedItem.Text;
                bomSetupItem.ReviewPrinterSupplier = ddlReviewPrinterSupplier.SelectedItem.Text;
                bomSetupItem.IsAllProcInfoCorrect = ddlIsAllProcInfoCorrect.SelectedItem.Text;
                bomSetupItem.WhatProcInfoHasChanged = txtWhatProcInfoHasChanged.Text;
                if (bomSetupItem.PackagingComponent.ToLower().Contains("film"))
                {
                    bomSetupItem.FilmPrintStyle = ddlFilmPrintStyle.SelectedItem.Text;
                    bomSetupItem.CorrugatedPrintStyle = "";
                }
                else if (bomSetupItem.PackagingComponent.ToLower().Contains("corrugated") || bomSetupItem.PackagingComponent.ToLower().Contains("paperboard"))
                {
                    bomSetupItem.CorrugatedPrintStyle = ddlFilmPrintStyle.SelectedItem.Text;
                    bomSetupItem.FilmPrintStyle = "";
                }
                if (bomSetupItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_PurchasedSemi || bomSetupItem.PackagingComponent == GlobalConstants.COMPONENTTYPE_TransferSemi)
                {
                    bomSetupItem.PHL1 = ddlPHL1.SelectedItem.Text;
                    bomSetupItem.PHL2 = ddlPHL2.SelectedItem.Text;
                    bomSetupItem.Brand = ddlBrand.SelectedItem.Text;
                    bomSetupItem.ProfitCenter = txtProfitCenterUC.Text;
                    bomSetupItem.ProfitCenter = hdnProfitCenterUC.Value;
                }
                string movedPackType = ddlMoveTS.SelectedItem.Text;
                int moveId = Convert.ToInt32(ddlMoveTS.SelectedItem.Value);
                if (moveId != -1)
                {
                    bomSetupItem.ParentID = moveId;
                }
                //New Fields
                bomSetupItem.DielineURL = txtDielineLinkEdit.Text;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "Packaging Component: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PackagingComponentEntryForm", "ConstructFormData");
            }

            return bomSetupItem;
        }
        protected void btnPopulateComponent_Click(object sender, EventArgs e)
        {
            LoadControlData();
        }
        #region Attachment Methods
        protected void lnkDeleteAttachment_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");
            hdnPageState.Value = "PE";
        }
        private void GetAttachments()
        {
            PackagingItemId = Convert.ToInt32(hdnPackagingItemIdClicked.Value);
            var files = BOMSetupService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Dieline);

            if (files.Count > 0)
            {
                rptDieline.Visible = true;
                rptDieline.DataSource = files;
                rptDieline.DataBind();
            }
            else
            {
                rptDieline.Visible = false;
            }

            #region Approved Graphics Asset
            var ApprovedGraphicsAssets = BOMSetupService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_ApprovedGraphicsAsset);

            if (ApprovedGraphicsAssets.Count > 0)
            {
                btnApprovedGraphicsAsset.Visible = false;
                rptApprovedGraphicsAsset.Visible = true;
                rptApprovedGraphicsAsset.DataSource = ApprovedGraphicsAssets;
                rptApprovedGraphicsAsset.DataBind();
            }
            else
            {
                rptApprovedGraphicsAsset.Visible = false;
                btnApprovedGraphicsAsset.Visible = true;
            }
            #endregion
        }
        protected void lnkDeleteApprovedGraphicsAsset_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
            var hdnPageState = (HiddenField)this.Parent.FindControl("hdnPageState");
            hdnPageState.Value = "PE";
        }
        private void BindAttachments(HtmlAnchor anc, FileUpload fld, HtmlAnchor anc1, FileUpload fld1, LinkButton lnk, LinkButton lnk1)
        {
            PackagingItemId = Convert.ToInt32(hdnPackagingItemIdClicked.Value);

            var files = BOMSetupService.GetUploadedFiles(ProjectNumber, PackagingItemId, GlobalConstants.DOCTYPE_Dieline);
            lnk.Visible = lnk1.Visible = false;
            if (files.Count > 0)
            {
                anc.InnerText = files[0].FileName;
                anc.HRef = files[0].FileUrl;
                fld.Visible = false;
                lnk.Visible = true;
                lnk.CommandName = files[0].FileUrl;

                if (files.Count > 1)
                {
                    anc1.InnerText = files[1].FileName;
                    anc1.HRef = files[1].FileUrl;
                    fld1.Visible = false;
                    lnk1.Visible = true;
                    lnk1.CommandName = files[1].FileUrl;
                }
            }
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }
        #endregion
        #endregion
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
    }
}
