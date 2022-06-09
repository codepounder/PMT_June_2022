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

namespace Ferrara.Compass.WebParts.WorldSyncRequestUpload
{
    [ToolboxItemAttribute(false)]
    public partial class WorldSyncRequestUpload : WebPart
    {
        #region Member Variables
        private IWorldSyncRequestService requestService;
        private IUtilityService utilityService;
        private IExceptionService exceptionService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WorldSyncRequestUpload()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            requestService = DependencyResolution.DependencyMapper.Container.Resolve<IWorldSyncRequestService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;
            }
            LoadFormData();
            InitializeScreen();
        }

        #region Data Transfer Methods
        private void LoadFormData()
        {
            int RequestId = 0;
            WorldSyncRequestItem request;
            if (!int.TryParse(Page.Request.QueryString[GlobalConstants.QUERYSTRING_RequestId], out RequestId))
            {
                ErrorSummary.AddError("Request Id " + RequestId + " is not valid!", Page);
                btnUpload.Visible = false;
                return;
            }
            request = requestService.GetRequestItemById(RequestId);
            if (request == null)
            {
                ErrorSummary.AddError("Request Id " + RequestId + " was not found!", Page);
                btnUpload.Visible = false;
                return;
            }
            lblRequestId.Text = request.RequestId.ToString();
            hddDocType.Value = request.RequestType;
            lblSAPnumber.Text = request.SAPnumber;
            lblSAPdescription.Text = request.SAPdescription;
            if (request.RequestType.Equals(GlobalConstants.DOCTYPE_RequestImage))
                lblRequestType.Text = "Image Request";
            if (request.RequestType.Equals(GlobalConstants.DOCTYPE_RequestNutritional))
                lblRequestType.Text = "Nutritionals Request";
            divRequestUploaded.Visible = request.RequestStatus.Equals(GlobalConstants.WORLDSYNCREQ_Completed);
            GetAttachments();
        }
        private void InitializeScreen()
        {
            bool rigthRequestTypeAndProfile = false;

            if (hddDocType.Value.Equals(GlobalConstants.DOCTYPE_RequestImage))
                rigthRequestTypeAndProfile = userMgmtService.IsCurrentUserInGroup(GlobalConstants.GROUP_Graphics) ||
                    userMgmtService.IsCurrentUserInGroup(GlobalConstants.GROUP_Marketing);
            else
                if (hddDocType.Value.Equals(GlobalConstants.DOCTYPE_RequestNutritional))
                    rigthRequestTypeAndProfile = userMgmtService.IsCurrentUserInGroup(GlobalConstants.GROUP_QualityAssurance);
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            this.divAccessDenied.Visible = !userMgmtService.HasWriteAccess(CompassForm.WorldSyncRequestUpload) || !rigthRequestTypeAndProfile;
            //If user does not have rights to save/ submit the page, disable the Save and Submit buttons
            this.btnSubmit.Enabled = btnSubmit.Enabled && !divAccessDenied.Visible;
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Operation Form");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }
        #endregion

        #region Button Methods
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool completed;
            try
            {
                var files = requestService.GetUploadedFilesByRequestId(lblSAPnumber.Text, Convert.ToInt32(lblRequestId.Text));
                if (files.Count <= 0)
                {
                    ErrorSummary.AddError("An uploaded file is required!", this.Page);
                    return;
                }

                // Complete the workflow task
                completed = workflowService.CompleteWorldSyncReqWorkflowTask(Convert.ToInt32(lblRequestId.Text), GlobalConstants.WORLDSYNCREQ_STEP_RequestFile);
                // Redirect to Home page after successfull Submit
                if (completed)
                    Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_TaskDashboard), false);
                else
                    ErrorSummary.AddError("An error occurred trying to register your upload", Page);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.FGPackSpec.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.FGPackSpec.ToString(), "btnSubmit_Click");
            }
        }
        #endregion

        #region Attachment Methods
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            GetAttachments();
        }
        protected void lnkDeleteAttachment_Click(object sender, EventArgs e)
        {
            var lnkButton = (LinkButton)sender;
            if (!string.IsNullOrEmpty(lnkButton.CommandName))
            {
                utilityService.DeleteAttachment(lnkButton.CommandName);
                GetAttachments();
            }
        }
        private void GetAttachments()
        {
            var files = requestService.GetUploadedFilesByRequestId(lblSAPnumber.Text, Convert.ToInt32(lblRequestId.Text));
            btnSubmit.Enabled = !divRequestUploaded.Visible && files.Count > 0;
            if (files.Count > 0)
            {
                rptAttachmentSpec.Visible = true;
                rptAttachmentSpec.DataSource = files;
                rptAttachmentSpec.DataBind();
            }
            else
                rptAttachmentSpec.Visible = false;
        }
        #endregion
    }
}
