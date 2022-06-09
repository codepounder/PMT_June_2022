using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Interfaces;
using Ferrara.Compass.Abstractions.Models;
using Ferrara.Compass.Classes;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;


namespace Ferrara.Compass.WebParts.StageGateFinancialBriefListForm
{
    [ToolboxItemAttribute(false)]
    public partial class StageGateFinancialBriefForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IStageGateGeneralService stageGateGeneralService;
        private IUserManagementService userManagementService;
        private IStageGateFinancialServices stageGateFinancialServices;

        private int StageGateProjectListItemId;

        private const string _ucConsolidatedFinancialSummaryPath = @"~/_controltemplates/15/Ferrara.Compass/ucConsolidatedFinancialSummary.ascx";
        private const string _ucFinancialBriefPath = @"~/_controltemplates/15/Ferrara.Compass/ucFinancialBrief.ascx";
        #endregion
        #region Properties
        private string ProjectNumber
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo];
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
        private string BriefName;
        private string BriefNo
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_FinancialBrief] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_FinancialBrief];
                return string.Empty;
            }
        }
        #endregion
        #region Constructor
        public StageGateFinancialBriefForm()
        {
        }
        #endregion
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            stageGateFinancialServices = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateFinancialServices>();
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();

            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";
            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;
            }
            else
            {
                StageGateProjectListItemId = Convert.ToInt32(hdnStageGateProjectListItemId.Value);
            }

            if (StageGateProjectListItemId > 0)
            {
                LoadUserControls();
            }
        }
        #endregion
        #region DataTransfer Methods
        private void LoadUserControls()
        {
            List<StageGateGateItem> gateBriefItems = stageGateGeneralService.GetStageGateBriefItem(StageGateProjectListItemId, Convert.ToInt32(GateNo));
            if (gateBriefItems.Count > 0)
            {
                var gateBriefItem = gateBriefItems.Find(x => x.BriefNo == Convert.ToInt32(BriefNo));
                BriefName = gateBriefItem?.BriefName;
            }

            ucConsolidatedFinancialSummary ctrlSummary = (ucConsolidatedFinancialSummary)Page.LoadControl(_ucConsolidatedFinancialSummaryPath);
            ctrlSummary.ProjectNo = ProjectNumber;
            ctrlSummary.StageGateProjectListItemId = StageGateProjectListItemId;
            ctrlSummary.GateNo = Convert.ToInt32(GateNo);
            ctrlSummary.BriefNo = Convert.ToInt32(BriefNo);
            ctrlSummary.BriefName = "Finance " + BriefName;
            lblFinanceBriefPageSubTitle.Text = "Finance Brief G" + GateNo.ToString() + " # " + BriefNo.ToString() + ": " + BriefName;

            phFinancialSummary.Controls.Clear();
            phFinancialSummary.Controls.Add(ctrlSummary);

            ucFinancialBrief ctrlAnalysis = (ucFinancialBrief)Page.LoadControl(_ucFinancialBriefPath);
            ctrlAnalysis.ProjectNo = ProjectNumber;
            ctrlAnalysis.StageGateProjectListItemId = StageGateProjectListItemId;
            ctrlAnalysis.GateNo = Convert.ToInt32(GateNo);
            ctrlAnalysis.BriefNo = Convert.ToInt32(BriefNo);
            ctrlAnalysis.BriefName = "Finance " + BriefName;
            phFinancialBriefAnalysis.Controls.Clear();
            phFinancialBriefAnalysis.Controls.Add(ctrlAnalysis);

            this.hdnStageGateProjectListItemId.Value = StageGateProjectListItemId.ToString();
            this.hdnGate.Value = GateNo.ToString();
            this.hdnBriefNumber.Value = BriefNo.ToString();

            var dtFinancialSummaryItems = stageGateFinancialServices.GetStageGateConsolidatedFinancialSummaryItem(StageGateProjectListItemId, GateNo.ToString(), BriefNo.ToString());

            if (dtFinancialSummaryItems != null)
            {
                if (dtFinancialSummaryItems.ModifiedDate != null && (Utilities.GetMinDate() != dtFinancialSummaryItems.ModifiedDate))
                    lblSaved.Text = "Changes Saved: " + dtFinancialSummaryItems.ModifiedDate;
            }
        }
        private void SaveData(bool Submitted)
        {                   
            foreach (var ctrl in phFinancialSummary.Controls)
            {
                if (ctrl is System.Web.UI.UserControl)
                {
                    var type = (ucConsolidatedFinancialSummary)ctrl;
                    type.saveData(Submitted);
                }
            }

            foreach (var ctrl in phFinancialBriefAnalysis.Controls)
            {
                if (ctrl is System.Web.UI.UserControl)
                {
                    var type = (ucFinancialBrief)ctrl;
                    type.saveData(Submitted);
                }
            }
        }
        #endregion
        #region Button Click Methods
        protected void btnSaveFinacialAnalysis_Click(object sender, EventArgs e)
        {
            SaveData(false);
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        #endregion
        #region Private Methods
        private bool CheckProjectNumber()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                StageGateProjectListItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);
            }

            // Store Id in Hidden field
            this.hdnStageGateProjectListItemId.Value = StageGateProjectListItemId.ToString();
            return true;
        }
        private bool CheckWriteAccess()
        {
            if (userManagementService.HasWriteAccess(CompassForm.StageGateFinancialBrief))
            {
                return true;
            }
            return false;
        }
        private Boolean ValidateForm()
        {
            Boolean bValid = true;


            return bValid;
        }

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
        }
        #endregion
    }
}
