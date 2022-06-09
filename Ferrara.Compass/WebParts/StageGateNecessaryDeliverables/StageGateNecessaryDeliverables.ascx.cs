using Ferrara.Compass.Abstractions.Constants;
using Ferrara.Compass.Abstractions.Interfaces;
using System;
using System.ComponentModel;
using System.Web;
using Microsoft.Practices.Unity;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Ferrara.Compass.Classes;
using Ferrara.Compass.Abstractions.Enum;
using Ferrara.Compass.Abstractions.Models;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.WebControls;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using System.Web.UI.HtmlControls;
using Ferrara.Compass.ControlTemplates.Ferrara.Compass;

namespace Ferrara.Compass.WebParts.StageGateNecessaryDeliverables
{
    [ToolboxItemAttribute(false)]
    public partial class StageGateNecessaryDeliverables : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private INotificationService notificationService;
        private IStageGateGeneralService stageGateGeneralService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private IExceptionService exceptionService;
        private IConfigurationManagementService configurationService;
        private IPDFService pdfService;
        private IWorkflowService workflowService;

        private int StageGateListItemId;
        private string webUrl;
        private string URLStage;
        private string GlobalStage;
        private int gate = 0;
        private string headerText = "";
        private string nextStageText = "";
        private string lookupList = "";
        private int gateBriefCount = 0;
        private const string _ucSGSProjectInformation = @"~/_controltemplates/15/Ferrara.Compass/ucSGSProjectInformation.ascx";
        private const string _ucGateDets = @"~/_controltemplates/15/Ferrara.Compass/ucGateDets.ascx";
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
        #endregion
        #region Constructor
        public StageGateNecessaryDeliverables()
        {

        }
        #endregion
        #region OnInit
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();

            stageGateCreateProjectService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateCreateProjectService>();
            utilityService = DependencyResolution.DependencyMapper.Container.Resolve<IUtilityService>();
            exceptionService = DependencyResolution.DependencyMapper.Container.Resolve<IExceptionService>();
            userManagementService = DependencyResolution.DependencyMapper.Container.Resolve<IUserManagementService>();
            configurationService = DependencyResolution.DependencyMapper.Container.Resolve<IConfigurationManagementService>();
            stageGateGeneralService = DependencyResolution.DependencyMapper.Container.Resolve<IStageGateGeneralService>();
            pdfService = DependencyResolution.DependencyMapper.Container.Resolve<IPDFService>();
            workflowService = DependencyResolution.DependencyMapper.Container.Resolve<IWorkflowService>();
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();

