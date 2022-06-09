using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace Ferrara.Compass.WebParts.AllProjectDetailsDashboard2
{
    [ToolboxItemAttribute(false)]
    public partial class AllProjectDetailsDashboard2 : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public AllProjectDetailsDashboard2()
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
                    dtQuery = "<Where><And><And><Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Neq><Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">On Hold</Value></Neq></And><Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Neq></And></Where>";
                }
                #endregion
                lst = dashboardService.getRequestedProjectDetails2(dtQuery);

                //List<int> projectNos = new List<int>();
                //foreach (ProjectDetailsItem item in lst)
                //{
                //    projectNos.Add(item.CompassItemId);
                //}
                //List<List<int>> projectNoChunks = SplitList<int>(projectNos, 50);

                //var packagingInfo = new List<KeyValuePair<int, Dictionary<string, string>>>();

                //packagingInfo.AddRange(GetAllPackagingNumbersForProjectAsync(projectNoChunks));

                //foreach (ProjectDetailsItem item in lst)
                //{
                //    List<KeyValuePair<string, string>> packagingNumbers = packagingInfo.Where(r => r.Key == item.CompassItemId).SelectMany(r => r.Value).ToList();
                //    string condensed = "";
                //    foreach (KeyValuePair<string, string> dets in packagingNumbers)
                //    {
                //        condensed += dets.Key + ": " + dets.Value + "; ";
                //    }
                //    item.PackagingNumbers = condensed;
                //    item.ProjectWorkflowLink = "<a href='/Pages/ProjectStatus.aspx?ProjectNo=" + item.ProjectNumber + "'>" + item.ProjectNumber + "</a>";
                //    item.CommercializationLink = "<a href='/Pages/CommercializationItem.aspx?ProjectNo=" + item.ProjectNumber + "'>" + item.ProjectNumber + "</a>";
                //    if (!string.IsNullOrEmpty(item.ParentProjectNumber))
                //    {
                //        item.ParentProjectNumberLink = "<a href='/Pages/StageGateProjectPanel.aspx?ProjectNo=" + item.ParentProjectNumber + "'>" + item.ParentProjectNumber + "</a>";
                //    }
                //}

                hidProjectGroup.Value = "procurement";

                JavaScriptSerializer js = new JavaScriptSerializer();
                js.MaxJsonLength = int.MaxValue;
                litScript.Text = "<script> var projectDetails=" + js.Serialize(lst) + "</script>";
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "AllProjectDetailsDashboard: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "AllProjectDetailsDashboard", "AllOpenProjects");
            }
        }

        //private List<KeyValuePair<int, Dictionary<string, string>>> GetAllPackagingNumbersForProjectAsync(List<List<int>> projectNoChunks)
        //{
        //    try
        //    {
        //        List<KeyValuePair<int, Dictionary<string, string>>> packagingInfo = new List<KeyValuePair<int, Dictionary<string, string>>>();

        //        Task.WhenAll(projectNoChunks.Select(async item => packagingInfo.AddRange(await dashboardService.GetAllPackagingNumbersForProjectAsync(item))));
        //        return packagingInfo;
        //    }
        //    catch (Exception exception)
        //    {
        //        LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "AllProjectDetailsDashboard: " + exception.Message);
        //        exceptionService.Handle(LogCategory.CriticalError, exception, "AllProjectDetailsDashboard", "AllOpenProjects");
        //        throw exception;
        //    }
        //}

        //public List<List<T>> SplitList<T>(List<T> items, int sliceSize = 30)
        //{
        //    List<List<T>> list = new List<List<T>>();

        //    for (int i = 0; i < items.Count; i += sliceSize)
        //    {
        //        list.Add(items.GetRange(i, Math.Min(sliceSize, items.Count - i)));
        //    }

        //    return list;
        //}
    }
}
