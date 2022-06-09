using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Linq;
using System.ComponentModel;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Classes;

namespace Ferrara.Compass.WebParts.WorldSyncRequestDashboard
{
    [ToolboxItemAttribute(false)]
    public partial class WorldSyncRequestDashboard : WebPart
    {
        private IDashboardService dashboardService;
        private IWorkflowService worflowService;
        private INotificationService notificationService;
        private IUserManagementService userMgmtService;
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public WorldSyncRequestDashboard()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            worflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
            userMgmtService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "Operation Form");
            this.divAccessDenied.Visible = false;
            this.divAccessRequest.Visible = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;
                hlkNewRequest.NavigateUrl = Utilities.RedirectPageValue(GlobalConstants.PAGE_WorldSyncRequestFile);
                hlkWorldSyncFuseFile.NavigateUrl = Utilities.RedirectPageValue(GlobalConstants.PAGE_WorldSyncFuseFile);
            }
            InitializeScreen();
            LoadMyRequestTasks();
        }

        private void InitializeScreen()
        {
        }

        private void LoadMyRequestTasks()
        {
            StringBuilder query;
            List<WorldSyncReqTask> myRequestList, openRequestList;
            HashSet<int> requestIdList;
            JavaScriptSerializer jss = new JavaScriptSerializer();
            requestIdList = worflowService.GetRequestIdAssignedToCurrentUser();
            if (requestIdList.Count == 0)
                myRequestList = new List<WorldSyncReqTask>(0);
            else
            {
                query = new StringBuilder("<Where><In><FieldRef Name = \"ID\"/><Values>");
                foreach (int requestId in requestIdList)
                    query.Append("<Value Type=\"Int\">" + requestId + "</Value>");
                query.Append("</Values></In></Where>");
                myRequestList = dashboardService.getWorldSyncRequestItems(query.ToString());
                foreach (WorldSyncReqTask task in myRequestList)
                {
                    if (task.WorkflowStep.Equals(GlobalConstants.WORLDSYNCREQ_STEP_ReceiveFile))
                        task.Task = "<a href='/Pages/" + GlobalConstants.PAGE_WorldSyncRequestReceipt + "?RequestId=" + task.RequestId + "'>Confirm Receipt</a>";
                    else
                        if (task.WorkflowStep.Equals(GlobalConstants.WORLDSYNCREQ_STEP_RequestFile))
                        switch (task.RequestType)
                        {
                            case GlobalConstants.DOCTYPE_RequestImage: task.Task = "<a href='/Pages/" + GlobalConstants.PAGE_WorldSyncRequestUpload + "?RequestId=" + task.RequestId + "'>Upload Image</a>"; break;
                            case GlobalConstants.DOCTYPE_RequestNutritional: task.Task = "<a href='/Pages/" + GlobalConstants.PAGE_WorldSyncRequestUpload + "?RequestId=" + task.RequestId + "'>Upload Nutritional</a>"; break;
                            default: task.Task = ""; break;
                        }
                    else
                        task.Task = "";
                }
            }
            requestIdList = worflowService.GetRequestIdAssigned();
            if (requestIdList.Count == 0)
                openRequestList = new List<WorldSyncReqTask>(0);
            else
            {
                query = new StringBuilder("<Where><In><FieldRef Name = \"ID\"/><Values>");
                foreach (int requestId in requestIdList)
                    query.Append("<Value Type=\"Int\">" + requestId + "</Value>");
                query.Append("</Values></In></Where>");
                openRequestList = dashboardService.getWorldSyncRequestItems(query.ToString());

                foreach (WorldSyncReqTask task in openRequestList)
                {
                    switch (task.RequestType)
                    {
                        case GlobalConstants.DOCTYPE_RequestImage: task.RequestType = "Image"; break;
                        case GlobalConstants.DOCTYPE_RequestNutritional: task.RequestType = "Nutritional"; break;
                        default: task.RequestType = ""; break;
                    }
                    switch (task.WorkflowStep)
                    {
                        case GlobalConstants.WORLDSYNCREQ_STEP_RequestFile: task.WorkflowStep = "Upload"; break;
                        case GlobalConstants.WORLDSYNCREQ_STEP_ReceiveFile: task.WorkflowStep = "Receive"; break;
                        default: task.RequestType = ""; break;
                    }
                }
            }
            litScript.Text = "<script>MyWorldSyncReqTasks = " + jss.Serialize(myRequestList) + ";" + '\n' +
                "OpenWorldSyncReqTasks = " + jss.Serialize(openRequestList) + ";</script>";
        }
    }
}