            EnsureScriptManager();
            EnsureUpdatePanelFixups();
        }
        #endregion
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
                "UpdatePanelFixup", "_spOriginalFormAction = document.forms[0].action; _spSuppressFormOnSubmitWrapper = true;SGSPostbackFuntions();", true);
        }
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;
            URLStage = Utilities.GetCurrentPageName();
            if (URLStage.ToLower() == GlobalConstants.PAGE_StageGateDesignDeliverables.ToLower())
            {
                headerText = "Necessary Items/Deliverables to go to Develop";
                nextStageText = "Design - Necessary Items/Deliverables";
                gate = 1;
                GlobalStage = "Design";
                lookupList = GlobalConstants.LIST_StageGateDesignDeliverables;
            }
            else if (URLStage.ToLower() == GlobalConstants.PAGE_StageGateDevelopDeliverables.ToLower())
            {
                headerText = "Necessary Items/Deliverables to go to Industrialize";
                nextStageText = "Develop - Necessary Items/Deliverables";
                gate = 2;
                GlobalStage = "Develop";
                lookupList = GlobalConstants.LIST_StageGateDevelopDeliverables;
            }
            else if (URLStage.ToLower() == GlobalConstants.PAGE_StageGateValidateDeliverables.ToLower())
            {
                headerText = "Necessary Items/Deliverables to go to Industrialize";
                nextStageText = "Validate - Necessary Items/Deliverables";
                gate = 2;
                GlobalStage = "Validate";
                lookupList = GlobalConstants.LIST_StageGateValidateDeliverables;
            }
            else if (URLStage.ToLower() == GlobalConstants.PAGE_StageGateIndustrializeDeliverables.ToLower())
            {
                headerText = "Necessary Items/Deliverables to go to Launch";
                nextStageText = "Industrialize - Necessary Items/Deliverables";
                gate = 3;
                GlobalStage = "Industrialize";
                lookupList = GlobalConstants.LIST_StageGateIndustrializDeliverables;
            }
            else if (URLStage.ToLower() == GlobalConstants.PAGE_StageGateLaunchDeliverables.ToLower())
            {
                headerText = "Launch - Necessary Items/Deliverables";
                nextStageText = "Deliverables";
                gate = 4;
                GlobalStage = "Launch";
                lookupList = GlobalConstants.LIST_StageGateLaunchDeliverables;
            }
            else if (URLStage.ToLower() == GlobalConstants.PAGE_StageGatePostLaunchDeliverables.ToLower())
            {
                headerText = "Post Launch - Necessary Items/Deliverables";
                nextStageText = "Deliverables";
                gate = 5;
                GlobalStage = "Post Launch";
                lookupList = GlobalConstants.LIST_StageGatePostLaunchDeliverables;
            }
            if (!Page.IsPostBack)
            {
                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                    return;

                if (StageGateListItemId > 0)
                {
                    Utilities.BindDropDownItemsByIdSortByValue(ddlProjectStage, GlobalConstants.LIST_StageLookup, webUrl);
                    LoadFormData();
                }
            }
            else
            {
                StageGateListItemId = Convert.ToInt32(hdnStageGateProjectListItemId.Value);
                if (gate < 4 && gate > 0)
                {
                    gateBriefCount = Convert.ToInt32(hdnGateBriefCount.Value);
                }
                //gate = Convert.ToInt32(hdnGateNumber.Value);
                //GlobalStage = hdnGlobalStage.Value;
            }
            hdnProjectNumber.Value = ProjectNumber;
            hdnCompassListItemId.Value = StageGateListItemId.ToString();
            LoadUserControls();
            string pageName = Utilities.GetCurrentPageName();

            if (hdnStage.Value == GlobalConstants.WORKFLOWPHASE_OnHold)
            {
                btnSubmit.Enabled = false;
            }
        }
        private void Deliverables(string lookupList, string stage, Repeater rptDeliverables)
        {
            List<StageGateNecessaryDeliverablesItem> necessaryDeliverables = stageGateGeneralService.GetStageGateDeliverables(lookupList);
            List<StageGateNecessaryDeliverablesItem> savedNecessaryDeliverables = stageGateGeneralService.GetSavedStageGateDeliverables(StageGateListItemId, stage);
            List<int> indexes = new List<int>();
            foreach (StageGateNecessaryDeliverablesItem savedDeliverable in savedNecessaryDeliverables)
            {
                StageGateNecessaryDeliverablesItem updateDeliverable = (from deliverable in necessaryDeliverables where deliverable.NecessaryDeliverables == savedDeliverable.NecessaryDeliverables select deliverable).FirstOrDefault();
                if (updateDeliverable != null)
                {
                    updateDeliverable.Status = savedDeliverable.Status;
                    updateDeliverable.Comments = savedDeliverable.Comments;
                    updateDeliverable.Applicable = savedDeliverable.Applicable;
                    updateDeliverable.ModifiedBy = Utilities.GetPersonFieldForDisplay(savedDeliverable.ModifiedBy);
                    updateDeliverable.ModifiedDate = savedDeliverable.ModifiedDate;
                }
                else
                {
                    int index = savedNecessaryDeliverables.IndexOf(savedDeliverable);
                    indexes.Add(index);
                }
            }
            foreach (int i in indexes)
            {
                savedNecessaryDeliverables[i].Added = true;
                string modifiedBy = savedNecessaryDeliverables[i].ModifiedBy;
                savedNecessaryDeliverables[i].ModifiedBy = Utilities.GetPersonFieldForDisplay(modifiedBy);
                necessaryDeliverables.Add(savedNecessaryDeliverables[i]);
            }
            rptDeliverables.DataSource = necessaryDeliverables;
            rptDeliverables.DataBind();
        }
        private void LoadFormData()
        {
            try
            {
                var stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(StageGateListItemId);
                hdnProjectType.Value = stageGateItem.ProjectType;
                hdnStage.Value = stageGateItem.Stage;
                if (stageGateItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                {
                    ddlProjectStage.Items.Clear();
                    ddlProjectStage.Items.Add(new ListItem("Select...", "-1"));
                    Utilities.BindDropDownValuesWithColumnFilter(ddlProjectStage, GlobalConstants.LIST_StageLookup, webUrl, "Graphics");
                }
                List<KeyValuePair<DateTime, string>> PrevSubmittedDate = stageGateGeneralService.SubmittedVersions(StageGateListItemId, gate, PMTRiskAssessmentFIelds.SubmittedDate, GlobalConstants.LIST_PMTRAListName, PMTRiskAssessmentFIelds.SubmittedBy);
                string PrevSubmittedDateString = "";
                foreach (KeyValuePair<DateTime, string> dates in PrevSubmittedDate)
                {
                    PrevSubmittedDateString = PrevSubmittedDateString + dates.Key + "<br />";
                }
                hdnPrevSubmittedDate.Value = PrevSubmittedDateString;//SGSGateItem.FormSubmittedDate == DateTime.MinValue ? "" : SGSGateItem.FormSubmittedDate.ToString();
                txtRevisedFirstShipDate.Text = stageGateItem.RevisedShipDate == DateTime.MinValue ? stageGateItem.DesiredShipDate.ToString("MM/dd/yyyy") : stageGateItem.RevisedShipDate.ToString("MM/dd/yyyy");
                List<string> parentStage = new List<string>();
                if (URLStage.ToLower() == GlobalConstants.PAGE_StageGateDevelopDeliverables.ToLower() || URLStage.ToLower() == GlobalConstants.PAGE_StageGateValidateDeliverables.ToLower())
                {
                    parentStage.Add("Develop");
                    parentStage.Add("Validate");
                }
                else
                {
                    parentStage.Add(GlobalStage);
                }
                if (GlobalStage == "Launch")
                {
                    ddlProjectStage.Items.Add("Complete");
                    Utilities.SetDropDownValue(stageGateItem.PostLaunchActive, ddlPostLaunch, Page);
                    if (stageGateItem.Stage == "Complete")
                    {
                        ddlPostLaunch.Enabled = false;
                    }
                    btnSubmit.Text = "Submit/Complete";
                }
                rptParentDeliverables.DataSource = parentStage;
                rptParentDeliverables.DataBind();

                if (gate > 3)
                {
                    sgsGateProjectInfoFooter.Visible = false;
                    sgsGateProjectInfoHeader.Visible = false;
                    phRAGuide.Visible = false;
                }
                if (string.Equals(stageGateItem.ProjectType, GlobalConstants.PROJECTTYPE_SimpleNetworkMove))
                {
                    phRAGuide.Visible = false;
                }

                if (stageGateItem.Stage == "Complete" || stageGateItem.Stage == "Cancelled")
                {
                    Utilities.AddItemToDropDown(this.ddlProjectStage, stageGateItem.Stage, stageGateItem.Stage, true);
                    ddlPostLaunch.Enabled = false;
                }
                Utilities.SetDropDownValue(stageGateItem.Stage, ddlProjectStage, Page);
                //if (ViewState["RevisedFirstShip"] == null)
                //{
                txtRevisedFirstShipDate.Text = stageGateItem.RevisedShipDate == DateTime.MinValue ? stageGateItem.DesiredShipDate.ToString("MM/dd/yyyy") : stageGateItem.RevisedShipDate.ToString("MM/dd/yyyy");
                /* ViewState["RevisedFirstShip"] = txtRevisedFirstShipDate.Text;
             }
             else
             {
                 txtRevisedFirstShipDate.Text = (String)ViewState["RevisedFirstShip"];
             }*/


                lblBriefStage.Text = gate.ToString();
                if (gate < 4 && gate > 0)
                {
                    //Gate Project Details
                    //saveUserControls(false);
                    //gateBriefItemsCopy = new List<StageGateGateItem>();
                    // if (ViewState["GateBriefs"] == null)
                    //{
                    List<StageGateGateItem> gateBriefItems = stageGateGeneralService.GetStageGateBriefItem(StageGateListItemId, gate);
                    gateBriefCount = gateBriefItems.Count;
                    if (gateBriefItems.Count <= 0)
                    {
                        gateBriefCount = 1;
                        StageGateGateItem gateItem = new StageGateGateItem();
                        gateItem.StageGateListItemId = StageGateListItemId;
                        gateItem.Gate = gate.ToString();
                        gateItem.ProjectNumber = ProjectNumber;
                        gateItem.BriefNo = 1;
                        gateItem.ID = stageGateGeneralService.UpsertGateBriefItem(gateItem);
                        gateBriefItems.Add(gateItem);
                    }
                    hdnGateBriefCount.Value = gateBriefCount.ToString();
                    List<StageGateGateItem> gateBriefItemsCopy = new List<StageGateGateItem>();
                    foreach (StageGateGateItem brief in gateBriefItems)
                    {
                        if (brief.Deleted != "Yes")
                        {
                            gateBriefItemsCopy.Add(brief);
                        }
                    }
                    /* ViewState["GateBriefs"] = gateBriefItemsCopy;
                 }
                 else
                 {
                     gateBriefItemsCopy = (List<StageGateGateItem>)ViewState["GateBriefs"];
                 }*/
                    rptGateProjectInfo.DataSource = gateBriefItemsCopy;
                    rptGateProjectInfo.DataBind();

                }
                LoadGateAttachments();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + ": LoadFormData: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateNecessaryDeliverables.ToString(), "LoadFormData");
            }
        }
        private void LoadUserControls()
        {
            SGSGateInfo.Controls.Clear();
            phMsg.Controls.Clear();

            //Project Information
            ucSGSProjectInformation ctrl2 = (ucSGSProjectInformation)Page.LoadControl(_ucSGSProjectInformation);
            ctrl2.StageGateItemId = StageGateListItemId;
            ctrl2.ID = "ucSGSProjectInformation";
            SGSGateInfo.Controls.Add(ctrl2);
            //Gate Color Details
            ucGateDets ctrl3 = (ucGateDets)Page.LoadControl(_ucGateDets);
            ctrl3.StageGateItemId = StageGateListItemId;
            ctrl3.ProjectNumber = ProjectNumber;
            ctrl3.Gate = gate;
            ctrl3.readOnly = true;
            ctrl3.ID = "ucGateDets";
            phMsg.Controls.Add(ctrl3);
        }
        #endregion
        #region RepeaterMethods
        protected void rptGateProjectInfo_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                StageGateGateItem gateBriefItem = (StageGateGateItem)e.Item.DataItem;
                if (gateBriefItem.BriefNo <= 0)
                {
                    gateBriefItem.BriefNo = gateBriefCount + 1;
                }

                PlaceHolder SGSProjectInformation = (PlaceHolder)e.Item.FindControl("SGSProjectInformation");

                Label lblPHGate = (Label)e.Item.FindControl("lblPHGate");
                Label lblGateText = (Label)e.Item.FindControl("lblGateText");
                Label lblBriefNo = (Label)e.Item.FindControl("lblBriefNo");
                Label lblBriefName = (Label)e.Item.FindControl("lblBriefName");

                HtmlTextArea txtProductFormatsT = (HtmlTextArea)e.Item.FindControl("txtProductFormatsT");
                HtmlTextArea txtOtherKeyInfoT = (HtmlTextArea)e.Item.FindControl("txtOtherKeyInfoT");
                HtmlTextArea txtMilestonesT = (HtmlTextArea)e.Item.FindControl("txtMilestonesT");
                HtmlTextArea txtImpactProjectHealthT = (HtmlTextArea)e.Item.FindControl("txtImpactProjectHealthT");
                HtmlTextArea txtTeamRecommendationT = (HtmlTextArea)e.Item.FindControl("txtTeamRecommendationT");

                /*HiddenField txtProductFormats = (HiddenField)e.Item.FindControl("txtProductFormatsT");
                HiddenField txtOtherKeyInfo = (HiddenField)e.Item.FindControl("txtOtherKeyInfoT");
                HiddenField txtMilestones = (HiddenField)e.Item.FindControl("txtMilestonesT");
                HiddenField txtImpactProjectHealth = (HiddenField)e.Item.FindControl("txtImpactProjectHealthT");
                HiddenField txtTeamRecommendation = (HiddenField)e.Item.FindControl("txtTeamRecommendationT");*/

                TextBox txtRetailExecution = (TextBox)e.Item.FindControl("txtRetailExecution");
                TextBox txtBriefName = (TextBox)e.Item.FindControl("txtBriefName");
                TextBox txtGateReadiness = (TextBox)e.Item.FindControl("txtGateReadiness");
                TextBox txtRiskReason = (TextBox)e.Item.FindControl("txtRiskReason");
                TextBox txtStatusReason = (TextBox)e.Item.FindControl("txtStatusReason");
                DropDownList ddlOverallRisk = (DropDownList)e.Item.FindControl("ddlOverallRisk");
                DropDownList ddlOverallStatus = (DropDownList)e.Item.FindControl("ddlOverallStatus");
                HiddenField hdnProjectBriefID = (HiddenField)e.Item.FindControl("hdnProjectBriefID");
                Button btnCopyBrief = (Button)e.Item.FindControl("btnCopyBrief");
                Button btnGenerateBrief = (Button)e.Item.FindControl("btnGenerateBrief");
                Button btnDeleteBrief = (Button)e.Item.FindControl("btnDeleteBrief");
                Button btnUploadImageBrief = (Button)e.Item.FindControl("btnUploadImageBrief");
                CheckBox chkFinanceBriefInGateBrief = (CheckBox)e.Item.FindControl("chkFinanceBriefInGateBrief");

                lblPHGate.Controls.Add(new LiteralControl(gate.ToString()));
                lblGateText.Text = gate.ToString();
                lblBriefNo.Text = gateBriefItem.BriefNo.ToString();
                lblBriefName.Text = gateBriefItem.BriefName;

                hdnProjectBriefID.Value = gateBriefItem.ID.ToString();

                txtProductFormatsT.InnerHtml = gateBriefItem.ProductFormats;
                txtOtherKeyInfoT.InnerHtml = gateBriefItem.OtherKeyInfo;
                txtMilestonesT.InnerHtml = gateBriefItem.Milestones;
                txtImpactProjectHealthT.InnerHtml = gateBriefItem.ImpactProjectHealth;
                txtTeamRecommendationT.InnerHtml = gateBriefItem.TeamRecommendation;
                /*txtProductFormats.Value = gateBriefItem.ProductFormats;
                txtOtherKeyInfo.Value = gateBriefItem.OtherKeyInfo;
                txtMilestones.Value = gateBriefItem.Milestones;
                txtImpactProjectHealth.Value = gateBriefItem.ImpactProjectHealth;
                txtTeamRecommendation.Value = gateBriefItem.TeamRecommendation;*/
                txtBriefName.Text = gateBriefItem.BriefName;

                Utilities.BindDropDownItems(ddlOverallRisk, GlobalConstants.LIST_OverallRiskLookup, webUrl);
                Utilities.SetDropDownValue(gateBriefItem.OverallRisk, ddlOverallRisk, Page);

                Utilities.BindDropDownItemsById(ddlOverallStatus, GlobalConstants.LIST_GateDetailColorsLookup, webUrl);
                Utilities.SetDropDownValue(gateBriefItem.OverallStatus, ddlOverallStatus, Page);
                txtGateReadiness.Text = gateBriefItem.GateReadiness;
                txtRiskReason.Text = gateBriefItem.OverallRiskReason;
                txtStatusReason.Text = gateBriefItem.OverallStatusReason;
                txtRetailExecution.Text = gateBriefItem.RetailExecution;

                btnCopyBrief.CommandArgument = gateBriefItem.ID.ToString();
                btnDeleteBrief.CommandArgument = gateBriefItem.ID.ToString();
                btnGenerateBrief.CommandArgument = gateBriefItem.BriefNo.ToString();
                btnUploadImageBrief.CommandArgument = gateBriefItem.BriefNo.ToString();

                chkFinanceBriefInGateBrief.Checked = (gateBriefItem.FinanceBriefInGateBrief == "Yes") ? true : false;

                var files = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateBriefImage, gate.ToString(), gateBriefItem.BriefNo.ToString(), webUrl);
                //HtmlGenericControl fileTable = (HtmlGenericControl)e.Item.FindControl("fileTable");
                Button lnkOtherAttachment = (Button)e.Item.FindControl("lnkOtherAttachment");
                HtmlAnchor briefImage = (HtmlAnchor)e.Item.FindControl("briefImage");
                if (files.Count > 0)
                {
                    btnUploadImageBrief.Visible = false;
                    lnkOtherAttachment.Visible = true;
                    lnkOtherAttachment.CommandArgument = files[0].FileUrl;
                    briefImage.InnerText = files[0].FileName;
                    briefImage.HRef = files[0].FileUrl;
                    briefImage.Visible = true;
                }
                else
                {
                    btnUploadImageBrief.Visible = true;
                    briefImage.Visible = false;
                    lnkOtherAttachment.Visible = false;
                }

                var FinacialBriefDocument = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateBriefPDF, gate.ToString(), gateBriefItem.BriefNo.ToString(), webUrl);
                if (FinacialBriefDocument.Count > 0)
                {
                    HtmlAnchor ancBriefPDF = ((HtmlAnchor)e.Item.FindControl("ancBriefPDF"));
                    if (ancBriefPDF != null)
                    {
                        string fileName = FinacialBriefDocument[0].FileName;
                        fileName = fileName.Replace("_", " ");
                        ancBriefPDF.InnerText = gateBriefItem.BriefName + ".pdf";
                        ancBriefPDF.HRef = FinacialBriefDocument[0].FileUrl;
                        ancBriefPDF.Visible = true;
                    }
                }
                /*if (files.Count <= 0 && FinacialBriefDocument.Count <= 0)
                {
                    fileTable.Visible = false;
                }
                else
                {
                    fileTable.Visible = true;
                }*/
            }
        }
        protected void rptGateProjectInfo_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            ConstructFormData(false);
            //save gate info
            if (Page.IsPostBack)
            {
                saveUserControls(false);
            }
            string id = e.CommandArgument.ToString();
            if (e.CommandName.ToLower() == "delete")
            {
                stageGateGeneralService.DeleteProjectGateInfo(Convert.ToInt32(id));
                rebindRepeater();
            }
            else if (e.CommandName.ToLower() == "copy")
            {

                StageGateGateItem gateItem = stageGateGeneralService.GetSingleStageGateBriefItem(Convert.ToInt32(id));
                gateBriefCount = Convert.ToInt32(hdnGateBriefCount.Value) + 1;
                gateItem.BriefNo = gateBriefCount;
                gateItem.ID = 0;
                int newId = stageGateGeneralService.UpsertGateBriefItem(gateItem);
                rebindRepeater();
            }
            else if (e.CommandName.ToLower() == "generate")
            {
                try
                {
                    string[] commandArgs = e.CommandArgument.ToString().Split(new char[] { ',' });
                    string BriefNo = commandArgs[0];
                    HtmlAnchor ancBriefPDF = ((HtmlAnchor)e.Item.FindControl("ancBriefPDF"));
                    CheckBox chkFinanceBriefInGateBrief = ((CheckBox)e.Item.FindControl("chkFinanceBriefInGateBrief"));

                    if (ancBriefPDF.Visible)
                    {
                        stageGateGeneralService.DeleteStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateBriefPDF, gate.ToString(), BriefNo.ToString());
                    }

                    var BriefDocumentName = pdfService.StageGateGenerateBriefPDF(ProjectNumber, Convert.ToInt32(StageGateListItemId), Convert.ToInt32(gate), Convert.ToInt32(BriefNo), chkFinanceBriefInGateBrief.Checked);
                    if (!string.IsNullOrEmpty(BriefDocumentName.FileName))
                    {
                        var BriefDocument = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateBriefPDF, gate.ToString(), BriefNo.ToString(), webUrl);
                        if (BriefDocument.Count > 0)
                        {
                            Label lblBriefName = (Label)e.Item.FindControl("lblBriefName");
                            if (ancBriefPDF != null)
                            {
                                ancBriefPDF.Visible = true;
                                ancBriefPDF.InnerText = lblBriefName.Text;
                                ancBriefPDF.HRef = BriefDocument[0].FileUrl;
                            }
                        }
                    }
                    rebindRepeater();
                    Label lblGeneratedBrief = (Label)rptGateProjectInfo.Controls[e.Item.ItemIndex].FindControl("lblGeneratedBrief");
                    lblGeneratedBrief.Text = "PDF Files Generated: " + DateTime.Now;
                }
                catch (Exception ex)
                {
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + " : Gate = " + gate.ToString() + " : btnCreatePDF_Click: " + ex.Message);
                }
                finally
                {
                }
            }
            else if (e.CommandName.ToLower() == "upload")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openBriefAttachment('Stage Gate Brief Image', 'Upload Brief Image', '" + gate + "', '" + id + "')", true);
            }
            else if (e.CommandName.ToLower() == "deleteimage")
            {
                utilityService.DeleteAttachment(id);
                rebindRepeater();
            }

        }
        protected void rptParentDeliverables_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                List<KeyValuePair<string, int>> gateForDeliv = new List<KeyValuePair<string, int>>();
                string stage = (String)e.Item.DataItem;

                if (stage == "Validate")
                {
                    headerText = "Necessary Items/Deliverables to go to Industrialize";
                    nextStageText = "Validate - Necessary Items/Deliverables";
                    gate = 2;
                    GlobalStage = "Validate";
                    lookupList = GlobalConstants.LIST_StageGateValidateDeliverables;
                }
                else if (stage == "Develop")
                {
                    headerText = "Necessary Items/Deliverables to go to Industrialize";
                    nextStageText = "Develop - Necessary Items/Deliverables";
                    gate = 2;
                    GlobalStage = "Develop";
                    lookupList = GlobalConstants.LIST_StageGateDevelopDeliverables;
                }
                Button btnAddDeliverable = (Button)e.Item.FindControl("btnAddDeliverable");
                btnAddDeliverable.CommandArgument = stage;
                hdnGateNumber.Value = gate.ToString();

                HiddenField hdnStage = (HiddenField)e.Item.FindControl("hdnStage");
                HiddenField hdnLookupList = (HiddenField)e.Item.FindControl("hdnLookupList");
                HiddenField hdnNextStageText = (HiddenField)e.Item.FindControl("hdnNextStageText");
                HtmlGenericControl lblNextStage = (HtmlGenericControl)e.Item.FindControl("lblNextStage");
                HtmlGenericControl gateReadiness = (HtmlGenericControl)e.Item.FindControl("gateReadiness");

                hdnStage.Value = stage;
                hdnLookupList.Value = lookupList;
                hdnNextStageText.Value = nextStageText;
                lblNextStage.InnerText = headerText;

                if (gate > 3)
                {
                    gateReadiness.Visible = false;
                }
                if (stage == "Launch")
                {
                    PanelPostLaunch.Visible = true;
                }
                #region Stages
                Repeater rptDeliverables = (Repeater)e.Item.FindControl("rptDeliverables");
                Deliverables(lookupList, stage, rptDeliverables);

                List<StageGateNecessaryDeliverablesItem> necDelivPct = new List<StageGateNecessaryDeliverablesItem>();
                int totalDelivs = 0;
                int delivsCompleted = 0;
                foreach (RepeaterItem item in rptDeliverables.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        string status = ((DropDownList)item.FindControl("ddlStatus")).SelectedItem.Text;
                        string applicable = ((DropDownList)item.FindControl("ddlApplicable")).SelectedItem.Text;
                        if (applicable == "Yes")
                        {
                            totalDelivs++;
                            if (status == "Completed")
                            {
                                delivsCompleted++;
                            }
                        }
                    }
                }
                int stagePct = 0;
                Label totalApplicable = (Label)rptDeliverables.Controls[rptDeliverables.Controls.Count - 1].Controls[0].FindControl("lblTotalApplicable");
                Label totalCompleted = (Label)rptDeliverables.Controls[rptDeliverables.Controls.Count - 1].Controls[0].FindControl("lblTotalCompleted");

                if (totalDelivs > 0)
                {
                    stagePct = (int)Math.Round((double)(100 * delivsCompleted) / totalDelivs, MidpointRounding.AwayFromZero);

                }
                totalApplicable.Text = totalDelivs.ToString();
                totalCompleted.Text = delivsCompleted.ToString();

                HtmlGenericControl lblPrevStage = (HtmlGenericControl)rptDeliverables.Controls[0].Controls[0].FindControl("lblPrevStage");
                lblPrevStage.InnerText = hdnNextStageText.Value;
                #endregion

                #region Gates
                StageGateGateItem gateItem = stageGateGeneralService.GetStageGateGateItem(StageGateListItemId, gate);
                HtmlGenericControl pchGateNumber = (HtmlGenericControl)e.Item.FindControl("pchGateNumber");
                pchGateNumber.InnerText = hdnGateNumber.Value;

                HiddenField hdnDeliverablesTotalApplicable = (HiddenField)e.Item.FindControl("hdnDeliverablesTotalApplicable");
                HiddenField hdnDeliverablesTotalCompleted = (HiddenField)e.Item.FindControl("hdnDeliverablesTotalCompleted");
                hdnDeliverablesTotalApplicable.Value = gateItem.DeliverablesApplicable.ToString();
                hdnDeliverablesTotalCompleted.Value = gateItem.DeliverablesCompleted.ToString();
                totalApplicable.Text = gateItem.DeliverablesApplicable.ToString();
                totalCompleted.Text = gateItem.DeliverablesCompleted.ToString();

                HiddenField hdnStagePct = (HiddenField)e.Item.FindControl("hdnStagePct");
                hdnStagePct.Value = stagePct.ToString();
                HtmlGenericControl pchGatePct = (HtmlGenericControl)e.Item.FindControl("pchGatePct");
                pchGatePct.InnerText = stagePct.ToString();

                #endregion
            }
        }
        protected void rptDeliverables_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                StageGateNecessaryDeliverablesItem deliverable = (StageGateNecessaryDeliverablesItem)e.Item.DataItem;

                Label lblDeliverable = (Label)e.Item.FindControl("lblDeliverable");
                Label lblOwner = (Label)e.Item.FindControl("lblOwner");
                Label lblModifiedBy = (Label)e.Item.FindControl("lblModifiedBy");
                Label lblModifiedDate = (Label)e.Item.FindControl("lblModifiedDate");
                DropDownList ddlStatus = (DropDownList)e.Item.FindControl("ddlStatus");
                DropDownList ddlApplicable = (DropDownList)e.Item.FindControl("ddlApplicable");
                TextBox txtComments = (TextBox)e.Item.FindControl("txtComments");
                Image btnDeleteRow = (Image)e.Item.FindControl("btnDeleteRow");
                HiddenField hdnDeliverableId = (HiddenField)e.Item.FindControl("hdnDeliverableId");


                Utilities.BindDropDownItems(ddlStatus, GlobalConstants.LIST_StageGateStageStatus, webUrl);
                Utilities.SetDropDownValue(deliverable.Status, ddlStatus, Page);
                if (deliverable.Added)
                {
                    TextBox txtDeliverable = (TextBox)e.Item.FindControl("txtDeliverable");
                    TextBox txtOwner = (TextBox)e.Item.FindControl("txtOwner");
                    txtDeliverable.Text = deliverable.NecessaryDeliverables;
                    txtDeliverable.Visible = true;
                    txtDeliverable.Attributes.Add("onclick", "changeMade('" + txtDeliverable.ClientID + "');");
                    txtOwner.Text = deliverable.Owner;
                    txtOwner.Visible = true;
                    txtOwner.Attributes.Add("onclick", "changeMade('" + txtOwner.ClientID + "');");

                    btnDeleteRow.Visible = true;
                    hdnDeliverableId.Value = deliverable.DelivId.ToString();
                }
                else
                {
                    if (deliverable.Subtask != "Y")
                    {
                        lblDeliverable.Text = deliverable.NecessaryDeliverables;
                    }
                    else
                    {
                        lblDeliverable.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + deliverable.NecessaryDeliverables;
                    }
                    lblOwner.Text = deliverable.Owner;
                }
                Utilities.SetDropDownValue(deliverable.Applicable, ddlApplicable, Page);
                ddlApplicable.Attributes.Add("onchange", "changeMade('" + ddlApplicable.ClientID + "');updateStatus();updateTotals();");
                ddlStatus.Attributes.Add("onchange", "changeMade('" + ddlStatus.ClientID + "');updateStatus();updateTotals();");
                txtComments.Text = deliverable.Comments;
                txtComments.Attributes.Add("onclick", "changeMade('" + txtComments.ClientID + "');");
                lblModifiedBy.Text = deliverable.ModifiedBy;
                lblModifiedDate.Text = deliverable.ModifiedDate == DateTime.MinValue ? "" : deliverable.ModifiedDate.ToString("MM/dd/yyyy");
            }
        }
        protected void rpGateAttachments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string fileName = e.CommandArgument.ToString();

                utilityService.DeleteAttachment(fileName);
                LoadGateAttachments();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>SGSPostbackFuntions();</script>", false);
            }
        }
        #endregion
        #region Button Methods
        protected void btnUploadDocs_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openBriefAttachment('Stage Gate Gate Document', 'Upload Gate Document', '" + gate + "', '0')", true);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + ": btnUploadDocs_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateNecessaryDeliverables.ToString(), "btnUploadDocs_Click");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                /*if (!CheckWriteAccess())
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }*/
                if (StageGateListItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                ConstructFormData(false);
                //save gate info
                saveUserControls(false);
                //redoing JS for Postback

                rebindRepeater();
                lblSaved.Text = "Changes Saved: " + DateTime.Now.ToString();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "btnSubmit_Click");
            }
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            rebindRepeater();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                /*if (!CheckWriteAccess())
                {
                    ErrorSummary.AddError("You do not have proper access rights to submit this page!", this.Page);
                    return;
                }*/
                if (StageGateListItemId <= 0)
                {
                    ErrorSummary.AddError("Invalid Item Id! Project cannot be updated!", this.Page);
                    return;
                }

                if (!ConstructFormData(true))
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "setFocusError();", true);
                    return;
                }
                //save gate info
                saveUserControls(true);
                //redoing JS for Postback

                if (GlobalStage == "Post Launch" && URLStage.ToLower() == GlobalConstants.PAGE_StageGateLaunchDeliverables.ToLower())
                {
                    workflowService.StartSpecificSGSWorkflow(StageGateListItemId, "Post Launch Notifications Workflow");
                    Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGatePostLaunchDeliverables, ProjectNumber), false);
                }
                else
                {
                    Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGateProjectPanel, ProjectNumber), false);
                }

            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "btnSubmit_Click");
            }
        }
        private void saveUserControls(bool submitted)
        {
            try
            {
                foreach (var ctrl in phMsg.Controls)
                {
                    if (ctrl is ucGateDets)
                    {
                        var type = (ucGateDets)ctrl;

                        type.saveData(submitted);

                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnAddDeliverable_Click(object sender, EventArgs e)
        {
            ConstructFormData(false);
            saveUserControls(false);
            List<StageGateNecessaryDeliverablesItem> savedNecessaryDeliverables = new List<StageGateNecessaryDeliverablesItem>();
            StageGateNecessaryDeliverablesItem newItem = new StageGateNecessaryDeliverablesItem();
            Button clicked = (Button)sender;
            string tempStage = clicked.CommandArgument.ToString();
            newItem.ProjectId = StageGateListItemId;
            newItem.ProjectNumber = ProjectNumber;
            newItem.Stage = tempStage;
            savedNecessaryDeliverables.Add(newItem);
            int newId = stageGateGeneralService.UpsertDeliverables(StageGateListItemId, tempStage, savedNecessaryDeliverables, ProjectNumber);
            newItem.DelivId = newId;
            rebindRepeater();
        }
        protected void btnGenerateBrief_Click(object sender, EventArgs e)
        {
            try
            {
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>()
                {
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_PMTListItemId, StageGateListItemId.ToString()),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_Gate, hdnGateNumber.Value),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_FinancialBrief,""),
                   new KeyValuePair<string, string>(GlobalConstants.QUERYSTRING_BriefType, "finance")
                };
                string url = Utilities.RedirectPageValue(GlobalConstants.PAGE_StageGateGenerateBriefPDF, parameters);
                Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('" + url + "','_newtab');", true);

            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + " : Gate = " + hdnGateNumber.Value + " : btnCreatePDF_Click: " + ex.Message);
            }
            finally
            {
            }
        }

        #endregion
        #region Private Methods
        private bool CheckProjectNumber()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                StageGateListItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);
            }

            // Store Id in Hidden field
            this.hdnStageGateProjectListItemId.Value = StageGateListItemId.ToString();
            return true;
        }
        private bool CheckWriteAccess()
        {
            if (userManagementService.HasWriteAccess(CompassForm.StageGateNecessaryDeliverables))
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
        private void LoadGateAttachments()
        {
            var files = stageGateGeneralService.GetUploadedStageGateFiles(ProjectNumber, GlobalConstants.DOCTYPE_StageGateGateDocument, gate.ToString(), "0", webUrl);

            if (files.Count > 0)
            {
                rpGateAttachments.Visible = true;
                rpGateAttachments.DataSource = files;
                rpGateAttachments.DataBind();

                rpGateAttachments.Visible = true;
                rpGateAttachments.DataSource = files;
                rpGateAttachments.DataBind();
            }
            else
            {
                rpGateAttachments.Visible = false;
                rpGateAttachments.Visible = false;
            }
        }
        #endregion
        #region Data Transfer Methods
        private bool ConstructFormData(bool submitted)
        {
            string strErrors = string.Empty;
            string saveGate = hdnGateNumber.Value;
            int applicableNumber = 0;
            int completedNumber = 0;
            bool valid = true;
            try
            {
                foreach (RepeaterItem parentItem in rptParentDeliverables.Items)
                {
                    if (parentItem.ItemType == ListItemType.Item || parentItem.ItemType == ListItemType.AlternatingItem)
                    {
                        HiddenField hdnStage = (HiddenField)parentItem.FindControl("hdnStage");
                        string stage = hdnStage.Value;
                        List<StageGateNecessaryDeliverablesItem> necessaryDeliverables = new List<StageGateNecessaryDeliverablesItem>();
                        Repeater rptDeliverables = (Repeater)parentItem.FindControl("rptDeliverables");
                        foreach (RepeaterItem item in rptDeliverables.Items)
                        {
                            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                            {
                                HiddenField hdnChangeMade = (HiddenField)item.FindControl("hdnChangeMade");
                                HiddenField hdnDeletedStatus = (HiddenField)item.FindControl("hdnDeletedStatus");
                                HiddenField hdnDeliverableId = (HiddenField)item.FindControl("hdnDeliverableId");
                                string status = ((DropDownList)item.FindControl("ddlStatus")).SelectedItem.Text;
                                string applicable = ((DropDownList)item.FindControl("ddlApplicable")).SelectedItem.Text;
                                if (applicable == "Yes")
                                {
                                    applicableNumber++;
                                }
                                if (status == "Completed")
                                {
                                    completedNumber++;
                                }
                                if (hdnChangeMade.Value == "true")
                                {
                                    if (applicable == "No")
                                    {
                                        status = "N/A";
                                    }
                                    if (!string.IsNullOrEmpty(((Label)item.FindControl("lblDeliverable")).Text))
                                    {
                                        string deliverable = ((Label)item.FindControl("lblDeliverable")).Text;
                                        string owner = ((Label)item.FindControl("lblOwner")).Text;
                                        if (deliverable.IndexOf("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") != -1)
                                        {
                                            deliverable = deliverable.Remove(0, 42);
                                        }
                                        var stageGateItem = new StageGateNecessaryDeliverablesItem
                                        {
                                            ProjectId = Convert.ToInt32(hdnStageGateProjectListItemId.Value),
                                            ProjectNumber = ProjectNumber,
                                            Stage = stage,
                                            NecessaryDeliverables = deliverable,
                                            Owner = owner,
                                            Applicable = applicable,
                                            Status = status,
                                            Comments = ((TextBox)item.FindControl("txtComments")).Text,
                                            DelivId = Convert.ToInt32(hdnDeliverableId.Value)
                                        };
                                        necessaryDeliverables.Add(stageGateItem);
                                    }
                                    else if (hdnDeletedStatus.Value != "true")
                                    {
                                        TextBox tdOwner = item.FindControl("txtOwner") as TextBox;
                                        TextBox tdDeliverable = item.FindControl("txtDeliverable") as TextBox;
                                        var stageGateItem = new StageGateNecessaryDeliverablesItem
                                        {
                                            ProjectId = Convert.ToInt32(hdnStageGateProjectListItemId.Value),
                                            ProjectNumber = ProjectNumber,
                                            Stage = stage,
                                            NecessaryDeliverables = tdDeliverable.Text,
                                            Owner = tdOwner.Text,
                                            Applicable = applicable,
                                            Status = status,
                                            Comments = ((TextBox)item.FindControl("txtComments")).Text,
                                            DelivId = Convert.ToInt32(hdnDeliverableId.Value)
                                        };
                                        necessaryDeliverables.Add(stageGateItem);
                                    }
                                    else if (hdnDeletedStatus.Value == "true")
                                    {
                                        stageGateGeneralService.deleteDeliverable(Convert.ToInt32(hdnDeliverableId.Value));
                                    }
                                }
                            }

                        }
                        int ReadinessPct = (int)Math.Round((double)(100 * completedNumber) / applicableNumber, MidpointRounding.AwayFromZero);
                        if (ReadinessPct < 0)
                        {
                            ReadinessPct = 0;
                        }
                        StageGateGateItem gateItem = new StageGateGateItem
                        {
                            DeliverablesApplicable = applicableNumber,
                            DeliverablesCompleted = completedNumber,
                            ReadinessPct = ReadinessPct.ToString(),
                            StageGateListItemId = StageGateListItemId,
                            ProjectNumber = ProjectNumber,
                            Gate = gate.ToString()
                        };
                        stageGateGeneralService.updateReadinessDetails(gateItem);
                        if (necessaryDeliverables.Count > 0)
                        {
                            int upsertedId = stageGateGeneralService.UpsertDeliverables(StageGateListItemId, stage, necessaryDeliverables, ProjectNumber);
                        }
                    }
                }
                foreach (RepeaterItem item in rptGateProjectInfo.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox txtRetailExecution = (TextBox)item.FindControl("txtRetailExecution");
                        TextBox txtBriefName = (TextBox)item.FindControl("txtBriefName");
                        TextBox txtGateReadiness = (TextBox)item.FindControl("txtGateReadiness");
                        TextBox txtRiskReason = (TextBox)item.FindControl("txtRiskReason");
                        TextBox txtStatusReason = (TextBox)item.FindControl("txtStatusReason");
                        DropDownList ddlOverallRisk = (DropDownList)item.FindControl("ddlOverallRisk");
                        DropDownList ddlOverallStatus = (DropDownList)item.FindControl("ddlOverallStatus");
                        HiddenField hdnProjectBriefID = (HiddenField)item.FindControl("hdnProjectBriefID");
                        HiddenField hdnDeleted = (HiddenField)item.FindControl("hdnDeleted");
                        Label lblBriefNo = (Label)item.FindControl("lblBriefNo");
                        CheckBox chkFinanceBriefInGateBrief = (CheckBox)item.FindControl("chkFinanceBriefInGateBrief");

                        HtmlTextArea txtProductFormatsT = (HtmlTextArea)item.FindControl("txtProductFormatsT");
                        HtmlTextArea txtOtherKeyInfoT = (HtmlTextArea)item.FindControl("txtOtherKeyInfoT");
                        HtmlTextArea txtMilestonesT = (HtmlTextArea)item.FindControl("txtMilestonesT");
                        HtmlTextArea txtImpactProjectHealthT = (HtmlTextArea)item.FindControl("txtImpactProjectHealthT");
                        HtmlTextArea txtTeamRecommendationT = (HtmlTextArea)item.FindControl("txtTeamRecommendationT");

                        StageGateGateItem gateItem = new StageGateGateItem();
                        gateItem.ID = Convert.ToInt32(hdnProjectBriefID.Value);
                        gateItem.ProductFormats = txtProductFormatsT.InnerHtml;
                        gateItem.RetailExecution = txtRetailExecution.Text;
                        gateItem.OtherKeyInfo = txtOtherKeyInfoT.InnerHtml;
                        gateItem.OverallRisk = ddlOverallRisk.SelectedItem.Text;
                        gateItem.OverallStatus = ddlOverallStatus.SelectedItem.Text;
                        gateItem.OverallRiskReason = txtRiskReason.Text;
                        gateItem.OverallStatusReason = txtStatusReason.Text;
                        gateItem.GateReadiness = txtGateReadiness.Text;

                        gateItem.Milestones = txtMilestonesT.InnerHtml;
                        gateItem.ImpactProjectHealth = txtImpactProjectHealthT.InnerHtml;
                        gateItem.TeamRecommendation = txtTeamRecommendationT.InnerHtml;
                        gateItem.BriefName = txtBriefName.Text;
                        gateItem.StageGateListItemId = StageGateListItemId;
                        gateItem.Gate = gate.ToString();
                        gateItem.ProjectNumber = ProjectNumber;
                        gateItem.BriefNo = Convert.ToInt32(lblBriefNo.Text);
                        gateItem.FinanceBriefInGateBrief = chkFinanceBriefInGateBrief.Checked ? "Yes" : "No";

                        if (hdnDeleted.Value == "true")
                        {
                            gateItem.Deleted = "Yes";
                        }
                        stageGateGeneralService.UpsertGateBriefItem(gateItem);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + ": SaveStageGateNecessaryDeliverablesItem: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateNecessaryDeliverables.ToString(), "SaveStageGateNecessaryDeliverablesItem");
            }
            try
            {
                StageGateCreateProjectItem stageItem = new StageGateCreateProjectItem();
                stageItem.RevisedShipDate = txtRevisedFirstShipDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtRevisedFirstShipDate.Text);
                stageItem.StageGateProjectListItemId = StageGateListItemId;
                stageGateGeneralService.updateRevisedFirstShip(stageItem);
                if (submitted)
                {
                    if (PanelPostLaunch.Visible)
                    {
                        if (ddlPostLaunch.SelectedItem.Text == "Yes")
                        {
                            stageItem.Stage = "Post Launch";
                            stageItem.PostLaunchActive = "Yes";
                        }
                        else
                        {
                            stageItem.Stage = "Complete";
                            stageItem.PostLaunchActive = ddlPostLaunch.SelectedItem.Text;
                        }

                        if (checkOnHoldChildProjects())
                        {
                            valid = false;
                            strErrors = "Parent Project cannot be marked as completed until all “On-Hold” projects are either marked as “Cancelled” or “Completed”. Please update these projects and then try marking the Parent Project as completed again";
                            ErrorSummary.AddError(strErrors, this.Page);
                        }
                        else
                        {
                            valid = false;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CompleteParentProject", "ShowDialogProjectCompletdMessage();", true);
                        }
                    }
                    else
                    {
                        stageItem.Stage = ddlProjectStage.SelectedItem.Text;
                        stageGateGeneralService.updateCurrentStage(stageItem);
                    }
                }
                else
                {
                    stageItem.Stage = ddlProjectStage.SelectedItem.Text;
                    stageGateGeneralService.updateCurrentStage(stageItem);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + ": SaveProjectItem: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateNecessaryDeliverables.ToString(), "SaveProjectItem");
            }
            ViewState["NecessaryDeliverables"] = null;
            ViewState["GateBriefs"] = null;
            ViewState["RevisedFirstShip"] = null;
            return valid;
        }
        protected void btnCompleteProject_Click(object sender, EventArgs e)
        {
            StageGateCreateProjectItem stageItem = new StageGateCreateProjectItem();
            stageItem.RevisedShipDate = txtRevisedFirstShipDate.Text == "" ? DateTime.MinValue : Convert.ToDateTime(txtRevisedFirstShipDate.Text);
            stageItem.StageGateProjectListItemId = StageGateListItemId;

            if (ddlPostLaunch.SelectedItem.Text == "Yes")
            {
                stageItem.Stage = "Post Launch";
                stageItem.PostLaunchActive = "Yes";
            }
            else
            {
                stageItem.Stage = "Complete";
                stageItem.PostLaunchActive = ddlPostLaunch.SelectedItem.Text;
            }

            stageGateGeneralService.updateStageforPostLaunch(stageItem);
            var stateGateItem = stageGateCreateProjectService.GetStageGateProjectItem(stageItem.StageGateProjectListItemId);
            //Send Parent Project Submitted Email
            if (stateGateItem.ProjectCompletedSent != "Yes")
            {
                if (notificationService.EmailParentWFStep("ParentProjectCompleted", stateGateItem))
                {
                    stateGateItem.ProjectCompletedSent = "Yes";
                    stageGateCreateProjectService.UpdateStageGateProjectCompletedEmailSent(stateGateItem);
                }
            }


            saveUserControls(true);
            //redoing JS for Postback

            if (GlobalStage == "Post Launch" && URLStage.ToLower() == GlobalConstants.PAGE_StageGateLaunchDeliverables.ToLower())
            {
                workflowService.StartSpecificSGSWorkflow(StageGateListItemId, "Post Launch Notifications Workflow");
                Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGatePostLaunchDeliverables, ProjectNumber), false);
            }
            else
            {
                Page.Response.Redirect(Utilities.RedirectPageForm(GlobalConstants.PAGE_StageGateProjectPanel, ProjectNumber), false);
            }
        }
        #endregion
        protected void btnAddBrief_Click(object sender, EventArgs e)
        {
            try
            {
                StageGateGateItem gateItem = new StageGateGateItem();
                gateItem.StageGateListItemId = StageGateListItemId;
                gateItem.Gate = gate.ToString();
                gateItem.ProjectNumber = ProjectNumber;
                gateBriefCount = Convert.ToInt32(hdnGateBriefCount.Value) + 1;
                gateItem.BriefNo = gateBriefCount;
                int newId = stageGateGeneralService.UpsertGateBriefItem(gateItem);
                ConstructFormData(false);
                saveUserControls(false);
                rebindRepeater();
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnLoadBrief_Click(object sender, EventArgs e)
        {
            try
            {
                ConstructFormData(false);
                saveUserControls(false);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "openLoadPreviousBrief(" + gate + ", " + StageGateListItemId + ", " + hdnGateBriefCount.Value + ",\"" + URLStage + "\")", true);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateNecessaryDeliverables.ToString() + ": btnUploadDocs_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateNecessaryDeliverables.ToString(), "btnUploadDocs_Click");
            }
        }
        private void rebindRepeater()
        {
            LoadUserControls();
            LoadFormData();
            hdnStageGateProjectListItemId.Value = StageGateListItemId.ToString();
            hdnGlobalStage.Value = GlobalStage;
            List<StageGateNecessaryDeliverablesItem> necDelivPct = new List<StageGateNecessaryDeliverablesItem>();

            foreach (RepeaterItem parentItem in rptParentDeliverables.Items)
            {
                if (parentItem.ItemType == ListItemType.Item || parentItem.ItemType == ListItemType.AlternatingItem)
                {
                    int totalDelivs = 0;
                    int delivsCompleted = 0;
                    Repeater rptDeliverables = ((Repeater)parentItem.FindControl("rptDeliverables"));
                    foreach (RepeaterItem item in rptDeliverables.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            string status = ((DropDownList)item.FindControl("ddlStatus")).SelectedItem.Text;
                            string applicable = ((DropDownList)item.FindControl("ddlApplicable")).SelectedItem.Text;
                            if (applicable == "Yes")
                            {
                                totalDelivs++;
                                if (status == "Complete")
                                {
                                    delivsCompleted++;
                                }
                            }
                            HiddenField hdnDeletedStatus = (HiddenField)item.FindControl("hdnDeletedStatus");
                            if (hdnDeletedStatus.Value == "true")
                            {
                                item.Visible = false;
                            }
                        }
                    }
                    int stagePct = 0;
                    if (totalDelivs > 0)
                    {
                        stagePct = (int)Math.Round((double)(100 * delivsCompleted) / totalDelivs, MidpointRounding.AwayFromZero);
                    }

                    HiddenField hdnDeliverablesTotalApplicable = (HiddenField)parentItem.FindControl("hdnDeliverablesTotalApplicable");
                    HiddenField hdnDeliverablesTotalCompleted = (HiddenField)parentItem.FindControl("hdnDeliverablesTotalCompleted");

                    hdnDeliverablesTotalApplicable.Value = totalDelivs.ToString();
                    hdnDeliverablesTotalCompleted.Value = delivsCompleted.ToString();

                    Label totalApplicable = (Label)rptDeliverables.Controls[rptDeliverables.Controls.Count - 1].Controls[0].FindControl("lblTotalApplicable");
                    Label totalCompleted = (Label)rptDeliverables.Controls[rptDeliverables.Controls.Count - 1].Controls[0].FindControl("lblTotalCompleted");

                    totalApplicable.Text = totalDelivs.ToString();
                    totalCompleted.Text = delivsCompleted.ToString();

                    //HtmlGenericControl lblNextStage = (HtmlGenericControl)parentItem.FindControl("lblNextStage");
                    //lblNextStage.InnerText = nextStageText;

                    HiddenField hdnStagePct = (HiddenField)parentItem.FindControl("hdnStagePct");
                    hdnStagePct.Value = stagePct.ToString();
                    HtmlGenericControl pchGatePct = (HtmlGenericControl)parentItem.FindControl("pchGatePct");
                    pchGatePct.InnerText = stagePct.ToString();

                }
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "script", "<script type='text/javascript'>SGSPostbackFuntions();</script>", false);
        }
        private bool checkOnHoldChildProjects()
        {
            return stageGateCreateProjectService.CheckOnHoldChildProjects(StageGateListItemId);
        }
    }
}
