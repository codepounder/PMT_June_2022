using System;
using System.ComponentModel;
using Microsoft.SharePoint;
using Microsoft.Practices.Unity;
using System.Web.UI.WebControls.WebParts;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.WebParts.WorldSyncRequestReceipt
{
    [ToolboxItemAttribute(false)]
    public partial class WorldSyncRequestReceipt : WebPart
    {
        private IUserManagementService userMgmtService;
        private IWorldSyncRequestService requestService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUtilityService utilityService;
        private IExceptionService exceptionService;
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WorldSyncRequestReceipt()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            requestService = DependencyResolution.DependencyMapper.Container.Resolve<IWorldSyncRequestService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
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
                btnSubmit.Visible = false;
                return;
            }
            request = requestService.GetRequestItemById(RequestId);
            if (request == null)
            {
                ErrorSummary.AddError("Request Id " + RequestId + " was not found!", Page);
                btnSubmit.Visible = false;
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
            btnSubmit.Enabled = request.RequestStatus.Equals(GlobalConstants.WORLDSYNCREQ_Completed);
            divRequestUploaded.Visible = !btnSubmit.Enabled;
            GetAttachments();
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            divAccessDenied.Visible = !userMgmtService.HasReadAccess(CompassForm.WorldSyncRequestReceipt);
            btnSubmit.Enabled = !divAccessDenied.Visible && userMgmtService.HasWriteAccess(CompassForm.WorldSyncRequestReceipt);
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
                // Complete the workflow task
                completed = workflowService.CompleteWorldSyncReqWorkflowTask(Convert.ToInt32(lblRequestId.Text), GlobalConstants.WORLDSYNCREQ_STEP_ReceiveFile);
                // Redirect to Home page after successfull Submit
                if (completed)
                    Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_TaskDashboard), false);
                else
                    ErrorSummary.AddError("An error occurred trying to register your receipt", Page);
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

        private void GetAttachments()
        {
            var files = utilityService.GetUploadedWorldSyncReqFilesByDocType(lblSAPnumber.Text, hddDocType.Value);
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
