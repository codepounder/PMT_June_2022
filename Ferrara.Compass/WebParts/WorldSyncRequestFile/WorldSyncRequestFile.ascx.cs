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

namespace Ferrara.Compass.WebParts.WorldSyncRequestFile
{
    [ToolboxItemAttribute(false)]
    public partial class WorldSyncRequestFile : WebPart
    {
        #region Member Variables
        private IWorldSyncRequestService requestService;
        private IExceptionService exceptionService;
        private IWorkflowService workflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        private IUtilityService utilityService;
        #endregion

        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WorldSyncRequestFile()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            requestService = DependencyResolution.DependencyMapper.Container.Resolve<IWorldSyncRequestService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;
            }
            InitializeScreen();
        }
        private void InitializeScreen()
        {
            // If user does not belong to a valid group for the page, inform them that they do not hvae access rights
            divAccessDenied.Visible = !userMgmtService.HasReadAccess(CompassForm.WorldSyncRequestFile);
            btnRequestImage.Enabled = userMgmtService.HasWriteAccess(CompassForm.WorldSyncRequestFile);
            btnRequestNutritionals.Enabled = btnRequestImage.Enabled;
            divAccessRequest.Visible = false;
            btnSearch.Enabled = userMgmtService.HasReadAccess(CompassForm.WorldSyncRequestFile);
            dvSearchResult.Visible = false;
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Operation Form");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }

        protected void btnRequestImage_Click(object sender, EventArgs e)
        {
            int requestId;
            List<WorldSyncRequestItem> requests;
            requests = requestService.GetRequestItems(txtSAPnumber.Text.ToUpper(), GlobalConstants.DOCTYPE_RequestImage, GlobalConstants.WORLDSYNCREQ_InProcess);
            if (requests.Count > 0)
                ErrorSummary.AddError("There is previous request for SAP number " + txtSAPnumber.Text + " and image request type " +
                    " currently in progress", Page);
            else
            {
                requestId = insertWorldSyncRequest(GlobalConstants.DOCTYPE_RequestImage);
                if (requestId > 0)
                {
                    workflowService.StartWorkflowWorldSyncRequest(requestId);
                    Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_WorldSyncRequestDashboard));
                }
            }
        }

        protected void btnRequestNutritionals_Click(object sender, EventArgs e)
        {
            int requestId;
            List<WorldSyncRequestItem> requests;
            requests = requestService.GetRequestItems(txtSAPnumber.Text.ToUpper(), GlobalConstants.DOCTYPE_RequestNutritional, GlobalConstants.WORLDSYNCREQ_InProcess);
            if (requests.Count > 0)
                ErrorSummary.AddError("There is previous request for SAP number " + txtSAPnumber.Text + " and nutritional request type " +
                    " currently in progress", Page);
            else
            {
                requestId = insertWorldSyncRequest(GlobalConstants.DOCTYPE_RequestNutritional);
                if (requestId > 0)
                {
                    workflowService.StartWorkflowWorldSyncRequest(requestId);
                    Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_WorldSyncRequestDashboard));
                }
            }
        }

        private int insertWorldSyncRequest(string requestType)
        {
            int requestId = 0;
            WorldSyncRequestItem request;
            try
            {
                request = new WorldSyncRequestItem();
                request.SAPnumber = txtSAPnumber.Text.ToUpper();
                request.SAPdescription = txtSAPdescription.Text;
                request.RequestType = requestType;
                request.RequestStatus = GlobalConstants.WORLDSYNCREQ_InProcess;
                request.WorkflowStep = GlobalConstants.WORLDSYNCREQ_STEP_RequestFile;
                requestId = requestService.InsertRequest(request);
            }
            catch (Exception ex)
            {
                ErrorSummary.AddError(ex.Message, this.Page);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.WorldSyncRequestFile.ToString() + ": insertWorldSyncRequest: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.WorldSyncRequestFile.ToString(), "insertWorldSyncRequest");
            }
            return requestId;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            List<CompassListItem> CompassListItems = utilityService.GetCompassListFromSAPNumber(txtSAPnumber.Text);
            lblmsg.Text = "";
            bool fileNotFound = true;
            bool projectNotExists = true;
            if (CompassListItems.Count > 0)
            {
                projectNotExists = false;
                foreach (CompassListItem item in CompassListItems)
                {
                    var files = utilityService.GetUploadedFiles(item.ProjectNumber).ToList();
                    var ImageFiles = files.Where(x => x.DocType == "Graphics").ToList();
                    var NLEAfiles = files.Where(x => x.DocType == "NLEA").ToList();
                    if (ImageFiles.Count > 0 || NLEAfiles.Count > 0)
                    {
                        dvSearchResult.Visible = true;
                        fileNotFound = false;
                        lblSAPNumber.Text = item.SAPItemNumber;
                        lblSAPDescription.Text = item.SAPDescription;
                        lblProjectNumber.Text = item.ProjectNumber;

                        rptAttachmentSpecImage.DataSource = ImageFiles;
                        rptAttachmentSpecImage.DataBind();

                        rptAttachmentSpecNLEA.DataSource = NLEAfiles;
                        rptAttachmentSpecNLEA.DataBind();
                        break;
                    }
                }
            }            

            if (projectNotExists)
            {
                lblmsg.Text = "No Projects found for the SAP No: " + txtSAPnumber.Text;
            }
            else if (fileNotFound)
            {
                lblmsg.Text = "No Images and Nutritionals attached for the SAP No: " + txtSAPnumber.Text;
            }
        }
    }

}

