using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.Unity;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Enum;
using System.Web.UI;
using System.Reflection;
using System.Linq;

namespace Ferrara.Compass.WebParts.RegulatoryComments
{
    [ToolboxItemAttribute(false)]
    public partial class RegulatoryComments : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IProjectNotesService notesService;
        private IExceptionService exceptionService;
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
        public RegulatoryComments()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            notesService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectNotesService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check for a valid project number
            if (!CheckProjectNumber())
                return;

            if (!Page.IsPostBack)
            {
                LoadFormData();
            }
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            try
            {
                //Check In Compass List
                iItemId = Utilities.GetItemIdFromProjectNumber(ProjectNumber);

                if (iItemId == 0)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                iItemId = 0;
                return false;
            }

            return true;
        }
        #endregion

        #region Data Transfer Methods
        private void LoadFormData()
        {
            if (iItemId != 0)
            {
                this.txtRegulatoryComments.Text = notesService.GetRegulatoryComments(iItemId);
            }
        }
        #endregion
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtRegulatoryComments.Text.Trim()) || iItemId == 0)
                return;

            notesService.UpdateRegulatoryComments(iItemId, this.txtRegulatoryComments.Text.Trim());
            lblSavedMessage.Text = "Changes Saved: " + DateTime.Now.ToString();
        }
        protected void lbHelpDeskEmail_Click(object sender, EventArgs e)
        {
            //notificationService.SendHelpDeskAccessEmail(SPContext.Current.Web.CurrentUser.LoginName, SPContext.Current.Web.CurrentUser.Email, "InTech Regulatory");
            //this.divAccessDenied.Visible = false;
            //this.divAccessRequest.Visible = true;
        }

    }
}
