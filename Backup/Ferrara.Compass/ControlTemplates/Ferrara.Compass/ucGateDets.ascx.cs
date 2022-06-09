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
    public partial class ucGateDets : UserControl
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
        public bool readOnly { get; set; }
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
            //saveData(false);
            Utilities.BindDropDownItemsById(ddlMarketingColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            Utilities.BindDropDownItemsById(ddlSalesColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            Utilities.BindDropDownItemsById(ddlFinanceColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            Utilities.BindDropDownItemsById(ddlRDColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            Utilities.BindDropDownItemsById(ddlQAColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            Utilities.BindDropDownItemsById(ddlPEColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            Utilities.BindDropDownItemsById(ddlManuColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            Utilities.BindDropDownItemsById(ddlSupplyChainColor, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
            LoadGateData();
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
        private void LoadGateData()
        {
            StageGateGateItem gateItem = stageGateGeneralService.GetStageGateGateItem(StageGateItemId, Gate);

            Utilities.SetDropDownValue(gateItem.MarketingColor, ddlMarketingColor, Page);
            Utilities.SetDropDownValue(gateItem.SalesColor, ddlSalesColor, Page);
            Utilities.SetDropDownValue(gateItem.FinanceColor, ddlFinanceColor, Page);
            Utilities.SetDropDownValue(gateItem.RDColor, ddlRDColor, Page);
            Utilities.SetDropDownValue(gateItem.QAColor, ddlQAColor, Page);
            Utilities.SetDropDownValue(gateItem.PEColor, ddlPEColor, Page);
            Utilities.SetDropDownValue(gateItem.ManuColor, ddlManuColor, Page);
            Utilities.SetDropDownValue(gateItem.SupplyChainColor, ddlSupplyChainColor, Page);

            ddlMarketingColor.CssClass = "required listColor " + gateItem.MarketingColor;
            ddlSalesColor.CssClass = "required listColor " + gateItem.SalesColor;
            ddlFinanceColor.CssClass = "required listColor " + gateItem.FinanceColor;
            ddlRDColor.CssClass = "required listColor " + gateItem.RDColor;
            ddlQAColor.CssClass = "required listColor " + gateItem.QAColor;
            ddlPEColor.CssClass = "required listColor " + gateItem.PEColor;
            ddlManuColor.CssClass = "required listColor " + gateItem.ManuColor;
            ddlSupplyChainColor.CssClass = "required listColor " + gateItem.SupplyChainColor;

            ddlMarketingColor.Enabled = readOnly;
            ddlSalesColor.Enabled = readOnly;
            ddlFinanceColor.Enabled = readOnly;
            ddlRDColor.Enabled = readOnly;
            ddlQAColor.Enabled = readOnly;
            ddlPEColor.Enabled = readOnly;
            ddlManuColor.Enabled = readOnly;
            ddlSupplyChainColor.Enabled = readOnly;

            txtMarketingComments.Text = gateItem.MarketingComments;
            txtSalesComments.Text = gateItem.SalesComments;
            txtFinanceComments.Text = gateItem.FinanceComments;
            txtRDComments.Text = gateItem.RDComments;
            txtQAComments.Text = gateItem.QAComments;
            txtPEComments.Text = gateItem.PEComments;
            txtManuComments.Text = gateItem.ManuComments;
            txtSupplyChainComments.Text = gateItem.SupplyChainComments;

            txtMarketingComments.Enabled = readOnly;
            txtSalesComments.Enabled = readOnly;
            txtFinanceComments.Enabled = readOnly;
            txtRDComments.Enabled = readOnly;
            txtQAComments.Enabled = readOnly;
            txtPEComments.Enabled = readOnly;
            txtManuComments.Enabled = readOnly;
            txtSupplyChainComments.Enabled = readOnly;

            txtSGMeetingDate.Text = gateItem.SGMeetingDate == DateTime.MinValue ? "" : gateItem.SGMeetingDate.ToString("MM/dd/yyyy");
            txtActualSGMeetingDate.Text = gateItem.ActualSGMeetingDate == DateTime.MinValue ? "" : gateItem.ActualSGMeetingDate.ToString("MM/dd/yyyy");
            ddlSGMeetingStatus.Text = string.IsNullOrEmpty(gateItem.SGMeetingStatus) ? "Waiting" : gateItem.SGMeetingStatus;
            txtSGMeetingDate.Enabled = readOnly;
            txtActualSGMeetingDate.Enabled = readOnly;

            lblGateNumber2.Controls.Add(new LiteralControl(Gate.ToString()));
            lblGateNumber.Controls.Add(new LiteralControl(Gate.ToString()));
            string gatePct = string.IsNullOrEmpty(gateItem.ReadinessPct) ? "0" : gateItem.ReadinessPct;
            pchReadinessPct.Controls.Add(new LiteralControl(gatePct));
        }
        #endregion   
        public void saveData(bool submitted)
        {
            try
            {
                PlaceHolder phMsg = (PlaceHolder)this.Parent.FindControl("phMsg");
                Repeater rptParentDeliverables = (Repeater)this.Parent.FindControl("rptParentDeliverables");
                if (phMsg != null)
                {
                    foreach (UserControl ctrl in phMsg.Controls)
                    {
                        if (ctrl is ucGateDets)
                        {
                            var type = (ucGateDets)ctrl;
                            StageGateGateItem gateItem = new StageGateGateItem();
                            gateItem.SGMeetingDate = txtSGMeetingDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtSGMeetingDate.Text);
                            gateItem.ActualSGMeetingDate = txtActualSGMeetingDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtActualSGMeetingDate.Text);
                            gateItem.ProjectNumber = ProjectNumber;
                            gateItem.StageGateListItemId = StageGateItemId;
                            gateItem.Gate = Gate.ToString();

                            gateItem.MarketingColor = ddlMarketingColor.SelectedItem.Text;
                            gateItem.SalesColor = ddlSalesColor.SelectedItem.Text;
                            gateItem.FinanceColor = ddlFinanceColor.SelectedItem.Text;
                            gateItem.RDColor = ddlRDColor.SelectedItem.Text;
                            gateItem.QAColor = ddlQAColor.SelectedItem.Text;
                            gateItem.PEColor = ddlPEColor.SelectedItem.Text;
                            gateItem.ManuColor = ddlManuColor.SelectedItem.Text;
                            gateItem.SupplyChainColor = ddlSupplyChainColor.SelectedItem.Text;

                            gateItem.MarketingComments = txtMarketingComments.Text;
                            gateItem.SalesComments = txtSalesComments.Text;
                            gateItem.FinanceComments = txtFinanceComments.Text;
                            gateItem.RDComments = txtRDComments.Text;
                            gateItem.QAComments = txtQAComments.Text;
                            gateItem.PEComments = txtPEComments.Text;
                            gateItem.ManuComments = txtManuComments.Text;
                            gateItem.SupplyChainComments = txtSupplyChainComments.Text;
                            if (submitted)
                            {
                                gateItem.FormSubmittedDate = DateTime.Now.ToString();
                                gateItem.FormSubmittedBy = SPContext.Current.Web.CurrentUser.ToString();
                                gateItem.SGMeetingStatus = "Approved";
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(ddlSGMeetingStatus.Text))
                                {
                                    gateItem.SGMeetingStatus = "Waiting";
                                }
                                else
                                {
                                    gateItem.SGMeetingStatus = ddlSGMeetingStatus.Text;
                                }
                            }
                            stageGateGeneralService.UpsertGateDetsItem(gateItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + ": SaveStageGateGateItem: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateNecessaryDeliverables.ToString(), "SaveStageGateGateItem");
            }
        }
    }
}
