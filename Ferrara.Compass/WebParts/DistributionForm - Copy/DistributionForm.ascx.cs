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

namespace Ferrara.Compass.WebParts.DistributionForm
{
    [ToolboxItemAttribute(false)]
    public partial class DistributionForm : WebPart
    {
        #region Member Variables
        private IDistributionService distributionService;
        private IExceptionService exceptionService;
        private IUtilityService utilityService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private int iItemId = 0;
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
        public DistributionForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            distributionService = DependencyResolution.DependencyMapper.Container.Resolve<IDistributionService>();
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

                    // Check for a valid project number
                    if (!CheckProjectNumber())
                        return;

                    Utilities.BindDropDownItems(ddlDistributionCenter, GlobalConstants.LIST_DistributionCentersLookup, SPContext.Current.Web.Url);

                    LoadFormData();
                    InitializeScreen();
                    if (hdnProjectType.Value.Contains("Renovations"))
                    {
                        dvMain.Visible = false;
                        dvMsg.Visible = true;
                    }
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError(exception.Message, this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.Distribution.ToString(), "Page_Load");
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
            if (!userMgmtService.HasReadAccess(CompassForm.Distribution))
            {
                this.divAccessDenied.Visible = true;
            }

            // If user does not have rights to save/submit the page, disable the Save and Submit buttons
            if (!userMgmtService.HasWriteAccess(CompassForm.Distribution))
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
            DistributionItem item = distributionService.GetDistributionItem(iItemId);

            this.lblProductHierarchyLevel1.Text = item.ProductHierarchyLevel1;
            this.lblProductHierarchyLevel2.Text = item.ProductHierarchyLevel2;
            this.lblMaterialGroup1.Text = item.MaterialGroup1Brand;
            this.lblMaterialGroup4.Text = item.MaterialGroup4ProductForm;
            this.lblMaterialGroup5.Text = item.MaterialGroup5PackType;
            hdnProjectType.Value = item.ProjectType;
            Utilities.SetDropDownValue(item.DistributionCenter, this.ddlDistributionCenter, this.Page);
        }
        private DistributionItem ConstructFormData()
        {
            var item = new DistributionItem();

            item.CompassListItemId = iItemId;
            item.DistributionCenter = this.ddlDistributionCenter.SelectedItem.Text;
            item.LastUpdatedFormName = CompassForm.Distribution.ToString();

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
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Distribution");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.Distribution))
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
                DistributionItem item = ConstructFormData();
                distributionService.UpdateDistributionItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                distributionService.UpdateDistributionApprovalItem(approvalItem, false);

                lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.Distribution.ToString(), "btnSave_Click");
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (!userMgmtService.HasWriteAccess(CompassForm.Distribution))
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
                DistributionItem item = ConstructFormData();
                distributionService.UpdateDistributionItem(item);

                ApprovalItem approvalItem = ConstructApprovalData();
                distributionService.UpdateDistributionApprovalItem(approvalItem, true);

                // Complete the workflow task
                workflowService.CompleteWorkflowTask(iItemId, WorkflowStep.Distribution);

                // Redirect to Home page after successfull Submit                    
                Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.Distribution.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.Distribution.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

        protected void lnlIPF_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_ItemProposal, ProjectNumber), false);
        }
    }
}
