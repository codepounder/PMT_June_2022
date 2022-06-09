using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Abstractions.Interfaces;
using System.Web;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucSGSProjectInformation : UserControl
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private IStageGateGeneralService stageGateGeneralService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private IExceptionService exceptionService;

        private string webUrl;
        private string URLStage;

        #endregion
        #region Properties
        public int StageGateItemId { get; set; }
        public string ProjectNumber { get; set; }
        public int Gate { get; set; }
        #endregion
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeScreen();

            stageGateCreateProjectService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();

            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;
            URLStage = Utilities.GetCurrentPageName();
            LoadProjectInformation();
        }

        #region Private Methods       
        private void EnsureScriptManager()
        {
            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

            if (scriptManager == null)
            {
                scriptManager = new ScriptManager();
                scriptManager.EnablePartialRendering = true;


                if (Page.Form != null)
                {
                    Page.Form.Controls.AddAt(0, scriptManager);
                }
            }
        }
        private void EnsureUpdatePanelFixups()
        {
            if (this.Page.Form != null)
            {
                String formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];
                if (formOnSubmitAtt == "return _spFormOnSubmitWrapper ();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(),
                "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper = true;", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "setDropdownColor", "setDropdownColor();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "projectStageChange", "projectStageChange();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "updateTotals", "updateTotals();", true);
        }
        private void InitializeScreen()
        {

        }
        #endregion
        #region Data Transfer Methods
        private void LoadProjectInformation()
        {
            StageGateCreateProjectItem stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateItemId);

            lblBrand.Text = stageGateItem.Brand;
            lblConceptOverview.Text = stageGateItem.ProjectConceptOverview;
            lblFirstShipDate.Text = stageGateItem.DesiredShipDate == DateTime.MinValue ? "" : stageGateItem.DesiredShipDate.ToString("MM/dd/yyyy");
            lblRevisedFirstShipDate.Text = stageGateItem.RevisedShipDate == DateTime.MinValue ? stageGateItem.DesiredShipDate.ToString("MM/dd/yyyy") : stageGateItem.RevisedShipDate.ToString("MM/dd/yyyy");
            lblSubmittedDate.Text = stageGateItem.Gate0ApprovedDate == DateTime.MinValue ? "" : stageGateItem.Gate0ApprovedDate.ToString("MM/dd/yyyy");
            lblLOB.Text = stageGateItem.LineOfBisiness;
            lblProjectType.Text = stageGateItem.ProjectType;
            lblProjectLeader.Text = stageGateItem.ProjectLeaderName;
            lblProjectManager.Text = stageGateItem.ProjectManagerName;
            lblProjectTier.Text = stageGateItem.ProjectTier;
            lblSkus.Text = stageGateItem.SKUs;
        }
        #endregion       
    }
}
