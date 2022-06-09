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
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Web.UI.WebControls;

namespace Ferrara.Compass.WebParts.SecondaryApprovalReviewForm
{
    [ToolboxItemAttribute(false)]
    public partial class SecondaryApprovalReviewForm : WebPart
    {
        #region Member Variables
        private ISecondaryApprovalReviewService secondaryApprovalReviewService;
        private IInitialCostingReviewService costingReviewService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        private int iItemId = 0;
        private INotificationService notificationService;
        private IInitialCapacityReviewService capacityReviewService;
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

        public SecondaryApprovalReviewForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            secondaryApprovalReviewService = DependencyResolution.DependencyMapper.Container.Resolve<ISecondaryApprovalReviewService>();
            costingReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IInitialCostingReviewService>();
            capacityReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IInitialCapacityReviewService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    this.divAccessDenied.Visible = false;
                    this.divAccessRequest.Visible = false;

                    Utilities.BindDropDownItems(drpCountinue, GlobalConstants.LIST_SecondaryInitialReviewDecisionsLookup, SPContext.Current.Web.Url);

                    // Check for a valid project number
                    if (!CheckProjectNumber())
                        return;

                    LoadFormData();
                    InitializeScreen();
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "SecondaryApprovalReviewForm " + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, "SecondaryApprovalReviewForm", "Page_Load");
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hiddenItemId.Value);
            }
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

            if (iItemId == 0)
            {
                // Invalid project Id supplied
                Page.Response.Redirect("/_layouts/Compass/AppPages/CompassErrorPage.aspx?ErrorId=1&ProjectNo=" + ProjectNumber, false);
                return false;
            }

            // Store Id in Hidden field
            this.hiddenItemId.Value = iItemId.ToString();
            return true;
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            //if (!userMgmtService.HasReadAccess(CompassForm.SrOBMApproval2))
            //{
            //    this.divAccessDenied.Visible = true;
            //}

            ////If user does not have rights to save/ submit the page, disable the Save and Submit buttons
            //if (!userMgmtService.HasWriteAccess(CompassForm.SrOBMApproval2))
            //{
            //    this.btnSave.Enabled = false;
            //    this.btnSubmit.Enabled = false;
            //}

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
            SecondaryApprovalReviewItem approvalItem = secondaryApprovalReviewService.GetSecondaryApprovalReviewItem(iItemId);
            InitialCapacityReviewItem initialCapacityItem = capacityReviewService.GetInitialCapacityReviewItem(iItemId);
            InitialCostingReviewItem initialCostingItem = costingReviewService.GetInitialCostingReviewItem(iItemId);
     
            // Set the Initial Approval Information
            try
            {
                //IPF Form
                lblAnnualDollar.Text = Utilities.FormatCurrency(approvalItem.AnnualProjectedDollars);
                lblAnnualUnits.Text = Utilities.FormatNumber(approvalItem.AnnualProjectedUnits);
                lblBrand.Text = approvalItem.MaterialGroup1Brand;
                lblExpectedGrossMargin.Text = Utilities.FormatPercentage(approvalItem.ExpectedGrossMarginPercent, 2);
                lblProjectNotes.Text = approvalItem.ItemConcept;
                lblProjectType.Text = approvalItem.ProjectType;
                lblRevisedFirstShipDate.Text = Utilities.GetDateForDisplay(approvalItem.RevisedFirstShipDate);
                lblWeeksToShip.Text = Utilities.DetermineWeeksToShip(approvalItem.RevisedFirstShipDate);
                lblProductHierarchy1.Text = approvalItem.ProductHierarchyLevel1;
                lblProductHierarchy2.Text = approvalItem.ProductHierarchyLevel2;
                lblCustomer.Text = approvalItem.Customer;
                txtChannel.Text = approvalItem.Channel;
                //Critical Data Points
                lblAnnualProjectedUnits.Text = Utilities.FormatNumber(initialCapacityItem.AnnualProjectedUnits);

                if (initialCapacityItem.SAPBaseUOM.ToLower() == "cs")
                {
                    if (initialCapacityItem.RetailSellingUnitsBaseUOM == 0 || initialCapacityItem.RetailSellingUnitsBaseUOM == -9999)
                        lblAnnualProjectedCases.Text = "NA";
                    else
                        lblAnnualProjectedCases.Text = Utilities.FormatNumber(initialCapacityItem.AnnualProjectedUnits / initialCapacityItem.RetailSellingUnitsBaseUOM);
                }
                else
                    lblAnnualProjectedCases.Text = "NA";

                lblAnnualProjectLbs.Text = Utilities.FormatNumber((initialCapacityItem.AnnualProjectedUnits * initialCapacityItem.RetailUnitWieghtOz) / 16);
                lblMakeLocation.Text = initialCapacityItem.ManufacturingLocation;
                lblPrimaryPackLocation.Text = initialCapacityItem.PackingLocation;
                lblCapacitySrOBMComments.Text = approvalItem.SrOBMApproval_CapacityReviewComments;

                /////////// Load Capacity Review
                lblMakeLocationIssues.Text = initialCapacityItem.InitialCapacity_MakeIssues;
                lblPackLocationIssues.Text = initialCapacityItem.InitialCapacity_PackIssues;
                lblCapacityComments.Text = initialCapacityItem.InitialCapacity_CapacityRiskComments;
                lblProjectDecisionCapacity.Text = initialCapacityItem.InitialCapacity_Decision;
                lblProjectAcceptance.Text = initialCapacityItem.InitialCapacity_AcceptanceComments;

                /////////// Load Costing Review
                lblExpectedGrossMargin.Text = Utilities.FormatPercentage(initialCostingItem.ExpectedGrossMarginPercent, 0);
                lblCostingSrOBMComments.Text = approvalItem.SrOBMApproval_CostingReviewComments;
                lblRevisedGrossMargin.Text = Utilities.FormatPercentage(initialCostingItem.RevisedGrossMarginPercent, 0);
                lblGrossMarginAccurate.Text = initialCostingItem.InitialCosting_GrossMarginAccurate;
                lblInitialCostingComments.Text = initialCostingItem.InitialCosting_Comments;
                lblProjectDecisionCosting.Text = initialCostingItem.InitialCosting_Decision;
                if (string.Equals(initialCostingItem.InitialCosting_GrossMarginAccurate.ToLower(), "yes"))
                    dvRevisedExpectedGrossMargin.Visible = false;

                /////////// Load Costing Review
                Utilities.SetDropDownValue(approvalItem.SrOBMApproval2_Decision, this.drpCountinue, this.Page);
            }
            catch (Exception ex)
            {
                //ErrorSummary.AddError(ex.Message, this.Page);
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SrOBMApproval2.ToString() + ": LoadFormData: " + ex.Message);
                //exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.SrOBMApproval2.ToString(), "LoadFormData");
            }
        }
        private SecondaryApprovalReviewItem ConstructFormData()
        {
            SecondaryApprovalReviewItem item = new SecondaryApprovalReviewItem();

            //item.CompassListItemId = iItemId;
            //item.SrOBMApproval2_Decision = drpCountinue.SelectedItem.Text;
            //item.LastUpdatedFormName = CompassForm.SrOBMApproval2.ToString();

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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Initial Review Approval");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!userMgmtService.HasWriteAccess(CompassForm.SrOBMApproval2))
                //{
                //    ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
                //    return;
                //}
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                // Retrieve the data from the form
                SecondaryApprovalReviewItem item = ConstructFormData();
                secondaryApprovalReviewService.UpdateSecondaryApprovalReviewItem(item);

                ApprovalItem appItem = ConstructApprovalData();
                secondaryApprovalReviewService.UpdateSecondaryApprovalReviewApprovalItem(appItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                //ErrorSummary.AddError(ex.Message, this.Page);
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.SrOBMApproval2.ToString() + ": btnSave_Click: " + ex.Message);
                //exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.SrOBMApproval2.ToString(), "btnSave_Click");
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!userMgmtService.HasWriteAccess(CompassForm.SrOBMApproval2))
                //{
                //    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                //    return;
                //}
                if (iItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                SecondaryApprovalReviewItem item = ConstructFormData();
                secondaryApprovalReviewService.UpdateSecondaryApprovalReviewItem(item);

                ApprovalItem appItem = ConstructApprovalData();
                secondaryApprovalReviewService.UpdateSecondaryApprovalReviewApprovalItem(appItem, true);

                // Complete the workflow task
                //workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.SrOBMApproval2);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea,   ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, "", "btnSubmit_Click");
            }
        }
        #endregion
    }
}
