using System;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.ComponentModel;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;

namespace Ferrara.Compass.WebParts.AllOpenProjectsForm
{
    [ToolboxItemAttribute(false)]
    public partial class AllOpenProjectsForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public AllOpenProjectsForm()
        {
        }
        #region Member Variables
        private IDashboardService dashboardService;

        #endregion
        public string dtQuery { get; set; }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
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
            List<ProjectDetailsItem> lst = new List<ProjectDetailsItem>();
            if (ProjectStatusFilter.SelectedValue == "Cancelled")
            {
                dtQuery = "<Where><Eq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Eq></Where>";
            }
            else if (ProjectStatusFilter.SelectedValue == "Completed")
            {
                dtQuery = "<Where><Eq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Eq></Where>";
            }
            else if (ProjectStatusFilter.SelectedValue == "On Hold")
            {
                dtQuery = "<Where><Eq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">On Hold</Value></Eq></Where>";
            }
            else if (ProjectStatusFilter.SelectedValue == "All Projects")
            {
                dtQuery = "<Where><IsNotNull><FieldRef Name=\"WorkflowPhase\" /></IsNotNull></Where>";
            }
            else
            {
                dtQuery = "<Where>" +
                                "<And>" +
                                    "<And>" +
                                        "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Neq>" +
                                        "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">On Hold</Value></Neq>" +
                                    "</And>" +
                                    "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Neq>" +
                                "</And>" +
                         "</Where>";
            }
            lst = dashboardService.getRequestedProjectDetails(dtQuery);
            foreach (ProjectDetailsItem item in lst)
            {
                item.ProjectWorkflowLink = "<a href='/Pages/ProjectStatus.aspx?ProjectNo=" + item.ProjectNumber + "'>" + item.ProjectNumber + "</a>";
                item.CommercializationLink = "<a href='/Pages/CommercializationItem.aspx?ProjectNo=" + item.ProjectNumber + "'>" + item.ProjectNumber + "</a>";
                if (!string.IsNullOrEmpty(item.ParentProjectNumber))
                {
                    item.ParentProjectNumberLink = "<a href='/Pages/StageGateProjectPanel.aspx?ProjectNo=" + item.ParentProjectNumber + "'>" + item.ParentProjectNumber + "</a>";
                }
            }
            List<ProjectDetailsItem> lstParents = new List<ProjectDetailsItem>();
            lstParents = (from item in lst where item.StageGateProjectListItemId > 0 select item).ToList<ProjectDetailsItem>();
            foreach (ProjectDetailsItem item in lstParents)
            {
                item.Parent = "<a href='/Pages/StageGateProjectPanel.aspx?ProjectNo=" + item.ProjectNumber + "'>" + item.ProjectNumber + "</a>";
            }
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            litScript.Text = "<script> var projectDetails=" + js.Serialize(lst) + "</script>";
        }

    }
}
