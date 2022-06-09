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
using System.Web.UI;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace Ferrara.Compass.WebParts.StageGateCreateProjectForm
{
    [ToolboxItemAttribute(false)]
    public partial class StageGateCreateProjectForm : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        #region Member Variables
        private IStageGateCreateProjectService stageGateCreateProjectService;
        private IUtilityService utilityService;
        private IUserManagementService userManagementService;
        private IExceptionService exceptionService;
        private IConfigurationManagementService configurationService;
        private INotificationService notificationService;
        private int iItemId = 0;
        private bool InvalidPeopleEditor = false;
        private string webUrl;
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
        private string TestProject
        {
            get
            {
                if (HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRINGVALUE_TestProject] != null)
                    return HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRINGVALUE_TestProject];
                return string.Empty;
            }
        }
        #endregion
        #region Constructor
        public StageGateCreateProjectForm()
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
            notificationService = DependencyResolution.DependencyMapper.Container.Resolve<INotificationService>();
        }
        #endregion
        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {

            this.Page.Form.Enctype = "multipart/form-data";
            webUrl = SPContext.Current.Web.Url;


            if (!Page.IsPostBack)
            {
                Utilities.BindDropDownItems(this.ddlLineOfBusiness, GlobalConstants.LIST_ProductHierarchyLevel1Lookup, webUrl);
                Utilities.BindDropDownItems(this.ddlProjectType, GlobalConstants.LIST_StageGateProjectTypesLookup, webUrl);
                Utilities.BindDropDownItems(this.ddlStage, GlobalConstants.LIST_StageLookup, webUrl);
                //Utilities.BindDropDownItems(this.ddlBrand, GlobalConstants.LIST_MaterialGroup1Lookup, webUrl);
                Utilities.BindDropDownItems(ddlProjectTypeSubCategory, GlobalConstants.LIST_ProjectTypesSubCategoryLookup, webUrl);
                Utilities.BindDropDownItems(ddlBusinessFunction, GlobalConstants.LIST_BusinessFunctionsLookup, webUrl);

                //Utilities.AddItemToDropDown(this.ddlBrand, "Multiple", "Multiple", true);
                Utilities.AddItemToDropDown(this.ddlProjectTypeSubCategory, "NA", "4", true);
                Utilities.AddItemToDropDown(this.ddlProjectTypeSubCategory, "Multiple", "5", true);

                this.divAccessDenied.Visible = false;
                this.divAccessRequest.Visible = false;

                // Check for a valid project number
                if (!CheckProjectNumber())
                {
                    return;
                }

                if (iItemId > 0)
                {
                    LoadFormData();
                    LoadProjectBriefAttachments();
                    LoadOtherAttachments();
                    this.btnUploadProjectBrief.Enabled = true;
                    this.btnUploadOtherAttachments.Enabled = true;
                    this.divUploadDocumetsNote.Visible = false;
                }
                else
                {
                    LoadBlankProjectTeamber();
                    BindGroupMembersToDropDowns();
                    this.btnUploadProjectBrief.Enabled = false;
                    this.btnUploadOtherAttachments.Enabled = false;
                    this.divUploadDocumetsNote.Visible = true;
                    Utilities.SetDropDownValue(GlobalConstants.StageLookup_Design, this.ddlStage, this.Page);
                }
            }
            else
            {
                iItemId = Convert.ToInt32(hdnCompassListItemId.Value);
            }

            if (string.IsNullOrWhiteSpace(ProjectNumber) || hdnStage.Value == GlobalConstants.WORKFLOWPHASE_OnHold)
            {
                btnSubmit.Enabled = false;
            }


            this.lblPageTitle.Text = "Project Information Form";
            CallUpdatePeopleEditorScriptFunction();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "BusinessFunctionChanged", "BusinessFunctionChanged();", true);
        }
        #endregion
        #region Button Methods
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateDates())
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateFailedOnSave", "setFocusError();", true);
                    return;
                }
                InvalidPeopleEditor = false;
                StageGateCreateProjectItem item = ConstructFormData(false);

                if (InvalidPeopleEditor)
                {
                    if (Page.Validators.Count < 4)
                    {
                        string strErrors = "Project Information could not be saved.";
                        ErrorSummary.AddError(strErrors, this.Page);
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "btnSave_Click", "setFocusError();", true);
                    return;
                }

                iItemId = stageGateCreateProjectService.UpdateStageGateProjectItem(item, false);

                hdnCompassListItemId.Value = iItemId.ToString();
                hdnProjectNumber.Value = item.ProjectNumber;
                Page.Response.Redirect(Utilities.RedirectPageValueFirstSave(GlobalConstants.PAGE_StageGateCreateProject, item.ProjectNumber), false);
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.BOMSetupPE.ToString() + ": btnSave_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.BOMSetupPE.ToString(), "btnSave_Click");
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var ValidForm = ValidateForm();
                var ValidProjectBrief = ValidateProjectBrief();
                var ValidDates = ValidateDates();

                if (!ValidForm || !ValidProjectBrief || !ValidDates)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateFailed", "setFocusError();", true);
                    return;
                }

                InvalidPeopleEditor = false;
                StageGateCreateProjectItem item = ConstructFormData(true);

                if (InvalidPeopleEditor)
                {
                    if (Page.Validators.Count < 4)
                    {
                        string strErrors = "Project Information could not be saved.";
                        ErrorSummary.AddError(strErrors, this.Page);
                    }

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "InvalidPeopleEditor", "setFocusError();", true);
                    return;
                }

                bool Submit = true;
                if (hdnProjectAlreadySubmitted.Value == "Yes" && hdnProjectAlreadySubmittedOK.Value != "Yes")
                {
                    Submit = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ResubmitScript", "DialogStageGateProjectResubmitMessage();", true);
                }

                if (Submit)
                {
                    hdnProjectAlreadySubmittedOK.Value = "No";
                    iItemId = stageGateCreateProjectService.UpdateStageGateProjectItem(item, true);
                    hdnCompassListItemId.Value = iItemId.ToString();
                    hdnProjectNumber.Value = item.ProjectNumber;
                    var stateGateItem = stageGateCreateProjectService.GetStageGateProjectItem(iItemId);
                    //Send Parent Project Submitted Email
                    if (stateGateItem.ProjectSubmittedSent != "Yes")
                    {
                        if (notificationService.EmailParentWFStep("ParentProjectSubmitted", stateGateItem))
                        {
                            stateGateItem.ProjectSubmittedSent = "Yes";
                            stageGateCreateProjectService.UpdateStageGateProjectSubmittedEmailSent(stateGateItem);
                        }
                    }
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ProjectCreated", "DialogStageGateProjectCreatedMessage();", true);
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": btnSubmit_Click: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "btnSubmit_Click");
            }
        }

        protected void btnRedirect_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
        }
        protected void btnReloadAttachment_Click(object sender, EventArgs e)
        {
            LoadProjectBriefAttachments();
            LoadOtherAttachments();
        }
        protected void lbTaskDashboard_Click(object sender, EventArgs e)
        {
            // Redirect to Home page after successfull Submit                        
            Page.Response.Redirect(Utilities.RedirecttoHomePage(), false);
        }
        #endregion
        #region Data Transfer Methods
        private void LoadFormData()
        {
            try
            {
                var stageGateItem = stageGateCreateProjectService.GetStageGateProjectItem(iItemId);
                if (stageGateItem != null)
                {
                    hdnCompassListItemId.Value = iItemId.ToString();
                    hdnStage.Value = stageGateItem.Stage;

                    if (stageGateItem.FormSubmittedDate != null && stageGateItem.FormSubmittedDate != DateTime.MinValue)
                    {
                        hdnProjectAlreadySubmitted.Value = "Yes";
                        hdnProjectAlreadySubmittedDate.Value = Utilities.GetDateForDisplay(stageGateItem.FormSubmittedDate);
                    }

                    //Project Information
                    #region Project Information
                    this.txtProjectName.Text = stageGateItem.ProjectName;
                    if ((stageGateItem.Gate0ApprovedDate != null) && (stageGateItem.Gate0ApprovedDate != DateTime.MinValue))
                    {
                        this.txtGate0ApprovedDate.Text = Utilities.GetDateForDisplay(stageGateItem.Gate0ApprovedDate);
                    }
                    Utilities.SetDropDownValue(stageGateItem.LineOfBisiness, this.ddlLineOfBusiness, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.ProjectTier, this.ddlProjectTier, this.Page);
                    this.txtNumberofNoveltySKUs.Text = stageGateItem.NumberofNoveltySKUs;
                    if ((stageGateItem.DesiredShipDate != null) && (stageGateItem.DesiredShipDate != DateTime.MinValue))
                    {
                        this.txtDesiredShipDate.Text = Utilities.GetDateForDisplay(Convert.ToDateTime(stageGateItem.DesiredShipDate));
                    }
                    hdnRevisedFirstShipDate.Value = Convert.ToString(stageGateItem.RevisedShipDate);
                    ddlProductHierarchyLevel1_SelectedIndexChanged(null, null);
                    Utilities.SetDropDownValue(stageGateItem.PHL2, this.ddlProductHierarchyLevel2, this.Page);
                    ddlProductHierarchyLevel2_SelectedIndexChanged(null, null);
                    Utilities.SetDropDownValue(stageGateItem.Brand, this.ddlBrand, this.Page);

                    this.txtSKU.Text = stageGateItem.SKUs;
                    Utilities.SetDropDownValue(stageGateItem.ProjectType, this.ddlProjectType, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.ProjectTypeSubCategory, this.ddlProjectTypeSubCategory, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.BusinessFunction, this.ddlBusinessFunction, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.NewFinishedGood, this.ddlNewFinishedGood, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.NewBaseFormula, this.ddlNewBaseFormula, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.NewShape, this.ddlNewShape, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.NewPackType, this.ddlNewPackType, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.NewNetWeight, this.ddlNewNetWeight, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.NewGraphics, this.ddlNewGraphics, this.Page);
                    Utilities.SetDropDownValue(stageGateItem.NewFlavorColor, this.ddlNewFlavorColor, this.Page);
                    this.txtProjectConceptOverview.Text = stageGateItem.ProjectConceptOverview;
                    this.txtBusinessFunctionOther.Text = stageGateItem.BusinessFunctionOther;
                    #endregion
                    if (stageGateItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                    {
                        ListItem CNMItem = ddlProjectTypeSubCategory.Items.FindByText("Complex Network Move");
                        this.ddlProjectTypeSubCategory.Items.Remove(CNMItem);
                    }
                    #region Stage
                    string Stage = stageGateItem.Stage;
                    if (string.IsNullOrEmpty(stageGateItem.Stage))
                    {
                        if (stageGateItem.ProjectType == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
                        {
                            Stage = GlobalConstants.StageLookup_Industrialize;
                        }
                        else if (stageGateItem.ProjectType == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
                        {
                            if (Stage == GlobalConstants.StageLookup_Design || Stage == GlobalConstants.StageLookup_Develop)
                            {
                                Stage = GlobalConstants.StageLookup_Industrialize;
                            }
                        }
                        else
                        {
                            Stage = GlobalConstants.StageLookup_Design;
                        }
                    }

                    if (Stage == "Complete" || Stage == "Cancelled")
                    {
                        Utilities.AddItemToDropDown(this.ddlStage, Stage, Stage, true);
                    }

                    Utilities.SetDropDownValue(Stage, this.ddlStage, this.Page);

                    #endregion
                    #region Project Team

                    BindGroupMembersToDropDowns();

                    //ProjectLeader
                    var ProjectLeaders = Utilities.SetPeoplePickerValue(stageGateItem.ProjectLeader, SPContext.Current.Web);
                    LoadProjectTeamMembers(ProjectLeaders, stageGateItem.ProjectLeaderName, "rptProjectLeaders", divProjectLeaders);

                    //ProjectManager
                    var ProjectManager = Utilities.SetPeoplePickerValue(stageGateItem.ProjectManager, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlProjectManagerMembers, ProjectManager, stageGateItem.ProjectManagerName, rptProjectManagers);

                    //SeniorProjectManager
                    var SeniorProjectManager = Utilities.SetPeoplePickerValue(stageGateItem.SeniorProjectManager, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlSrProjectManagerMembers, SeniorProjectManager, stageGateItem.SeniorProjectManagerName, rptSrProjectManagers);

                    //Marketing
                    var Marketing = Utilities.SetPeoplePickerValue(stageGateItem.Marketing, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlMarketingMembers, Marketing, stageGateItem.MarketingName, rptMarketingMembers);

                    //InTech (formal RnD)
                    var InTech = Utilities.SetPeoplePickerValue(stageGateItem.InTech, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlInTechMembers, InTech, stageGateItem.InTechName, rptInTechMembers);

                    //QAInnovation
                    var QAInnovation = Utilities.SetPeoplePickerValue(stageGateItem.QAInnovation, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlQualityInnovationMembers, QAInnovation, stageGateItem.QAInnovationName, rptQualityInnovationMembers);

                    //InTechRegulatory (It was RegulatoryRnD)
                    var InTechRegulatory = Utilities.SetPeoplePickerValue(stageGateItem.InTechRegulatory, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlInTechRegulatoryMembers, InTechRegulatory, stageGateItem.InTechRegulatoryName, rptInTechRegulatoryMembers);

                    //RegulatoryQA
                    var RegulatoryQA = Utilities.SetPeoplePickerValue(stageGateItem.RegulatoryQA, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlRegulatoryQAMembers, RegulatoryQA, stageGateItem.RegulatoryQAName, rptRegulatoryQAMembers);

                    //PackagingEngineering
                    var PackagingEngineering = Utilities.SetPeoplePickerValue(stageGateItem.PackagingEngineering, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlPackagingEngineeringMembers, PackagingEngineering, stageGateItem.PackagingEngineeringName, rptPackagingEngineeringMembers);

                    //SupplyChain
                    var SupplyChain = Utilities.SetPeoplePickerValue(stageGateItem.SupplyChain, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlSupplyChainMembers, SupplyChain, stageGateItem.SupplyChainName, rptSupplyChainMembers);

                    //Finance
                    var Finance = Utilities.SetPeoplePickerValue(stageGateItem.Finance, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlFinanceMembers, Finance, stageGateItem.FinanceName, rptFinanceMembers);

                    //Sales
                    var Sales = Utilities.SetPeoplePickerValue(stageGateItem.Sales, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlSalesMembers, Sales, stageGateItem.SalesName, rptSalesMembers);

                    //Manufacturing
                    var Manufacturing = Utilities.SetPeoplePickerValue(stageGateItem.Manufacturing, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlManufacturingMembers, Manufacturing, stageGateItem.ManufacturingName, rptManufacturingMembers);

                    //External Mfg - Procurement
                    var ExtMfgProcurement = Utilities.SetPeoplePickerValue(stageGateItem.ExtMfgProcurement, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlExternalMfgProcurementMembers, ExtMfgProcurement, stageGateItem.ExtMfgProcurementName, rptExternalMfgProcurementMembers);

                    //Packaging Procurement
                    var PackagingProcurement = Utilities.SetPeoplePickerValue(stageGateItem.PackagingProcurement, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlPackagingProcurementMembers, PackagingProcurement, stageGateItem.PackagingProcurementName, rptPackagingProcurementMembers);

                    //Life Cycle Management
                    var LifeCycleManagement = Utilities.SetPeoplePickerValue(stageGateItem.LifeCycleManagement, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlLifeCycleManagementMembers, LifeCycleManagement, stageGateItem.LifeCycleManagementName, rptLifeCycleManagementMembers);

                    //Legal
                    var Legal = Utilities.SetPeoplePickerValue(stageGateItem.Legal, SPContext.Current.Web);
                    LoadProjectTeamMembers_New(this.ddlLegalMembers, Legal, stageGateItem.LegalName, rptLegalMembers);

                    //Other Members
                    var OtherMember = Utilities.SetPeoplePickerValue(stageGateItem.OtherMember, SPContext.Current.Web);
                    LoadProjectTeamMembers(OtherMember, stageGateItem.OtherMemberName, "rptOtherTeamMembers", divOtherTeamMembers);
                    #endregion

                    if (CheckProjectNumberPresent())
                    {
                        lblSaved.Text = "Changes Saved: " + stageGateItem.ModifiedDate;

                        if ((stageGateItem.FormSubmittedDate != null) && (stageGateItem.FormSubmittedDate != DateTime.MinValue))
                        {
                            lblSubmit.Text = "Changes Submitted: " + stageGateItem.FormSubmittedDate;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": LoadFormData: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "LoadFormData");
            }
        }
        private StageGateCreateProjectItem ConstructFormData(bool Submitting)
        {
            var stageGateItem = new StageGateCreateProjectItem();
            string strErrors = string.Empty;
            int RepeaterCount = 0;

            try
            {
                if (string.IsNullOrEmpty(ProjectNumber))
                {
                    stageGateItem.ProjectNumber = GetNextProjectNumber();
                }
                else
                {
                    stageGateItem.ProjectNumber = ProjectNumber;
                }

                stageGateItem.StageGateProjectListItemId = iItemId;

                #region Project Information
                stageGateItem.ProjectName = this.txtProjectName.Text;
                if (!string.IsNullOrWhiteSpace(this.txtGate0ApprovedDate.Text))
                {
                    stageGateItem.Gate0ApprovedDate = Convert.ToDateTime(this.txtGate0ApprovedDate.Text.Trim());
                }
                stageGateItem.LineOfBisiness = this.ddlLineOfBusiness.SelectedItem.Text;
                stageGateItem.ProjectTier = this.ddlProjectTier.SelectedItem.Text;
                stageGateItem.NumberofNoveltySKUs = this.txtNumberofNoveltySKUs.Text;
                if (!string.IsNullOrWhiteSpace(this.txtDesiredShipDate.Text))
                {
                    stageGateItem.DesiredShipDate = Convert.ToDateTime(this.txtDesiredShipDate.Text);

                    DateTime dDate;
                    if (DateTime.TryParse(hdnRevisedFirstShipDate.Value, out dDate))
                    {
                        stageGateItem.RevisedShipDate = dDate;
                    }
                    else
                    {
                        stageGateItem.RevisedShipDate = stageGateItem.DesiredShipDate;
                    }
                }
                stageGateItem.Brand = this.ddlBrand.SelectedItem.Text;
                stageGateItem.PHL2 = this.ddlProductHierarchyLevel2.SelectedItem.Text;
                stageGateItem.SKUs = this.txtSKU.Text;
                stageGateItem.ProjectType = this.ddlProjectType.SelectedItem.Text;
                stageGateItem.ProjectTypeSubCategory = this.ddlProjectTypeSubCategory.SelectedItem.Text;
                stageGateItem.BusinessFunction = this.ddlBusinessFunction.SelectedItem.Text;
                stageGateItem.BusinessFunctionOther = this.txtBusinessFunctionOther.Text;
                if (ddlProjectType.SelectedItem.Text == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly || ddlProjectType.SelectedItem.Text == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
                {
                    stageGateItem.NewFinishedGood = this.hdnddlNewFinishedGood.Value;
                    stageGateItem.NewBaseFormula = this.hdnddlNewBaseFormula.Value;
                    stageGateItem.NewShape = this.hdnddlNewShape.Value;
                    stageGateItem.NewPackType = this.hdnddlNewPackType.Value;
                    stageGateItem.NewNetWeight = this.hdnddlNewNetWeight.Value;
                    stageGateItem.NewGraphics = this.hdnddlNewGraphics.Value;
                    stageGateItem.NewFlavorColor = this.hdnddlNewFlavorColor.Value;
                }
                else
                {
                    stageGateItem.NewFinishedGood = this.ddlNewFinishedGood.SelectedItem.Text;
                }

                stageGateItem.NewBaseFormula = this.ddlNewBaseFormula.SelectedItem.Text.Trim();
                stageGateItem.NewShape = this.ddlNewShape.SelectedItem.Text.Trim();
                stageGateItem.NewPackType = this.ddlNewPackType.SelectedItem.Text.Trim();
                stageGateItem.NewNetWeight = this.ddlNewNetWeight.SelectedItem.Text.Trim();
                stageGateItem.NewGraphics = this.ddlNewGraphics.SelectedItem.Text.Trim();
                stageGateItem.NewFlavorColor = this.ddlNewFlavorColor.SelectedItem.Text.Trim();
                stageGateItem.ProjectConceptOverview = this.txtProjectConceptOverview.Text;
                #endregion
                //Stage
                stageGateItem.Stage = this.ddlStage.SelectedItem.Text;

                //Project Team
                #region Project Team                
                string allUsers = "";
                List<string> users = new List<string>();
                #region ProjectLeader
                Dictionary<string, string> ProjectLeader = ConstructMembers(ref RepeaterCount, "Project Leader", divProjectLeaders, "rptProjectLeaders", "peProjectLeaderMembers", "hdnDeletedStatusForProjectLeader", true, Submitting);
                stageGateItem.ProjectLeader = ProjectLeader["Member"];
                stageGateItem.ProjectLeaderName = ProjectLeader["MemberName"];
                users.AddRange(stageGateItem.ProjectLeader.Split(';').ToList());
                #endregion

                #region ProjectManager
                Dictionary<string, string> ProjectManagers = ConstructMembers_New(ref RepeaterCount, ddlProjectManagerMembers, "Project Manager", rptProjectManagers, "txtProjectManagerMembers", "hdnDeletedStatusForProjectManagerMembers", true, Submitting);
                stageGateItem.ProjectManager = ProjectManagers["Member"];
                stageGateItem.ProjectManagerName = ProjectManagers["MemberName"];
                users.AddRange(stageGateItem.ProjectManager.Split(';').ToList());
                #endregion

                #region SeniorProjectManager
                Dictionary<string, string> SrProjectManagers = ConstructMembers_New(ref RepeaterCount, ddlSrProjectManagerMembers, "Sr. Project Manager", rptSrProjectManagers, "txtSrProjectManagerMembers", "hdnDeletedStatusForSrProjectManagerMembers", true, Submitting);
                stageGateItem.SeniorProjectManager = SrProjectManagers["Member"];
                stageGateItem.SeniorProjectManagerName = SrProjectManagers["MemberName"];
                users.AddRange(stageGateItem.SeniorProjectManager.Split(';').ToList());
                #endregion

                #region Marketing
                Dictionary<string, string> MarketingMembers = ConstructMembers_New(ref RepeaterCount, ddlMarketingMembers, "Marketing", rptMarketingMembers, "txtMarketingMembers", "hdnDeletedStatusForMarketingMembers", true, Submitting);
                stageGateItem.Marketing = MarketingMembers["Member"];
                stageGateItem.MarketingName = MarketingMembers["MemberName"];
                users.AddRange(stageGateItem.Marketing.Split(';').ToList());
                #endregion

                #region ResearchAndDevelopment / InTech
                bool required = hdnRequiredInTechMembers.Value == "True" ? true : false;
                Dictionary<string, string> ResearchAndDevelopmentMembers = ConstructMembers_New(ref RepeaterCount, ddlInTechMembers, "InTech", rptInTechMembers, "txtInTechMembers", "hdnDeletedStatusForInTechMembers", required, Submitting);
                stageGateItem.InTech = ResearchAndDevelopmentMembers["Member"];
                stageGateItem.InTechName = ResearchAndDevelopmentMembers["MemberName"];
                users.AddRange(stageGateItem.InTech.Split(';').ToList());
                #endregion

                #region QA Innovation 
                required = hdnRequiredQualityInnovationMembers.Value == "True" ? true : false;
                Dictionary<string, string> QualityInnovationMembers = ConstructMembers_New(ref RepeaterCount, ddlQualityInnovationMembers, "Quality Innovation", rptQualityInnovationMembers, "txtQualityInnovationMembers", "hdnDeletedStatusForQualityInnovationMembers", required, Submitting);
                stageGateItem.QAInnovation = QualityInnovationMembers["Member"];
                stageGateItem.QAInnovationName = QualityInnovationMembers["MemberName"];
                users.AddRange(stageGateItem.QAInnovation.Split(';').ToList());
                #endregion

                #region Intech Regulatory
                required = hdnRequiredInTechRegulatoryMembers.Value == "True" ? true : false;
                Dictionary<string, string> InTechRegulatoryMembers = ConstructMembers_New(ref RepeaterCount, ddlInTechRegulatoryMembers, "InTech Regulatory", rptInTechRegulatoryMembers, "txtInTechRegulatoryMembers", "hdnDeletedStatusForInTechRegulatoryMembers", required, Submitting);
                stageGateItem.InTechRegulatory = InTechRegulatoryMembers["Member"];
                stageGateItem.InTechRegulatoryName = InTechRegulatoryMembers["MemberName"];
                users.AddRange(stageGateItem.InTechRegulatory.Split(';').ToList());
                #endregion

                #region RegulatoryQA
                required = hdnRequiredRegulatoryQAMembers.Value == "True" ? true : false;
                Dictionary<string, string> RegulatoryQAMembers = ConstructMembers_New(ref RepeaterCount, ddlRegulatoryQAMembers, "Regulatory QA", rptRegulatoryQAMembers, "txtRegulatoryQAMembers", "hdnDeletedStatusForRegulatoryQAMembers", required, Submitting);
                stageGateItem.RegulatoryQA = RegulatoryQAMembers["Member"];
                stageGateItem.RegulatoryQAName = RegulatoryQAMembers["MemberName"];
                users.AddRange(stageGateItem.RegulatoryQA.Split(';').ToList());
                #endregion

                #region Packaging Engineering
                Dictionary<string, string> PackagingEngineeringMembers = ConstructMembers_New(ref RepeaterCount, ddlPackagingEngineeringMembers, "Packaging Engineering", rptPackagingEngineeringMembers, "txtPackagingEngineeringMembers", "hdnDeletedStatusForPackagingEngineeringMembers", true, Submitting);
                stageGateItem.PackagingEngineering = PackagingEngineeringMembers["Member"];
                stageGateItem.PackagingEngineeringName = PackagingEngineeringMembers["MemberName"];
                users.AddRange(stageGateItem.PackagingEngineering.Split(';').ToList());
                #endregion

                #region Supply Chain
                Dictionary<string, string> SupplyChainMembers = ConstructMembers_New(ref RepeaterCount, ddlSupplyChainMembers, "Supply Chain", rptSupplyChainMembers, "txtSupplyChainMembers", "hdnDeletedStatusForSupplyChainMembers", true, Submitting);
                stageGateItem.SupplyChain = SupplyChainMembers["Member"];
                stageGateItem.SupplyChainName = SupplyChainMembers["MemberName"];
                users.AddRange(stageGateItem.SupplyChain.Split(';').ToList());
                #endregion

                #region Finance
                Dictionary<string, string> FinanceMembers = ConstructMembers_New(ref RepeaterCount, ddlFinanceMembers, "Finance", rptFinanceMembers, "txtFinanceMembers", "hdnDeletedStatusForFinanceMembers", true, Submitting);
                stageGateItem.Finance = FinanceMembers["Member"];
                stageGateItem.FinanceName = FinanceMembers["MemberName"];
                users.AddRange(stageGateItem.Finance.Split(';').ToList());
                #endregion

                #region Sales
                required = hdnRequiredSalesMembers.Value == "True" ? true : false;
                Dictionary<string, string> SalesMembers = ConstructMembers_New(ref RepeaterCount, ddlSalesMembers, "Sales", rptSalesMembers, "txtSalesMembers", "hdnDeletedStatusForSalesMembers", true, Submitting);
                stageGateItem.Sales = SalesMembers["Member"];
                stageGateItem.SalesName = SalesMembers["MemberName"];
                users.AddRange(stageGateItem.Sales.Split(';').ToList());
                #endregion

                #region Manufacturing
                required = hdnRequiredManufacturingMembers.Value == "True" ? true : false;
                Dictionary<string, string> ManufacturingMembers = ConstructMembers_New(ref RepeaterCount, ddlManufacturingMembers, "Manufacturing", rptManufacturingMembers, "txtManufacturingMembers", "hdnDeletedStatusForManufacturingMembers", required, Submitting);
                stageGateItem.Manufacturing = ManufacturingMembers["Member"];
                stageGateItem.ManufacturingName = ManufacturingMembers["MemberName"];
                users.AddRange(stageGateItem.Manufacturing.Split(';').ToList());
                #endregion

                #region External Mfg - Procurement
                required = hdnRequiredExternalMfgProcurementMembers.Value == "True" ? true : false;
                Dictionary<string, string> ExternalMfgProcurementMembers = ConstructMembers_New(ref RepeaterCount, ddlExternalMfgProcurementMembers, "External Mfg - Procurement", rptExternalMfgProcurementMembers, "txtExternalMfgProcurementMembers", "hdnDeletedStatusForExternalMfgProcurementMembers", required, Submitting);
                stageGateItem.ExtMfgProcurement = ExternalMfgProcurementMembers["Member"];
                stageGateItem.ExtMfgProcurementName = ExternalMfgProcurementMembers["MemberName"];
                users.AddRange(stageGateItem.ExtMfgProcurement.Split(';').ToList());
                #endregion

                #region Packaging Procurement
                required = hdnRequiredPackagingProcurementMembers.Value == "True" ? true : false;
                Dictionary<string, string> PackagingProcurementMembers = ConstructMembers_New(ref RepeaterCount, ddlPackagingProcurementMembers, "Packaging Procurement", rptPackagingProcurementMembers, "txtPackagingProcurementMembers", "hdnDeletedStatusForPackagingProcurementMembers", required, Submitting);
                stageGateItem.PackagingProcurement = PackagingProcurementMembers["Member"];
                stageGateItem.PackagingProcurementName = PackagingProcurementMembers["MemberName"];
                users.AddRange(stageGateItem.PackagingProcurement.Split(';').ToList());
                #endregion

                #region Life Cycle Management
                required = hdnRequiredLifeCycleManagementMembers.Value == "True" ? true : false;
                Dictionary<string, string> LifeCycleManagementMembers = ConstructMembers_New(ref RepeaterCount, ddlLifeCycleManagementMembers, "Life Cycle Management", rptLifeCycleManagementMembers, "txtLifeCycleManagementMembers", "hdnDeletedStatusForLifeCycleManagementMembers", required, Submitting);
                stageGateItem.LifeCycleManagement = LifeCycleManagementMembers["Member"];
                stageGateItem.LifeCycleManagementName = LifeCycleManagementMembers["MemberName"];
                users.AddRange(stageGateItem.LifeCycleManagement.Split(';').ToList());
                #endregion

                #region Legal
                required = hdnRequiredLegalMembers.Value == "True" ? true : false;
                Dictionary<string, string> LegalMembers = ConstructMembers_New(ref RepeaterCount, ddlLegalMembers, "Legal", rptLegalMembers, "txtLegalMembers", "hdnDeletedStatusForLegalMembers", required, Submitting);
                stageGateItem.Legal = LegalMembers["Member"];
                stageGateItem.LegalName = LegalMembers["MemberName"];
                users.AddRange(stageGateItem.Legal.Split(';').ToList());
                #endregion

                #region Team Members
                Dictionary<string, string> OtherTeamMembers = ConstructMembers(ref RepeaterCount, "Other Team", divOtherTeamMembers, "rptOtherTeamMembers", "peOtherTeamMembers", "hdnDeletedStatusForOtherTeamMembers", false, Submitting);
                stageGateItem.OtherMember = OtherTeamMembers["Member"];
                stageGateItem.OtherMemberName = OtherTeamMembers["MemberName"];
                users.AddRange(stageGateItem.OtherMember.Split(';').ToList());
                #endregion
                List<string> removeEmpties = allUsers.Split(',').ToList();
                List<string> finalUsers = new List<string>();
                foreach (string user in users)
                {
                    string userTrimmed = Regex.Replace(user, "[^0-9.]", "");
                    int id;
                    if (int.TryParse(userTrimmed, out id))
                    {
                        if (id > 0)
                        {
                            finalUsers.Add(id.ToString());
                        }
                    }
                }
                List<string> finalUsersDistinct = finalUsers.Select(x => x).Distinct().ToList<string>();
                stageGateItem.AllUsers = "," + String.Join(",", finalUsersDistinct) + ",";
                #endregion

                stageGateItem.LastUpdatedFormName = CompassForm.StageGateCreateProject.ToString();
            }
            catch (Exception ex)
            {
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": ConstructFormData: " + ex.Message);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "ConstructFormData");
                InvalidPeopleEditor = true;
            }

            return stageGateItem;
        }
        private Dictionary<string, string> ConstructMembers(ref int repeaterCount, string MemberName, HtmlGenericControl div, string RepeaterName, string PeopleEditorName, string HiddenStatusFieldName, bool Required, bool Submitting)
        {
            Repeater rptMembers = ((Repeater)div.FindControl(RepeaterName));
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            bool NA;
            string NAText;

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMember = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        try
                        {
                            if (peMember.Entities.Count <= 0 && Required && Submitting)
                            {
                                InvalidPeopleEditor = true;
                                string strErrors = "Please enter " + MemberName + " member.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                                ErrorSummary.AddError(strErrors, this.Page);
                            }

                            if (peMember.Entities.Count > 0)
                            {
                                CheckForNA(out NA, out NAText, peMember);

                                if (NA)
                                {
                                    MembersNames += NAText + ";";
                                }
                                else
                                {
                                    Members.AddRange(Utilities.GetPeopleFromPickerControl(peMember, SPContext.Current.Web));
                                    MembersNames += Utilities.GetNamesFromPickerControl(peMember, SPContext.Current.Web) + ";";
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            InvalidPeopleEditor = true;
                            string strErrors = "Invalid " + MemberName + " member was entered. Please re-enter. <a href ='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": " + strErrors);
                            exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "ConstructFormData", strErrors);
                        }
                        repeaterCount++;
                    }
                }
            }

            return new Dictionary<string, string>()
            {
                { "Member", Members.ToString() },
                { "MemberName",MembersNames }
            };
        }
        private Dictionary<string, string> ConstructMembers_New(ref int repeaterCount, DropDownList ddlMember, string MemberName, Repeater rptMembers, string txtBoxName, string HiddenStatusFieldName, bool Required, bool Submitting)
        {
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();

            if (ddlMember.SelectedItem.Value != "-1")
            {
                MembersNames += ddlMember.SelectedItem.Text + ";";
                if (!string.IsNullOrEmpty(ddlMember.SelectedItem.Value) && !CheckForNA(ddlMember.SelectedItem.Value))
                {
                    Members.AddRange(Utilities.GetPeopleFromPickerControl(ddlMember.SelectedItem.Value, SPContext.Current.Web));
                }
            }

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtMember = (TextBox)item.FindControl(txtBoxName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        try
                        {
                            MembersNames += txtMember.Text + ";";
                            TextBox txtBoxNameLoginName = (TextBox)item.FindControl(string.Concat(txtBoxName + "LoginName"));

                            if (!string.IsNullOrEmpty(txtBoxNameLoginName.Text) && !CheckForNA(txtBoxNameLoginName.Text))
                            {
                                Members.AddRange(Utilities.GetPeopleFromPickerControl(txtBoxNameLoginName.Text, SPContext.Current.Web));
                            }
                        }
                        catch (Exception ex)
                        {
                            InvalidPeopleEditor = true;
                            string strErrors = "Invalid " + MemberName + " member was selected. <a href ='javascript:GotoPeoplePickerStepAndFocus(&quot;" + repeaterCount.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": " + strErrors);
                            exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.StageGateCreateProject.ToString(), "ConstructMembers_New", strErrors);
                        }
                        repeaterCount++;
                    }
                }
            }

            return new Dictionary<string, string>()
            {
                { "Member", Members.ToString() },
                { "MemberName",MembersNames }
            };
        }
        private string LoadProjectTeamMembers(string Members, string MemberNames, string RepeaterName, HtmlGenericControl div)
        {
            List<string> listTeamMemberNames = new List<string>();
            List<int> NAList = new List<int>();

            if (!string.IsNullOrEmpty(MemberNames))
            {
                MemberNames = MemberNames.Remove(MemberNames.LastIndexOf(";"), 1);
                listTeamMemberNames = MemberNames.Split(';').ToList();

                for (int i = 0; i < listTeamMemberNames.Count; i++)
                {
                    if (CheckForNA(listTeamMemberNames[i])) NAList.Add(i);
                }
            }

            List<string> listTeamMembers = new List<string>();
            if (!string.IsNullOrEmpty(Members))
            {
                Members = Members.Remove(Members.LastIndexOf(","), 1);
                listTeamMembers = Members.Split(',').ToList();
            }

            for (int i = 0; i < NAList.Count; i++)
            {
                listTeamMembers.Insert(NAList[i], "NA");
            }

            if (listTeamMembers.Count == 0)
            {
                listTeamMembers.Add(string.Empty);
            }

            Repeater Repeater = (Repeater)div.FindControl(RepeaterName);
            Repeater.DataSource = listTeamMembers;
            Repeater.DataBind();
            return Members;
        }
        public class TeamMember
        {
            private string mMemberName;
            private string mMemberLoginName;
            public string MemberName { get { return mMemberName; } set { mMemberName = value; } }
            public string MemberLoginName { get { return mMemberLoginName; } set { mMemberLoginName = value; } }
        }
        private string LoadProjectTeamMembers_New(DropDownList ddlMember, string Members, string MemberNames, Repeater Repeater)
        {
            try
            {
                if (!string.IsNullOrEmpty(MemberNames))
                {
                    List<TeamMember> TeamMembers = new List<TeamMember>();
                    List<int> NAList = new List<int>();
                    List<string> listTeamMembers = new List<string>();
                    List<string> listTeamMemberNames = new List<string>();

                    if (MemberNames.LastIndexOf(";") != -1)
                    {
                        MemberNames = MemberNames.Remove(MemberNames.LastIndexOf(";"), 1);
                    }

                    if (!string.IsNullOrEmpty(Members))
                    {
                        Members = Members.Remove(Members.LastIndexOf(","), 1);
                    }

                    listTeamMemberNames = MemberNames.Split(';').ToList();
                    listTeamMembers = Members.Split(',').ToList();

                    for (int i = 0; i < listTeamMemberNames.Count; i++)
                    {
                        if (CheckForNA(listTeamMemberNames[i])) NAList.Add(i);
                    }

                    for (int i = 0; i < NAList.Count; i++)
                    {
                        listTeamMembers.Insert(NAList[i], "NA");
                    }

                    for (int i = 0; i < listTeamMemberNames.Count; i++)
                    {
                        TeamMembers.Add(new TeamMember() { MemberName = listTeamMemberNames[i], MemberLoginName = listTeamMembers[i] });
                    }
                    //ddlMember.SelectedItem.Text = TeamMembers.FirstOrDefault().MemberName;
                    Utilities.SetDropDownValue(TeamMembers.FirstOrDefault().MemberName, ddlMember, Page);
                    TeamMembers.RemoveAt(0);

                    Repeater.DataSource = TeamMembers;
                    Repeater.DataBind();
                }
            }
            catch (Exception ex)
            {
                string strErrors = string.Concat("Exception while loading project team member .", " - RepeaterName:", Repeater.ID, "- MemberNames:", MemberNames);
                LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": " + strErrors);
                exceptionService.Handle(LogCategory.CriticalError, ex, CompassForm.IPF.ToString(), "LoadProjectTeamMembers_New", strErrors);
            }
            return MemberNames;
        }
        private void LoadBlankProjectTeamber()
        {
            //ProjectLeader
            LoadProjectTeamMembers(string.Empty, string.Empty, "rptProjectLeaders", divProjectLeaders);

            //Other Members
            LoadProjectTeamMembers(string.Empty, string.Empty, "rptOtherTeamMembers", divOtherTeamMembers);
        }

        private void BindGroupMembersToDropDowns()
        {
            //ProjectManager
            Utilities.BindGroupMembersToDropDown(this.ddlProjectManagerMembers, GlobalConstants.GROUP_ProjectManagers, true);

            //SeniorProjectManager
            Utilities.BindGroupMembersToDropDown(this.ddlSrProjectManagerMembers, GlobalConstants.GROUP_SeniorProjectManager, true);

            //Marketing
            Utilities.BindGroupMembersToDropDown(this.ddlMarketingMembers, GlobalConstants.GROUP_Marketing, true);

            //InTech (formal RnD)
            Utilities.BindGroupMembersToDropDown(this.ddlInTechMembers, GlobalConstants.GROUP_InTech, true);

            //QAInnovation
            Utilities.BindGroupMembersToDropDown(this.ddlQualityInnovationMembers, GlobalConstants.GROUP_QualityInnovation, true);

            //InTechRegulatory - (Earlier it was RegulatoryRnD)
            Utilities.BindGroupMembersToDropDown(this.ddlInTechRegulatoryMembers, GlobalConstants.GROUP_InTechRegulatory, true);

            //RegulatoryQA
            Utilities.BindGroupMembersToDropDown(this.ddlRegulatoryQAMembers, GlobalConstants.GROUP_RegulatoryQA, true);

            //PackagingEngineering
            Utilities.BindGroupMembersToDropDown(this.ddlPackagingEngineeringMembers, GlobalConstants.GROUP_PackagingEngineer, true);

            //SupplyChain
            Utilities.BindGroupMembersToDropDown(this.ddlSupplyChainMembers, GlobalConstants.GROUP_SupplyChain, true);

            //Finance
            Utilities.BindGroupMembersToDropDown(this.ddlFinanceMembers, GlobalConstants.GROUP_Finance, true);

            //Sales
            Utilities.BindGroupMembersToDropDown(this.ddlSalesMembers, GlobalConstants.GROUP_Sales, true);

            //Manufacturing
            Utilities.BindGroupMembersToDropDown(this.ddlManufacturingMembers, GlobalConstants.GROUP_Manufacturing, true);

            //External Mfg - Procurement
            Utilities.BindGroupMembersToDropDown(this.ddlExternalMfgProcurementMembers, GlobalConstants.GROUP_ExternalManufacturing, true);

            //Packaging Procurement
            Utilities.BindGroupMembersToDropDown(this.ddlPackagingProcurementMembers, GlobalConstants.GROUP_ProcurementPackaging, true);

            //Life Cycle Management
            Utilities.BindGroupMembersToDropDown(this.ddlLifeCycleManagementMembers, GlobalConstants.GROUP_LifeCycleMngmt, true);

            //Legal
            Utilities.BindGroupMembersToDropDown(this.ddlLegalMembers, GlobalConstants.GROUP_Legal, true);
        }
        #endregion
        #region Drop-down methods
        protected void ddlProductHierarchyLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadProductHierarchyLevel2(this.ddlLineOfBusiness.SelectedItem.Text);

        }
        private void ReloadProductHierarchyLevel2(string productHierarhcyLevel1)
        {
            // Get the Product Hierarchy Level 1 Value
            string level2 = Utilities.GetLookupValue(GlobalConstants.LIST_ProductHierarchyLevel1Lookup, productHierarhcyLevel1, webUrl);

            if ((!string.IsNullOrEmpty(level2)) && (!string.Equals(level2, "Select...")))
            {
                Utilities.BindDropDownItemsByValue(ddlProductHierarchyLevel2, GlobalConstants.LIST_ProductHierarchyLevel2Lookup, level2, webUrl);
                this.ddlProductHierarchyLevel2.SelectedIndex = -1;
            }
            else
            {
                this.ddlProductHierarchyLevel2.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                this.ddlProductHierarchyLevel2.Items.Add(li);
            }
            this.ddlBrand.Items.Clear();
            ListItem li3 = new ListItem();
            li3.Text = "Select...";
            li3.Value = "-1";
            this.ddlBrand.Items.Add(li3);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SGSPostbackFuntions", "SGSPostbackFuntions();", true);

        }
        protected void ddlProductHierarchyLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadBrand(this.ddlProductHierarchyLevel2.SelectedItem.Text);

        }
        private void ReloadBrand(string productHierarhcyLevel2)
        {
            if ((!string.IsNullOrEmpty(productHierarhcyLevel2)) && (!string.Equals(productHierarhcyLevel2, "Select...")))
            {
                Utilities.BindDropDownItemsByValueAndColumn(ddlBrand, GlobalConstants.LIST_MaterialGroup1Lookup, "ParentPHL2", productHierarhcyLevel2, webUrl);
                ListItem li2 = new ListItem();
                li2.Text = "Multiple";
                li2.Value = "Multiple";
                this.ddlBrand.Items.Add(li2);
                this.ddlBrand.SelectedIndex = -1;
            }
            else
            {
                this.ddlBrand.Items.Clear();
                ListItem li = new ListItem();
                li.Text = "Select...";
                li.Value = "-1";
                this.ddlBrand.Items.Add(li);
                ListItem li2 = new ListItem();
                li2.Text = "Multiple";
                li2.Value = "Multiple";
                this.ddlBrand.Items.Add(li2);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SGSPostbackFuntions", "SGSPostbackFuntions();", true);
            }
        }
        protected void ddlProjectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProjectType.SelectedItem.Text == GlobalConstants.PROJECTTYPE_SimpleNetworkMove)
            {
                Utilities.SetDropDownValue(GlobalConstants.StageLookup_Industrialize, this.ddlStage, this.Page);
            }
            else if (ddlProjectType.SelectedItem.Text == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                if (ddlStage.SelectedItem.Text == GlobalConstants.StageLookup_Design || ddlStage.SelectedItem.Text == GlobalConstants.StageLookup_Develop)
                {
                    Utilities.SetDropDownValue(GlobalConstants.StageLookup_Industrialize, this.ddlStage, this.Page);
                }
            }
            else
            {
                Utilities.SetDropDownValue(GlobalConstants.StageLookup_Design, this.ddlStage, this.Page);
            }

            ListItem CNMItem = ddlProjectTypeSubCategory.Items.FindByText("Complex Network Move");
            if (ddlProjectType.SelectedItem.Text == GlobalConstants.PROJECTTYPE_GraphicsChangeOnly)
            {
                this.ddlProjectTypeSubCategory.Items.Remove(CNMItem);
            }
            else
            {
                if (!this.ddlProjectTypeSubCategory.Items.Contains(CNMItem))
                {
                    Utilities.AddItemToDropDown(this.ddlProjectTypeSubCategory, "Complex Network Move", "2", true);
                }
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "SGSPostbackFuntions", "SGSPostbackFuntions();", true);
        }
        #endregion
        #region Add Team members Button events
        protected void btnAddProjectLeader_Click(object sender, EventArgs e)
        {
            AddMembers(divProjectLeaders, "rptProjectLeaders", "peProjectLeaderMembers", "hdnDeletedStatusForProjectLeader");
        }
        protected void btnAddProjectManager_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptProjectManagers, "ProjectManagerMembers", ddlProjectManagerMembers);
        }
        protected void btnAddSrProjectManagers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptSrProjectManagers, "SrProjectManagerMembers", ddlSrProjectManagerMembers);
        }
        protected void btnAddMarketingMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptMarketingMembers, "MarketingMembers", ddlMarketingMembers);
        }
        protected void btnAddInTechMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptInTechMembers, "InTechMembers", ddlInTechMembers);
        }
        protected void btnAddQualityInnovationMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptQualityInnovationMembers, "QualityInnovationMembers", ddlQualityInnovationMembers);
        }
        protected void btnAddInTechRegulatoryMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptInTechRegulatoryMembers, "InTechRegulatoryMembers", ddlInTechRegulatoryMembers);
        }
        protected void btnAddRegulatoryQAMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptRegulatoryQAMembers, "RegulatoryQAMembers", ddlRegulatoryQAMembers);
        }
        protected void btnAddPackagingEngineeringMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptPackagingEngineeringMembers, "PackagingEngineeringMembers", ddlPackagingEngineeringMembers);
        }
        protected void btnAddSupplyChainMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptSupplyChainMembers, "SupplyChainMembers", ddlSupplyChainMembers);
        }
        protected void btnAddFinanceMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptFinanceMembers, "FinanceMembers", ddlFinanceMembers);
        }
        protected void btnAddSalesMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptSalesMembers, "SalesMembers", ddlSalesMembers);
        }
        protected void btnAddManufacturingMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptManufacturingMembers, "ManufacturingMembers", ddlManufacturingMembers);
        }
        protected void btnAddExternalMfgProcurementMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptExternalMfgProcurementMembers, "ExternalMfgProcurementMembers", ddlExternalMfgProcurementMembers);
        }
        protected void btnAddPackagingProcurementMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptPackagingProcurementMembers, "PackagingProcurementMembers", ddlPackagingProcurementMembers);
        }
        protected void btnAddLifeCycleManagementMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptLifeCycleManagementMembers, "LifeCycleManagementMembers", ddlLifeCycleManagementMembers);
        }
        protected void btnAddLegalMembers_Click(object sender, EventArgs e)
        {
            AddTeamMemberButtonClick(rptLegalMembers, "LegalMembers", ddlLegalMembers);
        }
        protected void btnAddOtherTeamMembers_Click(object sender, EventArgs e)
        {
            AddMembers(divOtherTeamMembers, "rptOtherTeamMembers", "peOtherTeamMembers", "hdnDeletedStatusForOtherTeamMembers");
        }
        #region Common - Add Team members method
        private void AddMembers(HtmlGenericControl div, string repeaterName, string PeopleEditorName, string hiddenStatusFieldName)
        {
            Repeater rptMembers = ((Repeater)div.FindControl(repeaterName));
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            bool NA;
            string NAText;
            List<int> NAList = new List<int>();
            Dictionary<int, string> BadNamesList = new Dictionary<int, string>();
            int Counter = 0;

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMembers = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField hiddenStatusField = (HiddenField)item.FindControl(hiddenStatusFieldName);

                    if (hiddenStatusField.Value != "true")
                    {
                        try
                        {
                            if (peMembers.Entities.Count > 0)
                            {
                                CheckForNA(out NA, out NAText, peMembers);

                                if (NA)
                                {
                                    NAList.Add(Counter);
                                }
                                else
                                {
                                    Members.AddRange(Utilities.GetPeopleFromPickerControl(peMembers, SPContext.Current.Web));
                                }
                            }
                            else
                            {
                                Members.Add(new SPFieldUserValue());
                            }
                        }
                        catch (Exception ex)
                        {
                            string BadText;
                            foreach (PickerEntity entity in peMembers.Entities)
                            {
                                BadText = entity.Key;
                                BadText = string.IsNullOrEmpty(BadText) ? entity.DisplayText : BadText;
                                BadNamesList.Add(Counter, BadText);
                            }
                        }
                    }
                }
                Counter++;
            }

            string users = "";
            List<string> listpeMembers = new List<string>();

            foreach (SPFieldUserValue Member in Members)
            {
                if (Member.User == null)
                {
                    users += ",";
                }
                else
                {
                    users += Member.User.ToString() + ",";
                }
            }

            if (string.IsNullOrEmpty(users))
            {
                listpeMembers.Add(string.Empty);
            }
            else
            {
                listpeMembers = users.Split(',').ToList();
            }

            foreach (var index in NAList)
            {
                listpeMembers.Insert(index, "NA");
            }

            foreach (var BadValue in BadNamesList)
            {
                listpeMembers.Insert(BadValue.Key, BadValue.Value);
            }
            rptMembers.DataSource = listpeMembers;
            rptMembers.DataBind();
        }
        private void AddMembers_New(Repeater rptMembers, string txtBoxName, string NewMember, string NewMemberLoginName, string hiddenStatusFieldName)
        {
            SPFieldUserValueCollection Members = new SPFieldUserValueCollection();
            List<int> NAList = new List<int>();
            Dictionary<int, string> BadNamesList = new Dictionary<int, string>();
            int Counter = 0;
            List<TeamMember> listMembers = new List<TeamMember>();
            listMembers.Add(new TeamMember() { MemberName = NewMember, MemberLoginName = NewMemberLoginName });

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtMember = (TextBox)item.FindControl(txtBoxName);
                    HiddenField hiddenStatusField = (HiddenField)item.FindControl(hiddenStatusFieldName);
                    TextBox txtMembersLoginName = (TextBox)item.FindControl(string.Concat(txtBoxName, "LoginName"));

                    if (hiddenStatusField.Value != "true")
                    {
                        try
                        {
                            if (CheckForNA(txtMember.Text))
                            {
                                listMembers.Add(new TeamMember() { MemberName = "NA", MemberLoginName = "NA" });
                            }
                            else
                            {
                                listMembers.Add(new TeamMember() { MemberName = txtMember.Text, MemberLoginName = txtMembersLoginName.Text });
                            }
                        }
                        catch (Exception exception)
                        {

                            ErrorSummary.AddError("Error occurred while adding new member: " + exception.Message, this.Page);
                            LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.IPF.ToString() + ": AddMembers_New: " + exception.Message);
                            exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.IPF.ToString(), "AddMembers_New");
                        }
                    }
                }
                Counter++;
            }

            rptMembers.DataSource = listMembers;
            rptMembers.DataBind();
        }
        private void AddTeamMemberButtonClick(Repeater repeaterName, string MemberName, DropDownList ddlName)
        {
            if (ddlName.SelectedItem.Value != "-1")
            {
                AddMembers_New(repeaterName, string.Concat("txt", MemberName), ddlName.SelectedItem.Text, ddlName.SelectedItem.Value, string.Concat("hdnDeletedStatusFor", MemberName));
                ddlName.SelectedIndex = -1;
                CallUpdatePeopleEditorScriptFunction();
            }
        }

        #endregion 
        #endregion
        #region RepeaterMethods for Attachments
        protected void rpProjectBriefAttachments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string fileName = e.CommandArgument.ToString();

                utilityService.DeleteAttachment(fileName);
                LoadProjectBriefAttachments();
            }
        }
        protected void rpOtherAttachments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string fileName = e.CommandArgument.ToString();

                utilityService.DeleteAttachment(fileName);
                LoadOtherAttachments();
            }
        }
        #endregion
        #region Repeater Methods for Project Teams
        protected void rptProjectLeaders_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peProjectLeaderMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }

            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptProjectManagers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peProjectManagerMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptSrProjectManagers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peSrProjectManagerMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptMarketingMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peMarketingMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptInTechMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peInTechMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptQualityInnovationMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peQualityInnovationMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptInTechRegulatoryMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peInTechRegulatoryMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptRegulatoryQAMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peRegulatoryQAMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptPackagingEngineeringMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("pePackagingEngineeringMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptSupplyChainMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peSupplyChainMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptFinanceMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peFinanceMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptSalesMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peSalesMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptManufacturingMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peManufacturingMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptOtherTeamMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peOtherTeamMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptExternalMfgProcurementMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peExternalMfgProcurementMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptPackagingProcurementMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("pePackagingProcurementMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptLifeCycleManagementMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peLifeCycleManagementMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        protected void rptLegalMembers_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                string TeamMember = (string)e.Item.DataItem;
                PeopleEditor peTeamMember = (PeopleEditor)e.Item.FindControl("peLegalMembers");
                peTeamMember.CommaSeparatedAccounts = TeamMember;
            }
            CallUpdatePeopleEditorScriptFunction();
        }
        #endregion       
        #region Validation Methods        
        private Boolean ValidateForm()
        {
            Boolean bValid = true;
            bool required;
            // People fields

            int RepeaterCount = 0;
            if (!ValidateMembers(ref RepeaterCount, "Project Leader", divProjectLeaders, "rptProjectLeaders", "peProjectLeaderMembers", "hdnDeletedStatusForProjectLeader")) bValid = false;

            if (!ValidateMembers(ref RepeaterCount, "Other Team", divOtherTeamMembers, "rptOtherTeamMembers", "peOtherTeamMembers", "hdnDeletedStatusForOtherTeamMembers", false)) bValid = false;

            return bValid;
        }
        private bool ValidateMembers(ref int Count, string MemberName, HtmlGenericControl div, string RepeaterName, string PeopleEditorName, string HiddenStatusFieldName, bool Required = true, bool JustNAValid = true)
        {
            bool bValid = true;
            Repeater rptMembers = ((Repeater)div.FindControl(RepeaterName));
            string MembersNames = string.Empty;
            SPFieldUserValueCollection Memebers = new SPFieldUserValueCollection();
            int NACount = 0;
            List<string> strNAErrors = new List<string>();

            foreach (RepeaterItem item in rptMembers.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    PeopleEditor peMember = (PeopleEditor)item.FindControl(PeopleEditorName);
                    HiddenField HiddenStatus = (HiddenField)item.FindControl(HiddenStatusFieldName);

                    if (HiddenStatus.Value != "true")
                    {
                        if (peMember.Entities.Count <= 0 && Required)
                        {
                            string strErrors = MemberName + " member must be set.<a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + Count.ToString() + "&quot;)'>  [Update]</a>";
                            ErrorSummary.AddError(strErrors, this.Page);
                            bValid = false;
                        }
                        else if (Required && peMember.Entities.Count == 1 && !JustNAValid)
                        {
                            bool NA;
                            string NAText;
                            CheckForNA(out NA, out NAText, peMember);
                            if (NA)
                            {
                                NACount++;
                                strNAErrors.Add("NA is not a valid entry for " + MemberName + ". Please add a valid member. <a href='javascript:GotoPeoplePickerStepAndFocus(&quot;" + Count.ToString() + "&quot;)'>  [Update]</a>");
                            }
                        }
                    }

                }
                Count++;
            }

            if (rptMembers.Items.Count > 0 && rptMembers.Items.Count == NACount)
            {
                foreach (var error in strNAErrors)
                {
                    ErrorSummary.AddError(error, this.Page);
                    bValid = false;
                }
            }
            return bValid;
        }
        private bool ValidateDates()
        {
            bool valid = true;
            DateTime dateValue;
            if (!(DateTime.TryParse(txtGate0ApprovedDate.Text, out dateValue)))
            {
                valid = false;
                string strErrors = " Please enter a valid Gate 0 Approved Date. <a href='javascript:setFocusElement(&quot;" + "txtGate0ApprovedDate" + "&quot;)'>[Update]</a>";
                ErrorSummary.AddError(strErrors, this.Page);
            }

            if (!(DateTime.TryParse(txtDesiredShipDate.Text, out dateValue)))
            {
                valid = false;
                string strErrors = " Please enter a valid Desired 1st Ship Date. <a href='javascript:setFocusElement(&quot;" + "txtDesiredShipDate" + "&quot;)'>[Update]</a>";
                ErrorSummary.AddError(strErrors, this.Page);
            }
            return valid;
        }
        private bool ValidateProjectBrief()
        {
            bool valid = true;

            if (hdnAddProjectBriefRequired.Value != "True")
                return valid;

            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_StageGateProjectBrief);

                if (files.Count == 0)
                {
                    valid = false;
                    string strErrors = " Please upload Project Brief. <a href='javascript:setFocusElement(&quot;" + "lblUploadProjectBrief" + "&quot;)'>[Update]</a>";
                    ErrorSummary.AddError(strErrors, this.Page);
                }
            }

            return valid;
        }
        #endregion
        #region Private Methods
        #region CheckProjectNumber
        private bool CheckProjectNumber()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                iItemId = Utilities.GetItemIdByProjectNumberFromStageGateProjectList(ProjectNumber);
            }

            // Store Id in Hidden field
            this.hdnProjectNumber.Value = ProjectNumber;
            this.hdnCompassListItemId.Value = iItemId.ToString();
            return true;
        }
        #endregion

        private void CallUpdatePeopleEditorScriptFunction()
        {
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "", "updatePeopleEditors();", true);
        }
        private static bool CheckProjectNumberPresent()
        {
            return !string.IsNullOrWhiteSpace(HttpContext.Current.Request.QueryString[GlobalConstants.QUERYSTRING_ProjectNo]);
        }
        private static bool CheckForNA(string stateGateItemName)
        {
            return (stateGateItemName.ToUpper() == "NA" || stateGateItemName.ToUpper() == "NOT APPLICABLE" || stateGateItemName.ToUpper() == "N/A");
        }
        public string SetProjectTitle(StageGateCreateProjectItem item)
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
        private void CheckForNA(out bool NA, out string NAText, PeopleEditor peopleEditor)
        {
            NA = false;
            NAText = string.Empty;
            foreach (PickerEntity entity in peopleEditor.Entities)
            {
                if (entity.Key.ToUpper() == "NA" || entity.Key.ToUpper() == "NOT APPLICABLE" || entity.Key.ToUpper() == "N/A")
                {
                    NAText = entity.Key.ToUpper();
                    NA = true;
                }
                if (entity.DisplayText.ToUpper().Trim() == "NA" || entity.DisplayText.ToUpper().Trim() == "NOT APPLICABLE" || entity.DisplayText.ToUpper().Trim() == "N/A")
                {
                    NAText = entity.DisplayText.ToUpper();
                    NA = true;
                }
            }
        }
        private string GetNextProjectNumber()
        {
            string projectNumber = string.Empty;
            string projectYear = DateTime.Now.Year.ToString();
            string value;

            SystemConfiguration configuration = configurationService.GetConfigurations();
            if (configuration.Configurations.TryGetValue("Projects-" + projectYear, out value))
            {
                projectNumber = projectYear + "-" + configurationService.GetConfigurationFromList("Projects-" + projectYear);
                // Update to next number
                try
                {
                    int nextNumber = Convert.ToInt32(configurationService.GetConfigurationFromList("Projects-" + projectYear));
                    nextNumber++;
                    configurationService.UpdateConfiguration("Projects-" + projectYear, nextNumber.ToString());
                }
                catch (Exception exception)
                {
                    ErrorSummary.AddError("Unable to Update Project Number", this.Page);
                    LoggingService.LogError(LoggingService.WebPartLoggingDiagnosticArea, CompassForm.StageGateCreateProject.ToString() + ": GetNextProjectNumber: " + exception.Message);
                    exceptionService.Handle(LogCategory.CriticalError, exception, CompassForm.StageGateCreateProject.ToString(), "GetNextProjectNumber");
                }
            }
            else
            {
                // Configuration not found, need to add for current year
                projectNumber = projectYear + "-1";
                configurationService.CreateConfiguration("Projects-" + projectYear, "2");
            }

            return projectNumber;
        }
        private void SetUsers(string User, PeopleEditor control)
        {
            var users = Utilities.SetPeoplePickerValue(User, SPContext.Current.Web);
            if (!string.IsNullOrEmpty(users))
            {
                control.CommaSeparatedAccounts = users.Remove(users.LastIndexOf(","), 1);
            }
        }
        private void SetMembers(string Member, string MemberName, PeopleEditor peMember)
        {
            if (CheckForNA(MemberName))
            {
                peMember.CommaSeparatedAccounts = MemberName;
            }
            else
            {
                SetUsers(Member, peMember);
            }
        }
        #region Load AttachmentMethods
        private void LoadOtherAttachments()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_StageGateOthers);

                if (files.Count > 0)
                {
                    rptOtherAttachments.Visible = true;
                    rptOtherAttachments.DataSource = files;
                    rptOtherAttachments.DataBind();
                }
                else
                {
                    rptOtherAttachments.Visible = false;
                    rptOtherAttachments.Visible = false;
                }
            }
        }
        private void LoadProjectBriefAttachments()
        {
            if (!string.IsNullOrWhiteSpace(ProjectNumber))
            {
                var files = utilityService.GetUploadedCompassFilesByDocType(ProjectNumber, GlobalConstants.DOCTYPE_StageGateProjectBrief);

                if (files.Count > 0)
                {
                    rptProjectBriefAttachments.Visible = true;
                    rptProjectBriefAttachments.DataSource = files;
                    rptProjectBriefAttachments.DataBind();
                }
                else
                {
                    rptProjectBriefAttachments.Visible = false;
                    rptProjectBriefAttachments.Visible = false;
                }
            }
        }
        #endregion
        #endregion
    }
}
