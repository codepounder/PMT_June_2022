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
using Ferrara.Compass.Abstractions.Enum;

namespace Ferrara.Compass.ControlTemplates.Ferrara.Compass
{
    public partial class ucSGSProjectGateInfo : UserControl
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
        private IPDFService pdfService;

        private string webUrl;
        private string URLStage;

        #endregion
        #region Properties
        public int StageGateItemId { get; set; }
        public string ProjectNumber { get; set; }
        public int Gate { get; set; }
        public StageGateGateItem gateBriefItem { get; set; }
        bool addBrief { get; set; }
        public CallParentMethod LoadUserControls { get; set; }
        public delegate void CallParentMethod();
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
            pdfService = DependencyResolution.DependencyMapper.Container.Resolve<IPDFService>();

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
        }
        private void InitializeScreen()
        {

        }
        #endregion
        #region Data Transfer Methods
        private void LoadProjectInformation()
        {
            lblPHGate.Controls.Add(new LiteralControl(Gate.ToString()));
            lblGateText.Text = Gate.ToString();
            lblBriefNo.Text = gateBriefItem.BriefNo.ToString();
            lblBriefName.Text = gateBriefItem.BriefName;
            hdnProjectBriefID.Value = gateBriefItem.ID.ToString();
            txtProductFormats.Text = gateBriefItem.ProductFormats;
            txtRetailExecution.Text = gateBriefItem.RetailExecution;
            txtOtherKeyInfo.Text = gateBriefItem.OtherKeyInfo;
            txtMilestones.Text = gateBriefItem.Milestones;
            txtImpactProjectHealth.Text = gateBriefItem.ImpactProjectHealth;
            txtTeamRecommendation.Text = gateBriefItem.TeamRecommendation;
            txtBriefName.Text = gateBriefItem.BriefName;

            Utilities.BindDropDownItems(ddlOverallRisk, GlobalConstants.LIST_OverallRiskLookup, webUrl);
            Utilities.SetDropDownValue(gateBriefItem.OverallRisk, ddlOverallRisk, Page);

            Utilities.BindDropDownItems(ddlOverallStatus, GlobalConstants.LIST_OverallStatusLookup, webUrl);
            Utilities.SetDropDownValue(gateBriefItem.OverallStatus, ddlOverallStatus, Page);
            txtGateReadiness.Text = gateBriefItem.GateReadiness;
            txtRiskReason.Text = gateBriefItem.OverallRiskReason;
            txtStatusReason.Text = gateBriefItem.OverallStatusReason;

            btnCopyBrief.CommandArgument = gateBriefItem.ID.ToString();

        }
        #endregion 
        public void saveData()
        {
            try
            {
                ucSGSProjectGateInfo ctrl = (ucSGSProjectGateInfo)this.Parent.FindControl("gateBriefItem");
                if (ctrl != null)
                {
                    if (ctrl is ucSGSProjectGateInfo)
                    {
                        StageGateGateItem gateItem = new StageGateGateItem();
                        gateItem.ID = Convert.ToInt32(ctrl.hdnProjectBriefID.Value);
                        gateItem.ProductFormats = ctrl.txtProductFormats.Text;
                        gateItem.RetailExecution = ctrl.txtRetailExecution.Text;
                        gateItem.OtherKeyInfo = ctrl.txtOtherKeyInfo.Text;
                        gateItem.OverallRisk = ctrl.ddlOverallRisk.SelectedItem.Text;
                        gateItem.OverallStatus = ctrl.ddlOverallStatus.SelectedItem.Text;
                        gateItem.OverallRiskReason = ctrl.txtRiskReason.Text;
                        gateItem.OverallStatusReason = ctrl.txtStatusReason.Text;
                        gateItem.GateReadiness = ctrl.txtGateReadiness.Text;

                        gateItem.Milestones = ctrl.txtMilestones.Text;
                        gateItem.ImpactProjectHealth = ctrl.txtImpactProjectHealth.Text;
                        gateItem.TeamRecommendation = ctrl.txtTeamRecommendation.Text;
                        gateItem.BriefName = ctrl.txtBriefName.Text;
                        gateItem.StageGateListItemId = StageGateItemId;
                        gateItem.Gate = Gate.ToString();
                        gateItem.ProjectNumber = ProjectNumber;

                        if (hdnDeleted.Value == "true")
                        {
                            stageGateGeneralService.DeleteProjectGateInfo(Convert.ToInt32(ctrl.hdnProjectBriefID.Value));
                        }
                        else
                        {
                            stageGateGeneralService.UpsertGateBriefItem(gateItem);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
        }
        protected void btnDeleteBrief_Click(object sender, EventArgs e)
        {
            try
            {
                saveData();
                int deleteID = Convert.ToInt32(hdnProjectBriefID.Value);
                stageGateGeneralService.DeleteProjectGateInfo(deleteID);

                //LoadUserControls();
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnCopyBrief_Click(object sender, EventArgs e)
        {
            try
            {
                saveData();
                Button button = (Button)sender;
                string copyID = button.CommandArgument;

                StageGateGateItem gateItem = stageGateGeneralService.GetSingleStageGateBriefItem(Convert.ToInt32(copyID));
                gateItem.ID = 0;
                stageGateGeneralService.UpsertGateBriefItem(gateItem);
                LoadUserControls();
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnGenerateBrief_Click(object sender, EventArgs e)
        {
            try
            {

                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_PMTListItemId, StageGateItemId.ToString()),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_Gate, Gate.ToString()),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_FinancialBrief, gateBriefItem.BriefNo.ToString())
                };

                Page.Response.Redirect(Utilities.RedirectPageValue(GlobalConstants.PAGE_StageGateGenerateBriefPDF, parameters), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + " : Gate = " + Gate.ToString() + " : btnCreatePDF_Click: " + ex.Message);
            }
            finally
            {
            }
        }
    }
}
