using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using Microsoft.Practices.Unity;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using System.Web.UI;
using System.Web;

namespace Ferrara.Compass.WebParts.PMTHeaderForm
{
    [ToolboxItemAttribute(false)]
    public partial class PMTHeaderForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]

        #region Member Variables
        private IProjectHeaderService headerService;
        private IPackagingItemService packagingItemService;
        private IExceptionService exceptionService;
        private IConfigurationManagementService configurationService;
        private IWorkflowService workflowService;
        private int StageGateListItemId = 0;
        public string versionNumber = "99";

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
        private string GateNo
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Gate] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_Gate];
                return string.Empty;
            }
        }
        #endregion
        private string FormName
        {
            get
            {
                string filename;
                filename = System.IO.Path.GetFileName(Page.Request.Url.AbsolutePath);
                return filename;
            }
        }
        public PMTHeaderForm()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
            headerService = DependencyResolution.DependencyMapper.Container.Resolve<IProjectHeaderService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            packagingItemService = DependencyResolution.DependencyMapper.Container.Resolve<IPackagingItemService>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!Page.IsPostBack)
            // {
            try
            {
                if (!CheckProjectNumber())
                {
                    this.lbProjectStatusForm.Visible = false;
                    this.lbExtraLink.Visible = false;
                    this.divProjectTitle.Visible = false;
                    this.divProjectType.Visible = false;
                    this.divProjectTypeSubCategory.Visible = false;
                    this.divProjectStage.Visible = false;
                    this.divPageTitle.Visible = false;
                    return;
                }
                string workflowStatusWindow = @"javascript:window.open('StageGateProjectPanel.aspx?ProjectNo=" + ProjectNumber + "'); return false;";
                this.lbProjectStatusForm.Attributes.Add("onclick", workflowStatusWindow);
                LoadFormData();
                InitializeScreen();
            }
            catch (Exception exception)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, "PMT Header: " + exception.Message);
                exceptionService.Handle(LogCategory.CriticalError, exception, "PMT Header", "Page_Load");
            }
            /*}
            else
            {
                if (!CheckProjectNumber())
                {
                    this.divPMTProjectHeader.Visible = false;
                    return;
                }
            }*/

            string env = Page.Request.Url.Host.ToLower();
            if (env.Contains("spuat"))
            {
                UATMessage.Visible = true;
            }
            if (hddDeveloper.Value == "true")
            {
                btnDebugMode.Visible = true;
            }
        }

        #region Private Methods
        private bool CheckProjectNumber()
        {
            try
            {
                StageGateListItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);

                if (StageGateListItemId == 0)
                    return false;
            }
            catch (Exception ex)
            {
                StageGateListItemId = 0;
                return false;
            }

            return true;
        }
        private void InitializeScreen()
        {
            hddDeveloper.Value = Utilities.CheckIfCurrentUserInGroup(GlobalConstants.GROUP_Developers).ToString().ToLower();
        }
        #endregion

        #region Data Transfer Methods
        public string SetProjectTitle(StageGateProjectHeaderItem item)
        {
            string title = string.Empty;

            if (string.IsNullOrEmpty(item.ProjectNumber))
                title = "XXXXX : ";
            else
                title = item.ProjectNumber + " : ";

            if (string.IsNullOrEmpty(item.ProjectName))
                title = title + "(Proposed)";
            else
                title = title + item.ProjectName;

            return title;
        }
        protected void lbTaskDashboard_Click(object sender, EventArgs e)
        {
            // Redirect to Home page after successfull Submit                        
            Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
        }
        protected void lbExtraLink_Click(object sender, EventArgs e)
        {
            if (Utilities.GetCurrentPageName() == GlobalConstants.PAGE_StageGateFinancialBrief)
            {
                //redirect to Finance summary page
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_ProjectNo, ProjectNumber),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_Gate, GateNo.ToString())
                };

                Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_StageGateFinancialSummary, parameters), false);
            }
        }
        private void LoadFormData()
        {
            StageGateProjectHeaderItem headerItem = headerService.GetStagegateProjectHeaderItem(StageGateListItemId);

            this.lblProjectTitle.Text = SetProjectTitle(headerItem);
            string stage = headerItem.ProjectStage;
            string pageName = Utilities.GetCurrentPageName();
            string pageNameText = "";
            switch (pageName)
            {
                case GlobalConstants.PAGE_StageGateCreateProject:
                    this.divPageTitle.Visible = false;
                    pageNameText = "Project Information Form";
                    break;
                case GlobalConstants.PAGE_StageGateFinancialSummary:
                    pageNameText = "Financial Summary Form";
                    break;
                case GlobalConstants.PAGE_StageGateDesignDeliverables:
                    pageNameText = "Design Checklist";
                    break;
                case GlobalConstants.PAGE_StageGateGenerateIPFs:
                    pageNameText = "Generate IPF Form";
                    break;
                case GlobalConstants.PAGE_StageGateDevelopDeliverables:
                    pageNameText = "Develop/Validate Checklist";
                    break;
                case GlobalConstants.PAGE_StageGateValidateDeliverables:
                    pageNameText = "Develop/Validate Checklist";
                    break;
                case GlobalConstants.PAGE_StageGateIndustrializeDeliverables:
                    pageNameText = "Industrialize Checklist";
                    break;
                case GlobalConstants.PAGE_StageGateLaunchDeliverables:
                    pageNameText = "Launch Checklist";
                    break;
                case GlobalConstants.PAGE_StageGatePostLaunchDeliverables:
                    pageNameText = "Post Launch Checklist";
                    break;
                case GlobalConstants.PAGE_StageGateFinancialBrief:
                    pageNameText = "Financial Brief";
                    lbExtraLink.Visible = true;
                    lbExtraLink.Text = "Financial Summary";
                    break;
                case GlobalConstants.PAGE_StageGateProjectPanel:
                    pageNameText = "Project Summary Form";
                    lbProjectStatusForm.Visible = false;
                    break;
                default:
                    pageNameText = "";
                    break;
            }
            this.lblProjectType.Text = headerItem.ProjectType;
            if (headerItem.ProjectTypeSubCategory == "NA")
            {
                divProjectTypeSubCategory.Visible = false;
            }
            else
            {
                this.lblProjectTypeSubCategory.Text = headerItem.ProjectTypeSubCategory;
            }

            this.lblProjectStage.Text = stage;
            this.lblPageTitle.Text = pageNameText;
            if (headerItem.TestProject.ToLower() == "yes")
            {
                btnDebugMode.Text = "Turn Debug Mode Off";
                DebugMessage.Visible = true;
            }
            else
            {
                btnDebugMode.Text = "Turn Debug Mode On";
                DebugMessage.Visible = false;
            }
        }
        #endregion
        protected void btnDebugMode_Click(object sender, EventArgs e)
        {
            string testProject = "";
            if (btnDebugMode.Text.Contains("On"))
            {
                testProject = "Yes";
            }
            else
            {
                testProject = "No";
            }
            headerService.updateSGSDebugMode(StageGateListItemId, testProject);
            if (testProject.ToLower() == "yes")
            {
                btnDebugMode.Text = "Turn Debug Mode Off";
                DebugMessage.Visible = true;
            }
            else
            {
                btnDebugMode.Text = "Turn Debug Mode On";
                DebugMessage.Visible = false;
            }
            Page.Response.Redirect(HttpContext.Current.Request.Url.ToString(), true);
        }
    }
}
