using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.ComponentModel;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Threading.Tasks;

namespace Ferrara.Compass.WebParts.AllParentProjectDetailsDashboard
{
    [ToolboxItemAttribute(false)]
    public partial class AllParentProjectDetailsDashboard : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public AllParentProjectDetailsDashboard()
        {
        }
        #region Member Variables
        private IDashboardService dashboardService;
        private IExceptionService exceptionService;
        #endregion
        public string dtQuery { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (hasError == "true")
            {
                dashboardError.Controls.Add(new LiteralControl(errorText));
                dashboardError.Attributes.CssStyle.Add("display", "block");
            }
            this.hidTableDisplayType.Value = "false";

            AllOpenProjects();
        }
        private string hasError
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["error"] != null)
                    return HttpContext.Current.Request.QueryString["error"];
                return string.Empty;
            }
        }
        private string errorText
        {
            get
            {
                if (HttpContext.Current.Request.QueryString["errorText"] != null)
                    return HttpContext.Current.Request.QueryString["errorText"];
                return string.Empty;
            }
        }
        public void AllOpenProjects()
        {
            try
            {
                List<ProjectDetailsItem> lst = new List<ProjectDetailsItem>();
                #region Construct Filter
                if (ProjectStatusFilter.SelectedValue == "Completed")
                {
                    dtQuery = "<Where>" +
                                    "<Eq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">Complete</Value></Eq>" +
                              "</Where>";
                }
                else if (ProjectStatusFilter.SelectedValue == "Cancelled")
                {
                    dtQuery = "<Where><Eq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">Cancelled</Value></Eq></Where>";
                }
                else if (ProjectStatusFilter.SelectedValue == "All Projects")
                {
                    dtQuery = "<Where><Neq><FieldRef Name=\"Title\" /><Value Type=\"Text\"></Value></Neq></Where>";
                }
                else if (ProjectStatusFilter.SelectedValue == "OnHold")
                {
                    dtQuery = "<Where>" +
                                   "<Eq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">On Hold</Value></Eq>" +
                             "</Where>";
                }
                else
                {
                    dtQuery = "<Where>" +
                                    "<And>" +
                                        "<And>" +
                                            "<Neq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">Complete</Value></Neq>" +
                                            "<Neq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">Cancelled</Value></Neq>" +
                                        "</And>" +
                                        "<Neq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">On Hold</Value></Neq>" +
                                    "</And>" +
                              "</Where>";
                }
                #endregion
                lst = dashboardService.getStageGateRequestedProjectDetails(dtQuery);

                foreach (ProjectDetailsItem item in lst)
                {
                    item.ProjectNumber = "<a href='/Pages/StageGateProjectPanel.aspx?ProjectNo=" + item.ProjectNumber + "'>" + item.ProjectNumber + "</a>";
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                litScript.Text = "<script> var parentProjectDetails=" + js.Serialize(lst) + "</script>";
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "AllParentProjectDetailsDashboard: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "AllParentProjectDetailsDashboard", "AllOpenProjects");
            }
        }
    }
}
