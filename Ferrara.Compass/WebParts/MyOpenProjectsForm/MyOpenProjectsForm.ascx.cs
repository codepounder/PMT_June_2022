using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Microsoft.Practices.Unity;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace Ferrara.Compass.WebParts.MyOpenProjectsForm
{
    [ToolboxItemAttribute(false)]
    public partial class MyOpenProjectsForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public MyOpenProjectsForm()
        {
        }
        #region Member Variables
        private IDashboardService dashboardService;
        private IExceptionService exceptionService;
        private string ImpersonationId
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ImpersonationId] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ImpersonationId];
                return string.Empty;
            }
        }
        public string dtQuery { get; set; }
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
        #endregion
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            dashboardService = DependencyResolution.DependencyMapper.Container.Resolve<IDashboardService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
        }
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (hasError == "true")
            {
                dashboardError.Controls.Add(new LiteralControl(errorText));
                dashboardError.Attributes.CssStyle.Add("display", "block");
            }
            this.hidTableDisplayType.Value = "false";

            if (string.IsNullOrEmpty(ImpersonationId))
            {
                if (Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_ProcurementPackaging))
                {
                    this.hidProcurement.Value = "true";
                }
                
            }
            else
            {
                string userId = SPContext.Current.Web.CurrentUser.ID.ToString();

                string userName = "";
                using (SPSite spSite = new SPSite(SPContext.Current.Web.Url))
                {
                    using (SPWeb spWeb = spSite.OpenWeb())
                    {
                        SPUser spUser = new SPFieldUserValue(spWeb, Convert.ToInt32(ImpersonationId), null).User;
                        if (spUser != null)
                        {
                            userName = spUser.LoginName;
                            if (Utilities.CheckIfUserInGroup(GlobalConstants.GROUP_ProcurementPackaging, userName))
                            {
                                this.hidProcurement.Value = "true";
                            }
                        }
                    }
                }
            }
            BuildTable();
            AllOpenProjects();
        }
        #endregion
        #region Build Table
        private void BuildTable()
        {
            #region Parent Project
            StringBuilder str = new System.Text.StringBuilder();
            #region MyOpenParentProjects
            str.Append("<table id='MyOpenParentProjects' class='display'>");
            str.Append("<thead><tr>");
            str.Append(GetMyOpenParentProjectsColumns());
            str.Append("</tr></thead>");

            str.Append("<tfoot><tr>");
            str.Append(GetMyOpenParentProjectsColumns());
            str.Append("</tr></tfoot>");
            str.Append("</table>");
            #endregion

            this.litTableParentProjects.Text = str.ToString();
            #endregion
            #region Child Project
            str.Clear();
            #region MyOpenChildProjects
            str.Append("<table id='MyOpenChildProjects' class='display'>");
            str.Append("<thead><tr>");
            str.Append(GetMyOpenProjectsColumns());
            str.Append("</tr></thead>");

            str.Append("<tfoot><tr>");
            str.Append(GetMyOpenProjectsColumns());
            str.Append("</tr></tfoot>");
            str.Append("</table>");
            #endregion
            #region MyOpenChildProjectsProcurement
            str.Append("<table id='MyOpenChildProjectsProcurement' style='display:none' class='display'>");
            str.Append("<thead><tr>");
            str.Append(GetMyOpenProjectsProcurement());
            str.Append("</tr></thead>");

            str.Append("<tfoot><tr>");
            str.Append(GetMyOpenProjectsProcurement());
            str.Append("</tr></tfoot>");
            str.Append("</table>");
            #endregion
            this.litTableChildProjects.Text = str.ToString();
            #endregion
        }
        private static StringBuilder GetMyOpenProjectsProcurement()
        {
            StringBuilder str = new StringBuilder();
            str.Append("<th>Project Status</th>");              //0
            str.Append("<th>Project Name</th>");                //1
            str.Append("<th>Revised First Ship Date</th>");     //2
            str.Append("<th>Project Type</th>");                //3
            str.Append("<th>Project Type Subcategory</th>");    //4
            str.Append("<th>Material Group 1 (Brand)</th>");    //5
            str.Append("<th>Customer</th>");                    //6
            str.Append("<th>Initiator</th>");                   //7
            str.Append("<th>Packaging Numbers</th>");           //8
            str.Append("<th>Commercialization Link</th>");      //9
            str.Append("<th>Product Hierarchy Level 1</th>");   //10
            str.Append("<th>ID</th>");                          //11
            str.Append("<th>Parent</th>");                      //12
            str.Append("<th>StageGateProjectListItemId</th>");        //13

            return str;
        }
        private static StringBuilder GetMyOpenProjectsColumns()
        {
            StringBuilder str = new StringBuilder();
            str.Append("<th>Project Status</th>");              //0
            str.Append("<th>Project Name</th>");                //1
            str.Append("<th>1st Production Date</th>");         //2
            str.Append("<th>1st Ship Date</th>");               //3
            str.Append("<th>Project Type</th>");                //4
            str.Append("<th>Project Type Subcategory</th>");    //5
            str.Append("<th>Material Group 1 (Brand)</th>");    //6
            str.Append("<th>Customer</th>");                    //7
            str.Append("<th>Initiator</th>");                   //8
            str.Append("<th>Timeline Type</th>");               //9
            str.Append("<th>Progress</th>");                    //10
            str.Append("<th>Product Hierarchy Level 1</th>");   //11
            str.Append("<th>ID</th>");                          //12
            str.Append("<th>Parent</th>");                      //13
            str.Append("<th>StageGateProjectListItemId</th>");        //14

            return str;
        }
        private static StringBuilder GetMyOpenParentProjectsColumns()
        {
            StringBuilder str = new StringBuilder();
            str.Append("<th>Project Number</th>");
            str.Append("<th>Project Name</th>");
            str.Append("<th>Gate 0 Approved Date</th>");
            str.Append("<th>Desired 1st Ship Date</th>");
            str.Append("<th>Revised 1st Ship Date</th>");
            str.Append("<th>Current Project Stage</th>");
            str.Append("<th>Brand</th>");
            str.Append("<th>#SKUs</th>");
            str.Append("<th>Project Type</th>");
            str.Append("<th>Project Type SubCategory</th>");
            str.Append("<th>Project Manager</th>");
            str.Append("<th>Project Leader</th>");

            return str;
        }
        #endregion
        #region Get Projects
        public void AllOpenProjects()
        {
            var lstChildProjects = new List<ProjectDetailsItem>();
            var lstParentProjects = new List<ProjectDetailsItem>();

            lstParentProjects = GetParentProjects();
            lstChildProjects = GetChildProjects();
            UpdateLiteralScriptFieldParent(lstParentProjects);
            UpdateLiteralScriptFieldChild(lstChildProjects);
        }
        #region Get Parent Projects
        private List<ProjectDetailsItem> GetParentProjects()
        {
            try
            {
                var lst = dashboardService.getStageGateRequestedProjectDetails(UserFilterForParentProjectList());

                foreach (ProjectDetailsItem item in lst)
                {
                    item.ProjectNumber = "<a href='/Pages/StageGateProjectPanel.aspx?ProjectNo=" + item.ProjectNumber + "'>" + item.ProjectNumber + "</a>";
                }

                return lst;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "GetParentProjects: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "MyOpenProjectsForm", "GetParentProjects");
                return new List<ProjectDetailsItem>();
            }
        }
        #endregion
        #region Get Child Projects
        private List<ProjectDetailsItem> GetChildProjects()
        {
            try
            {
                List<ProjectDetailsItem> ChildProjects = new List<ProjectDetailsItem>();
                List<ProjectDetailsItem> ChildProjectsByTeam = new List<ProjectDetailsItem>();
                List<ProjectDetailsItem> ChildProjectsByInitiator = new List<ProjectDetailsItem>();

                ChildProjectsByTeam = getChildProjectsByProjectTeam();
                ChildProjectsByInitiator = getChildProjectsByInitiator();
                ChildProjects.AddRange(ChildProjectsByTeam);
                ChildProjects.AddRange(ChildProjectsByInitiator);

                var ChildUniqueProjects = ChildProjects
                                            .GroupBy(o => new { o.CompassItemId })
                                            .Select(o => o.FirstOrDefault())
                                            .ToList();

                return ChildUniqueProjects;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "GetChildProjects: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "MyOpenProjectsForm", "GetChildProjects");
                return new List<ProjectDetailsItem>();
            }
        }
        private List<ProjectDetailsItem> getChildProjectsByProjectTeam()
        {
            try
            {
                var ChileProjectTeamResults = dashboardService.getChildProjectsByProjectTeam(UserFilterForChildProjectTeamList());

                List<int> projectNos = new List<int>();
                foreach (ItemProposalItem item in ChileProjectTeamResults)
                {
                    projectNos.Add(item.CompassListItemId);
                }
                List<List<int>> projectNoChunks = SplitList<int>(projectNos, 50);

                var ChildProjects = new List<ProjectDetailsItem>();
                List<ProjectDetailsItem> ChildProject = new List<ProjectDetailsItem>();

                foreach (var projectNoChunk in projectNoChunks)
                {
                    Task.WhenAll(projectNoChunk.Select(async item => ChildProject.AddRange(await dashboardService.getRequestedProjectDetailsAsync(UserFilterForChildProjectListByID(item)))));
                }

                return ChildProject;
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "getChildProjectsByProjectTeam: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "MyOpenProjectsForm", "getChildProjectsByProjectTeam");
                return new List<ProjectDetailsItem>();
            }
        }
        private List<ProjectDetailsItem> getChildProjectsByInitiator()
        {
            try
            {
                return dashboardService.getRequestedProjectDetails(UserFilterForChildProjectListByInitiator());
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "getChildProjectsByInitiator: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "MyOpenProjectsForm", "getChildProjectsByInitiator");
                return new List<ProjectDetailsItem>();
            }
        }
        #endregion
        #endregion
        #region Update Literal scripts
        private void UpdateLiteralScriptFieldParent(List<ProjectDetailsItem> lst)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            litScriptParent.Text = "<script> var ParentProjectDetails=" + js.Serialize(lst) + "</script>";
        }
        private void UpdateLiteralScriptFieldChild(List<ProjectDetailsItem> lst)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = int.MaxValue;
            litScriptChild.Text = "<script> var ChildProjectDetails=" + js.Serialize(lst) + "</script>";
        }
        #endregion
        #region Build Filter
        #region Filter for Parent projects
        private string UserFilterForParentProjectList()
        {
            string userId = string.IsNullOrEmpty(ImpersonationId) ? SPContext.Current.Web.CurrentUser.ID.ToString() : ImpersonationId;

            var userfilter = "";

            userfilter = " <Where>" +
                            "<And>" +
                                "<And>" +
                                    "<Neq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">On Hold</Value></Neq>" +
                                        "<And>" +
                                            "<Neq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">Complete</Value></Neq>" +
                                            "<Neq><FieldRef Name=\"Stage\" /><Value Type=\"Text\">Cancelled</Value></Neq>" +
                                        "</And>" +
                                "</And>" +
                                "<Or><Eq><FieldRef Name=\"ProjectLeader\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                    "<Or><Eq><FieldRef Name=\"ProjectManager\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                        "<Or><Eq><FieldRef Name=\"SeniorProjectManager\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                            "<Or><Eq><FieldRef Name=\"Marketing\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                "<Or><Eq><FieldRef Name=\"InTech\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                    "<Or><Eq><FieldRef Name=\"QAInnovation\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                        "<Or><Eq><FieldRef Name=\"InTechRegulatory\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                            "<Or><Eq><FieldRef Name=\"RegulatoryQA\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                "<Or><Eq><FieldRef Name=\"PackagingEngineering\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                    "<Or><Eq><FieldRef Name=\"SupplyChain\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                        "<Or><Eq><FieldRef Name=\"Finance\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                            "<Or><Eq><FieldRef Name=\"Sales\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                "<Or><Eq><FieldRef Name=\"Manufacturing\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                    "<Or><Eq><FieldRef Name=\"TeamMembers\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                        "<Or><Eq><FieldRef Name=\"ExtMfgProcurement\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                            "<Or><Eq><FieldRef Name=\"PackagingProcurement\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                                 "<Or><Eq><FieldRef Name=\"LifeCycleManagement\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                                    "<Or>" +
                                                                                                        "<Eq><FieldRef Name=\"Legal\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                                        "<Eq><FieldRef Name=\"OtherMember\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                                    "</Or>" +
                                                                                                "</Or>" +
                                                                                            "</Or>" +
                                                                                        "</Or>" +
                                                                                    "</Or>" +
                                                                                "</Or>" +
                                                                            "</Or>" +
                                                                        "</Or>" +
                                                                    "</Or>" +
                                                                "</Or>" +
                                                            "</Or>" +
                                                        "</Or>" +
                                                    "</Or>" +
                                                "</Or>" +
                                            "</Or>" +
                                        "</Or>" +
                                    "</Or>" +
                                "</Or>" +
                            "</And>" +
                         " </Where>";
            return userfilter;
        }
        #endregion
        #region Filter For Child Projects
        private string UserFilterForChildProjectTeamList()
        {
            string userId = string.IsNullOrEmpty(ImpersonationId) ? SPContext.Current.Web.CurrentUser.ID.ToString() : ImpersonationId;

            var userfilter = "";

            userfilter = " <Where>" +
                            "<Or><Eq><FieldRef Name=\"ProjectLeader\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                "<Or><Eq><FieldRef Name=\"ProjectManager\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                    "<Or><Eq><FieldRef Name=\"SeniorProjectManager\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                        "<Or><Eq><FieldRef Name=\"Marketing\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                            "<Or><Eq><FieldRef Name=\"RnD\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                "<Or><Eq><FieldRef Name=\"QAInnovation\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                    "<Or><Eq><FieldRef Name=\"InTechRegulatory\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                        "<Or><Eq><FieldRef Name=\"RegulatoryQA\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                            "<Or><Eq><FieldRef Name=\"PackagingEngineering\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                "<Or><Eq><FieldRef Name=\"SupplyChain\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                    "<Or><Eq><FieldRef Name=\"Finance\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                        "<Or><Eq><FieldRef Name=\"Sales\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                            "<Or><Eq><FieldRef Name=\"Manufacturing\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                "<Or><Eq><FieldRef Name=\"TeamMembers\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                    "<Or><Eq><FieldRef Name=\"ExtMfgProcurement\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                        "<Or><Eq><FieldRef Name=\"PackagingProcurement\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                            "<Or><Eq><FieldRef Name=\"LifeCycleManagement\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                                "<Or>" +
                                                                                                    "<Eq><FieldRef Name=\"Legal\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                                    "<Eq><FieldRef Name=\"OtherMember\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                                                                "</Or>" +
                                                                                            "</Or>" +
                                                                                        "</Or>" +
                                                                                    "</Or>" +
                                                                                "</Or>" +
                                                                            "</Or>" +
                                                                        "</Or>" +
                                                                    "</Or>" +
                                                                "</Or>" +
                                                            "</Or>" +
                                                        "</Or>" +
                                                    "</Or>" +
                                                "</Or>" +
                                            "</Or>" +
                                        "</Or>" +
                                    "</Or>" +
                                "</Or>" +
                            "</Or>" +
                         " </Where>";
            return userfilter;
        }
        private string UserFilterForChildProjectListByID(int itemId)
        {
            var userfilter = "";

            userfilter = "<Where>" +
                            "<And>" +
                                "<And>" +
                                    "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Neq>" +
                                        "<And>" +
                                            "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Neq>" +
                                            "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">On Hold</Value></Neq>" +
                                        "</And>" +
                                "</And>"
                                    +
                                "<Eq><FieldRef Name=\"ID\" LookupId=\"TRUE\" /><Value Type=\"Int\">" + itemId + "</Value></Eq>" +
                            "</And>" +
                        "</Where>";
            return userfilter;
        }
        private string UserFilterForChildProjectListByInitiator()
        {
            string userId = string.IsNullOrEmpty(ImpersonationId) ? SPContext.Current.Web.CurrentUser.ID.ToString() : ImpersonationId; ;
            var userfilter = "";

            userfilter = "<Where>" +
                            "<And>" +
                                "<And>" +
                                    "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Cancelled</Value></Neq>" +
                                        "<And>" +
                                            "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">Completed</Value></Neq>" +
                                            "<Neq><FieldRef Name=\"WorkflowPhase\" /><Value Type=\"Text\">On Hold</Value></Neq>" +
                                        "</And>" +
                                "</And>" +
                                    "<Or><Eq><FieldRef Name=\"PM\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                        "<Or><Eq><FieldRef Name=\"ResearchDevelopmentLead\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                            "<Or>" +
                                                "<Eq><FieldRef Name=\"BrandManager\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                                "<Eq><FieldRef Name=\"Initiator\" LookupId=\"TRUE\" /><Value Type=\"Integer\">" + userId + "</Value></Eq>" +
                                            "</Or>" +
                                        "</Or>" +
                                    "</Or>" +
                            "</And>" +
                        "</Where>";
            return userfilter;
        }
        #endregion
        #endregion
        #region Helper methods
        private List<List<T>> SplitList<T>(List<T> items, int sliceSize = 30)
        {
            List<List<T>> list = new List<List<T>>();

            for (int i = 0; i < items.Count; i += sliceSize)
            {
                list.Add(items.GetRange(i, Math.Min(sliceSize, items.Count - i)));
            }

            return list;
        }
        #endregion
    }
}
