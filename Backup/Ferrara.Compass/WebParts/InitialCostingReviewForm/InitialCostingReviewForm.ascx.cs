using System;
using System.ComponentModel;
using System.Web.UI;
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

namespace Ferrara.Compass.WebParts.InitialCostingReviewForm
{
    [ToolboxItemAttribute(false)]
    public partial class InitialCostingReviewForm : WebPart
    {

        #region Member Variables
        private IInitialCostingReviewService costingReviewService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private IUserManagementService userMgmtService;
        public int iItemId = 0;
        private INotificationService notificationService;
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
        public InitialCostingReviewForm()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            costingReviewService = DependencyResolution.DependencyMapper.Container.Resolve<IInitialCostingReviewService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();

            ddlGrossMarginAccurate.Attributes.Add("onchange", "conditionalChecks();");
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
                    //ErrorSummary.AddError(exception.Message, this.Page);
                    //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCosting.ToString() + ": " + exception.Message);
                    //exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.InitialCosting.ToString(), "Page_Load");
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
            //if (!userMgmtService.HasReadAccess(CompassForm.InitialCosting))
            //{
            //    this.divAccessDenied.Visible = true;
            //}

            //If user does not have rights to save/ submit the page, disable the Save and Submit buttons
            //if (!userMgmtService.HasWriteAccess(CompassForm.InitialCosting))
            //{
            //    this.btnSave.Enabled = false;
            //    this.btnSubmit.Enabled = false;
            //}

            //if (Utilities.LockScreen(wfStep.ToString()))
            //{
            //    if (!Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_OBMAdmins))
            //    {
            //        this.btnSave.Enabled = false;
            //        this.btnSubmit.Enabled = false;
            //    }
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
            InitialCostingReviewItem costingItem = costingReviewService.GetInitialCostingReviewItem(iItemId);

            // Set the Initial Approval Information
            try
            {
                lblExpectedGrossMargin.Text = costingItem.ExpectedGrossMarginPercent.ToString();
                if (!string.IsNullOrEmpty(lblExpectedGrossMargin.Text))
                    lblExpectedGrossMargin.Text = lblExpectedGrossMargin.Text + "%";

                lblComments.Text = costingItem.SrOBMApproval_CostingReviewComments;

                // Load Costing Review
                Utilities.SetDropDownValue(costingItem.InitialCosting_Decision, this.ddlProjectDecision, this.Page);
                Utilities.SetDropDownValue(costingItem.InitialCosting_GrossMarginAccurate, this.ddlGrossMarginAccurate, this.Page);
                txtCostingComments.Text = costingItem.InitialCosting_Comments;
                
                txtRevisedGrossMargin.Text = costingItem.RevisedGrossMarginPercent.ToString();

                // Load Attachments
                GetAttachments();
            }
            catch (Exception exception)
            {
                //ErrorSummary.AddError(exception.Message, this.Page);
                //LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCosting.ToString() + "LoadFormData: " + exception.Message);
                //exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.InitialCosting.ToString(), "LoadFormData");
            }
        }
        private InitialCostingReviewItem ConstructFormData()
        {
            InitialCostingReviewItem item = new InitialCostingReviewItem();
            item.CompassListItemId = iItemId;

            item.RevisedGrossMarginPercent = string.IsNullOrEmpty(txtRevisedGrossMargin.Text) ? 0 : Convert.ToDouble(Utilities.RemoveFormatting(txtRevisedGrossMargin.Text));
            item.InitialCosting_Decision = ddlProjectDecision.SelectedItem.Text;
            item.InitialCosting_Comments = txtCostingComments.Text;
            item.InitialCosting_GrossMarginAccurate = ddlGrossMarginAccurate.SelectedItem.Text;

            return item;
        }
        private ApprovalItem ConstructApprovalData()
        {
            ApprovalItem item = new ApprovalItem();
            item.CompassListItemId = iItemId;

            item.ModifiedBy = SPContext.Current.Web.CurrentUser.Name;
            item.ModifiedDate = DateTime.Now.ToString();

            return item;
        }
        #endregion

        #region Attachment Methods
        private void GetAttachments()
        {
            var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_COSTING);
            if (files.Count > 0)
            {
                rpAttachments.Visible = true;
                rpAttachments.DataSource = files;
                rpAttachments.DataBind();
            }
            else {
                rpAttachments.Visible = false;
            }
        }
        protected void lnkFileDelete_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
        }
        #endregion

        #region Button Methods 
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Initial Approver Review");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!userMgmtService.HasWriteAccess(CompassForm.InitialCosting))
            //    {
            //        ErrorSummary.AddError("You do not have proper access rights to save this page!", this.Page);
            //        return;
            //    }
            //    if (iItemId <= 0)
            //    {
            //        ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
            //        return;
            //    }
            //    InitialCostingReviewItem item = ConstructFormData();
            //    costingReviewService.UpdateInitialCostingReviewItem(item);

            //    ApprovalItem appItem = ConstructApprovalData();
            //    costingReviewService.UpdateInitialCostingReviewApprovalItem(appItem, false);

            //    ////////// Load Attachments
            //    GetAttachments();

            //    lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            //}
            //catch (Exception ex)
            //{
            //    ErrorSummary.AddError(ex.Message, this.Page);
            //    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCosting.ToString() + ": btnSave_Click: " + ex.Message);
            //    exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.InitialCosting.ToString(), "btnSave_Click");
            //}
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //try
            //{
                //if (!userMgmtService.HasWriteAccess(CompassForm.InitialCosting))
                //{
                //    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                //    return;
                //}
                //if (iItemId <= 0)
                //{
                //    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                //    return;
                //}

                //InitialCostingReviewItem item = ConstructFormData();
                //costingReviewService.UpdateInitialCostingReviewItem(item);

                //ApprovalItem appItem = ConstructApprovalData();
                //costingReviewService.UpdateInitialCostingReviewApprovalItem(appItem, true);

                //// Complete the workflow task
                ////workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.InitialCosting);

                //// Redirect to Home page after successfull Submit                    
                //Page.Response.Redirect(Utilities.RedirecttoHomePage(), false); ////TODO:: fix the redirect method
            //}
            //catch (Exception ex)
            //{
            //    ErrorSummary.AddError(ex.Message, this.Page);
            //    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.InitialCosting.ToString() + ": btnSubmit_Click: " + ex.Message);
            //    exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.InitialCosting.ToString(), "btnSubmit_Click");
            //}
        }

        #endregion
    }
}
